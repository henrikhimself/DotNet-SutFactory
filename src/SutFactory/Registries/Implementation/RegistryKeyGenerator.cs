namespace Hj.SutFactory.Registries.Implementation;

public sealed class RegistryKeyGenerator : IRegistryKeyGenerator
{
  private static readonly string[] _excludedAssemblyQualifiedNames =
  [
    typeof(object).AssemblyQualifiedName!,
    typeof(object).GetType().AssemblyQualifiedName!,
  ];

  public string? GenerateKey(Type type)
  {
    var key = type.AssemblyQualifiedName;

    if (_excludedAssemblyQualifiedNames.Contains(key))
    {
      key = null;
    }

    return key;
  }
}
