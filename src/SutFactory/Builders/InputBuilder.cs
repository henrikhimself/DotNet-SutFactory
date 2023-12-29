// <copyright file="InputBuilder.cs" company="Henrik Jensen">
// Copyright 2024 Henrik Jensen
//
// Licensed under the Apache License, Version 2.0 (the "License")
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace Hj.SutFactory.Builders;

public sealed class InputBuilder(
  IInstanceFactory instanceFactory,
  IInputRegistry inputRegistry) : IServiceProvider
{
  private readonly IInstanceFactory _instanceFactory = instanceFactory;
  private readonly IInputRegistry _inputRegistry = inputRegistry;

  /// <summary>
  /// Gets advanced methods that provide more control of how instances is created.
  /// </summary>
  public InputBuilderAdvanced Advanced { get; } = new InputBuilderAdvanced(instanceFactory, inputRegistry);

  /// <summary>
  /// Explicitly registers NULL as type T.
  /// </summary>
  /// <typeparam name="T">Type of instance to register.</typeparam>
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
  /// <typeparam name="T">Type of instance to create and register.</typeparam>
  /// <returns>An instance of type T.</returns>
  public T Instance<T>()
    where T : class => Instance<T, T>();

  /// <summary>
  /// Creates an instance of type TImplementation as either a concrete type, a partial substitute or as a substitute.
  /// </summary>
  /// <typeparam name="TInterface">Type of interface to register.</typeparam>
  /// <typeparam name="TImplementation">Type of instance to create.</typeparam>
  /// <returns>An instance configurator.</returns>
  public TInterface Instance<TInterface, TImplementation>()
    where TInterface : class
    where TImplementation : class, TInterface
  {
    var value = _inputRegistry.GetOrCreateValue(
      typeof(TInterface),
      typeof(TImplementation),
      true,
      () => (TImplementation)_instanceFactory.AutoCreate(typeof(TImplementation)))!;
    return (TInterface)value;
  }

  public object? GetService(Type serviceType) => _inputRegistry.Get(serviceType);
}
