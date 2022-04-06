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
            System.Collections.Generic.List<IndexChange> indexesOfNewRelatedObject = null;
            using (CultureContext cultureContext = new CultureContext(Culture, false))
            {
                groupIndexChanges = Collection.GetIndexChanges(Transactions.Transaction.Current.LocalTransactionUri);
                indexesOfNewRelatedObject = Collection.GetTheIndexesOfAdditionalObject(Transactions.Transaction.Current.LocalTransactionUri);
            }
            XElement objRefCollection = null;
            if (groupIndexChanges.Count > 0 || indexesOfNewRelatedObject.Count > 0)
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

                objRefCollection = ownerStorageInstance.Element(_roleName);
                if (Collection.RelResolver.Multilingual)
                    objRefCollection = objRefCollection.Element(Culture.Name);
            }

            if (groupIndexChanges.Count > 0)
            {

                foreach (XElement inElement in objRefCollection.Elements().OrderBy(x => GetIndex(x)))
                {

                    IndexChange indexChangeForNewObjectLink = indexesOfNewRelatedObject.Where(x => x.CollectionItem?.PersistentObjectID.ToString() == inElement.Value).FirstOrDefault();

                    if (indexChangeForNewObjectLink != null && indexChangeForNewObjectLink.OldIndex == -1)
                    {
                        inElement.SetAttribute("Sort", indexChangeForNewObjectLink.NewIndex.ToString());
                        (Collection.RelResolver.Owner.ObjectStorage as MetaDataStorageSession).Dirty = true;
                    }
                    else
                    {
                        var index = GetIndex(inElement);
                        var indexChange = groupIndexChanges.Where(x => index >= x.StartIndex && index <= x.EndIndex).FirstOrDefault();
                        if (indexChange != null)
                        {
                            inElement.SetAttribute("Sort", (index + indexChange.Change).ToString());
                            (Collection.RelResolver.Owner.ObjectStorage as MetaDataStorageSession).Dirty = true;
                        }
                    }
                }


                if (objRefCollection.Elements().Select(x => GetIndex(x)).Distinct().Count() != objRefCollection.Elements().Count())
                {
                    int i = 0;

                }

                using (CultureContext cultureContext = new CultureContext(Culture, false))
                {
                    Collection.IndexRebuilded(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
                }
                (Collection.RelResolver.Owner.ObjectStorage as MetaDataStorageSession).Dirty = true;
            }


            //foreach (var indexOfNewRelatedObject in indexesOfNewRelatedObject)
            //{
            //    XElement inElement = objRefCollection.Elements().Where(x => x.Value == indexOfNewRelatedObject.CollectionItem.PersistentObjectID.ToString()).FirstOrDefault();
            //    inElement.SetAttribute("Sort", indexOfNewRelatedObject.NewIndex.ToString());
            //    (Collection.RelResolver.Owner.ObjectStorage as MetaDataStorageSession).Dirty = true;
            //}

            if (objRefCollection != null && objRefCollection.Elements().Select(x => GetIndex(x)).Distinct().Count() != objRefCollection.Elements().Count())
            {
                int i = 0;
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