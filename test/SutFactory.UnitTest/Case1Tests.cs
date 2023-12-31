using Hj.SutFactory.TestCase.Case1;

namespace Hj.SutFactory.UnitTest.Case1;

// Case 1 focus on the automatic creation of instances.
public class Case1Tests
{
  [Fact]
  public void CreateSut_GivenUnknownRegistrations_CreatesSut()
  {
    // act
    var result = SystemUnderTest.For<Case1Sut>();

    // assert
    Assert.NotNull(result.InterfaceInput);
    Assert.NotNull(result.ClassAbstractInput);
    Assert.NotNull(result.ClassGenericInput);
    Assert.NotNull(result.ClassGenericInput.Value);
    Assert.NotNull(result.ClassInput);
    Assert.NotNull(result.ClassNestedClassInput);
    Assert.NotNull(result.ClassSealedInput);
    Assert.NotNull(result.ClassWithDependency);
  }

  [Fact]
  public void CreateSut_GivenKnownRegistrations_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilder = sutBuilder.InputBuilder;

    var interfaceInput = inputBuilder.Instance<IInterfaceInput>();
    var classAbstractInput = inputBuilder.Instance<ClassAbstractInput>();
    var classGenericInputValue = inputBuilder.Instance<List<string>>();
    var classGenericInput = inputBuilder.Instance<ClassGenericInput<List<string>>>();
    var classInput = inputBuilder.Instance<ClassInput>();
    var classNestedClassInput = inputBuilder.Instance<ClassNestedClassInput.ClassInput>();
    var classSealedInput = inputBuilder.Instance<ClassSealedInput>();
    var classWithDependency = inputBuilder.Instance<ClassWithDependency>();

    // act
    var result = sutBuilder.CreateSut<Case1Sut>();

    // assert
    Assert.Same(interfaceInput, result.InterfaceInput);
    Assert.Same(classAbstractInput, result.ClassAbstractInput);
    Assert.Same(classGenericInput, result.ClassGenericInput);
    Assert.Same(classGenericInputValue, result.ClassGenericInput.Value);
    Assert.Same(classInput, result.ClassInput);
    Assert.Same(classNestedClassInput, result.ClassNestedClassInput);
    Assert.Same(classSealedInput, result.ClassSealedInput);
    Assert.Same(classWithDependency, result.ClassWithDependency);
  }
}
