using PartialRelationIdentity = System.String;
using System.Collections.Generic;
using System.Linq;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{a59cccbd-fbac-40b2-81d5-20fe12323a2b}</MetaDataID>
    public abstract class StorageDataLoader : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader
    {

        ///<summary>
        ///ObjectActivation specifies when query engine converts the storage instance data to activate object
        ///True means object activation
        ///</summary>
        /// <MetaDataID>{a335684b-c8e4-4068-9090-e188143b6800}</MetaDataID>
        public override bool ObjectActivation
        {
            get
            {
                return (DataNode.DataSource as StorageDataSource).ObjectActivation;
            }
        }

        protected List<AggregateExpressionDataNode> _ResolvedAggregateExpressions = new List<AggregateExpressionDataNode>();
        public override List<AggregateExpressionDataNode> ResolvedAggregateExpressions
        {
            get
            {
                if (DataNode is GroupDataNode && GroupedDataLoaded)
                    return (from aggregationDataNode in DataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                            select aggregationDataNode).ToList();

                return _ResolvedAggregateExpressions;
            }
        }




        ///<summary>
        ///Returns the data loader for data node of parameter which retrieve data from the same objecct context with the called storage dataloader 
        ///</summary>
        ///<param name="dataNode">
        ///Defines the data node of data loader where operation calller wants
        ///</param>
        /// <MetaDataID>{6dba7461-b42e-4fdd-a935-6792f6ab560f}</MetaDataID>
        protected override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader GetDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            if (dataNode.ObjectQuery is DistributedObjectQuery)
                return (dataNode.ObjectQuery as DistributedObjectQuery).GetDataLoader(dataNode);

            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                return GetDataLoader(dataNode.ParentDataNode);
            MetaDataRepository.ObjectQueryLanguage.DataLoader dataLoader = null;
            if (dataNode.DataSource != null)
                dataNode.DataSource.DataLoaders.TryGetValue(DataLoaderMetadata.ObjectsContextIdentity, out dataLoader);
            return dataLoader;
        }
        ///<summary>Definew the table with the retrieved data drom data loader </summary>
        /// <MetaDataID>{6394a052-ee06-4442-9a91-2064f54530e5}</MetaDataID>
        public override IDataTable Data
        {
            get
            {
                if (DataLoadedInParentDataSource)
                    return GetDataLoader(DataNode.ParentDataNode).Data;
                return base.Data;
            }
        }
        #region Loads value type relation links
#if !DeviceDotNet
        /// <summary>
        /// Load massively object links from a table. 
        /// When a program asks from the system to retrieve object from storage   real or virtual, 
        /// system must restore the object links. 
        /// The object link represent as reference which keep a member of object 
        /// or as entry in collection of object. 
        /// Data loader set reference to the member of object, 
        /// or constructs the collection and set the corresponding member massively.
        /// </summary>
        /// <param name="valueTypePath">
        /// This parameter defines the path which used from data loader to reach the member.
        /// </param>
        /// <param name="streamedTable">
        /// This parameter defines a streamed table with object links. It is useful for remote calls.  
        /// </param>
        /// <MetaDataID>{80cc24b1-676d-4a2e-9836-03b6acc3c57e}</MetaDataID>
        internal void LoadObjectRelationLinks(MetaDataRepository.ValueTypePath valueTypePath, System.Collections.Generic.Dictionary<string, StreamedTable> streamedTable)
        {
            System.Collections.Generic.Dictionary<string, IDataTable> dataTable = new System.Collections.Generic.Dictionary<string, IDataTable>();
            foreach (var entry in streamedTable)
                dataTable[entry.Key] = DataSource.DataObjectsInstantiator.CreateDataTable(entry.Value);
            LoadObjectRelationLinks(valueTypePath, dataTable);
        }
#endif

        ///<summary>
        ///This method load object link between data loader objects and parent datanode out storage dataloader objects 
        ///</summary>
        ///<param name="interStorageObjectLinks">
        ///Defines a dictionary with relation part identity as key and objects links relation data.
        ///</param>
        ///<param name="referenceObjectIdentityTypes">
        /// This parameter defines a collaction with reference ObjectIdentityTypes of ObjectIdentities which contains the data table for each relation part
        ///</param>
        ///<returns>
        ///Returns a dictionary with relation part identity as key and objects links relation data.
        ///The object links now has linked object from both stoarage.
        ///The related object from datanode parent storage now has been moved to OwnerObject column 
        ///and the just retrieved from dataloader object loaded to RelatedObject column 
        ///</returns>
        /// <MetaDataID>{d3ce9cc6-422c-4103-a72d-9929c5a3d99c}</MetaDataID>
        internal Dictionary<PartialRelationIdentity, IDataTable> LoadInterStorageObjectLinksWithParent(Dictionary<PartialRelationIdentity, IDataTable> interStorageObjectLinks, System.Collections.Generic.Dictionary<PartialRelationIdentity, System.Collections.Generic.List<ObjectIdentityType>> referenceObjectIdentityTypes)
        {
            //System.Collections.Generic.List<ObjectIdentityType> referenceObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
            //foreach (System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes in relationTableObjectIdentityTypes.Values)
            //{
            //    foreach (ObjectIdentityType objectIdentityType in objectIdentityTypes)
            //    {
            //        if (ObjectIdentityTypes.Contains(objectIdentityType))
            //            referenceObjectIdentityTypes.Add(objectIdentityType);
            //    }
            //}
            ObjectIdentityType lastRowReferenceObjectIdentityType = null;

            foreach (var relationPartIdentity in interStorageObjectLinks.Keys)
            {
                IDataTable dataTable = interStorageObjectLinks[relationPartIdentity];
                DotNetMetaDataRepository.AssociationEnd associationEnd = (DataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd() as DotNetMetaDataRepository.AssociationEnd;

                #region Retrieves association and for relationPartIdentity
                if (associationEnd.Identity.ToString() != relationPartIdentity)
                {
                    foreach (var association in associationEnd.Association.Specializations)
                    {
                        if (association.RoleA.Identity.ToString() == relationPartIdentity ||
                            association.RoleB.Identity.ToString() == relationPartIdentity)
                        {
                            if (associationEnd.IsRoleA)
                                associationEnd = association.RoleA as DotNetMetaDataRepository.AssociationEnd;
                            else
                                associationEnd = association.RoleB as DotNetMetaDataRepository.AssociationEnd;
                            break;
                        }
                    }
                }
                #endregion

                if (!associationEnd.Multiplicity.IsMany)
                {
                    foreach (IDataRow dataRow in dataTable.Rows)
                    {
                        PersistenceLayer.ObjectID objectID = GetObjectIDFromDataRow(dataRow, referenceObjectIdentityTypes[relationPartIdentity], ref lastRowReferenceObjectIdentityType);

                        //object[] partValues = null;
                        //object firstPartValue = null;
                        //if (lastRowReferenceObjectIdentityType != null)
                        //    firstPartValue = dataRow[lastRowReferenceObjectIdentityType.Parts[0].Name];

                        //if (lastRowReferenceObjectIdentityType == null || firstPartValue == null)
                        //{
                        //    #region Search for the row reference object identity type
                        //    foreach (ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                        //    {
                        //        firstPartValue = dataRow[objectIdentityType.Parts[0].Name];
                        //        if (firstPartValue != null)
                        //        {
                        //            if (lastRowReferenceObjectIdentityType != objectIdentityType)
                        //            {
                        //                lastRowReferenceObjectIdentityType = objectIdentityType;
                        //                partValues = new object[lastRowReferenceObjectIdentityType.Parts.Count];
                        //            }
                        //            break;
                        //        }
                        //    }
                        //    #endregion
                        //}

                        //if (firstPartValue == null)
                        //    continue;//if firstPartValue is null then the refernce object identity is null, therefore the linked object is null  

                        //#region Gets ObjectID
                        //partValues[0] = firstPartValue;
                        //if (partValues.Length > 1)
                        //{
                        //    int i = 0;
                        //    foreach (IIdentityPart part in lastRowReferenceObjectIdentityType.Parts)
                        //    {
                        //        if (i == 0)
                        //        {
                        //            i++;
                        //            continue;
                        //        }
                        //        partValues[i] = dataRow[part.Name];
                        //        i++;
                        //    }
                        //}
                        //PersistenceLayer.ObjectID objectID = new OOAdvantech.PersistenceLayer.ObjectID(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(lastRowReferenceObjectIdentityType)], partValues, 0);
                        //#endregion

                        object ownerObject = null;
                        if (objectID != null)
                            ownerObject = GetObjectFromIdentity(objectID);
                        object relatedObject = dataRow["RelatedObject"];

                        if (ownerObject is System.DBNull)
                            ownerObject = null;
                        if (relatedObject is System.DBNull)
                            relatedObject = null;
                        if (ownerObject != null)
                            LoadObjectsLink(associationEnd, ownerObject, relatedObject);

                        ///Load object in opposite way on table row for objectsLinks loading in parent data node at the other storage 
                        dataRow["OwnerObject"] = relatedObject;
                        dataRow["RelatedObject"] = ownerObject;
                    }
                }
                else if (associationEnd.Association == DataNode.Classifier.ClassHierarchyLinkAssociation)
                {
                    foreach (IDataRow dataRow in dataTable.Rows)
                    {

                        PersistenceLayer.ObjectID objectID = GetObjectIDFromDataRow(dataRow, referenceObjectIdentityTypes[relationPartIdentity], ref lastRowReferenceObjectIdentityType); ;
                        object relatedObject = dataRow["RelatedObject"];
                        object ownerObject = null;
                        if (objectID != null)
                            ownerObject = GetObjectFromIdentity(objectID);

                        if (ownerObject is System.DBNull)
                            ownerObject = null;
                        if (relatedObject is System.DBNull)
                            relatedObject = null;
                        if (ownerObject != null)
                        {
                            MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as MetaDataRepository.Classifier;
                            if (classifier is DotNetMetaDataRepository.Class && associationEnd.IsRoleA)
                                Member<object>.SetValueImplicitly((classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor, ref ownerObject, relatedObject);
                            if (classifier is DotNetMetaDataRepository.Class && !associationEnd.IsRoleA)
                                Member<object>.SetValueImplicitly((classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor, ref ownerObject, relatedObject);
                        }
                        ///Load object in opposite way on table row for objectsLinks loading in parent data node at the other storage 
                        dataRow["OwnerObject"] = relatedObject;
                        dataRow["RelatedObject"] = ownerObject;
                    }
                }
                else
                {
                    //The associtionEnd in opposite direction is many and parent dataloader may be haven't all related object
                    foreach (IDataRow dataRow in dataTable.Rows)
                    {
                        PersistenceLayer.ObjectID objectID = GetObjectIDFromDataRow(dataRow, referenceObjectIdentityTypes[relationPartIdentity], ref lastRowReferenceObjectIdentityType); ;
                        object ownerObject = null;
                        if (objectID != null)
                            ownerObject = GetObjectFromIdentity(objectID);
                        object relatedObject = dataRow["RelatedObject"];
                        if (ownerObject is System.DBNull)
                            ownerObject = null;
                        if (relatedObject is System.DBNull)
                            relatedObject = null;
                        ///Load object in opposite way on table row for objectsLinks loading in parent data node at the other storage 
                        dataRow["OwnerObject"] = relatedObject;
                        dataRow["RelatedObject"] = ownerObject;
                    }
                }
            }
            return interStorageObjectLinks;
        }


        ///<summary>
        ///This method load retrieve the related object from relation data.
        ///</summary>
        ///<param name="interStorageObjectLinks">
        ///Defines a dictionary with relation part identity as key and objects links relation data.
        ///</param>
        ///<param name="referenceObjectIdentityTypes">
        /// This parameter defines a collaction with reference ObjectIdentityTypes of ObjectIdentities which contains the data table for each relation part
        ///</param>
        ///<returns>
        ///Returns a dictionary with relation part identity as key and objects links relation data.
        ///The object links now has linked object from both stoarage.
        ///The related object from datanode parent storage now has been moved to OwnerObject column 
        ///and the just retrieved from dataloader object loaded to RelatedObject column 
        ///</returns>
        /// <MetaDataID>{05e69ab8-fc56-4914-b937-cd5df6b1618e}</MetaDataID>
        internal System.Collections.Generic.Dictionary<PartialRelationIdentity, IDataTable> GetRelatedObject(Dictionary<PartialRelationIdentity, IDataTable> interStorageObjectLinks, Dictionary<PartialRelationIdentity, List<ObjectIdentityType>> referenceObjectIdentityTypes)
        {



            //System.Collections.Generic.List<ObjectIdentityType> referenceObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
            //foreach (System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes in referenceObjectIdentityTypes.Values)
            //{
            //    foreach (ObjectIdentityType objectIdentityType in objectIdentityTypes)
            //    {
            //        if (ObjectIdentityTypes.Contains(objectIdentityType))
            //            referenceObjectIdentityTypes.Add(objectIdentityType);
            //    }
            //}
            foreach (var interStorageObjectLinksEntry in interStorageObjectLinks)
            {
                PartialRelationIdentity relationPartIdentity = interStorageObjectLinksEntry.Key;
                ObjectIdentityType lastRowReferenceObjectIdentityType = null;
                //object[] partValues = null;
                //object firstPartValue = null;
                foreach (IDataRow dataRow in interStorageObjectLinksEntry.Value.Rows)
                {

                    PersistenceLayer.ObjectID objectID = GetObjectIDFromDataRow(dataRow, referenceObjectIdentityTypes[relationPartIdentity], ref lastRowReferenceObjectIdentityType);
                    //if (referenceObjectIdentityType != null)
                    //    firstPartValue = dataRow[referenceObjectIdentityType.Parts[0].Name];
                    //if (referenceObjectIdentityType == null || firstPartValue == null)
                    //{
                    //    foreach (ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                    //    {
                    //        firstPartValue = dataRow[objectIdentityType.Parts[0].Name];
                    //        if (firstPartValue != null)
                    //        {
                    //            if (referenceObjectIdentityType != objectIdentityType)
                    //            {
                    //                referenceObjectIdentityType = objectIdentityType;
                    //                partValues = new object[referenceObjectIdentityType.Parts.Count];
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (firstPartValue == null)
                    //    continue;
                    //partValues[0] = firstPartValue;
                    //if (partValues.Length > 1)
                    //{
                    //    int i = 0;
                    //    foreach (IIdentityPart part in referenceObjectIdentityType.Parts)
                    //    {
                    //        if (i == 0)
                    //        {
                    //            i++;
                    //            continue;
                    //        }
                    //        partValues[i] = dataRow[part.Name];
                    //        i++;
                    //    }
                    //}
                    //objectID = new OOAdvantech.PersistenceLayer.ObjectID(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(referenceObjectIdentityType)], partValues, 0);



                    // for other and of relation the the related object is the owner and owner is the related

                    dataRow["OwnerObject"] = dataRow["RelatedObject"];
                    dataRow["RelatedObject"] = GetObjectFromIdentity(objectID);// dataRow["RelatedObject"];
                    
                    // the table is ready for other end of association initialization
                }
            }
            return interStorageObjectLinks;


        }


        /// <summary>
        /// This method  retrieves object identity from objects links relation data row
        /// </summary>
        /// <param name="dataRow">
        /// This parameter define the data row with objects links relation data
        /// </param>
        /// <param name="referenceObjectIdentityTypes">
        /// This parameter defines a collaction with reference ObjectIdentityTypes of ObjectIdentities which contains the datarow
        /// the datarow can have values only for one reference ObjectIdentityType.
        /// </param>
        /// <param name="lastRowReferenceObjectIdentityType">
        /// Defines the last row reference ObjectIdentityType where the last row has object identity values
        /// </param>
        /// <returns>
        /// Returns reference object identity
        /// </returns>
        private PersistenceLayer.ObjectID GetObjectIDFromDataRow(IDataRow dataRow, System.Collections.Generic.List<ObjectIdentityType> referenceObjectIdentityTypes, ref ObjectIdentityType lastRowReferenceObjectIdentityType)
        {
            PersistenceLayer.ObjectID objectID = null;
            object[] partValues = null;
            object firstPartValue = null;
            if (lastRowReferenceObjectIdentityType != null)
                firstPartValue = dataRow[lastRowReferenceObjectIdentityType.Parts[0].Name];
            if (lastRowReferenceObjectIdentityType == null || firstPartValue == null)
            {
                foreach (ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                {
                    firstPartValue = dataRow[objectIdentityType.Parts[0].Name];
                    if (firstPartValue != null)
                    {
                        if (lastRowReferenceObjectIdentityType != objectIdentityType)
                        {
                            lastRowReferenceObjectIdentityType = objectIdentityType;
                            partValues = new object[lastRowReferenceObjectIdentityType.Parts.Count];
                        }
                        break;
                    }
                }
            }
            else 
                partValues = new object[lastRowReferenceObjectIdentityType.Parts.Count];
            if (firstPartValue != null)
            {
                partValues[0] = firstPartValue;
                if (partValues.Length > 1)
                {
                    int i = 0;
                    foreach (IIdentityPart part in lastRowReferenceObjectIdentityType.Parts)
                    {
                        if (i == 0)
                        {
                            i++;
                            continue;
                        }
                        partValues[i] = dataRow[part.Name];
                        i++;
                    }
                }
                objectID = new OOAdvantech.PersistenceLayer.ObjectID(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(lastRowReferenceObjectIdentityType)], partValues, 0);
            }
            return objectID;
        }



        /// <summary>
        /// Load massively object links from a table. 
        /// When a program asks from the system to retrieve object from storage   real or virtual, 
        /// system must restore the object links. 
        /// The object link represent as reference which keep a member of object 
        /// or as entry in collection of object. 
        /// Data loader set reference to the member of object, 
        /// or constructs the collection and set the corresponding member massively.
        /// </summary>
        /// <param name="valueTypePath">
        /// This parameter defines the path which used from data loader to reach the member.
        /// </param>
        /// <param name="dataTable">
        /// This parameter defines the table with object links. 
        /// </param>
        /// <MetaDataID>{94bca4b9-e03a-4251-9557-a568c09e62b1}</MetaDataID>
        internal void LoadObjectRelationLinks(MetaDataRepository.ValueTypePath valueTypePath, System.Collections.Generic.Dictionary<string, IDataTable> dataTables)
        {


            foreach (var dataTableEntry in dataTables)
            {
                IDataTable dataTable = dataTableEntry.Value;
                DotNetMetaDataRepository.AssociationEnd associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(valueTypePath.Peek()) as DotNetMetaDataRepository.AssociationEnd;
                if (!associationEnd.Multiplicity.IsMany)
                {
                    foreach (IDataRow dataRow in dataTable.Rows)
                    {
                        object ownerObject = dataRow["OwnerObject"];
                        object relatedObject = dataRow["RelatedObject"];
                        if (ownerObject is System.DBNull)
                            ownerObject = null;
                        if (relatedObject is System.DBNull)
                            relatedObject = null;

                        LoadObjectsLink(associationEnd, valueTypePath, ownerObject, relatedObject);
                    }
                }
                else
                {
                    System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<object>> objectsLinks = new Dictionary<object, List<object>>(); 
                    foreach (IDataRow dataRow in dataTable.Rows)
                    {
                        object ownerObject = dataRow["OwnerObject"];
                        object relatedObject = dataRow["RelatedObject"];
                        if (ownerObject is System.DBNull)
                            ownerObject = null;
                        if (relatedObject is System.DBNull)
                            relatedObject = null;
                        if (ownerObject != null)
                        {
                            if (!objectsLinks.ContainsKey(ownerObject))
                                objectsLinks.Add(ownerObject, new List<object>());
                            objectsLinks[ownerObject].Add(relatedObject);
                        }
                    }
                    foreach (System.Collections.Generic.KeyValuePair<object, System.Collections.Generic.List<object>> entry in objectsLinks)
                        LoadObjectsLinks(associationEnd, valueTypePath, entry.Key, entry.Value);


                }
            }


        }

        #endregion

        #region Load objects relation links
#if !DeviceDotNet
        /// <summary>
        /// Load massively object links from a table. 
        /// When a program asks from the system to retrieve object from storage   real or virtual, 
        /// system must restore the object links. 
        /// The object link represent as reference which keep a member of object 
        /// or as entry in collection of object. 
        /// Data loader set reference to the member of object, 
        /// or constructs the collection and set the corresponding member massively.
        /// </summary>
        /// <param name="metaObjectID">
        /// This parameter defines the identity of member 
        /// </param>
        /// <param name="streamedTable">
        /// This parameter defines a streamed table with object links. It is useful for remote calls.  
        /// </param>
        /// <MetaDataID>{c9bccb3b-6e53-4118-9323-28237563006a}</MetaDataID>
        internal void LoadObjectRelationLinks(MetaObjectID metaObjectID, System.Collections.Generic.Dictionary<string, StreamedTable> streamedTable)
        {
            System.Collections.Generic.Dictionary<string, IDataTable> dataTable = new System.Collections.Generic.Dictionary<string, IDataTable>();
            foreach (var entry in streamedTable)
                dataTable[entry.Key] =DataSource.DataObjectsInstantiator.CreateDataTable(entry.Value);
            LoadObjectRelationLinks(metaObjectID, dataTable);
        }
#endif


        /// <summary>
        /// Load massively object links from a table. 
        /// When a program asks from the system to retrieve object from storage   real or virtual, 
        /// system must restore the object links. 
        /// The object link represent as reference which keep a member of object 
        /// or as entry in collection of object. 
        /// Data loader set reference to the member of object, 
        /// or constructs the collection and set the corresponding member massively.
        /// </summary>
        /// <param name="metaObjectID">
        /// This parameter defines the identity of member 
        /// </param>
        /// <param name="dataTable">
        /// This parameter defines the table with object links. 
        /// </param>
        /// <MetaDataID>{45a4b8c8-1553-485e-9ea3-735b6280c5e9}</MetaDataID>
        internal void LoadObjectRelationLinks(MetaObjectID metaObjectID, System.Collections.Generic.Dictionary<string, IDataTable> objectsLinksTables)
        {
            if (DataNode.AssignedMetaObject is AssociationEnd &&
                   (DataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Identity == metaObjectID)
            {
                //Opposite direction subDataNode parent DataNode zero or one multiplicity  objectsLinks 

                #region Opposite direction subDataNode parent DataNode zero or one multiplicity  objectsLinks 
                foreach (string relationPartIdentity in objectsLinksTables.Keys)
                {
                    DotNetMetaDataRepository.AssociationEnd associationEnd = (DataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd() as DotNetMetaDataRepository.AssociationEnd;

                    #region Gets association end from relationPartIdentity
                    if (associationEnd.Identity.ToString() != relationPartIdentity &&
                             associationEnd.GetOtherEnd().Identity.ToString() != relationPartIdentity)
                    {
                        foreach (var association in associationEnd.Association.Specializations)
                        {
                            if (association.RoleA.Identity.ToString() == relationPartIdentity ||
                                association.RoleB.Identity.ToString() == relationPartIdentity)
                            {
                                if (associationEnd.IsRoleA)
                                    associationEnd = association.RoleA as DotNetMetaDataRepository.AssociationEnd;
                                else
                                    associationEnd = association.RoleB as DotNetMetaDataRepository.AssociationEnd;
                                break;

                            }

                        }
                    }
                    #endregion

                    IDataTable dataTable = objectsLinksTables[relationPartIdentity];
                    if (!associationEnd.Multiplicity.IsMany)  // Zero or one multiplicity association end
                    {
                        foreach (IDataRow dataRow in dataTable.Rows)
                        {
                            object ownerObject = dataRow["OwnerObject"];
                            object relatedObject = dataRow["RelatedObject"];
                            if (ownerObject is System.DBNull)
                                ownerObject = null;
                            if (relatedObject is System.DBNull)
                                relatedObject = null;
                            LoadObjectsLink(associationEnd, ownerObject, relatedObject);
                        }
                    }
                    if (associationEnd.Association == DataNode.Classifier.ClassHierarchyLinkAssociation)  //  LinkClass 
                    {
                        foreach (IDataRow dataRow in dataTable.Rows)
                        {
                            object ownerObject = dataRow["OwnerObject"];
                            object relatedObject = dataRow["RelatedObject"];
                            if (ownerObject is System.DBNull)
                                ownerObject = null;
                            if (relatedObject is System.DBNull)
                                relatedObject = null;
                            MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as MetaDataRepository.Classifier;
                            if (classifier is DotNetMetaDataRepository.Class && associationEnd.IsRoleA)
                                Member<object>.SetValueImplicitly((classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor, ref ownerObject, relatedObject);
                            if (classifier is DotNetMetaDataRepository.Class && !associationEnd.IsRoleA)
                                Member<object>.SetValueImplicitly((classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor, ref ownerObject, relatedObject);
                        }
                    }
                } 
                #endregion
            }
            else
            {
                foreach (DataNode subNode in DataNode.SubDataNodes)
                {
                    if (subNode.AssignedMetaObject is AssociationEnd &&
                        (subNode.AssignedMetaObject as AssociationEnd).Identity == metaObjectID)
                    {
                        foreach (string relationPartIdentity in objectsLinksTables.Keys)
                        {
                            DotNetMetaDataRepository.AssociationEnd associationEnd = subNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd;

                            #region Gets association end from relationPartIdentity
                            if (associationEnd.Identity.ToString() != relationPartIdentity &&
                                associationEnd.GetOtherEnd().Identity.ToString() != relationPartIdentity)
                            {
                                foreach (var association in associationEnd.Association.Specializations)
                                {
                                    if (association.RoleA.Identity.ToString() == relationPartIdentity ||
                                        association.RoleB.Identity.ToString() == relationPartIdentity)
                                    {
                                        if (associationEnd.IsRoleA)
                                            associationEnd = association.RoleA as DotNetMetaDataRepository.AssociationEnd;
                                        else
                                            associationEnd = association.RoleB as DotNetMetaDataRepository.AssociationEnd;
                                        break;
                                    }
                                }
                            }
                            #endregion


                            IDataTable dataTable = objectsLinksTables[relationPartIdentity];
                            if (!associationEnd.Multiplicity.IsMany)
                            {

                                #region set single object member
                                foreach (IDataRow dataRow in dataTable.Rows)
                                {
                                    object ownerObject = dataRow["OwnerObject"];
                                    object relatedObject = dataRow["RelatedObject"];
                                    if (ownerObject is System.DBNull)
                                        ownerObject = null;
                                    if (relatedObject is System.DBNull)
                                        relatedObject = null;

                                    LoadObjectsLink(associationEnd, ownerObject, relatedObject);
                                    if (associationEnd.Association.General != null)
                                    {
                                        if (associationEnd.IsRoleA)
                                            DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.Association.General.RoleA, ownerObject, relatedObject);
                                        else
                                            DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.Association.General.RoleB, ownerObject, relatedObject);
                                    }
                                } 
                                #endregion

                            }
                            else if (associationEnd.Association.LinkClass == DataNode.Classifier)
                            {
                                #region sets link class roleA or roleB member
                                foreach (IDataRow dataRow in dataTable.Rows)
                                {
                                    object ownerObject = dataRow["OwnerObject"];
                                    object relatedObject = dataRow["RelatedObject"];
                                    if (ownerObject is System.DBNull)
                                        ownerObject = null;
                                    if (relatedObject is System.DBNull)
                                        relatedObject = null;
                                    MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as MetaDataRepository.Classifier;
                                    if (classifier is DotNetMetaDataRepository.Class && associationEnd.IsRoleA)
                                        Member<object>.SetValueImplicitly((classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor, ref ownerObject, relatedObject);
                                    if (classifier is DotNetMetaDataRepository.Class && !associationEnd.IsRoleA)
                                        Member<object>.SetValueImplicitly((classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor, ref ownerObject, relatedObject);
                                } 
                                #endregion
                            }
                            else
                            {
                                #region loads objects collection  member 
                                System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<object>> objectsLinks = new Dictionary<object, List<object>>();
                                var rows = dataTable.Rows.OfType<IDataRow>();
                                if (associationEnd.Indexer)
                                    rows = rows.OrderBy(x => ((int)x["sortIndex"]));


                                foreach (IDataRow dataRow in rows)
                                {
                                    object ownerObject = dataRow["OwnerObject"];
                                    object relatedObject = dataRow["RelatedObject"];
                                    if (ownerObject is System.DBNull)
                                        ownerObject = null;
                                    if (relatedObject is System.DBNull)
                                        relatedObject = null;
                                    if (ownerObject != null)
                                    {
                                        if (!objectsLinks.ContainsKey(ownerObject))
                                            objectsLinks.Add(ownerObject, new List<object>());
                                        if (relatedObject != null && !(relatedObject is System.DBNull))
                                            objectsLinks[ownerObject].Add(relatedObject);

                                        if (associationEnd.Association.General != null) // General association
                                        {
                                            if (associationEnd.IsRoleA)
                                                DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.Association.General.RoleA, ownerObject, relatedObject);
                                            else
                                                DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.Association.General.RoleB, ownerObject, relatedObject);
                                        }
                                    }
                                }
                                foreach (System.Collections.Generic.KeyValuePair<object, System.Collections.Generic.List<object>> entry in objectsLinks)
                                    LoadObjectsLinks(associationEnd, entry.Key, entry.Value); 
                                #endregion

                            }

                        }

                        break;
                    }
                }


            }

        }

        #endregion

        /// <MetaDataID>{7af905c9-ca05-49f6-bc7b-96447d04fe44}</MetaDataID>
        abstract protected void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject);
        /// <MetaDataID>{dcddd90e-0338-4bca-b7e6-07d22bd7444a}</MetaDataID>
        abstract protected void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, System.Collections.Generic.List<object> relatedObjects);
        /// <MetaDataID>{cc566994-17e1-4c19-bd60-733a4ed3e0f0}</MetaDataID>
        abstract protected void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, object relatedObject);
        /// <MetaDataID>{d06afdb8-748f-4fa2-abed-7bb221a6a983}</MetaDataID>
        abstract protected void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, System.Collections.Generic.List<object> relatedObjects);




        /// <MetaDataID>{1223ccc8-b72b-4de5-8775-289eea31599e}</MetaDataID>
        System.Collections.Generic.Stack<DataNode> CloneRoute(System.Collections.Generic.Stack<DataNode> route)
        {
            System.Collections.Generic.Stack<DataNode> clonedRoute = new System.Collections.Generic.Stack<DataNode>();
            DataNode[] dataNodes = route.ToArray();
            for (int i = dataNodes.Length; i != 0; i--)
            {
                clonedRoute.Push(dataNodes[i - 1]);
            }
            return clonedRoute;
        }




        /// <exclude>Excluded</exclude>
        protected System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]> _LocalResolvedCriterions;

        /// <MetaDataID>{167f45b8-6e06-4e18-9eee-6481021f5ad1}</MetaDataID>
        /// <summary>
        /// Defines a collection with local resolved filter critirions
        /// </summary>
        public virtual System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]> LocalResolvedCriterions
        {
            get
            {
                if (_LocalResolvedCriterions != null)
                    return _LocalResolvedCriterions;
                else
                {
                    FindLocalAndGlobalResolvedCiterions();
                    return _LocalResolvedCriterions;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        protected System.Collections.Generic.List<Criterion> _GlobalResolveCriterions;
        /// <MetaDataID>{211559a5-2d9a-4396-852e-167dd2840cd6}</MetaDataID>
        /// <summary>
        /// Defines a collection with the filter criterions which can't resolved localy
        /// </summary>
        public virtual System.Collections.Generic.List<Criterion> GlobalResolveCriterions
        {
            get
            {
                if (_GlobalResolveCriterions == null)
                    FindLocalAndGlobalResolvedCiterions();
                return _GlobalResolveCriterions;
            }
        }




        /// <exclude>Excluded</exclude>
        protected System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]> _LocalOnMemoryResolvedCriterions;
        /// <MetaDataID>{211559a5-2d9a-4396-852e-167dd2840cd6}</MetaDataID>
        /// <summary>
        /// Defines the collection with local resolved filter critirions which can't resolved from the Data Base Managment System 
        /// </summary>
        public virtual System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]> LocalOnMemoryResolvedCriterions
        {
            get
            {
                if (_LocalOnMemoryResolvedCriterions == null)
                    FindLocalAndGlobalResolvedCiterions();
                return _LocalOnMemoryResolvedCriterions;
            }
        }








        ///// <MetaDataID>{c3ad7afc-46f8-455a-a68d-a77f7aea12c4}</MetaDataID>
        //protected override bool CriterionsOnDataLoaderObjectsResolvedLocally
        //{
        //    get
        //    {
        //        foreach (var criterion in GlobalResolveCriterions)
        //        {
        //            if (criterion.FirstDataNode == DataNode || criterion.SecondDataNode == DataNode)
        //                return false;
        //        }


        //        return true;
        //    }
        //}


        bool _GroupedDataLoaded = false;

        public override bool GroupedDataLoaded
        {
            get
            {
                return _GroupedDataLoaded;
            }
            protected internal set
            {
                _GroupedDataLoaded = value;
            }
        }
        /// <exclude>Excluded</exclude>
        bool? _ParticipateInGlobalResolvedCriterion;
        /// Data node participate in filter criterions that cannot be resolved localy.
        public override bool ParticipateInGlobalResolvedCriterion
        {
            get
            {
                if (!_ParticipateInGlobalResolvedCriterion.HasValue)
                {
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (DataNode subDataNode in DataNode.SubDataNodes)
                        {
                            if (subDataNode.Type == DataNode.DataNodeType.Object &&
                                subDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.ObjectsContextIdentity) && (subDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ParticipateInOnMemoryResolvedCriterions)
                                _ParticipateInGlobalResolvedCriterion = true;
                        }
                    }
                    if (GlobalResolveCriterions == null || GlobalResolveCriterions.Count == 0)
                    {
                        _ParticipateInGlobalResolvedCriterion = false;
                        foreach (var criterion in DataNode.BranchSearchCriterions)
                        {

                            if (DataNode.ObjectQuery is DistributedObjectQuery && (DataNode.ObjectQuery as DistributedObjectQuery).IsGlobalResolvedCriterion(criterion))
                            {
                                _ParticipateInGlobalResolvedCriterion = true;
                                return _ParticipateInGlobalResolvedCriterion.Value;
                            }

                            DataNode searchConditionDataNode = GetSearchConditionDataNode(DataNode.HeaderDataNode, criterion.SearhConditionHeader);
                            var dataNodesRoute = new System.Collections.Generic.Stack<DataNode>();
                            if (criterion.LeftTermDataNode != null)
                            {
                                searchConditionDataNode.BuildRoute(criterion.LeftTermDataNode, dataNodesRoute);
                                if (!ExistOnlyLocalRoute(GetDataLoader(searchConditionDataNode) as StorageDataLoader, CloneRoute(dataNodesRoute)))
                                {
                                    _ParticipateInGlobalResolvedCriterion = true;
                                    break;
                                }
                            }
                            if (criterion.RightTermDataNode != null)
                            {
                                searchConditionDataNode.BuildRoute(criterion.RightTermDataNode,  dataNodesRoute);
                                if (!ExistOnlyLocalRoute(GetDataLoader(searchConditionDataNode) as StorageDataLoader, CloneRoute(dataNodesRoute)))
                                {
                                    _ParticipateInGlobalResolvedCriterion = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        _ParticipateInGlobalResolvedCriterion = true;
                }
                return _ParticipateInGlobalResolvedCriterion.Value;


            }
        }




        bool? _ParticipateInOnmemoryResolvedCriterions;
        /// <MetaDataID>{30bbd812-7903-4ffc-b957-7be735e54eac}</MetaDataID>
        /// <summary>
        /// Data node participate in filter criterions that cannot be resolved from nadive Data managment system.
        /// </summary>
        public bool ParticipateInOnMemoryResolvedCriterions
        {
            get
            {
                // return true;
                if (!_ParticipateInOnmemoryResolvedCriterions.HasValue)
                {
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (DataNode subDataNode in DataNode.SubDataNodes)
                        {
                            if (subDataNode.Type == DataNode.DataNodeType.Object &&
                                subDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.ObjectsContextIdentity) && (subDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ParticipateInOnMemoryResolvedCriterions)
                                _ParticipateInOnmemoryResolvedCriterions = true;
                        }
                    }
                    if ((GlobalResolveCriterions == null || GlobalResolveCriterions.Count == 0) &&
                        (LocalOnMemoryResolvedCriterions == null || LocalOnMemoryResolvedCriterions.Count == 0))
                    {
                        _ParticipateInOnmemoryResolvedCriterions = false;
                        foreach (var criterion in DataNode.BranchSearchCriterions)
                        {
                            if (criterion.OverridenComparisonOperator != null || !CriterionCanBeResolvedFromNativeSystem(criterion))
                            {
                                _ParticipateInOnmemoryResolvedCriterions = true;
                                break;
                            }
                            DataNode searchConditionDataNode = GetSearchConditionDataNode(DataNode.HeaderDataNode, criterion.SearhConditionHeader);

                            if (searchConditionDataNode == null)
                            {
                                var clo = DataNode.HeaderDataNode.BranchSearchConditions;
                                continue;
                            }

                            var dataNodesRoute = new System.Collections.Generic.Stack<DataNode>();
                            if (criterion.LeftTermDataNode != null)
                            {
                                searchConditionDataNode.BuildRoute(criterion.LeftTermDataNode,  dataNodesRoute);
                                if (!ExistOnlyLocalRoute(GetDataLoader(searchConditionDataNode) as StorageDataLoader, CloneRoute(dataNodesRoute)))
                                {
                                    _ParticipateInOnmemoryResolvedCriterions = true;
                                    break;
                                }
                            }
                            if (criterion.RightTermDataNode != null)
                            {
                                searchConditionDataNode.BuildRoute(criterion.RightTermDataNode, dataNodesRoute);
                                if (!ExistOnlyLocalRoute(GetDataLoader(searchConditionDataNode) as StorageDataLoader, CloneRoute(dataNodesRoute)))
                                {
                                    _ParticipateInOnmemoryResolvedCriterions = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        _ParticipateInOnmemoryResolvedCriterions = true;
                }
                return _ParticipateInOnmemoryResolvedCriterions.Value;

            }
        }

        private DataNode GetSearchConditionDataNode(DataNode rootDataNode, SearchCondition searchCondition)
        {
            if (rootDataNode.SearchConditions.Contains(searchCondition) ||
                (rootDataNode.Type == DataNode.DataNodeType.Group && (rootDataNode as GroupDataNode).GroupingSourceSearchCondition == searchCondition))
                return rootDataNode;
            else
            {
                foreach (var dataNode in rootDataNode.SubDataNodes)
                {
                    if (dataNode.SearchConditions.Contains(searchCondition))
                        return dataNode;
                }
                foreach (var dataNode in rootDataNode.SubDataNodes)
                {
                    DataNode searchConditionDataNode = GetSearchConditionDataNode(dataNode, searchCondition);
                    if (searchConditionDataNode != null)
                        return searchConditionDataNode;
                }
                return null;
            }
        }
        /// <summary>
        /// Data node participate in aggregate expression that cannot be resolved localy
        /// </summary>
        /// <MetaDataID>{b510e98f-f461-49d2-8553-e592d032d128}</MetaDataID>
        public override bool ParticipateInGlobalResolvedAggregateExpression
        {
            get
            {
                foreach (DataNode dataNode in DataNode.SubDataNodes)
                {
                    if (dataNode is AggregateExpressionDataNode)
                    {
                        bool localResolve = CheckAggregateFunctionForLocalResolve(dataNode as AggregateExpressionDataNode);
                        if (!localResolve)
                            return true;
                    }
                }

                foreach (DataNode dataNodeWithAggregationMembers in GetDataNodesWithAggregationSubDataNodes(DataNode.HeaderDataNode))
                {
                    foreach (var aggregateExpressionDataNode in (from aggregateExpressionDataNode in dataNodeWithAggregationMembers.SubDataNodes.OfType<AggregateExpressionDataNode>() select aggregateExpressionDataNode).ToList())
                    {
                        if (DataNode.BranchParticipateInMemberAggregateFunctionOn(dataNodeWithAggregationMembers))
                        {

                            bool localResolve = CheckAggregateFunctionForLocalResolve(aggregateExpressionDataNode);
                            if (!localResolve)
                                return true;
                        }
                    }
                }

                return false;
            }
        }

        bool? _ParticipateInGlobalResolvedGroup;
        /// <summary>
        /// Data node participate in grouping expression that cannot be resolved localy
        /// </summary>
        /// <MetaDataID>{e8f59286-7f43-4916-8c54-5cfb04acb0cc}</MetaDataID>
        public override bool ParticipateInGlobalResolvedGroup
        {
            get
            {
                if (!_ParticipateInGlobalResolvedGroup.HasValue)
                {
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        // return true;
                        GroupDataNode groupDataNode = DataNode as GroupDataNode;
                        _ParticipateInGlobalResolvedGroup = false;
                        DataNode groupBySourceDataNodeRoot = groupDataNode.GroupedDataNodeRoot;
                        while (groupBySourceDataNodeRoot.Type == DataNode.DataNodeType.Group)
                            groupBySourceDataNodeRoot = (groupBySourceDataNodeRoot as GroupDataNode).GroupedDataNodeRoot;

                        if (groupBySourceDataNodeRoot != groupDataNode.GroupedDataNode)
                        {
                            System.Collections.Generic.Stack<DataNode> dataNodeRoute = groupBySourceDataNodeRoot.BuildRoute(groupDataNode.GroupedDataNode);
                            // new System.Collections.Generic.Stack<DataNode>();
                            //groupBySourceDataNodeRoot.BuildRoute(groupDataNode.GroupedDataNode, dataNodeRoute);
                            if (!ExistOnlyLocalRoute(GetDataLoader(groupBySourceDataNodeRoot) as StorageDataLoader, CloneRoute(dataNodeRoute)))
                            {
                                _ParticipateInGlobalResolvedGroup = true;
                            }
                        }
                        foreach (DataNode groupKeyDataNode in groupDataNode.GroupKeyDataNodes)
                        {
                            System.Collections.Generic.Stack<DataNode> dataNodeRoute = groupBySourceDataNodeRoot.BuildRoute(groupKeyDataNode);
                            //new System.Collections.Generic.Stack<DataNode>();
                            //groupBySourceDataNodeRoot.BuildRoute(groupKeyDataNode,  dataNodeRoute);
                            if (!ExistOnlyLocalRoute(GetDataLoader(groupBySourceDataNodeRoot) as StorageDataLoader, CloneRoute(dataNodeRoute)))
                            {
                                _ParticipateInGlobalResolvedGroup = true;
                            }
                        }
                    }
                    else
                    {
                        _ParticipateInGlobalResolvedGroup = false;
                        foreach (GroupDataNode groupingDataNode in GetGroupDataNodes(DataNode.HeaderDataNode))
                        {
                            if (DataNode.BranchParticipateAsGroopedDataNodeOn(groupingDataNode) ||
                                DataNode.BranchParticipateInGroopByAsKeyOn(groupingDataNode))
                            {
                                DataLoader dataLoader = GetDataLoader(groupingDataNode);
                                if (dataLoader == null || (dataLoader as StorageDataLoader).ParticipateInGlobalResolvedGroup)
                                {
                                    _ParticipateInGlobalResolvedGroup = true;
                                    break;
                                }
                            }
                            if (DataNode.BranchParticipateInMemberAggregateFunctionOn(groupingDataNode) ||
                              DataNode.BranchParticipateInGroopByAsKeyOn(groupingDataNode))
                            {
                                DataLoader dataLoader = GetDataLoader(groupingDataNode);
                                if (dataLoader == null || (dataLoader as StorageDataLoader).ParticipateInGlobalResolvedAggregateExpression)
                                {
                                    _ParticipateInGlobalResolvedGroup = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                return _ParticipateInGlobalResolvedGroup.Value;
            }
        }

        private List<GroupDataNode> GetGroupDataNodes(DataNode dataNode)
        {
            List<GroupDataNode> groupedDataNodes = new List<GroupDataNode>();
            if (dataNode.Type == DataNode.DataNodeType.Group)
                groupedDataNodes.Add(dataNode as GroupDataNode);
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                groupedDataNodes.AddRange(GetGroupDataNodes(subDataNode));
            return groupedDataNodes;
        }

        private List<DataNode> GetDataNodesWithAggregationSubDataNodes(DataNode dataNode)
        {
            List<DataNode> groupedDataNodes = new List<DataNode>();
            int aggregationSubDataNodesCount = (from aggregationDataNode in dataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                                select aggregationDataNode).ToList().Count;
            if (aggregationSubDataNodesCount > 0)
                groupedDataNodes.Add(dataNode);
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                groupedDataNodes.AddRange(GetDataNodesWithAggregationSubDataNodes(subDataNode));
            return groupedDataNodes;
        }

        /// <MetaDataID>{0f78f9f2-0297-4808-8cfe-428db382722f}</MetaDataID>
        /// <summary>
        /// If HasRows is false the data table is empty
        /// </summary>
        public bool HasRows
        {
            get
            {
                return Data.Rows.Count > 0;
            }
        }


        ///<summary>
        ///This method lookups for local, local memory and global resolved criterions and loads the corresponding collections 
        ///</summary>
        ///<MetaDataID>{9ab87e3f-8562-4ba3-b15a-9554c3b4e7df}</MetaDataID>
        private void FindLocalAndGlobalResolvedCiterions()
        {
            
            if (_LocalResolvedCriterions != null)
                return;
            _LocalResolvedCriterions = new System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]>();
            _LocalOnMemoryResolvedCriterions = new System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]>();

            //if (DataNode.SearchCondition == null)
            //{
            //    _GlobalResolveCriterions = new System.Collections.Generic.List<Criterion>();
            //    return;
            //}
            _GlobalResolveCriterions = new System.Collections.Generic.List<Criterion>();
            if (SearchCondition != null)
            {
                foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in SearchCondition.Criterions)
                {
                    if (!_GlobalResolveCriterions.Contains(criterion))
                        _GlobalResolveCriterions.Add(criterion);
                }
            }



            if (DataNode.Type == DataNode.DataNodeType.Group && (DataNode as GroupDataNode).GroupingSourceSearchCondition != null)
            {
                foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in (DataNode as GroupDataNode).GroupingSourceSearchCondition.Criterions)
                {
                    if (!_GlobalResolveCriterions.Contains(criterion))
                        _GlobalResolveCriterions.Add(criterion);
                }
            }
            foreach (AggregateExpressionDataNode aggregateExpressionDataNode in DataNode.SubDataNodes.OfType<AggregateExpressionDataNode>())
            {
                if(aggregateExpressionDataNode.SourceSearchCondition!=null)
                    foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in aggregateExpressionDataNode.SourceSearchCondition.Criterions)
                    {
                        if (!_GlobalResolveCriterions.Contains(criterion))
                            _GlobalResolveCriterions.Add(criterion);
                    }
            }

            foreach (var searchCondition in DataNode.SearchConditions)
            {
                if (searchCondition != null && SearchCondition != searchCondition)
                {
                    foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in searchCondition.Criterions)
                    {
                        if (!_GlobalResolveCriterions.Contains(criterion))
                            _GlobalResolveCriterions.Add(criterion);
                    }
                }
            }
            //foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in DataNode.BranchSearchCriterions)
            //{
            //    if (!_GlobalResolveCriterions.Contains(criterion))
            //        _GlobalResolveCriterions.Add(criterion);
            //}

            #region Retrieve all local criterions
            foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in new System.Collections.Generic.List<Criterion>(_GlobalResolveCriterions))
            {

                System.Collections.Generic.Stack<DataNode> firstDataNodeRoute = null;
                System.Collections.Generic.Stack<DataNode> secondDataNodeRoute = null;

                bool GlobalResolvedCriterion = false;
                if (criterion.LeftTermDataNode != null)
                {
                    firstDataNodeRoute = new System.Collections.Generic.Stack<DataNode>();
                    DataNode.BuildRoute(criterion.LeftTermDataNode, criterion.LeftTermDataNodeRoute, new System.Collections.Generic.List<DataNode>(), firstDataNodeRoute);
                    if (!ExistOnlyLocalRoute(this, CloneRoute(firstDataNodeRoute)))
                    {
                        firstDataNodeRoute = null;
                        GlobalResolvedCriterion = true;
                    }
                }
                if (DataNode.FilterNotActAsLoadConstraint)
                {
                    DataNode.FilterNotActAsLoadConstraint = false;

                    try
                    {
                        if (DataNode.SearchCondition != null && DataNode.SearchCondition.Criterions.Contains(criterion))
                            GlobalResolvedCriterion = true;
                    }
                    finally
                    {
                        DataNode.FilterNotActAsLoadConstraint = true;
                    }
                }


                if (!GlobalResolvedCriterion && criterion.RightTermDataNode != null)
                {
                    secondDataNodeRoute = new System.Collections.Generic.Stack<DataNode>();
                    DataNode.BuildRoute(criterion.RightTermDataNode, criterion.RightTermDataNodeRoute, new System.Collections.Generic.List<DataNode>(), secondDataNodeRoute);
                    if (!ExistOnlyLocalRoute(this, CloneRoute(secondDataNodeRoute)))
                    {
                        secondDataNodeRoute = null;
                        GlobalResolvedCriterion = true;
                    }
                }
                if (!GlobalResolvedCriterion)
                {
                    if (!_LocalResolvedCriterions.ContainsKey(criterion))
                    {
                        if (firstDataNodeRoute != null && firstDataNodeRoute.Count == 0)
                            firstDataNodeRoute = null;

                        if (secondDataNodeRoute != null && secondDataNodeRoute.Count == 0)
                            secondDataNodeRoute = null;
                        if (criterion.OverridenComparisonOperator != null || !CriterionCanBeResolvedFromNativeSystem(criterion))
                        {
                            if (!_LocalOnMemoryResolvedCriterions.ContainsKey(criterion))
                            {
                                if (firstDataNodeRoute != null && secondDataNodeRoute != null)
                                    _LocalOnMemoryResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[2] { firstDataNodeRoute, secondDataNodeRoute });
                                else if (firstDataNodeRoute != null && secondDataNodeRoute == null)
                                    _LocalOnMemoryResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[1] { firstDataNodeRoute });
                                else if (firstDataNodeRoute == null && secondDataNodeRoute != null)
                                    _LocalOnMemoryResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[1] { secondDataNodeRoute });
                                else
                                    _LocalOnMemoryResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[0] { });
                            }
                        }
                        else
                        {
                            if (firstDataNodeRoute != null && secondDataNodeRoute != null)
                                _LocalResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[2] { firstDataNodeRoute, secondDataNodeRoute });
                            else if (firstDataNodeRoute != null && secondDataNodeRoute == null)
                                _LocalResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[1] { firstDataNodeRoute });
                            else if (firstDataNodeRoute == null && secondDataNodeRoute != null)
                                _LocalResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[1] { secondDataNodeRoute });
                            else
                                _LocalResolvedCriterions.Add(criterion, new System.Collections.Generic.Stack<DataNode>[0] { });
                        }
                    }
                    _GlobalResolveCriterions.Remove(criterion);
                }
            }
            #endregion

            #region Remove all criterions wich have "OR" relation
        ReCalculateLocalOnMemoryResolvedCriterions:
            foreach (Criterion localOnMemoryResolvedCriterion in _LocalOnMemoryResolvedCriterions.Keys)
            {
                foreach (var entry in new System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]>(_LocalResolvedCriterions))
                {
                    if (entry.Key.SearhConditionHeader == localOnMemoryResolvedCriterion.SearhConditionHeader)
                    {
                        if (!entry.Key.SearhConditionHeader.HasAndRelation(entry.Key, localOnMemoryResolvedCriterion))
                        {
                            _LocalResolvedCriterions.Remove(entry.Key);
                            _LocalOnMemoryResolvedCriterions.Add(entry.Key, entry.Value);
                            goto ReCalculateLocalOnMemoryResolvedCriterions;
                        }
                    }
                    //foreach (var searchCondition in DataNode.SearchConditions)
                    //{
                    //    if (searchCondition != null && searchCondition.ContainsCriterion(entry.Key) && searchCondition.ContainsCriterion(localOnMemoryResolvedCriterion))
                    //    {

                    //        if (!searchCondition.HasAndRelation(entry.Key, localOnMemoryResolvedCriterion))
                    //        {
                    //            _LocalResolvedCriterions.Remove(entry.Key);
                    //            _LocalOnMemoryResolvedCriterions.Add(entry.Key, entry.Value);
                    //            goto ReCalculateLocalOnMemoryResolvedCriterions;
                    //        }
                    //    }
                    //}
                }
            }


        ReCalculateGlobalCriterions:
            foreach (Criterion globalCriterion in _GlobalResolveCriterions)
            {
                foreach (var entry in new System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]>(_LocalResolvedCriterions))
                {
                    if (entry.Key.SearhConditionHeader == globalCriterion.SearhConditionHeader)
                    {
                        if (!entry.Key.SearhConditionHeader.HasAndRelation(entry.Key, globalCriterion))
                        {
                            _LocalResolvedCriterions.Remove(entry.Key);
                            _GlobalResolveCriterions.Add(entry.Key);
                            goto ReCalculateGlobalCriterions;
                        }
                    }
                    //foreach (var searchCondition in DataNode.SearchConditions)
                    //{
                    //    if (searchCondition != null && searchCondition.ContainsCriterion(entry.Key) && searchCondition.ContainsCriterion(globalCriterion))
                    //    {
                    //        if (!searchCondition.HasAndRelation(entry.Key, globalCriterion))
                    //        {
                    //            _LocalResolvedCriterions.Remove(entry.Key);
                    //            _GlobalResolveCriterions.Add(entry.Key);
                    //            goto ReCalculateGlobalCriterions;
                    //        }
                    //    }
                    //}
                }
                foreach (var entry in new System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]>(_LocalOnMemoryResolvedCriterions))
                {
                    if (entry.Key.SearhConditionHeader == globalCriterion.SearhConditionHeader)
                    {
                        if (!entry.Key.SearhConditionHeader.HasAndRelation(entry.Key, globalCriterion))
                        {
                            _LocalOnMemoryResolvedCriterions.Remove(entry.Key);
                            _GlobalResolveCriterions.Add(entry.Key);
                            goto ReCalculateGlobalCriterions;
                        }
                    }
                    //foreach (var searchCondition in DataNode.SearchConditions)
                    //{
                    //    if (searchCondition != null && searchCondition.ContainsCriterion(entry.Key) && searchCondition.ContainsCriterion(globalCriterion))
                    //    {
                    //        if (!searchCondition.HasAndRelation(entry.Key, globalCriterion))
                    //        {
                    //            _LocalOnMemoryResolvedCriterions.Remove(entry.Key);
                    //            _GlobalResolveCriterions.Add(entry.Key);
                    //            goto ReCalculateGlobalCriterions;
                    //        }
                    //    }
                    //}
                }
            }


            #endregion


            foreach (var gloabalCriterion in new List<Criterion>(GlobalResolveCriterions))
            {
                bool exist=false;
                foreach (var criterion in DataNode.BranchSearchCriterions)
                {
                    if (criterion == gloabalCriterion)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                    GlobalResolveCriterions.Remove(gloabalCriterion);
            }

        }
        /// <summary>
        /// Checks the criterion if it can be resolved by the native system
        /// </summary>
        /// <param name="criterion">
        /// Defines the checked criterion 
        /// </param>
        /// <returns>
        /// If it can be resolved from native system retruns true else return false
        /// </returns>
        /// <MetaDataID>{1f456668-87a6-4070-8a7a-09bcc2a132a9}</MetaDataID>
        abstract public bool CriterionCanBeResolvedFromNativeSystem(Criterion criterion);

        /// <MetaDataID>{eadcfe6e-6afa-42c8-973d-41b65c52ec88}</MetaDataID>
        public System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> LocalResolvedCriterionsRoutes
        {
            get
            {
                System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> localResolvedCriterionsRoutes = new System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>>();
                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in LocalResolvedCriterions)
                {
                    foreach (System.Collections.Generic.Stack<DataNode> route in entry.Value)
                    {
                        bool alreadyExist = false;
                        foreach (System.Collections.Generic.Stack<DataNode> localResolvedCriterionsRoute in localResolvedCriterionsRoutes)
                        {
                            if (localResolvedCriterionsRoute.ToArray()[localResolvedCriterionsRoute.Count - 1] == route.ToArray()[route.Count - 1])
                            {
                                alreadyExist = true;
                                break;
                            }
                        }
                        if (!alreadyExist)
                            localResolvedCriterionsRoutes.Add(route);
                    }
                }
                return localResolvedCriterionsRoutes;
            }
        }


        /// <MetaDataID>{4cdacdce-dba7-4f62-b1e2-bbdeebc78237}</MetaDataID>
        public System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> LocalResolvedGroupByRoutes
        {
            get
            {
                //System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> localResolvedGroupByRoutes = new System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>>();
                //if (DataNode.Type != DataNode.DataNodeType.Group)
                //    return localResolvedGroupByRoutes;


                System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> localResolvedGroupByRoutes = new System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>>();
                if (DataNode.Type != DataNode.DataNodeType.Group)
                    return localResolvedGroupByRoutes;
                DataNode headDataNode = (DataNode as GroupDataNode).GroupedDataNodeRoot;
                //while (headDataNode.Type == DataNode.DataNodeType.Group)
                //    headDataNode = headDataNode.GroupByDataNodeRoot;


                foreach (DataNode groupByKeyDataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    System.Collections.Generic.Stack<DataNode> dataNodeRoute = headDataNode.BuildRoute(groupByKeyDataNode);
                    // new System.Collections.Generic.Stack<DataNode>();
                    //headDataNode.BuildRoute(groupByKeyDataNode,  dataNodeRoute);
                    if (!ExistOnlyLocalRoute(this, CloneRoute(dataNodeRoute)))
                    {
                        localResolvedGroupByRoutes.Clear();
                        return localResolvedGroupByRoutes;
                    }
                    else
                        localResolvedGroupByRoutes.Add(dataNodeRoute);
                }
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode is AggregateExpressionDataNode)
                    {
                        foreach (DataNode aggregateDataNode in (subDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                        {
                            if (aggregateDataNode is AggregateExpressionDataNode)
                                continue;
                            System.Collections.Generic.Stack<DataNode> dataNodeRoute = (DataNode as GroupDataNode).GroupedDataNodeRoot.BuildRoute(aggregateDataNode);
                            //new System.Collections.Generic.Stack<DataNode>();
                            //(DataNode as GroupDataNode).GroupedDataNodeRoot.BuildRoute(aggregateDataNode,  dataNodeRoute);
                            if (!ExistOnlyLocalRoute(this, CloneRoute(dataNodeRoute)))
                            {
                                localResolvedGroupByRoutes.Clear();
                                return localResolvedGroupByRoutes;
                            }
                            else
                                localResolvedGroupByRoutes.Add(dataNodeRoute);
                        }
                    }
                }
                return localResolvedGroupByRoutes;
            }
        }


        /// <MetaDataID>{027dbfb9-c34a-45ec-95bb-07f89655e92b}</MetaDataID>
        public override bool AggregateExpressionDataNodeResolved(System.Guid aggregateExpressionDataNodeIdentity)
        {
            AggregateExpressionDataNode aggregateExpressionDataNode = DataNode.HeaderDataNode.GetDataNode(aggregateExpressionDataNodeIdentity) as AggregateExpressionDataNode;
            return ResolvedAggregateExpressions.Contains(aggregateExpressionDataNode);
        }

        internal override void AggregateExpressionDataNodeResolved(AggregateExpressionDataNode aggregateDataNode)
        {
            if (DataNode.SubDataNodes.Contains(aggregateDataNode) && !_ResolvedAggregateExpressions.Contains(aggregateDataNode))
                _ResolvedAggregateExpressions.Add(aggregateDataNode);
        }



        ///// <MetaDataID>{64f018de-7a84-4c32-becb-3a715ca6c045}</MetaDataID>
        ///// <summary>
        ///// Checks the type aggregate function datanode for local resolve
        ///// </summary>
        ///// <param name="aggregateFunctionDataNode"> Defines the aggregate data node which will be checked</param>
        ///// <returns>
        ///// If the aggregate function can be resolved locally, return true otherwise false
        ///// </returns>
        //abstract protected bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode);
        ///<summary>
        ///Check if the aggregation function can be resolved local in storage of data loader.
        ///</summary>
        ///<returns>
        ///Returns true if the aggregation function can be resolved locally
        ///</returns>
        /// <MetaDataID>{65b38cef-d685-4fae-ba17-cc616cf8b4da}</MetaDataID>
        internal protected bool CheckAggregateFunctionForLocalResolve(AggregateExpressionDataNode aggregationFunctionDataNode)
        {

            //if (!CanAggregateFanctionsResolvedLocally(aggregationFunctionDataNode))
            //{
            //    aggregationFunctionDataNode.ParticipateInAggregateFanction = true;
            //    return false;
            //}

            StorageDataLoader aggregationFunctionDataLoader= GetDataLoader( aggregationFunctionDataNode.ParentDataNode) as StorageDataLoader;

            foreach (DataNode aggregateExpressionDataNode in aggregationFunctionDataNode.AggregateExpressionDataNodes)
            {
                if (!aggregationFunctionDataLoader.ExistOnlyLocalRoute(aggregateExpressionDataNode))
                    return false;
            }
            return true;

            foreach (DataNode aggregateExpressionDataNode in aggregationFunctionDataNode.AggregateExpressionDataNodes)
            {
                if (aggregateExpressionDataNode is MetaDataRepository.ObjectQueryLanguage.AggregateExpressionDataNode)
                    continue;
                DataNode dataNode = aggregateExpressionDataNode;
                System.Collections.Generic.Stack<DataNode> dataNodesPath = new System.Collections.Generic.Stack<DataNode>();
                dataNodesPath.Push(dataNode);
                if (!(aggregationFunctionDataNode.ParentDataNode is GroupDataNode) || dataNode != (aggregationFunctionDataNode.ParentDataNode as GroupDataNode).GroupedDataNodeRoot)
                {
                    while ((!(aggregationFunctionDataNode.ParentDataNode is GroupDataNode) || dataNode.ParentDataNode != (aggregationFunctionDataNode.ParentDataNode as GroupDataNode).GroupedDataNodeRoot) &&
                        dataNode.ParentDataNode != null)
                    {
                        dataNode = dataNode.ParentDataNode;
                        dataNodesPath.Push(dataNode);
                    }
                }
                
                while (dataNodesPath.Count > 0)
                {
                    dataNode = dataNodesPath.Pop();

                    if (dataNode == DataNode)
                    {
                        foreach (DataLoaderMetadata dataLoaderMetadata in (dataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                        {
                            if (dataLoaderMetadata.HasOutStorageRelationsWithParent)
                                return false;
                        }
                    }
                    else
                    {
                        foreach (DataLoaderMetadata dataLoaderMetadata in (dataNode.ParentDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                        {
                            if (dataLoaderMetadata.SubDataNodeOutStorageRelations.ContainsKey(dataNode.AssignedMetaObjectIdenty) && dataLoaderMetadata.SubDataNodeOutStorageRelations[dataNode.AssignedMetaObjectIdenty])
                                return false;
                        }
                    }
                }

                //  aggregateExpressionDataNode.AggregateFanctionResultsCalculatedLocally = true;


            }
            return true;

        }

        /// <MetaDataID>{8a2a2d1b-a3e9-40b5-9814-876428242c92}</MetaDataID>
        bool ExistOnlyLocalRoute(StorageDataLoader dataLoader, System.Collections.Generic.Stack<DataNode> route)
        {
            //return false;
            if (route.Count == 0)
                return true;

            if (route.Peek() == dataLoader.DataNode)
                route.Pop();
            if (route.Count == 0)
                return true;

            if (route.Peek() == dataLoader.DataNode.ParentDataNode && dataLoader.DataLoaderMetadata.HasOutStorageRelationsWithParent)
                return false;

            List<DataNode> subDataNodes = new List<DataNode>(dataLoader.DataNode.SubDataNodes);
            //if (dataLoader.DataNode is GroupDataNode)
            //    subDataNodes.AddRange((dataLoader.DataNode as GroupDataNode).GroupKeyDataNodes);
            if (dataLoader.DataNode is GroupDataNode && (dataLoader.DataNode as GroupDataNode).GroupKeyDataNodes.Contains(route.Peek()))
            {
                StorageDataLoader subDataNodeDataLoader = null;
                if (route.Peek().DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.ObjectsContextIdentity))
                    subDataNodeDataLoader = route.Peek().DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader;
                if (subDataNodeDataLoader == null)
                    return false;

                return ExistOnlyLocalRoute(subDataNodeDataLoader, route);


            }
            foreach (DataNode subDataNode in subDataNodes)
            {


                if (subDataNode == route.Peek())
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Key && !ParticipateInGlobalResolvedGroup)
                    {
                        route.Pop();
                        return ExistOnlyLocalRoute(dataLoader, route);
                    }
                    else if (subDataNode.Type == DataNode.DataNodeType.Key && ParticipateInGlobalResolvedGroup)
                        return false;



                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                   dataLoader.HasOutStorageRelationOnCurrentTrasaction(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, subDataNode))
                        return false;

                    if ((route.Count == 1 && subDataNode.Type != DataNode.DataNodeType.Object) || subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        return true;

                    if (subDataNode.Type != DataNode.DataNodeType.Group && dataLoader.DataLoaderMetadata.SubDataNodeOutStorageRelations.ContainsKey(subDataNode.AssignedMetaObject.Identity) && dataLoader.DataLoaderMetadata.SubDataNodeOutStorageRelations[subDataNode.AssignedMetaObject.Identity])
                        return false;
                    if (GetDataLoader(subDataNode) is StorageDataLoader && subDataNode.Type != DataNode.DataNodeType.Group && (GetDataLoader(subDataNode) as StorageDataLoader).DataLoaderMetadata.HasOutStorageRelationsWithParent)
                    {
                        //return false;
                    }

                    StorageDataLoader subDataNodeDataLoader = null;
                    if (subDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.ObjectsContextIdentity))
                        subDataNodeDataLoader = subDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader;
                    if (subDataNodeDataLoader == null)
                        return false;

                    return ExistOnlyLocalRoute(subDataNodeDataLoader, route);

                }
            }
            if (route.Peek() == dataLoader.DataNode.ParentDataNode && !dataLoader.DataLoaderMetadata.HasOutStorageRelationsWithParent)
            {
                StorageDataLoader parentDataNodeDataLoader = null;
                if (dataLoader.DataNode.ParentDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.ObjectsContextIdentity))
                    parentDataNodeDataLoader = dataLoader.DataNode.ParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader;
                if (parentDataNodeDataLoader == null)
                    throw new System.Exception("System in InConsistent state");

                return ExistOnlyLocalRoute(parentDataNodeDataLoader, route);



            }
            return true;




        }

        /// <MetaDataID>{99450c2a-85c0-473a-8ed7-1b438d62389d}</MetaDataID>
        protected abstract bool HasOutStorageRelationOnCurrentTrasaction(AssociationEnd associationEnd, MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode);
        /// <MetaDataID>{682b2f3f-ddd6-48a3-a6bb-e8bb721b4031}</MetaDataID>
        internal bool ExistOnlyLocalRoute(DataNode dataNode)
        {
            if (dataNode == DataNode)
                return true;
            System.Collections.Generic.Stack<DataNode> route = new System.Collections.Generic.Stack<DataNode>();
            bool ret = DataNode.BuildRoute(dataNode,  route);
            return ExistOnlyLocalRoute(this, route);

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.PersistenceLayer.ObjectStorage _ObjectStorage;
        /// <MetaDataID>{1c56ab75-f6f3-4eec-b6fa-f6332ccb523d}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage
        {
            get
            {
                if (_ObjectStorage == null)
                    _ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(DataLoaderMetadata.StorageName, DataLoaderMetadata.StorageLocation, DataLoaderMetadata.StorageType);
                return _ObjectStorage;
            }
        }






        //[Association("LoadFromStorageCell", typeof(StorageCell), Roles.RoleA, "{DB851BD9-740B-4B76-963C-25B3B7D78530}")]
        //[RoleBMultiplicityRange(0)]
        //[RoleAMultiplicityRange(1)]
        //public Collections.Generic.Set<StorageCell> StorageCells;

        /// <MetaDataID>{25087e64-8435-4be3-b0ee-8c9ce3314016}</MetaDataID>
        public StorageDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode)
        {
            DataLoaderMetadata = dataLoaderMetadata;
            if ((dataNode.DataSource as StorageDataSource).DataLoadersMetadata == null)
                (dataNode.DataSource as StorageDataSource).DataLoadersMetadata = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata>();
            (dataNode.DataSource as StorageDataSource).DataLoadersMetadata[DataLoaderMetadata.ObjectsContextIdentity] = DataLoaderMetadata;
        }

        /// <MetaDataID>{336dae07-eefd-4e4f-8a29-b0c2edbb2359}</MetaDataID>
        public readonly DataLoaderMetadata DataLoaderMetadata;
        /// <MetaDataID>{fe133e22-5788-4346-9951-14288ff73c26}</MetaDataID>
        public Storage Storage
        {
            get
            {
                if (ObjectStorage != null)
                    return ObjectStorage.StorageMetaData as MetaDataRepository.Storage;
                else
                    return null;
            }

        }



        ///<summary>
        ///Returns the object for object identity
        ///</summary>
        ///<param name="objectIdentity">
        ///Defines the identity of object
        ///</param>
        ///<returns>
        ///The object for object identity
        ///</returns>
        /// <MetaDataID>{92f522f9-353c-482d-9e97-785b105c5680}</MetaDataID>
        protected abstract object GetObjectFromIdentity(PersistenceLayer.ObjectID objectIdentity);



        /// <MetaDataID>{bb40585b-5c36-4b1e-babd-037202472cf3}</MetaDataID>
        internal protected override bool CriterionsForDataLoaderResolvedLocally(DataLoader dataLoader)
        {
            foreach (var criterion in GlobalResolveCriterions)
            {
                if (criterion.LeftTermDataNode == dataLoader.DataNode)
                    return false;

                if (criterion.RightTermDataNode == dataLoader.DataNode)
                    return false;
            }
            return true;
        }
    }
}
