using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{1ebce1dd-6c60-40ed-8fe3-d8146c5091fd}</MetaDataID>
    public class ReferencesFolderTreeNode : MetaObjectTreeNode
    {

        /// <MetaDataID>{6628c11b-7665-430e-a51e-a0301f874067}</MetaDataID>
        protected override void OnMetaObjectChanged(object sender)
        {
            Dictionary<MetaObject, MetaObjectTreeNode> containedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>(_ContainedObjects);
            if (containedObjects.Count != ContainedObjects.Count)
            {
                base.OnMetaObjectChanged(sender);
                return;
            }
            foreach (System.Collections.Generic.KeyValuePair<MetaObject, MetaObjectTreeNode> entry in containedObjects)
            {
                if (!_ContainedObjects.ContainsKey(entry.Key))
                {
                    base.OnMetaObjectChanged(sender);
                    return;
                }
            }
        }

        /// <MetaDataID>{c6a3fd7d-28b2-438a-8b2e-49fd94d0259a}</MetaDataID>
         OOAdvantech.MetaDataRepository.Component Component;

        /// <MetaDataID>{d4a28f33-a0e3-4191-94c8-36dc2f21277b}</MetaDataID>
         public ReferencesFolderTreeNode(OOAdvantech.MetaDataRepository.Component component, MetaObjectTreeNode parent)
             : base(component,parent)
        {
            Component = component;

        }
        /// <MetaDataID>{170864fe-5fe0-4bcc-bf4d-5b933e312199}</MetaDataID>
        public override string Name
        {
            get
            {
                return "References";
            }
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

        /// <MetaDataID>{ecac875f-f23a-4052-aeee-9749e07d0fb3}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{ebb4cb2a-8101-4ce5-a526-651401019ee6}</MetaDataID>
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


                foreach (Dependency dependency in Component.ClientDependencies)
                {
                    removedObjects.Remove(dependency);
                    if (!_ContainedObjects.ContainsKey(dependency))
                        _ContainedObjects.Add(dependency, new DependencyTreeNode(dependency,this));

                }

           
                foreach (MetaObject metaObject in removedObjects)
                    _ContainedObjects.Remove(metaObject);

                return new List<MetaObjectTreeNode>(_ContainedObjects.Values);
            }
        }
        /// <MetaDataID>{75dcc7d4-fc08-4e03-a82e-a114759c675d}</MetaDataID>
        Bitmap _Image;
        /// <MetaDataID>{84198fd2-92b1-4285-ba10-763b3fc212b3}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (_Image == null)
                {

                    _Image = Resources.VSFolder_closed;
                    _Image.MakeTransparent(Color.FromArgb(255, 0, 255));
                }
                return _Image;
            }
        }


    }

}
