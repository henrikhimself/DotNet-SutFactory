namespace Hj.SutFactory.UnitTest.TestData;

public class TestSut
{
  public TestSut(
      NullInput nullInput,
      IInterfaceInput interfaceInput,
      ImplementationInput implementationInput,
      ClassInput classInput,
      ClassSealedInput classSealedInput,
      ClassAbstractInput classAbstractInput,
      ClassWithDependency classWithDependency,
      NestedInput.ClassInput nestedClassInput,
      ClassGenericInput<object> classGenericInput,
      IEnumerable<object> enumerableInput,
      IAutoInterfaceInput autoInterfaceInput,
      AutoImplementationInput autoImplementationInput,
      InstanceInput instanceInput)
  {
    NullInput = nullInput;
    InterfaceInputValue = interfaceInput;
    ImplementationInputValue = implementationInput;
    ClassInputValue = classInput;
    ClassSealedInputValue = classSealedInput;
    ClassAbstractInputValue = classAbstractInput;
    ClassWithDependency = classWithDependency;
    NestedClassInputValue = nestedClassInput;
    ClassGenericValue = classGenericInput;
    EnumerableInput = enumerableInput;
    AutoInterfaceInput = autoInterfaceInput;
    AutoImplementationInput = autoImplementationInput;
    InstanceInput = instanceInput;
  }

  public NullInput NullInput { get; }

  public IInterfaceInput InterfaceInputValue { get; }

  public ImplementationInput ImplementationInputValue { get; }

  public ClassInput ClassInputValue { get; }

  public ClassSealedInput ClassSealedInputValue { get; }

  public ClassAbstractInput ClassAbstractInputValue { get; }

  public ClassWithDependency ClassWithDependency { get; }

  public NestedInput.ClassInput NestedClassInputValue { get; }

  public ClassGenericInput<object> ClassGenericValue { get; }

  public IEnumerable<object> EnumerableInput { get; }

  public IAutoInterfaceInput AutoInterfaceInput { get; }

  public AutoImplementationInput AutoImplementationInput { get; }

  public InstanceInput InstanceInput { get; }
}
