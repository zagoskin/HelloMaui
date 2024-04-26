namespace HelloMaui.Infrastructure.Refit;
internal sealed class MauiLibraryResponse
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required Uri ImageSource { get; init; }
}
