using CommunityToolkit.Maui.Markup;

namespace HelloMaui;

public sealed class MainPageCollection : CollectionView
{
    public MainPageCollection()
    {
        Header = new Label()
            .Text(".NET MAUI Libraries")
            .AppThemeColorBinding(Label.TextColorProperty, Colors.Black, Colors.LightGray)
            .FontSize(32)
            .Paddings(0, 6, 0, 6)
            .Center()
            .TextCenter();

        Footer = new Label()
            .Text(".NET MAUI: From Zero to Hero")
            .AppThemeColorBinding(Label.TextColorProperty, Color.FromArgb("#474f52"), Colors.DarkGray)
            .FontSize(10)
            .Paddings(left: 8)
            .Center()
            .TextCenter();

        SelectionMode = SelectionMode.Single;                
    }
}
