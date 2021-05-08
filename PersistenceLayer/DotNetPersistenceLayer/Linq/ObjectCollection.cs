//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Collections.ObjectModel;
//using System.Reflection;
//using System.Linq.Expressions;
//using System.Collections;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;



namespace OOAdvantech.Linq
{


    /// <MetaDataID>{5b180376-f092-4d0c-bb4d-fd3da0a3b9a0}</MetaDataID>
    public class ObjectCollection<TEntity> : IQueryable<TEntity>, IQueryProvider, IEnumerable<TEntity>, IQueryable, IEnumerable where TEntity : class
    {
        /// <MetaDataID>{7897bb94-c52e-4585-82fc-718088d53fa9}</MetaDataID>
        readonly Storage _dataContext;


        /// <MetaDataID>{0eb6fd65-2399-4055-abf9-42fbe3ef62c7}</MetaDataID>
        internal ObjectCollection(Storage dataContext)
        {
            _dataContext = dataContext;

        }
        TEntity ObjectSource;
        /// <MetaDataID>{0eb6fd65-2399-4055-abf9-42fbe3ef62c7}</MetaDataID>
        internal ObjectCollection(TEntity objectSource)
        {
            ObjectSource = objectSource;
        }
        IEnumerable<TEntity> CollectionSource;

        internal ObjectCollection(IEnumerable<TEntity> collectionSource)
        {
            CollectionSource = collectionSource;
        }

        
       // TEntity RootObject;

        //internal ObjectCollection(TEntity rootObject)
        //{
        //    RootObject=rootObject;

        //}


        /// <MetaDataID>{eaf34248-a232-4ac7-b4d8-dd7182e26013}</MetaDataID>
        public Storage DataContext
        {
            get { return _dataContext; }
        }



        /// <MetaDataID>{1f4e52e2-ada9-4ce9-a726-ac80a41f9fb8}</MetaDataID>
        public override string ToString()
        {
            return ("Table(" + typeof(TEntity).Name + ")");
        }

        #region IEnumerable<TEntity> Members

        /// <MetaDataID>{97de0cd5-70af-45c7-8be5-039140a30811}</MetaDataID>
        public IEnumerator<TEntity> GetEnumerator()
        {

            return ((IEnumerable<TEntity>)_dataContext.Provider.Execute(Expression.Constant(this))).GetEnumerator();


        }

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{960e757a-9da2-4e35-b15c-0d51ad6cf4ca}</MetaDataID>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        /// <MetaDataID>{394133f6-9835-47be-9553-b74d8b3aa74a}</MetaDataID>
        public System.Type ElementType
        {
            get { return typeof(TEntity); }
        }

        /// <MetaDataID>{814f2a65-cbd2-4eea-8a74-ec125b1aae88}</MetaDataID>
        public Expression Expression
        {
            get { return Expression.Constant(this); }
        }

        /// <MetaDataID>{b9b005ed-c5eb-451b-8450-79716f22bfe9}</MetaDataID>
        public IQueryProvider Provider
        {
            get { return this; }
        }

        #endregion

        #region IQueryProvider Members

        /// <MetaDataID>{5d3707c3-62cc-430a-8fce-6c7214f07b5c}</MetaDataID>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!typeof(IQueryable<TElement>).GetMetaData().IsAssignableFrom(expression.Type))
                throw new Exception("ExpectedQueryableArgument");
            if (CollectionSource != null)
                return new ObjectStorageQuery<TElement>(CollectionSource as IEnumerable, expression,typeof(TEntity));
            else
                return new ObjectStorageQuery<TElement>(_dataContext, expression);
            
                

        }
        /// <MetaDataID>{ebf5ec54-ce2b-4603-baf0-adf2e98dee5a}</MetaDataID>
        public IQueryable CreateQuery(Expression expression)
        {


            if (expression == null)
                throw new ArgumentNullException("expression");

            System.Type elementType = TypeHelper.GetElementType(expression.Type);
            
            System.Type type2 =
              typeof(IQueryable<>).MakeGenericType(new[] { elementType });
            if (!type2.GetMetaData().IsAssignableFrom(expression.Type))
                throw new Exception("ExpectedQueryableArgument");

            return (IQueryable)Activator.CreateInstance(
              typeof(ObjectStorageQuery<>).MakeGenericType(new[] { elementType }),
              new object[] { _dataContext, expression });
        }

        /// <MetaDataID>{495c11a3-62a6-4e0a-a3f4-3869a1391154}</MetaDataID>
        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)_dataContext.Provider.Execute(expression);
        }

        /// <MetaDataID>{f653da02-ef6f-4fb2-84f7-a3af86fde682}</MetaDataID>
        public object Execute(Expression expression)
        {
            return _dataContext.Provider.Execute(expression);
        }

        #endregion


    }
}
