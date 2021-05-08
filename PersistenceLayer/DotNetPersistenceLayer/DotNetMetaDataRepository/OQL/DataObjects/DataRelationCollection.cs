using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{e9d5018a-ebe2-4a7e-94d9-5b49462fb621}</MetaDataID>
    public class DataRelationCollection:IDataRelationCollection
    {
        private DataSet DataSet;

        List<DataRelation> DataRelations = new List<DataRelation>();
        internal Dictionary<string, DataRelation> NamedDataRelations = new Dictionary<string, DataRelation>();

        public DataRelationCollection(DataSet dataSet)
        {
            // TODO: Complete member initialization
            DataSet = dataSet;
        }

        #region IDataRelationCollection Members

        public IDataRelation this[int index]
        {
            get 
            {
                return DataRelations[index];
            }
        }

        public IDataRelation this[string name]
        {
            get 
            { 
                DataRelation dataRelation=null;
                if (NamedDataRelations.TryGetValue(name, out dataRelation))
                    return dataRelation;
                else
                    return null;
                
            }
        }

        public bool Contains(string name)
        {
            return NamedDataRelations.ContainsKey(name);
        }

        public void Add(IDataRelation relation)
        {
            if (NamedDataRelations.ContainsKey(relation.RelationName))
                throw new Exception(string.Format("Relation '{0}' already exist", relation.RelationName));

            DataRelations.Add(relation as DataRelation);
            NamedDataRelations[relation.RelationName] = relation as DataRelation;
            
        }

        public IDataRelation Add(IDataColumn parentColumn, IDataColumn childColumn)
        {
            return Add("", new IDataColumn[1] { parentColumn }, new IDataColumn[1] { childColumn }, false);
        }

        public IDataRelation Add(IDataColumn[] parentColumns, IDataColumn[] childColumns)
        {
            return Add("", parentColumns, childColumns, false);

        }

        public IDataRelation Add(string name, IDataColumn parentColumn, IDataColumn childColumn)
        {
            return Add(name, new IDataColumn[1] { parentColumn }, new IDataColumn[1] { childColumn }, false);
        }

        public IDataRelation Add(string name, IDataColumn[] parentColumns, IDataColumn[] childColumns)
        {
            return Add(name,  parentColumns ,  childColumns , false);
        }

        public IDataRelation Add(string name, IDataColumn parentColumn, IDataColumn childColumn, bool createConstraints)
        {

            return Add(name, new IDataColumn[1] { parentColumn }, new IDataColumn[1] { childColumn }, createConstraints);

        }

        public IDataRelation Add(string name, IDataColumn[] parentColumns, IDataColumn[] childColumns, bool createConstraints)
        {
            if (parentColumns.Length != childColumns.Length)
                throw new System.Exception("Additional information: ParentColumns and ChildColumns should be the same length.");

            for (int i = 0; i < parentColumns.Length; i++)
            {
                if (parentColumns[i].DataType != childColumns[i].DataType)
                    throw new System.Exception("Parent Columns and Child Columns don't have type-matching columns.");
            }

            DataTable parentTable = parentColumns[0].Table as DataTable;
            DataIndex parentDataIndex = parentTable.GetDataIndex(parentColumns);
            if (parentDataIndex == null)
                parentDataIndex = new DataIndex( parentColumns.OfType<DataColumn>().ToList(), false);
            DataTable childTable = childColumns[0].Table as DataTable;
            DataIndex childDataIndex = childTable.GetDataIndex(childColumns);
            if (childDataIndex == null)
                childDataIndex = new DataIndex(childColumns.OfType<DataColumn>().ToList(), false);
            DataRelation dataRelation = new DataRelation(name, parentDataIndex, childDataIndex);

            childTable.ParentRelations.Add(dataRelation);
            parentTable.ChildRelations.Add(dataRelation);

            return dataRelation;
        }

        public void Clear()
        {
            if (DataRelations.Count == 0)
                return;
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
