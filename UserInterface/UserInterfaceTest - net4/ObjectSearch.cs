using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Collections.Generic;
using AbstractionsAndPersistency;
using OOAdvantech.PersistenceLayer;

using OOAdvantech.Collections;

namespace UserInterfaceTest
{

    /// <MetaDataID>{5f5def32-547b-4465-829c-5525b509e36a}</MetaDataID>
    public class ObjectSearch
    {


        public static Set<IClient> GetClients()
        {
            Set<IClient> clients = new Set<IClient>();
            ObjectStorage objectStorage = null;
            objectStorage = Form3.OpenStorage();

            string objectQuery = @"SELECT client 
                                FROM "+typeof(IClient).FullName+" client ";

            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IClient client = null;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                client = objectSet["client"] as AbstractionsAndPersistency.IClient;
                clients.Add(client);
            }
            return clients;
        }


        public static Set<IOrder> GetOrders()
        {
            Set<IOrder> orders = new Set<IOrder>();
            ObjectStorage objectStorage = null;
            objectStorage = Form3.OpenStorage();
            string objectQuery = "#OQL: SELECT order " +
                              " FROM AbstractionsAndPersistency.IOrder order #";

            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IOrder order = null;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                order = objectSet["order"] as AbstractionsAndPersistency.IOrder;
                orders.Add(order);
            }
            return orders;
        }
        public static Set<IClient> SearchForClient(object clientNameLike)
        {


            Set<IClient> clientCollection = new Set<IClient>();
            try
            {
                if (clientNameLike.ToString() == "s")
                    return clientCollection;
                ObjectStorage objectStorage = null;
                objectStorage = Form3.OpenStorage();
              //@"localhost\Debug",
              //"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                string objectQuery = "#OQL: SELECT client " +
                                  " FROM AbstractionsAndPersistency.IClient client where client.Name Like @ClientNamePatern#";

                OOAdvantech.Collections.Generic.Dictionary<string, object> paramters = new OOAdvantech.Collections.Generic.Dictionary<string, object>();
                paramters["@ClientNamePatern"] = clientNameLike;


                StructureSet objectSet = objectStorage.Execute(objectQuery, paramters);
                AbstractionsAndPersistency.IClient client = null;
                int i = 0;
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    client = objectSet["client"] as AbstractionsAndPersistency.Client;
                    i++;
                    clientCollection.Add(client);
                    //if (i > 12)
                    //    break;     
                }
            }
            catch (System.Exception error)
            {

            }
            return clientCollection;
            // return new Set<IOrder>();



        }


        public static Set<IPriceList> SearchForPriceList()
        {
            Set<IPriceList> priceListCollection = new Set<IPriceList>();
            ObjectStorage objectStorage = null;
            objectStorage = Form3.OpenStorage();
              //@"localhost\Debug",
              //"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

            string objectQuery = "#OQL: SELECT priceList  " +
                              " FROM AbstractionsAndPersistency.IPriceList priceList #";


            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IPriceList priceList = null;
            int i = 0;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                priceList = objectSet["priceList"] as AbstractionsAndPersistency.IPriceList;
                i++;
                priceListCollection.Add(priceList);
                if (i > 12)
                    break;
            }
            return priceListCollection;



        }

        public static Set<IProductPrice> Foo()
        {
            return null;
        }
        public static Set<SubPriceListB> Fooqads()
        {
            return null;
        }

        public static Set<IProductPrice> FooNA()
        {
            return null;
        }

        public static Set<IProductPrice> SearchForProduct(IPriceList priceList, string productNameLike)
        {
            Set<IProductPrice> productPriceCollection = new Set<IProductPrice>();
            ObjectStorage objectStorage = null;
            objectStorage = Form3.OpenStorage();
              //@"localhost\Debug",
              //"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

            string objectQuery = "#OQL: SELECT productPrice  " +
                              " FROM AbstractionsAndPersistency.ProductPrice productPrice  #";//WHERE productPrice.PriceList = @PriceList#";

            OOAdvantech.Collections.Hashtable parameters = new OOAdvantech.Collections.Hashtable();
            parameters["@PriceList"] = priceList;
            //StructureSet objectSet = objectStorage.Execute(objectQuery,parameters);
            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IProductPrice productPrice = null;
            int i = 0;
            foreach (StructureSet objectSetInstance in objectSet)
            {

                productPrice = objectSet["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                if (productPrice.PriceList == priceList)
                {
                    i++;
                    productPriceCollection.Add(productPrice);
                }
                if (i > 12)
                    break;
            }
            return productPriceCollection;



        }


    }
}
