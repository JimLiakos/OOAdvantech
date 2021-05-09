using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Uml.Classes;
using System.Xml.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{9f1157e5-b644-43de-8c92-544411174413}</MetaDataID>
    public static class VisualStudioUMLHelper
    {

        /// <MetaDataID>{63b95150-24c2-401b-a347-cbc27921f65c}</MetaDataID>
        internal static string GetStereotypePropertyValue(this IElement modelElement, string propertyName)
        {
            return "";
        }

        class ModelStoreEvents
        {
           public Delegate ModelElementAdded;
           public Delegate ModelElementDeleted;
        }
        static System.Collections.Generic.Dictionary<Microsoft.VisualStudio.Modeling.Store, ModelStoreEvents> ModelStores = new Dictionary<Store, ModelStoreEvents>();
        public static void SubscribeModelEventsConsumer(Microsoft.VisualStudio.Modeling.Store store)
        {
            //EventHandler<ElementAddedEventArgs>
            if (!ModelStores.ContainsKey(store))
            {
                ModelStores[store] =new ModelStoreEvents();
                ModelStores[store].ModelElementAdded=new EventHandler<ElementAddedEventArgs>(ModelElementAdded);
                ModelStores[store].ModelElementDeleted=new EventHandler<ElementDeletedEventArgs>(ModelElementDeleted);

                store.StoreDisposing += new EventHandler(store_StoreDisposing);
                
                store.EventManagerDirectory.ElementAdded.Add(ModelStores[store].ModelElementAdded);
                store.EventManagerDirectory.ElementDeleted.Add(ModelStores[store].ModelElementDeleted);
            }
        }

        static void store_StoreDisposing(object sender, EventArgs e)
        {
            Microsoft.VisualStudio.Modeling.Store store = sender as Microsoft.VisualStudio.Modeling.Store;
            store.EventManagerDirectory.ElementAdded.Remove(ModelStores[store].ModelElementAdded);
            store.EventManagerDirectory.ElementDeleted.Remove(ModelStores[store].ModelElementDeleted);

            store.StoreDisposing -= new EventHandler(store_StoreDisposing);
        }
        
      /// <summary>
      /// Event handler called whenever a stereotype instance is linked to a uml model element.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private static void ModelElementAdded(object sender, ElementAddedEventArgs e)
        {

        }
        private static void ModelElementDeleted(object sender, ElementDeletedEventArgs e)
        {
            if (e.ModelElement is IGeneralization && e.ModelElement.IsDeleted)
            {

                IVSUMLModelItemWrapper subClass = GetClassifierFor((e.ModelElement as IGeneralization).Source as IClassifier, (e.ModelElement as IGeneralization).Source.Owner as Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel) as IVSUMLModelItemWrapper;
                if (subClass != null)
                    subClass.Refresh();

                IVSUMLModelItemWrapper generalClass = GetClassifierFor((e.ModelElement as IGeneralization).Target as IClassifier, (e.ModelElement as IGeneralization).Source.Owner as Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel) as IVSUMLModelItemWrapper;
                if (generalClass != null)
                    generalClass.Refresh();

                MetaObjectMapper.RemoveMetaObject(generalClass.ModelElement.GetIdentity());
            }

            if (e.ModelElement is IInterfaceRealization && e.ModelElement.IsDeleted)
            {
                IVSUMLModelItemWrapper implementingClass = GetClassifierFor((e.ModelElement as IInterfaceRealization).Source as IClassifier, (e.ModelElement as IInterfaceRealization).Source.Owner as Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel) as IVSUMLModelItemWrapper;
                if (implementingClass != null)
                    implementingClass.Refresh();

                IVSUMLModelItemWrapper _interface = GetClassifierFor((e.ModelElement as IInterfaceRealization).Target as IClassifier, (e.ModelElement as IInterfaceRealization).Source.Owner as Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel) as IVSUMLModelItemWrapper;
                if (_interface != null)
                    _interface.Refresh();

                MetaObjectMapper.RemoveMetaObject(e.ModelElement.GetIdentity());
            }
            if (e.ModelElement is IOperation && e.ModelElement.IsDeleted)
            {
                MetaDataRepository.BehavioralFeature behavioralFeature = MetaObjectMapper.FindMetaObjectFor(e.ModelElement) as MetaDataRepository.BehavioralFeature;
                if (behavioralFeature != null)
                {
                    IVSUMLModelItemWrapper implementingClass = behavioralFeature.Namespace as IVSUMLModelItemWrapper;
                    (behavioralFeature.Namespace as MetaDataRepository.Classifier).RemoveFeature(behavioralFeature);
                    implementingClass.Refresh();
                }
                MetaObjectMapper.RemoveType(e.ModelElement);
            }
            if (e.ModelElement is IProperty && e.ModelElement.IsDeleted)
            {
                MetaDataRepository.StructuralFeature structuralFeature = MetaObjectMapper.FindMetaObjectFor(e.ModelElement) as MetaDataRepository.StructuralFeature;
                if (structuralFeature != null)
                {
                    IVSUMLModelItemWrapper implementingClass = structuralFeature.Namespace as IVSUMLModelItemWrapper;
                    (structuralFeature.Namespace as MetaDataRepository.Classifier).RemoveFeature(structuralFeature);
                    implementingClass.Refresh();
                }
                MetaObjectMapper.RemoveType(e.ModelElement);
            }

        }
        public static IProperty GetVSUmlAttribute(this MetaDataRepository.StructuralFeature structuralFeature)
        {
            if (structuralFeature is Attribute)
                return (structuralFeature as Attribute).VSUMLAttribute;

            if (structuralFeature is AttributeRealization)
                return (structuralFeature as AttributeRealization).VSUMLAttribute;

            return null;
        }

        public static Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel GetModel(this ModelElement modelElement)
        {
            return modelElement.Store.GetModelStore().Root as Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel;
        }

        public static string GetIdentity(this ModelElement modelElement)
        {
            if (modelElement is IInterfaceRealization)
                return ((modelElement as IInterfaceRealization).Source as ModelElement).GetIdentity() + "." + ((modelElement as IInterfaceRealization).Target as ModelElement).GetIdentity();
            if (modelElement is IGeneralization)
                return ((modelElement as IInterfaceRealization).Source as ModelElement).GetIdentity() + "." + ((modelElement as IInterfaceRealization).Target as ModelElement).GetIdentity();


            string metaObjectID = modelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (string.IsNullOrWhiteSpace(metaObjectID))
            {
                modelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", modelElement.Id.ToString());
                metaObjectID = modelElement.Id.ToString();
            }

            return metaObjectID;
        }

        /// <MetaDataID>{b875361d-379f-43b1-974c-8d7d81b5d8aa}</MetaDataID>
        internal static string GetStereotypePropertyValue(this Microsoft.VisualStudio.Modeling.ModelElement modelElement, string propertyName)
        {

            IStereotypeInstance stereotypeInstance = (from appliedStereotype in (modelElement as Microsoft.VisualStudio.Uml.Classes.IElement).AppliedStereotypes
                                                      where appliedStereotype.Profile == "OOAdvantechProfile"

                                                      select appliedStereotype).FirstOrDefault();
            if (stereotypeInstance == null)
            {
                Microsoft.VisualStudio.Uml.Profiles.IStereotype stereotype = (from appliedStereotype in (modelElement as Microsoft.VisualStudio.Uml.Classes.IElement).ApplicableStereotypes
                                                                              where appliedStereotype.Profile.Name == "OOAdvantechProfile"
                                                                              select appliedStereotype).FirstOrDefault();
                if (stereotype != null)
                    stereotypeInstance = (modelElement as Microsoft.VisualStudio.Uml.Classes.IElement).ApplyStereotype(stereotype);

                return "";
            }

            IStereotypePropertyInstance property = (from propertyInstance in stereotypeInstance.PropertyInstances
                                                    where propertyInstance.Name == propertyName
                                                    select propertyInstance).FirstOrDefault();
            if (property != null)
                return property.Value;
            else
                return null;
        }
        /// <MetaDataID>{427f81e6-231d-48d6-b2ea-ff8faf41057d}</MetaDataID>
        internal static int GetNumOfParameters(this Microsoft.VisualStudio.Uml.Classes.IOperation operation)
        {
            return (from operationParameter in operation.OwnedParameters
                    where operationParameter.Direction != ParameterDirectionKind.Return
                    select operationParameter).Count();
        }

        /// <MetaDataID>{dc6d6eb7-4aa8-4a99-82b5-4b26ac92048b}</MetaDataID>
        internal static ExtensionData GetExtensionData(this Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            return new ExtensionData(modelElement.GetExtensionDataDocument(), modelElement);
        }

        /// <MetaDataID>{90a1004f-8556-4b38-b979-131a4615959b}</MetaDataID>
        internal static XDocument GetExtensionDataDocument(this Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            string extensionData = modelElement.GetStereotypePropertyValue("ExtensionData");
            XDocument extensionDataDocument = null;
            if (string.IsNullOrWhiteSpace(extensionData))
            {
                extensionData = "<Main/>";
                extensionDataDocument = XDocument.Parse(extensionData);
                extensionDataDocument.Root.SetAttributeValue("Identity", modelElement.Id.ToString());
                modelElement.SetStereotypePropertyValue("ExtensionData", extensionDataDocument.ToString());
            }
            else
                extensionDataDocument = XDocument.Parse(extensionData);
            return extensionDataDocument;
        }

        /// <MetaDataID>{75e01359-4d79-4f3b-b3ad-cb0c54127138}</MetaDataID>
        internal static void SaveExtensionData(this Microsoft.VisualStudio.Modeling.ModelElement modelElement, XDocument extensionDataDocument)
        {
            modelElement.SetStereotypePropertyValue("ExtensionData", extensionDataDocument.ToString());
        }


        /// <MetaDataID>{66619c2b-a23a-48d3-84db-e52a1f57082d}</MetaDataID>
        internal static void SetStereotypePropertyValue(this Microsoft.VisualStudio.Modeling.ModelElement modelElement, string propertyName, string value)
        {
            IStereotypeInstance stereotypeInstance = (from appliedStereotype in (modelElement as Microsoft.VisualStudio.Uml.Classes.IElement).AppliedStereotypes
                                                      where appliedStereotype.Profile == "OOAdvantechProfile"
                                                      select appliedStereotype).FirstOrDefault();
            if (stereotypeInstance != null)
            {
                IStereotypePropertyInstance property = (from propertyInstance in stereotypeInstance.PropertyInstances
                                                        where propertyInstance.Name == propertyName
                                                        select propertyInstance).FirstOrDefault();
                if (property != null)
                    property.Value = value;
            }
        }
        /// <MetaDataID>{58d4f4dd-232b-476c-9337-dd89963722fd}</MetaDataID>
        internal static System.Collections.Generic.List<MetaDataRepository.Classifier> GetAllClassifiers(this IPackage VSUMLPackage, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel)
        {
            System.Collections.Generic.List<MetaDataRepository.Classifier> classifiers = (from vsUMLClass in VSUMLPackage.Members.OfType<IClassifier>()
                                                                                          from appliedStereotype in vsUMLClass.AppliedStereotypes
                                                                                          where appliedStereotype.Profile == "OOAdvantechProfile" &&
                                                                                          appliedStereotype.Name == "class" || appliedStereotype.Name == "interface" || appliedStereotype.Name == "struct"
                                                                                          select VisualStudioUMLHelper.GetClassifierFor(vsUMLClass, VSUmlModel)).ToList();

            return classifiers;

        }

        /// <MetaDataID>{ad6e8f9a-dff4-4419-bbf0-3cafb6548f9a}</MetaDataID>
        internal static uint GetNextAutoGenMemberID(this MetaDataRepository.Classifier classifier)
        {
            uint lastMemberID = 0;
            foreach (var feature in classifier.Features)
            {
                if (feature is IVSUMLModelItemWrapper)
                {
                    string memberId = (feature as IVSUMLModelItemWrapper).ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity");
                    if (!string.IsNullOrWhiteSpace(memberId))
                    {
                        if (memberId.Trim()[0] == '+')
                        {
                            memberId = memberId.Substring(1);
                            uint num = 0;
                            if (uint.TryParse(memberId, out num))
                                if (num > lastMemberID)
                                    lastMemberID = num;
                        }
                    }

                }
            }
            return lastMemberID + 1;

            //if(classifier is IVSUMLModelItemWrapper)


        }


        internal static MetaDataRepository.Classifier GetType(this Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel vsUmlModel,string typeName)
        {
            MetaDataRepository.Classifier type = (from classifier in vsUmlModel.GetTypes()
                                                  where classifier.FullName == typeName
                                                  select classifier).FirstOrDefault();
            if (type == null)
            {
                type = (from primitive in vsUmlModel.GetTypes().OfType<Primitive>()
                        where primitive.Name == typeName && primitive.ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString()
                        select primitive).FirstOrDefault();


                if (type == null)
                {
                    if (typeName == typeof(int).FullName)
                        typeName = "int";
                    else if (typeName == typeof(uint).FullName)
                        typeName = "uint";
                    else if (typeName == typeof(short).FullName)
                        typeName = "short";
                    else if (typeName == typeof(ushort).FullName)
                        typeName = "ushort";
                    else if (typeName == typeof(long).FullName)
                        typeName = "long";
                    else if (typeName == typeof(ulong).FullName)
                        typeName = "ulong";
                    else if (typeName == typeof(bool).FullName)
                        typeName = "bool";
                    else if (typeName == typeof(string).FullName)
                        typeName = "string";
                    type = (from classifier in vsUmlModel.GetTypes()
                            where classifier.FullName == typeName
                            select classifier).FirstOrDefault();
                    if (type == null)
                    {
                        type = (from primitive in vsUmlModel.GetTypes().OfType<Primitive>()
                                where primitive.Name == typeName && primitive.ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString()
                                select primitive).FirstOrDefault();
                    }

                }

                if (type == null)
                {
                    IPrimitiveType primitiveType = vsUmlModel.CreatePrimitiveType();
                    (primitiveType as ModelElement).GetExtensionData().SetPropertyValue("UnspecifiedType", true.ToString());
                    primitiveType.Name = typeName;
                    type = VisualStudioUMLHelper.GetClassifierFor(primitiveType, vsUmlModel);
                }

            }
            return type;

        }

        /// <MetaDataID>{f6d53d76-0ba3-4758-b5c8-1e270156e3ee}</MetaDataID>
        internal static IType GetUMLType(this MetaDataRepository.Classifier classifier)
        {

            if (classifier is IVSUMLModelItemWrapper)
            {
                if ((classifier as IVSUMLModelItemWrapper).ModelElement is IClassifier)
                    return (classifier as IVSUMLModelItemWrapper).ModelElement as IClassifier;
            }
            if (classifier is UnknownClassifier)
                return (classifier as UnknownClassifier).Type;
            return null;
        }


        /// <MetaDataID>{d9123cb0-f141-418b-addf-3781820ef1c7}</MetaDataID>
        internal static System.Collections.Generic.List<MetaDataRepository.Classifier> GetTypes(this Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel)
        {

            System.Collections.Generic.List<MetaDataRepository.Classifier> classifiers = (from vsUMLClass in VSUmlModel.Members.OfType<IClassifier>()
                                                                                          where (vsUMLClass is IClass || vsUMLClass is IInterface || vsUMLClass is IPrimitiveType) && VisualStudioUMLHelper.GetClassifierFor(vsUMLClass, VSUmlModel) != null
                                                                                          select VisualStudioUMLHelper.GetClassifierFor(vsUMLClass, VSUmlModel)).ToList();

            classifiers.AddRange((from member in VSUmlModel.Members
                                  where member.GetType().Name == "UndefinedType"
                                  select UnknownClassifier.GetClassifier(member as IType)));

            return classifiers;

        }
        /// <MetaDataID>{988d7373-84d2-4241-bc69-0b54f12c8aef}</MetaDataID>
        internal static System.Collections.Generic.List<MetaDataRepository.Classifier> GetAllClassifiers(this Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel)
        {


            System.Collections.Generic.List<MetaDataRepository.Classifier> classifiers = (from vsUMLClass in VSUmlModel.Members.OfType<IClassifier>()
                                                                                          from appliedStereotype in vsUMLClass.AppliedStereotypes
                                                                                          where appliedStereotype.Profile == "OOAdvantechProfile" &&
                                                                                          appliedStereotype.Name == "class" || appliedStereotype.Name == "interface" || appliedStereotype.Name == "struct"
                                                                                          select VisualStudioUMLHelper.GetClassifierFor(vsUMLClass, VSUmlModel)).ToList();

            foreach (IPackage package in VSUmlModel.Members.OfType<IPackage>())
                classifiers.AddRange(package.GetAllClassifiers(VSUmlModel));
            return classifiers;
        }
        /// <MetaDataID>{19183dbb-c66c-4502-a5b5-f9c71e7b4926}</MetaDataID>
        internal static bool HasStereotypePropertyValue(this Microsoft.VisualStudio.Modeling.ModelElement modelElement, string propertyName)
        {


            IStereotypeInstance stereotypeInstance = (from appliedStereotype in (modelElement as Microsoft.VisualStudio.Uml.Classes.IElement).AppliedStereotypes
                                                      where appliedStereotype.Profile == "OOAdvantechProfile"
                                                      select appliedStereotype).FirstOrDefault();
            if (stereotypeInstance == null)
                return false;

            IStereotypePropertyInstance property = (from propertyInstance in stereotypeInstance.PropertyInstances
                                                    where propertyInstance.Name == propertyName
                                                    select propertyInstance).FirstOrDefault();
            if (property != null)
                return true;
            else
                return false;


        }
        /// <MetaDataID>{519f052e-e335-4704-a058-d157b0c771c4}</MetaDataID>
        public static Package GetPackageFor(Microsoft.VisualStudio.Uml.Classes.IPackage vsPackage, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {

            if (vsPackage == null)
                return null;
            Package package = MetaObjectMapper.FindMetaObjectFor(vsPackage as Microsoft.VisualStudio.Modeling.ModelElement) as Package;

            if (package == null)
                package = new Package(vsPackage, iModel);

            return package;
        }


        /// <MetaDataID>{a05527b9-45b6-4258-a28b-d04d3b8de576}</MetaDataID>
        public static Component GetComponentFor(Microsoft.VisualStudio.Uml.Components.IComponent vsUmlComponent, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            if (vsUmlComponent == null)
                return null;
            //if ((vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString() != (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).GetStereotypePropertyValue("Identity") &&
            //    !string.IsNullOrWhiteSpace((vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).GetStereotypePropertyValue("Identity")))
            //{
            XDocument xDocument = (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).GetExtensionDataDocument();
            if (xDocument.Root.Attribute("Identity") == null)
            {
                xDocument.Root.SetAttributeValue("Identity", (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString());
                (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).SaveExtensionData(xDocument);
            }

            if (xDocument.Root.Attribute("Identity").Value != (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString())
            {
                if (xDocument.Root.Element("AssignedClasses") != null)
                {
                    xDocument.Root.Element("AssignedClasses").Remove();
                    xDocument.Root.Add(XElement.Parse("<AssignedClasses/>"));
                }
                xDocument.Root.SetAttributeValue("Identity", (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString());
                (vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement).SaveExtensionData(xDocument);
            }

            //}


            Component _component = MetaObjectMapper.FindMetaObjectFor(vsUmlComponent as Microsoft.VisualStudio.Modeling.ModelElement) as Component;

            if (_component == null)
                _component = new Component(vsUmlComponent, iModel);
            return _component;
        }

        /// <MetaDataID>{4857df9e-5b28-43e8-9ce7-29784ea16b0c}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(IType typeClass, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            OOAdvantech.MetaDataRepository.Classifier classifier = null;
            if (typeClass is IClassifier)
            {
                classifier = VisualStudioUMLHelper.GetClassifierFor(typeClass as IClassifier, iModel);
            }
            else
            {
                if (typeClass != null)
                {
                    classifier = MetaObjectMapper.FindMetaObjectFor(typeClass as ModelElement) as OOAdvantech.MetaDataRepository.Classifier;
                    if (classifier == null)
                        classifier = UnknownClassifier.GetClassifier(typeClass);
                }
            }
            return classifier;
        }

        /// <MetaDataID>{f7adbb1e-1f6d-4dc4-a4fa-dd1593b44334}</MetaDataID>
        public static Association GetAssociationFor(IAssociation vsUMLAssociation, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            Association association = MetaObjectMapper.FindMetaObjectFor(vsUMLAssociation as Microsoft.VisualStudio.Modeling.ModelElement) as Association;
            if (association == null)
            {
                MetaDataRepository.Classifier roleAClassifier = VisualStudioUMLHelper.GetClassifierFor(vsUMLAssociation.TargetElement as IClassifier, iModel);
                MetaDataRepository.Classifier roleBClassifier = VisualStudioUMLHelper.GetClassifierFor(vsUMLAssociation.SourceElement as IClassifier, iModel);
                IProperty vsUMLRoleBAssociationEnd = vsUMLAssociation.OwnedEnds.ToArray()[0];
                IProperty vsUMLRoleAAssociationEnd = vsUMLAssociation.OwnedEnds.ToArray()[1];


                AssociationEnd roleA = new AssociationEnd(vsUMLRoleAAssociationEnd, roleAClassifier, roleBClassifier, MetaDataRepository.Roles.RoleA, iModel);
                AssociationEnd roleB = new AssociationEnd(vsUMLRoleBAssociationEnd, roleBClassifier, roleAClassifier, MetaDataRepository.Roles.RoleB, iModel);

                return new Association(vsUMLAssociation, roleA, roleB, iModel);
            }
            else
                return association;

        }
        /// <MetaDataID>{71903e30-b72e-4677-ae16-6461a297bf38}</MetaDataID>
        public static MetaDataRepository.Classifier GetClassifierFor(IClassifier vsUmlClassifier, Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel iModel)
        {
            if (vsUmlClassifier == null)
                return null;

            SubscribeModelEventsConsumer((vsUmlClassifier as ModelElement).Store);
            XDocument xDocument = (vsUmlClassifier as Microsoft.VisualStudio.Modeling.ModelElement).GetExtensionDataDocument();
            if (xDocument.Root.Attribute("Identity") == null)
            {
                xDocument.Root.SetAttributeValue("Identity", (vsUmlClassifier as Microsoft.VisualStudio.Modeling.ModelElement).Id.ToString());
                (vsUmlClassifier as Microsoft.VisualStudio.Modeling.ModelElement).SaveExtensionData(xDocument);
            }

            MetaDataRepository.Classifier _classifier = MetaObjectMapper.FindMetaObjectFor(vsUmlClassifier as Microsoft.VisualStudio.Modeling.ModelElement) as MetaDataRepository.Classifier;
            if (_classifier == null)
            {
                if (vsUmlClassifier is IClass)
                {
                    var stereotype = (from appliedStereotype in (vsUmlClassifier as Microsoft.VisualStudio.Uml.Classes.IElement).AppliedStereotypes
                                      where appliedStereotype.Profile == "OOAdvantechProfile"
                                      select appliedStereotype).FirstOrDefault();
                    if (stereotype != null)
                    {
                        if (stereotype.Name == "class")
                            _classifier = new Class(vsUmlClassifier as IClass, iModel);
                        if (stereotype.Name == "struct")
                            _classifier = new Structure(vsUmlClassifier as IClass, iModel);
                    }
                }
                if (vsUmlClassifier is IInterface)
                    _classifier = new Interface(vsUmlClassifier as IInterface, iModel);

                if (vsUmlClassifier is IPrimitiveType)
                    _classifier = new Primitive(vsUmlClassifier as IPrimitiveType, iModel);


            }
            return _classifier;
        }


        /// <MetaDataID>{a4db9f60-8271-4795-a490-9ccdd6bfaa80}</MetaDataID>
        internal static MetaDataRepository.VisibilityKind GetVisibilityKind(VisibilityKind visibilityKind)
        {
            switch (visibilityKind)
            {
                case VisibilityKind.Public:
                    {
                        return MetaDataRepository.VisibilityKind.AccessPublic;
                    }
                case VisibilityKind.Protected:
                    {
                        return MetaDataRepository.VisibilityKind.AccessProtected;
                    }
                case VisibilityKind.Private:
                    {
                        return MetaDataRepository.VisibilityKind.AccessPrivate;
                    }
                case VisibilityKind.Package:
                    {
                        return MetaDataRepository.VisibilityKind.AccessComponent;
                    }
                default:
                    {
                        return MetaDataRepository.VisibilityKind.AccessPublic;
                    }
            }

        }


        internal static VisibilityKind GetVisibilityKind(MetaDataRepository.VisibilityKind visibilityKind)
        {
            switch (visibilityKind)
            {
                case MetaDataRepository.VisibilityKind.AccessPublic:
                    {
                        return VisibilityKind.Public;
                    }
                case MetaDataRepository.VisibilityKind.AccessProtected:
                    {
                        return VisibilityKind.Protected;
                    }
                case MetaDataRepository.VisibilityKind.AccessPrivate:
                    {
                        return VisibilityKind.Private;
                    }
                case MetaDataRepository.VisibilityKind.AccessComponent:
                    {
                        return VisibilityKind.Package;
                    }
                default:
                    {
                        return VisibilityKind.Public;
                    }
            }

        }


    }

    /// <MetaDataID>{fa7db039-6890-43f4-a197-eea78cf3db07}</MetaDataID>
    public class ExtensionData
    {
        /// <MetaDataID>{32905cc9-4b87-4aca-b435-fb463c515871}</MetaDataID>
        XDocument ExtensionDataDocument;
        /// <MetaDataID>{4a9ccf9f-ab4d-4395-913d-d4be137d5b37}</MetaDataID>
        Microsoft.VisualStudio.Modeling.ModelElement ModelElement;
        /// <MetaDataID>{41677416-ec85-4c4f-90aa-00413a266a06}</MetaDataID>
        public ExtensionData(XDocument extensionDataDocument, Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            ModelElement = modelElement;
            ExtensionDataDocument = extensionDataDocument;
        }
        /// <MetaDataID>{c469b655-7148-4259-a1e2-0eb0c9582cea}</MetaDataID>
        public string GetPropertyValue(string propertyName)
        {

            if (ExtensionDataDocument.Root.Element("Properties") != null &&
                ExtensionDataDocument.Root.Element("Properties").Attribute(propertyName) != null)
                return ExtensionDataDocument.Root.Element("Properties").Attribute(propertyName).Value;
            else
                return "";
        }
        /// <MetaDataID>{98f237e9-b0b2-4fdb-9669-a1c7413618a0}</MetaDataID>
        public void SetPropertyValue(string propertyName, string value)
        {
            if (ExtensionDataDocument.Root.Element("Properties") == null)
                ExtensionDataDocument.Root.Add(XElement.Parse("<Properties/>"));
            ExtensionDataDocument.Root.Element("Properties").SetAttributeValue(propertyName, value);
            ModelElement.SaveExtensionData(ExtensionDataDocument);
        }
        /// <MetaDataID>{92cc9289-bd57-453a-9e83-138c99de239a}</MetaDataID>
        public string GetPropertyValue(string propertyGroup, string propertyName)
        {
            if (ExtensionDataDocument.Root.Element("Properties") != null &&
                ExtensionDataDocument.Root.Element("Properties").Element(propertyGroup) != null &&
                ExtensionDataDocument.Root.Element("Properties").Element(propertyGroup).Attribute(propertyName) != null)
                return ExtensionDataDocument.Root.Element("Properties").Element(propertyGroup).Attribute(propertyName).Value;
            return "";
        }

        /// <MetaDataID>{dafd9870-2ae3-4ac9-80b8-5d5c9a3b9202}</MetaDataID>
        public void SetPropertyValue(string propertyGroup, string propertyName, string value)
        {
            if (ExtensionDataDocument.Root.Element("Properties") == null)
                ExtensionDataDocument.Root.Add(new XElement("Properties"));

            if (ExtensionDataDocument.Root.Element("Properties").Element(propertyGroup) == null)
                ExtensionDataDocument.Root.Element("Properties").Add(new XElement(
                    propertyGroup));

            ExtensionDataDocument.Root.Element("Properties").Element(propertyGroup).SetAttributeValue(propertyName, value);
            ModelElement.SaveExtensionData(ExtensionDataDocument);

        }


    }
}
