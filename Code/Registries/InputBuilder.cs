﻿namespace Hj.SutFactory.Registries;

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

  /// <summary>
  /// Gets advanced methods that provide more control of how instances is created.
  /// </summary>
  public InputBuilderAdvanced Advanced { get; }

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
      true,
      () => (TImplementation)_instanceFactory.AutoCreate(typeof(TImplementation)));
    return new InputBuilderConfigurator<TInterface>(_serviceProvider, value);
  }

  /// <summary>
  /// Adds an already constructed instance.
  /// </summary>
  /// <typeparam name="T">Type of T to register in service provider.</typeparam>
  /// <param name="instance">An instance.</param>
  public void Instance<T>(T instance)
  {
    _ = _inputRegistry.GetOrCreateValue(
      typeof(T),
      true,
      () => instance);
  }
}
