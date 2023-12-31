using Hj.SutFactory.TestCase.Case2;

namespace Hj.SutFactory.UnitTest.Case2;

// Case 2 focus on creating instances using the Advanced methods and implicit registrations.
public class Case2Tests
{
  [Fact]
  public void Instance_GivenImplementation_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;

    var implementationInput = inputBuilderAdvanced.Instance<ImplementationInput>();

    // act
    var result = sutBuilder.CreateSut<Case2Sut>();

    // assert
    var interfaceInput = inputBuilderAdvanced.GetRequiredService<IInterfaceInput>();
    Assert.Same(implementationInput, interfaceInput);

    Assert.Same(implementationInput, result.InterfaceInputValue);
    Assert.Same(implementationInput, result.ImplementationInputValue);
  }

  [Fact]
  public void Instance_GivenFactory_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;

    var implementationInput = inputBuilderAdvanced.Instance(() => new ImplementationInput());

    // act
    var result = sutBuilder.CreateSut<Case2Sut>();

    // assert
    var interfaceInput = inputBuilderAdvanced.GetRequiredService<IInterfaceInput>();
    Assert.Same(implementationInput, interfaceInput);

    Assert.Same(implementationInput, result.InterfaceInputValue);
    Assert.Same(implementationInput, result.ImplementationInputValue);
  }

  [Fact]
  public void SubstitutePartsOf_GivenImplementation_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;

    var implementationInput = inputBuilderAdvanced.SubstitutePartsOf<ImplementationInput>();

    // act
    var result = sutBuilder.CreateSut<Case2Sut>();

    // assert
    var interfaceInput = inputBuilderAdvanced.GetRequiredService<IInterfaceInput>();
    Assert.Same(implementationInput, interfaceInput);

    Assert.Same(implementationInput, result.InterfaceInputValue);
    Assert.Same(implementationInput, result.ImplementationInputValue);
  }

  [Fact]
  public void Substitute_GivenInterface_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;

    var interfaceInput = inputBuilderAdvanced.Substitute<IInterfaceInput>();

    // act
    var result = sutBuilder.CreateSut<Case2Sut>();

    // assert
    Assert.Same(interfaceInput, result.InterfaceInputValue);
  }
}
