using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.SchedulerControl.Events
{    
    /// <summary>
    /// Represents the methods that will handle the ColumnInserted, ColumnRemoved 
    /// and HeaderHeightChanged event of a ColumnModel
    /// </summary>
    public delegate void ColumnModelEventHandler(object sender, ColumnModelEventArgs e);

    /// <MetaDataID>{68f92453-4e3d-4775-ac91-b30ff1d3218d}</MetaDataID>
    public class ColumnModelEventArgs
    {
        #region Class Data

        /// <summary>
        /// The ColumnModel that Raised the event
        /// </summary>
        private DayTimeColumnModel source;

        /// <summary>
        /// The affected Column
        /// </summary>
        private Column column;

        /// <summary>
        /// The start index of the affected Column(s)
        /// </summary>
        private int fromIndex;

        /// <summary>
        /// The end index of the affected Column(s)
        /// </summary>
        private int toIndex;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ColumnModelEventArgs class with 
        /// the specified ColumnModel source, start index, end index and affected Column
        /// </summary>
        /// <param name="source">The ColumnModel that originated the event</param>
        /// <param name="column">The affected Column</param>
        /// <param name="fromIndex">The start index of the affected Column(s)</param>
        /// <param name="toIndex">The end index of the affected Column(s)</param>
        /// <MetaDataID>{BEF4C831-5F2E-4CB7-9A25-F3D7BCAAC75E}</MetaDataID>
        public ColumnModelEventArgs(DayTimeColumnModel source, Column column, int fromIndex, int toIndex)
            : base()
        {
            this.source = source;
            this.column = column;
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ColumnModel that Raised the event
        /// </summary>
        public DayTimeColumnModel ColumnModel
        {
            get
            {
                return this.source;
            }
        }


        /// <summary>
        /// Gets the affected Column
        /// </summary>
        public Column Column
        {
            get
            {
                return this.column;
            }
        }


        /// <summary>
        /// Gets the start index of the affected Column(s)
        /// </summary>
        public int FromIndex
        {
            get
            {
                return this.fromIndex;
            }
        }


        /// <summary>
        /// Gets the end index of the affected Column(s)
        /// </summary>
        public int ToIndex
        {
            get
            {
                return this.toIndex;
            }
        }

        #endregion
    }
}
