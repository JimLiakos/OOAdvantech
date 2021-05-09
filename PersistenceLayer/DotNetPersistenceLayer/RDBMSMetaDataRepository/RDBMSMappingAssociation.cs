namespace OOAdvantech.RDBMSMetaDataRepository
{
	using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
	/// <MetaDataID>{1B995D14-57A7-43BF-981B-4E4B1A14E7EA}</MetaDataID>
	[BackwardCompatibilityID("{1B995D14-57A7-43BF-981B-4E4B1A14E7EA}")]
	[Persistent()]
	public class Association : MetaDataRepository.Association
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(ObjectLinksStorages))
            {
                if (value == null)
                    ObjectLinksStorages = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>);
                else
                    ObjectLinksStorages = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(ObjectLinksStorages))
                return ObjectLinksStorages;


            return base.GetMemberValue(token, member);
        }

        public Association()
        {

        }
		/// <MetaDataID>{49803F69-3F54-4BEB-9091-73BAEB5DE497}</MetaDataID>
		public AssociationEnd GetAssociationEndWithReferenceColumns()
		{
			
			if(MultiplicityType==MetaDataRepository.AssociationType.ManyToMany||LinkClass!=null)
				throw new System.Exception("This method used only for \"one to one\" or \"one to Many\" relationships. It isn't used for link with \"many to many\" relationship");
			AssociationEnd associationEnd=RoleA as AssociationEnd; 
			if(MetaDataRepository.AssociationType.OneToOne==MultiplicityType)
				associationEnd=RoleA as AssociationEnd;
			else
				if(!associationEnd.Multiplicity.IsMany)
				associationEnd=(AssociationEnd)associationEnd.GetOtherEnd();
			return associationEnd;
		}

		///<summary>
		///This method returns all storage cells which have relation object related with someone object 
		///from the objects of storageCell parameter.      
		///</summary>
		///<param name="storageCell">
		///This parameter define the root storage cell, 
		///the method return the relation object storage cells which are related with it.
		///</param>
		///<param name="storageCellsRole">
		///This parameter define the role of storage cells on association.
		///</param>
		/// <MetaDataID>{38E82E18-74FF-482F-908F-CBA365A2B950}</MetaDataID>
		public Collections.Generic.Set<MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(StorageCell storageCell,MetaDataRepository.Roles storageCellsRole)
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
                Collections.Generic.Set<MetaDataRepository.StorageCell> StorageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
				foreach(StorageCellsLink storageCellsLink in ObjectLinksStorages)
				{
					if(storageCellsRole==MetaDataRepository.Roles.RoleA&&storageCellsLink.RoleAStorageCell==storageCell)
						StorageCells.AddRange(storageCellsLink.AssotiationClassStorageCells);
					else
					{
						if(storageCellsLink.RoleBStorageCell==storageCell)
							StorageCells.AddRange(storageCellsLink.AssotiationClassStorageCells);
					}
				}
				return StorageCells;
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock();
			}
		}

		///<summary>
		///This method returns all storage cells which have related object with someone object 
		///from the objects of storageCell parameter.      
		///</summary>
		///<param name="storageCell">
		///This parameter define the root storage cell, 
		///the method return the related storage cells with it.
		///</param>
		///<param name="linkedStorageCellsRole">
		///This parameter define the role of related storage cells on association.
		///</param>
		/// <MetaDataID>{9B9E7457-381F-4896-A568-BC778E2EDB84}</MetaDataID>
		public Collections.Generic.Set<MetaDataRepository.StorageCell> GetLinkedStorageCells(StorageCell storageCell,MetaDataRepository.Roles linkedStorageCellsRole)
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
                Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
				foreach(StorageCellsLink storageCellsLink in ObjectLinksStorages)
				{
					if(storageCellsLink.AssotiationClassStorageCells.Contains(storageCell))
					{
						if(linkedStorageCellsRole==MetaDataRepository.Roles.RoleA)
							storageCells.Add(storageCellsLink.RoleAStorageCell);
						else
							storageCells.Add(storageCellsLink.RoleBStorageCell);
					}

					if(linkedStorageCellsRole==MetaDataRepository.Roles.RoleA&&storageCellsLink.RoleBStorageCell==storageCell)
						storageCells.Add(storageCellsLink.RoleAStorageCell);
					else
					{
						if(storageCellsLink.RoleAStorageCell==storageCell)
							storageCells.Add(storageCellsLink.RoleBStorageCell);
					}
				}
				return storageCells;
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock();
			}
		}

        /// <MetaDataID>{E48F09B0-FF1B-453C-8406-25C2DA5153D7}</MetaDataID>
        public StorageCellsLink AddStorageCellsLink(MetaDataRepository.StorageCell RoleAObjectCell, MetaDataRepository.StorageCell RoleBObjectCell, string valueTypePath)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                StorageCellsLink mObjectCellsLink = GetStorageCellsLink(RoleAObjectCell, RoleBObjectCell,valueTypePath);
                if (mObjectCellsLink == null)
                {
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        PersistenceLayer.ObjectStorage metadataStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);

                        System.Type[] parameterTypes ={ typeof(Association), typeof(StorageCell), typeof(StorageCell) };


                        mObjectCellsLink = (StorageCellsLink)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(StorageCellsLink), parameterTypes,this, RoleAObjectCell, RoleBObjectCell);
                        mObjectCellsLink.ValueTypePath = valueTypePath;
                        if (PersistenceLayer.ObjectStorage.GetStorageOfObject(RoleAObjectCell.Properties) == metadataStorage)
                        {
                            RoleAObjectCell.Namespace.AddOwnedElement(mObjectCellsLink);
                            RoleAObjectCell.Namespace.MakeNameUnaryInNamesapce(mObjectCellsLink);
                        }
                        else if (PersistenceLayer.ObjectStorage.GetStorageOfObject(RoleBObjectCell.Properties) == metadataStorage)
                        {
                            RoleBObjectCell.Namespace.AddOwnedElement(mObjectCellsLink);
                            RoleBObjectCell.Namespace.MakeNameUnaryInNamesapce(mObjectCellsLink);
                        }
                        else
                        {
                            throw new System.Exception("Error on storage metada");
                        }


                        if (MultiplicityType == MetaDataRepository.AssociationType.ManyToMany && LinkClass == null)
                        {
                            foreach (StorageCellsLink CurrObjectLink in ObjectLinksStorages)
                            {
                                if (mObjectCellsLink.ValueTypePath==CurrObjectLink.ValueTypePath&&!CurrObjectLink.IsFull)
                                {
                                    if ((mObjectCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType == (CurrObjectLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType &&
                                        (mObjectCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType == (CurrObjectLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType)
                                    {

                                        mObjectCellsLink.ObjectLinksTable = CurrObjectLink.ObjectLinksTable;
                                        break;
                                    }
                                }
                            }
                        }
                        ObjectLinksStorages.Add(mObjectCellsLink);
                        StateTransition.Consistent = true; ;
                    }
                }

                return mObjectCellsLink;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        ///<summary>
        ///This method returns all storage cell links which refer 
        ///to parameter storageCell either as RoleAStorageCell or as RoleBStorageCell 
        ///</summary>
        ///<param name="storagteCell">
        ///Define the storage cell where we want to retrieve the related storage cell links. 
        ///</param>
        /// <MetaDataID>{EABBCE28-5202-4898-9095-E7B0B8844DC1}</MetaDataID>
        public Collections.Generic.Set<StorageCellsLink> GetStorageCellsLinks(MetaDataRepository.StorageCell storageCell)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
              
                //if ((!storageCell.Type.IsA(RoleA.Specification)) && (!storageCell.Type.IsA(RoleB.Specification)))
                //    throw new System.Exception("StorageCell type '" + storageCell.Type.FullName + "' isn't any from '" + RoleA.Specification.FullName + "' or '" + RoleB.Specification.FullName + "'.");
                Collections.Generic.Set <StorageCellsLink> storageCellLinks = new OOAdvantech.Collections.Generic.Set<StorageCellsLink>();
                foreach (StorageCellsLink storageCellsLink in ObjectLinksStorages)
                {
                    if (LinkClass != null && storageCell.Type.IsA(LinkClass) && storageCellsLink.AssotiationClassStorageCells.Contains(storageCell))
                    {
                        storageCellLinks.Add(storageCellsLink);
                    }
                    else
                    {
                        if (storageCellsLink.RoleAStorageCell == storageCell)
                            storageCellLinks.Add(storageCellsLink);
                        else
                        {
                            if (storageCellsLink.RoleBStorageCell == storageCell)
                                storageCellLinks.Add(storageCellsLink);
                        }
                    }
                }
                
                foreach (Association specializedAssoctiation in Specializations)
                    storageCellLinks.AddRange(specializedAssoctiation.GetStorageCellsLinks(storageCell));

                return storageCellLinks;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }



        ///<summary>
        ///This method returns all storage cell links which refer 
        ///to parameter storageCell either as RoleAStorageCell or as RoleBStorageCell 
        ///</summary>
        ///<param name="storagteCell">
        ///Define the storage cell where we want to retrieve the related storage cell links. 
        ///</param>

        public Collections.Generic.Set<StorageCellsLink> GetStorageCellsLinks(MetaDataRepository.StorageCell storageCell, MetaDataRepository.Roles linkedStorageCellsRole)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {

                //if ((!storageCell.Type.IsA(RoleA.Specification)) && (!storageCell.Type.IsA(RoleB.Specification)))
                //    throw new System.Exception("StorageCell type '" + storageCell.Type.FullName + "' isn't any from '" + RoleA.Specification.FullName + "' or '" + RoleB.Specification.FullName + "'.");
                Collections.Generic.Set<StorageCellsLink> storageCellLinks = new OOAdvantech.Collections.Generic.Set<StorageCellsLink>();
                foreach (StorageCellsLink storageCellsLink in ObjectLinksStorages)
                {

                    if (storageCellsLink.RoleAStorageCell == storageCell && linkedStorageCellsRole == MetaDataRepository.Roles.RoleB)
                        storageCellLinks.Add(storageCellsLink);
                    if (storageCellsLink.RoleBStorageCell == storageCell && linkedStorageCellsRole == MetaDataRepository.Roles.RoleA)
                        storageCellLinks.Add(storageCellsLink);

                }

                foreach (Association specializedAssoctiation in Specializations)
                    storageCellLinks.AddRange(specializedAssoctiation.GetStorageCellsLinks(storageCell, linkedStorageCellsRole));

                return storageCellLinks;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }


		///<summary>
		///This method returns the storage cell link object, 
		///for objects links between the objects of roleAStorageCell and roleBStorageCell if exist. 
		///Otherwise return null.
		///</summary>
		///<param name="roleAStorageCell">
		///This parameter defines the storage cell which contains the roleA objects.
		///</param>
		///<param name="roleBStorageCell">
		///This parameter defines the storage cell which contains the roleB objects.
		///</param>
        /// <param name="valueTypePath">
        /// </param>
		/// <MetaDataID>{EA2304F4-C348-4AFF-81B1-90FDB2C5E3F6}</MetaDataID>
        public StorageCellsLink GetStorageCellsLink(OOAdvantech.MetaDataRepository.StorageCell roleAObjectCell, OOAdvantech.MetaDataRepository.StorageCell roleBObjectCell, string valueTypePath)
		{
			return GetStorageCellsLink(roleAObjectCell, roleBObjectCell,valueTypePath ,false);

		}

		///<summary>
		///This method returns the storage cell link object, 
		///for objects links between the objects of roleAStorageCell and roleBStorageCell if exist. 
		///Otherwise return null.
		///</summary>
		///<param name="roleAStorageCell">
		///This parameter defines the storage cell which contains the roleA objects.
		///</param>
		///<param name="roleBStorageCell">
		///This parameter defines the storage cell which contains the roleB objects.
		///</param>
		///<param name="createIfNotExist">
		///If this parameter is true and there isn’t storage cells link 
		///for the roleAStorageCell and roleBStorageCell the method create one.
		///</param>
        /// <param name="valueTypePath">
        /// </param>
		/// <MetaDataID>{21D21649-8418-4CC2-B307-E82A4BF5C114}</MetaDataID>
        public StorageCellsLink GetStorageCellsLink(MetaDataRepository.StorageCell roleAStorageCell, MetaDataRepository.StorageCell roleBStorageCell, string valueTypePath, bool createIfNotExist)
		{
            lock (LockObject)
            {
                foreach (StorageCellsLink storageCellsLink in ObjectLinksStorages)
                {
                    if (roleAStorageCell == storageCellsLink.RoleAStorageCell && roleBStorageCell == storageCellsLink.RoleBStorageCell)
                    {
                        if ((string.IsNullOrEmpty(valueTypePath) && string.IsNullOrEmpty(storageCellsLink.ValueTypePath)) || storageCellsLink.ValueTypePath == valueTypePath)
                            return storageCellsLink;
                    }
                }
                if (!roleAStorageCell.Type.IsA(RoleA.Specification) || ((RoleB.Specification is Structure) && !(RoleB.Specification as Structure).Persistent))
                {
                    if (!(RoleA.Specification is Structure))
                        throw new System.Exception("roleAStorageCell type '" + roleAStorageCell.Type.FullName + "' isn't a '" + RoleA.Specification.FullName + "'.");
                    else if (!(roleAStorageCell.Type as Class).ContainsValueTypeAsMember(RoleA.Specification as Structure))
                        throw new System.Exception("roleAStorageCell type '" + roleAStorageCell.Type.FullName + "' isn't a '" + RoleA.Specification.FullName + "'.");
                }
                if (!roleBStorageCell.Type.IsA(RoleB.Specification) || ((RoleA.Specification is Structure) && !(RoleA.Specification as Structure).Persistent))
                {
                    if (!(RoleB.Specification is Structure) || !(RoleB.Specification as Structure).Persistent)
                        throw new System.Exception("roleBStorageCell type '" + roleBStorageCell.Type.FullName + "' isn't a '" + RoleB.Specification.FullName + "'.");
                    else if (!(roleBStorageCell.Type as Class).ContainsValueTypeAsMember(RoleB.Specification as Structure))
                        throw new System.Exception("roleBStorageCell type '" + roleBStorageCell.Type.FullName + "' isn't a '" + RoleB.Specification.FullName + "'.");
                }
                if (createIfNotExist)
                    return AddStorageCellsLink(roleAStorageCell, roleBStorageCell, valueTypePath);
                else
                    return null;
            }
			
		}


		//Error Prone Thread Safety.
		/// <MetaDataID>{4359264D-9B35-41DC-ABB0-9CFF96221CF3}</MetaDataID>
		[Association("LinksInstanceTypeRelation",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink),Roles.RoleA,"{ECFC605B-F4AA-41B4-BD22-130AC73F5BEA}")]
		[PersistentMember("ObjectLinksStorage")]
		[RoleAMultiplicityRange(1)]
		public Collections.Generic.Set<StorageCellsLink> ObjectLinksStorages=new OOAdvantech.Collections.Generic.Set<StorageCellsLink>();



        public void RemoveStorageCellsLink(StorageCellsLink StorageCellsLink)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                ObjectLinksStorages.Remove(StorageCellsLink);
                PersistenceLayer.ObjectStorage.DeleteObject(StorageCellsLink);
                stateTransition.Consistent = true;
            }
        
        }
    }
}
