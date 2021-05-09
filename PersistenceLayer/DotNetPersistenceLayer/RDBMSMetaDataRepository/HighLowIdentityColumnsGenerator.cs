namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{EE7E9481-7FD1-4765-B6E5-0658BA43C4BD}</MetaDataID>
	public class HighLowAutoProduceColumnsGenerator : AutoProduceColumnsGenerator
	{ 
//		/// <MetaDataID>{FA001638-8D3A-406D-8237-1F4E4808547A}</MetaDataID>
//		public override MetaDataRepository.MetaObjectCollection CreateAuxiliaryColumns(MetaDataRepository.MetaObject placeIdentifier)
//		{
//
//			MetaDataRepository.MetaObjectCollection columns=new OOAdvantech.MetaDataRepository.MetaObjectCollection();		
//			MetaDataRepository.Primitive SystemInt32Type=MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32",placeIdentifier) as MetaDataRepository.Primitive;
//			if(SystemInt32Type==null)
//			{
//				MetaDataRepository.Namespace mNamespace=new MetaDataRepository.Namespace();
//				mNamespace.Name="System";
//				MetaDataRepository.Primitive mPrimitive =new MetaDataRepository.Primitive();
//				mPrimitive.Name="Int32";
//				mNamespace.AddOwnedElement(mPrimitive);
//				SystemInt32Type=(Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive,placeIdentifier);
//				SystemInt32Type.Synchronize(mPrimitive);
//			}
//			using (OOAdvantech.Transactions.SystemStateTransition StateTransition=new OOAdvantech.Transactions.SystemStateTransition())
//			{
//				Column column=(Column)PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties).NewObject(typeof(Column));
//				column.Name="TableID";
//				column.Type=SystemInt32Type;
//				column.Identity=true;
//				column.AllowNulls=false;
//
//				columns.Add(column);
//				column=(Column)PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties).NewObject(typeof(Column));
//				column.Name="TypeID";
//				column.Type=SystemInt32Type;
//				column.AllowNulls=false;
//				columns.Add(column);
//				StateTransition.Consistent=true;
//			}
//			return columns;
//		
//		}
//
//	
//		/// <MetaDataID>{B46B2E38-55DC-4AC8-A439-B309A0B0A752}</MetaDataID>
//		public override MetaDataRepository.MetaObjectCollection CreateObjectIdentityColumns(MetaDataRepository.MetaObject placeIdentifier)
//		{
//
//
//			MetaDataRepository.MetaObjectCollection identityColums=new OOAdvantech.MetaDataRepository.MetaObjectCollection();		
//			MetaDataRepository.Primitive SystemInt32Type=MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32",placeIdentifier) as MetaDataRepository.Primitive;
//			if(SystemInt32Type==null)
//			{
//				MetaDataRepository.Namespace mNamespace=new MetaDataRepository.Namespace();
//				mNamespace.Name="System";
//				MetaDataRepository.Primitive mPrimitive =new MetaDataRepository.Primitive();
//				mPrimitive.Name="Int32";
//				mNamespace.AddOwnedElement(mPrimitive);
//				SystemInt32Type=(Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive,placeIdentifier);
//				SystemInt32Type.Synchronize(mPrimitive);
//			}
//			using (OOAdvantech.Transactions.SystemStateTransition StateTransition=new OOAdvantech.Transactions.SystemStateTransition())
//			{
//				IdentityColumn InternalObjIDColumn=(IdentityColumn)PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties).NewObject(typeof(IdentityColumn));
//				InternalObjIDColumn.Name="IntObjID";
//				InternalObjIDColumn.ProducedFromRDBMS=true;
//				InternalObjIDColumn.Type=SystemInt32Type;
//				InternalObjIDColumn.ColumnType="IntObjID";
//
//				IdentityColumn ObjCellIDColumn=(IdentityColumn)PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties).NewObject(typeof(IdentityColumn));
//				ObjCellIDColumn.Name="StorageCellID";
//				ObjCellIDColumn.Type=SystemInt32Type;
//				ObjCellIDColumn.ColumnType="StorageCellID";
//				identityColums.Add(ObjCellIDColumn);
//				identityColums.Add(InternalObjIDColumn);
//				StateTransition.Consistent=true;
//			}
//			return identityColums;
//		}
//

         /// <MetaDataID>{b1673112-c184-4d54-a425-0a8d45fb9223}</MetaDataID>
		public override Collections.Generic.Set<Column> GetAuxiliaryColumns(MetaDataRepository.MetaObject placeIdentifier)
		{

            Collections.Generic.Set <Column> columns = new OOAdvantech.Collections.Generic.Set<Column>();		
			MetaDataRepository.Primitive SystemInt32Type=MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32",placeIdentifier) as MetaDataRepository.Primitive;
			if(SystemInt32Type==null||SystemInt32Type.Name==null)
			{
				MetaDataRepository.Namespace mNamespace=new MetaDataRepository.Namespace();
				mNamespace.Name="System";
				MetaDataRepository.Primitive mPrimitive =new MetaDataRepository.Primitive();
				mPrimitive.Name="Int32";
				mNamespace.AddOwnedElement(mPrimitive);
				SystemInt32Type=(Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive,placeIdentifier);
				SystemInt32Type.Synchronize(mPrimitive);
			}
			Column column=new Column();
			column.Name="TableID";
			column.Type=SystemInt32Type;
			column.IsIdentity=true;
			column.AllowNulls=false;

			columns.Add(column);
			column=new Column();
			column.Name="TypeID";
			column.Type=SystemInt32Type;
			column.AllowNulls=false;
			columns.Add(column);
			return columns;
		
		}


        /// <MetaDataID>{760afb74-0dce-43b6-9588-fe9276729c8f}</MetaDataID>
        public override MetaDataRepository.ObjectIdentityType GetObjectIdentityRefernce(OOAdvantech.MetaDataRepository.MetaObject placeIdentifier, AssociationEnd associationEnd, string associationName)
        {
            System.Collections.Generic.List<MetaDataRepository.IIdentityPart> identityColums = new System.Collections.Generic.List<MetaDataRepository.IIdentityPart>();
            MetaDataRepository.Primitive SystemInt32Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32", placeIdentifier) as MetaDataRepository.Primitive;
            if (SystemInt32Type == null || SystemInt32Type.Name == null)
            {
                //MetaDataRepository.Namespace mNamespace = new MetaDataRepository.Namespace();
                //mNamespace.Name = "System";
                //MetaDataRepository.Primitive mPrimitive = new MetaDataRepository.Primitive();
                //mPrimitive.Name = "Int32";
                //mNamespace.AddOwnedElement(mPrimitive);
                MetaDataRepository.Primitive mPrimitive = MetaDataRepository.Classifier.GetClassifier(typeof(int)) as MetaDataRepository.Primitive;
                SystemInt32Type = (Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive, placeIdentifier);
                SystemInt32Type.Synchronize(mPrimitive);
            }
            if (associationEnd.IsRoleA)
            {
                IdentityColumn InternalObjIDColumn = new IdentityColumn(associationName + "_IntObjIDB",associationEnd, SystemInt32Type, "IntObjID", true);
                IdentityColumn ObjCellIDColumn = new IdentityColumn(associationName + "_StorageCellIDB", associationEnd, SystemInt32Type, "StorageCellID", false);
                InternalObjIDColumn.AllowNulls = true;
                ObjCellIDColumn.AllowNulls = true;

                identityColums.Add(ObjCellIDColumn);
                identityColums.Add(InternalObjIDColumn);
            }
            else
            {
                IdentityColumn InternalObjIDColumn = new IdentityColumn(associationEnd.Association.Name + "_IntObjIDA", associationEnd, SystemInt32Type, "IntObjID", true);
                IdentityColumn ObjCellIDColumn = new IdentityColumn(associationEnd.Association.Name + "_StorageCellIDA", associationEnd, SystemInt32Type, "StorageCellID", false);
                InternalObjIDColumn.AllowNulls = true;
                ObjCellIDColumn.AllowNulls = true;
                identityColums.Add(ObjCellIDColumn);
                identityColums.Add(InternalObjIDColumn);
            }
            //return new OOAdvantech.MetaDataRepository.ObjectIdentityType(identityColums);
            MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(identityColums);
            foreach (IdentityColumn idColumn in objectIdentityType.Parts)
                idColumn.ObjectIdentityType = objectIdentityType;

            return objectIdentityType;


            
        }
        /// <MetaDataID>{e9d0b6a8-c3e0-400f-8451-f067b62117fd}</MetaDataID>
		public override MetaDataRepository.ObjectIdentityType GetObjectIdentityType(MetaDataRepository.MetaObject placeIdentifier)
		{
            System.Collections.Generic.List<MetaDataRepository.IIdentityPart> identityColums = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>();
			MetaDataRepository.Primitive SystemInt32Type=MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32",placeIdentifier) as MetaDataRepository.Primitive;
			if(SystemInt32Type==null||SystemInt32Type.Name==null)
			{
				MetaDataRepository.Namespace mNamespace=new MetaDataRepository.Namespace();
				mNamespace.Name="System";
				MetaDataRepository.Primitive mPrimitive =new MetaDataRepository.Primitive();
				mPrimitive.Name="Int32";
				mNamespace.AddOwnedElement(mPrimitive);
				SystemInt32Type=(Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive,placeIdentifier);
				SystemInt32Type.Synchronize(mPrimitive);
			}
            IdentityColumn InternalObjIDColumn = new IdentityColumn("IntObjID", SystemInt32Type, "IntObjID",true);

            IdentityColumn ObjCellIDColumn = new IdentityColumn("StorageCellID", SystemInt32Type, "StorageCellID",false);
			identityColums.Add(ObjCellIDColumn);
			identityColums.Add(InternalObjIDColumn);
			//return new OOAdvantech.MetaDataRepository.ObjectIdentityType( identityColums);
            MetaDataRepository.ObjectIdentityType objectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(identityColums);
            foreach (IdentityColumn idColumn in objectIdentityType.Parts)
                idColumn.ObjectIdentityType = objectIdentityType;

            return objectIdentityType;

		}

        /// <MetaDataID>{cd814519-d40d-4096-9de0-d7c3f06f2a7c}</MetaDataID>
		public override string[] GetObjectIdentityColumnsName()
		{
			return new string[2]{"IntObjID","StorageCellID"};
		}
        /// <MetaDataID>{be74e46e-f483-4743-8e73-9f2d4c170b9f}</MetaDataID>
		public override string[] GetAuxiliaryColumnsName()
		{
			return new string[1]{"TypeID"};
		}


	}
}
