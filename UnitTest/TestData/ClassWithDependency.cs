namespace Hj.SutFactory.UnitTest.TestData;

public class ClassWithDependency
{
  public ClassWithDependency(IInterfaceInput interfaceInput)
  {
    InterfaceInput = interfaceInput;
  }

  public IInterfaceInput InterfaceInput { get; set; }
}
