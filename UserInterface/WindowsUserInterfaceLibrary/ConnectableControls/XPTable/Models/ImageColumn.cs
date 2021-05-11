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
using OOAdvantech.UserInterface;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Column whose Cells are displayed as an Image
    /// </summary>
    /// <MetaDataID>{70F2BB5B-6842-4C00-BA4E-9DCE7AAD5039}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class ImageColumn : Column
	{
		#region Class Data

		/// <summary>
		/// Specifies whether any text contained in the Cell should be drawn
		/// </summary>
		private bool drawText;

		#endregion
		
		
		#region Constructor

        /// <summary>
        /// Creates a new ImageColumn with default values
        /// </summary>
        /// <MetaDataID>{0AC70A9F-2C3E-45AB-8CDF-83B2144CBAF7}</MetaDataID>
		public ImageColumn() : base()
		{
			this.Init();
		}
        /// <MetaDataID>{93787167-BEBD-4EC8-8FCE-16F1D5ED1298}</MetaDataID>
        public ImageColumn(Column copyColumn)
            : base(copyColumn)
        {
            Init();
        }


        /// <summary>
        /// Creates a new ImageColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{C32B4884-3878-433A-8A64-030095DF98E2}</MetaDataID>
		public ImageColumn(string text) : base(text)
		{
			this.Init();
		}
        public ImageColumn(OOAdvantech.UserInterface.Column column)
            : base(column)
        {
            this.Init();
            drawText = (column as OOAdvantech.UserInterface.ImageColumn).DrawText;
        }
 


        /// <summary>
        /// Creates a new ImageColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{D9BE4717-E7CC-486B-BF2A-1721DB8FFDE7}</MetaDataID>
		public ImageColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ImageColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{60A99719-36B6-42C9-B582-6BF85D63EA64}</MetaDataID>
		public ImageColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ImageColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{53D6012B-399A-4144-8C4C-2D11452FE3A1}</MetaDataID>
		public ImageColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ImageColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{73A8DA6F-D924-4CB9-969E-C0CF706EEF48}</MetaDataID>
		public ImageColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new ImageColumn with the specified header text, image, width 
        /// and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{09B6FE74-4D85-4EC0-BB74-37A2E49B8CB2}</MetaDataID>
		public ImageColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the ImageColumn with default values
        /// </summary>
        /// <MetaDataID>{58DA36CA-FEA1-42BB-B143-B1A108EC9739}</MetaDataID>
		private void Init()
		{
            //_Type = ColumnType.ImageColumn;
			this.drawText = false;
			this.Editable = false;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{232889AE-F747-4552-B578-21BDD7095722}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "IMAGE";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{CF38D70F-3567-40CE-BA66-53FFE82815E1}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new ImageCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{0ECE9834-BC04-48EE-8E7E-04D3A441F624}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return null;
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{BCB4CD66-7580-4BE2-9D47-2CCE6C6502D5}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return null;
		}

		#endregion


		#region Properties
		
		/// <summary>
		/// Gets or sets whether any text contained in the Column's Cells should be drawn
		/// </summary>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Determines whether any text contained in the Column's Cells should be drawn")]
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
                    if (ColumnMetaData is OOAdvantech.UserInterface.ImageColumn)
                        (ColumnMetaData as OOAdvantech.UserInterface.ImageColumn).DrawText = value;
					this.drawText = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}

        ImageBoxSizeMode _SizeMode;
        [Category("Appearance"),
        DefaultValue(true),
        Description("Determines whether any text contained in the Column's Cells should be drawn")]
        public ImageBoxSizeMode SizeMode
        {
            get
            {
                if (ColumnMetaData is OOAdvantech.UserInterface.ImageColumn)
                    _SizeMode=(ColumnMetaData as OOAdvantech.UserInterface.ImageColumn).SizeMode;
                return _SizeMode;
            }

            set
            {
                if(_SizeMode!=value)
                {
                       if (ColumnMetaData is OOAdvantech.UserInterface.ImageColumn)
                        (ColumnMetaData as OOAdvantech.UserInterface.ImageColumn).SizeMode = value;
                    _SizeMode=value;
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
				return typeof(ImageComparer);
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
