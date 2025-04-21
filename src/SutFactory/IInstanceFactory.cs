﻿// <copyright file="IInstanceFactory.cs" company="Henrik Jensen">
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

using Hj.SutFactory.Factories;

namespace Hj.SutFactory;

public interface IInstanceFactory
{
  ICtorInstanceFactory CtorInstanceFactory { get; }

  IPartialInstanceFactory PartialInstanceFactory { get; }

  ISubstituteFactory SubstituteFactory { get; }

  object AutoCreate(Type type);
}
