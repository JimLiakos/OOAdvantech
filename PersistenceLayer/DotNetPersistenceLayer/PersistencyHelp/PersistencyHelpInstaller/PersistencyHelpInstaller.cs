using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace PersistencyHelpInstaller
{
    /// <summary>
    /// Summary description for PersistencyHelpInstaller.
    /// </summary>
    /// <MetaDataID>{080ef7d2-6b9f-4532-bdca-4c621bca37ed}</MetaDataID>
	[RunInstaller(true)]
	public class PersistencyHelpInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected override void OnAfterInstall(IDictionary savedState)
		{
			base.OnAfterInstall (savedState);


            H2Reg.UnPackResources(Location);
            H2Reg.BuildNames(Location, "OOAdvanceTech", "MS.VSCC+", "OOAdvanceTech", "PersistencyHelp");
            H2Reg.RegisterPlugIn(Location);
			//PersistencyHelpInstaller.PersistencyHelpInstaller.UnRegisterPlugIn(directory);
            H2Reg.RemoveTempFiles(Location);

		}
		protected override void OnBeforeRollback(IDictionary savedState)
		{
			base.OnBeforeRollback (savedState);
            H2Reg.UnPackResources(Location);
            H2Reg.BuildNames(Location, "OOAdvanceTech", "MS.VSCC+", "OOAdvanceTech", "PersistencyHelp");
			//RegisterPlugIn(Location);
			H2Reg.UnRegisterPlugIn(Location);
            H2Reg.RemoveResourcesFiles(Location);
		}

		protected override void OnBeforeUninstall(IDictionary savedState)
		{
			base.OnBeforeUninstall (savedState);

            H2Reg.UnPackResources(Location);
            H2Reg.BuildNames(Location, "OOAdvanceTech", "MS.VSCC+", "OOAdvanceTech", "PersistencyHelp");
			//RegisterPlugIn(Location);
            H2Reg.UnRegisterPlugIn(Location);
            H2Reg.RemoveResourcesFiles(Location);

		}
  


		string Location;
		public PersistencyHelpInstaller()
		{
			Location=GetType().Assembly.Location;
			Location=Location.ToLower();
			Location=Location.Replace("persistencyhelpinstaller.dll","");

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
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
