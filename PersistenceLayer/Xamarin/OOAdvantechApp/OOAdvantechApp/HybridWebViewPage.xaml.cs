using System.Threading.Tasks;
using Xamarin.Forms;

namespace OOAdvantechApp
{
	public partial class HybridWebViewPage : ContentPage
	{
		public HybridWebViewPage ()
		{

            //"http://192.168.2.10/WebPart/index.html" 
            //"http://10.0.0.10/WebPart/index.html" 

            InitializeComponent();

            //hybridWebView.Uri = "local://Content/index.html";

            //hybridWebView.Uri = "http://169.254.80.80/WebPart/index.html";

            //hybridWebView.Uri = "http://169.254.80.80/DemoNPMTypeScript/index.html";
            hybridWebView.Uri = "http://192.168.2.10/DemoNPMTypeScript/index.html";


            //hybridWebView.Uri = "http://192.168.2.10/WebPart/index.html";
            //hybridWebView.Uri = "http://10.0.0.10/WebPart/index.html";

            //hybridWebView.RegisterAction (data => DisplayAlert ("Alert", "Hello " + data, "OK"));
            hybridWebView.RegisterAction(async (string data) =>
            {

                //await DisplayAlert("Alert", "Hello " + data, "OK");
                string res = await hybridWebView.NativeWebBrowser.InvockeJSMethod("logA", new[] { "Hello mama" });

                int rr = 0; 
            });
        }
	}
}
