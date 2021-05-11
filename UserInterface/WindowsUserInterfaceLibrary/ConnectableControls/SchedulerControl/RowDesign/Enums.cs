namespace ConnectableControls.SchedulerControl.RowDesign
{
    #region public enum RowEventType
    /// <summary>
    /// Specifies the type of event generated when the value of a 
    /// Row's property changes
    /// </summary>
    /// <MetaDataID>{ddcaf7b5-8914-4b70-880a-9dfce05f84f6}</MetaDataID>
    public enum RowEventType
    {
        /// <summary>
        /// Occurs when the Row's property change type is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Occurs when the value of a Row's BackColor property changes
        /// </summary>
        BackColorChanged = 1,

        /// <summary>
        /// Occurs when the value of a Row's ForeColor property changes
        /// </summary>
        ForeColorChanged = 2,

        /// <summary>
        /// Occurs when the value of a Row's Font property changes
        /// </summary>
        FontChanged = 3,

        /// <summary>
        /// Occurs when the value of a Row's RowStyle property changes
        /// </summary>
        StyleChanged = 4,

        /// <summary>
        /// Occurs when the value of a Row's Alignment property changes
        /// </summary>
        AlignmentChanged = 5,

        /// <summary>
        /// Occurs when the value of a Row's Enabled property changes
        /// </summary>
        EnabledChanged = 6,

        /// <summary>
        /// Occurs when the value of a Row's Editable property changes
        /// </summary>
        EditableChanged = 7
    } 
    #endregion

    #region public enum RowAlignment
    /// <summary>
    /// Specifies alignment of a TotalRows content
    /// </summary>
    /// <MetaDataID>{7623b48e-791f-412d-924b-c3327d3dd579}</MetaDataID>
    public enum RowAlignment
    {
        /// <summary>
        /// The TotalRows content is aligned to the top
        /// </summary>
        Top = 0,

        /// <summary>
        /// The TotalRows content is aligned to the center
        /// </summary>
        Center = 1,

        /// <summary>
        /// The TotalRows content is aligned to the bottom
        /// </summary>
        Bottom = 2
    } 
    #endregion
}