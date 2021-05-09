using System;
namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{6C0BF911-0F7A-49FA-9A17-83B1345D9D0F}</MetaDataID>
    [Serializable]
    public class ObjectLinksDataSource:DataSource
	{
		/// <MetaDataID>{F1A3C8B1-9DB7-4C74-B718-84BC6D802075}</MetaDataID>
		public OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink ObjectsLinks;
		/// <MetaDataID>{95AB798E-181C-4192-A3EF-FAEE68284551}</MetaDataID>
		public ObjectLinksDataSource(MetaDataRepository.Association association, MetaDataRepository.MetaObjectCollection  objectsLinks)
		{
			RDBMSMetaDataRepository.AssociationEnd assEnd=association.RoleA as RDBMSMetaDataRepository.AssociationEnd;
			
			if(association.LinkClass!=null)
			{
				Collections.Generic.Set<MetaDataRepository.StorageCell>  storageCells=new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
				foreach(RDBMSMetaDataRepository.StorageCellsLink objectsLink in objectsLinks )
					storageCells.AddCollection(objectsLink.AssotiationClassStorageCells);
				View=(association.LinkClass as RDBMSMetaDataRepository.MappedClassifier).GetTypeView(storageCells);
			}
			else
			{
				if(objectsLinks!=null&&objectsLinks.Count>0 )
				{
					View=new OOAdvantech.RDBMSMetaDataRepository.View("Temporary");
					foreach(RDBMSMetaDataRepository.StorageCellsLink objectsLink in objectsLinks )
					{
						if(View.ViewColumns==null||View.ViewColumns.Count==0)
						{
							foreach(RDBMSMetaDataRepository.Column column in objectsLink.ObjectLinksTable.ContainedColumns)
								View.AddColumn(new RDBMSMetaDataRepository.Column(column));
						}
						OOAdvantech.RDBMSMetaDataRepository.View view=new OOAdvantech.RDBMSMetaDataRepository.View(objectsLink.ObjectLinksTable.Name,objectsLink.ObjectLinksTable.Namespace);
						view.AddJoinedTable(objectsLink.ObjectLinksTable);
						View.AddSubView(view);
					}
				}
				else
				{
					View=new OOAdvantech.RDBMSMetaDataRepository.View("Temporary");
					foreach(RDBMSMetaDataRepository.Column column in  (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
						View.AddColumn(column);
					foreach(RDBMSMetaDataRepository.Column column in (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns())
						View.AddColumn(column);
				}
			}
		}
	}
}
