using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <summary>
    /// Represents the method that will handle the CellMouseEnter, CellMouseLeave, 
    /// CellMouseDown, CellMouseUp, CellMouseMove and CellMouseHover events of a Table
    /// </summary>
    public delegate void CellMouseEventHandler(object sender, CellMouseEventArgs e);


    /// <MetaDataID>{c4c145c2-49a6-42f9-9b83-be8677a6a3e1}</MetaDataID>
    public class CellMouseEventArgs : System.Windows.Forms.MouseEventArgs
    {
        #region Class Data

        /// <summary>
        /// The Cell that raised the event
        /// </summary>
        private Cell cell;

        /// <summary>
        /// The Table the Cell belongs to
        /// </summary>
        private SchedulerListView table;

        /// <summary>
        /// The Row index of the Cell
        /// </summary>
        private int row;

        /// <summary>
        /// The Column index of the Cell
        /// </summary>
        private int column;

        /// <summary>
        /// The Cells bounding rectangle
        /// </summary>
        private Rectangle cellRect;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellMouseEventArgs class with 
        /// the specified source Cell, table, row index, column index and 
        /// cell bounds
        /// </summary>
        /// <param name="cell">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="cellPos"></param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <MetaDataID>{74D08576-DD56-4655-BCE4-C5BC64A6114F}</MetaDataID>
        public CellMouseEventArgs(Cell cell, SchedulerListView table, CellPos cellPos, Rectangle cellRect)
            : base(System.Windows.Forms.MouseButtons.None, 0, -1, -1, 0)
        {
            this.cell = cell;
            this.table = table;
            this.row = cellPos.Row;
            this.column = cellPos.Column;
            this.cellRect = cellRect;
        }


        /// <summary>
        /// Initializes a new instance of the CellMouseEventArgs class with 
        /// the specified source Cell, table, row index, column index and 
        /// cell bounds
        /// </summary>
        /// <param name="cell">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <MetaDataID>{64313A19-3396-4130-BF6D-348B73360C38}</MetaDataID>
        public CellMouseEventArgs(Cell cell, SchedulerListView table, int row, int column, Rectangle cellRect)
            : base(System.Windows.Forms.MouseButtons.None, 0, -1, -1, 0)
        {
            this.cell = cell;
            this.table = table;
            this.row = row;
            this.column = column;
            this.cellRect = cellRect;
        }


        /// <summary>
        /// Initializes a new instance of the CellMouseEventArgs class with 
        /// the specified source Cell, table, row index, column index, cell 
        /// bounds and MouseEventArgs
        /// </summary>
        /// <param name="cell">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <param name="mea">The MouseEventArgs that contains data about the 
        /// mouse event</param>
        /// <MetaDataID>{50CEB830-5F3A-40D6-A6A7-0FF53E8B3D7E}</MetaDataID>
        public CellMouseEventArgs(Cell cell, SchedulerListView table, int row, int column, Rectangle cellRect, System.Windows.Forms.MouseEventArgs mea)
            : base(mea.Button, mea.Clicks, mea.X, mea.Y, mea.Delta)
        {
            this.cell = cell;
            this.table = table;
            this.row = row;
            this.column = column;
            this.cellRect = cellRect;
        }


        /// <summary>
        /// Initializes a new instance of the CellMouseEventArgs class with 
        /// the specified source Cell, table, row index, column index and 
        /// cell bounds
        /// </summary>
        /// <param name="cell">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="cellPos"></param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <param name="mea"></param>
        /// <MetaDataID>{A2546727-B365-4B78-A5C5-A4CC5A5E9B36}</MetaDataID>
        public CellMouseEventArgs(Cell cell, SchedulerListView table, CellPos cellPos, Rectangle cellRect, System.Windows.Forms.MouseEventArgs mea)
            : base(mea.Button, mea.Clicks, mea.X, mea.Y, mea.Delta)
        {
            this.cell = cell;
            this.table = table;
            this.row = cellPos.Row;
            this.column = cellPos.Column;
            this.cellRect = cellRect;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Cell that Raised the event
        /// </summary>
        public Cell Cell
        {
            get
            {
                return this.cell;
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
        /// Gets the Row index of the Cell
        /// </summary>
        public int Row
        {
            get
            {
                return this.row;
            }
        }


        /// <summary>
        /// Gets the Column index of the Cell
        /// </summary>
        public int Column
        {
            get
            {
                return this.column;
            }
        }


        /// <summary>
        /// Gets the Cells bounding rectangle
        /// </summary>
        public Rectangle CellRect
        {
            get
            {
                return this.cellRect;
            }
        }


        /// <summary>
        /// Gets the position of the Cell
        /// </summary>
        public CellPos CellPos
        {
            get
            {
                return new CellPos(this.Row, this.Column);
            }
        }

        #endregion
    }
}
