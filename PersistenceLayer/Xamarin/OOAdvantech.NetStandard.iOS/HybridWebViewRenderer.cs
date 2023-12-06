using System.IO;
using OOAdvantech.Web;
using OOAdvantech.iOS;
using Foundation;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Remoting.RestApi.EmbeddedBrowser;
using Firebase.Core;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace OOAdvantech.iOS
{
    /// <MetaDataID>{2b10c922-d4bc-40e3-92e0-d8ac9e5785d4}</MetaDataID>
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler, INativeWebBrowser, IEndPoint
    {
        public HybridWebViewRenderer()
        {

        }
        public static void Init()
        {
            // Cause the assembly to load
        }

        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;
        HybridWebView HybridWebView;

        public string Url
        {
            get
            {
                if (Control != null)
                    return Control.Url.ToString();
                else
                    return null;
            }
        }

        public bool ConnectionIsOpen => true;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null&&e.NewElement!=null)
            {
                try
                {
                    userController = new WKUserContentController();
                  
                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    ///config.ApplicationNameForUserAgent = "Version/8.0.2 Safari/600.2.5";



                    try
                    {
                        if (Element.Uri.IndexOf(@"local://") == 0||Element.Uri.IndexOf(@"webapp://")==0)
                        {
                            config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("allowFileAccessFromFileURLs"));
                            
                            //config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("allowUniversalAccessFromFileURLs"));
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                    userController.AddUserScript(script);
                    userController.AddScriptMessageHandler(this, "invokeAction");



                    var webView = new WKWebView(Frame, config)
                    {
                        NavigationDelegate = new NavigationDelegate(this)
                    };
                    SetNativeControl(webView);
                    webView.ScrollView.PanGestureRecognizer.Enabled=false;
                    if (webView.ScrollView.PinchGestureRecognizer!=null)
                        webView.ScrollView.PinchGestureRecognizer.Enabled=false;
                    webView.ScrollView.MaximumZoomScale=1;
                    // webView.ScrollView.PinchGestureRecognizer.Enabled=false;

                    NSUrl url = null;
                    if (e.NewElement != null)
                    {
                        string uri = null;
                        if (Element.Uri.IndexOf(@"local://") == 0)
                        {
                            uri = Element.Uri.ToString().Replace(@"local://", @"Content/");
                            string filename = Path.Combine(NSBundle.MainBundle.BundlePath, uri);
                            uri = filename;
                            url = new NSUrl(uri, false);
                        }
                        else if (Element.Uri.IndexOf(@"webapp://") == 0)
                        {
                            var webAppPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                            string filePath = System.IO.Path.Combine(System.IO.Path.Combine(webAppPath, "webapp"), "index.html");
                            bool exist = System.IO.File.Exists(filePath);
                            if (exist)
                            {
                                System.Diagnostics.Debug.WriteLine(filePath);
                            }

                            uri = filePath;
                            System.Diagnostics.Debug.WriteLine(uri);
                            url = new NSUrl(uri, false);
                        }

                        else
                        {
                            uri = Element.Uri;
                            url = new NSUrl(uri);
                        }
                    }
                    var request = new NSUrlRequest(url);
                    webView.LoadRequest(request);

                    //if (Application.Current?.MainPage != null)
                    // Application.Current.MainPage.DisplayAlert("LoadRequest)", url.ToString(), "OK");

                    if (webView == Control)
                    {

                    }

                }
                catch (Exception error)
                {

                    throw;
                }
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            HybridWebView = e.NewElement;
            if (e.NewElement != null && Element.Uri != null)
            {
                e.NewElement.NativeWebBrowser = this;
                string uri;

                NSUrl url;
                if (Element.Uri.IndexOf(@"local://") == 0)
                {
                    uri = Element.Uri.ToString().Replace(@"local://", @"Content/");
                    string filename = Path.Combine(NSBundle.MainBundle.BundlePath, uri);
                    uri = filename;
                    url = new NSUrl(uri, false);
                }
                else
                {
                    uri = Element.Uri;
                    url = new NSUrl(uri);
                }

                var request = new NSUrlRequest(url);
                Control.LoadRequest(request);

                //if (Application.Current?.MainPage != null)
                //     Application.Current.MainPage.DisplayAlert("LoadRequest)", url.ToString(),"OK");


                //string url = null;
                //if (Element.Uri.IndexOf(@"local://") == 0)
                //    url = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));//url = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                //else
                //    url = Element.Uri;
                //Control.LoadRequest(new NSUrlRequest(new NSUrl(url, false)));
            }
        }

        internal void OnNavigated(NavigatedEventArgs navigatedEventArgs)
        {

            string javascriptStyle = "var css = '*{-webkit-touch-callout:none;-webkit-user-select:none}'; var head = document.head || document.getElementsByTagName('head')[0]; var style = document.createElement('style'); style.type = 'text/css'; style.appendChild(document.createTextNode(css)); head.appendChild(style);";
            //Control.EvaluateJavaScriptAsync(javascriptStyle);
            try
            {
                if (HybridWebView != null)
                    HybridWebView.OnNavigated(navigatedEventArgs);
            }
            catch (Exception error)
            {
            }
        }


        public async void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            string arg = "";

            if (message.Body != null)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    string data = message.Body.ToString();
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
                                request.SetCallContextData("DataContext", Element.BindingContext);
                                request.EventCallBackChannel = this;
                                responseData = await MessageDispatcher.MessageDispatchAsync(request); //MessageDispatcher.MessageDispatch(request);
                                responseData.CallContextID = request.CallContextID;
                                responseData.BidirectionalChannel = true;
                            }

                            string responseDatajson = JsonConvert.SerializeObject(responseData);
                            responseDatajson = ((int)MessageHeader.Response).ToString() + responseDatajson;
                            await InvockeJSMethod("SendMessage", new[] { responseDatajson });

                        }
                    }
                    catch (Exception error)
                    {
                    }

                    return;
                });

                //if (JSCallData.IsAsyncCall(data))
                //{
                //    JSCallData jsCallData = JsonConvert.DeserializeObject<JSCallData>(data);
                //    var obj = Element.BindingContext;
                //    try
                //    {
                //        if (jsCallData.Args == "OnlyAsyncCall")
                //        {
                //            await InvockeJSMethod("CSCallBack", new[] { jsCallData.CallID.ToString(), "True" });
                //        }
                //        else
                //        {
                //            var retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, Element.BindingContext);
                //            await InvockeJSMethod("CSCallBack", new object[] { jsCallData.CallID, retval });
                //        }
                //    }
                //    catch (Exception error)
                //    {
                //    }
                //}
            }
        }






        public async Task<string> InvockeJSMethod(string methodName, object[] args)
        {
            try
            {
                //string jsScriptArgs = null;
                //if (args.Length > 0)
                //{
                //    foreach (var arg in args)
                //    {
                //        if (jsScriptArgs != null)
                //            jsScriptArgs += ",";

                //        if (arg is string)
                //        {
                //            if (arg != null)
                //                jsScriptArgs += "'" + (arg as string).Replace("\\", "\\\\") + "'";
                //            else
                //                jsScriptArgs += "'" + arg + "'";
                //        }
                //        else
                //            jsScriptArgs += arg.ToString();
                //    }
                //}
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
                        else
                            jsScriptArgs += arg.ToString();
                    }
                }

                string jsScriptMethodCall = methodName + "(" + jsScriptArgs + ");";

                var tcs = new TaskCompletionSource<string>();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {

                            var result = await Control.EvaluateJavaScriptAsync(jsScriptMethodCall) as NSString;
                            tcs.SetResult(result);
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }
                    });
                });

                string retval = await tcs.Task;

                return retval;
            }
            catch (Exception error)
            {
                return "";
            }


            //return callback.Task;
        }
        public void RefreshPage()
        {
            Control?.ReloadFromOrigin();

        }

        public bool CanGoBack
        {
            get
            {
                return Control?.CanGoBack==true;
            }
        }

        public void GoBack()
        {
            if (Control != null)
                Control.GoBack();
        }

        public event NavigatedHandler Navigated;



        public Task<ResponseData> SendRequestAsync(RequestData requestData)
        {
            string requestDatajson = JsonConvert.SerializeObject(requestData);
            requestDatajson = ((int)MessageHeader.Request).ToString() + requestDatajson;
            InvockeJSMethod("SendMessage", new[] { requestDatajson });
            return Task.FromResult<ResponseData>(null);

        }

        public void RejectRequest(Task<ResponseData> task)
        {
            throw new NotImplementedException();
        }

        public void SendResponce(ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        public ResponseData SendRequest(RequestData requestData)
        {

            throw new NotImplementedException();


        }

        public void Navigate(string url)
        {



            var ns_url = new NSUrl(url);
            if (Control != null && Control.Url != ns_url)
            {
                var request = new NSUrlRequest(ns_url);
                Control.LoadRequest(request);
                //if (Application.Current?.MainPage != null)
                //     Application.Current.MainPage.DisplayAlert("LoadRequest)", url.ToString(), "OK");
            }

        }
    }



    public class NavigationDelegate : WKNavigationDelegate
    {
        private readonly HybridWebViewRenderer _renderer;

        public NavigationDelegate(HybridWebViewRenderer renderer)
        {
            _renderer = renderer;
        }

        public override void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            // call methods of your renderer or its properties like
            //_renderer.Element.OnNavigating(webView.Url);
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            var task = _renderer.InvockeJSMethod("SendMessage", new[] { "CSCodeCommunicationStart" });

            _renderer.OnNavigated(new NavigatedEventArgs(webView, webView.Url.AbsoluteUrl.AbsoluteString, webView.CanGoBack, webView.CanGoForward));
            //base.DidFinishNavigation(webView, navigation);
        }


        public override void DidCommitNavigation(WKWebView webView, WKNavigation navigation)
        {
            //base.DidCommitNavigation(webView, navigation);
        }
        public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            base.DidFailNavigation(webView, navigation, error);
        }
    }
}