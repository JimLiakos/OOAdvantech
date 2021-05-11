// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetConnectableControls.com
// *****************************************************************************

using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using ConnectableControls.Menus.Win32;
using ConnectableControls.Menus;
using ConnectableControls.Menus.Common;
using ConnectableControls.Menus.Controls;
using ConnectableControls.Menus.Collections;

namespace ConnectableControls.Menus
{

	/// <MetaDataID>{A84A6FAD-AA9E-40A6-8E20-629434927300}</MetaDataID>
	public interface IMenuComandViewer
	{
		/// <MetaDataID>{D716BEF0-705B-41F3-B33C-6484D760D4A2}</MetaDataID>
		void Refresh();
	}

	/// <MetaDataID>{96AD913B-6237-4D25-AC4B-0BBCDE7FE88E}</MetaDataID>
	[ToolboxBitmap(typeof(MenuControl))]
	[DefaultProperty("MenuCommands")]
	[DefaultEvent("PopupSelected")]
	[Designer(typeof(ConnectableControls.Menus.MenuControlDesigner))]
    [ToolboxItem(false)]
	public class MenuControl : ContainerControl, IMessageFilter,IMenuComandViewer//ContainerControl
	{
		/// <MetaDataID>{2810CCEA-6505-4AFD-97A2-EE6F9A04C64B}</MetaDataID>
		void Refresh()
		{
			Recalculate();
		}
        /// <MetaDataID>{e880a826-a2c9-4f91-a38e-f658b268ef32}</MetaDataID>
		private bool DragEnterFlag=false;
        /// <MetaDataID>{5dded38f-e8c4-4921-bf22-eb54b0ea6002}</MetaDataID>
		bool _DesignMode=false;
        /// <MetaDataID>{c1754e2a-0fbd-4ab2-b1db-c818d7116fda}</MetaDataID>
		public bool DesignMode
		{
			get{return _DesignMode;}
			set
			{
				_DesignMode=value;
			}
		}


		/// <MetaDataID>{D46B0010-909B-47E8-89D9-25C5A9B63159}</MetaDataID>
		void RemoveDragCommandCursor()
		{
			for(int i=0; i<_drawCommands.Count; i++)
			{
				DrawCommand dc = _drawCommands[i] as DrawCommand;
				if(dc.MenuCommand!=null)
				{
					dc.DragCommandCursorAtEnd=false;
					dc.DragCommandCursorAtStart=false;
				}
			}
			Invalidate();
		}
	
		/// <MetaDataID>{D9110F4F-24A7-429A-BE62-B66F7DBC6BC0}</MetaDataID>
		internal class MdiClientSubclass : NativeWindow
		{
			/// <MetaDataID>{23ED72A0-0882-4495-B308-005A12A6A0D7}</MetaDataID>
			protected override void WndProc(ref Message m)
			{
				switch(m.Msg)
				{
					case (int)Win32.Msgs.WM_MDISETMENU:
					case (int)Win32.Msgs.WM_MDIREFRESHMENU:
						return;
				}

				base.WndProc(ref m);
			}			
		}

		// Class constants
        /// <MetaDataID>{ee38aa19-0181-4e87-a55c-11c04fe577f1}</MetaDataID>
		protected const int _lengthGap = 3;
        /// <MetaDataID>{516a8103-96ba-4c61-9536-4cfe99dce53b}</MetaDataID>
		protected const int _boxExpandUpper = 1;
        /// <MetaDataID>{64a76b9d-6cc3-484f-b2be-15d8d537a05d}</MetaDataID>
		protected const int _boxExpandSides = 2;
        /// <MetaDataID>{f3f5a7a5-f1bf-4961-8c96-952f16de70a1}</MetaDataID>
		internal const int _shadowGap = 4;
        /// <MetaDataID>{c7355d31-7f99-4744-aebd-06935ca543a0}</MetaDataID>
		protected const int _shadowYOffset = 4;
        /// <MetaDataID>{bc6f5a73-f398-498f-8f32-577aacdfe27e}</MetaDataID>
		protected const int _separatorWidth = 15;
        /// <MetaDataID>{ffc9ceed-289a-43d5-a2c2-5c67e6f3bd2f}</MetaDataID>
		protected const int _subMenuBorderAdjust = 2;
        /// <MetaDataID>{ac7055d1-9e4e-4b62-a409-c70eb4d506ab}</MetaDataID>
		protected const int _minIndex = 0;
        /// <MetaDataID>{726493a4-93ee-42aa-a905-d9650a3474ed}</MetaDataID>
		protected const int _restoreIndex = 1;
        /// <MetaDataID>{ec942065-2b17-4e82-b617-a5437286b18a}</MetaDataID>
		protected const int _closeIndex = 2;
        /// <MetaDataID>{5ea60db4-0a8c-4eda-8e00-89984ad405e6}</MetaDataID>
		protected const int _chevronIndex = 3;
        /// <MetaDataID>{e6a541cf-b1a0-4436-8834-95306156d5ff}</MetaDataID>
		protected const int _buttonLength = 16;
        /// <MetaDataID>{d50931db-19dc-49cf-90fa-40d9a60b4a92}</MetaDataID>
		protected const int _chevronLength = 12;
        /// <MetaDataID>{3dc7a003-f5f5-431b-a52d-bb40f89f762f}</MetaDataID>
		protected const int _pendantLength = 48;
        /// <MetaDataID>{7069bca9-a876-4611-bff2-ef18501d14a1}</MetaDataID>
		protected const int _pendantOffset = 3;

		// Class constant is marked as 'readonly' to allow non constant initialization
        /// <MetaDataID>{dcf61201-f24a-4a00-aafa-941cd84d827f}</MetaDataID>
		protected readonly int WM_OPERATEMENU = (int)Win32.Msgs.WM_USER + 1;

		// Class fields
        /// <MetaDataID>{eef243f3-dbc1-4709-a5b5-39369c6c700c}</MetaDataID>
		protected static ImageList _menuImages = null;
        /// <MetaDataID>{b4cd15d6-f48b-4b6d-a420-619c2c75560e}</MetaDataID>
		protected static bool _supportsLayered = false;


		// Instance fields
        /// <MetaDataID>{1f04201e-58f0-43c3-8ff7-a28f912c803c}</MetaDataID>
		protected int _rowWidth;
        /// <MetaDataID>{e4a6ff35-ae21-4c04-a1ec-1c37065c7ac3}</MetaDataID>
		protected int _rowHeight;
        /// <MetaDataID>{d5ecb37e-5bd5-48a1-b87d-6ecc7ae13f90}</MetaDataID>
		protected int _ImageRowHeight;
        /// <MetaDataID>{bdbfc7e2-2d38-4018-86d5-e06dad9ac206}</MetaDataID>
		protected int _ImageRowWidth;
        /// <MetaDataID>{5361be6c-8275-4489-aee4-108af078bf7d}</MetaDataID>
		protected int _trackItem;
        /// <MetaDataID>{66b6cc82-1ca7-4730-8f99-6cc3ebac972e}</MetaDataID>
		protected int _breadthGap;
        /// <MetaDataID>{65aeef72-929e-426c-9f3a-4d236d820d3e}</MetaDataID>
		protected int _animateTime;
        /// <MetaDataID>{a1e57f0a-2650-426c-bb80-3e90b1480012}</MetaDataID>
		protected IntPtr _oldFocus;
        /// <MetaDataID>{d8f95708-6abd-4c79-9cbe-4b42dc33df65}</MetaDataID>
		protected Pen _controlLPen;
        /// <MetaDataID>{9cc549c9-00f0-4f2a-83d6-7a4540711e8d}</MetaDataID>
		protected bool _animateFirst;
        /// <MetaDataID>{f3cc02a5-6990-45e8-b883-edc6a8b32750}</MetaDataID>
		protected bool _selected;
        /// <MetaDataID>{b62802c3-1b26-4b8e-8412-175946cd442c}</MetaDataID>
		protected bool _multiLine;
        /// <MetaDataID>{213d1a07-a98b-4db7-b16d-8fc9409ba726}</MetaDataID>
		protected bool _mouseOver;
        /// <MetaDataID>{2a1b3c02-0f23-4c22-a765-75e6a6ca5615}</MetaDataID>
		protected bool _manualFocus;
        /// <MetaDataID>{50e321fb-6c4c-4798-b73a-e2ce0e986835}</MetaDataID>
		protected bool _drawUpwards;
        /// <MetaDataID>{29930496-bb77-40f1-8278-ebfc3dacf8d4}</MetaDataID>
		protected bool _defaultFont;
        /// <MetaDataID>{3eec367c-ca62-4919-8f2b-4c0cbff3a62f}</MetaDataID>
		protected bool _defaultBackColor;
        /// <MetaDataID>{230ee89f-525a-442d-b920-68dfb8756cbd}</MetaDataID>
		protected bool _defaultTextColor;
        /// <MetaDataID>{4fa93296-ca32-4ab3-96bf-207897295644}</MetaDataID>
		protected bool _defaultHighlightBackColor;
        /// <MetaDataID>{1a7032aa-141d-4321-a966-b2ff0d4ea253}</MetaDataID>
		protected bool _defaultHighlightTextColor;
        /// <MetaDataID>{59c35ab8-3d55-4108-b14d-24756f3d3e5c}</MetaDataID>
		protected bool _defaultSelectedBackColor;
        /// <MetaDataID>{08705ac2-6a13-4874-83dd-3e4e153fe504}</MetaDataID>
		protected bool _defaultSelectedTextColor;
        /// <MetaDataID>{9cce8fc3-d228-4597-b64d-bdf56aa10c9c}</MetaDataID>
		protected bool _defaultPlainSelectedTextColor;
        /// <MetaDataID>{4ac4cffa-7563-4257-aeab-5047d93588ad}</MetaDataID>
		protected bool _plainAsBlock;
        /// <MetaDataID>{6355cf8e-4de9-42a2-a415-74d09ead0f11}</MetaDataID>
		protected bool _dismissTransfer;
        /// <MetaDataID>{cd387ae1-4eb6-4792-a87e-d637bacd4548}</MetaDataID>
		protected bool _ignoreMouseMove;
        /// <MetaDataID>{84298b0e-75ed-4e28-baf8-a7b02b1ec1f7}</MetaDataID>
		protected bool _expandAllTogether;
        /// <MetaDataID>{e2d733f8-edc8-4a41-a2f2-c7c5cc26973d}</MetaDataID>
		protected bool _rememberExpansion;
        /// <MetaDataID>{6d26a8a1-6a77-4857-bf73-a363164a5b82}</MetaDataID>
		protected bool _deselectReset;
        /// <MetaDataID>{ecc6d39d-c4c2-4f85-84d1-a372310d252d}</MetaDataID>
		protected bool _highlightInfrequent;
        /// <MetaDataID>{9e62e116-683c-4989-bc23-0c164e8083d4}</MetaDataID>
		protected bool _exitLoop;
        /// <MetaDataID>{f4340e06-6816-4e61-b9f3-725ed4033a70}</MetaDataID>
		protected Color _textColor;
        /// <MetaDataID>{75ada4d4-524a-43f0-8f75-93ba3fa409b7}</MetaDataID>
		protected Color _highlightBackColor;
        /// <MetaDataID>{b5d62add-451b-4711-be36-5dbe4cec9cfc}</MetaDataID>
		protected Color _useHighlightBackColor;
        /// <MetaDataID>{b6d15daf-0b8e-4967-bbae-d4d9c0f1cb7e}</MetaDataID>
		protected Color _highlightTextColor;
        /// <MetaDataID>{89650f00-317b-4518-81fe-0ebd7e9a32be}</MetaDataID>
		protected Color _highlightBackDark;
        /// <MetaDataID>{c1bb61ae-ab3c-4156-b182-5e65d206f7c8}</MetaDataID>
		protected Color _highlightBackLight;
        /// <MetaDataID>{5026dad3-d508-4a58-8931-87741bdb26ec}</MetaDataID>
		protected Color _selectedBackColor;
        /// <MetaDataID>{c9890485-30ec-4095-9e0d-c731db2f69ce}</MetaDataID>
		protected Color _selectedTextColor;
        /// <MetaDataID>{35e0f6ab-372d-4c8e-b0f4-0884250c4b6b}</MetaDataID>
		protected Color _plainSelectedTextColor;
        /// <MetaDataID>{d448f55f-9445-4122-a6ad-b3b1b3a22804}</MetaDataID>
		protected Form _activeChild;
        /// <MetaDataID>{a81f81f8-ecdd-4188-8351-e4c528993a7b}</MetaDataID>
		protected Form _mdiContainer;
        /// <MetaDataID>{7afde2e3-91fd-42f7-8530-fd1f81368c90}</MetaDataID>
		protected VisualStyle _style;
        /// <MetaDataID>{71fae2a8-e87b-4bfc-910a-d21470337b0a}</MetaDataID>
		protected Direction _direction;
        /// <MetaDataID>{7234ca17-92c2-4eb1-93aa-0a305d3d93a1}</MetaDataID>
		protected PopupMenu _popupMenu;
        /// <MetaDataID>{af0960f6-316a-4fff-817e-cae7d6803355}</MetaDataID>
		public ArrayList _drawCommands;
        /// <MetaDataID>{a0b00941-8ea3-4549-96be-7da140095a18}</MetaDataID>
		protected SolidBrush _controlLBrush;
        /// <MetaDataID>{bffa22a1-5143-47f1-b1e3-1196c90bd95c}</MetaDataID>
		internal Brush _backBrush;
        /// <MetaDataID>{c189d1e9-fd95-4eb6-8104-6d00403bba45}</MetaDataID>
		protected Animate _animate;
        /// <MetaDataID>{7268fcc4-2922-4284-a0b0-b422f7fdb139}</MetaDataID>
		protected Animation _animateStyle;
        /// <MetaDataID>{5e8b1cc0-9f4d-4aac-bef8-b542ea464034}</MetaDataID>
		internal MdiClientSubclass _clientSubclass;
        /// <MetaDataID>{191cf71e-dea1-4d65-94bb-2c467461fe5a}</MetaDataID>
		protected MenuCommand _chevronStartCommand;
        /// <MetaDataID>{2e2aa131-98c9-4fc1-8890-dff0800cec74}</MetaDataID>
		protected MenuCommandCollection _menuCommands;
        /// <MetaDataID>{54cafa49-6044-43a0-82ea-49459d7a6a05}</MetaDataID>
		private Cursor _DragDropCursor;
        /// <MetaDataID>{9d60ce54-9403-41ac-8075-01438449b0bb}</MetaDataID>
        private ConnectableControls.Menus.Docking.CommandBarDockingManager _CommandBarManager;
        /// <MetaDataID>{978252d7-9d60-4993-96f0-02039cb8e6fb}</MetaDataID>
		private MenuCommand _Menu;
        /// <MetaDataID>{af2e5761-5336-45d9-9e5f-42fdf450f77b}</MetaDataID>
		ToolTip _toolTip ;

		// Instance fields - pendant buttons
        /// <MetaDataID>{b69009e7-f475-4747-bc13-1f668e6f6f20}</MetaDataID>
		protected InertButton _minButton;
        /// <MetaDataID>{e96a32a1-8d01-49b5-b0ab-467522e5b9dd}</MetaDataID>
		protected InertButton _restoreButton;
        /// <MetaDataID>{aaa09caa-9929-4df8-b329-3f8280bec628}</MetaDataID>
		protected InertButton _closeButton;

		// Instance fields - events
		public event CommandHandler Selected;
		public event CommandHandler Deselected;
		public event System.Windows.Forms.MouseEventHandler  DragCommandMove;
		public event EventHandler DropCommand;
		

		/// <MetaDataID>{A0AF5916-F694-46D7-88E0-A8F0CBCB4855}</MetaDataID>
		static MenuControl()
		{
			// Create a strip of images by loading an embedded bitmap resource
			_menuImages = ResourceHelper.LoadBitmapStrip(Type.GetType("ConnectableControls.Menus.MenuControl"),
				"ConnectableControls.Menu.Resources.ImagesMenuControl.bmp",
				new Size(_buttonLength, _buttonLength),
				new Point(0,0));

			// We need to know if the OS supports layered windows
			_supportsLayered = (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null);
		}
        /// <MetaDataID>{ac13cbb8-9a40-4d7f-8b29-bc8aa106aa4e}</MetaDataID>
        public readonly bool InMenuEditor = false;
        /// <MetaDataID>{3c8f2b67-0cdd-451b-a8f7-408270e2d54f}</MetaDataID>
        public MenuControl(bool inMenuEditor)
        {
            InMenuEditor = inMenuEditor;
            InternalMenuControl(null);
        }

		/// <MetaDataID>{F3440639-FCEE-4EC2-9990-76C6227ACE89}</MetaDataID>
		public MenuControl()
		{
			InternalMenuControl(null);
		}

		/// <MetaDataID>{0787DA5F-8A4A-4F78-8984-56A4AA56BE44}</MetaDataID>
		public MenuControl(MenuCommand Menu)
		{
			InternalMenuControl(Menu);
		}
		/// <MetaDataID>{73E52A5C-3BA2-4770-A2C1-2C2438522490}</MetaDataID>
		public void InternalMenuControl(MenuCommand Menu)
		{
			// Set default values
			_trackItem = -1;
			_oldFocus = IntPtr.Zero;
			_minButton = null;
			_popupMenu = null;
			_activeChild = null;
			_closeButton = null;
			_controlLPen = null;
			_mdiContainer = null;
			_restoreButton = null;
			_controlLBrush = null;
			_chevronStartCommand = null;
			_animateFirst = true;
			_exitLoop = false;
			_selected = false;
			_multiLine = false;
			_mouseOver = false;
			_defaultFont = true;
			_manualFocus = false;
			_drawUpwards = false;
			_plainAsBlock = false;
			_clientSubclass = null;
			_ignoreMouseMove = false;
			_deselectReset = true;
			_expandAllTogether = true;
			_rememberExpansion = true;
			_highlightInfrequent = true;
			_dismissTransfer = false;
			_style = VisualStyle.IDE;
			_direction = Direction.Horizontal;
			//_menuCommands = new MenuCommandCollection();

			
			_Menu=Menu;
			if(_Menu==null)
				_Menu=new MenuCommand();

            _menuCommands=_Menu.MenuCommands; 
			this.Dock = DockStyle.Top;
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			AllowDrop=true;
			
			// Animation details
			_animateTime = 100;
			_animate = Animate.System;
			_animateStyle = Animation.System;

			// Prevent flicker with double buffering and all painting inside WM_PAINT
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			// Should not be allowed to select this control
			SetStyle(ControlStyles.Selectable, false);

			// Hookup to collection events
			_menuCommands.Cleared += new CollectionClear(OnCollectionCleared);
			_menuCommands.Inserted += new CollectionChange(OnCollectionInserted);
			_menuCommands.Removed += new CollectionChange(OnCollectionRemoved);

			// Need notification when the MenuFont is changed
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new UserPreferenceChangedEventHandler(OnPreferenceChanged);

			DefineColors();
            
			// Set the starting Font
			DefineFont(SystemInformation.MenuFont);

			// Do not allow tab key to select this control
			this.TabStop = false;

			// Default to one line of items
			this.Height = _rowHeight;

			// Add ourself to the application filtering list
			Application.AddMessageFilter(this);


			// Create the ToolTip and associate with the Form container.
			_toolTip = new ToolTip();
			// Set up the delays for the ToolTip.
			_toolTip.AutoPopDelay = 5000;
			_toolTip.InitialDelay = 1000;
			_toolTip.ReshowDelay = 500;
			// Force the ToolTip text to be displayed whether or not the form is active.
			_toolTip.ShowAlways = true;
      
			// Set up the ToolTip text for the Button and Checkbox.
			
			_DragDropCursor = ResourceHelper.LoadCursor(Type.GetType("ConnectableControls.Menus.CommandCollectionForm"),
				"ConnectableControls.Menu.Resources.DragCommand.cur");

		}

		/// <MetaDataID>{585BD430-764A-4FB4-935B-B5E880F9A06A}</MetaDataID>
		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				// Remove notifications
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new UserPreferenceChangedEventHandler(OnPreferenceChanged);
			}
			base.Dispose( disposing );
		}

        /// <MetaDataID>{997432b3-ae4e-4573-8169-ce95aaccc72c}</MetaDataID>
		[Category("Behaviour")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public MenuCommandCollection MenuCommands
		{
			get { return _menuCommands; }
		} 

		//[Category("Appearance")]
        /// <MetaDataID>{f93006bf-b53f-48ed-898a-8aeb39847364}</MetaDataID>
		public VisualStyle Style
		{
			get { return _style; }
			
			set
			{
				if (_style != value)
				{
					_style = value;

					Recalculate();
					Invalidate();
				}
			}
		}

        /// <MetaDataID>{326183d6-f83c-4ad1-ba38-127e63af1560}</MetaDataID>
		public override Font Font
		{
			get { return base.Font; }
			
			set
			{
				if (value != base.Font)
				{
					_defaultFont = (value == SystemInformation.MenuFont);

					DefineFont(value);

					Recalculate();
					Invalidate();
				}
			}
		}

        /// <MetaDataID>{acd7d48e-940d-431d-836b-bd0749a26fcd}</MetaDataID>
		public override Color BackColor
		{
			get { return base.BackColor; }

			set
			{
				if (value != base.BackColor)
				{
					_defaultBackColor = (value == SystemColors.Control);
					base.BackColor = value;
                    _backBrush = new LinearGradientBrush(ClientRectangle,Color.FromArgb(246,244,239),Color.FromArgb(192,192,168), 90);// SolidBrush(base.BackColor);
                    
					Recalculate();
					Invalidate();
				}
			}
		}

		/// <MetaDataID>{63138D52-95AD-498E-A01D-46C607E00046}</MetaDataID>
		private bool ShouldSerializeBackColor()
		{
			return this.BackColor != SystemColors.Control;
		}


        /// <MetaDataID>{aaeaafa8-b0a5-4e9f-9b24-ad6aa7850618}</MetaDataID>
		[Category("Appearance")]
		public Color TextColor
		{
			get { return _textColor; }

			set
			{
				if (value != _textColor)
				{
					_textColor = value;
					_defaultTextColor = (value == SystemColors.MenuText);

					Recalculate();
					Invalidate();
				}
			}
		}
        /// <MetaDataID>{4fe659a9-0ea0-4f40-a582-16825f6c170d}</MetaDataID>
        internal ConnectableControls.Menus.Docking.CommandBarDockingManager CommandBarManager
		{
			get { return _CommandBarManager;}
			set
			{
				_CommandBarManager=value;
			}
		}

		/// <MetaDataID>{D7C2FF2B-BBF2-4032-AF57-CBF9ABD889FF}</MetaDataID>
		private bool ShouldSerializeTextColor()
		{
			return _textColor != SystemColors.MenuText;
		}

        /// <MetaDataID>{73917ffa-21a4-46f9-a790-2f741ac9580c}</MetaDataID>
		[Category("Appearance")]
		public Color HighlightBackColor
		{
			get { return _highlightBackColor; }

			set
			{
				if (value != _highlightBackColor)
				{
					_defaultHighlightBackColor = (value == SystemColors.Highlight);
					DefineHighlightBackColors(value);                    

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <MetaDataID>{0920B305-A671-4ADB-8415-097C245AA70E}</MetaDataID>
		private bool ShouldSerializeHighlightBackColor()
		{
			return _highlightBackColor != SystemColors.Highlight;
		}

        /// <MetaDataID>{c17e1fa8-e276-4760-9792-08b7bf7bdf57}</MetaDataID>
		[Category("Appearance")]
		public Color HighlightTextColor
		{
			get { return _highlightTextColor; }

			set
			{
				if (value != _highlightTextColor)
				{
					_highlightTextColor = value;
					_defaultHighlightTextColor = (value == SystemColors.MenuText);

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <MetaDataID>{59CDDF54-0577-45DB-A941-3D1F5A657BC9}</MetaDataID>
		private bool ShouldSerializeHighlightTextColor()
		{
			return _highlightTextColor != SystemColors.HighlightText;
		}

        /// <MetaDataID>{59ec67e5-82f6-499e-a3ec-183cf206a5ab}</MetaDataID>
		[Category("Appearance")]
		public Color SelectedBackColor
		{
			get { return _selectedBackColor; }

			set
			{
				if (value != _selectedBackColor)
				{
					DefineSelectedBackColors(value);
					_defaultSelectedBackColor = (value == SystemColors.Control);

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <MetaDataID>{791AF42A-735D-4E5F-9C2B-D339C0CCAFE4}</MetaDataID>
		private bool ShouldSerializeSelectedBackColor()
		{
			return _selectedBackColor != SystemColors.Control;
		}

        /// <MetaDataID>{9bca2707-5da6-4773-80a7-36b8fed7612a}</MetaDataID>
		[Category("Appearance")]
		public Color SelectedTextColor
		{
			get { return _selectedTextColor; }

			set
			{
				if (value != _selectedTextColor)
				{
					_selectedTextColor = value;
					_defaultSelectedTextColor = (value == SystemColors.MenuText);

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <MetaDataID>{10D64F4F-3D53-425D-9795-350513D110BD}</MetaDataID>
		private bool ShouldSerializeSelectedTextColor()
		{
			return _selectedTextColor != SystemColors.MenuText;
		}

        /// <MetaDataID>{d1388f26-378d-4675-8bec-af5c91d1a54b}</MetaDataID>
		[Category("Appearance")]
		public Color PlainSelectedTextColor
		{
			get { return _plainSelectedTextColor; }

			set
			{
				if (value != _plainSelectedTextColor)
				{
					_plainSelectedTextColor = value;
					_defaultPlainSelectedTextColor = (value == SystemColors.ActiveCaptionText);

					Recalculate();
					Invalidate();
				}
			}
		}

		/// <MetaDataID>{C90771CD-DD72-4C8B-A159-D66EAA6EAF9C}</MetaDataID>
		private bool ShouldSerializePlainSelectedTextColor()
		{
			return _plainSelectedTextColor != SystemColors.ActiveCaptionText;
		}

        /// <MetaDataID>{9ca54261-de4b-48c4-b8a2-5aeeb79c86c4}</MetaDataID>
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool PlainAsBlock
		{
			get { return _plainAsBlock; }

			set
			{
				if (_plainAsBlock != value)
				{
					_plainAsBlock = value;

					Recalculate();
					Invalidate();
				}
			}
		}

        /// <MetaDataID>{f0fd1ed0-e14f-4135-8363-2f7c74356350}</MetaDataID>
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool MultiLine
		{
			get { return _multiLine; }

			set
			{
				if (_multiLine != value)
				{
					_multiLine = value;

					Recalculate();
					Invalidate();
				}
			}
		}

        /// <MetaDataID>{1c6b690f-8d6e-422a-bd2e-0a079dc9fca4}</MetaDataID>
		[Category("Appearance")]
		public Direction Direction
		{
			get { return _direction; }

			set
			{
				if (_direction != value)
				{
					_direction = value;

					Recalculate();
					Invalidate();
				}
			}
		}

        /// <MetaDataID>{236c154e-7cca-4615-a7ef-7aa88fc7cf3b}</MetaDataID>
		[Category("Behaviour")]
		[DefaultValue(true)]
		public bool RememberExpansion
		{
			get { return _rememberExpansion; }
			set { _rememberExpansion = value; }
		}

        /// <MetaDataID>{119cf8c7-1b32-4239-a78d-a095d35468aa}</MetaDataID>
		[Category("Behaviour")]
		[DefaultValue(true)]
		public bool ExpandAllTogether
		{
			get { return _expandAllTogether; }
			set { _expandAllTogether = value; }
		}

        /// <MetaDataID>{ce0de01c-8ff8-4f02-b565-84c8189a61cf}</MetaDataID>
		[Category("Behaviour")]
		[DefaultValue(true)]
		public bool DeselectReset
		{
			get { return _deselectReset; }
			set { _deselectReset = value; }
		}

        /// <MetaDataID>{f0ddfa47-f1dd-447b-870c-cb3d11f614c7}</MetaDataID>
		[Category("Behaviour")]
		[DefaultValue(true)]
		public bool HighlightInfrequent
		{
			get { return _highlightInfrequent; }
			set { _highlightInfrequent = value; }
		}

        /// <MetaDataID>{1a1aeebc-8396-45b2-ab10-9e121a63f463}</MetaDataID>
		public override DockStyle Dock
		{
			get { return base.Dock; }

			set
			{
				base.Dock = value;
                Direction oldDirection = _direction;

				switch(value)
				{
					case DockStyle.None:
						_direction = Direction.Horizontal;
						break;
					case DockStyle.Top:
					case DockStyle.Bottom:
						this.Height = 0;
						_direction = Direction.Horizontal;
						break;
					case DockStyle.Left:
					case DockStyle.Right:
						this.Width = 0;
						_direction = Direction.Vertical;
						break;
				}

				Recalculate();
                if (oldDirection != _direction)
                {
                    _backBrush.Dispose();
                    if(_direction==Direction.Horizontal)
                        _backBrush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(246, 244, 239), Color.FromArgb(192, 192, 168), 90);
                    else
                        _backBrush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(246, 244, 239), Color.FromArgb(192, 192, 168), 360);


                }

				Invalidate();
			}
		}

        /// <MetaDataID>{de78acae-dd21-4b35-a937-39d55acc6525}</MetaDataID>
		[Category("Animate")]
		[DefaultValue(typeof(Animate), "System")]
		public Animate Animate
		{
			get { return _animate; }
			set { _animate = value; }
		}

        /// <MetaDataID>{ddaf2ff9-659e-4d61-9db7-25ff50ebee30}</MetaDataID>
		[Category("AnimateTime")]
		public int AnimateTime
		{
			get { return _animateTime; }
			set { _animateTime = value; }
		}

        /// <MetaDataID>{68e3a4aa-37df-41a3-89af-88ce3335cc3b}</MetaDataID>
		public MenuCommand Menu
		{
			get { return _Menu; }
			set { _Menu = value; }
		}

        /// <MetaDataID>{4afc60b9-674b-468b-b118-83f464262760}</MetaDataID>
		[Category("AnimateStyle")]
		public Animation AnimateStyle
		{
			get { return _animateStyle; }
			set { _animateStyle = value; }
		}

        /// <MetaDataID>{b8d69c81-12e7-4fb3-9c10-afd6ba7797b2}</MetaDataID>
		[Category("Behaviour")]
		[DefaultValue(null)]
		public Form MdiContainer
		{
			get { return _mdiContainer; }
			
			set
			{
				if (_mdiContainer != value)
				{
					if (_mdiContainer != null)
					{
						// Unsubclass from MdiClient and then remove object reference
						_clientSubclass.ReleaseHandle();
						_clientSubclass = null;

						// Remove registered events
						_mdiContainer.MdiChildActivate -= new EventHandler(OnMdiChildActivate);

						RemovePendantButtons();
					}

					_mdiContainer = value;

					if (_mdiContainer != null)
					{
						CreatePendantButtons();

						foreach(Control c in _mdiContainer.Controls)
						{
							MdiClient client = c as MdiClient;

							if (client != null)
							{
								// We need to subclass the MdiClient to prevent any attempt
								// to change the menu for the container Form. This prevents
								// the system from automatically adding the pendant.
								_clientSubclass = new MdiClientSubclass();
								_clientSubclass.AssignHandle(client.Handle);
							}
						}

						// Need to be informed of the active child window
						_mdiContainer.MdiChildActivate += new EventHandler(OnMdiChildActivate);
					}
				}
			}
		}

		/// <MetaDataID>{4E28B4CA-F2CE-44A9-943C-83BA30B9C6C8}</MetaDataID>
		protected void DefineColors()
		{
			// Define starting text colors
			_defaultTextColor = true;
			_defaultHighlightTextColor = true;
			_defaultSelectedTextColor = true;
			_defaultPlainSelectedTextColor = true;
			_textColor = SystemColors.MenuText;
			_highlightTextColor = SystemColors.MenuText;
			_selectedTextColor = SystemColors.MenuText;
			_plainSelectedTextColor = SystemColors.ActiveCaptionText;

			// Define starting back colors
			_defaultBackColor = true;
			_defaultHighlightBackColor = true;
			_defaultSelectedBackColor = true;
			base.BackColor = SystemColors.Control;
			_backBrush = new SolidBrush(base.BackColor);
			_highlightBackColor = SystemColors.Highlight;            
			DefineHighlightBackColors(SystemColors.Highlight);
			DefineSelectedBackColors(SystemColors.Control);
		}
        
		/// <MetaDataID>{71AE60D3-EDD2-4C9B-914B-2CEF715685DF}</MetaDataID>
		public void ResetColors()
		{
			this.BackColor = SystemColors.Control;
			this.TextColor = SystemColors.MenuText;
			this.HighlightBackColor = SystemColors.Highlight;            
			this.HighlightTextColor = SystemColors.MenuText;
			this.SelectedBackColor = SystemColors.Control;
			this.SelectedTextColor = SystemColors.MenuText;
		}

		/// <MetaDataID>{ACAE11D8-BB7A-4F08-8956-E63423B1A661}</MetaDataID>
		protected void DefineFont(Font newFont)
		{
			base.Font = newFont;

			_breadthGap = (this.Font.Height / 5) + 1;

			// Calculate the initial height/width of the control
			_rowWidth = _rowHeight = this.Font.Height + _breadthGap * 2 + 1;
			_ImageRowHeight= _ImageRowWidth=21;
		}

		/// <MetaDataID>{4B49740F-691B-4B87-B217-7333BFD1BD4C}</MetaDataID>
		protected void DefineSelectedBackColors(Color baseColor)
		{
			_selectedBackColor = baseColor;
			_controlLPen = new Pen(Color.FromArgb(200, baseColor));
			_controlLBrush = new SolidBrush(Color.FromArgb(200, baseColor));
		}

		/// <MetaDataID>{4DAE0523-6B82-4328-899E-90DF0B3F9F92}</MetaDataID>
		protected void DefineHighlightBackColors(Color baseColor)
		{
			_highlightBackColor = baseColor;
        
			if (_defaultHighlightBackColor)
			{
				_highlightBackDark = SystemColors.Highlight;
				_highlightBackLight = Color.FromArgb(70, _highlightBackDark);
			}
			else
			{
				_highlightBackDark = ControlPaint.Dark(baseColor);
				_highlightBackLight = baseColor;
			}
		}

		/// <MetaDataID>{DA109966-0852-4357-BD0C-D0D2EEC34377}</MetaDataID>
		public virtual void OnSelected(MenuCommand mc)
		{
			_toolTip.SetToolTip(this, mc.ToolTipText);
			// Any attached event handlers?
			if (Selected != null)
				Selected(mc);
		}

		/// <MetaDataID>{AC868E0F-1B77-4FF6-91D8-95342F42648B}</MetaDataID>
		public virtual void OnDeselected(MenuCommand mc)
		{
			// Any attached event handlers?
			if (Deselected != null)
				Deselected(mc);
		}

		/// <MetaDataID>{0DB9E8B3-DD99-4CE7-909A-8CF0FED906E1}</MetaDataID>
		protected void OnCollectionCleared()
		{
			// Reset state ready for a recalculation
			Deselect();
			RemoveItemTracking();

			Recalculate();
			Invalidate();
		}

		/// <MetaDataID>{DEFC4374-6846-4B68-BAAF-41DF43A2A853}</MetaDataID>
		protected void OnCollectionInserted(int index, object value)
		{
			MenuCommand mc = value as MenuCommand;

			// We need notification whenever the properties of this command change
			mc.PropertyChanged += new MenuCommand.PropChangeHandler(OnCommandChanged);
				
			// Reset state ready for a recalculation
			Deselect();
			RemoveItemTracking();

			Recalculate();
			Invalidate();
		}

		/// <MetaDataID>{13DD90EA-460C-4CC4-B75B-5DB283D3FBB6}</MetaDataID>
		protected void OnCollectionRemoved(int index, object value)
		{
			// Reset state ready for a recalculation
			Deselect();
			RemoveItemTracking();

			Recalculate();
			Invalidate();
		}

		/// <MetaDataID>{67850A68-7E47-4A5F-AE9A-0FD8BBA17F91}</MetaDataID>
		protected void OnCommandChanged(MenuCommand item, MenuCommand.Property prop)
		{
			Recalculate();
			Invalidate();
		}

		/// <MetaDataID>{54E6DF42-C5A1-4B5C-9E65-36F0413CB508}</MetaDataID>
		protected void OnMdiChildActivate(object sender, EventArgs e)
		{
			// Unhook from event
			if (_activeChild != null)
				_activeChild.SizeChanged -= new EventHandler(OnMdiChildSizeChanged);

			// Remember the currently active child form
			_activeChild = _mdiContainer.ActiveMdiChild;

			// Need to know when window becomes maximized
			if (_activeChild != null)
				_activeChild.SizeChanged += new EventHandler(OnMdiChildSizeChanged);

			// Might be a change in pendant requirements
			Recalculate();
			Invalidate();
		}

		/// <MetaDataID>{B429EAFB-0A66-4B41-A7E5-323449754424}</MetaDataID>
		protected void OnMdiChildSizeChanged(object sender, EventArgs e)
		{
			// Has window changed to become maximized?
			if (_activeChild.WindowState == FormWindowState.Maximized)
			{
				// Reflect change in menu
				Recalculate();
				Invalidate();
			}
		}

		/// <MetaDataID>{571F38DF-958C-4F1E-865A-BC6CEB7E1709}</MetaDataID>
		protected void OnMdiMin(object sender, EventArgs e)
		{
			if (_activeChild != null)
			{
				_activeChild.WindowState = FormWindowState.Minimized;

				// Reflect change in menu
				Recalculate();
				Invalidate();
			}
		}

		/// <MetaDataID>{648461A8-6954-4093-8C31-24952333A811}</MetaDataID>
		protected void OnMdiRestore(object sender, EventArgs e)
		{
			if (_activeChild != null)
			{
				_activeChild.WindowState = FormWindowState.Normal;

				// Reflect change in menu
				Recalculate();
				Invalidate();
			}
		}

		/// <MetaDataID>{E7B21F0A-5151-42E9-A956-DFC0CE67D651}</MetaDataID>
		protected void OnMdiClose(object sender, EventArgs e)
		{
			if (_activeChild != null)
			{
				_activeChild.Close();

				// Reflect change in menu
				Recalculate();
				Invalidate();
			}
		}

		/// <MetaDataID>{D016C3E8-0831-40D4-8067-F39005AE8A3A}</MetaDataID>
		protected void CreatePendantButtons()
		{
			// Create the objects
			_minButton = new InertButton(_menuImages, _minIndex);
			_restoreButton = new InertButton(_menuImages, _restoreIndex);
			_closeButton = new InertButton(_menuImages, _closeIndex);

			// Define the constant sizes
			_minButton.Size = new Size(_buttonLength, _buttonLength);
			_restoreButton.Size = new Size(_buttonLength, _buttonLength);
			_closeButton.Size = new Size(_buttonLength, _buttonLength);

			// Default their position so they are not visible
			_minButton.Location = new Point(-_buttonLength, -_buttonLength);
			_restoreButton.Location = new Point(-_buttonLength, -_buttonLength);
			_closeButton.Location = new Point(-_buttonLength, -_buttonLength);

			// Hookup event handlers
			_minButton.Click += new EventHandler(OnMdiMin);
			_restoreButton.Click += new EventHandler(OnMdiRestore);
			_closeButton.Click += new EventHandler(OnMdiClose);

			// Add to display
			this.Controls.AddRange(new Control[]{_minButton, _restoreButton, _closeButton});
		}

		/// <MetaDataID>{BA188C29-B7D2-4892-BF91-8E3D1222E934}</MetaDataID>
		protected void RemovePendantButtons()
		{
			// Unhook event handlers
			_minButton.Click -= new EventHandler(OnMdiMin);
			_restoreButton.Click -= new EventHandler(OnMdiRestore);
			_closeButton.Click -= new EventHandler(OnMdiClose);

			// Remove from display

			// Use helper method to circumvent form Close bug
			ControlHelper.Remove(this.Controls, _minButton);
			ControlHelper.Remove(this.Controls, _restoreButton);
			ControlHelper.Remove(this.Controls, _closeButton);

			// Release resources
			_minButton.Dispose();
			_restoreButton.Dispose();
			_closeButton.Dispose();

			// Remove references
			_minButton = null;
			_restoreButton = null;
			_closeButton = null;
		}
        
		/// <MetaDataID>{34F606C9-719F-45BC-ADB2-4D83CCE749E4}</MetaDataID>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			// Have we become disabled?
			if (!this.Enabled)
			{
				// Is an item selected?
				if (_selected)
				{
					// Is a popupMenu showing?
					if (_popupMenu != null)
					{
						// Dismiss the submenu
						_popupMenu.Dismiss();

						// No reference needed
						_popupMenu = null;
					}

					// Reset state
					Deselect();
					_drawUpwards = false;

					SimulateReturnFocus();
				}
			}

			// Do not draw any item as being tracked
			RemoveItemTracking();

			// Change in state changes the way items are drawn
			Invalidate();
		}

		/// <MetaDataID>{97422DF1-03EA-42A6-8D9A-8A3323A02963}</MetaDataID>
		internal void OnWM_MOUSEDOWN(Win32.POINT screenPos)
		{
			// Convert the mouse position to screen coordinates
			User32.ScreenToClient(this.Handle, ref screenPos);

			OnProcessMouseDown(screenPos.x, screenPos.y);
		}

		/// <MetaDataID>{90CD7CFB-615F-4C7A-91EE-C3B0ABADE949}</MetaDataID>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			OnProcessMouseDown(e.X, e.Y);

			base.OnMouseDown(e);
		}
        /// <MetaDataID>{01c2869c-5d38-441f-af14-9a7a4adcc62e}</MetaDataID>
		bool MouseDown=false;
        /// <MetaDataID>{0bd66f36-a7c6-47c4-8593-46e3362cf0c4}</MetaDataID>
		Point MouseDownPos;

		/// <MetaDataID>{E907F91D-C81C-4AF9-9AA1-C1DE71A3008B}</MetaDataID>
		protected void OnProcessMouseDown(int xPos, int yPos)
		{
			//System.Diagnostics.Debug.WriteLine("OnProcessMouseDown");

			
			if(Control.MouseButtons==MouseButtons.Left)
				MouseDown=true;
			MouseDownPos=Control.MousePosition;

			Point pos = new Point(xPos, yPos);

			for(int i=0; i<_drawCommands.Count; i++)
			{
				DrawCommand dc = _drawCommands[i] as DrawCommand;

				// Find the DrawCommand this is over
				if (dc.SelectRect.Contains(pos) && dc.Enabled||dc.SelectRect.Contains(pos) &&_DesignMode)
				{

					// Is an item already selected?
					if (_selected)
					{
						// Is it this item that is already selected?
						if (_trackItem == i)
						{
							// Is a popupMenu showing
							if (_popupMenu != null)
							{
								// Dismiss the submenu
								_popupMenu.Dismiss();

								// No reference needed
								_popupMenu = null;
							}
						}
					}
					else
					{
						// Select the tracked item
						_selected = true;
						_drawUpwards = false;
								
						// Is there a change in tracking?
						if (_trackItem != i)
						{
							// Modify the display of the two items 
							_trackItem = SwitchTrackingItem(_trackItem, i);
						}
						else
						{
							// Update display to show as selected
							DrawCommand(_trackItem, true);
						}

						// Is there a submenu to show?
                        if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
                        {
                            User32.PostMessage(this.Handle, WM_OPERATEMENU, 1, 0);
                        }
                        else if(DesignMode)
                        {
                            dc.MenuCommand.MenuCommands.Add(new CreateMenuCommand("Type Here"));
                            User32.PostMessage(this.Handle, WM_OPERATEMENU, 1, 0);
                        }
					}

					break;
				}
			}
		}

		/// <MetaDataID>{310F3338-DA06-4E9E-8C66-C05CC32E0225}</MetaDataID>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			//MessageBox.Show("OnMouseUp");
			MouseDown=false;
			if(DragEnterFlag)
			{
				if(DropCommand!=null)
					DropCommand(this,null);
			}

			// Is an item currently being tracked?
			if (_trackItem != -1)
			{
				// Is it also selected?
				if (_selected == true)
				{
					// Is it also showing a submenu
					if (_popupMenu == null)
					{
						// Deselect the item
						Deselect();
						_drawUpwards = false;

						DrawCommand(_trackItem, true);

						SimulateReturnFocus();



						Point pos = new Point(e.X, e.Y);

						for(int i=0; i<_drawCommands.Count; i++)
						{
							DrawCommand dc = _drawCommands[i] as DrawCommand;

							// Find the DrawCommand this is over
							if (dc.SelectRect.Contains(pos) && dc.Enabled)
							{
								dc.MenuCommand.OnClick(e);
							}
						}
					}
				}
			}

			base.OnMouseUp(e);
		}

		/// <MetaDataID>{A0BE8417-040F-4C0F-A843-A5577EDA953A}</MetaDataID>
		internal void OnWM_MOUSEMOVE(Win32.POINT screenPos)
		{
			// Convert the mouse position to screen coordinates
			User32.ScreenToClient(this.Handle, ref screenPos);

			OnProcessMouseMove(screenPos.x, screenPos.y);
		}

		/// <MetaDataID>{F95996C2-C4C4-450D-AB21-940215ECE9F9}</MetaDataID>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Sometimes we need to ignore this message
			if (_ignoreMouseMove)
				_ignoreMouseMove = false;
			else
				OnProcessMouseMove(e.X, e.Y);

			base.OnMouseMove(e);
		}
        
		/// <MetaDataID>{C4BAD7EC-69AA-4090-8EF4-DD85BBE90A51}</MetaDataID>
		protected void OnProcessMouseMove(int xPos, int yPos)
		{
            try
            {
                if (DragEnterFlag)
                {
                    if (DragCommandMove != null)
                        DragCommandMove(this, null);
                }

                if (MouseDown && _DesignMode && !DragEnterFlag)
                {
                    int XDistance = MouseDownPos.X - Control.MousePosition.X;
                    if (XDistance < 0)
                        XDistance = -XDistance;
                    int YDistance = MouseDownPos.Y - Control.MousePosition.Y;
                    if (YDistance < 0)
                        YDistance = -YDistance;
                    if ((YDistance > 4 || XDistance > 4) && _trackItem != -1)
                    {

                        MenuCommand DragedMenuCommand = (_drawCommands[_trackItem] as DrawCommand).MenuCommand;
                        if (DragedMenuCommand != null && _CommandBarManager!=null)
                            _CommandBarManager.OnDragCommandEnterDispacher(this, ref DragedMenuCommand, _Menu);
                    }

                }


                // Sometimes we need to ignore this message
                if (_ignoreMouseMove)
                    _ignoreMouseMove = false;
                else
                {


                    // Is the first time we have noticed a mouse movement over our window
                    if (!_mouseOver)
                    {
                        // Create the structure needed for User32 call
                        Win32.TRACKMOUSEEVENTS tme = new Win32.TRACKMOUSEEVENTS();

                        // Fill in the structure
                        tme.cbSize = 16;
                        tme.dwFlags = (uint)Win32.TrackerEventFlags.TME_LEAVE;
                        tme.hWnd = this.Handle;
                        tme.dwHoverTime = 0;

                        // Request that a message gets sent when mouse leaves this window
                        User32.TrackMouseEvent(ref tme);

                        // Yes, we know the mouse is over window
                        _mouseOver = true;
                    }

                    Form parentForm = this.FindForm();

                    // Only hot track if this Form is active
                    if ((parentForm != null) && parentForm.ContainsFocus)
                    {
                        Point pos = new Point(xPos, yPos);

                        int i = 0;

                        for (i = 0; i < _drawCommands.Count; i++)
                        {
                            DrawCommand dc = _drawCommands[i] as DrawCommand;

                            // Find the DrawCommand this is over
                            if (dc.SelectRect.Contains(pos) && dc.Enabled)
                            {
                                // Is there a change in selected item?
                                if (_trackItem != i)
                                {
                                    // We are currently selecting an item
                                    if (_selected)
                                    {
                                        if (_popupMenu != null)
                                        {
                                            // Note that we are dismissing the submenu but not removing
                                            // the selection of items, this flag is tested by the routine
                                            // that will return because the submenu has been finished
                                            _dismissTransfer = true;

                                            // Dismiss the submenu
                                            _popupMenu.Dismiss();

                                            // Default to downward drawing
                                            _drawUpwards = false;
                                        }

                                        // Modify the display of the two items 
                                        _trackItem = SwitchTrackingItem(_trackItem, i);

                                        // Does the newly selected item have a submenu?
                                        if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
                                            User32.PostMessage(this.Handle, WM_OPERATEMENU, 1, 0);
                                    }
                                    else
                                    {
                                        // Modify the display of the two items 
                                        _trackItem = SwitchTrackingItem(_trackItem, i);
                                    }
                                }

                                break;
                            }
                        }

                        // If not in selected mode
                        if (!_selected)
                        {
                            // None of the commands match?
                            if (i == _drawCommands.Count)
                            {
                                // If we have the focus then do not change the tracked item
                                if (!_manualFocus)
                                {
                                    // Modify the display of the two items 
                                    _trackItem = SwitchTrackingItem(_trackItem, -1);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
		}

		/// <MetaDataID>{A17F634F-D2CE-4A08-9B17-B7F0F6658395}</MetaDataID>
		protected override void OnMouseLeave(EventArgs e)
		{
			
			_mouseOver = false;

			// If we manually grabbed focus then do not switch
			// selection when the mouse leaves the control area
			if (!_manualFocus)
			{
				if (_trackItem != -1)
				{
					// If an item is selected then do not change tracking item when the 
					// mouse leaves the control area, as a popup menu might be showing and 
					// so keep the tracking and selection indication visible
					if (_selected == false)
						_trackItem = SwitchTrackingItem(_trackItem, -1);
				}
			}

			base.OnMouseLeave(e);
		}

		/// <MetaDataID>{841855DA-AEF6-469C-A8E6-EE0638320319}</MetaDataID>
		protected override void OnResize(EventArgs e)
		{
			Recalculate();

			// Any resize of control should redraw all of it otherwise when you 
			// stretch to the right it will not paint correctly as we have a one
			// pixel gap between text and min button which is not honoured otherwise
			this.Invalidate();

			base.OnResize(e);
		}

		/// <MetaDataID>{6D50F02A-BFCC-40C8-BDFE-E84B7E3E2C92}</MetaDataID>
		internal void DrawSelectionUpwards()
		{
			// Double check the state is correct for this method to be called
			if ((_trackItem != -1) && (_selected))
			{
				// This flag is tested in the DrawCommand method
				_drawUpwards = true;

				// Force immediate redraw of the item
				DrawCommand(_trackItem, true);
			}
		}

		/// <MetaDataID>{0B8A0527-40FD-43EB-A812-B2DB272413EA}</MetaDataID>
		protected void Deselect()
		{
			// The next submenu should be animated
			_animateFirst = true;

			// Remove selection state
			_selected = false;
            
			// Should expansion items be reset on deselection?
			if (_deselectReset)
			{
				// Set everything to expanded
				SetAllCommandsExpansion(_menuCommands, false);
			}
		}

		/// <MetaDataID>{BF841DB8-9424-4D00-817F-AFB10207C7B5}</MetaDataID>
		protected internal void Recalculate()
		{
			int length;

			if (_direction == Direction.Horizontal)
				length = this.Width;
			else 
				length = this.Height;

			// Is there space for any commands?
			if (length > 0)
			{
				// Count the number of rows needed
				int rows = 0;

				// Number of items on this row
				int columns = 0;

				// Create a collection of drawing objects
				_drawCommands = new ArrayList();

				// Minimum length is a gap on either side of the text
				int cellMinLength = _lengthGap * 2;

				// Each cell is as broad as the whole control
				int cellBreadth = this.Height;
				
				// Accumulate starting position of each cell
				int lengthStart = 0;

				// Allow enough space to draw a chevron
				length -= (cellMinLength + _chevronLength);

				bool showPendant = ((rows == 0) && (_activeChild != null));

				// If we are showing on a single line but the active child is not 
				// currently maximized then we can show a menu item in pendant space
				if (showPendant && !_multiLine && (_activeChild.WindowState != FormWindowState.Maximized))
					showPendant = false;

				// Pendant positioning information
				int xPos = 0; 
				int yPos = 0;
				int xInc = 0;
				int yInc = 0;

				// First line needs extra space for pendant
				if (showPendant)
				{
					length -= (_pendantLength + _pendantOffset + _shadowGap);

					bool popupStyle = (_style == VisualStyle.IDE);
					int borderWidth = (_style == VisualStyle.IDE) ? 1 : 2;

					// Define the correct visual style
					_minButton.PopupStyle = popupStyle;
					_restoreButton.PopupStyle = popupStyle;
					_closeButton.PopupStyle = popupStyle;

					// Define correct border width
					_minButton.BorderWidth = borderWidth;
					_restoreButton.BorderWidth = borderWidth;
					_closeButton.BorderWidth = borderWidth;

					if (_direction == Direction.Horizontal)
					{
						xPos = this.Width - _pendantOffset - _buttonLength;
						yPos = _pendantOffset;
						xInc = -_buttonLength;
					}
					else
					{
						xPos = _pendantOffset;
						yPos = this.Height - _pendantOffset - _buttonLength;
						yInc = -_buttonLength;
					}
				}

				// Assume chevron is not needed by default
				_chevronStartCommand = null;

				using(Graphics g = this.CreateGraphics())
				{
					// Count the item we are processing
					int index = 0;

					foreach(MenuCommand command in _menuCommands)
					{
						// Give the command a chance to update its state
						command.OnUpdate(EventArgs.Empty);

						// Ignore items that are marked as hidden
						if (!command.Visible)
							continue;

						int cellLength = 0;

						// Is this a separator?
						if (command.Text == "-")
							cellLength = _separatorWidth;
						else
						{
							// Calculate the text width of the cell
							SizeF dimension = g.MeasureString(command.Text, this.Font);

							// Always add 1 to ensure that rounding is up and not down
							cellLength = cellMinLength + (int)dimension.Width + 1;
						}

						Rectangle cellRect;

						// Create a new position rectangle
						if (_direction == Direction.Horizontal)
						{
							if(command is CommandBarButton)
							{
								cellRect = new Rectangle(lengthStart, _ImageRowHeight * rows, cellLength, _ImageRowHeight);
								if (_ImageRowHeight<_rowHeight)
									cellRect.Y+=_rowHeight/2-_ImageRowHeight/2;
								cellRect.Width=cellRect.Height;
								cellLength=cellRect.Width+1;

							}
							else
							{
								cellRect = new Rectangle(lengthStart, _rowHeight * rows, cellLength, _rowHeight);
								if(command.Image!=null)
								{
									cellRect.Width+=_ImageRowWidth;
									//selectRect.Width+=_ImageRowWidth;
									cellLength=cellRect.Width+1;//_ImageRowWidth;
									
								}
							}
						}
						else
						{
							if(command is CommandBarButton)
							{
								cellRect = new Rectangle(_ImageRowWidth * rows, lengthStart, _ImageRowWidth, cellLength);
								if (_ImageRowWidth<_rowWidth)
									cellRect.X+=_rowWidth/2-_ImageRowWidth/2;
								cellRect.Height=cellRect.Width;
								cellLength+=_ImageRowHeight;

							}
							else
							{
								cellRect = new Rectangle(_rowWidth * rows, lengthStart, _rowWidth, cellLength);
								if(command.Image!=null)
								{
									cellRect.Height+=_ImageRowHeight;
									//selectRect.Height+=_ImageRowHeight;
									cellLength=cellRect.Height+1;
									
								}
							}

						}

						lengthStart += cellLength;
						columns++;

						// If this item is overlapping the control edge and it is not the first
						// item on the line then we should wrap around to the next row.
						if ((lengthStart > length) && (columns > 1))
						{
							if (_multiLine)
							{
								// Move to next row
								rows++;

								// Reset number of items on this column
								columns = 1;

								// Reset starting position of next item
								lengthStart = cellLength;

								// Reset position of this item
								if (_direction == Direction.Horizontal)
								{
									cellRect.X = 0;
									cellRect.Y += _rowHeight;
								}
								else
								{
									cellRect.X += _rowWidth;
									cellRect.Y = 0;
								}

								// Only the first line needs extra space for pendant
								if (showPendant && (rows == 1))
									length += (_pendantLength + _pendantOffset);
							}
							else
							{
								// Is a tracked item being make invisible
								if (index <= _trackItem)
								{
									// Need to remove tracking of this item
									RemoveItemTracking();
								}

								// Remember which item is first for the chevron submenu
								_chevronStartCommand = command;

								if (_direction == Direction.Horizontal)
								{
									cellRect.Y = 0;
									cellRect.Width = cellMinLength + _chevronLength;
									cellRect.X = this.Width - cellRect.Width;
									cellRect.Height = _rowHeight;
									xPos -= cellRect.Width;
								}
								else
								{
									cellRect.X = 0;
									cellRect.Height = cellMinLength + _chevronLength;
									cellRect.Y = this.Height - (cellMinLength + _chevronLength);
									cellRect.Width = _rowWidth;
									yPos -= cellRect.Height;
								}

								// Create a draw command for this chevron
								_drawCommands.Add(new DrawCommand(cellRect));

								// Exit, do not add the current item or any afterwards
								break;
							}
						}

						Rectangle selectRect = cellRect;
						/*if(command.ImageIndex!=-1)
						{
							if (_direction == Direction.Horizontal)
								selectRect.Width+=2*_ImageRowWidth;
							else
								selectRect.Height+=_ImageRowHeight;
						}*/

							
							


						// Selection rectangle differs from drawing rectangle with IDE, because pressing the
						// mouse down causes the menu to appear and because the popup menu appears drawn slightly
						// over the drawing area the mouse up might select the first item in the popup. 
						if (_style == VisualStyle.IDE)
						{
							// Modify depending on orientation
							if (_direction == Direction.Horizontal)
								selectRect.Height -= (_lengthGap + 2);
							else
								selectRect.Width -= _breadthGap;
						}
						/*if(command.ImageIndex!=-1)
						{
							if(command is CommandBarButton)
							{
								Image Image=command.ImageList.Images[command.ImageIndex];
								if(_direction==Direction.Horizontal)
								{
									cellRect.Width= Image.Size.Width;
									
								}
								else
								{
									cellRect.Height+= Image.Size.Height;
									
								}

							}
							else
							{
								Image Image=command.ImageList.Images[command.ImageIndex];
								if(_direction==Direction.Horizontal)
								{
									cellRect.Width+= Image.Size.Width;
									cellRect.Width+=Image.Size.Width;
								}
								else
								{
									cellRect.Height+= Image.Size.Height;
									cellRect.Height+=Image.Size.Height;
								}
							}

						}*/

						// Create a drawing object
						_drawCommands.Add(new DrawCommand(command, cellRect, selectRect));
						index++;
					}
				}

				// Position the pendant buttons
				if (showPendant)
				{
					if (_activeChild.WindowState == FormWindowState.Maximized)
					{
						// Window maximzied, must show the buttons
						if (!_minButton.Visible)
						{
							_minButton.Show();
							_restoreButton.Show();
							_closeButton.Show();
						}
	
						// Only enabled minimize box if child is allowed to be minimized
						_minButton.Enabled = _activeChild.MinimizeBox;

						_closeButton.Location = new Point(xPos, yPos);

						xPos += xInc; yPos += yInc;
						_restoreButton.Location = new Point(xPos, yPos);

						xPos += xInc; yPos += yInc;
						_minButton.Location = new Point(xPos, yPos);
					}
					else
					{
						// No window is maximized, so hide the buttons
						if (_minButton.Visible)
						{
							_minButton.Hide();
							_restoreButton.Hide();
							_closeButton.Hide();
						}
					}
				}
				else
				{
					// No window is maximized, so hide the buttons
					if ((_minButton != null) && _minButton.Visible)
					{
						_minButton.Hide();
						_restoreButton.Hide();
						_closeButton.Hide();
					}
				}

				if (_direction == Direction.Horizontal)
				{
					int controlHeight = (rows + 1) * _rowHeight;

					// Ensure the control is the correct height
					if (this.Height != controlHeight)
						this.Height = controlHeight;
				}
				else
				{
					int controlWidth = (rows + 1) * _rowWidth;

					// Ensure the control is the correct width
					if (this.Width != controlWidth)
						this.Width = controlWidth;
				}				
			}
		}

		/// <MetaDataID>{6BCA9A9A-8ECF-4A0C-A533-3641339C8377}</MetaDataID>
		protected void DrawCommand(int drawItem, bool tracked)
		{
			try
			{
				// Create a graphics object for drawing
				using(Graphics g = this.CreateGraphics())
					DrawSingleCommand(g, _drawCommands[drawItem] as DrawCommand, tracked);
			}
			catch(System.Exception err)
			{
				int tt=0;
			}
		}

		/// <MetaDataID>{5647F237-AE49-442A-8F46-55FE6EC5532C}</MetaDataID>
		internal void DrawSingleCommand(Graphics g, DrawCommand dc, bool tracked)
		{
            //return;
			Rectangle drawRect = dc.DrawRect;
			MenuCommand mc = dc.MenuCommand;

			// Copy the rectangle used for drawing cell
			Rectangle shadowRect = drawRect;

			// Expand to right and bottom to cover the area used to draw shadows
			shadowRect.Width += _shadowGap;
			shadowRect.Height += _shadowGap;

			// Draw background color over cell and shadow area to the right
			g.FillRectangle(_backBrush, shadowRect);
			

			if (!dc.Separator)
			{
				Rectangle textRect;
				Rectangle netTextRect;
				Rectangle ImageRect=new Rectangle(0,0,0,0);



				// Text rectangle size depends on type of draw command we are drawing
				if (dc.Chevron)
				{
					// Create chevron drawing rectangle
					textRect = new Rectangle(drawRect.Left + _lengthGap, drawRect.Top + _boxExpandUpper,
						drawRect.Width - _lengthGap * 2, drawRect.Height - (_boxExpandUpper * 2));
					netTextRect=textRect;
				}
				else
				{
					// Create text drawing rectangle
					textRect = new Rectangle(drawRect.Left + _lengthGap, drawRect.Top + _lengthGap,
						drawRect.Width - _lengthGap * 2, drawRect.Height - _lengthGap * 2);
					netTextRect=textRect;

					if(dc.MenuCommand.Image!=null)
					{
						Size ImageSize=dc.MenuCommand.Image.Size;
						if(_direction==Direction.Horizontal)
						{
							if(dc.MenuCommand is CommandBarButton)
							{
								int Side=drawRect.Height;
								netTextRect=drawRect;
								ImageRect=new Rectangle(drawRect.Left+Side/2-ImageSize.Width/2,drawRect.Top+Side/2-ImageSize.Height/2,ImageSize.Width,ImageSize.Height);;
							}
							else
							{
								netTextRect=new Rectangle(textRect.Left+ImageSize.Width,textRect.Top,textRect.Width-=ImageSize.Width,textRect.Height);
								ImageRect=new Rectangle(textRect.Left,textRect.Height/2-ImageSize.Height/2,ImageSize.Width,ImageSize.Height);
							}
						}
						else
						{
							if(dc.MenuCommand is CommandBarButton)
							{
								int Side=drawRect.Width;
								netTextRect=drawRect;
								ImageRect=new Rectangle(drawRect.Left+Side/2-ImageSize.Width/2,drawRect.Top+Side/2-ImageSize.Height/2,ImageSize.Width,ImageSize.Height);;
							}
							else
							{
								netTextRect=new Rectangle(textRect.Left,textRect.Top+ImageSize.Height,textRect.Width,textRect.Height-=ImageSize.Height);
								ImageRect=new Rectangle(textRect.Width/2-ImageSize.Width/2,textRect.Top,ImageSize.Width,ImageSize.Height);
							}
						}
					}
				}
				Rectangle boxRect;
				// Culculate the rectangle for box around the text
				if (_direction == Direction.Horizontal)
				{
					boxRect = new Rectangle(textRect.Left - _boxExpandSides,
						textRect.Top - _boxExpandUpper,
						textRect.Width + _boxExpandSides * 2,
						textRect.Height + _boxExpandUpper);
					if(dc.MenuCommand!=null)
						if(dc.MenuCommand.Image!=null)
							boxRect.Width+=_ImageRowWidth-4;

					if(dc.MenuCommand is CommandBarButton)
					{
						boxRect=drawRect;
						int Side=drawRect.Height;
						boxRect= new Rectangle(boxRect.X,boxRect.Y,Side,Side);

					}
				}
				else
				{					
					if (!dc.Chevron)
					{
						boxRect = new Rectangle(textRect.Left,
							textRect.Top - _boxExpandSides,
							textRect.Width - _boxExpandSides,
							textRect.Height + _boxExpandSides * 2);
						if(dc.MenuCommand is CommandBarButton)
						{
								
							boxRect=drawRect;
							int Side=drawRect.Width;
							boxRect= new Rectangle(boxRect.X,boxRect.Y,Side,Side);
						}
					}
					else
					{
						boxRect = textRect;
						if(dc.MenuCommand!=null)
							if(dc.MenuCommand.Image!=null)
								boxRect.Height+=_ImageRowHeight;
					}
				}

				if(dc.MenuCommand!=null)
				{
					if (dc.DragCommandCursorAtEnd)
					{
						
						if(_direction==Direction.Horizontal)
						{
							boxRect.Width-=4;
							using (Pen selectPen = new Pen(Color.FromArgb(0,0,0),2))
							{
								g.DrawLine(selectPen,new Point(boxRect.Right,boxRect.Top),new Point(boxRect.Right,boxRect.Bottom));
								g.DrawLine(selectPen,new Point(boxRect.Right-3,boxRect.Top+1),new Point(boxRect.Right+3,boxRect.Top+1));
								g.DrawLine(selectPen,new Point(boxRect.Right-3,boxRect.Bottom-1),new Point(boxRect.Right+3,boxRect.Bottom-1));
								//g.DrawRectangle(selectPen, boxRect);
							}

						}
						else
						{
							boxRect.Height-=4;
							using (Pen selectPen = new Pen(Color.FromArgb(0,0,0),2))
							{
								g.DrawLine(selectPen,new Point(boxRect.Left,boxRect.Bottom),new Point(boxRect.Right,boxRect.Bottom));
								g.DrawLine(selectPen,new Point(boxRect.Left+1,boxRect.Bottom-3),new Point(boxRect.Left+1,boxRect.Bottom+3));
								g.DrawLine(selectPen,new Point(boxRect.Right-1,boxRect.Bottom-3),new Point(boxRect.Right-1,boxRect.Bottom+3));
								//g.DrawRectangle(selectPen, boxRect);
							}
						}
					}

					if (dc.DragCommandCursorAtStart)
					{
						//Rectangle boxRect=drawRect;
						if(_direction==Direction.Horizontal)
						{
							boxRect.Width-=4;
							using (Pen selectPen = new Pen(Color.FromArgb(0,0,0),2))
							{
								g.DrawLine(selectPen,new Point(boxRect.Left,boxRect.Top),new Point(boxRect.Left,boxRect.Bottom));
								g.DrawLine(selectPen,new Point(boxRect.Left-3,boxRect.Top+1),new Point(boxRect.Left+3,boxRect.Top+1));
								g.DrawLine(selectPen,new Point(boxRect.Left-3,boxRect.Bottom-1),new Point(boxRect.Left+3,boxRect.Bottom-1));
								//g.DrawRectangle(selectPen, boxRect);
							}

						}
						else
						{
							boxRect.Height-=4;
							using (Pen selectPen = new Pen(Color.FromArgb(0,0,0),2))
							{
								g.DrawLine(selectPen,new Point(boxRect.Left,boxRect.Top),new Point(boxRect.Right,boxRect.Top));
								g.DrawLine(selectPen,new Point(boxRect.Left+1,boxRect.Top-3),new Point(boxRect.Left+1,boxRect.Top+3));
								g.DrawLine(selectPen,new Point(boxRect.Right-1,boxRect.Top-3),new Point(boxRect.Right-1,boxRect.Top+3));
								//g.DrawRectangle(selectPen, boxRect);
							}
						}
					}
				}
			
				if (dc.Enabled)
				{
					// Draw selection 

					if (tracked)
					{
						

						switch(_style)
						{
							case VisualStyle.IDE:
								if (_selected)
								{
									if(dc.MenuCommand is CommandBarButton)
									{
										using (Pen selectPen = new Pen(_highlightBackDark))
										{
											// Draw the selection area in white so can alpha draw over the top
											g.FillRectangle(Brushes.White, boxRect);

										
											using (SolidBrush selectBrush = new SolidBrush(Color.FromArgb(130, _highlightBackDark)))
											{
												// Draw the selection area
												g.FillRectangle(selectBrush, boxRect);

												// Draw a border around the selection area
												g.DrawRectangle(selectPen, boxRect);
											}
										}
									}
									else
									{
									
										// Fill the entire inside
										g.FillRectangle(Brushes.White, boxRect);
										g.FillRectangle(_controlLBrush, boxRect);
								
										Color extraColor = Color.FromArgb(64, 0, 0, 0);
										Color darkColor = Color.FromArgb(48, 0, 0, 0);
										Color lightColor = Color.FromArgb(0, 0, 0, 0);
                
										int rightLeft = boxRect.Right + 1;
										int rightBottom = boxRect.Bottom;

										if (_drawUpwards && (_direction == Direction.Horizontal))                                    
										{
											// Draw the box around the selection area
											using(Pen dark = new Pen(ControlPaint.Dark(_selectedBackColor)))
												g.DrawRectangle(dark, boxRect);
										

											if (dc.SubMenu)
											{
												// Right shadow
												int rightTop = boxRect.Top;
												int leftLeft = boxRect.Left + _shadowGap;

												// Bottom shadow
												int top = boxRect.Bottom + 1;
												int left = boxRect.Left + _shadowGap;
												int width = boxRect.Width + 1;
												int height = _shadowGap;

												Brush rightShadow;
												Brush bottomLeftShadow;
												Brush bottomShadow;
												Brush bottomRightShadow;

												// Decide if we need to use an alpha brush
												if (_supportsLayered)
												{
													// Create brushes
													rightShadow = new LinearGradientBrush(new Point(rightLeft, 9999),
														new Point(rightLeft + _shadowGap, 9999),
														darkColor, lightColor);

													bottomLeftShadow = new LinearGradientBrush(new Point(left + _shadowGap, top - _shadowGap),
														new Point(left, top + height),
														extraColor, lightColor);

													bottomRightShadow = new LinearGradientBrush(new Point(left + width - _shadowGap - 2, top - _shadowGap - 2),
														new Point(left + width, top + height),
														extraColor, lightColor);

													bottomShadow = new LinearGradientBrush(new Point(9999, top),
														new Point(9999, top + height),
														darkColor, lightColor);
												}
												else
												{
													rightShadow = new SolidBrush(SystemColors.ControlDark);
													bottomLeftShadow = rightShadow;
													bottomShadow = rightShadow;
													bottomRightShadow = rightShadow;
												}

												// Draw each part of the shadow area
												g.FillRectangle(rightShadow, new Rectangle(rightLeft, rightTop, _shadowGap,  rightBottom - rightTop + 1));
												g.FillRectangle(bottomLeftShadow, left, top, _shadowGap, height);
												g.FillRectangle(bottomRightShadow, left + width - _shadowGap, top, _shadowGap, height);
												g.FillRectangle(bottomShadow, left + _shadowGap, top, width - _shadowGap * 2, height);

												// Dispose of brush objects		
												if (_supportsLayered)
												{
													rightShadow.Dispose();
													bottomLeftShadow.Dispose();
													bottomShadow.Dispose();
													bottomRightShadow.Dispose();
												}
												else
													rightShadow.Dispose();
											}
										}
										else
										{
											// Draw the box around the selection area
											using(Pen dark = new Pen(ControlPaint.Dark(_selectedBackColor)))
												g.DrawRectangle(dark, boxRect);

											if (dc.SubMenu)
											{
												if (_direction == Direction.Horizontal)
												{
													// Remove the bottom line of the selection area
													g.DrawLine(Pens.White, boxRect.Left, boxRect.Bottom, boxRect.Right, boxRect.Bottom);
													g.DrawLine(_controlLPen, boxRect.Left, boxRect.Bottom, boxRect.Right, boxRect.Bottom);

													int rightTop = boxRect.Top + _shadowYOffset;

													Brush shadowBrush;

													// Decide if we need to use an alpha brush
													if (_supportsLayered && (_style == VisualStyle.IDE))
													{
														using(LinearGradientBrush topBrush = new LinearGradientBrush(new Point(rightLeft - _shadowGap, rightTop + _shadowGap), 
																  new Point(rightLeft + _shadowGap, rightTop),
																  extraColor, lightColor))
														{
															g.FillRectangle(topBrush, new Rectangle(rightLeft, rightTop, _shadowGap, _shadowGap));
                        
															rightTop += _shadowGap;
														}

														shadowBrush = new LinearGradientBrush(new Point(rightLeft, 9999), 
															new Point(rightLeft + _shadowGap, 9999),
															darkColor, lightColor);
													}
													else
														shadowBrush = new SolidBrush(SystemColors.ControlDark);

													g.FillRectangle(shadowBrush, new Rectangle(rightLeft, rightTop, _shadowGap, rightBottom - rightTop));

													shadowBrush.Dispose();
												}
												else
												{

                                                    if (GapPosition != GapPosition.Right)
                                                    {
                                                        //// Remove the right line of the selection area
                                                        g.DrawLine(Pens.White, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                                                        g.DrawLine(_controlLPen, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                                                    }
                                                    else
                                                    {

                                                        // Remove the left line of the selection area
                                                        g.DrawLine(Pens.White, boxRect.Left, boxRect.Top, boxRect.Left, boxRect.Bottom);
                                                        g.DrawLine(_controlLPen, boxRect.Left, boxRect.Top, boxRect.Left, boxRect.Bottom);
                                                    }


													int leftLeft = boxRect.Left + _shadowYOffset;
                                                    int rightRight = boxRect.Right - _shadowYOffset;

                                                    if (GapPosition != GapPosition.Right)
                                                    {

                                                        Brush shadowBrush;

                                                        // Decide if we need to use an alpha brush
                                                        if (_supportsLayered && (_style == VisualStyle.IDE))
                                                        {
                                                            using (LinearGradientBrush topBrush = new LinearGradientBrush(new Point(leftLeft + _shadowGap, rightBottom + 1 - _shadowGap),
                                                                      new Point(leftLeft, rightBottom + 1 + _shadowGap),
                                                                      extraColor, lightColor))
                                                            {
                                                                g.FillRectangle(topBrush, new Rectangle(leftLeft, rightBottom + 1, _shadowGap, _shadowGap));

                                                                //leftLeft += _shadowGap;
                                                                leftLeft -= _shadowGap;
                                                            }

                                                            shadowBrush = new LinearGradientBrush(new Point(9999, rightBottom + 1),
                                                                new Point(9999, rightBottom + 1 + _shadowGap),
                                                                darkColor, lightColor);
                                                        }
                                                        else
                                                            shadowBrush = new SolidBrush(SystemColors.ControlDark);

                                                        g.FillRectangle(shadowBrush, new Rectangle(leftLeft, rightBottom + 1, rightBottom - leftLeft - _shadowGap, _shadowGap));

                                                        shadowBrush.Dispose();
                                                    }
                                                    else
                                                    {


                                                        Brush shadowBrush;
                                                        Brush vShadowBrush;


                                                        // Decide if we need to use an alpha brush
                                                        if (_supportsLayered && (_style == VisualStyle.IDE))
                                                        {
                                                            using (LinearGradientBrush topBrush = new LinearGradientBrush(new Point(rightRight, rightBottom + 1 - _shadowGap),
                                                                      new Point(rightRight - _shadowGap, rightBottom + 1 + _shadowGap),
                                                                      extraColor, lightColor))
                                                            {
                                                                g.FillRectangle(topBrush, new Rectangle(rightRight - _shadowGap, rightBottom + 1, _shadowGap, _shadowGap));

                                                                //leftLeft += _shadowGap;
                                                                leftLeft -= _shadowGap;
                                                            }

                                                            shadowBrush = new LinearGradientBrush(new Point(9999, rightBottom + 1),
                                                                new Point(9999, rightBottom + 1 + _shadowGap),
                                                                darkColor, lightColor);
                                                            vShadowBrush = new LinearGradientBrush(new Point(boxRect.Right + 1, 9999),
                                                            new Point(boxRect.Right + 1 + _shadowGap, 9999),
                                                            darkColor, lightColor);


                                                        }
                                                        else
                                                        {
                                                            shadowBrush = new SolidBrush(SystemColors.ControlDark);
                                                            vShadowBrush = shadowBrush;
                                                        }

                                                        g.FillRectangle(shadowBrush, new Rectangle(leftLeft - boxRect.Left, rightBottom + 1, rightBottom - leftLeft - _shadowGap, _shadowGap));
                                                        g.FillRectangle(vShadowBrush, new Rectangle(boxRect.Right, _shadowGap + boxRect.Top + 1, _shadowGap, boxRect.Height));

                                                        shadowBrush.Dispose();
                                                    }
												}
											}
										}
									}
								}
								else
								{
									using (Pen selectPen = new Pen(_highlightBackDark))
									{
										// Draw the selection area in white so can alpha draw over the top
										g.FillRectangle(Brushes.White, boxRect);

										
										using (SolidBrush selectBrush = new SolidBrush(_highlightBackLight))
										{
											// Draw the selection area
											g.FillRectangle(selectBrush, boxRect);

											// Draw a border around the selection area
											g.DrawRectangle(selectPen, boxRect);
										}
									}
								}
								break;
							case VisualStyle.Plain:
								if (_plainAsBlock)
								{
									using (SolidBrush selectBrush = new SolidBrush(_highlightBackDark))
										g.FillRectangle(selectBrush, drawRect);
								}
								else
								{
									if (_selected)
									{
										using(Pen lighlight = new Pen(ControlPaint.LightLight(this.BackColor)),
												  dark = new Pen(ControlPaint.DarkDark(this.BackColor)))
										{                                            
											g.DrawLine(dark, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
											g.DrawLine(dark, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
											g.DrawLine(lighlight, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
											g.DrawLine(lighlight, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
										}
									}
									else
									{
										using(Pen lighlight = new Pen(ControlPaint.LightLight(this.BackColor)),
												  dark = new Pen(ControlPaint.DarkDark(this.BackColor)))
										{
											g.DrawLine(lighlight, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
											g.DrawLine(lighlight, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
											g.DrawLine(dark, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
											g.DrawLine(dark, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
										}
									}
								}
								break;
						}
					}
				}

				if (dc.Chevron)
				{
					// Draw the chevron image in the centre of the text area
					int yPos = drawRect.Top;
					int xPos = drawRect.X + ((drawRect.Width - _chevronLength) / 2);

					// When selected...
					if (_selected)
					{
						// ...offset down and to the right
						xPos += 1;
						yPos += 1;
					}

					g.DrawImage(_menuImages.Images[_chevronIndex], xPos, yPos);
				}
				else
				{	
					// Left align the text drawing on a single line centered vertically
					// and process the & character to be shown as an underscore on next character
					StringFormat format = new StringFormat();
					format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;
					format.HotkeyPrefix = HotkeyPrefix.Show;

					if (_direction == Direction.Vertical)
						format.FormatFlags |= StringFormatFlags.DirectionVertical;

					if (dc.Enabled && this.Enabled)
					{
						if (tracked && (_style == VisualStyle.Plain) && _plainAsBlock)
						{
							// Is the item selected as well as tracked?
							if (_selected)
							{
								// Offset to show it is selected
								textRect.X += 2;
								textRect.Y += 2;
							}

							using (SolidBrush textBrush = new SolidBrush(_plainSelectedTextColor))
								g.DrawString(mc.Text, this.Font, textBrush, textRect, format);
						}
						else
						{
							if (_selected && tracked)
							{
								using (SolidBrush textBrush = new SolidBrush(_selectedTextColor))
									g.DrawString(mc.Text, this.Font, textBrush, netTextRect, format);
								if(dc.MenuCommand.Image!=null)
								{
									Image CommandImage=dc.MenuCommand.Image;
									g.DrawImage(CommandImage,ImageRect.X,ImageRect.Y);
								}

							}
							else
							{
								if (tracked)
								{
									using (SolidBrush textBrush = new SolidBrush(_highlightTextColor))
										g.DrawString(mc.Text, this.Font, textBrush, netTextRect, format);
									if(dc.MenuCommand.Image!=null)
									{
										Image CommandImage=dc.MenuCommand.Image;

										Bitmap shadowImage = new Bitmap((Bitmap) CommandImage);
										Color shadowColor = Color.FromArgb(154, 156, 146);
										Color transparent = Color.FromArgb(0, 0, 0, 0);

										for(int pixelX = 0; pixelX < CommandImage.Width; pixelX++)
										{
											for(int pixelY = 0; pixelY < CommandImage.Height; pixelY++)
											{
												if (shadowImage.GetPixel(pixelX, pixelY) != transparent)
													shadowImage.SetPixel(pixelX, pixelY, shadowColor);
											}
										}
		
										g.DrawImage(shadowImage, ImageRect.X + 1, ImageRect.Y + 1);

										// Draw an enabled icon offset up and left
										g.DrawImage(CommandImage, ImageRect.X - 1, ImageRect.Y - 1);
									}

								}
								else
								{
									using (SolidBrush textBrush = new SolidBrush(_textColor))
										g.DrawString(mc.Text, this.Font, textBrush, netTextRect, format);
									if(dc.MenuCommand.Image!=null)
									{
										Image CommandImage=dc.MenuCommand.Image;
										g.DrawImage(CommandImage,ImageRect.X,ImageRect.Y);
									}


								}
							}
						}
					}
					else 
					{
						// Helper values used when drawing grayed text in plain style
						Rectangle rectDownRight = textRect;
						rectDownRight.Offset(1,1);

						// Draw the text offset down and right
						g.DrawString(mc.Text, this.Font, Brushes.White, rectDownRight, format);

						// Draw then text offset up and left
						using (SolidBrush grayBrush = new SolidBrush(SystemColors.GrayText))
							g.DrawString(mc.Text, this.Font, grayBrush, textRect, format);
					}
					if(dc.MenuCommand!=null)
						if(dc.MenuCommand.IsDraged)
						{
							// Drow black rectangle around draged menucommand
							using (Pen selectPen = new Pen(Color.FromArgb(0,0,0),2))
							{
								g.DrawRectangle(selectPen, boxRect);
							}
						}

				}
			}
			

		}

		/// <MetaDataID>{381ED06B-0B3E-4B67-9148-4A01DD4E87C6}</MetaDataID>
		protected void DrawAllCommands(Graphics g)
		{
			for(int i=0; i<_drawCommands.Count; i++)
			{
				try
				{
					// Grab some commonly used values				
					DrawCommand dc = _drawCommands[i] as DrawCommand;

					// Draw this command only
					DrawSingleCommand(g, dc, (i == _trackItem));
				}
				catch(System.Exception Error)
				{
					throw new System.Exception(Error.Message,Error);
				}
			}
		}

		/// <MetaDataID>{4B6E8FAB-1F9C-41E3-B1E9-43D03EDCE321}</MetaDataID>
		protected int SwitchTrackingItem(int oldItem, int newItem)
		{
			_toolTip.RemoveAll();
			// Create a graphics object for drawinh
			using(Graphics g = this.CreateGraphics())
			{
				// Deselect the old draw command
				if (oldItem != -1)
				{
					DrawCommand dc = _drawCommands[oldItem] as DrawCommand;

					// Draw old item not selected
					DrawSingleCommand(g, _drawCommands[oldItem] as DrawCommand, false);

					// Generate an unselect event
					if (dc.MenuCommand != null)
						OnDeselected(dc.MenuCommand);
				}

				_trackItem = newItem;

				// Select the new draw command
				if (_trackItem != -1)
				{
					DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

					// Draw new item selected
					DrawSingleCommand(g, _drawCommands[_trackItem] as DrawCommand, true);
					
					// Generate an select event
					if (dc.MenuCommand != null)
						OnSelected(dc.MenuCommand);
				}

				// Force redraw of all items to prevent strange bug where some items
				// never get redrawn correctly and so leave blank spaces when using the
				// mouse/keyboard to shift between popup menus
				//DrawAllCommands(g);
				
			}

			
			return _trackItem;
		}

		/// <MetaDataID>{F20FF0AB-57B8-4855-B0A1-C0AB85A40314}</MetaDataID>
		protected void RemoveItemTracking()
		{
			if (_trackItem != -1)
			{
				DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

				// Generate an unselect event
				if (dc.MenuCommand != null)
					OnDeselected(dc.MenuCommand);

				// Remove tracking value
				_trackItem = -1;
			}		
		}
        /// <MetaDataID>{42619e82-19ba-433d-9b5a-eddaa28431fd}</MetaDataID>
        GapPosition GapPosition = GapPosition.None;

		/// <MetaDataID>{784DA32C-9E51-4F52-8C58-209FD0795BDF}</MetaDataID>
		internal void OperateSubMenu(DrawCommand dc, bool selectFirst, bool trackRemove)
		{
			if (this.IsDisposed)
				return;
                
			Rectangle drawRect = dc.DrawRect;

			// Find screen positions for popup menu
			Point screenPos;
            GapPosition = GapPosition.None;
            
			
			if (_style == VisualStyle.IDE)
			{
                if (_direction == Direction.Horizontal)
                    screenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Bottom - _lengthGap - 2));
                else
                {
                    screenPos = PointToScreen(new Point(dc.DrawRect.Right - _breadthGap, drawRect.Top + _boxExpandSides - 1));
                    if ((Screen.GetWorkingArea(screenPos).Width / 2) < screenPos.X)
                    {
                        screenPos = PointToScreen(new Point(dc.DrawRect.Left + _breadthGap, drawRect.Top + _boxExpandSides - 1));
                        GapPosition = GapPosition.Right;
                    }

                }
			}
			else
			{
				if (_direction == Direction.Horizontal)
					screenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Bottom));
				else
					screenPos = PointToScreen(new Point(dc.DrawRect.Right, drawRect.Top));
			}

			Point aboveScreenPos;
			
			if (_style == VisualStyle.IDE)
			{
				if (_direction == Direction.Horizontal)
					aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Top + _breadthGap + _lengthGap - 1));
				else
					aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Right - _breadthGap, drawRect.Bottom + _lengthGap + 1));
			}
			else
			{
				if (_direction == Direction.Horizontal)
					aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Left + 1, drawRect.Top));
				else
					aboveScreenPos = PointToScreen(new Point(dc.DrawRect.Right, drawRect.Bottom));
			}

			int borderGap;

			// Calculate the missing gap in the PopupMenu border
			if (_direction == Direction.Horizontal)
				borderGap = dc.DrawRect.Width - _subMenuBorderAdjust;
			else
				borderGap = dc.DrawRect.Height - _subMenuBorderAdjust;		
	
			
			if(_popupMenu !=null)
			{
				// only one first level popup menu active 
				// in design mode the the popup menus they are stay open after the left mouse button 
				//goes up (drag drop) 
				_popupMenu.Dismiss();
				_popupMenu=null;
			}
			_popupMenu = new PopupMenu(_CommandBarManager,DragEnterFlag,_DesignMode,_DragDropCursor);

			// Define the correct visual style based on ours
			_popupMenu.Style = this.Style;

			// Key direction when keys cause dismissal
			int returnDir = 0;

			// Command selected by the PopupMenu
			MenuCommand returnCommand = null;

			// Should the PopupMenu tell the collection to remember expansion state
			_popupMenu.RememberExpansion = _rememberExpansion;

			// Propogate our highlight setting
			_popupMenu.HighlightInfrequent = _highlightInfrequent;

			// Might need to define custom colors
			if (!_defaultSelectedBackColor)
				_popupMenu.BackColor = _selectedBackColor;
            
			if (!_defaultSelectedTextColor)
				_popupMenu.TextColor = _selectedTextColor;

			if (!_defaultHighlightTextColor)
				_popupMenu.HighlightTextColor = _highlightTextColor;

			if (!_defaultHighlightBackColor)
				_popupMenu.HighlightColor = _highlightBackColor;

			if (!_defaultFont)
				_popupMenu.Font = base.Font;

			// Pass on the animation values
			_popupMenu.Animate = _animate;
			_popupMenu.AnimateStyle = _animateStyle;
			_popupMenu.AnimateTime = _animateTime;
    
			if (dc.Chevron)
			{
				MessageBox.Show("Temporary Not Used Code");
				/*
				MenuCommandCollection mcc = new MenuCommandCollection();

				bool addCommands = false;

				// Generate a collection of menu commands for those not visible
				foreach(MenuCommand command in _menuCommands)
				{
					if (!addCommands && (command == _chevronStartCommand))
						addCommands = true;

					if (addCommands)
						mcc.Add(command,null);
				}

				// Track the popup using provided menu item collection
				returnCommand = _popupMenu.TrackPopup(screenPos, 
					aboveScreenPos,
					_direction,
					mcc, 
					borderGap,
					selectFirst, 
					this,
					_animateFirst,
					ref returnDir);
					*/
					
			}
			else
			{
				// Generate event so that caller has chance to modify MenuCommand contents
				dc.MenuCommand.OnPopupStart();
                
				// Honour the collections request for showing infrequent items
				_popupMenu.ShowInfrequent = dc.MenuCommand.MenuCommands.ShowInfrequent;

				// Track the popup using provided menu item collection
				returnCommand = _popupMenu.TrackPopup(screenPos, 
					aboveScreenPos,
					_direction,
					dc.MenuCommand, 
					borderGap,
                    GapPosition,
					selectFirst,
					this,
					_animateFirst,
					ref returnDir);
			}
            
			// No more animation till simulation ends
			_animateFirst = false;

			// If we are supposed to expand all items at the same time
			if (_expandAllTogether)
			{   
				// Is anything we have shown now in the expanded state
				if (AnythingExpanded(_menuCommands))
				{
					// Set everything to expanded
					SetAllCommandsExpansion(_menuCommands, true);
				}
			}
            
			// Was arrow key not used to dismiss the submenu?
			if (returnDir == 0)
			{
				// The submenu may have eaten the mouse leave event
				_mouseOver = false;

				// Only if the submenu was dismissed at the request of the submenu
				// should the selection mode be cancelled, otherwise keep selection mode
				if (!_dismissTransfer)
				{
					// This item is no longer selected
					Deselect();
					_drawUpwards = false;

					if (!this.IsDisposed)
					{
						// Should we stop tracking this item
						if (trackRemove)
						{
							// Unselect the current item
							_trackItem = SwitchTrackingItem(_trackItem, -1);
						}
						else
						{
							if (_trackItem != -1)
							{
								// Repaint the item
								DrawCommand(_trackItem, true);
							}
						}
					}
				}
				else
				{
					// Do not change _selected status
					_dismissTransfer = false;
				}
			}

			if (!dc.Chevron)
			{
				// Generate event so that caller has chance to modify MenuCommand contents
				dc.MenuCommand.OnPopupEnd();
			}

			// Spin the message loop so the messages dealing with destroying
			// the PopupMenu window are processed and cause it to disappear from
			// view before events are generated
			Application.DoEvents();

			// Remove unwanted object
		//	_popupMenu = null;

			// Was arrow key used to dismiss the submenu?
			if (returnDir != 0)
			{
				if (returnDir < 0)
				{
					// Shift selection left one
					ProcessMoveLeft(true);
				}
				else
				{
					// Shift selection right one
					ProcessMoveRight(true);
				}

				// A WM_MOUSEMOVE is generated when we open up the new submenu for 
				// display, ignore this as it causes the selection to move
				_ignoreMouseMove = true;
			}
			else
			{
				// Was a MenuCommand returned?
				if (returnCommand != null)
				{
					// Remove 

					// Are we simulating having the focus?
					if (_manualFocus)
					{
						// Always return focus to original when a selection is made
						SimulateReturnFocus();
					}

					// Pulse the selected event for the command
					returnCommand.OnClick(EventArgs.Empty);
				}
			}
		}

		/// <MetaDataID>{0A0A987B-865B-4E58-A245-D66066578CAC}</MetaDataID>
		internal void OnDragCommandDrop ( object sender, ref MenuCommand DropMenuCommand,MenuCommand OldCommandContainer )
		{

			MouseDown=false;
		
			Point pos = PointToClient(Control.MousePosition);
			if(!ClientRectangle.Contains(pos ))
			{
				if(!DragEnterFlag)
					return;
				RemoveDragCommandCursor();
				DragEnterFlag=false;
				//if(_popupMenu!=null)
					//_popupMenu.Dismiss();

				return;
			}

			DragEnterFlag=false;
			string mstring ="Liakos";
			
			
			int index=0;
			if(OldCommandContainer!=null)
			{
				if(OldCommandContainer.MenuCommands==_menuCommands)
				{
					for(int i=0; i<_drawCommands.Count; i++)
					{
						DrawCommand dc = _drawCommands[i] as DrawCommand;
						if(dc.MenuCommand==DropMenuCommand)
						{
							if(dc.DragCommandCursorAtEnd||dc.DragCommandCursorAtStart)
							{
								DropMenuCommand=null;
								DragEnterFlag=false;
								RemoveDragCommandCursor();
								return ;
							}
							if(index>i)
								index--;
						}
					}
				}
			}

			for(int i=0; i<_drawCommands.Count; i++)
			{
				DrawCommand dc = _drawCommands[i] as DrawCommand;
				if(dc.DragCommandCursorAtEnd||dc.DragCommandCursorAtStart)
				{
					if(DropMenuCommand!=null)
						if(OldCommandContainer!=null)
							OldCommandContainer.MenuCommands.Remove(DropMenuCommand);

					MenuCommand NewMenuCommand=DropMenuCommand;
					if(NewMenuCommand==null)
						NewMenuCommand=	new MenuCommand(mstring);

					if(dc.DragCommandCursorAtStart)
						_menuCommands.Insert(i, NewMenuCommand);
					else
						_menuCommands.Insert(i+1, NewMenuCommand);
					if(_popupMenu!=null)
					{
						_popupMenu.Dismiss();
						_popupMenu = null;
					}
					DropMenuCommand=null;
					Refresh();
					break;
				}
			}
		
		


			/* remove
			foreach(MenuCommand command in _menuCommands)
			{
				if(command.DragCommandCursorAtEnd||command.DragCommandCursorAtStart)
				{

					if(OldCommandContainer!=null)
					{
						if(OldCommandContainer.MenuCommands==_menuCommands)
						{
							for(int i=0; i<_drawCommands.Count; i++)
							{
								DrawCommand dc = _drawCommands[i] as DrawCommand;
								if(dc.MenuCommand==DropMenuCommand)
								{
									if(dc.DragCommandCursorAtEnd||dc.MenuCommand.DragCommandCursorAtStart)
									{
										DropMenuCommand=null;
										DragEnterFlag=false;
										RemoveDragCommandCursor();
										return ;
									}
									if(index>i)
										index--;
								}
							}
						}
					}

					if(DropMenuCommand!=null)
					{
						if(OldCommandContainer!=null)
						{
							OldCommandContainer.MenuCommands.Remove(DropMenuCommand);
						}
					}

					MenuCommand NewMenuCommand=DropMenuCommand;
					if(NewMenuCommand==null)
						NewMenuCommand=	new MenuCommand(mstring);

					if(command.DragCommandCursorAtStart)
						_menuCommands.Insert(index, NewMenuCommand);
					else
						_menuCommands.Insert(index+1, NewMenuCommand);
					if(_popupMenu!=null)
					{
						_popupMenu.Dismiss();
						_popupMenu = null;
					}

					DropMenuCommand=null;
					Refresh();


					break;
				}
				index++;
			}
		*/

			RemoveDragCommandCursor();
			Recalculate();
			Invalidate();

			if(Parent is Docking.DockingCommandBar.ClientArea)
			{
				((Docking.DockingCommandBar)Parent.Parent).RecalculateSize();
			}



			

		}
		/// <MetaDataID>{7EB52423-11FE-4A1C-A9FF-B3601DDF387C}</MetaDataID>
		internal void OnDragCommandEnter( object sender)
		{
			DragEnterFlag=true;

		}
        /// <MetaDataID>{8bafdf51-24c3-4053-bae4-a06adefc1a13}</MetaDataID>
		bool MenuCommandCursor=false;
		/// <MetaDataID>{14082C24-DAA4-4B8E-AE11-9E7BD97F6450}</MetaDataID>
		internal void OnDragCommandMove ( object sender,System.Windows.Forms.MouseEventArgs e )
		{

			
			Point pos = PointToClient(Control.MousePosition);
			//pos.Y+=5;
			if(!ClientRectangle.Contains(pos ))
			{
				if(MenuCommandCursor)
					RemoveDragCommandCursor();
				return;
			}
			bool InvalidateFlag=true;
			MenuCommandCursor=true;
			//Cursor=_DragStartCursor;
			
			for(int i=0; i<_drawCommands.Count; i++)
			{
				DrawCommand dc = _drawCommands[i] as DrawCommand;
				DrawCommand predc=null; 
				if(i>0)
					predc=_drawCommands[i-1] as DrawCommand;

				// Find the DrawCommand this is over
				if (dc.DrawRect.Contains(pos) )
				{
					int StartPos=0,MousePos=0,Length=0;
					if(_direction==Direction.Horizontal)
					{
						StartPos=dc.DrawRect.X;
						MousePos=pos.X;
						Length=dc.DrawRect.Width;
					}
					else
					{
						StartPos=dc.DrawRect.Y;
						MousePos=pos.Y;
						Length=dc.DrawRect.Height;

					}
					DrawCommand Activedc=dc;
					if(MousePos-StartPos<=Length/2)
					{
						if(predc!=null)
							Activedc=predc;
					}
					if((!Activedc.DragCommandCursorAtStart)&&(!Activedc.DragCommandCursorAtEnd))
						RemoveDragCommandCursor();
//					if(!Activedc.DragCommandCursorAtEnd)
					{
						Invalidate();
						if(dc.MenuCommand.MenuCommands.Count>0)
						{
							// don't expand Draged popup menu
							if(!dc.MenuCommand.IsDraged)
							{
								if(_popupMenu!=null)
								{
									if(_popupMenu.Menu!=dc.MenuCommand)
									{
										Point mPoint=PointToClient(Control.MousePosition);
										OnProcessMouseDown(mPoint.X,mPoint.Y);
									}
								}
								else
								{
									Point mPoint=PointToClient(Control.MousePosition);
									OnProcessMouseDown(mPoint.X,mPoint.Y);
								}
							}
							else
							{
								if(_popupMenu!=null)
								{
									if(_popupMenu.Menu==dc.MenuCommand)
									{
										//dismiss if it is already open
										_popupMenu.Dismiss();
										_popupMenu = null;
									}
								}
							}
						}
					}
					if(MousePos-StartPos<=Length/2&&i==0)
					{
						Activedc.DragCommandCursorAtStart=true;
						Activedc.DragCommandCursorAtEnd=false;
					}
					else
					{
						Activedc.DragCommandCursorAtStart=false;
						Activedc.DragCommandCursorAtEnd=true;
					}

					InvalidateFlag=false;
					break;
				}
			}
			if(InvalidateFlag)
				Invalidate();
		}

		/// <MetaDataID>{2CD1C044-EEEC-40C6-8C78-637C26145371}</MetaDataID>
		protected bool AnythingExpanded(MenuCommandCollection mcc)
		{
			foreach(MenuCommand mc in mcc)
			{
				if (mc.MenuCommands.ShowInfrequent)
					return true;
                    
				if (AnythingExpanded(mc.MenuCommands))
					return true;
			}
            
			return false;
		}
        
		/// <MetaDataID>{33EB6660-D031-444F-AA29-B321208D32F7}</MetaDataID>
		protected void SetAllCommandsExpansion(MenuCommandCollection mcc, bool show)
		{
			foreach(MenuCommand mc in mcc)
			{
				// Set the correct value for this collection
				mc.MenuCommands.ShowInfrequent = show;
                    
				// Recurse into all lower level collections
				SetAllCommandsExpansion(mc.MenuCommands, show);
			}
		}

		/// <MetaDataID>{793DE6DF-30BB-4279-9D9C-9FA3BECBCE64}</MetaDataID>
		protected void SimulateGrabFocus()
		{	
			if (!_manualFocus)
			{
				_manualFocus = true;
				_animateFirst = true;

				Form parentForm = this.FindForm();

				// Want notification when user selects a different Form
				parentForm.Deactivate += new EventHandler(OnParentDeactivate);

				// Must hide caret so user thinks focus has changed
				bool hideCaret = User32.HideCaret(IntPtr.Zero);

				// Create an object for storing windows message information
				Win32.MSG msg = new Win32.MSG();

				_exitLoop = false;

				// Process messages until exit condition recognised
				while(!_exitLoop)
				{
					// Suspend thread until a windows message has arrived
					if (User32.WaitMessage())
					{
						// Take a peek at the message details without removing from queue
						while(!_exitLoop && User32.PeekMessage(ref msg, 0, 0, 0, (int)Win32.PeekMessageFlags.PM_NOREMOVE))
						{
							//							Console.WriteLine("Loop {0} {1}", this.Handle, ((Win32.Msgs)msg.message).ToString());
    
							if (User32.GetMessage(ref msg, 0, 0, 0))
							{
								// Should this method be dispatched?
								if (!ProcessInterceptedMessage(ref msg))
								{
									User32.TranslateMessage(ref msg);
									User32.DispatchMessage(ref msg);
								}
							}
						}
					}
				}

				// Remove notification when user selects a different Form
				parentForm.Deactivate -= new EventHandler(OnParentDeactivate);

				// If caret was hidden then show it again now
				if (hideCaret)
					User32.ShowCaret(IntPtr.Zero);

				// We lost the focus
				_manualFocus = false;
			}
		}

		/// <MetaDataID>{AA118849-64C8-4C45-A880-326D5B60F1F5}</MetaDataID>
		protected void SimulateReturnFocus()
		{
			if (_manualFocus)
				_exitLoop = true;

			// Remove any item being tracked
			if (_trackItem != -1)
			{
				// Unselect the current item
				_trackItem = SwitchTrackingItem(_trackItem, -1);
			}
		}

		/// <MetaDataID>{38B7CCB3-9CC6-4640-895C-E005079B4089}</MetaDataID>
		protected void OnParentDeactivate(object sender, EventArgs e)
		{
			SimulateReturnFocus();
		}

		/// <MetaDataID>{11B1BA4F-A58B-4C06-90DC-5BFB8347A6DA}</MetaDataID>
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rectCaption = new Rectangle(0, 0, 10, 20);

	
			//DrawAllCommands(e.Graphics);
			base.OnPaint(e);
			
			/*	Color mColor=Color.FromArgb(0,0,0);
				using(SolidBrush backBrush = new SolidBrush(mColor))
				{
					e.Graphics.FillRectangle(backBrush, rectCaption);
				}*/
		}
        /// <MetaDataID>{7ff270b9-6bae-4dac-8e26-8c5237d4535d}</MetaDataID>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            //Brush m_Brush =new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.FromArgb(246, 244, 239), Color.FromArgb(192, 192, 168), 90); new SolidBrush(Color.BlueViolet);
            e.Graphics.FillRectangle(_backBrush, ClientRectangle);
            DrawAllCommands(e.Graphics);
            //m_Brush.Dispose();

        }

		/// <MetaDataID>{F0FBE635-86E2-40C1-9F12-253BC512A433}</MetaDataID>
		protected void ProcessMoveLeft(bool select)
		{
			if (_popupMenu == null)
			{
				int newItem = _trackItem;
				int startItem = newItem;

				for(int i=0; i<_drawCommands.Count; i++)
				{
					// Move to previous item
					newItem--;

					// Have we looped all the way around all choices
					if (newItem == startItem)
						return;

					// Check limits
					if (newItem < 0)
						newItem = _drawCommands.Count - 1;

					DrawCommand dc = _drawCommands[newItem] as DrawCommand;

					// Can we select this item?
					if (!dc.Separator && (dc.Chevron || dc.MenuCommand.Enabled))
					{
						// If a change has occured
						if (newItem != _trackItem)
						{
							// Modify the display of the two items 
							_trackItem = SwitchTrackingItem(_trackItem, newItem);
							
							if (_selected)
							{
								if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
									User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
							}

							break;
						}
					}
				}
			}
		}

		/// <MetaDataID>{B49138C2-8E43-4226-99FA-499916A33434}</MetaDataID>
		protected void ProcessMoveRight(bool select)
		{
			if (_popupMenu == null)
			{
				int newItem = _trackItem;
				int startItem = newItem;

				for(int i=0; i<_drawCommands.Count; i++)
				{
					// Move to previous item
					newItem++;

					// Check limits
					if (newItem >= _drawCommands.Count)
						newItem = 0;

					DrawCommand dc = _drawCommands[newItem] as DrawCommand;

					// Can we select this item?
					if (!dc.Separator && (dc.Chevron || dc.MenuCommand.Enabled))
					{
						// If a change has occured
						if (newItem != _trackItem)
						{
							// Modify the display of the two items 
							_trackItem = SwitchTrackingItem(_trackItem, newItem);

							if (_selected)
							{
								if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
									User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
							}

							break;
						}
					}
				}
			}
		}

		/// <MetaDataID>{36C25C8C-F539-41DB-98AE-DF3A02C1A8B4}</MetaDataID>
		protected void ProcessEnter()
		{
			if (_popupMenu == null)
			{
				// Are we tracking an item?
				if (_trackItem != -1)
				{
					// The item must not already be selected
					if (!_selected)
					{
						DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

						// Is there a submenu to show?
						if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count >= 0))
						{
							// Select the tracked item
							_selected = true;
							_drawUpwards = false;
										
							// Update display to show as selected
							DrawCommand(_trackItem, true);

							// Show the submenu

							if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
								User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
						}
						else
						{
							// No, pulse the Click event for the command
							dc.MenuCommand.OnClick(EventArgs.Empty);

							int item = _trackItem;

							// Not tracking anymore
							RemoveItemTracking();

							// Update display to show as not selected
							DrawCommand(item, false);

							// Finished, so return focus to origin
							SimulateReturnFocus();
						}
					}
					else
					{
						// Must be showing a submenu less item as selected
						DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

						// Pulse the event
						dc.MenuCommand.OnClick(EventArgs.Empty);

						int item = _trackItem;

						RemoveItemTracking();

						// Not selected anymore
						Deselect();
                        
						// Update display to show as not selected
						DrawCommand(item, false);

						// Finished, so return focus to origin
						SimulateReturnFocus();
					}
				}
			}
		}

		/// <MetaDataID>{34D2E698-8615-4E51-8815-1B47CDD76851}</MetaDataID>
		protected void ProcessMoveDown()
		{
			if (_popupMenu == null)
			{
				// Are we tracking an item?
				if (_trackItem != -1)
				{
					// The item must not already be selected
					if (!_selected)
					{
						DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

						// Is there a submenu to show?
						if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count >= 0))
						{
							// Select the tracked item
							_selected = true;
							_drawUpwards = false;
										
							// Update display to show as selected
							DrawCommand(_trackItem, true);

							// Show the submenu
							if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
								User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);
						}
					}
				}
			}
		}

		/// <MetaDataID>{6CD80C67-4CA9-4917-87E1-EBD3095E3867}</MetaDataID>
		protected bool ProcessMnemonicKey(char key)
		{
			// No current selection
			if (!_selected)
			{
				// Search for an item that matches
				for(int i=0; i<_drawCommands.Count; i++)
				{
					DrawCommand dc = _drawCommands[i] as DrawCommand;

					// Only interested in enabled items
					if ((dc.MenuCommand != null) && dc.MenuCommand.Enabled && dc.MenuCommand.Visible)
					{
						// Does the character match?
						if (key == dc.Mnemonic)
						{
							// Select the tracked item
							_selected = true;
							_drawUpwards = false;
										
							// Is there a change in tracking?
							if (_trackItem != i)
							{
								// Modify the display of the two items 
								_trackItem = SwitchTrackingItem(_trackItem, i);
							}
							else
							{
								// Update display to show as selected
								DrawCommand(_trackItem, true);
							}

							// Is there a submenu to show?
							if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count >= 0))
							{
								if (dc.Chevron || (dc.MenuCommand.MenuCommands.Count > 0))
									User32.PostMessage(this.Handle, WM_OPERATEMENU, 0, 1);

								return true;
							}
							else
							{			
								// No, pulse the Click event for the command
								dc.MenuCommand.OnClick(EventArgs.Empty);
	
								int item = _trackItem;

								RemoveItemTracking();

								// No longer seleted
								Deselect();
                                
								// Update display to show as not selected
								DrawCommand(item, false);

								// Finished, so return focus to origin
								SimulateReturnFocus();

								return false;
							}
						}
					}
				}
			}

			return false;
		}

		/// <MetaDataID>{5F7C5F68-974E-4C06-9A05-0BC83468A411}</MetaDataID>
		protected void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			// Are we using the default menu or a user defined value?
			if (_defaultFont)
			{
				DefineFont(SystemInformation.MenuFont);

				Recalculate();
				Invalidate();
			}
		}

		/// <MetaDataID>{986551B3-2ED0-4386-93C9-844BCA86CD5C}</MetaDataID>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			if (_defaultBackColor)
				this.BackColor = SystemColors.Control;
			
			if (_defaultHighlightBackColor)
				this.HighlightBackColor = SystemColors.Highlight;

			if (_defaultSelectedBackColor)
				this.SelectedBackColor = SystemColors.Control;
            
			if (_defaultTextColor)
				_textColor = SystemColors.MenuText;

			if (_defaultHighlightTextColor)
				_highlightTextColor = SystemColors.MenuText;

			if (_defaultSelectedTextColor)
				_selectedTextColor = SystemColors.MenuText;
            
			Recalculate();
			Invalidate();
    
			base.OnSystemColorsChanged(e);
		}
        
		/// <MetaDataID>{A3F5D589-CBCE-4380-96CC-93F60194C096}</MetaDataID>
		public bool PreFilterMessage(ref Message msg)
		{
			Form parentForm = this.FindForm();

			// Only interested if the Form we are on is activate (i.e. contains focus)
			if ((parentForm != null) && (parentForm == Form.ActiveForm) && parentForm.ContainsFocus)
			{		
				switch(msg.Msg)
				{
					case (int)Win32.Msgs.WM_KEYDOWN:
						// Ignore keyboard input if the control is disabled
						if (this.Enabled)
						{
							// Find up/down state of shift and control keys
							ushort shiftKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_SHIFT);
							ushort controlKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_CONTROL);

							// Basic code we are looking for is the key pressed...
							int code = (int)msg.WParam;

							// ...plus the modifier for SHIFT...
							if (((int)shiftKey & 0x00008000) != 0)
								code += 0x00010000;

							// ...plus the modifier for CONTROL
							if (((int)controlKey & 0x00008000) != 0)
								code += 0x00020000;

							// Construct shortcut from keystate and keychar
							Shortcut sc = (Shortcut)(code);

							// Search for a matching command
							return GenerateShortcut(sc, _menuCommands);
						}
						break;
					case (int)Win32.Msgs.WM_SYSKEYUP:
						// Ignore keyboard input if the control is disabled
						if (this.Enabled)
						{
							if ((int)msg.WParam == (int)Win32.VirtualKeys.VK_MENU)
							{
								// Are there any menu commands?
								if (_drawCommands.Count > 0)
								{
									// If no item is currently tracked then...
									if (_trackItem == -1)
									{
										// ...start tracking the first valid command
										for(int i=0; i<_drawCommands.Count; i++)
										{
											DrawCommand dc = _drawCommands[i] as DrawCommand;
											
											if (!dc.Separator && (dc.Chevron || dc.MenuCommand.Enabled))
											{
												_trackItem = SwitchTrackingItem(-1, i);
												break;
											}
										}
									}
											
									// Grab the focus for key events						
									SimulateGrabFocus();							
								}

								return true;
							}
						}
						break;
					case (int)Win32.Msgs.WM_SYSKEYDOWN:
						// Ignore keyboard input if the control is disabled
						if (this.Enabled)
						{
							if ((int)msg.WParam != (int)Win32.VirtualKeys.VK_MENU)
							{
								// Construct shortcut from ALT + keychar
								Shortcut sc = (Shortcut)(0x00040000 + (int)msg.WParam);
		
								if (GenerateShortcut(sc, _menuCommands))
									return true;
								
								// Last resort is treat as a potential mnemonic
								if (ProcessMnemonicKey((char)msg.WParam))
								{
									if (!_manualFocus)
										SimulateGrabFocus();

									return true;
								}
							}
						}
						break;
					default:
						break;
				}
			}

			return false;
		}

		/// <MetaDataID>{E4996528-2E43-46FC-9DC0-06B46D4C6A55}</MetaDataID>
		protected bool ProcessInterceptedMessage(ref Win32.MSG msg)
		{
			bool eat = false;
        
			switch(msg.message)
			{
				case (int)Win32.Msgs.WM_LBUTTONDOWN:
				case (int)Win32.Msgs.WM_MBUTTONDOWN:
				case (int)Win32.Msgs.WM_RBUTTONDOWN:
				case (int)Win32.Msgs.WM_XBUTTONDOWN:
				case (int)Win32.Msgs.WM_NCLBUTTONDOWN:
				case (int)Win32.Msgs.WM_NCMBUTTONDOWN:
				case (int)Win32.Msgs.WM_NCRBUTTONDOWN:
					// Mouse clicks cause the end of simulated focus unless they are
					// inside the client area of the menu control itself
					Point pt = new Point( (int)((uint)msg.lParam & 0x0000FFFFU), 
						(int)(((uint)msg.lParam & 0xFFFF0000U) >> 16));
				
					if (!this.ClientRectangle.Contains(pt))	
						SimulateReturnFocus();
					break;
				case (int)Win32.Msgs.WM_KEYDOWN:
					// Find up/down state of shift and control keys
					ushort shiftKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_SHIFT);
					ushort controlKey = User32.GetKeyState((int)Win32.VirtualKeys.VK_CONTROL);

					// Basic code we are looking for is the key pressed...
					int basecode = (int)msg.wParam;
					int code = basecode;

					// ...plus the modifier for SHIFT...
					if (((int)shiftKey & 0x00008000) != 0)
						code += 0x00010000;

					// ...plus the modifier for CONTROL
					if (((int)controlKey & 0x00008000) != 0)
						code += 0x00020000;

					if (code == (int)Win32.VirtualKeys.VK_ESCAPE)
					{
						// Is an item being tracked
						if (_trackItem != -1)
						{
							// Is it also showing a submenu
							if (_popupMenu == null)
							{
								// Unselect the current item
								_trackItem = SwitchTrackingItem(_trackItem, -1);

							}
						}

						SimulateReturnFocus();

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_LEFT)
					{
						if (_direction == Direction.Horizontal)
							ProcessMoveLeft(false);

						if (_selected)
							_ignoreMouseMove = true;

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_RIGHT)
					{
						if (_direction == Direction.Horizontal)
							ProcessMoveRight(false);
						else
							ProcessMoveDown();

						if (_selected)
							_ignoreMouseMove = true;

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_RETURN)
					{
						ProcessEnter();

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_DOWN)
					{
						if (_direction == Direction.Horizontal)
							ProcessMoveDown();
						else
							ProcessMoveRight(false);

						// Prevent intended destination getting message
						eat = true;
					}
					else if (code == (int)Win32.VirtualKeys.VK_UP)
					{
						ProcessMoveLeft(false);

						// Prevent intended destination getting message
						eat = true;
					}
					else
					{
						// Construct shortcut from keystate and keychar
						Shortcut sc = (Shortcut)(code);

						// Search for a matching command
						if (!GenerateShortcut(sc, _menuCommands))
						{
							// Last resort is treat as a potential mnemonic
							ProcessMnemonicKey((char)msg.wParam);

							if (_selected)
								_ignoreMouseMove = true;
						}
						else
						{
							SimulateReturnFocus();
						}

						// Always eat keyboard message in simulated focus
						eat = true;
					}
					break;
				case (int)Win32.Msgs.WM_KEYUP:
					eat = true;
					break;
				case (int)Win32.Msgs.WM_SYSKEYUP:
					// Ignore keyboard input if the control is disabled
					if ((int)msg.wParam == (int)Win32.VirtualKeys.VK_MENU)
					{
						if (_trackItem != -1)
						{
							// Is it also showing a submenu
							if (_popupMenu == null)
							{
								// Unselect the current item
								_trackItem = SwitchTrackingItem(_trackItem, -1);

							}
						}

						SimulateReturnFocus();

						// Always eat keyboard message in simulated focus
						eat = true;
					}
					break;
				case (int)Win32.Msgs.WM_SYSKEYDOWN:
					if ((int)msg.wParam != (int)Win32.VirtualKeys.VK_MENU)
					{
						// Construct shortcut from ALT + keychar
						Shortcut sc = (Shortcut)(0x00040000 + (int)msg.wParam);

						// Search for a matching command
						if (!GenerateShortcut(sc, _menuCommands))
						{
							// Last resort is treat as a potential mnemonic
							ProcessMnemonicKey((char)msg.wParam);

							if (_selected)
								_ignoreMouseMove = true;
						}
						else
						{
							SimulateReturnFocus();
						}

						// Always eat keyboard message in simulated focus
						eat = true;
					}
					break;
				default:
					break;
			}

			return eat;
		}

		/// <MetaDataID>{CC21E995-8A50-46AB-A7ED-93E32ACDE8BC}</MetaDataID>
		protected bool GenerateShortcut(Shortcut sc, MenuCommandCollection mcc)
		{
			foreach(MenuCommand mc in mcc)
			{
				// Does the command match?
				if (mc.Enabled && (mc.Shortcut == sc))
				{
					// Generate event for command
					mc.OnClick(EventArgs.Empty);

					return true;
				}
				else
				{
					// Any child items to test?
					if (mc.MenuCommands.Count > 0)
					{
						// Recursive descent of all collections
						if (GenerateShortcut(sc, mc.MenuCommands))
							return true;
					}
				}
			}

			return false;
		}

		/// <MetaDataID>{F042A856-EB89-4BD3-8545-C791CBB59EA1}</MetaDataID>
		protected void OnWM_OPERATEMENU(ref Message m)
		{
			// Is there a valid item being tracted?
			if (_trackItem != -1)
			{
				DrawCommand dc = _drawCommands[_trackItem] as DrawCommand;

				OperateSubMenu(dc, (m.LParam != IntPtr.Zero), (m.WParam != IntPtr.Zero));
			}
		}

		/// <MetaDataID>{66800B62-6C98-459B-9197-71D5CEA1F619}</MetaDataID>
		protected void OnWM_GETDLGCODE(ref Message m)
		{
			// We want to the Form to provide all keyboard input to us
			m.Result = (IntPtr)Win32.DialogCodes.DLGC_WANTALLKEYS;
		}

		/// <MetaDataID>{AB4EC17B-A03F-40BF-BF51-9BC53E1FDCAC}</MetaDataID>
		protected override void WndProc(ref Message m)
		{
		
			try
			{
				// WM_OPERATEMENU is not a constant and so cannot be in a switch

				if (m.Msg == WM_OPERATEMENU)
					OnWM_OPERATEMENU(ref m);
				else
				{
					switch(m.Msg)
					{
						case (int)Win32.Msgs.WM_GETDLGCODE:
							OnWM_GETDLGCODE(ref m);
							return;
					}
				}
			

				base.WndProc(ref m);
			}
			catch(System.Exception Error)
			{
				int werw=0;
			}
	
		}
        public event EventHandler EditMenu;


        /// <MetaDataID>{23589d22-c80a-4a36-aa47-b0413d26504a}</MetaDataID>
        internal void MenuClicked(MenuCommand menuCommand)
        {
            if (EditMenu != null)
                EditMenu(menuCommand, EventArgs.Empty);

        }
    }
}
