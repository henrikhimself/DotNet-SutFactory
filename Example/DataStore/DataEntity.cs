﻿namespace Hj.SutFactory.Example.DataStore;

public class DataEntity
{
  public Guid Id { get; set; }

  public required object Value { get; set; }

  public DataEntity Clone() => new() { Id = Id, Value = Value, };
}
