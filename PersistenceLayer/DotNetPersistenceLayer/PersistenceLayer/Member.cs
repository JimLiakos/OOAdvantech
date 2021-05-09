namespace OOAdvantech.Collections
{
	/// <MetaDataID>{A53BE1CF-F521-4E53-AFDF-C36564A317A5}</MetaDataID>
	/// <summary></summary>
    [System.Serializable]
	public abstract class Member
	{
		/// <MetaDataID>{0F18A432-8D13-4C97-875B-AC04653D3F9B}</MetaDataID>
		public abstract System.Type Type
		{
			get;
 
		}
		/// <MetaDataID>{4D0D1234-8481-436A-B3CF-B29D49A02916}</MetaDataID>
		public MemberList Owner;
		
		/// <summary></summary>
		/// <MetaDataID>{D9E4CFD4-24B0-4D31-96AE-FA2C6C997DEE}</MetaDataID>
		public abstract string Name
		{
			get;
			set; 
		}
	
		/// <summary></summary>
		/// <MetaDataID>{D3F49C5A-8E4B-4096-AA56-7E3BDB1BD81F}</MetaDataID>
		public abstract short ID
		{
			get;
			set;
		}
		/// <summary></summary>
		/// <MetaDataID>{2A877E3F-FE10-4F2C-A3B0-DC9794BD7141}</MetaDataID>
		public abstract object Value
		{
			get;
			set;
		}
        /// <MetaDataID>{414aabcd-2241-4ad0-99c7-c015bc8af0d8}</MetaDataID>
        public abstract bool DerivedMember
        {
            get;
        }
	}
}
