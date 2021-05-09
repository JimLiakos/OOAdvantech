namespace OOAdvantech.OraclePersistenceRunTime
{


    /// <MetaDataID>{508acbaf-9d7d-4d45-849b-6e7355cd92ac}</MetaDataID>
    public class StorageInstanceRef : RDBMSPersistenceRunTime.StorageInstanceRef
	{

        public StorageInstanceRef(object memoryInstance, OOAdvantech.MetaDataRepository.StorageCell storageCell, ObjectStorage activeStorageSession, PersistenceLayer.ObjectID objectID)
            : base(memoryInstance, storageCell, activeStorageSession, objectID)
        {


        }

	
		public RDBMSMetaDataRepository.StorageCell StorageInstanceSet;


	
        protected override PersistenceLayerRunTime.RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
		{
			return new RDBMSPersistenceRunTime. RelResolver(this,thePersistentAssociationEnd, fastFieldAccessor);
		}
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new RDBMSPersistenceRunTime.RelResolver(owner, thePersistentAssociationEnd, fastFieldAccessor);
            
        }
	
		public string StorageInstanceSetIdentity
		{
			get
			{
				return StorageInstanceSet.Identity.ToString();
			}
		}
	
        //public void SaveObjectState()
        //{



        //    /*
        //    if(!OleDbCommand.Parameters.Contains("@ObjCellID"))
        //        OleDbCommand.Parameters.Add("@ObjCellID",((int)PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(StorageInstanceSet.Properties).ObjectID));
        //    else
        //        OleDbCommand.Parameters["@ObjCellID"].Value=((int)PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(StorageInstanceSet.Properties).ObjectID);
        //        */

			
        //    //	theXmlElement.SetAttribute("ReferentialIntegrityCount",ReferentialIntegrityCount.ToString());

        //    if(Class.HasReferentialIntegrityRelations())
        //    {
        //        System.Data.SqlClient.SqlParameter CurrParameter=null;
        //        if(!OleDbCommand.Parameters.Contains("@ReferenceCount"))
        //            CurrParameter=OleDbCommand.Parameters.Add("@ReferenceCount",_ReferentialIntegrityCount);
        //        else
        //        {
        //            CurrParameter=OleDbCommand.Parameters["@ReferenceCount"];
        //            OleDbCommand.Parameters["@ReferenceCount"].Value=_ReferentialIntegrityCount;
        //        }
        //    }

        //    foreach(ValueOfAttribute  ValueOfAttribute in GetPersistentAttributeValues())
        //    {
        //        //if(!Class.IsPersistent(attribute))
        //        //    continue;

        //        RDBMSMetaDataRepository.Attribute rdbmsAttribute = ValueOfAttribute.Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
        //        RDBMSMetaDataRepository.Column column = null;
        //        foreach(RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(StorageInstanceSet))
        //        {
        //            if(attributeColumn.CreatorIdentity==ValueOfAttribute.PathIdentity)
        //            {
        //                column =attributeColumn;
        //                break;
        //            }
        //        }


        //        //string parameterName=column.MappedAttribute.CaseInsensitiveName;
        //        string parameterName = column.Name;



				
        //        string FieldName = ValueOfAttribute.Attribute.Name;

        //        object Value = ValueOfAttribute.Value;
        //        System.Data.SqlClient.SqlParameter CurrParameter=null;
        //        RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (ObjectStorage as ObjectStorage).TypeDictionary;

        //        if(Value==null)
        //            Value = TypeDictionary.GetNullValue(ValueOfAttribute.FieldInfo.FieldType.FullName);

        //        if(!OleDbCommand.Parameters.Contains("@"+parameterName))
        //            CurrParameter=OleDbCommand.Parameters.Add("@"+parameterName,Value);
        //        else
        //        {
        //            CurrParameter=OleDbCommand.Parameters["@"+parameterName];
        //            OleDbCommand.Parameters["@"+parameterName].Value=Value;
        //        }
        //        //TODO να τσεκαριστεί τι γίνεται όταν η τιμή είναι DBNull
        //        /*
        //        if(Value is System.DBNull)
        //            CurrParameter.OleDbType=System.Data.OleDb.OleDbType.Empty;
        //         */


        //        //				CurrParameter.Value=Value;
        //        if (ValueOfAttribute.FieldInfo.FieldType.BaseType == typeof(System.Enum))
        //        {
					
        //            if(Value!=null)
        //            {
        //                //StringValue=Value.ToString();
        //                //theXmlElement.SetAttribute(FieldName,StringValue);
        //            }
        //            continue;
        //        }
        //        if (ValueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
        //        {
        //        }
        //        //StringValue=CurrFieldInfo.GetValue(MemoryInstance).ToString();
        //        try
        //        {
        //            //System.Convert.ChangeType(StringValue,CurrFieldInfo.FieldType);
        //            //theXmlElement.SetAttribute(FieldName,StringValue);
        //        }
        //        catch(System.Exception aException)
        //        {

        //        }
        //    }

		
        //}
		
        //public void LoadObjectState(string ColumnsNameSuffix)
        //{

        //    if(Class.HasReferentialIntegrityRelations())
        //    {
        //        object ReferentialIntegrityCountValue=DbDataRecord["ReferenceCount"+ColumnsNameSuffix];
        //        if(ReferentialIntegrityCountValue.GetType()==typeof(System.DBNull))
        //            _ReferentialIntegrityCount=0;
        //        else
        //            _ReferentialIntegrityCount=(int)ReferentialIntegrityCountValue;


        //    }
        //    RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (ObjectStorage as ObjectStorage).TypeDictionary;


        //    foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues())
        //    {

                
        //        RDBMSMetaDataRepository.Attribute rdbmsAttribute=valueOfAttribute .Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;


        //        if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
        //        {

        //            throw new System.Exception("MSSQL subsystem can't save xml document");
        //        }
        //        else
        //        {
        //            RDBMSMetaDataRepository.Column column = null;
        //            foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(StorageInstanceSet))
        //            {
        //                if (attributeColumn.CreatorIdentity == valueOfAttribute.PathIdentity)
        //                {
        //                    column = attributeColumn;
        //                    break;
        //                }
        //            }


        //            //TODO: Performance tunning πανω στο get value από την recordset 
        //            //όταν πέρνω data χρησιμοποιό αλλη τεχνική από αυτήν που σώζω caseinsensitive τιμές



        //            object Value = DbDataRecord[column.Name + ColumnsNameSuffix];
        //            Value = TypeDictionary.Convert(Value, valueOfAttribute.FieldInfo.FieldType);

        //            if (Value is System.DBNull)
        //                Value = null;
        //            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, Value, valueOfAttribute.ValueTypePath));
        //            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, Value);
        //        }
        //    }

			


		
        //}

        //public System.Data.SqlClient.SqlCommand OleDbCommand;
		
        //public System.Data.DataRow  DbDataRecord;


        //internal static object GetValueTypeValue(OOAdvantech.MetaDataRepository.Attribute member, System.Data.DataRow row, string ColumnsNameSuffix, RDBMSMetaDataRepository.StorageCell storageInstanceSet)
        //{
        //    System.Type type = member.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
        //    object obj= type.Assembly.CreateInstance(type.FullName);

        //    RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (PersistenceLayerRunTime.ObjectStorage.GetStorageOfObject(storageInstanceSet) as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).TypeDictionary;

        //    MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
        //    valueTypePath.Push(member.Identity);
        //    foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues(obj, member.Type as DotNetMetaDataRepository.Structure, valueTypePath))
        //    {


        //        RDBMSMetaDataRepository.Attribute rdbmsAttribute = (storageInstanceSet.Namespace as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(valueOfAttribute.Attribute) as RDBMSMetaDataRepository.Attribute;


        //        if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
        //        {
        //            throw new System.Exception("MSSQL subsystem can't save xml document");
        //        }
        //        else
        //        {
                  

        //            //TODO: Performance tunning πανω στο get value από την recordset 
        //            //όταν πέρνω data χρησιμοποιό αλλη τεχνική από αυτήν που σώζω caseinsensitive τιμές
        //            object Value = row[rdbmsAttribute.Name + ColumnsNameSuffix];
        //            Value = TypeDictionary.Convert(Value, valueOfAttribute.FieldInfo.FieldType);

        //            bool valueSetted = false;
        //            if (Value is System.DBNull)
        //                SetAttributeValue(ref obj, new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, null, valueOfAttribute.ValueTypePath), member.Type as DotNetMetaDataRepository.Structure,0, out valueSetted);
        //            else
        //                SetAttributeValue(ref obj, new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, Value, valueOfAttribute.ValueTypePath), member.Type as DotNetMetaDataRepository.Structure,0, out valueSetted);
        //        }
        //    }



        //    return obj;


            
        //}
    
    }
}
