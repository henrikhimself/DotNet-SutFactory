using System.Collections;

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

  public void AddFactory(string key, IEnumerable<string?> interfaceKeys, Func<object?> valueFactory)
  {
    _factoryMap.Add(key, valueFactory);

    foreach (var interfaceKey in interfaceKeys)
    {
      if (interfaceKey is null)
      {
        continue;
      }

      if (!_interfaceKeysMap.TryGetValue(interfaceKey, out var keyList))
      {
        _interfaceKeysMap.Add(interfaceKey, keyList = []);
      }

      keyList.Add(key);
    }
  }

  public bool TryGet(string key, out object? value)
  {
    if (_factoryMap.TryGetValue(key, out var valueFactory))
    {
      value = valueFactory();
      return true;
    }

    value = default;
    return false;
  }

  public bool TryGetList(string key, Type type, out object? value)
  {
    var factoryKeys = new List<string>();
    if (_interfaceKeysMap.TryGetValue(key, out var factoryKeysByInterface))
    {
      factoryKeys.AddRange(factoryKeysByInterface);
    }

    if (_factoryMap.ContainsKey(key))
    {
      factoryKeys.Add(key);
    }

    if (factoryKeys.Count == 0)
    {
      value = default;
      return false;
    }

    var valueList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type))!;
    foreach (var factoryKey in factoryKeys)
    {
      var valueFactory = _factoryMap[factoryKey];
      var item = valueFactory();
      valueList.Add(item);
    }

    value = valueList;
    return true;
  }
}
