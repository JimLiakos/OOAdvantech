using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections;

namespace UserInterfaceTest
{
    /// <MetaDataID>{6423c304-6a64-4c77-a9bc-0c4e7f2d684e}</MetaDataID>
    public class OrderPresentationObject : OOAdvantech.UserInterface.Runtime.PresentationObject<AbstractionsAndPersistency.IOrder>
    {
        /// <MetaDataID>{92152d24-a19a-4ef4-8ba7-ee7a9a41e0cc}</MetaDataID>
        public override void Initialize()
        {
            base.Initialize();
        }



        /// <MetaDataID>{a69cd052-72b5-4345-ad81-7bf33e576d8d}</MetaDataID>
        AbstractionsAndPersistency.PriceList _PriceList;

        /// <MetaDataID>{f06bf86b-af10-4904-a86e-c9e8765f23f2}</MetaDataID>
        public AbstractionsAndPersistency.PriceList PriceList
        {
            get
            {
                return _PriceList;

            }
            set
            {
                _PriceList = value;
                _PriceList.Name = _PriceList.Name + "a";


            }
        }
       
      
        /// <MetaDataID>{d5b11a57-c704-4f0f-98da-285dddfe1fcb}</MetaDataID>
        public AbstractionsAndPersistency.OrderState OrderState
        {
            get
            {
                return RealObject.State;
            }
            set
            {
                RealObject.State = value;
            }
        }
        /// <MetaDataID>{bf001a92-85ad-4733-9e45-2b0ef27c4b37}</MetaDataID>
        bool ReturnOrderDetails = false;
        /// <MetaDataID>{7b67ab72-6e95-4bbb-883d-9efc310da2e0}</MetaDataID>
        public void Refresh()
        {
            ReturnOrderDetails = true;
            ObjectChangeState(this, "OrderDetails");
            if(OrderDetails.Count>3)
            {
                _SelOrderDetail = OrderDetails[6];
                ObjectChangeState(this, "SelOrderDetail");
            }
            
        }

        /// <MetaDataID>{59f16da3-99b5-4452-ae79-3b54247418c8}</MetaDataID>
        public string NewName
        {
            get
            {
                return "Test_" + RealObject.Name;
            }
        }
        /// <MetaDataID>{e16f9779-fb55-4d80-88d3-c085038e35ac}</MetaDataID>
        public AbstractionsAndPersistency.Category Category
        {
            get
            {
                
                OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(NativeObject).Execute("select categoty from AbstractionsAndPersistency.Category categoty where categoty.Root = true");
                foreach (OOAdvantech.Collections.StructureSet instanceSet in structureSet)
                {
                    return instanceSet["categoty"] as AbstractionsAndPersistency.Category;
                }

                AbstractionsAndPersistency.Category category = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(NativeObject).NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.Category;
                category.Name = "Root";
                category.Root = true;
                return category;
            }
        }

        /// <MetaDataID>{52c059ed-bea0-4134-8a26-5a1b0e655923}</MetaDataID>
        public OrderPresentationObject(AbstractionsAndPersistency.IOrder order)
            : base(order)
        {





        }

        /// <MetaDataID>{1e5bec72-9a77-456f-91b0-d45d012f3313}</MetaDataID>
        ~OrderPresentationObject()
        {
        }

        /// <MetaDataID>{e1c4ea0f-4924-4044-8cbf-3ae598b259fa}</MetaDataID>
        public OOAdvantech.DragDropMethod CanDropObjectAsCategory(object dropObject)
        {

            if (dropObject is AbstractionsAndPersistency.Category)
                return OOAdvantech.DragDropMethod.Move;
            else
                return OOAdvantech.DragDropMethod.None;
        }

        /// <MetaDataID>{8e33fc55-93d4-4542-9606-b454219377bd}</MetaDataID>
        public OOAdvantech.DragDropMethod CanDropObjectForClient(object dropObject)
        {

            if (dropObject is AbstractionsAndPersistency.IClient)
                return OOAdvantech.DragDropMethod.Copy;
            else
                return OOAdvantech.DragDropMethod.None;


        }

        /// <MetaDataID>{16ec7756-264f-464a-b38f-484b304f8dc7}</MetaDataID>
        public System.Collections.Generic.List<AbstractionsAndPersistency.IOrderDetail> DropObjectForOrderDetail(object dropObject)
        {
            System.Collections.Generic.List<AbstractionsAndPersistency.IOrderDetail> orderDetails = new System.Collections.Generic.List<AbstractionsAndPersistency.IOrderDetail>();
            if (dropObject is Array && (dropObject as Array).Length > 0)
            {
                foreach (object obj in (dropObject as Array))
                {
                    if ((obj is AbstractionsAndPersistency.IProductPrice))
                        orderDetails.Add(RealObject.CreateOrderDetail(obj as AbstractionsAndPersistency.IProductPrice));

                    if ((obj is AbstractionsAndPersistency.IOrderDetail))
                    {
                        RealObject.AddItem(obj as AbstractionsAndPersistency.IOrderDetail);
                        orderDetails.Add(obj as AbstractionsAndPersistency.IOrderDetail);
                    }

                }

            }
            else
            {
                if ((dropObject is AbstractionsAndPersistency.IProductPrice))
                    orderDetails.Add(RealObject.CreateOrderDetail(dropObject as AbstractionsAndPersistency.IProductPrice));

                if ((dropObject is AbstractionsAndPersistency.IOrderDetail))
                {
                    RealObject.AddItem(dropObject as AbstractionsAndPersistency.IOrderDetail);
                    orderDetails.Add(dropObject as AbstractionsAndPersistency.IOrderDetail);
                }
                if ((dropObject is AbstractionsAndPersistency.IProduct))
                {
                    foreach (AbstractionsAndPersistency.IProductPrice productPrice in (dropObject as AbstractionsAndPersistency.IProduct).PriceLists)
                    {
                        if (productPrice.Product == (dropObject as AbstractionsAndPersistency.IProduct))
                        {
                            orderDetails.Add(RealObject.CreateOrderDetail(productPrice));
                            return orderDetails;
                        }

                    }
                }
            }
            return orderDetails;



        }
        /// <MetaDataID>{d60b0f73-d406-4773-b5b2-73b5419c8b24}</MetaDataID>
        public AbstractionsAndPersistency.Category InsertCategory(AbstractionsAndPersistency.Category insideCategory, int atIndex, object copyCategory)
        {
            insideCategory.AddSubCategory(copyCategory as AbstractionsAndPersistency.Category);
            return copyCategory as AbstractionsAndPersistency.Category;

        }
        /// <MetaDataID>{55b6136f-3245-4c16-9eb8-ff8785642915}</MetaDataID>
        public bool CutCategory(AbstractionsAndPersistency.ICategory insideCategory, object removedCategory)
        {
            insideCategory.DeleteSubCategory(removedCategory as AbstractionsAndPersistency.Category);
            return true;

        }
        /// <MetaDataID>{8f8bc417-d079-4ec5-b4ce-1d79d0fd8f2c}</MetaDataID>
        public void EditOrderDetail(AbstractionsAndPersistency.IOrderDetail orderDetail)
        {
            OrderDetailForm orderDetailForm = new OrderDetailForm();
            orderDetailForm.Connection.Instance = orderDetail;
            orderDetailForm.ShowDialog();
        }

        /// <MetaDataID>{f742bc08-d605-40ad-a47a-3065279213f3}</MetaDataID>
        public OOAdvantech.DragDropMethod CanDropObjectForOrderDetail(object dropObject)
        {
            if (dropObject is Array && (dropObject as Array).Length > 0)
            {
                foreach (object obj in (dropObject as Array))
                {
                    if (!(obj is AbstractionsAndPersistency.IOrderDetail) &&
                        !(obj is AbstractionsAndPersistency.IProduct) &&
                        !(obj is AbstractionsAndPersistency.IProductPrice))
                    {
                        return OOAdvantech.DragDropMethod.None;
                    }
                }
                return OOAdvantech.DragDropMethod.Copy;
            }
            if (dropObject is AbstractionsAndPersistency.IOrderDetail ||
                dropObject is AbstractionsAndPersistency.IProductPrice ||
                dropObject is AbstractionsAndPersistency.IProduct)
                return OOAdvantech.DragDropMethod.Copy;
            else
                return OOAdvantech.DragDropMethod.None;


        }
        /// <MetaDataID>{ba2313ba-e2bf-4ede-936a-3418ac2b9d6b}</MetaDataID>
        public OOAdvantech.DragDropMethod CanDropObjectForOrder(object dropObject)
        {
            if (RealObject.Equals(dropObject))
                return OOAdvantech.DragDropMethod.None;
            if (dropObject is AbstractionsAndPersistency.IOrder)
                return OOAdvantech.DragDropMethod.Copy;
            else
                return OOAdvantech.DragDropMethod.None;
        }
        /// <MetaDataID>{c1dfb1b3-ff48-4585-ac66-04ce5cebb84f}</MetaDataID>
        public void Delete(AbstractionsAndPersistency.ICategory nodeObject)
        {

           // nodeObject.Parent.DeleteSubCategory(nodeObject);
        }
        /// <MetaDataID>{2846e855-f539-4db7-9b1e-b5edd33d3e78}</MetaDataID>
        public void DeleteOrderDetail(AbstractionsAndPersistency.IOrderDetail orderDetail)
        {
            RealObject.RemoveItem(orderDetail);
            
        }
        /// <MetaDataID>{f6170a4d-bcb8-41f4-aa47-405bcba45758}</MetaDataID>
        public AbstractionsAndPersistency.INodeObject AddFolder(AbstractionsAndPersistency.INodeObject nodeObject)
        {
            return (nodeObject as AbstractionsAndPersistency.Folder).CreateDirectory("Mitros");

        }
        /// <MetaDataID>{55923df1-a228-492c-b60a-d9df5ddecc17}</MetaDataID>
        public AbstractionsAndPersistency.Category AddCategory(AbstractionsAndPersistency.ICategory category)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                //AbstractionsAndPersistency.Category newCategory = new AbstractionsAndPersistency.Category();
                AbstractionsAndPersistency.Category newCategory = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(NativeObject).NewObject(typeof(AbstractionsAndPersistency.Category)) as AbstractionsAndPersistency.Category;
                newCategory.Name = "newCategory";
                category.AddSubCategory(newCategory);
                //OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(NativeObject).CommitTransientObjectState(newCategory);
                stateTransition.Consistent = true;
                return newCategory;
            }
        }
        /// <MetaDataID>{69b13a68-c209-4edc-963f-2bc007158d47}</MetaDataID>
        public AbstractionsAndPersistency.IOrderDetail AddOrderDetail(int index)
        {
            return RealObject.CreateOrderDetail(index);

        }
        /// <MetaDataID>{5e5959ac-003e-44dc-9921-18782b9fa372}</MetaDataID>
        AbstractionsAndPersistency.INodeObject _SelectedNode;
        /// <MetaDataID>{a107e072-7f55-4f8c-94f2-bf6e2fdb7d6e}</MetaDataID>
        public AbstractionsAndPersistency.INodeObject SelectedNode
        {
            get
            {
                return _SelectedNode;
            }
            set
            {
                _SelectedNode = value;
            }
        }
        /// <MetaDataID>{99381f1a-3bac-4e2f-932b-e3d1e80696ed}</MetaDataID>
        public List<AbstractionsAndPersistency.IProductPrice> GetCandidateProductPrices()
        {

            ObjectStorage objectStorage = Form3.OpenStorage();

            string objectQuery = "#OQL: SELECT productPrice  " +
                              " FROM AbstractionsAndPersistency.ProductPrice productPrice  #";//WHERE productPrice.PriceList = @PriceList#";

            List<AbstractionsAndPersistency.IProductPrice> productPrices = new List<AbstractionsAndPersistency.IProductPrice>();
            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IProductPrice productPrice = null;
            int i = 0;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                productPrice = objectSet["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                productPrices.Add(productPrice);
                //  break;
            }
            return productPrices;
        }

        /// <MetaDataID>{dfe09570-237c-48ee-b543-9e061efe510d}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IPriceList> SearchForPriceList()
        {
            OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IPriceList> priceListCollection = new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IPriceList>();
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = null;
            objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(NativeObject);// ObjectStorage.[.OpenStorage("Abstractions",


            string objectQuery = "#OQL: SELECT priceList  " +
                              " FROM AbstractionsAndPersistency.IPriceList priceList #";


            OOAdvantech.Collections.StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IPriceList priceList = null;
            int i = 0;
            foreach (OOAdvantech.Collections.StructureSet objectSetInstance in objectSet)
            {
                priceList = objectSet["priceList"] as AbstractionsAndPersistency.IPriceList;
                i++;
                priceListCollection.Add(priceList);
                if (i > 12)
                    break;
            }
            return priceListCollection;



        }


        /// <MetaDataID>{23453c21-0c51-4764-ae44-e4f4d883cf3b}</MetaDataID>
        [OOAdvantech.UserInterface.CollectionObjectPaths(new string[1] { "Price" })]
        public OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrderDetail> OrderDetails
        {
            get
            {
                if (ReturnOrderDetails)
                    return RealObject.OrderDetails;
                else
                    return new OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IOrderDetail>();
            }
        }

        /// <MetaDataID>{f8e52f31-e5d5-451e-a054-5871f12e13ac}</MetaDataID>
        bool _ListEnabled = true;
        /// <MetaDataID>{9b76b2c9-c3cf-4b7d-96c2-af7f4d53e18a}</MetaDataID>
        public bool ListEnabled
        {
            get
            {
                return _ListEnabled;
            }
        }
        /// <MetaDataID>{f78e4e5e-0b35-4f7f-9c55-dab30258abaa}</MetaDataID>
        private AbstractionsAndPersistency.IOrderDetail _SelOrderDetail;

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{3a41bdbf-5cba-42d6-b45f-5e74fab54d2b}</MetaDataID>
        public AbstractionsAndPersistency.IOrderDetail SelOrderDetail
        {
            get { return _SelOrderDetail; }
            set 
            {
                //_ListEnabled = false;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "ListEnabled");

                if (OrderDetailView == ConnectableControls.DynamicViewContainer.UserViewControlIdentity.Empty)
                    OrderDetailView = ConnectableControls.UserControl.GetUserControlIdentity<SpecOrderDetailUserControl>();
                else if (OrderDetailView == ConnectableControls.UserControl.GetUserControlIdentity <SpecOrderDetailUserControl>())
                {
                    OrderDetailView = ConnectableControls.UserControl.GetUserControlIdentity<OrderDetailUserControl>();
                }
                else
                {
                    OrderDetailView = ConnectableControls.UserControl.GetUserControlIdentity<SpecOrderDetailUserControl>();
                }

                _SelOrderDetail = value;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "OrderDetailView");
                
            }
        }

        /// <MetaDataID>{c3009460-d8c5-411a-acb6-da69a0bef59b}</MetaDataID>
        public ConnectableControls.DynamicViewContainer.UserViewControlIdentity OrderDetailView = ConnectableControls.UserControl.GetUserControlIdentity<SpecOrderDetailUserControl>();



        /// <MetaDataID>{7915611f-be7f-4aab-b4aa-ba2dff17e1e2}</MetaDataID>
        AbstractionsAndPersistency.IOrder _Order;

        /// <MetaDataID>{8c727415-05d5-4583-9ffe-922ccd4e3f4d}</MetaDataID>
        public AbstractionsAndPersistency.IOrder Order
        {
            get
            {
                return _Order;

            }
            set
            {
                _Order = value;
            }
        }


        /// <MetaDataID>{b231897e-5a0e-44f6-b6b9-2527141bfd23}</MetaDataID>
        public override AbstractionsAndPersistency.IOrder RealObject
        {
            get
            {
                return base.RealObject;
            }
        }
        /// <MetaDataID>{4f05422f-d644-424e-b0e3-270c9ef5729d}</MetaDataID>
        public void NewForm(AbstractionsAndPersistency.IOrder order)
        {
            Form2 form = new Form2();
            form.ViewControlObject.Instance = order;
            form.Text = "NestedForm";
            form.ShowDialog();
        }

    }


    /// <MetaDataID>{01f5dcb5-7265-41c6-896c-3313f51752d4}</MetaDataID>
    class OrderPresentationObjectB : OrderPresentationObject
    {
        public OrderPresentationObjectB(AbstractionsAndPersistency.IOrder order)
            : base(order)
        {

        }
    }
}
