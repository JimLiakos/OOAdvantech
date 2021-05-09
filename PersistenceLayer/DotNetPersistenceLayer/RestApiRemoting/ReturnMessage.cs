using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>OOAdvantech.Remoting.RestApi.ReturnMessage</MetaDataID>
    public class ReturnMessage
    {

        public ReturnMessage(string channelUri)
        {
            ChannelUri = channelUri;
        }
        public ReturnMessage( )
        {

        }

        //public void UnMarshal()
        //{
        //    var jSetttings = new JsonSerializerSettings(JsonContractType.Deserialize, true, null, null, null);// { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null) };
        //     OutArgs = JsonConvert.DeserializeObject(JsonOutArgs, jSetttings) as object[];
        //}


        [JsonIgnore]
        internal object[] OutArgs;

        [JsonIgnore]
        ServerSessionPart _ServerSession;

        [JsonIgnore]
        internal ServerSessionPart ServerSession
        {
            get
            {
                return _ServerSession;
            }
            set
            {
                _ServerSession = value;
                if (_ServerSession != null)
                    ServerProcessIdentity =( _ServerSession as IServerSessionPart) .ServerProcessIdentity.ToString();
                else
                    ServerProcessIdentity = null;

            }
        }

        [JsonIgnore]
        internal System.Reflection.MethodInfo MethodInfo;

        RestApiExceptionData _Exception;
        public RestApiExceptionData Exception
        {
            get
            {
                return _Exception;
            }
            set
            {
                _Exception = value;
            }
        }
        /// <MetaDataID>{37761c41-17f4-4dd3-a0bc-f2fc365692d8}</MetaDataID>
        public string JsonOutArgs { get; set; }
        /// <MetaDataID>{7cb4d382-6101-4bb8-a7a6-e863de73ecb8}</MetaDataID>
        public string ChannelUri { get; set; }
        /// <MetaDataID>{17d86c7b-3bd7-48c2-8c8b-c95b49074919}</MetaDataID>
        public string ServerProcessIdentity { get; set; }
        /// <MetaDataID>{9edaec98-a908-47f5-be38-8814d24ec2de}</MetaDataID>
        public string ReturnObjectJson { get; set; }
        /// <MetaDataID>{4d8a71b8-1a63-436e-a4bd-82e48d174742}</MetaDataID>
        public string ReturnType { get; set; }
        /// <MetaDataID>{3b7524a8-9215-49c2-8ca6-0ecde8845b17}</MetaDataID>
        public ProxyType ReturnTypeMetaData { get; set; }


        public bool ReAuthenticate { get; set; }


        /// <MetaDataID>{2b976efc-0635-443b-8c7d-821bcdc720c3}</MetaDataID>
        public string X_Access_Token { get; set; }

        [JsonIgnore]
        public object RetVal { get; internal set; }
        public bool Web { get; internal set; }
        public string InternalChannelUri { get;  set; }
        public ObjRef ServerSessionObjectRef { get;  set; }

        internal void Marshal()
        {
            //string internalChannelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("internalChannelUri") as string;
#if DeviceDotNet
            //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, SerializationBinder= new OOAdvantech.Remoting.RestApi.SerializationBinder(Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, ChannelUri, InternalChannelUri, ServerSession, Web) };
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, Web?JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, ChannelUri, InternalChannelUri, ServerSession);
#else
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, Web?JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, ChannelUri, InternalChannelUri, ServerSession);// { TypeNameHandling = ServerSession.Web ? TypeNameHandling.None : TypeNameHandling.All, Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, ChannelUri, InternalChannelUri, ServerSession,Web) };
#endif
            //if (Web)
            //{
            //    jSetttings.ReferenceResolver = new ReferenceResolver();
            //    jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
            //    jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //}
            JsonOutArgs = JsonConvert.SerializeObject(OutArgs, jSetttings);

            jSetttings.ClearObjectRefIds();

            ReturnObjectJson = JsonConvert.SerializeObject(RetVal, jSetttings);


            if (MethodInfo != null)
            {
                ReturnType = MethodInfo.ReturnType.FullName;
                ProxyType httpProxyType = null;
                lock (ServerSession)
                {
                    if (!ServerSession.MarshaledTypes.TryGetValue(MethodInfo.ReturnType.AssemblyQualifiedName, out httpProxyType))
                    {
                        httpProxyType = new ProxyType(MethodInfo.ReturnType);
                        ReturnTypeMetaData = httpProxyType;
                        ServerSession.MarshaledTypes[MethodInfo.ReturnType.AssemblyQualifiedName] = httpProxyType;
                    }
                }
            }
            else if(RetVal!=null)
            {
                ReturnType = RetVal.GetType(). FullName;
                ProxyType httpProxyType = null;
                lock (ServerSession)
                {
                    if (!ServerSession.MarshaledTypes.TryGetValue(RetVal.GetType().AssemblyQualifiedName, out httpProxyType))
                    {
                        httpProxyType = new ProxyType(RetVal.GetType());
                        ReturnTypeMetaData = httpProxyType;
                        ServerSession.MarshaledTypes[RetVal.GetType().AssemblyQualifiedName] = httpProxyType;
                    }
                }
            }
        }
    }
}
