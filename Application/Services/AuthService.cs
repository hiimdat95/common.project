using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Utilities.Auths;
using Utilities.Common;
using Utilities.Constants;
using Utilities.Contracts;
using Utilities.Models.Settings;
using Utilities.Services;
using ViewModels.Auth;
using ViewModels.Users;

namespace Application.Services
{
    public class AuthService<TUser> : Service, IAuthService
        where TUser : AppUser
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IBaseRepository _repository;
        private readonly IDapperProvider _dapperProvider;
        private readonly IAuthValidator _authValidator;
        private readonly AuthConfig _authConfig;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthService(IJwtHandler jwtHandler
            , IBaseRepository repository
            , IAuthValidator authValidator
            , IOptions<AuthConfig> authConfig
            , UserManager<AppUser> userManager
            , IMapper mapper
            , IUnitOfWork unitOfWork
            , IConfiguration config
            , IEmailService emailService
            , IDapperProvider dapperProvider
            , ICurrentPrincipal currentPrincipal
            , IHttpContextAccessor httpContextAccessor) : base(currentPrincipal, httpContextAccessor)
        {
            _jwtHandler = jwtHandler;
            _repository = repository;
            _authValidator = authValidator;
            _authConfig = authConfig.Value;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _config = config;
            _emailService = emailService;
            _dapperProvider = dapperProvider;
        }

        public async Task<ServiceResponse> AuthenticateAsync(AuthRequest request)
        {
            var iPageIndex = 1;
            var iPageSize = 1;
            object[] param = new object[]
             {
                iPageIndex,iPageSize
             };
            var x = await _dapperProvider.ExecuteQueryMultipleAsync<UserInfo>(@"    SET NOCOUNT ON;

	                SELECT  ROW_NUMBER() OVER(ORDER BY au.CreatedAt DESC) AS Stt
                           , au.Id
		                   , au.UserName
		                   , au.NormalizedUserName
		                   , au.Email
		                   , au.NormalizedEmail

                    INTO #Temp
                    FROM AppUsers au

                    SELECT * FROM #Temp ORDER BY Stt OFFSET ( @iPageIndex - 1) * @iPageSize ROWS FETCH NEXT @iPageSize ROWS ONLY;

                     SELECT    ROW_NUMBER() OVER(ORDER BY au.CreatedAt DESC) AS Stt
                           , au.Id
		                   , au.UserName
		                   , au.NormalizedUserName
		                   , au.Email
		                   , au.NormalizedEmail
                    FROM AppUsers au

                    SELECT
                            TOP 1 au.Id
		                   , au.UserName
		                   , au.NormalizedUserName
		                   , au.Email
		                   , au.NormalizedEmail FROM AppUsers au

                    SELECT COUNT(1) FROM AppUsers
                    DROP TABLE #Temp", CommandType.Text, param);
            //var strings = Newtonsoft.Json.JsonConvert.SerializeObject(x.ListUser1);
            List<UserViewModel> lstUser = JsonConvert.DeserializeObject<List<UserViewModel>>(JsonConvert.SerializeObject(x.ListUser1));
            var user = await _repository.FistOrDefaultAsync<AppUser>(u => u.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("login_failure", "Invalid username or password.");
            }
            //validate against ldap
            if (bool.Parse(_config.GetSection(SystemConstants.LdapInformation.ValidateLDAP).Value))
            {
                var authenticated = _authValidator.ValidateUserLive(request.UserName, request.Password, _authConfig.LdapAddress1, _authConfig.LdapAddress2);
                if (!authenticated)
                {
                    return BadRequest("login_failure", "Invalid username or password.");
                }
            }
            else if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest("login_failure", "Invalid username or password.");
            }
            user.DeactiveActiveRefreshTokens();
            var refreshToken = _jwtHandler.GenerateRefreshToken();
            user.AddRefreshToken(refreshToken, request.RemoteIpAddress);
            _repository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                AccessToken = _jwtHandler.GenerateAccessToken(user.Id, user.UserName, null),
                RefreshToken = refreshToken
            });
        }

        public async Task<ServiceResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var failedResponse = BadRequest("refresh_token_failure", "Invalid token.");
            var claimsPrincipal = _jwtHandler.GetPrincipalFromToken(request.AccessToken);
            if (claimsPrincipal == null)
            {
                return failedResponse;
            }

            var userIdClaim = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            var user = await _repository.FistOrDefaultAsync<AppUser>(x => x.Id == new Guid(userIdClaim.Value));

            return Ok(new AuthResponse
            {
                AccessToken = _jwtHandler.GenerateAccessToken(user.Id, user.UserName, null)
            });
        }

        public ServiceResponse ValidateToken(string token)
        {
            var badRequest = BadRequest("token_invalid", "Invalid token");
            if (token.IsNullOrEmpty())
            {
                return badRequest;
            }

            var claims = _jwtHandler.GetPrincipalFromToken(token);
            if (claims == null)
            {
                return badRequest;
            }

            return Ok(claims.Claims.ToDictionary(claim => claim.Type, claim => claim.Value));
        }

        public async Task<ServiceResponse> Logout()
        {
            var userId = _principal?.UserId;
            var result = false;
            var entity = await _repository.FistOrDefaultAsync<RefreshToken>(x => x.UserId == userId);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();

                result = true;
            }
            return Ok(result);
        }

        public async Task<ServiceResponse> ForgotPassword(ForgotPasswordRequestModel model)
        {
            var account = await _repository.FistOrDefaultAsync<AppUser>(x => x.Email == model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return BadRequest("Không tồn tại email này trong hệ thống.");

            // create reset token that expires after 1 day
            account.ResetToken = RandomTokenString();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _repository.Update(account);
            await _unitOfWork.SaveChangesAsync();

            // send email
            SendPasswordResetEmail(account);
            return Ok();
        }

        public async Task<ServiceResponse> CreateUser(UserCreateRequestModel model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                return BadRequest("", "Tên dăng nhập là bắt buộc.");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("", "Email là bắt buộc.");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("", "Mật khẩu là bắt buộc.");
            }
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email,
                LockoutEnabled = false
            };
            await _userManager.CreateAsync(user, model.Password);
            await _unitOfWork.SaveChangesAsync();
            return Ok(model, "", "Tạo người dùng thành công.");
        }

        #region private method

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void SendPasswordResetEmail(AppUser account)
        {
            string message;
            var origin = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }

        #endregion private method
    }
}