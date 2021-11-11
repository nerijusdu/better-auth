using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using BetterAuth.Data;
using BetterAuth.Services;
using Microsoft.Maui.LifecycleEvents;

namespace BetterAuth
{
    public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.RegisterBlazorMauiWebView()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddBlazorWebView();
			builder.Services.AddSingleton<WeatherForecastService>();
			builder.Services.AddSingleton<IBroadcastingService, BroadcastingService>();
			builder.Services.AddSingleton<IStorageService, StorageService>();
#if __ANDROID__
			builder.Services.AddSingleton<ISmsService, Platforms.Android.SmsService>();
#endif

#if WINDOWS
			builder.Services.AddSingleton<INotificationService, Platforms.Windows.NotificationService>();
#else
			builder.Services.AddSingleton<INotificationService, DummyNotificationService>();
#endif

			return builder.Build();
		}
	}
}