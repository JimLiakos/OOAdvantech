using Xamarin.Forms;

namespace OOAdvantechApp
{
	public class App : Application
	{
		public App ()
		{
            MainPage = new HybridWebViewPage();
			//MainPage = new NavigationPage(new HybridWebViewPage ());
            //MainPage = new NavigationPage(new MainPage());
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

