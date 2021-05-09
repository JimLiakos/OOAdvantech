using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.UserInterface.Runtime;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{74edba49-6f9e-4d2f-838b-6e52c552a704}</MetaDataID>
    public class AssociationEndPresentationObject : PresentationObject<AssociationEnd>
    {

        /// <MetaDataID>{410e949a-572e-4435-a0df-b9f272edfc38}</MetaDataID>
        public AssociationEnd AssociationEnd;

        /// <MetaDataID>{d4f63c36-4080-4301-bcd8-9c77484fad8e}</MetaDataID>
        public AssociationEndPresentationObject(AssociationEnd associationEnd)
            : base(associationEnd)
        {
            AssociationEnd = associationEnd;
        }

        /// <MetaDataID>{ac44c0c8-8db2-4fb9-a869-2b1536f5b749}</MetaDataID>
        public string Multiplicity
        {
            get
            {
                return RealObject.Multiplicity.ToString();
            }
            set
            {
                RealObject.SetMultiplicity(value);
            }
        }





    



        /// <exclude>Excluded</exclude>
        static List<string> _MultiplicityChoices = new List<string>() { "0..1", "0..*", "1", "1..*", "*" };
        /// <MetaDataID>{ba039fa6-ba8e-42e6-b281-f0e9c8f0b533}</MetaDataID>
        public List<string> MultiplicityChoices
        {
            get
            {
                return _MultiplicityChoices;
            }
        }


        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{59c98554-fce6-4403-a2f7-f0393c0f939b}</MetaDataID>
        public bool Persistent
        {
            get
            {
             
                return RealObject.Persistent;
            }
            set
            {
                RealObject.Persistent = value;
                if (RealObject.Persistent)
                    RefreshPersistentMember();
            
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }


        /// <MetaDataID>{97e5f8e2-5326-4542-994f-0ea4f0071d1f}</MetaDataID>
        public bool IsNavigable
        {
            get
            {
                return RealObject.Navigable;
            }
            set
            {
                RealObject.Navigable = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }

        /// <MetaDataID>{de2a709c-2c36-476f-81aa-d3ca2489c5be}</MetaDataID>
        public String ImplementationField
        {
            get
            {
                if (AutoGenerate)
                    return "Auto Generate";

                return RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "ImplementationField");
            }
            set
            {
                if (value != "Auto Generate")
                {
                    RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "ImplementationField", value);
                    RefreshPersistentMember();
                }
            }
        }

        /// <MetaDataID>{d79e0d1b-b5f5-4cee-9602-9f86669f94fa}</MetaDataID>
        public bool IsPersistentProperty
        {
            get
            {
                if (IsProperty)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{be6dc444-f51f-40ce-a9b3-68bc4ffebfcc}</MetaDataID>
        public bool EditImplementetionField
        {
            get
            {
                if (!AutoGenerate && IsProperty && AssociationEnd.Persistent)
                    return true;
                else
                    return false;
            }
        }



        /// <MetaDataID>{c5ac1a7e-e87d-43b6-a279-a77faeb3309f}</MetaDataID>
        public bool AutoGenerate
        {
            get
            {
                string autoGen = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "AutoGenerate");
                if (string.IsNullOrWhiteSpace(autoGen))
                    return true;
                else
                    return bool.Parse(autoGen);
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AutoGenerate", value.ToString());
                RefreshPersistentMember();
                if (ObjectChangeState != null)
                {
                    ObjectChangeState(this, "EditImplementetionField");
                    ObjectChangeState(this, "ImplementationField");
                }
                RefreshPersistentMember();
            }
        }

        /// <MetaDataID>{54e91826-3131-4941-8420-2fc94452def8}</MetaDataID>
        public bool HasBody
        {
            get
            {

                string hasBody = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "PropertyHasBody");
                if (string.IsNullOrWhiteSpace(hasBody))
                    return false;
                else
                    return bool.Parse(hasBody);
            }
            set
            {
                RealObject.PutPropertyValue("MetaData", "PropertyHasBody", value.ToString());
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "PropertyHasBody", value.ToString());
            }
        }

        /// <exclude>Excluded</exclude>
        PropertyMethodKind _GetPropertyMethod;

        /// <MetaDataID>{c948d79d-29ee-4a2c-91fa-cf9a33cbda56}</MetaDataID>
        public PropertyMethodKind GetPropertyMethod
        {
            get
            {
                string getterVisibility = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "GetterVisibility");
                if (string.IsNullOrWhiteSpace(getterVisibility))
                    return PropertyMethodKind.None;
                else
                    return (PropertyMethodKind)int.Parse(getterVisibility);
            }
            set
            {

                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "GetterVisibility", ((int)value).ToString());
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

        /// <MetaDataID>{32b46d92-0ebf-41d9-a3e5-744bb4ae818b}</MetaDataID>
        public PropertyMethodKind SetPropertyMethod
        {
            get
            {
                string setterVisibility = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "SetterVisibility");
                if (string.IsNullOrWhiteSpace(setterVisibility))
                    return PropertyMethodKind.None;
                else
                    return (PropertyMethodKind)int.Parse(setterVisibility);
            }
            set
            {
                _SetPropertyMethod = value;
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "SetterVisibility", ((int)value).ToString());
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


        /// <MetaDataID>{66edf691-07c7-46c7-b64f-df069335a090}</MetaDataID>
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


        /// <MetaDataID>{8efe1d2d-27ce-4c79-8c64-4164021a6fbc}</MetaDataID>
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



        /// <MetaDataID>{f1bebf2c-ec46-4a01-83f4-41fe9f45de55}</MetaDataID>
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


        /// <MetaDataID>{b07a952a-0536-4670-a1d0-f79dd21bf717}</MetaDataID>
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


        /// <MetaDataID>{1b8e1946-68a1-4d5b-813b-ab983ce1a46d}</MetaDataID>
        void RefreshPersistentMember()
        {
            if (IsPersistentProperty)
            {
                if (AutoGenerate)
                {
                    RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "ImplementationField", "_" + RealObject.Name);
                    RealObject.PutPropertyValue("MetaData", "ImplementationMember", "_" + RealObject.Name);
                }
                else
                    RealObject.PutPropertyValue("MetaData", "ImplementationMember", ImplementationField);
            }
        }


        /// <MetaDataID>{1751f0bb-467c-469c-b9bf-c48a8af9ef93}</MetaDataID>
        public bool CanBeAbstract
        {
            get
            {
                return IsProperty && (RealObject.Namespace is MetaDataRepository.Class) && (RealObject.Namespace as MetaDataRepository.Class).Abstract;
            }
        }

        /// <MetaDataID>{4714896b-72b3-44a2-9c54-9234d78dbb7a}</MetaDataID>
        public bool CanChangePropertyValue
        {
            get
            {
                if (RealObject.Namespace is MetaDataRepository.Interface)
                    return false;
                else
                    return true;
            }
        }

        /// <MetaDataID>{abc92f88-a335-4a8e-bcce-1fd6eeef1c5c}</MetaDataID>
        public bool CanChangeEventValue
        {
            get
            {
                if (RealObject.Namespace is MetaDataRepository.Interface && !IsProperty)
                    return false;
                else
                    return true;
            }
        }


        /// <MetaDataID>{8c26eb7b-2fd3-4c2b-8c08-e5fc8f342525}</MetaDataID>
        public bool CanChangeIndexerPropertyValue
        {
            get
            {
                return IsNavigable && Persistent;

            }
        }


        /// <MetaDataID>{87f5fe50-bacd-4409-9b92-152712891c07}</MetaDataID>
        public bool IsProperty
        {
            get
            {
                string propertyValue = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "AsProperty");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "AsProperty", value.ToString());
                RealObject.PutPropertyValue("MetaData", "AsProperty", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
                if (!value)
                    IsNone = true;
            }

        }


        /// <MetaDataID>{969d9883-4409-4fcb-98c9-c85b00c4f794}</MetaDataID>
        public bool IsAbstract
        {
            get
            {
                string propertyValue = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Abstract");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Abstract", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Abstract", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsAbstract");
            }
        }


        /// <MetaDataID>{151e6850-63a9-444b-9a6a-fb0fb46df332}</MetaDataID>
        public bool IsVirtual
        {
            get
            {
                string propertyValue = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Virtual");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Virtual", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Virtual", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsVirtual");
            }
        }


        /// <MetaDataID>{c706d27d-6133-4cb9-978e-fa1b42384032}</MetaDataID>
        public bool IsOverride
        {
            get
            {
                string propertyValue = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Override");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Override", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Override", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsOverride");
            }
        }


        /// <MetaDataID>{d29b5a71-9a09-4ee0-baa5-7fc15aa61553}</MetaDataID>
        public bool IsNew
        {
            get
            {
                string propertyValue = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "New");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "New", value.ToString());
                RealObject.PutPropertyValue("MetaData", "New", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsNew");
            }
        }


        /// <MetaDataID>{cbbd6d5e-d569-4b76-aecf-e6468707cba1}</MetaDataID>
        public bool IsSealed
        {
            get
            {
                string propertyValue = RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Sealed");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Sealed", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Sealed", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsSealed");
            }
        }


        /// <MetaDataID>{acb7cbe7-7fd9-45d6-9b05-d17a56fe0ad4}</MetaDataID>
        public bool IsNone
        {
            get
            {
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

        /// <MetaDataID>{b0854b99-5e15-4ce9-a58f-222c97ce069a}</MetaDataID>
        public bool IsIndexer
        {
            get
            {
                return RealObject.Indexer;
            }
            set
            {
                RealObject.Indexer = value;
             
            }
        }



        /// <MetaDataID>{23c3a9f4-24e7-4e5e-af7b-e8b27843a317}</MetaDataID>
        public  string CollectionType
        {
            get
            {
                return RealObject.ModelElement.GetExtensionData().GetPropertyValue("MetaData", "CollectionType");

            }
            set
            {
                RealObject.ModelElement.GetExtensionData().SetPropertyValue("MetaData", "CollectionType",value);
            }
        }

        /// <MetaDataID>{35832c56-602f-4097-af25-eafcec0de5ac}</MetaDataID>
        public bool IsLazyFetching
        {
            get
            {
                return RealObject.LazyFetching;
            }
            set
            {
                RealObject.LazyFetching = value;
                
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsLazyFetching");
            }
        }

        /// <MetaDataID>{8a4672e0-a409-48d8-b1ef-fc2466053362}</MetaDataID>
        public bool IsReferentialIntegrity
        {
            get
            {

                return RealObject.ReferentialIntegrity;
            }
            set
            {
                RealObject.ReferentialIntegrity = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsReferentialIntegrity");
            }
        }

        /// <MetaDataID>{b8a9fbf5-6c6d-461c-9c6b-a24914a637d1}</MetaDataID>
        public bool IsCascadeDelete
        {
            get
            {

                return RealObject.CascadeDelete;
            }
            set
            {
                RealObject.CascadeDelete = value;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsCascadeDelete");
            }
        }

        /// <MetaDataID>{7f210f10-7b57-4051-b36d-f987ace72690}</MetaDataID>
        public bool IsAllowTransient
        {
            get
            {
                return RealObject.AllowTransient;
            }
            set
            {
                RealObject.AllowTransient = value;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsAllowTransient");
            }
        }



        /// <MetaDataID>{5404fd11-8455-4eea-8daf-a6c9d6abcd14}</MetaDataID>
        public bool OwnerIsClassOrStruct
        {
            get
            {
                return !(RealObject.Namespace is MetaDataRepository.Interface);
            }
        }

        /// <MetaDataID>{103fc1f6-4557-4379-9a5f-ee52a8969f46}</MetaDataID>
        public bool CanChangeOverrideKind
        {
            get
            {
                if (AttributeRealization != null)
                    return true;
                return OwnerIsClassOrStruct;
            }
        }
        /// <exclude>Excluded</exclude>
        AttributeRealization _AttributeRealization;
        /// <MetaDataID>{4145670c-0c8a-4fef-b536-b02cb3688c77}</MetaDataID>
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



    }
}
