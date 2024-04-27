using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using HelloMaui.Database;
using HelloMaui.Handlers;
using HelloMaui.Infrastructure.Refit;
using HelloMaui.Pages;
using HelloMaui.Services;
using HelloMaui.Services.GraphQL;
using HelloMaui.ViewModels;
using HelloMaui.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;

namespace HelloMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CalendarView, CalendarHandler>();
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.Services
            .AddSingleton<AppShell>()
            .AddSingleton<App>()
            .AddSingleton<IMauiLibrariesService, MauiLibrariesGraphQLService>()
            .AddSingleton<WelcomePreferencesService>()
            .AddSingleton<IPreferences>(Preferences.Default)
            .AddSingleton<IFileSystem>(FileSystem.Current)
            .AddSingleton<LibraryModelRepository>(); 

        builder.Services.AddRefitClient<IMauiLibrariesClient>()
            .ConfigureHttpClient(
                static c => c.BaseAddress = new Uri("https://6dhbgfw1de.execute-api.us-west-1.amazonaws.com"))
            .AddStandardResilienceHandler(
                static configure => configure.Retry = new MobileHttpRetryStrategyOptions());
        builder.Services.AddLibrariesGraphql()
            .ConfigureHttpClient(
                static c => c.BaseAddress = new Uri("https://t41fbiwwda.execute-api.us-west-1.amazonaws.com/graphql/"),
                static builder => builder.AddStandardResilienceHandler(configure => configure.Retry = new MobileHttpRetryStrategyOptions()));
            

        builder.Services
            .AddTransient<ListPage>()
            .AddTransient<ListViewModel>()
            .AddTransient<DetailsPage>()
            .AddTransient<DetailsViewModel>()
            .AddTransient<CalendarPage>();

        return builder.Build();
    }
}

internal sealed class MobileHttpRetryStrategyOptions : HttpRetryStrategyOptions
{
    public MobileHttpRetryStrategyOptions()
    {
        BackoffType = DelayBackoffType.Exponential;
        MaxRetryAttempts = 3;
        UseJitter = true;
        Delay = TimeSpan.FromSeconds(2);
    }
}