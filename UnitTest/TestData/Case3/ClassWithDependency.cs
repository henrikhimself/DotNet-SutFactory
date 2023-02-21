﻿namespace Hj.SutFactory.UnitTest.TestData.Case3;

/// <summary>
/// An input can depend on other inputs that all gets resolved and injected.
/// </summary>
public class ClassWithDependency
{
  public ClassWithDependency(ClassKnownInput classKnownInput, ClassUnknownInput classUnknownInput)
  {
    ClassKnownInput = classKnownInput;
    ClassUnknownInput = classUnknownInput;
  }

  public ClassKnownInput ClassKnownInput { get; }

  public ClassUnknownInput ClassUnknownInput { get; }
}