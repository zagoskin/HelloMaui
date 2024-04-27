using HelloMaui.Infrastructure.Refit;
using HelloMaui.Models;
using Refit;

namespace HelloMaui.Services;
internal sealed class MauiLibrariesApiService : IMauiLibrariesService
{
    private readonly IMauiLibrariesClient _client;

    public MauiLibrariesApiService(IMauiLibrariesClient client)
    {
        _client = client;
    }

    public async Task<List<LibraryModel>> GetLibrariesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var libraries = await _client.GetMauiLibrariesAsync(cancellationToken).ConfigureAwait(false);
            return libraries.ConvertAll(l => new LibraryModel(
                l.Title,
                l.Description,
                l.ImageSource));
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
    }
}
