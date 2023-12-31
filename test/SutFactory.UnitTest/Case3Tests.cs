using Hj.SutFactory.TestCase.Case3;

namespace Hj.SutFactory.UnitTest.Case3;

// Case 3 focus on returning multiple implementations, of an interface, as an IEnumerable and
// overriding this inferred behavior.
public class Case3Tests
{
  [Fact]
  public void Instance_GivenImplementationsOfInterface_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilder = sutBuilder.InputBuilder;

    var implementationInput1 = inputBuilder.Instance<ImplementationInput1>();
    var implementationInput2 = inputBuilder.Instance<ImplementationInput2>();

    // act
    var result = sutBuilder.CreateSut<Case3Sut>();

    // assert
    Assert.Equal(2, result.InterfaceInputValues.Count());
    Assert.Contains(implementationInput1, result.InterfaceInputValues);
    Assert.Contains(implementationInput2, result.InterfaceInputValues);
  }

  [Fact]
  public void Instance_GivenImplementation_CreatesSut()
  {
    // arrange & act
    var result = SystemUnderTest.For<Case3Sut>(arrange =>
    {
      // Adding an explicit registration for IEnumerable<IInterfaceInput>, before any
      // implementations of the interface are registered, is important if this service
      // type are to return an empty enumerable. Otherwise, the Instance<T> method
      // will create and return an enumerable of the registerered implementations.
      arrange.Instance<IEnumerable<IInterfaceInput>>();

      // These implementations would otherwise be returned as an enumerable of the
      // implemented interface.
      arrange.Instance<ImplementationInput1>();
      arrange.Instance<ImplementationInput2>();
    });

    // assert
    Assert.Empty(result.InterfaceInputValues);
  }

  [Fact]
  public void Instance_GivenMultipleImplementations_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilder = sutBuilder.InputBuilder;

    var implementationInput1 = inputBuilder.Instance<ImplementationInput1>();
    var implementationInput2 = inputBuilder.Instance<ImplementationInput2>();

    // act & assert
    Assert.Throws<InvalidOperationException>(inputBuilder.Instance<IInterfaceInput>);
  }
}
