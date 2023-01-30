namespace Hj.SutFactory.UnitTest.TestData;

public class TestAmbiguousCtorSut
{
  public TestAmbiguousCtorSut(object o1) => OValue1 = o1;

  public TestAmbiguousCtorSut(object o1, object o2)
  {
    OValue1 = o1;
    OValue2 = o2;
  }

  public object? OValue1 { get; }

  public object? OValue2 { get; }
}
