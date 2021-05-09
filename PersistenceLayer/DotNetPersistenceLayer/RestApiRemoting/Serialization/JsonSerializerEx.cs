using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Json.Linq;
using OOAdvantech.Json.Serialization;

namespace OOAdvantech.Remoting.RestApi.Serialization
{

    /// <MetaDataID>{798c2871-32c1-4e06-8a54-633cc65e8091}</MetaDataID>
    public class JsonSerializerEx : OOAdvantech.Json.JsonConverter
    {
        JsonContractType JsonContructType;

        SerializeSession SerializeSession;
        //private string ChannelUri;
        //private string InternalChannelUri;
        private OOAdvantech.Remoting.RestApi.ServerSessionPart ServerSessionPart;
        JsonSerializationFormat SerializationFormat;
        public JsonSerializerEx(JsonContractType jsonContructType, SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, JsonSerializationFormat serializationFormat = JsonSerializationFormat.NetTypedValuesJsonSerialization)
        {
            SerializeSession = serializeSession;
            JsonContructType = jsonContructType;
            //this.ChannelUri = channelUri;
            // this.InternalChannelUri = internalChannelUri;
            this.ServerSessionPart = serverSessionPart;
            SerializationFormat = serializationFormat;
        }

        public override bool CanConvert(Type objectType)
        {
            bool isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(IExtMarshalByRefObject)));

            isMarshalByRefObject |= OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(OOAdvantech.Remoting.RestApi.Proxy)));

            isMarshalByRefObject = false;
            return isMarshalByRefObject;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jProxy = JObject.Load(reader);
            var jRealObject = jProxy.Property("RealObject").Value as JObject;
            var realObject = jRealObject.ToObject(typeof(object), serializer);
            if (realObject is ObjRef)
            {
                ObjRef objRef = realObject as ObjRef;

                string sessionChannelUri = null;// ChannelUri;
                if (ServerSessionPart != null)
                    sessionChannelUri = ServerSessionPart.ChannelUri;
                //if (!string.IsNullOrWhiteSpace(InternalChannelUri))
                //    sessionChannelUri += "(" + InternalChannelUri + ")";


                if (objRef.ChannelUri == sessionChannelUri || (!string.IsNullOrWhiteSpace(objRef.InternalChannelUri) && ServerSessionPart != null && objRef.InternalChannelUri == ServerSessionPart.InternalChannelUri))
                {
                    if (objRef.ChannelUri != sessionChannelUri)
                    {

                    }
                    var extObjectUri = ExtObjectUri.Parse(objRef.Uri, ServerSessionPart.ServerProcessIdentity);
                    //realObject = MethodCallMessage.GetObjectFromUri(extObjectUri);
                    realObject = ServerSessionPart.GetObjectFromUri(extObjectUri);

                    if (realObject == null)
                        throw new System.Exception("The object with ObjUri '" + extObjectUri.TransientUri + "' has been disconnected or does not exist at the server.");


                }
                else
                {


                    OOAdvantech.Remoting.ClientSessionPart clientSessionPart = RenewalManager.GetSession(objRef.ChannelUri, true, RemotingServices.CurrentRemotingServices);

                    OOAdvantech.Remoting.RestApi.Proxy restapiProxy = null;
                    lock (clientSessionPart)
                    {
                        restapiProxy = clientSessionPart.GetProxy(ExtObjectUri.Parse(objRef.Uri, clientSessionPart.ServerProcessIdentity).Uri ) as Proxy;
                        if (restapiProxy == null)
                        {
                            restapiProxy = new Proxy(objRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                            restapiProxy.ControlRemoteObjectLifeTime();

                        }
                        else
                        {
                            restapiProxy.ReconnectToServerObject(objRef);
                        }
                    }
                    realObject = restapiProxy.GetTransparentProxy(objectType);
                }
            }

            //Person per = realObject as Person;
            //per.Name = per.Name + "_1";

            OOAdvantech.Json.Serialization.Proxy proxy = new OOAdvantech.Json.Serialization.Proxy();
            proxy.RealObject = realObject;

            return proxy;
        }

        public override void WriteJson(JsonWriter writer, object _obj, JsonSerializer serializer)
        {
            string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(_obj as MarshalByRefObject);
            JToken token = null;
            if (uri == null)
            {
                var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_obj as MarshalByRefObject) as OOAdvantech.Remoting.RestApi.Proxy;
                if (proxy != null)
                {
                    if (ServerSessionPart != null)
                        token = GetObjectRefToken(ServerSessionPart.TransformToPublic(proxy.ObjectRef), serializer);
                    else
                        token = GetObjectRefToken(proxy.ObjectRef, serializer);


                    token.WriteTo(writer);
                    return;
                }
                uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;
                //System.Runtime.Remoting.RemotingServices.Unmarshal()
            }
            OOAdvantech.MetaDataRepository.ProxyType httpProxyType = null;
            Type type = _obj.GetType();
            bool typeAlreadyMarshaled = false;
            lock (ServerSessionPart)
            {
                if (ServerSessionPart == null || !ServerSessionPart.MarshaledTypes.TryGetValue(type.AssemblyQualifiedName, out httpProxyType))
                {
                    httpProxyType = new OOAdvantech.MetaDataRepository.ProxyType(type);
                    if (ServerSessionPart != null)
                        ServerSessionPart.MarshaledTypes[type.AssemblyQualifiedName] = httpProxyType;
                }
                else
                {
                    if (httpProxyType.Paired)
                        typeAlreadyMarshaled = true;

                }
            }

            ObjRef byref = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);
            byref.CachingObjectMemberValues(_obj);
            if (typeAlreadyMarshaled)
                byref.TypeMetaData = null;
            token = GetObjectRefToken(byref, serializer);
            token.WriteTo(writer);
        }

        private static JToken GetObjectRefToken(object _obj, JsonSerializer serializer)
        {


            JToken token = null;
            Type type = _obj.GetType();
            JsonSerializeObjtRef objectRef = new JsonSerializeObjtRef();
            OOAdvantech.Json.Serialization.Proxy proxy = new OOAdvantech.Json.Serialization.Proxy();
            objectRef.Value = proxy;
            token = null;
            token = JToken.FromObject(objectRef, serializer);
            JObject jObjectRef = (JObject)token;
            var jProxy = jObjectRef.Property("Value").Value as JObject;

            System.IO.StringWriter text = new System.IO.StringWriter();
            serializer.Serialize(text, _obj);

            string json = text.ToString();
            var jRealObject = JToken.Parse(json);

            //var jRealObject = JToken.FromObject(_obj);
            jProxy.Property("RealObject").Value = jRealObject;
            //string typeData = string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
            //(jRealObject as JObject).AddFirst(new JProperty("$type", typeData));

            return token;
        }
    }
}
