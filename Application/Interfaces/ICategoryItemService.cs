using Utilities.Contracts;
using ViewModels.Categories;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryItemService
    {
        Task<ServiceResponse> CreateCategoryItem(CategoryItemCreateRequestModel model);

        Task<ServiceResponse> UpdateCategoryItem(CategoryItemCreateRequestModel model);
    }
}