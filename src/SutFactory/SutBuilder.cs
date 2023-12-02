using Hj.SutFactory.Builders;
using Hj.SutFactory.Factories;
using Hj.SutFactory.Factories.Implementation;
using Hj.SutFactory.Registries;
using Hj.SutFactory.Registries.Implementation;
using Hj.SutFactory.ServiceLocation;

namespace Hj.SutFactory;

public class SutBuilder : ISutBuilderProvider, IServiceProvider
{
  private readonly InputCollection _registryInputCollection = new();
  private readonly IServiceProvider _sutBuilderServiceProvider;
  private readonly Lazy<IInputRegistry> _inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory;
  private readonly Lazy<InputBuilder> _inputBuilder;
  private readonly Lazy<SutBuilderAdvanced> _sutBuilderAdvanced;

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

    IServiceProvider GetSutBuilderServiceProvider() => _sutBuilderServiceProvider!;

    _instanceFactory = new Lazy<IInstanceFactory>(() => GetService<IInstanceFactory>(() =>
    {
      var ctorInstanceFactory = GetService<ICtorInstanceFactory>(() => new CtorInstanceFactory(GetSutBuilderServiceProvider()));
      var partialInstanceFactory = GetService<IPartialInstanceFactory>(() => new NSubstitutePartialFactory(GetSutBuilderServiceProvider()));
      var substituteFactory = GetService<ISubstituteFactory>(() => new NSubstituteSubstituteFactory());
      return new InstanceFactory(ctorInstanceFactory, partialInstanceFactory, substituteFactory);
    }));

    _inputRegistry = new Lazy<IInputRegistry>(() => GetService<IInputRegistry>(() =>
    {
      var registryKeyGenerator = GetService<IRegistryKeyGenerator>(() => new RegistryKeyGenerator());
      return new InputRegistry(registryKeyGenerator, _registryInputCollection);
    }));

    _sutBuilderServiceProvider = new SutBuilderServiceProvider(externalServiceProvider, _inputRegistry, _instanceFactory);

    _inputBuilder = new Lazy<InputBuilder>(() => new InputBuilder(_instanceFactory.Value, _inputRegistry.Value));
    _sutBuilderAdvanced = new Lazy<SutBuilderAdvanced>(() => new SutBuilderAdvanced(this));
  }

  public InputBuilder InputBuilder => _inputBuilder.Value;

  public SutBuilderAdvanced Advanced => _sutBuilderAdvanced.Value;

  public object? GetService(Type serviceType) => _inputRegistry.Value.Get(serviceType);

  public T CreateSut<T>()
      where T : class => (T)_instanceFactory.Value.AutoCreate(typeof(T));

  public SutBuilder GetSutBuilder() => this;

  public class SutBuilderAdvanced(SutBuilder sutBuilder)
  {
    private readonly SutBuilder _sutBuilder = sutBuilder;

    public T Instance<T>() => (T)_sutBuilder._instanceFactory.Value.CtorInstanceFactory.Create(typeof(T));

    public T SubstitutePartsOf<T>() => (T)_sutBuilder._instanceFactory.Value.PartialInstanceFactory.Create(typeof(T));

    public T Substitute<T>() => (T)_sutBuilder._instanceFactory.Value.SubstituteFactory.Create(typeof(T));
  }
}
