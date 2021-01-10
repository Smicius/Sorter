using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sorter.Models.Response;
using System.Linq;

namespace Sorter.Filters
{
  public class InputValidationFilter : IActionFilter
  {
    public void OnActionExecuted(ActionExecutedContext context)
    {
      if (context.ModelState.IsValid)
        return;

      var firstErrorMessage = context.ModelState.Values.First().Errors.First().ErrorMessage;
      var errorResponse = new ErrorResponseModel
      {
        ErrorMessage = firstErrorMessage
      };
      context.Result = new BadRequestObjectResult(errorResponse);
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
  }
}
