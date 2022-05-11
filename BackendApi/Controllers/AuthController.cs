using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Utilities.Contracts;
using ViewModels.Auth;
using ViewModels.Users;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<ServiceResponse> Login(LoginModel model) => await _authService.AuthenticateAsync(new AuthRequest
        {
            Password = model.Password,
            UserName = model.UserName,
            ExtraProps = model.ExtraProps,
            RemoteIpAddress = GetIpAddress()
        });

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ServiceResponse> RefreshToken(RefreshTokenModel model) => await _authService.RefreshTokenAsync(new RefreshTokenRequest
        {
            AccessToken = model.AccessToken,
            RefreshToken = model.RefreshToken,
            RemoteIpAdderss = GetIpAddress()
        });

        [HttpPost("validate")]
        public ServiceResponse ValidateToken(ValidateTokenModel model) => _authService.ValidateToken(model.Token);

        private string GetIpAddress()
        {
            return Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"].ToString()
                : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<ServiceResponse> CreateUser([FromBody] UserCreateRequestModel model)
        {
            var result = await _authService.CreateUser(model);
            return result;
        }
    }
}