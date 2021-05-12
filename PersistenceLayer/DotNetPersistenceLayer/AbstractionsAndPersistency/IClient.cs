namespace AbstractionsAndPersistency
{
	using OOAdvantech.MetaDataRepository;
	using OOAdvantech.Transactions;
    using OOAdvantech.Collections.Generic;

    /// <MetaDataID>{5901BE12-3447-4586-A73E-9B52B2B9DFF1}</MetaDataID>
    [BackwardCompatibilityID("{5901BE12-3447-4586-A73E-9B52B2B9DFF1}")]
    public interface IClient
    {

        /// <MetaDataID>{e1789596-2b0e-4d1b-bace-e6212c38debc}</MetaDataID>
        System.Collections.Generic.List<IOrder> GetOrders(System.DateTime fromDate, System.DateTime toDate);
        /// <MetaDataID>{384aba94-94e5-40fa-98f7-d5e8b940508d}</MetaDataID>
        Set<AbstractionsAndPersistency.IPriceList> GetPriceLists();
        /// <MetaDataID>{6d225b82-05c8-4f77-ad1f-71e0bf1030b0}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IPriceList> PriceLists
        {
            get;
        }
        ///// <MetaDataID>{384aba94-94e5-40fa-98f7-d5e8b940508d}</MetaDataID>
        //OOAdvantech.Collections.Generic.Set<AbstractionsAndPersistency.IPriceList> GetPriceLists();


        /// <MetaDataID>{99315096-50ba-423e-a4d3-9c1f1738dde2}</MetaDataID>
        [Association("ClientOrders", Roles.RoleA, "{7F7E54C7-CB3F-4159-8927-57724CBCE08D}"), RoleAMultiplicityRange(0)]
        Set<AbstractionsAndPersistency.IOrder> Orders
        {
            
            get;
        }
        
        /// <MetaDataID>{F4DA0E1A-437D-463B-AA46-CE32963F2A09}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name
        {
            get;
            set;
        }
        /// <MetaDataID>{5568a257-eff1-49eb-a952-3460a9dfe425}</MetaDataID>
        void RemoveOrder(IOrder order);

        /// <MetaDataID>{f49a106d-cb57-491a-8a60-b26f35294745}</MetaDataID>
        void AddOrder(AbstractionsAndPersistency.IOrder order, int index);

        /// <MetaDataID>{85bbcd67-0f7c-4d60-a773-05da7c4279b9}</MetaDataID>
        string GetUserName();
    }
}
