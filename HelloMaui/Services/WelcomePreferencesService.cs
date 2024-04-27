namespace HelloMaui.Services;
public sealed class WelcomePreferencesService
{
    private readonly IPreferences _preferences;

    public WelcomePreferencesService(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public bool IsFirstRun
    {
        get => _preferences.Get(nameof(IsFirstRun), true);
        set => _preferences.Set(nameof(IsFirstRun), value);
    }
}
