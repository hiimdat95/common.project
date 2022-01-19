using AutoMapper;
using cm.Application.AuthServices;
using cm.Application.Mappers;
using cm.Repository.Interfaces;
using cm.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace cm.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
                                {
                                    mc.AddProfile(new RequestMappingProfile());
                                    mc.AddProfile(new ResponeMappingProfile());
                                });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IBaseRepository, BaseRepository>();

            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddScoped<IAuthValidator, AuthValidator>();
            return services;
        }
    }
}