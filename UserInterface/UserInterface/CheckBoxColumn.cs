namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{5FFFA89C-AB43-4036-A6D8-77B34D151088}</MetaDataID>
    public class CheckBoxColumn : OOAdvantech.UserInterface.Column
    {
        /// <MetaDataID>{270fcdeb-64e0-433d-83b4-a34db90a0e26}</MetaDataID>
        public CheckBoxColumn(Column copyColumn)
            : base(copyColumn)
        {
          //  Alignment = ColumnAlignment.Center;
        }
        /// <MetaDataID>{5c67c3a8-114b-406e-89e3-809c36ddd561}</MetaDataID>
        public CheckBoxColumn()
        {
            Alignment = ColumnAlignment.Center;
        }
    }
}
