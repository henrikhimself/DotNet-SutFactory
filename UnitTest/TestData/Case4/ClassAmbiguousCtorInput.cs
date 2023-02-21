﻿namespace Hj.SutFactory.UnitTest.TestData.Case4;

public class ClassAmbiguousCtorInput : ISecondCtor
{
  public ClassAmbiguousCtorInput(object o1) => OValue1 = o1;

  public ClassAmbiguousCtorInput(object o1, object o2)
  {
    OValue1 = o1;
    OValue2 = o2;
  }

  public object? OValue1 { get; }

  public object? OValue2 { get; }
}