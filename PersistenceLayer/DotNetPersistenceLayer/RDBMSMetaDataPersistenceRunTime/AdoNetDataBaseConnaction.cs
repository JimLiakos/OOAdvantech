using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{
    /// <MetaDataID>{0b85874e-d21b-44df-9cb2-b2a949e8f5a2}</MetaDataID>
    public class DataBaseConnection : IDataBaseConnection
    {
        private System.Data.Common.DbConnection DbConnection;

        public DataBaseConnection(System.Data.Common.DbConnection dbConnection)
        {
            // TODO: Complete member initialization
            DbConnection = dbConnection;
        }


        public void Close()
        {
            DbConnection.Close();
        }

        public ConnectionState State
        {
            get
            {
                return (ConnectionState)(int)DbConnection.State;
            }

        }

        public void Open()
        {
            DbConnection.Open();
        }

        public IDataBaseCommand CreateCommand()
        {
            return new DataBaseCommand(DbConnection.CreateCommand());
        }

        public void EnlistTransaction(System.Transactions.Transaction transaction)
        {
            DbConnection.EnlistTransaction(transaction);
        }

        public object NativeConnaction
        {
            get { return DbConnection; }
        }
        public string ConnectionString
        {
            get
            {
                return DbConnection.ConnectionString;
            }
            set
            {
                DbConnection.ConnectionString = value;
            }
        }




        public void BeginTransaction(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
          
        }

        public void AbordTransaction(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            
        }

        public void CommitTransaction(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            
        }



        #region IDataBaseConnection Members

        public void CreateDataBase()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    /// <MetaDataID>{f20f13d1-a300-4032-80f1-a0b1fdca137c}</MetaDataID>
    public class DataBaseCommand : IDataBaseCommand
    {
        private System.Data.Common.DbCommand DbCommand;

        public DataBaseCommand(System.Data.Common.DbCommand dbCommand)
        {
            // TODO: Complete member initialization
            this.DbCommand = dbCommand;
        }
        public string CommandText
        {
            get
            {
                return DbCommand.CommandText;
            }
            set
            {
                DbCommand.CommandText = value;
            }
        }

        public IDataBaseParameter CreateParameter()
        {
            DataBaseParameter dataBaseParameter = new DataBaseParameter(DbCommand.CreateParameter());
            return dataBaseParameter;
        }

        DataBaseParameterCollection DataBaseParameterCollection = null;
        public IDataBaseParameterCollection Parameters
        {
            get
            {
                if (DataBaseParameterCollection == null)
                    DataBaseParameterCollection = new DataBaseParameterCollection(DbCommand.Parameters);
                return DataBaseParameterCollection;
            }
        }

        public int ExecuteNonQuery()
        {
            return DbCommand.ExecuteNonQuery();
        }

        public bool DesignTimeVisible
        {
            get
            {
                return DbCommand.DesignTimeVisible;
            }
            set
            {
                DbCommand.DesignTimeVisible = value;
            }
        }

        public MetaDataRepository.ObjectQueryLanguage.IDataReader ExecuteReader()
        {
            return new MetaDataRepository.ObjectQueryLanguage.DataReader(DbCommand.ExecuteReader());
        }



        public object ExecuteScalar()
        {
            return DbCommand.ExecuteScalar();
        }


    }
    /// <MetaDataID>{7ba8aa45-ce5d-4ea5-94df-c660ddafffe3}</MetaDataID>
    public class DataBaseParameter : IDataBaseParameter
    {
        internal System.Data.Common.DbParameter DpParameter;

        public DataBaseParameter(System.Data.Common.DbParameter dbParameter)
        {
            // TODO: Complete member initialization
            DpParameter = dbParameter;
        }




        public string ParameterName
        {
            get
            {
                return DpParameter.ParameterName;
            }
            set
            {
                DpParameter.ParameterName = value;
            }
        }

        public DbType DbType
        {
            get
            {
                return (DbType)(int)DpParameter.DbType;
            }
            set
            {
                DpParameter.DbType = (System.Data.DbType)(int)value;
            }
        }

        public object Value
        {
            get
            {
                return DpParameter.Value;
            }
            set
            {
                DpParameter.Value = value;
            }
        }

        public ParameterDirection Direction
        {
            get
            {
                return (ParameterDirection)(int)DpParameter.Direction;
            }
            set
            {
                DpParameter.Direction = (System.Data.ParameterDirection)(int)value;

            }
        }

        public int Size
        {
            get
            {
                return DpParameter.Size;
            }
            set
            {
                DpParameter.Size = value;
            }
        }
    }
    /// <MetaDataID>{636ed23b-4ad4-44af-9a50-583f6ce9849c}</MetaDataID>
    public class DataBaseParameterCollection : IDataBaseParameterCollection
    {
        internal System.Data.Common.DbParameterCollection DbParameterCollection;

        Dictionary<System.Data.Common.DbParameter, DataBaseParameter> NativeDbParameterDictionary = new Dictionary<System.Data.Common.DbParameter, DataBaseParameter>();

        public DataBaseParameterCollection(System.Data.Common.DbParameterCollection dbParameterCollection)
        {
            // TODO: Complete member initialization
            DbParameterCollection = dbParameterCollection;
        }


        public void Add(IDataBaseParameter parameter)
        {
            DbParameterCollection.Add((parameter as DataBaseParameter).DpParameter);

            NativeDbParameterDictionary[(parameter as DataBaseParameter).DpParameter] = parameter as DataBaseParameter;
        }

        public IDataBaseParameter this[string parameterName]
        {
            get
            {
                System.Data.Common.DbParameter dbParameter = DbParameterCollection[parameterName];
                DataBaseParameter dataBaseParameter = null;
                if (!NativeDbParameterDictionary.TryGetValue(dbParameter, out dataBaseParameter))
                    dataBaseParameter = new DataBaseParameter(DbParameterCollection[parameterName]);
                return dataBaseParameter;
            }
            set
            {
                NativeDbParameterDictionary[(value as DataBaseParameter).DpParameter] = value as DataBaseParameter;
                DbParameterCollection[parameterName] = (value as DataBaseParameter).DpParameter;
            }
        }



        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return (from dpParameter in DbParameterCollection.OfType<System.Data.Common.DbParameter>()
                    select NativeDbParameterDictionary[dpParameter]).ToList().GetEnumerator();
        }

        #endregion
    }

}
