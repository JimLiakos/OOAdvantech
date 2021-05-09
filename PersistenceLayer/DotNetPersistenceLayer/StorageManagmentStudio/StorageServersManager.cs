using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace StorageManagmentStudio
{
    /// <MetaDataID>{ef206368-9f79-4652-bfb3-ee23ee8b1f0e}</MetaDataID>

    [BackwardCompatibilityID("{ef206368-9f79-4652-bfb3-ee23ee8b1f0e}"), Persistent()]
    public class StorageServersManager : System.MarshalByRefObject
    {

        /// <MetaDataID>{e6e71f35-7c31-4053-a15a-cc8d0605b808}</MetaDataID>
        ObjectStorage ObjectStorage;
        /// <MetaDataID>{3e2fca84-e635-44fa-9605-9b1010217546}</MetaDataID>
        public StorageServersManager()
        {
            string appPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\StorageManagmentStudio";
            if (!System.IO.Directory.Exists(appPath))
                System.IO.Directory.CreateDirectory(appPath);
            
            string storageName = "Connections";
            string storageLocation = appPath + @"\Connections.xml";
            string storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";
            try
            {
                ObjectStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);
            }
            catch (StorageException error)
            {
                if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                    ObjectStorage = ObjectStorage.NewStorage(storageName, storageLocation, storageType);
                else throw;
            }

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);

            _Connections = (from storageCellsLink in storage.GetObjectCollection<StorageServerLink>()
                           select storageCellsLink).ToList();

            

        }


        /// <MetaDataID>{b9eb6d5a-3589-4e2e-90f6-0c5a86d469d8}</MetaDataID>
        public void AddConnection()
        {


            var storageServerLink =new StorageServerLink();
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(storageServerLink))
            {
                StorageServerConnectionWindow storageServerConnectionWindow = new StorageServerConnectionWindow();
                ObjectStorage.CommitTransientObjectState(storageServerLink);

                storageServerConnectionWindow.Connection.Instance = storageServerLink;
                storageServerConnectionWindow.ShowDialog(); 
                stateTransition.Consistent = true;
            }
        
        }


        /// <exclude>Excluded</exclude>
        List<StorageServerLink> _Connections = new List<StorageServerLink>();
        [Association("StorageServerConections", Roles.RoleA, "3fcf764d-7978-43e9-b950-6311e525e9b8")]
        public IList<StorageServerLink> Connections
        {
            get
            {
                return _Connections.AsReadOnly();
            }
        }

    


    }




 



}
