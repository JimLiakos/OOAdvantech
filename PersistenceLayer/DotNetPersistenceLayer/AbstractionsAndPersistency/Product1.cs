namespace AbstractionsAndPersistency
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.PersistenceLayer;
    using OOAdvantech.Transactions;
    using OOAdvantech.Collections.Generic;
    using OOAdvantech.Synchronization;
    using System.Xml;
    using OOAdvantech.Remoting;
    using System;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{603DD24C-59D4-4824-AF3F-BE6FB70EB768}</MetaDataID>
    [BackwardCompatibilityID("{603DD24C-59D4-4824-AF3F-BE6FB70EB768}")]
    [Persistent()]
    public class Product : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IProduct
    {

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1B4120B1-21EC-4032-9F30-815C219298C8}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<IProduct> _Compositions;
        /// <MetaDataID>{C8F7474B-B6F7-444D-9277-1C30E0F02DA0}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        [PersistentMember("_Compositions")]
        public OOAdvantech.Collections.Generic.Set<IProduct> Compositions
        {
            get
            {
                return new Set<IProduct>(_Compositions);
            }
        }
        /// <MetaDataID>{71c0c1b9-ed1f-4b30-a799-97553bfef06e}</MetaDataID>
        //public int[] ArrayTest = new int[4] { 1, 2, 3, 4 };
        /// <MetaDataID>{a4fbf4d6-9331-47f5-a33a-d2fc4103d212}</MetaDataID>
        public void RemovePart(IProduct part)
        {

            if (part != null && _Parts.Contains(part))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _Parts.Remove(part);
                    objStateTransition.Consistent = true;
                }
            }

        }
        /// <MetaDataID>{eaf3b677-dd65-4fb6-9f62-8efa3f687f90}</MetaDataID>
        public void AddPart(IProduct part)
        {
            if (part != null && !_Parts.Contains(part))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _Parts.Add(part);
                    objStateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1BD52DA2-EC58-4037-AB6D-43ED6B660ADA}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<IProduct> _Parts;
        /// <MetaDataID>{E0C66A2A-35D5-4695-A331-5077748E30E9}</MetaDataID>
        [BackwardCompatibilityID("+15")]
        [PersistentMember("_Parts")]
        public OOAdvantech.Collections.Generic.Set<IProduct> Parts
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<IProduct>(_Parts);
            }
        }
        /// <MetaDataID>{E6962EC2-CA69-4C0D-9DC7-FB283F1BC4C7}</MetaDataID>
        public virtual void AddStorePlace(IStorePlace storePlace)
        {
            if (storePlace != null && !_StorePlaces.Contains(storePlace))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _StorePlaces.Add(storePlace);
                    objStateTransition.Consistent = true;
                }
            }


        }
        /// <MetaDataID>{8D8320AE-6385-4CAD-B206-0FBA858AA6EA}</MetaDataID>
        public virtual void RemoveStorePlace(IStorePlace storePlace)
        {
            if (storePlace != null && _StorePlaces.Contains(storePlace))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _StorePlaces.Remove(storePlace);
                    objStateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2CD13DDA-28FD-4B47-BFAD-54A138B4E210}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<IStorePlace> _StorePlaces = new Set<IStorePlace>();
        /// <MetaDataID>{CEA83DF3-11C2-4323-9F5A-4ECFEBFCA81C}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        [PersistentMember("_StorePlaces")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
        public OOAdvantech.Collections.Generic.Set<IStorePlace> StorePlaces
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<IStorePlace>(_StorePlaces, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double _Vat = 0.180;
        /// <MetaDataID>{8E8BE22A-C236-4DB2-AD8F-C1E7ED169DE1}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [PersistentMember("_Vat")]
        public double Vat
        {
            get
            {
                return _Vat;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Vat = value;
                    stateTransition.Consistent = true;
                }
            }
        }
#if !DeviceDotNet
        /// <exclude>Excluded</exclude>
        System.Drawing.Image _Image;
        /// <MetaDataID>{70cef4be-1b2e-4c2a-b93a-7486eae5fd0d}</MetaDataID>
        [BackwardCompatibilityID("+19")]
        [PersistentMember("_Image")]
        public System.Drawing.Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (_Image != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Image = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

#endif
        /// <MetaDataID>{9DC117AF-A969-4DFB-8483-09739ACBB175}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember()]
        public decimal Amount;




        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F10CC200-FEE9-4FEE-BF19-1BD2128D385C}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<IUnitMeasure> _UnitMeasures = new Set<IUnitMeasure>();
        /// <MetaDataID>{036FC3E4-2CB8-47F7-A33E-9C75BAC1212E}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_UnitMeasures")]
        public OOAdvantech.Collections.Generic.Set<IUnitMeasure> UnitMeasures
        {
            get
            {
                return new Set<IUnitMeasure>(_UnitMeasures);
            }


        }
        /// <MetaDataID>{046A4EAE-204A-4B11-9698-2D11DD20A793}</MetaDataID>
        public void RemoveUnitMeasure(IUnitMeasure unitMeasure)
        {
            if (unitMeasure != null && _UnitMeasures.Contains(unitMeasure))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _UnitMeasures.Remove(unitMeasure);
                    objStateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{D26EB765-49BC-41E5-A397-11EEDB8D5FC0}</MetaDataID>
        public void AddUnitMeasure(IUnitMeasure unitMeasure)
        {
            if (unitMeasure != null && !_UnitMeasures.Contains(unitMeasure))
            {
                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this))
                {
                    _UnitMeasures.Add(unitMeasure);
                    objStateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{A9DE3880-F10A-4BFC-BC3F-642D29549049}</MetaDataID>
        protected Product()
        {
            Quantity quantity = new Quantity(12, null);
            _Quantity = new OOAdvantech.MemberAcount<Quantity>(quantity);

        }
        /// <MetaDataID>{697FEB40-B480-4734-BDB6-E7C9DCC55E96}</MetaDataID>
        public Product(string name)
        {

            Quantity quantity = new Quantity(12, null);
            _Quantity = new OOAdvantech.MemberAcount<Quantity>(quantity);
            _Name = name;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MemberAcount<Quantity> _Quantity;
        /// <MetaDataID>{b021050b-16a1-483c-8ee9-95947355172d}</MetaDataID>
        [BackwardCompatibilityID("+17")]
        [PersistentMember("_Quantity")]
        [TransactionalMember(LockOptions.Shared, "_Quantity")]
        public Quantity Quantity
        {
            get
            {
                //if (_Quantity.UnitMeasure == null)
                //{
                //    OOAdvantech.PersistenceLayer.StorageInstanceRef storageInstanceRef = OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(this);
                //    if (storageInstanceRef != null)
                //        storageInstanceRef.LazyFetching("_Quantity.UnitMeasure", GetType());
                //}
                return _Quantity;
            }
            set
            {
                //if (_Quantity != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "Quantity"))
                    {
                        _Quantity.Value = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Quantity _MinimumQuantity;
        /// <MetaDataID>{cb16ea6d-ee32-4684-82f8-d4c284f37d48}</MetaDataID>
        [BackwardCompatibilityID("+18")]
        [PersistentMember("_MinimumQuantity")]
        [TransactionalMember(LockOptions.Exclusive, "_MinimumQuantity")]
        public Quantity MinimumQuantity
        {
            get
            {
                return _MinimumQuantity;
            }
            set
            {
                //if (_MinimumQuantity != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "MinimumQuantity"))
                    {
                        _MinimumQuantity = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B82BBBBE-8A79-43F3-8ADB-50B660B09AD1}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<IProductPrice> _PriceLists = new Set<IProductPrice>();
        /// <MetaDataID>{216A3F4F-195A-475E-86E1-D37BD6629D6A}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_PriceLists")]
        public OOAdvantech.Collections.Generic.Set<IProductPrice> PriceLists
        {
            get
            {
                return new Set<IProductPrice>(_PriceLists);
            }
        }
        /// <exclude>Excluded</exclude>
        private OOAdvantech.ObjectStateManagerLink Properties = new OOAdvantech.ObjectStateManagerLink();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E519DA91-7E0C-4277-A64D-E309EAC46ECE}</MetaDataID>
        private string _Name;
        /// <MetaDataID>{DA222FE6-85FD-487E-B086-91AA4AA419A3}</MetaDataID>
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
                if (_Name != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "Name"))
                    {
                        _Name = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }
        //XmlDocument _Errors;
        //[PersistentMember("_Errors")]
        //[BackwardCompatibilityID("+40")]
        //public XmlDocument Errors
        //{
        //    get
        //    {
        //        return _Errors;
        //    }
        //    set
        //    {
        //        _Errors = value;
        //    }
        //}
    }
}
