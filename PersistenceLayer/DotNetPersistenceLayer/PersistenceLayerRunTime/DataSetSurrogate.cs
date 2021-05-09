using System;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Data;
using System.Runtime.Serialization;
using System.Diagnostics;

/*
    Author : Ravinder Vuppula.
    Purpose : To implement binary serialization of the DataSet through a Surrogate object.
    Notes:  
        1. All the surrogate objects DataSetSurrogate, DataTableSurrogate, DataColumnSurrogate are marked [Serializable] and hence will get automatically serialized by the remoting framework.
        2. The data is serialized in binary "column" wise.
        3. This class can be used as a wrapper around DataSet. A DataSetSurrogate object can be constructed from DataSet and vice-versa. This helps if the user wants to wrap the DataSet in DataSetSurrogate and serialize and deserialize DataSetSurrogate instead.
    History:
    05/10/04 - Fix for the  issue of serializing default values.
*/
namespace OOAdvantech.PersistenceLayerRunTime
{
    /// <MetaDataID>{f78050ac-cab2-4baa-8b35-0b6c3808cd0b}</MetaDataID>
    [Serializable]
    public class DataSetSurrogate
    {
        //DataSet properties
        /// <MetaDataID>{393dc5b5-35f5-42a8-bf62-e12f2297adcd}</MetaDataID>
        private string _datasetName;
        /// <MetaDataID>{92e98552-e540-420e-8518-00da412d2be6}</MetaDataID>
        private string _namespace;
        /// <MetaDataID>{bb45e7be-021d-4daa-b707-396d4fa2ab1f}</MetaDataID>
        private string _prefix;
        /// <MetaDataID>{5fc0f665-93ab-48b7-becc-c734782186c3}</MetaDataID>
        private bool _caseSensitive;
        /// <MetaDataID>{13d64414-3d26-4d37-ae48-e457da7579c5}</MetaDataID>
        private CultureInfo _locale;
        /// <MetaDataID>{afd9a5d6-5537-4a30-bed9-9da661d409df}</MetaDataID>
        private bool _enforceConstraints;

        //ForeignKeyConstraints
        /// <MetaDataID>{052bd5e1-8964-4477-92e0-1b87cb7d6db3}</MetaDataID>
        private ArrayList _fkConstraints;//An ArrayList of foreign key constraints :  [constraintName]->[parentTableIndex, parentcolumnIndexes]->[childTableIndex, childColumnIndexes]->[AcceptRejectRule, UpdateRule, Delete]->[extendedProperties]

        //Relations
        /// <MetaDataID>{ccfe08a0-6666-44d0-a37e-f857a4d7f8e2}</MetaDataID>
        private ArrayList _relations;//An ArrayList of foreign key constraints : [relationName]->[parentTableIndex, parentcolumnIndexes]->[childTableIndex, childColumnIndexes]->[Nested]->[extendedProperties]

        //ExtendedProperties
        /// <MetaDataID>{b61513ce-f9de-467a-9ff7-c9e5c4db29ea}</MetaDataID>
        private Hashtable _extendedProperties;

        //Columns and Rows
        /// <MetaDataID>{8c2e42a2-893a-4c9d-a4f7-d860592c0fa8}</MetaDataID>
        private DataTableSurrogate[] _dataTableSurrogates;

        /*
            Constructs a DataSetSurrogate object from a DataSet.
        */
        /// <MetaDataID>{aebaaa28-461f-4098-94d0-bdb883e3f327}</MetaDataID>
        public DataSetSurrogate(DataSet ds)
        {
            if (ds == null)
            {
                throw new ArgumentNullException("The parameter dataset is null");
            }

            //DataSet properties
            _datasetName = ds.DataSetName;
            _namespace = ds.Namespace;
            _prefix = ds.Prefix;
            _caseSensitive = ds.CaseSensitive;
            _locale = ds.Locale;
            _enforceConstraints = ds.EnforceConstraints;

            //Tables, Columns, Rows
            _dataTableSurrogates = new DataTableSurrogate[ds.Tables.Count];
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                _dataTableSurrogates[i] = new DataTableSurrogate(ds.Tables[i]);
            }

            //ForeignKeyConstraints
            _fkConstraints = GetForeignKeyConstraints(ds);

            //Relations
            _relations = GetRelations(ds);

            //ExtendedProperties
            _extendedProperties = new Hashtable();
            if (ds.ExtendedProperties.Keys.Count > 0)
            {
                foreach (object propertyKey in ds.ExtendedProperties.Keys)
                {
                    _extendedProperties.Add(propertyKey, ds.ExtendedProperties[propertyKey]);
                }
            }
        }

        /*
            Constructs a DataSet from the DataSetSurrogate object. This can be used after the user recieves a Surrogate object over the wire and wished to construct a DataSet from it.
        */
        /// <MetaDataID>{6aca6bc7-0ee9-43f5-817b-5c0fb17718d1}</MetaDataID>
        public DataSet ConvertToDataSet()
        {
            DataSet ds = new DataSet();
            ReadSchemaIntoDataSet(ds);
            ReadDataIntoDataSet(ds);
            return ds;
        }

        /*
            Reads the schema into the dataset from the DataSetSurrogate object.
        */
        /// <MetaDataID>{44f07703-6c47-4db1-ab9d-de9fedce515f}</MetaDataID>
        public void ReadSchemaIntoDataSet(DataSet ds)
        {
            if (ds == null)
            {
                throw new ArgumentNullException("The dataset parameter cannot be null");
            }

            //DataSet properties
            ds.DataSetName = _datasetName;
            ds.Namespace = _namespace;
            ds.Prefix = _prefix;
            ds.CaseSensitive = _caseSensitive;
            ds.Locale = _locale;
            ds.EnforceConstraints = _enforceConstraints;

            //Tables, Columns
            Debug.Assert(_dataTableSurrogates != null);
            foreach (DataTableSurrogate dataTableSurrogate in _dataTableSurrogates)
            {
                DataTable dt = new DataTable();
                dataTableSurrogate.ReadSchemaIntoDataTable(dt);
                ds.Tables.Add(dt);
            }

            //ForeignKeyConstraints
            SetForeignKeyConstraints(ds, _fkConstraints);

            //Relations
            SetRelations(ds, _relations);

            //Set ExpressionColumns        
            Debug.Assert(_dataTableSurrogates != null);
            Debug.Assert(ds.Tables.Count == _dataTableSurrogates.Length);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                DataTableSurrogate dataTableSurrogate = _dataTableSurrogates[i];
                dataTableSurrogate.SetColumnExpressions(dt);
            }

            //ExtendedProperties
            Debug.Assert(_extendedProperties != null);
            if (_extendedProperties.Keys.Count > 0)
            {
                foreach (object propertyKey in _extendedProperties.Keys)
                {
                    ds.ExtendedProperties.Add(propertyKey, _extendedProperties[propertyKey]);
                }
            }
        }

        /*
            Reads the data into the dataset from the DataSetSurrogate object.
        */
        /// <MetaDataID>{d0ab2c3e-1b8b-4e93-94f7-dedd2f8a0d98}</MetaDataID>
        public void ReadDataIntoDataSet(DataSet ds)
        {
            if (ds == null)
            {
                throw new ArgumentNullException("The dataset parameter cannot be null");
            }

            //Suppress  read-only columns and constraint rules when loading the data
            ArrayList readOnlyList = SuppressReadOnly(ds);
            ArrayList constraintRulesList = SuppressConstraintRules(ds);

            //Rows
            Debug.Assert(IsSchemaIdentical(ds));
            Debug.Assert(_dataTableSurrogates != null);
            Debug.Assert(ds.Tables.Count == _dataTableSurrogates.Length);
            bool enforceConstraints = ds.EnforceConstraints;
            ds.EnforceConstraints = false;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                DataTableSurrogate dataTableSurrogate = _dataTableSurrogates[i];
                dataTableSurrogate.ReadDataIntoDataTable(ds.Tables[i], false);
            }
            ds.EnforceConstraints = enforceConstraints;

            //Reset read-only columns and constraint rules back after loading the data
            ResetReadOnly(ds, readOnlyList);
            ResetConstraintRules(ds, constraintRulesList);
        }

        /*
            Gets foreignkey constraints availabe on the tables in the dataset.
            ***Serialized foreign key constraints format : [constraintName]->[parentTableIndex, parentcolumnIndexes]->[childTableIndex, childColumnIndexes]->[AcceptRejectRule, UpdateRule, Delete]->[extendedProperties]***
        */
        /// <MetaDataID>{1b97f55d-eba0-403c-819d-8f38d7c8fe18}</MetaDataID>
        private ArrayList GetForeignKeyConstraints(DataSet ds)
        {
            Debug.Assert(ds != null);

            ArrayList constraintList = new ArrayList();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                for (int j = 0; j < dt.Constraints.Count; j++)
                {
                    Constraint c = dt.Constraints[j];
                    ForeignKeyConstraint fk = c as ForeignKeyConstraint;
                    if (fk != null)
                    {
                        string constraintName = c.ConstraintName;
                        int[] parentInfo = new int[fk.RelatedColumns.Length + 1];
                        parentInfo[0] = ds.Tables.IndexOf(fk.RelatedTable);
                        for (int k = 1; k < parentInfo.Length; k++)
                        {
                            parentInfo[k] = fk.RelatedColumns[k - 1].Ordinal;
                        }

                        int[] childInfo = new int[fk.Columns.Length + 1];
                        childInfo[0] = i;//Since the constraint is on the current table, this is the child table.
                        for (int k = 1; k < childInfo.Length; k++)
                        {
                            childInfo[k] = fk.Columns[k - 1].Ordinal;
                        }

                        ArrayList list = new ArrayList();
                        list.Add(constraintName);
                        list.Add(parentInfo);
                        list.Add(childInfo);
                        list.Add(new int[] { (int)fk.AcceptRejectRule, (int)fk.UpdateRule, (int)fk.DeleteRule });
                        Hashtable extendedProperties = new Hashtable();
                        if (fk.ExtendedProperties.Keys.Count > 0)
                        {
                            foreach (object propertyKey in fk.ExtendedProperties.Keys)
                            {
                                extendedProperties.Add(propertyKey, fk.ExtendedProperties[propertyKey]);
                            }
                        }
                        list.Add(extendedProperties);

                        constraintList.Add(list);
                    }
                }
            }
            return constraintList;
        }

        /*
            Adds foreignkey constraints to the tables in the dataset. The arraylist contains the serialized format of the foreignkey constraints.
            ***Deserialize the foreign key constraints format : [constraintName]->[parentTableIndex, parentcolumnIndexes]->[childTableIndex, childColumnIndexes]->[AcceptRejectRule, UpdateRule, Delete]->[extendedProperties]***
        */
        /// <MetaDataID>{c8f1e0fa-5cea-46f9-921f-c27de701b406}</MetaDataID>
        private void SetForeignKeyConstraints(DataSet ds, ArrayList constraintList)
        {
            Debug.Assert(ds != null);
            Debug.Assert(constraintList != null);

            foreach (ArrayList list in constraintList)
            {
                Debug.Assert(list.Count == 5);
                string constraintName = (string)list[0];
                int[] parentInfo = (int[])list[1];
                int[] childInfo = (int[])list[2];
                int[] rules = (int[])list[3];
                Hashtable extendedProperties = (Hashtable)list[4];

                //ParentKey Columns.
                Debug.Assert(parentInfo.Length >= 1);
                DataColumn[] parentkeyColumns = new DataColumn[parentInfo.Length - 1];
                for (int i = 0; i < parentkeyColumns.Length; i++)
                {
                    Debug.Assert(ds.Tables.Count > parentInfo[0]);
                    Debug.Assert(ds.Tables[parentInfo[0]].Columns.Count > parentInfo[i + 1]);
                    parentkeyColumns[i] = ds.Tables[parentInfo[0]].Columns[parentInfo[i + 1]];
                }

                //ChildKey Columns.
                Debug.Assert(childInfo.Length >= 1);
                DataColumn[] childkeyColumns = new DataColumn[childInfo.Length - 1];
                for (int i = 0; i < childkeyColumns.Length; i++)
                {
                    Debug.Assert(ds.Tables.Count > childInfo[0]);
                    Debug.Assert(ds.Tables[childInfo[0]].Columns.Count > childInfo[i + 1]);
                    childkeyColumns[i] = ds.Tables[childInfo[0]].Columns[childInfo[i + 1]];
                }

                //Create the Constraint.
                ForeignKeyConstraint fk = new ForeignKeyConstraint(constraintName, parentkeyColumns, childkeyColumns);
                Debug.Assert(rules.Length == 3);
                fk.AcceptRejectRule = (AcceptRejectRule)rules[0];
                fk.UpdateRule = (Rule)rules[1];
                fk.DeleteRule = (Rule)rules[2];

                //Extended Properties.
                Debug.Assert(extendedProperties != null);
                if (extendedProperties.Keys.Count > 0)
                {
                    foreach (object propertyKey in extendedProperties.Keys)
                    {
                        fk.ExtendedProperties.Add(propertyKey, extendedProperties[propertyKey]);
                    }
                }

                //Add the constraint to the child datatable.
                Debug.Assert(ds.Tables.Count > childInfo[0]);
                ds.Tables[childInfo[0]].Constraints.Add(fk);
            }
        }

        /*
            Gets relations from the dataset.
            ***Serialized relations format : [relationName]->[parentTableIndex, parentcolumnIndexes]->[childTableIndex, childColumnIndexes]->[Nested]->[extendedProperties]***
        */
        /// <MetaDataID>{bd92ed3f-29d7-4637-870a-bd0e412a2b52}</MetaDataID>
        private ArrayList GetRelations(DataSet ds)
        {
            Debug.Assert(ds != null);

            ArrayList relationList = new ArrayList();
            foreach (DataRelation rel in ds.Relations)
            {
                string relationName = rel.RelationName;
                int[] parentInfo = new int[rel.ParentColumns.Length + 1];
                parentInfo[0] = ds.Tables.IndexOf(rel.ParentTable);
                for (int j = 1; j < parentInfo.Length; j++)
                {
                    parentInfo[j] = rel.ParentColumns[j - 1].Ordinal;
                }

                int[] childInfo = new int[rel.ChildColumns.Length + 1];
                childInfo[0] = ds.Tables.IndexOf(rel.ChildTable);
                for (int j = 1; j < childInfo.Length; j++)
                {
                    childInfo[j] = rel.ChildColumns[j - 1].Ordinal;
                }

                ArrayList list = new ArrayList();
                list.Add(relationName);
                list.Add(parentInfo);
                list.Add(childInfo);
                list.Add(rel.Nested);
                Hashtable extendedProperties = new Hashtable();
                if (rel.ExtendedProperties.Keys.Count > 0)
                {
                    foreach (object propertyKey in rel.ExtendedProperties.Keys)
                    {
                        extendedProperties.Add(propertyKey, rel.ExtendedProperties[propertyKey]);
                    }
                }
                list.Add(extendedProperties);

                relationList.Add(list);
            }
            return relationList;
        }

        /*
            Adds relations to the dataset. The arraylist contains the serialized format of the relations.
            ***Deserialize the relations format : [relationName]->[parentTableIndex, parentcolumnIndexes]->[childTableIndex, childColumnIndexes]->[Nested]->[extendedProperties]***
        */
        /// <MetaDataID>{e29f6f98-457b-4023-a4f3-f2d173eb0152}</MetaDataID>
        private void SetRelations(DataSet ds, ArrayList relationList)
        {
            Debug.Assert(ds != null);
            Debug.Assert(relationList != null);

            foreach (ArrayList list in relationList)
            {
                Debug.Assert(list.Count == 5);
                string relationName = (string)list[0];
                int[] parentInfo = (int[])list[1];
                int[] childInfo = (int[])list[2];
                bool isNested = (bool)list[3];
                Hashtable extendedProperties = (Hashtable)list[4];

                //ParentKey Columns.
                Debug.Assert(parentInfo.Length >= 1);
                DataColumn[] parentkeyColumns = new DataColumn[parentInfo.Length - 1];
                for (int i = 0; i < parentkeyColumns.Length; i++)
                {
                    Debug.Assert(ds.Tables.Count > parentInfo[0]);
                    Debug.Assert(ds.Tables[parentInfo[0]].Columns.Count > parentInfo[i + 1]);
                    parentkeyColumns[i] = ds.Tables[parentInfo[0]].Columns[parentInfo[i + 1]];
                }

                //ChildKey Columns.
                Debug.Assert(childInfo.Length >= 1);
                DataColumn[] childkeyColumns = new DataColumn[childInfo.Length - 1];
                for (int i = 0; i < childkeyColumns.Length; i++)
                {
                    Debug.Assert(ds.Tables.Count > childInfo[0]);
                    Debug.Assert(ds.Tables[childInfo[0]].Columns.Count > childInfo[i + 1]);
                    childkeyColumns[i] = ds.Tables[childInfo[0]].Columns[childInfo[i + 1]];
                }

                //Create the Relation, without any constraints[Assumption: The constraints are added earlier than the relations]
                DataRelation rel = new DataRelation(relationName, parentkeyColumns, childkeyColumns, false);
                rel.Nested = isNested;

                //Extended Properties.
                Debug.Assert(extendedProperties != null);
                if (extendedProperties.Keys.Count > 0)
                {
                    foreach (object propertyKey in extendedProperties.Keys)
                    {
                        rel.ExtendedProperties.Add(propertyKey, extendedProperties[propertyKey]);
                    }
                }

                //Add the relations to the dataset.
                ds.Relations.Add(rel);
            }
        }

        /*
            Suppress the read-only property and returns an arraylist of read-only columns.
        */
        /// <MetaDataID>{bd36260d-f13c-4deb-8bc8-a1a253622356}</MetaDataID>
        private ArrayList SuppressReadOnly(DataSet ds)
        {
            Debug.Assert(ds != null);

            ArrayList readOnlyList = new ArrayList();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].Expression == String.Empty && dt.Columns[j].ReadOnly == true)
                    {
                        dt.Columns[j].ReadOnly = false;
                        readOnlyList.Add(new int[] { i, j });
                    }
                }
            }
            return readOnlyList;
        }

        /*
            Suppress the foreign key constraint rules and returns an arraylist of the existing foreignkey constraint rules.
        */
        /// <MetaDataID>{f6517573-4ee2-44fd-9918-8f6be4abbf14}</MetaDataID>
        private ArrayList SuppressConstraintRules(DataSet ds)
        {
            Debug.Assert(ds != null);

            ArrayList constraintRulesList = new ArrayList();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dtChild = ds.Tables[i];
                for (int j = 0; j < dtChild.Constraints.Count; j++)
                {
                    Constraint c = dtChild.Constraints[j];
                    if (c is ForeignKeyConstraint)
                    {
                        ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                        ArrayList list = new ArrayList();
                        list.Add(new int[] { i, j });
                        list.Add(new int[] { (int)fk.AcceptRejectRule, (int)fk.UpdateRule, (int)fk.DeleteRule });
                        constraintRulesList.Add(list);

                        fk.AcceptRejectRule = AcceptRejectRule.None;
                        fk.UpdateRule = Rule.None;
                        fk.DeleteRule = Rule.None;
                    }
                }
            }
            return constraintRulesList;
        }

        /*
            Resets the read-only columns on the datatable based on the input readOnly list.
        */
        /// <MetaDataID>{3e309fa1-1053-4f92-87b2-52911a8380ed}</MetaDataID>
        private void ResetReadOnly(DataSet ds, ArrayList readOnlyList)
        {
            Debug.Assert(ds != null);
            Debug.Assert(readOnlyList != null);

            foreach (object o in readOnlyList)
            {
                int[] indicesArr = (int[])o;

                Debug.Assert(indicesArr.Length == 2);
                int tableIndex = indicesArr[0];
                int columnIndex = indicesArr[1];

                Debug.Assert(ds.Tables.Count > tableIndex);
                Debug.Assert(ds.Tables[tableIndex].Columns.Count > columnIndex);

                DataColumn dc = ds.Tables[tableIndex].Columns[columnIndex];
                Debug.Assert(dc != null);

                dc.ReadOnly = true;
            }
        }

        /*
            Resets the foreignkey constraint rules on the dataset based on the input constraint rules list.
        */
        /// <MetaDataID>{fd04c2fa-c279-417f-8690-e8d6d352bd84}</MetaDataID>
        private void ResetConstraintRules(DataSet ds, ArrayList constraintRulesList)
        {
            Debug.Assert(ds != null);
            Debug.Assert(constraintRulesList != null);

            foreach (ArrayList list in constraintRulesList)
            {
                Debug.Assert(list.Count == 2);
                int[] indicesArr = (int[])list[0];
                int[] rules = (int[])list[1];

                Debug.Assert(indicesArr.Length == 2);
                int tableIndex = indicesArr[0];
                int constraintIndex = indicesArr[1];

                Debug.Assert(ds.Tables.Count > tableIndex);
                DataTable dtChild = ds.Tables[tableIndex];

                Debug.Assert(dtChild.Constraints.Count > constraintIndex);
                ForeignKeyConstraint fk = (ForeignKeyConstraint)dtChild.Constraints[constraintIndex];

                Debug.Assert(rules.Length == 3);
                fk.AcceptRejectRule = (AcceptRejectRule)rules[0];
                fk.UpdateRule = (Rule)rules[1];
                fk.DeleteRule = (Rule)rules[2];
            }
        }

        /*
            Checks whether the dataset name and namespaces are as expected and the tables count is right.
        */
        /// <MetaDataID>{46040881-0325-4c90-913c-36e6f11bc0a7}</MetaDataID>
        private bool IsSchemaIdentical(DataSet ds)
        {
            Debug.Assert(ds != null);
            if (ds.DataSetName != _datasetName || ds.Namespace != _namespace)
            {
                return false;
            }
            Debug.Assert(_dataTableSurrogates != null);
            if (ds.Tables.Count != _dataTableSurrogates.Length)
            {
                return false;
            }
            return true;
        }
    }

    /// <MetaDataID>{b41ed44d-929b-453c-aed8-736d2a159657}</MetaDataID>
    [Serializable]
    class DataTableSurrogate
    {
        //DataTable properties
        /// <MetaDataID>{e923e8a1-d425-4859-a403-936147716689}</MetaDataID>
        private string _tableName;
        /// <MetaDataID>{293b9c2e-e4a9-48f3-a6b0-513c886f1414}</MetaDataID>
        private string _namespace;
        /// <MetaDataID>{66fb077e-189e-446e-82a8-88d1be21be83}</MetaDataID>
        private string _prefix;
        /// <MetaDataID>{348c61e2-6e7e-482d-b0c5-17dcaa56766f}</MetaDataID>
        private bool _caseSensitive;
        /// <MetaDataID>{5a97de79-e1ec-4201-b1dc-194b390e78b3}</MetaDataID>
        private CultureInfo _locale;
        /// <MetaDataID>{31079be6-7918-4624-b5ca-071961f74110}</MetaDataID>
        private string _displayExpression;
        /// <MetaDataID>{f9791d26-f396-46b3-87b0-8f97c816446e}</MetaDataID>
        private int _minimumCapacity;

        //Columns
        /// <MetaDataID>{97896a64-b9d9-4a55-b2f7-5b3f91f8c001}</MetaDataID>
        private DataColumnSurrogate[] _dataColumnSurrogates;

        //Constraints
        /// <MetaDataID>{412e0646-fc73-4d9e-9986-177928aa043c}</MetaDataID>
        private ArrayList _uniqueConstraints; //An ArrayList of unique constraints : [constraintName]->[columnIndexes]->[IsPrimaryKey]->[extendedProperties]

        //ExtendedProperties
        /// <MetaDataID>{0067bc0b-04a9-4459-b46e-630c1dac635c}</MetaDataID>
        private Hashtable _extendedProperties;

        //Rows
        /// <MetaDataID>{5cf7726f-d637-4dd0-84f0-81b8288b144a}</MetaDataID>
        private BitArray _rowStates;  //The 4 rowstates[Unchanged, Added, Modified, Deleted] are represented with 2 bits. The length of the BitArray will be twice the size of the number of rows.
        /// <MetaDataID>{d21c1c68-64f9-44aa-8275-8231951663f2}</MetaDataID>
        private object[][] _records;  //As many object[] as there are number of columns. Always send 2 records for 1 row. TradeOff between memory vs. performance. Time intensive to find which records are modified.
        /// <MetaDataID>{31a860b7-59a9-48c7-8a2d-851f416fcf4d}</MetaDataID>
        private Hashtable _rowErrors = new Hashtable(); //Keep a map between the row index and the row error
        /// <MetaDataID>{25352211-b241-4ab9-9d71-08269ab56d1b}</MetaDataID>
        private Hashtable _colErrors = new Hashtable(); //Keep a map between the row index and the Arraylist of columns that are in error and the error strings.

        /*
            Constructs a DataTableSurrogate from a DataTable.
        */
        /// <MetaDataID>{a115a166-0ac7-4ae9-b7f3-e2bb7d6c5211}</MetaDataID>
        public DataTableSurrogate(DataTable dt)
        {
            if (dt == null)
            {
                throw new ArgumentNullException("The parameter dt is null");
            }

            _tableName = dt.TableName;
            _namespace = dt.Namespace;
            _prefix = dt.Prefix;
            _caseSensitive = dt.CaseSensitive;
            _locale = dt.Locale;
            _displayExpression = dt.DisplayExpression;
            _minimumCapacity = dt.MinimumCapacity;

            //Columns
            _dataColumnSurrogates = new DataColumnSurrogate[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                _dataColumnSurrogates[i] = new DataColumnSurrogate(dt.Columns[i]);
            }

            //Constraints
            _uniqueConstraints = GetUniqueConstraints(dt);

            //ExtendedProperties
            _extendedProperties = new Hashtable();
            if (dt.ExtendedProperties.Keys.Count > 0)
            {
                foreach (object propertyKey in dt.ExtendedProperties.Keys)
                {
                    _extendedProperties.Add(propertyKey, dt.ExtendedProperties[propertyKey]);
                }
            }

            //Rows
            if (dt.Rows.Count > 0)
            {
                _rowStates = new BitArray(dt.Rows.Count << 1);
                _records = new object[dt.Columns.Count][];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _records[i] = new object[dt.Rows.Count << 1];
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetRecords(dt.Rows[i], i << 1);
                }
            }
        }

        /*
            Constructs a DataTable from DataTableSurrogate. 
        */
        /// <MetaDataID>{4877be8c-02bd-4fc2-b855-754a746658d1}</MetaDataID>
        public DataTable ConvertToDataTable()
        {
            DataTable dt = new DataTable();
            ReadSchemaIntoDataTable(dt);
            ReadDataIntoDataTable(dt);
            return dt;
        }

        /*
            Reads the schema into the datatable from DataTableSurrogate. 
        */
        /// <MetaDataID>{1367feaf-a0fe-4285-aae8-86aa57ecec59}</MetaDataID>
        public void ReadSchemaIntoDataTable(DataTable dt)
        {
            if (dt == null)
            {
                throw new ArgumentNullException("The datatable parameter cannot be null");
            }

            dt.TableName = _tableName;
            dt.Namespace = _namespace;
            dt.Prefix = _prefix;
            dt.CaseSensitive = _caseSensitive;
            dt.Locale = _locale;
            dt.DisplayExpression = _displayExpression;
            dt.MinimumCapacity = _minimumCapacity;

            Debug.Assert(_dataColumnSurrogates != null);
            for (int i = 0; i < _dataColumnSurrogates.Length; i++)
            {
                DataColumnSurrogate dataColumnSurrogate = _dataColumnSurrogates[i];
                DataColumn dc = dataColumnSurrogate.ConvertToDataColumn();
                dt.Columns.Add(dc);
            }

            //UniqueConstraints
            SetUniqueConstraints(dt, _uniqueConstraints);

            //Extended properties
            Debug.Assert(_extendedProperties != null);
            if (_extendedProperties.Keys.Count > 0)
            {
                foreach (object propertyKey in _extendedProperties.Keys)
                {
                    dt.ExtendedProperties.Add(propertyKey, _extendedProperties[propertyKey]);
                }
            }
        }

        /*
            Reads the data into a DataTable from DataTableSurrogate. 
        */
        /// <MetaDataID>{4433373e-ef9f-4b89-94fd-717a24530eb4}</MetaDataID>
        public void ReadDataIntoDataTable(DataTable dt)
        {
            ReadDataIntoDataTable(dt, true);
        }

        /*
            Copies the rows into a DataTable from DataTableSurrogate. 
        */
        /// <MetaDataID>{d855520d-00ba-41d3-a472-6623a7eaeb8f}</MetaDataID>
        internal void ReadDataIntoDataTable(DataTable dt, bool suppressSchema)
        {
            if (dt == null)
            {
                throw new ArgumentNullException("The datatable parameter cannot be null");
            }
            Debug.Assert(IsSchemaIdentical(dt));

            //Suppress read-only and constraint rules while loading the data.
            ArrayList readOnlyList = null;
            ArrayList constraintRulesList = null;
            if (suppressSchema)
            {
                readOnlyList = SuppressReadOnly(dt);
                constraintRulesList = SuppressConstraintRules(dt);
            }

            //Read the rows
            if (_records != null && dt.Columns.Count > 0)
            {
                Debug.Assert(_records.Length > 0);
                int rowCount = _records[0].Length >> 1;
                for (int i = 0; i < rowCount; i++)
                {
                    ConvertToDataRow(dt, i << 1);
                }
            }

            //Reset read-only column and constraint rules back after loading the data.
            if (suppressSchema)
            {
                ResetReadOnly(dt, readOnlyList);
                ResetConstraintRules(dt, constraintRulesList);
            }
        }

        /*
            Gets unique constraints availabe on the datatable.
            ***Serialized unique constraints format : [constraintName]->[columnIndexes]->[IsPrimaryKey]->[extendedProperties]***
        */
        /// <MetaDataID>{e81adda3-3135-4c5f-abf2-01dc3bdf12db}</MetaDataID>
        private ArrayList GetUniqueConstraints(DataTable dt)
        {
            Debug.Assert(dt != null);

            ArrayList constraintList = new ArrayList();
            for (int i = 0; i < dt.Constraints.Count; i++)
            {
                Constraint c = dt.Constraints[i];
                UniqueConstraint uc = c as UniqueConstraint;
                if (uc != null)
                {
                    string constraintName = c.ConstraintName;
                    int[] colInfo = new int[uc.Columns.Length];
                    for (int j = 0; j < colInfo.Length; j++)
                    {
                        colInfo[j] = uc.Columns[j].Ordinal;
                    }

                    ArrayList list = new ArrayList();
                    list.Add(constraintName);
                    list.Add(colInfo);
                    list.Add(uc.IsPrimaryKey);
                    Hashtable extendedProperties = new Hashtable();
                    if (uc.ExtendedProperties.Keys.Count > 0)
                    {
                        foreach (object propertyKey in uc.ExtendedProperties.Keys)
                        {
                            extendedProperties.Add(propertyKey, uc.ExtendedProperties[propertyKey]);
                        }
                    }
                    list.Add(extendedProperties);

                    constraintList.Add(list);
                }
            }
            return constraintList;
        }

        /*
            Adds unique constraints to the table. The arraylist contains the serialized format of the unique constraints.
            ***Deserialize the unique constraints format : [constraintName]->[columnIndexes]->[IsPrimaryKey]->[extendedProperties]***
        */
        /// <MetaDataID>{38f85893-4185-4b63-81a4-6bf13fb0f079}</MetaDataID>
        private void SetUniqueConstraints(DataTable dt, ArrayList constraintList)
        {
            Debug.Assert(dt != null);
            Debug.Assert(constraintList != null);

            foreach (ArrayList list in constraintList)
            {
                Debug.Assert(list.Count == 4);
                string constraintName = (string)list[0];
                int[] keyColumnIndexes = (int[])list[1];
                bool isPrimaryKey = (bool)list[2];
                Hashtable extendedProperties = (Hashtable)list[3];

                DataColumn[] keyColumns = new DataColumn[keyColumnIndexes.Length];
                for (int i = 0; i < keyColumnIndexes.Length; i++)
                {
                    Debug.Assert(dt.Columns.Count > keyColumnIndexes[i]);
                    keyColumns[i] = dt.Columns[keyColumnIndexes[i]];
                }
                //Create the constraint.
                UniqueConstraint uc = new UniqueConstraint(constraintName, keyColumns, isPrimaryKey);
                //Extended Properties.
                Debug.Assert(extendedProperties != null);
                if (extendedProperties.Keys.Count > 0)
                {
                    foreach (object propertyKey in extendedProperties.Keys)
                    {
                        uc.ExtendedProperties.Add(propertyKey, extendedProperties[propertyKey]);
                    }
                }
                dt.Constraints.Add(uc);
            }
        }

        /*
            Sets  expression on the columns.
        */
        /// <MetaDataID>{1852c6c2-19e3-4f24-8db6-9c00a6f0e42a}</MetaDataID>
        internal void SetColumnExpressions(DataTable dt)
        {
            Debug.Assert(dt != null);

            Debug.Assert(_dataColumnSurrogates != null);
            Debug.Assert(dt.Columns.Count == _dataColumnSurrogates.Length);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn dc = dt.Columns[i];
                DataColumnSurrogate dataColumnSurrogate = _dataColumnSurrogates[i];
                dataColumnSurrogate.SetColumnExpression(dc);
            }
        }

        /*
            Gets the records from the rows.
        */
        /// <MetaDataID>{0f7b4a5b-3dd4-4f5b-87d1-cc81434088db}</MetaDataID>
        private void GetRecords(DataRow row, int bitIndex)
        {
            Debug.Assert(row != null);

            ConvertToSurrogateRowState(row.RowState, bitIndex);
            ConvertToSurrogateRecords(row, bitIndex);
            ConvertToSurrogateRowError(row, bitIndex >> 1);
        }

        /*
            Constructs the row, rowError and columnErrors.
        */
        /// <MetaDataID>{f394ad34-788f-43d9-a407-10aa69f17221}</MetaDataID>
        public DataRow ConvertToDataRow(DataTable dt, int bitIndex)
        {
            DataRowState rowState = ConvertToRowState(bitIndex);
            DataRow row = ConstructRow(dt, rowState, bitIndex);
            ConvertToRowError(row, bitIndex >> 1);
            return row;
        }

        /*
            Sets the two bits in the bitArray to represent the DataRowState.
            The 4 rowstates[Unchanged, Added, Modified, Deleted] are represented with 2 bits. The length of the BitArray will be twice the size of the number of rows.
            Serialozed rowstate format : [00]->UnChanged, [01]->Added, [10]->Modified, [11]->Deleted.
        */
        /// <MetaDataID>{75f6c0ac-01da-4391-b978-ff9a77af5e9a}</MetaDataID>
        private void ConvertToSurrogateRowState(DataRowState rowState, int bitIndex)
        {
            Debug.Assert(_rowStates != null);
            Debug.Assert(_rowStates.Length > bitIndex);

            switch (rowState)
            {
                case DataRowState.Unchanged:
                    _rowStates[bitIndex] = false;
                    _rowStates[bitIndex + 1] = false;
                    break;
                case DataRowState.Added:
                    _rowStates[bitIndex] = false;
                    _rowStates[bitIndex + 1] = true;
                    break;
                case DataRowState.Modified:
                    _rowStates[bitIndex] = true;
                    _rowStates[bitIndex + 1] = false;
                    break;
                case DataRowState.Deleted:
                    _rowStates[bitIndex] = true;
                    _rowStates[bitIndex + 1] = true;
                    break;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognized row state {0}", rowState));
            }
        }

        /*
            Constructs the RowState from the two bits in the bitarray.
            Deserialize rowstate format : [00]->UnChanged, [01]->Added, [10]->Modified, [11]->Deleted.
        */
        /// <MetaDataID>{b3e5c406-54f4-4656-8cae-93007747c276}</MetaDataID>
        private DataRowState ConvertToRowState(int bitIndex)
        {
            Debug.Assert(_rowStates != null);
            Debug.Assert(_rowStates.Length > bitIndex);

            bool b1 = _rowStates[bitIndex];
            bool b2 = _rowStates[bitIndex + 1];

            if (!b1 && !b2)
            {
                return DataRowState.Unchanged;
            }
            else if (!b1 && b2)
            {
                return DataRowState.Added;
            }
            else if (b1 && !b2)
            {
                return DataRowState.Modified;
            }
            else if (b1 && b2)
            {
                return DataRowState.Deleted;
            }
            else
            {
                throw new ArgumentException("Unrecognized bitpattern");
            }
        }

        /*
            Constructs surrogate records from the DataRow.
        */
        /// <MetaDataID>{2279cdc3-dcd5-4589-b6f6-3381c42d0108}</MetaDataID>
        private void ConvertToSurrogateRecords(DataRow row, int bitIndex)
        {
            Debug.Assert(row != null);
            Debug.Assert(_records != null);

            int colCount = row.Table.Columns.Count;
            DataRowState rowState = row.RowState;

            Debug.Assert(_records.Length == colCount);
            if (rowState != DataRowState.Added)
            {//Unchanged, modified, deleted     
                for (int i = 0; i < colCount; i++)
                {
                    Debug.Assert(_records[i].Length > bitIndex);
                    _records[i][bitIndex] = row[i, DataRowVersion.Original];
                }
            }

            if (rowState != DataRowState.Unchanged && rowState != DataRowState.Deleted)
            {//Added, modified state
                for (int i = 0; i < colCount; i++)
                {
                    Debug.Assert(_records[i].Length > bitIndex + 1);
                    _records[i][bitIndex + 1] = row[i, DataRowVersion.Current];
                }
            }
        }

        /*
            Constructs a DataRow from records[original and current] and adds the row to the DataTable rows collection.
        */
        /// <MetaDataID>{ec162384-4356-47aa-8946-b2c6a934071c}</MetaDataID>
        private DataRow ConstructRow(DataTable dt, DataRowState rowState, int bitIndex)
        {
            Debug.Assert(dt != null);
            Debug.Assert(_records != null);

            DataRow row = dt.NewRow();
            int colCount = dt.Columns.Count;

            Debug.Assert(_records.Length == colCount);
            switch (rowState)
            {
                case DataRowState.Unchanged:
                    for (int i = 0; i < colCount; i++)
                    {
                        Debug.Assert(_records[i].Length > bitIndex);
                        row[i] = _records[i][bitIndex]; //Original Record
                    }
                    dt.Rows.Add(row);
                    row.AcceptChanges();
                    break;
                case DataRowState.Added:
                    for (int i = 0; i < colCount; i++)
                    {
                        Debug.Assert(_records[i].Length > bitIndex + 1);
                        row[i] = _records[i][bitIndex + 1]; //Current Record
                    }
                    dt.Rows.Add(row);
                    break;
                case DataRowState.Modified:
                    for (int i = 0; i < colCount; i++)
                    {
                        Debug.Assert(_records[i].Length > bitIndex);
                        row[i] = _records[i][bitIndex]; //Original Record
                    }
                    dt.Rows.Add(row);
                    row.AcceptChanges();
                    row.BeginEdit();
                    for (int i = 0; i < colCount; i++)
                    {
                        Debug.Assert(_records[i].Length > bitIndex + 1);
                        row[i] = _records[i][bitIndex + 1]; //Current Record
                    }
                    row.EndEdit();
                    break;
                case DataRowState.Deleted:
                    for (int i = 0; i < colCount; i++)
                    {
                        Debug.Assert(_records[i].Length > bitIndex);
                        row[i] = _records[i][bitIndex]; //Original Record
                    }
                    dt.Rows.Add(row);
                    row.AcceptChanges();
                    row.Delete();
                    break;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognized row state {0}", rowState));
            }
            return row;
        }

        /*
            Constructs the surrogate rowerror, columnsInError and columnErrors.
        */
        /// <MetaDataID>{9b45f530-47e4-4267-a298-ecf43d76f656}</MetaDataID>
        private void ConvertToSurrogateRowError(DataRow row, int rowIndex)
        {
            Debug.Assert(row != null);
            Debug.Assert(_rowErrors != null);
            Debug.Assert(_colErrors != null);

            if (row.HasErrors)
            {
                _rowErrors.Add(rowIndex, row.RowError);
                DataColumn[] dcArr = row.GetColumnsInError();
                if (dcArr.Length > 0)
                {
                    int[] columnsInError = new int[dcArr.Length];
                    string[] columnErrors = new string[dcArr.Length];
                    for (int i = 0; i < dcArr.Length; i++)
                    {
                        columnsInError[i] = dcArr[i].Ordinal;
                        columnErrors[i] = row.GetColumnError(dcArr[i]);
                    }
                    ArrayList list = new ArrayList();
                    list.Add(columnsInError);
                    list.Add(columnErrors);
                    _colErrors.Add(rowIndex, list);
                }
            }
        }

        /*
            Set the row and columns in error.
        */
        /// <MetaDataID>{58978c4b-8115-4bb8-ad65-a182073dde3d}</MetaDataID>
        private void ConvertToRowError(DataRow row, int rowIndex)
        {
            Debug.Assert(row != null);
            Debug.Assert(_rowErrors != null);
            Debug.Assert(_colErrors != null);

            if (_rowErrors.ContainsKey(rowIndex))
            {
                row.RowError = (string)_rowErrors[rowIndex];
            }
            if (_colErrors.ContainsKey(rowIndex))
            {
                ArrayList list = (ArrayList)_colErrors[rowIndex];
                int[] columnsInError = (int[])list[0];
                string[] columnErrors = (string[])list[1];
                Debug.Assert(columnsInError.Length == columnErrors.Length);
                for (int i = 0; i < columnsInError.Length; i++)
                {
                    row.SetColumnError(columnsInError[i], columnErrors[i]);
                }
            }
        }

        /*
            Suppress the read-only property and returns an arraylist of read-only columns.
        */
        /// <MetaDataID>{dd84a4e1-d771-44ff-8f76-7c87bb021511}</MetaDataID>
        private ArrayList SuppressReadOnly(DataTable dt)
        {
            Debug.Assert(dt != null);
            ArrayList readOnlyList = new ArrayList();
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].Expression == String.Empty && dt.Columns[j].ReadOnly == true)
                {
                    readOnlyList.Add(j);
                }
            }
            return readOnlyList;
        }

        /*
            Suppress the foreign key constraint rules and returns an arraylist of the existing foreignkey constraint rules.
        */
        /// <MetaDataID>{0316d660-b879-462f-8c5e-bc79e817dd4b}</MetaDataID>
        private ArrayList SuppressConstraintRules(DataTable dt)
        {
            Debug.Assert(dt != null);
            ArrayList constraintRulesList = new ArrayList();
            DataSet ds = dt.DataSet;
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dtChild = ds.Tables[i];
                    for (int j = 0; j < dtChild.Constraints.Count; j++)
                    {
                        Constraint c = dtChild.Constraints[j];
                        if (c is ForeignKeyConstraint)
                        {
                            ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                            if (fk.RelatedTable == dt)
                            {
                                ArrayList list = new ArrayList();
                                list.Add(new int[] { i, j });
                                list.Add(new int[] { (int)fk.AcceptRejectRule, (int)fk.UpdateRule, (int)fk.DeleteRule });
                                constraintRulesList.Add(list);

                                fk.AcceptRejectRule = AcceptRejectRule.None;
                                fk.UpdateRule = Rule.None;
                                fk.DeleteRule = Rule.None;
                            }
                        }
                    }
                }
            }
            return constraintRulesList;
        }

        /*
            Resets the read-only columns on the datatable based on the input readOnly list.
        */
        /// <MetaDataID>{9633dcba-0ac5-40be-9be6-38cbd24ee80a}</MetaDataID>
        private void ResetReadOnly(DataTable dt, ArrayList readOnlyList)
        {
            Debug.Assert(dt != null);
            Debug.Assert(readOnlyList != null);

            DataSet ds = dt.DataSet;
            foreach (object o in readOnlyList)
            {
                int columnIndex = (int)o;
                Debug.Assert(dt.Columns.Count > columnIndex);
                dt.Columns[columnIndex].ReadOnly = true;
            }
        }

        /*
            Reset the foreignkey constraint rules on the datatable based on the input constraintRules list.
        */
        /// <MetaDataID>{af20948b-3e90-4e24-af27-082c3d64926c}</MetaDataID>
        private void ResetConstraintRules(DataTable dt, ArrayList constraintRulesList)
        {
            Debug.Assert(dt != null);
            Debug.Assert(constraintRulesList != null);

            DataSet ds = dt.DataSet;
            foreach (ArrayList list in constraintRulesList)
            {
                Debug.Assert(list.Count == 2);
                int[] indicesArr = (int[])list[0];
                int[] rules = (int[])list[1];

                Debug.Assert(indicesArr.Length == 2);
                int tableIndex = indicesArr[0];
                int constraintIndex = indicesArr[1];

                Debug.Assert(ds.Tables.Count > tableIndex);
                DataTable dtChild = ds.Tables[tableIndex];

                Debug.Assert(dtChild.Constraints.Count > constraintIndex);
                ForeignKeyConstraint fk = (ForeignKeyConstraint)dtChild.Constraints[constraintIndex];

                Debug.Assert(rules.Length == 3);
                fk.AcceptRejectRule = (AcceptRejectRule)rules[0];
                fk.UpdateRule = (Rule)rules[1];
                fk.DeleteRule = (Rule)rules[2];
            }
        }

        /*
            Checks whether the datatable schema matches with the surrogate schema.
        */
        /// <MetaDataID>{1f649dab-d44a-4c97-ac5c-03acb24461c8}</MetaDataID>
        private bool IsSchemaIdentical(DataTable dt)
        {
            Debug.Assert(dt != null);

            if (dt.TableName != _tableName || dt.Namespace != _namespace)
            {
                return false;
            }

            Debug.Assert(_dataColumnSurrogates != null);
            if (dt.Columns.Count != _dataColumnSurrogates.Length)
            {
                return false;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn dc = dt.Columns[i];
                DataColumnSurrogate dataColumnSurrogate = _dataColumnSurrogates[i];
                if (!dataColumnSurrogate.IsSchemaIdentical(dc))
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <MetaDataID>{3128f451-aa45-4750-83d5-a7c1fb24089e}</MetaDataID>
    [Serializable]
    class DataColumnSurrogate
    {
        /// <MetaDataID>{2137bfaf-0411-464e-bbc5-fb01f6aa01a1}</MetaDataID>
        private string _columnName;
        /// <MetaDataID>{aaf26432-c4ce-4c49-a328-16015054e692}</MetaDataID>
        private string _namespace;
        /// <MetaDataID>{2de7ca5f-91e5-46d4-a907-1f424cd796ef}</MetaDataID>
        private string _prefix;
        /// <MetaDataID>{03c86b38-e797-4086-8e8f-168d4c1853a7}</MetaDataID>
        private MappingType _columnMapping;
        /// <MetaDataID>{12041514-a2c3-48e2-9d61-9a8a686489d5}</MetaDataID>
        private bool _allowNull;
        /// <MetaDataID>{87d3fd41-1f29-412f-b625-5a4d1742f36c}</MetaDataID>
        private bool _autoIncrement;
        /// <MetaDataID>{e3f9a22e-5634-4bba-978b-8f288e102605}</MetaDataID>
        private long _autoIncrementStep;
        /// <MetaDataID>{b348d0cf-b636-4663-8176-b18e25fb5609}</MetaDataID>
        private long _autoIncrementSeed;
        /// <MetaDataID>{b8134f15-531b-4398-9b58-98a23647f90c}</MetaDataID>
        private string _caption;
        /// <MetaDataID>{30fee9f5-3b5e-4b13-bfe4-22e079300c88}</MetaDataID>
        private object _defaultValue;
        /// <MetaDataID>{d8087252-87f8-4aff-a7e2-ca396816f5f9}</MetaDataID>
        private bool _readOnly;
        /// <MetaDataID>{7d55cf96-1f73-4a9a-a59c-6dc0ca351dff}</MetaDataID>
        private int _maxLength;
        /// <MetaDataID>{0b7f5983-af70-4675-b748-a80efaae2503}</MetaDataID>
        private Type _dataType;
        /// <MetaDataID>{e2e6fb18-cc59-4c63-b396-bcd8d86e6086}</MetaDataID>
        private string _expression;

        //ExtendedProperties
        /// <MetaDataID>{b5f69258-f353-486e-b8f7-73e435560baa}</MetaDataID>
        private Hashtable _extendedProperties;

        /*
            Constructs a DataColumnSurrogate from a DataColumn.
        */
        /// <MetaDataID>{0672f298-637e-4d39-ad03-aeec00fedf08}</MetaDataID>
        public DataColumnSurrogate(DataColumn dc)
        {
            if (dc == null)
            {
                throw new ArgumentNullException("The datacolumn parameter is null");
            }
            _columnName = dc.ColumnName;
            _namespace = dc.Namespace;
            _dataType = dc.DataType;
            _prefix = dc.Prefix;
            _columnMapping = dc.ColumnMapping;
            _allowNull = dc.AllowDBNull;
            _autoIncrement = dc.AutoIncrement;
            _autoIncrementStep = dc.AutoIncrementStep;
            _autoIncrementSeed = dc.AutoIncrementSeed;
            _caption = dc.Caption;
            _defaultValue = dc.DefaultValue;
            _readOnly = dc.ReadOnly;
            _maxLength = dc.MaxLength;
            _expression = dc.Expression;

            //ExtendedProperties
            _extendedProperties = new Hashtable();
            if (dc.ExtendedProperties.Keys.Count > 0)
            {
                foreach (object propertyKey in dc.ExtendedProperties.Keys)
                {
                    _extendedProperties.Add(propertyKey, dc.ExtendedProperties[propertyKey]);
                }
            }
        }

        /*
            Constructs a DataColumn from DataColumnSurrogate.
        */
        /// <MetaDataID>{8b430eb3-941d-49f3-9202-ced094ce52ab}</MetaDataID>
        public DataColumn ConvertToDataColumn()
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = _columnName;
            dc.Namespace = _namespace;
            dc.DataType = _dataType;
            dc.Prefix = _prefix;
            dc.ColumnMapping = _columnMapping;
            dc.AllowDBNull = _allowNull;
            dc.AutoIncrement = _autoIncrement;
            dc.AutoIncrementStep = _autoIncrementStep;
            dc.AutoIncrementSeed = _autoIncrementSeed;
            dc.Caption = _caption;
            dc.DefaultValue = _defaultValue;
            dc.ReadOnly = _readOnly;
            dc.MaxLength = _maxLength;
            //dc.Expression = _expression;

            //Extended properties
            Debug.Assert(_extendedProperties != null);
            if (_extendedProperties.Keys.Count > 0)
            {
                foreach (object propertyKey in _extendedProperties.Keys)
                {
                    dc.ExtendedProperties.Add(propertyKey, _extendedProperties[propertyKey]);
                }
            }
            return dc;
        }

        /*
            Set expression on the DataColumn.
        */
        /// <MetaDataID>{330ab4c2-a3e7-4266-a700-b816a6c086c3}</MetaDataID>
        internal void SetColumnExpression(DataColumn dc)
        {
            Debug.Assert(dc != null);

            if (_expression != null && !_expression.Equals(String.Empty))
            {
                dc.Expression = _expression;
            }
        }

        /*
            Checks whether the column schema is identical. Marked internal as the DataTableSurrogate objects needs to have access to this object.
            Note: ReadOnly is not checked here as we suppress readonly when reading data.
        */
        /// <MetaDataID>{a5b6e3d9-69f5-49bc-aeb8-ced6198de115}</MetaDataID>
        internal bool IsSchemaIdentical(DataColumn dc)
        {
            Debug.Assert(dc != null);
            if ((dc.ColumnName != _columnName) || (dc.Namespace != _namespace) || (dc.DataType != _dataType) ||
                (dc.Prefix != _prefix) || (dc.ColumnMapping != _columnMapping) ||
                (dc.ColumnMapping != _columnMapping) || (dc.AllowDBNull != _allowNull) ||
                (dc.AutoIncrement != _autoIncrement) || (dc.AutoIncrementStep != _autoIncrementStep) ||
                (dc.AutoIncrementSeed != _autoIncrementSeed) || (dc.Caption != _caption) ||
                (!(AreDefaultValuesEqual(dc.DefaultValue, _defaultValue))) || (dc.MaxLength != _maxLength) ||
                (dc.Expression != _expression))
            {
                return false;
            }
            return true;
        }

        /*
            Checks whether the default boxed objects are equal.
        */
        /// <MetaDataID>{40ee3c88-7aca-4b6e-a88a-cd7c9712a3d1}</MetaDataID>
        internal static bool AreDefaultValuesEqual(object o1, object o2)
        {
            if (o1 == null && o2 == null)
            {
                return true;
            }
            else if (o1 == null || o2 == null)
            {
                return false;
            }
            else
            {
                return o1.Equals(o2);
            }
        }
    }
}
