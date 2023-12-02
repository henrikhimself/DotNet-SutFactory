namespace Hj.SutFactory.Example.DataStore;

public interface IDataStoreFactory
{
  IDataStore GetOrCreateStore(string storeName, Type type);
}
