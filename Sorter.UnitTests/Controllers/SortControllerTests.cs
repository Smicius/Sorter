using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Sorter.Controllers;
using Sorter.Models.Request;
using Sorter.Models.Response;
using Sorter.Services;
using Sorter.UnitTests.TestHelpers;

namespace Sorter.UnitTests.Controllers
{
  public class SortControllerTests
  {
    [Test, AutoDataCustomization]
    public void Provide_SavesSortedSequenceAndReturnsOk(Mock<ISortService> sortService, NumberSequenceRequestModel request)
    {
      // Arrange
      var sortControllerSut = new SortController(sortService.Object);

      // Act
      var response = sortControllerSut.Provide(request);

      // Assert
      Assert.IsAssignableFrom<OkResult>(response);

      sortService.Verify(service => service.SaveSorted(request.Numbers), Times.Once);
      sortService.VerifyNoOtherCalls();
    }

    [Test, AutoDataCustomization]
    public void Latest_ReturnsLatestNumberSequence(Mock<ISortService> sortService, NumberSequenceRequestModel request, int[] expectedNumbers)
    {
      // Arrange
      sortService
        .Setup(service => service.LoadLatest())
        .Returns(expectedNumbers);

      var sortControllerSut = new SortController(sortService.Object);

      // Act
      var response = sortControllerSut.Latest();

      // Assert
      Assert.IsAssignableFrom<OkObjectResult>(response);
      var responseBody = ((OkObjectResult)response).Value;

      Assert.IsAssignableFrom<NumberSequenceResponseModel>(responseBody);
      var responseModel = (NumberSequenceResponseModel)responseBody;

      Assert.AreEqual(expectedNumbers, responseModel.Numbers);

      sortService.Verify(service => service.LoadLatest(), Times.Once);
      sortService.VerifyNoOtherCalls();
    }
  }
}
