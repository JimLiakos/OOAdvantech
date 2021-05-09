using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;


namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <summary>
	/// AppDataUpdateCache holds generic DataTable and performs SQL Bulk insert 
	/// of same at the specified number of rows interval.
	/// </summary>
	public sealed class CacheEngine
	{
		#region ctor	
		public CacheEngine(DataTable dt, string selectStoredProc,string insertStoredProc,string deleteStoredProc, string updateStoredProc, string connectionString, int numRowsPerUpdate)
		{
			this._cacheTable =dt;
			this._connectionString=connectionString;
			this._selectStoredProc =selectStoredProc;
			this._insertStoredProc =insertStoredProc;
			this._deleteStoredProc=deleteStoredProc;
			this._updateStoredProc =updateStoredProc;
			this._numRowsPerUpdate =numRowsPerUpdate;
			Init();
		}
		#endregion

		#region public fields / properties
		private DataTable _cacheTable;
		private DataSet ds= new DataSet();
		public DataTable CacheTable
		{
			get
			{
				return _cacheTable;
			}
			set
			{
				_cacheTable=value;
			}
		}

		private string _connectionString;
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
			set
			{
				_connectionString=value;
			}
		}
		private string _selectStoredProc;
		public string SelectStoredProc
		{
			get
			{
				return _selectStoredProc;
			}
			set
			{
				_selectStoredProc=value;
			}
		}
		private string _deleteStoredProc;
		public string DeleteStoredProc
		{
			get
			{
				return _deleteStoredProc;
			}
			set
			{
				_deleteStoredProc=value;
			}
		}

		private string _updateStoredProc;
		public string UpdateStoredProc
		{
			get
			{
				return _updateStoredProc;
			}
			set
			{
				_updateStoredProc=value;
			}
		}
		private string _insertStoredProc;
		public string InsertStoredProc
		{
			get
			{
				return _insertStoredProc;
			}
			set
			{
				_insertStoredProc=value;
			}
		}

		private int _numRowsPerUpdate=250;
		public int NumRowsPerUpdate
		{
			get
			{
				return _numRowsPerUpdate;
			}
			set
			{
				_numRowsPerUpdate=value;
			}
		}

		public void AddItem(object[] objItemArray)
		{
			try
			{
				if(this._connectionString==null || this._connectionString =="")
					throw new InvalidOperationException("All properties must be set to add an item.");
				DataRow row=this._cacheTable.NewRow();
				row.ItemArray=objItemArray;
				this._cacheTable.Rows.Add(row);		 
			}
			catch(System.Exception Err)
			{
				int dfd=0;
			}
		}
		#endregion

		#region private members
		private void Init()
		{
	
			DataSet newDs = new DataSet();
			this.ds=newDs;		
			//this._cacheTable.TableName ="AppData";
			this.ds.Tables.Add(_cacheTable);
			this._cacheTable.RowChanged += new DataRowChangeEventHandler( Row_Changed );
		}


		private  void Row_Changed( object sender, DataRowChangeEventArgs e )
		{	
				if( ((DataTable)sender).Rows.Count >this._numRowsPerUpdate )
				{					
					BulkInsertData();					 
				}
		}

		internal void BulkInsertData()
		{
		this._cacheTable.RowChanged -= new DataRowChangeEventHandler( Row_Changed );		
			try
			{
				DataSet ds2 = new DataSet();
				DataTable tbl;
				lock(_cacheTable)
				{
					  tbl = this._cacheTable.Copy();
				}
                tbl.TableName =_cacheTable.TableName;
				ds2.Tables.Add(tbl);
				DaHandler.SubmitChanges(ref ds2,this._connectionString,
					     this._selectStoredProc ,this._updateStoredProc,
					       this._insertStoredProc,this._deleteStoredProc);				
			}
			catch(Exception Ex)
			{				
            System.Diagnostics.Debug.Write(Ex.Message+Ex.StackTrace ) ;
			}
			finally
			{
				_cacheTable.Clear() ;
				this._cacheTable.RowChanged += new DataRowChangeEventHandler( Row_Changed );
			}			 
		}
		#endregion
	}
}