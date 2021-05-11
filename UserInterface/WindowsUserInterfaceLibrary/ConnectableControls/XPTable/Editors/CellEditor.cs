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
//using System.Windows.Forms;
using System.Windows.Forms.Design;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Win32;


namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// Base class for Cell editors
    /// </summary>
    /// <MetaDataID>{D17823CF-DBE2-4FA8-8D8F-22A12DA11981}</MetaDataID>
	public abstract class CellEditor : ICellEditor, IMouseMessageFilterClient, IKeyMessageFilterClient
	{
		#region Event Handlers

		/// <summary>
		/// Occurs when the CellEditor begins editing a Cell
		/// </summary>
		public event CellEditEventHandler BeginEdit;

		/// <summary>
		/// Occurs when the CellEditor stops editing a Cell
		/// </summary>
		public event CellEditEventHandler EndEdit;

		/// <summary>
		/// Occurs when the editing of a Cell is cancelled
		/// </summary>
		public event CellEditEventHandler CancelEdit;

		#endregion


		#region Class Data

		/// <summary>
		/// The Control that is performing the editing
		/// </summary>
        private System.Windows.Forms.Control control;

		/// <summary>
		/// The Cell that is being edited
		/// </summary>
		internal Cell cell;

		/// <summary>
		/// The Table that contains the Cell being edited
		/// </summary>
		private ListView table;

		/// <summary>
		/// A CellPos that represents the position of the Cell being edited
		/// </summary>
		private CellPos cellPos;

		/// <summary>
		/// The Rectangle that represents the Cells location and size
		/// </summary>
		private Rectangle cellRect;

		/// <summary>
		/// A MouseMessageFilter that receives mouse messages before they 
		/// are dispatched to their destination
		/// </summary>
		private MouseMessageFilter mouseMessageFilter;

		/// <summary>
		/// A KeyMessageFilter that receives key messages before they 
		/// are dispatched to their destination
		/// </summary>
		private KeyMessageFilter keyMessageFilter;

		#endregion
		
		
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the CellEditor class with default settings
        /// </summary>
        /// <MetaDataID>{07962A25-4BB0-4C78-AE1B-F3FA6C8F2934}</MetaDataID>
		protected CellEditor()
		{
			this.control = null;
			this.cell = null;
			this.table = null;
			this.cellPos = CellPos.Empty;
			this.cellRect = Rectangle.Empty;

			this.mouseMessageFilter = new MouseMessageFilter(this);
			this.keyMessageFilter = new KeyMessageFilter(this);
		}

		#endregion


		#region Methods

        /// <summary>
        /// Prepares the CellEditor to edit the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to be edited</param>
        /// <param name="table">The Table that contains the Cell</param>
        /// <param name="cellPos">A CellPos representing the position of the Cell</param>
        /// <param name="cellRect">The Rectangle that represents the Cells location and size</param>
        /// <param name="userSetEditorValues">Specifies whether the ICellEditors 
        /// starting value has already been set by the user</param>
        /// <returns>true if the ICellEditor can continue editing the Cell, false otherwise</returns>
        /// <MetaDataID>{05BEE38B-61CA-4EF0-8376-3CCF69798EB5}</MetaDataID>
		public virtual bool PrepareForEditing(Cell cell, ListView table, CellPos cellPos, Rectangle cellRect, bool userSetEditorValues)
		{
			//
			this.cell = cell;
			this.table = table;
			this.cellPos = cellPos;
			this.cellRect = cellRect;

			// check if the user has already set the editors value for us
			if (!userSetEditorValues)
			{
				this.SetEditValue();
			}

			this.SetEditLocation(cellRect);

			// raise the BeginEdit event
			CellEditEventArgs e = new CellEditEventArgs(cell, this, table, cellPos.Row, cellPos.Column, cellRect);
			e.Handled = userSetEditorValues;

			this.OnBeginEdit(e);
			
			// if the edit has been canceled, remove the editor and return false
			if (e.Cancel)
			{
				this.RemoveEditControl();

				return false;
			}

			return true;
		}


        /// <summary>
        /// Sets the location and size of the CellEditor
        /// </summary>
        /// <param name="cellRect">A Rectangle that represents the size and location 
        /// of the Cell being edited</param>
        /// <MetaDataID>{B1F21A8F-D2F8-40E8-B604-C49F0500B265}</MetaDataID>
		protected abstract void SetEditLocation(Rectangle cellRect);


        /// <summary>
        /// Sets the initial value of the editor based on the contents of 
        /// the Cell being edited
        /// </summary>
        /// <MetaDataID>{01549768-2AD0-4FCF-A8F9-844C1B8EA03E}</MetaDataID>
		protected abstract void SetEditValue();


        /// <summary>
        /// Sets the contents of the Cell being edited based on the value 
        /// in the editor
        /// </summary>
        /// <MetaDataID>{FC6A5699-41DB-48B2-93F4-98D90F7D1452}</MetaDataID>
		protected abstract void SetCellValue();


        /// <summary>
        /// Displays the editor to the user and adds it to the Table's Control
        /// collection
        /// </summary>
        /// <MetaDataID>{26823354-0F24-4525-AD94-CAE4E4C3159E}</MetaDataID>
		protected virtual void ShowEditControl()
		{
			this.control.Parent = this.table;

			this.control.Visible = true;
		}


        /// <summary>
        /// Conceals the editor from the user, but does not remove it from the 
        /// Table's Control collection
        /// </summary>
        /// <MetaDataID>{5252900D-F0E0-4220-B466-13992088A826}</MetaDataID>
		protected virtual void HideEditControl()
		{
			this.control.Visible = false;
		}


        /// <summary>
        /// Conceals the editor from the user and removes it from the Table's 
        /// Control collection
        /// </summary>
        /// <MetaDataID>{434D05D2-E057-4337-A583-5B3459EC5629}</MetaDataID>
        protected virtual void RemoveEditControl()
        {

            this.control.Visible = false;
            this.control.Parent = null;

            this.table.Focus();

            this.cell = null;
            this.table = null;
            this.cellPos = CellPos.Empty;
            this.cellRect = Rectangle.Empty;

        }


        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{2EC0A3CE-7A03-44CB-83EF-047C0B8AF6F8}</MetaDataID>
		public virtual void StartEditing()
		{
            this.ShowEditControl();
            System.Windows.Forms.Application.AddMessageFilter(this.keyMessageFilter);
            System.Windows.Forms.Application.AddMessageFilter(this.mouseMessageFilter);
		}


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{AF55242B-7020-4FDE-B37D-644AC404E357}</MetaDataID>
		public virtual void StopEditing()
		{
            System.Windows.Forms.Application.RemoveMessageFilter(this.keyMessageFilter);
            System.Windows.Forms.Application.RemoveMessageFilter(this.mouseMessageFilter);
			
			//
			CellEditEventArgs e = new CellEditEventArgs(this.cell, this, this.table, this.cellPos.Row, this.cellPos.Column, this.cellRect);

			this.table.OnEditingStopped(e);
			this.OnEndEdit(e);
			
			if (!e.Cancel && !e.Handled)
			{
				this.SetCellValue();
			}

			this.RemoveEditControl();
		}


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{647ABB8C-66F6-4CF3-BD24-48B44BFA612E}</MetaDataID>
		public virtual void CancelEditing()
		{
            System.Windows.Forms.Application.RemoveMessageFilter(this.keyMessageFilter);
            System.Windows.Forms.Application.RemoveMessageFilter(this.mouseMessageFilter);
			
			//
			CellEditEventArgs e = new CellEditEventArgs(this.cell, this, this.table, this.cellPos.Row, this.cellPos.Column, this.cellRect);

			this.table.OnEditingCancelled(e);
			this.OnCancelEdit(e);
			
			this.RemoveEditControl();
		}


        /// <summary>
        /// Filters out a mouse message before it is dispatched
        /// </summary>
        /// <param name="target">The Control that will receive the message</param>
        /// <param name="msg">A WindowMessage that represents the message to process</param>
        /// <param name="wParam">Specifies the WParam field of the message</param>
        /// <param name="lParam">Specifies the LParam field of the message</param>
        /// <returns>true to filter the message and prevent it from being dispatched; 
        /// false to allow the message to continue to the next filter or control</returns>
        /// <MetaDataID>{88B493A6-2C14-46D2-8055-2706223E6758}</MetaDataID>
        public virtual bool ProcessMouseMessage(System.Windows.Forms.Control target, WindowMessage msg, int wParam, int lParam)
		{
			if (msg == WindowMessage.WM_LBUTTONDOWN || msg == WindowMessage.WM_RBUTTONDOWN || 
				msg == WindowMessage.WM_MBUTTONDOWN || msg == WindowMessage.WM_XBUTTONDOWN || 
				msg == WindowMessage.WM_NCLBUTTONDOWN || msg == WindowMessage.WM_NCRBUTTONDOWN || 
				msg == WindowMessage.WM_NCMBUTTONDOWN || msg == WindowMessage.WM_NCXBUTTONDOWN)
			{
                Point cursorPos = System.Windows.Forms.Cursor.Position;
                if (target == null)
                    return false;
				
				if (target != this.EditingTable && target != this.Control)
				{
					this.EditingTable.StopEditing();
				}
			}
			
			return false;
		}


        /// <summary>
        /// Filters out a key message before it is dispatched
        /// </summary>
        /// <param name="target">The Control that will receive the message</param>
        /// <param name="msg">A WindowMessage that represents the message to process</param>
        /// <param name="wParam">Specifies the WParam field of the message</param>
        /// <param name="lParam">Specifies the LParam field of the message</param>
        /// <returns>true to filter the message and prevent it from being dispatched; 
        /// false to allow the message to continue to the next filter or control</returns>
        /// <MetaDataID>{3E8EE901-C1E0-4701-A1EB-6734368F8735}</MetaDataID>
        public virtual bool ProcessKeyMessage(System.Windows.Forms.Control target, WindowMessage msg, int wParam, int lParam)
		{
			return false;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the Control that is being used to edit the Cell
		/// </summary>
        protected System.Windows.Forms.Control Control
		{
			get
			{
				return this.control;
			}

			set
			{
				this.control = value;
			}
		}


		/// <summary>
		/// Gets the Cell that is being edited
		/// </summary>
		public Cell EditingCell
		{
			get
			{
				return this.cell;
			}
		}


		/// <summary>
		/// Gets the Table that contains the Cell being edited
		/// </summary>
		public ListView EditingTable
		{
			get
			{
				return this.table;
			}
		}


		/// <summary>
		/// Gets a CellPos that represents the position of the Cell being edited
		/// </summary>
		public CellPos EditingCellPos
		{
			get
			{
				return this.cellPos;
			}
		}


		/// <summary>
		/// Gets whether the CellEditor is currently editing a Cell
		/// </summary>
		public bool IsEditing
		{
			get
			{
				return this.cell != null;
			}
		}

		#endregion


		#region Events

        /// <summary>
        /// Raises the BeginEdit event
        /// </summary>
        /// <param name="e">A CellEditEventArgs that contains the event data</param>
        /// <MetaDataID>{3BBBFB79-B367-482F-8941-3A1BF60071B7}</MetaDataID>
		protected virtual void OnBeginEdit(CellEditEventArgs e)
		{
			if (this.BeginEdit != null)
			{
				this.BeginEdit(this, e);
			}
		}


        /// <summary>
        /// Raises the EndEdit event
        /// </summary>
        /// <param name="e">A CellEditEventArgs that contains the event data</param>
        /// <MetaDataID>{C817926F-5684-4462-955A-CEE690A2112E}</MetaDataID>
		protected virtual void OnEndEdit(CellEditEventArgs e)
		{
			if (this.EndEdit != null)
			{
				this.EndEdit(this, e);
			}
		}


        /// <summary>
        /// Raises the CancelEdit event
        /// </summary>
        /// <param name="e">A CellEditEventArgs that contains the event data</param>
        /// <MetaDataID>{DA32804F-8E2E-4F92-8EAD-65EFA7B85DA1}</MetaDataID>
		protected virtual void OnCancelEdit(CellEditEventArgs e)
		{
			if (this.CancelEdit != null)
			{
				this.CancelEdit(this, e);
			}
		}

		#endregion
	}
}
