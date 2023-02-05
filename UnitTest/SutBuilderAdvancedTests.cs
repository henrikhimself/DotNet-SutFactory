using Hj.SutFactory.UnitTest.TestData;

namespace Hj.SutFactory.UnitTest;

public class SutBuilderAdvancedTests
{
  [Fact]
  public void Instance_GivenInterface_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sutBuilder.Advanced.Instance<IInterfaceInput>());
  }

  [Fact]
  public void SubstitutePartsOf_GivenTInterface_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sutBuilder.Advanced.SubstitutePartsOf<IInterfaceInput>());
  }

  [Fact]
  public void Substitute_GivenT_ReturnsInstance()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    // act
    var result = sutBuilder.Advanced.Substitute<IInterfaceInput>();

    // assert
    Assert.NotNull(result);
  }
}
