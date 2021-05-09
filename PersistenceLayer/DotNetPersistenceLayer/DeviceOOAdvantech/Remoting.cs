#if NETCompactFramework 
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
    /// <MetaDataID>{eb58c59c-8e73-4cc4-8637-26b50dbfc525}</MetaDataID>
    public class MonoStateClass : System.Attribute
    {
    }
    /// <MetaDataID>{3ae3f9eb-d320-4745-8d3b-a7f8521b97bc}</MetaDataID>
    public interface IExtMarshalByRefObject
    {
    }
    /// <MetaDataID>{32b01ced-d7cd-42d3-a341-1012bdb852e6}</MetaDataID>
    public class RemotingServices
    {
     

 

        public static bool IsOutOfProcess(System.MarshalByRefObject marshalByRefObject)
        {
            return false;
        }

        public static RemotingServices GetRemotingServices(object obj)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public object CreateInstance(string classFullName)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public static object GetChannelUri(System.MarshalByRefObject marshalByRefObject)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }

}

#endif
