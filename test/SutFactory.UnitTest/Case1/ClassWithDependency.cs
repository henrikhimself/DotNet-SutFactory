namespace Hj.SutFactory.UnitTest.Case1;

public class ClassWithDependency(ClassInput classInput)
{
  public ClassInput ClassInputValue { get; } = classInput;
}
