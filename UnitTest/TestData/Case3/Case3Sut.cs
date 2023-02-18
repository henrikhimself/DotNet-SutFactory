namespace Hj.SutFactory.UnitTest.TestData.Case3;

public class Case3Sut
{
  public Case3Sut(
    ClassWithDependency classWithDependency,
    ClassUnknownInput classUnknownInput)
  {
    ClassWithDependency = classWithDependency;
    ClassUnknownInput = classUnknownInput;
  }

  public ClassWithDependency ClassWithDependency { get; }

  public ClassUnknownInput ClassUnknownInput { get; }
}
