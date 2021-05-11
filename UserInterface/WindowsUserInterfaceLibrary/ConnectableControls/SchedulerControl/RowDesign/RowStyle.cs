using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace ConnectableControls.SchedulerControl.RowDesign
{
    /// <summary>
    /// Stores visual appearance related properties for a Row
    /// </summary>
    /// <MetaDataID>{5c4a9f52-7183-4f05-b435-0d907f959787}</MetaDataID>
    public class RowStyle
    {
        #region Class Data

        /// <summary>
        /// The background color of the Row
        /// </summary>
        private Color backColor;

        /// <summary>
        /// The foreground color of the Row
        /// </summary>
        private Color foreColor;

        /// <summary>
        /// The font used to draw the text in the Row
        /// </summary>
        private Font font;

        /// <summary>
        /// The alignment of the text in the Row
        /// </summary>
        private RowAlignment alignment;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the RowStyle class with default settings
        /// </summary>
        /// <MetaDataID>{834DE417-36AE-48DC-9AA7-426E274E9FF0}</MetaDataID>
        public RowStyle()
        {
            this.backColor = Color.Empty;
            this.foreColor = Color.Empty;
            this.font = null;
            this.alignment = RowAlignment.Center;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Font used by the Row
        /// </summary>
        [Category("Appearance"),
        Description("The font used to display text in the row")]
        public Font Font
        {
            get
            {
                return this.font;
            }

            set
            {
                this.font = value;
            }
        }


        /// <summary>
        /// Gets or sets the background color for the Row
        /// </summary>
        [Category("Appearance"),
        Description("The background color used to display text and graphics in the row")]
        public Color BackColor
        {
            get
            {
                return this.backColor;
            }

            set
            {
                this.backColor = value;
            }
        }


        /// <summary>
        /// Gets or sets the foreground color for the Row
        /// </summary>
        [Category("Appearance"),Description("The foreground color used to display text and graphics in the row")]
        public Color ForeColor
        {
            get
            {
                return this.foreColor;
            }

            set
            {
                this.foreColor = value;
            }
        }


        /// <summary>
        /// Gets or sets the vertical alignment of the text displayed in the Row
        /// </summary>
        [Category("Appearance"),
        DefaultValue(RowAlignment.Center),
        Description("The vertical alignment of the objects displayed in the row")]
        public RowAlignment Alignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                this.alignment = value;
            }
        }

        #endregion
    }
}
