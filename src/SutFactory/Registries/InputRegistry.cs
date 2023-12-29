// <copyright file="InputRegistry.cs" company="Henrik Jensen">
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

using Hj.SutFactory.Registries.Implementation;

namespace Hj.SutFactory.Registries;

public class InputRegistry(
  IRegistryKeyGenerator registryKeyGenerator,
  InputCollection inputCollection) : IInputRegistry
{
  private readonly IRegistryKeyGenerator _registryKeyGenerator = registryKeyGenerator;
  private readonly InputCollection _inputCollection = inputCollection;

  public object? GetOrCreateValue(Type serviceType, Type implementationType, bool isSingleton, Func<object?> valueFactory)
  {
    if (!TryGet(serviceType, out var value))
    {
      _ = TrySet(serviceType, implementationType, isSingleton, valueFactory, out value);
    }

    return value;
  }

  public object? Get(Type serviceType)
  {
    _ = TryGet(serviceType, out var value);
    return value;
  }

  public void Set(Type serviceType, Type implementationType, bool isSingleton, Func<object?> valueFactory)
    => _ = TrySet(serviceType, implementationType, isSingleton, valueFactory, out var _);

  private bool TryGet(Type serviceType, out object? value)
  {
    value = default;

    var serviceTypeKey = _registryKeyGenerator.GenerateKey(serviceType);
    if (serviceTypeKey is not null)
    {
      if (_inputCollection.TryGet(serviceTypeKey, out value))
      {
        return true;
      }
    }

    if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
    {
      var genericType = serviceType.GetGenericArguments()[0];
      var genericTypeKey = _registryKeyGenerator.GenerateKey(genericType);
      if (genericTypeKey is null)
      {
        return false;
      }

      if (_inputCollection.TryGetList(genericTypeKey, genericType, out value))
      {
        return true;
      }
    }

    return false;
  }

  private bool TrySet(Type serviceType, Type implementationType, bool isSingleton, Func<object?> valueFactory, out object? value)
  {
    var serviceTypeKey = _registryKeyGenerator.GenerateKey(serviceType);
    if (serviceTypeKey is null)
    {
      value = null;
      return false;
    }

    var newValue = value = valueFactory();

    var interfaceKeys = implementationType.GetInterfaces()
        .Select(_registryKeyGenerator.GenerateKey);

    if (isSingleton)
    {
      _inputCollection.AddFactory(serviceTypeKey, interfaceKeys, () => newValue);
    }
    else
    {
      _inputCollection.AddFactory(serviceTypeKey, interfaceKeys, valueFactory);
    }

    return true;
  }
}
