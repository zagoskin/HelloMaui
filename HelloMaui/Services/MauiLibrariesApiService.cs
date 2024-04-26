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

    public async Task<List<LibraryModel>> GetLibrariesAsync()
    {
        try
        {
            var libraries = await _client.GetMauiLibrariesAsync().ConfigureAwait(false);
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
