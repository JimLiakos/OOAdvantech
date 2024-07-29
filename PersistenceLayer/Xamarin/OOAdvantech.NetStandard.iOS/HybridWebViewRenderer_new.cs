using System;
using System.IO;
using System.Threading.Tasks;
using OOAdvantech;
using OOAdvantech.iOS;
using Foundation;
using OOAdvantech.iOS;
using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Web;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace OOAdvantech.iOS

{
    public class HybridWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler, INativeWebBrowser, IEndPoint
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;

        string INativeWebBrowser.Url => throw new NotImplementedException();

        bool INativeWebBrowser.CanGoBack => throw new NotImplementedException();

        public bool ConnectionIsOpen => true;

        public HybridWebViewRenderer() : this(new WKWebViewConfiguration())
        {

        }
        public static void Init()
        {
            // Cause the assembly to load
        }



        public HybridWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
            userController.AddUserScript(script);
            userController.AddScriptMessageHandler(this, "invokeAction");
            config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("allowFileAccessFromFileURLs"));
        }
        WKWebView NativeWebView;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);


            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                HybridWebView hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
                
            }

            if (e.NewElement != null)
            {

                if (NativeWebView == null)
                {
                    NativeWebView = this;



                    if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0))
                    {
                        NativeWebView.Inspectable = true;
                    }

                    NativeWebView.ScrollView.PanGestureRecognizer.Enabled = false;
                    if (NativeWebView.ScrollView.PinchGestureRecognizer != null)
                        NativeWebView.ScrollView.PinchGestureRecognizer.Enabled = false;
                    NativeWebView.ScrollView.MaximumZoomScale = 1;
                    NavigationDelegate = new NavigationDelegate(this);
                }

                    ((HybridWebView)e.NewElement).NativeWebBrowser = this;
                if (((HybridWebView)Element).Uri.IndexOf(@"local://") == 0)
                {
                    string uri = ((HybridWebView)Element).Uri.ToString().Replace(@"local://", "");
                    string filename = Path.Combine(NSBundle.MainBundle.BundlePath, Path.Combine("Content", uri));
                    string webAppFolder = Path.Combine(NSBundle.MainBundle.BundlePath, "Content");
                    if (System.IO.File.Exists(filename))
                    {
                        System.Diagnostics.Debug.WriteLine("sd");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(filename);
                    }
                    LoadFileUrl(new NSUrl("file://" + filename), new NSUrl("file://" + webAppFolder));
                    LoadFileRequest(new NSUrlRequest(new NSUrl(filename, false)), new NSUrl("file://" + webAppFolder));



                }
                else if (((HybridWebView)Element).Uri.IndexOf(@"webapp://") == 0)
                {
                    string uri = ((HybridWebView)Element).Uri.ToString().Replace(@"webapp://", "");
                    string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine("webapp", uri));
                    string webAppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "webapp");

                    if (System.IO.File.Exists(filename))
                    {
                        System.Diagnostics.Debug.WriteLine("sd");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(filename);
                    }


                    LoadFileUrl(new NSUrl("file://" + filename), new NSUrl("file://" + webAppFolder));
                    LoadFileRequest(new NSUrlRequest(new NSUrl(filename, false)), new NSUrl("file://" + webAppFolder));



                }

                else
                {
                    string uri = ((HybridWebView)Element).Uri;
                    var url = new NSUrl(uri);
                    var request = new NSUrlRequest(url);
                    LoadRequest(request);
                }



                //string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine("webapp", ((HybridWebView)Element).Uri));
                //string webAppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "webapp");

                //if (System.IO.File.Exists(filename))
                //{
                //    System.Diagnostics.Debug.WriteLine("sd");
                //}
                //else
                //{
                //    System.Diagnostics.Debug.WriteLine(filename);
                //}


                //LoadFileUrl(new NSUrl("file://" + filename), new NSUrl("file://" + webAppFolder));
                //LoadFileRequest(new NSUrlRequest(new NSUrl(filename, false)), new NSUrl("file://" + webAppFolder));



                ///private/var/containers/Bundle/Application/92D3EF86-A957-4026-B871-B9D6302B968C/CustomRendereriOS.app/Content/index.html
                ///private/var/containers/Bundle/Application/8660FB3C-ED3C-466A-9349-32DBD7A8DAAB/DontWaitAppNS.iOS.app/Content/m-index.html
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
                            await InvokeJSMethod("SendMessage", new[] { responseDatajson });

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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((HybridWebView)Element).Cleanup();
            }
            base.Dispose(disposing);
        }

        public async Task<string> InvokeJSMethod(string methodName, object[] args)
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

                            var result = await EvaluateJavaScriptAsync(jsScriptMethodCall) as NSString;
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

        }

        void INativeWebBrowser.GoBack()
        {
            GoBack();
        }

        void INativeWebBrowser.Navigate(string url)
        {

            var ns_url = new NSUrl(url);
            if (Url != ns_url)
            {
                var request = new NSUrlRequest(ns_url);
                LoadRequest(request);
                //if (Application.Current?.MainPage != null)
                //     Application.Current.MainPage.DisplayAlert("LoadRequest)", url.ToString(), "OK");
            }
        }

        void INativeWebBrowser.RefreshPage()
        {
            throw new NotImplementedException();
        }

        public void RejectRequest(Task<ResponseData> task)
        {
            throw new NotImplementedException();
        }

        public void SendResponce(ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData> SendRequestAsync(RequestData requestData)
        {
            string requestDatajson = JsonConvert.SerializeObject(requestData);
            requestDatajson = ((int)MessageHeader.Request).ToString() + requestDatajson;
            string result = await (this as INativeWebBrowser).InvokeJSMethod("SendMessage", new[] { requestDatajson });
            return null;
        }

        public ResponseData SendRequest(RequestData requestData)
        {
            throw new NotImplementedException();
        }

        internal void OnNavigated(NavigatedEventArgs navigatedEventArgs)
        {

            
            //Control.EvaluateJavaScriptAsync(javascriptStyle);
            try
            {
                (this.Element as HybridWebView).OnNavigated(navigatedEventArgs);

            }
            catch (Exception error)
            {
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
            var task = _renderer.InvokeJSMethod("SendMessage", new[] { "CSCodeCommunicationStart" });

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
