/*
 * Copyright � 2005, Mathew Hall
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


namespace ConnectableControls.List.Themes
{
    /// <summary>
    /// Represents the different parts of a ProgressBar that can be 
    /// drawn by the Windows XP theme engine
    /// </summary>
    /// <MetaDataID>{cf36854d-829f-4352-9128-3cfcdcedafab}</MetaDataID>
	public enum ProgressBarParts
	{
		/// <summary>
		/// The ProgressBar's background and border
		/// </summary>
		Bar = 1,
		
		/*/// <summary>
		/// 
		/// </summary>
		BarVert = 2,*/
		
		/// <summary>
		/// The ProgressBar's contents
		/// </summary>
		Chunk = 3
		
		/*/// <summary>
		/// 
		/// </summary>
		ChunkVert = 4*/
	}
}
