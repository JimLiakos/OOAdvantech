using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{195ae621-fb6a-4327-be19-d6d871ec472d}</MetaDataID>
    public class DataRowCollection : IDataRowCollection
    {

        DataTable DataTable;
        internal DataRowCollection(DataTable table)
        {
            DataTable = table;
        }
        List<DataRow> _Rows = new List<DataRow>();
        #region IDataRowCollection Members

        public int Count
        {
            get
            {
                return _Rows.Count;
            }
        }

        public IDataRow this[int index]
        {
            get
            {
                return _Rows[index];
            }

        }

        public void Add(IDataRow row)
        {
            DataTable.OnRowAdded(row as DataRow);
            _Rows.Add(row as DataRow);
        }

        public IDataRow Add(params object[] values)
        {
            IDataRow row= new DataRow(values, DataTable);
            Add(row);
            return row;
        }

        public void Clear()
        {
            _Rows.Clear();
            DataTable.AllRowsRemoved();
        }

        public bool Contains(object key)
        {
            return DataTable.PrimaryKeyDataIndex.Contains(key);
        }

        public bool Contains(object[] keys)
        {
            return DataTable.PrimaryKeyDataIndex.Contains(keys);
        }

        public void CopyTo(Array ar, int index)
        {
            int k = 0;
            for (int i = index; i < _Rows.Count; i++)
                ar.SetValue(_Rows[i],k++);
        }

        public void CopyTo(IDataRow[] array, int index)
        {
            int k = 0;
            for (int i = index; i < _Rows.Count; i++)
                array.SetValue(_Rows[i], k++);
        }

        public IDataRow Find(object key)
        {
            return DataTable.PrimaryKeyDataIndex.Find(key);
        }

        public IDataRow Find(object[] keys)
        {

            return DataTable.PrimaryKeyDataIndex.Find(keys);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return _Rows.GetEnumerator();
        }

        public int IndexOf(IDataRow row)
        {
            return _Rows.IndexOf(row as DataRow);
        }

        public void InsertAt(IDataRow row, int pos)
        {
            DataTable.OnRowAdded(row as DataRow);
            _Rows.Insert(pos, row as DataRow);
            
        }

        public void Remove(IDataRow row)
        {
            _Rows.Remove(row as DataRow);
            DataTable.OnRowRemoved(row as DataRow);
        }

        public void RemoveAt(int index)
        {
            DataRow row = _Rows[index];
            _Rows.RemoveAt(index);
            DataTable.OnRowRemoved(row);
        }

        public IDataRow[] ToArray()
        {
            return _Rows.ToArray();
        }

        #endregion
    }
}
