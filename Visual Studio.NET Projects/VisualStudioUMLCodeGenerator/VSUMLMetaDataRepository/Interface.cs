using Microsoft.VisualStudio.Uml.Classes;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{316369b6-39e8-49a0-ab8b-718bddd1b618}</MetaDataID>
    public class Interface : OOAdvantech.MetaDataRepository.Interface, IVSUMLModelItemWrapper
    {

        /// <MetaDataID>{9fad3d43-956a-4f2b-b68d-fb1c1005b638}</MetaDataID>
        protected Interface()
        {

        }


        /// <MetaDataID>{0b78435d-bc64-4f85-a5a1-3c90acd318fa}</MetaDataID>
        public void Refresh()
        {
            _Generalizations.Clear();
        }


        /// <MetaDataID>{bbf78f83-524f-4bf9-aeba-5eff6fede9a3}</MetaDataID>
        public override string Name
        {
            get
            {
                if (VSUmlInterface.Name != _Name)
                    _Name = VSUmlInterface.Name;
                return base.Name;
            }
            set
            {
                base.Name = value;
                VSUmlInterface.Name = Name;
            }
        }


        /// <MetaDataID>{63edbda0-2b86-4213-bef0-8d0004fb25e6}</MetaDataID>
        public override MetaDataRepository.Attribute AddAttribute(string attributeName, MetaDataRepository.Classifier attributeType, string initialValue)
        {
            IProperty vsUMLAttribute = VSUmlInterface.CreateAttribute();
            vsUMLAttribute.Name = attributeName;
            if (attributeType != null)
                vsUMLAttribute.Type = attributeType.GetUMLType();
            OOAdvantech.MetaDataRepository.Attribute attribute = new Attribute(vsUMLAttribute, this, VSUmlModel);
            _Features.Add(attribute);

            return attribute;
        }

        /// <MetaDataID>{836f87f3-8537-4ad3-ae1d-ffd1847e1a21}</MetaDataID>
        public override void RemoveFeature(MetaDataRepository.Feature feature)
        {
            if (feature is Operation)
                (feature as Operation).VSUMLOperation.Delete();

            if (feature is Attribute)
                (feature as Attribute).VSUMLAttribute.Delete();

            base.RemoveFeature(feature);
        }

        /// <MetaDataID>{dc007a3a-d3dd-4e8f-9c8d-2717f4e4161b}</MetaDataID>
        public override MetaDataRepository.Operation AddOperation(string operationName, MetaDataRepository.Classifier opertionType)
        {
            IOperation opreation = VSUmlInterface.CreateOperation();
            opreation.Name = operationName;
            if (opertionType != null)
                opreation.Type = opertionType.GetUMLType();
            OOAdvantech.MetaDataRepository.Operation operation = new Operation(opreation, this, VSUmlModel);
            _Features.Add(operation);
            return operation;
        }


        public override Collections.Generic.Set<MetaDataRepository.AssociationEnd> Roles
        {
            get
            {
                foreach (IProperty vsUMLAssociationend in VSUmlInterface.GetOutgoingAssociationEnds())
                {
                    Association association = VisualStudioUMLHelper.GetAssociationFor(vsUMLAssociationend.Association, VSUmlModel);

                    //if ((association.RoleA as AssociationEnd).VSUMLAssociationEnd == vsUMLAssociationend && !_Roles.Contains(association.RoleA))
                    //    _Roles.Add(association.RoleA);
                    //if ((association.RoleB as AssociationEnd).VSUMLAssociationEnd == vsUMLAssociationend && !_Roles.Contains(association.RoleB))
                    //    _Roles.Add(association.RoleB);
                }
                return base.Roles;
            }
        }

        /// <MetaDataID>{0191f23c-c696-466d-9b52-b35d3383f898}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                foreach (IGeneralization vsUmlGeneralization in VSUmlInterface.Generalizations)
                {
                    if ((from generalization in _Generalizations.OfType<Generalization>()
                         where (generalization.VSUMLGeneralization as ModelElement).Id == (vsUmlGeneralization as ModelElement).Id
                         select generalization).FirstOrDefault() == null)
                    {
                        IClassifier vsUMLClassifier = vsUmlGeneralization.Target as IClassifier;
                        Interface _interface = VisualStudioUMLHelper.GetClassifierFor(vsUMLClassifier as IClassifier, VSUmlModel) as Interface;
                        _Generalizations.Add(new Generalization(vsUmlGeneralization, _interface, this));
                    }
                }
                return base.Generalizations;
            }
        }

        /// <MetaDataID>{11ce8504-c6c4-4d7f-a226-bf28d1c1a32e}</MetaDataID>
        public override MetaDataRepository.TemplateSignature OwnedTemplateSignature
        {
            get
            {
                var erer = VSUmlInterface as IInterface;

                if (VSUmlInterface.OwnedRedefinableTemplateSignature != null)
                {
                    if (_OwnedTemplateSignature == null)
                        _OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                    int i = 0;
                    foreach (var templateParameter in VSUmlInterface.OwnedRedefinableTemplateSignature.OwnedParameters)
                    {
                        if (_OwnedTemplateSignature.OwnedParameters.Count > i)
                            _OwnedTemplateSignature.OwnedParameters[i].Name = templateParameter.ParameteredElement.Name;
                        else
                            _OwnedTemplateSignature.AddOwnedParameter(new OOAdvantech.MetaDataRepository.TemplateParameter(templateParameter.ParameteredElement.Name));
                        i++;
                    }

                }

                return base.OwnedTemplateSignature;
            }
            set
            {
                base.OwnedTemplateSignature = value;
            }
        }


        /// <MetaDataID>{a959f9ec-ad7e-443c-b033-9a0aaa808646}</MetaDataID>
        bool IsFeaturesLoaded;
        /// <MetaDataID>{15458101-0e07-4be4-b890-25375e87e2c5}</MetaDataID>
        bool onFeaturesLoad;

        /// <MetaDataID>{7fd561a6-b1cf-498c-99a4-71a32d95539d}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> Features
        {
            get
            {
                if (!onFeaturesLoad)
                {
                    onFeaturesLoad = true;
                    IsFeaturesLoaded = false;
                    try
                    {
                        if (VSUmlInterface == null)
                            return base.Features;

                        foreach (IOperation vsUmlOperation in VSUmlInterface.OwnedOperations)
                        {
                            MetaDataRepository.Feature feature = GetFeature((vsUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString(), false);

                            if (feature != null)
                                continue;

                            feature = MetaObjectMapper.FindMetaObjectFor(vsUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement) as MetaDataRepository.Feature;
                            if (feature != null)
                                _Features.Add(feature);
                            else
                            {
                                OOAdvantech.MetaDataRepository.Operation operation = new Operation(vsUmlOperation, this, VSUmlModel);

                                _Features.Add(operation);

                            }
                        }

                        foreach (IProperty vsUmlAttribute in VSUmlInterface.OwnedAttributes)
                        {
                            MetaDataRepository.Feature feature = GetFeature((vsUmlAttribute as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(), false);
                            if (feature != null)
                                continue;



                            //OOAdvantech.MetaDataRepository.AssociationEnd associationEnd = GetAssociationEndFor(roseAttribute);
                            //if (associationEnd != null)
                            //{
                            //    _Features.Add(new AssociationEndRealization(roseAttribute, this, associationEnd));
                            //    continue;

                            //}

                            VSUMLMetaDataRepository.Attribute attribute = new Attribute(vsUmlAttribute, this, VSUmlModel); ;
                            _Features.Add(attribute);
                            attribute = GetFeature((vsUmlAttribute as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(), false) as VSUMLMetaDataRepository.Attribute;
                            if (attribute != null && ((vsUmlAttribute as ModelElement).GetStereotypePropertyValue("Persistent") == "Persistent" ||
                              (vsUmlAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "GenerateBackwardID") == "True"))
                            {
                                attribute.GenerateBackwardCompatibilityID();
                            }

                        }


                        IsFeaturesLoaded = true;
                    }
                    finally
                    {
                        onFeaturesLoad = false;
                        if (!IsFeaturesLoaded)
                            _Features.RemoveAll();

                    }
                }
                return base.Features;
            }
        }



        /// <MetaDataID>{c3fdccf9-b882-4757-b804-53a7edb87020}</MetaDataID>
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;

        /// <MetaDataID>{f21311ab-1883-4a55-8cda-37863344f651}</MetaDataID>
        internal  IInterface VSUmlInterface;

        /// <MetaDataID>{c096e39a-8247-4a0e-a732-3bd761e6d15c}</MetaDataID>
        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get
            {

                return VSUmlInterface as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }


        public Interface(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
        }


        /// <MetaDataID>{76c956ef-8c38-4b37-95af-91e71d79b8af}</MetaDataID>
        public Interface(IInterface vsUmlInterface, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlInterface = vsUmlInterface;
            VSUmlModel = iModel;
            _Name = VSUmlInterface.Name;

            string metaObjectID = this.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (!string.IsNullOrEmpty(metaObjectID))
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
            else
            {
                _Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", ModelElement.Id.ToString());
            }
            PutPropertyValue("MetaData", "MetaObjectID", _Identity.ToString());
            //_Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
            
            MetaObjectMapper.AddTypeMap(VSUmlInterface as Microsoft.VisualStudio.Modeling.ModelElement, this);
            SetNamespace(VisualStudioUMLHelper.GetPackageFor(VSUmlInterface.Package, iModel));
        }

        /// <MetaDataID>{5963a8e4-d8f3-4b68-96d0-6ef0e6d94170}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }



        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            if (VSUmlInterface == null)
            {
                VSUmlModel.GetPackages();
                if (_Namespace.Value == null)
                {
                    if (originMetaObject.Namespace != null)
                    {
                        _Namespace.Value = VSUmlModel.GetPackage(originMetaObject.Namespace.FullName);
                        if (_Namespace.Value == null)
                            _Namespace.Value = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.Namespace, this) as Package;
                        _Namespace.Value.ShallowSynchronize(originMetaObject.Namespace);
                    }
                }
                VSUmlInterface = (_Namespace.Value as Package).VSUmlPackage.CreateInterface() ;
                MetaObjectMapper.AddTypeMap(VSUmlInterface as ModelElement, this);
                VSUmlInterface.Name = originMetaObject.Name;
                string metaObjectID = GetPropertyValue<string>("MetaData", "MetaObjectID");
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", metaObjectID);
            }

            int count = Realizations.Count;
            count = Generalizations.Count;
            count = Features.Count;
            count = Roles.Count;
            base.Synchronize(originMetaObject);
        }


        /// <MetaDataID>{3f897f1e-fd44-4683-b406-6c08c7612560}</MetaDataID>
        public override MetaDataRepository.Component ImplementationUnit
        {
            get
            {
                System.Collections.Generic.List<Component> components = (from vsUMLComponent in VSUmlModel.Members.OfType<Microsoft.VisualStudio.Uml.Components.IComponent>()
                                                                         from appliedStereotype in vsUMLComponent.AppliedStereotypes
                                                                         where appliedStereotype.Profile == "OOAdvantechProfile"
                                                                         select VisualStudioUMLHelper.GetComponentFor(vsUMLComponent, VSUmlModel)).ToList();

                foreach (Component component in components)
                {
                    if (component.Residents.Contains(this))
                    {
                        _ImplementationUnit.Value = component;
                        return _ImplementationUnit;
                    }
                }


                return base.ImplementationUnit;
            }
        }

    }
}
