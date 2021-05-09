namespace OOAdvantech.PersistenceLayerRunTime
{
	using System;
	/// <MetaDataID>{C5E0C140-C95F-4A3C-A412-8060E8F65695}</MetaDataID>
	[System.Serializable]
	public class AssociationEndAgent
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{7120D78F-A86D-444C-8B57-4ECBF903BD71}</MetaDataID>
		private OOAdvantech.DotNetMetaDataRepository.AssociationEnd _RealAssociationEnd;
		/// <MetaDataID>{BCBFC689-04AD-4788-BFFA-38845F225AF5}</MetaDataID>
		public OOAdvantech.DotNetMetaDataRepository.AssociationEnd RealAssociationEnd
		{
			get
			{
				if(_RealAssociationEnd==null||Remoting.RemotingServices.IsOutOfProcess(_RealAssociationEnd))
				{
					MetaDataRepository.MetaObject metaObject=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(Identity);
					if(metaObject!=null)
						_RealAssociationEnd=metaObject as DotNetMetaDataRepository.AssociationEnd;
				}
				return _RealAssociationEnd;
			}
		}


		/// <MetaDataID>{993BD94D-697F-4F11-98C7-15611A3EA9CC}</MetaDataID>
		public readonly MetaDataRepository.MetaObjectID Identity;

        public readonly MetaDataRepository.MetaObjectID AssociationIdentity;

        public readonly MetaDataRepository.MetaObjectID OtherAssociationEndIdentity;

        public readonly bool IsRoleA;

        public readonly string AssociationEndName;

        public readonly string AssociationName;

        public readonly string OtherAssociationEndName;

        public readonly string RoleASpecificationName;

        public readonly MetaDataRepository.MetaObjectID RoleASpecificationIdentity;

        public readonly MetaDataRepository.MetaObjectID RoleASpecificationAssemblyIdentity;


        public readonly string RoleBSpecificationName;

        public readonly MetaDataRepository.MetaObjectID RoleBSpecificationIdentity;

        public readonly MetaDataRepository.MetaObjectID RoleBSpecificationAssemblyIdentity;



        /// <MetaDataID>{CFA22779-FC3D-45B1-906D-CC9300AA8BF4}</MetaDataID>
        public AssociationEndAgent(DotNetMetaDataRepository.AssociationEnd associationEnd)
		{
			if(associationEnd==null)
				throw new System.ArgumentNullException("the parameter associationEnd must be not null");
			_RealAssociationEnd=associationEnd;
			Identity=associationEnd.Identity;
            AssociationEndName = associationEnd.Name;
            AssociationIdentity = associationEnd.Association.Identity;
            AssociationName = associationEnd.Association.Name;
            OtherAssociationEndIdentity = associationEnd.GetOtherEnd().Identity;
            OtherAssociationEndName = associationEnd.GetOtherEnd().Name;
            IsRoleA = associationEnd.IsRoleA;
            if (IsRoleA)
            {
                RoleASpecificationName = associationEnd.Specification.Name;
                RoleASpecificationIdentity = associationEnd.Specification.Identity;
                RoleASpecificationAssemblyIdentity = associationEnd.Specification.ImplementationUnit.Identity;

                RoleBSpecificationName = associationEnd.GetOtherEnd().Specification.Name;
                RoleBSpecificationIdentity = associationEnd.GetOtherEnd().Specification.Identity;
                RoleBSpecificationAssemblyIdentity = associationEnd.GetOtherEnd().Specification.ImplementationUnit.Identity;

            }
            else
            {
                RoleBSpecificationName = associationEnd.Specification.Name;
                RoleBSpecificationIdentity = associationEnd.Specification.Identity;
                RoleBSpecificationAssemblyIdentity = associationEnd.Specification.ImplementationUnit.Identity;

                RoleASpecificationName = associationEnd.GetOtherEnd().Specification.Name;
                RoleASpecificationIdentity = associationEnd.GetOtherEnd().Specification.Identity;
                RoleASpecificationAssemblyIdentity = associationEnd.GetOtherEnd().Specification.ImplementationUnit.Identity;
            }


        }
	}
}
