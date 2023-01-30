namespace Hj.SutFactory;

public interface IInputRegistry
{
  T? GetOrCreateValue<T>(Type type, bool isSingleton, Func<T?> valueFactory);
}
