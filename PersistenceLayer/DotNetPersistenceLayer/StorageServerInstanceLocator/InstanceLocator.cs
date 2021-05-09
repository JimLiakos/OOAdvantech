using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;

namespace StorageServerInstanceLocator
{
    /// <MetaDataID>{42ff0f05-be01-4d01-b0a9-561ad4b24834}</MetaDataID>
	public class InstanceLocator : System.ServiceProcess.ServiceBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InstanceLocator()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}



		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "StorageServerInstanceLocator";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{

			bool Block=false;
            while (Block)
            {
                System.Threading.Thread.Sleep(1000);
            }

            
			
			try
			{
				string ConfigFileName=AppDomain.CurrentDomain.BaseDirectory+"StorageServerLocator.config";
				if(!System.IO.File.Exists(ConfigFileName))
				{
					System.Xml.XmlDocument XmlDocument=new System.Xml.XmlDocument();
					XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
					XmlDocument.Save(ConfigFileName);
				}
				System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName,false);				

				System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
				System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);

				//Force Transaction assembly load
				System.Type type=typeof(OOAdvantech.Transactions.SystemStateTransition );

                //Force PersistenceLayerRunTime assembly load
                type = typeof(OOAdvantech.PersistenceLayerRunTime.PersistencyService);


                OOAdvantech.Remoting.RemotingServices.RegisterSecureTcpChannel(9060,false);
                OOAdvantech.Remoting.RemotingServices.RegisterIpcClientChannel();
                //OOAdvantech.Remoting.RemotingServices.RegisterChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), false);//regiser ipc
				//OOAdvantech.PersistenceLayer.IPersistencyService persistencyService=ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.PersistencyService","",true,new Type[1]{typeof(bool)}) as OOAdvantech.PersistenceLayer.IPersistencyService;
				
				
			}
			catch(System.Exception Error)
			{

				//Error prone γεμισει με message το log file τοτε παράγει exception
				if(!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
					System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
				System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
				myLog.Source = "PersistencySystem";
				System.Diagnostics.Debug.WriteLine(
					Error.Message+Error.StackTrace);
				myLog.WriteEntry(Error.Message+Error.StackTrace,System.Diagnostics.EventLogEntryType.Error);
				throw Error;
				int hh=0;
			}

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
