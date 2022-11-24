using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;

using System.Reflection;

#if !NetStandard
using System.ServiceModel;
#endif

using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi.Serialization;
#if !DeviceDotNet
using System.Web;
#endif
using OOAdvantech.Json.Linq;

namespace OOAdvantech.Remoting.RestApi
{
    /// <summary>
    /// Stores all relevant information required to generate a proxy in order to communicate with a remote object.
    /// </summary>
    /// <MetaDataID>{994f36c6-7786-4858-bdfd-e8d5bc7be94d}</MetaDataID>
    public class ObjRef
    {
        /// <MetaDataID>{7f01d783-467b-4191-8f96-1ca991524382}</MetaDataID>
        public ObjRef()
        {
        }

        /// <MetaDataID>{ba810e9a-60e1-45fb-befa-b23d487e4fb7}</MetaDataID>
        public ObjRef(string uri, string channelUri, string internalChannelUri, string typeName, ProxyType returnTypeMetaData)
        {
            if (channelUri.LastIndexOf("(") != channelUri.IndexOf("("))
            {

            }
            Uri = uri;
            if (channelUri.IndexOf("net.tcp://") == 0 && internalChannelUri != null)
            {

            }

            if (!string.IsNullOrWhiteSpace(internalChannelUri) && channelUri.IndexOf("(" + internalChannelUri + ")") == -1)
                channelUri += "(" + internalChannelUri + ")";
            ChannelUri = channelUri;
            InternalChannelUri = internalChannelUri;
            TypeName = typeName;
            TypeMetaData = returnTypeMetaData;
            if (TypeMetaData == null)
            {

            }

        }

        /// <MetaDataID>{a4b200fe-324f-4428-bde5-870e1272b056}</MetaDataID>
        public static void GetChannelUriParts(string channelUri, out string publicChannelUri, out string internalchannelUri)
        {

            internalchannelUri = null;
            if (channelUri.IndexOf("(") != -1)
            {
                internalchannelUri = channelUri.Substring(channelUri.IndexOf("(") + 1);
                internalchannelUri = internalchannelUri.Substring(0, internalchannelUri.Length - 1);
                publicChannelUri = channelUri.Substring(0, channelUri.IndexOf("("));
            }
            else
                publicChannelUri = channelUri;
        }


        /// <MetaDataID>{6e1c6091-cbd7-4094-a5f0-5a8ca4e840ad}</MetaDataID>
        internal void CachingObjectMemberValues(object _obj)
        {

            TypeMetaData.CachingObjectMembersValue(_obj, MembersValues);
            if (MembersValues.Count > 0)
            {

            }
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{0ae88982-a84f-462d-b27d-f7168110d3e2}</MetaDataID>
        internal ProxyType GetProxyType()
        {
            if (TypeMetaData == null)
            {
                ClientSessionPart clientSessionPart = RenewalManager.GetSession(ChannelUri, true, RemotingServices.CurrentRemotingServices) as OOAdvantech.Remoting.RestApi.ClientSessionPart;


                if (!clientSessionPart.ProxyTypes.TryGetValue(TypeName, out _TypeMetaData))
                {
                    OOAdvantech.Remoting.RestApi.RemotingServices.GetServerSessionPartMarshaledTypes(clientSessionPart);
                    clientSessionPart.ProxyTypes.TryGetValue(TypeName, out _TypeMetaData);
                }
            }
            else
            {
                ClientSessionPart clientSessionPart = RenewalManager.GetSession(ChannelUri, false, RemotingServices.CurrentRemotingServices) as OOAdvantech.Remoting.RestApi.ClientSessionPart;
                if (clientSessionPart != null)
                    clientSessionPart.ProxyTypes[TypeName] = TypeMetaData;
            }
            return TypeMetaData;
        }

        /// <MetaDataID>{b124f923-40c6-41b4-9d35-6b1b80502f7c}</MetaDataID>
        [JsonProperty(Order = 1)]
        public string Uri { get; set; }
        /// <MetaDataID>{1f97762b-d6e9-4188-bf25-a06bb91634e2}</MetaDataID>

        string _ChannelUri;
        [JsonProperty(Order = 2)]
        public string ChannelUri
        {
            get
            {
                return _ChannelUri;
            }
            set
            {

                _ChannelUri = value;
                if (value != null && value.Contains("ws://127.255"))
                {

                }
            }
        }



        /// <MetaDataID>{727df9e0-09ba-466a-be73-6f487d6edbe1}</MetaDataID>
        [JsonProperty(Order = 3)]
        public string InternalChannelUri { get; set; }
        /// <MetaDataID>{83fc6ec5-c41a-45b2-b5a9-28e2fae8ef3e}</MetaDataID>
        [JsonProperty(Order = 4)]
        public string TypeName { get; set; }


        /// <exclude>Excluded</exclude>
        ProxyType _TypeMetaData;
        /// <MetaDataID>{9482a84a-c570-4485-bf03-f1ade2a993b9}</MetaDataID>
        [JsonProperty(Order = 5)]
        public ProxyType TypeMetaData { get { return _TypeMetaData; } set { _TypeMetaData = value; } }

        /// <MetaDataID>{7d7d326b-5a70-4394-8f10-5f6f6b06ae43}</MetaDataID>
        [JsonProperty(Order = 6)]
        public Dictionary<string, object> MembersValues = new Dictionary<string, object>();
    }

    /// <MetaDataID>{df5548b8-f2c3-4228-9d1f-f0b238c9fcff}</MetaDataID>
    public class SerializedData
    {
        /// <MetaDataID>{a28ed87d-aad2-4f2b-b1ff-9d7574c7aaea}</MetaDataID>
        public ObjRef Ref { get; set; }

        /// <MetaDataID>{06b4cfdc-e5cf-46ef-bad3-91954fe60818}</MetaDataID>
        public object Value { get; set; }



    }

    /// <MetaDataID>{3990d6af-1ab3-4e1e-aa02-1a12b7dfc6fd}</MetaDataID>
    [DataContract()]
    [Serializable]
    public class TransportData
    {

        [DataMember]
        public string RequestOS { get; set; } = MessageDispatcher.OSName;

        /// <MetaDataID>{13136d94-a067-486c-aceb-374a21be83b2}</MetaDataID>
        [DataMember]
        public int CallContextID { get; set; }

        /// <MetaDataID>{5b13284b-9c50-41b2-8a3a-6681502140cd}</MetaDataID>
        [DataMember]
        public string details { get; set; }

        [DataMember]
        public double SendTimeout { get; set; }


        /// <MetaDataID>{e106b000-5aa2-4618-999d-d64d47656cb3}</MetaDataID>
        [DataMember]
        public string SessionIdentity { get; set; }

        [DataMember]
        public string ServerSessionPartID { get; set; }

        [JsonIgnore]
        public ServerSessionPart ServerSession { get; internal set; }

        /// <MetaDataID>{c15885a3-c546-438f-8176-a05679e89ae7}</MetaDataID>
        [DataMember]
        public string ChannelUri { get; set; }


        /// <MetaDataID>{169676a6-5219-4f50-89a6-ec8b6a42ee87}</MetaDataID>
        [JsonIgnore]
        public string InternalChannelUri
        {
            get
            {
                string publicChannelUri = null;
                string internalchannelUri = null;
                ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);
                return internalchannelUri;
            }
        }


        /// <MetaDataID>{6f4b1bd6-0cfe-4ade-b11f-dcd3b5ed8db7}</MetaDataID>
        [JsonIgnore]
        public string PublicChannelUri
        {
            get
            {
                string publicChannelUri = null;
                string internalchannelUri = null;
                ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);
                return publicChannelUri;
            }
        }

    }



    /// <MetaDataID>{bc25c091-1531-461a-b08e-76d75e040d48}</MetaDataID>
    [DataContract()]
    [Serializable]
    public class RequestData : TransportData
    {

     

        [DataMember]
        public string RequestProcess { get; set; } = MessageDispatcher.ProcessName;

        /// <MetaDataID>{c680a894-12c8-4aba-8400-44e91bebdd40}</MetaDataID>
        [DataMember]
        public RequestType RequestType { get; set; }


        /// <summary>
        /// Defines the physical connection ID.
        /// Is the route of request to reach to the server session part. 
        /// </summary>
        [DataMember]
        public string PhysicalConnectionID { get; set; }
        


        /// <MetaDataID>{8dcafe1d-afcf-43f3-90c3-5a4e39a90391}</MetaDataID>
        public IEndPoint EventCallBackChannel;

        /// <MetaDataID>{0b3f0f37-5777-41a7-944e-5d5fe85a114a}</MetaDataID>
        internal Dictionary<string, object> CallContextDictionaryData;
        /// <MetaDataID>{6c32c7f4-35fb-47ec-8ac0-b40097008b7f}</MetaDataID>
        public void SetCallContextData(string name, object data)
        {
            if (CallContextDictionaryData == null)
                CallContextDictionaryData = new Dictionary<string, object>();

            CallContextDictionaryData[name] = data;
        }
        /// <MetaDataID>{672e66f2-94b1-4358-8381-2da0a69074dc}</MetaDataID>
        public object GetCallContextData(string name)
        {
            object data = null;
            if (CallContextDictionaryData != null && CallContextDictionaryData.TryGetValue(name, out data))
                return data;
            return null;
        }

        /// <MetaDataID>{bd1eb8ce-a4bd-4093-915b-d67c0113c414}</MetaDataID>
        public static bool IsRequestMessage(string requestData)
        {
            int nPos = requestData.IndexOf("\"details\":\"");
            if (nPos != -1)
                return true;
            else
                return false;

        }

        //[DataMember]
        //public string ClientProcessIdentity { get; private set; }



        ///// <MetaDataID>{5b5b8f1b-f780-441a-afda-a2f8bc1fc4b3}</MetaDataID>
        //string _InternalChannelUri;
        ///// <MetaDataID>{13390c50-c0f1-4e01-8b43-a17356b98f14}</MetaDataID>
        //[DataMember]
        //public string InternalChannelUri
        //{
        //    get
        //    {
        //        return _InternalChannelUri;
        //    }
        //    set
        //    {
        //        _InternalChannelUri = value;
        //    }
        //}


    }

    /// <MetaDataID>{d6c22f11-8b20-4e17-9b3c-59dda5aca713}</MetaDataID>
    [DataContract()]
    [Serializable]
    public class ResponseData : TransportData
    {

        public ResponseData(string channelUri)
        {
            this.ChannelUri = channelUri;
        }

        public ResponseData()
        {

        }

        /// <MetaDataID>{c1cb197b-786a-4bb3-8c2d-3c3f7efe1e23}</MetaDataID>
        [DataMember]
        public bool BidirectionalChannel { get; set; }
        /// <MetaDataID>{7faf2c00-0c9e-4d38-8d6b-d75f893aeee6}</MetaDataID>
        public bool IsSucceeded { get; internal set; }

        /// <MetaDataID>{457d1db9-152f-453c-b1e9-42dde7c4128f}</MetaDataID>
        [DataMember]
        public bool DirectConnect;

        [JsonIgnore]
        public bool InitCommunicationSession { get; internal set; }

        [DataMember]
        public bool BrokenSession { get; internal set; }

        [DataMember]
        public bool UpdateCaching { get; set; }
    }

    /// <MetaDataID>{8df98a8d-4aed-4e73-b987-34941905fd9d}</MetaDataID>
    public enum RequestType
    {
        MethodCall = 0,
        Event = 1,
        ConnectionInfo = 2,
        Disconnect = 3,
        LagTest=4
    }
}
