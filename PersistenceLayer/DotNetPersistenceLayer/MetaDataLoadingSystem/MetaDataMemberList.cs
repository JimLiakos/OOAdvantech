namespace OOAdvantech.MetaDataLoadingSystem
{
	/// <MetaDataID>{745A5DC2-F8D7-403F-BBC0-E46CF74EA9BD}</MetaDataID>
    public class MetaDataMemberList : Collections.MemberList
	{
        
		/// <MetaDataID>{9C91B6F0-532D-4844-AF34-3351ABDEF4B8}</MetaDataID>
        protected override Collections.Member GetMember(string Index)
		{
			if(Members.Contains(Index))
                return (Collections.Member)Members[Index];
			else
				throw new System.Exception("There isn't Member with name '"+Index+"'.");
		}
		/// <MetaDataID>{7E8FED5F-8390-403B-AC4F-C2DF7AE1D63E}</MetaDataID>
		public MetaDataMemberList()
		{
            Members = new System.Collections.Hashtable();
		}

		internal void AddMember(string MemberName)
		{
			if(Members.Contains(MemberName))
				return;
			else
			{
                Collections.Member mMember = new MetaObjectMember();
				Members[MemberName]=mMember;
				return;
			}
		}
	}
}
