namespace Hj.SutFactory.Registries;

public class InputRegistry : IInputRegistry
{
  private readonly IRegistryKeyGenerator _registryKeyGenerator;
  private readonly IDictionary<string, object?> _backingStore;

  public InputRegistry(
      IRegistryKeyGenerator registryKeyGenerator,
      IDictionary<string, object?> backingStore)
  {
    _registryKeyGenerator = registryKeyGenerator;
    _backingStore = backingStore;
  }

  public T? GetOrCreateValue<T>(Type type, bool isSingleton, Func<T?> valueFactory)
  {
    object? value;

    var key = _registryKeyGenerator.GenerateKey(type);
    if (key is null)
    {
      value = null;
    }
    else if (_backingStore.ContainsKey(key))
    {
      value = _backingStore[key];
    }
    else
    {
      value = valueFactory();

      if (isSingleton)
      {
        _backingStore.Add(key, value);
      }
    }

    return (T?)value;
  }
}
