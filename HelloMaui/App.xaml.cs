using System.Diagnostics;

namespace HelloMaui;

public partial class App : Application
{
    public App(AppShell shell)
    {
        InitializeComponent();

        MainPage = shell;        
    }

    protected override void OnResume()
    {
        base.OnResume();        

        Trace.WriteLine("***** APP RESUMED *****");
    }
    

    protected override void OnSleep()
    {
        base.OnSleep();        

        Trace.WriteLine("***** APP SLEEPING *****");
    }

    protected override void OnStart()
    {
        base.OnStart();

        Trace.WriteLine("***** APP STARTED *****");
    }
}
