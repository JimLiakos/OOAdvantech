using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.Linq.QueryExpressions;
using System.Reflection;
namespace OOAdvantech.Linq
{
    /// <MetaDataID>{d5cc7c39-7241-4af8-ab52-48155a5a3509}</MetaDataID>
    public class ExpressionTreeNode
    {
        /// <MetaDataID>{515b1c7a-e07f-4f18-b78e-15bde6ff8737}</MetaDataID>
        public string TypeDescription
        {
            get
            {
                return GetTypeDescription(Expression.Type);
            }
        }

        /// <MetaDataID>{4f88bebe-56bd-434c-8b50-9be0e80e8065}</MetaDataID>
        public string GetTypeDescription(Type type)
        {
            if (type.Name.IndexOf("<>f__AnonymousType") != -1)
            //if (ExpressionTranslator.GetDynamicTypeDataRetriever(OOAdvantech.TypeHelper.GetElementType(type)) != null)
            {
                string properties = null;
                foreach (var property in OOAdvantech.TypeHelper.GetElementType(type).GetMetaData().GetProperties())
                {
                    if (properties == null)
                        properties += "(";
                    else
                        properties += ",";
                    properties += property.Name;

                }
                properties += ")";
                return OOAdvantech.TypeHelper.GetElementType(type).Name + properties;

            }

            string typeDes = type.Name;
            if (type.GetMetaData().IsGenericType)
            {
                typeDes += "<";
                int i = 0;
                foreach (var gType in type.GetMetaData().GetGenericArguments())
                {
                    if (i != 0)
                        typeDes += ",";
                    typeDes += GetTypeDescription(gType);
                    i++;
                }

                typeDes += ">";
            }
            return typeDes;
        }
        //public virtual SearchCondition AncestorsFilterDataCondition
        //{
        //    get
        //    {
        //        var parentExpression = Parent;
        //        SearchCondition searchCondition = null;
        //        while (parentExpression != null && parentExpression.Name != "Root")
        //        {
        //            searchCondition = SearchCondition.JoinSearchConditions(searchCondition, parentExpression._FilterDataCondition);
        //            if (parentExpression is QueryExpressions.GroupByExpressionTreeNode)
        //                break;
        //            parentExpression = parentExpression.Parent;

        //        }
        //        return searchCondition;
        //    }

        //}

        /// <MetaDataID>{84a79e12-2add-4bd0-9aba-e3edb17645a9}</MetaDataID>
        protected SearchCondition _FilterDataCondition;
        ///<summary>
        ///Define a conditon that must meet the data of this expression
        ///</summary>
        /// <MetaDataID>{177da888-676b-4de4-8941-875c1a0b72c3}</MetaDataID>
        public virtual SearchCondition FilterDataCondition
        {
            get { return _FilterDataCondition; }
        }


        ///<summary>
        ///This method examines if the treeNode belong to the ancestors
        ///</summary>
        ///<param name="treeNode">
        ///Defines the assessement expression
        ///</param>
        ///<returns>
        ///If the assessement expression is ancestor the result is true 
        ///</returns>
        /// <MetaDataID>{135b098d-e197-4b3a-a431-30e7eea52912}</MetaDataID>
        public bool IsExpressionTreeNodeAncestor(ExpressionTreeNode treeNode)
        {
            if (treeNode == Parent)
                return true;
            if (Parent == null)
                return false;
            return Parent.IsExpressionTreeNodeAncestor(treeNode);
        }
        /// <MetaDataID>{48284b5e-a349-4b21-84bb-8b3c8b06e8e5}</MetaDataID>
        public static ExpressionTreeNode _lastExpressionTree;
        /// <MetaDataID>{5960fd38-251e-4bc0-879f-143bb8f4ab0d}</MetaDataID>
        public static ExpressionTreeNode lastExpressionTree
        {
            get
            {
                return _lastExpressionTree;
            }
            set
            {
                _lastExpressionTree = value;
            }
        }

        /// <exclude>Excluded</exclude>
        internal string _Alias = "";
        ///<summary>
        ///Defines the alias of expression
        ///</summary>
        /// <MetaDataID>{712cf797-9b38-4b3f-9145-ed20f71f7ddd}</MetaDataID>
        internal string Alias
        {
            get
            {
                return _Alias;
            }
            set
            {
                _Alias = value;
            }
        }

        /// <exclude>Excluded</exclude>
        protected string _Name;
        /// <MetaDataID>{ff84235a-d294-4854-adcc-215ebfb67d01}</MetaDataID>
        public virtual string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        /// <MetaDataID>{125a085e-2b3d-462a-8be7-d9f726540314}</MetaDataID>
        public string NamePrefix;
        /// <MetaDataID>{33421675-64ed-4a35-84e6-6206c5195e06}</MetaDataID>
        public string OriginDiscription;
        /// <MetaDataID>{c7eb6e4c-b416-4b4e-9ef8-194a63c7dbf8}</MetaDataID>
        public System.Linq.Expressions.Expression Expression;
        /// <exclude>Excluded</exclude>
        internal protected DataNode _DataNode;
        ///<summary>
        ///Defines the data node that used from expression to retrieve the data 
        ///</summary>
        /// <MetaDataID>{8a771ab2-b994-4435-b1dc-6775315b5756}</MetaDataID>
        internal virtual DataNode DataNode
        {
            get
            {
                return _DataNode;
            }
            set
            {
                _DataNode = value;
            }
        }
        /// <MetaDataID>{d269072b-d79c-4dbd-95d2-253324435f12}</MetaDataID>
        ExpressionTreeNode _Parent;

        ///<summary> 
        ///Defines the parent expression treenode
        ///</summary>
        /// <MetaDataID>{e52fb0d9-7990-46a5-a293-4543eb84b5a5}</MetaDataID>
        public ExpressionTreeNode Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
                if (_Parent != null)
                {
                    if (_Parent is ExpressionTreeNode && _Parent.Expression != null)
                    {
                        if (Name.IndexOf(Parent.OriginDiscription + ".") == 0)
                        {
                            Name = Name.Substring(_Parent.OriginDiscription.Length + 1);
                            if (Name.IndexOf("Item().") == 0)
                                Name = Name.Substring("Item().".Length);
                        }
                    }
                    if (_Parent is ExpressionTreeNode && _Parent.Expression != null)
                    {
                        if (Name.IndexOf(Parent.OriginDiscription + ".") == 0)
                            Name = Name.Substring(_Parent.OriginDiscription.Length + 1);
                        if (_Parent.Expression.NodeType == ExpressionType.New)
                        {
                            int i = 0;
                            foreach (ExpressionTreeNode childNode in _Parent.Nodes)
                            {
                                if (childNode == this)
                                    break;
                                i++;
                            }
                            Alias = _Parent.Expression.Type.GetMetaData().GetProperties()[i].Name;
                        }
                    }
                }
            }
        }

        ///<summary>
        ///</summary>
        /// <MetaDataID>{4f92c860-c258-4535-a0ec-4d94449e01f7}</MetaDataID>
        public System.Collections.Generic.List<ExpressionTreeNode> Nodes = new List<ExpressionTreeNode>();
        /// <MetaDataID>{aecba751-e4d0-46f4-8e88-95210138c3df}</MetaDataID>
        public override string ToString()
        {
            if (Expression == null)
                return Name;

            //   string Alias = "";


            string shr = null;

            if (Expression is MethodCallExpression)
            {
                //shr + "(" + GetHashCode().ToString() + ")" + NamePrefix + " : " +
                //shr + "(" + GetHashCode().ToString() + ")" + NamePrefix + " : " +
                return "[" + Expression.NodeType.ToString() + " " + (Expression as MethodCallExpression).Method.Name + " '" + Alias + "' ]  " + TypeDescription;// Name;
            }
            else
            {
                return "[" + Expression.NodeType.ToString() + " '" + Alias + "' ]  " + TypeDescription;// Name;
            }
        }
        /// <MetaDataID>{1c4a8a09-0b9b-4efc-8f5c-2f8ea2384157}</MetaDataID>
        internal ExpressionTreeNode(string name, Translators.ExpressionVisitor expressionTranslator)
        {
            Name = name;
            ExpressionTranslator = expressionTranslator;

        }

        /// <MetaDataID>{3340fbfd-49a2-48d2-9725-f415338ec146}</MetaDataID>
        internal Translators.ExpressionVisitor ExpressionTranslator;


        /// <MetaDataID>{e873dd06-d8b5-4c2c-bf6a-c7495f5918d4}</MetaDataID>
        internal ExpressionTreeNode(System.Linq.Expressions.Expression exp, ExpressionTreeNode parent, Translators.ExpressionVisitor expressionTranslator)
        {

            ExpressionTranslator = expressionTranslator;
            ExpressionTranslator.DataNodeTreeSimplification += new OOAdvantech.Linq.Translators.DataNodeTreesSimplificationHandler(OnDataNodeTreesSimplification);


            Expression = exp;
            ExpressionTranslator.ExpressionTreeNodes[Expression] = this;
            OriginDiscription = exp.ToString();
            Name = OriginDiscription;
            if (parent != null)
            {
                parent.Nodes.Add(this);
                _Parent = parent;

                if (_Parent is ExpressionTreeNode && Parent.Expression != null)
                {
                    if (Name.IndexOf(Parent.OriginDiscription + ".") == 0)
                    {
                        Name = Name.Substring(_Parent.OriginDiscription.Length + 1);
                        if (Name.IndexOf("Item().") == 0)
                            Name = Name.Substring("Item().".Length);
                    }
                }
                if (_Parent is ExpressionTreeNode && _Parent.Expression != null)
                {
                    if (Name.IndexOf(Parent.OriginDiscription + ".") == 0)
                        Name = Name.Substring(Parent.OriginDiscription.Length + 1);
                    if (_Parent.Expression.NodeType == ExpressionType.New)
                    {



                        int i = 0;
                        foreach (ExpressionTreeNode childNode in _Parent.Nodes)
                        {
                            if (childNode == this)
                                break;
                            i++;
                        }
                        // Alias = _Parent.Expression.Type.GetMetaData().GetProperties()[i].Name;
                    }
                }
            }
            //Text = ToString();
        }

        /// <MetaDataID>{3038243e-4890-4da7-81e5-eaba0d972baf}</MetaDataID>
        protected virtual void OnDataNodeTreesSimplification(Dictionary<DataNode, DataNode> replacedDataNodes)
        {
            if (DataNode == null)
                return;
            DataNode actualDataNode = Translators.ExpressionVisitor.GetActualDataNode(DataNode, replacedDataNodes);
            if (actualDataNode != null && actualDataNode != _DataNode)
            {
                if (actualDataNode == DataNode.ParentDataNode)
                {
                    if (DataNode.ParentDataNode != null && DataNode.ParentDataNode.IsSameOrParentDataNode(actualDataNode))
                    {
                        foreach (DataNode subDataNode in new System.Collections.Generic.List<DataNode>(DataNode.SubDataNodes))
                        {
                            string alias = actualDataNode.Alias;
                            foreach (string newAlias in DataNode.Aliases)
                                actualDataNode.Alias = newAlias;
                            actualDataNode.Alias = alias;

                            subDataNode.ParentDataNode = actualDataNode;
                            DataNode = actualDataNode;
                        }
                    }

                }
                else
                {
                    string alias = actualDataNode.Alias;
                    foreach (string newAlias in DataNode.Aliases)
                        actualDataNode.Alias = newAlias;
                    actualDataNode.Alias = alias;
                    if (DataNode.ParentDataNode != null)
                        actualDataNode.ParentDataNode = DataNode.ParentDataNode;
                    DataNode = actualDataNode;
                }
            }

        }

        /// <summary>
        /// Build necessary DataNodes for linq expression.
        /// </summary>
        /// <MetaDataID>{bd2e7a69-8f0d-47fa-941e-dee3ba1429fa}</MetaDataID>
        internal virtual DataNode BuildDataNodeTree(DataNode dataNode, ILINQObjectQuery linqObjectQuery)
        {
            return dataNode;
        }



        /// <MetaDataID>{1eeef512-8a27-42ea-86d5-8d59d91eb24c}</MetaDataID>
        public virtual SearchCondition GetPropertySearchCondition(System.Reflection.PropertyInfo property)
        {
            if (property == null)
                return null;
            var dynamicTypeDataRetriever = ExpressionTranslator.GetDynamicTypeDataRetriever(property.DeclaringType);
            if (this is QueryExpressions.NewExpressionTreeNode && (this as QueryExpressions.NewExpressionTreeNode)._DynamicTypeDataRetrieve != null)
                dynamicTypeDataRetriever = (this as QueryExpressions.NewExpressionTreeNode)._DynamicTypeDataRetrieve;

            if (dynamicTypeDataRetriever == null)
                return FilterDataCondition;
            else
            {

                return dynamicTypeDataRetriever.Properties[property].FilterDataCondition;
            }
        }



        /// <MetaDataID>{f8557bc0-5383-4e72-b087-4571e0b91c35}</MetaDataID>
        public Type Type
        {
            get
            {
                return Expression.Type;
            }
        }

        /// <MetaDataID>{cab97558-435d-4a16-9229-0a8aaefb5d33}</MetaDataID>
        internal virtual IDynamicTypeDataRetrieve DynamicTypeDataRetrieve
        {
            get
            {
                return null;
            }
        }

        public DataOrderBy OrderByFilter
        {
            get
            {
                if (DynamicTypeDataRetrieve != null)
                    return DynamicTypeDataRetrieve.OrderByFilter;
                else
                    return null;
            }
        }

        ///// <summary>
        ///// This method return the DynamicTypeDataRetrieve for exression
        ///// </summary>
        ///// <param name="expression">
        ///// Defines the expression where the method uses to find the DynamicTypeDataRetrieve which correspond to expression type.
        ///// </param>
        ///// <returns>
        ///// Return expression type correspond DynamicTypeDataRetrieve
        ///// </returns>
        //internal static IDynamicTypeDataRetrieve GetDynamicTypeDataRetriever(ExpressionTreeNode expression)
        //{
        //    if (expression is MethodCallAsCollectionProviderExpressionTreeNode)
        //        return (expression as MethodCallAsCollectionProviderExpressionTreeNode).BridgeEnumerator;
        //    if (expression is ParameterExpressionTreeNode)
        //    {
        //        ExpressionTreeNode parameterSource = expression.ExpressionTranslator.ParameterDeclareExpression[expression.Expression as ParameterExpression];
        //        IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = null;
        //        if ((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollectionParameter == expression.Expression)
        //            dynamicTypeDataRetrieve = GetDynamicTypeDataRetriever((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection);
        //        else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorSourceParameter == expression.Expression)
        //            dynamicTypeDataRetrieve = GetDynamicTypeDataRetriever((parameterSource as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection);
        //        else if (parameterSource is SelectManyExpressionTreeNode && (parameterSource as SelectManyExpressionTreeNode).SelectorCollectionParameter == expression.Expression)
        //            dynamicTypeDataRetrieve = GetDynamicTypeDataRetriever((parameterSource as SelectManyExpressionTreeNode).DerivedCollection);


        //        //var dynamicTypeRetrive = (ExpressionTranslator.ParameterDeclareExpression[SourceCollection.Expression as ParameterExpression] as MethodCallAsCollectionSourceExpressionTreeNode).BridgeEnumerator;
        //        if (dynamicTypeDataRetrieve != null)
        //        {
        //            if (expression.Nodes.Count == 0)
        //                return dynamicTypeDataRetrieve;
        //            else
        //                return GetDynamicTypeDataRetriever(dynamicTypeDataRetrieve, expression.Nodes[0] as MemberAccessExpressionTreeNode);
        //        }
        //        return null;
        //    }
        //    if (expression is ConstantExpressionTreeNode)
        //        return null;

        //    throw new System.Exception("Unknown Node Type");
        //}



    }
}
