using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using OOAdvantech.Transactions;
using System.Linq;
using System;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


namespace AbstractionsAndPersistency
{
	/// <MetaDataID>{F0260009-6CE7-475A-B01C-3AF911806C4B}</MetaDataID>
	[BackwardCompatibilityID("{F0260009-6CE7-475A-B01C-3AF911806C4B}")]
	[Persistent()]
	public class OrderDetail:MarshalByRefObject,  OOAdvantech.Remoting.IExtMarshalByRefObject,IOrderDetail
	{
        /// <MetaDataID>{1d21dd5b-2973-48e9-ac0e-d9074a9eee1d}</MetaDataID>
        IClient _Client;
        /// <MetaDataID>{32fa7105-8847-4249-b8e1-7eaea0651d04}</MetaDataID>
        public IClient Client
        {
            get
            { 
                if (_Client == null)
                    _Client = new Client("Stavros");
                return _Client;

            }
        }

        /// <MetaDataID>{c5025e02-f77d-48f8-913e-be7c98690945}</MetaDataID>
        public AbstractionsAndPersistency.IProduct Product
        {
            get
            {
                return null;
            }
        }
        //const System.Linq.IQueryable<IProduct> ProductsQuerys = ProductsQuery;
        //public static System.Linq.IQueryable<IProduct> ProductsQuery
        //{
        //    get
        //    {

        //        return from orderDetail in OOAdvantech.Linq.Storage.GetObjectCollectionDef<IOrderDetail>()
        //               where orderDetail.Price.Name == "Lola"
        //               select orderDetail.Price.Product;
        //    }
        //}

        

        /// <exclude>Excluded</exclude>
        private Quantity _Quantity;

        /// <MetaDataID>{59184ac8-d7d7-4e2d-94a7-b3594047d795}</MetaDataID>
        [PersistentMember("_Quantity")]
        [BackwardCompatibilityID("+5")]
        [TransactionalMember(LockOptions.Exclusive,"_Quantity")]
        public AbstractionsAndPersistency.Quantity Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                if (_Quantity != value)
                {
                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "Quantity"))
                    {
                        _Quantity = value;
                        objStateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        private SupperQuantity _SQuantity;

        /// <MetaDataID>{59184ac8-d7d7-4e2d-94a7-b3594047d795}</MetaDataID>
        [PersistentMember("_SQuantity")]
        [BackwardCompatibilityID("+6")]
        [TransactionalMember(LockOptions.Exclusive, "_SQuantity")]
        public SupperQuantity SQuantity
        {
            get
            {
                return _SQuantity;
            }
            set
            {

                using (ObjectStateTransition objStateTransition = new ObjectStateTransition(this, "SQuantity"))
                {
                    _SQuantity = value;
                    objStateTransition.Consistent = true;
                }

            }
        }





        /// <MetaDataID>{c880342c-78bb-45ae-aa03-f1ef932a7233}</MetaDataID>
        public decimal UnitPrice
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        /// <MetaDataID>{714f2136-8b95-485d-bdf0-61768c4dabc8}</MetaDataID>
        public OrderDetail()
        {
           

        }
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{03F63A59-1610-4C59-BCFB-15DF3D84E5F6}</MetaDataID>
		private IProductPrice _Price;
		/// <MetaDataID>{4058FF9A-FFF7-4681-B1AB-B7D002D1945C}</MetaDataID>
		[BackwardCompatibilityID("+3")]
		[PersistentMember("_Price")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.ReferentialIntegrity)]
		public IProductPrice Price
		{
			get
			{
				
				return _Price;
			}
			set
			{
				if(_Price!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Price=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{3121BA97-A9EF-40FF-9B40-C5EA3FBD1270}</MetaDataID>
		private IOrder _Order;
		/// <MetaDataID>{A2A16227-7CBA-4520-94AB-73F78AD4030F}</MetaDataID>
//		[PersistentMember("_Order")]
		[BackwardCompatibilityID("+2")]
		[PersistentMember("_Order")]
		public IOrder Order
		{
			get
			{
				
				return _Order;
			}
			set
			{
				if(_Order!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Order=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}
	
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{C826C96B-2200-4464-959C-381F67CB1A03}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{788BC057-E7DF-4763-80A1-184A03DBE17B}</MetaDataID>
//		[PersistentMember("_Name")]
		[BackwardCompatibilityID("+1")]
		[PersistentMember(255,"_Name")]
        [TransactionalMember(LockOptions.Exclusive, "_Name")]
        public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
                
				if(_Name!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this,"Name"))
					{
						_Name=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}
  
		#region IOrderDetail Members
		/// <MetaDataID>{99D9E6B1-60AC-4277-A81C-1AC55E7753EB}</MetaDataID>
		private OOAdvantech.ObjectStateManagerLink Properties=new OOAdvantech.ObjectStateManagerLink();

	
		#endregion

        #region IOrderDetail Members


        /// <MetaDataID>{e4ad6c50-c45c-4e0f-8231-37e72f88db24}</MetaDataID>
        double _Amount;
        /// <MetaDataID>{3e0c124f-514d-482a-89ca-ccd20d504fdb}</MetaDataID>
        public double Amount
        {
            get
            {
                return _Amount;// (double)decimal.Round((decimal)_Amount,2);
                
            }
            set
            {
                _Amount = value;
            }
        }

        /// <MetaDataID>{f8de8d07-f17b-43b2-a2a6-b3cb9dbd6a7a}</MetaDataID>
        int _Number;
        /// <MetaDataID>{ff059507-ac42-4777-917b-fcf4e8d30463}</MetaDataID>
        public int Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = value;
            }
        }

        #endregion
    }


}
