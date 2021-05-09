using System;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Data;
using OOAdvantech.Collections.Generic;
using OOAdvantech.Collections;
using System.Linq;
using OOAdvantech.Linq;


namespace PersistencySystemTester
{
    /// <summary></summary>
    /// <MetaDataID>{4ab85a92-e1fe-45d3-9ce4-f4b663a8dd78}</MetaDataID>
    public class AssociationClassTest:MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        private static void PrintRowValues(DataRow[] rows, string label)
        {

            System.Diagnostics.Debug.WriteLine("\n{0}", label);
            if (rows.Length <= 0)
            {
                System.Diagnostics.Debug.WriteLine("no rows found");
                return;
            }
            foreach (DataRow row in rows)
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    System.Diagnostics.Debug.Write("\table {0}", row[column].ToString());
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }
        /// <summary>
        /// Run time referential integrity test
        /// </summary>
        public void ReferentialIntegityCountTest()
        {
            ObjectStorage objectStorage = OpenStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            List<AbstractionsAndPersistency.IClient> clients = new List<AbstractionsAndPersistency.IClient>(from theClient in storage.GetObjectCollection<AbstractionsAndPersistency.IClient>()
                                                                                                            select theClient);
            AbstractionsAndPersistency.IClient client = null;
            if (clients.Count > 0)
                client = clients[0];
            else
            {
                client = new AbstractionsAndPersistency.Client("Kitsos");
                objectStorage.CommitTransientObjectState(client);
            }

            CommittableTransaction transactionA = new CommittableTransaction();
            CommittableTransaction transactionB = new CommittableTransaction();
            using (SystemStateTransition stateTransition = new SystemStateTransition(transactionA))
            {
                AbstractionsAndPersistency.Order orderA = new AbstractionsAndPersistency.Order();
                objectStorage.CommitTransientObjectState(orderA);
                client.AddOrder(orderA, -1);
                int refCount = (OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(client) as OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef).RuntimeReferentialIntegrityCount;
                int pRefCount = (OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(client) as OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef).ReferentialIntegrityCount;

                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    using (SystemStateTransition innenrStateTransition = new SystemStateTransition(transactionB))
                    {
                        AbstractionsAndPersistency.Order order = new AbstractionsAndPersistency.Order();
                        objectStorage.CommitTransientObjectState(order);
                        client.AddOrder(order, -1);
                        int innerRefCount = (OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(client) as OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef).RuntimeReferentialIntegrityCount;
                        int innerPRefCount = (OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(client) as OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef).ReferentialIntegrityCount;
                        innenrStateTransition.Consistent = true;
                    }
                    stateTransition.Consistent = true;
                }
                transactionB.Commit();

                refCount = (OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(client) as OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef).RuntimeReferentialIntegrityCount;
                pRefCount = (OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(client) as OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef).ReferentialIntegrityCount;

                stateTransition.Consistent = true;
            }
            transactionA.Commit();
        }
        public void InterStorageInstanceObjectsUnLink()
        {
            try
            {




           

                ObjectStorage storageSession = null;
                #region Open Instance A storage
                storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                #endregion

                ObjectStorage storageSessionB = null;
                #region Open Instance B storage
                storageSessionB = ObjectStorage.OpenStorage("AbstractionsB",
                    @"localhost\Family",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                #endregion


                string oql = "#OQL: SELECT order.Name ,order.Client.Name ClientName,order.OrderDetails OrderDetailName " +
                    "FROM AbstractionsAndPersistency.IOrder order #";

                //oql = "#OQL: SELECT order.Name ,order.Client.Name ClientName " +
                //    "FROM AbstractionsAndPersistency.IOrder order #";
                System.TimeSpan NewTimeSpan;
                System.TimeSpan ComiteTimeSpan;
                System.TimeSpan ComiteTimeSpanB;
                System.DateTime before;
                System.DateTime After;
                before = System.DateTime.Now;

                StructureSet objectSet = storageSession.Execute(oql);
                After = System.DateTime.Now;
                ComiteTimeSpan = After - before;
                System.Diagnostics.Debug.WriteLine("1 " + ComiteTimeSpan.ToString());
                before = System.DateTime.Now;

                int orddet = 0;
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    object obj1 = objectSet["Name"];
                    object obj2 = objectSet["ClientName"];
                    StructureSet obj3 = objectSet["OrderDetails"] as StructureSet;
                    foreach (StructureSet orderDetailsInstance in obj3)
                    {
                        object obj4 = orderDetailsInstance["OrderDetailName"];
                        AbstractionsAndPersistency.IOrderDetail orderDetail = obj4 as AbstractionsAndPersistency.IOrderDetail;
                        string namef = null;
                        //if (orderDetail != null)
                        //    namef = orderDetail.Name;
                        orddet++;

                        int erte = 0;

                    }


                }
                After = System.DateTime.Now;
                ComiteTimeSpanB = After - before;
                System.Diagnostics.Debug.WriteLine("2 " + ComiteTimeSpanB.ToString());

                int ertae = 0;

                //foreach (StructureSet objectSetInstance in objectSet)
                //{
                //    AbstractionsAndPersistency.IOrder order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;
                //    string name = order.Name;

                //    AbstractionsAndPersistency.OrderDetail oldOrderDetail = null;
                //    foreach (AbstractionsAndPersistency.OrderDetail orderDetail in order.OrderDetails)
                //    {
                //        name = orderDetail.Name;
                //        oldOrderDetail = orderDetail;

                //    }

                //    //if (oldOrderDetail != null)
                //    //{

                //    //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(order))
                //    //    {
                //    //        ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(oldOrderDetail);
                //    //        AbstractionsAndPersistency.OrderDetail newOrderDetail = objectStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                //    //        newOrderDetail.Name = "Γίδα";
                //    //        order.AddItem(newOrderDetail);
                //    //        stateTransition.Consistent = true;
                //    //    }
                //    //}

                //}




            }
            catch (System.Exception error)
            {


            }
        }

        /// <summary>
        /// Many to Many relation
        /// </summary>
        public void DeleteIndexedRelatedObjectFromStructure()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    //@"c:\Abstractions.xml",
                    //"OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                @"localhost\Debug",
                "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                AbstractionsAndPersistency.IProduct cocaCola = null; ;
                AbstractionsAndPersistency.IUnitMeasure secondUnit;
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    cocaCola = storageSession.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    AbstractionsAndPersistency.IUnitMeasure unit = null;
                    AbstractionsAndPersistency.Quantity quantity;
                    quantity.Amount = 12;
                    //quantity.UnitMeasures = new Set<AbstractionsAndPersistency.IUnitMeasure>();
                    cocaCola.Quantity = quantity;
                    unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    unit.Name = "metro";
                    quantity.Amount = 22;
                    //quantity.UnitMeasures = new Set<AbstractionsAndPersistency.IUnitMeasure>();
                    //quantity.UnitMeasures.Add(unit);
                    unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    unit.Name = "First";
                    //quantity.UnitMeasures.Insert(0, unit);
                    unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    unit.Name = "Second";
                    secondUnit = unit;
                    //quantity.UnitMeasures.Insert(0, unit);
                    cocaCola.MinimumQuantity = quantity;
                    stateTransition.Consistent = true;
                }
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.Quantity minQuantity = cocaCola.MinimumQuantity;
                    //minQuantity.UnitMeasures.Remove(secondUnit);
                    cocaCola.MinimumQuantity = minQuantity;
                    stateTransition.Consistent = true;
                }

            }
            catch (System.Exception Error)
            {

            }


        }

        public void TestAbstractAssosciation()
        {
            try
            { 
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"c:\Abstractions.xml",
                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    //ObjectStorage.OpenStorage("Abstractions",
                    //   @"localhost\Debug",
                    //   "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


                //using (SystemStateTransition stateTransition = new SystemStateTransition())
                //{
                //    AbstractionsAndPersistency.LiquidProduct liquidProduct = storageSession.NewObject(typeof(AbstractionsAndPersistency.LiquidProduct)) as AbstractionsAndPersistency.LiquidProduct;
                //    AbstractionsAndPersistency.LiquidStore liquidStore = storageSession.NewObject(typeof(AbstractionsAndPersistency.LiquidStore)) as AbstractionsAndPersistency.LiquidStore;
                //    liquidStore.StoredLiquidProduct = liquidProduct;

                //    stateTransition.Consistent = true;
                //}



                string query = @"select product, product.LiquidStorePlaces StorePlace
            from AbstractionsAndPersistency.LiquidProduct product where product.LiquidStorePlaces.Name='Oil'or product.LiquidStorePlaces.Name='sprite' ";

                foreach (OOAdvantech.Collections.StructureSet instanceSet in storageSession.Execute(query))
                {
                    AbstractionsAndPersistency.IProduct product = instanceSet["product"] as AbstractionsAndPersistency.IProduct;
                    int count = product.StorePlaces.Count;

                }



            }
            catch (System.Exception error)
            {


            }


        }
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

 
        public void StructureMemberTest()
        {
            try
            {
                //                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                //                    //@"c:\Abstractions.xml",
                //                    //"OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                //                @"localhost\Debug",
                //                "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


                //                //string query = @"select product,product.MinimumQuantity.UnitMeasures minUnit,product.MinimumQuantity minQuantity " +
                //                //"from AbstractionsAndPersistency.IProduct product ";
                ////                string query = @"select product,product.MinimumQuantity.UnitMeasures minUnit
                ////                                from AbstractionsAndPersistency.IProduct product ";

                //                string query = @"select product
                //                                from AbstractionsAndPersistency.IProduct product ";


                //                OOAdvantech.Collections.StructureSet structureSet = storageSession.Execute(query);

                //                foreach (OOAdvantech.Collections.StructureSet instanceSet in structureSet)
                //                {
                //                    using (SystemStateTransition stateTransition = new SystemStateTransition())
                //                    {

                //                        AbstractionsAndPersistency.IProduct product = instanceSet["product"] as AbstractionsAndPersistency.IProduct;
                //                        AbstractionsAndPersistency.Quantity minQuantity = product.MinimumQuantity;
                //                        AbstractionsAndPersistency.IUnitMeasure unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                //                        unit.Name = "Gage";
                //                        minQuantity.UnitMeasures.Add(unit);
                //                        product.MinimumQuantity = minQuantity;

                //                        stateTransition.Consistent = true;
                //                    }
                //                    break;


                //                    //object obj = instanceSet["$_minUnit"]; 

                //                    //foreach (OOAdvantech.Collections.StructureSet quanity in instanceSet["MinimumQuantity"] as OOAdvantech.Collections.StructureSet)
                //                    //{
                //                    ////    foreach (OOAdvantech.PersistenceLayer.StructureSet unitSet in quanity["UnitMeasure"] as OOAdvantech.PersistenceLayer.StructureSet)
                //                    ////    {

                //                    ////    }

                //                    //}


                //                }
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    //@"c:\Abstractions.xml",
                    //"OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                @"localhost\Debug",
                "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                AbstractionsAndPersistency.IProduct cocaCola = null; ;
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {


                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    cocaCola = storageSession.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;

                    AbstractionsAndPersistency.IUnitMeasure unit = null;
                    //AbstractionsAndPersistency.IUnitMeasure unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    //unit.Name = "kilo";
                    AbstractionsAndPersistency.Quantity quantity;
                    quantity.Amount = 12;
                    //quantity.UnitMeasures = new Set<AbstractionsAndPersistency.IUnitMeasure>();
                    ////quantity.UnitMeasures.Add(unit);
                    cocaCola.Quantity = quantity;
                    unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    unit.Name = "metro";

                    quantity.Amount = 22;
                    //quantity.UnitMeasures = new Set<AbstractionsAndPersistency.IUnitMeasure>();
                    //quantity.UnitMeasures.Add(unit);

                    //unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    //unit.Name = "metrimao";
                    //quantity.UnitMeasures.Add(unit);

                    cocaCola.MinimumQuantity = quantity;
                    stateTransition.Consistent = true;
                }
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.Quantity minQuantity = cocaCola.MinimumQuantity;
                    AbstractionsAndPersistency.IUnitMeasure unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    unit.Name = "First";
                    //minQuantity.UnitMeasures.Insert(0, unit);
                    unit = storageSession.NewObject(typeof(AbstractionsAndPersistency.UnitMeasure)) as AbstractionsAndPersistency.IUnitMeasure;
                    unit.Name = "Second";
                    //minQuantity.UnitMeasures.Insert(0, unit);

                    cocaCola.MinimumQuantity = minQuantity;

                    stateTransition.Consistent = true;
                }

            }
            catch (System.Exception Error)
            {

            }


        }

        public ObjectStorage OpenStorage()
        {
            //string storageName = "Abstractions";
            //string storageLocation = @"c:\Abstractions.sdf";
            //string storageType = "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider";
            //string storageName = "Abstractions";
            //string storageLocation = @"c:\Abstractions.xml";
            //string storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";

            string storageName = "Abstractions";
            string storageLocation = @"localhost\sqlexpress";
            string storageType = "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider";

            //string storageName = "Abstractions";
            //string storageLocation = @"localhost:1521/xe";
            //string storageType = "OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider";

            ObjectStorage storageSession = null;
            try
            {

                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
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


        //public ObjectStorage OpenStorage(object rawStorageData)
        //{

        //    ObjectStorage storageSession = null; 
        //    try
        //    {

        //        storageSession = ObjectStorage.OpenStorage("Abstractions",
        //            rawStorageData,
        //            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
        //            //@"localhost\Debug",
        //            //"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
        //    }
        //    catch (OOAdvantech.PersistenceLayer.StorageException Error)
        //    {
        //        if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
        //        {
        //            storageSession = ObjectStorage.NewStorage("Abstractions",
        //                rawStorageData,
        //                "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
        //                //@"localhost\Debug",
        //                //"OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
        //        }
        //        else
        //            throw Error;
        //        try
        //        {
        //            storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
        //        }
        //        catch (System.Exception Errore)
        //        {
        //            int sdf = 0;
        //        }
        //    }
        //    catch (System.Exception Error)
        //    {
        //        int tt = 0;
        //    }
        //    return storageSession;


        //}
        ///// <summary>
        /// Create an object link between objects where the first live 
        /// in one storage server instance and the second 
        /// live in other storage server instance.
        /// </summary>
        public void InterStorageInstanceObjectsLink()
        {


            try
            {

                ObjectStorage storageSession = null;
                #region Open Instance A storage
                try
                {
                    storageSession = ObjectStorage.OpenStorage("Abstractions",
                        @"localhost\Debug",
                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                }
                catch (OOAdvantech.PersistenceLayer.StorageException Error)
                {
                    if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                    {
                        storageSession = ObjectStorage.NewStorage("Abstractions",
                            @"localhost\Debug",
                            "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
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
                #endregion

                ObjectStorage storageSessionB = null;
                #region Open Instance B storage
                try
                {
                    storageSessionB = ObjectStorage.OpenStorage("AbstractionsB",
                        @"localhost\Family",
                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                }
                catch (OOAdvantech.PersistenceLayer.StorageException Error)
                {
                    if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                    {
                        storageSessionB = ObjectStorage.NewStorage("AbstractionsB",
                            @"localhost\Family",
                            "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                    }
                    else
                        throw Error;
                    try
                    {
                        storageSessionB.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
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
                #endregion

                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    //int k = 400;
                    //while (k >= 0)
                    //{
                    //    k--;
                    //    AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    //    order.Name = "Mitsos Order";
                    //    int i = 50;
                    //    while (i >= 0)
                    //    {
                    //        i--;
                    //        AbstractionsAndPersistency.OrderDetail orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    //        orderDetail.Name = "Γίδα"+i.ToString();
                    //        order.AddItem(orderDetail);
                    //    }
                    //}
                    //int rtyr = 0;
                    AbstractionsAndPersistency.IOrder order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Mitsos Order";
                    AbstractionsAndPersistency.IOrderDetail orderDetail = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "Γίδα";
                    order.AddItem(orderDetail);


                    //System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    //AbstractionsAndPersistency.IProduct gida = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.Product), "Γίδα", parameterTypes) as AbstractionsAndPersistency.IProduct;

                    //AbstractionsAndPersistency.IProductPrice gidaPrice = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), "Γίδα", parameterTypes) as AbstractionsAndPersistency.IProductPrice;
                    //gidaPrice.Product = gida;

                    //AbstractionsAndPersistency.IPriceList retailPriceList = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.PriceList), "Retail Price List", parameterTypes) as AbstractionsAndPersistency.IPriceList;
                    //retailPriceList.AddProduct(gidaPrice);
                    //orderDetail.Price = gidaPrice;
                    //order.AddItem(orderDetail);


                    //orderDetail = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    //orderDetail.Name = "Κότα";

                    //AbstractionsAndPersistency.IProduct Kota = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.Product), "Γίδα", parameterTypes) as AbstractionsAndPersistency.IProduct;

                    //AbstractionsAndPersistency.IProductPrice KotaPrice = storageSessionB.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), "Γίδα", parameterTypes) as AbstractionsAndPersistency.IProductPrice;
                    //KotaPrice.Product = Kota;
                    //retailPriceList.AddProduct(KotaPrice);
                    //orderDetail.Price = KotaPrice;

                    //order.AddItem(orderDetail);
                    stateTransition.Consistent = true;
                }
            }
            catch (System.Exception error)
            {

            }
        }


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
                    AbstractionsAndPersistency.IUnitMeasure unit = storageSession.NewObject<AbstractionsAndPersistency.UnitMeasure>();
                    unit.Name = "Kilos";

                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IStorePlace spriteStorePlace = storageSession.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "sprite") as AbstractionsAndPersistency.IStorePlace;
                    AbstractionsAndPersistency.IProduct sprite = storageSession.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;
                    sprite.Quantity = new AbstractionsAndPersistency.Quantity(5, unit);
                    sprite.AddStorePlace(spriteStorePlace);

                    AbstractionsAndPersistency.IProductPrice spritePrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    spritePrice.Price = AbstractionsAndPersistency.Quantity.GetQuantity(1, default(AbstractionsAndPersistency.IUnitMeasure));
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.IProduct cocaCola = storageSession.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    cocaCola.Quantity = new AbstractionsAndPersistency.Quantity(8, unit);
                    cocaCola.AddStorePlace(spriteStorePlace);

                    AbstractionsAndPersistency.IProductPrice cocaColaPrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                    cocaColaPrice.Price=AbstractionsAndPersistency.Quantity.GetQuantity(2,default(AbstractionsAndPersistency.IUnitMeasure));
                    cocaColaPrice.Product = cocaCola;

                    AbstractionsAndPersistency.IPriceList tradePriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                    tradePriceList.AddProduct(cocaColaPrice);
                    int orderCount = 5;
                    int dayOffset = 0;
                    AbstractionsAndPersistency.IClient client = storageSession.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos_" + orderCount.ToString()) as AbstractionsAndPersistency.IClient;
                    while (orderCount > 0)
                    {
                        AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                        order.OrderDate = DateTime.Now+TimeSpan.FromDays(dayOffset++);
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

        public void AddTwoAbstactionLayersObjects()
        {
            try
            {
                ObjectStorage storageSession = OpenStorage();

                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {

                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IProduct sprite = storageSession.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;

                    AbstractionsAndPersistency.IProductPrice spritePrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Loasas";

                    AbstractionsAndPersistency.OrderDetail orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "Sprite";
                    orderDetail.Price = spritePrice;
                    order.AddItem(orderDetail);

                    AbstractionsAndPersistency.IClient client = storageSession.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos") as AbstractionsAndPersistency.IClient;
                    order.Client = client;




                    AbstractionsAndPersistency.IProduct cocaCola = storageSession.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    //AbstractionsAndPersistency.IProduct cocaCola =storageSession.NewObject(typeof(AbstractionsAndPersistency.SubProduct),"coca cola",parameterTypes) as AbstractionsAndPersistency.IProduct;

                    AbstractionsAndPersistency.IProductPrice cocaColaPrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                    cocaColaPrice.Product = cocaCola;

                    AbstractionsAndPersistency.IPriceList tradePriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                    tradePriceList.AddProduct(cocaColaPrice);

                    order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Lora";

                    orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "coca cola";
                    orderDetail.Price = cocaColaPrice;
                    order.AddItem(orderDetail);
                    client = storageSession.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Jim") as AbstractionsAndPersistency.IClient;
                    order.Client = client;


                    sysStateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }
        public void RelationObjectReferencialIntegitryTest()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {

                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IProduct sprite = storageSession.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;

                    AbstractionsAndPersistency.IProductPrice spritePrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Loasas";

                    AbstractionsAndPersistency.OrderDetail orderDetail = storageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "Sprite";
                    orderDetail.Price = spritePrice;
                    order.AddItem(orderDetail);


                    sysStateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int erka = 0;
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
                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {


                    order = openStorage.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Loasas";

                    AbstractionsAndPersistency.IOrderDetail orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "Sprite";
                    order.AddItem(orderDetail);
                    orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "CocaCola";
                    order.AddItem(orderDetail);
                    sysStateTransition.Consistent = true;
                }

                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.IOrderDetail orderDetail = openStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    orderDetail.Name = "PepsiCola";
                    order.AddItem(0, orderDetail);

                    stateTransition.Consistent = true;
                }



            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }


        public void CopyObjectToOtherStorage()
        {
            ObjectStorage storageSession = null;
            try
            {
                storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                AbstractionsAndPersistency.Order copyOrder = storageSession.NewTransientObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                copyOrder = copyOrder.Copy();
                int tt = 0;

            }
            catch (System.Exception error)
            {
            }

        }
        public void AbstractionsStorageCreation()
        {
            ObjectStorage storageSession = null;
            try
            {
                storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage("Abstractions",
                        @"localhost\Debug",
                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                }
                else
                    throw Error;
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            try
            {

                storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);

            }
            catch (System.Exception Errore)
            {
                int sdf = 0;
            }
        }
        public void CommitOutOfStorageProcessTransientObject()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("AbstracClasses",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Mitras";

                    AbstractionsAndPersistency.OrderDetail orderDetail = new AbstractionsAndPersistency.OrderDetail();// storageSession.NewTransientObject(typeof(AbstractionsAndPersistency.SubOrderDetail))as AbstractionsAndPersistency.OrderDetail;
                    order.AddItem(orderDetail);
                    orderDetail.Name = "Tarko";
                    storageSession.CommitTransientObjectState(orderDetail);
                    stateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int ert = 0;
            }


        }
        public void TestLinkBetweenPersistentAndTransientObject()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("AbstracClasses",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Mitras";

                    AbstractionsAndPersistency.OrderDetail orderDetail = storageSession.NewTransientObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                    order.AddItem(orderDetail);
                    orderDetail.Name = "Tarko";
                    //storageSession.CommitTransientObjectState(orderDetail);
                    stateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int ert = 0;
            }

        }

        public void AbstracClassesStorageCreation()
        {
            ObjectStorage storageSession = null;
            try
            {
                storageSession = ObjectStorage.OpenStorage("AbstracClasses",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage("AbstracClasses",
                        @"localhost\Debug",
                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                }
                else
                    throw Error;
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            try
            {

                storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.Order).Assembly.FullName);
                //				storageSession.StorageMetaData.Build();
            }
            catch (System.Exception Errore)
            {
                int sdf = 0;
            }
        }


        public void RetrieveObjectsWithOnCustractionRelations()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                string objectQuery = "SELECT order " +
                    "FROM " + typeof(AbstractionsAndPersistency.IOrder).FullName + " order";
                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IOrder order = objectSetInstance["order"] as AbstractionsAndPersistency.IOrder;
                    Set<AbstractionsAndPersistency.IOrderDetail> orderDetails = order.OrderDetails;
                    foreach (AbstractionsAndPersistency.IOrderDetail orderDetail in orderDetails)
                    {

                        AbstractionsAndPersistency.IProductPrice productPrice = orderDetail.Price;
                        string PriceListName = productPrice.PriceList.Name;
                        string ProductName = productPrice.Product.Name;
                        int edf = 0;

                    }
                    int ttrt = 0;

                }
            }
            catch (System.Exception error)
            {
                int rtrt = 0;

            }

        }


        public void CreateAbstractClassSubTypeObject()
        {
            try
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    ObjectStorage storageSession = ObjectStorage.OpenStorage("AbstracClasses",
                        @"localhost\Debug",
                        "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                    AbstractionsAndPersistency.Order order = storageSession.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                    order.Name = "Mitras";

                    stateTransition.Consistent = true;
                }

            }
            catch (System.Exception error)
            {
                int ert = 0;
            }



        }

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

                    order.ObjectChangeState += new OOAdvantech.ObjectChangeStateHandle(order_ObjectChangeState);
                    order.Update();
                    order.ObjectChangeState -= new OOAdvantech.ObjectChangeStateHandle(order_ObjectChangeState);
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

        public void order_ObjectChangeState(object _object, string member)
        {
            System.Diagnostics.Debug.WriteLine("void order_ObjectChangeState(object _object, string member)");
        }
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
        public class CategoryData
        {
            public string CategoryName { get; set; }
            public bool Root { get; set; }
            public System.Collections.Generic.IEnumerable<CategoryData> SubCategories { get; set; }
        }
        public void RecursiveQuery()
        {
            try
            {
                ObjectStorage storageSession =  OpenStorage();

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

        public void RetrieveAbstractLinkClassObjects()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");

                //TODO Παράγει σοβαρό πρόβλημε με τα alias στο MSSQLPersistenceRunTime.ObjectQueryLanguage.DataNode
                string objectQuery = "SELECT priceList.Recursive(Products,5) productPrice " +
                "FROM "+typeof(AbstractionsAndPersistency.IPriceList).FullName+" priceList";


                //string objectQuery = "SELECT productPrice.Recursive(Product,5).Name " +
                //    "FROM " + typeof(AbstractionsAndPersistency.IProductPrice).FullName + " productPrice";

                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IProductPrice productPrice = (objectSetInstance["productPrice"] as System.Collections.ArrayList)[0] as AbstractionsAndPersistency.IProductPrice;
                    //AbstractionsAndPersistency.IProductPrice productPrice = objectSetInstance["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                    string Name = productPrice.Name;
                    AbstractionsAndPersistency.IProduct product = productPrice.Product;
                    AbstractionsAndPersistency.IPriceList priceList = productPrice.PriceList;
                    long ttt = priceList.Products.Count;
                    ttt = product.PriceLists.Count;

                    int tt = 0;
                }
            }
            catch (System.Exception Error)
            {
                int aw = 0;
            }



        }

        public void DeleteRoleBObjectofAbstractLinkClass()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


                string objectQuery = "SELECT priceList.ProductPrice productPrice " +
                    "FROM " + typeof(AbstractionsAndPersistency.IPriceList).FullName + " priceList";

                AbstractionsAndPersistency.IPriceList priceList = null;
                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    AbstractionsAndPersistency.IProductPrice productPrice = objectSetInstance["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                    string Name = productPrice.Name;
                    priceList = productPrice.PriceList;
                    int tt = 0;
                }
                if (priceList != null)
                    ObjectStorage.DeleteObject(priceList);
            }
            catch (System.Exception error)
            {
                int ee = 0;
            }

        }

        /// <summary>
        /// Delete an object where the class of object realize an association link interface 
        /// </summary>
        public void DeleteAbstractLinkClassObject()
        {
            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");


                string objectQuery = "SELECT priceList.ProductPrice productPrice " +
                    "FROM " + typeof(AbstractionsAndPersistency.IPriceList).FullName + " priceList";

                AbstractionsAndPersistency.IProductPrice productPrice = null;
                StructureSet objectSet = storageSession.Execute(objectQuery);
                foreach (StructureSet objectSetInstance in objectSet)
                {
                    productPrice = objectSetInstance["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                    string Name = productPrice.Name;
                    int tt = 0;
                }
                if (productPrice != null)
                    ObjectStorage.DeleteObject(productPrice);
            }
            catch (System.Exception error)
            {
                int ee = 0;
            }

        }

        /// <summary>
        /// Create an object where the class of object realize an association link interface 
        /// </summary>
        public void AddAbstractLinkClassObject()
        {

            try
            {
                ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {
                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IProduct sprite = storageSession.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;

                    AbstractionsAndPersistency.IProductPrice spritePrice = storageSession.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;

                    AbstractionsAndPersistency.IPriceList retailPriceList = storageSession.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    retailPriceList.AddProduct(spritePrice);
                    sysStateTransition.Consistent = true;

                }
            }
            catch (System.Exception Error)
            {
                int aw = 0;
            }
            finally
            {
            }




        }

        public void AssociationRolesFieldsTypeCheck()
        {

            try
            {
                Family.Company company = null;
                Family.Employee employee = null;

                ObjectStorage storageSession = ObjectStorage.OpenStorage("Family",
                    @"localhost\Debug",
                    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {
                    System.Type[] employeeParameterTypes = new System.Type[2] { typeof(string), typeof(double) };
                    Family.Employee jack = storageSession.NewObject(typeof(Family.Employee), employeeParameterTypes,
                        "Jack", 1500) as Family.Employee;

                    System.Type[] jobParameterTypes = new System.Type[2] { typeof(string), typeof(DateTime) };
                    Family.Job electrician = storageSession.NewObject(typeof(Family.Job), jobParameterTypes,
                        "Electrician", DateTime.Parse("10/11/2004")) as Family.Job;
                    electrician.Employee = jack;

                    System.Type[] companyParameterTypes = new System.Type[1] { typeof(string) };
                    Family.Company genecom = storageSession.NewObject(typeof(Family.Company), companyParameterTypes,
                        "Genecom") as Family.Company;
                    company = genecom;
                    company.AddEmployee(electrician);
                    sysStateTransition.Consistent = true;

                }
            }
            catch (System.Exception Error)
            {
                int aw = 0;
            }
            finally
            {
            }




        }



    }
}

