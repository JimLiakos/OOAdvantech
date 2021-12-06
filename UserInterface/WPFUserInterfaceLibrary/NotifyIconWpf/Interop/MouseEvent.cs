namespace NotifyIconWpf.Interop
{
    /// <summary>
    /// Event flags for clicked events.
    /// </summary>
    /// <MetaDataID>{2a444d7b-263b-4b3b-b97a-5efbbd99d951}</MetaDataID>
    public enum MouseEvent
    {
        /// <summary>
        /// The mouse was moved withing the
        /// taskbar icon's area.
        /// </summary>
        MouseMove,

        /// <summary>
        /// The right mouse button was clicked.
        /// </summary>
        IconRightMouseDown,

        /// <summary>
        /// The left mouse button was clicked.
        /// </summary>
        IconLeftMouseDown,

        /// <summary>
        /// The right mouse button was released.
        /// </summary>
        IconRightMouseUp,

        /// <summary>
        /// The left mouse button was released.
        /// </summary>
        IconLeftMouseUp,

        /// <summary>
        /// The middle mouse button was clicked.
        /// </summary>
        IconMiddleMouseDown,

        /// <summary>
        /// The middle mouse button was released.
        /// </summary>
        IconMiddleMouseUp,

        /// <summary>
        /// The taskbar icon was double clicked.
        /// </summary>
        IconDoubleClick,

        /// <summary>
        /// The balloon tip was clicked.
        /// </summary>
        BalloonToolTipClicked
    }
}