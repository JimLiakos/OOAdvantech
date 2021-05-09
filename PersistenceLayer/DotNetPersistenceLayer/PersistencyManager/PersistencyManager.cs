using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Security.Principal;

namespace PersistencyManager
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    /// <MetaDataID>{08fa781f-8d67-45cc-a7c0-ae2cf86393a9}</MetaDataID>
	public class PersistencyManager : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		VsHelp.Help VsHelp; 
		private System.Windows.Forms.NotifyIcon NotifyIcon ;
		private Icon ServerPauseIcon = new Icon(typeof(PersistencyManager).Assembly.GetManifestResourceStream("PersistencyManager.ServerPause.ico"));
		private Icon ServerRunIcon = new Icon(typeof(PersistencyManager).Assembly.GetManifestResourceStream("PersistencyManager.ServerRun.ico"));
		private Icon ServerStopIcon = new Icon(typeof(PersistencyManager).Assembly.GetManifestResourceStream("PersistencyManager.ServerStop.ico"));
		private System.Timers.Timer Timer;
		//System.ServiceProcess.ServiceController PersistencyServiceController=null;
        OOAdvantech.PersistenceLayer.StorageServerInstanceLocator Locator;// =OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.GetStorageServerInstanceLocator();
        string CurrentInstanceName ;
		public PersistencyManager()
        {


            OOAdvantech.Remoting.RemotingServices theLocalRemotingServices = OOAdvantech.Remoting.RemotingServices.GetRemotingServices("tcp://localhost:9060") as OOAdvantech.Remoting.RemotingServices;
            Locator = theLocalRemotingServices.CreateInstance(typeof(OOAdvantech.PersistenceLayer.StorageServerInstanceLocator).ToString(), typeof(OOAdvantech.PersistenceLayer.StorageServerInstanceLocator).Assembly.FullName) as OOAdvantech.PersistenceLayer.StorageServerInstanceLocator;




            string[] instances= Locator.GetStorageServerInstances();
            if (instances.Length > 0)
            {
                CurrentInstanceName = instances[0];
                //PersistencyServiceController = Locator.GetServiceForInstance(CurrentInstanceName);
            }

            
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			NotifyIcon = new System.Windows.Forms.NotifyIcon();
            //if (PersistencyServiceController != null && PersistencyServiceController.Status == System.ServiceProcess.ServiceControllerStatus.Running)
            //    NotifyIcon.Icon = ServerRunIcon;
            //else
            //    NotifyIcon.Icon = ServerStopIcon;

            if (Locator.GetServiceStatus(CurrentInstanceName) == System.ServiceProcess.ServiceControllerStatus.Running)// PersistencyServiceController != null && PersistencyServiceController.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                NotifyIcon.Icon = ServerRunIcon;
            else
                NotifyIcon.Icon = ServerStopIcon;

			NotifyIcon.Text = "Storage server manager. \nRight Click for the menu";
			NotifyIcon.Visible = true;
            
            mMenuItems[0] = new MenuItem( "Open Storage Server Manager");
            mMenuItems[0].Click += new EventHandler(NotifyIcon_DoubleClick); 
            mMenuItems[1] = new MenuItem(@"Server : \\" + ComputerName + @"\" + CurrentInstanceName);
			mMenuItems[2] = new MenuItem("Start", new EventHandler(this.OnStart));
			mMenuItems[2].DefaultItem = true;
			mMenuItems[3] = new MenuItem("Stop ", new EventHandler(this.OnStop));
			//mMenuItems[3] = new MenuItem("Pause ", new EventHandler(this.OnPause));
			mMenuItems[4] = new MenuItem("-");
			mMenuItems[5] = new MenuItem("Exit", new EventHandler(this.OnExit));
			mMenuItems[6] = new MenuItem("Help", new EventHandler(this.OnHelp));

			NotifyIcon.ContextMenu = new ContextMenu(mMenuItems);
            NotifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);

			

			this.Visible=false;
			this.ShowInTaskbar=false;
			this.Size=new Size(0,0);
			this.Location=new Point(-40,-40);
			Visible=false;
			int gg=0;

            Timer.Start();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
        string ComputerName = System.Net.Dns.GetHostName().ToUpper();
        StorageServerManager storageServerManager = null;
        void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (storageServerManager == null)
            {
                storageServerManager = new StorageServerManager();

                if (CurrentInstanceName != null)
                {
                    int index = 0;
                    if (CurrentInstanceName.ToLower().Trim() == "default")
                        index = storageServerManager.StorageServerInstances.Items.IndexOf(ComputerName.ToUpper());
                    else
                        index = storageServerManager.StorageServerInstances.Items.IndexOf(ComputerName.ToUpper() + @"\" + CurrentInstanceName);

                    if (index != -1)
                        storageServerManager.StorageServerInstances.SelectedIndex = index;

                }
                storageServerManager.StorageServerInstances.SelectedIndexChanged += new EventHandler(StorageServerInstancesChanged);
                storageServerManager.ShowDialog();
                CurrentInstanceName = storageServerManager.StorageServerInstances.SelectedItem as string;
                if (CurrentInstanceName == null)
                    return;
                CurrentInstanceName=CurrentInstanceName.Replace(ComputerName.ToUpper() + "\\", "");
                if (CurrentInstanceName == ComputerName.ToUpper())
                    CurrentInstanceName = "default";

                if(CurrentInstanceName.ToLower().Trim()=="default")
                    mMenuItems[1].Text = @"Server : \\" + ComputerName;
                else
                    mMenuItems[1].Text = @"Server : \\" + ComputerName + @"\" + CurrentInstanceName;
//                PersistencyServiceController = Locator.GetServiceForInstance(CurrentInstanceName);
                Timer_Elapsed(null, null);
                storageServerManager = null;

            }
            else
                storageServerManager.Activate();


        }

        void StorageServerInstancesChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentInstanceName = storageServerManager.StorageServerInstances.SelectedItem as string;
                CurrentInstanceName = CurrentInstanceName.Replace(ComputerName.ToUpper() + "\\", "");
                if (CurrentInstanceName == ComputerName.ToUpper())
                    CurrentInstanceName = "default";

                if (CurrentInstanceName.ToLower().Trim() == "default")
                    mMenuItems[1].Text = @"Server : \\" + ComputerName;
                else
                    mMenuItems[1].Text = @"Server : \\" + ComputerName + @"\" + CurrentInstanceName;
                //PersistencyServiceController = Locator.GetServiceForInstance(CurrentInstanceName);
                Timer_Elapsed(null, null);
            }
            catch { }

        }
		private MenuItem[] mMenuItems =new MenuItem[7] ;
		private void OnExit(object sender , System.EventArgs e )
		{
			if(VsHelp!=null)
				try{VsHelp.Close();}catch(System.Exception){}
			Close();
			System.Diagnostics.Process ww;
		}
		private void OnHelp(object sender , System.EventArgs e )
		{
			try
			{
				if(VsHelp==null)
					VsHelp = new VsHelp.DExploreAppObjClass();
				VsHelp.SetCollection("ms-help://OOAdvanceTech","");
				VsHelp.DisplayTopicFromURL("ms-help://OOAdvanceTech/PersistencyHelp/html/StartPage.htm");
			}
			catch(System.Exception)
			{

			}

		}

		private void OnPause(object sender , System.EventArgs e )
		{
			try
			{
                Locator.Pause(CurrentInstanceName);
				//PersistencyServiceController.Pause();
			}
			catch(System.Exception)
			{
			}
		}

		private void OnStop(object sender , System.EventArgs e )
		{
			try
			{
                Locator.Stop(CurrentInstanceName);
				//PersistencyServiceController.Stop();
			}
			catch(System.Exception error)
			{
			}
		}

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }

		private void OnStart(object sender , System.EventArgs e )
		{
			try
			{
                Locator.Start(CurrentInstanceName);
				//PersistencyServiceController.Start();
			}
			catch(System.Exception)
			{

			}
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			NotifyIcon.Visible = false;
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Timer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.Timer)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Interval = 500;
            this.Timer.SynchronizingObject = this;
            this.Timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer_Elapsed);
            // 
            // PersistencyManager
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(231, 127);
            this.Name = "PersistencyManager";
            this.Text = "Form1";
            this.VisibleChanged += new System.EventHandler(this.PersistencyManager_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.Timer)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			try
			{
				//TODO: θα πρέπει να άλλαξει αυτός ο τρόπος 
				// γιατί μερικές φορές εγείρει exception του τύπου "Process performance counter is disabled, so the requested operation cannot be performed."
				System.Diagnostics.ProcessModule thisProcessMainModule=System.Diagnostics.Process.GetCurrentProcess().MainModule;
				System.Diagnostics.Process thisProcess=System.Diagnostics.Process.GetCurrentProcess();
				System.Diagnostics.Process[] processes=System.Diagnostics.Process.GetProcesses();
				foreach(System.Diagnostics.Process process in processes)
				{
                    //try
                    //{
                    //    if(process.MainModule==null)
                    //        continue;
                    //}
                    //catch(System.Exception)
                    //{
                    //    continue;
                    //}
                    //if(thisProcessMainModule.FileName==process.MainModule.FileName&&
                    //    thisProcessMainModule.FileVersionInfo.FileVersion==process.MainModule.FileVersionInfo.FileVersion
                    //    &&thisProcess.Id!=process.Id)
                    //    return;
				}
			}
			catch(System.Exception error)
			{
				int er=0;

			}
			Application.Run(new PersistencyManager());
		}
         
		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{

            try
            {
                Visible = false;
                //if (PersistencyServiceController == null)
                //    return;
                if (string.IsNullOrEmpty(CurrentInstanceName))
                    return;
                //PersistencyServiceController.Refresh();
                mMenuItems[2].Enabled = true;
                mMenuItems[3].Enabled = true;
                mMenuItems[4].Enabled = true;
                var serviceStatus = Locator.GetServiceStatus(CurrentInstanceName);
                if (serviceStatus == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    NotifyIcon.Icon = ServerRunIcon;
                    mMenuItems[2].DefaultItem = true;
                    mMenuItems[3].DefaultItem = false;
                    //mMenuItems[2].DefaultItem=false;
                    return;
                }
                if (serviceStatus == System.ServiceProcess.ServiceControllerStatus.Paused)
                {
                    NotifyIcon.Icon = ServerPauseIcon;
                    mMenuItems[2].DefaultItem = false;
                    mMenuItems[3].DefaultItem = false;
                    mMenuItems[4].DefaultItem = true;
                    return;
                }
                NotifyIcon.Icon = ServerStopIcon;
                mMenuItems[2].DefaultItem = false;
                mMenuItems[3].DefaultItem = true;
            }
            catch (System.Exception error)
            {
                //foreach (System.ServiceProcess.ServiceController serviceController in System.ServiceProcess.ServiceController.GetServices())
                //{
                //    if (serviceController.ServiceName == PersistencyServiceController.ServiceName)
                //        return;
                //}
                mMenuItems[2].Enabled = false;
                mMenuItems[3].Enabled = false;
                mMenuItems[4].Enabled = false;
                mMenuItems[1].Text = @"Server :";
                //PersistencyServiceController = null;
                NotifyIcon.Icon = ServerStopIcon;
            }
			
			//mMenuItems[2].DefaultItem=false;
		}

		private void PersistencyManager_VisibleChanged(object sender, System.EventArgs e)
		{
			Visible=false;
		}
	}
}
