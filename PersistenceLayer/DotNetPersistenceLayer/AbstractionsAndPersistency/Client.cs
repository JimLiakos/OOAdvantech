namespace AbstractionsAndPersistency
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    using OOAdvantech.Collections.Generic;
    using OOAdvantech.PersistenceLayer;
    using OOAdvantech.Collections;
    using System.Linq;
    using System.Linq.Expressions;
    using System;
    using System.Reflection;
    using OOAdvantech.Remoting;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{2FA1B83F-36E4-499A-A73B-C8DDB3E4D624}</MetaDataID>
    [BackwardCompatibilityID("{2FA1B83F-36E4-499A-A73B-C8DDB3E4D624}")]
    [Persistent()]
    public class Client :MarshalByRefObject, IClient,IExtMarshalByRefObject
    {

        /// <MetaDataID>{7b624034-8599-4492-a5f3-5ceb9693b31b}</MetaDataID>
        public string GetUserName()
        {
#if !DeviceDotNet
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
#else
            return "";
#endif
        }

        


        /// <MetaDataID>{f1c9221c-5919-4406-a0a9-06400d886f5f}</MetaDataID>
        int mara;

        /// <MetaDataID>{ccce85fa-58ea-43cf-adcf-d939bccb819b}</MetaDataID>
        public Set<IPriceList> PriceLists
        {
            get
            {
                if (false)
                    throw new System.Exception("Test Exception");
                Set<IPriceList> priceListCollection = new Set<IPriceList>();
                ObjectStorage objectStorage = null;
                objectStorage = ObjectStorage.GetStorageOfObject(this);

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
        }
        /// <MetaDataID>{ef88f009-0878-4eec-ad05-51ca3190c7db}</MetaDataID>
        public Set<IPriceList> GetPriceLists()
        {
            return PriceLists;
        }
        /// <MetaDataID>{68a3e0b1-d671-46a4-8cea-08e52868ef38}</MetaDataID>
        public OOAdvantech.PersistenceLayer.StorageInstanceRef StorageInstanceRef;
        /// <MetaDataID>{36CBACE0-A030-4C3D-9518-3FC368F5362C}</MetaDataID>
        protected Client()
        {
            _Name = "tmp";

        }
        /// <summary/>
        /// <MetaDataID>{6E2FAD1C-A54E-43E0-BDEA-82A5B42C1FEB}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink Properties = new OOAdvantech.ObjectStateManagerLink();

        /// <MetaDataID>{c58840f6-d492-4e02-8f2c-a969a53af1d1}</MetaDataID>
        ~Client()
        {

        }
        /// <MetaDataID>{04FE71D2-6E3F-4AF3-9EB6-BD1432A8329B}</MetaDataID>
        public Client(string name)
        {
            _Name = name;

            //Properties = new OOAdvantech.ExtensionProperties();
            ////object memoryInstance, MetaDataStorageSession activeStorageSession, object objectID
            //this.StorageInstanceRef = ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.MetaDataLoadingSystem.MetaDataStorageInstanceRef","") as OOAdvantech.PersistenceLayer.StorageInstanceRef;
            //Properties.SetProperty(typeof(OOAdvantech.PersistenceLayer.StorageInstanceRef).FullName, StorageInstanceRef);

        }

        /// <exclude>Excluded</exclude>
        private OOAdvantech.Collections.Generic.Set<IOrder> _Orders = new Set<IOrder>();
        /// <MetaDataID>{296E4268-6EF2-4D6E-B401-1BB3274279C1}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_Orders")]
        [TransactionalMember(LockOptions.Shared, "_Orders")]
        //[AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
        public virtual OOAdvantech.Collections.Generic.Set<IOrder> Orders
        {
            get
            {
                int tt = _Orders.Count;
                if (tt == 0 && TheOrder != null)
                    _Orders.Add(TheOrder);

                return new Set<IOrder>(_Orders, CollectionAccessType.ReadOnly);
            }

        }

#if !DeviceDotNet
        /// <MetaDataID>{e863a5fc-baca-4f7b-95c9-d6f32ef7f03d}</MetaDataID>
        IQueryable<T> GetClients<T>()
        {
            ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(this);
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            IQueryable queryable = storage.GetObjectCollection(typeof(IClient));
            PropertyInfo collectionMember = queryable.ElementType.GetProperty("Orders");
            Type collectionItemType = typeof(IOrder);// collectionMember.PropertyType.GetElementType();

            ParameterExpression collectionOwner = Expression.Parameter(queryable.ElementType, "collectionOwner");
            ParameterExpression collectionItem = Expression.Parameter(typeof(IOrder), "collectionItem");
            MemberExpression memberAccess = Expression.MakeMemberAccess(collectionOwner, collectionMember);
            // ***** Where(client => (client == this )) *****
            Expression left = collectionOwner;
            Expression right = Expression.Constant(this);
            Expression equal = Expression.Equal(left, right);
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryable.ElementType },
                queryable.Expression,
                Expression.Lambda(typeof(Func<,>).MakeGenericType(new Type[] { queryable.ElementType, typeof(bool) }), equal, new ParameterExpression[] { collectionOwner }));
            MethodInfo createQueryMethod = null;

            MethodCallExpression selectCallExpression = Expression.Call(
                typeof(Queryable),
                "SelectMany",
                new Type[] { queryable.ElementType, collectionItemType, collectionItemType }, whereCallExpression,
                Expression.Lambda(typeof(Func<,>).MakeGenericType(new Type[] {queryable.ElementType,typeof(System.Collections.Generic.IEnumerable<>).MakeGenericType(new Type[]{collectionItemType})}), memberAccess, new ParameterExpression[] { collectionOwner }),
                Expression.Lambda(typeof(Func<,,>).MakeGenericType(new Type[] { queryable.ElementType, collectionItemType,collectionItemType }) , collectionItem, new ParameterExpression[] { collectionOwner,collectionItem }));
            foreach (MethodInfo methodInfo in queryable.Provider.GetType().GetMethods())
            {
                if (methodInfo.Name == "CreateQuery" && methodInfo.ReturnType.Name == typeof(System.Linq.IQueryable<>).Name)
                {
                    createQueryMethod = methodInfo;
                    break;
                }
            }
            return (IQueryable<T>)createQueryMethod.MakeGenericMethod(collectionItemType).Invoke(queryable.Provider, new object[1] { selectCallExpression });

        }
#endif


        /// <MetaDataID>{32f42824-9b66-474d-9d58-0fec8d1eec3a}</MetaDataID>
        public System.Collections.Generic.List<IOrder> GetOrders(System.DateTime fromDate, System.DateTime toDate)
        {
            //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

//var orders = from client in storage.GetObjectCollection<IClient>()
//             from item in client.Orders
//             where client == this
//             select item;

//foreach (var marma in from order in orders
//                      where order.OrderDate > fromDate && order.OrderDate < toDate
//                      select order)
//{

//}


#if !DeviceDotNet
            foreach (var result in from order in  _Orders.AsObjectContextQueryable
                                   where order.OrderDate >= fromDate && order.OrderDate <= toDate
                                   select order)
            {


            }
#endif

            return new List<IOrder>();
            // ***** End Where *****

            // ***** OrderBy(company => company) *****
            // Create an expression tree that represents the expression
            // 'whereCallExpression.OrderBy(company => company)'
            //MethodCallExpression orderByCallExpression = Expression.Call(
            //    typeof(Queryable),
            //    "OrderBy",
            //    new Type[] { queryableData.ElementType, queryableData.ElementType },
            //    whereCallExpression,
            //    Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));
            //// ***** End OrderBy *****

            //// Create an executable query from the expression tree.
            //IQueryable<string> results = queryableData.Provider.CreateQuery<string>(orderByCallExpression);

            //// Enumerate the results.
            //foreach (string company in results)
            //    Console.WriteLine(company);


          //  _Orders.Where()

            return new List<IOrder>();
            //(from order in _Orders
                   // where order.OrderDate > fromDate && order.OrderDate < toDate
              //      select order).ToList<IOrder>();

        }

        /// <MetaDataID>{afcea2bb-fb91-445b-8209-d257fce87543}</MetaDataID>
        public void AddOrder(IOrder order, int index)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, "Orders"))
            {
                if (index != -1)
                    _Orders.Insert(index, order);
                else
                    _Orders.Add(order);

                stateTransition.Consistent = true;
            }




        }
        /// <MetaDataID>{eee0fd3c-47a6-4b2e-acea-33efe8d9cda8}</MetaDataID>
        public void RemoveOrder(IOrder order)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, "Orders"))
            {
                _Orders.Remove(order);
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{e9a90eb3-685d-4abf-ba12-dcc0152580c2}</MetaDataID>
        private int Remain;

        /// <MetaDataID>{f29b3365-2a85-4d03-a601-9afccbe52061}</MetaDataID>
        private IClient SubClient;


        public static IOrder TheOrder;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{16630A1B-442C-43C0-8E26-0E953BB512D5}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{042BA329-CC2E-42BC-8349-679AF507DA4E}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember(255, "_Name")]
        [TransactionalMember(LockOptions.Exclusive, "_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                //if(_Name!=value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                    {
                        try
                        {

                            _Name = value;
                            OOAdvantech.EventUnderProtection.Invoke<OOAdvantech.ObjectChangeStateHandle>(ref ClientChanged, OOAdvantech.EventUnderProtection.ExceptionHandling.IgnoreExceptions | OOAdvantech.EventUnderProtection.ExceptionHandling.RemoveDisconnectedEventHandlers, this, "Name");
                        }
                        catch (System.Exception error)
                        {

                        }
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{d0bc0d71-af24-444f-88f7-6429b5b268fb}</MetaDataID>
        public void RaiseEvent()
        {
            ClientChanged(this, "Name");
        }

        public event OOAdvantech.ObjectChangeStateHandle ClientChanged;


      
    }
}
