using System.Reflection;

namespace Hj.SutFactory.Factories.Implementation;

public abstract class FactoryBase
{
  protected FactoryBase()
  {
  }

  protected static ConstructorInfo? GetConstructorInfo(Type type)
  {
    var typeInfo = type.GetTypeInfo();
    var ctors = typeInfo.DeclaredConstructors
        .Where(ctor => !ctor.IsStatic && ctor.IsPublic)
        .ToList();

    var ctor = ctors.Count == 1
        ? ctors.Single()
        : ctors.FirstOrDefault(x => x.GetParameters().Length == 0);
    return ctor;
  }

  protected static object?[] GetConstructorParameters(ConstructorInfo ctor, IServiceProvider serviceProvider)
  {
    var parameterTypes = ctor.GetParameters()
        .Select(x => x.ParameterType)
        .ToArray();
    var parameters = new object?[parameterTypes.Length];

    for (var i = 0; i < parameterTypes.Length; i++)
    {
      var parameterType = parameterTypes[i];
      var instance = serviceProvider.GetService(parameterType);
      parameters[i] = instance;
    }

    return parameters;
  }
}
