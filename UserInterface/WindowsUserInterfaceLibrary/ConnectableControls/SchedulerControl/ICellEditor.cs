using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{fdd3eba0-bf19-4ae1-ba4d-6e8f1ab18a06}</MetaDataID>
    public interface ICellEditor
    {
        /// <summary>
        /// Prepares the ICellEditor to edit the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to be edited</param>
        /// <param name="table">The Table that contains the Cell</param>
        /// <param name="cellPos">A CellPos representing the position of the Cell</param>
        /// <param name="cellRect">The Rectangle that represents the Cells location and size</param>
        /// <param name="userSetEditorValues">Specifies whether the ICellEditors 
        /// starting value has already been set by the user</param>
        /// <returns>true if the ICellEditor can continue editing the Cell, false otherwise</returns>
        /// <MetaDataID>{A1825F38-C873-403C-9E91-C94760BE33A7}</MetaDataID>
        bool PrepareForEditing(Cell cell, SchedulerListView list_view, CellPos cellPos, Rectangle cellRect, bool userSetEditorValues);


        /// <summary>
        /// Starts editing the Cell
        /// </summary>
        /// <MetaDataID>{B2DD6B48-3A63-4897-8683-27A3409A9803}</MetaDataID>
        void StartEditing();


        /// <summary>
        /// Stops editing the Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{54055535-DF73-474B-898F-C8C5D2B9922B}</MetaDataID>
        void StopEditing();


        /// <summary>
        /// Stops editing the Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{6C693775-CB78-416E-9F4D-D5047455BC95}</MetaDataID>
        void CancelEditing();
    }
}
