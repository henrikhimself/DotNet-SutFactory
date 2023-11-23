using Hj.SutFactory.Builders.Implementation;

namespace Hj.SutFactory.Builders;

public sealed class InputBuilder(
  IServiceProvider serviceProvider,
  IInstanceFactory instanceFactory,
  IInputRegistry inputRegistry) : IServiceProvider
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  private readonly IInstanceFactory _instanceFactory = instanceFactory;
  private readonly IInputRegistry _inputRegistry = inputRegistry;

  /// <summary>
  /// Gets advanced methods that provide more control of how instances is created.
  /// </summary>
  public InputBuilderAdvanced Advanced { get; } = new InputBuilderAdvanced(serviceProvider, instanceFactory, inputRegistry);

  public object? GetService(Type serviceType) => _inputRegistry.Get(serviceType);

  /// <summary>
  /// Explicitly registers NULL as type T.
  /// </summary>
  /// <typeparam name="T">An instance of type T.</typeparam>
  public void Null<T>()
  {
    _ = _inputRegistry.GetOrCreateValue(
      typeof(T),
      typeof(T),
      false,
      () => null);
  }

  /// <summary>
  /// Creates an instance of type T as either a concrete type, a partial substitute or as a substitute.
  /// </summary>
  /// <typeparam name="T">Type of instance to create and register in service provider.</typeparam>
  /// <returns>An instance configurator.</returns>
  public InputBuilderConfigurator<T> Instance<T>()
    where T : class => Instance<T, T>();

  /// <summary>
  /// Creates an instance of type TImplementation as either a concrete type, a partial substitute or as a substitute.
  /// </summary>
  /// <typeparam name="TInterface">Type of interface to register in service provider.</typeparam>
  /// <typeparam name="TImplementation">Type of instance to create.</typeparam>
  /// <returns>An instance configurator.</returns>
  public InputBuilderConfigurator<TInterface> Instance<TInterface, TImplementation>()
    where TInterface : class
    where TImplementation : class, TInterface
  {
    var value = _inputRegistry.GetOrCreateValue(
      typeof(TInterface),
      typeof(TImplementation),
      true,
      () => (TImplementation)_instanceFactory.AutoCreate(typeof(TImplementation)))!;
    return new InputBuilderConfigurator<TInterface>(_serviceProvider, value);
  }
}
