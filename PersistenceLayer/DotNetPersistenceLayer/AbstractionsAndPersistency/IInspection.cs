using OOAdvantech.MetaDataRepository;

namespace AbstractionsAndPersistency
{
    /// <MetaDataID>{24618e49-d65f-4995-ab02-15c4491ecd43}</MetaDataID>
    [BackwardCompatibilityID("{24618e49-d65f-4995-ab02-15c4491ecd43}")]
    public interface IInspection
    {
        [Association("InspectionOrder", Roles.RoleB, "dcf5a2fd-801b-4197-8d0c-1b86e3c19965")]
        [RoleBMultiplicityRange(1, 1)]
        IOrder Order
        {
            set;
            get; 
        }
        /// <MetaDataID>{d8ff9948-6ff5-49c6-aea5-575385119a81}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name
        {
            get;
            set;
        }
    }
}
