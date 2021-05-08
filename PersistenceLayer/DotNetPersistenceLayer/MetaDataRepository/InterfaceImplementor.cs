namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{571A0363-B598-4BA5-AAA7-7830C12F0C70}</MetaDataID>
    public interface InterfaceImplementor
    {
        /// <MetaDataID>{E210EB92-DBB0-4767-ADE0-D09F1DC4C85E}</MetaDataID>
        [Association("ImplementorPart", typeof(OOAdvantech.MetaDataRepository.Realization), Roles.RoleB, "{B7175039-FF22-434D-93BA-94065C932934}")]
        [RoleBMultiplicityRange(0)]
        OOAdvantech.Collections.Generic.Set<Realization> Realizations
        {
            get;
        }
        /// <MetaDataID>{14b65769-9fa0-43a1-8470-2e1b6a1a57da}</MetaDataID>
        Method AddMethod(Operation specification);
        /// <MetaDataID>{db8df554-7dcf-4122-82bf-edcdf2200974}</MetaDataID>
        AttributeRealization AddAttributeRealization(Attribute specification);
        /// <MetaDataID>{82aae70f-2397-4f33-a887-8eddb9e426cb}</MetaDataID>
        AssociationEndRealization AddAssociationEndRealization(AssociationEnd specification);
        /// <MetaDataID>{f8aa1694-7390-44a3-a592-cb97d7117f2c}</MetaDataID>
        void RemoveMethod(Method method);
        /// <MetaDataID>{f6ff54ac-8e23-4d75-b2c9-5c7a8049033a}</MetaDataID>
        void RemoveAttributeRealization(AttributeRealization attributeRealization);
        /// <MetaDataID>{67f20c10-98de-4e04-be63-0ded8e73abd2}</MetaDataID>
        void RemoveAssociationEndRealization(AssociationEndRealization associationEndRealization);


        /// <MetaDataID>{AA896A4F-0261-4D02-BB1A-B499CEF9E072}</MetaDataID>
        /// <summary>This method retrieves the all realized interfaces of Class hierarchy. Note this method is recursive.  
        /// To retrieve only interfaces that realized from  this Class call the GetAllInterfaces. </summary>
        OOAdvantech.Collections.Generic.Set<Interface> GetAllInterfaces();
        /// <MetaDataID>{6A22514C-77AD-4E68-A09B-20B5AF4E54FC}</MetaDataID>
        /// <summary>This method retrieves only the interfaces that realized from  this Class. </summary>
        OOAdvantech.Collections.Generic.Set<Interface> GetInterfaces();
    }
}
