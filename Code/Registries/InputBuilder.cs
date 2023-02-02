namespace Hj.SutFactory.Registries;

public sealed class InputBuilder
{
  private readonly IServiceProvider _serviceProvider;
  private readonly IInstanceFactory _instanceFactory;
  private readonly IInputRegistry _inputRegistry;

  public InputBuilder(IServiceProvider serviceProvider, IInstanceFactory instanceFactory, IInputRegistry inputRegistry)
  {
    _serviceProvider = serviceProvider;
    _instanceFactory = instanceFactory;
    _inputRegistry = inputRegistry;

    Advanced = new InputBuilderAdvanced(serviceProvider, instanceFactory, inputRegistry);
  }

  public InputBuilderAdvanced Advanced { get; }

  public InputBuilderConfigurator<T> AutoInstance<T>()
      where T : class => AutoInstance<T, T>();

  public InputBuilderConfigurator<TInterface> AutoInstance<TInterface, TImplementation>()
      where TInterface : class
      where TImplementation : class, TInterface
  {
    var value = _inputRegistry.GetOrCreateValue(
        typeof(TInterface),
        true,
        () => (TImplementation)_instanceFactory.AutoCreate(typeof(TImplementation)));
    return new InputBuilderConfigurator<TInterface>(_serviceProvider, value);
  }

  public InputBuilderConfigurator<T> Instance<T>(Func<T?>? instanceFactory)
      where T : class
  {
    var value = _inputRegistry.GetOrCreateValue(
        typeof(T),
        true,
        instanceFactory ??= () => null);
    return new InputBuilderConfigurator<T>(_serviceProvider, value);
  }
}
