namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
    /// <MetaDataID>{CC3902F0-D561-4B4F-BD78-AEB281A3780C}</MetaDataID>
    public class MemberCollection : OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage.Member
    {
        StructureSet structureSet;
        public MemberCollection(DataNode dataNode,MemberList owner)
        {
            MemberMedata = dataNode;
            Owner = owner;
            _Name = dataNode.Name;
            Members = new MemberList(dataNode);
	
        }
        public override void LoadRelatedObjects()
        {
            foreach (Member member in Members)
                member.LoadRelatedObjects();
        }

        public MemberList Members;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DBD315B8-8BA2-455B-B369-B1F4D232A90F}</MetaDataID>
        private object _Value;
        /// <MetaDataID>{DB4C52B8-5B3F-4634-BA6F-FC7C3304614B}</MetaDataID>
        public override object Value
        {
            get
            {
                return new StructureSet(Members,
                    (Owner as MemberList).DataRecord.GetChildRows(MemberMedata.Alias).GetEnumerator());
            }
            set
            {
            }
        }
    }
}
