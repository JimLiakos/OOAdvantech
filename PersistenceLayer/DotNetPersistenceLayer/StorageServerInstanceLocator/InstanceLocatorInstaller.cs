using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace StorageServerInstanceLocator
{
    /// <summary>
    /// Summary description for InstanceLocatorInstaller.
    /// </summary>
    /// <MetaDataID>{db80001c-5511-4113-ba87-df008640b295}</MetaDataID>
	[RunInstaller(true)]
	public class InstanceLocatorInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InstanceLocatorInstaller()
		{
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


		private System.ServiceProcess.ServiceInstaller serviceInstaller;
		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
		
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();


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
			this.serviceInstaller.ServiceName = "StorageServerInstanceLocator";
			
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
