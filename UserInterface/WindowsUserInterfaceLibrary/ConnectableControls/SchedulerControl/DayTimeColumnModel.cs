using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using ConnectableControls.SchedulerControl.Renderers;
using ConnectableControls.SchedulerControl.Events;
using System.Windows.Forms;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{54ce8117-fa0a-46db-8683-c2a5c3e8b4de}</MetaDataID>
    [DesignTimeVisible(false), ToolboxItem(false)]
    public class DayTimeColumnModel:Component
    {
        #region public void ClearColumnsButThis(DayColumn column)
        /// <MetaDataID>{b0bb5294-d5fb-43d0-9b20-efa16a697490}</MetaDataID>
        public void ClearColumnsButThis(DayColumn column)
        {
            List<Column> columns = new List<Column>();
            for (int i = 1; i != 8; i++)
            {
                if (this.Columns[i] == column)
                    continue;

                columns.Add(this.Columns[i]);
            }
            foreach (Column clm in columns)
                this.Columns.Remove(clm);
        } 
        #endregion

        #region internal Rectangle DayRectangle
        /// <MetaDataID>{2804638b-a3bf-4d18-9796-23a3a82067db}</MetaDataID>
        private Rectangle _DayRectangle;
        /// <MetaDataID>{32143533-33a1-410e-b7a7-e19b18a76a4c}</MetaDataID>
        internal Rectangle DayRectangle
        {
            get
            {
                if (_DayRectangle.Width != this.Columns[1].Width)
                    _DayRectangle.Width = System.Convert.ToInt32(this.Columns[1].Width);

                return _DayRectangle;
            }
        } 
        #endregion

        #region public int ColumnIndexAtX(int xPosition) 

        /// <summary>
        /// Returns the index of the Column that lies on the specified position
        /// </summary>
        /// <param name="xPosition">The x-coordinate to check</param>
        /// <returns>The index of the Column or -1 if no Column is found</returns>
        /// <MetaDataID>{C237057D-252C-455B-8B6D-42BD4F2469AD}</MetaDataID>
		public int ColumnIndexAtX(int xPosition) 
		{
			if (xPosition < 0 || xPosition > this.VisibleColumnsWidth)
			{
				return -1;
			}

			for (int i=0; i<this.Columns.Count; i++)
			{
				if (this.Columns[i].Visible && xPosition < this.Columns[i].Right)
				{
					return i;
				}
			}

			return -1;
		}


        /// <summary>
        /// Returns the Column that lies on the specified position
        /// </summary>
        /// <param name="xPosition">The x-coordinate to check</param>
        /// <returns>The Column that lies on the specified position, 
        /// or null if not found</returns>
        /// <MetaDataID>{04292E14-E589-43E3-B4CE-7C83D056746B}</MetaDataID>
		public Column ColumnAtX(int xPosition) 
		{
			if (xPosition < 0 || xPosition > this.VisibleColumnsWidth)
			{
				return null;
			}
			
			int index = this.ColumnIndexAtX(xPosition);

			if (index != -1)
			{
				return this.Columns[index];
			}

			return null;
		}


        /// <summary>
        /// Returns a rectangle that countains the header of the column 
        /// at the specified index in the ColumnModel
        /// </summary>
        /// <param name="index">The index of the column</param>
        /// <returns>that countains the header of the specified column</returns>
        /// <MetaDataID>{C711235F-A574-4FBB-944E-FCC95AD5AF67}</MetaDataID>
		public Rectangle ColumnHeaderRect(int index)
		{
			// make sure the index is valid and the column is not hidden
			if (index < 0 || index >= this.Columns.Count || !this.Columns[index].Visible)
			{
				return Rectangle.Empty;
			}

			return new Rectangle((System.Convert.ToInt32(this.Columns[index].Left)), 0, System.Convert.ToInt32(this.Columns[index].Width), this.HeaderHeight);
		}


        /// <summary>
        /// Returns a rectangle that countains the header of the specified column
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>A rectangle that countains the header of the specified column</returns>
        /// <MetaDataID>{005343D1-61C5-4673-B1D9-70920B53E0EB}</MetaDataID>
		public Rectangle ColumnHeaderRect(Column column)
		{
			// check if we actually own the column
			int index = this.Columns.IndexOf(column);
			
			if (index == -1)
			{
				return Rectangle.Empty;
			}

			return this.ColumnHeaderRect(index);
		}

		#endregion

		#region protected override void Dispose(bool disposing)

        /// <summary> 
        /// Releases the unmanaged resources used by the ColumnModel and optionally 
        /// releases the managed resources
        /// </summary>
        /// <MetaDataID>{8D457E6F-A7FF-45D6-8D96-57C7C48E8D09}</MetaDataID>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				
			}

			base.Dispose(disposing);
		}

		#endregion

		#region public ICellEditor GetCellEditor(string name)

        /// <summary>
        /// Returns the ICellEditor that is associated with the specified name
        /// </summary>
        /// <param name="name">The name thst is associated with an ICellEditor</param>
        /// <returns>The ICellEditor that is associated with the specified name, 
        /// or null if the name or ICellEditor do not exist</returns>
        /// <MetaDataID>{80183C32-C474-4058-8110-CE6DFDB6528D}</MetaDataID>
		public ICellEditor GetCellEditor(string name)
		{
            throw new NotImplementedException("GetCellEditor is not implemented");
            //if (name == null || name.Length == 0)
            //{
            //    return null;
            //}
			
            //name = name.ToUpper();
			
            //if (!this.cellEditors.ContainsKey(name))
            //{
            //    if (this.cellEditors.Count == 0)
            //    {
            //        this.SetCellEditor("TEXT", new TextCellEditor());
            //    }
				
            //    return null;
            //}

            //return (ICellEditor) this.cellEditors[name];
		}

        #endregion

        #region public ICellEditor GetCellEditor(int column)
		/// <summary>
        /// Gets the ICellEditor for the Column at the specified index in the 
        /// ColumnModel
        /// </summary>
        /// <param name="column">The index of the Column in the ColumnModel for 
        /// which an ICellEditor will be retrieved</param>
        /// <returns>The ICellEditor for the Column at the specified index, or 
        /// null if the editor does not exist</returns>
        /// <MetaDataID>{D2E683D5-13B4-4DB7-8BA6-A2D9A3CEE4CE}</MetaDataID>
		public ICellEditor GetCellEditor(int column)
		{
            throw new NotImplementedException("GetCellEditor is not implemented");

            //if (column < 0 || column >= this.Columns.Count)
            //{
            //    return null;
            //}

            ////
            //if (this.Columns[column].Editor != null)
            //{
            //    return this.Columns[column].Editor;
            //}

            //return this.GetCellEditor(this.Columns[column].GetDefaultEditorName());
		} 
	   #endregion

        #region public void SetCellEditor(string name, ICellEditor editor)
		/// <summary>
        /// Associates the specified ICellRenderer with the specified name
        /// </summary>
        /// <param name="name">The name to be associated with the specified ICellEditor</param>
        /// <param name="editor">The ICellEditor to be added to the ColumnModel</param>
        /// <MetaDataID>{B14BA01D-D3F9-498D-9636-C5B2859F09EF}</MetaDataID>
		public void SetCellEditor(string name, ICellEditor editor)
		{
			if (name == null || name.Length == 0 || editor == null)
			{
				return;
			}
			
			name = name.ToUpper();
			
			if (this.cellEditors.ContainsKey(name))
			{	
				this.cellEditors.Remove(name);
				
				this.cellEditors[name] = editor;
			}
			else
			{
				this.cellEditors.Add(name, editor);
			}
		} 
	#endregion

        #region public bool ContainsCellEditor(string name)
		/// <summary>
        /// Gets whether the ColumnModel contains an ICellEditor with the 
        /// specified name
        /// </summary>
        /// <param name="name">The name associated with the ICellEditor</param>
        /// <returns>true if the ColumnModel contains an ICellEditor with the 
        /// specified name, false otherwise</returns>
        /// <MetaDataID>{300578EF-BC3E-4773-93E5-787BC57B8898}</MetaDataID>
		public bool ContainsCellEditor(string name)
		{
			if (name == null)
			{
				return false;
			}

			return this.cellEditors.ContainsKey(name);
		}
 
	#endregion

		#region internal int EditorCount
        /// <summary>
        /// Gets the number of ICellEditors contained in the ColumnModel
        /// </summary>
        /// <MetaDataID>{b195afd6-003b-4430-931a-fa3b422d09f3}</MetaDataID>
		internal int EditorCount
		{
			get
			{
				return this.cellEditors.Count;
			}
		} 
	    #endregion

        #region public ICellRenderer GetCellRenderer(string name)
		/// <summary>
        /// Returns the ICellRenderer that is associated with the specified name
        /// </summary>
        /// <param name="name">The name thst is associated with an ICellEditor</param>
        /// <returns>The ICellRenderer that is associated with the specified name, 
        /// or null if the name or ICellRenderer do not exist</returns>
        /// <MetaDataID>{6D234861-B3B7-4C61-9D0F-C3EB6E2B644F}</MetaDataID>
		public ICellRenderer GetCellRenderer(string name)
		{
			if (name == null)
			{
				name = "TEXT";
			}
			
			name = name.ToUpper();
			
			if (!this.cellRenderers.ContainsKey(name))
			{
				if (this.cellRenderers.Count == 0)
				{
					this.SetCellRenderer("TEXT", new TextCellRenderer());
				}
				
				return (ICellRenderer) this.cellRenderers["TEXT"];
			}

			return (ICellRenderer) this.cellRenderers[name];
		} 
	    #endregion

        #region public ICellRenderer GetCellRenderer(int column)
		/// <summary>
        /// Gets the ICellRenderer for the Column at the specified index in the 
        /// ColumnModel
        /// </summary>
        /// <param name="column">The index of the Column in the ColumnModel for 
        /// which an ICellRenderer will be retrieved</param>
        /// <returns>The ICellRenderer for the Column at the specified index, or 
        /// null if the renderer does not exist</returns>
        /// <MetaDataID>{880BCFB8-4DD6-41D3-ACF6-83E20F2F4E2A}</MetaDataID>
		public ICellRenderer GetCellRenderer(int column)
		{
			//
			if (column < 0 || column >= this.Columns.Count)
			{
				return null;
			}

			//
			if (this.Columns[column].Renderer != null)
			{
				return this.Columns[column].Renderer;
			}

			//
			return this.GetCellRenderer(this.Columns[column].GetDefaultRendererName());
		} 
	#endregion

        #region public void SetCellRenderer(string name, ICellRenderer renderer)
		/// <summary>
        /// Associates the specified ICellRenderer with the specified name
        /// </summary>
        /// <param name="name">The name to be associated with the specified ICellRenderer</param>
        /// <param name="renderer">The ICellRenderer to be added to the ColumnModel</param>
        /// <MetaDataID>{DE07922C-74ED-4E6C-B925-8E864E792CB1}</MetaDataID>
		public void SetCellRenderer(string name, ICellRenderer renderer)
		{
			if (name == null || renderer == null)
			{
				return;
			}
			
			name = name.ToUpper();
			
			if (this.cellRenderers.ContainsKey(name))
			{	
				this.cellRenderers.Remove(name);
				
				this.cellRenderers[name] = renderer;
			}
			else
			{
				this.cellRenderers.Add(name, renderer);
			}
		} 
	#endregion

        #region public bool ContainsCellRenderer(string name)
		/// <summary>
        /// Gets whether the ColumnModel contains an ICellRenderer with the 
        /// specified name
        /// </summary>
        /// <param name="name">The name associated with the ICellRenderer</param>
        /// <returns>true if the ColumnModel contains an ICellRenderer with the 
        /// specified name, false otherwise</returns>
        /// <MetaDataID>{787BA5D2-DA08-4926-BB2D-3BDF661FB4D4}</MetaDataID>
		public bool ContainsCellRenderer(string name)
		{
			if (name == null)
			{
				return false;
			}

			return this.cellRenderers.ContainsKey(name);
		} 
	#endregion

		#region internal int RendererCount
        /// <summary>
        /// Gets the number of ICellRenderers contained in the ColumnModel
        /// </summary>
        /// <MetaDataID>{05991b1a-412b-4be0-8ee8-9a59ecb10077}</MetaDataID>
		internal int RendererCount
		{
			get
			{
				return this.cellRenderers.Count;
			}
		}
 
	    #endregion

		#region public int PreviousVisibleColumn(int index)

        /// <summary>
        /// Returns the index of the first visible Column that is to the 
        /// left of the Column at the specified index in the ColumnModel
        /// </summary>
        /// <param name="index">The index of the Column for which the first 
        /// visible Column that is to the left of the specified Column is to 
        /// be found</param>
        /// <returns>the index of the first visible Column that is to the 
        /// left of the Column at the specified index in the ColumnModel, or 
        /// -1 if the Column at the specified index is the first visible column, 
        /// or there are no Columns in the Column model</returns>
        /// <MetaDataID>{E1E0D60F-0710-4108-BF69-24354670EE9B}</MetaDataID>
		public int PreviousVisibleColumn(int index)
		{
			if (this.Columns.Count == 0)
			{
				return -1;
			}
			
			if (index <= 0)
			{
				return -1;
			}

			if (index >= this.Columns.Count)
			{
				if (this.Columns[this.Columns.Count-1].Visible)
				{
					return this.Columns.Count - 1;
				}
				
				index = this.Columns.Count - 1;
			}

			for (int i=index; i>0; i--)
			{
				if (this.Columns[i-1].Visible)
				{
					return i - 1;
				}
			}

			return -1;
		}

        #endregion

        #region public int NextVisibleColumn(int index)
		/// <summary>
        /// Returns the index of the first visible Column that is to the 
        /// right of the Column at the specified index in the ColumnModel
        /// </summary>
        /// <param name="index">The index of the Column for which the first 
        /// visible Column that is to the right of the specified Column is to 
        /// be found</param>
        /// <returns>the index of the first visible Column that is to the 
        /// right of the Column at the specified index in the ColumnModel, or 
        /// -1 if the Column at the specified index is the last visible column, 
        /// or there are no Columns in the Column model</returns>
        /// <MetaDataID>{19BDDC57-C064-434A-97C6-1047F168818D}</MetaDataID>
		public int NextVisibleColumn(int index)
		{
			if (this.Columns.Count == 0)
			{
				return -1;
			}
			
			if (index >= this.Columns.Count - 1)
			{
				return -1;
			}

			for (int i=index; i<this.Columns.Count-1; i++)
			{
				if (this.Columns[i+1].Visible)
				{
					return i + 1;
				}
			}

			return -1;
		} 
	#endregion

        #region Properties

        /// <MetaDataID>{6df94913-9ca1-4c12-987e-508a3415c050}</MetaDataID>
        private ColumnCollection _Columns;
        /// <summary>
        /// A ColumnCollection representing the collection of 
        /// Columns contained within the ColumnModel
        /// </summary>
        /// <MetaDataID>{709c257d-31e7-4fef-8f9d-7991e4ddbe72}</MetaDataID>
        [Category("Behavior"),
        Description("Column Collection"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(ColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ColumnCollection Columns
        {
            get
            {
                if (this._Columns == null)
                {
                    this._Columns = new ColumnCollection(this);
                }

                return this._Columns;
            }
        }


        /// <MetaDataID>{c9f706b7-5fb7-407e-96f6-1f271ac346ef}</MetaDataID>
        private int _HeaderHeight;
        /// <summary>
        /// Gets or sets the height of the column headers
        /// </summary>
        /// <MetaDataID>{90938cc9-70f8-4199-ab38-ee045e44c2f3}</MetaDataID>
        [Category("Appearance"),
        Description("The height of the column headers")]
        public int HeaderHeight
        {
            get
            {
                return this._HeaderHeight;
            }

            set
            {
                if (value < DayTimeColumnModel.MinimumHeaderHeight)
                {
                    value = DayTimeColumnModel.MinimumHeaderHeight;
                }
                else if (value > DayTimeColumnModel.MaximumHeaderHeight)
                {
                    value = DayTimeColumnModel.MaximumHeaderHeight;
                }

                if (this._HeaderHeight != value)
                {
                    this._HeaderHeight = value;

                    this.OnHeaderHeightChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raises the HeaderHeightChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{F1D409E9-6F32-4973-A3BD-0C638944A906}</MetaDataID>
        protected virtual void OnHeaderHeightChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnHeaderHeightChanged(e);
                }

                if (HeaderHeightChanged != null)
                {
                    HeaderHeightChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Specifies whether the HeaderHeight property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the HeaderHeight property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{0A5E6FB8-88E9-43A9-9F73-D37E152E7BFA}</MetaDataID>
        private bool ShouldSerializeHeaderHeight()
        {
            return this._HeaderHeight != DayTimeColumnModel.DefaultHeaderHeight;
        }


        /// <summary>
        /// Gets a rectangle that specifies the width and height of all the 
        /// visible column headers in the model
        /// </summary>
        /// <MetaDataID>{6cde6571-3be9-4f9a-994d-b9715822561c}</MetaDataID>
        [Browsable(false)]
        public Rectangle HeaderRect
        {
            get
            {
                if (this.VisibleColumnCount == 0)
                {
                    return Rectangle.Empty;
                }

                return new Rectangle(0, 0, System.Convert.ToInt32(this.VisibleColumnsWidth), this.HeaderHeight);
            }
        }


        /// <summary>
        /// Gets the total width of all the Columns in the model
        /// </summary>
        /// <MetaDataID>{cc31afff-63a4-4e0b-8164-fc6843d2197d}</MetaDataID>
        [Browsable(false)]
        public decimal TotalColumnWidth
        {
            get
            {
                return this.Columns.TotalColumnWidth;
            }
        }


        /// <summary>
        /// Gets the total width of all the visible Columns in the model
        /// </summary>
        /// <MetaDataID>{a08e5403-ea0a-4b22-8f20-56417630d989}</MetaDataID>
        [Browsable(false)]
        public decimal VisibleColumnsWidth
        {
            get
            {
                return this.Columns.VisibleColumnsWidth;
            }
        }


        /// <summary>
        /// Gets the index of the last Column that is not hidden
        /// </summary>
        /// <MetaDataID>{77cf31a8-ea9f-418d-8a8d-8cb2796556e6}</MetaDataID>
        [Browsable(false)]
        public int LastVisibleColumnIndex
        {
            get
            {
                return this.Columns.LastVisibleColumn;
            }
        }


        /// <summary>
        /// Gets the number of Columns in the ColumnModel that are visible
        /// </summary>
        /// <MetaDataID>{336afe36-ad07-4c7c-bb02-0c60deaf9c11}</MetaDataID>
        [Browsable(false)]
        public int VisibleColumnCount
        {
            get
            {
                return this.Columns.VisibleColumnCount;
            }
        }

        /// <MetaDataID>{7843b921-5c19-4c8a-8715-638963024ee9}</MetaDataID>
        private SchedulerListView _Table;
        /// <summary>
        /// Gets the Table the ColumnModel belongs to
        /// </summary>
        /// <MetaDataID>{f79a8c5f-4c5f-411f-84ed-f61ba9a7d33e}</MetaDataID>
        [Browsable(false)]
        public SchedulerListView Table
        {
            get
            {
                return this._Table;
            }
        }


        /// <summary>
        /// Gets or sets the Table the ColumnModel belongs to
        /// </summary>
        /// <MetaDataID>{1f49fde3-7975-4df9-82ab-b354dbf1f900}</MetaDataID>
        internal SchedulerListView InternalTable
        {
            get
            {
                return this._Table;
            }

            set
            {
                this._Table = value;
            }
        }


        /// <summary>
        /// Gets whether the ColumnModel is able to raise events
        /// </summary>
        /// <MetaDataID>{ffe9ee7b-3cc7-4bd1-9697-6e7f8278074c}</MetaDataID>
        protected internal bool CanRaiseEvents
        {
            get
            {
                // check if the Table that the ColumModel belongs to is able to 
                // raise events (if it can't, the ColumModel shouldn't raise 
                // events either)
                if (this.Table != null)
                {
                    return this.Table.CanRaiseEvents;
                }

                return true;
            }
        }


        /// <summary>
        /// Gets whether the ColumnModel is enabled
        /// </summary>
        /// <MetaDataID>{b627a89a-2cad-465e-8562-1676f687bd25}</MetaDataID>
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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ColumnModel class with default settings
        /// </summary>
        /// <MetaDataID>{DA304587-CF36-4321-91C7-27C2D80C43EF}</MetaDataID>
		public DayTimeColumnModel()
		{
			this.Init();
		}


        /// <summary>
        /// Initializes a new instance of the ColumnModel class with an array of strings 
        /// representing TextColumns
        /// </summary>
        /// <param name="columns">An array of strings that represent the Columns of 
        /// the ColumnModel</param>
        /// <MetaDataID>{05F51071-2E68-44A2-B65D-09DCA52046EF}</MetaDataID>
        public DayTimeColumnModel(string[] columns)
		{
			if (columns == null)
			{
				throw new ArgumentNullException("columns", "string[] cannot be null");
			}
			
			this.Init();

			if (columns.Length > 0)
			{
				Column[] cols = new Column[columns.Length];

				for (int i=0; i<columns.Length; i++)
				{
					cols[i] = new TextColumn(columns[i]);
				}

				this.Columns.AddRange(cols);
			}
		}


        /// <summary>
        /// Initializes a new instance of the Row class with an array of Column objects
        /// </summary>
        /// <param name="columns">An array of Cell objects that represent the Columns 
        /// of the ColumnModel</param>
        /// <MetaDataID>{8989DFA2-C194-47EC-AF58-E04C0CAA0FAD}</MetaDataID>
		public DayTimeColumnModel(Column[] columns)
		{
			if (columns == null)
			{
				throw new ArgumentNullException("columns", "Column[] cannot be null");
			}
			
			this.Init();

			if (columns.Length > 0)
			{
				this.Columns.AddRange(columns);
			}
		}


        /// <summary>
        /// Initialise default settings
        /// </summary>
        /// <MetaDataID>{DDF557C6-A6F4-40BD-A5D2-968CE43EF695}</MetaDataID>
		private void Init()
		{
			this._Columns = null;

            this._Table = null;
			this.HeaderHeight = DayTimeColumnModel.DefaultHeaderHeight;

			this.cellRenderers = new Hashtable();
			this.SetCellRenderer("TEXT", new TextCellRenderer());

            //this.cellEditors = new Hashtable();
            //this.SetCellEditor("TEXT", new TextCellEditor());
		}

		#endregion

        #region Events
        /// <summary>
        /// Occurs when a Column has been added to the ColumnModel
        /// </summary>
        public event ColumnModelEventHandler ColumnAdded;

        /// <summary>
        /// Occurs when a Column is removed from the ColumnModel
        /// </summary>
        public event ColumnModelEventHandler ColumnRemoved;

        /// <summary>
        /// Occurs when the value of the HeaderHeight property changes
        /// </summary>
        public event EventHandler HeaderHeightChanged; 
        #endregion

        #region properties

        /// <MetaDataID>{d2c6e0ed-3f49-4156-bf9d-e7abed6ce5a7}</MetaDataID>
        internal Image ZoomInDaysImage;
        /// <MetaDataID>{19e0a639-1a21-47d6-91a5-614e273c8251}</MetaDataID>
        internal Image ZoomOutDaysImage;

        /// <summary>
        /// Creates a column with the specified date and returns it
        /// </summary>
        /// <param name="description"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <MetaDataID>{656dab9b-2432-4b88-9875-045464b61720}</MetaDataID>
        public DayColumn CreateDayColumn(string description,DateTime date)
        {            
            DayColumn day_column = new DayColumn(description,date);
            day_column.ColumnModel = this;
            if (ZoomInDaysImage != null)
                day_column.ZoomInImage = ZoomInDaysImage;
            if (ZoomOutDaysImage != null)
                day_column.ZoomOutImage = ZoomOutDaysImage;

            if (this.Table.TableView == TableViewState.DayView)
            {
                this.Table.SetDetailDayMember(date);
            }
            else
                this.Table.SetDetailDayMember(DateTime.MinValue);

            return day_column;
        }

        /// <MetaDataID>{17d797de-c740-4d72-9332-8413f7fd6dae}</MetaDataID>
        public void CreateAndShowWeek(DateTime date,bool onbuild)
        {
            List<Column> next_week = new List<Column>();
            DateTime startweek = this.Table.UpdateWeekMembers(date); 
            
            for (int i = 0; i != 7; i++)
            {
                string datestr = startweek.ToLongDateString();                
                DayColumn day = this.CreateDayColumn(datestr, startweek);
                if (i == 0) day.StartsWeek = true;//Draw views optimization
                if (i == 6) day.EndsWeek = true;//Draw views optimization
                next_week.Add(day);
                startweek = startweek.AddDays(1);
            }
            if(!onbuild)
                this.ClearDayColumns();
            this.ShowWeek(next_week, onbuild);
        }

        /// <MetaDataID>{f978d2c3-c8cb-434b-b0c6-785c669908ac}</MetaDataID>
        public void ClearDayColumns()
        {
            for (int i = 1; i != 8; i++)
                this.Columns.Remove(this.Columns[1]);
        }

        /// <MetaDataID>{68ab94de-bb44-45f1-8441-cfa2080b7858}</MetaDataID>
        public void ShowWeek(List<Column> week, bool onbuild)
        {
            int i = 1;
            foreach (DayColumn daycolumnt in week)
            {
                this.Columns.AddToIndex(i, daycolumnt);
                i++;
            }
            //if (!onbuild)
            //    UpdateColumnsWidth();
        }

        /// <MetaDataID>{f2ee7ea5-dab2-4d8d-96b9-1e481250b5f7}</MetaDataID>
        public TimeColumn CreateTimeColumn(string description,int width)
        {
            TimeColumn column = new TimeColumn(description, width);
            column.ColumnModel = this;
            this.Columns.Add(column);
            return column;
        }

        /// <MetaDataID>{3ab12140-bee4-4440-84e2-173c8eb0a01d}</MetaDataID>
        public void UpdateColumnsWidth()//int total_row_height,int ctrl_height
        {
            int w=0;
            w = (this.Table.DisplayRectangle.Width - System.Convert.ToInt32(this.Columns[0].Width) - (this.Table.BorderWidth)) / (this.Columns.Count - 1);

            //if (total_row_height <= ctrl_height)
            //    w = (this.Table.DisplayRectangle.Width - System.Convert.ToInt32(this.Columns[0].Width)) / (this.Columns.Count - 1);
            //else
            //{
            //    if (!this.Table.VScroll)
            //    {
            //        w = (this.Table.DisplayRectangle.Width -
            //            System.Convert.ToInt32(this.Columns[0].Width) -
            //            SystemInformation.VerticalScrollBarWidth) / (this.Columns.Count - 1);
            //    }
            //    else
            //        w = (this.Table.DisplayRectangle.Width - System.Convert.ToInt32(this.Columns[0].Width)) / (this.Columns.Count - 1);
            //}
            //decimal dwidth = w > Column.DefaultWidth ? w : Column.DefaultWidth;
            for (int i = 1; i != this.Columns.Count; i++)
            {
                Column clm = this.Columns[i];                
                clm.Width = w;
                (clm as DayColumn).UpdateActionsWidth();
            }
        }

        /// <summary>
        /// The default height of a column header
        /// </summary>
        /// <MetaDataID>{13b9d994-9df6-45f5-9125-ebf118d676e7}</MetaDataID>
        public static readonly int DefaultHeaderHeight = 35;

        /// <summary>
        /// The minimum height of a column header
        /// </summary>
        /// <MetaDataID>{f03d053d-6021-480e-bf2a-95333115db17}</MetaDataID>
        public static readonly int MinimumHeaderHeight = 16;

        /// <summary>
        /// The maximum height of a column header
        /// </summary>
        /// <MetaDataID>{74e9985f-504b-4fa9-90aa-6a0b283129e3}</MetaDataID>
        public static readonly int MaximumHeaderHeight = 128;

        /// <summary>
        /// The list of all default CellRenderers used by the Columns in the ColumnModel
        /// </summary>
        /// <MetaDataID>{d2e82b7b-cdb6-4c24-861d-0d018189914b}</MetaDataID>
        private Hashtable cellRenderers;

        /// <summary>
        /// The list of all default CellEditors used by the Columns in the ColumnModel
        /// </summary>
        /// <MetaDataID>{0177e98b-4945-4c5a-9f1d-c908779d5745}</MetaDataID>
        private Hashtable cellEditors;
        

       
        #endregion

        #region protected internal virtual void OnColumnRemoved(ColumnModelEventArgs e)
        /// <summary>
        /// Raises the ColumnRemoved event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{D82E3512-AA3E-4F9E-8F38-BEE8B32890F4}</MetaDataID>
        protected internal virtual void OnColumnRemoved(ColumnModelEventArgs e)
        {
            if (e.Column != null && e.Column.ColumnModel == this)
            {
                e.Column.ColumnModel = null;
            }

            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnColumnRemoved(e);
                }

                if (ColumnRemoved != null)
                {
                    ColumnRemoved(this, e);
                }
            }
        } 
        #endregion

        #region protected internal virtual void OnColumnAdded(ColumnModelEventArgs e)
        /// <summary>
        /// Raises the ColumnAdded event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{5F9A301D-509D-454B-90F8-CCC9944DE043}</MetaDataID>
        protected internal virtual void OnColumnAdded(ColumnModelEventArgs e)
        {
            //e.Column.ColumnModel = this;

            if (!this.ContainsCellRenderer(e.Column.GetDefaultRendererName()))
            {
                this.SetCellRenderer(e.Column.GetDefaultRendererName(), e.Column.CreateDefaultRenderer());
            }

            //if (!this.ContainsCellEditor(e.Column.GetDefaultEditorName()))
            //{
            //    this.SetCellEditor(e.Column.GetDefaultEditorName(), e.Column.CreateDefaultEditor());
            //}

            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnColumnAdded(e);
                }

                if (ColumnAdded != null)
                {
                    ColumnAdded(this, e);
                }
            }
        } 
        #endregion

        #region internal void OnColumnPropertyChanged(ColumnEventArgs e)
        /// <summary>
        /// Raises the ColumnPropertyChanged event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{2CD52FB9-6657-4B56-B634-ED8A7369E400}</MetaDataID>
        internal void OnColumnPropertyChanged(ColumnEventArgs e)
        {
            if (e.EventType == ColumnEventType.WidthChanged || e.EventType == ColumnEventType.VisibleChanged)
            {
                this.Columns.RecalcWidthCache();
            }

            if (this.CanRaiseEvents)
            {
                if (this.Table != null)
                {
                    this.Table.OnColumnPropertyChanged(e);
                }
            }
        } 
        #endregion

        /// <MetaDataID>{1b84a091-5566-49b3-837f-a256af9c4d44}</MetaDataID>
        internal void ClearViewsFromColumns()
        {
            for (int i = 1; i != this.Columns.Count; i++)
            {
                DayColumn clm = this.Columns[i] as DayColumn;
                clm.ClearDayViews();
                clm.ActionViews.Clear();
            }
        }
    }
}
