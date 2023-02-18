namespace Hj.SutFactory.UnitTest.TestData.Case7;

public class Case7Sut
{
  public Case7Sut(
    InstanceInput instance,
    ClassPartialSubstituteInput classPartialSubstituteInput,
    IInterfaceSubstituteInput interfaceSubstituteInput)
  {
    InstanceInput = instance;
    ClassPartialSubstituteInput = classPartialSubstituteInput;
    IInterfaceSubstituteInput = interfaceSubstituteInput;
  }

  public InstanceInput InstanceInput { get; }

  public ClassPartialSubstituteInput ClassPartialSubstituteInput { get; }

  public IInterfaceSubstituteInput IInterfaceSubstituteInput { get; }
}
