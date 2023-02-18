using Hj.SutFactory.Builders;
using Hj.SutFactory.Factories;
using Hj.SutFactory.Factories.Implementation;
using Hj.SutFactory.Registries;
using Hj.SutFactory.Registries.Implementation;
using Hj.SutFactory.ServiceLocation;

namespace Hj.SutFactory;

public class SutBuilder : ISutBuilderProvider
{
  private readonly IServiceProvider _serviceProvider;
  private readonly Lazy<IInputRegistry> _inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory;
  private readonly Lazy<InputBuilder> _inputBuilder;
  private readonly Lazy<SutBuilderAdvanced> _sutBuilderAdvanced;

  [ThreadStatic]
  private InputCollection? _registryInputCollection;

  public SutBuilder()
      : this(null)
  {
  }

  public SutBuilder(IServiceProvider? externalServiceProvider)
  {
    T GetService<T>(Func<T> factory)
        where T : class
    {
      if (externalServiceProvider?.GetService(typeof(T)) is not T service)
      {
        service = factory();
      }
      return service;
    }

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
      _registryInputCollection = new InputCollection();
      return new InputRegistry(registryKeyGenerator, _registryInputCollection);
    }));

    _serviceProvider = new SutBuilderServiceProvider(externalServiceProvider, _inputRegistry, _instanceFactory);

    _inputBuilder = new Lazy<InputBuilder>(() => new InputBuilder(_serviceProvider, _instanceFactory.Value, _inputRegistry.Value));
    _sutBuilderAdvanced = new Lazy<SutBuilderAdvanced>(() => new SutBuilderAdvanced(this));
  }

  public InputBuilder InputBuilder => _inputBuilder.Value;

  public IServiceProvider ServiceProvider => _serviceProvider;

  public SutBuilderAdvanced Advanced => _sutBuilderAdvanced.Value;

  public T CreateSut<T>()
      where T : class => (T)_instanceFactory.Value.AutoCreate(typeof(T));

  public SutBuilder GetSutBuilder() => this;

  public class SutBuilderAdvanced
  {
    private readonly SutBuilder _sutBuilder;

    public SutBuilderAdvanced(SutBuilder sutBuilder) => _sutBuilder = sutBuilder;

    public T Instance<T>() => (T)_sutBuilder._instanceFactory.Value.CtorInstanceFactory.Create(typeof(T));

    public T SubstitutePartsOf<T>() => (T)_sutBuilder._instanceFactory.Value.PartialInstanceFactory.Create(typeof(T));

    public T Substitute<T>() => (T)_sutBuilder._instanceFactory.Value.SubstituteFactory.Create(typeof(T));
  }
}
