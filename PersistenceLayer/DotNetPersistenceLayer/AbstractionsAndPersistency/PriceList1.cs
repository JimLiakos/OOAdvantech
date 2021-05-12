namespace AbstractionsAndPersistency
{
    using OOAdvantech.PersistenceLayer;
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    using OOAdvantech.Collections.Generic;
    using OOAdvantech.Remoting;
    using System;

#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{CA0983F9-7E17-4F4D-96E7-935851C6C98B}</MetaDataID>
    [BackwardCompatibilityID("{CA0983F9-7E17-4F4D-96E7-935851C6C98B}")]
	[Persistent()]
	public class PriceList :MarshalByRefObject,  OOAdvantech.Remoting.IExtMarshalByRefObject, IPriceList
	{
		
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{5976697B-81E0-4D9F-8003-57BD76F8B82B}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<IProductPrice> _Products = new Set<IProductPrice>();
		/// <MetaDataID>{96F17C60-A816-4B16-8F68-9EFB63337B3B}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember("_Products")]
        public OOAdvantech.Collections.Generic.Set<IProductPrice> Products
		{
			get
			{
                return _Products;
                return new Set<IProductPrice>(_Products,OOAdvantech.Collections.CollectionAccessType.ReadOnly);
			}
		}
		/// <MetaDataID>{E2FD3693-AA17-4062-AE44-9D7384DF46CF}</MetaDataID>
		 protected PriceList()
		{

		}
		/// <MetaDataID>{E318918A-634B-4F80-A926-97F4E8321E5B}</MetaDataID>
		public PriceList(string name)
		{
			_Name=name;
		}

		/// <MetaDataID>{B32A7B73-1947-47E3-BED8-33C9AE40FE3F}</MetaDataID>
		private OOAdvantech.ObjectStateManagerLink Properties=new OOAdvantech.ObjectStateManagerLink();
		/// <MetaDataID>{54A11FE5-87D3-4D97-A774-2D29862C6965}</MetaDataID>
		public void AddProduct(IProductPrice productPrice)
		{
			if(productPrice==null)
				return;
		
			if(productPrice.Product==null)
				throw new System.Exception("the member Product of productPrice isn't set");

			using(ObjectStateTransition stateTransition=new ObjectStateTransition(this))
			{
				_Products.Add(productPrice);
				stateTransition.Consistent=true;
			}
		}



		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{EBFFE34D-9482-483C-AB18-08ED6DD8D482}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{13F8DD18-9B21-4D24-B567-E30B2A5BC6E9}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[PersistentMember(255,"_Name")]
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
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Name=value;
						objStateTransition.Consistent=true;
					}
				}
			}
		}
	}
}
