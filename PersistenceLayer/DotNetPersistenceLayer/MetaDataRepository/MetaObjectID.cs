namespace OOAdvantech.MetaDataRepository
{
	using System;
	/// <MetaDataID>{C8679E0C-B243-4D4E-8A7B-862627569037}</MetaDataID>
	//[System.AttributeUsage(System.AttributeTargets.All)]//.Class|System.AttributeTargets.Field|System.AttributeTargets.Property)]
	[Serializable()]
	public class MetaObjectID //: System.Attribute
	{
        /// <MetaDataID>{cd44e99c-4a0a-41e3-bd1b-ac6762c43b0d}</MetaDataID>
        public override int GetHashCode()
        {
            if (ID != null)
                return ID.GetHashCode();
            return base.GetHashCode();

        }
        /// <MetaDataID>{4c1d5325-861a-428e-a7ff-dabb6fe1351b}</MetaDataID>
        public override bool Equals(object obj)
        {
            if (obj is MetaDataRepository.MetaObjectID)
                return this == (obj as MetaDataRepository.MetaObjectID);
            return base.Equals(obj);
        }
		/// <MetaDataID>{7571FC6B-DBEC-4EB6-855C-1FA1C72C536A}</MetaDataID>
		public MetaObjectID(string theID)
		{
			if(theID==null||theID.Length==0)
				theID=Guid.NewGuid().ToString();

			 int ll=0;
			 if(theID=="StorageCellID")
				 ll++;

			ID=theID;
		}
		/// <MetaDataID>{C12D434E-54EC-4A9E-9D8D-7A4D9AFFC801}</MetaDataID>
		private string ID;
		/// <MetaDataID>{0E796CA0-F5F8-4C28-8EC7-FF9B662DEB05}</MetaDataID>
		public override string ToString()
		{
			return ID;
		}
		/// <MetaDataID>{5CD37B25-5C17-406F-B2E4-19DAF4869928}</MetaDataID>
		public static bool operator==( MetaObjectID Left, MetaObjectID Right)
		{

				
			if((object)Left==null)
				return (object)Left==(object)Right;
			if((object)Right==null)
				return false;
			if((object)Left==(object)Right)
				return true;

			return Left.ToString()==Right.ToString();
		}
		/// <MetaDataID>{D1CAF3F6-C7BD-4DCC-9722-7A7BA2DEAA31}</MetaDataID>
		public static bool operator!=( MetaObjectID Left, MetaObjectID Right)
		{
			if(Left==Right)
				return false;
			else return true;


		}

	}
}
