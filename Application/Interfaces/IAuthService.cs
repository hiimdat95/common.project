using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Contracts;
using ViewModels.Auth;
using ViewModels.Users;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse> GetAllAsync(PaginatedInputModel pagingParams);

        Task<ServiceResponse> AuthenticateAsync(AuthRequest request);

        Task<ServiceResponse> RefreshTokenAsync(RefreshTokenRequest request);

        ServiceResponse ValidateToken(string token);

        Task<ServiceResponse> CreateUser(UserCreateRequestModel model);
    }
}