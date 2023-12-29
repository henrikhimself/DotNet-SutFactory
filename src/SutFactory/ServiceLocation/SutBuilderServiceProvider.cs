// <copyright file="SutBuilderServiceProvider.cs" company="Henrik Jensen">
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

namespace Hj.SutFactory.ServiceLocation;

public class SutBuilderServiceProvider(
  IServiceProvider? externalServiceProvider,
  Lazy<IInputRegistry> inputRegistry,
  Lazy<IInstanceFactory> instanceFactory) : IServiceProvider
{
  private readonly IServiceProvider? _externalServiceProvider = externalServiceProvider;
  private readonly Lazy<IInputRegistry> _inputRegistry = inputRegistry;
  private readonly Lazy<IInstanceFactory> _instanceFactory = instanceFactory;

  public object? GetService(Type serviceType)
  {
    var externalService = _externalServiceProvider?.GetService(serviceType);
    if (externalService is not null)
    {
      return externalService;
    }

    // Service types without a configured input builder defaults to being Singletons. This is
    // useful for getting instances that was automically created during the resolving of constructor
    // parameters without having to configure these ahead of creating the SUT.
    var value = _inputRegistry.Value.GetOrCreateValue(
        serviceType,
        serviceType,
        true,
        () => _instanceFactory.Value.AutoCreate(serviceType));
    return value;
  }
}
