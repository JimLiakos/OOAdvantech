using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AssemblyNativeCode
{ 
    /// <MetaDataID>{ffc67b92-a8cd-4c24-b896-d164fec68ae6}</MetaDataID>
    public class AssemblyLoader
    { 
        class NativeCodeAssemblyLoader
        {
            [DllImport(@"NativeCodeAsseblyLoader.dll", CharSet = CharSet.Auto)]
            public static extern int LoadAssembly(string fileName, object nativeCodeBridgeObject);

            [DllImport(@"NativeCodeAsseblyLoader.dll", CharSet = CharSet.Auto)]
            public static extern int LoadAssemblyDebug(string assemblyFileName,string debugInfoFileName, object nativeCodeBridgeObject);

            [DllImport(@"NativeCodeAsseblyLoader.dll", CharSet = CharSet.Auto)]
            public static extern int MakeNative(string fileName);

            [DllImport(@"NativeCodeAsseblyLoader.dll", CharSet = CharSet.Auto)]
            public static extern int LoadRawAssembly(byte[] rawAssembly, int rawAssemblySize, object nativeCodeBridgeObject);

            [DllImport(@"NativeCodeAsseblyLoader.dll", CharSet = CharSet.Auto)]
            public static extern int LoadRawAssemblyDebug(byte[] rawAssembly, int rawAssemblySize,byte[] rawDebug, int rawDebugSize, object nativeCodeBridgeObject);

 
        }

        public static System.Reflection.Assembly LoadRawAssembly(byte[] rawAssembly)
        {
            NativeCodeBridge nativeCodeBridge = new NativeCodeBridge();
            // nativeCodeBridge.AssemblyLoader = this;

            NativeCodeAssemblyLoader.LoadRawAssembly(rawAssembly, rawAssembly.Length, nativeCodeBridge);
            return nativeCodeBridge.LoadedAssembly;
        }

        public static System.Reflection.Assembly LoadRawAssemblyDebug(byte[] rawAssembly, byte[] rawDebugInfo)
        {
            NativeCodeBridge nativeCodeBridge = new NativeCodeBridge();
            // nativeCodeBridge.AssemblyLoader = this;

            NativeCodeAssemblyLoader.LoadRawAssemblyDebug(rawAssembly, rawAssembly.Length,rawDebugInfo,rawDebugInfo.Length, nativeCodeBridge);
            return nativeCodeBridge.LoadedAssembly;
        }
        /// <MetaDataID>{e7504c31-e12a-4a58-95eb-9c5622b159ee}</MetaDataID>
        public static System.Reflection.Assembly LoadAssembly(string fileName)
        {

            if (System.IO.File.Exists(fileName))
            {

#if !DeviceDotNet                 
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                string debugInfoFile = null;
                if (!string.IsNullOrEmpty(fileInfo.Extension))
                    debugInfoFile = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(fileInfo.Extension)) + ".pdb";
                else
                    debugInfoFile = fileInfo.DirectoryName + @"\" + fileInfo.Name + ".pdb";

                NativeCodeBridge nativeCodeBridge = new NativeCodeBridge();
               // nativeCodeBridge.AssemblyLoader = this;

                if (System.IO.File.Exists(debugInfoFile))
                    NativeCodeAssemblyLoader.LoadAssemblyDebug(fileName, debugInfoFile, nativeCodeBridge);
                else
                    NativeCodeAssemblyLoader.LoadAssembly(fileName, nativeCodeBridge);
                return nativeCodeBridge.LoadedAssembly;
#else
                return System.Reflection.Assembly.LoadFrom(fileName);
#endif
            }
            else
                throw new System.Exception("Assembly " + fileName + " daoesn't exist");

        }

        /// <MetaDataID>{efa69bb8-71ec-4c56-ad49-67a2fd82eaa7}</MetaDataID>
        public static void MakeNadive(string fileName)
        {
            NativeCodeAssemblyLoader.MakeNative(fileName);
        }




    }
}
