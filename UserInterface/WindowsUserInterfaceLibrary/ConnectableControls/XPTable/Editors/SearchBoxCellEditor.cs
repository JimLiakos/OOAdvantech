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
using System.ComponentModel;



namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// A class for editing Cells that contain strings
    /// </summary>
    /// <MetaDataID>{1B6F45E8-BCE0-4D8A-93FC-1A064D7A1ADE}</MetaDataID>
    public class SearchBoxCellEditor : CellEditor
    {
        /// <MetaDataID>{13243CA1-FB22-4594-9884-D48DED2771D4}</MetaDataID>
        public override bool ProcessMouseMessage(Control target, WindowMessage msg, int wParam, int lParam)
        {
            if (msg == WindowMessage.WM_LBUTTONDOWN || msg == WindowMessage.WM_RBUTTONDOWN ||
            msg == WindowMessage.WM_MBUTTONDOWN || msg == WindowMessage.WM_XBUTTONDOWN ||
            msg == WindowMessage.WM_NCLBUTTONDOWN || msg == WindowMessage.WM_NCRBUTTONDOWN ||
            msg == WindowMessage.WM_NCMBUTTONDOWN || msg == WindowMessage.WM_NCXBUTTONDOWN)
            {
                Point cursorPos = this.Control.Parent.PointToClient(Cursor.Position);

                if (!(this.Control as ConnectableControls.SearchTextBox).Bounds.Contains(cursorPos))
                {

                    if (target != this.EditingTable && (target != (this.Control as ConnectableControls.SearchTextBox).TextBox && target != (this.Control as ConnectableControls.SearchTextBox).SearchBtn))
                    {

                        this.EditingTable.StopEditing();
                    }
                }
            }

            return false;
        }
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TextCellEditor class with default settings
        /// </summary>
        /// <MetaDataID>{AD1A0637-C7A4-4F4F-BC55-F2DE0E56B68C}</MetaDataID>
        public SearchBoxCellEditor()
            : base()
        {
            ConnectableControls.SearchTextBox textbox = new ConnectableControls.SearchTextBox();
            textbox.TextBox.AutoSize = false;
            textbox.TextBox.BorderStyle = BorderStyle.None;
            //textbox.TextBox.TextChanged += new EventHandler(textbox_TextChanged);

            this.Control = textbox;
        }
        string LastText;

        //void textbox_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox textbox = (Control as ConnectableControls.SearchTextBox).TextBox;
        //    try
        //    {

        //        System.Type valueType = EditingCell.Row.TableModel.InternalTable.ColumnModel.Columns[EditingCell.Index].ValuesType;
        //        object Value = System.Convert.ChangeType(textbox.Text, valueType);
        //        LastText = textbox.Text;
        //    }
        //    catch (System.Exception error)
        //    {
        //        int _SelectionStart = textbox.SelectionStart - 1;
        //        if (_SelectionStart < 0)
        //            _SelectionStart = 0;
        //        textbox.Text = LastText;
        //        textbox.SelectionStart = _SelectionStart;
        //    }


        //    // EditingCell.Row.TableModel.InternalTable.ColumnModel
        //}



        #endregion


        #region Methods

        /// <summary>
        /// Sets the location and size of the CellEditor
        /// </summary>
        /// <param name="cellRect">A Rectangle that represents the size and location 
        /// of the Cell being edited</param>
        /// <MetaDataID>{55ADDF3D-CB7E-4F3B-9825-F37881194FF1}</MetaDataID>
        protected override void SetEditLocation(Rectangle cellRect)
        {
            this.Control.Location = cellRect.Location;
            this.Control.Size = new Size(cellRect.Width - 1, cellRect.Height - 1);
        }


        /// <summary>
        /// Sets the initial value of the editor based on the contents of 
        /// the Cell being edited
        /// </summary>
        /// <MetaDataID>{D074827D-C640-49DF-96A1-DB9CBB6C8791}</MetaDataID>
        protected override void SetEditValue()
        {
            this.TextBox.Text = this.EditingCell.Text;
        }


        /// <summary>
        /// Sets the contents of the Cell being edited based on the value 
        /// in the editor
        /// </summary>
        /// <MetaDataID>{87F45BB0-34D7-415F-ABD4-C8BD12E39083}</MetaDataID>
        protected override void SetCellValue()
        {
            if (EditingCell.Data != (Control as ConnectableControls.SearchTextBox).Value)
            {
                EditingCell.Data = (Control as ConnectableControls.SearchTextBox).Value;
                EditingCell.Text = (Control as ConnectableControls.SearchTextBox).Text;
                EditingCell.Column.SetValue(cell.Row, cell.Data);
            }

        }



        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{47A1514E-0CC5-4DB5-A44C-59A47C7980E5}</MetaDataID>
        public override void StartEditing()
        {
            this.TextBox.KeyPress += new KeyPressEventHandler(OnKeyPress);
            this.TextBox.LostFocus += new EventHandler(OnLostFocus);
            (this.Control as ConnectableControls.SearchTextBox).Value = cell.Data;
            (this.Control as ConnectableControls.SearchTextBox).SearchBtn.LostFocus += new EventHandler(OnLostFocus);
            (this.Control as ConnectableControls.SearchTextBox).OperationCall = (this.cell.Row.TableModel.Table.ColumnModel.Columns[this.cell.Index] as SearchBoxColumn).OperationCall;
            (this.Control as ConnectableControls.SearchTextBox).UserInterfaceObjectConnection = cell.Column.UserInterfaceObjectConnection;
            (this.Control as ConnectableControls.SearchTextBox).DisplayMember = (this.cell.Row.TableModel.Table.ColumnModel.Columns[this.cell.Index] as SearchBoxColumn).DisplayMember as string;
            
            (this.Control as ConnectableControls.SearchTextBox).Enabled = true;

            base.StartEditing();

            this.TextBox.Focus();
        }


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{42566466-1583-4536-87A9-E4DC7F18D9DB}</MetaDataID>
        public override void StopEditing()
        {
            this.TextBox.KeyPress -= new KeyPressEventHandler(OnKeyPress);
            this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
            (this.Control as ConnectableControls.SearchTextBox).SearchBtn.LostFocus -= new EventHandler(OnLostFocus);


            base.StopEditing();
        }


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{50E63FA5-EF8D-4DBD-8823-028F497824DF}</MetaDataID>
        public override void CancelEditing()
        {
            this.TextBox.KeyPress -= new KeyPressEventHandler(OnKeyPress);
            this.TextBox.LostFocus -= new EventHandler(OnLostFocus);
            (this.Control as ConnectableControls.SearchTextBox).SearchBtn.LostFocus -= new EventHandler(OnLostFocus);

            base.CancelEditing();
        }


        #endregion


        #region Properties

        /// <summary>
        /// Gets the TextBox used to edit the Cells contents
        /// </summary>
        public System.Windows.Forms.TextBox TextBox
        {
            get
            {
                return (this.Control as ConnectableControls.SearchTextBox).TextBox;
            }
        }

        #endregion


        #region Events

        /// <summary>
        /// Handler for the editors TextBox.KeyPress event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data</param>
        /// <MetaDataID>{84744001-BB2F-46C0-B9ED-FCB7BB3CC548}</MetaDataID>
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
        /// <MetaDataID>{C47CAD92-FB31-448A-8E36-D87717E27E08}</MetaDataID>
        protected virtual void OnLostFocus(object sender, EventArgs e)
        {
            if (this.EditingTable != null)
            {
                if ((Control is ConnectableControls.SearchTextBox) &&
                    (Control as ConnectableControls.SearchTextBox).HasFocus)
                {
                    return;

                }

                this.EditingTable.StopEditing();
            }
        }

        #endregion
    }
}
