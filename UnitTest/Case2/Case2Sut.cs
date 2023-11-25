﻿namespace Hj.SutFactory.UnitTest.Case2;

public class Case2Sut(
  IInterfaceInput interfaceInputValue,
  ImplementationInput implementationInputValue)
{
  public IInterfaceInput InterfaceInputValue { get; } = interfaceInputValue;

  public ImplementationInput ImplementationInputValue { get; } = implementationInputValue;
}
