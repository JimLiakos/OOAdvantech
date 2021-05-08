using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.CodeMetaDataRepository
{


    /// <MetaDataID>{38D7EB2D-B244-4342-A91C-EE093D72F56C}</MetaDataID>
    public class LanguageParser
    {

        /// <MetaDataID>{f79ace7f-a2b9-44d5-9888-a1d66d77b740}</MetaDataID>
        public static string GetShortName(string fullName, EnvDTE.CodeElement definitionCodeElement)
        {
             
            if (definitionCodeElement == null)
                return fullName;
            EnvDTE.ProjectItem projectItem = definitionCodeElement.ProjectItem;
            Collections.Generic.List<string> projectItemNamespaces = new OOAdvantech.Collections.Generic.List<string>();

            EnvDTE.CodeNamespace projectItemNamespace = GetNamespace(definitionCodeElement);
            if (projectItemNamespace != null)
            {
                projectItemNamespaces.Add(projectItemNamespace.FullName);
                MetaDataRepository.Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(projectItemNamespace.FullName) as MetaDataRepository.Namespace;
                projectItemNamespaces.Add(_namespace.FullName);
                while (_namespace.Namespace != null)
                {
                    _namespace = _namespace.Namespace;
                    projectItemNamespaces.Add(_namespace.FullName);
                }

            }



            foreach (EnvDTE.CodeElement codeElement in projectItem.FileCodeModel.CodeElements)
            {
                //if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
                //{
                //    projectItemNamespace = codeElement as EnvDTE.CodeNamespace;
                //    foreach (EnvDTE.CodeElement namespaceCodeElement in projectItemNamespace.Members)
                //    {
                //        if (namespaceCodeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                //            projectItemNamespaces.Add((namespaceCodeElement as EnvDTE80.CodeImport).Namespace);
                //    }
                //    MetaDataRepository.Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(projectItemNamespace.FullName) as MetaDataRepository.Namespace;
                //    if (_namespace == null)
                //        _namespace = new Namespace(projectItemNamespace);

                //    projectItemNamespaces.Add(_namespace.FullName);
                //    while (_namespace.Namespace != null)
                //    {
                //        _namespace = _namespace.Namespace;
                //        projectItemNamespaces.Add(_namespace.FullName);
                //    }

                //}
                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                    projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
            }
            if (projectItemNamespace != null)
            {
                foreach (EnvDTE.CodeElement codeElement in projectItemNamespace.Members)
                {
                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                        projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
                }
            }
            foreach (string _namespace in projectItemNamespaces)
            {
                if (fullName.IndexOf(_namespace) == 0)
                    return fullName.Substring(_namespace.Length + 1);
            }

            return fullName;
        }

        /// <MetaDataID>{5896f401-60fd-4006-a3e6-b1f990adfff3}</MetaDataID>
        private static EnvDTE.CodeNamespace GetNamespace(EnvDTE.CodeElement codeElement)
        {
            if (codeElement == null)
                return null;
            if (codeElement is EnvDTE.CodeNamespace)
                return codeElement as EnvDTE.CodeNamespace;

            EnvDTE.CodeElement parent = null;
            if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementClass ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementInterface ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementEnum ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementEvent ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementStruct ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementProperty ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementVariable ||
                codeElement.Kind == EnvDTE.vsCMElement.vsCMElementFunction)
            {

                try
                {
                    return codeElement.StartPoint.CreateEditPoint().get_CodeElement(EnvDTE.vsCMElement.vsCMElementNamespace) as EnvDTE.CodeNamespace;
                }
                catch (System.Exception error)
                {
                    return null;
                }
            }
            else
                return null;


            if (codeElement is EnvDTE.CodeAttribute)
                parent = (codeElement as EnvDTE.CodeAttribute).Parent as EnvDTE.CodeElement;
            if (codeElement is EnvDTE.CodeProperty)
                parent = (codeElement as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
            if (codeElement is EnvDTE.CodeVariable)
                parent = (codeElement as EnvDTE.CodeVariable).Parent as EnvDTE.CodeElement;
            if (codeElement is EnvDTE.CodeFunction)
                parent = (codeElement as EnvDTE.CodeFunction).Parent as EnvDTE.CodeElement;
            if (codeElement is EnvDTE.CodeClass)
                parent = (codeElement as EnvDTE.CodeClass).Parent as EnvDTE.CodeElement;
            if (codeElement is EnvDTE.CodeInterface)
                parent = (codeElement as EnvDTE.CodeInterface).Parent as EnvDTE.CodeElement;
            if (codeElement is EnvDTE.CodeStruct)
                parent = (codeElement as EnvDTE.CodeStruct).Parent as EnvDTE.CodeElement;

            return GetNamespace(parent);


        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1709DAF8-7284-4FD2-B4D5-5FF68CF70861}</MetaDataID>
        private static Parser.Parser _CSharpGenericsParser;

        /// <MetaDataID>{56930BA2-A0AF-4BA4-83FC-343CDE26F638}</MetaDataID>
        internal static Parser.Parser CSharpGenericsParser
        {
            get
            {
                if (_CSharpGenericsParser == null)
                {
                    _CSharpGenericsParser = new Parser.Parser();

                    System.Type mType = typeof(LanguageParser);
                    string[] Resources = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
                    using (System.IO.Stream Grammar = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OOAdvantech.CodeMetaDataRepository.Grammars.Generics.cgt"))
                    {
                        byte[] bytes = new byte[Grammar.Length];
                        Grammar.Read(bytes, 0, (int)Grammar.Length);
                        _CSharpGenericsParser.SetGrammar(bytes, (int)Grammar.Length);
                        Grammar.Close();
                    }
                }

                return _CSharpGenericsParser;
            }

        }

        /// <MetaDataID>{d52564af-7ec9-4bdb-800b-5292e0272b03}</MetaDataID>
        internal static void GetGenericMetaData(string fullName, string language, ref string typeFullName, ref System.Collections.Generic.List<string> parameters)
        {
            //vsCMLanguageCSharp  {B5E9BD34-6D3E-4B5D-925E-8A43B79820B4} Visual C# .NET 
            //vsCMLanguageIDL     {B5E9BD35-6D3E-4B5D-925E-8A43B79820B4} Microsoft IDL 
            //vsCMLanguageMC      {B5E9BD36-6D3E-4B5D-925E-8A43B79820B4} Managed Extensions for C++ 
            //vsCMLanguageVB      {B5E9BD33-6D3E-4B5D-925E-8A43B79820B4} Visual Basic .NET 
            //vsCMLanguageVC      {B5E9BD32-6D3E-4B5D-925E-8A43B79820B4} Visual C++ .NET 

            if (language == "{B5E9BD34-6D3E-4B5D-925E-8A43B79820B4}")
                GetGenericMetaDataFromCSharpType(fullName, ref typeFullName, ref parameters);

        }
        /// <MetaDataID>{D7E24815-8471-4270-AECE-D4207FBB067E}</MetaDataID>
        internal static void GetGenericMetaDataFromCSharpType(string fullName, ref string typeFullName, ref System.Collections.Generic.List<string> parameters)
        {
            try
            {
                if (parameters == null)
                    parameters = new List<string>();
                CSharpGenericsParser.Parse(fullName);
                foreach (Parser.ParserNode parserNode in (CSharpGenericsParser.theRoot["Program"]["Generic_spec"]["Generic_type"]["Generic_parameters"] as Parser.ParserNode).ChildNodes)
                    parameters.Add(parserNode.Value);
                typeFullName = (CSharpGenericsParser.theRoot["Program"]["Generic_spec"]["Generic_type"]["FullName"] as Parser.ParserNode).Value;
            }
            catch (Exception error)
            {
            }
        }

        /// <MetaDataID>{0EB8583A-8A9F-48FF-8711-07FD391E3A5F}</MetaDataID>
        internal static bool IsGeneric(string fullName, string language)
        {
            //vsCMLanguageCSharp  {B5E9BD34-6D3E-4B5D-925E-8A43B79820B4} Visual C# .NET 
            //vsCMLanguageIDL     {B5E9BD35-6D3E-4B5D-925E-8A43B79820B4} Microsoft IDL 
            //vsCMLanguageMC      {B5E9BD36-6D3E-4B5D-925E-8A43B79820B4} Managed Extensions for C++ 
            //vsCMLanguageVB      {B5E9BD33-6D3E-4B5D-925E-8A43B79820B4} Visual Basic .NET 
            //vsCMLanguageVC      {B5E9BD32-6D3E-4B5D-925E-8A43B79820B4} Visual C++ .NET 

            if (language == "{B5E9BD34-6D3E-4B5D-925E-8A43B79820B4}" &&
                fullName.Trim().IndexOf(">") == fullName.Length - 1)
                return true;

            return false;


        }
        /// <MetaDataID>{b24a250d-eb72-4de1-8a62-33069374d111}</MetaDataID>
        internal static EnvDTE.CodeTypeRef CreateCodeTypeRef(MetaDataRepository.Classifier classifier, EnvDTE.CodeModel codeModel)
        {
            try
            {
                if (classifier == null)
                    return codeModel.CreateCodeTypeRef("void");
                if (typeof(void).FullName == classifier.FullName)
                    return codeModel.CreateCodeTypeRef("void");

                return codeModel.CreateCodeTypeRef(GetTypeFullName(classifier, codeModel.Parent));
            }
            catch (Exception error)
            {

                throw new System.Exception(string.Format("Unable to create type reference for '{0}'", classifier.FullName));
            }
        }
        internal static EnvDTE.CodeTypeRef CreateCodeTypeRef(string fullName, EnvDTE.CodeModel codeModel)
        {
            //if (classifier == null)
            //    return codeModel.CreateCodeTypeRef("void");
            //if (typeof(void).FullName == classifier.FullName)
            //    return codeModel.CreateCodeTypeRef("void");

            try
            {
                return codeModel.CreateCodeTypeRef(fullName);
            }
            catch (Exception error)
            {

                throw new System.Exception(string.Format("Unable to create type reference for '{0}'", fullName));
            }
        }

        /// <MetaDataID>{b3ba2578-39d2-448c-b787-8db9b96ca950}</MetaDataID>
        public static string GetInstantiationName(OOAdvantech.MetaDataRepository.TemplateBinding templateBinding, string language)
        {

            string name = (templateBinding.Signature.Template as MetaDataRepository.MetaObject).Name;
            name = name.Substring(0, name.IndexOf("`"));

            //  string identityString = name;
            string templateParma = null;

            foreach (OOAdvantech.MetaDataRepository.TemplateParameter parameter in templateBinding.Signature.OwnedParameters)
            {
                //MetaDataRepository.Classifier parameterClassifier = parameterSubstitution.ActualParameters[0] as MetaDataRepository.Classifier;
                //  identityString += "[" + parameter + "," + parameterClassifier.ImplementationUnit.Identity + "]";
                if (templateParma == null)
                    templateParma += "<";
                else
                    templateParma += ",";

                templateParma += templateBinding.GetActualParameterFor(parameter).FullName;
            }
            if (templateParma != null)
                name += templateParma + ">";

            return name;
        }

        /// <MetaDataID>{B0F9011E-7E53-4280-BBFD-20A700EC1E92}</MetaDataID>
        internal static bool IsGeneric(EnvDTE.CodeTypeRef codeTypeRef)
        {
            if (codeTypeRef.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefVoid)
                return false;
            if (codeTypeRef.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType)
                return IsGeneric(codeTypeRef.AsString, codeTypeRef.CodeType.Language)|IsGeneric(codeTypeRef.AsFullName, codeTypeRef.CodeType.Language);
                //return IsGeneric(codeTypeRef.AsString, codeTypeRef.CodeType.Language);
            return false;
        }

        internal static bool IsGeneric(EnvDTE.CodeTypeRef codeTypeRef,string language)
        {
            if (codeTypeRef.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefVoid)
                return false;
            if (codeTypeRef.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType)
                return IsGeneric(codeTypeRef.AsString, language) | IsGeneric(codeTypeRef.AsFullName, language);
            return IsGeneric(codeTypeRef.AsString, language);
            return false;
        }
        /// <MetaDataID>{b942b7fa-f0e8-479f-a7fc-27ce671dc8be}</MetaDataID>
        internal static bool IsGeneric(EnvDTE.CodeElement codeElement)
        {
            try
            {
                if (((codeElement is EnvDTE80.CodeClass2) && (codeElement as EnvDTE80.CodeClass2).IsGeneric) ||
                      ((codeElement is EnvDTE80.CodeInterface2) && (codeElement as EnvDTE80.CodeInterface2).IsGeneric) ||
                      ((codeElement is EnvDTE80.CodeStruct2) && (codeElement as EnvDTE80.CodeStruct2).IsGeneric))
                    return true;
            }
            catch (Exception error)
            {

            }

            if (codeElement is EnvDTE.CodeClass ||
                codeElement is EnvDTE.CodeInterface ||
                codeElement is EnvDTE.CodeStruct)
                return IsGeneric(codeElement.FullName, codeElement.Language);
            return false;
        }

        /// <MetaDataID>{a7a161b3-5553-4e4e-9473-045830e77d90}</MetaDataID>
        internal static bool IsAssociationAttribute(EnvDTE.CodeAttribute codeAttribute)
        {
            string attributeName = null;
            try
            {
                attributeName = codeAttribute.FullName;
            }
            catch (System.Exception error)
            {
                attributeName = codeAttribute.Name;
            }
            if ((attributeName == typeof(MetaDataRepository.AssociationAttribute).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationAttribute).FullName, codeAttribute as EnvDTE.CodeElement)))
                return true;
            else
            {
                attributeName += "Attribute";
                if ((attributeName == typeof(MetaDataRepository.AssociationAttribute).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationAttribute).FullName, codeAttribute as EnvDTE.CodeElement)))
                    return true;
                else if (codeAttribute.Name == typeof(MetaDataRepository.AssociationAttribute).Name || codeAttribute.Name+"Attribute" == typeof(MetaDataRepository.AssociationAttribute).Name)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{ac745511-2aa2-4b57-8d2b-f5a7539d875f}</MetaDataID>
        internal static bool IsEqual(System.Type attributeType, EnvDTE.CodeAttribute codeAttribute)
        {
            string attributeName = null;
            try
            {
                attributeName = codeAttribute.FullName;
            }
            catch (System.Exception error)
            {
                attributeName = codeAttribute.Name;
            }
            if ((attributeName == attributeType.FullName || attributeName == LanguageParser.GetShortName(attributeType.FullName, codeAttribute as EnvDTE.CodeElement)))
                return true;
            else
            {
                attributeName += "Attribute";
                if ((attributeName == attributeType.FullName || attributeName == LanguageParser.GetShortName(attributeType.FullName, codeAttribute as EnvDTE.CodeElement)))
                    return true;
                else
                    return false;
            }

        }

        internal static bool IsEqual(string  attributeName, EnvDTE.CodeAttribute codeAttribute)
        {
            string codeAttributeName = null;
            try
            {
                codeAttributeName = codeAttribute.FullName;
            }
            catch (System.Exception error)
            {
                codeAttributeName = codeAttribute.Name;
            }
            if (codeAttributeName == attributeName || codeAttributeName == LanguageParser.GetShortName(attributeName, codeAttribute as EnvDTE.CodeElement))
                return true;
            else
            {
                codeAttributeName += "Attribute";
                if ((codeAttributeName == attributeName || codeAttributeName == LanguageParser.GetShortName(attributeName, codeAttribute as EnvDTE.CodeElement)))
                    return true;
                else if(codeAttribute.Name+"Attribute"==attributeName)
                    return true;
                else
                    return false;
            }

        }


        /// <MetaDataID>{4a93814a-9893-4318-9aaa-d9770c850783}</MetaDataID>
        internal static void GetAssociationData(
                                            EnvDTE.CodeAttribute codeAttribute,
                                            out string associationName,
                                            out MetaDataRepository.Roles associationEndRole,
                                            out string identity,
                                            out bool indexer,
            /*out string generalAssociationHostType, */
                                            out string generalAssociationIdentity)
        {

            if (!IsAssociationAttribute(codeAttribute))
                throw new System.ArgumentException("The codeAttribute isn't AssociationAttribute.", "codeAttribute");

            associationName = null;
            associationEndRole = default(MetaDataRepository.Roles);
            identity = null;
            indexer = false;
            //  generalAssociationHostType = null;
            generalAssociationIdentity = null;




            string value = codeAttribute.Value;
            value = value.Trim();
            int nPos = value.IndexOf(',');
            if (nPos == -1)
            {
                associationName = value.Replace("\"", "").Trim();
                return;
            }
            else
                associationName = value.Substring(0, nPos).Replace("\"", "").Trim();
            value = value.Substring(nPos + 1);
            String otherEndType = null;
            value = value.Trim();
            if (value.IndexOf("typeof") == 0)
            {
                nPos = value.IndexOf(',');
                if (nPos == -1)
                    otherEndType = value.Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();
                else
                    otherEndType = value.Substring(0, nPos).Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();

                if (nPos == -1)
                    return;
                value = value.Substring(nPos + 1);
            }
            
            string associationEndRoleDefinition = null;
            value = value.Trim();
            nPos = value.IndexOf(',');

            if (nPos == -1)
                associationEndRoleDefinition = value;
            else
                associationEndRoleDefinition = value.Substring(0, nPos);
            associationEndRoleDefinition = associationEndRoleDefinition.Substring(associationEndRoleDefinition.LastIndexOf('.') + 1);
            if (associationEndRoleDefinition == "RoleA")
                associationEndRole = MetaDataRepository.Roles.RoleA;
            else
                associationEndRole = MetaDataRepository.Roles.RoleB;



            if (nPos == -1)
                return;

            value = value.Substring(nPos + 1);
            value = value.Trim();
            nPos = value.IndexOf(',');
            string nextParameter = null;
            if (nPos == -1)
                nextParameter = value;
            else
                nextParameter = value.Substring(0, nPos).Trim();

            if (nextParameter.IndexOf("\"") == 0)
                identity = nextParameter.Replace("\"", "").Trim();
            else
                bool.TryParse(nextParameter, out indexer);
            if (nPos == -1)
                return;
            value = value.Trim();
            value = value.Substring(nPos + 1);
            value = value.Trim();
            nPos = value.IndexOf(',');



            if (nPos == -1)
                nextParameter = value.Trim();
            else
                nextParameter = value.Substring(0, nPos).Trim();

            if (identity == null && nextParameter.IndexOf("\"") == 0)
                identity = nextParameter.Replace("\"", "").Trim();
            else goto AssociationHostType;
            if (nPos == -1)
                return;
            value = value.Trim();
            value = value.Substring(nPos + 1);
            value = value.Trim();
            nPos = value.IndexOf(',');

            if (nPos == -1)
                nextParameter = value.Trim();
            else
                nextParameter = value.Substring(0, nPos).Trim();
        AssociationHostType:

            //generalAssociationHostType = nextParameter.Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();
            //if (nPos == -1)
            //    return;
            //value = value.Trim();
            //value = value.Substring(nPos + 1);
            //value = value.Trim();
            //nPos = value.IndexOf(',');

            //if (nPos == -1)
            //    nextParameter = value.Trim();
            //else
            //    nextParameter = value.Substring(0, nPos).Trim();


            generalAssociationIdentity = nextParameter.Replace("\"", "").Trim();

        }

        /// <MetaDataID>{28a35670-08cc-46cf-9ea8-bee6769c717c}</MetaDataID>
        internal static void GetAssociationData(EnvDTE.CodeAttribute codeAttribute,
                                                out string associationName,
                                                out MetaDataRepository.Classifier otherEndType,
                                                out MetaDataRepository.Roles associationEndRole,
                                                out string identity,
                                                out bool indexer,
            /* out MetaDataRepository.Classifier generalAssociationHostType,*/
                                                out string generalAssociationIdentity)
        {

            if (!IsAssociationAttribute(codeAttribute))
                throw new System.ArgumentException("The codeAttribute isn't AssociationAttribute.", "codeAttribute");
            associationName = null;
            otherEndType = null;
            associationEndRole = default(MetaDataRepository.Roles);
            identity = null;

            indexer = false;
            generalAssociationIdentity = null;
            Project project =null;
            Collections.Generic.List<string> projectItemNamespaces = null;
            try
            {

                project = MetaObjectMapper.FindMetaObjectFor(codeAttribute.ProjectItem.ContainingProject) as Project;

                if (project == null)
                    project = new Project(codeAttribute.ProjectItem.ContainingProject);
                projectItemNamespaces = new OOAdvantech.Collections.Generic.List<string>();
                EnvDTE.CodeNamespace projectItemNamespace = GetNamespace(codeAttribute.Parent as EnvDTE.CodeElement);
                if (projectItemNamespace != null)
                {
                    projectItemNamespaces.Add(projectItemNamespace.FullName);
                    MetaDataRepository.Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(projectItemNamespace.FullName) as MetaDataRepository.Namespace;
                    if (_namespace == null)
                        _namespace = new Namespace(projectItemNamespace.FullName);

                    projectItemNamespaces.Add(_namespace.FullName);
                    while (_namespace.Namespace != null)
                    {
                        _namespace = _namespace.Namespace;
                        projectItemNamespaces.Add(_namespace.FullName);
                    }

                }

                foreach (EnvDTE.CodeElement codeElement in codeAttribute.ProjectItem.FileCodeModel.CodeElements)
                {

                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                        projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
                }
                if (projectItemNamespace != null)
                {
                    foreach (EnvDTE.CodeElement codeElement in projectItemNamespace.Members)
                    {
                        if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                            projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
                    }

                }
                string value = codeAttribute.Value;
                value = value.Trim();
                int nPos = value.IndexOf(',');
                if (nPos == -1)
                {
                    associationName = value.Replace("\"", "").Trim();
                    return;
                }
                else
                    associationName = value.Substring(0, nPos).Replace("\"", "").Trim();
                value = value.Substring(nPos + 1);
                string otherEndTypeDefinition = null;
                value = value.Trim();
                nPos = value.IndexOf(',');
                if (nPos == -1)
                    otherEndTypeDefinition = value.Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();
                else
                    otherEndTypeDefinition = value.Substring(0, nPos).Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();
                otherEndType = project.GetExternalClassifier(otherEndTypeDefinition, true);
                if (otherEndType == null)
                {
                    foreach (string namespaceFullName in projectItemNamespaces)
                    {
                        otherEndType = project.GetExternalClassifier(namespaceFullName + "." + otherEndTypeDefinition, true);
                        if (otherEndType != null)
                            break;
                    }
                }
                if (nPos == -1)
                    return;
                if(value.IndexOf("Roles")!=0)
                    value = value.Substring(nPos + 1);
                string associationEndRoleDefinition = null;
                value = value.Trim();
                nPos = value.IndexOf(',');
                if (nPos == -1)
                    associationEndRoleDefinition = value;
                else
                    associationEndRoleDefinition = value.Substring(0, nPos);

                associationEndRoleDefinition = associationEndRoleDefinition.Substring(associationEndRoleDefinition.LastIndexOf('.') + 1);

                if (associationEndRoleDefinition == "RoleA")
                    associationEndRole = MetaDataRepository.Roles.RoleA;
                else
                    associationEndRole = MetaDataRepository.Roles.RoleB;



                if (nPos == -1)
                    return;
                value = value.Trim();
                value = value.Substring(nPos + 1);
                value = value.Trim();
                nPos = value.IndexOf(',');
                string nextParameter = null;
                if (nPos == -1)
                    nextParameter = value;
                else
                    nextParameter = value.Substring(0, nPos).Trim();

                if (nextParameter.IndexOf("\"") == 0)
                    identity = nextParameter.Replace("\"", "").Trim();
                else
                    bool.TryParse(nextParameter, out indexer);
                if (nPos == -1)
                    return;
                value = value.Trim();
                value = value.Substring(nPos + 1);
                value = value.Trim();
                nPos = value.IndexOf(',');



                if (nPos == -1)
                    nextParameter = value.Trim();
                else
                    nextParameter = value.Substring(0, nPos).Trim();

                if (identity == null && nextParameter.IndexOf("\"") == 0)
                    identity = nextParameter.Replace("\"", "").Trim();
                else goto AssociationHostType;
                if (nPos == -1)
                    return;
                value = value.Trim();
                value = value.Substring(nPos + 1);
                value = value.Trim();
                nPos = value.IndexOf(',');

                if (nPos == -1)
                    nextParameter = value.Trim();
                else
                    nextParameter = value.Substring(0, nPos).Trim();
            AssociationHostType:

                generalAssociationIdentity = nextParameter.Replace("\"", "").Trim();
            }
            finally
            {
                if (otherEndType == null && project!=null)
                {
                    if (codeAttribute != null && codeAttribute.Parent is EnvDTE.CodeProperty)
                    {
                        MetaDataRepository.Classifier classifier = project.GetClassifier((codeAttribute.Parent as EnvDTE.CodeProperty).Type);
                        if (classifier.TemplateBinding != null)
                        {
                            foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                            {

                                if (generalClassifier.Name.IndexOf("IEnumerable`") == 0)
                                    otherEndType = generalClassifier.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as MetaDataRepository.Classifier;
                                if (generalClassifier.Name.IndexOf("IDictionary`") == 0)
                                    otherEndType = generalClassifier.TemplateBinding.ParameterSubstitutions[1].ActualParameters[0] as MetaDataRepository.Classifier;

                            }
                        }
                        else
                        {

                            if (classifier is UnknownClassifier)
                            {
                                string codeTypeFullName = (codeAttribute.Parent as EnvDTE.CodeProperty).Type.AsFullName;
                                MetaDataRepository.Classifier tempClassifier = project.GetClassifier(project.VSProject.CodeModel.CreateCodeTypeRef(codeTypeFullName));
                                if (tempClassifier != null && !(tempClassifier is UnknownClassifier))
                                    classifier = tempClassifier;
                                else
                                {
                                    foreach (var _namespace in projectItemNamespaces)
                                    {
                                        tempClassifier = project.GetClassifier(project.VSProject.CodeModel.CreateCodeTypeRef( _namespace+"."+codeTypeFullName));
                                        if (tempClassifier != null && !(tempClassifier is UnknownClassifier))
                                        {
                                            classifier = tempClassifier;
                                            break;
                                        }
                                    }
                                }
                            }
                             
                           // VSProject.CodeModel.CreateCodeTy)peRef(fullName)
                            otherEndType = classifier ;
                        }
                    }
                    else if (codeAttribute != null && codeAttribute.Parent is EnvDTE.CodeVariable)
                    {
                        MetaDataRepository.Classifier classifier = project.GetClassifier((codeAttribute.Parent as EnvDTE.CodeVariable).Type);
                        if (classifier.TemplateBinding != null)
                        {
                            foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                            {
                                

                                if (generalClassifier.Name.IndexOf("IEnumerable`") == 0)
                                    otherEndType = generalClassifier.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as MetaDataRepository.Classifier;

                                if (generalClassifier.Name.IndexOf("IDictionary`") == 0)
                                    otherEndType = generalClassifier.TemplateBinding.ParameterSubstitutions[1].ActualParameters[0] as MetaDataRepository.Classifier;

                                
                            }
                        }
                        else
                            otherEndType = classifier;
                    }


                }
            }

        }
        internal static bool IsEnumerable(EnvDTE.CodeProperty codeProperty)
        {
            Project project = MetaObjectMapper.FindMetaObjectFor(codeProperty.ProjectItem.ContainingProject) as Project;
            MetaDataRepository.Classifier classifier = project.GetClassifier(codeProperty.Type);
            if (classifier.TemplateBinding != null)
            {
                foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                {
                    if (generalClassifier.Name.IndexOf("IEnumerable`") == 0)
                        return true;
                    if (generalClassifier.Name.IndexOf("IDictionary`") == 0)
                        return true;
                }
                return false;
            }
            else
                return false;

        }

        internal static bool IsEnumerable(EnvDTE.CodeVariable codeVariable)
        {
            Project project = MetaObjectMapper.FindMetaObjectFor(codeVariable.ProjectItem.ContainingProject) as Project;
            MetaDataRepository.Classifier classifier = project.GetClassifier(codeVariable.Type);
            if (classifier.TemplateBinding != null)
            {
                foreach (MetaDataRepository.Classifier generalClassifier in classifier.GetAllGeneralClasifiers())
                {
                    if (generalClassifier.Name.IndexOf("IEnumerable`") == 0)
                        return true;
                    if (generalClassifier.Name.IndexOf("IDictionary`") == 0)
                        return true;
                }
                return false;
            }
            else
                return false;

        }



        /// <MetaDataID>{79b8683b-ab58-4d29-86f7-a3275b3231bf}</MetaDataID>
        internal static void GetAssociationData(EnvDTE.CodeAttribute codeAttribute,
                                         out string associationName,
                                         out MetaDataRepository.Roles associationEndRole,
                                         out string identity)
        {
            if (!IsAssociationAttribute(codeAttribute))
                throw new System.ArgumentException("The codeAttribute isn't AssociationAttribute.", "codeAttribute");

            associationName = null;

            associationEndRole = default(MetaDataRepository.Roles);
            identity = null;






            Project project = MetaObjectMapper.FindMetaObjectFor(codeAttribute.ProjectItem.ContainingProject) as Project;
            Collections.Generic.List<string> projectItemNamespaces = new OOAdvantech.Collections.Generic.List<string>();
            EnvDTE.CodeNamespace projectItemNamespace = null;
            foreach (EnvDTE.CodeElement codeElement in codeAttribute.ProjectItem.FileCodeModel.CodeElements)
            {
                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
                {
                    projectItemNamespace = codeElement as EnvDTE.CodeNamespace;
                    projectItemNamespaces.Add(projectItemNamespace.FullName);
                }
                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                    projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
            }
            if (projectItemNamespace != null)
            {
                foreach (EnvDTE.CodeElement codeElement in projectItemNamespace.Members)
                {
                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                        projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
                }
                MetaDataRepository.Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(projectItemNamespace.FullName) as MetaDataRepository.Namespace;
                if (_namespace == null)
                    _namespace = new Namespace(projectItemNamespace.FullName);

                projectItemNamespaces.Add(_namespace.FullName);
                while (_namespace.Namespace != null)
                {
                    _namespace = _namespace.Namespace;
                    projectItemNamespaces.Add(_namespace.FullName);
                }

            }
            string value = codeAttribute.Value;
            value = value.Trim();
            int nPos = value.IndexOf(',');
            if (nPos == -1)
            {
                associationName = value.Replace("\"", "").Trim();
                return;
            }
            else
                associationName = value.Substring(0, nPos).Replace("\"", "").Trim();
            value = value.Substring(nPos + 1);
            string otherEndTypeDefinition = null;
            value = value.Trim();
            nPos = value.IndexOf(',');
            if (nPos == -1)
                otherEndTypeDefinition = value.Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();
            else
                otherEndTypeDefinition = value.Substring(0, nPos).Replace("typeof", "").Replace("(", "").Replace(")", "").Trim();
            if (nPos == -1)
                return;
            value = value.Substring(nPos + 1);
            string associationEndRoleDefinition = null;
            value = value.Trim();
            nPos = value.IndexOf(',');
            if (nPos == -1)
                associationEndRoleDefinition = value;
            else
                associationEndRoleDefinition = value.Substring(0, nPos);

            associationEndRoleDefinition = associationEndRoleDefinition.Substring(associationEndRoleDefinition.LastIndexOf('.') + 1);

            if (associationEndRoleDefinition == "RoleA")
                associationEndRole = MetaDataRepository.Roles.RoleA;
            else
                associationEndRole = MetaDataRepository.Roles.RoleB;



            if (nPos == -1)
                return;
            value = value.Trim();
            value = value.Substring(nPos + 1);
            value = value.Trim();
            nPos = value.IndexOf(',');
            string nextParameter = null;
            if (nPos == -1)
                nextParameter = value;
            else
                nextParameter = value.Substring(0, nPos).Trim();

            if (nextParameter.IndexOf("\"") == 0)
                identity = nextParameter.Replace("\"", "").Trim();
            if (nPos == -1)
                return;
            value = value.Trim();
            value = value.Substring(nPos + 1);
            value = value.Trim();
            nPos = value.IndexOf(',');



            if (nPos == -1)
                nextParameter = value.Trim();
            else
                nextParameter = value.Substring(0, nPos).Trim();

            if (identity == null && nextParameter.IndexOf("\"") == 0)
                identity = nextParameter.Replace("\"", "").Trim();

            return;
            //if (nPos == -1)
            //    return;
            //value = value.Trim();
            //value = value.Substring(nPos + 1);
            //value = value.Trim();
            //nPos = value.IndexOf(',');

            //if (nPos == -1)
            //    nextParameter = value.Trim();
            //else
            //    nextParameter = value.Substring(0, nPos).Trim();




        }


        /// <MetaDataID>{3bb6a691-df12-491d-a339-da2bd3214b25}</MetaDataID>
        internal static bool IsAssociationClassAttribute(EnvDTE.CodeAttribute codeAttribute)
        {
            string attributeName = null;
            try
            {
                attributeName = codeAttribute.FullName;
            }
            catch (System.Exception error)
            {
                attributeName = codeAttribute.Name;
            }
            if ((attributeName == typeof(MetaDataRepository.AssociationClass).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, codeAttribute as EnvDTE.CodeElement)))
                return true;
            else
            {
                attributeName += "Attribute";
                if ((attributeName == typeof(MetaDataRepository.AssociationClass).FullName || attributeName == LanguageParser.GetShortName(typeof(MetaDataRepository.AssociationClass).FullName, codeAttribute as EnvDTE.CodeElement)))
                    return true;
                else
                    return false;
            }
        }



        /// <MetaDataID>{70f745ba-a7f7-460c-97c5-329df759d23a}</MetaDataID>
        internal static void GetAssociationClassData(EnvDTE.CodeAttribute codeAttribute, out MetaDataRepository.Classifier associationClass)
        {
            if (!IsAssociationClassAttribute(codeAttribute))
                throw new System.ArgumentException("The codeAttribute isn't AssociationClass.", "codeAttribute");

            Project project = MetaObjectMapper.FindMetaObjectFor(codeAttribute.ProjectItem.ContainingProject) as Project;
            Collections.Generic.List<string> projectItemNamespaces = new OOAdvantech.Collections.Generic.List<string>();
            EnvDTE.CodeNamespace projectItemNamespace = null;
            foreach (EnvDTE.CodeElement codeElement in codeAttribute.ProjectItem.FileCodeModel.CodeElements)
            {
                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
                    projectItemNamespace = codeElement as EnvDTE.CodeNamespace;
                if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                    projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
            }
            if (projectItemNamespace != null)
            {
                foreach (EnvDTE.CodeElement codeElement in projectItemNamespace.Members)
                {
                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementImportStmt)
                        projectItemNamespaces.Add((codeElement as EnvDTE80.CodeImport).Namespace);
                }
                MetaDataRepository.Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(projectItemNamespace.FullName) as MetaDataRepository.Namespace;
                projectItemNamespaces.Add(_namespace.FullName);
                while (_namespace.Namespace != null)
                {
                    _namespace = _namespace.Namespace;
                    projectItemNamespaces.Add(_namespace.FullName);
                }

            }
            string otherEndTypeDefinition = codeAttribute.Value.Replace("typeof", "").Replace("(", "").Replace(")", "");
            associationClass = project.GetClassifier(otherEndTypeDefinition, true);
            if (associationClass == null)
            {
                foreach (string namespaceFullName in projectItemNamespaces)
                {
                    associationClass = project.GetClassifier(namespaceFullName + "." + otherEndTypeDefinition, true);
                    if (associationClass != null)
                        break;
                }
            }
        }

        /// <MetaDataID>{02892d92-7ef7-4d87-ad37-164117e30a25}</MetaDataID>
        internal static void AddSetter(EnvDTE.CodeProperty codeProperty, EnvDTE.CodeElement owner)
        {
            if (codeProperty.Getter == null)
            {
                string docComment = codeProperty.DocComment;
                string type = codeProperty.Type.AsFullName;
                EnvDTE.vsCMAccess access = codeProperty.Access;
                string name = codeProperty.Name;

                List<AtrributeData> attributes = new List<AtrributeData>();
                foreach (EnvDTE.CodeAttribute attribute in codeProperty.Attributes)
                    attributes.Add(new AtrributeData(attribute.FullName, attribute.Value));
                int attributePosition = 0;
                if (owner is EnvDTE.CodeClass)
                {
                    EnvDTE.CodeClass _class = owner as EnvDTE.CodeClass;
                    _class.RemoveMember(codeProperty);
                    codeProperty = _class.AddProperty(null, name, type, 0, access, null);
                    codeProperty.DocComment = docComment;
                    foreach (AtrributeData attributeData in attributes)
                        codeProperty.AddAttribute(LanguageParser.GetShortName(attributeData.FullName, codeProperty as EnvDTE.CodeElement).Replace("Attribute", ""), attributeData.Value, attributePosition++);

                }
                if (owner is EnvDTE.CodeInterface)
                {
                    EnvDTE.CodeInterface _interface = owner as EnvDTE.CodeInterface;
                    _interface.RemoveMember(codeProperty);
                    codeProperty = _interface.AddProperty(null, name, type, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null);
                    codeProperty.DocComment = docComment;
                    foreach (AtrributeData attributeData in attributes)
                        codeProperty.AddAttribute(LanguageParser.GetShortName(attributeData.FullName, codeProperty as EnvDTE.CodeElement).Replace("Attribute", ""), attributeData.Value, attributePosition++);

                }
                if (owner is EnvDTE.CodeStruct)
                {
                    EnvDTE.CodeStruct _struct = owner as EnvDTE.CodeStruct;
                    _struct.RemoveMember(codeProperty);
                    codeProperty = _struct.AddProperty(null, name, type, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null);
                    codeProperty.DocComment = docComment;
                    foreach (AtrributeData attributeData in attributes)
                        codeProperty.AddAttribute(LanguageParser.GetShortName(attributeData.FullName, codeProperty as EnvDTE.CodeElement).Replace("Attribute", ""), attributeData.Value, attributePosition++);

                }
                return;
            }

            EnvDTE.EditPoint editPoint = codeProperty.Getter.StartPoint.CreateEditPoint();
            editPoint.StartOfLine();
            string offsetText = editPoint.GetText(codeProperty.Getter.StartPoint.LineCharOffset - 1);
            if (owner is EnvDTE.CodeClass)
                codeProperty.Getter.StartPoint.CreateEditPoint().Insert("set\n" + offsetText + "{\n" + offsetText + "\n" + offsetText + "}\n" + offsetText);
            else
                codeProperty.Getter.StartPoint.CreateEditPoint().Insert("set;\n");



        }
        struct AtrributeData
        {
            public readonly string FullName;
            public readonly string Value;
            public AtrributeData(string fullName, string value)
            {
                FullName = fullName;
                Value = value;
            }

        }

        /// <MetaDataID>{61864956-c0a2-4618-9e86-d01b18b95ad4}</MetaDataID>
        internal static void AddGetter(EnvDTE.CodeProperty codeProperty, EnvDTE.CodeElement owner)
        {
            if (codeProperty.Setter == null)
            {
                string docComment = codeProperty.DocComment;
                string type = codeProperty.Type.AsFullName;
                EnvDTE.vsCMAccess access = codeProperty.Access;
                List<AtrributeData> attributes = new List<AtrributeData>();
                foreach (EnvDTE.CodeAttribute attribute in codeProperty.Attributes)
                    attributes.Add(new AtrributeData(attribute.Name, attribute.Value));
                int attributePosition = 0;
                string name = codeProperty.Name;
                if (owner is EnvDTE.CodeClass)
                {
                    EnvDTE.CodeClass _class = owner as EnvDTE.CodeClass;
                    _class.RemoveMember(codeProperty);
                    codeProperty = _class.AddProperty(name, null, type, 0, access, null);
                    codeProperty.DocComment = docComment;
                    foreach (AtrributeData attributeData in attributes)
                        codeProperty.AddAttribute(LanguageParser.GetShortName(attributeData.FullName, codeProperty as EnvDTE.CodeElement).Replace("Attribute", ""), attributeData.Value, attributePosition++);
                }
                if (owner is EnvDTE.CodeInterface)
                {
                    EnvDTE.CodeInterface _interface = owner as EnvDTE.CodeInterface;
                    _interface.RemoveMember(codeProperty);
                    codeProperty = _interface.AddProperty(name, null, type, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null);
                    codeProperty.DocComment = docComment;
                    foreach (AtrributeData attributeData in attributes)
                        codeProperty.AddAttribute(LanguageParser.GetShortName(attributeData.FullName, codeProperty as EnvDTE.CodeElement).Replace("Attribute", ""), attributeData.Value, attributePosition++);
                }
                if (owner is EnvDTE.CodeStruct)
                {
                    EnvDTE.CodeStruct _struct = owner as EnvDTE.CodeStruct;
                    _struct.RemoveMember(codeProperty);
                    codeProperty = _struct.AddProperty(name, null, type, 0, EnvDTE.vsCMAccess.vsCMAccessDefault, null);
                    codeProperty.DocComment = docComment;
                    foreach (AtrributeData attributeData in attributes)
                        codeProperty.AddAttribute(LanguageParser.GetShortName(attributeData.FullName, codeProperty as EnvDTE.CodeElement).Replace("Attribute", ""), attributeData.Value, attributePosition++);
                }

                return;
            }
            else
            {

                EnvDTE.EditPoint editPoint = codeProperty.Setter.StartPoint.CreateEditPoint();
                editPoint.StartOfLine();
                string offsetText = editPoint.GetText(codeProperty.Setter.StartPoint.LineCharOffset - 1);
                if (owner is EnvDTE.CodeClass || owner is EnvDTE.CodeStruct)
                    codeProperty.Setter.StartPoint.CreateEditPoint().Insert("get\n" + offsetText + "{\n" + offsetText + "\n" + offsetText + "}\n" + offsetText);
                else
                    codeProperty.Setter.StartPoint.CreateEditPoint().Insert("get;\n" + offsetText);
            }

        }





        public static string GetShortTemplateInstantiationName(OOAdvantech.MetaDataRepository.TemplateBinding templateBinding, string language, EnvDTE.CodeElement codeElement)
        {
            string name = (templateBinding.Signature.Template as MetaDataRepository.MetaObject).Name;
            name = name.Substring(0, name.IndexOf("`"));
            string templateParma = null;
            foreach (MetaDataRepository.TemplateParameterSubstitution parameterSubstitution in templateBinding.ParameterSubstitutions)
            {
                MetaDataRepository.IParameterableElement parameterClassifier = parameterSubstitution.ActualParameters[0];
                //identityString += "[" + parameterClassifier.FullName + "," + parameterClassifier.ImplementationUnit.Identity + "]";
                if (templateParma == null)
                    templateParma += "<";
                else
                    templateParma += ",";

                if (parameterClassifier is MetaDataRepository.Classifier)
                    templateParma += GetShortName((parameterClassifier as MetaDataRepository.Classifier).FullName, codeElement);
                else
                    templateParma += parameterClassifier.Name;

            }
            if (templateParma != null)
                name += templateParma + ">";

            return name;
        }





        /// <MetaDataID>{3761dabd-fb03-4f77-9346-b48180f82bb7}</MetaDataID>
        public static string GetTemplateName(OOAdvantech.MetaDataRepository.TemplateSignature signature, string language)
        {

            string name = (signature.Template as MetaDataRepository.MetaObject).Name;
            if(name.IndexOf("`")!=-1)
                name = name.Substring(0, name.IndexOf("`"));

            //  string identityString = name;
            string templateParma = null;

            foreach (OOAdvantech.MetaDataRepository.TemplateParameter parameter in signature.OwnedParameters)
            {
                //MetaDataRepository.Classifier parameterClassifier = parameterSubstitution.ActualParameters[0] as MetaDataRepository.Classifier;
                //  identityString += "[" + parameter + "," + parameterClassifier.ImplementationUnit.Identity + "]";
                if (templateParma == null)
                    templateParma += "<";
                else
                    templateParma += ",";

                templateParma += parameter.Name;
            }
            if (templateParma != null)
                name += templateParma + ">";

            return name;
        }

        /// <MetaDataID>{74e64de2-621c-4fc9-a8e9-d0b4c2ddaa1f}</MetaDataID>
        public static string GetTemplateInstantiationName(OOAdvantech.MetaDataRepository.TemplateBinding templateBinding, string language)
        {
            string name = (templateBinding.Signature.Template as MetaDataRepository.MetaObject).Name;
            name = name.Substring(0, name.IndexOf("`"));
            string templateParma = null;
            foreach (MetaDataRepository.TemplateParameterSubstitution parameterSubstitution in templateBinding.ParameterSubstitutions)
            {
                MetaDataRepository.IParameterableElement parameterClassifier = parameterSubstitution.ActualParameters[0];
                //identityString += "[" + parameterClassifier.FullName + "," + parameterClassifier.ImplementationUnit.Identity + "]";
                if (templateParma == null)
                    templateParma += "<";
                else
                    templateParma += ",";

                if (parameterClassifier is MetaDataRepository.Classifier)
                    templateParma += (parameterClassifier as MetaDataRepository.Classifier).FullName;
                else
                    templateParma += parameterClassifier.Name;

            }
            if (templateParma != null)
                name += templateParma + ">";

            return name;
        }

        /// <MetaDataID>{6dfd323d-9abc-41ab-8a1e-9bcfde711824}</MetaDataID>
        public static string GetTypeFullName(OOAdvantech.MetaDataRepository.Classifier classifier, EnvDTE.Project project)
        {
            string typeFullName = null;
            string language = "";
            if (project != null)
                language = project.CodeModel.Language;
            return GetTypeFullName(classifier, language);
        }
        public static string GetTypeFullName(OOAdvantech.MetaDataRepository.Classifier classifier, string language)
        {
            string typeFullName = null;
            if (classifier.OwnedTemplateSignature != null)
                typeFullName = GetTemplateName(classifier.OwnedTemplateSignature, language);
            else if (classifier.TemplateBinding != null)
                typeFullName = GetTemplateInstantiationName(classifier.TemplateBinding, language);
            else
                typeFullName = classifier.Name;

            MetaDataRepository.Namespace _namespace = classifier.Namespace;
            while (_namespace != null)
            {
                typeFullName = _namespace.Name + "." + typeFullName;
                _namespace = _namespace.Namespace;
            }
            if (classifier.IsTemplate || classifier.IsTemplateInstantiation)
            {
                int pos = typeFullName.IndexOf("`");
                if (pos != -1)
                    typeFullName = typeFullName.Substring(0, pos);
            }
            return typeFullName;

        }

        /// <MetaDataID>{670651dd-a22b-427e-abc3-c5cfd89054de}</MetaDataID>
        public static string GetTypeName(OOAdvantech.MetaDataRepository.Classifier classifier, EnvDTE.Project project)
        {
            string typeName = null;
            string language = "";
            if (project != null)
                language = project.CodeModel.Language;
            if (classifier.OwnedTemplateSignature != null)
                typeName = GetTemplateName(classifier.OwnedTemplateSignature, language);
            else if (classifier.TemplateBinding != null)
                typeName = GetTemplateInstantiationName(classifier.TemplateBinding, language);
            else
                typeName = classifier.Name;
            return typeName;

            //MetaDataRepository.Namespace _namespace = classifier.Namespace;
            //while (_namespace != null)
            //{
            //    typeFullName = _namespace.Name + "." + typeFullName;
            //    _namespace = _namespace.Namespace;
            //}
            //if (classifier.IsTemplate || classifier.IsTemplateInstantiation)
            //{
            //    int pos = typeFullName.IndexOf("`");
            //    if (pos != -1)
            //        typeFullName = typeFullName.Substring(0, pos);
            //}
            //return typeFullName;


        }


        public static string GetShortTypeFullName(OOAdvantech.MetaDataRepository.Classifier classifier, EnvDTE.CodeElement codeElement)
        {

            string typeFullName = null;
            string language = "";
            if(codeElement!=null)
                language = codeElement.ProjectItem.ContainingProject.CodeModel.Language;

            if (classifier.OwnedTemplateSignature != null)
                typeFullName = GetTemplateName(classifier.OwnedTemplateSignature, language);
            else if (classifier.TemplateBinding != null)
                typeFullName = GetShortTemplateInstantiationName(classifier.TemplateBinding, language, codeElement);
            else
                typeFullName = classifier.Name;

            MetaDataRepository.Namespace _namespace = classifier.Namespace;
            while (_namespace != null)
            {
                typeFullName = _namespace.Name + "." + typeFullName;
                _namespace = _namespace.Namespace;
            }
            if (classifier.IsTemplate || classifier.IsTemplateInstantiation)
            {
                int pos = typeFullName.IndexOf("`");
                if (pos != -1)
                    typeFullName = typeFullName.Substring(0, pos);
            }
            typeFullName = GetShortName(typeFullName, codeElement);
            return typeFullName;


        }


        internal static string GetTemplateSignature(MetaDataRepository.TemplateSignature ownedTemplateSignature, EnvDTE.CodeElement codeElement)
        {
            string templateSignature = null;
            string language = "";
            if (codeElement != null)
                language = codeElement.ProjectItem.ContainingProject.CodeModel.Language;
            if (ownedTemplateSignature.OwnedParameters!=null&& ownedTemplateSignature.OwnedParameters.Count > 0)
            {
                
                foreach (var templateParameter in ownedTemplateSignature.OwnedParameters)
                {
                    if (templateSignature != null)
                        templateSignature += ", ";
                    else
                        templateSignature = "<";
                    templateSignature += templateParameter.Name;
                }
                templateSignature += ">";
            }
            return templateSignature;
        }
    }
}
