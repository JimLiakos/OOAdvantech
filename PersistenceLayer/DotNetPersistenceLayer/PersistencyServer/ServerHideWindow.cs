using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Diagnostics;

namespace PersistencyServer
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    /// <MetaDataID>{42a8215e-89c1-4d14-a522-ffdd3679ce11}</MetaDataID>
	public class ServerHideWindow : System.Windows.Forms.Form
	{
		private MenuItem[] mMenuItems =new MenuItem[4] ;
		private Icon mSmileIcon = new Icon(typeof(ServerHideWindow).Assembly.GetManifestResourceStream("PersistencyServer.Icon1.ico"));
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.NotifyIcon NotifyIcon ;
		public ServerHideWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			NotifyIcon = new System.Windows.Forms.NotifyIcon();
			NotifyIcon.Icon = mSmileIcon;
			NotifyIcon.Text = "Right Click for the menu";
			NotifyIcon.Visible = true;

        //Insert all MenuItem objects into an array and add them to
        //the context menu simultaneously.
        mMenuItems[0] = new MenuItem("Show Form...", new EventHandler(this.ShowFormSelect));
		mMenuItems[0].DefaultItem = true;
        mMenuItems[1] = new MenuItem("Collect ", new EventHandler(this.ToggleImageSelect));
        mMenuItems[2] = new MenuItem("-");
        mMenuItems[3] = new MenuItem("Exit", new EventHandler(this.ExitSelect));
        NotifyIcon.ContextMenu = new ContextMenu(mMenuItems);
		//this.Opacity=0;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		private void ShowFormSelect(object sender , System.EventArgs e )
		{
			int hh=0;
		}

		private void ToggleImageSelect(object sender , System.EventArgs e )
		{
			System.Diagnostics.Debug.WriteLine(" ");
			System.Diagnostics.Debug.WriteLine(" ");
			System.Diagnostics.Debug.WriteLine(" ");
			System.Diagnostics.Debug.WriteLine("  ************* Collect ****************");
			
			
			System.Diagnostics.Debug.WriteLine(System.GC.GetTotalMemory(true).ToString());
			//		label1.Text=PersistenceLayer.Info.ActivObjects.ToString();

			
			System.Diagnostics.Debug.WriteLine(" ");
			System.Diagnostics.Debug.WriteLine(" ");
			System.Diagnostics.Debug.WriteLine(" ");

		}
		private void ExitSelect(object sender , System.EventArgs e )
		{
			Close();
			int hh=0;
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
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(88, 112);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 40);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// ServerHideWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 270);
			this.Controls.Add(this.button1);
			this.Name = "ServerHideWindow";
			this.Text = "Form1";
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
			
				
				//if(System.Diagnostics.EventLog.SourceExists("MySource"))



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

                OOAdvantech.Remoting.RemotingServices.RegisterChannel(int.Parse(TCPPort), "");

                OOAdvantech.PersistenceLayer.IPersistencyService persistencyService = ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=8cd83e51ed2de794", "PersistenceLayerRunTime, Culture=neutral, PublicKeyToken=8cd83e51ed2de794", insatnceName, new Type[1] { typeof(string) }) as OOAdvantech.PersistenceLayer.IPersistencyService;
 

				Application.Run(new PersistencyServer.ServerHideWindow());
			}
			catch(System.Exception error)
			{
				int hh=0;

			}
		}

		
		OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage;
		private void button1_Click(object sender, System.EventArgs e)
		{
			
			
			//PersistenceLayer.StorageSession mStorageSession=null;
			try
			{
				if(ObjectStorage==null)
					ObjectStorage= OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("Family","Rocket","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

				string OQLExpression="SELECT theEmployee \n"+ //,theJob.Employer.Name theEmployer
					"FROM PersistenceLayerTestPrj.Employee theEmployee "+
					"WHERE theEmployee.Age=3000 ";

				OOAdvantech.PersistenceLayer.StructureSet aStructureSet=ObjectStorage.Execute(OQLExpression);
		
			
				int Count=0;
				foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
				{
					Count++;
					long lo=0;
					continue;
				}
			}
			catch(System.Exception Error)
			{
				int k=0;
			}


		}
	}
}
