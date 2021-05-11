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
using System.Windows.Forms;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Win32;


namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// A class for editing Cells that contain strings
    /// </summary>
    /// <MetaDataID>{43AFC910-76DF-4581-8B23-E944ED865E60}</MetaDataID>
	public class TextCellEditor : CellEditor
	{
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the TextCellEditor class with default settings
        /// </summary>
        /// <MetaDataID>{C4823DEA-F87D-4CB9-A1C3-DBDA76516466}</MetaDataID>
        public TextCellEditor()
            : base()
		{
			TextBox textbox = new TextBox();
			textbox.AutoSize = true;
			textbox.BorderStyle = BorderStyle.Fixed3D;
            textbox.TextChanged +=new EventHandler(textbox_TextChanged);
            textbox.KeyUp += new KeyEventHandler(KeyUp);

			this.Control = textbox;
		}

        void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Left || e.KeyData == Keys.Right)
                LastSelectionStart =(this.Control as TextBox).SelectionStart;
            

        }
        string LastText;
        int LastSelectionStart=0;
        bool InSelectionChange = false;

        /// <MetaDataID>{A3657E6E-AB1B-4C06-BCDC-6720391A809C}</MetaDataID>
        void textbox_TextChanged(object sender, EventArgs e)
        {

            if (InSelectionChange)
                return;
            TextBox textbox = Control as TextBox;

            try
            {
                InSelectionChange = true;

                try
                {
                    OOAdvantech.MetaDataRepository.Classifier valueType = EditingCell.Row.TableModel.InternalTable.ColumnModel.Columns[EditingCell.Index].ValueType;

                    if (string.IsNullOrEmpty(textbox.Text.Trim()) && valueType is OOAdvantech.MetaDataRepository.Primitive)
                    {
                        textbox.Text = "0";
                        textbox.SelectionStart = 0;
                        textbox.SelectionLength = 1;
                        LastSelectionStart = 1;
                        LastText = textbox.Text;

                    }
                    else
                    {
                        object Value = System.Convert.ChangeType(textbox.Text, valueType.GetExtensionMetaObject(typeof(Type)) as Type);
                        LastText = textbox.Text;
                        LastSelectionStart = (this.Control as TextBox).SelectionStart;
                    }
                }
                catch (System.Exception error)
                {
                    int _SelectionStart = textbox.SelectionStart - 1;
                    if (_SelectionStart < 0)
                        _SelectionStart = 0;
                    textbox.Text = LastText;
                    textbox.SelectionStart = LastSelectionStart;
                }
            }
            finally
            {
                InSelectionChange = false;

            }




            // EditingCell.Row.TableModel.InternalTable.ColumnModel
        }

       

		#endregion


		#region Methods

        /// <summary>
        /// Sets the location and size of the CellEditor
        /// </summary>
        /// <param name="cellRect">A Rectangle that represents the size and location 
        /// of the Cell being edited</param>
        /// <MetaDataID>{808BFDC0-AF3F-40C1-B46F-F38CD541B524}</MetaDataID>
		protected override void SetEditLocation(Rectangle cellRect)
		{
			this.TextBox.Location = cellRect.Location;
			this.TextBox.Size = new Size(cellRect.Width-1, cellRect.Height-1);
		}


        /// <summary>
        /// Sets the initial value of the editor based on the contents of 
        /// the Cell being edited
        /// </summary>
        /// <MetaDataID>{0CD69E12-D15C-42CD-9F8E-50F1DFB7CCC1}</MetaDataID>
		protected override void SetEditValue()
		{
			this.TextBox.Text = this.EditingCell.Text;
            LastText = this.EditingCell.Text;
		}


        /// <summary>
        /// Sets the contents of the Cell being edited based on the value 
        /// in the editor
        /// </summary>
        /// <MetaDataID>{92004B9C-7E7A-4663-8322-8712E16E3D7C}</MetaDataID>
		protected override void SetCellValue()
		{
            
            if (this.EditingCell.Text != this.TextBox.Text)
            {
                this.EditingCell.Data = Convert.ChangeType(Control.Text, this.EditingCell.Column.ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                this.EditingCell.Text = this.TextBox.Text;
                this.EditingCell.Column.SetValue(this.EditingCell.Row, this.EditingCell.Data);
            }
			
            
		}


        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{1F4AA52A-A73C-4CF1-93A5-CB72F67FE873}</MetaDataID>
		public override void StartEditing()
		{
			this.TextBox.KeyPress += new KeyPressEventHandler(OnKeyPress);
			this.TextBox.LostFocus += new EventHandler(OnLostFocus);
			
            base.StartEditing();
           // this.TextBox.SetValueType(cell.Column.ValueType);

			this.TextBox.Focus();
		}


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{4FD9D516-525F-4313-83F6-C3CBB5A6FABF}</MetaDataID>
		public override void StopEditing()
		{
			this.TextBox.KeyPress -= new KeyPressEventHandler(OnKeyPress);
			this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
			
			base.StopEditing();
		}


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{C90329C4-75C4-4018-9CD4-A3CD892A865F}</MetaDataID>
		public override void CancelEditing()
		{
			this.TextBox.KeyPress -= new KeyPressEventHandler(OnKeyPress);
			this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
			
			base.CancelEditing();
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

		#endregion


		#region Events

        /// <summary>
        /// Handler for the editors TextBox.KeyPress event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data</param>
        /// <MetaDataID>{F1DE2561-34A9-4BF5-BFE3-9DADB9C7B2E0}</MetaDataID>
		protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
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
        /// <MetaDataID>{439FB397-ED5C-49DD-BB6A-97A80E66AFC8}</MetaDataID>
		protected virtual void OnLostFocus(object sender, EventArgs e)
		{
			if (this.EditingTable != null)
			{
				this.EditingTable.StopEditing();
			}
		}

		#endregion
	}
}
