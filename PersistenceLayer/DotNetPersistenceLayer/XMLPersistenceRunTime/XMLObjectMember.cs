namespace OOAdvantech.XMLPersistenceRunTime
{

	/// <MetaDataID>{E8B8B4BE-1818-447B-AB04-594BA27F2F36}</MetaDataID>
	/// <summary></summary>
    public class ObjectMember : OOAdvantech.PersistenceLayer.Member
	{
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{852246DE-1D32-41DF-B6A7-2D2BD58631EE}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{D77771BE-1F52-4A1B-9E65-5EC7D155EF87}</MetaDataID>
        public override string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{82600258-7CA1-4C90-A8E4-C2FBDC8294A9}</MetaDataID>
        private short _ID;
        /// <MetaDataID>{E53C26A5-CEF3-4BD0-AF4D-160E59077002}</MetaDataID>
        public override short ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{141EB005-76AC-47BF-BDB0-4D422ADE93BF}</MetaDataID>
        private object _Value;
        /// <MetaDataID>{52FFC271-63FB-47AC-8D74-30B5CEA5A7F4}</MetaDataID>
        public override object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }


        /// <MetaDataID>{75F11B44-BCEC-4344-906B-C86406AC234B}</MetaDataID>
        public override System.Type Type
        {
            get
            {
                return null;
            }
        }
    }
}
