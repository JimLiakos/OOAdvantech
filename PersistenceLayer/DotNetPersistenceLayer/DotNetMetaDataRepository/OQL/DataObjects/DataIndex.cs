using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{2ab58ae7-ce3e-43c2-b61c-4ef9ddf818e1}</MetaDataID>
    public class DataIndex
    {
        DataTable OwnerTable;
        bool CreateConstraints;
       internal List<DataColumn> DataColumns;

        Dictionary<MultiPartKey, List<DataRow>> KeysRows = new Dictionary<MultiPartKey, List<DataRow>>();
        internal DataIndex( List<DataColumn> dataColumns, bool createConstraints)
        {
            if (dataColumns.Count == 0)
                throw new System.Exception("No columns defined");

            DataTable ownerTable = dataColumns[0].DataTable;
            foreach (var column in dataColumns)
            {
                if (column.Table != ownerTable)
                    throw new Exception("Cannot create a Key from Columns that belong to different tables.");
            }
            DataColumns = dataColumns;
            OwnerTable = ownerTable;

            CreateConstraints = createConstraints;
            OwnerTable.RowAdded += new DataObjects.RowAdded(OwnerTable_RowAdded);
            OwnerTable.RowRemoved += new DataObjects.RowRemoved(OwnerTable_RowRemoved);
            OwnerTable.RowChanged += new DataObjects.RowChanged(OwnerTable_RowChanged);
            foreach (var row in OwnerTable.Rows)
                OwnerTable_RowAdded(row as DataRow);

        }


        internal bool IsDataIndexOf(IDataColumn[] parentColumns)
        {
            if (parentColumns.Length != DataColumns.Count)
            {
                return false;
            }
            else
            {
                foreach (IDataColumn dataColumn in parentColumns)
                {
                    if (!DataColumns.Contains(dataColumn as DataColumn))
                        return false;
                }
            }
            return true;
        }
       




        void OwnerTable_RowAdded(DataRow dataRow)
        {
            MultiPartKey key = GetKey(dataRow, DataColumns);
            List<DataRow> rows = null;
            if (!KeysRows.TryGetValue(key, out rows))
            {
                rows = new List<DataRow>();
                KeysRows[key] = rows;
            }

            //Column 'IDH, IDL' is constrained to be unique.  Value '1, 1' is already present.

            if (CreateConstraints && rows.Count != 0)
            {
                string columns = null;
                string values = null;
                foreach (var dataColumn in DataColumns)
                {
                    if (!string.IsNullOrEmpty(columns))
                        columns += ",";
                    if (!string.IsNullOrEmpty(values))
                        values += ",";

                    columns += dataColumn.ColumnName;
                    object value = dataRow[dataColumn.Ordinal];
                    if (value != null)
                        values += value.ToString();
                    else
                        values += "null";

                }

                throw new Exception(string.Format("Column '{0}' is constrained to be unique.  Value '{1}' is already present.", columns, values));
            }

            rows.Add(dataRow);
        }
        internal MultiPartKey GetKey(DataRow dataRow)
        {
            return GetKey(dataRow, DataColumns);
        }
        private MultiPartKey GetKey(DataRow dataRow, List<DataColumn> dataColumns)
        {
            object[] parts = new object[dataColumns.Count];

            int i = 0;
            foreach (var column in dataColumns)
                parts[i++] = dataRow[column.Ordinal];

            return new MultiPartKey(parts);
        }

        
        void OwnerTable_RowRemoved(DataRow dataRow)
        {
            MultiPartKey key = GetKey(dataRow, DataColumns);
            List<DataRow> rows = null;
            if (KeysRows.TryGetValue(key, out rows))
                rows.Remove(dataRow);
        }

        void OwnerTable_RowChanged(DataRow dataRow, object[] oldValues)
        {
            MultiPartKey oldValueskey = GetKey(oldValues, DataColumns);
            MultiPartKey key = GetKey(dataRow, DataColumns);
            if (key != oldValueskey)
            {
                List<DataRow> rows = null;
                if (KeysRows.TryGetValue(oldValueskey, out rows))
                    rows.Remove(dataRow);
                if (!KeysRows.TryGetValue(key, out rows))
                {
                    rows = new List<DataRow>();
                    KeysRows[key] = rows;
                }
                rows.Add(dataRow);
            }
        }

        private MultiPartKey GetKey(object[] oldValues, List<DataColumn> dataColumns)
        {
            object[] parts = new object[dataColumns.Count];

            int i = 0;
            foreach (var column in dataColumns)
                parts[i] = oldValues[column.Ordinal];

            return new MultiPartKey(parts);

        }
        internal DataRow Find(object key)
        {
            return Find(new object[1]{key});
        }

        internal bool Contains(object key)
        {
            return Contains(new object[1] { key });
        }
        internal bool Contains(object[] keys)
        {
            MultiPartKey key = GetKey(keys, DataColumns);
            List<DataRow> rows = null;
            if (!KeysRows.TryGetValue(key, out rows))
                return false;
            else
            {
                if (rows.Count == 0)
                    return false;
                return true;
            }
        }

        internal DataRow Find(object[] keys)
        {
            MultiPartKey key = GetKey(keys, DataColumns);
            List<DataRow> rows = null;
            if (!KeysRows.TryGetValue(key, out rows))
                return null;
            else
            {
                if (rows.Count == 0)
                    return null;
                return rows[0];
            }
        }

        internal void Remove()
        {
            throw new NotImplementedException();
        }

        public DataTable Table
        {
            get
            {
                return OwnerTable;
            }
        }

        internal IDataRow[] GetRows(MultiPartKey key)
        {
            List<DataRow> rows = null;
            if (KeysRows.TryGetValue(key, out rows))
            {
                return rows.ToArray();
            }
            else
                return new IDataRow[0];
            
        }
    }
}
