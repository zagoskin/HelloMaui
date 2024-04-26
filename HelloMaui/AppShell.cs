using HelloMaui.Pages;

namespace HelloMaui;
public class AppShell : Shell
{
    public AppShell(ListPage mainPage)
    {
        //FlyoutHeader = new Label
        //{
        //    Text = "Hello, .NET MAUI!",
        //    FontSize = 24,
        //    HorizontalOptions = LayoutOptions.Center
        //};

        Items.Add(mainPage);

        CreateRoutes();
    }

    private static void CreateRoutes()
    {
        Routing.RegisterRoute(GetRoute<ListPage>(), typeof(ListPage));
        Routing.RegisterRoute(GetRoute<DetailsPage>(), typeof(DetailsPage));
        Routing.RegisterRoute(GetRoute<CalendarPage>(), typeof(CalendarPage));
    }

    public static string GetRoute<T>() where T : ContentPage
    {
        var type = typeof(T);
        return type switch
        {
            var _ when type == typeof(DetailsPage) => $"//{nameof(ListPage)}/{nameof(DetailsPage)}",
            var _ when type == typeof(ListPage) => $"//{nameof(ListPage)}",
            var _ when type == typeof(CalendarPage) => $"//{nameof(ListPage)}/{nameof(CalendarPage)}",
            _ => throw new NotImplementedException($"{typeof(T).FullName} not found in Routing Table")
        };
    }
}
