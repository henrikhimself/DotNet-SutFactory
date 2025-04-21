namespace Hj.SutFactory.TestCase.Case4;

public class Case4AmbiguousCtorSut : ISecondCtor
{
  public Case4AmbiguousCtorSut(object o1) => OValue1 = o1;

  public Case4AmbiguousCtorSut(object o1, object o2)
  {
    OValue1 = o1;
    OValue2 = o2;
  }

  public object? OValue1 { get; }

  public object? OValue2 { get; }
}
