using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Contracts;
using ViewModels.Auth;
using ViewModels.Users;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/dynamic")]
    public class DynamicController : ControllerBase
    {
        private readonly IDynamicService _dynamicService;

        public DynamicController(IDynamicService dynamicService) => _dynamicService = dynamicService;

        [HttpPost("list")]
        [AllowAnonymous]
        public async Task<ServiceResponse> GetAll(PaginatedInputModel model)
        {
            //Type genericType = typeof(IAuditedEntity);
            //Type[] typeArgs = { Type.GetType(model.Object) };
            //Type entityType = genericType.MakeGenericType(typeArgs);
            //object entity = Activator.CreateInstance(entityType);

            Assembly a = Assembly.Load("Domain");
            Type type = a.GetType("Domain.Entities.Profile");
            object entity = Activator.CreateInstance(type);
            return await _dynamicService.GetAllAsync<type>(model);
        }
    }
}