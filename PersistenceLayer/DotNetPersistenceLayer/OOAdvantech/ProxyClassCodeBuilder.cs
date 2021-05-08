using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace OOAdvantech
{
    /// <MetaDataID>{ac95bcf5-ed98-460b-8488-32004744991d}</MetaDataID>
    public class ProxyClassCodeBuilder
    {

        /// <MetaDataID>{c8dce2c6-bd70-4c7a-a6c1-c8a3ca3a035c}</MetaDataID>
        static public void BuildProxyAssembly(string assemblyFilePath, System.Type type)// System.Reflection.Assembly orgAssembly)
        {

            string fileName = "OrgTypeProxy.cs";

            Type orgType = type;

            //text writer to write the code
            TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create));

            //code generator and code provider
            ICodeGenerator codeGenerator = new CSharpCodeProvider().CreateGenerator();
            CSharpCodeProvider cdp = new CSharpCodeProvider();
            codeGenerator = cdp.CreateGenerator();

            //namespace and includes
            CodeNamespace samplesNamespace = new CodeNamespace(orgType.Namespace);
            //samplesNamespace.Imports.Add(new CodeNamespaceImport("System"));

            //declare a class
            CodeTypeDeclaration OrgTypeProxy = new CodeTypeDeclaration(orgType.Name);
            samplesNamespace.Types.Add(OrgTypeProxy);
            OrgTypeProxy.IsClass = true;
            CodeMemberField uriField = new CodeMemberField();
            uriField.Type = new CodeTypeReference("OOAdvantech.Remoting.ExtObjectUri");
            uriField.Name = "ExtObjectUri";
            OrgTypeProxy.Members.Add(uriField);

            foreach (System.Reflection.FieldInfo fieldInfo in orgType.GetFields())
            {
                CodeMemberProperty property = new CodeMemberProperty();
                property.Name = fieldInfo.Name;
                property.PrivateImplementationType = new CodeTypeReference(fieldInfo.FieldType);
                CodeMethodInvokeExpression methodInvokeOnSet = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("OOAdvantech.Remoting"), "Ionvoke");
                methodInvokeOnSet.Parameters.Add(new CodePrimitiveExpression("ExtObjectUri"));
                methodInvokeOnSet.Parameters.Add(new CodePrimitiveExpression("set_" + property.Name));
                methodInvokeOnSet.Parameters.Add(new CodeVariableReferenceExpression("value"));


                CodeMethodInvokeExpression methodInvokeOnGet = new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                           new CodeTypeReferenceExpression("OOAdvantech.Remoting"),
                               "Ionvoke",
                                   new CodeTypeReference[] {
                                    new CodeTypeReference(fieldInfo.FieldType),}),
                                             new CodeExpression[0]);
                methodInvokeOnGet.Parameters.Add(new CodeVariableReferenceExpression("ExtObjectUri"));
                methodInvokeOnGet.Parameters.Add(new CodePrimitiveExpression("get_" + property.Name));

                property.SetStatements.Add(methodInvokeOnSet);
                property.GetStatements.Add(new CodeMethodReturnStatement(methodInvokeOnGet));
                OrgTypeProxy.Members.Add(property);
            }

            foreach (System.Reflection.PropertyInfo propertyInfo in orgType.GetProperties())
            {
                CodeMemberProperty property = new CodeMemberProperty();
                property.Name = propertyInfo.Name;
                property.Type = new CodeTypeReference(propertyInfo.PropertyType);
                CodeMethodInvokeExpression methodInvokeOnSet = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("OOAdvantech.Remoting"), "Ionvoke");
                methodInvokeOnSet.Parameters.Add(new CodeVariableReferenceExpression("ExtObjectUri"));
                methodInvokeOnSet.Parameters.Add(new CodePrimitiveExpression("set_" + property.Name));

                methodInvokeOnSet.Parameters.Add(new CodeVariableReferenceExpression("value"));
                CodeMethodInvokeExpression methodInvokeOnGet = new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                           new CodeTypeReferenceExpression("OOAdvantech.Remoting"),
                               "Ionvoke",
                                   new CodeTypeReference[] {
                                    new CodeTypeReference(propertyInfo.PropertyType),}),
                                             new CodeExpression[0]);
                methodInvokeOnGet.Parameters.Add(new CodeVariableReferenceExpression("ExtObjectUri"));
                methodInvokeOnGet.Parameters.Add(new CodePrimitiveExpression("get_" + property.Name));
                property.SetStatements.Add(methodInvokeOnSet);
                property.GetStatements.Add(new CodeMethodReturnStatement(methodInvokeOnGet));
                OrgTypeProxy.Members.Add(property);
            }

            foreach (var methodInfo in orgType.GetMethods())
            {
                if (methodInfo.DeclaringType != orgType)
                    continue;
                if (methodInfo.IsSpecialName)
                    continue;
                CodeMemberMethod method = new CodeMemberMethod();
                method.Name = methodInfo.Name;
                method.ReturnType = new CodeTypeReference(methodInfo.ReturnType);

                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    CodeParameterDeclarationExpression parameter = new CodeParameterDeclarationExpression(new CodeTypeReference(parameterInfo.ParameterType), parameterInfo.Name);
                    method.Parameters.Add(parameter);
                }
                if (methodInfo.ReturnType != typeof(void))
                {
                    CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression(
                       new CodeMethodReferenceExpression(
                          new CodeTypeReferenceExpression("OOAdvantech.Remoting"),
                              "Ionvoke",
                                  new CodeTypeReference[] {
                                    new CodeTypeReference(methodInfo.ReturnType),}),
                                            new CodeExpression[0]);

                    methodInvoke.Parameters.Add(new CodeVariableReferenceExpression("ExtObjectUri"));
                    methodInvoke.Parameters.Add(new CodePrimitiveExpression(method.Name));
                    foreach (var parameterInfo in methodInfo.GetParameters())
                        methodInvoke.Parameters.Add(new CodeVariableReferenceExpression(parameterInfo.Name));
                    method.Statements.Add(new CodeMethodReturnStatement(methodInvoke));
                }
                else
                {
                    CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression(
                                       new CodeMethodReferenceExpression(
                                          new CodeTypeReferenceExpression("OOAdvantech.Remoting"),
                                              "Ionvoke"));
                    methodInvoke.Parameters.Add(new CodeVariableReferenceExpression("ExtObjectUri"));
                    methodInvoke.Parameters.Add(new CodePrimitiveExpression(method.Name));
                    foreach (var parameterInfo in methodInfo.GetParameters())
                        methodInvoke.Parameters.Add(new CodeVariableReferenceExpression(parameterInfo.Name));
                    method.Statements.Add(methodInvoke);
                }
                OrgTypeProxy.Members.Add(method);
            }
            //generate the source code file
            codeGenerator.GenerateCodeFromNamespace(samplesNamespace, tw, null);
            //close the text writer
            tw.Close();
        }

 
    }
}
