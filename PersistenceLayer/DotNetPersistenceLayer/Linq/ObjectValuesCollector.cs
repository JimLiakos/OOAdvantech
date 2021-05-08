using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Reflection;
using System.Collections;
#if PORTABLE 
using System.PCL.Reflection;
#else
using System.Reflection;
#endif

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{a1c04bcb-a9ac-425a-9a91-fdf76682d352}</MetaDataID>
    public class ObjectValuesCollector
    {
        /// <MetaDataID>{4132a187-d73e-4533-a4b9-3d59a5660a51}</MetaDataID>
        static ObjectValuesCollector()
        {
            OOAdvantech.ObjectsContext.Init();
        }

        /// <MetaDataID>{a698a7c1-789f-4e35-a8b7-74a003d83b24}</MetaDataID>
        public static ObjectCollection<TSource> GetFromObject<TSource>(TSource source) where TSource : class
        {

            ObjectCollection<TSource> collection = (ObjectCollection<TSource>)Activator.CreateInstance(
                              typeof(ObjectCollection<>).MakeGenericType(
                                new[] { typeof(TSource) }),
                                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                                new object[] { source }, null);
            return collection;

        }

#if !DeviceDotNet

        /// <MetaDataID>{bce3118e-0727-49b2-8593-4cd3b1fe81be}</MetaDataID>
        public static TResult GetObjectValues<TSource, TResult>(TSource source, System.Linq.Expressions.Expression<Func<TSource, TResult>> expression) where TSource : class
        {


            Translators.RetrieveObjectMemberTranslator translator = new Translators.RetrieveObjectMemberTranslator(typeof(TSource));
            System.Linq.Expressions.Expression[] arguments = new System.Linq.Expressions.Expression[2];
            Dictionary<string, Type> fields=new Dictionary<string,Type>(){{"<>f__AnonymousType_Data",typeof(TResult)}};
            Type selectResultType = Translators.LinqRuntimeTypeBuilder.GetDynamicType(fields);

            var selectorBody = System.Linq.Expressions.Expression.New(selectResultType.GetMetaData().GetConstructors()[0], Translators.ExpressionVisitor.GetLambdaExpression(expression).Body);
             var selectorBodyLambda=  System.Linq.Expressions.Expression.Lambda(selectorBody, Translators.ExpressionVisitor.GetLambdaExpression(expression).Parameters.ToArray());
            //Type selectionNewType=ObjectValues
            arguments[0] = System.Linq.Expressions.Expression.Constant(new ObjectCollection<TSource>(source));
            arguments[1] = System.Linq.Expressions.Expression.Quote(selectorBodyLambda); ;

            var methodInfo = Translators.LinqRuntimeTypeBuilder.GetMethod("Select", ref arguments);

            //arguments[0] = System.Linq.Expressions.Expression.Constant(source);
            //arguments[1] = expression;
            //var methodInfo = Translators.LinqRuntimeTypeBuilder.GetMethod("GetObjectValues", ref arguments);
            //System.Linq.Expressions.Expression[] arguments = new System.Linq.Expressions.Expression[2];
            //arguments[0] = System.Linq.Expressions.Expression.Constant(source);
            //arguments[1] = expression;
            //MethodInfo GetObjectValuesMethod=null;
            //foreach (var methodInfo in typeof(ObjectValuesCollector).GetMethods())
            //{
            //    if (methodInfo.Name == "GetObjectValues" && methodInfo.GetParameters().Length == 2)
            //    {
            //        GetObjectValuesMethod = methodInfo;
            //        break;
            //    }
            //}

            System.Linq.Expressions.Expression objectValuesColectorExpression = System.Linq.Expressions.Expression.Call(methodInfo, arguments);



            LINQStorageObjectQuery linqObjectQuery = new LINQStorageObjectQuery(objectValuesColectorExpression, new OOAdvantech.Collections.Generic.List<object> { source }, typeof(TSource));
            //LinqQueryOnRootObject queryOnRootObject = new LinqQueryOnRootObject(objectValuesColectorExpression, source);

            //queryOnRootObject.Execute();
            linqObjectQuery.Execute();

            //(queryOnRootObject.QueryResult as IEnumerator).MoveNext();
            //return (TResult)(queryOnRootObject.QueryResult as IEnumerator).Current;
            (linqObjectQuery.QueryResult as IEnumerator).MoveNext();

            return (TResult)selectResultType.GetMetaData().GetProperties()[0].GetValue((linqObjectQuery.QueryResult as IEnumerator).Current,null);





            //DataNode rootDataNode = queryOnRootObject.Translator.RootPaths[0];
            //OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
            //GetPaths(rootDataNode, ref paths);
            //OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(source as MarshalByRefObject, paths);



            //System.Reflection.ParameterInfo[] parametersInfo = typeof(TResult).GetConstructors()[0].GetParameters();
            //AccessorBuilder.FastInvokeHandler fastConstructor = AccessorBuilder.GetConstructorInvoker(typeof(TResult).GetConstructors()[0]);
            //object[] _params = new object[parametersInfo.Length];
            //foreach (OOAdvantech.Collections.StructureSet ss in structureSet)
            //{
            //    List<string> memberNames = new List<string>();
            //    //foreach (string memberName in ss.Members.Members.Keys)
            //    //{
            //    //    memberNames.Add(memberName);

            //    //}
            //    //foreach (DataNode dataNode in translator.TypesDataNodes[typeof(TResult)])
            //    //{
            //    int i = 0;
            //    //    foreach (System.Reflection.ParameterInfo parameterInfo in parametersInfo)
            //    //    {

            //    //        if (parameterInfo.Name == dataNode.Alias)
            //    //        {
            //    //            if (TypeHelper.FindIEnumerable(parameterInfo.ParameterType) == null)
            //    //            {
            //    //                object value = ss[dataNode.FullName.Replace(".", "_")];
            //    //                _params[i] = value;

            //    //            }
            //    //            else
            //    //            {
            //    //                if (dataNode.ParentDataNode == rootDataNode)
            //    //                {

            //    //                    object value = GetCollection(parameterInfo.ParameterType, translator.TypesDataNodes, ss[dataNode.Name] as StructureSet, dataNode, dataNode);
            //    //                    _params[i] = value;
            //    //                }

            //    //                else
            //    //                {
            //    //                    DataNode structureSetDataNode = dataNode;
            //    //                    while (structureSetDataNode.ParentDataNode != rootDataNode)
            //    //                        structureSetDataNode = structureSetDataNode.ParentDataNode;
            //    //                    object value = GetCollection(parameterInfo.ParameterType, translator.TypesDataNodes, ss[structureSetDataNode.Name] as StructureSet, structureSetDataNode, dataNode);
            //    //                    _params[i] = value;

            //    //                }
            //    //            }
            //    //        }
            //    //        i++;
            //    //    }

            //    //}
            //    //  return (TResult)fastConstructor.Invoke(null, _params);


            //}

            //return default(TResult);
        }

#endif
        /// <MetaDataID>{fc846f51-3265-4c15-b2fb-8b34cd06b560}</MetaDataID>
        public static ObjectCollection<TSource> GetObjectValues<TSource>(IEnumerable<TSource> source) where TSource : class
        {

            ObjectCollection<TSource> collection = new ObjectCollection<TSource>(source);
            return collection;
        }
        ///// <MetaDataID>{b19b9f6b-ba85-4059-9a5a-6772d1c33238}</MetaDataID>
        //public static IEnumerable<TResult> GetObjectValues<TSource, TResult>(IEnumerable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TResult>> expression) where TSource : class
        //{


        //    System.Linq.Expressions.Expression[] arguments = new System.Linq.Expressions.Expression[2];

        //    arguments[0] = System.Linq.Expressions.Expression.Constant(new ObjectCollection<TSource>(source));
        //    arguments[1] = expression;
        //    var methodInfo = Translators.LinqRuntimeTypeBuilder.GetMethod("Select", ref arguments);
        //    System.Linq.Expressions.Expression objectValuesColectorExpression = System.Linq.Expressions.Expression.Call(methodInfo, arguments);



        //    LINQStorageObjectQuery linqObjectQuery = new LINQStorageObjectQuery(objectValuesColectorExpression, new OOAdvantech.Collections.Generic.List<object> { source }, typeof(TSource));
        //    LinqQueryOnRootObject queryOnRootObject = new LinqQueryOnRootObject(objectValuesColectorExpression, source);

        //    //queryOnRootObject.Execute();
        //    linqObjectQuery.Execute();

        //    //(queryOnRootObject.QueryResult as IEnumerator).MoveNext();
        //    //return (TResult)(queryOnRootObject.QueryResult as IEnumerator).Current;
        //    return linqObjectQuery.QueryResult ;
        //    //(linqObjectQuery.QueryResult as IEnumerator).MoveNext();
        //    //return (TResult)(linqObjectQuery.QueryResult as IEnumerator).Current;




        //    List<TResult> resultCollection = new List<TResult>();
        //    Translators.RetrieveObjectMemberTranslator translator = new Translators.RetrieveObjectMemberTranslator(typeof(TSource));
        //    translator.Translate(expression);

        //    DataNode rootDataNode = translator.RootPaths[0];

        //    // LINQObjectQuery linqObjectQuery = new LINQObjectQuery(expression, translator);
        //    OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
        //    GetPaths(rootDataNode, ref paths);
        //    OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(source as System.Collections.IEnumerable, typeof(TSource), paths);



        //    System.Reflection.ParameterInfo[] parametersInfo = typeof(TResult).GetConstructors()[0].GetParameters();
        //    AccessorBuilder.FastInvokeHandler fastConstructor = AccessorBuilder.GetConstructorInvoker(typeof(TResult).GetConstructors()[0]);
        //    object[] _params = new object[parametersInfo.Length];
        //    foreach (OOAdvantech.Collections.StructureSet ss in structureSet)
        //    {
        //        foreach (DataNode dataNode in translator.TypesDataNodes[typeof(TResult)])
        //        {
        //            int i = 0;
        //            foreach (System.Reflection.ParameterInfo parameterInfo in parametersInfo)
        //            {

        //                if (parameterInfo.Name == dataNode.Alias)
        //                {
        //                    if (TypeHelper.FindIEnumerable(parameterInfo.ParameterType) == null)
        //                    {
        //                        object value = ss[dataNode.FullName.Replace(".", "_")];
        //                        if (value == null)
        //                            value = AccessorBuilder.GetDefaultValue(parameterInfo.ParameterType);
        //                        if (value is System.DBNull)
        //                            value = AccessorBuilder.GetDefaultValue(parameterInfo.ParameterType);

        //                        _params[i] = value;

        //                    }
        //                    else
        //                    {
        //                        if (dataNode.ParentDataNode == rootDataNode)
        //                        {

        //                            object value = GetCollection(parameterInfo.ParameterType, translator.TypesDataNodes, ss[dataNode.Name] as StructureSet, dataNode, dataNode);
        //                            if (value is System.DBNull)
        //                                value = AccessorBuilder.GetDefaultValue(parameterInfo.ParameterType);
        //                            _params[i] = value;
        //                        }

        //                        else
        //                        {
        //                            DataNode structureSetDataNode = dataNode;
        //                            while (structureSetDataNode.ParentDataNode != rootDataNode)
        //                                structureSetDataNode = structureSetDataNode.ParentDataNode;
        //                            object value = GetCollection(parameterInfo.ParameterType, translator.TypesDataNodes, ss[structureSetDataNode.Name] as StructureSet, structureSetDataNode, dataNode);
        //                            if (value is System.DBNull)
        //                                value = AccessorBuilder.GetDefaultValue(parameterInfo.ParameterType);

        //                            _params[i] = value;

        //                        }


        //                    }

        //                }
        //                i++;

        //            }

        //        }
        //        resultCollection.Add((TResult)fastConstructor.Invoke(null, _params));


        //    }

        //    return resultCollection;
        //}

        /// <MetaDataID>{b4598227-0e51-4a7a-8e4f-61c01eda7a50}</MetaDataID>
        private static object GetCollection(Type seqType, Dictionary<Type, List<DataNode>> typesDataNodes, StructureSet structureSet, DataNode structureSetDataNode, DataNode collectionDataNode)
        {
            //OOAdvantech.Collections.Generic.Set<object> 
            System.Type itemType = TypeHelper.GetElementType(seqType);
            AccessorBuilder.FastInvokeHandler fastConstructor = null;
            System.Collections.IList collection = null;
            if (itemType.Name.IndexOf("<>f__AnonymousType") == 0)
            {
                fastConstructor = AccessorBuilder.GetConstructorInvoker(itemType.GetMetaData().GetConstructors()[0]);
                collection = System.Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType)) as System.Collections.IList;
            }
            else
                collection = System.Activator.CreateInstance(seqType) as System.Collections.IList;

            if (structureSetDataNode != collectionDataNode)
            {
                DataNode nextStructureSetDataNode = collectionDataNode;
                while (nextStructureSetDataNode.ParentDataNode != structureSetDataNode)
                    nextStructureSetDataNode = nextStructureSetDataNode.ParentDataNode;

                foreach (StructureSet instanceSet in structureSet)
                {
                    object value = GetCollection(seqType, typesDataNodes, instanceSet[nextStructureSetDataNode.Name] as StructureSet, nextStructureSetDataNode, collectionDataNode);
                    foreach (object item in (value as System.Collections.IList))
                        collection.Add(item);
                }
                return collection;
            }
            else
            {
                if (itemType.Name.IndexOf("<>f__AnonymousType") == 0)
                {
                    System.Reflection.ParameterInfo[] parametersInfo = itemType.GetMetaData().GetConstructors()[0].GetParameters();
                    object[] _params = new object[parametersInfo.Length];


                    foreach (StructureSet instanceSet in structureSet)
                    {
                        foreach (DataNode dataNode in typesDataNodes[itemType])
                        {
                            int i = 0;
                            foreach (System.Reflection.ParameterInfo parameterInfo in parametersInfo)
                            {
                                if (parameterInfo.Name == dataNode.Alias)
                                {
                                    if (TypeHelper.FindIEnumerable(parameterInfo.ParameterType) == null)
                                    {
                                        object value = instanceSet[dataNode.FullName.Replace(".", "_")];
                                        if (value == null)
                                            value = AccessorBuilder.GetDefaultValue(parameterInfo.ParameterType);
                                        _params[i] = value;

                                    }
                                    else
                                    {
                                        if (dataNode.ParentDataNode == collectionDataNode)
                                        {
                                            object value = GetCollection(parameterInfo.ParameterType, typesDataNodes, instanceSet[dataNode.Name] as StructureSet, dataNode, dataNode);
                                            _params[i] = value;
                                        }
                                        else
                                        {
                                            structureSetDataNode = dataNode;
                                            while (structureSetDataNode.ParentDataNode != collectionDataNode)
                                                structureSetDataNode = structureSetDataNode.ParentDataNode;
                                            object value = GetCollection(parameterInfo.ParameterType, typesDataNodes, instanceSet[structureSetDataNode.Name] as StructureSet, structureSetDataNode, dataNode);
                                            _params[i] = value;
                                        }
                                    }
                                }
                                i++;
                            }
                        }
                        collection.Add(fastConstructor.Invoke(null, _params));
                    }
                }
                else
                {
                    foreach (StructureSet instanceSet in structureSet)
                        collection.Add(instanceSet["Object"]);
                }

                return collection;
            }
        }
        /// <MetaDataID>{6d7b7e5f-e586-43a7-a8ac-0a708a4f4232}</MetaDataID>
        static void GetPaths(DataNode dataNode, ref OOAdvantech.Collections.Generic.List<string> paths)
        {
            if (dataNode.SubDataNodes.Count == 0)
                paths.Add(dataNode.FullName);
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                GetPaths(subDataNode, ref paths);
            }
        }

    }


    ///// <MetaDataID>{06f9ec5c-a076-4ed8-ba54-9407e6b9ea8e}</MetaDataID>
    //class LinqQueryOnRootObject : QueryOnRootObject, ILINQObjectQuery
    //{
    //    public OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery ObjectQuery { get { return this; } }
    //    /// <MetaDataID>{91633159-8c5a-4a2d-b371-3f28fa43c215}</MetaDataID>
    //    internal OOAdvantech.Linq.Translators.QueryTranslator _Translator;

    //    /// <MetaDataID>{3a3d2731-dc4e-4531-b250-d02946f7e96f}</MetaDataID>
    //    public LinqQueryOnRootObject(System.Linq.Expressions.Expression query, object rootObject)
    //        : base(rootObject)
    //    {

    //        _Translator = new OOAdvantech.Linq.Translators.QueryTranslator(this);
    //        _Translator.Translate(query);

    //        QueryResult.ParticipateInQueryResults(null);
    //        foreach (QueryExpressions.FetchingExpressionTreeNode fetchingExpression in _Translator.FetchingExpressions)
    //            fetchingExpression.ParticipateInSelectList();

    //        //foreach (DataNode dataNode in _Translator.RootPaths)
    //        //    DataTrees.Add(dataNode);

    //        //string errors = null;
    //        //BuildDataNodeTree(ref errors);
    //        //_Translator.BuildSearchCondition();
    //        //foreach (DataNode dataNode in DataTrees)
    //        //    dataNode.MergeSearchConditions();
    //        DataTrees[0].AddSearchCondition(null);

    //        var tt = DataTrees[0].BranchSearchConditions;
    //    }

    //    #region ILINQObjectQuery Members

    //    IDynamicTypeDataRetrieve _QueryResult;
    //    public IDynamicTypeDataRetrieve QueryResult
    //    {
    //        get
    //        {
    //            return _QueryResult;
    //        }
    //        set
    //        {
    //            _QueryResult = value;
    //        }
    //    }

    //    public OOAdvantech.Linq.Translators.QueryTranslator Translator
    //    {
    //        get { return _Translator; }
    //    }

    //    #endregion

    //    internal void Execute()
    //    {

    //        OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
    //        foreach (DataNode dataNode in DataTrees)
    //            dataTrees.AddRange(dataNode.RemoveNamespacesDataNodes());
    //        DataTrees = dataTrees;
    //        BuildDataSources();
    //        Distribute();
    //        LoadData();

    //        //foreach (DataNode dataNode in DataTrees)
    //        //    BuildEmptyDataSource(dataNode);
    //    }


    //}
}
