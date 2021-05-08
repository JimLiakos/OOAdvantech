namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    /// <MetaDataID>{CC3902F0-D561-4B4F-BD78-AEB281A3780C}</MetaDataID>
    [System.Serializable]
    public class MemberCollection : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Member
    {
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(1)]
        [Association("", typeof(Collections.Member), Roles.RoleA, "02da183a-c778-4e43-b41b-83a2eef8d4ae")]
        [IgnoreErrorCheck]
        public MemberList Members;
        /// <MetaDataID>{c6155e5f-30de-4d2a-bf65-b1a97d7339a5}</MetaDataID>
        bool HasAssoctiotionTable;
        /// <MetaDataID>{51573475-9f85-499d-b414-2c5a9790eb51}</MetaDataID>
        DataNode RelationName;
        /// <MetaDataID>{B25B83E6-B1D4-4AAE-93C3-46BD40A6941F}</MetaDataID>
        [System.NonSerialized]
        Collections.StructureSet structureSet;
        /// <MetaDataID>{5DFCCF21-A7C9-43FB-A086-35CB9504628E}</MetaDataID>
        public MemberCollection(DataNode dataNode, MemberList owner)
            : base(dataNode)
        {
            MemberMedata = dataNode;
            Owner = owner;
            if (dataNode.Type == DataNode.DataNodeType.Count && !string.IsNullOrEmpty(dataNode.Alias))
                _Name = dataNode.Alias;
            else
                _Name = dataNode.Name;
            //RelationName = dataNode.Alias;
            RelationName = dataNode;

            Members = new MemberList(dataNode, this);

            if (dataNode.AssignedMetaObject is AssociationEnd &&
               dataNode.ParentDataNode.DataSource.RelationshipsData.ContainsKey(dataNode.Identity) && dataNode.ParentDataNode.DataSource.RelationshipsData[dataNode.Identity].AssotiationTableRelationshipData != null)
                HasAssoctiotionTable = true;

            bool hasLockRequest = HasLockRequest;

        }

        /// <MetaDataID>{70636368-D0A6-455D-877E-192659FF3243}</MetaDataID>
        public override void LoadRelatedObjects()
        {
            foreach (Member member in Members)
                member.LoadRelatedObjects();
        }

        ///// <MetaDataID>{77075F2C-DF2E-418F-9966-12670073A48A}</MetaDataID>
        //public MemberList Members;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DBD315B8-8BA2-455B-B369-B1F4D232A90F}</MetaDataID>
        private object _Value;
        /// <MetaDataID>{DB4C52B8-5B3F-4634-BA6F-FC7C3304614B}</MetaDataID>
        public override object Value
        {
            get
            {
                return new StructureSet(Members, MemberMedata.ParentDataNode.DataSource.GetRelatedRows((Owner as MemberList).DataRecord, RelationName).GetEnumerator(), (Owner as MemberList).OQLStatement);
                //if (HasAssoctiotionTable)
                //{
                //    System.Data.DataRow[] associationRows = (Owner as MemberList).DataRecord.GetChildRows(RelationName);
                //    System.Data.DataRow[] childRows = new System.Data.DataRow[associationRows.Length];
                //    int i = 0;

                //    string assotiationTableReationName = RelationName + "_AssociationTable";
                //    foreach (System.Data.DataRow associationRow in associationRows)
                //    {
                //        childRows[i++] = associationRow.GetParentRow(assotiationTableReationName);
                //    }
                //    return new StructureSet(Members, childRows.GetEnumerator(), (Owner as MemberList).OQLStatement);
                //}
                //else
                //{
                //    if ((Owner as MemberList).DataRecord.Table.ChildRelations.Contains(RelationName))
                //    {
                //        return new StructureSet(Members,
                //                (Owner as MemberList).DataRecord.GetChildRows(RelationName).GetEnumerator(),
                //                (Owner as MemberList).OQLStatement);
                //    }
                //    else
                //    {
                //        return new StructureSet(Members,
                //            (Owner as MemberList).DataRecord.GetParentRows(RelationName).GetEnumerator(),
                //            (Owner as MemberList).OQLStatement);
                //    }
                //}
            }
            set
            {
            }
        }
    }
}
