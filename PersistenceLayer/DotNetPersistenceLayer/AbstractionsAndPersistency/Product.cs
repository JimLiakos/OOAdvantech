namespace AbstractionsAndPersistency
{
    using OOAdvantech.MetaDataRepository;
#if !DeviceDotNet
    using System.Drawing;
#endif
    /// <MetaDataID>{4E7F6D47-4457-4FFC-A824-40E93C0150D0}</MetaDataID>
    [BackwardCompatibilityID("{4E7F6D47-4457-4FFC-A824-40E93C0150D0}")]
    public interface IProduct
    {


#if !DeviceDotNet
        /// <MetaDataID>{282e6c7c-5ee1-465b-953a-2a745a76b4bf}</MetaDataID>
        [BackwardCompatibilityID("5")]
        Image Image
        {
            get;
            set;
        }
#endif


        /// <MetaDataID>{b25a1870-ec3c-4494-a86e-4d45c9f2d4c3}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        double Vat
        {
            get;
            set;
        }

     

        /// <MetaDataID>{05F08AC7-B267-4871-A38A-E8A536668691}</MetaDataID>
        [Association("Assembly", Roles.RoleB, "{B5235D32-11B4-4B80-8A3D-C53EDBABB2CF}")]
        [RoleBMultiplicityRange(0)]

        OOAdvantech.Collections.Generic.Set<IProduct> Compositions
        {
            get;
        }
        /// <MetaDataID>{76F34327-D06E-4EC7-99F1-21CBA4C1EEF7}</MetaDataID>
        [Association("Assembly", Roles.RoleA, "{B5235D32-11B4-4B80-8A3D-C53EDBABB2CF}")]
        [RoleAMultiplicityRange(0)]
        OOAdvantech.Collections.Generic.Set<IProduct> Parts
        {
            get;
        }
        /// <MetaDataID>{1ADA5BF8-8296-4B7C-B399-CC9F792F7420}</MetaDataID>
        void RemovePart(IProduct part);
        /// <MetaDataID>{AA6140E9-7374-4820-BD4C-225DE215A1F9}</MetaDataID>
        void AddPart(IProduct part);
        /// <MetaDataID>{23676E40-475D-4A4A-9968-55F357E3E982}</MetaDataID>
        [Association("ProductsInStore",Roles.RoleA, "{932F5707-C060-4CB9-86C9-D7E9296813BE}")]
        [RoleAMultiplicityRange(0)]
        OOAdvantech.Collections.Generic.Set<IStorePlace> StorePlaces
        {
            get;
        }
        /// <MetaDataID>{6D0728EB-03EF-4A40-BA2F-5B8D84A68495}</MetaDataID>
        void AddStorePlace(IStorePlace storePlace);
        /// <MetaDataID>{B5154BDC-A8EB-4B4C-9F8B-105DED5F485A}</MetaDataID>
        void RemoveStorePlace(IStorePlace storePlace);

        /// <MetaDataID>{85BE00F3-54AA-409E-AB29-E11A6CD3C674}</MetaDataID>
        [Association("ProductUnitMeasures", Roles.RoleA, "{E6D135EE-E873-4837-B354-F7BB520C3E0A}")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        OOAdvantech.Collections.Generic.Set<IUnitMeasure> UnitMeasures
        {
            get;


        }

        /// <MetaDataID>{02B3A0C1-E975-4A9F-B5A1-73E272E38629}</MetaDataID>
        void AddUnitMeasure(IUnitMeasure unitMeasure);
        /// <MetaDataID>{EE14A682-553B-471F-8161-59EDC91F9842}</MetaDataID>
        void RemoveUnitMeasure(IUnitMeasure unitMeasure);

        /// <MetaDataID>{12E26650-16E4-41A4-9EF9-2E7BED7A53EE}</MetaDataID>
        string Name
        {
            get;
            set;
        }

        /// <MetaDataID>{9474f13c-5cae-48f1-9e98-a79f56611c60}</MetaDataID>
        [BackwardCompatibilityID("+18")]
        Quantity MinimumQuantity
        {
            get;
            set;
        }


        /// <MetaDataID>{0c90c805-dd40-4308-823f-e78016a0919f}</MetaDataID>
        [BackwardCompatibilityID("+17")]
        Quantity Quantity
        {
            get;
            set;
        }

        /// <MetaDataID>{24E34D01-8671-4223-A8FA-FBE8A69C9472}</MetaDataID>
        [Association("ProductPrice", typeof(IPriceList), Roles.RoleB, "{82EF20D6-8AF9-494E-B661-0384D66A7F27}")]
        [RoleBMultiplicityRange(1)]
        [AssociationClass(typeof(IProductPrice))]
        [BackwardCompatibilityID("+1")]
        OOAdvantech.Collections.Generic.Set<IProductPrice> PriceLists
        {
            get;
        }
    }
}
