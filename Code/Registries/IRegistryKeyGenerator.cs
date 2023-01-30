namespace Hj.SutFactory.Registries;

public interface IRegistryKeyGenerator
{
  string? GenerateKey(Type type);
}
