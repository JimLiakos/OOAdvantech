using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.RowDesign
{
    /// <MetaDataID>{d7c61566-2d71-4f7e-95bc-c7c0ef87fa82}</MetaDataID>
    public class CellCollection : CollectionBase
    {
        #region Class Data

        /// <summary>
        /// The Row that owns the CellCollection
        /// </summary>
        private Row owner;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellCollection class 
        /// that belongs to the specified Row
        /// </summary>
        /// <param name="owner">A Row representing the row that owns 
        /// the Cell collection</param>
        /// <MetaDataID>{65D78E87-5A9B-4697-9E3E-D66C6C9F38D8}</MetaDataID>
        public CellCollection(Row owner)
            : base()
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified Cell to the end of the collection
        /// </summary>
        /// <param name="cell">The Cell to add</param>
        /// <MetaDataID>{BF459994-466B-49B3-856A-4460F6E59A27}</MetaDataID>
        public int Add(Cell cell)
        {
            if (cell == null)
            {
                throw new System.ArgumentNullException("Cell is null");
            }

            int index = this.List.Add(cell);

            this.OnCellAdded(new RowEventArgs(this.owner, cell, index, index));

            return index;
        }


        /// <summary>
        /// Adds an array of Cell objects to the collection
        /// </summary>
        /// <param name="cells">An array of Cell objects to add 
        /// to the collection</param>
        /// <MetaDataID>{A4563AD4-28DC-439E-9C7F-5DDE60AC654F}</MetaDataID>
        public void AddRange(Cell[] cells)
        {
            if (cells == null)
            {
                throw new System.ArgumentNullException("Cell[] is null");
            }

            for (int i = 0; i < cells.Length; i++)
            {
                this.Add(cells[i]);
            }
        }


        /// <summary>
        /// Removes the specified Cell from the model
        /// </summary>
        /// <param name="cell">The Cell to remove</param>
        /// <MetaDataID>{2E96F3B0-AA05-4552-9247-58E1684961D9}</MetaDataID>
        public void Remove(Cell cell)
        {
            int cellIndex = this.IndexOf(cell);

            if (cellIndex != -1)
            {
                this.RemoveAt(cellIndex);
            }
        }


        /// <summary>
        /// Removes an array of Cell objects from the collection
        /// </summary>
        /// <param name="cells">An array of Cell objects to remove 
        /// from the collection</param>
        /// <MetaDataID>{61A430F5-5EB2-4452-A182-BD11992501FE}</MetaDataID>
        public void RemoveRange(Cell[] cells)
        {
            if (cells == null)
            {
                throw new System.ArgumentNullException("Cell[] is null");
            }

            for (int i = 0; i < cells.Length; i++)
            {
                this.Remove(cells[i]);
            }
        }


        /// <summary>
        /// Removes the Cell at the specified index from the collection
        /// </summary>
        /// <param name="index">The index of the Cell to remove</param>
        /// <MetaDataID>{A29F790D-A15B-4E8E-A22E-1E71C2FB9775}</MetaDataID>
        public new void RemoveAt(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                Cell cell = this[index];

                this.List.RemoveAt(index);

                this.OnCellRemoved(new RowEventArgs(this.owner, cell, index, index));
            }
        }


        /// <summary>
        /// Removes all Cells from the collection
        /// </summary>
        /// <MetaDataID>{BF1FF5A8-848B-4DDE-BE30-8E64A44E6D18}</MetaDataID>
        public new void Clear()
        {
            if (this.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.Count; i++)
            {
                this[i].InternalRow = null;
            }

            base.Clear();
            this.InnerList.Capacity = 0;

            this.OnCellRemoved(new RowEventArgs(this.owner, null, -1, -1));
        }


        /// <summary>
        /// Inserts a Cell into the collection at the specified index
        /// </summary>
        /// <param name="index">The zero-based index at which the Cell 
        /// should be inserted</param>
        /// <param name="cell">The Cell to insert</param>
        /// <MetaDataID>{5C03D6E0-2BFF-4D88-842D-BE7648387076}</MetaDataID>
        public void Insert(int index, Cell cell)
        {
            if (cell == null)
            {
                return;
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index >= this.Count)
            {
                this.Add(cell);
            }
            else
            {
                base.List.Insert(index, cell);

                this.OnCellAdded(new RowEventArgs(this.owner, cell, index, index));
            }
        }


        /// <summary>
        /// Inserts an array of Cells into the collection at the specified index
        /// </summary>
        /// <param name="index">The zero-based index at which the cells should be inserted</param>
        /// <param name="cells">An array of Cells to be inserted into the collection</param>
        /// <MetaDataID>{FE6BABB4-C82F-4EA0-838B-4056B3A92E94}</MetaDataID>
        public void InsertRange(int index, Cell[] cells)
        {
            if (cells == null)
            {
                throw new System.ArgumentNullException("Cell[] is null");
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index >= this.Count)
            {
                this.AddRange(cells);
            }
            else
            {
                for (int i = cells.Length - 1; i >= 0; i--)
                {
                    this.Insert(index, cells[i]);
                }
            }
        }


        /// <summary>
        /// Returns the index of the specified Cell in the model
        /// </summary>
        /// <param name="cell">The Cell to look for</param>
        /// <returns>The index of the specified Cell in the model</returns>
        /// <MetaDataID>{FCF823EC-A649-4804-8E68-D13C9CD91C2C}</MetaDataID>
        public int IndexOf(Cell cell)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] == cell)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Cell at the specified index
        /// </summary>
        public Cell this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    return null;
                }

                return this.List[index] as Cell;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the CellAdded event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{9EC9FD99-13E2-4C23-A9B4-8C1F4F382C4F}</MetaDataID>
        protected virtual void OnCellAdded(RowEventArgs e)
        {
            this.owner.OnCellAdded(e);
        }


        /// <summary>
        /// Raises the CellRemoved event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{42DEFD61-7844-443A-A827-F763118A095B}</MetaDataID>
        protected virtual void OnCellRemoved(RowEventArgs e)
        {
            this.owner.OnCellRemoved(e);
        }

        #endregion
    }
}
