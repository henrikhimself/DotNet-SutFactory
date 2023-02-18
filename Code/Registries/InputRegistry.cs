using Hj.SutFactory.Registries.Implementation;

namespace Hj.SutFactory.Registries;

public class InputRegistry : IInputRegistry
{
  private readonly IRegistryKeyGenerator _registryKeyGenerator;
  private readonly InputCollection _inputCollection;

  public InputRegistry(
      IRegistryKeyGenerator registryKeyGenerator,
      InputCollection inputCollection)
  {
    _registryKeyGenerator = registryKeyGenerator;
    _inputCollection = inputCollection;
  }

  public object? GetOrCreateValue(Type serviceType, Type implementationType, bool isSingleton, Func<object?> valueFactory)
  {
    object? value = null;

    var serviceTypeKey = _registryKeyGenerator.GenerateKey(serviceType);
    if (serviceTypeKey is not null)
    {
      if (!TryGet(serviceTypeKey, serviceType, out value))
      {
        value = Create(serviceTypeKey, implementationType, isSingleton, valueFactory);
      }
    }

    return value;
  }

  private bool TryGet(string typeKey, Type serviceType, out object? value)
  {
    value = default;

    if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
    {
      var genericTypeKey = _registryKeyGenerator.GenerateKey(serviceType.GetGenericArguments()[0]);
      if (genericTypeKey is null)
      {
        return false;
      }
      var valueList = _inputCollection.GetAllFactories(genericTypeKey)
        .Select(valueFactory => valueFactory())
        .AsEnumerable();

      value = valueList;
      return true;
    }

    if (_inputCollection.TryGetFactory(typeKey, out var registeredValueFactory))
    {
      value = registeredValueFactory();
      return true;
    }

    return false;
  }

  private object? Create(string typeKey, Type implementationType, bool isSingleton, Func<object?> valueFactory)
  {
    var value = valueFactory();

    var interfaceKeys = implementationType.GetInterfaces()
        .Select(interfaceType => _registryKeyGenerator.GenerateKey(interfaceType));
    if (isSingleton)
    {
      _inputCollection.Add(typeKey, interfaceKeys, () => value);
    }
    else
    {
      _inputCollection.Add(typeKey, interfaceKeys, valueFactory);
    }

    return value;
  }
}
