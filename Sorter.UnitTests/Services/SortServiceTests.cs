using AutoFixture;
using Moq;
using NUnit.Framework;
using Sorter.Services;
using Sorter.UnitTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorter.UnitTests.Services
{
  public class SortServiceTests
  {
    private const string LatestSequenceFilePath = "result.txt";

    private static readonly IFixture Fixture = AutoDataCustomizationAttribute.CustomizedFixture;

    public static IEnumerable<object[]> SaveSortedTestCases = new List<object[]>
    {
      new object[]{ Fixture.Create<Mock<IStorageService>>(), new [] { 5, 2, 8, 10, 1 }, new [] { 1, 2, 5, 8, 10 } },
      new object[]{ Fixture.Create<Mock<IStorageService>>(), new [] { 5, 5, 5, 5, 1 }, new [] { 1, 5, 5, 5, 5 } },
      new object[]{ Fixture.Create<Mock<IStorageService>>(), Array.Empty<int>(), Array.Empty<int>() },
      new object[]{ Fixture.Create<Mock<IStorageService>>(), new [] { 1, 2, 3, 4, 5 }, new [] { 1, 2, 3, 4, 5 } }
    };

    [Test, TestCaseSource(nameof(SaveSortedTestCases))]
    public void SaveSorted_SortsAndSavesToStorage(Mock<IStorageService> storageService, int[] numbers, int[] expectedSortedNumbers)
    {
      // Arrange
      var sortControllerSut = new SortService(storageService.Object);

      // Act
      sortControllerSut.SaveSorted(numbers);

      // Assert
      storageService.Verify(
        service => service.Save(It.Is<int[]>(actualNumbers => Enumerable.SequenceEqual(expectedSortedNumbers, actualNumbers)), LatestSequenceFilePath),
        Times.Once);
      storageService.VerifyNoOtherCalls();
    }

    [Test, AutoDataCustomization]
    public void LoadLatest_LoadsNumbersFromFile(Mock<IStorageService> storageService, int[] expectedNumbers)
    {
      // Arrange
      storageService
        .Setup(service => service.Load(It.IsAny<string>()))
        .Returns(expectedNumbers);

      var sortControllerSut = new SortService(storageService.Object);

      // Act
      var latestNumbers = sortControllerSut.LoadLatest();

      // Assert
      Assert.AreEqual(expectedNumbers, latestNumbers);

      storageService.Verify(service => service.Load(LatestSequenceFilePath), Times.Once);
      storageService.VerifyNoOtherCalls();
    }
  }
}
