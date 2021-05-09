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
    /// <MetaDataID>{97fcef61-d0d9-4958-a822-12557e572147}</MetaDataID>
    public class ParameterPresentationObject : PresentationObject<Parameter>
    {
        Parameter Parameter;
        public ParameterPresentationObject(Parameter parameter)
            : base(parameter)
        {
            Parameter = parameter;
        }

        public string NewType(string typeName)
        {
            return typeName;
        }
        public string Type
        {
            get
            {
                if (RealObject.Type is Primitive && (Parameter.Type as Primitive).ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString())
                    return RealObject.Type.Name;

                if (typeof(void).FullName == RealObject.Type.FullName)
                    return "";

                return RealObject.Type.FullName;
            }
            set
            {

                MetaDataRepository.Classifier type = (from classifier in Parameter.UMLModel.GetTypes()
                                                      where classifier.FullName == value
                                                      select classifier).FirstOrDefault();
                if (type == null)
                {
                    type = (from primitive in Parameter.UMLModel.GetTypes().OfType<Primitive>()
                            where primitive.Name == value && primitive.ModelElement.GetExtensionData().GetPropertyValue("UnspecifiedType") == true.ToString()
                            select primitive).FirstOrDefault();
                    if (type == null)
                    {
                        IPrimitiveType primitiveType = Parameter.UMLModel.CreatePrimitiveType();
                        (primitiveType as ModelElement).GetExtensionData().SetPropertyValue("UnspecifiedType", true.ToString());
                        primitiveType.Name = value;
                        type = VisualStudioUMLHelper.GetClassifierFor(primitiveType, Parameter.UMLModel);
                    }

                }
                RealObject.Type = type;
            }
        }
        public System.Collections.Generic.List<string> Types
        {
            get
            {
                List<string> typeNames = new List<string>();
                foreach (var classifier in Parameter.UMLModel.GetTypes())
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

    }
}
