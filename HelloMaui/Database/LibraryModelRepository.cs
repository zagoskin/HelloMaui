using HelloMaui.Models;

namespace HelloMaui.Database;
public sealed class LibraryModelRepository(IFileSystem fileSystem) : BaseRepository<LibraryModel>(fileSystem)
{
    public Task<List<LibraryModel>> GetLibrariesAsync(CancellationToken token)
    {
        return Execute(connection =>
        {
            return connection.Table<LibraryModel>().ToListAsync();
        }, token);
    }

    public Task InsertAllLibraries(IEnumerable<LibraryModel> libraryModels, CancellationToken token)
    {
        return Execute(connection =>
        {
            return connection.InsertAllAsync(libraryModels);
        }, token);
    }
}
