namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{3E6A16E4-EFEB-4318-883D-E10BAA2B2637}</MetaDataID>
    public class DerivedTemplateParameter
    {
        
        /// <MetaDataID>{F7252D1A-B5E4-4E0C-8996-4F823A3AC96C}</MetaDataID>
        [Association("TemplateParameterSubstitution",typeof(OOAdvantech.MetaDataRepository.TemplateParameter),Roles.RoleA,"{6830BF82-F7FE-4920-8C8A-132972FC6E50}")]
        [RoleAMultiplicityRange(1,1)]
        [RoleBMultiplicityRange(0)]
        public TemplateParameter OriginalTemplateParamter;
    }
}
