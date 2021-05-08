using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
	/// <MetaDataID>{96984B3C-6EFF-441C-BF78-B258398EC5EE}</MetaDataID>
	/// <summary>Define ComparisonTerm specialization for Object </summary>
    [Serializable]
    public class ObjectComparisonTerm : ComparisonTerm
	{
        ObjectComparisonTerm():base(null)
        {

        }
        internal override ComparisonTerm Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
            ObjectComparisonTerm newObjectComparisonTerm = new ObjectComparisonTerm();
            object dataNode =null;
            if (clonedObjects.TryGetValue(DataNode, out dataNode))
                newObjectComparisonTerm.DataNode = dataNode as DataNode;
            else
                newObjectComparisonTerm.DataNode = DataNode.Clone(clonedObjects);
            return newObjectComparisonTerm;
        }
        public override string ToString()
        {
            return DataNode.FullName;
        }
		/// <MetaDataID>{84C4282E-AA22-4F0A-82F0-E312A879B4EE}</MetaDataID>
		public DataNode DataNode;
		/// <MetaDataID>{77CAB1D9-4E10-40BF-8E7E-0D8D0EACDC48}</MetaDataID>
		public override System.Type ValueType
		{
			get
			{

                return DataNode.Classifier.GetExtensionMetaObject<System.Type>();
			}
		}

		/// <MetaDataID>{7735C2F9-F4BD-40D8-AEF6-EE0E89B8275F}</MetaDataID>
		public override string GetCompareExpression(Criterion.ComparisonType comparisonType, ComparisonTerm theOtherComparisonTerm)
		{
			return null;
		}
		/// <MetaDataID>{C1280473-B5C5-432E-B1AC-83918D129A3F}</MetaDataID>
		internal protected ObjectComparisonTerm(Parser.ParserNode ComparisonTermParserNode, ObjectsContextQuery oqlStatement):base(ComparisonTermParserNode,oqlStatement)
		{
            DataNode = oqlStatement.PathDataNodeMap[ComparisonTermParserNode["Path"]] as DataNode;
            //if (Route == null)
            //    Route = new System.Collections.Generic.Stack<DataNode>();

		}

        /// <MetaDataID>{9483be68-0beb-49fd-aa48-c84e370f7a64}</MetaDataID>
        public ObjectComparisonTerm(DataNode dataNode, ObjectQuery oqlStatement)
            : base(oqlStatement)
        {
            dataNode.ParticipateInWereClause = true;
            DataNode = dataNode;
            //if (Route == null)
            //    Route = new System.Collections.Generic.Stack<DataNode>();

        }


        //public ObjectComparisonTerm(DataNode dataNode, System.Collections.Generic.Stack<DataNode> route,ObjectQuery oqlStatement)
        //    : base(oqlStatement)
        //{
        //    Route = route;
        //    dataNode.ParticipateInWereClause = true;
        //    DataNode = dataNode;
        //}
        //readonly System.Collections.Generic.Stack<DataNode> Route;





        /// <MetaDataID>{BBC2C294-A722-46B1-A7B9-23054B4E5050}</MetaDataID>
        internal StorageCell GetStorageCellFromObjectID(Parser.ParserNode parserNode)
        {
            throw new System.Exception("Can't retrieve Storage Cell");
/*			if(objectIDParserNode==null)
				throw new System.Exception("Can't retrieve Storage Cell for null objectID");
			int Count=objectIDParserNode.ChildNodes.GetFirst().ChildNodes.Count;
			for(int i=0;i!=Count;i++)
			{
				Parser.ParserNode ObjectIDField=objectIDParserNode.ChildNodes.GetFirst().ChildNodes.GetAt(i+1);
				string  ObjectIDFieldName=ObjectIDField.ChildNodes.GetAt(1).Value;
				if(ObjectIDFieldName=="StorageCellID")
				{
					string ObjCellID=ObjectIDField.ChildNodes.GetAt(2).ChildNodes.GetFirst().ChildNodes.GetFirst().Value;
					return (DataNode.Classifier).GetStorageCell(int.Parse(ObjCellID));
				}
			}*/
			return null;

        }

        /// <MetaDataID>{73C829E3-C9F7-4023-A857-3D7B4FB0A982}</MetaDataID>
        internal StorageCell GetStorageCellFromParameterValue(object parameterValue)
        {
            throw new Exception("The method or operation is not implemented.");
            //StorageInstanceRef storageInstanceRef = null;
            //if (parameterValue != null)
            //{
            //    if (!ModulePublisher.ClassRepository.GetType((DataNode.Classifier as MetaDataRepository.MetaObject).FullName, "").IsInstanceOfType(parameterValue))
            //        throw new System.Exception("Type mismatch at " + ComparisonTermParserNode.ParentNode.Value);
            //    try
            //    {
            //        storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(parameterValue) as StorageInstanceRef;
            //        if (storageInstanceRef == null)
            //            return null;
            //        else
            //            return storageInstanceRef.StorageInstanceSet;

            //    }
            //    catch
            //    {
            //    }
            //}
            //return null;
        }
    }
}
