using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository;
using System.Drawing;
namespace VSMetadataRepositoryBrowser
{

    /// <MetaDataID>{d1cd0778-ee40-4667-960a-997449d1433d}</MetaDataID>
    public class ClassifierTreeNode : MetaObjectTreeNode
    {

        /// <MetaDataID>{6e77e427-5625-438c-945e-06ef50f85927}</MetaDataID>
        static Dictionary<string, Bitmap> Images = new Dictionary<string, Bitmap>();
        /// <MetaDataID>{13eb5d29-ca68-48c6-bc9c-5c1d503a8ed6}</MetaDataID>
        static ClassifierTreeNode()
        {

            Images["VSObject_Class"] = Resources.VSObject_Class;
            Images["VSObject_Class_Private"] = Resources.VSObject_Class_Private;
            Images["VSObject_Class_Protected"] = Resources.VSObject_Class_Protected;

            Images["VSObject_Interface"] = Resources.VSObject_Interface;
            Images["VSObject_Interface_Private"] = Resources.VSObject_Interface_Private;
            Images["VSObject_Interface_Protected"] = Resources.VSObject_Interface_Protected;

            Images["VSObject_Structure"] = Resources.VSObject_Structure;
            Images["VSObject_Structure_Private"] = Resources.VSObject_Structure_Private;
            Images["VSObject_Structure_Protected"] = Resources.VSObject_Structure_Protected;

            Images["VSObject_Enum"] = Resources.VSObject_Enum;
            Images["VSObject_Enum_Private"] = Resources.VSObject_Enum_Protected;
            Images["VSObject_Enum_Protected"] = Resources.VSObject_Enum_Protected;


            foreach (Bitmap image in Images.Values)
                image.MakeTransparent(Color.FromArgb(255, 0, 255));

        }

        /// <MetaDataID>{87d491ba-0091-429d-8b6a-de9df5aee4cf}</MetaDataID>
        Classifier Classifier;
        /// <MetaDataID>{de955ffc-553e-4c58-a36a-9bf550a9b802}</MetaDataID>
        public ClassifierTreeNode(Classifier classifier, MetaObjectTreeNode parent)
            : base(classifier, parent)
        {
            Classifier = classifier;
        }
        /// <MetaDataID>{56a2e6b6-2e44-4b68-9fba-537b1b3a9aff}</MetaDataID>
        bool ForRDBMSMapping;
        /// <MetaDataID>{610dc10d-2078-4136-a25d-20f511c1439c}</MetaDataID>
        public ClassifierTreeNode(Classifier classifier, MetaObjectTreeNode parent, bool forRDBMSMapping)
            : base(classifier, parent)
        {
            ForRDBMSMapping = forRDBMSMapping;
            Classifier = classifier;
        }

        /// <MetaDataID>{7c339420-88ad-4acc-89e4-d601f35a3c00}</MetaDataID>
        VisibilityKind _VisibilityKind;
        /// <MetaDataID>{d6399e99-3ef9-48b4-a252-a41c8c13842c}</MetaDataID>
        public override Image Image
        {
            get
            {
                _VisibilityKind = Classifier.Visibility;

                if (Classifier is Class)
                {
                    if (Classifier.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Class"];

                    if (Classifier.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Class_Private"];

                    return Images["VSObject_Class_Protected"];
                }

                if (Classifier is Interface)
                {
                    if (Classifier.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Interface"];

                    if (Classifier.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Interface_Private"];

                    return Images["VSObject_Interface_Protected"];
                }

                if (Classifier is Structure)
                {
                    if (Classifier.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Structure"];

                    if (Classifier.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Structure_Private"];

                    return Images["VSObject_Structure_Protected"];
                }

                if (Classifier is Enumeration)
                {
                    if (Classifier.Visibility == VisibilityKind.AccessPublic)
                        return Images["VSObject_Enum"];

                    if (Classifier.Visibility == VisibilityKind.AccessPrivate)
                        return Images["VSObject_Enum_Private"];

                    return Images["VSObject_Enum_Protected"];
                }



                return base.Image;

                //"VSObject_Class";
                //"VSObject_Class_Private";
                //"VSObject_Class_Protected";

                //"VSObject_Interface";
                //"VSObject_Interface_Private";
                //"VSObject_Interface_Protected";

                //"VSObject_Structure";
                //"VSObject_Structure_Private";
                //"VSObject_Structure_Protected";





            }
        }
        /// <MetaDataID>{45f4a86d-370a-4064-849f-6233b631dd53}</MetaDataID>
        protected override void OnMetaObjectChanged(object sender)
        {
            if (_Name != base.Name)
            {
                base.OnMetaObjectChanged(sender);
                return;
            }
            if (_VisibilityKind != Classifier.Visibility)
            {
                base.OnMetaObjectChanged(sender);
                return;
            }

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
        /// <MetaDataID>{3222ae28-18ae-4c39-9df0-e54c537ae97b}</MetaDataID>
        string _Name;
        /// <MetaDataID>{b9c453fb-0bd3-43c4-a027-ddfb0a1b7487}</MetaDataID>
        public override string Name
        {
            get
            {

                if (Classifier.IsBindedClassifier)
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateInstantiationName(Classifier.TemplateBinding, "");
                else if (Classifier.IsTemplate)
                    _Name = OOAdvantech.CodeMetaDataRepository.LanguageParser.GetTemplateName(Classifier.OwnedTemplateSignature, "");
                else
                    _Name = base.Name;
                return _Name;
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

        /// <MetaDataID>{de4cb2f9-9029-4251-821a-be809da4ddb3}</MetaDataID>
        Dictionary<MetaObject, MetaObjectTreeNode> _ContainedObjects = new Dictionary<MetaObject, MetaObjectTreeNode>();
        /// <MetaDataID>{a1116c15-94ed-48ca-acea-3ce870e60e91}</MetaDataID>
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
                List<MetaObjectTreeNode> containedObjects = new List<MetaObjectTreeNode>();
                List<MetaObject> removedObjects = new List<MetaObject>();
                removedObjects.AddRange(_ContainedObjects.Keys);
                List<MetaObjectTreeNode> items = new List<MetaObjectTreeNode>();
                if (ForRDBMSMapping)
                {


                    foreach (AssociationEnd associationEnd in Classifier.GetAssociateRoles(true))
                    {

                        if (associationEnd.Navigable)
                        {
                            removedObjects.Remove(associationEnd);
                            if (!_ContainedObjects.ContainsKey(associationEnd))
                                _ContainedObjects.Add(associationEnd, new AssociationTreeNode(associationEnd, this));
                            items.Add(_ContainedObjects[associationEnd]);

                        }
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in Classifier.GetAttributes(true))
                    {
                        if (!attribute.Persistent && !(attribute is OOAdvantech.CodeMetaDataRepository.Attribute))
                            continue;
                        removedObjects.Remove(attribute);
                        if (!_ContainedObjects.ContainsKey(attribute))
                            _ContainedObjects.Add(attribute, new AttributeTreeNode(attribute, this));
                        items.Add(_ContainedObjects[attribute]);
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();

                    OOAdvantech.MetaDataRepository.Classifier classifier = Classifier;
                    while (classifier is OOAdvantech.CodeMetaDataRepository.Class)
                    {

                        foreach (Feature feature in Classifier.Features)
                        {
                            if (feature is OOAdvantech.MetaDataRepository.AttributeRealization)
                            {
                                if (!((feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification is OOAdvantech.CodeMetaDataRepository.Attribute))
                                    continue;
                                removedObjects.Remove((feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification);
                                if (!_ContainedObjects.ContainsKey((feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification))
                                    _ContainedObjects.Add((feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification, new AttributeTreeNode((feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification, this));
                                items.Add(_ContainedObjects[(feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification]);
                            }
                        }

                        items.Sort(new MetaObjectsSort());
                        containedObjects.AddRange(items);
                        items.Clear();
                        foreach (Feature feature in Classifier.Features)
                        {
                            if (feature is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                            {
                                if (!((feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification is OOAdvantech.CodeMetaDataRepository.AssociationEnd))
                                    continue;
                                removedObjects.Remove((feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification);
                                if (!_ContainedObjects.ContainsKey((feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification))
                                    _ContainedObjects.Add((feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification, new AssociationTreeNode((feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification, this));
                                items.Add(_ContainedObjects[(feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification]);
                            }
                        }
                        items.Sort(new MetaObjectsSort());
                        containedObjects.AddRange(items);
                        items.Clear();

                        if (classifier.Generalizations.Count > 0)
                            classifier = classifier.Generalizations[0].Parent;
                    }
                }
                else
                {

                    if (!_ContainedObjects.ContainsKey(Classifier))
                        _ContainedObjects.Add(Classifier, new BaseTypesFolderTreeNode(Classifier, this));
                    else
                        removedObjects.Remove(Classifier);
                    containedObjects.Add(_ContainedObjects[Classifier]);

                    foreach (AssociationEnd associationEnd in Classifier.GetAssociateRoles(false))
                    {
                        if (associationEnd.Navigable)
                        {

                            removedObjects.Remove(associationEnd);
                            if (!_ContainedObjects.ContainsKey(associationEnd))
                                _ContainedObjects.Add(associationEnd, new AssociationTreeNode(associationEnd, this));
                            items.Add(_ContainedObjects[associationEnd]);
                        }
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();
                    foreach (Feature feature in Classifier.Features)
                    {
                        if (feature is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                        {
                            removedObjects.Remove(feature);
                            if (!_ContainedObjects.ContainsKey(feature))
                                _ContainedObjects.Add(feature, new AssociationTreeNode(feature as OOAdvantech.MetaDataRepository.AssociationEndRealization, this));
                            items.Add(_ContainedObjects[feature]);
                        }
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();

                    foreach (Feature feature in Classifier.Features)
                    {

                        if (feature is OOAdvantech.MetaDataRepository.Attribute)
                        {
                            removedObjects.Remove(feature);
                            if (!_ContainedObjects.ContainsKey(feature))
                                _ContainedObjects.Add(feature, new AttributeTreeNode(feature as OOAdvantech.MetaDataRepository.Attribute, this));
                            items.Add(_ContainedObjects[feature]);
                        }

                        if (feature is OOAdvantech.MetaDataRepository.AttributeRealization &&
                            (feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification.Owner == feature.Owner)
                            continue;

                        if (feature is OOAdvantech.MetaDataRepository.AttributeRealization)
                        {
                            removedObjects.Remove(feature);
                            if (!_ContainedObjects.ContainsKey(feature))
                                _ContainedObjects.Add(feature, new AttributeTreeNode(feature as OOAdvantech.MetaDataRepository.AttributeRealization, this));
                            items.Add(_ContainedObjects[feature]);
                        }
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();

                    foreach (Feature feature in Classifier.Features)
                    {
                        if (feature is Operation)
                        {
                            removedObjects.Remove(feature);
                            if (!_ContainedObjects.ContainsKey(feature))
                                _ContainedObjects.Add(feature, new MethodTreeNode(feature as Operation, this));
                            items.Add(_ContainedObjects[feature]);
                        }
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();

                    foreach (Feature feature in Classifier.Features)
                    {
                        if (feature is OOAdvantech.MetaDataRepository.Method &&
                            (feature as OOAdvantech.MetaDataRepository.Method).Specification.Owner == feature.Owner)
                            continue;
                        if (feature is Method)
                        {
                            removedObjects.Remove(feature);
                            if (!_ContainedObjects.ContainsKey(feature))
                                _ContainedObjects.Add(feature, new MethodTreeNode(feature as Method, this));
                            items.Add(_ContainedObjects[feature]);
                        }
                    }
                    items.Sort(new MetaObjectsSort());
                    containedObjects.AddRange(items);
                    items.Clear();

                }


                foreach (MetaObject metaObject in removedObjects)
                    _ContainedObjects.Remove(metaObject);

                return containedObjects;

            }
        }


    }


}
