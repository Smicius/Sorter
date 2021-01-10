using Moq;
using NUnit.Framework;
using Sorter.Helpers;
using Sorter.Repositories;
using Sorter.UnitTests.TestHelpers;
using System;
using System.IO;
using System.Linq;

namespace Sorter.UnitTests.Repositories
{
  public class StorageRepositoryTests
  {
    [Test, AutoDataCustomization]
    public void Save_SavesNumbersInValidFormat(Mock<IFileHelper> fileHelper, string filePath, int[] numbers)
    {
      // Arrange
      var expectedSavedContent = string.Join(" ", numbers);
      var stream = new MemoryStreamMock();

      fileHelper
        .Setup(service => service.Open(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>(), It.IsAny<FileShare>()))
        .Returns(stream);

      var storageRepositorySut = new StorageRepository(fileHelper.Object);

      // Act
      storageRepositorySut.Save(numbers, filePath);

      // Assert
      var savedContent = StreamHelper.ToString(stream);
      Assert.AreEqual(expectedSavedContent, savedContent);

      Assert.IsTrue(stream.IsClosed);

      fileHelper.Verify(service => service.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None), Times.Once);
      fileHelper.VerifyNoOtherCalls();
    }

    [Test, AutoDataCustomization]
    public void Load_ValidData_ReturnsParsedNumbers(Mock<IFileHelper> fileHelper, string filePath, int[] expectedNumbers)
    {
      // Arrange
      var stream = StreamHelper.Create(string.Join(" ", expectedNumbers));

      fileHelper
        .Setup(service => service.OpenRead(It.IsAny<string>()))
        .Returns(stream);

      var storageRepositorySut = new StorageRepository(fileHelper.Object);

      // Act
      var actualNumbers = storageRepositorySut.Load(filePath);

      // Assert
      Assert.True(Enumerable.SequenceEqual(expectedNumbers, actualNumbers));

      fileHelper.Verify(service => service.OpenRead(filePath), Times.Once);
      fileHelper.VerifyNoOtherCalls();
    }

    [Test, AutoDataCustomization]
    public void Load_InvalidData_ThrowsException(Mock<IFileHelper> fileHelper, string filePath)
    {
      // Arrange
      var stream = StreamHelper.Create("5 7 NaN 155");

      fileHelper
        .Setup(service => service.OpenRead(It.IsAny<string>()))
        .Returns(stream);

      var storageRepositorySut = new StorageRepository(fileHelper.Object);

      // Act & Assert
      var exception = Assert.Throws<Exception>(() => storageRepositorySut.Load(filePath));

      Assert.AreEqual("'NaN' couldn't be parsed to a valid number.", exception.Message);
    }

    public class MemoryStreamMock : MemoryStream
    {
      public bool IsClosed { get; private set; }

      protected override void Dispose(bool disposing)
      {
        IsClosed = true;
      }
    }
  }
}
