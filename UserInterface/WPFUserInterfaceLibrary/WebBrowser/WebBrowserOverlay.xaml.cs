using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Navigation;
using CefSharp.Wpf;
using CefSharp;
//using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System.Threading.Tasks;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Json;
using WPFUIElementObjectBind;
using UIBaseEx;
using System.IO;
//using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace GenWebBrowser
{

    //
    // Summary:
    //     Event arguments to the LoadingStateChanged event handler set up in IWebBrowser.
    /// <MetaDataID>{df105f1c-860f-441b-b454-cce3fecda07a}</MetaDataID>
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

    /// <MetaDataID>{7c87e9bf-4d60-414f-82a7-784c28bab520}</MetaDataID>
    public class LocalGetData
    {
        public bool Resolved;
        public string Data;
    }

    /// <MetaDataID>{282456be-64be-4352-8d74-c85d3bca5869}</MetaDataID>
    public class CustomProtocolResponse
    {
        public bool Resolved;
        public MemoryStream Stream;
    }



    public delegate void ProcessRequestHandler(Uri requestUri, CustomProtocolResponse response);

    public delegate void NavigatedHandler(object sender, NavigatedEventArgs e);
    /// <summary>
    /// Displays a WebBrowser control over a given placement target element in a WPF Window.
    /// The owner window can be transparent, but not this one, due mixing DirectX and GDI drawing. 
    /// WebBrowserOverlayWF uses WinForms to avoid this limitation.
    /// </summary>
    /// <MetaDataID>{151406ed-2c13-4ae2-a5bd-1d537a0de62a}</MetaDataID>
    public partial class WebBrowserOverlay : Window, IEndPoint, IHtmlView
    {

        public void DisposeBrowser()
        {

            if (ChromeBrowser != null && ProcessActiveBrowsers.ContainsKey(ChromeBrowser))
            {
                ChromeBrowser.Dispose();
               
                ProcessActiveBrowsers.Remove(ChromeBrowser);
                ChromeBrowser = null;
            }
        }

        public event EventHandler<object> DataContexRetrieving;

        FrameworkElement _placementTarget;
        public event LoadCompletedEventHandler LoadCompleted;



        public event ProcessRequestHandler ProcessRequest;
        //
        // Summary:
        //     Occurs when the document being navigated to is located and has started downloading.
        public event NavigatedHandler Navigated;
        //
        // Summary:
        //     Occurs just before navigation to a document.
        public event NavigatingCancelEventHandler Navigating;

        public object InvockeJSMethod(string methodName, object[] args, bool _async = false)
        {
            //_placementTarget.Dispatcher.BeginInvoke(delegate () => { });






            try
            {
                if (ChromeBrowser != null)
                {
                    string jsScriptArgs = null;
                    if (args.Length > 0)
                    {
                        foreach (var arg in args)
                        {
                            if (jsScriptArgs != null)
                                jsScriptArgs += ",";
                            if (arg is string)
                            {
                                if (arg != null)
                                    jsScriptArgs += "'" + (arg as string).Replace("\\", "\\\\").Replace("'", "\\'") + "'";
                                else
                                    jsScriptArgs += "'" + arg + "'";
                            }
                            else if (arg is double)
                                jsScriptArgs += ((double)arg).ToString(System.Globalization.CultureInfo.GetCultureInfo(1033));
                            else if (arg is float)
                                jsScriptArgs += ((float)arg).ToString(System.Globalization.CultureInfo.GetCultureInfo(1033));
                            else if (arg is decimal)
                                jsScriptArgs += ((decimal)arg).ToString(System.Globalization.CultureInfo.GetCultureInfo(1033));
                            else
                                jsScriptArgs += arg.ToString();

                        }
                    }
                    string jsScriptMethodCall = methodName + "(" + jsScriptArgs + ");";
                    object result = null;
                    if (!_async)
                    {
                        return Application.Current.Dispatcher.Invoke(delegate ()
                         {
                             var task = ChromeBrowser.EvaluateScriptAsync(jsScriptMethodCall);
                             task.Wait(10000);
                             result = task.Result.Result;
                             return result;
                         });
                    }
                    else
                    {
                        var retVal = Task<object>.Run(async () =>
                          {
                              JavascriptResponse javascriptResponse = await Application.Current.Dispatcher.Invoke(delegate ()
                               {
                                   var task = ChromeBrowser.EvaluateScriptAsync(jsScriptMethodCall);

                                   return task;
                                   //task.Wait(10000);
                                   //result = task.Result.Result;
                                   //return result;
                               });

                              if (javascriptResponse.Success)
                              {

                              }
                              else
                              {

                              }

                          });
                        return retVal;
                    }


                }
                Application.Current.Dispatcher.Invoke(delegate ()
                {
                    if (IEWebBrowser != null)
                        IEWebBrowser.InvokeScript(methodName, args);


                    //if (EdgeWebBrowser != null)
                    //    EdgeWebBrowser.InvokeScript(methodName, TrasformToStrings(args));
                });

            }
            catch (Exception error)
            {
            }
            return null;
        }

        private static string[] TrasformToStrings(object[] args)
        {
            string[] argsAsString = new string[args.Length];
            int i = 0;
            foreach (var arg in args)
            {
                if (arg != null)
                    argsAsString[i] = arg.ToString();
                else
                    argsAsString[i] = "";
            }

            return argsAsString;
        }

        WebBrowser IEWebBrowser;

        // Microsoft.Toolkit.Win32.UI.Controls.WPF.WebView EdgeWebBrowser;

        bool _SuppressScriptErrors;
        public bool SuppressScriptErrors
        {
            set
            {
                _SuppressScriptErrors = value;
                if (IEWebBrowser != null)
                    IEWebBrowser.SuppressScriptErrors(_SuppressScriptErrors);

            }
            get
            {
                return _SuppressScriptErrors;
            }
        }

        public bool ConnectionIsOpen => true;

        public object InvokeScript(string methodName, params object[] args)
        {
            if (ChromeBrowser != null)
                ChromeBrowser.ExecuteScriptAsync(methodName, args);
            if (IEWebBrowser != null)
                IEWebBrowser.InvokeScript(methodName, args);

            //if (EdgeWebBrowser != null)
            //    EdgeWebBrowser.InvokeScript(methodName, TrasformToStrings(args));


            return null;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                if (ChromeBrowser != null)
                    ChromeBrowser.GetBrowser().GetHost().ShowDevTools();
            }
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Key == Key.F5)
                {
                    if (ChromeBrowser != null)
                        ChromeBrowser.GetBrowser().Reload(true);//.GetHost().ShowDevTools();
                }
            }
            if (e.Key == Key.BrowserBack)
            {
                InvockeJSMethod("BackButtonPress", new object[0], true);
            }
            base.OnKeyUp(e);
        }
        public void Navigate(string source)
        {
            if (IEWebBrowser != null)
                IEWebBrowser.Navigate(source);

            //if (EdgeWebBrowser != null)
            //    EdgeWebBrowser.Navigate(source);


            if (ChromeBrowser != null)
                ChromeBrowser.Address = source;
        }
        //
        // Summary:
        //     Navigate asynchronously to the document at the specified System.Uri.
        //
        // Parameters:
        //   source:
        //     The System.Uri to navigate to.
        //
        // Exceptions:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        //
        //   T:System.Security.SecurityException:
        //     Navigation from an application that is running in partial trust to a System.Uri
        //     that is not located at the site of origin.

        public void Navigate(Uri source)
        {

            if (IEWebBrowser != null)
                IEWebBrowser.Navigate(source.AbsoluteUri);


            //if (EdgeWebBrowser != null)
            //    EdgeWebBrowser.Navigate(source.AbsoluteUri);


            if (ChromeBrowser != null)
                ChromeBrowser.Address = source.AbsoluteUri;
        }

        public void Back()
        {
            if (ChromeBrowser != null)
                ChromeBrowser.Back();
        }

        //
        // Summary:
        //     Navigates asynchronously to the document at the specified URL and specify the
        //     target frame to load the document's content into. Additional HTTP POST data and
        //     HTTP headers can be sent to the server as part of the navigation request.
        //
        // Parameters:
        //   source:
        //     The URL to navigate to.
        //
        //   targetFrameName:
        //     The name of the frame to display the document's content in.
        //
        //   postData:
        //     HTTP POST data to send to the server when the source is requested.
        //
        //   additionalHeaders:
        //     HTTP headers to send to the server when the source is requested.
        public void Navigate(string source, string targetFrameName, byte[] postData, string additionalHeaders)
        {
            if (IEWebBrowser != null)
                IEWebBrowser.Navigate(source, targetFrameName, postData, additionalHeaders);
        }
        //
        // Summary:
        //     Navigate asynchronously to the document at the specified System.Uri and specify
        //     the target frame to load the document's content into. Additional HTTP POST data
        //     and HTTP headers can be sent to the server as part of the navigation request.
        //
        // Parameters:
        //   source:
        //     The System.Uri to navigate to.
        //
        //   targetFrameName:
        //     The name of the frame to display the document's content in.
        //
        //   postData:
        //     HTTP POST data to send to the server when the source is requested.
        //
        //   additionalHeaders:
        //     HTTP headers to send to the server when the source is requested.
        //
        // Exceptions:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        //
        //   T:System.Security.SecurityException:
        //     Navigation from an application that is running in partial trust:To a System.Uri
        //     that is not located at the site of origin, or targetFrameName name is not null
        //     or empty.
        public void Navigate(Uri source, string targetFrameName, byte[] postData, string additionalHeaders)
        {
            if (IEWebBrowser != null)
                IEWebBrowser.Navigate(source, targetFrameName, postData, additionalHeaders);
        }

        //
        // Summary:
        //     Navigate asynchronously to a System.IO.Stream that contains the content for a
        //     document.
        //
        // Parameters:
        //   stream:
        //     The System.IO.Stream that contains the content for a document.
        //
        // Exceptions:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        public void NavigateToStream(System.IO.Stream stream)
        {
            if (IEWebBrowser != null)
                IEWebBrowser.NavigateToStream(stream);

        }
        //
        // Summary:
        //     Navigate asynchronously to a System.String that contains the content for a document.
        //
        // Parameters:
        //   text:
        //     The System.String that contains the content for a document.
        //
        // Exceptions:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        public void NavigateToString(string text)
        {
            if (IEWebBrowser != null)
                IEWebBrowser.NavigateToString(text);

            //if (EdgeWebBrowser != null)
            //    EdgeWebBrowser.NavigateToString(text);
        }



        public WebBrowserOverlay()
        {
            if (!IsCefInitialized)
                Initcef();
        }



        static bool IsCefInitialized = false;

#if DEBUG
        public static void SetCefExtraCachePath(string extraCachePath)
        {
            if (!IsCefInitialized)
                Initcef(extraCachePath);
        }
#endif

        private static void Initcef(string extraCachePath = null)
        {
            if (IsCefInitialized)
                return;
            try
            {
                IsCefInitialized = true;
                string cachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Format(@"Microneme\{0}\CefSharp\Cache", AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "")));
                if (!string.IsNullOrWhiteSpace(extraCachePath))
                    cachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Format(@"Microneme\{0}\{1}\CefSharp\Cache", AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ""), extraCachePath));

                var settings = new CefSettings()
                {
                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = cachePath,
                    RemoteDebuggingPort = 9222,
                    IgnoreCertificateErrors = true,
                    Locale = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name

                };
                //CachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Format("Microneme\\{0}\\CefSharp\\Cache", AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ""))),

#if DEBUG
                //settings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";

                settings.CefCommandLineArgs.Add("-ignore-urlfetcher-cert-requests", "1"); // Solve the certificate problem
                settings.CefCommandLineArgs.Add("-ignore-certificate-errors", "1"); // Solve the certificate problem
#endif
                //settings.RegisterScheme(


                //Example of setting a command line argument
                //Enables WebRTC
                settings.CefCommandLineArgs.Add("enable-media-stream", "1");




                settings.RegisterScheme(new CefCustomScheme
                {
                    SchemeName = CustomProtocolSchemeHandlerFactory.SchemeName,
                    SchemeHandlerFactory = new CustomProtocolSchemeHandlerFactory()
                });

                //Perform dependency check to make sure all relevant resources are in our output directory.
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }
            catch (Exception error)
            {

            }
        }

        public void SetVisibility(Visibility visibility)
        {

            if (BrowserType == BrowserType.Chrome)
            {
                ChromeBrowser.Visibility = visibility;
            }
            if (BrowserType == BrowserType.IE)
            {
                IEWebBrowser.Visibility = visibility;
            }
            if (BrowserType == BrowserType.Edge)
            {
                //EdgeWebBrowser.Visibility = visibility;
            }
        }
        public readonly BrowserType BrowserType;
        ChromiumWebBrowser ChromeBrowser;

        const string JavaScriptFunction = "var result;  function invokeCSharpAction(data){result=null; window.external.notify(data); return result;}  function CSharpCallResult(parameter){result = parameter;}";
        bool EnableRestApi;
        public WebBrowserOverlay(FrameworkElement placementTarget, BrowserType browserType, bool enableRestApi = false):this()
        {

            EnableRestApi = enableRestApi;
            InitializeComponent();

            Loaded += WebBrowserOverlay_Loaded;
            Unloaded += WebBrowserOverlay_Unloaded;


            BrowserType = browserType;
            // WebBrowserHelper.FixBrowserVersion();

            _placementTarget = placementTarget;
            Window owner = Window.GetWindow(placementTarget);
            Debug.Assert(owner != null);

            LaunchBrowser(browserType);

        

            _placementTarget.LayoutUpdated += delegate { OnSizeLocationChanged(); };
            owner.LocationChanged += delegate { OnSizeLocationChanged(); };
            if (owner.IsVisible)
            {
                Owner = owner;
                InitializeCsharpJSBridge();
                Show();
            }
            else
                owner.IsVisibleChanged += delegate
                {
                    if (owner.IsVisible)
                    {
                        Owner = owner;
                        InitializeCsharpJSBridge();

                        Show();
                    }

                };

            _placementTarget.IsVisibleChanged += delegate
            {
                if (_placementTarget.IsVisible)
                {

                    Show();
                }
                else
                {
                    Hide();
                }
            };


            //owner.LayoutUpdated += new EventHandler(OnOwnerLayoutUpdated);
        }

     

        private void LaunchBrowser(BrowserType browserType)
        {
            if (browserType == BrowserType.Chrome)
            {



                ChromeBrowser = new ChromiumWebBrowser();
                ChromeBrowser.MenuHandler = new CustomMenuHandler();
                BrowserHostGrid.Children.Add(ChromeBrowser);

            }
            if (browserType == BrowserType.IE)
            {
                //WebBrowserHelper.ClearCache();
                IEWebBrowser = new WebBrowser();
                BrowserHostGrid.Children.Add(IEWebBrowser);
            }
            if (browserType == BrowserType.Edge)
            {
                //EdgeWebBrowser = new Microsoft.Toolkit.Win32.UI.Controls.WPF.WebView();
                //BrowserHostGrid.Children.Add(IEWebBrowser);
            }
            ChromeBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;

            if (ChromeBrowser != null)
            {
                CefSharpSettings.WcfEnabled = true;
                ChromeBrowser.JavascriptObjectRepository.Register("cscallbackObj", new CallbackObjectForJs(this), isAsync: false, options: BindingOptions.DefaultBinder);
            }
            // ChromeBrowser.RegisterAsyncJsObject("cscallbackObj", new CallbackObjectForJs(this));
        }

        private void WebBrowserOverlay_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ChromeBrowser != null && ProcessActiveBrowsers.ContainsKey(ChromeBrowser))
            {
                //ChromeBrowser.Dispose();
                
                ProcessActiveBrowsers.Remove(ChromeBrowser);
            }
        }
        static Dictionary<ChromiumWebBrowser, WebBrowserOverlay> ProcessActiveBrowsers = new Dictionary<ChromiumWebBrowser, WebBrowserOverlay>();

        private void WebBrowserOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            if (BrowserType == BrowserType.Chrome && ChromeBrowser != null && ChromeBrowser.IsDisposed)
                LaunchBrowser(BrowserType);
            if (ChromeBrowser != null)
                ProcessActiveBrowsers[ChromeBrowser] = this;
        }

        private void InitializeCsharpJSBridge()
        {
            if (IEWebBrowser != null)
            {
                IEWebBrowser.ObjectForScripting = new HtmlInteropInternal(this);


                //ChromeBrowser.Loaded += delegate (object sender, RoutedEventArgs e) { Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, false, false)); };
                //ChromeBrowser.LoadingStateChanged += delegate (object sender, LoadingStateChangedEventArgs e) { Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, e.CanGoBack, e.CanGoForward)); };


                IEWebBrowser.Navigating += delegate (object sender, NavigatingCancelEventArgs e) { Navigating?.Invoke(sender, e); };
                IEWebBrowser.Navigated += delegate (object sender, NavigationEventArgs e)
                {
                    IEWebBrowser.InvokeScript("eval", new[] { JavaScriptFunction }); Navigated?.Invoke(sender, new NavigatedEventArgs(IEWebBrowser, e.Uri.AbsoluteUri, false, false));

                };

                IEWebBrowser.LoadCompleted += delegate (object sender, NavigationEventArgs e) { LoadCompleted?.Invoke(sender, e); };
            }

            //if (EdgeWebBrowser != null)
            //{
            //    //EdgeWebBrowser.ObjectForScripting = new HtmlInteropInternal(this);


            //    //ChromeBrowser.Loaded += delegate (object sender, RoutedEventArgs e) { Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, false, false)); };
            //    //ChromeBrowser.LoadingStateChanged += delegate (object sender, LoadingStateChangedEventArgs e) { Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, e.CanGoBack, e.CanGoForward)); };


            //    EdgeWebBrowser.NavigationCompleted += delegate (object sender, WebViewControlNavigationCompletedEventArgs e) { Navigated?.Invoke(sender, new NavigatedEventArgs(EdgeWebBrowser, e.Uri.AbsoluteUri, false, false)); };

            //    //{
            //    //    EdgeWebBrowser.InvokeScript("eval", new[] { JavaScriptFunction }); Navigated?.Invoke(sender, new NavigatedEventArgs(IEWebBrowser, e.Uri.AbsoluteUri, false, false));

            //    //};

            //    //EdgeWebBrowser.Loaded+= delegate (object sender, NavigationEventArgs e) { LoadCompleted?.Invoke(sender, e); };
            //}

            if (ChromeBrowser != null)
            {

                ChromeBrowser.Loaded += delegate (object sender, RoutedEventArgs e)
                {
                    if (!Dispatcher.CheckAccess())
                    {
                        if (Navigated != null)
                            Task.Run(() => Dispatcher.Invoke(Navigated, sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, false, false)));
                    }
                    else
                        Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, false, false));
                };
                ChromeBrowser.LoadingStateChanged += delegate (object sender, LoadingStateChangedEventArgs e)
                {
                    if (!Dispatcher.CheckAccess())
                    {
                        if (Navigated != null)
                            Task.Run(() =>
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, e.CanGoBack, e.CanGoForward));
                                }));
                            });
                    }
                    else
                        Navigated?.Invoke(sender, new NavigatedEventArgs(ChromeBrowser, ChromeBrowser.Address, e.CanGoBack, e.CanGoForward));
                };

                //ChromeBrowser.Loaded += delegate (object sender, RoutedEventArgs e) { Navigated?.Invoke(sender, e); };
                //ChromeBrowser.LoadingStateChanged += delegate (object sender, LoadingStateChangedEventArgs e) { Navigated?.Invoke(sender, e); };
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {

        }



        public delegate void NotifyEventHandler(System.Object sender, string data);
        public delegate Task<string> notifyHandel(string data);

        public event NotifyEventHandler ScriptNotify;
        internal async Task<string> notify(string data)
        {
            // if (!OOAdvantech.Remoting.RestApi.JSCallData.IsAsyncCall(data))
            {
                if (!Dispatcher.CheckAccess())
                {
                    return Dispatcher.Invoke(new notifyHandel(notify), data) as string;
                }
            }
            if (EnableRestApi)
            {


                string retval = null;
                if (OOAdvantech.Remoting.RestApi.EmbeddedBrowser.JSCallData.IsAsyncCall(data))
                {
                    OOAdvantech.Remoting.RestApi.EmbeddedBrowser.JSCallData jsCallData = OOAdvantech.Remoting.RestApi.EmbeddedBrowser.JSCallData.GetJSCallData(data);
                    if (jsCallData.Args == "OnlyAsyncCall")
                    {
                        var task = System.Threading.Tasks.Task.Run(() =>
                        {
                            InvockeJSMethod("CSCallBack", new[] { jsCallData.CallID.ToString(), "True" }, true);
                        });
                    }
                }
                if (OOAdvantech.Remoting.RestApi.RequestData.IsRequestMessage(data))
                {
                    try
                    {
                        string headerStr = "" + data[0];
                        MessageHeader header = (MessageHeader)int.Parse(headerStr);
                        data = data.Substring(1);

                        if (header == MessageHeader.Request)
                        {
                            RequestData request = JsonConvert.DeserializeObject<RequestData>(data);
                            ResponseData responseData = null;
                            if (request.details == "OnlyAsyncCall")
                            {
                                responseData = new ResponseData();
                                responseData.ChannelUri = request.ChannelUri;
                                responseData.details = "True";
                                responseData.CallContextID = request.CallContextID;
                                responseData.BidirectionalChannel = true;
                            }
                            else
                            {
                                var objectContext = _placementTarget.GetDataContextObject();

                                request.SetCallContextData("DataContext", objectContext);

                                if (_placementTarget.GetObjectContextConnection() != null && _placementTarget.GetObjectContextConnection().Transaction != null)
                                    request.SetCallContextData("Transaction", _placementTarget.GetObjectContextConnection().Transaction);
                                else
                                    request.SetCallContextData("Transaction", null);

                                request.EventCallBackChannel = this;
                                responseData = await MessageDispatcher.MessageDispatchAsync(request); //MessageDispatcher.MessageDispatch(request);


                                responseData.CallContextID = request.CallContextID;
                                responseData.BidirectionalChannel = true;
                            }

                            //retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, _placementTarget.DataContext);
                            var task = System.Threading.Tasks.Task.Run(() =>
                            {
                                string responseDatajson = JsonConvert.SerializeObject(responseData);
                                responseDatajson = ((int)MessageHeader.Response).ToString() + responseDatajson;
                                InvockeJSMethod("SendMessage", new[] { responseDatajson }, true);

                                //InvockeJSMethod("CSCallBack", new[] { jsCallData.CallID.ToString(), retval }, true);
                            });


                            if (responseData.InitCommunicationSession)
                            {
                                DataContexRetrieving?.Invoke(this, _placementTarget.GetDataContextObject());
                            }
                        }
                    }
                    catch (Exception error)
                    {
                    }

                    return null;
                }



                //if (OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessage(data, _placementTarget.DataContext, out retval))
                //    return retval;
            }
            ScriptNotify?.BeginInvoke(this, data, null, null);
            return null;

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
                // Delayed call to avoid crash due to Window bug.
                Dispatcher.BeginInvoke((Action)delegate
                {
                    Owner.Close();
                });
        }

        public void OnSizeLocationChanged()
        {
            if (Owner == null || Visibility != Visibility.Visible)
                return;
            Point offset = _placementTarget.TranslatePoint(new Point(), Owner);
            Point size = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);
            HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(Owner);
            if (hwndSource != null)
            {
                CompositionTarget ct = hwndSource.CompositionTarget;
                offset = ct.TransformToDevice.Transform(offset);
                size = ct.TransformToDevice.Transform(size);

                Win32.POINT screenLocation = new Win32.POINT(offset);
                Win32.ClientToScreen(hwndSource.Handle, ref screenLocation);
                Win32.POINT screenSize = new Win32.POINT(size);
                //System.Diagnostics.Debug.WriteLine(string.Format("x:{0}  y:{1}", screenLocation.X, screenLocation.Y));
                if (HwndSource.FromVisual(this) is HwndSource)
                    Win32.MoveWindow(((HwndSource)HwndSource.FromVisual(this)).Handle, screenLocation.X, screenLocation.Y, screenSize.X, screenSize.Y, true);
            }
        }

        public void SendResponce(ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        public OOAdvantech.Remoting.RestApi.ResponseData SendRequest(RequestData requestData)
        {
            var task = SendRequestAsync(requestData);

            if (!task.Wait(TimeSpan.FromSeconds(25)))
            {
                task.Wait();
            }
            return task.Result;

        }

        public Task<ResponseData> SendRequestAsync(RequestData requestData)
        {
            string requestDatajson = JsonConvert.SerializeObject(requestData);
            requestDatajson = ((int)MessageHeader.Request).ToString() + requestDatajson;
            InvockeJSMethod("SendMessage", new[] { requestDatajson }, true);
            Task.FromResult(false);
            return Task.FromResult<ResponseData>(null);

        }



        public void RejectRequest(Task<ResponseData> task)
        {
        }



        internal string InternalLocalGet(string uri)
        {
            LocalGetData localGetData = new LocalGetData();
            if (!string.IsNullOrWhiteSpace(localGetData.Data))
                return localGetData.Data;
            else
                return null;
        }

        internal CustomProtocolResponse OnProcessRequest(Uri requestUri)
        {
            CustomProtocolResponse customProtocolResponse = new CustomProtocolResponse();
            ProcessRequest?.Invoke(requestUri, customProtocolResponse);
            return customProtocolResponse;

        }

        internal static WebBrowserOverlay GetWebBrowserOverlay(IBrowser browser)
        {

            return ProcessActiveBrowsers.Where(x => x.Key.GetBrowser().Identifier == browser.Identifier).Select(x => x.Value).FirstOrDefault();

        }
    }


    /// <MetaDataID>{8255e316-a87d-44a2-8d74-5a2225a2017a}</MetaDataID>
    public class CustomMenuHandler : CefSharp.IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {

            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
    /// <MetaDataID>{720f3a27-3334-4773-b17e-ce7dd1e3aa6d}</MetaDataID>
    static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public POINT(Point pt)
            {
                X = Convert.ToInt32(pt.X);
                Y = Convert.ToInt32(pt.Y);
            }
        };

        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    }


    /// <MetaDataID>{9bd27ba1-9c0f-4342-ade5-6ebc25b37c00}</MetaDataID>
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class HtmlInteropInternal
    {
        private WebBrowserOverlay webBrowserOverlay;

        public HtmlInteropInternal(WebBrowserOverlay webBrowserOverlay)
        {
            this.webBrowserOverlay = webBrowserOverlay;
        }

        public async void notify(string data)
        {

            var retvalue = await webBrowserOverlay.notify(data);
            webBrowserOverlay.InvokeScript("CSharpCallResult", retvalue);
        }
    }

    /// <MetaDataID>{d9985a39-c6b2-4594-ada1-ec997fbcfb5e}</MetaDataID>
    public static class WebBrowserExtensions
    {
        public static void SuppressScriptErrors(this WebBrowser browser, bool silent)
        {


            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }


        }
    }

    //public static void SetSilent(WebBrowser browser, bool silent)
    //{

    //}


    /// <MetaDataID>{441557c0-daaf-4653-85dd-923a167d03c4}</MetaDataID>
    [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IOleServiceProvider
    {
        [PreserveSig]
        int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
    }

    /// <MetaDataID>{65e23594-6ca5-4ffa-9070-c9f851702635}</MetaDataID>
    public class CallbackObjectForJs
    {
        WebBrowserOverlay WebBrowserHost;


        public CallbackObjectForJs(WebBrowserOverlay browserHost)
        {
            this.WebBrowserHost = browserHost;
        }

        public string notify(string data)
        {//Read Note

            var retVal = WebBrowserHost.notify(data);
            retVal.Wait();
            return retVal.Result;
        }


    }

    /// <MetaDataID>{29b20421-b002-4ac4-83e9-5b14aa83fbb3}</MetaDataID>
    public enum BrowserType
    {
        IE,
        Chrome,
        Edge
    }


    /// <MetaDataID>{aca70c19-7198-41f2-8dff-52364d949b19}</MetaDataID>
    public class WebBrowserHelper
    {


        public static int GetEmbVersion()
        {
            int ieVer = GetBrowserVersion();

            if (ieVer > 9)
                return ieVer * 1000 + 1;

            if (ieVer > 7)
                return ieVer * 1111;

            return 7000;
        } // End Function GetEmbVersion

        public static void FixBrowserVersion()
        {

            string appName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            appName = System.IO.Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
            FixBrowserVersion(appName);
        }

        public static void FixBrowserVersion(string appName)
        {
            FixBrowserVersion(appName, GetEmbVersion());
        } // End Sub FixBrowserVersion

        // FixBrowserVersion("<YourAppName>", 9000);
        public static void FixBrowserVersion(string appName, int ieVer)
        {
            FixBrowserVersion_Internal("HKEY_LOCAL_MACHINE", appName + ".exe", ieVer);
            FixBrowserVersion_Internal("HKEY_CURRENT_USER", appName + ".exe", ieVer);
            //FixBrowserVersion_Internal("HKEY_LOCAL_MACHINE", appName + ".vshost.exe", ieVer);
            //FixBrowserVersion_Internal("HKEY_CURRENT_USER", appName + ".vshost.exe", ieVer);
        } // End Sub FixBrowserVersion 

        private static void FixBrowserVersion_Internal(string root, string appName, int ieVer)
        {
            try
            {
                //For 64 bit Machine 
                if (Environment.Is64BitOperatingSystem)
                    Microsoft.Win32.Registry.SetValue(root + @"\Software\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, ieVer);
                else  //For 32 bit Machine 
                    Microsoft.Win32.Registry.SetValue(root + @"\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, ieVer);


            }
            catch (Exception)
            {
                // some config will hit access rights exceptions
                // this is why we try with both LOCAL_MACHINE and CURRENT_USER
            }
        } // End Sub FixBrowserVersion_Internal 

        public static int GetBrowserVersion()
        {
            // string strKeyPath = @"HKLM\SOFTWARE\Microsoft\Internet Explorer";
            string strKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string[] ls = new string[] { "svcVersion", "svcUpdateVersion", "Version", "W2kVersion" };

            int maxVer = 0;
            for (int i = 0; i < ls.Length; ++i)
            {
                object objVal = Microsoft.Win32.Registry.GetValue(strKeyPath, ls[i], "0");
                string strVal = System.Convert.ToString(objVal);
                if (strVal != null)
                {
                    int iPos = strVal.IndexOf('.');
                    if (iPos > 0)
                        strVal = strVal.Substring(0, iPos);

                    int res = 0;
                    if (int.TryParse(strVal, out res))
                        maxVer = Math.Max(maxVer, res);
                } // End if (strVal != null)

            } // Next i

            return maxVer;
        } // End Function GetBrowserVersion 




        #region Definitions/DLL Imports
        /// <summary>
        /// For PInvoke: Contains information about an entry in the Internet cache
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 80)]
        public struct INTERNET_CACHE_ENTRY_INFOA
        {
            [FieldOffset(0)]
            public uint dwStructSize;
            [FieldOffset(4)]
            public IntPtr lpszSourceUrlName;
            [FieldOffset(8)]
            public IntPtr lpszLocalFileName;
            [FieldOffset(12)]
            public uint CacheEntryType;
            [FieldOffset(16)]
            public uint dwUseCount;
            [FieldOffset(20)]
            public uint dwHitRate;
            [FieldOffset(24)]
            public uint dwSizeLow;
            [FieldOffset(28)]
            public uint dwSizeHigh;
            [FieldOffset(32)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
            [FieldOffset(40)]
            public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;
            [FieldOffset(48)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            [FieldOffset(56)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
            [FieldOffset(64)]
            public IntPtr lpHeaderInfo;
            [FieldOffset(68)]
            public uint dwHeaderInfoSize;
            [FieldOffset(72)]
            public IntPtr lpszFileExtension;
            [FieldOffset(76)]
            public uint dwReserved;
            [FieldOffset(76)]
            public uint dwExemptDelta;
        }

        // For PInvoke: Initiates the enumeration of the cache groups in the Internet cache
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindFirstUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheGroup(
            int dwFlags,
            int dwFilter,
            IntPtr lpSearchCondition,
            int dwSearchCondition,
            ref long lpGroupId,
            IntPtr lpReserved);

        // For PInvoke: Retrieves the next cache group in a cache group enumeration
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheGroup(
            IntPtr hFind,
            ref long lpGroupId,
            IntPtr lpReserved);

        // For PInvoke: Releases the specified GROUPID and any associated state in the cache index file
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheGroup",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheGroup(
            long GroupId,
            int dwFlags,
            IntPtr lpReserved);

        // For PInvoke: Begins the enumeration of the Internet cache
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
           EntryPoint = "FindFirstUrlCacheEntryA",
           CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheEntry(
           [MarshalAs(UnmanagedType.LPTStr)] string lpszUrlSearchPattern,
           IntPtr lpFirstCacheEntryInfo,
           ref int lpdwFirstCacheEntryInfoBufferSize);

        // For PInvoke: Retrieves the next entry in the Internet cache
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "FindNextUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheEntry(
            IntPtr hFind,
            IntPtr lpNextCacheEntryInfo,
            ref int lpdwNextCacheEntryInfoBufferSize);

        // For PInvoke: Removes the file that is associated with the source name from the cache, if the file exists
        [DllImport(@"wininet",
            SetLastError = true,
            CharSet = CharSet.Auto,
            EntryPoint = "DeleteUrlCacheEntryA",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheEntry(
            IntPtr lpszUrlName);
        #endregion

        #region Public Static Functions

        /// <summary>
        /// Clears the cache of the web browser
        /// </summary>
        static void ClearCache()
        {
            // Indicates that all of the cache groups in the user's system should be enumerated
            const int CACHEGROUP_SEARCH_ALL = 0x0;
            // Indicates that all the cache entries that are associated with the cache group
            // should be deleted, unless the entry belongs to another cache group.
            const int CACHEGROUP_FLAG_FLUSHURL_ONDELETE = 0x2;
            // File not found.
            const int ERROR_FILE_NOT_FOUND = 0x2;
            // No more items have been found.
            const int ERROR_NO_MORE_ITEMS = 259;
            // Pointer to a GROUPID variable
            long groupId = 0;

            // Local variables
            int cacheEntryInfoBufferSizeInitial = 0;
            int cacheEntryInfoBufferSize = 0;
            IntPtr cacheEntryInfoBuffer = IntPtr.Zero;
            INTERNET_CACHE_ENTRY_INFOA internetCacheEntry;
            IntPtr enumHandle = IntPtr.Zero;
            bool returnValue = false;

            // Delete the groups first.
            // Groups may not always exist on the system.
            // For more information, visit the following Microsoft Web site:
            // http://msdn.microsoft.com/library/?url=/workshop/networking/wininet/overview/cache.asp            
            // By default, a URL does not belong to any group. Therefore, that cache may become
            // empty even when the CacheGroup APIs are not used because the existing URL does not belong to any group.            
            enumHandle = FindFirstUrlCacheGroup(0, CACHEGROUP_SEARCH_ALL, IntPtr.Zero, 0, ref groupId, IntPtr.Zero);
            // If there are no items in the Cache, you are finished.
            if (enumHandle != IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                return;

            // Loop through Cache Group, and then delete entries.
            while (true)
            {
                if (ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() || ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error()) { break; }
                // Delete a particular Cache Group.
                returnValue = DeleteUrlCacheGroup(groupId, CACHEGROUP_FLAG_FLUSHURL_ONDELETE, IntPtr.Zero);
                if (!returnValue && ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error())
                {
                    returnValue = FindNextUrlCacheGroup(enumHandle, ref groupId, IntPtr.Zero);
                }

                if (!returnValue && (ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error() || ERROR_FILE_NOT_FOUND == Marshal.GetLastWin32Error()))
                    break;
            }

            // Start to delete URLs that do not belong to any group.
            enumHandle = FindFirstUrlCacheEntry(null, IntPtr.Zero, ref cacheEntryInfoBufferSizeInitial);
            if (enumHandle != IntPtr.Zero && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                return;

            cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
            cacheEntryInfoBuffer = Marshal.AllocHGlobal(cacheEntryInfoBufferSize);
            enumHandle = FindFirstUrlCacheEntry(null, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);

            while (true)
            {
                internetCacheEntry = (INTERNET_CACHE_ENTRY_INFOA)Marshal.PtrToStructure(cacheEntryInfoBuffer, typeof(INTERNET_CACHE_ENTRY_INFOA));
                if (ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error()) { break; }

                cacheEntryInfoBufferSizeInitial = cacheEntryInfoBufferSize;
                returnValue = DeleteUrlCacheEntry(internetCacheEntry.lpszSourceUrlName);
                if (!returnValue)
                {
                    returnValue = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);
                }
                if (!returnValue && ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                {
                    break;
                }
                if (!returnValue && cacheEntryInfoBufferSizeInitial > cacheEntryInfoBufferSize)
                {
                    cacheEntryInfoBufferSize = cacheEntryInfoBufferSizeInitial;
                    cacheEntryInfoBuffer = Marshal.ReAllocHGlobal(cacheEntryInfoBuffer, (IntPtr)cacheEntryInfoBufferSize);
                    returnValue = FindNextUrlCacheEntry(enumHandle, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSizeInitial);
                }
            }
            Marshal.FreeHGlobal(cacheEntryInfoBuffer);
        }
        #endregion
    }





}

