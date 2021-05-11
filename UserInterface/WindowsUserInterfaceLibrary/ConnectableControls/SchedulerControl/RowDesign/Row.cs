using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;
using ConnectableControls.SchedulerControl.TableModelDesign;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.RowDesign
{
    /// <summary>
    /// SRepresents a row of Cells displayed in a Table
    /// </summary>
    /// <MetaDataID>{F4D75238-473A-46C1-B8FE-F821DE2A7116}</MetaDataID>
    [DesignTimeVisible(true)]//, TypeConverter(typeof(RowConverter))]
    public class Row : IDisposable
    {
        #region EventHandlers

        /// <summary>
        /// Occurs when a Cell is added to the Row
        /// </summary>
        public event RowEventHandler CellAdded;

        /// <summary>
        /// Occurs when a Cell is removed from the Row
        /// </summary>
        public event RowEventHandler CellRemoved;

        /// <summary>
        /// Occurs when the value of a Row's property changes
        /// </summary>
        public event RowEventHandler PropertyChanged;

        #endregion

        #region Class Data

        // Row state flags
        private static readonly int STATE_EDITABLE = 1;
        private static readonly int STATE_ENABLED = 2;

        /// <summary>
        /// The collection of Cells's contained in the Row
        /// </summary>
        private CellCollection cells;

        /// <summary>
        /// An object that contains data about the Row
        /// </summary>
        private object tag;

        /// <summary>
        /// The index of the Row
        /// </summary>
        private int index;

        /// <summary>
        /// the current state of the Row
        /// </summary>
        private byte state;

        /// <summary>
        /// The Row's RowStyle
        /// </summary>
        private RowStyle rowStyle;

        /// <summary>
        /// The number of Cells in the Row that are selected
        /// </summary>
        private int selectedCellCount;

        /// <summary>
        /// Specifies whether the Row has been disposed
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Row class with default settings
        /// </summary>
        /// <MetaDataID>{7579C1A3-A270-4224-9B26-D71397A02967}</MetaDataID>
        public Row()
        {
            this.Init();
        }

        internal bool TimeRowIncludesMinute(int minute, int actionendhour)
        {
            if (this.ContainsMinute(minute))
                return true;


            if (minute < this.StartMinute && this.Hour < actionendhour)
            {
                int timespan = 60 / this.TableModel.Rows[0].InnerRows.Count;

                minute += timespan;
                return TimeRowIncludesMinute(minute, actionendhour);
            }
            return false;
        }

        public bool ContainsMinute(int minute)
        {
            if (this.StartMinute <= minute && this.EndMinute >= minute)
                return true;

            return false;
        }
        //internal object _PresentationObject;
        //public object PresentationObject
        //{
        //    get
        //    {
        //        if (_PresentationObject != null)
        //            return _PresentationObject;
        //        return CollectionObject;
        //    }
        //}
        //public object _CollectionObject;
        //public object CollectionObject
        //{
        //    get
        //    {
        //        return _CollectionObject;
        //    }
        //    set
        //    {
        //        _CollectionObject = value;
        //    }
        //}



        #region public Row(string[] items)
        /// <summary>
        /// Initializes a new instance of the Row class with an array of strings 
        /// representing Cells
        /// </summary>
        /// <param name="items">An array of strings that represent the Cells of 
        /// the Row</param>
        /// <MetaDataID>{900BCA70-1D61-4119-A55B-EBC497206EC5}</MetaDataID>
        public Row(string[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "string[] cannot be null");
            }

            this.Init();

            if (items.Length > 0)
            {
                Cell[] cells = new Cell[items.Length];

                for (int i = 0; i < items.Length; i++)
                {
                    cells[i] = new Cell(items[i]);
                }

                this.Cells.AddRange(cells);
            }
        } 
        #endregion

        #region public Row(Cell[] cells)
        /// <summary>
        /// Initializes a new instance of the Row class with an array of Cell objects 
        /// </summary>
        /// <param name="cells">An array of Cell objects that represent the Cells of the Row</param>
        /// <MetaDataID>{CF9BDE40-16D1-41B9-AE23-858598C4AC35}</MetaDataID>
        public Row(Cell[] cells)
        {
            if (cells == null)
            {
                throw new ArgumentNullException("cells", "Cell[] cannot be null");
            }

            this.Init();

            if (cells.Length > 0)
            {
                this.Cells.AddRange(cells);
            }
        }
        
        #endregion

        #region public Row(string[] items, Color foreColor, Color backColor, Font font)
        /// <summary>
        /// Initializes a new instance of the Row class with an array of strings 
        /// representing Cells and the foreground color, background color, and font 
        /// of the Row
        /// </summary>
        /// <param name="items">An array of strings that represent the Cells of the Row</param>
        /// <param name="foreColor">The foreground Color of the Row</param>
        /// <param name="backColor">The background Color of the Row</param>
        /// <param name="font">The Font used to draw the text in the Row's Cells</param>
        /// <MetaDataID>{62F46DD3-C204-49D4-A5B7-65545DAA6795}</MetaDataID>
        public Row(string[] items, Color foreColor, Color backColor, Font font,int hour,int start_min,int end_min,bool draw_hour)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "string[] cannot be null");
            }

            this.Init();

            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;

            if (items.Length > 0)
            {
                Cell[] cells = new Cell[items.Length];

                for (int i = 0; i < items.Length; i++)
                {
                    cells[i] = new Cell(items[i]);
                    if (i == 0 && draw_hour)
                        cells[i].Text = hour.ToString();
                }

                this.Cells.AddRange(cells);
            }
            _Hour = hour;
            _StartMinute = start_min;
            _EndMinute = end_min;
        } 
        #endregion

        public Row(TableModel owner,int hour,Color foreColor, Color backColor, Font font)
        {
            _Hour = hour;
            //_RowMinutes = minutes;
            IsTimeRow = true;

            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
            this._TableModel = owner;

            
            Row row = new Row(new string[] { "", "", "", "", "", "", "", "", }, foreColor, backColor, font,hour, 0,29,true);
            row._TableModel = this.TableModel;
            Row row2 = new Row(new string[] { "", "", "", "", "", "", "", "", }, foreColor, backColor, font, hour,30,59,false);
            row2._TableModel = this._TableModel;
            _InnerRows = new RowCollection(this._TableModel, this);
            _InnerRows.AddRange(new Row[] { row, row2 });
        }


        private int _StartMinute;
        public int StartMinute
        {
            get
            {
                return _StartMinute;
            }
        }

        private int _EndMinute;
        public int EndMinute
        {
            get
            {
                return _EndMinute;
            }
        }

        private int _Hour;
        public int Hour
        {
            get
            {
                return _Hour;
            }
        }

        private RowCollection _InnerRows;
        public RowCollection InnerRows
        {
            get
            {
                return _InnerRows;
            }
        }
        public bool IsTimeRow = false;

        /// <summary>
        /// Initializes a new instance of the Row class with an array of Cell objects and 
        /// the foreground color, background color, and font of the Row
        /// </summary>
        /// <param name="cells">An array of Cell objects that represent the Cells of the Row</param>
        /// <param name="foreColor">The foreground Color of the Row</param>
        /// <param name="backColor">The background Color of the Row</param>
        /// <param name="font">The Font used to draw the text in the Row's Cells</param>
        /// <MetaDataID>{5D5ADFC9-9B50-4923-BEE7-06D2D23D570A}</MetaDataID>
        public Row(Cell[] cells, Color foreColor, Color backColor, Font font)
        {
            if (cells == null)
            {
                throw new ArgumentNullException("cells", "Cell[] cannot be null");
            }

            this.Init();

            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;

            if (cells.Length > 0)
            {
                this.Cells.AddRange(cells);
            }
        }


        /// <summary>
        /// Initialise default values
        /// </summary>
        /// <MetaDataID>{6911C497-64F9-49AB-B0D0-115E8E914B52}</MetaDataID>
        private void Init()
        {
            this.cells = null;

            this.tag = null;
            //this.tableModel = null;
            this.index = -1;
            this.rowStyle = null;
            this.selectedCellCount = 0;

            this.state = (byte)(STATE_EDITABLE | STATE_ENABLED);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases all resources used by the Row
        /// </summary>
        /// <MetaDataID>{0F4742F3-5EFA-4CBF-92E2-E33D670464DD}</MetaDataID>
        public void Dispose()
        {
            if (!this.disposed)
            {
                this.tag = null;

                if (this._TableModel != null)
                {
                    this._TableModel.TotalRows.Remove(this);
                }

                this._TableModel = null;
                this.index = -1;

                if (this.cells != null)
                {
                    Cell cell;

                    for (int i = 0; i < this.cells.Count; i++)
                    {
                        cell = this.cells[i];

                        cell.InternalRow = null;
                        cell.Dispose();
                    }

                    this.cells = null;
                }

                this.rowStyle = null;
                this.state = (byte)0;

                this.disposed = true;
            }
        }


        /// <summary>
        /// Returns the state represented by the specified state flag
        /// </summary>
        /// <param name="flag">A flag that represents the state to return</param>
        /// <returns>The state represented by the specified state flag</returns>
        /// <MetaDataID>{D90A4665-31CA-4B9B-80D3-D3D0D56F43E5}</MetaDataID>
        internal bool GetState(int flag)
        {
            return ((this.state & flag) != 0);
        }


        /// <summary>
        /// Sets the state represented by the specified state flag to the specified value
        /// </summary>
        /// <param name="flag">A flag that represents the state to be set</param>
        /// <param name="value">The new value of the state</param>
        /// <MetaDataID>{B4D990FF-0C33-44C0-A80D-A07E7F5928D0}</MetaDataID>
        internal void SetState(int flag, bool value)
        {
            this.state = (byte)(value ? (this.state | flag) : (this.state & ~flag));
        }

        #endregion

        #region Properties

        /// <summary>
        /// A CellCollection representing the collection of 
        /// Cells contained within the Row
        /// </summary>
        [Category("Behavior"),
        Description("Cell Collection"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //Editor(typeof(CellCollectionEditor), typeof(UITypeEditor))]
        public CellCollection Cells
        {
            get
            {
                if (this.cells == null)
                {
                    this.cells = new CellCollection(this);
                }

                return this.cells;
            }
        }


        /// <summary>
        /// Gets or sets the object that contains data about the Row
        /// </summary>
        [Category("Appearance"),
        DefaultValue(null),
        Description("User defined data associated with the row"),
        TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get
            {
                return this.tag;
            }

            set
            {
                this.tag = value;
            }
        }


        /// <summary>
        /// Gets or sets the RowStyle used by the Row
        /// </summary>
        [Browsable(false),
        DefaultValue(null)]
        public RowStyle RowStyle
        {
            get
            {
                return this.rowStyle;
            }

            set
            {
                if (this.rowStyle != value)
                {
                    this.rowStyle = value;

                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.StyleChanged));
                }
            }
        }


        /// <summary>
        /// Gets or sets the background color for the Row
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        Description("The background color used to display text and graphics in the row")]
        public Color BackColor
        {
            get
            {
                if (this.RowStyle == null)
                {
                    return Color.Transparent;
                }

                return this.RowStyle.BackColor;
            }

            set
            {
                if (this.RowStyle == null)
                {
                    this.RowStyle = new RowStyle();
                }

                if (this.RowStyle.BackColor != value)
                {
                    this.RowStyle.BackColor = value;

                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.BackColorChanged));
                }
            }
        }


        /// <summary>
        /// Specifies whether the BackColor property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the BackColor property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{5B6C8505-FDF5-4B90-9C06-C2CB92CBA446}</MetaDataID>
        private bool ShouldSerializeBackColor()
        {
            return (this.rowStyle != null && this.rowStyle.BackColor != Color.Empty);
        }


        /// <summary>
        /// Gets or sets the foreground Color for the Row
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        Description("The foreground color used to display text and graphics in the row")]
        public Color ForeColor
        {
            get
            {
                if (this.RowStyle == null)
                {
                    if (this.TableModel != null && this.TableModel.Table != null)
                    {
                        return this.TableModel.Table.ForeColor;
                    }

                    return Color.Black;
                }
                else
                {
                    if (this.RowStyle.ForeColor == Color.Empty || this.RowStyle.ForeColor == Color.Transparent)
                    {
                        if (this.TableModel != null && this.TableModel.Table != null)
                        {
                            return this.TableModel.Table.ForeColor;
                        }
                    }

                    return this.RowStyle.ForeColor;
                }
            }

            set
            {
                if (this.RowStyle == null)
                {
                    this.RowStyle = new RowStyle();
                }

                if (this.RowStyle.ForeColor != value)
                {
                    this.RowStyle.ForeColor = value;

                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.ForeColorChanged));
                }
            }
        }


        /// <summary>
        /// Specifies whether the ForeColor property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the ForeColor property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{B6935BD2-3C16-48C2-881D-2BE70316FDF1}</MetaDataID>
        private bool ShouldSerializeForeColor()
        {
            return (this.rowStyle != null && this.rowStyle.ForeColor != Color.Empty);
        }


        /// <summary>
        /// Gets or sets the vertical alignment of the objects displayed in the Row
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        DefaultValue(RowAlignment.Center),
        Description("The vertical alignment of the objects displayed in the row")]
        public RowAlignment Alignment
        {
            get
            {
                if (this.RowStyle == null)
                {
                    return RowAlignment.Center;
                }

                return this.RowStyle.Alignment;
            }

            set
            {
                if (!Enum.IsDefined(typeof(RowAlignment), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(RowAlignment));
                }

                if (this.RowStyle == null)
                {
                    this.RowStyle = new RowStyle();
                }

                if (this.RowStyle.Alignment != value)
                {
                    this.RowStyle.Alignment = value;

                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.AlignmentChanged));
                }
            }
        }


        /// <summary>
        /// Gets or sets the Font used by the Row
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        Description("The font used to display text in the row")]
        public Font Font
        {
            get
            {
                if (this.RowStyle == null)
                {
                    if (this.TableModel != null && this.TableModel.Table != null)
                    {
                        return this.TableModel.Table.Font;
                    }

                    return null;
                }
                else
                {
                    if (this.RowStyle.Font == null)
                    {
                        if (this.TableModel != null && this.TableModel.Table != null)
                        {
                            return this.TableModel.Table.Font;
                        }
                    }

                    return this.RowStyle.Font;
                }
            }

            set
            {
                if (this.RowStyle == null)
                {
                    this.RowStyle = new RowStyle();
                }

                if (this.RowStyle.Font != value)
                {
                    this.RowStyle.Font = value;

                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.FontChanged));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Font property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Font property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{9F4AE034-2014-4D01-8063-CA342B214A0C}</MetaDataID>
        private bool ShouldSerializeFont()
        {
            return (this.rowStyle != null && this.rowStyle.Font != null);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Row's Cells are able 
        /// to be edited
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        Description("Controls whether the row's cell contents are able to be changed by the user")]
        public bool Editable
        {
            get
            {
                if (!this.GetState(STATE_EDITABLE))
                {
                    return false;
                }

                return this.Enabled;
            }

            set
            {
                bool editable = this.Editable;

                this.SetState(STATE_EDITABLE, value);

                if (editable != value)
                {
                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.EditableChanged));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Editable property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Editable property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{B0E7F95C-5609-4D4A-A0BA-4F3CE3E8953A}</MetaDataID>
        private bool ShouldSerializeEditable()
        {
            return !this.GetState(STATE_EDITABLE);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Row's Cells can respond to 
        /// user interaction
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        Description("Indicates whether the row's cells can respond to user interaction"),
        RefreshProperties(RefreshProperties.All)]
        public bool Enabled
        {
            get
            {
                if (!this.GetState(STATE_ENABLED))
                {
                    return false;
                }

                if (this.TableModel == null)
                {
                    return true;
                }

                return this.TableModel.Enabled;
            }

            set
            {
                bool enabled = this.Enabled;

                this.SetState(STATE_ENABLED, value);

                if (enabled != value)
                {
                    this.OnPropertyChanged(new RowEventArgs(this, RowEventType.EnabledChanged));
                }
            }
        }


        /// <summary>
        /// Specifies whether the Enabled property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Enabled property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{3B128BFA-BA26-4F9C-97EE-667CE095C863}</MetaDataID>
        private bool ShouldSerializeEnabled()
        {
            return !this.GetState(STATE_ENABLED);
        }


        private TableModel _TableModel;
        /// <summary>
        /// Gets the TableModel the Row belongs to
        /// </summary>
        [Browsable(false)]
        public TableModel TableModel
        {
            get
            {
                return _TableModel;
            }
            set
            {
                _TableModel = value;
            }
        }


        ///// <summary>
        ///// Gets or sets the TableModel the Row belongs to
        ///// </summary>
        //internal TableModel InternalTableModel
        //{
        //    get
        //    {
        //        return this.tableModel;
        //    }

        //    set
        //    {
        //        this.tableModel = value;
        //    }
        //}


        /// <summary>
        /// Gets the index of the Row within its TableModel
        /// </summary>
        [Browsable(false)]
        public int Index
        {
            get
            {
                return this.index;
            }
        }


        /// <summary>
        /// Gets or sets the index of the Row within its TableModel
        /// </summary>
        internal int InternalIndex
        {
            get
            {
                return this.index;
            }

            set
            {
                this.index = value;
            }
        }


        /// <summary>
        /// Updates the Cell's Index property so that it matches the Cells 
        /// position in the CellCollection
        /// </summary>
        /// <param name="start">The index to start updating from</param>
        /// <MetaDataID>{9C436EBC-4C83-4FB7-943C-5F0985820FDC}</MetaDataID>
        internal void UpdateCellIndicies(int start)
        {
            if (start == -1)
            {
                start = 0;
            }

            for (int i = start; i < this.Cells.Count; i++)
            {
                this.Cells[i].InternalIndex = i;
            }
        }


        /// <summary>
        /// Gets whether the Row is able to raise events
        /// </summary>
        protected internal bool CanRaiseEvents
        {
            get
            {
                if (this.TableModel != null)
                {
                    return this.TableModel.CanRaiseEvents;
                }

                return true;
            }
        }


        /// <summary>
        /// Gets the number of Cells that are selected within the Row
        /// </summary>
        [Browsable(false)]
        public int SelectedCellCount
        {
            get
            {
                return this.selectedCellCount;
            }
        }
        internal bool IsSelectedOnLastClick = false;

        /// <summary>
        /// Gets or sets the number of Cells that are selected within the Row
        /// </summary>
        internal int InternalSelectedCellCount
        {
            get
            {
                return this.selectedCellCount;

            }

            set
            {
                if (selectedCellCount > 0)
                    IsSelectedOnLastClick = true;
                this.selectedCellCount = value;
                if (selectedCellCount == 0)
                    IsSelectedOnLastClick = false;
            }
        }


        /// <summary>
        /// Gets whether any Cells within the Row are selected
        /// </summary>
        [Browsable(false)]
        public bool AnyCellsSelected
        {
            get
            {
                return (this.selectedCellCount > 0);
            }
        }


        /// <summary>
        /// Returns whether the Cell at the specified index is selected
        /// </summary>
        /// <param name="index">The index of the Cell in the Row's Row.CellCollection</param>
        /// <returns>True if the Cell at the specified index is selected, 
        /// otherwise false</returns>
        /// <MetaDataID>{1039FAFD-7877-4564-A25B-EFA3776AC2BF}</MetaDataID>
        public bool IsCellSelected(int index)
        {
            if (this.Cells.Count == 0)
            {
                return false;
            }

            if (index < 0 || index >= this.Cells.Count)
            {
                return false;
            }

            return this.Cells[index].Selected;
        }


        /// <summary>
        /// Removes the selected state from all the Cells within the Row
        /// </summary>
        /// <MetaDataID>{C1CA9F30-A7D1-4B44-9660-D39AB193A77C}</MetaDataID>
        internal void ClearSelection()
        {
            this.selectedCellCount = 0;
            IsSelectedOnLastClick = false;

            for (int i = 0; i < this.Cells.Count; i++)
            {
                this.Cells[i].SetSelected(false);
            }
        }


        /// <summary>
        /// Returns an array of Cells that contains all the selected Cells 
        /// within the Row
        /// </summary>
        [Browsable(false)]
        public Cell[] SelectedItems
        {
            get
            {
                if (this.SelectedCellCount == 0 || this.Cells.Count == 0)
                {
                    return new Cell[0];
                }

                Cell[] items = new Cell[this.SelectedCellCount];
                int count = 0;

                for (int i = 0; i < this.Cells.Count; i++)
                {
                    if (this.Cells[i].Selected)
                    {
                        items[count] = this.Cells[i];
                        count++;
                    }
                }

                return items;
            }
        }


        /// <summary>
        /// Returns an array that contains the indexes of all the selected Cells 
        /// within the Row
        /// </summary>
        [Browsable(false)]
        public int[] SelectedIndicies
        {
            get
            {
                if (this.Cells.Count == 0)
                {
                    return new int[0];
                }

                int[] indicies = new int[this.SelectedCellCount];
                int count = 0;

                for (int i = 0; i < this.Cells.Count; i++)
                {
                    if (this.Cells[i].Selected)
                    {
                        indicies[count] = i;
                        count++;
                    }
                }

                return indicies;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{B992A87F-D110-496A-870D-BF8B88D87AFF}</MetaDataID>
        protected virtual void OnPropertyChanged(RowEventArgs e)
        {
            e.SetRowIndex(this.Index);

            if (this.CanRaiseEvents)
            {
                if (this.TableModel != null)
                {
                    this.TableModel.OnRowPropertyChanged(e);
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellAdded event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{CF5F6228-A547-41CA-A982-B22FB0733427}</MetaDataID>
        protected internal virtual void OnCellAdded(RowEventArgs e)
        {
            e.SetRowIndex(this.Index);

            e.Cell.InternalRow = this;
            e.Cell.InternalIndex = e.CellFromIndex;
            e.Cell.SetSelected(false);

            this.UpdateCellIndicies(e.CellFromIndex);

            if (this.CanRaiseEvents)
            {
                if (this.TableModel != null)
                {
                    this.TableModel.OnCellAdded(e);
                }

                if (CellAdded != null)
                {
                    CellAdded(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellRemoved event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{C17C0324-BE32-4DC3-A1D7-4A11E0DCC037}</MetaDataID>
        protected internal virtual void OnCellRemoved(RowEventArgs e)
        {
            e.SetRowIndex(this.Index);

            if (e.Cell != null)
            {
                if (e.Cell.Row == this)
                {
                    e.Cell.InternalRow = null;
                    e.Cell.InternalIndex = -1;

                    if (e.Cell.Selected)
                    {
                        e.Cell.SetSelected(false);

                        this.InternalSelectedCellCount--;

                        if (this.SelectedCellCount == 0 && this.TableModel != null)
                        {
                            this.TableModel.Selections.RemoveRow(this);
                        }
                    }
                }
            }
            else
            {
                if (e.CellFromIndex == -1 && e.CellToIndex == -1)
                {
                    if (this.SelectedCellCount != 0 && this.TableModel != null)
                    {
                        this.InternalSelectedCellCount = 0;

                        this.TableModel.Selections.RemoveRow(this);
                    }
                }
            }

            this.UpdateCellIndicies(e.CellFromIndex);

            if (this.CanRaiseEvents)
            {
                if (this.TableModel != null)
                {
                    this.TableModel.OnCellRemoved(e);
                }

                if (CellRemoved != null)
                {
                    CellRemoved(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellPropertyChanged event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{BAC0BA3F-70AA-44D0-8E0F-20E1392F96CA}</MetaDataID>
        internal void OnCellPropertyChanged(CellEventArgs e)
        {
            if (this.TableModel != null)
            {
                this.TableModel.OnCellPropertyChanged(e);
            }
        }

        #endregion
    }    
}
