namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{e05cbdff-08de-4387-8067-38d8cd698642}</MetaDataID>
    public class QueryOnRootObject : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery
    {
        public QueryOnRootObject(object rootObject, OOAdvantech.Collections.Generic.List<string> paths)
        {
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
                    dataNodes[dataNode.HeaderDataNode.Name] = dataNode;
            }

            OOAdvantech.MetaDataRepository.Classifier rootDataNodeClassifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(rootObject.GetType()) as OOAdvantech.MetaDataRepository.Classifier;
            if (rootDataNodeClassifier == null)
                rootDataNodeClassifier = OOAdvantech.DotNetMetaDataRepository.Type.CreateClassifierObject(rootObject.GetType()) as OOAdvantech.MetaDataRepository.Classifier;
            string error = null;
            dataNodes["Root"].BuildDataNodeTree(rootDataNodeClassifier, ref error);
            dataNodes["Root"].BuildDataSource(null);


        }

        public override DataSource CreateDataSourceFor(DataNode dataNode)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override DataSource CreateRelationObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override DataSource CreateRelatedObjectDataSource(DataNode dataNode, DataNode referenceDataNode, AssociationEnd associationEnd)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override bool IsRemovedRow(System.Data.DataRow row)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        /// <MetaDataID>{0eb5d1cb-fa3a-41b5-ae2a-53da3998999b}</MetaDataID>
        DataNode CreateDataNodeFor(Path path, DataNode ParentDataNode)
        {
            DataNode dataNode = new DataNode(this, path);

            dataNode.ParentDataNode = ParentDataNode;
            if (path.SubPath != null)
                dataNode = CreateDataNodeFor(path.SubPath, dataNode);
            return dataNode;
        }
		
    }
}
