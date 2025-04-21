namespace Hj.SutFactory.TestCase.Case6;

public class ExternalServiceProvider : IServiceProvider
{
  private readonly ExternalService _externalServiceSingleton = new();

  public object? GetService(Type serviceType)
  {
    object? value = serviceType.Name switch
    {
      nameof(ExternalService) => _externalServiceSingleton,
      _ => null,
    };
    return value;
  }
}
