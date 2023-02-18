namespace Hj.SutFactory;

public interface IInputRegistry
{
  object? GetOrCreateValue(Type serviceType, Type implementationType, bool isSingleton, Func<object?> valueFactory);
}
