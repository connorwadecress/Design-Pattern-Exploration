namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// The ABSTRACT FACTORY.
///
/// Declares one creation method per product type in the family.
/// Each concrete factory (LightThemeFactory, DarkThemeFactory) guarantees
/// that the products it returns belong to the same theme — client code
/// can't accidentally mix a light button with a dark textbox.
///
/// Adding a new product type (e.g. CreateCheckbox) would require updating
/// every concrete factory — that's the main trade-off of this pattern:
/// easy to add a new FAMILY, harder to add a new PRODUCT.
/// </summary>
internal interface IUiFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}
