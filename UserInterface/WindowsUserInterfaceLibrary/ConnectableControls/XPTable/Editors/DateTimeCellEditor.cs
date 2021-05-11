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

using ConnectableControls.List.Models;
using ConnectableControls.List.Renderers;


namespace ConnectableControls.List.Editors
{
    /// <summary>
    /// A class for editing Cells that contain DateTimes
    /// </summary>
    /// <MetaDataID>{78F5BFAC-C165-46D7-B2E5-BFE61AC8DD0C}</MetaDataID>
	public class DateTimeCellEditor : CellEditor
	{
		#region EventHandlers

		/// <summary>
		/// Occurs when the user makes an explicit date selection using the mouse
		/// </summary>
		public event DateRangeEventHandler DateSelected;

		#endregion
		
		
		#region Class Data

        ///// <summary>
        ///// The MonthCalendar that will be shown in the drop-down portion of the 
        ///// DateTimeCellEditor
        ///// </summary>
        //private MonthCalendar calendar;

		#endregion


		#region Constructor

        /// <summary>
        /// Initializes a new instance of the DateTimeCellEditor class with default settings
        /// </summary>
        /// <MetaDataID>{61905581-43D6-4EA1-8BD1-E93E3A5F5488}</MetaDataID>
		public DateTimeCellEditor() : base()
		{
            Control=new DateTimePicker();
            


            //this.calendar = new MonthCalendar();
            //this.calendar.Location = new System.Drawing.Point(0, 0);
            //this.calendar.MaxSelectionCount = 1;

            //this.DropDown.Width = this.calendar.Width + 2;
            //this.DropDown.Height = this.calendar.Height + 2;
            //this.DropDown.Control = this.calendar;

            //base.DropDownStyle = DropDownStyle.DropDownList;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Sets the location and size of the CellEditor
        /// </summary>
        /// <param name="cellRect">A Rectangle that represents the size and location 
        /// of the Cell being edited</param>
        /// <MetaDataID>{CBE4FA3A-3FD8-42AF-8607-5C17112129B5}</MetaDataID>
		protected override void SetEditLocation(Rectangle cellRect)
		{
			// calc the size of the textbox
			ICellRenderer renderer = this.EditingTable.ColumnModel.GetCellRenderer(this.EditingCellPos.Column);
            Control.Location = cellRect.Location;
            Control.Size = cellRect.Size;

			//int buttonWidth = ((DateTimeCellRenderer) renderer).ButtonWidth;

            //this.TextBox.Size = new Size(cellRect.Width - 1 - buttonWidth, cellRect.Height-1);
            //this.TextBox.Location = cellRect.Location;
		}


        /// <summary>
        /// Sets the initial value of the editor based on the contents of 
        /// the Cell being edited
        /// </summary>
        /// <MetaDataID>{4A6BEB4B-DF13-428C-9D72-C64587C19428}</MetaDataID>
		protected override void SetEditValue()
		{
			// set default values incase we can't find what we're looking for
            //DateTime date = DateTime.Now;
            //String format = DateTimeColumn.LongDateFormat;
			
            //if (this.EditingCell.Data != null && this.EditingCell.Data is DateTime)
            //{
            //    date = (DateTime) this.EditingCell.Data;

            //    if (this.EditingTable.ColumnModel.Columns[this.EditingCellPos.Column] is DateTimeColumn)
            //    {
            //        DateTimeColumn dtCol = (DateTimeColumn) this.EditingTable.ColumnModel.Columns[this.EditingCellPos.Column];
					
            //        switch (dtCol.DateTimeFormat)
            //        {
            //            case DateTimePickerFormat.Short:	
            //                format = DateTimeColumn.ShortDateFormat;
            //                break;

            //            case DateTimePickerFormat.Time:	
            //                format = DateTimeColumn.TimeFormat;
            //                break;

            //            case DateTimePickerFormat.Custom:	
            //                format = dtCol.CustomDateTimeFormat;
            //                break;
            //        }
            //    }
            //}
				
            if(EditingCell.Data is DateTime)
			    (Control as DateTimePicker).Value=(DateTime)EditingCell.Data;
			//this.TextBox.Text = date.ToString(format);
		}


        /// <summary>
        /// Sets the contents of the Cell being edited based on the value 
        /// in the editor
        /// </summary>
        /// <MetaDataID>{F0A23DEC-3702-45FE-A458-B907B82A3DB5}</MetaDataID>
		protected override void SetCellValue()
		{
            if (EditingCell.Data==null||((DateTime)EditingCell.Data) != (Control as System.Windows.Forms. DateTimePicker).Value)
            {
                EditingCell.Data = (Control as DateTimePicker).Value;
                EditingCell.Text = (Control as DateTimePicker).Text;
                EditingCell.Column.SetValue(cell.Row, cell.Data);
            }
		}


        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{A4102666-46E8-4160-8C8E-62A85A8A64AE}</MetaDataID>
		public override void StartEditing()
		{
			//this.calendar.DateSelected += new DateRangeEventHandler(calendar_DateSelected);

			//this.TextBox.SelectionLength = 0;
			
			base.StartEditing();
            (Control as DateTimePicker).Format = (this.cell.Column as DateTimeColumn).DateTimeFormat;
            (Control as DateTimePicker).CustomFormat = (this.cell.Column as DateTimeColumn).CustomDateTimeFormat;
           // Control.LostFocus += new EventHandler(OnControlLostFocus);
          
		}

        void OnControlLostFocus(object sender, EventArgs e)
        {
            StopEditing();
        }


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{7C370605-61D8-4603-B2A7-DB746A0E90E3}</MetaDataID>
		public override void StopEditing()
		{
           // Control.LostFocus -= new EventHandler(OnControlLostFocus);
			//this.calendar.DateSelected -= new DateRangeEventHandler(calendar_DateSelected);
			
			base.StopEditing();
		}


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{59EA2574-675A-4859-BA13-D7F37B283262}</MetaDataID>
		public override void CancelEditing()
		{
			//this.calendar.DateSelected -= new DateRangeEventHandler(calendar_DateSelected);
          //  Control.LostFocus -= new EventHandler(OnControlLostFocus);
			
			base.CancelEditing();
		}

		#endregion


		#region Properties

        ///// <summary>
        ///// Gets or sets a value specifying the style of the drop down editor
        ///// </summary>
        //public new DropDownStyle DropDownStyle
        //{
        //    get
        //    {
        //        return base.DropDownStyle;
        //    }

        //    set
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

		#endregion


		#region Events

        /// <summary>
        /// Raises the DateSelected event
        /// </summary>
        /// <param name="e">A DateRangeEventArgs that contains the event data</param>
        /// <MetaDataID>{67C1EE78-B2A7-4572-82BF-25C08A571989}</MetaDataID>
		protected virtual void OnDateSelected(DateRangeEventArgs e)
		{
			if (DateSelected != null)
			{
				DateSelected(this, e);
			}
		}


        ///// <summary>
        ///// Handler for the editors MonthCalendar.DateSelected events
        ///// </summary>
        ///// <param name="sender">The object that raised the event</param>
        ///// <param name="e">A DateRangeEventArgs that contains the event data</param>
        ///// <MetaDataID>{220F2A33-2AB7-42D7-A08C-42C2E2985DCE}</MetaDataID>
        //private void calendar_DateSelected(object sender, DateRangeEventArgs e)
        //{
        //    this.DroppedDown = false;

        //    this.OnDateSelected(e);

        //    this.EditingTable.StopEditing();
        //}

		#endregion
	}
}
