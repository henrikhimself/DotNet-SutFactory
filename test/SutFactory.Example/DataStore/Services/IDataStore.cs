using Hj.SutFactory.Example.DataStore.Models;

namespace Hj.SutFactory.Example.DataStore.Services;

public interface IDataStore
{
  /// <summary>
  /// Saves an entity to the data store.
  /// </summary>
  /// <param name="value">The entity to save.</param>
  /// <returns>The unique identifier of the saved entity.</returns>
  Guid Save(DataEntity value);

  /// <summary>
  /// Loads an entity from the data store.
  /// </summary>
  /// <param name="id">The unique identifier of the entity to load.</param>
  /// <returns>The entity if found; otherwise, null.</returns>
  DataEntity? Load(Guid id);

  /// <summary>
  /// Loads all entities from the data store.
  /// </summary>
  /// <returns>An enumerable collection of all entities in the store.</returns>
  IEnumerable<DataEntity> LoadAll();

  /// <summary>
  /// Deletes the entity with the specified id from the data store.
  /// </summary>
  /// <param name="id">The unique identifier of the entity to delete.</param>
  void Delete(Guid id);
}
