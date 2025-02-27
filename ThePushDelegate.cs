using Shiny.Push;

#if IOS
using UIKit;
using UserNotifications;
#endif

namespace shinypushbug;

public partial class ThePushDelegate : PushDelegate { }

#if IOS
public partial class ThePushDelegate : IApplePushDelegate
{
    // return null for default value
    public UNNotificationPresentationOptions? GetPresentationOptions(PushNotification notification)
        // show notification when UI is up
        => UNNotificationPresentationOptions.Banner | UNNotificationPresentationOptions.Sound;

    public UIBackgroundFetchResult? GetFetchResult(PushNotification notification) => null;
}
#endif