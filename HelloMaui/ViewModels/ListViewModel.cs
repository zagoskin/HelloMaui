using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelloMaui.Database;
using HelloMaui.Models;
using HelloMaui.Pages;
using HelloMaui.Services;
using HelloMaui.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HelloMaui.ViewModels;
public partial class ListViewModel : BaseViewModel
{
    private static readonly TimeSpan _minimumRefreshTime = TimeSpan.FromMilliseconds(1000);

    [ObservableProperty]
    private string _searchBarText = string.Empty;

    [ObservableProperty]
    private bool _isSearchBarEnabled = true;

    [ObservableProperty]
    private bool _isRefreshing = false;

    [ObservableProperty]
    private object? _selectedItem;
    public ObservableCollection<LibraryModel> MauiLibraries { get; } = [];
    
    private readonly IDispatcher _dispatcher;
    private readonly IMauiLibrariesService _mauiLibrariesService;
    private readonly LibraryModelRepository _libraryModelDatabase;

    public ListViewModel(IDispatcher dispatcher, IMauiLibrariesService mauiLibrariesService, LibraryModelRepository libraryModelDatabase)
    {
        _dispatcher = dispatcher;
        _mauiLibrariesService = mauiLibrariesService;
        _libraryModelDatabase = libraryModelDatabase;
    }

    [RelayCommand]
    private async Task SelectionChanged()
    {        
        if (SelectedItem is LibraryModel library)
        {
            await Shell.Current.GoToAsync(AppShell.GetRoute<DetailsPage>(), new Dictionary<string, object>
            {
                { DetailsViewModel.ModelKey, library }
            });
        }

        SelectedItem = null;
    }

    [RelayCommand]
    private async Task ApplyFilter(CancellationToken token)
    {
        if (IsRefreshing)
        {
            return;
        }
        await _dispatcher.DispatchAsync(MauiLibraries.Clear).ConfigureAwait(false);
        var existing = new List<LibraryModel>(MauiLibraries);
        var libraries = await _libraryModelDatabase.GetLibrariesAsync(token).ConfigureAwait(false);

        var allDistinct = existing.Concat(libraries).DistinctBy(l => l.Title.ToLower()).ToList();

        if (string.IsNullOrWhiteSpace(SearchBarText))
        {
            foreach (var item in allDistinct)
            {
                await _dispatcher.DispatchAsync(() => MauiLibraries.Add(item)).ConfigureAwait(false);
            }
            return;
        }

        foreach (var item in allDistinct.Where(x => x.Title.Contains(SearchBarText, StringComparison.OrdinalIgnoreCase)))        
        {
            await _dispatcher.DispatchAsync(() => MauiLibraries.Add(item)).ConfigureAwait(false);
        }        
    }

    [RelayCommand]
    private async Task Refresh()
    {        
        IsSearchBarEnabled = false;

        CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

        var libraries = await _libraryModelDatabase.GetLibrariesAsync(cts.Token).ConfigureAwait(false);
        await using (var delay = new SimulatedDelay(_minimumRefreshTime, () => { IsRefreshing = false; IsSearchBarEnabled = true; }))
        {
            if (libraries.Count is 0)
            {
                await _mauiLibrariesService.GetLibrariesAsync(cts.Token)
                    .ForEachAwaitWithCancellationAsync((item, ct) =>
                    {
                        if (MauiLibraries.Any(x => x.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase)))
                        {
                            return Task.CompletedTask;
                        }
                        return _dispatcher.DispatchAsync(() => MauiLibraries.Add(item));
                    }, cts.Token)
                    .ConfigureAwait(false);
                await _libraryModelDatabase.InsertAllLibraries(MauiLibraries, cts.Token).ConfigureAwait(false);
            }
        }  
    }

    
}