using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace ConnectableControls.Menus.Docking
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{D5DF8FD2-91C8-44A9-B961-7D8BA017F1E7}</MetaDataID>
	
	public class CommandBarDockingViewArea: ContainerControl
	{
		/// <MetaDataID>{AD251D67-4DE5-46CF-B47F-F022300CDAF0}</MetaDataID>
		public event System.EventHandler OnRightClick;
        public  Docking.CommandBarDockingManager CommandBarDockingManager;
		/// <MetaDataID>{B8F74A4A-2534-412E-AA23-4536D6DE237E}</MetaDataID>
        public CommandBarDockingViewArea(Docking.CommandBarDockingManager commandBarDockingManager)
		{
			this.BackColor=Color.FromArgb(212,208,200);
            this.BackColor = Color.FromArgb(255,0, 0);
            this.BackColor = Color.FromArgb(241, 239, 226);
            CommandBarDockingManager = commandBarDockingManager;

			//this.AllowDrop = true;	
		//	this.DragEnter+= new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
		}
		/// <MetaDataID>{F0175434-D43E-414B-B845-EA53774BEB57}</MetaDataID>
		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e )
		{
			if( e.Button==MouseButtons.Right)
			{
				OnRightClick(this,e);
			}


			base.OnMouseDown(e);
			

			
		}
	

		


		/// <MetaDataID>{9F9A52BA-504D-4B82-942D-84201C7914CC}</MetaDataID>
		public void AddCommandBar(DockingCommandBar DockingCommandBar)
		{

			
		//	Height= DockingCommandBar.Height+10;
		//	DockingCommandBar.Width=Width;
		//	DockingCommandBar.Height=Height;

			Controls.Add(DockingCommandBar);
			if(Dock==DockStyle.Top||Dock==DockStyle.Bottom)
			{
				DockingCommandBar.Top=0;
				DockingCommandBar.HorizontalCaption=true;
			}
			else
			{
				DockingCommandBar.Left=0;
				DockingCommandBar.VerticalCaption=true;
			}
			DockingCommandBar.RecalculateSize();
			if(Width<DockingCommandBar.Width)
				Width=DockingCommandBar.Width;
			if(Height<DockingCommandBar.Height)
				Height=DockingCommandBar.Height;
		}

	
		/// <MetaDataID>{797C66A5-5844-43D1-A05E-85D46E98871D}</MetaDataID>
		public void RemoveCommandBar(DockingCommandBar DockingCommandBar)
		{
			Controls.Remove(DockingCommandBar);
			if(Controls.Count==0)
			{
				Width=3;
				Height=3;
			}

		}

	}
}
