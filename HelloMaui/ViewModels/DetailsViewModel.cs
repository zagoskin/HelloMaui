using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelloMaui.Models;

namespace HelloMaui.ViewModels;
public partial class DetailsViewModel : BaseViewModel, IQueryAttributable
{
    public const string ModelKey = "Model";

    [ObservableProperty]
    private ImageSource? _libraryImageSource;

    [ObservableProperty]
    private string? _libraryTitle;

    [ObservableProperty]
    private string? _libraryDescription;

    [RelayCommand]
    public static async Task HandleGoBackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count is 0)
        {
            return;
        }

        if (query.TryGetValue(ModelKey, out var value) && value is LibraryModel model)
        {
            LibraryImageSource = model.ImageSource;
            LibraryTitle = model.Title;
            LibraryDescription = model.Description;
        }
        return;
    }
}
