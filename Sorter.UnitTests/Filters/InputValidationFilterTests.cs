using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using Sorter.Filters;
using Sorter.Models.Response;
using Sorter.UnitTests.TestHelpers;
using System.Collections.Generic;
using System.Net;

namespace Sorter.UnitTests.Filters
{
  public class InputValidationFilterTests
  {
    private static readonly IFixture Fixture = AutoDataCustomizationAttribute.CustomizedFixture;

    [Test, AutoDataCustomization]
    public void OnActionExecuted_ValidModelState_DoesNothing()
    {
      // Arrange
      var actionContext = CreateActionContext();
      var executedActionContext = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new object());
      var inputValidationFilterSut = new InputValidationFilter();

      // Act
      inputValidationFilterSut.OnActionExecuted(executedActionContext);

      // Assert
      Assert.IsNull(executedActionContext.Result);
    }

    [Test, AutoDataCustomization]
    public void OnActionExecuted_InvalidModelState_SetsBadRequestResponse(string errorKey, string errorMessage)
    {
      // Arrange
      var actionContext = CreateActionContext();
      actionContext.ModelState.AddModelError(errorKey, errorMessage);

      var executedActionContext = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new object());
      var inputValidationFilterSut = new InputValidationFilter();

      // Act
      inputValidationFilterSut.OnActionExecuted(executedActionContext);

      // Assert
      Assert.IsAssignableFrom<BadRequestObjectResult>(executedActionContext.Result);
      var response = (BadRequestObjectResult)executedActionContext.Result;

      Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
      Assert.IsAssignableFrom<ErrorResponseModel>(response.Value);

      var errorResponse = (ErrorResponseModel)response.Value;
      Assert.AreEqual(errorMessage, errorResponse.ErrorMessage);
    }

    private static ActionContext CreateActionContext()
    {
      // Fixture fails to create ActionContext due to some deeper properties of ActionDescriptor.
      // TODO: Move this object creation logic to Fixture Customization or analyze why it fails to create ActionDescriptor.
      var actionContext = new ActionContext();
      actionContext.HttpContext = Fixture.Create<HttpContext>();
      actionContext.RouteData = Fixture.Create<RouteData>();
      actionContext.ActionDescriptor = new ActionDescriptor();
      return actionContext;
    }
  }
}
