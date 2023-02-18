namespace Hj.SutFactory.ServiceLocation;

public class SutBuilderServiceProvider : IServiceProvider
{
  private readonly IServiceProvider? _externalServiceProvider;
  private readonly Lazy<IInputRegistry> _inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory;

  public SutBuilderServiceProvider(
    IServiceProvider? externalServiceProvider,
    Lazy<IInputRegistry> inputRegistry,
    Lazy<IInstanceFactory> instanceFactory)
  {
    _externalServiceProvider = externalServiceProvider;
    _inputRegistry = inputRegistry;
    _instanceFactory = instanceFactory;
  }

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
