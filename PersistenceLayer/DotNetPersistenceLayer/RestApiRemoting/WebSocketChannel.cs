using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;

#if DeviceDotNet
using Xamarin.Essentials;
#endif

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{2b78dfaf-e415-4d38-8ef7-0d8c7bc759d0}</MetaDataID>
    public class WebSocketChannel : IChannel
    {


        /// <MetaDataID>{3bfe5a41-8777-4aa9-b67a-768f64927eae}</MetaDataID>
        private string ChannelUri;
        /// <MetaDataID>{7960b443-53b0-4b91-b21b-b398549e8d24}</MetaDataID>
        ClientSessionPart ClientSessionPart;
        /// <MetaDataID>{dc7a9429-8523-448b-9849-a743bf657471}</MetaDataID>
        System.Timers.Timer DirectConnectionTimer = new System.Timers.Timer();

        /// <MetaDataID>{4e08166e-6f56-4e4b-a753-0ce330005b32}</MetaDataID>
        System.Timers.Timer CloseConnectionTimer = new System.Timers.Timer();
        /// <MetaDataID>{5017efc1-c665-4e98-95e3-b6d7a2bb22e1}</MetaDataID>
        public WebSocketChannel(string channelUri, ClientSessionPart clientSessionPart)
        {
#if DEBUG
            this.Timer = new Timer(new System.Threading.TimerCallback(OnTimer), null, 20000, 10000);
#endif
            this.ChannelUri = channelUri;
            ClientSessionPart = clientSessionPart;
            DirectConnectionTimer.Interval = 3000;
            DirectConnectionTimer.Elapsed += DirectConnectionTimer_Elapsed;
            DirectConnectionTimer.Stop();


            CloseConnectionTimer.Interval = 3000;
            CloseConnectionTimer.Elapsed += CloseConnectionTimer_Elapsed;

            string publicChannelUri = null;
            string internalchannelUri = null;
            ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);
            WebSocketClient = WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages", Binding.DefaultBinding);
            //this.ChannelIsOpenEvent.Set();
            //WebSocketEndPoint = WebSocketClient;



#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Resumed += Apllication_Resumed;
#endif


        }

        /// <MetaDataID>{79fe8961-bdda-4ff2-b13c-78a8218418f1}</MetaDataID>
        private void Apllication_Resumed()
        {
            //DirectConnectionTimer.Start();

            RequestData requestData = new RequestData();
            requestData.RequestType = RequestType.ConnectionInfo;
            requestData.ChannelUri = ChannelUri;
            var task = WebSocketClient.SendRequestAsync(requestData);
            if (!task.Wait(TimeSpan.FromSeconds(2)))
            {



                _WebSocketClient.RemoveWebSocketChannel(this);
                if (_WebSocketClient.WebSocketChannels.Count == 0 && _WebSocketClient.State == WebSocketState.Open)
                {
                    _WebSocketClient.CloseAsync();
                    _WebSocketClient = null;

                    //                    if (_WebSocketClient.PendingRequest > 0)
                    //                    {
                    //                        PendingClientSocketsToClose.Add(_WebSocketClient);
                    //                        CloseConnectionTimer.Start();
                    //                    }
                    //                    else
                    //                    {
                    //                        _WebSocketClient.CloseAsync();
                    //#if !DeviceDotNet
                    //                               System.Threading.Thread.Sleep(1000);
                    //#else
                    //                        System.Threading.Tasks.Task.Delay(1000).Wait();
                    //#endif

                    //                    }
                }
                string publicChannelUri = null;
                string internalchannelUri = null;
                ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);

                Binding binding = new Binding() { OpenTimeout = TimeSpan.FromSeconds(2) };
                WebSocketClient = WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, Binding.DefaultBinding);// WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages", binding);

                requestData = new RequestData();
                requestData.RequestType = RequestType.ConnectionInfo;
                requestData.ChannelUri = ChannelUri;
                var responseData = this.ProcessRequest(requestData);
                if (!responseData.DirectConnect)
                    DirectConnectionTimer.Start();
            }
        }

        /// <MetaDataID>{14cf0f67-ed52-48fe-a153-25bcea27b71b}</MetaDataID>
        private void CloseConnectionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                CloseConnectionTimer.Stop();
                lock (PendingClientSocketsToClose)
                {
                    foreach (var clientSocket in PendingClientSocketsToClose.ToList())
                    {
                        if (clientSocket.PendingRequest == 0 &&
                            clientSocket.WebSocketChannels.Count == 0 &&
                            clientSocket.State == WebSocketState.Open)
                        {
                            PendingClientSocketsToClose.Remove(clientSocket);
                            clientSocket.CloseAsync();

                        }
                    }
                }
                if (PendingClientSocketsToClose.Count > 0)
                    CloseConnectionTimer.Start();
            });
        }

#if DEBUG
        Timer Timer;

        bool onLagTest = false;
        void OnTimer(object state)
        {

            try
            {
                if (onLagTest)
                    return;

                onLagTest = true;
                var endPoint = EndPoint;
                if (endPoint != null &&
                    endPoint is WebSocketClient &&
                        (endPoint as WebSocketClient).State == WebSocketState.Open)
                {
                    RequestData requestData = new RequestData();
                    requestData.RequestType = RequestType.LagTest;
                    requestData.ChannelUri = ChannelUri;
                    DateTime dateTime = DateTime.Now;

                    var task = endPoint.SendRequestAsync(requestData);
                    task.Wait(5000);
                    var timeSpan = DateTime.Now - dateTime;
                    System.Diagnostics.Debug.WriteLine("timeout for " + ChannelUri + "  " + timeSpan.ToString());

                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine("timeout for " + ChannelUri + "  more than 5 seconds");
            }
            finally
            {
                onLagTest = false;
            }

        }

#endif

        /// <MetaDataID>{0a5af218-8654-4a68-a243-7503eb5b7f8b}</MetaDataID>
        bool DirectConnectionCheck;

        /// <MetaDataID>{ce70f914-1f4d-4dff-8830-b654eaa7c9c5}</MetaDataID>
        int DirectConnectionTries;
        /// <MetaDataID>{601f0695-33c1-48e0-9e2c-55654b56f4a5}</MetaDataID>
        List<WebSocketClient> PendingClientSocketsToClose = new List<WebSocketClient>();

        /// <MetaDataID>{4a724d9e-8273-46ec-b041-58e9d4551c64}</MetaDataID>
        private void DirectConnectionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            Task.Run(() =>
             {
                 try
                 {
                     lock (this)
                     {
                         DirectConnectionCheck = true;
                     }
                     DirectConnectionTimer.Stop();
                     lock (DirectConnectionTimer)
                     {

                         var datetime = DateTime.Now;
                         string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();


                         System.Diagnostics.Debug.WriteLine(string.Format("RestApp {0}  {1} DirectConnectionTimer Check ConnectionInfo ", timestamp, ChannelUri));

                         string publicChannelUri = null;
                         string internalchannelUri = null;
                         ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);

                         if (WebSocketClient.WebSocketConnections.ContainsKey((publicChannelUri + "WebSocketMessages" + internalchannelUri).ToLower()))
                         {
                             if (WebSocketClient.WebSocketConnections[(publicChannelUri + "WebSocketMessages" + internalchannelUri).ToLower()].State == WebSocketState.Open)
                             {
                                 var requests = WebSocketClient.WebSocketConnections[(publicChannelUri + "WebSocketMessages" + internalchannelUri).ToLower()].GetRequestTasks().Count;

                                 var dire = WebSocketClient.WebSocketConnections[(publicChannelUri + "WebSocketMessages" + internalchannelUri).ToLower()].DirectConnect;
                             }

                         }

                         var tryDirectConnectionSocketClient = WebSocketClient.OpenNewConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, Binding.DefaultBinding);

                         if (tryDirectConnectionSocketClient == null)
                             return;

                         //WebSocketEndPoint = WebSocketClient;


                         RequestData requestData = new RequestData();
                         requestData.RequestType = RequestType.ConnectionInfo;
                         requestData.ChannelUri = ChannelUri;
                         var task = tryDirectConnectionSocketClient.SendRequestAsync(requestData);
                         if (task.Wait(Binding.DefaultBinding.SendTimeout))
                         {

                             var responseData = task.Result; ;
                             if (responseData.DirectConnect)
                             {
                                 WebSocketClient = tryDirectConnectionSocketClient;
                                 WebSocketClient.ReplaceConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, tryDirectConnectionSocketClient);
                                 // WebSocketEndPoint = WebSocketClient;
                             }
                             else
                             {
                                 DirectConnectionTries++;
                                 tryDirectConnectionSocketClient.CloseAsync();
                                 DirectConnectionTimer.Interval = 3000;
                                 DirectConnectionTimer.Start();
                             }
                         }
                         else
                         {
                             tryDirectConnectionSocketClient.RejectRequest(task);
                             DirectConnectionTimer.Interval = 3000;
                             DirectConnectionTimer.Start();
                         }

                     }
                 }
                 finally
                 {
                     lock (this)
                     {
                         DirectConnectionCheck = false;
                     }

                 }
             });

        }

        /// <MetaDataID>{94e7c48b-b97d-4919-a674-f6aa926263ad}</MetaDataID>
        public WebSocketChannel(IEndPoint webSocketEndPoint)
        {

            WebSocketEndPoint = webSocketEndPoint;
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(webSocketEndPoint as MarshalByRefObject))
            {

            }
#if DEBUG
            this.Timer = new Timer(new System.Threading.TimerCallback(OnTimer), null, 20000, 1000);
#endif
            //this.ChannelIsOpenEvent.Set();
        }

        ///// <summary>
        ///// The system blocks all other thread to use the channel except the thread which initialize channel
        ///// </summary>
        //[ThreadStatic]
        //bool DontWaitChannleEvent;

        ///// <summary>
        ///// When channel is ready to send the ChannelIsOpenEvent is set.
        ///// Otherwise is reset
        ///// </summary>
        //System.Threading.ManualResetEvent ChannelIsOpenEvent = new System.Threading.ManualResetEvent(true);


        ///// <summary>
        ///// 
        ///// </summary>
        //private void EnsureChannelIsOpen()
        //{
        //    if (!DontWaitChannleEvent)
        //    {
        //        if (this.ChannelIsOpenEvent.WaitOne(Binding.DefaultBinding.SendTimeout))
        //            throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
        //    }
        //    else
        //    {
        //    }
        //}



        /// <MetaDataID>{e8caaa80-4c5e-40fe-837f-794da0159790}</MetaDataID>
        bool SusspendWebSocketClientChange;

        /// <exclude>Excluded</exclude>
        WebSocketClient _WebSocketClient;

        [Association("", Roles.RoleA, "25f6cb73-2c3a-4cb1-b575-56521957754d")]
        [RoleAMultiplicityRange(1, 1)]
        public WebSocketClient WebSocketClient
        {
            get
            {
                return _WebSocketClient;
            }
            set
            {

                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(this, 25000, ref lockTaken);
                    if (!lockTaken)
                        throw new TimeoutException(); // or compensate

                    SusspendWebSocketClientChange = true;
                    try
                    {
                        if (_WebSocketClient != null)
                        {
                            if (_WebSocketClient != value)
                            {
                                var datetime = DateTime.Now;
                                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                                System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel replace WebSocketClient  {0} ", timestamp));

#region Wait pending requests to complete  
                                Dictionary<int, TaskCompletionSource<ResponseData>> requestTasks = _WebSocketClient.GetRequestTasks();
                                if (requestTasks.Count > 0)
                                {
                                    foreach (var key in requestTasks.Keys)
                                    {
                                        TaskCompletionSource<ResponseData> task = null;
                                        if (requestTasks.TryGetValue(key, out task))
                                        {
                                            if (!task.Task.Wait(System.TimeSpan.FromSeconds(25)))
                                                task.Task.Wait(Binding.DefaultBinding.SendTimeout);
                                        }
                                    }
                                }
#endregion

                                CloseWebSocket();

                                if (_WebSocketClient.State == WebSocketState.Open)
                                    DropPhysicalConnection(_WebSocketClient);
                                else
                                {
                                }
                                _WebSocketClient.Closed -= WebSocketClient_Closed;
                                _WebSocketClient = value;
                                _WebSocketClient.Closed += WebSocketClient_Closed;
                                _WebSocketClient.AddWebSocketChannel(this);

                                WebSocketEndPoint = WebSocketClient;
                                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(WebSocketEndPoint as MarshalByRefObject))
                                {

                                }
                                ClientSessionPart.Reconnect();

                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            _WebSocketClient = value;
                            WebSocketEndPoint = WebSocketClient;
                            _WebSocketClient.Closed += WebSocketClient_Closed;
                            _WebSocketClient.AddWebSocketChannel(this);
                        }
                    }
                    finally
                    {
                        SusspendWebSocketClientChange = false;
                    }                                   // work here...
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(this);
                }

                //lock (this)
                //{
                //    SusspendWebSocketClientChange = true;
                //    try
                //    {
                //        if (_WebSocketClient != null)
                //        {
                //            if (_WebSocketClient != value)
                //            {
                //                var datetime = DateTime.Now;
                //                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                //                System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel replace WebSocketClient  {0} ", timestamp));

                //                #region Wait pending requests to complete  
                //                Dictionary<int, TaskCompletionSource<ResponseData>> requestTasks = _WebSocketClient.GetRequestTasks();
                //                if (requestTasks.Count > 0)
                //                {
                //                    foreach (var key in requestTasks.Keys)
                //                    {
                //                        TaskCompletionSource<ResponseData> task = null;
                //                        if (requestTasks.TryGetValue(key, out task))
                //                        {
                //                            if (!task.Task.Wait(System.TimeSpan.FromSeconds(25)))
                //                                task.Task.Wait(Binding.DefaultBinding.SendTimeout);
                //                        }
                //                    }
                //                }
                //                #endregion

                //                CloseWebSocket();

                //                if (_WebSocketClient.State == WebSocketState.Open)
                //                    DropPhysicalConnection(_WebSocketClient);
                //                else
                //                {
                //                }
                //                _WebSocketClient.Closed -= WebSocketClient_Closed;
                //                _WebSocketClient = value;
                //                _WebSocketClient.Closed += WebSocketClient_Closed;
                //                _WebSocketClient.AddWebSocketChannel(this);

                //                WebSocketEndPoint = WebSocketClient;
                //                ClientSessionPart.Reconnect();

                //            }
                //            else
                //            {
                //            }
                //        }
                //        else
                //        {
                //            _WebSocketClient = value;
                //            WebSocketEndPoint = WebSocketClient;
                //            _WebSocketClient.Closed += WebSocketClient_Closed;
                //            _WebSocketClient.AddWebSocketChannel(this);
                //        }
                //    }
                //    finally
                //    {
                //        SusspendWebSocketClientChange = false;
                //    }
                //}
            }
        }

        /// <summary>
        /// Close web socket when used exclusively from this channel.
        ///  
        /// </summary>
        /// <MetaDataID>{c246bfd9-e649-43e5-9c9d-15107cfcc9ff}</MetaDataID>
        private void CloseWebSocket()
        {
            _WebSocketClient.RemoveWebSocketChannel(this);

            if (_WebSocketClient.WebSocketChannels.Count == 0 && _WebSocketClient.State == WebSocketState.Open)
            {
                if (_WebSocketClient.PendingRequest > 0)
                {
                    //Close web socket when all pending requests ended. 
                    PendingClientSocketsToClose.Add(_WebSocketClient);
                    CloseConnectionTimer.Start();
                }
                else
                {
                    _WebSocketClient.CloseAsync();
#if !DeviceDotNet
                    System.Threading.Thread.Sleep(1000);
#else
                    System.Threading.Tasks.Task.Delay(1000).Wait();
#endif
                }
            }
        }

        /// <MetaDataID>{df0db290-5ec8-4352-bfed-a48351ce44af}</MetaDataID>
        private void WebSocketClient_Closed(object sender, EventArgs e)
        {
            WebSocketClient closedWebSocketClient = (WebSocketClient)sender;
            if (!SusspendWebSocketClientChange)
            {

                Task.Run(() =>
                {
                    while (true)
                    { 

#if DeviceDotNet
                        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                        {
                            System.Threading.Thread.Sleep(100);
                            continue;
                        }
#endif

                    try
                    {
                        string publicChannelUri = null;
                        string internalchannelUri = null;
                        ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);
                        var binding = new Binding();
                        binding.OpenTimeout = TimeSpan.FromSeconds(3);

                        var webSocketClient = WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages", binding);
                        if (webSocketClient != null && webSocketClient.State == WebSocketState.Open)
                        {
                            WebSocketClient = webSocketClient;
                            ClientSessionPart.Reconnect();
                        }
                        else
                        {
                        }
                    }
                    catch (Exception error)
                    {
                    }
                    if (WebSocketClient != null && WebSocketClient.State == WebSocketState.Open)
                        break;
                    else
                    {
                    }
                }

                });
        }
    }

    /// <MetaDataID>{624c1f21-7502-42ce-b896-ad111b5d36a4}</MetaDataID>
    public IEndPoint EndPoint
    {
        get
        {

            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(this, 25000, ref lockTaken);
                if (!lockTaken)
                    throw new TimeoutException(); // or compensate

                return WebSocketEndPoint;                                    // work here...
            }
            finally
            {
                if (lockTaken) Monitor.Exit(this);
            }

            //lock (this)
            //{
            //    return WebSocketEndPoint;
            //}
        }

        set
        {
            WebSocketEndPoint = value;
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(WebSocketEndPoint as MarshalByRefObject))
            {

            }
        }
    }


    ///// <MetaDataID>{a5bb4adc-2933-4a89-b8bf-bf302f1cc7bc}</MetaDataID>
    //WebSocketClient WebSocketClient
    //{
    //    get
    //    {
    //        return _WebSocketClient;
    //    }
    //    set
    //    {
    //        if (_WebSocketClient != value && _WebSocketClient != null)
    //        {
    //            _WebSocketClient.RemoveWebSocketChannel(this);
    //            if (_WebSocketClient.WebSocketChannels.Count == 0 && _WebSocketClient.State == WebSocketState.Open)
    //            {
    //                if (_WebSocketClient.PendingRequest > 0)
    //                {
    //                    PendingClientSocketsToClose.Add(_WebSocketClient);
    //                    CloseConnectionTimer.Start();
    //                }
    //                else
    //                {
    //                    _WebSocketClient.CloseAsync();
    //                    System.Threading.Thread.Sleep(1000);
    //                }
    //            }

    //            DropLogicalConnection(_WebSocketClient);
    //        }
    //        if (_WebSocketClient != null)
    //        {
    //            _WebSocketClient = value;
    //            WebSocketEndPoint = WebSocketClient;
    //            _WebSocketClient.AddWebSocketChannel(this);
    //            ClientSessionPart.Reconnect();
    //        }
    //        else
    //        {
    //            _WebSocketClient = value;
    //            WebSocketEndPoint = WebSocketClient;
    //            _WebSocketClient.AddWebSocketChannel(this);
    //        }
    //    }
    //}

    /// <MetaDataID>{e5ff0c28-c397-46cc-bfa1-cec9eb3a17af}</MetaDataID>
    private void DropPhysicalConnection(WebSocketClient webSocket)
    {
        string clientProcessIdentity = ClientSessionPart.SessionIdentity.Substring(0, ClientSessionPart.SessionIdentity.IndexOf("."));
        var datetime = DateTime.Now;
        string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
        //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel DropLogicalConnection  {0} ", timestamp));

        var methodCallMessage = new MethodCallMessage(ClientSessionPart.ChannelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", clientProcessIdentity, "", StandardActions.Disconnected, new object[0]);
        RequestData requestData = new RequestData();
        requestData.RequestType = RequestType.Disconnect;
        requestData.SessionIdentity = ClientSessionPart.SessionIdentity;
        requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
        var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
        requestData.ChannelUri = ClientSessionPart.ChannelUri;
        var task = webSocket.SendRequestAsync(requestData);
        if (!task.Wait(System.TimeSpan.FromSeconds(2)))
            if (!task.Wait(System.TimeSpan.FromSeconds(25)))
                task.Wait(Binding.DefaultBinding.SendTimeout);
    }

    //private void ReconfigureLogicalConnection(WebSocketClient webSocket)
    //{

    //    var datetime = DateTime.Now;
    //    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
    //    System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel Send ReconfigureChannel  {0} {1} ", timestamp, ClientSessionPart.SessionIdentity));


    //    string clientProcessIdentity = ClientSessionPart.SessionIdentity.Substring(0, ClientSessionPart.SessionIdentity.IndexOf("."));
    //    var methodCallMessage = new MethodCallMessage(ClientSessionPart.ChannelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", clientProcessIdentity, "", StandardActions.ReconfigureChannel, new object[0]);
    //    RequestData requestData = new RequestData();
    //    requestData.RequestType = RequestType.Disconnect;
    //    requestData.SessionIdentity = ClientSessionPart.SessionIdentity;
    //    requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
    //    var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
    //    requestData.ChannelUri = ClientSessionPart.ChannelUri;
    //    var task = webSocket.SendRequestAsync(requestData);
    //    if (!task.Wait(System.TimeSpan.FromSeconds(2)))
    //        if (!task.Wait(System.TimeSpan.FromSeconds(25)))
    //            task.Wait(Binding.DefaultBinding.SendTimeout);
    //}
    /// <MetaDataID>{c30cb6c5-12be-49c3-8a19-07f1a2282082}</MetaDataID>
    internal IEndPoint WebSocketEndPoint;

    /// <MetaDataID>{89c008a5-539c-4a94-b983-49f0ff9d09ff}</MetaDataID>
    public Task<ResponseData> AsyncProcessRequest(RequestData requestData)
    {
        if (EndPoint != null)
        {
#if DeviceDotNet
            var task = WebSocketEndPoint.SendRequestAsync(requestData);
            return task;

#else
                if (!System.Runtime.Remoting.RemotingServices.IsTransparentProxy(EndPoint as MarshalByRefObject))
                {
                    var task = EndPoint.SendRequestAsync(requestData);
                    return task;
                }
                else
                {
                    return Task<ResponseData>.Run(() =>
                      {
                          return EndPoint.SendRequest(requestData);
                      });
                }
#endif
        }
        else
        {
#region Uses web socket request channel
            //string publicChannelUri = null;
            //string internalchannelUri = null;
            //ByRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);

            //WebSocketClient webSocket = WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages");


            //if (webSocket.State != WebSocket4Net.WebSocketState.Open)
            //{
            //    ReturnMessage responseMessage = new ReturnMessage();
            //    responseMessage.Exception = new RestApiExceptionData();
            //    responseMessage.Exception.ExceptionMessage = webSocket.SocketException.Message;
            //    responseMessage.Exception.ServerStackTrace = webSocket.SocketException.StackTrace;
            //    responseMessage.Exception.ExceptionCode = ExceptionCode.ConnectionError;
            //    return Task<ResponseData>.Factory.StartNew(() =>
            //    {
            //        return new ResponseData() { CallContextID = requestData.CallContextID, SessionIdentity = requestData.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
            //    });
            //}
            var task = WebSocketEndPoint.SendRequestAsync(requestData);
            return task;
#endregion
        }
    }

    /// <MetaDataID>{cfb7fd00-d3cb-4edb-88c0-70b0cc072829}</MetaDataID>
    public ResponseData ProcessRequest(RequestData requestData)
    {
        //EnsureChannelIsOpen();

        IEndPoint endPoint = null;

        try
        {
            endPoint = EndPoint;
        }
        catch (TimeoutException error)
        {
            throw new System.Net.WebException(error.Message, error, System.Net.WebExceptionStatus.ConnectFailure, null);
        }

        if (endPoint != null)
        {
            var task = endPoint.SendRequestAsync(requestData);
            if (!task.Wait(System.TimeSpan.FromSeconds(25)))
            {
                if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                {
                    endPoint.RejectRequest(task);
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }

            }
            var responseData = task.Result;
            if (!responseData.DirectConnect)
            {
                lock (this)
                {
                    if (!DirectConnectionCheck)
                    {
                        if (!DirectConnectionTimer.Enabled)
                            DirectConnectionTimer.Start();
                    }
                }
            }

            //task.Wait();
            return responseData;
        }
        else
        {
#region Uses web socket request channel
            //string publicChannelUri = null;
            //string internalchannelUri = null;
            //ByRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);

            //WebSocketClient webSocket = WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages");
            //if (webSocket.State != WebSocket4Net.WebSocketState.Open)
            //{
            //    ReturnMessage responseMessage = new ReturnMessage();
            //    responseMessage.Exception = new RestApiExceptionData();
            //    responseMessage.Exception.ExceptionMessage = webSocket.SocketException.Message;
            //    responseMessage.Exception.ServerStackTrace = webSocket.SocketException.StackTrace;
            //    responseMessage.Exception.ExceptionCode = ExceptionCode.ConnectionError;
            //    return new ResponseData() { CallContextID = requestData.CallContextID, SessionIdentity = requestData.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
            //}

            return endPoint.SendRequest(requestData);
            //var task = WebSocketEndPoint.SendRequestWithResponceAsync(requestData);
            //if (!task.Wait(System.TimeSpan.FromSeconds(25)))
            //{
            //    task.Wait();
            //}
            //task.Wait();
            //return task.Result;
#endregion
        }
    }



    /// <MetaDataID>{93778861-f327-4e2b-aad7-7bacde6fffc5}</MetaDataID>
    public void Close()
    {
        if (WebSocketClient != null)
            WebSocketClient.CloseAsync();
#if DeviceDotNet
        OOAdvantech.DeviceApplication.Current.Resumed -= Apllication_Resumed;
#endif

    }

    /// <MetaDataID>{72be126e-7a66-4512-978b-cd813cc82b3f}</MetaDataID>
    public void DropPhysicalConnection()
    {
        string publicChannelUri = null;
        string internalchannelUri = null;
        ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);

        var tryDirectConnectionSocketClient = WebSocketClient.OpenNewConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, Binding.DefaultBinding);

        if (tryDirectConnectionSocketClient == null)
        {
            lock (this)
            {
                if (!DirectConnectionCheck)
                {
                    if (!DirectConnectionTimer.Enabled)
                        DirectConnectionTimer.Start();
                }
            }
        }
        else
            WebSocketClient = tryDirectConnectionSocketClient;

    }
}
}
