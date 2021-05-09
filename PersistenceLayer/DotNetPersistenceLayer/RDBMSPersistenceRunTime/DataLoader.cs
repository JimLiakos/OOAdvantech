using System;
using System.Collections.Generic;
using System.Text;
using SubDataNodeIdentity = System.Guid;
using ComparisonTermsType = OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType;
using PartTypeName = System.String;
using CreatorIdentity = System.String;
using OOAdvantech.Transactions;
using System.Linq;
namespace OOAdvantech.RDBMSPersistenceRunTime
{
    using MetaDataRepository.ObjectQueryLanguage;
    using OOAdvantech.MetaDataRepository;
    /// <MetaDataID>{8386fdac-845d-46c8-9ba8-2ab540b704e4}</MetaDataID>
    public class DataLoader : PersistenceLayerRunTime.StorageDataLoader
    {

        ///// <MetaDataID>{3f566d9a-a67d-4df7-aa63-a397c6d341ef}</MetaDataID>
        //protected override bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode)
        //{
        //    return true;
        //}


        #region RowRemove code
        ///// <exclude>Excluded</exclude>
        //List<string> _RowRemoveColumns;
        ///// <MetaDataID>{9a8ef256-8802-4240-8539-004322cc1bd3}</MetaDataID>
        //public override List<string> RowRemoveColumns
        //{
        //    get 
        //    {
        //        if (_RowRemoveColumns == null)
        //        {
        //            _RowRemoveColumns = new List<string>();
        //            return _RowRemoveColumns;


        //            foreach (var searchCondition in DataNode.SearchConditions)
        //            {
        //                if ((DataNode.FilterNotActAsLoadConstraint && searchCondition == DataNode.SearchCondition) ||
        //                       (searchCondition != DataNode.SearchCondition&& DataNode.SearchConditions.Contains(searchCondition)))
        //                {
        //                    if (searchCondition != null)
        //                        _RowRemoveColumns.Add(DataSource.GetRowRemoveColumnName(DataNode, searchCondition));
        //                }
        //            }
        //            foreach (var dataLoader in HostedDataDataLoaders)
        //            {
        //                foreach (string rowRemoveColumn in dataLoader.RowRemoveColumns)
        //                    _RowRemoveColumns.Add(rowRemoveColumn);
        //            }
        //        }
        //        return _RowRemoveColumns; 
        //    }
        //}
        #endregion
        /// <MetaDataID>{8682ac6a-406a-44d1-b01b-94cd3dade09d}</MetaDataID>
        public override List<Criterion> GlobalResolveCriterions
        {                                                                                                                                        
            get
            {
                return base.GlobalResolveCriterions;
                List<Criterion> globalResolveCriterions = new List<Criterion>();
                foreach (var criterion in base.GlobalResolveCriterions)
                    globalResolveCriterions.Add(criterion);
                foreach (var criterion in base.LocalOnMemoryResolvedCriterions.Keys)
                    globalResolveCriterions.Add(criterion);
                return globalResolveCriterions;
            }
        }
        /// <MetaDataID>{7152ab1a-0502-4260-85b1-89bb861bcc6f}</MetaDataID>
        protected override object GetObjectFromIdentity(PersistenceLayer.ObjectID objectIdentity)
        {
            object @object = null;
            LoadedObjects.TryGetValue(objectIdentity, out @object);
            return @object;
        }
        /// <exclude>Excluded</exclude>
        bool? _DataLoadedInParentDataSource;
        /// <MetaDataID>{f72b31c2-4e0c-47f7-b93d-5b09456ddbc6}</MetaDataID>
        public override bool DataLoadedInParentDataSource
        {
            get
            {

                if (!_DataLoadedInParentDataSource.HasValue)
                {
                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {

                        foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                        {
                            if (storageCell is MetaDataRepository.StorageCellReference)
                            {
                                ///Out storage relation 
                                _DataLoadedInParentDataSource = false;
                                return _DataLoadedInParentDataSource.Value;
                            }

                        }
                        if (DataNode.RealParentDataNode.DataSource.HasOutObjectContextData)
                        {
                            ///Out storage relation 
                            _DataLoadedInParentDataSource = false;
                            return _DataLoadedInParentDataSource.Value;
                        }
                    }
                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                        !(DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany &&
                          (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass == null)
                    {
                        /// One to One and meny to one relations data loaded in parent
                        if (DataLoaderMetadata.StorageCells.Count > 0 && GetDataLoader(DataNode.RealParentDataNode) != null && GetDataLoader(DataNode.RealParentDataNode).RetrievesData)
                        {
                            _DataLoadedInParentDataSource = true;
                            DataNode.DataSource.DataLoadedInParentDataSource = true;
                        }
                        else
                            _DataLoadedInParentDataSource = false;

                    }
                    else if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                          (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass != null &&
                         DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation == (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
                    {
                        /// Data loader load assocition class data
                        if (GetDataLoader(DataNode.RealParentDataNode) != null && GetDataLoader(DataNode.RealParentDataNode).RetrievesData)
                        {
                            _DataLoadedInParentDataSource = true;
                            DataNode.DataSource.DataLoadedInParentDataSource = true;
                        }
                        else
                            _DataLoadedInParentDataSource = false;
                    }
                    else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                    {
                        ///all valuetype data load in parent dataloader
                        _DataLoadedInParentDataSource = true;
                        DataNode.DataSource.DataLoadedInParentDataSource = true;
                    }
                    else
                        _DataLoadedInParentDataSource = false;
                }
                return _DataLoadedInParentDataSource.Value;
            }
            set
            {
                _DataLoadedInParentDataSource = value;
                DataNode.DataSource.DataLoadedInParentDataSource = value;
            }
        }

        /// <MetaDataID>{501353c4-20b1-42ba-bb78-4719e6cb4d51}</MetaDataID>
        public override bool CriterionCanBeResolvedFromNativeSystem(Criterion criterion)
        {

            if (criterion.OverridenComparisonOperator != null)
                return false;
            else
                return RDBMSSQLScriptGenarator.CriterionResolvedFromNativeSystem(criterion);
        }

        /// <exclude>Excluded</exclude>
        private MetaDataRepository.Classifier _Classifier;
        /// <summary>
        /// This property defines classifier.
        /// It is corresponding of the data node classifier and keeps the mapping data of classifier with the data object of RDBMS. 
        /// </summary>
        /// <MetaDataID>{ADD47CF9-27E6-434E-928E-495C1AEE5FEE}</MetaDataID>
        public override MetaDataRepository.Classifier Classifier
        {
            get
            {
                if (_Classifier == null)
                {
                    foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(storageCell))
                            throw new System.Exception("System can't access classifier when data loader live in different process, than the storage.");

                        if (storageCell.Type != null)
                        {
                            if (storageCell.Type.Identity == DataNode.Classifier.Identity)
                            {
                                _Classifier = storageCell.Type;
                            }
                            else
                            {
                                if (DataNode.Classifier is MetaDataRepository.Structure && (DataNode.Classifier as MetaDataRepository.Structure).Persistent)
                                    if (storageCell.Type.Identity == DataNode.RealParentDataNode.Classifier.Identity)
                                    {
                                        _Classifier = storageCell.Type;
                                        return _Classifier;
                                    }
                                foreach (MetaDataRepository.Classifier classifier in storageCell.Type.GetAllGeneralClasifiers())
                                {
                                    if (classifier.Identity == DataNode.Classifier.Identity)
                                    {
                                        _Classifier = classifier;
                                        break;
                                    }
                                    else
                                    {
                                        if (DataNode.Classifier is MetaDataRepository.Structure && (DataNode.Classifier as MetaDataRepository.Structure).Persistent)
                                            if (classifier.Identity == DataNode.RealParentDataNode.Classifier.Identity)
                                            {
                                                _Classifier = classifier;
                                                return _Classifier;
                                            }
                                    }
                                }
                            }
                            break;
                        }

                    }
                    if (_Classifier == null)
                    {
                        _Classifier = (Storage as OOAdvantech.RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.Classifier) as MetaDataRepository.Classifier;
                        if (_Classifier != null)
                            return _Classifier;
                        throw new System.Exception("System can't find RDBMS meta object for " + DataNode.Classifier.FullName);
                    }
                }
                return _Classifier;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> _GroupByKeyRelationColumns;
        /// <MetaDataID>{b0b9eae1-9c7d-44db-bd53-1c56b0f11b84}</MetaDataID>
        public override System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> GroupByKeyRelationColumns
        {
            get
            {

                if (_GroupByKeyRelationColumns == null)
                {
                    _GroupByKeyRelationColumns = new Dictionary<System.Guid, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>>();
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                        {
                            if (dataNode.Type == DataNode.DataNodeType.Object)
                            {
                                foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(dataNode).ObjectIdentityTypes)// (dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.StorageIdentity] as StorageDataLoader).DataLoaderMetadata.StorageCells)
                                {

                                    //List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                    //foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    //{
                                    //    if (GetDataLoader(dataNode).DataLoadedInParentDataSource)
                                    //        parts.Add(new MetaDataRepository.IdentityPart(dataNode.Alias + "_" + part.Name, part.PartTypeName, part.Type));
                                    //    else
                                    //        parts.Add(new MetaDataRepository.IdentityPart(part.Name, part.PartTypeName, part.Type));
                                    //}
                                    //OOAdvantech.MetaDataRepository.ObjectIdentityType identityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                                    List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                    if (!_GroupByKeyRelationColumns.ContainsKey(dataNode.Identity))
                                        _GroupByKeyRelationColumns[dataNode.Identity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>();
                                    if (!_GroupByKeyRelationColumns[dataNode.Identity].ContainsKey(objectIdentityType))
                                    {
                                        List<string> groupingColumnsNames = new List<string>();
                                        List<string> groupedDataColumnsNames = new List<string>();
                                        foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                        {
                                            groupingColumnsNames.Add(dataNode.Alias + "_" + identityPart.PartTypeName);
                                            if ((dataNode.DataSource as StorageDataSource).DataLoadedInParentDataSource)
                                                groupedDataColumnsNames.Add(dataNode.Alias + "_" + identityPart.PartTypeName);
                                            else
                                                groupedDataColumnsNames.Add(identityPart.PartTypeName);
                                            parts.Add(new OOAdvantech.MetaDataRepository.IdentityPart(dataNode.Alias + "_" + identityPart.PartTypeName, identityPart.Name, identityPart.Type));
                                        }
                                        MetaDataRepository.ObjectIdentityType keyObjectIdentityType = new MetaDataRepository.ObjectIdentityType(parts);
                                        ObjectKeyRelationColumns keyReferenceColumn = new ObjectKeyRelationColumns(keyObjectIdentityType, groupingColumnsNames, groupedDataColumnsNames, dataNode.DataSource);
                                        _GroupByKeyRelationColumns[dataNode.Identity][keyObjectIdentityType] = keyReferenceColumn;
                                    }
                                }
                            }
                        }
                    }
                }
                return _GroupByKeyRelationColumns;
            }
        }

        /// <MetaDataID>{9610b2e7-ae4f-40f1-abdf-833f55025f69}</MetaDataID>
        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {
            ObjectsUnderTransactionData = new ObjectUnderTransactionDataManager(this);
            SqlScriptsBuilder = new SQLScriptsBuilder(this);
        }

        ///// <MetaDataID>{87024c24-82b4-4001-b997-66a48a7c147b}</MetaDataID>
        //internal SQLFilterScriptBuilder SQLFilterScriptBulder;

        /// <MetaDataID>{f47eaddc-ec0d-4921-bba0-520ce486e262}</MetaDataID>
        public override void OnAllQueryDataLoaded()
        {
            if (ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction == null)
                return;

            // System.Data.Common.DbConnection connection = (Storage as Storage).StorageDataBase.SchemaUpdateConnection;
            var connection = (Storage as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            lock (connection)
            {
                bool closeConnection = false;
                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                {
                    connection.Open();
                    closeConnection = true;
                }
                //else
                //{
                //    connection.Close();
                //    connection.Open();
                //}
                var command = connection.CreateCommand();

                foreach (string dropSript in RDBMSSQLScriptGenarator.GetDropTableScript(ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TableName, RDBMSDataObjects.TableType.TemporaryTable))
                {
                    if (ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.TemporaryTableTransfered)
                    {
                        command.CommandText = dropSript;
                        command.ExecuteNonQuery();
                    }
                }

                foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in LocalResolvedCriterions.Keys)
                {
                    if (criterion.CriterionType == ComparisonTermsType.CollectionContainsAnyAll)
                    {
                        object tableDropped = null;
                        if (!(criterion.Tag as System.Collections.Generic.Dictionary<string, object>).TryGetValue("TableDropped", out  tableDropped))
                        {
                            foreach (string dropSript in RDBMSSQLScriptGenarator.GetDropTableScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName, RDBMSDataObjects.TableType.TemporaryTable))
                            {
                                command.CommandText = dropSript;
                                command.ExecuteNonQuery();

                                //if (dropTemTables != null)
                                //    dropTemTables += "\r\n";
                                //dropTemTables += dropSript; //RDBMSSQLScriptGenarator.GetDropTableScript((criterion.ComparisonTerms[1] as ParameterComparisonTerm).ParameterName, RDBMSDataObjects.TableType.TemporaryTable);
                            }
                            (criterion.Tag as System.Collections.Generic.Dictionary<string, object>)["TableDropped"] = true;
                        }
                    }
                }


                foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                {
                    if (subDataNode.ThroughRelationTable && subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        IDataTable tableWithRelationChanges = ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(subDataNode);
                        if (tableWithRelationChanges != null && GetStorageCellsLinks(subDataNode).Count > 0)
                        {
                            /* if (ManyToManyReltionsSQLscripts[subDataNode] != null)*/

                            foreach (string dropSript in RDBMSSQLScriptGenarator.GetDropTableScript(tableWithRelationChanges.TableName, RDBMSDataObjects.TableType.TemporaryTable))
                            {
                                command.CommandText = dropSript;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                if (closeConnection)
                    connection.Close();
            }
            base.OnAllQueryDataLoaded();
        }



        /// <MetaDataID>{d8c5f84b-6906-4942-a57f-bc76134d8e61}</MetaDataID>
        internal RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return (Storage as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary;
            }
        }

        /// <MetaDataID>{658c21fd-243e-4b2d-972e-f5d1ff469c2f}</MetaDataID>
        public RDBMSDataObjects.IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator
        {
            get
            {
                return (Storage as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSQLScriptGenarator;
            }
        }



        /// <MetaDataID>{b6747566-12c7-4873-9e03-69cc552ed989}</MetaDataID>
        internal string GetArithmeticTermSqlScript(ArithmeticExpression arithmeticTermExpression)
        {
            if (arithmeticTermExpression is CompositeArithmeticExpression)
            {
                CompositeArithmeticExpression arithmeticExpression = arithmeticTermExpression as CompositeArithmeticExpression;
                string operatorSign = null;
                switch (arithmeticExpression.Operator)
                {
                    case ArithmeticOperator.Add:
                        {
                            operatorSign = "+";
                            break;
                        }
                    case ArithmeticOperator.Subtract:
                        {
                            operatorSign = "-";
                            break;
                        }
                    case ArithmeticOperator.Multiply:
                        {
                            operatorSign = "*";
                            break;
                        }
                    case ArithmeticOperator.Divide:
                        {
                            operatorSign = "/";
                            break;
                        }

                    default:
                        break;
                }
                return "(" + GetArithmeticTermSqlScript(arithmeticExpression.Left) + operatorSign + GetArithmeticTermSqlScript(arithmeticExpression.Right) + ")";
            }
            if (arithmeticTermExpression is ScalarFromLiteral)
            {
                return (arithmeticTermExpression as ScalarFromLiteral).LiteralValue.ToString();
            }
            if (arithmeticTermExpression is ScalarFromData)
            {
                DataNode aggregateExpressionDataNode = (arithmeticTermExpression as ScalarFromData).DataNode;
                //RDBMSMetaDataRepository.Attribute rdbmsAttribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(aggregateExpressionDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                string aggregateExpression = null;
                if (aggregateExpressionDataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                        (aggregateExpressionDataNode.RealParentDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                {
                    //RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(aggregateExpressionDataNode.RealParentDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
                    //string columnName = attribute.GetAttributeColumnName(aggregateExpressionDataNode.AssignedMetaObject as MetaDataRepository.Attribute);
                    //if (DataNode.Type == DataNode.DataNodeType.Group)
                    //else
                    //{
                    //    aggregateExpression += GetDataTreeUniqueColumnName(aggregateExpressionDataNode);// GetSQLScriptForName(GetDataLoader(aggregateExpressionDataNode).DataNode.Alias + "_" + GetDataLoader(aggregateExpressionDataNode.ParentDataNode).GetColumnName(aggregateExpressionDataNode));
                    //}
                    aggregateExpression += SqlScriptsBuilder.GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(aggregateExpressionDataNode));

                }
                else if (aggregateExpressionDataNode is MetaDataRepository.ObjectQueryLanguage.AggregateExpressionDataNode)
                {
                    aggregateExpression += SqlScriptsBuilder.GetSQLScriptForName(DataNode.Alias) + "." + SqlScriptsBuilder.GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(aggregateExpressionDataNode));
                }
                else
                    aggregateExpression += SqlScriptsBuilder.GetSQLScriptForName(DataNode.Alias) + "." + SqlScriptsBuilder.GetSQLScriptForName((aggregateExpressionDataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name);

                return "(" + aggregateExpression + ")";
            }
            return "";
        }


        /// <MetaDataID>{04908cab-a15d-4826-9372-c882719a0ef0}</MetaDataID>
        protected override List<string> DataColumnNames
        {
            get
            {
                List<string> columns = new List<string>();
                foreach (DataColumn column in ClassifierDataColumns)
                {
                    string columnName = null;
                    if (column.Alias != null)
                        columnName = column.Alias;
                    else
                        columnName = column.Name;

                    columns.Add(columnName);
                    if (column.MappedObject != null)
                    {
                        foreach (var dataNode in DataNode.SubDataNodes)
                        {
                            if (dataNode.AssignedMetaObject != null && dataNode.AssignedMetaObject.Identity == column.MappedObject.Identity)
                            {
                                if (!string.IsNullOrEmpty(DataSource.GetColumnName(dataNode)) && DataSource.GetColumnName(dataNode) != columnName)
                                    DataNodeColumnsNames[columnName] = DataSource.GetColumnName(dataNode);
                            }
                        }
                    }
                }
                if (!DataLoadedInParentDataSource)
                    columns.Add("OSM_StorageIdentity");
                foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Object &&
                        !(subDataNode.ValueTypePath.Count > 0) &&// The columns for value type subDataNode included in  ClassifierDataColumns
                        GetDataLoader(subDataNode) != null && (GetDataLoader(subDataNode).DataLoadedInParentDataSource))
                        columns.AddRange((GetDataLoader(subDataNode) as DataLoader).DataColumnNames);

                }

                return columns;

            }
        }


        /// <MetaDataID>{78d0302d-fd37-4d8d-8cd4-75e7ccf72100}</MetaDataID>
        System.Collections.Generic.Dictionary<PersistenceLayer.ObjectID, object> LoadedObjects = new Dictionary<PersistenceLayer.ObjectID, object>();

        /// <summary>
        /// Collection of attribute that defines the access path to retrieve the value type value
        /// </summary>
        /// <MetaDataID>{6e04d767-d5ba-4d04-bf92-1f04dfe07cf4}</MetaDataID>
        DotNetMetaDataRepository.Attribute[] ValueTypeAccessPath;


        /// <summary>
        /// This member keeps dictionaries with attributes indices and dictionaries with association end indices on data table row.
        /// </summary>
        /// <MetaDataID>{5fd4946f-d6ee-422d-8d04-b610ecd58e1b}</MetaDataID>
        ClassMembersMappedColumnsIndices ClassMembersMappedColumnsIndices = null;


        ///<summary>
        ///Defines the storageCell of last row.
        ///Data loader cashing storage cell to increase perfomance
        ///</summary>
        /// <MetaDataID>{b53cef6e-a143-43ef-94bc-a95f79922798}</MetaDataID>
        RDBMSMetaDataRepository.StorageCell LastStorageCell = null;

        ///<summary>
        ///Defines the storageCell class type of last row.
        ///Data loader cashing type to increase perfomance
        ///</summary>
        /// <MetaDataID>{e28b657f-fa5c-41ae-ab0f-110acbfb458b}</MetaDataID>
        System.Type LastStorageCellType = null;

        public override object GetObjectIdentity(PersistenceLayer.StorageInstanceRef.ObjectSate row)
        {
            string columnPrefix = null;
            #region Build column prefix

            DataNode nonValueTypeDataNode = DataNode;
            while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;

            if (nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                columnPrefix = nonValueTypeDataNode.Alias + "_";

            PersistenceLayerRunTime.StorageInstanceRef.ObjectSate StorageInstance = row;

            var storageCellIDCalue = StorageInstance[columnPrefix + "StorageCellID"];
            if (storageCellIDCalue is DBNull || storageCellIDCalue == null)
                return null;

            int storageCellID = System.Convert.ToInt32(StorageInstance[columnPrefix + "StorageCellID"]);
            if (storageCellID != -1)
            {
                GetRowStorageCell(storageCellID);
                if (LastStorageCell == null)
                    throw new System.Exception("System can't retrieve storage cell");
            }
            ObjectID objectID = null;
            if (storageCellID == -1)
            {
                object fieldValue = null;
                #region New object in transaction context
                fieldValue = StorageInstance[columnPrefix + _ObjectIdentityTypes[_ObjectIdentityTypes.IndexOf(NewObjectIdentityType)].Parts[0].Name];
                objectID = new ObjectID((Guid)TypeDictionary.Convert(fieldValue, typeof(System.Guid)), 0);
                return objectID;
                #endregion

            }
            else
            {

                var objectIdentityType = _ObjectIdentityTypes[_ObjectIdentityTypes.IndexOf(LastStorageCell.ObjectIdentityType)];
                object[] partValues = new object[objectIdentityType.PartsCount];
                int i = 0;
                foreach (var part in objectIdentityType.Parts)
                {
                    object fieldValue = StorageInstance[columnPrefix + part.Name];
                    if (fieldValue is System.DBNull)
                        return null;
                    if (fieldValue == null)
                        return null;
                    fieldValue = TypeDictionary.Convert(fieldValue, part.Type);
                    partValues[i++] = fieldValue;
                }
                objectID = new ObjectID(LastStorageCell.ObjectIdentityType, partValues, storageCellID);
                return objectID;

            }

            #endregion

        }
        /// <MetaDataID>{68d9c74c-cfc3-4018-b88d-2c14a33a4afa}</MetaDataID>
        public override object GetObject(PersistenceLayerRunTime.StorageInstanceRef.ObjectSate row, out bool loadObjectLinks)
        {

            string columnPrefix = null;

            #region Build column prefix

            DataNode nonValueTypeDataNode = DataNode;
            while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;

            if (nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                columnPrefix = nonValueTypeDataNode.Alias + "_";

            #endregion

            if (ClassMembersMappedColumnsIndices == null)
                ClassMembersMappedColumnsIndices = BuildClassMembersMappedColumnsIndices(row.ColumnsIndices, columnPrefix);
            row.Tag = ClassMembersMappedColumnsIndices;
            lock (ObjectStorage)
            {

                loadObjectLinks = false;
                PersistenceLayerRunTime.StorageInstanceRef.ObjectSate StorageInstance = row;
                var storageCellIDCalue = StorageInstance[columnPrefix + "StorageCellID"];
                if (storageCellIDCalue is DBNull || storageCellIDCalue==null)
                    return null;
                int storageCellID = System.Convert.ToInt32(StorageInstance[columnPrefix + "StorageCellID"]);
                if (storageCellID != -1)
                {
                    GetRowStorageCell(storageCellID);
                    if (LastStorageCell == null)
                        throw new System.Exception("System can't retrieve storage cell");
                }
                object _object;

                ObjectID objectID = null;
                if (storageCellID == -1)
                {
                    object fieldValue = null;
                    #region New object in transaction context

                    fieldValue = StorageInstance[columnPrefix + _ObjectIdentityTypes[_ObjectIdentityTypes.IndexOf(NewObjectIdentityType)].Parts[0].Name];

                    _object = GetObjectUnderTransaction(new ObjectID((Guid)TypeDictionary.Convert(fieldValue, typeof(System.Guid)), 0));

                    if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                   (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {
                        if (DataNode.ParentDataNode != null)
                        {

                            if (_object != null)
                            {
                                if (ValueTypeAccessPath == null)
                                {
                                    int count = 0;
                                    DataNode dataNode = DataNode;
                                    while (dataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
                                    {
                                        count++;
                                        dataNode = dataNode.ParentDataNode;
                                        if (dataNode == null)
                                        {
                                            count = 0;
                                            break;
                                        }
                                        if (!(dataNode.Classifier is MetaDataRepository.Classifier))
                                        {
                                            count = 0;
                                            break;
                                        }
                                    }
                                    ValueTypeAccessPath = new DotNetMetaDataRepository.Attribute[count];
                                    if (count > 0)
                                    {
                                        dataNode = DataNode;
                                        while (dataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
                                        {
                                            ValueTypeAccessPath[--count] = dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute;
                                            dataNode = dataNode.ParentDataNode;
                                        }
                                    }
                                }

                                foreach (DotNetMetaDataRepository.Attribute attribute in ValueTypeAccessPath)
                                {
                                    if (attribute.FastPropertyAccessor != null)
                                        _object = attribute.FastPropertyAccessor.GetValue(_object);
                                    else if (attribute.FastFieldAccessor != null)
                                        _object = attribute.FastFieldAccessor.GetValue(_object);
                                    else
                                    {
                                        _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, columnPrefix, default(string), LastStorageCell);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, columnPrefix, default(string), LastStorageCell);
                            }
                        }
                        else
                            _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, columnPrefix, default(string), LastStorageCell);


                    }
                    #endregion

                    return _object;

                }
                else
                {

                    var objectIdentityType = _ObjectIdentityTypes[_ObjectIdentityTypes.IndexOf(LastStorageCell.ObjectIdentityType)];
                    object[] partValues = new object[objectIdentityType.PartsCount];
                    int i = 0;
                    foreach (var part in objectIdentityType.Parts)
                    {
                        object fieldValue = StorageInstance[columnPrefix + part.Name];
                        if (fieldValue is System.DBNull)
                            return null;
                        if (fieldValue == null)
                            return null;
                        fieldValue = TypeDictionary.Convert(fieldValue, part.Type);
                        partValues[i++] = fieldValue;
                    }
                    objectID = new ObjectID(LastStorageCell.ObjectIdentityType, partValues, storageCellID);

                    if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                       (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {

                        #region Gets value type value
                        if (DataNode.ParentDataNode != null)
                        {
                            StorageInstanceRef storageInstanceRef = (ObjectStorage as PersistenceLayerRunTime.ObjectStorage).OperativeObjectCollections[LastStorageCellType][objectID] as StorageInstanceRef;
                            if (storageInstanceRef != null)
                            {

                                if (ValueTypeAccessPath == null)
                                {
                                    #region Builds value type acces path

                                    int count = 0;
                                    DataNode dataNode = DataNode;
                                    while (dataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
                                    {
                                        count++;
                                        dataNode = dataNode.ParentDataNode;
                                        if (dataNode == null)
                                        {
                                            count = 0;
                                            break;
                                        }
                                        if (!(dataNode.Classifier is MetaDataRepository.Classifier))
                                        {
                                            count = 0;
                                            break;
                                        }
                                    }
                                    ValueTypeAccessPath = new DotNetMetaDataRepository.Attribute[count];
                                    if (count > 0)
                                    {
                                        dataNode = DataNode;
                                        while (dataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
                                        {
                                            ValueTypeAccessPath[--count] = dataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute;
                                            dataNode = dataNode.ParentDataNode;
                                        }
                                    }

                                    #endregion
                                }

                                #region retrieves value type value

                                _object = storageInstanceRef.MemoryInstance;
                                foreach (DotNetMetaDataRepository.Attribute attribute in ValueTypeAccessPath)
                                {
                                    if (attribute.FastPropertyAccessor != null)
                                        _object = attribute.FastPropertyAccessor.GetValue(_object);
                                    else if (attribute.FastFieldAccessor != null)
                                        _object = attribute.FastFieldAccessor.GetValue(_object);
                                    else
                                    {
                                        _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, columnPrefix, default(string), LastStorageCell);
                                        break;
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, columnPrefix, default(string), LastStorageCell);
                            }
                        }
                        else
                            _object = StorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, row, columnPrefix, default(string), LastStorageCell);

                        #endregion

                    }
                    else
                    {
                        StorageInstanceRef storageInstanceRef = (ObjectStorage as PersistenceLayerRunTime.ObjectStorage).OperativeObjectCollections[LastStorageCellType][objectID] as StorageInstanceRef;
                        if (storageInstanceRef != null)
                        {
                            _object = storageInstanceRef.MemoryInstance;
                        }
                        else
                        {
                            #region Moves storageInstance to operate mode
                            storageInstanceRef = (StorageInstanceRef)(ObjectStorage as ObjectStorage).CreateStorageInstanceRef(AccessorBuilder.CreateInstance(LastStorageCellType), objectID, LastStorageCell);
                            storageInstanceRef.DbDataRecord = StorageInstance;
                            storageInstanceRef.LoadObjectState(columnPrefix);
                            #endregion
                            _object = storageInstanceRef.MemoryInstance;
                            loadObjectLinks = true;
                        }

                        LoadedObjects[objectID as ObjectID] = _object;
                    }
                    return _object;
                }
            }
        }

        /// <summary>
        /// Get storage cell for row data 
        /// </summary>
        /// <param name="storageCellID">
        /// Defines the identitity of storage cell
        /// </param>
        /// <MetaDataID>{9ed234e0-919c-407e-98e7-7b257e4f9683}</MetaDataID>
        private void GetRowStorageCell(int storageCellID)
        {
            if (DataNode.ValueTypePath.Count > 0)
            {
                (GetDataLoader(DataNode.ParentDataNode) as DataLoader).GetRowStorageCell(storageCellID);
                LastStorageCell = (GetDataLoader(DataNode.ParentDataNode) as DataLoader).LastStorageCell;
                LastStorageCellType = (GetDataLoader(DataNode.ParentDataNode) as DataLoader).LastStorageCellType;
            }
            if (LastStorageCell == null || LastStorageCell.SerialNumber != storageCellID)
            {
                LastStorageCell = null;
                foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                {
                    if (storageCell.SerialNumber == storageCellID && storageCell is RDBMSMetaDataRepository.StorageCell)
                    {
                        LastStorageCell = storageCell as RDBMSMetaDataRepository.StorageCell;
                        LastStorageCellType = storageCell.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                        break;
                    }
                }
                if (LastStorageCell == null)
                {
                    foreach (MetaDataRepository.StorageCell storageCell in StorageCellOfObjectUnderTransaction)
                    {
                        if (storageCell.SerialNumber == storageCellID)
                        {
                            LastStorageCell = storageCell as RDBMSMetaDataRepository.StorageCell;
                            LastStorageCellType = storageCell.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds ClassMembersMappedColumnsIndices which has the indices for attributes and associationEnds data 
        /// </summary>
        /// <param name="columnsIndices">
        /// Defines a dictionary which maps Classifier Data Columns names with indices in object state vaues array
        /// </param>
        /// <param name="columnPrefix">
        /// Column prefix is nessesary in case of value type data loader or DataLoadedInParentDataSource
        /// </param>
        /// <returns>
        /// Returns the ClassMembersMappedColumnsIndices object
        /// </returns>
        /// <MetaDataID>{cd56dfc7-cf9c-42e6-9e9f-4932d7d37b05}</MetaDataID>
        private ClassMembersMappedColumnsIndices BuildClassMembersMappedColumnsIndices(System.Collections.Generic.Dictionary<string, int> columnsIndices, string columnPrefix)
        {

            ClassMembersMappedColumnsIndices metaObjectsColumnsIndices = new ClassMembersMappedColumnsIndices();
            metaObjectsColumnsIndices.AttributeIndices = new Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Dictionary<string, int>>();
            metaObjectsColumnsIndices.AssociationEndIndices = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, Dictionary<string, int>>>>();

            foreach (var dataColumn in ClassifierDataColumns)
            {
                if (dataColumn.MappedObject is RDBMSMetaDataRepository.Attribute)
                {
                    Dictionary<string, int> valueTypePaths = null;
                    if (!metaObjectsColumnsIndices.AttributeIndices.TryGetValue(dataColumn.MappedObject.Identity, out valueTypePaths))
                    {
                        valueTypePaths = new Dictionary<string, int>();
                        metaObjectsColumnsIndices.AttributeIndices[dataColumn.MappedObject.Identity] = valueTypePaths;
                    }
                    if (dataColumn.Alias != null)
                        valueTypePaths[dataColumn.CreatorIdentity] = columnsIndices[(dataColumn.Alias).ToLower()];
                    else
                        valueTypePaths[dataColumn.CreatorIdentity] = columnsIndices[(columnPrefix + dataColumn.Name).ToLower()];
                }
                if (dataColumn.MappedObject is RDBMSMetaDataRepository.AssociationEnd)
                {
                    Dictionary<MetaDataRepository.MetaObjectID, Dictionary<MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>>> valueTypePaths = null;
                    if (!metaObjectsColumnsIndices.AssociationEndIndices.TryGetValue(dataColumn.CreatorIdentity, out valueTypePaths))
                    {
                        valueTypePaths = new Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, Dictionary<string, int>>>();
                        metaObjectsColumnsIndices.AssociationEndIndices[dataColumn.CreatorIdentity] = valueTypePaths;
                    }
                    Dictionary<MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>> objectIdentityTypesColumnsIndences = null;
                    if (!valueTypePaths.TryGetValue(dataColumn.MappedObject.Identity, out objectIdentityTypesColumnsIndences))
                    {
                        objectIdentityTypesColumnsIndences = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>>();
                        valueTypePaths[dataColumn.MappedObject.Identity] = objectIdentityTypesColumnsIndences;
                    }
                    if (dataColumn.ObjectIdentityType != null)
                    {
                        Dictionary<PartTypeName, int> objectIdentityTypeColumnsIndences = null;
                        if (!objectIdentityTypesColumnsIndences.TryGetValue(dataColumn.ObjectIdentityType, out objectIdentityTypeColumnsIndences))
                        {
                            objectIdentityTypeColumnsIndences = new Dictionary<string, int>();
                            objectIdentityTypesColumnsIndences[dataColumn.ObjectIdentityType] = objectIdentityTypeColumnsIndences;
                        }

                        if (dataColumn.Alias != null)
                            objectIdentityTypeColumnsIndences[dataColumn.IdentityPart.PartTypeName] = columnsIndices[(dataColumn.Alias).ToLower()];
                        else
                            objectIdentityTypeColumnsIndences[dataColumn.IdentityPart.PartTypeName] = columnsIndices[(columnPrefix + dataColumn.Name).ToLower()];
                    }
                    else
                    {
                        Dictionary<MetaDataRepository.MetaObjectID, int> indexerValueTypePaths = null;
                        if (!metaObjectsColumnsIndices.AssociationEndIndexerColumnIndices.TryGetValue(dataColumn.CreatorIdentity, out indexerValueTypePaths))
                        {
                            indexerValueTypePaths = new Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, int>();
                            metaObjectsColumnsIndices.AssociationEndIndexerColumnIndices[dataColumn.CreatorIdentity] = indexerValueTypePaths;
                        }
                        if (dataColumn.Alias != null)
                            indexerValueTypePaths[dataColumn.MappedObject.Identity] = columnsIndices[(dataColumn.Alias).ToLower()];
                        else
                            indexerValueTypePaths[dataColumn.MappedObject.Identity] = columnsIndices[(columnPrefix + dataColumn.Name).ToLower()];
                    }
                }
            }
            return metaObjectsColumnsIndices;
        }


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>> _RelationsColumnsWithParent;

        /// <summary>
        /// This property defines the relation columns, 
        /// which used in table relation with parent data source. 
        /// </summary>
        /// <MetaDataID>{2f707d71-6301-49c8-a228-181f2e840fac}</MetaDataID>
        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>> DataSourceRelationsColumnsWithParent
        {
            get
            {
                if (_RelationsColumnsWithParent != null)
                    return _RelationsColumnsWithParent;
                _RelationsColumnsWithParent = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>();
                MetaDataRepository.MetaObject assignedMetaObject = DataNode.AssignedMetaObject;

                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    if (DataNode.RealParentDataNode != null)
                    {
                        string relationPartIdentity = DataNode.FullName;
                        _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)// (DataNode.GroupByDataNodeRoot.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity] as StorageDataLoader).ObjectIdentityTypes)
                        {
                            System.Collections.Generic.List<string> relationColumns = new List<string>();
                            if (!_RelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType))
                            {
                                System.Collections.Generic.List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();

                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                {
                                    relationColumns.Add(DataNode.RealParentDataNode.Alias + "_" + part.PartTypeName);
                                    parts.Add(new MetaDataRepository.IdentityPart(DataNode.RealParentDataNode.Alias + "_" + part.Name, part.PartTypeName, part.Type));
                                }
                                _RelationsColumnsWithParent[relationPartIdentity][new MetaDataRepository.ObjectIdentityType(parts)] = new RelationColumns( relationColumns ,"OSM_StorageIdentity");
                            }
                        }
                        return _RelationsColumnsWithParent;
                    }
                }
                if (assignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    DataNode parentRelationDataNode = null;
                    
                    if (HasRelationIdentityColumns)
                    {

                        if (DataLoaderMetadata.MemoryCell != null)
                        {
                            System.Collections.Generic.List<string> childRelationColumns = new System.Collections.Generic.List<string>();
                            ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                            string columnName = "MC_ObjectID";
                            //identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart(columnName, "MC_ObjectID", typeof(System.Guid)) });
                            childRelationColumns.Add(columnName);

                            if (!_RelationsColumnsWithParent.ContainsKey(DataNode.AssignedMetaObject.Identity.ToString()))
                                _RelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>();
                            _RelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = new RelationColumns(childRelationColumns,"OSM_StorageIdentity");
                        }
                        else
                        {

                            parentRelationDataNode = DataNode.RealParentDataNode;
                            //Parent has relation reference Identity columns or there is assocition table
                            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> relationObjectIdentiyTypesWithParent = GetRelationPartsObjectIdentityTypes(parentRelationDataNode);

                            #region Retrieve object identity columns
                            
                            foreach (string relationPartIdentity in relationObjectIdentiyTypesWithParent.Keys)
                            {
                                _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in relationObjectIdentiyTypesWithParent[relationPartIdentity])
                                {
                                    System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                                    foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                        relationColumns.Add(identityPart.Name);
                                    _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = new RelationColumns( relationColumns ,"OSM_StorageIdentity");
                                }
                            }

                            #endregion
                        }

                        #region RemovedCode
                        //Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> storageCellsLinks = GetStorageCellsLinks();
                        //foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in storageCellsLinks)
                        //{
                        //    string relationPartIdentity = null;
                        //    if (associationEnd.IsRoleA)
                        //        relationPartIdentity = storageCellsLink.Type.RoleA.Identity.ToString();
                        //    else
                        //        relationPartIdentity = storageCellsLink.Type.RoleB.Identity.ToString();
                        //    if (!_RelationsColumnsWithParent.ContainsKey(relationPartIdentity))
                        //    {
                        //        _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                        //        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                        //        {
                        //            if (!_RelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType) && (ParentDataLoader.DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].ContainsKey(objectIdentityType) || DataNode.ThrougthRelationTable))
                        //            {
                        //                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                        //                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                        //                    relationColumns.Add(identityPart.Name);
                        //                _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = relationColumns;
                        //            }
                        //        }
                        //    }
                        //}
                        //if (storageCellsLinks.Count == 0)
                        //{
                        //    string relationPartIdentity = associationEnd.Identity.ToString();
                        //    RDBMSMetaDataRepository.AssociationEnd relationPartAssociationEnd = associationEnd;
                        //    if (!_RelationsColumnsWithParent.ContainsKey(relationPartIdentity))
                        //    {
                        //        _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                        //        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                        //        {
                        //            if (!_RelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType) && (ParentDataLoader.DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].ContainsKey(objectIdentityType) || DataNode.ThrougthRelationTable))
                        //            {
                        //                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                        //                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                        //                    relationColumns.Add(identityPart.Name);
                        //                _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = relationColumns;
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        
                    }
                    else
                    {
                        //Data loader has relation reference Identity columns 
                        if (DataLoaderMetadata.MemoryCell!=null)
                        {
                            
                            System.Collections.Generic.List<string> childRelationColumns = new System.Collections.Generic.List<string>();
                            
                            RelationColumns relationColumns = new RelationColumns(childRelationColumns, "OSM_StorageIdentity");

                            ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                           
                            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                            if (associationEnd != null && HasRelationIdentityColumns)
                                childRelationColumns.Add(identityType.Parts[0].Name);
                            else
                            {
                                if (associationEnd != null)
                                {
                                    identityType = associationEnd.GetReferenceObjectIdentityType(identityType);
                                    childRelationColumns.Add(identityType.Parts[0].Name);
                                    relationColumns = new RelationColumns(childRelationColumns, associationEnd.ReferenceStorageIdentityColumnName);
                                }
                                
                            }

                            if (!_RelationsColumnsWithParent.ContainsKey(DataNode.AssignedMetaObject.Identity.ToString()))
                                _RelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>();
                            _RelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = relationColumns;
                            return _RelationsColumnsWithParent;
                        }
                        else
                        {

                            string StorageIdentityColumnName = "OSM_StorageIdentity";
                            RDBMSMetaDataRepository.Attribute attribute = null;
                            if (DataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                            {
                                parentRelationDataNode = DataNode.RealParentDataNode;
                                while (parentRelationDataNode.RealParentDataNode != null && parentRelationDataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                                    parentRelationDataNode = parentRelationDataNode.RealParentDataNode;
                                attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(parentRelationDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                            }
                            else
                                parentRelationDataNode = DataNode.RealParentDataNode;


                            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(assignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> relationObjectIdentiyTypesWithParent = (GetDataLoader(parentRelationDataNode) as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);

                            #region Retrieve object identity reference columns

                            foreach (string relationPartIdentity in relationObjectIdentiyTypesWithParent.Keys)
                            {
                                RDBMSMetaDataRepository.AssociationEnd relationPartAssociationEnd = null;
                                if (relationPartIdentity == associationEnd.Identity.ToString())
                                {
                                    relationPartAssociationEnd = associationEnd;
                                }
                                else
                                {
                                    foreach (MetaDataRepository.Association association in associationEnd.Association.Specializations)
                                    {
                                        if (associationEnd.Role == MetaDataRepository.Roles.RoleA && association.RoleA.Identity.ToString() == relationPartIdentity)
                                        {
                                            relationPartAssociationEnd = association.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                                            break;
                                        }
                                        if (associationEnd.Role == MetaDataRepository.Roles.RoleB && association.RoleB.Identity.ToString() == relationPartIdentity)
                                        {
                                            relationPartAssociationEnd = association.RoleB as RDBMSMetaDataRepository.AssociationEnd;
                                            break;
                                        }
                                    }
                                }

                                List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = null;
                                if (attribute != null)
                                {
                                    referenceObjectIdentityTypes = attribute.GetReferenceObjectIdentityTypes(relationPartAssociationEnd, relationObjectIdentiyTypesWithParent[relationPartIdentity]);
                                    StorageIdentityColumnName = attribute.GetReferenceStorageIdentityColumnName(relationPartAssociationEnd );
                                }
                                else
                                {
                                    referenceObjectIdentityTypes = relationPartAssociationEnd.GetReferenceObjectIdentityTypes(relationObjectIdentiyTypesWithParent[relationPartIdentity]);
                                    StorageIdentityColumnName = relationPartAssociationEnd.ReferenceStorageIdentityColumnName;
                                }

                                _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns >();
                                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                                {
                                    System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                                    foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                        relationColumns.Add(identityPart.Name);
                                    _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = new RelationColumns( relationColumns,StorageIdentityColumnName);
                                }
                            }


                            //if(relationObjectIdentiyTypesWithParent.ContainsKey(associationEnd.Identity.ToString()))
                            //{

                            //}

                            #region RemovedCode
                            //Collections.Generic.Set<RDBMSMetaDataRepository.StorageCellsLink> storageCellsLinks = GetStorageCellsLinks();
                            //foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in storageCellsLinks)
                            //{
                            //    string relationPartIdentity = null;
                            //    RDBMSMetaDataRepository.AssociationEnd relationPartAssociationEnd = null;
                            //    if (associationEnd.IsRoleA)
                            //    {
                            //        relationPartAssociationEnd = storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                            //        relationPartIdentity = storageCellsLink.Type.RoleA.Identity.ToString();
                            //    }
                            //    else
                            //    {
                            //        relationPartAssociationEnd = storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd;
                            //        relationPartIdentity = storageCellsLink.Type.RoleB.Identity.ToString();
                            //    }
                            //    if (!_RelationsColumnsWithParent.ContainsKey(relationPartIdentity))
                            //    {
                            //        _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                            //        List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = null;
                            //        if (attribute != null)
                            //            referenceObjectIdentityTypes = attribute.GetReferenceObjectIdentityTypes(relationPartAssociationEnd, new List<MetaDataRepository.ObjectIdentityType>(DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys));
                            //        else
                            //            referenceObjectIdentityTypes = relationPartAssociationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys));
                            //        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                            //        {
                            //            if (!_RelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType))
                            //            {
                            //                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                            //                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                            //                    relationColumns.Add(identityPart.Name);
                            //                _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = relationColumns;
                            //            }
                            //        }
                            //    }
                            //}
                            //if (storageCellsLinks.Count == 0)
                            //{
                            //    string relationPartIdentity = associationEnd.Identity.ToString();
                            //    RDBMSMetaDataRepository.AssociationEnd relationPartAssociationEnd = associationEnd;
                            //    if (!_RelationsColumnsWithParent.ContainsKey(relationPartIdentity))
                            //    {
                            //        _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                            //        List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = null;
                            //        if (attribute != null)
                            //            referenceObjectIdentityTypes = attribute.GetReferenceObjectIdentityTypes(relationPartAssociationEnd, new List<MetaDataRepository.ObjectIdentityType>(DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys));
                            //        else
                            //            referenceObjectIdentityTypes = relationPartAssociationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity].DataSourceRelationsColumnsWithSubDataNodes[DataNode.Identity][relationPartIdentity].Keys));

                            //        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                            //        {
                            //            if (!_RelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType))
                            //            {
                            //                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                            //                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                            //                    relationColumns.Add(identityPart.Name);
                            //                _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = relationColumns;
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                            #endregion
                        }
                    }

                }


                return _RelationsColumnsWithParent;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>>> _DataSourceRelationsColumnsWithSubDataNodes;
        /// <summary>
        /// This property defines the relation columns, 
        /// which used in table relation with subDataNodes data source. 
        /// </summary>
        /// <MetaDataID>{9d5065f6-e8b4-4d64-9c79-8f7f56ea4562}</MetaDataID>
        public override System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get
            {

                DataNode objectDataNode = DataNode;
                while (objectDataNode.AssignedMetaObject is MetaDataRepository.Attribute && objectDataNode.Type == DataNode.DataNodeType.Object)
                    objectDataNode = objectDataNode.RealParentDataNode;
                if (DataNode != objectDataNode)
                    return objectDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithSubDataNodes;

                if (_DataSourceRelationsColumnsWithSubDataNodes != null)
                    return _DataSourceRelationsColumnsWithSubDataNodes;
                SetRelationshipColumnsWithSubDataNodes();
                return _DataSourceRelationsColumnsWithSubDataNodes;
            }
        }

        /// <MetaDataID>{d1ece65f-b819-4536-afe5-48846dd01867}</MetaDataID>
        private void SetRelationshipColumnsWithSubDataNodes()
        {
            _DataSourceRelationsColumnsWithSubDataNodes = new Dictionary<Guid, Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>>();
            if (!DataNode.DataSource.HasInObjectContextData && DataNode.MembersFetchingObjectActivation)
                return;

            List<DataNode> relatedDataNodes = new List<DataNode>();
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    continue;
                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                {
                    relatedDataNodes.AddRange(subDataNode.GetValueTypeRelatedDataNodes());
                    continue;
                }
                relatedDataNodes.Add(subDataNode);
                if (subDataNode.Type == DataNode.DataNodeType.Group && DataNode.Type == DataNode.DataNodeType.Object)
                    relatedDataNodes.AddRange((subDataNode as GroupDataNode).GroupingDataNodes);
            }

            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in relatedDataNodes)
            {
                if (dataNode.DataSource == null||dataNode is DerivedDataNode)
                    continue;
                Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>> objectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                MetaDataRepository.MetaObject assignedMetaObject = dataNode.AssignedMetaObject;
                if (dataNode.Type == DataNode.DataNodeType.Group)
                    objectIdentityTypes[dataNode.FullName] = ObjectIdentityTypes;
                if (dataNode is AggregateExpressionDataNode)
                    objectIdentityTypes[DataNode.AssignedMetaObject.Identity.ToString()] = ObjectIdentityTypes;

                if (assignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == DataNode.DataNodeType.Object)
                    objectIdentityTypes[assignedMetaObject.Identity.ToString()] = ObjectIdentityTypes;
                string storageIdentityColumnName="OSM_StorageIdentity";
                if (assignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    if (HasRelationIdentityColumnsFor(dataNode))
                    {
                        //subDataNode DataLoader has relation reference Identity columns or there is assocition table
                        objectIdentityTypes = GetRelationPartsObjectIdentityTypes(dataNode);// dataNode.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity].ObjectIdentityTypes;
                    }
                    else
                    {
                        RDBMSMetaDataRepository.Attribute attribute = null;
                        DataNode parentRelationDataNode = null;
                        if (dataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                        {
                            parentRelationDataNode = dataNode.RealParentDataNode;
                            while (parentRelationDataNode.RealParentDataNode != null && parentRelationDataNode.RealParentDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                                parentRelationDataNode = parentRelationDataNode.RealParentDataNode;
                            attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(parentRelationDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                        }
                        else
                            parentRelationDataNode = dataNode.RealParentDataNode;

                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                        {
                            throw new System.NotSupportedException("ValueType DataNodes doesn't support DataSourceRelationsColumnsWithSubDataNodes");
                        }
                        else
                        {
                            //DataLoader has relation reference Identity columns or there is assocition table
                            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(assignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                            objectIdentityTypes = (GetDataLoader(dataNode) as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode);
                            foreach (string relationPartIdentity in new List<string>(objectIdentityTypes.Keys))
                            {
                                RDBMSMetaDataRepository.AssociationEnd relationPartAssociationEnd = null;
                                if (associationEnd.Identity.ToString() == relationPartIdentity)
                                    relationPartAssociationEnd = associationEnd;
                                else
                                {
                                    foreach (MetaDataRepository.Association specializeAssociation in associationEnd.Association.Specializations)
                                    {
                                        if (associationEnd.Role == MetaDataRepository.Roles.RoleA && specializeAssociation.RoleA.Identity.ToString() == relationPartIdentity)
                                        {
                                            relationPartAssociationEnd = specializeAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd;
                                            break;
                                        }
                                        if (associationEnd.Role == MetaDataRepository.Roles.RoleB && specializeAssociation.RoleB.Identity.ToString() == relationPartIdentity)
                                        {
                                            relationPartAssociationEnd = specializeAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd;
                                            break;
                                        }
                                    }
                                }
                                if (attribute != null)
                                {
                                    objectIdentityTypes[relationPartIdentity] = attribute.GetReferenceObjectIdentityTypes(relationPartAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd, objectIdentityTypes[relationPartIdentity]);
                                    storageIdentityColumnName = attribute.GetReferenceStorageIdentityColumnName(relationPartAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd);
                                }
                                else
                                {
                                    objectIdentityTypes[relationPartIdentity] = (relationPartAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(objectIdentityTypes[relationPartIdentity]);
                                    if (relationPartAssociationEnd.GetOtherEnd().Identity == DataNode.AssignedMetaObjectIdenty)
                                        storageIdentityColumnName = (relationPartAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).ReferenceStorageIdentityColumnName+"_SubDataNode";
                                    else
                                        storageIdentityColumnName = (relationPartAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).ReferenceStorageIdentityColumnName;

                                }
                            }
                        }
                    }
                }
                foreach (string relationPartIdentity in new List<string>(objectIdentityTypes.Keys))
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in objectIdentityTypes[relationPartIdentity])
                    {
                        MetaDataRepository.ObjectIdentityType relationObjectIdentityType = null;
                        relationObjectIdentityType = objectIdentityType;
                        if (!_DataSourceRelationsColumnsWithSubDataNodes.ContainsKey(dataNode.Identity))
                            _DataSourceRelationsColumnsWithSubDataNodes[dataNode.Identity] = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();
                        if (!_DataSourceRelationsColumnsWithSubDataNodes[dataNode.Identity].ContainsKey(relationPartIdentity))
                            _DataSourceRelationsColumnsWithSubDataNodes[dataNode.Identity][relationPartIdentity] = new Dictionary<ObjectIdentityType,RelationColumns>();
                        if (!_DataSourceRelationsColumnsWithSubDataNodes[dataNode.Identity][relationPartIdentity].ContainsKey(relationObjectIdentityType))
                        {
                            System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                            foreach (MetaDataRepository.IIdentityPart identityPart in relationObjectIdentityType.Parts)
                            {
                                if (DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                                    relationColumns.Add(DataNode.Alias + "_" + identityPart.Name);
                                else
                                    relationColumns.Add(identityPart.Name);

                            }
                          
                            _DataSourceRelationsColumnsWithSubDataNodes[dataNode.Identity][relationPartIdentity][relationObjectIdentityType] = new RelationColumns( relationColumns,storageIdentityColumnName) ;
                        }
                    }
                }
            }
        }




        /// <summary>
        /// Recursive relation is container containment relation where container and containment have the same type. 
        /// This property provide the parent reference columns
        ///  </summary>
        /// <MetaDataID>{5f2ac713-b1a8-4f51-bef4-57595f52408d}</MetaDataID>
        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveParentReferenceColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    return new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>>();
                else
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>> recursiveParentReferenceColumns = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>>();
                    foreach (string relationPartIdentity in this.RecursiveParentColumns.Keys)
                    {
                        if (associationEnd.Identity.ToString() == relationPartIdentity)
                        {
                            if (!recursiveParentReferenceColumns.ContainsKey(relationPartIdentity))
                                recursiveParentReferenceColumns[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();

                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in associationEnd.GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>(this.RecursiveParentColumns[relationPartIdentity].Keys)))
                            {
                                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                                foreach (MetaDataRepository.IdentityPart part in objectIdentityType.Parts)
                                    relationColumns.Add(part.Name);
                                recursiveParentReferenceColumns[associationEnd.Identity.ToString()][objectIdentityType] = relationColumns;
                            }
                        }
                        else
                        {

                        }

                    }

                    return recursiveParentReferenceColumns;
                }
            }
        }
        /// <summary>
        /// Recursive relation is container containment relation where container and containment have the same type.
        /// This property provide the parent identity columns.
        ///  </summary>
        /// <MetaDataID>{d86bbc2e-93c0-4110-b447-19073bcf2a11}</MetaDataID>
        public override Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>> RecursiveParentColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    return new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>>();
                else
                {
                    Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>> relationColumns = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>>();
                    MetaDataRepository.ObjectIdentityType identityType;
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    System.Collections.Generic.List<string> recursiveParentColumns = new System.Collections.Generic.List<string>();
                    MetaDataRepository.ObjectIdentityType referenceObjectIdentityType = associationEnd.GetReferenceObjectIdentityType(Classifier);
                    if (associationEnd.Association.MultiplicityType != OOAdvantech.MetaDataRepository.AssociationType.ManyToMany &&
                        referenceObjectIdentityType != null)
                    {

                        foreach (KeyValuePair<string, List<MetaDataRepository.ObjectIdentityType>> reationPartObjectIdentityTypes in (DataNode.ParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode))
                        {
                            relationColumns[reationPartObjectIdentityTypes.Key] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in reationPartObjectIdentityTypes.Value)
                            {
                                recursiveParentColumns = new System.Collections.Generic.List<string>();
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    recursiveParentColumns.Add(part.Name);
                                relationColumns[reationPartObjectIdentityTypes.Key][objectIdentityType] = recursiveParentColumns;
                            }
                        }
                        return relationColumns;
                    }
                    else if (associationEnd.Association.MultiplicityType == OOAdvantech.MetaDataRepository.AssociationType.ManyToMany)
                    {
                        foreach (KeyValuePair<string, List<MetaDataRepository.ObjectIdentityType>> reationPartObjectIdentityTypes in (DataNode.ParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationPartsObjectIdentityTypes(DataNode))
                        {
                            relationColumns[reationPartObjectIdentityTypes.Key] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in reationPartObjectIdentityTypes.Value)
                            {
                                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    recursiveParentColumns.Add(part.Name);
                                relationColumns[reationPartObjectIdentityTypes.Key][objectIdentityType] = recursiveParentColumns;
                            }
                        }
                        return relationColumns;
                    }
                    else
                    {
                        List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                        foreach (MetaDataRepository.IIdentityPart part in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityType().Parts)
                            parts.Add(new MetaDataRepository.IdentityPart(part));
                        identityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);

                        foreach (MetaDataRepository.IIdentityPart part in (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityType().Parts)
                            recursiveParentColumns.Add(part.Name);

                    }
                    relationColumns[associationEnd.Identity.ToString()] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>();
                    relationColumns[associationEnd.Identity.ToString()][identityType] = recursiveParentColumns;
                    return relationColumns;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        bool? _HasRelationIdentityColumns;
        /// <MetaDataID>{515b34a6-c017-467f-9558-7454870ebbcb}</MetaDataID>
        public override bool HasRelationIdentityColumns
        {
            get
            {
                if (DataNode.ThroughRelationTable)
                {
                    _HasRelationIdentityColumns = true;
                    return _HasRelationIdentityColumns.Value;
                }

                if (!_HasRelationIdentityColumns.HasValue)
                {
                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                        foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                        {
                            if (storageCell is OOAdvantech.MetaDataRepository.StorageCellReference)
                                continue;
                            foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in associationEnd.Association.StorageCellsLinks)
                            {
                                if (associationEnd.IsRoleA && storageCell == storageCellsLink.RoleAStorageCell && storageCellsLink.ObjectLinksTable != null)
                                {
                                    _HasRelationIdentityColumns = true;
                                    return _HasRelationIdentityColumns.Value;
                                }
                                if (!associationEnd.IsRoleA && storageCell == storageCellsLink.RoleBStorageCell && storageCellsLink.ObjectLinksTable != null)
                                {
                                    _HasRelationIdentityColumns = true;
                                    return _HasRelationIdentityColumns.Value;
                                }
                            }
                            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = associationEnd.GetReferenceColumnsFor(storageCell);
                            if (referenceColumns.Count == 0)
                            {
                                _HasRelationIdentityColumns = true;
                                return _HasRelationIdentityColumns.Value;
                            }
                        }
                        if (DataLoaderMetadata.StorageCells.Count == 0)
                        {
                            if (!associationEnd.Multiplicity.IsMany &&
                                associationEnd.Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany)
                            {
                                _HasRelationIdentityColumns = true;
                                return true;
                            }
                            else if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne &&
                                    !associationEnd.IsRoleA)
                            {
                                _HasRelationIdentityColumns = true;
                                return true;
                            }
                            else if (associationEnd.Association.LinkClass != null)
                            {
                                _HasRelationIdentityColumns = true;
                                return true;
                            }
                            else
                            {
                                _HasRelationIdentityColumns = false;
                                return false;
                            }
                        }
                    }
                    _HasRelationIdentityColumns = false;
                }
                return _HasRelationIdentityColumns.Value;
            }
        }


        /// <MetaDataID>{97da8546-a9d5-4b07-8be3-c4087dcb26a2}</MetaDataID>
        public override bool HasRelationIdentityColumnsFor(DataNode subDataNode)
        {

            if (subDataNode.ThroughRelationTable)
                return true;
            bool byDefaulthasRelationIdentityColumns = true;
            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
            {
                if (!(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                    (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany &&//)//&&
                    (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType != MetaDataRepository.AssociationType.OneToOne)
                {
                    //One to many relation
                    byDefaulthasRelationIdentityColumns = true;
                }
                else if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne &&
                    !(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().IsRoleA)
                {
                    //When relation is one to one the RoleA has the reference colums 
                    byDefaulthasRelationIdentityColumns = true;
                }
                else if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association == subDataNode.Classifier.ClassHierarchyLinkAssociation)
                {
                    byDefaulthasRelationIdentityColumns = true;
                }
                else
                    byDefaulthasRelationIdentityColumns = false;

                RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
                foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                {
                    ///TODO Πρεπει να βαλω και το vauepath γιατί σε association με structure κάνει λάθος
                    if (!(storageCell is RDBMSMetaDataRepository.StorageCellReference) &&
                        storageCell.Type.IsA(associationEnd.Specification) &&
                        ((associationEnd.GetReferenceColumnsFor(storageCell, subDataNode.ParentDataNode.ValueTypePath.ToString()).Count > 0 && byDefaulthasRelationIdentityColumns) ||
                        (associationEnd.GetReferenceColumnsFor(storageCell, subDataNode.ParentDataNode.ValueTypePath.ToString()).Count == 0 && !byDefaulthasRelationIdentityColumns)))
                    {
                        // subDataNode.ThrougthRelationTable = true;
                        return true;
                    }
                }
            }
            return byDefaulthasRelationIdentityColumns;
        }


        /// <MetaDataID>{33e1f8ee-eb1d-4aae-8150-96b3c274b948}</MetaDataID>
        override public string GetLocalDataColumnName(DataNode dataNode)
        {
            if (dataNode is AggregateExpressionDataNode)
                return dataNode.Alias;
            if (DataNode.ValueTypePath.Count > 0)
                return GetDataLoader(DataNode.ParentDataNode).GetLocalDataColumnName(dataNode);

            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
            {
                foreach (DataColumn column in ClassifierDataColumns)
                {
                    if (column.MappedObject != null && column.MappedObject.Identity == dataNode.AssignedMetaObject.Identity && column.CreatorIdentity == dataNode.ValueTypePath.ToString())
                    {
                        if (string.IsNullOrEmpty(column.Alias))
                            return column.Name;
                        else
                            return column.Alias;
                    }
                }
                foreach (DataColumn column in DerivedDataColoumns)
                {
                    if (MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(dataNode) == column.Name)
                    {
                        if (string.IsNullOrEmpty(column.Alias))
                            return column.Name;
                        else
                            return column.Alias;
                    }

                }
            }
            throw new System.Exception("Column doesn't exist for dataNode");

        }

        public override bool LocalDataColumnExistFor(DataNode dataNode)
        {
            if (dataNode is AggregateExpressionDataNode)
                return true;
            if (DataNode.ValueTypePath.Count > 0)
                return GetDataLoader(DataNode.ParentDataNode).LocalDataColumnExistFor(dataNode);

            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
            {
                foreach (DataColumn column in ClassifierDataColumns)
                {
                    if (column.MappedObject != null && column.MappedObject.Identity == dataNode.AssignedMetaObject.Identity && column.CreatorIdentity == dataNode.ValueTypePath.ToString())
                        return true;
                }
                foreach (DataColumn column in DerivedDataColoumns)
                {
                    if (MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(dataNode) == column.Name)
                        return true;
                }
            }
            return true;
        }


        /// <MetaDataID>{22e11a32-20b0-422e-92cb-25c6b9cc6c6f}</MetaDataID>
        public ObjectUnderTransactionDataManager ObjectsUnderTransactionData;

        
        internal bool OnDataLoadingPreparation;
        /// <MetaDataID>{c4067ab9-40a1-45d9-9aab-2042f9e138ab}</MetaDataID>
        protected override void OnPrepareForDataLoading()
        {
            base.OnPrepareForDataLoading();
            if (DataLoadingPrepared)
                return;
            OnDataLoadingPreparation = true;

            try
            {
                if (DataNode.ValueTypePath.Count == 0)
                {

                    ObjectsUnderTransactionData.PrepareTablesWithDataOfObjectsUnderTransaction();
                    SetRelationshipColumnsWithSubDataNodes();
                }

            }
            finally
            {
                OnDataLoadingPreparation = false;
            }
            _DataLoadingPrepared = true;
        }


        /// <MetaDataID>{bd2637de-598e-4499-845f-bcf5f0b2ac59}</MetaDataID>
        bool TablesTransferred;
        /// <MetaDataID>{f5845809-0f21-4170-9eb1-eaa76bda2c6c}</MetaDataID>
        internal void TransferTemporaryTableToDataBase()
        {

            if (!TablesTransferred)
            {

                if (ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction != null)
                    (ObjectStorage as ObjectStorage).TransferOnTemporaryTableRecords(ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction);
                foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in LocalResolvedCriterions.Keys)
                {
                    if (criterion.CriterionType == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType.CollectionContainsAnyAll)
                    {
                        if (criterion.Tag == null)
                            criterion.Tag = new System.Collections.Generic.Dictionary<string, object>();
                        object transferred = null;
                        if (!(criterion.Tag as System.Collections.Generic.Dictionary<string, object>).TryGetValue("CollectionDataTransfered", out transferred))
                        {
                            IDataTable dataTable = GetCriterionCollectionAsTable(criterion);
                            (ObjectStorage as ObjectStorage).TransferOnTemporaryTableRecords(dataTable);
                            (criterion.Tag as System.Collections.Generic.Dictionary<string, object>)["CollectionDataTransfered"] = true;
                        }
                    }
                }

                foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                {
                    if (subDataNode.ThroughRelationTable && subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {

                        /* if (ManyToManyReltionsSQLscripts[subDataNode] != null)*/

                        if (GetStorageCellsLinks(subDataNode).Count > 0)
                        {
                            MetaDataRepository.ObjectQueryLanguage.IDataTable tableWithRelationChanges = ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(subDataNode);
                            if (tableWithRelationChanges != null)
                                (ObjectStorage as ObjectStorage).TransferOnTemporaryTableRecords(tableWithRelationChanges);
                        }
                    }
                }

                TablesTransferred = true;
            }
        }


        ///<summary> 
        ///Transform criterion parameter value from collection of values to data table
        ///</summary>
        ///<param name="criterion">
        ///Defines the criterion which has the collection of values as parameter 
        ///</param>
        /// <MetaDataID>{28bb577f-3394-42c4-9e77-0505b5394a41}</MetaDataID>
        private IDataTable GetCriterionCollectionAsTable(MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            IDataTable dataTable = null;
            string parameterName = null;
            object parameterValue = null;

            if (criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
            {
                parameterName = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterName;
                parameterValue = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
            }
            else if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm)
            {
                parameterName = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterName;
                parameterValue = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ParameterComparisonTerm).ParameterValue;
            }

            MetaDataRepository.MetaObjectsStack oldMetaObjectCreator = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator;
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSPersistenceRunTime.MetaObjectsStack();
            OOAdvantech.RDBMSMetaDataRepository.Table table = new OOAdvantech.RDBMSMetaDataRepository.Table();
            table.Name = parameterName;
            dataTable = DataSource.DataObjectsInstantiator.CreateDataTable(parameterName);

            if (criterion.CollectionContainsDataNode.Type == DataNode.DataNodeType.OjectAttribute)
            {
                RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(criterion.CollectionContainsDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                int length = attribute.GetPropertyValue<int>("Persistent", "SizeOf");
                foreach (MetaDataRepository.AttributeRealization attributeRealization in attribute.AttributeRealizations)
                {
                    int attributeRealizationLength = attributeRealization.GetPropertyValue<int>("Persistent", "SizeOf");
                    if (attributeRealizationLength > length)
                        length = attributeRealizationLength;
                }
                table.AddColumn(new OOAdvantech.RDBMSMetaDataRepository.Column(attribute.Name, attribute.Type, length, true, false, 0));
                foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                    dataTable.Columns.Add(column.Name, column.Type.GetExtensionMetaObject<Type>());
                foreach (object value in criterion.ParameterValue as System.Collections.IEnumerable)
                {
                    IDataRow dataRow = dataTable.NewRow();
                    dataRow[attribute.Name] = value;
                    dataTable.Rows.Add(dataRow);
                }
            }
            else
            {
                System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                System.Collections.Generic.Dictionary<ObjectID, StorageInstanceRef> transferedObjects = new Dictionary<ObjectID, StorageInstanceRef>();
                foreach (object value in criterion.ParameterValue as System.Collections.IEnumerable)
                {
                    ObjectID objectID = null;
                    StorageInstanceRef storageInstanceRef = null;
                    try
                    {
                        storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(value) as StorageInstanceRef;
                        if (storageInstanceRef == null)
                            objectID = new ObjectID(System.Guid.NewGuid(), 0);
                        else
                            objectID = storageInstanceRef.PersistentObjectID as ObjectID;
                    }
                    catch
                    {
                        objectID = new ObjectID(System.Guid.NewGuid(), 0);
                    }
                    if (transferedObjects.ContainsKey(objectID))
                        continue;
                    else
                        transferedObjects.Add(objectID, storageInstanceRef);

                    if (!objectIdentityTypes.Contains(objectID.ObjectIdentityType))
                    {
                        objectIdentityTypes.Add(objectID.ObjectIdentityType);
                        foreach (MetaDataRepository.IIdentityPart part in objectID.ObjectIdentityType.Parts)
                            dataTable.Columns.Add(part.Name, part.Type);
                    }

                    IDataRow dataRow = dataTable.NewRow();
                    foreach (MetaDataRepository.IIdentityPart part in objectID.ObjectIdentityType.Parts)
                        dataRow[part.Name] = objectID.GetMemberValue(part.Name);
                    dataTable.Rows.Add(dataRow);
                }

            }
            return dataTable;
        }

        System.Collections.Generic.Dictionary<DataNode, bool> _GroupResultsRelatedDataNodes;
        internal protected System.Collections.Generic.Dictionary<DataNode, bool> GroupResultsRelatedDataNodes
        {
            get
            {
                if (_GroupResultsRelatedDataNodes != null)
                    return new System.Collections.Generic.Dictionary<DataNode, bool>(_GroupResultsRelatedDataNodes);


                _GroupResultsRelatedDataNodes = new Dictionary<DataNode, bool>();
                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in LocalResolvedCriterions)
                {
                    if (DataNode is GroupDataNode &&
                        ((DataNode as GroupDataNode).GroupingSourceSearchCondition == null || !(DataNode as GroupDataNode).GroupingSourceSearchCondition.Criterions.Contains(entry.Key)))
                    {
                        foreach (System.Collections.Generic.Stack<DataNode> route in entry.Value)
                        {
                            foreach (DataNode dataNode in route)
                            {
                                if (dataNode != DataNode)
                                {
                                    if (!_GroupResultsRelatedDataNodes.ContainsKey(dataNode))
                                    {
                                        if (entry.Key.IsNULL && (entry.Key.LeftTermDataNode == dataNode || entry.Key.RightTermDataNode == dataNode))
                                            _GroupResultsRelatedDataNodes.Add(dataNode, false);
                                        else
                                            _GroupResultsRelatedDataNodes.Add(dataNode, true);
                                    }
                                    else
                                    {
                                        if (entry.Key.IsNULL && (entry.Key.LeftTermDataNode == dataNode || entry.Key.RightTermDataNode == dataNode))
                                            _GroupResultsRelatedDataNodes[dataNode] = false;
                                    }
                                    if (!entry.Key.ConstrainCriterion)
                                        _GroupResultsRelatedDataNodes[dataNode] = false;
                                }
                            }
                        }
                    }
                }

                return new System.Collections.Generic.Dictionary<DataNode, bool>(_GroupResultsRelatedDataNodes);

            }
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<DataNode, bool> _RelatedDataNodes;
        /// <MetaDataID>{8b3a4a7e-938c-4552-835a-d6f925215647}</MetaDataID>
        internal protected System.Collections.Generic.Dictionary<DataNode, bool> RelatedDataNodes
        {
            get
            {
                if (_RelatedDataNodes != null)
                    return new System.Collections.Generic.Dictionary<DataNode, bool>(_RelatedDataNodes);
                _RelatedDataNodes = new System.Collections.Generic.Dictionary<DataNode, bool>();
                System.Collections.Generic.List<System.Collections.Generic.Stack<DataNode>> localResolvedCriterionsRoutes = LocalResolvedCriterionsRoutes;
                foreach (System.Collections.Generic.KeyValuePair<Criterion, System.Collections.Generic.Stack<DataNode>[]> entry in LocalResolvedCriterions)
                {
                    if (DataNode is GroupDataNode &&
                        ((DataNode as GroupDataNode).GroupingSourceSearchCondition == null || !(DataNode as GroupDataNode).GroupingSourceSearchCondition.Criterions.Contains(entry.Key)))
                        continue;
                    foreach (System.Collections.Generic.Stack<DataNode> route in entry.Value)
                    {
                        foreach (DataNode dataNode in route)
                        {
                            if (dataNode != DataNode)
                            {
                                if (!_RelatedDataNodes.ContainsKey(dataNode))
                                {
                                    if (entry.Key.IsNULL && (entry.Key.LeftTermDataNode == dataNode || entry.Key.RightTermDataNode == dataNode))
                                        _RelatedDataNodes.Add(dataNode, false);
                                    else
                                        _RelatedDataNodes.Add(dataNode, true);
                                }
                                else
                                {
                                    if (entry.Key.IsNULL && (entry.Key.LeftTermDataNode == dataNode || entry.Key.RightTermDataNode == dataNode))
                                        _RelatedDataNodes[dataNode] = false;
                                }
                                if (!entry.Key.ConstrainCriterion)
                                    _RelatedDataNodes[dataNode] = false;
                            }
                        }
                    }
                }
                foreach (System.Collections.Generic.Stack<DataNode> route in LocalResolvedGroupByRoutes)
                {
                    int i = 0;
                    foreach (DataNode dataNode in route.ToArray())
                    {
                        if (i != 0)
                            _RelatedDataNodes[dataNode] = true;
                        i++;
                    }
                    //if (_RelatedDataNodes.ContainsKey(route.ToArray()[0]))
                    //    _RelatedDataNodes.Remove(route.ToArray()[0]);
                }
                if (DataNode.Type == DataNode.DataNodeType.Group && DataNode.ParentDataNode != null)
                    _RelatedDataNodes[DataNode.RealParentDataNode] = true;

                if (DataNode.Type == DataNode.DataNodeType.Group && LocalResolvedGroupByRoutes.Count > 0 && LocalResolvedGroupByRoutes[0].ToArray()[0].Type != DataNode.DataNodeType.Group)
                {
                    foreach (System.Collections.Generic.KeyValuePair<DataNode, bool> entry in (LocalResolvedGroupByRoutes[0].ToArray()[0].DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).RelatedDataNodes)
                    {
                        if (!_RelatedDataNodes.ContainsKey(entry.Key))
                            _RelatedDataNodes[entry.Key] = entry.Value;

                    }
                }
                foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                {

                    if (!_RelatedDataNodes.ContainsKey(hostedDataDataLoader.DataNode))
                    {
                        _RelatedDataNodes.Add(hostedDataDataLoader.DataNode, false);

                        DataNode dataNode = hostedDataDataLoader.DataNode.ParentDataNode;
                        while (dataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                            dataNode.Type == DataNode.DataNodeType.Object &&
                            !_RelatedDataNodes.ContainsKey(dataNode))
                        {

                            _RelatedDataNodes.Add(dataNode, false);
                            dataNode = dataNode.ParentDataNode;
                        }
                    }

                }

                return new System.Collections.Generic.Dictionary<DataNode, bool>(_RelatedDataNodes);
            }
        }

        ///<summary>Defines the index of sql script on batch SQL command execution</summary>
        /// <MetaDataID>{4426c848-d41d-460d-8667-029df8d1a446}</MetaDataID>
        int LoadDataScriptIndex = -1;

        ///<summary>Defines the indecies of sql script on batch SQL command execution for many to many relation data</summary>
        /// <MetaDataID>{866dc0eb-07ff-4bfc-8e67-6965ffb7f149}</MetaDataID>
        System.Collections.Generic.Dictionary<DataNode, int> ManyToManyRelationSQLScriptIndicies;


        /// <summary>
        /// Build and add to sqlSripts collection the necessary SQL scripts for DataLoader data 
        /// in case where system execute batch SQL command for all data loaders
        /// </summary>
        /// <MetaDataID>{ec60cb33-d223-430a-93af-90c9b1b6a7a1}</MetaDataID>
        internal void GetSQLScripts(System.Collections.Generic.List<string> sqlScripts)
        {
            //TODO τί γινεται όταν εχει subDataNode με DataNode.ThrougthRelationTable true
            if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                return;

            if (!DataNode.BranchParticipateInSelectClause &&
                !ParticipateInOnMemoryResolvedCriterions
                && (!DataNode.BranchParticipateInAggregateFunction))
            {
                foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).TypeView.ViewColumns)
                    Data.Columns.Add(column.Name, column.Type.GetExtensionMetaObject<Type>());
                Data.Columns.Add("StorageCellID", typeof(int));
                Data.Columns.Add("OSM_StorageIdentity", typeof(int));
                return;
            }
            if (DataNode.Type == DataNode.DataNodeType.Group && (ParticipateInGlobalResolvedGroup || ParticipateInOnMemoryResolvedCriterions || ParticipateInGlobalResolvedAggregateExpression))
            {
                #region The group expression can't resolved from native data managment system and data loader build an empty table
                foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                                Data.Columns.Add(dataNode.Alias + "_" + column.Name, column.Type);
                        }
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        Data.Columns.Add(dataNode.Alias + "_" + dataNode.AssignedMetaObject.Name, (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                }
                if (DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataNode.RealParentDataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                            Data.Columns.Add(DataNode.RealParentDataNode.Alias + "_" + column.Name, column.Type);
                    }
                }
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode is AggregateExpressionDataNode)
                    {
                        if ((subDataNode as AggregateExpressionDataNode).ArithmeticExpression != null)
                            Data.Columns.Add(subDataNode.Alias, (subDataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType);
                        else
                        {
                            if (subDataNode.Type != DataNode.DataNodeType.Count)
                                Data.Columns.Add(subDataNode.Alias, ((subDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes[0].AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                            else
                                Data.Columns.Add(subDataNode.Alias, typeof(int));
                        }
                    }
                }
                #endregion

                return;
            }

            //TransferCollectionsDataToServer();
            LoadDataScriptIndex = sqlScripts.Count;
            sqlScripts.Add(SQLStatament);
            foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
            {
                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && subDataNode.ThroughRelationTable)
                {
                    if (ManyToManyRelationSQLScriptIndicies == null)
                        ManyToManyRelationSQLScriptIndicies = new Dictionary<DataNode, int>();
                    //System.Data.DataTable tableWithRelationChangesInTransaction = ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, subDataNode);
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(subDataNode.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;

                    string sqlScript = SqlScriptsBuilder.BulidRelationTableSQlScript(subDataNode);
                    if (!string.IsNullOrEmpty(sqlScript))
                    {
                        ManyToManyRelationSQLScriptIndicies[subDataNode] = sqlScripts.Count;
                        sqlScripts.Add(sqlScript);
                    }
                    else
                    {
                        ManyToManyRelationSQLScriptIndicies[subDataNode] = -1;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        Dictionary<string, string> _AliasesDictionary = new Dictionary<string, string>();
        /// <MetaDataID>{18f672ef-f94e-47b1-a315-af64cae5d394}</MetaDataID>
        /// <summary>
        /// Defines a dictionary, which maps the RDBMS friendly name with the original OLTMS (Object Lifetime Management System) name.
        /// </summary>
        internal Dictionary<string, string> AliasesDictionary
        {
            get
            {
                if (DataLoadedInParentDataSource)
                    return (GetDataLoader(DataNode.ParentDataNode) as DataLoader).AliasesDictionary;
                else
                    return _AliasesDictionary;
            }

        }
        /// <exclude>Excluded</exclude>
        Dictionary<string, string> _OrgNamesDictionary = new Dictionary<string, string>();
        /// <MetaDataID>{b90eb9f2-9d5c-4f7d-be7b-4877efa01e1b}</MetaDataID>
        internal Dictionary<string, string> OrgNamesDictionary
        {
            get
            {
                if (DataLoadedInParentDataSource)
                    return (GetDataLoader(DataNode.ParentDataNode) as DataLoader).OrgNamesDictionary;
                else
                    return _OrgNamesDictionary;
            }
        }

        ///<summary>
        ///Retrievs data loader data from data reader
        ///</summary>
        /// <MetaDataID>{5b3eb5ea-0caa-445e-ae5e-31560e66673c}</MetaDataID>
        internal void RetrieveFromDataReader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader dataReader, int scriptIndex)
        {
            if (LoadDataScriptIndex == scriptIndex)
            {
                bool ensureDistinction = !DisctionResolveOnRdbms;
                LoadDataLoaderTable( dataReader, AliasesDictionary, ensureDistinction);
            }
            if (ManyToManyRelationSQLScriptIndicies != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<DataNode, int> entry in ManyToManyRelationSQLScriptIndicies)
                {
                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(entry.Key.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                    IDataTable ManyToManyRelationData = null;
                    OOAdvantech.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> objectIdentityTypesDataSourceRelated = new OOAdvantech.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                    OOAdvantech.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> objectIdentityTypesSubNodeDataSourceRelated = new OOAdvantech.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                    AssotiationTableRelationshipData relationshipData = null;
                    if (entry.Value == -1 && !RelationshipsData.ContainsKey(entry.Key.Identity))
                    {
                        ManyToManyRelationData =MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(false);
                        foreach (string relationPartIdentity in entry.Key.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent.Keys)
                        {
                            objectIdentityTypesDataSourceRelated[relationPartIdentity] = associationEnd.GetReferenceObjectIdentityTypes(ObjectIdentityTypes);
                            objectIdentityTypesSubNodeDataSourceRelated[relationPartIdentity] = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(entry.Key.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes);
                        }
                        relationshipData = new AssotiationTableRelationshipData(objectIdentityTypesDataSourceRelated, objectIdentityTypesSubNodeDataSourceRelated, ManyToManyRelationData,associationEnd.GetOtherEnd().Role, DataNode.Identity, entry.Key.Identity);
                        RelationshipsData.Add(entry.Key.Identity, relationshipData);
                        continue;
                    }
                    if (entry.Value != scriptIndex)
                        continue;
                    ManyToManyRelationData = DataSource.DataObjectsInstantiator.CreateDataTable(false);
                    for (int i = 0; i != dataReader.FieldCount; i++)
                        ManyToManyRelationData.Columns.Add(dataReader.GetName(i), dataReader.GetFieldType(i)).ReadOnly = false;

                    ManyToManyRelationData.BeginLoadData();
                    object[] values = new object[ManyToManyRelationData.Columns.Count];
                    while (dataReader.Read())
                    {
                        dataReader.GetValues(values);
                        ManyToManyRelationData.LoadDataRow(values, LoadOption.OverwriteChanges);
                    }

                    ManyToManyRelationData.EndLoadData();
                    ManyToManyRelationData.TableName = DataNode.Alias;

                    foreach (string relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[entry.Key.Identity].Keys)
                        objectIdentityTypesDataSourceRelated[relationPartIdentity] = associationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataSourceRelationsColumnsWithSubDataNodes[entry.Key.Identity][relationPartIdentity].Keys));
                    foreach (string relationPartIdentity in entry.Key.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent.Keys)
                        objectIdentityTypesSubNodeDataSourceRelated[relationPartIdentity] = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(entry.Key.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys));

                    relationshipData = new AssotiationTableRelationshipData(objectIdentityTypesDataSourceRelated, objectIdentityTypesSubNodeDataSourceRelated, ManyToManyRelationData,associationEnd.GetOtherEnd().Role, DataNode.Identity, entry.Key.Identity);
                    if (!RelationshipsData.ContainsKey(entry.Key.Identity))
                        RelationshipsData.Add(entry.Key.Identity, relationshipData);

                }
            }
        }

        ///// <MetaDataID>{c83461af-3712-4799-82a3-4bc253b55ca6}</MetaDataID>
        ///// <summary> In case where the data node is retrieves grouping data 
        ///// if the data can be retrieved from the data base the value of property value is true else 
        ///// the property value is false.</summary>
        //public override bool GroupedDataLoaded
        //{
        //    get
        //    {
        //        if (DataNode.Type != DataNode.DataNodeType.Group)
        //            return false;
        //        else if ((ParticipateInGlobalResolvedGroup || ParticipateInOnMemoryResolvedCriterions || ParticipateInGlobalResolvedAggregateExpression))
        //            return false;
        //        else
        //            return true;
        //    }
        //}


        bool? _ParticipateInQueryResult;
        bool ParticipateInQueryResult
        {
            get
            {
                if (!_ParticipateInQueryResult.HasValue)
                {
                    if ((DataNode.ObjectQuery as DistributedObjectQuery).QueryResultType != null
                        && (DataNode.ObjectQuery as DistributedObjectQuery).QueryResultType.DataLoaderDataNodes.Contains(DataNode))
                    {
                        _ParticipateInQueryResult = true;
                    }
                    else
                        _ParticipateInQueryResult = false;
                }
                return _ParticipateInQueryResult.Value;
            }
        }

        /// <MetaDataID>{ea707339-3a6a-4b0a-b513-d15a9d8d83e4}</MetaDataID>
        public override bool RetrievesData
        {
            get
            {

                if (!DataNode.BranchParticipateInSelectClause &&
                    !ParticipateInOnMemoryResolvedCriterions &&
                    !DataNode.MembersFetchingObjectActivation &&
                    !ParticipateInGlobalResolvedCriterion &&
                    !ParticipateInGlobalResolvedGroup &&
                    !ParticipateInGlobalResolvedAggregateExpression &&
                    !ParticipateInQueryResult)
                //!DataNode.BranchParticipateInGroopByAsKey &&
                //!DataNode.BranchParticipateInGroopBy &&
                //(!DataNode.BranchParticipateInAggregateFanction || DataNode.AggregateFanctionResultsCalculatedLocally))
                {
                    return false;
                }
                if (DataLoaderMetadata.StorageCells.Count == 0 && NewObjects.Count == 0 && UpdatedObjects.Count == 0)
                {
                    return false;
                }
                return true;


            }
        }

        bool? _GroupedDataLoaded = false;
        public override bool GroupedDataLoaded
        {
            get
            {
                if (_GroupedDataLoaded.HasValue)
                    return _GroupedDataLoaded.Value;

                if (!(DataNode is GroupDataNode))
                    return false;
                if (!RetrievesData)
                    return false;
                if (DataNode.Type == DataNode.DataNodeType.Group &&
                    (ParticipateInGlobalResolvedGroup || ParticipateInOnMemoryResolvedCriterions || ParticipateInGlobalResolvedAggregateExpression))
                {
                    return false;
                }

                return true;
            }
            protected set
            {
                _GroupedDataLoaded = value;
            }

        }

        public void CreateParentRelationshipData()
        {
            ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
            AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>> parentRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>> childRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>>();
            parentRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<ObjectIdentityType>();
            childRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<ObjectIdentityType>();

            parentRelationColumns[associationEnd.Identity.ToString()].Add(associationEnd.GetReferenceObjectIdentityType(identityType));
            childRelationColumns[associationEnd.Identity.ToString()].Add(associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(identityType));
            _ParentRelationshipData = new AssotiationTableRelationshipData(parentRelationColumns, childRelationColumns, DataSource.DataObjectsInstantiator.CreateDataTable(false), associationEnd.GetOtherEnd().Role, DataNode.ParentDataNode.Identity, DataNode.Identity);


            //AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
            //System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>> parentRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>>();
            //System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>> childRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>>();
            //parentRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<ObjectIdentityType>();
            //childRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<ObjectIdentityType>();

            //if (associationEnd.IsRoleA)
            //{
            //    parentRelationColumns[associationEnd.Identity.ToString()].Add(new ObjectIdentityType(new System.Collections.Generic.List<IIdentityPart> { new MetaDataRepository.IdentityPart(DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID", "MC_ObjectID", typeof(System.Guid)) }));
            //    childRelationColumns[associationEnd.Identity.ToString()].Add(new ObjectIdentityType(new System.Collections.Generic.List<IIdentityPart> { new MetaDataRepository.IdentityPart(DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID", "MC_ObjectID", typeof(System.Guid)) }));
            //}
            //else
            //{
            //    parentRelationColumns[associationEnd.Identity.ToString()].Add(new ObjectIdentityType(new System.Collections.Generic.List<IIdentityPart> { new MetaDataRepository.IdentityPart(DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID", "MC_ObjectID", typeof(System.Guid)) }));
            //    childRelationColumns[associationEnd.Identity.ToString()].Add(new ObjectIdentityType(new System.Collections.Generic.List<IIdentityPart> { new MetaDataRepository.IdentityPart(DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID", "MC_ObjectID", typeof(System.Guid)) }));
            //}
            //_ParentRelationshipData = new AssotiationTableRelationshipData(parentRelationColumns, childRelationColumns, new DataLoader.DataTable(false), DataNode.ParentDataNode.Identity, DataNode.Identity);

        }


        /// <MetaDataID>{D8F4F951-2F96-414B-83AF-DFB22794FA8D}</MetaDataID>
        public override void RetrieveFromStorage()
        {
            if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                return;

            if (!RetrievesData)
                return;

   

            if (DataNode.Type == DataNode.DataNodeType.Group &&
                (ParticipateInGlobalResolvedGroup || ParticipateInOnMemoryResolvedCriterions || ParticipateInGlobalResolvedAggregateExpression))
            {
                _Data = null;
                return;
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable(false);
                _Data.TableName = DataNode.Alias;
                #region Grouping data can't be resolved localy. Data loader build an empty table with necessary columns
                foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                Data.Columns.Add(dataNode.Alias + "_" + part.Name, part.Type);
                        }
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            Data.Columns.Add(dataNode.ParentDataNode.ParentDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name, (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                        else
                            Data.Columns.Add(dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name, (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                    }
                }
                if (DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in DataNode.RealParentDataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            Data.Columns.Add(DataNode.RealParentDataNode.Alias + "_" + part.Name, part.Type);
                    }
                }
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode is AggregateExpressionDataNode)
                    {
                        if ((subDataNode as AggregateExpressionDataNode).ArithmeticExpression != null)
                            Data.Columns.Add(subDataNode.Alias, (subDataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType);
                        else
                        {
                            if (subDataNode.Type != DataNode.DataNodeType.Count)
                                Data.Columns.Add(subDataNode.Alias, ((subDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes[0].AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                            else
                                Data.Columns.Add(subDataNode.Alias, typeof(int));
                        }
                    }
                }
                _Data.Columns.Add("OSM_StorageIdentity", typeof(int));
                _Data.Columns.Add(GetGroupingDataColumnName(DataNode), typeof(object));
                #endregion
                return;
            }

            System.Collections.Generic.Dictionary<DataNode, string> manyToManyRelationSQLs = ManyToManyReltionsSQLscripts;
            if (DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute) && manyToManyRelationSQLs.Count == 0)
                return;
            
            ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });

            if (DataNode.ThroughRelationTable&&DataLoaderMetadata.MemoryCell!=null)
            {
                CreateParentRelationshipData();
                AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
                
                foreach (ObjectData objectData in DataLoaderMetadata.MemoryCell.Objects.Values)
                {
                    System.Guid OID = objectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                    AssotiationTableRelationshipData relationshipData = _ParentRelationshipData;
                    foreach (var parentObjectData in objectData.ParentDataNodeRelatedObjects.Values)
                    {
                        IDataRow associationRow = relationshipData.Data.NewRow();
                        associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetReferenceObjectIdentityType(identityType).Parts[0].Name] = parentObjectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                        associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(identityType).Parts[0].Name] = OID;

                        relationshipData.Data.Rows.Add(associationRow);
                    }
                }

            }
            if (DataNode.Type == DataNode.DataNodeType.Group)
                GroupedDataLoaded = true;
            var connection = (Storage as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            lock (connection)
            {

                if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    connection.Open();
                var command = connection.CreateCommand();
                if (!(DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute)))
                {
                    command.CommandText = SQLStatament;// +";";

//                    command.CommandText = @"select * from
//                                        (SELECT [ReferenceCount] AS [ReferenceCount] , [TypeID] AS [TypeID] , 10 AS [StorageCellID] , [ProductPrice_ObjectIDA] AS [ProductPrice_ObjectIDA] ,  NULL  AS [name] , [Price_Amount] AS [Price_Amount] , [IProductPrice_Name] AS [IProductPrice_Name] , [ProductPrice_ObjectIDB] AS [ProductPrice_ObjectIDB] , [Price_QuantityUnit_ObjectIDA] AS [Price_QuantityUnit_ObjectIDA] , [ObjectID] AS [ObjectID]
//                                        FROM T_ProductPrice
//                                        UNION ALL 
//                                        SELECT [ReferenceCount] AS [ReferenceCount] , [TypeID] AS [TypeID] , 15 AS [StorageCellID] , [ProductPrice_ObjectIDA] AS [ProductPrice_ObjectIDA] , [name] AS [name] , [Price_Amount] AS [Price_Amount] , [IProductPrice_Name] AS [IProductPrice_Name] , [ProductPrice_ObjectIDB] AS [ProductPrice_ObjectIDB] , [Price_QuantityUnit_ObjectIDA] AS [Price_QuantityUnit_ObjectIDA] , [ObjectID] AS [ObjectID]
//                                        FROM T_SubProductPrice) AS [Abstract_IProductPrice_1]
//                                        LEFT OUTER JOIN (SELECT [Name] AS [Name] , [ObjectID] AS [ObjectID] , 9 AS [StorageCellID]
//                                        FROM T_Product
//                                        UNION ALL 
//                                        SELECT [Name] AS [Name] , [ObjectID] AS [ObjectID] , 14 AS [StorageCellID]
//                                        FROM T_MaterialProduct
//                                        UNION ALL 
//                                        SELECT [Name] AS [Name] , [ObjectID] AS [ObjectID] , 4 AS [StorageCellID]
//                                        FROM T_LiquidProduct) AS [Abstract_IProduct_1] ON ([Abstract_IProductPrice_1].[ProductPrice_ObjectIDA] = [Abstract_IProduct_1].[ObjectID])";

                    if (RDBMSSQLScriptGenarator.SupportBatchSQL)
                        command.CommandText += "; ";
                }
                if (RDBMSSQLScriptGenarator.SupportBatchSQL)
                {
                    foreach (System.Collections.Generic.KeyValuePair<DataNode, string> entry in manyToManyRelationSQLs)
                    {
                        if (!string.IsNullOrEmpty(entry.Value))
                            command.CommandText = command.CommandText + "\r\n" + entry.Value + ";";
                    }
                }//Quantity35173667
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader dataReader = null;
                if (!string.IsNullOrEmpty(command.CommandText))
                    dataReader = command.ExecuteReader();
                try
                {
                    if (!(DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute)))
                    {
                        bool ensureDistinction = !DisctionResolveOnRdbms; ;
                        #region RowRemove code
                        //if (RowRemoveColumns.Count > 0)
                        //    ensureDistinction = true;
                        #endregion
                        try
                        {
                            LoadDataLoaderTable( dataReader, AliasesDictionary, ensureDistinction);
                        }
                        catch (Exception error)
                        {
                            if (dataReader != null)
                                dataReader.Close();
                            throw;
                        }
                        if (dataReader != null)
                        {
                            if (RDBMSSQLScriptGenarator.SupportBatchSQL)
                                dataReader.NextResult();
                            else
                                dataReader.Close();
                        }
                    }

                    LoadManyToManyRelationData(manyToManyRelationSQLs, connection, dataReader);
                    manyToManyRelationSQLs.Clear();
                    if (RDBMSSQLScriptGenarator.SupportBatchSQL && dataReader != null)
                        dataReader.Close();

                    ActivateObjects();



                    _ResolvedAggregateExpressions = (from aggregationDataNode in DataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                                     where CheckAggregateFunctionForLocalResolve(aggregationDataNode)
                                                     select aggregationDataNode).ToList();

                }
                finally
                {

                }
            }

            int count = DataSourceRelationsColumnsWithSubDataNodes.Count;
            count = DataSourceRelationsColumnsWithParent.Count;
        }

        /// <summary>
        /// Builds SQL scripts for relation table for relations with sub DataNodes data
        /// </summary>
        /// <MetaDataID>{4354341d-2152-42ca-9977-aba785b3a4e6}</MetaDataID>
        private Dictionary<DataNode, string> ManyToManyReltionsSQLscripts
        {
            get
            {
                System.Collections.Generic.Dictionary<DataNode, string> manyToManyRelationSQLs = new Dictionary<DataNode, string>();
                if (DataNode.Type == DataNode.DataNodeType.Group)
                    return manyToManyRelationSQLs;
                foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                {

                    DataLoader subDatanodeLoader = GetDataLoader(subDataNode) as DataLoader;
                    // In case where subDataNode is objects data node and not valuetype and subDataNode dataLoader data loaded In ParentDataSource 
                    // system doesn't produce many relation script
                    if (subDataNode.Type == DataNode.DataNodeType.Object &&
                        (subDataNode.DataSource != null && (subDataNode.DataSource as StorageDataSource).HasInObjectContextData && subDatanodeLoader.DataLoadedInParentDataSource) &&
                        !(subDataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                        continue;

                    // Builds many to many sql scripts for subDataNodes which has relation with parent through relation table
                    if (subDataNode.ThroughRelationTable &&
                        subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd
                        && subDatanodeLoader != null && !subDatanodeLoader.IsEmpty)
                        manyToManyRelationSQLs[subDataNode] = SqlScriptsBuilder.BulidRelationTableSQlScript(subDataNode);

                    // Because value type subDataNode loaded in firstParent non value type data node 
                    // the relation tables with non value type subDatnodes of valueType data node Loaded in parent
                    if (subDataNode.ValueTypePath.Count > 0)
                    {
                        foreach (DataNode valueTypeSubDataNode in subDataNode.GetValueTypeRelatedDataNodes())
                        {
                            DataLoader valueTypeSubDataNodeLoader = GetDataLoader(valueTypeSubDataNode) as DataLoader;
                            // Builds many to many sql scripts for subDataNodes which has relation with parent through relation table
                            if (valueTypeSubDataNode.ThroughRelationTable && valueTypeSubDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && valueTypeSubDataNodeLoader != null && !valueTypeSubDataNodeLoader.IsEmpty)
                                manyToManyRelationSQLs[valueTypeSubDataNode] = SqlScriptsBuilder.BulidRelationTableSQlScript(valueTypeSubDataNode);
                        }
                    }
                }
                return manyToManyRelationSQLs;
            }
        }

        /// <summary>
        /// Retrieves relation data from datadabase and load them in RelationshipsData dictionary  
        /// </summary>
        /// <param name="manyToManyRelationSQLs"> Defines sql scripts for each related dataNode</param>
        /// <param name="connection">RDBMS ADO.NET Connection to retrieve relationship data </param>
        /// <param name="dataReader">Defines an ADO.NET DataReader. It is useful in case where RDBMS load batch all data at once</param>
        /// <MetaDataID>{6dc1fbd0-abd9-48ef-b53e-1ba0e85dc1f0}</MetaDataID>
        private void LoadManyToManyRelationData(System.Collections.Generic.Dictionary<DataNode, string> manyToManyRelationSQLs, OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader dataReader)
        {


            foreach (System.Collections.Generic.KeyValuePair<DataNode, string> entry in manyToManyRelationSQLs)
            {
                RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
                IDataTable ManyToManyRelationData =DataSource.DataObjectsInstantiator.CreateDataTable(false);
                Dictionary<string, System.Type> associationTableColumnsTypes = new Dictionary<string, Type>();
                Dictionary<string, string> associationTableColumns = new Dictionary<string, string>();
                Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> parentRelationObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> childRelationObjectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                foreach (string relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[entry.Key.Identity].Keys)
                {
                    associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relationPartIdentity, typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    parentRelationObjectIdentityTypes[relationPartIdentity] = associationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataSourceRelationsColumnsWithSubDataNodes[entry.Key.Identity][relationPartIdentity].Keys));
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in parentRelationObjectIdentityTypes[relationPartIdentity])
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            associationTableColumnsTypes[part.Name.ToLower()] = part.Type;
                            associationTableColumns[part.Name.ToLower()] = part.Name;
                        }
                    }
                }


                foreach (string relationPartIdentity in entry.Key.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent.Keys)
                {
                    associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relationPartIdentity, typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                    childRelationObjectIdentityTypes[relationPartIdentity] = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(entry.Key.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].DataSourceRelationsColumnsWithParent[relationPartIdentity].Keys));
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in childRelationObjectIdentityTypes[relationPartIdentity])
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            associationTableColumnsTypes[part.Name.ToLower()] = part.Type;
                            associationTableColumns[part.Name.ToLower()] = part.Name;
                        }
                    }
                }
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(entry.Key.AssignedMetaObject.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                if (associationEnd.Indexer)
                {
                    associationTableColumnsTypes[associationEnd.IndexerColumn.Name.ToLower()] = ModulePublisher.ClassRepository.GetType(associationEnd.IndexerColumn.Type.FullName, "");
                    associationTableColumns[associationEnd.IndexerColumn.Name.ToLower()] = associationEnd.IndexerColumn.Name;
                }

                if (!string.IsNullOrEmpty(entry.Value))
                {

                    if (!RDBMSSQLScriptGenarator.SupportBatchSQL)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = entry.Value;
                        dataReader = command.ExecuteReader();
                    }
                    bool typeConvertion = false;
                    Type[] manyToManyRelationDataTypes = new Type[dataReader.FieldCount];
                    for (int i = 0; i != dataReader.FieldCount; i++)
                    {
                        string columnName = dataReader.GetName(i);
                        if (columnName.ToLower() == "roleastorageidentity")
                        {
                            manyToManyRelationDataTypes[i] = typeof(int);
                            ManyToManyRelationData.Columns.Add("RoleAStorageIdentity", typeof(int)).ReadOnly = false;
                        }
                        else if (columnName.ToLower() == "rolebstorageidentity")
                        {
                            manyToManyRelationDataTypes[i] = typeof(int);
                            ManyToManyRelationData.Columns.Add("RoleBStorageIdentity", typeof(int)).ReadOnly = false;
                        }
                        else
                        {
                            manyToManyRelationDataTypes[i] = associationTableColumnsTypes[columnName.ToLower()];
                            if (manyToManyRelationDataTypes[i] == dataReader.GetFieldType(i))
                            {
                                manyToManyRelationDataTypes[i] = null;
                                ManyToManyRelationData.Columns.Add(associationTableColumns[dataReader.GetName(i).ToLower()], dataReader.GetFieldType(i)).ReadOnly = false;

                            }
                            else
                            {
                                typeConvertion = true;
                                ManyToManyRelationData.Columns.Add(associationTableColumns[dataReader.GetName(i).ToLower()], manyToManyRelationDataTypes[i]).ReadOnly = false;
                            }
                        }
                    }
                    ManyToManyRelationData.BeginLoadData();
                    object[] values = new object[ManyToManyRelationData.Columns.Count];
                    while (dataReader.Read())
                    {
                        dataReader.GetValues(values);
                        if (typeConvertion)
                        {
                            for (int i = 0; i != dataReader.FieldCount; i++)
                                if (manyToManyRelationDataTypes[i] != null)
                                    values[i] = TypeDictionary.Convert(values[i], manyToManyRelationDataTypes[i]);
                        }
                        ManyToManyRelationData.LoadDataRow(values, LoadOption.OverwriteChanges);
                    }
                    ManyToManyRelationData.EndLoadData();
                }
                else
                {

                    IDataTable associtionTableWithChanges = ObjectsUnderTransactionData.GetTableWithRelationChangesInTransaction(entry.Key);
                    if (associtionTableWithChanges != null)
                    {
                        int changeTypeIndex = -1;

                        bool typeConvertion = false;
                        Type[] manyToManyRelationDataTypes = new Type[associtionTableWithChanges.Columns.Count];
                        int i = 0;
                        foreach (IDataColumn column in associtionTableWithChanges.Columns)
                        {
                            string columnName = column.ColumnName;
                            if (columnName.ToLower() == "roleastorageidentity")
                            {
                                manyToManyRelationDataTypes[i] = typeof(int);
                                ManyToManyRelationData.Columns.Add("RoleAStorageIdentity", typeof(int)).ReadOnly = false;
                            }
                            else if (columnName.ToLower() == "rolebstorageidentity")
                            {
                                manyToManyRelationDataTypes[i] = typeof(int);
                                ManyToManyRelationData.Columns.Add("RoleBStorageIdentity", typeof(int)).ReadOnly = false;
                            }
                            else if (columnName == "ChangeType")
                            {
                                changeTypeIndex = i;
                            }
                            else
                            {
                                manyToManyRelationDataTypes[i] = associationTableColumnsTypes[columnName.ToLower()];
                                if (manyToManyRelationDataTypes[i] == column.DataType)
                                {
                                    manyToManyRelationDataTypes[i] = null;
                                    ManyToManyRelationData.Columns.Add(associationTableColumns[associtionTableWithChanges.Columns[i].ColumnName.ToLower()], associtionTableWithChanges.Columns[i].DataType).ReadOnly = false;

                                }
                                else
                                {
                                    typeConvertion = true;
                                    ManyToManyRelationData.Columns.Add(associationTableColumns[associtionTableWithChanges.Columns[i].ColumnName.ToLower()], manyToManyRelationDataTypes[i]).ReadOnly = false;
                                }
                            }
                            i++;
                        }
                        ManyToManyRelationData.BeginLoadData();
                        //object[] values = new object[ManyToManyRelationData.Columns.Count];
                        foreach (MetaDataRepository.ObjectQueryLanguage.IDataRow row in associtionTableWithChanges.Rows)
                        {
                            object[] values = row.ItemArray;

                            if ((int)values[changeTypeIndex] == 1)
                                continue;

                            object[] newRowValues = new object[ManyToManyRelationData.Columns.Count];
                            //dataReader.GetValues(values);
                            i = 0;
                            int k = 0;
                            for (i = 0; i != associtionTableWithChanges.Columns.Count; i++)
                            {
                                if (i != changeTypeIndex)
                                {
                                    if (manyToManyRelationDataTypes[i] != null)
                                        newRowValues[k++] = TypeDictionary.Convert(values[i], manyToManyRelationDataTypes[i]);
                                    else
                                        newRowValues[k++] = values[i];
                                }
                            }
                            ManyToManyRelationData.LoadDataRow(newRowValues, LoadOption.OverwriteChanges);
                        }
                        ManyToManyRelationData.EndLoadData();

                    }
                }
                ManyToManyRelationData.TableName = DataNode.Alias; ;
                AssotiationTableRelationshipData relationshipData = new AssotiationTableRelationshipData(parentRelationObjectIdentityTypes, childRelationObjectIdentityTypes, ManyToManyRelationData, associationEnd.GetOtherEnd().Role, DataNode.Identity, entry.Key.Identity);
                if (!RelationshipsData.ContainsKey(entry.Key.Identity))
                    RelationshipsData.Add(entry.Key.Identity, relationshipData);
                if (RDBMSSQLScriptGenarator.SupportBatchSQL)
                    dataReader.NextResult();
                else
                    dataReader.Close();

            }
        }

        /// <MetaDataID>{8361882d-f9fe-4b1c-a487-87eb70a4c362}</MetaDataID>
        internal bool DisctionResolveOnRdbms
        {
            get
            {
                bool disctionResolved = true;
#if !DeviceDotNet
                foreach (DataColumn column in ClassifierDataColumns)
                {
                    if (column.Type == typeof(System.Drawing.Image))
                    {
                        disctionResolved = false;
                        break;
                    }
                }
                if (disctionResolved)
                {
                    foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                    {
                        foreach (DataColumn column in hostedDataDataLoader.ClassifierDataColumns)
                        {
                            if (column.Type == typeof(System.Drawing.Image))
                            {
                                disctionResolved = false;
                                break;
                            }
                        }
                        if (!disctionResolved)
                            break;
                    }
                }
#endif
                if (DataNode.ObjectQuery.SelectListItems.Count == 1 &&
                    DataNode.ObjectQuery.SelectListItems[0].Type == DataNode.DataNodeType.Count &&
                    (DataNode.ObjectQuery.SelectListItems[0].ParentDataNode == DataNode &&
                    (DataNode.ObjectQuery.SelectListItems[0] as AggregateExpressionDataNode).AggregateExpressionDataNodes.Count == 0))
                {
                    return true;
                }

                return disctionResolved;
            }
        }

        public override SearchCondition SearchCondition
        {
            get
            {
                return DataNode.RemoveGroupResultCriterions(base.SearchCondition);
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4B49971A-4B59-45A2-BD88-507969AE4E8F}</MetaDataID>
        private string _SQLStatament;
        /// <MetaDataID>{BF3A0DFA-447E-4160-80C8-1D344DCD4BD5}</MetaDataID>
        internal protected virtual string SQLStatament
        {
            get
            {
                lock (ObjectStorage)
                {

                    if (_SQLStatament != null)
                        return _SQLStatament;
                    _SQLStatament = null;
                    string fromClause = SqlScriptsBuilder.FromClause;
                    if (DataNode.Recursive)
                        _SQLStatament = SqlScriptsBuilder.BuildRecursiveLoad();
                    else if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        _SQLStatament = SqlScriptsBuilder.SelectClause + " " + fromClause + " " + SqlScriptsBuilder.GroupByClause;
                        if (!string.IsNullOrEmpty(SqlScriptsBuilder.OrderByClause) || !string.IsNullOrEmpty(SqlScriptsBuilder.WhereClause))
                        {
                            _SQLStatament = "SELECT " + SqlScriptsBuilder.DataLoaderColumnsList + " FROM (" + _SQLStatament + ") " + RDBMSSQLScriptGenarator.AliasDefSqlScript + SqlScriptsBuilder.GetSQLScriptForName(DataNode.Alias);
                            string groupResultFilterConditionDataSqlScript = SqlScriptsBuilder.GroupResultFilterConditionDataSqlScript;
                            if (!string.IsNullOrEmpty(groupResultFilterConditionDataSqlScript))
                            {
                                if (DisctionResolveOnRdbms)
                                    _SQLStatament = "SELECT DISTINCT ";
                                else
                                    _SQLStatament = "SELECT ";
                                _SQLStatament += SqlScriptsBuilder.DataLoaderColumnsList + " FROM (" + _SQLStatament + ") " + RDBMSSQLScriptGenarator.AliasDefSqlScript + DataNode.Alias + groupResultFilterConditionDataSqlScript;

                            }

                            _SQLStatament += " " + SqlScriptsBuilder.WhereClause + " " + SqlScriptsBuilder.OrderByClause;

                        }
                    }
                    else
                    {
                        if (SqlScriptsBuilder.HasCriterionOnAggregationFunc)
                        {
                            _SQLStatament = "SELECT " + SqlScriptsBuilder.DataLoaderColumnsList + " FROM ( " + SqlScriptsBuilder.SelectClause + " " + fromClause +" ) "+ RDBMSSQLScriptGenarator.AliasDefSqlScript + SqlScriptsBuilder.GetSQLScriptForName(DataNode.Alias)+"\n";
                            _SQLStatament += SqlScriptsBuilder.GroupByClause + " " + SqlScriptsBuilder.WhereClause + " " + SqlScriptsBuilder.OrderByClause;
                        }
                        else
                        {
                            _SQLStatament = SqlScriptsBuilder.SelectClause + " " + fromClause + " " + SqlScriptsBuilder.GroupByClause + " " + SqlScriptsBuilder.WhereClause + " " + SqlScriptsBuilder.OrderByClause;
                        }
                    }

                    string dropTemTables = null;
                    _SQLStatament += dropTemTables;
                    return _SQLStatament;

                }
            }
        }




        /// <MetaDataID>{a141f0a2-ba2f-4320-a15c-2e0c99aba711}</MetaDataID>
        new internal System.Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataLoader> HostedDataDataLoaders
        {
            get
            {
                return base.HostedDataDataLoaders;
            }
        }



        /// <MetaDataID>{19a0396f-e89e-4ee7-a2a9-2a189a427460}</MetaDataID>
        private bool IsEmpty
        {
            get
            {
                if (DataLoaderMetadata.StorageCells.Count == 0 && NewObjects.Count == 0 && UpdatedObjects.Count == 0)
                    return true;
                else
                    return false;
            }

        }


        /// <MetaDataID>{5b5f924a-9add-4f1f-9a5f-51a955d12346}</MetaDataID>
        /// <summary>Retrieves the storageCellLinks with the related dataNode  </summary>
        public override Collections.Generic.Set<MetaDataRepository.StorageCellsLink> GetStorageCellsLinks(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode)
        {
            Collections.Generic.Set<MetaDataRepository.StorageCellsLink> ObjectsLinks = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink>();
            MetaDataRepository.Roles relatedDataNodeRole;
            RDBMSMetaDataRepository.AssociationEnd associationEnd = null;
            MetaDataRepository.ValueTypePath valueTypePath;
            bool relationObject = false;
            if (DataNode.IsParentDataNode(relatedDataNode))
            {
                if (!(DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
                    throw new System.Exception(string.Format("Invalid DataNode, The relation between the ‘{0}’ and ‘{1}’ doesn’t based on association", DataNode.RealParentDataNode.Name, DataNode.Name));
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
                valueTypePath = DataNode.RealParentDataNode.ValueTypePath;
                relatedDataNodeRole = associationEnd.Role;
                if ((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass == relatedDataNode.Classifier)
                    relationObject = true;
            }
            else
            {
                if (!(relatedDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
                    throw new System.Exception(string.Format("Invalid DataNode, The relation between the ‘{0}’ and ‘{1}’ doesn’t based on association", relatedDataNode.RealParentDataNode.Name, relatedDataNode.Name));
                associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                valueTypePath = relatedDataNode.RealParentDataNode.ValueTypePath;
                relatedDataNodeRole = associationEnd.Role;
                if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass == relatedDataNode.Classifier)
                    relationObject = true;

            }
            RDBMSMetaDataRepository.Association association = associationEnd.Association as RDBMSMetaDataRepository.Association;

#region Construct ObjectLinksDataSource object
            foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
            {

                foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in association.GetStorageCellsLinks(storageCell))
                {
                    if (valueTypePath.ToString() == storageCellsLink.ValueTypePath)
                    {
                        foreach (KeyValuePair<string, DataLoaderMetadata> entry in (relatedDataNode.DataSource as StorageDataSource).DataLoadersMetadata)
                        {

                            if (!relationObject && relatedDataNodeRole == OOAdvantech.MetaDataRepository.Roles.RoleA &&
                                entry.Value.StorageCells.Contains(storageCellsLink.RoleAStorageCell))
                            {
                                ObjectsLinks.Add(storageCellsLink);
                                break;
                            }
                            else if (!relationObject && relatedDataNodeRole == OOAdvantech.MetaDataRepository.Roles.RoleB &&
                                    (entry.Value.StorageCells.Contains(storageCellsLink.RoleBStorageCell)))
                            {
                                ObjectsLinks.Add(storageCellsLink);
                                break;
                            }
                            else if (relationObject)
                            {
                                bool breakExternaLoop = false;
                                foreach (OOAdvantech.RDBMSMetaDataRepository.StorageCell relationObjectsStorageCell in storageCellsLink.AssotiationClassStorageCells)
                                {
                                    if ((relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).DataLoaderMetadata.StorageCells.Contains(relationObjectsStorageCell))
                                    {
                                        ObjectsLinks.Add(storageCellsLink);
                                        breakExternaLoop = true;
                                        break;
                                    }


                                }
                                if (breakExternaLoop)
                                    break;
                            }

                        }

                    }
                }


            }
#endregion
            return ObjectsLinks;

        }
        ///// <MetaDataID>{75604d4c-2a02-476f-b182-b4ac598cb32f}</MetaDataID>
        ///// <summary>
        ///// Retrieves the dataloader object identity types which used for the relation of dataloader with related data node data loader
        ///// </summary>
        //internal Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> GetRelationPartsObjectIdentityTypes(DataNode relatedDataNode)
        //{

        //    Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> objectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
        //    MetaDataRepository.Roles dataNodeDataLoaderRole;
        //    MetaDataRepository.Roles relationPartIdentityAsociatioEndRole;
        //    string relationPartIdentity = null;
        //    if (DataNode.IsParentDataNode(relatedDataNode))
        //    {
        //        dataNodeDataLoaderRole = (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Role;
        //        relationPartIdentityAsociatioEndRole = (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Role;
        //    }
        //    else
        //    {
        //        dataNodeDataLoaderRole = (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Role;
        //        relationPartIdentityAsociatioEndRole = (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Role;
        //    }
        //    foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in GetStorageCellsLinks(relatedDataNode))
        //    {
        //        if (relationPartIdentityAsociatioEndRole == OOAdvantech.MetaDataRepository.Roles.RoleA)
        //            relationPartIdentity = storageCellsLink.Type.RoleA.Identity.ToString();
        //        else
        //            relationPartIdentity = storageCellsLink.Type.RoleB.Identity.ToString();
        //        if (!objectIdentityTypes.ContainsKey(relationPartIdentity))
        //            objectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>();
        //        if (DataNode.Classifier.ClassHierarchyLinkAssociation != null &&
        //            relatedDataNode.IsParentDataNode(DataNode) &&
        //            DataNode.Classifier.ClassHierarchyLinkAssociation == (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
        //        {
        //            foreach (RDBMSMetaDataRepository.StorageCell storageCell in storageCellsLink.AssotiationClassStorageCells)
        //            {
        //                if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]))
        //                    objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]);
        //            }

        //        }
        //        else if (DataNode.Classifier.ClassHierarchyLinkAssociation != null &&
        //            DataNode.IsParentDataNode(relatedDataNode) &&
        //            DataNode.Classifier.ClassHierarchyLinkAssociation == (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
        //        {
        //            foreach (RDBMSMetaDataRepository.StorageCell storageCell in storageCellsLink.AssotiationClassStorageCells)
        //            {
        //                if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]))
        //                    objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]);
        //            }

        //        }
        //        else
        //        {
        //            if (dataNodeDataLoaderRole == OOAdvantech.MetaDataRepository.Roles.RoleA)
        //            {
        //                if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleAStorageCell.ObjectIdentityType)]))
        //                    objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleAStorageCell.ObjectIdentityType)]);
        //            }
        //            else
        //            {
        //                if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleBStorageCell.ObjectIdentityType)]))
        //                    objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleBStorageCell.ObjectIdentityType)]);
        //            }
        //        }

        //    }

        //    #region relation object identity types for relations under transactions
        //    if (DataNode.IsParentDataNode(relatedDataNode))
        //        relationPartIdentity = (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString();
        //    else
        //        relationPartIdentity = (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString();

        //    if (DataNode.IsParentDataNode(relatedDataNode) && ThereIsRelationWithTransactionNewObject)
        //    {
        //        if (!objectIdentityTypes.ContainsKey(relationPartIdentity))
        //            objectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>();

        //        if (NewObjects.Count > 0 && !objectIdentityTypes[relationPartIdentity].Contains(NewObjectIdentityType))
        //            objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(NewObjectIdentityType)]);

        //        foreach (MetaDataRepository.ObjectIdentityType outStorageTransientObjectIdentityType in OutStorageTransientObjectIdentityTypes)
        //        {
        //            if (!objectIdentityTypes[relationPartIdentity].Contains(outStorageTransientObjectIdentityType))
        //                objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(outStorageTransientObjectIdentityType)]);

        //        }
        //    }
        //    else
        //    {
        //        if (ThereIsSubDataNodeRelationWithTransactionNewObject.ContainsKey(relatedDataNode) &&
        //            ThereIsSubDataNodeRelationWithTransactionNewObject[relatedDataNode])
        //        {
        //            if (!objectIdentityTypes.ContainsKey(relationPartIdentity))
        //                objectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>();

        //            if (!objectIdentityTypes[relationPartIdentity].Contains(NewObjectIdentityType))
        //                objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(NewObjectIdentityType)]);
        //        }
        //    }
        //    #endregion

        //    //Empty data source
        //    if (objectIdentityTypes.Count == 0)
        //    {
        //        objectIdentityTypes[relationPartIdentity] = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
        //        objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(NewObjectIdentityType)]);
        //    }
        //    return objectIdentityTypes;
        //}

        public override ObjectIdentityType ObjectIdentityTypeForNewObject
        {
            get { return NewObjectIdentityType; }
        }
        /// <MetaDataID>{3d7f257d-36ee-45f3-b39c-e74a2160844d}</MetaDataID>
        internal SQLScriptsBuilder SqlScriptsBuilder;
        /// <MetaDataID>{d3c8c38b-6b07-410e-a088-ccda7efadfcd}</MetaDataID>
        static internal MetaDataRepository.ObjectIdentityType NewObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(Guid)) });

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> _ObjectIdentityTypes;
        /// <MetaDataID>{24159e5f-c323-424f-9344-9aa688b3388f}</MetaDataID>
        public override List<OOAdvantech.MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {
            get
            {
                if (_ObjectIdentityTypes == null)
                {
                    if (DataNode.Type == DataNode.DataNodeType.Group && DataNode.RealParentDataNode != null)
                    {
                        _ObjectIdentityTypes = GetDataLoader(DataNode.RealParentDataNode).ObjectIdentityTypes;
                    }
                    else
                    {
                        _ObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                        foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                        {
                            if (!_ObjectIdentityTypes.Contains(storageCell.ObjectIdentityType))
                            {
                                List<OOAdvantech.MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                foreach (MetaDataRepository.IIdentityPart part in storageCell.ObjectIdentityType.Parts)
                                    parts.Add(new MetaDataRepository.IdentityPart(part.PartTypeName, part.PartTypeName, part.Type));
                                OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                                _ObjectIdentityTypes.Add(objectIdentityType);
                            }
                        }
                        foreach (MetaDataRepository.StorageCell storageCell in StorageCellOfObjectUnderTransaction)
                        {
                            if (!DataLoaderMetadata.StorageCells.Contains(storageCell) && !_ObjectIdentityTypes.Contains((storageCell).ObjectIdentityType))
                            {
                                List<OOAdvantech.MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                foreach (MetaDataRepository.IIdentityPart part in storageCell.ObjectIdentityType.Parts)
                                    parts.Add(new MetaDataRepository.IdentityPart(part.PartTypeName, part.PartTypeName, part.Type));
                                OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                                _ObjectIdentityTypes.Add(objectIdentityType);
                            }
                        }
                        if (NewObjects.Count > 0 || _ObjectIdentityTypes.Count == 0)
                        {
                            if (!_ObjectIdentityTypes.Contains(NewObjectIdentityType))
                                _ObjectIdentityTypes.Add(NewObjectIdentityType);
                        }
                        foreach (DataNode subDataNode in DataNode.SubDataNodes)
                        {
                            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                            {
                                if (GetDataLoader(subDataNode)!=null&& (GetDataLoader(subDataNode) as DataLoader).DataLoaderMetadata.StorageCells.Count == 0)
                                {
                                    if (!_ObjectIdentityTypes.Contains(NewObjectIdentityType))
                                        _ObjectIdentityTypes.Add(NewObjectIdentityType);
                                    break;
                                }
                            }
                        }

                    }
                }
                return _ObjectIdentityTypes;
            }
        }

        /// <MetaDataID>{6dd8bc9e-ab50-46c8-9fb2-188eba7698c7}</MetaDataID>
        public override void UpdateObjectIdentityTypes(System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> dataLoaderObjectIdentityTypes)
        {
            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataLoaderObjectIdentityTypes)
            {
                if (_ObjectIdentityTypes.Contains(objectIdentityType))
                {
                    _ObjectIdentityTypes.RemoveAt(_ObjectIdentityTypes.IndexOf(objectIdentityType));
                    _ObjectIdentityTypes.Add(objectIdentityType);
                }
                else
                {
                    _ObjectIdentityTypes.Add(objectIdentityType);
                }
            }
        }


        /// <summary>
        /// Convert value to new type.
        /// </summary>
        /// <MetaDataID>{b26515c1-3531-46e5-8b6f-2b54610b1d43}</MetaDataID>
        protected override object Convert(object value, System.Type type)
        {
            if (value == null || value is System.DBNull)
                return value;
            else
                return TypeDictionary.Convert(value, type);

        }

        /// <MetaDataID>{392442d9-7657-46a9-903e-b11801aeb959}</MetaDataID>
        protected override Type GetColumnType(string columnName)
        {
            if (columnName.ToLower() == "OSM_StorageIdentity".ToLower())
                return typeof(int);
            string rowRemoveName = (DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove").ToLower();
            if (columnName.ToLower().IndexOf(rowRemoveName) == 0)
                return typeof(bool);

            foreach (DataColumn column in ClassifierDataColumns)
            {
                if (column.Alias != null && column.Alias.ToLower() == columnName.ToLower())
                    return column.Type;

                if (column.Name.ToLower() == columnName.ToLower())
                    return column.Type;

            }
            foreach (DataNode dataNode in DataNode.SubDataNodes)
            {
                if (dataNode is AggregateExpressionDataNode && dataNode.Alias == columnName)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Count)
                        return typeof(int);

                }
            }
            foreach (DataColumn column in DerivedDataColoumns)
            {
                if (column.Name.ToLower() == columnName.ToLower())
                    return column.Type;
            }
            if (DataNode.Recursive && columnName.ToLower() == "iteration".ToLower())
                return typeof(int);

            foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
            {
                Type columnType = hostedDataDataLoader.GetColumnType(columnName);
                if (columnType != null)
                    return columnType;
            }



            if (DataNode.Type == DataNode.DataNodeType.Group)
            {
                foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                            {
                                if (columnName == dataNode.Alias + "_" + column.Name)
                                    return column.Type;
                            }
                        }
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {

                        if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                        {
                            if (columnName == dataNode.ParentDataNode.ParentDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name)
                                return (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject<System.Type>();

                            else
                            {
                                if (columnName == dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name)
                                    return (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject<System.Type>();


                            }
                        }
                        else
                        {
                            if (columnName == dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name)
                                return (dataNode.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject<System.Type>();

                        }
                    }
                }
                if (DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in DataNode.RealParentDataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                        {

                            if (columnName == DataNode.RealParentDataNode.Alias + "_" + column.Name)
                                return column.Type;
                        }
                    }
                }

            }
            foreach (DataNode dataNode in DataNode.SubDataNodes)
            {
                if (dataNode is AggregateExpressionDataNode && dataNode.Alias == columnName)
                    return (dataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType;
            }

            //if (DataNode.Recursive)
            //{
            //    foreach (System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> entry in RecursiveParentReferenceColumns.Values)
            //    {
            //        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in entry.Keys)
            //        {
            //            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
            //            {

            //            }
            //        }
            //    }
            //}
            return null;
        }

#region Retrieves DataLoader columns

        public struct DataColumn
        {
            public DataColumn(string name,
                                string alias,
                                System.Type type, ObjectQuery objectQuery)
            {
                ObjectQuery = objectQuery;
                Name = name;
                _Alias = alias;
                Type = type;
                IdentityColumn = false;
                MappedObject = null;
                CreatorIdentity = null;
                IdentityPart = null;
                ObjectIdentityType = null;
                if (ObjectQuery is DistributedObjectQuery && (ObjectQuery as DistributedObjectQuery).ObjectsContext is ObjectStorage && ((ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectStorage).StorageMetaData is Storage)
                    RDBMSSQLScriptGenarator = (((ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectStorage).StorageMetaData as Storage).StorageDataBase.RDBMSSQLScriptGenarator;
                else
                    RDBMSSQLScriptGenarator = null;

            }

            ObjectQuery ObjectQuery;
            public readonly MetaDataRepository.ObjectIdentityType ObjectIdentityType;
            public DataColumn(string name,
                                string alias,
                                System.Type type,
                                OOAdvantech.MetaDataRepository.MetaObject mappedObject,
                                string creatorIdentity,
                                OOAdvantech.MetaDataRepository.IIdentityPart identityPart, MetaDataRepository.ObjectIdentityType objectIdentityType, ObjectQuery objectQuery)
            {
                ObjectQuery = objectQuery;
                Name = name;
                _Alias = alias;
                Type = type;
                MappedObject = mappedObject;
                CreatorIdentity = creatorIdentity;
                // IdentityColumn = identityColumn;
                IdentityPart = identityPart;
                ObjectIdentityType = objectIdentityType;
                if (identityPart != null)
                    IdentityColumn = true;
                else
                    IdentityColumn = false;

                if (ObjectQuery is DistributedObjectQuery && (ObjectQuery as DistributedObjectQuery).ObjectsContext is ObjectStorage && ((ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectStorage).StorageMetaData is Storage)
                    RDBMSSQLScriptGenarator = (((ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectStorage).StorageMetaData as Storage).StorageDataBase.RDBMSSQLScriptGenarator;
                else
                    RDBMSSQLScriptGenarator = null;


                    
            }


            public readonly OOAdvantech.MetaDataRepository.MetaObject MappedObject;
            public string CreatorIdentity;
            public readonly bool IdentityColumn;

            public readonly OOAdvantech.MetaDataRepository.IIdentityPart IdentityPart;

            OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator;

            public readonly System.Type Type;
            public readonly string Name;
            public string Alias
            {
                get
                {
                    if (RDBMSSQLScriptGenarator != null && _Alias == null && RDBMSSQLScriptGenarator.AlwaysDeclareColumnAlias)
                        return Name;

                    return _Alias;
                }
            }

            public readonly string _Alias;
        }



        /// <MetaDataID>{f9811584-00dc-404c-b961-f18d3abd8c7a}</MetaDataID>
        public DataColumn GetColumn(DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.DerivedDataNode)
                dataNode = (dataNode as DerivedDataNode).OrgDataNode;
            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
            {
                foreach (DataColumn column in ClassifierDataColumns)
                {
                    if (column.MappedObject != null && column.MappedObject.Identity == dataNode.AssignedMetaObject.Identity && column.CreatorIdentity == dataNode.ValueTypePath.ToString())
                        return column;
                }
            }
            if (dataNode is MetaDataRepository.ObjectQueryLanguage.AggregateExpressionDataNode)
            {
                if (dataNode.Type == DataNode.DataNodeType.Count)
                    return new DataColumn(dataNode.Alias, dataNode.Alias, typeof(int), DataNode.ObjectQuery);
                else
                    return new DataColumn(dataNode.Alias, dataNode.Alias, (dataNode as MetaDataRepository.ObjectQueryLanguage.AggregateExpressionDataNode).ArithmeticExpression.ResultType, DataNode.ObjectQuery);
            }


            throw new System.Exception("Column doesn't exist for dataNode");

        }



        /// <MetaDataID>{8331eea9-e8f0-474b-a0d5-bac603de0151}</MetaDataID>
        internal System.Collections.Generic.List<DataColumn> DerivedDataColoumns = new List<DataColumn>();
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<DataColumn> _ClassifierDataColumns;
        /// <MetaDataID>{0EF21F47-B73E-4D70-A960-191D61E8DF8A}</MetaDataID>
        /// ClassifierDataColoumns is a collection with necessary columns for the data 
        /// loader to retrieve data for the query. 
        internal protected System.Collections.Generic.List<DataColumn> ClassifierDataColumns
        {
            get
            {
                if (_ClassifierDataColumns == null)
                {
                    bool AddObjectIDColumns = false;

                    System.Collections.Generic.List<RDBMSMetaDataRepository.Class> concreteClassesMaximumColumns = GetMostSpecializedClasses();
                    List<string> columnNames = new List<string>();
                    System.Collections.Generic.List<DataColumn> coloumns = new System.Collections.Generic.List<DataColumn>();
                    DataNode recursiveSubDataNode = null;
                    foreach (DataNode subDataNode in DataNode.RealSubDataNodes)
                    {
                        if (subDataNode.Recursive)
                        {
                            recursiveSubDataNode = subDataNode;
                            break;
                        }
                    }

#region Retrieve parent data node reference columns
                    if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                        //  GetAssociationEndColumns(associationEnd, storageCellWithMaximumColumns, columnNames, coloumns);


                        if (HasRelationReferenceColumns)
                        {
                            foreach (string relationPartIdentity in DataSourceRelationsColumnsWithParent.Keys)
                            {
                                foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, RelationColumns> objectIdentityType in DataSourceRelationsColumnsWithParent[relationPartIdentity])
                                {
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Key.Parts)
                                    {
                                        if (!columnNames.Contains(part.Name))
                                        {
                                            coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, DataNode.ValueTypePath.ToString(), part, objectIdentityType.Key,DataNode.ObjectQuery));
                                            columnNames.Add(part.Name);
                                        }
                                    }
                                }
                            }
                            if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Indexer)
                            {
                                if (associationEnd.IndexerColumn != null && !columnNames.Contains(associationEnd.IndexerColumn.Name))
                                {
                                    coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null, associationEnd.IndexerColumn.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, associationEnd, associationEnd.IndexerColumn.CreatorIdentity, null, null, DataNode.ObjectQuery));
                                    columnNames.Add(associationEnd.IndexerColumn.Name);
                                }
                            }
                        }
                    }
#endregion

                    if (ObjectActivation)
                    {
                        AddObjectIDColumns = true;
                        foreach (RDBMSMetaDataRepository.Class _class in concreteClassesMaximumColumns)
                        {
                            if ((_class.ActiveStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ReferentialIntegrityColumn != null)
                            {
                                coloumns.Add(new DataColumn((_class.ActiveStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ReferentialIntegrityColumn.Name, null, typeof(int), null, null, null, null, DataNode.ObjectQuery));
                                break;
                            }
                        }
                        coloumns.Add(new DataColumn("TypeID", null, typeof(int), null, null, null, null, DataNode.ObjectQuery));
                        coloumns.Add(new DataColumn("StorageCellID", null, typeof(int), null, null, null, null, DataNode.ObjectQuery));
                        columnNames.Add("TypeID");
                        columnNames.Add("StorageCellID");


#region Retrieve subDanodes reference columns
                        foreach (KeyValuePair<SubDataNodeIdentity, Dictionary<string, Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>>> entry in DataSourceRelationsColumnsWithSubDataNodes)
                        {
                            DataNode dataNode = DataNode.HeaderDataNode.GetDataNode(entry.Key);
                            if (dataNode.IsParentDataNode(DataNode) && HasRelationReferenceColumnsFor(dataNode))
                            {
                                foreach (string relationPartIdentity in entry.Value.Keys)
                                {
                                    foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, RelationColumns> objectIdentityType in entry.Value[relationPartIdentity])
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Key.Parts)
                                        {
                                            if (!columnNames.Contains(part.Name))
                                            {
                                                if (dataNode.RealParentDataNode != null && dataNode.RealParentDataNode.Classifier is MetaDataRepository.Structure)
                                                    coloumns.Add(new DataColumn(part.Name, null, part.Type, (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()), dataNode.RealParentDataNode.ValueTypePath.ToString() + ".(" + (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Identity.ToString() + ")", part, objectIdentityType.Key, DataNode.ObjectQuery));
                                                else
                                                    coloumns.Add(new DataColumn(part.Name, null, part.Type, (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()), dataNode.ValueTypePath.ToString(), part, objectIdentityType.Key, DataNode.ObjectQuery));
                                                columnNames.Add(part.Name);
                                            }
                                        }
                                    }
                                }
                                if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Indexer)
                                {
                                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()) as RDBMSMetaDataRepository.AssociationEnd;
                                    if (associationEnd.IndexerColumn != null && !columnNames.Contains(associationEnd.IndexerColumn.Name))
                                    {
                                        coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null, associationEnd.IndexerColumn.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, associationEnd, associationEnd.IndexerColumn.CreatorIdentity, null, null, DataNode.ObjectQuery));
                                        columnNames.Add(associationEnd.IndexerColumn.Name);
                                    }
                                }
                            }
                        }
#endregion

#region retrieve recursion reference columns
                        if (DataNode.Recursive)
                        {

                            int rt = DataSourceRelationsColumnsWithParent.Count;
                            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                            foreach (string relationPartIdentity in RecursiveParentReferenceColumns.Keys)
                            {

                                foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> objectIdentityType in RecursiveParentReferenceColumns[relationPartIdentity])
                                {
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Key.Parts)
                                    {
                                        if (!columnNames.Contains(part.Name))
                                        {
                                            coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, DataNode.ValueTypePath.ToString(), part, objectIdentityType.Key, DataNode.ObjectQuery));
                                            columnNames.Add(part.Name);
                                        }
                                    }
                                }
                            }
                        }
                        if (recursiveSubDataNode != null && !recursiveSubDataNode.ThroughRelationTable)
                        {
                            int rt = DataSourceRelationsColumnsWithParent.Count;
                            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                            foreach (string relationPartIdentity in RecursiveParentReferenceColumns.Keys)
                            {
                                foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> objectIdentityType in RecursiveParentReferenceColumns[relationPartIdentity])
                                {
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Key.Parts)
                                    {
                                        if (!columnNames.Contains(part.Name))
                                        {
                                            coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, DataNode.ValueTypePath.ToString(), part, objectIdentityType.Key, DataNode.ObjectQuery));
                                            columnNames.Add(part.Name);
                                        }
                                    }
                                }
                            }
                        }

#endregion

#region Retrieve attributes columns
                        if (DataNode.ValueTypePath.Count == 0)
                        {
                            System.Collections.Generic.List<RDBMSMetaDataRepository.Attribute> hierarchyAttributes = new List<OOAdvantech.RDBMSMetaDataRepository.Attribute>();
                            foreach (RDBMSMetaDataRepository.Class _class in concreteClassesMaximumColumns)
                            {
                                foreach (RDBMSMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
                                {
                                    if (_class.IsPersistent(attribute) && !hierarchyAttributes.Contains(attribute))
                                    {
                                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                            DataNode.Type == DataNode.DataNodeType.Object)
                                            if (DataNode.AssignedMetaObject.Identity != attribute.Identity)
                                                continue;

                                        hierarchyAttributes.Add(attribute);
                                        GetAttributeColumns(attribute, DataNode.ValueTypePath, _class, columnNames, coloumns);
                                    }
                                }
                            }
                            if (concreteClassesMaximumColumns.Count == 0)
                            {

                                foreach (RDBMSMetaDataRepository.Attribute attribute in Classifier.GetAttributes(true))
                                {
                                    foreach (DataNode subDataNode in DataNode.SubDataNodes)
                                    {
                                        if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.AssignedMetaObject.Identity == attribute.Identity)
                                        {
                                            if (!hierarchyAttributes.Contains(attribute))
                                            {
                                                hierarchyAttributes.Add(attribute);
                                                GetAttributeColumns(attribute, DataNode.ValueTypePath, Classifier, columnNames, coloumns);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (concreteClassesMaximumColumns.Count == 0)
                            {
                                System.Collections.Generic.List<RDBMSMetaDataRepository.Attribute> hierarchyAttributes = new List<OOAdvantech.RDBMSMetaDataRepository.Attribute>();
                                RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                                GetAttributeColumns(attribute, DataNode.RealParentDataNode.ValueTypePath, Classifier, columnNames, coloumns);
                            }
                            else
                            {
                                foreach (RDBMSMetaDataRepository.Class _class in concreteClassesMaximumColumns)
                                {
                                    System.Collections.Generic.List<RDBMSMetaDataRepository.Attribute> hierarchyAttributes = new List<OOAdvantech.RDBMSMetaDataRepository.Attribute>();
                                    RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                                    GetAttributeColumns(attribute, DataNode.RealParentDataNode.ValueTypePath, _class, columnNames, coloumns);
                                }

                            }

                        }
#endregion

#region Retrieve AssociationEnd reference columns
                        if (DataNode.ValueTypePath.Count == 0)
                        {
                            foreach (RDBMSMetaDataRepository.Class _class in concreteClassesMaximumColumns)
                            {
                                foreach (RDBMSMetaDataRepository.AssociationEnd associationEnd in _class.GetRoles(true))
                                {
                                    GetAssociationEndColumns(associationEnd, _class, columnNames, coloumns);
                                }
                                if (_class.ClassHierarchyLinkAssociation != null)
                                {
                                    GetAssociationEndColumns(_class.ClassHierarchyLinkAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd, _class, columnNames, coloumns);
                                    GetAssociationEndColumns(_class.ClassHierarchyLinkAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd, _class, columnNames, coloumns);
                                }

                                foreach (RDBMSMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
                                {
                                    if (attribute.Type is MetaDataRepository.Structure && _class.IsPersistent(attribute))
                                        GetValueTypeAssociationEndColumns(attribute, DataNode.ValueTypePath, _class, columnNames, coloumns);
                                }
                            }
                        }
                        else
                        {
                            if (concreteClassesMaximumColumns.Count > 0)
                            {
                                foreach (var _class in concreteClassesMaximumColumns)
                                {
                                    RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                                    GetValueTypeAssociationEndColumns(attribute, DataNode.ParentDataNode.ValueTypePath, _class, columnNames, coloumns);
                                }
                            }

                        }
#endregion

#region Retrieve AssociationClass columns
                        //if (storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation != null)
                        //{
                        //    Collections.Generic.List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = (storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(storageCellWithMaximumColumns.Type, ObjectIdentityTypes);
                        //    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                        //    {
                        //        foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                        //        {
                        //            if (!columnNames.Contains(column.Name))
                        //            {
                        //                coloumns.Add(new DataColumn(column.Name, null, column.Type, storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleA, "", true, column as MetaDataRepository.IIdentityPart));
                        //                columnNames.Add(column.Name);
                        //            }
                        //        }
                        //    }
                        //    RDBMSMetaDataRepository.Column indexerColumn = (storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumn(storageCellWithMaximumColumns.Type, storageCellWithMaximumColumns.Type.GetClassHerarchyCaseInsensitiveUniqueNames(storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation));
                        //    if (indexerColumn != null && !columnNames.Contains(indexerColumn.Name))
                        //    {
                        //        coloumns.Add(new DataColumn(indexerColumn.Name, null, indexerColumn.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleA, indexerColumn.CreatorIdentity, false, null));
                        //        columnNames.Add(indexerColumn.Name);
                        //    }


                        //    referenceObjectIdentityTypes = (storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(storageCellWithMaximumColumns.Type, ObjectIdentityTypes);
                        //    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in referenceObjectIdentityTypes)
                        //    {
                        //        foreach (MetaDataRepository.IIdentityPart column in objectIdentityType.Parts)
                        //        {
                        //            if (!columnNames.Contains(column.Name))
                        //            {
                        //                coloumns.Add(new DataColumn(column.Name, null, column.Type, storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleB, "", true, column as MetaDataRepository.IIdentityPart));
                        //                columnNames.Add(column.Name);
                        //            }
                        //        }
                        //    }
                        //    indexerColumn = (storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumn(storageCellWithMaximumColumns.Type, storageCellWithMaximumColumns.Type.GetClassHerarchyCaseInsensitiveUniqueNames(storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation));
                        //    if (indexerColumn != null && !columnNames.Contains(indexerColumn.Name))
                        //    {
                        //        coloumns.Add(new DataColumn(indexerColumn.Name, null, indexerColumn.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, storageCellWithMaximumColumns.Type.ClassHierarchyLinkAssociation.RoleB, indexerColumn.CreatorIdentity, false, null));
                        //        columnNames.Add(indexerColumn.Name);
                        //    }
                        //}
#endregion

                    }
                    else if ((DataNode.ParticipateInSelectClause || DataNode.ParticipateInAggregateFunction) &&
                        DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (DataNode keyMember in (DataNode as GroupDataNode).GroupKeyDataNodes)
                        {
                            if (keyMember.Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                MetaDataRepository.Attribute attribute = keyMember.AssignedMetaObject as MetaDataRepository.Attribute;
                                coloumns.Add(new DataColumn(attribute.Name, DataSource.GetDataTreeUniqueColumnName(keyMember), attribute.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, DataNode.ObjectQuery));
                            }
                        }
                    }
                    else
                    {

                        if (DataNode.Type == DataNode.DataNodeType.Object)
                        {
                            foreach (var criterion in DataNode.SearchCriterions)
                            {
                                if (criterion.ComparisonOperator == Criterion.ComparisonType.TypeIs)
                                {
                                    if (!columnNames.Contains("TypeID"))
                                    {
                                        coloumns.Add(new DataColumn("TypeID", null, typeof(int), null, null, null, null, DataNode.ObjectQuery));
                                        columnNames.Add("TypeID");
                                    }
                                    break;
                                }
                            }
                        }
                        if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd ||
                            DataNode.ParticipateInWereClause)
                        {
                            AddObjectIDColumns = true;
                        }
                        else
                        {
                            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataNode.RealSubDataNodes)
                            {
                                if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                                {
                                    AddObjectIDColumns = true;
                                    break;
                                }
                            }
                        }



#region Retrieve subDanodes reference columns
                        foreach (KeyValuePair<SubDataNodeIdentity, Dictionary<string, Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>>> entry in DataSourceRelationsColumnsWithSubDataNodes)
                        {
                            DataNode dataNode = DataNode.GetDataNode(entry.Key);
                            if (dataNode == null && DataLoadedInParentDataSource && DataNode.HeaderDataNode.GetDataNode(entry.Key) != null)
                                continue;
                            if (HasRelationReferenceColumnsFor(dataNode))
                            {
                                foreach (string relationPartIdentity in entry.Value.Keys)
                                {
                                    foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, RelationColumns> objectIdentityType in entry.Value[relationPartIdentity])
                                    {
                                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Key.Parts)
                                        {
                                            if (!columnNames.Contains(part.Name))
                                            {
                                                string creatorIdentity = "";
                                                if (dataNode.RealParentDataNode.ValueTypePath.Count > 0)
                                                    creatorIdentity = dataNode.RealParentDataNode.ValueTypePath.ToString() + ".(" + (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Identity.ToString() + ")";

                                                coloumns.Add(new DataColumn(part.Name, null, part.Type, (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd()), creatorIdentity, part, objectIdentityType.Key, DataNode.ObjectQuery));
                                                columnNames.Add(part.Name);
                                            }
                                        }
                                    }
                                }
                                if (dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && (dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Indexer)
                                {
                                    RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                                    if (associationEnd.IndexerColumn != null && !columnNames.Contains(associationEnd.IndexerColumn.Name))
                                    {
                                        coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null, associationEnd.IndexerColumn.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, associationEnd, associationEnd.IndexerColumn.CreatorIdentity, null, null, DataNode.ObjectQuery));
                                        columnNames.Add(associationEnd.IndexerColumn.Name);
                                    }
                                }
                            }
                        }
#endregion

#region Retrieve subDanodes attributes columns
                        foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataNode.RealSubDataNodes)
                        {
                            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            {

                                RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(dataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
                                foreach (RDBMSMetaDataRepository.Class _class in concreteClassesMaximumColumns)
                                {
                                    if (DataNode.ValueTypePath.Count > 0)
                                    {
                                        DataNode valueTypePathHeader = DataNode;
                                        while (valueTypePathHeader.ParentDataNode.ValueTypePath.Count > 0)
                                            valueTypePathHeader = valueTypePathHeader.ParentDataNode;
                                        MetaDataRepository.Classifier valueTypeHeaderOwner = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject((valueTypePathHeader.AssignedMetaObject as MetaDataRepository.Attribute).Owner) as MetaDataRepository.Classifier;


                                        if (_class.GetAllGeneralClasifiers().Contains(valueTypeHeaderOwner) || valueTypeHeaderOwner == _class)
                                        {
                                            GetAttributeColumns(attribute, DataNode.ValueTypePath, _class, columnNames, coloumns);
                                            break;
                                        }
                                    }
                                    else if (_class.GetAllGeneralClasifiers().Contains(attribute.Owner) || attribute.Owner == _class)
                                    {
                                        GetAttributeColumns(attribute, DataNode.ValueTypePath, _class, columnNames, coloumns);
                                        break;
                                    }
                                }
                                if (DataLoaderMetadata.StorageCells.Count == 0)
                                    GetAttributeColumns(attribute, DataNode.ValueTypePath, Classifier, columnNames, coloumns);

                            }
                            if (dataNode.Type == DataNode.DataNodeType.Object && dataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                            {
                                foreach (DataColumn dataColumn in (GetDataLoader(dataNode) as DataLoader).ClassifierDataColumns)
                                {
                                    if (!columnNames.Contains(dataColumn.Name))
                                    {
                                        columnNames.Add(dataColumn.Name);
                                        coloumns.Add(dataColumn);
                                    }
                                }
                            }

                            AddObjectIDColumns = true;
                        }
#endregion

#region retrieve recursion reference columns
                        if (DataNode.Recursive)
                        {
                            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                            foreach (System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> entry in RecursiveParentReferenceColumns.Values)
                            {
                                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in entry.Keys)
                                {
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        if (!columnNames.Contains(part.Name))
                                        {
                                            coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, DataNode.ParentDataNode.ValueTypePath.ToString(), part, objectIdentityType, DataNode.ObjectQuery));
                                            columnNames.Add(part.Name);
                                        }

                                    }
                                }
                            }
                        }
                        if (recursiveSubDataNode != null)
                        {
                            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(recursiveSubDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                            foreach (System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> entry in (GetDataLoader(recursiveSubDataNode) as DataLoader).RecursiveParentReferenceColumns.Values)
                            {
                                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in entry.Keys)
                                {
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        if (!columnNames.Contains(part.Name))
                                        {
                                            coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, recursiveSubDataNode.ParentDataNode.ValueTypePath.ToString(), part, objectIdentityType, DataNode.ObjectQuery));
                                            columnNames.Add(part.Name);
                                        }

                                    }
                                }
                            }
                        }
#endregion
                    }
                    if (AddObjectIDColumns)
                    {

                        if (DataLoaderMetadata.MemoryCell != null&&HasRelationIdentityColumns)
                            foreach (var objectIdentityTypes in DataSourceRelationsColumnsWithParent.Values)
                            {
                                foreach (var objectIdentityType in objectIdentityTypes.Keys)
                                {
                                    foreach (var part in objectIdentityType.Parts)
                                    {
                                        if (!columnNames.Contains(part.Name))
                                        {
                                            coloumns.Add(new DataColumn(part.Name, null, part.Type, null, "", part, objectIdentityType, DataNode.ObjectQuery));
                                            columnNames.Add(part.Name);
                                        }
                                    }
                                }
                            }

                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                if (!columnNames.Contains(part.Name))
                                {
                                    coloumns.Add(new DataColumn(part.Name, null, part.Type, null, "", part, objectIdentityType, DataNode.ObjectQuery));
                                    columnNames.Add(part.Name);
                                }
                            }
                        }
                        if (!columnNames.Contains("StorageCellID"))
                        {
                            coloumns.Add(new DataColumn("StorageCellID", null, typeof(int), null, null, null, null, DataNode.ObjectQuery));
                            columnNames.Add("StorageCellID");
                        }
                    }
                    if (DataLoadedInParentDataSource && !(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                    {
#region Adds data node name as prefix
                        _ClassifierDataColumns = new List<DataColumn>();
                        foreach (DataColumn column in coloumns)
                        {

                            if (!string.IsNullOrEmpty(column.Alias))
                            {

                                _ClassifierDataColumns.Add(new DataColumn(column.Name,
                                                            DataNode.Alias + "_" + column.Alias,
                                                            column.Type, column.MappedObject,
                                                            column.CreatorIdentity,
                                                            column.IdentityPart, column.ObjectIdentityType, DataNode.ObjectQuery));
                            }
                            else
                            {
                                _ClassifierDataColumns.Add(new DataColumn(column.Name,
                                                            DataNode.Alias + "_" + column.Name,
                                                            column.Type, column.MappedObject,
                                                            column.CreatorIdentity,
                                                            column.IdentityPart, column.ObjectIdentityType, DataNode.ObjectQuery));
                            }
                        }
#endregion
                    }
                    else
                        _ClassifierDataColumns = coloumns;

                    foreach (DataColumn column in _ClassifierDataColumns)
                    {
                        string sqlScriptName = SqlScriptsBuilder.GetSQLScriptValidAlias(column.Name);
                    }

                }
                return _ClassifierDataColumns;
            }
        }

        /// <MetaDataID>{f2c7615e-311c-411d-bd82-0109f829e050}</MetaDataID>
        private void GetAttributeColumns(RDBMSMetaDataRepository.Attribute attribute, MetaDataRepository.ValueTypePath valueTypePath, MetaDataRepository.Classifier concreteClass, System.Collections.Generic.List<string> columnNames, System.Collections.Generic.List<DataColumn> coloumns)
        {

            string classHerarchyCaseInsensitiveUniqueName = null;

            if (valueTypePath.Count > 0)
            {
                RDBMSMetaDataRepository.Attribute memberAttribute = attribute;

                for (int i = 0; i < valueTypePath.Count; i++)
                {

                    RDBMSMetaDataRepository.Attribute valueTypeAttribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(valueTypePath.ToArray()[valueTypePath.Count - 1 - i].ToString(), typeof(MetaDataRepository.MetaObject)) as RDBMSMetaDataRepository.Attribute;
                    if (classHerarchyCaseInsensitiveUniqueName == null)
                        classHerarchyCaseInsensitiveUniqueName = valueTypeAttribute.Type.GetClassHerarchyCaseInsensitiveUniqueNames(memberAttribute);
                    else
                        classHerarchyCaseInsensitiveUniqueName = valueTypeAttribute.Type.GetClassHerarchyCaseInsensitiveUniqueNames(memberAttribute) + "_" + classHerarchyCaseInsensitiveUniqueName;
                    memberAttribute = valueTypeAttribute;
                }
                classHerarchyCaseInsensitiveUniqueName = concreteClass.GetClassHerarchyCaseInsensitiveUniqueNames(memberAttribute) + "_" + classHerarchyCaseInsensitiveUniqueName; ;

            }
            else
                classHerarchyCaseInsensitiveUniqueName = concreteClass.GetClassHerarchyCaseInsensitiveUniqueNames(attribute);





            //while(attribute.Owner
            foreach (RDBMSMetaDataRepository.Column column in attribute.GetColumns(classHerarchyCaseInsensitiveUniqueName, valueTypePath))
            {
                if (!columnNames.Contains(column.Name) && column.MappedAssociationEnd == null)
                {
                    coloumns.Add(new DataColumn(column.Name, null, column.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, column.MappedAttribute, column.CreatorIdentity, null, null, DataNode.ObjectQuery));
                    columnNames.Add(column.Name);
                }
            }
        }

        /// <MetaDataID>{f70e67dc-a51e-424f-b029-f38b8bf4983c}</MetaDataID>
        private void GetAssociationEndColumns(RDBMSMetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.Classifier concreteClass, System.Collections.Generic.List<string> columnNames, System.Collections.Generic.List<DataColumn> coloumns)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in associationEnd.MappedColumns)
                {
                    if ((identityColumn.Namespace is RDBMSMetaDataRepository.Table) && (identityColumn.Namespace as RDBMSMetaDataRepository.Table).TableCreator is RDBMSMetaDataRepository.StorageCell &&
                        DataLoaderMetadata.StorageCells.Contains((identityColumn.Namespace as RDBMSMetaDataRepository.Table).TableCreator as RDBMSMetaDataRepository.StorageCell))
                    {
                        if (!referenceObjectIdentityTypes.Contains(identityColumn.ObjectIdentityType))
                            referenceObjectIdentityTypes.Add(identityColumn.ObjectIdentityType.OriginObjectIdentityType);
                    }
                }
                if (referenceObjectIdentityTypes.Count > 0)
                {
                    foreach (var objectIdentityType in associationEnd.GetReferenceObjectIdentityTypes(referenceObjectIdentityTypes))
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                        {
                            if (!columnNames.Contains(part.Name))
                            {
                                coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, "", part, objectIdentityType, DataNode.ObjectQuery));
                                columnNames.Add(part.Name);
                            }
                        }
                    }
                    if (associationEnd.Indexer)
                    {
                        if (!columnNames.Contains(associationEnd.IndexerColumn.Name))
                        {
                            coloumns.Add(new DataColumn(associationEnd.IndexerColumn.Name, null, typeof(int), associationEnd, associationEnd.IndexerColumn.CreatorIdentity, null, null, DataNode.ObjectQuery));
                            columnNames.Add(associationEnd.IndexerColumn.Name);
                        }
                    }
                }
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{9fe6d501-64ad-4aa1-beab-79632c670b62}</MetaDataID>
        private void GetValueTypeAssociationEndColumns(RDBMSMetaDataRepository.Attribute attribute, MetaDataRepository.ValueTypePath valueTypePath, MetaDataRepository.Class concreteClass, System.Collections.Generic.List<string> columnNames, System.Collections.Generic.List<DataColumn> coloumns)
        {
            string classHerarchyCaseInsensitiveUniqueName = null;
            if (valueTypePath.Count > 0)
            {
                RDBMSMetaDataRepository.Attribute memberAttribute = attribute;
                for (int i = 0; i < valueTypePath.Count; i++)
                {
                    RDBMSMetaDataRepository.Attribute valueTypeAttribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(valueTypePath.ToArray()[valueTypePath.Count - 1 - i].ToString(), typeof(MetaDataRepository.MetaObject)) as RDBMSMetaDataRepository.Attribute;
                    if (classHerarchyCaseInsensitiveUniqueName == null)
                        classHerarchyCaseInsensitiveUniqueName = valueTypeAttribute.Type.GetClassHerarchyCaseInsensitiveUniqueNames(memberAttribute);
                    else
                        classHerarchyCaseInsensitiveUniqueName = valueTypeAttribute.Type.GetClassHerarchyCaseInsensitiveUniqueNames(memberAttribute) + "_" + classHerarchyCaseInsensitiveUniqueName;
                    memberAttribute = valueTypeAttribute;
                }
                classHerarchyCaseInsensitiveUniqueName = concreteClass.GetClassHerarchyCaseInsensitiveUniqueNames(memberAttribute) + "_" + classHerarchyCaseInsensitiveUniqueName; ;
            }
            else
                classHerarchyCaseInsensitiveUniqueName = concreteClass.GetClassHerarchyCaseInsensitiveUniqueNames(attribute);

            if (!concreteClass.IsPersistent(attribute))
                return;
            foreach (RDBMSMetaDataRepository.Column column in attribute.GetColumns(classHerarchyCaseInsensitiveUniqueName))
            {
                if (!columnNames.Contains(column.Name) && column.MappedAssociationEnd != null)
                {
                    foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in column.MappedAssociationEnd.MappedColumns)
                    {
                        if (identityColumn.Namespace is RDBMSMetaDataRepository.Table && (identityColumn.Namespace as RDBMSMetaDataRepository.Table).TableCreator is RDBMSMetaDataRepository.StorageCell &&
                            DataLoaderMetadata.StorageCells.Contains((identityColumn.Namespace as RDBMSMetaDataRepository.Table).TableCreator as RDBMSMetaDataRepository.StorageCell))
                        {
                            if ((column as RDBMSMetaDataRepository.IdentityColumn).CreatorIdentity == identityColumn.CreatorIdentity &&
                                (column as RDBMSMetaDataRepository.IdentityColumn).ObjectIdentityType == identityColumn.ObjectIdentityType &&
                                !columnNames.Contains(column.Name))
                            {
                                coloumns.Add(new DataColumn(column.Name, null, column.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, column.MappedAssociationEnd, column.CreatorIdentity, (column as RDBMSMetaDataRepository.IdentityColumn), (column as RDBMSMetaDataRepository.IdentityColumn).ObjectIdentityType, DataNode.ObjectQuery));
                                columnNames.Add(column.Name);
                            }
                        }
                    }
                }
            }
        }

        /// <MetaDataID>{8cdf44a5-a6fa-480f-9ffe-f3d80fa068ec}</MetaDataID>
        private void GetValueTypeDataNodeColumns(DataNode valuTypeDataNode, MetaDataRepository.Classifier concreteClass, List<string> columnNames, System.Collections.Generic.List<DataColumn> coloumns)
        {
            RDBMSMetaDataRepository.Attribute attribute = (Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(valuTypeDataNode.AssignedMetaObject) as RDBMSMetaDataRepository.Attribute;
            {
                if (valuTypeDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity].ObjectActivation)
                {
                    foreach (RDBMSMetaDataRepository.Column column in attribute.GetColumns(concreteClass.GetClassHerarchyCaseInsensitiveUniqueNames(attribute)))
                    {
                        if (!columnNames.Contains(column.Name) && column.MappedAssociationEnd == null)
                        {
                            coloumns.Add(new DataColumn(column.Name, null, column.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, column.MappedAttribute, column.CreatorIdentity, null, null, DataNode.ObjectQuery));
                            columnNames.Add(column.Name);
                        }
                    }
                }
                else
                {
                    foreach (RDBMSMetaDataRepository.Column column in attribute.GetColumns(concreteClass.GetClassHerarchyCaseInsensitiveUniqueNames(attribute)))
                    {
                        if (!columnNames.Contains(column.Name) && column.MappedAssociationEnd == null)
                        {
                            foreach (DataNode subDataNode in valuTypeDataNode.SubDataNodes)
                            {
                                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                                {

                                }
                                else
                                {
                                    if (column.MappedAttribute != null && subDataNode.AssignedMetaObject != null && subDataNode.AssignedMetaObject.Identity == column.MappedAttribute.Identity)
                                    {
                                        coloumns.Add(new DataColumn(column.Name, null, column.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, column.MappedAttribute, column.CreatorIdentity, null, null, DataNode.ObjectQuery));
                                        columnNames.Add(column.Name);
                                        break;
                                    }
                                    if (column.MappedAssociationEnd != null && subDataNode.AssignedMetaObject != null && subDataNode.AssignedMetaObject.Identity == column.MappedAssociationEnd.Identity)
                                    {
                                        MetaDataRepository.ObjectIdentityType objectIdentityType = null;
                                        if (column is RDBMSMetaDataRepository.IdentityColumn)
                                            objectIdentityType = (column as RDBMSMetaDataRepository.IdentityColumn).ObjectIdentityType;
                                        coloumns.Add(new DataColumn(column.Name, null, column.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type, column.MappedAssociationEnd, column.CreatorIdentity, column as RDBMSMetaDataRepository.IdentityColumn, objectIdentityType, DataNode.ObjectQuery));
                                        columnNames.Add(column.Name);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

#endregion



        /// <MetaDataID>{457a4f0a-1d7f-43e8-9f92-803c33eda700}</MetaDataID>
        private System.Collections.Generic.List<RDBMSMetaDataRepository.Class> GetMostSpecializedClasses()
        {
            System.Collections.Generic.List<RDBMSMetaDataRepository.Class> mostSpecializedClasses = new List<OOAdvantech.RDBMSMetaDataRepository.Class>();
            RDBMSMetaDataRepository.StorageCell storageCellWithMaximumColumns = null;
            System.Collections.Generic.List<MetaDataRepository.Class> storageCellsClasses = new List<OOAdvantech.MetaDataRepository.Class>();

            foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
            {
                if (!storageCellsClasses.Contains(storageCell.Type))
                    storageCellsClasses.Add(storageCell.Type);

            }

            foreach (MetaDataRepository.Class _class in storageCellsClasses)
            {
                bool mostSpecializedClass = true;
                foreach (MetaDataRepository.Classifier classifier in _class.GetAllSpecializeClasifiers())
                {

                    if (classifier is RDBMSMetaDataRepository.Class && storageCellsClasses.Contains((classifier as RDBMSMetaDataRepository.Class)))
                    {

                        mostSpecializedClass = false;
                        break;
                    }
                }
                if (mostSpecializedClass)
                    mostSpecializedClasses.Add(_class as RDBMSMetaDataRepository.Class);
            }
            return mostSpecializedClasses;
            //foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
            //{
            //    if (storageCell is RDBMSMetaDataRepository.StorageCell)
            //    {
            //        if (storageCellWithMaximumColumns == null)
            //            storageCellWithMaximumColumns = storageCell as RDBMSMetaDataRepository.StorageCell;
            //        else if (storageCellWithMaximumColumns.ClassView.ViewColumnsNames.Count < (storageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumnsNames.Count)
            //            storageCellWithMaximumColumns = storageCell as RDBMSMetaDataRepository.StorageCell;
            //    }
            //}
            //foreach (MetaDataRepository.StorageCell storageCell in StorageCellOfObjectUnderTransaction)
            //{
            //    if (storageCellWithMaximumColumns == null)
            //        storageCellWithMaximumColumns = storageCell as RDBMSMetaDataRepository.StorageCell;
            //    else if (storageCellWithMaximumColumns.ClassView.ViewColumnsNames.Count < (storageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumnsNames.Count)
            //        storageCellWithMaximumColumns = storageCell as RDBMSMetaDataRepository.StorageCell;
            //}
            //// return storageCellWithMaximumColumns;
            //return mostSpecializedClasses;
        }


        /// <MetaDataID>{65686eb5-612a-4513-91eb-8a5dd7da6c15}</MetaDataID>
        new internal bool CheckAggregateFunctionForLocalResolve(AggregateExpressionDataNode aggregateExpressionDataNode)
        {
            return base.CheckAggregateFunctionForLocalResolve(aggregateExpressionDataNode);
        }
    }

    /// <MetaDataID>{df46b742-a5b8-4610-9175-3f0a906b45d2}</MetaDataID>
    public enum TableJoinType
    {
        Inner,
        Left,
        Right
    }


    /// <summary>
    /// Has class members mapped columns indices on data table row 
    /// </summary>
    /// <MetaDataID>{a5add89a-2008-4610-b2cc-cb423fe776d6}</MetaDataID>
    internal class ClassMembersMappedColumnsIndices
    {
        /// <summary>
        /// This member keeps dictionaries with attributes indices on data table row. 
        /// </summary>
        /// <MetaDataID>{57ec23d1-34e0-4b07-9749-dcf6b7912117}</MetaDataID>
        public Dictionary<MetaDataRepository.MetaObjectID, Dictionary<string, int>> AttributeIndices;
        /// <summary>
        /// This member keeps dictionaries with association end indices indices on data table row. 
        /// </summary>
        /// <MetaDataID>{b6145795-1181-441e-b09c-45a31cf603d5}</MetaDataID>
        public Dictionary<CreatorIdentity, Dictionary<MetaDataRepository.MetaObjectID, Dictionary<MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>>>> AssociationEndIndices;

        ///<summary>
        ///
        ///</summary>
        /// <MetaDataID>{8c0097e8-23e0-4ef3-912f-bf7828d1cbc5}</MetaDataID>
        public Dictionary<CreatorIdentity, Dictionary<MetaDataRepository.MetaObjectID, int>> AssociationEndIndexerColumnIndices = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, int>>();
    }


    /// <MetaDataID>{ad1f1c34-79de-44d3-bf8b-480886b40e04}</MetaDataID>
    public class RelatedDataNode
    {
        public readonly bool InnerJoin;
        public readonly DataNode DataNode;
        public readonly Stack<DataNode> Route;

        public RelatedDataNode(bool innerJoin, DataNode dataNode, Stack<DataNode> route)
        {
            InnerJoin = innerJoin;
            DataNode = dataNode;
            Route = route;
        }
        public override int GetHashCode()
        {
            int num = -1162279000;
            num = (-1521134295 * num) + GetHashCode(DataNode);
            num = (-1521134295 * num) + GetHashCode(Route);
            return num;
        }
        public override bool Equals(object obj)
        {
            if (obj is RelatedDataNode && (obj as RelatedDataNode).DataNode == DataNode && (obj as RelatedDataNode).Route == Route)
                return true;
            return false;
        }

        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }



    }



}
