namespace Hj.SutFactory.UnitTest.TestData.Case3;

public class Case3Sut(
  ClassWithDependency classWithDependency,
  ClassUnknownInput classUnknownInput)
{
  public ClassWithDependency ClassWithDependency { get; } = classWithDependency;

  public ClassUnknownInput ClassUnknownInput { get; } = classUnknownInput;
}
