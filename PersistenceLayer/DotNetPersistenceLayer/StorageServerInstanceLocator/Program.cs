using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace StorageServerInstanceLocator
{
    /// <MetaDataID>{ca924a71-1d19-4a26-ae6b-8e3f4efa96f4}</MetaDataID>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static void Main(string[] args)
        {


            if (args != null && args.Length == 1 && args[0].Length > 1
          && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "install":
                    case "i":
                        SelfInstaller.InstallMe();
                        break;
                    case "uninstall":
                    case "u":
                        SelfInstaller.UninstallMe();
                        break;
                    case "console":
                    case "c":
                        {
                            RunService();
                            break;
                        }
                }
            }
            else
            {
                RunService();
            }
        }


        static void RunService()
        {
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new InstanceLocator(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new InstanceLocator() };

            ServiceBase.Run(ServicesToRun);
        }
    }
}