namespace Hj.SutFactory.UnitTest.TestData.Case1;

public class Case1Sut
{
  public Case1Sut(
    IInterfaceInput interfaceInput,
    ImplementationInput implementationInput)
  {
    InterfaceInputValue = interfaceInput;
    ImplementationInputValue = implementationInput;
  }

  public IInterfaceInput InterfaceInputValue { get; }

  public ImplementationInput ImplementationInputValue { get; }
}
