using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{15ae82d5-cb4a-4339-82cb-ac3df504fa34}</MetaDataID>
    public class ComponentTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{67e9b319-ea7c-4886-a953-4a765ca37572}</MetaDataID>
        Component Component;
        /// <MetaDataID>{483c2112-c1e9-4659-83f2-0ef14f375744}</MetaDataID>
        public ComponentTreeNode(Component component, MetaObjectTreeNode parent)
            : base(component,parent)
        {
            Component = component; 
        }
        bool _LazyLoad = true;
        internal override void LazyLoad()
        {
            if (_LazyLoad)
            {

                _LazyLoad = false;
                _ContainedObjects.Clear();
                base.OnMetaObjectChanged(null);
            }
        }
        /// <MetaDataID>{49faa469-8940-4fb8-b38f-442e7cf715c4}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{6f2d58d9-1e89-413c-9a54-85b37b8fdcf2}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                if(_LazyLoad)
                {
                    if (_ContainedObjects.Count == 0)
                        _ContainedObjects.Add(MetaObject, new NodesLoader(this.MetaObject, this));

                        return _ContainedObjects.Values.ToList();

                }
                List<MetaObject> removedObjects = new List<MetaObject>();
                removedObjects.AddRange(_ContainedObjects.Keys);
                if (!_ContainedObjects.ContainsKey(Component))
                    _ContainedObjects.Add(Component, new ReferencesFolderTreeNode(Component,this));
                else
                    removedObjects.Remove(Component);

                foreach (MetaObject metaObject in Component.Residents)
                {
                    if (!(metaObject is Classifier))
                        continue;
                    if (metaObject is OOAdvantech.MetaDataRepository.Classifier && (metaObject as OOAdvantech.MetaDataRepository.Classifier).TemplateBinding != null && (metaObject as OOAdvantech.MetaDataRepository.Classifier).OwnedTemplateSignature == null)
                        continue;
                    if (!_ContainedObjects.ContainsKey(metaObject))
                    {
                        Namespace _namespace = metaObject.Namespace;
                        if (_namespace != null)
                        {
                            while (_namespace.Namespace != null)
                                _namespace = _namespace.Namespace;
                            removedObjects.Remove(_namespace);
                            if (!_ContainedObjects.ContainsKey(_namespace))
                                _ContainedObjects.Add(_namespace, new NamespaceTreeNode(_namespace, Component,this));
                        }
                        else
                        {
                            removedObjects.Remove(metaObject);
                            if (!_ContainedObjects.ContainsKey(metaObject))
                                _ContainedObjects.Add(metaObject, new ClassifierTreeNode(metaObject as Classifier,this));
                        }
                    }
                    else
                    {
                           Namespace _namespace = metaObject.Namespace;
                           if (_namespace != null)
                           {
                               while (_namespace.Namespace != null)
                                   _namespace = _namespace.Namespace;
                               removedObjects.Remove(_namespace);
                               if (!_ContainedObjects.ContainsKey(_namespace))
                                   _ContainedObjects.Add(_namespace, new NamespaceTreeNode(_namespace, Component,this));
                           }
                     
                     
                        if(metaObject.Namespace==null)
                            removedObjects.Remove(metaObject);
                    }
                }
                foreach (MetaObject metaObject in removedObjects)
                    _ContainedObjects.Remove(metaObject);


                List<MetaObjectTreeNode> items = new List<MetaObjectTreeNode>(_ContainedObjects.Values);
                items.Sort(new MetaObjectsSort());
                items.Remove(_ContainedObjects[Component]);
                items.Insert(0, _ContainedObjects[Component]);
                return items;
            }
        }
    }


    /// <MetaDataID>{3530c10e-7e85-4636-bce1-3e70828c8113}</MetaDataID>
    class MetaObjectsSort : IComparer<MetaObjectTreeNode>
    {




        public int Compare(MetaObjectTreeNode x, MetaObjectTreeNode y)
        {
            if(x==null&&y==null)
                return 0;
            if(x!=null&&y==null)
                return x.Name.CompareTo("");
            if(x==null&&y!=null)
                return "".CompareTo(y.Name);
            return x.Name.CompareTo(y.Name);
        }

        
    }
}
