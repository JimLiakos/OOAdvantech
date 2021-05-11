using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <summary>
    /// Represents the method that will handle the CellKeyDown and CellKeyUp 
    /// events of a Table
    /// </summary>
    public delegate void CellKeyEventHandler(object sender, CellKeyEventArgs e);

    /// <MetaDataID>{ec45ff61-f2fc-46c0-aa71-2bd5d42dbc6c}</MetaDataID>
    public class CellKeyEventArgs : System.Windows.Forms.KeyEventArgs
    {
        #region Class Data

        /// <summary>
        /// The Cell that Raised the event
        /// </summary>
        private Cell cell;

        /// <summary>
        /// The Table the Cell belongs to
        /// </summary>
        private SchedulerListView _ListView;

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
        /// Initializes a new instance of the CellKeyEventArgs class with 
        /// the specified source Cell, table, row index, column index, cell 
        /// bounds and KeyEventArgs
        /// </summary>
        /// <param name="cell">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <param name="kea"></param>
        /// <MetaDataID>{D6060B28-9A27-42FD-9922-C5F5CB91C004}</MetaDataID>
        public CellKeyEventArgs(Cell cell, SchedulerListView list_view, int row, int column, Rectangle cellRect, System.Windows.Forms.KeyEventArgs kea)
            : base(kea.KeyData)
        {
            this.cell = cell;
            this._ListView = list_view;
            this.row = row;
            this.column = column;
            this.cellRect = cellRect;
        }


        /// <summary>
        /// Initializes a new instance of the CellKeyEventArgs class with 
        /// the specified source Cell, table, row index, column index and 
        /// cell bounds
        /// </summary>
        /// <param name="cell">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="cellPos"></param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <param name="kea"></param>
        /// <MetaDataID>{9E578A48-3667-46B4-A0DA-B1FB813E16D0}</MetaDataID>
        public CellKeyEventArgs(Cell cell, SchedulerListView list_view, CellPos cellPos, Rectangle cellRect, System.Windows.Forms.KeyEventArgs kea)
            : base(kea.KeyData)
        {
            this.cell = cell;
            this._ListView = list_view;
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
        public SchedulerListView ListView
        {
            get
            {
                return this._ListView;
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
