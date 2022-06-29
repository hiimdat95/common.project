using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
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
            Type genericType = typeof(IAuditedEntity);
            Assembly a = Assembly.Load("Domain");
            Type[] typeArgs = { Type.GetType("Domain.Entities.Profile") };
            Type type2 = a.GetType("Domain.Entities.Profile");
            Type entityType = genericType.MakeGenericType(typeArgs);
            object entity = Activator.CreateInstance(type2);

            return await _dynamicService.GetAllAsync<>(model);
        }
        public static bool TryParse<T>(string source, out T value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertTo(typeof(T)) && converter.CanConvertFrom(typeof(string)))
            {
                value = (T)converter.ConvertFromString(source);
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}