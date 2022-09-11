using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OOAdvantech.Remoting.RestApi
{
#if !DeviceDotNet
    using System.Runtime.Remoting.Lifetime;
    using SuperSocket.ClientEngine;
#endif
#if DeviceDotNet
    using WebSocket4Net;
#endif
    using Json;
    using MetaDataRepository;
    using WebSocketSendAsync = Func<ArraySegment<byte>, int, bool, CancellationToken, Task>;
    using WebSocketCloseAsync = Func<
                                 int, // closeStatus
                                 string, // closeDescription
                                 System.Threading.CancellationToken, // cancel
                                 System.Threading.Tasks.Task>;
    using System.Net.WebSockets;
    using System.Runtime.Remoting.Messaging;



    /// <MetaDataID>{5bb6c37b-309c-4370-8d54-45dbe68837ec}</MetaDataID>
    public enum WebSocketState
    {
        None = -1,
        Connecting = 0,
        Open = 1,
        Closing = 2,
        Closed = 3
    }

    /// <MetaDataID>{7b595fa4-8555-4010-96f3-85fe8b105b86}</MetaDataID>
    public class WebSocketClient : IEndPoint
    {




        /// <MetaDataID>{1c0b7211-0b21-4477-a31f-13f65adc2c45}</MetaDataID>
        public bool? DirectConnect;

        public event EventHandler Closed;
        /// <MetaDataID>{fe757fd8-65a5-4f3b-aab6-811d75361987}</MetaDataID>
        public void AddWebSocketChannel(WebSocketChannel webSocketChannel)
        {
            if (!WebSocketChannels.Contains(webSocketChannel))
                WebSocketChannels.Add(webSocketChannel);
        }

        /// <MetaDataID>{48e6cd4c-a342-43b2-95af-f73716aca778}</MetaDataID>
        public void RemoveWebSocketChannel(WebSocketChannel webSocketChannel)
        {
            WebSocketChannels.Remove(webSocketChannel);
        }

        /// <MetaDataID>{9080b9cc-f1b1-4297-bd08-e830e3779676}</MetaDataID>
        [RoleBMultiplicityRange(1)]
        public List<WebSocketChannel> WebSocketChannels = new List<RestApi.WebSocketChannel>();


        /// <exclude>Excluded</exclude>
        Collections.Generic.Set<InterConnection> _PublicWebSockets = new Collections.Generic.Set<InterConnection>();

        /// <MetaDataID>{06cb2f23-958d-4a0a-884c-f17517c3ad28}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        [Association("WebSocketsInterconnection", Roles.RoleB, "6ec21518-d79c-4713-bcf2-7c1fd575db1c")]
        [AssociationClass(typeof(InterConnection))]
        [ImplementationMember(nameof(_PublicWebSockets))]

        public IList<InterConnection> PublicWebSockets
        {
            get
            {
                return _PublicWebSockets.AsReadOnly();
            }
        }

        /// <MetaDataID>{20b23e94-3bf3-4482-8173-3f7125fbf5d0}</MetaDataID>
        public void AddLogicalConnection(InterConnection logicalConnection)
        {
            lock (_PublicWebSockets)
            {
                _PublicWebSockets.Add(logicalConnection);
            }
        }

        /// <MetaDataID>{7680e65e-0554-41e9-b3bd-6f21b49b8ab3}</MetaDataID>
        public void RemoveLogicalConnection(InterConnection logicalConnection)
        {
            lock (_PublicWebSockets)
            {
                _PublicWebSockets.Remove(logicalConnection);

                lock (WebSocketClient.WebSocketConnections)
                {
                    if (_PublicWebSockets.Count == 0 && this.WebSocketChannels.Count == 0)
                        CloseAsync();

                }

            }



            //Disconnected


            //logicalConnection.Internal.SE
            //logicalConnection.Internal

        }

        /// <MetaDataID>{8d86c173-1d7c-45cd-9180-5e88f65124a0}</MetaDataID>
        public static System.Collections.Generic.Dictionary<string, OOAdvantech.Remoting.RestApi.WebSocketClient> WebSocketConnections = new Dictionary<string, WebSocketClient>();


#if PORTABLE

        Websockets.IWebSocketConnection NativeWebSocket;
#else
        /// <MetaDataID>{65a517ed-7546-4df2-a7f8-3e60f9fbadae}</MetaDataID>
        WebSocket4Net.WebSocket NativeWebSocket;
#endif
        /// <MetaDataID>{89208d6a-c91e-4416-85e4-851125419f3a}</MetaDataID>
        Guid WebSocketClientID = Guid.NewGuid();
        /// <MetaDataID>{74c60b86-269e-412c-af85-b3425ad615b8}</MetaDataID>
        public readonly Binding Binding;
        /// <MetaDataID>{8c3ad36f-d564-4729-b0a5-8385d1d92e91}</MetaDataID>
        string Uri;
        /// <MetaDataID>{4ae3e486-c4e1-4aaf-9016-4e09ad92061c}</MetaDataID>
        public WebSocketClient(string uri, Binding binding)
        {

            Binding = binding;
            Uri = uri;
#if PORTABLE
            NativeWebSocket = Websockets.WebSocketFactory.Create();
            NativeWebSocket.OnOpened += NativeWebSocket_OnOpened;
            NativeWebSocket.OnClosed += NativeWebSocket_OnClosed;
            NativeWebSocket.OnError += NativeWebSocket_OnError;
            NativeWebSocket.OnMessage += NativeWebSocket_OnMessage;

#else
            NativeWebSocket = new WebSocket4Net.WebSocket(uri);
            NativeWebSocket.Opened += OnOpened;
            NativeWebSocket.Closed += OnClosed;
            NativeWebSocket.Error += OnError;
            NativeWebSocket.MessageReceived += OnMessageReceived;
#endif
        }



        /// <MetaDataID>{6f76b746-62ea-4c15-945f-822e61dea47d}</MetaDataID>
        public int PendingRequest
        {
            get
            {
                lock (RequestTasks)
                {
                    return RequestTasks.Count;
                }
            }
        }
        /// <MetaDataID>{2dcefd59-6e10-4646-aaf6-26a73e37f06b}</MetaDataID>
        public void RejectRequest(Task<ResponseData> task)
        {
            lock (RequestTasks)
            {
                int requestID = (from taskSourceEntry in RequestTasks
                                 where taskSourceEntry.Value.Task == task
                                 select taskSourceEntry.Key).FirstOrDefault();
                if (requestID != 0)
                    RequestTasks.Remove(requestID);
            }

        }

        /// <MetaDataID>{1d125f72-482f-49fc-9db7-e5bf4ebe3f12}</MetaDataID>
        private void MessageDispatch(string messageData)
        {

            string headerStr = "" + messageData[0];
            MessageHeader header = (MessageHeader)int.Parse(headerStr);
            messageData = messageData.Substring(1);

            if (header == MessageHeader.Request)
            {
                RequestData request = OOAdvantech.Json.JsonConvert.DeserializeObject<RequestData>(messageData);
                if (request.RequestType == RequestType.Event)
                {
                    var clientSessionPart = RenewalManager.GetSession(request.SessionIdentity);
                    if (clientSessionPart != null)
                    {
                        #region publish event locally


                        var eventCallbackMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<EventCallbackMessage>(request.details);
                        eventCallbackMessage.UnMarshal();
                        clientSessionPart.EventCallback(eventCallbackMessage.EventPublisherUri.Uri, eventCallbackMessage.EventInfoData.EventInfo, eventCallbackMessage.Args.ToList());

                        ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                        responseMessage.Web = eventCallbackMessage.Web;
                        responseMessage.ChannelUri = request.ChannelUri;
                        responseMessage.InternalChannelUri = request.InternalChannelUri;
                        responseMessage.Marshal();

                        ResponseData responseData = new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, ChannelUri = request.ChannelUri, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                        responseData.CallContextID = request.CallContextID;
                        SendResponce(responseData);

                        #endregion
                    }
                    else
                    {
                        #region propagate event message on logical connection

                        var logicalConnection = (from interConnection in _PublicWebSockets
                                                 where interConnection.SessionIdentity == request.SessionIdentity
                                                 select interConnection).FirstOrDefault();
                        RequestData forwordRequest = new RequestData() { SessionIdentity = request.SessionIdentity, ChannelUri = request.ChannelUri, details = request.details, RequestType = request.RequestType };

                        ResponseData responseData = null;
                        if (logicalConnection != null && logicalConnection.Public != null && logicalConnection.Public.State == WebSocketState.Open)
                        {
                            var task = logicalConnection.Public.SendRequestAsync(forwordRequest);

                            if (!task.Wait(System.TimeSpan.FromSeconds(25)) && !task.Wait(Binding.SendTimeout))
                            {
                                ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                                responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout)));
                                responseData = new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };

                            }
                            else
                                responseData = task.Result;
                        }
                        else
                        {

                            #region Error response

                            ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                            responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, new System.Net.WebSockets.WebSocketException(System.Net.WebSockets.WebSocketError.ConnectionClosedPrematurely, "Connection was terminated unexpectedly"));
                            responseData = new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = forwordRequest.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                            #endregion
                        }


                        #region propagate message
                        string roleInstanceServerUrl = RemotingServices.InternalEndPointResolver.GetRoleInstanceServerUrl(responseData);
                        ResponseData forwordResponse = new ResponseData(request.ChannelUri) { IsSucceeded = responseData.IsSucceeded, CallContextID = request.CallContextID, SessionIdentity = responseData.SessionIdentity, ChannelUri = responseData.ChannelUri, details = responseData.details, UpdateCaching = responseData.UpdateCaching };
                        SendResponce(forwordResponse);
                        #endregion


                        #endregion
                    }
                    return;
                }
                if (request.RequestType == RequestType.Disconnect)
                {
                    var clientSessionPart = RenewalManager.GetSession(request.SessionIdentity) as ClientSessionPart;
                    if (clientSessionPart != null)
                        clientSessionPart.Channel.DropPhysicalConnection();
                }
            }
            else if (header == MessageHeader.Response)
            {
                ResponseData responseData = OOAdvantech.Json.JsonConvert.DeserializeObject<ResponseData>(messageData);

                DirectConnect = responseData.DirectConnect;

                #region Deleted code
                //if (responseData.CallContextID == -1)
                //{
                //    //var clientSessionPart = RenewalManager.GetSession(responseData.ChannelUri, null);
                //    var clientSessionPart = RenewalManager.GetSession(responseData.SessionIdentity);
                //    if (clientSessionPart != null)
                //    {
                //        Task.Run(() =>
                //        {
                //            var eventCallbackMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<EventCallbackMessage>(responseData.details);
                //            eventCallbackMessage.UnMarshal();
                //            clientSessionPart.EventCallback(eventCallbackMessage.EventPublisherUri.TransientUri, eventCallbackMessage.EventInfoData.EventInfo, eventCallbackMessage.Args.ToList());
                //        });
                //    }
                //    else
                //    {
                //        var logicalConnection = (from interConnection in _PublicWebSockets
                //                                 where interConnection.SessionIdentity == responseData.SessionIdentity
                //                                 select interConnection).FirstOrDefault();
                //        logicalConnection.Public.SendResponceAsync(responseData);
                //    }
                //    return;
                //}
                #endregion

                System.Threading.Tasks.TaskCompletionSource<OOAdvantech.Remoting.RestApi.ResponseData> requestTask = null;
                lock (RequestTasks)
                {

                    if (!this.RequestTasks.ContainsKey(responseData.CallContextID))
                    {
                    }


                    if (!this.RequestTasks.TryGetValue(responseData.CallContextID, out requestTask))
                    {

                    }


                    this.RequestTasks.Remove(responseData.CallContextID);
                }
                if (requestTask != null)
                    requestTask.SetResult(responseData);
            }
        }

        /// <MetaDataID>{b78e2496-e371-4849-89d8-9c6ddd20131e}</MetaDataID>
        public string LastError;

#if PORTABLE
        private void NativeWebSocket_OnOpened()
        {
            _State = WebSocketState.Open;
            OnInternalOpened();
        }

        private void NativeWebSocket_OnMessage(string messageData)
        {
            MessageDispatch(messageData);
        }


        private void NativeWebSocket_OnError(string obj)
        {
            LastError = obj;
            OnInternalError();
        }

        private void NativeWebSocket_OnClosed()
        {
            _State = WebSocketState.Closed;
            OnInternalClosed();
        }

#else
        /// <MetaDataID>{aecaf65b-f30c-4840-9e9d-f99753b8bea8}</MetaDataID>
        private void OnMessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Message))
            {

            }
            Task.Run(() =>
            {
                m_MessageRecieveTaskSrc?.SetResult(e.Message);
                m_MessageRecieveTaskSrc = null;
                string messageData = e.Message;
                MessageDispatch(e.Message);
            });
        }


        /// <MetaDataID>{b6adc4fe-67f8-4e5d-b675-014412680cfa}</MetaDataID>
        private void OnError(object sender, ErrorEventArgs e)
        {
            SocketException = e.Exception;
            OnInternalError();
        }

        /// <MetaDataID>{eb1ec07b-1a34-4321-81a3-7055f4ce736a}</MetaDataID>
        private void OnClosed(object sender, System.EventArgs e)
        {
            OnInternalClosed();
        }

        /// <MetaDataID>{5c50df1e-7a0a-4a0d-bc92-7988108b3ab5}</MetaDataID>
        private void OnOpened(object sender, System.EventArgs e)
        {
            OnInternalOpened();
        }

#endif
        /// <MetaDataID>{37291073-b67a-43cb-a87e-cb61622a8fce}</MetaDataID>
        public Exception SocketException;

        ////
        //// Summary:
        ////     Defines the different states a WebSockets instance can be in.
        //public enum WebSocketState
        //{
        //    //
        //    // Summary:
        //    //     Reserved for future use.
        //    None = 0,
        //    //
        //    // Summary:
        //    //     The connection is negotiating the handshake with the remote endpoint.
        //    Connecting = 1,
        //    //
        //    // Summary:
        //    //     The initial state after the HTTP handshake has been completed.
        //    Open = 2,
        //    //
        //    // Summary:
        //    //     A close message was sent to the remote endpoint.
        //    CloseSent = 3,
        //    //
        //    // Summary:
        //    //     A close message was received from the remote endpoint.
        //    CloseReceived = 4,
        //    //
        //    // Summary:
        //    //     Indicates the WebSocket close handshake completed gracefully.
        //    Closed = 5,
        //    //
        //    // Summary:
        //    //     Reserved for future use.
        //    Aborted = 6
        //}


        /// <exclude>Excluded</exclude>
        WebSocketState _State;

        /// <MetaDataID>{61bd9e56-bde1-4e2a-8de3-053eb5ed2f4a}</MetaDataID>
        public WebSocketState State
        {
            get
            {

#if PORTABLE
                return _State;
#else
#if DeviceDotNet
                switch (NativeWebSocket.State)
                {
                    case System.Net.WebSockets.WebSocketState.Open:
                        {
                            return WebSocketState.Open;
                        }
                    case System.Net.WebSockets.WebSocketState.None:
                        {
                            return WebSocketState.None;
                        }
                    case System.Net.WebSockets.WebSocketState.Closed:
                        {
                            return WebSocketState.Closed;
                        }
                    case System.Net.WebSockets.WebSocketState.CloseSent:
                        {
                            return WebSocketState.Closing;
                        }
                    case System.Net.WebSockets.WebSocketState.Connecting:
                        {
                            return WebSocketState.Connecting;
                        }
                    default:
                        return WebSocketState.None;
                }

#else
                switch (NativeWebSocket.State)
                {
                    case WebSocket4Net.WebSocketState.Open:
                        {
                            return WebSocketState.Open;
                        }
                    case WebSocket4Net.WebSocketState.None:
                        {
                            return WebSocketState.None;
                        }
                    case WebSocket4Net.WebSocketState.Closed:
                        {
                            return WebSocketState.Closed;
                        }
                    case WebSocket4Net.WebSocketState.Closing:
                        {
                            return WebSocketState.Closing;
                        }
                    case WebSocket4Net.WebSocketState.Connecting:
                        {
                            return WebSocketState.Connecting;
                        }
                    default:
                        return WebSocketState.None;
                }
#endif

#endif
            }
        }
        /// <MetaDataID>{c7d67a57-65e9-45f3-9f6b-adfc3b1c4b2e}</MetaDataID>
        private System.Threading.Tasks.TaskCompletionSource<bool> m_OpenTaskSrc;
        /// <MetaDataID>{c20764b3-b586-4716-a9fa-dc43d94c8eb9}</MetaDataID>
        private System.Threading.Tasks.TaskCompletionSource<bool> m_CloseTaskSrc;
        /// <MetaDataID>{030e8cfc-2fbb-457e-8052-6427b3688b38}</MetaDataID>
        private System.Threading.Tasks.TaskCompletionSource<string> m_MessageRecieveTaskSrc;

        /// <MetaDataID>{5969f90e-534b-47c4-a500-77e932dcb533}</MetaDataID>
        public Task<bool> OpenAsync()
        {
            System.Threading.Tasks.TaskCompletionSource<bool> openTaskSrc = null;


            lock (this)
            {
                openTaskSrc = m_OpenTaskSrc;
                if (openTaskSrc != null)
                    return openTaskSrc.Task;
                openTaskSrc = m_OpenTaskSrc = new TaskCompletionSource<bool>();
            }


#if DeviceDotNet
            if (NativeWebSocket.State == System.Net.WebSockets.WebSocketState.Closed)
#else
            if (NativeWebSocket.State == WebSocket4Net.WebSocketState.Closed)
#endif
            {
#if PORTABLE
            NativeWebSocket.OnOpened -= NativeWebSocket_OnOpened;
            NativeWebSocket.OnClosed -= NativeWebSocket_OnClosed;
            NativeWebSocket.OnError -= NativeWebSocket_OnError;
            NativeWebSocket.OnMessage -= NativeWebSocket_OnMessage;

            NativeWebSocket = Websockets.WebSocketFactory.Create();
            NativeWebSocket.OnOpened += NativeWebSocket_OnOpened;
            NativeWebSocket.OnClosed += NativeWebSocket_OnClosed;
            NativeWebSocket.OnError += NativeWebSocket_OnError;
            NativeWebSocket.OnMessage += NativeWebSocket_OnMessage;

#else
                NativeWebSocket.Opened -= OnOpened;
                NativeWebSocket.Closed -= OnClosed;
                NativeWebSocket.Error -= OnError;
                NativeWebSocket.MessageReceived -= OnMessageReceived;
                NativeWebSocket = new WebSocket4Net.WebSocket(Uri);
                NativeWebSocket.Opened += OnOpened;
                NativeWebSocket.Closed += OnClosed;
                NativeWebSocket.Error += OnError;
                NativeWebSocket.MessageReceived += OnMessageReceived;
#endif
            }
#if PORTABLE
            NativeWebSocket.Open(this.Uri);
#else
            NativeWebSocket.Open();
#endif
            return openTaskSrc.Task;
        }

        /// <MetaDataID>{90c0e9f1-1374-4510-baf3-4ce293bee05d}</MetaDataID>
        int NextRequestID = 1;
        /// <MetaDataID>{52e89894-8243-47ed-8289-e90e6c16aed5}</MetaDataID>
        internal System.Collections.Generic.Dictionary<int, System.Threading.Tasks.TaskCompletionSource<OOAdvantech.Remoting.RestApi.ResponseData>> RequestTasks = new Dictionary<int, TaskCompletionSource<ResponseData>>();

        /// <MetaDataID>{5d5dd9c9-c27d-4ba1-bdbc-01defb500c20}</MetaDataID>
        internal System.Collections.Generic.Dictionary<int, System.Threading.Tasks.TaskCompletionSource<OOAdvantech.Remoting.RestApi.ResponseData>> GetRequestTasks()
        {
            lock (RequestTasks)
            {
                return new Dictionary<int, System.Threading.Tasks.TaskCompletionSource<OOAdvantech.Remoting.RestApi.ResponseData>>(RequestTasks);
            }
        }

        /// <MetaDataID>{60d273ba-fb0c-487c-a47c-7b4b74433cf1}</MetaDataID>
        public Task<ResponseData> SendRequestAsync(RequestData request)
        {


            Binding binding = CallContext.LogicalGetData("Binding") as Binding;
            if (binding == null)
                binding = Binding;


            TaskCompletionSource<ResponseData> taskCompletionSource;
            if (!EnsureConnection())
            {
                if (State == WebSocketState.Closed|| State == WebSocketState.None)
                    EnsureConnection(this.Uri, binding);

                if (!EnsureConnection())
                {
                    if (this.SocketException != null)
                        throw this.SocketException;
                    ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                    responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, SocketException);
                    taskCompletionSource = new TaskCompletionSource<ResponseData>();
                    taskCompletionSource.SetResult(new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) });
                    return taskCompletionSource.Task;
                }
            }

            WebSocket4Net.WebSocket nativeWebSocket;
            lock (this)
            {
                nativeWebSocket = NativeWebSocket;
                request.CallContextID = NextRequestID++;
                taskCompletionSource = new TaskCompletionSource<ResponseData>();
                lock (RequestTasks)
                {
                    RequestTasks[request.CallContextID] = taskCompletionSource;
                }
            }
            string message = Json.JsonConvert.SerializeObject(request);
            message = ((int)MessageHeader.Request).ToString() + message;
            nativeWebSocket.Send(message);
            return taskCompletionSource.Task;

        }
        /// <MetaDataID>{33ad8d1a-fd7d-4c76-ba1a-0b1df2e87dee}</MetaDataID>
        public OOAdvantech.Remoting.RestApi.ResponseData SendRequest(RequestData request)
        {
            Binding binding = CallContext.LogicalGetData("Binding") as Binding;
            if (binding == null)
                binding = this.Binding;

            var task = SendRequestAsync(request);


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
                        RejectRequest(task);
                        throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                    }
                }
                else
                {
                    RejectRequest(task);
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                }
            }
#else

                if (!task.Wait(binding.SendTimeout))
                {
                    RejectRequest(task);
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", binding.SendTimeout));
                }
#endif
            return task.Result;

        }

        /// <MetaDataID>{3c2efc79-bab5-4a16-90b0-e5fcf15f8e9b}</MetaDataID>
        private bool EnsureConnection()
        {
            try
            {
                var state = State;

                if (State == WebSocketState.Closed || State == WebSocketState.Closing)
                    return false;


                if (State == WebSocketState.Connecting)
                {
                    Task<bool> openTask = OpenAsync();
                    bool openTaskCompleted = openTask.Wait(Binding.RetryOpenTimeout);
                    if (!openTaskCompleted || !openTask.Result)
                        return false;
                }
                if (State == WebSocketState.Open)
                    return true;
                else
                    return false;
            }
            catch (Exception error)
            {

                throw;
            }
        }

        ///// <MetaDataID>{071605a8-72af-4a6b-a881-43a213a65036}</MetaDataID>
        //public void SendRequestAsync(RequestData requestData)
        //{
        //    if (!EnsureConnection() && this.SocketException != null)
        //        throw this.SocketException;

        //    lock (this)
        //    {
        //        requestData.CallContextID = -2;
        //        string message = Json.JsonConvert.SerializeObject(requestData);
        //        message = ((int)MessageHeader.Request).ToString() + message;
        //        NativeWebSocket.Send(message);
        //    }
        //}
        /// <MetaDataID>{7c2d1d19-a0f8-466a-ae1f-619355d17d4c}</MetaDataID>
        public void SendResponce(ResponseData responseData)
        {
            if (!EnsureConnection() && this.SocketException != null)
                throw this.SocketException;
            WebSocket4Net.WebSocket nativeWebSocket;

            lock (this)
            {
                nativeWebSocket = NativeWebSocket;
            }
            string responseDatajson = Json.JsonConvert.SerializeObject(responseData);
            responseDatajson = ((int)MessageHeader.Response).ToString() + responseDatajson;
            nativeWebSocket.Send(responseDatajson);

        }


        /// <MetaDataID>{5060730c-40d2-4fdd-9355-8c161a269f12}</MetaDataID>
        public Task<bool> CloseAsync()
        {
            var closeTaskSrc = m_CloseTaskSrc;

            if (State != WebSocketState.Closed&& State != WebSocketState.None)
            {
                if (closeTaskSrc != null)
                    return closeTaskSrc.Task;

                closeTaskSrc = m_CloseTaskSrc = new TaskCompletionSource<bool>();
                _State = WebSocketState.Closing;
                Task.Run(() =>
                {
                    NativeWebSocket.Close();
                });
                return closeTaskSrc.Task;
            }
            else
               return Task<bool>.FromResult(true);

     
            
        }

        /// <MetaDataID>{079af359-a860-4a56-bfa8-a3d2797c0c02}</MetaDataID>
        private void FinishOpenTask()
        {

            //WebSocketClient.WebSocketConnections[Uri.ToLower()] = this;
            m_OpenTaskSrc?.SetResult(State == WebSocketState.Open);
            m_OpenTaskSrc = null;
        }

        /// <MetaDataID>{0e1f41e1-b325-4f79-a7f6-a9c1fb6191a7}</MetaDataID>
        private void FinishCloseTask()
        {
            m_CloseTaskSrc?.SetResult(State == WebSocketState.Closed);
            m_CloseTaskSrc = null;

            foreach (var logicalConnection in (from interConnection in PublicWebSockets
                                               where interConnection.Internal == this
                                               select interConnection).ToList())
            {
                RemoveLogicalConnection(logicalConnection);
                logicalConnection.Public.RemoveInnterConnection(logicalConnection);
            }
            lock (RequestTasks)
            {
                if (RequestTasks.Count > 0)
                {
                    foreach (var requestTask in RequestTasks.Values)
                    {
                        ReturnMessage responseMessage = new ReturnMessage("");
                        var restApiException = new RestApiExceptionData();
                        if (SocketException != null)
                        {
                            restApiException.ExceptionMessage = SocketException.Message;
                            restApiException.ServerStackTrace = SocketException.StackTrace;
                        }
                        restApiException.ExceptionCode = ExceptionCode.ConnectionError;
                        responseMessage.Exception = restApiException;
                        var response = new ResponseData("") { IsSucceeded = responseMessage.Exception == null, CallContextID = -1, SessionIdentity = "", details = JsonConvert.SerializeObject(responseMessage) };
                        requestTask.SetResult(response);
                    }
                }
                RequestTasks.Clear();
            }

        }

        /// <MetaDataID>{adb2753b-0b16-416e-aa35-6858b2da3709}</MetaDataID>
        void OnInternalOpened()
        {
            FinishOpenTask();
        }

        /// <MetaDataID>{0b0a4aed-29d5-4e40-94da-aa65cd8c4a24}</MetaDataID>
        void OnInternalClosed()
        {

            FinishOpenTask();
            FinishCloseTask();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <MetaDataID>{6e7af075-3189-41b6-b904-5e62c4456ab9}</MetaDataID>
        void OnInternalError()
        {
            FinishOpenTask();
            FinishCloseTask();
        }


        /// <MetaDataID>{2e91e2c9-1f79-401c-a9ba-bee9afaf2158}</MetaDataID>
        internal static WebSocketClient OpenNewConnection(string uri, string internalChannelUri, Binding binding)
        {
            var webSocket = new WebSocketClient(uri, binding);
            Task<bool> openTask = webSocket.OpenAsync();
            openTask.Wait(binding.OpenTimeout);
            if (openTask.Result)
                return webSocket;
            else
                return null;
        }
        /// <MetaDataID>{43335e56-3f33-47e5-9446-b5cecef89b4d}</MetaDataID>
        internal static void ReplaceConnection(string uri, string internalChannelUri, WebSocketClient webSocket)
        {
            lock (WebSocketClient.WebSocketConnections)
            {
                string combinedUri = uri + internalChannelUri;
                WebSocketClient.WebSocketConnections[combinedUri.ToLower()] = webSocket;

            }
        }


        /// <MetaDataID>{68cd1e53-7bca-4f2d-877e-3a9ca348e0e5}</MetaDataID>
        internal static WebSocketClient EnsureConnection(string uri, string internalChannelUri, Binding binding)
        {


            string combinedUri = uri + internalChannelUri;
            return EnsureConnection(combinedUri, binding);
            //WebSocketClient webSocket = null;
            //lock (WebSocketClient.WebSocketConnections)
            //{
            //    if (!WebSocketClient.WebSocketConnections.TryGetValue(combinedUri.ToLower(), out webSocket))
            //    {
            //        webSocket = new WebSocketClient(uri, binding);
            //        WebSocketClient.WebSocketConnections[combinedUri.ToLower()] = webSocket;
            //    }
            //    if (webSocket.State == WebSocketState.Open)
            //    {
            //        return webSocket;

            //    }
            //    else
            //        if (webSocket.State == WebSocketState.None)
            //    {
            //        WebSocketClient.WebSocketConnections[combinedUri.ToLower()] = webSocket;
            //        Task<bool> openTask = webSocket.OpenAsync();
            //        openTask.Wait(binding.OpenTimeout);
            //        if (!openTask.Result)
            //        {
            //            webSocket.SocketException = new System.TimeoutException(string.Format("OpenTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
            //            return webSocket;
            //        }
            //    }
            //    else if (webSocket.State == WebSocketState.Connecting)
            //    {
            //        Task<bool> openTask = webSocket.OpenAsync();
            //        openTask.Wait(binding.OpenTimeout);
            //        if (!openTask.Result)
            //        {
            //            webSocket.SocketException = new System.TimeoutException(string.Format("OpenTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
            //            return webSocket;
            //        }

            //    }
            //    else
            //    {
            //        webSocket = new WebSocketClient(uri, binding);
            //        WebSocketClient.WebSocketConnections[combinedUri.ToLower()] = webSocket;
            //        Task<bool> openTask = webSocket.OpenAsync();
            //        openTask.Wait(binding.OpenTimeout);
            //        if (!openTask.Result)
            //        {
            //            webSocket.SocketException = new System.TimeoutException(string.Format("OpenTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
            //            return webSocket;
            //        }
            //    }

            //}
            //return webSocket;

        }

        /// <MetaDataID>{68ef0cbd-1597-4b07-9c41-215b85b259c4}</MetaDataID>
        internal static WebSocketClient EnsureConnection(string uri, Binding binding)
        {
            WebSocketClient webSocket = null;
            DateTime startTime = DateTime.Now;

            lock (WebSocketClient.WebSocketConnections)
            {
                if (!WebSocketClient.WebSocketConnections.TryGetValue(uri.ToLower(), out webSocket))
                {
                    webSocket = new WebSocketClient(uri, binding);
                    WebSocketClient.WebSocketConnections[uri.ToLower()] = webSocket;
                }
            }
            if (webSocket.State != WebSocketState.Open)
            {
                if (webSocket.State == WebSocketState.Connecting)
                {
                    Task<bool> openTask = webSocket.OpenAsync();
                    bool openTaskCompleted = openTask.Wait(binding.OpenTimeout);
                    if (!openTaskCompleted || !openTask.Result)
                    {
                        lock (WebSocketClient.WebSocketConnections)
                        {
                            WebSocketClient.WebSocketConnections.Remove(uri.ToLower());
                        }
                        webSocket.SocketException = new System.TimeoutException(string.Format("OpenTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                        return webSocket;
                    }
                    else
                    {
                    }
                    if (webSocket.State != WebSocketState.Open)
                    {
                    }
                }
                else
                {
                    Task<bool> openTask = null;
                    //webSocket = new WebSocketClient(uri, binding);
                    //WebSocketClient.WebSocketConnections[uri.ToLower()] = webSocket;
                    bool openTaskCompleted = false;
                    openTask = webSocket.OpenAsync();
                    do
                    {

                        openTaskCompleted = openTask.Wait(binding.RetryOpenTimeout);
#if !DeviceDotNet
                        System.Threading.Thread.Sleep(200);
                        var state = webSocket.State;
                        if (state == WebSocketState.Closed)
                            openTask = webSocket.OpenAsync();
#else
                        System.Threading.Tasks.Task.Delay(200).Wait();
                        var state = webSocket.State;
                        if (state == WebSocketState.Closed)
                            openTask = webSocket.OpenAsync();

#endif
                    }
                    while ((!openTaskCompleted || !openTask.Result) && (DateTime.Now - startTime) < binding.OpenTimeout);


                    if (!openTaskCompleted || !openTask.Result)
                    {
                        if (webSocket.State == WebSocketState.Connecting)
                            webSocket.CloseAsync();

                        lock (WebSocketClient.WebSocketConnections)
                        {
                            WebSocketClient.WebSocketConnections.Remove(uri.ToLower());
                        }
                        webSocket.SocketException = new System.TimeoutException(string.Format("OpenTimeout {0} expired", binding.OpenTimeout));
                        return webSocket;
                    }
                    else
                    {
                    }
                    var openTimeSpan = (DateTime.Now - startTime).ToString();
                    System.Diagnostics.Debug.WriteLine(openTimeSpan);
                    if (webSocket.State != WebSocketState.Open)
                    {

                    }

                }
            }



            if (webSocket.State != WebSocketState.Open)
            {

            }
            return webSocket;
        }
    }





    /// <MetaDataID>{4A959F4A-6E02-4D06-AFAC-DF2526FEE286}</MetaDataID>
    public class WebSocketServer : MarshalByRefObject, IEndPoint
    {
        /// <summary>
        /// RunContexts are all connection where the machine out of azure context connect with the 
        /// the IsolatedComputingContext machine directly.
        /// </summary>
        [Association("RunContextCommunication", Roles.RoleA, "1e5a27e4-1979-4b5d-9475-c6fe2ed808d6")]
        [AssociationClass(typeof(LocalConnection))]
        public List<LocalConnection> RunContexts = new List<LocalConnection>();




        /// <summary>
        /// Timer for net remotin life time
        /// The WebSocketServer server created in worker role application domain.
        /// The WebSocketServer used from   
        /// </summary>
        /// <MetaDataID>{7f6329cc-d74e-4476-9a37-eea76abc1fd6}</MetaDataID>
        System.Timers.Timer LifeTimeLeaseTimer = new System.Timers.Timer();





        /// <exclude>Excluded</exclude> 
        Collections.Generic.Set<InterConnection> _InterConnections = new Collections.Generic.Set<InterConnection>();

        /// <summary>
        /// Inter conections are all connection where the machine out of azure context connect with the 
        /// first available machine and all requests propagated to  IsolatedComputingContext machine
        /// </summary>
        [RoleAMultiplicityRange(0)]
        [Association("WebSocketsInterconnection", Roles.RoleA, "6ec21518-d79c-4713-bcf2-7c1fd575db1c")]
        [AssociationClass(typeof(InterConnection))]
        [ImplementationMember(nameof(_InterConnections))]
        public IList<InterConnection> InterConnections
        {
            get
            {
                lock (_InterConnections)
                {
                    return _InterConnections.ToList();
                }
            }
        }

        /// <MetaDataID>{44bb264a-e9a3-4bb9-b6de-fc201dd75f0c}</MetaDataID>
        public void AddInterConnection(InterConnection logicalConnection)
        {
            lock (_InterConnections)
            {
                _InterConnections.Add(logicalConnection);
            }
        }

        /// <MetaDataID>{d53b7a00-231c-423c-acb2-af9b1a237e3f}</MetaDataID>
        public void RemoveInnterConnection(InterConnection logicalConnection)
        {
            lock (_InterConnections)
            {
                _InterConnections.Remove(logicalConnection);
            }
        }

        //private static WebSocketCollection connections = new WebSocketCollection();
        /// <exclude>Excluded</exclude>
        private WebSocketSendAsync _sendFunc;

        /// <exclude>Excluded</exclude>
        private WebSocketCloseAsync _closeFunc;

        /// <exclude>Excluded</exclude>
        private CancellationToken _token;

        string RequestUri;
        /// <MetaDataID>{572aebb7-320d-4b8a-841e-93e7766d7023}</MetaDataID>
        public WebSocketServer(string requestUri, WebSocketSendAsync sendFunc, WebSocketCloseAsync closeFunc, CancellationToken token, IDictionary<string, object> wsEnv)
        {
            RequestUri = requestUri;
            _sendFunc = sendFunc;
            _closeFunc = closeFunc;
            _token = token;
            _State = WebSocketState.Open;

#if !DeviceDotNet
            LifeTimeLeaseTimer.Interval = TimeSpan.FromMinutes(15).TotalMilliseconds;
            LifeTimeLeaseTimer.Elapsed += LifeTimeLeaseTimerElapsed;
#endif

            WebSocketServerID = Guid.NewGuid().ToString("N");
        }

#if !DeviceDotNet

        public override object InitializeLifetimeService()
        {
            var retObject = base.InitializeLifetimeService();

            //new Action(() =>
            //{
            //    using (CultureContext cultureContext = new CultureContext(culture, useDefaultCultureValue))
            //    {
            //        _ObjectChangeState?.Invoke(this, nameof(Description));
            //    }
            //}));

            Task.Run(async delegate
            {

                await Task.Delay(1000);
                ILease lease = System.Runtime.Remoting.RemotingServices.GetLifetimeService(this) as ILease;
                if (lease != null)
                {
                    lease.Renew(System.TimeSpan.FromMinutes(17));
                    lease = System.Runtime.Remoting.RemotingServices.GetLifetimeService(this) as ILease;
                }
            });




            return retObject;
        }
#endif




#if !DeviceDotNet
        /// <MetaDataID>{04504810-a7e6-443b-b4f2-5fa704890000}</MetaDataID>
        private void LifeTimeLeaseTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ILease lease = System.Runtime.Remoting.RemotingServices.GetLifetimeService(this) as ILease;
            if (lease != null)
                lease.Renew(System.TimeSpan.FromMinutes(17));

        }
#endif
        /// <MetaDataID>{dc06c105-cb53-43ab-8814-e8a71eb49b85}</MetaDataID>
        public void OnOpen()
        {

            LifeTimeLeaseTimer.Start();
            //connections.Add(this);
        }


        /// <exclude>Excluded</exclude>
        WebSocketState _State;

        /// <MetaDataID>{61bd9e56-bde1-4e2a-8de3-053eb5ed2f4a}</MetaDataID>
        public WebSocketState State
        {
            get
            {
                return _State;
            }
        }




        /// <MetaDataID>{f8f62501-8a65-4b98-b08a-0a1dc4a4fed1}</MetaDataID>
        public void OnClose()
        {
            LifeTimeLeaseTimer.Stop();
            _State = WebSocketState.Closed;

            lock (_InterConnections)
            {


                #region Sends to all azure interconection which are intitalized from this webSocket server disconnect message.
                foreach (var interConnection in (from interConnection in InterConnections
                                                 where interConnection.Public == this
                                                 select interConnection).ToList())
                {

                    RemoveInnterConnection(interConnection);
                    interConnection.Internal.RemoveLogicalConnection(interConnection);

                    try
                    {
                        var datetime = DateTime.Now;
                        string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                        //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel on server close DropLogicalConnection  {0} : {1}  ,  {2}", timestamp, interConnection.SessionIdentity, Guid.NewGuid().ToString("N")));
                        string clientProcessIdentity = interConnection.SessionIdentity.Substring(0, interConnection.SessionIdentity.IndexOf("."));
                        var methodCallMessage = new MethodCallMessage(interConnection.ChannelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", clientProcessIdentity, "", StandardActions.Disconnected, new object[0]);
                        RequestData requestData = new RequestData();
                        requestData.RequestType = RequestType.Disconnect;
                        requestData.SessionIdentity = interConnection.SessionIdentity;
                        requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
                        requestData.RequestType = RequestType.MethodCall;
                        var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
                        requestData.ChannelUri = interConnection.ChannelUri;
                        requestData.PhysicalConnectionID = this.WebSocketServerID;
                        interConnection.Internal.SendRequestAsync(requestData);
                    }
                    catch (Exception error)
                    {
                    }
                }
                #endregion



            }
            #region Sends to all local conections disconnect message.

            lock (RunContexts)
            {
                foreach (var runContextConnection in RunContexts.ToList())
                {
                    RunContexts.Remove(runContextConnection);
#if !DeviceDotNet
                    runContextConnection.IsolatedContext.IsolatedContextUnloaded -= IsolatedContext_IsolatedContextUnloaded;

                    #region Sends request message to server session part where it has physical connection through  this web socket server
                    try
                    {
                        var datetime = DateTime.Now;
                        string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                        //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel on server close DropLogicalConnection  {0} : {1}  ,  {2}", timestamp, localConnection.SessionIdentity, Guid.NewGuid().ToString("N")));
                        string clientProcessIdentity = runContextConnection.SessionIdentity.Substring(0, runContextConnection.SessionIdentity.IndexOf("."));
                        var methodCallMessage = new MethodCallMessage(runContextConnection.ChannelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", clientProcessIdentity, "", StandardActions.Disconnected, new object[0]);
                        RequestData requestData = new RequestData();
                        requestData.RequestType = RequestType.Disconnect;
                        requestData.SessionIdentity = runContextConnection.SessionIdentity;
                        requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
                        requestData.RequestType = RequestType.MethodCall;
                        var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
                        requestData.ChannelUri = runContextConnection.ChannelUri;
                        requestData.PhysicalConnectionID = this.WebSocketServerID;
                        requestData.EventCallBackChannel = this;
                        var responseData = IsolatedContext.DispatchMessage(requestData);


                        //localConnection.Internal.SendRequestAsync(requestData);
                    }
                    catch (Exception error)
                    {
                    }
                    #endregion
#endif

                }



            }

            #endregion
        }



        /// <MetaDataID>{f9d2d7da-2331-4587-b2fa-7d7a2fb6488b}</MetaDataID>
        public void OnMessage(string message)
        {
#if DEBUG
            if (RequestUri != null && RequestUri.IndexOf(@"http://127.255") != 0)
            {

            }
#endif
            if (string.IsNullOrWhiteSpace(message))
            {

            }

            Task.Run(() =>
            {
                try
                {
                    MessageDispatch(message);
                }
                catch (Exception)
                {

                    throw;
                }
            });
        }

        /// <MetaDataID>{eea2724f-ad6c-4d0f-b466-2e2083946813}</MetaDataID>
        void SendMessage(string message)
        {
            lock (this)
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                var buffer = new ArraySegment<byte>(new byte[bytes.Length]);
                Array.Copy(bytes, 0, buffer.Array, 0, bytes.Length);
                var task = _sendFunc(new ArraySegment<byte>(buffer.Array, 0, bytes.Length), 1, true, _token);
                if (!task.Wait(TimeSpan.FromSeconds(10)))
                {
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));

                    //if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                    //{
                    //    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                    //}
                }
            }
        }

        /// <MetaDataID>{a90e746b-202f-4fe3-bde1-0a5077ed5172}</MetaDataID>
        public Task Close()
        {
            return _closeFunc((int)WebSocketCloseStatus.EndpointUnavailable, "Closing", CancellationToken.None);
        }

        /// <MetaDataID>{3c1d4da0-8c85-4315-8b02-85b75729c9c5}</MetaDataID>
        public string WebSocketServerID;

        /// <MetaDataID>{9afcde9f-6b5f-4ac3-a8db-8c1f5a62a3e6}</MetaDataID>
        Dictionary<int, RequestData> callsPair = new Dictionary<int, RequestData>();

        /// <MetaDataID>{b75d2cc3-bdb6-4e3e-9790-96c794f1d0bc}</MetaDataID>
        public void MessageDispatch(string messageData)
        {
            string headerStr = "" + messageData[0];
            MessageHeader header = (MessageHeader)int.Parse(headerStr);
            messageData = messageData.Substring(1);
            if (header == MessageHeader.Request)
            {
                RequestData request = Json.JsonConvert.DeserializeObject<RequestData>(messageData);
                if (request.RequestProcess != "WaWorkerHost")
                {

                }
                lock (callsPair)
                {
                    callsPair.Add(request.CallContextID, request);
                }
                ResponseData responseData = null;
                if (request.RequestType == RequestType.ConnectionInfo)
                {
                    //  string roleInstanceID = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;

                    responseData = new ResponseData(request.ChannelUri);
                    responseData.ChannelUri = request.ChannelUri;
                    responseData.CallContextID = request.CallContextID;
                    responseData.DirectConnect = RemotingServices.InternalEndPointResolver.CanBeResolvedLocal(request);

                    var datetime = DateTime.Now;
                    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                    System.Diagnostics.Debug.WriteLine(string.Format("RestApp {3} {0} call ID {1}  Check ConnectionInfo DirectConnect : {2}", request.ChannelUri, request.CallContextID, responseData.DirectConnect, timestamp));

                    //if(!responseData.DirectConnect)
                    //{
                    //    string id = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;
                    //}
                }
                else if (request.RequestType == RequestType.LagTest)
                {
                    responseData = new ResponseData(request.ChannelUri);
                    responseData.ChannelUri = request.ChannelUri;
                    responseData.CallContextID = request.CallContextID;


                    SendResponce(responseData);
                }
                else
                {
                    //if (request.SessionIdentity != null)
                    //    WebSocketConnections[request.SessionIdentity] = this;
                    //else
                    //{
                    //}

                    try
                    {

                        bool localResolveRequest = RemotingServices.InternalEndPointResolver.CanBeResolvedLocal(request);
                        if (string.IsNullOrWhiteSpace(request.PhysicalConnectionID))
                            request.PhysicalConnectionID = WebSocketServerID;
                        else
                            request.PhysicalConnectionID += " => " + WebSocketServerID;


                        if (localResolveRequest)
                        {
                            try
                            {
                                System.Runtime.Remoting.Messaging.CallContext.SetData("internalChannelUri", request.InternalChannelUri);
                                System.Runtime.Remoting.Messaging.CallContext.SetData("PublicChannelUri", request.PublicChannelUri);
                                try
                                {
                                    //this.clo
                                    request.EventCallBackChannel = this;
                                    responseData = IsolatedContext.DispatchMessage(request);
                                    responseData.DirectConnect = true;
#if !DeviceDotNet
                                    if (request.RequestType == RequestType.Disconnect)
                                        RemoveLocalConnection(request.InternalChannelUri, responseData.SessionIdentity);
                                    else
                                        AddLocalConnection(request.InternalChannelUri, responseData.SessionIdentity, responseData.ChannelUri);
#endif
                                }
                                finally
                                {
                                    System.Runtime.Remoting.Messaging.CallContext.SetData("internalChannelUri", null);
                                    System.Runtime.Remoting.Messaging.CallContext.SetData("PublicChannelUri", null);
                                }
                            }
                            catch (Exception error)
                            {
                                #region Error response

                                ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                                responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, error);
                                responseData = new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                                #endregion
                            }
                        }
                        else
                        {
                            #region Forward request to target computational resource
                            string roleInstanceServerUrl = RemotingServices.InternalEndPointResolver.GetRoleInstanceServerUrl(request);

                            System.Diagnostics.Debug.WriteLine(request.ChannelUri + " , " + roleInstanceServerUrl, "Channel");

                            RequestData forwordRequest = new RequestData() { SessionIdentity = request.SessionIdentity, ChannelUri = request.ChannelUri, details = request.details, RequestType = request.RequestType, PhysicalConnectionID = request.PhysicalConnectionID };

                            if (roleInstanceServerUrl.Trim().IndexOf("http://") == 0)
                                roleInstanceServerUrl = "ws://" + roleInstanceServerUrl.Substring("http://".Length);


                            WebSocketClient webSocket = WebSocketClient.EnsureConnection(roleInstanceServerUrl + "WebSocketMessages", Binding.DefaultBinding);
                            if (webSocket.State == WebSocketState.Open)
                            {
                                InterConnection logicalConnection = null;
                                if (!string.IsNullOrWhiteSpace(forwordRequest.SessionIdentity))
                                {
                                    lock (_InterConnections)
                                    {
                                        logicalConnection = (from interConnection in InterConnections
                                                             where interConnection.Internal == webSocket && interConnection.Public == this && interConnection.SessionIdentity == forwordRequest.SessionIdentity
                                                             select interConnection).FirstOrDefault();

                                    }
                                }
                                var task = webSocket.SendRequestAsync(forwordRequest);

                                if (!task.Wait(System.TimeSpan.FromSeconds(25)) && !task.Wait(webSocket.Binding.SendTimeout))
                                {
                                    webSocket.RejectRequest(task);
                                    ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                                    responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout)));
                                    responseData = new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                                    webSocket.RejectRequest(task);
                                }
                                else
                                {
                                    responseData = task.Result;
                                    responseData.ChannelUri = forwordRequest.ChannelUri;
                                    if (request.RequestType != RequestType.Disconnect && logicalConnection == null)
                                        EnsureLogicalConnection(responseData, webSocket);
                                }
                                if (request.RequestType == RequestType.Disconnect)
                                {
                                    lock (_InterConnections)
                                    {
                                        logicalConnection = (from interConnection in InterConnections
                                                             where interConnection.SessionIdentity == responseData.SessionIdentity
                                                             select interConnection).FirstOrDefault();
                                        if (logicalConnection != null)
                                        {
                                            RemoveInnterConnection(logicalConnection);
                                            logicalConnection.Internal.RemoveLogicalConnection(logicalConnection);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                #region Error response

                                ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                                responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, webSocket.SocketException);
                                responseData = new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, CallContextID = forwordRequest.CallContextID, SessionIdentity = forwordRequest.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                                #endregion
                            }
                            responseData.CallContextID = request.CallContextID;
                            responseData.DirectConnect = false;

                            //responseData.SessionIdentity = request.SessionIdentity;

                            #endregion
                        }


                    }
                    catch (Exception error)
                    {
                        ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
                        responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ConnectionError, error);
                        responseData = new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                    }
                }
                lock (callsPair)
                {
                    callsPair.Remove(responseData.CallContextID);
                    if (callsPair.Values.Count > 0)
                    {
                        var req = callsPair.Values.ToArray()[0];

                        System.Diagnostics.Debug.WriteLine(string.Format("Remain : {0} ", req.CallContextID));
                        System.Diagnostics.Debug.WriteLine(string.Format("Remain : {0} ", req.details));
                    }
                    else
                        System.Diagnostics.Debug.WriteLine("Non Remain  Calls");
                }
                SendResponce(responseData);

            }
            else if (header == MessageHeader.Response)
            {
                ResponseData responseData = Json.JsonConvert.DeserializeObject<ResponseData>(messageData);
                TaskCompletionSource<OOAdvantech.Remoting.RestApi.ResponseData> requestTask = null;
                lock (RequestTasks)
                {
                    requestTask = this.RequestTasks[responseData.CallContextID];
                    this.RequestTasks.Remove(responseData.CallContextID);
                }
                requestTask.SetResult(responseData);
            }
        }

#if !DeviceDotNet
        /// <MetaDataID>{f4f93072-247a-4ce9-8ac3-3a07fdc91c0f}</MetaDataID>
        private void AddLocalConnection(string contextID, string sessionIdentity, string channelUri)//RequestData request, ResponseData responseData)
        {
            lock (RunContexts)
            {
                #region Add local connection with run context

                var localConnection = (from interConnection in RunContexts
                                       where interConnection.SessionIdentity == sessionIdentity
                                       select interConnection).FirstOrDefault();

                if (localConnection == null)
                {
                    localConnection = new LocalConnection() { SessionIdentity = sessionIdentity, ChannelUri = channelUri };
                    localConnection.IsolatedContext = IsolatedContext.GetIsolatedContext(contextID);

                    if ((from interConnection in RunContexts
                         where interConnection.IsolatedContext == IsolatedContext.GetIsolatedContext(contextID)
                         select interConnection).FirstOrDefault() == null)
                    {
                        localConnection.IsolatedContext.IsolatedContextUnloaded += IsolatedContext_IsolatedContextUnloaded;
                    }

                    RunContexts.Add(localConnection);
                }

                #endregion
            }
        }

        /// <MetaDataID>{98431b80-aee6-4e97-adfc-fd0bd1a2f1e9}</MetaDataID>
        private void RemoveLocalConnection(string contextID, string sessionIdentity)
        {
            var localConnection = (from interConnection in RunContexts
                                   where interConnection.SessionIdentity == sessionIdentity
                                   select interConnection).FirstOrDefault();
            if (localConnection != null)
            {
                lock (RunContexts)
                {
                    RunContexts.Remove(localConnection);
                    if (localConnection.IsolatedContext == IsolatedContext.GetIsolatedContext(contextID))
                        localConnection.IsolatedContext.IsolatedContextUnloaded -= IsolatedContext_IsolatedContextUnloaded;
                }
            }
        }

        /// <MetaDataID>{2034cd67-9e64-4de9-8a83-bb7eaf240015}</MetaDataID>
        private void IsolatedContext_IsolatedContextUnloaded(object sender, EventArgs e)
        {
            IsolatedContext isolatedContext = sender as IsolatedContext;
            var localConnections = (from connection in RunContexts
                                    where connection.IsolatedContext == isolatedContext
                                    select connection).ToList();
            foreach (var localConnection in localConnections)
            {
                RunContexts.Remove(localConnection);
                isolatedContext.IsolatedContextUnloaded -= IsolatedContext_IsolatedContextUnloaded;
            }

            if (RunContexts.Count == 0 && InterConnections.Count == 0)
                Close();
            else
            {
                foreach (var localConnection in localConnections)
                {
                    RequestData disconnectRequestData = new RequestData();
                    disconnectRequestData.SessionIdentity = localConnection.SessionIdentity;
                    disconnectRequestData.RequestType = RequestType.Disconnect;
                    disconnectRequestData.ChannelUri = localConnection.ChannelUri;

                    Task.Run(async () =>
                    {
                        try
                        {
                            SendRequest(disconnectRequestData);
                        }
                        catch (Exception error)
                        {
                        }
                    });
                }
            }

        }

#endif

        /// <MetaDataID>{d33fa544-7c73-483f-bfed-bbbf9714ebd0}</MetaDataID>
        private void EnsureLogicalConnection(ResponseData responseData, WebSocketClient webSocket)
        {
            lock (_InterConnections)
            {
                var logicalConnection = (from interConnection in InterConnections
                                         where interConnection.Internal == webSocket && interConnection.Public == this && interConnection.SessionIdentity == responseData.SessionIdentity
                                         select interConnection).FirstOrDefault();
                if (logicalConnection == null)
                {
                    logicalConnection = new InterConnection() { Public = this, Internal = webSocket, SessionIdentity = responseData.SessionIdentity, ChannelUri = responseData.ChannelUri };
                    AddInterConnection(logicalConnection);
                    webSocket.AddLogicalConnection(logicalConnection);
                }
            }
        }

        /// <MetaDataID>{634e2c37-d8fd-4c8e-a929-691f15ed3d7b}</MetaDataID>
        public OOAdvantech.Remoting.RestApi.ResponseData SendRequest(RequestData request)
        {
            var task = SendRequestAsync(request);

            if (!task.Wait(TimeSpan.FromSeconds(25)))
            {
                if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                {

                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }
            }
            return task.Result;

        }


        /// <MetaDataID>{90c0e9f1-1374-4510-baf3-4ce293bee05d}</MetaDataID>
        int NextRequestID = 1;
        /// <MetaDataID>{03a617fc-63e0-49d3-aa4c-50f2c2eb66a3}</MetaDataID>
        System.Collections.Generic.Dictionary<int, System.Threading.Tasks.TaskCompletionSource<OOAdvantech.Remoting.RestApi.ResponseData>> RequestTasks = new Dictionary<int, TaskCompletionSource<ResponseData>>();
        /// <MetaDataID>{60d273ba-fb0c-487c-a47c-7b4b74433cf1}</MetaDataID>
        public System.Threading.Tasks.Task<OOAdvantech.Remoting.RestApi.ResponseData> SendRequestAsync(RequestData request)
        {
            lock (this)
            {
                request.CallContextID = NextRequestID++;
                string message = Json.JsonConvert.SerializeObject(request);
                message = ((int)MessageHeader.Request).ToString() + message;
                var taskCompletionSource = new TaskCompletionSource<ResponseData>();
                lock (RequestTasks)
                {
                    RequestTasks[request.CallContextID] = taskCompletionSource;
                }
                SendMessage(message);
                return taskCompletionSource.Task;
            }
        }

        ///// <MetaDataID>{81d44b90-4323-43c2-9abc-b8a4f2b86f21}</MetaDataID>
        //public void SendRequestAsync(RequestData requestData)
        //{
        //    lock (this)
        //    {
        //        requestData.CallContextID = -2;
        //        string message = Json.JsonConvert.SerializeObject(requestData);
        //        message = ((int)MessageHeader.Request).ToString() + message;
        //        SendMessage(message);
        //    }
        //}

        /// <MetaDataID>{6136c77e-0910-4df9-90d9-a38bafbe4d73}</MetaDataID>
        public void SendResponce(ResponseData responseData)
        {
            string responseDatajson = Json.JsonConvert.SerializeObject(responseData);
            responseDatajson = ((int)MessageHeader.Response).ToString() + responseDatajson;
            SendMessage(responseDatajson);
        }

        /// <MetaDataID>{a96cc5f7-0c50-47c7-8078-4ca8df27ed67}</MetaDataID>
        public void RejectRequest(Task<ResponseData> task)
        {
            lock (RequestTasks)
            {
                int requestID = (from taskSourceEntry in RequestTasks
                                 where taskSourceEntry.Value.Task == task
                                 select taskSourceEntry.Key).FirstOrDefault();
                if (requestID != 0)
                    RequestTasks.Remove(requestID);
            }
        }


    }

    /// <MetaDataID>{f99ebbfb-4590-4cfa-b2a0-4647a634c78f}</MetaDataID>
    public enum MessageHeader
    {
        Request = 1,
        Response = 2

    }


}

namespace System.Net.WebSockets
{
#if DeviceDotNet
    //
    // Summary:
    //     Contains the list of possible WebSocket errors.
    /// <MetaDataID>{1410ec39-36fa-4929-bb48-17892161ecae}</MetaDataID>
    public enum WebSocketError
    {
        //
        // Summary:
        //     Indicates that there was no native error information for the exception.
        Success = 0,
        //
        // Summary:
        //     Indicates that a WebSocket frame with an unknown opcode was received.
        InvalidMessageType = 1,
        //
        // Summary:
        //     Indicates a general error.
        Faulted = 2,
        //
        // Summary:
        //     Indicates that an unknown native error occurred.
        NativeError = 3,
        //
        // Summary:
        //     Indicates that the incoming request was not a valid websocket request.
        NotAWebSocket = 4,
        //
        // Summary:
        //     Indicates that the client requested an unsupported version of the WebSocket protocol.
        UnsupportedVersion = 5,
        //
        // Summary:
        //     Indicates that the client requested an unsupported WebSocket subprotocol.
        UnsupportedProtocol = 6,
        //
        // Summary:
        //     Indicates an error occurred when parsing the HTTP headers during the opening
        //     handshake.
        HeaderError = 7,
        //
        // Summary:
        //     Indicates that the connection was terminated unexpectedly.
        ConnectionClosedPrematurely = 8,
        //
        // Summary:
        //     Indicates the WebSocket is an invalid state for the given operation (such as
        //     being closed or aborted).
        InvalidState = 9
    }

    /// <MetaDataID>{ac30fe25-3f53-49ae-a3b2-c87e9035abfe}</MetaDataID>
    public sealed class WebSocketException : System.Exception
    {
        public readonly WebSocketError ConnectionClosedPrematurely;


        public WebSocketException()
        {

        }

        public WebSocketException(WebSocketError connectionClosedPrematurely)
        {
            this.ConnectionClosedPrematurely = connectionClosedPrematurely;
        }

        public WebSocketException(WebSocketError connectionClosedPrematurely, string message) : base(message)
        {
            ConnectionClosedPrematurely = connectionClosedPrematurely;

        }
    }
#endif
}





