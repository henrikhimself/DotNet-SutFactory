namespace Hj.SutFactory.UnitTest.TestData.Case2;

public class Case2Sut
{
  public Case2Sut(
    ClassAbstractInput classAbstractInput,
    ClassGenericInput<object> classGenericInput,
    ClassInput classInput,
    ClassNestedClassInput.ClassInput classNestedClassInput,
    ClassSealedInput classSealedInput)
  {
    ClassAbstractInputValue = classAbstractInput;
    ClassGenericValue = classGenericInput;
    ClassInputValue = classInput;
    ClassNestedClassInput = classNestedClassInput;
    ClassSealedInputValue = classSealedInput;
  }

  public ClassAbstractInput ClassAbstractInputValue { get; }

  public ClassGenericInput<object> ClassGenericValue { get; }

  public ClassInput ClassInputValue { get; }

  public ClassNestedClassInput.ClassInput ClassNestedClassInput { get; }

  public ClassSealedInput ClassSealedInputValue { get; }
}
