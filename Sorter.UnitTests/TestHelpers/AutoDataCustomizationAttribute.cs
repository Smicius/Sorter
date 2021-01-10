using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace Sorter.UnitTests.TestHelpers
{
  public class AutoDataCustomizationAttribute : AutoDataAttribute
  {
    public static readonly IFixture CustomizedFixture = new Fixture().Customize(new AutoMoqCustomization());

    public AutoDataCustomizationAttribute()
      : base(() => CustomizedFixture)
    {
    }
  }
}
