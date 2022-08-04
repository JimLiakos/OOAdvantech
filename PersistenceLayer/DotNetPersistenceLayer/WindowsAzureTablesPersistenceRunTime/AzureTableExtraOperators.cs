using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Cosmos.Table;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{2d4ec483-3d16-4e96-821f-1a3f8ca470a3}</MetaDataID>
    public static class AzureTableExtraOperators
    {

        /// <MetaDataID>{8f4491db-e7ee-4a65-9564-10e8448ff67e}</MetaDataID>
        static public void InsertEntity(this Microsoft.Azure.Cosmos.Table.CloudTable table, Microsoft.Azure.Cosmos.Table.TableEntity tableEntity)
        {
            TableOperation insertOrMergeOperation = TableOperation.Insert(tableEntity);
            TableResult result = table.Execute(insertOrMergeOperation);

        }

        static public void UpdateEntity(this CloudTable table, Microsoft.Azure.Cosmos.Table.TableEntity tableEntity)
        {

            TableOperation insertOrUpdateOperation = TableOperation.InsertOrReplace(tableEntity);
            TableResult result = table.Execute(insertOrUpdateOperation);

        }


        //public static List<T> CreateQuery<T>(this Azure.Data.Tables.TableClient table,string query) where T : class, ITableEntity
        //{

        //    var result = table.Query<T>(query);

        //    //List<T> results = typeof<

        //    //// Details on pagination available here: https://docs.microsoft.com/en-us/dotnet/azure/sdk/pagination
        //    //// if you want to extend this in a paged version / understand more.
        //    //await foreach (var page in result.AsPages())
        //    //{
        //    //    results.AddRange(page.Values);
        //    //}

        //    //return results;

        //    //TableOperation insertOrMergeOperation = TableOperation.Insert(tableEntity);
        //    //TableResult result = table.Execute(insertOrMergeOperation);
        //    return null;

        //}
    }
}
