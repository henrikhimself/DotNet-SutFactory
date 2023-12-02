namespace Hj.SutFactory.Builders;

public sealed class InputBuilderAdvanced(
  IInstanceFactory instanceFactory,
  IInputRegistry inputRegistry) : IServiceProvider
{
  private readonly IInstanceFactory _instanceFactory = instanceFactory;
  private readonly IInputRegistry _inputRegistry = inputRegistry;

  /// <summary>
  /// Creates an instance of type T as a concrete type.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <returns>An instance.</returns>
  public T Instance<T>()
    where T : class => Instance<T, T>();

  /// <summary>
  /// Creates an instance of type TImplementation.
  /// </summary>
  /// <typeparam name="TInterface">Type of interface to register in service provider.</typeparam>
  /// <typeparam name="TImplementation">Type of instance to create.</typeparam>
  /// <returns>An instance.</returns>
  public TInterface Instance<TInterface, TImplementation>()
    where TInterface : class
    where TImplementation : class, TInterface
      => GetOrCreateValue<TInterface>(instanceFactory => instanceFactory.CtorInstanceFactory.Create(typeof(TImplementation)));

  /// <summary>
  /// Creates an instance of type T as a partial substitute.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <returns>An instance.</returns>
  public T SubstitutePartsOf<T>()
    where T : class => GetOrCreateValue<T>(instanceFactory => instanceFactory.PartialInstanceFactory.Create(typeof(T)));

  /// <summary>
  /// Creates an instance of type T as a substitute.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <returns>An instance.</returns>
  public T Substitute<T>()
    where T : class => GetOrCreateValue<T>(instanceFactory => instanceFactory.SubstituteFactory.Create(typeof(T)));

  /// <summary>
  /// Creates an instance of type T using an instance factory.
  /// Beware that using a factory is discouraged as it may make using a sut builder pointless. However, if an instance
  /// has multiple constructors or is somehow complex to create, this method can help with registering it as in input.
  /// </summary>
  /// <typeparam name="T">Type of instance to create.</typeparam>
  /// <param name="customInstanceFactory">A func that creates an instance of T.</param>
  /// <returns>An instance.</returns>
  public T Instance<T>(Func<T> customInstanceFactory)
    where T : class => Instance<T, T>(customInstanceFactory);

  /// <summary>
  /// Creates an instance of type T using an instance factory.
  /// Beware that using a factory is discouraged as it may make using a sut builder pointless. However, if an instance
  /// has multiple constructors or is somehow complex to create, this method can help with registering it as in input.
  /// </summary>
  /// <typeparam name="TInterface">Type of interface to register in service provider.</typeparam>
  /// <typeparam name="TImplementation">Type of instance to create.</typeparam>
  /// <param name="customInstanceFactory">A func that creates an instance of T.</param>
  /// <returns>An instance.</returns>
  public TImplementation Instance<TInterface, TImplementation>(Func<TImplementation> customInstanceFactory)
    where TInterface : class
    where TImplementation : class, TInterface
      => GetOrCreateValue<TImplementation>(_ => customInstanceFactory());

  public object? GetService(Type serviceType) => _inputRegistry.Get(serviceType);

  private T GetOrCreateValue<T>(Func<IInstanceFactory, object> valueFactory)
    where T : class
  {
    var value = _inputRegistry.GetOrCreateValue(
      typeof(T),
      typeof(T),
      true,
      () => valueFactory(_instanceFactory))!;
    return (T)value;
  }
}
