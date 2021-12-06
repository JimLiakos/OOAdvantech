namespace NotifyIconWpf.Interop
{
    /// <summary>
    /// The state of the icon - can be set to
    /// hide the icon.
    /// </summary>
    /// <MetaDataID>{1577c08e-d01b-4c49-bfb8-8881fce47e19}</MetaDataID>
    public enum IconState
    {
        /// <summary>
        /// The icon is visible.
        /// </summary>
        Visible = 0x00,

        /// <summary>
        /// Hide the icon.
        /// </summary>
        Hidden = 0x01,

        // The icon is shared - currently not supported, thus commented out.
        //Shared = 0x02
    }
}