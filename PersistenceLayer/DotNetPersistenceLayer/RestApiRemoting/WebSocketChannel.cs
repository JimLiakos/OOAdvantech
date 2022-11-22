using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;

#if DeviceDotNet
using Xamarin.Essentials;
using Xamarin.Forms;
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

#if DeviceDotNet
            if (Application.Current is IAppLifeTime)
            {
                (Application.Current as IAppLifeTime).ApplicationResuming +=ApplicationResuming;
                (Application.Current as IAppLifeTime).ApplicationSleeping +=ApplicationSleeping;
            }

            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "Opens : "+channelUri });
#endif

#if DEBUG
            this.WebSocketResponseTimeCheckTimer = new System.Threading.Timer(new System.Threading.TimerCallback(OnWebSocketResponseTimeCheckTick), null, 20000, 10000);
#endif
            this.ChannelUri = channelUri;
            ClientSessionPart = clientSessionPart;
            DirectConnectionTimer.Interval = 3000;
            DirectConnectionTimer.Elapsed += TryDirectConnectionTimerTick;
            DirectConnectionTimer.Stop();


            CloseConnectionTimer.Interval = 3000;
            CloseConnectionTimer.Elapsed += CloseCanceledConnectionTimerTick;

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

        object AppLifeTimeLock = new object();
        bool Sleeping = false;

        private void ApplicationSleeping(object sender, EventArgs e)
        {
            lock (AppLifeTimeLock)
                Sleeping=true;

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "WebSocketChannel : ApplicationSleeping" });
#endif
        }

        private void ApplicationResuming(object sender, EventArgs e)
        {
            lock (AppLifeTimeLock)
                Sleeping=false;

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "WebSocketChannel : ApplicationResuming" });
#endif
        }

        static DateTime? OffLineDatetime;
#if DeviceDotNet

        static WebSocketChannel()
        {
            Connectivity.ConnectivityChanged+=Connectivity_ConnectivityChanged;
        }


        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess!=NetworkAccess.Internet)
                OffLineDatetime=System.DateTime.UtcNow;
            else
                OffLineDatetime=null;

        }
#endif

        /// <MetaDataID>{79fe8961-bdda-4ff2-b13c-78a8218418f1}</MetaDataID>
        private void Apllication_Resumed()
        {
            //MainThread.BeginInvokeOnMainThread(() =>
            //{
            //    Application.Current?.MainPage?.DisplayAlert("App","Application Resume","")
            //    // Code to run on the main thread
            //});
#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "WebSocketReconnect : "+DateTime.Now.ToString()+" - " +ChannelUri });
#endif
            WebSocketReconnect();

            //DirectConnectionTimer.Start();


            //RequestData requestData = new RequestData();
            //requestData.RequestType = RequestType.ConnectionInfo;
            //requestData.ChannelUri = ChannelUri;
            //requestData.SendTimeout = TimeSpan.FromSeconds(2).TotalMilliseconds;
            //var task = WebSocketClient.SendRequestAsync(requestData);

            //if (!task.Wait(TimeSpan.FromSeconds(2)))
            //{



            //    _WebSocketClient.RemoveWebSocketChannel(this);
            //    if (_WebSocketClient.WebSocketChannels.Count == 0 && _WebSocketClient.State == WebSocketState.Open)
            //    {
            //        _WebSocketClient.CloseAsync();
            //        _WebSocketClient = null;

            //    }
            //    string publicChannelUri = null;
            //    string internalchannelUri = null;
            //    ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);

            //    Binding binding = new Binding() { OpenTimeout = TimeSpan.FromSeconds(2) };
            //    WebSocketClient = WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, Binding.DefaultBinding);// WebSocketClient.EnsureConnection(publicChannelUri + "WebSocketMessages", binding);

            //    requestData = new RequestData();
            //    requestData.RequestType = RequestType.ConnectionInfo;
            //    requestData.ChannelUri = ChannelUri;
            //    var responseData = this.ProcessRequest(requestData);
            //    if (!responseData.DirectConnect)
            //        DirectConnectionTimer.Start();
            //}
        }

        /// <summary>
        /// Direct connections mechanism close indirect web socket connections immediately when there aren't pending request that use the indirection connection
        /// otherwise there is a timer to check the pending request periodically.
        /// If there aren't no more pending request then closes the canceled connection web socket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <MetaDataID>{14cf0f67-ed52-48fe-a153-25bcea27b71b}</MetaDataID>
        private void CloseCanceledConnectionTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                CloseConnectionTimer.Stop();
                lock (PendingClientSocketsToClose)
                {
                    foreach (var clientSocket in PendingClientSocketsToClose.ToList())
                    {
                        if (clientSocket.PendingRequest == 0 &&
                            clientSocket.WebSocketChannels.Where(x => x != this).Count() == 0 &&
                            clientSocket.State == WebSocketState.Open)
                        {
                            PendingClientSocketsToClose.Remove(clientSocket);
                            clientSocket.RemoveWebSocketChannel(this);
                            clientSocket.CloseAsync();

                        }
                    }
                }
                if (PendingClientSocketsToClose.Count > 0)
                    CloseConnectionTimer.Start();
            });
        }


#if DEBUG

        #region websocket response time monitoring
        /// <MetaDataID>{37904b63-e2b9-43fd-9771-9580c685ced3}</MetaDataID>
        System.Threading.Timer WebSocketResponseTimeCheckTimer;
        /// <MetaDataID>{1949a48e-bb8b-4536-b31a-8b0c0b08790c}</MetaDataID>
        bool onLagTest = false;
        /// <summary>
        /// Checks periodicaly the websocket response time
        /// </summary>
        /// <param name="state"></param>
        /// <MetaDataID>{b77ad430-e2a0-426c-aff1-a389d240119b}</MetaDataID>
        void OnWebSocketResponseTimeCheckTick(object state)
        {

            try
            {
                if (onLagTest)
                    return;

                onLagTest = true;
                var endPoint = EndPoint;

#if DEBUG
#if DeviceDotNet
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    var socketState = (endPoint as WebSocketClient)?.State;

                    if (socketState!=null)
                        System.Diagnostics.Debug.WriteLine(socketState.ToString());

                    DateTime? offLineDatetime = OffLineDatetime;
                    if (offLineDatetime!=null&&(DateTime.Now-offLineDatetime.Value).TotalSeconds>20&&endPoint is WebSocketClient)
                    {
                        OffLineDatetime=null;
                        WebSocketReconnect();
                    }


                }
#endif
#endif

                if (endPoint != null &&
                    endPoint is WebSocketClient &&
                        (endPoint as WebSocketClient).State == WebSocketState.Open)
                {
                    RequestData requestData = new RequestData();
                    requestData.RequestType = RequestType.LagTest;
                    requestData.ChannelUri = ChannelUri;
                    DateTime dateTime = DateTime.Now;
                    requestData.SendTimeout = 5000;
                    var task = endPoint.SendRequestAsync(requestData);
                    task.Wait(5000);
                    var timeSpan = DateTime.Now - dateTime;

                    System.Diagnostics.Debug.WriteLine("timeout for " + endPoint.GetHashCode().ToString() + " : " + ChannelUri + "  " + timeSpan.ToString());
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "timeout for " + endPoint.GetHashCode().ToString() + " : " + ChannelUri + "  " + timeSpan.ToString() });
#endif

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
        #endregion

#endif

        /// <MetaDataID>{0a5af218-8654-4a68-a243-7503eb5b7f8b}</MetaDataID>
        bool DirectConnectionCheck;

        /// <MetaDataID>{ce70f914-1f4d-4dff-8830-b654eaa7c9c5}</MetaDataID>
        int DirectConnectionTries;
        /// <MetaDataID>{601f0695-33c1-48e0-9e2c-55654b56f4a5}</MetaDataID>
        List<WebSocketClient> PendingClientSocketsToClose = new List<WebSocketClient>();

        /// <MetaDataID>{4a724d9e-8273-46ec-b041-58e9d4551c64}</MetaDataID>
        private void TryDirectConnectionTimerTick(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                if (DirectConnectionCheck)
                    return;
            }
            Task.Run(() =>
             {
                 try
                 {
                     lock (this)
                     {
                         if (DirectConnectionCheck)
                             return;
                         DirectConnectionCheck = true;
                     }


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
                     Task<ResponseData> task = null;
                     RequestData requestData = null;
                     #region Check current socket for direct connection


                     var currentWebSocketClient = WebSocketClient;
                     if (currentWebSocketClient?.State==WebSocketState.Open)
                     {
                         requestData = new RequestData();
                         requestData.RequestType = RequestType.ConnectionInfo;
                         requestData.ChannelUri = ChannelUri;
                         requestData.SendTimeout = Binding.DefaultBinding.SendTimeout.TotalMilliseconds;
                         task = currentWebSocketClient.SendRequestAsync(requestData);
                         if (task.Wait(Binding.DefaultBinding.SendTimeout))
                         {
                             var responseData = task.Result; ;
                             if (responseData.DirectConnect)
                             {
                                 DirectConnectionTimer.Stop();
                                 return;
                             }
                         }


                     }
                     #endregion

                     var tryDirectConnectionSocketClient = WebSocketClient.OpenNewConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, Binding.DefaultBinding);
                     if (tryDirectConnectionSocketClient == null)
                         return;


                     requestData = new RequestData();
                     requestData.RequestType = RequestType.ConnectionInfo;
                     requestData.ChannelUri = ChannelUri;
                     requestData.SendTimeout = Binding.DefaultBinding.SendTimeout.TotalMilliseconds;
                     task = tryDirectConnectionSocketClient.SendRequestAsync(requestData);
                     if (task.Wait(Binding.DefaultBinding.SendTimeout))
                     {

                         var responseData = task.Result; ;
                         if (responseData.DirectConnect)
                         {

                             WebSocketClient = tryDirectConnectionSocketClient;
                             WebSocketClient.ReplaceConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, tryDirectConnectionSocketClient);
                             DirectConnectionTimer.Stop();
                         }
                         else
                         {
                             DirectConnectionTries++;
                             tryDirectConnectionSocketClient.CloseAsync();
                         }
                     }
                     else
                     {
                         tryDirectConnectionSocketClient.RejectRequest(task);
                     }


                 }
                 finally
                 {
                     lock (this)
                         DirectConnectionCheck = false;
                 }
             });

        }

        /// <MetaDataID>{94e7c48b-b97d-4919-a674-f6aa926263ad}</MetaDataID>
        public WebSocketChannel(IEndPoint webSocketEndPoint)
        {

            EndPoint = webSocketEndPoint;
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(webSocketEndPoint as MarshalByRefObject))
            {

            }
#if DEBUG
            this.WebSocketResponseTimeCheckTimer = new System.Threading.Timer(new System.Threading.TimerCallback(OnWebSocketResponseTimeCheckTick), null, 20000, 3000);
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

        object ReplaceWebSocketClientLock = new object();
        Task ReplaceWebSocketClientLockTask;

        static int set_WebSocketClientCount;

        ///// <MetaDataID>{e8caaa80-4c5e-40fe-837f-794da0159790}</MetaDataID>
        //bool SusspendWebSocketClientChange;

        /// <exclude>Excluded</exclude>
        WebSocketClient _WebSocketClient;

        [Association("", Roles.RoleA, "25f6cb73-2c3a-4cb1-b575-56521957754d")]
        [RoleAMultiplicityRange(1, 1)]
        public WebSocketClient WebSocketClient
        {
            get
            {
                lock (ReplaceWebSocketClientLock)
                    return _WebSocketClient;
            }
            set
            {
                Task replaceWebSocketClientLockTask = null;
                lock (ReplaceWebSocketClientLock)
                    replaceWebSocketClientLockTask= ReplaceWebSocketClientLockTask;
                if (replaceWebSocketClientLockTask!=null)
                    if (!replaceWebSocketClientLockTask.Wait(10000))
                        throw new TimeoutException(); // or compensate

                try
                {

                    set_WebSocketClientCount++;
#if DeviceDotNet
                    DeviceApplication.Current.Log(new List<string>() { "WebSocketClient  in :"+ set_WebSocketClientCount });
#endif

                    try
                    {
                        if (_WebSocketClient != null)
                        {
                            if (_WebSocketClient != value)
                            {

                                lock (ReplaceWebSocketClientLock)
                                    ReplaceWebSocketClientLockTask=ReplaceWebSocketClient(value);

                                if (!ReplaceWebSocketClientLockTask.Wait(Binding.DefaultBinding.OpenTimeout+Binding.DefaultBinding.SendTimeout))
                                {

#if DeviceDotNet
                                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "ReplaceWebSocketClient  time out in second :"+ (Binding.DefaultBinding.OpenTimeout+Binding.DefaultBinding.SendTimeout).TotalSeconds.ToString() });
#endif
                                    lock (ReplaceWebSocketClientLock)
                                    {
                                        if (_WebSocketClient ==value)
                                        {
                                            _EndPoint = null;
                                            _WebSocketClient.Closed -= WebSocketClient_Closed;
                                            _WebSocketClient.RemoveWebSocketChannel(this);
                                            _WebSocketClient=null;
                                        }

                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            lock (ReplaceWebSocketClientLock)
                            {
                                _WebSocketClient = value;
                                _EndPoint = WebSocketClient;
                                if (_WebSocketClient!=null)
                                {
                                    _WebSocketClient.Closed += WebSocketClient_Closed;
                                    _WebSocketClient.AddWebSocketChannel(this);
                                }
                            }
                        }
                    }
                    finally
                    {
#if DeviceDotNet
                        OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "WebSocketClient  exit :"+ set_WebSocketClientCount });
#endif
                    }                                   // work here...
                }
                finally
                {


                }

            }
        }

        private Task ReplaceWebSocketClient(WebSocketClient webSocket)
        {

            return Task.Run(() =>
            {
                try
                {
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "ReplaceWebSocketClient  in :"+ set_WebSocketClientCount });
                    if(_WebSocketClient==null)
                    {
                        OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "WebSocketClient == null"+ set_WebSocketClientCount });
                    }
                    
#endif

                    var clossingWebSocketClient = _WebSocketClient;
                    clossingWebSocketClient.Closed -= WebSocketClient_Closed;
                    lock (ReplaceWebSocketClientLock)
                    {
                        _WebSocketClient=webSocket;
                        if (_WebSocketClient!=null)
                        {
                            _WebSocketClient.Closed += WebSocketClient_Closed;
                            _WebSocketClient.AddWebSocketChannel(this);
                            _EndPoint = WebSocketClient;
                        }
                    }

                    var datetime = DateTime.Now;
                    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                    System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel replace WebSocketClient  {0} ", timestamp));

#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "RestApp channel replace WebSocketClient :"+ set_WebSocketClientCount });
#endif


                    if (clossingWebSocketClient.State == WebSocketState.Open)
                        DropPhysicalConnection(clossingWebSocketClient);
                    else
                    {
                    }
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "CloseWebSocket :"+ set_WebSocketClientCount });
#endif
                    CloseWebSocket(clossingWebSocketClient);

                    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_EndPoint as MarshalByRefObject))
                    {
                    }
                    if (_WebSocketClient!=null)
                    {
#if DeviceDotNet
                        OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "ClientSessionPart.Reconnect :"+ set_WebSocketClientCount });
#endif


                        ClientSessionPart.Reconnect(false, webSocket);
                    }
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "ReplaceWebSocketClient  out :"+ set_WebSocketClientCount });
#endif
                }
                catch (Exception error)
                {
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "ReplaceWebSocketClient  error :"+ error.Message, error.StackTrace });
#endif
                    throw;
                }
            });
        }

        /// <summary>
        /// Close web socket when used exclusively from this channel.
        ///  
        /// </summary>
        /// <MetaDataID>{c246bfd9-e649-43e5-9c9d-15107cfcc9ff}</MetaDataID>
        private void CloseWebSocket(WebSocketClient webSocketClient)
        {


            if (webSocketClient.WebSocketChannels.Where(x => x != this).Count() == 0 && webSocketClient.State == WebSocketState.Open)
            {
                if (webSocketClient.PendingRequest > 0)
                {
                    //Close web socket when all pending requests ended. 
                    PendingClientSocketsToClose.Add(webSocketClient);
                    CloseConnectionTimer.Start();
                }
                else
                {
                    webSocketClient.RemoveWebSocketChannel(this);
                    webSocketClient.CloseAsync();
                    //#if !DeviceDotNet
                    //                    System.Threading.Thread.Sleep(1000);
                    //#else
                    //                    System.Threading.Tasks.Task.Delay(1000).Wait();
                    //#endif
                }
            }
            else
                webSocketClient.RemoveWebSocketChannel(this);

        }
        Task WebsocketReconnectionTask;

        /// <MetaDataID>{df0db290-5ec8-4352-bfed-a48351ce44af}</MetaDataID>
        private void WebSocketClient_Closed(object sender, EventArgs e)
        {
#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "WebSocketClient_Closed : " });
#endif

            if (Sleeping)
            {
                WebSocketClient=null;

            }
            WebSocketClient closedWebSocketClient = (WebSocketClient)sender;
            WebSocketReconnect();
        }

        private void WebSocketReconnect()
        {

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "WebSocketReconnect : "+DateTime.Now.ToString()+" - " +ChannelUri });

#endif
            //if (!SusspendWebSocketClientChange)
            {
                if (WebsocketReconnectionTask!=null&&WebsocketReconnectionTask.Status==TaskStatus.Running)
                    return;
                WebsocketReconnectionTask= Task.Run(() =>
                {
                    int count = 0;
                    while (true)
                    {


#if DeviceDotNet
                        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                        {
                            System.Threading.Thread.Sleep(100);
                            continue;
                        }
#endif
                        count++;
                        if (count>20)
                        {
                            count=0;
                            System.Diagnostics.Debug.WriteLine("WebsocketReconnectionTask");
                        }

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
                                //ClientSessionPart.Reconnect(true);
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

        IEndPoint _EndPoint;
        /// <MetaDataID>{624c1f21-7502-42ce-b896-ad111b5d36a4}</MetaDataID>
        public IEndPoint EndPoint
        {
            get
            {

                Task replaceWebSocketClientLockTask = null;
                lock (ReplaceWebSocketClientLock)
                    replaceWebSocketClientLockTask= ReplaceWebSocketClientLockTask;
                if (replaceWebSocketClientLockTask!=null)
                    if (!replaceWebSocketClientLockTask.Wait(10000))
                        throw new TimeoutException(); // or compensate
                return _EndPoint;                                    // work here...


                //lock (this)
                //{
                //    return WebSocketEndPoint;
                //}
            }

            set
            {
                _EndPoint = value;
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_EndPoint as MarshalByRefObject))
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
            requestData.SendTimeout = Binding.DefaultBinding.SendTimeout.TotalMilliseconds;
            var task = webSocket.SendRequestAsync(requestData);
            //task.Wait(Binding.DefaultBinding.SendTimeout);
            if (!task.Wait(System.TimeSpan.FromSeconds(5)))
            {
            }
        }


        ///// <MetaDataID>{c30cb6c5-12be-49c3-8a19-07f1a2282082}</MetaDataID>
        //internal IEndPoint WebSocketEndPoint;

        /// <MetaDataID>{89c008a5-539c-4a94-b983-49f0ff9d09ff}</MetaDataID>
        public Task<ResponseData> AsyncProcessRequest(RequestData requestData)
        {
            if (EndPoint != null)
            {
#if DeviceDotNet
                var task = EndPoint.SendRequestAsync(requestData);
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
                var task = _EndPoint.SendRequestAsync(requestData);
                return task;
                #endregion
            }
        }


        /// <MetaDataID>{7f6c27b0-9b61-46eb-b114-6c2330c4ab6b}</MetaDataID>
        bool IOpen
        {
            get
            {
                return true;
            }
        }
        /// <MetaDataID>{31eafd3a-021e-4c4d-b5ed-f10e1d63b4aa}</MetaDataID>
        bool ConnectionIsOpen
        {
            get
            {
                if (EndPoint == null)
                    return false;
                else
                    return EndPoint.ConnectionIsOpen;
            }

        }
        /// <MetaDataID>{cfb7fd00-d3cb-4edb-88c0-70b0cc072829}</MetaDataID>
        public ResponseData ProcessRequest(RequestData requestData)
        {


            //EnsureChannelIsOpen();
            Binding binding = CallContext.LogicalGetData("Binding") as Binding;
            if (binding == null)
                binding = Binding.DefaultBinding;
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

                if (endPoint is WebSocketClient)
                {
                    if ((endPoint as WebSocketClient).State!=WebSocketState.Open&&(endPoint as WebSocketClient).State!=WebSocketState.Connecting)
                    {
                        if (WebsocketReconnectionTask!=null)
                        {
                            if (!WebsocketReconnectionTask.Wait(binding.SendTimeout))
                                throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                        }
                        else
                        {
#if DeviceDotNet
                            OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "ProcessRequest : " });
#endif
                            WebSocketReconnect();
                            if (WebsocketReconnectionTask!=null)
                            {
                                if (!WebsocketReconnectionTask.Wait(binding.SendTimeout))
                                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                            }
                        }
                        endPoint = EndPoint;
                    }
                }
                var task = endPoint.SendRequestAsync(requestData);


#if DEBUG

                TimeSpan debugSendTimeout = System.TimeSpan.FromSeconds(5);
                TimeSpan sendTimeout = binding.SendTimeout - debugSendTimeout;
                if (sendTimeout.TotalSeconds < 0)
                    debugSendTimeout = binding.SendTimeout;
                if (!task.Wait(debugSendTimeout))
                {
                    if (sendTimeout.TotalSeconds > 0)
                    {
                        if (!task.Wait(sendTimeout))
                        {
                            endPoint.RejectRequest(task);
                            throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                        }
                    }
                    else
                    {
                        endPoint.RejectRequest(task);
                        throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                    }
                }
#else

                if (!task.Wait(binding.SendTimeout))
                {
                    endPoint.RejectRequest(task);
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                }
#endif
                var responseData = task.Result;
                if (!responseData.DirectConnect)
                {
                    lock (this)
                    {
                        if (endPoint==WebSocketClient)
                        {
                            if (!DirectConnectionCheck)
                            {
                                if (!DirectConnectionTimer.Enabled)
                                    DirectConnectionTimer.Start();

                            }
                        }
                        else
                        {

                        }
                    }
                }

                //task.Wait();
                return responseData;
            }
            else
            {



                #region Uses web socket request channel


#if DeviceDotNet
                OOAdvantech.DeviceApplication.Current.Log(new Collections.Generic.List<string> { "endpoint = null : " });
#endif
                WebSocketReconnect();
                if (WebsocketReconnectionTask!=null)
                {
                    if (!WebsocketReconnectionTask.Wait(binding.SendTimeout))
                        throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                }
                endPoint = EndPoint;

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
        /// <summary>
        /// Inform client that physical connection has been dropped.
        /// this happened because computing context which serve client has been moved from computing resources allocation mechanism.
        /// </summary>
        public void PhysicalConnectionDropped()
        {
            ClientSessionPart.Reconnect(true, EndPoint);

            //string publicChannelUri = null;
            //string internalchannelUri = null;
            //ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);


            ////Tries to connect with server side in new location again
            //var tryDirectConnectionSocketClient = WebSocketClient.OpenNewConnection(publicChannelUri + "WebSocketMessages", internalchannelUri, Binding.DefaultBinding);

            //if (tryDirectConnectionSocketClient == null)
            //{
            //    lock (this)
            //    {
            //        if (!DirectConnectionCheck)
            //        {
            //            if (!DirectConnectionTimer.Enabled)
            //                DirectConnectionTimer.Start();
            //        }
            //    }
            //}
            //else
            //    WebSocketClient = tryDirectConnectionSocketClient;

        }
    }
}
