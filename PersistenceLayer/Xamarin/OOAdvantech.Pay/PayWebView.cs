using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OOAdvantech.Pay
{
    /// <MetaDataID>{77781d4e-65bc-4132-8ff4-0c1423eb2f84}</MetaDataID>
    public class PayWebView : View
    {

        public PayWebView()
        {

        }
        public void OnNavigated(NavigatedEventArgs navigatedEventArgs)
        {
            try
            {
                Navigated?.Invoke(this, navigatedEventArgs);
            }
            catch (Exception error)
            {
            }
        }

        public event NavigatedHandler Navigated;


        Action<string> action;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(PayWebView),
            defaultValue: default(string));

        public bool EnableRestApi
        {
            get { return (bool)GetValue(EnableRestApiProperty); }
            set { SetValue(EnableRestApiProperty, value); }
        }


        public static readonly BindableProperty EnableRestApiProperty = BindableProperty.Create(
            propertyName: "EnableRestApi",
            returnType: typeof(bool),
            declaringType: typeof(PayWebView),
            defaultValue: default(bool));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set
            {
                SetValue(UriProperty, value);
                if (NativeWebBrowser != null)
                {
                    NativeWebBrowser.Navigate(Uri);
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "NativeWebBrowser Navigate" });

#endif
                }
            }
        }


        public void RegisterAction(Action<string> callback)
        {
            action = callback;
        }

        public void Cleanup()
        {
            action = null;
        }

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }

        public INativeWebBrowser NativeWebBrowser;

        public virtual void GoBack()
        {
            NativeWebBrowser.GoBack();

        }
        public virtual void RefreshPage()
        {
            NativeWebBrowser.RefreshPage();
        }

        public bool CanGoBack
        {
            get
            {
                return NativeWebBrowser.CanGoBack;
            }
        }

    }



    /// <MetaDataID>{c9d9b7f0-348a-4ee3-a5c8-55435ecc2459}</MetaDataID>
    public interface INativeWebBrowser
    {
        Task<string> InvockeJSMethod(string methodName, object[] args);

        void GoBack();
        void Navigate(string url);

        void RefreshPage();


        string Url { get; }

        bool CanGoBack { get; }

    }

    /// <MetaDataID>{0d3b1cb2-e19e-4ce3-9f46-8660cc56ecf9}</MetaDataID>
    public class NavigatedEventArgs : EventArgs
    {
        public NavigatedEventArgs(object browser, string address, bool canGoBack, bool canGoForward)
        {
            Browser = browser;
            Address = address;
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
        }

        public string Address { get; private set; }

        //
        // Summary:
        //     Access to the underlying CefSharp.IBrowser object
        public object Browser { get; private set; }
        //
        // Summary:
        //     Returns true if the browser can navigate backwards.
        public bool CanGoBack { get; private set; }
        //
        // Summary:
        //     Returns true if the browser can navigate forwards.
        public bool CanGoForward { get; private set; }

    }

    public delegate void NavigatedHandler(object sender, NavigatedEventArgs e);
}
