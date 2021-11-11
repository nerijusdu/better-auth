using BetterAuth.Services;
using Microsoft.Toolkit.Uwp.Notifications;

namespace BetterAuth.Platforms.Windows;

public class NotificationService : INotificationService
{
    public NotificationService()
    {
        ToastNotificationManagerCompat.OnActivated += (notification) =>
        {
            var code = notification.Argument;
            WindowsClipboard.SetText(code);
        };
    }

    public void ShowNotification(string title, string body)
    {
        WindowsClipboard.SetText(body);

        new ToastContentBuilder()
            .AddToastActivationInfo(body, ToastActivationType.Background)
            //.AddAppLogoOverride(new Uri("ms-appx:///Assets/dotnet_bot.png"))
            .AddText(title, hintStyle: AdaptiveTextStyle.Header)
            .AddText(body, hintStyle: AdaptiveTextStyle.Body)
            .Show();
    }
}
