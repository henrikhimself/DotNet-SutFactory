namespace Hj.SutFactory.Registries;

public sealed class InputBuilderConfigurator<T>
    where T : class
{
  private readonly IServiceProvider _serviceProvider;
  private readonly T? _value;

  public InputBuilderConfigurator(IServiceProvider serviceProvider, T? value)
  {
    _serviceProvider = serviceProvider;
    _value = value;
  }

  public void Configure(Action<T> action)
  {
    if (_value is not null && action is not null)
    {
      action(_value!);
    }
  }

  public void Configure(Action<T, IServiceProvider> action)
  {
    if (_value is not null && action is not null)
    {
      action(_value!, _serviceProvider);
    }
  }
}