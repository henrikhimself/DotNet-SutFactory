namespace Hj.SutFactory.UnitTest.TestData.Case5;

public class Case5Sut
{
  public Case5Sut(IEnumerable<IHandlerInput> enumerableInput)
  {
    EnumerableInput = enumerableInput;
  }

  public IEnumerable<IHandlerInput> EnumerableInput { get; }
}
