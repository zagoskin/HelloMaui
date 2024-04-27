using HelloMaui.Infrastructure.Refit;
using HelloMaui.Models;
using Refit;
using System.Runtime.CompilerServices;

namespace HelloMaui.Services;
internal sealed class MauiLibrariesApiService : IMauiLibrariesService
{
    private readonly IMauiLibrariesClient _client;

    public MauiLibrariesApiService(IMauiLibrariesClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerable<LibraryModel> GetLibrariesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        List<MauiLibraryResponse> libraries = [];
        try
        {
            libraries = await _client.GetMauiLibrariesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        foreach (var lib in libraries)
        {
            yield return new LibraryModel(
                lib.Title,
                lib.Description,
                lib.ImageSource);
        }
    }
}
