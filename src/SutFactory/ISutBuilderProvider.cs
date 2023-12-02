namespace Hj.SutFactory;

/// <summary>
/// Defines a provider that allows decorators to wrap a sut builder in order to preconfigure sut inputs.
/// </summary>
public interface ISutBuilderProvider
{
  SutBuilder GetSutBuilder();
}
