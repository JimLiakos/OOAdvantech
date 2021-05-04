using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AssemblyNativeCode
{
#if !NETCompactFramework 
    /// <MetaDataID>{d41f2712-1231-40d6-ab99-8aba2ba5d04c}</MetaDataID>
    [Guid("99B2EC28-E334-4021-BA29-B430A41FA431")]
    [ComVisible(true)]
    public interface INativeCodeBridge
    {
        /// <MetaDataID>{b22151a6-04e5-4fbf-8d69-735578f79ebd}</MetaDataID>
        System._AppDomain CurrentDomain
        {
            get; 
        }
        /// <MetaDataID>{2ead714f-f0c7-42fb-a5ff-eb2950dd522b}</MetaDataID>
        void AssemblyLoaded(System.Reflection.Assembly assembly);
 

    }
    /// <MetaDataID>{6176f243-97a0-404f-9a76-a21b993f4bbf}</MetaDataID>
    [Guid("1A7637F7-A08B-4f0f-AE03-55F2D8E0D545")]
    [ComVisible(true)]
    public class NativeCodeBridge : INativeCodeBridge
    {
        public AssemblyLoader AssemblyLoader;

        public _AppDomain CurrentDomain
        {
            get
            {
                

                return System.AppDomain.CurrentDomain;
            }
        }
        /// <MetaDataID>{8b4279f8-953a-4632-8e9e-acc660327fda}</MetaDataID>
        public  System.Reflection.Assembly LoadedAssembly;
        public void AssemblyLoaded(System.Reflection.Assembly assembly)
        {
            LoadedAssembly = assembly;
        }


    }
#endif
}
