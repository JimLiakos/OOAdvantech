using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq.QueryExpressions
{
    /// <MetaDataID>{de4674fb-122f-40df-9dec-99c3f73dc132}</MetaDataID>
    class NewExpressionTreeNode : ExpressionTreeNode
    {

        /// <MetaDataID>{f78ef0d1-ccd6-45d4-ad55-1d3039d77bb7}</MetaDataID>
        public override SearchCondition FilterDataCondition
        {
            get
            {
               return Parent.FilterDataCondition;
            }
        }
        /// <MetaDataID>{6d576f0d-7867-44dc-9b51-3402060625ce}</MetaDataID>
        internal protected IDynamicTypeDataRetrieve BridgeEnumerator;
        /// <MetaDataID>{65c88cab-568b-423c-b205-47823c89acf6}</MetaDataID>
        public NewExpressionTreeNode(Expression exp, ExpressionTreeNode parent,Translators.ExpressionVisitor expressionTranslator)
            : base(exp, parent, expressionTranslator)
        {
            if (this.Expression.NodeType != ExpressionType.New)
                throw new System.Exception("Wrong expression type");
        }


        //public override SearchCondition GetPropertySearchCondition(System.Reflection.PropertyInfo property)
        //{
        //    if (DynamicTypeDataRetrieve!= null)
        //    {
        //        DynamicTypeProperty dynamicProperty = null;
        //        if (DynamicTypeDataRetrieve.Properties.TryGetValue(property, out dynamicProperty))
        //            return dynamicProperty.SearchCondition;
        //    }

        //    return base.GetPropertySearchCondition(property);
        //}


        /// <MetaDataID>{1bf9d4a5-f770-4895-b99e-729a7a919a91}</MetaDataID>
        internal IDynamicTypeDataRetrieve _DynamicTypeDataRetrieve;

        internal override IDynamicTypeDataRetrieve DynamicTypeDataRetrieve
        {
            get
            {
                return _DynamicTypeDataRetrieve;
            }
        }

        /// <MetaDataID>{f4ea4896-867a-4e10-8413-0a0d8ddd9ecf}</MetaDataID>
        public MemberInitExpression MemberInitExpression;
        /// <MetaDataID>{820ffa32-6491-4b61-a56a-6f3ed90db8ae}</MetaDataID>
        internal override DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {

            if (/*!(linqObjectQuery is LinqQueryOnRootObject) ||*/Parent==null ||Parent.Name!="Root")
                return base.BuildDataNodeTree(dataNode, linqObjectQuery);
            if (dataNode == null)
            {
                dataNode = new DataNode(linqObjectQuery.ObjectQuery);
                dataNode.Name = "Root";
                if (Parent.Expression is System.Linq.Expressions.LambdaExpression)
                {
                    dataNode.AssignedMetaObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier((Parent.Expression as System.Linq.Expressions.LambdaExpression).Parameters[0].Type);
                    //dataNode.Name = (Parent.Expression as System.Linq.Expressions.LambdaExpression).Parameters[0].Name;
                }
                else
                    dataNode.Temporary = true;
                _DataNode = dataNode;
                //(linqObjectQuery as ILINQObjectQuery).EnumerableType = (Expression as NewExpression).Type;
            }
            //int i = 0;
            //foreach (ExpressionTreeNode expressionTreeNode in Nodes)
            //{
            //    DataNode memberDataNode = expressionTreeNode.BuildDataNodeTree(dataNode, linqObjectQuery);
            //    memberDataNode.Alias = (Expression as NewExpression).Type.GetProperties()[i].Name;
            //    i++;
            //}

            Dictionary<object, DynamicTypeProperty> dynamicTypeProperties = new Dictionary<object, DynamicTypeProperty>();
            LoadPropertiesMetaData(dynamicTypeProperties);

            IDynamicTypeDataRetrieve bridgeEnumerator = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(NewExpressionType, linqObjectQuery, DataNode, dynamicTypeProperties, this);
            ExpressionTranslator.AddDynamicTypeDataRetriever(NewExpressionType, bridgeEnumerator);
            if (Parent.Name == "Root")
                (linqObjectQuery as ILINQObjectQuery).QueryResult = bridgeEnumerator;

            return dataNode;


            if (Nodes.Count > 0)
            {

                string name = Nodes[0].Name;
                bool exist = false;
                foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (subDataNode.Name == name)
                    {
                        exist = true;
                        dataNode = subDataNode;
                        break;
                    }
                }
                if (!exist)
                {
                    //Type type = TypeHelper.GetElementType(((Nodes[0] as MyTreeNode).Expression as MemberExpression).Type);
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(linqObjectQuery.ObjectQuery);
                    subDataNode.Name = name;
                    subDataNode.ParentDataNode = dataNode;
                    dataNode = subDataNode;
                }

                dataNode = Nodes[0].BuildDataNodeTree(dataNode, linqObjectQuery);
            }
            return dataNode;
        }


        /// <MetaDataID>{471220d0-bbae-4e60-a122-9548a3db48c2}</MetaDataID>
        Type NewExpressionType
        {
            get
            {
                return (Expression as NewExpression).Type;
            }
        }

        /// <MetaDataID>{72b2183e-d964-48dc-ae7f-12218eee8b25}</MetaDataID>
        private void LoadPropertiesMetaData(Dictionary<object, DynamicTypeProperty> dynamicTypeProperties)
        {
            int i = 0;
            DataNode dataNode = null;
            foreach (ExpressionTreeNode propertyTreeNode in Nodes)
            {
                dataNode = DataNode;
                dataNode = propertyTreeNode.BuildDataNodeTree(dataNode, ExpressionTranslator.LINQObjectQuery as ILINQObjectQuery);
                //dataNode.Alias = treeNode.Alias;
                //if (treeNode is SelectExpressionTreeNode)
                //{
                //    if ((treeNode as SelectExpressionTreeNode).SelectCollection.Expression.NodeType == ExpressionType.New)
                //    {
                //        dataNode.AddBranchAlias(treeNode.Alias);
                //    }
                //    else
                //        (treeNode as SelectExpressionTreeNode).SelectCollection.DataNode.Alias = treeNode.Alias;
                //}

                
                    System.Reflection.PropertyInfo property = NewExpressionType.GetMetaData().GetProperty(propertyTreeNode.Alias);

                    if (propertyTreeNode is MethodCallAsCollectionProviderExpressionTreeNode)
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (propertyTreeNode as MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                        
                    }
                    else if (propertyTreeNode is QueryExpressions.ConstantExpressionTreeNode)
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, (propertyTreeNode as QueryExpressions.ConstantExpressionTreeNode).DataNode, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    }
                    else if (/*treeNode.Alias == "Key" &&*/ dataNode.Type == DataNode.DataNodeType.Key && dataNode.ParentDataNode.Type == DataNode.DataNodeType.Group)
                    {
                        IDynamicTypeDataRetrieve propertyType = null;
                        if (propertyTreeNode is ParameterExpressionTreeNode)
                        {
                            var selectionExpression = Parent;
                            while (selectionExpression is NewExpressionTreeNode)
                                selectionExpression = selectionExpression.Parent;
                            propertyType = propertyTreeNode.DynamicTypeDataRetrieve;


                            //var source = (treeNode as ParameterExpressionTreeNode).SourceCollection;
                            //if (source is MethodCallAsCollectionSourceExpressionTreeNode)
                            //    propertyType = (source as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
                            //if (source is GroupKeyExpressionTreeNode)
                            //    propertyType = (source as GroupKeyExpressionTreeNode).BridgeEnumerator;

                        }
                        if (propertyType == null)
                            propertyType = ExpressionTranslator.GetDynamicTypeDataRetriever(property.PropertyType);
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, propertyType, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);

                    }
                    else if (propertyTreeNode is QueryExpressions.ParameterExpressionTreeNode)
                    {
                        var selectionExpression = Parent;
                        while (selectionExpression is NewExpressionTreeNode)
                            selectionExpression = selectionExpression.Parent;
                        IDynamicTypeDataRetrieve propertyType = propertyTreeNode.DynamicTypeDataRetrieve;
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, propertyType, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    }
                    //else if (property.PropertyType.Name.IndexOf("<>f") == 0)
                    //{

                    //    MethodCallAsCollectionSourceExpressionTreeNode propertySource = (treeNode as QueryExpressions.ParameterExpressionTreeNode).SourceCollection as MethodCallAsCollectionSourceExpressionTreeNode;
                    //    //ExpressionTreeNode propertySource = ExpressionTranslator.GetSourceCollection(SourceCollection, treeNode.Alias);
                    //    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, (propertySource as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator, treeNode);
                    //    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    //}
                    //else if (TypeHelper.FindIEnumerable(property.PropertyType) != null /*&& property.PropertyType.Name!=typeof(System.Linq.IGrouping<,>).Name */&& treeNode is ParameterExpressionTreeNode)
                    //{
                    //    ExpressionTreeNode expressionTreeNode = treeNode;
                    //    ExpressionTreeNode propertySource = Nodes[0];
                    //    if (treeNode is ParameterExpressionTreeNode)
                    //    {
                    //        propertySource = (treeNode as ParameterExpressionTreeNode).SourceCollection;
                    //    }
                    //    else
                    //    {
                    //        while (expressionTreeNode != null)
                    //        {

                    //            propertySource = ExpressionTranslator.GetSourceCollection(propertySource, expressionTreeNode.Name);
                    //            if (expressionTreeNode.Nodes.Count > 0)
                    //                expressionTreeNode = expressionTreeNode.Nodes[0];
                    //            else
                    //                expressionTreeNode = null;
                    //        }
                    //    }
                    //    IDynamicTypeDataRetrieve propertyType = null;
                    //    if (propertySource != null && propertySource is MethodCallAsCollectionSourceExpressionTreeNode)
                    //        propertyType = (propertySource as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
                    //    DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, propertyType, treeNode);
                    //    dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    //    //ExpressionTranslator.AddAliasEnumerator(treeNode.Alias, dynamicTypeProperty.PropertyType);


                    //}
                    else
                    {
                        DynamicTypeProperty dynamicTypeProperty = new DynamicTypeProperty(property, dataNode, null, propertyTreeNode);
                        dynamicTypeProperties.Add(property, dynamicTypeProperty);
                    }


                
                i++;
            }
        }

    }
}
