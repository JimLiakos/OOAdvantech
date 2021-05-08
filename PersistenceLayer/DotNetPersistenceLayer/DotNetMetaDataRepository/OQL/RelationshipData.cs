using System;
using System.Collections.Generic;
using System.Text;
using PartialRelationIdentity = System.String;
using System.Runtime.Serialization;
//#if !PORTABLE
//using System.Data;
//#endif 

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <summary>
    /// Defines usefull data for relation with association table
    /// </summary>
    /// <MetaDataID>{622efa88-31b6-4138-b5a4-687e674e0f3b}</MetaDataID>
    [Serializable]
    public class AssotiationTableRelationshipData : System.Runtime.Serialization.IDeserializationCallback, System.Runtime.Serialization.ISerializable
    {

        /// <summary>
        /// Defines the object identity types for sub or detail DataNode   DataSource 
        /// </summary>
        /// <MetaDataID>{59afc153-ce01-4d22-9d70-92c939f8e722}</MetaDataID>
        public Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>> DetailDataSourceReferenceObjectIdentityTypes;


        /// <summary>
        /// Defines the object identity types for Master DataNode  DataSource 
        /// </summary>
        /// <MetaDataID>{c32139e5-84bb-4fdc-a4e0-61b15221ca84}</MetaDataID>
        public Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>> MasterDataSourceReferenceObjectIdentityTypes;

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        IDataTable _Data;

        /// <MetaDataID>{e8a5bc27-b9cd-4a17-a5bc-b1c29e342145}</MetaDataID>
        public IDataTable Data
        {
            get
            {
                if (StreamedTable.StreamedData != null && _Data == null)
                    _Data = DataSource.DataObjectsInstantiator.CreateDataTable(StreamedTable);
                else
                {
                    if (_Data == null)
                        _Data = DataSource.DataObjectsInstantiator.CreateDataTable();
                }
                return _Data;

            }
        }
        /// <MetaDataID>{21a7ae4b-eb7b-4137-b6c5-e7763f24f301}</MetaDataID>
        DataLoader.StreamedTable StreamedTable;
        /// <MetaDataID>{5948285c-a116-43d7-a281-50f96d8e2d83}</MetaDataID>
        public readonly Guid DetailDataNodeIdentity;
        /// <MetaDataID>{9f32dc25-bf5c-45c2-b6d3-0f5536d96b37}</MetaDataID>
        string DataTableName;

        Roles MasterAssociationEndRole;

        /// <MetaDataID>{4cab05f7-ab8c-4d02-8ada-7eed07397600}</MetaDataID>
        public AssotiationTableRelationshipData(Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>> masterDataSourceReferenceObjectIdentityTypes,
            Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>> detailDataSourceReferenceObjectIdentityTypes,
            IDataTable data,
            Roles masterAssociationEndRole,
            Guid masterDataNodeIdentity,
            Guid detailDataNodeIdentity)
        {
            DetailDataNodeIdentity = detailDataNodeIdentity;
            MasterDataSourceReferenceObjectIdentityTypes = masterDataSourceReferenceObjectIdentityTypes;
            DetailDataSourceReferenceObjectIdentityTypes = detailDataSourceReferenceObjectIdentityTypes;
            _Data = data;
            MasterAssociationEndRole = masterAssociationEndRole;
            DataTableName = data.TableName;
            if (_Data.Rows.Count == 0)
            {
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable();
                _Data.TableName = DataTableName;
                foreach (string relationIdentityPart in masterDataSourceReferenceObjectIdentityTypes.Keys)
                {
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in masterDataSourceReferenceObjectIdentityTypes[relationIdentityPart])
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            _Data.Columns.Add(part.Name, part.Type);
                    }
                }
                foreach (string relationIdentityPart in detailDataSourceReferenceObjectIdentityTypes.Keys)
                {

                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in detailDataSourceReferenceObjectIdentityTypes[relationIdentityPart])
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            _Data.Columns.Add(part.Name, part.Type);
                    }
                }

                _Data.Columns.Add("RoleAStorageIdentity", typeof(int)).ReadOnly = false;
                _Data.Columns.Add("RoleBStorageIdentity", typeof(int)).ReadOnly = false;
            }
        }




        #region IDeserializationCallback Members

        /// <MetaDataID>{feccaf9e-5c31-461a-a4dc-9868df792b2f}</MetaDataID>
        public void OnDeserialization(object sender)
        {
            // throw new NotImplementedException();
        }

        #endregion


        /// <summary>
        /// This method returns the column names which refered to the master DataSource for relation part and object identity type
        /// </summary>
        /// <param name="relationPartIdentity">
        /// Defines the relation part identity 
        /// One relationship can conatains one or more relation parts
        /// </param>
        /// <param name="objectIdentityType">
        /// Defines the object identity type 
        /// One relationship can use one more object idintity types
        /// </param>
        /// <param name="useStorageIdintityInTablesRelations">
        /// When is true the table relation must be contains the storage identity columns in parent and child columns collections
        /// </param>
        /// <returns>
        /// Returns the column names which refered to the master DataSource  
        /// </returns>
        /// <MetaDataID>{499be010-6fdb-40fe-a146-a243403315af}</MetaDataID>
        internal List<string> GetDataSourceRelatedColumns(string relationPartIdentity, ObjectIdentityType objectIdentityType, bool useStorageIdintityInTablesRelations)
        {
            List<string> relatedColumns = new List<string>();
            foreach (MetaDataRepository.ObjectIdentityType dataSourceObjectIdentityType in MasterDataSourceReferenceObjectIdentityTypes[relationPartIdentity])
            {
                if (objectIdentityType == dataSourceObjectIdentityType)
                {
                    foreach (IIdentityPart part in dataSourceObjectIdentityType.Parts)
                        relatedColumns.Add(part.Name);
                    break;
                }
            }
            if (useStorageIdintityInTablesRelations)
            {
                if (MasterAssociationEndRole == Roles.RoleA)
                    relatedColumns.Add("RoleAStorageIdentity");
                else
                    relatedColumns.Add("RoleBStorageIdentity");
            }

            return relatedColumns;
        }



        /// <summary>
        /// This method returns the column names which refered to the details DataSource for relation part and object identity type
        /// </summary>
        /// <param name="relationPartIdentity">
        /// Defines the relation part identity 
        /// One relationship can conatains one or more relation parts
        /// </param>
        /// <param name="objectIdentityType">
        /// Defines the object identity type 
        /// One relationship can use one more object idintity types
        /// </param>
        /// <returns>
        /// Returns the column names which refered to the details DataSource  
        /// </returns>
        /// <MetaDataID>{7339999b-bc92-42bc-8e41-5c438a4efc2e}</MetaDataID>
        internal List<string> GetSubNodeDataSourceRelatedColumns(string relationPartIdentity, ObjectIdentityType objectIdentityType, bool useStorageIdintityInTablesRelations)
        {
            List<string> relatedColumns = new List<string>();


            foreach (MetaDataRepository.ObjectIdentityType dataSourceObjectIdentityType in DetailDataSourceReferenceObjectIdentityTypes[relationPartIdentity])
            {
                if (objectIdentityType == dataSourceObjectIdentityType)
                {
                    foreach (IIdentityPart part in dataSourceObjectIdentityType.Parts)
                        relatedColumns.Add(part.Name);
                    break;
                }
            }

            if (useStorageIdintityInTablesRelations)
            {
                if (MasterAssociationEndRole == Roles.RoleA)
                    relatedColumns.Add("RoleBStorageIdentity");
                else
                    relatedColumns.Add("RoleAStorageIdentity");
            }

            return relatedColumns;
        }
#if !PORTABLE
        public AssotiationTableRelationshipData(SerializationInfo info, StreamingContext ctxt)
        {
            var streamedTable = info.GetValue("StreamedTable", typeof(DataLoader.StreamedTable));
            string streamedTableName = info.GetValue("StreamedTableName", typeof(string)) as string;
            if (streamedTable is DataLoader.StreamedTable)
            {
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable((DataLoader.StreamedTable)streamedTable);
                _Data.TableName = streamedTableName;
            }

            DetailDataNodeIdentity = (System.Guid)info.GetValue("DetailDataNodeIdentity", typeof(System.Guid));
            MasterDataSourceReferenceObjectIdentityTypes = info.GetValue("MasterDataSourceReferenceObjectIdentityTypes", typeof(Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>>)) as Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>>;
            DetailDataSourceReferenceObjectIdentityTypes = info.GetValue("DetailDataSourceReferenceObjectIdentityTypes", typeof(Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>>)) as Dictionary<PartialRelationIdentity, List<MetaDataRepository.ObjectIdentityType>>;
            MasterAssociationEndRole = (Roles)info.GetValue("MasterAssociationEndRole", typeof(Roles));

        }
        #region ISerializable Members

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("StreamedTable", (Data as DataTable).SerializeTable());
            info.AddValue("StreamedTableName", Data.TableName);
            info.AddValue("DetailDataNodeIdentity", DetailDataNodeIdentity);
            info.AddValue("MasterDataSourceReferenceObjectIdentityTypes", MasterDataSourceReferenceObjectIdentityTypes);
            info.AddValue("DetailDataSourceReferenceObjectIdentityTypes", DetailDataSourceReferenceObjectIdentityTypes);
            info.AddValue("MasterAssociationEndRole", MasterAssociationEndRole);
        }

        #endregion
#endif
        internal void Merge(AssotiationTableRelationshipData sourceAssotiationTableRelationshipData)
        {
            #region Update target Assotiation Table Reletionship Data ObjectIdentityTpes

            foreach (var partialRelationIdentity in sourceAssotiationTableRelationshipData.MasterDataSourceReferenceObjectIdentityTypes.Keys)
            {
                if (!MasterDataSourceReferenceObjectIdentityTypes.ContainsKey(partialRelationIdentity))
                    MasterDataSourceReferenceObjectIdentityTypes[partialRelationIdentity] = sourceAssotiationTableRelationshipData.MasterDataSourceReferenceObjectIdentityTypes[partialRelationIdentity];
                else
                {
                    foreach (var objectIdentityType in sourceAssotiationTableRelationshipData.MasterDataSourceReferenceObjectIdentityTypes[partialRelationIdentity])
                        if (!MasterDataSourceReferenceObjectIdentityTypes[partialRelationIdentity].Contains(objectIdentityType))
                            MasterDataSourceReferenceObjectIdentityTypes[partialRelationIdentity].Add(objectIdentityType);
                }

                if (!DetailDataSourceReferenceObjectIdentityTypes.ContainsKey(partialRelationIdentity))
                    DetailDataSourceReferenceObjectIdentityTypes[partialRelationIdentity] = sourceAssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes[partialRelationIdentity];
                else
                {
                    foreach (var objectIdentityType in sourceAssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes[partialRelationIdentity])
                        if (!DetailDataSourceReferenceObjectIdentityTypes[partialRelationIdentity].Contains(objectIdentityType))
                            DetailDataSourceReferenceObjectIdentityTypes[partialRelationIdentity].Add(objectIdentityType);
                }
            }

            #endregion

            Data.Merge(sourceAssotiationTableRelationshipData.Data);
            //Merge(Data, sourceAssotiationTableRelationshipData.Data);
        }
    }

    /// <MetaDataID>{de360626-4c74-484c-9e84-aef0dce06e2b}</MetaDataID>
    [Serializable]
    public class RelationColumns
    {
        RelationColumns()
        {
            ObjectIdentityColumns = null;
            StorageIdentityColumn = null;
        }
        public RelationColumns(List<string> objectIdentityColumns, string storageIdentityColumn)
        {
            ObjectIdentityColumns = objectIdentityColumns;
            StorageIdentityColumn = storageIdentityColumn;
        }
        public List<string> ObjectIdentityColumns;
        public string StorageIdentityColumn;
    }

    /// <summary>
    /// Defines the data for a partial relation
    /// </summary>
    /// <MetaDataID>{0235d266-a47f-4824-ba31-2865ef8c0993}</MetaDataID>
    [Serializable]
    public struct PartialRelationData
    {
        /// <summary>
        /// Initialize the partial relation data
        /// </summary>
        /// <param name="relationPartIdentity">
        /// Defines the identity of partial relation
        /// </param>
        /// <param name="relationName">
        /// Defines the partial relation name
        /// </param>
        /// <param name="objectIdentityType">
        /// defines the object identity type of partial relation
        /// the relation use the object identity columns and the reference object identity coloummns
        /// the the object identities must be belongs to the same object identity type
        /// </param>
        public PartialRelationData(string relationPartIdentity, string relationName, ObjectIdentityType objectIdentityType)
        {
            RelationPartIdentity = relationPartIdentity;
            RelationName = relationName;
            ObjectIdentityType = objectIdentityType;
        }
        /// <summary>
        /// Defines the partial relation name
        /// </summary>
        public readonly string RelationName;
        /// <summary>
        /// Defines the partial relation identity
        /// </summary>
        public readonly string RelationPartIdentity;
        /// <summary>
        /// Defines the object identity type of partial relation
        /// </summary>
        public readonly ObjectIdentityType ObjectIdentityType;
    }
    /// <summary>
    /// This class defines useful information about the relation of sub data node and parent.
    /// </summary>
    /// <MetaDataID>{5e42bf3b-a5a9-4081-baa1-f4054dfe9737}</MetaDataID>
    [Serializable]
    public class DataNodesRelationshipData
    {
        /// <summary>
        /// Defines the retated data node from parent data node sub data node relation
        /// </summary>
        public readonly DataNode RelatedDataNode;


        /// <summary>
        /// DataNodesRelationshipData object constructor with one parameter  
        /// initialize the object.
        /// </summary>
        /// <param name="subDataNode">
        /// Defines the sub data node from parent data node sub data node relation
        /// </param>
        public DataNodesRelationshipData(DataNode relatedDataNode)
        {
            RelatedDataNode = relatedDataNode;
        }


        ///<summary>
        ///Defines the relation data between association table and sub datanode table 
        ///</summary>
        public List<PartialRelationData> AssociationRelationsData = new List<PartialRelationData>();

        ///<summary>
        ///Defines the relation data between datanode and association table  
        ///</summary>
        public List<PartialRelationData> RelationsData = new List<PartialRelationData>();

        public List<PartialRelationData> RecursiveRelationsData = new List<PartialRelationData>();

        public AssotiationTableRelationshipData AssotiationTableRelationshipData;
        public List<string> RecursiveRelationNames = new List<string>();

    }
}
