namespace DesignPatterns.Patterns.Mediator;

/// <summary>
/// The MEDIATOR interface.
///
/// Declares the methods colleagues call to notify events or request actions.
/// Here, users either register themselves with the chat room or ask the
/// chat room to deliver a message on their behalf.
///
/// Key property: USERS DO NOT REFERENCE EACH OTHER. They only reference
/// an IChatRoomMediator. Add a new user, existing users are untouched.
/// Remove a user, existing users are untouched.
/// </summary>
internal interface IChatRoomMediator
{
    /// <summary>
    /// Join the chat room — makes the user eligible to both send and receive.
    /// </summary>
    void Register(User user);

    /// <summary>
    /// Send a message from one user. The mediator decides what to do with it
    /// (broadcast to everyone except the sender, apply filtering rules, etc.).
    /// </summary>
    void SendMessage(string message, User from);
}
