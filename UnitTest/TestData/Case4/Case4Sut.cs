namespace Hj.SutFactory.UnitTest.TestData.Case4;

public class Case4Sut
{
  public Case4Sut(ClassAmbiguousCtorInput classAmbiguousCtorInput1, ISecondCtor classAmbiguousCtorInput2)
  {
    ClassAmbiguousCtorInput1 = classAmbiguousCtorInput1;
    ClassAmbiguousCtorInput2 = classAmbiguousCtorInput2;
  }

  public ClassAmbiguousCtorInput ClassAmbiguousCtorInput1 { get; }

  public ISecondCtor ClassAmbiguousCtorInput2 { get; }
}
