namespace Hj.SutFactory.Registries.Implementation;

public sealed class InputCollection
{
  private readonly IDictionary<string, Func<object?>> _factoryMap;

  private readonly IDictionary<string, List<string>> _interfaceKeysMap;

  public InputCollection()
  {
    _factoryMap = new Dictionary<string, Func<object?>>();
    _interfaceKeysMap = new Dictionary<string, List<string>>();
  }

  public void Add(string key, IEnumerable<string?> interfaceKeys, Func<object?> value)
  {
    _factoryMap.Add(key, value);

    foreach (var interfaceKey in interfaceKeys)
    {
      if (interfaceKey is null)
      {
        continue;
      }

      if (!_interfaceKeysMap.TryGetValue(interfaceKey, out var keyList))
      {
        _interfaceKeysMap.Add(interfaceKey, keyList = new List<string>());
      }
      keyList.Add(key);
    }
  }

  public IEnumerable<Func<object?>> GetAllFactories(string key)
  {
    if (_interfaceKeysMap.TryGetValue(key, out var factoryKeys))
    {
      foreach (var factoryKey in factoryKeys)
      {
        var factory = _factoryMap[factoryKey];
        yield return factory;
      }
    }
  }

  public bool TryGetFactory(string key, out Func<object?> value)
  {
    return _factoryMap.TryGetValue(key, out value!);
  }
}
