using OOAdvantech.MetaDataRepository;
namespace DemoUMLModel
{
    /// <MetaDataID>b1546892-6134-4c70-90c1-e49ea875b32d</MetaDataID>
    public class Client 
    {
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        [Association("ClientOrders", Roles.RoleA, "1e63d3c8-9cf1-4e92-937f-e623f6c036d5")]
        public System.Collections.Generic.List<Order> Orders;
      
      
       
        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>4151c510-c54c-4f64-901f-b26e0c0421c3</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(""), OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return "";
            }
            set
            {
            }
        }
      
  
   
    
        /// <MetaDataID>cf53565a-c5ce-4d78-a571-b7d15582a837</MetaDataID>
        public virtual void GetInvoicesFor(System.DateTime formDate, System.DateTime toDate)
        {

        }
    }
}
