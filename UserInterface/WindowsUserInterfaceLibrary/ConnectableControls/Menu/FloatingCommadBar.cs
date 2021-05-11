using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.Menus.Controls;
using System.Drawing.Imaging;
using ConnectableControls.Menus.Common;

namespace ConnectableControls.Menus.Docking
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{68AC7995-1EF6-4D75-8A83-B06A60995DF1}</MetaDataID>
	class FloatingCommadBar:Form
	{

		protected enum ImageIndex
		{
			Close					= 0,
			EnabledVerticalMax		= 1,
			EnabledVerticalMin		= 2,
			AutoHide		        = 3, 
			AutoShow		        = 4 
		}

		/// <MetaDataID>{7BDDB310-2735-42FA-AAD9-6A828BA60EB3}</MetaDataID>
		protected static ImageList _images;

		/// <MetaDataID>{A2268B29-9574-46E3-B094-8B1B30CF78C6}</MetaDataID>
		static FloatingCommadBar()
		{
            _images = ResourceHelper.LoadBitmapStrip(typeof(FloatingCommadBar),
				"ConnectableControls.Menu.Resources.WhiteImagesCaptionIDE.bmp",
				new Size(_imageWidth, _imageHeight),
				new Point(0,0));
		}
		/// <MetaDataID>{C3DACBF9-4575-4E8B-85A6-93A1332BA208}</MetaDataID>
		protected InertButton _closeButton;
		/// <MetaDataID>{8956B21B-172E-4C42-BE63-0C61F2E4CA9A}</MetaDataID>
		protected const int _buttonWidth = 12;
		/// <MetaDataID>{7919EC46-2ACF-4E75-8A2E-E72CD7C0946B}</MetaDataID>
		protected const int _buttonHeight = 12;
		/// <MetaDataID>{1E1D3084-753E-4314-A479-718E0851E6A3}</MetaDataID>
		protected const int _imageWidth = 12;
		/// <MetaDataID>{4F0F404F-58DD-4BC3-9D5A-50F700C92634}</MetaDataID>
		protected const int _imageHeight = 11;
		/// <MetaDataID>{1EB23F9A-8086-4629-8924-B0B712B9AECA}</MetaDataID>
		protected static ImageAttributes _activeAttr = new ImageAttributes();
		/// <MetaDataID>{C21E4A97-B670-4251-8168-4266DB0C932E}</MetaDataID>
		protected static ImageAttributes _inactiveAttr = new ImageAttributes();



		/// <MetaDataID>{837B69D2-1E68-44C0-B594-93561EE6033B}</MetaDataID>
		public FloatingCommadBar(DockingCommandBar CommandBar,Control HostForm)
		{
			

			MouseIsDown=false;
                while (!(HostForm is Form) && HostForm != null)
                {
                    HostForm = HostForm.Parent;

                }
			Owner= HostForm as Form;
			this.StartPosition = FormStartPosition.Manual;
			this.ShowInTaskbar = false;

			Left=Control.MousePosition.X+1;
			Top=Control.MousePosition.Y+1;
			_CommandBar=CommandBar;
			Controls.Add(_CommandBar);


			_closeButton = new InertButton(_images, (int)ImageIndex.Close);
			_closeButton.Size = new Size(_buttonWidth+2, _buttonHeight+1);
			// Shows border all the time and not just when mouse is over control
		//	_closeButton.PopupStyle = false;

			// Define the fixed remapping
			_closeButton.ImageAttributes = _activeAttr;

	

			Controls.Add(_closeButton);
		
			_CommandBar.HorizontalCaption=true;
			Show();
			_closeButton.Show();
			_CommandBar.RecalculateSize();
			Width=_CommandBar.Width;
			Height=_CommandBar.Height+14;
			_closeButton.Location = new Point( Width-_buttonWidth-4,0);
			_closeButton.BackColor=Color.FromArgb(128,128,128);

			_closeButton.Click += new EventHandler(OnFloatingCommadBarClose);

			_CommandBar.Left=0;
			_CommandBar.Top=14;
			_CommandBar.Dock=DockStyle.Bottom;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ControlBox = false;
			
			
		}

		/// <MetaDataID>{A5554871-83C0-4E90-BABB-60A6CC08509B}</MetaDataID>
		protected void OnFloatingCommadBarClose(object sender, EventArgs e)
		{
			_closeButton.Click -= new EventHandler(OnFloatingCommadBarClose);
			Close();

		}

		/// <MetaDataID>{FBDA990A-5476-466E-928B-27D9CAC82A70}</MetaDataID>
		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e )
		{
			

				if(MouseIsDown)
				{
					Point mPoint=new Point(MousePosition.X,MousePosition.Y);
					if(LastPoint.X!=0||LastPoint.Y!=0)
					{
						int XOffset=mPoint.X-LastPoint.X;
						int YOffset=mPoint.Y-LastPoint.Y;
						{
							int TmpWidth=Width;
							int TmpHeight=Height;
							Top=Top+YOffset;
							Left=Left+XOffset;
							Width=TmpWidth;
							Height=TmpHeight;
							
						
						}
					}

					LastPoint=mPoint;
					if(e.X>1&&e.X<Width&& e.Y>1&&e.Y<10)
						this.Cursor=Cursors.SizeAll;// new Cursor("X:\\source\\UserInterface\\Magic Library 1.7\\Source\\MagicLibrary\\Resources\\MyCursor.cur");
					else
						this.Cursor=Cursors.Default;

				}
				else
					this.Cursor=Cursors.Default;


			//ConnectableControls.Common.ResourceHelper.LoadCursor(this.GetType(),"ConnectableControls.Resources.MyCursor.cur");
		}

		/// <MetaDataID>{C93BCABF-B958-4F7D-BA3A-393D5436129A}</MetaDataID>
		Point LastPoint; 
		/// <MetaDataID>{356AA35F-A672-4FCC-BE3C-07DE2FDB0BCA}</MetaDataID>
		bool MouseIsDown;
		/// <MetaDataID>{65E3707C-AE43-43CA-B0D9-D4E847E85849}</MetaDataID>
		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			
			 
			System.Drawing.SolidBrush mBrush=
				new SolidBrush(Color.FromArgb(128,128,128));
			System.Drawing.SolidBrush activeTextBrush=new SolidBrush(Color.FromArgb(255,255,255));
			//mHatchBrush.BackgroundColor=Color.FromArgb(255,255,255);
			//mHatchBrush.ForegroundColor=Color.FromArgb(128,128,128);
			e.Graphics.FillRectangle(mBrush,0,0,Width,Height-_CommandBar.Height);
			System.Drawing.Font mFont=new Font("Courier New",10,System.Drawing.FontStyle.Bold);
			
			

			e.Graphics.DrawString(_CommandBar.Name, mFont, activeTextBrush, new Rectangle(4,0,Width-_closeButton.Width-3,Height-_CommandBar.Height));
			base.OnPaint(e);

		}
		/// <MetaDataID>{D28031D8-2449-47BF-9D2A-A6E19F76AA92}</MetaDataID>
		protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e )
		{
			if(e.Button==MouseButtons.Left)
			{
				this.Cursor=Cursors.SizeAll;
				LastPoint.X=0;
				LastPoint.Y=0;
				MouseIsDown=true;
			}
		}
		/// <MetaDataID>{78898B49-8447-47DA-987E-CFF516F80915}</MetaDataID>
		protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e )
		{
			if(e.Button==MouseButtons.Left)
			{
				this.Cursor=Cursors.Default;
				MouseIsDown=false;
			}
		}

		/// <MetaDataID>{1BD54F93-8F87-474C-A1A2-566AFC8E545B}</MetaDataID>
		protected override CreateParams CreateParams 
		{
			get 
			{
                //return base.CreateParams;
				// Let base class fill in structure first
				CreateParams cp = base.CreateParams;
				// The only way to get a caption bar with only small 
				// close button is by providing this extended style
				//cp.ExStyle |= (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW;
				cp.Height=10;
				cp.Width=10;
				cp.X=Control.MousePosition.X;
				cp.Y=Control.MousePosition.X;
				cp.Style&=(~(int)Win32.WindowStyles.WS_CAPTION);
				cp.Style&=(~(int)Win32.WindowStyles.WS_THICKFRAME);
				cp.Style|=((int)Win32.WindowStyles.WS_BORDER);
				return cp;
			}
		}
		/// <MetaDataID>{E82137F2-E9F6-4C4A-8C68-1ABC13285308}</MetaDataID>
		DockingCommandBar _CommandBar;

		/// <MetaDataID>{69DC6346-91ED-4D75-B89A-4DDE20046363}</MetaDataID>
		public DockingCommandBar CommandBar
		{
			get
			{
				return _CommandBar;
			}
		}
		/// <MetaDataID>{8F2E2953-7F9D-4DCC-9D2D-9C965867E06F}</MetaDataID>
		protected override void OnLoad ( System.EventArgs e )
		{
			
	
		}

		/// <MetaDataID>{DCFF02E2-93F9-4FC2-A3CD-2C63F66BE8E7}</MetaDataID>
		private void InitializeComponent()
		{
			// 
			// FloatingCommadBar
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 270);
			this.MinimizeBox = false;
			this.Name = "FloatingCommadBar";

		}
	
		/// <MetaDataID>{CE7994A4-E2C8-4D96-83F8-E9863CC923E2}</MetaDataID>
		public void RemoveCommandBar(DockingCommandBar DockingCommandBar)
		{
			MouseIsDown=false;
			Controls.Remove(DockingCommandBar);
            
			Close();
		}

		
	}
}
