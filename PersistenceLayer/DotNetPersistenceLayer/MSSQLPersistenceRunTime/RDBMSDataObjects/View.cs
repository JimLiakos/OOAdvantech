namespace OOAdvantech.RDBMSDataObjects
{
	/// <MetaDataID>{D12ADE49-1C9A-4036-BAC1-5FF135CB32DA}</MetaDataID>
	internal class SortViews: System.Collections.Generic.IComparer<View>	
    {
		/// <MetaDataID>{3C83D796-7A1C-4ABD-9109-D35279F9C871}</MetaDataID>
        public int Compare(View x, View y)
        {
            if (!(x is View || y is View)) throw new System.ArgumentException("The objects to compare must be of type 'ColumnAlias'");
            return ((View)x).CreationOrder - ((View)y).CreationOrder;
        }

    }
	/// <MetaDataID>{B4EEB3C6-9C12-4CD7-A139-5CBF5C57B18A}</MetaDataID>
	public class View : MetaDataRepository.Namespace
	{
		public View()
		{
		}
		/// <MetaDataID>{E27AD1B4-5063-4D06-9476-2B1B02B5AA99}</MetaDataID>
		public int CreationOrder=0;
		/// <MetaDataID>{18E3C1C4-E80D-49F7-9C09-8BE355E73CF6}</MetaDataID>
		public Table Tables;
		/// <MetaDataID>{095F3A3E-10C0-486C-9246-D48C33AB07AA}</MetaDataID>
		public View SubViews;
		/// <MetaDataID>{B3D4014F-2CC9-4886-9052-BF6D50A58FDE}</MetaDataID>
		//private string ConnectionString=null;
		/// <MetaDataID>{C8A9CA16-90F3-4DCC-B541-D2034D7C36D6}</MetaDataID>
		private bool NewView=true;
		/// <MetaDataID>{40FB33EC-F197-4D3B-BF13-9AD6F2E02BBD}</MetaDataID>
		public View(string name, bool newView)
		{
			_Name=name;
			NewView=newView;
			//ConnectionString=connectionString;
		}
		/// <MetaDataID>{FD820E3F-717D-416F-B58F-90B6F9F6AF94}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				return base.Identity;
				if(_Identity==null)
					_Identity=new MetaDataRepository.MetaObjectID(Name);
				return _Identity;
			}
		}
		string DropOldView;
		/// <MetaDataID>{5C52FD88-010B-4946-A513-36AAD8F2E76D}</MetaDataID>
		private string ViewDefinition;
		/// <MetaDataID>{F973621F-2DC6-41B6-8C45-25AF7C3DC295}</MetaDataID>
		public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
		{
			RDBMSMetaDataRepository.View OriginView=(RDBMSMetaDataRepository.View)OriginMetaObject;
			CreationOrder=OriginView.CreationOrder;
			if(!NewView&&OriginView.Name!=Name&&OriginView.DataBaseViewName==Name)
			{
				DropOldView="drop view ["+Name+"] ";
				_Name=OriginView.Name;
				NewView=true;
			}
            DataBase dataBase = _Namespace as DataBase;
            ViewDefinition = dataBase.RDBMSSchema.GetDefinition(OriginView, NewView);
            OriginView.DataBaseViewName = Name;
			
		}
		/// <MetaDataID>{F5A1FB51-1E7D-4A55-981B-1BD4567C4D92}</MetaDataID>
		public void Update()
		{
			DataBase dataBase =_Namespace as DataBase;
			System.Data.Common.DbConnection connection=dataBase.Connection;

            if (string.IsNullOrEmpty(ViewDefinition ))
				return;
			try
			{
                System.Data.Common.DbCommand command = connection.CreateCommand();
                command.CommandText = ViewDefinition;
				command.ExecuteNonQuery();
				if(!string.IsNullOrEmpty(DropOldView))
				{
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(DropOldView, Connection);
                    command.CommandText = DropOldView;
					command.ExecuteNonQuery();
				}
				NewView=false;
			}
			catch(System.Exception Error)
			{
				throw new System.Exception("It can't save the View '"+Name+"' changes.",Error);
			}


		}




	}
}
