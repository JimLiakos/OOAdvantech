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

using ConnectableControls.List.Models;


namespace ConnectableControls.List.Renderers
{
    /// <MetaDataID>{CDC83F2D-9A5E-4387-A6B5-AC7C43C239D6}</MetaDataID>
	public interface IRenderer
	{
        /// <summary>
        /// Gets a Rectangle that represents the client area of the object 
        /// being rendered
        /// </summary>
        /// <MetaDataID>{9512f5d5-3620-4274-aa69-40a5edb5e655}</MetaDataID>
		Rectangle ClientRectangle
		{
			get;
		}


        /// <summary>
        /// Gets or sets a Rectangle that represents the size and location 
        /// of the object being rendered
        /// </summary>
        /// <MetaDataID>{c5be6c10-5363-4fbf-b819-6d6bd91adc13}</MetaDataID>
		Rectangle Bounds
		{
			get;
			set;
		}


        /// <summary>
        /// Gets or sets the font of the text displayed by the object being 
        /// rendered
        /// </summary>
        /// <MetaDataID>{f9141482-3dd1-4612-b619-431a024033c6}</MetaDataID>
		Font Font
		{
			get;
			set;
		}


        /// <summary>
        /// Gets or sets the foreground color of the object being rendered
        /// </summary>
        /// <MetaDataID>{b894864b-1a91-4703-9090-6e084746a5cd}</MetaDataID>
		Color ForeColor
		{
			get;
			set;
		}


        /// <summary>
        /// Gets or sets the background color for the object being rendered
        /// </summary>
        /// <MetaDataID>{b28ea3d7-ed8a-44bb-9f88-e774da7df5db}</MetaDataID>
		Color BackColor
		{
			get;
			set;
		}


        /// <summary>
        /// Gets or sets how the Renderers contents are aligned horizontally
        /// </summary>
        /// <MetaDataID>{aafd86ea-66f5-4ac3-b7ee-e5b0579d5f02}</MetaDataID>
		ColumnAlignment Alignment
		{
			get;
			set;
		}


        /// <summary>
        /// Gets or sets how the Renderers contents are aligned vertically
        /// </summary>
        /// <MetaDataID>{ac8f1c50-e6a6-4d8a-8c3a-812bc8318e5f}</MetaDataID>
		RowAlignment LineAlignment
		{
			get;
			set;
		}
	}
}
