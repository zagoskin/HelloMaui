using HelloMaui.Models;
using Refit;
using System.Threading;

namespace HelloMaui.Infrastructure.Refit;

internal interface IMauiLibrariesClient
{
    [Get("/default/MauiLibraries")]
    Task<List<MauiLibraryResponse>> GetMauiLibrariesAsync(CancellationToken cancellationToken = default);
}