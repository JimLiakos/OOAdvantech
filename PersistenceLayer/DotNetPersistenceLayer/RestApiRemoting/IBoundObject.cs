﻿using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DeviceDotNet
using Xamarin.Essentials;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{505ef4bf-5829-4391-8d08-30293a919292}</MetaDataID>
    [HttpVisible]
    public interface IBoundObject
    {
        /// <MetaDataID>{a18e58a4-0272-415d-b81f-d9e328aa1973}</MetaDataID>
        MarshalByRefObject GetObjectFromUri(string uri);

    }

    [HttpVisible]
    public interface INativeConsole
    {
       void Log(List<string> lines);
    }
}
