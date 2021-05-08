using OOAdvantech.Transactions;
namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{62303b73-f8de-4c1d-b689-b63201af69fe}</MetaDataID>
    [BackwardCompatibilityID("{62303b73-f8de-4c1d-b689-b63201af69fe}"), Persistent()]
    public class StorageReference : OOAdvantech.MetaDataRepository.MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_NativeStorageID))
            {
                if (value == null)
                    _NativeStorageID = default(string);
                else
                    _NativeStorageID = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Storage))
            {
                if (value == null)
                    _Storage = default(OOAdvantech.MetaDataRepository.Storage);
                else
                    _Storage = (OOAdvantech.MetaDataRepository.Storage)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageType))
            {
                if (value == null)
                    _StorageType = default(string);
                else
                    _StorageType = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageName))
            {
                if (value == null)
                    _StorageName = default(string);
                else
                    _StorageName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageLocation))
            {
                if (value == null)
                    _StorageLocation = default(string);
                else
                    _StorageLocation = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_NativeStorageID))
                return _NativeStorageID;

            if (member.Name == nameof(_Storage))
                return _Storage;

            if (member.Name == nameof(_StorageType))
                return _StorageType;

            if (member.Name == nameof(_StorageName))
                return _StorageName;

            if (member.Name == nameof(_StorageLocation))
                return _StorageLocation;


            return base.GetMemberValue(token, member);
        }


        /// <MetaDataID>{13e8252a-c68e-43e4-9410-8219d2f0ac77}</MetaDataID>
        string _NativeStorageID;
        /// <MetaDataID>{a2eab704-2ec3-49e1-a68f-64dbfe6f067b}</MetaDataID>
        [PersistentMember("_NativeStorageID"), BackwardCompatibilityID("+4")]
        public string NativeStorageID
        {
            get
            {
                return _NativeStorageID;
            }
            set
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    _NativeStorageID = value; 
                    stateTransition.Consistent = true;
                }
        
            }
        }
        /// <MetaDataID>{d40e39d7-0e07-4592-b9ab-c1eac0111592}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }

        /// <exclude>Excluded</exclude>
        Storage _Storage;
        [Association("ActualSorage", Roles.RoleA, "1b72ee27-9a52-44f1-8156-80ce806d72ff")]
        public Storage Storage
        {
            get
            {
                if (_Storage == null)
                    _Storage = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType).StorageMetaData as Storage;
                return _Storage;
            }
        }

        
        /// <exclude>Excluded</exclude>
        string _StorageType;
        /// <MetaDataID>{7c838319-666a-4e7e-b63f-815ad3368e70}</MetaDataID>
        [PersistentMember("_StorageType"), BackwardCompatibilityID("+1")]
        public string StorageType
        {
          get
          {
            return _StorageType;
          }
          set
          {
            using (ObjectStateTransition stateTransition= new ObjectStateTransition(this))
            {
	      	    _StorageType=value;
        	    stateTransition.Consistent = true;
            }
          }
        }

        
       
      
        /// <exclude>Excluded</exclude>
        string _StorageName;
        /// <MetaDataID>{d5e58fac-baa8-496d-8a0a-d8f546bcea47}</MetaDataID>
        [PersistentMember("_StorageName"), BackwardCompatibilityID("+2")]
        public string StorageName
        {
          get
          {
            return _StorageName;
          }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StorageName = value;
                    stateTransition.Consistent = true;
                }
            }
        }
      
      
        
        /// <exclude>Excluded</exclude>
        string _StorageLocation;
        /// <MetaDataID>{754d944b-0016-4a43-b483-541f00008b33}</MetaDataID>
        [PersistentMember("_StorageLocation"), BackwardCompatibilityID("+3")]
        public string StorageLocation
        {
          get
          {
            return _StorageLocation;
          }
          set
          {
            using (ObjectStateTransition stateTransition= new ObjectStateTransition(this))
            {
	      	    _StorageLocation=value;
        	    stateTransition.Consistent = true;
            }
          }
        }
      
 
        ///// <exclude>Excluded</exclude>
        //string _StorageLocation;
        //public string StorageLocation
        //{
        //    get { return _StorageLocation}
        //    set { }

        //}

        public OOAdvantech.PersistenceLayer.ObjectStorage OpenObjectSorage()
        {
            return PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType);
        }
    }
}
