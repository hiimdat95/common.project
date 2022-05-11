using Utilities.Contracts;
using ViewModels.Categories;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        ServiceResponse GetListCategory();

        Task<ServiceResponse> CreateCategory(CategoryCreateRequestModel model);

        Task<ServiceResponse> UpdateCategory(CategoryCreateRequestModel model);

        Task<ServiceResponse> GetHierarchical(CategoryHierarchicalSearchRequest model);

        Task<ServiceResponse> GetTreeItemGroupByCategory(CategoryHierarchicalSearchRequest model);
    }
}