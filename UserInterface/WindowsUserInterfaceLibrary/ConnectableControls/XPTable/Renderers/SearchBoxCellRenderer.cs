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
using System.Globalization;
using System.Windows.Forms;

using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Models;


namespace ConnectableControls.List.Renderers
{
    /// <summary>
    /// A CellRenderer that draws Cell contents as strings
    /// </summary>
    /// <MetaDataID>{B9A5B874-835C-4455-8A8A-02752A8FCFA6}</MetaDataID>
	public class SearchBoxCellRenderer : CellRenderer
	{
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the TextCellRenderer class with 
        /// default settings
        /// </summary>
        /// <MetaDataID>{4996BDD2-8A69-4087-824E-E02360DE5270}</MetaDataID>
        public SearchBoxCellRenderer()
            : base()
		{
			
		}

		#endregion


		#region Events

		#region Paint

        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{50D477FB-E107-4133-B4B1-C0D4598249AA}</MetaDataID>
		protected override void OnPaint(PaintCellEventArgs e)
		{
			base.OnPaint(e);
			
			// don't bother going any further if the Cell is null 
			if (e.Cell == null)
			{
				return;
			}

			string text = e.Cell.Text;

			if (text != null && text.Length != 0)
			{
				if (e.Enabled)
				{
					e.Graphics.DrawString(text, this.Font, this.ForeBrush, this.ClientRectangle, this.StringFormat);
				}
				else
				{
					e.Graphics.DrawString(text, this.Font, this.GrayTextBrush, this.ClientRectangle, this.StringFormat);
				}
			}
			
			if (e.Focused && e.Enabled)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
			}
		}

		#endregion

		#endregion
	}
}
