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
using System.Globalization;
using System.Windows.Forms;

using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Models.Design;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Sorting;


namespace ConnectableControls.List.Models
{
    /// <summary>
    /// Represents a Column whose Cells are displayed as a DateTime
    /// </summary>
    /// <MetaDataID>{8ECE6A7F-ECE1-4C76-874D-B0569D3D51D3}</MetaDataID>
	[DesignTimeVisible(false),
	ToolboxItem(false)]
	public class DateTimeColumn : Column
	{
		#region Class Data

		/// <summary>
		/// Default long date format
		/// </summary>
		public static readonly string LongDateFormat = DateTimeFormatInfo.CurrentInfo.LongDatePattern;

		/// <summary>
		/// Default short date format
		/// </summary>
		public static readonly string ShortDateFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

		/// <summary>
		/// Default time format
		/// </summary>
		public static readonly string TimeFormat = DateTimeFormatInfo.CurrentInfo.LongTimePattern;

		/// <summary>
		/// The format of the date and time displayed in the Cells
		/// </summary>
		private DateTimePickerFormat dateFormat;

		/// <summary>
		/// The custom date/time format string
		/// </summary>
		private string customFormat;

		#endregion
		
		
		#region Constructor

        /// <summary>
        /// Creates a new DateTimeColumn with default values
        /// </summary>
        /// <MetaDataID>{77EF44A1-36F1-41D1-ABFB-D6A1731311A9}</MetaDataID>
		public DateTimeColumn() : base()
		{
			this.Init();
		}

        /// <MetaDataID>{012B58D6-CB90-455C-A63C-5B34E4BA184D}</MetaDataID>
        public DateTimeColumn(Column copyColumn)
            : base(copyColumn)
        {
            Init();
        }


        /// <summary>
        /// Creates a new DateTimeColumn with the specified header text
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <MetaDataID>{6DDC3E8C-5922-4621-9237-C4416365076A}</MetaDataID>
		public DateTimeColumn(string text) : base(text)
		{
			this.Init();
		}

        public DateTimeColumn(OOAdvantech.UserInterface.DateTimeColumn copyColumn)
            : base(copyColumn)
        {
            Init();

            switch (copyColumn.DateTimeFormat)
            {
                case OOAdvantech.UserInterface.DateTimePickerFormat.Custom:
                    this.dateFormat = DateTimePickerFormat.Custom;
                    break;
                case OOAdvantech.UserInterface.DateTimePickerFormat.Long:
                    this.dateFormat = DateTimePickerFormat.Long;
                    break;
                case OOAdvantech.UserInterface.DateTimePickerFormat.Short:
                    this.dateFormat = DateTimePickerFormat.Short;
                    break;
                case OOAdvantech.UserInterface.DateTimePickerFormat.Time:
                    this.dateFormat = DateTimePickerFormat.Time;
                    break;
            }
        }


        /// <summary>
        /// Creates a new DateTimeColumn with the specified header text and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{1B5FCA69-A04C-4B15-B021-75820E5872BA}</MetaDataID>
		public DateTimeColumn(string text, int width) : base(text, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DateTimeColumn with the specified header text, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{5D1D1FB7-AD6D-4C4D-B094-727A7CC049BE}</MetaDataID>
		public DateTimeColumn(string text, int width, bool visible) : base(text, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DateTimeColumn with the specified header text and image
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <MetaDataID>{1C977F8F-2435-4F21-992A-1C2B0FA68A3F}</MetaDataID>
		public DateTimeColumn(string text, Image image) : base(text, image)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DateTimeColumn with the specified header text, image and width
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <MetaDataID>{F4D61621-F1CD-4249-A63D-9F888AC7310E}</MetaDataID>
		public DateTimeColumn(string text, Image image, int width) : base(text, image, width)
		{
			this.Init();
		}


        /// <summary>
        /// Creates a new DateTimeColumn with the specified header text, image, width and visibility
        /// </summary>
        /// <param name="text">The text displayed in the column's header</param>
        /// <param name="image">The image displayed on the column's header</param>
        /// <param name="width">The column's width</param>
        /// <param name="visible">Specifies whether the column is visible</param>
        /// <MetaDataID>{6FA2E200-2C27-4B57-9911-9DF1EE3D8CAB}</MetaDataID>
		public DateTimeColumn(string text, Image image, int width, bool visible) : base(text, image, width, visible)
		{
			this.Init();
		}


        /// <summary>
        /// Initializes the DateTimeColumn with default values
        /// </summary>
        /// <MetaDataID>{21491F70-3FCF-4E5D-A59A-672F3D85A144}</MetaDataID>
		internal void Init()
		{
            this.dateFormat = DateTimePickerFormat.Short ;
			this.customFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern;
            //_Type = ColumnType.DateTimeColumn;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{DDDF378C-CF88-4225-B7A1-94EC92CB0EFA}</MetaDataID>
		public override string GetDefaultRendererName()
		{
			return "DATETIME";
		}


        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{0D7F567E-F007-42C9-9FAA-27376F504766}</MetaDataID>
		public override ICellRenderer CreateDefaultRenderer()
		{
			return new DateTimeCellRenderer();
		}


        /// <summary>
        /// Gets a string that specifies the name of the Column's default CellEditor
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellEditor</returns>
        /// <MetaDataID>{76DC634F-007D-4E58-864C-7CB2DD32B9A2}</MetaDataID>
		public override string GetDefaultEditorName()
		{
			return "DATETIME";
		}


        /// <summary>
        /// Gets the Column's default CellEditor
        /// </summary>
        /// <returns>The Column's default CellEditor</returns>
        /// <MetaDataID>{291D0F0B-AF4E-42B8-B503-560C78489925}</MetaDataID>
		public override ICellEditor CreateDefaultEditor()
		{
			return new DateTimeCellEditor();
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the format of the date and time displayed in the Column's Cells
		/// </summary>
		[Category("Appearance"),
		DefaultValue(DateTimePickerFormat.Long),
		Description("The format of the date and time displayed in the Column's Cells")]
		public DateTimePickerFormat DateTimeFormat
		{
			get
			{
				return this.dateFormat;
			}

            set
            {
                if (!Enum.IsDefined(typeof(DateTimePickerFormat), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(DateTimePickerFormat));
                }


                if (this.dateFormat != value)
                {

                    if (this.dateFormat != value)
                    {
                        this.dateFormat = value;
                        if (ColumnMetaData != null && ColumnMetaData is OOAdvantech.UserInterface.DateTimeColumn)
                        {
                            switch (this.dateFormat)
                            {
                                case DateTimePickerFormat.Custom:
                                    (ColumnMetaData as OOAdvantech.UserInterface.DateTimeColumn).DateTimeFormat = OOAdvantech.UserInterface.DateTimePickerFormat.Custom;
                                    break;
                                case DateTimePickerFormat.Long:
                                    (ColumnMetaData as OOAdvantech.UserInterface.DateTimeColumn).DateTimeFormat = OOAdvantech.UserInterface.DateTimePickerFormat.Long;
                                    break;
                                case DateTimePickerFormat.Short:
                                    (ColumnMetaData as OOAdvantech.UserInterface.DateTimeColumn).DateTimeFormat = OOAdvantech.UserInterface.DateTimePickerFormat.Short;
                                    break;
                                case DateTimePickerFormat.Time:
                                    (ColumnMetaData as OOAdvantech.UserInterface.DateTimeColumn).DateTimeFormat = OOAdvantech.UserInterface.DateTimePickerFormat.Time;
                                    break;

                            }
                        }

                    }
                }
            }
		}


		/// <summary>
		/// Gets or sets the custom date/time format string
		/// </summary>
		[Category("Appearance"),
		Description("The custom date/time format string")]
		public string CustomDateTimeFormat
		{
			get
			{
				return this.customFormat;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("CustomFormat cannot be null");
				}

				if (!this.customFormat.Equals(value))
				{
					this.customFormat = value;

					this.OnPropertyChanged(new ColumnEventArgs(this, ColumnEventType.RendererChanged, null));
				}

				DateTime.Now.ToString(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
			}
		}


        /// <summary>
        /// Specifies whether the CustomDateTimeFormat property should be serialized at 
        /// design time
        /// </summary>
        /// <returns>true if the CustomDateTimeFormat property should be serialized, 
        /// false otherwise</returns>
        /// <MetaDataID>{29C64214-C235-46D1-9674-4563CAD75AAD}</MetaDataID>
		private bool ShouldSerializeCustomDateTimeFormat()
		{
			return !this.customFormat.Equals(DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern);
		}


		/// <summary>
		/// Gets or sets the string that specifies how the Column's Cell contents 
		/// are formatted
		/// </summary>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string Format
		{
			get
			{
				return this.CustomDateTimeFormat;
			}

			set
			{
				this.CustomDateTimeFormat = value;
			}
		}


		/// <summary>
		/// Gets the Type of the Comparer used to compare the Column's Cells when 
		/// the Column is sorting
		/// </summary>
		public override Type DefaultComparerType
		{
			get
			{
				return typeof(DateTimeComparer);
			}
		}

		#endregion
	}
}
