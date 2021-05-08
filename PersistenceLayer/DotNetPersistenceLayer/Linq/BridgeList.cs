using System;
using System.Collections.Generic;

using System.Reflection;
using System.Collections;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Linq.QueryExpressions;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Linq;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{85188c0f-d67d-4ba0-8b86-9756a3e742fb}</MetaDataID>
    internal interface IDynamicTypeDataRetrieve
    {


        /// <MetaDataID>{868a2c95-361c-470a-a85d-694f64290a88}</MetaDataID>
        IDynamicTypeDataRetrieve GetNewEnumarator();


        IEnumerator GetEnumarator();
        /// <summary>
        /// This dictionary contains properties Meta data useful for dynamic type data retriever 
        /// to retrieves object properties values.
        ///  </summary>
        /// <MetaDataID>{ca05d85d-3eb3-42f6-b718-0fc8f82fef15}</MetaDataID>
        [Association("", Roles.RoleA, "dee7639c-0997-4cc0-ad5d-ae404af1f2a5")]
        Dictionary<object, DynamicTypeProperty> Properties
        {
            get;
        }

        /// <MetaDataID>{2cc1d523-39f2-4ebb-9ea6-1d2ee5311e6c}</MetaDataID>
        SearchCondition FilterDataCondition
        {
            get;
        }

        DataOrderBy OrderByFilter
        {
            get;
        }
        ///// <MetaDataID>{4d166153-ce0d-4f81-bee3-e7a028e786b9}</MetaDataID>
        //ExpressionTreeNode SourceCollectionExpression
        //{
        //    get;
        //}

        /// <summary>
        /// Defines the method call expression for instance "Select - SelectMany " that defines the dynamicType data retriever as result Type
        /// or  member type of result Type.
        /// </summary>
        /// <MetaDataID>{6df6d65e-c37d-4de0-925a-5ccc93b9633c}</MetaDataID>
        MethodCallAsCollectionProviderExpressionTreeNode CollectionProviderMethodExpression
        {
            get;
        }


        ///<summary>
        ///Define the query which use the dynamic type retrievers to transform data to linq expression results 
        ///</summary>
        /// <MetaDataID>{2ae4c02e-54e7-4432-ad1f-436ecfefcc73}</MetaDataID>
        ILINQObjectQuery ObjectQuery
        {
            get;
        }

        /// <MetaDataID>{895d75ea-8f12-421d-be00-fc4022b05de1}</MetaDataID>
        /// <summary>
        /// 
        /// </summary>
        GroupingMetaData GroupingMetaData
        {
            get;
        }


        /// <MetaDataID>{720a05f6-1274-4464-9e69-a7c76ddc358c}</MetaDataID>
        bool IsGrouping
        {
            get;
        }


        ///<summary>
        ///Metadata which used from dynamic type data retriever to retrieve conventional type object
        ///</summary>
        /// <MetaDataID>{0cdef555-3a67-4698-b0bf-b021924a360a}</MetaDataID>
        DataNode MemberDataNode
        {
            get;
        }



        /// <summary>
        /// This method add all data nodes where used for data retrieve, in object query selection list.
        /// </summary>
        /// <param name="filterCondition">
        /// Defines the filter condition which defined in previous dynamic type retriever. 
        /// </param>
        /// <MetaDataID>{aecb91aa-a7f3-4341-8777-b9c0647928c6}</MetaDataID>
        void ParticipateInQueryResults(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchCondition filterCondition);




        /// <summary>
        /// The Type property defines the type of produced object.
        /// </summary>
        /// <MetaDataID>{08355027-2bc0-4dfe-ae88-81dcd8bfdada}</MetaDataID>
        Type Type
        {
            get;
        }
        ///<summary>
        /// This property defines the data node where system uses as main source to retrieve 
        /// the objects of this type.
        /// Usually root is the data node which produced for the first form expression of a selection query . 
        ///</summary>
        /// <MetaDataID>{b555973e-0505-415f-8971-0bd04ed3e54c}</MetaDataID>
        DataNode RootDataNode
        {
            get;
            set;
        }


        /// <MetaDataID>{378116b0-4974-413b-bb58-acdebb399a7c}</MetaDataID>
        MetaDataRepository.ObjectQueryLanguage.QueryResultType QueryResult
        {
            get;
            set;
        }

        MetaDataRepository.ObjectQueryLanguage.QueryResultDataLoader QueryResultDataLoader { get; }


        DataOrderBy OrderBy { get; set; }




        /// <MetaDataID>{715863b1-842b-4cb3-8d06-8c4905e21497}</MetaDataID>
        object InstantiateObject(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResultLoaderEnumartor queryResultLoaderEnumartor, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows dataNodesRows);
        /// <MetaDataID>{b6536f12-c9e2-492b-8ab4-89a18192419f}</MetaDataID>
        object InstantiateObject(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.CompositeRowData compositeRow, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows dataNodesRows);

        /// <MetaDataID>{4ce767d0-daf1-4552-98dd-d2f578002731}</MetaDataID>
        object GetRelatedDataEnumerator(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResultDataLoader queryResultDataLoader);
    }

    ///<summary>
    ///This class defines all meta data for data grouping
    ///</summary>
    /// <MetaDataID>{9654044c-40d0-44b7-9088-fbadd4bdf024}</MetaDataID>
    class GroupingMetaData
    {



        /// <MetaDataID>{104d6380-e8f8-427f-a45d-c3f3dd989921}</MetaDataID>
        ExpressionTreeNode SourceTreeNode;
        /// <MetaDataID>{23d63a76-bcc6-435a-a0b9-de62bb5f19c9}</MetaDataID>
        ExpressionTreeNode GroupKeyExpression;

        /// <MetaDataID>{7e80acf3-f3fc-466b-9a95-0e248ce02c07}</MetaDataID>
        public GroupingMetaData(IDynamicTypeDataRetrieve groupingMetaDataOwner, ExpressionTreeNode sourceTreeNode, ExpressionTreeNode groupKeyExpression)
        {
            SourceTreeNode = sourceTreeNode;
            GroupKeyExpression = groupKeyExpression;

            GroupingMetaDataOwner = groupingMetaDataOwner;
            GroupingType = groupingMetaDataOwner.Type;
            System.Type keyType = GroupingType.GetMetaData().GetGenericArguments()[0];
            //if (keyType.Name.IndexOf("<>f__AnonymousType") == 0)
            //{
            //    CompositeKey = true;
            //    foreach (System.Reflection.PropertyInfo propertyInfo in keyType.GetProperties())
            //    {
            //        foreach (DataNode groupingDataNode in GroupDataNode.GroupKeyDataNodes)
            //        {
            //            if (groupingDataNode.HasAlias(propertyInfo.Name))
            //            {
            //                KeyProperties[propertyInfo] = new DynamicTypeProperty(propertyInfo, groupingDataNode, null, null);
            //                break;
            //            }
            //        }
            //    }
            //    _KeyConstInfo = keyType.GetConstructors()[0];
            //    _FastKeyConstructorInvoke = AccessorBuilder.CreateConstructorInvoker(KeyConstInfo);
            //    foreach (System.Reflection.ParameterInfo paramInfo in KeyConstInfo.GetParameters())
            //    {
            //        System.Reflection.PropertyInfo property = KeyConstInfo.DeclaringType.GetProperty(paramInfo.Name);
            //        CtorParamsDataRetrieverProperies.Add(KeyProperties[property]);
            //    }
            //}
        }
        /// <MetaDataID>{b7a807b6-9148-45e1-b401-392292dd410a}</MetaDataID>
        GroupingMetaData()
        {
        }

        /// <exclude>Excluded</exclude>
        IDynamicTypeDataRetrieve _KeyDynamicTypeDataRetrieve;
        /// <MetaDataID>{0f642ab4-8d29-4c33-9da7-908d092fc7de}</MetaDataID>
        internal IDynamicTypeDataRetrieve KeyDynamicTypeDataRetrieve
        {
            get
            {
                return _KeyDynamicTypeDataRetrieve;
            }
            set
            {
                _KeyDynamicTypeDataRetrieve = value;
            }
        }


        /// <exclude>Excluded</exclude>
        IDynamicTypeDataRetrieve _GroupCollectionDynamicTypeDataRetrieve;

        /// <MetaDataID>{271b0b86-2332-4b01-b8cc-99148fe7d21c}</MetaDataID>
        internal IDynamicTypeDataRetrieve GroupCollectionDynamicTypeDataRetrieve
        {
            get
            {
                return _GroupCollectionDynamicTypeDataRetrieve;
            }
            set
            {
                _GroupCollectionDynamicTypeDataRetrieve = value;
            }
        }

        /// <MetaDataID>{148ee4e4-de90-4059-83b8-7d4aa03f1072}</MetaDataID>
        IDynamicTypeDataRetrieve GroupingMetaDataOwner;


        /// <MetaDataID>{274b9c14-77b5-4027-a2ca-299ff5e249c6}</MetaDataID>
        internal MetaDataRepository.ObjectQueryLanguage.GroupDataNode GroupDataNode
        {
            get
            {
                return GroupingMetaDataOwner.RootDataNode as GroupDataNode;
            }
        }

        /// <MetaDataID>{9f07da3b-7320-4a3d-a7f8-2b5c9a565a29}</MetaDataID>
        System.Type GroupingType;

        /// <exclude>Excluded</exclude>
        IDynamicTypeDataRetrieve _GroupedDataRetrieve;
        /// <MetaDataID>{3c719dc3-027f-48ec-8198-ad174aab5ac1}</MetaDataID>
        /// <summary>
        /// Define a data retriever which retrevies grouped data in group by expression
        /// </summary>
        public IDynamicTypeDataRetrieve GroupedDataRetrieve
        {
            get
            {
                if (_GroupedDataRetrieve != null)
                    return _GroupedDataRetrieve;


                if (GroupingType.GetMetaData().GetGenericArguments()[1].Name == typeof(System.Linq.IGrouping<,>).Name)
                {
                    System.Diagnostics.Debug.Assert(false, "IDynamicGrouping ");


                    Type[] typeArguments = new Type[2];
                    typeArguments[0] = GroupingType.GetMetaData().GetGenericArguments()[1].GetMetaData().GetGenericArguments()[0];
                    typeArguments[1] = GroupingType.GetMetaData().GetGenericArguments()[1].GetMetaData().GetGenericArguments()[1];
                    Dictionary<System.Reflection.PropertyInfo, DynamicTypeProperty> dynamicTypeProperties = new Dictionary<PropertyInfo, DynamicTypeProperty>();
                    DataNode keyDataNode = null;
                    foreach (DataNode subDataNode in GroupDataNode.GroupedDataNode.SubDataNodes)
                    {
                        if (subDataNode.Type == DataNode.DataNodeType.Key)
                        {
                            keyDataNode = subDataNode;
                            break;
                        }
                    }

                    Type DynamicType = typeof(IDynamicGrouping<,>).MakeGenericType(typeArguments);
                    DynamicTypeProperty keyProperty = new DynamicTypeProperty(DynamicType.GetMetaData().GetProperty("Key"), keyDataNode, GroupingMetaDataOwner.ObjectQuery.Translator.GetDynamicTypeDataRetriever(GroupingType.GetMetaData().GetGenericArguments()[1].GetMetaData().GetGenericArguments()[0]), SourceTreeNode);
                    _GroupedDataRetrieve = GroupingMetaDataOwner.ObjectQuery.Translator.GetDynamicTypeDataRetriever(GroupingType.GetMetaData().GetGenericArguments()[1]);
                    if (_GroupedDataRetrieve == null)
                        _GroupedDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(GroupingType.GetMetaData().GetGenericArguments()[1], GroupingMetaDataOwner.ObjectQuery, GroupDataNode.GroupedDataNode, GroupDataNode.GroupedDataNode, SourceTreeNode);
                    DynamicTypeProperty groupProperty = new DynamicTypeProperty(DynamicType.GetMetaData().GetProperty("GroupedData"), GroupDataNode.GroupedDataNode, _GroupedDataRetrieve, null);
                    dynamicTypeProperties.Add(keyProperty.PropertyInfo, keyProperty);
                    dynamicTypeProperties.Add(groupProperty.PropertyInfo, groupProperty);

                    _GroupedDataRetrieve = Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(typeof(IDynamicGrouping<,>).MakeGenericType(typeArguments)), GroupingMetaDataOwner.ObjectQuery, GroupDataNode.GroupedDataNode, dynamicTypeProperties, SourceTreeNode) as IDynamicTypeDataRetrieve;

                }
                else
                {
                    IDynamicTypeDataRetrieve dynamicTypeDataRetrieve = null;
                    if (SourceTreeNode is QueryExpressions.GroupByExpressionTreeNode)
                    {

                        var sourceCollection = (SourceTreeNode as QueryExpressions.GroupByExpressionTreeNode).GroupingCollection;
                        if (sourceCollection is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                            dynamicTypeDataRetrieve = (sourceCollection as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve;
                        else if (sourceCollection is QueryExpressions.ParameterExpressionTreeNode)
                        {
                            sourceCollection = (sourceCollection as QueryExpressions.ParameterExpressionTreeNode).SourceCollection;
                            if (sourceCollection is QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode)
                                dynamicTypeDataRetrieve = (sourceCollection as QueryExpressions.MethodCallAsCollectionProviderExpressionTreeNode).DynamicTypeDataRetrieve;
                        }

                    }
                    if (dynamicTypeDataRetrieve != null && dynamicTypeDataRetrieve.Type == GroupingType.GetMetaData().GetGenericArguments()[1])
                        _GroupedDataRetrieve = dynamicTypeDataRetrieve;
                    else
                        _GroupedDataRetrieve = GroupingMetaDataOwner.ObjectQuery.Translator.GetDynamicTypeDataRetriever(GroupingType.GetMetaData().GetGenericArguments()[1]);
                    if (_GroupedDataRetrieve == null)
                    {
                        if (SourceTreeNode is OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode)
                            _GroupedDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(GroupingType.GetMetaData().GetGenericArguments()[1], GroupingMetaDataOwner.ObjectQuery, GroupDataNode.GroupedDataNode, GroupDataNode.GroupedDataNode, (SourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).SourceCollection) as IDynamicTypeDataRetrieve;
                        else
                            _GroupedDataRetrieve = DynamicTypeDataRetrieve<object>.CreateDynamicTypeDataRetrieve(GroupingType.GetMetaData().GetGenericArguments()[1], GroupingMetaDataOwner.ObjectQuery, GroupDataNode.GroupedDataNode, GroupDataNode.GroupedDataNode, SourceTreeNode);


                    }
                }
                return _GroupedDataRetrieve;
            }
        }
    }

    /// <summary>
    /// Dynamic type data retriever converts the row data to object. 
    /// The dynamic type data retriever can act as enumerator and 
    /// construct dynamic type object for each row or retrieve a conventional type object 
    /// for each row. 
    /// Also dynamic type data retriever can act as subsidiary of enumerator dynamic type data retriever 
    /// to retrieve a member of main dynamic type object.   
    /// </summary>
    /// <typeparam name="Type">
    /// Type parameter defines the type of produced object. 
    /// </typeparam>
    /// <MetaDataID>{814927bd-42d0-4608-a80a-0480f046ef30}</MetaDataID>
    internal class DynamicTypeDataRetrieve<ObjectType> : IEnumerator<ObjectType>, IDynamicTypeDataRetrieve, IEnumerable<ObjectType>
    {

        /// <MetaDataID>{4dc0a988-6aa2-41db-b600-557e4b405b14}</MetaDataID>
        public MetaDataRepository.ObjectQueryLanguage.QueryResultType QueryResult
        {
            get;
            set;
        }

        DataOrderBy _OrderBy;
        public DataOrderBy OrderBy
        {
            get
            {
                return _OrderBy;
            }
            set
            {
                _OrderBy = value;
            }

        }

        public DataOrderBy OrderByFilter
        {
            get
            {
                if (_OrderBy != null)
                    return _OrderBy;
                if (CollectionProviderMethodExpression != null)
                {
                    if (CollectionProviderMethodExpression.GetType() == typeof(QueryExpressions.SelectExpressionTreeNode) &&
                        (CollectionProviderMethodExpression as QueryExpressions.SelectExpressionTreeNode).SelectCollection.DynamicTypeDataRetrieve != null &&
                        (CollectionProviderMethodExpression as QueryExpressions.SelectExpressionTreeNode).SelectCollection.DynamicTypeDataRetrieve != this)
                    {
                        return (CollectionProviderMethodExpression as QueryExpressions.SelectExpressionTreeNode).SelectCollection.DynamicTypeDataRetrieve.OrderByFilter;
                    }
                    else
                        if (CollectionProviderMethodExpression.SourceCollection != null)
                    {
                        if (CollectionProviderMethodExpression.SourceCollection is ParameterExpressionTreeNode)
                        {
                            var sourceCollection = (CollectionProviderMethodExpression.SourceCollection as ParameterExpressionTreeNode).HeadNodeSourceCollection;
                            if (sourceCollection.DynamicTypeDataRetrieve != null)
                            {
                                if (!sourceCollection.DynamicTypeDataRetrieve.IsGrouping)
                                    return sourceCollection.DynamicTypeDataRetrieve.OrderByFilter;
                                else if (IsGrouping)
                                    return sourceCollection.DynamicTypeDataRetrieve.OrderByFilter;
                            }

                            //if (sourceCollection != null && sourceCollection.DynamicTypeDataRetrieve != null &&
                            //    (!sourceCollection.DynamicTypeDataRetrieve.IsGrouping || (Properties != null && Properties.Values.Where(x => x.Type == sourceCollection.DynamicTypeDataRetrieve.Type).Count() > 0)))
                            //    return sourceCollection.DynamicTypeDataRetrieve.OrderByFilter;


                        }
                        else
                        {
                            var sourceCollection = CollectionProviderMethodExpression.SourceCollection;

                            if (sourceCollection.DynamicTypeDataRetrieve != null)
                            {
                                if (!sourceCollection.DynamicTypeDataRetrieve.IsGrouping)
                                    return sourceCollection.DynamicTypeDataRetrieve.OrderByFilter;
                                else if (IsGrouping)
                                    return sourceCollection.DynamicTypeDataRetrieve.OrderByFilter;
                            }
                        }
                    }
                    return null;
                }
                else

                    return null;

            }
        }

        /// <MetaDataID>{bdc5bb2d-7db7-43eb-8e57-516bbcf38de9}</MetaDataID>
        MetaDataRepository.ObjectQueryLanguage.QueryResultDataLoader _QueryResultDataLoader;
        /// <MetaDataID>{47239ff0-50b7-4b4c-8896-f7b613725619}</MetaDataID>
        public MetaDataRepository.ObjectQueryLanguage.QueryResultDataLoader QueryResultDataLoader
        {
            get
            {
                if (_QueryResultDataLoader != null)
                    return _QueryResultDataLoader;
                else
                    return QueryResult.DataLoader;

            }

        }

        /// <MetaDataID>{0ef2ccc7-646d-4974-aac3-5bd540bbc11c}</MetaDataID>
        public static IDynamicTypeDataRetrieve CreateDynamicTypeDataRetrieve(Type type, ILINQObjectQuery linqObjectQuery,
                                 DataNode rootDataNode,
                                 DataNode memberDataNode,
                                 ExpressionTreeNode typeDefinitionExpresion)
        {
            return Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(type), linqObjectQuery, rootDataNode, memberDataNode, typeDefinitionExpresion) as IDynamicTypeDataRetrieve;
        }


        /// <MetaDataID>{682554e7-21a9-4bae-939a-dca12146835b}</MetaDataID>
        public static IDynamicTypeDataRetrieve CreateDynamicTypeDataRetrieve(Type type,
                                                   ILINQObjectQuery linqObjectQuery,
                                                   DataNode rootDataNode,
                                                   Dictionary<object, DynamicTypeProperty> dynamicTypeProperties,
                                                   ExpressionTreeNode typeDefinitionExpresion)
        {
            return Activator.CreateInstance(typeof(DynamicTypeDataRetrieve<>).MakeGenericType(type), linqObjectQuery, rootDataNode, dynamicTypeProperties, typeDefinitionExpresion) as IDynamicTypeDataRetrieve;
        }
        ///// <MetaDataID>{1ccdde91-6060-4649-9af5-53f0b771f445}</MetaDataID>
        //Member<SearchCondition> SourceTreeNodeSearchCondition = new Member<SearchCondition>();

        /// <MetaDataID>{9924424e-adeb-4e59-9c38-a1b4bf807c23}</MetaDataID>
        public SearchCondition FilterDataCondition
        {
            get
            {
                if (CollectionProviderMethodExpression != null)
                {
                    if (CollectionProviderMethodExpression.GetType() == typeof(QueryExpressions.SelectExpressionTreeNode))
                        return (CollectionProviderMethodExpression as QueryExpressions.SelectExpressionTreeNode).SelectCollection.FilterDataCondition;
                    else
                        return CollectionProviderMethodExpression.SourceCollection.FilterDataCondition;
                }
                else

                    return null;
            }
        }





        /// <exclude>Excluded</exclude>
        DataNode _MemberDataNode;
        ///<summary>
        ///Metadata which used from dynamic type data retriever to retrieve conventional type object
        ///</summary>
        /// <MetaDataID>{7881c487-91ab-4ede-9002-51af4bc11f6e}</MetaDataID>
        public DataNode MemberDataNode
        {
            get
            {
                return _MemberDataNode;
            }
        }





        /// <exclude>Excluded</exclude>
        ExpressionTreeNode _SourceCollectionExpression;

        /////<summary>
        /////
        /////</summary>
        ///// <MetaDataID>{8039b1ae-137f-48bd-916f-43c7d6be6b7d}</MetaDataID>
        //public ExpressionTreeNode SourceCollectionExpression
        //{
        //    get
        //    {
        //        return _SourceCollectionExpression;
        //    }
        //}

        /// <summary>
        /// Defines the expression which defines the result type.
        /// in case where CollectionProviderMethodExpression is  select o select meny expression the expresion is the selector result 
        /// </summary>
        /// <MetaDataID>{e0fb5423-68be-41de-a0c9-8e55e3fcc880}</MetaDataID>
        ExpressionTreeNode ResultTypeExpression;

        /// <exclude>Excluded</exclude>
        MethodCallAsCollectionProviderExpressionTreeNode _CollectionProviderMethodExpression;

        /// <summary>
        /// Defines the method call expression for instance "Select - SelectMany " that defines the dynamicType data retriever as result Type
        /// or  member type of result Type.
        /// </summary>
        /// <MetaDataID>{9d6862d3-b0de-4f64-ae90-4462fc5fc1ee}</MetaDataID>
        public MethodCallAsCollectionProviderExpressionTreeNode CollectionProviderMethodExpression
        {
            get
            {
                return _CollectionProviderMethodExpression;
            }
        }




        ///<summary>
        ///This contstructor used when the dynamic type data retriever acts as enumerator on conventional type objects
        ///</summary>
        /// <MetaDataID>{3600905c-c54b-46b4-9284-0fbf8cc92173}</MetaDataID>
        public DynamicTypeDataRetrieve(ILINQObjectQuery objectQuery,
                                DataNode rootDataNode,
                                DataNode memberDataNode,
                                ExpressionTreeNode typeDefinitionExpresion)
        {
            ResultTypeExpression = typeDefinitionExpresion;
            if (typeDefinitionExpresion is GroupKeyExpressionTreeNode)
            {

                while (!(typeDefinitionExpresion is MethodCallAsCollectionProviderExpressionTreeNode))
                    typeDefinitionExpresion = typeDefinitionExpresion.Parent;

                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                _SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;

            }
            else if (typeDefinitionExpresion is GroupByExpressionTreeNode)
            {
                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                _SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;
            }
            else if (typeDefinitionExpresion is ParameterExpressionTreeNode)
            {
                while (!(typeDefinitionExpresion is MethodCallAsCollectionProviderExpressionTreeNode))
                    typeDefinitionExpresion = typeDefinitionExpresion.Parent;

                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                //_SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;
                _SourceCollectionExpression = typeDefinitionExpresion;
            }
            else
            {
                _SourceCollectionExpression = typeDefinitionExpresion;
                if (typeDefinitionExpresion is WhereExpressionTreeNode)
                    _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;

                if (typeDefinitionExpresion is OrderByExpressionTreeNode)
                    _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;

            }

            _ObjectQuery = objectQuery;
            _RootDataNode = rootDataNode;
            _MemberDataNode = memberDataNode;
            if (rootDataNode.IsParentDataNode(memberDataNode))
                _RootDataNode = _MemberDataNode;
            ObjectQuery.Translator.DataNodeTreeSimplification += new OOAdvantech.Linq.Translators.DataNodeTreesSimplificationHandler(OnDataNodeTreeSimplification);

            SelectionDataNodesAsBranch.Add(RootDataNode);
            RetrieveSelectionDataNodesAsBranch(memberDataNode);

            //if (_RootDataNode.Type == DataNode.DataNodeType.Group && Type.Name == typeof(System.Linq.IGrouping<,>).Name)
            //{
            //    _GroupingMetaData = new GroupingMetaData(this);
            //    _GroupingMetaData.GroupingType = Type;
            //    System.Type keyType = _GroupingMetaData.GroupingType.GetGenericArguments()[0];
            //    foreach (System.Reflection.PropertyInfo propertyInfo in keyType.GetProperties())
            //    {
            //        foreach (DataNode groupingDataNode in _RootDataNode.GroupKeyDataNodes)
            //        {
            //            if (propertyInfo.Name == groupingDataNode.Alias)
            //            {
            //                RetrieveSelectionDataNodesAsBranch(groupingDataNode);
            //                _GroupingMetaData.KeyProperties[propertyInfo] = new DynamicTypeProperty(propertyInfo, groupingDataNode, null);
            //                break;
            //            }
            //        }
            //    }
            //    _GroupingMetaData.KeyConstInfo = keyType.GetConstructors()[0];
            //    _GroupingMetaData.FastKeyConstructorInvoke = AccessorBuilder.CreateConstructorInvoker(_GroupingMetaData.KeyConstInfo);
            //    foreach (System.Reflection.ParameterInfo paramInfo in _GroupingMetaData.KeyConstInfo.GetParameters())
            //    {
            //        System.Reflection.PropertyInfo property = _GroupingMetaData.KeyConstInfo.DeclaringType.GetProperty(paramInfo.Name);
            //        _GroupingMetaData.CtorParamsDataRetrieverProperies.Add(_GroupingMetaData.KeyProperties[property]);
            //    }
            //}

        }

        /// <exclude>Excluded</exclude>
        GroupingMetaData _GroupingMetaData;

        /// <MetaDataID>{39e229e7-8ffd-4d58-a36a-6de8369d99e8}</MetaDataID>
        public GroupingMetaData GroupingMetaData
        {
            get
            {
                if (_GroupingMetaData == null && IsGrouping)
                {

                    if ((CollectionProviderMethodExpression is SelectExpressionTreeNode) && _SourceCollectionExpression == CollectionProviderMethodExpression)
                    {
                        var dataRetrieve = (CollectionProviderMethodExpression as SelectExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;

                        return dataRetrieve.GroupingMetaData;
                    }

                    _GroupingMetaData = new GroupingMetaData(this, _SourceCollectionExpression, (CollectionProviderMethodExpression as GroupByExpressionTreeNode).GroupKeyExpression);

                    //foreach (DataNode groupingDataNode in  RootDataNode.GroupKeyDataNodes)
                    //    RetrieveSelectionDataNodesAsBranch(groupingDataNode);
                }
                return _GroupingMetaData;
            }
        }


        /// <MetaDataID>{be524b67-6759-47b8-92fc-be39817c70e4}</MetaDataID>
        public bool IsGrouping
        {
            get
            {

                return _RootDataNode.Type == DataNode.DataNodeType.Group && Type.Name == typeof(System.Linq.IGrouping<,>).Name && CollectionProviderMethodExpression is GroupByExpressionTreeNode;
            }
        }




        /// <MetaDataID>{e7539446-34d0-418f-9705-226647311775}</MetaDataID>
        NewExpressionTreeNode NewExpression;

        /// <summary>
        ///This contstructor used when the dynamic type data retriever 
        ///acts as enumerator on dynamictype objects or 
        ///acts as  subsidiary of dynamic type data retriever enumerator
        /// </summary>
        /// <param name="objectQuery">
        /// This parameter defines the query which will use the dynamic type.  
        /// </param>
        /// <param name="rootDataNode">
        /// This parameter defines the data node where system uses as main source to retrieve the objects of this type.
        /// Usually is the data node which produced for the first form expression of a selection query . 
        /// </param>
        /// <param name="dynamicTypeProperties">
        /// This parameter defines the properties of dynamic type enumerator.
        /// </param>
        /// <MetaDataID>{80bcb833-581b-4a9b-812a-1ccaff5305cd}</MetaDataID>
        public DynamicTypeDataRetrieve(ILINQObjectQuery objectQuery,
                                DataNode rootDataNode,
                                Dictionary<object, DynamicTypeProperty> dynamicTypeProperties,
                                ExpressionTreeNode typeDefinitionExpresion)
        {
            ResultTypeExpression = typeDefinitionExpresion;
            if (typeDefinitionExpresion is NewExpressionTreeNode)
            {
                NewExpression = typeDefinitionExpresion as NewExpressionTreeNode;
                NewExpression._DynamicTypeDataRetrieve = this;
                while (!(typeDefinitionExpresion is MethodCallAsCollectionProviderExpressionTreeNode))
                    typeDefinitionExpresion = typeDefinitionExpresion.Parent;
                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                _SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;

            }
            else if (typeDefinitionExpresion is GroupKeyExpressionTreeNode)
            {
                //NewExpression = treeNode as NewExpressionTreeNode;
                //NewExpression.DynamicTypeDataRetrieve = this;
                while (!(typeDefinitionExpresion is MethodCallAsCollectionProviderExpressionTreeNode))
                    typeDefinitionExpresion = typeDefinitionExpresion.Parent;

                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                _SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;

            }
            else if (typeDefinitionExpresion is GroupByExpressionTreeNode)
            {
                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                _SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;
            }
            else if (typeDefinitionExpresion is OrderByExpressionTreeNode)
            {
                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                _SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;
            }
            else if (typeDefinitionExpresion is ParameterExpressionTreeNode)
            {
                while (!(typeDefinitionExpresion is MethodCallAsCollectionProviderExpressionTreeNode))
                    typeDefinitionExpresion = typeDefinitionExpresion.Parent;

                _CollectionProviderMethodExpression = typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode;
                //_SourceCollectionExpression = (typeDefinitionExpresion as MethodCallAsCollectionProviderExpressionTreeNode).SourceCollection;
                _SourceCollectionExpression = typeDefinitionExpresion;
            }
            else
                _SourceCollectionExpression = typeDefinitionExpresion;

            objectQuery.Translator.AddDynamicTypeDataRetriever(Type, this);
            _ObjectQuery = objectQuery;


            _RootDataNode = rootDataNode;
            _Properties = dynamicTypeProperties;

            foreach (DynamicTypeProperty dynamicTypeProperty in _Properties.Values)
                dynamicTypeProperty.PropertyOwnerType = this;

            SelectionDataNodesAsBranch.Add(RootDataNode);
            foreach (DynamicTypeProperty dynamicTypeProperty in _Properties.Values)
                if (!dynamicTypeProperty.IsLocalScopeValue)
                    RetrieveSelectionDataNodesAsBranch(dynamicTypeProperty.SourceDataNode);

            ObjectQuery.Translator.DataNodeTreeSimplification += new OOAdvantech.Linq.Translators.DataNodeTreesSimplificationHandler(OnDataNodeTreeSimplification);
            if (!_RootDataNode.Recursive)
            {
                foreach (KeyValuePair<object, DynamicTypeProperty> entry in _Properties)
                    if (entry.Value.SourceDataNode.Recursive)
                    {
                        Dictionary<object, DynamicTypeProperty> recursivDynamicTypeProperties = new Dictionary<object, DynamicTypeProperty>();
                        foreach (DataNode subDataNode in entry.Value.SourceDataNode.SubDataNodes)
                        {
                            foreach (DynamicTypeProperty dynamicTypeProperty in _Properties.Values)
                            {
                                if (dynamicTypeProperty.SourceDataNode.Name == subDataNode.Name)
                                {
                                    DynamicTypeProperty recursiveDynamicTypeProperty = new DynamicTypeProperty(dynamicTypeProperty.PropertyInfo, subDataNode, null, dynamicTypeProperty.TreeNode);

                                    recursivDynamicTypeProperties.Add(dynamicTypeProperty.PropertyInfo, recursiveDynamicTypeProperty);
                                }
                            }

                        }
                        if (entry.Key is System.Reflection.PropertyInfo)
                            recursivDynamicTypeProperties[entry.Key] = new DynamicTypeProperty(entry.Key as System.Reflection.PropertyInfo, entry.Value.SourceDataNode, null, entry.Value.TreeNode);
                        if (entry.Key is System.Reflection.ParameterInfo)
                            recursivDynamicTypeProperties[entry.Key] = new DynamicTypeProperty(entry.Key as System.Reflection.ParameterInfo, entry.Value.SourceDataNode, null, entry.Value.TreeNode);

                        DynamicTypeDataRetrieve<ObjectType> recursiveDynamicDataRetrieve = new DynamicTypeDataRetrieve<ObjectType>(objectQuery, entry.Value.SourceDataNode, recursivDynamicTypeProperties, ResultTypeExpression);

                        if (entry.Key is System.Reflection.PropertyInfo)
                            recursivDynamicTypeProperties[entry.Key] = new DynamicTypeProperty(entry.Key as System.Reflection.PropertyInfo, entry.Value.SourceDataNode, recursiveDynamicDataRetrieve, entry.Value.TreeNode);
                        if (entry.Key is System.Reflection.ParameterInfo)
                            recursivDynamicTypeProperties[entry.Key] = new DynamicTypeProperty(entry.Key as System.Reflection.ParameterInfo, entry.Value.SourceDataNode, recursiveDynamicDataRetrieve, entry.Value.TreeNode);

                        foreach (DynamicTypeProperty dynamicTypeProperty in recursiveDynamicDataRetrieve.Properties.Values)
                            dynamicTypeProperty.PropertyOwnerType = recursiveDynamicDataRetrieve;

                        if (entry.Key is System.Reflection.PropertyInfo)
                            _Properties[entry.Key] = new DynamicTypeProperty(entry.Key as System.Reflection.PropertyInfo, entry.Value.SourceDataNode, recursiveDynamicDataRetrieve, entry.Value.TreeNode);
                        if (entry.Key is System.Reflection.ParameterInfo)
                            _Properties[entry.Key] = new DynamicTypeProperty(entry.Key as System.Reflection.ParameterInfo, entry.Value.SourceDataNode, recursiveDynamicDataRetrieve, entry.Value.TreeNode);


                        _Properties[entry.Key].PropertyOwnerType = this;
                        break;
                    }
            }

            #region Build Grouping Metadata
            DataNode groupDataNode = null;
            if (_Properties != null)
            {
                System.Type keyType = null;
                foreach (DynamicTypeProperty dynamicProperty in _Properties.Values)
                {
                    if (dynamicProperty.SourceDataNode.Type == DataNode.DataNodeType.Key)
                    {
                        keyType = dynamicProperty.Type;
                        //foreach (DataNode groupingDataNode in (dynamicProperty.SourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                        //    RetrieveSelectionDataNodesAsBranch(groupingDataNode);
                        foreach (DataNode groupKeyDataNode in dynamicProperty.SourceDataNode.SubDataNodes)
                            RetrieveSelectionDataNodesAsBranch(groupKeyDataNode);
                    }
                }
            }
            #endregion


            OnDataNodeTreeSimplification(new Dictionary<DataNode, DataNode>());
        }

        /// <summary>
        /// The copy constructor of DynamicTypeDataRetrieve
        /// </summary>
        /// <param name="copyEnumerator"></param>
        /// <MetaDataID>{65cb7fcd-f8ea-4a7b-920b-dd2c878d393a}</MetaDataID>
        DynamicTypeDataRetrieve(DynamicTypeDataRetrieve<ObjectType> copyEnumerator)
        {
            //_DataNodeRowIndices = copyEnumerator._DataNodeRowIndices;
            _CtorParametersIndices = copyEnumerator.CtorParametersIndices;
            //CompositeRows = copyEnumerator.CompositeRows;
            _GroupingMetaData = copyEnumerator._GroupingMetaData;
            //GroupedData = copyEnumerator.GroupedData;
            //if(this.CompositeRows!=null)
            //    RealEnum = this.CompositeRows.GetEnumerator();
            _MemberDataNode = copyEnumerator.MemberDataNode;
            ConstructorInfo = copyEnumerator.ConstructorInfo;
            FastConstructorInvoke = copyEnumerator.FastConstructorInvoke;
            Parameters = copyEnumerator.Parameters;
            _ObjectQuery = copyEnumerator.ObjectQuery;

            _Properties = copyEnumerator._Properties;
            //PropertiesIndices = copyEnumerator.PropertiesIndices;
            _RootDataNode = copyEnumerator.RootDataNode;
            SelectionDataNodesAsBranch = copyEnumerator.SelectionDataNodesAsBranch;
            //ConventionTypeRowIndex = copyEnumerator.ConventionTypeRowIndex;
            //ConventionTypeColumnIndex = copyEnumerator.ConventionTypeColumnIndex;
            //_ExtensionRow = copyEnumerator.ExtensionRow;
            _QueryResultDataLoader = copyEnumerator._QueryResultDataLoader;
            QueryResult = copyEnumerator.QueryResult;
        }
        /// <MetaDataID>{ec4f35a2-5a77-478c-8fed-a034c60b5f82}</MetaDataID>
        DynamicTypeDataRetrieve(DynamicTypeDataRetrieve<ObjectType> copyEnumerator, QueryResultDataLoader queryResultDataLoader)
                    : this(copyEnumerator)
        {
            _QueryResultDataLoader = queryResultDataLoader;
            QueryResult = copyEnumerator.QueryResult;

        }


        /// <summary>
        /// The Type property defines the type of produced object.
        /// </summary>
        /// <MetaDataID>{335959bb-07ce-4030-a640-a9072f344200}</MetaDataID>
        public Type Type
        {
            get
            {
                return typeof(ObjectType);
            }
        }


        /// <exclude>Excluded</exclude>
        Dictionary<object, DynamicTypeProperty> _Properties;
        /// <summary>
        /// This dictionary contains properties Meta data useful for dynamic type data retriever 
        /// to retrieves object properties values.
        ///  </summary>
        /// <MetaDataID>{85335332-c2fc-4a9e-b12a-46dc3c6e4db0}</MetaDataID>
        public Dictionary<object, DynamicTypeProperty> Properties
        {
            get
            {
                return _Properties;
            }
        }


        /////<summary>
        /////Defines the master row of parent DataNode.
        /////From master row gets details rows for enumerable dynamic type retrieve.   
        /////</summary>
        ///// <MetaDataID>{9a293914-2b77-49cb-8637-7384742e19fa}</MetaDataID>
        //System.Data.DataRow MasterRow;


        /// <MetaDataID>{d471d082-db57-472c-923d-a02fd1e090d3}</MetaDataID>
        public object GetRelatedDataEnumerator(QueryResultDataLoader queryResultDataLoader)
        {
            queryResultDataLoader.ObjectQueryContextReference.ObjectQueryContext = QueryResult.ObjectQueryContextReference.ObjectQueryContext;
            if (queryResultDataLoader.Type.IsGroupingType)
                return System.Activator.CreateInstance(typeof(GroupingDataRetriever<,>).MakeGenericType(Type.GetMetaData().GetGenericArguments()), this.GroupingMetaData.GroupedDataRetrieve, queryResultDataLoader);
            else
                return new DynamicTypeDataRetrieve<ObjectType>(this, queryResultDataLoader);


        }

        /// <MetaDataID>{c61c1d80-55ec-4c5a-b161-379d1ead0f8d}</MetaDataID>
        public IDynamicTypeDataRetrieve GetNewEnumarator()
        {
            return new DynamicTypeDataRetrieve<ObjectType>(this, QueryResultDataLoader);
        }

        public IEnumerator GetEnumarator()
        {

            //if (Properties==null&& MemberDataNode!=null&&MemberDataNode.Type == DataNode.DataNodeType.Group && typeof(ObjectType).GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>))
            //{

            //    var dataRetrieve = CollectionProviderMethodExpression.SourceCollection.DynamicTypeDataRetrieve;



            //    var queryResultDataLoader = dataRetrieve.QueryResult.DataLoader;
            //    var pas= queryResultDataLoader.Type = dataRetrieve.QueryResult;

            //    var sss =      System.Activator.CreateInstance(typeof(GroupingDataRetriever<,>).MakeGenericType(Type.GetMetaData().GetGenericArguments()), dataRetrieve.GroupingMetaData.KeyDynamicTypeDataRetrieve, dataRetrieve.GroupingMetaData.GroupedDataRetrieve, queryResultDataLoader);

            //    var ennn = (sss as IEnumerable).GetEnumerator();
            //    return ennn as IEnumerator<ObjectType>;

            //    //public object GetRelatedDataEnumerator(QueryResultDataLoader queryResultDataLoader)
            //    //{
            //    //    queryResultDataLoader.ObjectQueryContextReference.ObjectQueryContext = QueryResult.ObjectQueryContextReference.ObjectQueryContext;
            //    //    if (queryResultDataLoader.Type.IsGroupingType)
            //    //        return System.Activator.CreateInstance(typeof(GroupingDataRetriever<,>).MakeGenericType(Type.GetMetaData().GetGenericArguments()), this.GroupingMetaData.KeyDynamicTypeDataRetrieve, this.GroupingMetaData.GroupedDataRetrieve, queryResultDataLoader);
            //    //    else
            //    //        return new DynamicTypeDataRetrieve<ObjectType>(this, queryResultDataLoader);


            //    //}


            //    //GroupingEntry groupingEntry = value as GroupingEntry;
            //    //IEnumerator<TElement> _enum = new GroupedDataEnumerator<TElement>(groupingEntry.GroupedCompositeRows.GetEnumerator(), GroupedDataRetrieve, QueryResultDataLoader.GetEnumerator() as QueryResultLoaderEnumartor);
            //}



            return (IEnumerator<ObjectType>)(this);
        }




        //Dictionary<object, System.Data.DataRow[]> GroupedData;


        #region Data Node tree code
        /// <summary>
        /// This property defines the the object query which use the dynamic type data retriever 
        /// </summary>
        /// <exclude>Excluded</exclude>
        ILINQObjectQuery _ObjectQuery;
        /// <MetaDataID>{a7b0a223-fec9-4d98-9ad2-c1513a922429}</MetaDataID>
        public ILINQObjectQuery ObjectQuery
        {
            get
            {
                return _ObjectQuery;
            }
        }

        /// <exclude>Excluded</exclude>
        DataNode _RootDataNode;
        /// <MetaDataID>{67abd4ef-67e6-4651-b805-aeaee60dacdd}</MetaDataID>
        /// <summary>Sl</summary>
        public DataNode RootDataNode
        {
            get
            {
                return _RootDataNode;
            }
            set
            {
                _RootDataNode = value;
                if (!SelectionDataNodesAsBranch.Contains(value))
                    SelectionDataNodesAsBranch.Add(value);
            }
        }
        /// <summary>
        /// This method add all data nodes which used for data retrieve, in object query selection list.
        /// </summary>
        /// <MetaDataID>{c04cc1aa-2a0b-465d-a801-6dfc58ac3ef0}</MetaDataID>
        public void ParticipateInQueryResults(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.SearchCondition searchCondition)
        {
            //BuildDataRetrieveMetadata();

            //ExpressionTreeNode parent = SourceTreeNode;
            //while (!(parent is QueryExpressions.SelectExpressionTreeNode) && parent != null)
            //    parent = parent.Parent;
            //QueryExpressions.SelectExpressionTreeNode selectExpression = parent as QueryExpressions.SelectExpressionTreeNode;
            ExpressionTreeNode sourceTreeNode = _SourceCollectionExpression;
            if (sourceTreeNode is ParameterExpressionTreeNode)
                sourceTreeNode = (sourceTreeNode as ParameterExpressionTreeNode).SourceCollection;
            if (sourceTreeNode is GroupKeyExpressionTreeNode)
                sourceTreeNode = sourceTreeNode.Parent;
            if (sourceTreeNode is AggregateFunctionExpressionTreeNode)
                sourceTreeNode = sourceTreeNode.Parent;

            if (sourceTreeNode is OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode && RootDataNode.Type == DataNode.DataNodeType.Group)
                (sourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).DynamicTypeDataRetrieve.ParticipateInQueryResults(searchCondition);

            if (MemberDataNode != null && MemberDataNode.Type == DataNode.DataNodeType.Group)
            {
                var sourceCollectionDataRetrieve = (CollectionProviderMethodExpression as Linq.QueryExpressions.SelectExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;
                sourceCollectionDataRetrieve.ParticipateInQueryResults(searchCondition);
            }


            #region code removed

            //if (sourceTreeNode is OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode && RootDataNode.Type == DataNode.DataNodeType.Group)
            //{

            //    if (GroupingMetaData != null)
            //    {
            //        ObjectQuery.AddSelectListItem(RootDataNode);
            //        foreach (DataNode dataNode in (RootDataNode as GroupDataNode).GroupKeyDataNodes)
            //            ObjectQuery.AddSelectListItem(dataNode);
            //    }

            //    if ((sourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).KeyDynamicTypeDataRetrieve != null)
            //    {
            //        foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in (sourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).KeyDynamicTypeDataRetrieve.Properties)
            //        {
            //            if (entry.Value.PropertyType != null)
            //                entry.Value.PropertyType.ParticipateInSelectList();
            //            else
            //            {
            //                if (entry.Value.SourceDataNode.Type == DataNode.DataNodeType.Key)
            //                    foreach (var keyDataNode in (entry.Value.SourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
            //                        ObjectQuery.AddSelectListItem(keyDataNode);
            //                var shrc = entry.Value.TreeNode.FilterDataCondition;
            //                shrc = SearchCondition.JoinSearchConditions((sourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).SourceCollection.FilterDataCondition, shrc);
            //                entry.Value.SourceDataNode.AddSearchCondition(shrc);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var shrc = (sourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).SourceCollection.FilterDataCondition;
            //        (RootDataNode as GroupDataNode).GroupedDataNode.AddSearchCondition(shrc);
            //    }
            //    (RootDataNode as GroupDataNode).GroupingSourceSearchCondition = (sourceTreeNode as OOAdvantech.Linq.QueryExpressions.GroupByExpressionTreeNode).SourceCollection.FilterDataCondition;
            //}
            #endregion

            searchCondition = SearchCondition.JoinSearchConditions(searchCondition, _SourceCollectionExpression.FilterDataCondition);

            if (Properties != null)
            {

                if (_CollectionProviderMethodExpression is GroupByExpressionTreeNode && RootDataNode is GroupDataNode)
                    (RootDataNode as GroupDataNode).GroupingSourceSearchCondition = _SourceCollectionExpression.FilterDataCondition;
                else
                {
                    //if (_SourceCollectionExpression.FilterDataCondition != null)
                    //    RootDataNode.AddSearchCondition(_SourceCollectionExpression.FilterDataCondition);
                    if (searchCondition != null)
                        RootDataNode.AddSearchCondition(searchCondition);

                }
                foreach (KeyValuePair<object, DynamicTypeProperty> prepertyEntry in Properties)
                {
                    if (prepertyEntry.Value.IsLocalScopeValue || prepertyEntry.Value.SourceDataNode == RootDataNode && RootDataNode.Recursive)
                        continue;
                    DataNode propertyFilterDataNode = prepertyEntry.Value.SourceDataNode;
                    if (prepertyEntry.Value.TreeNode is ParameterExpressionTreeNode)
                    {
                        foreach (var groupByDataRetriever in (prepertyEntry.Value.TreeNode as ParameterExpressionTreeNode).GetPathGroupByDataRetrievers())
                        {
                            propertyFilterDataNode = groupByDataRetriever.RootDataNode;
                            break;
                        }
                    }


                    //foreach (TypeExpressionNode node in prepertyEntry.Value.TypeExpresionsPathToSource)
                    //{

                    //    if (node.SourceDynamicTypeDataRetrieve != null && node.SourceDynamicTypeDataRetrieve.GroupingMetaData != null)
                    //    {
                    //        propertyFilterDataNode = node.SourceDynamicTypeDataRetrieve.RootDataNode;
                    //        break;
                    //    }
                    //}


                    // var vel = prepertyEntry.Value.TypeExpresionsPathToSource;

                    if (prepertyEntry.Value.PropertyType != null)
                    {
                        prepertyEntry.Value.PropertyType.ParticipateInQueryResults(searchCondition);
                        var filterDataCondition = prepertyEntry.Value.FilterDataCondition;
                        if (RootDataNode.Type != DataNode.DataNodeType.Group)
                        {

                            propertyFilterDataNode.AddSearchCondition(SearchCondition.JoinSearchConditions(searchCondition, filterDataCondition));
                            //propertyFilterDataNode.AddSearchCondition(filterDataCondition);
                            //prepertyEntry.Value.SourceDataNode.AddSearchCondition(filterDataCondition);
                        }

                    }
                    else
                    {
                        if (prepertyEntry.Value.SourceDataNode.Recursive)
                        {
                            //TODO Write TestCase
                            foreach (DataNode dataNode in prepertyEntry.Value.SourceDataNode.SubDataNodes)
                                ObjectQuery.AddSelectListItem(dataNode);
                        }
                        else
                            ObjectQuery.AddSelectListItem(prepertyEntry.Value.SourceDataNode);

                        var filterDataCondition = prepertyEntry.Value.FilterDataCondition;

                        #region Code will be removed
                        System.Diagnostics.Debug.Assert(SearchCondition.JoinSearchConditions(_SourceCollectionExpression.FilterDataCondition, filterDataCondition) == filterDataCondition);
                        if (SearchCondition.JoinSearchConditions(_SourceCollectionExpression.FilterDataCondition, filterDataCondition) != filterDataCondition)
                        {

                        }
                        //shrc = SearchCondition.JoinSearchConditions(_SourceCollectionExpression.SearchCondition, shrc);
                        #endregion

                        if (RootDataNode.Type != DataNode.DataNodeType.Group)
                        {
                            propertyFilterDataNode.AddSearchCondition(SearchCondition.JoinSearchConditions(searchCondition, filterDataCondition));
                            //propertyFilterDataNode.AddSearchCondition(filterDataCondition);
                            //prepertyEntry.Value.SourceDataNode.AddSearchCondition(filterDataCondition);
                        }

                    }
                    if (prepertyEntry.Value.TreeNode is ParameterExpressionTreeNode)
                    {
                        foreach (var groupByDataRetriever in (prepertyEntry.Value.TreeNode as ParameterExpressionTreeNode).GetPathGroupByDataRetrievers())
                            groupByDataRetriever.ParticipateInQueryResults(searchCondition);
                    }
                }
                if (!RootDataNode.BranchParticipateInSelectClause)
                    ObjectQuery.AddSelectListItem(RootDataNode);

            }
            else
            {

                if (ResultTypeExpression is OOAdvantech.Linq.QueryExpressions.AggregateFunctionExpressionTreeNode && MemberDataNode is AggregateExpressionDataNode)
                {
                    //_SourceCollectionExpression.NamePrefix = "SL_" + _SourceCollectionExpression.NamePrefix;
                    ObjectQuery.AddSelectListItem(MemberDataNode);
                    var shrc = (ResultTypeExpression as OOAdvantech.Linq.QueryExpressions.AggregateFunctionExpressionTreeNode).SourceCollection.FilterDataCondition;

                    #region Deleted code
                    //if ((_SourceCollectionExpression as OOAdvantech.Linq.QueryExpressions.AggregateFunctionExpressionTreeNode).SourceCollection is QueryExpressions.ParameterExpressionTreeNode)
                    //{
                    //    QueryExpressions.SelectExpressionTreeNode selectExpressionTreeNode = GetParentSelectExpression((_SourceCollectionExpression as OOAdvantech.Linq.QueryExpressions.AggregateFunctionExpressionTreeNode).SourceCollection as QueryExpressions.ParameterExpressionTreeNode);
                    //    if (selectExpressionTreeNode != null)
                    //        shrc = SearchCondition.JoinSearchConditions(selectExpressionTreeNode.SearchCondition, shrc);
                    //}
                    #endregion

                    if (MemberDataNode.ParentDataNode is GroupDataNode)
                    {
                        if ((MemberDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
                        {
                            (MemberDataNode.ParentDataNode as GroupDataNode).GroupingSourceSearchCondition = SearchCondition.JoinSearchConditions(shrc, (MemberDataNode as AggregateExpressionDataNode).SourceSearchCondition);
                            (MemberDataNode as AggregateExpressionDataNode).SourceSearchCondition = null;
                        }
                        else
                            (MemberDataNode.ParentDataNode as GroupDataNode).GroupingSourceSearchCondition = shrc;

                    }
                    else
                        MemberDataNode.ParentDataNode.AddSearchCondition(SearchCondition.JoinSearchConditions(searchCondition, shrc));
                }
                else
                {
                    //_SourceCollectionExpression.NamePrefix = "SL_" + _SourceCollectionExpression.NamePrefix;
                    ObjectQuery.AddSelectListItem(MemberDataNode);


                    #region Deleted code
                    //if (_SourceCollectionExpression is QueryExpressions.ParameterExpressionTreeNode)
                    //{
                    //    QueryExpressions.SelectExpressionTreeNode selectExpressionTreeNode = GetParentSelectExpression(_SourceCollectionExpression as QueryExpressions.ParameterExpressionTreeNode);
                    //    if (selectExpressionTreeNode != null)
                    //        shrc = JoinSearchConditions(selectExpressionTreeNode.SearchCondition, shrc);
                    //}
                    #endregion
                    if (CollectionProviderMethodExpression is GroupByExpressionTreeNode)
                    {
                        var shrc = CollectionProviderMethodExpression.SourceCollection.FilterDataCondition;
                        MemberDataNode.AddSearchCondition(shrc);
                        if (_RootDataNode.Type == DataNode.DataNodeType.Group && Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                            (_RootDataNode as GroupDataNode).GroupingSourceSearchCondition = shrc;

                    }
                    else
                    {
                        var shrc = ResultTypeExpression.FilterDataCondition;
                        MemberDataNode.AddSearchCondition(SearchCondition.JoinSearchConditions(searchCondition, shrc));
                        //if (_RootDataNode.Type == DataNode.DataNodeType.Group && Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                        //    (_RootDataNode as GroupDataNode).GroupingSourceSearchCondition = shrc;
                    }

                }

            }
            if (GroupingMetaData != null)
            {
                ObjectQuery.AddSelectListItem(RootDataNode);
                foreach (DataNode dataNode in (RootDataNode as GroupDataNode).GroupKeyDataNodes)
                    ObjectQuery.AddSelectListItem(dataNode);
                GroupingMetaData.GroupedDataRetrieve.ParticipateInQueryResults(CollectionProviderMethodExpression.SourceCollection.FilterDataCondition);
            }

        }

        /// <MetaDataID>{981e0b16-62aa-4ba5-90fa-0fd8d46d77ba}</MetaDataID>
        private OOAdvantech.Linq.QueryExpressions.SelectExpressionTreeNode GetParentSelectExpression(QueryExpressions.ParameterExpressionTreeNode treeNode)
        {
            ExpressionTreeNode parent = treeNode.Parent;
            while (parent != null && !(parent is QueryExpressions.SelectExpressionTreeNode))
                parent = parent.Parent;
            return parent as QueryExpressions.SelectExpressionTreeNode;
        }

        ///// <MetaDataID>{074a0e18-25f6-4ee7-b910-e8350a1ea5a5}</MetaDataID>
        //private SearchCondition JoinSearchConditions(SearchCondition leftSearchCondition, SearchCondition rightearchCondition)
        //{
        //    if (leftSearchCondition == null && rightearchCondition == null)
        //        return null;
        //    if (leftSearchCondition != null && rightearchCondition == null)
        //        return leftSearchCondition;

        //    if (leftSearchCondition == null && rightearchCondition != null)
        //        return rightearchCondition;

        //    if (leftSearchCondition.ContainsSearchCondition(rightearchCondition))
        //        return leftSearchCondition;
        //    if (rightearchCondition.ContainsSearchCondition(leftSearchCondition))
        //        return rightearchCondition;

        //    List<SearchFactor> searchFactors = new List<SearchFactor>();
        //    searchFactors.Add(new SearchFactor(leftSearchCondition));
        //    searchFactors.Add(new SearchFactor(rightearchCondition));
        //    SearchTerm searchTerm = new SearchTerm(searchFactors);
        //    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, _ObjectQuery as ObjectQuery);
        //    return newSearchCondition;


        //}

        /// <summary>
        /// This method defines the data tree simplification event consumer.
        /// Initial the linq translator produce a data node tree were a lot of data nodes are equivalent.
        /// </summary>
        /// <param name="replacedDataNodes">
        /// This parameter defines a dictionary with the replaced data nodes.
        /// </param>
        /// <MetaDataID>{321b83c9-5b36-4ed8-988d-03f630d62b0a}</MetaDataID>
        void OnDataNodeTreeSimplification(Dictionary<DataNode, DataNode> replacedDataNodes)
        {

            _RootDataNode = Translators.ExpressionVisitor.GetActualDataNode(_RootDataNode, replacedDataNodes);
            SelectionDataNodesAsBranch.Clear();
            SelectionDataNodesAsBranch.Add(_RootDataNode);

            if (ObjectQuery.QueryResult == this && !SelectionDataNodesAsBranch.Contains(_RootDataNode.HeaderDataNode))
                SelectionDataNodesAsBranch.Add(_RootDataNode.HeaderDataNode);

            if (Properties != null && Properties.Count > 0)
            {

                foreach (System.Reflection.PropertyInfo property in typeof(ObjectType).GetMetaData().GetProperties())
                {
                    if (Properties.ContainsKey(property)) //Predefined  type
                    {
                        DataNode propertyDataNode = Properties[property].SourceDataNode;
                        propertyDataNode = Translators.ExpressionVisitor.GetActualDataNode(propertyDataNode, replacedDataNodes);
                        if (propertyDataNode == Properties[property].SourceDataNode)
                            continue;
                        if (propertyDataNode != RootDataNode && !propertyDataNode.IsParentDataNode(RootDataNode))
                            throw new System.Exception("Error on selection attribute '" + propertyDataNode.Name);
                        Properties[property].SourceDataNode = propertyDataNode;
                    }
                }

                //if (TypeHelper.FindIEnumerable(entry.Key.PropertyType) != null)
                foreach (DynamicTypeProperty dynamicProperty in Properties.Values)
                {
                    if (TypeHelper.FindIEnumerable(dynamicProperty.Type) == null)
                        RetrieveSelectionDataNodesAsBranch(dynamicProperty.SourceDataNode);

                    //}
                    //foreach (DynamicTypeProperty dynamicProperty in _Properties.Values)
                    //{
                    if ((dynamicProperty.SourceDataNode.Type == DataNode.DataNodeType.Key && dynamicProperty.SourceDataNode.ParentDataNode == RootDataNode) ||
                        (dynamicProperty.SourceDataNode.Type == DataNode.DataNodeType.Group && dynamicProperty.SourceDataNode == RootDataNode))
                    {

                        if (dynamicProperty.SourceDataNode.Type == DataNode.DataNodeType.Key)
                        {
                            foreach (DataNode dataNode in dynamicProperty.SourceDataNode.SubDataNodes)
                                RetrieveSelectionDataNodesAsBranch(dataNode);
                        }
                        else if (dynamicProperty.SourceDataNode.ParentDataNode != null && (dynamicProperty.SourceDataNode.ParentDataNode is GroupDataNode))
                        {
                            foreach (DataNode dataNode in (dynamicProperty.SourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                                RetrieveSelectionDataNodesAsBranch(dataNode);
                        }
                    }
                    //}
                    //foreach (DynamicTypeProperty dynamicProperty in Properties.Values)
                    //{
                    if (TypeHelper.FindIEnumerable(dynamicProperty.Type) != null)
                    {

                        if (dynamicProperty.PropertyType != null)
                        {
                            DataNode dataNode = dynamicProperty.PropertyType.RootDataNode;
                            while (dataNode != RootDataNode && dataNode != null && !SelectionDataNodesAsBranch.Contains(dataNode.ParentDataNode))
                                dataNode = dataNode.ParentDataNode;
                            if (dataNode != null)
                            {
                                ////TODO να τσεκαριστουν τα test case που γίνεται αυτό
                                //if (dynamicProperty.PropertyType.RootDataNode.Type != DataNode.DataNodeType.Group)
                                //{
                                //    //παράγει λαθος οταν
                                //    //var aggrResault = from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                //    //                  where order.Name == "AK_1"
                                //    //                  select new
                                //    //                  {
                                //    //                      order,
                                //    //                      orderDetails = from clientOrder in order.Client.Orders
                                //    //                                     from orderDetail in clientOrder.OrderDetails
                                //    //                                     where orderDetail.Name.Like("Sprite_*") || orderDetail.Name.Like("Coc*")
                                //    //                                     select orderDetail
                                //    //                  };

                                //    //var aggrResaultD = from res in aggrResault
                                //    //                   select new
                                //    //                   {
                                //    //                       res,
                                //    //                       res.order.Name,
                                //    //                       itemCount = res.orderDetails.Count()
                                //    //                   };
                                //    dynamicProperty.PropertyType.RootDataNode = dataNode;
                                //    dynamicProperty.SourceDataNode = dataNode;
                                //}
                            }

                        }
                        if (dynamicProperty.SourceDataNode.ParentDataNode != null && dynamicProperty.SourceDataNode != RootDataNode)
                            RetrieveSelectionDataNodesAsBranch(Translators.ExpressionVisitor.GetActualDataNode(dynamicProperty.SourceDataNode.ParentDataNode, replacedDataNodes));
                    }
                }
            }

            if (_MemberDataNode != null)
            {
                _MemberDataNode = Translators.ExpressionVisitor.GetActualDataNode(MemberDataNode, replacedDataNodes);
                if (MemberDataNode != RootDataNode && !RootDataNode.HasAlias(_MemberDataNode.Name) && !MemberDataNode.IsParentDataNode(RootDataNode))
                {
                    if (RootDataNode.Type != DataNode.DataNodeType.Group || ((RootDataNode as GroupDataNode).GroupedDataNode != MemberDataNode))
                        throw new System.Exception("Error on selection attribute '" + MemberDataNode.Name);
                }
                RetrieveSelectionDataNodesAsBranch(MemberDataNode);
                if (_MemberDataNode.Type == DataNode.DataNodeType.Key)
                {
                    foreach (DataNode dataNode in (_MemberDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                        RetrieveSelectionDataNodesAsBranch(dataNode);
                }
                if (_MemberDataNode.Type == DataNode.DataNodeType.Group)
                {
                    foreach (DataNode dataNode in (_MemberDataNode as GroupDataNode).GroupKeyDataNodes)
                        RetrieveSelectionDataNodesAsBranch(dataNode);
                }
                if (RootDataNode.Type == DataNode.DataNodeType.Group && Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                {
                    foreach (DataNode dataNode in (RootDataNode as GroupDataNode).GroupKeyDataNodes)
                        RetrieveSelectionDataNodesAsBranch(dataNode);
                }

            }





        }


        /// <summary>
        /// SelectionDataNodesAsBranch defines all DataNodes which used 
        /// from the system to retrieve the data from the data tree. 
        /// Usually are the root DataNode and all DataNodes between the root DataNode  
        /// and member DataNode or DynamicType properties DataNodes.
        /// </summary>
        /// <MetaDataID>{23a0e408-51f9-4c80-b8ec-189829a981f2}</MetaDataID>
        List<DataNode> SelectionDataNodesAsBranch = new List<DataNode>();

        /// <summary>
        /// This method retrieves all precedents DataNodes in data tree and add them to the SelectionDataNodesAsBranch collection. 
        /// Usually stop on root DataNode that is already added in SelectionDataNodesAsBranch collection 
        /// or on precedent DataNode which is already added in SelectionDataNodesAsBranch collection.
        /// </summary>
        /// <param name="dataNode">
        /// dataNode parameter defines the start point for the backward search.  
        /// </param>
        /// <MetaDataID>{fa3ff3f5-d0ea-43bc-8992-49effba9b8b8}</MetaDataID>
        private void RetrieveSelectionDataNodesAsBranch(DataNode dataNode)
        {
            if (RootDataNode.Type == DataNode.DataNodeType.Group)
            {
                if ((RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(DerivedDataNode.GetOrgDataNode(dataNode)) && Type.Name != typeof(System.Linq.IGrouping<,>).Name)
                {
                    if (!SelectionDataNodesAsBranch.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
                        SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                    return;
                }
                foreach (DataNode subDataNode in RootDataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Group && (subDataNode as GroupDataNode).GroupKeyDataNodes.Contains(DerivedDataNode.GetOrgDataNode(dataNode)))
                    {
                        SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                        RetrieveSelectionDataNodesAsBranch(subDataNode);
                        return;
                    }
                }
            }
            if (!SelectionDataNodesAsBranch.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
            {
                SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                if (dataNode.ParentDataNode != null)
                    RetrieveSelectionDataNodesAsBranch(dataNode.ParentDataNode);
            }
        }


        #endregion






        /// <MetaDataID>{cee133b9-a9f7-40a1-ac77-d379f97b1ac9}</MetaDataID>
        Dictionary<int, object> _CtorParametersLocalScopeValues = new Dictionary<int, object>();

        /// <exclude>Excluded</exclude>   
        Dictionary<int, int[]> _CtorParametersIndices;
        /// <summary>
        /// CtorParametersIndices defines a dictionary which keeps the indices on composite row for each paremeter of dynamic type costactor. 
        /// Each constactor parameter has two indices the first index used from system to access the row on in composite row 
        /// and the second index used from system to access the cell value on row.
        /// </summary>
        /// <MetaDataID>{6f277176-4632-4be8-be5b-390ae290c631}</MetaDataID>
        internal Dictionary<int, int[]> CtorParametersIndices
        {
            get
            {
                if (_CtorParametersIndices != null)
                    return _CtorParametersIndices;

                _CtorParametersIndices = new Dictionary<int, int[]>();

                ConstructorInfo constructorInfo = ConstructorInfo;

                if (constructorInfo == null)
                {
                    if (ResultTypeExpression.Expression is System.Linq.Expressions.NewExpression)
                        constructorInfo = (ResultTypeExpression.Expression as System.Linq.Expressions.NewExpression).Constructor;
                    else
                        constructorInfo = typeof(ObjectType).GetMetaData().GetConstructors()[0];
                }



                if (Type.Name.IndexOf("<>f__AnonymousType") == 0 || constructorInfo.GetParameters().Length > 0)
                {
                    System.Reflection.ParameterInfo[] parameters = constructorInfo.GetParameters();
                    int i = 0;
                    foreach (System.Reflection.ParameterInfo paramInfo in parameters)
                    {
                        if (Properties.ContainsKey(paramInfo))
                        {
                            if (!Properties[paramInfo].IsLocalScopeValue)
                                _CtorParametersIndices[i] = Properties[paramInfo].QueryResultMember.PartIndices;
                            else
                            {
                                _CtorParametersIndices[i] = new int[2] { -1, -1 };
                                _CtorParametersLocalScopeValues[i] = Properties[paramInfo].LocalScopeValue;
                            }

                            i++;
                        }
                        else
                        {
                            System.Reflection.PropertyInfo property = typeof(ObjectType).GetMetaData().GetProperty(paramInfo.Name);
                            if (!Properties[property].IsLocalScopeValue)
                                _CtorParametersIndices[i] = Properties[property].QueryResultMember.PartIndices;
                            else
                            {
                                _CtorParametersIndices[i] = new int[2] { -1, -1 };
                                _CtorParametersLocalScopeValues[i] = Properties[property].LocalScopeValue;
                            }

                            i++;
                        }
                    }
                }
                else
                {
                }

                return _CtorParametersIndices;
            }

        }




        /// <MetaDataID>{79f68ee7-46fc-4b14-a965-32be8d2295f6}</MetaDataID>
        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResultLoaderEnumartor QueryResultLoaderEnumartor;

        /// <MetaDataID>{d0ff5a8c-0088-4119-b1f3-6825b72c1a6b}</MetaDataID>
        object IDynamicTypeDataRetrieve.InstantiateObject(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResultLoaderEnumartor masterTypetDataLoader, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows dataNodesRows)
        {
            QueryResultLoaderEnumartor = masterTypetDataLoader;
            if (Properties == null)
                return masterTypetDataLoader.Current[QueryResult.ConventionTypeRowIndex][QueryResult.ConventionTypeColumnIndex];
            else
            {
                if (_QueryResultDataLoader == null)
                    _QueryResultDataLoader = new QueryResultDataLoader(QueryResult, QueryResult.ObjectQueryContextReference);
                //_QueryResultDataLoader.MasterTypeDataLoader = masterTypetDataLoader;

                //LoadDynamicTypePropertiesValues(QueryResultLoaderEnumartor, dataNodesRows);
                // CurrentCompositeRow = QueryResultLoaderEnumartor.Current;
                // QueryResultLoaderEnumartor.DataNodesRows = dataNodesRows;
                return Current;
            }
        }
        /// <MetaDataID>{b11753b7-2407-4323-9331-735d343f6264}</MetaDataID>
        object IDynamicTypeDataRetrieve.InstantiateObject(CompositeRowData compositeRow, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows dataNodesRows)
        {
            if (Properties == null)
                return compositeRow[QueryResult.ConventionTypeRowIndex][QueryResult.ConventionTypeColumnIndex];
            else
            {
                //if (_QueryResultDataLoader == null)
                //    _QueryResultDataLoader = new QueryResultDataLoader(QueryResult);
                //_QueryResultDataLoader.MasterTypeDataLoader = masterTypetDataLoader;
                throw new System.NotImplementedException();
                //LoadDynamicTypePropertiesValues(compositeRow, dataNodesRows);
                //CurrentCompositeRow = _QueryResultDataLoader.Current;
                //QueryResultDataLoader.DataNodesRows = dataNodesRows;
                //return Current;
            }
        }




        ///// <summary>
        ///// This method loads the extension row with the enumerable object (“collection”) 
        ///// which corresponds to the type of property
        ///// </summary>
        ///// <param name="compositeRow">
        ///// This parameter defines the composite row.
        ///// The composite row used from method to load the enumerable (“collection”) object.
        ///// </param>
        ///// <param name="dynamicTypeProperty">
        ///// This parameter defines the property where the type is enumerable and the method will 
        ///// produce the enumerable (“collection”) object.
        ///// The dynamicTypeProperty keeps usefull metadata for the method. 
        ///// </param>
        ///// <param name="parentsDataNodesRows">
        ///// parentsDataNodeRows dictionary keeps the data node row pairs. 
        ///// This pairs is useful when the RetrieveDataPath of (dynamic type data retriever) property has Data nodes 
        ///// which are parents (in data tree) of rows Data Node (parameter rows defines this rows). 
        ///// </param>
        ///// <param name="rows">
        ///// This parameter defines the rows where the property dynamic type data retriever 
        ///// uses to produce the collection of objects (conventional or dynamic type.
        ///// </param>
        ///// <MetaDataID>{942d1437-7c90-46e9-9ad0-20e78f4f898e}</MetaDataID>
        //private void LoadPropertyCollection(System.Data.DataRow[] compositeRow,
        //    DynamicTypeProperty dynamicTypeProperty,
        //    Dictionary<DataNode, System.Data.DataRow> parentsDataNodesRows,
        //    System.Data.DataRow masterRow)
        //{

        //    compositeRow[PropertiesIndices[dynamicTypeProperty.PropertyInfo][0]][PropertiesIndices[dynamicTypeProperty.PropertyInfo][1]] = dynamicTypeProperty.GetRelatedDataCollection(masterRow, parentsDataNodesRows);
        //    return;
        //    if (!dynamicTypeProperty.PropertyTypeIsEnumerable)
        //        throw new System.Exception("method can load collection only for enumerable properties");

        //    System.Collections.IList collection = null;
        //    if (dynamicTypeProperty.PropertyTypeIsDynamic)//  entry.Value.PropertyType!=null)//  (elementType.Name.IndexOf("<>f__AnonymousType") == 0)
        //        compositeRow[PropertiesIndices[dynamicTypeProperty.PropertyInfo][0]][PropertiesIndices[dynamicTypeProperty.PropertyInfo][1]] = dynamicTypeProperty.PropertyType.GetRelatedDataEnumerator(masterRow, parentsDataNodesRows);
        //    else
        //    {

        //        ICollection<System.Data.DataRow> rows = dynamicTypeProperty.SourceDataNode.RealParentDataNode.DataSource.GetRelatedRows(masterRow, dynamicTypeProperty.SourceDataNode);
        //        //Δεν θα φτάσει ποτέ ο κωδικας εδώ γιατί οι callers αυτής της function με συνθήκες if αποκλύουν αυτό το γεγονός.  
        //        collection = Activator.CreateInstance(dynamicTypeProperty.PropertyInfo.PropertyType) as System.Collections.IList;
        //        //int rowRemoveIndex = dynamicTypeProperty.SourceDataNode.DataSource.RowRemoveIndex;
        //        if (masterRow != null)
        //        {

        //            foreach (System.Data.DataRow row in rows)
        //            {
        //                if (dynamicTypeProperty.SourceDataNode.SearchCondition != null && dynamicTypeProperty.SourceDataNode.SearchCondition.IsRemovedRow(row, dynamicTypeProperty.SourceDataNode.DataSource))
        //                    continue;
        //                collection.Add(row[dynamicTypeProperty.SourceDataNode.DataSource.ObjectIndex]);
        //            }
        //        }
        //        compositeRow[PropertiesIndices[dynamicTypeProperty.PropertyInfo][0]][PropertiesIndices[dynamicTypeProperty.PropertyInfo][1]] = collection;
        //    }
        //}








        ///<summary>
        ///Retrieves related rows, the master row is one from rows of composite row.
        ///</summary>
        ///<param name="compositeRow">
        ///Defines composite row which contains the master row.
        ///</param>
        ///<param name="dataNodeRowIndices">
        ///dataNodeRowIndices dictionary keeps the row index in composite row for each DataNode.
        ///</param>
        ///<param name="detailsDataNode">
        ///Defines details rows DataNode
        ///</param>
        ///<param name="masterDataNode">
        ///Defines master row DataNode
        ///</param>
        /// <MetaDataID>{79d8b0ed-fd3e-474f-8241-461f2fbc618b}</MetaDataID>
        private ICollection<IDataRow> RetrieveRelatedRows(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode masterDataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode detailsDataNode, DataNodesRows dataNodesRows)
        {
            return masterDataNode.DataSource.GetRelatedRows(dataNodesRows[masterDataNode], detailsDataNode);
        }





        #region IEnumerator<DynamicType> Members


        /// <MetaDataID>{2c484aed-d81a-4772-925e-0a815079f448}</MetaDataID>
        System.Reflection.ConstructorInfo ConstructorInfo;
        /// <MetaDataID>{a8660197-fcbc-45c2-b44a-2e30ea291da5}</MetaDataID>
        AccessorBuilder.FastInvokeHandler FastConstructorInvoke;


        /// <MetaDataID>{3d66f702-4a14-49ab-8389-89470febc26c}</MetaDataID>
        System.Reflection.ParameterInfo[] Parameters;
        /// <MetaDataID>{aaef0c0d-0ca5-4d45-959d-8a5e87233e04}</MetaDataID>
        System.Reflection.PropertyInfo[] PropertyInfos;

        /// <MetaDataID>{86de792a-36c8-49c2-acd7-ddb4be48226f}</MetaDataID>
        bool nullRow = false;


        /// <exclude>Excluded</exclude>
        ObjectType _Current;





        /// <summary>
        /// Convert the current composite row to an object. 
        /// If the type of returned object is dynamic create a new object 
        /// and initialize it through the constructor 
        /// otherwise return the retrieved object from composite row. 
        /// </summary>
        /// <MetaDataID>{eebd7290-8d93-44a1-b4d9-5bfc84b51b95}</MetaDataID>
        public ObjectType Current
        {
            get
            {
                int i = 0;
                //if (!RetrieveCurrent)
                //    return _Current;
                try
                {
                    lock (this)
                    {

                        if (Properties != null)
                        {

                            LoadDynamicTypePropertiesValues(QueryResultLoaderEnumartor, QueryResultLoaderEnumartor.DataNodesRows);
                            nullRow = true;
                            #region Gets constructor metadata
                            if (ConstructorInfo == null)
                            {
                                if (ResultTypeExpression.Expression is System.Linq.Expressions.NewExpression)
                                    ConstructorInfo = (ResultTypeExpression.Expression as System.Linq.Expressions.NewExpression).Constructor;
                                else
                                    ConstructorInfo = typeof(ObjectType).GetMetaData().GetConstructors()[0];
                                FastConstructorInvoke = AccessorBuilder.GetConstructorInvoker(ConstructorInfo);
                                Parameters = ConstructorInfo.GetParameters();
                            }
                            #endregion

                            object[] _params = new object[Parameters.Length];
                            if (Type.Name.IndexOf("<>f__AnonymousType") == 0)
                            {
                                #region loads constractor parameters values
                                i = 0;
                                foreach (System.Reflection.ParameterInfo paramInfo in Parameters)
                                {
                                    if (CtorParametersIndices[i][1] == -1 && CtorParametersIndices[i][0] == -1)
                                    {
                                        //when contructor parameter indexes is double -1 the parameter value comes from local scope variable 
                                        _params[i] = _CtorParametersLocalScopeValues[i];
                                    }
                                    else if ((QueryResultLoaderEnumartor.Current)[CtorParametersIndices[i][0]] != null)
                                    {
                                        int valueIndex = CtorParametersIndices[i][1];
                                        _params[i] = QueryResultLoaderEnumartor.Current[CtorParametersIndices[i][0]][CtorParametersIndices[i][1]];
                                        if (_params[i] is System.DBNull)
                                            _params[i] = AccessorBuilder.GetDefaultValue(paramInfo.ParameterType);
                                        else
                                        {
                                            nullRow = false;
                                            if (paramInfo.ParameterType.GetMetaData().IsValueType && _params[i] != null)
                                            {
                                                if (paramInfo.ParameterType.GetMetaData().IsGenericType &&
                                                    !paramInfo.ParameterType.GetMetaData().IsGenericTypeDefinition &&
                                                    paramInfo.ParameterType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                                                {

                                                    i++;
                                                    continue;
                                                }
                                                else
                                                {
                                                    if (paramInfo.ParameterType.GetMetaData().BaseType == typeof(System.Enum))
                                                        _params[i] = System.Enum.GetValues(paramInfo.ParameterType).GetValue((int)_params[i]);
                                                    else
                                                    {
                                                        if (_params[i] is AverageValue)
                                                            _params[i] = (_params[i] as AverageValue).Average;
                                                        else
                                                            _params[i] = System.Convert.ChangeType(_params[i], paramInfo.ParameterType);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                        _params[i] = AccessorBuilder.GetDefaultValue(paramInfo.ParameterType);
                                    i++;
                                }
                                #endregion
                                ObjectType retValue = (ObjectType)FastConstructorInvoke.Invoke(null, _params);
                                _Current = retValue;
                                if (nullRow)
                                {
                                    _Current = default(ObjectType);
                                    return _Current;
                                }
                                return retValue;
                            }
                            else
                            {
                                ObjectType retValue = default(ObjectType);
                                if (Parameters.Length > 0)
                                {
                                    #region loads constractor parameters values
                                    i = 0;

                                    foreach (System.Reflection.ParameterInfo paramInfo in Parameters)
                                    {
                                        if (CtorParametersIndices[i][1] == -1 && CtorParametersIndices[i][0] == -1)
                                        {
                                            //when contructor parameter indexes is double -1 the parameter value comes from local scope variable 
                                            _params[i] = _CtorParametersLocalScopeValues[i];
                                        }
                                        else if ((QueryResultLoaderEnumartor.Current)[CtorParametersIndices[i][0]] != null)
                                        {
                                            int valueIndex = CtorParametersIndices[i][1];
                                            _params[i] = QueryResultLoaderEnumartor.Current[CtorParametersIndices[i][0]][CtorParametersIndices[i][1]];
                                            if (_params[i] is System.DBNull)
                                                _params[i] = AccessorBuilder.GetDefaultValue(paramInfo.ParameterType);
                                            else
                                            {
                                                nullRow = false;
                                                if (paramInfo.ParameterType.GetMetaData().IsValueType && _params[i] != null)
                                                {
                                                    if (paramInfo.ParameterType.GetMetaData().IsGenericType &&
                                                        !paramInfo.ParameterType.GetMetaData().IsGenericTypeDefinition &&
                                                        paramInfo.ParameterType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                                                    {

                                                        i++;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        if (paramInfo.ParameterType.GetMetaData().BaseType == typeof(System.Enum))
                                                            _params[i] = System.Enum.GetValues(paramInfo.ParameterType).GetValue((int)_params[i]);
                                                        else
                                                        {
                                                            if (_params[i] is AverageValue)
                                                                _params[i] = (_params[i] as AverageValue).Average;
                                                            else
                                                                _params[i] = System.Convert.ChangeType(_params[i], paramInfo.ParameterType);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            _params[i] = AccessorBuilder.GetDefaultValue(paramInfo.ParameterType);
                                        i++;
                                    }
                                    #endregion

                                    retValue = (ObjectType)FastConstructorInvoke.Invoke(null, _params);
                                }
                                else
                                    retValue = (ObjectType)FastConstructorInvoke.Invoke(null, new object[0]);

                                i = 0;
                                foreach (System.Reflection.PropertyInfo propertyInfo in typeof(ObjectType).GetMetaData().GetProperties())
                                {
                                    if (Properties.ContainsKey(propertyInfo))
                                    {
                                        if (Properties[propertyInfo].IsLocalScopeValue)
                                            propertyInfo.SetValue(retValue, Properties[propertyInfo].LocalScopeValue, null);
                                        else if (QueryResultLoaderEnumartor.Current[Properties[propertyInfo].QueryResultMember.PartIndices[0]] != null)
                                        {
                                            //if (Properties[propertyInfo].SourceDataNode == RootDataNode && Properties[propertyInfo].PropertyTypeIsEnumerable)
                                            //{
                                            //    //Dictionary<DataNode, System.Data.DataRow> dataNodesRows = new Dictionary<DataNode, System.Data.DataRow>();
                                            //    //foreach (DataNode dataNode in DataNodeRowIndices.Keys)
                                            //    //    dataNodesRows.Add(dataNode, CurrentCompositeRow[DataNodeRowIndices[dataNode]]);
                                            //    var dataNodesRows = new MetaDataRepository.ObjectQueryLanguage.DataNodesRows(QueryResultDataLoader.Current , DataNodeRowIndices);

                                            //    //Properties[propertyInfo]
                                            //    QueryResultDataLoader.Current [PropertiesIndices[propertyInfo][0]][PropertiesIndices[propertyInfo][1]] = Properties[propertyInfo].GetRelatedDataCollection(QueryResultDataLoader.Current [DataNodeRowIndices[Properties[propertyInfo].SourceDataNode]], dataNodesRows);
                                            //    // LoadPropertyCollection(CurrentCompositeRow, Properties[propertyInfo], dataNodesRows, CurrentCompositeRow[DataNodeRowIndices[Properties[propertyInfo].SourceDataNode]]);// rows);
                                            //}

                                            var dynamicProperty = Properties[propertyInfo];
                                            object propertyValue = QueryResultLoaderEnumartor.Current[dynamicProperty.QueryResultMember.PartIndices[0]][dynamicProperty.QueryResultMember.PartIndices[1]];
                                            if (propertyValue is System.DBNull)
                                                propertyValue = AccessorBuilder.GetDefaultValue(propertyInfo.PropertyType);

                                            if (propertyValue != null)
                                                nullRow = false;
                                            propertyInfo.SetValue(retValue, propertyValue, null);
                                        }
                                        else
                                        {
                                            propertyInfo.SetValue(retValue, AccessorBuilder.GetDefaultValue(propertyInfo.PropertyType), null);
                                        }
                                        i++;
                                    }
                                }
                                _Current = retValue;
                                if (nullRow)
                                {
                                    _Current = default(ObjectType);
                                    return _Current;
                                }
                                return retValue;
                            }
                        }
                        else
                        {
                            if (QueryResultLoaderEnumartor.Current == null)
                            {
                                _Current = default(ObjectType);
                                return default(ObjectType);

                            }
                            object value = null;
                            ObjectType retValue;
                            if (MemberDataNode != null && MemberDataNode.Type == DataNode.DataNodeType.Group && typeof(ObjectType).GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>))
                            {

                                var queryResultDataLoader = QueryResultLoaderEnumartor.Current[QueryResult.Members[1].PartIndices[0]][QueryResult.Members[1].PartIndices[1]] as QueryResultDataLoader;
                                var groupingMetaData = (CollectionProviderMethodExpression as Linq.QueryExpressions.SelectExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve.GroupingMetaData;
                                value = System.Activator.CreateInstance(typeof(GroupingDataRetriever<,>).MakeGenericType(Type.GetMetaData().GetGenericArguments()), groupingMetaData.GroupedDataRetrieve, queryResultDataLoader);

                                if (QueryResult.Members[0].SourceDataNode.Type == DataNode.DataNodeType.Key &&
                                    DerivedDataNode.GetOrgDataNode(QueryResult.Members[0].SourceDataNode.SubDataNodes[0]).Type == DataNode.DataNodeType.OjectAttribute &&
                                    QueryResult.Members[0] is SinglePart)
                                {
                                    GroupingEntry groupingEntry = queryResultDataLoader.DataNodesRows.CompositeRow[queryResultDataLoader.Type.Members[1].PartIndices[0]][queryResultDataLoader.Type.Members[1].PartIndices[1]] as GroupingEntry;
                                    (value as IGroupingDataRetriever).KeyValue = groupingEntry.GroupingKey.KeyPartsValues[0];
                                }
                                else if (groupingMetaData.KeyDynamicTypeDataRetrieve != null)
                                {
                                    (value as IGroupingDataRetriever).KeyValue = groupingMetaData.KeyDynamicTypeDataRetrieve.InstantiateObject(QueryResultLoaderEnumartor, QueryResultLoaderEnumartor.DataNodesRows);
                                }
                                else
                                {
                                    (value as IGroupingDataRetriever).KeyValue = QueryResultLoaderEnumartor.Current[QueryResult.Members[0].PartIndices[0]][QueryResult.Members[0].PartIndices[1]];
                                }


                                retValue = (ObjectType)value;
                                _Current = retValue;
                                return retValue;

                                //IEnumerator<TElement> _enum = new GroupedDataEnumerator<TElement>(groupingEntry.GroupedCompositeRows.GetEnumerator(), GroupedDataRetrieve, QueryResultDataLoader.GetEnumerator() as QueryResultLoaderEnumartor);

                            }

                            //IEnumerator<TElement> _enum = new GroupedDataEnumerator<TElement>(groupingEntry.GroupedCompositeRows.GetEnumerator(), GroupedDataRetrieve, QueryResultDataLoader.GetEnumerator() as QueryResultLoaderEnumartor);
                            //_enum.Reset();
                            //return _enum;

                            if (QueryResultLoaderEnumartor.Current[QueryResult.ConventionTypeRowIndex] == null)
                            {
                                _Current = default(ObjectType);
                                return default(ObjectType);
                            }

                            if (QueryResultLoaderEnumartor.Current[QueryResult.ConventionTypeRowIndex] != null)
                                value = QueryResultLoaderEnumartor.Current[QueryResult.ConventionTypeRowIndex][QueryResult.ConventionTypeColumnIndex];
                            if (typeof(ObjectType).GetMetaData().IsValueType && value != null && !(value is System.DBNull))
                                value = System.Convert.ChangeType(value, typeof(ObjectType));

                            if (value == null || value is System.DBNull)
                                retValue = default(ObjectType);
                            else
                            {






                                retValue = (ObjectType)value;
                            }
                            _Current = retValue;
                            return retValue;
                        }
                    }
                }
                finally
                {
                    RetrieveCurrent = false;
                }
            }
        }

        /// <MetaDataID>{b1d7e216-27c3-4f53-be4a-a8e9c0f4a3cd}</MetaDataID>
        private void LoadDynamicTypePropertiesValues(QueryResultLoaderEnumartor queryResultDataLoader, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows parentsdDataNodesRows)
        {
            var dataNodesRows = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodesRows(queryResultDataLoader.Current, QueryResult.DataNodeRowIndices, parentsdDataNodesRows);
            foreach (KeyValuePair<object, DynamicTypeProperty> entry in _Properties)
            {
                entry.Value.LoadPropertyValue(queryResultDataLoader, dataNodesRows);
            }
        }




        /// <MetaDataID>{a180cd0a-d8f3-47d6-a0f3-90397d82b273}</MetaDataID>
        public IEnumerator<ObjectType> GetEnumerator()
        {
            return new DynamicTypeDataRetrieve<ObjectType>(this);
        }

        #endregion

        #region IEnumerator Members

        /// <MetaDataID>{b3779b1a-78ff-4e79-8921-08af15f7b32f}</MetaDataID>
        object System.Collections.IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        /// <MetaDataID>{d4769068-cf81-4b4b-9352-dd7dd2fc3a1d}</MetaDataID>
        System.Collections.IEnumerator RealEnum;

        /// <MetaDataID>{2f475d3a-cfe5-4e96-a519-6f3472a6e12e}</MetaDataID>
        bool RetrieveCurrent;

        /// <MetaDataID>{33e7b163-b7e9-4fbe-aaaa-3a9395025d44}</MetaDataID>
        public void Reset()
        {
            if (RealEnum == null)
                RealEnum = QueryResultDataLoader.GetEnumerator();
            QueryResultLoaderEnumartor = RealEnum as QueryResultLoaderEnumartor;

        }


        /// <MetaDataID>{2b6def38-2a41-4a56-a649-cd8b9412768f}</MetaDataID>
        public bool MoveNext()
        {

            if (RealEnum == null)
                RealEnum = QueryResultDataLoader.GetEnumerator();
            //bool retval = RealEnum.MoveNext();
            QueryResultLoaderEnumartor = RealEnum as QueryResultLoaderEnumartor;
            bool retval = QueryResultLoaderEnumartor.MoveNext();
            _Current = default(ObjectType);
            return retval;
            //RetrieveCurrent = true;
            //_Current = default(ObjectType);
            //if (retval)
            //{
            //    if (RealEnum.Current is System.Data.DataRow[])
            //        CurrentCompositeRow = RealEnum.Current as System.Data.DataRow[];
            //    else
            //    {
            //        if (CurrentCompositeRow == null)
            //        {
            //            CurrentCompositeRow = new System.Data.DataRow[1];
            //            System.Data.DataTable temporaryTable = new System.Data.DataTable(RootDataNode.Alias);
            //            temporaryTable.Columns.Add("Key", typeof(object));
            //            CurrentCompositeRow[0] = temporaryTable.NewRow();
            //            ConventionTypeRowIndex = 0;
            //            ConventionTypeColumnIndex = 0;
            //        }
            //        System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>> entry = (System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>>)RealEnum.Current;
            //        CurrentCompositeRow[0][0] = entry.Key;
            //    }

            //}
            //while (retval && Current == null)
            //{
            //    retval = RealEnum.MoveNext();
            //    if (retval)
            //    {
            //        if (RealEnum.Current is System.Data.DataRow[])
            //            CurrentCompositeRow = RealEnum.Current as System.Data.DataRow[];
            //        else
            //        {
            //            if (CurrentCompositeRow == null)
            //            {
            //                CurrentCompositeRow = new System.Data.DataRow[1];
            //                System.Data.DataTable temporaryTable = new System.Data.DataTable(RootDataNode.Alias);
            //                temporaryTable.Columns.Add("Key", typeof(object));
            //                CurrentCompositeRow[0] = temporaryTable.NewRow();
            //                ConventionTypeRowIndex = 0;
            //                ConventionTypeColumnIndex = 0;
            //            }
            //            System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>> entry = (System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>>)RealEnum.Current;
            //            CurrentCompositeRow[0][0] = entry.Key;
            //        }
            //    }
            //}

            //return retval;
        }


        //public void Reset()
        //{
        //    //if (CompositeRows == null)
        //    //{
        //    //    DataNodesRows parentsdDataNodesRows = new DataNodesRows(null, new Dictionary<DataNode, int>());

        //    //    CompositeRows = GetAllDataAsSingleTable(null, parentsdDataNodesRows);
        //    //    RealEnum = CompositeRows.GetEnumerator();
        //    //}
        //    //RealEnum = CompositeRows.GetEnumerator();
        //    QueryResultDataLoader.Reset();

        //}


        /// <MetaDataID>{7946c343-a2c9-4ee3-91ab-3b8f8b761423}</MetaDataID>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {

            return new DynamicTypeDataRetrieve<ObjectType>(this);
        }

        #endregion

        #region IDisposable Members

        /// <MetaDataID>{3eb02737-3a7d-4fd9-95bb-cc51a57fdd60}</MetaDataID>
        public void Dispose()
        {
            if (RealEnum is IDisposable)
                (RealEnum as IDisposable).Dispose();

        }

        #endregion



    }


    /// <MetaDataID>{12dc662b-0c90-469a-ade1-0dcdb767fa3c}</MetaDataID>
    public interface IDynamicGrouping<TKey, TElement>
    {
        /// <MetaDataID>{8e5fb038-907e-40fb-addf-d7f30bddcc51}</MetaDataID>
        TKey Key
        {
            get;
        }
        /// <MetaDataID>{7b688ac7-1e1a-484c-bc74-8a13b0a614b5}</MetaDataID>
        System.Linq.IGrouping<TKey, TElement> GroupedData
        {
            get;
        }
    }

    /// <MetaDataID>{d651d203-64b4-4b0b-b034-292432ca7d1f}</MetaDataID>
    class SelectedGroupedData<TKey, TElement>
    {
        public System.Linq.IGrouping<TKey, TElement> GroupedData { get; set; }


    }

}
