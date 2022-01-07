using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections;

namespace UserInterfaceTest
{
    /// <MetaDataID>{57f0f1ca-1584-4893-85d7-afa96914ec1c}</MetaDataID>
    public class Company
    {
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrder> Orders
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrder> orders = new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrder>();
                ObjectStorage objectStorage = Form3.OpenStorage();

                string objectQuery = "#OQL: SELECT order " +
                  " FROM AbstractionsAndPersistency.IOrder order #";

                StructureSet objectSet = objectStorage.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                    orders.Add(objectSet["order"] as AbstractionsAndPersistency.IOrder);
                return orders;

            }
        }


        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IProduct> Products
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IProduct> products = new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IProduct>();
                ObjectStorage objectStorage = ObjectStorage.OpenStorage("Abstractions",
                                                            @"c:\Abstractions.xml",
                                                            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                string objectQuery = "#OQL: SELECT product " +
                  " FROM AbstractionsAndPersistency.IProduct product #";

                StructureSet objectSet = objectStorage.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                    products.Add(objectSet["product"] as AbstractionsAndPersistency.IProduct);
                return products;

            }
        }
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IClient> Clients
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IClient> clients = new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IClient>();
                ObjectStorage objectStorage = Form3.OpenStorage();

                string objectQuery = "#OQL: SELECT client " +
                  " FROM AbstractionsAndPersistency.IClient client #";

                StructureSet objectSet = objectStorage.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                    clients.Add(objectSet["client"] as AbstractionsAndPersistency.IClient);
                return clients;

            }
        }
       
    }
}
