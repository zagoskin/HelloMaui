using CommunityToolkit.Maui.Markup;

namespace HelloMaui;

public sealed class MainPageCollection : CollectionView
{
    public MainPageCollection()
    {
        Header = new Label()
            .Text(".NET MAUI Libraries")
            .FontSize(32)
            .Paddings(0, 6, 0, 6)
            .Center()
            .TextCenter();

        Footer = new Label()
            .Text(".NET MAUI: From Zero to Hero")
            .FontSize(10)
            .Paddings(left: 8)
            .Center()
            .TextCenter();

        SelectionMode = SelectionMode.Single;        
    }
}
