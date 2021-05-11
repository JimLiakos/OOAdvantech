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
using System.ComponentModel;
using System.Drawing;

using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Models.Design;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Sorting;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Column whose Cells are displayed as a ProgressBar
    /// </summary>
    /// <MetaDataID>{EA4690A4-656D-42B5-A9CA-50A45D54C49F}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class ProgressBarColumn : Column
	{
		#region Class Data

		/// <summary>
		/// Specifies whether the ProgressBar's value as a string 
		/// should be displayed
		/// </summary>
		private bool drawPercentageText;

		#endregion
		
		
		#region Constructor

        /// <summary>
        /// Creates a new ProgressBarColumn with default values
        /// </summary>
        /// <MetaDataID>{60FC63CA-F412-4539-9B1C-A6E31091BC9B}</MetaDataID>
		public ProgressBarColumn() : base()
		{
			this.Init();
		}
        /// <MetaDataID>{4980161F-A49C-4688-BD1F-A5705E3874A0}</MetaDataID>
        public ProgressBarColumn(Column copyColumn)
            : base(copyColumn)
        {
            Init();
        }

        /// <summary>
        /// Creates a new ProgressBarColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{44AB22D7-69FE-4AB2-8D25-6A5D47AC7B72}</MetaDataID>
		public ProgressBarColumn(string text) : base(text)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ProgressBarColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{FA17B6BE-678A-4510-8764-DE66C1661608}</MetaDataID>
		public ProgressBarColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ProgressBarColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{6B038DDD-16B3-4DE9-9477-26A141A9CD37}</MetaDataID>
		public ProgressBarColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ProgressBarColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{09F1D6BF-ED96-4030-A8D5-31167912CA7B}</MetaDataID>
		public ProgressBarColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ProgressBarColumn with the specified header text, image 
        /// and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{1213F6F4-FDDD-4F47-A2BD-E7B54926A103}</MetaDataID>
		public ProgressBarColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ProgressBarColumn with the specified header text, image, 
        /// width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{81EFA522-A6E4-48C3-81F5-0A5531B3B7DF}</MetaDataID>
		public ProgressBarColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the ProgressBarColumn with default values
        /// </summary>
        /// <MetaDataID>{FE8BB351-A2FD-4C7E-86B0-F03E708AF5E0}</MetaDataID>
		private void Init()
		{
            //_Type = ColumnType.ProgressBarColumn;
			this.drawPercentageText = true;
			this.Editable = false;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{96254421-0C7C-4CEF-847F-DCF6CDCD4AD1}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "PROGRESSBAR";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{6DFA585A-DD4F-4B3D-BBFA-DB746E2F5695}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new ProgressBarCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{2AF26E56-10D3-4B35-A6F0-C99F9717B77A}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return null;
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{C44AF9B3-856B-48A3-B8D6-E3FD838D93E0}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return null;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets whether a Cell's percantage value should be drawn as a string
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Indicates whether a Cell's percantage value is drawn as a string")]
		public bool DrawPercentageText
		{
			get
			{
				return this.drawPercentageText;
			}

			set
			{
				if(this.drawPercentageText != value)
				{
					this.drawPercentageText = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets the Type of the Comparer used to compare the Column's Cells when 
		/// the Column is sorting
		/// </summary>
		public override Type DefaultComparerType
		{
			get
			{
				return typeof(NumberComparer);
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether the Column's Cells contents 
		/// are able to be edited
		/// </summary>
		[Category("Appearance"),
		DefaultValue(false),
		Description("Controls whether the column's cell contents are able to be changed by the user")]
		public new bool Editable
		{
			get
			{
				return base.Editable;
			}

			set
			{
				base.Editable = value;
			}
		}

		#endregion
	}
}
