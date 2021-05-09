using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace OOAdvantech.Transactions.Bridge
{
	/// <summary>
	/// Summary description for NativeTPMonitorInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class NativeTPMonitorInstaller : System.Configuration.Install.Installer
	{
		
		public void UnRegisterComPlusApp(string appName)
		{
			try
			{
				COMAdmin.ICOMAdminCatalog COMAdminCatalog=new COMAdmin.COMAdminCatalogClass();
				COMAdmin.COMAdminCatalogCollection apps=COMAdminCatalog.GetCollection("Applications")as COMAdmin.COMAdminCatalogCollection;
				COMAdmin.COMAdminCatalogObject app=null;
				apps.Populate();
				int Count=apps.Count;
				for(int i=0;i!=Count;i++)
				{
					COMAdmin.COMAdminCatalogObject currApp = apps.get_Item(i) as COMAdmin.COMAdminCatalogObject;
					string currAppName=currApp.Name as string;
					currAppName=currAppName.ToLower().Trim();
					appName=appName.ToLower().Trim();
					if(appName==currAppName)
					{
						apps.Remove(i);
						apps.SaveChanges();
						break;
					}
				}
			}
			catch(System.Exception)
			{
			}
		}
		public void RegisterComPlusApp(string appName,string appDesc,string appID,string dllName,int ShutdownAfter,bool RunForever,bool applicationAccessChecksEnabled)
		{
			COMAdmin.ICOMAdminCatalog COMAdminCatalog=new COMAdmin.COMAdminCatalogClass();
			COMAdmin.COMAdminCatalogCollection apps=COMAdminCatalog.GetCollection("Applications")as COMAdmin.COMAdminCatalogCollection;
			COMAdmin.COMAdminCatalogObject app=null;
			apps.Populate();
			int Count=apps.Count;
			for(int i=0;i!=Count;i++)
			{
				COMAdmin.COMAdminCatalogObject currApp = apps.get_Item(i) as COMAdmin.COMAdminCatalogObject;
				string CurrAppID=currApp.get_Value("ID") as string;
				appID=appID.ToLower();
				CurrAppID=CurrAppID.ToLower();
				if(CurrAppID==appID)
				{
					apps.Remove(i);
					apps.SaveChanges();
					break;
				}
			}
			app=apps.Add() as COMAdmin.COMAdminCatalogObject;
			app.set_Value("Name",appName);
			app.set_Value("ID",appID);
			app.set_Value("Description",appDesc);
			app.set_Value("ApplicationAccessChecksEnabled",true);
			app.set_Value("AccessChecksLevel",COMAdmin.COMAdminAccessChecksLevelOptions.COMAdminAccessChecksApplicationComponentLevel);
			app.set_Value("ShutdownAfter",ShutdownAfter);
			app.set_Value("RunForever",RunForever);
			app.set_Value("ApplicationAccessChecksEnabled",applicationAccessChecksEnabled);
			apps.SaveChanges();
			COMAdminCatalog.InstallComponent(appName,dllName,"","");
		
		}

		void UnRegister()
		{
			try
			{
				
				try{ModulePublisher.ClassRepository.Remove(Location+"OOAdvantech.dll","",false);}
				catch(System.Exception){}
				try{ModulePublisher.ClassRepository.Remove(Location+"TransactionManagmentSystem.dll","",false);}
				catch(System.Exception){}
//				try{ModulePublisher.ClassRepository.Remove(Location+"NativeTPMonitor.dll","",false);}
//				catch(System.Exception){}
				string commandLine=null;
				string arguments=null;
				long Count=0;
				System.Reflection.Assembly assembly=null;
				System.Diagnostics.ProcessStartInfo ProcessStartInfo=null;
				System.Diagnostics.Process process=null;
				try
				{
					assembly=System.Reflection.Assembly.LoadFile(Location+"AssemblyCrypto.dll");
					commandLine=Location+"GACutil.exe";
					arguments="-u \""+assembly.FullName+"\"";
					ProcessStartInfo=new System.Diagnostics.ProcessStartInfo(commandLine,arguments);
					ProcessStartInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Hidden;
					process= System.Diagnostics.Process.Start(ProcessStartInfo);
					while(!process.HasExited&&Count<40)
					{
						Count++;
						System.Threading.Thread.Sleep(200);
					}
				}
				catch(System.Exception)
				{
				}

				try
				{
					assembly=System.Reflection.Assembly.LoadFile(Location+"ModulePublisher.dll");
					commandLine=Location+"GACutil.exe";
					arguments="-u \""+assembly.FullName+"\"";
					ProcessStartInfo=new System.Diagnostics.ProcessStartInfo(commandLine,arguments);
					ProcessStartInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Hidden;
					process= System.Diagnostics.Process.Start(ProcessStartInfo);
					Count=0;
					while(!process.HasExited&&Count<40)
					{
						Count++;
						System.Threading.Thread.Sleep(200);
					}
				}
				catch(System.Exception)
				{
				}

				//UnRegisterComPlusApp("ComPlusTransactionManager");//,"ComPlusTransactionManager","{D1B2D338-43AF-4542-A156-3E70E5C3FCB7}",Location+"ComPlusTransactionManager.dll",15,true);
				UnRegisterComPlusApp("OOAdvantech");

				try
				{
					System.IO.File.Delete(Location+"OOAdvantech.tlb");
				}
				catch(System.Exception)
				{
				}

			}
			catch(System.Exception)
			{
			}

		}

		protected override void OnBeforeRollback(IDictionary savedState)
		{
			base.OnBeforeRollback (savedState);
			try{UnRegister();}catch(System.Exception Error){}
		}

		protected override void OnBeforeUninstall(IDictionary savedState)
		{
			base.OnBeforeUninstall (savedState);
			try{UnRegister();}catch(System.Exception Error){}
		}
 
		protected override void OnCommitted(IDictionary savedState)
		{
			base.OnCommitted (savedState);


			string message= "If this machine run Windows XP SP2 then press yes to read the guidelines \nfor the proper configuration of  Transaction Processing (TP) Monitor. \n\nPress Yes to read the guidelines, otherwise press No.";
			if(System.Windows.Forms.MessageBox.Show(message,"Transaction System",System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Information)==System.Windows.Forms.DialogResult.Yes)
			{
				System.Diagnostics.ProcessStartInfo processStartInfo=new System.Diagnostics.ProcessStartInfo(Location+"TPMonitorGuideLines.doc");
				System.Diagnostics.Process.Start(processStartInfo);
			}

			//string dd="
		}
 


		protected override void OnAfterInstall(IDictionary savedState)
		{	
			try
			{
				base.OnAfterInstall (savedState);
//				System.Windows.Forms.MessageBox.Show(Location); 
//				System.Windows.Forms.MessageBox.Show("NativeTP mitroulas"); 


				string commandLine=Location+"GACutil.exe";
				string arguments="-if \""+Location+"AssemblyCrypto.dll\"";

				System.Diagnostics.ProcessStartInfo ProcessStartInfo=new System.Diagnostics.ProcessStartInfo(commandLine,arguments);
				ProcessStartInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Hidden;
				System.Diagnostics.Process process= System.Diagnostics.Process.Start(ProcessStartInfo);
				long Count=0;
				while(!process.HasExited&&Count<40)
				{
					Count++;
					System.Threading.Thread.Sleep(200);
				}


				commandLine=Location+"GACutil.exe";
				arguments="-if \""+Location+"ModulePublisher.dll\"";


				ProcessStartInfo=new System.Diagnostics.ProcessStartInfo(commandLine,arguments);
				ProcessStartInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Hidden;
				process= System.Diagnostics.Process.Start(ProcessStartInfo);
				Count=0;
				while(!process.HasExited&&Count<40)
				{
					Count++;
					System.Threading.Thread.Sleep(200);
				}


				
				ModulePublisher.ClassRepository.Publish(Location+"OOAdvantech.dll","","");
				ModulePublisher.ClassRepository.Publish(Location+"TransactionManagmentSystem.dll","","");
				//ModulePublisher.ClassRepository.Publish(Location+"NativeTPMonitor.dll","","");

				//UnRegisterComPlusApp("ComPlusTransactionManager");
				UnRegisterComPlusApp("OOAdvantech");

//				commandLine=Location+"regsvr32.exe";
//				arguments="/s \""+ Location+"ComPlusTransactionManager.dll\"";
//				ProcessStartInfo=new System.Diagnostics.ProcessStartInfo(commandLine,arguments);
//				ProcessStartInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Hidden;
//				process= System.Diagnostics.Process.Start(ProcessStartInfo);
//				Count=0;
//				while(!process.HasExited&&Count<40)
//				{
//					Count++;
//					System.Threading.Thread.Sleep(200);
//				}
//				RegisterComPlusApp("ComPlusTransactionManager","ComPlusTransactionManager","{D1B2D338-43AF-4542-A156-3E70E5C3FCB7}",Location+"ComPlusTransactionManager.dll",15,true,false);
//				
				commandLine=Location+"regsvcs.exe";
				arguments="/appname:OOAdvantech /tlb:OOAdvantech.tlb \""+Location+"OOAdvantech.dll\"";
				ProcessStartInfo=new System.Diagnostics.ProcessStartInfo(commandLine,arguments);
				ProcessStartInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Hidden;
				process= System.Diagnostics.Process.Start(ProcessStartInfo);
				Count=0;
				while(!process.HasExited&&Count<40)
				{
					Count++;
					System.Threading.Thread.Sleep(200);
				}

			}
			catch(System.Exception Error)
			{
				throw Error;
			}

		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		string Location;
		public NativeTPMonitorInstaller()
		{
			Location=GetType().Assembly.Location;
			Location=Location.ToLower();
			Location=Location.Replace("nativetpmonitor.dll","");

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
