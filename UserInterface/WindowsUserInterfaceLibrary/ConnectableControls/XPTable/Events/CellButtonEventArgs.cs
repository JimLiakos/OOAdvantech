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

using ConnectableControls.List.Models;


namespace ConnectableControls.List.Events
{
	#region Delegates

	/// <summary>
	/// Represents the method that will handle the CellButtonClicked event of a Table
	/// </summary>
	public delegate void CellButtonEventHandler(object sender, CellButtonEventArgs e);

	#endregion



	#region CellButtonEventArgs

    /// <summary>
    /// Provides data for the CellButtonClicked event of a Table
    /// </summary>
    /// <MetaDataID>{A8AD3EB5-C21D-4D07-BB82-A668E14ABEE5}</MetaDataID>
	public class CellButtonEventArgs : CellEventArgsBase
	{
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the CellButtonEventArgs class with 
        /// the specified Cell source, row index and column index
        /// </summary>
        /// <param name="source">The Cell that raised the event</param>
        /// <param name="column">The Column index of the Cell</param>
        /// <param name="row">The Row index of the Cell</param>
        /// <MetaDataID>{0A84512E-01EF-49B2-A6A3-BA2FB9CAA2BF}</MetaDataID>
		public CellButtonEventArgs(Cell source, int column, int row) : base(source, column, column)
		{
			
		}

		#endregion
	}

	#endregion
}
