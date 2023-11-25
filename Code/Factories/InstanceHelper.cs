namespace Hj.SutFactory;

public static class InstanceHelper
{
  public static bool IsContract(Type type) => type.IsInterface || type.IsAbstract;

  public static bool IsImplementation(Type type) => type.IsSealed || !IsContract(type);
}
