using System.Xml.Linq;
using System.Linq;
using OOAdvantech.Transactions;
using System.Globalization;
using OOAdvantech.PersistenceLayerRunTime;

namespace OOAdvantech.MetaDataLoadingSystem
{
    /// <MetaDataID>{853BDF35-81AD-4CC4-A0B2-D5631621DBDA}</MetaDataID>
    public class MetaDataStorageInstanceRef : PersistenceLayerRunTime.StorageInstanceRef
    {
        public MetaDataStorageInstanceRef(object memoryInstance, MetaDataStorageSession activeStorageSession, PersistenceLayer.ObjectID objectID)
            : base(memoryInstance, activeStorageSession,null, objectID)
        {

        }
        MetaDataStorageInstanceRef()
        {

        }


        internal static object GetValueTypeValue(OOAdvantech.MetaDataRepository.Attribute member, XElement theXmlElement, string columnPrefix, string columnsNameSuffix, StorageCell storageInstanceSet)
        {
            System.Type type = member.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            object obj = AccessorBuilder.GetDefaultValue(type);
            //obj= type.Assembly.CreateInstance(type.FullName);

            MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
            valueTypePath.Push(member.Identity);
            object FieldValue = null;
            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData(member.Type as DotNetMetaDataRepository.Structure, valueTypePath, member.Name))
            {
                string FieldName = (storageInstanceSet.Namespace as Storage).ObjectStorage.GetMappedTagName(valueOfAttribute.Attribute.Identity.ToString());
                string Value = theXmlElement.GetAttribute(FieldName);
                if (Value.Length > 0)
                {
                    if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() != Value.GetType())
                    {

                        if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>().GetMetaData().BaseType == typeof(System.Enum))
                        {
                            if (Value != null)
                                if (Value.Length > 0)
                                {
                                    FieldValue = System.Enum.Parse(valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>(), Value);
                                    if (FieldValue.ToString() != Value)
                                        theXmlElement.SetAttribute(FieldName, FieldValue.ToString());
                                }
                        }
                        else
                            FieldValue = System.Convert.ChangeType(Value, valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>(), CultureInfoHelper.GetCultureInfo(1033));
                    }
                    else
                        FieldValue = Value;
                }
                bool valueSetted = false;
                SetAttributeValue(ref obj, new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, FieldValue, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null), member.Type as DotNetMetaDataRepository.Structure, 0, out valueSetted);
            }

            foreach (var associationEnd in member.Type.GetAssociateRoles(true))
            {
                if (associationEnd.Persistent)
                {
                    System.Collections.Generic.List<SortedObject> ObjectCollection = new System.Collections.Generic.List<SortedObject>();

                    #region gets role name with backward computability

                    string roleName = (storageInstanceSet.Namespace as Storage).ObjectStorage.GetMappedTagName(associationEnd.Identity.ToString().ToLower());
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
                        (storageInstanceSet.Namespace as Storage).ObjectStorage.SetMappedTagName(associationEnd.Identity.ToString().ToLower(), roleName);
                    }
                    #endregion

                    foreach (XElement currElement in theXmlElement.Elements())
                    {
                        //string RoleName = AssociationEnd.Name;
                        //if (RoleName == null || RoleName.Trim().Length == 0)
                        //    if (AssociationEnd.IsRoleA)
                        //        RoleName = AssociationEnd.Association.Name + "RoleAName";
                        //    else
                        //        RoleName = AssociationEnd.Association.Name + "RoleBName";

                        /*string RoleName;
                        if(IsRoleA)
                            RoleName=RoleAName;
                        else
                            RoleName=RoleBName;*/
                        if (currElement != null)
                        {
                            // string RelationID = CurrElement.GetAttribute("RelationID");

                            //if ((!string.IsNullOrEmpty(RelationID) && AssociationEnd.Identity.ToString().ToLower() == RelationID.ToLower()) || CurrElement.Name == RoleName)
                            if (currElement.Name == roleName)
                            {
                                //if (string.IsNullOrEmpty(RelationID))
                                //    CurrElement.SetAttribute("RelationID", AssociationEnd.Identity.ToString());

                                foreach (XElement RefElement in currElement.Elements())
                                {
                                    if (RefElement.Attribute("StorageCellReference") != null)
                                        continue;
                                    //object RefObjectID=System.Convert.ChangeType(RefElement.GetAttribute("oid"),Owner.ObjectID.GetType());
                                    ObjectID RefObjectID = new ObjectID((ulong)System.Convert.ChangeType(RefElement.Value, OOAdvantech.MetaDataLoadingSystem.ObjectID.XMLObjectIdentityType.Parts[0].Type));
                                    string classInstaditationName = RefElement.GetAttribute("ClassInstaditationName");
                                    string assemblyFullName = RefElement.GetAttribute("AssemblyFullName");

                                    System.Type StorageInstanceType = ModulePublisher.ClassRepository.GetType(classInstaditationName, assemblyFullName);
                                    int sort = -1;
                                    int.TryParse(RefElement.GetAttribute("Sort"), out sort);
                                    //PersistenceLayerRunTime.PersClassObjects ClassObjects=((PersistenceLayerRunTime.StorageSession)OwnerStorageSession).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(ClassInstaditationName,"Version")];
                                    SortedObject sortedObject;
                                    PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)(storageInstanceSet.Namespace as Storage).ObjectStorage).OperativeObjectCollections[StorageInstanceType][RefObjectID];
                                    if (StorageInstanceRef != null)
                                    {

                                        sortedObject.index = sort;
                                        sortedObject.LinkedObject = StorageInstanceRef.MemoryInstance;
                                        ObjectCollection.Add(sortedObject);
                                        continue;
                                    }
                                    XElement mElement = (storageInstanceSet.Namespace as Storage).ObjectStorage.GetXMLElement(StorageInstanceType, RefObjectID);
                                    if (mElement == null)
                                    {

                                        continue;
                                    }

                                    object NewObject = AccessorBuilder.CreateInstance(StorageInstanceType);
                                    if (NewObject == null)
                                        throw new System.Exception("PersistencyService can't instadiate the " + classInstaditationName);
                                    StorageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)(storageInstanceSet.Namespace as Storage).ObjectStorage).CreateStorageInstanceRef(NewObject, RefObjectID);
                                    //StorageInstanceRef.ObjectID=RefObjectID;
                                    ((MetaDataStorageInstanceRef)StorageInstanceRef).TheStorageIstance = mElement;
                                    ((MetaDataStorageInstanceRef)StorageInstanceRef).LoadObjectState();
                                    //StorageInstanceRef.SnapshotStorageInstance();
                                    StorageInstanceRef.ObjectActived();


                                    sortedObject.index = sort;
                                    sortedObject.LinkedObject = NewObject;

                                    ObjectCollection.Add(sortedObject);
                                }
                            }
                        }
                    }
                    if (ObjectCollection.Count > 0)
                    {
                        if (!associationEnd.Multiplicity.IsMany)
                        {
                            if (ObjectCollection.Count > 1)
                                System.Diagnostics.Debug.WriteLine(associationEnd.Name + ": Role Constraints mismatch");

                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (member.Type as DotNetMetaDataRepository.Structure).GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref obj, ObjectCollection[0].LinkedObject);
                        }
                    }


                }
            }
            return obj;
        }





        /// <MetaDataID>{13fc2924-e45c-4e53-b92c-0145867a6930}</MetaDataID>
        static StorageCell StorageCell;
        public override OOAdvantech.MetaDataRepository.StorageCell StorageInstanceSet
        {
            get
            {

                return (this.ObjectStorage as MetaDataStorageSession).GetStorageCell(Class);

            }
        }

        /// <MetaDataID>{3A80A74E-4EE7-4F6D-B6E3-0ED041716120}</MetaDataID>
        public void Delete()
        {

        }
        /// <MetaDataID>{8D7A3076-F891-408F-9FC5-FA24C4D970E9}</MetaDataID>
        protected override PersistenceLayerRunTime.RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
        {
            //	MetaDataRelResolver mResolver =new MetaDataRelResolver(this,thePersistentAssociationEnd);
            return new MetaDataRelResolver(this, thePersistentAssociationEnd, fastFieldAccessor);

            //return mResolver;
        }
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new MetaDataRelResolver(owner, thePersistentAssociationEnd, fastFieldAccessor);
        }
        /// <MetaDataID>{CDD97EAB-1F31-48D9-B114-68F215C42923}</MetaDataID>
        public XElement TheStorageIstance;
        /// <MetaDataID>{11E6CAE1-7BD7-4034-8B34-EB9F07624B0F}</MetaDataID>
        public void LoadObjectState()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                try
                {
                    XElement instanceElement = TheStorageIstance;
                    string RefIntgrCountStr = instanceElement.GetAttribute("ReferentialIntegrityCount");
                    if (RefIntgrCountStr.Length > 0)
                    {
                        _ReferentialIntegrityCount = (int)System.Convert.ChangeType(RefIntgrCountStr, typeof(int));
                        _RuntimeReferentialIntegrityCount.Value = (decimal)_ReferentialIntegrityCount;
                    }
                    // GetPersistentAttributeMetaData(DotNetMetaDataRepository.Class _class)


                    // foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues())
                    foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData(Class))
                    {
                        instanceElement = TheStorageIstance;
                        string fieldName = GetMemberName(valueOfAttribute.Attribute);
                        bool exist = false;
                        if (valueOfAttribute.ValueTypePath.Count >= 2)
                        {

                            MetaDataRepository.MetaObjectID[] path = valueOfAttribute.ValueTypePath.ToArray();
                            for (int i = 0; i != path.Length - 1; i++)
                            {
                                MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(path[path.Length - i - 1]);
                                string memberName = GetMemberName(metaObject);
                                exist = false;
                                XElement xmlElement = instanceElement.Element(memberName);
                                if (xmlElement != null)
                                {
                                    instanceElement = xmlElement;
                                    exist = true;
                                }
                                else
                                    break;
                            }
                            if (!exist)
                                continue;
                        }
                        if (valueOfAttribute.IsMultilingual)
                        {
                            var multilingualElement = instanceElement.Element(fieldName);
                            if (multilingualElement != null)
                            {
                                foreach (XElement cultureInfoElement in multilingualElement.Elements())
                                    LoadAttributeValue(cultureInfoElement, valueOfAttribute, null);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(ObjectStorage.StorageMetaData.Culture))
                                {
                                    using (CultureContext cultureContext = new CultureContext(CultureInfo.GetCultureInfo(ObjectStorage.StorageMetaData.Culture), false))
                                    {
                                        LoadAttributeValue(instanceElement, valueOfAttribute, fieldName);
                                    }
                                }
                                else
                                    LoadAttributeValue(instanceElement, valueOfAttribute, fieldName);
                            }
                            var multilingualFieldValue = GetAttributeValue(valueOfAttribute);
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, multilingualFieldValue);
                        }
                        else
                        {
                            try
                            {
                                LoadAttributeValue(instanceElement, valueOfAttribute, fieldName);
                            }
                            catch (System.Exception error)
                            {
                                throw;
                            }
                        }
                    }

                    foreach (RelResolver relResolver in RelResolvers)
                    {
                        RelResolver orgRelResolver = relResolver;
                        DotNetMetaDataRepository.AssociationEnd assoctiationHierarchyAssociationEnd = orgRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;

                        //Load relation resolver related object in for all association in  association hierarchy
                        do
                        {
                            if (assoctiationHierarchyAssociationEnd.Multiplicity.IsMany && orgRelResolver.ValueTypePath.Count == 0)
                                break;

                            //Loads related objects for value type or zero or one multiplicity association end
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(assoctiationHierarchyAssociationEnd);
                            string fieldName;
                            if (orgRelResolver.AssociationEnd.PropertyMember != null)
                                fieldName = orgRelResolver.AssociationEnd.PropertyMember.Name;
                            else
                                fieldName = fastFieldAccessor.MemberInfo.Name;

                            if (!orgRelResolver.Owner.Class.IsLazyFetching(orgRelResolver.AssociationEnd) || (orgRelResolver.FieldInfo.DeclaringType.IsValueType && orgRelResolver.ValueTypePath.Count > 0/*&& assoctiationHierarchyAssociationEnd.Multiplicity.IsMany */ ))
                            {
                                var LinkedObjects = orgRelResolver.GetLinkedObjects("");
                                //Gets relation resolver of this level in association hierarchy
                                PersistenceLayerRunTime.RelResolver assoctiationHierarchyRelResolver = orgRelResolver;
                                if (assoctiationHierarchyAssociationEnd != orgRelResolver.AssociationEnd)
                                {
                                    foreach (PersistenceLayerRunTime.RelResolver theRelatioResolver in RelResolvers)
                                    {
                                        if (theRelatioResolver.AssociationEnd == assoctiationHierarchyAssociationEnd)
                                        {
                                            assoctiationHierarchyRelResolver = theRelatioResolver;
                                            break;
                                        }
                                    }
                                }
                                if (orgRelResolver.Multilingual)
                                {
                                    if (LinkedObjects.Count > 0)
                                    {
                                        if ((assoctiationHierarchyAssociationEnd.Multiplicity.IsMany && orgRelResolver.FieldInfo.DeclaringType.IsValueType && orgRelResolver.ValueTypePath.Count > 0))
                                        {
                                            //only value type many relation loaded
                                            LoadMultilingualCollection(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                        }
                                        else
                                        {
                                            //only zero or one multiplicity loaded
                                            if (Class.HasReferentialIntegrity(orgRelResolver.AssociationEnd) || (orgRelResolver.AssociationEnd.Navigable && orgRelResolver.AssociationEnd.GetOtherEnd().Navigable))
                                            {
                                                InitializeRelatedObject(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                                SetMultilingualMemberValue(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                            }
                                            else
                                            {
                                                foreach (PersistenceLayerRunTime.MultilingualObjectLink multiligualObjectLink in LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>())
                                                {
                                                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(multiligualObjectLink.LinkedObject) as PersistenceLayerRunTime.StorageInstanceRef;
                                                    storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(orgRelResolver.OnObjectDeleted);
                                                }
                                                InitializeRelatedObject(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                                SetMultilingualMemberValue(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LinkedObjects.Count > 1 )
                                        System.Diagnostics.Debug.WriteLine(fieldName + ": Role Constraints mismatch");


                                    if ((assoctiationHierarchyAssociationEnd.Multiplicity.IsMany && orgRelResolver.FieldInfo.DeclaringType.IsValueType && orgRelResolver.ValueTypePath.Count > 0))
                                    {
                                        //only value type many relation loaded
                                        orgRelResolver.CompleteLoad(LinkedObjects);
                                        PersistenceLayer.ObjectContainer objectCollection = GetMemoryInstanceMemberValue(orgRelResolver) as PersistenceLayer.ObjectContainer;
                                        foreach (object @object in LinkedObjects)
                                            (GetObjectCollection(objectCollection) as OnMemoryObjectCollection).Add(@object, true);
                                    }
                                    else
                                    {
                                        //only zero or one multiplicity loaded
                                        object linkedObject = LinkedObjects.FirstOrDefault();
                                        if (linkedObject != null)
                                        {
                                            //TODO: εάν το type του association end είναι interface τότε υπάρχει πρόβλημα  
                                            if (Class.HasReferentialIntegrity(orgRelResolver.AssociationEnd) || (orgRelResolver.AssociationEnd.Navigable && orgRelResolver.AssociationEnd.GetOtherEnd().Navigable))
                                            {
                                                InitializeRelatedObject(assoctiationHierarchyRelResolver, linkedObject);
                                                SetMemberValue(assoctiationHierarchyRelResolver, linkedObject);
                                            }
                                            else
                                            {
                                                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(linkedObject) as PersistenceLayerRunTime.StorageInstanceRef;
                                                storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(orgRelResolver.OnObjectDeleted);
                                                InitializeRelatedObject(assoctiationHierarchyRelResolver, linkedObject);
                                                SetMemberValue(assoctiationHierarchyRelResolver, linkedObject);
                                            }
                                        }
                                    }
                                }
                            }

                            //Get on step up in association hierarchy associationEnd
                            if (assoctiationHierarchyAssociationEnd.IsRoleA && assoctiationHierarchyAssociationEnd.Association.General != null)
                                assoctiationHierarchyAssociationEnd = assoctiationHierarchyAssociationEnd.Association.General.RoleA as DotNetMetaDataRepository.AssociationEnd;
                            else if (!assoctiationHierarchyAssociationEnd.IsRoleA && assoctiationHierarchyAssociationEnd.Association.General != null)
                                assoctiationHierarchyAssociationEnd = assoctiationHierarchyAssociationEnd.Association.General.RoleB as DotNetMetaDataRepository.AssociationEnd;
                            else
                                assoctiationHierarchyAssociationEnd = null;
                        }
                        while (assoctiationHierarchyAssociationEnd != null);
                        assoctiationHierarchyAssociationEnd = orgRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
                    }


                    if (Class.ClassHierarchyLinkAssociation != null)
                    {
                        //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                        ObjectID RoleARefObjectID = new ObjectID((ulong)System.Convert.ChangeType(instanceElement.GetAttribute("RoleA"), (PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
                        string RoleAClassInstaditationName = instanceElement.GetAttribute("RoleAClassInstaditationName");

                        System.Type roleAClassInstaditationType = (ObjectStorage as OOAdvantech.MetaDataLoadingSystem.MetaDataStorageSession).GetObjectCollectionType(RoleAClassInstaditationName);
                        PersistenceLayerRunTime.StorageInstanceRef RoleAStorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[roleAClassInstaditationType][RoleARefObjectID];

                        ObjectID RoleBRefObjectID = new ObjectID((ulong)System.Convert.ChangeType(instanceElement.GetAttribute("RoleB"), (PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
                        string RoleBClassInstaditationName = instanceElement.GetAttribute("RoleBClassInstaditationName");
                        System.Type roleBClassInstaditationType = (ObjectStorage as OOAdvantech.MetaDataLoadingSystem.MetaDataStorageSession).GetObjectCollectionType(RoleBClassInstaditationName);
                        PersistenceLayerRunTime.StorageInstanceRef RoleBStorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[roleBClassInstaditationType][RoleBRefObjectID];


                        if (RoleAStorageInstanceRef == null)
                        {

                            object NewObject = AccessorBuilder.CreateInstance(roleAClassInstaditationType);

                            if (NewObject == null)
                                throw new System.Exception("PersistencyService can't instadiate the " + RoleAClassInstaditationName);
                            XElement mElement = ((MetaDataStorageSession)ObjectStorage).GetXMLElement(NewObject.GetType(), (ObjectID)RoleARefObjectID);
                            RoleAStorageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, RoleARefObjectID);
                            //RoleAStorageInstanceRef.ObjectID=RoleARefObjectID;
                            ((MetaDataStorageInstanceRef)RoleAStorageInstanceRef).TheStorageIstance = mElement;
                            ((MetaDataStorageInstanceRef)RoleAStorageInstanceRef).LoadObjectState();
                            //RoleAStorageInstanceRef.SnapshotStorageInstance();
                            RoleAStorageInstanceRef.ObjectActived();
                        }

                        if (RoleBStorageInstanceRef == null)
                        {

                            object NewObject = AccessorBuilder.CreateInstance(roleBClassInstaditationType);
                            if (NewObject == null)
                                throw new System.Exception("PersistencyService can't instadiate the " + RoleBClassInstaditationName);
                            XElement mElement = ((MetaDataStorageSession)ObjectStorage).GetXMLElement(NewObject.GetType(), (ObjectID)RoleBRefObjectID);
                            RoleBStorageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, RoleBRefObjectID);
                            //RoleBStorageInstanceRef.ObjectID=RoleBRefObjectID;
                            ((MetaDataStorageInstanceRef)RoleBStorageInstanceRef).TheStorageIstance = mElement;
                            ((MetaDataStorageInstanceRef)RoleBStorageInstanceRef).LoadObjectState();
                            //RoleBStorageInstanceRef.SnapshotStorageInstance();
                            RoleBStorageInstanceRef.ObjectActived();
                        }


                        System.Reflection.FieldInfo FieldRoleA = Class.LinkClassRoleAField;
                        object memoryInstance = MemoryInstance;
                        //FieldRoleA.SetValue(MemoryInstance, RoleAStorageInstanceRef.MemoryInstance);
                        Member<object>.SetValueImplicitly(Class.LinkClassRoleAFastFieldAccessor, ref memoryInstance, RoleAStorageInstanceRef.MemoryInstance);

                        System.Reflection.FieldInfo FieldRoleB = Class.LinkClassRoleBField;
                        //FieldRoleB.SetValue(MemoryInstance, RoleBStorageInstanceRef.MemoryInstance);
                        Member<object>.SetValueImplicitly(Class.LinkClassRoleBFastFieldAccessor, ref memoryInstance, RoleBStorageInstanceRef.MemoryInstance);

                    }
                }
                catch (System.Exception error)
                {
                    throw new System.Exception(error.Message, error);
                    int gg = 0;
                }
                stateTransition.Consistent = true;
            }

        }

        private string GetMemberName(MetaDataRepository.MetaObject metaObject)
        {
            string memberName = (this._ObjectStorage as MetaDataStorageSession).GetMappedTagName(metaObject.Identity.ToString());
            if (string.IsNullOrEmpty(memberName))
            {
                memberName = metaObject.Name;
                (this._ObjectStorage as MetaDataStorageSession).SetMappedTagName(metaObject.Identity.ToString(), metaObject.Name);
            }

            return memberName;
        }

        private void LoadAttributeValue(XElement theXmlElement, ValueOfAttribute valueOfAttribute, string fieldName)
        {
            System.Globalization.CultureInfo cultureInfo = null;
            if (valueOfAttribute.IsMultilingual && fieldName == null)
                cultureInfo = new CultureInfo(theXmlElement.Name.ToString());
            else
                cultureInfo = CultureContext.CurrentCultureInfo;


            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = valueOfAttribute.FastFieldAccessor;
#if !DeviceDotNet
            if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() == typeof(System.Xml.XmlDocument))
            {

                XElement XMLFieldNode = null;
                if (fieldName != null)
                    XMLFieldNode = theXmlElement.Element(fieldName);
                else
                    XMLFieldNode = theXmlElement;

                //foreach (XElement CurrNode in theXmlElement.Elements())
                //{
                //    if (fieldName == CurrNode.Name)
                //    {
                //        XMLFieldNode = CurrNode;
                //        break;
                //    }
                //}
                if (XMLFieldNode == null)
                {
                    if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                    {
                        using (var cultureContext = new CultureContext(cultureInfo, false))
                        {
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, null);
                        }
                    }
                    else
                    {
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, null);
                    }
                }
                else
                {
                    //System.Xml.XmlDocument theXmlDocument=(System.Xml.XmlDocument)CurrFieldInfo.GetValue(MemoryInstance);
                    System.Xml.XmlDocument theXmlDocument = (System.Xml.XmlDocument)Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                    if (theXmlDocument == null)
                    {
                        theXmlDocument = new System.Xml.XmlDocument();
                        theXmlDocument.InnerXml = XMLFieldNode.Value;
                        if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                        {
                            using (var cultureContext = new CultureContext(cultureInfo, false))
                            {
                                SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, theXmlDocument, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo));
                                SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                            }
                        }
                        else
                        {
                            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, theXmlDocument, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo));
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                        }

                    }
                    else
                    {
                        theXmlDocument.InnerXml = XMLFieldNode.Value;
                        if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                        {
                            using (var cultureContext = new CultureContext(cultureInfo, false))
                            {
                                SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                            }
                        }
                        else
                        {
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                        }
                    }
                }
            }
            else
#endif
            if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() == typeof(XDocument))
            {
                #region Gets xml document 
                XElement XMLFieldNode = null;
                if (fieldName != null)
                    XMLFieldNode = theXmlElement.Element(fieldName);
                else
                    XMLFieldNode = theXmlElement;


                if (XMLFieldNode == null)
                {
                    if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                    {
                        using (var cultureContext = new CultureContext(cultureInfo, false))
                        {
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, null);
                        }
                    }
                    else
                    {
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, null);
                    }
                }
                else
                {
                    XDocument theXmlDocument = (XDocument)Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                    if (theXmlDocument == null)
                    {
                        if (XMLFieldNode != null)
                            theXmlDocument = XDocument.Parse(XMLFieldNode.ToString());
                        if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                        {
                            using (var cultureContext = new CultureContext(cultureInfo, false))
                            {
                                SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, theXmlDocument, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo));
                                SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                            }
                        }
                        else
                        {
                            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, theXmlDocument, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo));
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                        }
                    }
                    else
                    {
                        if (XMLFieldNode == null)
                            theXmlDocument = null;
                        else
                            theXmlDocument = XDocument.Parse(XMLFieldNode.ToString());
                        if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                        {
                            using (var cultureContext = new CultureContext(cultureInfo, false))
                            {
                                SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                            }
                        }
                        else
                        {
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, theXmlDocument);
                        }
                    }
                }
                #endregion
            }
            else
            {
                if (valueOfAttribute.IsMultilingual)
                {
                }
                if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() == typeof(System.DateTime))
                {
                }

                string Value = null;
                if (fieldName != null)
                    Value = theXmlElement.GetAttribute(fieldName);
                else
                    Value = theXmlElement.Value;

                if (!string.IsNullOrEmpty(Value))
                {
                    object FieldValue = null;
                    FieldValue = ParseValueOfAttribute(valueOfAttribute, Value);

                    if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>().GetMetaData().BaseType == typeof(System.Enum) && FieldValue.ToString() != Value)
                    {
                        if (fieldName != null)
                            theXmlElement.SetAttribute(fieldName, FieldValue.ToString());
                        else
                            theXmlElement.Value = FieldValue.ToString();
                    }
                    //CurrFieldInfo.SetValue(MemoryInstance, FieldValue);
                    if (valueOfAttribute.IsMultilingual && cultureInfo != null)
                    {
                        using (var cultureContext = new CultureContext(cultureInfo, false))
                        {
                            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, FieldValue, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo));
                        }

                        var multilingualValue = GetAttributeValue(valueOfAttribute);
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, multilingualValue);
                    }
                    else
                    {
                        SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, FieldValue, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo));
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, FieldValue);
                    }

                    //Member<object>.SetValue(fastFieldAccessor, MemoryInstance, FieldValue);
                }
                else
                {
                    if (valueOfAttribute.IsMultilingual)
                    {
                        var multilingualValue = GetAttributeValue(valueOfAttribute);
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, multilingualValue);
                    }
                    else
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, AccessorBuilder.GetDefaultValue(valueOfAttribute.FieldInfo.FieldType));
                }
            }
        }

        private static object ParseValueOfAttribute(ValueOfAttribute valueOfAttribute, string Value)
        {
            object FieldValue;
            if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() != Value.GetType())
            {
                if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() == typeof(System.DateTime))
                {
                    System.IFormatProvider format = CultureInfoHelper.GetCultureInfo(0x0409);
                    FieldValue = System.DateTime.Parse(Value, format);

                }
                else if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>().GetMetaData().BaseType == typeof(System.Enum))
                    FieldValue = System.Enum.Parse(valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>(), Value);
                else if (valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() == typeof(System.Guid))
                    FieldValue = System.Guid.Parse(Value);
                else
                    FieldValue = System.Convert.ChangeType(Value, valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>(), CultureInfoHelper.GetCultureInfo(1033));
            }
            else
                FieldValue = Value;
            return FieldValue;
        }

        public void UpdateObjectState()
        {
            using (ObjectStateTransition innerStateTransition = new ObjectStateTransition(this))
            {
                XElement instanceElement = (StorageInstanceSet.Namespace as Storage).ObjectStorage.GetXMLElement(MemoryInstance.GetType(), ObjectID as ObjectID);

                TheStorageIstance = instanceElement;
                if (TheStorageIstance == null)
                    return;


                string RefIntgrCountStr = instanceElement.GetAttribute("ReferentialIntegrityCount");
                if (RefIntgrCountStr.Length > 0)
                {
                    _ReferentialIntegrityCount = (int)System.Convert.ChangeType(RefIntgrCountStr, typeof(int));
                    _RuntimeReferentialIntegrityCount.Value = (decimal)_ReferentialIntegrityCount;
                }

                foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues())
                {
                    instanceElement = TheStorageIstance;
                    string fieldName = GetMemberName(valueOfAttribute.Attribute);
                    bool exist = false;
                    if (valueOfAttribute.ValueTypePath.Count >= 2)
                    {
                        MetaDataRepository.MetaObjectID[] path = valueOfAttribute.ValueTypePath.ToArray();
                        for (int i = 0; i != path.Length - 1; i++)
                        {
                            MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(path[path.Length - i - 1]);
                            string memberName = GetMemberName(metaObject);
                            exist = false;
                            XElement xmlElement = instanceElement.Element(memberName);
                            if (xmlElement != null)
                            {
                                instanceElement = xmlElement;
                                exist = true;
                            }
                            else
                                break;
                        }
                        if (!exist)
                            continue;
                    }
                    if (valueOfAttribute.IsMultilingual)
                    {
                        var multilingualElement = instanceElement.Element(fieldName);
                        if (multilingualElement != null)
                        {
                            foreach (XElement cultureInfoElement in multilingualElement.Elements())
                                LoadAttributeValue(cultureInfoElement, valueOfAttribute, null);
                        }
                        else
                            LoadAttributeValue(instanceElement, valueOfAttribute, fieldName);
                        var multilingualFieldValue = GetAttributeValue(valueOfAttribute);
                        SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, multilingualFieldValue);
                    }
                    else
                    {
                        try
                        {
                            LoadAttributeValue(instanceElement, valueOfAttribute, fieldName);
                        }
                        catch (System.Exception error)
                        {
                            throw;
                        }
                    }
                }


                foreach (RelResolver relResolver in RelResolvers)
                {
                    RelResolver orgRelResolver = relResolver;
                    DotNetMetaDataRepository.AssociationEnd assoctiationHierarchyAssociationEnd = orgRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;

                    //Load relation resolver related object in for all association in  association hierarchy
                    do
                    {
                        if (assoctiationHierarchyAssociationEnd.Multiplicity.IsMany && orgRelResolver.ValueTypePath.Count == 0)
                            break;

                        //Loads related objects for value type or zero or one multiplicity association end
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(assoctiationHierarchyAssociationEnd);
                        string fieldName;
                        if (orgRelResolver.AssociationEnd.PropertyMember != null)
                            fieldName = orgRelResolver.AssociationEnd.PropertyMember.Name;
                        else
                            fieldName = fastFieldAccessor.MemberInfo.Name;

                        if (!orgRelResolver.Owner.Class.IsLazyFetching(orgRelResolver.AssociationEnd) || (orgRelResolver.FieldInfo.DeclaringType.IsValueType && orgRelResolver.ValueTypePath.Count > 0/*&& assoctiationHierarchyAssociationEnd.Multiplicity.IsMany */ ))
                        {
                            var LinkedObjects = orgRelResolver.GetLinkedObjects("");
                            if (LinkedObjects.Count > 1)
                                System.Diagnostics.Debug.WriteLine(fieldName + ": Role Constraints mismatch");
                            //Gets relation resolver of this level in association hierarchy
                            PersistenceLayerRunTime.RelResolver assoctiationHierarchyRelResolver = orgRelResolver;
                            if (assoctiationHierarchyAssociationEnd != orgRelResolver.AssociationEnd)
                            {
                                foreach (PersistenceLayerRunTime.RelResolver theRelatioResolver in RelResolvers)
                                {
                                    if (theRelatioResolver.AssociationEnd == assoctiationHierarchyAssociationEnd)
                                    {
                                        assoctiationHierarchyRelResolver = theRelatioResolver;
                                        break;
                                    }
                                }
                            }
                            if (orgRelResolver.Multilingual)
                            {
                                if (LinkedObjects.Count > 0)
                                {
                                    if ((assoctiationHierarchyAssociationEnd.Multiplicity.IsMany && orgRelResolver.FieldInfo.DeclaringType.IsValueType && orgRelResolver.ValueTypePath.Count > 0))
                                    {
                                        //only value type many relation loaded
                                        LoadMultilingualCollection(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                    }
                                    else
                                    {
                                        //only zero or one multiplicity loaded
                                        if (Class.HasReferentialIntegrity(orgRelResolver.AssociationEnd) || (orgRelResolver.AssociationEnd.Navigable && orgRelResolver.AssociationEnd.GetOtherEnd().Navigable))
                                        {
                                            InitializeRelatedObject(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                            SetMultilingualMemberValue(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                        }
                                        else
                                        {
                                            foreach (PersistenceLayerRunTime.MultilingualObjectLink multiligualObjectLink in LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>())
                                            {
                                                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(multiligualObjectLink.LinkedObject) as PersistenceLayerRunTime.StorageInstanceRef;
                                                storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(orgRelResolver.OnObjectDeleted);
                                            }
                                            InitializeRelatedObject(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                            SetMultilingualMemberValue(assoctiationHierarchyRelResolver, LinkedObjects.OfType<PersistenceLayerRunTime.MultilingualObjectLink>().ToList());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((assoctiationHierarchyAssociationEnd.Multiplicity.IsMany && orgRelResolver.FieldInfo.DeclaringType.IsValueType && orgRelResolver.ValueTypePath.Count > 0))
                                {
                                    //only value type many relation loaded
                                    orgRelResolver.CompleteLoad(LinkedObjects);
                                    PersistenceLayer.ObjectContainer objectCollection = GetMemoryInstanceMemberValue(orgRelResolver) as PersistenceLayer.ObjectContainer;
                                    foreach (object @object in LinkedObjects)
                                        (GetObjectCollection(objectCollection) as OnMemoryObjectCollection).Add(@object, true);
                                }
                                else
                                {
                                    //only zero or one multiplicity loaded
                                    object linkedObject = LinkedObjects.FirstOrDefault();
                                    if (linkedObject != null)
                                    {
                                        //TODO: εάν το type του association end είναι interface τότε υπάρχει πρόβλημα  
                                        if (Class.HasReferentialIntegrity(orgRelResolver.AssociationEnd) || (orgRelResolver.AssociationEnd.Navigable && orgRelResolver.AssociationEnd.GetOtherEnd().Navigable))
                                        {
                                            InitializeRelatedObject(assoctiationHierarchyRelResolver, linkedObject);
                                            SetMemberValue(assoctiationHierarchyRelResolver, linkedObject);
                                        }
                                        else
                                        {
                                            PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(linkedObject) as PersistenceLayerRunTime.StorageInstanceRef;
                                            storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(orgRelResolver.OnObjectDeleted);
                                            InitializeRelatedObject(assoctiationHierarchyRelResolver, linkedObject);
                                            SetMemberValue(assoctiationHierarchyRelResolver, linkedObject);
                                        }
                                    }
                                }
                            }
                        }

                        //Get on step up in association hierarchy associationEnd
                        if (assoctiationHierarchyAssociationEnd.IsRoleA && assoctiationHierarchyAssociationEnd.Association.General != null)
                            assoctiationHierarchyAssociationEnd = assoctiationHierarchyAssociationEnd.Association.General.RoleA as DotNetMetaDataRepository.AssociationEnd;
                        else if (!assoctiationHierarchyAssociationEnd.IsRoleA && assoctiationHierarchyAssociationEnd.Association.General != null)
                            assoctiationHierarchyAssociationEnd = assoctiationHierarchyAssociationEnd.Association.General.RoleB as DotNetMetaDataRepository.AssociationEnd;
                        else
                            assoctiationHierarchyAssociationEnd = null;
                    }
                    while (assoctiationHierarchyAssociationEnd != null);
                    assoctiationHierarchyAssociationEnd = orgRelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
                }


                if (Class.ClassHierarchyLinkAssociation != null)
                {
                    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                    ObjectID RoleARefObjectID = new ObjectID((ulong)System.Convert.ChangeType(instanceElement.GetAttribute("RoleA"), (PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
                    string RoleAClassInstaditationName = instanceElement.GetAttribute("RoleAClassInstaditationName");

                    System.Type roleAClassInstaditationType = (ObjectStorage as OOAdvantech.MetaDataLoadingSystem.MetaDataStorageSession).GetObjectCollectionType(RoleAClassInstaditationName);
                    PersistenceLayerRunTime.StorageInstanceRef RoleAStorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[roleAClassInstaditationType][RoleARefObjectID];

                    ObjectID RoleBRefObjectID = new ObjectID((ulong)System.Convert.ChangeType(instanceElement.GetAttribute("RoleB"), (PersistentObjectID as PersistenceLayer.ObjectID).ObjectIDPartsValues[0].GetType()));
                    string RoleBClassInstaditationName = instanceElement.GetAttribute("RoleBClassInstaditationName");
                    System.Type roleBClassInstaditationType = (ObjectStorage as OOAdvantech.MetaDataLoadingSystem.MetaDataStorageSession).GetObjectCollectionType(RoleBClassInstaditationName);
                    PersistenceLayerRunTime.StorageInstanceRef RoleBStorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[roleBClassInstaditationType][RoleBRefObjectID];


                    if (RoleAStorageInstanceRef == null)
                    {

                        object NewObject = AccessorBuilder.CreateInstance(roleAClassInstaditationType);

                        if (NewObject == null)
                            throw new System.Exception("PersistencyService can't instadiate the " + RoleAClassInstaditationName);
                        XElement mElement = ((MetaDataStorageSession)ObjectStorage).GetXMLElement(NewObject.GetType(), (ObjectID)RoleARefObjectID);
                        RoleAStorageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, RoleARefObjectID);
                        //RoleAStorageInstanceRef.ObjectID=RoleARefObjectID;
                        ((MetaDataStorageInstanceRef)RoleAStorageInstanceRef).TheStorageIstance = mElement;
                        ((MetaDataStorageInstanceRef)RoleAStorageInstanceRef).LoadObjectState();
                        //RoleAStorageInstanceRef.SnapshotStorageInstance();
                        RoleAStorageInstanceRef.ObjectActived();
                    }

                    if (RoleBStorageInstanceRef == null)
                    {

                        object NewObject = AccessorBuilder.CreateInstance(roleBClassInstaditationType);
                        if (NewObject == null)
                            throw new System.Exception("PersistencyService can't instadiate the " + RoleBClassInstaditationName);
                        XElement mElement = ((MetaDataStorageSession)ObjectStorage).GetXMLElement(NewObject.GetType(), (ObjectID)RoleBRefObjectID);
                        RoleBStorageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, RoleBRefObjectID);
                        //RoleBStorageInstanceRef.ObjectID=RoleBRefObjectID;
                        ((MetaDataStorageInstanceRef)RoleBStorageInstanceRef).TheStorageIstance = mElement;
                        ((MetaDataStorageInstanceRef)RoleBStorageInstanceRef).LoadObjectState();
                        //RoleBStorageInstanceRef.SnapshotStorageInstance();
                        RoleBStorageInstanceRef.ObjectActived();
                    }


                    System.Reflection.FieldInfo FieldRoleA = Class.LinkClassRoleAField;
                    object memoryInstance = MemoryInstance;
                    //FieldRoleA.SetValue(MemoryInstance, RoleAStorageInstanceRef.MemoryInstance);
                    Member<object>.SetValueImplicitly(Class.LinkClassRoleAFastFieldAccessor, ref memoryInstance, RoleAStorageInstanceRef.MemoryInstance);

                    System.Reflection.FieldInfo FieldRoleB = Class.LinkClassRoleBField;
                    //FieldRoleB.SetValue(MemoryInstance, RoleBStorageInstanceRef.MemoryInstance);
                    Member<object>.SetValueImplicitly(Class.LinkClassRoleBFastFieldAccessor, ref memoryInstance, RoleBStorageInstanceRef.MemoryInstance);

                }
                innerStateTransition.Consistent = true;
            }







        }



        /// <MetaDataID>{A13128B5-2B99-430C-8A7F-8864ACFC0D80}</MetaDataID>
        public void SaveObjectState()
        {
            XElement theXmlElement = TheStorageIstance;
            //System.Collections.Specialized.HybridDictionary PersistenceMembers;

            theXmlElement.SetAttribute("ReferentialIntegrityCount", ReferentialIntegrityCount.ToString());

            System.Collections.Generic.Dictionary<XElement, bool> valueTypeNodeMembersValues = new System.Collections.Generic.Dictionary<XElement, bool>();
            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues())
            {

                theXmlElement = TheStorageIstance;

                if (valueOfAttribute.ValueTypePath.Count >= 2)
                {
                    MetaDataRepository.MetaObjectID[] path = valueOfAttribute.ValueTypePath.ToArray();
                    for (int i = 0; i != path.Length - 1; i++)
                    {
                        MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(path[path.Length - i - 1]);
                        string memberName = (this._ObjectStorage as MetaDataStorageSession).GetMappedTagName(metaObject.Identity.ToString());
                        if (string.IsNullOrEmpty(memberName))
                        {
                            memberName = metaObject.Name;
                            (this._ObjectStorage as MetaDataStorageSession).SetMappedTagName(metaObject.Identity.ToString(), metaObject.Name);
                        }
                        XElement attributeElement = theXmlElement.Element(memberName);
                        if (attributeElement == null)
                        {
                            attributeElement = new XElement(memberName);
                            theXmlElement.Add(attributeElement);
                        }
                        theXmlElement = attributeElement;
                        if (!valueTypeNodeMembersValues.ContainsKey(theXmlElement))
                            valueTypeNodeMembersValues[theXmlElement] = false;
                    }
                }

                #region gets field name with backward computability
                string fieldName = (this._ObjectStorage as MetaDataStorageSession).GetMappedTagName(valueOfAttribute.Attribute.Identity.ToString());
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName = valueOfAttribute.Attribute.Name;
                    (this._ObjectStorage as MetaDataStorageSession).SetMappedTagName(valueOfAttribute.Attribute.Identity.ToString(), valueOfAttribute.Attribute.Name);
                }
                #endregion


                //if(CurrFieldInfo.GetValue(MemoryInstance)!=null)
                if (valueOfAttribute.Value != null)
                {
                    valueTypeNodeMembersValues[theXmlElement] = true;
                    System.Type fieldType = valueOfAttribute.FieldInfo.FieldType;
                    if (valueOfAttribute.IsMultilingual)
                    {
                        if (fieldType.IsGenericType && typeof(OOAdvantech.MultilingualMember<string>).GetGenericTypeDefinition() == typeof(OOAdvantech.MultilingualMember<>))
                            fieldType = fieldType.GetGenericArguments()[0];


                        var multilingualElement = theXmlElement.Element(fieldName);
                        if (multilingualElement != null)
                        {
                            multilingualElement.Remove();
                            multilingualElement = null;
                        }

                        foreach (System.Collections.DictionaryEntry entry in (valueOfAttribute.Value as System.Collections.IDictionary))
                        {
                            if (multilingualElement == null)
                            {
                                multilingualElement = new XElement(fieldName);
                                theXmlElement.Add(multilingualElement);
                            }
                            System.Globalization.CultureInfo cultureInfo = entry.Key as System.Globalization.CultureInfo;
                            XElement cultureElement = new XElement(cultureInfo.Name);
                            multilingualElement.Add(cultureElement);

                            var cultureValueOfAttribute = new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, entry.Value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, cultureInfo);

                            if (cultureValueOfAttribute.Value != null)
                                SetXMLValue(cultureElement, cultureValueOfAttribute, null, fieldType);
                            else
                            {

                            }
                        }
                    }
                    else
                        SetXMLValue(theXmlElement, valueOfAttribute, fieldName, fieldType);
                }
                else
                {
                    if (valueOfAttribute.IsMultilingual && valueOfAttribute.ValueTypePath.Count >= 2)
                    {
                    //    if (theXmlElement.Attribute(fieldName) != null)
                    //        theXmlElement.Attribute(fieldName).Remove();
                    //    if (theXmlElement.Element(fieldName) != null)
                    //        theXmlElement.Element(fieldName).Remove();
                    //    if (theXmlElement.Attributes().Count() == 0 && theXmlElement.Elements().Count() == 0)
                    //        theXmlElement.Remove();

                    }
                    else
                        theXmlElement.SetAttribute(fieldName, "");

                }

            }
            foreach( var entry in  valueTypeNodeMembersValues)
            {
                if(!entry.Value)
                    entry.Key.Remove();
            }
        }

        private static void SetXMLValue(XElement theXmlElement, ValueOfAttribute ValueOfAttribute, string fieldName, System.Type fieldType)
        {
            string stringValue;
            if (fieldType.GetMetaData().BaseType == typeof(System.Enum))
            {
                //object Value = CurrFieldInfo.GetValue(MemoryInstance);
                object Value = ValueOfAttribute.Value;
                if (Value != null)
                {
                    stringValue = Value.ToString();
                    if (fieldName != null)
                        theXmlElement.SetAttribute(fieldName, stringValue);
                    else
                        theXmlElement.Value = stringValue;

                }
                //continue;
                return;
            }
#if !DeviceDotNet
            if (fieldType == typeof(System.Xml.XmlDocument))
            {

                System.Xml.XmlDocument XmlDocumentValue = (System.Xml.XmlDocument)ValueOfAttribute.Value;
                XElement XMLFieldNode = null;
                if (fieldName != null)
                {
                    foreach (XElement CurrNode in theXmlElement.Elements())
                    {
                        if (fieldName == CurrNode.Name)
                        {
                            XMLFieldNode = CurrNode;
                            break;
                        }
                    }
                }
                else
                    XMLFieldNode = theXmlElement;

                if (XmlDocumentValue.ChildNodes.Count > 0)
                {
                    if (XMLFieldNode == null)
                    {
                        XMLFieldNode = new XElement(fieldName);
                        theXmlElement.Add(XMLFieldNode);
                    }
                    XMLFieldNode.Value = XmlDocumentValue.InnerXml;
                }
                else
                {
                    if (XMLFieldNode != null)
                        XMLFieldNode.Remove();
                }
                //continue;
                return;
            }
#endif

            if (fieldType == typeof(XDocument))
            {

                XDocument XmlDocumentValue = ValueOfAttribute.Value as XDocument;
                if (fieldName != null)
                {
                    XElement XMLFieldNode = null;
                    foreach (XElement CurrNode in theXmlElement.Elements())
                    {
                        if (fieldName == CurrNode.Name)
                        {
                            XMLFieldNode = CurrNode;
                            break;
                        }
                    }
                    if (XMLFieldNode != null)
                        XMLFieldNode.Remove();
                    XMLFieldNode = new XElement(fieldName);
                    if (XmlDocumentValue.Elements().Count() > 0)
                    {
                        XMLFieldNode.Add(XElement.Parse(XmlDocumentValue.Root.ToString()));
                        theXmlElement.Add(XMLFieldNode);
                    }
                }
                else
                {
                    XElement XMLFieldNode = theXmlElement.Elements().FirstOrDefault();
                    if (XMLFieldNode != null)
                        XMLFieldNode.Remove();
                    theXmlElement.Add(XElement.Parse(XmlDocumentValue.Root.ToString()));
                }
                //continue;
                return;
            }


            if (ValueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>() == typeof(System.DateTime))
            {
                System.DateTime mDateTime = (System.DateTime)ValueOfAttribute.Value;
                System.IFormatProvider format = OOAdvantech.CultureInfoHelper.GetCultureInfo(0x0409);

                stringValue = mDateTime.ToString(format);
                stringValue = mDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt", format);
                try
                {
                    if (fieldName != null)
                        theXmlElement.SetAttribute(fieldName, stringValue);
                    else
                        theXmlElement.Value = stringValue;

                }
                catch (System.Exception aException)
                {
                }
                //continue;
                return;
            }
            stringValue = ValueOfAttribute.Value.ToString();

            if (ValueOfAttribute.Value is float)
                stringValue = ((float)ValueOfAttribute.Value).ToString(CultureInfoHelper.GetCultureInfo(1033));

            if (ValueOfAttribute.Value is double)
                stringValue = ((double)ValueOfAttribute.Value).ToString(CultureInfoHelper.GetCultureInfo(1033));

            if (ValueOfAttribute.Value is decimal)
                stringValue = ((decimal)ValueOfAttribute.Value).ToString(CultureInfoHelper.GetCultureInfo(1033));

            try
            {
                if (fieldName != null)
                    theXmlElement.SetAttribute(fieldName, stringValue);
                else
                    theXmlElement.Value = stringValue;

            }
            catch (System.Exception aException)
            {
            }
        }
    }
}
