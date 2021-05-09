using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.ServiceProcess;
using System.Drawing;
using OOAdvantech.PersistenceLayer;
using System.Threading.Tasks;


namespace PersistencyService
{
    /// <MetaDataID>{9ebae1f1-195c-4de6-a110-2f83c1b0922f}</MetaDataID>
    public class ServiceInitiator : MarshalByRefObject
    {
        public void Init()
        {
            try
            {
          

                string ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "PersistencyService.config";
                if (!System.IO.File.Exists(ConfigFileName))
                {
                    System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
                    XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
                    XmlDocument.Save(ConfigFileName);
                }
                System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName, false);

                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);

                //Force Transaction assembly load
                System.Type type = typeof(OOAdvantech.Transactions.SystemStateTransition);

                System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
                string TCPPort = storageServerConfig.DocumentElement.GetAttribute("TCPPort");

                OOAdvantech.Remoting.RemotingServices.RegisterSecureTcpChannel(int.Parse(TCPPort), true);
                OOAdvantech.PersistenceLayer.IPersistencyService persistencyService = ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b", "PersistenceLayerRunTime, Culture=neutral, PublicKeyToken=95eeb2468d93212b", insatnceName, new Type[1] { typeof(string) }) as OOAdvantech.PersistenceLayer.IPersistencyService;


                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnCatchUnhandledException);
                //try
                //{
                //    OOAdvantech.PersistenceLayer.IStorageServerInstanceLocator instanceLocator=OOAdvantech.Remoting.RemotingServices.CreateInstance("tcp://"+System.Net.Dns.GetHostName()+":4500","OOAdvantech.PersistenceLayerRunTime.StorageServerInstanceLocator") as OOAdvantech.PersistenceLayer.IStorageServerInstanceLocator;
                //    int port=instanceLocator.GetInstancePort(System.Net.Dns.GetHostName()+"\\Default");
                //    instanceLocator.AddInstance(4000,System.Net.Dns.GetHostName()+"\\Default");
                //    port=instanceLocator.GetInstancePort(System.Net.Dns.GetHostName()+"\\Default");
                //}
                //catch(System.Exception error)
                //{
                //}


                //System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime=TimeSpan.FromSeconds(20);
                //System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime=TimeSpan.FromSeconds(10);
                //System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout=TimeSpan.FromSeconds(10);/**/
            }
            catch (System.Exception Error)
            {

                //TODO  prone γεμισει με message το log file τοτε παράγει exception
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                    myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);


                System.Diagnostics.Debug.WriteLine(
                    Error.Message + Error.StackTrace);
                //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                throw Error;


                int hh = 0;

            }

        }

        void OnCatchUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            int ttr = 0;

        }
    }
    /// <MetaDataID>{091a9e76-6676-4e19-a5a2-441207ff1a20}</MetaDataID>
    public class PersistencyService : System.ServiceProcess.ServiceBase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public PersistencyService()
        {

            // This call is required by the Windows.Forms Component Designer.
            InitializeComponent();

        }
        private void ShowFormSelect(object sender, System.EventArgs e)
        {
            int hh = 0;
        }

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


        // The main entry point for the process
        static void RunService()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new PersistencyService() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
            storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");

            string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
            string TCPPort = storageServerConfig.DocumentElement.GetAttribute("TCPPort");
            this.ServiceName = "StorageServer";
            if (insatnceName != null && insatnceName.Trim().ToLower() != "default")
                this.ServiceName += "$" + insatnceName.Trim();
          
            
            

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            //System.Windows.Forms.MessageBox.Show("in OnStart");


            bool Block = false;
            while (Block)
            {
                System.Threading.Thread.Sleep(1000);
            }

            try
            {
                string regKeyPath = @"Software\HandySoft\StorageServerInstances";
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKeyPath);
                if (registryKey != null)
                {
                    if ((registryKey.GetValue("DebugBlock") is string) && (registryKey.GetValue("DebugBlock") as string).ToLower() == "true")
                        Block = true;
                    registryKey.Close();
                }
            }
            catch (System.Exception error)
            {
            }

            ModulePublisher.ClassRepository.DontUseRegistryClassesRoot = true;

            //try
            //{
            //    AppDomain secDom = AppDomain.CreateDomain("SecondaryDomain");
            //    ServiceInitiator serviceInitiator = (ServiceInitiator)secDom.CreateInstance(this.GetType().Assembly.FullName, "PersistencyService.ServiceInitiator").Unwrap();
            //    serviceInitiator.Init();
            //}
            //catch (System.Exception Error)
            //{
            //    throw Error;
            //}
            //return;
            try
            {
                string ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "PersistencyService.config";
                if (!System.IO.File.Exists(ConfigFileName))
                {
                    System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
                    XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
                    XmlDocument.Save(ConfigFileName);
                }
                System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName);

                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);

                //Force Transaction assembly load
                System.Type type = typeof(OOAdvantech.Transactions.SystemStateTransition);

                System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");

                string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
                string TCPPort = storageServerConfig.DocumentElement.GetAttribute("TCPPort");


                OOAdvantech.Remoting.RemotingServices.RegisterSecureTcpChannel(int.Parse(TCPPort), true);
                OOAdvantech.PersistenceLayerRunTime.TransactioContextProvider.Impersonate = true;

                OOAdvantech.Remoting.RemotingServices.RegisterIpcClientChannel();
                System.Xml.XmlElement loadAssembliesElement = storageServerConfig.DocumentElement.SelectSingleNode("LoadAssemblies") as System.Xml.XmlElement;
                if (loadAssembliesElement != null)
                {
                    foreach (System.Xml.XmlElement assembly in loadAssembliesElement.SelectNodes("Assembly"))
                        ModulePublisher.ClassRepository.LoadAssembly(assembly.GetAttribute("FullName"));
                }
                // AppDomain.CurrentDomain.BaseDirectory+ this.ServiceName+.

                //OOAdvantech.Remoting.RemotingServices.RegisterChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), true);//regiser ipc
                OOAdvantech.PersistenceLayer.IPersistencyService persistencyService = OOAdvantech.AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"), new Type[1] { typeof(string) }, insatnceName) as OOAdvantech.PersistenceLayer.IPersistencyService;


                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnCatchUnhandledException);
                OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.InStorageService = true;
                string metaDataFileName = AppDomain.CurrentDomain.BaseDirectory + this.ServiceName.Replace("$", "_") + "MetaData.xml";
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.OpenStorage(this.ServiceName.Replace("$", "_") + "MetaData", metaDataFileName, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));

                var storageServerCollection = (from storageServer in storage.GetObjectCollection<OOAdvantech.MetaDataRepository.StorageServer>()
                                               select storageServer).ToArray();
                if (storageServerCollection.Length == 0)
                {
                    OOAdvantech.PersistenceLayerRunTime.StorageServer storageServer = new OOAdvantech.PersistenceLayerRunTime.StorageServer();
                    storageServer.Name = this.ServiceName;
                    storage.ObjectStorage.CommitTransientObjectState(storageServer);

                    (OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService as OOAdvantech.PersistenceLayerRunTime.PersistencyService).StorageServer = storageServer;
                }
                else
                    (OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService as OOAdvantech.PersistenceLayerRunTime.PersistencyService).StorageServer = storageServerCollection[0];


                if ((OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService as OOAdvantech.PersistenceLayerRunTime.PersistencyService).StorageServer != null)
                {

                    Parallel.ForEach((OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService as OOAdvantech.PersistenceLayerRunTime.PersistencyService).StorageServer.Storages, storageRef =>
                    {
                        try
                        {
                            OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(storageRef.StorageName, storageRef.StorageLocation, storageRef.StorageType);
                        }
                        catch (Exception error)
                        {


                        }
                    });
                }
                //try
                //{
                //    OOAdvantech.PersistenceLayer.IStorageServerInstanceLocator instanceLocator=OOAdvantech.Remoting.RemotingServices.CreateInstance("tcp://"+System.Net.Dns.GetHostName()+":4500","OOAdvantech.PersistenceLayerRunTime.StorageServerInstanceLocator") as OOAdvantech.PersistenceLayer.IStorageServerInstanceLocator;
                //    int port=instanceLocator.GetInstancePort(System.Net.Dns.GetHostName()+"\\Default");
                //    instanceLocator.AddInstance(4000,System.Net.Dns.GetHostName()+"\\Default");
                //    port=instanceLocator.GetInstancePort(System.Net.Dns.GetHostName()+"\\Default");
                //}
                //catch(System.Exception error)
                //{
                //}


                //System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime=TimeSpan.FromSeconds(20);
                //System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime=TimeSpan.FromSeconds(10);
                //System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout=TimeSpan.FromSeconds(10);/**/
            }
            catch (System.Exception Error)
            {

                //TODO prone γεμισει με message το log file τοτε παράγει exception
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                    myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);


                System.Diagnostics.Debug.WriteLine(
                    Error.Message + Error.StackTrace);
                //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                throw Error;


                int hh = 0;

            }


        }

        void OnCatchUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            int tt = 0;

        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {

            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
