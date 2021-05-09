namespace OOAdvantech.RDBMSMetaDataRepository
{
	using OOAdvantech.MetaDataRepository;

    /// <MetaDataID>{84964fc3-7c68-4ee2-9e7d-4334b2889e47}</MetaDataID>
	public enum GeneralizationMappingType
	{
		/// <summary>
		/// </summary>
		NotItialized				= 0,
		OneTablePerHierarchy		= 1,
		OneTablePerConcreteClass	= 2,
		OneTablePerClass			= 3
	}
	/// <MetaDataID>{DF728A38-96A3-4D03-9463-FE82AD041E10}</MetaDataID>
	[BackwardCompatibilityID("{DF728A38-96A3-4D03-9463-FE82AD041E10}")]
	[Persistent()]
	public class Generalization : MetaDataRepository.Generalization
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(GeneralizationMappingType))
            {
                if (value == null)
                    GeneralizationMappingType = default(OOAdvantech.RDBMSMetaDataRepository.GeneralizationMappingType);
                else
                    GeneralizationMappingType = (OOAdvantech.RDBMSMetaDataRepository.GeneralizationMappingType)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ForeignKeys))
            {
                if (value == null)
                    ForeignKeys = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Key>);
                else
                    ForeignKeys = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Key>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(GeneralizationMappingType))
                return GeneralizationMappingType;

            if (member.Name == nameof(ForeignKeys))
                return ForeignKeys;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{56AFF960-4A34-4275-9751-E6B26C2DF609}</MetaDataID>
        public void BuildMappingElement()
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(GeneralizationMappingType==GeneralizationMappingType.OneTablePerClass)
				{
					foreach(Table CurrTable in ((Class)Parent).ActiveStorageCell.MappedTables)
					{
						if(!((Class)Child).ActiveStorageCell.MappedTables.Contains(CurrTable))
							((Class)Child).ActiveStorageCell.AddMappedTable(CurrTable);
					}
					foreach(Key CurrKey in ForeignKeys)
					{
						if(CurrKey.ReferedTable==((Class)Parent).ActiveStorageCell.MainTable&&
							CurrKey.OriginTable==((Class)Child).ActiveStorageCell.MainTable)
						{
							return;
						}
					}
					string ForeignKeyName="FK";
					ForeignKeyName+="INH_";
					ForeignKeyName+=((Class)Child).ActiveStorageCell.MainTable.Name;
					ForeignKeyName+="_"+((Class)Parent).ActiveStorageCell.MainTable.Name;

					Key ForeignKey=(Key)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(Key));
					ForeignKey.Name = ForeignKeyName;
					ForeignKey.ReferedTable=((Class)Parent).ActiveStorageCell.MainTable;
					((Class)Child).ActiveStorageCell.MainTable.AddForeignKey(ForeignKey);
					ForeignKey.OriginTable=((Class)Child).ActiveStorageCell.MainTable;
					ForeignKeys.Add(ForeignKey);
				
					foreach(Column CurrColumn in ForeignKey.ReferedTable.ObjectIDColumns)
						ForeignKey.AddReferedColumn(CurrColumn);
					foreach(Column CurrColumn in ForeignKey.OriginTable.ObjectIDColumns)
						ForeignKey.AddColumn(CurrColumn);
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		
		}
		//Error prone Thread Safety
		/// <MetaDataID>{B85475AA-58A5-4622-82E0-37DEE8FC0788}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember()]
		public GeneralizationMappingType GeneralizationMappingType;
		/// <MetaDataID>{37D5812D-622B-413D-B799-19FD673BA378}</MetaDataID>
		public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
		{
			base.Synchronize(OriginMetaObject);
		}
		//Error prone Thread Safety
		/// <MetaDataID>{B2340276-09D3-4BB1-9F2B-2910551E0F40}</MetaDataID>
		[Association("GeneralizationForeignKey",typeof(OOAdvantech.RDBMSMetaDataRepository.Key),Roles.RoleA,"{2D86E525-9563-4908-AF4A-F269FDD7B5AC}")]
		[PersistentMember("ForeignKeys")]
		[RoleAMultiplicityRange(0)]
		[RoleBMultiplicityRange(1,1)]
		public Collections.Generic.Set<Key> ForeignKeys;
	}
}
