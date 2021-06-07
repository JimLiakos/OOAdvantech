using System.Xml.Linq;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{D4C6E3F3-0DC4-4E7A-AC6C-1FD61AC2E77A}</MetaDataID>
    public class MetaDataDeleteStorageInstanceCommand : PersistenceLayerRunTime.Commands.DeleteStorageInstanceCommand
    {
        /// <MetaDataID>{7D8BC8A9-91F0-4F4E-8919-4C4B2E3D53C7}</MetaDataID>
        public MetaDataDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceForDeletion, PersistenceLayer.DeleteOptions deleteOption) : base(storageInstanceForDeletion, deleteOption)
        {
        }
        /// <MetaDataID>{54D51C51-E84A-4AE6-B01A-685EAB31834F}</MetaDataID>
        public override void Execute()
        {

            try
            {
                base.Execute();
            }
            catch (System.Exception Error)
            {
                if (DeleteOption == PersistenceLayer.DeleteOptions.TryToDelete)
                    return;
                else
                    throw Error;
            }

            MetaDataStorageSession ObjectStorageSession =
                (MetaDataStorageSession)StorageInstanceForDeletion.ObjectStorage;
            ObjectStorageSession.Dirty = true;
            XElement mElement =
                ((MetaDataStorageSession)StorageInstanceForDeletion.ObjectStorage).
                GetXMLElement(StorageInstanceForDeletion.MemoryInstance.GetType(), (ObjectID)StorageInstanceForDeletion.PersistentObjectID);
            ((MetaDataStorageSession)StorageInstanceForDeletion.ObjectStorage).DeletedNodeUnderTransaction(mElement, OwnerTransactiont);
            ((MetaDataStorageSession)StorageInstanceForDeletion.ObjectStorage).DeleteStorageInstance((MetaDataStorageInstanceRef)StorageInstanceForDeletion);

            //Mitsos nick

            string StrObjectID = StorageInstanceForDeletion.PersistentObjectID.ToString();
            StorageInstanceForDeletion.PersistentObjectID = null;

            if (StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation != null && StorageInstanceForDeletion.Class.Persistent)
            {
                PersistenceLayerRunTime.StorageInstanceAgent roleA = null, roleB = null;
                GetRolesObject(StorageInstanceForDeletion.Class, StorageInstanceForDeletion, ref roleA, ref roleB);

                XElement RoleAStorageInstance = null;
                if (roleA != null && roleA.PersistentObjectID != null)
                {
                    RoleAStorageInstance = ObjectStorageSession.GetXMLElement(roleA.MemoryInstance.GetType(), (ObjectID)roleA.PersistentObjectID);
                    ObjectStorageSession.NodeChangedUnderTransaction(RoleAStorageInstance, this.OwnerTransactiont);
                }

                XElement RoleBStorageInstance = null;
                if (roleB != null && roleB.StorageIdentity == ObjectStorageSession.StorageMetaData.StorageIdentity)
                {
                    if (roleB.PersistentObjectID != null)
                    {
                        RoleBStorageInstance = ObjectStorageSession.GetXMLElement(roleB.MemoryInstance.GetType(), (ObjectID)roleB.PersistentObjectID);
                        ObjectStorageSession.NodeChangedUnderTransaction(RoleBStorageInstance, this.OwnerTransactiont);
                    }
                }

                if (RoleAStorageInstance != null)
                {
                    #region Remove role id 
                    string roleBName = ObjectStorageSession.GetMappedTagName(StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleB.Identity.ToString().ToLower());
                    if (roleBName == null)
                    {
                        roleBName = StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleB.Name;
                        if (roleBName == null)
                            roleBName = StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.Name + "RoleBName";
                        ObjectStorageSession.SetMappedTagName(StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleB.Identity.ToString().ToLower(), roleBName);
                    }

                    foreach (XElement Element in RoleAStorageInstance.Elements())
                    {
                        if (Element.Name == roleBName)
                        {
                            int index = -1;
                            var RoleBCollection = Element;
                            foreach (XElement inCurrNode in RoleBCollection.Elements())
                            {

                                XElement inElement = (XElement)inCurrNode;


                                if (inElement.Value == StrObjectID)
                                {
                                    if (StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleB.Indexer)
                                    {
                                        if (!string.IsNullOrEmpty(inElement.GetAttribute("Sort")))
                                            int.TryParse(inElement.GetAttribute("Sort"), out index);

                                    }
                                    inElement.Remove();
                                    break;
                                }
                            }
                            //if (StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleB.Indexer)
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
                        }
                    }
                    #endregion
                }
                if (RoleBStorageInstance != null)
                {
                    #region Remove role id 
                    #region gets role name with backward computability

                    string roleAName = ObjectStorageSession.GetMappedTagName(StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleA.Identity.ToString().ToLower());
                    if (string.IsNullOrWhiteSpace(roleAName))
                    {
                        roleAName = StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleA.Name;
                        if (string.IsNullOrWhiteSpace(roleAName))
                            roleAName = StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.Name + "RoleAName";
                        ObjectStorageSession.SetMappedTagName(StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleA.Identity.ToString().ToLower(), roleAName);
                    }
                    #endregion


                    foreach (XElement element in RoleBStorageInstance.Elements())
                    {

                        if (element.Name == roleAName)
                        {
                            int index = -1;
                            var RoleACollection = element;
                            foreach (XElement inCurrNode in RoleACollection.Elements())
                            {
                                XElement inElement = (XElement)inCurrNode;
                                if (inElement.Value == StrObjectID)
                                {
                                    if (StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleA.Indexer)
                                    {
                                        if (!string.IsNullOrEmpty(inElement.GetAttribute("Sort")))
                                            int.TryParse(inElement.GetAttribute("Sort"), out index);
                                    }
                                    inElement.Remove();
                                    break;
                                }
                            }
                            //if (StorageInstanceForDeletion.Class.ClassHierarchyLinkAssociation.RoleA.Indexer)
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
                    #endregion
                }
            }
        }
    }
}