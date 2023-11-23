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

    var serviceTypeKey = _registryKeyGenerator.GenerateKey(serviceType);
    if (serviceTypeKey is not null)
    {
      if (_inputCollection.TryGet(serviceTypeKey, out value))
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
