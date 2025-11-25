using Hj.SutFactory.Example.DataStore.Models;
using Hj.SutFactory.Example.DataStore.Services;

namespace Hj.SutFactory.Example.DataStore;

/// <summary>
/// These tests favors using the SystemUnderTest.For'T extension method.
/// This is a good choice because it promotes an "arrange" phase that is clearly
/// seperated from the act and assert phase.
///
/// See the ShoppingCart example for how the SutBuilder can be used to avoid
/// using the "arrange" lambda.
/// </summary>
public class DataRepositoryTests
{
  private static readonly Guid _knownEntityId = new("00000000-0000-0000-0000-000000000001");

  [Fact]
  public void Create_GivenSaveReturnEmptyIdentity_Throws()
  {
    // arrange
    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Break the happy path by configuring the Save method to return an empty identity.
      arrange.GetRequiredService<IDataStore>()
        .Save(Arg.Any<DataEntity>())
        .Returns(Guid.Empty);
    });

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sut.Create("my decimal store", 30m));
  }

  [Fact]
  public void Create_GivenValue_SavesValue()
  {
    // arrange
    List<DataEntity>? dataEntitiesSpy = null;

    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Retrieve the data entities list set by the happy path so that it can be verified later.
      dataEntitiesSpy = arrange.GetService<List<DataEntity>>();
    });

    // act
    var result = sut.Create("my decimal store", 30m);

    // assert
    Assert.NotNull(dataEntitiesSpy);
    var entity = dataEntitiesSpy.Single(item => item.Id == result);
    Assert.Equal(30m, entity.Value);
  }

  [Fact]
  public void Read_GivenCompatibleDataStore_ReturnsAll()
  {
    // arrange
    var sut = SystemUnderTest.For<DataRepository>(SetHappyPath);

    // act
    var result = sut.Read<int>("my integer store");

    // assert
    Assert.Collection(
      result,
      item1 => Assert.Equal(10, item1),
      item2 => Assert.Equal(20, item2));
  }

  [Fact]
  public void Read_GivenIncompatibleDataStore_Throws()
  {
    // arrange
    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Break the happy path by modifying the data entities such that a format exception will be thrown.
      arrange.GetRequiredService<List<DataEntity>>()
        .ForEach(entity => entity.Value = "not an integer");
    });

    // act & assert
    Assert.Throws<FormatException>(() => sut.Read<int>("my integer store").ToList());
  }

  [Fact]
  public void Update_GivenKnownId_UpdatesValue()
  {
    // arrange
    List<DataEntity>? dataEntitiesSpy = null;

    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Retrieve the data entities list set by the happy path so that it can be verified later.
      dataEntitiesSpy = arrange.GetService<List<DataEntity>>();
    });

    // act
    sut.Update("my integer store", _knownEntityId, "100");

    // assert
    Assert.NotNull(dataEntitiesSpy);
    var entity = dataEntitiesSpy.Single(item => item.Id == _knownEntityId);
    Assert.Equal("100", entity.Value);
  }

  [Fact]
  public void Update_GivenSaveReturnEmptyIdentity_Throws()
  {
    // arrange
    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Break the happy path by configuring the Save method to return an empty identity.
      arrange.GetRequiredService<IDataStore>()
        .Save(Arg.Any<DataEntity>())
        .Returns(Guid.Empty);
    });

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sut.Update("my integer store", _knownEntityId, 100));
  }

  [Fact]
  public void Delete_GivenId_RemovesEntity()
  {
    // arrange
    List<DataEntity>? dataEntitiesSpy = null;

    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Retrieve the data entities list set by the happy path so that it can be verified later.
      dataEntitiesSpy = arrange.GetRequiredService<List<DataEntity>>();
    });

    // act
    sut.Delete("my integer store", _knownEntityId);

    // assert
    Assert.NotNull(dataEntitiesSpy);
    Assert.DoesNotContain(dataEntitiesSpy, item => item.Id == _knownEntityId);
  }

  private static void SetHappyPath(InputBuilder inputBuilder)
  {
    // A data entities list is configured which is used later.
    var dataEntities = inputBuilder.Instance<List<DataEntity>>();
    dataEntities.Add(new() { Id = _knownEntityId, Value = 10, });
    dataEntities.Add(new() { Id = Guid.NewGuid(), Value = 20, });

    // Create a fake IDataStore that acts like the real service but without the external dependencies.
    // Each method is configured to alter the data entities list. The content of this list can be
    // retrieved and verified later in tests.
    var dataStore = inputBuilder.Instance<IDataStore>();
    dataStore.LoadAll().Returns(x => dataEntities.Select(x => x.Clone()));
    dataStore.Load(Arg.Any<Guid>()).Returns(x =>
    {
      var id = x.ArgAt<Guid>(0);
      return dataEntities.SingleOrDefault(item => item.Id == id)?.Clone();
    });
    dataStore.When(x => x.Delete(Arg.Any<Guid>())).Do(x =>
    {
      var id = x.ArgAt<Guid>(0);
      dataEntities.RemoveAll(item => item.Id == id);
    });
    dataStore.Save(Arg.Any<DataEntity>()).Returns(x =>
    {
      // Set Save method to insert or update an entity.
      var dataEntity = x.ArgAt<DataEntity>(0);

      // Handle "insert".
      if (dataEntity.Id == Guid.Empty)
      {
        // Create new entity and assign identity
        var newDataEntity = dataEntity.Clone();
        newDataEntity.Id = Guid.NewGuid();
        dataEntities.Add(newDataEntity);
        return newDataEntity.Id;
      }

      // Handle "update".
      var oldDataEntity = dataEntities.SingleOrDefault(item => item.Id == dataEntity.Id);
      if (oldDataEntity is not null)
      {
        oldDataEntity.Value = dataEntity.Value;
        return oldDataEntity.Id;
      }

      return Guid.Empty;
    });

    // A data store factory service is configured to return the fake IDataStore.
    var dataStoreFactory = inputBuilder.Instance<IDataStoreFactory>();
    dataStoreFactory.GetOrCreateStore(Arg.Any<string>(), Arg.Any<Type>()).Returns(dataStore);
  }
}
