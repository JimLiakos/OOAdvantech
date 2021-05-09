namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

	/// <MetaDataID>{F70FE71D-D8F2-4F79-ABCF-5789DC6420EF}</MetaDataID>
	public class MemberList : PersistenceLayer.MemberList
	{
        System.Collections.ArrayList ObjectTypeMembers = new System.Collections.ArrayList();
        public System.Collections.ArrayList GetObjectTypeMembers()
        {
            return ObjectTypeMembers;
        }
		/// <MetaDataID>{026E454F-5FB5-4E16-BA78-C7F87BF19F34}</MetaDataID>
		internal MetaDataRepository.ObjectQueryLanguage.OQLStatement OQLStatement;
		/// <MetaDataID>{1429C586-E567-47A0-8AEA-4F9C4ABDD1C2}</MetaDataID>
		public System.Data.DataRow DataRecord;
        //public System.Data.DataTable Table;
        public DataNode RootDataNode;
        internal MemberList(DataNode rootDataNode)
        {
//            RootDataNode = rootDataNode;
            OQLStatement = rootDataNode.ObjectQuery;
            //foreach (System.Data.DataTable table in tables)
            //{
            //    if (table.TableName == rootDataNode.Name + rootDataNode.GetHashCode().ToString())
            //    {
            //        Table = table;
            //        break;
            //    }
            //}
            

            Members = new System.Collections.Hashtable();
            if (OQLStatement.SelectListItems.Contains(rootDataNode))
                BuildMemberList(rootDataNode);
            else
            {
                foreach (DataNode subDataNode in rootDataNode.SubDataNodes)
                    BuildMemberList(subDataNode);
            }



        }
        ///// <MetaDataID>{3DA17944-5D0C-4977-A787-FBC01472B808}</MetaDataID>
        //internal MemberList(MetaDataRepository.ObjectQueryLanguage.OQLStatement mOQLStatement,System.Data.DataTable table)
        //{
        //    Table = table;

	 
        //    OQLStatement=mOQLStatement;
        //    Members =new System.Collections.Hashtable();
        //    short FieldID=0;
        //    foreach (DataNode dataNode in OQLStatement.DataTrees)
        //    {
        //        BuildMemberList(dataNode );

        //    }

        //    foreach (System.Collections.DictionaryEntry entry in Members)
        //    {
        //        Member member = entry.Value as Member;
        //        member.LoadRelatedObjects();
        //    }

        //}

        private void BuildMemberList(DataNode dataNode)
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
                &&dataNode.Type == DataNode.DataNodeType.Object
                &&((!(dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany))
                || dataNode == RootDataNode || 
                ((dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd 
                &&(dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association==dataNode.ParentDataNode.Classifier.LinkAssociation
                ))))
            {
                RootDataNode = dataNode;
                Member member = new MemberObject(dataNode, this);
                ObjectTypeMembers.Add(member);
                Members.Add(member.Name,member);
            }
            else if (dataNode.DataNodeMembersParticipateInSelectClause 
                &&((dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)))
            {
                RootDataNode = dataNode.ParentDataNode;
                Member member = new MemberCollection(dataNode, this);
                Members.Add(member.Name, member);
                return;

            }
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                BuildMemberList(subDataNode);
            
            
        }
	
		/// <MetaDataID>{89050CB8-36C8-4C8A-9667-1CBE419E00B2}</MetaDataID>
        protected override PersistenceLayer.Member GetMember(string Index)
        {
            if (Members.Contains(Index))
                return (PersistenceLayer.Member)Members[Index];
            else
                throw new System.Exception("There isn't Member with name '" + Index + "'.");
        }

        internal void CloneDataSet(StructureSet.DataBlock dataBlock)
        {
            bool hasMeberObject = false;
            System.Data.DataTable  clonedTable=null;
            foreach (System.Collections.DictionaryEntry entry in this.Members)
            {
                if (entry.Value is MemberObject)
                {
                    hasMeberObject=true;
                    break;
                }
            }
            if (!hasMeberObject)
            {
                clonedTable = this.RootDataNode.DataSource.DataTable.Copy();
                dataBlock.Data.Tables.Add(clonedTable);
            }
            else
            {
                System.Data.DataRow backupRecord = DataRecord;
                clonedTable = new System.Data.DataTable();
                clonedTable.TableName = this.RootDataNode.DataSource.DataTable.TableName;
                dataBlock.Data.Tables.Add(clonedTable);
                foreach (System.Collections.DictionaryEntry entry in this.Members)
                {
                    Member member = entry.Value as Member;
                    if (member.GetType() == typeof(Member))
                    {
                        System.Data.DataColumn column = this.RootDataNode.DataSource.DataTable.Columns[member.ID];
                        clonedTable.Columns.Add(member.Name, column.DataType);
                    }
                    else if (member is MemberObject)
                    {

                        System.Data.DataColumn objectColumn=clonedTable.Columns.Add(member.Name, typeof(int));
                        dataBlock.ColumnsWithObject.Add(objectColumn.Table.TableName + "_" + objectColumn.ColumnName);
                    }
                 
                }
                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                foreach (System.Data.DataRelation relation in this.RootDataNode.DataSource.DataTable.ParentRelations)
                {
                    foreach (System.Data.DataColumn column in relation.ChildColumns)
                    {
                        System.Data.DataColumn relationColumn = clonedTable.Columns.Add(column.ColumnName, column.DataType);
                        relationColumns.Add(relationColumn.ColumnName);
                    }
                }
                foreach (System.Data.DataRelation relation in this.RootDataNode.DataSource.DataTable.ChildRelations)
                {
                    foreach (System.Data.DataColumn column in relation.ParentColumns)
                    {
                        System.Data.DataColumn relationColumn = clonedTable.Columns.Add(column.ColumnName, column.DataType);
                        relationColumns.Add(relationColumn.ColumnName);
                    }
                }
                foreach (System.Data.DataRow record in this.RootDataNode.DataSource.DataTable.Rows)
                {
                    DataRecord = record;
                    System.Data.DataRow newRecord = clonedTable.NewRow();

                    foreach (System.Collections.DictionaryEntry entry in this.Members)
                    {
                        Member member = entry.Value as Member;
                        if (member.GetType() == typeof(Member))
                        {
                            newRecord[member.Name] = member.Value;
                        }
                        else if (member is MemberObject)
                        {

                            object value = member.Value;
                            if (value != null)
                            {
                                dataBlock.Objects[value.GetHashCode()] = value;
                                newRecord[member.Name] = value.GetHashCode();
                            }
                       }
                    }
                    foreach(string relationColumnName in relationColumns)
                        newRecord[relationColumnName]=DataRecord[relationColumnName];

                    clonedTable.Rows.Add(newRecord);

                }
                DataRecord = backupRecord;
            }

            foreach (System.Data.DataRelation relation in this.RootDataNode.DataSource.DataTable.ParentRelations)
            {
                System.Data.DataTable clonedParentTable = dataBlock.Data.Tables[relation.ParentColumns[0].Table.TableName];
                System.Data.DataColumn[] parentColumns=new System.Data.DataColumn[relation.ParentColumns.Length];
                System.Data.DataColumn[] childColumns=new System.Data.DataColumn[relation.ParentColumns.Length];
                int i=0;
                foreach(System.Data.DataColumn parentColumn in relation.ParentColumns)
                {
                    parentColumns[i]=clonedParentTable.Columns[parentColumn.ColumnName];
                    i++;
                }
                i=0;
                foreach(System.Data.DataColumn childColumn in relation.ChildColumns)
                {
                    childColumns[i] = clonedTable.Columns[childColumn.ColumnName];
                    i++;
                }
                System.Data.DataRelation clonedRelation = new System.Data.DataRelation(relation.RelationName, parentColumns, childColumns);
                dataBlock.Data.Relations.Add(clonedRelation);

            }

            

            foreach (System.Collections.DictionaryEntry entry in this.Members)
            {
                if (entry.Value is MemberCollection)
                    (entry.Value as MemberCollection).Members.CloneDataSet(dataBlock);
            }
            
        }
    }
}
