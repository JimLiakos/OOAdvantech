using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <summary>
    /// Represents the method that will handle the HeaderMouseEnter, HeaderMouseLeave, 
    /// HeaderMouseDown, HeaderMouseUp, HeaderMouseMove, HeaderClick and HeaderDoubleClick 
    /// events of a Table
    /// </summary>
    public delegate void HeaderMouseEventHandler(object sender, HeaderMouseEventArgs e);

    /// <MetaDataID>{5e8f5f24-3a77-42f5-80f9-36b9a39d1916}</MetaDataID>
    public class HeaderMouseEventArgs : System.Windows.Forms.MouseEventArgs
    {
        #region Class Data

        /// <summary>
        /// The Column that raised the event
        /// </summary>
        private Column column;

        /// <summary>
        /// The Table the Column belongs to
        /// </summary>
        private SchedulerListView table;

        /// <summary>
        /// The index of the Column
        /// </summary>
        private int index;

        /// <summary>
        /// The column header's bounding rectangle
        /// </summary>
        private Rectangle headerRect;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the HeaderMouseEventArgs class with 
        /// the specified source Column, Table, column index and column header bounds
        /// </summary>
        /// <param name="column">The Column that Raised the event</param>
        /// <param name="table">The Table the Column belongs to</param>
        /// <param name="index">The index of the Column</param>
        /// <param name="headerRect">The column header's bounding rectangle</param>
        /// <MetaDataID>{56456F26-1460-4256-B584-ACFA4677AF30}</MetaDataID>
        public HeaderMouseEventArgs(Column column, SchedulerListView table, int index, Rectangle headerRect)
            : base(System.Windows.Forms.MouseButtons.None, 0, -1, -1, 0)
        {
            this.column = column;
            this.table = table;
            this.index = index;
            this.headerRect = headerRect;
        }


        /// <summary>
        /// Initializes a new instance of the HeaderMouseEventArgs class with 
        /// the specified source Column, Table, column index, column header bounds 
        /// and MouseEventArgs
        /// </summary>
        /// <param name="column">The Column that Raised the event</param>
        /// <param name="table">The Table the Column belongs to</param>
        /// <param name="index">The index of the Column</param>
        /// <param name="headerRect">The column header's bounding rectangle</param>
        /// <param name="mea">The MouseEventArgs that contains data about the 
        /// mouse event</param>
        /// <MetaDataID>{68B2F94F-A4C0-4B1A-B94D-356737417D0D}</MetaDataID>
        public HeaderMouseEventArgs(Column column, SchedulerListView table, int index, Rectangle headerRect, System.Windows.Forms.MouseEventArgs mea)
            : base(mea.Button, mea.Clicks, mea.X, mea.Y, mea.Delta)
        {
            this.column = column;
            this.table = table;
            this.index = index;
            this.headerRect = headerRect;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Column that Raised the event
        /// </summary>
        public Column Column
        {
            get
            {
                return this.column;
            }
        }


        /// <summary>
        /// Gets the Table the Cell belongs to
        /// </summary>
        public SchedulerListView Table
        {
            get
            {
                return this.table;
            }
        }


        /// <summary>
        /// Gets the index of the Column
        /// </summary>
        public int Index
        {
            get
            {
                return this.index;
            }
        }


        /// <summary>
        /// Gets the column header's bounding rectangle
        /// </summary>
        public Rectangle HeaderRect
        {
            get
            {
                return this.headerRect;
            }
        }

        #endregion
    }
}
