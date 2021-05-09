using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Uml.AuxiliaryConstructs;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{51f28b3f-2be9-4af9-ba5b-b99966ed1840}</MetaDataID>
    public class AttributeRealization : OOAdvantech.MetaDataRepository.AttributeRealization, IVSUMLModelItemWrapper
    {

        /// <MetaDataID>{cd961c72-eae0-4c7d-a106-1db8eb28a23c}</MetaDataID>
        protected AttributeRealization()
        {

        }

        public void Refresh()
        {

        }

        /// <MetaDataID>{a5c8310c-3852-493e-8d53-d20228ec43b6}</MetaDataID>
        internal  Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;

        /// <MetaDataID>{c0868d44-3d7c-49a0-be3a-0b96f972f0c4}</MetaDataID>
        internal  IProperty VSUMLAttribute;

        /// <MetaDataID>{91b482c9-33aa-4d7d-a347-7867f72b4aae}</MetaDataID>
        public AttributeRealization(IProperty vsUmlAttribute, OOAdvantech.MetaDataRepository.Classifier owner, Attribute specification,IModel iModel)
            :base(specification)
        {
            VSUMLAttribute = vsUmlAttribute;
            VSUmlModel = iModel;
            _Owner = owner;
            _Name = vsUmlAttribute.Name;
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID((VSUMLAttribute as ModelElement).GetIdentity());

            Visibility = VisualStudioUMLHelper.GetVisibilityKind(VSUMLAttribute.Visibility);

            PutPropertyValue("MetaData", "BackwardCompatibilityID", (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "Identity"));

            string persistentMemberName = (VSUMLAttribute as ModelElement).GetExtensionData().GetPropertyValue("Persistent", "Member that implement");
            if (persistentMemberName == "Auto Generate")
                PutPropertyValue("MetaData", "ImplementationMember", "_" + Name);
            else
                PutPropertyValue("MetaData", "ImplementationMember", persistentMemberName);

            PutPropertyValue("MetaData", "BackwardCompatibilityID", ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity"));
        }


        internal AttributeRealization(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VSUmlModel = iModel;
        }


        /// <MetaDataID>{fa8f5dfd-1e9b-40ea-a6ba-45337be2c946}</MetaDataID>
        public ModelElement ModelElement
        {
            get
            {
                return VSUMLAttribute as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }
        public override void Synchronize(MetaDataRepository.MetaObject originMetaObject)
        {
            MetaDataRepository.Attribute orgAttributeRealization = (originMetaObject as MetaDataRepository.AttributeRealization).Specification;
            if (VSUMLAttribute == null)
            {
                string metaObjectID = GetPropertyValue<string>("MetaData", "MetaObjectID");
                if ((Owner as IVSUMLModelItemWrapper).ModelElement is IClass)
                {
                    VSUMLAttribute = ((Owner as IVSUMLModelItemWrapper).ModelElement as IClass).CreateAttribute();
                    VSUMLAttribute.Name = orgAttributeRealization.Name;
                }
                VSUmlModel = (Owner as IVSUMLModelItemWrapper).UMLModel;
                _Identity = new MetaDataRepository.MetaObjectID(metaObjectID);
                this.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", metaObjectID);
                _Type = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgAttributeRealization.Type.FullName);
                VSUMLAttribute.Type = _Type.GetUMLType();
                VSUMLAttribute.Visibility = VisualStudioUMLHelper.GetVisibilityKind(originMetaObject.Visibility);

                MetaObjectMapper.AddTypeMap(ModelElement, this);

            }
            else
            {
                VSUMLAttribute.Name = orgAttributeRealization.Name;
                _Type = (Owner as IVSUMLModelItemWrapper).UMLModel.GetType(orgAttributeRealization.Type.FullName);
                VSUMLAttribute.Type = _Type.GetUMLType();
                VSUMLAttribute.Visibility = VisualStudioUMLHelper.GetVisibilityKind(originMetaObject.Visibility);
            }
            base.Synchronize(originMetaObject);
        }

        /// <MetaDataID>{5d826e25-d69f-4aa6-ac24-066cc08a5be3}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }

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
                uint nextMemember = Owner.GetNextAutoGenMemberID();
                (VSUMLAttribute as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "Identity", "+" + nextMemember);
                PutPropertyValue("MetaData", "BackwardCompatibilityID", ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity"));
                this.MetaObjectChangeState();
            }
        }
    }
}
