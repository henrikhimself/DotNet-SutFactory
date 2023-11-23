namespace Hj.SutFactory.Builders.Implementation;

public sealed class InputBuilderConfigurator<T>(
  IServiceProvider serviceProvider,
  object value)
    where T : class
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  private readonly object _value = value;

  public void Configure(Action<T> action)
  {
    if (action is not null)
    {
      action((T)_value);
    }
  }

  public void Configure(Action<T, IServiceProvider> action)
  {
    if (action is not null)
    {
      action((T)_value, _serviceProvider);
    }
  }
}
