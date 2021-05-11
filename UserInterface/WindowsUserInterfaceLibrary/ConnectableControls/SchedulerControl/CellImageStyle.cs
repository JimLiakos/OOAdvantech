using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{796bc9b1-6da0-4ffe-b275-14535be6a9e3}</MetaDataID>
    internal class CellImageStyle
    {
        #region Class Data

        /// <summary>
        /// The Image displayed in the Cell
        /// </summary>
        private Image image;

        /// <summary>
        /// Determines how Images are sized in the Cell
        /// </summary>
        private ImageSizeMode imageSizeMode;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellImageStyle class with default settings
        /// </summary>
        /// <MetaDataID>{1E4701DC-C1A3-447F-A7D6-CF6454599C2F}</MetaDataID>
        public CellImageStyle()
        {
            this.image = null;
            this.imageSizeMode = ImageSizeMode.Normal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the image that is displayed in the Cell
        /// </summary>
        public Image Image
        {
            get
            {
                return this.image;
            }

            set
            {
                this.image = value;
            }
        }


        /// <summary>
        /// Gets or sets how the Cells image is sized within the Cell
        /// </summary>
        public ImageSizeMode ImageSizeMode
        {
            get
            {
                return this.imageSizeMode;
            }

            set
            {
                if (!Enum.IsDefined(typeof(ImageSizeMode), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ImageSizeMode));
                }

                if (this.imageSizeMode != value)
                {
                    this.imageSizeMode = value;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Specifies how Images are sized within a Cell
    /// </summary>
    /// <MetaDataID>{667bd8d3-643d-4334-a042-074ed7022314}</MetaDataID>
    public enum ImageSizeMode
    {
        /// <summary>
        /// The Image will be displayed normally
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The Image will be stretched/shrunken to fit the Cell
        /// </summary>
        SizedToFit = 1,

        /// <summary>
        /// The Image will be scaled to fit the Cell
        /// </summary>
        ScaledToFit = 2
    }
}
