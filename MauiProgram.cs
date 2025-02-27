#if ANDROID
using Android.App;
#endif

using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Push;

namespace shinypushbug;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseShiny()
            .AddFirebase()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder AddFirebase(this MauiAppBuilder builder)
    {
        builder.Services.AddPushFirebaseMessaging<ThePushDelegate>(
            new FirebaseConfiguration(
                false,
#if IOS
                builder.Configuration["Firebase:AppleAppId"],
#elif ANDROID
                builder.Configuration["Firebase:AndroidAppId"],
#endif
                builder.Configuration["Firebase:ProjectNumber"],
                builder.Configuration["Firebase:ProjectId"],
                builder.Configuration["Firebase:ApiKey"]
#if ANDROID
                , DefaultChannel
#endif
            )
        );

        return builder;
    }


#if ANDROID
    static NotificationChannel DefaultChannel => new(
        "default_channel",
        "Default Channel",
        NotificationImportance.Default
    )
    {
        LockscreenVisibility = NotificationVisibility.Public
    };
#endif
}
