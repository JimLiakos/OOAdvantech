using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.UserInterface.Runtime;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{bdbf6d38-9f8f-42b4-a505-9f5ccad3f779}</MetaDataID>
    public class OperationPresentationObject : PresentationObject<MetaDataRepository.BehavioralFeature>
    {
        /// <MetaDataID>{8123c38e-65be-4291-89e4-b6a244eaab0d}</MetaDataID>
        public OperationPresentationObject(MetaDataRepository.BehavioralFeature behavioralFeature)
            : base(behavioralFeature)
        {
            behavioralFeature.Changed += new MetaDataRepository.MetaObjectChangedEventHandler(operation_Changed);
            Operation = behavioralFeature as Operation;
            _Method = behavioralFeature as Method;
            if (_Method != null)
                Operation = _Method.Specification as Operation;

            //RefreshPersistentMember();
        }


      

        /// <MetaDataID>{14bd3a46-ba41-441c-ba18-d9957fe5841f}</MetaDataID>
        public string ReturnType
        {
            get
            {
                if (Operation.ReturnType is Primitive && (Operation.ReturnType as Primitive).ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString())
                    return Operation.ReturnType.Name;

                if (Operation.ReturnType.FullName == typeof(void).FullName)
                    return "void";
                return Operation.ReturnType.FullName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Operation.ReturnType = null;
                    return;
                }

               MetaDataRepository.Classifier returnType= (from classifier in Operation.UMLModel.GetTypes()
                 where classifier.FullName==value
                 select classifier).FirstOrDefault();
               if (returnType == null)
               {
                   returnType = (from primitive in Operation.UMLModel.GetTypes().OfType<Primitive>()
                                 where primitive.Name == value&&primitive.ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType")==true.ToString()
                                 select primitive).FirstOrDefault();
                   if(returnType==null)
                   {
                       IPrimitiveType primitiveType = Operation.UMLModel.CreatePrimitiveType();
                       (primitiveType as ModelElement).GetExtensionData().SetPropertyValue("UnspecifiedType", true.ToString());
                       primitiveType.Name = value;
                       returnType = VisualStudioUMLHelper.GetClassifierFor(primitiveType, Operation.UMLModel);
                   }
                   
               }
               Operation.ReturnType = returnType;
            }
        }
        /// <MetaDataID>{09dcd76c-0fd0-41f6-a623-098945b00032}</MetaDataID>
        public string NewType(string typeName)
        {
            return typeName;
        }

        /// <MetaDataID>{737daddb-eebe-400e-94f4-3cc4f9dab178}</MetaDataID>
        public System.Collections.Generic.List<string> Types
        {
            get
            {
                List<string> typeNames = new List<string>();
                foreach (var classifier in Operation.UMLModel.GetTypes())
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
                //return (from classifier in Operation.UMLModel.GetTypes()
                // select classifier.FullName).ToList();
            }
        }
        /// <MetaDataID>{7fa1515b-f351-45bc-8129-e78673fff26d}</MetaDataID>
        Operation Operation;
        /// <MetaDataID>{e1789af9-c955-4790-9bae-69a43fee35e5}</MetaDataID>
        Method _Method;
        /// <MetaDataID>{c940e133-897c-4c1f-af19-2cf25401fea2}</MetaDataID>
        internal Method Method
        {
            get
            {
                return _Method;
            }
            set
            {
                _Method = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
            }
        }

        class ParameterMetaData
        {
            public string Name;
            public MetaDataRepository.Classifier Type;
 
        }
        /// <MetaDataID>{48b730cd-6548-4c72-abf5-368d3f78ab22}</MetaDataID>
        public Parameter InserNewParameter(int index)
        {
            List<ParameterMetaData> parametersData = (from parameter in Operation.Parameters
                                                      select new ParameterMetaData() { Name = parameter.Name, Type = parameter.Type }).ToList();
            int newParameterIndex = parametersData.Count;
            if (index != -1 && Operation.Parameters.Count > index)
            {
                newParameterIndex = index;
                parametersData.Insert(index, new ParameterMetaData() { Name = "newParam" });
            }
            else
                parametersData.Add(new ParameterMetaData() { Name = "newParam" });

            Operation.VSUMLOperation.CreateParameter().Name = "newParam";

            int i = 0;
            foreach (var VSUMLParam in Operation.VSUMLOperation.OwnedParameters)
            {
                VSUMLParam.Name = parametersData[i].Name;
                VSUMLParam.Type = parametersData[i].Type.GetUMLType();
                i++;
            }
            
            var newParameter=  Operation.Parameters[newParameterIndex] as Parameter ;
            if (ObjectChangeState != null)
                ObjectChangeState(this, "Parameters");
            return newParameter;

        }
        /// <MetaDataID>{6ea258e2-95df-47dd-89a7-5b8fb6f2125e}</MetaDataID>
        void operation_Changed(object sender)
        {

        }
        /// <MetaDataID>{bbc2ca8d-0298-4132-986c-0e3f7a0d935a}</MetaDataID>
       public List<Parameter> Parameters
        {
            get
            {
                return (from parameter in Operation.Parameters.OfType<Parameter>()
                 select parameter).ToList();
            }
        }
       /// <MetaDataID>{3dc64d37-34df-45f0-81b5-07ab7ae96af2}</MetaDataID>
        public void DeleteParameter(Parameter parameter)
        {
            Operation.DeleteParameter(parameter);
            parameter.VSUmlparameter.Delete();
            if(ObjectChangeState!=null)
                ObjectChangeState(this,"Parameters");

       
        }

        /// <MetaDataID>{e0540909-821a-4690-93e3-eab68d97772f}</MetaDataID>
        public bool OwnerIsInterface
        {
            get
            {
                return RealObject.Owner is MetaDataRepository.Interface;
            }
        }

        /// <MetaDataID>{5c034ad8-fec6-477d-879f-067575fd1a5d}</MetaDataID>
        public bool CanBeAbstract
        {
            get
            {
                return CanChangeOverrideKind&&RealObject.Owner is Class && !IsStatic && (RealObject.Owner as Class).Abstract;
            }
        }

        /// <MetaDataID>{c6c5e836-a664-4ec4-8b47-a31842cf181d}</MetaDataID>
        System.Drawing.Bitmap PublicImage = Resource.VSObject_Method;

        /// <MetaDataID>{f69b0c36-f70d-4d3e-9ee0-6f2a8dd57fc1}</MetaDataID>
        System.Drawing.Bitmap InternalImage = Resource.VSObject_Method_Friend;

        /// <MetaDataID>{854a4f2a-69e2-462e-8520-162c704b723e}</MetaDataID>
        System.Drawing.Bitmap ProtectedImage = Resource.VSObject_Method_Protected;

        /// <MetaDataID>{e5864b70-2013-4172-96b9-ce8c017fca4f}</MetaDataID>
        System.Drawing.Bitmap PrivateImage = Resource.VSObject_Method_Private;

        /// <MetaDataID>{57eb31b1-f212-4f35-b7b0-6b43e4aaaf7f}</MetaDataID>
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

        /// <MetaDataID>{1ceb92f0-5d89-4d32-8c3d-64f2383c0a52}</MetaDataID>
        public bool CanChangeOverrideKind
        {
            get
            {
                return !(RealObject.Owner is MetaDataRepository.Interface) && !IsStatic;
            }
        }



        /// <MetaDataID>{14fc0bf0-c787-433b-98f4-dd7cd5fe57a0}</MetaDataID>
        public bool CanChangeVisibility
        {
            get
            {
                return !(RealObject.Owner is MetaDataRepository.Interface) && Method == null;
            }
        }



        /// <MetaDataID>{60591914-146d-4a77-9c63-3de71aea852c}</MetaDataID>
        public bool CanBeStatic
        {
            get
            {
                return !(RealObject.Owner is MetaDataRepository.Interface) && Method == null;
            }
        }

        /// <MetaDataID>{cd0f64ac-3d86-42c1-90a6-5f621df56563}</MetaDataID>
        public ModelElement ModelElement
        {
            get
            {
                if (Method != null)
                    return Method.VSUmlOperation as Microsoft.VisualStudio.Modeling.ModelElement;
                else 
                    return Operation.VSUMLOperation as Microsoft.VisualStudio.Modeling.ModelElement;
            }
        }


        /// <MetaDataID>{02418b39-3d8a-4862-9576-00b690e7653f}</MetaDataID>
        public bool IsAbstract
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Abstract");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Abstract", value.ToString());

                if (value)
                    if(Method!=null)
                        Method.OverrideKind = MetaDataRepository.OverrideKind.Abstract;
                    else
                        Operation.OverrideKind = MetaDataRepository.OverrideKind.Abstract;



                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsAbstract");
            }
        }

        /// <MetaDataID>{eebc9ad6-c135-49e6-bfb7-dde31739d516}</MetaDataID>
        public bool IsVirtual
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Virtual");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Virtual", value.ToString());

                if (value)
                    if (Method != null)
                        Method.OverrideKind = MetaDataRepository.OverrideKind.Virtual;
                    else
                        Operation.OverrideKind = MetaDataRepository.OverrideKind.Virtual;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsVirtual");
            }
        }

        /// <MetaDataID>{d9af3e92-d4d8-4e1a-bac7-e5fc149c9b3a}</MetaDataID>
        public bool IsOverride
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Override");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Override", value.ToString());

                if (value)
                    if (Method != null)
                        Method.OverrideKind = MetaDataRepository.OverrideKind.Override;
                    else
                        Operation.OverrideKind = MetaDataRepository.OverrideKind.Override;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsOverride");
            }
        }

        /// <MetaDataID>{87853414-8bc3-49ea-ab12-d39ca9ec71fb}</MetaDataID>
        public bool IsNew
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "New");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "New", value.ToString());

                if (value)
                    if (Method != null)
                        Method.OverrideKind = MetaDataRepository.OverrideKind.New;
                    else
                        Operation.OverrideKind = MetaDataRepository.OverrideKind.New;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsNew");
            }
        }

        /// <MetaDataID>{40f3eb91-e99a-4d47-b22e-94bed702b80d}</MetaDataID>
        public bool IsSealed
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Sealed");
                
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Sealed", value.ToString());

                if (value)
                    if (Method != null)
                        Method.OverrideKind = MetaDataRepository.OverrideKind.Sealed;
                    else
                        Operation.OverrideKind = MetaDataRepository.OverrideKind.Sealed;


                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsSealed");
            }
        }

        /// <MetaDataID>{5a74bf23-0086-4bf6-af5e-3e0a847c915f}</MetaDataID>
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
                if (value)
                    if (Method != null)
                        Method.OverrideKind = MetaDataRepository.OverrideKind.None;
                    else
                        Operation.OverrideKind = MetaDataRepository.OverrideKind.None;

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "IsNone");
            }
        }

        /// <MetaDataID>{e623b6e0-6418-4d51-bd68-7d853ebc03cb}</MetaDataID>
        public bool IsStatic
        {
            get
            {
                string propertyValue = ModelElement.GetExtensionData().GetPropertyValue("MetaData", "Static");
                bool value = false;
                if (bool.TryParse(propertyValue, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                ModelElement.GetExtensionData().SetPropertyValue("MetaData", "Static", value.ToString());
                RealObject.PutPropertyValue("MetaData", "Static", value);
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);
                if (value == true)
                {
                    IsNone = true;
                }
            }
        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{367902f5-5b1d-41a0-8cae-369a2fd41393}</MetaDataID>
        public bool Public
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility == VisibilityKind.Public;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility = VisibilityKind.Public;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }


        /// <MetaDataID>{9ece04f7-3246-4003-aa22-8e8d67cf7700}</MetaDataID>
        public bool Private
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility == VisibilityKind.Private;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility = VisibilityKind.Private;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }



        /// <MetaDataID>{193bc9bb-b726-4f96-8040-da7db5d64a3f}</MetaDataID>
        public bool Protected
        {
            get
            {
                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility == VisibilityKind.Protected;
            }
            set
            {
                if (value)
                {

                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility = VisibilityKind.Protected;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }


        /// <MetaDataID>{f8c1ab3e-1556-4867-9c53-9619bfb228b0}</MetaDataID>
        public bool Package
        {
            get
            {

                return ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility == VisibilityKind.Package;
            }
            set
            {
                if (value)
                {
                    ((RealObject as IVSUMLModelItemWrapper).ModelElement as IOperation).Visibility = VisibilityKind.Package;
                    RealObject.Visibility = MetaDataRepository.VisibilityKind.AccessComponent;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, null);
                }
            }
        }
    }
}
