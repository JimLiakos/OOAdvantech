using Microsoft.VisualStudio.Uml.Classes;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Uml.AuxiliaryConstructs;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{b975b092-a464-4463-8c06-073f476978ee}</MetaDataID>
    public class Class : OOAdvantech.MetaDataRepository.Class, IVSUMLModelItemWrapper
    {

        /// <MetaDataID>{a0429b70-5d3a-440a-913c-2882119918cf}</MetaDataID>
        protected Class()
        {

        }

        public void Refresh()
        {
            _Generalizations.Clear();
            _Realizations.Clear();
        }
        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            if (VSUmlClass == null)
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
                VSUmlClass = (_Namespace.Value as Package).VSUmlPackage.CreateClass();
                MetaObjectMapper.AddTypeMap(VSUmlClass as ModelElement, this);
                VSUmlClass.Name = originMetaObject.Name;
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


        /// <MetaDataID>{0a36d0bc-a88e-4d85-aec8-89eefa877afa}</MetaDataID>
        public override void RemoveFeature(MetaDataRepository.Feature feature)
        {
            if (feature is Operation)
                (feature as Operation).VSUMLOperation.Delete();

            if (feature is Attribute)
                (feature as Attribute).VSUMLAttribute.Delete();

            base.RemoveFeature(feature);
        }
        /// <MetaDataID>{6665421a-8b58-4214-85e1-6b6c044d787f}</MetaDataID>
        public override MetaDataRepository.Attribute AddAttribute(string attributeName, MetaDataRepository.Classifier attributeType, string initialValue)
        {
            IProperty vsUMLAttribute = VSUmlClass.CreateAttribute();
            vsUMLAttribute.Name = attributeName;
            if (attributeType != null)
                vsUMLAttribute.Type = attributeType.GetUMLType();
            OOAdvantech.MetaDataRepository.Attribute attribute = new Attribute(vsUMLAttribute, this, VSUmlModel);
            _Features.Add(attribute);

            return attribute;
        }
        /// <MetaDataID>{035ba46e-9faa-4267-ac97-fb751274b9a7}</MetaDataID>
        public override MetaDataRepository.Operation AddOperation(string operationName, MetaDataRepository.Classifier opertionType)
        {
            IOperation vsUMLopretion = VSUmlClass.CreateOperation();
            vsUMLopretion.Name = operationName;
            if (opertionType != null)
                vsUMLopretion.Type = opertionType.GetUMLType();
            OOAdvantech.MetaDataRepository.Operation operation = new Operation(vsUMLopretion, this, VSUmlModel);
            _Features.Add(operation);
            return operation;
        }

        /// <MetaDataID>{b9c86c3d-2cd5-42dd-a29e-3c69f512a964}</MetaDataID>
        public override MetaDataRepository.TemplateSignature OwnedTemplateSignature
        {
            get
            {
                var erer = VSUmlClass as IClass;

                if (VSUmlClass.OwnedRedefinableTemplateSignature != null)
                {
                    if (_OwnedTemplateSignature == null)
                        _OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                    int i = 0;
                    foreach (var templateParameter in VSUmlClass.OwnedRedefinableTemplateSignature.OwnedParameters)
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

        /// <MetaDataID>{7ec57fc9-1672-40f8-bf80-916e2d077e4a}</MetaDataID>
        public override string Name
        {
            get
            {
                if (VSUmlClass.Name != _Name)
                    _Name = VSUmlClass.Name;
                return base.Name;
            }
            set
            {
                base.Name = value;
                VSUmlClass.Name = Name;
            }
        }

        

        /// <MetaDataID>{32ceb540-cdf7-4ba7-b2f5-e20a261a8e03}</MetaDataID>
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        /// <MetaDataID>{c7d6203a-546f-4c89-90dc-0251c6dc6e53}</MetaDataID>
        public IClass VSUmlClass;

        public Class(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
        }
        /// <MetaDataID>{31467c67-8e33-4b98-a039-432a190479ff}</MetaDataID>
        public Class(Microsoft.VisualStudio.Uml.Classes.IClass vsUmlClass, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {

            VSUmlClass = vsUmlClass;
            VSUmlModel = iModel;
            _Name = VSUmlClass.Name;
            string metaObjectID = this.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (!string.IsNullOrEmpty(metaObjectID))
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
            else
            {
                _Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", ModelElement.Id.ToString());
            }
            PutPropertyValue("MetaData", "MetaObjectID", _Identity.ToString());
            
            MetaObjectMapper.AddTypeMap(VSUmlClass as Microsoft.VisualStudio.Modeling.ModelElement, this);


            SetNamespace(VisualStudioUMLHelper.GetPackageFor(VSUmlClass.Package, iModel));
            if (ModelElement.GetStereotypePropertyValue("Persistent") == "Persistent")
                _Persistent = true;
            else
                _Persistent = false;

            _Abstract = (ModelElement as IClassifier).IsAbstract;
        }


        /// <MetaDataID>{eee7ab83-a644-4d1c-8033-20da8a31cc2b}</MetaDataID>
        public override bool Abstract
        {
            get
            {
                return base.Abstract;
            }
            set
            {
                base.Abstract = value;
                (ModelElement as IClassifier).IsAbstract = value;
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
                        if (VSUmlClass == null)
                            return base.Features;

                        foreach (IOperation vsUmlOperation in VSUmlClass.OwnedOperations)
                        {
                            MetaDataRepository.Feature feature=GetFeature((vsUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(),false);

                            if(feature!=null)
                                continue;

                            feature = MetaObjectMapper.FindMetaObjectFor(vsUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement) as MetaDataRepository.Feature;
                            if (feature != null)
                                _Features.Add(feature);
                            else
                            {
                                OOAdvantech.MetaDataRepository.Operation operation = GetOperationForMethod(vsUmlOperation);
                                if (operation == null)
                                    continue;
                                if (operation.Owner == this)
                                    _Features.Add(operation);
                                else
                                    _Features.Add(new Method(operation, vsUmlOperation, this, VSUmlModel));
                            }
                        }

                        foreach (IProperty vsUmlAttribute in VSUmlClass.OwnedAttributes)
                        {
                            MetaDataRepository.Feature feature=GetFeature((vsUmlAttribute as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(),false);
                            if(feature!=null)
                                continue;

  
                            VSUMLMetaDataRepository.Attribute attribute = GetAttributeFor(vsUmlAttribute);

                            if (attribute.Owner != this)
                                _Features.Add(new AttributeRealization(vsUmlAttribute, this, attribute,VSUmlModel));
                            else
                                _Features.Add(attribute);
                            attribute = GetFeature((vsUmlAttribute as Microsoft.VisualStudio.Modeling.ModelElement).GetIdentity(), false) as VSUMLMetaDataRepository.Attribute;
                            if (attribute!=null&&( (vsUmlAttribute as ModelElement).GetStereotypePropertyValue("Persistent") == "Persistent" ||
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

    

        /// <MetaDataID>{3214b001-05a4-40ae-b1c5-9a1d8ebe428a}</MetaDataID>
        private Attribute GetAttributeFor(IProperty vsUmlAttribute)
        {
            string vsUmlAttributeName = vsUmlAttribute.Name;
            foreach (OOAdvantech.MetaDataRepository.Interface _Interface in GetAllInterfaces())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in _Interface.Features)
                {

                    Attribute attribute = feature as Attribute;
                    if (attribute != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") && attribute.Name == vsUmlAttributeName && MetaObjectMapper.GetShortNameFor(attribute.Type.FullName) == vsUmlAttribute.Type.Name)
                        return attribute;
                }

            }


            //foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
            //{
            //    foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
            //    {
            //        OOAdvantech.MetaDataRepository.Attribute attribute = feature as OOAdvantech.MetaDataRepository.Attribute;
            //        if (attribute != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") && attribute.Name == roseAttribute.Name && MetaObjectMapper.GetShortNameFor(attribute.Type.FullName) == roseAttribute.Type)
            //            return attribute;
            //    }
            //}
            return new Attribute(vsUmlAttribute, this, VSUmlModel);
        }

    

        /// <MetaDataID>{c790f8c6-e1c0-47b1-8e06-4ae7c3e6efb1}</MetaDataID>
        internal MetaDataRepository.Operation GetOperationForMethod(IOperation vsUmlOperation)
        {
            string methodSignature = Operation.GetSignature(vsUmlOperation,UMLModel);
            foreach (OOAdvantech.MetaDataRepository.Interface _Interface in GetAllInterfaces())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in _Interface.Features)
                {
                    string operationSignature = null;
                    OOAdvantech.MetaDataRepository.Operation interfaceOperation = feature as OOAdvantech.MetaDataRepository.Operation;
                    if (interfaceOperation != null && interfaceOperation.Name == vsUmlOperation.Name && interfaceOperation.Parameters.Count == vsUmlOperation.GetNumOfParameters())
                    {
                        operationSignature = Operation.GetSignature(interfaceOperation);
                        if (operationSignature == methodSignature)
                            return interfaceOperation;
                    }
                }

            }

            foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
            {
                foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
                {
                    string operationSignature = null;
                    OOAdvantech.MetaDataRepository.Operation generalOperation = feature as OOAdvantech.MetaDataRepository.Operation;
                    if (generalOperation != null && generalOperation.Name == vsUmlOperation.Name && generalOperation.Parameters.Count == vsUmlOperation.GetNumOfParameters())
                    {
                        operationSignature = Operation.GetSignature(generalOperation);
                        if (operationSignature == methodSignature)
                            return generalOperation;
                    }
                }
            }
            Operation operation = new Operation(vsUmlOperation, this, VSUmlModel);

            return operation;


        }



        /// <MetaDataID>{94e2ce65-51c4-4951-bfb6-1f535193d311}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            { 
                foreach (IGeneralization vsUmlGeneralization in VSUmlClass.Generalizations)
                {
                    if ((from generalization in _Generalizations.OfType<Generalization>()
                         where (generalization.VSUMLGeneralization as ModelElement).Id == (vsUmlGeneralization as ModelElement).Id
                         select generalization).FirstOrDefault() == null)
                    {
                        IClassifier vsUMLClassifier = vsUmlGeneralization.Target as IClassifier;
                        Class _class = VisualStudioUMLHelper.GetClassifierFor(vsUMLClassifier as IClassifier, VSUmlModel) as Class;
                        _Generalizations.Add(new Generalization(vsUmlGeneralization, _class, this));
                    } 
                }
                return base.Generalizations;
            }
        }
         
        /// <MetaDataID>{e0cac870-9310-4664-8dab-64134f6adbf0}</MetaDataID>
        public override bool Persistent
        {
            get
            {
                if (ModelElement.GetStereotypePropertyValue("Persistent") == "Persistent")
                    _Persistent = true;
                else
                    _Persistent = false;

                return base.Persistent; 
            }
            set
            {
                base.Persistent = value;
                if(base.Persistent )
                    ModelElement.SetStereotypePropertyValue("Persistent", "Persistent");
                else
                    ModelElement.SetStereotypePropertyValue("Persistent", "Transient");
            }
        }


        /// <MetaDataID>{f737018f-a994-4af0-9f15-4bcedaceedc8}</MetaDataID>
        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get 
            {
                return VSUmlClass as Microsoft.VisualStudio.Modeling.ModelElement;
                
            }
        }
        /// <MetaDataID>{e636668a-b073-4fac-bfc9-963056c98429}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }
        /// <MetaDataID>{c673b7f1-cb8a-4d95-9a8b-dee6d50ed4be}</MetaDataID>
        public override MetaDataRepository.Component ImplementationUnit
        {
            get
            {
                System.Collections.Generic.List<Component> components = (from vsUMLComponent in VSUmlModel.Members.OfType<Microsoft.VisualStudio.Uml.Components.IComponent>()
                                                                        from appliedStereotype in vsUMLComponent.AppliedStereotypes
                                                           where appliedStereotype.Profile == "OOAdvantechProfile"
                                                                         select VisualStudioUMLHelper.GetComponentFor(vsUMLComponent, VSUmlModel)).ToList();

                foreach( Component component  in components)
                {
                    if(component.Residents.Contains(this))
                    {
                        _ImplementationUnit.Value=component;
                        return _ImplementationUnit;
                    }
                }


                return base.ImplementationUnit;
            }
        }



        /// <MetaDataID>{6f89b281-ed5a-469b-b7a7-4173b76e7570}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Realization> Realizations
        {
            get
            {
                var vsUMLRealizations = (from interfaceRealization in VSUmlClass.ClientDependencies.OfType<IInterfaceRealization>()
                                         select interfaceRealization);

                foreach (IInterfaceRealization interfaceRealization in vsUMLRealizations)
                {
                    if ((from realization in _Realizations.OfType<Realization>()
                         where (realization.InterfaceRealization as ModelElement).Id == (interfaceRealization as ModelElement).Id
                         select realization).FirstOrDefault() == null)
                    {
                        IInterface vsUMLInterface = interfaceRealization.Supplier as IInterface;
                        Interface _interface = VisualStudioUMLHelper.GetClassifierFor(vsUMLInterface as IClassifier, VSUmlModel) as Interface;
                        _Realizations.Add(new Realization(interfaceRealization, _interface, this));
                    }

                        
                }



                

                return base.Realizations;
            }
        }



        /// <MetaDataID>{fdb790b4-f4ef-4e79-b0e1-e6d527f7ac9b}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.AssociationEnd> Roles
        {
            get
            {
                foreach (IProperty vsUMLAssociationend in VSUmlClass.GetOutgoingAssociationEnds())
                { 

                    Association association = VisualStudioUMLHelper.GetAssociationFor(vsUMLAssociationend.Association,VSUmlModel);
                    //if ((association.RoleA as AssociationEnd).VSUMLAssociationEnd == vsUMLAssociationend && !_Roles.Contains(association.RoleA))
                    //    _Roles.Add(association.RoleA);
                    //if ((association.RoleB as AssociationEnd).VSUMLAssociationEnd == vsUMLAssociationend && !_Roles.Contains(association.RoleB))
                    //    _Roles.Add(association.RoleB);

                }
                return base.Roles;
            }
        }
       
    }
}
