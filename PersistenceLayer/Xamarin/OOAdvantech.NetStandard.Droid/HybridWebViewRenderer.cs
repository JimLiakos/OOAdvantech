using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Webkit;

using OOAdvantech.Droid;
using Java.Interop;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Text;
using OOAdvantech.Web;
using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Remoting.RestApi.EmbeddedBrowser;
using Android.Runtime;
using Android.Net.Http;
using Android.App;
using Android.Content;
using Android.Views;
using Android.OS;
using WebView = Android.Webkit.WebView;
using System.Reflection;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace OOAdvantech.Droid
{
    /// <MetaDataID>{a1637bb6-e65c-43af-95ed-6703b0f1a69d}</MetaDataID>
    public class HybridWebViewRenderer : WebViewRenderer, INativeWebBrowser, IEndPoint
    {
        Android.Content.Context _context;
        public HybridWebViewRenderer(Android.Content.Context context) : base(context)
        {

            _context = context;
            OnBackPressed += HybridWebViewRenderer_OnBackPressed;

        }


        //A3piKEtGw7w6W+FL7pMNzL1rpqE=

        private void HybridWebViewRenderer_OnBackPressed()
        {


            try
            {

                IVisualElementRenderer parent = Parent as IVisualElementRenderer;


                bool HostPageIsVisible = false;
                var page = Xamarin.Forms.Application.Current.MainPage;
                if (page is NavigationPage)
                    page = (page as NavigationPage).CurrentPage;

                while (parent != null)
                {
                    if (parent.Element == page)
                    {
                        HostPageIsVisible = true;
                        break;
                    }
                    parent = (parent as IViewParent)?.Parent as IVisualElementRenderer;
                }




                if (HostPageIsVisible)
                    InvockeJSMethod("BackButtonPress", new object[] { });
            }
            catch (ObjectDisposedException e)
            {
                OnBackPressed -= HybridWebViewRenderer_OnBackPressed;
                // now I know object has been disposed
            }
        }
        internal void OnNavigated(NavigatedEventArgs navigatedEventArgs)
        {
            try
            {

                if (HybridWebView != null)
                    HybridWebView.OnNavigated(navigatedEventArgs);
            }
            catch (Exception error)
            {
            }
        }

        public bool OnShouldOverrideUrlLoading(string url)
        {
            if (HybridWebView != null)
                return HybridWebView.OnShouldOverrideUrlLoading(url);
            else
                return false;
        }

        delegate void BackPressedandle();

        static event BackPressedandle OnBackPressed;
        public static void BackPressed()
        {
            OnBackPressed?.Invoke();
        }
        //const string JavaScriptFunction = "var result='mm'; function invokeCSharpAction(data){ jsBridge.invokeAction(data); retutn 'result';}   function CSharpCallResult(parameter){result = parameter;}";
        const string JavaScriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        public Task<string> InvockeJSMethod(string methodName, object[] args)
        {
            //webSettings.setDomStorageEnabled(true); 

            var callback = new StringCallback();
            if (ThreadHelper.IsOnMainThread)
            {
                InternalInvokeJS(methodName, args, callback);
                callback?.Task.Wait();
                if (callback?.Task?.Exception?.GetBaseException() != null)
                    throw callback.Task.Exception.GetBaseException();
            }
            else
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    InternalInvokeJS(methodName, args, callback);
                });
            }

            return Task<string>.FromResult("Hello");
        }

        private void InternalInvokeJS(string methodName, object[] args, StringCallback callback)
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
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
                    Control.EvaluateJavascript(jsScriptMethodCall, callback);
                    callback.SetTaskResult("OK");
                }
                else
                {
                    string jsScriptArgs = null;
                    if (args.Length > 0)
                    {
                        foreach (var arg in args)
                        {
                            if (jsScriptArgs != null)
                                jsScriptArgs += ",";
                            if (arg is string)
                                jsScriptArgs += "\"" + arg + "\"";
                            else
                                jsScriptArgs += arg.ToString();
                        }
                    }
                    string jsScriptMethodCall = methodName + "(" + jsScriptArgs + ");";
                    Control.LoadUrl("javascript: " + jsScriptMethodCall);
                    callback.SetTaskResult("OK");
                }
            }
            catch (ObjectDisposedException error)
            {
                callback.SetTaskException(error);
            }
            catch (System.Exception error)
            {

                callback.SetTaskException(error);
            }
        }
        public bool CanGoBack
        {
            get
            {
                return Control?.CanGoBack() == true;
            }
        }
        public void GoBack()
        {
            if (Control != null)
                Control.GoBack();
        }
        public void RefreshPage()
        {
            if (Control != null)
                Control.Reload();

        }


        public static void Init()
        {

        }
        //string ClientProcessIdentity = "";
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Control is Android.Webkit.WebView && WebViews.Contains(Control as Android.Webkit.WebView))
                    WebViews.Remove(Control as Android.Webkit.WebView);
            }
            catch (Exception error)
            {
            }
            base.Dispose(disposing);
            string channelUri = "local-device";
            // MessageDispatcher.DesconnectAsync(channelUri, SessionIdentity, ServerSessionPart.ServerProcessIdentity.ToString());

        }
        static List<Android.Webkit.WebView> WebViews = new List<Android.Webkit.WebView>();
        HybridWebView HybridWebView;
        static String USER_AGENT = "com.microneme.dontwait.android";// "Mozilla/5.0 (Linux; Android 4.1.1; Galaxy Nexus Build/JRO03C) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";

        string _SessionIdentity;
        public string SessionIdentity
        {
            get => _SessionIdentity;
            internal set
            {
                if (_SessionIdentity != value)
                    _SessionIdentity = value;
            }
        }

        Android.Webkit.WebView NativeWebView;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            //            if (Control == null)
            //            {


            //                var webView = new Android.Webkit.WebView(Context);
            //                //webView.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
            //                webView.Settings.CacheMode = CacheModes.Default;
            //                webView.Settings.DomStorageEnabled = true;
            //                webView.Settings.AllowFileAccess = true;
            //                webView.Settings.AllowFileAccessFromFileURLs = true;
            //                var ss = webView.Settings.UserAgentString;
            //                //webView.Settings.UserAgentString = USER_AGENT;



            //                // webView.Settings.CacheMode =CacheModes.NoCache;//.CacheMode(WebSettings.lo.LOAD_NO_CACHE);
            //                //webView.SetWebViewClient(new CustomWebViewClient(this));
            //                webView.Settings.JavaScriptEnabled = true;
            //                //webView.ClearCache(false);
            //                webView.SetWebViewClient(new JavascriptWebViewClient(webView, this, $"javascript: {JavaScriptFunction}", _context));
            //                WebViews.Add(webView);


            //                SetNativeControl(webView);
            //#if DEBUG
            //                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            //                {
            //                    Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
            //                }
            //#endif 
            //            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
                if (e.OldElement is HybridWebView)
                    (e.OldElement as HybridWebView).NativeWebBrowser = null;

            }
            HybridWebView = e.NewElement as HybridWebView;
            if (e.NewElement != null)
            {
                if (NativeWebView == null || NativeWebView != Control)
                {
                    NativeWebView = Control;

                    //NativeWebView.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
                    NativeWebView.Settings.CacheMode = CacheModes.Default;
                    NativeWebView.Settings.DomStorageEnabled = true;
                    NativeWebView.Settings.AllowFileAccess = true;
                    NativeWebView.Settings.AllowFileAccessFromFileURLs = true;
                    var ss = NativeWebView.Settings.UserAgentString;
                    //NativeWebView.Settings.UserAgentString = USER_AGENT;

                    // NativeWebView.Settings.CacheMode =CacheModes.NoCache;//.CacheMode(WebSettings.lo.LOAD_NO_CACHE);
                    //NativeWebView.SetWebViewClient(new CustomWebViewClient(this));
                    NativeWebView.Settings.JavaScriptEnabled = true;
                    //NativeWebView.ClearCache(false);
                    NativeWebView.SetWebViewClient(new JavascriptWebViewClient(NativeWebView, this, $"javascript: {JavaScriptFunction}", _context));
                    WebViews.Add(NativeWebView);

#if DEBUG
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                    {
                        Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
                    }
#endif
                }

                if (e.NewElement is HybridWebView)
                    (e.NewElement as HybridWebView).NativeWebBrowser = this;


                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                string url = null;
                if ((Element as HybridWebView).Uri != null)
                {
                    if ((Element as HybridWebView).Uri.IndexOf(@"local://") == 0)
                    {
                        url = (Element as HybridWebView).Uri.Replace(@"local://", @"file:///android_asset/Content/");
                    }
                    else if ((Element as HybridWebView).Uri.IndexOf(@"webapp://") == 0)
                    {
                        var webAppPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

                        var ll = System.IO.File.Exists(System.IO.Path.Combine(webAppPath, "webapp/index.html"));

                        url = @"file://" + System.IO.Path.Combine(webAppPath, "webapp/index.html");
                    }
                    else
                        url = (Element as HybridWebView).Uri;
                    //file:///android_asset/Content/index.html
                    Control.LoadUrl(url);

                }

                //Control.LoadUrl(string.Format(@"http://192.168.2.10/WebPart/{0}", Element.Uri));
                //Control.LoadUrl(string.Format(@"http://10.0.0.10/WebPart/{0}", Element.Uri));
                //Control.LoadUrl (string.Format ("file:///android_asset/Content/{0}", Element.Uri));
                // InjectJS(JavaScriptFunction);
            }
        }

        internal static void OnDestroy()
        {
            foreach (var webView in WebViews)
                webView.Destroy();
        }

        void InjectJS(string JavascriptFunction)
        {
            if (Control != null)
            {
                //Control.LoadUrl(string.Format("javascript: {0}", script));
                Control.EvaluateJavascript($"javascript: {JavascriptFunction}", null);
            }
        }

        internal void OnPageFinished(string url)
        {

        }



        public void SendResponce(ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData> SendRequestAsync(RequestData requestData)
        {
            string requestDatajson = JsonConvert.SerializeObject(requestData);
            requestDatajson = ((int)MessageHeader.Request).ToString() + requestDatajson;
            InvockeJSMethod("SendMessage", new[] { requestDatajson });
            // InvockeJSMethod("SendMessage", new[] { responseDatajson });

            return Task.FromResult<ResponseData>(null);


        }

        public ResponseData SendRequest(RequestData requestData)
        {
            throw new NotImplementedException();
        }
        public void RejectRequest(System.Threading.Tasks.Task<ResponseData> task)
        {
            throw new NotImplementedException();
        }

        public void Navigate(string url)
        {
            if (Control != null && Control.Url != url)
                Control.LoadUrl(url);
        }

        public string Url
        {
            get
            {
                if (Control != null)
                    return Control.Url;
                else
                    return null;
            }
        }

        public bool ConnectionIsOpen
        {

            get
            {
                try
                {
                    if (ThreadHelper.IsOnMainThread)
                    {
                        var url = this.Url;
                    }
                    else
                    {
                        var task=Device.InvokeOnMainThreadAsync(() =>
                        {

                            try
                            {
                                return this.Url;
                            }
                            catch (Exception error)
                            {
                                throw;
                            }
                        });
                        task.Wait();
                        if (task.Exception!=null)
                            return false;
                    }

                }
                catch (System.Exception error)
                {

                    string errorMessage = error.Message;
                    string errorStackTrace = error.StackTrace;


                   


                    return false;
                }
                return true;
            }
        }

    }



    /// <MetaDataID>{7e4f3217-90f8-4468-ad05-0d367c54c233}</MetaDataID>
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

        public JSBridge(HybridWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
        }


        [JavascriptInterface]
        [Export("invokeAction")]
        public async void InvokeAction(string data)
        {

            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {

                HybridWebViewRenderer hybridRenderer;

                if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
                {

                    if (Remoting.RestApi.RequestData.IsRequestMessage(data))
                    {
                        try
                        {

                            string headerStr = "" + data[0];
                            MessageHeader header = (MessageHeader)int.Parse(headerStr);
                            data = data.Substring(1);

                            if (header == MessageHeader.Request)
                            {
                                RequestData request = JsonConvert.DeserializeObject<RequestData>(data);

                                request.EventCallBackChannel = hybridRenderer;
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
                                    request.SetCallContextData("DataContext", hybridRenderer.Element.BindingContext);
                                    responseData = await MessageDispatcher.MessageDispatchAsync(request); //MessageDispatcher.MessageDispatch(request);
                                    responseData.CallContextID = request.CallContextID;
                                    responseData.BidirectionalChannel = true;
                                }

                                string responseDatajson = JsonConvert.SerializeObject(responseData);
                                responseDatajson = ((int)MessageHeader.Response).ToString() + responseDatajson;
                                hybridRenderer.SessionIdentity = responseData.SessionIdentity;

                                await hybridRenderer.InvockeJSMethod("SendMessage", new[] { responseDatajson });

                                //await Control.InvokeScriptAsync("SendMessage", new[] { responseDatajson });
                                //retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, _placementTarget.DataContext);

                            }
                        }
                        catch (Exception error)
                        {
                        }

                        return;
                    }


                    if (JSCallData.IsAsyncCall(data))
                    {
                        JSCallData jsCallData = JsonConvert.DeserializeObject<JSCallData>(data);


                        var obj = hybridRenderer.Element.BindingContext;


                        //await Control.InvokeScriptAsync("CSCallBack", new[] { jsCallData.CallID.ToString(), "Hello C# $ " + jsCallData.Args });


                        try
                        {
                            if (jsCallData.Args == "OnlyAsyncCall")
                            {
                                await hybridRenderer.InvockeJSMethod("CSCallBack", new[] { jsCallData.CallID.ToString(), "True" });
                            }
                            else
                            {
                                var retval = await OOAdvantech.Remoting.RestApi.MessageDispatcher.TryProcessMessageAsync(jsCallData.Args, hybridRenderer.Element.BindingContext);
                                //var task = System.Threading.Tasks.Task.Run(async() =>
                                //{
                                await hybridRenderer.InvockeJSMethod("CSCallBack", new object[] { jsCallData.CallID, retval });

                                //  await Control.InvokeScriptAsync("CSCallBack", new[] { jsCallData.CallID.ToString(), retval });
                                //});
                            }

                        }
                        catch (Exception error)
                        {
                        }
                    }


                    //hybridRenderer.InvockeJSMethod("CSharpCallResult", new object[] { "Slama" });
                    //hybridRenderer.Element.InvokeAction(data);
                }
            });

        }

    }
    /// <MetaDataID>{b2f35949-93a5-4082-b831-2c923a66f185}</MetaDataID>
    public class StringCallback : Java.Lang.Object, IValueCallback
    {
        private TaskCompletionSource<string> source;

        public Task<string> Task { get { return source.Task; } }

        bool taskCompleted = false;

        public StringCallback()
        {
            source = new TaskCompletionSource<string>();
        }

        public void SetTaskResult(string value)
        {
            lock (source)
            {
                if (!taskCompleted)
                {
                    taskCompleted = true;
                    source.SetResult(value);
                }
            }


        }
        public void SetTaskException(System.Exception exception)
        {
            source.SetException(exception);
        }
        public void OnReceiveValue(Java.Lang.Object value)
        {
            try
            {
                lock (source)
                {
                    if (taskCompleted)
                        return;
                    taskCompleted = true;
                }


                    var jstr = (Java.Lang.String)value;
                    var str = new string(jstr.AsEnumerable().ToArray());
                    source.SetResult(StringUtils.Unquote(str));
                
            }
            catch (System.Exception ex)
            {
                source.SetException(ex);
            }
        }
    }



    /// <summary>
    /// Provides utility functions for working with strings.
    /// </summary>
    /// <MetaDataID>{2c217d88-a73a-4f1c-ac47-f9001d3a481a}</MetaDataID>
    public static class StringUtils
    {
        /// <summary>
        /// Truncates the string to the specified maximum length.
        /// Discards characters at the end of the string with indices greater than
        /// or equal to <paramref name="maxLength"/>.
        /// </summary>
        /// <param name="str">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the string to retain.</param>
        /// <returns>The truncated string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is negative.</exception>
        public static string Truncate(string str, int maxLength)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "Max length must be non-negative.");

            if (str.Length > maxLength)
                return str.Substring(0, maxLength);

            return str;
        }

        /// <summary>
        /// If the string is longer than the specified maximum length, truncates
        /// it and appends an ellipsis mark ("...").  If the maximum length is
        /// less than or equal to 3, omits the ellipsis mark on truncation.
        /// </summary>
        /// <param name="str">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the string to retain
        /// including the ellipsis mark when used.</param>
        /// <returns>The truncated string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is negative.</exception>
        public static string TruncateWithEllipsis(string str, int maxLength)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "Max length must be non-negative.");

            if (str.Length > maxLength)
            {
                if (maxLength > 3)
                    return str.Substring(0, maxLength - 3) + @"...";
                else
                    return str.Substring(0, maxLength);
            }

            return str;
        }

        /// <summary>
        /// Gets a lowercase hexadecimal digit corresponding to the least significant nybble of
        /// the specified value.
        /// </summary>
        /// <param name="value">The value, only the last 4 bits of which are used.</param>
        /// <returns>The hexadecimal digit.</returns>
        public static char ToHexDigit(int value)
        {
            value = value & 0xf;
            return (char)(value < 10 ? 48 + value : 87 + value);
        }

        /// <summary>
        /// Formats a character value as "'x'" or "'\n'" with support for escaped characters
        /// as a valid literal value.  Encloses the char in single quotes (').
        /// </summary>
        /// <remarks>
        /// <para>
        /// Replaces common escaped characters with C# style escape codes.  Unprintable characters
        /// are represented by a Unicode character escape.
        /// </para>
        /// </remarks>
        /// <param name="value">The character value to format.</param>
        /// <returns>The formatted character.</returns>
        public static string ToCharLiteral(char value)
        {
            var str = new StringBuilder(8);
            str.Append('\'');
            AppendUnquotedCharLiteral(str, value);
            str.Append('\'');
            return str.ToString();
        }

        /// <summary>
        /// Escapes a character value as "x" or "\n".  Unlike <see cref="ToCharLiteral"/>,
        /// does not enclose the literal in single quotes (').
        /// </summary>
        /// <remarks>
        /// <para>
        /// Replaces common escaped characters with C# style escape codes.  Unprintable characters
        /// are represented by a Unicode character escape.
        /// </para>
        /// </remarks>
        /// <param name="value">The character value to format.</param>
        /// <returns>The unquoted char literal.</returns>
        public static string ToUnquotedCharLiteral(char value)
        {
            var str = new StringBuilder(6);
            AppendUnquotedCharLiteral(str, value);
            return str.ToString();
        }

        private static void AppendUnquotedCharLiteral(StringBuilder str, char value)
        {
            char previousChar = '\0';
            AppendEscapedChar(str, value, ref previousChar);
        }

        /// <summary>
        /// Formats a string value as ""abc\ndef"" with support for escaped characters
        /// as a valid literal value.  Encloses the string in quotes (").
        /// </summary>
        /// <remarks>
        /// <para>
        /// Replaces common escaped characters with C# style escape codes.  Unprintable characters
        /// are represented by a Unicode character escape.
        /// </para>
        /// </remarks>
        /// <param name="value">The string value to format.</param>
        /// <returns>The formatted string.</returns>
        public static string ToStringLiteral(string value)
        {
            StringBuilder str = new StringBuilder(value.Length + 2);
            str.Append('"');
            AppendUnquotedStringLiteral(str, value);
            str.Append('"');
            return str.ToString();
        }

        /// <summary>
        /// Escapes a string value such as "abc\ndef".  Unlike <see cref="ToStringLiteral"/>,
        /// does not enclose the literal in quotes (").
        /// </summary>
        /// <remarks>
        /// <para>
        /// Replaces common escaped characters with C# style escape codes.  Unprintable characters
        /// are represented by a Unicode character escape.
        /// </para>
        /// </remarks>
        /// <param name="value">The string value to format.</param>
        /// <returns>The unquoted string literal.</returns>
        public static string ToUnquotedStringLiteral(string value)
        {
            StringBuilder str = new StringBuilder(value.Length);
            AppendUnquotedStringLiteral(str, value);
            return str.ToString();
        }

        private static void AppendUnquotedStringLiteral(StringBuilder str, string value)
        {
            char previousChar = '\0';
            foreach (char c in value)
                AppendEscapedChar(str, c, ref previousChar);
        }

        private static void AppendEscapedChar(StringBuilder str, char c, ref char previousChar)
        {
            switch (c)
            {
                case '\0':
                    str.Append(@"\0");
                    break;

                case '\a':
                    str.Append(@"\a");
                    break;

                case '\b':
                    str.Append(@"\b");
                    break;

                case '\f':
                    str.Append(@"\f");
                    break;

                case '\n':
                    str.Append(@"\n");
                    break;

                case '\r':
                    str.Append(@"\r");
                    break;

                case '\t':
                    str.Append(@"\t");
                    break;

                case '\v':
                    str.Append(@"\v");
                    break;

                case '\'':
                    str.Append(@"\'");
                    break;

                case '\"':
                    str.Append(@"\""");
                    break;

                case '\\':
                    str.Append(@"\\");
                    break;

                default:
                    if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c) || char.IsWhiteSpace(c))
                    {
                        str.Append(c);
                    }
                    else
                    {
                        int code;
                        if (char.IsHighSurrogate(previousChar) && char.IsLowSurrogate(c))
                        {
                            code = char.ConvertToUtf32(previousChar, c);

                            str.Length -= 5;
                            str.Append('U');
                            str.Append('0');
                            str.Append('0');
                            str.Append(ToHexDigit(code >> 20));
                            str.Append(ToHexDigit(code >> 16));
                        }
                        else
                        {
                            code = c;

                            str.Append('\\');
                            str.Append('u');
                        }

                        str.Append(ToHexDigit(code >> 12));
                        str.Append(ToHexDigit(code >> 8));
                        str.Append(ToHexDigit(code >> 4));
                        str.Append(ToHexDigit(code));
                    }
                    break;
            }

            previousChar = c;
        }

        /// <summary>
        /// Parses a key/value pair from an input string of the form "key=value", with the
        /// value optionally quoted and optional surrounding whitespace removed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value is quoted, the outermost quotes will be removed.
        /// If the equals or value is absent then an empty string will be used as the value.
        /// Also trims whitespace around the key and value.
        /// </para>
        /// </remarks>
        /// <param name="input">The input string.</param>
        /// <returns>The key value pair.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is null.</exception>
        public static KeyValuePair<string, string> ParseKeyValuePair(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int equalsPos = input.IndexOf('=');
            if (equalsPos < 0)
                return new KeyValuePair<string, string>(input.Trim(), "");

            string key = input.Substring(0, equalsPos).Trim();
            string value = input.Substring(equalsPos + 1).Trim();

            return new KeyValuePair<string, string>(key, Unquote(value));
        }

        public static string Unquote(string value)
        {
            return IsQuoted(value) ? value.Substring(1, value.Length - 2) : value;
        }

        private static bool IsQuoted(string value)
        {
            if (value.Length < 2)
                return false;

            char firstChar = value[0];
            if (firstChar != '"' && firstChar != '\'')
                return false;

            return value[value.Length - 1] == firstChar;
        }

        /// <summary>
        /// Parses a string of whitespace delimited and possibly quoted arguments and
        /// returns an array of each one unquoted.
        /// </summary>
        /// <param name="arguments">The arguments string, eg. "/foo 'quoted arg' /bar.</param>
        /// <returns>The parsed and unquoted arguments.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arguments"/> is null.</exception>
        public static string[] ParseArguments(string arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            List<string> result = new List<string>();

            char quoteChar = '\0';
            bool inQuotes = false;

            int startPos, currentPos;
            for (startPos = 0, currentPos = 0; currentPos < arguments.Length; currentPos++)
            {
                char c = arguments[currentPos];
                if (currentPos > startPos)
                {
                    if (inQuotes && c == quoteChar && (currentPos + 1 == arguments.Length || char.IsWhiteSpace(arguments[currentPos + 1]))
                        || !inQuotes && char.IsWhiteSpace(c))
                    {
                        result.Add(arguments.Substring(startPos, currentPos - startPos));
                        startPos = currentPos + 1;
                        inQuotes = false;
                    }
                }
                else
                {
                    if (c == '"' || c == '\'')
                    {
                        inQuotes = true;
                        quoteChar = c;
                        startPos = currentPos + 1;
                    }
                    else if (char.IsWhiteSpace(c))
                    {
                        startPos = currentPos + 1;
                    }
                }
            }

            if (currentPos > startPos)
                result.Add(arguments.Substring(startPos, currentPos - startPos));

            return result.ToArray();
        }
    }


    /// <MetaDataID>{87f035d5-1b78-4c52-833c-39b2a5911a10}</MetaDataID>
    public class JavascriptWebViewClient : WebViewClient, IValueCallback
    {
        string _javascript;

        Context Context;
        public JavascriptWebViewClient(Android.Webkit.WebView view, HybridWebViewRenderer hybridWebViewRenderer, string javascript, Context context)
        {
            Context = context;
            _javascript = javascript;
            View = view;
            this.hybridWebViewRenderer = hybridWebViewRenderer;
        }
        private HybridWebViewRenderer hybridWebViewRenderer;

        Android.Webkit.WebView View;
        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);

            View = view;
            view.EvaluateJavascript("csCodeCallDefined()", this);
            hybridWebViewRenderer.OnNavigated(new NavigatedEventArgs(view, url, view.CanGoBack(), view.CanGoForward()));
        }

        public override void OnReceivedError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);
        }
        public override void OnReceivedHttpError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceResponse errorResponse)
        {
            base.OnReceivedHttpError(view, request, errorResponse);
        }
        public override void OnReceivedSslError(Android.Webkit.WebView view, SslErrorHandler handler, SslError error)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Context);
            AlertDialog alertDialog = builder.Create();
            String message = "SSL Certificate error.";
            switch (error.PrimaryError)
            {
                case SslErrorType.Untrusted:
                    message = "The certificate authority is not trusted.";
                    break;
                case SslErrorType.Expired:
                    message = "The certificate has expired.";
                    break;
                case SslErrorType.Idmismatch:
                    message = "The certificate Hostname mismatch.";
                    break;
                case SslErrorType.Notyetvalid:
                    message = "The certificate is not yet valid.";
                    break;
            }

            message += " Do you want to continue anyway?";
            alertDialog.SetTitle("SSL Certificate Error");
            alertDialog.SetMessage(message);
            alertDialog.SetButton((int)DialogButtonType.Positive, "OK", new EventHandler<DialogClickEventArgs>((object sender, DialogClickEventArgs e) =>
            {

                handler.Proceed();

            }));
            alertDialog.Show();

        }


        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            return hybridWebViewRenderer.OnShouldOverrideUrlLoading(url);
        }


        void handllerNotingButton(object sender, DialogClickEventArgs e)
        {

        }
        public void OnReceiveValue(Java.Lang.Object value)
        {
            try
            {
                var jstr = (Java.Lang.String)value;
                var str = new string(jstr.AsEnumerable().ToArray());
                var ret = StringUtils.Unquote(str);
                Android.Webkit.WebView view = View;
                if (ret == "false")
                {

                    if (view != null)
                    {
                        view.EvaluateJavascript(_javascript, null);

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {

                            view.EvaluateJavascript("SendMessage('CSCodeCommunicationStart')", null);
                        });
                    }

                }
            }
            catch (System.Exception error)
            {

            }
            View = null;

        }

        //public bool shouldOverrideUrlLoading(WebView view, String url)
        //{
        //    return false;
        //}
    }



    public class CustomWebChromeClient : WebChromeClient
    {
        public override bool OnCreateWindow(Android.Webkit.WebView view, bool isDialog, bool isUserGesture, Message resultMsg)
        {
            return base.OnCreateWindow(view, isDialog, isUserGesture, resultMsg);
        }
    }

}

