using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{


    /// <MetaDataID>{f83a35c1-8517-45a3-8c09-716c9b6467ff}</MetaDataID>
    public interface IDataReader
    {

        bool Read();
        int GetValues(object[] values);
        int GetOrdinal(string name);
        string GetName(int i);
        int FieldCount { get; }
        Type GetFieldType(int i);

        object this[int ordinal] { get; }
        object this[string name] { get; }
        void Close();

        bool NextResult();
    }
    /// <MetaDataID>{66351ee8-cd33-4a49-b5b5-8630ea5e5bdd}</MetaDataID>
    public interface IDataTable
    {

        Dictionary<string, object> ExtendedProperties
        {
            get;
        }
        bool TemporaryTableTransfered
        {
            get;
            set;
        }
        IDataColumnCollection Columns
        {
            get;
        }
        DataSource OwnerDataSource { get; set; }
        IDataRowCollection Rows { get; }

        IDataSet DataSet { get; }

        string TableName { get; set; }

        //IDataColumn AddColumn(string columnName, Type type);

        IDataColumn[] PrimaryKey { get; set; }

        IDataRow NewRow();

        IDataRow LoadDataRow(object[] values, LoadOption loadOption);
        IDataRelationCollection ChildRelations { get; }
        IDataRelationCollection ParentRelations { get; }


        void BeginLoadData();

        void EndLoadData();

        void Clear();

        void Merge(IDataTable dataTable);

        void RemoveTableRelations();

        DataLoader.StreamedTable SerializeTable();

        IDataReader CreateDataReader();

        bool FilteredTable { get; set; }

        Guid DataSourceIdentity { get; set; }
    }
    /// <MetaDataID>{95ec5e16-b557-411a-bdef-6537d5d3c88f}</MetaDataID>
    public interface IDataColumn
    {

        IDataTable Table { get; }

        int Ordinal { get; }

        System.Type DataType { get; set; }
        bool ReadOnly { get; set; }
        string ColumnName { get; set; }

        object DefaultValue { get; set; }
    }

    /// <MetaDataID>{620a8fd7-8f8f-4717-bb39-dd5aea25dbff}</MetaDataID>
    public enum LoadOption
    {
        // Summary:
        //     The incoming values for this row will be written to both the current value
        //     and the original value versions of the data for each column.
        OverwriteChanges = 1,
        //
        // Summary:
        //     The incoming values for this row will be written to the original value version
        //     of each column. The current version of the data in each column will not be
        //     changed. This is the default.
        PreserveChanges = 2,
        //
        // Summary:
        //     The incoming values for this row will be written to the current version of
        //     each column. The original version of each column's data will not be changed.
        Upsert = 3,
    }

    /// <MetaDataID>{570e4418-5b19-4654-b59d-04dc15c82c3d}</MetaDataID>
    public interface IDataRow
    {
        object this[int columnIndex] { get; set; }
        object this[string columnName] { get; set; }
        object[] ItemArray { get; set; }

        IDataTable Table { get; }

        IDataRow[] GetChildRows(string relationName);
        IDataRow[] GetParentRows(string relationName);
        IDataRow GetParentRow(string relationName);


        void Delete();
    }


    /// <MetaDataID>{677e2888-3db4-4bec-84c2-5c06f900aa7f}</MetaDataID>
    public interface IDataRelationCollection : System.Collections.IEnumerable
    {
        IDataRelation this[int index] { get; }
        IDataRelation this[string name] { get; }
        
        bool Contains(string name);

        void Add(IDataRelation relation);
        //
        // Summary:
        //     Creates a System.Data.DataRelation with a specified parent and child column,
        //     and adds it to the collection.
        //
        // Parameters:
        //   parentColumn:
        //     The parent column of the relation.
        //
        //   childColumn:
        //     The child column of the relation.
        //
        // Returns:
        //     The created relation.
        IDataRelation Add(IDataColumn parentColumn, IDataColumn childColumn);
        //
        // Summary:
        //     Creates a System.Data.DataRelation with the specified parent and child columns,
        //     and adds it to the collection.
        //
        // Parameters:
        //   parentColumns:
        //     The parent columns of the relation.
        //
        //   childColumns:
        //     The child columns of the relation.
        //
        // Returns:
        //     The created relation.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The relation argument is a null value.
        //
        //   System.ArgumentException:
        //     The relation already belongs to this collection, or it belongs to another
        //     collection.
        //
        //   System.Data.DuplicateNameException:
        //     The collection already has a relation with the same name. (The comparison
        //     is not case sensitive.)
        //
        //   System.Data.InvalidConstraintException:
        //     The relation has entered an invalid state since it was created.
        IDataRelation Add(IDataColumn[] parentColumns, IDataColumn[] childColumns);
        //
        // Summary:
        //     Creates a System.Data.DataRelation with the specified name, and parent and
        //     child columns, and adds it to the collection.
        //
        // Parameters:
        //   name:
        //     The name of the relation.
        //
        //   parentColumn:
        //     The parent column of the relation.
        //
        //   childColumn:
        //     The child column of the relation.
        //
        // Returns:
        //     The created relation.
        IDataRelation Add(string name, IDataColumn parentColumn, IDataColumn childColumn);
        //
        // Summary:
        //     Creates a System.Data.DataRelation with the specified name and arrays of
        //     parent and child columns, and adds it to the collection.
        //
        // Parameters:
        //   name:
        //     The name of the DataRelation to create.
        //
        //   parentColumns:
        //     An array of parent System.Data.DataColumn objects.
        //
        //   childColumns:
        //     An array of child DataColumn objects.
        //
        // Returns:
        //     The created DataRelation.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The relation name is a null value.
        //
        //   System.ArgumentException:
        //     The relation already belongs to this collection, or it belongs to another
        //     collection.
        //
        //   System.Data.DuplicateNameException:
        //     The collection already has a relation with the same name. (The comparison
        //     is not case sensitive.)
        //
        //   System.Data.InvalidConstraintException:
        //     The relation has entered an invalid state since it was created.
        IDataRelation Add(string name, IDataColumn[] parentColumns, IDataColumn[] childColumns);
        //
        // Summary:
        //     Creates a System.Data.DataRelation with the specified name, parent and child
        //     columns, with optional constraints according to the value of the createConstraints
        //     parameter, and adds it to the collection.
        //
        // Parameters:
        //   name:
        //     The name of the relation.
        //
        //   parentColumn:
        //     The parent column of the relation.
        //
        //   childColumn:
        //     The child column of the relation.
        //
        //   createConstraints:
        //     true to create constraints; otherwise false. (The default is true).
        //
        // Returns:
        //     The created relation.
        IDataRelation Add(string name, IDataColumn parentColumn, IDataColumn childColumn, bool createConstraints);
        //
        // Summary:
        //     Creates a System.Data.DataRelation with the specified name, arrays of parent
        //     and child columns, and value specifying whether to create a constraint, and
        //     adds it to the collection.
        //
        // Parameters:
        //   name:
        //     The name of the DataRelation to create.
        //
        //   parentColumns:
        //     An array of parent System.Data.DataColumn objects.
        //
        //   childColumns:
        //     An array of child DataColumn objects.
        //
        //   createConstraints:
        //     true to create a constraint; otherwise false.
        //
        // Returns:
        //     The created relation.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The relation name is a null value.
        //
        //   System.ArgumentException:
        //     The relation already belongs to this collection, or it belongs to another
        //     collection.
        //
        //   System.Data.DuplicateNameException:
        //     The collection already has a relation with the same name. (The comparison
        //     is not case sensitive.)
        //
        //   System.Data.InvalidConstraintException:
        //     The relation has entered an invalid state since it was created.
        IDataRelation Add(string name, IDataColumn[] parentColumns, IDataColumn[] childColumns, bool createConstraints);


        void Clear();
    }

    /// <MetaDataID>{03c973f6-fa7e-428c-b134-d9621b38d510}</MetaDataID>
    public interface IDataRelation
    {
        string RelationName
        {
            get;
            set;
        }

    }

    /// <MetaDataID>{d11092b8-cd64-4cf0-aaf9-8f6370cdc34d}</MetaDataID>
    public interface IDataRowCollection:System.Collections.IEnumerable
    {
        int Count { get; }
        IDataRow this[int index] { get; }
        void Add(IDataRow row);
        IDataRow Add(params object[] values);
        void Clear();
        bool Contains(object key);
        bool Contains(object[] keys);
        void CopyTo(Array ar, int index);
        void CopyTo(IDataRow[] array, int index);
        IDataRow Find(object key);
        IDataRow Find(object[] keys);
        System.Collections.IEnumerator GetEnumerator();
        int IndexOf(IDataRow row);
        void InsertAt(IDataRow row, int pos);
        void Remove(IDataRow row);
        void RemoveAt(int index);
        IDataRow[] ToArray();
    }

    /// <MetaDataID>{ce722ef6-b362-4666-82d4-c67460d2492f}</MetaDataID>
    public interface IDataSet
    {
        void AddTable(IDataTable dataTable);
        void RemoveTable(IDataTable dataTable);

        IDataRelationCollection Relations { get; }
        IDataTableCollection Tables { get; }

        void Clear();
    }

    /// <MetaDataID>{3855da0a-896a-4048-9d92-aea90ef66395}</MetaDataID>
    public interface IDataTableCollection
    {
        // Summary:
        //     Gets the System.Data.DataTable object at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the System.Data.DataTable to find.
        //
        // Returns:
        //     A System.Data.DataTable.
        //
        // Exceptions:
        //   System.IndexOutOfRangeException:
        //     The index value is greater than the number of items in the collection.
        IDataTable this[int index] { get; }
        //
        // Summary:
        //     Gets the System.Data.DataTable object with the specified name.
        //
        // Parameters:
        //   name:
        //     The name of the DataTable to find.
        //
        // Returns:
        //     A System.Data.DataTable with the specified name; otherwise null if the System.Data.DataTable
        //     does not exist.
        IDataTable this[string name] { get; }




        // Summary:
        //     Creates a new System.Data.DataTable object by using a default name and adds
        //     it to the collection.
        //
        // Returns:
        //     The newly created System.Data.DataTable.
        IDataTable Add();
        //
        // Summary:
        //     Adds the specified DataTable to the collection.
        //
        // Parameters:
        //   table:
        //     The DataTable object to add.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The value specified for the table is null.
        //
        //   System.ArgumentException:
        //     The table already belongs to this collection, or belongs to another collection.
        //
        //   System.Data.DuplicateNameException:
        //     A table in the collection has the same name. The comparison is not case sensitive.
        void Add(IDataTable table);
        //
        // Summary:
        //     Creates a System.Data.DataTable object by using the specified name and adds
        //     it to the collection.
        //
        // Parameters:
        //   name:
        //     The name to give the created System.Data.DataTable.
        //
        // Returns:
        //     The newly created System.Data.DataTable.
        //
        // Exceptions:
        //   System.Data.DuplicateNameException:
        //     A table in the collection has the same name. (The comparison is not case
        //     sensitive.)
        IDataTable Add(string name);

        //
        // Summary:
        //     Verifies whether the specified System.Data.DataTable object can be removed
        //     from the collection.
        //
        // Parameters:
        //   table:
        //     The DataTable in the collection to perform the check against.
        //
        // Returns:
        //     true if the table can be removed; otherwise false.
        bool CanRemove(IDataTable table);
        //
        // Summary:
        //     Clears the collection of all System.Data.DataTable objects.
        void Clear();
        //
        // Summary:
        //     Gets a value that indicates whether a System.Data.DataTable object with the
        //     specified name exists in the collection.
        //
        // Parameters:
        //   name:
        //     The name of the System.Data.DataTable to find.
        //
        // Returns:
        //     true if the specified table exists; otherwise false.
        bool Contains(string name);


        //
        // Summary:
        //     Gets the index of the specified System.Data.DataTable object.
        //
        // Parameters:
        //   table:
        //     The DataTable to search for.
        //
        // Returns:
        //     The zero-based index of the table, or -1 if the table is not found in the
        //     collection.
        int IndexOf(IDataTable table);
        //
        // Summary:
        //     Gets the index in the collection of the System.Data.DataTable object with
        //     the specified name.
        //
        // Parameters:
        //   tableName:
        //     The name of the DataTable object to look for.
        //
        // Returns:
        //     The zero-based index of the DataTable with the specified name, or -1 if the
        //     table does not exist in the collection.Note:Returns -1 when two or more tables
        //     have the same name but different namespaces. The call does not succeed if
        //     there is any ambiguity when matching a table name to exactly one table.
        int IndexOf(string tableName);

        //
        // Summary:
        //     Removes the specified System.Data.DataTable object from the collection.
        //
        // Parameters:
        //   table:
        //     The DataTable to remove.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The value specified for the table is null.
        //
        //   System.ArgumentException:
        //     The table does not belong to this collection.-or- The table is part of a
        //     relationship.
        void Remove(IDataTable table);
        //
        // Summary:
        //     Removes the System.Data.DataTable object with the specified name from the
        //     collection.
        //
        // Parameters:
        //   name:
        //     The name of the System.Data.DataTable object to remove.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The collection does not have a table with the specified name.
        void Remove(string name);

        //
        // Summary:
        //     Removes the System.Data.DataTable object at the specified index from the
        //     collection.
        //
        // Parameters:
        //   index:
        //     The index of the DataTable to remove.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The collection does not have a table at the specified index.
        void RemoveAt(int index);
    }


    /// <MetaDataID>{fe39df8b-c55c-4f9d-80f6-1fe1a3aa0b2a}</MetaDataID>
    public interface IDataColumnCollection : System.Collections.IEnumerable
    {


        int Count { get; }
        // Summary:
        //     Gets the System.Data.DataColumn from the collection at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the column to return.
        //
        // Returns:
        //     The System.Data.DataColumn at the specified index.
        //
        // Exceptions:
        //   System.IndexOutOfRangeException:
        //     The index value is greater than the number of items in the collection.
        IDataColumn this[int index] { get; }
        //
        // Summary:
        //     Gets the System.Data.DataColumn from the collection with the specified name.
        //
        // Parameters:
        //   name:
        //     The System.Data.DataColumn.ColumnName of the column to return.
        //
        // Returns:
        //     The System.Data.DataColumn in the collection with the specified System.Data.DataColumn.ColumnName;
        //     otherwise a null value if the System.Data.DataColumn does not exist.
        IDataColumn this[string name] { get; }

        // Summary:
        ////     Occurs when the columns collection changes, either by adding or removing
        ////     a column.
        //[ResDescription("collectionChangedEventDescr")]
        //public event CollectionChangeEventHandler CollectionChanged;

        // Summary:
        //     Creates and adds a System.Data.DataColumn object to the System.Data.DataColumnCollection.
        //
        // Returns:
        //     The newly created System.Data.DataColumn.
        IDataColumn Add();
        //
        // Summary:
        //     Creates and adds the specified System.Data.DataColumn object to the System.Data.DataColumnCollection.
        //
        // Parameters:
        //   column:
        //     The System.Data.DataColumn to add.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The column parameter is null.
        //
        //   System.ArgumentException:
        //     The column already belongs to this collection, or to another collection.
        //
        //   System.Data.DuplicateNameException:
        //     The collection already has a column with the specified name. (The comparison
        //     is not case-sensitive.)
        //
        //   System.Data.InvalidExpressionException:
        //     The expression is invalid. See the System.Data.DataColumn.Expression property
        //     for more information about how to create expressions.
        void Add(IDataColumn column);
        //
        // Summary:
        //     Creates and adds a System.Data.DataColumn object that has the specified name
        //     to the System.Data.DataColumnCollection.
        //
        // Parameters:
        //   columnName:
        //     The name of the column.
        //
        // Returns:
        //     The newly created System.Data.DataColumn.
        //
        // Exceptions:
        //   System.Data.DuplicateNameException:
        //     The collection already has a column with the specified name. (The comparison
        //     is not case-sensitive.)
        IDataColumn Add(string columnName);
        //
        // Summary:
        //     Creates and adds a System.Data.DataColumn object that has the specified name
        //     and type to the System.Data.DataColumnCollection.
        //
        // Parameters:
        //   columnName:
        //     The System.Data.DataColumn.ColumnName to use when you create the column.
        //
        //   type:
        //     The System.Data.DataColumn.DataType of the new column.
        //
        // Returns:
        //     The newly created System.Data.DataColumn.
        //
        // Exceptions:
        //   System.Data.DuplicateNameException:
        //     The collection already has a column with the specified name. (The comparison
        //     is not case-sensitive.)
        //
        //   System.Data.InvalidExpressionException:
        //     The expression is invalid. See the System.Data.DataColumn.Expression property
        //     for more information about how to create expressions.
        IDataColumn Add(string columnName, Type type);

        //
        // Summary:
        //     Copies the elements of the specified System.Data.DataColumn array to the
        //     end of the collection.
        //
        // Parameters:
        //   columns:
        //     The array of System.Data.DataColumn objects to add to the collection.
        void AddRange(IDataColumn[] columns);
        //
        // Summary:
        //     Checks whether a specific column can be removed from the collection.
        //
        // Parameters:
        //   column:
        //     A System.Data.DataColumn in the collection.
        //
        // Returns:
        //     true if the column can be removed; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The column parameter is null.
        //
        //   System.ArgumentException:
        //     The column does not belong to this collection.-Or- The column is part of
        //     a relationship.-Or- Another column's expression depends on this column.
        bool CanRemove(IDataColumn column);
        //
        // Summary:
        //     Clears the collection of any columns.
        void Clear();
        //
        // Summary:
        //     Checks whether the collection contains a column with the specified name.
        //
        // Parameters:
        //   name:
        //     The System.Data.DataColumn.ColumnName of the column to look for.
        //
        // Returns:
        //     true if a column exists with this name; otherwise, false.
        bool Contains(string name);


        //
        // Summary:
        //     Gets the index of a column specified by name.
        //
        // Parameters:
        //   column:
        //     The name of the column to return.
        //
        // Returns:
        //     The index of the column specified by column if it is found; otherwise, -1.
        int IndexOf(IDataColumn column);
        //
        // Summary:
        //     Gets the index of the column with the specific name (the name is not case
        //     sensitive).
        //
        // Parameters:
        //   columnName:
        //     The name of the column to find.
        //
        // Returns:
        //     The zero-based index of the column with the specified name, or -1 if the
        //     column does not exist in the collection.
        int IndexOf(string columnName);
        //
        // Summary:
        //     Removes the specified System.Data.DataColumn object from the collection.
        //
        // Parameters:
        //   column:
        //     The System.Data.DataColumn to remove.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The column parameter is null.
        //
        //   System.ArgumentException:
        //     The column does not belong to this collection.-Or- The column is part of
        //     a relationship.-Or- Another column's expression depends on this column.
        void Remove(IDataColumn column);
        //
        // Summary:
        //     Removes the System.Data.DataColumn object that has the specified name from
        //     the collection.
        //
        // Parameters:
        //   name:
        //     The name of the column to remove.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The collection does not have a column with the specified name.
        void Remove(string name);
        //
        // Summary:
        //     Removes the column at the specified index from the collection.
        //
        // Parameters:
        //   index:
        //     The index of the column to remove.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The collection does not have a column at the specified index.
        void RemoveAt(int index);
    }



    /// <MetaDataID>{c49a1aee-1ac1-45bb-911e-6451bdf4cb5c}</MetaDataID>
    public interface IDataObjectsInstantiator
    {
        IDataSet CreateDataSet();
        IDataTable CreateDataTable();
        IDataTable CreateDataTable(bool autoGenerated);
        IDataTable CreateDataTable(bool autoGenerated,Guid dataSourceIdentity);
        
        IDataTable CreateDataTable(DataLoader.StreamedTable streamedTable);
        IDataTable CreateDataTable(string tableName);
    }



}
