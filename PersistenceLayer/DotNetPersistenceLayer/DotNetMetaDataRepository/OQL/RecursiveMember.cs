using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{82ca0d45-6c3a-4849-b376-e45a2bd82111}</MetaDataID>
    [System.Serializable]
    public class RecursiveMember : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Member
    {
        /// <MetaDataID>{c769c404-18cc-44d0-9ced-a34f456b104e}</MetaDataID>
        string RelationName;
                /// <MetaDataID>{77075F2C-DF2E-418F-9966-12670073A48A}</MetaDataID>
        public MemberList Members;

        /// <MetaDataID>{cf27e69e-6059-4a3d-a334-bdacf4e34581}</MetaDataID>
        public RecursiveMember(DataNode dataNode, MemberList owner)
            : base(dataNode)
        {
            MemberMedata = dataNode;
            Owner = owner;
            _Name = dataNode.Name;
            RelationName = "Recursive_" + dataNode.Alias;

            Members = owner;


        }
        /// <MetaDataID>{6e494e66-7477-4853-b23e-f7225a34c1a3}</MetaDataID>
        public override void LoadRelatedObjects()
        {
            foreach (Member member in Members)
                member.LoadRelatedObjects();
        }

        /// <exclude>Excluded</exclude>
        private object _Value;

        /// <MetaDataID>{796ca0e2-9913-43ea-b683-45336148f8c5}</MetaDataID>
        public override object Value
        {
            get
            {
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


                return new StructureSet(Members, MemberMedata.DataSource.GetRelatedRows((Owner as MemberList).DataRecord, MemberMedata).GetEnumerator(), (Owner as MemberList).OQLStatement);

                //if ((Owner as MemberList).DataRecord.Table.ChildRelations.Contains(RelationName))
                //{
                //    return new StructureSet(Members,
                //            (Owner as MemberList).DataRecord.GetChildRows(RelationName).GetEnumerator(),
                //            (Owner as MemberList).OQLStatement);
                //}
                //else
                //{
                //    return new StructureSet(Members,
                //        (Owner as MemberList).DataRecord.GetParentRows(RelationName).GetEnumerator(),
                //        (Owner as MemberList).OQLStatement);
                //}

            }
            set
            {
            }
        }
    }
}
