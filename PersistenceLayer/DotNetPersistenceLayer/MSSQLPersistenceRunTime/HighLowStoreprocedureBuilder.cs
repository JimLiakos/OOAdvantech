namespace OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects
{
	/// <MetaDataID>{B42A9E53-EEFA-4A5D-A099-57C4837F2E53}</MetaDataID>
	public class HighLowStoreprocedureBuilder : OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects.StoreprocedureCodeBuilder
	{
		/// <MetaDataID>{3C4AE61C-3B8B-4207-A93F-CA48F60CBD8F}</MetaDataID>
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


		/// <MetaDataID>{38E3D2F3-01ED-4531-B482-ED8758586BF7}</MetaDataID>
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
				if(CurrParameter.Direction==MetaDataRepository.Parameter.DirectionType.Out)
					PrameterList+=" output";

			}
			NewStoreProcedureDef+=PrameterList;



			NewStoreProcedureDef+="\nAS\n";
			
			/*####################### Create StoreProcedure parameters ##################*/

			/*####################### Find next table ID ##################*/
			NewStoreProcedureDef+="DECLARE @TableID int\nDECLARE @FirstRecord int\nDECLARE @TypeID int\nDECLARE @RecordCount int\n";
			NewStoreProcedureDef+="set @FirstRecord=0\nset @TypeID="+(storeProcedure.ActsOnStorageCell.Type as RDBMSMetaDataRepository.Class).TypeID.ToString()+"\n";
			NewStoreProcedureDef+="SET @TableID = IDENT_CURRENT('"+storeProcedure.ActsOnStorageCell.MainTable.Name+"')\n";
			//TODO:Η IDENT_CURRENT θα πρέπει να αντικατασταθεί απο την SCOPE_IDENTITY()

			NewStoreProcedureDef+="if (@TableID<1 OR @TableID>1)\nbegin\n\tset @TableID=@TableID+1\nend\n";
			NewStoreProcedureDef+="else\nbegin\n\t";
			NewStoreProcedureDef+="set @FirstRecord=1\n\t";
			NewStoreProcedureDef+="INSERT INTO "+storeProcedure.ActsOnStorageCell.MainTable.Name+"(ObjCellID, IntObjID, TypeID)\n\t";
			NewStoreProcedureDef+="VALUES     (0, 0, 0)\n\t";
			NewStoreProcedureDef+="SET @TableID = IDENT_CURRENT('"+storeProcedure.ActsOnStorageCell.MainTable.Name+"')\n\t";
			NewStoreProcedureDef+="set @TableID=@TableID+1\nend\n";
			
			/*####################### Find next table ID ##################*/
			string InsertClause=null;
			string ValueClause=null;

			/*####################### Insert main table columns ##################*/
			InsertClause+="INSERT INTO "+storeProcedure.ActsOnStorageCell.MainTable.Name+"(ObjCellID, IntObjID, TypeID";
			ValueClause+="VALUES(@ObjCellID, @TableID, @TypeID";
			if(storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn!=null)
			{
				InsertClause+=","+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
				ValueClause+=",@"+storeProcedure.ActsOnStorageCell.MainTable.ReferentialIntegrityColumn.Name;
			}

			foreach(RDBMSMetaDataRepository.Column CurrColumn in storeProcedure.ActsOnStorageCell.MainTable.ContainedColumns)
			{
				if(CurrColumn.MappedAttribute!=null)
				{
					if(ClassAttriutes.Contains(CurrColumn.MappedAttribute.Name))
					{
						InsertClause+=","+CurrColumn.Name;
						ValueClause+=",@"+CurrColumn.MappedAttribute.Name;
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
				InsertClause+="INSERT INTO "+CurrTable .Name+"(ObjCellID, IntObjID, TypeID";
				ValueClause+="VALUES(@ObjCellID, @TableID, @TypeID";
				foreach(RDBMSMetaDataRepository.Column CurrColumn in CurrTable.ContainedColumns)
				{
					if(CurrColumn.MappedAttribute!=null)
					{
						InsertClause+=","+CurrColumn.Name;
						ValueClause+=",@"+CurrColumn.MappedAttribute.Name;
					}
				}
				InsertClause+=")\n";
				ValueClause+=")\n";
				NewStoreProcedureDef+=InsertClause+ValueClause;
			}

			/*####################### Insert mapped tables columns ##################*/

			NewStoreProcedureDef+="if  @FirstRecord=1\nbegin\n\t";
			NewStoreProcedureDef+="delete   "+storeProcedure.ActsOnStorageCell.MainTable.Name+"\n\t";
			NewStoreProcedureDef+="WHERE (IntObjID = 0) AND (ObjCellID = 0) AND (TypeID = 0)\nend\n";
			NewStoreProcedureDef+="set @IntObjID=@TableID\n";
			return NewStoreProcedureDef;
		

		}

		/// <MetaDataID>{F93714FE-1F4A-4121-A3BF-F0B61A5E4B66}</MetaDataID>
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
                PrameterList += "@" + CurrParameter.Name + " " + TypeDictionary.GetDBType(parameterType);
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
			foreach(RDBMSMetaDataRepository.Column Column in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(storeProcedure))
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
				foreach(RDBMSMetaDataRepository.Column Column in RDBMSMetaDataRepository.AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(storeProcedure))
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
		/// <MetaDataID>{33D96D2F-B20F-4715-80A1-CAA574321D7D}</MetaDataID>
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
				if(TypeDictionary.IsTypeVarLength(CurrParameter.Type.FullName))
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
					if(ClassAttriutes.Contains(CurrColumn.MappedAttribute.Name))
					{
						if(FirstColumn)
						{
							UpdateStoreProcedure+=CurrColumn.Name+" = "+"@"+CurrColumn.MappedAttribute.Name;
							FirstColumn=false;
						}
						else
							UpdateStoreProcedure+=","+CurrColumn.Name+" = "+"@"+CurrColumn.MappedAttribute.Name;
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
								UpdateStoreProcedure+=CurrColumn.Name+" = "+"@"+CurrColumn.MappedAttribute.Name;
								FirstColumn=false;
							}
							else
								UpdateStoreProcedure+=","+CurrColumn.Name+" = "+"@"+CurrColumn.MappedAttribute.Name;
						}
					}
				}
				UpdateStoreProcedure+="\nWHERE(";
				Count=0;
				foreach(RDBMSMetaDataRepository.Column Column in storeProcedure.ActsOnStorageCell.MainTable.ObjectIDColumns)
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


 
		
		       
		

	}
}
