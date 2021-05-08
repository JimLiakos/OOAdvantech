using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{5acc2b9b-a870-45bb-9258-168615308631}</MetaDataID>
    public class DataReader : IDataReader
    {
        #region IDataReader Members

        System.Collections.IEnumerator Enumerator;
        DataTable DataTable;
        public DataReader(DataTable dataTable )
        {
            DataTable = dataTable;
            Enumerator = DataTable.Rows.GetEnumerator(); ;
        }
        public bool Read()
        {
            return Enumerator.MoveNext();
            
        }

        public int GetValues(object[] values)
        {
            object[] currentRowValues=(Enumerator.Current as DataRow).ItemArray;
            for (int i = 0; i != currentRowValues.Length; i++)
                values[i] = currentRowValues[i];
            return currentRowValues.Length;

        }

        public int GetOrdinal(string name)
        {
            return DataTable.Columns[name].Ordinal;
        }

        public string GetName(int index)
        {
            return DataTable.Columns[index].ColumnName;
        }

        public int FieldCount
        {
            get { return DataTable.Columns.Count; }
        }

        public Type GetFieldType(int index)
        {
            return DataTable.Columns[index].DataType;
        }

        #endregion

        #region IDataReader Members


        public object this[int ordinal]
        {
            get 
            {
                return (Enumerator.Current as DataRow)[ordinal];
            }
        }


        public object this[string name]
        {
            get { return (Enumerator.Current as DataRow)[name]; }
        }

        #endregion

        #region IDataReader Members


        public void Close()
        {
            
        }

        #endregion

        #region IDataReader Members


        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
