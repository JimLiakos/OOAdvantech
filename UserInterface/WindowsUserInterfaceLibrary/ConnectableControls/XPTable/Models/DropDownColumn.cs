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
using ConnectableControls.List.Renderers;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Column whose Cells are displayed with a drop down 
    /// button for editing
    /// </summary>
    /// <MetaDataID>{01B4B12A-A3D7-44A9-9DFF-744ECFE14B64}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public abstract class DropDownColumn : Column
	{
		#region Class Data

        /// <summary>
        /// Specifies whether the Cells should draw a drop down button
        /// </summary>
        /// <MetaDataID>{438fc560-7f4b-45d5-85c1-829441f5068f}</MetaDataID>
		private bool showButton;

		#endregion


		#region Constructor

        /// <summary>
        /// Creates a new DropDownColumn with default values
        /// </summary>
        /// <MetaDataID>{E0867943-997A-4390-92D1-7EEA68959F13}</MetaDataID>
		public DropDownColumn() : base()
		{
			this.Init();
		}
        /// <MetaDataID>{09C2A964-4898-42E9-8799-DFA9FEBD0347}</MetaDataID>
        public DropDownColumn(Column copyColumn)
            : base(copyColumn)
        {
            this.Init();
        }

        /// <MetaDataID>{09C2A964-4898-42E9-8799-DFA9FEBD0347}</MetaDataID>
        public DropDownColumn(OOAdvantech.UserInterface.Column copyColumn)
            : base(copyColumn)
        {
            this.Init();
        }



        /// <summary>
        /// Creates a new DropDownColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{D6241AAB-BF04-48DA-AC21-B1A0231A773B}</MetaDataID>
		public DropDownColumn(string text) : base(text)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DropDownColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{A396A835-BF69-4F8B-BFB6-70E7C50008D9}</MetaDataID>
		public DropDownColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DropDownColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{0EBD88DF-B9AD-4D1E-BF43-E2E7C965C6C1}</MetaDataID>
		public DropDownColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DropDownColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{DC896AAD-46D8-4AED-9FFA-D0F47AF93FE2}</MetaDataID>
		public DropDownColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DropDownColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{1B21B8A1-B409-491C-ABFB-F395F536AC3C}</MetaDataID>
		public DropDownColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DropDownColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{5B0D41C8-5239-4B6D-A647-64E4C933718F}</MetaDataID>
		public DropDownColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the DropDownColumn with default values
        /// </summary>
        /// <MetaDataID>{AEE2F359-A78A-48C6-B236-0A60B9D3C970}</MetaDataID>
		private void Init()
		{
			this.showButton = true;
		}

		#endregion


		#region Properties

        /// <summary>
        /// Gets or sets whether the Column's Cells should draw a drop down button
        /// </summary>
        /// <MetaDataID>{eb63a068-ce48-4ef9-8e0b-97d54890badf}</MetaDataID>
		[Category("Appearance"),
		DefaultValue(true),
		Description("Determines whether the Column's Cells should draw a drop down button")]
		public bool ShowDropDownButton
		{
			get
			{
				return this.showButton;
			}

			set
			{
				if(this.showButton != value)
				{
					this.showButton = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}
			}
		}

		#endregion
	}
}
