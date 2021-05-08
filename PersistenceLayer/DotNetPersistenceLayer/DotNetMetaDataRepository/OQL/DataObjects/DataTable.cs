using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{

    delegate void RowAdded(DataRow dataRow);
    delegate void RowRemoved(DataRow dataRow);
    delegate void RowChanged(DataRow dataRow, object[] oldValues);



    /// <MetaDataID>{84fc6b0a-dad7-4d4e-9f02-686824b19786}</MetaDataID>
    public class DataTable : IDataTable
    {

        #region IDataTable Members

        
        Dictionary<string, object> _ExtendedProperties;
        Dictionary<string, object> IDataTable.ExtendedProperties
        {
            get
            {
                if (_ExtendedProperties == null)
                    _ExtendedProperties = new Dictionary<string, object>();
                return _ExtendedProperties;
            }
        }

        bool _TemporaryTableTransfered;
        public bool TemporaryTableTransfered
        {
            get
            {
                return _TemporaryTableTransfered;
            }
            set
            {
                _TemporaryTableTransfered = value;
            }
        }

        IDataColumnCollection _Columns; 

        public IDataColumnCollection Columns
        {
            get 
            {
                
                return _Columns;
            }
        }
        

        DataSource _OwnerDataSource;
        public DataSource OwnerDataSource
        {
            get
            {
                return _OwnerDataSource;
            }
            set
            {
                _OwnerDataSource = value;
            }
        }

        IDataRowCollection _Rows;
        public IDataRowCollection Rows
        {
            get 
            {
                if (_Rows == null)
                    _Rows = new DataRowCollection(this);

                return _Rows;
            }
        }

        bool _FilteredTable;
        public bool FilteredTable 
        {
            get
            {
                return _FilteredTable;
            }
            set
            {
                _FilteredTable = value;
            }
        }

        IDataSet _DataSet;
        public IDataSet DataSet
        {
            get 
            {
                return _DataSet;
            }
            internal set
            {
                _DataSet = value;
            }
        }

        string _TableName;
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }



        public IDataColumn[] PrimaryKey
        {
            get
            {
                return PrimaryKeyDataIndex.DataColumns.ToArray();
            }
            set
            {
                if (value != null && value.Length > 0)
                {
                    foreach (var column in value)
                    {
                        if (column.Table != this)
                            throw new System.Exception("columns do not belong to this table");

                        if (PrimaryKeyDataIndex == null)
                            PrimaryKeyDataIndex = new DataIndex(value.OfType<DataColumn>().ToList(), true);
                        else
                        {
                            if (PrimaryKeyDataIndex.DataColumns.Count != value.Length)
                            {
                                PrimaryKeyDataIndex.Remove();
                            }
                            else
                            {
                                for (int i = 0; i < value.Length; i++)
                                {
                                    if (value[i] != PrimaryKeyDataIndex.DataColumns[i])
                                    {
                                        PrimaryKeyDataIndex.Remove();
                                        break;
                                    }
                                }
                            }

                            PrimaryKeyDataIndex = new DataIndex(value.OfType<DataColumn>().ToList(), true);
                        }
                    }
                }
            }
        }
        //List<object[]> RowsValues=new List<object[]>
        public IDataRow NewRow()
        {
            return new DataRow(this);
        }

        public IDataRow LoadDataRow(object[] values, LoadOption loadOption)
        {
            DataRow row = new DataRow(values,this);
            Rows.Add(row);
            return row;
        }

        DataRelationCollection _ChildRelations;
        public IDataRelationCollection ChildRelations
        {
            get 
            { 
                if(_ChildRelations==null)
                    _ChildRelations =  new DataRelationCollection(DataSet as DataSet);
                return _ChildRelations;
            }
        }

        DataRelationCollection _ParentRelations;
        public IDataRelationCollection ParentRelations
        {
            get 
            {
                if (_ParentRelations == null)
                    _ParentRelations = new DataRelationCollection(DataSet as DataSet);
                return _ParentRelations;

            }
        }

        public void BeginLoadData()
        {
            //throw new NotImplementedException();
        }

        public void EndLoadData()
        {
           // throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Merge(IDataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void RemoveTableRelations()
        {
            throw new NotImplementedException();
        }

        public DataLoader.StreamedTable SerializeTable()
        {
            throw new NotImplementedException();
        }

        public IDataReader CreateDataReader()
        {
            return new DataReader(this);
        }

        #endregion

        internal DataIndex PrimaryKeyDataIndex;
        bool AutoGenerate;
        public DataTable(bool autoGenerate)
        {
            AutoGenerate = autoGenerate;
            _Columns = new DataColumnCollection(this);
        }
        public DataTable():this(false)
        {

        }
        public DataTable(string tableName)
        {
            _TableName=tableName;

            _Columns = new DataColumnCollection(this);
        }
        byte[] Buffer = new byte[40];
        int Offset = 0;
        public DataTable(DataLoader.StreamedTable streamedTable)
        {

#if !DeviceDotNet

            TableName = BinaryFormatter.BinaryFormatter.ToString(streamedTable.StreamedData, Offset, ref Offset);
            int columnsCount = BinaryFormatter.BinaryFormatter.ToInt32(streamedTable.StreamedData, Offset, ref Offset, true);
            for (int i = 0; i != columnsCount; i++)
            {
                string columnName = BinaryFormatter.BinaryFormatter.ToString(streamedTable.StreamedData, Offset, ref Offset);
                string dataType = BinaryFormatter.BinaryFormatter.ToString(streamedTable.StreamedData, Offset, ref Offset);
                Columns.Add(columnName, ModulePublisher.ClassRepository.GetType(dataType, ""));
            }
            int rowsCount = BinaryFormatter.BinaryFormatter.ToInt32(streamedTable.StreamedData, Offset, ref Offset, true);
            for (int i = 0; i != rowsCount; i++)
            {
                byte[] rowBuffer = BinaryFormatter.BinaryFormatter.ToBytes(streamedTable.StreamedData, Offset, ref Offset);
                int rowOffset = 0;
                IDataRow row = NewRow();
                for (int columnIndex = 0; columnIndex != columnsCount; columnIndex++)
                {

                    //if (!Columns[columnIndex].DataType.IsPrimitive && !Columns[columnIndex].DataType.IsValueType)
                    if (!BinaryFormatter.BinaryFormatter.IsSerializeable(Columns[columnIndex].DataType))////if (Columns[columnIndex].DataType.IsSubclassOf(typeof(MarshalByRefObject)))
                    {

                        int index = BinaryFormatter.BinaryFormatter.ToInt32(rowBuffer, rowOffset, ref rowOffset, false);

                        object value = streamedTable.Objects[index];
                        if (Columns[columnIndex].DataType == typeof(Transactions.Transaction) && value is string)
                        {
                            value = Transactions.TransactionRunTime.UnMarshal(value as string);
                            streamedTable.Objects[index] = value;
                        }

                        row[columnIndex] = value;
                    }
                    else
                    {
                        try
                        {
                            object value = BinaryFormatter.BinaryFormatter.ToObject(Columns[columnIndex].DataType, rowBuffer, rowOffset, ref rowOffset);
                            if (value != null)
                                row[columnIndex] = value;
                            else
                                row[columnIndex] = System.DBNull.Value;
                        }
                        catch (System.Exception error)
                        {
                            throw error;
                        }
                    }
                }
                Rows.Add(row);
            }


            int tt = 0;

#endif
        }
        internal  void AllRowsRemoved()
        {
            throw new NotImplementedException();
        }




        internal event RowAdded RowAdded;
        internal event RowRemoved RowRemoved;
        internal event RowChanged RowChanged;
        private DataLoader.StreamedTable streamedTable;

        
        internal void OnRowAdded(DataRow row)
        {

            if (RowAdded != null)
                RowAdded(row);
        }

        internal  void OnRowRemoved(DataRow row)
        {
            if (RowRemoved != null)
                RowRemoved(row);
        }
        internal void OnRowChanged(DataRow dataRow, object[] oldValues)
        {
            if (RowChanged != null)
                RowChanged(dataRow, oldValues);
        }

        internal DataIndex GetDataIndex(IDataColumn[] dataColumns)
        {
            if (PrimaryKeyDataIndex != null)
            {
                if (PrimaryKeyDataIndex.IsDataIndexOf(dataColumns))
                    return PrimaryKeyDataIndex;
            }
            return null;
        }



        #region IDataTable Members

        public Guid _DataSourceIdentity;
        public Guid DataSourceIdentity
        {
            get
            {
                return _DataSourceIdentity;
            }
            set
            {
                _DataSourceIdentity = value;
            }
        }

        #endregion
    }
}
