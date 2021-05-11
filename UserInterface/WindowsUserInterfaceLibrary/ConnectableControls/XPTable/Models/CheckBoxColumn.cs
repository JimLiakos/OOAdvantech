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
    /// Represents a Column whose Cells are displayed as a CheckBox
    /// </summary>
    /// <MetaDataID>{34D8C06D-1D73-4DD1-849F-80CDC3D3C2CE}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class CheckBoxColumn : Column
	{
        
		#region Class Data

		/// <summary>
		/// The size of the checkbox
		/// </summary>
		private Size checkSize=new Size(10,10);

		/// <summary>
		/// Specifies whether any text contained in the Cell should be drawn
		/// </summary>
		private bool drawText;

		/// <summary>
		/// The style of the checkboxes
		/// </summary>
        private CheckBoxColumnStyle checkStyle = CheckBoxColumnStyle.CheckBox;

		#endregion
		

		#region Constructor

        /// <summary>
        /// Creates a new CheckBoxColumn with default values
        /// </summary>
        /// <MetaDataID>{42419C20-5B36-4D3A-8E4C-9D648A565AE9}</MetaDataID>
		public CheckBoxColumn() : base()
		{
			this.Init();
		}
        /// <MetaDataID>{652CD959-E757-4D4D-9AA4-079EA59458EE}</MetaDataID>
        public CheckBoxColumn(Column copyColumn)
            : base(copyColumn)
        {
            Init();
        }
        public CheckBoxColumn(OOAdvantech.UserInterface.CheckBoxColumn copyColumn)
            : base(copyColumn)
        {
        }



        /// <summary>
        /// Creates a new CheckBoxColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{6B3524F8-B264-4994-A5C0-9539ED84ECD1}</MetaDataID>
		public CheckBoxColumn(string text) : base(text)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new CheckBoxColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{3249420C-C79F-4FAD-89AE-056DB4B037CD}</MetaDataID>
		public CheckBoxColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new CheckBoxColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{D7A7F820-E3A7-4F0D-AC69-EB4A8BBBC2E4}</MetaDataID>
		public CheckBoxColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new CheckBoxColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{2A5C9992-F9AC-4358-A2C5-FD0B97380CFB}</MetaDataID>
		public CheckBoxColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new CheckBoxColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{947105F7-1EA5-43EA-9AFD-B6F0C30E7B75}</MetaDataID>
		public CheckBoxColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new CheckBoxColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{EB0F923E-0C86-487D-9DFD-DDF92D23A0BB}</MetaDataID>
		public CheckBoxColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the CheckBoxColumn with default values
        /// </summary>
        /// <MetaDataID>{CE6907FF-A0A1-4555-8F07-78E754EBC948}</MetaDataID>
		private void Init()
		{
			this.checkSize = new Size(13, 13);
			this.drawText = true;
			this.checkStyle = CheckBoxColumnStyle.CheckBox;
            //_Type = ColumnType.CheckBoxColumn;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{F821DF20-28A5-4B3D-AD18-2F382EA6CA6C}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "CHECKBOX";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{8557E2D6-2752-4764-8FE8-FF9BD1F30F31}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new CheckBoxCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{28B70CEB-3925-4B97-8CBB-2D9057414690}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return null;
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{4A0AA6C8-32C6-4296-A977-A46E2A3D61A8}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return null;
		}

		#endregion
		
		
		#region Properties

		/// <summary>
		/// Gets or sets the size of the checkboxes
		/// </summary>
		[Category("Appearance"),
		Description("Specifies the size of the checkboxes")]
		public Size CheckSize
		{
			get
			{
                checkSize = new Size(12, 12);
				return this.checkSize;
			}

			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					value = new Size(13, 13);
				}
				
				if (this.checkSize != value)
				{
					this.checkSize = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
                checkSize = new Size(12, 12);
			}
		}


        /// <summary>
        /// Specifies whether the CheckSize property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the CheckSize property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{72D23260-9D06-4F3E-8E9B-DB6F31AB1442}</MetaDataID>
		private bool ShouldSerializeCheckSize()
		{
			return (this.checkSize.Width != 13 || this.checkSize.Height != 13);
		}

		
		/// <summary>
		/// Gets or sets whether any text contained in the Cell should be drawn
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Specifies whether any text contained in the Cell should be drawn")]
		public bool DrawText
		{
			get
			{
				return this.drawText;
			}

			set
			{
				if (this.drawText != value)
				{
					this.drawText = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}

		
		/// <summary>
		/// Gets or sets whether any text contained in the Cell should be drawn
		/// </summary>
		[Category("Appearance"),
		DefaultValue(CheckBoxColumnStyle.CheckBox),
		Description("Specifies the style of the checkboxes")]
		public CheckBoxColumnStyle CheckStyle
		{
			get
			{
				return this.checkStyle;
			}

			set
			{
				if (!Enum.IsDefined(typeof(CheckBoxColumnStyle), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(CheckBoxColumnStyle));
				}
					
				if (this.checkStyle != value)
				{
					this.checkStyle = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
                checkStyle = CheckBoxColumnStyle.CheckBox;
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
				return typeof(CheckBoxComparer);
			}
		}

		#endregion
	}
}
