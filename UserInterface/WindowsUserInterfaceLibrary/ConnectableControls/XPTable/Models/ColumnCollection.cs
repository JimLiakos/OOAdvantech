/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.Collections;

using ConnectableControls.List.Events;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a collection of Column objects
    /// </summary>
    /// <MetaDataID>{A5FF9458-C0E5-4CF5-9A24-2E1C9050BC37}</MetaDataID>
	public class ColumnCollection : CollectionBase
	{
		#region Class Data

		/// <summary>
		/// The ColumnModel that owns the CollumnCollection
		/// </summary>
		private ColumnModel owner;

		/// <summary>
		/// A local cache of the combined width of all columns
		/// </summary>
		private int totalColumnWidth;

		/// <summary>
		/// A local cache of the combined width of all visible columns
		/// </summary>
		private int visibleColumnsWidth;

		/// <summary>
		/// A local cache of the number of visible columns
		/// </summary>
		private int visibleColumnCount;

		/// <summary>
		/// A local cache of the last visible column in the collection
		/// </summary>
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
		public ColumnCollection(ColumnModel owner) : base()
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
				
			this.owner = owner;
			this.totalColumnWidth = 0;
			this.visibleColumnsWidth = 0;
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

			this.OnColumnAdded(new ColumnModelEventArgs(this.owner, column, index, index));

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

			for (int i=0; i<columns.Length; i++)
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

			for (int i=0; i<columns.Length; i++)
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

				this.OnColumnRemoved(new ColumnModelEventArgs(this.owner, column, index, index));
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

			for (int i=0; i<this.Count; i++)
			{
				this[i].ColumnModel = null;
			}

			base.Clear();
			this.InnerList.Capacity = 0;

			this.RecalcWidthCache();

			this.OnColumnRemoved(new ColumnModelEventArgs(this.owner, null, -1, -1));
		}


        /// <summary>
        /// Returns the index of the specified Column in the model
        /// </summary>
        /// <param name="column">The Column to look for</param>
        /// <returns>The index of the specified Column in the model</returns>
        /// <MetaDataID>{A623B7EB-F991-4910-A59A-B14F8A54BFC2}</MetaDataID>
		public int IndexOf(Column column)
		{
			for (int i=0; i<this.Count; i++)
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
			int total = 0;
			int visibleWidth = 0;
			int visibleCount = 0;
			int lastVisible = -1;

			for (int i=0; i<this.Count; i++)
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

			this.totalColumnWidth = total;
			this.visibleColumnsWidth = visibleWidth;
			this.visibleColumnCount = visibleCount;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the Column at the specified index
		/// </summary>
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
		public ColumnModel ColumnModel
		{
			get
			{
				return this.owner;
			}
		}


		/// <summary>
		/// Returns the total width of all the Columns in the model
		/// </summary>
		internal int TotalColumnWidth
		{
			get
			{
				return this.totalColumnWidth;
			}
		}


		/// <summary>
		/// Returns the total width of all the visible Columns in the model
		/// </summary>
		internal int VisibleColumnsWidth
		{
			get
			{
				return this.visibleColumnsWidth;
			}
		}


		/// <summary>
		/// Returns the number of visible Columns in the model
		/// </summary>
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
			this.owner.OnColumnAdded(e);
		}


        /// <summary>
        /// Raises the ColumnRemoved event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{9D80202A-91AE-4007-A72C-2BB5B991D2CB}</MetaDataID>
		protected virtual void OnColumnRemoved(ColumnModelEventArgs e)
		{
			this.owner.OnColumnRemoved(e);
		}

		#endregion
	}
}
