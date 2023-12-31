using Hj.SutFactory.TestCase.Case4;

namespace Hj.SutFactory.UnitTest.Case4;

// Case 4 focus on handling implementations that has multiple constructors.
public class Case4Tests
{
  [Fact]
  public void CreateSut_GivenFactories_CreatesSut()
  {
    // arrange & act
    var result = SystemUnderTest.For<Case4Sut>(arrange =>
    {
      // Registering factories that each choose a specific constructor allow registration of implementations
      // that has multiple constructors.
      arrange.Advanced.Instance(() => new ClassAmbiguousCtorInput(new object()));
      arrange.Advanced.Instance<ISecondCtor, ClassAmbiguousCtorInput>(() => new ClassAmbiguousCtorInput(new object(), new object()));
    });

    // assert
    Assert.NotNull(result.ClassAmbiguousCtorInput1);
    Assert.NotNull(result.ClassAmbiguousCtorInput2);
  }

  [Fact]
  public void CreateSut_GivenImplementation_Throws()
  {
    // arrange, act & assert
    Assert.Throws<InvalidOperationException>(() => SystemUnderTest.For<Case4AmbiguousCtorSut>());
  }
}
