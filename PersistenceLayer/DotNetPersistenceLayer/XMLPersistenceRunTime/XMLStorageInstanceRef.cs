namespace OOAdvantech.XMLPersistenceRunTime
{
	/// <MetaDataID>{F8063F61-436D-46AB-9C72-B558DBAE3648}</MetaDataID>
    public class StorageInstanceRef : PersistenceLayerRunTime.StorageInstanceRef
	{
        public System.Xml.XmlNode StorageIstance;

        /// <MetaDataID>{AA10CB6F-C671-4DE8-9690-B3E4D5EE75DB}</MetaDataID>
        public StorageInstanceRef(object memoryInstance, ObjectStorage objectStorage, object objectID)
            : base(memoryInstance, objectStorage, objectID)
		{

		}

		/// <MetaDataID>{D57136B5-4D98-429C-B167-49F8043DE5D5}</MetaDataID>
		public void LoadObjectState()
		{
            try
            {
                System.Xml.XmlElement theXmlElement = (System.Xml.XmlElement)StorageIstance;
                string RefIntgrCountStr = theXmlElement.GetAttribute("ReferentialIntegrityCount");
                if (RefIntgrCountStr.Length > 0)
                    _ReferentialIntegrityCount = int.Parse(RefIntgrCountStr);

                foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                {
                    if (!Class.IsPersistent(attribute))
                        continue;
                    System.Reflection.FieldInfo CurrFieldInfo = Class.GetFieldMember(attribute);
                    string FieldName = attribute.Name;

                    if (CurrFieldInfo.FieldType == typeof(System.Xml.XmlDocument))
                    {
                        System.Xml.XmlNode XMLFieldNode = null;
                        foreach (System.Xml.XmlNode CurrNode in theXmlElement)
                        {
                            if (FieldName == CurrNode.Name)
                            {
                                XMLFieldNode = CurrNode;
                                break;
                            }
                        }
                        if (XMLFieldNode == null)
                            continue;
                        System.Xml.XmlDocument theXmlDocument = (System.Xml.XmlDocument)CurrFieldInfo.GetValue(MemoryInstance);
                        if (theXmlDocument == null)
                        {
                            theXmlDocument = new System.Xml.XmlDocument();
                            theXmlDocument.InnerXml = XMLFieldNode.InnerXml;
                            CurrFieldInfo.SetValue(MemoryInstance, theXmlDocument);
                        }
                        else
                            theXmlDocument.InnerXml = XMLFieldNode.InnerXml;
                    }
                    else
                    {

                        if (CurrFieldInfo.FieldType == typeof(System.DateTime))
                        {
                            try
                            {
                                string Value = theXmlElement.GetAttribute(FieldName);
                                System.IFormatProvider format =
                                    new System.Globalization.CultureInfo(0x0409);
                                System.DateTime mDateTime = System.DateTime.Parse(Value, format);
                                CurrFieldInfo.SetValue(MemoryInstance, mDateTime);
                            }
                            catch (System.Exception Error)
                            {

                            }
                        }
                        else
                        {

                            string Value = theXmlElement.GetAttribute(FieldName);
                            if (Value.Length > 0)
                            {
                                object FieldValue = null;
                                if (CurrFieldInfo.FieldType != Value.GetType())
                                {

                                    if (CurrFieldInfo.FieldType.BaseType == typeof(System.Enum))
                                    {
                                        if (Value != null)
                                            if (Value.Length > 0)
                                                FieldValue = System.Enum.Parse(CurrFieldInfo.FieldType, Value,false);
                                    }
                                    else
                                        FieldValue = System.Convert.ChangeType(Value, CurrFieldInfo.FieldType,System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                                }
                                else
                                    FieldValue = Value;
                                CurrFieldInfo.SetValue(MemoryInstance, FieldValue);
                            }
                        }
                    }
                }

                foreach (PersistenceLayerRunTime.RelResolver relResolver in RelResolvers)
                {
                    PersistenceLayerRunTime.RelResolver mResolver = relResolver;
                    DotNetMetaDataRepository.AssociationEnd associationEnd = mResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;



                    System.Reflection.FieldInfo CurrFieldInfo = Class.GetFieldMember(associationEnd);
                    if (CurrFieldInfo.FieldType == typeof(PersistenceLayer.ObjectContainer) || CurrFieldInfo.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        continue;
                    string FieldName;
                    if (associationEnd.PropertyMember != null)
                        FieldName = associationEnd.PropertyMember.Name;
                    else
                        FieldName = CurrFieldInfo.Name;

                    if (!mResolver.Owner.Class.IsLazyFetching(mResolver.AssociationEnd))
                    {
                        System.Collections.ArrayList LinkedObjects = mResolver.GetLinkedObjects("");
                        if (LinkedObjects.Count > 1)
                            System.Diagnostics.Debug.WriteLine(FieldName + ": Role Constraints mismatch");
                        object LinkedObject = null;
                        foreach (object _object in LinkedObjects)
                        {

                            //TODO: εάν το type του association end είναι interface τότε υπάρχει πρόβλημα  

                            if (Class.HasReferentialIntegrity(mResolver.AssociationEnd) || (mResolver.AssociationEnd.Navigable && mResolver.AssociationEnd.GetOtherEnd().Navigable))
                            {
                                LinkedObject = _object;
                                CurrFieldInfo.SetValue(MemoryInstance, LinkedObject);
                            }
                            else
                            {
                                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(_object) as PersistenceLayerRunTime.StorageInstanceRef;
                                storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(mResolver.OnObjectDeleted);
                                LinkedObject = _object;
                                CurrFieldInfo.SetValue(MemoryInstance, LinkedObject);
                            }

                            break;
                        }
                    }
                }
                if (Class.LinkAssociation != null)
                {
                    //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass

                    object RoleARefObjectID = System.Convert.ChangeType(theXmlElement.GetAttribute("RoleA"), ObjectID.GetType(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                    string RoleAClassInstaditationName = theXmlElement.GetAttribute("RoleAClassInstaditationName");
                    PersistenceLayerRunTime.StorageInstanceRef RoleAStorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(RoleAClassInstaditationName, "Version")][RoleARefObjectID];

                    object RoleBRefObjectID = System.Convert.ChangeType(theXmlElement.GetAttribute("RoleB"), ObjectID.GetType(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                    string RoleBClassInstaditationName = theXmlElement.GetAttribute("RoleBClassInstaditationName");
                    PersistenceLayerRunTime.StorageInstanceRef RoleBStorageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[ModulePublisher.ClassRepository.GetType(RoleBClassInstaditationName, "Version")][RoleBRefObjectID];


                    if (RoleAStorageInstanceRef == null)
                    {

                        object NewObject = ModulePublisher.ClassRepository.CreateInstance(RoleAClassInstaditationName, "");

                        if (NewObject == null)
                            throw new System.Exception("PersistencyService can't instadiate the " + RoleAClassInstaditationName);
                        System.Xml.XmlElement mElement = ((ObjectStorage)ObjectStorage).GetXMLElement(NewObject.GetType(), RoleARefObjectID);
                        RoleAStorageInstanceRef = (StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, RoleARefObjectID);
                        //RoleAStorageInstanceRef.ObjectID=RoleARefObjectID;
                        ((StorageInstanceRef)RoleAStorageInstanceRef).StorageIstance = mElement;
                        ((StorageInstanceRef)RoleAStorageInstanceRef).LoadObjectState();
                        RoleAStorageInstanceRef.SnapshotStorageInstance();
                        RoleAStorageInstanceRef.ObjectActived();
                    }

                    if (RoleBStorageInstanceRef == null)
                    {

                        object NewObject = ModulePublisher.ClassRepository.CreateInstance(RoleBClassInstaditationName, "");
                        if (NewObject == null)
                            throw new System.Exception("PersistencyService can't instadiate the " + RoleBClassInstaditationName);
                        System.Xml.XmlElement mElement = ((ObjectStorage)ObjectStorage).GetXMLElement(NewObject.GetType(), RoleBRefObjectID);
                        RoleBStorageInstanceRef = (StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, RoleBRefObjectID);
                        //RoleBStorageInstanceRef.ObjectID=RoleBRefObjectID;
                        ((StorageInstanceRef)RoleBStorageInstanceRef).StorageIstance = mElement;
                        ((StorageInstanceRef)RoleBStorageInstanceRef).LoadObjectState();
                        RoleBStorageInstanceRef.SnapshotStorageInstance();
                        RoleBStorageInstanceRef.ObjectActived();
                    }


                    System.Reflection.FieldInfo FieldRoleA = MemoryInstance.GetType().GetField("RoleA", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); // Error Prone if FieldRoleA==null;
                    FieldRoleA.SetValue(MemoryInstance, RoleAStorageInstanceRef.MemoryInstance);

                    System.Reflection.FieldInfo FieldRoleB = MemoryInstance.GetType().GetField("RoleB", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); // Error Prone if FieldRoleA==null;
                    FieldRoleB.SetValue(MemoryInstance, RoleBStorageInstanceRef.MemoryInstance);
                }
            }
            catch (System.Exception error)
            {
                throw new System.Exception(error.Message, error);
                int gg = 0;
            }
		}
		/// <MetaDataID>{E9A2AE67-0872-4218-8274-1DF9AC0B6BFB}</MetaDataID>
		public void SaveObjectState()
		{
            System.Xml.XmlElement theXmlElement = (System.Xml.XmlElement)StorageIstance;

            theXmlElement.SetAttribute("ReferentialIntegrityCount", ReferentialIntegrityCount.ToString());

            foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
            {
                if (!Class.IsPersistent(attribute))
                    continue;
                System.Reflection.FieldInfo CurrFieldInfo = Class.GetFieldMember(attribute);
                string FieldName = attribute.Name;

                if (CurrFieldInfo.GetValue(MemoryInstance) != null)
                {
                    string StringValue;
                    if (CurrFieldInfo.FieldType.BaseType == typeof(System.Enum))
                    {
                        object Value = CurrFieldInfo.GetValue(MemoryInstance);
                        if (Value != null)
                        {
                            StringValue = Value.ToString();
                            theXmlElement.SetAttribute(FieldName, StringValue);
                        }
                        continue;
                    }
                    if (CurrFieldInfo.FieldType == typeof(System.Xml.XmlDocument))
                    {
                        System.Xml.XmlDocument XmlDocumentValue = (System.Xml.XmlDocument)CurrFieldInfo.GetValue(MemoryInstance);
                        System.Xml.XmlNode XMLFieldNode = null;
                        foreach (System.Xml.XmlNode CurrNode in theXmlElement)
                        {
                            if (FieldName == CurrNode.Name)
                            {
                                XMLFieldNode = CurrNode;
                                break;
                            }
                        }
                        if (XmlDocumentValue.ChildNodes.Count > 0)
                        {
                            if (XMLFieldNode == null)
                            {
                                XMLFieldNode = theXmlElement.OwnerDocument.CreateElement(FieldName);
                                theXmlElement.AppendChild(XMLFieldNode);
                            }
                            XMLFieldNode.InnerXml = XmlDocumentValue.InnerXml;
                        }
                        else
                        {
                            if (XMLFieldNode != null)
                                theXmlElement.RemoveChild(XMLFieldNode);

                        }
                        continue;
                    }
                    if (CurrFieldInfo.FieldType == typeof(System.DateTime))
                    {
                        System.DateTime mDateTime = (System.DateTime)CurrFieldInfo.GetValue(MemoryInstance);

                        System.IFormatProvider format =
                            new System.Globalization.CultureInfo(0x0409);
                        StringValue = mDateTime.ToString(format);
                        try
                        {
                            theXmlElement.SetAttribute(FieldName, StringValue);
                        }
                        catch (System.Exception aException)
                        {
                        }
                        continue;
                    }
                    StringValue = CurrFieldInfo.GetValue(MemoryInstance).ToString();
                    try
                    {
                        System.Convert.ChangeType(StringValue, CurrFieldInfo.FieldType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                        theXmlElement.SetAttribute(FieldName, StringValue);
                    }
                    catch (System.Exception aException)
                    {

                    }
                }
            }

		}

        /// <MetaDataID>{32F5D854-9E67-40A4-9E4D-BED2F8A59418}</MetaDataID>
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }
}
