using cm.Utilities.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Gem.Core.AspNetCore
{
    public class ModelStateValidatorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotSupportedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = string.Join(" ", context.ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage.EnsureEndsWithDot())));
                context.Result = new BadRequestObjectResult(ServiceResponse.Fail(StatusCodes.Status400BadRequest, "G001", message));
            }
        }
    }
}