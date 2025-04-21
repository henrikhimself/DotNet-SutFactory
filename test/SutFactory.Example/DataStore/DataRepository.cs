using System.Globalization;
using Hj.SutFactory.Example.DataStore.Models;
using Hj.SutFactory.Example.DataStore.Services;

namespace Hj.SutFactory.Example.DataStore;

public class DataRepository(IDataStoreFactory dataStoreFactory) : IDataRepository
{
  private readonly IDataStoreFactory _dataStoreFactory = dataStoreFactory;

  public Guid Create<T>(string storeName, T value)
    where T : IConvertible
  {
    var dataStore = _dataStoreFactory.GetOrCreateStore(storeName, typeof(DataEntity));
    var id = dataStore.Save(new DataEntity
    {
      Value = value,
    });

    if (id == Guid.Empty)
    {
      throw new InvalidOperationException("Create operation failed");
    }

    return id;
  }

  public IEnumerable<T> Read<T>(string storeName)
    where T : IConvertible
  {
    var returnType = typeof(T);
    var dataStore = _dataStoreFactory.GetOrCreateStore(storeName, typeof(DataEntity));

    var entities = dataStore.LoadAll();
    foreach (var entity in entities)
    {
      var value = Convert.ChangeType(entity.Value, returnType, CultureInfo.InvariantCulture);
      yield return (T)value;
    }
  }

  public void Update<T>(string storeName, Guid id, T value)
    where T : IConvertible
  {
    var dataStore = _dataStoreFactory.GetOrCreateStore(storeName, typeof(DataEntity));

    var entity = dataStore.Load(id);
    if (entity is null)
    {
      return;
    }

    entity.Value = value;
    if (dataStore.Save(entity) == Guid.Empty)
    {
      throw new InvalidOperationException("Update operation failed");
    }
  }

  public void Delete(string storeName, Guid id)
  {
    var dataStore = _dataStoreFactory.GetOrCreateStore(storeName, typeof(DataEntity));
    dataStore.Delete(id);
  }
}
