using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{



    /// <MetaDataID>{23404ec5-1f91-4f5d-a26b-52d5a4bc1bb9}</MetaDataID>
    public class AssociationTreeNode : MetaObjectTreeNode
    {



        /// <MetaDataID>{7111edc6-2a13-4bc3-9b96-55cea3582abe}</MetaDataID>
        static Dictionary<string, Bitmap> Images = new Dictionary<string, Bitmap>();
        /// <MetaDataID>{653924cf-bb07-41e4-8e82-86aa96480e34}</MetaDataID>
        static AssociationTreeNode()
        {
            Images["VSObject_Association"] = Resources.VSObject_Association;
            Images["VSObject_Association_Private"] = Resources.VSObject_Association_Private;
            Images["VSObject_Association_Protected"] = Resources.VSObject_Association_Protected;
            Images["VSObject_Association_Sealed"] = Resources.VSObject_Association_Sealed;


            Images["VSObject_Realized_Association"] = Resources.VSObject_Realized_Association;
            Images["VSObject_Realized_Association_Private"] = Resources.VSObject_Realized_Association_Private;
            Images["VSObject_Realized_Association_Protected"] = Resources.VSObject_Realized_Association_Protected;
            Images["VSObject_Realized_Association_Sealed"] = Resources.VSObject_Realized_Association_Sealed;


            foreach (Bitmap image in Images.Values)
                image.MakeTransparent(Color.FromArgb(255, 0, 255));

        }

        /// <MetaDataID>{2bac2d93-416f-4a55-8e87-08850474bc9c}</MetaDataID>
        AssociationEnd AssociationEnd;
        /// <MetaDataID>{be88168f-975b-485d-aed3-a476063554a4}</MetaDataID>
        public AssociationTreeNode(AssociationEnd associationEnd, MetaObjectTreeNode parent)
            : base(associationEnd,parent)
        {
            AssociationEnd = associationEnd;
        }

        /// <MetaDataID>{8fab1aed-1cc6-4a75-9c96-0ec5b1dca082}</MetaDataID>
        AssociationEndRealization AssociationEndRealization;

        /// <MetaDataID>{e3a3f361-ec0b-41ae-aaeb-cc60a60945ad}</MetaDataID>
        public AssociationTreeNode(AssociationEndRealization associationEndRealization, MetaObjectTreeNode parent)
            : base(associationEndRealization,parent)
        {
            AssociationEndRealization = associationEndRealization;
            AssociationEnd = associationEndRealization.Specification;
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

        /// <MetaDataID>{880f2e9e-cb39-4b44-9fdf-818644907807}</MetaDataID>
        List<MetaObjectTreeNode> _ContainedObjects = new List<MetaObjectTreeNode>();
        /// <MetaDataID>{15fe09fb-0634-4079-a9a0-f08681adf1eb}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                if (_LazyLoad)
                {
                    if (_ContainedObjects.Count == 0)
                        _ContainedObjects.Add(new NodesLoader(this.MetaObject, this));

                    return _ContainedObjects.ToList();

                }

                List<MetaObjectTreeNode> containedObjects = new List<MetaObjectTreeNode>();

                Classifier memberType = AssociationEnd.CollectionClassifier;
                if (memberType == null)
                    memberType = AssociationEnd.Specification;


                if (memberType != null)
                {
                    bool exist = false;
                    foreach (AssociationEndTypeTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is AssociationEndTypeTreeNode)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        _ContainedObjects.Add(new AssociationEndTypeTreeNode(AssociationEnd,this));
                }
                else
                {
                    foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is AssociationEndTypeTreeNode)
                        {
                            _ContainedObjects.Remove(treeNode);
                            break;
                        }
                    }
                }
                return new List<MetaObjectTreeNode>(_ContainedObjects);
            }
        }



        /// <MetaDataID>{1149283a-9204-4e38-8b0a-9f01345aef33}</MetaDataID>
        public override Image Image
        {
            get
            {
                if (AssociationEndRealization == null)
                {
                    if (AssociationEnd.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Association"];

                    if (AssociationEnd.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Association_Private"];
                    if (AssociationEnd.Visibility == VisibilityKind.AccessProtected)
                        return Images["VSObject_Association_Protected"];
                    if (AssociationEnd.Visibility == VisibilityKind.AccessComponentOrProtected)
                        return Images["VSObject_Association_Protected"];
                    return Images["VSObject_Association_Sealed"];
                }
                else
                {

                    if (AssociationEnd.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Realized_Association"];

                    if (AssociationEnd.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Realized_Association_Private"];
                    if (AssociationEnd.Visibility == VisibilityKind.AccessProtected)
                        return Images["VSObject_Realized_Association_Protected"];
                    if (AssociationEnd.Visibility == VisibilityKind.AccessComponentOrProtected)
                        return Images["VSObject_Realized_Association_Protected"];
                    return Images["VSObject_Realized_Association_Sealed"];


                }


            }
        }
    }

}
