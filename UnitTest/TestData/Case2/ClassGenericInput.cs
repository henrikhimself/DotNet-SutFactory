namespace Hj.SutFactory.UnitTest.TestData.Case2;

public class ClassGenericInput<T>
{
  public ClassGenericInput(T t) => TValue = t;

  public T TValue { get; }
}
