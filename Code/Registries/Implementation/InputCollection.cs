using System.Collections;

namespace Hj.SutFactory.Registries.Implementation;

public sealed class InputCollection
{
  /// <summary>
  /// The factory map contains explicit registrations.
  /// It maps a service type to a value factory or singleton value.
  /// </summary>
  private readonly Dictionary<string, Func<object?>> _factoryMap = [];

  /// <summary>
  /// The interfaces map contains implicit registrations.
  /// It enables retrieval of implementations that implement a given interface.
  /// </summary>
  private readonly Dictionary<string, List<string>> _interfaceKeysMap = [];

  public void AddFactory(string key, IEnumerable<string?> interfaceKeys, Func<object?> valueFactory)
  {
    // Explicit registration of a value factory.
    _factoryMap.Add(key, valueFactory);

    // Implicit registrations of implemented interfaces.
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
    bool TryGetInstance(string factoryKey, out object? instance)
    {
      if (_factoryMap.TryGetValue(factoryKey, out var valueFactory))
      {
        instance = valueFactory();
        return true;
      }

      instance = default;
      return false;
    }

    // Lookup explicit registration first.
    if (TryGetInstance(key, out value))
    {
      return true;
    }

    // Lookup implicit registration second.
    if (_interfaceKeysMap.TryGetValue(key, out var factoryKeysByInterface))
    {
      if (factoryKeysByInterface.Count > 1)
      {
        throw new InvalidOperationException($"Implicit lookup yielded mulitple implementations.");
      }

      var implicitKey = factoryKeysByInterface.SingleOrDefault();
      if (implicitKey is not null && TryGetInstance(implicitKey, out value))
      {
        return true;
      }
    }

    value = default;
    return false;
  }

  public bool TryGetList(string key, Type type, out object? value)
  {
    var factoryKeys = new List<string>();

    // Get explicit registrations first.
    if (_factoryMap.ContainsKey(key))
    {
      factoryKeys.Add(key);
    }

    // Get implicit registrations second where key is not already added.
    if (_interfaceKeysMap.TryGetValue(key, out var factoryKeysByInterface))
    {
      foreach (var factoryKey in factoryKeysByInterface)
      {
        if (_factoryMap.ContainsKey(factoryKey))
        {
          factoryKeys.Add(factoryKey);
        }
      }
    }

    if (factoryKeys.Count == 0)
    {
      value = default;
      return false;
    }

    // Transform value factories into a list of instances.
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
