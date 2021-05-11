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
using System.Windows.Forms;

using ConnectableControls.List.Events;


namespace ConnectableControls.List.Renderers
{
    /// <MetaDataID>{36A3A7AC-513D-4D1E-A43F-D0567F579723}</MetaDataID>
	public interface ICellRenderer : IRenderer
	{
        /// <summary>
        /// Raises the PaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{FC2E1480-939F-4550-8A73-DBB8AC619129}</MetaDataID>
		void OnPaintCell(PaintCellEventArgs e);


        /// <summary>
        /// Raises the GotFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{95037A05-7846-4930-8B68-E962AF20863F}</MetaDataID>
		void OnGotFocus(CellFocusEventArgs e);


        /// <summary>
        /// Raises the LostFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{50565B39-F17B-4E59-B493-1842FF60B057}</MetaDataID>
		void OnLostFocus(CellFocusEventArgs e);


        /// <summary>
        /// Raises the KeyDown event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{E8010EC7-F3D7-4201-887D-5FD31F7CD9C7}</MetaDataID>
		void OnKeyDown(CellKeyEventArgs e);


        /// <summary>
        /// Raises the KeyUp event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{A755B788-7C47-416F-A3D0-985D8A2C05AC}</MetaDataID>
		void OnKeyUp(CellKeyEventArgs e);


        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{5C6396D4-28A2-4B01-B4C9-71BCF7D647C7}</MetaDataID>
		void OnMouseEnter(CellMouseEventArgs e);


        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2D363B1E-A114-45FD-89BE-2FE23C99D038}</MetaDataID>
		void OnMouseLeave(CellMouseEventArgs e);


        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{BC46CD27-4439-4BF4-B212-BFE46014410F}</MetaDataID>
		void OnMouseUp(CellMouseEventArgs e);


        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{EB3EAC3C-4E72-45F8-8D78-0F162E84850D}</MetaDataID>
		void OnMouseDown(CellMouseEventArgs e);


        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{7DA20E32-9F7A-4BFF-ABA2-50BB637F45C3}</MetaDataID>
		void OnMouseMove(CellMouseEventArgs e);


        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{19302A39-C40C-45C2-BB3C-39E061456C09}</MetaDataID>
		void OnClick(CellMouseEventArgs e);


        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{4B3D5F5E-5726-420E-AA82-96583C89B1F1}</MetaDataID>
		void OnDoubleClick(CellMouseEventArgs e);
	}
}
