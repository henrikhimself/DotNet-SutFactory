namespace Hj.SutFactory.UnitTest.TestData.Case1;

public class ExternalServiceProvider : IServiceProvider
{
  public object? GetService(Type serviceType)
  {
    object? value = serviceType.Name switch
    {
      nameof(ExternalService) => new ExternalService(),
      _ => null,
    };
    return value;
  }
}
