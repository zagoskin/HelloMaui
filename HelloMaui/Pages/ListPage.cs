using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using HelloMaui.Models;
using HelloMaui.ViewModels;
using HelloMaui.Views;
using System.Collections.ObjectModel;

namespace HelloMaui.Pages;

public sealed class ListPage : BaseContentPage<ListViewModel>
{
    private readonly RefreshView _refreshView;
    private static bool _initializing = true;
    public ListPage(ListViewModel viewModel) : base(viewModel)
    {
        this.AppThemeColorBinding(BackgroundColorProperty, Colors.AliceBlue, Color.FromArgb("#282c2e"));

        ToolbarItems.Add(new ToolbarItem()
            .Text("Calendar")
            .Invoke(item => item.Clicked += static async(s, e) => await Shell.Current.GoToAsync(AppShell.GetRoute<CalendarPage>())));

        Content = new RefreshView
        {
            //Content = new CollectionView
            //{
            //    Header = new SearchBar()
            //        .Placeholder("search titles")
            //        .AppThemeColorBinding(Label.TextColorProperty, Colors.Black, Colors.LightGray)
            //        .Center()
            //        .TextCenter()
            //        .Behaviors(new UserStoppedTypingBehavior
            //        {
            //            BindingContext = viewModel,
            //            StoppedTypingTimeThreshold = 750,
            //            ShouldDismissKeyboardAutomatically = true,
            //        }.Bind(UserStoppedTypingBehavior.CommandProperty,
            //                getter: static (ListViewModel vm) => vm.ApplyFilterCommand))
            //        .TapGesture(async () => await Toast.Make("TOTASO").Show(), 2)
            //        .Bind(SearchBar.TextProperty,
            //            getter: static (ListViewModel vm) => vm.SearchBarText,
            //            setter: static (ListViewModel vm, string text) => vm.SearchBarText = text)
            //        .Bind(IsEnabledProperty,
            //            getter: static (ListViewModel vm) => vm.IsSearchBarEnabled),

            //    Footer = new Label()
            //        .Text(".NET MAUI: From Zero to Hero")
            //        .AppThemeColorBinding(Label.TextColorProperty, Color.FromArgb("#474f52"), Colors.DarkGray)
            //        .FontSize(10)
            //        .Paddings(left: 8)
            //        .Center()
            //        .TextCenter(),

            //    SelectionMode = SelectionMode.Single
            //}
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
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_initializing)
        {
            _initializing = false;
            this.ShowPopup(new WelcomePopup());
        }        

        if (_refreshView.Children[0] is CollectionView { ItemsSource: ObservableCollection<LibraryModel> libraryCollection }
            && !libraryCollection.Any())
        {
            _refreshView.IsRefreshing = true;
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
