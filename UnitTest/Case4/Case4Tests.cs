namespace Hj.SutFactory.UnitTest.Case4;

// Case 4 focus on handling implementations that has multiple constructors.
public class Case4Tests
{
  [Fact]
  public void CreateSut_GivenFactories_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilder = sutBuilder.InputBuilder;

    // Registering factories that each choose a specific constructor allow registration of implementations
    // that has multiple constructors.
    inputBuilder.Advanced.Instance(() => new ClassAmbiguousCtorInput(new object()));
    inputBuilder.Advanced.Instance<ISecondCtor, ClassAmbiguousCtorInput>(() => new ClassAmbiguousCtorInput(new object(), new object()));

    // act
    var result = sutBuilder.CreateSut<Case4Sut>();

    // assert
    Assert.NotNull(result.ClassAmbiguousCtorInput1);
    Assert.NotNull(result.ClassAmbiguousCtorInput2);
  }

  [Fact]
  public void CreateSut_GivenImplementation_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    // act & assert
    Assert.Throws<InvalidOperationException>(sutBuilder.CreateSut<Case4AmbiguousCtorSut>);
  }
}
