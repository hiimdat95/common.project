using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Dapper;
using Infrastructure.EF;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Repository.Repositories;
using Repository.UnitOfWork;
using Utilities.Auths;
using Utilities.Common;

namespace Ioc
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
            services.AddScoped<ICurrentPrincipal, CurrentPrincipal>();
            services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
            services.AddScoped(typeof(IDapperProvider<>), typeof(DapperProvider<>));
            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddScoped<IAuthValidator, AuthValidator>();
            services.AddScoped<IAuthService, AuthService<AppUser>>();

            //Repositories

            //Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryItemService, CategoryItemService>();

            return services;
        }
    }
}