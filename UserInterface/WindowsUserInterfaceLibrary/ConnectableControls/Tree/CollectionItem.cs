using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Collections.Generic;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{6fc7da05-1328-45b9-9baa-21efa70d95c3}</MetaDataID>
    public class TreeItem
    {
        public virtual string Name
        {
            get
            {
                return ItemPath;
            }
        }

        public TreeItem(string name)
        {
            ItemPath = name;
        }

        #region public string ItemPath
        private string _path = "";
        public string ItemPath
        {
            get { return _path; }
            set { _path = value; }
        }
        #endregion

        #region public TreeItem Parent;
        private TreeItem _parent;
        public TreeItem Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        #endregion
    }

    /// <MetaDataID>{1b04e6b8-e22f-48fa-9eb1-ddafc2a07d35}</MetaDataID>
    public class ObjectItem : TreeItem
    {
        private Object _iobject;
        public Object Object
        {
            get
            {
                return _iobject;
            }
        }

        public ObjectItem(object obj, string name, TreeItem parent)
            :base(name)
        {            
            Parent = parent;
            _iobject = obj ;
        }
    }

    /// <MetaDataID>{68b373da-20f3-4238-a2dd-0cd75f01dc15}</MetaDataID>
    public class CollectionItem : TreeItem
    {
        private Set<object> _collection;
        public Set<object> collection
        {
            get
            {
                return _collection;
            }
        }

        public CollectionItem(string name, Set<object> set_collection)
            :base(name)
		{			
            _collection = set_collection;
		}		
    }
}
