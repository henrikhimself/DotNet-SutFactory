using Hj.SutFactory.Builders;

namespace Hj.SutFactory;

public static class SystemUnderTest
{
  public static T For<T>(Action<InputBuilder>? arrange = null, IServiceProvider? externalServiceProvider = null)
    where T : class
  {
    var sutBuilder = new SutBuilder(externalServiceProvider);

    if (arrange is not null)
    {
      arrange(sutBuilder.InputBuilder);
    }

    var sut = sutBuilder.CreateSut<T>();
    return sut;
  }
}
