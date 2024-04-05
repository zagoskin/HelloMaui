﻿using Ardalis.GuardClauses;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using System.Collections.ObjectModel;

namespace HelloMaui;


public sealed class MainPage : BaseContentPage
{ 
    public MainPage() : base()
    {
        this.AppThemeColorBinding(BackgroundColorProperty, Colors.AliceBlue, Color.FromArgb("#282c2e")); 

        Content = new RefreshView
        {
            Content = new MainPageCollection()
                .ItemsSource(MauiLibraries)
                .ItemTemplate(new MauiLibrariesDataTemplate())
                .Invoke(collectionView => collectionView.SelectionChanged += HandleSelectionChanged)

        }
        .Padding(12)
        .CenterVertical()
        .Invoke(refreshView => refreshView.Refreshing += HandleRefreshing);
    }

    private async void HandleRefreshing(object? sender, EventArgs e)
    {
        Guard.Against.Null(sender);

        var refreshView = (RefreshView)sender;

        await Task.Delay(TimeSpan.FromSeconds(2));

        MauiLibraries.Add(new LibraryModel
        {
            Title = "Sharpnado.Tabs",
            Description = "Pure Xamarin.Forms tabs:* Fixed tabs (android tabs style)  * Scrollable tabs* Vertical tabs* Material design tabs (top and leading icon)* Support for SVG images* Segmented tabs* Custom shadows (neumorphism ready)* Badges on tabs* Circle button in tab bar* Bottom bar tabs (ios tabs style)* Custom tabs (be creative just implement TabItem)* Independent ViewSwitcher* Bindable with ItemsSource",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/sharpnado.tabs/2.2.0/icon"
        });

        refreshView.IsRefreshing = false;
    }

    private async void HandleSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Guard.Against.Null(sender);

        var collectionView = (CollectionView)sender;

        if (collectionView.SelectedItem is LibraryModel library)
        {
            await Toast.Make($"{library.Title} Tapped", ToastDuration.Short).Show();
        }

        collectionView.SelectedItem = null;
    }

    private ObservableCollection<LibraryModel> MauiLibraries { get; } =
    [
        new LibraryModel
        {
            Title = "Microsoft.Maui",
            Description = ".NET Multi-platform App UI is a framework for building native device applications spanning mobile, tablet and desktop",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/microsoft.maui.graphics/9.0.0-preview.2.10293/icon"
        },
        new LibraryModel
        {
            Title = "CommunityToolkit.Maui",
            Description = "The .NET MAUI Community Toolkit is a collection of Animations, Behaviors, Converters, and Custom Views for development with .NET MAUI. It simplifies and demonstrates common developer tasks building iOS, Android, macOS and Windows apps with .NET MAUI.",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/communitytoolkit.maui/8.0.1/icon"
        },
        new LibraryModel
        {
            Title = "CommunityToolkit.Maui.Markup",
            Description = "CommunityToolkit.Maui.Markup is a set of fluent helper methods and classes to simplify building declarative .NET MAUI user interfaces in C#",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/communitytoolkit.maui/8.0.1/icon"
        },
        new LibraryModel
        {
            Title = "CommunityToolkit.MVVM",
            Description = "This package includes a .NET MVVM library with helpers such as ObservableObject, ObservableRecipient, ObservableValidator, RelayCommand, AsyncRelayCommand, etc.",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/communitytoolkit.mvvm/8.2.2/icon"
        },
        new LibraryModel
        {
            Title = "Sentry.Maui",
            Description = "Bad software is everywhere, and we're tired of it. Sentry is on a mission to help developers write better software faster, so we can get back to enjoying technology. If you want to join us, check out our open positions.",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/sentry.maui/4.2.1/icon"
        },
        new LibraryModel
        {
            Title = "Syncfusion.Maui.Core",
            Description = "This package contains common classes and interfaces that are used in other Syncfusion .NET MAUI controls.",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/syncfusion.maui.core/25.1.38/icon"
        },
        new LibraryModel
        {
            Title = "Esri.ArcGISRuntime.Maui",
            Description = "Contains APIs and UI controls for building mapping and location analysis applications for .NET Android, .NET iOS, MacOS, and Windows with ArcGIS Maps SDK for .NET.",
            ImageSource = "https://api.nuget.org/v3-flatcontainer/esri.arcgisruntime.maui/200.3.0/icon"
        }
    ];
}
