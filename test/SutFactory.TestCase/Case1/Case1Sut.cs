namespace Hj.SutFactory.TestCase.Case1;

public class Case1Sut(
  IInterfaceInput interfaceInput,
  ClassAbstractInput classAbstractInput,
  ClassGenericInput<List<string>> classGenericInput,
  ClassInput classInput,
  ClassNestedClassInput.ClassInput classNestedClassInput,
  ClassSealedInput classSealedInput,
  ClassWithDependency classWithDependency)
{
  public IInterfaceInput InterfaceInput { get; } = interfaceInput;

  public ClassAbstractInput ClassAbstractInput { get; } = classAbstractInput;

  public ClassGenericInput<List<string>> ClassGenericInput { get; } = classGenericInput;

  public ClassInput ClassInput { get; } = classInput;

  public ClassNestedClassInput.ClassInput ClassNestedClassInput { get; } = classNestedClassInput;

  public ClassSealedInput ClassSealedInput { get; } = classSealedInput;

  public ClassWithDependency ClassWithDependency { get; } = classWithDependency;
}
