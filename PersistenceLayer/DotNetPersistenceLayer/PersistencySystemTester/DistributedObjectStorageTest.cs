using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using AbstractionsAndPersistency;

namespace PersistencySystemTester
{
    /// <MetaDataID>{c5565030-4422-420d-b6d4-7e2eaa4f8f99}</MetaDataID>
    public class DistributedObjectStorageTest
    {
        public ObjectStorage OpenFirstObjectStorage()
        {
            string storageName = "FirstAbstractions";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";
            //storageLocation = @"localhost:1521/xe";
            //storageType = "OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider";
            //storageLocation = @"c:\FirstAbstractions.xml";
            //storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";


            ObjectStorage objectStorage = null;
            try
            {
                objectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                //objectStorage.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    objectStorage = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    objectStorage.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
                }
                catch (System.Exception errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception error)
            {
                int tt = 0;
            }
            return objectStorage;
        }

        public ObjectStorage OpenSecondObjectStorage()
        {
            string storageName = "SecondAbstractions";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";


            //storageLocation = @"c:\SecondAbstractions.xml";
            //storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";




            ObjectStorage objectStorage = null;
            try
            {

                objectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                //storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    objectStorage = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    objectStorage.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
                }
                catch (System.Exception errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception error)
            {
                int tt = 0;
            }
            return objectStorage;
        }


        public void InterStorageInstanceObjectsLink()
        {
            ObjectStorage firstObjectStorage = OpenFirstObjectStorage();
            ObjectStorage secondObjectStorage = OpenSecondObjectStorage();
            IProductPrice theProductPrice = null;
            //IUnitMeasure kilo = null;
            IClient client = null;
            foreach (IProductPrice productPrice in from productPrice in new OOAdvantech.Linq.Storage(firstObjectStorage).GetObjectCollection<IProductPrice>() select productPrice)
            {
                theProductPrice = productPrice;
                break;
            }
         //   kilo = (from unitMesure in new OOAdvantech.Linq.Storage(firstObjectStorage).GetObjectCollection<IUnitMeasure>() select unitMesure).ToList<IUnitMeasure>()[0];
            client = (from theClient in new OOAdvantech.Linq.Storage(firstObjectStorage).GetObjectCollection<IClient>() select theClient).ToList<IClient>()[0];
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                IUnitMeasure kilo = secondObjectStorage.NewObject<UnitMeasure>();
                kilo.Name = "kilo";

                IOrder order = secondObjectStorage.NewObject<Order>();
                order.Client = client;
                order.Name = "AK_1";
                IOrderDetail orderDetail = secondObjectStorage.NewObject<OrderDetail>();
                orderDetail.Name = theProductPrice.Product.Name;
                orderDetail.Price = theProductPrice;
                orderDetail.Quantity = Quantity.GetQuantity(12, kilo);
                order.AddItem(orderDetail);

                System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                AbstractionsAndPersistency.IProduct cocaCola = secondObjectStorage.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                AbstractionsAndPersistency.IProductPrice cocaColaPrice = secondObjectStorage.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                cocaColaPrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(2, default(AbstractionsAndPersistency.IUnitMeasure));
                cocaColaPrice.Product = cocaCola;
                AbstractionsAndPersistency.IPriceList tradePriceList = secondObjectStorage.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                tradePriceList.AddProduct(cocaColaPrice);

                orderDetail = secondObjectStorage.NewObject<OrderDetail>();
                orderDetail.Name = cocaColaPrice.Product.Name;
                orderDetail.Price = cocaColaPrice;
                orderDetail.Quantity = Quantity.GetQuantity(12, kilo);
                order.AddItem(orderDetail);



                order = firstObjectStorage.NewObject<Order>();
                order.Client = client;
                order.Name = "AK_1.5";


                stateTransition.Consistent = true;
            }
        }
        public void DistributedQuery()
        {


            //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenFirstObjectStorage());//OpenSecondObjectStorage());

            //var ordersData = from client in storage.GetObjectCollection<IClient>()
            //                 from order in client.Orders
            //                 from orderDetail in order.OrderDetails
            //                 select new { client, order, orderDetail };//= orderDetail.Fetching(orderDetail.Quantity.UnitMeasure), orderDetail.Quantity.UnitMeasure.Name };//, orderDetail.Price };

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenSecondObjectStorage());
                var ordersData = from order in storage.GetObjectCollection<IOrder>()
                                 select new { client = order.Client, order };//= orderDetail.Fetching(orderDetail.Quantity.UnitMeasure), orderDetail.Quantity.UnitMeasure.Name };//, orderDetail.Price };


                Order newOrder = new Order();
                newOrder.Name = "Makaka";
                Client newClient = new Client("Perikalas");
                newClient.AddOrder(newOrder, 0);
                OpenSecondObjectStorage().CommitTransientObjectState(newOrder);
                OpenFirstObjectStorage().CommitTransientObjectState(newClient);
                foreach (var orderData in ordersData)
                {
                    var client = orderData.order.Client;
                }
                //stateTransition.Consistent = true;

            }
        }

        public void DistributedQueryOnLinkClass()
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenFirstObjectStorage());//OpenSecondObjectStorage());

            var productPrices = from priceList in storage.GetObjectCollection<IPriceList>()
                                from productPrice in priceList.Products
                                select new { productPrice, prName = productPrice.Name, pName = productPrice.Product.Name, productPrice.Product.Quantity.UnitMeasure.Name };//= orderDetail.Fetching(orderDetail.Quantity.UnitMeasure), orderDetail.Quantity.UnitMeasure.Name };//, orderDetail.Price };

            foreach (var prductPriceData in productPrices)
            {

            }
        }


        public void LoadFirstStorageWithObjects()
        {
            try
            {
                ObjectStorage firstOpenStorage = OpenFirstObjectStorage();
                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {

                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IStorePlace spriteStorePlace = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "sprite") as AbstractionsAndPersistency.IStorePlace;
                    AbstractionsAndPersistency.IProduct sprite = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;
                    //sprite.AddStorePlace(spriteStorePlace);

                    AbstractionsAndPersistency.IProductPrice spritePrice = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    spritePrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(1, default(AbstractionsAndPersistency.IUnitMeasure));
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.IProduct cocaCola = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    //cocaCola.AddStorePlace(spriteStorePlace);

                    AbstractionsAndPersistency.IProductPrice cocaColaPrice = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                    cocaColaPrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(2, default(AbstractionsAndPersistency.IUnitMeasure));
                    cocaColaPrice.Product = cocaCola;

                    AbstractionsAndPersistency.IPriceList tradePriceList = firstOpenStorage.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                    tradePriceList.AddProduct(cocaColaPrice);
                    IUnitMeasure unitMeasure = firstOpenStorage.NewObject<UnitMeasure>();
                    unitMeasure.Name = "kilo";
                    IClient client = firstOpenStorage.NewObject<Client>(new Type[1] { typeof(string) }, "Liakos");
                    sysStateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }



        public class CategoryData
        {
            public string CategoryName { get; set; }
            public bool Root { get; set; }
            public System.Collections.Generic.IEnumerable<CategoryData> SubCategories { get; set; }
        }

        public void SaveRecursiveObjects()
        {
            try
            {
                ObjectStorage firstObjectStorage = OpenFirstObjectStorage();
                ObjectStorage secondObjectStorage = OpenSecondObjectStorage();

                //ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.ICategory category = firstObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    category.Root = true;
                    category.Name = "Root";
                    AbstractionsAndPersistency.ICategory subCategory = firstObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub1A";
                    category.AddSubCategory(subCategory);
                    subCategory = firstObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub1B";
                    category.AddSubCategory(subCategory);

                    category = subCategory;
                    subCategory = secondObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub2A";
                    category.AddSubCategory(subCategory);
                    subCategory = secondObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub2B";
                    category.AddSubCategory(subCategory);
                    stateTransition.Consistent = true;
                }


            }
            catch (System.Exception error)
            {




            }


        }
        public void RecursiveQueryOnDistributeData()
        {
            try
            {
                ObjectStorage storageSession = OpenFirstObjectStorage();

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


        public void MoveObjectTest()
        {

            AbstractionsAndPersistency.Order order = null;
            OpenSecondObjectStorage().MoveObject(order, mOrder => mOrder.GetInside(mOrder.OrderDetails), mOrder => mOrder.LetOut(mOrder.Client));


            //AbstractionsAndPersistency.IOrder order=null;
            //OOAdvantech.Linq.ObjectValuesCollector.GetObjectValues(order,
            //            morder => morder.Fetching(morder.OrderDetails, morder.OrderDetails.Item().Price));



        }

    }
}
