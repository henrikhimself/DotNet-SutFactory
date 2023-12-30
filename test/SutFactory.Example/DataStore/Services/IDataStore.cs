using Hj.SutFactory.Example.DataStore.Models;

namespace Hj.SutFactory.Example.DataStore.Services;

public interface IDataStore
{
  /// <summary>
  /// Saves an entity to the data store.
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  Guid Save(DataEntity value);

  /// <summary>
  /// Loads an entity from the data store.
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  DataEntity? Load(Guid id);

  /// <summary>
  /// Loads all entities from the data store.
  /// </summary>
  /// <returns></returns>
  IEnumerable<DataEntity> LoadAll();

  /// <summary>
  /// Deletes the entity with the specified id from the data store.
  /// </summary>
  /// <param name="id"></param>
  void Delete(Guid id);
}
