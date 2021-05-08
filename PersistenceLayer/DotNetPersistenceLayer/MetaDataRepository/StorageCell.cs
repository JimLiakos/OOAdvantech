namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{3EA97399-BEB0-4A22-8F94-FFC6336D8343}</MetaDataID>
    /// <summary>StorageCell defines a stack of objects. </summary>
    public abstract class StorageCell : MetaObject
    {

        public MetaObjectID TypeIdentity
        {
            get
            {
                
                if (_Type != null)
                    return _Type.Identity;
                return
                    null;
            }
        }

        public string TypeAssemblyQualifiedName
        {
            get
            {
                if (_Type != null && _Type.GetExtensionMetaObject<System.Type>() != null)
                    return _Type.GetExtensionMetaObject<System.Type>().AssemblyQualifiedName;
                else
                    return null;
            }
        }
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Type))
            {
                if (value == null)
                    _Type = default(OOAdvantech.MetaDataRepository.Class);
                else
                    _Type = (OOAdvantech.MetaDataRepository.Class)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Type))
                return _Type;


            return base.GetMemberValue(token, member);
        }

        public abstract bool IsTypeOf(string ofTypeIdentity);


        /// <MetaDataID>{20fab7fd-b07e-4691-bf40-f9710daaf7f9}</MetaDataID>
        [BackwardCompatibilityID("+14")]
        public virtual string StorageType
        {
            get
            {
                return (Namespace as Storage).StorageType;
            }
            set
            {
            }
        }


        public static MetaDataRepository.Classifier GetOfTypeClassifier(string ofTypeIdentity, MetaDataRepository.Classifier baseClassifier)
        {

            MetaDataRepository.Classifier ofTypeClassifier = null;
            if (baseClassifier.Identity.ToString() == ofTypeIdentity)
            {
                ofTypeClassifier = baseClassifier;
            }
            else
            {
                foreach (var generalClassifier in baseClassifier.GetAllGeneralClasifiers())
                {
                    if (generalClassifier.Identity.ToString() == ofTypeIdentity)
                    {
                        ofTypeClassifier = generalClassifier;
                        break;
                    }
                }
            }
            return ofTypeClassifier;
        }

        [Association("StorageCellIdentityType", typeof(ObjectIdentityType), Roles.RoleA, "ae1ff1aa-0be6-463d-8e8a-64891f93bdee")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public abstract ObjectIdentityType ObjectIdentityType
        {
            get;
            set;
        }
        /// <MetaDataID>{8E4FAD25-8022-4946-9B49-E8F8D8F69BA1}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        public virtual string StorageLocation
        {
            set
            {
            
            }
            get
            {
                return (Namespace as Storage).StorageLocation;
            }
        }
        /// <MetaDataID>{588D848D-2477-4AEB-BD8C-6B2382722D83}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        public virtual string StorageName
        {
            set
            {
            
            }
            get
            {
                return (Namespace as Storage).StorageName;
            }
        }



        
        /// <MetaDataID>{6620c8f1-3f2d-40c6-9e83-9a9ae0696b65}</MetaDataID>
        public abstract Collections.Generic.List<StorageCell> GetLinkedStorageCells(MetaDataRepository.MetaObjectID associationIdentity, MetaDataRepository.Roles linkedStorageCellsRole);


        /// <MetaDataID>{7780b18a-f90e-4f09-abdc-02de8a04ac98}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        public abstract int SerialNumber
        {

            set;
            get;
        }

        /// <MetaDataID>{05f5511d-1602-48b1-9afb-f478c4939f7d}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        public abstract bool AllObjectsInActiveMode
        {
      
            get;
        }

        /// <MetaDataID>{3c5c16f1-c479-47ee-9971-a4e4edf6ba37}</MetaDataID>
        public void GetStorageConnectionData(out string storageIdentity, out string storageName, out string storageLocation, out string storageProvider)
        {
            storageIdentity = (Namespace as Storage).StorageIdentity;
            storageName = (Namespace as Storage).StorageName;
            storageLocation = (Namespace as Storage).StorageLocation;
            storageProvider = (Namespace as Storage).StorageType;
        }

        

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{44753B28-CFC2-4C20-8D72-20F942C26D44}</MetaDataID>
        protected Class _Type;
        /// <MetaDataID>{8B0CADA3-8D66-495E-8307-B7C0C90E744C}</MetaDataID>
        [Association("StoreObjectInCell",typeof(OOAdvantech.MetaDataRepository.Class),MetaDataRepository.Roles.RoleB,"{E86388A2-3151-4935-BCF8-9A19CCFE6122}")]
        [RoleBMultiplicityRange(1,1)]
   		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public virtual Class Type
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _Type;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_Type != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Type = value;
                            stateTransition.Consistent = true;

                        }
                    }
                }
                finally
                {
                    
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }


            }
        }


        //}
        /// <MetaDataID>{180BEA89-D30A-43E5-9514-F081DAD7BFB0}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        public  virtual string StorageIdentity
        {
            set
            {
            
            }
            get
            {
                return (Namespace as Storage).StorageIdentity;
            }
        }

        




        /// <MetaDataID>{c2aeb32c-4931-4e6e-96c6-21bc21609394}</MetaDataID>
        public abstract void ActivateAllObjects();


    }
}
