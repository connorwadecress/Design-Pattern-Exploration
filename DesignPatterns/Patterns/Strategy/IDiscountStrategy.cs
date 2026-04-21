namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// The STRATEGY interface.
///
/// Defines the contract that every discount algorithm implements. The
/// <see cref="Checkout"/> context holds a reference to one of these
/// and delegates the "how do we work out a discount?" question to it.
///
/// Notice the method takes EVERYTHING the algorithm might need
/// (Cart + Customer) — different strategies use different subsets, but
/// the signature is uniform so the context can call any of them the same way.
/// </summary>
internal interface IDiscountStrategy
{
    /// <summary>
    /// Short human-readable name for demo/logging purposes.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Calculate the discount amount (a positive number in the same currency
    /// as the cart). The context subtracts this from the subtotal.
    /// </summary>
    decimal CalculateDiscount(Cart cart, Customer customer);
}
