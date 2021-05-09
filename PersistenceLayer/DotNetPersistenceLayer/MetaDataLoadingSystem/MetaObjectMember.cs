namespace OOAdvantech.MetaDataLoadingSystem
{
	/// <MetaDataID>{2FC45718-D3CE-4B3E-9B08-87C199474221}</MetaDataID>
	public class MetaObjectMember : Collections.Member
	{
		/// <MetaDataID>{685E664E-E375-45CA-BEBC-F984E5B07F9E}</MetaDataID>
		public override System.Type Type
		{
			get
			{
				return null;
			}
		}
		/// <MetaDataID>{A891F00C-857A-4AED-A8B7-15112B9823DE}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private string _Name;
		/// <MetaDataID>{4E21F2F8-BAF9-44A2-B29A-2F6AA2DF44F5}</MetaDataID>
		public override string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name=value;
			}
		}
		/// <MetaDataID>{A55D03A0-F7E3-4DD4-BD7A-19AB3ECEB033}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private short _ID;
		/// <MetaDataID>{C63C09DB-6273-4A0F-831D-265AEE3C2F86}</MetaDataID>
		public override short ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID=value;
			}
		}
		/// <MetaDataID>{D5DC04E9-FE6D-47B7-B358-98948411053C}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private object _Value;
		/// <MetaDataID>{259BE1CB-F793-4FEC-A36D-20D7ABCD10A3}</MetaDataID>
		public override object Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value=value;
			}
		}
	}
}
