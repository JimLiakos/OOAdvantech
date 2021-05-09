using Microsoft.VisualStudio.Uml.Classes;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{2953bb75-36a9-4e5d-ab56-73c909c20ad5}</MetaDataID>
    public class Structure : OOAdvantech.MetaDataRepository.Structure, IVSUMLModelItemWrapper
    {

        /// <MetaDataID>{a60f2e75-f535-4ea3-adf8-1488e796dc6e}</MetaDataID>
        protected Structure()
        {

        }
        /// <MetaDataID>{b00cd792-a0f7-4bc5-9cb1-32d9cf8a9526}</MetaDataID>
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;

        /// <MetaDataID>{4ee78f5a-4bb5-4b88-accb-68142f680043}</MetaDataID>
        public readonly IClass VSUmlStructure;

        /// <MetaDataID>{1054e5b2-f69a-49df-8ff7-88e537c26565}</MetaDataID>
        public Structure(IClass vsUmlStructure, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {

            VSUmlStructure = vsUmlStructure;
            VSUmlModel = iModel;
            _Name = VSUmlStructure.Name;
            MetaObjectMapper.AddTypeMap(VSUmlStructure as Microsoft.VisualStudio.Modeling.ModelElement, this);


            string metaObjectID = this.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (!string.IsNullOrEmpty(metaObjectID))
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
            else
            {
                _Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", ModelElement.Id.ToString());
            }
            PutPropertyValue("MetaData", "MetaObjectID", _Identity.ToString());

           // _Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());

            SetNamespace( VisualStudioUMLHelper.GetPackageFor(VSUmlStructure.Package, iModel));

            if (ModelElement.GetStereotypePropertyValue("Persistent") == "Persistent")
                _Persistent = true;
            else
                _Persistent = _Persistent = false;

        }

        /// <MetaDataID>{b4d3d569-73dd-45e6-aa87-236e028823b6}</MetaDataID>
        public override string Name
        {
            get
            {
                if (VSUmlStructure.Name != _Name)
                    _Name = VSUmlStructure.Name;

                return base.Name;
            }
            set
            {
                base.Name = value;
                VSUmlStructure.Name = Name;
            }
        }

        /// <MetaDataID>{abe4a1b5-f2c9-4038-a919-3eaf12dc1b53}</MetaDataID>
        public override bool Persistent
        {
            get
            {
                return base.Persistent;
            }
            set
            {
                base.Persistent = value;
                if (base.Persistent)
                    ModelElement.SetStereotypePropertyValue("Persistent", "Persistent");
                else
                    ModelElement.SetStereotypePropertyValue("Persistent", "Transient");

                
            }
        }

        /// <MetaDataID>{7c2d99f5-8b9f-43c6-8c31-5fe700617501}</MetaDataID>
        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get
            {
                return VSUmlStructure as Microsoft.VisualStudio.Modeling.ModelElement;

            }
        }
        /// <MetaDataID>{76718a6d-9fe5-44a0-a856-b3150bc24604}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }
        /// <MetaDataID>{af7b9255-b879-45ba-b875-7d27433bcf30}</MetaDataID>
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

        /// <MetaDataID>{1cf78bbc-a094-406e-bf44-d3cfcbeec3d6}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                foreach (IGeneralization vsUmlGeneralization in VSUmlStructure.Generalizations)
                {
                    if ((from generalization in _Generalizations.OfType<Generalization>()
                         where (generalization.VSUMLGeneralization as ModelElement).Id == (vsUmlGeneralization as ModelElement).Id
                         select generalization).FirstOrDefault() == null)
                    {
                        IClassifier vsUMLClassifier = vsUmlGeneralization.Target as IClassifier;
                        Structure structure = VisualStudioUMLHelper.GetClassifierFor(vsUMLClassifier as IClassifier, VSUmlModel) as Structure;
                        if(structure!=null)
                            _Generalizations.Add(new Generalization(vsUmlGeneralization, structure, this));
                    }
                }
                return base.Generalizations;
            }
        }


        /// <MetaDataID>{277da287-3fd4-47d5-a265-57632af67b18}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Realization> Realizations
        {
            get
            {
                var vsUMLRealizations = (from interfaceRealization in VSUmlStructure.ClientDependencies.OfType<IInterfaceRealization>()
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



        /// <MetaDataID>{70434fce-bea2-4666-aea4-4e65dd899938}</MetaDataID>
        bool onFeaturesLoad;
        /// <MetaDataID>{600168e0-3926-4a48-a6a8-f62b0e52bd1b}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature> Features
        {
            get
            {
                if (!onFeaturesLoad)
                {
                    onFeaturesLoad = true;
                    
                    try
                    {
                        if (VSUmlStructure == null)
                            return base.Features;

                        foreach (IOperation vsUmlOperation in VSUmlStructure.OwnedOperations)
                        {
                            MetaDataRepository.Feature feature = GetFeature((vsUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString(), false);

                            if (feature != null)
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
                                    _Features.Add(new Method(operation, vsUmlOperation, this));
                            }
                        }

                        foreach (IProperty vsUmlAttribute in VSUmlStructure.OwnedAttributes)
                        {
                            MetaDataRepository.Feature feature = GetFeature((vsUmlAttribute as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString(), false);
                            if (feature != null)
                                continue;


                            VSUMLMetaDataRepository.Attribute attribute = GetAttributeFor(vsUmlAttribute);

                            if (attribute.Owner != this)
                                _Features.Add(new AttributeRealization(vsUmlAttribute, this, attribute, VSUmlModel));
                            else
                                _Features.Add(attribute);
                            attribute = GetFeature((vsUmlAttribute as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString(), false) as VSUMLMetaDataRepository.Attribute;
                            if (attribute != null && ((vsUmlAttribute as ModelElement).GetStereotypePropertyValue("Persistent") == "Persistent" ||
                              (vsUmlAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "GenerateBackwardID") == "True"))
                            {
                                attribute.GenerateBackwardCompatibilityID();
                            }
                        }
                       
                    }
                    finally
                    {
                        onFeaturesLoad = false;
                    }
                }
                return base.Features;
            }
        }


        /// <MetaDataID>{3214b001-05a4-40ae-b1c5-9a1d8ebe428a}</MetaDataID>
        private Attribute GetAttributeFor(IProperty vsUmlAttribute)
        {
            string vsUmlAttributeName = vsUmlAttribute.Name;
            //foreach (OOAdvantech.MetaDataRepository.Interface _Interface in GetAllInterfaces())
            //{
            //    foreach (OOAdvantech.MetaDataRepository.Feature feature in _Interface.Features)
            //    {

            //        OOAdvantech.MetaDataRepository.Attribute attribute = feature as OOAdvantech.MetaDataRepository.Attribute;
            //        if (attribute != null && (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "AsProperty") && attribute.Name == roseAttribute.Name && MetaObjectMapper.GetShortNameFor(attribute.Type.FullName) == roseAttribute.Type)
            //            return attribute;
            //    }

            //}


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
            //string methodSignature = Operation.GetSignature(vsUmlperation);
            //foreach (OOAdvantech.MetaDataRepository.Interface _Interface in GetAllInterfaces())
            //{
            //    foreach (OOAdvantech.MetaDataRepository.Feature feature in _Interface.Features)
            //    {
            //        string operationSignature = null;
            //        OOAdvantech.MetaDataRepository.Operation operation = feature as OOAdvantech.MetaDataRepository.Operation;
            //        if (operation != null && operation.Name == method.Name && operation.Parameters.Count == method.Parameters.Count)
            //        {
            //            operationSignature =Operation.GetSignature(operation);
            //            if (operationSignature == methodSignature)
            //                return operation;
            //        }
            //    }

            //}

            //foreach (OOAdvantech.MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
            //{
            //    foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
            //    {
            //        string operationSignature = null;
            //        OOAdvantech.MetaDataRepository.Operation operation = feature as OOAdvantech.MetaDataRepository.Operation;
            //        if (operation != null && operation.Name == method.Name && operation.Parameters.Count == method.Parameters.Count)
            //        {
            //            operationSignature = Operation.GetSignature(operation);
            //            if (operationSignature == methodSignature)
            //                return operation;
            //        }
            //    }
            //}
            Operation operation = new Operation(vsUmlOperation, this, VSUmlModel);

            return operation;


        }




        /// <MetaDataID>{81c15844-9c30-4823-87cc-b475a4025013}</MetaDataID>
        public override MetaDataRepository.Attribute AddAttribute(string attributeName, MetaDataRepository.Classifier attributeType, string initialValue)
        {
            IProperty vsUMLAttribute = VSUmlStructure.CreateAttribute();
            vsUMLAttribute.Name = attributeName;
            if (attributeType != null)
                vsUMLAttribute.Type = attributeType.GetUMLType();
            OOAdvantech.MetaDataRepository.Attribute attribute = new Attribute(vsUMLAttribute, this, VSUmlModel);
            _Features.Add(attribute);

            return attribute;
        }

        /// <MetaDataID>{388b62fc-5291-4054-bef5-0be4a84cd81b}</MetaDataID>
        public override void RemoveFeature(MetaDataRepository.Feature feature)
        {
            if (feature is Operation)
                (feature as Operation).VSUMLOperation.Delete();

            if (feature is Attribute)
                (feature as Attribute).VSUMLAttribute.Delete();

            base.RemoveFeature(feature);
        }

        /// <MetaDataID>{6b559f94-7300-42b5-ac4f-d59adf190aed}</MetaDataID>
        public override MetaDataRepository.Operation AddOperation(string operationName, MetaDataRepository.Classifier opertionType)
        {
            IOperation opreation = VSUmlStructure.CreateOperation();
            opreation.Name = operationName;
            if (opertionType != null)
                opreation.Type = opertionType.GetUMLType();
            OOAdvantech.MetaDataRepository.Operation operation = new Operation(opreation, this, VSUmlModel);
            _Features.Add(operation);
            return operation;
        }

    }
}
