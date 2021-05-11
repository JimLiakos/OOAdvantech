using ConnectableControls.Tree;
using System.ComponentModel;
using System;
using OOAdvantech.Collections.Generic;
using System.Threading;
using System.Collections.Generic;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{CA58777C-A885-4046-89DD-A500CD53E1E2}</MetaDataID>
    public class ObjectTreeModel : ITreeModel
    {
        
        /// <MetaDataID>{A265D98A-6D59-4274-8A30-B2A4D88286A9}</MetaDataID>
        private Set<object>[] BaseNodes;
        //private List<TreeItem> _itemsToRead;
        /// <MetaDataID>{A78E1D4D-B20A-4589-B540-E51C2A8BBA35}</MetaDataID>
        public void ReadFilesProperties(object sender, DoWorkEventArgs e)
        {
            
        }

        /// <MetaDataID>{E142AE85-6A6F-4720-BF46-3D437EB80B0A}</MetaDataID>
        public ObjectTreeModel(Set<object>[] collections)
        {
            BaseNodes = collections;
            //_itemsToRead = new List<TreeItem>();
            //Worker = new BackgroundWorker();
            //Worker.DoWork += new DoWorkEventHandler(ReadFilesProperties);
        }
       
        /// <MetaDataID>{85E71929-70D7-4885-B54F-CA739C8618CC}</MetaDataID>
        private BackgroundWorker Worker;

        #region ITreeModel Members

        /// <MetaDataID>{DB03A3A1-703E-4AB8-A0AB-E41A807897B3}</MetaDataID>
        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {            
            if (treePath.IsEmpty())
                foreach (Set<object> set_collection in BaseNodes)
                {
                    CollectionItem Collectionitem = new CollectionItem(set_collection.ToString(), set_collection);
                    yield return Collectionitem;
                }
            else
            {
                //List<TreeItem> items = new List<TreeItem>();
                CollectionItem parent = treePath.LastNode as CollectionItem;
                if (parent != null)
                {
                    foreach (Object obj in parent.collection)
                    {
                        ObjectItem oi=new ObjectItem(obj,obj.ToString(), parent);
                        yield return oi;
                        //items.Add(new );    
                        NodesChanged(this, new TreeModelEventArgs(treePath, new object[] { oi }));                
                    }
                }
                else
                    yield break;
            }          
        }

        /// <MetaDataID>{4C41BFAA-12ED-464B-8D98-7C6240EC67C9}</MetaDataID>
        public bool IsLeaf(TreePath treePath)
        {
            return treePath.LastNode is ObjectItem;
        }

        public event System.EventHandler<TreeModelEventArgs> NodesChanged;
        public event System.EventHandler<TreeModelEventArgs> NodesInserted;
        public event System.EventHandler<TreeModelEventArgs> NodesRemoved;
        public event System.EventHandler<TreePathEventArgs> StructureChanged;

        #endregion
    }
}
