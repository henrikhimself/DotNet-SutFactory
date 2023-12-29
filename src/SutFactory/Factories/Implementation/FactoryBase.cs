// <copyright file="FactoryBase.cs" company="Henrik Jensen">
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

using System.Reflection;

namespace Hj.SutFactory.Factories.Implementation;

public abstract class FactoryBase
{
  protected FactoryBase()
  {
  }

  protected static ConstructorInfo? GetConstructorInfo(Type type)
  {
    var typeInfo = type.GetTypeInfo();
    var ctors = typeInfo.DeclaredConstructors
        .Where(ctor => !ctor.IsStatic && ctor.IsPublic)
        .ToList();

    var ctor = ctors.Count == 1
        ? ctors.Single()
        : ctors.FirstOrDefault(x => x.GetParameters().Length == 0);
    return ctor;
  }

  protected static object?[] GetConstructorParameters(ConstructorInfo ctor, IServiceProvider serviceProvider)
  {
    var parameterTypes = ctor.GetParameters()
        .Select(x => x.ParameterType)
        .ToArray();
    var parametersCount = parameterTypes.Length;
    var parameters = new object?[parametersCount];

    if (parametersCount == 1)
    {
      ResolveOneParameter(parameterTypes, parameters, serviceProvider);
    }
    else
    {
      ResolveManyParameters(parameterTypes, parameters, serviceProvider);
    }

    return parameters;
  }

  private static void ResolveOneParameter(Type[] parameterTypes, object?[] parameters, IServiceProvider serviceProvider)
    => parameters[0] = serviceProvider.GetService(parameterTypes[0]);

  private static void ResolveManyParameters(Type[] parameterTypes, object?[] parameters, IServiceProvider serviceProvider)
  {
    void SetInstance(int parameterIndex, Type serviceType)
    {
      var instance = serviceProvider.GetService(serviceType);
      parameters[parameterIndex] = instance;
    }

    // Defer contract types as registering implementations first may add implicit registrations
    // that can be reused later.
    for (var i = 0; i < parameterTypes.Length; i++)
    {
      var parameterType = parameterTypes[i];
      var isContract = InstanceHelper.IsContract(parameterType);
      if (isContract)
      {
        parameters[i] = new Deferred(i, parameterType);
      }
      else
      {
        SetInstance(i, parameterType);
      }
    }

    // Set deferred registrations.
    for (var i = 0; i < parameterTypes.Length; i++)
    {
      if (parameters[i] is Deferred deferred)
      {
        SetInstance(deferred.ParameterIndex, deferred.ParameterType);
      }
    }
  }

  internal readonly struct Deferred(int parameterIndex, Type parameterType)
  {
    internal int ParameterIndex { get; } = parameterIndex;

    internal Type ParameterType { get; } = parameterType;
  }
}
