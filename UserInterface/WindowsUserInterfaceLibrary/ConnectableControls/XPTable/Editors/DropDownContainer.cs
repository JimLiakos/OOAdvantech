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
using System.Windows.Forms;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Win32;


namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// Summary description for DropDownContainer.
    /// </summary>
    /// <MetaDataID>{5480F51D-13B2-470A-9E11-855DDA2B9238}</MetaDataID>
	[ToolboxItem(false)]
	public class DropDownContainer : Form
	{
		#region Class Data

		/// <summary>
		/// The DropDownCellEditor that owns the DropDownContainer
		/// </summary>
		private DropDownCellEditor editor;

		/// <summary>
		/// The Control displayed in the DropDownContainer
		/// </summary>
		private Control dropdownControl;

		/// <summary>
		/// A Panel that provides the black border around the DropDownContainer
		/// </summary>
		private Panel panel;

		#endregion

		
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the DropDownContainer class with the 
        /// specified DropDownCellEditor owner
        /// </summary>
        /// <MetaDataID>{BE7FD937-C10C-41AE-A1DB-6A924BC1A8EC}</MetaDataID>
		public DropDownContainer(DropDownCellEditor editor) : base()
		{
			if (editor == null)
			{
				throw new ArgumentNullException("editor", "DropDownCellEditor cannot be null");
			}
			
			this.editor = editor;
			
			this.ControlBox = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.Manual;
			this.TabStop = false;
			this.TopMost = true;

			this.dropdownControl = null;

			this.panel = new Panel();
			this.panel.AutoScroll = false;
			this.panel.BorderStyle = BorderStyle.None;
			this.panel.Size = this.Size;
			this.Controls.Add(this.panel);
			this.SizeChanged += new EventHandler(DropDownContainer_SizeChanged);
		}

		#endregion


		#region Methods

        /// <summary>
        /// Displays the DropDownContainer to the user
        /// </summary>
        /// <MetaDataID>{41860AE7-E6F1-42E0-9302-46ECD4001E59}</MetaDataID>
		public void ShowDropDown()
		{
			this.FlushPaintMessages();

			this.Show();
		}


        /// <summary>
        /// Hides the DropDownContainer from the user
        /// </summary>
        /// <MetaDataID>{1070281A-2C64-4456-BFC9-56B7712A136E}</MetaDataID>
		public void HideDropDown()
		{
			this.FlushPaintMessages();

			this.Hide();
		}


        /// <summary>
        /// Processes any Paint messages in the message queue
        /// </summary>
        /// <MetaDataID>{4492B065-B53A-45CD-989E-BE9E651B9172}</MetaDataID>
		private void FlushPaintMessages()
		{
			MSG msg = new MSG();
			
			while (NativeMethods.PeekMessage(ref msg, IntPtr.Zero, (int) WindowMessage.WM_PAINT, (int) WindowMessage.WM_PAINT, 1 /*PM_REMOVE*/))
			{
				NativeMethods.TranslateMessage(ref msg);
				NativeMethods.DispatchMessage(ref msg);
			}
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the Control displayed in the DropDownContainer
		/// </summary>
		public Control Control
		{
			get
			{
				return this.dropdownControl;
			}

			set
			{
				if (value != this.dropdownControl)
				{
					this.panel.Controls.Clear();

					this.dropdownControl = value;

					if (value != null)
					{
						this.panel.Controls.Add(value);
					}
				}
			}
		}


		/// <summary>
		/// Gets the required creation parameters when the control handle is created
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cparams = base.CreateParams;

				cparams.ExStyle |= (int) WindowExtendedStyles.WS_EX_TOOLWINDOW;

				if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major > 5)
				{
					cparams.ExStyle |= (int) WindowExtendedStyles.WS_EX_NOACTIVATE;
				}

				cparams.ClassStyle |= 0x800 /*CS_SAVEBITS*/;
				
				return cparams;
			}
		}


        /// <summary>
        /// Handler for the DropDownContainer's SizeChanged event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{4AAE34AD-AC2F-4EAC-B89A-C9A513109FDD}</MetaDataID>
		private void DropDownContainer_SizeChanged(object sender, EventArgs e)
		{
			this.panel.Size = this.Size;
		}

		#endregion
	}
}
