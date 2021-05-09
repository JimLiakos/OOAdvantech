using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
namespace PersistencyService
{
	/// <summary>
	/// Summary description for ProjectInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		protected override void OnAfterInstall(IDictionary savedState)
		{
		/*	base.OnAfterInstall (savedState);
			string location= GetType().Assembly.Location;
			location= GetType().Assembly.Location;
			location=location.Replace("persistencyservice.exe","");*/
		//	System.Windows.Forms.MessageBox.Show(location);

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


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// 
		private void InitializeComponent()
		{
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
			this.serviceInstaller.ServiceName = "PersistencyService";
			
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
