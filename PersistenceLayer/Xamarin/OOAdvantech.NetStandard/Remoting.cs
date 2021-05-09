#if DeviceDotNet 
using System;

#if PORTABLE
namespace System.Runtime.Serialization
{
    /// <MetaDataID>{b5e1761f-d9e3-4868-9c95-ed66b7bf5409}</MetaDataID>
    public interface IDeserializationCallback
    {

    }
    /// <MetaDataID>{c1255d64-7e9c-4f7f-af63-b7e597373f8b}</MetaDataID>
    public interface ISerializable
    {
    }
}
#endif

//namespace System
//{
//    public class SerializableAttribute : System.Attribute
//    {
//    }
 
  
//}
namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{2fcf5bc5-719b-4a48-b8c1-6d925b4b759c}</MetaDataID>
    public interface IPersistentObjectLifeTimeController
    {
        string GetPersistentObjectUri(object obj);
    }
    ///// <MetaDataID>{eb58c59c-8e73-4cc4-8637-26b50dbfc525}</MetaDataID>
    //public class MonoStateClass : System.Attribute
    //{
    //}
    ///// <MetaDataID>{722a4957-0b4e-42ce-8e0b-c7abb243b931}</MetaDataID>
    //public struct ServerSessionPartInfo
    //{
    //    public IServerSessionPart ServerSessionPart;
    //    public Guid ServerProcessIdentity;
    //}
    ///// <MetaDataID>{f01f4074-5413-4e77-ba07-ffd15fa8025f}</MetaDataID>
    //public interface IRemotingServices
    //{
    //    ServerSessionPartInfo GetServerSession(string channelUri, Guid processIdentity);
    //}

    /// <MetaDataID>{32b01ced-d7cd-42d3-a341-1012bdb852e6}</MetaDataID>
    //public class RemotingServices
    //{

    //    /// <MetaDataID>{7e6c4772-5ef4-4153-9ae7-3b09598b8259}</MetaDataID>
    //    public static System.Guid ProcessIdentity = System.Guid.NewGuid();

    //    public static bool IsOutOfProcess(MarshalByRefObject marshalByRefObject)
    //    {
    //        return false;
    //    }

    //    public static RemotingServices GetRemotingServices(object obj)
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }

    //    public object CreateInstance(string classFullName)
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }
    //    public static T CreateInstance<T>(string ChannelUri, System.Type[] paramsTypes, params object[] ctorParams) where T : class
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }
    //    public static T CreateRemoteInstance<T>(string ChannelUri, System.Type[] paramsTypes, params object[] ctorParams) where T : class
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }
    //    public static object CreateInstance(string ChannelUri, string TypeFullName, string assemblyData)
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }

    //    public static object CreateRemoteInstance(string ChannelUri, string TypeFullName, string assemblyData)
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }

        
    //    public static string GetChannelUri(MarshalByRefObject marshalByRefObject)
    //    {
    //        throw new System.Exception("The method or operation is not implemented.");
    //    }
    //}

}

#endif
