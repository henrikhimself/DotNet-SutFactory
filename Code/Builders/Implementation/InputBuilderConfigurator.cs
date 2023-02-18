namespace Hj.SutFactory.Builders.Implementation;

public sealed class InputBuilderConfigurator<T>
    where T : class
{
  private readonly IServiceProvider _serviceProvider;
  private readonly object? _value;

  public InputBuilderConfigurator(IServiceProvider serviceProvider, object? value)
  {
    _serviceProvider = serviceProvider;
    _value = value;
  }

  public void Configure(Action<T> action)
  {
    if (_value is not null && action is not null)
    {
      action((T)_value!);
    }
  }

  public void Configure(Action<T, IServiceProvider> action)
  {
    if (_value is not null && action is not null)
    {
      action((T)_value!, _serviceProvider);
    }
  }
}
