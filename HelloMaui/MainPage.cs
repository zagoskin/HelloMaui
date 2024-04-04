using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace HelloMaui;
public sealed class MainPage : BaseContentPage
{
    const int ImageWidth = 500;
    const int ImageHeight = 250;
    const int LabelHeight = 32;
    const int EntryHeight = 40;
    const int DefaultSpacing = 12;
    
    static readonly Color DarkerGray = Color.FromArgb("#2B2B2B");
    public MainPage() : base()
    {
        BackgroundColor = Colors.DarkViolet;

        Content = new ScrollView()
        {
            Content = new Grid
            {
                RowDefinitions = Rows.Define(
                    (Row.Image, Star),
                    (Row.Label, LabelHeight),
                    (Row.Entry, EntryHeight),
                    (Row.LargeTextLabel, 500)),

                RowSpacing = DefaultSpacing,

                ColumnDefinitions = Columns.Define(
                    (Column.EntryOne, Star),
                    (Column.EntryTwo, Star),
                    (Column.EntryThree, Star)),

                ColumnSpacing = DefaultSpacing,

                BackgroundColor = Colors.AliceBlue,

                Children =
                {
                    new Image()
                        .Size(ImageWidth, ImageHeight)
                        .Aspect(Aspect.AspectFit)
                        .Source("dotnet_bot")
                        .Row(Row.Image)
                        .ColumnSpan(All<Column>()),

                    new Label()
                        .Text("Hello MAUI!", textColor: Colors.Black)
                        .Height(LabelHeight)
                        .Center()
                        .TextCenter()
                        .Row(Row.Label)
                        .ColumnSpan(All<Column>())
                        ,

                    new Entry()
                        .Placeholder("First Entry", DarkerGray)
                        .TextColor(Colors.Black)
                        .Row(Row.Entry)
                        .Column(Column.EntryOne),
                    new Entry()
                        .Placeholder("Second Entry", DarkerGray)
                        .TextColor(Colors.Black)
                        .Row(Row.Entry)
                        .Column(Column.EntryTwo),
                    new Entry()
                        .Placeholder("Third Entry", DarkerGray)
                        .TextColor(Colors.Black)
                        .Row(Row.Entry)
                        .Column(Column.EntryThree),

                    new Label { LineBreakMode = LineBreakMode.WordWrap }
                        .Text("LARGE TEXT", DarkerGray)
                        .Font(size: 100)
                        .TextCenter()
                        .Row(Row.LargeTextLabel)
                        .ColumnSpan(All<Column>())
                }
            }.Top().CenterHorizontal()
        };
    }

    enum Row
    { 
        Image, 
        Label, 
        Entry,
        LargeTextLabel
    };
    enum Column
    {
        EntryOne,
        EntryTwo,
        EntryThree
    } 
}
