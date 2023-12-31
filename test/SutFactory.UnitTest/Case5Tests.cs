using Hj.SutFactory.TestCase.Case5;

namespace Hj.SutFactory.UnitTest.Case5;

// Case 5 focus on NULL.
public class Case5Tests
{
  [Fact]
  public void CreateSut_GivenCase6Sut_CreatesSut()
  {
    // arrange & act
    var result = SystemUnderTest.For<Case5Sut>(arrange =>
    {
      arrange.Null<NullInput>();
    });

    // assert
    Assert.Null(result.NullInput);
  }
}
