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


    /// <MetaDataID>{e09c9ca5-ad00-4855-8697-364c875cc2ba}</MetaDataID>
    [AttributeUsage(AttributeTargets.Property)]
    public class OnDemandCachingDataOnClientSide : System.Attribute
    {

    }
    /// <MetaDataID>{7a64cf13-18c8-4cc7-a273-585e6c77a52b}</MetaDataID>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Field)]
    public class CachingOnlyReferenceOnClientSide : System.Attribute
    {

    }
}
