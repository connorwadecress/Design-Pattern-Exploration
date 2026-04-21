namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// The FACADE.
///
/// Hides four subsystem services behind one high-level method: PlaceOrder.
/// Callers no longer have to know:
///   • the correct order of operations,
///   • how to roll back a reservation when payment fails,
///   • what a tracking number or a payment reference looks like.
///
/// Everything that matters to the CALLER (did it work, what's the tracking
/// number, what went wrong) comes back in a single OrderResult.
///
/// Design notes worth pointing at in the interview:
///   • All four subsystems are injected via the CONSTRUCTOR as INTERFACES —
///     Dependency Inversion Principle. In tests we'd pass in fakes.
///   • The facade is SMALL and DOMAIN-ORIENTED. Its vocabulary is
///     "place order", not "charge card" + "create shipment" + "send email".
///   • The facade does NOT hide the subsystem — a caller who needs
///     something off the common path can still new up IInventoryService
///     directly (see FacadeDemo.RunBypass).
/// </summary>
internal class OrderCheckoutFacade
{
    private readonly IInventoryService _inventory;
    private readonly IPaymentService _payment;
    private readonly IShippingService _shipping;
    private readonly INotificationService _notification;

    public OrderCheckoutFacade(
        IInventoryService inventory,
        IPaymentService payment,
        IShippingService shipping,
        INotificationService notification)
    {
        _inventory = inventory;
        _payment = payment;
        _shipping = shipping;
        _notification = notification;
    }

    /// <summary>
    /// Place an order for a single item. Orchestrates the four subsystems
    /// in the correct order and handles rollback if payment fails.
    /// </summary>
    public OrderResult PlaceOrder(Customer customer, Item item)
    {
        Console.WriteLine($"  >> PlaceOrder started for {customer.Name}, item {item.Sku} (${item.Price}).");

        // Step 1: stock check.
        if (!_inventory.CheckStock(item))
        {
            Console.WriteLine("  >> Out of stock — aborting early.");
            return new OrderResult(false, null, "Item out of stock.");
        }

        // Step 2: reserve inventory BEFORE charging — a charge with no
        // reservation could see the last unit sold out from under it.
        var reservationId = _inventory.Reserve(item);

        // Step 3: charge the customer.
        var payment = _payment.Charge(customer, item.Price);
        if (!payment.Succeeded)
        {
            // Step 3a: ROLLBACK. The caller doesn't need to know this happens.
            // This is the kind of cross-cutting concern a facade absorbs so
            // every caller doesn't have to re-implement it.
            _inventory.Release(reservationId);
            Console.WriteLine("  >> Payment failed — reservation released. Clean rollback.");
            return new OrderResult(false, null, payment.FailureReason);
        }

        // Step 4: create the shipment.
        var tracking = _shipping.CreateShipment(customer, item);

        // Step 5: notify the customer.
        _notification.SendOrderConfirmation(customer, tracking);

        Console.WriteLine("  >> PlaceOrder succeeded.");
        return new OrderResult(true, tracking, null);
    }
}
