﻿using Hj.SutFactory.Builders;
using Hj.SutFactory.Example.DataStore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Hj.SutFactory.Example;

public class DataStoreTests
{
  private static readonly Guid _knownEntityId = new("00000000-0000-0000-0000-000000000001");

  [Fact]
  public void Create_GivenValue_SavesValue()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // Retrieve the data entities list for later when we assert that the Create operation was successful.
    var dataEntities = sutBuilder
      .GetRequiredService<List<DataEntity>>();

    // act
    var result = sut.Create("my decimal store", 30m);

    // assert
    var entity = dataEntities.Single(item => item.Id == result);
    Assert.Equal(30m, entity.Value);
  }

  [Fact]
  public void Create_GivenSaveReturnEmptyIdentity_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // Break the happy path by configuring the Save method to return an empty identity.
    sutBuilder
      .GetRequiredService<IDataStore>()
      .Save(Arg.Any<DataEntity>())
      .Returns(Guid.Empty);

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sut.Create("my decimal store", 30m));
  }

  [Fact]
  public void Read_GivenValidDataFormat_ReturnsAll()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // act
    var result = sut.Read<int>("my integer store");

    // assert
    Assert.Collection(
      result,
      item1 => Assert.Equal(10, item1),
      item2 => Assert.Equal(20, item2));
  }

  [Fact]
  public void Read_GivenInvalidDataFormat_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // Break the happy path by retrieving the data entities list and change
    // the data such that we provoke a format exception to be thrown.
    sutBuilder
      .GetRequiredService<List<DataEntity>>()
      .ForEach(entity => entity.Value = "not an integer");

    // act & assert
    Assert.Throws<FormatException>(() =>
    {
      sut
        .Read<int>("my integer store")
        .ToList();
    });
  }

  [Fact]
  public void Update_GivenKnownId_UpdatesValue()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // Retrieve the data entities list for later when we assert that the Update operation was successful.
    var dataEntities = sutBuilder
      .GetRequiredService<List<DataEntity>>();

    // act
    // It is safe to also change the Type here without affecting the other unit tests. Even though the "known entity"
    // is part of the "happy path", it is stored inside the SutBuilder instance and this is only known within the scope
    // of this particular unit test.
    sut.Update("my integer store", _knownEntityId, "100");

    // assert
    var entity = dataEntities.Single(item => item.Id == _knownEntityId);
    Assert.Equal("100", entity.Value);
  }

  [Fact]
  public void Update_GivenSaveReturnEmptyIdentity_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // Break the happy path by configuring the Save method to return an empty identity.
    sutBuilder
      .GetRequiredService<IDataStore>()
      .Save(Arg.Any<DataEntity>())
      .Returns(Guid.Empty);

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sut.Update("my integer store", _knownEntityId, 100));
  }

  [Fact]
  public void Delete_GivenId_RemovesEntity()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // Retrieve the data entities list for later when we assert that the Delete operation was successful.
    var dataEntities = sutBuilder
      .GetRequiredService<List<DataEntity>>();

    // act
    sut.Delete("my integer store", _knownEntityId);

    // assert
    Assert.DoesNotContain(dataEntities, item => item.Id == _knownEntityId);
  }

  private static void SetHappyPath(InputBuilder inputBuilder)
  {
    // A data entities list is configured which is used later.
    var dataEntities = inputBuilder.Instance<List<DataEntity>>();
    dataEntities.Add(new() { Id = _knownEntityId, Value = 10, });
    dataEntities.Add(new() { Id = Guid.NewGuid(), Value = 20, });

    // A data store fake is configured to use the data entities list.
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

    // A data store factory service is configured to return the data store fake.
    var dataStoreFactory = inputBuilder.Instance<IDataStoreFactory>();
    dataStoreFactory.GetOrCreateStore(Arg.Any<string>(), Arg.Any<Type>()).Returns(dataStore);
  }
}