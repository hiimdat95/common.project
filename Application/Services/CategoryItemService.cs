using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Contracts;
using Utilities.Services;
using ViewModels.Categories;

namespace Application.Services
{
    public class CategoryItemService : Service, ICategoryItemService
    {
        private readonly IBaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryItemService(
            IBaseRepository repository,
            IMapper mapper,
            IConfiguration config,
            IUnitOfWork unitOfWork
            , ICurrentPrincipal currentPrincipal
            , IHttpContextAccessor httpContextAccessor) : base(currentPrincipal, httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateCategoryItem(CategoryItemCreateRequestModel model)
        {
            model.Id = Guid.NewGuid();

            var categoryItem = _mapper.Map<CategoryItem>(model);
            await _repository.AddAsync<CategoryItem>(categoryItem);
            await _unitOfWork.SaveChangesAsync();
            return Ok(model, "", "Thêm mới danh mục thành công.");
        }

        public async Task<ServiceResponse> UpdateCategoryItem(CategoryItemCreateRequestModel model)
        {
            if (!model.Id.HasValue)
            {
                return BadRequest("", "Bạn chưa chọn danh mục sửa.");
            }
            var entity = await _repository.FistOrDefaultAsync<CategoryItem>(x => x.Id == model.Id.Value);
            entity.Name = model.Name;
            entity.Order = model.Order;
            entity.Code = model.Code;
            entity.Description = model.Description;
            entity.CategoryId = model.CategoryId;
            entity.ParentId = model.ParentId;
            _repository.Update<CategoryItem>(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(model, "", "Cập nhật danh mục thành công.");
        }
    }
}