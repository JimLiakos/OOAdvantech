using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using OOAdvantech.PersistenceLayer;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
namespace PersistencyService
{
    /// <summary>
    /// Summary description for ProjectInstaller.
    /// </summary>
    /// <MetaDataID>{c8459556-8738-41ca-b2c9-ec633848f28b}</MetaDataID>
	[RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		protected override void OnAfterInstall(IDictionary savedState)
		{


            PrincipalContext context = new PrincipalContext(ContextType.Machine);
            GroupPrincipal group = null;

            // Create a PrincipalSearcher object.     
            PrincipalSearcher oPrincipalSearcher = new PrincipalSearcher(new GroupPrincipal(context));

            // Searches for all groups named "Administrators".
            PrincipalSearchResult<Principal> oPrincipalSearchResult = oPrincipalSearcher.FindAll();

            foreach (GroupPrincipal oResult in oPrincipalSearchResult)
            {
                if (oResult.Name == "OSM_STOR")
                    group = oResult;
            }
            if (group == null)
            {
                group = new GroupPrincipal(context);
                group.Name = "OSM_STOR";
                group.Save();
                var entry = (DirectoryEntry)group.GetUnderlyingObject();
                entry.Properties["description"].Add("Microneme Object State Managment System");
                entry.CommitChanges();
            }

            string metaDataFileName = AppDomain.CurrentDomain.BaseDirectory + this.serviceInstaller.ServiceName.Replace("$", "_") + "MetaData.xml";
            if (!System.IO.File.Exists(metaDataFileName))
                ObjectStorage.NewStorage(this.serviceInstaller.ServiceName.Replace("$","_") + "MetaData", metaDataFileName, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

			int gg=0;
		}

		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller serviceInstaller;
		/// <summary>
		/// Required designer variable.
		
		private System.ComponentModel.Container components = null;

		public ProjectInstaller()
		{
			//System.Windows.Forms.MessageBox.Show("location");
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
        bool HasCreateRegistryKey = false;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
        protected override void OnBeforeInstall(IDictionary savedState)
        {

            System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
            storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
            string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
            string TCPPort = storageServerConfig.DocumentElement.GetAttribute("TCPPort").Trim();;
            foreach (string registerInstanceName in StorageServerInstanceLocator.Current.GetStorageServerInstances())
            {
                if (registerInstanceName.Trim().ToLower() == insatnceName.ToLower())
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine("ERROR: The instance '" + insatnceName + "' already exist.");
                    System.Console.WriteLine("");
                    throw new System.Exception("ERROR: The instance '" + insatnceName + "' already exist.");
                }
                if (StorageServerInstanceLocator.Current.GetTCPPort(registerInstanceName) == TCPPort)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine("ERROR: The TCP Port: " + TCPPort + "  belongs to '" + registerInstanceName + "' and it can't be used from '" + insatnceName + "' instance.");
                    System.Console.WriteLine("");
                    throw new System.Exception("ERROR: The TCP Port: " + TCPPort + "  belongs to '" + registerInstanceName + "' and it can't be used from '" + insatnceName + "' instance.");
                }

            }

            String regKeyPath = null;
            if (insatnceName != null && insatnceName.Trim().ToLower() != "default")
            {
                regKeyPath = @"Software\HandySoft\StorageServerInstances\" + insatnceName;
                //this.serviceInstaller.ServiceName += "$" + insatnceName.Trim();
            }
            else
                regKeyPath = @"Software\HandySoft\StorageServerInstances\Default";

            Microsoft.Win32.RegistryKey storageServerKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKeyPath, true);
            if (storageServerKey == null)
                storageServerKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(regKeyPath);
            storageServerKey.SetValue("TCPPort", TCPPort);
            HasCreateRegistryKey = true;
            storageServerKey.SetValue("StorageServerInstanceconfigPath", AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");



            storageServerKey.Close();

            base.OnBeforeInstall(savedState);
        }
        protected override void OnAfterRollback(IDictionary savedState)
        {
            if (HasCreateRegistryKey)
            {
                try
                {
                    System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                    storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                    string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
                    string TCPPort = storageServerConfig.DocumentElement.GetAttribute("TCPPort").Trim(); ;

                    String regKeyPath = null;
                    if (insatnceName != null && insatnceName.Trim().ToLower() != "default")
                    {
                        regKeyPath = @"Software\HandySoft\StorageServerInstances\" + insatnceName;
                        //this.serviceInstaller.ServiceName += "$" + insatnceName.Trim();
                    }
                    else
                        regKeyPath = @"Software\HandySoft\StorageServerInstances\Default";
                    Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(regKeyPath);

                }
                catch (Exception error)
                {
                }
            }
            base.OnAfterRollback(savedState);
        }    
        protected override void OnAfterUninstall(IDictionary savedState)
        {

            try
            {
                

                System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
                string TCPPort = storageServerConfig.DocumentElement.GetAttribute("TCPPort").Trim(); ;

                String regKeyPath = null;
                if (insatnceName != null && insatnceName.Trim().ToLower() != "default")
                {
                    regKeyPath = @"Software\HandySoft\StorageServerInstances\" + insatnceName;
                    //this.serviceInstaller.ServiceName += "$" + insatnceName.Trim();
                }
                else
                    regKeyPath = @"Software\HandySoft\StorageServerInstances\Default";
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(regKeyPath);

            }
            catch (Exception error)
            {
            }

            base.OnAfterUninstall(savedState);
        }


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// 
		private void InitializeComponent()
		{
            System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
            storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
            string insatnceName = storageServerConfig.DocumentElement.GetAttribute("InstanceName");
            string TCPPort=storageServerConfig.DocumentElement.GetAttribute("TCPPort");

            
            


			if(!EventLog.SourceExists("PersistencySystem"))
				EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
			//return ;
			this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
			this.serviceInstaller.StartType=System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// serviceProcessInstaller
			// 

			this.serviceProcessInstaller.Account=System.ServiceProcess.ServiceAccount.LocalSystem;
			//this.serviceProcessInstaller.Password = "Astraxan";
			//this.serviceProcessInstaller.Username = "Jim";
			// 
			// serviceInstaller
			// 
            
            //TODO θα πρέπει να γίνεται από administrator user
			this.serviceInstaller.ServiceName = "StorageServer";
            if (insatnceName != null && insatnceName.Trim().ToLower() != "default")
            {
                this.serviceInstaller.ServiceName += "$" + insatnceName.Trim();
                //this.serviceInstaller.ServiceName += "$" + insatnceName.Trim();
            }
          	
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
																					  this.serviceProcessInstaller,
																					  this.serviceInstaller});

		}
		#endregion
	}
}
