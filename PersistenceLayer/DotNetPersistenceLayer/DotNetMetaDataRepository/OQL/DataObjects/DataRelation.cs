using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{25ec0c77-cc8a-4b9a-afd7-b8a562041d19}</MetaDataID>
    public class DataRelation:IDataRelation
    {
        #region IDataRelation Members
        string _RelationName;
        internal readonly DataIndex ParentDataIndex;
        internal readonly DataIndex ChildDataIndex;

        internal DataTable ParentTable
        {
            get
            {
                return ParentDataIndex.Table;
            }
        }

        internal DataTable ChildTable
        {
            get
            {
                return ChildDataIndex.Table;
            }
        }

        public DataRelation(string name, DataIndex parentDataIndex, DataIndex childDataIndex)
        {
            // TODO: Complete member initialization
            this._RelationName = name;
            this.ParentDataIndex = parentDataIndex;
            this.ChildDataIndex = childDataIndex;
            
        }
        public string RelationName
        {
            get
            {
                return _RelationName;
            }
            set
            {
                //_RelationName = value;
            }
        }


        #endregion

        
    }
}
