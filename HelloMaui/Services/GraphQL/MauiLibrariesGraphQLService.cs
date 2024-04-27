using HelloMaui.Models;
using StrawberryShake;
using System.Runtime.CompilerServices;

namespace HelloMaui.Services.GraphQL;
internal sealed class MauiLibrariesGraphQLService : IMauiLibrariesService
{
    private readonly ILibrariesGraphql _client;

    public MauiLibrariesGraphQLService(ILibrariesGraphql client)
    {
        _client = client;
    }
    public async IAsyncEnumerable<LibraryModel> GetLibrariesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await _client.LibrariesQuery.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        response.EnsureNoErrors();

        if (response.Data?.Libraries is null)
        {
            throw new Exception("No libraries found");
        }

        foreach (var library in response.Data.Libraries)
        {
            yield return new LibraryModel(
                library.Title,
                library.Description,
                library.ImageSource);
        }            
    }
}
