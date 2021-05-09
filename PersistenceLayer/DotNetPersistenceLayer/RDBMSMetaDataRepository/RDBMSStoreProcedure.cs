namespace OOAdvantech.RDBMSMetaDataRepository
{
	using MetaDataRepository;
	/// <MetaDataID>{3EDE05B9-641C-41C6-9612-85144D66DD51}</MetaDataID>
	[BackwardCompatibilityID("{3EDE05B9-641C-41C6-9612-85144D66DD51}")]
	[Persistent()]
	public class StoreProcedure : MetaDataRepository.Operation
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(Type))
            {
                if (value == null)
                    Type = default(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure.Types);
                else
                    Type = (OOAdvantech.RDBMSMetaDataRepository.StoreProcedure.Types)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ActsOnStorageCell))
            {
                if (value == null)
                    ActsOnStorageCell = default(OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                else
                    ActsOnStorageCell = (OOAdvantech.RDBMSMetaDataRepository.StorageCell)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(Type))
                return Type;

            if (member.Name == nameof(ActsOnStorageCell))
                return ActsOnStorageCell;


            return base.GetMemberValue(token, member);
        }

        //Error prone Thread Safety
        /// <MetaDataID>{B091B0F8-0FC6-4EA9-BA9C-9C7AB78261E2}</MetaDataID>
        [BackwardCompatibilityID("+9")]
		[PersistentMember()]
		public Types Type;
	
		public enum Types{New=1,Update,Delete};

        /// <MetaDataID>{8195bd92-598a-4c5d-ad0a-d038aa716fc4}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    MetaDataRepository.MetaObjectID identity = null;
                    //if (_DataBaseViewName != null && _DataBaseViewName.Trim().Length > 0)
                    //{
                    //    if (Namespace != null)
                    //        identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + _DataBaseViewName);
                    //    else
                    //        identity = new MetaDataRepository.MetaObjectID(_DataBaseViewName);
                    //}
                    //else
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + Name);
                        else
                            identity = new MetaDataRepository.MetaObjectID(Name);
                        identity = new MetaObjectID(identity.ToString().ToLower().Trim());


                        return identity;
                    }
                    if (base.Identity == null || identity.ToString() != base.Identity.ToString())
                    {
                        _Identity = null;
                        //using (OOAdvantech.Transactions.ObjectStateTransition stateTransition=new OOAdvantech.Transactions.ObjectStateTransition(this))
                        //{
                        SetIdentity(identity);
                        //stateTransition.Consistent=true;
                        //}
                    }
                    return _Identity;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

		/// <MetaDataID>{B2B29997-1A01-4D67-BF2C-56E9E6B696D5}</MetaDataID>
		[Association("ActsOn",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell),Roles.RoleA,"{FA9C8CE2-48E7-4980-8ECC-F93EF497766F}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.LazyFetching)]
		[PersistentMember("ActsOnStorageCell")]
		[RoleAMultiplicityRange(1,1)]
		[RoleBMultiplicityRange(0)]
		public StorageCell ActsOnStorageCell;
		/// <MetaDataID>{2E4311C4-C028-4962-97EA-78F6E630F6E0}</MetaDataID>
		 public StoreProcedure()
		{

		}

		/// <MetaDataID>{A8A0B2A2-DB7E-473F-B0E0-CC1F718C355B}</MetaDataID>
		public StoreProcedure(StorageCell actsOnStorageCell, Types type)
		{
            Type=type;
			ActsOnStorageCell=actsOnStorageCell;
		}

	}
}
