
namespace OOAdvantech.PersistenceLayerRunTime 
{
	using System;
	using System.Reflection;
	using PersistenceLayer;
	using System.Runtime.Remoting.Messaging;

	/// <MetaDataID>{230B8E38-145E-4C6B-9E96-4B164AC9E2D7}</MetaDataID>
	/// <summary>Los
	/// tt 
	/// djdgj
	/// dgjdf</summary>
	[MetaDataRepository.MetaObjectID("{230B8E38-145E-4C6B-9E96-4B164AC9E2D7}")]
	public class PersistencyContext : PersistenceLayer.PersistencyContext
	{
		/// <summary>This method creates a new object with type that defined from parameter Type in storage of the StorageSession Storage.</summary>
		/// <param name="Type">Define the type of object that will be creating.</param>
		/// <param name="Version">Define version of module that implement the type.</param>
		/// <param name="Storage">Define the storage session with storage in which will be creating the object.</param>
		/// <MetaDataID>{4606ADE0-4B51-49B5-BE30-F8B012290FCF}</MetaDataID>
		public override object NewObject (System.Type Type,PersistenceLayer.StorageSession Storage,params object[] ctorParams)
		{
			//Error prone εάν το Assembly του type δεν είναι persistent to χτιπάει στο ξεκούδουνο
			return NewObject(Type.FullName,"",Storage,ctorParams);
		}

		/// <MetaDataID>{FC61BDD1-2E1C-46F7-86FF-7C2E9660E710}</MetaDataID>
		public override PersistenceLayer.StorageSession NewStorage(PersistenceLayer.ObjectStorage OriginalStorage, string StorageName, string StorageLocation, string StorageType,bool InProcess)
		{
			if(InProcess)
			{
				StorageProvider aStorageProvider= (StorageProvider)ModulePublisher.ClassRepository.CreateInstance(StorageType,"");
				if(aStorageProvider==null)
					throw (new System.Exception("PersistencyContext Cann't instadiate the "+StorageType+ " Provider"));
				return aStorageProvider.NewStorage(OriginalStorage, StorageName,StorageLocation);
			}
			else
				return NewStorage(OriginalStorage, StorageName, StorageLocation, StorageType);

		}

		/// <MetaDataID>{16DB4D4D-8600-41FD-BEBA-4D19832489CA}</MetaDataID>
		public void PersistencyContextInitialize()
		{
			_CurrentPersistencyContext=this;
		}
		/// <MetaDataID>{75600A3D-42E0-4781-9988-CA57DDCED64A}</MetaDataID>
		 public PersistencyContext ()
		{
			Transactions.TransactionManager.RegisterTransactionContextProvider(new TransactioContextProvider());

			foreach(System.Reflection.Assembly CurrAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					DotNetMetaDataRepository.Assembly MetaAssembly=(DotNetMetaDataRepository.Assembly)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(CurrAssembly);
					if(MetaAssembly==null)
						MetaAssembly=new DotNetMetaDataRepository.Assembly(CurrAssembly);
				}
				catch(System.Exception mException)
				{
					System.Diagnostics.Debug.WriteLine(mException.StackTrace);
					int lo=0;
					//Error Prone
				}
			}
		}

		/// <MetaDataID>{477E3558-6CBB-4AF4-9960-944D566566C4}</MetaDataID>
		private System.Collections.Specialized.HybridDictionary OpenStorages;
	
	
		/// <MetaDataID>{6b277d8e-f5d8-426c-9473-ab3cd96492ab}</MetaDataID>
		public override object NewObject(string ClassName, string ClassVersion, PersistenceLayer.StorageSession Storage,params object[] ctorParams)
		{
			if(Storage==null)
				throw (new System.Exception("The Storage is empty"));

			string ComputerName=System.Windows.Forms.SystemInformation.ComputerName;
			if(Storage.StorageMetaData!=null) //Error Prone οτι δεν έχει StorageMetaData δεν παει ναπει ότι 
			{
				if(Storage.StorageMetaData.StorageLocation.ToLower()!=ComputerName.ToLower())
				{
					int werr=9;
					return GetPersistencyContextOnMachine(Storage.StorageMetaData.StorageLocation).NewObject(ClassName,ClassVersion, Storage,ctorParams) ; 
				}
			}


			PersistenceLayer.Persistent mPresistentAttribute=new PersistenceLayer.Persistent("");
			object NewObject=null;
			try
			{
				NewObject=ModulePublisher.ClassRepository.CreateInstance(ClassName,ClassVersion,ctorParams);
			}
			catch(System.Exception Error)
			{
				throw new System.Exception("PersistencyContext Cann't instadiate the "+ClassName,Error);
			}
			if(NewObject==null)
				throw (new System.Exception("PersistencyContext Cann't instadiate the "+ClassName));
			PersistenceLayerRunTime.StorageInstanceRef mStorageInstanceRef;
			mStorageInstanceRef=(PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.StorageSession)Storage).CreateStorageInstanceRef(NewObject);
			using (Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(NewObject))
			{
				
				StateTransition.SetComplete();
				return NewObject;
			}


			//long ObjectStateTransitionID=BeginObjectStateTransition(NewObject,Transactions.TransactionOption.Required);
			//CommitObjectStateTransition(NewObject,ObjectStateTransitionID );
			return NewObject;
		}
	
		/// <MetaDataID>{67cc348d-d791-4f8a-948e-b707d1d24e51}</MetaDataID>
		public override void DeleteObject(object thePersistentObject)
		{
			if(thePersistentObject==null)
				return;
			StorageInstanceRef ObjStorageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(thePersistentObject) as StorageInstanceRef;
			if(ObjStorageInstanceRef==null)
				return;
			//if(ObjStorageInstanceRef.ObjectID==null)
				//return;
			//long ObjectStateTransitionID=BeginObjectStateTransition(thePersistentObject,Transactions.TransactionOption.Required);
			using (Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(thePersistentObject))
			{
				StorageSession mStorageSession=(StorageSession)ObjStorageInstanceRef.ActiveStorageSession;
				mStorageSession.CreateDeleteStorageInstanceCommand(ObjStorageInstanceRef,false);
				StateTransition.SetComplete();
			}

			//CommitObjectStateTransition(thePersistentObject,ObjectStateTransitionID );
		}
		/// <MetaDataID>{e0b88b8a-dead-4b95-b94e-aafcb6f4aee4}</MetaDataID>
		public override PersistenceLayer.StorageSession NewStorage(ObjectStorage OriginalStorage, string StorageName, string StorageLocation, string StorageType)
		{
			string ComputerName=System.Windows.Forms.SystemInformation.ComputerName;
			if(StorageLocation.ToLower()!=ComputerName.ToLower())
			{
				int werr=9;
				return GetPersistencyContextOnMachine(StorageLocation).NewStorage(OriginalStorage,StorageName, StorageLocation, StorageType) ; 
			}

			StorageProvider aStorageProvider= (StorageProvider)ModulePublisher.ClassRepository.CreateInstance(StorageType,"");
			if(aStorageProvider==null)
				throw (new System.Exception("PersistencyContext Cann't instadiate the "+StorageType+ " Provider"));

			return aStorageProvider.NewStorage(OriginalStorage, StorageName,StorageLocation);
		}
		/// <MetaDataID>{2D5834A2-9C2B-4080-A007-F1C8C7CF67B4}</MetaDataID>
		PersistencyContext GetPersistencyContextOnMachine(string ComputerName)
		{
			try
			{
				Remoting.RemotingServices theLocalRemotingServices=System.Activator.GetObject(typeof(Remoting.RemotingServices),"tcp://"+ComputerName+":4000/PersistencyServer/RemotingServices") as Remoting.RemotingServices;
				return (PersistencyContext)theLocalRemotingServices.CreateInstance(typeof(PersistencyContext).ToString());
			}
			catch(System.Exception Error)
			{
				int hh=0;
				throw new System.Exception("System can't connect with machine '"+ComputerName+"'.",Error);

			}
		}
		
		/// <MetaDataID>{232FEFC4-3CBD-4F07-A7D4-25BA302717B4}</MetaDataID>
		public PersistenceLayer.StorageSession OpenStorage(string StorageName, string StorageLocation, string StorageType,bool Embedded)  
		{
			if(!Embedded)
			{
				string ComputerName=System.Windows.Forms.SystemInformation.ComputerName;
				if(StorageLocation.ToLower()!=ComputerName.ToLower())
					return GetPersistencyContextOnMachine(StorageLocation).OpenStorage(StorageName, StorageLocation, StorageType) ; 
			}
			if(OpenStorages==null)
				OpenStorages=new System.Collections.Specialized.HybridDictionary();
			if(OpenStorages.Contains(StorageLocation+"."+StorageName))
				return (PersistenceLayer.StorageSession)OpenStorages[StorageLocation+"."+StorageName];
			StorageProvider aStorageProvider= (StorageProvider)ModulePublisher.ClassRepository.CreateInstance(StorageType,"");
			if(aStorageProvider==null)
				throw (new System.Exception("PersistencyContext Cann't instadiate the "+StorageType+ " Provider"));
			PersistenceLayer.StorageSession OpenedStorageSession=aStorageProvider.OpenStorage(StorageName,StorageLocation);
			OpenStorages[StorageLocation+"."+StorageName]=OpenedStorageSession; // error prone;
			return OpenedStorageSession;


		}
		/// <summary>Trading</summary>
		/// <param name="StorageName">Tracking</param>
		/// <param name="StorageLocation">Used to indicate status.</param>
		/// <param name="StorageType">Trading</param>
		/// <MetaDataID>{29ebf772-0bb1-49fd-bf67-5a628a6280cf}</MetaDataID>
		public override PersistenceLayer.StorageSession OpenStorage(string StorageName, string StorageLocation, string StorageType)  
		{
			return OpenStorage(StorageName, StorageLocation, StorageType,false);  
		}
		 
	

		/// <MetaDataID>{A5622BD1-3687-4752-9924-467523427298}</MetaDataID>
		public override void CommitObjectState(object Object,PersistenceLayer.StorageSession Storage )
		{
			if(StorageInstanceRef.GetStorageInstanceRef(Object)==null)
				((PersistenceLayerRunTime.StorageSession)Storage).CreateStorageInstanceRef(Object);
			CommitObjectState(Object);
		}

		/// <MetaDataID>{FB67F76E-C9CA-4E0D-94FA-360E2551D273}</MetaDataID>
		public override void CommitObjectState(object Object )
		{
			using (Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(Object))
			{
				StateTransition.SetComplete();
			}

//			long ObjectStateTransitionID=BeginObjectStateTransition(Object,TransactionType);
//			CommitObjectStateTransition(Object,ObjectStateTransitionID);
		}


	

	}
}
