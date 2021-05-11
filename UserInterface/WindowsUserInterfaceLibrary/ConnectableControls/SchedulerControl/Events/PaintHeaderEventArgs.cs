using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <summary>
    /// Represents the method that will handle the PaintHeader events of a Table
    /// </summary>
    public delegate void PaintHeaderEventHandler(object sender, PaintHeaderEventArgs e);

    // <summary>
    /// Provides data for the PaintHeader event
    /// </summary>
    public class PaintHeaderEventArgs : System.Windows.Forms.PaintEventArgs
    {
        #region Class Data

        /// <summary>
        /// The Column to be painted
        /// </summary>
        /// <MetaDataID>{736f7e2e-b1a5-4cbc-a049-69b2654c2bfd}</MetaDataID>
        private Column column;

        /// <summary>
        /// The Table the Column's ColumnModel belongs to
        /// </summary>
        /// <MetaDataID>{ab7f45b4-66c0-4976-b06a-fa68674bf5ab}</MetaDataID>
        private SchedulerListView table;

        /// <summary>
        /// The index of the Column in the Table's ColumnModel
        /// </summary>
        /// <MetaDataID>{ca7db60f-d976-4d2c-bb0f-3af18559ad84}</MetaDataID>
        private int columnIndex;

        /// <summary>
        /// The style of the Column header
        /// </summary>
        /// <MetaDataID>{7585d2ba-d3ec-41da-b7f7-c3d3351792b6}</MetaDataID>
        private System.Windows.Forms.ColumnHeaderStyle headerStyle;



        /// <summary>
        /// Indicates whether the user has done the paining for us
        /// </summary>
        /// <MetaDataID>{8febdd3e-84af-4062-8c18-75606ebd985e}</MetaDataID>
        private bool handled;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PaintHeaderEventArgs class with 
        /// the specified graphics and clipping rectangle
        /// </summary>
        /// <param name="g">The Graphics used to paint the Column header</param>
        /// <param name="headerRect">The Rectangle that represents the rectangle 
        /// in which to paint</param>
        /// <MetaDataID>{4E87E5AC-F43B-44FC-BABB-A8DA09F39A39}</MetaDataID>
        public PaintHeaderEventArgs(Graphics g, Rectangle headerRect)
            : this(g, null, null, -1, System.Windows.Forms.ColumnHeaderStyle.None, headerRect)
        {

        }


        /// <summary>
        /// Initializes a new instance of the PaintHeaderEventArgs class with 
        /// the specified graphics, column, table, column index, header style 
        /// and clipping rectangle
        /// </summary>
        /// <param name="g">The Graphics used to paint the Column header</param>
        /// <param name="column">The Column to be painted</param>
        /// <param name="table">The Table the Column's ColumnModel belongs to</param>
        /// <param name="columnIndex">The index of the Column in the Table's ColumnModel</param>
        /// <param name="headerStyle">The style of the Column's header</param>
        /// <param name="headerRect">The Rectangle that represents the rectangle 
        /// in which to paint</param>
        /// <MetaDataID>{F31D6340-8EB5-44ED-A0A8-A820937081DA}</MetaDataID>
        public PaintHeaderEventArgs(Graphics g, Column column, SchedulerListView table, int columnIndex, System.Windows.Forms.ColumnHeaderStyle headerStyle, Rectangle headerRect)
            : base(g, headerRect)
        {
            this.column = column;
            this.table = table;
            this.columnIndex = columnIndex;
            this.column = column;
            this.headerStyle = headerStyle;
            this._HeaderRect = headerRect;
            this.handled = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Column to be painted
        /// </summary>
        /// <MetaDataID>{1aa6a3be-dfbd-44a6-8ccb-34f35c21d2e5}</MetaDataID>
        public Column Column
        {
            get
            {
                return this.column;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="column"></param>
        /// <MetaDataID>{FED73FD2-531A-4102-8F55-B9FE81F91B3A}</MetaDataID>
        internal void SetColumn(Column column)
        {
            this.column = column;
        }


        /// <summary>
        /// Gets the Table the Column's ColumnModel belongs to
        /// </summary>
        /// <MetaDataID>{da0400c8-cab7-40e9-b189-5855d0dea6b5}</MetaDataID>
        public SchedulerListView Table
        {
            get
            {
                return this.table;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="table"></param>
        /// <MetaDataID>{AA77D17B-6008-4462-9466-47FD7084D642}</MetaDataID>
        internal void SetTable(SchedulerListView table)
        {
            this.table = table;
        }


        /// <summary>
        /// Gets the index of the Column in the Table's ColumnModel
        /// </summary>
        /// <MetaDataID>{99bebda4-4d7b-4b61-861b-5da372e9d8b1}</MetaDataID>
        public int ColumnIndex
        {
            get
            {
                return this.columnIndex;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <MetaDataID>{ED6B949E-EE00-4D2D-9A99-E49C6FA1666A}</MetaDataID>
        internal void SetColumnIndex(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }


        /// <summary>
        /// Gets the style of the Column's header
        /// </summary>
        /// <MetaDataID>{c5ca612c-ff05-4e7b-a2dc-39841e2a925f}</MetaDataID>
        public System.Windows.Forms.ColumnHeaderStyle HeaderStyle
        {
            get
            {
                return this.headerStyle;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="headerStyle"></param>
        /// <MetaDataID>{468E9F9C-3CCB-444B-81D6-6EC10E5D675B}</MetaDataID>
        internal void SetHeaderStyle(System.Windows.Forms.ColumnHeaderStyle headerStyle)
        {
            this.headerStyle = headerStyle;
        }


        /// <MetaDataID>{5566f9bd-94b2-45c2-b038-e630e159879e}</MetaDataID>
        private Rectangle _HeaderRect;
        /// <summary>
        /// Gets the column header's bounding rectangle
        /// </summary>
        /// <MetaDataID>{d8958de4-371b-4080-b7ad-9bb39a62438a}</MetaDataID>
        public Rectangle HeaderRect
        {
            get
            {
                return this._HeaderRect;
            }
        }


        /// <summary>
        /// Gets the Day Rectangle
        /// </summary>
        /// <MetaDataID>{c88748c8-1da7-4e66-a678-9da3d2ffe146}</MetaDataID>
        public Rectangle DayRect
        {
            get
            {
                return _DayRect;
            }
        }

        /// <MetaDataID>{1b5a09ed-9fdb-4a15-89f9-0e54c528a0f1}</MetaDataID>
        private Rectangle _DayRect;
        /// <summary>
        /// sets the Day Rectangle
        /// </summary>
        /// <param name="day_rect"></param>
        /// <MetaDataID>{4dbc2481-f558-4446-855c-55335b66c7b5}</MetaDataID>
        internal void SetDayRect(Rectangle day_rect)
        {
            _DayRect = day_rect;
        }

        
        /// <summary>
        /// </summary>
        /// <param name="headerRect"></param>
        /// <MetaDataID>{CDF4A496-D77F-42A2-B9AD-65E39C8D71D8}</MetaDataID>
        internal void SetHeaderRect(Rectangle headerRect)
        {
            this._HeaderRect = headerRect;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the BeforePaintHeader 
        /// event was handled
        /// </summary>
        /// <MetaDataID>{52bb94d3-0e12-4ca0-8bb6-d96321c74558}</MetaDataID>
        public bool Handled
        {
            get
            {
                return this.handled;
            }

            set
            {
                this.handled = value;
            }
        }

        #endregion
    }
}
