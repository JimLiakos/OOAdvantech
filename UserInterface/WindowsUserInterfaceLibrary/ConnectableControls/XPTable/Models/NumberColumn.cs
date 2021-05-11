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
using System.Windows.Forms;

using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Models.Design;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Sorting;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Column whose Cells are displayed as a numbers
    /// </summary>
    /// <MetaDataID>{AFB73AF2-49FC-43A9-826E-A0360C17FD63}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class NumberColumn : Column
	{
		#region Class Data

		/// <summary>
		/// The value to increment or decrement a Cell when its up or down buttons are clicked
		/// </summary>
		private decimal increment;

		/// <summary>
		/// The maximum value for a Cell
		/// </summary>
		private decimal maximum;

		/// <summary>
		/// The minimum value for a Cell
		/// </summary>
		private decimal minimum;

		/// <summary>
		/// The alignment of the up and down buttons in the Column
		/// </summary>
		private LeftRightAlignment upDownAlignment;

		/// <summary>
		/// Specifies whether the up and down buttons should be drawn
		/// </summary>
		private bool showUpDownButtons;

		#endregion
		
		
		#region Constructor

        /// <summary>
        /// Creates a new NumberColumn with default values
        /// </summary>
        /// <MetaDataID>{8B9DFED4-2CAF-4856-A6EF-1477DB22275D}</MetaDataID>
		public NumberColumn() : base()
		{
			this.Init();
		}

        /// <MetaDataID>{DBB70FA7-8C27-44A5-BC70-BB7BBDD64A79}</MetaDataID>
        public NumberColumn(Column copyColumn)
            : base(copyColumn)
        {
            Init();
        }
        /// <summary>
        /// Creates a new NumberColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{C976D98E-EE77-4758-A131-D5EB8616F2BB}</MetaDataID>
		public NumberColumn(string text) : base(text)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new NumberColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{7C5EE108-53DD-4308-BAB0-2DCE5D2D7DE6}</MetaDataID>
		public NumberColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new NumberColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{94F8EF70-465D-49EC-ADFF-37F3E0AA7EF4}</MetaDataID>
		public NumberColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{AA049337-E2BE-4279-846E-7C97277DDDED}</MetaDataID>
		public NumberColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{CD82AB86-0998-485C-988D-66B0B8498FCC}</MetaDataID>
		public NumberColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, image, width 
        /// and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{CDC717E8-8A0C-4400-95B1-DE684C9791CA}</MetaDataID>
		public NumberColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the NumberColumn with default values
        /// </summary>
        /// <MetaDataID>{4ED6DE31-EE38-4F6F-8365-0ECD513E739E}</MetaDataID>
		private void Init()
		{
            //_Type = ColumnType.NumberColumn;
			this.Format = "G";

			this.maximum = (decimal) 100;
			this.minimum = (decimal) 0;
			this.increment = (decimal) 1;

			this.showUpDownButtons = false;
			this.upDownAlignment = LeftRightAlignment.Right;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{4386B9B6-E0D5-4A3D-A0E7-61E1CE8DA432}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "NUMBER";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{D33AD79D-3070-478E-B855-9468D7E45D62}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new NumberCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{B5264FDE-C0F1-4A70-ADF2-F24D99A59D29}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return "NUMBER";
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{820420FB-7CC0-44AC-9012-DFBFEA7215E3}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return new NumberCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the maximum value for Column's Cells
		/// </summary>
		[Category("Appearance"),
		Description("The maximum value for Column's Cells")]
		public decimal Maximum
		{
			get
			{
				return this.maximum;
			}

			set
			{
				this.maximum = value;
				
				if (this.minimum > this.maximum)
				{
					this.minimum = this.maximum;
				}

				this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
			}
		}


        /// <summary>
        /// Specifies whether the Maximum property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Maximum property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{91544287-87CC-4D2A-85A5-47EDE30F459D}</MetaDataID>
		private bool ShouldSerializeMaximum()
		{
			return this.maximum != (decimal) 100;
		}


		/// <summary>
		/// Gets or sets the minimum value for Column's Cells
		/// </summary>
		[Category("Appearance"),
		Description("The minimum value for Column's Cells")]
		public decimal Minimum
		{
			get
			{
				return this.minimum;
			}

			set
			{
				this.minimum = value;
				
				if (this.minimum > this.maximum)
				{
					this.maximum = value;
				}

				this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
			}
		}


        /// <summary>
        /// Specifies whether the Minimum property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Minimum property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{45B5900F-87E1-4BAC-96B7-B2E05EF738B2}</MetaDataID>
		private bool ShouldSerializeMinimum()
		{
			return this.minimum != (decimal) 0;
		}


		/// <summary>
		/// Gets or sets the value to increment or decrement a Cell when its up or down 
		/// buttons are clicked
		/// </summary>
		[Category("Appearance"),
		Description("The value to increment or decrement a Cell when its up or down buttons are clicked")]
		public decimal Increment
		{
			get
			{
				return this.increment;
			}

			set
			{
				if (value < new decimal(0))
				{
					throw new ArgumentException("value must be greater than zero");
				}

				this.increment = value;
			}
		}


        /// <summary>
        /// Specifies whether the Increment property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the Increment property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{AA8FE37D-DC5E-4791-B5ED-C46CAF011262}</MetaDataID>
		private bool ShouldSerializeIncrement()
		{
			return this.increment != (decimal) 1;
		}


		/// <summary>
		/// Gets or sets whether the Column's Cells should draw up and down buttons
		/// </summary>
		[Category("Appearance"),
		DefaultValue(false),
		Description("Determines whether the Column's Cells draw up and down buttons")]
		public bool ShowUpDownButtons
		{
			get
			{
				return this.showUpDownButtons;
			}

			set
			{
				if (this.showUpDownButtons != value)
				{
					this.showUpDownButtons = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets or sets the alignment of the up and down buttons in the Column
		/// </summary>
		[Category("Appearance"),
		DefaultValue(LeftRightAlignment.Right),
		Description("The alignment of the up and down buttons in the Column")]
		public LeftRightAlignment UpDownAlign
		{
			get
			{
				return this.upDownAlignment;
			}

			set
			{
				if (!Enum.IsDefined(typeof(LeftRightAlignment), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(LeftRightAlignment));
				}
					
				if (this.upDownAlignment != value)
				{
					this.upDownAlignment = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}


		/// <summary>
		/// Gets or sets the string that specifies how a Column's Cell contents 
		/// are formatted
		/// </summary>
		[Category("Appearance"),
		DefaultValue("G"),
		Description("A string that specifies how a column's cell contents are formatted.")]
		public new string Format
		{
			get
			{
				return base.Format;
			}

			set
			{
				base.Format = value;
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

		#endregion
	}
}
