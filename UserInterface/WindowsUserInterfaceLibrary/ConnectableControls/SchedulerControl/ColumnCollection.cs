using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{a7660493-2d7c-497f-9506-8eaa50e4bd7b}</MetaDataID>
    public class ColumnCollection : CollectionBase
    {
        #region Class Data

        /// <summary>
        /// The ColumnModel that owns the CollumnCollection
        /// </summary>
        /// <MetaDataID>{e8c36c1a-4cd7-4914-82af-83e582b83f5c}</MetaDataID>
        private DayTimeColumnModel _DayTimeColumnModel;
        /// <summary>
        /// A local cache of the number of visible columns
        /// </summary>
        /// <MetaDataID>{ea1264f0-0f63-4a5c-9834-2e957d1ae6fb}</MetaDataID>
        private int visibleColumnCount;

        /// <summary>
        /// A local cache of the last visible column in the collection
        /// </summary>
        /// <MetaDataID>{f4898905-e621-49c4-82aa-e0a3f6ea28d2}</MetaDataID>
        private int lastVisibleColumn;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ColumnModel.ColumnCollection class 
        /// that belongs to the specified ColumnModel
        /// </summary>
        /// <param name="owner">A ColumnModel representing the columnModel that owns 
        /// the Column collection</param>
        /// <MetaDataID>{B875A73D-2E47-4F96-96A7-7632FC58B8D6}</MetaDataID>
        public ColumnCollection(DayTimeColumnModel owner_model)
            : base()
        {
            if (owner_model == null)
            {
                throw new ArgumentNullException("owner_model");
            }

            this._DayTimeColumnModel = owner_model;
            this._TotalColumnWidth = 0;
            this._VisibleColumnsWidth= 0;
            this.visibleColumnCount = 0;
            this.lastVisibleColumn = -1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified Column to the end of the collection
        /// </summary>
        /// <param name="column">The Column to add</param>
        /// <MetaDataID>{2FDF98D8-4E9B-4E52-94DB-FFD5E115A6DB}</MetaDataID>
        public int Add(Column column)
        {
            if (column == null)
            {
                throw new System.ArgumentNullException("Column is null");
            }

            int index = this.List.Add(column);

            this.RecalcWidthCache();

            this.OnColumnAdded(new ColumnModelEventArgs(this._DayTimeColumnModel, column, index, index));

            return index;
        }

        /// <MetaDataID>{28e9146f-d5b1-4c4f-b65c-3e06b253c7ee}</MetaDataID>
        public int AddToIndex(int index,Column column)
        {
            if (column == null)
            {
                throw new System.ArgumentNullException("Column is null");
            }

            this.List.Insert(index,column);

            this.RecalcWidthCache();

            this.OnColumnAdded(new ColumnModelEventArgs(this._DayTimeColumnModel, column, index, index));

            return index;
        }


        /// <summary>
        /// Adds an array of Column objects to the collection
        /// </summary>
        /// <param name="columns">An array of Column objects to add 
        /// to the collection</param>
        /// <MetaDataID>{77DB3F00-3F09-48EE-B0F0-AB1DF4EA1CE5}</MetaDataID>
        public void AddRange(Column[] columns)
        {
            if (columns == null)
            {
                throw new System.ArgumentNullException("Column[] is null");
            }

            for (int i = 0; i < columns.Length; i++)
            {
                this.Add(columns[i]);
            }
        }


        /// <summary>
        /// Removes the specified Column from the model
        /// </summary>
        /// <param name="column">The Column to remove</param>
        /// <MetaDataID>{87A83E2F-7F6D-453E-B23C-5CD2F5B4CA22}</MetaDataID>
        public void Remove(Column column)
        {
            int columnIndex = this.IndexOf(column);

            if (columnIndex != -1)
            {
                this.RemoveAt(columnIndex);
            }
        }


        /// <summary>
        /// Removes an array of Column objects from the collection
        /// </summary>
        /// <param name="columns">An array of Column objects to remove 
        /// from the collection</param>
        /// <MetaDataID>{F6C04991-5A88-4AA3-9A5F-5FAA57C6010A}</MetaDataID>
        public void RemoveRange(Column[] columns)
        {
            if (columns == null)
            {
                throw new System.ArgumentNullException("Column[] is null");
            }

            for (int i = 0; i < columns.Length; i++)
            {
                this.Remove(columns[i]);
            }
        }


        /// <summary>
        /// Removes the Column at the specified index from the collection
        /// </summary>
        /// <param name="index">The index of the Column to remove</param>
        /// <MetaDataID>{90ABDF0A-6227-43F7-A4BA-C58B102157EC}</MetaDataID>
        public new void RemoveAt(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                Column column = this[index];

                this.List.RemoveAt(index);

                this.RecalcWidthCache();

                this.OnColumnRemoved(new ColumnModelEventArgs(this._DayTimeColumnModel, column, index, index));
            }
        }


        /// <summary>
        /// Removes all Columns from the collection
        /// </summary>
        /// <MetaDataID>{117DC2E2-1B26-4469-ABEC-0FC8D9907A30}</MetaDataID>
        public new void Clear()
        {
            if (this.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.Count; i++)
            {
                this[i].ColumnModel = null;
            }

            base.Clear();
            this.InnerList.Capacity = 0;

            this.RecalcWidthCache();

            this.OnColumnRemoved(new ColumnModelEventArgs(this._DayTimeColumnModel, null, -1, -1));
        }


        /// <summary>
        /// Returns the index of the specified Column in the model
        /// </summary>
        /// <param name="column">The Column to look for</param>
        /// <returns>The index of the specified Column in the model</returns>
        /// <MetaDataID>{A623B7EB-F991-4910-A59A-B14F8A54BFC2}</MetaDataID>
        public int IndexOf(Column column)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] == column)
                {
                    return i;
                }
            }

            return -1;
        }


        /// <summary>
        /// Recalculates the total combined width of all columns
        /// </summary>
        /// <MetaDataID>{85D0BBA1-C734-43C6-A7EC-83A6EE675243}</MetaDataID>
        protected internal void RecalcWidthCache()
        {
            decimal total = 0;
            decimal visibleWidth = 0;
            int visibleCount = 0;
            int lastVisible = -1;

            for (int i = 0; i < this.Count; i++)
            {
                total += this[i].Width;

                if (this[i].Visible)
                {
                    this[i].X = visibleWidth;
                    visibleWidth += this[i].Width;
                    visibleCount++;
                    lastVisible = i;
                }
            }

            this._TotalColumnWidth = total;
            this._VisibleColumnsWidth = visibleWidth;
            this.visibleColumnCount = visibleCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Column at the specified index
        /// </summary>
        /// <MetaDataID>{1257203d-8d91-421d-820c-45a90b6f01e4}</MetaDataID>
        public Column this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    return null;
                }

                return this.List[index] as Column;
            }
        }


        /// <summary>
        /// Gets the ColumnModel that owns this ColumnCollection
        /// </summary>
        /// <MetaDataID>{2ea953a5-c192-462a-8433-dff0eff0521d}</MetaDataID>
        public DayTimeColumnModel ColumnModel
        {
            get
            {
                return this._DayTimeColumnModel;
            }
        }



        /// <MetaDataID>{4007cf08-c4d2-4fa3-a109-f0842a84f0bd}</MetaDataID>
        private decimal _TotalColumnWidth;
        /// <summary>
        /// Returns the total width of all the Columns in the model
        /// </summary>
        /// <MetaDataID>{7f3d8c4f-8f89-458f-acfc-30bf8bd30ac9}</MetaDataID>
        internal decimal TotalColumnWidth
        {
            get
            {
                return this._TotalColumnWidth;
            }
        }


        /// <MetaDataID>{19eae865-cd97-4985-9cc0-1c8a370e08b5}</MetaDataID>
        private decimal _VisibleColumnsWidth;
        /// <summary>
        /// Returns the total width of all the visible Columns in the model
        /// </summary>
        /// <MetaDataID>{a4d656df-ecfc-487c-8a26-594ffc0c592d}</MetaDataID>
        internal decimal VisibleColumnsWidth
        {
            get
            {
                return this._VisibleColumnsWidth;
            }
        }


        /// <summary>
        /// Returns the number of visible Columns in the model
        /// </summary>
        /// <MetaDataID>{853fe789-5ad7-4d20-93a3-dd4cc4c3d320}</MetaDataID>
        internal int VisibleColumnCount
        {
            get
            {
                return this.visibleColumnCount;
            }
        }


        /// <summary>
        /// Returns the index of the last visible Column in the model
        /// </summary>
        /// <MetaDataID>{fbe43ce1-3264-443b-bcb9-da4046431f39}</MetaDataID>
        internal int LastVisibleColumn
        {
            get
            {
                return this.lastVisibleColumn;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the ColumnAdded event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{311A01CA-4E5C-440D-B7F9-114C406D875B}</MetaDataID>
        protected virtual void OnColumnAdded(ColumnModelEventArgs e)
        {
            this._DayTimeColumnModel.OnColumnAdded(e);
        }


        /// <summary>
        /// Raises the ColumnRemoved event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{9D80202A-91AE-4007-A72C-2BB5B991D2CB}</MetaDataID>
        protected virtual void OnColumnRemoved(ColumnModelEventArgs e)
        {
            this._DayTimeColumnModel.OnColumnRemoved(e);
        }

        #endregion
    }
}
