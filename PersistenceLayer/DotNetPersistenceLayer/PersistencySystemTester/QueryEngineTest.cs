using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Data;
using OOAdvantech.Collections.Generic;
using OOAdvantech.Collections;
using System.Linq;
using OOAdvantech.Linq;
using AbstractionsAndPersistency;
namespace PersistencySystemTester
{
    /// <MetaDataID>{0fea6f67-251e-4939-b940-fb3f8fa8c385}</MetaDataID>
    public class QueryEngineTest
    {

        /// <MetaDataID>{65669145-3644-469e-a0a8-8df8b29af5ad}</MetaDataID>
        public void QueryOnObjectMemberCollection()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            AbstractionsAndPersistency.IClient theClient = null;
            var clientResult = from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                               select client;


            foreach (AbstractionsAndPersistency.IClient aClient in clientResult)
            {
                theClient = aClient;
                break;
            }
            theClient.GetOrders(System.DateTime.Now, System.DateTime.Now + TimeSpan.FromDays(4));

        }

        /// <MetaDataID>{90aa95e3-34d7-4247-a7be-818027356544}</MetaDataID>
        public void LoadDataForQueryWithEmptySelectorCollection()
        {
            ObjectStorage objectStorage = OpenStorage();

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                           where client.Name == "EmptySelectorCollection"
                           select client).ToArray();

            if (clients.Length == 0)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    AbstractionsAndPersistency.IClient client = new AbstractionsAndPersistency.Client("EmptySelectorCollection");
                    objectStorage.CommitTransientObjectState(client);
                    AbstractionsAndPersistency.IOrder order = new AbstractionsAndPersistency.Order();
                    order.Name = "Ak_m";
                    objectStorage.CommitTransientObjectState(order);
                    client.AddOrder(order, 0);
                    stateTransition.Consistent = true;
                }

            }

        }

        /// <MetaDataID>{4cbf2570-1aad-43b7-8624-f0cc6101c733}</MetaDataID>
        public void RunQueryWithEmptySelectorCollection()
        {
            //the order.OrderDetails collectionSelector select from empty collection
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());

            var orderDetailsData = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                                    from order in client.Orders
                                    from orderDetail in order.OrderDetails
                                    select new { client, order, orderDetail }).ToArray();

        }


        public ObjectStorage OpenStorage(string storageName, string storageLocation, string storageType)
        {

            ObjectStorage storageSession = null;

            try
            {

                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);

            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
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
            return storageSession;


        }

        /// <MetaDataID>{ddc91117-871f-4d6c-8b21-ffe989a6a569}</MetaDataID>
        public ObjectStorage OpenStorage()
        {

            string storageName = "Abstractions";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            //string storageName = "Abstractions";
            //string storageLocation = @"c:\Abstractions.xml";
            //string storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";

            //string storageName = "Abstractions";
            //string storageLocation = @"localhost:1521/xe";
            //string storageType = "OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider";


            //string storageName = "MixedMappingStorage";
            //string storageLocation = @"localhost:1521/xe";
            //string storageType = "OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider";


            //string storageName = "MixedMappingStorage";
            //string storageLocation = @"localhost\sqlexpress";
            //string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            //string storageName = "Abstractions";
            //string storageLocation = @"c:\Abstractions.sdf";
            //string storageType = "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider";


            ObjectStorage storageSession = null;
            try
            {

                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                //storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
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
            return storageSession;


        }


        /// <MetaDataID>{6800b032-f986-463d-8b7b-8269444f5035}</MetaDataID>
        public void TestOverridenComparisonOperator()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            AbstractionsAndPersistency.Quantity quantity = new AbstractionsAndPersistency.Quantity(3m, null);

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                AbstractionsAndPersistency.IUnitMeasure unitMeasure = (from unit in storage.GetObjectCollection<AbstractionsAndPersistency.IUnitMeasure>()
                                                                       select unit).ToList()[0];

                AbstractionsAndPersistency.IUnitMeasure newUnitMeasure = new AbstractionsAndPersistency.UnitMeasure();
                unitMeasure.Name = "Liter";
                AbstractionsAndPersistency.Quantity newQuantity = new AbstractionsAndPersistency.Quantity(3m, newUnitMeasure);
                AbstractionsAndPersistency.Product newProduct = new AbstractionsAndPersistency.Product("Fanta");
                newProduct.Quantity = newQuantity;
                OpenStorage().CommitTransientObjectState(newProduct);
                OpenStorage().CommitTransientObjectState(newUnitMeasure);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(newUnitMeasure,  DeleteOptions.TryToDelete);

                //AbstractionsAndPersistency.IProduct uProduct = (from product in storage.GetObjectCollection<AbstractionsAndPersistency.IProduct>()
                //                                                where product.Name == "coca cola"
                //                                                select product).ToList()[0];
                //uProduct.Quantity = newQuantity;
                //uProduct.Name = "Pasa";
                //var ordersItem = (from product in storage.GetObjectCollection<AbstractionsAndPersistency.IProduct>()
                //                  where product.Quantity == quantity
                //                  select new { product.Name, product.Quantity.Amount }).ToList();
                //foreach (var item in ordersItem)
                //{
                //    //var unit= 
                //}
                stateTransition.Consistent = true;
            }

            //var ordersItem = (from product in storage.GetObjectCollection<AbstractionsAndPersistency.IProduct>()
            //                  where product.Quantity == quantity
            //                  select new { product.Name, product.Quantity.Amount }).ToList();
            //foreach (var item in ordersItem)
            //{
            //    //var unit= 
            //}
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{

            //    AbstractionsAndPersistency.IOrder uOrdere = (from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
            //                                                 where order.Name == "AK_5"
            //                                                 select order).ToList()[0];
            //    //uOrdere.Invoiced = !uOrdere.Invoiced;
            //    AbstractionsAndPersistency.IClient client = new AbstractionsAndPersistency.Client("Lvras");
            //    uOrdere.Client = client;
            //    OpenStorage().CommitTransientObjectState(client);
            //    var orders = (from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
            //                  select new { orderName = order.Name, clientName = order.Client.Name }).ToList();

            //    int tt = 0;

            //}

        }
        public void LoadPrefetchingMechanismTestData()
        {
            string storageName = "PrefetchingMechanismMain";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";
            ObjectStorage prefetchingMechanismMain = OpenStorage(storageName, storageLocation, storageType);
            storageName = "PrefetchingMechanismSecondary";
            ObjectStorage prefetchingMechanismSecondary = OpenStorage(storageName, storageLocation, storageType);



            using (SystemStateTransition sysStateTransition = new SystemStateTransition())
            {

                System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                AbstractionsAndPersistency.IStorePlace spriteStorePlace = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "sprite") as AbstractionsAndPersistency.IStorePlace;
                AbstractionsAndPersistency.IProduct sprite = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;
                sprite.AddStorePlace(spriteStorePlace);

                AbstractionsAndPersistency.IProductPrice spritePrice = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                spritePrice.Product = sprite;

                AbstractionsAndPersistency.IPriceList retailPriceList = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                retailPriceList.AddProduct(spritePrice);

                AbstractionsAndPersistency.IStorePlace cocaStorePlace = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "coca") as AbstractionsAndPersistency.IStorePlace;
                AbstractionsAndPersistency.IProduct cocaCola = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                cocaCola.AddStorePlace(cocaStorePlace);

                AbstractionsAndPersistency.IProductPrice cocaColaPrice = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                cocaColaPrice.Product = cocaCola;

                AbstractionsAndPersistency.IPriceList tradePriceList = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                tradePriceList.AddProduct(cocaColaPrice);
                AbstractionsAndPersistency.IUnitMeasure unit = prefetchingMechanismSecondary.NewObject<AbstractionsAndPersistency.UnitMeasure>();
                unit.Name = "Kilos";

                int orderCount = 40;// 100;
                while (orderCount > 0)
                {
                    AbstractionsAndPersistency.Order order = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "AK_" + orderCount.ToString();
                    int count = 15;
                    while (count > 0)
                    {
                        AbstractionsAndPersistency.OrderDetail orderDetail = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                        orderDetail.Name = "Sprite_" + count.ToString();
                        orderDetail.Price = spritePrice;
                        orderDetail.Quantity = new Quantity(count, unit);
                        order.AddItem(orderDetail);
                        //orderDetail = abstractionsStorageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                        //orderDetail.Name = "coca cola_" + count.ToString();
                        //orderDetail.Price = cocaColaPrice;
                        //orderDetail.Quantity = new Quantity(count, null);
                        //order.AddItem(orderDetail);
                        count--;
                    }
                    AbstractionsAndPersistency.IClient client = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos_" + orderCount.ToString()) as AbstractionsAndPersistency.IClient;
                    order.Client = client;
                    orderCount--;
                }





                sysStateTransition.Consistent = true;
            }


            using (SystemStateTransition sysStateTransition = new SystemStateTransition())
            {

                System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                AbstractionsAndPersistency.IStorePlace spriteStorePlace = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "sprite") as AbstractionsAndPersistency.IStorePlace;
                AbstractionsAndPersistency.IProduct sprite = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;
                sprite.AddStorePlace(spriteStorePlace);

                AbstractionsAndPersistency.IProductPrice spritePrice = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                spritePrice.Product = sprite;

                AbstractionsAndPersistency.IPriceList retailPriceList = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                retailPriceList.AddProduct(spritePrice);

                AbstractionsAndPersistency.IStorePlace cocaStorePlace = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "coca") as AbstractionsAndPersistency.IStorePlace;
                AbstractionsAndPersistency.IProduct cocaCola = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                cocaCola.AddStorePlace(cocaStorePlace);

                AbstractionsAndPersistency.IProductPrice cocaColaPrice = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                cocaColaPrice.Product = cocaCola;

                AbstractionsAndPersistency.IPriceList tradePriceList = prefetchingMechanismMain.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                tradePriceList.AddProduct(cocaColaPrice);
                int orderCount = 40;// 100;
                AbstractionsAndPersistency.IUnitMeasure unit = prefetchingMechanismMain.NewObject<AbstractionsAndPersistency.UnitMeasure>();
                unit.Name = "Kilos";

                while (orderCount > 0)
                {
                    AbstractionsAndPersistency.Order order = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "AK_S_" + orderCount.ToString();
                    int count = 15;
                    while (count > 0)
                    {

                        AbstractionsAndPersistency.OrderDetail orderDetail = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                        orderDetail.Name = "Sprite_S_" + count.ToString();
                        orderDetail.Price = spritePrice;
                        orderDetail.Quantity = new Quantity(count, unit);
                        order.AddItem(orderDetail);
                        //orderDetail = linkedAbstractionsStorageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                        //orderDetail.Name = "coca cola_S_" + count.ToString();
                        //orderDetail.Quantity = new Quantity(count, null);
                        //orderDetail.Price = cocaColaPrice;
                        //order.AddItem(orderDetail);
                        count--;
                    }
                    AbstractionsAndPersistency.IClient client = prefetchingMechanismSecondary.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos_S_" + orderCount.ToString()) as AbstractionsAndPersistency.IClient;
                    order.Client = client;
                    orderCount--;
                }
                sysStateTransition.Consistent = true;
            }


        }
        public void PrefetchingMechanismTest()
        {
            string storageName = "PrefetchingMechanismMain";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";
            var openStorage = OpenStorage(storageName, storageLocation, storageType);

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(openStorage);

            var morders = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                           from order in client.Orders
                           from orderDetail in order.OrderDetails
                           select new { client, order, orderDetail }).ToList();
            foreach (var item in morders)
            {
                if (item.orderDetail.Quantity.UnitMeasure == null)
                {
                    //error
                }
                if (item.orderDetail.Price == null)
                {
                    //error
                }

            }
            int tt = 0;
        }

       
        /// <MetaDataID>{d200e441-7db0-40e1-b0f4-ac4b4ccc5d6c}</MetaDataID>
        public void LoadStorageWithObjects()
        {
            try
            {
                ObjectStorage storageSession = OpenStorage();
                //ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {

                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IStorePlace spriteStorePlace = storageSession.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "sprite") as AbstractionsAndPersistency.IStorePlace;
                    AbstractionsAndPersistency.IProduct sprite = storageSession.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;
                    sprite.AddStorePlace(spriteStorePlace);
                    AbstractionsAndPersistency.LiquidProduct liquidProduct = storageSession.NewObject<AbstractionsAndPersistency.LiquidProduct>(new Type[1] { typeof(string) }, "Milk");
                    AbstractionsAndPersistency.LiquidStore liquidStore = storageSession.NewObject<AbstractionsAndPersistency.LiquidStore>(new Type[1] { typeof(string) }, "Tank22");
                    liquidStore.StoredLiquidProduct = liquidProduct;

                    AbstractionsAndPersistency.IProductPrice spritePrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    spritePrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(1, default(AbstractionsAndPersistency.IUnitMeasure));
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.IProduct cocaCola = storageSession.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    cocaCola.AddStorePlace(spriteStorePlace);
                    AbstractionsAndPersistency.Quantity quantity = new AbstractionsAndPersistency.Quantity();
                    AbstractionsAndPersistency.IUnitMeasure unit = storageSession.NewObject<AbstractionsAndPersistency.UnitMeasure>();
                    unit.Name = "Kilos";
                    quantity.Amount = 2;
                    quantity.UnitMeasure = unit;
                    cocaCola.Quantity = quantity;

                    AbstractionsAndPersistency.IProductPrice cocaColaPrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                    cocaColaPrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(2, default(AbstractionsAndPersistency.IUnitMeasure));
                    cocaColaPrice.Product = cocaCola;

                    AbstractionsAndPersistency.IClient client = storageSession.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos_1") as AbstractionsAndPersistency.IClient;
                    AbstractionsAndPersistency.IPriceList tradePriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                    tradePriceList.AddProduct(cocaColaPrice);
                    int orderCount = 5;
                    int dayOffset = 0;
                    while (orderCount > 0)
                    {
                        AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                        order.OrderDate = DateTime.Now + TimeSpan.FromDays(dayOffset++);
                        order.Name = "AK_" + orderCount.ToString();
                        int count = 15;
                        while (count > 0)
                        {

                            AbstractionsAndPersistency.OrderDetail orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                            orderDetail.Name = "Sprite_" + count.ToString();
                            orderDetail.Price = spritePrice;
                            orderDetail.Quantity = new AbstractionsAndPersistency.Quantity(2, default(AbstractionsAndPersistency.IUnitMeasure));
                            order.AddItem(orderDetail);
                            orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                            orderDetail.Name = "coca cola_" + count.ToString();
                            orderDetail.Price = cocaColaPrice;
                            orderDetail.Quantity = new AbstractionsAndPersistency.Quantity(2, default(AbstractionsAndPersistency.IUnitMeasure));
                            order.AddItem(orderDetail);
                            count--;
                        }

                        order.Client = client;
                        orderCount--;
                    }
                    sysStateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }


        /// <MetaDataID>{0ce89ba6-8d88-403f-be14-7b7ac1a6e508}</MetaDataID>
        public void QueryOnStructureIndexedRelation()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                //string query = "select order, order.OrderDetails orderDetail from AbstractionsAndPersistency.Order order ";


                //string query = "";


                //string query = "select product,product.MinimumQuantity.UnitMeasures unitMesure from AbstractionsAndPersistency.IProduct product ";
                string query = "select product from AbstractionsAndPersistency.IProduct product ";
                //string query = "select orderDetail from AbstractionsAndPersistency.IOrder order ,order.OrderDetails orderDetail ";



                OOAdvantech.Collections.StructureSet structureSet = storageSession.Execute(query);
                foreach (OOAdvantech.Collections.StructureSet instanceSet in structureSet)
                {
                    AbstractionsAndPersistency.IProduct product = instanceSet["product"] as AbstractionsAndPersistency.IProduct;
                    AbstractionsAndPersistency.Quantity min = product.MinimumQuantity;
                    //foreach (AbstractionsAndPersistency.IUnitMeasure unit in product.MinimumQuantity.UnitMeasures)
                    //{
                    //    string name = unit.Name;
                    //}


                    //    //foreach (OOAdvantech.Collections.StructureSet minimumQuantityInstanceSet in instanceSet["MinimumQuantity"] as OOAdvantech.Collections.StructureSet)
                    //    //{

                    //    //    foreach (OOAdvantech.Collections.StructureSet unitInstanceSet in minimumQuantityInstanceSet["UnitMeasures"] as OOAdvantech.Collections.StructureSet)
                    //    //    {
                    //    //        AbstractionsAndPersistency.IUnitMeasure unit = unitInstanceSet["unitMesure"] as AbstractionsAndPersistency.IUnitMeasure;
                    //    //        String name = unit.Name;
                    //    //    }
                    //    //}
                }
            }
            catch (System.Exception error)
            {
                int erka = 0;
            }


        }

        /// <MetaDataID>{c6cfe263-9a37-45ee-ba2c-1e27a633c7a4}</MetaDataID>
        public void QueryOnDateTime()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var orders = (from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                          select new { order.OrderDate.Value.DayOfWeek, order.OrderDate.Value.DayOfYear, order.OrderDate.Value.Date, order.OrderDate.Value.Hour, order.OrderDate.Value.Minute, order.OrderDate.Value.Second }).ToList();

        }

        /// <MetaDataID>{b5c32cf1-839e-4cba-8387-f4e2903e59dc}</MetaDataID>
        public void PreFetchingTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var ordersResult = from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                               from order in client.Orders
                               from orderDetail in order.OrderDetails
                               // where orderDetail.Name == "Sprite_10" && orderDetail.Price.Product.Name == "sprite"
                               select new { client = client.Fetching(client.Orders), orderDetail = orderDetail };

            foreach (var order in ordersResult)
            {
                int count = order.client.Orders.Count;
                if (count > 0)
                {
                    var client = order.client.Orders[0].Client;
                }
                var unit = order.orderDetail.Price.Product.Quantity.UnitMeasure;
            }
            AbstractionsAndPersistency.IProductPrice prodPric = (from prodPrice in storage.GetObjectCollection<AbstractionsAndPersistency.IProductPrice>() select prodPrice).ToArray()[0];
            var priceListResult = from priceList in storage.GetObjectCollection<AbstractionsAndPersistency.IPriceList>()
                                  select priceList;//.Fetching(priceList.Products);
            foreach (var priceList in priceListResult)
            {
                bool c = priceList.Products.Contains(prodPric);
                var count = priceList.Products.Count;
                if (count > 0)
                {
                    var product = priceList.Products[0].Product;
                    var pList = priceList.Products[0].PriceList;

                }
            }


            var storagePlaces = from storagePlace in storage.GetObjectCollection<AbstractionsAndPersistency.IStorePlace>()
                                select new { storagPlace = storagePlace.Fetching(storagePlace.StoredProducts) };
            foreach (var storagePlace in storagePlaces)
            {
                int count = storagePlace.storagPlace.StoredProducts.Count;

                if (storagePlace.storagPlace is AbstractionsAndPersistency.LiquidStore)
                {
                    var product = (storagePlace.storagPlace as AbstractionsAndPersistency.LiquidStore).StoredLiquidProduct;
                }
            }


        }

        #region Grouping and Aggregation functions
        /// <MetaDataID>{8005d001-f121-4936-81fe-614012a25959}</MetaDataID>
        public void DoubleGroupTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var clients = storage.GetObjectCollection<AbstractionsAndPersistency.IClient>();


            //var salesResult = from client in clients
            //                  select new
            //                  {
            //                      clientName = client,
            //                      sales = from order in client.Orders
            //                              from morderdetail in order.OrderDetails
            //                              //where morderdetail.Quantity.Amount >= 23
            //                              group morderdetail by new { morderdetail.Price, order.Name, order.OrderDate.Value.Year, order.OrderDate.Value.Month } into salesGroup
            //                              // where salesGroup.Sum(morderdetail => (morderdetail.Quantity.Amount + 3) * morderdetail.Quantity.Amount) > 2
            //                              //select new { marmaga = salesGroup, salesGroup.Key, keyPrice = salesGroup.Key.Price.Name, salesTotal = salesGroup.Count() }//.Sum(morderdetaild => morderdetaild.Quantity.Amount ) }// new { salesGroup.Key, salesGroup.Key.Price.Product.Name }//new { salesGroup.Key, salesGroup.Key.Name }//,salesGroup.Key.Name}//, salesTotal = salesGroup.Sum(morderdetail=>morderdetail.Quantity.Amount) }
            //                              //select new { keyPrice = salesGroup.Key.Price.Name, salesTotal = salesGroup.Count() }//.Sum(morderdetaild => morderdetaild.Quantity.Amount ) }// new { salesGroup.Key, salesGroup.Key.Price.Product.Name }//new { salesGroup.Key, salesGroup.Key.Name }//,salesGroup.Key.Name}//, salesTotal = salesGroup.Sum(morderdetail=>morderdetail.Quantity.Amount) }
            //                              select new { salesGroup, salesGroupkey = salesGroup.Key, suma = salesGroup.Sum(morderdetail => (morderdetail.Quantity.Amount) * morderdetail.Quantity.Amount) } // {salesGroup, salesGroupkey = salesGroup.Key, product = salesGroup.Key.Price.Name, salesGroup.Key.Year, salesGroup.Key.Month, salesTotal = salesGroup.Count() }//.Sum(morderdetaild => morderdetaild.Quantity.Amount ) }// new { salesGroup.Key, salesGroup.Key.Price.Product.Name }//new { salesGroup.Key, salesGroup.Key.Name }//,salesGroup.Key.Name}//, salesTotal = salesGroup.Sum(morderdetail=>morderdetail.Quantity.Amount) }
            //                  };



            //var Lora = from tmporder in salesResult
            //           select new
            //           {
            //               tmporder.clientName,
            //               sales = from sale in tmporder.sales
            //                       group sale by new { productName = sale.salesGroupkey.Price.Product.Name } into productSales
            //                       select new { productName = productSales.Key.productName, productSales, genSum = productSales.Sum(prods => prods.suma) }
            //           };

            var salesResult = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                              select new
                              {
                                  clientName = client,
                                  sales = from order in client.Orders
                                          from orderdetail in order.OrderDetails
                                          group orderdetail by new { orderdetail.Price, order.Name, order.OrderDate.Value.Year, order.OrderDate.Value.Month } into salesGroup
                                          select new { salesGroup, salesGroupkey = salesGroup.Key, salesGroup.Key.Price.Product, salesGroup.Key.Name, salesGroup.Key.Year, salesGroup.Key.Month, suma = salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount * orderdetail.Price.Price.Amount) } //, salesGroupkey = salesGroup.Key, suma = salesGroup.Sum(morderdetail => (morderdetail.Quantity.Amount + 3) * morderdetail.Quantity.Amount) } // {salesGroup, salesGroupkey = salesGroup.Key, product = salesGroup.Key.Price.Name, salesGroup.Key.Year, salesGroup.Key.Month, salesTotal = salesGroup.Count() }//.Sum(morderdetaild => morderdetaild.Quantity.Amount ) }// new { salesGroup.Key, salesGroup.Key.Price.Product.Name }//new { salesGroup.Key, salesGroup.Key.Name }//,salesGroup.Key.Name}//, salesTotal = salesGroup.Sum(morderdetail=>morderdetail.Quantity.Amount) }
                              };


            var Lora = from tmporder in salesResult
                       select new
                       {
                           tmporder.clientName,
                           sales = from sale in tmporder.sales
                                   group sale by new { productName = sale.salesGroupkey.Price.Product.Name } into productSales
                                   select new { productName = productSales.Key.productName, productSales, genSum = productSales.Sum(prods => prods.suma) }
                       };

            foreach (var entry in Lora)
            {
                foreach (var sale in entry.sales)
                {
                    foreach (var prodSale in sale.productSales)
                    {

                    }
                }

            }


        }
        public void CheckAggregationFunctionWithDoubleFilter()
        {
            ///Αντλεί δεδομένα με collection σε data που πρεπει να διανίσεις δύο collections για να τα αντλίσεις
            ///order.Client.Orders.OrderDetails
            ///Τα αποτελεσματα βγαίνουν με πολλαπλά φίλτρα
            ///Υπάρχει aggregation function αποτέλεσμα με διπλό φίλτρο ένα που έρχετε από την source collection και ένα 
            ///αποκλιστικά για τα counted data

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());

            var clientOrderDetails = from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                     where order.Name == "AK_1"
                                     select new
                                     {
                                         order,
                                         orderDetails = from clientOrder in order.Client.Orders
                                                        from orderDetail in clientOrder.OrderDetails
                                                        where orderDetail.Name.Like("Sprite_*") || orderDetail.Name.Like("Coc*")
                                                        select orderDetail
                                     };
            var aggrResault = from res in clientOrderDetails
                              select new
                              {
                                  res,
                                  //res.order.Name,
                                  itemCount = res.orderDetails.Where(orderDetail => orderDetail.Name != "mak").Count()
                              };
            var aggrResaultD = from mres in aggrResault
                               where mres.itemCount != -3
                               select new
                               {
                                   mres,
                                   morderDetails = from ordet in mres.res.orderDetails
                                                   where ordet.Name != "fas"
                                                   select ordet
                               };

            foreach (var item in aggrResaultD)
            {
                //item.mres.res.
            }

        }

        /// <MetaDataID>{940fd82e-3a8e-4472-bc1c-ac630605cd49}</MetaDataID>
        public void CheckAggregationFunctionWithoutGroup()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());

            var aggrResault = from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                              from ordDetail in theOrder.OrderDetails
                              where theOrder.Name == "AK_1"
                              select new
                              {
                                  theOrder.Name,
                                  orderdetailCount = theOrder.OrderDetails.Count(),
                                  orderdetailSum = theOrder.OrderDetails.Sum(ord => ord.Quantity.Amount),
                                  orderdetailMin = theOrder.OrderDetails.Min(ord => ord.Quantity.Amount),
                                  orderdetailMax = theOrder.OrderDetails.Max(ord => ord.Quantity.Amount),
                                  orderdetailAvrg = theOrder.OrderDetails.Average(ord => ord.Quantity.Amount)
                              };

            foreach (var item in aggrResault)
            {

            }


          

            int countResault = (from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                from ordDetail in theOrder.OrderDetails
                                where theOrder.Name == "AK_1"
                                select ordDetail).Count();

            countResault = (from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                            from ordDetail in theOrder.OrderDetails
                            where theOrder.Name == "AK_1"
                            select ordDetail).Count(ordDetail => ordDetail.Price.Name == "sprite");

            var countResaultCollection = (from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                          where theOrder.Name == "AK_1"
                                          select theOrder.OrderDetails.Count(ordDetail => ordDetail.Price.Name == "sprite")).ToArray();

            countResault = (from ordDetail in storage.GetObjectCollection<AbstractionsAndPersistency.IOrderDetail>()
                            where ordDetail.Order.Name == "AK_1"
                            select ordDetail).Count();

            countResault = (from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                            from ordDetail in theOrder.OrderDetails
                            where theOrder.Name == "AK_1"
                            select ordDetail).Count(orderDetail => orderDetail.Price.Name == "sprite");

            var aggrResaultB = from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                               from ordDetail in theOrder.OrderDetails
                               where theOrder.Name == "AK_1"
                               select new
                               {
                                   theOrder.Name,
                                   orderdetailCount = theOrder.OrderDetails.Count(orderDetail => orderDetail.Price.Name == "sprite"),
                                   orderdetailSum = theOrder.OrderDetails.Sum(orderDetail => orderDetail.Price.Name == "sprite" ? 0 : orderDetail.Quantity.Amount * 2),
                                   orderdetailMin = theOrder.OrderDetails.Min(ord => ord.Quantity.Amount),
                                   orderdetailMax = theOrder.OrderDetails.Max(ord => ord.Quantity.Amount),
                                   orderdetailAvrg = theOrder.OrderDetails.Average(ord => ord.Quantity.Amount)
                               };

            foreach (var item in aggrResaultB)
            {

            }

            //Many To Many relationship
            var productPlacesInfos = from prodPlace in storage.GetObjectCollection<AbstractionsAndPersistency.StorePlace>()
                                     select new
                                     {
                                         prodPlace,
                                         productsCount = prodPlace.StoredProducts.Where(product => product.Name != "sds").Count()


                                     };
            foreach (var productPlaceInfo in productPlacesInfos)
            {

            }



        }



        /// <MetaDataID>{45aa7f1d-fb4c-4f9b-bbb8-e9e06988b430}</MetaDataID>
        public void GroupDerivedTypeQueryTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var resuls = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                         from order in client.Orders
                         from orderDetail in order.OrderDetails
                         select new { client, order, orderDetail };

            var groupData = from mOrderDetail in resuls
                            group mOrderDetail by new { mOrderDetail.client, mOrderDetail.orderDetail.Price.Product, mOrderDetail.order.Name, mOrderDetail.order.OrderDate.Value.Year, mOrderDetail.order.OrderDate.Value.Month } into salesGroup
                            select new { salesGroup, salesGroup.Key.Product, salesGroup.Key.Name, salesGroup.Key.Year, salesGroup.Key.Month, total = salesGroup.Sum(orderdetail => orderdetail.orderDetail.Quantity.Amount * orderdetail.orderDetail.Price.Price.Amount) };

            foreach (var groupDataElement in groupData)
            {
                foreach (var groupedData in groupDataElement.salesGroup)
                {

                }
            }

        }

        public void DerivedMemberQueryTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());


            var queryResultForDerivedMemberA = (from client in storage.GetObjectCollection<IClient>()
                                                from order in client.Orders
                                                from orderDetailProduct in order.OrderDetailProducts
                                                where orderDetailProduct.Order.Invoiced != true
                                                select new { order, orderDetailProduct }).ToArray();

            var queryResultForDerivedMemberB = (from client in storage.GetObjectCollection<IClient>()
                                                from order in client.Orders
                                                select new
                                                {
                                                    order,
                                                    orderDetailProducts = from orderDetailProduct in order.OrderDetailProducts
                                                                          select orderDetailProduct

                                                }).ToArray();


            var queryResultForDerivedMemberC = (from client in storage.GetObjectCollection<IClient>()
                                                from order in client.Orders

                                                from orderDetailProduct in order.FilteredOrderDetailProducts
                                                where orderDetailProduct.Order.Invoiced != true
                                                select new { order, orderDetailProduct }).ToArray();

            var queryResultForDerivedMemberD = (from client in storage.GetObjectCollection<IClient>()
                                                from order in client.Orders
                                                select new
                                                {
                                                    order,
                                                    orderDetailProducts = from orderDetailProduct in order.FilteredOrderDetailProducts
                                                                          select orderDetailProduct
                                                }).ToArray();

        }

        /// <summary>
        /// This method grouping data and gets data related with the grouping collections
        /// </summary>
        public void GetRelatedDataWithGroupingResultTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());

            var ordesInfos = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                             from order in client.Orders
                             group order by new { order.Client, order.OrderDate.Value.Year } into saleGroup
                             select new { saleGroup.Key, saleGroup.Key.Client.Name, saleGroup };

            ///Gets data related with grouped data
            var groupRelatedData = from ordesInfo in ordesInfos
                     select new
                     {
                         ordesInfo = ordesInfo,
                         clientName = ordesInfo.Key.Client.Name,
                         orderDetails = from order in ordesInfo.saleGroup
                                        from orderDetail in order.OrderDetails
                                        where orderDetail.Name.Like("Sprite_S_1*")
                                        select new { orderDetail, orderDetail.Name }
                     };


            foreach (var salesInfo in groupRelatedData)
            {
                int len = salesInfo.orderDetails.ToArray().Length;
            }


            ///Dynamic type result collection
            var dynResults = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                             select new
                             {
                                 client,
                                 clientName = client.Name,
                                 orders = from order in client.Orders
                                          select new
                                          {
                                              orderName = order.Name,
                                              order
                                          }
                             };

            ///Gets data related with grouped data
            var dynOrdesInfos = from dynClient in dynResults
                                from dynOrder in dynClient.orders
                                group dynOrder by new { dynOrder.order.Client, dynOrder.order.OrderDate.Value.Year } into dynSaleGroup
                                select new { dynSaleGroup.Key, dynSaleGroup.Key.Client.Name, dynSaleGroup };

            ///Gets data related with grouped data
            var dynGroupRelatedData = from dynOrdesInfo in dynOrdesInfos
                        select new
                        {
                            dynOrdesInfo,
                            clientName = dynOrdesInfo.Key.Client.Name,
                            orderDetails = from order in dynOrdesInfo.dynSaleGroup
                                           from orderDetail in order.order.OrderDetails
                                           where orderDetail.Name.Like("Sprite_S_1*")
                                           select new { orderDetail, orderDetail.Name }
                        };


            foreach (var salesInfo in dynGroupRelatedData)
            {
                int len = salesInfo.orderDetails.ToArray().Length;
                foreach (var dynSales in salesInfo.dynOrdesInfo.dynSaleGroup)
                {

                }
            }


        }
        public void GroupQueryResultMergeAverageTest()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());

            var salesInfos = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                             from order in client.Orders
                             from orderDetail in order.OrderDetails
                             group orderDetail by new { order.Name, order.OrderDate.Value.Year } into saleGroup
                             select new { saleGroup.Key, itemAverage = saleGroup.Average(ord => ord.Quantity.Amount) };

            foreach (var salesInfo in salesInfos)
            {

            }
        }




        public void GroupQueryResultWithFilteredAggregationTest()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            //grouped orderDetails filtered itemsCount 
            var salesInfos = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                             from order in client.Orders
                             from orderDetail in order.OrderDetails
                             group orderDetail by new { orderDetail.Quantity.Amount, order.OrderDate.Value.Year } into saleGroup
                             select new { saleGroup.Key, itemsCount=saleGroup.Count(), filteredItemsCount = saleGroup.Where(orderDetail => orderDetail.Name.Like("Sprite_S_1*") || orderDetail.Name.Like("Sprite_1*")).Count() };

            foreach (var salesInfo in salesInfos)
            {

            }

            //Client grouped orderDetails filtered itemsCount 
            var clientSalesInfos = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                                   select new
                                   {
                                       client,
                                       sales = from order in client.Orders
                                               from orderDetail in order.OrderDetails
                                               group orderDetail by new { order.Name, order.OrderDate.Value.Year } into saleGroup
                                               select new { saleGroup.Key, itemsCount = saleGroup.Count(), filteredItemsCount = saleGroup.Where(orderDetail => orderDetail.Name.Like("Sprite_S_1*")).Count() }
                                   };


            foreach (var salesInfo in clientSalesInfos)
            {

            }


            //storage place grouped products filtered itemsCount (many to many relationship
            var productPlacesInfos = from prodPlace in storage.GetObjectCollection<AbstractionsAndPersistency.StorePlace>()
                             select new
                             {
                                 prodPlace,
                                 products = from prod in prodPlace.StoredProducts
                                         group prod by new { prod.Name } into saleGroup
                                         select new { saleGroup.Key, itemCount = saleGroup.Where(product => product.Name != "sds").Count() }


                             };

            foreach (var productPlaceInfo in productPlacesInfos)
            {

            }

        }


        public void GroupOnRootQueryTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var salesInfos = from orderDetail in storage.GetObjectCollection<AbstractionsAndPersistency.IOrderDetail>()
                             where orderDetail.Name.Like("Sprite_1*") || orderDetail.Name.Like("coca cola_1*")
                             group orderDetail by new { orderDetail.Price.Product, orderDetail.Order.Name, orderDetail.Order.OrderDate.Value.Year, orderDetail.Order.OrderDate.Value.Month } into salesGroup
                             where salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount * orderdetail.Price.Price.Amount) > 12
                             select new
                             {
                                 salesGroup,
                                 salesGroup.Key,
                                 salesGroup.Key.Name,
                                 total = salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount * orderdetail.Price.Price.Amount),
                                 orderDetailCount = salesGroup.Count(),
                                 orderDetailSum = salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount),
                                 orderDetailMin = salesGroup.Min(orderdetail => orderdetail.Quantity.Amount),
                                 orderDetailMax = salesGroup.Max(orderdetail => orderdetail.Quantity.Amount),
                                 orderDetailAvrg = salesGroup.Average(orderdetail => orderdetail.Quantity.Amount)
                             };


        }
        /// <MetaDataID>{f54795e4-1eb4-4994-8fdf-e26cd3ec1863}</MetaDataID>
        public void GroupQueryTest()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var salesInfos = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                             select new
                             {
                                 client,
                                 orderDetails = from order in client.Orders
                                                from orderDetail in order.OrderDetails
                                                select orderDetail,
                                 sales = from order in client.Orders
                                         from orderdetail in order.OrderDetails
                                         where orderdetail.Name.Like("Sprite_1*") || orderdetail.Name.Like("coca cola_1*")
                                         group orderdetail by new { orderdetail.Price.Product, order.Name, order.OrderDate.Value.Year, order.OrderDate.Value.Month } into salesGroup
                                         where salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount * orderdetail.Price.Price.Amount) > 12
                                         select new
                                         {
                                             salesGroup,
                                             salesGroup.Key.Product,
                                             salesGroup.Key.Name,
                                             salesGroup.Key.Year,
                                             salesGroup.Key.Month,
                                             total = salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount * orderdetail.Price.Price.Amount),
                                             orderDetailCount = salesGroup.Count(),
                                             orderDetailSum = salesGroup.Sum(orderdetail => orderdetail.Quantity.Amount),
                                             orderDetailMin = salesGroup.Min(orderdetail => orderdetail.Quantity.Amount),
                                             orderDetailMax = salesGroup.Max(orderdetail => orderdetail.Quantity.Amount),
                                             orderDetailAvrg = salesGroup.Average(orderdetail => orderdetail.Quantity.Amount)
                                         } //, salesGroupkey = salesGroup.Key, suma = salesGroup.Sum(morderdetail => (morderdetail.Quantity.Amount + 3) * morderdetail.Quantity.Amount) } // {salesGroup, salesGroupkey = salesGroup.Key, product = salesGroup.Key.Price.Name, salesGroup.Key.Year, salesGroup.Key.Month, salesTotal = salesGroup.Count() }//.Sum(morderdetaild => morderdetaild.Quantity.Amount ) }// new { salesGroup.Key, salesGroup.Key.Price.Product.Name }//new { salesGroup.Key, salesGroup.Key.Name }//,salesGroup.Key.Name}//, salesTotal = salesGroup.Sum(morderdetail=>morderdetail.Quantity.Amount) }
                             };






            //var salesInfosa = from ord in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
            //                  select new { ord.OrderDate.Value.Year };
            //foreach (var clientSalesInfoa in salesInfosa)
            //{
            //}

            foreach (var clientSalesInfo in salesInfos)
            {
                foreach (var salesInfo in clientSalesInfo.sales)
                {
                    foreach (var saleGroup in salesInfo.salesGroup)
                    {

                    }

                    System.Diagnostics.Debug.WriteLine(salesInfo.ToString());


                }

            }

            //{ Name = coca cola, Year = 2012, Month = 2, total = 120 }
            //{ Name = sprite, Year = 2012, Month = 2, total = 60 }

            //{ Name = sprite, Year = 2012, Month = 2, total = 60 }
            //{ Name = coca cola, Year = 2012, Month = 2, total = 120 }
            //{ Name = sprite, Year = 2012, Month = 2, total = 60 }
            //{ Name = coca cola, Year = 2012, Month = 2, total = 120 }

        }

        #endregion

        /// <MetaDataID>{5aa47477-9545-4c94-8203-64101317ed93}</MetaDataID>
        public void LoadIndexChangeTestObjects()
        {
            ObjectStorage openStorage = OpenStorage();
            using (SystemStateTransition sysStateTransition = new SystemStateTransition())
            {
                var order = openStorage.NewObject<AbstractionsAndPersistency.Order>();
                order.Name = "Loasas";
                AbstractionsAndPersistency.IOrderDetail orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                orderDetail.Name = "PepsiColaa0";
                order.AddItem(orderDetail);
                orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                orderDetail.Name = "PepsiColaa1";
                order.AddItem(orderDetail);
                orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                orderDetail.Name = "PepsiColaa2";
                order.AddItem(orderDetail);

                orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                orderDetail.Name = "PepsiColaa3";
                order.AddItem(orderDetail);

                sysStateTransition.Consistent = true;

            }

        }
        /// <MetaDataID>{5e087b98-8219-4f5b-9eb0-1bf46d8fa3c7}</MetaDataID>
        public void IndexChangeTest()
        {
            try
            {
                //ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                ObjectStorage openStorage = OpenStorage();
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(openStorage);

                AbstractionsAndPersistency.IOrder order = (from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                                           where theOrder.Name == "Loasas"
                                                           select theOrder).ToArray()[0];

                var orderDetails = (from theOrderDetail in storage.GetObjectCollection<AbstractionsAndPersistency.IOrderDetail>()
                                    where theOrderDetail.Name == "PepsiColaa2" || theOrderDetail.Name == "PepsiColaa0"
                                    select theOrderDetail).ToArray();



                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    //int mer = order.OrderDetails.Count;
                    //order.RemoveItem(orderDetails[1]);
                    //order.RemoveItem(orderDetails[0]);
                    orderDetails[1].Order = null;
                    orderDetails[0].Order = null;
                    AbstractionsAndPersistency.IOrderDetail orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "PepsiColaaNew";
                    orderDetail.Order = order;
                    //order.AddItem(0, orderDetail);

                    stateTransition.Consistent = true;
                }



            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }


        /// <MetaDataID>{4578a503-7ebf-49b1-af06-4c050f982e16}</MetaDataID>
        public void QueryOnIndexedRelation()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                //string query = "select order, order.OrderDetails orderDetail from AbstractionsAndPersistency.Order order ";
                string query = "select order from AbstractionsAndPersistency.Order order ";

                OOAdvantech.Collections.StructureSet structureSet = storageSession.Execute(query);
                foreach (OOAdvantech.Collections.StructureSet instanceSet in structureSet)
                {
                    AbstractionsAndPersistency.IOrder order = instanceSet["order"] as AbstractionsAndPersistency.IOrder;
                    foreach (AbstractionsAndPersistency.IOrderDetail orderDetail in order.OrderDetails)
                    {
                        String name = orderDetail.Name;

                    }
                    //foreach (OOAdvantech.Collections.StructureSet orderDetailsInstanceSet in instanceSet["OrderDetails"] as OOAdvantech.Collections.StructureSet)
                    //{

                    //    AbstractionsAndPersistency.IOrderDetail orderDetail = orderDetailsInstanceSet["orderDetail"] as AbstractionsAndPersistency.IOrderDetail;
                    //    String name = orderDetail.Name;

                    //}
                }



            }
            catch (System.Exception error)
            {
                int erka = 0;
            }


        }

        /// <MetaDataID>{1254ad6e-0e88-4fdb-9da3-b657a6938dd3}</MetaDataID>
        public void RunDistributedQuery()
        {
            try
            {





                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                                    @"localhost\Debug",
                                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");



                //AbstractionsAndPersistency.IOrder order;
                AbstractionsAndPersistency.IOrderDetail order;

                string objectQuery = "#OQL: SELECT order.Name OrderName, order.OrderDetails.Name OrderDetailName" +
                    " FROM AbstractionsAndPersistency.IOrder order #";


                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    object _obj = objectSetInstance["OrderName"];
                    _obj = objectSetInstance["OrderDetails"];
                    foreach (StructureSet subObjectSetInstance in objectSetInstance["OrderDetails"] as StructureSet)
                    {
                        _obj = subObjectSetInstance["OrderDetailName"];

                    }
                    int tt = 0;
                }


            }
            catch (System.Exception error)
            {
            }
        }
        /// <summary>
        /// Test data Loaders 
        /// </summary>
        /// <MetaDataID>{9b85d8de-c002-4b32-ad08-c83f0230d1a3}</MetaDataID>
        public void RunQueryWithTowDataNodeInSelectClause()
        {
            try
            {


                ObjectStorage storageSession = OpenStorage();



                //AbstractionsAndPersistency.IOrder order;
                AbstractionsAndPersistency.IOrderDetail order;





                string objectQuery = "#OQL: SELECT order.Name OrderName, order.OrderDetails.Name OrderDetailName" +
                    " FROM AbstractionsAndPersistency.IOrder order WHERE order.OrderDetails.Price.Name <> 'as' #";

                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    object _obj = objectSetInstance["OrderName"];
                    _obj = objectSetInstance["OrderDetails"];
                    foreach (StructureSet subObjectSetInstance in objectSetInstance["OrderDetails"] as StructureSet)
                    {
                        _obj = subObjectSetInstance["OrderDetailName"];

                    }
                    int tt = 0;
                }
            }
            catch (System.Exception Error)
            {
                int aw = 0;
            }

        }

        /// <MetaDataID>{dafa6368-24c8-4c77-a45d-0b1ece63b943}</MetaDataID>
        public void RunQueryOnCube()
        {


            try
            {

                //ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                ObjectStorage storageSession = OpenStorage();


                //AbstractionsAndPersistency.IOrderDetail order;
                System.DateTime Start = System.DateTime.Now;
                //string objectQuery = "#OQL: SELECT order.Name OrderName, order.OrderDetails.Name OrderDetailName" +
                //    " FROM AbstractionsAndPersistency.IOrder order #";
                //string objectQuery = "#OQL: SELECT order.Name OrderName ,  order.OrderDetails.Name OrderDetailName " +
                //" FROM AbstractionsAndPersistency.IOrder order  WHERE order.OrderDetails.Price.Product.StorePlaces.Name = order.Client.Name    OR order.Name=order.OrderDetails.Price.Name  OR order.OrderDetails.Price.Product.Name  = 'sprite' #";
                string objectQuery = @"#OQL: SELECT order, order.Name OrderName ,  order.OrderDetails.Name OrderDetailName 
                                 FROM AbstractionsAndPersistency.IOrder order  WHERE order.Name <> 'order.Client.Name'   OR (order.OrderDetails.Price.Product.Name  = 'sprite' AND order.OrderDetails.Price.Product.Name ='kes') #";

                //string objectQuery = "#OQL: SELECT storePlace.Name OrderName, storePlace.StoredProducts.Name OrderDetailName ,storePlace.StoredProducts.PriceLists.Name priceListName" +
                //" FROM AbstractionsAndPersistency.IStorePlace storePlace #";



                //AbstractionsAndPersistency.IStorePlace storePlace;

                //AbstractionsAndPersistency.IOrderDetail a;
                //objectQuery = "#OQL: SELECT order " +
                //         " FROM AbstractionsAndPersistency.IOrder order #";


                int count = 0;
                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {


                    AbstractionsAndPersistency.IOrder order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;

                    // order.ObjectChangeState += new OOAdvantech.ObjectChangeStateHandle(order_ObjectChangeState);
                    order.Update();
                    // order.ObjectChangeState -= new OOAdvantech.ObjectChangeStateHandle(order_ObjectChangeState);
                    order.Update();
                    order = null;



                    //foreach (AbstractionsAndPersistency.IOrderDetail orderDetail in order.OrderDetails)
                    //{
                    //    int tt = 0;
                    //}

                    //StructureSet subobjectSet = objectSetInstance["m_Order"] as StructureSet;
                    //foreach (StructureSet subobjectSetInstance in subobjectSet)
                    //{

                    //    object sobj = subobjectSetInstance["OrderDetailName"];
                    //    count++;
                    ////    StructureSet subobjectSet2 = subobjectSetInstance["PriceLists"] as StructureSet;
                    ////    foreach (StructureSet subobjectSetInstance2 in subobjectSet2)
                    ////    {
                    ////        object sobj2 = subobjectSetInstance2["priceListName"];

                    ////    }
                    //}


                }
                System.DateTime End = System.DateTime.Now;
                System.TimeSpan span = End - Start;
                System.Diagnostics.Debug.WriteLine(span);
                objectSet = null;



            }
            catch (System.Exception Error)
            {


            }
            GC.Collect();

        }
        /// <MetaDataID>{0c185f49-f14a-45a8-9f20-7b8ea91ef617}</MetaDataID>
        public void RunContainsAnyQuery()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                           select client.Fetching(client.Orders)).ToList();

            var orders = new System.Collections.Generic.List<AbstractionsAndPersistency.IOrder>() { clients[2].Orders[0], clients[3].Orders[0] };
            clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                       where client.Orders.ContainsAny(orders)
                       select client).ToList();
            int tmp = 0;
        }
        /// <MetaDataID>{eb2552cb-b865-46fa-82a6-2e8c422a5c14}</MetaDataID>
        public void RunContainsAllQuery()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                           select client.Fetching(client.Orders)).ToList();

            var orders = new System.Collections.Generic.List<AbstractionsAndPersistency.IOrder>() { clients[2].Orders[0], clients[2].Orders[1] };
            clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                       where client.Orders.ContainsAll(orders)
                       select client).ToList();
            int tmp = 0;
        }

        /// <MetaDataID>{ad9ee98a-6d0e-4d46-a1f7-49fcc39624ef}</MetaDataID>
        public void RunContainsAnyOnAttributeQuery()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                           select client.Fetching(client.Orders)).ToList();
            var orders = new System.Collections.Generic.List<string>() { clients[0].Orders[0].Name, clients[0].Orders[1].Name };
            var clientsOrders = from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                                select new
                                {
                                    Client = client,
                                    ClientOrderNames = from order in client.Orders
                                                       select order.Name
                                };
            clients = (from client in clientsOrders
                       where client.ClientOrderNames.ContainsAny(orders)
                       select client.Client).ToList();


            int tmp = 0;

        }

        /// <MetaDataID>{c7218f52-9fd9-44ee-96d7-58c741751650}</MetaDataID>
        public void RunContainsAllOnAttributeQuery()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var clients = (from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                           select client.Fetching(client.Orders)).ToList();

            var orders = new System.Collections.Generic.List<string>(from order in clients[0].Orders select order.Name);// { clients[0].Orders[0], clients[1].Orders[0] };


            var clientsOrders = from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                                select new
                                {
                                    Client = client,
                                    ClientOrderNames = from order in client.Orders
                                                       select order.Name
                                };
            clients = (from client in clientsOrders
                       where client.ClientOrderNames.ContainsAll(orders)
                       select client.Client).ToList();


            int tmp = 0;



        }
        /// <MetaDataID>{f578f621-85b5-462f-bf55-19561fb2d8dc}</MetaDataID>
        public void RunQueryWithIsNullCriterion()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                AbstractionsAndPersistency.Order newOrder = storage.ObjectStorage.NewObject<AbstractionsAndPersistency.Order>();
                newOrder.Name = "Kitsos";
                var salesInfos = (from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                  where order.Client == null
                                  select order).ToList();
                int count = salesInfos.Count;
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{af13166f-81a8-4605-aae2-a0c2bf6b2dca}</MetaDataID>
        public void RunQueryHastRelationWithObject()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            AbstractionsAndPersistency.IClient client = (from theClient in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                                                         select theClient).ToList()[0];
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                AbstractionsAndPersistency.Order newOrder = storage.ObjectStorage.NewObject<AbstractionsAndPersistency.Order>();
                newOrder.Name = "Kitsos";
                var salesInfos = (from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                  where order.Client == client
                                  select order).ToList();
                int count = salesInfos.Count;
                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{d2e16e20-55ad-46bd-92ff-b75948c4aa2c}</MetaDataID>
        public void RunQueryOnNullableAttribute()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());
            var salesInfos = from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                             from orderDetail in order.OrderDetails
                             //where orderDetail.Quantity.UnitMeasure.Name=="kilo"
                             select new { Order = order, orderDetail, OrderDate = order.OrderDate.Value };


            foreach (var orderData in salesInfos)
            {

            }


        }



        /// <MetaDataID>{eac16b2d-23b4-443d-b503-66a893584d8a}</MetaDataID>
        public void RunQueryOnAbstractClass()
        {
            try
            {



                ObjectStorage storageSession = ObjectStorage.OpenStorage("AbstracClasses",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                string objectQuery = "SELECT order.Name,order.OrderDetails.Price price " +
                    "FROM " + typeof(AbstractionsAndPersistency.Order).FullName + " order WHERE order.OrderDetails.Price.Name ='Lola'";




                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.ProductPrice price = objectSetInstance["price"] as AbstractionsAndPersistency.ProductPrice;
                    string priceName = price.Name;

                    //					string orderDetailName=orderDetail.Name;
                    int tt = 0;
                }

            }
            catch (System.Exception error)
            {
                int ert = 0;
            }

        }

        /// <MetaDataID>{aaa591df-35f5-461a-b3eb-5e2a1a2a0c67}</MetaDataID>
        public void RunQueryOnInterfaceWithoutPersistentSubClass()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                string objectQuery = "SELECT product, unitMeasure " +
                    "FROM " + typeof(AbstractionsAndPersistency.IProduct).FullName + " product, product.UnitMeasures unitMeasure ";//+
                //"WHERE unitMeasure.Name = 'Mert'";

                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IProduct product = objectSetInstance["product"] as AbstractionsAndPersistency.IProduct;

                    string productName = product.Name;
                    int tt = 0;
                }

            }
            catch (System.Exception error)
            {
                int ert = 0;
            }

        }
        /// <summary>
        /// This test case checks the system if builds the correct SQL query, 
        /// when the abstract link class hasn’t storage cells.
        /// </summary>
        /// <MetaDataID>{7c3ee87b-6855-4ea1-95d8-90c87aa9badd}</MetaDataID>
        public void QueryOnAbstractLinkClassWithoutObjects()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");



                //AbstractionsAndPersistency.IProductPrice gg;
                //gg.Product.StorePlaces;
                string objectQuery = "#OQL: SELECT priceList.Name priceListName, priceList.ProductPrice.Name productPriceName, priceList.ProductPrice.Product.StorePlaces.Name storePlacesName" +
                    " FROM AbstractionsAndPersistency.IPriceList priceList  WHERE priceList.ProductPrice.Product.StorePlaces.Name='test'#";

                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IProductPrice productPrice = objectSetInstance["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                    int tt = 0;
                }
            }
            catch (System.Exception Error)
            {
                int aw = 0;
            }

        }
        /// <summary>
        /// This test case checks the system if builds the correct SQL query, 
        /// when the abstract link class hasn’t storage cells.
        /// </summary>
        /// <MetaDataID>{748cc64a-3570-4c49-93bb-d07cd585cf11}</MetaDataID>
        public void QueryOnManyToManyAssociationWithoutLinks()
        {
            try
            {


                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");



                //AbstractionsAndPersistency.IProductPrice gg;
                //gg.Product.StorePlaces;
                string objectQuery = "#OQL: SELECT priceList.Name priceListName, priceList.ProductPrice.Name productPriceName, priceList.ProductPrice.Product.StorePlaces.Name storePlacesName" +
                    " FROM AbstractionsAndPersistency.IPriceList priceList  WHERE priceList.ProductPrice.Product.StorePlaces.Name='test'#";

                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IProductPrice productPrice = objectSetInstance["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                    int tt = 0;
                }
            }
            catch (System.Exception Error)
            {
                int aw = 0;
            }


        }
        /// <MetaDataID>{01229b84-c5e2-4c79-95e9-144572e4e0ce}</MetaDataID>
        public void RecursiveQueryOld()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                                                        @"localhost\Debug",
                                                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                AbstractionsAndPersistency.ICategory category;


                string recursiveSQLQuery = @"WITH TemporaryTable([RefernceColumns], [ObjectIDColumns], iteration,[DataColumns]) 
                AS (
                SELECT     [RefernceColumns], [ObjectIDColumns], 0 AS iteration,[DataColumns]
                FROM         [RecursiveDataSource]
                WHERE     ([StartPointObjectIDFilter])
                UNION ALL
                SELECT     [ChildRefernceColumns], [ChildObjectIDColumns], parent.iteration + 1 AS iteration,[ChildDataColumns]
                FROM       TemporaryTable AS parent INNER JOIN
                            [RecursiveDataSource] AS child ON [InnerJoinColumns]
                WHERE     (parent.iteration < [RecursiveStep]))

                SELECT     [RefernceColumns], [ObjectIDColumns], iteration,[DataColumns]
                FROM         TemporaryTable";

                recursiveSQLQuery = recursiveSQLQuery.Replace("[ObjectIDColumns]", "ObjectID");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[RefernceColumns]", "SubCategory_ObjectIDB");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveDataSource]", "dbo.Abstract_ICategory");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[StartPointObjectIDFilter]", "SubCategory_ObjectIDB ='557D0F5B-A562-47F5-AB81-9F9DB6E70AC6'");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[DataColumns]", "TypeID, Name, Root");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildDataColumns]", "child.TypeID, child.Name, child.Root");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildObjectIDColumns]", "child.ObjectID");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[ChildRefernceColumns]", "child.SubCategory_ObjectIDB");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[InnerJoinColumns]", " parent.ObjectID=child.SubCategory_ObjectIDB");
                recursiveSQLQuery = recursiveSQLQuery.Replace("[RecursiveStep]", "2");






                string objectQuery = @"#OQL: SELECT category, category.Recursive(SubCategories,2) subCategories
                                    FROM AbstractionsAndPersistency.ICategory category 
                                    WHERE category.Name='Sub2B'#";
                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    StructureSet tt = objectSetInstance["SubCategories"] as StructureSet;
                    object adsd = objectSetInstance["category"];
                    object zz = objectSetInstance["subCategories"];
                    foreach (StructureSet intt in tt)
                    {
                        object mm = intt["Object"];

                        string name = (intt["Object"] as AbstractionsAndPersistency.ICategory).Name;

                        StructureSet ttb = intt["SubCategories"] as StructureSet;

                        foreach (StructureSet inttb in ttb)
                        {
                            object mmb = intt["Object"];

                            string nameb = (intt["Object"] as AbstractionsAndPersistency.ICategory).Name;

                        }

                    }

                }




            }
            catch (System.Exception error)
            {


            }

        }

        public class CategoryData
        {
            public string CategoryName { get; set; }
            public bool Root { get; set; }
            public System.Collections.Generic.IEnumerable<CategoryData> SubCategories { get; set; }
        }
        /// <MetaDataID>{5192ae1d-e1da-47cf-a9e1-a7ad6c8ff979}</MetaDataID>
        public void RecursiveQuery()
        {
            try
            {
                ObjectStorage storageSession = OpenStorage();

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(storageSession);
                AbstractionsAndPersistency.ICategory category;
                var cats = from mcategory in storage.GetObjectCollection<AbstractionsAndPersistency.ICategory>()
                           where mcategory.Root
                           select new CategoryData
                           {
                               CategoryName = mcategory.Name,
                               Root = mcategory.Root
                               ,
                               SubCategories = mcategory.SubCategories.Recursive<CategoryData>(4)
                           };
                foreach (var cat in cats)
                {
                    foreach (var catt in cat.SubCategories)
                    {
                        foreach (var cattt in catt.SubCategories)
                        {
                            foreach (var catttt in cattt.SubCategories)
                            {
                            }
                        }
                    }
                }

                //                AbstractionsAndPersistency.ICategory category;
                //                string objectQuery = @"#OQL: SELECT category, category.Recursive(SubCategories,2) subCategories
                //                                    FROM AbstractionsAndPersistency.ICategory category 
                //                                    WHERE category.Name='Root'#";
                //                StructureSet objectSet = storageSession.Execute(objectQuery);
                //                foreach (StructureSet objectSetInstance in objectSet)
                //                {
                //                    StructureSet tt = objectSetInstance["SubCategories"] as StructureSet;
                //                    object adsd = objectSetInstance["category"];
                //                    object zz = objectSetInstance["subCategories"];
                //                    foreach (StructureSet intt in tt)
                //                    {
                //                        object mm = intt["Object"];

                //                        string name = (intt["Object"] as AbstractionsAndPersistency.ICategory).Name;

                //                        StructureSet ttb = intt["SubCategories"] as StructureSet;

                //                        foreach (StructureSet inttb in ttb)
                //                        {
                //                            object mmb = intt["Object"];

                //                            string nameb = (intt["Object"] as AbstractionsAndPersistency.ICategory).Name;

                //                        }
                //                    }
                //                }
            }
            catch (System.Exception error)
            {


            }

        }


        /// <MetaDataID>{dd172b47-d1bd-4b90-ac5e-019061ddb365}</MetaDataID>
        public void SaveRecursiveObjects()
        {
            try
            {
                ObjectStorage storageSession = OpenStorage();
                //ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.ICategory category = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    category.Root = true;
                    category.Name = "Root";
                    AbstractionsAndPersistency.ICategory subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub1A";
                    category.AddSubCategory(subCategory);
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub1B";
                    category.AddSubCategory(subCategory);
                    category = subCategory;
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub2A";
                    category.AddSubCategory(subCategory);
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub2B";
                    category.AddSubCategory(subCategory);
                    stateTransition.Consistent = true;
                }


            }
            catch (System.Exception error)
            {




            }


        }

        /// <MetaDataID>{068bac16-b090-4b20-b429-3efd2138f2e2}</MetaDataID>
        public void ObjectWithLiteralCriterionTest()
        {

            try
            {
                ObjectStorage storageSession = OpenStorage();

                string objectQuery = @"SELECT [order]
                                 FROM AbstractionsAndPersistency.IOrder [order]  WHERE [order] = ObjectID('cd7e43be-08ac-4cca-b04a-05d10102907d')";
                StructureSet objectSet = storageSession.Execute(objectQuery);
                int count = 0;
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    count++;
                }

            }
            catch (System.Exception error)
            {

            }
        }


        /// <MetaDataID>{092cbc79-de57-4f30-9455-e6e95c0c68b0}</MetaDataID>
        public void ObjectAttributeWithLiteralCriterionTest()
        {

            try
            {



                ObjectStorage storageSession = OpenStorage();

                string objectQuery = @"#OQL: SELECT [order], [order].Name OrderName ,  [order].OrderDetails.Name OrderDetailName 
                                 FROM AbstractionsAndPersistency.IOrder [order]  WHERE [order].OrderDetails.Quantity.Amount<=22 ORDER BY [order].OrderDetails.Quantity.Amount DESC #";
                StructureSet objectSet = storageSession.Execute(objectQuery);
                int count = 0;
                string name = "";
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IOrder order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;
                    name = order.Name;



                    count++;
                    System.Diagnostics.Debug.WriteLine(name);
                }

            }
            catch (System.Exception error)
            {

            }
        }

        /// <MetaDataID>{37668de7-d574-4cf4-8df2-8de22f8e44bc}</MetaDataID>
        public void ObjectAttributeWithParameterCriterionTest()
        {

            try
            {
                ObjectStorage storageSession = OpenStorage();

                string objectQuery = @"#OQL: SELECT order, order.Name OrderName ,  order.OrderDetails.Name OrderDetailName 
                                 FROM AbstractionsAndPersistency.IOrder order  WHERE order.OrderDate<@orderDate  #";

                OOAdvantech.Collections.Generic.Dictionary<string, object> parameters = new OOAdvantech.Collections.Generic.Dictionary<string, object>(1);
                parameters["@orderDate"] = System.DateTime.Parse("2/6/2008 2:05:02 μμ");

                StructureSet objectSet = storageSession.Execute(objectQuery, parameters);
                int count = 0;
                string name = "";
                foreach (StructureSet objectSetInstance in objectSet)
                {

                    AbstractionsAndPersistency.IOrder order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;


                    count++;
                    name = order.Name;
                    System.Diagnostics.Debug.WriteLine(name);
                }


            }
            catch (System.Exception error)
            {

            }


        }


        /// <MetaDataID>{38dcfab7-5980-48aa-b942-47c8ae271dbd}</MetaDataID>
        public void ObjectWithParameterCriterionTest()
        {

            try
            {
                ObjectStorage storageSession = OpenStorage();

                string objectQuery = @"#OQL: SELECT order, order.Name OrderName ,  order.OrderDetails.Name OrderDetailName 
                                 FROM AbstractionsAndPersistency.IOrder order  WHERE ObjectID('253a3774-3875-4c13-9dbf-7d5fed29f9ff') = order  #";
                StructureSet objectSet = storageSession.Execute(objectQuery);
                int count = 0;
                AbstractionsAndPersistency.IOrder order = null;
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;
                    count++;
                }


                objectQuery = @"#OQL: SELECT order, order.Name OrderName ,  order.OrderDetails.Name OrderDetailName 
                                 FROM AbstractionsAndPersistency.IOrder order  WHERE @order=order   #";

                OOAdvantech.Collections.Generic.Dictionary<string, object> parameters = new OOAdvantech.Collections.Generic.Dictionary<string, object>(1);
                parameters["@order"] = order;

                objectSet = storageSession.Execute(objectQuery, parameters);
                count = 0;
                string name = order.Name;
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;
                    count++;
                    name = order.Name;
                }




            }
            catch (System.Exception error)
            {

            }


        }

    }
}
