using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{b69c994c-8fb5-4138-aa9e-382a51728940}</MetaDataID>
    public class DataTableCollection : IDataTableCollection
    {
        DataSet DataSet;
        internal DataTableCollection(DataSet dataSet)
        {
            DataSet = dataSet;
        }

        List<IDataTable> DataTables = new List<IDataTable>();
        internal Dictionary<string, IDataTable> NamedDataTables = new Dictionary<string, IDataTable>();

        #region IDataTableCollection Members

        public IDataTable this[int index]
        {
            get { return DataTables[index]; }
        }

        public IDataTable this[string name]
        {
            get { return NamedDataTables[name]; }
        }

        public IDataTable Add()
        {
            throw new NotImplementedException();
        }

        public void Add(IDataTable table)
        {
            if (NamedDataTables.ContainsKey(table.TableName))
                throw new Exception(string.Format("Table '{0}' already exist", table.TableName));

            DataTables.Add(table);
            NamedDataTables[table.TableName] = table;
            (table as DataTable).DataSet = DataSet;
            //table.DataSet = DataSet;
        }

        public IDataTable Add(string name)
        {
            DataTable table = new DataTable(name);
            Add(table);
            return table;
        }

        public bool CanRemove(IDataTable table)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            DataTables.Clear();
            NamedDataTables.Clear();
        }

        public bool Contains(string name)
        {
            return NamedDataTables.ContainsKey(name);
        }

        public int IndexOf(IDataTable table)
        {
            return DataTables.IndexOf(table);
        }

        public int IndexOf(string tableName)
        {
            if (NamedDataTables.ContainsKey(tableName))
                return DataTables.IndexOf(NamedDataTables[tableName]);
            return -1;
        }

        public void Remove(IDataTable table)
        {
            NamedDataTables.Remove(table.TableName);
            DataTables.Remove(table);
            (table as DataTable).DataSet = null;
        }

        public void Remove(string tableName)
        {
            if (NamedDataTables.ContainsKey(tableName))
            {
                DataTables.Remove(NamedDataTables[tableName]);
                NamedDataTables.Remove(tableName);
            }
        }
        public void RemoveAt(int index)
        {
            var dataTable = DataTables[index];
            Remove(dataTable);
        }

        #endregion
    }
}
