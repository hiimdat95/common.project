using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.Contracts;
using ViewModels.Categories;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryItemController : ControllerBase
    {
        private readonly ICategoryItemService _service;

        public CategoryItemController(ICategoryItemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ServiceResponse> CreateCategoryItem([FromBody] CategoryItemCreateRequestModel model)
        {
            return await _service.CreateCategoryItem(model);
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateCategoryItem([FromBody] CategoryItemCreateRequestModel model)
        {
            return await _service.UpdateCategoryItem(model);
        }
    }
}