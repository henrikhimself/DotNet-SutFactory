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
    var externalService = _externalServiceProvider?.GetService(serviceType);
    if (externalService is not null)
    {
      return externalService;
    }

    // Service types without a configured input builder defaults to being Singletons. This is
    // useful for getting instances that was automically created during the resolving of constructor
    // parameters without having to configure these ahead of creating the SUT.
    var value = _inputRegistry.Value.GetOrCreateValue(
        serviceType,
        serviceType,
        true,
        () => _instanceFactory.Value.AutoCreate(serviceType));
    return value;
  }
}
