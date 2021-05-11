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

using ConnectableControls.List.Events;


namespace ConnectableControls.List.Renderers
{
    /// <MetaDataID>{EA03E34B-9438-4364-9E23-7802CC5F4006}</MetaDataID>
	public interface IHeaderRenderer : IRenderer
	{
        /// <summary>
        /// Raises the PaintHeader event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{1EB7360C-C7F5-4BC9-9297-D35A2E66DB46}</MetaDataID>
		void OnPaintHeader(PaintHeaderEventArgs e);


        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{6BB9ECAD-699D-4017-AAEF-D15FDAF88892}</MetaDataID>
		void OnMouseEnter(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{BFE34151-8CF8-46DB-B7B5-2BFF55F46E47}</MetaDataID>
		void OnMouseLeave(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{6D9A9E60-E48B-4B0A-851A-82F98C6F7EF5}</MetaDataID>
		void OnMouseUp(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{50AB71BA-4974-43FB-82DF-BB2774CD4AB4}</MetaDataID>
		void OnMouseDown(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{953FFA77-A517-43A4-99A7-4DD843BBA51C}</MetaDataID>
		void OnMouseMove(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2DBE586A-2B27-44E3-9322-EBB61749A475}</MetaDataID>
		void OnClick(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{B921DC9C-97AC-43E8-936A-97BD626AC335}</MetaDataID>
		void OnDoubleClick(HeaderMouseEventArgs e);
	}
}
