namespace Hj.SutFactory.UnitTest.TestData;

public class ClassGenericInput<T>
{
  public ClassGenericInput(T t) => TValue = t;

  public T TValue { get; }
}
