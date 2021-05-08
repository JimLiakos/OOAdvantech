namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{AFC62556-A026-49FC-9EAD-006FCC4D810D}</MetaDataID>
	public class AssociationClassRole : System.Attribute
	{
		/// <MetaDataID>{BCD5F6EB-53D0-4FEC-B49B-E9694AF85E9B}</MetaDataID>
		public AssociationClassRole(Roles role, string implMemberName)
		{
			if(role==Roles.RoleA)
				IsRoleA=true;
			else
				IsRoleA=false;

			ImplMemberName=implMemberName;
		}

		/// <MetaDataID>{5374FA9E-727D-4C6F-82BB-24BC237B0C80}</MetaDataID>
		public AssociationClassRole(Roles role)
		{
			if(role==Roles.RoleA)
				IsRoleA=true;
			else
				IsRoleA=false;
		}

		/// <MetaDataID>{7CDCD47D-11A5-4960-960F-624EA70D6F5F}</MetaDataID>
		public bool IsRoleA;
		/// <MetaDataID>{5F7B9339-A38A-4D62-9F84-C1F422AAC9DD}</MetaDataID>
		public string ImplMemberName;

	}
}
