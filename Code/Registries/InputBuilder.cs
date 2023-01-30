namespace Hj.SutFactory.Registries;

public sealed class InputBuilder
{
  private readonly IServiceProvider _serviceProvider;
  private readonly IInstanceFactory _instanceFactory;
  private readonly IInputRegistry _inputRegistry;

  private readonly Lazy<InputBuilderAdvanced> _inputBuilderAdvanced;

  public InputBuilder(IServiceProvider serviceProvider, IInstanceFactory instanceFactory, IInputRegistry inputRegistry)
  {
    _serviceProvider = serviceProvider;
    _instanceFactory = instanceFactory;
    _inputRegistry = inputRegistry;

    _inputBuilderAdvanced = new Lazy<InputBuilderAdvanced>(() => new InputBuilderAdvanced(this));
  }

  public InputBuilderAdvanced Advanced => _inputBuilderAdvanced.Value;

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
    return new InputBuilderConfigurator<TInterface>(this, value);
  }

  public InputBuilderConfigurator<T> Instance<T>(Func<T?>? instanceFactory)
      where T : class
  {
    var value = _inputRegistry.GetOrCreateValue(
        typeof(T),
        true,
        instanceFactory ??= () => null);
    return new InputBuilderConfigurator<T>(this, value);
  }

  public class InputBuilderAdvanced
  {
    private readonly InputBuilder _inputBuilder;

    public InputBuilderAdvanced(InputBuilder inputBuilder) => _inputBuilder = inputBuilder;

    public InputBuilderConfigurator<T> Instance<T>()
        where T : class => Instance<T, T>();

    public InputBuilderConfigurator<TInterface> Instance<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
        => GetOrCreateValue<TInterface>(instanceFactory => instanceFactory.CtorInstanceFactory.Create(typeof(TImplementation)));

    public InputBuilderConfigurator<T> SubstitutePartsOf<T>()
        where T : class
        => GetOrCreateValue<T>(instanceFactory => instanceFactory.PartialInstanceFactory.Create(typeof(T)));

    public InputBuilderConfigurator<T> Substitute<T>()
        where T : class
        => GetOrCreateValue<T>(instanceFactory => instanceFactory.SubstituteFactory.Create(typeof(T)));

    private InputBuilderConfigurator<T> GetOrCreateValue<T>(Func<IInstanceFactory, object> valueFactory)
        where T : class
    {
      var value = (T?)_inputBuilder._inputRegistry.GetOrCreateValue(
          typeof(T),
          true,
          () => valueFactory(_inputBuilder._instanceFactory));
      return new InputBuilderConfigurator<T>(_inputBuilder, value);
    }
  }

  public class InputBuilderConfigurator<T>
      where T : class
  {
    private readonly InputBuilder _inputBuilder;
    private readonly T? _value;

    public InputBuilderConfigurator(InputBuilder inputBuilder, T? value)
    {
      _inputBuilder = inputBuilder;
      _value = value;
    }

    public void Configure(Action<T> action)
    {
      if (_value is not null && action is not null)
      {
        action(_value!);
      }
    }

    public void Configure(Action<T, IServiceProvider> action)
    {
      if (_value is not null && action is not null)
      {
        action(_value!, _inputBuilder._serviceProvider);
      }
    }
  }
}
