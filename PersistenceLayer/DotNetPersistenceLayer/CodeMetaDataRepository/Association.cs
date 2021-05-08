namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{E6A62EEA-C0C5-4A89-9877-6A56ECC524F9}</MetaDataID>
    public class Association : OOAdvantech.MetaDataRepository.Association
    {
        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, originMetaObject);
                return;
            }
            base.Synchronize(originMetaObject);
        }
        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                SetIdentity(new OOAdvantech.MetaDataRepository.MetaObjectID(PropertyValue as string));
            }
            else
            { 
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
        }

        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                return Identity.ToString();
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }
        /// <MetaDataID>{f4780951-7135-4837-88af-99fd37d8ad45}</MetaDataID>
        public Association(string name, MetaDataRepository.AssociationEnd roleA, MetaDataRepository.AssociationEnd roleB, string identity, MetaDataRepository.Association generalAssociation)
            : base(name, roleA, roleB, identity)
        {
            if (roleA is AssociationEnd && !string.IsNullOrEmpty((roleA as AssociationEnd).AssociationIdentity))
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID((roleA as AssociationEnd).AssociationIdentity);

            if (roleB is AssociationEnd && !string.IsNullOrEmpty((roleB as AssociationEnd).AssociationIdentity))
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID((roleB as AssociationEnd).AssociationIdentity);
            if( generalAssociation!=null&&
                roleA.Specification.IsA(generalAssociation.RoleA.Specification)&&
                roleB.Specification.IsA(generalAssociation.RoleB.Specification))
            _General.Value = generalAssociation;
        }

        /// <MetaDataID>{26c71bd9-49b0-4423-9a43-2002a6fd372a}</MetaDataID>
        public Association()
        {

        }

        public void ReplaceRoleA(MetaDataRepository.AssociationEnd associationEnd)
        {
            if(RoleA!=null)
                Connections.Remove(_RoleA);
            Connections.Add(associationEnd);
            _RoleA = associationEnd;
        }
        public void ReplaceRoleB(MetaDataRepository.AssociationEnd associationEnd)
        {
            if (RoleB != null)
                Connections.Remove(_RoleB);
            Connections.Add(associationEnd);
            _RoleB = associationEnd;
        }

    
    }
}
