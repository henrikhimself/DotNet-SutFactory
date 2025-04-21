// <copyright file="NSubstitutePartialFactory.cs" company="Henrik Jensen">
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

using NSubstitute.Core;

namespace Hj.SutFactory.Factories.Implementation;

public sealed class NSubstitutePartialFactory(IServiceProvider serviceProvider) : FactoryBase, IPartialInstanceFactory
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  private readonly NSubstitute.Core.ISubstituteFactory _nSubstituteFactory = SubstitutionContext.Current.SubstituteFactory;

  public object Create(Type type)
  {
    var ctor = GetConstructorInfo(type);
    if (ctor == null)
    {
      throw new InvalidOperationException($"Cannot create parts of instance '{type.AssemblyQualifiedName}' due to missing or ambiguous constructors.");
    }

    var parameters = GetConstructorParameters(ctor, _serviceProvider);

    var instance = _nSubstituteFactory.CreatePartial([type], parameters!);
    return instance;
  }
}
