using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Contracts;
using ViewModels.Auth;
using ViewModels.Users;

namespace Application.Interfaces
{
    public interface IDynamicService
    {
        Task<ServiceResponse> GetAllAsync<T>(PaginatedInputModel pagingParams);
    }
}