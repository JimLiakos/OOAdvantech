namespace OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects
{
	/// <MetaDataID>{44852FD6-2945-4CEB-84A6-8670BB0B591D}</MetaDataID>
	public class UUIDStoreprocedureBuilder : OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects.StoreprocedureCodeBuilder
	{
		/// <MetaDataID>{A77E3C0C-E479-4088-8C4C-48A99AAB0A40}</MetaDataID>
		public override string BuildStoreProcedureBodyCode(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure storeProcedure,bool create)
		{
			switch(storeProcedure.Type)
			{
				case RDBMSMetaDataRepository.StoreProcedure.Types.New:
					return BuildNewInstanceStoreProcedure(storeProcedure,create);
				case RDBMSMetaDataRepository.StoreProcedure.Types.Update:
					return BuildUpdateInstanceStoreProcedure(storeProcedure,create);
				case RDBMSMetaDataRepository.StoreProcedure.Types.Delete:
					return BuildDeleteInstanceStoreProcedure(storeProcedure,create);
			}
			return "";

		}
		/// <MetaDataID>{64B0C3E9-3D44-4C63-9C4D-74FE5C0CC2A8}</MetaDataID>
		private string BuildDeleteInstanceStoreProcedure(RDBMSMetaDataRepository.StoreProcedure storeProcedure,bool create)
		{
			/*####################### Create StoreProcedure parameters ##################*/
			string DeleteStoreProcedure=null;
			if(create)
				DeleteStoreProcedure="Create ";
			else
				DeleteStoreProcedure="Alter ";

			DeleteStoreProcedure+="Procedure dbo."+storeProcedure.Name+"\n";
			string PrameterList=null;//UpdateStoreProcedure+="@IntObjID int,\n@ObjCellID int";

			foreach(MetaDataRepository.Parameter CurrParameter in storeProcedure.Parameters)
			{
				if(PrameterList!=null)
					PrameterList+=",\n";
                string parameterType = CurrParameter.Type.FullName;
                if (CurrParameter.Type is MetaDataRepository.Enumeration)
                    parameterType = "enum";

				PrameterList+="@"+CurrParameter.Name+" "+TypeDictionary.GetDBType(parameterType);
				if(TypeDictionary.IsTypeVarLength(parameterType))
				{
					object mLength=CurrParameter.GetPropertyValue(typeof(int),"Persistent","SizeOf");
					if(mLength!=null)
					{
						if(((int)mLength)==0)
							PrameterList+="("+TypeDictionary.GeDefaultLength(parameterType).ToString()+")";
						else
							PrameterList+="("+mLength.ToString()+")";
					}
				}
			}
			DeleteStoreProcedure+=PrameterList;
			DeleteStoreProcedure+="\nAS\n";
			/*####################### Create StoreProcedure parameters ##################*/

			

			/*####################### Update main table columns ##################*/
			DeleteStoreProcedure+="DELETE FROM "+storeProcedure.ActsOnStorageCell.MainTable.Name;
			DeleteStoreProcedure+="\nWHERE(";
			int Count=0;
			foreach(RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
			{
				if(Count!=0)
					DeleteStoreProcedure+=" AND ";
				DeleteStoreProcedure+=Column.Name+" = @"+Column.Name;
				Count++;
			}
			DeleteStoreProcedure+=")";
			
			/*####################### Update main table columns ##################*/


			/*####################### Update mapped tables columns ##################*/
			foreach(RDBMSMetaDataRepository.Table CurrTable in storeProcedure.ActsOnStorageCell.MappedTables)
			{
				if(CurrTable==storeProcedure.ActsOnStorageCell.MainTable)
					continue;
				DeleteStoreProcedure+="\nDELETE FROM "+CurrTable.Name;
				DeleteStoreProcedure+="\nWHERE(";
				Count=0;
				foreach(RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
				{
					if(Count!=0)
						DeleteStoreProcedure+=" AND ";
					DeleteStoreProcedure+=Column.Name+" = @"+Column.Name;
					Count++;
				}
				DeleteStoreProcedure+=")";
			}
			/*####################### Update mapped tables columns ##################*/
			return DeleteStoreProcedure;

		}
		/// <MetaDataID>{E8143EE6-5ACE-4F37-82C6-9D6D965E4334}</MetaDataID>
		private string BuildUpdateInstanceStoreProcedure(RDBMSMetaDataRepository.StoreProcedure storeProcedure,bool create)
		{
			
			/*####################### Create StoreProcedure parameters ##################*/
			
			string UpdateStoreProcedure=null;
			if(create)
				UpdateStoreProcedure="Create ";
			else
				UpdateStoreProcedure="Alter ";

			UpdateStoreProcedure+="Procedure dbo."+storeProcedure.Name+"\n";

			
			string PrameterList=null;//UpdateStoreProcedure+="@IntObjID int,\n@ObjCellID int";
			System.Collections.Specialized.HybridDictionary ClassAttriutes=new System.Collections.Specialized.HybridDictionary();

			foreach(MetaDataRepository.Parameter CurrParameter in storeProcedure.Parameters)
			{
                string parameterType = CurrParameter.Type.FullName;
                if (CurrParameter.Type is MetaDataRepository.Enumeration)
                    parameterType = "enum";

				ClassAttriutes.Add(CurrParameter.Name,CurrParameter);
				if(PrameterList!=null)
					PrameterList+=",\n";
				PrameterList+="@"+CurrParameter.Name+" "+TypeDictionary.GetDBType(parameterType);
				if(TypeDictionary.IsTypeVarLength(parameterType))
				{
					object mLength=CurrParameter.GetPropertyValue(typeof(int),"Persistent","SizeOf");
					if(mLength!=null)
					{
						if(((int)mLength)==0)
							PrameterList+="("+TypeDictionary.GeDefaultLength(parameterType).ToString()+")";
						else
							PrameterList+="("+mLength.ToString()+")";
					}
				}
			}
			UpdateStoreProcedure+=PrameterList;
			UpdateStoreProcedure+="\nAS\n";
			
			/*####################### Create StoreProcedure parameters ##################*/

			/*####################### Update main table columns ##################*/
			UpdateStoreProcedure+="UPDATE   "+storeProcedure.ActsOnStorageCell.MainTable.Name+"\nSet ";
			bool FirstColumn=true;
			foreach(RDBMSMetaDataRepository.Column CurrColumn in storeProcedure.ActsOnStorageCell.MainTable.ContainedColumns)
			{
				if(CurrColumn.MappedAttribute!=null)
				{
                    //string parameterName = CurrColumn.MappedAttribute.CaseInsensitiveName;
					string parameterName=CurrColumn.Name;


//					if(CurrColumn.Name.Trim().ToLower()!=CurrColumn.MappedAttribute.CaseInsensitiveName.Trim().ToLower())
//						parameterName+=CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;
//					else
//						parameterName=CurrColumn.Name;


					if(ClassAttriutes.Contains(parameterName))
					{
						if(FirstColumn)
						{
							UpdateStoreProcedure+=CurrColumn.Name+" = "+"@";
							FirstColumn=false;
						}
						else
							UpdateStoreProcedure+=","+CurrColumn.Name+" = "+"@";
						UpdateStoreProcedure+=parameterName;
					}
				}
			}




			if(storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn!=null)
			{
				if(ClassAttriutes.Contains(storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name))
				{
					if(FirstColumn)
					{
						UpdateStoreProcedure+=storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name+" = "+"@"+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
						FirstColumn=false;
					}
					else
						UpdateStoreProcedure+=","+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name+" = "+"@"+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
				}
			}

			UpdateStoreProcedure+="\nWHERE(";
			int Count=0;
			foreach(RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
			{
				if(Count!=0)
					UpdateStoreProcedure+=" AND ";
				UpdateStoreProcedure+=Column.Name+" = @"+Column.Name;
				Count++;
			}
			UpdateStoreProcedure+=")";

			
			/*####################### Update main table columns ##################*/


			/*####################### Update mapped tables columns ##################*/
			foreach(RDBMSMetaDataRepository.Table CurrTable in storeProcedure.ActsOnStorageCell.MappedTables)
			{
				if(CurrTable==storeProcedure.ActsOnStorageCell.MainTable)
					continue;
				UpdateStoreProcedure+="\nUPDATE   "+CurrTable.Name+"\nSet ";
				FirstColumn=true;
				foreach(RDBMSMetaDataRepository.Column CurrColumn in CurrTable.ContainedColumns)
				{
					if(CurrColumn.MappedAttribute!=null)
					{
						if(ClassAttriutes.Contains(CurrColumn.MappedAttribute.Name))
						{
							if(FirstColumn)
							{
								UpdateStoreProcedure+=CurrColumn.Name+" = "+"@";
								FirstColumn=false;
							}
							else
								UpdateStoreProcedure+=","+CurrColumn.Name+" = "+"@";
						}
                        //UpdateStoreProcedure += CurrColumn.MappedAttribute.CaseInsensitiveName;
						UpdateStoreProcedure+=CurrColumn.Name;
//						if(CurrColumn.Name.Trim().ToLower()==CurrColumn.MappedAttribute.Name.Trim().ToLower())
//							UpdateStoreProcedure+=CurrColumn.Name;
//						else
//							UpdateStoreProcedure+=CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;

					}
				}
				UpdateStoreProcedure+="\nWHERE(";
				Count=0;
				foreach(RDBMSMetaDataRepository.Column Column in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(storeProcedure))
				{
					if(Count!=0)
						UpdateStoreProcedure+=" AND ";
					UpdateStoreProcedure+=Column.Name+" = @"+Column.Name;
					Count++;
				}
				UpdateStoreProcedure+=")";

			}
			return UpdateStoreProcedure;
			/*####################### Update mapped tables columns ##################*/

		}
		/// <MetaDataID>{18649047-88FC-4C05-90E0-FE914A804616}</MetaDataID>
		private string BuildNewInstanceStoreProcedure(RDBMSMetaDataRepository.StoreProcedure storeProcedure,bool create)
		{

			/*####################### Create StoreProcedure parameters ##################*/
			
			string NewStoreProcedureDef=null;
			if(create)
				NewStoreProcedureDef="Create ";
			else
				NewStoreProcedureDef="Alter ";

			NewStoreProcedureDef+="Procedure dbo."+storeProcedure.Name+"\n";
			string PrameterList=null;//UpdateStoreProcedure+="@IntObjID int,\n@ObjCellID int";
			System.Collections.Specialized.HybridDictionary ClassAttriutes=new System.Collections.Specialized.HybridDictionary();

			foreach(MetaDataRepository.Parameter CurrParameter in storeProcedure.Parameters)
			{
				ClassAttriutes.Add(CurrParameter.Name,CurrParameter);
				if(PrameterList!=null)
					PrameterList+=",\n";
                string parameterType = CurrParameter.Type.FullName;
                if (CurrParameter.Type is MetaDataRepository.Enumeration)
                    parameterType = "enum";

				PrameterList+="@"+CurrParameter.Name+" "+TypeDictionary.GetDBType(parameterType);
				if(TypeDictionary.IsTypeVarLength(parameterType))
				{
					object mLength=CurrParameter.GetPropertyValue(typeof(int),"Persistent","SizeOf");
					if(mLength!=null)
					{
						if(((int)mLength)==0)
							PrameterList+="("+TypeDictionary.GeDefaultLength(parameterType).ToString()+")";
						else
							PrameterList+="("+mLength.ToString()+")";
					}
				}
				if(CurrParameter.Direction==MetaDataRepository.Parameter.DirectionType.Out)
					PrameterList+=" output";

			}
			NewStoreProcedureDef+=PrameterList;



			NewStoreProcedureDef+="\nAS\n";

			NewStoreProcedureDef+="DECLARE @TypeID int\n";
			NewStoreProcedureDef+="set @TypeID="+(storeProcedure.ActsOnStorageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString()+"\n";

			


			/*####################### Insert main table columns ##################*/
			string InsertClause="INSERT INTO "+storeProcedure.ActsOnStorageCell.MainTable.Name+"(";//ObjCellID, IntObjID, TypeID";
			string ValueClause="VALUES(";//ObjCellID, @TableID, @TypeID";
			int count=0;
			foreach(RDBMSMetaDataRepository.Column column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
			{
				if(count!=0)
				{
					InsertClause+=",";
					ValueClause+=",";
				}
				count++;
				InsertClause+=column.Name;
				ValueClause+="@"+column.Name;
			}
			foreach(string columnName in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumnsName())
			{
				if(count!=0)
				{
					InsertClause+=",";
					ValueClause+=",";
				}
				count++;
				InsertClause+=columnName;
				ValueClause+="@"+columnName;
			}
			if(storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn!=null)
			{
				InsertClause+=","+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
				ValueClause+=",@"+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
			}
			foreach(RDBMSMetaDataRepository.Column CurrColumn in storeProcedure.ActsOnStorageCell.MainTable.ContainedColumns)
			{
				if(CurrColumn.MappedAttribute!=null)
				{
					//string parameterName=CurrColumn.MappedAttribute.CaseInsensitiveName;
                    string parameterName = CurrColumn.Name;

//					if(CurrColumn.Name.Trim().ToLower()!=CurrColumn.MappedAttribute.CaseInsensitiveName.Trim().ToLower())
//						parameterName=CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;
//					else
//						parameterName=CurrColumn.Name;

					if(ClassAttriutes.Contains(parameterName))
					{
						InsertClause+=","+CurrColumn.Name;
						ValueClause+=",@"+parameterName;
					}
				}
			}
			InsertClause+=")\n";
			ValueClause+=")\n";
			NewStoreProcedureDef+=InsertClause+ValueClause;
			/*####################### Insert main table columns ##################*/

			InsertClause=null;
			ValueClause=null;

			/*####################### Insert mapped tables columns ##################*/
			foreach(RDBMSMetaDataRepository.Table CurrTable in storeProcedure.ActsOnStorageCell.MappedTables)
			{
				if(CurrTable==storeProcedure.ActsOnStorageCell.MainTable)
					continue;
				InsertClause+="INSERT INTO "+CurrTable .Name+"(";//ObjCellID, IntObjID, TypeID";
				ValueClause+="VALUES(";//@ObjCellID, @TableID, @TypeID";

				foreach(RDBMSMetaDataRepository.Column column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
				{
					if(count!=0)
					{
						InsertClause+=",";
						ValueClause+=",";
					}
					count++;
					InsertClause+=column.Name;
					ValueClause+="@"+column.Name;
				}
				foreach(string columnName in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumnsName())
				{
					if(count!=0)
					{
						InsertClause+=",";
						ValueClause+=",";
					}
					count++;
					InsertClause+=columnName;
					ValueClause+="@"+columnName;
				}
				foreach(RDBMSMetaDataRepository.Column CurrColumn in CurrTable.ContainedColumns)
				{
					if(CurrColumn.MappedAttribute!=null)
					{
						InsertClause+=","+CurrColumn.Name;
						//ValueClause+=",@"+CurrColumn.MappedAttribute.CaseInsensitiveName;
                        ValueClause += ",@" + CurrColumn.Name;
//						if(CurrColumn.Name.Trim().ToLower()==CurrColumn.MappedAttribute.Name.Trim().ToLower())
//							ValueClause+=",@"+CurrColumn.Name;
//						else
//							ValueClause+=",@"+CurrColumn.MappedAttribute.Owner.Name+"_"+CurrColumn.MappedAttribute.CaseInsensitiveName;
					}
				}
				InsertClause+=")\n";
				ValueClause+=")\n";
				NewStoreProcedureDef+=InsertClause+ValueClause;
			}
			/*####################### Insert mapped tables columns ##################*/
			return NewStoreProcedureDef;
		}



	}
}
