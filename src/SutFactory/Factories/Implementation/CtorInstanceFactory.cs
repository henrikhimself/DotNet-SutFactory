// <copyright file="CtorInstanceFactory.cs" company="Henrik Jensen">
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

namespace Hj.SutFactory.Factories.Implementation;

public sealed class CtorInstanceFactory(IServiceProvider serviceProvider) : FactoryBase, ICtorInstanceFactory
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;

  public object Create(Type type)
  {
    var ctor = GetConstructorInfo(type)
      ?? throw new InvalidOperationException($"Cannot create instance '{type.AssemblyQualifiedName}' due to missing or ambiguous constructors.");

    var parameters = GetConstructorParameters(ctor, _serviceProvider);

    var instance = ctor.Invoke(parameters);
    return instance;
  }
}
