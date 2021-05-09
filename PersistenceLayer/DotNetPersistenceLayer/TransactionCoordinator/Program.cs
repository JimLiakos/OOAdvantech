using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace TransactionCoordinator
{
    /// <MetaDataID>{1a6af43b-e0cc-4b6d-9442-a4e42a0f0978}</MetaDataID>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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

        private static void RunService()
        {
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new TransactionCoordinatorService() };

            ServiceBase.Run(ServicesToRun);
        }
    }
}