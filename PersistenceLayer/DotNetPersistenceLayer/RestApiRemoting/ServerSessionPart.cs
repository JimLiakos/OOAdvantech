using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Reflection;
using OOAdvantech.Json;
using System.Timers;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

#if !DeviceDotNet
using System.Data.HashFunction.CRC;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;
using System.Security.Claims;
using System.Runtime.Remoting.Lifetime;
#endif

using OOAdvantech.Json.Linq;
using OOAdvantech.Remoting.RestApi.Serialization;


namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{9e696a9f-b75e-480a-814f-ca22f6d9b12c}</MetaDataID>
    //public interface IServerSessionPart
    //{
    //    string SessionID { get; }
    //}

    /// <MetaDataID>{90de0c93-7466-4ef6-8479-444e5784619c}</MetaDataID>
    public class ServerSessionPart : OOAdvantech.Remoting.ServerSessionPart
    {

        /// <MetaDataID>{d4f67b3d-d782-4539-9fc0-852c652ddfc6}</MetaDataID>
        System.Timers.Timer DisconnectTimer;

        /// <MetaDataID>{854e0860-1ae9-4a2b-837a-d51482c35f20}</MetaDataID>
        public Dictionary<string, ProxyType> MarshaledTypes = new Dictionary<string, ProxyType>();

        /// <MetaDataID>{c9d73168-d8bb-406b-a837-cada411fb29b}</MetaDataID>
        public IChannel Channel;


        public override bool UseNetRemotingChamnel => false;

#if DeviceDotNet
        static CRCTool CRCTool = new CRCTool();
        static ServerSessionPart()
        {

            CRCTool.Init(CRCTool.CRCCode.CRC32);
        }
#endif


        /// <MetaDataID>{177c1f59-af6c-4a1d-a477-0a4e23f38bc9}</MetaDataID>
        ElapsedEventHandler Elapsed;
        /// <MetaDataID>{3320e1ab-7925-44d4-819e-a811606806a5}</MetaDataID>
        public ServerSessionPart(System.Guid clientProcessIdentity, string channelUri, string internalChannelUri, bool web) : base(clientProcessIdentity)
        {



            DisconnectTimer = new System.Timers.Timer();
            DisconnectTimer.Interval = TimeSpan.FromMinutes(0.5).TotalMilliseconds;
            Elapsed = (object Header, ElapsedEventArgs e) =>
            {
                _Connected = false;
                DisconnectTimer.Elapsed -= Elapsed;
                ClientProcessTerminates();
            };
            DisconnectTimer.Elapsed += Elapsed;



            lock (ServerSessions)
            {
                //if (!string.IsNullOrWhiteSpace(internalChannelUri))
                //    channelUri += "(" + internalChannelUri + ")";

                ChannelUri = channelUri;
                InternalChannelUri = internalChannelUri;
                Web = web;
#if DeviceDotNet
                string channelID = CRCTool.crcbitbybitfast(System.Text.Encoding.UTF8.GetBytes(channelUri.ToLower())).ToString("X");
#else
                string channelID = CRCFactory.Instance.Create(CRCConfig.CRC64).ComputeHash(System.Text.Encoding.UTF8.GetBytes(channelUri.ToLower())).AsHexString();
#endif
                _SessionIdentity += "." + channelID;

                ServerSessions[_SessionIdentity] = this;
            }
            //this.SessionIdentity = "";
        }

#if DeviceDotNet

#endif

        ~ServerSessionPart()
        {

        }

        /// <MetaDataID>{a3db7a27-9f77-4c60-8b86-e5d0bde489b6}</MetaDataID>
        internal static Dictionary<string, ServerSessionPart> ServerSessions = new Dictionary<string, ServerSessionPart>();



        public override void ClientProcessTerminates()
        {
            base.ClientProcessTerminates();
            lock (ServerSessions)
            {
                ServerSessions.Remove(_SessionIdentity);

                //ILease lease = System.Runtime.Remoting.RemotingServices.GetLifetimeService(this) as ILease;
                //lease.Renew(TimeSpan.FromSeconds(3));
                //lease = System.Runtime.Remoting.RemotingServices.GetLifetimeService(this) as ILease;
            }
        }


        /// <MetaDataID>{03fe2d1c-94f3-4194-8dac-ca2046941dbe}</MetaDataID>
        internal static ServerSessionPart GetServerSessionPart(Guid clientProcessIdentity, string channelUri)
        {
            lock (ServerSessions)
            {

#if DeviceDotNet
                string channelID = CRCTool.crcbitbybitfast(System.Text.Encoding.UTF8.GetBytes(channelUri.ToLower())).ToString("X");
#else
                string channelID = CRCFactory.Instance.Create(CRCConfig.CRC64).ComputeHash(System.Text.Encoding.UTF8.GetBytes(channelUri.ToLower())).AsHexString();
#endif
                string sessionIdentity = clientProcessIdentity.ToString("N") + "." + ServerProcessIdentity.ToString("N") + "." + channelID;
                ServerSessionPart serverSessionPart = null;
                ServerSessions.TryGetValue(sessionIdentity, out serverSessionPart);
                return serverSessionPart;
            }
        }

        internal static ServerSessionPart GetServerSessionPart(string sessionIdentity)
        {
            lock (ServerSessions)
            {
                ServerSessionPart serverSessionPart = null;
                ServerSessions.TryGetValue(sessionIdentity, out serverSessionPart);
                return serverSessionPart;
            }
        }

        /// <MetaDataID>{bc4f09f3-cbaf-4025-8cce-4fac11b6186d}</MetaDataID>
        public readonly bool Web;

        /// <MetaDataID>{5eb25f22-8d0f-411f-873f-1612d1f4cba9}</MetaDataID>
        public readonly String ChannelUri;
        /// <MetaDataID>{4ad950d2-add3-4311-b790-f74419f7560b}</MetaDataID>
        public readonly string InternalChannelUri;


        /// <MetaDataID>{20b5ff2c-36d9-4dcc-8c88-e54f0f9e289a}</MetaDataID>
        public IEndPoint WebSocketEndPoint
        {
            set
            {
                if ((Channel is WebSocketChannel) && (Channel as WebSocketChannel).WebSocketEndPoint == value)
                    return;
                if (Channel != null)
                {
                }
                Channel = new WebSocketChannel(value);
            }
        }



        /// <MetaDataID>{b7f0d14a-f832-4f7a-b1c8-46bbcfeed0c4}</MetaDataID>
        internal override bool EventCallback(EventInfo eventInfo, ExtObjectUri eventPublisherUri, object[] args, InvokeType invokeType)
        {
            if (Connected)
            {
                EventCallbackMessage eventCallbackMessage = new EventCallbackMessage()
                {
                    ServerSession = this,
                    Args = args,
                    EventInfoData = new EventInfoData(eventInfo),
                    EventPublisherUri = eventPublisherUri,
                    SessionIdentity = SessionIdentity,
                    Web = this.Web
                };
                eventCallbackMessage.Marshal();

                string json = JsonConvert.SerializeObject(eventCallbackMessage);

                RequestData requestData = new RequestData();
                requestData.SessionIdentity = SessionIdentity;
                requestData.RequestType = RequestType.Event;
                requestData.ChannelUri = ChannelUri;

                requestData.details = json;

                bool? ConnectionIsOpen = true;

                try
                {
                    ConnectionIsOpen = Channel?.EndPoint?.ConnectionIsOpen;
                    if (ConnectionIsOpen != null && !ConnectionIsOpen.Value)
                    {

                    }
                }
                catch (Exception error)
                {
                }
                Task.Run(async () =>
                {
                    bool retry = false;
                    do
                    {
                        try
                        {
                            if (invokeType == InvokeType.Sync)
                                Channel.ProcessRequest(requestData);
                            else
                            {
                                requestData.SendTimeout = Binding.DefaultBinding.SendTimeout.TotalMilliseconds;
                                var task = Channel.AsyncProcessRequest(requestData);
                                if (task == null)
                                {

                                }
                                await task;
                            }

                        }
                        catch (Exception error)
                        {

                            //if (error is System.Net.WebSockets.WebSocketException || error.InnerException is System.Net.WebSockets.WebSocketException)
                            //    retry = true;
                            System.Diagnostics.Debug.Assert(false, "RestApi AsyncProcessRequest failed");

#if !DeviceDotNet

                            if (!System.Diagnostics.EventLog.SourceExists("Rest Api channel", "."))
                                System.Diagnostics.EventLog.CreateEventSource("Rest Api channel", "OOAdvance");

                            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                            myLog.Source = "Rest Api channel";

                            if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                                myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);

                            myLog.WriteEntry("RestApi AsyncProcessRequest failed :" + Environment.NewLine + error.Message + Environment.NewLine + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif


                        }
                    } while (retry);
                });
            }
            else
            {
            }
            return true;
        }

        /// <summary>
        /// Marks all remote ref objects types which are successfully cached in client session part
        /// </summary>
        /// <param name="methodCallMessage">
        /// Method call message keeps info for  cached types in client session part 
        /// </param>
        internal void SessionTypesSync(MethodCallMessage methodCallMessage)
        {
            if (methodCallMessage.CachedTypes != null)
            {
                lock (this)
                {

                    foreach (string cachedType in methodCallMessage.CachedTypes)
                    {
                        ProxyType proxyType = null;
                        if (MarshaledTypes.TryGetValue(cachedType, out proxyType))
                            proxyType.Paired = true;
                        else
                        {
                        }
                    }
                }
            }

        }

#if !DeviceDotNet
#endif

        /// <MetaDataID>{b1a2c097-c840-4e6e-810d-f6731c0ea07b}</MetaDataID>
        public override MarshalByRefObject GetObjectFromUri(ExtObjectUri extObjectUri)
        {
            return MethodCallMessage.GetObjectFromUri(extObjectUri) as MarshalByRefObject;
        }

        /// <MetaDataID>{dc8b5103-ea81-40bb-97d0-3a59ff766059}</MetaDataID>
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <MetaDataID>{6ea4ebf2-c232-465c-92cd-76413220df7c}</MetaDataID>
        /// <summary>
        /// Is the number of seconds that have elapsed since January 1, 1970 (midnight UTC/GMT)
        /// </summary>
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <MetaDataID>{ed809c8d-6990-4fc9-9d23-d0dbc1d934ee}</MetaDataID>
        public string X_Auth_Token { get; internal set; }

        /// <exclude>Excluded</exclude>
        string _X_Access_Token;
        /// <MetaDataID>{b5a12e43-65e4-40ec-ad07-592e422dd12e}</MetaDataID>
        public string X_Access_Token
        {
            get
            {
                return _X_Access_Token;
            }
            internal set
            {
                if (_X_Access_Token != null && value == null)
                {

                }
                _X_Access_Token = value;

            }
        }
        /// <MetaDataID>{28499a53-2ed0-4a3d-9674-e1c395939ba0}</MetaDataID>
        public AuthUser AuthUser { get; internal set; }

        /// <exclude>Excluded</exclude> 
        bool _Connected;
        /// <MetaDataID>{12300ba4-1ea7-4cde-9a8e-ac94098eac23}</MetaDataID>
        public bool Connected
        {
            get
            {
                return _Connected;
            }

            //internal set
            //{
            //    var datetime = DateTime.UtcNow;
            //    string timestamp = datetime.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
            //    if (!value && _Connected)
            //    {
            //        System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel disconnect {1} : {0} ", this.SessionIdentity, timestamp));

            //        DisconnectTimer.Start();
            //    }

            //    if (value && !_Connected && DisconnectTimer.Enabled)
            //    {
            //        System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel connect {1} : {0} ", this.SessionIdentity, timestamp));
            //        DisconnectTimer.Stop();
            //    }


            //    _Connected = value;

            //}
        }

        /// <summary>
        /// Keeps session physical channels state connected true or false.
        /// Client can be establish two types physical connection.
        /// Directs and indirects.
        /// </summary>
        /// <MetaDataID>{86865da1-058c-4466-8a43-c59e1521ff18}</MetaDataID>
        Dictionary<string, bool> PhysicalConnections = new Dictionary<string, bool>();

        /// <summary>
        /// Sets state for physical connection
        /// </summary>
        /// <param name="physicalConnectionID">
        /// Define the physical connection id
        /// </param>
        /// <param name="connected">
        /// Defines the state connected true otherwise false
        /// </param>
        /// <MetaDataID>{a428e9ab-ef4c-455a-8d71-b0584d0f77de}</MetaDataID>
        public void SetConnectionState(string physicalConnectionID, bool connected)
        {
            if (physicalConnectionID == null)
                physicalConnectionID = "";//web view


            try
            {
                var ConnectionIsOpen = Channel?.EndPoint?.ConnectionIsOpen;
                if (ConnectionIsOpen != null && !ConnectionIsOpen.Value)
                {

                }
            }
            catch (Exception error)
            {
            }

            bool sessionConnected = false;
            lock (PhysicalConnections)
            {
                if (!PhysicalConnections.ContainsKey(physicalConnectionID) || PhysicalConnections[physicalConnectionID] != connected)
                    PhysicalConnections[physicalConnectionID] = connected;

                foreach (bool channelConnected in PhysicalConnections.Values)
                    sessionConnected |= channelConnected;
            }

            #region Communication session completed when all physical connections are disconnected for a period of time
            if (!sessionConnected && _Connected)
                DisconnectTimer.Start();

            if (sessionConnected && !_Connected && DisconnectTimer.Enabled)
                DisconnectTimer.Stop();
            #endregion

            _Connected = sessionConnected;
        }


        /// <MetaDataID>{93d22bfc-8cc4-4112-a858-9a7c006d5348}</MetaDataID>
        internal AuthUser GetAuthData(MethodCallMessage methodCallMessage)
        {
            int threadId = 0;
            var task = System.Threading.Tasks.Task<AuthUser>.Run(() =>
              {
#if !DeviceDotNet
                  threadId = AppDomain.GetCurrentThreadId();
#endif
                  string authToken = methodCallMessage.X_Auth_Token;
                  string x_Access_Token = methodCallMessage.X_Access_Token;

                  if (authToken == null && x_Access_Token == null)
                      return null;

                  if (!string.IsNullOrWhiteSpace(authToken) && (AuthUser == null || AuthUser.AuthToken != authToken))
                  {
                      //https://stackoverflow.com/questions/39938542/firebase-net-token-verification/48180173
#if !DeviceDotNet

                      GetUserFromjwToken(authToken);
#else
                      if (authToken == DeviceAuthentication.IDToken)
                          AuthUser = DeviceAuthentication.AuthUser;
#endif

                      if (AuthUser == null)
                      {
                          X_Access_Token = null;
                          methodCallMessage.ReAuthenticate = true;
                      }
                  }
                  else if (string.IsNullOrWhiteSpace(x_Access_Token) || X_Access_Token != x_Access_Token)
                  {
                      if (!string.IsNullOrWhiteSpace(authToken) && AuthUser != null && AuthUser.AuthToken == authToken)
                          return AuthUser;

                      if (X_Access_Token != null)
                          methodCallMessage.ReAuthenticate = true;
                      X_Access_Token = null;
                      AuthUser = null;
                  }
                  else if (!string.IsNullOrWhiteSpace(x_Access_Token))
                  {
                      if (AuthUser.ExpirationTime < DateTime.Now)
                      {
                          methodCallMessage.ReAuthenticate = true;
                          X_Access_Token = null;
                          AuthUser = null;
                      }
                  }
                  return AuthUser;
              });

            if (!task.Wait(System.TimeSpan.FromSeconds(9)))
            {
                if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                {
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }

                return task.Result;
            }
            else
            {
                return task.Result;
            }
        }

        /// <MetaDataID>{72c2d38e-a4aa-4407-938d-65830eb69702}</MetaDataID>
        public List<string> AccessTokens = new List<string>();

#if !DeviceDotNet
        /// <MetaDataID>{8a30873f-6c9a-4cfd-a701-5c2037449aa2}</MetaDataID>
        private void GetUserFromjwToken(string authToken)
        {
            lock (this)
            {
                if (AuthUser != null && AuthUser.AuthToken == authToken)
                    return;
                var startTime = System.DateTime.Now;
                try
                {
                    AuthUser authUser;
                    //var handler = new JwtSecurityTokenHandler();
                    //JwtSecurityToken tokenS = handler.ReadToken(authToken) as JwtSecurityToken;
                    JwtSecurityToken tokenS = Validate(authToken);
                    // var validToken= handler.ValidateToken.ValidateToken(tokenS);

                    //dontwait4waiter
                    //var firebaseJWTAuth = new FirebaseJWTAuth("demomicroneme");
                    //var firebaseJWTAuth = new FirebaseJWTAuth("dontwait4waiter");
                    //string res = firebaseJWTAuth.Verify(authToken);

                    authUser = new AuthUser();

                    string kid = tokenS.Header["kid"] as string;

                    string iat = (from claim in tokenS.Claims where claim.Type == "iat" select claim.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(iat))
                        authUser.IssuedAt = FromUnixTime(int.Parse(iat)).ToLocalTime();

                    string exp = (from claim in tokenS.Claims where claim.Type == "exp" select claim.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(exp))
                        authUser.ExpirationTime = FromUnixTime(int.Parse(exp)).ToLocalTime();


                    string auth_time = (from claim in tokenS.Claims where claim.Type == "auth_time" select claim.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(auth_time))
                        authUser.Auth_Time = FromUnixTime(int.Parse(auth_time)).ToLocalTime();

                    authUser.Audience = (from claim in tokenS.Claims where claim.Type == "aud" select claim.Value).FirstOrDefault();
                    authUser.Email = (from claim in tokenS.Claims where claim.Type == "email" select claim.Value).FirstOrDefault();

                    if ((from claim in tokenS.Claims where claim.Type == "email_verified" select claim.Value).FirstOrDefault() != null)
                        authUser.Email_Verified = bool.Parse((from claim in tokenS.Claims where claim.Type == "email_verified" select claim.Value).FirstOrDefault());
                    authUser.Iss = (from claim in tokenS.Claims where claim.Type == "iss" select claim.Value).FirstOrDefault();
                    authUser.Name = (from claim in tokenS.Claims where claim.Type == "name" select claim.Value).FirstOrDefault();
                    authUser.Picture = (from claim in tokenS.Claims where claim.Type == "picture" select claim.Value).FirstOrDefault();
                    authUser.Subject = (from claim in tokenS.Claims where claim.Type == "sub" select claim.Value).FirstOrDefault();
                    authUser.User_ID = (from claim in tokenS.Claims where claim.Type == "user_id" select claim.Value).FirstOrDefault();

                    var firebaseAttributes = (from claim in tokenS.Claims where claim.Type == "firebase" select JObject.Parse(claim.Value)).FirstOrDefault();

                    authUser.Firebase_Sign_in_Provider = (from fireBaseProperty in firebaseAttributes.Properties()
                                                          where fireBaseProperty.Name == "sign_in_provider"
                                                          select (fireBaseProperty.Value as JValue).Value).FirstOrDefault() as string;

                    authUser.AuthToken = authToken;

                    DateTime now = System.DateTime.Now;
                    bool isValidToken = true;
                    if (authUser.Audience != Authentication.FirebaseProjectId ||
                    authUser.Iss != "https://securetoken.google.com/" + Authentication.FirebaseProjectId ||
                    (authUser.IssuedAt - now).TotalSeconds > 10 ||
                    authUser.ExpirationTime <= now)
                    {
                        isValidToken = false;
                    }

                    if (!isValidToken || X_Auth_Token != authToken)
                        X_Access_Token = Guid.NewGuid().ToString("N");
                    if (!isValidToken)
                        authUser = null;
                    else
                    {
                        X_Auth_Token = authToken;
                    }

                    AccessTokens.Add(X_Access_Token);


                    AuthUser = authUser;
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.Fail("Failed to extract authenticated user from access token.");
                    AuthUser = null;
                }

                double secs = (System.DateTime.Now - startTime).TotalSeconds;
                if (secs > 05)
                {

                }
                return;
            }
        }

        /// <MetaDataID>{e61fa8eb-91be-4a85-a431-9b10727e568a}</MetaDataID>
        static Dictionary<string, string> Cx509Data;
        /// <MetaDataID>{7c8d195d-94e0-4b21-9f63-7d78fdd660d5}</MetaDataID>
        public static JwtSecurityToken Validate(string authToken)
        {

            HttpClient client = new HttpClient();

            string encodedJwt = authToken;
            // 1. Get Google signing keys
            client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");


            //var task = response.Content.ReadAsStringAsync();
            //task.Wait();
            //var responseDataString = task.Result;

            var task = client.GetAsync(
              "x509/securetoken@system.gserviceaccount.com");
            if (!task.Wait(System.TimeSpan.FromSeconds(9)))
            {
                if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                {
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }

            }

            HttpResponseMessage response = task.Result;
            if (!response.IsSuccessStatusCode) { return null; }

            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            var x509DataTask = response.Content.ReadAsStringAsync();
            x509DataTask.Wait();


            if (!x509DataTask.Wait(System.TimeSpan.FromSeconds(9)))
            {
                if (!x509DataTask.Wait(Binding.DefaultBinding.SendTimeout))
                {
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }

            }

            var x509Data = JSserializer.Deserialize<Dictionary<string, string>>(x509DataTask.Result);

            if (Cx509Data == null)
                Cx509Data = x509Data;

            foreach (var dictionaryEntry in x509Data)
            {
                string dictionaryData = null;
                if (Cx509Data.TryGetValue(dictionaryEntry.Key, out dictionaryData))
                {
                    if (dictionaryData != dictionaryEntry.Value)
                    {

                    }
                }
                else
                {

                }
            }
            Cx509Data = x509Data;

            SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
            // 2. Configure validation parameters
            string FirebaseProjectId = Authentication.FirebaseProjectId;

            if (string.IsNullOrWhiteSpace(FirebaseProjectId))
            {
                throw new Exception("Empty FirebaseProjectId. Initialize with 'OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase'");
            }
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = "https://securetoken.google.com/" + FirebaseProjectId,
                ValidAudience = FirebaseProjectId,
                IssuerSigningKeys = keys,
            };
            // 3. Use JwtSecurityTokenHandler to validate signature, issuer, audience and lifetime
            var handler = new JwtSecurityTokenHandler();
            SecurityToken token;
            try
            {
                ClaimsPrincipal principal = handler.ValidateToken(encodedJwt, parameters, out token);
                var jwt = (JwtSecurityToken)token;
                // 4.Validate signature algorithm and other applicable valdiations
                if (jwt.Header.Alg != SecurityAlgorithms.RsaSha256)
                {
                    throw new SecurityTokenInvalidSignatureException(
                      "The token is not signed with the expected algorithm.");
                }
                return jwt;
            }
            catch (Exception error)
            {

                throw;
            }
        }
        /// <MetaDataID>{29f7bb2b-637c-4780-8b24-8dad58ec73b2}</MetaDataID>
        static SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }
#endif

        /// <MetaDataID>{8d568766-3fec-4727-bbb1-ba656265a9f2}</MetaDataID>
        internal ObjRef TransformToPublic(ObjRef objectRef)
        {
            if (RemotingServices.InternalEndPointResolver != null && objectRef.ChannelUri != ChannelUri)
            {
                ObjRef byRef = new ObjRef(objectRef.Uri, RemotingServices.InternalEndPointResolver.TranslateToPublic(objectRef.ChannelUri), objectRef.InternalChannelUri, objectRef.TypeName, objectRef.TypeMetaData);
                byRef.MembersValues = objectRef.MembersValues;

            }

            return objectRef;
        }

        /// <MetaDataID>{1e6197d1-250a-4819-9a91-f23547b9e07d}</MetaDataID>
        internal ObjRef GetServerSesionObjectRef()
        {


            ProxyType httpProxyType = null;
            Type instanceType = GetType();
            string uri = null;
            lock (this)
            {
                uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(this as MarshalByRefObject);
                if (uri == null)
                    uri = System.Runtime.Remoting.RemotingServices.Marshal(this as MarshalByRefObject).URI;

                Remoting.Tracker.WeakReferenceOnMarshaledObjects[uri] = new WeakReference(this);



                if (!MarshaledTypes.TryGetValue(instanceType.AssemblyQualifiedName, out httpProxyType))
                {
                    httpProxyType = new ProxyType(instanceType);
                    MarshaledTypes[instanceType.AssemblyQualifiedName] = httpProxyType;
                }
            }
            ObjRef byref = new ObjRef(uri, ChannelUri, InternalChannelUri, GetType().AssemblyQualifiedName, httpProxyType);
            return byref;

        }
    }



    /// <MetaDataID>{0382da57-d2e6-4834-b18b-1b7ef993d56a}</MetaDataID>
    public class ClientSessionPart : Remoting.ClientSessionPart
    {
        /// <MetaDataID>{43fefbb0-6091-4402-a8c6-dcbeac9ca542}</MetaDataID>
        protected override void OnProcessExit(object sender, EventArgs e)
        {
            base.OnProcessExit(sender, e);
            SessionTerminated = true;

            int openSessionsCount = (from clientSessionPart in RenewalManager.GetSessions().Values.OfType<ClientSessionPart>()
                                     where !clientSessionPart.SessionTerminated
                                     select clientSessionPart).Count();
            if (openSessionsCount == 0)
            {
                foreach (var clientSessionPart in (from clientSessionPart in RenewalManager.GetSessions().Values.OfType<ClientSessionPart>()
                                                   where clientSessionPart.SessionTerminated
                                                   select clientSessionPart))
                {
                    clientSessionPart.Channel.Close();
                }
            }



        }



        public override void Subscribe(IProxy proxy, EventInfoData eventInfoData, bool allowAsynchronous)
        {
            try
            {
                base.Subscribe(proxy, eventInfoData, allowAsynchronous);
            }
            catch (MissingServerObjectException error)
            {
                Reconnect(false);
                base.Subscribe(proxy, eventInfoData, allowAsynchronous);

            }
            catch (Exception error)
            {

                throw;
            }
        }

        public override bool UseNetRemotingChamnel => false;

        public ServerSessionPartInfo GetServerSession(string channelUri, Guid processIdentity)
        {
            var methodCallMessage = new MethodCallMessage(channelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", "", "", StandardActions.CreateCommunicationSession, new object[0]);
            methodCallMessage.ClientProcessIdentity = processIdentity.ToString("N");
            RequestData requestData = new RequestData();
            requestData.SessionIdentity = null;// methodCallMessage.ClientProcessIdentity;
            requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
            requestData.RequestType = RequestType.MethodCall;

            var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
            requestData.ChannelUri = channelUri;


            var responseData = Channel.ProcessRequest(requestData); //Invoke(requestData.PublicChannelUri, requestData, null, null, Binding.DefaultBinding);
            if (responseData != null)
            {
                var returnMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                if (returnMessage.Exception != null)
                {
                    if (returnMessage.Exception.ExceptionCode == ExceptionCode.ConnectionError)
                        throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.ConnectFailure);
#if DeviceDotNet
                    if (returnMessage.Exception.ExceptionCode == ExceptionCode.AccessTokenExpired)
                        throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.RequestCanceled);
#else
                    if (returnMessage.Exception.ExceptionCode == ExceptionCode.AccessTokenExpired)
                        throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.TrustFailure);
#endif
                    throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.UnknownError);
                }

                var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
                ObjRef remoteRef = JsonConvert.DeserializeObject<ObjRef>(returnMessage.ReturnObjectJson, jSetttings);
                Proxy proxy = new Proxy(remoteRef, typeof(IServerSessionPart));// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.IServerSessionPart));
                IServerSessionPart serverSessionPart = proxy.GetTransparentProxy(typeof(IServerSessionPart)) as IServerSessionPart;// (GetTransparentProxy() as IRomotingServer).GetServerSession(RemotingServices.ProcessIdentity);
                return new ServerSessionPartInfo() { SessionIdentity = responseData.SessionIdentity, ServerSessionPart = serverSessionPart, ServerProcessIdentity = Guid.Parse(returnMessage.ServerProcessIdentity), BidirectionalChannel = responseData.BidirectionalChannel };
            }
            return default(ServerSessionPartInfo);
        }


        /// <summary>
        /// This method reconnect client session part with the server
        /// 
        /// </summary>
        /// <param name="disconnectedChanel">
        /// When the previous channel closed unexpectedly  the value of the parameter is true.
        /// Otherwise is false
        /// </param>
        /// <MetaDataID>{c84867b4-e737-4de4-9b51-360677d9fbae}</MetaDataID>
        internal void Reconnect(bool disconnectedChannel)
        {
            int tries = 5; //makes five tries to reconnect
            X_Access_Token = null;
            while (tries > 0)
            {
                try
                {
                    var datetime = DateTime.Now;
                    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                    //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel Reconnect {0} :({2}) {1}", timestamp, _SessionIdentity, System.Diagnostics.Process.GetCurrentProcess().Id));


                    var serverSessionPartInfo = GetServerSession(ChannelUri, ClientProcessIdentity);
                    var serverSessionPartUri = (System.Runtime.Remoting.RemotingServices.GetRealProxy(serverSessionPartInfo.ServerSessionPart) as IProxy)?.Uri;

                    if (ServerProcessIdentity != serverSessionPartInfo.ServerProcessIdentity || ServerSessionPartUri != serverSessionPartUri)
                    {


                        //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel SessionId {0}", SessionIdentity));

                        System.Collections.Generic.List<RemoteEventSubscription> channelSubscriptions = new List<RemoteEventSubscription>();
                        foreach (var entry in EventSubscriptions)
                        {
                            foreach (var subscription in entry.Value)
                            {
                                RemoteEventSubscription remoteEventSubscription = new RemoteEventSubscription();
                                remoteEventSubscription.AllowAsynchronous = false;
                                remoteEventSubscription.eventInfo = new EventInfoData(subscription.EventInfo);
                                remoteEventSubscription.ExtObjectUri = entry.Key;
                                channelSubscriptions.Add(remoteEventSubscription);
                            }
                        }
                        //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel reSubscribe for process {0}", System.Diagnostics.Process.GetCurrentProcess().Id));
                        serverSessionPartInfo.ServerSessionPart.Subscribe(channelSubscriptions);


                        ServerProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
                        _SessionIdentity = serverSessionPartInfo.SessionIdentity;
                        ServerSessionPart = serverSessionPartInfo.ServerSessionPart;
                        ServerSessionPartUri = serverSessionPartUri;
                        SynchronizeSession();

                        System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel clientSessionPart Reconnect {0} :({2}) {1}", timestamp, _SessionIdentity, System.Diagnostics.Process.GetCurrentProcess().Id));


                        foreach (System.WeakReference weakReference in Proxies.Values)
                        {
                            try
                            {
                                if (weakReference.IsAlive)
                                {
                                    var proxy = weakReference.Target as Proxy;
                                    proxy.RaiseReconnectEvent();
                                }
                            }
                            catch (System.Exception error)
                            {

                            }
                        }
                    }
                    else
                    {
                        if (disconnectedChannel)
                        {
                            foreach (System.WeakReference weakReference in Proxies.Values)
                            {
                                try
                                {
                                    if (weakReference.IsAlive)
                                    {
                                        var proxy = weakReference.Target as Proxy;
                                        proxy.RaiseReconnectEvent();
                                    }
                                }
                                catch (System.Exception error)
                                {

                                }
                            }
                        }
                        if (ServerSessionPartUri != serverSessionPartUri)
                        {

                        }
                        //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel same ServerProcessIdentity {0} :({2}) {1}", timestamp, _SessionIdentity, System.Diagnostics.Process.GetCurrentProcess().Id));
                    }

                    return;
                }
                catch (Exception error)
                {
                }
                tries--;
#if !DeviceDotNet
                System.Threading.Thread.Sleep(500);
#else
                System.Threading.Tasks.Task.Delay(500).Wait();
#endif
            }
        }

        public override void EventCallback(string objectUri, EventInfo eventInfo, List<object> args)
        {
            base.EventCallback(objectUri, eventInfo, args);

            //IProxy proxy = null;
            //lock (this)
            //{
            //    if (Proxies.ContainsKey(objectUri))
            //    {
            //        proxy = Proxies[objectUri].Target as IProxy;
            //    }
            //}
        }

        protected override Task SynchronizeSession()
        {
            return Task.FromResult(false);
        }

        internal void UpdateServerSessionPart(ObjRef serverSessionObjectRef)
        {

            if ((System.Runtime.Remoting.RemotingServices.GetRealProxy(ServerSessionPart) as Proxy).ObjectRef.Uri != serverSessionObjectRef.Uri)
                Reconnect(true);
            //(System.Runtime.Remoting.RemotingServices.GetRealProxy(ServerSessionPart) as Proxy).ReconnectToServerObject(serverSessionObjectRef);

        }



        /// <MetaDataID>{8555922b-043e-4e0c-bd5d-2b17588a69a7}</MetaDataID>
        public override bool AllowedBidirectionalCall
        {
            get
            {
                return Bidirectional;
            }
        }

        /// <MetaDataID>{411c8855-04b2-4331-b801-b03ba8246b02}</MetaDataID>
        bool Bidirectional;

        /// <MetaDataID>{a65f2bfa-a65c-44ab-bcf9-c523cefe4a96}</MetaDataID>
        public readonly IChannel Channel;


        /// <MetaDataID>{297e4797-ab50-4fbc-988b-bf8c73c5153c}</MetaDataID>
        bool SessionTerminated;

        /// <MetaDataID>{e8783fbc-9413-4a30-8894-00aa47713dd7}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, ProxyType> ProxyTypes = new Dictionary<string, ProxyType>();
        /// <MetaDataID>{eff5877e-c65e-4a20-9051-6bd38564eea5}</MetaDataID>
        public ClientSessionPart(string channelUri, Guid clientProcessIdentity, ServerSessionPartInfo serverSessionPartInfo, IRemotingServices remotingServices) : base(channelUri, clientProcessIdentity, serverSessionPartInfo, remotingServices)
        {

            _SessionIdentity = serverSessionPartInfo.SessionIdentity;
            //?System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel SessionId {0}", SessionIdentity));
            if (serverSessionPartInfo.BidirectionalChannel.HasValue)
                Bidirectional = serverSessionPartInfo.BidirectionalChannel.Value;
            else
                Bidirectional = false;

            var serverSessionProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(ServerSessionPart) as Proxy;
            ProxyTypes[serverSessionProxy.ObjectRef.TypeName] = serverSessionProxy.ObjectRef.TypeMetaData;

            if (channelUri.Trim().IndexOf("http://") == 0)
                channelUri = "ws://" + channelUri.Substring("http://".Length);

            if (channelUri.StartsWith("ws://") || channelUri.StartsWith("wss://"))
                Channel = new WebSocketChannel(channelUri, this);
#if !DeviceDotNet
            else if (channelUri.Trim().IndexOf("net.tcp://") == 0)
                Channel = new TCPChannel(channelUri);
            else
            {

            }
#endif


            //string internalChannelUri = (serverSessionProxy as Proxy).InternalChannelUri;
        }
    }



}

