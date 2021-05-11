using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ConnectableControls.SchedulerControl.TableModelDesign;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.RowDesign
{
    /// <summary>
    /// Represents a collection of Row objects
    /// </summary>
    /// <MetaDataID>{d451f7e5-2ea6-47fc-84c1-145b411c236b}</MetaDataID>
    public class RowCollection : CollectionBase
    {
        #region Class Data

        /// <summary>
        /// The TableModel that owns the RowCollection
        /// </summary>
        private TableModel owner;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the RowCollection class 
        /// that belongs to the specified TableModel
        /// </summary>
        /// <param name="owner">A TableModel representing the tableModel that owns 
        /// the RowCollection</param>
        /// <MetaDataID>{B7E87A6B-4C29-4392-8F24-6CD8B5BB4DA9}</MetaDataID>
        public RowCollection(TableModel owner)
            : base()
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// Initializes a new instance of the RowCollection class 
        /// that belongs to the specified TableModel
        /// </summary>
        /// <param name="owner">A TableModel representing the tableModel that owns 
        /// the RowCollection</param>
        /// <MetaDataID>{B7E87A6B-4C29-4392-8F24-6CD8B5BB4DA9}</MetaDataID>
        public RowCollection(TableModel owner,Row parent_row)
            : base()
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            if (parent_row == null)
                throw new ArgumentNullException("parent_row");

            this.owner = owner;
            _ParentRow = parent_row;
        }

        #endregion

        private Row _ParentRow;
        /// <summary>
        /// Return the parent time row
        /// </summary>
        public Row ParentRow
        {
            get
            {
                return _ParentRow;
            }
        }

        #region Methods

        /// <summary>
        /// Adds the specified Row to the end of the collection
        /// </summary>
        /// <param name="row">The Row to add</param>
        /// <MetaDataID>{D130C35B-02C7-4221-93FA-10C0AEE51B67}</MetaDataID>
        public int Add(Row row)
        {
            if (row == null)
            {
                throw new System.ArgumentNullException("Row is null");
            }

            int index = this.List.Add(row);

            this.OnRowAdded(new TableModelEventArgs(this.owner, row, index, index));

            return index;
        }


        /// <summary>
        /// Adds an array of Row objects to the collection
        /// </summary>
        /// <param name="TotalRows">An array of Row objects to add 
        /// to the collection</param>
        /// <MetaDataID>{257BB941-03EB-43A2-A7C8-AA28B186A3F3}</MetaDataID>
        public void AddRange(Row[] Rows)
        {
            if (Rows == null)
            {
                throw new System.ArgumentNullException("Row[] is null");
            }

            for (int i = 0; i < Rows.Length; i++)
            {
                this.Add(Rows[i]);
            }
        }


        /// <summary>
        /// Removes the specified Row from the model
        /// </summary>
        /// <param name="row">The Row to remove</param>
        /// <MetaDataID>{2C88E95F-166C-4C2D-91FF-06EAEE3D0811}</MetaDataID>
        public void Remove(Row row)
        {
            int rowIndex = this.IndexOf(row);

            if (rowIndex != -1)
            {
                this.RemoveAt(rowIndex);
            }
        }


        /// <summary>
        /// Removes an array of Row objects from the collection
        /// </summary>
        /// <param name="TotalRows">An array of Row objects to remove 
        /// from the collection</param>
        /// <MetaDataID>{5B7103C0-DA64-4CCF-A018-0A093B7EE3FF}</MetaDataID>
        public void RemoveRange(Row[] TotalRows)
        {
            if (TotalRows == null)
            {
                throw new System.ArgumentNullException("Row[] is null");
            }

            for (int i = 0; i < TotalRows.Length; i++)
            {
                this.Remove(TotalRows[i]);
            }
        }


        /// <summary>
        /// Removes the Row at the specified index from the collection
        /// </summary>
        /// <param name="index">The index of the Row to remove</param>
        /// <MetaDataID>{BC22AE21-7140-45EC-AEC8-B58C3AC2FA25}</MetaDataID>
        public new void RemoveAt(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                Row row = this[index];

                this.List.RemoveAt(index);

                this.OnRowRemoved(new TableModelEventArgs(this.owner, row, index, index));
            }
        }


        /// <summary>
        /// Removes all TotalRows from the collection
        /// </summary>
        /// <MetaDataID>{ABC37290-FC63-4C05-BFF0-1ADF1ECE729D}</MetaDataID>
        public new void Clear()
        {
            if (this.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.Count; i++)
            {
                this[i].TableModel = null;
            }

            base.Clear();
            this.InnerList.Capacity = 0;

            this.owner.OnRowRemoved(new TableModelEventArgs(this.owner, null, -1, -1));
        }


        /// <summary>
        /// Inserts a Row into the collection at the specified index
        /// </summary>
        /// <param name="index">The zero-based index at which the Row 
        /// should be inserted</param>
        /// <param name="row">The Row to insert</param>
        /// <MetaDataID>{20605F68-0849-4BC1-9CF2-DABEB0621ACC}</MetaDataID>
        public void Insert(int index, Row row)
        {
            if (row == null)
            {
                return;
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index >= this.Count)
            {
                this.Add(row);
            }
            else
            {
                base.List.Insert(index, row);

                this.owner.OnRowAdded(new TableModelEventArgs(this.owner, row, index, index));
            }
        }


        /// <summary>
        /// Inserts an array of TotalRows into the collection at the specified 
        /// index
        /// </summary>
        /// <param name="index">The zero-based index at which the TotalRows 
        /// should be inserted</param>
        /// <param name="TotalRows">The array of TotalRows to be inserted into 
        /// the collection</param>
        /// <MetaDataID>{F92B43E7-C402-44E4-AD84-4B243228F3A0}</MetaDataID>
        public void InsertRange(int index, Row[] TotalRows)
        {
            if (TotalRows == null)
            {
                throw new System.ArgumentNullException("Row[] is null");
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index >= this.Count)
            {
                this.AddRange(TotalRows);
            }
            else
            {
                for (int i = TotalRows.Length - 1; i >= 0; i--)
                {
                    this.Insert(index, TotalRows[i]);
                }
            }
        }


        /// <summary>
        /// Returns the index of the specified Row in the model
        /// </summary>
        /// <param name="row">The Row to look for</param>
        /// <returns>The index of the specified Row in the model</returns>
        /// <MetaDataID>{4BFCC5F4-73CA-4C4B-A530-40D14CB38FA7}</MetaDataID>
        public int IndexOf(Row row)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] == row)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Row at the specified index
        /// </summary>
        public Row this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    return null;
                }

                return this.List[index] as Row;
            }
        }


        /// <summary>
        /// Replaces the Row at the specified index to the specified Row
        /// </summary>
        /// <param name="index">The index of the Row to be replaced</param>
        /// <param name="row">The Row to be placed at the specified index</param>
        /// <MetaDataID>{7985735D-9E91-44BC-A3AE-33738D08386B}</MetaDataID>
        internal void SetRow(int index, Row row)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            if (row == null)
            {
                throw new ArgumentNullException("row cannot be null");
            }

            this.List[index] = row;

            row.InternalIndex = index;
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the RowAdded event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{0DEA80EC-73A5-4D33-8A36-C3500D64164F}</MetaDataID>
        protected virtual void OnRowAdded(TableModelEventArgs e)
        {
            this.owner.OnRowAdded(e);
        }


        /// <summary>
        /// Raises the RowRemoved event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{7C48171E-94EF-443A-9A32-DF5F02D70ACB}</MetaDataID>
        protected virtual void OnRowRemoved(TableModelEventArgs e)
        {
            this.owner.OnRowRemoved(e);
        }

        #endregion
    }
}
