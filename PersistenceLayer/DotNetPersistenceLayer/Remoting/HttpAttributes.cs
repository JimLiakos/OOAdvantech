using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{fbe20c8f-af60-4910-a602-09c5935c9ca0}</MetaDataID>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property| AttributeTargets.Event)]
    public class HttpVisible : System.Attribute
    {

    }

    /// <MetaDataID>{529b1cb0-496a-4c73-894f-cc650f8ba870}</MetaDataID>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property| AttributeTargets.Event)]
    public class HttpInVisible : System.Attribute
    {
         
    }


    /// <MetaDataID>{529b1cb0-496a-4c73-894f-cc650f8ba870}</MetaDataID>
    [AttributeUsage(AttributeTargets.Property)]
    public class CachingDataOnClientSide : System.Attribute
    {

    }
}
