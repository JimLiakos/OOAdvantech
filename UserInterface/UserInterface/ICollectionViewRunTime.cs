namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{E9FD8A83-F4CE-4FDB-9101-6B03D7AE10F5}</MetaDataID>
    public interface ICollectionViewRunTime:System.Collections.ICollection
    {
        /// <MetaDataID>{CC1D2D98-C83D-4FE7-B8C7-9F8CA6454E40}</MetaDataID>
        void SetSelected(object[] items);

        /// <MetaDataID>{E04C49B4-5BA6-4D74-A698-9A131CE2D17D}</MetaDataID>
        void AddItem(object item);
        /// <MetaDataID>{044BE93B-75AF-423D-97DA-787F8C1B3CD3}</MetaDataID>
        void RemoveItem(object item);
        /// <MetaDataID>{CC9D5650-7693-4AA2-9120-B62116E298F0}</MetaDataID>

        /// <MetaDataID>{A2E97AB2-5DE5-4CA0-A806-5CB3559CAE74}</MetaDataID>
        object this[int index]
        {
            get;
            set;
        }

        /// <MetaDataID>{9057672F-234A-4BC7-A003-3BE23A476776}</MetaDataID>
        void SetRowColor(int index, System.Drawing.Color color);
    }
}
