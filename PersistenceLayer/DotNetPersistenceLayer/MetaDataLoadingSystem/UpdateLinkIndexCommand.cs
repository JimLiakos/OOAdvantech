using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{fd570674-6b58-4a4b-a8d2-5b0b9b75482f}</MetaDataID>
    public class UpdateLinkIndexCommand : PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand
    {
        private bool Multilingual;
        private CultureInfo Culture;

        public UpdateLinkIndexCommand(PersistenceLayerRunTime.ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index) :
            base(objectStorage, roleA, roleB, linkInitiatorAssociationEnd, index)
        {
            Multilingual = PersistenceLayerRunTime.Commands.LinkCommand.IsMultilingualLink(RoleA, RoleB, LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association);
            if (Multilingual)
                Culture = CultureContext.CurrentNeutralCultureInfo;
        }
        public override void GetSubCommands(int currentExecutionOrder)
        {

        }

        public override void Execute()
        {

            if (LinkInitiatorAssociationEnd.Indexer)
                UpdateRoleIndex(LinkInitiatorAssociationEnd);
            if (LinkInitiatorAssociationEnd.GetOtherEnd().Indexer)
                UpdateRoleIndex(LinkInitiatorAssociationEnd.GetOtherEnd());

            (ObjectStorage as MetaDataStorageSession).Dirty = true;

        }

        private void UpdateRoleIndex(MetaDataRepository.AssociationEnd associationEnd)
        {
            MetaDataStorageInstanceRef owner = null;
            MetaDataStorageInstanceRef reletedObject = null;
            int index = 0;
            if (associationEnd.IsRoleA)
            {
                owner = RoleB.RealStorageInstanceRef as MetaDataStorageInstanceRef;
                reletedObject = RoleA.RealStorageInstanceRef as MetaDataStorageInstanceRef;
                index = RoleAIndex;
            }
            else
            {
                owner = RoleA.RealStorageInstanceRef as MetaDataStorageInstanceRef;
                reletedObject = RoleB.RealStorageInstanceRef as MetaDataStorageInstanceRef;
                index = RoleBIndex;
            }

            var ownerStorageInstance = (owner.ObjectStorage as MetaDataStorageSession).GetXMLElement(owner.MemoryInstance.GetType(), (ObjectID)owner.PersistentObjectID);

            #region gets role name 


            var _roleName = (owner.ObjectStorage as MetaDataStorageSession).GetMappedTagName(associationEnd.Identity.ToString().ToLower());
            if (string.IsNullOrWhiteSpace(_roleName))
            {
                _roleName = associationEnd.GetOtherEnd().Name;
                if (string.IsNullOrWhiteSpace(_roleName))
                {
                    if (associationEnd.GetOtherEnd().IsRoleA)
                        _roleName = associationEnd.Association.Name + "RoleAName";
                    else
                        _roleName = associationEnd.Association.Name + "RoleBName";
                }

            }
            #endregion

            var objRefCollection = ownerStorageInstance.Element(_roleName);
            if (Multilingual)
                objRefCollection = objRefCollection.Element(Culture.Name);

            var reletedObjectRefElement = objRefCollection.Elements().Where(x => x.Value == reletedObject.PersistentObjectID.ToString()).FirstOrDefault();
            reletedObjectRefElement.SetAttribute("Sort", index.ToString());
        }
    }
}
