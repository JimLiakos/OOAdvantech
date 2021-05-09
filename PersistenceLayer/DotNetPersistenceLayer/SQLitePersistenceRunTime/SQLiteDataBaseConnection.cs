
#if WINDOWS_PHONE && SILVERLIGHT
#define USE_CSHARP_SQLITE
#define SQLITE_SUPPORT_GUID
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.RDBMSMetaDataPersistenceRunTime;

#if USE_CSHARP_SQLITE
using Community.CsharpSqlite;
using Sqlite3DatabaseHandle = Community.CsharpSqlite.Sqlite3.sqlite3;
using Sqlite3Statement = Community.CsharpSqlite.Sqlite3.Vdbe;
#else
using Sqlite3DatabaseHandle = System.IntPtr;
#if NetStandard
using Sqlite3Statement = SQLitePCL.sqlite3_stmt;
#else
using Sqlite3Statement = System.IntPtr;
#endif

#endif
namespace OOAdvantech.SQLitePersistenceRunTime
{
    /// <MetaDataID>{b8ec683d-d008-4a37-9967-d6e696f73774}</MetaDataID>
    public class SQLiteDataBaseConnection :IDataBaseConnection
    {

        public SQLiteDataBaseConnection()
        {

        }
        string _SQLiteFilePath;
        public string SQLiteFilePath
        {
            get
            {
                return _SQLiteFilePath;
            }
            set
            {
                if (State == ConnectionState.Closed)
                    _SQLiteFilePath = value;
                else
                    throw new System.Exception("You can set SQLiteFilePath only when connection is closed");
            }
        }
     

        public string ConnectionString
        {
            get
            {
                return SQLiteFilePath;
            }

            set
            {
                SQLiteFilePath = value;
            }
        }

        public object NativeConnaction
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ConnectionState _State = ConnectionState.Closed;
        public ConnectionState State
        {
            get
            {
                return _State;
            }
        }

        public void Close()
        {
            if (_State == ConnectionState.Open)
            {
                _nativeConnaction.Dispose();
                _nativeConnaction = null;
                _State = ConnectionState.Closed;
            }

        }

        public IDataBaseCommand CreateCommand()
        {
            return new SQLiteCommand(this);
        }

        public void EnlistTransaction(System.Transactions.Transaction transaction)
        {
            //throw new NotImplementedException();
        }
        internal SQLite.SQLiteConnection _nativeConnaction;
        public void Open()
        {
#if DeviceDotNet

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();

            OOAdvantech.IFileSystem fileSystem=  deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;

#if WINDOWS_PHONE && SILVERLIGHT
            if (fileSystem.FileExists(SQLiteFilePath))
            {
                _nativeConnaction = new SQLite.SQLiteConnection(SQLiteFilePath,true);
                _nativeConnaction.Execute("PRAGMA temp_store = memory;");
                //var TempPath = Windows.Storage.ApplicationData.Current.TemporaryFolder.Path;
                //_nativeConnaction.Execute(String.Format("PRAGMA temp_store_directory = '{0}';", TempPath));
                _State = ConnectionState.Open;
            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }
#elif WINDOWS_UWP
            string sqliteFilePath = SQLiteFilePath.Trim();
            //if(sqliteFilePath.Length>0&& sqliteFilePath[0]=='\\')
            //    sqliteFilePath = sqliteFilePath.Substring(1);

            if (fileSystem.FileExists(sqliteFilePath))
            {
                _nativeConnaction = new SQLite.SQLiteConnection(fileSystem.GetDeviceSpecificPath(sqliteFilePath));
                _State = ConnectionState.Open;
            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }
#else
            if (fileSystem.FileExists(fileSystem.GetDeviceSpecificPath(SQLiteFilePath)))
            {
                _nativeConnaction = new SQLite.SQLiteConnection(fileSystem.GetDeviceSpecificPath(SQLiteFilePath));
                _State = ConnectionState.Open;
            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }
#endif
#else
            if (System.IO.File.Exists(SQLiteFilePath))
            {
                _nativeConnaction = new SQLite.SQLiteConnection(SQLiteFilePath);
                _State = ConnectionState.Open;

            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }


#endif
        }

        #region IDataBaseConnection Members

        System.Collections.Generic.Dictionary<string, bool> ActiveTransactions = new Dictionary<string, bool>();

        public void BeginTransaction( PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (State == ConnectionState.Closed)
                Open();
            var command = CreateCommand();
            bool active = false;
            if (!ActiveTransactions.TryGetValue(theTransaction.Transaction.LocalTransactionUri, out active))
            {
                active = true;
                ActiveTransactions[theTransaction.Transaction.LocalTransactionUri] = active;

                command.CommandText = "PRAGMA foreign_keys;";
                var res = command.ExecuteScalar();

                command.CommandText = "PRAGMA foreign_keys = OFF;";
                command.ExecuteNonQuery();

                command.CommandText = "PRAGMA foreign_keys;";
                res = command.ExecuteScalar();

                command.CommandText = "BEGIN TRANSACTION;";
                command.ExecuteNonQuery();
                
            }
            else
            {
                if (!active)
                {
                    active = true;
                    ActiveTransactions[theTransaction.Transaction.LocalTransactionUri] = active;
                    command.CommandText = "BEGIN TRANSACTION;";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AbordTransaction(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            bool active = false;
            if(!ActiveTransactions.TryGetValue(theTransaction.Transaction.LocalTransactionUri, out active))
                active =false;
            if (active)
            {
                ActiveTransactions.Remove(theTransaction.Transaction.LocalTransactionUri);
                var command = CreateCommand();
                command.CommandText = "ROLLBACK;";
                command.ExecuteNonQuery();

                ActiveTransactions[theTransaction.Transaction.LocalTransactionUri] = false;
            }

        }

        public void CommitTransaction(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            bool active = false;
            if (!ActiveTransactions.TryGetValue(theTransaction.Transaction.LocalTransactionUri, out active))
                active = false;
            if (active)
            {

                var command = CreateCommand();
                command.CommandText = "COMMIT;";
                command.ExecuteNonQuery();
                ActiveTransactions[theTransaction.Transaction.LocalTransactionUri] = false;
            }

        }

        public void CreateDataBase()
        {

#if DeviceDotNet
         
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;

#if WINDOWS_PHONE && SILVERLIGHT
            IntPtr Db = IntPtr.Zero;
            using (SQLite.SQLiteConnection SQLiteConnection = new SQLite.SQLiteConnection(SQLiteFilePath))
            {

            }
#else

            

            using (SQLite.SQLiteConnection SQLiteConnection = new SQLite.SQLiteConnection(fileSystem.GetDeviceSpecificPath(SQLiteFilePath)))
            {

            }
#endif
#else
            
            using (SQLite.SQLiteConnection SQLiteConnection = new SQLite.SQLiteConnection(SQLiteFilePath))
            {

            }
#endif
        }

        #endregion
    }

    /// <MetaDataID>{912685b4-1a99-459a-8bcf-965276625cbd}</MetaDataID>
    public class SQLiteCommand : IDataBaseCommand
    {
        private SQLiteDataBaseConnection SQLiteDataBaseConnection;
        SQLite.SQLiteCommand _nativeCommand;
        public SQLiteCommand(SQLiteDataBaseConnection sqliteDataBaseConnection)
        {
              
            // TODO: Complete member initialization
            SQLiteDataBaseConnection = sqliteDataBaseConnection;
            _nativeCommand = new SQLite.SQLiteCommand(SQLiteDataBaseConnection._nativeConnaction);
        }



        #region IDataBaseCommand Members
        string _CommandText;
        public string CommandText
        {
            get
            {
                return _nativeCommand.CommandText;
            }
            set
            {
                _nativeCommand.CommandText= value;
            }
        }

        public IDataBaseParameter CreateParameter()
        {

            return new SQLiteParameter(this);
        }
        IDataBaseParameterCollection _Parameters=new SQLiteParameterCollection();
        public IDataBaseParameterCollection Parameters
        {
            get { return _Parameters; }
        }

        public int ExecuteNonQuery()
        {
            


            foreach (IDataBaseParameter parameter in _Parameters)
                _nativeCommand.Bind(parameter.ParameterName, parameter.Value);
            
            return _nativeCommand.ExecuteNonQuery();
            //int res = SQLiteBase.sqlite3_exec(SQLiteDataBaseConnection.SqliteDB, SQLiteBase.StringToPointer(_CommandText), IntPtr.Zero, IntPtr.Zero, out error);
            //return 0;
        }

        public bool DesignTimeVisible
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public MetaDataRepository.ObjectQueryLanguage.IDataReader ExecuteReader()
        {
          return  new SQLiteDatareader(_nativeCommand.ExecuteDataReader(),_nativeCommand);
        }

        public object ExecuteScalar()
        {
            return _nativeCommand.ExecuteScalar<object>();
            
        }

        #endregion
    }

    /// <MetaDataID>{e94cb981-a48d-4ee0-a731-edfd4b45dace}</MetaDataID>
    public class SQLiteParameter : IDataBaseParameter
    {
        private SQLiteCommand SQLiteCommand;

        public SQLiteParameter(SQLiteCommand sqliteCommand)
        {
            // TODO: Complete member initialization
            this.SQLiteCommand = sqliteCommand;
        }


        #region IDataBaseParameter Members

        string _ParameterName;

        public string ParameterName
        {
            get
            {
                return _ParameterName; 
            }
            set
            {
                _ParameterName = value;
            }
        }
        DbType _DbType;
        public DbType DbType
        {
            get
            {
                return _DbType; ;
            }
            set
            {
                _DbType = value;
            }
        }

        object _Value;
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        ParameterDirection _Direction;
        public ParameterDirection Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
            }
        }
        int _Size;
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                
                _Size = value;
            }
        }


        #endregion
    }

    /// <MetaDataID>{10f013cc-886f-4157-b136-b97a7e849946}</MetaDataID>
    public class SQLiteParameterCollection : IDataBaseParameterCollection
    {

        internal List<IDataBaseParameter> parameters = new List<IDataBaseParameter>();

        #region IDataBaseParameterCollection Members

        public void Add(IDataBaseParameter parameter)
        {
            parameters.Add(parameter);
        }

        public IDataBaseParameter this[string parameterName]
        {
            get
            {
               return (from parameter in parameters
                 where parameter.ParameterName == parameterName
                 select parameter).FirstOrDefault();
                
                
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IDataBaseParameterCollection Members


        public int Count
        {
            get { return parameters.Count; }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        #endregion
    }

    /// <MetaDataID>{0fdce2f3-37ec-4eea-a92c-50c6b57ef5ec}</MetaDataID>
    public class SQLiteDatareader : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader
    {
        Sqlite3Statement Sqlite3Statement;
        Dictionary<string, int> ColumnsIndices;
        private   SQLite.SQLiteCommand NativeCommand;
        public SQLiteDatareader(Sqlite3Statement stmt, SQLite.SQLiteCommand _nativeCommand)
        {
            Sqlite3Statement = stmt;
            NativeCommand = _nativeCommand;
            ColumnsIndices=NativeCommand.GetColumnsIndices(Sqlite3Statement);
        }
        #region IDataReader Members
        object[] Values;
        public bool Read()
        {
            bool ret= NativeCommand.Read(Sqlite3Statement);
            if (ret)
                Values = NativeCommand.GetValues(Sqlite3Statement);
            return ret;
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < Values.Length; i++)
             values[i]= Values[i];

            return Values.Length;
        }

        public int GetOrdinal(string name)
        {
            return ColumnsIndices[name];
        }

        public string GetName(int i)
        {
            foreach (var entry in ColumnsIndices)
                if (entry.Value == i)
                    return entry.Key;

            return null;
        }

        public int FieldCount
        {
            get { return ColumnsIndices.Count; }
        }

        public Type GetFieldType(int i)
        {  
            Type colType = NativeCommand.ColumnType(Sqlite3Statement, i);

            return colType;
        }

        public object this[int ordinal]
        {
            get { return Values[ordinal]; }
        }

        public object this[string name]
        {
            get { return Values[ColumnsIndices[ name]]; }
        }

        public void Close()
        {
            NativeCommand.Close(Sqlite3Statement);
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
