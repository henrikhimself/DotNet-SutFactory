namespace Hj.SutFactory.Registries.Implementation;

public sealed class RegistryKeyGenerator : IRegistryKeyGenerator
{
  private static readonly string[] ExcludedAssemblyQualifiedNames = new[]
  {
    typeof(object).AssemblyQualifiedName!,
    typeof(object).GetType().AssemblyQualifiedName!,
  };

  public string? GenerateKey(Type type)
  {
    var key = type.AssemblyQualifiedName;

    if (ExcludedAssemblyQualifiedNames.Contains(key))
    {
      key = null;
    }

    return key;
  }
}
