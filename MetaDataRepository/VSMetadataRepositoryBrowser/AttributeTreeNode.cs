using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{6a48e768-056a-41fc-a7d5-349d6d042318}</MetaDataID>
    public class AttributeTreeNode : MetaObjectTreeNode
    {

        /// <MetaDataID>{e4dbe052-77ed-4b39-abf6-6ea1937b7a7e}</MetaDataID>
        static Dictionary<string, Bitmap> Images = new Dictionary<string, Bitmap>();
        /// <MetaDataID>{b0f6ef4a-ca2d-4152-9ad2-17f2dcb92339}</MetaDataID>
        static AttributeTreeNode()
        {
            Images["VSObject_Field"] = Resources.VSObject_Field;
            Images["VSObject_Field_Private"] = Resources.VSObject_Field_Private;
            Images["VSObject_Field_Protected"] = Resources.VSObject_Field_Protected;
            Images["VSObject_Field_Sealed"] = Resources.VSObject_Field_Sealed;

            Images["VSObject_Realized_Field"] = Resources.VSObject_Realized_Field;
            Images["VSObject_Realized_Field_Private"] = Resources.VSObject_Realized_Field_Private;
            Images["VSObject_Realized_Field_Protected"] = Resources.VSObject_Realized_Field_Protected;
            Images["VSObject_Realized_Field_Sealed"] = Resources.VSObject_Realized_Field_Sealed;

            foreach (Bitmap image in Images.Values)
                image.MakeTransparent(Color.FromArgb(255, 0, 255));

        }

        /// <MetaDataID>{95c98fbb-2d90-4f51-bb2c-682876eaf644}</MetaDataID>
        OOAdvantech.MetaDataRepository.Attribute Attribute;
        /// <MetaDataID>{c12ed05f-7adb-4a8a-a49b-d0076e7be4b1}</MetaDataID>
        public AttributeTreeNode(OOAdvantech.MetaDataRepository.Attribute attribute, MetaObjectTreeNode parent)
            : base(attribute,parent)
        {
            Attribute = attribute;
        }

        /// <MetaDataID>{ad3e269b-6cb5-46ed-8979-d40630cf4c10}</MetaDataID>
        OOAdvantech.MetaDataRepository.AttributeRealization AttributeRealization;
        /// <MetaDataID>{6b429fd5-4da4-4b60-8673-b8d182331f4e}</MetaDataID>
        public AttributeTreeNode(OOAdvantech.MetaDataRepository.AttributeRealization attributeRealization, MetaObjectTreeNode parent)
            : base(attributeRealization,parent)
        {
            AttributeRealization = attributeRealization;
            Attribute = attributeRealization.Specification;
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
        /// <MetaDataID>{633eab45-17b9-43dc-a740-e0386a3ba1ea}</MetaDataID>
        List<MetaObjectTreeNode> _ContainedObjects = new List<MetaObjectTreeNode>();
        /// <MetaDataID>{40845bb1-67a9-4042-9430-b9b4f44c09a2}</MetaDataID>
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


                if (Attribute.Type != null)
                {
                    if ((Attribute.Type is OOAdvantech.MetaDataRepository.Structure) && (Attribute.Type as OOAdvantech.MetaDataRepository.Structure).Persistent  )
                    {
                        bool exist = false;
                        foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                        {
                            if (treeNode is ClassifierTreeNode)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                            _ContainedObjects.Add(new ClassifierTreeNode(Attribute.Type,this));
               
                    }
                    else
                    {
                        bool exist = false;
                        foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                        {
                            if (treeNode is StructuralFeatureTypeTreeNode)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                            _ContainedObjects.Add(new StructuralFeatureTypeTreeNode(Attribute,this));
                    }
                }
                else
                {
                    foreach (MetaObjectTreeNode treeNode in _ContainedObjects)
                    {
                        if (treeNode is StructuralFeatureTypeTreeNode)
                        {
                            _ContainedObjects.Remove(treeNode);
                            break;
                        }
                    }
                }
                return new List<MetaObjectTreeNode>(_ContainedObjects);
            }
        }

        /// <MetaDataID>{9eb63bfb-f9e0-4270-a9f2-1b0b39194840}</MetaDataID>
        public override Image Image
        {
            get
            {

                if (AttributeRealization == null)
                {
                    if (Attribute.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Field"];

                    if (Attribute.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Field_Private"];
                    if (Attribute.Visibility == VisibilityKind.AccessProtected)
                        return Images["VSObject_Field_Protected"];
                    if (Attribute.Visibility == VisibilityKind.AccessComponentOrProtected)
                        return Images["VSObject_Field_Protected"];
                    return Images["VSObject_Field_Sealed"];

                }
                else
                {
                    if (Attribute.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Realized_Field"];

                    if (Attribute.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Realized_Field_Private"];
                    if (Attribute.Visibility == VisibilityKind.AccessProtected)
                        return Images["VSObject_Realized_Field_Protected"];
                    if (Attribute.Visibility == VisibilityKind.AccessComponentOrProtected)
                        return Images["VSObject_Realized_Field_Protected"];
                    return Images["VSObject_Realized_Field_Sealed"];

                }

            }
        }
    }

}
