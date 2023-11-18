namespace Hj.SutFactory.UnitTest.TestData.Case1;

public class Case1Sut(
  IInterfaceInput interfaceInput,
  ImplementationInput implementationInput)
{
  public IInterfaceInput InterfaceInputValue { get; } = interfaceInput;

  public ImplementationInput ImplementationInputValue { get; } = implementationInput;
}
