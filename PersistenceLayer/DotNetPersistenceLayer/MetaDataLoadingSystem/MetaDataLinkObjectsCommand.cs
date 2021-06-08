using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;
using System;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{4A6704F3-6EC9-44A6-BB24-F51D08EB801C}</MetaDataID>
    public class MetaDataLinkObjectsCommand : PersistenceLayerRunTime.Commands.LinkObjectsCommand
    {
        public MetaDataLinkObjectsCommand(PersistenceLayerRunTime.ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index) :
           base(objectStorage, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
        {
            if (RoleA.RealStorageInstanceRef.PersistentObjectID != null && RoleB.RealStorageInstanceRef.PersistentObjectID != null && (RoleB.RealStorageInstanceRef.PersistentObjectID.ToString() == "2022" || RoleA.RealStorageInstanceRef.PersistentObjectID.ToString() == "2022"))
            {

            }
            Multilingual = IsMultilingualLink(RoleA, RoleB, LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association);
            if (Multilingual)
                Culture = CultureContext.CurrentNeutralCultureInfo;
        }

        /// <MetaDataID>{B715C36B-9783-444B-BE2A-4E97406E1E19}</MetaDataID>
        public override void Execute()
        {

            if (RoleA.RealStorageInstanceRef.PersistentObjectID != null && RoleB.RealStorageInstanceRef.PersistentObjectID != null && (RoleB.RealStorageInstanceRef.PersistentObjectID.ToString() == "2022" || RoleA.RealStorageInstanceRef.PersistentObjectID.ToString() == "2022"))
            {

            }
            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            #endregion

            if (Multilingual)
            {
                using (CultureContext cultureContext = new CultureContext(Culture, false))
                {
                    base.Execute();
                    if (LinkInitiatorAssociationEnd.Association.LinkClass == null)
                        CreateObjectsLink();
                    else
                        CreateRelationStorageInstance();
                }
            }
            else
            {
                base.Execute();
                if (LinkInitiatorAssociationEnd.Association.LinkClass == null)
                    CreateObjectsLink();
                else
                    CreateRelationStorageInstance();
            }
        }
        void CreateObjectsLink()
        {
            bool multiligual = IsMultilingualLink(RoleA, RoleB, LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association);
            MetaDataStorageSession roleAObjectStorage = null;
            MetaDataStorageSession roleBObjectStorage = null;
            if (RoleA.ObjectStorage is MetaDataStorageSession)
                roleAObjectStorage = (MetaDataStorageSession)RoleA.ObjectStorage;

            if (RoleB.ObjectStorage is MetaDataStorageSession)
                roleBObjectStorage = (MetaDataStorageSession)RoleB.ObjectStorage;

            if (roleAObjectStorage != null)
                roleAObjectStorage.Dirty = true;

            string storageName = null;
            XElement RoleAStorageInstance = null;
            XElement RoleBStorageInstance = null;
            if (roleAObjectStorage != null)
            {
                XDocument XmlDocument = roleAObjectStorage.XMLDocument;
                storageName = XmlDocument.Root.Name.LocalName;
                RoleAStorageInstance = roleAObjectStorage.GetXMLElement(RoleA.MemoryInstance.GetType(), (ObjectID)RoleA.PersistentObjectID);
                roleAObjectStorage.NodeChangedUnderTransaction(RoleAStorageInstance, this.OwnerTransactiont);
            }

            if (roleBObjectStorage != null)
            {
                XDocument XmlDocument = roleBObjectStorage.XMLDocument;
                storageName = XmlDocument.Root.Name.LocalName;
                RoleBStorageInstance = roleBObjectStorage.GetXMLElement(RoleB.MemoryInstance.GetType(), (ObjectID)RoleB.PersistentObjectID);//(System.Xml.XmlElement)ObjectStorageSession.XMLDocument.SelectSingleNode(XQuery);
                roleBObjectStorage.NodeChangedUnderTransaction(RoleBStorageInstance, this.OwnerTransactiont);
            }


            MetaDataRepository.AssociationEnd associationEnd = LinkInitiatorAssociationEnd.Association.RoleB;
            MetaDataStorageSession ownerObjectStorage = roleAObjectStorage;
            PersistenceLayerRunTime.StorageInstanceAgent ownerObjectRef = RoleA;
            PersistenceLayerRunTime.StorageInstanceAgent relatedObjectRef = RoleB;
            int index = RoleBIndex;
            XElement ownerStorageInstance = RoleAStorageInstance;
            if (ownerObjectStorage != null)
                SaveObjectsLinkToxml(associationEnd, ownerObjectStorage, ownerObjectRef, ownerStorageInstance, relatedObjectRef, index);





            associationEnd = LinkInitiatorAssociationEnd.Association.RoleA;
            ownerObjectStorage = roleBObjectStorage;
            ownerObjectRef = RoleB;
            relatedObjectRef = RoleA;
            index = RoleAIndex;
            ownerStorageInstance = RoleBStorageInstance;
            if (ownerObjectStorage != null)
                SaveObjectsLinkToxml(associationEnd, ownerObjectStorage, ownerObjectRef, ownerStorageInstance, relatedObjectRef, index);
        }

        private void SaveObjectsLinkToxml(MetaDataRepository.AssociationEnd associationEnd, MetaDataStorageSession ownerObjectStorage, PersistenceLayerRunTime.StorageInstanceAgent ownerObjectRef, XElement ownerStorageInstance, PersistenceLayerRunTime.StorageInstanceAgent relatedObjectRef, int index)
        {
            int i = 0;
            bool alreadyExist;
            XElement roleObjectReferencesData = null;//=(System.Xml.XmlElement)RoleBStorageInstance.SelectSingleNode(XQuery);
            string _roleName = null;
            string _roleObjectIDAsStr = relatedObjectRef.PersistentObjectID.ToString(CultureInfoHelper.GetCultureInfo(1033));





            while (true)
            {
                if (i == ownerObjectRef.ValueTypePath.Count)
                {
                    #region gets role name with backward computability

                    _roleName = ownerObjectStorage.GetMappedTagName(associationEnd.Identity.ToString().ToLower());
                    if (string.IsNullOrWhiteSpace(_roleName))
                    {
                        _roleName = associationEnd.Name;
                        if (string.IsNullOrWhiteSpace(_roleName))
                        {
                            if (associationEnd.IsRoleA)
                                _roleName = LinkInitiatorAssociationEnd.Association.Name + "RoleAName";
                            else
                                _roleName = LinkInitiatorAssociationEnd.Association.Name + "RoleBName";
                        }
                        ownerObjectStorage.SetMappedTagName(associationEnd.Identity.ToString().ToLower(), _roleName);
                    }

                    #endregion

                    CultureInfo culture = null;
                    bool multiligual = IsMultilingualLink(RoleA, RoleB, LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association);
                    if (multiligual)
                        culture = this.Culture;


                    alreadyExist = UpdateExistingRoleData(ownerStorageInstance, ref roleObjectReferencesData, culture, _roleName, _roleObjectIDAsStr, associationEnd.Indexer, index);
                    break;
                }
                else
                {
                    #region walk to value type path

                    MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(ownerObjectRef.ValueTypePath.ToArray()[ownerObjectRef.ValueTypePath.Count - i - 1]);

                    #region gets attribute name with backward computability

                    string elementName = metaObject.Name;
                    elementName = ownerObjectStorage.GetMappedTagName(metaObject.Identity.ToString().ToLower());
                    if (string.IsNullOrEmpty(elementName))
                    {
                        elementName = metaObject.Name;
                        ownerObjectStorage.SetMappedTagName(metaObject.Identity.ToString().ToLower(), elementName);
                    }

                    #endregion


                    XElement element = ownerStorageInstance.Element(elementName);
                    if (element != null)
                    {
                        ownerStorageInstance = element;
                    }
                    else
                    {
                        var valueTypeInstance = new XElement(elementName);
                        ownerStorageInstance.Add(valueTypeInstance);
                        ownerStorageInstance = valueTypeInstance;
                    }

                    i++;
                    #endregion


                }
            }
            if (!alreadyExist)
            {
                if (roleObjectReferencesData == null)
                {
                    roleObjectReferencesData = new XElement(_roleName);
                    ownerStorageInstance.Add(roleObjectReferencesData);
                }
                XElement objRefCollection = roleObjectReferencesData;
                bool multiligual = IsMultilingualLink(RoleA, RoleB, LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association);
                if (multiligual)
                {

                    var cultureElement = objRefCollection.Element(this.Culture.Name);
                    if (cultureElement == null)
                    {
                        cultureElement = new XElement(this.Culture.Name);
                        objRefCollection.Add(cultureElement);
                    }
                    objRefCollection = cultureElement;
                }
                XElement newXmlElement = new XElement("oid");
                if (associationEnd.Indexer)
                {
                    if (index == -1)
                        newXmlElement.SetAttribute("Sort", objRefCollection.Elements().Count().ToString());
                    else
                        newXmlElement.SetAttribute("Sort", index.ToString());
                }
                objRefCollection.Add(newXmlElement);//(System.Xml.XmlNode)
                newXmlElement.Value = _roleObjectIDAsStr;
                newXmlElement.SetAttribute("ClassInstaditationName", relatedObjectRef.MemoryInstance.GetType().FullName);
                newXmlElement.SetAttribute("AssemblyFullName", relatedObjectRef.MemoryInstance.GetType().GetMetaData().Assembly.FullName);
                ulong storageCellReferenceID = UpdateExternalStorageData(ownerObjectStorage, relatedObjectRef);
                if (storageCellReferenceID != 0)
                {
                    newXmlElement.SetAttribute("StorageCellReference", storageCellReferenceID.ToString());

                    XElement storageCellReferences = (roleObjectReferencesData.Parent.Parent as XElement).Element("StorageCellReferences");
                    if (storageCellReferences == null)
                    {
                        storageCellReferences = new XElement("StorageCellReferences");
                        (roleObjectReferencesData.Parent.Parent as XElement).Add(storageCellReferences);
                    }

                    XElement storageCellReference = null;
                    foreach (XElement storageCellReferenceEntry in storageCellReferences.Elements())
                    {
                        if (storageCellReferenceEntry.Value == storageCellReferenceID.ToString())
                        {
                            if (storageCellReferenceEntry.GetAttribute("AssociationEndID") == associationEnd.Identity.ToString())
                                storageCellReference = storageCellReferenceEntry;
                            break;
                        }
                    }
                    if (storageCellReference == null)
                    {
                        storageCellReference = new XElement("StorageCellReference");
                        storageCellReferences.Add(storageCellReference);
                        storageCellReference.Value = storageCellReferenceID.ToString();
                        storageCellReference.SetAttribute("AssociationEndID", associationEnd.Identity.ToString());
                    }
                }
                else
                {
                    XElement storageCellReferences = roleObjectReferencesData.Parent.Parent.Element("StorageCellReferences");
                    if (storageCellReferences == null)
                    {
                        storageCellReferences = new XElement("StorageCellReferences");
                        roleObjectReferencesData.Parent.Parent.Add(storageCellReferences);
                    }

                    XElement storageCell = null;
                    foreach (XElement storageCellReferenceEntry in storageCellReferences.Elements())
                    {
                        if (storageCellReferenceEntry.Value == relatedObjectRef.StorageCellSerialNumber.ToString())
                        {
                            if (storageCellReferenceEntry.GetAttribute("AssociationEndID") == associationEnd.Identity.ToString())
                            {
                                storageCell = storageCellReferenceEntry;
                                break;
                            }
                        }
                    }
                    if (storageCell == null)
                    {
                        storageCell = new XElement("StorageCell");
                        storageCellReferences.Add(storageCell);
                        storageCell.Value = relatedObjectRef.StorageCellSerialNumber.ToString();
                        storageCell.SetAttribute("AssociationEndID", associationEnd.Identity.ToString());
                    }
                }
            }
            else
            {

            }


        }

        private static bool UpdateExistingRoleData(XElement storageInstance, ref XElement roleObjectReferencesData, CultureInfo culture, string roleName, string roleObjectIDAsStr, bool indexer, int index)
        {
            bool alreadyExist = false;
            roleObjectReferencesData = storageInstance.Element(roleName);
            XElement objRefCollection = roleObjectReferencesData;
            if (culture != null && objRefCollection != null)
                objRefCollection = objRefCollection.Element(culture.Name);

            if (objRefCollection != null)
            {
                foreach (XElement refElement in objRefCollection.Elements())
                {
                    if (refElement.Value == roleObjectIDAsStr)
                    {
                        alreadyExist = true;
                        if (indexer)
                        {
                            if (refElement != null)
                                refElement.SetAttribute("Sort", index.ToString());
                        }
                        break;
                    }
                }
            }
            return alreadyExist;
        }

        private static int GetIndex(XElement indexElement)
        {
            string indexStr = indexElement.GetAttribute("Sort");
            int index = 0;
            if (int.TryParse(indexStr, out index))
                return index;
            else
                return 0;

        }


        private ulong UpdateExternalStorageData(MetaDataStorageSession objectStorage, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstanceAgent)
        {
            if (objectStorage.StorageIdentity != storageInstanceAgent.StorageIdentity)
            {
                XElement linkedStoragesElement = objectStorage.XMLDocument.Root.Element("LinkedStorages");
                XElement linkedStorageElement = null;
                if (linkedStoragesElement == null)
                {
                    linkedStoragesElement = new XElement("LinkedStorages");
                    objectStorage.XMLDocument.Root.Add(linkedStoragesElement);
                    linkedStorageElement = new XElement("LinkedStorage");
                    linkedStoragesElement.Add(linkedStorageElement);
                    linkedStorageElement.SetAttribute("StorageIdentity", storageInstanceAgent.StorageIdentity);
                    linkedStorageElement.SetAttribute("StorageLocation", storageInstanceAgent.StorageLocation);
                    linkedStorageElement.SetAttribute("StorageType", storageInstanceAgent.StorageType);
                    linkedStorageElement.SetAttribute("StorageName", storageInstanceAgent.StorageName);
                }
                foreach (XElement element in linkedStoragesElement.Elements("LinkedStorage"))
                {
                    if (element.GetAttribute("StorageIdentity").ToLower() == storageInstanceAgent.StorageIdentity.ToLower())
                    {
                        linkedStorageElement = element;
                        break;
                    }
                }
                if (linkedStorageElement == null)
                {
                    linkedStorageElement = new XElement("LinkedStorage");
                    linkedStoragesElement.Add(linkedStorageElement);
                    linkedStorageElement.SetAttribute("StorageIdentity", storageInstanceAgent.StorageIdentity);
                    linkedStorageElement.SetAttribute("StorageLocation", storageInstanceAgent.StorageLocation);
                    linkedStorageElement.SetAttribute("StorageType", storageInstanceAgent.StorageType);
                    linkedStorageElement.SetAttribute("StorageName", storageInstanceAgent.StorageName);
                }

                XElement storageCellsReference = linkedStorageElement.Element("StorageCellsReference");
                if (storageCellsReference == null)
                {
                    storageCellsReference = new XElement("StorageCellsReference");
                    linkedStorageElement.Add(storageCellsReference);

                }
                XElement storageCellReference = null;
                foreach (XElement element in storageCellsReference.Elements())
                {
                    if (element.GetAttribute("SerialNumber").ToLower() == storageInstanceAgent.StorageCellSerialNumber.ToString().ToLower())
                    {
                        storageCellReference = element;
                        break;
                    }
                }
                if (storageCellReference == null)
                {
                    ulong ObjID = ulong.Parse(objectStorage.ObjectIDNode.GetAttribute("ObjID"));
                    ObjID++;
                    objectStorage.ObjectIDNode.SetAttribute("ObjID", ObjID.ToString());
                    storageCellReference = new XElement("StorageCellReference");
                    storageCellsReference.Add(storageCellReference);

                    storageCellReference.SetAttribute("SerialNumber", storageInstanceAgent.StorageCellSerialNumber.ToString());
                    storageCellReference.SetAttribute("Name", storageInstanceAgent.StorageCellName);
                    storageCellReference.SetAttribute("OID", ObjID.ToString());
                    XElement objectIdentityType = new XElement("ObjectIdentityType");
                    storageCellReference.Add(objectIdentityType);
                    foreach (var part in storageInstanceAgent.ObjectIdentityType.Parts)
                    {
                        XElement partXmlElement = new XElement("Part");
                        objectIdentityType.Add(partXmlElement);
                        partXmlElement.SetAttribute("PartTypeName", part.PartTypeName);
                        partXmlElement.SetAttribute("Type", part.Type.FullName);
                    }
                    return ObjID;
                }
                else
                {
                    return ulong.Parse(storageCellReference.GetAttribute("OID"));
                }


            }
            return 0;
        }





        void CreateRelationStorageInstance()
        {
            //Met AssociationClass=
            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            MetaDataStorageInstanceRef associationObject = (MetaDataStorageInstanceRef)RelationObject.RealStorageInstanceRef;

            MetaDataRepository.Class AssociationClass = associationObject.Class;

            PersistenceLayerRunTime.StorageInstanceRef AssociationRoleA = this.RoleA.RealStorageInstanceRef;
            PersistenceLayerRunTime.StorageInstanceRef AssociationRoleB = this.RoleB.RealStorageInstanceRef;



            MetaDataStorageSession ObjectStorageSession =
                (MetaDataStorageSession)associationObject.ObjectStorage;
            ObjectStorageSession.Dirty = true;
            XDocument XmlDocument = ObjectStorageSession.XMLDocument;
            string StorageName = XmlDocument.Root.Name.LocalName;
            XElement RoleAStorageInstance = ObjectStorageSession.GetXMLElement(AssociationRoleA.MemoryInstance.GetType(), (ObjectID)AssociationRoleA.PersistentObjectID);
            XElement RoleBStorageInstance = ObjectStorageSession.GetXMLElement(AssociationRoleB.MemoryInstance.GetType(), (ObjectID)AssociationRoleB.PersistentObjectID);
            XElement AssociationObjectStorageInstance = ObjectStorageSession.GetXMLElement(associationObject.MemoryInstance.GetType(), (ObjectID)associationObject.PersistentObjectID);
            //RoleAStorageInstance.SetAttribute("ReferentialIntegrityCount",AssociationRoleA.ReferentialIntegrityCount.ToString());
            //RoleBStorageInstance.SetAttribute("ReferentialIntegrityCount",AssociationRoleB.ReferentialIntegrityCount.ToString());
            StorageName = XmlDocument.Root.Name.LocalName;
            XElement RoleACollection = null;
            XElement RoleBCollection = null;


            bool AlreadyExist = false;
            foreach (XElement CurrNode in RoleAStorageInstance.Elements())
            {
                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                string StrObjectID = associationObject.PersistentObjectID.ToString(OOAdvantech.CultureInfoHelper.GetCultureInfo(1033));
                XElement Element = CurrNode;
                string RoleBName = AssociationClass.ClassHierarchyLinkAssociation.RoleB.Name;
                if (RoleBName == null || RoleBName.Trim().Length == 0)
                    RoleBName = AssociationClass.ClassHierarchyLinkAssociation.Name + "RoleBName";
                if (Element.Name == RoleBName)
                {
                    RoleBCollection = Element;
                    foreach (XElement inCurrNode in RoleBCollection.Elements())
                    {
                        XElement inElement = inCurrNode;
                        if (inElement.Value == StrObjectID)
                        //if(inElement.GetAttribute("oid")==StrObjectID)
                        {
                            AlreadyExist = true;
                            break;
                        }
                    }
                    break;
                }
            }
            if (!AlreadyExist)
            {
                if (RoleBCollection == null)
                {
                    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                    string RoleBName = AssociationClass.ClassHierarchyLinkAssociation.RoleB.Name;
                    if (RoleBName == null || RoleBName.Trim().Length == 0)
                        RoleBName = AssociationClass.ClassHierarchyLinkAssociation.Name + "RoleBName";
                    RoleBCollection = new XElement(RoleBName);// RoleAStorageInstance.OwnerDocument.CreateElement(RoleBName);
                    RoleAStorageInstance.Add(RoleBCollection);
                }
                XElement newXmlElement = new XElement("oid");
                RoleBCollection.Add(newXmlElement);//(System.Xml.XmlNode)
                newXmlElement.Value = associationObject.PersistentObjectID.ToString(CultureInfoHelper.GetCultureInfo(1033));
                //NewXmlElement.SetAttribute("oid",AssociationObject.ObjectID.ToString());

                newXmlElement.SetAttribute("ClassInstaditationName", associationObject.MemoryInstance.GetType().FullName);
                newXmlElement.SetAttribute("AssemblyFullName", associationObject.MemoryInstance.GetType().GetMetaData().Assembly.FullName);

                XElement storageCellReferences = (RoleBCollection.Parent.Parent as XElement).Element("StorageCellReferences");
                if (storageCellReferences == null)
                {
                    storageCellReferences = new XElement("StorageCellReferences");
                    RoleBCollection.Parent.Parent.Add(storageCellReferences);
                    //storageCellReferences = (newXmlElement.Parent.Parent.Parent as XElement).AppendChild(newXmlElement.OwnerDocument.CreateElement("StorageCellReferences")) as System.Xml.XmlElement; ;
                }
                XElement storageCell = null;
                foreach (XElement storageCellReferenceEntry in storageCellReferences.Elements())
                {
                    if (storageCellReferenceEntry.Value == RelationObject.StorageCellSerialNumber.ToString())
                    {
                        if (storageCellReferenceEntry.GetAttribute("AssociationEndID") == LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString())
                        {
                            storageCell = storageCellReferenceEntry;
                            break;
                        }
                    }
                }
                if (storageCell == null)
                {
                    storageCell = new XElement("StorageCell");
                    storageCellReferences.Add(storageCell);
                    storageCell.Value = RelationObject.StorageCellSerialNumber.ToString();
                    storageCell.SetAttribute("AssociationEndID", LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString());
                }
            }
            AlreadyExist = false;

            foreach (XElement CurrNode in RoleBStorageInstance.Elements())
            {
                string StrObjectID = associationObject.PersistentObjectID.ToString();
                XElement Element = CurrNode;
                string RoleAName = AssociationClass.ClassHierarchyLinkAssociation.RoleA.Name;
                if (RoleAName == null || RoleAName.Trim().Length == 0)
                    RoleAName = AssociationClass.ClassHierarchyLinkAssociation.Name + "RoleAName";
                if (Element.Name == RoleAName)
                {
                    RoleACollection = Element;
                    foreach (XElement inCurrNode in RoleACollection.Elements())
                    {
                        XElement inElement = inCurrNode;
                        //if(inElement.GetAttribute("oid")==StrObjectID)
                        if (inElement.Value == StrObjectID)
                        {
                            AlreadyExist = true;
                            break;
                        }
                    }
                    break;
                }
            }
            if (!AlreadyExist)
            {
                if (RoleACollection == null)
                {
                    string RoleAName = AssociationClass.ClassHierarchyLinkAssociation.RoleA.Name;
                    if (RoleAName == null || RoleAName.Trim().Length == 0)
                        RoleAName = AssociationClass.ClassHierarchyLinkAssociation.Name + "RoleAName";
                    RoleACollection = new XElement(RoleAName);
                    RoleBStorageInstance.Add(RoleACollection);
                }
                XElement newXmlElement = new XElement("oid");
                RoleACollection.Add(newXmlElement);//(System.Xml.XmlNode)
                newXmlElement.Value = associationObject.PersistentObjectID.ToString(CultureInfoHelper.GetCultureInfo(1033));
                //NewXmlElement.SetAttribute("oid",AssociationObject.ObjectID.ToString());
                newXmlElement.SetAttribute("ClassInstaditationName", associationObject.MemoryInstance.GetType().FullName);
                newXmlElement.SetAttribute("AssemblyFullName", associationObject.MemoryInstance.GetType().GetMetaData().Assembly.FullName);

                XElement storageCellReferences = RoleACollection.Parent.Parent.Element("StorageCellReferences");
                if (storageCellReferences == null)
                {
                    storageCellReferences = new XElement("StorageCellReferences");
                    RoleACollection.Parent.Parent.Add(storageCellReferences);

                    XElement storageCell = null;
                    foreach (XElement storageCellReferenceEntry in storageCellReferences.Elements())
                    {
                        if (storageCellReferenceEntry.Value == RelationObject.StorageCellSerialNumber.ToString())
                        {
                            if (storageCellReferenceEntry.GetAttribute("AssociationEndID") == LinkInitiatorAssociationEnd.Association.RoleA.Identity.ToString())
                            {
                                storageCell = storageCellReferenceEntry;
                                break;
                            }
                        }
                    }
                    if (storageCell == null)
                    {
                        storageCell = new XElement("StorageCell");
                        storageCellReferences.Add(storageCell);
                        storageCell.Value = RelationObject.StorageCellSerialNumber.ToString();
                        storageCell.SetAttribute("AssociationEndID", LinkInitiatorAssociationEnd.Association.RoleA.Identity.ToString());
                    }
                }
            }
            AlreadyExist = false;
            AssociationObjectStorageInstance.SetAttribute("RoleA", AssociationRoleA.PersistentObjectID.ToString(CultureInfoHelper.GetCultureInfo(1033)));
            AssociationObjectStorageInstance.SetAttribute("RoleAClassInstaditationName", AssociationRoleA.MemoryInstance.GetType().FullName);
            AssociationObjectStorageInstance.SetAttribute("RoleB", AssociationRoleB.PersistentObjectID.ToString(CultureInfoHelper.GetCultureInfo(1033)));
            AssociationObjectStorageInstance.SetAttribute("RoleBClassInstaditationName", AssociationRoleB.MemoryInstance.GetType().FullName);

            XElement associationObjectStorageCellReferences = (AssociationObjectStorageInstance.Parent).Element("StorageCellReferences");
            if (associationObjectStorageCellReferences == null)
            {
                associationObjectStorageCellReferences = new XElement("StorageCellReferences");
                AssociationObjectStorageInstance.Parent.Add(associationObjectStorageCellReferences);
            }
            XElement roleAStorageCell = null;
            foreach (XElement storageCellReferenceEntry in associationObjectStorageCellReferences.Elements())
            {
                if (storageCellReferenceEntry.Value == RoleA.StorageCellSerialNumber.ToString())
                {
                    if (storageCellReferenceEntry.GetAttribute("AssociationEndID") == LinkInitiatorAssociationEnd.Association.RoleA.Identity.ToString())
                    {
                        roleAStorageCell = storageCellReferenceEntry;
                        break;
                    }
                }
            }
            if (roleAStorageCell == null)
            {
                roleAStorageCell = new XElement("StorageCell");
                associationObjectStorageCellReferences.Add(roleAStorageCell); //.AppendChild(associationObjectStorageCellReferences.OwnerDocument.CreateElement("StorageCell")) as System.Xml.XmlElement;
                roleAStorageCell.Value = RoleA.StorageCellSerialNumber.ToString();
                roleAStorageCell.SetAttribute("AssociationEndID", LinkInitiatorAssociationEnd.Association.RoleA.Identity.ToString());
            }

            XElement roleBStorageCell = null;
            foreach (XElement storageCellReferenceEntry in associationObjectStorageCellReferences.Elements())
            {
                if (storageCellReferenceEntry.Value == RoleB.StorageCellSerialNumber.ToString())
                {
                    if (storageCellReferenceEntry.GetAttribute("AssociationEndID") == LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString())
                    {
                        roleBStorageCell = storageCellReferenceEntry;
                        break;
                    }
                }
            }
            if (roleBStorageCell == null)
            {
                roleBStorageCell = new XElement("StorageCell");
                associationObjectStorageCellReferences.Add(roleBStorageCell);//.AppendChild(associationObjectStorageCellReferences.OwnerDocument.CreateElement("StorageCell")) as System.Xml.XmlElement;
                roleBStorageCell.Value = RoleB.StorageCellSerialNumber.ToString();
                roleBStorageCell.SetAttribute("AssociationEndID", LinkInitiatorAssociationEnd.Association.RoleB.Identity.ToString());
            }

        }

    }



}
