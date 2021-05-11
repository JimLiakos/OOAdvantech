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
using System.Globalization;
//using System.Windows.Forms;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Win32;


namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// A class for editing Cells that contain numbers
    /// </summary>
    /// <MetaDataID>{B87EE33F-AB1E-4A33-9744-C3DD85DBFD2A}</MetaDataID>
	public class NumberCellEditor : CellEditor, IEditorUsesRendererButtons
	{
		#region Class Data

		/// <summary>
		/// ID number for the up button
		/// </summary>
		protected static readonly int UpButtonID = 1;

		/// <summary>
		/// ID number for the down button
		/// </summary>
		protected static readonly int DownButtonID = 2;

		/// <summary>
		/// The current value of the editor
		/// </summary>
		private decimal currentValue;

		/// <summary>
		/// The value to increment or decrement when the up or down buttons are clicked
		/// </summary>
		private decimal increment;

		/// <summary>
		/// The maximum value for the editor
		/// </summary>
		private decimal maximum;

		/// <summary>
		/// The inximum value for the editor
		/// </summary>
		private decimal minimum;

		/// <summary>
		/// A string that specifies how editors value is formatted
		/// </summary>
		private string format;

		/// <summary>
		/// The amount the mouse wheel has moved
		/// </summary>
		private int wheelDelta;

		/// <summary>
		/// Indicates whether the arrow keys should be passed to the editor
		/// </summary>
		private bool interceptArrowKeys;

		/// <summary>
		/// Specifies whether the editors text value is changing
		/// </summary>
		private bool changingText;

		/// <summary>
		/// Initial interval between timer events
		/// </summary>
		private const int TimerInterval = 500;

		/// <summary>
		/// Current interval between timer events
		/// </summary>
		private int interval;

		/// <summary>
		/// Indicates whether the user has changed the editors value
		/// </summary>
		private bool userEdit;

		/// <summary>
		/// The bounding Rectangle of the up and down buttons
		/// </summary>
		private Rectangle buttonBounds;

		/// <summary>
		/// The id of the button that was pressed
		/// </summary>
		private int buttonID;

		/// <summary>
		/// Timer to to fire button presses at regular intervals while 
		/// a button is pressed
		/// </summary>
        private System.Windows.Forms.Timer timer;

		#endregion
		

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the NumberCellEditor class with default settings
        /// </summary>
        /// <MetaDataID>{DC63D967-5A17-4F66-82D5-264CC1F29E42}</MetaDataID>
		public NumberCellEditor()
		{
			TextBox textbox = new TextBox();
			textbox.AutoSize = false;
            textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Control = textbox;

			this.currentValue = new decimal(0);
			this.increment = new decimal(1);
			this.minimum = new decimal(0);
			this.maximum = new decimal(100);
			this.format = "G";

			this.wheelDelta = 0;
			this.interceptArrowKeys = true;
			this.userEdit = false;
			this.changingText = false;
			this.buttonBounds = Rectangle.Empty;
			this.buttonID = 0;
			this.interval = TimerInterval;
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
        /// <MetaDataID>{22B415B3-D96C-4465-BE55-708F763BD5D8}</MetaDataID>
		public override bool PrepareForEditing(Cell cell, ListView table, CellPos cellPos, Rectangle cellRect, bool userSetEditorValues)
		{
			//
			if (!(table.ColumnModel.Columns[cellPos.Column] is NumberColumn))
			{
				throw new InvalidOperationException("Cannot edit Cell as NumberCellEditor can only be used with a NumberColumn");
			}
			
			if (!(table.ColumnModel.GetCellRenderer(cellPos.Column) is NumberCellRenderer))
			{
				throw new InvalidOperationException("Cannot edit Cell as NumberCellEditor can only be used with a NumberColumn that uses a NumberCellRenderer");
			}
			
			this.Minimum = ((NumberColumn) table.ColumnModel.Columns[cellPos.Column]).Minimum;
			this.Maximum = ((NumberColumn) table.ColumnModel.Columns[cellPos.Column]).Maximum;
			this.Increment = ((NumberColumn) table.ColumnModel.Columns[cellPos.Column]).Increment;
			
			return base.PrepareForEditing (cell, table, cellPos, cellRect, userSetEditorValues);
		}


        /// <summary>
        /// Sets the initial value of the editor based on the contents of 
        /// the Cell being edited
        /// </summary>
        /// <MetaDataID>{6483E612-17D7-4007-A37F-516211BBA1E2}</MetaDataID>
		protected override void SetEditValue()
		{
			// make sure we start with a valid value
			this.Value = this.Minimum;

			// attempt to get the cells data
			this.Value = Convert.ToDecimal(this.EditingCell.Data);
		}


        /// <summary>
        /// Sets the contents of the Cell being edited based on the value 
        /// in the editor
        /// </summary>
        /// <MetaDataID>{15A6441F-CD09-4B07-9A9D-64037D6BF8B2}</MetaDataID>
		protected override void SetCellValue()
		{
			this.EditingCell.Data = this.Value;
		}


        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{BBEA8F71-9AC7-449F-969A-9FEF951B0365}</MetaDataID>
		public override void StartEditing()
		{
            this.TextBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(OnMouseWheel);
            this.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(OnTextBoxKeyDown);
            this.TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(OnTextBoxKeyPress);
			this.TextBox.LostFocus += new EventHandler(OnTextBoxLostFocus);
			
			base.StartEditing();

			this.TextBox.Focus();
		}


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{6501CE3C-8006-491B-8A09-DE5656CFB4FF}</MetaDataID>
		public override void StopEditing()
		{
            this.TextBox.MouseWheel -= new System.Windows.Forms.MouseEventHandler(OnMouseWheel);
            this.TextBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnTextBoxKeyDown);
            this.TextBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(OnTextBoxKeyPress);
            this.TextBox.LostFocus -= new EventHandler(OnTextBoxLostFocus);
			
			base.StopEditing();
		}


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{FD7D3CC8-FDE4-4297-A09B-8E54C1396EC0}</MetaDataID>
		public override void CancelEditing()
		{
            this.TextBox.MouseWheel -= new System.Windows.Forms.MouseEventHandler(OnMouseWheel);
            this.TextBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnTextBoxKeyDown);
            this.TextBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(OnTextBoxKeyPress);
			this.TextBox.LostFocus -= new EventHandler(OnTextBoxLostFocus);
			
			base.CancelEditing();
		}


        /// <summary>
        /// Sets the location and size of the CellEditor
        /// </summary>
        /// <param name="cellRect">A Rectangle that represents the size and location 
        /// of the Cell being edited</param>
        /// <MetaDataID>{8C90758E-E488-46BA-8A49-44FD37A71FDF}</MetaDataID>
		protected override void SetEditLocation(Rectangle cellRect)
		{
			// calc the size of the textbox
			ICellRenderer renderer = this.EditingTable.ColumnModel.GetCellRenderer(this.EditingCellPos.Column);
			int buttonWidth = ((NumberCellRenderer) renderer).ButtonWidth;

			this.TextBox.Size = new Size(cellRect.Width - 1 - buttonWidth, cellRect.Height-1);
			
			// calc the location of the textbox
			this.TextBox.Location = cellRect.Location;
			this.buttonBounds = new Rectangle(this.TextBox.Left + 1, this.TextBox.Top, buttonWidth, this.TextBox.Height);

            if (((NumberColumn)this.EditingTable.ColumnModel.Columns[this.EditingCellPos.Column]).UpDownAlign == System.Windows.Forms.LeftRightAlignment.Left)
			{
				this.TextBox.Location = new Point(cellRect.Left + buttonWidth, cellRect.Top);
				this.buttonBounds.Location = new Point(cellRect.Left, cellRect.Top);
			}
		}


        /// <summary>
        /// Simulates the up button being pressed
        /// </summary>
        /// <MetaDataID>{AFBC04EA-785B-4DDD-8F6D-2D6B27C512ED}</MetaDataID>
		protected void UpButton()
		{
			if (this.UserEdit)
			{
				this.ParseEditText();
			}

			decimal num = this.currentValue;

			if (num > (new decimal(-1, -1, -1, false, 0) - this.increment))
			{
				num = new decimal(-1, -1, -1, false, 0);
			}
			else
			{
				num += this.increment;

				if (num > this.maximum)
				{
					num = this.maximum;
				}
			}

			this.Value = num;
		}


        /// <summary>
        /// Simulates the down button being pressed
        /// </summary>
        /// <MetaDataID>{AA2B8190-8820-456C-9739-1458053CDD70}</MetaDataID>
		protected void DownButton()
		{
			if (this.UserEdit)
			{
				this.ParseEditText();
			}

			decimal num = this.currentValue;

			if (num < (new decimal(-1, -1, -1, true, 0) + this.increment))
			{
				num = new decimal(-1, -1, -1, true, 0);
			}
			else
			{
				num -= this.increment;

				if (num < this.minimum)
				{
					num = this.minimum;
				}
			}

			this.Value = num;
		}


        /// <summary>
        /// Updates the editors text value to the current value
        /// </summary>
        /// <MetaDataID>{E2EF3CAD-22FE-4946-996D-1A8CC8A73E02}</MetaDataID>
		protected void UpdateEditText()
		{
			if (this.UserEdit)
			{
				this.ParseEditText();
			}

			this.ChangingText = true;

			this.Control.Text = this.currentValue.ToString(this.Format);
		}


        /// <summary>
        /// Checks the current value and updates the editors text value
        /// </summary>
        /// <MetaDataID>{6A978CF3-251C-40C8-9EBE-603B6FECF546}</MetaDataID>
		protected virtual void ValidateEditText()
		{
			this.ParseEditText();
			this.UpdateEditText();
		}


        /// <summary>
        /// Converts the editors current value to a number
        /// </summary>
        /// <MetaDataID>{45CC9C1A-6FCB-4EE7-A7D8-420393D4CB26}</MetaDataID>
		protected void ParseEditText()
		{
			try
			{
				this.Value = this.Constrain(decimal.Parse(this.Control.Text));
			}
			catch (Exception)
			{
				return;
			}
			finally
			{
				this.UserEdit = false;
			}
		}


        /// <summary>
        /// Ensures that the specified value is between the editors Maximun and 
        /// Minimum values
        /// </summary>
        /// <param name="value">The value to be checked</param>
        /// <returns>A value is between the editors Maximun and Minimum values</returns>
        /// <MetaDataID>{5CE4D380-D967-433D-A67F-975206CCE6A2}</MetaDataID>
		private decimal Constrain(decimal value)
		{
			if (value < this.minimum)
			{
				value = this.minimum;
			}

			if (value > this.maximum)
			{
				value = this.maximum;
			}

			return value;
		}


        /// <summary>
        /// Starts the Timer
        /// </summary>
        /// <MetaDataID>{00DFD981-4A03-4E42-9B9D-D1B1A734A93F}</MetaDataID>
		protected void StartTimer()
		{
			if (this.timer == null)
			{
                this.timer = new System.Windows.Forms.Timer();
				this.timer.Tick += new EventHandler(this.TimerHandler);
			}

			this.interval = TimerInterval;
			this.timer.Interval = this.interval;
			this.timer.Start();
		}


        /// <summary>
        /// Stops the Timer
        /// </summary>
        /// <MetaDataID>{12919531-3AB4-478A-A5DD-AAC3265C617A}</MetaDataID>
		protected void StopTimer()
		{
			if (this.timer != null)
			{
				this.timer.Stop();
				this.timer.Dispose();
				this.timer = null;
			}
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the TextBox used to edit the Cells contents
		/// </summary>
		public TextBox TextBox
		{
			get
			{
				return this.Control as TextBox;
			}
		}


		/// <summary>
		/// Gets or sets the editors current value
		/// </summary>
		protected decimal Value
		{
			get
			{
				if (this.UserEdit)
				{
					this.ValidateEditText();
				}

				return this.currentValue;
			}

			set
			{
				if (value != this.currentValue)
				{
					if (value < this.minimum)
					{
						value = this.maximum;
					}

					if (value > this.maximum)
					{
						value = this.maximum;
					}

					this.currentValue = value;

					this.UpdateEditText();
				}
			}
		}

		/// <summary>
		/// Gets or sets the value to increment or decrement when the up or down 
		/// buttons are clicked
		/// </summary>
		protected decimal Increment
		{
			get
			{
				return this.increment;
			}

			set
			{
				if (value < new decimal(0))
				{
					throw new ArgumentException("increment must be greater than zero");
				}

				this.increment = value;
			}
		}


		/// <summary>
		/// Gets or sets the maximum value for the editor
		/// </summary>
		protected decimal Maximum
		{
			get
			{
				return this.maximum;
			}

			set
			{
				this.maximum = value;
				
				if (this.minimum > this.maximum)
				{
					this.minimum = this.maximum;
				}
			}
		}


		/// <summary>
		/// Gets or sets the minimum value for the editor
		/// </summary>
		protected decimal Minimum
		{
			get
			{
				return this.minimum;
			}

			set
			{
				this.minimum = value;

				if (this.minimum > this.maximum)
				{
					this.maximum = value;
				}
			}
		}


		/// <summary>
		/// Gets or sets the string that specifies how the editors contents 
		/// are formatted
		/// </summary>
		protected string Format
		{
			get
			{
				return this.format;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				
				this.format = value;

				this.UpdateEditText();
			}
		}


		/// <summary>
		/// Gets or sets whether the editors text is being updated
		/// </summary>
		protected bool ChangingText
		{
			get
			{
				return this.changingText;
			}

			set
			{
				this.changingText = value;
			}
		}


		/// <summary>
		/// Gets or sets whether the arrow keys should be passed to the editor
		/// </summary>
		public bool InterceptArrowKeys
		{
			get
			{
				return this.interceptArrowKeys;
			}

			set
			{
				this.interceptArrowKeys = value;
			}
		}


		/// <summary>
		/// Gets or sets whether the user has changed the editors value
		/// </summary>
		protected bool UserEdit
		{
			get
			{
				return this.userEdit;
			}
			set
			{
				this.userEdit = value;
			}
		}

		#endregion


		#region Events

        /// <summary>
        /// Handler for the editors TextBox.MouseWheel event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{A2DCDED6-1D3C-4644-A887-E1D33EB481A7}</MetaDataID>
        protected internal virtual void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bool up = true;

			this.wheelDelta += e.Delta;

			if (Math.Abs(this.wheelDelta) >= 120)
			{
				if (this.wheelDelta < 0)
				{
					up = false;
				}

				if (up)
				{
					this.UpButton();
				}
				else
				{
					this.DownButton();
				}

				this.wheelDelta = 0;
			}
		}


        /// <summary>
        /// Handler for the editors TextBox.KeyDown event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{826CA88D-6431-419F-9C70-AB91D98A8B8A}</MetaDataID>
        protected virtual void OnTextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (this.interceptArrowKeys)
			{
                if (e.KeyData == System.Windows.Forms.Keys.Up)
				{
					this.UpButton();

					e.Handled = true;
				}
                else if (e.KeyData == System.Windows.Forms.Keys.Down)
				{
					this.DownButton();

					e.Handled = true;
				}
			}

            if (e.KeyCode == System.Windows.Forms.Keys.Return)
			{
				this.ValidateEditText();
			}
		}


        /// <summary>
        /// Handler for the editors TextBox.KeyPress event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data</param>
        /// <MetaDataID>{5F6CB6C1-48CD-41FD-8573-3874B328AFBD}</MetaDataID>
        protected virtual void OnTextBoxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			char enter = AsciiChars.CarriageReturn;
			char escape = AsciiChars.Escape;
			char tab = AsciiChars.HorizontalTab;
			
			NumberFormatInfo info = CultureInfo.CurrentCulture.NumberFormat;
			
			string decimalSeparator = info.NumberDecimalSeparator;
			string groupSeparator = info.NumberGroupSeparator;
			string negativeSign = info.NegativeSign;
			string character = e.KeyChar.ToString();
			
			if ((!char.IsDigit(e.KeyChar) && !character.Equals(decimalSeparator) && !character.Equals(groupSeparator)) && 
				!character.Equals(negativeSign) && (e.KeyChar != tab))
			{
                if ((System.Windows.Forms.Control.ModifierKeys & (System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Control)) == System.Windows.Forms.Keys.None)
				{
					e.Handled = true;

					if (e.KeyChar == enter)
					{
						if (this.EditingTable != null)
						{
							this.EditingTable.StopEditing();
						}
					}
					else if (e.KeyChar == escape)
					{
						if (this.EditingTable != null)
						{
							this.EditingTable.CancelEditing();
						}
					}
					else
					{
						NativeMethods.MessageBeep(0 /*MB_OK*/);
					}
				}
			}
		}


        /// <summary>
        /// Handler for the editors TextBox.LostFocus event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{09856954-9733-47E9-BCB4-801FDFB8D839}</MetaDataID>
		protected virtual void OnTextBoxLostFocus(object sender, EventArgs e)
		{
			if (this.UserEdit)
			{
				this.ValidateEditText();
			}

			if (this.EditingTable != null)
			{
				this.EditingTable.StopEditing();
			}
		}


        /// <summary>
        /// Handler for the editors buttons MouseDown event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{8A84358B-F27D-450D-9F5A-2319D92BCEF2}</MetaDataID>
		public void OnEditorButtonMouseDown(object sender, CellMouseEventArgs e)
		{
			this.ParseEditText();

			if (e.Y < this.buttonBounds.Top + (this.buttonBounds.Height / 2))
			{
				this.buttonID = UpButtonID;
				
				this.UpButton();
			}
			else
			{
				this.buttonID = DownButtonID;
				
				this.DownButton();
			}

			this.StartTimer();
		}


        /// <summary>
        /// Handler for the editors buttons MouseUp event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{4DC91896-DD80-41B5-A506-5389DF1021D4}</MetaDataID>
		public void OnEditorButtonMouseUp(object sender, CellMouseEventArgs e)
		{
			this.StopTimer();

			this.buttonID = 0;
		}


        /// <summary>
        /// Handler for the editors Timer event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{C007D312-5AF4-403E-AECC-1E005A69B679}</MetaDataID>
		private void TimerHandler(object sender, EventArgs e)
		{
			if (buttonID == 0)
			{
				this.StopTimer();

				return;
			}

			if (buttonID == UpButtonID)
			{
				this.UpButton();
			}
			else
			{
				this.DownButton();
			}
				
			this.interval *= 7;
			this.interval /= 10;
			
			if (this.interval < 1)
			{
				this.interval = 1;
			}
			
			this.timer.Interval = this.interval;
		}


		#endregion
	}
}
