namespace DemoUMLModel
{
    /// <MetaDataID>9b9b98ed-19b2-4422-a9f2-9a3b782e2b70</MetaDataID>
    public struct Quantity : DemoUMLModel.IQuantity 
    {
        /// <MetaDataID>3edb4c3d-8649-4ae6-bc19-32cf6ea4252a</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(""), OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return default(string);
            }
            set
            {
            }
        }
     

        /// <MetaDataID>3fbeb8d3-de6c-4990-b90f-bf4bb571c8be</MetaDataID>
        public void SetUnit()
        {

        }
    }
}
