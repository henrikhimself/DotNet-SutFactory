namespace Hj.SutFactory.ServiceLocation;

public class SutBuilderServiceProvider(
  IServiceProvider? externalServiceProvider,
  Lazy<IInputRegistry> inputRegistry,
  Lazy<IInstanceFactory> instanceFactory) : IServiceProvider
{
  private readonly IServiceProvider? _externalServiceProvider = externalServiceProvider;
  private readonly Lazy<IInputRegistry> _inputRegistry = inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory = instanceFactory;

  public object? GetService(Type serviceType)
  {
    var value = _externalServiceProvider?.GetService(serviceType) ?? _inputRegistry.Value.GetOrCreateValue(
        serviceType,
        serviceType,
        false,
        () => _instanceFactory.Value.AutoCreate(serviceType));
    return value;
  }
}
