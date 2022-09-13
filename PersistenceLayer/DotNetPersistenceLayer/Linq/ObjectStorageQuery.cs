
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{563f214f-0e18-429e-b05c-e69106e0cf16}</MetaDataID>
    internal sealed class ObjectStorageQuery<T> : IOrderedQueryable<T>, IQueryable<T>, IQueryProvider, IEnumerable<T>, IOrderedQueryable, IQueryable, IEnumerable
    {
        /// <MetaDataID>{2cfc0daa-054c-4918-86f7-e8ab3afb2cae}</MetaDataID>
        private readonly Storage context;
        /// <MetaDataID>{8a92c032-c4d6-40a0-a1ad-5d8771456f0a}</MetaDataID>
        private readonly Expression queryExpression;

        /// <MetaDataID>{8a57002b-ba81-4daf-a2c7-bb1d5e3a1b6f}</MetaDataID>
        public ObjectStorageQuery(Storage context, Expression expression)
        {
            this.context = context;
            queryExpression = expression;
        }
        /// <MetaDataID>{01b0947e-ba8e-480a-92a4-60ba96a12c31}</MetaDataID>
        System.Collections.IEnumerable RemoteObjectsCollection;
        /// <MetaDataID>{f95ec8d4-e100-4dac-a884-4e553ab789e5}</MetaDataID>
        public ObjectStorageQuery(System.Collections.IEnumerable remoteObjectsCollection, Expression expression, Type objectsCollectionType)
        {
            RemoteObjectsCollection = remoteObjectsCollection;
            queryExpression = expression;
            ObjectsCollectionType = objectsCollectionType;
        }
        Type ObjectsCollectionType;
         
        bool QueryExecuted=false;
        IDynamicTypeDataRetrieve QueryResult;

         static object masterLock=new object();

        /// <MetaDataID>{5449c451-f277-4729-8292-8d42ee516b06}</MetaDataID>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //lock (masterLock)
            {
                if (context != null)
                {
                    if (!QueryExecuted)
                    {
                        QueryResult = (IDynamicTypeDataRetrieve)context.Provider.Execute(queryExpression);
                        QueryExecuted = true;
                    }
                    return (IEnumerator<T>)QueryResult.GetEnumarator();
                }
                else
                {
                    OOAdvantech.Collections.Generic.List<object> objects = new OOAdvantech.Collections.Generic.List<object>();

                    foreach (var @object in RemoteObjectsCollection)
                        objects.Add(@object);

                    LINQStorageObjectQuery linqObjectQuery = new LINQStorageObjectQuery(queryExpression, objects, ObjectsCollectionType);


                    //queryOnRootObject.Execute();
                    linqObjectQuery.Execute();

                    //(queryOnRootObject.QueryResult as IEnumerator).MoveNext();
                    //return (TResult)(queryOnRootObject.QueryResult as IEnumerator).Current;
                    return (IEnumerator<T>)linqObjectQuery.QueryResult;
                } 
            }
        }
        /// <MetaDataID>{2a91bed8-d52e-4452-9fe5-91307660f94d}</MetaDataID>
        public IEnumerator<T> Execute(Expression query)
        {
            // LINQObjectQuery linqObjectQuery = new LINQObjectQuery(query);

            throw new NotImplementedException();
        }
        /// <MetaDataID>{009dcab7-9621-4881-bf76-589b24581bee}</MetaDataID>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (context != null)
            {
                if (!QueryExecuted)
                {
                    QueryResult = (IDynamicTypeDataRetrieve)context.Provider.Execute(queryExpression);
                    QueryExecuted = true;
                }
                return (IEnumerator)QueryResult;
            }
            else
            {
                return Execute(queryExpression);
            }


        }

        /// <MetaDataID>{d2afd304-d37a-43bd-a757-31a3b27a3320}</MetaDataID>
        IQueryable<TResult> IQueryProvider.CreateQuery<TResult>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!typeof(IQueryable<TResult>).GetMetaData().IsAssignableFrom(expression.Type))
                throw new Exception("ExpectedQueryableArgument");

            return new ObjectStorageQuery<TResult>(context, expression);
        }

        /// <MetaDataID>{3fb54835-a430-488b-812b-6f60c3ba0758}</MetaDataID>
        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            System.Type elementType = TypeHelper.GetElementType(expression.Type);
            System.Type type = typeof(IQueryable<>).MakeGenericType(new[] { elementType });
            if (!type.GetMetaData().IsAssignableFrom(expression.Type))
                throw new Exception("ExpectedQueryableArgument");

            return (IQueryable)Activator.CreateInstance(
              typeof(ObjectStorageQuery<>).MakeGenericType(new[] { elementType }),
              new object[] { context, expression });
        }

        /// <MetaDataID>{df560b82-9ce1-4646-9ff7-ab57d814023d}</MetaDataID>
        object IQueryProvider.Execute(Expression expression)
        {
            return context.Provider.Execute(expression);
        }

        /// <MetaDataID>{5eb03343-a537-4ac1-9253-3eba599844d8}</MetaDataID>
        TElement IQueryProvider.Execute<TElement>(Expression expression)
        {
            return (TElement)context.Provider.Execute(expression);
        }



        /// <MetaDataID>{1f85be03-59ce-476c-9c9a-d70469274262}</MetaDataID>
        System.Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        /// <MetaDataID>{f04f7da4-3c92-458d-af79-3be4d8022e1f}</MetaDataID>
        Expression IQueryable.Expression
        {
            get { return queryExpression; }
        }

        /// <MetaDataID>{daf90e4d-4549-4f40-a8f6-48e864a09db9}</MetaDataID>
        IQueryProvider IQueryable.Provider
        {
            get { return this; }
        }
    }
}
