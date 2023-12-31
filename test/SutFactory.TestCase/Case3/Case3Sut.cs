namespace Hj.SutFactory.TestCase.Case3;

public class Case3Sut(IEnumerable<IInterfaceInput> interfaceInputValues)
{
  public IEnumerable<IInterfaceInput> InterfaceInputValues { get; } = interfaceInputValues;
}
