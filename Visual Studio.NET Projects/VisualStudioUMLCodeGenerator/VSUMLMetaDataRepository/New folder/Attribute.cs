using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{fa80181f-95f2-4340-bf60-f170271e9fee}</MetaDataID>
    public class Attribute : OOAdvantech.MetaDataRepository.Attribute, IVSUMLModelItemWrapper
    {


        /// <MetaDataID>{fbb56f42-fdc7-4e31-b4d3-7f8178db5042}</MetaDataID>
        protected Attribute()
        {

        }


        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            MetaDataRepository.Attribute orgAttribute = originMetaObject as MetaDataRepository.Attribute;
            if (VSUMLAttribute == null)
            {
                string metaObjectID = GetPropertyValue<string>("MetaData", "MetaObjectID");
                if ((Owner as IVSUMLModelItemWrapper).ModelElement is IInterface)
                {
                    VSUMLAttribute = ((Owner as IVSUMLModelItemWrapper).ModelElement as IInterface).CreateAttribute();
                    VSUMLAttribute.Name = orgAttribute.Name;
                    //_Type = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgAttribute.Type.FullName);
                    //VSUMLAttribute.Type = _Type.GetUMLType();

                }
                else if ((Owner as IVSUMLModelItemWrapper).ModelElement is IClass)
                {
                    VSUMLAttribute = ((Owner as IVSUMLModelItemWrapper).ModelElement as IClass).CreateAttribute();
                    VSUMLAttribute.Name = orgAttribute.Name;
                    //_Type = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgAttribute.Type.FullName);
                    //VSUMLAttribute.Type = _Type.GetUMLType();
                }
                VSUmlModel = (Owner as IVSUMLModelItemWrapper).UMLModel;
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", metaObjectID);

            }
            _Type = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgAttribute.Type.FullName);
            VSUMLAttribute.Type = _Type.GetUMLType();
            VSUMLAttribute.Visibility = VisualStudioUMLHelper.GetVisibilityKind(originMetaObject.Visibility);



            //string metaObjectID = this.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            base.Synchronize(originMetaObject);
        }

        /// <MetaDataID>{7f05c07c-4e86-4a53-8e40-8f0f7b97a918}</MetaDataID>
        internal Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;

        /// <MetaDataID>{d7186757-2f81-476a-b24b-208d7b4df832}</MetaDataID>
        internal IProperty VSUMLAttribute;

        /// <MetaDataID>{68681b6c-840c-4e05-8415-08885512f83a}</MetaDataID>
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
                if (base.Persistent)
                    ModelElement.SetStereotypePropertyValue("Persistent", "Persistent");
                else
                    ModelElement.SetStereotypePropertyValue("Persistent", "Transient");
            }
        }
        /// <MetaDataID>{753a1400-8ffe-4529-9b31-9ed7b394e140}</MetaDataID>
        internal void GenerateBackwardCompatibilityID()
        {
            if (string.IsNullOrEmpty((VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "Identity")))
            {
               uint nextMemember=Owner.GetNextAutoGenMemberID();
               (VSUMLAttribute as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "Identity", "+" + nextMemember);
               PutPropertyValue("MetaData", "BackwardCompatibilityID", ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity"));
               this.MetaObjectChangeState();
            }
        }

        /// <MetaDataID>{858833be-bfb8-4ead-849c-74e16251fce6}</MetaDataID>
        public bool IsProperty
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "AsProperty");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;

            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AsProperty", value.ToString());
                PutPropertyValue("MetaData", "AsProperty", value);
            }
        }

        internal Attribute(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {

        }
        /// <MetaDataID>{6445f56d-35a2-4644-a3d4-8254f9d04595}</MetaDataID>
        public Attribute(IProperty vsUMLAttribute, MetaDataRepository.Classifier owner, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            
            VSUMLAttribute = vsUMLAttribute;
            _Owner = owner;
            VSUmlModel = iModel;
            _Name = vsUMLAttribute.Name;

            
            //if (string.IsNullOrEmpty((VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "UniqueID")))
            //    (VSUMLAttribute as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "UniqueID", (vsUMLAttribute as ModelElement).Id.ToString());

            //if (string.IsNullOrEmpty((VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID")) ||
            //    (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "UniqueID") != (vsUMLAttribute as ModelElement).Id.ToString())
            //{
            //    (VSUMLAttribute as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID",  (VSUMLAttribute as ModelElement).Id.ToString());
            //    (VSUMLAttribute as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "UniqueID", (vsUMLAttribute as ModelElement).Id.ToString());
            //}
            

            string metaObjectID = this.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (!string.IsNullOrEmpty(metaObjectID))
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
            else
            {
                _Identity = new MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", ModelElement.Id.ToString());
            }
            PutPropertyValue("MetaData", "MetaObjectID", _Identity.ToString());
            //_Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(ModelElement.Id.ToString());
     
            Visibility = VisualStudioUMLHelper.GetVisibilityKind(VSUMLAttribute.Visibility);
            if ((VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "AsProperty") == "True" || _Owner is Interface)
            {
                PutPropertyValue("MetaData", "AsProperty", true);
                bool getMethod = (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("Csharp", "GenerateGetOperation") == "True";
                bool setMethod = (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("Csharp", "GenerateSetOperation") == "True";
                PutPropertyValue("MetaData", "Getter", getMethod);
                PutPropertyValue("MetaData", "Setter", setMethod);

            }
            else
                PutPropertyValue("MetaData", "AsProperty", false);

            PutPropertyValue("MetaData", "BackwardCompatibilityID", (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "Identity"));

            string persistentMemberName = (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("Persistent", "Member that implement");
            if (persistentMemberName == "Auto Generate")
                PutPropertyValue("MetaData", "ImplementationMember", "_" + Name);
            else
                PutPropertyValue("MetaData", "ImplementationMember", persistentMemberName);

            PutPropertyValue("MetaData", "BackwardCompatibilityID", ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity"));

            PutPropertyValue("MetaData", "AssociationClassRole", (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("Persistent", "Association Class Role"));
            OOAdvantech.MetaDataRepository.Classifier type = Type;
            GetPropertyValue(typeof(string), "MetaData", "MetaObjectID");
            MetaObjectMapper.AddTypeMap(VSUMLAttribute as ModelElement, this);
        }
        /// <MetaDataID>{8775dfa1-6469-467d-9162-7abf68fe9a2c}</MetaDataID>
        public override MetaDataRepository.Classifier Type
        {
            get
            {
                //  if (_Type == null)
                //{
                IType typeClass = VSUMLAttribute.Type;
                if (typeClass is IClassifier)
                {
                    _Type = VisualStudioUMLHelper.GetClassifierFor(typeClass as IClassifier, UMLModel);
                }
                else
                {
                    if (typeClass == null)
                    {
                        _Type = UnknownClassifier.GetClassifier(typeof(void).FullName);
                    }
                    else
                    {
                        _Type = MetaObjectMapper.FindMetaObjectFor(typeClass as ModelElement) as OOAdvantech.MetaDataRepository.Classifier;
                        if (_Type == null)
                            _Type = UnknownClassifier.GetClassifier(typeClass);
                    }
                }

                if (_Type != null)
                {
                    object obj = _Type.FullName;
                }
                //}

                return base.Type;
            }
            set
            {
                if(value!=null)
                    VSUMLAttribute.Type = value.GetUMLType();
              
                base.Type = value;
            }
        }
        /// <MetaDataID>{51509bf7-8d13-40af-8c1b-9537f6ab495a}</MetaDataID>
        public ModelElement ModelElement
        {
            get
            {
                return VSUMLAttribute as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }
        /// <MetaDataID>{87ca37e2-d42c-4617-9807-47566660e3f0}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }
        /// <MetaDataID>{db1a5de7-1526-42c7-a50c-695948fd85b6}</MetaDataID>
        public override string Name
        {
            get
            {
                _Name = VSUMLAttribute.Name;
                return base.Name;
            }
            set
            {
                VSUMLAttribute.Name = value;
                base.Name = value;
            }
        }
    }
}
