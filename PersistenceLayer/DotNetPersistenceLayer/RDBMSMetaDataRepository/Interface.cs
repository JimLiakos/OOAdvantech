namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{AB25D958-A397-4F27-89B2-57E30B93D09D}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{AB25D958-A397-4F27-89B2-57E30B93D09D}")]
	[MetaDataRepository.Persistent()]
	public class Interface : OOAdvantech.MetaDataRepository.Interface,MappedClassifier	

	{

		MetaDataRepository.MetaObjectCollection MappedClassifier.StorageCells
		{
			get
			{
				MetaDataRepository.MetaObjectCollection StorageCells=new OOAdvantech.MetaDataRepository.MetaObjectCollection();
				foreach(MetaDataRepository.Realization realization in Realizations)
				{
					if(realization.Implementor.Persistent)
					{
						StorageCells.AddCollection((realization.Implementor as RDBMSMetaDataRepository.RDBMSMappingClass).StorageCellsOfThisType);
					}
				}
				return StorageCells;
			}
		}

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{DD8D9A0E-7646-46AC-B868-68FF399949F6}</MetaDataID>
		private RDBMSView _InterfaceView;
		/// <MetaDataID>{C632FD7E-3A57-45DB-9D56-D1B09B6BA33A}</MetaDataID>
		[MetaDataRepository.BackwardCompatibilityID("+1")]
		[MetaDataRepository.Association("InterfaceView",typeof(OOAdvantech.RDBMSMetaDataRepository.RDBMSView),MetaDataRepository.Roles.RoleA)]
		[MetaDataRepository.AssociationEndBehavior(MetaDataRepository.PersistencyFlag.OnConstruction)]
		[MetaDataRepository.PersistentMember("_InterfaceView")]
		[MetaDataRepository.RoleAMultiplicityRange(1,1)]
		[MetaDataRepository.RoleBMultiplicityRange(0)]
		public RDBMSView InterfaceView
		{
			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					if(_InterfaceView==null)
					{
						_InterfaceView=new RDBMSView();
						if(Name==null)
							throw new System.Exception("Build Error :You must set the OutStorageObjectCollection Name");
						_InterfaceView.Name="Abstract_"+Name; 
						PersistenceLayer.StorageSession.GetStorageOfObject(Properties).CommitTransientObjectState(_InterfaceView);
					}
					else
					{
						if(_InterfaceView.Name!=("Abstract_"+Name))
						{
							_InterfaceView.Name="Abstract_"+Name; 
							OOAdvantech.PersistenceLayer.StorageSession.CommitObjectState(_InterfaceView);
						}
					}
					return _InterfaceView;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			
			}
		}

		/// <MetaDataID>{7C1E62D6-9090-406C-801E-8CFE1DBB3147}</MetaDataID>
		public void BuildMappingElement(PersistenceLayer.ObjectStorage hostObjectStorage  )
		{
			
			
			
			foreach(MetaDataRepository.Attribute attribute in GetAttributes(true))
			{
				if(attribute.Type is MetaDataRepository.Primitive)
				{
					RDBMSColumn newColumnAlias=new RDBMSColumn();
					newColumnAlias.Name=attribute.Name;
					newColumnAlias.Type=attribute.Type;
					PersistenceLayer.StorageSession.GetStorageOfObject(Properties).CommitTransientObjectState(newColumnAlias);
					InterfaceView.AddColumn(newColumnAlias);
				}
			}

		}
		#region MappedClassifier Members

		public RDBMSView TypeView
		{
			get
			{
				return InterfaceView;
			}
		
		}
		public MetaDataRepository.MetaObjectCollection GetStorageCells(System.DateTime TimePeriodStartDate, System.DateTime TimePeriodEndDate)
		{
			return new MetaDataRepository.MetaObjectCollection();
		}

		public MetaDataRepository.MetaObjectCollection ObjectIDColumns
		{
			get
			{
				//TODO Θα πρέπει να διασφαλιστή ότι όλη class ιεραρχία έχει κοινό ObjectID Format
				foreach (MetaDataRepository.Realization realization in Realizations)
					return (realization.Implementor as RDBMSMappingClass).ActiveStorageCell.MainTable.ObjectIDColumns;

				return null;
			}
		}
		public RDBMSView GetAbstracClasstView(MetaDataRepository.MetaObjectCollection storageCells,RDBMSMappingClass concreteClass)
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
				RDBMSView mView=new RDBMSView();
				foreach(RDBMSColumn CurrColumn in InterfaceView.ViewColumns)
					mView.AddColumn(CurrColumn);
				foreach(RDBMSObjectCollection CurrStorageCell in storageCells)
				{
					if(concreteClass!=null)
						if(CurrStorageCell.Type!=concreteClass)
							continue;
					if(CurrStorageCell is OutStorageObjectCollection)
					{
						try
						{
							OutStorageObjectCollection OutStorageCell=CurrStorageCell as OutStorageObjectCollection;
							//RDBMSObjectCollection StorageCell=OutStorageCell.RealStorageCell;
							mView.AddSubView(OutStorageCell.ConcreteClassView);
						}
						catch(System.Exception Error)
						{
							int k=0;
						}
					}
					else
						mView.AddSubView(CurrStorageCell.ConcreteClassView);
				}
				return mView;
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock();
			}
		}


		#endregion
	}
}
