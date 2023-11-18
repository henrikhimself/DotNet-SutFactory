namespace Hj.SutFactory.UnitTest.TestData.Case7;

public class Case7Sut(
  InstanceInput instance,
  ClassPartialSubstituteInput classPartialSubstituteInput,
  IInterfaceSubstituteInput interfaceSubstituteInput)
{
  public InstanceInput InstanceInput { get; } = instance;

  public ClassPartialSubstituteInput ClassPartialSubstituteInput { get; } = classPartialSubstituteInput;

  public IInterfaceSubstituteInput IInterfaceSubstituteInput { get; } = interfaceSubstituteInput;
}
