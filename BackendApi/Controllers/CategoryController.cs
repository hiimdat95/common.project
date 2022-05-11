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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public ServiceResponse GetAll()
        {
            return _service.GetListCategory();
        }

        [HttpPost]
        public async Task<ServiceResponse> CreateCategory([FromBody] CategoryCreateRequestModel model)
        {
            return await _service.CreateCategory(model);
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateCategory([FromBody] CategoryCreateRequestModel model)
        {
            return await _service.UpdateCategory(model);
        }

        [HttpPost("get-hierarchical")]
        public async Task<ServiceResponse> GetHierarchical([FromBody] CategoryHierarchicalSearchRequest model)
        {
            return await _service.GetHierarchical(model);
        }

        [HttpPost("get-tree-item-groupby-category")]
        public async Task<ServiceResponse> GetTreeItemGroupByCategory([FromBody] CategoryHierarchicalSearchRequest model)
        {
            return await _service.GetTreeItemGroupByCategory(model);
        }
    }
}