using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ConnectableControls.SchedulerControl.Renderers;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{0096f915-cedd-474f-ba00-932e85c0f17a}</MetaDataID>
    [DesignTimeVisible(false), ToolboxItem(false)]
    public abstract class Column : Component
    {
        #region Event Handlers

        /// <summary>
        /// Occurs when one of the Column's properties changes
        /// </summary>
        public event ColumnEventHandler PropertyChanged;

        #endregion

        #region Class Data

        // Column state flags
        /// <MetaDataID>{8AE6BE8A-5214-4DE6-92E1-FA1234999900}</MetaDataID>
        private readonly static int STATE_EDITABLE = 1;
        /// <MetaDataID>{EA10870A-1817-4BF9-A425-6D43042B0754}</MetaDataID>
        private readonly static int STATE_ENABLED = 2;
        /// <MetaDataID>{8CCC759F-F72A-4CB3-97E5-EA6E710FAB38}</MetaDataID>
        private readonly static int STATE_VISIBLE = 4;
        /// <MetaDataID>{EB0F33E7-2C9B-4524-AC80-4D9DC83A2213}</MetaDataID>
        private readonly static int STATE_SELECTABLE = 8;
        /// <MetaDataID>{C5F9A66E-E3EA-40D5-8CB4-6305CC3D55E0}</MetaDataID>
        private readonly static int STATE_SORTABLE = 16;

        /// <summary>
        /// The amount of space on each side of the Column that can 
        /// be used as a resizing handle
        /// </summary>
        /// <MetaDataID>{37D164D0-193D-4657-B8B1-098F6F7BBAB2}</MetaDataID>
        public static readonly int ResizePadding = 8;

        /// <summary>
        /// The default width of a Column
        /// </summary>
        /// <MetaDataID>{6C3D4867-C3EB-41EB-8E8C-B3BED25CFD91}</MetaDataID>
        public static readonly int DefaultWidth = 75;

        /// <summary>
        /// The maximum width of a Column
        /// </summary>
        /// <MetaDataID>{865892A9-DB7D-42CF-B900-DE970B3B0D32}</MetaDataID>
        public static readonly int MaximumWidth = 1024;

        /// <summary>
        /// The minimum width of a Column
        /// </summary>
        /// <MetaDataID>{ABC726E7-7BCC-47CC-8DE0-81EBD32B540A}</MetaDataID>
        public static readonly int MinimumWidth = ResizePadding * 2;

        /// <summary>
        /// Contains the current state of the the Column
        /// </summary>
        /// <MetaDataID>{B1C14F8F-4D8B-4397-959A-169FE6D44CEF}</MetaDataID>
        public byte state;

        /// <summary>
        /// The text displayed in the Column's header
        /// </summary>
        /// <MetaDataID>{915C3843-689E-417F-818B-451F3DBAF9DF}</MetaDataID>
        private string text;

        /// <summary>
        /// A string that specifies how a Column's Cell contents are formatted
        /// </summary>
        /// <MetaDataID>{8CE68868-0D95-49AD-AF2E-81502167CF27}</MetaDataID>
        private string format;

        /// <summary>
        /// The alignment of the text displayed in the Column's Cells
        /// </summary>
        /// <MetaDataID>{48710A4A-0637-4CC5-AFA8-0A5FC0CB7929}</MetaDataID>
        private ColumnAlignment alignment;                

        /// <summary>
        /// Specifies whether the Image displayed on the Column's header should 
        /// be draw on the right hand side of the Column
        /// </summary>
        /// <MetaDataID>{7A5B8C00-94DB-4C2E-BE53-982165B622DF}</MetaDataID>
        private bool imageOnRight;

        /// <summary>
        /// The current state of the Column
        /// </summary>
        /// <MetaDataID>{6E2094C0-E64D-46CF-B777-65D0E1A4EBBF}</MetaDataID>
        private ColumnState columnState;

        /// <summary>
        /// The text displayed when a ToolTip is shown for the Column's header
        /// </summary>
        /// <MetaDataID>{CEE0428D-2C6A-4938-89B7-263A1EEBAF75}</MetaDataID>
        private string tooltipText;

        /// <summary>
        /// The ColumnModel that the Column belongs to
        /// </summary>
        /// <MetaDataID>{6582037D-4452-4DDD-BD61-C369BA433EC9}</MetaDataID>
        private DayTimeColumnModel columnModel;

        
        /// <summary>
        /// The current SortOrder of the Column
        /// </summary>
        /// <MetaDataID>{B696DF08-61D1-4393-9C1F-F03293F4E32F}</MetaDataID>
        private SortOrder sortOrder;

        /// <summary>
        /// The CellRenderer used to draw the Column's Cells
        /// </summary>
        /// <MetaDataID>{865F7B52-2F6A-482F-AE3D-25415B6DE4C8}</MetaDataID>
        private ICellRenderer renderer;

        /// <summary>
        /// The CellEditor used to edit the Column's Cells
        /// </summary>
        /// <MetaDataID>{71985CBC-122C-403C-9DBA-FFD9218444CE}</MetaDataID>
        private ICellEditor editor;

        /// <summary>
        /// The Type of the IComparer used to compare the Column's Cells
        /// </summary>
        /// <MetaDataID>{A690B623-280E-4F34-A5E0-2E7135BB8CCB}</MetaDataID>
        private Type comparer;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Column with default values
        /// </summary>
        /// <MetaDataID>{11D2CB2B-43F4-425C-B851-D49553D14A39}</MetaDataID>
        public Column()
            : base()
        {
            this.Init();
        }


        /// <summary>
        /// Creates a new Column with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{AE3B1B3D-9B96-4FC5-BB99-55A15B502628}</MetaDataID>
        public Column(string text)
            : base()
        {
            this.Init();

            this.text = text;
        }


        /// <summary>
        /// Creates a new Column with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{3E313300-5E96-4E1A-86A0-938A537A2BC5}</MetaDataID>
        public Column(string text, decimal width)
            : base()
        {
            this.Init();

            this.text = text;
            this._Width = width;
        }


        /// <summary>
        /// Creates a new Column with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{509D9D37-6F35-4BF9-8870-55F5B4748C03}</MetaDataID>
        public Column(string text, int width, bool visible)
            : base()
        {
            this.Init();

            this.text = text;
            this._Width = width;
            this.Visible = visible;
        }


        ///// <summary>
        ///// Creates a new Column with the specified header text and image
        ///// </summary>
        ///// <param name="text">The text displayed in the column's header</param>
        ///// <param name="image">The image displayed on the column's header</param>
        ///// <MetaDataID>{51AA87C8-93D5-4DFE-AF2A-9AE516E752D7}</MetaDataID>
        //public Column(string text, Image image)
        //    : base()
        //{
        //    this.Init();

        //    this.text = text;
        //    this._ZoomInImage = image;
        //}


        ///// <summary>
        ///// Creates a new Column with the specified header text, image and width
        ///// </summary>
        ///// <param name="text">The text displayed in the column's header</param>
        ///// <param name="image">The image displayed on the column's header</param>
        ///// <param name="width">The column's width</param>
        ///// <MetaDataID>{50B59AC6-4C25-4D9C-8D2C-6AC81AF96D55}</MetaDataID>
        //public Column(string text, Image image, decimal width)
        //    : base()
        //{
        //    this.Init();

        //    this.text = text;
        //    this._Image = image;
        //    this._Width= width;
        //}


        ///// <summary>
        ///// Creates a new Column with the specified header text, image, width and visibility
        ///// </summary>
        ///// <param name="text">The text displayed in the column's header</param>
        ///// <param name="image">The image displayed on the column's header</param>
        ///// <param name="width">The column's width</param>
        ///// <param name="visible">Specifies whether the column is visible</param>
        ///// <MetaDataID>{099FDFB0-B7E0-4F8D-BCFE-11F84D2E4C1E}</MetaDataID>
        //public Column(string text, Image image, decimal width, bool visible)
        //    : base()
        //{
        //    this.Init();

        //    this.text = text;
        //    this._Image = image;
        //    this._Width = width;
        //    this.Visible = visible;
        //}


        /// <summary>
        /// Initialise default values
        /// </summary>
        /// <MetaDataID>{5009C7AA-41A5-4A40-8B19-477E80423069}</MetaDataID>
        private void Init()
        {
            this.text = null;
            this._Width = Column.DefaultWidth;
            this.columnState = ColumnState.Normal;
            this.alignment = ColumnAlignment.Left;
            this._ZoomInImage = null;
            this.imageOnRight = false;
            this.columnModel = null;
            this._X = 0;
            this.tooltipText = null;
            this.format = "";
            this.sortOrder = SortOrder.None;
            this.renderer = null;
            this.editor = null;
            this.comparer = null;

            this.state = (byte)(STATE_ENABLED | STATE_EDITABLE | STATE_VISIBLE | STATE_SELECTABLE | STATE_SORTABLE);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{802EC675-DB91-492D-90DA-798C9456F64E}</MetaDataID>
        public abstract string GetDefaultRendererName();


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{0D9137BD-2BA6-474D-9E92-71B2FA2DD9F0}</MetaDataID>
        public abstract ICellRenderer CreateDefaultRenderer();


        ///// <summary>
        ///// Gets a string that specifies the name of the Column's default CellEditor
        ///// </summary>
        ///// <returns>A string that specifies the name of the Column's default 
        ///// CellEditor</returns>
        ///// <MetaDataID>{A3DA711C-0F58-49B5-8D69-28FA1E3833F9}</MetaDataID>
        //public abstract string GetDefaultEditorName();


        ///// <summary>
        ///// Gets the Column's default CellEditor
        ///// </summary>
        ///// <returns>The Column's default CellEditor</returns>
        ///// <MetaDataID>{EB036DEF-794C-4B15-8313-11198F67A722}</MetaDataID>
        //public abstract ICellEditor CreateDefaultEditor();


        /// <summary>
        /// Returns the state represented by the specified state flag
        /// </summary>
        /// <param name="flag">A flag that represents the state to return</param>
        /// <returns>The state represented by the specified state flag</returns>
        /// <MetaDataID>{292880E4-DC18-43AB-8A9A-4A833357ED27}</MetaDataID>
        internal bool GetState(int flag)
        {
            return ((this.state & flag) != 0);
        }


        /// <summary>
        /// Sets the state represented by the specified state flag to the specified value
        /// </summary>
        /// <param name="flag">A flag that represents the state to be set</param>
        /// <param name="value">The new value of the state</param>
        /// <MetaDataID>{4D03E14A-9B8C-44F0-8E1F-0048C30FD13C}</MetaDataID>
        internal void SetState(int flag, bool value)
        {
            this.state = (byte)(value ? (this.state | flag) : (this.state & ~flag));
        }

        #endregion

        #region Properties

        #region public string Text
        /// <summary>
        /// Gets or sets the text displayed on the Column header
        /// </summary>
        /// <MetaDataID>{4C7AA8C8-3D56-4C1B-B413-1FE8F2F84CCF}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
        Description("The text displayed in the column's header.")]
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                if (!value.Equals(this.text))
                {
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Text = value;

                    string oldText = this.text;

                    this.text = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.TextChanged, oldText));
                }
            }
        } 
        #endregion

        #region public string Format
        /// <summary>
        /// Gets or sets the string that specifies how a Column's Cell contents 
        /// are formatted
        /// </summary>
        /// <MetaDataID>{45692E1B-BAF4-4CBF-B731-FA880D978BA3}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(""),
        Description("A string that specifies how a column's cell contents are formatted.")]
        public string Format
        {
            get
            {
                return this.format;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                if (!value.Equals(this.format))
                {

                    string oldFormat = this.format;

                    this.format = value;

                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Format = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.FormatChanged, oldFormat));
                }
            }
        }
        
        #endregion

        #region public virtual ColumnAlignment Alignment
        /// <summary>
        /// Gets or sets the horizontal alignment of the Column's Cell contents
        /// </summary>
        /// <MetaDataID>{13591E42-5BE0-475C-BE30-3062681AA862}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(ColumnAlignment.Left),
        Description("The horizontal alignment of the column's cell contents.")]
        public virtual ColumnAlignment Alignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                if (!Enum.IsDefined(typeof(ColumnAlignment), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnAlignment));
                }

                if (this.alignment != value)
                {
                    ColumnAlignment oldAlignment = this.alignment;

                    //this.alignment = value;
                    //if (_ColumnMetaData != null)
                    //{
                    //    switch (this.alignment)
                    //    {
                    //        case ColumnAlignment.Center:
                    //            _ColumnMetaData.Alignment = OOAdvantech.UserInterface.ColumnAlignment.Center;
                    //            break;
                    //        case ColumnAlignment.Left:
                    //            _ColumnMetaData.Alignment = OOAdvantech.UserInterface.ColumnAlignment.Left;
                    //            break;
                    //        case ColumnAlignment.Right:
                    //            _ColumnMetaData.Alignment = OOAdvantech.UserInterface.ColumnAlignment.Right;
                    //            break;

                    //    }
                    //}
                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.AlignmentChanged, oldAlignment));
                }
            }
        } 
        #endregion

        #region public decimal Width
        /// <MetaDataID>{a88544ee-01cf-42e1-b1df-7ba51d4c6369}</MetaDataID>
        private decimal _Width;
        /// <summary>
        /// Gets or sets the width of the Column
        /// </summary>
        /// <MetaDataID>{8B23DB64-0C12-4AB7-9787-E620CDF6DF7D}</MetaDataID>
        [Category("Appearance"),
        Description("The width of the column.")]
        public decimal Width
        {
            get
            {
                return this._Width;
            }

            set
            {
                if (this._Width != value)
                {
                    //int oldWidth = this.Width;

                    // Set the width, and check min & max
                    //this.width = Math.Min(Math.Max(value, MinimumWidth), MaximumWidth);
                    this._Width = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.WidthChanged, _Width));
                }
            }
        }
        
        #endregion

        #region private bool ShouldSerializeWidth()
        /// <summary>
        /// Specifies whether the Width property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Width property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{4FED7AB3-323E-4B51-8885-2AD3C78939CC}</MetaDataID>
        private bool ShouldSerializeWidth()
        {
            return this.Width != Column.DefaultWidth;
        }
        
        #endregion

        #region public Image ZoomInImage
        /// <MetaDataID>{7a5e3c98-7677-4ce9-9645-e15e31dc8dfa}</MetaDataID>
        private Image _ZoomInImage;
        /// <summary>
        /// Gets or sets the Image displayed in the Column's header
        /// </summary>
        /// <MetaDataID>{E8106AC9-EECE-44DF-B798-47040279F0FD}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
        Description("Ihe image displayed in the column's header")]
        public Image ZoomInImage
        {
            get
            {
                return this._ZoomInImage;
            }

            set
            {
                if (this._ZoomInImage != value)
                {
                    //Image oldImage = this.ZoomInImage;

                    this._ZoomInImage = value;

                    //this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ImageChanged, oldImage));
                }
            }
        } 
        #endregion

        #region public Image ZoomOutImage
        /// <MetaDataID>{0f6b37a3-9d62-4708-aa96-b62ac305b7f1}</MetaDataID>
        private Image _ZoomOutImage;
        /// <MetaDataID>{2245cb24-17a1-4663-b1bc-976713ef2aac}</MetaDataID>
        public Image ZoomOutImage
        {
            get
            {
                return this._ZoomOutImage;
            }

            set
            {
                if (this._ZoomOutImage != value)
                {
                    this._ZoomOutImage = value;
                }
            }
        }  
        #endregion

        #region public bool ImageOnRight
        /// <summary>
        /// Gets or sets whether the Image displayed on the Column's header should 
        /// be draw on the right hand side of the Column
        /// </summary>
        /// <MetaDataID>{A5409CA3-CA3A-41F9-A89A-540D07B1038C}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(false),
        Description("Specifies whether the image displayed on the column's header should be drawn on the right hand side of the column")]
        public bool ImageOnRight
        {
            get
            {
                return this.imageOnRight;
            }

            set
            {
                if (this.imageOnRight != value)
                {
                    this.imageOnRight = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ImageChanged, null));
                }
            }
        } 
        #endregion


        /// <summary>
        /// Gets the state of the Column
        /// </summary>
        /// <MetaDataID>{19466F3C-7C02-4AE6-A42B-6D585CBA4183}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnState ColumnState
        {
            get
            {
                return this.columnState;
            }
        }


        /// <summary>
        /// Gets or sets the state of the Column
        /// </summary>
        /// <MetaDataID>{EF47D9A5-913D-4404-B025-766D13F1E204}</MetaDataID>
        internal ColumnState InternalColumnState
        {
            get
            {
                return this.ColumnState;
            }

            set
            {
                if (!Enum.IsDefined(typeof(ColumnState), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnState));
                }

                if (this.columnState != value)
                {
                    ColumnState oldState = this.columnState;

                    this.columnState = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.StateChanged, oldState));
                }
            }
        }


        /// <summary>
        /// Gets or sets the whether the Column is displayed
        /// </summary>
        /// <MetaDataID>{ACD7DC15-E1F8-4268-AD45-0ABAF22C8F29}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(true),
        Description("Determines whether the column is visible or hidden.")]
        public bool Visible
        {
            get
            {
                return this.GetState(STATE_VISIBLE);
            }

            set
            {
                bool visible = this.Visible;

                this.SetState(STATE_VISIBLE, value);

                if (visible != value)
                {
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Visible = value;
                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.VisibleChanged, visible));
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Column is able to be sorted
        /// </summary>
        /// <MetaDataID>{3ADF38D7-D0A3-4F0E-9D9E-1F8C2C7BE6AF}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(true),
        Description("Determines whether the column is able to be sorted.")]
        public virtual bool Sortable
        {
            get
            {
                return this.GetState(STATE_SORTABLE);
            }

            set
            {
                bool sortable = this.Sortable;

                this.SetState(STATE_SORTABLE, value);

                if (sortable != value)
                {
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Sortable = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.SortableChanged, sortable));
                }
            }
        }


        /// <summary>
        /// Gets or sets the user specified ICellRenderer that is used to draw the 
        /// Column's Cells
        /// </summary>
        /// <MetaDataID>{0D9E55AB-E4D0-412B-98EE-2316BC907EDA}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICellRenderer Renderer
        {
            get
            {
                return this.renderer;
            }

            set
            {
                if (this.renderer != value)
                {
                    this.renderer = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
                }
            }
        }


        /// <summary>
        /// Gets or sets the user specified ICellEditor that is used to edit the 
        /// Column's Cells
        /// </summary>
        /// <MetaDataID>{FE52B7BC-BA9C-43ED-9B7A-88BDCAC077BE}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICellEditor Editor
        {
            get
            {
                return this.editor;
            }

            set
            {
                if (this.editor != value)
                {
                    this.editor = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.EditorChanged, null));
                }
            }
        }


        /// <summary>
        /// Gets or sets the user specified Comparer type that is used to edit the 
        /// Column's Cells
        /// </summary>
        /// <MetaDataID>{21BAFAA5-33AB-4143-AD56-DA8793D3EAF2}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type Comparer
        {
            get
            {
                return this.comparer;
            }

            set
            {
                if (this.comparer != value)
                {
                    this.comparer = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ComparerChanged, null));
                }
            }
        }


        ///// <summary>
        ///// Gets the Type of the default Comparer used to compare the Column's Cells when 
        ///// the Column is sorting
        ///// </summary>
        ///// <MetaDataID>{D7DD1DC6-E10E-4990-B2F7-98748A731C20}</MetaDataID>
        //[Browsable(false)]
        //public abstract Type DefaultComparerType
        //{
        //    get;
        //}


        /// <summary>
        /// Gets the current SortOrder of the Column
        /// </summary>
        /// <MetaDataID>{76F30E6A-30F5-4F2A-9214-8FB2131F0FC4}</MetaDataID>
        [Browsable(false)]
        public SortOrder SortOrder
        {
            get
            {
                return this.sortOrder;
            }
        }


        /// <summary>
        /// Gets or sets the current SortOrder of the Column
        /// </summary>
        /// <MetaDataID>{D698DFAC-4CAD-4D99-AAE7-11BACFD9C2C9}</MetaDataID>
        internal SortOrder InternalSortOrder
        {
            get
            {
                return this.SortOrder;
            }

            set
            {
                if (!Enum.IsDefined(typeof(SortOrder), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(SortOrder));
                }

                if (this.sortOrder != value)
                {
                    SortOrder oldOrder = this.sortOrder;

                    this.sortOrder = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.SortOrderChanged, oldOrder));
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Column's Cells contents 
        /// are able to be edited
        /// </summary>
        /// <MetaDataID>{BE71B337-965A-46E2-86E9-EEA0E1771AC6}</MetaDataID>
        [Category("Appearance"),
        Description("Controls whether the column's cell contents are able to be changed by the user")]
        public virtual bool Editable
        {
            get
            {
                if (!this.GetState(STATE_EDITABLE))
                {
                    return false;
                }

                return this.Visible && this.Enabled;
            }

            set
            {
                bool editable = this.GetState(STATE_EDITABLE);

                this.SetState(STATE_EDITABLE, value);

                if (editable != value)
                {
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Editable = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.EditableChanged, editable));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Editable property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Editable property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{4178D37E-C414-43FB-BDFD-D066301B3F15}</MetaDataID>
        private bool ShouldSerializeEditable()
        {
            return !this.GetState(STATE_EDITABLE);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Column's Cells can respond to 
        /// user interaction
        /// </summary>
        /// <MetaDataID>{8B807EF6-795B-48DD-BD4E-594FC22678DC}</MetaDataID>
        [Category("Appearance"),
        Description("Indicates whether the column's cells can respond to user interaction")]
        public bool Enabled
        {
            get
            {
                if (!this.GetState(STATE_ENABLED))
                {
                    return false;
                }

                if (this.ColumnModel == null)
                {
                    return true;
                }

                return this.ColumnModel.Enabled;
            }

            set
            {
                bool enabled = this.GetState(STATE_ENABLED);

                this.SetState(STATE_ENABLED, value);

                if (enabled != value)
                {
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Enabled = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.EnabledChanged, enabled));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Enabled property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Enabled property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{B9E90865-3C73-4FF6-A0E3-DAC9F57D71DB}</MetaDataID>
        private bool ShouldSerializeEnabled()
        {
            return !this.GetState(STATE_ENABLED);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Column's Cells can be selected
        /// </summary>
        /// <MetaDataID>{2D1F3FB4-A798-46F1-BF56-8B52BE3C17DB}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(true),
        Description("Indicates whether the column's cells can be selected")]
        public virtual bool Selectable
        {
            get
            {
                return this.GetState(STATE_SELECTABLE);
            }

            set
            {
                bool selectable = this.Selectable;

                this.SetState(STATE_SELECTABLE, value);

                if (selectable != value)
                {
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.Selectable = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.SelectableChanged, selectable));
                }
            }
        }


        /// <summary>
        /// Gets or sets the ToolTip text associated with the Column
        /// </summary>
        /// <MetaDataID>{5BCF2C77-8B2D-440A-B51B-32485C98BE9C}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(null),
        Description("The ToolTip text associated with the Column")]
        public string ToolTipText
        {
            get
            {
                return this.tooltipText;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                if (!value.Equals(this.tooltipText))
                {
                    string oldTip = this.tooltipText;

                    this.tooltipText = value;
                    //if (_ColumnMetaData != null)
                    //    _ColumnMetaData.ToolTipText = value;

                    this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.ToolTipTextChanged, oldTip));
                }
            }
        }



        /// <MetaDataID>{e7124449-8f62-48b0-a498-6e04afe67d71}</MetaDataID>
        private decimal _X;
        /// <summary>
        /// Gets the x-coordinate of the column's left edge in pixels
        /// </summary>
        /// <MetaDataID>{096A0EFE-D64D-4890-A013-CC98274C24DE}</MetaDataID>
        internal decimal X
        {
            get
            {
                return this._X;
            }

            set
            {
                this._X = value;
            }
        }


        /// <summary>
        /// Gets the x-coordinate of the column's left edge in pixels
        /// </summary>
        /// <MetaDataID>{ED2904F1-B40C-48AC-8CCD-4B6B19ADDAD1}</MetaDataID>
        [Browsable(false)]
        public decimal Left
        {
            get
            {
                return this.X;
            }
        }


        /// <summary>
        /// Gets the x-coordinate of the column's right edge in pixels
        /// </summary>
        /// <MetaDataID>{A1D0C1AD-0DBB-43B4-AC28-E331A15EA413}</MetaDataID>
        [Browsable(false)]
        public decimal Right
        {
            get
            {
                return this.Left + this.Width;
            }
        }


        /// <summary>
        /// Gets or sets the ColumnModel the Column belongs to
        /// </summary>
        /// <MetaDataID>{F102C8DD-CD4D-4F4B-AF4C-E5BC278C1EC5}</MetaDataID>
        protected internal DayTimeColumnModel ColumnModel
        {
            get
            {
                return this.columnModel;
            }

            set
            {
                this.columnModel = value;
            }
        }



        /// <summary>
        /// Gets the ColumnModel the Column belongs to.  This member is not 
        /// intended to be used directly from your code
        /// </summary>
        /// <MetaDataID>{8BC857B9-A409-4CA3-8A04-DDF567F24F75}</MetaDataID>
        [Browsable(false)]
        public DayTimeColumnModel Parent
        {
            get
            {
                return this.ColumnModel;
            }
        }


        /// <summary>
        /// Gets whether the Column is able to raise events
        /// </summary>
        /// <MetaDataID>{624394D0-10C2-41F1-B7F0-14839C88EC46}</MetaDataID>
        protected bool CanRaiseEvents
        {
            get
            {
                // check if the ColumnModel that the Colum belongs to is able to 
                // raise events (if it can't, the Colum shouldn't raise events either)
                if (this.ColumnModel != null)
                {
                    return this.ColumnModel.CanRaiseEvents;
                }

                return true;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{EEEEAD19-AD3B-44B5-B331-6030E0850A69}</MetaDataID>
        protected virtual void OnPropertyChanged(ColumnEventArgs e)
        {
            if (this.ColumnModel != null)
            {
                e.SetIndex(this.ColumnModel.Columns.IndexOf(this));
            }

            if (this.CanRaiseEvents)
            {
                if (this.ColumnModel != null)
                {
                    this.ColumnModel.OnColumnPropertyChanged(e);
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// Specifies alignment of a Columns content
    /// </summary>
    /// <MetaDataID>{069df27f-0325-42eb-acef-6ada5c3b5a1f}</MetaDataID>
    public enum ColumnAlignment
    {
        /// <summary>
        /// The Columns content is aligned to the left
        /// </summary>
        Left = 0,

        /// <summary>
        /// The Columns content is aligned to the center
        /// </summary>
        Center = 1,

        /// <summary>
        /// The Columns content is aligned to the right
        /// </summary>
        Right = 2
    }

    /// <summary>
    /// Specifies the state of a Column
    /// </summary>
    /// <MetaDataID>{5e2faa0e-5dcb-4584-9754-4b72e10319d2}</MetaDataID>
    public enum ColumnState
    {
        /// <summary>
        /// Column is in its normal state
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Mouse is over the Column
        /// </summary>
        Hot = 2,

        /// <summary>
        /// Column is being pressed
        /// </summary>
        Pressed = 3
    }
}
