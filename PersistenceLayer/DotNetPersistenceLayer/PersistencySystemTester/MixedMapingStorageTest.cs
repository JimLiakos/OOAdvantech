using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using OOAdvantech.Linq;
using AbstractionsAndPersistency;
using OOAdvantech.Collections;
namespace PersistencySystemTester
{
    /// <MetaDataID>{839ece5c-a998-4689-b6e0-f3b6d7f6f682}</MetaDataID>
    public class MixedMapingStorageTest
    {

        public ObjectStorage OpenStorage()
        {
            //string storageName = "Abstractions";
            //string storageLocation = @"c:\Abstractions.sdf";
            //string storageType = "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider";
            //string storageName = "Abstractions";
            //string storageLocation = @"c:\Abstractions.xml";
            //string storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";

            //string storageName = "Abstractions";
            //string storageLocation = @"localhost\sqlexpress";
            //string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            //string storageName = "MixedMappingStorage";
            //string storageLocation = @"localhost\sqlexpress";
            //string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            string storageName = "MixedMappingStorage";
            string storageLocation = @"c:\MixedMappingStorage.sdf";
            string storageType = "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider";


            //string storageName = "MixedMappingStorage";
            //string storageLocation = @"localhost:1521/xe";
            //string storageType = "OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider";


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
                    storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName, "SingularControl");
                    //storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
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
                    //sprite.AddStorePlace(spriteStorePlace);

                    AbstractionsAndPersistency.IProductPrice spritePrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    spritePrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(1, default(AbstractionsAndPersistency.IUnitMeasure));
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.IProduct cocaCola = storageSession.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    //cocaCola.AddStorePlace(spriteStorePlace);

                    AbstractionsAndPersistency.IProductPrice cocaColaPrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                    cocaColaPrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(2, default(AbstractionsAndPersistency.IUnitMeasure));
                    cocaColaPrice.Product = cocaCola;

                    AbstractionsAndPersistency.IClient client = storageSession.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos" ) as AbstractionsAndPersistency.IClient;
                    AbstractionsAndPersistency.IPriceList tradePriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                    tradePriceList.AddProduct(cocaColaPrice);
                    int orderCount = 2;
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

        public void CountQueryOnOneToManyMixMapping()
        {


            ObjectStorage objectStorage = OpenStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                IProduct product = objectStorage.NewObject<Product>();
                Quantity quantity = new Quantity();
                IUnitMeasure unit = objectStorage.NewObject<UnitMeasure>();
                unit.Name = "Kilos";
                quantity.Amount = 2;
                quantity.UnitMeasure = unit;
                product.Quantity = quantity;

                var products = from theProduct in storage.GetObjectCollection<IProduct>()
                               where theProduct.Quantity.UnitMeasure.Name == "Kilos"
                               select theProduct;

                foreach (IProduct myProduct in products)
                {

                }

                //return;
            }




            var clients = from client in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                          where client.Name == "Liakos_1"
                          select client;
            IClient _client = null;
            foreach (IClient client in clients)
            {
                int count = client.Orders.Count;
                _client = client;
                break;
            }


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {


                IProductPrice price = _client.Orders[0].OrderDetails[0].Price;
                IOrder order = objectStorage.NewObject<Order>();

                order.Client = _client;
                IOrderDetail orderDetail = objectStorage.NewObject<OrderDetail>();
                orderDetail.Price = _client.Orders[0].OrderDetails[0].Price;
                order.AddItem(orderDetail);

                string query = @"select [client].count(Orders.OrderDetails ) [count]
                            from AbstractionsAndPersistency.IClient [client]
                            where [client].Name = 'Liakos_1'";
                OOAdvantech.Collections.StructureSet structureSet = objectStorage.Execute(query);
                foreach (OOAdvantech.Collections.StructureSet rowm in structureSet)
                {
                    int tter = (int)rowm["Count"];
                }

                //stateTransition.Consistent = true;
            }
        }

        public void QueryOnOneToManyMixMapping()
        {
            ObjectStorage objectStorage = OpenStorage();

            //IUnitMeasure unit = objectStorage.NewObject<UnitMeasure>();
            //unit.Name = "Kilos";

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                //IOrder newOrder = objectStorage.NewObject<Order>();
                //newOrder.Name = "NiewOrder";
                //IClient newClient = objectStorage.NewObject<Client>(new Type[1]{typeof(string)},"NiewClient");
                //newOrder.Client = newClient;

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

                var results = from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                              from ordDetail in order.OrderDetails
                              //where ordDetail.Price.Product.MinimumQuantity.UnitMeasure == unit
                              select new { order, client = order.Client };

                
                
                foreach (var item in results)
                {

                }
                //stateTransition.Consistent = true;
            }

        }



        public void SaveRecursiveObjects()
        {
            try
            {
                ObjectStorage storageSession = OpenStorage();
                //ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                AbstractionsAndPersistency.ICategory category = null;
                AbstractionsAndPersistency.ICategory subCategory = null;
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    category = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    category.Root = true;
                    category.Name = "Root";
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub1A";
                    category.AddSubCategory(subCategory);
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub1B";
                    category.AddSubCategory(subCategory);
                    category = subCategory;
                    stateTransition.Consistent = true;
                }

                ObjectStorage metaDataObjectStorage = ObjectStorage.GetStorageOfObject(storageSession.StorageMetaData);
                OOAdvantech.Linq.Storage linqStorage = new OOAdvantech.Linq.Storage(metaDataObjectStorage);

                var classes = from _class in linqStorage.GetObjectCollection<OOAdvantech.MetaDataRepository.Class>()
                              where _class.Name == "Category"
                              select _class;
                foreach (OOAdvantech.RDBMSMetaDataRepository.Class _class in classes)
                {
                    foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCell storageCell in _class.StorageCells)
                    {
                        if (storageCell.MainTable.Name == "T_Category")
                        {

                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                _class.ActiveStorageCell = storageCell;
                                stateTransition.Consistent = true;
                            }
                            break;
                        }
                    }
                    break;
                }

                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub2A";
                    category.AddSubCategory(subCategory);
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub2B";
                    category.AddSubCategory(subCategory);
                    category = subCategory;
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub3A";
                    category.AddSubCategory(subCategory);
                    subCategory = storageSession.NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.ICategory;
                    subCategory.Name = "Sub3B";
                    category.AddSubCategory(subCategory);
                    stateTransition.Consistent = true;
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
            public IEnumerable<CategoryData> SubCategories { get; set; }
        }

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
                //                string objectQuery = @"#OQL: SELECT category, category.Recursive(SubCategories,3) subCategories
                //                                    FROM AbstractionsAndPersistency.ICategory category 
                //                                    WHERE category.Name='Root'#";
                //                StructureSet objectSet = storageSession.Execute(objectQuery);
                //                foreach (StructureSet objectSetInstance in objectSet)
                //                {
                //                    StructureSet tt = objectSetInstance["SubCategories"] as StructureSet;
                //                    object adsd = objectSetInstance["category"];

                //                    foreach (StructureSet intt in tt)
                //                    {
                //                        object mm = intt["Object"];
                //                        string name = (intt["Object"] as AbstractionsAndPersistency.ICategory).Name;
                //                        StructureSet ttb = intt["SubCategories"] as StructureSet;
                //                        foreach (StructureSet inttb in ttb)
                //                        {
                //                            object mmb = inttb["Object"];
                //                            string nameb = (inttb["Object"] as AbstractionsAndPersistency.ICategory).Name;

                //                            StructureSet ttbc = inttb["SubCategories"] as StructureSet;
                //                            foreach (StructureSet inttbc in ttbc)
                //                            {
                //                                object mmbc = inttbc["Object"];
                //                                string namebc = (inttbc["Object"] as AbstractionsAndPersistency.ICategory).Name;
                //                            }
                //                        }
                //                    }
                //                }




            }
            catch (System.Exception error)
            {


            }

        }



        public void DeleteIndexedRelatedObject()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                AbstractionsAndPersistency.IOrder order = null;
                AbstractionsAndPersistency.IOrderDetail orderDetail = null;
                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {


                    order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Loasas";

                    orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "Sprite";
                    order.AddItem(orderDetail);
                    orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "CocaCola";
                    order.AddItem(orderDetail);
                    sysStateTransition.Consistent = true;
                }

                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "PepsiCola";
                    order.AddItem(0, orderDetail);

                    stateTransition.Consistent = true;
                }

                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    order.RemoveItem(orderDetail);

                    stateTransition.Consistent = true;
                }



            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }

        public void IndexChangeTest()
        {
            try
            {
                //ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                ObjectStorage openStorage = OpenStorage();
                AbstractionsAndPersistency.IOrder order = null;
               var orderff = (from mOrder in new OOAdvantech.Linq.Storage(openStorage).GetObjectCollection<IOrderDetail>()
                        // where mOrder.Name=="Loasas"
                         select mOrder).ToList()[0];
                //using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                //{
                //    order = openStorage.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                //    order.Name = "Loasas";
                //    AbstractionsAndPersistency.IOrderDetail orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                //    orderDetail.Name = "Sprite";
                //    order.AddItem(orderDetail);
                //    orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                //    orderDetail.Name = "CocaCola";
                //    order.AddItem(orderDetail);
                //    sysStateTransition.Consistent = true;
                //}

                //using (SystemStateTransition stateTransition = new SystemStateTransition())
                //{
                //    AbstractionsAndPersistency.IOrderDetail orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                //    orderDetail.Name = "PepsiCola";
                //    order.AddItem(0, orderDetail);
                //    stateTransition.Consistent = true;
                //}
            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }


    }
}
