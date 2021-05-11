using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using ConnectableControls.SchedulerControl.RowDesign;
using System.Collections;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.TableModelDesign
{
    /// <summary>
    /// Represents a collection of TotalRows and Cells displayed in a Table.
    /// </summary>
    /// <MetaDataID>{E934D473-837C-4CBE-ADDD-B56BF91065D6}</MetaDataID>
    [DesignTimeVisible(false),ToolboxItem(false),ToolboxBitmap(typeof(TableModel))]
    public class TableModel : Component
    {
        #region Event Handlers

        /// <summary>
        /// Occurs when a Row is added to the TableModel
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

        #region Class Data

        /// <summary>
        /// The default height of a Row
        /// </summary>
        public static readonly int DefaultRowHeight = 24;

        /// <summary>
        /// The minimum height of a Row
        /// </summary>
        public static readonly int MinimumRowHeight = 24;

        /// <summary>
        /// The maximum height of a Row
        /// </summary>
        public static readonly int MaximumRowHeight = 1024;
        
        /// <summary>
        /// The Table that the TableModel belongs to
        /// </summary>
        private SchedulerListView table;

        /// <summary>
        /// The currently selected TotalRows and Cells
        /// </summary>
        private Selection selection;

        /// <summary>
        /// The height of each Row in the TableModel
        /// </summary>
        private int rowHeight;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TableModel class with default settings
        /// </summary>
        /// <MetaDataID>{5BBC5456-22D2-4384-BF6F-E3F2F2DAA211}</MetaDataID>
        public TableModel()
        {
            this.Init();
        }


        /// <summary>
        /// Initializes a new instance of the TableModel class with an array of Row objects
        /// </summary>
        /// <param name="TotalRows">An array of Row objects that represent the TotalRows 
        /// of the TableModel</param>
        /// <MetaDataID>{2EECE529-E690-465D-8B7B-1B0925C79894}</MetaDataID>
        public TableModel(Row[] TotalRows)
        {
            if (TotalRows == null)
            {
                throw new ArgumentNullException("TotalRows", "Row[] cannot be null");
            }

            this.Init();

            if (TotalRows.Length > 0)
            {
                this.TotalRows.AddRange(TotalRows);
            }
        }


        /// <summary>
        /// Initialise default settings
        /// </summary>
        /// <MetaDataID>{E0CE65A1-0123-43FB-B499-7C3DF00D8B8E}</MetaDataID>
        private void Init()
        {
            this._Rows = null;
            _RowsOnTime = 2;
            this.selection = new Selection(this);
            this.table = null;
            this.rowHeight = TableModel.DefaultRowHeight;
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Releases the unmanaged resources used by the TableModel and optionally 
        /// releases the managed resources
        /// </summary>
        /// <MetaDataID>{25366015-71F5-4772-B605-39B262283F6A}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Returns the index of the Row that lies on the specified position
        /// </summary>
        /// <param name="yPosition">The y-coordinate to check</param>
        /// <returns>The index of the Row at the specified position or -1 if 
        /// no Row is found</returns>
        /// <MetaDataID>{67842BA7-E7B5-44AC-B049-5E252C0656FC}</MetaDataID>
        public int RowIndexAt(int yPosition)
        {
            int row = yPosition / this.RowHeight;

            if (row < 0 || row > this.TotalRows.Count - 1)
            {
                return -1;
            }

            return row;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Cell located at the specified row index and column index
        /// </summary>
        /// <param name="row">The row index of the Cell</param>
        /// <param name="column">The column index of the Cell</param>
        [Browsable(false)]
        public Cell this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= this.TotalRows.Count)
                {
                    return null;
                }

                if (column < 0 || column >= this.TotalRows[row].Cells.Count)
                {
                    return null;
                }

                return this.TotalRows[row].Cells[column];
            }
        }


        /// <summary>
        /// Gets the Cell located at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        [Browsable(false)]
        public Cell this[CellPos cellPos]
        {
            get
            {
                return this[cellPos.Row, cellPos.Column];
            }
        }

        private List<Row> _Rows;
        /// <summary>
        /// Gets the Time rows this table model has
        /// </summary>
        [Category("Behavior"),
        Description("Row Collection"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //Editor(typeof(RowCollectionEditor), typeof(UITypeEditor))]
        public List<Row> Rows
        {
            get
            {
                if (this._Rows == null)
                {
                    this._Rows = new List<Row>();
                }

                return this._Rows;
            }
        }

        private RowCollection _TotalRows;
        /// <summary>
        /// Gets all the InnerRows of each Row
        /// </summary>
        public RowCollection TotalRows
        {
            get
            {
                if (_TotalRows == null)
                {
                    _TotalRows = new RowCollection(this);
                }
                
                
                
               
                return _TotalRows;
            }
        }

        public int TotalRowCount
        {
            get
            {
                if (TotalRows.Count == 0)
                    return 0;

                int num = Rows[0].InnerRows.Count;
                return num * Rows.Count;
            }
        }


        /// <summary>
        /// A TableModel.Selection representing the collection of selected
        /// TotalRows and Cells contained within the TableModel
        /// </summary>
        [Browsable(false)]
        public Selection Selections
        {
            get
            {
                if (this.selection == null)
                {
                    this.selection = new Selection(this);
                }

                return this.selection;
            }
        }


        /// <summary>
        /// Gets or sets the height of each Row in the TableModel
        /// </summary>
        [Category("Appearance"),
        Description("The height of each row")]
        public int RowHeight
        {
            get
            {
                return this.rowHeight;
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

                if (this.rowHeight != value)
                {
                    this.rowHeight = value;

                    this.OnRowHeightChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// Specifies whether the RowHeight property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the RowHeight property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{65029736-5DBD-46BA-95AA-C095424C406F}</MetaDataID>
        private bool ShouldSerializeRowHeight()
        {
            return this.rowHeight != TableModel.DefaultRowHeight;
        }


        /// <summary>
        /// Gets the total height of all the TotalRows in the TableModel
        /// </summary>
        [Browsable(false)]
        public int TotalRowHeight
        {
            get
            {
                return this.TotalRows.Count * this.RowHeight;
            }
        }


        /// <summary>
        /// Gets the Table the TableModel belongs to
        /// </summary>
        [Browsable(false)]
        public SchedulerListView Table
        {
            get
            {
                return this.table;
            }
        }


        /// <summary>
        /// Gets or sets the Table the TableModel belongs to
        /// </summary>
        internal SchedulerListView InternalTable
        {
            get
            {
                return this.table;
            }

            set
            {
                this.table = value;
            }
        }


        /// <summary>
        /// Gets whether the TableModel is able to raise events
        /// </summary>
        protected internal bool CanRaiseEvents
        {
            get
            {
                // check if the Table that the TableModel belongs to is able to 
                // raise events (if it can't, the TableModel shouldn't raise 
                // events either)
                if (this.Table != null)
                {
                    return this.Table.CanRaiseEvents;
                }

                return true;
            }
        }


        /// <summary>
        /// Gets whether the TableModel is enabled
        /// </summary>
        internal bool Enabled
        {
            get
            {
                if (this.Table == null)
                {
                    return true;
                }

                return this.Table.Enabled;
            }
        }


        /// <summary>
        /// Updates the Row's Index property so that it matches the TotalRows 
        /// position in the RowCollection
        /// </summary>
        /// <param name="start">The index to start updating from</param>
        /// <MetaDataID>{BD02624F-DCD1-4480-B044-7DB002D2CCC8}</MetaDataID>
        internal void UpdateRowIndicies(int start)
        {
            if (start == -1)
            {
                start = 0;
            }

            for (int i = start; i < this.TotalRows.Count; i++)
            {
                this.TotalRows[i].InternalIndex = i;
            }
        }

        #endregion

        public Row RowFromIndex(int rowindex)
        {
            return this.TotalRows[rowindex];
        }

        public int _RowsOnTime;
        public int RowsOnTime
        {
            get
            {
                return _RowsOnTime;
            }
        }

        public Row CreateRow(int time, Color forecolor, Color backcolor, Font font)
        {
            Row row = new Row(this,time,forecolor, backcolor, font);            
            this.Rows.Add(row);
            
            return row;
        }

        public void UpdateTotalRows()
        {
            for (int i = 0; i != Rows.Count; i++)
            {
                for (int k = 0; k != _Rows[i].InnerRows.Count; k++)
                    _TotalRows.Add(_Rows[i].InnerRows[k]);
            }
        }

        #region Events

        /// <summary>
        /// Raises the RowAdded event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{64F6A67D-3D14-4E21-B66E-72E30ED3EDD1}</MetaDataID>
        protected internal virtual void OnRowAdded(TableModelEventArgs e)
        {
            //e.Row.InternalTableModel = this;
            e.Row.InternalIndex = e.RowFromIndex;
            e.Row.ClearSelection();

            this.UpdateRowIndicies(e.RowFromIndex);

            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnRowAdded(e);
                }

                if (RowAdded != null)
                {
                    RowAdded(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowRemoved event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{F4630D92-67C4-46A2-B377-E46E3ADF3C4F}</MetaDataID>
        protected internal virtual void OnRowRemoved(TableModelEventArgs e)
        {
            if (e.Row != null && e.Row.TableModel == this)
            {
                e.Row.TableModel = null;
                e.Row.InternalIndex = -1;

                if (e.Row.AnyCellsSelected)
                {
                    e.Row.ClearSelection();

                    this.Selections.RemoveRow(e.Row);
                }
            }

            this.UpdateRowIndicies(e.RowFromIndex);

            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnRowRemoved(e);
                }

                if (RowRemoved != null)
                {
                    RowRemoved(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the SelectionChanged event
        /// </summary>
        /// <param name="e">A SelectionEventArgs that contains the event data</param>
        /// <MetaDataID>{D034F6F8-3DF0-46E5-A104-FDECDF820C99}</MetaDataID>
        protected virtual void OnSelectionChanged(SelectionEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnSelectionChanged(e);
                }

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
        /// <MetaDataID>{D27AB927-8045-46C8-A318-AA6995DA0B22}</MetaDataID>
        protected virtual void OnRowHeightChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnRowHeightChanged(e);
                }

                if (RowHeightChanged != null)
                {
                    RowHeightChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowPropertyChanged event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{38D52299-23AC-4D1A-BF1E-0F7D40A50CA1}</MetaDataID>
        internal void OnRowPropertyChanged(RowEventArgs e)
        {
            if (this.Table != null)
            {
                this.Table.OnRowPropertyChanged(e);
            }
        }


        /// <summary>
        /// Raises the CellAdded event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{30021DAD-8D45-478D-9D06-7E85AE0455B1}</MetaDataID>
        internal void OnCellAdded(RowEventArgs e)
        {
            if (this.Table != null)
            {
                this.Table.OnCellAdded(e);
            }
        }


        /// <summary>
        /// Raises the CellRemoved event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{4644AACF-058A-433B-880C-ECEC48FDF7A6}</MetaDataID>
        internal void OnCellRemoved(RowEventArgs e)
        {
            if (this.Table != null)
            {
                this.Table.OnCellRemoved(e);
            }
        }


        /// <summary>
        /// Raises the CellPropertyChanged event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{CB123B0F-1484-4F32-93C7-E3FD01B4F25C}</MetaDataID>
        internal void OnCellPropertyChanged(CellEventArgs e)
        {
            if (this.Table != null)
            {
                this.Table.OnCellPropertyChanged(e);
            }
        }

        #endregion

        #region Selection

        /// <summary>
        /// Represents the collection of selected TotalRows and Cells in a TableModel.
        /// </summary>
        /// <MetaDataID>{C5F70BA4-3EB8-4C38-A8AD-0F3C1E9F8ACE}</MetaDataID>
        public class Selection
        {
            #region Class Data

            /// <summary>
            /// The TableModel that owns the Selection
            /// </summary>
            private TableModel owner;

            /// <summary>
            /// The list of TotalRows that have selected Cells
            /// </summary>
            private ArrayList TotalRows;

            /// <summary>
            /// The starting cell of a selection that uses the shift key
            /// </summary>
            private CellPos shiftSelectStart;

            /// <summary>
            /// The ending cell of a selection that uses the shift key
            /// </summary>
            private CellPos shiftSelectEnd;

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the TableModel.Selection class 
            /// that belongs to the specified TableModel
            /// </summary>
            /// <param name="owner">A TableModel representing the tableModel that owns 
            /// the Selection</param>
            /// <MetaDataID>{D2C981E2-B9C7-4A36-A684-4F31056159B1}</MetaDataID>
            public Selection(TableModel owner)
            {
                if (owner == null)
                {
                    throw new ArgumentNullException("owner", "owner cannot be null");
                }

                this.owner = owner;
                this.TotalRows = new ArrayList();

                this.shiftSelectStart = CellPos.Empty;
                this.shiftSelectEnd = CellPos.Empty;
            }

            #endregion

            #region Methods

            #region Add

            /// <summary>
            /// Replaces the currently selected Cells with the Cell at the specified 
            /// row and column indexes
            /// </summary>
            /// <param name="row">The row index of the Cell to be selected</param>
            /// <param name="column">The column index of the Cell to be selected</param>
            /// <MetaDataID>{FB31A964-E9C5-44BC-9C74-C2FECCC96778}</MetaDataID>
            public void SelectCell(int row, int column)
            {
                // don't bother going any further if the cell 
                // is already selected
                if (this.TotalRows.Count == 1)
                {
                    Row r = (Row)this.TotalRows[0];

                    if (r.InternalIndex == row && r.SelectedCellCount == 1)
                    {
                        if (column >= 0 && column < r.Cells.Count)
                        {
                            if (r.Cells[column].Selected)
                            {
                                return;
                            }
                        }
                    }
                }

                this.SelectCells(row, column, row, column);
            }


            /// <summary>
            /// Replaces the currently selected Cells with the Cell at the specified CellPos
            /// </summary>
            /// <param name="cellPos">A CellPos thst specifies the row and column indicies of 
            /// the Cell to be selected</param>
            /// <MetaDataID>{2B81E8F7-B171-4747-9D80-A31A4B55D2C6}</MetaDataID>
            public void SelectCell(CellPos cellPos)
            {
                this.SelectCell(cellPos.Row, cellPos.Column);
            }


            /// <summary>
            /// Replaces the currently selected Cells with the Cells located between the specified 
            /// start and end row/column indicies
            /// </summary>
            /// <param name="startRow">The row index of the start Cell</param>
            /// <param name="startColumn">The column index of the start Cell</param>
            /// <param name="endRow">The row index of the end Cell</param>
            /// <param name="endColumn">The column index of the end Cell</param>
            /// <MetaDataID>{0B8F0A99-FB2A-46C7-805A-C610BAFE83AE}</MetaDataID>
            public void SelectCells(int startRow, int startColumn, int endRow, int endColumn)
            {
                int[] oldSelectedIndicies = this.SelectedIndicies;

                this.InternalClear();

                if (this.InternalAddCells(startRow, startColumn, endRow, endColumn))
                {
                    this.owner.OnSelectionChanged(new SelectionEventArgs(this.owner, oldSelectedIndicies, this.SelectedIndicies));
                }

                this.shiftSelectStart = new CellPos(startRow, startColumn);
                this.shiftSelectEnd = new CellPos(endRow, endColumn);
            }


            /// <summary>
            /// Replaces the currently selected Cells with the Cells located between the specified 
            /// start and end CellPos
            /// </summary>
            /// <param name="start">A CellPos that specifies the start Cell</param>
            /// <param name="end">A CellPos that specifies the end Cell</param>
            /// <MetaDataID>{864B5A45-2AEE-4E4D-8100-246F141F8920}</MetaDataID>
            public void SelectCells(CellPos start, CellPos end)
            {
                this.SelectCells(start.Row, start.Column, end.Row, end.Column);
            }


            /// <summary>
            /// Adds the Cell at the specified row and column indicies to the current selection
            /// </summary>
            /// <param name="row">The row index of the Cell to add to the selection</param>
            /// <param name="column">The column index of the Cell to add to the selection</param>
            /// <MetaDataID>{7B8B9662-7B10-497F-8B69-7888F311E541}</MetaDataID>
            public void AddCell(int row, int column)
            {
                this.AddCells(row, column, row, column);
            }


            /// <summary>
            /// Adds the Cell at the specified row and column indicies to the current selection
            /// </summary>
            /// <param name="cellPos">A CellPos that specifies the Cell to add to the selection</param>
            /// <MetaDataID>{31DD15DC-210E-4792-92E0-E5A23821FA87}</MetaDataID>
            public void AddCell(CellPos cellPos)
            {
                this.AddCell(cellPos.Row, cellPos.Column);
            }


            /// <summary>
            /// Adds the Cells located between the specified start and end row/column indicies 
            /// to the current selection
            /// </summary>
            /// <param name="startRow">The row index of the start Cell</param>
            /// <param name="startColumn">The column index of the start Cell</param>
            /// <param name="endRow">The row index of the end Cell</param>
            /// <param name="endColumn">The column index of the end Cell</param>
            /// <MetaDataID>{7CE70A8B-94A1-448E-BBFA-706A89FE4682}</MetaDataID>
            public void AddCells(int startRow, int startColumn, int endRow, int endColumn)
            {
                int[] oldSelectedIndicies = this.SelectedIndicies;

                if (InternalAddCells(startRow, startColumn, endRow, endColumn))
                {
                    this.owner.OnSelectionChanged(new SelectionEventArgs(this.owner, oldSelectedIndicies, this.SelectedIndicies));
                }

                this.shiftSelectStart = new CellPos(startRow, startColumn);
                this.shiftSelectEnd = new CellPos(endRow, endColumn);
            }


            /// <summary>
            /// Adds the Cells located between the specified start and end CellPos to the
            /// current selection
            /// </summary>
            /// <param name="start">A CellPos that specifies the start Cell</param>
            /// <param name="end">A CellPos that specifies the end Cell</param>
            /// <MetaDataID>{29896C4D-9D6E-4BCF-B69D-3D059298450A}</MetaDataID>
            public void AddCells(CellPos start, CellPos end)
            {
                this.AddCells(start.Row, start.Column, end.Row, end.Column);
            }


            /// <summary>
            /// Adds the Cells located between the specified start and end CellPos to the
            /// current selection without raising an event
            /// </summary>
            /// <param name="start">A CellPos that specifies the start Cell</param>
            /// <param name="end">A CellPos that specifies the end Cell</param>
            /// <returns>true if any Cells were added, false otherwise</returns>
            /// <MetaDataID>{25CED4B3-4F27-4291-8244-66C66E6A071C}</MetaDataID>
            private bool InternalAddCells(CellPos start, CellPos end)
            {
                return this.InternalAddCells(start.Row, start.Column, end.Row, end.Column);
            }


            /// <summary>
            /// Adds the Cells located between the specified start and end row/column indicies 
            /// to the current selection without raising an event
            /// </summary>
            /// <param name="startRow">The row index of the start Cell</param>
            /// <param name="startColumn">The column index of the start Cell</param>
            /// <param name="endRow">The row index of the end Cell</param>
            /// <param name="endColumn">The column index of the end Cell</param>
            /// <returns>true if any Cells were added, false otherwise</returns>
            /// <MetaDataID>{4D386214-0195-4E43-9F41-EFC20FA7A516}</MetaDataID>
            private bool InternalAddCells(int startRow, int startColumn, int endRow, int endColumn)
            {
                this.Normalise(ref startRow, ref endRow);
                this.Normalise(ref startColumn, ref endColumn);

                bool anyAdded = false;
                bool anyAddedInRow = false;

                for (int i = startRow; i <= endRow; i++)
                {
                    if (i >= this.owner.TotalRows.Count)
                    {
                        break;
                    }

                    Row r = this.owner.TotalRows[i];

                    for (int j = startColumn; j <= endColumn; j++)
                    {
                        if (j >= r.Cells.Count)
                        {
                            break;
                        }

                        if (!r.Cells[j].Selected && r.Cells[j].Enabled)
                        {
                            if (this.owner.Table != null && !this.owner.Table.IsCellEnabled(i, j))
                            {
                                continue;
                            }

                            r.Cells[j].SetSelected(true);
                            r.InternalSelectedCellCount++;

                            anyAdded = true;
                            anyAddedInRow = true;
                        }
                    }

                    if (anyAddedInRow && !this.TotalRows.Contains(r))
                    {
                        this.TotalRows.Add(r);
                    }

                    anyAddedInRow = false;
                }

                return anyAdded;
            }


            /// <summary>
            /// Adds the Cells between the last selection start Cell and the Cell at the 
            /// specified row/column indicies to the current selection.  Any Cells that are 
            /// between the last start and end Cells that are not in the new area are 
            /// removed from the current selection
            /// </summary>
            /// <param name="row">The row index of the shift selected Cell</param>
            /// <param name="column">The column index of the shift selected Cell</param>
            /// <MetaDataID>{8639128E-E3C7-4073-BFA0-0A37FAB9CF55}</MetaDataID>
            public void AddShiftSelectedCell(int row, int column)
            {
                int[] oldSelectedIndicies = this.SelectedIndicies;

                if (this.shiftSelectStart == CellPos.Empty)
                {
                    this.shiftSelectStart = new CellPos(0, 0);
                }

                bool changed = false;

                if (this.shiftSelectEnd != CellPos.Empty)
                {
                    changed = this.InternalRemoveCells(this.shiftSelectStart, this.shiftSelectEnd);
                    changed |= this.InternalAddCells(this.shiftSelectStart, new CellPos(row, column));
                }
                else
                {
                    changed = this.InternalAddCells(0, 0, row, column);
                }

                if (changed)
                {
                    this.owner.OnSelectionChanged(new SelectionEventArgs(this.owner, oldSelectedIndicies, this.SelectedIndicies));
                }

                this.shiftSelectEnd = new CellPos(row, column);
            }


            /// <summary>
            /// Adds the Cells between the last selection start Cell and the Cell at the 
            /// specified CellPas to the current selection.  Any Cells that are 
            /// between the last start and end Cells that are not in the new area are 
            /// removed from the current selection
            /// </summary>
            /// <param name="cellPos">A CellPos that specifies the shift selected Cell</param>
            /// <MetaDataID>{D8C9EFD4-FC63-42AC-86A6-F9823E665901}</MetaDataID>
            public void AddShiftSelectedCell(CellPos cellPos)
            {
                this.AddShiftSelectedCell(cellPos.Row, cellPos.Column);
            }


            /// <summary>
            /// Ensures that the first index is smaller than the second index, 
            /// performing a swap if necessary
            /// </summary>
            /// <param name="a">The first index</param>
            /// <param name="b">The second index</param>
            /// <MetaDataID>{4BE4B654-025F-488E-90ED-9A69B0DEF557}</MetaDataID>
            private void Normalise(ref int a, ref int b)
            {
                if (a < 0)
                {
                    a = 0;
                }

                if (b < 0)
                {
                    b = 0;
                }

                if (b < a)
                {
                    int temp = a;
                    a = b;
                    b = temp;
                }
            }

            #endregion

            #region Clear

            /// <summary>
            /// Removes all selected TotalRows and Cells from the selection
            /// </summary>
            /// <MetaDataID>{F1061572-75C9-4FBF-AEE7-D9FD81CB8A59}</MetaDataID>
            public void Clear()
            {
                if (this.TotalRows.Count > 0)
                {
                    int[] oldSelectedIndicies = this.SelectedIndicies;

                    this.InternalClear();

                    this.shiftSelectStart = CellPos.Empty;
                    this.shiftSelectEnd = CellPos.Empty;

                    this.owner.OnSelectionChanged(new SelectionEventArgs(this.owner, oldSelectedIndicies, this.SelectedIndicies));
                }
            }


            /// <summary>
            /// Removes all selected TotalRows and Cells from the selection without raising an event
            /// </summary>
            /// <MetaDataID>{5E8BC6EE-07EF-4E78-9634-D693D96B211C}</MetaDataID>
            private void InternalClear()
            {
                if (this.TotalRows.Count > 0)
                {
                    for (int i = 0; i < this.TotalRows.Count; i++)
                    {
                        ((Row)this.TotalRows[i]).ClearSelection();
                    }

                    this.TotalRows.Clear();
                    this.TotalRows.Capacity = 0;
                }
            }

            #endregion

            #region Remove

            /// <summary>
            /// Removes the Cell at the specified row and column indicies from the current selection
            /// </summary>
            /// <param name="row">The row index of the Cell to remove from the selection</param>
            /// <param name="column">The column index of the Cell to remove from the selection</param>
            /// <MetaDataID>{692954C0-AFF6-4D21-A99C-F0D8E7C602E5}</MetaDataID>
            public void RemoveCell(int row, int column)
            {
                this.RemoveCells(row, column, row, column);
            }


            /// <summary>
            /// Removes the Cell at the specified row and column indicies from the current selection
            /// </summary>
            /// <param name="cellPos">A CellPos that specifies the Cell to remove from the selection</param>
            /// <MetaDataID>{90BED8F6-6BFF-4F2A-87B1-B2A4653C24B8}</MetaDataID>
            public void RemoveCell(CellPos cellPos)
            {
                this.RemoveCell(cellPos.Row, cellPos.Column);
            }


            /// <summary>
            /// Removes the Cells located between the specified start and end row/column indicies 
            /// from the current selection
            /// </summary>
            /// <param name="startRow">The row index of the start Cell</param>
            /// <param name="startColumn">The column index of the start Cell</param>
            /// <param name="endRow">The row index of the end Cell</param>
            /// <param name="endColumn">The column index of the end Cell</param>
            /// <MetaDataID>{81955F0F-8141-48B2-8286-FA546BB4101E}</MetaDataID>
            public void RemoveCells(int startRow, int startColumn, int endRow, int endColumn)
            {
                if (this.TotalRows.Count > 0)
                {
                    int[] oldSelectedIndicies = this.SelectedIndicies;

                    if (this.InternalRemoveCells(startRow, startColumn, endRow, endColumn))
                    {
                        this.owner.OnSelectionChanged(new SelectionEventArgs(this.owner, oldSelectedIndicies, this.SelectedIndicies));
                    }

                    this.shiftSelectStart = new CellPos(startRow, startColumn);
                    this.shiftSelectEnd = new CellPos(endRow, endColumn);
                }
            }


            /// <summary>
            /// Removes the Cells located between the specified start and end CellPos from the
            /// current selection
            /// </summary>
            /// <param name="start">A CellPos that specifies the start Cell</param>
            /// <param name="end">A CellPos that specifies the end Cell</param>
            /// <MetaDataID>{57EFD494-465E-4C09-A6E9-5926FFE340E4}</MetaDataID>
            public void RemoveCells(CellPos start, CellPos end)
            {
                this.RemoveCells(start.Row, start.Column, end.Row, end.Column);
            }


            /// <summary>
            /// Removes the Cells located between the specified start and end CellPos from the
            /// current selection without raising an event
            /// </summary>
            /// <param name="start">A CellPos that specifies the start Cell</param>
            /// <param name="end">A CellPos that specifies the end Cell</param>
            /// <returns>true if any Cells were added, false otherwise</returns>
            /// <MetaDataID>{196C9B97-FD63-49DB-8021-9E336DECA1AA}</MetaDataID>
            private bool InternalRemoveCells(CellPos start, CellPos end)
            {
                return this.InternalRemoveCells(start.Row, start.Column, end.Row, end.Column);
            }


            /// <summary>
            /// Removes the Cells located between the specified start and end row/column indicies 
            /// from the current selection without raising an event
            /// </summary>
            /// <param name="startRow">The row index of the start Cell</param>
            /// <param name="startColumn">The column index of the start Cell</param>
            /// <param name="endRow">The row index of the end Cell</param>
            /// <param name="endColumn">The column index of the end Cell</param>
            /// <returns>true if any Cells were added, false otherwise</returns>
            /// <MetaDataID>{679635EB-C271-498D-A9E9-ECE4FF7253EE}</MetaDataID>
            private bool InternalRemoveCells(int startRow, int startColumn, int endRow, int endColumn)
            {
                this.Normalise(ref startRow, ref endRow);
                this.Normalise(ref startColumn, ref endColumn);

                bool anyRemoved = false;

                for (int i = startRow; i <= endRow; i++)
                {
                    if (i >= this.owner.TotalRows.Count)
                    {
                        break;
                    }

                    Row r = this.owner.TotalRows[i];

                    for (int j = startColumn; j <= endColumn; j++)
                    {
                        if (j >= r.Cells.Count)
                        {
                            break;
                        }

                        if (r.Cells[j].Selected)
                        {
                            r.Cells[j].SetSelected(false);
                            r.InternalSelectedCellCount--;

                            anyRemoved = true;
                        }
                    }

                    if (!r.AnyCellsSelected)
                    {
                        if (this.TotalRows.Contains(r))
                        {
                            this.TotalRows.Remove(r);
                        }
                    }
                }

                return anyRemoved;
            }


            /// <summary>
            /// Removes the specified Row from the selection
            /// </summary>
            /// <param name="row">The Row to be removed from the selection</param>
            /// <MetaDataID>{8626EA21-D628-486A-A840-61EC9AC9AEF7}</MetaDataID>
            internal void RemoveRow(Row row)
            {
                if (this.TotalRows.Contains(row))
                {
                    int[] oldSelectedIndicies = this.SelectedIndicies;

                    this.TotalRows.Remove(row);

                    this.owner.OnSelectionChanged(new SelectionEventArgs(this.owner, oldSelectedIndicies, this.SelectedIndicies));
                }
            }

            #endregion

            #region Queries

            /// <summary>
            /// Returns whether the Cell at the specified row and column indicies is 
            /// currently selected
            /// </summary>
            /// <param name="row">The row index of the specified Cell</param>
            /// <param name="column">The column index of the specified Cell</param>
            /// <returns>true if the Cell at the specified row and column indicies is 
            /// selected, false otherwise</returns>
            /// <MetaDataID>{091EE844-88AA-4240-8452-564B63C236CC}</MetaDataID>
            public bool IsCellSelected(int row, int column)
            {
                if (row < 0 || row >= this.owner.TotalRows.Count)
                {
                    return false;
                }

                return this.owner.TotalRows[row].IsCellSelected(column);
            }


            /// <summary>
            /// Returns whether the Cell at the specified CellPos is currently selected
            /// </summary>
            /// <param name="cellPos">A CellPos the represents the row and column indicies 
            /// of the Cell to check</param>
            /// <returns>true if the Cell at the specified CellPos is currently selected, 
            /// false otherwise</returns>
            /// <MetaDataID>{A11612F7-9511-4888-8F0F-46171C317F87}</MetaDataID>
            public bool IsCellSelected(CellPos cellPos)
            {
                return this.IsCellSelected(cellPos.Row, cellPos.Column);
            }


            /// <summary>
            /// Returns whether the Row at the specified index in th TableModel is 
            /// currently selected
            /// </summary>
            /// <param name="index">The index of the Row to check</param>
            /// <returns>true if the Row at the specified index is currently selected, 
            /// false otherwise</returns>
            /// <MetaDataID>{8C01123E-CB29-4BC7-A729-27152E2F796A}</MetaDataID>
            public bool IsRowSelected(int index)
            {
                if (index < 0 || index >= this.owner.TotalRows.Count)
                {
                    return false;
                }

                return this.owner.TotalRows[index].AnyCellsSelected;
            }

            #endregion

            #endregion

            #region Properties

            /// <summary>
            /// Gets an array that contains the currently selected TotalRows
            /// </summary>
            public Row[] SelectedItems
            {
                get
                {
                    if (this.TotalRows.Count == 0)
                    {
                        return new Row[0];
                    }

                    //this.TotalRows.Sort(new RowComparer());

                    return (Row[])this.TotalRows.ToArray(typeof(Row));
                }
            }


            /// <summary>
            /// Gets an array that contains the indexes of the currently selected TotalRows
            /// </summary>
            public int[] SelectedIndicies
            {
                get
                {
                    if (this.TotalRows.Count == 0)
                    {
                        return new int[0];
                    }

                    //this.TotalRows.Sort(new RowComparer());

                    int[] indicies = new int[this.TotalRows.Count];

                    for (int i = 0; i < this.TotalRows.Count; i++)
                    {
                        indicies[i] = ((Row)this.TotalRows[i]).InternalIndex;
                    }

                    return indicies;
                }
            }


            /// <summary>
            /// Returns a Rectange that bounds the currently selected TotalRows
            /// </summary>
            public Rectangle SelectionBounds
            {
                get
                {
                    if (this.TotalRows.Count == 0)
                    {
                        return Rectangle.Empty;
                    }

                    int[] indicies = this.SelectedIndicies;

                    return this.CalcSelectionBounds(indicies[0], indicies[indicies.Length - 1]);
                }
            }


            /// <summary>
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            /// <MetaDataID>{DDE10F58-D77C-4A69-AD47-97342E0D175D}</MetaDataID>
            internal Rectangle CalcSelectionBounds(int start, int end)
            {
                this.Normalise(ref start, ref end);

                Rectangle bounds = new Rectangle();

                if (this.owner.Table != null && this.owner.Table.ColumnModel != null)
                {
                    bounds.Width = System.Convert.ToInt32(this.owner.Table.ColumnModel.VisibleColumnsWidth);
                }

                bounds.Y = start * this.owner.RowHeight;

                if (start == end)
                {
                    bounds.Height = this.owner.RowHeight;
                }
                else
                {
                    bounds.Height = ((end + 1) * this.owner.RowHeight) - bounds.Y;
                }

                return bounds;
            }

            #endregion
        }

        #endregion
    }
}
