using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <summary>
    /// Represents the method that will handle the CellGotFocus and CellLostFocus 
    /// events of a Table
    /// </summary>
    public delegate void CellFocusEventHandler(object sender, CellFocusEventArgs e);

    /// <MetaDataID>{afd98605-e1d4-4e7a-8794-4de39a7c60b4}</MetaDataID>
    public class CellFocusEventArgs : CellEventArgsBase
    {
        #region Class Data

        /// <summary>
        /// The Table the Cell belongs to
        /// </summary>
        private SchedulerListView table;

        /// <summary>
        /// The Cells bounding rectangle
        /// </summary>
        private Rectangle cellRect;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellFocusEventArgs class with 
        /// the specified source Cell, table, row index, column index and 
        /// cell bounds
        /// </summary>
        /// <param name="source">The Cell that Raised the event</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="cellRect">The Cell's bounding rectangle</param>
        /// <MetaDataID>{5443D74D-02A9-416C-BE63-DE4B365BB46E}</MetaDataID>
        public CellFocusEventArgs(Cell source, SchedulerListView table, int row, int column, Rectangle cellRect)
            : base(source, column, row)
        {
            this.table = table;
            this.cellRect = cellRect;
        }

        #endregion

        #region Properties

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
        /// Gets the Cell's bounding rectangle
        /// </summary>
        public Rectangle CellRect
        {
            get
            {
                return this.cellRect;
            }
        }

        #endregion
    }
}
