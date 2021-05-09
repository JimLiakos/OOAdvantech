using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OOAdvantech.WindowsUniversal;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using OOAdvantech.Web;
using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Remoting.RestApi.EmbeddedBrowser;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace OOAdvantech.WindowsUniversal
{
    /// <MetaDataID>{286b4c4a-0bcf-48d5-9868-49faa3934eb5}</MetaDataID>
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Windows.UI.Xaml.Controls.WebView>, INativeWebBrowser, IEndPoint
    {

        public HybridWebViewRenderer()
        {
            Windows.UI.Xaml.Controls.WebView.ClearTemporaryWebDataAsync();
        }
        public static void Init()
        {
            // Cause the assembly to load
        }

        const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";
        //const string JavaScriptFunction = "var result;  function invokeCSharpAction(data){ window.external.notify(data);}  function CSharpCallResult(parameter){ser = parameter;return ser;}";

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                SetNativeControl(new Windows.UI.Xaml.Controls.WebView());
            }
            if (e.OldElement != null)
            {
                Control.NavigationCompleted -= OnWebViewNavigationCompleted;
                Control.ScriptNotify -= OnWebViewScriptNotify;
            }
            if (e.NewElement != null)
            {
                Control.NavigationCompleted += OnWebViewNavigationCompleted;
                Control.ScriptNotify += OnWebViewScriptNotify;
                //Control.AllowedScriptNotifyUris
                string url = null;
                if (Element.Uri.IndexOf(@"local://") == 0)
                    url = Element.Uri.Replace(@"local://", @"ms-appx-web:///");
                else
                    url = Element.Uri;
                Control.Source = new Uri(url);
                //Control.Refresh();
                //Control.Source = new Uri(string.Format(@"http://192.168.2.10/WebPart/{0}", Element.Uri));
                //Control.Source = new Uri(string.Format(@"http://10.0.0.10/WebPart/{0}", Element.Uri));
                //Control.Source = new Uri(string.Format("ms-appx-web:///Content//{0}", Element.Uri));
                e.NewElement.NativeWebBrowser = this;
            }
        }

        async void OnWebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                // Control.Refresh();
                //Control.InvokeScriptAsync("logA", new[] { "HelloppA" }).AsTask();
                // Inject JS script
                await Control.InvokeScriptAsync("eval", new[] { JavaScriptFunction });
            }
        }


        async void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {

            if (Remoting.RestApi.RequestData.IsRequestMessage(e.Value))
            {
                try
                {
                    string data = e.Value;
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
                            request.SetData("DataContext", Element.BindingContext);
                            request.EventCallBackChannel = this;
                            responseData = await MessageDispatcher.MessageDispatchAsync(request); //MessageDispatcher.MessageDispatch(request);
                            responseData.CallContextID = request.CallContextID;
                            responseData.BidirectionalChannel = true;
                        }

                        string responseDatajson = JsonConvert.SerializeObject(responseData);
                        responseDatajson = ((int)MessageHeader.Response).ToString() + responseDatajson;
                        await Control.InvokeScriptAsync("SendMessage", new[] { responseDatajson });
                        //retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, _placementTarget.DataContext);

                    }
                }
                catch (Exception error)
                {
                }

                return;
            }

            if (JSCallData.IsAsyncCall(e.Value))
            {

                var obj = Element.BindingContext;

                JSCallData jsCallData = JsonConvert.DeserializeObject<JSCallData>(e.Value);

                try
                {
                    var retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, Element.BindingContext);

                    await Control.InvokeScriptAsync("CSCallBack", new[] { jsCallData.CallID.ToString(), retval });


                }
                catch (Exception error)
                {
                }
            }

        }

        public Task<string> InvockeJSMethod(string methodName, object[] args)
        {
            string[] strArgs = (from arg in args
                                select arg.ToString()).ToArray();
            return Control.InvokeScriptAsync(methodName, strArgs).AsTask();
        }

        public void GoBack()
        {
            if (Control != null)
                Control.GoBack();
        }

        public void SendResponce(ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData> SendRequestAsync(RequestData requestData)
        {
            string requestDatajson = JsonConvert.SerializeObject(requestData);
            requestDatajson = ((int)MessageHeader.Request).ToString() + requestDatajson;
            Control.InvokeScriptAsync("SendMessage", new[] { requestDatajson });



            return null;
        }

        public ResponseData SendRequest(RequestData requestData)
        {
            throw new NotImplementedException();
        }
        public void RejectRequest(System.Threading.Tasks.Task<ResponseData> task)
        {
            throw new NotImplementedException();
        }

    }
}
