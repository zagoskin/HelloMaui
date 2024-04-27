using HelloMaui.Models;

namespace HelloMaui.Services;

public interface IMauiLibrariesService
{
    Task<List<LibraryModel>> GetLibrariesAsync(CancellationToken cancellationToken = default);
}