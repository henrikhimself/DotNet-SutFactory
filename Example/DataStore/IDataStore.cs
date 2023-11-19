namespace Hj.SutFactory.Example.DataStore;

public interface IDataStore
{
  Guid Save(DataEntity value);

  DataEntity? Load(Guid id);

  IEnumerable<DataEntity> LoadAll();

  void Delete(Guid id);
}
