using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXConnectableControls
{
    /// <MetaDataID>{f6958f2a-6f50-47ba-b81a-04c39dae1f9d}</MetaDataID>
    public class DevXpressModules
    {
        
        public static bool ModulesLoaded;
        public static void LoadModules()
        {

            if (!ModulesLoaded)
            {
                AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
                ModulesLoaded = true;
            }
            
            
            
;
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            //if (args.LoadedAssembly.FullName.IndexOf("DevExpress.") == 0)
            //{
            //    string assemblyFileName = args.LoadedAssembly.FullName;
            //    string gacLocation = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\";

            //    string fileName = null;
            //    string arguments = null;
            //    try
            //    {
            //        if (System.IO.File.Exists(gacLocation + "gacutil.exe"))
            //        {
            //            fileName = gacLocation + "gacutil.exe";
            //            arguments = "-u \"" + assemblyFileName + "\"";

            //        }
            //        else
            //        {
            //            fileName = gacLocation + "gacutil.exe";
            //            arguments = "-u \"" + assemblyFileName + "\"";
            //        }

            //    }
            //    catch (Exception error)
            //    {


            //    }


            //}


        }

 
      
    }
}
