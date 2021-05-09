using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.UserInterface.Runtime;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
//usinf ech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Classifier>
//sd
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{28afaa95-7406-4b04-b756-eeb28e0babfb}</MetaDataID>
    public class AttributePresentationObject : PresentationObject<MetaDataRepository.StructuralFeature>
    {

        /// <MetaDataID>{a71abf94-4367-4fc9-82b0-c31cc56597aa}</MetaDataID>
        Attribute Attribute;
        /// <MetaDataID>{09cc7b16-c9f2-4c83-abb3-960f6e2b17cb}</MetaDataID>
        public AttributePresentationObject(MetaDataRepository.StructuralFeature attribute)
            : base(attribute)
        {
            Attribute = attribute as Attribute;


            attribute.Changed += new MetaDataRepository.MetaObjectChangedEventHandler(attribute_Changed);


            RefreshPersistentMember();
        }
        /// <MetaDataID>{15738728-f8b4-487a-837d-870268317054}</MetaDataID>
        public override void FormClosed()
        {
            base.FormClosed();
            RealObject.Changed -= new MetaDataRepository.MetaObjectChangedEventHandler(attribute_Changed);
        }

        /// <MetaDataID>{0504c2db-c016-4c74-88bf-fb3952ea2224}</MetaDataID>
        void attribute_Changed(object sender)
        {
            if (ObjectChangeState != null)
                ObjectChangeState(this, null);
        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{f20a79e0-ea2c-4050-b090-ac68a32dd109}</MetaDataID>
        public bool Persistent
        {
            get
            {
                if (Attribute.Persistent && string.IsNullOrWhiteSpace(BackwardCompatibilityID))
                    Attribute.GenerateBackwardCompatibilityID();

                return Attribute.Persistent;
            }
            set
            {
                Attribute.Persistent = value;

                if (Attribute.Persistent && string.IsNullOrWhiteSpace(BackwardCompatibilityID))
                    Attribute.GenerateBackwardCompatibilityID();

                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }
        /// <MetaDataID>{963531bd-ba7c-48e8-83bf-c3fe0a445ce2}</MetaDataID>
        public String ImplementetionField
        {
            get
            {
                if (AutoGenerate)
                    return "Auto Generate";

                return Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "ImplementetionField");
            }
            set
            {
                if (value != "Auto Generate")
                {
                    Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "ImplementetionField", value);
                    RefreshPersistentMember();
                }
            }
        }

        /// <MetaDataID>{09360ddd-20e3-4d6b-844c-e34ca7ad2338}</MetaDataID>
        public string NewType(string typeName)
        {
            return typeName;
        }
        /// <MetaDataID>{4b048c0c-c298-4e96-9d28-f335b66e7da5}</MetaDataID>
        public string Type
        {
            get
            {
                if (RealObject.Type is Primitive && (RealObject.Type as Primitive).ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString())
                    return RealObject.Type.Name;

                if (RealObject.Type.FullName == typeof(void).FullName)
                    return "";

                return RealObject.Type.FullName;
            }
            set
            {

                MetaDataRepository.Classifier type = (from classifier in Attribute.UMLModel.GetTypes()
                                                            where classifier.FullName == value
                                                            select classifier).FirstOrDefault();
                if (type == null)
                {
                    type = (from primitive in Attribute.UMLModel.GetTypes().OfType<Primitive>()
                                  where primitive.Name == value && primitive.ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString()
                                  select primitive).FirstOrDefault();
                    if (type == null)
                    {
                        IPrimitiveType primitiveType = Attribute.UMLModel.CreatePrimitiveType();
                        (primitiveType as ModelElement).GetExtensionData().SetPropertyValue("UnspecifiedType", true.ToString());
                        primitiveType.Name = value;
                        type = VisualStudioUMLHelper.GetClassifierFor(primitiveType, Attribute.UMLModel);
                    }

                }
                RealObject.Type = type;

            }
        }
        /// <MetaDataID>{ea85f81a-ac33-4f28-8e6f-745aa9b6bd59}</MetaDataID>
        public System.Collections.Generic.List<string> Types
        {
            get
            {
                List<string> typeNames = new List<string>();
                foreach (var classifier in Attribute.UMLModel.GetTypes())
                {

                    if (classifier is Primitive)
                    {
                        if ((classifier as Primitive).ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString())
                            typeNames.Add(classifier.Name);
                        else
                            typeNames.Add(classifier.FullName);
                    }
                    else
                        typeNames.Add(classifier.FullName);
                }
                return typeNames;
            }
        }

        /// <MetaDataID>{08e1ba74-c4a7-476d-a778-a9d09522cd8c}</MetaDataID>
        public bool IsPersistentProperty
        {
            get
            {
                if (IsProperty && Persistent)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{0f2265d1-d7f5-4075-a2d8-e2e2078e9868}</MetaDataID>
        public bool EditImplementetionField
        {
            get
            {
                if (!AutoGenerate && IsProperty && Persistent)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{d53b11bc-abb3-4a89-813e-fa74b83e2749}</MetaDataID>
        public string BackwardCompatibilityID
        {

            get
            {
                return Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity");
            }
            set
            {

                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Identity", value);
                RealObject.PutPropertyValue("MetaData", "BackwardCompatibilityID", Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Identity"));
            }

        }

        /// <MetaDataID>{3a4f0e16-a746-4389-ad72-e97b2cfaa54a}</MetaDataID>
        public uint NextMemberID
        {
            get
            {
                string lastMemberID = (Attribute.VSUMLAttribute.Owner as ModelElement).GetExtensionData().GetPropertyValue("MetaData", "LastMemberID");
                if (string.IsNullOrWhiteSpace(lastMemberID))
                    return 1;
                else
                    return uint.Parse(lastMemberID);

            }
            set
            {
                (Attribute.VSUMLAttribute.Owner as ModelElement).GetExtensionData().SetPropertyValue("MetaData", "LastMemberID", value.ToString());
            }
        }

        /// <MetaDataID>{ef4b0d74-9464-46b2-a741-d0f2113e9ddf}</MetaDataID>
        public void GenerateBackwardCompatibilityID()
        {
            if (Attribute.Persistent && string.IsNullOrWhiteSpace(BackwardCompatibilityID))
                Attribute.GenerateBackwardCompatibilityID();

            if (ObjectChangeState != null)
                ObjectChangeState(this, null);

        }


        /// <MetaDataID>{5570e031-4aab-4712-bf83-cd8d17131f6d}</MetaDataID>
        public bool AutoGenerate
        {
            get
            {
                string autoGen = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "AutoGenerate");
                if (string.IsNullOrWhiteSpace(autoGen))
                    return true;
                else
                    return bool.Parse(autoGen);
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AutoGenerate", value.ToString());
                RefreshPersistentMember();
                if (ObjectChangeState != null)
                {
                    ObjectChangeState(this, "EditImplementetionField");
                    ObjectChangeState(this, "ImplementetionField");
                }
                RefreshPersistentMember();
            }
        }

        /// <MetaDataID>{65ad6574-49c3-41d4-831f-867a27dd316b}</MetaDataID>
        public bool HasBody
        {
            get
            {

                string hasBody = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "PropertyHasBody");
                if (string.IsNullOrWhiteSpace(hasBody))
                    return false;
                else
                    return bool.Parse(hasBody);
            }
            set
            {
                Attribute.PutPropertyValue("MetaData", "PropertyHasBody", value.ToString());
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "PropertyHasBody", value.ToString());
            }
        }

        /// <exclude>Excluded</exclude>
        PropertyMethodKind _GetPropertyMethod;
        /// <MetaDataID>{7fceb1ef-3ea3-4cc6-a7ec-e0cb94de1d2b}</MetaDataID>
        public PropertyMethodKind GetPropertyMethod
        {
            get
            {
                string getterVisibility = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "GetterVisibility");
                if (string.IsNullOrWhiteSpace(getterVisibility))
                    return PropertyMethodKind.None;
                else
                    return (PropertyMethodKind)int.Parse(getterVisibility);
            }
            set
            {

                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "GetterVisibility", ((int)value).ToString());
                if (value == PropertyMethodKind.None)
                    RealObject.PutPropertyValue("MetaData", "Getter", false);
                else
                {
                    RealObject.PutPropertyValue("MetaData", "Getter", true);
                    switch (_GetPropertyMethod)
                    {
                        case PropertyMethodKind.Public:
                            RealObject.PutPropertyValue("MetaData", "GetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessPublic).ToString());
                            break;
                        case PropertyMethodKind.Protected:
                            RealObject.PutPropertyValue("MetaData", "GetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessProtected).ToString());
                            break;
                        case PropertyMethodKind.Private:
                            RealObject.PutPropertyValue("MetaData", "GetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessPrivate).ToString());
                            break;
                        case PropertyMethodKind.Package:
                            RealObject.PutPropertyValue("MetaData", "GetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessComponent).ToString());
                            break;
                        case PropertyMethodKind.ProtectedInternal:
                            RealObject.PutPropertyValue("MetaData", "GetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessComponentOrProtected).ToString());
                            break;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        PropertyMethodKind _SetPropertyMethod;
        /// <MetaDataID>{8e05167f-41cf-440e-bd8f-c9abb8b6e72c}</MetaDataID>
        public PropertyMethodKind SetPropertyMethod
        {
            get
            {
                string setterVisibility = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "SetterVisibility");
                if (string.IsNullOrWhiteSpace(setterVisibility))
                    return PropertyMethodKind.None;
                else
                    return (PropertyMethodKind)int.Parse(setterVisibility);
            }
            set
            {
                _SetPropertyMethod = value;
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "SetterVisibility", ((int)value).ToString());
                if (value == PropertyMethodKind.None)
                    RealObject.PutPropertyValue("MetaData", "Setter", false);
                else
                {
                    RealObject.PutPropertyValue("MetaData", "Setter", true);

                    switch (_SetPropertyMethod)
                    {
                        case PropertyMethodKind.Public:
                            RealObject.PutPropertyValue("MetaData", "SetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessPublic).ToString());
                            break;
                        case PropertyMethodKind.Protected:
                            RealObject.PutPropertyValue("MetaData", "SetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessProtected).ToString());
                            break;
                        case PropertyMethodKind.Private:
                            RealObject.PutPropertyValue("MetaData", "SetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessPrivate).ToString());
                            break;
                        case PropertyMethodKind.Package:
                            RealObject.PutPropertyValue("MetaData", "SetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessComponent).ToString());
                            break;
                        case PropertyMethodKind.ProtectedInternal:
                            RealObject.PutPropertyValue("MetaData", "SetterVisibility", ((int)MetaDataRepository.VisibilityKind.AccessComponentOrProtected).ToString());
                            break;
                    }
                }
            }
        }

        /// <MetaDataID>{1f614e86-2128-459c-a0ee-8aa176e5e2be}</MetaDataID>
        public bool Public
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility == VisibilityKind.Public;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility = VisibilityKind.Public;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }

        /// <MetaDataID>{0642ce9b-ae59-4e80-9e9c-70d57119b7f9}</MetaDataID>
        public bool Private
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility == VisibilityKind.Private;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility = VisibilityKind.Private;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }


        /// <MetaDataID>{8bd91f25-ec54-4c09-a56c-d2e56f4c2614}</MetaDataID>
        public bool Protected
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility == VisibilityKind.Protected;
            }
            set
            {
                if (value)
                {

                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility = VisibilityKind.Protected;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }

        /// <MetaDataID>{b8479773-eeac-486a-865d-b52934943e0d}</MetaDataID>
        public bool Package
        {
            get
            {

                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility == VisibilityKind.Package;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IProperty).Visibility = VisibilityKind.Package;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessComponent;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }

        /// <MetaDataID>{7d6adb61-2903-4a63-bbbc-8c63b8840bb6}</MetaDataID>
        void RefreshPersistentMember()
        {
            if (IsPersistentProperty)
            {
                if (AutoGenerate)
                    RealObject.PutPropertyValue("MetaData", "ImplementationMember", "_" + RealObject.Name);
                else
                    RealObject.PutPropertyValue("MetaData", "ImplementationMember", ImplementetionField);
            }
        }

        /// <MetaDataID>{ef59fcdc-c969-468e-bbc6-b60fccf26109}</MetaDataID>
        public bool CanBeAbstract
        {
            get
            {
                return IsProperty && (RealObject.Owner is MetaDataRepository.Class) && (RealObject.Owner as MetaDataRepository.Class).Abstract;
            }
        }
        /// <MetaDataID>{97fa042a-0794-4c53-b53d-b5b7668d5c29}</MetaDataID>
        public bool CanChangePropertyValue
        {
            get
            {
                if (RealObject.Owner is MetaDataRepository.Interface && !IsEvent)
                    return false;
                else
                    return true;
            }
        }
        /// <MetaDataID>{0ddd276e-e302-4fa1-8aa7-c3079b373559}</MetaDataID>
        public bool CanChangeEventValue
        {
            get
            {
                if (RealObject.Owner is MetaDataRepository.Interface && !IsProperty)
                    return false;
                else
                    return true;
            }
        }

        /// <MetaDataID>{526560bc-609a-4f88-9a1b-2403b653629b}</MetaDataID>
        public bool CanBePersistent
        {
            get
            {
                if (!IsEvent&&!IsStatic)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{55c16cde-0202-4cf3-bdd9-8b9c0561bf95}</MetaDataID>
        public bool IsEvent
        {
            get
            {

                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "AsEvent");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;

            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AsEvent", value.ToString());
                Attribute.PutPropertyValue("MetaData", "AsEvent", value);

                if (!value)
                    IsNone = true;

                if (value)
                {
                    IsProperty = false;
                    Persistent = false;
                }
                
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }

        }

        /// <MetaDataID>{41c020f1-adba-47d2-897d-51bc56c260d4}</MetaDataID>
        public bool IsProperty
        {
            get
            {
                //if (RealObject.Owner is MetaDataRepository.Interface)
                //{
                //    RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AsProperty", true.ToString());
                //    RealObject.PutPropertyValue("MetaData", "AsProperty", true);
                //    return true;
                //}

                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "AsProperty");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;

            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AsProperty", value.ToString());
                Attribute.PutPropertyValue("MetaData", "AsProperty", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
                if (!value)
                    IsNone = true;
                if (value)
                    IsEvent = false;

            }

        }

        /// <MetaDataID>{45622f0f-6f74-4679-99fb-02ea8d769ed1}</MetaDataID>
        public bool IsAbstract
        {
            get
            {
                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Abstract");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Abstract", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Abstract", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsAbstract");
            }
        }

        /// <MetaDataID>{be9eaae6-82c4-4d90-84cc-31bfd4911a71}</MetaDataID>
        public bool IsVirtual
        {
            get
            {
                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Virtual");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Virtual", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Virtual", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsVirtual");
            }
        }

        /// <MetaDataID>{82a3bc13-d865-4d59-ade6-a06eea52cfd2}</MetaDataID>
        public bool IsOverride
        {
            get
            {
                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Override");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Override", value.ToString());
                Attribute.PutPropertyValue("MetaData", "Override", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsOverride");
            }
        }

        /// <MetaDataID>{9e921375-abf1-4c4d-a78a-6f7443fe23e7}</MetaDataID>
        public bool IsNew
        {
            get
            {
                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "New");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "New", value.ToString());
                RealObject.PutPropertyValue("MetaData", "New", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsNew");
            }
        }

        /// <MetaDataID>{2c1e9b67-e0b4-4322-b108-295df8905dc4}</MetaDataID>
        public bool IsSealed
        {
            get
            {
                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Sealed");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Sealed", value.ToString());
                Attribute.PutPropertyValue("MetaData", "Sealed", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsSealed");
            }
        }

        /// <MetaDataID>{ba885df7-d8dd-456b-8f13-795ede79e5d7}</MetaDataID>
        public bool IsNone
        {
            get
            {
                if (IsStatic)
                    return true;
                if (IsSealed || IsNew || IsOverride || IsVirtual || IsAbstract)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value == true)
                    IsSealed = IsNew = IsOverride = IsVirtual = IsAbstract = false;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsNone");
            }
        }

        /// <MetaDataID>{a14e17f2-1346-4a46-b597-3ff6af5776e8}</MetaDataID>
        public bool IsStatic
        {
            get
            {
                string propertyValue = Attribute.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Static");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                Attribute.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Static", value.ToString());
                Attribute.PutPropertyValue("MetaData", "Static", value);
                if (value == true)
                {
                    IsNone = true;
                    IsProperty = false;
                }
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
              
            }
        }
        /// <MetaDataID>{410d4598-94e3-4b44-8432-c21a2cead801}</MetaDataID>
        public bool OwnerIsClassOrStruct
        {
            get
            {
                return !(RealObject.Owner is MetaDataRepository.Interface);
            }
        }

        /// <MetaDataID>{fdb0fb59-5fe6-420b-98e3-562c17a042db}</MetaDataID>
        public bool CanChangeOverrideKind
        {
            get
            {
                if (AttributeRealization != null)
                    return true;
                return OwnerIsClassOrStruct && !IsStatic && (IsProperty||IsEvent);
            }
        }

        /// <MetaDataID>{3f107a9e-9b00-495e-bd2a-9e8726e48f5c}</MetaDataID>
        AttributeRealization _AttributeRealization;
        /// <MetaDataID>{5f3b78f9-197e-4c3a-98a5-59ece8a28a85}</MetaDataID>
        public AttributeRealization AttributeRealization
        {
            get
            {
                return _AttributeRealization;
            }
            set
            {
                _AttributeRealization = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }

        /// <MetaDataID>{e73a79bc-56bd-4af6-8ff6-cfdfd6873996}</MetaDataID>
        public System.Drawing.Bitmap VisibilityImage
        {
            get
            {
                switch (RealObject.Visibility)
                {
                    case MetaDataRepository.VisibilityKind.AccessPublic:
                        {
                            return PublicImage;
                            break;
                        }

                    case MetaDataRepository.VisibilityKind.AccessComponent:
                        {
                            return InternalImage;
                            break;
                        }

                    case MetaDataRepository.VisibilityKind.AccessProtected:
                        {
                            return ProtectedImage;
                            break;
                        }

                    case MetaDataRepository.VisibilityKind.AccessPrivate:
                        {
                            return PrivateImage;
                            break;
                        }
                }
                return null;

            }
        }


        /// <MetaDataID>{e726f17e-de24-46f2-9ac5-f7a1d7f72743}</MetaDataID>
        System.Drawing.Bitmap PublicImage = Resource.VSObject_Field;
        /// <MetaDataID>{e61f3082-c2c5-4c6a-bb9f-7eb3f051a876}</MetaDataID>
        System.Drawing.Bitmap InternalImage = Resource.VSObject_Field_Friend;
        /// <MetaDataID>{9ea7f8b2-37d7-4745-9bc1-16b539797c59}</MetaDataID>
        System.Drawing.Bitmap ProtectedImage = Resource.VSObject_Field_Protected;
        /// <MetaDataID>{e387a11e-c7f7-4e04-a6dc-d7b85634c0fb}</MetaDataID>
        System.Drawing.Bitmap PrivateImage = Resource.VSObject_Field_Private;

    }
}
