using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq
{

    /// <MetaDataID>{f86bc8d6-cfc0-48a8-a4eb-c2a5b870b2c9}</MetaDataID>
    interface IGroupingDataRetriever
    {
        object KeyValue { get; set; }

    }
    /// <summary>
    /// Represents a collection of objects that have a common key. 
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key of the System.Linq.IGrouping<TKey,TElement>.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the values in the System.Linq.IGrouping<TKey,TElement>.
    /// </typeparam>
    internal class GroupingDataRetriever<TKey, TElement> : System.Linq.IGrouping<TKey, TElement>, IGroupingDataRetriever //ystem.Linq.IGrouping`2[<>f__AnonymousType2`4[AbstractionsAndPersistency.IProductPrice,System.String,System.Int32,System.Int32],AbstractionsAndPersistency.IOrderDetail]]'
    {

        /// <summary>
        /// This field defines dynamic type data retriever for values
        /// </summary>
        IDynamicTypeDataRetrieve GroupedDataRetrieve;

        /// <summary>
        /// This field defines the Data Loader which provides the data
        /// </summary>
        QueryResultDataLoader QueryResultDataLoader;

        
        /// <summary>
        /// Defines the instance constructor 
        /// </summary>
        
        /// <param name="groupedDataRetrieve">
        /// This parameter defines dynamic type data retriever for values
        /// </param>
        /// <param name="queryResultDataLoader">
        /// This parameter defines the Data Loader which provides the data
        /// </param>
        public GroupingDataRetriever(IDynamicTypeDataRetrieve groupedDataRetrieve, QueryResultDataLoader queryResultDataLoader)
        {
            GroupedDataRetrieve = groupedDataRetrieve;
            QueryResultDataLoader = queryResultDataLoader;
        }

        /// <exclude>Excluded</exclude>
        TKey _Key;


        /// <summary>
        /// Defines the key of group entry
        /// </summary>
        public TKey Key
        {
            get
            {
                return _Key;
                //GroupingEntry groupingEntry = QueryResultDataLoader.DataNodesRows.CompositeRow[QueryResultDataLoader.Type.Members[1].PartIndices[0]][QueryResultDataLoader.Type.Members[1].PartIndices[1]] as GroupingEntry;


                //var queryResultLoaderEnumartor = new QueryResultLoaderEnumartor(QueryResultDataLoader);
                //queryResultLoaderEnumartor.MoveNext();
                //return (TKey)KeyDynamicTypeDataRetrieve.InstantiateObject(queryResultLoaderEnumartor, QueryResultDataLoader.DataNodesRows);
            }
            set
            {
                _Key = value;
            }
        }

        public object KeyValue
        {
            get => _Key;
            set
            {
                _Key = (TKey)value;
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            GroupingEntry groupingEntry = QueryResultDataLoader.DataNodesRows.CompositeRow[QueryResultDataLoader.Type.Members[1].PartIndices[0]][QueryResultDataLoader.Type.Members[1].PartIndices[1]] as GroupingEntry;
            IEnumerator<TElement> _enum = new GroupedDataEnumerator<TElement>(groupingEntry.GroupedCompositeRows.GetEnumerator(), GroupedDataRetrieve, QueryResultDataLoader.GetEnumerator() as QueryResultLoaderEnumartor);
            _enum.Reset();
            return _enum;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    /// <summary>
    /// Defines the grouped data enumerator.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <MetaDataID>{91162e76-76d6-4019-9a0b-d37cecb26c1e}</MetaDataID>
    class GroupedDataEnumerator<TElement> : IEnumerator<TElement>
    {
        IDynamicTypeDataRetrieve GroupedDataRetrieve;
        QueryResultLoaderEnumartor QueryResultDataLoader;

        public GroupedDataEnumerator(System.Collections.Generic.IEnumerator<CompositeRowData> compositeRowsEnum, IDynamicTypeDataRetrieve groupedDataRetrieve, QueryResultLoaderEnumartor queryResultDataLoader)
        {
            GroupedDataRetrieve = groupedDataRetrieve;
            QueryResultDataLoader = queryResultDataLoader;
            CompositeRowsEnumA = compositeRowsEnum;
        }

        System.Collections.Generic.IEnumerator<CompositeRowData> CompositeRowsEnumA;
        #region IEnumerator<TElement> Members

        public TElement Current
        {
            get
            {
                var value = GroupedDataRetrieve.InstantiateObject(QueryResultDataLoader, QueryResultDataLoader.DataNodesRows);

                if (value is TElement)
                    return (TElement)value;
                else
                    return default(TElement);

            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }


        public bool MoveNext()
        {
            //return CompositeRowsEnum.MoveNext();
            return QueryResultDataLoader.MoveNext();

        }

        public void Reset()
        {
            QueryResultDataLoader.Reset();

        }

        #endregion
    }


}
