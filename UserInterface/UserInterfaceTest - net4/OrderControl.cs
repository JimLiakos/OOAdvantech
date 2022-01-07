using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Collections.Generic;
using AbstractionsAndPersistency;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections;


namespace UserInterfaceTest
{
    /// <MetaDataID>{b28d7c7e-2d63-411a-933d-4c7d8c237998}</MetaDataID>
    public class OrderControl
    {
        /// <MetaDataID>{374ccd53-f8ec-4118-b793-748514400de9}</MetaDataID>
        public static void AddOrderDetail(OOAdvantech.UserInterface.Runtime.ICollectionViewRunTime collectionView, AbstractionsAndPersistency.IOrder order)
        {
            try
            {
                collectionView.AddItem(order.CreateOrderDetail());
            }
            catch (System.Exception error)
            {

            }

        }


        /// <MetaDataID>{588ab8df-36ea-41ac-8d1d-08d169242bc8}</MetaDataID>
        public static void EditClient(IClient client)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.Connection.Instance = client;
            clientForm.ShowDialog();




        }

        /// <MetaDataID>{dbc21eda-ce22-4c76-a5e7-1b09a3cd2df0}</MetaDataID>
        public static void NewClient(OOAdvantech.UserInterface.Runtime.ICollectionViewRunTime collectionView)
        {
            ObjectStorage objectStorage = null;
            objectStorage = Form3.OpenStorage();
            Client client = new Client("Mitsos");
            objectStorage.CommitTransientObjectState(client);
            collectionView.AddItem(client);


        }

        /// <MetaDataID>{f8506238-c4cc-44a7-ad28-4a8fb042a31f}</MetaDataID>
        public static void ShowClientOrders(IClient client)
        {
            OrdersPresentation ordersPresentation = new OrdersPresentation(client);
            OrdersListForm ordersListForm = new OrdersListForm();
            ordersListForm.ConnectionControl.Instance = ordersPresentation;
            ordersListForm.Show();



        }
        /// <MetaDataID>{640caa0f-9693-4f35-afce-855d20bbd460}</MetaDataID>
        public static Set<IOrder> SearchForOrder(System.DateTime timePeriodStart, System.DateTime timePeriodEnd, string clientNameLike)
        {

            Set<IOrder> priceListCollection = new Set<IOrder>();
            ObjectStorage objectStorage = null;
            objectStorage = Form3.OpenStorage();

            string objectQuery = "#OQL: SELECT priceList  " +
                              " FROM AbstractionsAndPersistency.IOrder priceList #";


            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IOrder priceList = null;
            int i = 0;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                priceList = objectSet["priceList"] as AbstractionsAndPersistency.IOrder;
                i++;
                priceListCollection.Add(priceList);
                if (i > 12)
                    break;
            }
            return priceListCollection;

        }

    }


    /// <MetaDataID>{57874eb9-ed38-489a-b48a-5f77f2c42044}</MetaDataID>
    public class TestClass
    {

    }
    /// <MetaDataID>{df6a5dd8-f4a4-47b0-8cad-e1bde74d5917}</MetaDataID>
    public class TestClassB : TestClass
    {
        public override string ToString()
        {
            return base.ToString();
        }
        
    }


    /// <MetaDataID>{c8076599-7511-4bff-9d1e-a4dd3ac50b7c}</MetaDataID>
    public class TemplateClass<T,U>
    {


    }

   

}
