namespace Hj.SutFactory.TestCase.Case4;

public class ClassAmbiguousCtorInput(
  object? o1,
  object? o2) : ISecondCtor
{
  public ClassAmbiguousCtorInput(object o1)
    : this(o1, null)
  {
  }

  public object? OValue1 { get; } = o1;

  public object? OValue2 { get; } = o2;
}
