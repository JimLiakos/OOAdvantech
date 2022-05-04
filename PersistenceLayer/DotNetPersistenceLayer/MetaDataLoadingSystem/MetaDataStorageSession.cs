using System;
using System.Linq;
using System.Xml.Linq;
using OOAdvantech.Collections.Generic;
using System.Reflection;
using OOAdvantech.PersistenceLayerRunTime;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.MetaDataLoadingSystem
{
    /// <MetaDataID>{D1F92A93-7119-4CE8-A38F-35F2DFE6F6A9}</MetaDataID>
    [Transactional]
    public class MetaDataStorageSession : PersistenceLayerRunTime.ObjectStorage
    {

        public override bool IsReadonly
        {
            get
            {
                if (RawStorageData != null && RawStorageData.IsReadonly)
                    return true;
                else
                    return false;
            }
        }


        /// <MetaDataID>{272e5927-3b89-406d-be98-c92b6a8f0aea}</MetaDataID>
        protected override OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            foreach (var ownedElement in (_StorageMetaData as Storage).OwnedElements)
            {
                if (ownedElement is StorageCellReference)
                {
                    if ((ownedElement as StorageCellReference).StorageIdentity == storageCell.StorageIdentity &&
                        (ownedElement as StorageCellReference).SerialNumber == storageCell.SerialNumber)
                        return ownedElement as StorageCellReference;
                }
            }

            return null;
        }


        /// <MetaDataID>{1c75db52-97eb-4be8-af7d-ea3f1a3b183b}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber)
        {
            foreach (var storageCell in _StorageCells.Values)
            {
                if (storageCell.SerialNumber == storageCellSerialNumber)
                    return storageCell;
            }

            foreach (var entry in ObjectCollectionNodes)
            {

                if (entry.Key is System.Type && !_StorageCells.ContainsKey(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class))
                {
                    StorageCell storageCell = new StorageCell(StorageIdentity, OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class, entry.Value, StorageMetaData as MetaDataRepository.Namespace);
                    _StorageCells[OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class] = storageCell;
                }
            }
            foreach (var storageCell in _StorageCells.Values)
            {
                if (storageCell.SerialNumber == storageCellSerialNumber)
                    return storageCell;
            }



            return null;

        }
        /// <MetaDataID>{2de13032-3621-4b5a-8ba5-95a87d3729fd}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstanceAgent)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{86edde25-2322-4ea0-908d-30fc2c597340}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
        {
            return new ObjectQueryLanguage.DataLoader(dataNode, dataLoaderMetadata);
        }


        /// <MetaDataID>{634ad3c3-7c63-4c8f-a484-6981e2372476}</MetaDataID>
        string StorageIdentity = System.Guid.NewGuid().ToString();

        /// <MetaDataID>{9b096eab-2383-4e4e-aaa3-c3768104ce71}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells, OOAdvantech.MetaDataRepository.Roles storageCellsRole, string ofTypeIdentity = null)
        {

            Dictionary<StorageCell, Dictionary<string, List<string>>> storageCellReferenceSerialNumbers = new Dictionary<StorageCell, Dictionary<string, List<string>>>();
            Set<MetaDataRepository.RelatedStorageCell> relatedStorageCells = new Set<OOAdvantech.MetaDataRepository.RelatedStorageCell>();
            //base.GetLinkedStorageCellsFromObjectsUnderTransaction(
            Dictionary<MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell> linkedStorageCells = new Dictionary<OOAdvantech.MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell>();


            MetaDataRepository.AssociationEnd associationEnd = null;
            if (storageCellsRole == MetaDataRepository.Roles.RoleA)
                associationEnd = association.RoleB;
            else
                associationEnd = association.RoleA;

            System.Collections.Generic.List<MetaDataRepository.AssociationEnd> associationEnds = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd>();
            associationEnds.Add(associationEnd);
            foreach (MetaDataRepository.Association specilazationAssociation in associationEnd.Association.Specializations)
            {
                if (associationEnd.IsRoleA)
                    associationEnds.Add(specilazationAssociation.RoleA);
                else
                    associationEnds.Add(specilazationAssociation.RoleB);
            }
            string[] path = new string[0];

            foreach (MetaDataRepository.AssociationEnd theAssociationEnd in associationEnds)
            {
                foreach (StorageCell rootStorageCell in storageCells)
                {
                    bool withRelationTable = false;
                    //if (associationEnd != null &&
                    //    (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                    //    associationEnd.Association.Specializations.Count > 0) &&
                    //    associationEnd.Association != rootStorageCell.Type.ClassHierarchyLinkAssociation)
                    //{
                    //    withRelationTable = true;
                    //}
                    if (rootStorageCell.XmlElement == null)
                        continue;
                    foreach (XElement childNode in rootStorageCell.XmlElement.Elements())
                    {
                        if (childNode.Name != "Object")
                            continue;
                        bool exist = false;
                        XElement objectElement = childNode;
                        if (path.Length > 0)
                        {
                            foreach (string nodeName in path)
                            {
                                exist = false;
                                foreach (XElement xmlElement in objectElement.Elements())
                                {
                                    if (xmlElement.Name == nodeName)
                                    {
                                        exist = true;
                                        objectElement = xmlElement;
                                        break;
                                    }
                                }
                                if (!exist)
                                    break;
                            }
                            if (!exist)
                                continue;
                        }
                        foreach (XElement relationElement in objectElement.Elements())
                        {
                            if (relationElement.Name == theAssociationEnd.Name)
                            {
                                int _break = 0;
                                foreach (XElement OIDElement in relationElement.Elements())
                                {
                                    if (OIDElement.HasAttribute("StorageCellReference"))
                                    {
                                        string StorageCellReferenceSerialNumber = OIDElement.GetAttribute("StorageCellReference");
                                        if (!storageCellReferenceSerialNumbers.ContainsKey(rootStorageCell))
                                            storageCellReferenceSerialNumbers[rootStorageCell] = new Dictionary<string, List<string>>();

                                        if (!storageCellReferenceSerialNumbers[rootStorageCell].ContainsKey(associationEnd.Identity.ToString()))
                                            storageCellReferenceSerialNumbers[rootStorageCell][associationEnd.Identity.ToString()] = new List<string>();

                                        if (!storageCellReferenceSerialNumbers[rootStorageCell][associationEnd.Identity.ToString()].Contains(StorageCellReferenceSerialNumber))
                                            storageCellReferenceSerialNumbers[rootStorageCell][associationEnd.Identity.ToString()].Add(StorageCellReferenceSerialNumber);
                                    }
                                    else
                                    {

                                        MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(
                                                          ModulePublisher.ClassRepository.GetType(OIDElement.GetAttribute("ClassInstaditationName"), OIDElement.GetAttribute("AssemblyFullName"))) as MetaDataRepository.Classifier;

                                        StorageCell linkedstorageCell = GetStorageCell(classifier as MetaDataRepository.Class) as StorageCell;
                                        if (linkedstorageCell != null && !linkedStorageCells.ContainsKey(linkedstorageCell))
                                            linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, theAssociationEnd.Identity.ToString(), withRelationTable));
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (storageCellReferenceSerialNumbers.Count > 0)
            {
                foreach (var ownedElement in (_StorageMetaData as Storage).OwnedElements)
                {
                    if (ownedElement is StorageCellReference)
                    {
                        foreach (StorageCell rootStorageCell in storageCellReferenceSerialNumbers.Keys)
                        {
                            foreach (string relationPart in storageCellReferenceSerialNumbers[rootStorageCell].Keys)
                            {
                                if (storageCellReferenceSerialNumbers[rootStorageCell][relationPart].Contains((ownedElement as StorageCellReference).StorageCellReferenceElement.GetAttribute("OID")))
                                {

                                    StorageCellReference storageCellReference = ownedElement as StorageCellReference;

                                    MetaDataRepository.StorageCell storageCell = storageCellReference.RealStorageCell;

                                    if (!linkedStorageCells.ContainsKey(storageCell))
                                        linkedStorageCells.Add(storageCell, new MetaDataRepository.RelatedStorageCell(storageCell, rootStorageCell, relationPart, true));
                                    if (!linkedStorageCells.ContainsKey(storageCellReference))
                                        linkedStorageCells.Add(storageCellReference, new MetaDataRepository.RelatedStorageCell(storageCellReference, rootStorageCell, relationPart, true));
                                }
                            }
                        }

                    }
                }
            }

            relatedStorageCells.AddRange(linkedStorageCells.Values);
            return relatedStorageCells;

            //    return GetLinkedStorageCells(associationEnd, new MetaDataRepository.ValueTypePath(), relatedStorageCells);


            //    MetaDataRepository.Classifier classifier = association.LinkClass;
            //OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell>();

            //foreach (MetaDataRepository.Classifier subClassifier in classifier.GetAllSpecializeClasifiers())
            //{
            //    if (!(subClassifier is MetaDataRepository.Class) || (subClassifier as MetaDataRepository.Class).Abstract)
            //        continue;
            //    if (_StorageCells.ContainsKey(subClassifier as MetaDataRepository.Class))
            //        storageCells.Add( _StorageCells[subClassifier as MetaDataRepository.Class]);
            //    else
            //    {

            //        XElement xmlElement = null;
            //        if (!ObjectCollectionNodes.TryGetValue(subClassifier.GetExtensionMetaObject<System.Type>(), out xmlElement))
            //            continue;
            //        StorageCell storageCell = new StorageCell(StorageIdentity, subClassifier as MetaDataRepository.Class, xmlElement, StorageMetaData as MetaDataRepository.Namespace);


            //        storageCells.Add(storageCell);
            //        _StorageCells[subClassifier as MetaDataRepository.Class] = storageCell;
            //    }
            //}
            //if (classifier is MetaDataRepository.Class && !(classifier as MetaDataRepository.Class).Abstract)
            //{
            //    if (_StorageCells.ContainsKey(classifier as MetaDataRepository.Class))
            //        storageCells.Add(_StorageCells[classifier as MetaDataRepository.Class]);
            //    else
            //    {
            //        XElement xmlElement = ObjectCollectionNodes[classifier.GetExtensionMetaObject<System.Type>()];
            //        if (xmlElement != null)
            //        {
            //            StorageCell storageCell = new StorageCell(StorageIdentity, classifier as MetaDataRepository.Class, xmlElement, StorageMetaData as MetaDataRepository.Namespace);
            //            storageCells.Add(storageCell);
            //            _StorageCells[classifier as MetaDataRepository.Class] = storageCell;
            //        }
            //    }
            //}

            //return storageCells;
        }

        /// <MetaDataID>{319e1a30-504e-40b1-8b87-b4befde5f09a}</MetaDataID>
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            Commands.BuildContainedObjectIndicies buildContainedObjectIndicies = new Commands.BuildContainedObjectIndicies(collection);
            transactionContext.EnlistCommand(buildContainedObjectIndicies);

            // throw new NotImplementedException();
        }


        /// <MetaDataID>{d9a2ec29-1064-48c6-a8b1-aab072380e9b}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {

            throw new Exception("The method or operation is not implemented.");
        }
        /// <MetaDataID>{4b6a06dd-b455-40de-9731-c572193312d9}</MetaDataID>
        internal string GetMappedTagName(string id)
        {
            if (id != null)
                id = id.ToLower();
            if (MappedTagNames == null)
            {
                MappedTagNames = new Dictionary<string, string>();
                foreach (XElement xmlNode in XMLDocument.Root.Elements())
                {
                    if (xmlNode.Name == "MetaData")
                    {
                        foreach (XElement memberElement in xmlNode.Elements())
                        {
                            MappedTagNames[memberElement.GetAttribute("id").ToLower()] = memberElement.GetAttribute("TagName");
                            //if (memberElement.GetAttribute("id") == id)
                            //    return memberElement.GetAttribute("TagName");
                        }
                        //return null;
                    }
                }
            }
            string tagName = null;
            MappedTagNames.TryGetValue(id, out tagName);
            return tagName;
        }
        /// <MetaDataID>{a41f88f6-b673-4382-8158-5b8ff205cb1b}</MetaDataID>
        Dictionary<string, string> MappedTagNames;
        /// <MetaDataID>{2467a706-b59b-49d5-a59f-a10f8714a5ec}</MetaDataID>
        internal void SetMappedTagName(string id, string tagName)
        {

            if (GetMappedTagName(id) != null)
                return; // id slready exist;


            XElement member = null;
            foreach (XElement xmlNode in XMLDocument.Root.Elements())
            {
                if (xmlNode.Name == "MetaData")
                {
                    foreach (XElement memberElement in xmlNode.Elements())
                    {
                        if (memberElement.GetAttribute("id") == id)
                            return;
                    }
                    MappedTagNames[id] = tagName;
                    member = new XElement("Member");
                    xmlNode.Add(member);
                    member.SetAttribute("id", id);
                    member.SetAttribute("TagName", tagName);
                    return;
                }
            }
            XElement metadataElement = new XElement("MetaData");
            XMLDocument.Root.Add(metadataElement);
            member = new XElement("Member");
            metadataElement.Add(member);
            member.SetAttribute("id", id);
            member.SetAttribute("TagName", tagName);
            MappedTagNames[id] = tagName;
            return;


            //foreach(System.Xml.XmlNode xmlNode ObjectCollectionNodes[_class.GetExtensionMetaObject(typeof(System.Type))] as System.Xml.XmlNode)



        }

        /// <MetaDataID>{46392d10-9852-4273-86b6-882a82dc7c5f}</MetaDataID>
        public override Set<MetaDataRepository.RelatedStorageCell> GetLinkedStorageCells(MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath, Set<MetaDataRepository.StorageCell> storageCells, string ofTypeIdentity = null)
        {
            if (associationEnd.Specification.Identity.ToString() == ofTypeIdentity)
                ofTypeIdentity = null;

            

            Dictionary<StorageCell, Dictionary<string, List<string>>> storageCellReferenceSerialNumbers = new Dictionary<StorageCell, Dictionary<string, List<string>>>();
            Set<MetaDataRepository.RelatedStorageCell> relatedStorageCells = new Set<OOAdvantech.MetaDataRepository.RelatedStorageCell>();
            //base.GetLinkedStorageCellsFromObjectsUnderTransaction(
            Dictionary<MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell> linkedStorageCells = new Dictionary<OOAdvantech.MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell>();

            string[] path = new string[valueTypePath.Count];

            for (int i = 0; i < valueTypePath.Count; i++)
            {
                MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(valueTypePath.ToArray()[valueTypePath.Count - i - 1]);
                string memberName = GetMappedTagName(metaObject.Identity.ToString());
                if (string.IsNullOrEmpty(memberName))
                {
                    memberName = metaObject.Name;
                    SetMappedTagName(metaObject.Identity.ToString(), metaObject.Name);
                }
                path[i] = memberName;
            }
            System.Collections.Generic.List<MetaDataRepository.AssociationEnd> associationEnds = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd>();
            associationEnds.Add(associationEnd);
            foreach (MetaDataRepository.Association association in associationEnd.Association.Specializations)
            {
                if (associationEnd.IsRoleA)
                    associationEnds.Add(association.RoleA);
                else
                    associationEnds.Add(association.RoleB);
            }

            foreach (MetaDataRepository.AssociationEnd hierarchyAssociationEnd in associationEnds)
            {

                foreach (StorageCell rootStorageCell in storageCells)
                {
                    bool multilingual = rootStorageCell.Type.IsMultilingual(hierarchyAssociationEnd);
                    bool withRelationTable = false;
                    if (associationEnd != null &&
                        (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                        associationEnd.Association.Specializations.Count > 0) &&
                        associationEnd.Association != rootStorageCell.Type.ClassHierarchyLinkAssociation)
                    {
                        withRelationTable = true;
                    }
                    if (rootStorageCell.XmlElement == null)
                        continue;
                    foreach (XElement childNode in rootStorageCell.XmlElement.Elements())
                    {
                        if (childNode.Name != "Object")
                            continue;
                        bool exist = false;
                        XElement objectElement = childNode;
                        if (path.Length > 0)
                        {
                            foreach (string nodeName in path)
                            {
                                exist = false;
                                foreach (XElement xmlElement in objectElement.Elements())
                                {
                                    if (xmlElement.Name == nodeName)
                                    {
                                        exist = true;
                                        objectElement = xmlElement;
                                        break;
                                    }
                                }
                                if (!exist)
                                    break;
                            }
                            if (!exist)
                                continue;
                        }
                        foreach (XElement relationElement in objectElement.Elements())
                        {
                            if (associationEnd.Association.LinkClass != null && rootStorageCell.Type.ClassHierarchyLinkAssociation == associationEnd.Association)
                            {
                                if (associationEnd.Role == MetaDataRepository.Roles.RoleA)
                                {
                                    string RoleAClassInstaditationName = objectElement.GetAttribute("RoleAClassInstaditationName");
                                    System.Type roleAClassInstaditationType = GetObjectCollectionType(RoleAClassInstaditationName);
                                    MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(roleAClassInstaditationType) as MetaDataRepository.Classifier;

                                    StorageCell linkedstorageCell = GetStorageCell(classifier as MetaDataRepository.Class) as StorageCell;
                                    if (linkedstorageCell != null && !linkedStorageCells.ContainsKey(linkedstorageCell))
                                    {

                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (linkedstorageCell.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, associationEnd.Identity.ToString(), withRelationTable));
                                        }
                                        else
                                            linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, associationEnd.Identity.ToString(), withRelationTable));
                                    }
                                }
                                else
                                {
                                    string RoleBClassInstaditationName = objectElement.GetAttribute("RoleBClassInstaditationName");
                                    System.Type roleBClassInstaditationType = GetObjectCollectionType(RoleBClassInstaditationName);

                                    MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(roleBClassInstaditationType) as MetaDataRepository.Classifier;

                                    StorageCell linkedstorageCell = GetStorageCell(classifier as MetaDataRepository.Class) as StorageCell;
                                    if (linkedstorageCell != null && !linkedStorageCells.ContainsKey(linkedstorageCell))
                                    {
                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (linkedstorageCell.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, associationEnd.Identity.ToString(), withRelationTable));
                                        }
                                        else
                                            linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, associationEnd.Identity.ToString(), withRelationTable));
                                    }
                                }
                            }
                            else
                            {
                                if (relationElement.Name == hierarchyAssociationEnd.Name)
                                {
                                    GetLinkectStorageCellsFromElement(associationEnd, hierarchyAssociationEnd, relationElement, rootStorageCell, withRelationTable, ofTypeIdentity, linkedStorageCells, storageCellReferenceSerialNumbers);
                                    if (multilingual)
                                    {
                                        if (OOAdvantech.CultureContext.CurrentNeutralCultureInfo != null)
                                        {
                                            XElement langugeRelationElement = relationElement.Element(OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
                                            if (langugeRelationElement != null)
                                                GetLinkectStorageCellsFromElement(associationEnd, hierarchyAssociationEnd, langugeRelationElement, rootStorageCell, withRelationTable, ofTypeIdentity, linkedStorageCells, storageCellReferenceSerialNumbers);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (storageCellReferenceSerialNumbers.Count > 0)
            {
                foreach (var ownedElement in (_StorageMetaData as Storage).OwnedElements)
                {
                    if (ownedElement is StorageCellReference)
                    {
                        foreach (StorageCell rootStorageCell in storageCellReferenceSerialNumbers.Keys)
                        {
                            foreach (string relationPart in storageCellReferenceSerialNumbers[rootStorageCell].Keys)
                            {
                                if (storageCellReferenceSerialNumbers[rootStorageCell][relationPart].Contains((ownedElement as StorageCellReference).StorageCellReferenceElement.GetAttribute("OID")))
                                {

                                    StorageCellReference storageCellReference = ownedElement as StorageCellReference;

                                    MetaDataRepository.StorageCell storageCell = storageCellReference.RealStorageCell;

                                    if (!linkedStorageCells.ContainsKey(storageCell))
                                    {
                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (storageCell.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add(storageCell, new MetaDataRepository.RelatedStorageCell(storageCell, rootStorageCell, relationPart, true));
                                        }
                                        else
                                            linkedStorageCells.Add(storageCell, new MetaDataRepository.RelatedStorageCell(storageCell, rootStorageCell, relationPart, true));
                                    }
                                    if (!linkedStorageCells.ContainsKey(storageCellReference))
                                    {
                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (storageCellReference.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add(storageCellReference, new MetaDataRepository.RelatedStorageCell(storageCellReference, rootStorageCell, relationPart, true));
                                        }
                                        else
                                            linkedStorageCells.Add(storageCellReference, new MetaDataRepository.RelatedStorageCell(storageCellReference, rootStorageCell, relationPart, true));
                                    }
                                }
                            }
                        }

                    }
                }
            }
            foreach (OOAdvantech.MetaDataRepository.RelatedStorageCell relatedStorageCell in base.GetLinkedStorageCellsFromObjectsUnderTransaction(associationEnd, valueTypePath, storageCells))
            {
                if (!linkedStorageCells.ContainsKey(relatedStorageCell.StorageCell))
                    linkedStorageCells.Add(relatedStorageCell.StorageCell, relatedStorageCell);
            }
            if (ofTypeIdentity != null)
            {
                var ofTypeClassifier= DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(new MetaDataRepository.MetaObjectID(ofTypeIdentity)) as OOAdvantech.MetaDataRepository.Classifier;
                foreach(var storageCell in linkedStorageCells.Keys)
                {
                    if(storageCell.Type.IsA(ofTypeClassifier))
                        relatedStorageCells.Add(linkedStorageCells[storageCell]);
                    else
                    {

                    }
                }
            }
            else
            {

                relatedStorageCells.AddRange(linkedStorageCells.Values);
            }
            return relatedStorageCells;

        }

        private void GetLinkectStorageCellsFromElement(MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.AssociationEnd hierarchyAssociationEnd, XElement relationElement, StorageCell rootStorageCell, bool withRelationTable, string ofTypeIdentity, Dictionary<MetaDataRepository.StorageCell, MetaDataRepository.RelatedStorageCell> linkedStorageCells, Dictionary<StorageCell, Dictionary<string, List<string>>> storageCellReferenceSerialNumbers)
        {
            foreach (XElement OIDElement in relationElement.Elements("oid"))
            {
                if (OIDElement.HasAttribute("StorageCellReference"))
                {
                    string StorageCellReferenceSerialNumber = OIDElement.GetAttribute("StorageCellReference");
                    if (!storageCellReferenceSerialNumbers.ContainsKey(rootStorageCell))
                        storageCellReferenceSerialNumbers[rootStorageCell] = new Dictionary<string, List<string>>();

                    if (!storageCellReferenceSerialNumbers[rootStorageCell].ContainsKey(associationEnd.Identity.ToString()))
                        storageCellReferenceSerialNumbers[rootStorageCell][associationEnd.Identity.ToString()] = new List<string>();

                    if (!storageCellReferenceSerialNumbers[rootStorageCell][associationEnd.Identity.ToString()].Contains(StorageCellReferenceSerialNumber))
                        storageCellReferenceSerialNumbers[rootStorageCell][associationEnd.Identity.ToString()].Add(StorageCellReferenceSerialNumber);
                }
                else
                {
                    MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(
                                  ModulePublisher.ClassRepository.GetType(OIDElement.GetAttribute("ClassInstaditationName"), OIDElement.GetAttribute("AssemblyFullName"))) as MetaDataRepository.Classifier;

                    StorageCell linkedstorageCell = GetStorageCell(classifier as MetaDataRepository.Class) as StorageCell;
                    if (linkedstorageCell != null && !linkedStorageCells.ContainsKey(linkedstorageCell))
                    {
                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                        {
                            if (linkedstorageCell.IsTypeOf(ofTypeIdentity))
                                linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, hierarchyAssociationEnd.Identity.ToString(), withRelationTable));
                        }
                        else
                            linkedStorageCells.Add(linkedstorageCell, new MetaDataRepository.RelatedStorageCell(linkedstorageCell, rootStorageCell, hierarchyAssociationEnd.Identity.ToString(), withRelationTable));
                    }
                }
            }
        }

        internal MetaDataStorageInstanceRef GetStorageInstanceRef(Type storageInstanceType, ObjectID refObjectID)
        {
            lock (this)
            {
                var storageInstanceRef = (MetaDataStorageInstanceRef)OperativeObjectCollections[storageInstanceType][refObjectID];
                if (storageInstanceRef != null)
                    return storageInstanceRef;



                XElement mElement = GetXMLElement(storageInstanceType, refObjectID);
                if (mElement == null)
                    return null;


                object newObject = null;
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    newObject = AccessorBuilder.CreateInstance(storageInstanceType);
                    stateTransition.Consistent = true;
                }

                if (newObject == null)
                    throw new System.Exception("PersistencyService can't instadiate the " + storageInstanceType.FullName);
                storageInstanceRef = (MetaDataStorageInstanceRef)CreateStorageInstanceRef(newObject, refObjectID);
                //StorageInstanceRef.ObjectID=RefObjectID;
                storageInstanceRef.TheStorageIstance = mElement;
                storageInstanceRef.LoadObjectState();
                //StorageInstanceRef.SnapshotStorageInstance();
                storageInstanceRef.ObjectActived();

                return storageInstanceRef;
            }

        }



        /// <MetaDataID>{c746ed21-bb47-474c-9ade-1d1045a19ab5}</MetaDataID>
        internal System.Collections.Generic.Dictionary<MetaDataRepository.Class, StorageCell> StorageCells
        {
            get
            {
                foreach (var entry in ObjectCollectionNodes)
                {
                    if (entry.Key is System.Type && !_StorageCells.ContainsKey(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class))
                    {
                        if (!_StorageCells.ContainsKey(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class))
                        {
                            StorageCell storageCell = new StorageCell(StorageIdentity, OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class, entry.Value as XElement, StorageMetaData as MetaDataRepository.Namespace);
                            _StorageCells[OOAdvantech.MetaDataRepository.Classifier.GetClassifier(entry.Key as System.Type) as MetaDataRepository.Class] = storageCell;
                        }
                    }
                }
                return _StorageCells;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{e75138f2-ff21-461a-a74d-ea45ffbde6fc}</MetaDataID>
        System.Collections.Generic.Dictionary<MetaDataRepository.Class, StorageCell> _StorageCells = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Class, StorageCell>();
        /// <MetaDataID>{641d095f-e3cd-416f-8ff2-e0ac753ce640}</MetaDataID>
        internal protected OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(OOAdvantech.MetaDataRepository.Class _class)
        {
            if (_StorageCells.ContainsKey(_class))
                return _StorageCells[_class];
            else
            {

                GetStorageCells(_class);
                if (_StorageCells.ContainsKey(_class))
                    return _StorageCells[_class];

                //XElement ObjectCollection = ObjectCollectionNodes[_class.GetExtensionMetaObject<System.Type>()] as XElement;
                //if (ObjectCollection == null)
                //{
                //    if (mObjectCollections == null)
                //        mObjectCollections = _XMLDocument.SelectSingleNode(StorageName + "/ObjectCollections");
                //    if (mObjectCollections == null)
                //        throw new System.Exception("Bad Storage File: There isn't ObjectCollections.");

                //    ObjectCollection = (XElement)mObjectCollections.AppendChild(mObjectCollections.OwnerDocument.CreateElement(_class.GetExtensionMetaObject<System.Type>().FullName));
                //    ObjectCollection.SetAttribute("ClassInstaditationName", _class.GetExtensionMetaObject<System.Type>().FullName);
                //    ObjectCollectionNodes[_class.GetExtensionMetaObject<System.Type>()] = ObjectCollection;
                //    if (StorageCells.ContainsKey(_class))
                //        StorageCells[_class].XmlElement = ObjectCollection;
                //    return StorageCells[_class];
                //}

                return null;
                //StorageCell storageCell = new StorageCell(StorageIntentity, _class, ObjectCollectionNodes[_class.GetExtensionMetaObject(typeof(System.Type))] as XElement, StorageMetaData as MetaDataRepository.Namespace);
                //StorageCells[_class] = storageCell;
                //return storageCell;
            }

        }
        /// <MetaDataID>{9022d2b8-414f-48cb-aa12-68e087383646}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {


            OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();

            foreach (MetaDataRepository.Classifier subClassifier in classifier.GetAllSpecializeClasifiers())
            {
                if (!(subClassifier is MetaDataRepository.Class) || (subClassifier as MetaDataRepository.Class).Abstract)
                    continue;
                if (_StorageCells.ContainsKey(subClassifier as MetaDataRepository.Class))
                    storageCells.Add(_StorageCells[subClassifier as MetaDataRepository.Class]);
                else
                {

                    XElement xmlElement = null;

                    if (!ObjectCollectionNodes.TryGetValue(subClassifier.GetExtensionMetaObject<System.Type>(), out xmlElement))
                        continue;
                    StorageCell storageCell = new StorageCell(StorageIdentity, subClassifier as MetaDataRepository.Class, xmlElement, StorageMetaData as MetaDataRepository.Namespace);


                    storageCells.Add(storageCell);
                    _StorageCells[subClassifier as MetaDataRepository.Class] = storageCell;
                }
            }
            if (classifier is MetaDataRepository.Class && !(classifier as MetaDataRepository.Class).Abstract)
            {
                if (_StorageCells.ContainsKey(classifier as MetaDataRepository.Class))
                    storageCells.Add(_StorageCells[classifier as MetaDataRepository.Class]);
                else
                {
                    XElement xmlElement = null;

                    if (ObjectCollectionNodes.TryGetValue(classifier.GetExtensionMetaObject<System.Type>(), out xmlElement))
                    {
                        StorageCell storageCell = new StorageCell(StorageIdentity, classifier as MetaDataRepository.Class, xmlElement, StorageMetaData as MetaDataRepository.Namespace);
                        storageCells.Add(storageCell);
                        _StorageCells[classifier as MetaDataRepository.Class] = storageCell;
                    }
                }
            }
            if (storageCells.Count == 0)
            {

                //if (PersistenceLayerRunTime.StorageInstanceRef.GetNewObjectsUnderTransaction(classifier).Count > 0)
                {
                    foreach (var storageInstanceRef in PersistenceLayerRunTime.StorageInstanceRef.GetNewObjectsUnderTransaction(classifier))
                    {

                        if (!_StorageCells.ContainsKey(storageInstanceRef.Class))
                        {
                            StorageCell storageCell = new StorageCell(StorageIdentity, storageInstanceRef.Class, null, StorageMetaData as MetaDataRepository.Namespace);
                            storageCells.Add(storageCell);
                            _StorageCells[storageInstanceRef.Class] = storageCell;
                        }
                    }

                }
            }
            return storageCells;
        }

        /// <MetaDataID>{07425264-b084-4e9b-a32e-1ddecc78b801}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object objectID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{a36fb9fa-b325-4961-878e-27f2da73cdbe}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            string[] persistentObjectUriParts = persistentObjectUri.Split('\\');
            string storageIdentity = persistentObjectUriParts[0];
            string storageSerialNumber = persistentObjectUriParts[1];
            string objectIdentityAsString = persistentObjectUriParts[2];

            MetaDataRepository.StorageCell storageCell = GetStorageCell(int.Parse(storageSerialNumber));

            if (storageCell == null)
                return null;

            var objectID = storageCell.ObjectIdentityType.Parse(objectIdentityAsString, storageIdentity);

            var operativeObjectCollection = OperativeObjectCollections[storageCell.Type.GetExtensionMetaObject<System.Type>()];
            var storageInstanceRef = operativeObjectCollection[objectID];
            if (storageInstanceRef != null)
            {
                storageInstanceRef.WaitUntilObjectIsActive();
                return storageInstanceRef.MemoryInstance;
            }
            else
            {
                object @object = GetObjectFromStorageCell(storageCell, objectID);
                return @object;
            }

        }


        /// <MetaDataID>{7bae4b79-93df-4d3d-8745-8ca1add805ce}</MetaDataID>
        public override string GetPersistentObjectUri(object obj)
        {
            if (obj is PersistenceLayerRunTime.StorageInstanceRef)
            {
                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = obj as PersistenceLayerRunTime.StorageInstanceRef;
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.StorageInstanceSet.SerialNumber.ToString() + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            else if (PersistencyService.ClassOfObjectIsPersistent(obj))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceRef(obj);
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.StorageInstanceSet.SerialNumber.ToString() + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            return null;
        }





        /// <MetaDataID>{1C74913B-DE2E-4FE5-9D7E-6CCDAF88CDFE}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            try
            {
                if (_XMLDocument == null)
                    throw new System.Exception("You can't execute any command because the storage session is not initialized properly.");
                if (!MetadataLoaded)
                    LoadMetadata();


                MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
                mStructureSet.Open(OQLStatement, parameters);
                return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
            }
            catch (System.Exception error)
            {
                throw;
            }


        }


        /// <MetaDataID>{BB19D3C3-191E-4B51-8004-65458C563D07}</MetaDataID>
        public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {

            PersistenceLayerRunTime.Commands.LinkObjectsCommand linkObjectsCommand = null;

            string cmdIdentity = PersistenceLayerRunTime.Commands.LinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command);

            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.LinkObjectsCommand;

            if (linkObjectsCommand == null)
            {
                //if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage == this)
                {
                    linkObjectsCommand = new Commands.MetaDataLinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(linkObjectsCommand);
                }
            }
            else
            {

                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    linkObjectsCommand.RoleAIndex = index;
                else
                    linkObjectsCommand.RoleBIndex = index;
            }

        }

        /// <MetaDataID>{9bc58cb7-ff09-49c0-a239-c1bc261537a6}</MetaDataID>
        public override void CreateUpdateLinkIndexCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleA, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleB, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationObject, OOAdvantech.PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand linkObjectsCommand = null;

            string cmdIdentity = PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command);

            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand;

            if (linkObjectsCommand == null)
            {
                //if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage == this)
                {
                    linkObjectsCommand = new Commands.UpdateLinkIndexCommand(this, roleA, roleB, linkInitiatorAssociationEnd, index);
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(linkObjectsCommand);
                }
            }
            else
            {

                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    linkObjectsCommand.RoleAIndex = index;
                else
                    linkObjectsCommand.RoleBIndex = index;
            }
        }

        /// <MetaDataID>{58F1CA4F-8A57-44D0-B459-B765F9919AF9}</MetaDataID>
        public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            Commands.MetaDataUnLinkObjectsCommand mUnLinkObjectsCommand = null;
            if (roleA.ObjectStorage == this || roleB.ObjectStorage == this)
            {
                if (roleA.StorageIdentity != roleB.StorageIdentity)
                {

                }
                string cmdIdentity = PersistenceLayerRunTime.Commands.UnLinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
                PersistenceLayerRunTime.Commands.Command command = null;
                if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command))
                {
                    mUnLinkObjectsCommand = new Commands.MetaDataUnLinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                    PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mUnLinkObjectsCommand);
                }
            }
        }

        /// <MetaDataID>{314d03f4-2af4-4330-a3fd-a47f889d96c4}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent deletedOutStorageInstanceRef, OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd, OOAdvantech.MetaDataRepository.StorageCell LinkedObjectsStorageCell)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{83E9D4BF-7AB1-4149-8AA5-1CB60F43DE71}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(storageInstanceRef))
            {
                Commands.MetaDataUpdateReferentialIntegrity Command = new Commands.MetaDataUpdateReferentialIntegrity();
                Command.UpdatedStorageInstanceRef = storageInstanceRef;

                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                transactionContext.EnlistCommand(Command);
                stateTransition.Consistent = true;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2628acc8-b297-4753-8b4f-80090870e34c}</MetaDataID>
        PersistenceLayer.Storage _StorageMetaData;
        /// <MetaDataID>{7F56D74E-CB7F-43D7-B688-669BA768B76A}</MetaDataID>
        public override PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                if (_StorageMetaData == null)
                    _StorageMetaData = new Storage(StorageIdentity, StorageName, StorageName, typeof(MetaDataStorageProvider).FullName, this.XMLDocument.Root.GetAttribute("Culture"), this);

                return _StorageMetaData;
            }
        }
        /// <MetaDataID>{7A191AE4-660B-421C-A22C-52D96A5E5923}</MetaDataID>
        private System.Collections.Generic.Dictionary<PersistenceLayerRunTime.ITransactionContext, Dictionary<XElement, string>> TransactionsNodesChanges = new System.Collections.Generic.Dictionary<PersistenceLayerRunTime.ITransactionContext, Dictionary<XElement, string>>();
        /// <MetaDataID>{59263C1B-7039-4C64-9439-F8E61F1680BA}</MetaDataID>
        private System.Collections.Generic.Dictionary<PersistenceLayerRunTime.ITransactionContext, List<XElement>> TransactionsNewNodes = new System.Collections.Generic.Dictionary<PersistenceLayerRunTime.ITransactionContext, List<XElement>>();
        /// <MetaDataID>{7C50E221-9B22-4685-9F6F-6A16D7079886}</MetaDataID>
        private Dictionary<PersistenceLayerRunTime.ITransactionContext, Dictionary<XElement, XElement>> TransactionsDeletedNodes = new Dictionary<PersistenceLayerRunTime.ITransactionContext, Dictionary<XElement, XElement>>();
        /// <MetaDataID>{2DBB86C4-114C-410E-998D-668D391E731A}</MetaDataID>
        public void DeletedNodeUnderTransaction(XElement Node, PersistenceLayerRunTime.ITransactionContext theTransaction)
        {
            if (Node == null || theTransaction == null)
                throw new System.Exception("The Node and theTransaction parammeters must be not null");
            Dictionary<XElement, XElement> DeletedNodes = null;
            if (TransactionsDeletedNodes.ContainsKey(theTransaction))
                DeletedNodes = TransactionsDeletedNodes[theTransaction] as Dictionary<XElement, XElement>;
            else
            {
                DeletedNodes = new Dictionary<XElement, XElement>();
                TransactionsDeletedNodes[theTransaction] = DeletedNodes;
            }
            if (!DeletedNodes.ContainsKey(Node))
                DeletedNodes[Node] = Node.Parent;

        }

        /// <MetaDataID>{649364CB-26F4-4E95-A8AF-51DC72E96848}</MetaDataID>
        public void NewNodeUnderTransaction(XElement Node, PersistenceLayerRunTime.ITransactionContext theTransaction)
        {

            if (Node == null || theTransaction == null)
                throw new System.Exception("The Node and theTransaction parammeters must be not null");
            List<XElement> NewNodes = null;
            if (TransactionsNewNodes.ContainsKey(theTransaction))
                NewNodes = TransactionsNewNodes[theTransaction] as List<XElement>;
            else
            {
                NewNodes = new List<XElement>();
                TransactionsNewNodes[theTransaction] = NewNodes;
            }
            if (!NewNodes.Contains(Node))
                NewNodes.Add(Node);

        }
        /// <MetaDataID>{655380F4-620D-4167-AAC1-627FE0840E1F}</MetaDataID>
        public void NodeChangedUnderTransaction(XElement Node, PersistenceLayerRunTime.ITransactionContext theTransaction)
        {

            if (Node == null || theTransaction == null)
                throw new System.Exception("The Node and theTransaction parammeters must be not null");
            Dictionary<XElement, string> OldXmlNodesState = null;
            if (TransactionsNodesChanges.ContainsKey(theTransaction))
                OldXmlNodesState = TransactionsNodesChanges[theTransaction] as Dictionary<XElement, string>;
            else
            {
                OldXmlNodesState = new Dictionary<XElement, string>();
                TransactionsNodesChanges[theTransaction] = OldXmlNodesState;
            }
            if (!OldXmlNodesState.ContainsKey(Node))
                OldXmlNodesState[Node] = Node.ToString();

        }

        /// <MetaDataID>{D94F6372-7710-4BC2-B6CB-A13D09459B32}</MetaDataID>
        public override void AbortChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            string FileName = null;
            System.Uri FileUri = null;
            if (TransactionsDeletedNodes.ContainsKey(theTransaction))
            {
                Dictionary<XElement, XElement> DeletedNodes = TransactionsDeletedNodes[theTransaction] as Dictionary<XElement, XElement>;
                foreach (var dictionaryEntry in DeletedNodes)
                {
                    XElement CurrNode = dictionaryEntry.Key;
                    XElement ParentNode = dictionaryEntry.Value;
                    ParentNode.Add(CurrNode);
                    ObjectID ObjectID = new ObjectID(System.Convert.ToUInt64(CurrNode.Attribute("oid").Value, 10));
                    if (!_ObjectElement.ContainsKey(ObjectID))
                        _ObjectElement.Add(ObjectID, CurrNode);
                }
                if (!_Dirty)
                    return;

                OnObjectStorageChanged();


                FileName = _StorageMetaData.StorageLocation;// _XMLDocument.BaseURI;
                if (FileName.Length == 0)
                    return;

                if (System.Uri.TryCreate(FileName, UriKind.Absolute, out FileUri))
                    FileName = FileUri.LocalPath;

                if (!IsReadOnly)
                    _XMLDocument.SaveToFile(FileName);

            }
            if (TransactionsNodesChanges.ContainsKey(theTransaction))
            {
                Dictionary<XElement, string> OldXmlNodesState = TransactionsNodesChanges[theTransaction];
                foreach (var dictionaryEntry in OldXmlNodesState)
                {
                    XElement CurrNode = dictionaryEntry.Key;
                    XElement oldElement = XElement.Parse(dictionaryEntry.Value);
                    CurrNode.Elements().Remove();
                    foreach (var childElement in oldElement.Elements())
                        CurrNode.Add(XElement.Parse(childElement.ToString()));
                    CurrNode.Attributes().Remove();
                    foreach (var attribute in oldElement.Attributes())
                        CurrNode.SetAttributeValue(attribute.Name, attribute.Value);
                }
            }
            if (TransactionsNewNodes.ContainsKey(theTransaction))
            {
                List<XElement> NewNodes = TransactionsNewNodes[theTransaction];
                foreach (XElement CurrNode in NewNodes)
                {
                    object OidStr = CurrNode.GetAttribute("oid");
                    string tt = OidStr.ToString();
                    ObjectID ObjectID = new ObjectID(System.Convert.ToUInt64(CurrNode.GetAttribute("oid"), 10));
                    if (_ObjectElement.ContainsKey(ObjectID))
                        _ObjectElement.Remove(ObjectID);
                    CurrNode.Remove();
                    //XElement ParentNode = CurrNode.ParentNode;
                    //ParentNode.RemoveChild(CurrNode);
                }
            }
            if (TransactionsNewNodes.ContainsKey(theTransaction))
                TransactionsNewNodes.Remove(theTransaction);
            if (TransactionsNodesChanges.ContainsKey(theTransaction))
                TransactionsNodesChanges.Remove(theTransaction);
            if (TransactionsDeletedNodes.ContainsKey(theTransaction))
                TransactionsDeletedNodes.Remove(theTransaction);
            FileName = _StorageMetaData.StorageLocation;

            _Dirty = false;
            if (FileName.Length == 0)
                return;

            if (System.Uri.TryCreate(FileName, UriKind.Absolute, out FileUri))
                FileName = FileUri.LocalPath;

            if (!IsReadOnly && !IsRawStorageData)
                _XMLDocument.SaveToFile(FileName);

        }
        /// <MetaDataID>{368D1EF7-352F-400A-A166-43C6E3AF2BFB}</MetaDataID>
        public override void MakeChangesDurable(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (!_Dirty)
                return;
            if (IsRawStorageData)
            {
                if (RawStorageData != null)
                    RawStorageData.SaveRawData();
                _Dirty = false;

                OnObjectStorageChanged();
                return;
            }


            if (Uri.IsWellFormedUriString(_StorageMetaData.StorageLocation, UriKind.Absolute))
                throw new System.Exception("Object storage is read only.  StorageLocation : " + _StorageMetaData.StorageLocation);

            string FileName = _StorageMetaData.StorageLocation;
            OnObjectStorageChanged();
            _Dirty = false;

            if (FileName.Length == 0)
                return;
            System.Uri FileUri = null;
            if (System.Uri.TryCreate(FileName, UriKind.Absolute, out FileUri))
                FileName = FileUri.LocalPath;
            _XMLDocument.SaveToFile(FileName);

        }
        /// <MetaDataID>{68CCCBE3-3BFF-4A35-B7CD-764572C9A04E}</MetaDataID>
        public override void CommitChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (TransactionsNewNodes.ContainsKey(theTransaction))
                TransactionsNewNodes.Remove(theTransaction);
            if (TransactionsNodesChanges.ContainsKey(theTransaction))
                TransactionsNodesChanges.Remove(theTransaction);
            if (TransactionsDeletedNodes.ContainsKey(theTransaction))
                TransactionsDeletedNodes.Remove(theTransaction);
            _Dirty = false;

        }
        /// <MetaDataID>{30810491-0389-463D-90B8-B996701B0EE4}</MetaDataID>
        public override void BeginChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

        public override void PrepareForChanges(TransactionContext theTransaction)
        {

        }

        /// <MetaDataID>{E439CCCF-2681-4466-BC0C-16A748545E70}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent deletedStorageInstanceRef, DotNetMetaDataRepository.AssociationEnd AssociationEnd)
        {

            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(deletedStorageInstanceRef.RealStorageInstanceRef))
            {
                Commands.MetaDataUnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.MetaDataUnlinkAllObjectCommand(deletedStorageInstanceRef);
                //mUnlinkAllObjectCommand.DeletedStorageInstance = DeletedStorageInstanceRef;
                mUnlinkAllObjectCommand.theAssociationEnd = AssociationEnd;

                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                transactionContext.EnlistCommand(mUnlinkAllObjectCommand);

                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{00CBFD5F-29A3-4C2F-948E-B6957FE18A4C}</MetaDataID>

        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(storageInstance))
            {
                Commands.MetaDataDeleteStorageInstanceCommand Command = new Commands.MetaDataDeleteStorageInstanceCommand(storageInstance, deleteOption);
                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                if (!transactionContext.ContainCommand(Command.Identity))
                    transactionContext.EnlistCommand(Command);

                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{7e7cd61d-92cb-4f25-bd39-314de43c33ca}</MetaDataID>
        public override void CreateMoveStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstance)
        {
            throw new NotImplementedException();
        }


        /// <exclude>Excluded</exclude>
        private XDocument _XMLDocument;
        /// <MetaDataID>{FA2EA582-05E5-4D9B-969B-2291B88CAAED}</MetaDataID>
        public XDocument XMLDocument
        {
            get
            {
                return _XMLDocument;
            }
            private set
            {
                _XMLDocument = value;
                if (!MetadataLoaded)
                    LoadMetadata();

            }
        }
        /// <exclude>Excluded</exclude>
        private System.Collections.Generic.Dictionary<ObjectID, XElement> _ObjectElement = new System.Collections.Generic.Dictionary<ObjectID, XElement>();
        /// <MetaDataID>{B52015E3-EE8C-4E23-8BD1-3C18030B0CC9}</MetaDataID>
        internal void DeleteStorageInstance(MetaDataStorageInstanceRef StorageInstanceRef)
        {
            XElement mElement = GetXMLElement(StorageInstanceRef.MemoryInstance.GetType(), (ObjectID)StorageInstanceRef.PersistentObjectID);
            mElement.Remove();

            _ObjectElement.Remove(StorageInstanceRef.PersistentObjectID as ObjectID);


        }
        /// <MetaDataID>{EDE8C5E2-9914-446D-898F-B7535CBD792B}</MetaDataID>
        public XElement GetXMLElement(System.Type ClassCollection, ObjectID ObjectID)
        {

            if (ObjectID == null)
            {
                int kk = 0;
            }
            string CollectionName = "";
            if (ClassCollection != null)
                CollectionName = ClassCollection.FullName;


            XElement storageInstance = null;
            if (_ObjectElement != null)
                _ObjectElement.TryGetValue(ObjectID, out storageInstance);
            if (storageInstance != null)
                return (XElement)storageInstance;
            if (_ObjectElement == null)
                _ObjectElement = new System.Collections.Generic.Dictionary<ObjectID, XElement>();
            {

                string StorageName = this.XMLDocument.Root.Name.LocalName;

                //string XQuery = StorageName + "/ObjectCollections/" + CollectionName +
                //    "/Object";//[@oid = "+ObjectID.ToString()+"]";


                var Nodes = XMLDocument.Root.Element("ObjectCollections").Element(CollectionName).Elements();// XMLDocument.Root.SelectNodes(XQuery);

                System.Type ObjectIDType = ObjectID.GetMemberValue("ObjectID").GetType();
                foreach (var CurrNode in Nodes)
                {
                    if (CurrNode.Name != "Object")
                        continue;
                    var StorageInstance = CurrNode;
                    ObjectID elementObjectID = new ObjectID((ulong)System.Convert.ChangeType(StorageInstance.GetAttribute("oid"), ObjectIDType));
                    if (!_ObjectElement.ContainsKey(elementObjectID))
                        _ObjectElement.Add(elementObjectID, StorageInstance);
                }
            }

            storageInstance = null;
            _ObjectElement.TryGetValue(ObjectID, out storageInstance);
            if (storageInstance == null)
            {
                System.Type ObjectIDType = (ObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType();
                string StorageName = _XMLDocument.Root.Name.LocalName;
                //string XQuery = StorageName + "/ObjectCollections/" + CollectionName +
                //    "/Object[@oid = " + ObjectID.ToString() + "]";
                storageInstance = (from element in XMLDocument.Root.Element("ObjectCollections").Element(CollectionName).Elements()
                                   where element.Attribute("oid") != null && element.Attribute("oid").Value == ObjectID.ToString()
                                   select element).FirstOrDefault();

                if (storageInstance != null)
                {
                    ObjectID elementObjectID = new ObjectID((ulong)System.Convert.ChangeType(storageInstance.GetAttribute("oid"), ObjectIDType));
                    _ObjectElement.Add(elementObjectID, storageInstance);
                }
                return storageInstance;
            }
            return storageInstance;
        }
        /// <summary>UpdateStorageInstanceCommand </summary>
        /// <MetaDataID>{B2D7A2D8-AB3D-4891-8482-11C182760B1D}</MetaDataID>
        public override void CreateUpdateStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {

            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(storageInstanceRef))
            {
                Commands.MetaDataUpdateStorageInstanceCommand Command = new Commands.MetaDataUpdateStorageInstanceCommand(storageInstanceRef);
                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                transactionContext.EnlistCommand(Command);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{1abb57e6-2986-4a67-bac9-78419971f999}</MetaDataID>
        System.Collections.Generic.Dictionary<Transactions.Transaction, ulong> TransactionAvailableTempOID = new System.Collections.Generic.Dictionary<OOAdvantech.Transactions.Transaction, ulong>();
        /// <MetaDataID>{73735772-c3a6-4784-87a0-b5638aed2451}</MetaDataID>
        public override PersistenceLayer.ObjectID GetTemporaryObjectID()
        {
            if (Transactions.Transaction.Current == null)
                throw new System.Exception("There isn't transaction");
            lock (this)
            {
                Transactions.Transaction transaction = Transactions.Transaction.Current;
                while (transaction.OriginTransaction != null)
                    transaction = transaction.OriginTransaction;
                ulong ObjectIdentity = 0;
                if (!TransactionAvailableTempOID.TryGetValue(transaction, out ObjectIdentity))
                {
                    ObjectIdentity = 0xFFFFFFFFFFFFFFFF;
                    TransactionAvailableTempOID[transaction] = ObjectIdentity;
                }
                TransactionAvailableTempOID[transaction] = --TransactionAvailableTempOID[transaction];
                return new ObjectID(TransactionAvailableTempOID[transaction]);
            }

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{c797e9a1-52e5-4c6a-b8e6-f376ca588aac}</MetaDataID>
        XElement _ObjectIDNode;
        /// <MetaDataID>{EFB59D45-3338-4497-8986-D3E6AEC35986}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private string StorageName = null;
        /// <MetaDataID>{FB6DAB96-0703-4DC5-BE2A-9D8EA211BC21}</MetaDataID>
        internal XElement ObjectIDNode
        {
            get
            {
                if (_ObjectIDNode == null)
                {
                    foreach (XElement element in _XMLDocument.Root.Elements())
                    {
                        if (element.Name == "NextObjID")
                        {
                            _ObjectIDNode = element;
                            break;
                        }
                    }
                    if (_ObjectIDNode == null)
                        _ObjectIDNode = _XMLDocument.Element(StorageName).Element("NextObjID");

                }
                if (_ObjectIDNode == null)
                    throw (new System.Exception("Bad Storage File: Problem With ObjectIDNode"));
                return _ObjectIDNode;

            }
        }

        internal Type GetObjectCollectionType(string typeFullName)
        {
            var type = (from objectCollectionType in ObjectCollectionNodes.Keys where objectCollectionType.FullName == typeFullName select objectCollectionType).FirstOrDefault();
            return type;
        }
        /// <MetaDataID>{902AB30E-53AF-4F57-AABE-0FC0B0B0C17F}</MetaDataID>
        private System.Collections.Generic.Dictionary<System.Type, XElement> ObjectCollectionNodes = new System.Collections.Generic.Dictionary<Type, XElement>();
        /// <MetaDataID>{183439B6-9079-4487-BADD-CAE0EB98213E}</MetaDataID>
        private XElement mObjectCollections = null;
        /// <MetaDataID>{A67375D3-C5C4-4655-A942-AD6FE17AC634}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
        {
            if (StorageName == null)
                StorageName = _XMLDocument.Root.Name.LocalName;



            XElement objectCollection = null;




            if (!ObjectCollectionNodes.TryGetValue(StorageInstance.MemoryInstance.GetType(), out objectCollection))
            {
                if (mObjectCollections == null)
                    mObjectCollections = _XMLDocument.Element(StorageName).Element("ObjectCollections");
                if (mObjectCollections == null)
                    throw new System.Exception("Bad Storage File: There isn't ObjectCollections.");

                objectCollection = new XElement(StorageInstance.MemoryInstance.GetType().FullName);
                mObjectCollections.Add(objectCollection);
                objectCollection.SetAttribute("ClassInstaditationName", StorageInstance.MemoryInstance.GetType().FullName);
                objectCollection.SetAttribute("AssemblyFullName", StorageInstance.MemoryInstance.GetType().GetMetaData().Assembly.FullName);

                objectCollection.SetAttribute("ClassIdentity", StorageInstance.Class.Identity.ToString());

                ObjectCollectionNodes[StorageInstance.MemoryInstance.GetType()] = objectCollection;
                if (_StorageCells.ContainsKey(StorageInstance.Class))
                {
                    _StorageCells[StorageInstance.Class].XmlElement = objectCollection;
                    var serialNumber = _StorageCells[StorageInstance.Class].SerialNumber;
                }
                else
                {
                    // load storageCell
                    int count = StorageCells.Count;
                }
            }

            Commands.MetaDataNewStorageInstanceCommand aMetaDataNewStorageInstanceCommand = new Commands.MetaDataNewStorageInstanceCommand(StorageInstance as MetaDataStorageInstanceRef);
            aMetaDataNewStorageInstanceCommand.NextObjectIDNode = ObjectIDNode;
            aMetaDataNewStorageInstanceCommand.ObjectCollectionNode = objectCollection;
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(aMetaDataNewStorageInstanceCommand);
        }
        /// <MetaDataID>{9CE3725C-D726-4E24-9D81-F69DF218C249}</MetaDataID>
        private bool MetadataLoaded;
        /// <exclude>Excluded</exclude>
        private bool _Dirty;
        /// <MetaDataID>{9DE9273D-438B-437B-8000-AEADD979D8C2}</MetaDataID>
        public bool Dirty
        {
            set
            {
                _Dirty = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (IsRawStorageData)
                    return RawStorageData.IsReadonly;

                if (Uri.IsWellFormedUriString(_StorageMetaData.StorageLocation, UriKind.Absolute))
                    return true;

                return false;
            }
        }

        /// <MetaDataID>{3b6132d1-943f-4083-b5fb-a94a30934f08}</MetaDataID>
        internal bool IsRawStorageData = false;
        readonly internal IRawStorageData RawStorageData;

        /// <MetaDataID>{EAA8684C-352F-4C36-AC21-0DEBDAE229F0}</MetaDataID>
        public MetaDataStorageSession(string storageName, string storageLocation, XDocument document, IRawStorageData rawStorageData = null)
        {
            RawStorageData = rawStorageData;
            _XMLDocument = document;
            StorageIdentity = document.Root.GetAttribute("StorageIdentity");
            //StorageIdentity = storageIdentity;
            if (StorageIdentity != null)
                StorageIdentity = StorageIdentity.Trim();

            if (string.IsNullOrEmpty(StorageIdentity))
                throw new System.Exception("Invalid StorageIdentity");
            StorageIdentity = document.Root.GetAttribute("StorageIdentity");

            _StorageMetaData = new Storage(StorageIdentity, storageLocation, storageName, typeof(MetaDataStorageProvider).FullName, this.XMLDocument.Root.GetAttribute("Culture"), this);
            Dirty = false;
            MetadataLoaded = false;
            if (RawStorageData == null)
            {

            }
        }
        /// <MetaDataID>{913EDBB1-E0ED-4A9E-884A-2768ED3F163E}</MetaDataID>
        internal void LoadMetadata()
        {
            if (MetadataLoaded)
                return;


            XElement objectCollectionsNode = XMLDocument.Root.Elements().ToArray()[1];
            if (objectCollectionsNode == null)
                throw new System.Exception("Bad XML format");
            if (objectCollectionsNode.Name != "ObjectCollections")
                throw new System.Exception("Bad XML format");

            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.RequiresNew))
            {
                foreach (XElement objectCollection in objectCollectionsNode.Elements())
                {





                    System.Type ObjectCollectionType = ModulePublisher.ClassRepository.GetType(objectCollection.Name.LocalName, objectCollection.GetAttribute("AssemblyFullName"));
                    if (ObjectCollectionType == null)
                    {
                        var assembly = ModulePublisher.ClassRepository.GetAssembly(objectCollection.GetAttribute("AssemblyFullName"));
                        var component = OOAdvantech.DotNetMetaDataRepository.Assembly.GetComponent(assembly);
                        var objectCollectionClass = component.Residents.Where(x => x.Identity.ToString().ToLower() == objectCollection.GetAttribute("ClassIdentity").ToLower()).FirstOrDefault();
                        if (objectCollectionClass != null)
                            ObjectCollectionType = objectCollectionClass.GetExtensionMetaObject<Type>();

                        if (ObjectCollectionType != null)
                        {

                            SetMappedTagName(objectCollection.GetAttribute("ClassIdentity").ToLower(), objectCollection.Name.ToString());

                            var relationElements = objectCollection.Document.Root.Descendants("oid").Where(x => x.Attribute("ClassInstaditationName") != null && x.Attribute("ClassInstaditationName").Value == objectCollection.Name).ToList();

                            objectCollection.Name = ObjectCollectionType.FullName;
                            foreach (var relationElement in relationElements)
                                relationElement.SetAttributeValue("ClassInstaditationName", objectCollection.Name);

                        }
                    }

                    MetaDataRepository.Component AssemblyMetaObject = null;
                    if (ObjectCollectionType == null)
                    {
                        var reflectionAssembly = ModulePublisher.ClassRepository.LoadAssembly(objectCollection.GetAttribute("AssemblyFullName"));

                        DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.Assembly.GetComponent(reflectionAssembly) as DotNetMetaDataRepository.Assembly;
                        //AssemblyMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(reflectionAssembly) as MetaDataRepository.Component;
                        //if (AssemblyMetaObject == null)
                        //{
                        //    DotNetMetaDataRepository.Assembly assembly = new DotNetMetaDataRepository.Assembly(reflectionAssembly);

                        //    DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(reflectionAssembly, assembly);
                        //    long count = assembly.Residents.Count;
                        //}
                        //else
                        //{
                        //    long count = AssemblyMetaObject.Residents.Count;
                        //}
                        long count = assembly.Residents.Count;
                        if (objectCollection.HasAttribute("ClassIdentity"))
                        {
                            var identity = new OOAdvantech.MetaDataRepository.MetaObjectID(objectCollection.GetAttribute("ClassIdentity"));

                            OOAdvantech.MetaDataRepository.Class _class = AssemblyMetaObject.Residents.OfType<OOAdvantech.MetaDataRepository.Class>().Where(x => x.Identity == identity).FirstOrDefault();
                            if (_class != null)
                                ObjectCollectionType = _class.GetExtensionMetaObject<System.Type>();
                        }


                    }
                    else
                    {
                        AssemblyMetaObject = DotNetMetaDataRepository.Assembly.GetComponent(ObjectCollectionType.GetMetaData().Assembly);
                        long count = AssemblyMetaObject.Residents.Count;

                        //DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType.GetMetaData().Assembly) as MetaDataRepository.Component;

                        //if (AssemblyMetaObject == null)
                        //{
                        //    DotNetMetaDataRepository.Assembly assembly = new DotNetMetaDataRepository.Assembly(ObjectCollectionType.GetMetaData().Assembly);

                        //    DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(ObjectCollectionType.GetMetaData().Assembly, assembly);
                        //    long count = assembly.Residents.Count;
                        //}
                        //else
                        //{
                        //    long count = AssemblyMetaObject.Residents.Count;
                        //}
                        //AssemblyMetaObject.Residents.OfType<OOAdvantech.MetaDataRepository.Class>().Where(x => Identity ==
                    }


                    if (ObjectCollectionType == null)
                        throw new OOAdvantech.PersistenceLayer.StorageException("There isn't implementation for class " + objectCollection.Name.LocalName, StorageException.ExceptionReason.StorageMetadataDotNetTypeMismatch);


                    if (!objectCollection.HasAttribute("ClassIdentity"))
                    {
                        objectCollection.SetAttribute("ClassIdentity", OOAdvantech.MetaDataRepository.Classifier.GetClassifier(ObjectCollectionType).Identity.ToString());
                        _Dirty = true;
                    }



                    ObjectCollectionNodes[ObjectCollectionType] = objectCollection;
                    MetaDataRepository.MetaObject ObjectCollectionMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType);
                    if (ObjectCollectionMetaObject == null)
                    {
                        AssemblyMetaObject = DotNetMetaDataRepository.Assembly.GetComponent(ObjectCollectionType.GetMetaData().Assembly);
                        var count = AssemblyMetaObject.Residents.Count;
                        //    = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType.GetMetaData().Assembly) as MetaDataRepository.Component;
                        //if (AssemblyMetaObject == null)
                        //    DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(ObjectCollectionType.GetMetaData().Assembly, new DotNetMetaDataRepository.Assembly(ObjectCollectionType.GetMetaData().Assembly));
                        ObjectCollectionMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType);
                        if (ObjectCollectionMetaObject == null)
                            throw new System.Exception("Fatal error with Storage Meta Data");
                    }


                    //PersistenceLayerRunTime.PersClassObjects mPersClassObjects=OperativeObjectCollections[ObjectCollectionType];
                }

                if (_Dirty)
                {
                    if (RawStorageData != null && !RawStorageData.IsReadonly)
                        MakeChangesDurable(null);
                    else if (RawStorageData == null)
                        MakeChangesDurable(null);
                }
                // load storageCell
                int storageCellsCount = StorageCells.Count;

                stateTransition.Consistent = true;
            }


            MetadataLoaded = true;
        }
        /// <MetaDataID>{53F1B3A9-65B6-4133-B7AA-87491D2A84F4}</MetaDataID>
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            try
            {
                if (_XMLDocument == null)
                    throw new System.Exception("You can't execute any command because the storage session is not initialized properly.");
                if (!MetadataLoaded)
                    LoadMetadata();


                MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
                mStructureSet.Open(OQLStatement, null);
                return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);

            }
            catch (System.Exception error)
            {
                throw;
            }


            //MetaDataStructureSet mStructureSet=new MetaDataStructureSet();
            //mStructureSet.SourceStorageSession=this;
            //mStructureSet.Open(OQLStatement);
            //return mStructureSet;
        }
        /// <MetaDataID>{5E002448-B7C0-423B-87A2-20EE89720232}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID)
        {


            return new MetaDataStorageInstanceRef(memoryInstance, this, objectID);
        }
        /// <MetaDataID>{92131b42-fdea-4adf-8b14-3c8fb345adfc}</MetaDataID>
        public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateOperativeObjects()
        {
            _XMLDocument = XDocument.Load(StorageMetaData.StorageLocation);
            _ObjectElement.Clear();


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {

                foreach (var storageCell in _StorageCells.Values)
                {
                    storageCell.XmlElement = (from storageCellElement in XMLDocument.Root.Element("ObjectCollections").Elements()
                                              where storageCellElement.Attribute("SerialNumber").Value == storageCell.SerialNumber.ToString()
                                              select storageCellElement).First();
                }

                foreach (System.Collections.Generic.KeyValuePair<System.Type, ClassMemoryInstanceCollection> entry in OperativeObjectCollections)
                {

                    ClassMemoryInstanceCollection memoryInstanceCollection = entry.Value;

                    foreach (var weakRef in memoryInstanceCollection.StorageInstanceRefs.Values)
                    {
                        if (weakRef.IsAlive)
                        {
                            MetaDataStorageInstanceRef instanceRef = weakRef.Target as MetaDataStorageInstanceRef;
                            if (instanceRef != null)
                            {
                                instanceRef.UpdateObjectState();
                                if (instanceRef.TheStorageIstance == null)
                                    weakRef.Target = null;
                            }
                        }
                    }

                }
                stateTransition.Consistent = true;
            }

        }
    }
}
