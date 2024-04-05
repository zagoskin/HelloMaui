namespace HelloMaui;

using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
public class MauiLibrariesDataTemplate : DataTemplate
{
    const int ImageRadius = 25;
    const int ImagePadding = 8;

    public MauiLibrariesDataTemplate() : base(CreateGridTemplate)
    {
        
    }

    private static Grid CreateGridTemplate() => new()
    {
        RowDefinitions = Rows.Define(
            (Row.Title, 22),
            (Row.Description, 44),
            (Row.BottomPadding, 8)),

        ColumnDefinitions = Columns.Define(
            (Column.Icon, ImageRadius * 2 + ImagePadding * 2),
            (Column.Text, Star)),

        RowSpacing = 4,

        Children =
        {
            new Image()
                .Row(Row.Title).RowSpan(2)
                .Column(Column.Icon)
                .Center()
                .Aspect(Aspect.AspectFit)
                .Size(ImageRadius * 2)
                .Bind(Image.SourceProperty, 
                    getter: (LibraryModel model) => model.ImageSource,
                    mode: BindingMode.OneWay),

            new Label()
                .Row(Row.Title)
                .Column(Column.Text)
                .Font(size: 18, bold: true)
                .TextColor(Color.FromArgb("#262626"))
                .TextTop()
                .TextStart()
                .Bind(Label.TextProperty,
                    getter: (LibraryModel model) => model.Title,
                    mode: BindingMode.OneWay),

            new Label { MaxLines = 2, LineBreakMode = LineBreakMode.TailTruncation }
                .Row(Row.Description)
                .Column(Column.Text)
                .Font(size: 12)
                .TextColor(Color.FromArgb("#595959"))
                .TextTop()
                .TextStart()
                .Paddings(right: 12)
                .Bind(Label.TextProperty,
                    getter: (LibraryModel model) => model.Description,
                    mode: BindingMode.OneWay),
        }

    };

    enum Column { Icon, Text }
    enum Row { Title, Description, BottomPadding }
}
