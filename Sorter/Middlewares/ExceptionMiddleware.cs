using Microsoft.AspNetCore.Http;
using Sorter.Exceptions;
using Sorter.Models.Response;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Sorter.Middlewares
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate Next;

    public ExceptionMiddleware(RequestDelegate next)
    {
      Next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
      try
      {
        await Next(httpContext);
      }
      catch (BadRequestException badRequestException)
      {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var responseBody = new ErrorResponseModel { ErrorMessage = badRequestException.Message };
        await httpContext.Response.WriteAsJsonAsync(responseBody);
      }
      catch (Exception)
      {
        // TODO: implement logging.
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var responseBody = new ErrorResponseModel { ErrorMessage = "Something went wrong. Please contact your system administrator." };
        await httpContext.Response.WriteAsJsonAsync(responseBody);
      }
    }
  }
}
