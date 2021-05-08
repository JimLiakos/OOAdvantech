using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{0e024fe6-d694-4af5-9820-e860ae4604a6}</MetaDataID>
    public class DataColumn : IDataColumn
    {

        DataColumnCollection _ColumnContainer;


        public DataColumnCollection ColumnContainer
        {
            get
            {
                return _ColumnContainer;
            }
            set
            {
                _ColumnContainer = value;
                
            }
        }
        #region IDataColumn Members

        public int Ordinal
        {
            get 
            {
                if (_ColumnContainer == null)
                    return -1;
                return _ColumnContainer.IndexOf(this);
            }
        }

        Type _DataType;
        public Type DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
            }
        }

        bool _ReadOnly;
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
            }
        }

        string _ColumnName;
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                if (_ColumnName != value && _ColumnContainer != null)
                {
                    _ColumnContainer.NamedDataColumns.Add(value, this);
                    _ColumnContainer.NamedDataColumns.Remove(_ColumnName);
                }
                _ColumnName = value;
            }
        }

        public IDataTable Table
        {
            get 
            {
                return DataTable; 
            }
        }


        #endregion
        internal DataTable DataTable;
        public DataColumn(string columnName, DataTable dataTable)
        {
            // TODO: Complete member initialization
            DataTable = dataTable;
            _ColumnName = columnName;
        }

        public DataColumn(string columnName, Type dataType, DataTable dataTable)
        {
            // TODO: Complete member initialization
            DataTable = dataTable;
            _ColumnName = columnName;
            _DataType = dataType;
        }






        #region IDataColumn Members

        object _DefaultValue;
        public object DefaultValue
        {
            get
            {
                return _DefaultValue;
            }
            set
            {
                _DefaultValue = value;
            }
        }

        #endregion
    }
}
