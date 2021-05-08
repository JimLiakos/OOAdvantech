namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{65D54BC8-D89D-45E1-835C-AA4B4354D403}</MetaDataID>
	[System.AttributeUsage(System.AttributeTargets.Field|System.AttributeTargets.Property|System.AttributeTargets.Class|System.AttributeTargets.Interface)]
	public class AssociationClass : System.Attribute
	{

		/// <MetaDataID>{10F4F50F-82DA-45D7-BDE0-E078E8022496}</MetaDataID>
		public System.Type AssocciationEndRoleA;
		/// <MetaDataID>{96C325FD-C297-42AB-B104-5289E6035AC9}</MetaDataID>
		public System.Type AssocciationEndRoleB;
		/// <MetaDataID>{002B00A8-AF6A-4AAE-BEF2-5D6B880588C3}</MetaDataID>
		public string AssocciationName;
		/// <MetaDataID>{E20ED7F9-168B-4CD2-9027-8794D9E03211}</MetaDataID>
		public System.Type AssocciationClass;
		/// <MetaDataID>{451D0B67-52DE-488D-A35D-A17126ED62B0}</MetaDataID>
		public AssociationClass(System.Type theAssocciationClass)
		{
			AssocciationClass=theAssocciationClass;
		}

		/// <MetaDataID>{D35063FC-22FC-4997-B89A-F5E5AB47484A}</MetaDataID>
		public AssociationClass(System.Type theAssocciationEndRoleA,System.Type theAssocciationEndRoleB,string theAssocciationName)
		{
			AssocciationEndRoleA=theAssocciationEndRoleA;
			AssocciationEndRoleB=theAssocciationEndRoleB;
			AssocciationName=theAssocciationName;
		}
	}
}
