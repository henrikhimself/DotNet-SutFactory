namespace Hj.SutFactory.Example;

public interface IDataRepository
{
  Guid Create<T>(string storeName, T value)
    where T : IConvertible;

  IEnumerable<T> Read<T>(string storeName)
    where T : IConvertible;

  void Update<T>(string storeName, Guid id, T value)
    where T : IConvertible;

  void Delete(string storeName, Guid id);
}
