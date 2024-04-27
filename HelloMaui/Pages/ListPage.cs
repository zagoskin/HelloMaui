using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using HelloMaui.Models;
using HelloMaui.Services;
using HelloMaui.ViewModels;
using HelloMaui.Views;
using System.Collections.ObjectModel;

namespace HelloMaui.Pages;

public sealed class ListPage : BaseContentPage<ListViewModel>
{
    private readonly RefreshView _refreshView;
    private readonly WelcomePreferencesService _welcomePreferencesService;
    
    public ListPage(ListViewModel viewModel, WelcomePreferencesService welcomePreferencesService) : base(viewModel)
    {
        this.AppThemeColorBinding(BackgroundColorProperty, Colors.AliceBlue, Color.FromArgb("#282c2e"));

        ToolbarItems.Add(new ToolbarItem()
            .Text("Calendar")
            .Invoke(item => item.Clicked += static async(s, e) => await Shell.Current.GoToAsync(AppShell.GetRoute<CalendarPage>())));

        Content = new RefreshView
        {
            Content = new LibrariesCollectionView(viewModel)
                .ItemTemplate(new LibrariesDataTemplate())
                .Bind(SelectableItemsView.SelectedItemProperty,
                    getter: static (ListViewModel vm) => vm.SelectedItem,
                    setter: static (ListViewModel vm, object? selectedItem) => vm.SelectedItem = selectedItem)
                .Bind(SelectableItemsView.SelectionChangedCommandProperty,
                    getter: static (ListViewModel vm) => vm.SelectionChangedCommand)
                .Bind(ItemsView.ItemsSourceProperty,
                    getter: static (ListViewModel vm) => vm.MauiLibraries)
        }
        .Padding(12)
        .CenterVertical()
        .Bind(RefreshView.CommandProperty,
            getter: static (ListViewModel vm) => vm.RefreshCommand)
        .Bind(RefreshView.IsRefreshingProperty,
            getter: static (ListViewModel vm) => vm.IsRefreshing,
            setter: static (ListViewModel vm, bool isRefreshing) => vm.IsRefreshing = isRefreshing)
        .Assign(out _refreshView);
        _welcomePreferencesService = welcomePreferencesService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
              

        if (_refreshView.Children[0] is CollectionView { ItemsSource: ObservableCollection<LibraryModel> libraryCollection }
            && !libraryCollection.Any())
        {
            _refreshView.IsRefreshing = true;
        }

        if (_welcomePreferencesService.IsFirstRun)
        {
            await this.ShowPopupAsync(new WelcomePopup());
            _welcomePreferencesService.IsFirstRun = false;
        }
    }    

    private class WelcomePopup : Popup
    {
        public WelcomePopup()
        {
            Content = new Label()
                .Text("Welcome to .NET MAUI")
                .BackgroundColor(Color.FromArgb("#333366"))
                .Font(size: 42, bold: true)
                .Center()
                .TextCenter();
        }
    }
}
