using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Markup;
using HelloMaui.ViewModels;

namespace HelloMaui.Views;
public class LibrariesCollectionView : CollectionView
{
    public LibrariesCollectionView(ListViewModel viewModel)
    {
        BindingContext = viewModel;
        Header = new SearchBar()
            .Placeholder("search titles")
            .AppThemeColorBinding(Label.TextColorProperty, Colors.Black, Colors.LightGray)
            .Center()
            .TextCenter()
            .Behaviors(new UserStoppedTypingBehavior
            {
                BindingContext = viewModel,
                StoppedTypingTimeThreshold = 750,
                ShouldDismissKeyboardAutomatically = true,
            }.Bind(UserStoppedTypingBehavior.CommandProperty,
                    getter: static (ListViewModel vm) => vm.ApplyFilterCommand))
            .TapGesture(async () => await Toast.Make("TOTASO").Show(), 2)
            .Bind(SearchBar.TextProperty,
                getter: static (ListViewModel vm) => vm.SearchBarText,
                setter: static (ListViewModel vm, string text) => vm.SearchBarText = text)
            .Bind(IsEnabledProperty,
                getter: static (ListViewModel vm) => vm.IsSearchBarEnabled);

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
        