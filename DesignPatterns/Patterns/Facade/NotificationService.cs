namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// Subsystem service — sends the customer an order confirmation.
/// Real implementation would send an email, SMS, push notification, etc.
/// </summary>
internal interface INotificationService
{
    void SendOrderConfirmation(Customer customer, string trackingNumber);
}

internal class NotificationService : INotificationService
{
    public void SendOrderConfirmation(Customer customer, string trackingNumber)
    {
        Console.WriteLine($"    [Notification]  Emailed {customer.Email}:");
        Console.WriteLine($"    [Notification]    \"Your order is on the way. Tracking: {trackingNumber}.\"");
    }
}
