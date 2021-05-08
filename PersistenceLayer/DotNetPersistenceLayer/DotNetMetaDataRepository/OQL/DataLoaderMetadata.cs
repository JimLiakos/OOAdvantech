using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using SubDataNodeIdentity = System.Guid;
    using RelationPartIdentity = System.String;

    /// <MetaDataID>{DDE24759-60CB-46e9-BFF4-1640C9C0B720}</MetaDataID>
    [Serializable]
    public class DataLoaderMetadata
    {

        /// <exclude>Excluded</exclude> 
        OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> _StorageCells = new OOAdvantech.Collections.Generic.List<StorageCell>();
        /// <MetaDataID>{7895c8d9-2800-4e5b-b256-c998bad1c108}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> StorageCells
        {
            get
            {
                return new OOAdvantech.Collections.Generic.List<StorageCell>(_StorageCells);
            }
        }

         MemoryCell _MemoryCell;
        /// <MetaDataID>{c714ce52-9a7c-4d68-bef0-235736e8f274}</MetaDataID>
         public  MemoryCell MemoryCell
         {
             get
             {
                 return _MemoryCell;
             }
         }

        /// <MetaDataID>{aa003d9a-8d9b-4762-a47d-454142c99fba}</MetaDataID>
        internal void AddStorageCell(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            if (!_StorageCells.Contains(storageCell))
                _StorageCells.Add(storageCell);
        }
        /// <MetaDataID>{b00b8fc0-94f2-48ce-badd-cd5f015cb623}</MetaDataID>
        internal void RemoveStorageCell(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            if (storageCell != null && _StorageCells.Contains(storageCell))
                _StorageCells.Remove(storageCell);
        }
        /// <MetaDataID>{9ff73b52-d3dc-4726-9e21-87b8b030be82}</MetaDataID>
        public List<MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData> StorageCellReferencesMetaData = new List<StorageCellReference.StorageCellReferenceMetaData>();

        /// <MetaDataID>{e2e53ba4-b6f0-4ba3-a45f-10abf4e87afe}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<MetaDataRepository.MetaObjectID, bool> SubDataNodeOutStorageRelations = new OOAdvantech.Collections.Generic.Dictionary<MetaObjectID, bool>();

        ///<summary>
        ///This method defines out storage relation with subDataNode
        ///</summary>
        ///<param name="dataNode">
        ///This parameter define the subDataNode which has related data in storage other than 
        ///DataLoaderMetadata sotrage
        ///</param>
        /// <MetaDataID>{706a4140-eb03-47b3-bc11-12d3edfa7eb1}</MetaDataID>
        public void HasOutStorageRelationsWithSubDataNode(DataNode dataNode)
        {
            SubDataNodeOutStorageRelations[dataNode.AssignedMetaObject.Identity] = true;
        }

        /// <MetaDataID>{965c0e24-de21-4810-8bfa-b20bae74da59}</MetaDataID>
        bool _HasParentSubDataNodeOutStorageRelations;
        ///<summary>
        ///This field defines the existance of out storage relations with
        ///parent DataNode objects
        ///</summary>
        /// <MetaDataID>{555eb590-e2b4-4f6d-ae34-bf317c1658d1}</MetaDataID>
        public bool HasOutStorageRelationsWithParent
        {
            get
            {
                return _HasParentSubDataNodeOutStorageRelations;
            }
            set
            {
                _HasParentSubDataNodeOutStorageRelations = value;
            }
        }

        /// <exclude>Excluded</exclude>
        int _QueryStorageID;
        ///<summary>
        ///This field specifies the identity of DataLoaderMetadatStorage that is valid in query context
        ///Query engine use this type of identity in any table DataRow instead of StorageIdentity 
        ///to save memor space and for faster data transfer 
        ///</summary>
        /// <MetaDataID>{79474c84-2794-4edc-a19b-34c6f3c5b368}</MetaDataID>
        public int QueryStorageID
        {
            get
            {
                return _QueryStorageID;
            }
        }

        ///// <MetaDataID>{21905582-d638-4257-9356-e00f7b1c7655}</MetaDataID>
        /// <MetaDataID>{e4a000c6-ad42-471a-a02e-8a66a4fd9992}</MetaDataID>
        internal DataNode DataNode;

        /// <exclude>Excluded</exclude>
        Guid _DataNodeIdentity;
        /// <MetaDataID>{85172688-aef2-42a0-a386-7f10c04cef55}</MetaDataID>
        internal Guid DataNodeIdentity
        {
            get
            {
                return _DataNodeIdentity;
            }
        }

        string _ObjectsContextIdentity;
        ///<summary>
        ///This field defines the identity of objects context which will be used for data retrieving.    
        ///</summary>
        /// <MetaDataID>{6420c27e-20aa-4a3f-b8ca-918a4614210b}</MetaDataID>
        public  string ObjectsContextIdentity;
        /// <MetaDataID>{d0945ff1-4fbf-4a6d-a216-d2b9cc7d3015}</MetaDataID>
        public DataLoaderMetadata(DataNode dataNode, int queryStorageID, string objectsContextIdentity, string storageName, string storageLocation, string storageType)
        {
            DataNode = dataNode;
            _DataNodeIdentity = dataNode.Identity;
            _QueryStorageID = queryStorageID;
            ObjectsContextIdentity = objectsContextIdentity;
            _StorageName = storageName;
            _StorageLocation = storageLocation;
            _StorageType = storageType;
        }

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        ObjectsContext _ObjectsContext;

        /// <MetaDataID>{0afa77e0-f1b0-4063-9e78-6b3225bd5f20}</MetaDataID>
        public ObjectsContext ObjectsContext
        {
            get
            {
                if (_ObjectsContext != null)
                    return _ObjectsContext;

                if (!string.IsNullOrEmpty(StorageName) && !string.IsNullOrEmpty(StorageType) && !string.IsNullOrEmpty(StorageType))
                    _ObjectsContext = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType);
                return _ObjectsContext;
            }
        }

        /// <MetaDataID>{ef8e8713-9ee6-4059-8ee3-bb94d1bda13e}</MetaDataID>
        public DataLoaderMetadata(DataLoaderMetadata copyDataLoaderMetadata, ObjectQuery objectQuery)
        {
            _HasParentSubDataNodeOutStorageRelations = copyDataLoaderMetadata._HasParentSubDataNodeOutStorageRelations;
            _StorageCells = copyDataLoaderMetadata._StorageCells;
            _ObjectsContext = copyDataLoaderMetadata._ObjectsContext;
            _DataNodeIdentity = copyDataLoaderMetadata.DataNodeIdentity;
            this.DataNode = objectQuery.DataTrees[0].GetDataNode(copyDataLoaderMetadata.DataNodeIdentity);
            this._MemoryCell = copyDataLoaderMetadata.MemoryCell;
            this.ObjectsContextIdentity = copyDataLoaderMetadata.ObjectsContextIdentity;
            _QueryStorageID = copyDataLoaderMetadata.QueryStorageID;

            this.RelatedMemoryCells = copyDataLoaderMetadata.RelatedMemoryCells;
            this.RelatedStorageCells = copyDataLoaderMetadata.RelatedStorageCells;
            this.StorageCellReferencesMetaData = copyDataLoaderMetadata.StorageCellReferencesMetaData;

            this._StorageLocation = copyDataLoaderMetadata.StorageLocation;
            this._StorageName = copyDataLoaderMetadata.StorageName;
            this._StorageType = copyDataLoaderMetadata.StorageType;
            this.SubDataNodeOutStorageRelations = copyDataLoaderMetadata.SubDataNodeOutStorageRelations;
        }
        /// <MetaDataID>{61d935a7-6205-437c-be2a-5a494d310a44}</MetaDataID>
        public DataLoaderMetadata(DataNode dataNode, int queryStorageID, PersistenceLayer.ObjectStorage objectStorage, MemoryCell memoryCell)
        {

            DataNode = dataNode;
            _DataNodeIdentity = dataNode.Identity;
            _QueryStorageID = queryStorageID;
            ObjectsContextIdentity = objectStorage.StorageMetaData.StorageIdentity;
            _StorageName = objectStorage.StorageMetaData.StorageName;
            _StorageLocation = objectStorage.StorageMetaData.StorageLocation;
            _StorageType = objectStorage.StorageMetaData.StorageType;
            _MemoryCell = memoryCell;
        }

        /// <MetaDataID>{ca68148a-9f0f-4c03-a4ad-21922267eb15}</MetaDataID>
        public DataLoaderMetadata(DataNode dataNode, MemoryCell memoryCell, ObjectsContext objectsContext)
        {
            DataNode = dataNode;
            _DataNodeIdentity = dataNode.Identity;
            _MemoryCell = memoryCell;
            if (MemoryCell is OutProcessMemoryCell)
                _ObjectsContext = (objectsContext as MetaDataRepository.MemoryObjectsContext).GetOutProcessMemoryObjectsCotext((MemoryCell as OutProcessMemoryCell).Channeluri, dataNode.ObjectQuery.QueryIdentity);
            else
                _ObjectsContext = objectsContext;

            ObjectsContextIdentity = _ObjectsContext.Identity;
            if (MemoryCell is PartialLoadedMemoryCell)
            {
                //_StorageCells.AddRange((MemoryCell as PartialLoadedMemoryCell).StorageCells);
                _StorageName = (MemoryCell as PartialLoadedMemoryCell).StorageCells[0].StorageName;
                _StorageLocation = (MemoryCell as PartialLoadedMemoryCell).StorageCells[0].StorageLocation;
                _StorageType = (MemoryCell as PartialLoadedMemoryCell).StorageCells[0].StorageType;
            }
        }

        /// <exclude>Excluded</exclude>
        string _StorageName;
        ///<summary>
        ///This field defines the name of object storage that use for data retrieve.    
        ///</summary>
        /// <MetaDataID>{596051b1-34ce-499d-8537-ba8516e2ee2d}</MetaDataID>
        public string StorageName
        {
            get
            {
                return _StorageName;
            }
        }


        /// <exclude>Excluded</exclude>
        string _StorageLocation;
        ///<summary>
        ///This field defines the location of object storage that use for data retrieve.    
        ///</summary>
        /// <MetaDataID>{beea5908-9df5-4211-af13-7c9b2bc19134}</MetaDataID>
        public string StorageLocation
        {
            get
            {
                return _StorageLocation;
            }
        }


        string _StorageType;
        ///<summary>
        ///This field defines the type of object storage that use for data retrieve.    
        ///</summary>
        /// <MetaDataID>{816d69d0-e1df-496d-8393-16df69274c44}</MetaDataID>
        public string StorageType
        {
            get
            {
                return _StorageType;
            }
        }



        ///<summary>
        ///This method adds extra metadata, like StorageCells StorageCellsReferenceMetaData outstorage relations etc
        ///</summary>
        ///<param name="dataLoaderMetadata">
        ///This parameter defines the dataLoaderMetada which contains the extra metadata
        ///</param>
        /// <MetaDataID>{54481307-2143-48a2-b45b-e3848aa52082}</MetaDataID>
        internal void UpdateWithExtraMetadataInfo(DataLoaderMetadata dataLoaderMetadata)
        {

            foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData newStorageCellReferenceMetaData in dataLoaderMetadata.StorageCellReferencesMetaData)
            {
                bool exist = false;
                foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData in StorageCellReferencesMetaData)
                {
                    if (newStorageCellReferenceMetaData.SerialNumber == storageCellReferenceMetaData.SerialNumber &&
                        newStorageCellReferenceMetaData.StorageIdentity == storageCellReferenceMetaData.StorageIdentity)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                    StorageCellReferencesMetaData.Add(new StorageCellReference.StorageCellReferenceMetaData(newStorageCellReferenceMetaData));

            }
            foreach (MetaDataRepository.StorageCell newStorageCell in dataLoaderMetadata.StorageCells)
            {
                if (!_StorageCells.Contains(newStorageCell))
                    _StorageCells.Add(newStorageCell);
            }
            foreach (var entry in dataLoaderMetadata.SubDataNodeOutStorageRelations)
            {
                bool dataNodeOutStorageRelations = false;
                SubDataNodeOutStorageRelations.TryGetValue(entry.Key, out dataNodeOutStorageRelations);
                if (entry.Value && !dataNodeOutStorageRelations)
                    SubDataNodeOutStorageRelations[entry.Key] = true;
            }
            if (dataLoaderMetadata.HasOutStorageRelationsWithParent)
                HasOutStorageRelationsWithParent = true;
        }

        /// <summary>
        /// This field defines a double dictionary, the main dictionary keep the related storageCell wich corresponds to each subDataNode 
        /// and the secondary dictionary maps each storage cell of DataLoaderMetadata with the list of related storegeCells        /// 
        /// </summary>
        /// <MetaDataID>{b4e06aa7-7ece-49d7-9e75-5f1bf837dc85}</MetaDataID>
        internal Dictionary<SubDataNodeIdentity, Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>>> RelatedStorageCells = new Dictionary<Guid, Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<string, List<StorageCellReference.StorageCellReferenceMetaData>>>>();

        /// <summary>
        /// This field defines a  dictionary wich keeps the related memoryCells wich corresponds to each subDataNode 
        /// </summary>
        /// <MetaDataID>{d498ee5f-7719-4234-8223-4dbe5570588b}</MetaDataID>
        internal Dictionary<Guid, List<MemoryCellReference>> RelatedMemoryCells = new Dictionary<Guid, List<MemoryCellReference>>();

        /// <MetaDataID>{26d1bbff-a9fa-4c76-b9ec-565bdf8db0f2}</MetaDataID>
        internal void AddRelatedStorageCell(DataNode dataNode, OOAdvantech.Collections.Generic.Set<RelatedStorageCell> linkedStorageCells)
        {
            if (linkedStorageCells.Count == 0)
                return;
            foreach (var relatedStorgeCell in linkedStorageCells)
                AddRelatedStorageCell(dataNode, relatedStorgeCell);
            //Dictionary<StorageCellReference.StorageCellReferenceMetaData,Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>> dataNodeRelatedStorageCells = null;
            //if (!RelatedStorageCells.TryGetValue(dataNode.Identity, out dataNodeRelatedStorageCells))
            //{
            //    dataNodeRelatedStorageCells = new Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<string, List<StorageCellReference.StorageCellReferenceMetaData>>>();
            //    RelatedStorageCells[dataNode.Identity] = dataNodeRelatedStorageCells;
            //}
            //foreach (var relatedStorgeCell in linkedStorageCells)
            //{
            //    var rootStorageCellMetadata = new StorageCellReference.StorageCellReferenceMetaData(relatedStorgeCell.RootStorageCell);
            //    Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>> relatedStorageCells = null;
            //    if (!dataNodeRelatedStorageCells.TryGetValue(rootStorageCellMetadata, out relatedStorageCells))
            //    {
            //        relatedStorageCells = new Dictionary<string,List<StorageCellReference.StorageCellReferenceMetaData>>();
            //        dataNodeRelatedStorageCells[rootStorageCellMetadata] = relatedStorageCells;
            //    }
            //    List<StorageCellReference.StorageCellReferenceMetaData> relationPartStorageCells = null;
            //    if (!relatedStorageCells.TryGetValue(relatedStorgeCell.AssociationEndIdentity, out relationPartStorageCells))
            //    {
            //        relationPartStorageCells = new List<StorageCellReference.StorageCellReferenceMetaData>();
            //        relatedStorageCells[relatedStorgeCell.AssociationEndIdentity] = relationPartStorageCells;
            //    }
            //    relationPartStorageCells.Add(new StorageCellReference.StorageCellReferenceMetaData(relatedStorgeCell.StorageCell));
            //}
        }

        internal void AddRelatedStorageCell(DataNode dataNode, RelatedStorageCell relatedStorgeCell)
        {
            Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>> dataNodeRelatedStorageCells = null;
            if (!RelatedStorageCells.TryGetValue(dataNode.Identity, out dataNodeRelatedStorageCells))
            {
                dataNodeRelatedStorageCells = new Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<string, List<StorageCellReference.StorageCellReferenceMetaData>>>();
                RelatedStorageCells[dataNode.Identity] = dataNodeRelatedStorageCells;
            }
            var rootStorageCellMetadata = new StorageCellReference.StorageCellReferenceMetaData(relatedStorgeCell.RootStorageCell);
            Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>> relatedStorageCells = null;
            if (!dataNodeRelatedStorageCells.TryGetValue(rootStorageCellMetadata, out relatedStorageCells))
            {
                relatedStorageCells = new Dictionary<string, List<StorageCellReference.StorageCellReferenceMetaData>>();
                dataNodeRelatedStorageCells[rootStorageCellMetadata] = relatedStorageCells;
            }
            List<StorageCellReference.StorageCellReferenceMetaData> relationPartStorageCells = null;
            if (!relatedStorageCells.TryGetValue(relatedStorgeCell.AssociationEndIdentity, out relationPartStorageCells))
            {
                relationPartStorageCells = new List<StorageCellReference.StorageCellReferenceMetaData>();
                relatedStorageCells[relatedStorgeCell.AssociationEndIdentity] = relationPartStorageCells;
            }
            relationPartStorageCells.Add(new StorageCellReference.StorageCellReferenceMetaData(relatedStorgeCell.StorageCell));

        }

        /// <MetaDataID>{3791ab6a-a36b-4ae0-aae1-875819620b20}</MetaDataID>
        internal static Dictionary<string, DataLoaderMetadata> TransformDataLoadersMetadata(ObjectQuery objectQuery, Dictionary<string, DataLoaderMetadata> dataLoadersMetadata)
        {
            return (from dataLoaderMetadata in dataLoadersMetadata
                    select new DataLoaderMetadata(dataLoaderMetadata.Value, objectQuery)).ToDictionary(x => x.ObjectsContextIdentity);
        }

        internal DataLoaderMetadata Clone(Dictionary<object, object> clonedObjects)
        {

            DataLoaderMetadata newDataLoaderMetadata = new DataLoaderMetadata(DataNode.Clone(clonedObjects), QueryStorageID, ObjectsContextIdentity, StorageName, StorageLocation, StorageType);
            newDataLoaderMetadata.RelatedMemoryCells = new Dictionary<SubDataNodeIdentity, List<MemoryCellReference>>();
            foreach (var relatedMemoryCellEntry in RelatedMemoryCells)
            {
                newDataLoaderMetadata.RelatedMemoryCells[relatedMemoryCellEntry.Key] = new List<MemoryCellReference>();
                foreach (var relatedMemoryCell in newDataLoaderMetadata.RelatedMemoryCells[relatedMemoryCellEntry.Key])
                    newDataLoaderMetadata.RelatedMemoryCells[relatedMemoryCellEntry.Key].Add(relatedMemoryCell.Clone(clonedObjects));
            }

            newDataLoaderMetadata.RelatedStorageCells = new Dictionary<SubDataNodeIdentity, Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>>>();
            foreach (var dataNodeRelatedStorageCellEntry in RelatedStorageCells)
            {
                newDataLoaderMetadata.RelatedStorageCells[dataNodeRelatedStorageCellEntry.Key] = new Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>>();
                foreach (var relatedStorageCellsEntry in dataNodeRelatedStorageCellEntry.Value)
                {
                    newDataLoaderMetadata.RelatedStorageCells[dataNodeRelatedStorageCellEntry.Key][relatedStorageCellsEntry.Key] = new Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>();
                    foreach (var relatedStorageCellEntry in relatedStorageCellsEntry.Value)
                        newDataLoaderMetadata.RelatedStorageCells[dataNodeRelatedStorageCellEntry.Key][relatedStorageCellsEntry.Key][relatedStorageCellEntry.Key] = new List<StorageCellReference.StorageCellReferenceMetaData>(relatedStorageCellEntry.Value);
                }
            }
            newDataLoaderMetadata.StorageCellReferencesMetaData = new List<StorageCellReference.StorageCellReferenceMetaData>(StorageCellReferencesMetaData);
            if(MemoryCell!=null)
                newDataLoaderMetadata._MemoryCell = MemoryCell.Clone(clonedObjects);
            newDataLoaderMetadata._HasParentSubDataNodeOutStorageRelations = _HasParentSubDataNodeOutStorageRelations;
            newDataLoaderMetadata._StorageCells = _StorageCells;
            newDataLoaderMetadata.DataNode = DataNode.Clone(clonedObjects);
            newDataLoaderMetadata._DataNodeIdentity = _DataNodeIdentity;

            newDataLoaderMetadata._QueryStorageID = _QueryStorageID;

            newDataLoaderMetadata._ObjectsContextIdentity = _ObjectsContextIdentity;
            newDataLoaderMetadata.StorageCellReferencesMetaData = StorageCellReferencesMetaData;
            newDataLoaderMetadata._StorageName = _StorageName;

            newDataLoaderMetadata._StorageLocation = StorageLocation;
            newDataLoaderMetadata._StorageType = _StorageType;
            newDataLoaderMetadata.SubDataNodeOutStorageRelations = new Collections.Generic.Dictionary<MetaObjectID, bool>(SubDataNodeOutStorageRelations);
            return newDataLoaderMetadata;
        }
    }
}
