using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{1429970e-47fb-4666-b276-72d6377e8976}</MetaDataID>
    public class DataColumnCollection : IDataColumnCollection
    {
        List<IDataColumn> DataColumns = new List<IDataColumn>();
        internal Dictionary<string, IDataColumn> NamedDataColumns = new Dictionary<string, IDataColumn>();

        DataTable DataTable;
        internal DataColumnCollection(DataTable dataTable)
        {
            DataTable = dataTable;
        }

        #region IDataColumnCollection Members

        public int Count
        {
            get
            {
                return DataColumns.Count;
            }
        }

        public IDataColumn this[int index]
        {
            get { return DataColumns[index]; }
        }

        public IDataColumn this[string name]
        {
            get { return NamedDataColumns[name]; }
        }

        public IDataColumn Add()
        {
            throw new NotImplementedException();
        }

        public void Add(IDataColumn dataColumn)
        {
            if (NamedDataColumns.ContainsKey(dataColumn.ColumnName))
                throw new Exception(string.Format("Column '{0}' already exist", dataColumn.ColumnName));

            DataColumns.Add(dataColumn);
            NamedDataColumns[dataColumn.ColumnName] = dataColumn;
            (dataColumn as DataColumn).ColumnContainer = this;
            (dataColumn as DataColumn).DataTable = DataTable;
            

        }

        public IDataColumn Add(string columnName)
        {

            DataColumn dataColumn = new DataColumn(columnName,DataTable);
            Add(dataColumn);
            return dataColumn;

        }

        public IDataColumn Add(string columnName, Type dataType)
        {
            DataColumn dataColumn = new DataColumn(columnName, dataType, DataTable);
            Add(dataColumn);
            return dataColumn;

        }

        public void AddRange(IDataColumn[] columns)
        {
            foreach (var dataColumn in columns)
            {
                (dataColumn as DataColumn).DataTable = DataTable;
                Add(dataColumn);
            }
        }

        public bool CanRemove(IDataColumn column)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {

            foreach (var column in DataColumns)
                (column as DataColumn).ColumnContainer = null;

            DataColumns.Clear();
            NamedDataColumns.Clear();
        }

        public bool Contains(string name)
        {
            return NamedDataColumns.ContainsKey(name);
        }

        public int IndexOf(IDataColumn column)
        {
            return DataColumns.IndexOf(column);
        }

        public int IndexOf(string columnName)
        {
            IDataColumn column=null;
            if (NamedDataColumns.TryGetValue(columnName, out column))
                return DataColumns.IndexOf(column);

            return -1;
        }

        public void Remove(IDataColumn column)
        {
            if (DataColumns.Contains(column))
            {
                DataColumns.Remove(column);
                NamedDataColumns.Remove(column.ColumnName);
            }
            
        }

        public void Remove(string name)
        {
            IDataColumn column = null;
            if (NamedDataColumns.TryGetValue(name, out column))
                Remove(column);
        }

        public void RemoveAt(int index)
        {
            IDataColumn column = DataColumns[index];
            Remove(column);

        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return DataColumns .GetEnumerator();
        }

        #endregion
    }
}
