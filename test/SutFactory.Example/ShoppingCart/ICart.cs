using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart;

public interface ICart
{
  /// <summary>
  /// Gets the total price of the items in the cart.
  /// </summary>
  decimal TotalPrice { get; }

  /// <summary>
  /// Adds a cart item to the cart.
  /// </summary>
  /// <param name="cartItem">The cart item to add.</param>
  /// <returns>The added cart item.</returns>
  CartItem AddItem(CartItem cartItem);

  /// <summary>
  /// Removes a cart item from the cart.
  /// </summary>
  /// <param name="cartItem">The cart item to remove.</param>
  void RemoveItem(CartItem cartItem);

  /// <summary>
  /// Retrieves the items in the cart.
  /// </summary>
  /// <returns>An enumeration of the items in the cart.</returns>
  IEnumerable<CartItem> GetItems();
}
