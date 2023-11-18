namespace Hj.SutFactory.UnitTest.TestData.Case2;

public class Case2Sut(
  ClassAbstractInput classAbstractInput,
  ClassGenericInput<object> classGenericInput,
  ClassInput classInput,
  ClassNestedClassInput.ClassInput classNestedClassInput,
  ClassSealedInput classSealedInput)
{
  public ClassAbstractInput ClassAbstractInputValue { get; } = classAbstractInput;

  public ClassGenericInput<object> ClassGenericValue { get; } = classGenericInput;

  public ClassInput ClassInputValue { get; } = classInput;

  public ClassNestedClassInput.ClassInput ClassNestedClassInput { get; } = classNestedClassInput;

  public ClassSealedInput ClassSealedInputValue { get; } = classSealedInput;
}
