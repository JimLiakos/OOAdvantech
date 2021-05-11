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
	/// Represents the method that will handle the PaintCell events of a Table
	/// </summary>
	public delegate void PaintCellEventHandler(object sender, PaintCellEventArgs e);

	#endregion



	#region PaintCellEventArgs

    /// <summary>
    /// Provides data for the PaintCell event
    /// </summary>
    /// <MetaDataID>{0762ED35-B238-4BC9-8917-0DC2981C2B80}</MetaDataID>
    public class PaintCellEventArgs : System.Windows.Forms.PaintEventArgs
	{
		#region Class Data

		/// <summary>
		/// The Cell to be painted
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
		/// Specifies whether the Cell is selected
		/// </summary>
		private bool selected;
		
		/// <summary>
		/// Specifies whether the Cell has focus
		/// </summary>
		private bool focused;

		/// <summary>
		/// Specifies whether the Cell's Column is sorted
		/// </summary>
		private bool sorted;

		/// <summary>
		/// Specifies whether the Cell is editable
		/// </summary>
		private bool editable;

		/// <summary>
		/// Specifies whether the Cell is enabled
		/// </summary>
		private bool enabled;
		
		/// <summary>
		/// The rectangle in which to paint the Cell
		/// </summary>
		private Rectangle cellRect;

		/// <summary>
		/// Indicates whether the user has done the paining for us
		/// </summary>
		private bool handled;

		#endregion


		#region Constructor

        /// <summary>
        /// Initializes a new instance of the PaintCellEventArgs class with 
        /// the specified graphics and clipping rectangle
        /// </summary>
        /// <param name="g">The Graphics used to paint the Cell</param>
        /// <param name="cellRect">The Rectangle that represents the rectangle 
        /// in which to paint</param>
        /// <MetaDataID>{981CE894-6A2E-4F5A-B693-5BDE4712E382}</MetaDataID>
		public PaintCellEventArgs(Graphics g, Rectangle cellRect) : this(g, null, null, -1, -1, false, false, false, false, true, cellRect)
		{
			
		}


        /// <summary>
        /// Initializes a new instance of the PaintCellEventArgs class with 
        /// the specified graphics, table, row index, column index, selected value,  
        /// focused value, mouse value and clipping rectangle
        /// </summary>
        /// <param name="g">The Graphics used to paint the Cell</param>
        /// <param name="cell">The Cell to be painted</param>
        /// <param name="table">The Table the Cell belongs to</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="selected">Specifies whether the Cell is selected</param>
        /// <param name="focused">Specifies whether the Cell has focus</param>
        /// <param name="sorted">Specifies whether the Cell's Column is sorted</param>
        /// <param name="editable">Specifies whether the Cell is able to be edited</param>
        /// <param name="enabled">Specifies whether the Cell is enabled</param>
        /// <param name="cellRect">The rectangle in which to paint the Cell</param>
        /// <MetaDataID>{554B6032-A8C1-4B99-94C3-F84BD3F78FB6}</MetaDataID>
		public PaintCellEventArgs(Graphics g, Cell cell, ListView table, int row, int column, bool selected, bool focused, bool sorted, bool editable, bool enabled, Rectangle cellRect) : base(g, cellRect)
		{
			this.cell = cell;
			this.table = table;
			this.row = row;
			this.column = column;
			this.selected = selected;
			this.focused = focused;
			this.sorted = sorted;
			this.editable = editable;
			this.enabled = enabled;
			this.cellRect = cellRect;
			this.handled = false;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the Cell to be painted
		/// </summary>
		public Cell Cell
		{
			get
			{
				return this.cell;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="cell"></param>
        /// <MetaDataID>{1FF55F1A-BCA2-4E0D-8AD1-C000EA714FC7}</MetaDataID>
		internal void SetCell(Cell cell)
		{
			this.cell = cell;
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
        /// </summary>
        /// <param name="table"></param>
        /// <MetaDataID>{D937C4AA-CF10-436E-BA31-858BD6EF1100}</MetaDataID>
		internal void SetTable(ListView table)
		{
			this.table = table;
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
        /// <MetaDataID>{E6A6F0B9-96E0-4E52-B1B5-AD9B36B2893B}</MetaDataID>
		internal void SetRow(int row)
		{
			this.row = row;
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
        /// <MetaDataID>{152CD9E5-1D4A-4CA9-A41D-E04047248A68}</MetaDataID>
		internal void SetColumn(int column)
		{
			this.column = column;
		}


		/// <summary>
		/// Gets whether the Cell is selected
		/// </summary>
		public bool Selected
		{
			get
			{
				return this.selected;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="selected"></param>
        /// <MetaDataID>{87B37F78-2BD7-498B-A600-F35C999C13D8}</MetaDataID>
		internal void SetSelected(bool selected)
		{
			this.selected = selected;
		}


		/// <summary>
		/// Gets whether the Cell has focus
		/// </summary>
		public bool Focused
		{
			get
			{
				return this.focused;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="focused"></param>
        /// <MetaDataID>{39BD4853-0529-48C7-AB91-799F6E76E159}</MetaDataID>
		internal void SetFocused(bool focused)
		{
			this.focused = focused;
		}


		/// <summary>
		/// Gets whether the Cell's Column is sorted
		/// </summary>
		public bool Sorted
		{
			get
			{
				return this.sorted;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="sorted"></param>
        /// <MetaDataID>{15156114-2582-4130-ADC9-A6FBCE7435DE}</MetaDataID>
		internal void SetSorted(bool sorted)
		{
			this.sorted = sorted;
		}


		/// <summary>
		/// Gets whether the Cell is able to be edited
		/// </summary>
		public bool Editable
		{
			get
			{
				return this.editable;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="editable"></param>
        /// <MetaDataID>{9657F30C-7AE2-4661-AA51-184D147D5EC7}</MetaDataID>
		internal void SetEditable(bool editable)
		{
			this.editable = editable;
		}


		/// <summary>
		/// Gets whether the Cell is enabled
		/// </summary>
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="enabled"></param>
        /// <MetaDataID>{2691E22F-5283-4841-B784-93AB4A0385CE}</MetaDataID>
		internal void SetEnabled(bool enabled)
		{
			this.enabled = enabled;
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
        /// </summary>
        /// <param name="cellRect"></param>
        /// <MetaDataID>{57D77882-E6D2-4BC9-98ED-B494C17E77E0}</MetaDataID>
		internal void SetCellRect(Rectangle cellRect)
		{
			this.cellRect = cellRect;
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


		/// <summary>
		/// Gets or sets a value indicating whether the BeforePaintCell 
		/// event was handled
		/// </summary>
		public bool Handled
		{
			get
			{
				return this.handled;
			}

			set
			{
				this.handled = value;
			}
		}

		#endregion
	}

	#endregion
}
