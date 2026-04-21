namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// A customer placing an order. Records give us value-equality and a tidy
/// positional constructor — useful for simple DTOs like this.
/// </summary>
internal record Customer(string Name, string Email, string Address);

/// <summary>
/// An item being purchased.
/// </summary>
internal record Item(string Sku, string Name, decimal Price);

/// <summary>
/// Result returned by <see cref="IPaymentService.Charge"/>. FailureReason
/// is null when Succeeded is true.
/// </summary>
internal record PaymentResult(bool Succeeded, string Reference, string? FailureReason);

/// <summary>
/// Result returned by <see cref="OrderCheckoutFacade.PlaceOrder"/> —
/// the ONE thing callers see from the whole subsystem.
/// </summary>
internal record OrderResult(bool Succeeded, string? TrackingNumber, string? FailureReason);
