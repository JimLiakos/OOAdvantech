using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using OOAdvantech.Linq;

namespace PersistencySystemTester
{
    /// <MetaDataID>{29b75777-ca7c-4fe5-b6fc-41540639b573}</MetaDataID>
    public class DerivedAssoctationsTest
    {
        static ObjectStorage ObjectStorage = null;
        public ObjectStorage OpenDerivedAssoctationsXMLStorage()
        {
            string storageName = "DerivedAssoctations";
            string storageLocation = @"c:\DerivedAssoctations.xml";
            string storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";
          
            try
            {
                ObjectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    ObjectStorage = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    ObjectStorage.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName, "SingularControl");
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            return ObjectStorage;
        }

        public ObjectStorage OpenDerivedAssoctationsStorage()
        {
            string storageName = "DerivedAssoctations";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            try
            {
                ObjectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    ObjectStorage = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    ObjectStorage.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);//, "SingularControl");
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            return ObjectStorage;
        }


        public ObjectStorage OpenDerivedAssoctationsMixedStorage()
        {
            string storageName = "DerivedAssoctations";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            try
            {
                ObjectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    ObjectStorage = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    ObjectStorage.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName, "SingularControl");
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            return ObjectStorage;
        }


        public void CreateDerivedAssoctationLink()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                AbstractionsAndPersistency.LiquidProduct product = new AbstractionsAndPersistency.LiquidProduct("Oil");
                ObjectStorage.CommitTransientObjectState(product);
                AbstractionsAndPersistency.LiquidStore tank = new AbstractionsAndPersistency.LiquidStore("OilTank24");
                ObjectStorage.CommitTransientObjectState(tank);
                product.AddStorePlace(tank);
                AbstractionsAndPersistency.StorePlace store = new AbstractionsAndPersistency.StorePlace("OilPlace24");
                ObjectStorage.CommitTransientObjectState(store);
                store.AddProduct(product);
                stateTransition.Consistent = true;
            }
        }
        public void RetrieveDerivedAssoctationObjects()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
            var liquidProducts = from _product in storage.GetObjectCollection<AbstractionsAndPersistency.Product>()
                                 select new 
                                 { 
                                     _product,
                                     storePlaces = from storePlace in _product.StorePlaces
                                     select storePlace
                                 };

            bool ThereAreRelatedStorePlaces = false;
            
            foreach (var product in liquidProducts)
            {

                foreach (var ee in (product._product as AbstractionsAndPersistency.LiquidProduct).LiquidStorePlaces)
                {
                }
                if (new List<AbstractionsAndPersistency.IStorePlace>(product.storePlaces).Count!=2)
                {
                    throw new System.Exception("Test case error");
                }
              
            }

        }


    }
}
