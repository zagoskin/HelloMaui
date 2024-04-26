using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using HelloMaui.Models;
using System.Collections.ObjectModel;

namespace HelloMaui.Pages;

public sealed class LibraryCollection : CollectionView
{
    private readonly SearchBar _searchBar = null!;

    private readonly ObservableCollection<LibraryModel> _originalSource;
    private readonly ObservableCollection<LibraryModel> _currentSource;
    public LibraryCollection(ICollection<LibraryModel> source)
    {
        _originalSource = new(source);
        _currentSource = new(_originalSource);
        ItemsSource = _currentSource;
        Header = new SearchBar()
            .Placeholder("search titles")
            .AppThemeColorBinding(Label.TextColorProperty, Colors.Black, Colors.LightGray)
            .Center()
            .TextCenter()
            .Behaviors(new UserStoppedTypingBehavior
            {
                StoppedTypingTimeThreshold = 750,
                ShouldDismissKeyboardAutomatically = true,
            })
            .TapGesture(OnTap, 2)
            .Assign(out _searchBar);

        Footer = new Label()
            .Text(".NET MAUI: From Zero to Hero")
            .AppThemeColorBinding(Label.TextColorProperty, Color.FromArgb("#474f52"), Colors.DarkGray)
            .FontSize(10)
            .Paddings(left: 8)
            .Center()
            .TextCenter();

        SelectionMode = SelectionMode.Single;
    }

    private async void OnTap()
    {
        await Toast.Make("TOTASO", ToastDuration.Short).Show();
    }

    public void AddModel(LibraryModel libraryModel)
    {
        _originalSource.Add(libraryModel);
    }
}
