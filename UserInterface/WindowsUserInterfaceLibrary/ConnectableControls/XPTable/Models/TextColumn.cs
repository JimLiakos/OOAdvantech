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
    /// Summary description for TextColumn.
    /// </summary>
    /// <MetaDataID>{48E01AE5-9F1C-4DDF-87FE-39B755B683C8}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class TextColumn : Column
	{
		#region Constructor

        /// <summary>
        /// Creates a new TextColumn with default values
        /// </summary>
        /// <MetaDataID>{13DAB401-1A58-4E7E-8ED1-23EFEC8D5753}</MetaDataID>
		public TextColumn() : base()
		{
            Intit();

		}
        /// <MetaDataID>{0D9C40DA-4B31-493B-926C-4A75999B65B3}</MetaDataID>
        public TextColumn(Column copyColumn)
            : base(copyColumn)
        {
            Intit();

        }

        /// <MetaDataID>{EE4982F6-231C-477A-A35A-8548AD03F82D}</MetaDataID>
        public TextColumn(OOAdvantech.UserInterface.Column column)
            : base(column)
        {
            Intit();

        }



        /// <MetaDataID>{5C79AD60-6AD4-4D0B-9D20-CBDF49163622}</MetaDataID>
        private void Intit()
        {
            //_Type = ColumnType.TextColumn;
        }


        /// <summary>
        /// Creates a new TextColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{3B6A7B2A-89D4-4425-8A3D-01DD152DCD96}</MetaDataID>
		public TextColumn(string text) : base(text)
		{
            Intit();

		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{5515AD74-677D-45A5-A0F4-5A3F314F78A4}</MetaDataID>
		public TextColumn(string text, int width) : base(text, width)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{D3BD3F8F-8510-418B-9980-D71722454D37}</MetaDataID>
		public TextColumn(string text, int width, bool visible) : base(text, width, visible)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{DC5C9CDD-7B5C-4DD1-8141-B1A3ADD21F41}</MetaDataID>
		public TextColumn(string text, Image image) : base(text, image)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{6314BE91-7655-46F1-A37E-46242012C27F}</MetaDataID>
		public TextColumn(string text, Image image, int width) : base(text, image, width)
		{
            Intit();
		}


        /// <summary>
        /// Creates a new TextColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{35847857-803D-4E6A-B51F-E5C194A76445}</MetaDataID>
		public TextColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
            Intit();
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{2CBCE7B1-0448-40B2-9C6A-0F12338711AD}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "TEXT";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{75354A93-299B-495C-AC37-1FA7CCA71C42}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new TextCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{F1002597-C9F9-4A0E-A6B8-27F381B4883C}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return "TEXT";
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{1DE6A8D2-F105-415A-B1DC-6AACEF75EBDC}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return new TextCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the Type of the Comparer used to compare the Column's Cells when 
		/// the Column is sorting
		/// </summary>
		public override Type DefaultComparerType
		{
			get
			{
				return typeof(TextComparer);
			}
		}

		#endregion
	}
}
