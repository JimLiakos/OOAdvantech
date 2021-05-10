using OOAdvantech.PersistenceLayerRunTime;
using System.Linq;
using System.Xml.Linq;

namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{2a3a8518-06d9-453e-86e5-c8d647e7cc16}</MetaDataID>
    internal class BuildContainedObjectIndicies : PersistenceLayerRunTime.Commands.BuildContainedObjectIndicies
    {

        public BuildContainedObjectIndicies(IndexedCollection collection) : base(collection)
        {
            
        }

        public override void Execute()
        {

            System.Collections.Generic.List<GroupIndexChange> groupIndexChanges = null;
            using (CultureContext cultureContext = new CultureContext(Culture, false))
            {
                groupIndexChanges=Collection.GetIndexChanges(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
            }


            if (groupIndexChanges.Count > 0)
            {
                var ownerStorageInstance = (Collection.RelResolver.Owner.ObjectStorage as MetaDataStorageSession).GetXMLElement(Collection.RelResolver.Owner.MemoryInstance.GetType(), (ObjectID)Collection.RelResolver.Owner.PersistentObjectID);

                StorageInstanceRef collectionOwner = Collection.RelResolver.Owner as StorageInstanceRef;

                #region gets role name 

                var _roleName = (Collection.RelResolver.Owner.ObjectStorage as MetaDataStorageSession).GetMappedTagName(Collection.RelResolver.AssociationEnd.Identity.ToString().ToLower());
                if (string.IsNullOrWhiteSpace(_roleName))
                {
                    _roleName = Collection.RelResolver.AssociationEnd.Name;
                    if (string.IsNullOrWhiteSpace(_roleName))
                    {
                        if (Collection.RelResolver.AssociationEnd.IsRoleA)
                            _roleName = Collection.RelResolver.AssociationEnd.Association.Name + "RoleAName";
                        else
                            _roleName = Collection.RelResolver.AssociationEnd.Association.Name + "RoleBName";
                    }

                }
                #endregion

                var objRefCollection = ownerStorageInstance.Element(_roleName);
                if (Collection.RelResolver.Multilingual)
                    objRefCollection = objRefCollection.Element(Culture.Name);

                foreach (XElement inElement in objRefCollection.Elements().OrderBy(x => GetIndex(x)))
                {
                    var index = GetIndex(inElement);
                    var indexChange = groupIndexChanges.Where(x => index >= x.StartIndex && index <= x.EndIndex).FirstOrDefault();
                    if(indexChange!=null)
                        inElement.SetAttribute("Sort", (index + indexChange.Change).ToString());
                }
            }

        }
        private static int GetIndex(XElement indexElement)
        {
            string indexStr = indexElement.GetAttribute("Sort");
            int index = 0;
            if (int.TryParse(indexStr, out index))
                return index;
            else
                return 0;

        }
    }
}