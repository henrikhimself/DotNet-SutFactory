namespace Hj.SutFactory.Example.DataStore.Services;

public interface IDataStoreFactory
{
  /// <summary>
  /// Gets an existing data store or creates a new named data store for the specified type.
  /// </summary>
  /// <param name="storeName">The name of the data store to retrieve or create.</param>
  /// <param name="type">The type of entities that will be stored in the data store.</param>
  /// <returns>A store instance for the specified type and name.</returns>
  IDataStore GetOrCreateStore(string storeName, Type type);
}
