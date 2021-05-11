using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ConnectableControls.SchedulerControl.RowDesign;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{68043e42-2def-4793-a0b8-9e7749f44cf3}</MetaDataID>
    public interface IRenderer
    {
        /// <summary>
        /// Gets a Rectangle that represents the client area of the object 
        /// being rendered
        /// </summary>
        /// <MetaDataID>{10ad55ec-f769-4531-8692-a59ce090c929}</MetaDataID>
        Rectangle ClientRectangle
        {
            get;
        }


        /// <summary>
        /// Gets or sets a Rectangle that represents the size and location 
        /// of the object being rendered
        /// </summary>
        /// <MetaDataID>{b318ef75-b6c5-4408-976c-3a761793864f}</MetaDataID>
        Rectangle Bounds
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the font of the text displayed by the object being 
        /// rendered
        /// </summary>
        /// <MetaDataID>{5062984c-3234-416c-9885-48a39e75e5c0}</MetaDataID>
        Font Font
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the foreground color of the object being rendered
        /// </summary>
        /// <MetaDataID>{7c3894d7-0ed1-45c4-b730-36a2260925ab}</MetaDataID>
        Color ForeColor
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the background color for the object being rendered
        /// </summary>
        /// <MetaDataID>{6c3c328e-473e-4dcf-9618-93da9351bed0}</MetaDataID>
        Color BackColor
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets how the Renderers contents are aligned horizontally
        /// </summary>
        /// <MetaDataID>{8057c91c-caf1-4640-a8a5-e7453c1c6145}</MetaDataID>
        ColumnAlignment Alignment
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets how the Renderers contents are aligned vertically
        /// </summary>
        /// <MetaDataID>{95725d8b-462d-4654-9b63-d2e80fcd33d5}</MetaDataID>
        RowAlignment LineAlignment
        {
            get;
            set;
        }
    }
}
