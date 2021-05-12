using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Collections.Generic;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections;
using System.Security.Principal;
using System.Threading;
using System.Linq;
using OOAdvantech.Remoting;
using System;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif
//using System.Runtime.Remoting.Channels.Tcp;




namespace AbstractionsAndPersistency
{

    /// <MetaDataID>{4BA5C2F2-62EE-4E74-B022-64B3B5F2FACA}</MetaDataID>
    [BackwardCompatibilityID("{4BA5C2F2-62EE-4E74-B022-64B3B5F2FACA}")]
    [Persistent()]
    public class Order : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IOrder
    {

        /// <MetaDataID>{0733737b-9b17-4299-aff9-2ba6bca16d56}</MetaDataID>
        public int OutArgTestMethod(int ar1, ref string arg2, double arg3, out short arg4)
        {
            arg2 = "retVal";
            arg4 = 34;
            return 5;
        }

        /// <exclude>Excluded</exclude> 
        IInspection _Inspection;
        /// <MetaDataID>{515a05a9-132a-4b8d-9904-6e898f4b2d91}</MetaDataID>
        [PersistentMember("_Inspection")]
        [BackwardCompatibilityID("+17")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IInspection Inspection
        {
            get
            {
                return _Inspection;
            }
            set
            {
                if (_Inspection != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Inspection = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }







        /// <MetaDataID>{2e134be3-f28f-4ccb-a862-e8654d41153d}</MetaDataID>
        public System.Collections.Generic.List<IProduct> Products
        {
            get
            {
                return null;
            }
        }

        /// <MetaDataID>{05e43c61-5995-4e35-af5d-66f188491f09}</MetaDataID>
        public System.Collections.Generic.List<StoredProduct> StoredProducts
        {
            get
            {
                return null;
            }

        }
#if !DeviceDotNet
        /// <MetaDataID>{3272f68d-4058-42c4-aabf-70fc80c8abe4}</MetaDataID>
        OrderDetailProductsQuery OrderDetailProductsQuery =new OrderDetailProductsQuery();
       /// <MetaDataID>{20ff80af-051c-4ea6-bf2b-15f7eaf0b17d}</MetaDataID>
       public System.Collections.Generic.List<OrderDetailProduct> OrderDetailProducts
       {
           get
           {
               var tmp= OrderDetailProductsQuery.GetCollection(this).ToList();
               return tmp;
           }

       }

       /// <MetaDataID>{bcb3a281-e4da-4e6c-989c-c3db38e41973}</MetaDataID>
        FilteredOrderDetailProductsQuery FilteredOrderDetailProductsQuery=new FilteredOrderDetailProductsQuery();
        /// <MetaDataID>{9440f7ef-6d43-47c5-92fe-8fbd08f2a869}</MetaDataID>
       public System.Collections.Generic.List<OrderDetailProduct> FilteredOrderDetailProducts
       {
           get
           {
               var tmp= FilteredOrderDetailProductsQuery.GetCollection(this).ToList();
               return tmp;
           }

       }

#endif

        /// <MetaDataID>{CDC4282C-EF09-4C40-B7CA-494A96A12610}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_OrderDetails")]
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrderDetail> OrderDetails
        {
            get
            {
                // int cc = _OrderDetails.Count;
                //var ret = new OOAdvantech.Collections.Generic.Set<IOrderDetail>();
                //ret.Add(_OrderDetails[0]);
                //ret.Add(_OrderDetails[1]);
                return new OOAdvantech.Collections.Generic.Set<IOrderDetail>(_OrderDetails);
            }
        }



        /// <MetaDataID>{5e954bab-8d31-4dfc-bdf3-e7a25f08454e}</MetaDataID>
        public Order()
        {

        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{0618b12f-8c82-4ba8-ba00-3b5f01734cf1}</MetaDataID>
        public void Update()
        {
            OOAdvantech.EventUnderProtection.Invoke<OOAdvantech.ObjectChangeStateHandle>(ref ObjectChangeState, OOAdvantech.EventUnderProtection.ExceptionHandling.RemoveDisconnectedEventHandlers | OOAdvantech.EventUnderProtection.ExceptionHandling.IgnoreExceptions, this, "Client");

        }



        /// <MetaDataID>{47eaa8a0-e26f-4cee-aa75-49821cf5780d}</MetaDataID>
        public Set<IClient> Clients
        {
            get
            {
                Set<IClient> clientCollection = new Set<IClient>();
                ObjectStorage objectStorage = null;
                objectStorage = ObjectStorage.GetStorageOfObject(this);

                string objectQuery = "#OQL: SELECT client " +
                                  " FROM AbstractionsAndPersistency.IClient client #";

                StructureSet objectSet = objectStorage.Execute(objectQuery);
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
                return clientCollection;
            }
        }

        /// <exclude>Excluded</exclude>
        private IClient _Client;//OOAdvantech.Member<IClient> _Client = new OOAdvantech.Member<IClient>();
        /// <MetaDataID>{5AF5263D-D1F3-43B3-935E-0FBB52CC17A0}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_Client")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.OnConstruction)]//PersistencyFlag.OnConstruction | 
        public AbstractionsAndPersistency.IClient Client
        {
            get
            {
                //return _Client.Value;
                return _Client;
            }
            set
            {
                //if (_Client.Value != value)
                //{
                //    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                //    {
                //        _Client.Value = value;
                //        objStateTransition.Consistent = true;
                //    }
                //}
                _Client = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "Client");

            }
        }

        /// <MetaDataID>{cd886027-e536-4475-b1ce-314f106b6567}</MetaDataID>
        public Order Copy()
        {
            return new Order();

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{83C00F18-291D-49CA-BE94-F972C91C56A0}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{36AE5BCF-DD58-464B-90C7-DAE12F6A4D84}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_Name")]
        [TransactionalMember(LockOptions.Exclusive, "_Name")]
        public virtual string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {

                    //_Name = value;
                    //if (ObjectChangeState!=null)
                    //    ObjectChangeState(this, "Name");

                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "Name"))
                    {
                        _Name = value;
                        objStateTransition.Consistent = true;
                    }
                    //_Name = value;
                    //if (ObjectChangeState != null)
                    //    ObjectChangeState(this, "Name");

                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DB37DAFD-E7CF-4EC1-A8B7-7D25B5477C39}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<IOrderDetail> _OrderDetails = new OOAdvantech.Collections.Generic.Set<IOrderDetail>();


        /// <MetaDataID>{6C001D27-0211-4135-A14F-5A8A996568F2}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink Properties = new OOAdvantech.ObjectStateManagerLink();


        /// <MetaDataID>{217F7589-5F53-42FF-92D5-D06162D24FE0}</MetaDataID>
        public virtual void AddItem(IOrderDetail item)
        {
            if (item == null)
                return;
            if (_OrderDetails.Contains(item))
                return;
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {
                _OrderDetails.Add(item);
                objStateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{f76ea0f4-2323-4505-8c4b-0aac0a92b836}</MetaDataID>
        public void AddItem(int index, IOrderDetail item)
        {
            if (item == null)
                return;
            if (_OrderDetails.Contains(item))
                return;
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {
                _OrderDetails.Insert(index, item);
                objStateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{88F6DB1D-77FE-4EA1-AE36-FA6987D562CC}</MetaDataID>
        public virtual void RemoveItem(IOrderDetail item)
        {
            if (item == null)
                return;
            if (!_OrderDetails.Contains(item))
                return;
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {
                _OrderDetails.Remove(item);
                objStateTransition.Consistent = true;
                item.Order = null;
            }
            //OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(item);
        }

        /// <MetaDataID>{4f8f31e3-b3eb-4307-bdfe-e6b17230f951}</MetaDataID>
        public void RemoveItemAt(int index)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _OrderDetails.RemoveAt(index);
                stateTransition.Consistent = true;
            }

        }




        /// <MetaDataID>{5805a5d3-c582-468e-bb2e-851f15a89ed3}</MetaDataID>
        public Set<IClient> SearchForClient()
        {
            Set<IClient> clientCollection = new Set<IClient>();
            ObjectStorage objectStorage = null;
            objectStorage = ObjectStorage.GetStorageOfObject(this);
            string objectQuery = "#OQL: SELECT client " +
                              " FROM AbstractionsAndPersistency.IClient client #";

            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IClient client = null;
            int i = 0;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                client = objectSet["client"] as AbstractionsAndPersistency.Client;
                i++;
                clientCollection.Add(client);
                if (i > 12)
                    break;
            }
            return clientCollection;

        }












        /// <MetaDataID>{c2370b9a-4652-4221-b53b-363b68937346}</MetaDataID>
        public IOrderDetail CreateOrderDetail(int index)
        {
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {

                //IOrderDetail item =new OrderDetail();
                IOrderDetail item = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).NewObject(typeof(OrderDetail)) as IOrderDetail;

                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(item);
                _OrderDetails.Insert(index, item);
                // item.Order = this;
                objStateTransition.Consistent = true;
                return item;
            }


        }
        /// <MetaDataID>{34f1f3fb-7a0b-4fce-a622-5b6a8f3dafe4}</MetaDataID>
        public IOrderDetail CreateOrderDetail()
        {
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {

                //IOrderDetail item =new OrderDetail();
                IOrderDetail item = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).NewObject(typeof(OrderDetail)) as IOrderDetail;

                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(item);
                _OrderDetails.Add(item);
                // item.Order = this;
                objStateTransition.Consistent = true;
                return item;
            }
        }

        /// <MetaDataID>{075ccab8-cf56-4060-977c-685506bda5ea}</MetaDataID>
        public IOrderDetail CreateOrderDetail(IProductPrice productPrice)
        {
            using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
            {

                IOrderDetail item = new OrderDetail();
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(item);
                _OrderDetails.Add(item);
                item.Price = productPrice;
                //item.Order = this;
                item.Amount = 1;
                item.Name = productPrice.Product.Name;
                objStateTransition.Consistent = true;
                return item;
            }


        }


        /// <exclude>Excluded</exclude>
        System.DateTime? _OrderDate = System.DateTime.Now;

        /// <MetaDataID>{8c6848a9-e7d2-4dd5-bdca-1841cb6edf55}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember("_OrderDate")]
        public System.DateTime? OrderDate
        {
            get
            {
                return _OrderDate;
            }
            set
            {
                if (_OrderDate != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                    {
                        _OrderDate = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        private bool _Invoiced;
        /// <MetaDataID>{4fd9b34b-f4c5-4bd1-99d8-67742be3c5d7}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember(255, "_Invoiced")]
        [TransactionalMember(LockOptions.Exclusive, "_Invoiced")]
        public bool Invoiced
        {
            get
            {
                return _Invoiced;
            }
            set
            {
                if (_Invoiced != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "Invoiced"))
                    {
                        _Invoiced = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }

        //[Test.TestOnTheFly(Order.marteka)]
        /// <MetaDataID>{c7e7e841-4f10-497a-b721-dc8c7efe0827}</MetaDataID>
        public object Maana
        {
            get
            {
                return null;
            }
        }

        /// <exclude>Excluded</exclude>
        private OrderState _State;
        /// <MetaDataID>{4c527f76-2545-46b3-83c0-ca4e631c6d03}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember(255, "_State")]
        public OrderState State
        {
            get
            {
                return _State;
            }
            set
            {
                if (_State != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                    {
                        _State = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        private double _ItemsNumber;
        /// <MetaDataID>{01331848-9f5a-4e0c-b4c0-478cef70c99e}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        [PersistentMember(255, "_ItemsNumber")]
        public double ItemsNumber
        {
            get
            {
                return _ItemsNumber;
            }
            set
            {
                if (_ItemsNumber != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsNumber = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }
#if !DeviceDotNet
        /// <exclude>Excluded</exclude>
        Folder _CDriveFolder;
        /// <MetaDataID>{9cc47e2f-7da3-4225-9298-aa6f6f8ecc15}</MetaDataID>
        public INodeObject CDriveFolder
        {
            get
            {
                if (_CDriveFolder == null)
                    _CDriveFolder = new Folder(new System.IO.DirectoryInfo(@"C:\"));
                return _CDriveFolder;

            }
        }


        /// <MetaDataID>{88aca187-b9f9-4920-9116-8022838a125c}</MetaDataID>
        public IClient ClientB
        {
            get
            {


                try
                {
                    System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(@"Data Source=ROCKET\sqlexpress;Initial Catalog=Abstractions;Integrated Security=True");
                    connection.Open();

                }
                catch (System.Exception error)
                {


                }
                WindowsIdentity Identity = WindowsIdentity.GetCurrent();

                WindowsIdentity identity = (WindowsIdentity)Thread.CurrentPrincipal.Identity;

                using (WindowsImpersonationContext ctx = identity.Impersonate())
                {
                    // Here we are impersonating as the management client user
                    System.Diagnostics.Debug.WriteLine(WindowsIdentity.GetCurrent().Name);
                }

                using (WindowsImpersonationContext ctx = Identity.Impersonate())
                {
                    // Do work acting as the service user
                    System.Diagnostics.Debug.WriteLine(WindowsIdentity.GetCurrent().Name);
                }

                using (WindowsImpersonationContext ctx = identity.Impersonate())
                {
                    // Here we are impersonating as the management client user
                    System.Diagnostics.Debug.WriteLine(WindowsIdentity.GetCurrent().Name);
                }




                return Client;

            }
        }
#endif




    }
#if !DeviceDotNet
    /// <MetaDataID>{2f0e8a62-14bf-4509-94aa-67660b69f5e1}</MetaDataID>
    public class TransientFolder : System.MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{1977a02d-23b5-4005-ac29-7139f8426a04}</MetaDataID>
        public TransientFolder()
        {
            Name = "Liolios";
        }
        /// <MetaDataID>{c3fd9697-3fb8-4130-9869-b5eb651ee0d9}</MetaDataID>
        public string Name
        {
            get;
            set;
        }
    }

    /// <MetaDataID>{b8da8e08-f559-4c20-b860-36209ec0f53b}</MetaDataID>
    public class Folder : INodeObject
    {
        /// <MetaDataID>{00c7d5b2-888f-45c9-9202-81980b059c57}</MetaDataID>
        public System.IO.DirectoryInfo Directory;
        /// <MetaDataID>{e5a91708-9ead-4beb-b7e3-f409a5131a1f}</MetaDataID>
        public Folder(System.IO.DirectoryInfo directory)
        {
            Directory = directory;


        }
    #region INodeObject Members

        /// <MetaDataID>{0ac43936-f700-4c2b-a642-231e0bb4c2fc}</MetaDataID>
        Set<INodeObject> _SubNodeObjects;
        /// <MetaDataID>{fb1288f1-f5b6-435c-83cf-e5408e601b59}</MetaDataID>
        public Set<INodeObject> SubNodeObjects
        {
            get
            {
                if (_SubNodeObjects == null)
                {
                    _SubNodeObjects = new Set<INodeObject>();

                    try
                    {
                        foreach (System.IO.DirectoryInfo subDir in Directory.GetDirectories())
                            _SubNodeObjects.Add(new Folder(subDir));
                    }
                    catch (System.Exception error)
                    {

                    }
                }

                return new Set<INodeObject>(_SubNodeObjects, OOAdvantech.Collections.CollectionAccessType.ReadOnly);

            }

        }

        /// <MetaDataID>{aec01703-9f2e-4b92-9d53-02d4b45d46cd}</MetaDataID>
        public string Name
        {
            get
            {
                return Directory.Name;
            }
            set
            {

                Directory.MoveTo(Directory.Parent.FullName + @"\" + value);

            }
        }

    #endregion
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{072e82ff-5398-4960-b613-f9dae6d0c6e6}</MetaDataID>
        public Folder CreateDirectory(string name)
        {
            Folder folder = new Folder(System.IO.Directory.CreateDirectory(Directory.FullName + @"\" + name));
            if (_SubNodeObjects == null)
                _SubNodeObjects = new Set<INodeObject>();

            _SubNodeObjects.Add(folder);
            return folder;
        }



    }
#endif









}

