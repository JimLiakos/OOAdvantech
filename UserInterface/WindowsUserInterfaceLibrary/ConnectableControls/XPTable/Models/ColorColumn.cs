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
    /// Represents a Column whose Cells are displayed as a Color
    /// </summary>
    /// <MetaDataID>{AC2ABE59-DE9C-43B3-B532-A26819C87937}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class ColorColumn : DropDownColumn
	{
		#region Class Data
		
		/// <summary>
		/// Specifies whether the Cells should draw their Color value
		/// </summary>
		private bool showColor;

		/// <summary>
		/// Specifies whether the Cells should draw their Color name
		/// </summary>
		private bool showColorName;

		#endregion


		#region Constructor

        /// <summary>
        /// Creates a new ColorColumn with default values
        /// </summary>
        /// <MetaDataID>{B3AD93C9-BE9B-46B2-B862-8DF2CCF8D840}</MetaDataID>
		public ColorColumn() : base()
		{
			this.Init();
		}
        /// <MetaDataID>{2453A916-4FD6-42A6-A590-916F87D485FE}</MetaDataID>
        public ColorColumn(Column copyColumn)
            : base(copyColumn)
        {
            Init();
        }


        /// <summary>
        /// Creates a new ColorColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{3C15BE11-5B44-4F06-88D5-3B544C9C5646}</MetaDataID>
		public ColorColumn(string text) : base(text)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ColorColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{C2116F35-B848-414F-BDCA-1EDCDB7C4C74}</MetaDataID>
		public ColorColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ColorColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{A3645B66-5ED5-4C7F-A2C2-BD44C9DB7E42}</MetaDataID>
		public ColorColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ColorColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{2B14340F-5E58-4BE2-AAD1-EF9B8D7952F3}</MetaDataID>
		public ColorColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ColorColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{6B21C1B7-528D-45D4-BCE3-C1ACEA488447}</MetaDataID>
		public ColorColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ColorColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{8A84630C-7299-412E-B976-ED251E7B8401}</MetaDataID>
		public ColorColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the ColorColumn with default values
        /// </summary>
        /// <MetaDataID>{3333C758-D611-41D3-9982-CE22F5FFAA4A}</MetaDataID>
		private void Init()
		{
			this.showColor = true;
			this.showColorName = true;
            //_Type = ColumnType.ColorColumn;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{D7DCC9AC-0A1C-4E61-ADA1-506EA2CB9D9D}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "COLOR";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{7230B26F-5461-4B9C-A7AE-163DC611F2FB}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new ColorCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{9D326552-D492-4862-A179-B06859803AA1}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return "COLOR";
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{AADDB06C-85FF-4059-A9AA-5ACF08BF7BA8}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return new ColorCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets whether the Column's Cells should draw their Color value
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Specifies whether the Column's Cells should draw their Color value")]
		public bool ShowColor
		{
			get
			{
				return this.showColor;
			}

			set
			{
				if (this.showColor != value)
				{
					this.showColor = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets or sets whether the Column's Cells should draw their Color name
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Specifies whether the Column's Cells should draw their Color name")]
		public bool ShowColorName
		{
			get
			{
				return this.showColorName;
			}

			set
			{
				if (this.showColorName != value)
				{
					this.showColorName = value;

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
				return typeof(ColorComparer);
			}
		}

		#endregion
	}
}
