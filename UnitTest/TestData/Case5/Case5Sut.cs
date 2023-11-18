namespace Hj.SutFactory.UnitTest.TestData.Case5;

public class Case5Sut(IEnumerable<IHandlerInput> enumerableInput)
{
  public IEnumerable<IHandlerInput> EnumerableInput { get; } = enumerableInput;
}
