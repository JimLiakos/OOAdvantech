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

            if (ObjectStorage.StorageIdentity != RoleA.StorageIdentity || ObjectStorage.StorageIdentity != RoleB.StorageIdentity)
            {

            }

            if (RoleA.RealStorageInstanceRef.PersistentObjectID != null && RoleB.RealStorageInstanceRef.PersistentObjectID != null && (RoleB.RealStorageInstanceRef.PersistentObjectID.ToString() == "2022" || RoleA.RealStorageInstanceRef.PersistentObjectID.ToString() == "2022"))
            {

            }

            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            #endregion
            base.Execute();
            XElement RoleAStorageInstance = null;
            if (RoleA.StorageIdentity == this.ObjectStorage.StorageIdentity)
            {
                MetaDataStorageSession ObjectStorageSession = (MetaDataStorageSession)RoleA.ObjectStorage;
                ObjectStorageSession.Dirty = true;

                XDocument XmlDocument = ObjectStorageSession.XMLDocument;
                string StorageName = XmlDocument.Root.Name.LocalName;

                string XQuery = null;

                if (RoleA.PersistentObjectID != null)
                {
                    RoleAStorageInstance = ObjectStorageSession.GetXMLElement(RoleA.MemoryInstance.GetType(), (ObjectID)RoleA.PersistentObjectID);
                    ObjectStorageSession.NodeChangedUnderTransaction(RoleAStorageInstance, this.OwnerTransactiont);
                    RoleAStorageInstance.SetAttribute("ReferentialIntegrityCount", RoleA.RealStorageInstanceRef.ReferentialIntegrityCount.ToString());
                }
            }
            XElement RoleBStorageInstance = null;
            if (RoleB.StorageIdentity == this.ObjectStorage.StorageIdentity)
            {
                MetaDataStorageSession ObjectStorageSession = (MetaDataStorageSession)RoleB.ObjectStorage;
                XDocument XmlDocument = ObjectStorageSession.XMLDocument;
                string StorageName = XmlDocument.Root.Name.LocalName;



                if (RoleB.PersistentObjectID != null)
                {
                    RoleBStorageInstance = ObjectStorageSession.GetXMLElement(RoleB.MemoryInstance.GetType(), (ObjectID)RoleB.PersistentObjectID);//(XElement)ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);
                    ObjectStorageSession.NodeChangedUnderTransaction(RoleBStorageInstance, this.OwnerTransactiont);
                    RoleBStorageInstance.SetAttribute("ReferentialIntegrityCount", RoleB.RealStorageInstanceRef.ReferentialIntegrityCount.ToString());
                }
            }
            string StrObjectID = null;
            XElement RoleBCollection = null;
            if (RoleB.PersistentObjectID != null && RoleAStorageInstance != null)
            {
                StrObjectID = RoleB.PersistentObjectID.ToString();
                MetaDataStorageSession ObjectStorageSession = ObjectStorage as MetaDataStorageSession;

                string RoleBName = ObjectStorageSession.GetMappedTagName(LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString().ToLower());
                if (RoleBName == null)
                {
                    RoleBName = LinkInitiatorAssociationEnd.Association.RoleB.Name;
                    if (string.IsNullOrEmpty(RoleBName))
                        RoleBName = LinkInitiatorAssociationEnd.Association.Name + "RoleBName";
                    ObjectStorageSession.SetMappedTagName(LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString().ToLower(), RoleBName);
                }

                foreach (XElement CurrNode in RoleAStorageInstance.Elements())
                {
                    XElement Element = (XElement)CurrNode;

                    //string RoleBName=LinkInitiatorAssociationEnd.Association.RoleB.Name;
                    //if(RoleBName==null||RoleBName.Trim().Length==0)
                    //    RoleBName=LinkInitiatorAssociationEnd.Association.Name+"RoleBName";

                    if (Element.Name == RoleBName)
                    {



                        int index = -1;
                        RoleBCollection = Element;
                        if (Multilingual&& Culture != null)
                            RoleBCollection = RoleBCollection.Element(Culture.Name);

                        foreach (XElement inCurrNode in RoleBCollection.Elements())
                        {

                            XElement inElement = (XElement)inCurrNode;


                            if (inElement.Value == StrObjectID)
                            {
                                if (LinkInitiatorAssociationEnd.Association.RoleB.Indexer)
                                {
                                    if (!string.IsNullOrEmpty(inElement.GetAttribute("Sort")))
                                        int.TryParse(inElement.GetAttribute("Sort"), out index);

                                }
                                inElement.Remove();


                                break;
                            }

                        }
                        //if (LinkInitiatorAssociationEnd.Association.RoleB.Indexer)
                        //{
                        //    if (index != -1)
                        //    {
                        //        foreach (XElement inCurrNode in RoleBCollection.Elements())
                        //        {
                        //            int sort = 0;
                        //            XElement inElement = (XElement)inCurrNode;
                        //            if (!string.IsNullOrEmpty(inElement.GetAttribute("Sort")))
                        //            {
                        //                int.TryParse(inElement.GetAttribute("Sort"), out sort);
                        //                if (sort > index)
                        //                    inElement.SetAttribute(("Sort"), ((int)sort - 1).ToString());
                        //            }
                        //        }
                        //    }
                        //}

                        break;
                    }
                }
            }
            XElement RoleACollection = null;
            if (RoleA.PersistentObjectID != null && RoleBStorageInstance != null)
            {
                MetaDataStorageSession ObjectStorageSession = ObjectStorage as MetaDataStorageSession;

                StrObjectID = RoleA.PersistentObjectID.ToString();

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


                foreach (XElement element in RoleBStorageInstance.Elements())
                {

                    //if(Element.Name==mResolver.RoleAName)
                    //string RoleAName=LinkInitiatorAssociationEnd.Association.RoleA.Name;
                    //if(RoleAName==null||RoleAName.Trim().Length==0)
                    //    RoleAName=LinkInitiatorAssociationEnd.Association.Name+"RoleAName";
                    if (element.Name == roleAName)
                    {
                        int index = -1;
                        RoleACollection = element;

                        if (Multilingual && Culture != null)
                            RoleACollection = RoleACollection.Element(Culture.Name);

                        foreach (XElement inCurrNode in RoleACollection.Elements())
                        {
                            XElement inElement = (XElement)inCurrNode;
                            //string Strvalue=inElement.GetAttribute("oid");
                            if (inElement.Value == StrObjectID)
                            //if(inElement.GetAttribute("oid")==StrObjectID)
                            {
                                if (LinkInitiatorAssociationEnd.Association.RoleA.Indexer)
                                {
                                    if (!string.IsNullOrEmpty(inElement.GetAttribute("Sort")))
                                        int.TryParse(inElement.GetAttribute("Sort"), out index);
                                }
                                inElement.Remove();
                                break;
                            }
                        }
                        //if (LinkInitiatorAssociationEnd.Association.RoleA.Indexer)
                        //{
                        //    if (index != -1)
                        //    {
                        //        foreach (XElement inCurrNode in RoleACollection.Elements())
                        //        {
                        //            int sort = 0;
                        //            XElement inElement = (XElement)inCurrNode;
                        //            if (!string.IsNullOrEmpty(inElement.GetAttribute("Sort")))
                        //            {
                        //                int.TryParse(inElement.GetAttribute("Sort"), out sort);
                        //                if (sort > index)
                        //                    inElement.SetAttribute(("Sort"), ((int)sort - 1).ToString());
                        //            }
                        //        }
                        //    }
                        //}
                        break;
                    }
                }
            }

        }
    }
}
