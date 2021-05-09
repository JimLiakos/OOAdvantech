
using System;
using System.Xml;
namespace OOAdvantech.XMLPersistenceRunTime
{
	
    

	/// <MetaDataID>{CFBDF32B-B5E1-4B6A-9EA2-D1161A37B9AE}</MetaDataID>
	/// <summary></summary>
    public class ObjectStorage : OOAdvantech.PersistenceLayerRunTime.ObjectStorage
	{
        private System.Collections.Specialized.HybridDictionary ObjectElements = new System.Collections.Specialized.HybridDictionary();

        public System.Xml.XmlElement GetXMLElement(System.Type ClassCollection, object ObjectID)
        {
            
            if (ObjectID == null)
            {
                int kk = 0;
            }
            string CollectionName = "";
            if (ClassCollection != null)
                CollectionName = ClassCollection.FullName;


            object Object = null;
            if (ObjectElements != null)
                Object = ObjectElements[ObjectID];
            if (Object != null)
                return (System.Xml.XmlElement)Object;
            if (ObjectElements == null)
                ObjectElements = new System.Collections.Specialized.HybridDictionary();
            {
                string StorageName = this.XMLDocument.ChildNodes[0].Name;

                string XQuery = StorageName + "/ObjectCollections/" + CollectionName +
                    "/Object";//[@oid = "+ObjectID.ToString()+"]";
                System.Xml.XmlNodeList Nodes = XMLDocument.SelectNodes(XQuery);
                System.Type ObjectIDType = ObjectID.GetType();
                foreach (System.Xml.XmlNode CurrNode in Nodes)
                {

                    System.Xml.XmlElement StorageInstance = (System.Xml.XmlElement)CurrNode;
                    if (!ObjectElements.Contains(System.Convert.ChangeType(StorageInstance.GetAttribute("oid"), ObjectIDType,System.Globalization.CultureInfo.CurrentCulture.NumberFormat)))
                        ObjectElements.Add(System.Convert.ChangeType(StorageInstance.GetAttribute("oid"), ObjectIDType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat), StorageInstance);
                }
            }

            Object = ObjectElements[ObjectID];
            if (Object == null)
            {
                System.Type ObjectIDType = ObjectID.GetType();
                string StorageName = _XMLDocument.ChildNodes[0].Name;
                string XQuery = StorageName + "/ObjectCollections/" + CollectionName +
                    "/Object[@oid = " + ObjectID.ToString() + "]";

                System.Xml.XmlElement StorageInstance = (System.Xml.XmlElement)XMLDocument.SelectSingleNode(XQuery);
                if (StorageInstance != null)
                    ObjectElements.Add(System.Convert.ChangeType(StorageInstance.GetAttribute("oid"), ObjectIDType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat), StorageInstance);
                return StorageInstance;
            }
            return (System.Xml.XmlElement)Object;
        }

        private System.Collections.Hashtable TransactionsNodesChanges = new System.Collections.Hashtable();
        private System.Collections.Hashtable TransactionsNewNodes = new System.Collections.Hashtable();  
        private System.Collections.Hashtable TransactionsDeletedNodes = new System.Collections.Hashtable();  
        

        /// <MetaDataID>{B1FDCAD3-F8FC-4599-9D92-3DE1E5C7382E}</MetaDataID>
        public void NewNodeUnderTransaction(XmlNode Node, PersistenceLayerRunTime.TransactionContext theTransaction)
        {

            if (Node == null || theTransaction == null)
                throw new System.Exception("The Node and theTransaction parammeters must be not null");
            System.Collections.ArrayList NewNodes = null;
            if (TransactionsNewNodes.Contains(theTransaction))
                NewNodes = (System.Collections.ArrayList)TransactionsNewNodes[theTransaction];
            else
            {
                NewNodes = new System.Collections.ArrayList();
                TransactionsNewNodes[theTransaction] = NewNodes;
            }
            if (!NewNodes.Contains(Node))
                NewNodes.Add(Node);
        }

        /// <MetaDataID>{2069B06B-3F23-4554-9429-836FC0F47591}</MetaDataID>
        public void NodeChangedUnderTransaction(XmlNode Node, PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (Node == null || theTransaction == null)
                throw new System.Exception("The Node and theTransaction parammeters must be not null");
            System.Collections.Hashtable OldXmlNodesState = null;
            if (TransactionsNodesChanges.Contains(theTransaction))
                OldXmlNodesState = (System.Collections.Hashtable)TransactionsNodesChanges[theTransaction];
            else
            {
                OldXmlNodesState = new System.Collections.Hashtable();
                TransactionsNodesChanges[theTransaction] = OldXmlNodesState;
            }
            if (!OldXmlNodesState.Contains(Node))
                OldXmlNodesState[Node] = Node.InnerXml;

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7CD30C17-5A2F-49E9-8A78-73D640A93D62}</MetaDataID>
        private OOAdvantech.PersistenceLayer.Storage _StorageMetaData;
        /// <MetaDataID>{477E217F-712E-48DE-8124-AE6853B7A97D}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                return _StorageMetaData;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{602BF12C-C1E2-43FF-BCF0-D97B770D430F}</MetaDataID>
        private bool _Dirty;
        /// <MetaDataID>{82B8DE38-1756-4E92-8FF0-1EF63E836B00}</MetaDataID>
        public bool Dirty
        {
            set
            {
                _Dirty = value;
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F9F55066-0F85-40DB-B389-D15C23D15B43}</MetaDataID>
        private XmlDocument _XMLDocument;

        /// <MetaDataID>{EAEFB2F5-8945-4A19-AB67-54DB91D32B80}</MetaDataID>
        public XmlDocument XMLDocument
        {
            get
            {
                return _XMLDocument;
            }
            set
            {
                _XMLDocument = value;
                if (_XMLDocument == null)
                    return;
                if (!StorageObjectsLoaded)
                    LoadStorageObjects();
            }
        }
        bool StorageObjectsLoaded = false;

        private void LoadStorageObjects()
        {
            if (StorageObjectsLoaded)
                return;
            System.Xml.XmlNode ObjectCollectionsNode = XMLDocument.ChildNodes[0].ChildNodes[1];
            if (ObjectCollectionsNode == null)
                throw new System.Exception("Bad XML format");
            if (ObjectCollectionsNode.Name != "ObjectCollections")
                throw new System.Exception("Bad XML format");

            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.RequiresNew))
            {
                foreach (System.Xml.XmlNode ObjectCollection in ObjectCollectionsNode)
                {
                    System.Type ObjectCollectionType = ModulePublisher.ClassRepository.GetType(ObjectCollection.Name, "");
                    ObjectCollectionNodes[ObjectCollectionType] = ObjectCollection;
                    MetaDataRepository.Class _class = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType) as MetaDataRepository.Class ;
                    if (_class == null)
                    {
                        MetaDataRepository.MetaObject AssemblyMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType.Assembly);
                        if (AssemblyMetaObject == null)
                            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(ObjectCollectionType.Assembly, new DotNetMetaDataRepository.Assembly(ObjectCollectionType.Assembly));
                        _class = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ObjectCollectionType) as MetaDataRepository.Class;

                        ///TODO φορτώνει την class χωρίς να φορτωθούν όλα τα meta data του assembly 
                        ///αυτός ο τρόπος δεν έχει τεσταριστεί επαρκώς.
                        if (_class == null)
                        {
                            _class = new OOAdvantech.DotNetMetaDataRepository.Class(new OOAdvantech.DotNetMetaDataRepository.Type(ObjectCollectionType));
                            long count = _class.Roles.Count;
                        }

                        if (_class == null)
                            throw new System.Exception("Fatal error with Storage Meta Data");
                    }
                    //PersistenceLayerRunTime.PersClassObjects mPersClassObjects=OperativeObjectCollections[ObjectCollectionType];
                }
                stateTransition.Consistent = true;
            }


            StorageObjectsLoaded = true;

        }

        /// <MetaDataID>{DC0A91AC-C0C2-4F1F-A787-CE7B593E1C68}</MetaDataID>
        public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{47D6DE32-DECA-423F-A096-F9013B05F581}</MetaDataID>
        public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{D79E7A2C-63A3-4463-A522-B1A6037DA3E1}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{E1D485BA-EC56-473C-8813-30AA59A14722}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef sourceStorageInstance, DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{3B556B86-D4EB-49E3-84AA-E670AC303A45}</MetaDataID>
        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{41D44018-379F-4A6F-B879-C7498154ABAC}</MetaDataID>
        public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{97CD761D-82C6-438A-81C7-5A50000D244A}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private string StorageName = null;
        /// <MetaDataID>{B6D4581C-C89D-4123-8161-52C3BCA58F22}</MetaDataID>
        private System.Xml.XmlNode ObjectIDNode = null;
        /// <MetaDataID>{5AB29A66-A53A-44C9-B299-3FB169666B6F}</MetaDataID>
        private System.Collections.Hashtable ObjectCollectionNodes = new System.Collections.Hashtable(100);
        /// <MetaDataID>{AFD9B5AB-A136-4D40-B590-66800764FFF5}</MetaDataID>
        private System.Xml.XmlNode ObjectCollectionsNode = null;

        /// <MetaDataID>{9ADCFAA6-8DB6-48F9-8774-9B22507C1344}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance)
        {
            if (StorageName == null)
                StorageName = _XMLDocument.ChildNodes[0].Name;

            if (ObjectIDNode == null)
                ObjectIDNode = _XMLDocument.SelectSingleNode(StorageName + "/NextObjID");
            if (ObjectIDNode == null)
                throw (new System.Exception("Bad Storage File: Problem With ObjectIDNode"));


            System.Xml.XmlElement ObjectCollection = ObjectCollectionNodes[storageInstance.MemoryInstance.GetType()] as System.Xml.XmlElement;


            if (ObjectCollection == null)
            {
                if (ObjectCollectionsNode == null)
                    ObjectCollectionsNode = _XMLDocument.SelectSingleNode(StorageName + "/ObjectCollections");
                if (ObjectCollectionsNode == null)
                    throw new System.Exception("Bad Storage File: There isn't ObjectCollections.");

                ObjectCollection = (System.Xml.XmlElement)ObjectCollectionsNode.AppendChild(ObjectCollectionsNode.OwnerDocument.CreateElement(storageInstance.MemoryInstance.GetType().FullName));
                ObjectCollection.SetAttribute("ClassInstaditationName", storageInstance.MemoryInstance.GetType().FullName);
                ObjectCollectionNodes[storageInstance.MemoryInstance.GetType()] = ObjectCollection;

            }

            Commands.NewStorageInstanceCommand newStorageInstanceCommand = new Commands.NewStorageInstanceCommand(storageInstance as StorageInstanceRef);
            newStorageInstanceCommand.NextObjectIDNode = ObjectIDNode;
            newStorageInstanceCommand.ObjectCollectionNode = ObjectCollection;
            PersistenceLayerRunTime.TransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(newStorageInstanceCommand);
        }

        /// <MetaDataID>{BA0ABFE7-B09A-4713-9EDA-C264A81A5CF2}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, object objectID)
        {
            return new  StorageInstanceRef(memoryInstance, this, objectID);
        }

        /// <MetaDataID>{C9150B5A-B2E3-408D-823C-361CA2AD6F6B}</MetaDataID>
        public override void AbortChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            string FileName = null;
            System.Uri FileUri = null;
            if (TransactionsDeletedNodes.Contains(theTransaction))
            {
                System.Collections.Hashtable DeletedNodes = (System.Collections.Hashtable)TransactionsDeletedNodes[theTransaction];
                foreach (System.Collections.DictionaryEntry DictionaryEntry in DeletedNodes)
                {
                    System.Xml.XmlNode CurrNode = (System.Xml.XmlNode)DictionaryEntry.Key;
                    System.Xml.XmlNode ParentNode = (System.Xml.XmlNode)DictionaryEntry.Value;
                    ParentNode.AppendChild(CurrNode);
                    int ObjectID = System.Convert.ToInt32(CurrNode.Attributes["oid"].Value, 10);
                    if (!ObjectElements.Contains(ObjectID))
                        ObjectElements.Add(ObjectID, CurrNode);
                }
                if (!_Dirty)
                    return;

                OnObjectStorageChanged();


                FileName = _XMLDocument.BaseURI;
                if (FileName.Length == 0)
                    return;
                FileUri = new System.Uri(FileName);
                FileName = FileUri.LocalPath;
                _XMLDocument.Save(FileName);
                System.Xml.XmlWriter pp;
            }
            if (TransactionsNodesChanges.Contains(theTransaction))
            {
                System.Collections.Hashtable OldXmlNodesState = (System.Collections.Hashtable)TransactionsNodesChanges[theTransaction];
                foreach (System.Collections.DictionaryEntry DictionaryEntry in OldXmlNodesState)
                {
                    System.Xml.XmlNode CurrNode = (System.Xml.XmlNode)DictionaryEntry.Key;
                    CurrNode.InnerXml = (string)DictionaryEntry.Value;
                }
            }

            if (TransactionsNewNodes.Contains(theTransaction))
            {
                System.Collections.ArrayList NewNodes = (System.Collections.ArrayList)TransactionsNewNodes[theTransaction];
                foreach (System.Xml.XmlNode CurrNode in NewNodes)
                {
                    object OidStr = CurrNode.Attributes["oid"].Value;
                    string tt = OidStr.ToString();
                    int ObjectID = System.Convert.ToInt32(CurrNode.Attributes["oid"].Value.ToString(), 10);
                    if (ObjectElements.Contains(ObjectID))
                        ObjectElements.Remove(ObjectID);
                    System.Xml.XmlNode ParentNode = CurrNode.ParentNode;
                    ParentNode.RemoveChild(CurrNode);
                }
            }


            if (TransactionsNewNodes.Contains(theTransaction))
                TransactionsNewNodes.Remove(theTransaction);
            if (TransactionsNodesChanges.Contains(theTransaction))
                TransactionsNodesChanges.Remove(theTransaction);
            if (TransactionsDeletedNodes.Contains(theTransaction))
                TransactionsDeletedNodes.Remove(theTransaction);
            FileName = _XMLDocument.BaseURI;

            _Dirty = false;
            if (FileName.Length == 0)
                return;
            FileUri = new System.Uri(FileName);
            FileName = FileUri.LocalPath;
            _XMLDocument.Save(FileName);
        }

        /// <MetaDataID>{61F1599E-EDCF-4B75-9874-BF14BF1C5B7F}</MetaDataID>
        public override void CommitChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (TransactionsNewNodes.Contains(theTransaction))
                TransactionsNewNodes.Remove(theTransaction);
            if (TransactionsNodesChanges.Contains(theTransaction))
                TransactionsNodesChanges.Remove(theTransaction);
            if (TransactionsDeletedNodes.Contains(theTransaction))
                TransactionsDeletedNodes.Remove(theTransaction);
            _Dirty = false;
        }

        /// <MetaDataID>{7FAEFCA4-971F-479C-ADA4-D4665666062B}</MetaDataID>
        public override void BeginChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
        }

        /// <MetaDataID>{689FE517-DDCE-4B15-9285-B2C774E6C4C4}</MetaDataID>
        public override void MakeChangesDurable(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (!_Dirty)
                return;
            string FileName = _XMLDocument.BaseURI;
            OnObjectStorageChanged();
            _Dirty = false;

            if (FileName.Length == 0)
                return;
            System.Uri FileUri = new System.Uri(FileName);
            FileName = FileUri.LocalPath;
            _XMLDocument.Save(FileName);
        }

        /// <MetaDataID>{AF882B7C-5639-4031-AD0E-5DE004EFBF6D}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{EE1CCEBA-2795-4244-9291-C3B13789F600}</MetaDataID>
        public override string GetPersistentObjectUri(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{C63F5079-1545-4901-9438-8DFB6212BC94}</MetaDataID>
        public override PersistenceLayer.StructureSet Execute(string OQLStatement, Collections.Hashtable parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{922D0814-FABB-4261-8645-2F303366C708}</MetaDataID>
        public override PersistenceLayer.StructureSet Parse(string OQLStatement)
        {
            throw new Exception("The method or operation is not implemented.");
        }



        /// <MetaDataID>{252E9B6A-3BF7-4EED-9E3C-7F23D8392C2A}</MetaDataID>
        public override PersistenceLayer.StructureSet Execute(string OQLStatement)
        {
           
            if (_XMLDocument == null)
                throw new System.Exception("You can't execute any command because the storage session is not initialized properly.");
            if (!StorageObjectsLoaded)
                LoadStorageObjects();
            return null;

            //MetaDataStructureSet mStructureSet = new MetaDataStructureSet();
            //mStructureSet.SourceStorageSession = this;
            //mStructureSet.Open(OQLStatement);
            //return mStructureSet;


        }
    }
}
