namespace OOAdvantech.PersistenceLayerRunTime
{
	using System.Runtime.Remoting.Contexts;
	using System;
	using System.Collections;
	using System.Runtime.Remoting;
	using System.Runtime.Remoting.Channels;
	using System.Runtime.Remoting.Channels.Tcp;
	using System.Runtime.Remoting.Proxies;
	using System.Runtime.Remoting.Messaging;

	/// <MetaDataID>{FE85E55E-70B0-40EF-AB0A-E2366A08240B}</MetaDataID>
	public class ClientPersistencyContext : PersistenceLayer.PersistencyContext
	{

		/// <MetaDataID>{244AAEF9-9E89-498E-96A4-EC806B222664}</MetaDataID>
		public ClientPersistencyContext()
		{
			Transactions.TransactionManager.RegisterTransactionContextProvider(new TransactioContextProvider());
			RegisterChannel();

		}

		/// <MetaDataID>{6CF36EAD-0CC5-471D-B0DE-3DFDA3C290F6}</MetaDataID>
		private System.Collections.Hashtable RemotePersistencyContexts=new Hashtable();
		/// <MetaDataID>{02B5A95F-2016-407D-B29D-B7706706145E}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool IsChannelRegister=false;
		/// <MetaDataID>{0025DF35-4990-461E-9A49-7C525A1520D3}</MetaDataID>
		void RegisterChannel()
		{
			if(IsChannelRegister)
				return;
			//error prone τι γίνεται αν καποιοσ άλλος έχει πιασεi το κανάλι
					
			BinaryServerFormatterSinkProvider ServerProvider = new BinaryServerFormatterSinkProvider();
			
			System.Runtime.Remoting.Channels.IServerChannelSinkProvider ServerSinkProvider= OOAdvantech.Remoting.RemotingServices.GetServerChannelSinkProvider(null,null);
			System.Runtime.Remoting.Channels.IServerChannelSinkProvider TransactionServerSinkProvider=OOAdvantech.Transactions.TransactionManager.GetServerChannelSinkProvider(null,null);
			ServerSinkProvider.Next=TransactionServerSinkProvider;
			ServerProvider.Next=ServerSinkProvider;

			ServerProvider.TypeFilterLevel =System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

			System.Runtime.Remoting.Channels.IClientChannelSinkProvider ClientProvider= OOAdvantech.Remoting.RemotingServices.GetClientChannelSinkProvider(null,null);
			System.Runtime.Remoting.Channels.IClientChannelSinkProvider TransactionClientProvider=OOAdvantech.Transactions.TransactionManager.GetClientChannelSinkProvider(null,null);
			TransactionClientProvider.Next=ClientProvider;
			((IClientChannelSinkProvider)ClientProvider).Next=new BinaryClientFormatterSinkProvider();

			// Creating the IDictionary to set the port on the channel instance.
			System.Collections.IDictionary props = new System.Collections.Hashtable();
			int port=0;
			props["port"] = port;
			TcpChannel Channel = new TcpChannel(props, TransactionClientProvider, ServerProvider);
			ChannelServices.RegisterChannel(Channel);

			/*BinaryServerFormatterSinkProvider ServerProvider = new BinaryServerFormatterSinkProvider();
			OOAdvantech.Remoting.Sinks.ServerSinkProvider ServerSinkProvider=new OOAdvantech.Remoting.Sinks.ServerSinkProvider(null,null);
			ServerProvider.Next=ServerSinkProvider;
			ServerProvider.TypeFilterLevel =System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

			OOAdvantech.Remoting.Sinks.ClientSinkProvider ClientProvider=new OOAdvantech.Remoting.Sinks.ClientSinkProvider(null,null);
			((IClientChannelSinkProvider)ClientProvider).Next=new BinaryClientFormatterSinkProvider();

			// Creating the IDictionary to set the port on the channel instance.
			System.Collections.IDictionary props = new System.Collections.Hashtable();
			TcpChannel Channel = new TcpChannel(props, ClientProvider, ServerSinkProvider);
			int port=0;
			props["port"] = port;
			ChannelServices.RegisterChannel(Channel);*/
			IsChannelRegister=true;


		}
		/// <MetaDataID>{0708CE4E-7611-4F77-B74E-71DFECC8A569}</MetaDataID>
		PersistencyContext  GetPersistencyContext(string ChannelURI)
		{
			Remoting.RemotingServices theRemotingServices=System.Activator.GetObject(typeof(Remoting.RemotingServices),ChannelURI+"/PersistencyServer/RemotingServices") as Remoting.RemotingServices;
			return (PersistencyContext)theRemotingServices.CreateInstance(typeof(PersistencyContext).ToString());

		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{12789F0A-EA4B-4B4E-A9ED-AC385ED54092}</MetaDataID>
		private PersistencyContext _ServerSidePersistencyContext;
		/// <MetaDataID>{BCAF4E16-A441-4C0A-9105-C1E7791E45AF}</MetaDataID>
		private PersistencyContext ServerSidePersistencyContext
		{
			get
			{
				if(_ServerSidePersistencyContext!=null)
					return _ServerSidePersistencyContext;
				else
				{
					RegisterChannel();
					_ServerSidePersistencyContext =	GetPersistencyContext("tcp://localhost:4000");
					if(_ServerSidePersistencyContext !=null)
						RemotePersistencyContexts.Add( OOAdvantech.Remoting.RemotingServices.GetChannelUri(_ServerSidePersistencyContext),_ServerSidePersistencyContext);
					return _ServerSidePersistencyContext;
				}
			}
		}
	
		/// <summary>Trading</summary>
		/// <param name="StorageName">Tracking</param>
		/// <param name="StorageLocation">Used to indicate status.</param>
		/// <param name="StorageType">Trading</param>
		/// <MetaDataID>{D11367F1-7DBD-447F-87BD-B278AC2FC21A}</MetaDataID>
		public override PersistenceLayer.StorageSession OpenStorage(string StorageName, string StorageLocation, string StorageType)
		{
			StorageProvider aStorageProvider= (StorageProvider)ModulePublisher.ClassRepository.CreateInstance(StorageType,"");
			if(aStorageProvider==null)
				throw (new System.Exception("PersistencyContext Cann't instadiate the "+StorageType+ " Provider"));
			if(aStorageProvider.IsEmbeddedStorage(StorageName, StorageLocation))
				return EmbeddedPersistencyContext.OpenStorage(StorageName, StorageLocation, StorageType,true);

			PersistenceLayer.StorageSession StorageSession=ServerSidePersistencyContext.OpenStorage(StorageName, StorageLocation, StorageType);
			string StorageSessionChannelUri=OOAdvantech.Remoting.RemotingServices.GetChannelUri(StorageSession);
			if(!RemotePersistencyContexts.Contains(StorageSessionChannelUri))
				RemotePersistencyContexts.Add(StorageSessionChannelUri,GetPersistencyContext(StorageSessionChannelUri));
			return new ClientStorageSession(StorageSession);
		}
	
		private PersistencyContext EmbeddedPersistencyContext=new PersistencyContext();
		/// <MetaDataID>{FF94AB30-5C96-4F76-92CD-A50FF138CE55}</MetaDataID>
		public override PersistenceLayer.StorageSession NewStorage(PersistenceLayer.ObjectStorage OriginalStorage, string StorageName, string StorageLocation, string StorageType,bool Embedded)
		{
			StorageProvider aStorageProvider= (StorageProvider)ModulePublisher.ClassRepository.CreateInstance(StorageType,"");
			if(aStorageProvider==null)
				throw (new System.Exception("PersistencyContext Cann't instadiate the "+StorageType+ " Provider"));
			if(Embedded&&aStorageProvider.AllowEmbeddedStorage())
				return EmbeddedPersistencyContext.NewStorage(OriginalStorage, StorageName, StorageLocation, StorageType,Embedded);
			else
				throw new System.Exception("The storage type  '"+StorageType +"' doesn't allow embedded storage.");

			return NewStorage(OriginalStorage, StorageName, StorageLocation, StorageType);
		}

		/// <MetaDataID>{494E6EE7-CB64-4C7B-8FF6-13BD339EC49E}</MetaDataID>
		public override PersistenceLayer.StorageSession NewStorage(PersistenceLayer.ObjectStorage OriginalStorage, string StorageName, string StorageLocation, string StorageType)
		{
			return new ClientStorageSession(ServerSidePersistencyContext.NewStorage(OriginalStorage, StorageName, StorageLocation, StorageType));
			 
		}
		/// <summary>This method creates a new object with type that defined from parameter Type in storage of the StorageSession Storage.</summary>
		/// <param name="Type">Define the type of object that will be creating.</param>
		/// <param name="Storage">Define the storage session with storage in which will be creating the object.</param>
		/// <MetaDataID>{225F74C8-BA43-4717-BC75-582F1CB74C83}</MetaDataID>
		public override object NewObject(System.Type Type, PersistenceLayer.StorageSession Storage,params object[] ctorParams)
		{
			if(!(Storage is OOAdvantech.PersistenceLayerRunTime.ClientStorageSession))
				return EmbeddedPersistencyContext.NewObject(Type, Storage,ctorParams);
			else
				Storage =(Storage as OOAdvantech.PersistenceLayerRunTime.ClientStorageSession).ServerSideStorageSession;
			string StorageSessionChannelUri=OOAdvantech.Remoting.RemotingServices.GetChannelUri(Storage);
			if(!RemotePersistencyContexts.Contains(StorageSessionChannelUri))
				RemotePersistencyContexts.Add(StorageSessionChannelUri,GetPersistencyContext(StorageSessionChannelUri));
			return (RemotePersistencyContexts[StorageSessionChannelUri]as PersistenceLayer.PersistencyContext) .NewObject(Type, Storage,ctorParams);
			
  
			return ServerSidePersistencyContext.NewObject(Type, ((ClientStorageSession) Storage).ServerSideStorageSession,ctorParams);
		}
	
		/// <MetaDataID>{D9C0BD9A-5AF4-4044-BEFE-3142357FF3FA}</MetaDataID>
		public override void DeleteObject(object thePersistentObject)
		{
			if(!Remoting.RemotingServices.IsOutOfProcess(thePersistentObject as MarshalByRefObject))
			{
				EmbeddedPersistencyContext.DeleteObject(thePersistentObject);
				return;
			}
			string ObjectChannelUri=OOAdvantech.Remoting.RemotingServices.GetChannelUri((MarshalByRefObject)thePersistentObject);
			if(!RemotePersistencyContexts.Contains(ObjectChannelUri))
				RemotePersistencyContexts.Add(ObjectChannelUri,GetPersistencyContext(ObjectChannelUri));
			(RemotePersistencyContexts[ObjectChannelUri]as PersistenceLayer.PersistencyContext).DeleteObject(thePersistentObject);
		}
		/// <MetaDataID>{A5AEFE86-5332-42C4-B367-2794D36D61F6}</MetaDataID>
		public override void CommitObjectState(object Object)
		{
			string ObjectChannelUri=OOAdvantech.Remoting.RemotingServices.GetChannelUri((MarshalByRefObject)Object);
			if(!RemotePersistencyContexts.Contains(ObjectChannelUri))
				RemotePersistencyContexts.Add(ObjectChannelUri,GetPersistencyContext(ObjectChannelUri));
			(RemotePersistencyContexts[ObjectChannelUri]as PersistenceLayer.PersistencyContext).CommitObjectState(Object);
		}
	
	

		/// <MetaDataID>{13B4EB4C-0249-417D-B00D-FDC256E81FDC}</MetaDataID>
		public override object NewObject(string ClassName, string ClassVersion, PersistenceLayer.StorageSession Storage,params object[] ctorParams)
		{
			if(Storage is OOAdvantech.PersistenceLayerRunTime.ClientStorageSession)
				Storage =(Storage as OOAdvantech.PersistenceLayerRunTime.ClientStorageSession).ServerSideStorageSession;

			string StorageSessionChannelUri=OOAdvantech.Remoting.RemotingServices.GetChannelUri(Storage);
			if(!RemotePersistencyContexts.Contains(StorageSessionChannelUri))
				RemotePersistencyContexts.Add(StorageSessionChannelUri,GetPersistencyContext(StorageSessionChannelUri));
			return (RemotePersistencyContexts[StorageSessionChannelUri]as PersistenceLayer.PersistencyContext) .NewObject(ClassName, ClassVersion, Storage,ctorParams);

		}
		/// <MetaDataID>{84D78B42-3D30-4FDA-8404-8457DDECEE62}</MetaDataID>
		public override void CommitObjectState(object Object, PersistenceLayer.StorageSession StorageSession)
		{



			if(!(StorageSession is OOAdvantech.PersistenceLayerRunTime.ClientStorageSession))
			{
				EmbeddedPersistencyContext.CommitObjectState(Object, StorageSession);
				return ;
			}
			else
				StorageSession =(StorageSession as OOAdvantech.PersistenceLayerRunTime.ClientStorageSession).ServerSideStorageSession;
			string StorageSessionChannelUri=OOAdvantech.Remoting.RemotingServices.GetChannelUri(StorageSession);
			if(!RemotePersistencyContexts.Contains(StorageSessionChannelUri))
				RemotePersistencyContexts.Add(StorageSessionChannelUri,GetPersistencyContext(StorageSessionChannelUri));
			(RemotePersistencyContexts[StorageSessionChannelUri]as PersistenceLayer.PersistencyContext).CommitObjectState(Object, StorageSession);


/*
			if(!Remoting.RemotingServices.IsOutOfProcess(Object as MarshalByRefObject))
			{
				EmbeddedPersistencyContext.CommitObjectState(Object, StorageSession, TransactionType);
				return;
			}
			else
			{
				if(Remoting.RemotingServices.GetChannelUri(Object as MarshalByRefObject)!=Remoting.RemotingServices.GetChannelUri(StorageSession))
					throw new System.Exception("The object and storage are instantieded in different processes");
			}
			ServerSidePersistencyContext.CommitObjectState(Object, StorageSession, TransactionType);
			*/
		
		}
		
	}
}
