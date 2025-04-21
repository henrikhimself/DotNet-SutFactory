using Hj.SutFactory.Example.DataStore.Models;
using Hj.SutFactory.Example.DataStore.Services;

namespace Hj.SutFactory.Example;

#nullable disable

public abstract class ReadmeExampleTestsBase
{
  public abstract void Read_GivenCompatibleDataStore_ReturnsAll();

  public abstract void Read_GivenIncompatibleDataStore_Throws();

  protected static void SetHappyPath(InputBuilder arrange)
  {
    var dataEntities = arrange.Instance<List<DataEntity>>();
    dataEntities.Add(new() { Id = Guid.NewGuid(), Value = 10, });

    var dataStore = arrange.Instance<IDataStore>();
    dataStore
      .LoadAll()
      .Returns(_ => dataEntities.Select(entity => entity.Clone()));

    arrange
      .Instance<IDataStoreFactory>()
      .GetOrCreateStore(default, default)
      .ReturnsForAnyArgs(dataStore);
  }
}
