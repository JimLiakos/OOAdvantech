using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{a477e7ec-92a9-4754-8c7c-f464921d3c79}</MetaDataID>
    public class NamespaceTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{e6208522-d181-4cbe-9740-c19c4376b049}</MetaDataID>
        Namespace Namespace;
        /// <MetaDataID>{16294c85-9489-48a6-9ab5-fd4b6c6b7ea2}</MetaDataID>
        Component ImplamentationUnit;
        /// <MetaDataID>{34255a71-fc13-4338-a7e2-18f6c456a396}</MetaDataID>
        public NamespaceTreeNode(Namespace _namespace, Component implamentationUnit,MetaObjectTreeNode parent)
            : base(_namespace,parent)
        {
            Namespace = _namespace;
            ImplamentationUnit = implamentationUnit;
        }

        /// <MetaDataID>{04bb1d56-7ce7-41aa-b215-ced4d1c45f7f}</MetaDataID>
        bool HasImplementationUnitContainedMetaObject(Namespace _namespace)
        {
            foreach (MetaObject metaObject in _namespace.OwnedElements)
            {
                if ((metaObject is Classifier) && (metaObject as Classifier).ImplementationUnit == ImplamentationUnit)
                    return true;

                if (!(metaObject is Classifier) && (metaObject is Namespace) && HasImplementationUnitContainedMetaObject(metaObject as Namespace))
                    return true;
            }
            return false;

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
        /// <MetaDataID>{0824dd79-7474-4e9c-b151-bd1e243153a4}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{6af3e8f1-9809-47cf-bc40-4add75d1f06c}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {

                if (_LazyLoad)
                {
                    if (_ContainedObjects.Count == 0)
                        _ContainedObjects.Add(MetaObject, new NodesLoader(this.MetaObject, this));

                    return _ContainedObjects.Values.ToList();

                }

                List<MetaObject> removedObjects = new List<MetaObject>();
                removedObjects.AddRange(_ContainedObjects.Keys);

                foreach (MetaObject metaObject in Namespace.OwnedElements)
                {
                    if (removedObjects.Contains(metaObject))
                        removedObjects.Remove(metaObject);
                    if (!_ContainedObjects.ContainsKey(metaObject))
                    {
                        if (metaObject is OOAdvantech.MetaDataRepository.Classifier && (metaObject as OOAdvantech.MetaDataRepository.Classifier).TemplateBinding != null && (metaObject as OOAdvantech.MetaDataRepository.Classifier).OwnedTemplateSignature == null)
                            continue;
                        if (metaObject is Classifier && metaObject.ImplementationUnit == ImplamentationUnit)
                            _ContainedObjects.Add(metaObject, new ClassifierTreeNode(metaObject as Classifier,this));
                        else
                            if (metaObject is Namespace && !(metaObject is Classifier) && HasImplementationUnitContainedMetaObject(metaObject as Namespace))
                            {
                                _ContainedObjects.Add(metaObject, new NamespaceTreeNode(metaObject as Namespace, ImplamentationUnit,this));
                            }

                    }
                }
                foreach (MetaObject metaObject in removedObjects)
                    _ContainedObjects.Remove(metaObject);

                List<MetaObjectTreeNode> items = new List<MetaObjectTreeNode>(_ContainedObjects.Values);
                items.Sort(new MetaObjectsSort());
                return items;
                //return new List<MetaObjectTreeNode>(_ContainedObjects.Values);
            }
        }
        /// <MetaDataID>{c896b8eb-9309-4bb4-adda-8ba400fa25ed}</MetaDataID>
        Bitmap _Image;
        /// <MetaDataID>{2db4acd8-e080-4d14-a8b2-52360b82ec85}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSObject_Namespace;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }


    }

}
