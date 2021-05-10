using OOAdvantech.Transactions;
namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{958c8312-72fa-4a96-b7d7-2f63c58a7162}</MetaDataID>
    [BackwardCompatibilityID("{958c8312-72fa-4a96-b7d7-2f63c58a7162}"), Persistent()]
    public class StorageServer : OOAdvantech.MetaDataRepository.Namespace
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Users))
            {
                if (value == null)
                    _Users = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.Security.User>);
                else
                    _Users = (OOAdvantech.Collections.Generic.Set<OOAdvantech.Security.User>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Storages))
            {
                if (value == null)
                    _Storages = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageReference>);
                else
                    _Storages = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageReference>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Users))
                return _Users;

            if (member.Name == nameof(_Storages))
                return _Storages;


            return base.GetMemberValue(token, member);
        }

        ///TODO να γραφτούν test case στην περίπτωση που γίνεται attach storage που έχει διμιουργηθεί σε άλλο server
        ///και test case περίπτωση που αλλάξει όνομα ο υπόλιγιστής.

        /// <exclude>Excluded</exclude>
        Collections.Generic.Set<Security.User> _Users = new OOAdvantech.Collections.Generic.Set<OOAdvantech.Security.User>();

        [Association("StorageServerUsers", Roles.RoleA, "56513e2b-18e6-4101-8bcb-0c56bf03d071")]
        [PersistentMember("_Users")]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.Security.User> Users
        {
            get
            {
                int tt = _Users.AsReadOnly().Count;

                return _Users.AsReadOnly();// new OOAdvantech.Collections.Generic.Set<OOAdvantech.Security.User>(_Users, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }

        /// <MetaDataID>{e021393d-f582-44a7-b8df-a867ba794340}</MetaDataID>
        public void AddUser(OOAdvantech.Security.User user)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Users.Add(user);
                int mm = Users.Count;
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{cf7a2168-556f-4ebf-9d38-1586c7eacd7d}</MetaDataID>
        public void DeleteUser(Security.User user)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Users.Remove(user); 
                stateTransition.Consistent = true;
            }
        
        }
      
        /// <exclude>Excluded</exclude>
        Collections.Generic.Set<StorageReference> _Storages = new OOAdvantech.Collections.Generic.Set<StorageReference>();
        [RoleAMultiplicityRange(1), PersistentMember("_Storages")]
        [Association("ManagedStorage", Roles.RoleA, "5110b128-7fd3-4597-895a-ddb522bb70d3")]
        public Collections.Generic.Set<StorageReference> Storages
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<StorageReference>(_Storages, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }

        /// <MetaDataID>{34c6a737-98d4-4203-81e5-313129f1e037}</MetaDataID>
        public void AddStorage(StorageReference storage)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Storages.Add(storage); 
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{e802923f-125b-450f-869c-59984e85c411}</MetaDataID>
        public void RemoveStorage(StorageReference storage)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Storages.Remove(storage);
                stateTransition.Consistent = true;
            } 
        }

        /// <MetaDataID>{1b885a3d-ed89-4018-9c8e-d991d417d9ab}</MetaDataID>
        public virtual void AttachStorage(string storageName, string storageType, string storageDataLocation)
        {
            
            
        }

        /// <MetaDataID>{fb0b5e31-ef8c-43cc-b9f7-5f4a16bf7f2c}</MetaDataID>
        internal bool ExisStorageReference(string storageName, string storageType, string storageLocation)
        {
            if (PersistenceLayer.ObjectStorage.StorageProviders.ContainsKey(storageType))
                storageType = PersistenceLayer.ObjectStorage.StorageProviders[storageType];

            foreach (var storageRef in Storages)
            {
                if (storageRef.StorageLocation.ToLower() == storageLocation.ToLower() &&
                    storageRef.StorageName.ToLower() == storageName.ToLower())
                {
                    if (storageType.ToLower() == storageRef.StorageType.ToLower())
                        return true;
                    if (PersistenceLayer.ObjectStorage.StorageProviders.ContainsKey(storageRef.StorageType) &&
                        storageType.ToLower() == PersistenceLayer.ObjectStorage.StorageProviders[storageRef.StorageType].ToLower())
                        return true;
                }
            }
            return false;
        }
    }
}
