using System.Linq;
using Microsoft.VisualStudio.Uml.Classes;
using System.Xml.Linq;
using OOAdvantech.Collections.Generic;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{adc25ead-4469-49c8-ae7f-2871c3f278e3}</MetaDataID>
    public class Component : OOAdvantech.MetaDataRepository.Component, IVSUMLModelItemWrapper
    {

        protected Component()
        {

        }

        /// <MetaDataID>{fc882f89-360d-4f0c-a131-4114050e19c0}</MetaDataID>
        public readonly Microsoft.VisualStudio.Uml.Components.IComponent VsUmlComponent;
        /// <MetaDataID>{f1099d60-6f7e-4193-b23d-3c949ad0a222}</MetaDataID>
        public readonly Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        /// <MetaDataID>{5547eead-971f-4885-8bd8-1df01847783d}</MetaDataID>
        public Component(Microsoft.VisualStudio.Uml.Components.IComponent component, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            VsUmlComponent = component;
            _Name = component.Name;
            VSUmlModel = iModel;
            MetaObjectMapper.AddTypeMap(VsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement, this);
        }
        /// <MetaDataID>{a6dcaee6-f555-4033-9816-aae3e5b5a5ed}</MetaDataID>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                VsUmlComponent.Name = Name;
            }
        }
        /// <MetaDataID>{7aa7bc73-f2fb-48be-bb73-45a6a6d42f9f}</MetaDataID>
        public override void AddResident(MetaDataRepository.MetaObject metaObject)
        {
            string modelElementId = (metaObject as IVSUMLModelItemWrapper).ModelElement.Id.ToString();
            if (AssignedClassesElement.Element("ID_" + modelElementId) == null)
            {
                AssignedClassesElement.Add(XElement.Parse(string.Format("<{0}/>", "ID_" + modelElementId)));
                SaveExtensionData();
            }

            base.AddResident(metaObject);
        }
        /// <MetaDataID>{6f99023a-c7e6-4c2b-9ee9-6d8a57f5b61e}</MetaDataID>
        public override void RemoveResident(MetaDataRepository.MetaObject metaObject)
        {
            string modelElementId = (metaObject as IVSUMLModelItemWrapper).ModelElement.Id.ToString();
            if (AssignedClassesElement.Element("ID_" + modelElementId) != null)
            {
                AssignedClassesElement.Element("ID_" + modelElementId).Remove();
                SaveExtensionData();
            }
            base.RemoveResident(metaObject);
        }





        /// <MetaDataID>{1a18a6ea-ddcc-4e52-8996-5ff485fc855a}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel UMLModel
        {
            get
            {
                return VSUmlModel;
            }
        }


        /// <MetaDataID>{00e2330c-a1dc-4947-ac5c-d1f7186339b4}</MetaDataID>
        public CodeMetaDataRepository.Project AssignedProject
        {
            get
            {
                string assignedProjectIdentity = null;
                if (ExtensionDataDocument.Root.Attribute("AssignedProjectIdentity") != null)
                {
                    assignedProjectIdentity = ExtensionDataDocument.Root.Attribute("AssignedProjectIdentity").Value;
                    if (assignedProjectIdentity == null)
                        return null;
                    CodeMetaDataRepository.Project project=(from slnProject in new CodeMetaDataRepository.IDEManager().Solution.Projects
                                                            where slnProject.Identity.ToString() == assignedProjectIdentity
                                                            select slnProject).FirstOrDefault();
                    if(project!=null)
                        PutPropertyValue("MetaData", "MetaObjectID", project.Identity.ToString());
                    return project;
                }
                else
                    return null;
            }
            set
            {
                string assignedProjectIdentity = null;
                if(value==null)
                    ExtensionDataDocument.Root.SetAttributeValue("AssignedProjectIdentity", "");
                else
                    ExtensionDataDocument.Root.SetAttributeValue("AssignedProjectIdentity", value.Identity);
                SaveExtensionData();
            }

        }


        /// <MetaDataID>{e043164b-5805-4f2d-b384-72bec51570fa}</MetaDataID>
        void SaveExtensionData()
        {
            ExtensionDataProperty.Value = ExtensionDataDocument.ToString();
        }
        /// <MetaDataID>{d6a55e51-214e-4062-95bf-2278ec897387}</MetaDataID>
        XElement AssignedClassesElement
        {
            get
            {
                if (ExtensionDataDocument.Root.Element("AssignedClasses") == null)
                    ExtensionDataDocument.Root.Add(XElement.Parse("<AssignedClasses/>"));
                return ExtensionDataDocument.Root.Element("AssignedClasses");
            }
        }
        /// <MetaDataID>{9f53b99d-59fd-4662-bc6c-cff540f418b3}</MetaDataID>
        System.Collections.Generic.List<string> AsignedClassesIdentities
        {
            get
            {
                return (from assignedClassElement in AssignedClassesElement.Elements()
                        select assignedClassElement.Name.ToString()).ToList();
            }
        }
        /// <MetaDataID>{c2f4681d-65e4-4d11-88c1-1587d03ebd35}</MetaDataID>
        IStereotypePropertyInstance ExtensionDataProperty
        {
            get
            {
                IStereotypeInstance stereotypeInstance = (from appliedStereotype in (ModelElement as Microsoft.VisualStudio.Uml.Classes.IElement).AppliedStereotypes
                                                          where appliedStereotype.Profile == "OOAdvantechProfile"
                                                          select appliedStereotype).FirstOrDefault();
                IStereotypePropertyInstance property = (from propertyInstance in stereotypeInstance.PropertyInstances
                                                        where propertyInstance.Name == "ExtensionData"
                                                        select propertyInstance).FirstOrDefault();
                return property;
            }
        }

        /// <MetaDataID>{7011d4b9-424c-429a-a440-889b5fe5209e}</MetaDataID>
        XDocument _ExtensionDataDocument;
        /// <MetaDataID>{e6e82742-0da2-4b98-af0b-c66c8e31cba7}</MetaDataID>
        XDocument ExtensionDataDocument
        {
            get
            {
                if (_ExtensionDataDocument == null)
                {
                    string extensionData = ExtensionDataProperty.Value;
                    if (string.IsNullOrWhiteSpace(extensionData))
                        extensionData = "<Main/>";
                    _ExtensionDataDocument = XDocument.Parse(extensionData);
                }
                return _ExtensionDataDocument;
            }
        }


        /// <MetaDataID>{fa2bcb13-f254-4ac0-9aad-0b3e09d88d13}</MetaDataID>
        public Microsoft.VisualStudio.Modeling.ModelElement ModelElement
        {
            get
            {
                return VsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }


        /// <MetaDataID>{5a8e7109-8e27-4e8f-8bbc-5022ed1ee414}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.MetaObject> Residents
        {
            get
            {

                System.Collections.Generic.List<MetaDataRepository.Classifier> classes = VSUmlModel.GetAllClassifiers();
                    //(from vsUMLClass in VSUmlModel.GetAllClassifiers()
                    //                                                                      select VisualStudioUMLHelper.GetClassifierFor(vsUMLClass, VSUmlModel)).ToList();

                foreach (var @class in classes)
                {

                    if (AsignedClassesIdentities.Contains("ID_" + (@class as IVSUMLModelItemWrapper).ModelElement.Id.ToString()))
                        if (!_Residents.Contains(@class))
                            _Residents.Add(@class);
                }

                return base.Residents;
            }
        }

    }
}
