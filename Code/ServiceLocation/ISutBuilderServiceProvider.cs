namespace Hj.SutFactory.ServiceLocation;

public interface ISutBuilderServiceProvider : IServiceProvider
{
  T GetService<T>();
}
