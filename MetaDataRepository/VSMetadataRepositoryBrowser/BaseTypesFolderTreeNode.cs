using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{1ebce1dd-6c60-40ed-8fe3-d8146c5091fd}</MetaDataID>
    public class BaseTypesFolderTreeNode : MetaObjectTreeNode
    {
        /// <MetaDataID>{244bbad5-a2d2-490b-b177-d148eb3de8ea}</MetaDataID>
        Classifier Classifier;

        /// <MetaDataID>{d4a28f33-a0e3-4191-94c8-36dc2f21277b}</MetaDataID>
        public BaseTypesFolderTreeNode(Classifier classifier, MetaObjectTreeNode parent)
            : base(classifier,parent)
        {
            Classifier = classifier;

        }
        /// <MetaDataID>{170864fe-5fe0-4bcc-bf4d-5b933e312199}</MetaDataID>
        public override string Name
        {
            get
            {
                return "Base Types";
            }
        }

        /// <MetaDataID>{213ba84a-d88a-4855-8ba0-d143b7469e49}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{ebb4cb2a-8101-4ce5-a526-651401019ee6}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                List<MetaObject> removedObjects = new List<MetaObject>();
                removedObjects.AddRange(_ContainedObjects.Keys);

                foreach (Generalization generalization in Classifier.Generalizations)
                {
                    if (generalization.Parent != null)
                    {
                        removedObjects.Remove(generalization.Parent);
                        if (!_ContainedObjects.ContainsKey(generalization.Parent))
                            _ContainedObjects.Add(generalization.Parent, new ClassifierTreeNode(generalization.Parent,this));
                    }

                }
                if (Classifier is InterfaceImplementor)
                {
                    foreach (Realization realization in (Classifier as InterfaceImplementor).Realizations)
                    {
                        if (realization.Abstarction != null)
                        {
                            removedObjects.Remove(realization.Abstarction);
                            if (!_ContainedObjects.ContainsKey(realization.Abstarction))
                                _ContainedObjects.Add(realization.Abstarction, new ClassifierTreeNode(realization.Abstarction,this));
                        }

                    }
                }
                foreach (MetaObject metaObject in removedObjects)
                    _ContainedObjects.Remove(metaObject);

                return new List<MetaObjectTreeNode>(_ContainedObjects.Values);
            }
        }
        /// <MetaDataID>{73782d12-abf0-48e8-9d96-0dad86e0b94d}</MetaDataID>
        Bitmap _Image;
        /// <MetaDataID>{3d9f2643-a684-4307-be38-eb788ae5b48a}</MetaDataID>
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
