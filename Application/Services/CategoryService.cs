using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Contracts;
using Utilities.Services;
using ViewModels.Categories;
using ViewModels.CategoryItems;

namespace Application.Services
{
    public class CategoryService : Service, ICategoryService
    {
        private readonly IBaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
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

        public ServiceResponse GetListCategory()
        {
            var lstCategoryItems = _repository
                .Where<CategoryItem>(x => x.IsDeleted == false);
            var result = (_repository
                .Where<Category>(x => x.IsDeleted == false))
               .OrderBy(x => x.Order).ToList().Select(x => new CategoryResponseModel
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name,
                   Order = x.Order,
                   CreatedAt = x.CreatedAt,
                   CreatedBy = x.CreatedBy,
                   UpdatedAt = x.UpdatedAt,
                   UpdatedBy = x.UpdatedBy,
                   CategoryItems = lstCategoryItems.Where(c => c.CategoryId == x.Id).OrderBy(c => c.Order).Select(y => new CategoryItemResponseModel
                   {
                       Id = y.Id,
                       CategoryId = y.CategoryId,
                       Code = y.Code,
                       Name = y.Name,
                       Order = y.Order,
                       Description = y.Description,
                       ParentId = y.ParentId,
                       CreatedAt = y.CreatedAt,
                       CreatedBy = y.CreatedBy,
                       UpdatedAt = y.UpdatedAt,
                       UpdatedBy = y.UpdatedBy
                   }).ToList()
               });
            return Ok(result);
        }

        public async Task<ServiceResponse> CreateCategory(CategoryCreateRequestModel model)
        {
            model.Id = Guid.NewGuid();
            var category = _mapper.Map<Category>(model);
            await _repository.AddAsync<Category>(category);
            await _unitOfWork.SaveChangesAsync();
            return Ok(model, "", "Thêm mới danh mục thành công.");
        }

        public async Task<ServiceResponse> UpdateCategory(CategoryCreateRequestModel model)
        {
            if (!model.Id.HasValue)
            {
                return BadRequest("", "Bạn chưa chọn danh mục sửa.");
            }
            var entity = await _repository.FistOrDefaultAsync<Category>(x => x.Id == model.Id.Value);
            entity.Name = model.Name;
            entity.Order = model.Order;
            entity.Code = model.Code;
            _repository.Update<Category>(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(model, "", "Cập nhật danh mục thành công.");
        }

        public async Task<ServiceResponse> GetHierarchical(CategoryHierarchicalSearchRequest model)
        {
            List<CategoryResponseModel> lstCategoryReponse = new List<CategoryResponseModel>();
            var lstCategory = await _repository.WhereAsync<Category>(x => (model != null && model.LstCategoryCode != null) ? model.LstCategoryCode.Contains(x.Code) : 1 == 1);
            var lstCategoryItem = await _repository.WhereAsync<CategoryItem>(x => lstCategory.Select(c => c.Id).Contains(x.CategoryId));
            foreach (var item in lstCategory.OrderBy(x => x.Order))
            {
                var itemInCategory = lstCategoryItem.OrderBy(x => x.Order).Where(x => x.CategoryId == item.Id && (model.ParentId.HasValue ? x.ParentId == model.ParentId.Value : x.ParentId == null)).Select(y => new CategoryItemResponseModel
                {
                    Id = y.Id,
                    CategoryId = y.CategoryId,
                    Code = y.Code,
                    Name = y.Name,
                    Order = y.Order,
                    Description = y.Description,
                    ParentId = y.ParentId,
                    CreatedAt = y.CreatedAt,
                    CreatedBy = y.CreatedBy,
                    UpdatedAt = y.UpdatedAt,
                    UpdatedBy = y.UpdatedBy
                }).ToList();
                var categoryResponse = new CategoryResponseModel
                {
                    Id = item.Id,
                    Code = item.Code,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    Name = item.Name,
                    Order = item.Order,
                    UpdatedAt = item.UpdatedAt,
                    UpdatedBy = item.UpdatedBy
                };
                if (model.IsGetMoreDepthChildren)
                {
                    foreach (var itemC in itemInCategory)
                    {
                        GetTreeCategoryItem(lstCategoryItem, itemC, out CategoryItemResponseModel categoryItemResponse);
                        categoryResponse.CategoryItems.Add(categoryItemResponse);
                    }
                }
                else
                {
                    categoryResponse.CategoryItems.AddRange(itemInCategory);
                }

                lstCategoryReponse.Add(categoryResponse);
            }
            return Ok(lstCategoryReponse);
        }

        public async Task<ServiceResponse> GetTreeItemGroupByCategory(CategoryHierarchicalSearchRequest model)
        {
            Dictionary<string, List<CategoryItemResponseModel>> pairs = new Dictionary<string, List<CategoryItemResponseModel>>();
            var lstCategory = await _repository.WhereAsync<Category>(x => (model != null && model.LstCategoryCode != null) ? model.LstCategoryCode.Contains(x.Code) : 1 == 1);
            var lstCategoryItem = await _repository.WhereAsync<CategoryItem>(x => lstCategory.Select(c => c.Id).Contains(x.CategoryId));

            var itemAll = await _repository.FistOrDefaultAsync<CategoryItem>(x => x.Id == Guid.Parse("fbe7e8bc-c2bd-41f4-beca-021c339e1131"));
            foreach (var item in lstCategory.OrderBy(x => x.Order))
            {
                var itemInCategory = lstCategoryItem.Where(x => x.CategoryId == item.Id && (model.ParentId.HasValue ? x.ParentId == model.ParentId.Value : x.ParentId == null)).OrderBy(x => x.Order).Select(y => new CategoryItemResponseModel
                {
                    Id = y.Id,
                    CategoryId = y.CategoryId,
                    Code = y.Code,
                    Name = y.Name,
                    Order = y.Order,
                    Description = y.Description,
                    ParentId = y.ParentId,
                    CreatedAt = y.CreatedAt,
                    CreatedBy = y.CreatedBy,
                    UpdatedAt = y.UpdatedAt,
                    UpdatedBy = y.UpdatedBy
                }).ToList();
                var lstItemResult = new List<CategoryItemResponseModel>();
                if (model.IsGetValueAll)
                {
                    lstItemResult.Add(new CategoryItemResponseModel
                    {
                        Id = itemAll.Id,
                        CategoryId = itemAll.CategoryId,
                        Code = itemAll.Code,
                        Name = itemAll.Name,
                        Order = itemAll.Order,
                        Description = itemAll.Description,
                        ParentId = itemAll.ParentId,
                        CreatedAt = itemAll.CreatedAt,
                        CreatedBy = itemAll.CreatedBy,
                        UpdatedAt = itemAll.UpdatedAt,
                        UpdatedBy = itemAll.UpdatedBy
                    });
                }
                if (model.IsGetMoreDepthChildren)
                {
                    foreach (var itemC in itemInCategory)
                    {
                        GetTreeCategoryItem(lstCategoryItem, itemC, out CategoryItemResponseModel categoryItemResponse);
                        lstItemResult.Add(categoryItemResponse);
                    }
                }
                else
                {
                    lstItemResult.AddRange(itemInCategory);
                }
                pairs.Add(item.Code, lstItemResult);
            }
            return Ok(pairs);
        }

        public void GetTreeCategoryItem(List<CategoryItem> lstCategoryItem, CategoryItemResponseModel item, out CategoryItemResponseModel categoryItemResponse)
        {
            var listUnitChilds = lstCategoryItem.Where(x => x.ParentId == item.Id).OrderBy(x => x.Order);
            if (listUnitChilds != null)
            {
                item.Children = listUnitChilds.Select(y => new CategoryItemResponseModel
                {
                    Id = y.Id,
                    CategoryId = y.CategoryId,
                    Code = y.Code,
                    Name = y.Name,
                    Order = y.Order,
                    Description = y.Description,
                    ParentId = y.ParentId,
                    CreatedAt = y.CreatedAt,
                    CreatedBy = y.CreatedBy,
                    UpdatedAt = y.UpdatedAt,
                    UpdatedBy = y.UpdatedBy
                }).ToList();
                foreach (var children in item.Children)
                {
                    GetTreeCategoryItem(lstCategoryItem, children, out categoryItemResponse);
                }
            }
            categoryItemResponse = item;
        }
    }
}