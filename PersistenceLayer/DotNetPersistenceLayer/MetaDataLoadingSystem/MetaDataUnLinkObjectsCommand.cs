using System.Linq;
using System.Xml.Linq;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{AA7EB01B-2C5A-483B-8DC8-45A3CF3C2E38}</MetaDataID>
    public class MetaDataUnLinkObjectsCommand : PersistenceLayerRunTime.Commands.UnLinkObjectsCommand
    {
        public MetaDataUnLinkObjectsCommand(PersistenceLayerRunTime.ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index) :
            base(objectStorage, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
        {
            Multilingual = IsMultilingualLink(RoleA, RoleB, LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association);
            if (Multilingual)
                Culture = CultureContext.CurrentNeutralCultureInfo;
        }


        /// <MetaDataID>{3E8908A1-C3AA-4F5D-9C57-98D0124E657A}</MetaDataID>
        public override void Execute()
        {
            #region Preconditions Check
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            #endregion
            base.Execute();
            XElement RoleAStorageInstance = null;
            if (RoleA.StorageIdentity == this.ObjectStorage.StorageIdentity)
            {
                MetaDataStorageSession ObjectStorageSession = (MetaDataStorageSession)RoleA.ObjectStorage;
                ObjectStorageSession.Dirty = true;

                if (RoleA.PersistentObjectID != null)
                {
                    RoleAStorageInstance = ObjectStorageSession.GetXMLElement(RoleA.MemoryInstance.GetType(), (ObjectID)RoleA.PersistentObjectID);
                    ObjectStorageSession.NodeChangedUnderTransaction(RoleAStorageInstance, this.OwnerTransactiont);
                    RoleAStorageInstance.SetAttribute("ReferentialIntegrityCount", RoleA.RealStorageInstanceRef.ReferentialIntegrityCount.ToString());
                }
            }
            XElement RoleBStorageInstance = null;
            if (RoleB.StorageIdentity == ObjectStorage.StorageIdentity)
            {
                MetaDataStorageSession ObjectStorageSession = (MetaDataStorageSession)RoleB.ObjectStorage;

                if (RoleB.PersistentObjectID != null)
                {
                    RoleBStorageInstance = ObjectStorageSession.GetXMLElement(RoleB.MemoryInstance.GetType(), (ObjectID)RoleB.PersistentObjectID);//(XElement)ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);
                    ObjectStorageSession.NodeChangedUnderTransaction(RoleBStorageInstance, OwnerTransactiont);
                    RoleBStorageInstance.SetAttribute("ReferentialIntegrityCount", RoleB.RealStorageInstanceRef.ReferentialIntegrityCount.ToString());
                }
            }
            string StrObjectID = null;
            XElement RoleBCollection = null;
            if (RoleB.PersistentObjectID != null && RoleAStorageInstance != null)
            {
                StrObjectID = RoleB.PersistentObjectID.ToString();
                MetaDataStorageSession ObjectStorageSession = ObjectStorage as MetaDataStorageSession;

                #region gets role name with backward computability
                string roleBName = ObjectStorageSession.GetMappedTagName(LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString().ToLower());
                if (string.IsNullOrWhiteSpace(roleBName))
                {
                    roleBName = LinkInitiatorAssociationEnd.Association.RoleB.Name;
                    if (string.IsNullOrEmpty(roleBName))
                        roleBName = LinkInitiatorAssociationEnd.Association.Name + "RoleBName";
                    ObjectStorageSession.SetMappedTagName(LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString().ToLower(), roleBName);
                }
                #endregion

                XElement element = RoleAStorageInstance.Elements().Where(x => x.Name == roleBName).FirstOrDefault();
                if (element != null)
                {
                    int index = -1;
                    RoleBCollection = element;
                    if (Multilingual && Culture != null)
                    {
                        RoleBCollection = RoleBCollection.Element(Culture.Name);
                        if (RoleBCollection == null)
                        {
                            System.Globalization.CultureInfo storageCulture = CultureContext.GetNeutralCultureInfo(ObjectStorage.StorageMetaData.Culture);
                            if (storageCulture != null && Culture == storageCulture)
                                RoleBCollection = element;
                        }
                    }
                    if (RoleBCollection != null)
                    {
                        XElement refElement = RoleBCollection.Elements().FirstOrDefault(x => x.Value == StrObjectID);
                        if (refElement != null)
                        {
                            if (LinkInitiatorAssociationEnd.Association.RoleB.Indexer)
                            {
                                if (!string.IsNullOrEmpty(refElement.GetAttribute("Sort")))
                                    int.TryParse(refElement.GetAttribute("Sort"), out index);
                            }
                            refElement.Remove();
                        }
                    }
                }
            }

            XElement RoleACollection = null;
            if (RoleA.PersistentObjectID != null && RoleBStorageInstance != null)
            {
                StrObjectID = RoleA.PersistentObjectID.ToString();
                MetaDataStorageSession ObjectStorageSession = ObjectStorage as MetaDataStorageSession;

                #region gets role name with backward computability
                string roleAName = ObjectStorageSession.GetMappedTagName(LinkInitiatorAssociationEnd.Association.RoleA.Identity.ToString().ToLower());
                if (string.IsNullOrWhiteSpace(roleAName))
                {
                    roleAName = LinkInitiatorAssociationEnd.Association.RoleA.Name;
                    if (string.IsNullOrWhiteSpace(roleAName))
                        roleAName = LinkInitiatorAssociationEnd.Association.Name + "RoleAName";
                    ObjectStorageSession.SetMappedTagName(LinkInitiatorAssociationEnd.Association.RoleA.Identity.ToString().ToLower(), roleAName);
                }
                #endregion

                XElement element = RoleBStorageInstance.Elements().Where(x => x.Name == roleAName).FirstOrDefault();
                if (element != null)
                {
                    int index = -1;
                    RoleACollection = element;
                    if (Multilingual && Culture != null)
                    {
                        RoleACollection = RoleACollection.Element(Culture.Name);
                        if (RoleACollection == null)
                        {
                            System.Globalization.CultureInfo storageCulture = CultureContext.GetNeutralCultureInfo(ObjectStorage.StorageMetaData.Culture);
                            if (storageCulture != null && Culture == storageCulture)
                                RoleACollection = element;
                        }
                    }
                    if (RoleACollection != null)
                    {
                        XElement refElement = RoleACollection.Elements().FirstOrDefault(x => x.Value == StrObjectID);
                        if (refElement != null)
                        {
                            if (LinkInitiatorAssociationEnd.Association.RoleA.Indexer)
                            {
                                if (!string.IsNullOrEmpty(refElement.GetAttribute("Sort")))
                                    int.TryParse(refElement.GetAttribute("Sort"), out index);
                            }
                            refElement.Remove();
                        }
                    }

                }
            }

        }
    }
}
