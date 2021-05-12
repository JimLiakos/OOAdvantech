namespace AbstractionsAndPersistency
{
    using System;
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Remoting;
    using OOAdvantech.Transactions;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif



    /// <MetaDataID>{3FB56D6E-2AA9-433C-A6DA-CB430F351E50}</MetaDataID>
    [BackwardCompatibilityID("{3FB56D6E-2AA9-433C-A6DA-CB430F351E50}")]
    [Persistent()]
    public class ProductPrice : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IProductPrice
    {
        /// <MetaDataID>{F1820A03-18F9-42E1-9F5E-6233F3724438}</MetaDataID>
        protected ProductPrice()
        {

        }
        /// <MetaDataID>{2261441b-e21c-4e39-bcd5-030d4a40bafc}</MetaDataID>
        public override string ToString()
        {
            return base.ToString();
        }
        /// <MetaDataID>{65C2BA81-0803-4020-A3A3-465D47FF5060}</MetaDataID>
        public ProductPrice(string name)
        {
            _Name = name;
        }
        /// <MetaDataID>{64B15AB6-C470-442A-8C75-41C87CBECC6C}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink Properties = new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4A7782D2-B983-4297-9688-F0A4D41953C8}</MetaDataID>
        private IPriceList _PriceList;
        /// <MetaDataID>{5103AFEF-6ADF-4D48-828E-3D6AE19DA2DE}</MetaDataID>
        [AssociationClassRole(Roles.RoleB, "_PriceList")]
        public IPriceList PriceList
        {
            get
            {
                return _PriceList;
            }
            set
            {
                if (_PriceList == value)
                    return;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _PriceList = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A406899F-B1D7-4267-B3B5-11B81CD8CBC8}</MetaDataID>
        private IProduct _Product;
        /// <MetaDataID>{578F7127-655E-40DA-8BA6-9A2E80CF6FD3}</MetaDataID>
        [AssociationClassRole(Roles.RoleA, "_Product")]
        public IProduct Product
        {
            get
            {
                return _Product;
            }
            set
            {
                if (_Product == value)
                    return;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Product = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8B8D2AD6-3452-4518-A33D-FD25C21F7EED}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{011D353C-B0EB-4F70-A824-97608A46A258}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember(255, "_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        Quantity _Price;
        /// <MetaDataID>{97871e58-ebfc-43ce-8774-ce2562647764}</MetaDataID>
        [PersistentMember("_Price")]
        [BackwardCompatibilityID("+3")]
        public Quantity Price
        {
            get
            {
                return _Price;
            }
            set
            {
                if (_Price != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Price = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


    }
}
