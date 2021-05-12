using OOAdvantech.MetaDataRepository;
using System.Linq;
namespace AbstractionsAndPersistency
{
	/// <MetaDataID>{63FE8DE4-E419-46DE-988E-BD6AEB7CBA75}</MetaDataID>
	[BackwardCompatibilityID("{498CB7C2-E5B4-4a0b-9180-F93A5984112E}")]
	public interface IOrderDetail
	{
        /// <MetaDataID>{0e2529ae-4f7c-418b-95a0-0a5cd4615c59}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        AbstractionsAndPersistency.Quantity Quantity
        {
            get;
            set; 
        }

        /// <MetaDataID>{46476dbb-4dc3-4964-8f9b-0a9c530dd5e2}</MetaDataID>
        [OOAdvantech.MetaDataRepository.DerivedMember(typeof( OrderDetailProductQuery))]
        AbstractionsAndPersistency.IProduct Product
        {
            get;
        }

		/// <MetaDataID>{E2DD5D00-7DD7-4560-AA74-30F71A3E3617}</MetaDataID>
        [Association("ItemPrice",  Roles.RoleA, "{8790E566-902B-414B-8EF2-4EEE5D6B05B7}")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        AbstractionsAndPersistency.IProductPrice Price
		{
			get;
			set;
		}
		/// <MetaDataID>{64092CDB-7195-4A85-8DC4-B5715B1100B4}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string Name
		{
			get;
			set;
		}

		/// <MetaDataID>{F222F968-B314-420D-85CA-9E1AFD09EEA9}</MetaDataID>
        [Association("OrderDetails", Roles.RoleB, "{BC804838-B915-48E2-998A-D482ED2D048A}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [RoleBMultiplicityRange(1, 1)]
		[BackwardCompatibilityID("+1")]
        AbstractionsAndPersistency.IOrder Order
		{
			get;
			set;
		}
        /// <MetaDataID>{db456eb8-cdb5-41f7-b430-b0a6fdc7ade2}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double Amount
        {
            get;
            set;
        }
        /// <MetaDataID>{24c38184-5c37-43b6-9f3a-98bc2c1d9dc4}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        int Number
        {
            get;
            set;
        }

        /// <MetaDataID>{734a75dd-5297-4e55-9088-a0412383cb52}</MetaDataID>
        decimal UnitPrice
        {
            get;
            set;
        }
        /// <MetaDataID>{b0d12d06-826f-44cc-a9e8-ef3441defc4b}</MetaDataID>
        IClient Client
        {
            get;
        }

	}


    /// <MetaDataID>{0d058d02-026e-4942-a1c6-b9103f56f493}</MetaDataID>
    class OrderDetailProductQuery :IDerivedMemberExpression<IOrder,IProduct> 
    {

        /// <MetaDataID>{e80770ef-e4a1-4d49-b0fd-409a02a08972}</MetaDataID>
        public System.Collections.Generic.IEnumerable<IProduct> GetCollection(IOrder order)
        {
            throw new System.NotImplementedException(); 
        }



        /// <MetaDataID>{1d32fa2f-53eb-4665-822f-85fe83d55c13}</MetaDataID>
        public IQueryable QueryableCollection
        {
            get 
            {    return from orderDetail in OOAdvantech.Linq.DerivedMembersExpresionBuilder<IOrderDetail>.ObjectCollection
                       //where orderDetail.Price.Product.Vat>0.19
                       select orderDetail.Price.Product;
            }
        }


        /// <MetaDataID>{cb7dc21a-a0ad-4791-9825-e20d5aaf6a78}</MetaDataID>
        public IProduct GetValue(IOrder _object)
        {
            throw new System.NotImplementedException();
        }

    }

}
