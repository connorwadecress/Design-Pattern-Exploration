namespace DesignPatterns.Patterns.Facade;

// The facade. Hides four subsystem services behind one PlaceOrder call.
// Owns the correct ordering and the rollback if payment fails.
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

    public OrderResult PlaceOrder(Customer customer, Item item)
    {
        var reservationId = _inventory.Reserve(item);

        if (!_payment.Charge(customer, item.Price))
        {
            // Rollback - caller never writes this code themselves.
            _inventory.Release(reservationId);
            return new OrderResult(false, null, "Payment declined");
        }

        var tracking = _shipping.CreateShipment(customer, item);
        _notification.Notify(customer, tracking);

        return new OrderResult(true, tracking, null);
    }
}
