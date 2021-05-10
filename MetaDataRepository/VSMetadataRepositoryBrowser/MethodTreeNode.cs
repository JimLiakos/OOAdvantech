using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OOAdvantech.MetaDataRepository;

namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{56a6c4b6-0be4-44f9-ba72-be38eff15f55}</MetaDataID>
    public class MethodTreeNode : MetaObjectTreeNode
    {

        /// <MetaDataID>{3bab5bdd-b656-40ff-9cb2-15a408fbf044}</MetaDataID>
        static Dictionary<string, Bitmap> Images = new Dictionary<string, Bitmap>();
        /// <MetaDataID>{6c4c337c-e66c-4bf7-ae59-99a6b2cf158b}</MetaDataID>
        static MethodTreeNode()
        {
            Images["VSObject_Method"] = Resources.VSObject_Method;
            Images["VSObject_Method_Private"] = Resources.VSObject_Method_Private;
            Images["VSObject_Method_Protected"] = Resources.VSObject_Method_Protected;
            Images["VSObject_Method_Sealed"] = Resources.VSObject_Method_Sealed;

            Images["VSObject_MethodOverload"] = Resources.VSObject_MethodOverload;
            Images["VSObject_MethodOverload_Private"] = Resources.VSObject_MethodOverload_Private;
            Images["VSObject_MethodOverload_Protected"] = Resources.VSObject_MethodOverload_Protected;
            Images["VSObject_MethodOverload_Sealed"] = Resources.VSObject_MethodOverload_Sealed;



            foreach (Bitmap image in Images.Values)
                image.MakeTransparent(Color.FromArgb(255, 0, 255));

        }

        /// <MetaDataID>{574423fb-4cc0-4227-bbd0-7afbb469d104}</MetaDataID>
        Operation Operation;
        /// <MetaDataID>{62f34c4d-9667-4afd-aff0-5ea163548c2a}</MetaDataID>
        public MethodTreeNode(Operation operation, MetaObjectTreeNode parent)
            : base(operation,parent)
        {
            Operation = operation;
        }

        /// <MetaDataID>{09bc8a5a-f81a-4977-b35c-e94fc0fabc18}</MetaDataID>
        Method Method;
        /// <MetaDataID>{1ae89d7d-38ec-4fb3-a6e8-42299295f726}</MetaDataID>
        public MethodTreeNode(Method method, MetaObjectTreeNode parent)
            : base(method,parent)
        {
            Method = method;
            Operation = method.Specification;
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
        /// <MetaDataID>{fdffb8a5-bd6b-455c-810e-b526804f162e}</MetaDataID>
        List<MetaObjectTreeNode> _ContainedObjects = new List<MetaObjectTreeNode>();
        /// <MetaDataID>{32b552f2-efa2-411a-a841-b4a73e0517f6}</MetaDataID>
        public override List<MetaObjectTreeNode> ContainedObjects
        {
            get
            {
                if (_LazyLoad)
                {
                    if (_ContainedObjects.Count == 0)
                        _ContainedObjects.Add( new NodesLoader(this.MetaObject, this));

                    return _ContainedObjects.ToList();

                }

                List<MetaObjectTreeNode> containedObjects = new List<MetaObjectTreeNode>();

                if (Operation.Parameters.Count > 0)
                {
                    bool exist = false;
                    foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is ParametersTreeNode)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        _ContainedObjects.Insert(0,new ParametersTreeNode(Operation,this));
                }
                else
                {
                    foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is ParametersTreeNode)
                        {
                            _ContainedObjects.Remove(treeNode);
                            break;
                        }
                    }

                }

                if (Operation.ReturnType != null && 
                    Operation.ReturnType.FullName!="System.Void"&&
                    Operation.ReturnType.FullName!="void")
                {
                    bool exist = false;
                    foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is ReturnTypeTreeNode)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        _ContainedObjects.Add( new ReturnTypeTreeNode(Operation,this));
                }
                else
                {
                    foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is ReturnTypeTreeNode)
                        {
                            _ContainedObjects.Remove(treeNode);
                            break;
                        }
                    }
                }
                return new List<MetaObjectTreeNode>(_ContainedObjects);
            }
        }



        /// <MetaDataID>{31c5b3ec-58c2-448e-b89a-1eb5ab8e8f7c}</MetaDataID>
        public override Image Image
        {
            get
            {

                //if (Method != null && Method.Visibility == VisibilityKind.AccessPublic)
                if (Method != null && Method.Visibility == VisibilityKind.AccessPublic)
                    return Images["VSObject_MethodOverload"];
                if (Method != null && Method.Visibility == VisibilityKind.AccessPrivate)
                    return Images["VSObject_MethodOverload_Private"];
                if (Method != null && Method.Visibility == VisibilityKind.AccessProtected)
                    return Images["VSObject_MethodOverload_Protected"];
                if (Method != null && Method.Visibility == VisibilityKind.AccessComponentOrProtected)
                    return Images["VSObject_MethodOverload_Protected"];
                if (Method != null)
                    return Images["VSObject_MethodOverload_Sealed"];





                if (Operation.Visibility == VisibilityKind.AccessPublic)
                    return Images["VSObject_Method"];

                if (Operation.Visibility == VisibilityKind.AccessPrivate)
                    return Images["VSObject_Method_Private"];
                if (Operation.Visibility == VisibilityKind.AccessProtected)
                    return Images["VSObject_Method_Protected"];
                if (Operation.Visibility == VisibilityKind.AccessComponentOrProtected)
                    return Images["VSObject_Method_Protected"];
                return Images["VSObject_Method_Sealed"];

            }
        }
    }

}
