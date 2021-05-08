namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{5449AC3E-D83F-4561-8B35-EE2C76809260}</MetaDataID>
    /// <summary>A TemplateableElement that has a template signature is a specification of a template. A template is a parameterized
    /// element that can be used to generate other model elements using TemplateBinding relationships. The template parameters
    /// for the template signature specify the formal parameters that will be substituted by actual parameters (or the default) in a
    /// binding. </summary>
    public interface ITemplateable
    {
        /// <summary>The optional bindings from this element to templates. </summary>
        /// <MetaDataID>{C75CD513-936D-4C51-8908-7BF3121305C1}</MetaDataID>
        [Association("BindTamplate",typeof(OOAdvantech.MetaDataRepository.TemplateBinding),Roles.RoleB,"{0C4399F4-36AF-45E4-AF09-41C6A7CA4EFB}")]
        [RoleBMultiplicityRange(0,1)]
        TemplateBinding TemplateBinding
        {
            get;
            set;
        }



        /// <summary>The optional template signature specifying the formal template parameters. </summary>
        /// <MetaDataID>{3CE2F221-CC1A-4B0E-B687-C24728039209}</MetaDataID>
        [Association("TamplateDefinition",typeof(OOAdvantech.MetaDataRepository.TemplateSignature),Roles.RoleA,"{660B2569-F0CD-4CA2-8241-CFD1DDBB02F0}")]
        [RoleAMultiplicityRange(0,1)]
        TemplateSignature OwnedTemplateSignature
        {
            get;
            set;
        }

    }
}
