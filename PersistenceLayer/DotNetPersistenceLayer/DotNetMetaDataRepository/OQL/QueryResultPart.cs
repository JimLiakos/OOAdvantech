using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{aed42d02-1cb4-430f-8d34-00a789cda1f3}</MetaDataID>
    [Serializable]
    
    public abstract class QueryResultPart
    {
        /// <MetaDataID>{ee0f1e4e-7cfe-4ef5-bf31-084d11ed67b0}</MetaDataID>
        public override string ToString()
        {
            return Name;
        }
        internal protected QueryResultType OwnerType;
        /// <MetaDataID>{2ac14eb0-ad47-4c24-b413-2fecba2c8b65}</MetaDataID>
        public QueryResultPart(string name, QueryResultType ownerType)
        {
            OwnerType = ownerType;
            _Name = name;
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{554085b2-f0db-4901-848b-0508c6d3460a}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public abstract DataNode SourceDataNode
        {
            get;
            set;
        }

        int[] _PartIndices = new int[2] { -1, -1 };
        public int[] PartIndices
        {
            get
            {
                return _PartIndices;
            }

            set
            {
                _PartIndices = value;
            }

        }

        internal object GetValue(CompositeRowData rowData)
        {
            return rowData[PartIndices[0]][PartIndices[1]];
        }

        internal abstract QueryResultPart Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects);
       
    }
}
