using OOAdvantech.MetaDataRepository;
using OOAdvantech.Collections.Generic;
using System.Linq;
namespace AbstractionsAndPersistency
{
    /// <MetaDataID>{1c3956a4-d0f9-4f6c-9155-17bc86cee637}</MetaDataID>
    public enum PaymentMethodType
    {
        Cash,
        Credit,
        Cheque,
        CreditCardPayment,
        BillOfExchange,
        CompoundPayment
    }

    /// <MetaDataID>{03b6e425-8f25-444f-ba57-c1646e5b9d50}</MetaDataID>
    public enum OrderState
    {
        Open,
        InProgress,
        UnderConstruction,
        Delivered
    }
    /// <MetaDataID>{991a7ab9-956b-4188-8283-19bd349c683d}</MetaDataID>
    public interface IPorder
    {
        /// <MetaDataID>{633ce8a6-f3b8-4e60-8bcc-aa24e07b38c4}</MetaDataID>
        string Camer
        {
            get;
            set;
        }
    }
    /// <MetaDataID>{6C7B04EB-053A-46FD-BB2C-281665043BA2}</MetaDataID>
    [BackwardCompatibilityID("{6C7B04EB-053A-46FD-BB2C-281665043BA2}")]
    public interface IOrder
    {
        [Association("InspectionOrder", Roles.RoleA, "dcf5a2fd-801b-4197-8d0c-1b86e3c19965")]
        [RoleAMultiplicityRange(0, 1)]
        IInspection Inspection
        {
            get;
            set;
        }



        /// <MetaDataID>{cf76d04b-895f-4755-896b-764d3dc71d09}</MetaDataID>
        [Association("ClientOrders", Roles.RoleB, "{7F7E54C7-CB3F-4159-8927-57724CBCE08D}")]
        [RoleBMultiplicityRange(0, 1)]
        AbstractionsAndPersistency.IClient Client
        {
            get;
            set;
        }


      


        /// <MetaDataID>{c409b2f1-5735-4cdc-a3be-d0dfb2215281}</MetaDataID>
        [OOAdvantech.MetaDataRepository.DerivedMember(typeof(OrderStoredProductsQuery))]
        System.Collections.Generic.List<StoredProduct> StoredProducts
        {
            get;
        }

#if !DeviceDotNet
        /// <MetaDataID>{9ab7a59d-9b75-440c-a05f-b46e997d020d}</MetaDataID>
        IClient ClientB
        {
            get;
        }

          /// <MetaDataID>{c409b2f1-5735-4cdc-a3be-d0dfb2215281}</MetaDataID>
        [OOAdvantech.MetaDataRepository.DerivedMember(typeof(OrderProductsQuery))]
        System.Collections.Generic.List<IProduct> Products
        {
            get;
        }
        /// <MetaDataID>{dc075869-de09-4588-a1b1-0bb25774c7cb}</MetaDataID>
        [OOAdvantech.MetaDataRepository.DerivedMember(typeof(OrderDetailProductsQuery))]
        System.Collections.Generic.List<OrderDetailProduct> OrderDetailProducts
        {
            get;
        }
        /// <MetaDataID>{0f7d9a96-6a17-4513-a724-f45cbf5eec03}</MetaDataID>
        [OOAdvantech.MetaDataRepository.DerivedMember(typeof(FilteredOrderDetailProductsQuery))]
        System.Collections.Generic.List<OrderDetailProduct> FilteredOrderDetailProducts
        {
            get;
        }





        /// <MetaDataID>{e63921f3-c5ad-40cb-b070-ed346af0c477}</MetaDataID>
        INodeObject CDriveFolder
        {
            get;
        }
#endif
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{7ba3f034-b5c9-455f-909b-3b4ca5adf96b}</MetaDataID>
        Set<IClient> SearchForClient();
        /// <MetaDataID>{51c7a912-2f1d-4d2a-a6d8-57304574a375}</MetaDataID>
        Set<IClient> Clients
        {
            get;
        }
        /// <MetaDataID>{2f83913d-33f6-4b02-ac60-117860b6e154}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        bool Invoiced
        {
            get;
            set;
        }
        /// <MetaDataID>{ada03914-7793-4046-bd99-49a953249ed0}</MetaDataID>
        [BackwardCompatibilityID("+22")]
        OrderState State
        {
            get;
            set;
        }
        /// <MetaDataID>{ad85774a-7b5f-4d9b-9cc6-b8e0d045891b}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double ItemsNumber
        {
            get;
            set;
        }
 

        /// <MetaDataID>{9C91BF01-2D6A-4BF8-89F6-91DF5D40DFAE}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string Name
        {
            get;
            set;
        }
        /// <MetaDataID>{a1c209df-7731-4ce2-950b-408968e20152}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        System.DateTime? OrderDate
        {
            get;
            set;
        }



        /// <MetaDataID>{5983D693-5566-402C-9F5C-1E8D3502322C}</MetaDataID>
        void AddItem(IOrderDetail item);
        /// <MetaDataID>{DB78C53F-296F-4375-A85E-FB6843343D0B}</MetaDataID>
        void RemoveItem(IOrderDetail item);
        /// <MetaDataID>{ffd68016-ac7c-4c06-b027-2b98fd4101a3}</MetaDataID>
        void RemoveItemAt(int index);

        /// <MetaDataID>{92d2c23a-af8e-4cda-bdfb-d8bf4e28049f}</MetaDataID>
        IOrderDetail CreateOrderDetail();
        /// <MetaDataID>{f8c3f330-95f7-478b-8b89-35819d295dbd}</MetaDataID>
        IOrderDetail CreateOrderDetail(int index);
        /// <MetaDataID>{e98ac363-189d-4350-9558-c6aa5b4c85a4}</MetaDataID>
        IOrderDetail CreateOrderDetail(IProductPrice productPrice);



        /// <MetaDataID>{06F16AF7-2AA3-4352-AC2A-F98C5AA956F7}</MetaDataID>
        [Association("OrderDetails", Roles.RoleA, true, "{BC804838-B915-48E2-998A-D482ED2D048A}")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)] //PersistencyFlag.OnConstruction |
        [RoleAMultiplicityRange(1)]
        OOAdvantech.Collections.Generic.Set<IOrderDetail> OrderDetails
        {
            get;
        }

        /// <MetaDataID>{e64df86b-fa9c-4fa7-8ce3-b9e2aa08f04c}</MetaDataID>
        void AddItem(int index, IOrderDetail item);

        /// <MetaDataID>{61e4e138-25a6-45da-b738-c5540bfda352}</MetaDataID>
        void Update();
    }

    /// <MetaDataID>{8fadff4a-8b25-4cd0-b1ca-9a145b74fb2a}</MetaDataID>
    class OrderProductsQuery : IDerivedMemberExpression<IOrder,IProduct>
    {
        /// <MetaDataID>{16d03fc9-9fe1-4f37-ab12-70dc2cb353cd}</MetaDataID>
        public System.Collections.Generic.IEnumerable<IProduct> GetCollection(IOrder order)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{95b42c45-0311-493d-b204-a0c6ff0d706e}</MetaDataID>
        public IQueryable QueryableCollection
        {
            get
            {
                return from order in OOAdvantech.Linq.DerivedMembersExpresionBuilder<IOrder>.ObjectCollection
                       from orderDetail in order.OrderDetails
                       where orderDetail.Price.Product.Name == "sprite"
                       select orderDetail.Price.Product;
            }
        }


        /// <MetaDataID>{4ede97b0-5055-4c49-8260-ef1d46b2bcb1}</MetaDataID>
        public IProduct GetValue(IOrder _object)
        {
            throw new System.NotImplementedException();
        }

    }

    /// <MetaDataID>{8fadff4a-8b25-4cd0-b1ca-9a145b74fb2a}</MetaDataID>
    class OrderStoredProductsQuery : IDerivedMemberExpression<IOrder,StoredProduct>
    {


        /// <MetaDataID>{4c966d9e-82fd-49fd-9c61-10fbd6b3966d}</MetaDataID>
        public System.Collections.Generic.IEnumerable<StoredProduct> GetCollection(IOrder order)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{45f99143-f2f1-4b5d-8614-29a4fc4f3f3d}</MetaDataID>
        public IQueryable QueryableCollection
        {
            get
            {
                return from order in OOAdvantech.Linq.DerivedMembersExpresionBuilder<IOrder>.ObjectCollection
                       from orderDetail in order.OrderDetails
                       //where (orderDetail.Price.Product.Name == "sprite" && orderDetail.Price.Name == "sprite") || orderDetail.Name.Like("*spr*")
                       from priceListProduct in orderDetail.Price.PriceList.Products
                       from storePlace in orderDetail.Price.Product.StorePlaces

                       select new StoredProduct() { Product = orderDetail.Price.Product, StorePlace = storePlace };
            }
        }

        /// <MetaDataID>{9caa233a-7cc4-490d-9e22-8c22d5188691}</MetaDataID>
        public StoredProduct GetValue(IOrder _object)
        {
            throw new System.NotImplementedException();
        }

    }


#if !DeviceDotNet
      /// <MetaDataID>{322d9b35-f931-4b56-9a11-faf20cefe874}</MetaDataID>
    class FilteredOrderDetailProductsQuery : IDerivedMemberExpression<IOrder, OrderDetailProduct>
    {

        /// <MetaDataID>{a326eeee-7cec-4f8a-be7f-196b58580745}</MetaDataID>
        public IQueryable QueryableCollection
        {
            get
            {
                return from order in OOAdvantech.Linq.DerivedMembersExpresionBuilder<IOrder>.ObjectCollection
                       from orderDetail in order.OrderDetails
                       where orderDetail.Price.Product.Name == "sprite"
                       select new OrderDetailProduct()
                       {
                           Order = order,
                           Product = orderDetail.Price.Product,
                           OrderDetail = orderDetail
                       };
            }
        }

        /// <MetaDataID>{23b3c2ba-a7c7-4398-81b2-cbd030c70d10}</MetaDataID>
        public System.Collections.Generic.IEnumerable<OrderDetailProduct> GetCollection(IOrder order)
        {
            {
                return OOAdvantech.Linq.ObjectValuesCollector.GetObjectValues(order,
                        mOrder => from orderDetail in mOrder.OrderDetails
                                  where orderDetail.Price.Product.Name == "sprite"
                                  select new OrderDetailProduct()
                                  {
                                      Order = order,
                                      Product = orderDetail.Price.Product,
                                      OrderDetail = orderDetail
                                  });
            }
        }


        /// <MetaDataID>{ba7ca984-4f64-4195-a1a1-54a25b91e165}</MetaDataID>
        public OrderDetailProduct GetValue(IOrder _object)
        {
            throw new System.NotImplementedException();
        }

    }
 

    /// <MetaDataID>{322d9b35-f931-4b56-9a11-faf20cefe874}</MetaDataID>
    class OrderDetailProductsQuery : IDerivedMemberExpression<IOrder,OrderDetailProduct>
    {

        /// <MetaDataID>{3d9901a2-ccb8-436d-9809-3284c4a71e4b}</MetaDataID>
        public IQueryable QueryableCollection
        {
            get
            {
                return from order in OOAdvantech.Linq.DerivedMembersExpresionBuilder<IOrder>.ObjectCollection
                       from orderDetail in order.OrderDetails
                      // where orderDetail.Price.Product.Name == "sprite"
                       select new OrderDetailProduct()
                       {
                           Order = order,
                           Product = orderDetail.Price.Product,
                           OrderDetail = orderDetail
                       };
            }
        }

        /// <MetaDataID>{f3a86310-6089-4ea0-98b8-8e5710b9602d}</MetaDataID>
        public System.Collections.Generic.IEnumerable<OrderDetailProduct> GetCollection(IOrder order)
        {
            {
                var result= OOAdvantech.Linq.ObjectValuesCollector.GetObjectValues(order,
                        mOrder => new
                        {
                            Order = mOrder.Fetching(mOrder.OrderDetails),
                            OrderDetailProducts = from orderDetail in mOrder.OrderDetails
                                                  //where orderDetail.Price.Product.Name == "sprite"
                                                  select new OrderDetailProduct()
                                                  {
                                                      Order = order,
                                                      Product = orderDetail.Price.Product,
                                                      OrderDetail = orderDetail
                                                  }
                        });
                return result.OrderDetailProducts;
            }
        }


        /// <MetaDataID>{d76b36cd-c59f-40d9-bf51-7170fa0c9003}</MetaDataID>
        public OrderDetailProduct GetValue(IOrder _object)
        {
            throw new System.NotImplementedException();
        }


}
#endif
    /// <MetaDataID>{a0d02cf4-0b04-41f2-a48f-a766f96e468b}</MetaDataID>
    public class StoredProduct
    {
        /// <MetaDataID>{d4f02fc5-44f3-4bf3-808d-d7423781057f}</MetaDataID>
        public IProduct Product
        {
            get;
            set;
        }

        /// <MetaDataID>{cf965b73-bc5c-4c32-9082-ed00c8cbd0b3}</MetaDataID>
        public IStorePlace StorePlace
        {
            get;
            set;
        }
    }

    /// <MetaDataID>{23a1b30f-b2d5-4a5f-868b-524e4fec6bac}</MetaDataID>
    public class OrderDetailProduct
    {
        /// <MetaDataID>{1120432f-965a-48f2-a3e8-3622c4ea79ae}</MetaDataID>
        public IProduct Product
        {
            get;
            set;
        }

        /// <MetaDataID>{5d061eb1-71d1-48b9-a10f-ca2a1d4b58cd}</MetaDataID>
        public IOrderDetail OrderDetail
        {
            get;
            set;
        }
        /// <MetaDataID>{6c09a0a8-60d7-407a-8257-112e59260423}</MetaDataID>
        public IOrder Order
        {
            get;
            set;
        }


    }

}
