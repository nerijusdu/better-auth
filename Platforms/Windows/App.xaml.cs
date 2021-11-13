using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BetterAuth.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            Microsoft.Maui.Handlers.WindowHandler.WindowMapper.Add(nameof(IWindow), (handler, view) =>
            {
                var nativeWindow = handler.NativeView;
                nativeWindow.Activate();
                IntPtr windowHandle = PInvoke.User32.GetActiveWindow();

                PInvoke.User32.SetWindowPos(windowHandle, 
                    PInvoke.User32.SpecialWindowHandles.HWND_TOP,
                    0, 0, 300, 300,  // width and height are ints
                    PInvoke.User32.SetWindowPosFlags.SWP_NOMOVE);
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            Microsoft.Maui.Essentials.Platform.OnLaunched(args);
        }
    }
}
