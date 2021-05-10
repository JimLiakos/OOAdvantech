namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;

    /// <MetaDataID>{E7AC394D-B7DD-42E5-8D93-009A972D6A14}</MetaDataID>
    [BackwardCompatibilityID("{E7AC394D-B7DD-42E5-8D93-009A972D6A14}")]
    [Persistent()]
    public class TextColumn : Column
    {
        /// <MetaDataID>{44d40756-cdd8-4ca1-9c2a-5732160f672b}</MetaDataID>
        public TextColumn(Column copyColumn)
            : base(copyColumn)
        {
        }
        /// <MetaDataID>{447b122e-ad98-4e13-b150-0c0803aac2d8}</MetaDataID>
        public TextColumn()
        {
        }

    }
}
