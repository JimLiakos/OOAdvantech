namespace OOAdvantech.PersistenceLayerRunTime.ClientSide
{
	/// <MetaDataID>{D82E6816-CB5B-461E-A8C7-39BE8BC81C7E}</MetaDataID>
	public class MemberListAgent : PersistenceLayer.MemberList
	{
		/// <MetaDataID>{71107394-11BB-4072-9E48-C2D8F1AA99BC}</MetaDataID>
		protected override PersistenceLayer.Member GetMember(string Index)
		{
			if(Members.Contains(Index))
				return (PersistenceLayer.Member)Members[Index];
			else
				throw new System.Exception("There isn't Member with name '"+Index+"'.");
		}
		/// <MetaDataID>{C36AB2CD-6010-456A-A701-66E6F011374F}</MetaDataID>
		 public MemberListAgent(StructureSet.DataBlock dataSource,System.Data.DataTable dataTable,System.Collections.IEnumerator rowEnumerator)
		{
			if(Members==null)
				Members=new System.Collections.Hashtable();
            foreach (System.Data.DataColumn CurrColumn in dataTable.Columns)
            {
                MemberAgent member = new MemberAgent(CurrColumn.ColumnName, dataSource, rowEnumerator);
                Members.Add(member.Name, member);
            }
            foreach (System.Data.DataRelation relation in dataTable.ChildRelations)
            {
                PersistenceLayer.Member member = new MemberCollectionAgent(dataSource, relation, rowEnumerator);
                Members.Add(member.Name, member);


            }

		}
	}
}
