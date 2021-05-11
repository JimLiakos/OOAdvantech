using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ConnectableControls.SchedulerControl.Renderers;
using ConnectableControls.SchedulerControl.Events;
using ConnectableControls.SchedulerControl.RowDesign;
using System.ComponentModel;
using ConnectableControls.SchedulerControl.TableModelDesign;
using ConnectableControls.SchedulerControl.Win32;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using OOAdvantech.UserInterface.Runtime;
using ConnectableControls.PropertyEditors;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Collections;
using OOAdvantech.UserInterface;
using System.Xml.Linq;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{7c35e88d-8d13-4f6b-9c0a-3bce736b15ad}</MetaDataID>
    [DesignTimeVisible(true),ToolboxItem(true)]
    public class SchedulerListView : System.Windows.Forms.Control,IObjectMemberViewControl,IPathDataDisplayer                                        
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{4b0901f1-d79b-46fb-8ed3-e4531f2065f7}</MetaDataID>
        public void InitializeControl()
        {
            //UpdateWeekMembers(DateTime.Now);
            //CallLoadActionsOperationCall();
            //SrcollToDayStartTime();
        }

        /// <MetaDataID>{4b9ef965-f569-46ed-9574-04a3691bc803}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{21587711-81b6-45a7-806f-9a693ba7f1ec}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        } 

        #region public object BeforeShowContextMenuOperationCall
        /// <MetaDataID>{49cf127b-abc7-49d2-ab03-ee66b2141683}</MetaDataID>
        XDocument BeforeShowContextMenuOperationCallMetaData;
        /// <MetaDataID>{16d23c1e-9342-4dfe-8edd-983b88c05e80}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _BeforeShowContextMenuOperationCall;
        /// <MetaDataID>{20024bc4-3f13-41f3-859f-f0054b9196b5}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [Browsable(true), Description(@"Gets or sets the Operation ")]
        public object BeforeShowContextMenuOperationCall
        {
            get
            {
                if (BeforeShowContextMenuOperationCallMetaData == null)
                {
                    BeforeShowContextMenuOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", BeforeShowContextMenuOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _BeforeShowContextMenuOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (BeforeShowContextMenuOperationCaller == null || BeforeShowContextMenuOperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(BeforeShowContextMenuOperationCallMetaData.ToString() as string, "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(BeforeShowContextMenuOperationCallMetaData.ToString() as string, BeforeShowContextMenuOperationCaller.Operation.Name);
                metaDataVaue.MetaDataAsObject = _BeforeShowContextMenuOperationCall;
                return metaDataVaue;
            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("BeforeShowContextMenuOperationCall", false).SetValue(this, BeforeShowContextMenuOperationCall);
                    return;
                }
                _BeforeShowContextMenuOperationCaller = null;
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                if (metaDataVaue == null)
                    return;
                if (BeforeShowContextMenuOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    _BeforeShowContextMenuOperationCall = null;
                    BeforeShowContextMenuOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            BeforeShowContextMenuOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (System.Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", BeforeShowContextMenuOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            BeforeShowContextMenuOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", BeforeShowContextMenuOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }
                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        BeforeShowContextMenuOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", BeforeShowContextMenuOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        _BeforeShowContextMenuOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (_BeforeShowContextMenuOperationCall == null)
                        _BeforeShowContextMenuOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        _BeforeShowContextMenuOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }
        #endregion

        #region public OOAdvantech.UserInterface.Runtime.OperationCaller BeforeShowContextMenuOperationCaller
        /// <MetaDataID>{388337d9-c55c-4c5e-ba0d-5bedd137a638}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _BeforeShowContextMenuOperationCaller;
        /// <MetaDataID>{70d479e7-f934-4c04-a02e-69b20f42e67c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller BeforeShowContextMenuOperationCaller
        {
            get
            {
                if (BeforeShowContextMenuOperationCallMetaData == null || _BeforeShowContextMenuOperationCall == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_BeforeShowContextMenuOperationCaller != null)
                    return _BeforeShowContextMenuOperationCaller;
                _BeforeShowContextMenuOperationCaller = new OOAdvantech.UserInterface.Runtime.OperationCaller(_BeforeShowContextMenuOperationCall, this);
                return _BeforeShowContextMenuOperationCaller;
            }
        }
        #endregion
              

        /// <summary>
        /// Scrolls the control to the time the day starts
        /// </summary>
        /// <MetaDataID>{cab56efa-0a93-4b51-af15-3795929cd093}</MetaDataID>
        public void ScrollToDayStartTime(int hour)
        {
            try
            {
                //bool iscollection = false;
                //object display_value = this.UserInterfaceObjectConnection.GetDisplayedValue(WorkTimeStartsMember as string, this, out iscollection);
                //DateTime dtime = (DateTime)display_value;
                //if (VScroll)
                //{
                hour *= this.TableModel.RowsOnTime;
                int newVal = this._VScrollBar.Value - ((-120 / 120) * hour);

                if (newVal < 0)
                {
                    newVal = 0;
                }
                else if (newVal > this._VScrollBar.Maximum - this._VScrollBar.LargeChange + 1)
                {
                    newVal = this._VScrollBar.Maximum - this._VScrollBar.LargeChange + 1;
                }

                this.VerticalScroll(newVal);
                this._VScrollBar.Value = newVal;
                //}

            }
            catch (System.Exception)
            {
                
                
            }
        }

        #region Event Handlers

        #region Cells

        /// <summary>
        /// Occurs when the value of a Cells property changes
        /// </summary>
        public event CellEventHandler CellPropertyChanged;

        #region Focus

        /// <summary>
        /// Occurs when a Cell gains focus
        /// </summary>
        public event CellFocusEventHandler CellGotFocus;

        /// <summary>
        /// Occurs when a Cell loses focus
        /// </summary>
        public event CellFocusEventHandler CellLostFocus;

        #endregion

        #region Keys

        /// <summary>
        /// Occurs when a key is pressed when a Cell has focus
        /// </summary>
        public event CellKeyEventHandler CellKeyDown;

        /// <summary>
        /// Occurs when a key is released when a Cell has focus
        /// </summary>
        public event CellKeyEventHandler CellKeyUp;

        #endregion

        #region Mouse

        /// <summary>
        /// Occurs when the mouse pointer enters a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseLeave;

        /// <summary>
        /// Occurs when a mouse pointer is over a Cell and a mouse button is pressed
        /// </summary>
        public event CellMouseEventHandler CellMouseDown;

        /// <summary>
        /// Occurs when a mouse pointer is over a Cell and a mouse button is released
        /// </summary>
        public event CellMouseEventHandler CellMouseUp;

        /// <summary>
        /// Occurs when a mouse pointer is moved over a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer hovers over a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseHover;

        /// <summary>
        /// Occurs when a Cell is clicked
        /// </summary>
        public event CellMouseEventHandler CellClick;

        /// <summary>
        /// Occurs when a Cell is double-clicked
        /// </summary>
        public event CellMouseEventHandler CellDoubleClick;

        #endregion

        //#region Buttons

        ///// <summary>
        ///// Occurs when a Cell's button is clicked
        ///// </summary>
        //public event CellButtonEventHandler CellButtonClicked;

        //#endregion

        //#region CheckBox

        ///// <summary>
        ///// Occurs when a Cell's Checked value changes
        ///// </summary>
        //public event CellCheckBoxEventHandler CellCheckChanged;

        //#endregion

        #endregion

        #region Column

        /// <summary>
        /// Occurs when a Column's property changes
        /// </summary>
        public event ColumnEventHandler ColumnPropertyChanged;

        #endregion

        #region Column Headers

        /// <summary>
        /// Occurs when the mouse pointer enters a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseLeave;

        /// <summary>
        /// Occurs when a mouse pointer is over a Column Header and a mouse button is pressed
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseDown;

        /// <summary>
        /// Occurs when a mouse pointer is over a Column Header and a mouse button is released
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseUp;

        /// <summary>
        /// Occurs when a mouse pointer is moved over a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer hovers over a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseHover;

        /// <summary>
        /// Occurs when a Column Header is clicked
        /// </summary>
        public event HeaderMouseEventHandler HeaderClick;

        /// <summary>
        /// Occurs when a Column Header is double-clicked
        /// </summary>
        public event HeaderMouseEventHandler HeaderDoubleClick;

        /// <summary>
        /// Occurs when the height of the Column Headers changes
        /// </summary>
        public event EventHandler HeaderHeightChanged;

        #endregion

        #region ColumnModel

        /// <summary>
        /// Occurs when the value of the Table's ColumnModel property changes 
        /// </summary>
        public event EventHandler ColumnModelChanged;

        /// <summary>
        /// Occurs when a Column is added to the ColumnModel
        /// </summary>
        public event ColumnModelEventHandler ColumnAdded;

        /// <summary>
        /// Occurs when a Column is removed from the ColumnModel
        /// </summary>
        public event ColumnModelEventHandler ColumnRemoved;

        #endregion

        //#region Editing

        ///// <summary>
        ///// Occurs when the Table begins editing a Cell
        ///// </summary>
        //public event CellEditEventHandler BeginEditing;

        ///// <summary>
        ///// Occurs when the Table stops editing a Cell
        ///// </summary>
        //public event CellEditEventHandler EditingStopped;

        ///// <summary>
        ///// Occurs when the editing of a Cell is cancelled
        ///// </summary>
        //public event CellEditEventHandler EditingCancelled;

        //#endregion

        #region TotalRows

        /// <summary>
        /// Occurs when a Cell is added to a Row
        /// </summary>
        public event RowEventHandler CellAdded;

        /// <summary>
        /// Occurs when a Cell is removed from a Row
        /// </summary>
        public event RowEventHandler CellRemoved;

        /// <summary>
        /// Occurs when the value of a TotalRows property changes
        /// </summary>
        public event RowEventHandler RowPropertyChanged;

        #endregion

        #region Sorting

        /// <summary>
        /// Occurs when a Column is about to be sorted
        /// </summary>
        public event ColumnEventHandler BeginSort;

        /// <summary>
        /// Occurs after a Column has finished sorting
        /// </summary>
        public event ColumnEventHandler EndSort;

        #endregion

        #region Painting

        /// <summary>
        /// Occurs before a Cell is painted
        /// </summary>
        public event PaintCellEventHandler BeforePaintCell;

        /// <summary>
        /// Occurs after a Cell is painted
        /// </summary>
        public event PaintCellEventHandler AfterPaintCell;

        /// <summary>
        /// Occurs before a Column header is painted
        /// </summary>
        public event PaintHeaderEventHandler BeforePaintHeader;

        /// <summary>
        /// Occurs after a Column header is painted
        /// </summary>
        public event PaintHeaderEventHandler AfterPaintHeader;

        #endregion

        #region TableModel

        /// <summary>
        /// Occurs when the value of the Table's TableModel property changes 
        /// </summary>
        public event EventHandler TableModelChanged;

        /// <summary>
        /// Occurs when a Row is added into the TableModel
        /// </summary>
        public event TableModelEventHandler RowAdded;

        /// <summary>
        /// Occurs when a Row is removed from the TableModel
        /// </summary>
        public event TableModelEventHandler RowRemoved;

        /// <summary>
        /// Occurs when the value of the TableModel Selection property changes
        /// </summary>
        public event SelectionEventHandler SelectionChanged;

        /// <summary>
        /// Occurs when the value of the RowHeight property changes
        /// </summary>
        public event EventHandler RowHeightChanged;

        #endregion

        #endregion

        #region Class Data

        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{28e20e29-eee5-492a-873a-7f7cc213a459}</MetaDataID>
        private System.ComponentModel.Container components = null;

        #region Border

        /// <summary>
        /// The style of the Table's border
        /// </summary>
        /// <MetaDataID>{ff2daaed-42cb-4870-b563-63ae3405f832}</MetaDataID>
        private BorderStyle borderStyle;

        #endregion

        #region Cells

        /// <summary>
        /// The last known cell position that the mouse was over
        /// </summary>
        /// <MetaDataID>{822e5c09-433f-4406-b83c-122a6ad4d08d}</MetaDataID>
        private CellPos lastMouseCell;

        /// <summary>
        /// The last known cell position that the mouse's left 
        /// button was pressed in
        /// </summary>
        /// <MetaDataID>{82467f55-6784-4257-8ac8-0d35a142bac6}</MetaDataID>
        private CellPos lastMouseDownCell;

        /// <MetaDataID>{1a9f7a18-1a75-4483-9e7a-0b80df53a409}</MetaDataID>
        private CellPos lastMouseClickCell;

        /// <summary>
        /// The position of the Cell that currently has focus
        /// </summary>
        /// <MetaDataID>{227c26bb-4451-42b6-9eb4-9665edeb9590}</MetaDataID>
        private CellPos focusedCell;

        /// <summary>
        /// The Cell that is currently being edited
        /// </summary>
        /// <MetaDataID>{436355ea-c722-4a63-b346-34e2650f8016}</MetaDataID>
        private CellPos editingCell;

        /// <summary>
        /// The ICellEditor that is currently being used to edit a Cell
        /// </summary>
        /// <MetaDataID>{4f43f62b-08d7-4025-9893-3772e1648226}</MetaDataID>
        private ICellEditor curentCellEditor;

        ///// <summary>
        ///// The action that must be performed on a Cell to start editing
        ///// </summary>
        //private EditStartAction editStartAction;

        ///// <summary>
        ///// The key that must be pressed for editing to start when 
        ///// editStartAction is set to EditStartAction.CustomKey
        ///// </summary>
        //private Keys customEditKey;

        /// <summary>
        /// The amount of time (in milliseconds) that that the 
        /// mouse pointer must hover over a Cell or Column Header before 
        /// a MouseHover event is raised
        /// </summary>
        /// <MetaDataID>{02ad31ee-207b-457a-8b0f-e00d571c32c3}</MetaDataID>
        private int hoverTime;

        /// <summary>
        /// A TRACKMOUSEEVENT used to set the hoverTime
        /// </summary>
        /// <MetaDataID>{0c8df4a8-b660-4c22-a14f-a7da524569e8}</MetaDataID>
        private TRACKMOUSEEVENT trackMouseEvent;

        #endregion

        #region Columns

        /// <summary>
        /// The ColumnModel of the Table
        /// </summary>
        /// <MetaDataID>{071a7137-dace-4e1d-bad8-c2793337a330}</MetaDataID>
        private DayTimeColumnModel _ColumnModel;

        ///// <summary>
        ///// Whether the Table supports column resizing
        ///// </summary>
        //private bool columnResizing;

        ///// <summary>
        ///// The index of the column currently being resized
        ///// </summary>
        //private int resizingColumnIndex;

        ///// <summary>
        ///// The x coordinate of the currently resizing column
        ///// </summary>
        //private int resizingColumnAnchor;

        ///// <summary>
        ///// The horizontal distance between the resize starting
        ///// point and the right edge of the resizing column
        ///// </summary>
        //private int resizingColumnOffset;

        ///// <summary>
        ///// The width that the resizing column will be set to 
        ///// once column resizing is finished
        ///// </summary>
        //private int resizingColumnWidth;

        ///// <summary>
        ///// The index of the current pressed column
        ///// </summary>
        //private int pressedColumn;

        ///// <summary>
        ///// The index of the current "hot" column
        ///// </summary>
        //private int hotColumn;

        ///// <summary>
        ///// The index of the last sorted column
        ///// </summary>
        //private int lastSortedColumn;

        ///// <summary>
        ///// The Color of a sorted Column's background
        ///// </summary>
        //private Color sortedColumnBackColor;

        #endregion

        #region Grid

        /// <summary>
        /// Indicates whether grid lines appear between the TotalRows and columns 
        /// containing the TotalRows and cells in the Table
        /// </summary>
        /// <MetaDataID>{77ea76cb-a92d-40c4-ba95-03243a6f758f}</MetaDataID>
        private GridLines _GridLines;

        /// <summary>
        /// The line style of the grid lines
        /// </summary>
        /// <MetaDataID>{0950c2c7-48e4-4099-9d89-9c6f096ee045}</MetaDataID>
        private GridLineStyle gridLineStyle;

        #endregion

        #region Header

        /// <summary>
        /// The styles of the column headers 
        /// </summary>
        /// <MetaDataID>{666c25ac-ceef-4a5e-9f7c-349bfc1a22c4}</MetaDataID>
        private ColumnHeaderStyle headerStyle;

        /// <summary>
        /// The font used to draw the text in the column header
        /// </summary>
        /// <MetaDataID>{c71da936-f964-4fe5-8f16-1561adc4948a}</MetaDataID>
        private Font headerFont;

        ///// <summary>
        ///// The ContextMenu for the column headers
        ///// </summary>
        //private HeaderContextMenu headerContextMenu;

        #endregion

        #region Items
        

        #endregion

        #region Scrollbars

        /// <summary>
        /// Indicates whether the Table will allow the user to scroll to any 
        /// columns or TotalRows placed outside of its visible boundaries
        /// </summary>
        /// <MetaDataID>{0d0ed7fc-dc58-40d1-824e-09c5dc7f739e}</MetaDataID>
        private bool scrollable;

        ///// <summary>
        ///// The Table's horizontal ScrollBar
        ///// </summary>
        //private HScrollBar hScrollBar;

        /// <summary>
        /// The Table's vertical ScrollBar
        /// </summary>
        /// <MetaDataID>{5725c0d2-94c9-4de9-9e92-463e0a5b66c8}</MetaDataID>
        private VScrollBar _VScrollBar;
        /// <MetaDataID>{9751edc7-7c2f-435c-9e78-55ad5b2d93ea}</MetaDataID>
        [Browsable(false)]
        public VScrollBar VScrollBar
        {
            get
            {
                return _VScrollBar;
            }
        }

        #endregion

        #region Selection

        /// <summary>
        /// Specifies whether TotalRows and cells can be selected
        /// </summary>
        /// <MetaDataID>{49a568f6-fbc9-4e23-8184-7754867f01fc}</MetaDataID>
        private bool allowSelection;

        ///// <summary>
        ///// Specifies whether multiple TotalRows and cells can be selected
        ///// </summary>
        //private bool multiSelect;        

        ///// <summary>
        ///// Specifies whether the selected TotalRows and cells in the Table remain 
        ///// highlighted when the Table loses focus
        ///// </summary>
        //private bool hideSelection;

        #endregion

        #region Table

        ///// <summary>
        ///// The state of the table
        ///// </summary>
        ///// <MetaDataID>{206388bf-ac5d-4a44-9d82-e794af2c61da}</MetaDataID>
        //private TableState tableState;

        /// <summary>
        /// Is the Table currently initialising
        /// </summary>
        /// <MetaDataID>{1a13a68e-471b-4bbb-a3f3-ad8df3925494}</MetaDataID>
        private bool init;

        /// <summary>
        /// The number of times BeginUpdate has been called
        /// </summary>
        /// <MetaDataID>{a3d5e481-7e49-4ef6-9eed-f594d6cb43b8}</MetaDataID>
        private int beginUpdateCount;

        /// <summary>
        /// The ToolTip used by the Table to display cell and column tooltips
        /// </summary>
        /// <MetaDataID>{7706ede9-3a47-4311-affc-f1270c3c02f3}</MetaDataID>
        private ToolTip toolTip;

        /// <summary>
        /// The alternating row background color
        /// </summary>
        /// <MetaDataID>{f1a17b14-cca7-4c25-bf7c-f77771779dc0}</MetaDataID>
        private Color alternatingRowColor;

        ///// <summary>
        ///// The text displayed in the Table when it has no data to display
        ///// </summary>
        //private string noItemsText;

        /// <summary>
        /// Specifies whether the Table is being used as a preview Table 
        /// in a ColumnColection editor
        /// </summary>
        /// <MetaDataID>{d2434108-1ccb-491a-a12b-824c5b833e3d}</MetaDataID>
        private bool preview;

        /*/// <summary>
        /// Specifies whether pressing the Tab key while editing moves the 
        /// editor to the next available cell
        /// </summary>
        private bool tabMovesEditor;*/

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Table class with default settings
        /// </summary>
        /// <MetaDataID>{13F8BA10-9A09-4A50-B37C-65A9E13F6291}</MetaDataID>
        public SchedulerListView()
        {
            // starting setup
            this.init = true;
            //_ListConnection = new ListConnection(this);

            // This call is required by the Windows.Forms Form Designer.
            components = new System.ComponentModel.Container();

            //
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.TabStop = true;

            this.Size = new Size(150, 150);

            this.BackColor = Color.White;

            ////
            //this._ColumnModel = null;
            //this._TableModel = null;

            // header
            this.headerStyle = ColumnHeaderStyle.Clickable;
            this.headerFont = this.Font;
            this._HeaderRenderer = new XPHeaderRenderer();
            this._LongActionsRectRenderer = new LongActionsRectRenderer();
            //this.headerRenderer = new GradientHeaderRenderer();
            //this.headerRenderer = new FlatHeaderRenderer();
            this._HeaderRenderer.Font = this.headerFont;
            //TODO:να δω αν θα βάλω Menu
            //this.headerContextMenu = new HeaderContextMenu();

            ////this.columnResizing = true;
            //this.resizingColumnIndex = -1;
            //this.resizingColumnWidth = -1;
            //this.hotColumn = -1;
            //this.pressedColumn = -1;
            //this.lastSortedColumn = -1;
            //this.sortedColumnBackColor = Color.WhiteSmoke;

            // borders
            this.borderStyle = BorderStyle.Fixed3D;

            // scrolling
            this.scrollable = true;
            
            this._VScrollBar = new VScrollBar();
            this._VScrollBar.Visible = false;
            this._VScrollBar.Location = new Point(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth, this.BorderWidth);
            this._VScrollBar.Height = this.Height - (this.BorderWidth * 2) - SystemInformation.HorizontalScrollBarHeight;
            this._VScrollBar.Scroll += new ScrollEventHandler(this.OnVerticalScroll);
            this.Controls.Add(this._VScrollBar);

            this._LongActionsHScrollBar = new HScrollBar();
            this._LongActionsHScrollBar.Visible = false;
            //this._LongActionsVScrollBar.Location = new Point(_TimeColumnWidth - SystemInformation.VerticalScrollBarWidth, this.BorderWidth + HeaderHeight);
            this._LongActionsHScrollBar.Location = new Point(this.BorderWidth, this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight);
            //this._LongActionsHScrollBar.Height = this.LongActionsRectHeight;
            this._LongActionsHScrollBar.Scroll += new ScrollEventHandler(this.OnLongActionVerticalScroll);
            this.Controls.Add(this._LongActionsHScrollBar);

            _DetailDayHScrollBar.Visible = false;
            this._DetailDayHScrollBar.Location = new Point(this.BorderWidth, this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight);
            this._DetailDayHScrollBar.Scroll += new ScrollEventHandler(this.OnDetailDayHScroll);
            this.Controls.Add(this._DetailDayHScrollBar);

            //
            this._GridLines = GridLines.None; ;
            this._ColumnGridColor = Color.Brown;
            //this._TimeRowGridColor = Color.Brown;
            this._RowGridColor = SystemColors.Control;
            this.gridLineStyle = GridLineStyle.Solid;

            this.allowSelection = true;
            //this.multiSelect = false;
            //this._FullRowSelect = false;
            //this.hideSelection = false;
            this._SelectedCellBackColor = SystemColors.Highlight;
            this._SelectedCellForeColor = SystemColors.HighlightText;
            this._UnfocusedSelectedCellBackColor = SystemColors.Control;
            this._UnfocusedSelectedCellForeColor = SystemColors.ControlText;
            this._SelectionStyle = SelectionStyle.Grid;
            this.alternatingRowColor = Color.Transparent;            

            this.lastMouseCell = new CellPos(-1, -1);
            this.lastMouseDownCell = new CellPos(-1, -1);
            this.focusedCell = new CellPos(-1, -1);
            this.hoverTime = 1000;
            this.trackMouseEvent = null;
            this.ResetMouseEventArgs();

            this.toolTip = new ToolTip(this.components);
            this.toolTip.Active = false;
            this.toolTip.InitialDelay = 1000;

            //this.noItemsText = "There are no items in this view";

            this.editingCell = new CellPos(-1, -1);
            //this.lastEditingCell = this.editingCell;
            this.curentCellEditor = null;
            //this.editStartAction = EditStartAction.DoubleClick;
            //this.customEditKey = Keys.F5;
            //this.tabMovesEditor = true;

            // finished setting up
            this.beginUpdateCount = 0;
            //this.init = false;
            this.preview = false;
            _FocusedBackColor = BackColor;
            //MouseDownTimer.Tick += new EventHandler(MouseDownTimerTick);
            base.BackColor = Color.White;
            this._GridLines = GridLines.Both;
            _MinActionViewWidth = 10;

            //this.TableModel.RowHeight = 24;

            this.ColumnModel = new DayTimeColumnModel();
            this.ColumnModel.ZoomInDaysImage = new Bitmap(typeof(Column).Assembly.GetManifestResourceStream("ConnectableControls.SchedulerControl.zoom_in.gif"));//this.DetailDayPictureBox.InitialImage;
            this.ColumnModel.ZoomOutDaysImage = new Bitmap(typeof(Column).Assembly.GetManifestResourceStream("ConnectableControls.SchedulerControl.zoom_out.gif"));//this.DetailDayPictureBox.InitialImage;
            this.ColumnModel.CreateTimeColumn("Hour", _TimeColumnWidth);
            this.ColumnModel.CreateAndShowWeek(DateTime.Now, true);            

            for (int i = 0; i != 24; i++)
            {
                this.TableModel.CreateRow(i, Color.Black, Color.WhiteSmoke, this.Font);
            }
            this.TableModel.UpdateTotalRows();

            this.LongActionsRectHeight = this.HeaderHeight - 1;

            _WeekStartsProperty = new DependencyProperty(this, "WeekStarts");
            _WorkTimeStartsProperty = new DependencyProperty(this, "WorkTimeStarts");
            _DependencyProperties.Add(_WeekStartsProperty);
            _DependencyProperties.Add(_WorkTimeStartsProperty);

            //_LongActionsRectangle = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);
        }

        /// <MetaDataID>{12038f5b-6943-4992-9180-8f976e5e56a5}</MetaDataID>
        protected override void InitLayout()
        {
            base.InitLayout();
            //this.UpdateScrollBars();Does not updates the bars from here            
            this.ColumnModel.UpdateColumnsWidth();
            this.init = false;
            
        }     
   
        

        #endregion

        #region public bool LoadActionsOnViewShift
        /// <MetaDataID>{b341cc31-f666-42cf-b435-9c37520aa5b5}</MetaDataID>
        private bool _LoadActionsOnViewShift;
        /// <MetaDataID>{5ec6ac6c-07c2-4138-b81e-6e3ea75fc591}</MetaDataID>
        [Category("Object Model Connection")]
        [NotifyParentProperty(true), DefaultValue(false)]
        [Browsable(true), Description("Gets or sets whether the Scheduler will load the actions when moves to new week or day")]
        public bool LoadActionsOnViewShift
        {
            get
            {
                return _LoadActionsOnViewShift;
            }
            set
            {
                _LoadActionsOnViewShift = value;
            }
        } 
        #endregion

        #region public OOAdvantech.UserInterface.Runtime.OperationCaller LoadActionsOperationCaller
        /// <MetaDataID>{388337d9-c55c-4c5e-ba0d-5bedd137a638}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _LoadActionsOperationCaller;
        /// <MetaDataID>{70d479e7-f934-4c04-a02e-69b20f42e67c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller LoadActionsOperationCaller
        {
            get
            {
                if (LoadActionsOperationCallMetaData == null || _LoadActionsOperationCall == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_LoadActionsOperationCaller != null)
                    return _LoadActionsOperationCaller;
                _LoadActionsOperationCaller = new OOAdvantech.UserInterface.Runtime.OperationCaller(_LoadActionsOperationCall, this);
                return _LoadActionsOperationCaller;
            }
        } 
        #endregion

        #region public object LoadActionsOperationCall
        /// <MetaDataID>{49cf127b-abc7-49d2-ab03-ee66b2141683}</MetaDataID>
        XDocument LoadActionsOperationCallMetaData;
        /// <MetaDataID>{16d23c1e-9342-4dfe-8edd-983b88c05e80}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _LoadActionsOperationCall;
        /// <MetaDataID>{20024bc4-3f13-41f3-859f-f0054b9196b5}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [Browsable(true), Description(@"Gets or sets the Operation that will be called to load the actions of the Current week
          .The action will be called when the LoadActionsOnViewShift is true and the results of the action must be put 
          in the Path collection")]
        public object LoadActionsOperationCall
        {
            get
            {
                if (LoadActionsOperationCallMetaData == null)
                {
                    LoadActionsOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", LoadActionsOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _LoadActionsOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }


                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (LoadActionsOperationCaller == null || LoadActionsOperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(LoadActionsOperationCallMetaData.ToString() as string, "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(LoadActionsOperationCallMetaData.ToString() as string, LoadActionsOperationCaller.Operation.Name);

                metaDataVaue.MetaDataAsObject = _LoadActionsOperationCall;
                return metaDataVaue;



            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("LoadActionsOperationCall", false).SetValue(this, LoadActionsOperationCall);
                    return;
                }
                _LoadActionsOperationCaller = null;
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                if (metaDataVaue == null)
                    return;


                if (LoadActionsOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    _LoadActionsOperationCall = null;
                    LoadActionsOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            LoadActionsOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (System.Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", LoadActionsOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            LoadActionsOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", LoadActionsOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }



                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        LoadActionsOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", LoadActionsOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        _LoadActionsOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (_LoadActionsOperationCall == null)
                        _LoadActionsOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        _LoadActionsOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        } 
        #endregion

        /// <summary>
        /// Calls the Operation which loads the Actions in the Path of the control
        /// </summary>
        /// <MetaDataID>{83aa5815-80fc-4396-a54c-d51a1e915c36}</MetaDataID>
        public void CallLoadActionsOperationCall()
        {
            if (this.LoadActionsOnViewShift && this._LoadActionsOperationCall != null)
            {
                //OperationCaller opcaller = new OperationCaller(_LoadActionsOperationCall, this as OOAdvantech.UserInterface.Runtime.IOperationCallerSource);
                //opcaller.Invoke();
                if(LoadActionsOperationCaller!=null)
                    _LoadActionsOperationCaller.Invoke();
            }
        }

        /// <MetaDataID>{f6d9be3d-0c0a-4bf3-aa65-272c87dbeee9}</MetaDataID>
        internal int LongActionsCount;
        /// <MetaDataID>{6248687e-dc1c-481f-9523-55ea1c29618f}</MetaDataID>
        internal int _TimeColumnWidth = 60;

        #region private bool ShowLongActionsScroll
        /// <MetaDataID>{a3ae32b3-e2e2-4760-aaf3-4cd71f694610}</MetaDataID>
        private bool _ShowLongActionsScroll;
        /// <MetaDataID>{25e55364-b1f2-41de-8468-b4fe090a12d0}</MetaDataID>
        private bool ShowLongActionsScroll
        {
            get
            {
                return _ShowLongActionsScroll;
            }
            set
            {
                if (_ShowLongActionsScroll != value)
                {
                    _ShowLongActionsScroll = value;
                    this.UpdateLongActionsScroll();
                }
            }
        } 
        #endregion

        #region private Rectangle LongActionsRectangle
        /// <MetaDataID>{25b8c018-fc7b-4c02-a9eb-498b77da66c5}</MetaDataID>
        private Rectangle _LongActionsRectangle;
        /// <MetaDataID>{8867df4a-8949-4f3e-a40e-5d85d31a0056}</MetaDataID>
        private Rectangle LongActionsRectangle
        {
            get
            {
                if (_LongActionsRectangle == null)
                {
                    _LongActionsRectangle = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);
                }
                else if (_LongActionsRectangle.Height != this.LongActionsRectHeight)
                    _LongActionsRectangle.Height = this.LongActionsRectHeight;


                return _LongActionsRectangle;
            }
        } 
        #endregion

        /// <summary>
        /// Gets the index of the first visible row in the Table
        /// </summary>
        /// <MetaDataID>{44218d03-d819-4dfe-9135-9e06fcac5dd9}</MetaDataID>
        [Browsable(false)]
        internal int TopLongActionIndex
        {
            get
            {
                return this._LongActionsHScrollBar.Value;
            }
        }

        /// <summary>
        /// Gets the number of TotalRows that are visible in the Table
        /// </summary>
        /// <MetaDataID>{3ad94afa-401e-4a25-9ebe-8c25cbb9c342}</MetaDataID>
        [Browsable(false)]
        public int VisibleLongActionsCount
        {
            get
            {
                int lapadding = this.LongActionHeight + this.ActionsPadding;
                int count = this.LongActionsRectHeight / lapadding;                

                return count;
            }
        }

        /// <summary>
        /// Gets the index of the first visible action starting from the left
        /// </summary>
        /// <MetaDataID>{44218d03-d819-4dfe-9135-9e06fcac5dd9}</MetaDataID>
        [Browsable(false)]
        internal int LeftDayActionIndex
        {
            get
            {
                return this._DetailDayHScrollBar.Value;
            }
        }

        /// <MetaDataID>{eab91895-2e2e-4295-a778-7c8825b61689}</MetaDataID>
        private bool _ShowDetailDayScroll;
        /// <MetaDataID>{5402fa13-0d88-47da-9f4a-bc896da1c100}</MetaDataID>
        internal bool ShowDetailDayScroll
        {
            get
            {
                return _ShowDetailDayScroll;
            }
            set
            {
                if (_ShowDetailDayScroll != value)
                {
                    _ShowDetailDayScroll = value;
                    this._DetailDayHScrollBar.Visible = value;//OnLayout event call

                    //this.UpdateDetailDayScroll();
                }
            }
        }

        /// <MetaDataID>{b497bea6-6f29-4068-b7cb-bd3983418544}</MetaDataID>
        private HScrollBar _DetailDayHScrollBar = new HScrollBar();
        /// <MetaDataID>{ca97c035-990b-4d02-8804-3a474ddfbb46}</MetaDataID>
        [Browsable(false)]
        public HScrollBar DetailDayHScrollBar
        {
            get
            {
                return _DetailDayHScrollBar;
            }
        }

        /// <summary>
        /// The Table's H ScrollBar
        /// </summary>
        /// <MetaDataID>{42f4a00d-1ec5-4b9a-bff6-c7a13e60fc90}</MetaDataID>
        private HScrollBar _LongActionsHScrollBar;
        /// <MetaDataID>{ca97c035-990b-4d02-8804-3a474ddfbb46}</MetaDataID>
        [Browsable(false)]
        public HScrollBar LongActionsHScrollBar
        {
            get
            {
                return _LongActionsHScrollBar;
            }
        }

        /// <MetaDataID>{19f3a0b4-46ab-4533-8081-dcf9fff9a70b}</MetaDataID>
        internal int MaxActionViewWidth = 45;

        /// <MetaDataID>{9dee640b-e75e-40be-bc44-e8c37d4e8b7a}</MetaDataID>
        private int _MinActionViewWidth;
        /// <MetaDataID>{8a483da4-5499-4618-8b01-0d340ace0a1f}</MetaDataID>
        public int MinActionViewWidth
        {
            get
            {
                return _MinActionViewWidth;
            }
        }

        //public event ActionMouseEventHandler ActionMouseDown;
        //public event ActionMouseEventHandler ActionMouseUp;
        /// <MetaDataID>{607b3e86-df71-45e3-a232-6966dc024ad6}</MetaDataID>
        internal List<SchedulerActionView> FocusedActionViews;
        //public DateTime StartDateTime = new DateTime(2010, 2, 3);
        /// <MetaDataID>{e2f86fd9-17a5-42c3-b2ba-0e1aa26adf60}</MetaDataID>
        protected ActionRenderer ActionRenderer = new ActionRenderer();
        //protected Dictionary<Action, PaintActionEventArgs> ActionPaintActionEvent = new Dictionary<Action, PaintActionEventArgs>();
        /// <MetaDataID>{07661fea-d396-4b01-a4da-df4f4cbb4e38}</MetaDataID>
        internal int LongActionHeight = 19;

        #region private ISchedulerAction SelectedAction
        /// <MetaDataID>{5690c638-3acb-4f53-9753-86fae62d7911}</MetaDataID>
        private ISchedulerAction _SelectedAction;
        /// <MetaDataID>{3e77d677-f6eb-4485-80e7-80434e0c2eeb}</MetaDataID>
        private ISchedulerAction SelectedAction
        {
            get
            {
                return _SelectedAction;
            }
            set
            {
                if (_SelectedAction != value)
                {
                    _SelectedAction = value;
                }
                UserInterfaceObjectConnection.SetValue(value, _SelectionMember);
            }
        } 
        #endregion

        #region Path members
        /// <MetaDataID>{593c930d-e406-4d25-ba49-37665db43f5c}</MetaDataID>
        private DateTime _SelectedDate;
        /// <MetaDataID>{a7773493-a4a2-4001-964d-0df069bcc59e}</MetaDataID>
        private DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; }
        }

        /// <MetaDataID>{b9a8869e-6151-4e4c-8285-26c60029bafe}</MetaDataID>
        private int _SelectedTimeStart;
        /// <MetaDataID>{b8c72618-85d4-44ed-abfe-2b944094f7f5}</MetaDataID>
        private int SelectedTimeStart
        {
            get { return _SelectedTimeStart; }
            set { _SelectedTimeStart = value; }
        }

        /// <MetaDataID>{34f9c74c-d91e-4f36-8d57-93271dbf9812}</MetaDataID>
        private int _SelectedTimeEnd;
        /// <MetaDataID>{405bcbf7-0ca2-4bea-be94-c240ea3c2b95}</MetaDataID>
        private int SelectedTimeEnd
        {
            get { return _SelectedTimeEnd; }
            set { _SelectedTimeEnd = value; }
        }

        /// <MetaDataID>{34f9c74c-d91e-4f36-8d57-93271dbf9812}</MetaDataID>
        private DateTime _DateStarts;
        /// <MetaDataID>{405bcbf7-0ca2-4bea-be94-c240ea3c2b95}</MetaDataID>
        private DateTime DateStarts
        {
            get { return _DateStarts; }
            set { _DateStarts = value; }
        }

        /// <MetaDataID>{34f9c74c-d91e-4f36-8d57-93271dbf9812}</MetaDataID>
        private DateTime _DateEnds;
        /// <MetaDataID>{405bcbf7-0ca2-4bea-be94-c240ea3c2b95}</MetaDataID>
        private DateTime DateEnds
        {
            get { return _DateEnds; }
            set { _DateEnds = value; }
        } 
        #endregion

        #region public DependencyProperty WorkTimeStartsMember

        /// <MetaDataID>{2b2d0822-d5bd-4380-ab80-f84f74a946f3}</MetaDataID>
        private DependencyProperty _WorkTimeStartsProperty;

        /// <MetaDataID>{ad20addd-4d57-48b0-a7c1-8173359802f7}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true), Description(@"Sets the member that its value tells the control the time that the working day starts to scroll to that time")]
        public DependencyProperty WorkTimeStartsProperty
        {
            get
            {
                return _WorkTimeStartsProperty;
            }
            set
            {
                if (value != null)
                {
                    _WorkTimeStartsProperty = value;
                    _WorkTimeStartsProperty.ConnectableControl = this;
                }                
            }
        } 
        #endregion

        #region public DateTime WorkTimeStarts

        /// <MetaDataID>{2b2d0822-d5bd-4380-ab80-f84f74a946f3}</MetaDataID>
        private DateTime _WorkTimeStarts;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]        
        public DateTime WorkTimeStarts
        {
            get
            {
                return _WorkTimeStarts;
            }
            set
            {
                if (value != null)
                {
                    _WorkTimeStarts = value;
                }
            }
        }
        #endregion

        #region public object SelectionMember
        /// <MetaDataID>{6aa2e888-6535-44f9-bb78-790c1f960ead}</MetaDataID>
        string _SelectionMember;
        /// <MetaDataID>{4fc7b35b-3211-4633-8b85-ae8ff8fcb32f}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true), Description("Sets the member that is initialized with the selected action")]
        public object SelectionMember
        {
            get
            {
                return _SelectionMember;
            }
            set
            {
                if (value is string)
                    _SelectionMember = value as string;
                else if (value is MetaData)
                    _SelectionMember = (value as MetaData).Path;
            }
        } 
        #endregion


        #region public object WeekStartsProperty
        /// <MetaDataID>{5ce7d225-a739-4354-9e34-6c4fd5cd8013}</MetaDataID>
        DependencyProperty _WeekStartsProperty;
        /// <MetaDataID>{25d4a1e7-1cf5-412a-b431-8c7cc0d2227f}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true), Description("Sets the member that is initialized with the starting date of the week that is being view")]
        public DependencyProperty WeekStartsProperty
        {
            get
            {
                return _WeekStartsProperty;
            }
            set
            {
                if (value != null)
                {
                    _WeekStartsProperty = value;
                    _WeekStartsProperty.ConnectableControl = this;
                }               
            }
        } 
        #endregion

        #region public object WeekStarts
        /// <MetaDataID>{5ce7d225-a739-4354-9e34-6c4fd5cd8013}</MetaDataID>
        private DateTime _WeekStarts;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public DateTime WeekStarts
        {
            get
            {
                return _WeekStarts;
            }
            set
            {
                if(_WeekStarts!=value)
                    _WeekStarts = value;
            }
        }
        #endregion

        #region public object DetailDayMember
        /// <MetaDataID>{36744913-cbf5-4c75-8afc-3eba8fdd5884}</MetaDataID>
        string _DetailDayMember;
        /// <MetaDataID>{27779918-a588-4d35-acc1-9eddca981e8d}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true), Description(@"When the table is in DayView the member is set with the specified date 
            otherwise the member has the DateTime.MinValue")]
        public object DetailDayMember
        {
            get
            {
                return _DetailDayMember;
            }
            set
            {
                if (value is string)
                    _DetailDayMember = value as string;
                else if (value is MetaData)
                    _DetailDayMember = (value as MetaData).Path;
            }
        } 
        #endregion

        #region internal DateTime UpdateWeekMembers(DateTime date)
        /// <summary>
        /// Sets the week starts member that includes the specified date and returns the starting week date
        /// </summary>
        /// <param name="weekstarts"></param>
        /// <MetaDataID>{7e46a332-0202-4b07-bfc8-43808662bb42}</MetaDataID>
        internal DateTime UpdateWeekMembers(DateTime date)
        {
            DateTime startweek = date;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Friday: startweek = date.Subtract(new TimeSpan(4, 0, 0, 0));//AddDays(3);
                    break;
                case DayOfWeek.Monday: startweek = date;//.AddDays(7);
                    break;
                case DayOfWeek.Saturday: startweek = date.Subtract(new TimeSpan(5, 0, 0, 0));//AddDays(2);
                    break;
                case DayOfWeek.Sunday: startweek = date.Subtract(new TimeSpan(6, 0, 0, 0)); //AddDays(1);
                    break;
                case DayOfWeek.Thursday: startweek = date.Subtract(new TimeSpan(3, 0, 0, 0)); //AddDays(4);
                    break;
                case DayOfWeek.Tuesday: startweek = date.Subtract(new TimeSpan(1, 0, 0, 0)); //AddDays(6);
                    break;
                case DayOfWeek.Wednesday: startweek = date.Subtract(new TimeSpan(2, 0, 0, 0)); //AddDays(5);
                    break;
                default:
                    break;
            }
                        
            this.DateStarts = new DateTime(startweek.Year, startweek.Month, startweek.Day, 0, 0, 0);
            SetOutDateMembers();
            return startweek;
        } 
        #endregion


        /// <summary>
        /// Initializes some properties of the control to communicate with the calling application and starts the View of the control
        /// These are the WeekStartsMember,DateStarts and DateEnds
        /// </summary>
        /// <param name="startweek"></param>
        /// <MetaDataID>{780c72f6-6456-4fa5-bbea-d4bf30005819}</MetaDataID>
        public void SetOutDateMembers()
        {
            //if (UserInterfaceObjectConnection != null)
            //    UserInterfaceObjectConnection.SetValue(this.DateStarts, _WeekStartsProperty);
            if(WeekStartsProperty!=null)
                WeekStartsProperty.Value=this.DateStarts;
            this.SelectedDate = this.DateStarts;
            //this.DateStarts = new DateTime(StartOfTheWeek.Year, StartOfTheWeek.Month, StartOfTheWeek.Day, 0, 0, 0);
            DateTime endweek = this.DateStarts;
            if(this.TableView!= TableViewState.DayView)
                endweek = this.DateStarts.AddDays(6);
            this.DateEnds = new DateTime(endweek.Year, endweek.Month, endweek.Day, 0, 0, 0);
        }

        /// <summary>
        /// used for the DayTimeColumnModel
        /// </summary>
        /// <param name="weekstarts"></param>
        /// <MetaDataID>{379b0cc7-c02a-47d5-ac25-995f353db74e}</MetaDataID>
        internal void SetDetailDayMember(DateTime detailday)
        {
            if (UserInterfaceObjectConnection != null)
                UserInterfaceObjectConnection.SetValue(detailday, _DetailDayMember);
        }

        //βλεπε SelecionMember στην ListConection και να το παιρνω απο το loadControlData
        //UserInterfaceObjectConnection.SetValue(value, _SelectionMember);
        //UserInterfaceObjectConnection.SetValue(value, _SelectionMember);
        //object value = UserInterfaceObjectConnection.GetDisplayedValue( _SelectionMember as string, this, out returnValueAsCollection);
        //UserInterfaceObjectConnection.GetValue( _SelectionMember);

        /// <MetaDataID>{7f7cdc48-2b41-4277-949a-9ae25b822ec3}</MetaDataID>
        private List<ISchedulerAction> _Actions = new List<ISchedulerAction>();
        /// <MetaDataID>{9684d33c-2749-4143-be4d-5ea62fbdd586}</MetaDataID>
        [Browsable(false)]
        internal List<ISchedulerAction> Actions
        {
            get
            {
                return _Actions;
            }
        }

        /// <MetaDataID>{c8938822-1993-4c0f-837f-16cd01244305}</MetaDataID>
        private List<ISchedulerAction> _LongActions = new List<ISchedulerAction>();
        /// <MetaDataID>{eb27cea6-5129-4201-be65-1dc576f5e653}</MetaDataID>
        private List<ISchedulerAction> LongActions
        {
            get
            {
                return _LongActions;
            }
        }

        /// <MetaDataID>{0a64cb51-cf13-4339-930e-42a99f27d43a}</MetaDataID>
        internal bool IsLongAction(ISchedulerAction action)
        {
            if (action.DateStart.Date < action.DateEnd.Date)
                return true;
            //if (action.DateStart.Year <= action.DateEnd.Year)
            //{
            //    if (action.DateEnd.Year > action.DateStart.Year)
            //        return true;
            //    if (action.DateStart.Month <= action.DateEnd.Month)
            //    {
            //        if (action.DateEnd.Month > action.DateStart.Month)//start=30/4 end 1/5
            //            return true;
            //        else if (action.DateStart.Day < action.DateEnd.Day)
            //            return true;
            //    }
            //}
            return false;
        }

        /// <MetaDataID>{98f67dc6-0575-4908-8d2a-7d214c7e1ec1}</MetaDataID>
        protected void OnDetailDayHScroll(object sender, ScrollEventArgs e)
        {
            this.DayRectangleHScroll(e.NewValue);
            this.Invalidate();
        }

        #region protected void DayRectangleHScroll(int value)
        /// <MetaDataID>{c18a89c2-e51d-4f63-8aed-d56bfdd09618}</MetaDataID>
        protected void DayRectangleHScroll(int value)
        {
            int scrollVal = this.DetailDayHScrollBar.Value - value;

            if (scrollVal != 0)
            {
                //Rectangle r = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);
                RECT scrollRect = RECT.FromRectangle(this.ColumnModel.DayRectangle);
                //Rectangle rclip = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.DisplayRectangle.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);

                //RECT clipRect = RECT.FromRectangle(rclip);

                Rectangle invalidateRect = scrollRect.ToRectangle();
                int lahp = (this.ColumnModel.Columns[1] as DayColumn).ActionWidth;
                scrollVal *= lahp;

                NativeMethods.ScrollWindow(this.Handle, scrollVal, 0, ref scrollRect, ref scrollRect);
                if (scrollVal < 0)
                {
                    invalidateRect.X = invalidateRect.Right + scrollVal;
                }
                //else
                //    invalidateRect.Y = invalidateRect.Top + scrollVal;

                //invalidateRect.Y = this.HeaderHeight ;
                invalidateRect.Width = Math.Abs(scrollVal);

                this.Invalidate(invalidateRect, false);

            }
        } 
        #endregion

        #region internal void UpdateDetailDayScroll()
        /// <MetaDataID>{bb55683f-ece4-42cb-8415-51ce4a7444b3}</MetaDataID>
        internal void UpdateDetailDayScroll()
        {
            if (ShowDetailDayScroll)
            {
                Rectangle hscrollBounds = new Rectangle(this.BorderWidth /*+ 80*/,
                    this.Height - 1 - SystemInformation.HorizontalScrollBarHeight,//this.BorderWidth
                    this.Width /*- 80*/ - (this.BorderWidth * 2) - SystemInformation.VerticalScrollBarWidth,
                    SystemInformation.HorizontalScrollBarHeight);

                //this._DetailDayHScrollBar.Visible = true;
                this._DetailDayHScrollBar.Bounds = hscrollBounds;
                this._DetailDayHScrollBar.Minimum = 0;

                int dayActionCount = (this.ColumnModel.Columns[1] as DayColumn).DayActionViewsCount;
                int visibleDayActionsCount = (this.ColumnModel.Columns[1] as DayColumn).VisibleDayActionsCount;

                if (dayActionCount > 0 && visibleDayActionsCount > 0)
                    this._DetailDayHScrollBar.Maximum = (dayActionCount > visibleDayActionsCount ? dayActionCount - 1 : visibleDayActionsCount);
                this._LongActionsHScrollBar.SmallChange = 1;

                if (visibleDayActionsCount > 0)
                    this._DetailDayHScrollBar.LargeChange = visibleDayActionsCount;

                if (this._DetailDayHScrollBar.Value > this._DetailDayHScrollBar.Maximum - this._DetailDayHScrollBar.LargeChange)
                {
                    this._DetailDayHScrollBar.Value = this._DetailDayHScrollBar.Maximum - this._DetailDayHScrollBar.LargeChange;
                }
            }
            else
            {
                this._DetailDayHScrollBar.Visible = false;
                this._DetailDayHScrollBar.Value = 0;
            }
        } 
        #endregion

        /// <MetaDataID>{924809d9-790c-461f-b3fa-f396c6f01468}</MetaDataID>
        internal int ActionsPadding = 3;

        /// <MetaDataID>{49cfe405-a5b4-4321-b044-b4b8c2794709}</MetaDataID>
        internal void UpdateLongActionsScroll()
        {
            if (ShowLongActionsScroll)
            {
                int vscrollwidth = VScroll == true ? this.VScrollBar.Width : 0;
                
                Rectangle hscrollBounds = new Rectangle(this.BorderWidth /*+ 80*/,
                    this.Height - 1 - SystemInformation.HorizontalScrollBarHeight,//this.BorderWidth
                    this.Width - (this.BorderWidth * 2) - vscrollwidth,
                    SystemInformation.HorizontalScrollBarHeight);
                //Rectangle vscrollBounds = new Rectangle(this._TimeColumnWidth - SystemInformation.VerticalScrollBarWidth,
                //    this.BorderWidth+this.HeaderHeight,
                //    SystemInformation.VerticalScrollBarWidth,
                //    this.LongActionsRectHeight);


                this._LongActionsHScrollBar.Visible = true;
                this._LongActionsHScrollBar.Bounds = hscrollBounds;
                this._LongActionsHScrollBar.Minimum = 0;
                //int lahp = this.LongActionHeight + this._LongActionsPadding;
                if (LongActionsCount > 0 && VisibleLongActionsCount > 0)
                    this._LongActionsHScrollBar.Maximum = (LongActionsCount > this.VisibleLongActionsCount ? LongActionsCount-1 : this.VisibleLongActionsCount);
                this._LongActionsHScrollBar.SmallChange = 1;

                if (this.VisibleLongActionsCount > 0)
                    this._LongActionsHScrollBar.LargeChange = this.VisibleLongActionsCount ;

                if (this._LongActionsHScrollBar.Value > this._LongActionsHScrollBar.Maximum - this._LongActionsHScrollBar.LargeChange)
                {
                    this._LongActionsHScrollBar.Value = this._LongActionsHScrollBar.Maximum - this._LongActionsHScrollBar.LargeChange;
                }
            }
            else
            {
                this._LongActionsHScrollBar.Visible = false;
                this._LongActionsHScrollBar.Value = 0;
            }
        }

        /// <MetaDataID>{9bce8dce-62b7-4ddd-9cac-7dd1bda1c737}</MetaDataID>
        protected void OnLongActionVerticalScroll(object sender, ScrollEventArgs e)
        {
            this.LongActionsVerticalScroll(e.NewValue);
            this.Invalidate();
        }

        /// <MetaDataID>{9f112d94-6bd4-4164-876c-0245d3066155}</MetaDataID>
        protected void LongActionsVerticalScroll(int value)
        {
            int scrollVal = this._LongActionsHScrollBar.Value - value;

            if (scrollVal != 0)
            {
                //Rectangle r = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);
                RECT scrollRect = RECT.FromRectangle(LongActionsRectangle);
                //Rectangle rclip = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.DisplayRectangle.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);

                //RECT clipRect = RECT.FromRectangle(rclip);

                Rectangle invalidateRect = scrollRect.ToRectangle();
                int lahp = this.LongActionHeight + this.ActionsPadding;
                scrollVal *= lahp;

                NativeMethods.ScrollWindow(this.Handle, 0, scrollVal, ref scrollRect, ref scrollRect);
                if (scrollVal < 0)
                {
                    invalidateRect.Y = invalidateRect.Bottom + scrollVal;
                }
                //else
                //    invalidateRect.Y = invalidateRect.Top + scrollVal;

                //invalidateRect.Y = this.HeaderHeight ;
                invalidateRect.Height = Math.Abs(scrollVal);

                this.Invalidate(invalidateRect, false);

            }
        }


        /// <MetaDataID>{60fb1f72-83b6-4d5c-b317-4f8c4780cbff}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        /// <MetaDataID>{3eb0003c-3dbd-4800-811f-6bbf0eb7e7de}</MetaDataID>
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public ViewControlObject ViewControlObject
        {
            get
            {
                if (UserInterfaceObjectConnection == null)
                    return null;
                return UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject;
            }
            set
            {
                if (value != null)
                    UserInterfaceObjectConnection = value.UserInterfaceObjectConnection;
                else
                    UserInterfaceObjectConnection = null;
            }
        }

        #region Methods

        #region Coordinate Translation

        #region ClientToDisplayRect

        /// <summary>
        /// Computes the location of the specified client point into coordinates 
        /// relative to the display rectangle
        /// </summary>
        /// <param name="x">The client x coordinate to convert</param>
        /// <param name="y">The client y coordinate to convert</param>
        /// <returns>A Point that represents the converted coordinates (x, y), 
        /// relative to the display rectangle</returns>
        /// <MetaDataID>{F4680C1A-9202-4813-B0A1-C22ABE3CAD79}</MetaDataID>
        public Point ClientToDisplayRect(int x, int y)
        {
            int xPos = x - this.BorderWidth;

            //if (this.HScroll)
            //{
            //    xPos += this.hScrollBar.Value;
            //}

            int yPos = y - this.BorderWidth;

            if (this.VScroll)
            {
                yPos += this.TopIndex * this.RowHeight;
            }

            return new Point(xPos, yPos);
        }


        /// <summary>
        /// Computes the location of the specified client point into coordinates 
        /// relative to the display rectangle
        /// </summary>
        /// <param name="p">The client coordinate Point to convert</param>
        /// <returns>A Point that represents the converted Point, p, 
        /// relative to the display rectangle</returns>
        /// <MetaDataID>{9091FE51-0A20-442C-B402-6245BCC55E60}</MetaDataID>
        public Point ClientToDisplayRect(Point p)
        {
            return this.ClientToDisplayRect(p.X, p.Y);
        }


        /// <summary>
        /// Converts the location of the specified Rectangle into coordinates 
        /// relative to the display rectangle
        /// </summary>
        /// <param name="rect">The Rectangle to convert whose location is in 
        /// client coordinates</param>
        /// <returns>A Rectangle that represents the converted Rectangle, rect, 
        /// relative to the display rectangle</returns>
        /// <MetaDataID>{20EE23A6-9CAD-4D01-A7F0-2C8EB649AC3E}</MetaDataID>
        public Rectangle ClientToDisplayRect(Rectangle rect)
        {
            return new Rectangle(this.ClientToDisplayRect(rect.Location), rect.Size);
        }

        #endregion

        #region DisplayRectToClient

        /// <summary>
        /// Computes the location of the specified point relative to the display 
        /// rectangle point into client coordinates 
        /// </summary>
        /// <param name="x">The x coordinate to convert relative to the display rectangle</param>
        /// <param name="y">The y coordinate to convert relative to the display rectangle</param>
        /// <returns>A Point that represents the converted coordinates (x, y) relative to 
        /// the display rectangle in client coordinates</returns>
        /// <MetaDataID>{FE28B61F-6BDD-49A9-B75C-89093EBC7F8D}</MetaDataID>
        public Point DisplayRectToClient(int x, int y)
        {
            int xPos = x + this.BorderWidth;

            //if (this.HScroll)
            //{
            //    xPos -= this.hScrollBar.Value;
            //}

            int yPos = y + this.BorderWidth;

            if (this.VScroll)
            {
                yPos -= this.TopIndex * this.RowHeight;
            }

            return new Point(xPos, yPos);
        }


        /// <summary>
        /// Computes the location of the specified point relative to the display 
        /// rectangle into client coordinates 
        /// </summary>
        /// <param name="p">The point relative to the display rectangle to convert</param>
        /// <returns>A Point that represents the converted Point relative to 
        /// the display rectangle, p, in client coordinates</returns>
        /// <MetaDataID>{EDD4784A-A2EF-4BB7-B4D6-51F64CA76CD3}</MetaDataID>
        public Point DisplayRectToClient(Point p)
        {
            return this.DisplayRectToClient(p.X, p.Y);
        }


        /// <summary>
        /// Converts the location of the specified Rectangle relative to the display 
        /// rectangle into client coordinates 
        /// </summary>
        /// <param name="rect">The Rectangle to convert whose location is relative to 
        /// the display rectangle</param>
        /// <returns>A Rectangle that represents the converted Rectangle relative to 
        /// the display rectangle, rect, in client coordinates</returns>
        /// <MetaDataID>{A59B149F-11EC-4B3A-84D8-9795089AF7BD}</MetaDataID>
        public Rectangle DisplayRectToClient(Rectangle rect)
        {
            return new Rectangle(this.DisplayRectToClient(rect.Location), rect.Size);
        }

        #endregion

        #region Cells

        /// <summary>
        /// Returns the Cell at the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate of the Cell</param>
        /// <param name="y">The client y coordinate of the Cell</param>
        /// <returns>The Cell at the specified client coordinates, or
        /// null if it does not exist</returns>
        /// <MetaDataID>{622458A8-7192-4A6E-ADEC-2FF46A3F9EA3}</MetaDataID>
        public Cell CellAt(int x, int y)
        {
            int row = this.RowIndexAt(x, y);
            int column = this.ColumnIndexAt(x, y);

            // return null if the row or column don't exist
            if (row == -1 || row >= this.TableModel.TotalRows.Count || column == -1 || column >= this.TableModel.TotalRows[row].Cells.Count)
            {
                return null;
            }

            return this.TableModel[row, column];
        }


        /// <summary>
        /// Returns the Cell at the specified client Point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>The Cell at the specified client Point, 
        /// or null if not found</returns>
        /// <MetaDataID>{3828568A-272E-48C0-986D-48CD432D1331}</MetaDataID>
        public Cell CellAt(Point p)
        {
            return this.CellAt(p.X, p.Y);
        }


        /// <summary>
        /// Returns a Rectangle that specifies the size and location the cell at 
        /// the specified row and column indexes in client coordinates
        /// </summary>
        /// <param name="row">The index of the row that contains the cell</param>
        /// <param name="column">The index of the column that contains the cell</param>
        /// <returns>A Rectangle that specifies the size and location the cell at 
        /// the specified row and column indexes in client coordinates</returns>
        /// <MetaDataID>{09A4AD98-3EA7-44A2-BBCA-7C680FAFCD0D}</MetaDataID>
        public Rectangle CellRect(int row, int column)
        {
            // return null if the row or column don't exist
            if (row == -1 || row >= this.TableModel.TotalRows.Count || column == -1 || column >= this.TableModel.TotalRows[row].Cells.Count)
            {
                return Rectangle.Empty;
            }

            Rectangle columnRect = this.ColumnRect(column);

            if (columnRect == Rectangle.Empty)
            {
                return columnRect;
            }

            Rectangle rowRect = this.RowRect(row);

            if (rowRect == Rectangle.Empty)
            {
                return rowRect;
            }

            return new Rectangle(columnRect.X, rowRect.Y, columnRect.Width, rowRect.Height);
        }


        /// <summary>
        /// Returns a Rectangle that specifies the size and location the cell at 
        /// the specified cell position in client coordinates
        /// </summary>
        /// <param name="cellPos">The position of the cell</param>
        /// <returns>A Rectangle that specifies the size and location the cell at 
        /// the specified cell position in client coordinates</returns>
        /// <MetaDataID>{31BB48C4-E186-4D4F-8C52-B5E04FA14AA8}</MetaDataID>
        public Rectangle CellRect(CellPos cellPos)
        {
            return this.CellRect(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        ///  Returns a Rectangle that specifies the size and location of the 
        ///  specified cell in client coordinates
        /// </summary>
        /// <param name="cell">The cell whose bounding rectangle is to be retrieved</param>
        /// <returns>A Rectangle that specifies the size and location the specified 
        /// cell in client coordinates</returns>
        /// <MetaDataID>{4C5E8CBB-F434-4183-8386-5179DA2B3D2A}</MetaDataID>
        public Rectangle CellRect(Cell cell)
        {
            if (cell == null || cell.Row == null || cell.InternalIndex == -1)
            {
                return Rectangle.Empty;
            }

            if (this.TableModel == null || this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            int row = this.TableModel.TotalRows.IndexOf(cell.Row);
            int col = cell.InternalIndex;

            return this.CellRect(row, col);
        }


        /// <summary>
        /// Returns whether Cell at the specified row and column indexes 
        /// is not null
        /// </summary>
        /// <param name="row">The row index of the cell</param>
        /// <param name="column">The column index of the cell</param>
        /// <returns>True if the cell at the specified row and column indexes 
        /// is not null, otherwise false</returns>
        /// <MetaDataID>{8EE2846C-7215-45E7-A753-B9D9C90C1BF7}</MetaDataID>
        protected internal bool IsValidCell(int row, int column)
        {
            if (this.TableModel != null && this.ColumnModel != null)
            {
                if (row >= 0 && row < this.TableModel.TotalRows.Count)
                {
                    if (column >= 1 && column < this.ColumnModel.Columns.Count)
                    {
                        return (this.TableModel.TotalRows[row].Cells[column] != null);
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Returns whether Cell at the specified cell position is not null
        /// </summary>
        /// <param name="cellPos">The position of the cell</param>
        /// <returns>True if the cell at the specified cell position is not 
        /// null, otherwise false</returns>
        /// <MetaDataID>{2A2B4F6B-2E7D-449E-91E2-C93866C084FD}</MetaDataID>
        protected internal bool IsValidCell(CellPos cellPos)
        {
            return this.IsValidCell(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        /// Returns a CellPos that specifies the next Cell that is visible 
        /// and enabled from the specified Cell
        /// </summary>
        /// <param name="start">A CellPos that specifies the Cell to start 
        /// searching from</param>
        /// <param name="wrap">Specifies whether to move to the start of the 
        /// next Row when the end of the current Row is reached</param>
        /// <param name="forward">Specifies whether the search should travel 
        /// in a forward direction (top to bottom, left to right) through the Cells</param>
        /// <param name="includeStart">Indicates whether the specified starting 
        /// Cell is included in the search</param>
        /// <param name="checkOtherCellsInRow">Specifies whether all Cells in 
        /// the Row should be included in the search</param>
        /// <returns>A CellPos that specifies the next Cell that is visible 
        /// and enabled, or CellPos.Empty if there are no Cells that are visible 
        /// and enabled</returns>
        /// <MetaDataID>{E086300F-29F8-4DB8-883A-15911CD8D786}</MetaDataID>
        protected CellPos FindNextVisibleEnabledCell(CellPos start, bool wrap, bool forward, bool includeStart, bool checkOtherCellsInRow)
        {
            if (this.ColumnCount == 0 || this.RowCount == 0)
            {
                return CellPos.Empty;
            }


            int startRow = start.Row != -1 ? start.Row : 0;
            int startCol = start.Column != -1 ? start.Column : 0;

            bool first = true;

            if (forward)
            {
                for (int i = startRow; i < this.RowCount; i++)
                {
                    int j = (first || !checkOtherCellsInRow ? startCol : 0);

                    for (; j < this.TableModel.TotalRows[i].Cells.Count; j++)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                if (!checkOtherCellsInRow)
                                {
                                    break;
                                }

                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Enabled && this.ColumnModel.Columns[j].Enabled && this.ColumnModel.Columns[j].Visible)
                        {
                            return new CellPos(i, j);
                        }

                        if (!checkOtherCellsInRow)
                        {
                            continue;
                        }
                    }

                    if (wrap)
                    {
                        if (i + 1 == this.TableModel.TotalRows.Count)
                        {
                            i = -1;
                        }
                    }
                    else
                    {
                        if (checkOtherCellsInRow)
                        {
                            if (FocusedActionViews != null && FocusedActionViews.Count > 0)
                                FocusedActionViews.Clear();

                            if (this.TableView != TableViewState.DayView)
                            {                                
                                DateTime date = (this.ColumnModel.Columns[start.Column] as DayColumn).Date;
                                date = date.AddDays(1);
                                this.ColumnModel.CreateAndShowWeek(date, false);
                                this.ColumnModel.UpdateColumnsWidth();//this.TableModel.TotalRowHeight, this.Height
                                this.ShowLongActionsScroll = false;

                                CallLoadActionsOperationCall();                                

                                return new CellPos(start.Row, start.Column);
                            }
                            else
                            {
                                DateTime date = (this.ColumnModel.Columns[start.Column] as DayColumn).Date;
                                date = date.AddDays(1);
                                string datestr = date.ToLongDateString();
                                DayColumn clm = this.ColumnModel.CreateDayColumn(datestr,date);
                                
                                this.ShowDetailDayScroll = false;

                                this.ColumnModel.ClearDayColumns();
                                this.ColumnModel.Columns.AddToIndex(1, clm);
                                this.ColumnModel.UpdateColumnsWidth();
                                this.DateStarts = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                                SetOutDateMembers();
                                CallLoadActionsOperationCall();
                                return new CellPos(start.Row, start.Column);
                            }                            
                        }
                        else
                            break;
                    }
                }
            }
            else
            {
                for (int i = startRow; i >= 0; i--)
                {
                    int j = (first || !checkOtherCellsInRow ? startCol : this.TableModel.TotalRows[i].Cells.Count);

                    for (; j >= 0; j--)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                if (!checkOtherCellsInRow)
                                {
                                    break;
                                }

                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Enabled && this.ColumnModel.Columns[j].Enabled && this.ColumnModel.Columns[j].Visible)
                        {
                            return new CellPos(i, j);
                        }

                        if (!checkOtherCellsInRow)
                        {
                            continue;
                        }
                    }

                    if (wrap)
                    {
                        if (i - 1 == -1)
                        {
                            i = this.TableModel.TotalRows.Count;
                        }
                    }
                    else
                    {                        
                        if (checkOtherCellsInRow)
                        {
                            if (FocusedActionViews != null && FocusedActionViews.Count > 0)
                                FocusedActionViews.Clear();

                            if (this.TableView != TableViewState.DayView)
                            {
                                DateTime date = (this.ColumnModel.Columns[start.Column] as DayColumn).Date;
                                //this.ColumnModel.Columns.Remove(this.ColumnModel.Columns[7]);
                                TimeSpan ts = new TimeSpan(1, 0, 0, 0);
                                date = date.Subtract(ts);
                                this.ColumnModel.CreateAndShowWeek(date, false);
                                //DayColumn dc = this.ColumnModel.CreateDayColumn(date.ToLongDateString(), date);
                                //this.ColumnModel.Columns.AddToIndex(1, dc);
                                this.ColumnModel.UpdateColumnsWidth();//this.TableModel.TotalRowHeight, this.Height
                                this.ShowLongActionsScroll = false;
                                CallLoadActionsOperationCall();
                                return new CellPos(start.Row, start.Column);
                            }
                            else
                            {
                                DateTime date = (this.ColumnModel.Columns[start.Column] as DayColumn).Date;
                                TimeSpan ts = new TimeSpan(1, 0, 0, 0);
                                date = date.Subtract(ts);

                                string datestr = date.ToLongDateString();
                                DayColumn clm = this.ColumnModel.CreateDayColumn(datestr, date);
                                this.ShowDetailDayScroll = false;

                                this.ColumnModel.ClearDayColumns();
                                this.ColumnModel.Columns.AddToIndex(1, clm);
                                this.ColumnModel.UpdateColumnsWidth();
                                this.DateStarts = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                                SetOutDateMembers();
                                CallLoadActionsOperationCall();
                                return new CellPos(start.Row, start.Column);
                            }
                        }
                        else
                            break;
                    }
                }
            }

            return CellPos.Empty;
        }

        /// <summary>
        /// Returns a CellPos that specifies the next Cell that able to be 
        /// edited from the specified Cell
        /// </summary>
        /// <param name="start">A CellPos that specifies the Cell to start 
        /// searching from</param>
        /// <param name="wrap">Specifies whether to move to the start of the 
        /// next Row when the end of the current Row is reached</param>
        /// <param name="forward">Specifies whether the search should travel 
        /// in a forward direction (top to bottom, left to right) through the Cells</param>
        /// <param name="includeStart">Indicates whether the specified starting 
        /// Cell is included in the search</param>
        /// <returns>A CellPos that specifies the next Cell that is able to
        /// be edited, or CellPos.Empty if there are no Cells that editable</returns>
        /// <MetaDataID>{8566C171-3A55-4D82-A450-774B7FCDEB3D}</MetaDataID>
        protected CellPos FindNextEditableCell(CellPos start, bool wrap, bool forward, bool includeStart)
        {
            if (this.ColumnCount == 0 || this.RowCount == 0)
            {
                return CellPos.Empty;
            }

            int startRow = start.Row != -1 ? start.Row : 0;
            int startCol = start.Column != -1 ? start.Column : 0;

            bool first = true;

            if (forward)
            {
                for (int i = startRow; i < this.RowCount; i++)
                {
                    int j = (first ? startCol : 0);

                    for (; j < this.TableModel.TotalRows[i].Cells.Count; j++)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Editable && this.ColumnModel.Columns[j].Editable)
                        {
                            return new CellPos(i, j);
                        }
                    }

                    if (wrap)
                    {
                        if (i + 1 == this.TableModel.TotalRows.Count)
                        {
                            i = -1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = startRow; i >= 0; i--)
                {
                    int j = (first ? startCol : this.TableModel.TotalRows[i].Cells.Count);

                    for (; j >= 0; j--)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Editable && this.ColumnModel.Columns[j].Editable)
                        {
                            return new CellPos(i, j);
                        }
                    }

                    if (wrap)
                    {
                        if (i - 1 == -1)
                        {
                            i = this.TableModel.TotalRows.Count;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return CellPos.Empty;
        }

        #endregion

        #region Columns

        /// <summary>
        /// Returns the index of the Column at the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate of the Column</param>
        /// <param name="y">The client y coordinate of the Column</param>
        /// <returns>The index of the Column at the specified client coordinates, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{3E026154-BA60-4FE2-AC82-EE377712D28D}</MetaDataID>
        public int ColumnIndexAt(int x, int y)
        {
            if (this.ColumnModel == null)
            {
                return -1;
            }

            // convert to DisplayRect coordinates before 
            // sending to the ColumnModel
            return this.ColumnModel.ColumnIndexAtX(x - this.BorderWidth);//this.hScrollBar.Value
        }


        /// <summary>
        /// Returns the index of the Column at the specified client point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>The index of the Column at the specified client point, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{921E6214-2C2A-4681-8575-21D185ACAFB5}</MetaDataID>
        public int ColumnIndexAt(Point p)
        {
            return this.ColumnIndexAt(p.X, p.Y);
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified 
        /// column's header in client coordinates
        /// </summary>
        /// <param name="column">The index of the column</param>
        /// <returns>The bounding rectangle of the specified 
        /// column's header</returns>
        /// <MetaDataID>{DBCA2C31-A848-4AC9-8596-A6A0859EFFB7}</MetaDataID>
        public Rectangle ColumnHeaderRect(int column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = this.ColumnModel.ColumnHeaderRect(column);

            if (rect == Rectangle.Empty)
            {
                return rect;
            }

            rect.X -= this.BorderWidth;//this.hScrollBar.Value - 
            rect.Y = this.BorderWidth;

            return rect;
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified 
        /// column's header in client coordinates
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The bounding rectangle of the specified 
        /// column's header</returns>
        /// <MetaDataID>{28BF0218-06C1-4C32-A1BA-1702F4854542}</MetaDataID>
        public Rectangle ColumnHeaderRect(Column column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            return this.ColumnHeaderRect(this.ColumnModel.Columns.IndexOf(column));
        }


        /// <summary>
        /// Returns the bounding rectangle of the column at the 
        /// specified index in client coordinates
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The bounding rectangle of the column at the 
        /// specified index</returns>
        /// <MetaDataID>{6A0B3E89-A89B-4214-A240-05D0B15E0012}</MetaDataID>
        public Rectangle ColumnRect(int column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = this.ColumnHeaderRect(column);

            if (rect == Rectangle.Empty)
            {
                return rect;
            }

            rect.Y += this.HeaderHeight;
            rect.Height = this.TotalRowHeight;

            return rect;
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified column 
        /// in client coordinates
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The bounding rectangle of the specified 
        /// column</returns>
        /// <MetaDataID>{68A72ECC-D157-4CB3-961E-77B4666880B3}</MetaDataID>
        public Rectangle ColumnRect(Column column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            return this.ColumnRect(this.ColumnModel.Columns.IndexOf(column));
        }

        #endregion

        #region TotalRows

        /// <summary>
        /// Returns the index of the Row at the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate of the Row</param>
        /// <param name="y">The client y coordinate of the Row</param>
        /// <returns>The index of the Row at the specified client coordinates, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{2A210CE8-7A9D-4176-9091-52AC02FAC2D7}</MetaDataID>
        public int RowIndexAt(int x, int y)
        {
            if (this.TableModel == null)
            {
                return -1;
            }

            y -= (this.LongActionsRectHeight + this.HeaderHeight);

            //if (this.HeaderStyle != ColumnHeaderStyle.None)
            //{
            //    y -= (this.LongActionsRectHeight + this.HeaderHeight);
            //}

            y -= this.BorderWidth;

            if (y < 0)
            {
                return -1;
            }

            if (this.VScroll)
            {
                y += this.TopIndex * this.RowHeight;
            }

            return this.TableModel.RowIndexAt(y);
        }


        /// <summary>
        /// Returns the index of the Row at the specified client point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>The index of the Row at the specified client point, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{64581C9A-CDBB-4015-AB47-BF3EF3F481CC}</MetaDataID>
        public int RowIndexAt(Point p)
        {
            return this.RowIndexAt(p.X, p.Y);
        }


        /// <summary>
        /// Returns the bounding rectangle of the row at the 
        /// specified index in client coordinates
        /// </summary>
        /// <param name="row">The index of the row</param>
        /// <returns>The bounding rectangle of the row at the 
        /// specified index</returns>
        /// <MetaDataID>{1D7D17B7-6AE3-4727-98A4-4F057213340B}</MetaDataID>
        public Rectangle RowRect(int row)
        {
            if (this.TableModel == null || this.ColumnModel == null || row == -1 || row > this.TableModel.TotalRows.Count)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = new Rectangle();

            rect.X = this.DisplayRectangle.X;
            rect.Y = this.BorderWidth + ((row - this.TopIndex) * this.RowHeight);

            rect.Width = System.Convert.ToInt32(this.ColumnModel.VisibleColumnsWidth);
            rect.Height = this.RowHeight;

            rect.Y += (this.LongActionsRectHeight + this.HeaderHeight);
            //if (this.HeaderStyle != ColumnHeaderStyle.None)
            //{
            //    rect.Y += (this.LongActionsRectHeight+this.HeaderHeight);
            //}

            return rect;
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified row 
        /// in client coordinates
        /// </summary>
        /// <param name="row">The row</param>
        /// <returns>The bounding rectangle of the specified 
        /// row</returns>
        /// <MetaDataID>{6600F62C-8819-4554-96FB-8524C2537504}</MetaDataID>
        public Rectangle RowRect(Row row)
        {
            if (this.TableModel == null)
            {
                return Rectangle.Empty;
            }

            return this.RowRect(this.TableModel.TotalRows.IndexOf(row));
        }

        #endregion

        #region Hit Tests

        /// <MetaDataID>{5ff852cd-f18a-4c6a-9c4c-f855690c1fb5}</MetaDataID>
        private List<SchedulerActionView> HitActionTest(int x, int y)
        {
            List<SchedulerActionView> avs = new List<SchedulerActionView>();
            SchedulerActionView sav = null;
            int scrollindex = -1;
            for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
            {
                DayColumn day_column = this.ColumnModel.Columns[ic]  as DayColumn;
                foreach (SchedulerActionView action_view in day_column.ActionViews)
                {
                    if (action_view.IsLongActionView)
                        scrollindex = this.TopLongActionIndex;
                    else
                        scrollindex=this.LeftDayActionIndex;

                    if (action_view.Contains(x, y) && action_view.Index >= scrollindex)//Is Visible  after scroll
                    {
                        sav = action_view;
                        break;
                    }
                }
            }

            if (sav != null)
            {
                for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                {
                    DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                    foreach (SchedulerActionView action_view in day_column.ActionViews)
                    {
                        if (sav.Action == action_view.Action)
                        {
                            avs.Add(action_view);                           
                        }
                    }
                }
            }
            return avs;
        }

        /// <summary>
        /// Returns a TableRegions value that represents the table region at 
        /// the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate</param>
        /// <param name="y">The client y coordinate</param>
        /// <returns>A TableRegions value that represents the table region at 
        /// the specified client coordinates</returns>
        /// <MetaDataID>{6391A3B9-34B5-4FDE-B206-20667CF201B5}</MetaDataID>
        public TableRegion HitTest(int x, int y)
        {            

            if (this.HeaderRectangle.Contains(x, y))//this.HeaderStyle != ColumnHeaderStyle.None && 
            {
                return TableRegion.ColumnHeader;
            }
            else if (this.CellDataRect.Contains(x, y))
            {
                return TableRegion.Cells;
            }
            else if (!this.Bounds.Contains(x, y))
            {
                return TableRegion.NoWhere;
            }

            return TableRegion.NonClientArea;
        }


        /// <summary>
        /// Returns a TableRegions value that represents the table region at 
        /// the specified client point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>A TableRegions value that represents the table region at 
        /// the specified client point</returns>
        /// <MetaDataID>{096F6E8D-67A3-4380-9AB8-A9E08B47AA08}</MetaDataID>
        public TableRegion HitTest(Point p)
        {
            return this.HitTest(p.X, p.Y);
        }

        #endregion

        #endregion

        #region Dispose

        /// <summary>
        /// Releases the unmanaged resources used by the Control and optionally 
        /// releases the managed resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged 
        /// resources; false to release only unmanaged resources</param>
        /// <MetaDataID>{76286D37-0EE2-4A87-93DD-9267B1DD5A9E}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Removes the ColumnModel and TableModel from the Table
        /// </summary>
        /// <MetaDataID>{480ED4F9-3163-4BEA-9496-650FD9450E77}</MetaDataID>
        public void Clear()
        {
            if (this.ColumnModel != null)
            {
                this.ColumnModel = null;
            }

            if (this.TableModel != null)
            {
                this.TableModel = null;
            }
        }

        #endregion

        /// <summary>
        /// Returns whether the Cell at the specified row and column is able 
        /// to respond to user interaction
        /// </summary>
        /// <param name="row">The row index of the Cell to check</param>
        /// <param name="column">The column index of the Cell to check</param>
        /// <returns>True if the Cell at the specified row and column is able 
        /// to respond to user interaction, false otherwise</returns>
        /// <MetaDataID>{5A9E0D6E-5EF3-4F93-88B2-51524E358DD8}</MetaDataID>
        public bool IsCellEnabled(int row, int column)
        {
            return this.IsCellEnabled(new CellPos(row, column));
        }



        /// <summary>
        /// Returns whether the Cell at the specified CellPos is able 
        /// to respond to user interaction
        /// </summary>
        /// <param name="cellpos">A CellPos that specifies the Cell to check</param>
        /// <returns>True if the Cell at the specified CellPos is able 
        /// to respond to user interaction, false otherwise</returns>
        /// <MetaDataID>{867F0780-65EA-4466-87AA-263030C06905}</MetaDataID>
        public bool IsCellEnabled(CellPos cellpos)
        {
            // don't bother if the cell doesn't exists or the cell's
            // column is not visible
            if (!this.IsValidCell(cellpos) || !this.ColumnModel.Columns[cellpos.Column].Visible)
            {
                return false;
            }

            return (this.TableModel[cellpos].Enabled &&
                this.ColumnModel.Columns[cellpos.Column].Enabled);
        }

        #region Invalidate
        
        /// <summary>
        /// Invalidates the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to be invalidated</param>
        /// <MetaDataID>{B07D2099-430C-46B9-AFD5-C2A895C7B152}</MetaDataID>
        public void InvalidateCell(Cell cell)
        {
            this.InvalidateCell(cell.Row.Index, cell.Index);
        }


        /// <summary>
        /// Invalidates the Cell located at the specified row and column indicies
        /// </summary>
        /// <param name="row">The row index of the Cell to be invalidated</param>
        /// <param name="column">The column index of the Cell to be invalidated</param>
        /// <MetaDataID>{5453565C-B895-4A29-8ACA-4C0295AD6A43}</MetaDataID>
        public void InvalidateCell(int row, int column)
        {
            Rectangle cellRect = this.CellRect(row, column);

            if (cellRect == Rectangle.Empty)
            {
                return;
            }

            if (cellRect.IntersectsWith(this.CellDataRect))
            {
                this.Invalidate(Rectangle.Intersect(this.CellDataRect, cellRect), false);
            }
        }


        /// <summary>
        /// Invalidates the Cell located at the specified CellPos
        /// </summary>
        /// <param name="cellPos">A CellPos that specifies the Cell to be invalidated</param>
        /// <MetaDataID>{ADEBBD00-5315-4E77-803D-B2AD258E80D1}</MetaDataID>
        public void InvalidateCell(CellPos cellPos)
        {
            this.InvalidateCell(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        /// Invalidates the specified Row
        /// </summary>
        /// <param name="row">The Row to be invalidated</param>
        /// <MetaDataID>{4C538CD3-5D5D-4153-AC5A-37074F88FF34}</MetaDataID>
        public void InvalidateRow(Row row)
        {
            this.InvalidateRow(row.Index);
        }


        /// <summary>
        /// Invalidates the Row located at the specified row index
        /// </summary>
        /// <param name="row">The row index of the Row to be invalidated</param>
        /// <MetaDataID>{681EBBED-AEFA-488D-AF78-A15AC119DA38}</MetaDataID>
        public void InvalidateRow(int row)
        {
            Rectangle rowRect = this.RowRect(row);

            if (rowRect == Rectangle.Empty)
            {
                return;
            }

            if (rowRect.IntersectsWith(this.CellDataRect))
            {
                this.Invalidate(Rectangle.Intersect(this.CellDataRect, rowRect), false);
            }
        }


        /// <summary>
        /// Invalidates the Row located at the specified CellPos
        /// </summary>
        /// <param name="cellPos">A CellPos that specifies the Row to be invalidated</param>
        /// <MetaDataID>{32F7FAD9-4FA6-4278-94C1-975C8C1BE949}</MetaDataID>
        public void InvalidateRow(CellPos cellPos)
        {
            this.InvalidateRow(cellPos.Row);
        }

        #endregion

        #region Keys

        /// <summary>
        /// Determines whether the specified key is reserved for use by the Table
        /// </summary>
        /// <param name="key">One of the Keys values</param>
        /// <returns>true if the specified key is reserved for use by the Table; 
        /// otherwise, false</returns>
        /// <MetaDataID>{0A7EE5D6-DA0E-4D0D-AD2D-1E439CD04B25}</MetaDataID>
        protected internal bool IsReservedKey(Keys key)
        {
            if ((key & Keys.Alt) != Keys.Alt)
            {
                Keys k = key & Keys.KeyCode;

                return (k == Keys.Up ||
                    k == Keys.Down ||
                    k == Keys.Left ||
                    k == Keys.Right ||
                    k == Keys.PageUp ||
                    k == Keys.PageDown ||
                    k == Keys.Home ||
                    k == Keys.End ||
                    k == Keys.Tab ||
                    k == Keys.Insert ||
                    k == Keys.Delete);
            }

            return false;
        }


        /// <summary>
        /// Determines whether the specified key is a regular input key or a special 
        /// key that requires preprocessing
        /// </summary>
        /// <param name="keyData">One of the Keys values</param>
        /// <returns>true if the specified key is a regular input key; otherwise, false</returns>
        /// <MetaDataID>{4B1F0419-FE9A-401A-B6E6-C5470C945993}</MetaDataID>
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) != Keys.Alt)
            {
                Keys key = keyData & Keys.KeyCode;

                switch (key)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Prior:
                    case Keys.Next:
                    case Keys.End:
                    case Keys.Home:
                        {
                            return true;
                        }
                }

                if (base.IsInputKey(keyData))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Layout

        /// <summary>
        /// Prevents the Table from drawing until the EndUpdate method is called
        /// </summary>
        /// <MetaDataID>{9E9FE3D4-BB02-4693-8A57-5AB380E81E61}</MetaDataID>
        public void BeginUpdate()
        {
            if (this.IsHandleCreated)
            {
                if (this.beginUpdateCount == 0)
                {
                    NativeMethods.SendMessage(this.Handle, 11, 0, 0);
                }

                this.beginUpdateCount++;
            }
        }


        /// <summary>
        /// Resumes drawing of the Table after drawing is suspended by the 
        /// BeginUpdate method
        /// </summary>
        /// <MetaDataID>{D09161ED-D0DA-47C5-AEE5-83669E8258D0}</MetaDataID>
        public void EndUpdate()
        {
            if (this.beginUpdateCount <= 0)
            {
                return;
            }

            this.beginUpdateCount--;

            if (this.beginUpdateCount == 0)
            {
                NativeMethods.SendMessage(this.Handle, 11, -1, 0);

                this.PerformLayout();
                this.Invalidate(true);
            }
        }


        /// <summary>
        /// Signals the object that initialization is starting
        /// </summary>
        /// <MetaDataID>{D5198D05-4E3D-4A36-A64D-12F0B4EE0618}</MetaDataID>
        public void BeginInit()
        {
            this.init = true;
        }


        /// <summary>
        /// Signals the object that initialization is complete
        /// </summary>
        /// <MetaDataID>{0111BEFC-2D09-48BE-90EA-40A5F85831F7}</MetaDataID>
        public void EndInit()
        {
            this.init = false;

            this.PerformLayout();
        }


        /// <summary>
        /// Gets whether the Table is currently initializing
        /// </summary>
        /// <MetaDataID>{8e1b13b5-67b8-42b3-8288-4971c98618a0}</MetaDataID>
        [Browsable(false)]
        public bool Initializing
        {
            get
            {
                return this.init;
            }
        }

        #endregion

        #region Mouse

        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code
        /// </summary>
        /// <MetaDataID>{BA0EA68A-FCDD-4523-B35B-ED04B7D2A1D3}</MetaDataID>
        public new void ResetMouseEventArgs()
        {
            if (this.trackMouseEvent == null)
            {
                this.trackMouseEvent = new TRACKMOUSEEVENT();
                this.trackMouseEvent.dwFlags = 3;
                this.trackMouseEvent.hwndTrack = base.Handle;
            }

            this.trackMouseEvent.dwHoverTime = this.HoverTime;

            NativeMethods.TrackMouseEvent(this.trackMouseEvent);
        }

        #endregion

        #region Scrolling

        /// <summary>
        /// Updates the scrollbars to reflect any changes made to the Table
        /// </summary>
        /// <MetaDataID>{0ED6C54C-8E3A-4F6E-B16D-D12DF8B989D3}</MetaDataID>
        public void UpdateScrollBars()
        {
            if (!this.Scrollable || this.ColumnModel == null)
            {
                return;
            }

            // fix: Add width/height check as otherwise minimize 
            //      causes a crash
            //      Portia4ever (kangxj@126.com)
            //      13/09/2005
            //      v1.0.1
            if (this.Width == 0 || this.Height == 0)
            {
                return;
            }

            bool vscroll = this.TotalRowAndHeaderHeight > (this.Height - (this.BorderWidth * 2));//- (hscroll ? SystemInformation.HorizontalScrollBarHeight : 0));

            if (vscroll)
            {
                Rectangle vscrollBounds = new Rectangle(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth,
                    this.BorderWidth,
                    SystemInformation.VerticalScrollBarWidth,
                    this.Height - (this.BorderWidth * 2));

                if (ShowLongActionsScroll)//(hscroll)
                {
                    vscrollBounds.Height -= SystemInformation.HorizontalScrollBarHeight;
                }


                this._VScrollBar.Visible = true;
                this._VScrollBar.Bounds = vscrollBounds;
                this._VScrollBar.Minimum = 0;
                if (RowCount > 0 && VisibleRowCount > 0)
                    this._VScrollBar.Maximum = (this.RowCount > this.VisibleRowCount ? this.RowCount - 1 : this.VisibleRowCount);
                this._VScrollBar.SmallChange = 1;

                if (VisibleRowCount > 0)
                    this._VScrollBar.LargeChange = this.VisibleRowCount - 1;

                if (this._VScrollBar.Value > this._VScrollBar.Maximum - this._VScrollBar.LargeChange)
                {
                    this._VScrollBar.Value = this._VScrollBar.Maximum - this._VScrollBar.LargeChange;
                }
            }
            else
            {
                this._VScrollBar.Visible = false;
                this._VScrollBar.Value = 0;
            }
        }


        /// <summary>
        /// Scrolls the contents of the Table vertically to the specified value
        /// </summary>
        /// <param name="value">The value to scroll to</param>
        /// <MetaDataID>{51FA14EB-BC2D-4182-850F-59E618200CA5}</MetaDataID>
        protected void VerticalScroll(int value)
        {
            int scrollVal = this._VScrollBar.Value - value;

            if (scrollVal != 0)
            {
                //if (this.IsEditing)
                //{
                //    this.StopEditing();
                //}

                RECT scrollRect = RECT.FromRectangle(this.CellDataRect);

                Rectangle invalidateRect = scrollRect.ToRectangle();
                Rectangle Rect = scrollRect.ToRectangle();

                scrollVal *= this.RowHeight;

                NativeMethods.ScrollWindow(this.Handle, 0, scrollVal, ref scrollRect, ref scrollRect);

                if (scrollVal < 0)
                {
                    invalidateRect.Y = invalidateRect.Bottom + scrollVal;
                }

                invalidateRect.Height = Math.Abs(scrollVal);

                //this.Invalidate(invalidateRect, false);//old code
                this.Invalidate();
                                
            }
        }


        /// <summary>
        /// Ensures that the Cell at the specified row and column is visible 
        /// within the Table, scrolling the contents of the Table if necessary
        /// </summary>
        /// <param name="row">The zero-based index of the row to scroll into view</param>
        /// <param name="column">The zero-based index of the column to scroll into view</param>
        /// <returns>true if the Table scrolled to the Cell at the specified row 
        /// and column, false otherwise</returns>
        /// <MetaDataID>{91BB0FFE-3B54-4D25-875C-6F0890F05691}</MetaDataID>
        public bool EnsureVisible(int row, int column)
        {
            if (!this.Scrollable || (!this.VScroll) || row == -1)
            {
                return false;
            }
            //   return false;

            if (column == -1)
            {
                if (this.FocusedCell.Column != -1)
                {
                    column = this.FocusedCell.Column;
                }
                else
                {
                    column = 0;
                }
            }

            int vscrollVal = this._VScrollBar.Value;
            bool moved = false;

            //if (this.HScroll)
            //{
            //    if (column < 0)
            //    {
            //        column = 0;
            //    }
            //    else if (column >= this.ColumnCount)
            //    {
            //        column = this.ColumnCount - 1;
            //    }

            //    if (this.ColumnModel.Columns[column].Visible)
            //    {
            //        if (this.ColumnModel.Columns[column].Left < this.hScrollBar.Value)
            //        {
            //            hscrollVal = System.Convert.ToInt32(this.ColumnModel.Columns[column].Left);
            //        }
            //        else if (this.ColumnModel.Columns[column].Right > this.hScrollBar.Value + this.CellDataRect.Width)
            //        {
            //            hscrollVal = System.Convert.ToInt32(this.ColumnModel.Columns[column].Right) - this.CellDataRect.Width;
            //        }

            //        if (hscrollVal > this.hScrollBar.Maximum - this.hScrollBar.LargeChange)
            //        {
            //            hscrollVal = this.hScrollBar.Maximum - this.hScrollBar.LargeChange;
            //        }
            //    }
            //}

            if (this.VScroll)
            {
                if (row < 0)
                {
                    vscrollVal = 0;
                }
                else if (row >= this.RowCount)
                {
                    vscrollVal = this.RowCount - 1;
                }
                else
                {
                    if (row < vscrollVal)
                    {
                        vscrollVal = row;
                    }
                    else if (row > vscrollVal + this._VScrollBar.LargeChange)
                    {
                        vscrollVal += row - (vscrollVal + this._VScrollBar.LargeChange);
                    }
                }

                if (vscrollVal > this._VScrollBar.Maximum - this._VScrollBar.LargeChange)
                {
                    vscrollVal = (this._VScrollBar.Maximum - this._VScrollBar.LargeChange) + 1;
                }
            }

            if (this.RowRect(row).Bottom > this.CellDataRect.Bottom)
            {
                vscrollVal++;
            }

            moved = ( this._VScrollBar.Value != vscrollVal);//this.hScrollBar.Value != hscrollVal ||

            if (moved)
            {
                //this.hScrollBar.Value = hscrollVal;
                this._VScrollBar.Value = vscrollVal;


                this.Invalidate(this.PseudoClientRect);
            }

            return moved;
        }


        /// <summary>
        /// Ensures that the Cell at the specified CellPos is visible within 
        /// the Table, scrolling the contents of the Table if necessary
        /// </summary>
        /// <param name="cellPos">A CellPos that contains the zero-based index 
        /// of the row and column to scroll into view</param>
        /// <returns></returns>
        /// <MetaDataID>{140C416E-7E16-4A32-B63D-3C8798E284C7}</MetaDataID>
        public bool EnsureVisible(CellPos cellPos)
        {
            //return true;
            return this.EnsureVisible(cellPos.Row, cellPos.Column);
        }


        ///// <summary>
        ///// Gets the index of the first visible Column currently displayed in the Table
        ///// </summary>
        //[Browsable(false)]
        //public int FirstVisibleColumn
        //{
        //    get
        //    {
        //        if (this.ColumnModel == null || this.ColumnModel.VisibleColumnCount == 0)
        //        {
        //            return -1;
        //        }

        //        return this.ColumnModel.ColumnIndexAtX(this.hScrollBar.Value);
        //    }
        //}


        ///// <summary>
        ///// Gets the index of the last visible Column currently displayed in the Table
        ///// </summary>
        //[Browsable(false)]
        //public int LastVisibleColumn
        //{
        //    get
        //    {
        //        if (this.ColumnModel == null || this.ColumnModel.VisibleColumnCount == 0)
        //        {
        //            return -1;
        //        }

        //        int rightEdge = this.hScrollBar.Value + this.PseudoClientRect.Right;

        //        if (this.VScroll)
        //        {
        //            rightEdge -= this._VScrollBar.Width;
        //        }

        //        int col = this.ColumnModel.ColumnIndexAtX(rightEdge);

        //        if (col == -1)
        //        {
        //            return this.ColumnModel.PreviousVisibleColumn(this.ColumnModel.Columns.Count);
        //        }
        //        else if (!this.ColumnModel.Columns[col].Visible)
        //        {
        //            return this.ColumnModel.PreviousVisibleColumn(col);
        //        }

        //        return col;
        //    }
        //}

        #endregion

        #region Sorting IN COMMENTS

        ///// <summary>
        ///// Sorts the last sorted column opposite to its current sort order, 
        ///// or sorts the currently focused column in ascending order if no 
        ///// columns have been sorted
        ///// </summary>
        ///// <MetaDataID>{4F9E0A9D-61BE-44F9-9895-09DA291A9370}</MetaDataID>
        //public void Sort()
        //{
        //    this.Sort(true);
        //}


        ///// <summary>
        ///// Sorts the last sorted column opposite to its current sort order, 
        ///// or sorts the currently focused column in ascending order if no 
        ///// columns have been sorted
        ///// </summary>
        ///// <param name="stable">Specifies whether a stable sorting method 
        ///// should be used to sort the column</param>
        ///// <MetaDataID>{61C741F1-D4AB-4C44-A0E5-97F8A7D5E6C2}</MetaDataID>
        //public void Sort(bool stable)
        //{
        //    // don't allow sorting if we're being used as a 
        //    // preview table in a ColumnModel editor
        //    if (this.Preview)
        //    {
        //        return;
        //    }

        //    // if we don't have a sorted column already, check if 
        //    // we can use the column of the cell that has focus
        //    if (!this.IsValidColumn(this.lastSortedColumn))
        //    {
        //        if (this.IsValidColumn(this.focusedCell.Column))
        //        {
        //            this.lastSortedColumn = this.focusedCell.Column;
        //        }
        //    }

        //    // make sure the last sorted column exists
        //    if (this.IsValidColumn(this.lastSortedColumn))
        //    {
        //        // don't bother if the column won't let us sort
        //        if (!this.ColumnModel.Columns[this.lastSortedColumn].Sortable)
        //        {
        //            return;
        //        }

        //        // work out which direction we should sort
        //        SortOrder newOrder = SortOrder.Ascending;

        //        Column column = this.ColumnModel.Columns[this.lastSortedColumn];

        //        if (column.SortOrder == SortOrder.Ascending)
        //        {
        //            newOrder = SortOrder.Descending;
        //        }

        //        this.Sort(this.lastSortedColumn, column, newOrder, stable);
        //    }
        //}


        ///// <summary>
        ///// Sorts the specified column opposite to its current sort order, 
        ///// or in ascending order if the column is not sorted
        ///// </summary>
        ///// <param name="column">The index of the column to sort</param>
        ///// <MetaDataID>{184CCD6C-D2BE-4681-820A-086DDDEE1DAA}</MetaDataID>
        //public void Sort(int column)
        //{
        //    this.Sort(column, true);
        //}


        ///// <summary>
        ///// Sorts the specified column opposite to its current sort order, 
        ///// or in ascending order if the column is not sorted
        ///// </summary>
        ///// <param name="column">The index of the column to sort</param>
        ///// <param name="stable">Specifies whether a stable sorting method 
        ///// should be used to sort the column</param>
        ///// <MetaDataID>{864B9F65-1F8A-4C06-A877-BFF3C43415E3}</MetaDataID>
        //public void Sort(int column, bool stable)
        //{
        //    // don't allow sorting if we're being used as a 
        //    // preview table in a ColumnModel editor
        //    if (this.Preview)
        //    {
        //        return;
        //    }

        //    // make sure the column exists
        //    if (this.IsValidColumn(column))
        //    {
        //        // don't bother if the column won't let us sort
        //        if (!this.ColumnModel.Columns[column].Sortable)
        //        {
        //            return;
        //        }

        //        // if we already have a different sorted column, set 
        //        // its sort order to none
        //        if (column != this.lastSortedColumn)
        //        {
        //            if (this.IsValidColumn(this.lastSortedColumn))
        //            {
        //                this.ColumnModel.Columns[this.lastSortedColumn].InternalSortOrder = SortOrder.None;
        //            }
        //        }

        //        this.lastSortedColumn = column;

        //        // work out which direction we should sort
        //        SortOrder newOrder = SortOrder.Ascending;

        //        Column col = this.ColumnModel.Columns[column];

        //        if (col.SortOrder == SortOrder.Ascending)
        //        {
        //            newOrder = SortOrder.Descending;
        //        }

        //        this.Sort(column, col, newOrder, stable);
        //    }
        //}


        ///// <summary>
        ///// Sorts the specified column in the specified sort direction
        ///// </summary>
        ///// <param name="column">The index of the column to sort</param>
        ///// <param name="sortOrder">The direction the column is to be sorted</param>
        ///// <MetaDataID>{6DACE91E-6676-4336-844E-8CB81183A204}</MetaDataID>
        //public void Sort(int column, SortOrder sortOrder)
        //{
        //    this.Sort(column, sortOrder, true);
        //}


        ///// <summary>
        ///// Sorts the specified column in the specified sort direction
        ///// </summary>
        ///// <param name="column">The index of the column to sort</param>
        ///// <param name="sortOrder">The direction the column is to be sorted</param>
        ///// <param name="stable">Specifies whether a stable sorting method 
        ///// should be used to sort the column</param>
        ///// <MetaDataID>{729B2136-3D5F-4356-9352-47A3CD7277F1}</MetaDataID>
        //public void Sort(int column, SortOrder sortOrder, bool stable)
        //{
        //    // don't allow sorting if we're being used as a 
        //    // preview table in a ColumnModel editor
        //    if (this.Preview)
        //    {
        //        return;
        //    }

        //    // make sure the column exists
        //    if (this.IsValidColumn(column))
        //    {
        //        // don't bother if the column won't let us sort
        //        if (!this.ColumnModel.Columns[column].Sortable)
        //        {
        //            return;
        //        }

        //        // if we already have a different sorted column, set 
        //        // its sort order to none
        //        if (column != this.lastSortedColumn)
        //        {
        //            if (this.IsValidColumn(this.lastSortedColumn))
        //            {
        //                this.ColumnModel.Columns[this.lastSortedColumn].InternalSortOrder = SortOrder.None;
        //            }
        //        }

        //        this.lastSortedColumn = column;

        //        this.Sort(column, this.ColumnModel.Columns[column], sortOrder, stable);
        //    }
        //}


        ///// <summary>
        ///// Sorts the specified column in the specified sort direction
        ///// </summary>
        ///// <param name="index">The index of the column to sort</param>
        ///// <param name="column">The column to sort</param>
        ///// <param name="sortOrder">The direction the column is to be sorted</param>
        ///// <param name="stable">Specifies whether a stable sorting method 
        ///// should be used to sort the column</param>
        ///// <MetaDataID>{3EBEDD04-7DCC-40C3-941B-F4AEDCB93894}</MetaDataID>
        //private void Sort(int index, Column column, SortOrder sortOrder, bool stable)
        //{
        //    // make sure a null comparer type doesn't sneak past

        //    ComparerBase comparer = null;

        //    if (column.Comparer != null)
        //    {
        //        comparer = (ComparerBase)Activator.CreateInstance(column.Comparer, new object[] { this.TableModel, index, sortOrder });
        //    }
        //    else if (column.DefaultComparerType != null)
        //    {
        //        comparer = (ComparerBase)Activator.CreateInstance(column.DefaultComparerType, new object[] { this.TableModel, index, sortOrder });
        //    }
        //    else
        //    {
        //        return;
        //    }

        //    column.InternalSortOrder = sortOrder;

        //    // create the comparer
        //    SorterBase sorter = null;

        //    // work out which sort method to use.
        //    // - InsertionSort/MergeSort are stable sorts, 
        //    //   whereas ShellSort/HeapSort are unstable
        //    // - InsertionSort/ShellSort are faster than 
        //    //   MergeSort/HeapSort on small lists and slower 
        //    //   on large lists
        //    // so we choose based on the size of the list and
        //    // whether the user wants a stable sort
        //    if (this.TableModel.TotalRows.Count < 1000)
        //    {
        //        if (stable)
        //        {
        //            sorter = new InsertionSorter(this.TableModel, index, comparer, sortOrder);
        //        }
        //        else
        //        {
        //            sorter = new ShellSorter(this.TableModel, index, comparer, sortOrder);
        //        }
        //    }
        //    else
        //    {
        //        if (stable)
        //        {
        //            sorter = new MergeSorter(this.TableModel, index, comparer, sortOrder);
        //        }
        //        else
        //        {
        //            sorter = new HeapSorter(this.TableModel, index, comparer, sortOrder);
        //        }
        //    }

        //    // don't let the table redraw
        //    this.BeginUpdate();

        //    this.OnBeginSort(new ColumnEventArgs(column, index, ColumnEventType.Sorting, null));

        //    sorter.Sort();

        //    this.OnEndSort(new ColumnEventArgs(column, index, ColumnEventType.Sorting, null));

        //    // redraw any changes
        //    this.EndUpdate();
        //}


        /// <summary>
        /// Returns whether a Column exists at the specified index in the 
        /// Table's ColumnModel
        /// </summary>
        /// <param name="column">The index of the column to check</param>
        /// <returns>True if a Column exists at the specified index in the 
        /// Table's ColumnModel, false otherwise</returns>
        /// <MetaDataID>{E7CB4C06-D82C-47C0-8257-6C7FB9B06DBF}</MetaDataID>
        public bool IsValidColumn(int column)
        {
            if (this.ColumnModel == null)
            {
                return false;
            }

            return (column >= 1 && column < this.ColumnModel.Columns.Count);
        }

        #endregion

        #endregion

        #region Properties


        /// <MetaDataID>{a151ab94-6b8d-40c6-973a-57e3671ad42d}</MetaDataID>
        private int _RowHeight = TableModel.DefaultRowHeight;
        /// <summary>
        /// Gets or sets the height of each Row in the TableModel
        /// </summary>
        /// <MetaDataID>{c29c9877-8703-461d-80ed-6ebb7c93a445}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RowHeight
        {
            get
            {
                return this._RowHeight;
            }

            set
            {
                if (value < TableModel.MinimumRowHeight)
                {
                    value = TableModel.MinimumRowHeight;
                }
                else if (value > TableModel.MaximumRowHeight)
                {
                    value = TableModel.MaximumRowHeight;
                }

                if (this._RowHeight != value)
                {
                    this._RowHeight = value;

                    this.OnRowHeightChanged(EventArgs.Empty);
                }
            }
        }






        #region Borders

        /// <summary>
        /// Gets or sets the border style for the Table
        /// </summary>
        /// <MetaDataID>{f0353c0d-0cf6-4de5-96e8-756561756c58}</MetaDataID>
        //[Category("Appearance"),
        //DefaultValue(BorderStyle.FixedSingle),
        //Description("Indicates the border style for the Table")]
        [Browsable(false)]
        public BorderStyle BorderStyle
        {
            get
            {
                return this.borderStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(BorderStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
                }

                if (borderStyle != value)
                {
                    this.borderStyle = value;

                    this.Invalidate(true);
                }
            }
        }


        /// <summary>
        /// Gets the width of the Tables border
        /// </summary>
        /// <MetaDataID>{f0c25c4a-16df-4868-a468-8aa3837b5852}</MetaDataID>
        internal int BorderWidth
        {
            get
            {
                if (this.BorderStyle == BorderStyle.Fixed3D)
                {
                    return SystemInformation.Border3DSize.Width;
                }
                else if (this.BorderStyle == BorderStyle.FixedSingle)
                {
                    return 1;
                }

                return 0;
            }
        }

        #endregion

        #region Cells

        /// <summary>
        /// Gets the last known cell position that the mouse was over
        /// </summary>
        /// <MetaDataID>{a8d08341-2c8a-4b8a-bccc-e95e28b15db4}</MetaDataID>
        [Browsable(false)]
        public CellPos LastMouseCell
        {
            get
            {
                return this.lastMouseCell;
            }
        }


        /// <summary>
        /// Gets the last known cell position that the mouse's left 
        /// button was pressed in
        /// </summary>
        /// <MetaDataID>{e864356a-54fc-4da4-8cd8-7e307d4626c6}</MetaDataID>
        [Browsable(false)]
        public CellPos LastMouseDownCell
        {
            get
            {
                return this.lastMouseDownCell;
            }
        }

        /// <MetaDataID>{67336dff-b014-4d3b-b588-42183e229469}</MetaDataID>
        [Browsable(false)]
        public CellPos LastMouseClickCell
        {
            get
            {
                return this.lastMouseClickCell;
            }
        }


        /// <summary>
        /// Gets or sets the position of the Cell that currently has focus
        /// </summary>
        /// <MetaDataID>{ce4dc37e-e20a-4be8-b624-d457795dd488}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CellPos FocusedCell
        {
            get
            {
                return this.focusedCell;
            }

            set
            {
                if (!this.IsValidCell(value))
                {
                    return;
                }

                if (!this.TableModel[value].Enabled)
                {
                    return;
                }

                if (this.focusedCell != value)
                {
                    if (!this.focusedCell.IsEmpty)
                    {
                        this.RaiseCellLostFocus(this.focusedCell);
                    }

                    this.focusedCell = value;

                    if (!value.IsEmpty)
                    {
                        this.EnsureVisible(value);

                        this.RaiseCellGotFocus(value);
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the amount of time (in milliseconds) that that the 
        /// mouse pointer must hover over a Cell or Column Header before 
        /// a MouseHover event is raised
        /// </summary>
        /// <MetaDataID>{77e7dbee-92b4-4830-bfbd-8484f28e2c9c}</MetaDataID>
        [Category("Behavior"),
        DefaultValue(1000),
        Description("The amount of time (in milliseconds) that that the mouse pointer must hover over a Cell or Column Header before a MouseHover event is raised")]
        public int HoverTime
        {
            get
            {
                return this.hoverTime;
            }

            set
            {
                if (value < 100)
                {
                    throw new ArgumentException("HoverTime cannot be less than 100", "value");
                }

                if (this.hoverTime != value)
                {
                    this.hoverTime = value;

                    this.ResetMouseEventArgs();
                }
            }
        }

        #endregion

        //[Browsable(false)]
        //public Rectangle HScrollRectangle
        //{
        //    get
        //    {
        //        Rectangle clientRect = new Rectangle(this.InternalBorderRect.X + 80, this.InternalBorderRect.Y, this.InternalBorderRect.Width, this.InternalBorderRect.Height);
        //        if (this.HScroll)
        //        {
        //            clientRect.Height -= SystemInformation.HorizontalScrollBarHeight;
        //        }

        //        if (this.VScroll)
        //        {
        //            clientRect.Width -= SystemInformation.VerticalScrollBarWidth;
        //        }

        //        return clientRect;
        //    }
        //}

        #region ClientRectangle

        /// <summary>
        /// Gets the rectangle that represents the "client area" of the control.
        /// (The rectangle excludes the borders and scrollbars)
        /// </summary>
        /// <MetaDataID>{8de71b96-451d-4c8f-ad2f-cb0218d75290}</MetaDataID>
        [Browsable(false)]
        public Rectangle PseudoClientRect
        {
            get
            {
                Rectangle clientRect = this.InternalBorderRect;
                //clientRect.Width -= 80;
                //if (this.HScroll)
                //{
                //    clientRect.Height -= SystemInformation.HorizontalScrollBarHeight;
                //}

                if (this.VScroll)
                {
                    clientRect.Width -= SystemInformation.VerticalScrollBarWidth;
                }
                else
                {
                    //Sometimes when  the control shows for the first time the HScrollBar.width does not substracted 
                    //fro the width beacuse stays visible==false even though the following statement in the 
                    //UpdateScrollBars function is true so correct it here 
                    bool vscroll = this.TotalRowAndHeaderHeight > (this.Height - (this.BorderWidth * 2));//- (hscroll ? SystemInformation.HorizontalScrollBarHeight : 0));
                    if(vscroll)
                        clientRect.Width -= SystemInformation.VerticalScrollBarWidth;
                }

                return clientRect;
            }
        }


        /// <summary>
        /// Gets the rectangle that represents the "cell data area" of the control.
        /// (The rectangle excludes the borders, column headers and scrollbars)
        /// </summary>
        /// <MetaDataID>{626a8cd8-a1c7-4ae1-9e94-554d69a7d960}</MetaDataID>
        [Browsable(false)]
        public Rectangle CellDataRect
        {
            get
            {
                Rectangle clientRect = this.PseudoClientRect;
                //clientRect.X += 80;
                //clientRect.Width -= 80;
                if (this.ColumnCount > 0)//this.HeaderStyle != ColumnHeaderStyle.None && 
                {
                    clientRect.Y += this.LongActionsRectHeight+this.HeaderHeight+1;
                    clientRect.Height -= this.LongActionsRectHeight + this.HeaderHeight + 1; 
                }
                if (ShowLongActionsScroll)
                {
                    clientRect.Height -= SystemInformation.HorizontalScrollBarHeight;
                }

                return clientRect;
            }
        }


        /// <summary></summary>
        /// <MetaDataID>{652a50f7-619f-43f6-aaf7-891b92079b18}</MetaDataID>
        private Rectangle InternalBorderRect
        {
            get
            {
                //Debug.WriteLine("InternalBorderRect="+this.Width.ToString());
                return new Rectangle(this.BorderWidth,
                    this.BorderWidth,
                    this.Width - (this.BorderWidth * 2),
                    this.Height - (this.BorderWidth * 2));
            }
        }

        #endregion

        #region ColumnModel

        /// <summary>
        /// Gets or sets the ColumnModel that contains all the Columns
        /// displayed in the Table
        /// </summary>
        /// <MetaDataID>{c5ebe5ad-b12a-45de-8d72-eb1453171bc2}</MetaDataID>
        [Browsable(false)]
        public DayTimeColumnModel ColumnModel
        {
            get
            {
                if (this._ColumnModel == null)
                {
                    ColumnModel = new DayTimeColumnModel();
                }
                return this._ColumnModel;
            }

            set
            {
                if (this._ColumnModel != value)
                {
                    if (this._ColumnModel != null && this._ColumnModel.Table == this)
                    {
                        this._ColumnModel.InternalTable = null;
                    }

                    this._ColumnModel = value;

                    if (value != null)
                    {
                        value.InternalTable = this;
                    }

                    this.OnColumnModelChanged(EventArgs.Empty);
                }
            }
        }


        ///// <summary>
        ///// Gets or sets whether the Table allows users to resize Column widths
        ///// </summary>
        //[Category("Columns"),
        //DefaultValue(true),
        //Description("Specifies whether the Table allows users to resize Column widths")]
        //public bool ColumnResizing
        //{
        //    get
        //    {
        //        return this.columnResizing;
        //    }

        //    set
        //    {
        //        if (this.columnResizing != value)
        //        {
        //            this.columnResizing = value;
        //        }
        //    }
        //}


        /// <summary>
        /// Returns the number of Columns in the Table
        /// </summary>
        /// <MetaDataID>{a2734961-cfa8-4062-9d01-e88b66f3f29d}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ColumnCount
        {
            get
            {
                if (this.ColumnModel == null)
                {
                    return -1;
                }

                return this.ColumnModel.Columns.Count;
            }
        }


        ///// <summary>
        ///// Returns the index of the currently sorted Column
        ///// </summary>
        //[Browsable(false),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public int SortingColumn
        //{
        //    get
        //    {
        //        return this.lastSortedColumn;
        //    }
        //}


        ///// <summary>
        ///// Gets or sets the background Color for the currently sorted column
        ///// </summary>
        //[Category("Columns"),
        //Description("The background Color for a sorted Column")]
        //public Color SortedColumnBackColor
        //{
        //    get
        //    {
        //        return this.sortedColumnBackColor;
        //    }

        //    set
        //    {
        //        if (this.sortedColumnBackColor != value)
        //        {
        //            this.sortedColumnBackColor = value;

        //            if (this.IsValidColumn(this.lastSortedColumn))
        //            {
        //                Rectangle columnRect = this.ColumnRect(this.lastSortedColumn);

        //                if (this.PseudoClientRect.IntersectsWith(columnRect))
        //                {
        //                    this.Invalidate(Rectangle.Intersect(this.PseudoClientRect, columnRect));
        //                }
        //            }
        //        }
        //    }
        //}


        ///// <summary>
        ///// Specifies whether the Table's SortedColumnBackColor property 
        ///// should be serialized at design time
        ///// </summary>
        ///// <returns>True if the SortedColumnBackColor property should be 
        ///// serialized, False otherwise</returns>
        ///// <MetaDataID>{3F60CFB4-778F-499A-BC33-7A09C217620B}</MetaDataID>
        //private bool ShouldSerializeSortedColumnBackColor()
        //{
        //    return this.sortedColumnBackColor != Color.WhiteSmoke;
        //}

        #endregion

        #region DisplayRectangle

        /// <summary>
        /// Gets the rectangle that represents the display area of the Table
        /// </summary>
        /// <MetaDataID>{d5d87a9e-f91d-4d6a-8415-b509c97e161d}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                //Debug.WriteLine("CellRect=" + this.CellDataRect.Width.ToString());
                Rectangle displayRect = this.CellDataRect;

                if (!this.init)
                {
                    //displayRect.X -= this.hScrollBar.Value;
                    displayRect.Y -= this._VScrollBar.Value;
                }
                //Substracted from the pseudorectangle
                //if (this.VScroll)
                //    displayRect.Width -= this.VScrollBar.Width;

                if (this.ColumnModel == null)
                {
                    return displayRect;
                }

                //displayRect.Width = this.CellDataRect.Width;
                //displayRect.Height = this.CellDataRect.Height;

                //if (this.ColumnModel.TotalColumnWidth <= this.CellDataRect.Width)
                //{
                //    displayRect.Width = this.CellDataRect.Width;
                //}
                //else
                //{
                //    displayRect.Width = System.Convert.ToInt32(this.ColumnModel.VisibleColumnsWidth);
                //}

                //if (this.TotalRowHeight <= this.CellDataRect.Height)
                //{
                //    displayRect.Height = this.CellDataRect.Height;
                //}
                //else
                //{
                //    displayRect.Height = this.TotalRowHeight;
                //}

                return displayRect;
            }
        }

        #endregion

        #region Editing

        /// <summary>
        /// Gets whether the Table is currently editing a Cell
        /// </summary>
        /// <MetaDataID>{da8bfd95-2fd8-4795-a731-3548f37bbfad}</MetaDataID>
        [Browsable(false)]
        public bool IsEditing
        {
            get
            {
                return !this.EditingCell.IsEmpty;
            }
        }


        /// <summary>
        /// Gets a CellPos that specifies the position of the Cell that 
        /// is currently being edited
        /// </summary>
        /// <MetaDataID>{7c4dffc3-1b81-4c8c-9365-9cc3f9e793b2}</MetaDataID>
        [Browsable(false)]
        public CellPos EditingCell
        {
            get
            {
                return this.editingCell;
            }
        }


        /// <summary>
        /// Gets the ICellEditor that is currently being used to edit a Cell
        /// </summary>
        /// <MetaDataID>{49038e4a-ea84-4844-8e9c-01192eaa34ec}</MetaDataID>
        [Browsable(false)]
        public ICellEditor EditingCellEditor
        {
            get
            {
                return this.curentCellEditor;
            }
        }


        ///// <summary>
        ///// Gets or sets the action that causes editing to be initiated
        ///// </summary>
        //[Category("Editing"),
        //DefaultValue(EditStartAction.DoubleClick),
        //Description("The action that causes editing to be initiated")]
        //public EditStartAction EditStartAction
        //{
        //    get
        //    {
        //        return this.editStartAction;
        //    }

        //    set
        //    {
        //        if (!Enum.IsDefined(typeof(EditStartAction), value))
        //        {
        //            throw new InvalidEnumArgumentException("value", (int)value, typeof(EditStartAction));
        //        }

        //        if (this.editStartAction != value)
        //        {
        //            this.editStartAction = value;
        //        }
        //    }
        //}


        ///// <summary>
        ///// Gets or sets the custom key used to initiate Cell editing
        ///// </summary>
        //[Category("Editing"),
        //DefaultValue(Keys.F5),
        //Description("The custom key used to initiate Cell editing")]
        //public Keys CustomEditKey
        //{
        //    get
        //    {
        //        return this.customEditKey;
        //    }

        //    set
        //    {
        //        if (this.IsReservedKey(value))
        //        {
        //            throw new ArgumentException("CustomEditKey cannot be one of the Table's reserved keys " +
        //                "(Up arrow, Down arrow, Left arrow, Right arrow, PageUp, " +
        //                "PageDown, Home, End, Tab)", "value");
        //        }

        //        if (this.customEditKey != value)
        //        {
        //            this.customEditKey = value;
        //        }
        //    }
        //}


        /*/// <summary>
        /// Gets or sets whether pressing the Tab key during editing moves
        /// the editor to the next editable Cell
        /// </summary>
        [Category("Editing"),
        DefaultValue(true),
        Description("")]
        public bool TabMovesEditor
        {
            get
            {	
                return this.tabMovesEditor;
            }

            set
            {
                this.tabMovesEditor = value;
            }
        }*/

        #endregion

        #region Grid

        /// <summary>
        /// Gets or sets how grid lines are displayed around TotalRows and columns
        /// </summary>
        /// <MetaDataID>{e18ae308-eaa8-455a-9136-37055b1db14f}</MetaDataID>
        [Category("Grid"),
        DefaultValue(GridLines.None),
        Description("Determines how grid lines are displayed around TotalRows and columns")]
        public GridLines GridLines
        {
            get
            {
                return this._GridLines;
            }

            set
            {
                if (!Enum.IsDefined(typeof(GridLines), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(GridLines));
                }

                if (this._GridLines != value)
                {
                    this._GridLines = value;

                    this.Invalidate(this.PseudoClientRect, false);
                }
            }
        }


        /// <summary>
        /// Gets or sets the style of the lines used to draw the grid
        /// </summary>
        /// <MetaDataID>{3e838a7c-68d7-4648-9032-cbae6278968d}</MetaDataID>
        [Category("Grid"),
        DefaultValue(GridLineStyle.Solid),
        Description("The style of the lines used to draw the grid")]
        public GridLineStyle GridLineStyle
        {
            get
            {
                return this.gridLineStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(GridLineStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(GridLineStyle));
                }

                if (this.gridLineStyle != value)
                {
                    this.gridLineStyle = value;

                    if (this.GridLines != GridLines.None)
                    {
                        this.Invalidate(this.PseudoClientRect, false);
                    }
                }
            }
        }

        /// <MetaDataID>{1c46420a-026b-46ca-a027-eed11e0eea7b}</MetaDataID>
        private Color _TimeRowGridColor;
        /// <MetaDataID>{02fbb26a-4d13-4a9d-9f11-d22cdbd42064}</MetaDataID>
        private Color _ColumnGridColor;
        /// <summary>
        /// Gets or sets the Color of the grid lines
        /// </summary>
        /// <MetaDataID>{716120ca-9a2a-43b6-ae0b-550f16aaf9d8}</MetaDataID>
        [Category("Grid"), Description("The color of the grid column")]
        public Color ColumnGridColor
        {
            get
            {
                return this._ColumnGridColor;
            }

            set
            {
                if (this._ColumnGridColor != value)
                {
                    this._ColumnGridColor = value;
                    this._TimeRowGridColor = value;
                    if (this.GridLines != GridLines.None)
                    {
                        this.Invalidate(this.PseudoClientRect, false);
                    }
                }
            }
        }

        /// <MetaDataID>{380766bd-6f57-4dd3-a89a-26c4dc24a181}</MetaDataID>
        private Color _RowGridColor;
        /// <summary>
        /// Gets or sets the Color of the grid lines
        /// </summary>
        /// <MetaDataID>{b50d4aba-a8cf-48e2-9b55-b668c2922b15}</MetaDataID>
        [Category("Grid"), Description("The color of the grid row")]
        public Color RowGridColor
        {
            get
            {
                return this._RowGridColor;
            }

            set
            {
                if (this._RowGridColor != value)
                {
                    this._RowGridColor = value;

                    if (this.GridLines != GridLines.None)
                    {
                        this.Invalidate(this.PseudoClientRect, false);
                    }
                }
            }
        }

        /// <summary>
        /// Specifies whether the Table's GridColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the GridColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{CB3C1069-916D-485C-AC1D-D5A0D507E0FB}</MetaDataID>
        private bool ShouldSerializeGridColor()
        {
            return (this.ColumnGridColor != SystemColors.Control && this.RowGridColor != SystemColors.Control);
        }


        /// <summary></summary>
        /// <MetaDataID>{4fbc93bf-af5e-48ab-8ad6-b938f237e01f}</MetaDataID>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                base.BackColor = value;
            }
        }


        /// <MetaDataID>{d3766420-9194-48a9-b878-60d1aa60c46b}</MetaDataID>
        Color _BackColor;
        /// <MetaDataID>{364f648a-ee23-4403-b199-6bb3c5c8bb0c}</MetaDataID>
        Color _FocusedBackColor;

        /// <MetaDataID>{0cfdb4f2-67df-4228-a4a1-f0a86ea3831a}</MetaDataID>
        public Color FocusedBackColor
        {
            get
            {

                return _FocusedBackColor;
            }

            set
            {
                _FocusedBackColor = value;
            }
        }


        /// <summary>
        /// Specifies whether the Table's BackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the BackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{7F292E73-8607-4733-BF2C-88FC3FD4CF6D}</MetaDataID>
        private bool ShouldSerializeBackColor()
        {
            return (this.BackColor != Color.White);
        }

        #endregion

        #region Header

        /// <summary>
        /// Gets or sets the column header style
        /// </summary>
        //[Category("Columns"),
        //DefaultValue(ColumnHeaderStyle.Clickable),
        //Description("The style of the column headers")]
        //[Browsable(false)]
        //public ColumnHeaderStyle HeaderStyle
        //{
        //    get
        //    {
        //        return this.headerStyle;
        //    }

        //    set
        //    {
        //        if (!Enum.IsDefined(typeof(ColumnHeaderStyle), value))
        //        {
        //            throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnHeaderStyle));
        //        }

        //        if (this.headerStyle != value)
        //        {
        //            this.headerStyle = value;

        //            this.pressedColumn = -1;
        //            this.hotColumn = -1;

        //            this.Invalidate();
        //        }
        //    }
        //}


        /// <summary>
        /// Gets the height of the column headers
        /// </summary>
        /// <MetaDataID>{72953df0-3517-4101-91c6-8d40a271167f}</MetaDataID>
        [Browsable(false)]
        public int HeaderHeight
        {
            get
            {
                if (this.ColumnModel == null)// || this.HeaderStyle == ColumnHeaderStyle.None)
                {
                    return 0;
                }

                return this.ColumnModel.HeaderHeight;
            }
        }


        /// <summary>
        /// Gets a Rectangle that specifies the size and location of 
        /// the Table's column header area
        /// </summary>
        /// <MetaDataID>{1c2e8569-6dbb-4786-bd58-69fed662c925}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle HeaderRectangle
        {
            get
            {
                return new Rectangle(this.BorderWidth, this.BorderWidth, this.PseudoClientRect.Width, this.HeaderHeight*2);
            }
        }


        /// <summary>
        /// Gets or sets the font used to draw the text in the column headers
        /// </summary>
        /// <MetaDataID>{e636a3df-1482-4b99-a404-891b1869a28c}</MetaDataID>
        [Category("Columns"),
        Description("The font used to draw the text in the column headers")]
        public Font HeaderFont
        {
            get
            {
                return this.headerFont;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("HeaderFont cannot be null");
                }

                if (this.headerFont != value)
                {
                    this.headerFont = value;

                    this.HeaderRenderer.Font = value;

                    this.Invalidate(this.HeaderRectangle, false);
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's HeaderFont property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the HeaderFont property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{D8EAD87B-8B92-4FDE-A67A-EC1E2357DDB8}</MetaDataID>
        private bool ShouldSerializeHeaderFont()
        {
            return this.HeaderFont != this.Font;
        }
        /// <MetaDataID>{5A987133-4D57-4C40-BF2E-ABF7E6BBEBD8}</MetaDataID>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
        }

        /// <MetaDataID>{0beae892-b407-448d-a7fd-668ed20b9dc4}</MetaDataID>
        private LongActionsRectRenderer _LongActionsRectRenderer;
        /// <MetaDataID>{cb1429c5-566a-4e9c-8a0b-ee536aad6889}</MetaDataID>
        private int _LongActionsRectHeight = 0;
        /// <MetaDataID>{b861c39d-0f12-4324-b609-6621c78c34f0}</MetaDataID>
        internal int LongActionsRectHeight
        {
            get
            {
                return _LongActionsRectHeight;
            }
            set
            {
                if (_LongActionsRectHeight != value)
                {
                    _LongActionsRectHeight = value;
                    this.UpdateScrollBars();
                }
            }
        }

        /// <MetaDataID>{9d8110a6-7705-423f-b3df-ce606f3eb7d7}</MetaDataID>
        private int _MinLongActionsRectHeight = 23;

        #region public HeaderRenderer HeaderRenderer
        /// <MetaDataID>{25b97bd1-d347-4ec8-8904-e4bd0a7e88d4}</MetaDataID>
        private HeaderRenderer _HeaderRenderer;
        /// <summary>
        /// Gets or sets the HeaderRenderer used to draw the Column headers
        /// </summary>
        /// <MetaDataID>{c93a7955-99ca-436d-916b-d3550072d3c2}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeaderRenderer HeaderRenderer
        {
            get
            {
                if (this._HeaderRenderer == null)
                {
                    this.HeaderRenderer = new XPHeaderRenderer();
                }

                return this._HeaderRenderer;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("HeaderRenderer cannot be null");
                }

                if (this._HeaderRenderer != value)
                {
                    this._HeaderRenderer = value;
                    this._HeaderRenderer.Font = this.HeaderFont;

                    this.Invalidate(this.HeaderRectangle, false);
                }
            }
        }
        
        #endregion
       

        #endregion

        #region HeaderContextmenu IN COMMENTS
        ///// <summary>
        ///// Gets the ContextMenu used for Column Headers
        ///// </summary>
        //[Browsable(false)]
        //public HeaderContextMenu HeaderContextMenu
        //{
        //    get
        //    {
        //        return this.headerContextMenu;
        //    }
        //}


        ///// <summary>
        ///// Gets or sets whether the HeaderContextMenu is able to be 
        ///// displayed when the user right clicks on a Column Header
        ///// </summary>
        //[Category("Columns"),
        //DefaultValue(true),
        //Description("Indicates whether the HeaderContextMenu is able to be displayed when the user right clicks on a Column Header")]
        //public bool EnableHeaderContextMenu
        //{
        //    get
        //    {
        //        return this.HeaderContextMenu.Enabled;
        //    }

        //    set
        //    {
        //        this.HeaderContextMenu.Enabled = value;
        //    }
        //} 
        #endregion

        #region TotalRows



        /// <summary>
        /// Gets the combined height of all the TotalRows in the Table
        /// </summary>
        /// <MetaDataID>{4ef1df9d-458a-41ba-872c-0ff9f96abbed}</MetaDataID>
        [Browsable(false)]
        protected int TotalRowHeight
        {
            get
            {
                if (this.TableModel == null)
                {
                    return 0;
                }

                return this.TableModel.TotalRowHeight;
            }
        }


        /// <summary>
        /// Gets the combined height of all the TotalRows in the Table 
        /// plus the height of the column headers
        /// </summary>
        /// <MetaDataID>{2bba58c2-6aed-48d3-b326-90685471cb01}</MetaDataID>
        [Browsable(false)]
        protected int TotalRowAndHeaderHeight
        {
            get
            {
                return this.TotalRowHeight + this.HeaderHeight;
            }
        }


        /// <summary>
        /// Returns the number of TotalRows in the Table
        /// </summary>
        /// <MetaDataID>{3fa39a34-1ebc-4ef0-810c-b992db8859fc}</MetaDataID>
        [Browsable(false)]
        public int RowCount
        {
            get
            {
                if (this.TableModel == null)
                {
                    return 0;
                }

                return this.TableModel.TotalRows.Count;
            }
        }


        /// <summary>
        /// Gets the number of TotalRows that are visible in the Table
        /// </summary>
        /// <MetaDataID>{ad2084bf-e3f4-403a-b50d-3b899fdb347d}</MetaDataID>
        [Browsable(false)]
        public int VisibleRowCount
        {
            get
            {
                int count = this.CellDataRect.Height / this.RowHeight;

                if ((this.CellDataRect.Height % this.RowHeight) > 0)
                {
                    count++;
                }

                return count;
            }
        }


        /// <summary>
        /// Gets the index of the first visible row in the Table
        /// </summary>
        /// <MetaDataID>{206099d8-dfea-4458-a7bf-aea561e16245}</MetaDataID>
        [Browsable(false)]
        public int TopIndex
        {
            get
            {
                if (this.TableModel == null || this.TableModel.TotalRows.Count == 0)
                {
                    return -1;
                }

                if (this.VScroll)
                {
                    return this._VScrollBar.Value;
                }

                return 0;
            }
        }


        /// <summary>
        /// Gets the first visible row in the Table
        /// </summary>
        /// <MetaDataID>{bfce705b-2e44-47dc-9db8-d872e1a24090}</MetaDataID>
        [Browsable(false)]
        public Row TopItem
        {
            get
            {
                if (this.TableModel == null || this.TableModel.TotalRows.Count == 0)
                {
                    return null;
                }

                return this.TableModel.TotalRows[this.TopIndex];
            }
        }


        /// <summary>
        /// Gets or sets the background color of odd-numbered TotalRows in the Table
        /// </summary>
        /// <MetaDataID>{d14d7837-ca4d-4524-a866-14560814d679}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(typeof(Color), "Transparent"),
        Description("The background color of odd-numbered TotalRows in the Table")]
        public Color AlternatingRowColor
        {
            get
            {
                return this.alternatingRowColor;
            }

            set
            {
                if (this.alternatingRowColor != value)
                {
                    this.alternatingRowColor = value;

                    this.Invalidate(this.CellDataRect, false);
                }
            }
        }

        #endregion

        #region Scrolling

        /// <summary>
        /// Gets or sets a value indicating whether the Table will 
        /// allow the user to scroll to any columns or TotalRows placed 
        /// outside of its visible boundaries
        /// </summary>
        /// <MetaDataID>{99ab54c5-846a-4f33-9e9e-d27d28f33e42}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Scrollable
        {
            get
            {
                return this.scrollable;
            }

            set
            {
                if (this.scrollable != value)
                {
                    this.scrollable = value;

                    this.PerformLayout();
                }
            }
        }


        /// <summary>
        /// Gets a value indicating whether the vertical 
        /// scroll bar is visible
        /// </summary>
        /// <MetaDataID>{cce57fab-6c1b-4bd4-a18a-3881ac6aa910}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool VScroll
        {
            get
            {
                if (this._VScrollBar == null)
                {
                    return false;
                }

                return this._VScrollBar.Visible;
            }
        }

        #endregion

        #region Selection

        /// <summary>
        /// Gets or sets whether cells are allowed to be selected
        /// </summary>
        /// <MetaDataID>{eca5b841-419d-4fde-bd1b-b395b55bad70}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowSelection
        {
            get
            {
                return this.allowSelection;
            }

            set
            {
                if (this.allowSelection != value)
                {
                    this.allowSelection = value;

                    if (!value && this.TableModel != null)
                    {
                        this.TableModel.Selections.Clear();
                    }
                }
            }
        }

        #region public SelectionStyle SelectionStyle
        /// <MetaDataID>{457fad45-7117-4135-82d6-42f7bb778a67}</MetaDataID>
        private SelectionStyle _SelectionStyle;
        /// <summary>
        /// Gets or sets how selected Cells are drawn by a Table
        /// </summary>
        /// <MetaDataID>{a87adee5-557f-4914-9b73-966edf867afe}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SelectionStyle SelectionStyle
        {
            get
            {
                return this._SelectionStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(SelectionStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectionStyle));
                }

                if (this._SelectionStyle != value)
                {
                    this._SelectionStyle = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        } 
        #endregion


        ///// <summary>
        ///// Gets or sets whether multiple cells are allowed to be selected
        ///// </summary>
        //[Browsable(false),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool MultiSelect
        //{
        //    get
        //    {

        //        return this.multiSelect;
        //    }

        //    set
        //    {
        //        if (this.multiSelect != value)
        //        {
        //            this.multiSelect = value;
        //        }
        //    }
        //}

        
        //private bool _FullRowSelect;
        ///// <summary>
        ///// Gets or sets whether all other cells in the row are highlighted 
        ///// when a cell is selected
        ///// </summary>
        //[Browsable(false),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool FullRowSelect
        //{
        //    get
        //    {
        //        return this._FullRowSelect;
        //    }

        //    set
        //    {
        //        if (this._FullRowSelect != value)
        //        {
        //            this._FullRowSelect = value;

        //            if (this.TableModel != null)
        //            {
        //                //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
        //                this.Invalidate(this.CellDataRect, false);
        //            }
        //        }
        //    }
        //}


        ///// <summary>
        ///// Gets or sets whether highlighting is removed from the selected 
        ///// cells when the Table loses focus
        ///// </summary>
        //[Category("Selection"),
        //DefaultValue(false),
        //Description("Specifies whether highlighting is removed from the selected cells when the Table loses focus")]
        //public bool HideSelection
        //{
        //    get
        //    {
        //        return this.hideSelection;
        //    }

        //    set
        //    {
        //        if (this.hideSelection != value)
        //        {
        //            this.hideSelection = value;

        //            if (!this.Focused && this.TableModel != null)
        //            {
        //                //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
        //                this.Invalidate(this.CellDataRect, false);
        //            }
        //        }
        //    }
        //}

        /// <MetaDataID>{2e53f09e-02a8-4b1c-a208-3ccda28d444a}</MetaDataID>
        private Color _SelectedCellBackColor;
        /// <summary>
        /// Gets or sets the background color of a selected cell
        /// </summary>
        /// <MetaDataID>{ffd832d7-106d-4445-abb3-cb7489cab577}</MetaDataID>
        [Category("Selection"),
        Description("The background color of a selected cell")]
        public Color SelectedCellBackColor
        {
            get
            {
                return this._SelectedCellBackColor;
            }

            set
            {
                if (this._SelectedCellBackColor != value)
                {
                    this._SelectedCellBackColor = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's SelectionBackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the SelectionBackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{00C01D96-E386-4F03-B7FA-5B8930F5B54B}</MetaDataID>
        private bool ShouldSerializeSelectionBackColor()
        {
            return (this._SelectedCellBackColor != SystemColors.Highlight);
        }

        /// <summary>
        /// The foreground color of selected TotalRows and cells
        /// </summary>
        /// <MetaDataID>{95c72101-eb90-4e98-81f5-0acb27b03848}</MetaDataID>
        private Color _SelectedCellForeColor;
        /// <summary>
        /// Gets or sets the foreground color of a selected cell
        /// </summary>
        /// <MetaDataID>{85cd0aa9-d3d5-4a4f-8a03-eafd2f80eae6}</MetaDataID>
        [Category("Selection"),
        Description("The foreground color of a selected cell")]
        public Color SelectedCellForeColor
        {
            get
            {
                return this._SelectedCellForeColor;
            }

            set
            {
                if (this._SelectedCellForeColor != value)
                {
                    this._SelectedCellForeColor = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's SelectionForeColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the SelectionForeColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{95FAD47E-F361-478D-8307-1CD281C2A8A4}</MetaDataID>
        private bool ShouldSerializeSelectionForeColor()
        {
            return (this._SelectedCellForeColor != SystemColors.HighlightText);
        }


        /// <MetaDataID>{3d6f43ee-524d-441d-b4dd-c65d40ef5638}</MetaDataID>
        private Color _UnfocusedSelectedCellBackColor;
        /// <summary>
        /// Gets or sets the background color of a selected cell when the 
        /// Table doesn't have the focus
        /// </summary>
        /// <MetaDataID>{7418b324-66d4-49de-be98-e50020f1df48}</MetaDataID>
        [Category("Selection"),
        Description("The background color of a selected cell when the Table doesn't have the focus")]
        public Color UnfocusedSelectedCellBackColor
        {
            get
            {
                return this._UnfocusedSelectedCellBackColor;
            }

            set
            {
                if (this._UnfocusedSelectedCellBackColor != value)
                {
                    this._UnfocusedSelectedCellBackColor = value;

                    if (!this.Focused && this.TableModel != null)//&& !this.HideSelection 
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's UnfocusedSelectionBackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the UnfocusedSelectionBackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{59D51259-7940-45C5-9DC1-547547DBE771}</MetaDataID>
        private bool ShouldSerializeUnfocusedSelectionBackColor()
        {
            return (this._UnfocusedSelectedCellBackColor != SystemColors.Control);
        }

        /// <MetaDataID>{12d9e4e9-d896-49cd-ad91-6afdf76ddb25}</MetaDataID>
        private Color _UnfocusedSelectedCellForeColor;
        /// <summary>
        /// Gets or sets the foreground color of a selected cell when the 
        /// Table doesn't have the focus
        /// </summary>
        /// <MetaDataID>{689044f8-5e6e-43ca-853e-4347298f0994}</MetaDataID>
        [Category("Selection"),
        Description("The foreground color of a selected cell when the Table doesn't have the focus")]
        public Color UnfocusedSelectedCellForeColor
        {
            get
            {
                return this._UnfocusedSelectedCellForeColor;
            }

            set
            {
                if (this._UnfocusedSelectedCellForeColor != value)
                {
                    this._UnfocusedSelectedCellForeColor = value;

                    if (!this.Focused  && this.TableModel != null)//&& !this.HideSelection
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's UnfocusedSelectionForeColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the UnfocusedSelectionForeColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{BAB4041D-A0EC-474C-9007-83B731D165C7}</MetaDataID>
        private bool ShouldSerializeUnfocusedSelectionForeColor()
        {
            return (this._UnfocusedSelectedCellForeColor != SystemColors.ControlText);
        }


        /// <summary>
        /// Gets an array that contains the currently selected TotalRows
        /// </summary>
        /// <MetaDataID>{668e7135-33fc-4b18-9945-7621c2c572d5}</MetaDataID>
        [Browsable(false)]
        public Row[] SelectedItems
        {
            get
            {
                if (this.TableModel == null)
                {
                    return new Row[0];
                }

                return this.TableModel.Selections.SelectedItems;
            }
        }


        /// <summary>
        /// Gets an array that contains the indexes of the currently selected TotalRows
        /// </summary>
        /// <MetaDataID>{ec6a3bea-eba0-4f97-a69a-b5b03566b580}</MetaDataID>
        [Browsable(false)]
        public int[] SelectedIndicies
        {
            get
            {
                if (this.TableModel == null)
                {
                    return new int[0];
                }

                return this.TableModel.Selections.SelectedIndicies;
            }
        }

        #endregion

        #region public TableModel TableModel

        /// <MetaDataID>{c46de248-dfd5-4382-a1eb-bd1b620bc8b3}</MetaDataID>
        private TableModel _TableModel;
        /// <summary>
        /// Gets or sets the TableModel that contains all the TotalRows
        /// and Cells displayed in the Table
        /// </summary>
        /// <MetaDataID>{0f073e14-e429-4fd3-b9b4-5ba9f824878b}</MetaDataID>
        [Category("Items"),
        DefaultValue(null),
        Description("Specifies the TableModel that contains all the TotalRows and Cells displayed in the Table")]
        public TableModel TableModel
        {
            get
            {
                if (this._TableModel == null)
                    this.TableModel = new TableModel();
                return this._TableModel;
            }

            set
            {
                if (this._TableModel != value)
                {
                    if (this._TableModel != null && this._TableModel.Table == this)
                    {
                        this._TableModel.InternalTable = null;
                    }

                    this._TableModel = value;

                    if (value != null)
                    {
                        value.RowHeight = _RowHeight;
                        value.InternalTable = this;
                    }

                    this.OnTableModelChanged(EventArgs.Empty);
                }
            }
        }


        ///// <summary>
        ///// Gets or sets the text displayed by the Table when it doesn't 
        ///// contain any items
        ///// </summary>
        //[Category("Appearance"),
        //DefaultValue("There are no items in this view"),
        //Description("Specifies the text displayed by the Table when it doesn't contain any items")]
        //public string NoItemsText
        //{
        //    get
        //    {
        //        return this.noItemsText;
        //    }

        //    set
        //    {
        //        if (!this.noItemsText.Equals(value))
        //        {
        //            this.noItemsText = value;

        //            if (this.ColumnModel == null || this.TableModel == null || this.TableModel.TotalRows.Count == 0)
        //            {
        //                this.Invalidate(this.PseudoClientRect);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region TableState

        /// <MetaDataID>{f8d75817-5756-4050-80ed-87e692a262f7}</MetaDataID>
        private TableViewState _TableView = TableViewState.WeekView;
        /// <summary>
        /// Gets or sets the current state of the Table
        /// </summary>
        /// <MetaDataID>{d76e1eb2-c1d9-4926-82c4-1758c3857329}</MetaDataID>
        internal TableViewState TableView
        {
            get
            {
                return this._TableView;
            }

            set
            {
                this._TableView = value;
            }
        }


        #region protected void CalcTableState(int x, int y) In COMMENTS
        /// <summary>
        /// Calculates the state of the Table at the specified 
        /// client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate</param>
        /// <param name="y">The client y coordinate</param>
        /// <MetaDataID>{8D4E0335-63BF-4A4F-84FA-0395A4A93A51}</MetaDataID>
        //protected void CalcTableState(int x, int y)
        //{
        //    TableRegion region = this.HitTest(x, y);

        //    // are we in the header
        //    if (region == TableRegion.ColumnHeader)
        //    {
        //        int column = this.ColumnIndexAt(x, y);

        //        // get out of here if we aren't in a column
        //        if (column == -1)
        //        {
        //            this.TableState = TableState.Normal;

        //            return;
        //        }

        //        // get the bounding rectangle for the column's header
        //        Rectangle columnRect = this.ColumnModel.ColumnHeaderRect(column);
        //        x = this.ClientToDisplayRect(x, y).X;

        //        // are we in a resizing section on the left
        //        if (x < columnRect.Left + Column.ResizePadding)
        //        {
        //            this.TableState = TableState.ColumnResizing;

        //            while (column != 0)
        //            {
        //                if (this.ColumnModel.Columns[column - 1].Visible)
        //                {
        //                    break;
        //                }

        //                column--;
        //            }

        //            // if we are in the first visible column or the next column 
        //            // to the left is disabled, then we should be potentialy 
        //            // selecting instead of resizing
        //            if (column == 0 || !this.ColumnModel.Columns[column - 1].Enabled)
        //            {
        //                this.TableState = TableState.ColumnSelecting;
        //            }
        //        }
        //        // or a resizing section on the right
        //        else if (x > columnRect.Right - Column.ResizePadding)
        //        {
        //            this.TableState = TableState.ColumnResizing;
        //        }
        //        // looks like we're somewhere in the middle of 
        //        // the column header
        //        else
        //        {
        //            this.TableState = TableState.ColumnSelecting;
        //        }
        //    }
        //    else if (region == TableRegion.Cells)
        //    {
        //        this.TableState = TableState.Selecting;
        //    }
        //    else
        //    {
        //        this.TableState = TableState.Normal;
        //    }

        //    if (this.TableState == TableState.ColumnResizing && !this.ColumnResizing)
        //    {
        //        this.TableState = TableState.ColumnSelecting;
        //    }
        //} 
        #endregion


        /// <summary>
        /// Gets whether the Table is able to raise events
        /// </summary>
        /// <MetaDataID>{6ab68210-8c83-4831-999d-98dfa540157b}</MetaDataID>
        protected internal bool CanRaiseEvents
        {
            get
            {
                return (this.IsHandleCreated && this.beginUpdateCount == 0);
            }
        }


        /// <summary>
        /// Gets or sets whether the Table is being used as a preview Table in 
        /// a ColumnCollectionEditor
        /// </summary>
        /// <MetaDataID>{cbe44857-9376-46dd-ba6b-56a241507fea}</MetaDataID>
        internal bool Preview
        {
            get
            {
                return this.preview;
            }

            set
            {
                this.preview = value;
            }
        }

        #endregion

        #region ToolTips

        /// <summary>
        /// Gets the internal tooltip component
        /// </summary>
        /// <MetaDataID>{c8f57460-8549-4ea3-842d-5862d23a2c41}</MetaDataID>
        internal ToolTip ToolTip
        {
            get
            {
                return this.toolTip;
            }
        }


        /// <summary>
        /// Gets or sets whether ToolTips are currently enabled for the Table
        /// </summary>
        /// <MetaDataID>{59a4da97-f47d-44fb-be5d-99f2fc22eb3f}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(false),
        Description("Specifies whether ToolTips are enabled for the Table.")]
        public bool EnableToolTips
        {
            get
            {
                return this.toolTip.Active;
            }

            set
            {
                this.toolTip.Active = value;
            }
        }


        /// <summary>
        /// Gets or sets the automatic delay for the Table's ToolTip
        /// </summary>
        /// <MetaDataID>{a25f98c1-05da-46a8-a77c-c83a4ff2fe57}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(500),
        Description("Specifies the automatic delay for the Table's ToolTip.")]
        public int ToolTipAutomaticDelay
        {
            get
            {
                return this.toolTip.AutomaticDelay;
            }

            set
            {
                if (value > 0 && this.toolTip.AutomaticDelay != value)
                {
                    this.toolTip.AutomaticDelay = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the period of time the Table's ToolTip remains visible if 
        /// the mouse pointer is stationary within a Cell with a valid ToolTip text
        /// </summary>
        /// <MetaDataID>{b84d2595-ed4b-49a3-8cb5-644356209401}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(5000),
        Description("Specifies the period of time the Table's ToolTip remains visible if the mouse pointer is stationary within a cell with specified ToolTip text.")]
        public int ToolTipAutoPopDelay
        {
            get
            {
                return this.toolTip.AutoPopDelay;
            }

            set
            {
                if (value > 0 && this.toolTip.AutoPopDelay != value)
                {
                    this.toolTip.AutoPopDelay = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the time that passes before the Table's ToolTip appears
        /// </summary>
        /// <MetaDataID>{0af20b0e-c759-4479-8123-b59e9bb4ebc6}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(1000),
        Description("Specifies the time that passes before the Table's ToolTip appears.")]
        public int ToolTipInitialDelay
        {
            get
            {
                return this.toolTip.InitialDelay;
            }

            set
            {
                if (value > 0 && this.toolTip.InitialDelay != value)
                {
                    this.toolTip.InitialDelay = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Table's ToolTip window is 
        /// displayed even when its parent control is not active
        /// </summary>
        /// <MetaDataID>{9b1ee0f5-cbed-4030-a080-50c06633669d}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(false),
        Description("Specifies whether the Table's ToolTip window is displayed even when its parent control is not active.")]
        public bool ToolTipShowAlways
        {
            get
            {
                return this.toolTip.ShowAlways;
            }

            set
            {
                if (this.toolTip.ShowAlways != value)
                {
                    this.toolTip.ShowAlways = value;
                }
            }
        }


        /// <summary>
        /// </summary>
        /// <MetaDataID>{46CE6F61-DB82-43C3-BA86-6AF3AD307F18}</MetaDataID>
        private void ResetToolTip()
        {
            bool tooltipActive = this.ToolTip.Active;

            if (tooltipActive)
            {
                this.ToolTip.Active = false;
            }

            this.ResetMouseEventArgs();

            this.ToolTip.SetToolTip(this, null);

            if (tooltipActive)
            {
                this.ToolTip.Active = true;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <MetaDataID>{27e5dcbc-4a32-400f-802d-5eaedb281577}</MetaDataID>
        protected override void OnResize(EventArgs e)
        {            
            if (!init)
            {
                this.ColumnModel.UpdateColumnsWidth();//this.TableModel.TotalRowHeight, this.Height
                ////Debug.WriteLine(this.DisplayRectangle.Width.ToString());
            }
            base.OnResize(e);
        }

        #region Cells

        /// <summary>
        /// Raises the CellPropertyChanged event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{0238BBA3-C3A9-42C8-B3F1-5ADA9AF34261}</MetaDataID>
        protected internal virtual void OnCellPropertyChanged(CellEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateCell(e.Row, e.Column);

                if (CellPropertyChanged != null)
                {
                    CellPropertyChanged(this, e);
                }

                //if (e.EventType == CellEventType.CheckStateChanged)
                //{
                //    this.OnCellCheckChanged(new CellCheckBoxEventArgs(e.Cell, e.Column, e.Row));
                //}
            }
        }


        /// <summary>
        /// Handler for a Cells PropertyChanged event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{70F051BE-FE93-4BFE-A6D5-59ED5B8A4064}</MetaDataID>
        private void cell_PropertyChanged(object sender, CellEventArgs e)
        {
            this.OnCellPropertyChanged(e);
        }


        #region Buttons INCOMMENTS

        ///// <summary>
        ///// Raises the CellButtonClicked event
        ///// </summary>
        ///// <param name="e">A CellButtonEventArgs that contains the event data</param>
        ///// <MetaDataID>{7266324A-3B35-4FD4-821B-181E8B4EB0BD}</MetaDataID>
        //protected internal virtual void OnCellButtonClicked(CellButtonEventArgs e)
        //{
        //    if (this.CanRaiseEvents)
        //    {
        //        if (CellButtonClicked != null)
        //        {
        //            CellButtonClicked(this, e);
        //        }
        //    }
        //}

        #endregion

        #region CheckBox INCOMMENTS

        ///// <summary>
        ///// Raises the CellCheckChanged event
        ///// </summary>
        ///// <param name="e">A CellCheckChanged that contains the event data</param>
        ///// <MetaDataID>{1A565C8B-71EE-4C59-9432-9BD14B5869DA}</MetaDataID>
        //protected internal virtual void OnCellCheckChanged(CellCheckBoxEventArgs e)
        //{
        //    if (this.CanRaiseEvents)
        //    {
        //        if (CellCheckChanged != null)
        //        {
        //            CellCheckChanged(this, e);
        //        }
        //    }
        //}

        #endregion

        #region Focus

        /// <summary>
        /// Raises the CellGotFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{02771F10-C2DF-4FE4-8F27-21AEEBC829E0}</MetaDataID>
        protected virtual void OnCellGotFocus(CellFocusEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnGotFocus(e);
                }

                if (CellGotFocus != null)
                {
                    CellGotFocus(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the GotFocus event for the Cell at the specified position
        /// </summary>
        /// <param name="cellPos">The position of the Cell that gained focus</param>
        /// <MetaDataID>{26B56C10-ADF2-4045-9F52-293A21E76D0A}</MetaDataID>
        protected void RaiseCellGotFocus(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            ICellRenderer renderer = this.ColumnModel.GetCellRenderer(cellPos.Column);

            if (renderer != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellFocusEventArgs cfea = new CellFocusEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellGotFocus(cfea);
            }
        }


        /// <summary>
        /// Raises the CellLostFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{292FB897-A941-4FAF-A929-DDF1C2106A06}</MetaDataID>
        protected virtual void OnCellLostFocus(CellFocusEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnLostFocus(e);
                }

                if (CellLostFocus != null)
                {
                    CellLostFocus(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the LostFocus event for the Cell at the specified position
        /// </summary>
        /// <param name="cellPos">The position of the Cell that lost focus</param>
        /// <MetaDataID>{4D2527BD-DD77-4D9B-9603-97A416A9FC57}</MetaDataID>
        protected void RaiseCellLostFocus(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            ICellRenderer renderer = this.ColumnModel.GetCellRenderer(cellPos.Column);

            if (renderer != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel[cellPos.Row, cellPos.Column];
                }

                CellFocusEventArgs cfea = new CellFocusEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellLostFocus(cfea);
            }
        }

        #endregion

        #region Keys 

        /// <summary>
        /// Raises the CellKeyDown event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{97326148-0840-41C7-B3DF-62230A48ADCC}</MetaDataID>
        protected virtual void OnCellKeyDown(CellKeyEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnKeyDown(e);
                }

                if (CellKeyDown != null)
                {
                    CellKeyDown(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a KeyDown event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{5AA751D2-12F7-476E-867C-FF6A6A263EA2}</MetaDataID>
        protected void RaiseCellKeyDown(CellPos cellPos, KeyEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellKeyEventArgs ckea = new CellKeyEventArgs(cell, this, cellPos, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellKeyDown(ckea);
            }
        }


        /// <summary>
        /// Raises the CellKeyUp event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{8ABAF03F-1030-4C85-BD5C-AC4410968671}</MetaDataID>
        protected virtual void OnCellKeyUp(CellKeyEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnKeyUp(e);
                }

                if (CellKeyUp != null)
                {
                    CellKeyUp(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a KeyUp event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{ED36EDB0-F411-40C0-BD83-898B3B11585B}</MetaDataID>
        protected void RaiseCellKeyUp(CellPos cellPos, KeyEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellKeyEventArgs ckea = new CellKeyEventArgs(cell, this, cellPos, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellKeyUp(ckea);
            }
        }

        #endregion

        #region Mouse

        #region MouseEnter

        /// <summary>
        /// Raises the CellMouseEnter event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{901CE110-E96A-4D2D-9DCE-F95513E6312D}</MetaDataID>
        protected virtual void OnCellMouseEnter(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseEnter(e);
                }

                if (CellMouseEnter != null)
                {
                    CellMouseEnter(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseEnter event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <MetaDataID>{E56309FF-6F2B-491B-9AAA-82D08451DD7C}</MetaDataID>
        protected void RaiseCellMouseEnter(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellMouseEnter(mcea);
            }
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the CellMouseLeave event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{0594AA39-C707-43B3-B61D-5DC1BE2DF8D2}</MetaDataID>
        protected virtual void OnCellMouseLeave(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseLeave(e);
                }

                if (CellMouseLeave != null)
                {
                    CellMouseLeave(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseLeave event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <MetaDataID>{4D964B3E-17E4-46FF-BEE1-7FB11EFAEE61}</MetaDataID>
        protected internal void RaiseCellMouseLeave(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellMouseLeave(mcea);
            }
        }

        #endregion

        #region MouseUp

        /// <summary>
        /// Raises the CellMouseUp event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{D40F58CB-DF18-4F5D-B23B-A21CCDD9FFF1}</MetaDataID>
        protected virtual void OnCellMouseUp(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseUp(e);
                }

                if (CellMouseUp != null)
                {
                    CellMouseUp(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseUp event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{75CD7E58-E5CE-4754-9DD3-0D03897FF9AC}</MetaDataID>
        protected void RaiseCellMouseUp(CellPos cellPos, MouseEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellMouseUp(mcea);
            }
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Raises the CellMouseDown event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{92C87C93-1B55-40BD-A3F5-493F668B157A}</MetaDataID>
        protected virtual void OnCellMouseDown(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseDown(e);
                }

                if (CellMouseDown != null)
                {
                    CellMouseDown(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseDown event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{9CE6208F-ADAC-4E6D-A38B-D8131833495A}</MetaDataID>
        protected void RaiseCellMouseDown(CellPos cellPos, MouseEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellMouseDown(mcea);
            }
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Raises the CellMouseMove event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{96076AEB-9E2E-40B6-92B3-037375AC44EE}</MetaDataID>
        protected virtual void OnCellMouseMove(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseMove(e);
                }

                if (CellMouseMove != null)
                {
                    CellMouseMove(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseMove event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{91B6B4BF-2C1F-44B0-9F7E-DABD2E1EE79F}</MetaDataID>
        protected void RaiseCellMouseMove(CellPos cellPos, MouseEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.TotalRows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.TotalRows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellMouseMove(mcea);
            }
        }


        /// <summary>
        /// Resets the last known cell position that the mouse was over to empty
        /// </summary>
        /// <MetaDataID>{8400B4A6-BC7C-4CDC-9342-F71AB8B1B42E}</MetaDataID>
        internal void ResetLastMouseCell()
        {
            if (!this.lastMouseCell.IsEmpty)
            {
                this.ResetMouseEventArgs();

                CellPos oldLastMouseCell = this.lastMouseCell;
                this.lastMouseCell = CellPos.Empty;

                this.RaiseCellMouseLeave(oldLastMouseCell);
            }
        }

        #endregion

        #region MouseHover

        /// <summary>
        /// Raises the CellHover event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{0FAEE45C-36F8-40B9-9516-887AB6A5EF10}</MetaDataID>
        protected virtual void OnCellMouseHover(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (CellMouseHover != null)
                {
                    CellMouseHover(e.Cell, e);
                }
            }
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the CellClick event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{170795AC-DD2C-41DC-BE30-8B28ACBEF573}</MetaDataID>
        protected virtual void OnCellClick(CellMouseEventArgs e)
        {
            lastMouseClickCell = e.CellPos;

            if (this.EditingCell != CellPos.Empty)
            {
                // don't bother if we're already editing the cell.  
                // if we're editing a different cell stop editing
                if (this.EditingCell == e.CellPos)
                {
                    return;
                }
                else
                {
                    this.EditingCellEditor.StopEditing();
                    this.editingCell = CellPos.Empty;
                }

            }

            if (!this.IsCellEnabled(e.CellPos))
            {
                return;
            }

            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(this.LastMouseCell.Column);

                if (renderer != null)
                {
                    renderer.OnClick(e);
                }

                if (CellClick != null)
                {
                    CellClick(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellDoubleClick event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{2A15B027-96D5-4283-942A-64933DAC5C7A}</MetaDataID>
        protected virtual void OnCellDoubleClick(CellMouseEventArgs e)
        {
            if (!this.IsCellEnabled(e.CellPos))
            {
                return;
            }

            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(this.LastMouseCell.Column);

                if (renderer != null)
                {
                    renderer.OnDoubleClick(e);
                }

                if (CellDoubleClick != null)
                {
                    CellDoubleClick(e.Cell, e);
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Columns

        /// <summary>
        /// Raises the ColumnPropertyChanged event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{11BD74E6-BF96-4522-8DC7-89181EBD3A5A}</MetaDataID>
        protected internal virtual void OnColumnPropertyChanged(ColumnEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                Rectangle columnHeaderRect;

                if (e.Index != -1)
                {
                    columnHeaderRect = this.ColumnHeaderRect(e.Index);
                }
                else
                {
                    columnHeaderRect = this.ColumnHeaderRect(e.Column);
                }

                switch (e.EventType)
                {
                    case ColumnEventType.VisibleChanged:
                    case ColumnEventType.WidthChanged:
                        {
                            if (e.EventType == ColumnEventType.VisibleChanged)
                            {
                                //if (e.Column.Visible && e.Index != this.lastSortedColumn)
                                //{
                                //    e.Column.InternalSortOrder = SortOrder.None;
                                //}

                                if (e.Index == this.FocusedCell.Column && !e.Column.Visible)
                                {
                                    int index = this.ColumnModel.NextVisibleColumn(e.Index);

                                    if (index == -1)
                                    {
                                        index = this.ColumnModel.PreviousVisibleColumn(e.Index);
                                    }

                                    if (index != -1)
                                    {
                                        this.FocusedCell = new CellPos(this.FocusedCell.Row, index);
                                    }
                                    else
                                    {
                                        this.FocusedCell = CellPos.Empty;
                                    }
                                }
                            }

                            if (columnHeaderRect.X <= 0)
                            {
                                this.Invalidate(this.PseudoClientRect);
                            }
                            else if (columnHeaderRect.Left <= this.PseudoClientRect.Right)
                            {
                                this.Invalidate(new Rectangle(columnHeaderRect.X,
                                    this.PseudoClientRect.Top,
                                    this.PseudoClientRect.Right - columnHeaderRect.X,
                                    this.PseudoClientRect.Height));
                            }

                            this.UpdateScrollBars();

                            break;
                        }

                    case ColumnEventType.TextChanged:
                    case ColumnEventType.StateChanged:
                    case ColumnEventType.ImageChanged:
                    case ColumnEventType.HeaderAlignmentChanged:
                        {
                            if (columnHeaderRect.IntersectsWith(this.HeaderRectangle))
                            {
                                this.Invalidate(columnHeaderRect);
                            }

                            break;
                        }

                    case ColumnEventType.AlignmentChanged:
                    case ColumnEventType.RendererChanged:
                    case ColumnEventType.EnabledChanged:
                        {
                            if (e.EventType == ColumnEventType.EnabledChanged)
                            {
                                if (e.Index == this.FocusedCell.Column)
                                {
                                    this.FocusedCell = CellPos.Empty;
                                }
                            }

                            if (columnHeaderRect.IntersectsWith(this.HeaderRectangle))
                            {
                                this.Invalidate(new Rectangle(columnHeaderRect.X,
                                    this.PseudoClientRect.Top,
                                    columnHeaderRect.Width,
                                    this.PseudoClientRect.Height));
                            }

                            break;
                        }
                }

                if (ColumnPropertyChanged != null)
                {
                    ColumnPropertyChanged(e.Column, e);
                }
            }
        }

        #endregion

        #region Column Headers

        #region MouseEnter

        /// <summary>
        /// Raises the HeaderMouseEnter event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{6E42CA31-12A2-44C5-ACE4-61D8086DF5BC}</MetaDataID>
        protected virtual void OnHeaderMouseEnter(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseEnter(e);
                }

                if (HeaderMouseEnter != null)
                {
                    HeaderMouseEnter(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseEnter event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <MetaDataID>{4D8C9FFB-13AA-4F56-AA5E-D53ADD1F2E4E}</MetaDataID>
        protected void RaiseHeaderMouseEnter(int index)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)));

                this.OnHeaderMouseEnter(mhea);
            }
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the HeaderMouseLeave event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{AFDFEA5F-5AE5-461A-9862-943D167F349D}</MetaDataID>
        protected virtual void OnHeaderMouseLeave(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseLeave(e);
                }

                if (HeaderMouseLeave != null)
                {
                    HeaderMouseLeave(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseLeave event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <MetaDataID>{E5CC2B42-EF2E-4870-8722-05A11C01A0AB}</MetaDataID>
        protected void RaiseHeaderMouseLeave(int index)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)));

                this.OnHeaderMouseLeave(mhea);
            }
        }

        #endregion

        #region MouseUp

        /// <summary>
        /// Raises the HeaderMouseUp event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{9A9C0364-689A-40EB-AE40-C904938A2961}</MetaDataID>
        protected virtual void OnHeaderMouseUp(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseUp(e);
                }

                if (HeaderMouseUp != null)
                {
                    HeaderMouseUp(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseUp event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{72A44C4E-1813-4BA4-86EC-51C2E0BDA9F1}</MetaDataID>
        protected void RaiseHeaderMouseUp(int index, MouseEventArgs e)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)), e);

                this.OnHeaderMouseUp(mhea);
            }
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Raises the HeaderMouseDown event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{99808724-FEF1-4D6F-B70F-68FC4F7C69D7}</MetaDataID>
        protected virtual void OnHeaderMouseDown(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseDown(e);
                }

                if (HeaderMouseDown != null)
                {
                    HeaderMouseDown(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseDown event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{E90C4BD2-258E-4068-BBD1-76F8DF973E5F}</MetaDataID>
        protected void RaiseHeaderMouseDown(int index, MouseEventArgs e)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)), e);

                this.OnHeaderMouseDown(mhea);
            }
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Raises the HeaderMouseMove event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{C6C14CF2-8B71-45B7-892B-9573F6E6F7C3}</MetaDataID>
        protected virtual void OnHeaderMouseMove(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseMove(e);
                }

                if (HeaderMouseMove != null)
                {
                    HeaderMouseMove(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseMove event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{8FB71649-691F-4ED9-8262-9AD7045660EA}</MetaDataID>
        protected void RaiseHeaderMouseMove(int index, MouseEventArgs e)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)), e);

                this.OnHeaderMouseMove(mhea);
            }
        }


        ///// <summary>
        ///// Resets the current "hot" column
        ///// </summary>
        ///// <MetaDataID>{BC66791E-617A-4A7F-9C4D-04ED4209338C}</MetaDataID>
        //internal void ResetHotColumn()
        //{
        //    if (this.hotColumn != -1)
        //    {
        //        this.ResetMouseEventArgs();

        //        int oldHotColumn = this.hotColumn;
        //        this.hotColumn = -1;

        //        this.RaiseHeaderMouseLeave(oldHotColumn);
        //    }
        //}

        #endregion

        #region MouseHover

        /// <summary>
        /// Raises the HeaderMouseHover event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{7C2E2EA7-AAFF-467E-88DB-5C28D2D48DFA}</MetaDataID>
        protected virtual void OnHeaderMouseHover(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (HeaderMouseHover != null)
                {
                    HeaderMouseHover(e.Column, e);
                }
            }
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the HeaderClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2C34EF30-97CE-498B-BCBA-7E2963F21DEA}</MetaDataID>
        protected virtual void OnHeaderClick(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnClick(e);
                }

                if (HeaderClick != null)
                {
                    HeaderClick(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises the HeaderDoubleClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{7412E055-C775-4066-8C5B-AD5A21A4ED6F}</MetaDataID>
        protected virtual void OnHeaderDoubleClick(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnDoubleClick(e);
                }

                if (HeaderDoubleClick != null)
                {
                    HeaderDoubleClick(e.Column, e);
                }
            }
        }

        #endregion

        #endregion

        #region ColumnModel

        /// <summary>
        /// Raises the ColumnModelChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{8DFE4D27-FD13-4AB8-A137-983A518DC816}</MetaDataID>
        protected virtual void OnColumnModelChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (ColumnModelChanged != null)
                {
                    ColumnModelChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the ColumnAdded event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{5CADEE99-565F-461E-AB51-10FC3AEDB4C7}</MetaDataID>
        protected internal virtual void OnColumnAdded(ColumnModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (ColumnAdded != null)
                {
                    ColumnAdded(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the ColumnRemoved event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{E7B4C008-E361-43FF-9436-E352A4D53EB1}</MetaDataID>
        protected internal virtual void OnColumnRemoved(ColumnModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (ColumnRemoved != null)
                {
                    ColumnRemoved(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the HeaderHeightChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{DB8B1904-833E-4F9C-A252-6086CA3FF131}</MetaDataID>
        protected internal virtual void OnHeaderHeightChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (HeaderHeightChanged != null)
                {
                    HeaderHeightChanged(this, e);
                }
            }
        }

        #endregion

        #region Editing IN COMMENTS

        ///// <summary>
        ///// Raises the BeginEditing event
        ///// </summary>
        ///// <param name="e">A CellEditEventArgs that contains the event data</param>
        ///// <MetaDataID>{82A15115-CFEA-4D98-9C3E-54BB9F3FB262}</MetaDataID>
        //protected internal virtual void OnBeginEditing(CellEditEventArgs e)
        //{
        //    if (this.CanRaiseEvents)
        //    {
        //        if (BeginEditing != null)
        //        {
        //            BeginEditing(e.Cell, e);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Raises the EditingStopped event
        ///// </summary>
        ///// <param name="e">A CellEditEventArgs that contains the event data</param>
        ///// <MetaDataID>{FB375E0A-B43C-4D55-9F6B-DE44653EA41B}</MetaDataID>
        //protected internal virtual void OnEditingStopped(CellEditEventArgs e)
        //{
        //    if (this.CanRaiseEvents)
        //    {
        //        if (EditingStopped != null)
        //        {
        //            EditingStopped(e.Cell, e);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Raises the EditingCancelled event
        ///// </summary>
        ///// <param name="e">A CellEditEventArgs that contains the event data</param>
        ///// <MetaDataID>{0DB1EFEF-402A-40B6-8B51-5D28D8BDF6FE}</MetaDataID>
        //protected internal virtual void OnEditingCancelled(CellEditEventArgs e)
        //{
        //    if (this.CanRaiseEvents)
        //    {
        //        if (EditingCancelled != null)
        //        {
        //            EditingCancelled(e.Cell, e);
        //        }
        //    }
        //}

        #endregion

        #region Focus

        /// <summary>
        /// Raises the GotFocus event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{AAED0A08-E2B6-4118-B1C5-13CBC6FFFE2E}</MetaDataID>
        protected override void OnGotFocus(EventArgs e)
        {
            //if (SelectedIndicies.Length == 0 && _TableModel != null && _TableModel.TotalRows.Count > 0 && columnModel.Columns.Count > 0)
            //    this.TableModel.Selections.SelectCells(0, 0, 0, columnModel.Columns.Count - 1);

            _BackColor = BackColor;
            BackColor = _FocusedBackColor;
            if (this.FocusedCell.IsEmpty)
            {
                CellPos p = this.FindNextVisibleEnabledCell(this.FocusedCell, true, true, true, true);

                if (this.IsValidCell(p))
                {
                    this.FocusedCell = p;
                }
            }
            else
            {
                this.RaiseCellGotFocus(this.FocusedCell);
            }

            if (this.SelectedIndicies.Length > 0)
            {
                this.Invalidate(this.CellDataRect);
            }

            base.OnGotFocus(e);
        }


        /// <summary>
        /// Raises the LostFocus event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{245AA511-2C19-4BE7-8EBF-EDDE6E2D9FEB}</MetaDataID>
        protected override void OnLostFocus(EventArgs e)
        {
            base.BackColor = _BackColor;
            if (!this.FocusedCell.IsEmpty)
            {
                this.RaiseCellLostFocus(this.FocusedCell);
            }

            if (this.SelectedIndicies.Length > 0)
            {
                this.Invalidate(this.CellDataRect);
            }

            base.OnLostFocus(e);
        }

        #endregion

        #region Keys 

        #region KeyDown 

        /// <summary>
        /// Raises the KeyDown event
        /// </summary>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{15009A9C-38A9-4D1F-89AC-A831DAD67B4B}</MetaDataID>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (this.IsValidCell(this.FocusedCell))
            {
                if (this.IsReservedKey(e.KeyData))
                {
                    Keys key = e.KeyData & Keys.KeyCode;

                    if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right)
                    {
                        CellPos nextCell;

                        if (key == Keys.Up)
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, this.FocusedCell.Row > 0, false, false, false);
                        }
                        else if (key == Keys.Down)
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, this.FocusedCell.Row < this.RowCount - 1, true, false, false);
                        }
                        else if (key == Keys.Left)
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, false, false, false, true);
                            //this.Invalidate();
                        }
                        else
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, false, true, false, true);
                        }

                        if (nextCell != CellPos.Empty)
                        {
                            this.FocusedCell = nextCell;
                            this.TableModel.Selections.SelectCell(this.FocusedCell);//e.ClipRectangle.Top + 2

                            //if ((e.KeyData & Keys.Modifiers) == Keys.Shift && this.MultiSelect)
                            //{
                            //    this.TableModel.Selections.AddShiftSelectedCell(this.FocusedCell);
                            //}
                            //else
                            //{
                            //    this.TableModel.Selections.SelectCell(this.FocusedCell);//e.ClipRectangle.Top + 2
                            //    //this.Invalidate();
                            //    //Column clm = this.ColumnModel.Columns[this.FocusedCell.Column];
                            //    //e.Graphics.DrawLine(gridPen, right - 1, clm.X, right - 1, e.ClipRectangle.Bottom);

                            //}
                        }
                    }
                    else if (e.KeyData == Keys.PageUp)
                    {
                        if (this.RowCount > 0)
                        {
                            CellPos nextCell;

                            if (!this.VScroll)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(new CellPos(0, this.FocusedCell.Column), true, true, true, false);
                            }
                            else
                            {
                                if (this.FocusedCell.Row > this._VScrollBar.Value && this.TableModel[this._VScrollBar.Value, this.FocusedCell.Column].Enabled)
                                {
                                    nextCell = this.FindNextVisibleEnabledCell(new CellPos(this._VScrollBar.Value, this.FocusedCell.Column), true, true, true, false);
                                }
                                else
                                {
                                    nextCell = this.FindNextVisibleEnabledCell(new CellPos(Math.Max(-1, this._VScrollBar.Value - (this._VScrollBar.LargeChange - 1)), this.FocusedCell.Column), true, true, true, false);
                                }
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                    else if (e.KeyData == Keys.PageDown)
                    {
                        if (this.RowCount > 0)
                        {
                            CellPos nextCell;

                            if (!this.VScroll)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(new CellPos(this.RowCount - 1, this.FocusedCell.Column), true, false, true, false);
                            }
                            else
                            {
                                if (this.FocusedCell.Row < this._VScrollBar.Value + this._VScrollBar.LargeChange)
                                {
                                    if (this.FocusedCell.Row == (this._VScrollBar.Value + this._VScrollBar.LargeChange) - 1 &&
                                        this.RowRect(this._VScrollBar.Value + this._VScrollBar.LargeChange).Bottom > this.CellDataRect.Bottom)
                                    {
                                        nextCell = this.FindNextVisibleEnabledCell(new CellPos(Math.Min(this.RowCount - 1, this.FocusedCell.Row - 1 + this._VScrollBar.LargeChange), this.FocusedCell.Column), true, false, true, false);
                                    }
                                    else
                                    {
                                        nextCell = this.FindNextVisibleEnabledCell(new CellPos(this._VScrollBar.Value + this._VScrollBar.LargeChange - 1, this.FocusedCell.Column), true, false, true, false);
                                    }
                                }
                                else
                                {
                                    nextCell = this.FindNextVisibleEnabledCell(new CellPos(Math.Min(this.RowCount - 1, this.FocusedCell.Row + this._VScrollBar.LargeChange), this.FocusedCell.Column), true, false, true, false);
                                }
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                    else if (e.KeyData == Keys.Home || e.KeyData == Keys.End)
                    {
                        if (this.RowCount > 0)
                        {
                            CellPos nextCell;

                            if (e.KeyData == Keys.Home)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(CellPos.Empty, true, true, true, true);
                            }
                            else
                            {
                                nextCell = this.FindNextVisibleEnabledCell(new CellPos(this.RowCount - 1, this.TableModel.TotalRows[this.RowCount - 1].Cells.Count), true, false, true, true);
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                }
                else
                {
                    //// check if we can start editing with the custom edit key
                    //if (e.KeyData == this.CustomEditKey)//&& this.EditStartAction == EditStartAction.CustomKey)
                    //{
                    //    this.EditCell(this.FocusedCell);

                    //    return;
                    //}

                    // send all other key events to the cell's renderer
                    // for further processing
                    this.RaiseCellKeyDown(this.FocusedCell, e);
                }
            }
            else
            {
                if (this.FocusedCell == CellPos.Empty)
                {
                    Keys key = e.KeyData & Keys.KeyCode;

                    if (this.IsReservedKey(e.KeyData))
                    {
                        if (key == Keys.Down || key == Keys.Right)
                        {
                            CellPos nextCell;

                            if (key == Keys.Down)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, true, true, true, false);
                            }
                            else
                            {
                                nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, false, true, true, true);
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;
                                this.TableModel.Selections.SelectCell(this.FocusedCell);

                                //if ((e.KeyData & Keys.Modifiers) == Keys.Shift && this.MultiSelect)
                                //{
                                //    this.TableModel.Selections.AddShiftSelectedCell(this.FocusedCell);
                                //}
                                //else
                                //{
                                //    this.TableModel.Selections.SelectCell(this.FocusedCell);
                                //}
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region protected override void OnKeyUp(KeyEventArgs e) 

        /// <summary>
        /// Raises the KeyUp event
        /// </summary>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{9190D24F-1418-4BCA-8C54-1F869F8DC8A3}</MetaDataID>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (!this.IsReservedKey(e.KeyData))
            {
                // 
                //if (e.KeyData == this.CustomEditKey && this.EditStartAction == EditStartAction.CustomKey)
                //{
                //    return;
                //}

                // send all other key events to the cell's renderer
                // for further processing
                this.RaiseCellKeyUp(this.FocusedCell, e);
            }
            else
            {
                //if (e.KeyData == Keys.Insert)
                //{
                //    InsertRow();
                //}
                //if (e.KeyData == Keys.Delete)
                //{
                //    foreach (Row row in SelectedItems)
                //        ListConnection.DeleteRow(row);
                //}

                //if (e.KeyData == Keys.Tab)
                //{
                //    if (this.lastEditingCell != CellPos.Empty)
                //    {
                //        int nextColumn = this.lastEditingCell.Column + 1;
                //        while (columnModel.Columns.Count > nextColumn && !columnModel.Columns[nextColumn].Editable)
                //            nextColumn++;

                //        if (columnModel.Columns.Count > nextColumn)
                //        {
                //            EditCell(new CellPos(this.lastEditingCell.Row, nextColumn));
                //        }

                //    }


                //}


            }
        }

        #endregion

        #endregion

        #region protected override void OnLayout(LayoutEventArgs levent)

        /// <summary>
        /// Raises the Layout event
        /// </summary>
        /// <param name="levent">A LayoutEventArgs that contains the event data</param>
        /// <MetaDataID>{F69D37E0-CB3B-47E8-AC65-88840BE9C555}</MetaDataID>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (!this.IsHandleCreated || this.init)
            {
                return;
            }

            base.OnLayout(levent);

            this.UpdateScrollBars();
            this.UpdateLongActionsRect();
            this.UpdateLongActionsScroll();
            this.UpdateDetailDayScroll();            

        }

        #endregion

        #region Mouse

        #region protected override void OnMouseUp(MouseEventArgs e)

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{F229E761-171A-44A3-AE8D-D497EC04B4F2}</MetaDataID>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (!this.CanRaiseEvents)
            {
                return;
            }

            // work out the current state of  play
            //this.CalcTableState(e.X, e.Y);

            TableRegion region = this.HitTest(e.X, e.Y);

            #region if (e.Button == MouseButtons.Left)
            if (e.Button == MouseButtons.Left)
            {
                // if the left mouse button was down for a cell, 
                // Raise a mouse up for that cell
                if (!this.LastMouseDownCell.IsEmpty)
                {
                    if (this.IsValidCell(this.LastMouseDownCell))
                    {
                        this.RaiseCellMouseUp(this.LastMouseDownCell, e);
                    }

                    // reset the lastMouseDownCell
                    this.lastMouseDownCell = CellPos.Empty;
                }

                //// if we have just finished resizing, it might
                //// be a good idea to relayout the table
                //if (this.resizingColumnIndex != -1)
                //{
                //    if (this.resizingColumnWidth != -1)
                //    {
                //        this.DrawReversibleLine(this.ColumnRect(this.resizingColumnIndex).Left + this.resizingColumnWidth);
                //    }

                //    this.ColumnModel.Columns[this.resizingColumnIndex].Width = this.resizingColumnWidth;

                //    this.resizingColumnIndex = -1;
                //    this.resizingColumnWidth = -1;

                //    this.UpdateScrollBars();
                //    this.Invalidate(this.PseudoClientRect, true);
                //}

                // check if the mouse was released in a column header
                if (region == TableRegion.ColumnHeader)
                {
                    int columnindex = this.ColumnIndexAt(e.X, e.Y);
                    DayColumn column = this.ColumnModel.Columns[columnindex] as DayColumn;
                    if (column == null)
                        return;
                    if (column.DetailDayViewRectange.Contains(e.X, e.Y))
                    {
                        if (this.TableView != TableViewState.DayView)
                        {
                            this.TableView = TableViewState.DayView;
                            this.ColumnModel.ClearColumnsButThis(column);
                            this.ShowLongActionsScroll = false;


                            UpdateLongActionsRect();
                            this.ColumnModel.UpdateColumnsWidth();//this.TableModel.TotalRowHeight, this.Height
                            //this.ShowDetailDayScroll = true;//UpdateWidth sets it

                            //this._DetaiDayRectangle.X = System.Convert.ToInt32(column.X);
                            //this.Table._DetaiDayRectangle.Y = this.Table.BorderWidth + this.Table.HeaderHeight + this.Table.LongActionsRectHeight;
                            //this.Table._DetaiDayRectangle.Height = this.Table.DisplayRectangle.Height;

                        }
                        else
                        {
                            this.ShowDetailDayScroll = false;
                            this.TableView = TableViewState.WeekView;
                            this.ColumnModel.CreateAndShowWeek(column.Date, false);
                            this.ColumnModel.UpdateColumnsWidth();//this.TableModel.TotalRowHeight, this.Height                            
                        }
                    }
                    //// if we are in the header, check if we are in the pressed column
                    //if (this.pressedColumn != -1)
                    //{
                    //    if (this.pressedColumn == column)
                    //    {
                    //        if (this.hotColumn != -1 && this.hotColumn != column)
                    //        {
                    //            this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;
                    //        }

                    //        this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Hot;

                    //        this.RaiseHeaderMouseUp(column, e);
                    //    }

                    //    this.pressedColumn = -1;

                    //    //// only sort the column if we have TotalRows to sort
                    //    //if (this.ColumnModel.Columns[column].Sortable)
                    //    //{
                    //    //    if (this.TableModel != null && this.TableModel.TotalRows.Count > 0)
                    //    //    {
                    //    //        this.Sort(column);
                    //    //    }
                    //    //}

                    //    this.Invalidate(this.HeaderRectangle, false);
                    //}

                    return;
                }
                
            
                //// the mouse wasn't released in a column header, so if we 
                //// have a pressed column then we need to make it unpressed
                //if (this.pressedColumn != -1)
                //{
                //    this.pressedColumn = -1;

                //    this.Invalidate(this.HeaderRectangle, false);
                //}
            }
            #endregion

            if (e.Button == MouseButtons.Right)
            {
                FocusedActionViews = this.HitActionTest(e.X, e.Y);
                this.OnViewMenu(this,e);
                if (FocusedActionViews.Count != 0)
                {
                    //ActionMouseEventArgs amea = new ActionMouseEventArgs(FocusedActionViews, e);
                    //ActionRenderer.OnActionMouseUp(amea);
                    //if (this.ActionMouseUp != null)
                    //    this.ActionMouseUp(FocusedActionViews, amea);
                    return;
                }
            }
            //if (TableModel != null && lastMouseDownCell.Row != null && lastMouseDownCell.Column != null)
            //    this.TableModel.Selections.SelectCell(lastMouseDownCell.Row, lastMouseDownCell.Column);
        }



        #endregion

        #region protected override void OnMouseDown(MouseEventArgs e)



        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{90842172-6028-4A7B-A6B8-34D4B2C8B540}</MetaDataID>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //if (!MouseDownTimer.Enabled && SelectedItems.Length > 1)
            //{
            //    LazyMouseEvent = e;
            //    MouseDownTimer.Interval = 150;
            //    MouseDownTimer.Enabled = true;
            //    return;
            //}


            base.OnMouseDown(e);

            if (!this.CanRaiseEvents)
            {
                return;
            }

            if (FocusedActionViews != null)
            {
                foreach(SchedulerActionView av in FocusedActionViews)
                    av.Focus = false;
                this.SelectedAction=null;
                FocusedActionViews.Clear();
                //ActionMouseEventArgs amea = new ActionMouseEventArgs(FocusedActionView, e, false);
                //ActionRenderer.OnActionMouseDown(amea);
            }

            FocusedActionViews = this.HitActionTest(e.X, e.Y);
            if (FocusedActionViews.Count != 0)
            {
                foreach (SchedulerActionView av in FocusedActionViews)
                {                      
                    SelectedAction=av.Action;
                    av.Focus = true;
                }

                this.Invalidate();
                //ActionMouseEventArgs amea = new ActionMouseEventArgs(FocusedActionView, e);
                //ActionRenderer.OnActionMouseDown(amea);
                //if(this.ActionMouseDown!=null)
                //    this.ActionMouseDown(FocusedActionView, amea);
                return;
            }

            //this.CalcTableState(e.X, e.Y);
            TableRegion region = this.HitTest(e.X, e.Y);

            int rowindex = this.RowIndexAt(e.X, e.Y);
            int columnindex = this.ColumnIndexAt(e.X, e.Y);

            Row row = this.TableModel.RowFromIndex(rowindex);
            if (row != null)
            {
                this.SelectedTimeStart = row.StartMinute;
                this.SelectedTimeEnd = row.EndMinute;
            }

            DayColumn column = this.ColumnModel.ColumnAtX(e.X) as DayColumn;            
            if (column != null&&row!=null)
                this.SelectedDate = new DateTime(column.Date.Year,column.Date.Month,column.Date.Day,row.Hour,0,0);

            if (this.IsEditing)
            {
                if (this.EditingCell.Row != rowindex || this.EditingCell.Column != columnindex)
                {
                    this.Focus();

                    if (region == TableRegion.ColumnHeader && e.Button != MouseButtons.Right)
                    {
                        return;
                    }
                }
            }

            #region ColumnHeader clicked IN COMMENTS

            //if (region == TableRegion.ColumnHeader)
            //{
            //    //if (e.Button == MouseButtons.Right && this.HeaderContextMenu.Enabled)
            //    //{
            //    //    this.HeaderContextMenu.Show(this, new Point(e.X, e.Y));

            //    //    return;
            //    //}

            //    if (column == -1 || !this.ColumnModel.Columns[column].Enabled)
            //    {
            //        return;
            //    }

            //    if (e.Button == MouseButtons.Left)
            //    {
            //        this.FocusedCell = new CellPos(-1, -1);

            //        // don't bother going any further if the user 
            //        // double clicked
            //        if (e.Clicks > 1)
            //        {
            //            return;
            //        }

            //        this.RaiseHeaderMouseDown(column, e);

            //        if (this.TableState == TableState.ColumnResizing)
            //        {
            //            #region TableState.ColumnResizing
            //            //Rectangle columnRect = this.ColumnModel.ColumnHeaderRect(column);
            //            //int x = this.ClientToDisplayRect(e.X, e.Y).X;

            //            //if (x <= columnRect.Left + Column.ResizePadding)
            //            //{
            //            //    //column--;
            //            //    column = this.ColumnModel.PreviousVisibleColumn(column);
            //            //}

            //            //this.resizingColumnIndex = column;

            //            //if (this.resizingColumnIndex != -1)
            //            //{
            //            //    this.resizingColumnAnchor = this.ColumnModel.ColumnHeaderRect(column).Left;
            //            //    this.resizingColumnOffset = x - System.Convert.ToInt32((this.resizingColumnAnchor + this.ColumnModel.Columns[column].Width));
            //            //} 
            //            #endregion
            //        }
            //        else
            //        {
            //            if (this.HeaderStyle != ColumnHeaderStyle.Clickable || !this.ColumnModel.Columns[column].Sortable)
            //            {
            //                return;
            //            }

            //            if (column == -1)
            //            {
            //                return;
            //            }

            //            if (this.pressedColumn != -1)
            //            {
            //                this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Normal;
            //            }

            //            this.pressedColumn = column;
            //            this.ColumnModel.Columns[column].InternalColumnState = ColumnState.Pressed;
            //        }

            //        return;
            //    }
            //}

            #endregion

            #region Cells Area clicked

            if (region == TableRegion.Cells)
            {
                if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (!this.IsValidCell(rowindex, columnindex) || !this.IsCellEnabled(rowindex, columnindex))
                {
                    // clear selections
                    if (TableModel != null)
                        this.TableModel.Selections.Clear();

                    return;
                }

                this.FocusedCell = new CellPos(rowindex, columnindex);

                // don't bother going any further if the user 
                // double clicked or we're not allowed to select
                if (e.Clicks > 1 || !this.AllowSelection)
                {
                    return;
                }

                this.lastMouseDownCell.Row = rowindex;
                this.lastMouseDownCell.Column = columnindex;

                //
                this.RaiseCellMouseDown(new CellPos(rowindex, columnindex), e);

                if (!this.ColumnModel.Columns[columnindex].Selectable)
                {
                    return;
                }

                //

                //if ((ModifierKeys & Keys.Shift) == Keys.Shift && this.MultiSelect)
                //{
                //    if (e.Button == MouseButtons.Right)
                //    {
                //        return;
                //    }
                //    if (TableModel != null)
                //        this.TableModel.Selections.AddShiftSelectedCell(row, column);

                //    return;
                //}

                //if ((ModifierKeys & Keys.Control) == Keys.Control && this.MultiSelect)
                //{
                //    if (e.Button == MouseButtons.Right)
                //    {
                //        return;
                //    }
                //    if (TableModel != null)
                //    {

                //        if (this.TableModel.Selections.IsCellSelected(row, column))
                //        {
                //            this.TableModel.Selections.RemoveCell(row, column);
                //        }
                //        else
                //        {
                //            this.TableModel.Selections.AddCell(row, column);
                //        }
                //    }

                //    return;
                //}
                if (TableModel != null)
                {
                    this.TableModel.Selections.SelectCell(rowindex, columnindex);
                    this.Invalidate();
                }
            }

            #endregion
        }

        #endregion

        #region protected override void OnMouseMove(MouseEventArgs e)
        /// <MetaDataID>{6c1610e1-2686-4c65-86e7-977c93158c6e}</MetaDataID>
        private Cursor _Oldcursor = null;
        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{DF055BA9-6EBE-44D1-BB11-2AF046C99FD7}</MetaDataID>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
                        
            int columnindex = this.ColumnIndexAt(e.X, e.Y);
            DayColumn column = this.ColumnModel.Columns[columnindex] as DayColumn;
            if (column != null)
            {
                if (column.DetailDayViewRectange.Contains(e.X, e.Y))
                {
                    if(_Oldcursor==null)
                        _Oldcursor = Cursor;
                    Cursor = Cursors.Hand;
                }
                else 
                {
                    if (_Oldcursor != null)
                    {
                        Cursor = _Oldcursor;//cursor;
                        _Oldcursor = null;
                    }
                }
            }
          
            #region IN COMMENTS
            ////// don't go any further if the table is editing
            ////if (this.TableState == TableState.Editing)
            ////{
            ////    return;
            ////}
            ////if (ListConnection.AllowDrag)
            ////    DragRow(e);

            //// if the left mouse button is down, check if the LastMouseDownCell 
            //// references a valid cell.  if it does, send the mouse move message 
            //// to the cell and then exit (this will stop other cells/headers 
            //// from getting the mouse move message even if the mouse is over 
            //// them - this seems consistent with the way windows does it for 
            //// other controls)
            //if (e.Button == MouseButtons.Left)
            //{
            //    if (!this.LastMouseDownCell.IsEmpty)
            //    {
            //        if (this.IsValidCell(this.LastMouseDownCell))
            //        {
            //            this.RaiseCellMouseMove(this.LastMouseDownCell, e);
            //            return;
            //        }
            //    }
            //}

            //// are we resizing a column?
            //if (this.resizingColumnIndex != -1)
            //{
            //    if (this.resizingColumnWidth != -1)
            //    {
            //        this.DrawReversibleLine(this.ColumnRect(this.resizingColumnIndex).Left + this.resizingColumnWidth);
            //    }

            //    // calculate the new width for the column
            //    int width = this.ClientToDisplayRect(e.X, e.Y).X - this.resizingColumnAnchor - this.resizingColumnOffset;

            //    // make sure the new width isn't smaller than the minimum allowed
            //    // column width, or larger than the maximum allowed column width
            //    if (width < Column.MinimumWidth)
            //    {
            //        width = Column.MinimumWidth;
            //    }
            //    else if (width > Column.MaximumWidth)
            //    {
            //        width = Column.MaximumWidth;
            //    }

            //    this.resizingColumnWidth = width;

            //    //this.ColumnModel.Columns[this.resizingColumnIndex].Width = width;
            //    this.DrawReversibleLine(this.ColumnRect(this.resizingColumnIndex).Left + this.resizingColumnWidth);


            //    return;
            //}
            //if (e.Button == MouseButtons.None)
            //    Cursor = Cursors.Default;
            //// work out the potential state of play
            ////this.CalcTableState(e.X, e.Y);

            //TableRegion hitTest = this.HitTest(e.X, e.Y);

            //#region ColumnHeader

            //if (hitTest == TableRegion.ColumnHeader)
            //{
            //    // this next bit is pretty complicated. need to work 
            //    // out which column is displayed as pressed or hot 
            //    // (so we have the same behaviour as a themed ListView
            //    // in Windows XP)

            //    int column = this.ColumnIndexAt(e.X, e.Y);

            //    // if this isn't the current hot column, reset the
            //    // hot columns state to normal and set this column
            //    // to be the hot column
            //    if (this.hotColumn != column)
            //    {
            //        if (this.hotColumn != -1)
            //        {
            //            this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;

            //            this.RaiseHeaderMouseLeave(this.hotColumn);
            //        }

            //        if (this.TableState != TableState.ColumnResizing)
            //        {
            //            this.hotColumn = column;

            //            if (this.hotColumn != -1 && this.ColumnModel.Columns[column].Enabled)
            //            {
            //                this.ColumnModel.Columns[column].InternalColumnState = ColumnState.Hot;

            //                this.RaiseHeaderMouseEnter(column);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (column != -1 && this.ColumnModel.Columns[column].Enabled)
            //        {
            //            this.RaiseHeaderMouseMove(column, e);
            //        }
            //    }

            //    // if this isn't the pressed column, then the pressed columns
            //    // state should be set back to normal
            //    if (this.pressedColumn != -1 && this.pressedColumn != column)
            //    {
            //        this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Normal;
            //    }
            //    // else if this is the pressed column and its state is not
            //    // pressed, then we had better set it
            //    else if (column != -1 && this.pressedColumn == column && this.ColumnModel.Columns[this.pressedColumn].ColumnState != ColumnState.Pressed)
            //    {
            //        this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Pressed;
            //    }

            //    // set the cursor to a resizing cursor if necesary
            //    if (this.TableState == TableState.ColumnResizing)
            //    {
            //        Rectangle columnRect = this.ColumnModel.ColumnHeaderRect(column);
            //        int x = this.ClientToDisplayRect(e.X, e.Y).X;

            //        this.Cursor = Cursors.VSplit;

            //        // if the left mouse button is down, we don't want
            //        // the resizing cursor so set it back to the default
            //        if (e.Button == MouseButtons.Left)
            //        {
            //            this.Cursor = Cursors.Default;
            //        }

            //        // if the mouse is in the left side of the column, 
            //        // the first non-hidden column to the left needs to
            //        // become the hot column (so the user knows which
            //        // column would be resized if a resize action were
            //        // to take place
            //        if (x < columnRect.Left + Column.ResizePadding)
            //        {
            //            int col = column;

            //            while (col != 0)
            //            {
            //                col--;

            //                if (this.ColumnModel.Columns[col].Visible)
            //                {
            //                    break;
            //                }
            //            }

            //            if (col != -1)
            //            {
            //                if (this.ColumnModel.Columns[col].Enabled)
            //                {
            //                    if (this.hotColumn != -1)
            //                    {
            //                        this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;
            //                    }

            //                    this.hotColumn = col;
            //                    this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Hot;

            //                    this.RaiseHeaderMouseEnter(col);
            //                }
            //                else
            //                {
            //                    this.Cursor = Cursors.Default;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (this.ColumnModel.Columns[column].Enabled)
            //            {
            //                // this mouse is in the right side of the column, 
            //                // so this column needs to be dsiplayed hot
            //                this.hotColumn = column;
            //                this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Hot;
            //            }
            //            else
            //            {
            //                this.Cursor = Cursors.Default;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        // we're not in a resizing area, so make sure the cursor
            //        // is the default cursor (we may have just come from a
            //        // resizing area)
            //        this.Cursor = Cursors.Default;
            //    }

            //    // reset the last cell the mouse was over
            //    this.ResetLastMouseCell();

            //    return;
            //}

            //#endregion

            ////// we're outside of the header, so if there is a hot column,
            ////// it need to be reset
            ////if (this.hotColumn != -1)
            ////{
            ////    this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;

            ////    this.ResetHotColumn();
            ////}

            ////// if there is a pressed column, its state need to beset to normal
            ////if (this.pressedColumn != -1)
            ////{
            ////    this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Normal;
            ////}

            //#region Cells

            //if (hitTest == TableRegion.Cells)
            //{
            //    // find the cell the mouse is over
            //    CellPos cellPos = new CellPos(this.RowIndexAt(e.X, e.Y), this.ColumnIndexAt(e.X, e.Y));

            //    if (!cellPos.IsEmpty)
            //    {
            //        if (cellPos != this.lastMouseCell)
            //        {
            //            // check if the cell exists (ie is not null)
            //            if (this.IsValidCell(cellPos))
            //            {
            //                CellPos oldLastMouseCell = this.lastMouseCell;

            //                if (!oldLastMouseCell.IsEmpty)
            //                {
            //                    this.ResetLastMouseCell();
            //                }

            //                this.lastMouseCell = cellPos;

            //                this.RaiseCellMouseEnter(cellPos);
            //            }
            //            else
            //            {
            //                this.ResetLastMouseCell();

            //                // make sure the cursor is the default cursor 
            //                // (we may have just come from a resizing area in the header)
            //                this.Cursor = Cursors.Default;
            //            }
            //        }
            //        else
            //        {
            //            this.RaiseCellMouseMove(cellPos, e);
            //        }
            //    }
            //    else
            //    {
            //        this.ResetLastMouseCell();

            //        if (this.TableModel == null)
            //        {
            //            this.ResetToolTip();
            //        }
            //    }

            //    return;
            //}
            //else
            //{
            //    this.ResetLastMouseCell();

            //    if (!this.lastMouseDownCell.IsEmpty)
            //    {
            //        this.RaiseCellMouseLeave(this.lastMouseDownCell);
            //    }

            //    if (this.TableModel == null)
            //    {
            //        this.ResetToolTip();
            //    }

            //    // make sure the cursor is the default cursor 
            //    // (we may have just come from a resizing area in the header)
            //    this.Cursor = Cursors.Default;
            //}

            //#endregion
            #endregion
        }


        #endregion

        #region protected override void OnMouseLeave(EventArgs e)

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{514BB8E0-7A4C-4820-BC16-CFA6E4B617CD}</MetaDataID>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            //// we're outside of the header, so if there is a hot column,
            //// it needs to be reset (this shouldn't happen, but better 
            //// safe than sorry ;)
            //if (this.hotColumn != -1)
            //{
            //    this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;

            //    this.ResetHotColumn();
            //}
        }

        #endregion

        #region protected override void OnMouseWheel(MouseEventArgs e)

        /// <summary>
        /// Raises the MouseWheel event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{4F0B99C2-C2F1-49C3-B678-86B57A295C22}</MetaDataID>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!this.Scrollable || ( !this.VScroll))//!this.HScroll &&
            {
                return;
            }

            if (this.VScroll)
            {
                int newVal = this._VScrollBar.Value - ((e.Delta / 120) * SystemInformation.MouseWheelScrollLines);

                if (newVal < 0)
                {
                    newVal = 0;
                }
                else if (newVal > this._VScrollBar.Maximum - this._VScrollBar.LargeChange + 1)
                {
                    newVal = this._VScrollBar.Maximum - this._VScrollBar.LargeChange + 1;
                }

                this.VerticalScroll(newVal);
                this._VScrollBar.Value = newVal;
            }
            //else if (this.HScroll)
            //{
            //    int newVal = this.hScrollBar.Value - ((e.Delta / 120) * Column.MinimumWidth);

            //    if (newVal < 0)
            //    {
            //        newVal = 0;
            //    }
            //    else if (newVal > this.hScrollBar.Maximum - this.hScrollBar.LargeChange)
            //    {
            //        newVal = this.hScrollBar.Maximum - this.hScrollBar.LargeChange;
            //    }

            //    this.HorizontalScroll(newVal);
            //    this.hScrollBar.Value = newVal;
            //}
        }

        #endregion

        #region  protected override void OnMouseHover(EventArgs e)

        /// <summary>
        /// Raises the MouseHover event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{2DD1AA40-E424-4F64-901B-1FEBEBC9CF24}</MetaDataID>
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            
            //if (this.IsValidCell(this.LastMouseCell))
            //{
            //    this.OnCellMouseHover(new CellMouseEventArgs(this.TableModel[this.LastMouseCell], this, this.LastMouseCell, this.CellRect(this.LastMouseCell)));
            //}
            //else if (this.hotColumn != -1)
            //{
            //    this.OnHeaderMouseHover(new HeaderMouseEventArgs(this.ColumnModel.Columns[this.hotColumn], this, this.hotColumn, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(this.hotColumn))));
            //}
        }

        #endregion

        #region protected override void OnClick(EventArgs e)

        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{D2334325-29D0-41DD-BA2B-45722B2CC326}</MetaDataID>
        protected override void OnClick(EventArgs e)
        {
            Focus();
            base.OnClick(e);

            
            if (this.IsValidCell(this.LastMouseCell))
            {
                this.OnCellClick(new CellMouseEventArgs(this.TableModel[this.LastMouseCell], this, this.LastMouseCell, this.CellRect(this.LastMouseCell), e as MouseEventArgs));

            }
            //else if (this.hotColumn != -1)
            //{
            //    this.OnHeaderClick(new HeaderMouseEventArgs(this.ColumnModel.Columns[this.hotColumn], this, this.hotColumn, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(this.hotColumn))));
            //}
    

        }

        /// <MetaDataID>{f050f1f5-982c-4bab-ba3b-faa6fc8bfa31}</MetaDataID>
        protected override void OnCreateControl()
        {
            //_BackColor = BackColor;
            //BackColor = _FocusedBackColor;

            base.OnCreateControl();
        }
        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{61F81C9D-A9BA-4226-800B-CB6C7CE5ECE6}</MetaDataID>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (this.IsValidCell(this.LastMouseCell))
            {
                Rectangle cellRect = this.CellRect(this.LastMouseCell);

                this.OnCellDoubleClick(new CellMouseEventArgs(this.TableModel[this.LastMouseCell], this, this.LastMouseCell, this.CellRect(this.LastMouseCell)));
            }
            //else if (this.hotColumn != -1)
            //{
            //    this.OnHeaderDoubleClick(new HeaderMouseEventArgs(this.ColumnModel.Columns[this.hotColumn], this, this.hotColumn, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(this.hotColumn))));
            //}
        }

        #endregion

        #endregion

        #region Paint

        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{859D63A6-E7CE-42FD-BDA1-C0CCA0B6CB77}</MetaDataID>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            //this.OnPaintBorder(e);
            //this.OnPaintActions(e);
        }



        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{41F740CD-1C68-4B41-AB78-354E8064B3BD}</MetaDataID>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                //Debug.WriteLine("On paint");
                // we'll do our own painting thanks
                //base.OnPaint(e);
                
                // check if we actually need to paint
                if (this.Width == 0 || this.Height == 0)
                {
                    return;
                }
                
                this.UpdateLongActionsRect();
                
                

                if (this.ColumnModel != null)
                {
                    // keep a record of the current clip region
                    Region clip = e.Graphics.Clip;

                    if (this.TableModel != null && this.TableModel.TotalRows.Count > 0)
                    {
                        this.OnPaintTimeRows(e);
                        //this.OnPaintRows(e);
                        
                        // reset the clipping region
                        e.Graphics.Clip = clip;
                    }

                    if (this.GridLines != GridLines.None)
                    {
                        this.OnPaintGrid(e);
                    }



                    //if (this.HeaderStyle != ColumnHeaderStyle.None && this.ColumnModel.Columns.Count > 0)
                    //{
                    //    if (this.HeaderRectangle.IntersectsWith(e.ClipRectangle))
                    //    {
                    //        this.OnPaintHeader(e);
                    //    }
                    //}

                    // reset the clipping region
                    e.Graphics.Clip = clip;
                }

                //this.OnPaintEmptyTableText(e);

                this.OnPaintBorder(e);
                this.OnPaintActions(e);
                this.OnPaintHeader(e);
                this.OnPaintLongActionsRect(e);
                this.OnPaintLongActions(e);
            }
            catch (System.Exception error)
            {


            }

        }


        /// <summary>
        /// Draws a reversible line at the specified screen x-coordinate 
        /// that is the height of the PseudoClientRect
        /// </summary>
        /// <param name="x">The screen x-coordinate of the reversible line 
        /// to be drawn</param>
        /// <MetaDataID>{ED92D71D-D264-4C27-AA8A-764719D2970A}</MetaDataID>
        private void DrawReversibleLine(int x)
        {
            Point start = this.PointToScreen(new Point(x, this.PseudoClientRect.Top));

            ControlPaint.DrawReversibleLine(start, new Point(start.X, start.Y + this.PseudoClientRect.Height), this.BackColor);
        }

        #region Border

        /// <summary>
        /// Paints the Table's border
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{C53F8515-2ADE-4D84-8978-32CEEDD6E0DF}</MetaDataID>
        protected void OnPaintBorder(PaintEventArgs e)
        {
            e.Graphics.SetClip(e.ClipRectangle);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);

            if (this.BorderStyle == BorderStyle.Fixed3D)
            {
                #region BorderStyle.Fixed3D IN COMMENTS
                //if (ThemeManager.VisualStylesEnabled)
                //{
                //    TextBoxStates state = TextBoxStates.Normal;
                //    if (!this.Enabled)
                //    {
                //        state = TextBoxStates.Disabled;
                //    }

                //    // draw the left border
                //    Rectangle clipRect = new Rectangle(0, 0, SystemInformation.Border3DSize.Width, this.Height);
                //    if (clipRect.IntersectsWith(e.ClipRectangle))
                //    {
                //        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                //    }

                //    // draw the top border
                //    clipRect = new Rectangle(0, 0, this.Width, SystemInformation.Border3DSize.Height);
                //    if (clipRect.IntersectsWith(e.ClipRectangle))
                //    {
                //        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                //    }

                //    // draw the right border
                //    clipRect = new Rectangle(this.Width - SystemInformation.Border3DSize.Width, 0, this.Width, this.Height);
                //    if (clipRect.IntersectsWith(e.ClipRectangle))
                //    {
                //        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                //    }

                //    // draw the bottom border
                //    clipRect = new Rectangle(0, this.Height - SystemInformation.Border3DSize.Height, this.Width, SystemInformation.Border3DSize.Height);
                //    if (clipRect.IntersectsWith(e.ClipRectangle))
                //    {
                //        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                //    }
                //}
                //else
                //{
                //    ControlPaint.DrawBorder3D(e.Graphics, 0, 0, this.Width, this.Height, Border3DStyle.Sunken);
                //} 
                #endregion
            }
            //else if (this.BorderStyle == BorderStyle.FixedSingle)
            //{
            //    e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            //}

            //if (this.HScroll && this.VScroll)
            //{
            //    Rectangle rect = new Rectangle(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth,
            //        this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight,
            //        SystemInformation.VerticalScrollBarWidth,
            //        SystemInformation.HorizontalScrollBarHeight);

            //    if (rect.IntersectsWith(e.ClipRectangle))
            //    {
            //        e.Graphics.FillRectangle(SystemBrushes.Control, rect);
            //    }
            //}
        }

        #endregion

        #region Cells

        /// <summary>
        /// Paints the Cell at the specified row and column indexes
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <param name="row">The index of the row that contains the cell to be painted</param>
        /// <param name="column">The index of the column that contains the cell to be painted</param>
        /// <param name="cellRect">The bounding Rectangle of the Cell</param>
        /// <MetaDataID>{A2E59225-1819-4326-A3B6-7DC2A27486F1}</MetaDataID>
        protected void OnPaintCell(PaintEventArgs e, int row_index, int column, Rectangle cellRect)
        {
            if (row_index == 0 && column == 1)
            {
                column = 1;
            }

            // get the renderer for the cells column
            ICellRenderer renderer = this.ColumnModel.Columns[column].Renderer;
            if (renderer == null)
            {
                // get the default renderer for the column
                renderer = this.ColumnModel.GetCellRenderer(this.ColumnModel.Columns[column].GetDefaultRendererName());
            }

            // if the renderer is still null (which it shouldn't)
            // the get out of here
            if (renderer == null)
            {
                return;
            }

            PaintCellEventArgs pcea = new PaintCellEventArgs(e.Graphics, cellRect);
            pcea.Graphics.SetClip(Rectangle.Intersect(e.ClipRectangle, cellRect));

            if (column < this.TableModel.TotalRows[row_index].Cells.Count)
            {
                // is the cell selected
                bool selected = false;
                if (this.SelectionStyle == SelectionStyle.ListView)
                {
                    if (this.TableModel.Selections.IsRowSelected(row_index) && this.ColumnModel.PreviousVisibleColumn(column) == -1)
                    {
                        selected = true;
                    }
                }
                else if (this.SelectionStyle == SelectionStyle.Grid)
                {
                    if (this.TableModel.Selections.IsCellSelected(row_index, column))
                    {
                        Debug.WriteLine("Cell Selected");
                        selected = true;
                    }
                }
                //if (this.FullRowSelect)
                //{
                //    selected = this.TableModel.Selections.IsRowSelected(row_index);
                //}
                //else
                //{
                //    if (this.SelectionStyle == SelectionStyle.ListView)
                //    {
                //        if (this.TableModel.Selections.IsRowSelected(row_index) && this.ColumnModel.PreviousVisibleColumn(column) == -1)
                //        {
                //            selected = true;
                //        }
                //    }
                //    else if (this.SelectionStyle == SelectionStyle.Grid)
                //    {
                //        if (this.TableModel.Selections.IsCellSelected(row_index, column))
                //        {
                //            Debug.WriteLine("Cell Selected");
                //            selected = true;
                //        }
                //    }
                //}

                //
                bool editable = this.TableModel[row_index, column].Editable && this.TableModel.TotalRows[row_index].Editable && this.ColumnModel.Columns[column].Editable;
                bool enabled = this.TableModel[row_index, column].Enabled && this.TableModel.TotalRows[row_index].Enabled && this.ColumnModel.Columns[column].Enabled;

                // draw the cell
                pcea.SetCell(this.TableModel[row_index, column]);
                pcea.SetRow(row_index);
                pcea.SetColumn(column);
                pcea.SetTable(this);
                pcea.SetSelected(selected);
                pcea.SetFocused(this.Focused && this.FocusedCell.Row == row_index && this.FocusedCell.Column == column);
                //pcea.SetSorted(column == this.lastSortedColumn);
                pcea.SetEditable(editable);
                pcea.SetEnabled(enabled);
                pcea.SetCellRect(cellRect);
            }
            else
            {
                // there isn't a cell for this column, so send a 
                // null value for the cell and the renderer will 
                // take care of the rest (it should draw an empty cell)

                pcea.SetCell(null);
                pcea.SetRow(row_index);
                pcea.SetColumn(column);
                pcea.SetTable(this);
                pcea.SetSelected(false);
                pcea.SetFocused(false);
                pcea.SetSorted(false);
                pcea.SetEditable(false);
                pcea.SetEnabled(false);
                pcea.SetCellRect(cellRect);
            }

            // let the user get the first crack at painting the cell
            this.OnBeforePaintCell(pcea);

            // only send to the renderer if the user hasn't 
            // set the handled property
            if (!pcea.Handled)
            {
                renderer.OnPaintCell(pcea);
            }

            // let the user have another go
            this.OnAfterPaintCell(pcea);
        }


        /// <summary>
        /// Raises the BeforePaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{A1E368D6-2752-40EE-8C1F-ECEE9712A442}</MetaDataID>
        protected virtual void OnBeforePaintCell(PaintCellEventArgs e)
        {
            if (BeforePaintCell != null)
            {
                BeforePaintCell(this, e);
            }
        }


        /// <summary>
        /// Raises the AfterPaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{57277FD3-65D2-4BE1-86A7-CDE27E64C880}</MetaDataID>
        protected virtual void OnAfterPaintCell(PaintCellEventArgs e)
        {
            if (AfterPaintCell != null)
            {
                AfterPaintCell(this, e);
            }
        }

        #endregion

        #region Grid

        /// <summary>
        /// Paints the Table's grid
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{E5BA7573-B108-4990-A16C-245BF8C40AD2}</MetaDataID>
        protected void OnPaintGrid(PaintEventArgs e)
        {
            //DragDropMarkPos = -1;
            if (this.GridLines == GridLines.None)
            {
                return;
            }

            //
            //e.Graphics.SetClip(e.ClipRectangle);

            if (this.ColumnModel == null || this.ColumnModel.Columns.Count == 0)
            {
                return;
            }

            //e.Graphics.SetClip(e.ClipRectangle);

            if (this.ColumnModel != null)
            {                
                using (Pen gridPen = new Pen(this.RowGridColor))
                {
                    if (this.TableModel != null)
                    {
                        // check if we can draw row lines
                        if ((this.GridLines & GridLines.Rows) == GridLines.Rows)
                        {
                            Pen timepen = new Pen(this._TimeRowGridColor);
                            int rows_drawn = 0;// this.TableModel.RowsOnTime;
                            int y = this.CellDataRect.Y + this.RowHeight - 1;
                            int leftstart=e.ClipRectangle.Left+(System.Convert.ToInt32(this.ColumnModel.Columns[0].Width)/2);
                            int timestart = e.ClipRectangle.Left;
                            for (int i = y; i <= e.ClipRectangle.Bottom; i += this.RowHeight)
                            {
                                if (i >= this.CellDataRect.Top)
                                {
                                    //if (rows_drawn == this.TableModel.RowsOnTime - 1)
                                    //{
                                    //    e.Graphics.DrawLine(timepen, timestart, i, e.ClipRectangle.Right, i);//CellDataRect.Right
                                    //    rows_drawn = 0;
                                    //}
                                    //else
                                    //{
                                    //    e.Graphics.DrawLine(gridPen, leftstart, i, CellDataRect.Right, i);//
                                    //    rows_drawn++; 
                                    //}
                                    e.Graphics.DrawLine(gridPen, leftstart, i, CellDataRect.Right, i);//
                                    rows_drawn++; 
                                }
                                
                            }
                            timepen.Dispose();
                        }
                        //if (DragDropEffect != DragDropEffects.None && RectangleToScreen(this.CellDataRect).Contains(System.Windows.Forms.Control.MousePosition))
                        //{
                        //    //using (Pen cursorPen = new Pen(_CursorColor,_CursorWidth))
                        //    //{
                        //    int y = this.CellDataRect.Y + this.RowHeight - 1;

                        //    for (int i = y; i <= e.ClipRectangle.Bottom; i += this.RowHeight)
                        //    {
                        //        if (i >= this.CellDataRect.Top && i >= PointToClient(System.Windows.Forms.Control.MousePosition).Y)
                        //        {
                        //            int scrollBarWidth = 0;
                        //            if (this._VScrollBar.Visible)
                        //                scrollBarWidth = this._VScrollBar.Width;
                        //            //int leftCursorMatgin = (int)(_CursorWidth / 2)+2;
                        //            //int rightCursorMatgin = (int)(_CursorWidth / 2) + 2 + scrollBarWidth;
                        //            //if (_CursorWidth % 2 > 0)
                        //            //    rightCursorMatgin++;

                        //            e.Graphics.DrawLine(_markPen, e.ClipRectangle.Left + DragDropMarkWidth + 3, i, e.ClipRectangle.Right - scrollBarWidth - DragDropMarkWidth - 3, i);
                        //            DragDropMarkPos = i / this.RowHeight;

                        //            //   e.Graphics.DrawLine(cursorPen, e.ClipRectangle.Left + leftCursorMatgin, i - (_CursorWidth + 3), e.ClipRectangle.Left + leftCursorMatgin , i + (_CursorWidth + 3));
                        //            ////   rightCursorMatgin = 27;
                        //            //   e.Graphics.DrawLine(cursorPen, e.ClipRectangle.Right - rightCursorMatgin,i- (_CursorWidth + 3), e.ClipRectangle.Right - rightCursorMatgin,i+ (_CursorWidth + 3));
                        //            break;
                        //        }
                        //    }
                        //    //}
                        //}


                    }
                }
                using (Pen gridPen = new Pen(this.ColumnGridColor))
                {
                    //
                    gridPen.DashStyle = (DashStyle)this.GridLineStyle;

                    // check if we can draw column lines
                    if ((this.GridLines & GridLines.Columns) == GridLines.Columns)
                    {
                        int right = this.DisplayRectangle.X+1;
                        e.Graphics.DrawLine(gridPen, right-1 , e.ClipRectangle.Top+2, right-1 , e.ClipRectangle.Bottom);

                        for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
                        {
                            if (this.ColumnModel.Columns[i].Visible)
                            {
                                right += System.Convert.ToInt32(this.ColumnModel.Columns[i].Width);

                                if (right >= e.ClipRectangle.Left && right <= e.ClipRectangle.Right)
                                {
                                    e.Graphics.DrawLine(gridPen, right-1 , e.ClipRectangle.Top+2, right-1 , e.ClipRectangle.Bottom);
                                }
                            }
                        }
                    }
                }
            }
        }


        //int DragDropMarkPos = -1;
        //DragDropEffects DragDropEffect = DragDropEffects.None;

        #endregion

        #region Header

        #region protected void UpdateLongActionsRect()
        /// <MetaDataID>{69f6d63e-882e-4cd7-bced-c07ff2e03053}</MetaDataID>
        protected void UpdateLongActionsRect()
        {
            if (TableView == TableViewState.DayView)
            {
                this.LongActionsRectHeight = 23;
                return;
            }
            //int totalspace = 0;
            //int higheractionscount = 0;
            //int colactcount = 0;
            //this.ShowLongActionsScroll = false;
            LongActionsCount = 0;
            int needed_space = 5;//23 the starting height of the _LongActionsRectHeight
                
            foreach (ISchedulerAction action in this.LongActions)
            {

                //if (action.DateStart.Year <= action.DateEnd.Year &&
                //    action.DateStart.Month <= action.DateEnd.Month &&
                //    action.DateStart.Day < action.DateEnd.Day)
                //{
                    for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
                    {
                        //Insure that the actions are in the range of the columns 
                        
                        DayColumn day_column = this.ColumnModel.Columns[i] as DayColumn;
                        if (day_column != null)
                        {
                            if (day_column.IncludesAction(action))
                            {
                                needed_space += this.LongActionHeight + ActionsPadding;//3
                                //colactcount++;
                                LongActionsCount++;
                                break;
                            }
                        }
                    }
                //}
            
                

                //if (colactcount > LongActionsCount)
                //    LongActionsCount = colactcount;

                //if (totalspace < needed_space)
                //    totalspace = needed_space;


            }
            if (needed_space > _MinLongActionsRectHeight && ((this.Height - this.HeaderHeight) / 2) < needed_space)
                needed_space = (this.Height - this.HeaderHeight) / 2;

            this.LongActionsRectHeight = needed_space > _MinLongActionsRectHeight ? needed_space : _MinLongActionsRectHeight;
            //_LongActionsRectangle = new Rectangle(this._TimeColumnWidth + 1, this.HeaderHeight, this.Width - this._TimeColumnWidth + 1, this.LongActionsRectHeight);

            int totactionsrect = LongActionsCount * (this.LongActionHeight + ActionsPadding);
            if (totactionsrect > this.LongActionsRectHeight)
            {
                this.ShowLongActionsScroll = true;
            }
            else
                this.ShowLongActionsScroll = false;
        } 
        #endregion        

        #region private bool ContainsDate
        /// <MetaDataID>{fdadd1ed-51cb-4d5e-b919-a61cbd4b2f6c}</MetaDataID>//int hour, int start_min, int end_min
        private bool ContainsDate(DateTime date, Row row, bool init_startfound, out bool start_found, ISchedulerAction action)
        {
            start_found = init_startfound;
            if (date.Year >= action.DateStart.Year && date.Month >= action.DateStart.Month && date.Day >= action.DateStart.Day &&
                action.DateEnd.Year >= date.Year && action.DateEnd.Month >= date.Month && action.DateEnd.Day >= date.Day)
            {
                //if (date.Year > DateStart.Year && date.Month > DateStart.Month && date.Day > DateStart.Day)
                //    return true;

                //if (date.Year == DateStart.Year && date.Month == DateStart.Month && date.Day == DateStart.Day)
                if (action.DateEnd.Day == date.Day)
                {
                    //2 columns draw.It was in the begining when the long actions was drawn in vertical 
                    if (action.DateEnd.Day > action.DateStart.Day)
                    {
                        if (action.TimeEnd.Hour > row.Hour)
                            return true;
                        if (action.TimeEnd.Hour == row.Hour && action.TimeEnd.Minute >= row.StartMinute)
                            return true;
                        return false;
                    }
                    else
                    {
                        if (action.TimeEnd.Hour >= row.Hour && row.Hour >= action.TimeStart.Hour)
                        {
                            if (row.Hour == action.TimeStart.Hour)
                            {
                                if (row.TimeRowIncludesMinute(action.TimeStart.Minute, action.TimeEnd.Hour))
                                    return true;
                                return false;

                                //if (action.TimeStart.Minute >= row.StartMinute && action.DateStart.Minute <= row.EndMinute)
                                //{
                                //    //start_found = true;
                                //    return true;
                                //}

                                //return false;                               
                            }

                            if (action.TimeEnd.Hour == row.Hour && action.TimeEnd.Minute >= row.StartMinute)
                                return true;

                            //if(this.IsStartRowInAction(
                            //if (action.TimeStart.Hour < hour)
                            //{
                            //    return false;
                            //}

                            if (action.TimeEnd.Hour > row.Hour)
                            {
                                return true;
                            }

                            return false;
                        }
                    }
                    return false;
                }
                else
                {
                    if (date.Day > action.DateStart.Day)
                        return true;

                    if (date.Day == action.DateStart.Day)
                    {
                        //find the start
                        if (row.Hour == action.TimeStart.Hour && row.StartMinute <= action.TimeStart.Minute && action.TimeStart.Minute <= row.EndMinute)
                        {
                            start_found = true;
                            return true;
                        }
                        if (row.Hour == action.TimeStart.Hour && start_found)
                            return true;

                        if (row.Hour > action.TimeStart.Hour)
                            return true;
                        return false;
                    }
                }
                return true;
            }

            return false;
        }
        
        #endregion

        #region protected void OnPaintLongActionsRect(PaintEventArgs e)
        /// <MetaDataID>{af9f8715-fd8c-4b3f-a121-946611de0948}</MetaDataID>
        protected void OnPaintLongActionsRect(PaintEventArgs e)
        {
            //int xPos = this.DisplayRectangle.Left;

            PaintHeaderEventArgs phea = new PaintHeaderEventArgs(e.Graphics, e.ClipRectangle);

            //this.UpdateScrollBars();

            for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
            {
                int xPos = System.Convert.ToInt32(this.ColumnModel.Columns[i].X + this.BorderWidth);
                if (i == 0)
                    this._HeaderRenderer.Bounds = new Rectangle(xPos, this.BorderWidth, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), (this.LongActionsRectHeight + this.HeaderHeight));
                else
                {
                    this._LongActionsRectRenderer.Bounds = new Rectangle(xPos, this.BorderWidth + this.HeaderHeight, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), this.LongActionsRectHeight);
                    phea.SetDayRect(_LongActionsRectRenderer.Bounds);
                }

                Rectangle ClipRect = new Rectangle(xPos, this.BorderWidth, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), (this.LongActionsRectHeight + this.HeaderHeight + 1));
                phea.Graphics.SetClip(Rectangle.Intersect(e.ClipRectangle, ClipRect));//this.headerRenderer.Bounds

                if (i == 0)
                {
                    phea.SetHeaderRect(this._HeaderRenderer.Bounds);
                    _HeaderRenderer.OnPaintHeader(phea);
                }
                else
                    this._LongActionsRectRenderer.OnPaintHeader(phea);

                phea.Graphics.ResetClip();

            }

        }
        
        #endregion

        #region protected void OnPaintHeader(PaintEventArgs e)
        /// <summary>
        /// Paints the Table's Column headers
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{360E4B41-60BA-4855-BC64-595B2B6F3C1B}</MetaDataID>
        protected void OnPaintHeader(PaintEventArgs e)
        {
            // only bother if we actually get to paint something
            if (!this.HeaderRectangle.IntersectsWith(e.ClipRectangle))
            {
                return;
            }

            int xPos = this.DisplayRectangle.Left;
            //bool needDummyHeader = true;

            PaintHeaderEventArgs phea = new PaintHeaderEventArgs(e.Graphics, e.ClipRectangle);

            for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
            {
                // check that the column isn't hidden
                if (this.ColumnModel.Columns[i].Visible)
                {
                    Rectangle colHeaderRect = new Rectangle(xPos, this.BorderWidth, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), this.HeaderHeight);

                    //// check that the column intersects with the clipping rect
                    if (e.ClipRectangle.IntersectsWith(colHeaderRect))
                    {
                        // move and resize the headerRenderer                        
                        if (i == 0)
                            this._HeaderRenderer.Bounds = new Rectangle(xPos, this.BorderWidth, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), (this.LongActionsRectHeight + this.HeaderHeight));
                        else
                        {
                            this._HeaderRenderer.Bounds = new Rectangle(xPos, this.BorderWidth, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), this.HeaderHeight);
                            decimal w = (this.ColumnModel.Columns[i].Width / 3) + 10;
                            Rectangle detaildayviewRect = new Rectangle(xPos + (System.Convert.ToInt32(this.ColumnModel.Columns[i].Width)-18), this.BorderWidth + (this.HeaderHeight / 2), 16,16);
                            //phea.DetailDayViewRectange = detaildayviewRect;
                            (this.ColumnModel.Columns[i] as DayColumn).DetailDayViewRectange = detaildayviewRect;
                        }
                        // set the clipping area to the header renderers bounds
                        Rectangle ClipRect = new Rectangle(xPos, this.BorderWidth, System.Convert.ToInt32(this.ColumnModel.Columns[i].Width), (this.LongActionsRectHeight + this.HeaderHeight + 1));
                        phea.Graphics.SetClip(Rectangle.Intersect(e.ClipRectangle, ClipRect));//this.headerRenderer.Bounds

                        // draw the column header
                        phea.SetColumn(this.ColumnModel.Columns[i]);
                        phea.SetColumnIndex(i);
                        phea.SetTable(this);
                        //phea.SetHeaderStyle(this.HeaderStyle);
                        phea.SetHeaderRect(this._HeaderRenderer.Bounds);
                        //phea.SetDayRect(_LongActionsRectRenderer.Bounds);

                        // let the user get the first crack at painting the header
                        this.OnBeforePaintHeader(phea);

                        // only send to the renderer if the user hasn't 
                        // set the handled property
                        if (!phea.Handled)
                        {
                            this._HeaderRenderer.OnPaintHeader(phea);
                            //this._LongActionsRectRenderer.OnPaintHeader(phea);
                        }
                        phea.Graphics.ResetClip();
                        // let the user have another go
                        this.OnAfterPaintHeader(phea);
                    }

                    // set the next column start position
                    xPos += System.Convert.ToInt32(this.ColumnModel.Columns[i].Width);

                    // if the next start poition is past the right edge
                    // of the clipping rectangle then we don't need to
                    // draw anymore
                    if (xPos >= e.ClipRectangle.Right)
                    {
                        return;
                    }
                  
                }
            }            
        }
        
        #endregion

        #region OnBeforePaintHeader(PaintHeaderEventArgs e),OnAfterPaintHeader(PaintHeaderEventArgs e)
        /// <summary>
        /// Raises the BeforePaintHeader event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{60CF8972-3F5F-4ABE-8F0E-0C703879B26C}</MetaDataID>
        protected virtual void OnBeforePaintHeader(PaintHeaderEventArgs e)
        {
            if (BeforePaintHeader != null)
            {
                BeforePaintHeader(this, e);
            }
        }


        /// <summary>
        /// Raises the AfterPaintHeader event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{3F393A1F-A501-4EBF-ADDB-FA0EA40D5D29}</MetaDataID>
        protected virtual void OnAfterPaintHeader(PaintHeaderEventArgs e)
        {
            if (AfterPaintHeader != null)
            {
                AfterPaintHeader(this, e);
            }
        } 
        #endregion

        #endregion

        #region TotalRows

        #region protected void OnPaintTimeRows(PaintEventArgs e)
        /// <MetaDataID>{a3a0c7c4-606d-4700-9016-26cedc1539d3}</MetaDataID>
        protected void OnPaintTimeRows(PaintEventArgs e)
        {
            int xPos = this.DisplayRectangle.Left;
            int yPos = this.PseudoClientRect.Top;
            yPos += this.LongActionsRectHeight + this.HeaderHeight + 1;

            //if (this.HeaderStyle != ColumnHeaderStyle.None)
            //{
            //    yPos += this.LongActionsRectHeight + this.HeaderHeight + 1; 
            //}

            Rectangle rowRect = new Rectangle(xPos, yPos, System.Convert.ToInt32(this.ColumnModel.TotalColumnWidth), this.RowHeight);


            for (int i = this.TopIndex; i < Math.Min(this.TableModel.TotalRowCount, this.TopIndex + this.VisibleRowCount + 1); i++)
            {
                if (rowRect.IntersectsWith(e.ClipRectangle))
                {
                    this.OnPaintRow(e, i, rowRect);
                }
                else if (rowRect.Top > e.ClipRectangle.Bottom)
                {
                    break;
                }

                // move to the next row
                rowRect.Y += this.RowHeight;
            }
        } 
        #endregion

        //#region protected void OnPaintRows(PaintEventArgs e)
        ///// <summary>
        ///// Paints the Table's TotalRows
        ///// </summary>
        ///// <param name="e">A PaintEventArgs that contains the event data</param>
        ///// <MetaDataID>{E950F81F-6CE1-49CC-93A2-B249AB21684B}</MetaDataID>
        //protected void OnPaintRows(PaintEventArgs e)
        //{
        //    int xPos = this.DisplayRectangle.Left;
        //    int yPos = this.PseudoClientRect.Top;

        //    if (this.HeaderStyle != ColumnHeaderStyle.None)
        //    {
        //        yPos += this.HeaderHeight;
        //    }

        //    Rectangle rowRect = new Rectangle(xPos, yPos, System.Convert.ToInt32(this.ColumnModel.TotalColumnWidth), this.RowHeight);

        //    for (int i = this.TopIndex; i < Math.Min(this.TableModel.TotalRows.Count, this.TopIndex + this.VisibleRowCount + 1); i++)
        //    {
        //        if (rowRect.IntersectsWith(e.ClipRectangle))
        //        {
        //            this.OnPaintRow(e, i, rowRect);
        //        }
        //        else if (rowRect.Top > e.ClipRectangle.Bottom)
        //        {
        //            break;
        //        }

        //        // move to the next row
        //        rowRect.Y += this.RowHeight;
        //    }


        //    if (this.IsValidColumn(this.lastSortedColumn))
        //    {
        //        if (rowRect.Y < this.PseudoClientRect.Bottom)
        //        {
        //            Rectangle columnRect = this.ColumnRect(this.lastSortedColumn);
        //            columnRect.Y = rowRect.Y;
        //            columnRect.Height = this.PseudoClientRect.Bottom - rowRect.Y;

        //            if (columnRect.IntersectsWith(e.ClipRectangle))
        //            {
        //                columnRect.Intersect(e.ClipRectangle);

        //                e.Graphics.SetClip(columnRect);

        //                using (SolidBrush brush = new SolidBrush(this.SortedColumnBackColor))
        //                {
        //                    e.Graphics.FillRectangle(brush, columnRect);
        //                }
        //            }
        //        }
        //    }
        //}
        
        //#endregion

        /// <MetaDataID>{09575bd5-4f2d-4676-8352-11340ea32bf9}</MetaDataID>
        protected void OnPaintLongActions(PaintEventArgs e)
        {
            if (this.Path == null)
                return;

            if (TableView == TableViewState.DayView)
                return;            

            int action_rectX = 0;
            bool action_open = false;
            int action_index = -1;
            //hold the views to check thme  in the end to find if they have the same Y Position
            //List<SchedulerActionView> action_views = new List<SchedulerActionView>();
            //int acypos = 0;
            foreach (ISchedulerAction action in this.LongActions)
            {
                //int new_height=0;
                
                //action_views.Clear();
                action_index++;

                #region Columns for
                for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                {
                    DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                    if (day_column.IncludesAction(action))
                    {
                        int row_rectY = this.PseudoClientRect.Top + this.HeaderHeight + 4;
                        //int needed_space = 23;//the starting height of the _LongActionsRectHeight

                        action_rectX = System.Convert.ToInt32(day_column.X) + 5;
                        //if the action exists returns its position,otherwise returns a the height of the LongActionHeight
                        int action_ypos = day_column.GetYPositionForAction(action);//, out new_height
                        if (action_ypos != 0)
                        {
                            row_rectY = action_ypos;
                            //needed_space = action_ypos;
                        }
                        else
                        {
                            row_rectY += action_index * this.LongActionHeight;
                            //row_rectY += new_height + acypos;
                            ////needed_space += new_height;
                        }                       

                        Rectangle action_rect = new Rectangle(action_rectX, row_rectY, System.Convert.ToInt32(day_column.Width) - 5, LongActionHeight);

                        if (day_column.ContainsActionViewForAction(action,true))
                        {
                            day_column.UpdateLongActionView(action, action_rect);
                        }
                        else
                        {
                            SchedulerActionView action_view = null;

                            action_view = new SchedulerActionView(action, action_rect);
                            action_view.Index = action_index;

                            if (FocusedActionViews!=null && FocusedActionViews.Count != 0)
                            {
                                action_view.Focus = true;
                                FocusedActionViews.Add(action_view);
                            }

                            day_column.AddSchedulerActionView(action_view);
                            //action_views.Add(action_view);

                            if (action_open == false)
                            {
                                action_open = true;
                                if (action.DateStart.Date == day_column.Date.Date)
                                {
                                    action_view.ViewState = LongActionViewState.Start;
                                    //string nam = action.PerformerName.Substring(0, 4) + ".";
                                    //if (!string.IsNullOrEmpty(action.PerformerLastName))
                                    //    nam += action.PerformerLastName.Substring(0, 4);
                                    string nam = action.PerformerName + " ";
                                    nam += action.PerformerLastName;
                                    action_view.StringToDraw = nam;
                                    continue;
                                }

                                if (action.DateEnd.Date == day_column.Date.Date)
                                {
                                    action_view.ViewState = LongActionViewState.End;
                                    continue;
                                }
                            }


                            if (action_open)
                            {
                                if (day_column.Date.Date == action.DateEnd.Date)
                                {
                                    action_open = false;
                                    action_view.ViewState = LongActionViewState.End;
                                    continue;
                                }

                                if (day_column.Date.Date == action.DateStart.Date)
                                {
                                    action_open = false;
                                    action_view.ViewState = LongActionViewState.Start;
                                    continue;
                                }

                                action_view.ViewState = LongActionViewState.Middle;
                            }

                        }


                    }                    
                }
                ////
                //int avypos = 0;
                //foreach (SchedulerActionView av in action_views)
                //{                        
                //    if (avypos < av.ActionRectangle.Y)
                //        avypos = av.ActionRectangle.Y;
                //}

                //foreach (SchedulerActionView av in action_views)
                //{
                //    av.ActionRectangle.Y = avypos;
                //}
                //set the Yposition of the next action
                //acypos += ActionsPadding;
            
                #endregion
            }

            int ypos = this.PseudoClientRect.Top + this.HeaderHeight + 4;

            for (int i = TopLongActionIndex; i < Math.Min(LongActionsCount, TopLongActionIndex + VisibleLongActionsCount); i++)
            {
                for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                {
                    DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                    day_column.OnPaintLongActions(ActionRenderer, this.CellDataRect, e.Graphics, i, ypos);
                    //day_column.ClearActionPaints();
                }
                ypos += this.LongActionHeight + this.ActionsPadding;
            }
        }

        ///// <MetaDataID>{0e2bbb9f-9316-4a69-b75d-44f945191a53}</MetaDataID>
        //public void InvalidateActionRect(Rectangle rect)
        //{
        //    this.Invalidate(rect);
        //}

        /// <MetaDataID>{58e6a58b-b21e-4516-99dd-4d6876c2725b}</MetaDataID>
        protected void OnPaintActions(PaintEventArgs e)
        {
            if (this.Path == null)
                return;

            int row_rectY = this.PseudoClientRect.Top;
            row_rectY += this.LongActionsRectHeight + this.HeaderHeight;

            bool action_open = false;
            bool action_start_found = false;
            //int action_index = -1;

            foreach (ISchedulerAction action in this.Actions)
            {
                action_open = false;
                action_start_found = false;

                //if (action.DateStart.Year == action.DateEnd.Year &&
                //    action.DateStart.Month == action.DateEnd.Month &&
                //    action.DateStart.Day == action.DateEnd.Day)
                //{
                    //action_index++;
                    for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                    {
                        DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                        if (day_column.IncludesAction(action))
                        {
                            if (this.TableView != TableViewState.DayView && day_column.NumOfActionsThatCanBeDrawn > day_column.DayActionViewsCount)
                                SetTimeActionRect(e, action, day_column, row_rectY, action_open, out action_open, out action_start_found);
                            else if(this.TableView == TableViewState.DayView)
                                SetTimeActionRect(e, action, day_column, row_rectY, action_open, out action_open, out action_start_found);
                        }
                        //else if (day_column.ContainsActionViewForAction(action, false))
                        //    day_column.DeleteActionViewForAction(action, false);

                    }
                //}
            }
            //this.Invalidate();
            for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
            {
                DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                day_column.OnPaintActions(ActionRenderer, this.CellDataRect, e.Graphics);
            }          
        }

        /// <MetaDataID>{5d10543c-b016-4277-b88c-360b3fde2429}</MetaDataID>
        protected void SetTimeActionRect(PaintEventArgs e, ISchedulerAction action, DayColumn day_column, int row_rectY, bool isaction_open, out bool action_open, out bool action_start_found)
        {
            int action_rectX = 0;
            int action_rectY = 0;
            int action_rectWidth = 0;
            int action_rectHeight = 0;
            bool action_started = false;
            int rowrecty = row_rectY;
            //if we dont put these properties here we must put them in the ISchedulerAction.Not Acceptable
            action_open = isaction_open;
            action_start_found = isaction_open;

            #region Find where the Action begins
            for (int i = this.TopIndex; i < Math.Min(this.TableModel.TotalRowCount, this.TopIndex + this.VisibleRowCount + 1); i++)
            {
                Row hour_row = this.TableModel.TotalRows[i];
                if (ContainsDate(day_column.Date, 
                    hour_row, 
                    action_start_found,
                    out action_start_found, 
                    action))
                {
                    if (!action_open)
                    {
                        action_rectX = System.Convert.ToInt32(day_column.X)+5;
                        //action_rectY = row_rectY;
                        if (IsStartRowInAction(action, hour_row) || TopIndex == 0)
                        {
                            action_rectY = row_rectY + 5;
                            action_rectHeight += this.RowHeight - 10;
                        }
                        else
                        {
                            action_rectY = row_rectY - 5;
                            action_rectHeight += this.RowHeight-2;
                        }
                        action_rectWidth = System.Convert.ToInt32(day_column.Width);
                        //action_rectHeight += this.RowHeight;
                        action_started = true;
                        action_open = true;
                    }
                    else
                        action_rectHeight += this.RowHeight;
                }
                

                // move to the next row
                
                row_rectY += this.RowHeight;
            } 
            #endregion

            #region This section is used when the Action expands in more than 1 day
            if (!action_started)
            {
                //This section is used when the Action expands in more than 1 day
                int row_Y = this.PseudoClientRect.Top;
                action_rectHeight = 0;
                row_Y += this.LongActionsRectHeight + HeaderHeight;
                //if (this.HeaderStyle != ColumnHeaderStyle.None)
                //{
                //    row_Y += this.HeaderHeight * 2;
                //}
                for (int i = this.TopIndex; i < Math.Min(this.TableModel.TotalRowCount, this.TopIndex + this.VisibleRowCount + 1); i++)
                {
                    Row hour_row = this.TableModel.TotalRows[i];
                    if (ContainsDate(day_column.Date, hour_row, action_start_found, out action_start_found, action))
                    {
                        action_rectX = System.Convert.ToInt32(day_column.X);
                        if (IsStartRowInAction(action, hour_row) || TopIndex == 0)
                            action_rectY = row_Y;
                        else
                            action_rectY = row_Y - 7;
                        //if(TopIndex==0)
                        //    action_rectY = row_Y;
                        //else
                        //    action_rectY = row_Y-7;
                        //    top_rowTime_visible=hour_row.RowTime;
                        //    action_rectY = row_Y;
                        //else
                        //if(action.ContainsDate(day_column.Date,hour_row.RowTime,hour_row.StartMinute,hour_row.EndMinute)
                        //    action_rectY = row_Y;
                        action_rectWidth = System.Convert.ToInt32(day_column.Width);
                        action_rectHeight += this.RowHeight;
                        action_open = true;
                        //Debug.WriteLine(action_rectY.ToString());
                    }

                    //row_rectY += this.RowHeight;
                }

            } 
            #endregion

            Rectangle action_rect = new Rectangle(action_rectX, action_rectY, action_rectWidth, action_rectHeight);

            if (day_column.ContainsActionViewForAction(action,false))
            {
                day_column.UpdateActionViewRect(action, action_rect);               
            }
            else
            {                
                //if (action_rect.IsEmpty)//the row is not visible
                //    return;
                SchedulerActionView action_view = new SchedulerActionView(action, action_rect);
                
                string nam = action.PerformerName + " ";
                nam += action.PerformerLastName;
                action_view.StringToDraw = nam;

                day_column.AddSchedulerActionView(action_view);                

                if (ShowDetailDayScroll)
                    this.UpdateDetailDayScroll();
                //invalidate rect with shadow 
                Rectangle r = new Rectangle(action_rect.X, action_rect.Y, action_rect.Width + 2, action_rect.Height + 4);
                this.Invalidate(r);             
            }
             
        }

        /// <summary>
        /// Chackes if the first row contains the action
        /// </summary>
        /// <param name="act"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <MetaDataID>{bc9f3985-2223-4cf8-beba-26b6f19ae3fb}</MetaDataID>
        private bool IsStartRowInAction(ISchedulerAction act, Row row)
        {
            if (act.DateStart.Hour == row.Hour && act.DateStart.Minute >= row.StartMinute && act.DateStart.Minute <= row.EndMinute)
                return true;
            return false;
        }

        #region protected void OnPaintRow(PaintEventArgs e, int row, Rectangle rowRect)
        /// <summary>
        /// Paints the Row at the specified index
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <param name="row">The index of the Row to be painted</param>
        /// <param name="rowRect">The bounding Rectangle of the Row to be painted</param>
        /// <MetaDataID>{6E4EB320-E388-4775-913B-CCDEF34B2CF0}</MetaDataID>
        protected void OnPaintRow(PaintEventArgs e, int row, Rectangle rowRect)
        {
            Rectangle cellRect = new Rectangle(rowRect.X, rowRect.Y, 0, rowRect.Height);

            e.Graphics.SetClip(rowRect);

            for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
            {
                if (this.ColumnModel.Columns[i].Visible)
                {
                    cellRect.Width = System.Convert.ToInt32(this.ColumnModel.Columns[i].Width);

                    if (cellRect.IntersectsWith(e.ClipRectangle))
                    {
                        this.OnPaintCell(e, row, i, cellRect);
                    }
                    else if (cellRect.Left > e.ClipRectangle.Right)
                    {
                        break;
                    }

                    cellRect.X += System.Convert.ToInt32(this.ColumnModel.Columns[i].Width);
                }
            }
        } 
        #endregion

        #endregion

        #endregion

        #region TotalRows

        /// <summary>
        /// Raises the RowPropertyChanged event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{E34BE75B-274D-4F08-8659-D76A44BEBDA9}</MetaDataID>
        protected internal virtual void OnRowPropertyChanged(RowEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateRow(e.Index);

                if (RowPropertyChanged != null)
                {
                    RowPropertyChanged(e.Row, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellAdded event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{9619380E-B325-488D-BB29-05AC080D8D09}</MetaDataID>
        protected internal virtual void OnCellAdded(RowEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateRow(e.Index);

                if (CellAdded != null)
                {
                    CellAdded(e.Row, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellRemoved event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{D42F284C-339C-4C91-846A-D0BC0E532449}</MetaDataID>
        protected internal virtual void OnCellRemoved(RowEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateRow(e.Index);

                if (CellRemoved != null)
                {
                    CellRemoved(this, e);
                }

                if (e.CellFromIndex == -1 && e.CellToIndex == -1)
                {
                    if (this.FocusedCell.Row == e.Index)
                    {
                        this.focusedCell = CellPos.Empty;
                    }
                }
                else
                {
                    for (int i = e.CellFromIndex; i <= e.CellToIndex; i++)
                    {
                        if (this.FocusedCell.Row == e.Index && this.FocusedCell.Column == i)
                        {
                            this.focusedCell = CellPos.Empty;

                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Scrollbars

        ///// <summary>
        ///// Occurs when the Table's horizontal scrollbar is scrolled
        ///// </summary>
        ///// <param name="sender">The object that Raised the event</param>
        ///// <param name="e">A ScrollEventArgs that contains the event data</param>
        ///// <MetaDataID>{D84FF003-2E80-4E27-B2AA-BF030461EBEF}</MetaDataID>
        //protected void OnHorizontalScroll(object sender, ScrollEventArgs e)
        //{
        //    //// stop editing as the editor doesn't move while 
        //    //// the table scrolls
        //    //if (this.IsEditing)
        //    //{
        //    //    this.StopEditing();
        //    //}

        //    if (this.CanRaiseEvents)
        //    {
        //        // non-solid row lines develop artifacts while scrolling 
        //        // with the thumb so we invalidate the table once thumb 
        //        // scrolling has finished to make them look nice again
        //        if (e.Type == ScrollEventType.ThumbPosition)
        //        {
        //            if (this.GridLineStyle != GridLineStyle.Solid)
        //            {
        //                if (this.GridLines == GridLines.Rows || this.GridLines == GridLines.Both)
        //                {
        //                    this.Invalidate(this.CellDataRect, false);
        //                }
        //            }

        //            // same with the focus rect
        //            if (this.FocusedCell != CellPos.Empty)
        //            {
        //                this.Invalidate(this.CellRect(this.FocusedCell), false);
        //            }
        //        }
        //        else
        //        {
        //            this.HorizontalScroll(e.NewValue);
        //        }
        //    }
        //}


        #region protected void OnVerticalScroll(object sender, ScrollEventArgs e)
        /// <summary>
        /// Occurs when the Table's vertical scrollbar is scrolled
        /// </summary>
        /// <param name="sender">The object that Raised the event</param>
        /// <param name="e">A ScrollEventArgs that contains the event data</param>
        /// <MetaDataID>{05003D41-C34F-401D-8A95-32CB30B03B2F}</MetaDataID>
        protected void OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            //// stop editing as the editor doesn't move while 
            //// the table scrolls
            //if (this.IsEditing)
            //{
            //    this.StopEditing();
            //}

            if (this.CanRaiseEvents)
            {
                // non-solid column lines develop artifacts while scrolling 
                // with the thumb so we invalidate the table once thumb 
                // scrolling has finished to make them look nice again
                if (e.Type == ScrollEventType.ThumbPosition)
                {
                    if (this.GridLineStyle != GridLineStyle.Solid)
                    {
                        if (this.GridLines == GridLines.Columns || this.GridLines == GridLines.Both)
                        {
                            this.Invalidate(this.CellDataRect, false);
                        }
                    }
                }
                else
                {
                    this.VerticalScroll(e.NewValue);
                    this.Invalidate();
                }
            }
        } 
        #endregion


        /// <summary>
        /// Handler for a ScrollBars GotFocus event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{9AAC450E-298F-4DFC-8D7A-E1056F430493}</MetaDataID>
        private void scrollBar_GotFocus(object sender, EventArgs e)
        {
            // don't let the scrollbars have focus 
            // (appears to slow scroll speed otherwise)
            this.Focus();
        }

        #endregion        

        #region TableModel

        /// <summary>
        /// Raises the TableModelChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{56C7CC39-A7CB-4670-9FE3-0175D2BCB5B2}</MetaDataID>
        protected internal virtual void OnTableModelChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (TableModelChanged != null)
                {
                    TableModelChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the SelectionChanged event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{F08F87D3-CD70-4CC3-A454-01B7845865D6}</MetaDataID>
        protected internal virtual void OnSelectionChanged(SelectionEventArgs e)
        {
            if (this.EditingCell != CellPos.Empty)
            {
                // don't bother if we're already editing the cell.  
                // if we're editing a different cell stop editing

                this.EditingCellEditor.StopEditing();
                this.editingCell = CellPos.Empty;
            }




            if (this.CanRaiseEvents)
            {
                if (e.OldSelectionBounds != Rectangle.Empty)
                {
                    //Rectangle invalidateRect = new Rectangle(this.DisplayRectToClient(e.OldSelectionBounds.Location), e.OldSelectionBounds.Size);
                    //invalidateRect.Y += this.LongActionsRectHeight + this.HeaderHeight ;                    
                    //this.Invalidate(invalidateRect);
                    this.Invalidate(this.CellDataRect);

                }

                if (e.NewSelectionBounds != Rectangle.Empty)
                {
                    //Rectangle invalidateRect = new Rectangle(this.DisplayRectToClient(e.NewSelectionBounds.Location), e.NewSelectionBounds.Size);
                    //invalidateRect.Y += this.LongActionsRectHeight + this.HeaderHeight;                    
                    //this.Invalidate(invalidateRect);
                    this.Invalidate(this.CellDataRect);
                }
                //ListConnection.SelectionChanged();
           
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowHeightChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{121E4D8C-D2B9-48E0-A010-FA3D1E74FFD2}</MetaDataID>
        protected internal virtual void OnRowHeightChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (RowHeightChanged != null)
                {
                    RowHeightChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowAdded event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{66608B80-A9DB-458C-8664-51451BFE680E}</MetaDataID>
        protected internal virtual void OnRowAdded(TableModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (RowAdded != null)
                {
                    RowAdded(e.TableModel, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowRemoved event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{05278530-B6E0-48DD-AC26-21DED5580967}</MetaDataID>
        protected internal virtual void OnRowRemoved(TableModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (RowRemoved != null)
                {
                    RowRemoved(e.TableModel, e);
                }
            }
        }

        #endregion

        #endregion

        
        #region IObjectMemberViewControl Members

        #region public Object MetaData
        /// <MetaDataID>{d78e95c5-ee5c-4704-840c-eef86cdcab51}</MetaDataID>
        XDocument MetaDataAsXmlDocument;
        /// <MetaDataID>{bece4623-50fd-4091-a22d-c33fe735e74b}</MetaDataID>
        //public OOAdvantech.UserInterface.ListView ListViewMetaData;
        /// <MetaDataID>{00281ae7-1fd9-404a-98b4-ee279e9cfe24}</MetaDataID>
        private object _MetaData;
        /// <MetaDataID>{5ED59BC4-4269-41B8-B1EE-C5262355FFED}</MetaDataID>
        [Browsable(false)]
        public Object MetaData
        {
            get
            {
                if (MetaDataAsXmlDocument == null)
                {
                    MetaDataAsXmlDocument = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    ActionMenu = storage.NewObject(typeof(OOAdvantech.UserInterface.MenuCommand)) as OOAdvantech.UserInterface.MenuCommand;
                    ActionMenu.Name = "SchedulerListViewMenu";
                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = new UserInterfaceMetaData.MetaDataValue(MetaDataAsXmlDocument.ToString(), "(Tree MetaData)");
                metaDataVaue.MetaDataAsObject = this;



                return metaDataVaue;
            }
            set
            {

                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;



                if (MetaDataAsXmlDocument == null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    MetaDataAsXmlDocument = new XDocument();
                    try
                    {

                        if (!string.IsNullOrEmpty(metaData))
                            MetaDataAsXmlDocument = XDocument.Parse(metaData);

                    }
                    catch (System.Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            MetaDataAsXmlDocument = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                            ActionMenu = storage.NewObject(typeof(OOAdvantech.UserInterface.MenuCommand)) as OOAdvantech.UserInterface.MenuCommand;
                            ActionMenu.Name = "SchedulerListViewMenu";
                        }

                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        MetaDataAsXmlDocument = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        ActionMenu = storage.NewObject(typeof(OOAdvantech.UserInterface.MenuCommand)) as OOAdvantech.UserInterface.MenuCommand;
                        ActionMenu.Name = "SchedulerListViewMenu";
                    }
                    try
                    {

                        OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT action_menu FROM OOAdvantech.UserInterface.MenuCommand action_menu WHERE action_menu.Name = 'SchedulerListViewMenu' ");
                        foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                        {
                            ActionMenu = setInstance["action_menu"] as OOAdvantech.UserInterface.MenuCommand;
                            break;
                        }
                        if (ActionMenu == null)
                        {
                            ActionMenu = storage.NewObject(typeof(OOAdvantech.UserInterface.MenuCommand)) as OOAdvantech.UserInterface.MenuCommand;
                            ActionMenu.Name = "SchedulerListViewMenu";
                        }
                    }

                    catch (System.Exception error)
                    {
                        throw;
                    }

                    return;

                }
            }
        }
        
        #endregion

        #region public object DesignMenu
        /// <MetaDataID>{8780eb6f-a2d2-4b70-8333-453c1a99b109}</MetaDataID>
        [Editor(typeof(ConnectableControls.PropertyEditors.EditMenuMetadata), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object DesignMenu
        {
            get
            {
                return Menu;
            }
            set
            {
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                if (Menu != null)
                    Menu.OwnerControl = this;

            }
        } 
        #endregion

        /// <MetaDataID>{95120a3e-0480-42ae-9a1d-c91985e3ca71}</MetaDataID>
        private OOAdvantech.UserInterface.MenuCommand RowMenu;

        /// <MetaDataID>{a027bdce-d650-4b8b-b49f-d3f4046cb85d}</MetaDataID>
        void OnViewMenu(object sender, EventArgs e)
        { 
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
            {

                //if (Menu.MenuCommands.Count > 0)
                {
                    ConnectableControls.Menus.PopupMenu popupMenu = new ConnectableControls.Menus.PopupMenu(false);

                    RowMenu = new OOAdvantech.UserInterface.MenuCommand(ActionMenu);///ListViewMetaData.Menu

                    if (BeforeShowContextMenuOperationCaller != null && BeforeShowContextMenuOperationCaller.Operation != null)
                        BeforeShowContextMenuOperationCaller.Invoke();

                    if (RowMenu.SubMenuCommands.Count > 0)
                    {
                        ConnectableControls.Menus.MenuCommand menu = new ConnectableControls.Menus.MenuCommand(RowMenu, this);
                        if (Site == null || !Site.DesignMode)
                        {
                            foreach (ConnectableControls.Menus.MenuCommand menuCommand in menu.GetAllMenuCommands())
                                menuCommand.Click += new EventHandler(MenuCommandClicked);

                        }
                        int returnDir = 0;
                        //Menu.Command
                        popupMenu.TrackPopup(
                            System.Windows.Forms.Control.MousePosition,
                            System.Windows.Forms.Control.MousePosition,
                            ConnectableControls.Menus.Common.Direction.Horizontal,
                            menu,
                            0,
                            ConnectableControls.Menus.GapPosition.None, false, null, false, ref returnDir);

                        if (Site == null || !Site.DesignMode)
                        {
                            foreach (ConnectableControls.Menus.MenuCommand menuCommand in menu.GetAllMenuCommands())
                                menuCommand.Click -= new EventHandler(MenuCommandClicked);

                        }

                    }

                }

            }
        }

        #region private void MenuCommandClicked(object sender, EventArgs e)
        /// <MetaDataID>{7e6af5ae-55bb-4e1a-8cc3-5422d90a5abd}</MetaDataID>
        private void MenuCommandClicked(object sender, EventArgs e)
        {
            ConnectableControls.Menus.MenuCommand menucommand = sender as ConnectableControls.Menus.MenuCommand;

            if (menucommand.OnCommandOperationCaller != null)
            {
                if (UserInterfaceObjectConnection == null)
                    return;
                menucommand.OnCommandOperationCaller.ExecuteOperationCall();
            }
            else
                menucommand.Command.MenuClicked(GetPropertyValue("SelectedAction"));            
        } 
        #endregion
        

        //protected OOAdvantech.ExtensionProperties Properties;
        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{9184EE3C-0FDF-4610-8EEA-70A8F31CB0BF}</MetaDataID>
        //private OOAdvantech.UserInterface.MenuCommand _MenuScheduler;
        ///// <MetaDataID>{3BF1CF90-3728-43EF-B337-46B802FAECDC}</MetaDataID>
        //[Association("SchedulerMenu", typeof(OOAdvantech.UserInterface.MenuCommand), Roles.RoleA, "{A33BB9E0-B100-4213-92E9-6BDD0CD676C7}")]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        //[PersistentMember("_MenuScheduler")]
        //[RoleAMultiplicityRange(0, 1)]
        //[RoleBMultiplicityRange(0)]
        //public OOAdvantech.UserInterface.MenuCommand MenuScheduler
        //{
        //    get
        //    {
        //        if (_MenuScheduler == null)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _MenuScheduler = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(MenuCommand)) as MenuCommand;
        //                _MenuScheduler.Name = "ListViewMainMenu";
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //        return _MenuScheduler;
        //    }
        //    set
        //    {
        //        if (_MenuScheduler != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _MenuScheduler = value;
        //                stateTransition.Consistent = true;
        //            }


        //        }
        //    }
        //}
        

        #region public OOAdvantech.MetaDataRepository.Classifier CollectionObjectType
        /// <MetaDataID>{c74b528e-a841-4b7a-a50c-e9ee55d01f5b}</MetaDataID>
        //OOAdvantech.MetaDataRepository.Classifier _CollectionObjectType;
        ///// <MetaDataID>{c7a9fb23-6b90-40ed-a266-9c043cde8a4a}</MetaDataID>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public OOAdvantech.MetaDataRepository.Classifier CollectionObjectType
        //{
        //    get
        //    {
        //      if (_CollectionObjectType == null)
        //        {
        //        //    if (_CollectionObjectType == null && !string.IsNullOrEmpty(_Path) && _Path.IndexOf("Control: ") == 0)
        //        //    {
        //        //        IObjectMemberViewControl control = UserInterfaceObjectConnection.GetControlWithName(_Path.Replace("Control: ", "")) as IObjectMemberViewControl;
        //        //        if (control != null)
        //        //        {
        //        //            _CollectionObjectType = control.ValueType;
        //        //            if (_CollectionObjectType == null)
        //        //                return null;



        //        //            foreach (OOAdvantech.MetaDataRepository.Operation operation in control.ValueType.GetOperations("GetEnumerator"))
        //        //            {
        //        //                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
        //        //                if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
        //        //                {
        //        //                    _CollectionObjectType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
        //        //                    break;
        //        //                }
        //        //            }


        //        //        }
        //        //    }
        //        //    else 
        //                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path))
        //            {
        //                OOAdvantech.MetaDataRepository.Classifier type = _ViewControlObject.GetClassifier(_Path as string);
        //                if (type != null && type.IsBindedClassifier)
        //                    _CollectionObjectType = type.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
        //            }                    
        //        }
        //        return _CollectionObjectType;

        //    }
        //}
        #endregion

        #region public object Path
        /// <MetaDataID>{dd390ef2-b4bc-4290-8464-60c9e2d46070}</MetaDataID>
        private string _Path;
        /// <MetaDataID>{27a858ff-85de-49a4-ae4a-85fc5a6bc3ab}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor)), Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                string newPath = null;
                if (value is MetaData)
                    newPath = (value as MetaData).Path;
                if (value is string)
                    newPath = value as string;


                if ((_Path) != (newPath))
                {
                    if (_ViewControlObject != null)
                    {
                        if (_Path != null)
                            _MetaData = null;
                    }

                    _Path = newPath;
                }
            }
        }
        
        #endregion



        bool _AllowDrag = false;
        /// <MetaDataID>{8f1dc4a3-760b-43d0-9e3e-da508b0b7dad}</MetaDataID>
        [Browsable(false)]
        public bool AllowDrag
        {

            get
            {
                return _AllowDrag;
            }
            set
            {
                _AllowDrag = value; ;
            }
        }

          
        /// <MetaDataID>{76edb2df-1c51-431f-83a5-4877e51fb70f}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{b154c197-8be3-4b68-bf0a-10639084bc31}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.Name == "Path" && UserInterfaceObjectConnection != null)
            {
                if (context.PropertyDescriptor.Name == "Path")
                    return UserInterfaceObjectConnection.PresentationObjectType;
                else
                    return AssemblyManager.GetActiveWindowProject();
            }

            if (context.PropertyDescriptor.Name == "SelectionMember")// || context.PropertyDescriptor.Name == "WeekStartsMember"
               // || context.PropertyDescriptor.Name == "WorkTimeStartsMember")
            {
                if (UserInterfaceObjectConnection != null)
                    return UserInterfaceObjectConnection.PresentationObjectType;
            }
            return null;
        }

        /// <MetaDataID>{76ce4d06-903e-42f9-899e-bf3071ecb9b5}</MetaDataID>
        [Browsable(false)]
        public bool ConnectedObjectAutoUpdate
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        #region public object Value
        /// <MetaDataID>{0e68e067-a3cc-4f16-abe0-65c151e57740}</MetaDataID>
        object _Value;
        /// <MetaDataID>{1485b662-bf61-4e85-bef4-340d3c19d2ce}</MetaDataID>
        [Category("Object Model Connection")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                if (value == null)
                    return;
            }
        } 
        #endregion

        /// <MetaDataID>{9ee6bce4-63fb-4835-81e0-659924604ca9}</MetaDataID>
        [Browsable(false)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get { return null; }//CollectionObjectType
        }

        #region public ConnectableControls.Menus.MenuCommand Menu
        /// <MetaDataID>{E950000D-2BB8-4217-BC9E-242747F0DE1B}</MetaDataID>
        ConnectableControls.Menus.MenuCommand _Menu = null;

        /// <MetaDataID>{196e9385-b441-4248-bbb6-a52ff514126f}</MetaDataID>
        OOAdvantech.UserInterface.MenuCommand ActionMenu;
        /// <MetaDataID>{c01eb2df-5020-404a-a664-6cf392697a0f}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectableControls.Menus.MenuCommand Menu
        {
            get
            {
                if (_Menu == null)
                {
                    object metadata = MetaData;//Load meta data creates list view
                    _Menu = new ConnectableControls.Menus.MenuCommand(ActionMenu, this);
                    if (Site == null || !Site.DesignMode)
                    {
                        //foreach (ConnectableControls.Menus.MenuCommand menuCommand in _Menu.GetAllMenuCommands())
                        //    menuCommand.Click += new EventHandler(MenuCommandClicked);


                    }
                }
                return _Menu;
            }
        } 
        #endregion

        /// <MetaDataID>{feca4902-5b70-44cf-9216-fe5b33507915}</MetaDataID>
        public void LoadControlValues()
        {
            //System.DateTime.Now.WeekStartDate();
            //System.DateTime myDate;
            //myDate.WeekStartDate();
            

            _Actions.Clear();
            _LongActions.Clear();
            this.ColumnModel.ClearViewsFromColumns();
            if (FocusedActionViews != null && FocusedActionViews.Count > 0)
                FocusedActionViews.Clear();

            if (string.IsNullOrEmpty(_Path))
                return;

            object objectCollection = null;
            bool returnValueAsCollection = false;
            {
                objectCollection = UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
            }
            if (!returnValueAsCollection || objectCollection == null)
                return;
            //_Actions.AddRange(objectCollection as IEnumerable<ISchedulerAction>);

            foreach (object element in (objectCollection as System.Collections.IEnumerable))
            {
                Classifier classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(element.GetType());
                object DatestartMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(element, classifier, "DateStart", this, out returnValueAsCollection);
                object DateEndMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(element, classifier, "DateEnd", this, out returnValueAsCollection);
                object TimeStartMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(element, classifier, "TimeStart", this, out returnValueAsCollection);
                object TimeEndMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(element, classifier, "TimeEnd", this, out returnValueAsCollection);
                UserInterfaceObjectConnection.GetDisplayedValue(element, classifier, "PerformerName", this, out returnValueAsCollection);
                UserInterfaceObjectConnection.GetDisplayedValue(element, classifier, "PerformerLastName", this, out returnValueAsCollection);

                ISchedulerAction action = element as ISchedulerAction;
                if (IsLongAction(action))
                    _LongActions.Add(action);
                else
                    _Actions.Add(action);
                //if(action.ContainsDate(
                //βαλτα για το ΟνΠαιντ
            }
            if (this.ShowLongActionsScroll)
            {
                this.UpdateLongActionsRect();//TODO: This call is to update the LongActionsCount for the scroll
                this.UpdateLongActionsScroll();
            }

            this.Invalidate();
        }

        /// <MetaDataID>{e2cd31a3-de00-4e70-90e6-954a5137d390}</MetaDataID>
        public void SaveControlValues()
        {
            
        }

        #endregion

        #region IOperetionCallerSource Members

        /// <MetaDataID>{31fa1484-588e-47ec-8729-f49e521a5ab5}</MetaDataID>
        [Browsable(false)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[7] { "SelectedAction", "SelectedDate", "SelectedTimeStart", "SelectedTimeEnd", "DateStarts", "DateEnds", "RowMenu" };
            }
        }

        /// <MetaDataID>{de724196-e785-4c57-ae9d-c6cbc22684ef}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "RowMenu")
                return RowMenu;
            if (propertyName == "SelectedAction")
                return this.SelectedAction;
            if (propertyName == "SelectedDate")
                return this.SelectedDate;
            if (propertyName == "SelectedTimeStart")
                return this.SelectedTimeStart;
            if (propertyName == "SelectedTimeEnd")
                return this.SelectedTimeEnd;
            if (propertyName == "DateStarts")
                return this.DateStarts;
            if (propertyName == "DateEnds")
                return this.DateEnds;
            return null;
        }

        /// <MetaDataID>{04ea4fe1-2ad0-4a0b-ad63-e4026f7a0a8f}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d021259c-5611-4916-a43c-eb7edf1e3b15}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "RowMenu" || propertyName == "CellMenu")
                return Classifier.GetClassifier(typeof(OOAdvantech.UserInterface.MenuCommand));
            if (propertyName == "SelectedAction")                
                return Classifier.GetClassifier(typeof(ISchedulerAction));
            if (propertyName == "SelectedDate")
                return Classifier.GetClassifier(typeof(DateTime));
            if (propertyName == "SelectedTimeStart")
                return Classifier.GetClassifier(typeof(int));
            if (propertyName == "SelectedTimeEnd")
                return Classifier.GetClassifier(typeof(int));
            if (propertyName == "DateStarts")
                return Classifier.GetClassifier(typeof(DateTime));
            if (propertyName == "DateEnds")
                return Classifier.GetClassifier(typeof(DateTime));
            return null;
        }

        /// <MetaDataID>{d94d5e5d-1ef6-459b-93b9-a336652fe0a6}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "SelectedAction" || 
                propertyName == "SelectedDate" || 
                propertyName == "SelectedTimeStart" || 
                propertyName == "SelectedTimeEnd" ||
                propertyName == "DateStarts" ||
                propertyName == "DateEnds"||
                propertyName == "RowMenu")
                return true;
            return false;
        }

        #endregion

        #region IConnectableControl Members

        /// <MetaDataID>{2dedb012-13e5-4bb4-8d9c-f08444e21943}</MetaDataID>
        [Browsable(false)]
        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _ViewControlObject;
            }
            set
            {
                _ViewControlObject = value;
                if (_ViewControlObject == null)
                    return;
                if (_ViewControlObject != null)
                    _ViewControlObject.AddControlledComponent(this);

            }
        }

        ///// <MetaDataID>{944322bb-cd73-4f42-9dd3-9d342bfbad2a}</MetaDataID>
        //[Browsable(false)]
        //public object UIMetaDataObject
        //{
        //    get
        //    {
        //        return ListViewMetaData;
        //    }
        //    set
        //    {
        //    }
        //}

        /// <MetaDataID>{df27aad6-619b-4ce2-b544-cecf5ccfebd7}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion

        #region IMetadataSelectionResolver Members

        /// <MetaDataID>{7ba27e5c-2bcc-4576-87a4-2c17ebb9462a}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if ((metaObject is OOAdvantech.UserInterface.OperationCall) && propertyDescriptor == "BeforeShowContextMenuOperationCall")
            {
                (metaObject as OOAdvantech.UserInterface.OperationCall).TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
                return true;
            }

            if ((metaObject is OOAdvantech.UserInterface.OperationCall) && propertyDescriptor == "LoadActionsOperationCall")
            {
                OOAdvantech.MetaDataRepository.Operation operation = new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation;

                if (operation != null)
                {
                    if (!string.IsNullOrEmpty(_Path) && ValueType != null)
                    {
                        OOAdvantech.MetaDataRepository.Classifier collectionType = OOAdvantech.UserInterface.OperationCall.GetElementType(operation.ReturnType);
                        if (!string.IsNullOrEmpty(_Path) && ValueType != null)
                            return ((ValueType == collectionType) ||
                                (ValueType != null && collectionType != null && collectionType.IsA(ValueType)));
                    }
                    return true;

                }
                else
                    return false;
            }

            if (propertyDescriptor == "Path"&& metaObject is OOAdvantech.MetaDataRepository.Attribute)
            {

                OOAdvantech.MetaDataRepository.Classifier collectionClassifier = (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type;
                OOAdvantech.MetaDataRepository.Classifier elementType= OOAdvantech.UserInterface.OperationCall.GetElementType(collectionClassifier);
                Classifier ishcedulerclassifier=Classifier.GetClassifier(typeof(ISchedulerAction));

                if (elementType != null && (elementType.IsA(ishcedulerclassifier) || elementType == ishcedulerclassifier))
                {
                    return true;
                }
                else
                    MessageBox.Show("The specified Path selection does not \n implements the ISchedulerAction Interface");
            }
            else if (metaObject is OOAdvantech.MetaDataRepository.Feature || propertyDescriptor == "SelectionMember")// || propertyDescriptor == "WeekStartsMember")
                return true;          
           
            return false;
        }

        #endregion

        #region IPathDataDisplayer Members


        /// <MetaDataID>{fd301abd-a379-4d75-8ff6-ac1118b3da24}</MetaDataID>
        [Browsable(false)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get { return null; }
        }

        /// <MetaDataID>{f556bcd0-0566-4712-a6c5-2dd127bad413}</MetaDataID>
        [Browsable(false)]
        public bool HasLockRequest
        {
            get { return false; }
        }

        #region public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        /// <MetaDataID>{5bfcdee8-d6cd-4f79-9bc4-5df303db506f}</MetaDataID>
        public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        {
            List<OOAdvantech.UserInterface.Runtime.MemberChange> memberChanges = null;
            if ((change.MemberOwner as ISchedulerAction) != null)
            {
                ISchedulerAction action = change.MemberOwner as ISchedulerAction;
                if (this.IsLongAction(action))
                {
                    //check to see if it was day action
                    if (this._Actions.Contains(action))
                    {
                        for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                        {
                            DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                            if (day_column.ContainsActionViewForAction(action, true))
                            {
                                day_column.DeleteGarbageViewForAction(action, true);
                                //day_column.ReduceDayActionViewsCount();
                                day_column.ClearDayViews();//reposition the day actions
                            }
                        }
                        this._Actions.Remove(action);
                        this._LongActions.Add(action);
                    }
                    else
                    {
                        if (change.MemberName == "PerformerName" || change.MemberName == "PerformerLastName")
                        {
                            for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                            {
                                DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                                day_column.UpdateActionViews(action);
                            }
                        }
                    }
                }
                else
                {
                    //check to see if it was long action
                    if (this._LongActions.Contains(action))
                    {
                        Point p = new Point();
                        for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                        {
                            DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                            if (day_column.ContainsActionViewForAction(action, false))
                            {
                                if (p.X == 0 & p.Y == 0)
                                    p = day_column.GetPositionForAction(action);
                                day_column.DeleteGarbageViewForAction(action, false);
                            }
                        }

                        for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                        {
                            DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                            day_column.ClearLongActionViewsAbovePos(p);//reposition the long day actions
                        }

                        this._LongActions.Remove(action);
                        this._Actions.Add(action);
                    }
                    else
                    {
                        if (change.MemberName == "PerformerName" || change.MemberName == "PerformerLastName")
                        {
                            for (int ic = 1; ic < this.ColumnModel.Columns.Count; ic++)
                            {
                                DayColumn day_column = this.ColumnModel.Columns[ic] as DayColumn;
                                day_column.UpdateActionViews(action);
                            }
                        }
                    }
                }

                //memberChanges = UserInterfaceObjectConnection.GetChanges(change, this);
                this.Invalidate();
                return;
            }

            memberChanges = UserInterfaceObjectConnection.GetChanges(_Path, change, this);
            if (memberChanges.Count == 0)
                return;

            if (memberChanges.Count == 1 && memberChanges[0].Type == OOAdvantech.UserInterface.Runtime.ChangeType.ValueChanged)
                LoadControlValues();
            if (memberChanges.Count == 1 && memberChanges[0].Type == OOAdvantech.UserInterface.Runtime.ChangeType.ItemsRemoved)
            {
                foreach (object element in (memberChanges[0].Value as System.Collections.IEnumerable))
                {
                    ISchedulerAction action = element as ISchedulerAction;
                    _Actions.Remove(action);
                }
                this.Invalidate();
            }
            if (memberChanges.Count == 1 && memberChanges[0].Type == OOAdvantech.UserInterface.Runtime.ChangeType.ItemsAdded)
            {
                foreach (object element in (memberChanges[0].Value as System.Collections.IEnumerable))
                {
                    ISchedulerAction action = element as ISchedulerAction;
                    _Actions.Add(action);
                }
                this.Invalidate();
            }
        } 
        #endregion

        /// <MetaDataID>{f2d36cd0-017f-4bb6-a160-a3092246cd4e}</MetaDataID>
        public void LockStateChange(object sender)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <MetaDataID>{dde2c0c1-9840-4b0c-85de-2f0047b7f8d4}</MetaDataID>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }

    #region public enum GridLines
    /// <summary>
    /// Specifies how a Table draws grid lines between its TotalRows and columns
    /// </summary>
    /// <MetaDataID>{874fabae-e206-4eab-98ff-eebaf3e28b0b}</MetaDataID>
    public enum GridLines
    {
        /// <summary>
        /// No grid lines are drawn
        /// </summary>
        None = 0,

        /// <summary>
        /// Grid lines are only drawn between columns
        /// </summary>
        Columns = 1,

        /// <summary>
        /// Grid lines are only drawn between TotalRows
        /// </summary>
        Rows = 2,

        /// <summary>
        /// Grid lines are drawn between TotalRows and columns
        /// </summary>
        Both = 3
    } 
    #endregion

    #region public enum GridLineStyle
    /// <summary>
    /// Specifies the style of the lines drawn when a Table draws its grid lines
    /// </summary>
    /// <MetaDataID>{1c8b655b-80a4-47d7-afd3-9bf01891b558}</MetaDataID>
    public enum GridLineStyle
    {
        /// <summary>
        /// Specifies a solid line
        /// </summary>
        Solid = 0,

        /// <summary>
        /// Specifies a line consisting of dashes
        /// </summary>
        Dash = 1,

        /// <summary>
        /// Specifies a line consisting of dots
        /// </summary>
        Dot = 2,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot
        /// </summary>
        DashDot = 3,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot-dot
        /// </summary>
        DashDotDot = 4
    } 
    #endregion

    #region public enum SelectionStyle
    /// <summary>
    /// Specifies how selected Cells are drawn by a Table
    /// </summary>
    /// <MetaDataID>{b6b6c024-2b99-43ef-a3ec-96626e365157}</MetaDataID>
    public enum SelectionStyle
    {
        /// <summary>
        /// The first visible Cell in the selected Cells Row is drawn as selected
        /// </summary>
        ListView = 0,

        /// <summary>
        /// The selected Cells are drawn as selected
        /// </summary>
        Grid = 1
    }
    #endregion

    #region public enum TableState
    /// <summary>
    /// Specifies the current state of the Table
    /// </summary>
    /// <MetaDataID>{441c41b7-d5ac-433c-82eb-5443d71c6dcb}</MetaDataID>
    public enum TableViewState
    {
        /// <summary>
        /// All week is view
        /// </summary>
        WeekView,
        /// <summary>
        /// Week except weekend
        /// </summary>
        WorkWeekView,
        /// <summary>
        /// DayView
        /// </summary>
        DayView        
    } 
    #endregion

    #region public enum ActionState
    /// <MetaDataID>{b3f31bc9-3465-4821-906b-e2f5db4e3afa}</MetaDataID>
    public enum ActionState
    {
        Proposed,
        Started,
        Completed,
        Suspended,
        Abandoned
    } 
    #endregion

    //public static class DateTimeExtraOperators
    //{
    //    public static DateTime WeekStartDate(this DateTime date)
    //    {
    //        DateTime weekStartDay = date - new TimeSpan((int)DateTime.Now.DayOfWeek - 1, 0, 0, 0);
    //        return weekStartDay;
    //    }
    //}


}
