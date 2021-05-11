using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.Menus;

namespace ConnectableControls.Menus.Docking
{
	
	

	/// <summary>
	/// </summary>
	/// <MetaDataID>{BFB15906-DA56-4C6E-86BD-A6BADCADB9FA}</MetaDataID>
    [ToolboxItem(false)]
	public class DockingCommandBar : ContainerControl
	{
		/// <MetaDataID>{5BE3CD9B-F73D-4399-B947-4BDA201B3A0F}</MetaDataID>
		private System.Windows.Forms.DockStyle _Dock;
		/// <MetaDataID>{029601A3-AD38-42AE-8553-2652783415A8}</MetaDataID>
		public override System.Windows.Forms.DockStyle Dock
		{
			get
			{
				return _Dock;
			}
			set
			{
				_Dock = value;
				if(Parent !=null)
				{
					if(Parent is FloatingCommadBar)
						base.Dock=_Dock;
				}
				if(value==DockStyle.Top||value==DockStyle.Bottom)
				{
					HorizontalCaption=true;
					if(_MenuControl!=null)
						_MenuControl.Dock=DockStyle.Top;
				}
				if(value==DockStyle.Left||value==DockStyle.Right)
				{
					VerticalCaption=false;
					if(_MenuControl!=null)
						_MenuControl.Dock=DockStyle.Left;
				}
			}		
		}
	

		/// <MetaDataID>{9B4D96E4-E6EF-4DF0-A6BE-26D2725BDCE1}</MetaDataID>
		private bool _HorizontalCaption;
		/// <MetaDataID>{3ADEF4D7-7538-4830-A463-FF2E7AE43B7B}</MetaDataID>
		public bool HorizontalCaption
		{
			set
			{
				if(value==true)
				{
					_HorizontalCaption=true;
					_VerticalCaption=false;
					_MenuControl.Dock=DockStyle.Top;
				}
			}
		}
		/// <MetaDataID>{23EC3578-FEEA-4563-8B7B-DA4E7D817551}</MetaDataID>
		private bool _VerticalCaption;
		/// <MetaDataID>{0DCF2A7E-921F-4417-B6EB-5D7151AA5C55}</MetaDataID>
		public bool VerticalCaption
		{
			set
			{
				if(value==true)
				{
					_VerticalCaption=true;
					_HorizontalCaption=false;
					_MenuControl.Dock=DockStyle.Left;
				}
			}
		}
		/// <MetaDataID>{D1346B52-0280-4944-B1D8-63BBE7FE1776}</MetaDataID>
        [ToolboxItem(false)]
		public class ClientArea:ContainerControl
		{
			/// <MetaDataID>{87AAC120-546F-4A91-AC51-0EFEA6508C72}</MetaDataID>
			 public ClientArea()
			{
				this.Height=34;
				this.Width=6;
				//this.BackColor=Color.FromArgb(0,128,128);
			
			}
	
		}
		/// <MetaDataID>{FC1D3E3A-001C-4E6F-8DF8-C5012A170E0A}</MetaDataID>
		protected ClientArea _ClientArea;

	
		/// <MetaDataID>{20358D6F-5A72-4914-89FF-E14ABBE50186}</MetaDataID>
		internal System.Drawing.Point LastPoint;
		/// <MetaDataID>{3C4138D5-76AE-4D0E-8266-D2F9A7360CE9}</MetaDataID>
		private MenuControl _MenuControl;
		/// <MetaDataID>{849C9B27-DF69-4819-84A6-726DDFB82EC1}</MetaDataID>
		public MenuControl MenuControl
		{
			set 
			{
				_MenuControl=value;
				if(value!=null)
				{
					Name=value.Menu.Text;
					//ErrorProne
					_MenuControl.Width=10000;
					_MenuControl.Height=10000;

					
				
					
					value.BackColor=Color.FromArgb(222,218,210);
					Width=CommandBarWidth;
					Height=CommandBarHeight;
					if(_HorizontalCaption)
					{
						

						_ClientArea.Width=Width-6;
						_ClientArea.Height=Height;


					}
					else
					{
						_ClientArea.Width=Width;
						_ClientArea.Height=Height-6;
					}

					_ClientArea.Controls.Add(value);
				}
			}
			get
			{
				return _MenuControl;
			}
		}
		/// <MetaDataID>{B37344D7-0982-42C5-BA2D-A1C2D5B0CBA4}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{E0448EA9-DED8-4C7A-B81D-54928C9D855C}</MetaDataID>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name=value;
			}
		}

	
	
		

	
		
		/*
		/// <MetaDataID>{1B85E299-A6E1-4EEA-AAEA-31E7715E4FE5}</MetaDataID>
		public override DockStyle Dock
		{
			get
			{
				return _Dock;
			}
			set
			{
				_Dock = value;
				if(Parent !=null)
				{
					if(Parent is FloatingCommadBar)
						base.Dock=_Dock;
				}
				if(value==DockStyle.Top||value==DockStyle.Bottom)
				{
					HorizontalCaption=true;
					if(_MenuControl!=null)
						_MenuControl.Dock=DockStyle.Top;
				}
				if(value==DockStyle.Left||value==DockStyle.Right)
				{
					VerticalCaption=false;
					if(_MenuControl!=null)
						_MenuControl.Dock=DockStyle.Left;
				}


					
			}
		}*/

		/// <MetaDataID>{FBEF9850-A640-4A4A-8851-3AAF75313A92}</MetaDataID>
		public void RecalculateSize()
		{
			
			MenuControl.Recalculate();
			Width=CommandBarWidth;
			Height=CommandBarHeight;

			if(_HorizontalCaption)
			{
				_ClientArea.Left=6;
				_ClientArea.Top=0;
				_ClientArea.Width=Width-6;
				_ClientArea.Height=Height;
				/*if(CommandBarButtons.Count>0)
				{
					
					((Menus.CommandBarButton1)CommandBarButtons[0]).Left=Width;
					((Menus.CommandBarButton1)CommandBarButtons[0]).Top=2;
					Width+=CommandBarButtons.Count*28;
					Width+=5;
				}*/

			}
			else
			{
				_ClientArea.Left=0;
				_ClientArea.Top=6;
				_ClientArea.Width=Width;
				_ClientArea.Height=Height-6;
				/*if(CommandBarButtons.Count>0)
				{
					((Menus.CommandBarButton1)CommandBarButtons[0]).Top=Height;
					((Menus.CommandBarButton1)CommandBarButtons[0]).Left=2;
					Height+=CommandBarButtons.Count*27;
				}*/

			}
		}
		
	

		/// <MetaDataID>{956BBADE-F06A-422F-BCDA-CF55EF02CDEB}</MetaDataID>
		private int CommandBarHeight
		{
			get
			{


				int CommandBarHeight=0;
				if(_MenuControl._drawCommands==null)
					return 0;
				int BackupHeight=_ClientArea.Height;
				_ClientArea.Height=10000;
				
				if(_MenuControl.Dock==DockStyle.Left||_MenuControl.Dock==DockStyle.Right)
				{
					foreach(ConnectableControls.Menus.DrawCommand  CurrDrawCommand in _MenuControl._drawCommands)
						CommandBarHeight+=CurrDrawCommand.DrawRect.Height+Menus.MenuControl._shadowGap;
					CommandBarHeight+=6;
				}
				else
				{
					foreach(ConnectableControls.Menus.DrawCommand  CurrDrawCommand in _MenuControl._drawCommands)
					{
						CommandBarHeight+=CurrDrawCommand.DrawRect.Height;
						break;
					}
				}
				_ClientArea.Height=BackupHeight;
				return CommandBarHeight;

			}
		}
		/// <MetaDataID>{8C6DA96C-0F61-4D0D-BCCC-0BCAEE4D7DC5}</MetaDataID>
		private int CommandBarWidth
		{
			get
			{

				int CommandBarWidth=0;
				if(_MenuControl._drawCommands==null)
					return 0;
				int BackupWidth=_ClientArea.Width;
				_ClientArea.Width=10000;

				if(_MenuControl.Dock==DockStyle.Top||_MenuControl.Dock==DockStyle.Bottom)
				{
					_MenuControl.Recalculate();
					foreach(ConnectableControls.Menus.DrawCommand  CurrDrawCommand in _MenuControl._drawCommands)
						CommandBarWidth+=CurrDrawCommand.DrawRect.Width+Menus.MenuControl._shadowGap;
					CommandBarWidth+=6;
				}
				else
				{
					foreach(ConnectableControls.Menus.DrawCommand  CurrDrawCommand in _MenuControl._drawCommands)
					{
						CommandBarWidth+=CurrDrawCommand.DrawRect.Width;
						break;
					}
				}
				_ClientArea.Width=BackupWidth;
				return CommandBarWidth;
			}
		

		}


	
		/// <MetaDataID>{08842CCD-BEAC-46F5-B549-A26DEEE4EE00}</MetaDataID>
		internal bool MouseOnCaptionArea;
		/// <MetaDataID>{962523F7-9694-41A8-87B6-8D661CF69028}</MetaDataID>
		internal bool MouseIsDown;
		/// <MetaDataID>{4F9FBA59-105E-4A38-B457-A1F551BAD610}</MetaDataID>
		protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e )
		{
			if(e.Button==MouseButtons.Left)
			{
				LastPoint.X=0;
				LastPoint.Y=0;
				MouseIsDown=true;
			}
		}
		/// <MetaDataID>{B42693A9-C373-4631-B257-D56651F979BA}</MetaDataID>
		protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e )
		{
			if(e.Button==MouseButtons.Left)
			{
				MouseIsDown=false;
				this.Cursor=Cursors.Default;
			}
		}
		/// <MetaDataID>{31F4DB23-B374-4EA8-9B50-03837A6BE987}</MetaDataID>
		internal bool NewCursor;
		/// <MetaDataID>{F9CEA3FA-DBD2-4E11-8460-B5D135CD3235}</MetaDataID>
		internal int MouseSampling;
		

		/// <MetaDataID>{B356B6E3-D602-43EE-BC8E-335B8D4C9EB8}</MetaDataID>
		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e )
		{
			if(Parent is CommandBarDockingViewArea)
			{

				if(MouseIsDown)
				{
					Point mPoint=new Point(MousePosition.X,MousePosition.Y);
					mPoint=Parent.PointToClient(mPoint);
					if(LastPoint.X!=0||LastPoint.Y!=0)
					{
						int XOffset=mPoint.X-LastPoint.X;
						int YOffset=mPoint.Y-LastPoint.Y;

					{
						int TmpWidth=Width;
						int TmpHeight=Height;
						if((Top+YOffset)>0)
							Top=Top+YOffset;
						if((Left+XOffset)>0)
							Left=Left+XOffset;
					}
					}
					LastPoint=mPoint;


					//else
					//	MouseSampling++;
				}
				if(e.X>1&&e.X<6&&_HorizontalCaption||e.Y>1&&e.Y<6&&_VerticalCaption)
				{
					this.Cursor=Cursors.SizeAll;// new Cursor("X:\\source\\UserInterface\\Magic Library 1.7\\Source\\MagicLibrary\\Resources\\MyCursor.cur");
					NewCursor=true;
					MouseOnCaptionArea=true;
				}
				else
				{
					NewCursor=false;
					this.Cursor=Cursors.Default;
					MouseOnCaptionArea=false;
				}
			}
			else
			{
				NewCursor=false;
				this.Cursor=Cursors.Default;
				MouseOnCaptionArea=false;
			}



			//ConnectableControls.Common.ResourceHelper.LoadCursor(this.GetType(),"ConnectableControls.Resources.MyCursor.cur");
		}
		/// <MetaDataID>{39743D74-D0B4-4746-AF6A-5DACC09F1A61}</MetaDataID>
		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			
			 
			System.Drawing.Drawing2D.HatchBrush mHatchBrush=
				new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent70,this.BackColor,Color.FromArgb(128,128,128));
			if(_HorizontalCaption)
				e.Graphics.FillRectangle(mHatchBrush,1,5,6,_ClientArea.Height-10);
			else
				e.Graphics.FillRectangle(mHatchBrush,5,1,_ClientArea.Width-10,6);
			base.OnPaint(e);
		}
	
		/// <MetaDataID>{FD93ECDF-93C5-49AD-B12F-8F1954FEC132}</MetaDataID>
		 public DockingCommandBar()
		{
			_Dock=DockStyle.None;
			_HorizontalCaption=true;
			_VerticalCaption=false;
			MouseSampling=0;
			NewCursor=false;
			_ClientArea=new ClientArea();
			this.Height=34;
			Controls.Add(_ClientArea);
			this.BackColor=Color.FromArgb(222,218,210);
			_ClientArea.Left=6;
			this.Width=300;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			// 
			// TODO: Add constructor logic here
			//
		}
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (_MenuControl != null && _MenuControl.Direction == Common.Direction.Horizontal)
            {
                Brush m_Brush =new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.FromArgb(246, 244, 239), Color.FromArgb(192, 192, 168), 90); new SolidBrush(Color.BlueViolet);
                e.Graphics.FillRectangle(m_Brush, ClientRectangle);
                m_Brush.Dispose();
            }
            else
            {
                Brush m_Brush =new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.FromArgb(246, 244, 239), Color.FromArgb(192, 192, 168), 360); new SolidBrush(Color.BlueViolet);
                e.Graphics.FillRectangle(m_Brush, ClientRectangle);
                m_Brush.Dispose();


            }

        }
	}
}
