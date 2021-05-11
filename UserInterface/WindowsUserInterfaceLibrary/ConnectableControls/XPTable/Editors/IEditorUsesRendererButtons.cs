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


namespace ConnectableControls.List.Editors
{
    /// <MetaDataID>{64BACCAD-B02C-4760-AA1E-10BD2D8F5322}</MetaDataID>
	public interface IEditorUsesRendererButtons
	{
        /// <summary>
        /// Raises the EditorButtonMouseDown event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{8BD625BB-48FC-472A-AA69-2D8177EFEF01}</MetaDataID>
		void OnEditorButtonMouseDown(object sender, CellMouseEventArgs e);


        /// <summary>
        /// Raises the EditorButtonMouseUp event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{8E34B4DD-4B3A-41BD-8210-7E83CF10BB5E}</MetaDataID>
		void OnEditorButtonMouseUp(object sender, CellMouseEventArgs e);
	}
}
