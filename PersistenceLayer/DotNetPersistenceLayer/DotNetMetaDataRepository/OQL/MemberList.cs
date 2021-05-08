namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

    /// <MetaDataID>{F70FE71D-D8F2-4F79-ABCF-5789DC6420EF}</MetaDataID>
    [System.Serializable]
    public class MemberList : OOAdvantech.Collections.MemberList
    {
        ///// <MetaDataID>{cb1ff243-f9c7-41c4-9e32-e7fde1ed272f}</MetaDataID>
        //internal void RemoveAutoConstructed()
        //{

        //    foreach (System.Collections.DictionaryEntry entry in new System.Collections.Hashtable(Members))
        //    {
        //        if ((entry.Value as Member).MemberMedata.OnConstuctionFetching)
        //            Members.Remove(entry.Key);
        //        if ((entry.Value as Member) is MemberCollection)
        //            ((entry.Value as Member) as MemberCollection).Members.RemoveAutoConstructed();
        //    }

        //}
        /// <MetaDataID>{74EC1D79-54D8-4C27-BA8D-9F74B08BB484}</MetaDataID>
        System.Collections.Generic.List<Member> ObjectTypeMembers = new System.Collections.Generic.List<Member>();
        /// <MetaDataID>{25FE1368-7A47-417D-A0A5-2CA82270A6DB}</MetaDataID>
        public System.Collections.Generic.List<Member> GetObjectTypeMembers()
        {
            return ObjectTypeMembers;
        }
        /// <MetaDataID>{03cb0ebe-c324-4712-b64b-8205b7d76851}</MetaDataID>
        Member OwnerMember;
        /// <MetaDataID>{4425b8ac-8504-4471-81c5-f9b5e57f6bdf}</MetaDataID>
        internal MemberList(DataNode rootDataNode, Member ownerMember)
        {
            OwnerMember = ownerMember;
            OQLStatement = rootDataNode.ObjectQuery;
            RootDataNode = rootDataNode;
            Members = new System.Collections.Generic.Dictionary<string, Collections.Member>();

            BuildMemberList(rootDataNode);
            foreach (DataNode selectionDataNode in OQLStatement.SelectListItems)
            {
                if (selectionDataNode.IsPathNode(rootDataNode) && !selectionDataNode.MembersFetchingObjectActivation)
                {
                    DataPath dataPath = new DataPath();
                    DataNode parent = selectionDataNode.ParentDataNode;
                    if (selectionDataNode != rootDataNode && selectionDataNode.Type != DataNode.DataNodeType.OjectAttribute)
                    {

                        //if (selectionDataNode.AssignedMetaObject is AssociationEnd &&
                        //       selectionDataNode.ParentDataNode.DataSource.RelationshipsData.ContainsKey(selectionDataNode.AssignedMetaObject.Identity.ToString()))
                        //    dataPath.Push(selectionDataNode.Alias + "_AssociationTable");

                        //dataPath.Push(selectionDataNode.Alias);
                        dataPath.Push(selectionDataNode);


                    }
                    while (parent != rootDataNode)
                    {
                        
                        {

                            //if (parent.AssignedMetaObject is AssociationEnd &&
                            //       parent.ParentDataNode.DataSource.RelationshipsData.ContainsKey(parent.AssignedMetaObject.Identity.ToString()))
                            //{
                            //    dataPath.Push(parent.Alias + "_AssociationTable");
                            //}
                            //dataPath.Push(parent.Alias);

                            dataPath.Push(parent);

                        }
                        parent = parent.ParentDataNode;


                    }


                    if (selectionDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        Member member = new Member(selectionDataNode, this, dataPath, MemberValueIsStructureSet(selectionDataNode));

                        if (!Members.ContainsKey(member.Name))
                            Members.Add(member.Name, member);
                    }

                    if (selectionDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        Member member = new MemberObject(selectionDataNode, this, dataPath, MemberValueIsStructureSet(selectionDataNode));

                        if (!Members.ContainsKey(member.Name))
                            Members.Add(member.Name, member);
                    }

                }
            }
            //bool hasLockRequest = HasLockRequest;


        }
        /// <MetaDataID>{026E454F-5FB5-4E16-BA78-C7F87BF19F34}</MetaDataID>
        [System.NonSerialized]
        internal MetaDataRepository.ObjectQueryLanguage.ObjectQuery OQLStatement;
        /// <MetaDataID>{1429C586-E567-47A0-8AEA-4F9C4ABDD1C2}</MetaDataID>
        [System.NonSerialized]
        public IDataRow DataRecord;
        //public System.Data.DataTable Table;
        /// <MetaDataID>{064FEC4A-BA32-4C30-B9A5-57A7617B3147}</MetaDataID>
        public DataNode RootDataNode;
        /// <MetaDataID>{D2C4F5C2-4702-4DEE-9332-3069C2D21107}</MetaDataID>
        internal MemberList(DataNode rootDataNode)
        {

            OQLStatement = rootDataNode.ObjectQuery;
            while (rootDataNode.Type == DataNode.DataNodeType.Namespace)
                rootDataNode = rootDataNode.SubDataNodes[0];

            RootDataNode = rootDataNode;
            Members = new System.Collections.Generic.Dictionary<string, Collections.Member>();
            BuildMemberList(rootDataNode);

            foreach (DataNode selectionDataNode in OQLStatement.SelectListItems)
            {
                if (selectionDataNode.IsPathNode(rootDataNode) && !selectionDataNode.MembersFetchingObjectActivation)
                {
                    DataPath dataPath = new DataPath();

                    DataNode parent = selectionDataNode.ParentDataNode;
                    if (selectionDataNode.Type == DataNode.DataNodeType.Object)//if (parent != rootDataNode && selectionDataNode.Type!=DataNode.DataNodeType.OjectAttribute)
                    {

                        //if (selectionDataNode.AssignedMetaObject is AssociationEnd &&
                        //       selectionDataNode.ParentDataNode.DataSource.RelationshipsData.ContainsKey(selectionDataNode.AssignedMetaObject.Identity.ToString()))
                        //    dataPath.Push(selectionDataNode.Alias + "_AssociationTable");
                        //dataPath.Push(selectionDataNode.Alias);
                        dataPath.Push(selectionDataNode);
                        
                    }

                    while (parent != rootDataNode)
                    {
                        //TODO να ελχθεί αν ορθος έφυγε το if
                        //if (parent.ParentDataNode != rootDataNode)
                        {

                            //if (parent.AssignedMetaObject is AssociationEnd &&
                            //       parent.ParentDataNode.DataSource.RelationshipsData.ContainsKey(parent.AssignedMetaObject.Identity.ToString()))
                            //{
                            //    dataPath.Push(parent.Alias + "_AssociationTable");
                            //}
                            //dataPath.Push(parent.Alias);
                            dataPath.Push(parent);
                        }
                        parent = parent.ParentDataNode;
                    }


                    if (selectionDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        Member member = new Member(selectionDataNode, this, dataPath, MemberValueIsStructureSet(selectionDataNode));
                        if (!Members.ContainsKey(member.Name))
                            Members.Add(member.Name, member);
                    }

                    if (selectionDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        Member member = new MemberObject(selectionDataNode, this, dataPath, MemberValueIsStructureSet(selectionDataNode));
                        if (!Members.ContainsKey(member.Name))
                            Members.Add(member.Name, member);
                    }

                }
            }
            //bool hasLockRequest = HasLockRequest;

        }
        /// <MetaDataID>{976abbbe-f778-4a37-b408-ae6ed0d6601e}</MetaDataID>
        bool MemberValueIsStructureSet(DataNode selectionDataNode)
        {

            if (selectionDataNode == RootDataNode)
                return false;
            if (selectionDataNode.AssignedMetaObject is AssociationEnd && (selectionDataNode.AssignedMetaObject as AssociationEnd).Multiplicity.IsMany)
                return true;
            else
                return MemberValueIsStructureSet(selectionDataNode.ParentDataNode);

        }


        /// <MetaDataID>{5A0790C3-F019-4A2C-90D3-5E8965BF89CD}</MetaDataID>
        private void BuildMemberList(DataNode dataNode)
        {
            //if (dataNode.CountSelect)
            //{
            //    Member member = new Member(dataNode, this);
            //    Members.Add(member.Name, member);
            //    return;
            //}

            if (RootDataNode != dataNode)
            {
                if (OQLStatement.SelectListItems.Contains(dataNode) && dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {


                    if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                    {

                    }
                    else
                    {
                        Member member = new Member(dataNode, this);
                        Members.Add(member.Name, member);
                        if (RootDataNode == null)
                            RootDataNode = dataNode.ParentDataNode;

                    }

                }
                else if (OQLStatement.SelectListItems.Contains(dataNode)
                    && dataNode.ParentDataNode != null && dataNode.ParentDataNode.Type != DataNode.DataNodeType.Object
                    && dataNode.Type == DataNode.DataNodeType.Object
                    && (!(dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    || dataNode == RootDataNode))
                {
                    RootDataNode = dataNode;
                    Member member = new MemberObject(dataNode, this);
                    ObjectTypeMembers.Add(member);
                    Members.Add(member.Name, member);
                }
                else if (dataNode.BranchParticipateInSelectClause
                    && dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    if (dataNode.MembersFetchingObjectActivation)// && (dataNode.DataSource.DataTable == null || dataNode.DataSource.DataTable.Rows.Count == 0))
                        return;
                    RootDataNode = dataNode.ParentDataNode;
                    Member member = new MemberCollection(dataNode, this);
                    Members.Add(member.Name, member);
                    return;

                }
                else if (dataNode.BranchParticipateInSelectClause
               && dataNode.ParentDataNode != null && dataNode.ParentDataNode.Type == DataNode.DataNodeType.Object
               && dataNode.Type == DataNode.DataNodeType.Object)
                {
                    if (dataNode.MembersFetchingObjectActivation )//&& (dataNode.DataSource.DataTable == null || dataNode.DataSource.DataTable.Rows.Count == 0))
                        return;
                    RootDataNode = dataNode.ParentDataNode;
                    Member member = new MemberCollection(dataNode, this);
                    Members.Add(member.Name, member);
                    return;

                }
                else if (dataNode.Type == DataNode.DataNodeType.Count)
                {

                    Member member = new Member(dataNode, this);
                    Members.Add(member.Name, member);
                    if (RootDataNode == null)
                        RootDataNode = dataNode.ParentDataNode;

                    //DataNode rootDataNode = dataNode.ParentDataNode;
                    //DataNode aggregateExpressionDataNode = dataNode.AggregateExpressionDataNode;
                    //DataPath dataPath = new DataPath();
                    //DataNode parent = dataNode.AggregateExpressionDataNode.ParentDataNode;
                    //if (aggregateExpressionDataNode != rootDataNode && aggregateExpressionDataNode.Type != DataNode.DataNodeType.OjectAttribute)
                    //{
                    //    if (aggregateExpressionDataNode.AssignedMetaObject is AssociationEnd &&
                    //           aggregateExpressionDataNode.ParentDataNode.DataSource.RelationshipsData.ContainsKey(aggregateExpressionDataNode.AssignedMetaObject.Identity.ToString()))
                    //        dataPath.Push(aggregateExpressionDataNode.Alias + "_AssociationTable");
                    //    dataPath.Push(aggregateExpressionDataNode.Alias);
                    //}
                    //while (parent != rootDataNode)
                    //{
                    //    if (parent.AssignedMetaObject is AssociationEnd &&
                    //           parent.ParentDataNode.DataSource.RelationshipsData.ContainsKey(parent.AssignedMetaObject.Identity.ToString()))
                    //    {
                    //        dataPath.Push(parent.Alias + "_AssociationTable");
                    //    }
                    //    dataPath.Push(parent.Alias);
                    //    parent = parent.ParentDataNode;
                    //}


                    //if (aggregateExpressionDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    //{
                    //    Member member = new Member(aggregateExpressionDataNode, this, dataPath, MemberValueIsStructureSet(aggregateExpressionDataNode));

                    //    if (!Members.ContainsKey(member.Name))
                    //        Members.Add(member.Name, member);
                    //}

                    //if (aggregateExpressionDataNode.Type == DataNode.DataNodeType.Object)
                    //{
                    //    Member member = new MemberObject(dataNode, this, dataPath, MemberValueIsStructureSet(aggregateExpressionDataNode));

                    //    if (!Members.ContainsKey(member.Name))
                    //        Members.Add(member.Name, member);
                    //}


                }

                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    BuildMemberList(subDataNode);

            }
            else
            {
                if (RootDataNode.Recursive)
                    Members.Add(RootDataNode.Name, new RecursiveMember(RootDataNode, this));


                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    BuildMemberList(subDataNode);

                if (OQLStatement.SelectListItems.Contains(dataNode))
                {
                    Member member = new MemberObject(dataNode, this);
                    ObjectTypeMembers.Add(member);
                    Members.Add(member.Name, member);
                    if (!Members.ContainsKey("Object"))
                        Members.Add("Object", member);
                }

            }
        }

        /// <MetaDataID>{89050CB8-36C8-4C8A-9667-1CBE419E00B2}</MetaDataID>
        protected override OOAdvantech.Collections.Member GetMember(string Index)
        {
            if (Members.ContainsKey(Index))
                return (Collections.Member)Members[Index];
            else if (RootDataNode.Recursive)
            {
                return (Collections.Member)Members[Index];

            }
            else
                throw new System.Exception("There isn't Member with name '" + Index + "'.");
        }

        //internal void CloneDataSet(Collections.StructureSet.DataBlock dataBlock)
        //{
        //    bool hasMeberObject = false;
        //    System.Data.DataTable  clonedTable=null;
        //    foreach (System.Collections.DictionaryEntry entry in this.Members)
        //    {
        //        if (entry.Value is MemberObject)
        //        {
        //            hasMeberObject=true;
        //            break;
        //        }
        //    }
        //    if (!hasMeberObject)
        //    {
        //        clonedTable = this.RootDataNode.DataSource.DataTable.Copy();
        //        dataBlock.Data.Tables.Add(clonedTable);
        //    }
        //    else
        //    {
        //        System.Data.DataRow backupRecord = DataRecord;
        //        clonedTable = new System.Data.DataTable();
        //        clonedTable.TableName = this.RootDataNode.DataSource.DataTable.TableName;
        //        dataBlock.Data.Tables.Add(clonedTable);
        //        foreach (System.Collections.DictionaryEntry entry in this.Members)
        //        {
        //            Member member = entry.Value as Member;
        //            if (member.GetType() == typeof(Member))
        //            {
        //                System.Data.DataColumn column = this.RootDataNode.DataSource.DataTable.Columns[member.ID];
        //                clonedTable.Columns.Add(member.Name, column.DataType);
        //            }
        //            else if (member is MemberObject)
        //            {

        //                System.Data.DataColumn objectColumn=clonedTable.Columns.Add(member.Name, typeof(int));
        //                dataBlock.ColumnsWithObject.Add(objectColumn.Table.TableName + "_" + objectColumn.ColumnName);
        //            }

        //        }
        //        System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
        //        foreach (System.Data.DataRelation relation in this.RootDataNode.DataSource.DataTable.ParentRelations)
        //        {
        //            foreach (System.Data.DataColumn column in relation.ChildColumns)
        //            {
        //                System.Data.DataColumn relationColumn = clonedTable.Columns.Add(column.ColumnName, column.DataType);
        //                relationColumns.Add(relationColumn.ColumnName);
        //            }
        //        }
        //        foreach (System.Data.DataRelation relation in this.RootDataNode.DataSource.DataTable.ChildRelations)
        //        {
        //            foreach (System.Data.DataColumn column in relation.ParentColumns)
        //            {
        //                System.Data.DataColumn relationColumn = clonedTable.Columns.Add(column.ColumnName, column.DataType);
        //                relationColumns.Add(relationColumn.ColumnName);
        //            }
        //        }
        //        foreach (System.Data.DataRow record in this.RootDataNode.DataSource.DataTable.Rows)
        //        {
        //            DataRecord = record;
        //            System.Data.DataRow newRecord = clonedTable.NewRow();

        //            foreach (System.Collections.DictionaryEntry entry in this.Members)
        //            {
        //                Member member = entry.Value as Member;
        //                if (member.GetType() == typeof(Member))
        //                {
        //                    newRecord[member.Name] = member.Value;
        //                }
        //                else if (member is MemberObject)
        //                {

        //                    object value = member.Value;
        //                    if (value != null)
        //                    {
        //                        dataBlock.Objects[value.GetHashCode()] = value;
        //                        newRecord[member.Name] = value.GetHashCode();
        //                    }
        //               }
        //            }
        //            foreach(string relationColumnName in relationColumns)
        //                newRecord[relationColumnName]=DataRecord[relationColumnName];

        //            clonedTable.Rows.Add(newRecord);

        //        }
        //        DataRecord = backupRecord;
        //    }

        //    foreach (System.Data.DataRelation relation in this.RootDataNode.DataSource.DataTable.ParentRelations)
        //    {
        //        System.Data.DataTable clonedParentTable = dataBlock.Data.Tables[relation.ParentColumns[0].Table.TableName];
        //        System.Data.DataColumn[] parentColumns=new System.Data.DataColumn[relation.ParentColumns.Length];
        //        System.Data.DataColumn[] childColumns=new System.Data.DataColumn[relation.ParentColumns.Length];
        //        int i=0;
        //        foreach(System.Data.DataColumn parentColumn in relation.ParentColumns)
        //        {
        //            parentColumns[i]=clonedParentTable.Columns[parentColumn.ColumnName];
        //            i++;
        //        }
        //        i=0;
        //        foreach(System.Data.DataColumn childColumn in relation.ChildColumns)
        //        {
        //            childColumns[i] = clonedTable.Columns[childColumn.ColumnName];
        //            i++;
        //        }
        //        System.Data.DataRelation clonedRelation = new System.Data.DataRelation(relation.RelationName, parentColumns, childColumns);
        //        dataBlock.Data.Relations.Add(clonedRelation);

        //    }



        //    foreach (System.Collections.DictionaryEntry entry in this.Members)
        //    {
        //        if (entry.Value is MemberCollection)
        //            (entry.Value as MemberCollection).Members.CloneDataSet(dataBlock);
        //    }

        //}
    }
}
