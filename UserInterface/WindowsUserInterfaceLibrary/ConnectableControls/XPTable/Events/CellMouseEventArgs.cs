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
using System.Drawing;
//using System.Windows.Forms;

using ConnectableControls.List.Models;


namespace ConnectableControls.List.Events
{
	#region Delegates

	/// <summary>
	/// Represents the method that will handle the CellMouseEnter, CellMouseLeave, 
	/// CellMouseDown, CellMouseUp, CellMouseMove and CellMouseHover events of a Table
	/// </summary>
	public delegate void CellMouseEventHandler(object sender, CellMouseEventArgs e);

	#endregion



	#region CellMouseEventArgs

    /// <summary>
    /// Provides data for the CellMouseEnter, CellMouseLeave, CellMouseDown, 
    /// CellMouseUp and CellMouseMove events of a Table
    /// </summary>
    /// <MetaDataID>{A9BF7841-08C6-4165-977A-C1133D1B4FE5}</MetaDataID>
	public class CellMouseEventArgs :System.Windows.Forms.MouseEventArgs
	{
		#region Class Data

		/// <summary>
		/// The Cell that raised the event
		/// </summary>
		private Cell cell;
		
		/// <summary>
		/// The Table the Cell belongs to
		/// </summary>
		private ListView table;
		
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
		public CellMouseEventArgs(Cell cell, ListView table, CellPos cellPos, Rectangle cellRect) : base(System.Windows.Forms.MouseButtons.None, 0, -1, -1, 0)
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
        public CellMouseEventArgs(Cell cell, ListView table, int row, int column, Rectangle cellRect): base(System.Windows.Forms.MouseButtons.None, 0, -1, -1, 0)
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
        public CellMouseEventArgs(Cell cell, ListView table, int row, int column, Rectangle cellRect, System.Windows.Forms.MouseEventArgs mea)
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
        public CellMouseEventArgs(Cell cell, ListView table, CellPos cellPos, Rectangle cellRect, System.Windows.Forms.MouseEventArgs mea)
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
		public ListView Table
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

	#endregion
}
