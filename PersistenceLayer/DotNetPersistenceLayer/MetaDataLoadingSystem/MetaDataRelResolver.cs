using System;
using System.Xml.Linq;
using OOAdvantech.Transactions;
using System.Linq;

namespace OOAdvantech.MetaDataLoadingSystem
{
    /// <MetaDataID>{D8ED22E2-343F-4121-8BDC-19947E253CA5}</MetaDataID>
    public class MetaDataRelResolver : PersistenceLayerRunTime.RelResolver
    {
        /// <MetaDataID>{B07580BF-DCCD-4EC9-B97E-1E32F269AF35}</MetaDataID>
        public MetaDataRelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
            : base(theOwner, associationEnd, fastFieldAccessor)
        {

        }


        public override bool Contains(object obj)
        {
            if (obj == null)
                return false;
            if (AssociationEnd.Multiplicity.IsMany)
            {
                if (InternalLoadedRelatedObjects != null && InternalLoadedRelatedObjects.Contains(obj))
                    return true;
                if (!_IsCompleteLoaded)
                {
                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceRef(obj) as PersistenceLayerRunTime.StorageInstanceRef;
                    if (storageInstanceRef == null || storageInstanceRef.PersistentObjectID == null)
                        return false;
                    Load("");
                    //Load(AssociationEnd.Name+" = "+storageInstanceRef.ObjectID.ToString());
                    return InternalLoadedRelatedObjects.Contains(obj);
                }
                else
                {
                    if (InternalLoadedRelatedObjects == null)
                        return false;
                    return InternalLoadedRelatedObjects.Contains(obj);
                }
            }
            else
            {
                return RelatedObject == obj;
            }
        }

        /// <MetaDataID>{BDCE14B9-43D0-4FBD-9261-A73D59DA0419}</MetaDataID>
        public override System.Collections.Generic.List<object> GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
        {

            System.Collections.Generic.List<object> Objects = null;
            if (IsCompleteLoaded)
            {
                if (AssociationEnd.Multiplicity.IsMany)
                {
                    Objects = InternalLoadedRelatedObjects;
                }
                else
                {
                    Objects = new OOAdvantech.Collections.Generic.List<object>();
                    if (RelatedObject != null)
                        Objects.Add(RelatedObject);
                }
            }
            else
                Objects = GetLinkedObjects("");

            object relatedObjects = null;
            object relatedObject = null;

            if (AssociationEnd.Multiplicity.IsMany)
                relatedObjects = FastFieldAccessor.GetValue(Owner.MemoryInstance);
            else
                relatedObject = FastFieldAccessor.GetValue(Owner.MemoryInstance);




            System.Collections.Generic.List<object> StorageInstanceRefs = new System.Collections.Generic.List<object>(Objects.Count);
            foreach (object _objcet in Objects)
                StorageInstanceRefs.Add(PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(_objcet));
            return StorageInstanceRefs;

            //bool SomeRefObjectCantFind = false;
            //OOAdvantech.Collections.Generic.List<object> ObjectCollection = new Collections.Generic.List<object>();
            //MetaDataStorageSession OwnerStorageSession = (MetaDataStorageSession)Owner.ObjectStorage;
            //XElement Element = OwnerStorageSession.GetXMLElement(Owner.MemoryInstance.GetType(), (ObjectID)Owner.PersistentObjectID);

            //#region gets role name with backward computability

            //string roleName = OwnerStorageSession.GetMappedTagName(AssociationEnd.Identity.ToString().ToLower());
            //if (string.IsNullOrWhiteSpace(roleName))
            //{
            //    roleName = AssociationEnd.Name;
            //    if (string.IsNullOrWhiteSpace(roleName))
            //    {
            //        if (AssociationEnd.IsRoleA)
            //            roleName = AssociationEnd.Association.Name + "RoleAName";
            //        else
            //            roleName = AssociationEnd.Association.Name + "RoleBName";
            //    }
            //    OwnerStorageSession.SetMappedTagName(AssociationEnd.Identity.ToString().ToLower(), roleName);
            //}

            //#endregion

            //foreach (XElement CurrElement in Element.Elements())
            //{

            //    /*string RoleName;
            //    if(IsRoleA)
            //        RoleName=RoleAName;
            //    else
            //        RoleName=RoleBName;*/
            //    if (CurrElement != null)
            //    {

            //        //string RelationID = CurrElement.GetAttribute("RelationID");
            //        //if ((!string.IsNullOrEmpty(RelationID) && AssociationEnd.Identity.ToString().ToLower() == RelationID.ToLower()) || CurrElement.Name == RoleName)
            //        if (CurrElement.Name == roleName)
            //        {
            //            //if (string.IsNullOrEmpty(RelationID))
            //            //    CurrElement.SetAttribute("RelationID", AssociationEnd.Identity.ToString());

            //            //if (CurrElement.Name == RoleName)
            //            //{
            //            foreach (XElement RefElement in CurrElement.Elements())
            //            {
            //                //object RefObjectID=System.Convert.ChangeType(RefElement.GetAttribute("oid"),Owner.ObjectID.GetType());

            //                ObjectID RefObjectID = new ObjectID((ulong)System.Convert.ChangeType(RefElement.Value, (Owner.PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
            //                string ClassInstaditationName = RefElement.GetAttribute("ClassInstaditationName");
            //                string assemblyFullName = RefElement.GetAttribute("AssemblyFullName");
            //                System.Type StorageInstanceType = ModulePublisher.ClassRepository.GetType(ClassInstaditationName, assemblyFullName);

            //                //PersistenceLayerRunTime.PersClassObjects ClassObjects=((PersistenceLayerRunTime.StorageSession)OwnerStorageSession).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(ClassInstaditationName,"Version")];

            //                PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)OwnerStorageSession).OperativeObjectCollections[StorageInstanceType][RefObjectID];
            //                if (StorageInstanceRef != null)
            //                {
            //                    ObjectCollection.Add(StorageInstanceRef);
            //                    continue;
            //                }
            //                if (OperativeObjectOnly)
            //                    continue;

            //                XElement mElement = OwnerStorageSession.GetXMLElement(StorageInstanceType, (ObjectID)RefObjectID);
            //                if (mElement == null)
            //                {
            //                    SomeRefObjectCantFind = true;
            //                    continue;
            //                }

            //                object NewObject = AccessorBuilder.CreateInstance(StorageInstanceType);
            //                if (NewObject == null)
            //                    throw new System.Exception("PersistencyService can't instadiate the " + ClassInstaditationName);
            //                StorageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)OwnerStorageSession).CreateStorageInstanceRef(NewObject, RefObjectID);
            //                //StorageInstanceRef.ObjectID=RefObjectID;
            //                ((MetaDataStorageInstanceRef)StorageInstanceRef).TheStorageIstance = mElement;
            //                ((MetaDataStorageInstanceRef)StorageInstanceRef).LoadObjectState();
            //                //StorageInstanceRef.SnapshotStorageInstance();
            //                StorageInstanceRef.ObjectActived();
            //                ObjectCollection.Add(StorageInstanceRef);
            //            }
            //        }
            //    }
            //}
            //return ObjectCollection;
        }


        bool IsCompleteLoadedInitialized;

        public override bool IsCompleteLoaded
        {
            get
            {
                if (Owner.StorageInstanceSet.AllObjectsInActiveMode)
                    if (!_IsCompleteLoaded)
                    {
                        if (IsCompleteLoadedInitialized)
                            return _IsCompleteLoaded;
                        return _IsCompleteLoaded;

                    }
                ///TODO 
                return base.IsCompleteLoaded;
            }
        }

        /// <MetaDataID>{B9C35DA4-DD14-477F-AD51-6D207903CF2F}</MetaDataID>
        public override System.Collections.Generic.List<object> GetLinkedObjects(string criterion)
        {
            ///TODO το criterion δεν δουλεύει

            lock (this)
            {

                try
                {
                    if (_IsCompleteLoaded)
                    {
                        if (InternalLoadedRelatedObjects == null)
                        {
                            // System.Windows.Forms.MessageBox.Show(AssociationEnd.FullName);
                            return new OOAdvantech.Collections.Generic.List<object>();
                        }
                        if (Multilingual)
                        {
                            System.Collections.Generic.List<object> relatedObjects = new OOAdvantech.Collections.Generic.List<object>();

                            foreach (var cultureEntry in MultilingualLoadedRelatedObjects)
                            {
                                foreach (var relatedObject in cultureEntry.Value)
                                {
                                    PersistenceLayerRunTime.MultilingualObjectLink multilingualObjectLink = new PersistenceLayerRunTime.MultilingualObjectLink();
                                    multilingualObjectLink.Culture = cultureEntry.Key;
                                    multilingualObjectLink.LinkedObject = relatedObject;
                                    relatedObjects.Add(multilingualObjectLink);
                                }
                            }
                            return relatedObjects;


                        }
                        else
                            return InternalLoadedRelatedObjects;
                    }
                    bool thereAreUnknownRelatedObjects = false;

                    if (Owner.PersistentObjectID == null)
                        return new OOAdvantech.Collections.Generic.List<object>();



                    System.Collections.Generic.List<SortedObject> ObjectCollection = new System.Collections.Generic.List<SortedObject>();
                    if (Multilingual)
                    {

                    }

                    System.Collections.Generic.List<DotNetMetaDataRepository.AssociationEnd> associationEnds = new System.Collections.Generic.List<OOAdvantech.DotNetMetaDataRepository.AssociationEnd>();
                    associationEnds.Add(AssociationEnd);
                    foreach (DotNetMetaDataRepository.Association association in AssociationEnd.Association.Specializations)
                    {
                        if (AssociationEnd.IsRoleA)
                            if (Owner.Class.IsA(association.RoleA.Namespace as MetaDataRepository.Classifier))
                                associationEnds.Add(association.RoleA as DotNetMetaDataRepository.AssociationEnd);
                        if (!AssociationEnd.IsRoleA)
                            if (Owner.Class.IsA(association.RoleB.Namespace as MetaDataRepository.Classifier))
                                associationEnds.Add(association.RoleB as DotNetMetaDataRepository.AssociationEnd);
                    }
                    foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in associationEnds)
                    {

                        MetaDataStorageSession OwnerStorageSession = (MetaDataStorageSession)Owner.ObjectStorage;
                        XElement element = OwnerStorageSession.GetXMLElement(Owner.MemoryInstance.GetType(), (ObjectID)Owner.PersistentObjectID);

                        #region gets role name with backward computability

                        string roleName = OwnerStorageSession.GetMappedTagName(associationEnd.Identity.ToString().ToLower());
                        if (string.IsNullOrWhiteSpace(roleName))
                        {
                            roleName = associationEnd.Name;
                            if (string.IsNullOrWhiteSpace(roleName))
                            {
                                if (associationEnd.IsRoleA)
                                    roleName = associationEnd.Association.Name + "RoleAName";
                                else
                                    roleName = associationEnd.Association.Name + "RoleBName";
                            }
                            OwnerStorageSession.SetMappedTagName(associationEnd.Identity.ToString().ToLower(), roleName);
                        }

                        #endregion

                        int i = 0;
                        #region walk through value path to reach the element of the role element

                        if (Owner is PersistenceLayerRunTime.StorageInstanceValuePathRef)
                        {
                            MetaDataRepository.MetaObjectID[] path = (Owner as PersistenceLayerRunTime.StorageInstanceValuePathRef).ValueTypePath.ToArray();

                            while (true)
                            {
                                if (i < path.Length)
                                {
                                    MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(path[path.Length - i - 1]);

                                    #region gets attribute name with backward computability

                                    string elementName = OwnerStorageSession.GetMappedTagName(metaObject.Name.ToLower());
                                    if (elementName == null)
                                    {
                                        elementName = metaObject.Name;
                                        OwnerStorageSession.SetMappedTagName(metaObject.Identity.ToString().ToLower(), elementName);
                                    }

                                    #endregion

                                    XElement childElement = element.Element(elementName);
                                    if (childElement == null)
                                        return new OOAdvantech.Collections.Generic.List<object>();
                                    else
                                        element = childElement;
                                    i++;
                                }
                                else
                                    break;
                            }
                        }

                        #endregion


                        if (Multilingual)
                        {
                            if (roleName == "Page")
                            {

                            }
                            XElement roleElement = element.Element(roleName);
                            if (roleElement != null)
                            {
                                foreach (var languageElement in roleElement.Elements().Where(x => x.Name.ToString().ToLower() != "oid")) //child's which aren't oid 
                                {
                                    var culture = System.Globalization.CultureInfo.GetCultureInfo(languageElement.Name.ToString());
                                    using (CultureContext cultureContext = new CultureContext(culture, false))
                                    {
                                        foreach (var refElement in languageElement.Elements("oid"))
                                        {
                                            //Has out storage related object
                                            if (refElement.HasAttribute("StorageCellReference"))
                                                return base.GetLinkedObjects(criterion);
                                        }
                                        GetRoleRelatedObjects(criterion, ref thereAreUnknownRelatedObjects, ObjectCollection, OwnerStorageSession, languageElement);
                                    }
                                }
                                foreach (var refElement in roleElement.Elements("oid"))
                                {
                                    //Has out storage related object
                                    if (refElement.HasAttribute("StorageCellReference"))
                                        return base.GetLinkedObjects(criterion);
                                }
                                GetRoleRelatedObjects(criterion, ref thereAreUnknownRelatedObjects, ObjectCollection, OwnerStorageSession, roleElement);
                            }
                        }
                        else
                        {
                            XElement roleElement = element.Element(roleName);
                            if (roleElement != null)
                            {

                                foreach (var languageElement in roleElement.Elements().Where(x => x.Name.ToString().ToLower() != "oid")) //child's which aren't oid 
                                {
                                    var culture = System.Globalization.CultureInfo.GetCultureInfo(languageElement.Name.ToString());
                                    foreach (var refElement in languageElement.Elements("oid"))
                                    {
                                        //Has out storage related object
                                        if (refElement.HasAttribute("StorageCellReference"))
                                            return base.GetLinkedObjects(criterion);
                                    }
                                    GetRoleRelatedObjects(criterion, ref thereAreUnknownRelatedObjects, ObjectCollection, OwnerStorageSession, languageElement);

                                }


                                foreach (var RefElement in roleElement.Elements())
                                {
                                    //Has out storage related object
                                    if (RefElement.HasAttribute("StorageCellReference"))
                                        return base.GetLinkedObjects(criterion);
                                }
                                GetRoleRelatedObjects(criterion, ref thereAreUnknownRelatedObjects, ObjectCollection, OwnerStorageSession, roleElement);
                            }
                        }
                    }

                    ObjectCollection.Sort(new Sorter());
                    System.Collections.Generic.List<object> collection = new OOAdvantech.Collections.Generic.List<object>();

                    foreach (SortedObject sortedObject in ObjectCollection)
                        collection.Add(sortedObject.LinkedObject);

                    return collection;

                }
                catch (System.Exception error)
                {
                    throw;
                }
                finally
                {
                    //IsCompleteLoaded = true;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterion"></param>
        /// <param name="thereAreUnknownRelatedObjects"></param>
        /// <param name="ObjectCollection"></param>
        /// <param name="OwnerStorageSession"></param>
        /// <param name="roleElement"></param>
        private void GetRoleRelatedObjects(string criterion, ref bool thereAreUnknownRelatedObjects, System.Collections.Generic.List<SortedObject> ObjectCollection, MetaDataStorageSession OwnerStorageSession, XElement roleElement)
        {
            if (AssociationEnd.Indexer)
            {
                if (roleElement.Elements("oid").Select(x => GetIndex(x)).Distinct().Count() != roleElement.Elements("oid").Count())
                {

                }
            }

            var refElements = roleElement.Elements("oid").ToList();
            if (!AssociationEnd.Multiplicity.IsMany && refElements.Count > 1)
            {
                int i = 0;
                foreach (var refElement in refElements)
                {
                    if (i == 0)
                    {
                        ObjectID RefObjectID = new ObjectID((ulong)System.Convert.ChangeType(refElement.Value, (Owner.PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
                        string classInstaditationName = refElement.GetAttribute("ClassInstaditationName");
                        string assemblyFullName = refElement.GetAttribute("AssemblyFullName");
                        System.Type StorageInstanceType = ModulePublisher.ClassRepository.GetType(classInstaditationName, assemblyFullName);
                        MetaDataStorageInstanceRef storageInstanceRef = OwnerStorageSession.GetStorageInstanceRef(StorageInstanceType, RefObjectID);
                        if(storageInstanceRef==null)
                            refElement.Remove();
                        else
                            i++;
                    }
                    else
                    {
                        refElement.Remove();
                        i++;
                    }
                    

                }
                refElements = roleElement.Elements("oid").ToList();
            }

            foreach (var refElement in refElements.ToList())
            {
                if (refElement.Name != "oid")
                {

                }
                ObjectID RefObjectID = new ObjectID((ulong)System.Convert.ChangeType(refElement.Value, (Owner.PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
                string classInstaditationName = refElement.GetAttribute("ClassInstaditationName");
                string assemblyFullName = refElement.GetAttribute("AssemblyFullName");
                System.Type StorageInstanceType = ModulePublisher.ClassRepository.GetType(classInstaditationName, assemblyFullName);
                MetaDataStorageInstanceRef storageInstanceRef = OwnerStorageSession.GetStorageInstanceRef(StorageInstanceType, RefObjectID);

                int sort = -1;
                int.TryParse(refElement.GetAttribute("Sort"), out sort);
                //PersistenceLayerRunTime.PersClassObjects ClassObjects=((PersistenceLayerRunTime.StorageSession)OwnerStorageSession).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(ClassInstaditationName,"Version")];
                SortedObject sortedObject;
                
                if (storageInstanceRef == null)
                {
                    thereAreUnknownRelatedObjects = true;
                    refElement.Remove();
                    continue;
                }
                else
                {
                    if (AssociationEnd.Multiplicity.IsMany && AssociationEnd.GetOtherEnd().Navigable && !AssociationEnd.GetOtherEnd().Multiplicity.IsMany)
                    {
                        var otherEndRelationResolver = storageInstanceRef.RelResolvers.Where(x => x.AssociationEnd == AssociationEnd.GetOtherEnd()).FirstOrDefault();
                        if (otherEndRelationResolver != null)
                        {
                            var relateObj = otherEndRelationResolver.RelatedObject;
                            if(Owner.MemoryInstance!=relateObj)
                            {

                            }
                        }
                    }


                    sortedObject.index = sort;
                    if (Multilingual)
                    {
                        PersistenceLayerRunTime.MultilingualObjectLink multiligualObjectLink = new PersistenceLayerRunTime.MultilingualObjectLink();
                        multiligualObjectLink.Culture = CultureContext.CurrentNeutralCultureInfo;
                        multiligualObjectLink.LinkedObject = storageInstanceRef.MemoryInstance;
                        sortedObject.LinkedObject = multiligualObjectLink;
                    }
                    else
                    {
                        sortedObject.LinkedObject = storageInstanceRef.MemoryInstance;
                    }
                    ObjectCollection.Add(sortedObject);
                }
            }

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

        /// <MetaDataID>{E3A06187-30C9-44D7-982F-6FF8DCBE46B3}</MetaDataID>
        public override long GetLinkedObjectsCount()
        {
            if (!_IsCompleteLoaded)
                CompleteLoad();
            return InternalLoadedRelatedObjects.Count;
        }

        internal void UpdateState()
        {


            this.InternalRelatedObject = null;
            InternalLoadedRelatedObjects = null;
            _IsCompleteLoaded = false;
            InvalidateCount();

        }
    }
    /// <MetaDataID>{f4cd7ba2-320d-4d31-8bca-db1d2207285f}</MetaDataID>
    struct SortedObject
    {
        public int index;
        public object LinkedObject;
    }
    /// <MetaDataID>{7ec47139-db3f-4458-9f44-3764ad82d9ed}</MetaDataID>
    class Sorter : System.Collections.Generic.IComparer<SortedObject>
    {
        public int Compare(SortedObject x, SortedObject y)
        {
            return x.index.CompareTo(y.index);
        }
    }
}
