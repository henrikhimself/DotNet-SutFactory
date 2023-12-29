﻿// <copyright file="InstanceFactory.cs" company="Henrik Jensen">
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

namespace Hj.SutFactory.Factories;

public class InstanceFactory(
  ICtorInstanceFactory ctorInstanceFactory,
  IPartialInstanceFactory partialInstanceFactory,
  ISubstituteFactory substituteFactory) : IInstanceFactory
{
  public ICtorInstanceFactory CtorInstanceFactory { get; } = ctorInstanceFactory;

  public IPartialInstanceFactory PartialInstanceFactory { get; } = partialInstanceFactory;

  public ISubstituteFactory SubstituteFactory { get; } = substituteFactory;

  public object AutoCreate(Type type)
  {
    object instance;

    if (InstanceHelper.IsImplementation(type))
    {
      if (type.IsSealed)
      {
        instance = CtorInstanceFactory.Create(type);
      }
      else
      {
        instance = PartialInstanceFactory.Create(type);
      }
    }
    else
    {
      instance = SubstituteFactory.Create(type);
    }

    return instance;
  }
}
