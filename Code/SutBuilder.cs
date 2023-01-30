using Hj.SutFactory.Factories;
using Hj.SutFactory.Factories.Implementation;
using Hj.SutFactory.Registries;
using Hj.SutFactory.Registries.Implementation;

namespace Hj.SutFactory;

public class SutBuilder
{
  private readonly IServiceProvider? _externalServiceProvider;

  private readonly Lazy<IInputRegistry> _inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory;
  private readonly IServiceProvider _serviceProvider;
  private readonly Lazy<InputBuilder> _inputBuilder;
  private readonly Lazy<SutBuilderAdvanced> _sutBuilderAdvanced;

  [ThreadStatic]
  private IDictionary<string, object?>? _registryBackingStore;

  public SutBuilder()
      : this(null)
  {
  }

  public SutBuilder(IServiceProvider? serviceProvider)
  {
    T GetService<T>(Func<T> factory)
        where T : class
    {
      if (serviceProvider?.GetService(typeof(T)) is not T service)
      {
        service = factory();
      }
      return service;
    }

    _externalServiceProvider = serviceProvider;
    _serviceProvider = new SutBuilderServiceProvider(this);

    _instanceFactory = new Lazy<IInstanceFactory>(() => GetService<IInstanceFactory>(() =>
    {
      var ctorInstanceFactory = GetService<ICtorInstanceFactory>(() => new CtorInstanceFactory(ServiceProvider));
      var partialInstanceFactory = GetService<IPartialInstanceFactory>(() => new NSubstitutePartialFactory(ServiceProvider));
      var substituteFactory = GetService<ISubstituteFactory>(() => new NSubstituteSubstituteFactory());
      return new InstanceFactory(ctorInstanceFactory, partialInstanceFactory, substituteFactory);
    }));

    _inputRegistry = new Lazy<IInputRegistry>(() => GetService<IInputRegistry>(() =>
    {
      var registryKeyGenerator = GetService<IRegistryKeyGenerator>(() => new RegistryKeyGenerator());
      _registryBackingStore = new Dictionary<string, object?>();
      return new InputRegistry(registryKeyGenerator, _registryBackingStore);
    }));

    _inputBuilder = new Lazy<InputBuilder>(() => new InputBuilder(_serviceProvider, _instanceFactory.Value, _inputRegistry.Value));
    _sutBuilderAdvanced = new Lazy<SutBuilderAdvanced>(() => new SutBuilderAdvanced(this));

    Initialize();
  }

  public InputBuilder InputBuilder => _inputBuilder.Value;

  public IServiceProvider ServiceProvider => _serviceProvider;

  public SutBuilderAdvanced Advanced => _sutBuilderAdvanced.Value;

  public virtual void Initialize()
  {
  }

  public T CreateSut<T>()
      where T : class => (T)_instanceFactory.Value.AutoCreate(typeof(T));

  public class SutBuilderAdvanced
  {
    private readonly SutBuilder _sutBuilder;

    public SutBuilderAdvanced(SutBuilder sutBuilder) => _sutBuilder = sutBuilder;

    public T Instance<T>() => (T)_sutBuilder._instanceFactory.Value.CtorInstanceFactory.Create(typeof(T));

    public T SubstitutePartsOf<T>() => (T)_sutBuilder._instanceFactory.Value.PartialInstanceFactory.Create(typeof(T));

    public T Substitute<T>() => (T)_sutBuilder._instanceFactory.Value.SubstituteFactory.Create(typeof(T));
  }

  public class SutBuilderServiceProvider : IServiceProvider
  {
    private readonly SutBuilder _sutBuilder;

    public SutBuilderServiceProvider(SutBuilder sutBuilder) => _sutBuilder = sutBuilder;

    public object? GetService(Type type)
    {
      var value = _sutBuilder._inputRegistry.Value.GetOrCreateValue<object?>(
          type,
          false,
          () => _sutBuilder._externalServiceProvider?.GetService(type) ?? _sutBuilder._instanceFactory.Value.AutoCreate(type));
      return value;
    }
  }
}
