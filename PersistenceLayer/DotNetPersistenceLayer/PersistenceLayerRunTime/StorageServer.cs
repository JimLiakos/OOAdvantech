using OOAdvantech.Transactions;
namespace OOAdvantech.PersistenceLayerRunTime
{
    /// <MetaDataID>{7fa3c90e-aca6-490c-ae10-0509b0963786}</MetaDataID>
    public class StorageServer : OOAdvantech.MetaDataRepository.StorageServer
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{311a8aac-b30d-4a0d-b071-20459821f430}</MetaDataID>
        public override void AttachStorage(string storageName, string storageType, string storageDataLocation)
        {
#if !DeviceDotNet
            StorageProvider storageProvider = ObjectStorage.GetStorageProvider(storageType);


            string nativeStorageID = storageProvider.GetNativeStorageID(storageDataLocation);
            foreach (var storageReference in Storages)
            {
                if (storageReference.NativeStorageID == nativeStorageID)
                    return;
            }
         
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                string instanceName=null;
                if (Name.IndexOf("$") != -1)
                    instanceName = Name.Substring(Name.IndexOf("$") + 1);

                string storageLocation = System.Environment.MachineName;
                if (!string.IsNullOrEmpty(instanceName))
                    storageLocation += @"\" + instanceName;

                OOAdvantech.PersistenceLayer.Storage storage = storageProvider.AttachStorage(storageName, storageLocation, storageDataLocation);

                OOAdvantech.MetaDataRepository.StorageReference storageReference = new OOAdvantech.MetaDataRepository.StorageReference();
                storageReference.Name = storageName;
                storageReference.StorageName = storageName;
                storageReference.StorageType = storageType;
                storageReference.StorageLocation = storageLocation;
                storageReference.NativeStorageID = nativeStorageID;
                AddStorage(storageReference);
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(storageReference); 
                stateTransition.Consistent = true;
            }
#endif
        }
    }
}
