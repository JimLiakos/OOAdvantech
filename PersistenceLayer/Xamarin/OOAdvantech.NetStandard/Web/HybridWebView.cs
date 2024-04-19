using System;
using System.IO.Compression;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.Linq;

namespace OOAdvantech.Web
{
    /// <MetaDataID>{77781d4e-65bc-4132-8ff4-0c1423eb2f84}</MetaDataID>
    public class HybridWebView : WebView
    {

        public HybridWebView()
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

        public bool OnShouldOverrideUrlLoading(string url)
        {

            if (ShouldOverrideUrlLoading != null)
                return ShouldOverrideUrlLoading(url);
            else
                return false;
        }


        public event NavigatedHandler Navigated;

        public ShouldOverrideUrlLoadingHandler ShouldOverrideUrlLoading;

        Action<string> action;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public bool EnableRestApi
        {
            get { return (bool)GetValue(EnableRestApiProperty); }
            set { SetValue(EnableRestApiProperty, value); }
        }


        public static readonly BindableProperty EnableRestApiProperty = BindableProperty.Create(
            propertyName: "EnableRestApi",
            returnType: typeof(bool),
            declaringType: typeof(HybridWebView),
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

        public static void WebAppUpdate(string url)
        {
            try
            {
                var webAppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                if (File.Exists(Path.Combine(webAppPath, "appHeader.txt")))
                {
                    if (File.ReadAllText(Path.Combine(webAppPath, "appHeader.txt")) == url)
                        return;
                    File.Delete(Path.Combine(webAppPath, "appHeader.txt"));
                }




                using (WebClient wc = new WebClient())
                {

                    var webAppBundl = wc.DownloadData(url);

                    //var downloadStream = downloadStreamTask.Result;
                    using (MemoryStream memoryStream = new MemoryStream(webAppBundl))
                    {
                        ZipArchive zip = new ZipArchive(memoryStream);
                        if (!Directory.Exists(Path.Combine(webAppPath, "webapp")))
                            Directory.CreateDirectory(Path.Combine(webAppPath, "webapp"));
                        else
                        {
                            if (File.Exists("/var/mobile/Containers/Data/Application/875AE1C5-985A-463B-A71E-151A38DAEC33/Documents/webapp/index.html"))
                            {
                                System.Diagnostics.Debug.WriteLine("Exist:  /var/mobile/Containers/Data/Application/875AE1C5-985A-463B-A71E-151A38DAEC33/Documents/webapp/index.html");
                            }
                            Directory.Delete(Path.Combine(webAppPath, "webapp"), true);
                            Directory.CreateDirectory(Path.Combine(webAppPath, "webapp"));
                        }


                        var files= zip.Entries.Select(x => x.FullName).ToArray();

                        foreach (var entry in zip.Entries)
                        {

                            webAppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // iOS: Environment.SpecialFolder.Resources
                            webAppPath += "/webapp";
                            var pathArray = entry.FullName.Split('/');
                            int i = 0;
                            foreach (string dir in pathArray)
                            {
                                i++;
                                if (i < pathArray.Length)
                                {
                                    if (!Directory.Exists(Path.Combine(webAppPath, dir)))
                                        Directory.CreateDirectory(Path.Combine(webAppPath, dir));
                                    webAppPath += "/" + dir;
                                }

                            }

                            if (entry.Length==0)
                                continue;

                            var filePath = Path.Combine(webAppPath, entry.Name);
                            var entryStream = entry.Open();
                            byte[] entryBuffer;
                            using (var ms = new MemoryStream())
                            {
                                entryStream.CopyTo(ms);
                                entryBuffer = ms.ToArray();
                            }
                            entryStream.Read(entryBuffer, 0, entryBuffer.Length);
                            entryStream.Close();

                            File.WriteAllBytes(filePath, entryBuffer);

                            if (entry.Name.ToLower() == "index.html")
                            {
                                bool exist = System.IO.File.Exists(filePath);
                                System.Diagnostics.Debug.WriteLine(filePath);
                            }
                        }
                    }



                }

                webAppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                File.WriteAllText(Path.Combine(webAppPath, "appHeader.txt"), url);

            }
            catch (Exception error)
            {


            }
            if (File.Exists("/var/mobile/Containers/Data/Application/875AE1C5-985A-463B-A71E-151A38DAEC33/Documents/webapp/index.html"))
            {
                System.Diagnostics.Debug.WriteLine("Exist:  /var/mobile/Containers/Data/Application/875AE1C5-985A-463B-A71E-151A38DAEC33/Documents/webapp/index.html");
            }




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

    public delegate bool ShouldOverrideUrlLoadingHandler(string url);

}
