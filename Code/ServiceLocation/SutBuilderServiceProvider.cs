namespace Hj.SutFactory.ServiceLocation;

public class SutBuilderServiceProvider(
  IServiceProvider? externalServiceProvider,
  Lazy<IInputRegistry> inputRegistry,
  Lazy<IInstanceFactory> instanceFactory) : IServiceProvider
{
  private readonly IServiceProvider? _externalServiceProvider = externalServiceProvider;
  private readonly Lazy<IInputRegistry> _inputRegistry = inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory = instanceFactory;

  public object? GetService(Type type)
  {
    var value = _externalServiceProvider?.GetService(type);

    if (value is null)
    {
      value = _inputRegistry.Value.GetOrCreateValue(
        type,
        type,
        false,
        () => _instanceFactory.Value.AutoCreate(type));
    }

    return value;
  }
}
