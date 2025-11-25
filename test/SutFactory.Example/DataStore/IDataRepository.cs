namespace Hj.SutFactory.Example;

public interface IDataRepository
{
  /// <summary>
  /// Creates a new item in the data store.
  /// </summary>
  /// <typeparam name="T">The type of value to store.</typeparam>
  /// <param name="storeName">The name of the data store.</param>
  /// <param name="value">The value to create.</param>
  /// <returns>The unique identifier of the created item.</returns>
  Guid Create<T>(string storeName, T value)
    where T : IConvertible;

  /// <summary>
  /// Reads all items from the data store.
  /// </summary>
  /// <typeparam name="T">The type of values to retrieve.</typeparam>
  /// <param name="storeName">The name of the data store.</param>
  /// <returns>An enumerable collection of all items in the store.</returns>
  IEnumerable<T> Read<T>(string storeName)
    where T : IConvertible;

  /// <summary>
  /// Updates an item in the data store.
  /// </summary>
  /// <typeparam name="T">The type of value to update.</typeparam>
  /// <param name="storeName">The name of the data store.</param>
  /// <param name="id">The unique identifier of the item to update.</param>
  /// <param name="value">The new value.</param>
  void Update<T>(string storeName, Guid id, T value)
    where T : IConvertible;

  /// <summary>
  /// Deletes an item from the data store.
  /// </summary>
  /// <param name="storeName">The name of the data store.</param>
  /// <param name="id">The unique identifier of the item to delete.</param>
  void Delete(string storeName, Guid id);
}
