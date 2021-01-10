using Moq;
using NUnit.Framework;
using Sorter.Repositories;
using Sorter.Services;
using Sorter.UnitTests.TestHelpers;

namespace Sorter.UnitTests.Services
{
  public class StorageServiceTests
  {
    [Test, AutoDataCustomization]
    public void Save_SavesNumbers(Mock<IStorageRepository> storageRepository, int[] numbers, string filePath)
    {
      // Arrange
      var sortControllerSut = new StorageService(storageRepository.Object);

      // Act
      sortControllerSut.Save(numbers, filePath);

      // Assert
      storageRepository.Verify(service => service.Save(numbers, filePath), Times.Once);
      storageRepository.VerifyNoOtherCalls();
    }

    [Test, AutoDataCustomization]
    public void Load_LoadsNumbers(Mock<IStorageRepository> storageRepository, int[] expectedNumbers, string filePath)
    {
      // Arrange
      storageRepository
        .Setup(service => service.Load(It.IsAny<string>()))
        .Returns(expectedNumbers);

      var sortControllerSut = new StorageService(storageRepository.Object);

      // Act
      var actualNumbers = sortControllerSut.Load(filePath);

      // Assert
      Assert.AreEqual(expectedNumbers, actualNumbers);

      storageRepository.Verify(service => service.Load(filePath), Times.Once);
      storageRepository.VerifyNoOtherCalls();
    }
  }
}
