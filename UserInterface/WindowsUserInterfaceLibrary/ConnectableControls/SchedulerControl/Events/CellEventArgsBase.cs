using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <MetaDataID>{6680f2cc-8f1f-48e0-95ca-2f40e8e3e92b}</MetaDataID>
    public class CellEventArgsBase : EventArgs
    {
        #region Class Data

        /// <summary>
        /// The Cell that Raised the event
        /// </summary>
        private Cell source;

        /// <summary>
        /// The Column index of the Cell
        /// </summary>
        private int column;

        /// <summary>
        /// The Row index of the Cell
        /// </summary>
        private int row;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellEventArgs class with 
        /// the specified Cell source and event type
        /// </summary>
        /// <param name="source">The Cell that Raised the event</param>
        /// <MetaDataID>{9FCFAF40-5F2A-4402-B21E-BE9C0ACD06CC}</MetaDataID>
        public CellEventArgsBase(Cell source)
            : this(source, -1, -1)
        {

        }


        /// <summary>
        /// Initializes a new instance of the CellEventArgs class with 
        /// the specified Cell source, column index and row index
        /// </summary>
        /// <param name="source">The Cell that Raised the event</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <MetaDataID>{7416BAFF-2F93-432D-BDF5-42CD2CB0CDD1}</MetaDataID>
        public CellEventArgsBase(Cell source, int column, int row)
            : base()
        {
            this.source = source;
            this.column = column;
            this.row = row;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the Cell that Raised the event
        /// </summary>
        public Cell Cell
        {
            get
            {
                return this.source;
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
        /// </summary>
        /// <param name="column"></param>
        /// <MetaDataID>{1342DD48-0F95-4C80-B631-837623F10BA7}</MetaDataID>
        internal void SetColumn(int column)
        {
            this.column = column;
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
        /// </summary>
        /// <param name="row"></param>
        /// <MetaDataID>{52D6A830-95B9-4F0D-BE09-3E4FA7EE7AA1}</MetaDataID>
        internal void SetRow(int row)
        {
            this.row = row;
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
