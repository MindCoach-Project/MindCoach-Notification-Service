using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MinhCoach_Notification_Service.Api.Common.Errors.Http;

namespace MinhCoach_Notification_Service.Api.Controllers;
[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {    
        if(errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(e => 
                e.Type == ErrorType.Validation &&
                (e.Metadata != null && 
                 e.Metadata.TryGetValue("IsValidationError", out var isValidation) 
                    ? (bool)isValidation : true)))
        {
            return ValidationProblem(errors);
        }
        
        var firstError = errors[0];
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        return Problem(firstError);
    }
    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors) 
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }
        return ValidationProblem(modelStateDictionary);
    }
}