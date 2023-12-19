using System;
using System.Collections.Generic;

using System.Linq.Expressions;

using System.Reflection;
using System.Collections;
using System.IO;
using OOAdvantech.Linq.Translators;
using OOAdvantech;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.Remoting;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{c0b2a139-5a6e-4b52-8d99-3ba71a4d98a8}</MetaDataID>
    internal class StorageProvider : IDisposable// : IStorageProvider
    {

        /// <MetaDataID>{3bba6590-d4d9-438f-95f1-9cf602353383}</MetaDataID>
        private bool _isDisposed;
        /// <MetaDataID>{515e2325-4bc9-4113-9d99-927b4e79248d}</MetaDataID>
        private readonly Storage _dataContext;

        /// <MetaDataID>{af84a877-aac3-47a8-bf40-3feb80d89e27}</MetaDataID>
        public StorageProvider(Storage dataContext)
        {
            _dataContext = dataContext;
        }


        /// <MetaDataID>{fe3fb659-4ffa-43f2-8712-edbc289e8cd5}</MetaDataID>
        public TextWriter Log { get; set; }

        ///// <MetaDataID>{b104762f-258b-40ff-939a-357966be2c86}</MetaDataID>
        //public OOAdvantech.Collections.StructureSet Execute(string query)
        //{
        //    CheckDispose();

        //    if (string.IsNullOrEmpty(query))
        //        throw new ArgumentNullException("query");



        //    // execute
        //    throw new System.NotImplementedException();

        //}
        LINQStorageObjectQuery LinqObjectQuery;
        // bool QueryExecuted;
        /// <summary>
        /// This is very the query is executed and the results are mapped to objects
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <MetaDataID>{f38f2569-4b22-4afd-b888-338619757b65}</MetaDataID>
        public object Execute(Expression query)
        {


            //    return LinqObjectQuery.QueryResult.GetNewEnumarator();
            CheckDispose();
            try
            {
                LinqObjectQuery = new LINQStorageObjectQuery(query, _dataContext.ObjectStorage);
                LinqObjectQuery.Execute();

                //List<System.Data.DataRow[]> dataTable = linqObjectQuery.GetAllDataAsSigleTable();
                //if (ConstructorInfo == null)
                //{
                //    ConstructorInfo = linqObjectQuery.EnumerableType.GetConstructors()[0];
                //    FastConstructorInvoke = AccessorBuilder.GetConstructorInvoker(ConstructorInfo);
                //    parameters = ConstructorInfo.GetParameters();
                //}

                if (TypeHelper.FindIEnumerable(query.Type) == null)
                {
                    (LinqObjectQuery.QueryResult as IEnumerator).MoveNext();
                    return (LinqObjectQuery.QueryResult as IEnumerator).Current;
                }

                //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                //binaryFormatter.Serialize(memoryStream, LinqObjectQuery.QueryResultType);
                //memoryStream.Position = 0;
                //OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResult queryResultType = binaryFormatter.Deserialize(memoryStream) as OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResult;



                return LinqObjectQuery.QueryResult;// Activator.CreateInstance(typeof(OOAdvantech.Linq.BridgeEnumerator<>).MakeGenericType(linqObjectQuery.EnumerableType), dataTable, linqObjectQuery.ParamsMetadata);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{79e84422-c9e0-452d-86b9-b66e442d056d}</MetaDataID>
        System.Reflection.ConstructorInfo ConstructorInfo;
        /// <MetaDataID>{a7482889-097c-484c-a8c3-9e54fb1cefa9}</MetaDataID>
        AccessorBuilder.FastInvokeHandler FastConstructorInvoke;
        /// <MetaDataID>{439b96fd-c162-47e9-9108-770ef8d94dc1}</MetaDataID>
        System.Reflection.ParameterInfo[] parameters;





        //public string GetQueryText(Expression query)
        //{

        //    CheckDispose();

        //    LINQObjectQuery translator = new LINQObjectQuery(query,_dataContext);
        //    return "";
        //}



        /// <MetaDataID>{581b4cc0-e2e6-4a7a-b40a-546164717277}</MetaDataID>
        private void CheckDispose()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(typeof(StorageProvider).Name);
        }



        /// <MetaDataID>{33ac19d8-f583-410a-8b55-fc260dca3c13}</MetaDataID>
        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;


        }



    }




    /// <MetaDataID>{d5132975-2b46-4c7f-b28f-407cd67924e9}</MetaDataID>
    internal class ObjectValuesProvider : IDisposable// : IStorageProvider
    {

        /// <MetaDataID>{3bba6590-d4d9-438f-95f1-9cf602353383}</MetaDataID>
        private bool _isDisposed;
        /// <MetaDataID>{515e2325-4bc9-4113-9d99-927b4e79248d}</MetaDataID>
        private readonly object ObjectAsDataSource;



        public ObjectValuesProvider(object objectAsDataSource)
        {
            ObjectAsDataSource = objectAsDataSource;
        }


        /// <MetaDataID>{fe3fb659-4ffa-43f2-8712-edbc289e8cd5}</MetaDataID>
        public TextWriter Log { get; set; }

        ///// <MetaDataID>{b104762f-258b-40ff-939a-357966be2c86}</MetaDataID>
        //public OOAdvantech.Collections.StructureSet Execute(string query)
        //{
        //    CheckDispose();

        //    if (string.IsNullOrEmpty(query))
        //        throw new ArgumentNullException("query");



        //    // execute
        //    throw new System.NotImplementedException();

        //}

        /// <summary>
        /// This is very the query is executed and the results are mapped to objects
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <MetaDataID>{f38f2569-4b22-4afd-b888-338619757b65}</MetaDataID>
        public object Execute(Expression query)
        {
            CheckDispose();
            try
            {
                LINQStorageObjectQuery linqObjectQuery = new LINQStorageObjectQuery(query, default(PersistenceLayer.ObjectStorage));
                linqObjectQuery.Execute();
                if (TypeHelper.FindIEnumerable(query.Type) == null)
                {
                    (linqObjectQuery.QueryResult as IEnumerator).MoveNext();
                    return (linqObjectQuery.QueryResult as IEnumerator).Current;
                }
                return linqObjectQuery.QueryResult;// Activator.CreateInstance(typeof(OOAdvantech.Linq.BridgeEnumerator<>).MakeGenericType(linqObjectQuery.EnumerableType), dataTable, linqObjectQuery.ParamsMetadata);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{79e84422-c9e0-452d-86b9-b66e442d056d}</MetaDataID>
        System.Reflection.ConstructorInfo ConstructorInfo;
        /// <MetaDataID>{a7482889-097c-484c-a8c3-9e54fb1cefa9}</MetaDataID>
        AccessorBuilder.FastInvokeHandler FastConstructorInvoke;
        /// <MetaDataID>{439b96fd-c162-47e9-9108-770ef8d94dc1}</MetaDataID>
        System.Reflection.ParameterInfo[] parameters;





        //public string GetQueryText(Expression query)
        //{

        //    CheckDispose();

        //    LINQObjectQuery translator = new LINQObjectQuery(query,_dataContext);
        //    return "";
        //}



        /// <MetaDataID>{581b4cc0-e2e6-4a7a-b40a-546164717277}</MetaDataID>
        private void CheckDispose()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(typeof(StorageProvider).Name);
        }



        /// <MetaDataID>{33ac19d8-f583-410a-8b55-fc260dca3c13}</MetaDataID>
        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;


        }



    }
}

namespace System.Linq
{
    /// <MetaDataID>{68e9bf1c-b02e-42ba-86e6-ed167fb2f9d9}</MetaDataID>
    public static class OOAdvantechExtraOperators
    {
        /// <MetaDataID>{89e59bc2-4da3-49c2-b3ff-1d55ca39357a}</MetaDataID>
        public static bool Like(this string matchExpression, string pattern)
        {
            return false;
        }
        /// <MetaDataID>{632e8915-d691-42a8-8537-65fb6f2f723d}</MetaDataID>
        public static IQueryable<TSource> ExtendDerivedMembers<TSource>(this IQueryable<TSource> queryable)
        {
            Expression expression = ExtendExpressionDerivedMembers(queryable.Expression);
            return queryable;
        }

        /// <MetaDataID>{081b0b14-54ea-4d53-8cfd-c438276bba5f}</MetaDataID>
        private static Expression ExtendExpressionDerivedMembers(Expression expression)
        {
            if ((expression as MethodCallExpression).Method.Name == "SelectMany")
            {
                Expression selector = (expression as MethodCallExpression).Arguments[1];
                MemberExpression memberExpression = GetDerividedMemberAccess(selector);
                if (memberExpression != null)
                {

                }

            }
            else
            {
                return ExtendExpressionDerivedMembers((expression as MethodCallExpression).Arguments[0]);
            }
            return expression;
        }

        /// <MetaDataID>{94f90abf-084a-4c1e-bd5d-63b03842631a}</MetaDataID>
        private static System.Linq.Expressions.MemberExpression GetDerividedMemberAccess(Expression selector)
        {
            if (selector is UnaryExpression)
                selector = (selector as UnaryExpression).Operand;

            if (selector is LambdaExpression)
                selector = (selector as LambdaExpression).Body;
            while ((selector is System.Linq.Expressions.MemberExpression) && !((selector as System.Linq.Expressions.MemberExpression).Member.HasCustomAttribute(typeof(OOAdvantech.MetaDataRepository.DerivedMember))))
                selector = (selector as System.Linq.Expressions.MemberExpression).Expression;

            if (selector is System.Linq.Expressions.MemberExpression && ((selector as System.Linq.Expressions.MemberExpression).Member.HasCustomAttribute(typeof(OOAdvantech.MetaDataRepository.DerivedMember))))
                return selector as System.Linq.Expressions.MemberExpression;
            return null;
        }
        //
        // Summary:
        //     Filters a sequence of values based on a predicate.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable<T> to filter.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable<T> that contains elements from
        //     the input sequence that satisfy the condition.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     source or predicate is null.


        /// <MetaDataID>{26866b8e-8a80-4676-aec4-56e6248fb8da}</MetaDataID>
        public static bool ContainsAny<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> value)
        {
            return false;
        }
        /// <MetaDataID>{bd03e2f9-09ee-4f9b-800f-4985b0903e44}</MetaDataID>
        public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> value)
        {
            return false;
        }





        /// <MetaDataID>{dbeaaf44-253c-4b96-8398-a4b07f041a0d}</MetaDataID>
        public static IEnumerable<TResult> Recursive<TResult>(this IEnumerable source, int depth)
        {
            return default(IEnumerable<TResult>);
        }

        /// <MetaDataID>{b526ea3c-26ec-4369-b160-3ea0729577cb}</MetaDataID>
        public static TSource Item<TSource>(this IEnumerable<TSource> source)
        {
            return default(TSource);
        }
        /// <MetaDataID>{1a8bf461-2d73-4795-b7d7-25a65da1758b}</MetaDataID>
        public static TResult Fetching<TSource, TResult>(this TSource source, Expression<System.Func<TSource, TResult>> expression) where TResult : class
        {

            Exception serverException = null;
            try
            {
                var linqObjectQuery = new OOAdvantech.Linq.LINQStorageObjectQuery(expression, default(OOAdvantech.PersistenceLayer.ObjectStorage));
                Dictionary<string, List<string>> cachingMembers = GetCachingMembers(linqObjectQuery.DataTrees[0].SubDataNodes[0]);

                try
                {
                    if (cachingMembers.Count>0)
                        System.Runtime.Remoting.Messaging.CallContext.SetData("CachingMetadata", cachingMembers);

                    if (source!=null)
                    {
                        object returnValue = null;
                        var sourceExpression = (expression.Body as MethodCallExpression).Arguments[0];
                        if (sourceExpression is MethodCallExpression)
                        {
                            object[] args = new object[(sourceExpression as MethodCallExpression).Arguments.Count];
                            int i = 0;
                            foreach (var argument in (sourceExpression as MethodCallExpression).Arguments)
                            {
                                if (argument is MemberExpression)
                                {
                                    object value = GetValue(argument as MemberExpression);
                                    args[i]=value;
                                }
                                if (argument is ConstantExpression)
                                {
                                    args[i]=(argument as ConstantExpression).Value;
                                }
                                i++;
                            }
                            try
                            {
                                returnValue=(sourceExpression as MethodCallExpression).Method.Invoke(source, args);
                            }
                            catch (System.Reflection.TargetInvocationException error)
                            {

                                serverException= error.InnerException;
                                throw error.InnerException;
                            }
                            return returnValue as TResult;

                        }
                        else if (sourceExpression is MemberExpression)
                        {
                            if ((sourceExpression as  MemberExpression).Member is PropertyInfo)
                            {
                                returnValue=((sourceExpression as  MemberExpression).Member as PropertyInfo).GetValue(source);
                                return returnValue as TResult;
                            }
                            if ((sourceExpression as  MemberExpression).Member is FieldInfo)
                            {
                                returnValue=((sourceExpression as  MemberExpression).Member as FieldInfo).GetValue(source);
                                return returnValue as TResult;
                            }
                        }
                        else if (sourceExpression is ParameterExpression)
                        {
                            IProxy proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(source) as IProxy;
                            proxy?.FetchMembersValues(cachingMembers);

                            return source as TResult;

                        }
                    }
                }
                finally
                {
                    if (cachingMembers.Count>0)
                        System.Runtime.Remoting.Messaging.CallContext.FreeNamedDataSlot("CachingMetadata");
                }


            }
            catch (Exception error)
            {
            }

            if (serverException!=null)
                throw serverException;

            return default(TResult);

        }
        static Dictionary<string, List<string>> GetCachingMembers(DataNode dataNode, Dictionary<string, List<string>> cachingMemebers = null)
        {
            if (cachingMemebers==null)
                cachingMemebers = new Dictionary<string, List<string>>();
            if (dataNode.SubDataNodes.Count==0)
                return cachingMemebers;
            List<string> members = null;

            if (!cachingMemebers.TryGetValue(dataNode.Classifier.FullName, out members))
            {
                members = new List<string>();
                cachingMemebers[dataNode.Classifier.FullName]= members;
            }

            foreach (DataNode node in dataNode.SubDataNodes)
            {
                string memberName = node.AssignedMetaObject.Name;
                members.Add(memberName);
                if (dataNode.Type==DataNode.DataNodeType.Object)
                    GetCachingMembers(node, cachingMemebers);
            }

            return cachingMemebers;
        }

        private static object GetValue(MemberExpression member)
        {
            if (member.Expression is ConstantExpression)
            {
                return (member.Expression as ConstantExpression).Value.GetType().GetField(member.Member.Name).GetValue((member.Expression as ConstantExpression).Value);
            }

            return null;


        }

        public static TSource Caching<TSource>(this TSource source, Expression<System.Func<TSource, dynamic>> expression)
        {

            return default(TSource);
        }


        public static TSource Fetching<TSource>(this IEnumerable<TSource> source, params object[] list)
        {
            return default(TSource);
        }

        public static IQueryable<TSource> Partition<TSource>(this IQueryable<TSource> source, string partitionKey)
        {
            if (source is IQueryable<TSource>)
            {
                var method = typeof(OOAdvantechExtraOperators).GetMethod("Partition").MakeGenericMethod(typeof(TSource));
                var call = Expression.Call(method, (source as IQueryable<TSource>).Expression, Expression.Constant(partitionKey));

                var result = source.Provider.CreateQuery<TSource>(call);
                return result;
            }


            return source;
        }

        /// <MetaDataID>{77275ddc-2948-46a5-bcf6-a6eba420c700}</MetaDataID>
        public static TSource Fetching<TSource>(this TSource source, params object[] list)
        {
            return source;

        }


        /// <MetaDataID>{8b594487-bdd3-464b-bcfe-96310c6da258}</MetaDataID>
        public static TSource Refresh<TSource>(this IEnumerable<TSource> source, params object[] list)
        {
            return default(TSource);
        }

        /// <MetaDataID>{a753a591-2d94-4bb4-a1ef-80c9b0c8009e}</MetaDataID>
        public static TSource Refresh<TSource>(this TSource source, params object[] list)
        {
            return source;
        }

        //public static TSource Member<TSource>(this TSource source)
        //{
        //    return default(TSource);
        //}



    }
}