using Application = Microsoft.Maui.Controls.Application;

namespace BetterAuth
{
    public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}
	}
}
