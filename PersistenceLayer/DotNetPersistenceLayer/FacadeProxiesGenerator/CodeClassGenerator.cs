
using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Linq;
using System.Collections.Generic;

namespace FacadeProxiesGenerator
{
    /// <summary>
    /// This code example creates a graph using a CodeCompileUnit and  
    /// generates source code for the graph using the CSharpCodeProvider.
    /// </summary>
    /// <MetaDataID>{517bcbda-12ee-4c97-88f1-854419717f02}</MetaDataID>
    class CodeClass
    {
        /// <summary>
        /// Define the compile unit to use for code generation. 
        /// </summary>
        //CodeCompileUnit targetUnit;


        /// <summary>
        /// The only class in the compile unit. This class contains 2 fields,
        /// 3 properties, a constructor, an entry point, and 1 simple method. 
        /// </summary>
        //CodeTypeDeclaration targetClass;

        /// <summary>
        /// The name of the file to contain the source code.
        /// </summary>
        private const string outputFileName = "SampleCode.cs";


        public static CodeTypeDeclaration CreateCodeType(System.Type type, CodeCompileUnit targetUnit)
        {

            CodeNamespace sample = (from _namespace in targetUnit.Namespaces.OfType<CodeNamespace>()
                                    where _namespace.Name == type.Namespace + ".Proxies"
                                    select _namespace).FirstOrDefault();
            if (sample == null)
            {
                sample = new CodeNamespace(type.Namespace + ".Proxies");
                sample.Imports.Add(new CodeNamespaceImport("System"));
                targetUnit.Namespaces.Add(sample);
            }
            var targetClass = new CodeTypeDeclaration("Pr_" + type.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;
            sample.Types.Add(targetClass);

            return targetClass;
        }
        public static CodeTypeDeclaration CreateCodeType(System.Reflection.EventInfo _event, CodeCompileUnit targetUnit)
        {

            CodeNamespace sample = (from _namespace in targetUnit.Namespaces.OfType<CodeNamespace>()
                                    where _namespace.Name == _event.DeclaringType.Namespace + ".Proxies"
                                    select _namespace).FirstOrDefault();
            if (sample == null)
            {
                sample = new CodeNamespace(_event.DeclaringType.Namespace + ".Proxies");
                sample.Imports.Add(new CodeNamespaceImport("System"));
                targetUnit.Namespaces.Add(sample);
            }
            var targetClass = new CodeTypeDeclaration("CNSPr_" + _event.DeclaringType.Name + "_" + _event.Name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;
            sample.Types.Add(targetClass);

            return targetClass;
        }


        //public Sample(System.Type type, CodeCompileUnit targetUnit)
        //{
        //    targetUnit = new CodeCompileUnit();
        //    CodeNamespace samples = new CodeNamespace(type.Namespace + ".Proxies");
        //    samples.Imports.Add(new CodeNamespaceImport("System"));
        //    targetClass = new CodeTypeDeclaration("Pr_" + type.Name);
        //    targetClass.IsClass = true;
        //    targetClass.TypeAttributes =
        //        TypeAttributes.Public | TypeAttributes.Sealed;
        //    samples.Types.Add(targetClass);
        //    targetUnit.Namespaces.Add(samples);
        //}

        /// <summary>
        /// Adds two fields to the class.
        /// </summary>
        public static void AddFields(CodeTypeDeclaration targetClass)
        {
            //// Declare the widthValue field.
            //CodeMemberField widthValueField = new CodeMemberField();
            //widthValueField.Attributes = MemberAttributes.Private;
            //widthValueField.Name = "widthValue";
            //widthValueField.Type = new CodeTypeReference(typeof(System.Double));
            //widthValueField.Comments.Add(new CodeCommentStatement(
            //    "The width of the object."));
            //targetClass.Members.Add(widthValueField);

            //// Declare the heightValue field
            //CodeMemberField heightValueField = new CodeMemberField();
            //heightValueField.Attributes = MemberAttributes.Private;
            //heightValueField.Name = "heightValue";
            //heightValueField.Type =
            //    new CodeTypeReference(typeof(System.Double));
            //heightValueField.Comments.Add(new CodeCommentStatement(
            //    "The height of the object."));
            //targetClass.Members.Add(heightValueField);


            CodeMemberField proxyField = new CodeMemberField();
            proxyField.Attributes = MemberAttributes.Private;
            proxyField.Name = "Proxy";
            proxyField.Type = new CodeTypeReference(typeof(OOAdvantech.Remoting.RestApi.Proxy));
            targetClass.Members.Add(proxyField);



        }



        internal static void AddBaseType(CodeTypeDeclaration targetClass, Type interaface)
        {
            targetClass.BaseTypes.Add(new CodeTypeReference(interaface));
        }

        internal static void AddEvent(EventInfo eventInfo, CodeTypeDeclaration targetClass, List<Assembly> assemblies)
        {

            var codeEvent = new CodeSnippetTypeMember();
            string addInvoke = $@"this.Proxy.Invoke(typeof({eventInfo.DeclaringType.FullName}), ""{eventInfo.AddMethod.Name}"",new object[] {{value}} , new Type[] {{ typeof({eventInfo.EventHandlerType.FullName})}});";

            string removeInvoke = $@"this.Proxy.Invoke(typeof({eventInfo.DeclaringType.FullName}), ""{eventInfo.RemoveMethod.Name}"",new object[] {{value}} , new Type[] {{ typeof({eventInfo.EventHandlerType.FullName})}});";
            codeEvent.Text = $@"public event {eventInfo.EventHandlerType.FullName} { eventInfo.Name}
            {{
                add
                {{
                    {addInvoke}
                }}
                remove
                {{
                    {removeInvoke}
                }}
            }}";




            //CodeMemberEvent codeEvent = new CodeMemberEvent();
            //codeEvent.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            //codeEvent.Name = eventInfo.Name;


            //codeEvent.Type = new CodeTypeReference(eventInfo.EventHandlerType);
            //codeEvent.Comments.Add(new CodeCommentStatement(
            //    "The Width property for the object."));

            if (!assemblies.Contains(eventInfo.EventHandlerType.Assembly))
                assemblies.Add(eventInfo.EventHandlerType.Assembly);


            ////if (codeProperty.HasGet)
            //{

            //    int numOfParmas = 0;
            //    CodeArrayCreateExpression argsArrrayCreation = new CodeArrayCreateExpression("System.Object", numOfParmas);
            //    CodeVariableDeclarationStatement argsArrrayDef = new CodeVariableDeclarationStatement("System.Object[]", "args", argsArrrayCreation);
            //    codeProperty.GetStatements.Add(argsArrrayDef);

            //    CodeArrayCreateExpression argsTypesArrayCreation = new CodeArrayCreateExpression(new CodeTypeReference(typeof(System.Type)), numOfParmas);
            //    CodeVariableDeclarationStatement argsTypesArrayDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Type[])), "argsTypes", argsTypesArrayCreation);
            //    codeProperty.GetStatements.Add(argsTypesArrayDef);
            //    var argsReference = new CodeVariableReferenceExpression("args");
            //    var argsTypesReference = new CodeVariableReferenceExpression("argsTypes");
            //    CodeFieldReferenceExpression proxyReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");

            //    CodeVariableDeclarationStatement retValDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Object)), "retValue", new CodeMethodInvokeExpression(proxyReference, "Invoke", new CodeTypeOfExpression(new CodeTypeReference(property.GetGetMethod().DeclaringType)), new CodePrimitiveExpression(property.GetGetMethod().Name), argsReference, argsTypesReference));
            //    codeProperty.GetStatements.Add(retValDef);

            //    CodeMethodReturnStatement returnValueStatement = new CodeMethodReturnStatement();
            //    returnValueStatement.Expression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(proxyReference, "GetValue", new CodeTypeReference[] { new CodeTypeReference(property.PropertyType) }), new CodeVariableReferenceExpression("retValue"));
            //    codeProperty.GetStatements.Add(returnValueStatement);

            //}
            //if (codeProperty.HasSet)
            //{
            //    int numOfParmas = 1;
            //    CodeArrayCreateExpression argsArrrayCreation = new CodeArrayCreateExpression("System.Object", numOfParmas);
            //    CodeVariableDeclarationStatement argsArrrayDef = new CodeVariableDeclarationStatement("System.Object[]", "args", argsArrrayCreation);
            //    codeProperty.SetStatements.Add(argsArrrayDef);

            //    CodeArrayCreateExpression argsTypesArrayCreation = new CodeArrayCreateExpression(new CodeTypeReference(typeof(System.Type)), numOfParmas);
            //    CodeVariableDeclarationStatement argsTypesArrayDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Type[])), "argsTypes", argsTypesArrayCreation);
            //    codeProperty.SetStatements.Add(argsTypesArrayDef);
            //    var argsReference = new CodeVariableReferenceExpression("args");
            //    var argsTypesReference = new CodeVariableReferenceExpression("argsTypes");
            //    CodeFieldReferenceExpression proxyReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");

            //    //System.Type paramType = parameter.ParameterType;
            //    var argsIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("args"), new CodePrimitiveExpression(0));
            //    codeProperty.SetStatements.Add(new CodeAssignStatement(argsIndexReference, new CodeArgumentReferenceExpression("value")));

            //    CodeTypeOfExpression paramTypeof = new CodeTypeOfExpression(new CodeTypeReference(property.PropertyType));

            //    var argsTypesIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("argsTypes"), new CodePrimitiveExpression(0));
            //    codeProperty.SetStatements.Add(new CodeAssignStatement(argsTypesIndexReference, paramTypeof));


            //    codeProperty.SetStatements.Add(new CodeMethodInvokeExpression(proxyReference, "Invoke", new CodeTypeOfExpression(new CodeTypeReference(property.GetGetMethod().DeclaringType)), new CodePrimitiveExpression(property.GetGetMethod().Name), argsReference, argsTypesReference));



            //    //returnValueStatement.Expression = new CodeVariableReferenceExpression("retValue");
            //    //widthProperty.SetStatements.Add(returnValueStatement);
            //}

            targetClass.Members.Add(codeEvent);
        }

        /// <summary>
        /// Add three properties to the class.
        /// </summary>
        public static void AddProperty(PropertyInfo property, CodeTypeDeclaration targetClass, List<System.Reflection.Assembly> assemblies)
        {
            // Declare the read-only Width property.
            CodeMemberProperty codeProperty = new CodeMemberProperty();
            codeProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            codeProperty.Name = property.Name;
            codeProperty.HasGet = property.GetMethod != null;
            codeProperty.HasSet = property.SetMethod != null;
            codeProperty.Type = new CodeTypeReference(property.PropertyType);
            codeProperty.Comments.Add(new CodeCommentStatement(
                "The Width property for the object."));

            if (!assemblies.Contains(property.PropertyType.Assembly))
                assemblies.Add(property.PropertyType.Assembly);


            if (codeProperty.HasGet)
            {
                int numOfParmas = 0;
                CodeArrayCreateExpression argsArrrayCreation = new CodeArrayCreateExpression("System.Object", numOfParmas);
                CodeVariableDeclarationStatement argsArrrayDef = new CodeVariableDeclarationStatement("System.Object[]", "args", argsArrrayCreation);
                codeProperty.GetStatements.Add(argsArrrayDef);

                CodeArrayCreateExpression argsTypesArrayCreation = new CodeArrayCreateExpression(new CodeTypeReference(typeof(System.Type)), numOfParmas);
                CodeVariableDeclarationStatement argsTypesArrayDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Type[])), "argsTypes", argsTypesArrayCreation);
                codeProperty.GetStatements.Add(argsTypesArrayDef);
                var argsReference = new CodeVariableReferenceExpression("args");
                var argsTypesReference = new CodeVariableReferenceExpression("argsTypes");
                CodeFieldReferenceExpression proxyReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");

                CodeVariableDeclarationStatement retValDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Object)), "retValue", new CodeMethodInvokeExpression(proxyReference, "Invoke", new CodeTypeOfExpression(new CodeTypeReference(property.GetGetMethod().DeclaringType)), new CodePrimitiveExpression(property.GetGetMethod().Name), argsReference, argsTypesReference));
                codeProperty.GetStatements.Add(retValDef);

                CodeMethodReturnStatement returnValueStatement = new CodeMethodReturnStatement();
                returnValueStatement.Expression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(proxyReference, "GetValue", new CodeTypeReference[] { new CodeTypeReference(property.PropertyType) }), new CodeVariableReferenceExpression("retValue"));
                codeProperty.GetStatements.Add(returnValueStatement);

            }
            if (codeProperty.HasSet)
            {
                int numOfParmas = 1;
                CodeArrayCreateExpression argsArrrayCreation = new CodeArrayCreateExpression("System.Object", numOfParmas);
                CodeVariableDeclarationStatement argsArrrayDef = new CodeVariableDeclarationStatement("System.Object[]", "args", argsArrrayCreation);
                codeProperty.SetStatements.Add(argsArrrayDef);

                CodeArrayCreateExpression argsTypesArrayCreation = new CodeArrayCreateExpression(new CodeTypeReference(typeof(System.Type)), numOfParmas);
                CodeVariableDeclarationStatement argsTypesArrayDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Type[])), "argsTypes", argsTypesArrayCreation);
                codeProperty.SetStatements.Add(argsTypesArrayDef);
                var argsReference = new CodeVariableReferenceExpression("args");
                var argsTypesReference = new CodeVariableReferenceExpression("argsTypes");
                CodeFieldReferenceExpression proxyReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");

                //System.Type paramType = parameter.ParameterType;
                var argsIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("args"), new CodePrimitiveExpression(0));
                codeProperty.SetStatements.Add(new CodeAssignStatement(argsIndexReference, new CodeArgumentReferenceExpression("value")));

                CodeTypeOfExpression paramTypeof = new CodeTypeOfExpression(new CodeTypeReference(property.PropertyType));

                var argsTypesIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("argsTypes"), new CodePrimitiveExpression(0));
                codeProperty.SetStatements.Add(new CodeAssignStatement(argsTypesIndexReference, paramTypeof));


                codeProperty.SetStatements.Add(new CodeMethodInvokeExpression(proxyReference, "Invoke", new CodeTypeOfExpression(new CodeTypeReference(property.GetSetMethod().DeclaringType)), new CodePrimitiveExpression(property.GetSetMethod().Name), argsReference, argsTypesReference));



                //returnValueStatement.Expression = new CodeVariableReferenceExpression("retValue");
                //widthProperty.SetStatements.Add(returnValueStatement);
            }

            targetClass.Members.Add(codeProperty);
            //widthProperty.GetStatements.Add(new CodeMethodReturnStatement(
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "widthValue")));
            //targetClass.Members.Add(widthProperty);

            //// Declare the read-only Height property.
            //CodeMemberProperty heightProperty = new CodeMemberProperty();
            //heightProperty.Attributes =
            //    MemberAttributes.Public | MemberAttributes.Final;
            //heightProperty.Name = "Height";
            //heightProperty.HasGet = true;
            //heightProperty.Type = new CodeTypeReference(typeof(System.Double));
            //heightProperty.Comments.Add(new CodeCommentStatement(
            //    "The Height property for the object."));
            //heightProperty.GetStatements.Add(new CodeMethodReturnStatement(
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "heightValue")));
            //targetClass.Members.Add(heightProperty);

            //// Declare the read only Area property.
            //CodeMemberProperty areaProperty = new CodeMemberProperty();
            //areaProperty.Attributes =
            //    MemberAttributes.Public | MemberAttributes.Final;
            //areaProperty.Name = "Area";
            //areaProperty.HasGet = true;
            //areaProperty.Type = new CodeTypeReference(typeof(System.Double));
            //areaProperty.Comments.Add(new CodeCommentStatement(
            //    "The Area property for the object."));

            //// Create an expression to calculate the area for the get accessor 
            //// of the Area property.
            //CodeBinaryOperatorExpression areaExpression =
            //    new CodeBinaryOperatorExpression(
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "widthValue"),
            //    CodeBinaryOperatorType.Multiply,
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "heightValue"));
            //areaProperty.GetStatements.Add(
            //    new CodeMethodReturnStatement(areaExpression));
            //targetClass.Members.Add(areaProperty);
        }
        /// <summary>
        /// Add three properties to the class.
        /// </summary>
        public void AddProperties(CodeTypeDeclaration targetClass)
        {
            // Declare the read-only Width property.
            CodeMemberProperty widthProperty = new CodeMemberProperty();
            widthProperty.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            widthProperty.Name = "Width";
            widthProperty.HasGet = true;
            widthProperty.Type = new CodeTypeReference(typeof(System.Double));
            widthProperty.Comments.Add(new CodeCommentStatement(
                "The Width property for the object."));
            widthProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "widthValue")));
            targetClass.Members.Add(widthProperty);

            // Declare the read-only Height property.
            CodeMemberProperty heightProperty = new CodeMemberProperty();
            heightProperty.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            heightProperty.Name = "Height";
            heightProperty.HasGet = true;
            heightProperty.Type = new CodeTypeReference(typeof(System.Double));
            heightProperty.Comments.Add(new CodeCommentStatement(
                "The Height property for the object."));
            heightProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "heightValue")));
            targetClass.Members.Add(heightProperty);

            // Declare the read only Area property.
            CodeMemberProperty areaProperty = new CodeMemberProperty();
            areaProperty.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            areaProperty.Name = "Area";
            areaProperty.HasGet = true;
            areaProperty.Type = new CodeTypeReference(typeof(System.Double));
            areaProperty.Comments.Add(new CodeCommentStatement(
                "The Area property for the object."));

            // Create an expression to calculate the area for the get accessor 
            // of the Area property.
            CodeBinaryOperatorExpression areaExpression =
                new CodeBinaryOperatorExpression(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "widthValue"),
                CodeBinaryOperatorType.Multiply,
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "heightValue"));
            areaProperty.GetStatements.Add(
                new CodeMethodReturnStatement(areaExpression));
            targetClass.Members.Add(areaProperty);
        }

        public static void AddMethod(MethodInfo methodInfo, CodeTypeDeclaration targetClass, List<System.Reflection.Assembly> assemblies)
        {

            if (methodInfo.ReturnType != typeof(void) && methodInfo.ReturnType != null)
            {
                if (!assemblies.Contains(methodInfo.ReturnType.Assembly))
                {
                    assemblies.Add(methodInfo.ReturnType.Assembly);
                }
            }

            bool isSetAccessor = methodInfo.DeclaringType.GetProperties().Any(prop => prop.GetSetMethod() == methodInfo);
            if (isSetAccessor)
                return;
            bool isGetAccessor = methodInfo.DeclaringType.GetProperties().Any(prop => prop.GetGetMethod() == methodInfo);
            if (isGetAccessor)
                return;

            bool isAddEvent = methodInfo.DeclaringType.GetEvents().Any(prop => prop.AddMethod == methodInfo);
            if (isAddEvent)
                return;

            bool isRemoveEvent = methodInfo.DeclaringType.GetEvents().Any(prop => prop.RemoveMethod == methodInfo);
            if (isRemoveEvent)
                return;
            // Declaring a ToString method
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            codeMethod.Name = methodInfo.Name;
            codeMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType);



            foreach (var parameter in methodInfo.GetParameters())
            {
                System.Type paramType = parameter.ParameterType;

                if (!assemblies.Contains(paramType.Assembly))
                    assemblies.Add(paramType.Assembly);

                if (parameter.IsOut)
                    paramType = paramType.GetElementType();
                else if (parameter.ParameterType.IsByRef)
                    paramType = paramType.GetElementType();


                var parameterDeclaration = new CodeParameterDeclarationExpression(paramType, parameter.Name);
                if (parameter.IsOut)
                    parameterDeclaration.Direction = FieldDirection.Out;
                else if (parameter.ParameterType.IsByRef)
                    parameterDeclaration.Direction = FieldDirection.Ref;
                // Add parameters.
                codeMethod.Parameters.Add(parameterDeclaration);
            }
            int numOfParmas = methodInfo.GetParameters().Length;
            CodeArrayCreateExpression argsArrrayCreation = new CodeArrayCreateExpression("System.Object", numOfParmas);
            CodeVariableDeclarationStatement argsArrrayDef = new CodeVariableDeclarationStatement("System.Object[]", "args", argsArrrayCreation);
            codeMethod.Statements.Add(argsArrrayDef);

            CodeArrayCreateExpression argsTypesArrayCreation = new CodeArrayCreateExpression(new CodeTypeReference(typeof(System.Type)), numOfParmas);
            CodeVariableDeclarationStatement argsTypesArrayDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Type[])), "argsTypes", argsTypesArrayCreation);
            codeMethod.Statements.Add(argsTypesArrayDef);


            int i = 0;
            foreach (var parameter in methodInfo.GetParameters())
            {

                System.Type paramType = parameter.ParameterType;
                if (parameter.IsOut)
                    paramType = paramType.GetElementType();
                else if (parameter.ParameterType.IsByRef)
                    paramType = paramType.GetElementType();

                if (!parameter.IsOut)
                {
                    var argsIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("args"), new CodePrimitiveExpression(i));
                    codeMethod.Statements.Add(new CodeAssignStatement(argsIndexReference, new CodeArgumentReferenceExpression(parameter.Name)));
                }
                CodeTypeOfExpression paramTypeof = new CodeTypeOfExpression(new CodeTypeReference(paramType));

                var argsTypesIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("argsTypes"), new CodePrimitiveExpression(i));
                codeMethod.Statements.Add(new CodeAssignStatement(argsTypesIndexReference, paramTypeof));

                i++;

            }
            var argsReference = new CodeVariableReferenceExpression("args");
            var argsTypesReference = new CodeVariableReferenceExpression("argsTypes");
            CodeFieldReferenceExpression proxyReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");

            CodeVariableDeclarationStatement retValDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Object)), "retValue", new CodeMethodInvokeExpression(proxyReference, "Invoke", new CodeTypeOfExpression(new CodeTypeReference(methodInfo.DeclaringType)), new CodePrimitiveExpression(methodInfo.Name), argsReference, argsTypesReference));
            codeMethod.Statements.Add(retValDef);
            i = 0;
            foreach (var parameter in methodInfo.GetParameters())
            {
                if (parameter.ParameterType.IsByRef)
                {

                    var argsIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("args"), new CodePrimitiveExpression(i));
                    var getValueStatament = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(proxyReference, "GetValue", new CodeTypeReference[] { new CodeTypeReference(parameter.ParameterType.GetElementType()) }), argsIndexReference);
                    codeMethod.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression(parameter.Name), getValueStatament));
                }
                i++;
            }
            if (methodInfo.ReturnType != typeof(void))
            {
                // Declaring a return statement for method ToString.
                CodeMethodReturnStatement returnValueStatement = new CodeMethodReturnStatement();
                returnValueStatement.Expression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(proxyReference, "GetValue", new CodeTypeReference[] { new CodeTypeReference(methodInfo.ReturnType) }), new CodeVariableReferenceExpression("retValue"));
                codeMethod.Statements.Add(returnValueStatement);

                //CodeCastExpression castExpression = new CodeCastExpression(new CodeTypeReference(methodInfo.ReflectedType), new CodeVariableReferenceExpression("retValue"));
                //returnValueStatement.Expression = castExpression;

                //CodeMethodReturnStatement returnDefaultValueStatement = new CodeMethodReturnStatement();

                //returnDefaultValueStatement.Expression = new CodeDefaultValueExpression(new CodeTypeReference(methodInfo.ReturnType));


                //CodeConditionStatement conditionalStatement = new CodeConditionStatement(
                //        new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("retValue"), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(null)),
                //        // The statements to execute if the condition evaluates to true.
                //        new CodeStatement[] { returnDefaultValueStatement },
                //        // The statements to execute if the condition evalues to false.
                //        new CodeStatement[] { returnValueStatement });

                //codeMethod.Statements.Add(conditionalStatement);
            }

            //CodeFieldReferenceExpression widthReference =
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "Width");
            //CodeFieldReferenceExpression heightReference =
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "Height");
            //CodeFieldReferenceExpression areaReference =
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "Area");

            //// Declaring a return statement for method ToString.
            //CodeMethodReturnStatement returnStatement =
            //    new CodeMethodReturnStatement();

            //// This statement returns a string representation of the width,
            //// height, and area.
            //string formattedOutput = "The object:" + Environment.NewLine +
            //    " width = {0}," + Environment.NewLine +
            //    " height = {1}," + Environment.NewLine +
            //    " area = {2}";
            //returnStatement.Expression =
            //    new CodeMethodInvokeExpression(
            //    new CodeTypeReferenceExpression("System.String"), "Format",
            //    new CodePrimitiveExpression(formattedOutput),
            //    widthReference, heightReference, areaReference);


            //codeMethod.Statements.Add(returnStatement);
            targetClass.Members.Add(codeMethod);

        }



        public static void AddEventConsumMethod(MethodInfo methodInfo, CodeTypeDeclaration targetClass, List<System.Reflection.Assembly> assemblies)
        {

            if (methodInfo.ReturnType != typeof(void) && methodInfo.ReturnType != null)
            {
                if (!assemblies.Contains(methodInfo.ReturnType.Assembly))
                {
                    assemblies.Add(methodInfo.ReturnType.Assembly);
                }
            }

            bool isSetAccessor = methodInfo.DeclaringType.GetProperties().Any(prop => prop.GetSetMethod() == methodInfo);
            if (isSetAccessor)
                return;
            bool isGetAccessor = methodInfo.DeclaringType.GetProperties().Any(prop => prop.GetGetMethod() == methodInfo);
            if (isGetAccessor)
                return;

            bool isAddEvent = methodInfo.DeclaringType.GetEvents().Any(prop => prop.AddMethod == methodInfo);
            if (isAddEvent)
                return;

            bool isRemoveEvent = methodInfo.DeclaringType.GetEvents().Any(prop => prop.RemoveMethod == methodInfo);
            if (isRemoveEvent)
                return;
            // Declaring a ToString method
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            codeMethod.Name = methodInfo.Name;
            codeMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType);



            foreach (var parameter in methodInfo.GetParameters())
            {
                System.Type paramType = parameter.ParameterType;

                if (!assemblies.Contains(paramType.Assembly))
                    assemblies.Add(paramType.Assembly);

                if (parameter.IsOut)
                    paramType = paramType.GetElementType();
                else if (parameter.ParameterType.IsByRef)
                    paramType = paramType.GetElementType();


                var parameterDeclaration = new CodeParameterDeclarationExpression(paramType, parameter.Name);
                if (parameter.IsOut)
                    parameterDeclaration.Direction = FieldDirection.Out;
                else if (parameter.ParameterType.IsByRef)
                    parameterDeclaration.Direction = FieldDirection.Ref;
                // Add parameters.
                codeMethod.Parameters.Add(parameterDeclaration);
            }
            int numOfParmas = methodInfo.GetParameters().Length;
            CodeArrayCreateExpression argsArrrayCreation = new CodeArrayCreateExpression("System.Object", numOfParmas);
            CodeVariableDeclarationStatement argsArrrayDef = new CodeVariableDeclarationStatement("System.Object[]", "args", argsArrrayCreation);
            codeMethod.Statements.Add(argsArrrayDef);

            CodeArrayCreateExpression argsTypesArrayCreation = new CodeArrayCreateExpression(new CodeTypeReference(typeof(System.Type)), numOfParmas);
            CodeVariableDeclarationStatement argsTypesArrayDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Type[])), "argsTypes", argsTypesArrayCreation);
            codeMethod.Statements.Add(argsTypesArrayDef);


            int i = 0;
            foreach (var parameter in methodInfo.GetParameters())
            {

                System.Type paramType = parameter.ParameterType;
                if (parameter.IsOut)
                    paramType = paramType.GetElementType();
                else if (parameter.ParameterType.IsByRef)
                    paramType = paramType.GetElementType();

                if (!parameter.IsOut)
                {
                    var argsIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("args"), new CodePrimitiveExpression(i));
                    codeMethod.Statements.Add(new CodeAssignStatement(argsIndexReference, new CodeArgumentReferenceExpression(parameter.Name)));
                }
                CodeTypeOfExpression paramTypeof = new CodeTypeOfExpression(new CodeTypeReference(paramType));

                var argsTypesIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("argsTypes"), new CodePrimitiveExpression(i));
                codeMethod.Statements.Add(new CodeAssignStatement(argsTypesIndexReference, paramTypeof));

                i++;

            }
            var argsReference = new CodeVariableReferenceExpression("args");
            var argsTypesReference = new CodeVariableReferenceExpression("argsTypes");
            CodeThisReferenceExpression thisReference = new CodeThisReferenceExpression();// new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");

            CodeVariableDeclarationStatement retValDef = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(System.Object)), "retValue", new CodeMethodInvokeExpression(thisReference, "Invoke", new CodeTypeOfExpression(new CodeTypeReference(methodInfo.DeclaringType)), new CodePrimitiveExpression(methodInfo.Name), argsReference, argsTypesReference));
            codeMethod.Statements.Add(retValDef);
            i = 0;
            foreach (var parameter in methodInfo.GetParameters())
            {
                if (parameter.ParameterType.IsByRef)
                {

                    var argsIndexReference = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("args"), new CodePrimitiveExpression(i));
                    var getValueStatament = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(thisReference, "GetValue", new CodeTypeReference[] { new CodeTypeReference(parameter.ParameterType.GetElementType()) }), argsIndexReference);
                    codeMethod.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression(parameter.Name), getValueStatament));
                }
                i++;
            }
            if (methodInfo.ReturnType != typeof(void))
            {
                // Declaring a return statement for method ToString.
                CodeMethodReturnStatement returnValueStatement = new CodeMethodReturnStatement();
                returnValueStatement.Expression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(thisReference, "GetValue", new CodeTypeReference[] { new CodeTypeReference(methodInfo.ReturnType) }), new CodeVariableReferenceExpression("retValue"));
                codeMethod.Statements.Add(returnValueStatement);

                //CodeCastExpression castExpression = new CodeCastExpression(new CodeTypeReference(methodInfo.ReflectedType), new CodeVariableReferenceExpression("retValue"));
                //returnValueStatement.Expression = castExpression;

                //CodeMethodReturnStatement returnDefaultValueStatement = new CodeMethodReturnStatement();

                //returnDefaultValueStatement.Expression = new CodeDefaultValueExpression(new CodeTypeReference(methodInfo.ReturnType));


                //CodeConditionStatement conditionalStatement = new CodeConditionStatement(
                //        new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("retValue"), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(null)),
                //        // The statements to execute if the condition evaluates to true.
                //        new CodeStatement[] { returnDefaultValueStatement },
                //        // The statements to execute if the condition evalues to false.
                //        new CodeStatement[] { returnValueStatement });

                //codeMethod.Statements.Add(conditionalStatement);
            }

            //CodeFieldReferenceExpression widthReference =
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "Width");
            //CodeFieldReferenceExpression heightReference =
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "Height");
            //CodeFieldReferenceExpression areaReference =
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "Area");

            //// Declaring a return statement for method ToString.
            //CodeMethodReturnStatement returnStatement =
            //    new CodeMethodReturnStatement();

            //// This statement returns a string representation of the width,
            //// height, and area.
            //string formattedOutput = "The object:" + Environment.NewLine +
            //    " width = {0}," + Environment.NewLine +
            //    " height = {1}," + Environment.NewLine +
            //    " area = {2}";
            //returnStatement.Expression =
            //    new CodeMethodInvokeExpression(
            //    new CodeTypeReferenceExpression("System.String"), "Format",
            //    new CodePrimitiveExpression(formattedOutput),
            //    widthReference, heightReference, areaReference);


            //codeMethod.Statements.Add(returnStatement);
            targetClass.Members.Add(codeMethod);

        }


        /// <summary>
        /// Adds a method to the class. This method multiplies values stored 
        /// in both fields.
        /// </summary>
        public void AddMethod(CodeTypeDeclaration targetClass)
        {
            // Declaring a ToString method
            CodeMemberMethod toStringMethod = new CodeMemberMethod();
            toStringMethod.Attributes =
                MemberAttributes.Public | MemberAttributes.Override;
            toStringMethod.Name = "ToString";
            toStringMethod.ReturnType =
                new CodeTypeReference(typeof(System.String));

            CodeFieldReferenceExpression widthReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "Width");
            CodeFieldReferenceExpression heightReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "Height");
            CodeFieldReferenceExpression areaReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "Area");

            // Declaring a return statement for method ToString.
            CodeMethodReturnStatement returnStatement =
                new CodeMethodReturnStatement();

            // This statement returns a string representation of the width,
            // height, and area.
            string formattedOutput = "The object:" + Environment.NewLine +
                " width = {0}," + Environment.NewLine +
                " height = {1}," + Environment.NewLine +
                " area = {2}";
            returnStatement.Expression =
                new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.String"), "Format",
                new CodePrimitiveExpression(formattedOutput),
                widthReference, heightReference, areaReference);


            toStringMethod.Statements.Add(returnStatement);
            targetClass.Members.Add(toStringMethod);
        }
        /// <summary>
        /// Add a constructor to the class.
        /// </summary>
        public void AddConstructor(CodeTypeDeclaration targetClass)
        {
            // Declare the constructor
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;

            // Add parameters.
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(System.Double), "width"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(System.Double), "height"));

            // Add field initialization logic
            CodeFieldReferenceExpression widthReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "widthValue");
            constructor.Statements.Add(new CodeAssignStatement(widthReference,
                new CodeArgumentReferenceExpression("width")));
            CodeFieldReferenceExpression heightReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "heightValue");
            constructor.Statements.Add(new CodeAssignStatement(heightReference,
                new CodeArgumentReferenceExpression("height")));


            int numOfParmas = 2;
            CodeArrayCreateExpression ca1 = new CodeArrayCreateExpression("System.Object", numOfParmas);
            CodeVariableDeclarationStatement cv1 = new CodeVariableDeclarationStatement("System.Object[]", "args", ca1);
            constructor.Statements.Add(cv1);

            targetClass.Members.Add(constructor);
        }


        public static void AddProxyConstructor(CodeTypeDeclaration targetClass)
        {
            // Declare the constructor
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;

            // Add parameters.
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(OOAdvantech.Remoting.RestApi.Proxy), "proxy"));

            // Add field initialization logic
            CodeFieldReferenceExpression widthReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");
            constructor.Statements.Add(new CodeAssignStatement(widthReference, new CodeArgumentReferenceExpression("proxy")));



            targetClass.Members.Add(constructor);
        }

        internal static void AddConsumeEventproxyConstructor(CodeTypeDeclaration targetClass, EventInfo _event, List<System.Reflection.Assembly> assemblies)
        {


            //CodeMemberMethod codeMethod = new CodeMemberMethod();
            //codeMethod.Attributes =
            //    MemberAttributes.Public | MemberAttributes.Final;
            //codeMethod.Name = _event.DeclaringType.Name + "_" + _event.Name;

            var eventHandlerMethodInfo = _event.EventHandlerType.GetMethod("Invoke");
            AddEventConsumMethod(eventHandlerMethodInfo, targetClass, assemblies);

            //codeMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType);
            //foreach (var parameter in methodInfo.GetParameters())
            //{
            //    System.Type paramType = parameter.ParameterType;

            //    if (!assemblies.Contains(paramType.Assembly))
            //        assemblies.Add(paramType.Assembly);

            //    if (parameter.IsOut)
            //        paramType = paramType.GetElementType();
            //    else if (parameter.ParameterType.IsByRef)
            //        paramType = paramType.GetElementType();


            //    var parameterDeclaration = new CodeParameterDeclarationExpression(paramType, parameter.Name);
            //    if (parameter.IsOut)
            //        parameterDeclaration.Direction = FieldDirection.Out;
            //    else if (parameter.ParameterType.IsByRef)
            //        parameterDeclaration.Direction = FieldDirection.Ref;
            //    // Add parameters.
            //    codeMethod.Parameters.Add(parameterDeclaration);
            //}
            //targetClass.Members.Add(codeMethod);

            //codeMethod = new CodeMemberMethod();
            //codeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            //codeMethod.Name = _event.DeclaringType.Name + "_" + _event.Name;

            // codeMethod.ReturnType = new CodeTypeReference(typeof());

            // Declare the constructor
            CodeMemberMethod addEventHandler = new CodeMemberMethod();

            addEventHandler.Name = "AddEventHandler";

            addEventHandler.Attributes =
                MemberAttributes.Public | MemberAttributes.Override;

            // Add parameters.
            addEventHandler.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(object), "target"));

            // Add parameters.
            addEventHandler.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(System.Reflection.EventInfo), "eventInfo"));

            CodeFieldReferenceExpression methodReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), eventHandlerMethodInfo.Name);
            CodeObjectCreateExpression eventHandleCreation = new CodeObjectCreateExpression(_event.EventHandlerType, methodReference);

            var eventInfoReference = new CodeArgumentReferenceExpression("eventInfo");
            var addEventHandlerMethodCall = new CodeMethodInvokeExpression(eventInfoReference, "AddEventHandler", new CodeArgumentReferenceExpression("target"), eventHandleCreation);// new CodeTypeOfExpression(new CodeTypeReference(methodInfo.DeclaringType)), new CodePrimitiveExpression(methodInfo.Name), argsReference, argsTypesReference)
            addEventHandler.Statements.Add(addEventHandlerMethodCall);
            targetClass.Members.Add(addEventHandler);




            //codeMethod = new CodeMemberMethod();
            //codeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            //codeMethod.Name = _event.DeclaringType.Name + "_" + _event.Name;

            // codeMethod.ReturnType = new CodeTypeReference(typeof());

            // Declare the constructor
            CodeMemberMethod RemoveEventHandler = new CodeMemberMethod();

            RemoveEventHandler.Name = "RemoveEventHandler";
            RemoveEventHandler.Attributes =
                MemberAttributes.Public | MemberAttributes.Override;

            // Add parameters.
            RemoveEventHandler.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(object), "target"));

            // Add parameters.
            RemoveEventHandler.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(System.Reflection.EventInfo), "eventInfo"));

            methodReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), eventHandlerMethodInfo.Name);
            eventHandleCreation = new CodeObjectCreateExpression(_event.EventHandlerType, methodReference);

            eventInfoReference = new CodeArgumentReferenceExpression("eventInfo");
            var removeEventHandlerMethodCall = new CodeMethodInvokeExpression(eventInfoReference, "RemoveEventHandler", new CodeArgumentReferenceExpression("target"), eventHandleCreation);// new CodeTypeOfExpression(new CodeTypeReference(methodInfo.DeclaringType)), new CodePrimitiveExpression(methodInfo.Name), argsReference, argsTypesReference)
            RemoveEventHandler.Statements.Add(removeEventHandlerMethodCall);
            targetClass.Members.Add(RemoveEventHandler);


            //CodeFieldReferenceExpression proxyReference = new CodeFieldReferenceExpression(eventInfo, "Proxy");



            //// Add field initialization logic
            //CodeFieldReferenceExpression widthReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Proxy");
            //constructor.Statements.Add(new CodeAssignStatement(widthReference, new CodeArgumentReferenceExpression("proxy")));





        }

        /// <summary>
        /// Add an entry point to the class.
        /// </summary>
        public void AddEntryPoint(CodeTypeDeclaration targetClass)
        {
            CodeEntryPointMethod start = new CodeEntryPointMethod();
            CodeObjectCreateExpression objectCreate =
                new CodeObjectCreateExpression(
                new CodeTypeReference("CodeDOMCreatedClass"),
                new CodePrimitiveExpression(5.3),
                new CodePrimitiveExpression(6.9));

            // Add the statement:
            // "CodeDOMCreatedClass testClass = 
            //     new CodeDOMCreatedClass(5.3, 6.9);"
            start.Statements.Add(new CodeVariableDeclarationStatement(
                new CodeTypeReference("CodeDOMCreatedClass"), "testClass",
                objectCreate));

            // Creat the expression:
            // "testClass.ToString()"
            CodeMethodInvokeExpression toStringInvoke =
                new CodeMethodInvokeExpression(
                new CodeVariableReferenceExpression("testClass"), "ToString");

            // Add a System.Console.WriteLine statement with the previous 
            // expression as a parameter.
            start.Statements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", toStringInvoke));
            targetClass.Members.Add(start);
        }
        /// <summary>
        /// Generate CSharp source code from the compile unit.
        /// </summary>
        /// <param name="filename">Output file name</param>
        public static void GenerateCSharpCode(string fileName, CodeCompileUnit targetUnit)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            string code = null;
            string existingCode = null;

            if (System.IO.File.Exists(fileName))
            {
                using (StreamWriter sourceWriter = new StreamWriter(fileName + "~"))
                {
                    provider.GenerateCodeFromCompileUnit(
                        targetUnit, sourceWriter, options);
                }
                code = System.IO.File.ReadAllText(fileName + "~");
                existingCode = System.IO.File.ReadAllText(fileName);
                if (existingCode != code)
                    System.IO.File.WriteAllText(fileName, code);

                try
                {
                    System.IO.File.Delete(fileName + "~");
                }
                catch (Exception error)
                {
                }

            }
            else
            {
                using (StreamWriter sourceWriter = new StreamWriter(fileName))
                {
                    provider.GenerateCodeFromCompileUnit(
                        targetUnit, sourceWriter, options);
                }
            }

            //using (MemoryStream memStream = new MemoryStream())
            //{
            //    using (StreamWriter sourceWriter = new StreamWriter(memStream))
            //    {
            //        provider.GenerateCodeFromCompileUnit(
            //            targetUnit, sourceWriter, options);
            //        memStream.Position = 0;

            //        TextReader tr = new StreamReader(memStream);
            //        code = tr.ReadToEnd();
            //        //code =System.Text.Encoding.Unicode.GetString(memStream.ToArray());
            //    }

            //}

            //if (System.IO.File.Exists(fileName))
            //    existingCode = System.IO.File.ReadAllText(fileName);

            //if(existingCode!=code)
            //    System.IO.File.WriteAllText(fileName, code);

            //using (StreamWriter sourceWriter = new StreamWriter(fileName))
            //{
            //    provider.GenerateCodeFromCompileUnit(
            //        targetUnit, sourceWriter, options);
            //}
        }




    }
}