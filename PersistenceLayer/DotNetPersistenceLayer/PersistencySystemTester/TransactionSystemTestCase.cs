using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
using AbstractionsAndPersistency;
using System.Linq;

namespace PersistencySystemTester
{
    /// <MetaDataID>{95c22277-7f33-40d5-abd1-55a90245fee9}</MetaDataID>
    [Transactional]
    public class TransactionalClass : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        OOAdvantech.Collections.Generic.Set<string> TestStringCollection = new OOAdvantech.Collections.Generic.Set<string>();

        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                    {
                        _Name = value;
                        scope.Complete();
                    }
                    stateTransition.Consistent = true;
                }
            }
        }




        public void TestTransaction()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {

                try
                {

                    using (OOAdvantech.Transactions.ObjectStateTransition nestedStateTransition = new ObjectStateTransition(this, TransactionOption.RequiredNested))
                    {
                        _Name = "Kitsos";
                        TestStringCollection.Add(_Name);
                        using (OOAdvantech.Transactions.ObjectStateTransition sNestedStateTransition = new ObjectStateTransition(this, TransactionOption.RequiredNested))
                        {
                            _Name = "Liakos";
                            TestStringCollection.Add(_Name);
                            sNestedStateTransition.Consistent = true;
                        }

                        nestedStateTransition.Consistent = true;
                    }
                }
                catch (System.Exception error)
                {

                }
                using (OOAdvantech.Transactions.ObjectStateTransition mnestedStateTransition = new ObjectStateTransition(this))
                {
                    _Name = "value";
                    mnestedStateTransition.Consistent = true;
                }

                stateTransition.Consistent = true;
            }
        }
        public void TestNestedTransaction()
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Name = "value";
                stateTransition.Consistent = true;
            }

        }
    }
    /// <MetaDataID>{0fa20a1b-5eee-41fb-9f4a-44237ac010ca}</MetaDataID>
    public class TransactionSystemTestCase
    {
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

        public void SetSystemStateForQueryTest()
        {
            ObjectStorage objectStorage = OpenStorage();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                StorePlace storePlace = objectStorage.NewObject<StorePlace>(new Type[1] { typeof(string) }, "WareHousePlaceA1");
                Product product = objectStorage.NewObject<Product>(new Type[1] { typeof(string) }, "ToshibaA40");
                storePlace.AddProduct(product);
                product = objectStorage.NewObject<Product>(new Type[1] { typeof(string) }, "HP2410");
                storePlace.AddProduct(product);
                product = objectStorage.NewObject<Product>(new Type[1] { typeof(string) }, "WesternDigital500S3");
                storePlace.AddProduct(product);
                stateTransition.Consistent = true;
            }



        }

        public void TestObjectAttributeChangeUnderTransaction()
        {

            ObjectStorage objectStorage = OpenStorage();


            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var storedProducts = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                 from product in storePlace.StoredProducts
                                 where storePlace.Name == "WareHousePlaceA1"
                                 select new { storePlace, product };
            IStorePlace theStorePlace = null;
            IProduct theProduct = null;
            foreach (var storedProduct in storedProducts)
            {
                theStorePlace = storedProduct.storePlace;
                theProduct = storedProduct.product;
                break;
            }



            //Product newProduct = new Product("Pepsi cola");
            //storageSession.CommitTransientObjectState(newProduct);
            //theStorePlace.AddProduct(newProduct);
            CommittableTransaction transactionA = new CommittableTransaction();
            CommittableTransaction transactionB = new CommittableTransaction();


            using (SystemStateTransition stateTransition = new SystemStateTransition(transactionA))
            {

                theProduct.Name = "Pepsi cola";
                stateTransition.Consistent = true;
            }
            using (SystemStateTransition stateTransition = new SystemStateTransition(transactionB))
            {
                string tmp = theProduct.Name;
                theProduct.Name = "Pepsi cola B";
                stateTransition.Consistent = true;
            }

            //Μεσα στην Transaction η συνθήκη ικανοποιείται

            using (SystemStateTransition stateTransition = new SystemStateTransition(transactionA))
            {
                var storedProductsUnderTransaction = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                                     from product in storePlace.StoredProducts
                                                     where product.Name == "Pepsi cola"
                                                     select new { storePlace, product };
                foreach (var storedProduct in storedProductsUnderTransaction)
                {
                }
                stateTransition.Consistent = true;
            }
            using (SystemStateTransition stateTransition = new SystemStateTransition(transactionB))
            {
                var storedProductsUnderTransaction = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                                     from product in storePlace.StoredProducts
                                                     where product.Name == "Pepsi cola B"
                                                     select new { storePlace, product };
                foreach (var storedProduct in storedProductsUnderTransaction)
                {
                }
                stateTransition.Consistent = true;
            }

            //Εκτώς Transaction το product έχει την παλιά τιμή και η συνθήκη δεν ικανοποιείται


            using (SystemStateTransition supressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                var storedProductsUnderTransaction = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                                     from product in storePlace.StoredProducts
                                                     where product.Name == "Pepsi cola B" || product.Name == "Pepsi cola"
                                                     select new { storePlace, product };

                foreach (var storedProduct in storedProductsUnderTransaction)
                {

                }
            }
        }


        public void TestObjectPartialEnlistmen()
        {
             
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OpenStorage());

            var mOrderDetail = (from order in storage.GetObjectCollection<OrderDetail>()
                                   select order).ToList()[2];
            var unitMeasure = (from unit in storage.GetObjectCollection<IUnitMeasure>()
                        select unit).ToList()[0];

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            { 
                Quantity quantity = mOrderDetail.Quantity;
                quantity.Amount++;
                mOrderDetail.Quantity = quantity;
                Quantity sQuantity = new Quantity(21, unitMeasure);
                mOrderDetail.SQuantity = new SupperQuantity("sName", sQuantity);
                //mOrderDetail = (from order in storage.GetObjectCollection<OrderDetail>()
                //                    where order.SQuantity.Quantity.UnitMeasure.Name == "Kilos"
                //                    select order).ToList()[0];
                
                stateTransition.Consistent = true;
            }
        }

        public void TestOneToManyRelationRemoveUnderTransaction()
        {
            ObjectStorage objectStorage = OpenStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var orderDetailsData = from order in storage.GetObjectCollection<IOrder>()
                                   from orderDetail in order.OrderDetails
                                   select new { order, orderDetail };
            IOrder theOrder = null;
            IOrderDetail theOrderDetail = null;
            foreach (var orderDetailData in orderDetailsData)
            {
                theOrder = orderDetailData.order;
                theOrderDetail = orderDetailData.orderDetail;
                break;
            }

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                theOrderDetail.Name = "Pepsi Cola";
                theOrderDetail.Order = null;
                var result = from order in storage.GetObjectCollection<IOrder>()
                             from orderDetail in order.OrderDetails
                             where orderDetail.Name == "Pepsi Cola"
                             select new { order, orderDetail };
                foreach (var orderDetailData in result)
                {

                }

            }

        }

        public void TestAssociationClassRelationRemoveUnderTransaction()
        {
            ObjectStorage objectStorage = OpenStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var result = from productPrice in storage.GetObjectCollection<IProductPrice>()
                         select productPrice;

            var prodresult = from product in storage.GetObjectCollection<IProduct>()
                             select new {  product.Quantity, product.Quantity.Amount };

            foreach (var prod in prodresult)
            {

            }
                

            IProductPrice theProductPrice = null;
       
            foreach (var productPrice in result)
            {
                theProductPrice = productPrice;
                break;
            }
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                //Product newProduct = objectStorage.NewObject<Product>(new Type[1] { typeof(string) }, "ToshibaA40");
                //ProductPrice newProductPrice = objectStorage.NewObject<ProductPrice>(new Type[1] { typeof(string) }, "ToshibaA40");
                //newProductPrice.PriceList = thePriceList;
                //newProductPrice.Product = newProduct;
                //theOrderDetail.Price = newProductPrice;

                theProductPrice.Product.Name = "ToshibaA40";
                ObjectStorage.DeleteObject(theProductPrice, DeleteOptions.TryToDelete);

                var producPricesData = from priceList in storage.GetObjectCollection<IPriceList>()
                                       from productPrice in priceList.Products
                                       where productPrice.Product.Name == "ToshibaA40"
                                       select new { priceList, productPrice, product = productPrice.Product };
                foreach (var productPrice in producPricesData)
                {
                }
            }
        }
        public void TestAssociationClassRelationAddUnderTransaction()
        {
            ObjectStorage objectStorage = OpenStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var productPrices = from productPrice in storage.GetObjectCollection<IProductPrice>()
                                select productPrice;

            foreach (var prodPrie in productPrices)
            {

            }
            var result = from priceList in storage.GetObjectCollection<IPriceList>()
                         select priceList;
            var orderDetailResults = from orderDetail in storage.GetObjectCollection<IOrderDetail>()
                                     select orderDetail;


            IPriceList thePriceList=null;
            IOrderDetail theOrderDetail = null;
            foreach (var orderDetail in orderDetailResults)
            {
                theOrderDetail = orderDetail;
                break;
            }
            foreach (var priceList in result)
            {
                thePriceList = priceList;
                break;
            }
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                Product newProduct = objectStorage.NewObject<Product>(new Type[1] { typeof(string) }, "ToshibaA41");
                ProductPrice newProductPrice = objectStorage.NewObject<ProductPrice>(new Type[1] { typeof(string) }, "ToshibaA41");
                newProductPrice.PriceList = thePriceList;
                newProductPrice.Product = newProduct;
                theOrderDetail.Price = newProductPrice;
                var producPricesData = from priceList in storage.GetObjectCollection<IPriceList>()
                                       from productPrice in priceList.Products
                                       where productPrice.Product.Name == "ToshibaA41"
                                       select new { priceList, productPrice, product = productPrice.Product };
                foreach (var productPrice in producPricesData)
                {
                }
            }
        }
        class OrderDetailData
        {
            public IOrderDetail OrderDetail
            {
                get;
                set;
            }
            public IOrder Order
            {
                get;
                set;
            }

        }
        public void QueryOnEmptyStorageOnTransaction()
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage=OpenStorage();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                IOrder order = objectStorage.NewObject<Order>();
                IOrderDetail orderDetail = objectStorage.NewObject<OrderDetail>();
                order.AddItem(orderDetail);
                OOAdvantech.Linq.Storage storage=new OOAdvantech.Linq.Storage(objectStorage);

                //var orderDetailsData = from theOrderDetail in storage.GetObjectCollection<IOrderDetail>()
                //                       where theOrderDetail.Order == order
                //                       select new OrderDetailData{OrderDetail= theOrderDetail,Order= theOrderDetail.Order };

                //int count = orderDetailsData.ToList<OrderDetailData>().Count;

            }
        
        }
        public void TestOneToManyRelationAddUnderTransaction()
        {

            ObjectStorage objectStorage = OpenStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var orderDetailsData = from order in storage.GetObjectCollection<IOrder>()
                                   from orderDetail in order.OrderDetails
                                   select new { order, orderDetail };
            IOrder theOrder = null;
            IOrderDetail theOrderDetail = null;
            foreach (var orderDetailData in orderDetailsData)
            {
                theOrder = orderDetailData.order;
                theOrderDetail = orderDetailData.orderDetail;
                break;
            }
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{
            //    IOrderDetail newOrderDetail = objectStorage.NewObject<OrderDetail>();
            //    newOrderDetail.Name = "Pepsi Cola";
            //    newOrderDetail.Order = theOrder;

            //    var result = from order in storage.GetObjectCollection<IOrder>()
            //                           from orderDetail in order.OrderDetails
            //                           where orderDetail .Name=="Pepsi Cola"
            //                 select new { order, orderDetail, order.Name };
            //     foreach (var orderDetailData in result)
            //     {
            //     }
            //}

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                IOrderDetail newOrderDetail = objectStorage.NewObject<OrderDetail>();
                newOrderDetail.Name = "Pepsi Cola";
                newOrderDetail.Order = theOrder;

                var result = from orderDetail in storage.GetObjectCollection<IOrderDetail>()
                             //from orderDetail in order.OrderDetails
                             where orderDetail.Name == "Pepsi Cola"
                             select new { orderDetail.Order, orderDetail, orderDetail.Order.Name };
                foreach (var orderDetailData in result)
                {

                }

            }
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{
            //    IOrderDetail newOrderDetail = objectStorage.NewObject<OrderDetail>();
            //    IOrder newOrder = objectStorage.NewObject<Order>();
            //    newOrderDetail.Name = "Pepsi Cola";
            //    newOrderDetail.Order = theOrder;
            //    theOrder.Name = "A4022";
            //    var result = from order in storage.GetObjectCollection<IOrder>()
            //                 from orderDetail in order.OrderDetails
            //                 where orderDetail.Name == "Pepsi Cola"
            //                 select new { order, orderDetail };
            //    foreach (var orderDetailData in result)
            //    {

            //    }

            //}
        }


        /// <summary>
        /// Create new storagePlace and add product with product Name  change under transaction
        /// </summary>
        public void TestManyToManyRelationAddUnderTransaction()
        {

            ObjectStorage objectStorage = OpenStorage();


            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var storedProducts = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                 from product in storePlace.StoredProducts
                                // where storePlace.Name == "WareHousePlaceA1"
                                 select new { storePlace, product };
            IStorePlace theStorePlace = null;
            IProduct theProduct = null;
            foreach (var storedProduct in storedProducts)
            {
                theStorePlace = storedProduct.storePlace;
                theProduct = storedProduct.product;
               // break;
            }
            return;


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                StorePlace newStorePlace =objectStorage.NewObject<StorePlace>(new Type[1]{typeof( string)},"WareHousePlaceB1");

                objectStorage.CommitTransientObjectState(newStorePlace);
                //theStorePlace.AddProduct(newProduct);
                theProduct.Name = "Pepsi cola";
                newStorePlace.AddProduct(theProduct);

                var storedProductsUnderTransaction = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                                     from product in storePlace.StoredProducts
                                                     where product.Name == "Pepsi cola" && storePlace.Name == "WareHousePlaceB1"
                                                     select new { storePlace, product };
                //Δεν πρέπει να υπάρχει storedProduct που να ικανοποιή την συνθήκη γιατι η σχέση 
                //έχει διαγραφεί εντός τις transaction
                foreach (var storedProduct in storedProductsUnderTransaction)
                {
                    string name = storedProduct.product.Name;
                }
            }

        }

        public void TestManyToManyRelationRemoveUnderTransaction()
        {

            ObjectStorage objectStorage = OpenStorage();


            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var storedProducts = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                 from product in storePlace.StoredProducts
                                 where storePlace.Name == "WareHousePlaceA1"
                                 select new { storePlace, product };
            IStorePlace theStorePlace = null;
            IProduct theProduct = null;
            foreach (var storedProduct in storedProducts)
            {
                theStorePlace = storedProduct.storePlace;
                theProduct = storedProduct.product;
                break;
            }


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                //Product newProduct = new Product("Pepsi cola");
                //storageSession.CommitTransientObjectState(newProduct);
                //theStorePlace.AddProduct(newProduct);
                theProduct.Name = "Pepsi cola";
                theStorePlace.RemoveProduct(theProduct);

                var storedProductsUnderTransaction = from storePlace in storage.GetObjectCollection<IStorePlace>()
                                                     from product in storePlace.StoredProducts
                                                     where product.Name == "Pepsi cola"
                                                     select new { storePlace, product };
                //Δεν πρέπει να υπάρχει storedProduct που να ικανοποιή την συνθήκη γιατι η σχέση 
                //έχει διαγραφεί εντός τις transaction
                foreach (var storedProduct in storedProductsUnderTransaction)
                {
                }
            }

        }


        public void NestTransactionDistribution()
        {
            System.DateTime startDate = System.DateTime.Now;
            try
            {
                TransactionalClass transactionalClass = new TransactionalClass();
                transactionalClass.TestTransaction();


            }
            catch (System.Exception error)
            {


            }

            System.TimeSpan span = System.DateTime.Now - startDate;
            System.Diagnostics.Debug.WriteLine(span);


        }
        public void LocalTransactionCommit()
        {
            TransactionalClass transactionalClass = new TransactionalClass();
            try
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    transactionalClass.Name = "Kitsos";
                    stateTransition.Consistent = true;
                }
            }
            catch (System.Exception error)
            {
                int e = 0;
            }
        }

        public void LocalTransactionAbort()
        {

        }

    }
}
