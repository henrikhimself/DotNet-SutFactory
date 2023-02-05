namespace Hj.SutFactory.ServiceLocation;

public class SutBuilderServiceProvider : ISutBuilderServiceProvider
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
    var value = _inputRegistry.Value.GetOrCreateValue<object?>(
        type,
        false,
        () => _externalServiceProvider?.GetService(type) ?? _instanceFactory.Value.AutoCreate(type));
    return value;
  }

  public T GetService<T>() => (T)GetService(typeof(T))!;
}
