using Domain.Entities;
using Qldm.Infrastructure.Helpers;
using ViewModels.Categories;

namespace Application.Mappers
{
    public class RequestMappingProfile : AutoMapper.Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<CategoryCreateRequestModel, Category>().IgnoreAllNonExisting();
            CreateMap<CategoryItemCreateRequestModel, CategoryItem>().IgnoreAllNonExisting();
        }
    }
}