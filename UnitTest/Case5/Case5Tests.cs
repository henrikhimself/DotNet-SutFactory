namespace Hj.SutFactory.UnitTest.Case5;

// Case 5 focus on NULL.
public class Case5Tests
{
  [Fact]
  public void CreateSut_GivenCase6Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilder = sutBuilder.InputBuilder;

    inputBuilder.Null<NullInput>();

    // act
    var result = sutBuilder.CreateSut<Case5Sut>();

    // assert
    Assert.Null(result.NullInput);
  }
}
