using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{c7f6e024-0ce1-41cc-98ec-59ae807be0c4}</MetaDataID>
    public class EventCallbackMessage
    {

        /// <MetaDataID>{760c01ce-e8a6-41d2-ae45-2a7ac6125abe}</MetaDataID>
        public void UnMarshal()
        {


            try
            {
                Args = MethodCallMessage.UnMarshalArguments(JsonArgs, this.EventInfoData.EventInfo.EventHandlerType.GetMethod("Invoke"), null, false);
            }
            catch (Exception error)
            {

                throw;
            }

//#if DeviceDotNet
//            //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web), ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null) };
//            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, Web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
//#else
//            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, Web?JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
//            //{ TypeNameHandling = TypeNameHandling.All, Binder = new SerializationBinder(Web), ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null) };
//            //if(Web)
//            //{
//            //    jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
//            //    jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
//            //}
//#endif 
//            Args = JsonConvert.DeserializeObject<object[]>(JsonArgs, jSetttings);

//            try
//            {
                
//                Args = args;
//            }
//            catch (Exception error)
//            {
//                throw;
//            }
        }
        public bool Web { get; set; }

        /// <MetaDataID>{2c88aa61-4334-4f2d-9dc0-69cb592ccb35}</MetaDataID>
        internal ServerSessionPart ServerSession;
        /// <MetaDataID>{cb42fc05-6200-40ef-a270-dd5b09c951e7}</MetaDataID>
        public EventInfoData EventInfoData;

        /// <MetaDataID>{6529ad37-b8d7-4c15-8e95-dd7f72d9fcc0}</MetaDataID>
        public ExtObjectUri EventPublisherUri;

        /// <MetaDataID>{1063ced5-2166-426b-9552-4705e53945c7}</MetaDataID>
        [JsonIgnore]
        public object[] Args;

        /// <MetaDataID>{554858b7-b908-4e52-924a-9ce54f7d3c96}</MetaDataID>
        public string SessionIdentity;

        /// <MetaDataID>{4b99333b-71d0-46cc-86ae-c004b47f203c}</MetaDataID>
        public string JsonArgs;


        public string[] ParamsNames;

        /// <MetaDataID>{0799db9f-c104-430d-b316-aadaa2067dd6}</MetaDataID>
        internal void Marshal()
        {

            ParamsNames = (from parameter in EventInfoData.EventInfo.EventHandlerType.GetMetaData().GetMethods()[0].GetParameters()
                           select parameter.Name).ToArray();


            //string internalChannelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("internalChannelUri") as string;

#if DeviceDotNet
            //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, SerializationBinder= new OOAdvantech.Remoting.RestApi.SerializationBinder(ServerSession.Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, ServerSession.ChannelUri, ServerSession.InternalChannelUri, ServerSession, ServerSession.Web) };
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, ServerSession.Web?JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, ServerSession.ChannelUri, ServerSession.InternalChannelUri, ServerSession,null);

#else
            var jSetttings = new Serialization.JSonSerializeSettings(JsonContractType.Serialize, ServerSession.Web?JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, ServerSession.ChannelUri, ServerSession.InternalChannelUri, ServerSession,null);// { TypeNameHandling = ServerSession.Web? TypeNameHandling.None:TypeNameHandling.All, Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(ServerSession.Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, ServerSession.ChannelUri, ServerSession.InternalChannelUri, ServerSession, ServerSession.Web) };
#endif


            //if (ServerSession.Web)
            //{
            //    jSetttings.ReferenceResolver = new ReferenceResolver();
            //    jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
            //    jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //}
            JsonArgs = JsonConvert.SerializeObject(Args, jSetttings);

        }


        //        internal void UnMarshal()
        //        {
        //            var serverSessionPart = MessageDispatcher.ServerSessions[ClientProcessIdentity];
        //            this.ExtObjectUri = ExtObjectUri.Parse(ObjectUri, serverSessionPart.ClientProcessIdentity);
        //            this.Object = GetObjectFromUri(ExtObjectUri);
        //            int i = 0;
        //            var parameters = MethodInfo.GetParameters();
        //            Type[] argsTypes = (from parameter in parameters select parameter.ParameterType).ToArray();
        //#if DeviceDotNet
        //            var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, ChannelUri, InternalChannelUri, serverSessionPart, argsTypes, Web), SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web) };
        //#else
        //            var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, ChannelUri, InternalChannelUri, serverSessionPart, argsTypes, Web), Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web) };
        //#endif


        //            Args = JsonConvert.DeserializeObject<object[]>(JsonArgs, jSetttings);
        //            ArgCount = Args.Length;
        //            foreach (var argValue in Args)
        //            {
        //                Type parameterType = parameters[i].ParameterType;
        //                if (parameterType.GetMetaData().IsPrimitive)
        //                    Args[i] = Convert.ChangeType(Args[i], parameterType);
        //                else if (parameterType == typeof(decimal))
        //                    Args[i] = Convert.ChangeType(Args[i], parameterType);
        //                else if (parameterType.GetMetaData().IsEnum && Args[i] is long)
        //                    Args[i] = (int)(long)Args[i];
        //                else if (parameterType.GetMetaData().IsEnum && Args[i] is ulong)
        //                    Args[i] = (int)(ulong)Args[i];
        //                i++;
        //            }
        //        }

    }
}
