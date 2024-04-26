using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelloMaui.Models;
using HelloMaui.Pages;
using HelloMaui.Services;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net;

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

    private IReadOnlyList<LibraryModel> _originalLibraries = [];
    private readonly IDispatcher _dispatcher;
    private readonly IMauiLibrariesService _mauiLibrariesService;

    public ListViewModel(IDispatcher dispatcher, IMauiLibrariesService mauiLibrariesService)
    {
        _dispatcher = dispatcher;
        _mauiLibrariesService = mauiLibrariesService;
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
    private async Task ApplyFilter()
    {
        await _dispatcher.DispatchAsync(MauiLibraries.Clear).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(SearchBarText))
        {
            foreach (var item in _originalLibraries)
            {
                await _dispatcher.DispatchAsync(() => MauiLibraries.Add(item)).ConfigureAwait(false);
            }
            return;
        }

        foreach (var item in _originalLibraries.Where(x => x.Title.Contains(SearchBarText, StringComparison.OrdinalIgnoreCase)))
        {
            await _dispatcher.DispatchAsync(() => MauiLibraries.Add(item)).ConfigureAwait(false);
        }        
    }

    [RelayCommand]
    private async Task Refresh()
    {        
        IsSearchBarEnabled = false;

        await using (var delay = new SimulatedDelay(_minimumRefreshTime))
        {
            if (_originalLibraries.Count is 0)
            {
                _originalLibraries = await _mauiLibrariesService.GetLibrariesAsync().ConfigureAwait(false);
            }
            MauiLibraries.Clear();

            foreach (var item in _originalLibraries.Where(lib => !MauiLibraries.Any(x => x.Title.Equals(lib.Title, StringComparison.OrdinalIgnoreCase))))
            {
                await _dispatcher.DispatchAsync(() => MauiLibraries.Add(item)).ConfigureAwait(false);
            }
        }
        //try
        //{
        //    if (_originalLibraries.Count is 0)
        //    {
        //        _originalLibraries = await _mauiLibrariesService.GetLibrariesAsync().ConfigureAwait(false);
        //    }        
        //    MauiLibraries.Clear();

        //    foreach (var item in _originalLibraries.Where(lib => !MauiLibraries.Any(x => x.Title.Equals(lib.Title, StringComparison.OrdinalIgnoreCase))))
        //    {
        //        await _dispatcher.DispatchAsync(() => MauiLibraries.Add(item)).ConfigureAwait(false);
        //    }
        //}
        //finally
        //{
        //    await Task.Delay(_minimumRefreshTime).ConfigureAwait(false);
        //}
        IsRefreshing = false;
        IsSearchBarEnabled = true;

    }

    
}

public class SimulatedDelay : IAsyncDisposable, IDisposable
{
    private bool _disposedValue;
    private readonly TimeSpan _delay;

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~SimulatedDelay()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public SimulatedDelay(TimeSpan delay)
    {
        _delay = delay;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();

        Dispose(disposing: false);

        GC.SuppressFinalize(this);
    }

    private async Task DisposeAsyncCore()
    {
        await Task.Delay(_delay).ConfigureAwait(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _disposedValue = true;
        }
    }
    

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
