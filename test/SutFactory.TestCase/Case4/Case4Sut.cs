namespace Hj.SutFactory.TestCase.Case4;

public class Case4Sut(
  ClassAmbiguousCtorInput classAmbiguousCtorInput1,
  ISecondCtor classAmbiguousCtorInput2)
{
  public ClassAmbiguousCtorInput ClassAmbiguousCtorInput1 { get; } = classAmbiguousCtorInput1;

  public ISecondCtor ClassAmbiguousCtorInput2 { get; } = classAmbiguousCtorInput2;
}
