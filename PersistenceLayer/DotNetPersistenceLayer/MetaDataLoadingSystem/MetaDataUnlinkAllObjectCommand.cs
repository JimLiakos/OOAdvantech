using System.Xml.Linq;
using System.Linq;
namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
	/// <MetaDataID>{C834AC9E-7373-4387-8F1B-B8F79D98832F}</MetaDataID>
	public class MetaDataUnlinkAllObjectCommand :PersistenceLayerRunTime.Commands.OnMemoryUnlinkAllObjectCommand
	{
        public MetaDataUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent deletedStorageInstance)
            : base(deletedStorageInstance)
        {
        }
		/// <MetaDataID>{7253C8EB-6753-479C-99BD-0D22DB1BC14B}</MetaDataID>
		protected override PersistenceLayerRunTime.RelResolver theResolver
		{
			get
			{ 

                foreach (PersistenceLayerRunTime.RelResolver relResolcer in DeletedStorageInstance.RealStorageInstanceRef.RelResolvers)
                {

                    if (relResolcer.AssociationEnd == theAssociationEnd && relResolcer.ValueTypePath == DeletedStorageInstance.ValueTypePath)
                        return relResolcer;
                }
                throw new System.Exception("System can't find the relation resolver for association end '" + theAssociationEnd.Name);
            }
		}
	

		/// <MetaDataID>{48D8D403-FBFE-4751-A205-5F45C4060E5F}</MetaDataID>
		private void RemoveLinkFrom(PersistenceLayerRunTime.StorageInstanceRef theStorageInstanceRef)
		{

			MetaDataStorageSession ObjectStorageSession=(MetaDataStorageSession)theStorageInstanceRef.ObjectStorage;
			XDocument XmlDocument=ObjectStorageSession.XMLDocument;
			string StorageName=XmlDocument.Root.Name.LocalName;
            XElement StorageInstanceNode = ObjectStorageSession.GetXMLElement(theStorageInstanceRef.MemoryInstance.GetType(), (ObjectID)theStorageInstanceRef.PersistentObjectID);
			ObjectStorageSession.NodeChangedUnderTransaction(StorageInstanceNode,OwnerTransactiont);
			StorageInstanceNode.SetAttribute("ReferentialIntegrityCount",theStorageInstanceRef.ReferentialIntegrityCount.ToString());
			string StrObjectID=DeletedStorageInstance.PersistentObjectID.ToString();

            string RoleName = ObjectStorageSession.GetMappedTagName(theResolver.AssociationEnd.GetOtherEnd().Identity.ToString().ToLower());
            if (RoleName == null)
            {
                RoleName = theResolver.AssociationEnd.GetOtherEnd().Name;
                if (string.IsNullOrEmpty(RoleName))
                {
                    if (theResolver.AssociationEnd.GetOtherEnd().IsRoleA)
                        RoleName = theResolver.AssociationEnd.Association.Name+ "RoleAName";
                    else
                        RoleName = theResolver.AssociationEnd.Association.Name + "RoleBName";
                }
                ObjectStorageSession.SetMappedTagName(theResolver.AssociationEnd.GetOtherEnd().Identity.ToString().ToLower(), RoleName);
            }

			foreach(XElement CurrNode in  StorageInstanceNode.Elements())
			{
				XElement Element=CurrNode;
                //string RoleName=theResolver.AssociationEnd.GetOtherEnd().Name;
                //if(RoleName==null||RoleName.Trim().Length==0)
                //    if(theResolver.AssociationEnd.IsRoleA)
                //        RoleName=theResolver.AssociationEnd.Association.Name+"RoleAName";
                //    else
                //        RoleName=theResolver.AssociationEnd.Association.Name+"RoleBName";

				/*string RoleName;
				if(theResolver.AssociationEnd.IsRoleA)
					RoleName=((MetaDataRelResolver)theResolver).RoleBName;
				else
					RoleName=((MetaDataRelResolver)theResolver).RoleAName;*/
				if(Element.Name==RoleName)
				{
					if (theResolver.Multilingual)
					{
						XElement linkCollectionNode = Element.Element(CultureContext.CurrentNeutralCultureInfo.Name);
						if(linkCollectionNode!=null)
						{
							foreach (XElement inCurrNode in linkCollectionNode.Elements())
							{
								XElement inElement = (XElement)inCurrNode;
								if (inElement.Value == StrObjectID)
								//if(inElement.GetAttribute("oid")==StrObjectID)
								{
									inElement.Remove();

									break;
								}
							}
						}
					}
					else
					{
						XElement LinkCollectionNode = Element;
						foreach (XElement inCurrNode in LinkCollectionNode.Descendants("oid"))
						{
							XElement inElement = (XElement)inCurrNode;
							if (inElement.Value == StrObjectID)
							//if(inElement.GetAttribute("oid")==StrObjectID)
							{
								inElement.Remove();

								break;
							}
						}
					}
				}
			}
		}
		/// <MetaDataID>{CB3E29A0-D2AA-47EA-B383-AA6D68C7ABAB}</MetaDataID>
		public override void Execute()
		{
			
			base.Execute();
			if (theResolver.Multilingual)
			{
				foreach (PersistenceLayerRunTime.MultilingualObjectLink multilingualObjectLink in theResolver.GetLinkedStorageInstanceRefsUnderTransaction(false).OfType<PersistenceLayerRunTime.MultilingualObjectLink>())
				{
					using (CultureContext cultureContext =new CultureContext(multilingualObjectLink.Culture,false) )
					{
						PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = multilingualObjectLink.LinkedObject as PersistenceLayerRunTime.StorageInstanceRef;
						RemoveLinkFrom(storageInstanceRef);
					}
				}
			}
			else
			{
				foreach (PersistenceLayerRunTime.StorageInstanceRef CurrStorageInstanceRef in theResolver.GetLinkedStorageInstanceRefsUnderTransaction(false))
					RemoveLinkFrom(CurrStorageInstanceRef);
			}

			
			//MetaDataRelResolver mResolver=(MetaDataRelResolver)theResolver;
			MetaDataStorageSession ObjectStorageSession=(MetaDataStorageSession)DeletedStorageInstance.ObjectStorage;

			ObjectStorageSession.Dirty=true;

			XDocument XmlDocument=ObjectStorageSession.XMLDocument;
			string StorageName=XmlDocument.Root.Name.LocalName;
            XElement StorageInstanceNode = ObjectStorageSession.GetXMLElement(DeletedStorageInstance.MemoryInstance.GetType(), (ObjectID)DeletedStorageInstance.PersistentObjectID);
			ObjectStorageSession.NodeChangedUnderTransaction(StorageInstanceNode,OwnerTransactiont);
			StorageInstanceNode.SetAttribute("ReferentialIntegrityCount",DeletedStorageInstance.ReferentialIntegrityCount.ToString());
			foreach(XElement CurrNode in  StorageInstanceNode.Elements())
			{
				XElement Element=(XElement)CurrNode;
				string RoleName=theResolver.AssociationEnd.Name;
				if(RoleName==null||RoleName.Trim().Length==0)
					if(theResolver.AssociationEnd.IsRoleA)
						RoleName=theResolver.AssociationEnd.Association.Name+"RoleAName";
					else
						RoleName=theResolver.AssociationEnd.Association.Name+"RoleBName";

				if(Element.Name==RoleName)
				{
                    Element.Remove();
				}
			}

		}
	}
}
