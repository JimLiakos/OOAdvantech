using System.Collections.Generic;
using System;
using OOAdvantech.Remoting;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{e05cbdff-08de-4387-8067-38d8cd698642}</MetaDataID>
    public class QueryOnRootObject : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery
    {

        /// <MetaDataID>{7BD46C75-2F12-4D0D-8A8B-C1E3A0413859}</MetaDataID>
        public void BuildDataNodeTree(ref string errorOutput)
        {
            OOAdvantech.Collections.Generic.List<DataNode> objectTypeDataNodes = new OOAdvantech.Collections.Generic.List<DataNode>();
            foreach (DataNode CurrDataNode in DataTrees)
                GetObjectTypeDataNodes(CurrDataNode, objectTypeDataNodes);



            foreach (DataNode dataNode in objectTypeDataNodes)
            {
                if (dataNode.AssignedMetaObject == null)
                {
                    dataNode.MergeIdenticalDataNodes();
                    dataNode.AssignDataNodeToParserPaths(PathDataNodeMap);
                    System.Type type = GetType(dataNode.FullName, "");
                    MetaDataRepository.Classifier classifier = null;
                    if (type != null)
                        classifier = MetaDataRepository.Classifier.GetClassifier(type);

                    if (classifier == null)
                    {
                        errorOutput += "There isn't type '" + dataNode.FullName + "'";
                        return;
                    }

                    dataNode.AssignedMetaObject = classifier;
                }
                dataNode.Validate(ref errorOutput);
                dataNode.MergeIdenticalDataNodes();
                if (errorOutput != null && errorOutput.Length > 0)
                    return;
            }
            OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
            foreach (DataNode dataNode in DataTrees)
                dataTrees.AddRange(dataNode.RemoveNamespacesDataNodes());
            DataTrees = dataTrees;



        }

        void GetObjectTypeDataNodes(DataNode dataNode, System.Collections.Generic.List<DataNode> objectTyeDataNodes)
        {
            if (dataNode.Type == DataNode.DataNodeType.Object)
            {
                objectTyeDataNodes.Add(dataNode);
                return;

            }

            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                GetObjectTypeDataNodes(subDataNode, objectTyeDataNodes);
        }

        internal protected void BuildDataSources()
        {
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataTrees)
                BuildRootObjectDataSource(dataNode);
        }
        private void BuildRootObjectDataSource(DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.Object)
            {
                dataNode.DataSource = new RootObjectDataSource(dataNode);
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    BuildRootObjectDataSource(subDataNode);
            }
        }

        protected internal override void LoadData()
        {
            ObjectQueryDataSet = DataSource.DataObjectsInstantiator.CreateDataSet();
            DataTrees[0].GetData(ObjectQueryDataSet);

            //FilterData();
            
        }

        internal protected void Distribute()
        {

            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(RootObject as MarshalByRefObject))
            {
                #if !DeviceDotNet

                string channeluri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(RootObject as MarshalByRefObject);
                Type[] paramTypes = new Type[]{typeof(OOAdvantech.Collections.Generic.List<DataNode>),
                                            typeof(Collections.Generic.List<DataNode>), 
                                            typeof(OOAdvantech.Collections.Generic.Dictionary<string, object>),
                                            typeof( List<string>)};

                DistributedObjectQuery distributedObjectQuery = Remoting.RemotingServices.CreateRemoteInstance<DistributedObjectQuery>(channeluri,
                                                                                                                paramTypes,
                                                                                                                DataTrees,
                                                                                                                QueryResultType,
                                                                                                                SelectListItems,
                                                                                                                new OOAdvantech.Collections.Generic.Dictionary<string, object>(),
                                                                                                                new List<string>());




                ObjectsDataRetriever objectsDataRetriever = new ObjectsDataRetriever(new OOAdvantech.Collections.Generic.List<ObjectData>() { new ObjectData(RootObject, System.Guid.Empty,new List<MetaObjectID>()) }, DataTrees[0]);

                var outOfContextobjectsDataRetrievers = distributedObjectQuery.BuildDataRetrievingModel(new List<ObjectsDataRetriever>() { objectsDataRetriever });
                
                System.Collections.Generic.Dictionary<Guid, DataSource> dataSources = null;
                (DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).GetDataSources(ref dataSources);
                OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader> dataLoaders = distributedObjectQuery.DataLoaders;
                foreach (System.Collections.Generic.KeyValuePair<Guid, DataSource> dataSourceEntry in dataSources)
                {
                    if (dataLoaders.ContainsKey(dataSourceEntry.Value.Identity) && !dataSourceEntry.Value.DataLoaders.ContainsKey(distributedObjectQuery.Identity))
                        dataSourceEntry.Value.DataLoaders.Add(distributedObjectQuery.Identity, dataLoaders[dataSourceEntry.Value.Identity]);
                }
            #endif

            }
            else
            {
#if !DeviceDotNet
                DataNode headerDataNode = DataTrees[0];
                Collections.Generic.List<DataNode> selectListItems = new Collections.Generic.List<DataNode>();
                DistributedObjectQueryData distributedObjectQueryData = new DistributedObjectQueryData();
                distributedObjectQueryData.HeaderDataNode = headerDataNode;
                distributedObjectQueryData.QueryResultMetaData = QueryResultType;


                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                System.Collections.Generic.Dictionary<string, SearchingData> searchingData = new System.Collections.Generic.Dictionary<string, SearchingData>();
                binaryFormatter.Serialize(memoryStream, distributedObjectQueryData);
                memoryStream.Position = 0;
                distributedObjectQueryData = (DistributedObjectQueryData)binaryFormatter.Deserialize(memoryStream);
                headerDataNode = distributedObjectQueryData.HeaderDataNode;

                //headerDataNode = binaryFormatter.Deserialize(memoryStream) as DataNode;

                foreach (DataNode selecteDataNode in new List<DataNode>(SelectListItems))
                {
                    if (!DataTrees.Contains(selecteDataNode.HeaderDataNode))
                        SelectListItems.Remove(selecteDataNode);
                }

                foreach (DataNode dataNode in SelectListItems)
                    selectListItems.Add(headerDataNode.GetDataNode(dataNode.Identity));

                MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery distributedObjectQuery = new DistributedObjectQuery(QueryIdentity,
                    new OOAdvantech.Collections.Generic.List<DataNode>() { headerDataNode },
                    distributedObjectQueryData.QueryResultMetaData,
                    selectListItems,
                    new OOAdvantech.Collections.Generic.Dictionary<string, object>(),
                    new List<string>());

                ObjectsDataRetriever objectsDataRetriever = new ObjectsDataRetriever(new OOAdvantech.Collections.Generic.List<ObjectData>() { new ObjectData(RootObject, System.Guid.Empty) }, headerDataNode);

                var outOfContextobjectsDataRetrievers = distributedObjectQuery.BuildDataRetrievingModel(new List<ObjectsDataRetriever>() { objectsDataRetriever });

                System.Collections.Generic.Dictionary<Guid, DataSource> dataSources = null;
                (DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).GetDataSources(ref dataSources);
                OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader> dataLoaders = distributedObjectQuery.DataLoaders;
                foreach (System.Collections.Generic.KeyValuePair<Guid, DataSource> dataSourceEntry in dataSources)
                {
                    if (dataLoaders.ContainsKey(dataSourceEntry.Value.Identity) && !dataSourceEntry.Value.DataLoaders.ContainsKey(distributedObjectQuery.Identity))
                        dataSourceEntry.Value.DataLoaders.Add(distributedObjectQuery.Identity, dataLoaders[dataSourceEntry.Value.Identity]);
                }
#endif
            }


            //

        }



        /// <MetaDataID>{96fc6493-31c6-4fd9-8721-cfd29be9b763}</MetaDataID>
        public readonly object RootObject;
        /// <MetaDataID>{c08a46ce-e935-499f-8d1b-8b40856da217}</MetaDataID>
        public readonly System.Type CollectionObjectType;
        /// <MetaDataID>{bb3ffab8-c934-4907-b2fc-2b16a1721d8e}</MetaDataID>
        public readonly System.Collections.Generic.List<object> ObjectCollection;

        //protected internal override Namespace GetNamespace(string _namespace)
        //{
        //    return null;
            
        //}

        /// <MetaDataID>{2e9abb11-558b-4872-ad62-76f5f14c2605}</MetaDataID>
        protected void BuildDataSourceFor(DataNode dataNode, DataNode referenceDataNode)
        {
            if (dataNode.DataSource != null)
                return; // the data source already builded
            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)//.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.SubDataNodes.Count==0)
            {
                if (dataNode.HasTimePeriodConstrain)
                    throw new System.Exception("You can,t apply 'TIMEPERIOD' keyword on Class primitive member");
                return;
            }
            if (dataNode.ObjectIDConstrainStorageCell != null)
            {
                dataNode.DataSource = CreateDataSourceFor(dataNode);
            }
            else
            {
                if (referenceDataNode == null)
                {
                    if (dataNode.Classifier != null)
                        dataNode.DataSource = CreateDataSourceFor(dataNode);
                }
                else
                {
                    if (dataNode.Classifier == null)
                        return;
                    Association association = null;
                    AssociationEnd associationEnd = null;
                    if (referenceDataNode == dataNode.ParentDataNode && dataNode.AssignedMetaObject is AssociationEnd)
                    {
                        associationEnd = ((AssociationEnd)dataNode.AssignedMetaObject);
                        association = associationEnd.Association;
                    }
                    else if (referenceDataNode.AssignedMetaObject is AssociationEnd)
                    {
                        associationEnd = (AssociationEnd)referenceDataNode.AssignedMetaObject;
                        associationEnd = associationEnd.GetOtherEnd();
                        association = associationEnd.Association;
                    }
                    if (associationEnd != null)
                    {
                        if (dataNode.Name == association.Name && dataNode.Name != associationEnd.Name)
                            dataNode.DataSource = CreateRelationObjectDataSource(dataNode, referenceDataNode, associationEnd);
                        else
                            dataNode.DataSource = CreateRelatedObjectDataSource(dataNode, referenceDataNode, associationEnd);
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        dataNode.DataSource = CreateDataSourceFor(dataNode);
                    }
                    else
                    {
                        throw new System.Exception("Error on Data tree");
                    }
                }
            }
           // dataNode.CreateOnConstructionSubDataNode();
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                if (subDataNode != referenceDataNode)
                {
                    if (dataNode.Classifier == null)
                        BuildDataSourceFor(subDataNode, null);

                    else
                        BuildDataSourceFor(subDataNode, dataNode);
                }
            }
            if (dataNode.ParentDataNode != null && dataNode.ParentDataNode.Classifier != null)
                BuildDataSourceFor(dataNode.ParentDataNode, dataNode);

        }

        public QueryOnRootObject(object rootObject):base(Guid.NewGuid())
        {
            RootObject = rootObject;
            Parameters = new OOAdvantech.Collections.Generic.Dictionary<string, object>();
        }
        /// <MetaDataID>{0a522929-313e-4083-b590-222cd4dd7f90}</MetaDataID>
        public QueryOnRootObject(object rootObject, System.Collections.Generic.List<string> paths):base(Guid.NewGuid())
        {
            paths.Reverse();
            Parameters = new OOAdvantech.Collections.Generic.Dictionary<string, object>();

            RootObject = rootObject;
            System.Collections.Generic.Dictionary<string, DataNode> dataNodes = new System.Collections.Generic.Dictionary<string, DataNode>();
            foreach (string pathFullName in paths)
            {
                Path path = Path.CreatePath(pathFullName);
                DataNode dataNode = CreateDataNodeFor(path, null);
                if (dataNodes.ContainsKey(dataNode.HeaderDataNode.Name))
                {
                    foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.HeaderDataNode.SubDataNodes))
                        subDataNode.ParentDataNode = dataNodes[dataNode.HeaderDataNode.Name];
                    dataNodes[dataNode.HeaderDataNode.Name].MergeIdenticalDataNodes();
                }
                else
                    dataNodes[dataNode.HeaderDataNode.Name] = dataNode.HeaderDataNode;
            }

            OOAdvantech.MetaDataRepository.Classifier rootDataNodeClassifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(rootObject.GetType()) as OOAdvantech.MetaDataRepository.Classifier;
            string error = null;
            dataNodes["Root"].BuildDataNodeTree(rootDataNodeClassifier, ref error);

            if (!string.IsNullOrEmpty(error))
                RemoveUnknownDataNodes(dataNodes["Root"]);
            BuildDataSourceFor(dataNodes["Root"], null);
            DataTrees.Add(dataNodes["Root"]);


        }

        private void RemoveUnknownDataNodes(DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.Unknown)
                dataNode.ParentDataNode = null;
            else
            {
                foreach (var subDataNode in  dataNode.SubDataNodes.ToArray())
                    RemoveUnknownDataNodes(subDataNode);
            }
        }
        /// <MetaDataID>{a440fa5b-b10c-48a5-b584-0228852af03a}</MetaDataID>
        public QueryOnRootObject(OOAdvantech.Collections.Generic.List<object> objectCollection, System.Type collectionObjectType, System.Collections.Generic.List<string> paths)
            : base(Guid.NewGuid())
        {
            CollectionObjectType = collectionObjectType;
            ObjectCollection = objectCollection;
            System.Collections.Generic.Dictionary<string, DataNode> dataNodes = new System.Collections.Generic.Dictionary<string, DataNode>();
            foreach (string pathFullName in paths)
            {
                Path path = Path.CreatePath(pathFullName);
                DataNode dataNode = CreateDataNodeFor(path, null);
                if (dataNodes.ContainsKey(dataNode.HeaderDataNode.Name))
                {
                    foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(dataNode.HeaderDataNode.SubDataNodes))
                        subDataNode.ParentDataNode = dataNodes[dataNode.HeaderDataNode.Name];
                    dataNodes[dataNode.HeaderDataNode.Name].MergeIdenticalDataNodes();
                }
                else
                    dataNodes[dataNode.HeaderDataNode.Name] = dataNode.HeaderDataNode;
            }

            OOAdvantech.MetaDataRepository.Classifier rootDataNodeClassifier = OOAdvantech.DotNetMetaDataRepository.Type.GetClassifierObject(collectionObjectType) as OOAdvantech.MetaDataRepository.Classifier;
            string error = null;
            dataNodes["Root"].BuildDataNodeTree(rootDataNodeClassifier, ref error);
            BuildDataSourceFor(dataNodes["Root"], null);
            DataTrees.Add(dataNodes["Root"]);


        }


        //        public void LoadData()
        //        {
        //            MainDataReader = new System.Data.DataSet();
        //            (DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).GetData(MainDataReader);
        ////            System.Collections.Generic.LinkedList<DataNode> filteredDataPath = new System.Collections.Generic.LinkedList<DataNode>();
        //            if (SearchCondition != null)
        //                SearchCondition.FilterData();
        //        }


        /// <MetaDataID>{71fa4b7e-4a4c-43ad-aeb4-bc19c747c68c}</MetaDataID>
        public DataSource CreateValueTypeDataSource(DataNode dataNode, DataNode referenceDataNode, Attribute attribute)
        {
            return new RootObjectDataSource(dataNode);
        }

        /// <MetaDataID>{fb3efb73-67ca-4f98-af88-6bfe3c1501ac}</MetaDataID>
        public DataSource CreateDataSourceFor(DataNode dataNode)
        {
            return new RootObjectDataSource(dataNode);
            //throw new System.Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{a338c2dd-5724-4fc9-8164-5b8122dcac05}</MetaDataID>
        public DataSource CreateRelationObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd)
        {
            return new RootObjectDataSource(dataNode);
            throw new System.Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{4e965872-a5ef-4c1c-9457-c3560b7080b9}</MetaDataID>
        public DataSource CreateRelatedObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd)
        {
            return new RootObjectDataSource(dataNode);
            throw new System.Exception("The method or operation is not implemented.");
        }

        ///// <MetaDataID>{cb13c7ea-8447-4f61-a1ca-d637af06e0c3}</MetaDataID>
        //public override bool IsRemovedRow(System.Data.DataRow row)
        //{
        //    return false;
        //    //throw new System.Exception("The method or operation is not implemented.");
        //}
        /// <MetaDataID>{0eb5d1cb-fa3a-41b5-ae2a-53da3998999b}</MetaDataID>
        DataNode CreateDataNodeFor(Path path, DataNode ParentDataNode)
        {
            DataNode dataNode = new DataNode(this, path);


            dataNode.ParentDataNode = ParentDataNode;
            dataNode.Alias = dataNode.FullName.Replace(".", "_");
            AddSelectListItem(dataNode);
            if (path.SubPath != null)
            {
                dataNode = CreateDataNodeFor(path.SubPath, dataNode);
                AddSelectListItem(dataNode);
            }

            return dataNode;
        }

    }
}
