namespace OOAdvantech.PersistenceLayerRunTime.ClientSide
{
    /// <MetaDataID>{2B89C9E3-8D23-42AF-8B2E-C03C69402040}</MetaDataID>
    public class MemberCollectionAgent : OOAdvantech.PersistenceLayer.Member
    {
        private StructureSet.DataBlock DataSource;
        private System.Collections.IEnumerator RowEnumerator;
        System.Data.DataRelation Relation;
        public MemberCollectionAgent(StructureSet.DataBlock dataSource, System.Data.DataRelation dataRelation, System.Collections.IEnumerator rowEnumerator)
        {
            Relation = dataRelation;
            RowEnumerator = rowEnumerator;
            DataSource = dataSource;

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1FDCEFC2-BD45-4FAF-BE0C-10873F712A3B}</MetaDataID>
        private short _ID;
        /// <MetaDataID>{21FCB372-9BAE-4DD4-898A-0A0952C2C9C1}</MetaDataID>
        public override short ID
        {
            get
            {
                return default(short);
            }
            set
            {
            }
        }
        /// <MetaDataID>{A95C4867-1F60-4F1C-985C-C2366FA18459}</MetaDataID>
        public override System.Type Type
        {
            get
            {
                return default(System.Type);
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B514D4D2-455F-4C29-8B84-6BCD53E677BF}</MetaDataID>
        private object _Value;
        /// <MetaDataID>{A33AEA15-3DDB-425A-BFA6-2104BD3CE401}</MetaDataID>
        public override object Value
        {
            get
            {
                return new StructureSetAgent(DataSource, Relation.ChildTable,(RowEnumerator.Current as System.Data.DataRow).GetChildRows(Relation).GetEnumerator());
            }
            set
            {
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{077BA889-8E0E-44D9-AB90-7825C32065A6}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{F9369B0E-4638-4FA4-BCEF-DADACC42B037}</MetaDataID>
        public override string Name
        {
            get
            {
                return Relation.RelationName;
            }
            set
            {
            }
        }
    }
}
