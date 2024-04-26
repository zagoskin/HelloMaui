using HelloMaui.Models;
using Refit;

namespace HelloMaui.Infrastructure.Refit;

internal interface IMauiLibrariesClient
{
    [Get("/default/MauiLibraries")]
    Task<List<MauiLibraryResponse>> GetMauiLibrariesAsync();
}