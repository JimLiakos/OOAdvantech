namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{2B55942A-75E9-49F0-8FB0-0F3C09B12141}</MetaDataID>
	public class MetaObjectsStack : MetaDataRepository.MetaObjectsStack
	{
        public MetaObjectsStack()
        {
        }
        public MetaObjectsStack(bool manualMapping)
        {
            ManualMapping = manualMapping;
        }
        /// <MetaDataID>{e971f202-4402-454a-83c5-6d7389ebcdf3}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID GetIdentity(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            return metaObject.Identity;
        }
        public readonly bool ManualMapping;
        /// <MetaDataID>{7D7AC63F-19DE-4646-A57A-9B93150E0F77}</MetaDataID>
        System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> MetaObjects = new System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();
		/// <MetaDataID>{89D73733-F74E-4A07-A7EB-8FDBB010426F}</MetaDataID>
		bool MetObjectsLoaded=false;
		/// <MetaDataID>{E3681881-2415-4114-B827-8336B17A4081}</MetaDataID>
		public override MetaDataRepository.MetaObject FindMetaObjectInPLace(MetaDataRepository.MetaObject OriginMetaObject, MetaDataRepository.MetaObject placeIdentifier)
		{
			return FindMetaObjectInPLace(OriginMetaObject.Identity.ToString(),placeIdentifier);
		}
		/// <MetaDataID>{3B910160-2CD4-4A9A-B0D8-57A77BF881BD}</MetaDataID>
		public override OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectInPLace(string MetaObjectID, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
		{
			if(MetaObjectID==null)
				return null;
			MetaDataRepository.MetaObject FindedMetaObject=null;
			if(!MetObjectsLoaded)
			{

                Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties).Execute("SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject ");//WHERE MetaObjectIDStream = \""+OriginMetaObject.Identity.ToString()+"\" ");
                foreach (Collections.StructureSet Rowset in aStructureSet)
				{
					MetaDataRepository.MetaObject metaObject=(MetaDataRepository.MetaObject)Rowset["MetaObject"]; 
					if(metaObject.Identity.ToString()==null)
					{
						int were=0;
					}
					MetaObjects[metaObject.Identity.ToString()]=metaObject;
				}
				//TODO:Είναι λάθος γιατί φορτώνουμε τα object αυτής της storage και θεωρού ότι εχουμε φορτώσει και για τις άλλε που τυχών θα έρθουν.
				MetObjectsLoaded=true;
			}
            MetaDataRepository.MetaObject theMetaObject=null;
            if (MetaObjects.TryGetValue(MetaObjectID, out theMetaObject))
                return theMetaObject;
            return null;
			
		}

		/// <MetaDataID>{6837AFE8-6EFB-49DD-A9DE-0947D97BD1FD}</MetaDataID>
		private System.Collections.Generic.Dictionary<string,OOAdvantech.MetaDataRepository.MetaObject> NewCreatedObjects;
		/// <MetaDataID>{592A0A55-A1AC-4618-B644-78763A19BD2A}</MetaDataID>
		public override MetaDataRepository.MetaObject CreateMetaObjectInPlace(MetaDataRepository.MetaObject OriginMetaObject, MetaDataRepository.MetaObject placeIdentifier)
		{
			PersistenceLayer.ObjectStorage InStorage=PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties);
			if(NewCreatedObjects==null)
				NewCreatedObjects=new System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject>();
			if(NewCreatedObjects.ContainsKey(OriginMetaObject.Identity.ToString()))
				return (MetaDataRepository.MetaObject)NewCreatedObjects[OriginMetaObject.Identity.ToString()];
			MetaDataRepository.MetaObject NewMetaObject=null;
			
			if(typeof(MetaDataRepository.Class).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Class));
				goto End;
			}

			if(typeof(MetaDataRepository.Structure).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Structure));
				goto End;
			}

			if(typeof(MetaDataRepository.Generalization).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Generalization));
				goto End;
			}

			if(typeof(MetaDataRepository.Dependency).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Dependency));
				goto End;
			}

			
			if(typeof(MetaDataRepository.Component).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Component));
				goto End;
			}


			if(typeof(MetaDataRepository.Enumeration).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Enumeration));
				goto End;
			}

			if(typeof(MetaDataRepository.Interface).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Interface));
				goto End;
			}



			if(typeof(MetaDataRepository.Attribute).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Attribute));
				goto End;
			}


            if (typeof(MetaDataRepository.Operation).GetMetaData().IsInstanceOfType(OriginMetaObject))
            {
               // NewMetaObject = (MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Operation));
                return null;
            }

            if (typeof(MetaDataRepository.Method).GetMetaData().IsInstanceOfType(OriginMetaObject))
            {
               // NewMetaObject = (MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Method));
                return null;
                
            }


			if(typeof(MetaDataRepository.Primitive).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Primitive));
				goto End;
			}

			
			if(typeof(MetaDataRepository.AssociationEnd).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.AssociationEnd));
				goto End;
			}

	
			if(typeof(MetaDataRepository.Association).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(RDBMSMetaDataRepository.Association));
				goto End;
			}


			if(typeof(MetaDataRepository.Namespace).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Namespace));
				goto End;
			}
			if(typeof(MetaDataRepository.AssociationEndRealization).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.AssociationEndRealization));
				goto End;
			}
			if(typeof(MetaDataRepository.AttributeRealization).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.AttributeRealization));
				goto End;
			}

			if(typeof(MetaDataRepository.Realization).GetMetaData().IsInstanceOfType(OriginMetaObject))
			{
				NewMetaObject=(MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Realization));
				goto End;
			}
            if (OriginMetaObject is MetaDataRepository.Parameter)
            {
                NewMetaObject = (MetaDataRepository.MetaObject)InStorage.NewObject(typeof(MetaDataRepository.Parameter));
                goto End;
            }



			
			End:
				if(NewMetaObject!=null)
					InitializeMetaObject(OriginMetaObject,NewMetaObject);
			NewCreatedObjects[NewMetaObject.Identity.ToString()]=NewMetaObject;
			if(MetaObjects!=null)
				MetaObjects[NewMetaObject.Identity.ToString()]=NewMetaObject;

			return NewMetaObject;
		}
	}
}
