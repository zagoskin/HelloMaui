namespace HelloMaui.Models;
public class LibraryModel
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageSource { get; init; } = null!;

    public LibraryModel(string title, string description, string imageSource)
    {
        Title = title;
        Description = description;
        ImageSource = imageSource;
    }

    public LibraryModel(string title, string description, Uri imageSource)
    {
        Title = title;
        Description = description;
        ImageSource = imageSource.AbsoluteUri;
    }

    public LibraryModel() { } // sqlite-net-pcl requires a parameterless constructor
}
