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

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace OOAdvantech.iOS
{
    /// <MetaDataID>{2b10c922-d4bc-40e3-92e0-d8ac9e5785d4}</MetaDataID>
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler, INativeWebBrowser
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

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                try
                {
                    userController = new WKUserContentController();
                    var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                    userController.AddUserScript(script);
                    userController.AddScriptMessageHandler(this, "invokeAction");

                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    var webView = new WKWebView(Frame, config);
                    //var webView = new WKWebView(Frame, new WKWebViewConfiguration());
                    SetNativeControl(webView);
                    
                     
                    //var url = new NSUrl("https://evolve.xamarin.com");
                    //if (e.NewElement != null)
                    //{
                    //    string uri = null;
                    //    if (Element.Uri.IndexOf(@"local://") == 0)
                    //        uri = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));//url = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                    //    else
                    //        uri = Element.Uri;
                    //    url = new NSUrl(uri);
                    //}
                    //var request = new NSUrlRequest(url);
                    //webView.LoadRequest(request);
                    //if(webView==Control)
                    //{

                    //}

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
            if (e.NewElement != null)
            {
                e.NewElement.NativeWebBrowser = this;
                string uri = null;
                if (Element.Uri.IndexOf(@"local://") == 0)
                    uri = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));//url = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                else
                    uri = Element.Uri;
                var url = new NSUrl(uri);
                var request = new NSUrlRequest(url);
                Control.LoadRequest(request);


                //string url = null;
                //if (Element.Uri.IndexOf(@"local://") == 0)
                //    url = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));//url = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                //else
                //    url = Element.Uri;
                //Control.LoadRequest(new NSUrlRequest(new NSUrl(url, false)));
            }
        }

       

        public async void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            string arg = "";
            if (message.Body != null)
            {
                string data = message.Body.ToString();

                if (JSCallData.IsAsyncCall(data))
                {
                    JSCallData jsCallData = JsonConvert.DeserializeObject<JSCallData>(data);


                    var obj = Element.BindingContext;


                    //await Control.InvokeScriptAsync("CSCallBack", new[] { jsCallData.CallID.ToString(), "Hello C# $ " + jsCallData.Args });


                    try
                    {
                        if (jsCallData.Args == "OnlyAsyncCall")
                        {
                            await InvockeJSMethod("CSCallBack", new[] { jsCallData.CallID.ToString(), "True" });
                        }
                        else
                        {
                            var retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, Element.BindingContext);
                            //var task = System.Threading.Tasks.Task.Run(async() =>
                            //{
                            await InvockeJSMethod("CSCallBack", new object[] { jsCallData.CallID, retval });

                            //  await Control.InvokeScriptAsync("CSCallBack", new[] { jsCallData.CallID.ToString(), retval });
                            //});
                        }

                    }
                    catch (Exception error)
                    {
                    }
                }


                ////{
                ////    JSCallData jsCallData = JsonConvert.DeserializeObject<JSCallData>(data);
                ////    await hybridRenderer.InvockeJSMethod("CSCallBack", new object[] { jsCallData.CallID.ToString(), "Hello C# $ " + jsCallData.Args });

                ////    //hybridRenderer.InvockeJSMethod("CSharpCallResult", new object[] { "Slama" });
                ////    //hybridRenderer.Element.InvokeAction(data);
                ////}

                //JSCallData jsCallData = JsonConvert.DeserializeObject<JSCallData>(arg);
                //InvockeJSMethod("CSCallBack", new object[] { jsCallData.CallID.ToString(), "Hello C# $ "+ jsCallData.Args });
            }



           // Element.InvokeAction(arg);

            //   WKJavascriptEvaluationResult handler = (NSObject result, NSError err) =>
            //   {
            //       if (err != null)
            //       {
            //           System.Console.WriteLine(err);
            //       }
            //       if (result != null)
            //       {
            //           System.Console.WriteLine(result);
            //       }
            //   };

            ////   Control.EvaluateJavaScript("logA(\"Hello mama\");", handler);
        }

        public async Task<string> InvockeJSMethod(string methodName, object[] args)
        {


            try
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
                        else
                            jsScriptArgs += arg.ToString();
                    }
                }
                string jsScriptMethodCall = methodName + "(" + jsScriptArgs + ");";
                var result = await Control.EvaluateJavaScriptAsync(jsScriptMethodCall) as NSString;
                return result;
            }
            catch (Exception error)
            {

                return "";
            }


            //return callback.Task;
        }
    }
}
