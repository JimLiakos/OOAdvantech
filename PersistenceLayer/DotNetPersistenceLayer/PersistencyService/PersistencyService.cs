using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Drawing;

namespace PersistencyService
{
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
		private void ShowFormSelect(object sender , System.EventArgs e )
		{
			int hh=0;
		}

	

		// The main entry point for the process
		static void Main()
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
			this.ServiceName = "PersistencyService";
			
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
			//System.Windows.Forms.MessageBox.Show("in OnStart");
			/*bool Block=true;
			while(Block)
			{
				System.Threading.Thread.Sleep(1000);
			}*/
			
			try
			{
				string ConfigFileName=AppDomain.CurrentDomain.BaseDirectory+"PersistencyService.config";
				if(!System.IO.File.Exists(ConfigFileName))
				{
					System.Xml.XmlDocument XmlDocument=new System.Xml.XmlDocument();
					XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
					XmlDocument.Save(ConfigFileName);
				}
				System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName);				

				System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
				System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);

				//Force Transaction assembly load
				System.Type type=typeof(OOAdvantech.Transactions.SystemStateTransition );

				OOAdvantech.Remoting.RemotingServices.RegisterChannel(4000);
				OOAdvantech.PersistenceLayer.IPersistencyService persistencyService=ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.PersistencyService","",true,new Type[1]{typeof(bool)}) as OOAdvantech.PersistenceLayer.IPersistencyService;
				
				
				System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime=TimeSpan.FromSeconds(20);
				System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime=TimeSpan.FromSeconds(10);
				System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout=TimeSpan.FromSeconds(10);/**/
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
