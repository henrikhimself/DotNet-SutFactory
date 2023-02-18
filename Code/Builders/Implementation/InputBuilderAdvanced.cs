namespace Hj.SutFactory.Builders.Implementation;

public sealed class InputBuilderAdvanced
{
  private readonly IServiceProvider _serviceProvider;
  private readonly IInstanceFactory _instanceFactory;
  private readonly IInputRegistry _inputRegistry;

  public InputBuilderAdvanced(IServiceProvider serviceProvider, IInstanceFactory instanceFactory, IInputRegistry inputRegistry)
  {
    _serviceProvider = serviceProvider;
    _instanceFactory = instanceFactory;
    _inputRegistry = inputRegistry;
  }

  /// <summary>
  /// Creates an instance of type T as a concrete type.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <returns>An instance of type T.</returns>
  public InputBuilderConfigurator<T> Instance<T>()
    where T : class => Instance<T, T>();

  /// <summary>
  /// Creates an instance of type TImplementation as either a concrete type.
  /// </summary>
  /// <typeparam name="TInterface">Type of interface to register in service provider.</typeparam>
  /// <typeparam name="TImplementation">Type of instance to create.</typeparam>
  /// <returns>An instance configurator.</returns>
  public InputBuilderConfigurator<TInterface> Instance<TInterface, TImplementation>()
    where TInterface : class
    where TImplementation : class, TInterface
      => GetOrCreateValue<TInterface>(instanceFactory => instanceFactory.CtorInstanceFactory.Create(typeof(TImplementation)));

  /// <summary>
  /// Creates an instance of type T using an instance factory.
  /// Beware that using a factory is discouraged as it may make using a sut builder pointless. However, if an instance
  /// has multiple constructors or is somehow complex to create, this method can help with registering it as in input.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <param name="instanceFactory">A func that creates an instance of T.</param>
  /// <returns>An instance configurator.</returns>
  public InputBuilderConfigurator<T> Instance<T>(Func<T?>? instanceFactory)
    where T : class => Instance<T, T>(instanceFactory);

  /// <summary>
  /// Creates an instance of type T using an instance factory.
  /// Beware that using a factory is discouraged as it may make using a sut builder pointless. However, if an instance
  /// has multiple constructors or is somehow complex to create, this method can help with registering it as in input.
  /// </summary>
  /// <typeparam name="TInterface">Type of interface to register in service provider.</typeparam>
  /// <typeparam name="TImplementation">Type of instance to create.</typeparam>
  /// <param name="instanceFactory">A func that creates an instance of T.</param>
  /// <returns>An instance configurator.</returns>
  public InputBuilderConfigurator<TImplementation> Instance<TInterface, TImplementation>(Func<TImplementation?>? instanceFactory)
    where TInterface : class
    where TImplementation : class, TInterface
  {
    var value = _inputRegistry.GetOrCreateValue(
      typeof(TInterface),
      typeof(TImplementation),
      true,
      instanceFactory ??= () => null);
    return new InputBuilderConfigurator<TImplementation>(_serviceProvider, value);
  }

  /// <summary>
  /// Creates an instance of type T as a partial substitute.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <returns>An instance of type T.</returns>
  public InputBuilderConfigurator<T> SubstitutePartsOf<T>()
    where T : class => GetOrCreateValue<T>(instanceFactory => instanceFactory.PartialInstanceFactory.Create(typeof(T)));

  /// <summary>
  /// Creates an instance of type T as a substitute.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <returns>An instance of type T.</returns>
  public InputBuilderConfigurator<T> Substitute<T>()
    where T : class => GetOrCreateValue<T>(instanceFactory => instanceFactory.SubstituteFactory.Create(typeof(T)));

  private InputBuilderConfigurator<T> GetOrCreateValue<T>(Func<IInstanceFactory, object> valueFactory)
    where T : class
  {
    var value = _inputRegistry.GetOrCreateValue(
      typeof(T),
      typeof(T),
      true,
      () => valueFactory(_instanceFactory));
    return new InputBuilderConfigurator<T>(_serviceProvider, value);
  }
}
