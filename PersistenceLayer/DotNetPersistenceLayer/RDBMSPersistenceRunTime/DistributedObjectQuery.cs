using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime
{

    /// <MetaDataID>{62649b2a-8c44-425d-a86a-a34733a82c94}</MetaDataID>
    public class DistributedObjectQuery:MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery
    {
        /// <MetaDataID>{dac8724e-14b9-45b8-8547-ecefc62f0ce5}</MetaDataID>
        public DistributedObjectQuery(Guid queryIdentity,
           OOAdvantech.Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataNode> dataTrees,
            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.QueryResultType queryResult,
            OOAdvantech.Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataNode> selectListItems,
           MetaDataRepository.ObjectQueryLanguage.IObjectQueryPartialResolver objectStorage,
           OOAdvantech.Collections.Generic.Dictionary<Guid, MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> dataLoadersMetadata,
           OOAdvantech.Collections.Generic.Dictionary<string, object> parameters,
            System.Collections.Generic.List<string> usedAliases,
            System.Collections.Generic.List<string> queryStorageIdentities) :
            base(queryIdentity,dataTrees, queryResult, selectListItems, objectStorage, dataLoadersMetadata, parameters, usedAliases, queryStorageIdentities)
        {

        }
        /// <MetaDataID>{4803032f-463e-433e-93d0-c010d85ffdbb}</MetaDataID>
        public RDBMSDataObjects.IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator
        {
            get
            {
                return ((ObjectsContext as ObjectStorage).StorageMetaData as Storage).StorageDataBase.RDBMSSQLScriptGenarator;
            }
        }
        /// <MetaDataID>{87af1770-40ff-4681-b55e-e835455e9548}</MetaDataID>
        Dictionary<string, string> AliasesDictionary=new Dictionary<string,string>();
        /// <MetaDataID>{0e244c9a-113e-4357-b5f9-3fd368cc92e6}</MetaDataID>
        protected override void LoadData()
        
        {
            foreach (DataLoader dataLoader in DataLoaders.Values)
            {
                if (!string.IsNullOrEmpty(dataLoader.DataNode.Alias))
                {
                    string validAlias = RDBMSSQLScriptGenarator.GeValidRDBMSName(dataLoader.DataNode.Alias, new List<string>(AliasesDictionary.Keys));
                    AliasesDictionary[validAlias] = dataLoader.DataNode.Alias;
                    dataLoader.DataNode.Alias = validAlias;
                }
            }
            base.LoadData();
            foreach (DataLoader dataLoader in DataLoaders.Values)
            {
                if (!string.IsNullOrEmpty(dataLoader.DataNode.Alias))
                    dataLoader.DataNode.Alias = AliasesDictionary[dataLoader.DataNode.Alias];
            }
                
            return;
            try
            {
                OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection = null;
                System.Collections.Generic.List<string> sqlScripts = new List<string>();
                System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader> dataLoaders = new List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader>();
                foreach (DataLoader dataLoader in DataLoaders.Values)
                    dataLoaders.Add(dataLoader);

                dataLoaders.Sort(new DataLoader.DataLoaderSorting());
                foreach (DataLoader dataLoader in dataLoaders)
                {
                    if (connection == null)
                        connection = (dataLoader.Storage as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
                    dataLoader.GetSQLScripts(sqlScripts);
                }
                lock (connection)
                {
                    if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                        connection.Open();

                    var command = connection.CreateCommand();

                    foreach (string sqlScript in sqlScripts)
                        command.CommandText = command.CommandText + sqlScript + ";\r\n";

                   // System.Data.DataSet data = new System.Data.DataSet();

                    var dataReader = command.ExecuteReader();

                    int i = 0;
                    foreach (DataLoader dataLoader in DataLoaders.Values)
                        dataLoader.RetrieveFromDataReader(dataReader, i);
                    while (dataReader.NextResult())
                    {
                        i++;
                        foreach (DataLoader dataLoader in DataLoaders.Values)
                        {
                            dataLoader.RetrieveFromDataReader(dataReader, i);
                        }
                    }
                    dataReader.Close();


                    foreach (DataLoader dataLoader in DataLoaders.Values)
                        dataLoader.ActivateObjects();

            
                }
            }
            finally
            {
                foreach (DataLoader dataLoader in DataLoaders.Values)
                {
                    try
                    {
                        dataLoader.OnAllQueryDataLoaded();
                    }
                    catch (Exception error)
                    {
                    }
                }
            }
            

        }
    }
}
