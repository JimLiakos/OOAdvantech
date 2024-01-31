using System;
#if !DeviceDotNet

#else
using OOAdvantech.Json;
#endif

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{B1B4AE3C-282C-49A0-8016-C390E38BDB75}</MetaDataID>
    public interface IExtMarshalByRefObject
    {
    }

    /// <MetaDataID>{62883c86-ca3c-430c-9029-2635031f6d6d}</MetaDataID>
    public class ExtMarshalByRefObject : MarshalByRefObject, IExtMarshalByRefObject
    {

    }

}
