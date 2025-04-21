namespace Hj.SutFactory.Example;

public interface IDataRepository
{
  /// <summary>
  /// Creates a new item in the data store.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="storeName"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  Guid Create<T>(string storeName, T value)
    where T : IConvertible;

  /// <summary>
  /// Reads all items from the data store.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="storeName"></param>
  /// <returns></returns>
  IEnumerable<T> Read<T>(string storeName)
    where T : IConvertible;

  /// <summary>
  /// Updates an item in the data store.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="storeName"></param>
  /// <param name="id"></param>
  /// <param name="value"></param>
  void Update<T>(string storeName, Guid id, T value)
    where T : IConvertible;

  /// <summary>
  /// Deletes an item from the data store.
  /// </summary>
  /// <param name="storeName"></param>
  /// <param name="id"></param>
  void Delete(string storeName, Guid id);
}
