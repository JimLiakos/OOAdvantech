using System;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <summary>
	/// 
	/// </summary>
	public class MemberList: PersistenceLayer.MemberList
	{
		protected override PersistenceLayer.Member GetMember(string Index)
		{
			if(Members.Contains(Index))
				return (PersistenceLayer.Member)Members[Index];
			else
				throw new System.Exception("There isn't Member with name '"+Index+"'.");
		}
		/// <MetaDataID>{7E8FED5F-8390-403B-AC4F-C2DF7AE1D63E}</MetaDataID>
		public MemberList()
		{
            Members = new System.Collections.Hashtable();
		}

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
