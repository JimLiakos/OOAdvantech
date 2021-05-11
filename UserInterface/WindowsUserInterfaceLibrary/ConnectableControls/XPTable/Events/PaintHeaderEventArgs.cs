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
	/// Represents the method that will handle the PaintHeader events of a Table
	/// </summary>
	public delegate void PaintHeaderEventHandler(object sender, PaintHeaderEventArgs e);

	#endregion



	#region PaintHeaderEventArgs

    /// <summary>
    /// Provides data for the PaintHeader event
    /// </summary>
    /// <MetaDataID>{91352B53-3B50-4CFB-B32E-B6C7AA0A94CF}</MetaDataID>
    public class PaintHeaderEventArgs : System.Windows.Forms.PaintEventArgs
	{
		#region Class Data

		/// <summary>
		/// The Column to be painted
		/// </summary>
		private Column column;
		
		/// <summary>
		/// The Table the Column's ColumnModel belongs to
		/// </summary>
		private ListView table;
		
		/// <summary>
		/// The index of the Column in the Table's ColumnModel
		/// </summary>
		private int columnIndex;
		
		/// <summary>
		/// The style of the Column header
		/// </summary>
        private System.Windows.Forms.ColumnHeaderStyle headerStyle;

		/// <summary>
		/// The rectangle in which to paint
		/// </summary>
		private Rectangle headerRect;

		/// <summary>
		/// Indicates whether the user has done the paining for us
		/// </summary>
		private bool handled;

		#endregion


		#region Constructor

        /// <summary>
        /// Initializes a new instance of the PaintHeaderEventArgs class with 
        /// the specified graphics and clipping rectangle
        /// </summary>
        /// <param name="g">The Graphics used to paint the Column header</param>
        /// <param name="headerRect">The Rectangle that represents the rectangle 
        /// in which to paint</param>
        /// <MetaDataID>{4E87E5AC-F43B-44FC-BABB-A8DA09F39A39}</MetaDataID>
        public PaintHeaderEventArgs(Graphics g, Rectangle headerRect)
            : this(g, null, null, -1, System.Windows.Forms.ColumnHeaderStyle.None, headerRect)
		{

		}


        /// <summary>
        /// Initializes a new instance of the PaintHeaderEventArgs class with 
        /// the specified graphics, column, table, column index, header style 
        /// and clipping rectangle
        /// </summary>
        /// <param name="g">The Graphics used to paint the Column header</param>
        /// <param name="column">The Column to be painted</param>
        /// <param name="table">The Table the Column's ColumnModel belongs to</param>
        /// <param name="columnIndex">The index of the Column in the Table's ColumnModel</param>
        /// <param name="headerStyle">The style of the Column's header</param>
        /// <param name="headerRect">The Rectangle that represents the rectangle 
        /// in which to paint</param>
        /// <MetaDataID>{F31D6340-8EB5-44ED-A0A8-A820937081DA}</MetaDataID>
        public PaintHeaderEventArgs(Graphics g, Column column, ListView table, int columnIndex, System.Windows.Forms.ColumnHeaderStyle headerStyle, Rectangle headerRect)
            : base(g, headerRect)
		{
			this.column = column;
			this.table = table;
			this.columnIndex = columnIndex;
			this.column = column;
			this.headerStyle = headerStyle;
			this.headerRect = headerRect;
			this.handled = false;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the Column to be painted
		/// </summary>
		public Column Column
		{
			get
			{
				return this.column;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="column"></param>
        /// <MetaDataID>{FED73FD2-531A-4102-8F55-B9FE81F91B3A}</MetaDataID>
		internal void SetColumn(Column column)
		{
			this.column = column;
		}


		/// <summary>
		/// Gets the Table the Column's ColumnModel belongs to
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
        /// <MetaDataID>{AA77D17B-6008-4462-9466-47FD7084D642}</MetaDataID>
		internal void SetTable(ListView table)
		{
			this.table = table;
		}


		/// <summary>
		/// Gets the index of the Column in the Table's ColumnModel
		/// </summary>
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <MetaDataID>{ED6B949E-EE00-4D2D-9A99-E49C6FA1666A}</MetaDataID>
		internal void SetColumnIndex(int columnIndex)
		{
			this.columnIndex = columnIndex;
		}


		/// <summary>
		/// Gets the style of the Column's header
		/// </summary>
        public System.Windows.Forms.ColumnHeaderStyle HeaderStyle
		{
			get
			{
				return this.headerStyle;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="headerStyle"></param>
        /// <MetaDataID>{468E9F9C-3CCB-444B-81D6-6EC10E5D675B}</MetaDataID>
        internal void SetHeaderStyle(System.Windows.Forms.ColumnHeaderStyle headerStyle)
		{
			this.headerStyle = headerStyle;
		}


		/// <summary>
		/// Gets the column header's bounding rectangle
		/// </summary>
		public Rectangle HeaderRect
		{
			get
			{
				return this.headerRect;
			}
		}


        /// <summary>
        /// </summary>
        /// <param name="headerRect"></param>
        /// <MetaDataID>{CDF4A496-D77F-42A2-B9AD-65E39C8D71D8}</MetaDataID>
		internal void SetHeaderRect(Rectangle headerRect)
		{
			this.headerRect = headerRect;
		}


		/// <summary>
		/// Gets or sets a value indicating whether the BeforePaintHeader 
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
