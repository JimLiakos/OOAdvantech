using System;

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{432772b7-e6e6-40fa-b9c5-bccf3f0164a0}</MetaDataID>
    [AttributeUsage(AttributeTargets.Event)]

    public class RemoteEventPublishAttribute : System.Attribute
    {
       public readonly InvokeType InvokeType = InvokeType.Async;
        public RemoteEventPublishAttribute(InvokeType invokeType)
        {
            InvokeType = invokeType;
        }
    }

    /// <MetaDataID>{d984aa57-657e-43e0-9fdb-0370fbbfed01}</MetaDataID>
    public enum InvokeType
    {
        Async = 1,
        Sync = 2
    }
}