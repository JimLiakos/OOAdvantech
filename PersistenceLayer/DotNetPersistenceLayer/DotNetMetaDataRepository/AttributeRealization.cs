namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{F875931E-BDB1-4FED-9E14-CB29A26432E7}</MetaDataID>
	public class AttributeRealization : OOAdvantech.MetaDataRepository.AttributeRealization
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{DDB01746-9197-4644-95C7-7CB3F8F7DC1D}</MetaDataID>
		private System.Reflection.FieldInfo _FieldMember;
		/// <MetaDataID>{7D852C7F-E325-4540-8232-5A3D21A74149}</MetaDataID>
		public System.Reflection.FieldInfo FieldMember
		{
			get
			{
				return _FieldMember;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{5BCDEBA0-7BE2-4B6B-8CB7-6274688CE932}</MetaDataID>
		private System.Reflection.PropertyInfo _PropertyMember;
		/// <MetaDataID>{3DEFEF4C-A999-4679-9B20-07B53E645B6D}</MetaDataID>
		public System.Reflection.PropertyInfo PropertyMember
		{
			get
			{
				return _PropertyMember;
			}
		}
		/// <MetaDataID>{AFD78227-5182-4D45-9416-73DE13150281}</MetaDataID>
		private bool FieldMemberLoaded;
		/// <MetaDataID>{C4C7A9D4-C550-4917-845B-F6EE78BC8424}</MetaDataID>
		 public AttributeRealization (System.Reflection.PropertyInfo property, Attribute attribute,MetaDataRepository.Classifier owner)
		{
			_Owner=owner;
			Name=attribute.Name;
			_Specification=attribute;
			attribute.AddAttributeRealization(this);
			 _PropertyMember=property;

		}
	
	}
}
