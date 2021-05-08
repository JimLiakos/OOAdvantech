using System;

using System.Linq;
using System.Text;
using System.Collections;
using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.Remoting;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{89807f67-a9e7-4f26-81d3-8303d08b41fb}</MetaDataID>
    public class MemoryObjectsContext : ObjectsContext, IObjectQueryPartialResolver
    {

        public readonly List<object> ObjectCollection;
        public readonly Type CollectionItemType;
        string ObjectsContextIdentity;
        internal string CommonChannelUri;
        internal Guid QueryIdentity;
        public MemoryObjectsContext(List<object> objectCollection, Type collectionItemType, Guid queryIdentity)
        {
            ObjectCollection = objectCollection;
            foreach (object @object in objectCollection)
            {
                string channelUri = null;
                if (Remoting.RemotingServices.IsOutOfProcess(@object as MarshalByRefObject))
                    channelUri = Remoting.RemotingServices.GetChannelUri(@object as MarshalByRefObject);
                if (channelUri == null)
                    channelUri = "";
                if (CommonChannelUri == null)
                    CommonChannelUri = channelUri;

                if (channelUri != CommonChannelUri)
                {
                    CommonChannelUri = "";
                    break;
                }

            }
            if (!string.IsNullOrEmpty(CommonChannelUri))
                ObjectsHasOutProcessCommonMemoryContext = true;
            CollectionItemType = collectionItemType;
            ObjectsContextIdentity = "OnMemory:" + queryIdentity.ToString() + "_" + Guid.NewGuid().ToString();

            ActiveMemoryObjectsContexts[queryIdentity] = new WeakReference(this);
            QueryIdentity = queryIdentity;
        }
        //public MemoryObjectsContext(string objectsContextIdentity,Guid queryIdentity)
        //{
        //    ActiveMemoryObjectsContexts[queryIdentity] = new WeakReference(this);
        //    ObjectsContextIdentity = objectsContextIdentity;
        //}

        public MemoryObjectsContext(string objectsContextIdentity)
        {
            ObjectsContextIdentity = objectsContextIdentity;
        }
        public MemoryObjectsContext()
        {
        }

        public override string Identity
        {
            get
            {
                return ObjectsContextIdentity;
            }
        }



        internal System.Collections.Generic.IEnumerable<MemoryCell> GetMemoryCells(OOAdvantech.Collections.Generic.List<ObjectData> objects, Type collectionItemType, Guid dataNodeIdentity)
        {
            Dictionary<string, MemoryCell> memoryCellsDictionary = new Dictionary<string, MemoryCell>();
            foreach (var objectData in objects)
            {
                if (objectData._Object is MarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(objectData._Object as MarshalByRefObject))
                {
                    string channelUri = Remoting.RemotingServices.GetChannelUri(objectData._Object as MarshalByRefObject);
                    string objectsContextIdentity = "OnMemory:" + channelUri;
                    MemoryCell memoryCell = null;
                    if (!memoryCellsDictionary.TryGetValue(objectsContextIdentity, out memoryCell))
                    {

                        memoryCell = new OutProcessMemoryCell(new List<ObjectData>(), collectionItemType, GetOutProcessMemoryObjectsCotext(channelUri, QueryIdentity).Identity, channelUri, dataNodeIdentity);
                        memoryCellsDictionary[objectsContextIdentity] = memoryCell;
                    }
                    memoryCell.Objects.Add(objectData._Object, objectData);
                }
                else
                {

                    MemoryCell memoryCell = null;

                    if (objectData.PartialLoaded)
                    {
                        if (!memoryCellsDictionary.TryGetValue(objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity, out memoryCell))
                        {
                            memoryCell = new PartialLoadedMemoryCell(new List<ObjectData>(), collectionItemType, objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity, dataNodeIdentity);
                            memoryCellsDictionary[objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity] = memoryCell;
                            MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
                        }
                    }
                    else
                    {
                        if (!memoryCellsDictionary.TryGetValue(ObjectsContextIdentity, out memoryCell))
                        {
                            foreach (MemoryCell existingMemoryCell in MemoryCells.Values)
                            {
                                if (existingMemoryCell.DataNodeIdentity == dataNodeIdentity && existingMemoryCell.ObjectsContextIdentity == ObjectsContextIdentity)
                                {
                                    memoryCell = existingMemoryCell;
                                    break;
                                }
                            }
                            if (memoryCell == null)
                                memoryCell = new InProcessMemoryCell(new List<ObjectData>(), collectionItemType, ObjectsContextIdentity, dataNodeIdentity);
                            MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
                            memoryCellsDictionary[ObjectsContextIdentity] = memoryCell;
                        }
                    }
                    memoryCell.Objects.Add(@objectData._Object, objectData);


                    //MemoryCell memoryCell = null;
                    //if (!memoryCells.TryGetValue(ObjectsContextIdentity, out memoryCell))
                    //{
                    //    memoryCell = new InProcessMemoryCell(new List<ObjectData>(), collectionItemType, ObjectsContextIdentity);
                    //    memoryCells[ObjectsContextIdentity] = memoryCell;
                    //}
                    //memoryCell.Objects[@objectData._Object] = objectData;
                }
            }
            List<MemoryCell> memoryCells = new List<MemoryCell>();
            foreach (var memoryCell in memoryCellsDictionary.Values)
            {
                if (memoryCell is OutProcessMemoryCell)
                {
                    foreach (OutProcessMemoryCell outProcessMemoryCell in GetOutProcessMemoryObjectsCotext((memoryCell as OutProcessMemoryCell).Channeluri, QueryIdentity).Synchronize(memoryCell as OutProcessMemoryCell))
                        memoryCells.Add(outProcessMemoryCell);
                }
                else
                    memoryCells.Add(memoryCell);

            }
            return memoryCells;
        }

        internal System.Collections.Generic.IEnumerable<MemoryCell> GetMemoryCells(DataNode dataNode)
        {
            System.Collections.Generic.List<MetaObjectID> lazyFetchingMembersIdentities = ObjectData.GetLazyFetchingMembers(dataNode.Classifier, dataNode.SubDataNodes);


            List<ObjectData> objects = new List<ObjectData>(ObjectCollection.Distinct().Select(@object => new ObjectData(@object, Guid.Empty, lazyFetchingMembersIdentities)).ToList());
            return GetMemoryCells(objects, dataNode.Classifier.GetExtensionMetaObject<Type>(), dataNode.Identity);
            

            //Dictionary<string, MemoryCell> memoryCellsDictionary = new Dictionary<string, MemoryCell>();
            //foreach (object @object in ObjectCollection)
            //{
            //    if (@object is MarshalByRefObject && Remoting.RemotingServices.IsOutOfProcess(@object as MarshalByRefObject))
            //    {
            //        string channelUri = Remoting.RemotingServices.GetChannelUri(@object as MarshalByRefObject);
            //        string objectsContextIdentity = "OnMemory:" + channelUri;
            //        MemoryCell memoryCell = null;
            //        if (!memoryCellsDictionary.TryGetValue(objectsContextIdentity, out memoryCell))
            //        {

            //            memoryCell = new OutProcessMemoryCell(new List<ObjectData>(), CollectionItemType, GetOutProcessMemoryObjectsCotext(channelUri, QueryIdentity).Identity, channelUri, dataNode.Identity);
            //            memoryCellsDictionary[objectsContextIdentity] = memoryCell;
            //        }
            //        memoryCell.Objects[@object] = new ObjectData(@object, lazyFetchingMembersIdentities);
            //    }
            //    else
            //    {
            //        ObjectData objectData = new ObjectData(@object, lazyFetchingMembersIdentities);
            //        MemoryCell memoryCell = null;

            //        if (objectData.PartialLoaded)
            //        {
            //            if (!memoryCellsDictionary.TryGetValue(objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity, out memoryCell))
            //            {
            //                memoryCell = new PartialLoadedMemoryCell(new List<ObjectData>(), CollectionItemType, objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity, dataNode.Identity);
            //                memoryCellsDictionary[objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity] = memoryCell;
            //                MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
            //            }
            //        }
            //        else
            //        {
            //            if (!memoryCellsDictionary.TryGetValue(ObjectsContextIdentity, out memoryCell))
            //            {
            //                memoryCell = new InProcessMemoryCell(new List<ObjectData>(), CollectionItemType, ObjectsContextIdentity, dataNode.Identity);
            //                memoryCellsDictionary[ObjectsContextIdentity] = memoryCell;
            //                MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
            //            }
            //        }
            //        memoryCell.Objects[@object] = objectData;

            //    }
            //}
            //List<MemoryCell> memoryCells = new List<MemoryCell>();
            //foreach (var memoryCell in memoryCellsDictionary.Values)
            //{

            //    if (memoryCell is OutProcessMemoryCell)
            //    {
            //        foreach (OutProcessMemoryCell outProcessMemoryCell in GetOutProcessMemoryObjectsCotext((memoryCell as OutProcessMemoryCell).Channeluri, QueryIdentity).Synchronize(memoryCell as OutProcessMemoryCell))
            //            memoryCells.Add(outProcessMemoryCell);
            //    }
            //    else
            //        memoryCells.Add(memoryCell);


            //    //if (memoryCell is OutProcessMemoryCell)
            //    //{
            //    //    OutProcessMemoryCell outProcessMemoryCell = memoryCell as OutProcessMemoryCell;
            //    //    GetOutProcessMemoryObjectsCotext((memoryCell as OutProcessMemoryCell).Channeluri, QueryIdentity).Synchronize(ref outProcessMemoryCell);
            //    //    memoryCells.Add(outProcessMemoryCell);
            //    //}


            //}
            //return memoryCells;
        }
        internal Dictionary<Guid, MemoryCell> MemoryCells = new Dictionary<Guid, MemoryCell>();
        #region IObjectQueryPartialResolver Members

        public DistributedObjectQuery DistributeObjectQuery(Guid mainQueryIdentity,
                                                            List<DataNode> dataTrees,
                                                            QueryResultType queryResult,
                                                            List<DataNode> selectListItems,
                                                            Dictionary<Guid, DataLoaderMetadata> dataLoadersMetadata,
                                                            Dictionary<string, object> parameters,
                                                            System.Collections.Generic.List<string> usedAliases,
                                                            System.Collections.Generic.List<string> queryStorageIdentities)
        {
            foreach (var dataLoaderMetadataKey in new List<Guid>(dataLoadersMetadata.Keys))
            {
                if (dataLoadersMetadata[dataLoaderMetadataKey].MemoryCell is OutProcessMemoryCell &&
                    (dataLoadersMetadata[dataLoaderMetadataKey].MemoryCell as OutProcessMemoryCell).ObjectsContextIdentity == ObjectsContextIdentity)
                {

                    OutProcessMemoryCell memoryCell = dataLoadersMetadata[dataLoaderMetadataKey].MemoryCell as OutProcessMemoryCell;
                    OOAdvantech.Collections.Generic.List<ObjectData> objects = new List<ObjectData>(memoryCell.Objects.Values);
                    MemoryCell inProcessMemoryCell = MemoryCells[memoryCell.MemoryCellIdentity];
                    MemoryCells[inProcessMemoryCell.MemoryCellIdentity] = inProcessMemoryCell;
                    System.Collections.Generic.Dictionary<Guid, System.Collections.Generic.List<MemoryCellReference>> relatedMemoryCells = dataLoadersMetadata[dataLoaderMetadataKey].RelatedMemoryCells;
                    dataLoadersMetadata[dataLoaderMetadataKey] = new DataLoaderMetadata(dataLoadersMetadata[dataLoaderMetadataKey].DataNode, inProcessMemoryCell, this);
                    dataLoadersMetadata[dataLoaderMetadataKey].RelatedMemoryCells = relatedMemoryCells;
                }
            }
            DistributedObjectQuery objectQuery = new DistributedObjectQuery(mainQueryIdentity, dataTrees, queryResult, selectListItems, this, dataLoadersMetadata, parameters, usedAliases, queryStorageIdentities);
            return objectQuery;
        }

        public DataLoader CreateDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
        {
            return new MemroryObjectsDataLoader(dataNode, dataLoaderMetadata);
        }

        #endregion

        static System.Collections.Generic.Dictionary<System.Guid, WeakReference> ActiveMemoryObjectsContexts = new System.Collections.Generic.Dictionary<Guid, WeakReference>();

        public static void RemoveAllQueryMemoryContext(Guid queryIdentity)
        {
            ActiveMemoryObjectsContexts.Remove(queryIdentity);
        }


        public List<OutProcessMemoryCell> Synchronize(OutProcessMemoryCell outProcessMemoryCell)
        {
            Dictionary<string, OutProcessMemoryCell> outProcessMemoryCells = new Dictionary<string, OutProcessMemoryCell>();
            if (outProcessMemoryCell.MemoryCellIdentity == Guid.Empty)
            {
                List<ObjectData> objects = new List<ObjectData>(outProcessMemoryCell.Objects.Values);
                var memoryCells = (from memoryCell in MemoryCells.Values
                                   where memoryCell.DataNodeIdentity == outProcessMemoryCell.DataNodeIdentity
                                   select memoryCell).ToDictionary(mc => mc.ObjectsContextIdentity);
                
                foreach (ObjectData objectData in objects)
                {
                    objectData.CheckForPartialLoad();
                    if (!objectData.PartialLoaded)
                    {
                        MemoryCell memoryCell = null;
                        if (memoryCells.TryGetValue(this.ObjectsContextIdentity, out memoryCell))
                        {
                            if (!memoryCell.Objects.ContainsKey(objectData._Object))
                                memoryCell.Objects[objectData._Object] = objectData;
                            else
                            {
                                ObjectData existingObjectData = memoryCell.Objects[objectData._Object];
                                UpdateObjectDataRelations(objectData, existingObjectData);
                            }
                        }
                        else
                        {
                            memoryCell = new InProcessMemoryCell(new List<ObjectData>(), outProcessMemoryCell.Type, ObjectsContextIdentity, outProcessMemoryCell.DataNodeIdentity);
                            MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
                            memoryCells[ObjectsContextIdentity] = memoryCell;
                            memoryCell.Objects[objectData._Object] = objectData;
                        }
                        OutProcessMemoryCell newOutProcessMemoryCell = null;
                        if (!outProcessMemoryCells.TryGetValue(memoryCell.ObjectsContextIdentity, out newOutProcessMemoryCell))
                        {
                            newOutProcessMemoryCell = new OutProcessMemoryCell(memoryCell as InProcessMemoryCell, outProcessMemoryCell.Channeluri, outProcessMemoryCell.DataNodeIdentity);
                            outProcessMemoryCells[memoryCell.ObjectsContextIdentity] = newOutProcessMemoryCell;
                        }
                    }
                    else
                    {
                        MemoryCell memoryCell = null;
                        if (memoryCells.TryGetValue(objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity, out memoryCell))
                        {
                            if (!memoryCell.Objects.ContainsKey(objectData._Object))
                                memoryCell.Objects[objectData._Object] = objectData;
                            else
                            {
                                ObjectData existingObjectData = memoryCell.Objects[objectData._Object];
                                UpdateObjectDataRelations(objectData, existingObjectData);
                            }

                        }
                        else
                        {
                            memoryCell = new PartialLoadedMemoryCell(new List<ObjectData>(), outProcessMemoryCell.Type, objectData.StorageInstanceRef.StorageInstanceSet.StorageIdentity, outProcessMemoryCell.DataNodeIdentity);
                            MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
                            memoryCells[ObjectsContextIdentity] = memoryCell;
                            memoryCell.Objects[objectData._Object] = objectData;
                        }
                        OutProcessMemoryCell newOutProcessMemoryCell = null;
                        if (!outProcessMemoryCells.TryGetValue(memoryCell.ObjectsContextIdentity, out newOutProcessMemoryCell))
                        {
                            newOutProcessMemoryCell = new OutProcessMemoryCell(memoryCell as PartialLoadedMemoryCell, outProcessMemoryCell.Channeluri, outProcessMemoryCell.DataNodeIdentity);
                            outProcessMemoryCells[memoryCell.ObjectsContextIdentity] = newOutProcessMemoryCell;
                        }
                    }
                }

                return new List<OutProcessMemoryCell>(outProcessMemoryCells.Values);

            }
            else
            {
                return new List<OutProcessMemoryCell>() { outProcessMemoryCell };
            }



        }

        private static void UpdateObjectDataRelations(ObjectData objectData, ObjectData existingObjectData)
        {

            foreach (ObjectData parentObjectData in objectData.ParentDataNodeRelatedObjects.Values)
            {
                if (!existingObjectData.ParentDataNodeRelatedObjects.ContainsKey(parentObjectData._Object))
                    existingObjectData.ParentDataNodeRelatedObjects[parentObjectData._Object] = parentObjectData;
            }

            foreach (var subDataNodeIdentity in objectData.RelatedObjects.Keys)
            {
                if (existingObjectData.RelatedObjects.ContainsKey(subDataNodeIdentity))
                {
                    foreach (ObjectData parentObjectData in objectData.RelatedObjects[subDataNodeIdentity].Values)
                    {
                        if (!existingObjectData.RelatedObjects[subDataNodeIdentity].ContainsKey(parentObjectData._Object))
                            existingObjectData.RelatedObjects[subDataNodeIdentity][parentObjectData._Object] = parentObjectData;
                    }
                }
                else
                {
                    existingObjectData.RelatedObjects[subDataNodeIdentity] = objectData.RelatedObjects[subDataNodeIdentity];
                }
            }
        }

        /// <summary>
        /// This method looking for mememory context of query in process
        /// In case where does not exist create one
        /// </summary>
        /// <param name="queryIdentity">
        /// Defines the query Identity 
        /// </param>
        /// <returns>
        /// Returns memory context 
        /// </returns>
        public MemoryObjectsContext GetOueryObjectsCotext(Guid queryIdentity)
        {
            WeakReference memoryObjectsContextWeakReference = null;
            MemoryObjectsContext memoryObjectsContext = null;
            ActiveMemoryObjectsContexts.TryGetValue(queryIdentity, out memoryObjectsContextWeakReference);
            if (memoryObjectsContextWeakReference != null && memoryObjectsContextWeakReference.IsAlive)
                memoryObjectsContext = memoryObjectsContextWeakReference.Target as MemoryObjectsContext;
            if (memoryObjectsContext == null)
            {
                memoryObjectsContext = new MemoryObjectsContext("OnMemory:" + queryIdentity.ToString() + "_" + Guid.NewGuid().ToString());
                ActiveMemoryObjectsContexts[queryIdentity] = new WeakReference(memoryObjectsContext);
                memoryObjectsContext.QueryIdentity = queryIdentity;

            }
            return memoryObjectsContext;
        }

        /// <summary>
        /// Defines the out process memory context
        /// </summary>
        Dictionary<string, MemoryObjectsContext> OutProcessMemoryContexts = new Dictionary<string, MemoryObjectsContext>();

        /// <summary>
        /// Gets the out process MemoryObjectsContext  
        /// </summary>
        /// <param name="channeluri">
        /// Defines the comunication channel with the process
        /// </param>
        /// <param name="queriIdentity">
        /// Defines the identity of query where belongs the MemoryObjectsContext
        /// </param>
        /// <returns></returns>
        public MemoryObjectsContext GetOutProcessMemoryObjectsCotext(string channeluri, Guid queriIdentity)
        {
            MemoryObjectsContext outStorageMemoryContext = null;

#if !DeviceDotNet
            if (!OutProcessMemoryContexts.TryGetValue(channeluri.ToLower(), out outStorageMemoryContext))
            {
                outStorageMemoryContext = OOAdvantech.Remoting.RemotingServices.CreateRemoteInstance<MemoryObjectsContext>(channeluri);//, new Type[1] { typeof(string) }, "OnMemory:" + channeluri);
                outStorageMemoryContext = outStorageMemoryContext.GetOueryObjectsCotext(queriIdentity);

                OutProcessMemoryContexts[channeluri.ToLower()] = outStorageMemoryContext;
            }
#endif 
            return outStorageMemoryContext;
        }
        /// <summary>
        /// This method returns the related MemoryCells for the member with identity the memberIdentity 
        /// </summary>
        /// <param name="outProcessMemoryCell">
        /// Defines the memory cell where method looking for related memory cell
        /// </param>
        /// <param name="memberIdentity">
        /// Defines member where method looking for related memory cell
        /// </param>
        /// <param name="lazyFetchingMembersIdentities">
        /// Defines the members where method checks to decide if the member object is partial loaded or not 
        /// </param>
        /// <param name="queryStorageIdentities">
        /// Define a collection with storage identities of storages where participate in query  
        /// </param>
        /// <returns>
        /// Returns a collection related MemoryCell
        /// </returns>
        public List<RelatedMemoryCellData> GetRelatedMemoryCells(MemoryCell masterMemoryCell, MetaObjectID memberIdentity, Guid memberdataNodeIdentity, List<MetaObjectID> lazyFetchingMembersIdentities, ref System.Collections.Generic.List<string> queryStorageIdentities)
        {

            string memberStringIdentity = memberIdentity.ToString();

            MemoryCell inProcessMemoryCell = masterMemoryCell;
            OutProcessMemoryCell outProcessMemoryCell = null;
            if (masterMemoryCell is OutProcessMemoryCell)
            {
                inProcessMemoryCell = MemoryCells[masterMemoryCell.MemoryCellIdentity];
                outProcessMemoryCell = masterMemoryCell as OutProcessMemoryCell;
                //outProcessMemoryCell = masterMemoryCell as OutProcessMemoryCell;
                //if (!MemoryCells.TryGetValue(outProcessMemoryCell.MemoryCellIdentity, out inProcessMemoryCell))
                //    inProcessMemoryCell = MemoryCell.GetProcessMemoryCell(outProcessMemoryCell);
                //MemoryCells[inProcessMemoryCell.MemoryCellIdentity] = inProcessMemoryCell;
            }



            Classifier memoryCellClassifier = inProcessMemoryCell.Type;
            MetaObject memberMetaObject = memoryCellClassifier.GetMember(memberIdentity);
            Classifier relatedMemoryCellClassifier = null;
            OOAdvantech.Collections.Generic.List<ObjectData> subDataNodeObjects = new OOAdvantech.Collections.Generic.List<ObjectData>();

#region Gets subDataNodeObjects
            System.Collections.Generic.Dictionary<object, ObjectData> objectsMap = new System.Collections.Generic.Dictionary<object, ObjectData>();
            foreach (var objectData in inProcessMemoryCell.Objects.Values)
            {
                System.Collections.Generic.Dictionary<object, ObjectData> relatedObjects = null;
                if (!objectData.RelatedObjects.TryGetValue(memberStringIdentity, out relatedObjects))
                {
                    relatedObjects = new System.Collections.Generic.Dictionary<object, ObjectData>();
                    objectData.RelatedObjects[memberStringIdentity] = relatedObjects;
                }
                var _object = objectData._Object;


                if (memberMetaObject is MetaDataRepository.AssociationEnd)
                {
                    MetaDataRepository.AssociationEnd associationEnd = memberMetaObject as MetaDataRepository.AssociationEnd;
                    relatedMemoryCellClassifier = associationEnd.Specification;
                    object memberObject = null;
                    if (memoryCellClassifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                    {

#region Related object is relation object roleA or roleB
                        if (associationEnd.IsRoleA)
                        {
                            if (memoryCellClassifier is DotNetMetaDataRepository.Interface)
                                memberObject = (memoryCellClassifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(_object, null);
                            if (memoryCellClassifier is DotNetMetaDataRepository.Class)
                                memberObject = Member<object>.GetValue((memoryCellClassifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, _object);
                        }
                        else
                        {
                            if (memoryCellClassifier is DotNetMetaDataRepository.Interface)
                                memberObject = (memoryCellClassifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(_object, null);
                            if (memoryCellClassifier is DotNetMetaDataRepository.Class)
                                memberObject = Member<object>.GetValue((memoryCellClassifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, _object);
                        }

                        ObjectData relatedObjectData = null;
                        if (!objectsMap.TryGetValue(memberObject, out relatedObjectData))
                        {
                            relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, queryStorageIdentities.IndexOf(inProcessMemoryCell.ObjectsContextIdentity), lazyFetchingMembersIdentities);
                            objectsMap[memberObject] = relatedObjectData;
                            subDataNodeObjects.Add(relatedObjectData);
                        }
                        relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;
                        relatedObjects[relatedObjectData._Object] = relatedObjectData;

#endregion

                    }
                    else
                    {
                        if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
                            memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(_object);
                        else
                            memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, _object);
                        if (memberObject != null)
                        {
                            if (associationEnd.CollectionClassifier != null)
                            {
                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                enumerator.Reset();
                                while (enumerator.MoveNext())
                                {
                                    object collectionObj = enumerator.Current;
                                    ObjectData relatedObjectData = null;
                                    if (!objectsMap.TryGetValue(collectionObj, out relatedObjectData))
                                    {
                                        relatedObjectData = new ObjectData(collectionObj, objectData.ObjectID, queryStorageIdentities.IndexOf(inProcessMemoryCell.ObjectsContextIdentity), lazyFetchingMembersIdentities);
                                        objectsMap[collectionObj] = relatedObjectData;
                                        subDataNodeObjects.Add(relatedObjectData);
                                    }
                                    relatedObjects[relatedObjectData._Object] = relatedObjectData;
                                    relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;
                                }

                            }
                            else
                            {
                                ObjectData relatedObjectData = null;
                                if (!objectsMap.TryGetValue(memberObject, out relatedObjectData))
                                {
                                    relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, queryStorageIdentities.IndexOf(inProcessMemoryCell.ObjectsContextIdentity), lazyFetchingMembersIdentities);
                                    objectsMap[memberObject] = relatedObjectData;
                                    subDataNodeObjects.Add(relatedObjectData);
                                }

                                relatedObjects[relatedObjectData._Object] = relatedObjectData;
                                relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;

                            }
                        }
                    }
                }

                if (memberMetaObject is MetaDataRepository.Attribute)
                {
                    MetaDataRepository.Attribute attribute = memberMetaObject as MetaDataRepository.Attribute;


                    object memberObject = null;

                    if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(_object);
                    else
                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(_object);
                    if (memberObject != null)
                    {
                        if (memberObject != null)
                        {
                            ///TODO να γραφτεί test case για όλες τις περιπτώσεις enumerable collections
                            if (TypeHelper.IsEnumerable(memberObject.GetType()))
                            {

                                relatedMemoryCellClassifier = Classifier.GetClassifier(TypeHelper.GetElementType(memberObject.GetType()));

                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                enumerator.Reset();

                                while (enumerator.MoveNext())
                                {
                                    object collectionObj = enumerator.Current;
                                    ObjectData relatedObjectData = null;
                                    if (!objectsMap.TryGetValue(collectionObj, out relatedObjectData))
                                    {
                                        relatedObjectData = new ObjectData(collectionObj, objectData.ObjectID, queryStorageIdentities.IndexOf(inProcessMemoryCell.ObjectsContextIdentity), lazyFetchingMembersIdentities);
                                        objectsMap[collectionObj] = relatedObjectData;
                                        subDataNodeObjects.Add(relatedObjectData);
                                    }

                                    relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;


                                }
                            }
                            else
                            {
                                relatedMemoryCellClassifier = attribute.Type;
                                //ObjectData relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, sourceDataLoaderMetadata.QueryStorageID,dataNode );
                                ObjectData relatedObjectData = null;
                                if (!objectsMap.TryGetValue(memberObject, out relatedObjectData))
                                {
                                    relatedObjectData = new ObjectData(memberObject, objectData.ObjectID, queryStorageIdentities.IndexOf(inProcessMemoryCell.ObjectsContextIdentity), lazyFetchingMembersIdentities);
                                    objectsMap[memberObject] = relatedObjectData;
                                    subDataNodeObjects.Add(relatedObjectData);
                                }

                                relatedObjectData.ParentDataNodeRelatedObjects[objectData._Object] = objectData;
                            }
                        }
                    }
                }
            }

#endregion

#region Gets related memory cells

            List<RelatedMemoryCellData> relatedMemoryCells = new List<RelatedMemoryCellData>();
            foreach (var memoryCell in GetMemoryCells(subDataNodeObjects, relatedMemoryCellClassifier.GetExtensionMetaObject<Type>(), memberdataNodeIdentity))
            {
                OutProcessMemoryCell outProcessRelatedMemoryCell = null;
                if (memoryCell is OutProcessMemoryCell)
                {
                    outProcessRelatedMemoryCell = memoryCell as OutProcessMemoryCell;
                    relatedMemoryCells.Add(new RelatedMemoryCellData(outProcessRelatedMemoryCell, new List<StorageCell>()));
                }
                else if (memoryCell is PartialLoadedMemoryCell)
                {
                    if (outProcessMemoryCell != null)
                    {
                        outProcessRelatedMemoryCell = new OutProcessMemoryCell(memoryCell as PartialLoadedMemoryCell, outProcessMemoryCell.Channeluri, memberdataNodeIdentity);
                        relatedMemoryCells.Add(new RelatedMemoryCellData(outProcessRelatedMemoryCell, (memoryCell as PartialLoadedMemoryCell).StorageCells));
                    }
                    else
                        relatedMemoryCells.Add(new RelatedMemoryCellData(memoryCell, (memoryCell as PartialLoadedMemoryCell).StorageCells));
                }
                if (memoryCell is InProcessMemoryCell)
                {
                    if (outProcessMemoryCell != null)
                    {
                        outProcessRelatedMemoryCell = new OutProcessMemoryCell(memoryCell as InProcessMemoryCell, outProcessMemoryCell.Channeluri, memberdataNodeIdentity);
                        relatedMemoryCells.Add(new RelatedMemoryCellData(outProcessRelatedMemoryCell, new List<StorageCell>()));
                    }
                    else
                        relatedMemoryCells.Add(new RelatedMemoryCellData(memoryCell, new List<StorageCell>()));
                }

                MemoryCells[memoryCell.MemoryCellIdentity] = memoryCell;
            }

#endregion


            return relatedMemoryCells;

        }

        /// <summary>
        /// This field defines when all object that contains MemoryObjectsContext are proxies of objects which are instantiated in common process 
        /// </summary>
        internal readonly bool ObjectsHasOutProcessCommonMemoryContext;
    }

    /// <summary>
    /// Defines the relate memory cell data.
    /// </summary>
    /// <MetaDataID>{1d28b948-857b-4acd-be59-6559e678537f}</MetaDataID>
    [Serializable]
    public class RelatedMemoryCellData
    {
        /// <summary>
        /// Defines the related MemoryCell
        /// </summary>
        public readonly MemoryCell MemoryCell;
        /// <summary>
        /// Defines the StorargeCells in case where the  related MemoryCell is PartialLoadedMemoryCell 
        /// </summary>
        public List<StorageCell> StorageCells;

        /// <summary>
        /// Initialize the RelatedMemoryCellData instance
        /// </summary>
        /// <param name="memoryCell">
        /// Defines the related MemoryCell
        /// </param>
        /// <param name="storageCells">
        /// Defines the StorargeCells in case where the related MemoryCell is PartialLoadedMemoryCell 
        /// </param>
        public RelatedMemoryCellData(MemoryCell memoryCell, List<StorageCell> storageCells)
        {
            MemoryCell = memoryCell;
            StorageCells = storageCells;
        }


    }
}
