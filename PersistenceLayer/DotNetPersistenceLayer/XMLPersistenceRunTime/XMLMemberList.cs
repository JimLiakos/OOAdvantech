namespace OOAdvantech.XMLPersistenceRunTime
{
	/// <MetaDataID>{1CB2BB91-DDEB-46F1-A221-DA778A2509C1}</MetaDataID>
	/// <summary></summary>
	public class MemberList : PersistenceLayer.MemberList
	{


        /// <MetaDataID>{B2D23291-8F4E-44EA-B67D-4584534368B5}</MetaDataID>
		protected override PersistenceLayer.Member GetMember(string Index)
		{
			if(Members.Contains(Index))
				return (PersistenceLayer.Member)Members[Index];
			else
				throw new System.Exception("There isn't Member with name '"+Index+"'.");
		}

        /// <MetaDataID>{F60F1A11-0145-45AC-932C-B446ECC8206D}</MetaDataID>
        public MemberList()
		{
			Members=new System.Collections.Hashtable();
		}

        /// <MetaDataID>{21EDD247-88C3-48CF-A3D9-E1348EEF0945}</MetaDataID>
		internal void AddMember(string MemberName)
		{
			if(Members.Contains(MemberName))
				return;
			else
			{
				PersistenceLayer.Member mMember=new ObjectMember();
				Members[MemberName]=mMember;
				return;
			}
		}
	}
}
