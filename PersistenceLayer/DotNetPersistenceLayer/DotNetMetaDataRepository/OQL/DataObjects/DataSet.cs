using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{9a747954-36a1-469e-a51e-15e6d40bb482}</MetaDataID>
    public class DataSet:IDataSet
    {
        #region IDataSet Members


        public void AddTable(IDataTable dataTable)
        {
            Tables.Add(dataTable);
        }
        public void Clear()
        {
            Tables.Clear();
            if (_Relations != null)
                _Relations.Clear();
        }

        public void RemoveTable(IDataTable dataTable)
        {
            Tables.Remove(dataTable);
        }

        IDataRelationCollection _Relations;
        public IDataRelationCollection Relations
        {
            get 
            {
                if (_Relations == null)
                    _Relations = new DataRelationCollection(this);
                return _Relations;
            }
        }

        DataTableCollection _Tables;
        public IDataTableCollection Tables
        {
            get 
            {
                if(_Tables ==null)
                    _Tables = new DataTableCollection(this);
                return _Tables;
            }
        }

        #endregion
    }
}
