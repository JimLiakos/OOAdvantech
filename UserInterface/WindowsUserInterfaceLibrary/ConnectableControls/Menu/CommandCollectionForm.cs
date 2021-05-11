using System;
using System.Windows.Forms;
using System.Drawing;
using ConnectableControls.Menus.Common;

namespace ConnectableControls.Menus
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{336E2A45-1F20-4451-8462-F26FD0E697F6}</MetaDataID>
	public class CommandCollectionForm : System.Windows.Forms.Form
	{
		/// <MetaDataID>{0F2A6F82-D4F3-44BB-8C6C-029D03CFB963}</MetaDataID>
		public event System.Windows.Forms.MouseEventHandler  DragCommandMove;
		/// <MetaDataID>{102D18B3-4135-47D6-ABC4-A00F69D4CEF2}</MetaDataID>
		public event EventHandler DropCommand;
		/// <MetaDataID>{B2BEB0FB-47C4-498B-90F3-68ECF389A5B3}</MetaDataID>
        public event ConnectableControls.Menus.Docking.DragDropCommandEventHandler DragCommandEnter;

		/// <MetaDataID>{12D21656-06F1-4D14-A914-215C8B394C30}</MetaDataID>
		private System.Windows.Forms.Label label1;
		/// <MetaDataID>{6B91721F-6671-47D5-8074-20437C82D5A2}</MetaDataID>
		private Cursor _DragStartCursor;
		/// <MetaDataID>{EE2F3E15-E5C7-43CA-B389-97EA0965FB14}</MetaDataID>
		private bool OnDragMode=false;

		/// <MetaDataID>{BEABCD83-C22A-4583-A8C0-A60CC4D459CA}</MetaDataID>
		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e )
		{
		
		}
			
		/// <MetaDataID>{DC1E1DAA-28B8-469C-8C27-F8E09B649997}</MetaDataID>
		public CommandCollectionForm ()
		{
				InitializeComponent();

			_DragStartCursor  = ResourceHelper.LoadCursor(Type.GetType("ConnectableControls.Menus.CommandCollectionForm"),
				"ConnectableControls.Menu.Resources.DragCommand.cur");

		}

		/// <MetaDataID>{BB0056B3-5CC3-471A-B319-A9F4CFECF102}</MetaDataID>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(161)));
			this.label1.Location = new System.Drawing.Point(32, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
			this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
			this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
			// 
			// CommandCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 270);
			this.Controls.Add(this.label1);
			this.Name = "CommandCollectionForm";
			this.Text = "Liakos";
			this.ResumeLayout(false);

		}
	
	

		/// <MetaDataID>{86258EAE-D41C-4709-8DAE-5F2DCC2AC65D}</MetaDataID>
		private void label1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			

			 //DoDragDrop("asas",	DragDropEffects.All);
			Point ClickPoint=label1.PointToClient(Control.MousePosition);
			if(label1.ClientRectangle.Contains(ClickPoint))
			{


				Cursor=_DragStartCursor;
				OnDragMode=true;
				MenuCommand DragMenuCommand=new ConnectableControls.Menus.MenuCommand("Mitros");
				if(DragCommandEnter!=null)
					DragCommandEnter(this, ref DragMenuCommand,null);
			}
		}

		/// <MetaDataID>{2D4EE971-546C-4EE7-934F-C3E374AF4F74}</MetaDataID>
		private void label1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
			if(OnDragMode)
			{

				if(DragCommandMove!=null)
					DragCommandMove(this, e);
			}

		
		}

		/// <MetaDataID>{5279370B-D9E2-453D-B0DE-4A7EC6C30850}</MetaDataID>
		private void label1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(OnDragMode)
			{
				if(DropCommand!=null)
					DropCommand(this,null);
			}
			OnDragMode=false;
			Cursor=Cursors.Default;

		
		}
	}
}
