using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OOAdvantech.Droid
{
    /// <MetaDataID>{45ebb9c2-79ae-48ef-95dc-df47ab12fc8c}</MetaDataID>
    public class SQLiteDBConnaction : IDBConnaction
    {
        SQLitePCL.sqlite3 db;
        public SQLiteDBConnaction()
        {
            
            try
            {
                var sqliteFilename = "MyDatabase.db3";
                sqliteFilename = "MyDatabase.sqlite";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);


                string sqlScript = @"SELECT [ObjectID],[TypeID],[Invoiced],[State],[ItemsNumber],[Name],datetime([OrderDate]) AS[OrderDate],[ClientOrders_ObjectIDB],[Orders_Indexer] FROM [T_Order]";
                int res = SQLitePCL.raw.sqlite3_open(path, out db);
                if (res == SQLitePCL.raw.SQLITE_OK)
                {
                    SQLitePCL.sqlite3_stmt sqlite_stmt = null;
                    int er = SQLitePCL.raw.sqlite3_prepare_v2(db, sqlScript, out sqlite_stmt);
                    if (er == SQLitePCL.raw.SQLITE_OK)
                    {
                        int columnsCount = SQLitePCL.raw.sqlite3_column_count(sqlite_stmt);
                        for (int i = 0; i != columnsCount; i++)
                        {

                            int typeId = SQLitePCL.raw.sqlite3_column_type(sqlite_stmt, i);
                            string columnName = SQLitePCL.raw.sqlite3_column_name(sqlite_stmt, i);
                        }
                        while (SQLitePCL.raw.sqlite3_step(sqlite_stmt) == SQLitePCL.raw.SQLITE_ROW)
                        {
                            columnsCount = SQLitePCL.raw.sqlite3_column_count(sqlite_stmt);
                            for (int i = 0; i != columnsCount; i++)
                            {
                                object value = null;
                                int typeId = SQLitePCL.raw.sqlite3_column_type(sqlite_stmt, i);
                                string columnName = SQLitePCL.raw.sqlite3_column_name(sqlite_stmt, i);
                                if (typeId == SQLitePCL.raw.SQLITE_TEXT)
                                    value = SQLitePCL.raw.sqlite3_column_text(sqlite_stmt, i);
                                if (typeId == SQLitePCL.raw.SQLITE_INTEGER)
                                    value = SQLitePCL.raw.sqlite3_column_int(sqlite_stmt, i);
                                if (typeId == SQLitePCL.raw.SQLITE_FLOAT)
                                    value = SQLitePCL.raw.sqlite3_column_double(sqlite_stmt, i);
                                if (typeId == SQLitePCL.raw.SQLITE_NULL)
                                    value = null;
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = SQLitePCL.raw.sqlite3_errmsg(db);
                    }
                    SQLitePCL.raw.sqlite3_close(db);
                }



            }
            catch (Exception error)
            {


            }
          
        }

    
        public bool IsOpen
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}