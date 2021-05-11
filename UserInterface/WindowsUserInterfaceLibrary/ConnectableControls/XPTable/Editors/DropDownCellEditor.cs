/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 * 
 * DropDownCellEditor.ActivationListener, DropDownCellEditor.ShowDropDown() and 
 * DropDownCellEditor.HideDropDown() contains code based on Steve McMahon's 
 * PopupWindowHelper (see http://www.vbaccelerator.com/home/NET/Code/Controls/Popup_Windows/Popup_Windows/article.asp)
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
//using System.Windows.Forms;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Win32;


namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// A base class for editing Cells that contain drop down buttons
    /// </summary>
    /// <MetaDataID>{8AC704E5-14B4-484E-B0BA-ABFFA0C23C54}</MetaDataID>
	public abstract class DropDownCellEditor : CellEditor, IEditorUsesRendererButtons
	{
		#region Class Data

		/// <summary>
		/// The container that holds the Control displayed when editor is dropped down
		/// </summary>
		private DropDownContainer dropDownContainer;

		/// <summary>
		/// Specifies whether the DropDownContainer is currently displayed
		/// </summary>
		private bool droppedDown;

		/// <summary>
		/// Specifies the DropDown style
		/// </summary>
		private DropDownStyle dropDownStyle;

		/// <summary>
		/// The user defined width of the DropDownContainer
		/// </summary>
		private int dropDownWidth;

		/// <summary>
		/// Listener for WM_NCACTIVATE and WM_ACTIVATEAPP messages
		/// </summary>
		private ActivationListener activationListener;

		/// <summary>
		/// The Form that will own the DropDownContainer
		/// </summary>
        private System.Windows.Forms.Form parentForm;

		/// <summary>
		/// Specifies whether the mouse is currently over the 
		/// DropDownContainer
		/// </summary>
		private bool containsMouse;

		#endregion


		#region Constructor

        /// <summary>
        /// Initializes a new instance of the DropDownCellEditor class with default settings
        /// </summary>
        /// <MetaDataID>{66FE242C-5A74-4D5A-918E-1468C14C0855}</MetaDataID>
		public DropDownCellEditor() : base()
		{
			TextBox textbox = new TextBox();
			textbox.AutoSize = false;
			textbox.BackColor = SystemColors.Window;
            textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			textbox.MouseEnter += new EventHandler(textbox_MouseEnter);
			this.Control = textbox;

			this.dropDownContainer = new DropDownContainer(this);

			this.droppedDown = false;
			this.DropDownStyle = DropDownStyle.DropDownList;
			this.dropDownWidth = -1;

			this.parentForm = null;
			this.activationListener = new ActivationListener(this);
			this.containsMouse = false;
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
        /// <MetaDataID>{6C4E05AE-A70A-4C76-A4BF-14C0715F6B62}</MetaDataID>
		public override bool PrepareForEditing(Cell cell, ListView table, CellPos cellPos, Rectangle cellRect, bool userSetEditorValues)
		{
			if (!(table.ColumnModel.Columns[cellPos.Column] is DropDownColumn))
			{
				throw new InvalidOperationException("Cannot edit Cell as DropDownCellEditor can only be used with a DropDownColumn");
			}
			
			return base.PrepareForEditing (cell, table, cellPos, cellRect, userSetEditorValues);
		}


        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{6D897B57-8979-4CDB-83BE-B04932E8DE46}</MetaDataID>
		public override void StartEditing()
		{
            this.TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(OnKeyPress);
            this.TextBox.LostFocus += new EventHandler(OnLostFocus);
			
			base.StartEditing();

			this.parentForm = this.EditingTable.FindForm();

			if (this.DroppedDown)
			{
				this.ShowDropDown();
			}

			this.TextBox.Focus();
		}


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{1F31008B-F134-46A6-9DD1-73934327FE3C}</MetaDataID>
		public override void StopEditing()
		{
            this.TextBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(OnKeyPress);
            this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
			
			base.StopEditing();

			this.DroppedDown = false;

			this.parentForm = null;
		}


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{C7F63087-0FFC-4713-9785-C733331B89DC}</MetaDataID>
		public override void CancelEditing()
		{
            this.TextBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(OnKeyPress);
            this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
			
			base.CancelEditing();

			this.DroppedDown = false;

			this.parentForm = null;
		}


        /// <summary>
        /// Displays the drop down portion to the user
        /// </summary>
        /// <MetaDataID>{CB780DAA-BCE0-410E-8011-8AE7D900AB5B}</MetaDataID>
		protected virtual void ShowDropDown()
		{
			Point p = this.EditingTable.PointToScreen(this.TextBox.Location);
			p.Y += this.TextBox.Height + 1;

            Rectangle screenBounds = System.Windows.Forms.Screen.GetBounds(p);

			if (p.Y + this.dropDownContainer.Height > screenBounds.Bottom)
			{
				p.Y -= this.TextBox.Height + this.dropDownContainer.Height + 1;
			}

			if (p.X + this.dropDownContainer.Width > screenBounds.Right)
			{
				ICellRenderer renderer = this.EditingTable.ColumnModel.GetCellRenderer(this.EditingCellPos.Column);
				int buttonWidth = ((DropDownCellRenderer) renderer).ButtonWidth;
				
				p.X = p.X + this.TextBox.Width + buttonWidth - this.dropDownContainer.Width;
			}
			
			this.dropDownContainer.Location = p;

			this.parentForm.AddOwnedForm(this.dropDownContainer);
			this.activationListener.AssignHandle(this.parentForm.Handle);

			this.dropDownContainer.ShowDropDown();
			this.dropDownContainer.Activate();

			// A little bit of fun.  We've shown the popup,
			// but because we've kept the main window's
			// title bar in focus the tab sequence isn't quite
			// right.  This can be fixed by sending a tab,
			// but that on its own would shift focus to the
			// second control in the form.  So send a tab,
			// followed by a reverse-tab.

			// Send a Tab command:
            NativeMethods.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, 0, 0);
            NativeMethods.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, KeyEventFFlags.KEYEVENTF_KEYUP, 0);

			// Send a reverse Tab command:
            NativeMethods.keybd_event((byte)System.Windows.Forms.Keys.ShiftKey, 0, 0, 0);
            NativeMethods.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, 0, 0);
            NativeMethods.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, KeyEventFFlags.KEYEVENTF_KEYUP, 0);
            NativeMethods.keybd_event((byte)System.Windows.Forms.Keys.ShiftKey, 0, KeyEventFFlags.KEYEVENTF_KEYUP, 0);
		}


        /// <summary>
        /// Conceals the drop down portion from the user
        /// </summary>
        /// <MetaDataID>{D42FBCF8-EC8B-45F1-BA61-260078F3B237}</MetaDataID>
		protected virtual void HideDropDown()
		{
			this.dropDownContainer.HideDropDown();

			this.parentForm.RemoveOwnedForm(this.dropDownContainer);

			this.activationListener.ReleaseHandle();

			this.parentForm.Activate();
		}


        /// <summary>
        /// Gets whether the editor should stop editing if a mouse click occurs 
        /// outside of the DropDownContainer while it is dropped down
        /// </summary>
        /// <param name="target">The Control that will receive the message</param>
        /// <param name="cursorPos">The current position of the mouse cursor</param>
        /// <returns>true if the editor should stop editing, false otherwise</returns>
        /// <MetaDataID>{D875DD99-1F46-45E6-B045-13BBEA864E88}</MetaDataID>
        protected virtual bool ShouldStopEditing(System.Windows.Forms.Control target, Point cursorPos)
		{
			return true;
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
        /// <MetaDataID>{E4ACF6BE-2790-4A4F-B310-FA8A31316224}</MetaDataID>
        public override bool ProcessMouseMessage(System.Windows.Forms.Control target, WindowMessage msg, int wParam, int lParam)
		{
			if (this.DroppedDown)
			{
				if (msg == WindowMessage.WM_LBUTTONDOWN || msg == WindowMessage.WM_RBUTTONDOWN || 
					msg == WindowMessage.WM_MBUTTONDOWN || msg == WindowMessage.WM_XBUTTONDOWN || 
					msg == WindowMessage.WM_NCLBUTTONDOWN || msg == WindowMessage.WM_NCRBUTTONDOWN || 
					msg == WindowMessage.WM_NCMBUTTONDOWN || msg == WindowMessage.WM_NCXBUTTONDOWN)
				{
                    Point cursorPos = System.Windows.Forms.Cursor.Position;
					if (!this.DropDown.Bounds.Contains(cursorPos))
					{
						if (target != this.EditingTable && target != this.TextBox)
						{
							if (this.ShouldStopEditing(target, cursorPos))
							{
								this.EditingTable.StopEditing();
							}
						}
					}
				}
				else if (msg == WindowMessage.WM_MOUSEMOVE)
				{
                    Point cursorPos = System.Windows.Forms.Cursor.Position;
				
					if (this.DropDown.Bounds.Contains(cursorPos))
					{
						if (!this.containsMouse)
						{
							this.containsMouse = true;

							this.EditingTable.RaiseCellMouseLeave(this.EditingCellPos);
						}
					}
					else
					{
						this.containsMouse = true;
					}
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
        /// <MetaDataID>{D3E781F8-D253-4960-915B-BF9B4E22E41B}</MetaDataID>
        public override bool ProcessKeyMessage(System.Windows.Forms.Control target, WindowMessage msg, int wParam, int lParam)
		{
			if (msg == WindowMessage.WM_KEYDOWN)
			{
                if (((System.Windows.Forms.Keys)wParam) == System.Windows.Forms.Keys.F4)
				{
					if (this.TextBox.Focused || this.DropDown.ContainsFocus)
					{
						this.DroppedDown = !this.DroppedDown;

						return true;
					}
				}
			}

			return false;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the TextBox used to edit the Cells contents
		/// </summary>
		protected TextBox TextBox
		{
			get
			{
				return this.Control as TextBox;
			}
		}


		/// <summary>
		/// Gets the container that holds the Control displayed when editor is dropped down
		/// </summary>
		protected DropDownContainer DropDown
		{
			get
			{
				return this.dropDownContainer;
			}
		}


		/// <summary>
		/// Gets or sets whether the editor is displaying its drop-down portion
		/// </summary>
		public bool DroppedDown
		{
			get
			{
				return this.droppedDown;
			}

			set
			{
				if (this.droppedDown != value)
				{
					this.droppedDown = value;

					if (value)
					{
						this.ShowDropDown();
					}
					else
					{
						this.HideDropDown();
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets the width of the of the drop-down portion of the editor
		/// </summary>
		public int DropDownWidth
		{
			get
			{
				if (this.dropDownWidth != -1)
				{
					return this.dropDownWidth;
				}
				
				return this.dropDownContainer.Width;
			}

			set
			{
				this.dropDownWidth = value;				
				this.dropDownContainer.Width = value;
			}
		}


		/// <summary>
		/// Gets the user defined width of the of the drop-down portion of the editor
		/// </summary>
		internal int InternalDropDownWidth
		{
			get
			{
				return this.dropDownWidth;
			}
		}


		/// <summary>
		/// Gets or sets a value specifying the style of the drop down editor
		/// </summary>
		public DropDownStyle DropDownStyle
		{
			get
			{
				return this.dropDownStyle;
			}

			set
			{
				if (!Enum.IsDefined(typeof(DropDownStyle), value))
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(DropDownStyle));
				}
				
				if (this.dropDownStyle != value)
				{
					this.dropDownStyle = value;

					this.TextBox.ReadOnly = (value == DropDownStyle.DropDownList);
				}
			}
		}


		/// <summary>
		/// Gets or sets the text that is selected in the editable portion of the editor
		/// </summary>
		public string SelectedText
		{
			get
			{
				if (this.DropDownStyle == DropDownStyle.DropDownList)
				{
					return "";
				}

				return this.TextBox.SelectedText;
			}

			set
			{
				if (this.DropDownStyle != DropDownStyle.DropDownList && value != null)
				{
					this.TextBox.SelectedText = value;
				}
			}
		}


		/// <summary>
		/// Gets or sets the number of characters selected in the editable portion 
		/// of the editor
		/// </summary>
		public int SelectionLength
		{
			get
			{
				return this.TextBox.SelectionLength;
			}

			set
			{
				this.TextBox.SelectionLength = value;
			}
		}


		/// <summary>
		/// Gets or sets the starting index of text selected in the editor
		/// </summary>
		public int SelectionStart
		{
			get
			{
				return this.TextBox.SelectionStart;
			}

			set
			{
				this.TextBox.SelectionStart = value;
			}
		}


		/// <summary>
		/// Gets or sets the text associated with the editor
		/// </summary>
		public string Text
		{
			get
			{
				return this.TextBox.Text;
			}

			set
			{
				this.TextBox.Text = value;
			}
		}

		#endregion
		

		#region Events

        /// <summary>
        /// Handler for the editors TextBox.KeyPress event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data</param>
        /// <MetaDataID>{B791434E-6F3A-4001-9573-7C95A3E86CAD}</MetaDataID>
        protected virtual void OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == AsciiChars.CarriageReturn /*Enter*/)
			{
				if (this.EditingTable != null)
				{
					this.EditingTable.StopEditing();
				}
			}
			else if (e.KeyChar == AsciiChars.Escape)
			{
				if (this.EditingTable != null)
				{
					this.EditingTable.CancelEditing();
				}
			}
		}


        /// <summary>
        /// Handler for the editors TextBox.LostFocus event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{0CC97235-57CF-4E0D-BC95-61F55D4A152E}</MetaDataID>
		protected virtual void OnLostFocus(object sender, EventArgs e)
		{
			if (this.TextBox.Focused || this.DropDown.ContainsFocus)
			{
				return;
			}
			
			if (this.EditingTable != null)
			{
				this.EditingTable.StopEditing();
			}
		}


        /// <summary>
        /// Handler for the editors drop down button MouseDown event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{16EFFE38-25BD-4DCD-92F7-0270A316A17F}</MetaDataID>
		public virtual void OnEditorButtonMouseDown(object sender, CellMouseEventArgs e)
		{
			this.DroppedDown = !this.DroppedDown;
		}


        /// <summary>
        /// Handler for the editors drop down button MouseUp event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{745ED293-E837-4881-A729-A3695D9B1E9D}</MetaDataID>
		public virtual void OnEditorButtonMouseUp(object sender, CellMouseEventArgs e)
		{
			
		}


        /// <summary>
        /// Handler for the editors textbox MouseEnter event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{8132B365-F836-4F2E-B687-595AF6701F78}</MetaDataID>
		private void textbox_MouseEnter(object sender, EventArgs e)
		{
			this.EditingTable.RaiseCellMouseLeave(this.EditingCellPos);
		}

		#endregion


		#region ActivationListener

        /// <summary>
        /// Listener for WM_NCACTIVATE and WM_ACTIVATEAPP messages
        /// </summary>
        /// <MetaDataID>{DBA25974-FB02-47BA-92BD-249BA9EC73C2}</MetaDataID>
		internal class ActivationListener : List.Win32.NativeWindow
		{
			/// <summary>
			/// The DropDownCellEditor that owns the listener
			/// </summary>
			private DropDownCellEditor owner;


            /// <summary>
            /// Initializes a new instance of the DropDownCellEditor class with the 
            /// specified DropDownCellEditor owner
            /// </summary>
            /// <param name="owner">The DropDownCellEditor that owns the listener</param>
            /// <MetaDataID>{8A7B3192-54F0-490F-BF3F-40C88D58DA92}</MetaDataID>
			public ActivationListener(DropDownCellEditor owner) : base()
			{
				this.owner = owner;
			}


			/// <summary>
			/// Gets or sets the DropDownCellEditor that owns the listener
			/// </summary>
			public DropDownCellEditor Editor
			{
				get
				{
					return this.owner;
				}

				set
				{
					this.owner = value;
				}
			}


            /// <summary>
            /// Processes Windows messages
            /// </summary>
            /// <param name="m">The Windows Message to process</param>
            /// <MetaDataID>{86B06AFF-386E-40C7-BAA5-5244D59305CD}</MetaDataID>
            protected override void WndProc(ref System.Windows.Forms.Message m)
			{
				base.WndProc(ref m);
				
				if (this.owner != null && this.owner.DroppedDown)
				{
					if (m.Msg == (int) WindowMessage.WM_NCACTIVATE)
					{
						if (((int) m.WParam) == 0)
						{
							NativeMethods.SendMessage(this.Handle, (int) WindowMessage.WM_NCACTIVATE, 1, 0);
						}
					}
					else if (m.Msg == (int) WindowMessage.WM_ACTIVATEAPP)
					{
						if ((int)m.WParam == 0)
						{
							this.owner.DroppedDown = false;
							
							NativeMethods.PostMessage(this.Handle, (int) WindowMessage.WM_NCACTIVATE, 0, 0);
						}
					}
				}
			}
		}

		#endregion
	}
}
