using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataObjects
{
    /// <MetaDataID>{987623d6-6560-4fb7-bc40-d5708521f868}</MetaDataID>
    public class DataRow : IDataRow
    {

        object[] RowValues;
        Type[] Types;


        DataTable OwnerTable;
        private object[] values;
        private DataTable DataTable;

        internal DataRow(DataTable table)
        {
            OwnerTable = table;
            Types = new System.Type[table.Columns.Count];
            int i = 0;
            RowValues = new object[table.Columns.Count];
            foreach (IDataColumn column in table.Columns)
            {
                RowValues[i] = column.DefaultValue;
                Types[i++] = column.DataType;
            }

            

        }

        int SortIndex = 0;

        public void SetSortIndexValue(int sortIndex)
        {
            SortIndex = sortIndex;
        }
        public int GetSortIndexValue()
        {
            return SortIndex;
        }

        public DataRow(object[] values, DataTable dataTable)
            :this(dataTable)
        {
            // TODO: Complete member initialization
            this.values = values;
            DataTable = dataTable;
            int i = 0;
            foreach (object value in values)
                this[i++] = value;
        }


        #region IDataRow Members

        public object this[int columnIndex]
        {
            get
            {
                return RowValues[columnIndex];
            }
            set
            {
                System.Type type = Types[columnIndex];
                if (value == null)
                {
                    if (value == null)
                        value = DataTable.Columns[columnIndex].DefaultValue;
                    RowValues[columnIndex] = value;
                }
                else
                {
                    if (type.GetMetaData().IsInstanceOfType(value))
                        RowValues[columnIndex] = value;
                    else
                    {
#if DeviceDotNet
                        try
                        {
                            value = Convert.ChangeType(value, type);
                            RowValues[columnIndex] = value;
                        }
                        catch (Exception error)
                        {
                            throw new System.Exception("Type mismatch");
                        }
#else
                        if (value is IConvertible)
                        {
                            try
                            {
                                value = (value as IConvertible).ToType(type, null);
                                RowValues[columnIndex] = value;
                            }
                            catch (Exception error)
                            {

                                throw new System.Exception("Type mismatch");
                            }
                        }
                        else
                            throw new System.Exception("Type mismatch");
#endif
                    }
                }
            }
        }

        public object this[string columnName]
        {
            get
            {

                return RowValues[OwnerTable.Columns.IndexOf(columnName)];
                
            }
            set
            {
                int columnIndex = OwnerTable.Columns.IndexOf(columnName);

                 System.Type type = Types[columnIndex];
                if (value == null)
                    RowValues[columnIndex] = value;
                else
                {
                    if (type.GetMetaData().IsInstanceOfType(value))
                        RowValues[columnIndex] = value;
                    else
                        throw new System.Exception("Type mismatch");
                }
                
            }
        }

        public object[] ItemArray
        {
            get
            {
                return RowValues;
            }
            set
            {
                if(value==null)
                    throw new System.Exception("Invalid ItemArray");
                int i=0;
                foreach (object obj in value)
                {

                    if (value != null)
                    {
                        System.Type type = Types[i];
                         if (!type.GetMetaData().IsInstanceOfType(value))
                             throw new System.Exception("Type mismatch");
                    }
                }
                RowValues = value;

            }
        }

        public IDataTable Table
        {
            get { return OwnerTable; }
        }

        public IDataRow[] GetChildRows(string relationName)
        {
            MultiPartKey key= (OwnerTable.ChildRelations[relationName] as DataRelation).ParentDataIndex.GetKey(this);
            return (OwnerTable.ChildRelations[relationName] as DataRelation).ChildDataIndex.GetRows(key);
        }

        public IDataRow[] GetParentRows(string relationName)
        {
            MultiPartKey key = (OwnerTable.ParentRelations[relationName] as DataRelation).ChildDataIndex.GetKey(this);
            return (OwnerTable.ParentRelations[relationName] as DataRelation).ParentDataIndex.GetRows(key);
        }

        public IDataRow GetParentRow(string relationName)
        {
            var parentRows = GetParentRows(relationName);
            if (parentRows.Length > 0)
                return parentRows[0];
            else
                return null;
        }

        public void Delete()
        {
            OwnerTable.Rows.Remove(this);
            OwnerTable = null;
        }

#endregion
    }
}
