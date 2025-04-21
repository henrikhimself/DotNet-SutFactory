namespace Hj.SutFactory.Example.DataStore.Services;

public interface IDataStoreFactory
{
  /// <summary>
  /// Gets an existing data store or creates a new named data store for the specified type.
  /// </summary>
  /// <param name="storeName"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  IDataStore GetOrCreateStore(string storeName, Type type);
}
