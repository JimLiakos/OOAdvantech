namespace XMLPersistenceRunTime
{
	using System.Xml;
	using System;
	using PersistenceLayerRunTime;

	/// <MetaDataID>{CFBDF32B-B5E1-4B6A-9EA2-D1161A37B9AE}</MetaDataID>
	/// <summary></summary>
	public class XMLStorageSession : PersistenceLayerRunTime.StorageSession 
	{
		public override PersistenceLayerRunTime.LinkCommand CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB,MetaDataRepository.Association LinkType)
		{
			return null;
		}
		public override PersistenceLayerRunTime.LinkCommand CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB,MetaDataRepository.Association LinkType)
		{
			return null;
		}

		public override PersistenceLayer.ObjectStorage StorageMetaData
		{
			get
			{
				return null;
			}
		}
		public override PersistenceLayerRunTime.UpdateReferentialIntegrity CreateUpdateReferentialIntegrity()
		{
			return null;
		}

		/// <MetaDataID>{11CD92EC-4208-4BF8-B0E3-19FE1DC41589}</MetaDataID>
		public override AssociationLinkObjectCommand CreateAssociationLinkObjectCommand()
		{
			return null;
		}
		/// <MetaDataID>{B89FA516-498E-422B-9F24-18E92FA1BB19}</MetaDataID>
		public override DeleteAssociationLinkObjectCommand CreateDeleteAssociationLinkObjectCommand()
		{
			return null;
		}

		/// <MetaDataID>{2F57BD81-E7B6-4217-8C14-23D96E4E74BA}</MetaDataID>
		public override void AbortChanges(PersistenceLayerRunTime.Transaction  theTransaction)
		{


		}
		/// <MetaDataID>{3E4B5779-90B5-4E74-924D-BDADBDD17088}</MetaDataID>
		public override void CommitChanges(PersistenceLayerRunTime.Transaction  theTransaction)
		{
		}
		/// <MetaDataID>{04A74A3E-4690-4906-82F1-6C2491E6FB4E}</MetaDataID>
		public override void BeginChanges(PersistenceLayerRunTime.Transaction theTransaction)
		{

		}

		/// <MetaDataID>{ECEDE5D6-74D8-4332-8DC2-737ACF5E7F6A}</MetaDataID>
		public override PersistenceLayerRunTime.UnlinkAllObjectCommand CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.RelResolver theRelationResolver)
		{
			return null;
		}
		/// <MetaDataID>{616A5D67-A35A-4342-9FFC-5E4053BC783C}</MetaDataID>
		private XmlDocument _XMLDocument;
		/// <MetaDataID>{3C2CC675-6700-4315-9061-2AFA3E0CE379}</MetaDataID>
		public override PersistenceLayerRunTime.DeleteStorageInstanceCommand CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef)
		{
			return null;
		}
		

		/// <MetaDataID>{DECCD5DC-9836-4EF1-8B7F-E7D71A6FA28C}</MetaDataID>
		protected override PersistenceLayer.StorageInstanceRef CreateStorageInstanceRef()
		{
			return new XMLStorageInstanceRef();
		}
		/// <summary>UpdateStorageInstanceCommand</summary>
		/// <MetaDataID>{DAAFC3BC-6F5A-40F1-B10C-0468E008200D}</MetaDataID>
		public override UpdateStorageInstanceCommand CreateUpdateStorageInstanceCommand()
		{
			return new XMLPersistenceRunTime.XMLUpdateStorageInstanceCommand();
		}
		/// <summary></summary>
		/// <MetaDataID>{2A12A199-E265-43A0-BE81-D9E74468D045}</MetaDataID>
		/// <param name="OQLStatement"></param>
		public override PersistenceLayer.StructureSet Execute(string OQLStatement)
		{
			XMLPersistenceRunTime.XMLStructureSet mXMLStructureSet=new XMLPersistenceRunTime.XMLStructureSet();
			mXMLStructureSet.SourceStorageSession=this;
			mXMLStructureSet.Open(OQLStatement);
			return mXMLStructureSet;
		}
		/// <summary></summary>
		/// <MetaDataID>{D3C3009D-0063-48B7-AEAE-B518FCA74EA9}</MetaDataID>
		/// <param name="StorageInstanceType"></param>
		/// <param name="OwnerTransactiont"></param>
		public override NewStorageInstanceCommand CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
		{

			//Exception

			XmlNodeList ObjectIDNode=XMLDocumentData.SelectNodes("Root/NextObjID");
			if(ObjectIDNode.Count!=1)
				throw (new System.Exception("Bad Storage File: Problem With ObjectIDNode"));
			XmlNodeList ObjectCollections=XMLDocumentData.SelectNodes("Root/ObjectCollections/"+ StorageInstance.MemoryInstance.GetType().FullName+"[@ClassInstaditationName = \""+StorageInstance.MemoryInstance.GetType().FullName+"\"]");
			if(ObjectCollections.Count>1)
				throw (new System.Exception("Bad Storage File: More than one ObjectCollection for Class "+StorageInstance.MemoryInstance.GetType().FullName));
			if(ObjectCollections.Count==0)
				throw (new System.Exception("Bad Storage File: There isn't ObjectCollection for Class "+StorageInstance.MemoryInstance.GetType().FullName));
			XmlNewStorageInstanceCommand aXmlNewStorageInstanceCommand=new XmlNewStorageInstanceCommand();
			aXmlNewStorageInstanceCommand.NextObjectIDNode=ObjectIDNode[0];
			aXmlNewStorageInstanceCommand.ObjectCollectionNode=ObjectCollections[0];

//************************************ initialize Association filds ****************************************
			/* temp
			MetaDataLoadingSystem.MetaDataObjectStorageWr MetaDataObjectStorage=(MetaDataLoadingSystem.MetaDataObjectStorageWr)StorageInstance.ActiveStorageSession.StorageMetaData;
			string OQLStatement="SELECT theXMLMappingMClass FROM XMLMappingMClass theXMLMappingMClass WHERE ClassInstaditationName = \"VT(8)";
			OQLStatement+=StorageInstance.MemoryInstance.GetType().FullName+"\"";
			PERSISTENCELAYERLIBLib.StructureSet theXMLMappingMClasss =MetaDataObjectStorage.MetataDataStorageSession.Execute(OQLStatement);
			theXMLMappingMClasss =MetaDataObjectStorage.MetataDataStorageSession.Execute(OQLStatement);
			XMLMAPPINGMETAOBJECTSLib.XMLMappingMClass MClass=null;
			while(theXMLMappingMClasss.IsAttheEnd()==0)
			{
				MClass=(XMLMAPPINGMETAOBJECTSLib.XMLMappingMClass)theXMLMappingMClasss.theMemberList.GetAt("theXMLMappingMClass").Value;
				theXMLMappingMClasss.MoveNext();
			}
			if(MClass!=null)
			{
				string Name=MClass.ClassInstaditationName;
				long lo=0;

			}
			
			System.Collections.Specialized.HybridDictionary AssociationMaps=new System.Collections.Specialized.HybridDictionary();
			METAMODELLib.MRoleCollection Roles=new METAMODELLib.MRoleCollectionClass(); 
			MClass.GetHeirarchyPersAssRoles(Roles);
			short Count=Roles.Count;
			for(short i=0;i!=Count;i++)
			{
				short k=i;
				k++;
				string AssociationName=Roles.GetAt(k).Association.Name;
				AssociationName+=Roles.GetAt(k).GetOtherEndRole().Name;
				AssociationMaps[AssociationName]=Roles.GetAt(k).GetOtherEndRole();
			}
			
			System.Collections.Specialized.HybridDictionary PersistenceMembers;
			PersistenceMembers=StorageInstance.LifeTimeController.PersistentAssociations;
			foreach (System.Collections.DictionaryEntry CurrObject in PersistenceMembers)
			{
				PersistenceLayerRunTime.PersClassObjects.PersistentAssociation PersistentAssociation=
					(PersistenceLayerRunTime.PersClassObjects.PersistentAssociation)CurrObject.Value;


				
				METAMODELLib.MRole FieldTypeRole=null;

				string tmpAssociationName=PersistentAssociation.PersistentAttribute.AssociationName;
				tmpAssociationName+=PersistentAssociation.FieldMember.Name;
				FieldTypeRole=(METAMODELLib.MRole)AssociationMaps[tmpAssociationName];
				if(FieldTypeRole!=null)
				{
					XMLPersistenceRunTime.XMLRelResolver mXMLRelResolver=new XMLPersistenceRunTime.XMLRelResolver();
					mXMLRelResolver.Owner=StorageInstance;
					mXMLRelResolver.DestinationObjectRole=FieldTypeRole;
					StorageInstance.RelResolvers.Add(PersistentAssociation.FieldMember,mXMLRelResolver);
					if(PersistentAssociation.FieldMember.FieldType.FullName=="PersistenceLayer.ObjectCollection") // Error prone
					{
					
						PersistenceLayerRunTime.ObjectCollection mObjectCollection=new PersistenceLayerRunTime.ObjectCollection();
						mObjectCollection.theRelResolver=mXMLRelResolver;
						PersistentAssociation.FieldMember.SetValue(StorageInstance.MemoryInstance,mObjectCollection);
					}
				}
			}

			*/
//************************************ initialize Association filds ****************************************			
			/* temp
			return aXmlNewStorageInstanceCommand;
			*/
			return null;
		
		}
		/// <summary></summary>
		/// <MetaDataID>{807D8F7A-710C-4781-8DA6-3DBCCD124DE9}</MetaDataID>
		private XmlDocument XMLDocumentData;
	
		/// <summary></summary>
		/// <MetaDataID>{5D46F167-9E24-4DD1-8FFA-EF2A94B2EA90}</MetaDataID>
		public XmlDocument XMLDocument
		{
			get
			{
				return XMLDocumentData;
			}
			set
			{
				XMLDocumentData=value;
				if(XMLDocumentData==null)
					return;
				XmlNode RootElement=XMLDocumentData.ChildNodes[0];
				if(RootElement==null)
					return;//exception
				if(RootElement.Name!="Root")
					return;//exception

				foreach(XmlNode CurrNode in RootElement.ChildNodes)
				{
					/*if(CurrNode.Name=="MetaData")
					{
						MetaDataLoadingSystem.MetaDataStorageSessionWr MetaDataStorageSession=new MetaDataLoadingSystem.MetaDataStorageSessionWr();
						MetaDataLoadingSystem.MetaDataObjectStorageWr mStorageMetaData=new MetaDataLoadingSystem.MetaDataObjectStorageWr();
						mStorageMetaData.LoadFromXMLNode(CurrNode.ChildNodes[0]);
						StorageMetaData=mStorageMetaData;
						return;
					}*/
				}
				throw (new System.Exception("Bad Storage File"));
			}
		}
	}
}
