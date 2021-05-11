using System;
using System.Collections.Generic;
using System.Text;
using ConnectableControls.SchedulerControl.Events;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{1b4ede48-85c2-4936-86c9-78a33e3caca1}</MetaDataID>
    public interface IHeaderRenderer : IRenderer
    {
        /// <summary>
        /// Raises the PaintHeader event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{1EB7360C-C7F5-4BC9-9297-D35A2E66DB46}</MetaDataID>
        void OnPaintHeader(PaintHeaderEventArgs e);


        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{6BB9ECAD-699D-4017-AAEF-D15FDAF88892}</MetaDataID>
        void OnMouseEnter(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{BFE34151-8CF8-46DB-B7B5-2BFF55F46E47}</MetaDataID>
        void OnMouseLeave(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{6D9A9E60-E48B-4B0A-851A-82F98C6F7EF5}</MetaDataID>
        void OnMouseUp(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{50AB71BA-4974-43FB-82DF-BB2774CD4AB4}</MetaDataID>
        void OnMouseDown(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{953FFA77-A517-43A4-99A7-4DD843BBA51C}</MetaDataID>
        void OnMouseMove(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2DBE586A-2B27-44E3-9322-EBB61749A475}</MetaDataID>
        void OnClick(HeaderMouseEventArgs e);


        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{B921DC9C-97AC-43E8-936A-97BD626AC335}</MetaDataID>
        void OnDoubleClick(HeaderMouseEventArgs e);
    }
}
