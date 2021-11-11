using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Microsoft.Maui;

namespace BetterAuth
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            // TODO: this dont work https://github.com/davidortinau/WeatherTwentyOne/blob/main/src/WeatherTwentyOne/Platforms/Android/MainActivity.cs
            var read = ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadSms);
            var receive = ContextCompat.CheckSelfPermission(this, Manifest.Permission.BroadcastSms);
            if (read != (int)Permission.Granted || receive != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.ReadSms, Manifest.Permission.ReceiveSms }, 1);
            }

            base.OnCreate(savedInstanceState, persistentState);
        }
    }
}