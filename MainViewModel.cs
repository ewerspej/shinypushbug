using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shiny;
using Shiny.Push;

namespace shinypushbug;

public partial class MainViewModel : ObservableObject
{
    private readonly IPreferences _preferences;
    private readonly IPushManager _pushManager;

    public MainViewModel(IPreferences preferences, IPushManager pushManager)
    {
        _preferences = preferences;
        _pushManager = pushManager;
        FcmToken = _preferences.Get("token", string.Empty);
    }

    [ObservableProperty]
    private string _fcmToken;

    [RelayCommand]
    private async Task ToggleRegistrationAsync()
    {
        try
        {
            if (_pushManager.RegistrationToken is not null)
            {
                await _pushManager.UnRegister();
                FcmToken = string.Empty;
                _preferences.Remove("token");
                return;
            }

            var (accessState, token) = await _pushManager.RequestAccess();
            if (accessState != AccessState.Available)
            {
                Console.WriteLine("Failed to register:" + accessState);
            }

            FcmToken = token ?? string.Empty;
            _preferences.Set("token", FcmToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}