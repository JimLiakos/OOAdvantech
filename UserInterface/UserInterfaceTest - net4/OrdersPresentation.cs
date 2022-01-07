using System;
using System.Text;
using OOAdvantech.Collections.Generic;
using OOAdvantech.Collections;
using OOAdvantech.PersistenceLayer;
using AbstractionsAndPersistency;

namespace UserInterfaceTest
{
    /// <MetaDataID>{0db5fc06-6e63-4239-be16-3f91ab33296a}</MetaDataID>
    public class OrdersPresentation
    {


        public OrdersPresentation(IClient client)
        {
            Client = client;
        }
        public OrdersPresentation()
        {

        }

        IClient Client;
        public Set<IOrder> Orders
        {
            get
            {
                if (Client != null)
                {
                    try
                    {
                        return Client.Orders;
                    }
                    catch (System.Exception error)
                    {
                        return new Set<IOrder>();
                    }
                }
                else
                {
                    Set<IOrder> orders = new Set<IOrder>();
                    ObjectStorage objectStorage = null;
                    objectStorage = Form3.OpenStorage();


                    string objectQuery = "#OQL: SELECT order " +
                                      " FROM AbstractionsAndPersistency.IOrder order #";

                    StructureSet objectSet = objectStorage.Execute(objectQuery);
                    IOrder order = null;
                    foreach (StructureSet objectSetInstance in objectSet)
                    {
                        order = objectSet["order"] as AbstractionsAndPersistency.IOrder;
                        orders.Add(order);
                    }
                    return orders;





                }
            }
        }

    }
}
