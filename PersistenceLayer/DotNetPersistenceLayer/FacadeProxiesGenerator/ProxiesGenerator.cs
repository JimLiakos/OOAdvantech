using OOAdvantech.Remoting;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FacadeProxiesGenerator
{

    /// <MetaDataID>{54e9d1c2-9d4c-488a-99e3-dcbdc4e5c19e}</MetaDataID>
    public enum ProxiesOutput
    {
        CSharp,
        Assembly
    }
    /// <MetaDataID>{7e8f5e4d-879d-493b-ad0e-42f9c6874e20}</MetaDataID>
    public class ProxiesGenerator
    {
        //OOAdvantech.Remoting.RestApi.Proxy Proxy;
        //public void foo(int y, out string name, ref int x)
        //{
        //    object[] args = new object[3];
        //    System.Type[] argsTypes = new System.Type[3];
        //    args[0] = y;
        //    argsTypes[0] = typeof(int);
        //    argsTypes[1] = typeof(string);
        //    args[2] = x;
        //    argsTypes[2] = typeof(int);
        //    object retValue = this.Proxy.Invoke(typeof(int), "foo", args, argsTypes);

        //    name = this.Proxy.GetValue<string>(args[1]);
        //    x = args[2];
        //}


        public static void GenerateProxiesAssembly(System.Reflection.Assembly assembly, string output, string errorFile)
        {
            GenerateProxies(assembly, ProxiesOutput.Assembly, output, errorFile);
        }

        public static void GenerateProxiesCode(System.Reflection.Assembly assembly, string output, string errorFile)
        {
            GenerateProxies(assembly, ProxiesOutput.CSharp, output, errorFile);
        }
        public static void GenerateProxies(System.Reflection.Assembly assembly, ProxiesOutput proxiesOutput, string output, string errorFile)
        {


            List<System.Reflection.Assembly> assemblies = new List<System.Reflection.Assembly>();
            CodeCompileUnit targetUnit = null;
            try
            {
                var ss = assembly.GetTypes();
                var tmpProxyInterfaces = (from _interface in assembly.DefinedTypes
                                       from attributeType in _interface.GetCustomAttributes(false)
                                       where _interface.IsInterface && _interface.IsPublic && attributeType.GetType().FullName == typeof(OOAdvantech.MetaDataRepository.GenerateFacadeProxy).FullName
                                       select _interface).ToList();

                var tmpConsumeEventproxies = (from _interface in assembly.GetTypes()
                                           from eventInfo in _interface.GetEvents()
                                           from attributeType in eventInfo.GetCustomAttributes(false)
                                           where _interface.IsInterface && attributeType.GetType().FullName == typeof(OOAdvantech.MetaDataRepository.GenerateEventConsumerProxy).FullName
                                           select eventInfo).ToList();
            }
            catch (Exception Error)
            {

                if (!string.IsNullOrWhiteSpace(errorFile))
                {
                    XDocument xmlDocument = null;
                    if (System.IO.File.Exists(errorFile))
                    {
                        xmlDocument = XDocument.Load(errorFile);
                        if(xmlDocument.Root.Name!= "Errors")
                            xmlDocument = XDocument.Parse("<Errors/>");
                    }
                    else
                        xmlDocument = XDocument.Parse("<Errors/>");
                    XElement errorsElement = xmlDocument.Root;
                    foreach (string refDll in ModulePublisher.ClassRepository.ErrorLoadingRefDll)
                    {

                        XElement errorElement = new XElement("Error");
                        errorsElement.Add(errorElement);

                        errorElement.Value = "Facade Proxies Generator: error loading " + refDll;
                        errorElement.SetAttributeValue("FileName", " ");
                        errorElement.SetAttributeValue("LineNumber", ((int)1).ToString());
                    }
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(errorFile))
                            xmlDocument.Save(errorFile);
                    }
                    catch (Exception error)
                    {

                    }
                }
            }


            var proxyInterfaces = (from _interface in assembly.DefinedTypes
                                   from attributeType in _interface.GetCustomAttributes(false)
                                   where _interface.IsInterface && _interface.IsPublic && attributeType.GetType().FullName == typeof(OOAdvantech.MetaDataRepository.GenerateFacadeProxy).FullName
                                   select _interface).ToList();

            var consumeEventproxies = (from _interface in assembly.GetTypes()
                                       from eventInfo in _interface.GetEvents()
                                       from attributeType in eventInfo.GetCustomAttributes(false)
                                       where _interface.IsInterface && attributeType.GetType().FullName == typeof(OOAdvantech.MetaDataRepository.GenerateEventConsumerProxy).FullName
                                       select eventInfo).ToList();

            //  from eventInfo in _interface.GetEvents()

            //from attributeType in eventInfo.GetCustomAttributes(false)
            //where _interface.IsInterface //&& _interface.IsPublic //&& attributeType.GetType().FullName == typeof(OOAdvantech.MetaDataRepository.GenerateEventConsumerProxy).FullName
            //select new { eventInfo, attributeType, _interface }).Distinct().ToList();


            foreach (var interaface in proxyInterfaces) // assembly.GetTypes().Where(x=>x.IsInterface&& x.IsPublic && x.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.GenerateFacadeProxy), false).Length > 0))
            {
                if (targetUnit == null)
                    targetUnit = new CodeCompileUnit();
                System.Type type = interaface;
                if (!assemblies.Contains(type.Assembly))
                    assemblies.Add(type.Assembly);

                var targetClass = CodeClass.CreateCodeType(type, targetUnit);
                CodeClass.AddFields(targetClass);

                var proxyField = new CodeMemberField();
                proxyField.Attributes = MemberAttributes.Public;
                proxyField.Name = "Org";
                proxyField.Type = new CodeTypeReference(type);
                targetClass.Members.Add(proxyField);

                CodeClass.AddProxyConstructor(targetClass);
                

                CodeClass.AddBaseType(targetClass, typeof(OOAdvantech.Remoting.MarshalByRefObject));
                CodeClass.AddBaseType(targetClass, interaface);
                CodeClass.AddBaseType(targetClass, typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy));

                foreach (var methodInfo in typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy).GetMethods())
                    CodeClass.AddMethod(methodInfo, targetClass, assemblies);

                foreach (var eventInfo in typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy).GetEvents())
                    CodeClass.AddEvent(eventInfo, targetClass, assemblies);

                foreach (var methodInfo in type.GetMethods())
                    CodeClass.AddMethod(methodInfo, targetClass, assemblies);

                foreach (var propertyInfo in type.GetProperties())
                    CodeClass.AddProperty(propertyInfo, targetClass, assemblies);

                foreach (var eventInfo in type.GetEvents())
                    CodeClass.AddEvent(eventInfo, targetClass, assemblies);





                foreach (var typeOfHierarchy in type.GetInterfaces())
                {

                    foreach (var methodInfo in typeOfHierarchy.GetMethods())
                        CodeClass.AddMethod(methodInfo, targetClass, assemblies);

                    foreach (var propertyInfo in typeOfHierarchy.GetProperties())
                        CodeClass.AddProperty(propertyInfo, targetClass, assemblies);

                    foreach (var eventInfo in typeOfHierarchy.GetEvents())
                        CodeClass.AddEvent(eventInfo, targetClass, assemblies);

                }



            }

            foreach (var _event in consumeEventproxies)
            {
                if (targetUnit == null)
                    targetUnit = new CodeCompileUnit();
                System.Type type = _event.DeclaringType;

                if (!assemblies.Contains(type.Assembly))
                    assemblies.Add(type.Assembly);

                var targetClass = CodeClass.CreateCodeType(_event, targetUnit);
                CodeClass.AddBaseType(targetClass, typeof(OOAdvantech.Remoting.EventConsumerHandler));


                CodeClass.AddConsumeEventproxyConstructor(targetClass, _event, assemblies);


            }
            if (targetUnit == null)
                return;

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            //targetUnit.Namespaces.Add(new CodeNamespace(assembly.Assembly.GetName().Name + ".Proxies"));
            CompilerParameters compparams = new CompilerParameters(new string[] { "mscorlib.dll" });

            foreach (var theAssembly in (from refAssembly in assemblies
                                         where refAssembly.GetName().Name.ToLower() == "mscorlib" ||
                                         refAssembly.GetName().Name.ToLower() == "system"
                                         select refAssembly).ToList())
            {
                assemblies.Remove(theAssembly);
            }

            compparams.ReferencedAssemblies.Add("System.dll");

            if (!assemblies.Contains(typeof(OOAdvantech.Remoting.RestApi.Proxy).Assembly))
                assemblies.Add(typeof(OOAdvantech.Remoting.RestApi.Proxy).Assembly);

            foreach (var refAssembly in assemblies)
                compparams.ReferencedAssemblies.Add(refAssembly.Location);

            compparams.GenerateExecutable = false;
            compparams.OutputAssembly = output + string.Format(@"\RM_{0}.dll", assembly.GetName().Name);
            Microsoft.CSharp.CSharpCodeProvider csharp = new Microsoft.CSharp.CSharpCodeProvider();
            //ICodeCompiler cscompiler = csharp.CreateCompiler();
            if (proxiesOutput == ProxiesOutput.Assembly)
            {
                CompilerResults compresult = csharp.CompileAssemblyFromDom(compparams, targetUnit);
            }
            else
            {
                string csharpOutput = output + string.Format(@"\RM_{0}.cs", assembly.GetName().Name);
                CodeClass.GenerateCSharpCode(csharpOutput, targetUnit);

            }
            //provider.CompileAssemblyFromDom()
            //CodeClass.GenerateCSharpCode(@"C:\Sera.cs",targetUnit);
        }


    }
}

namespace OOAdvantech.Remoting
{
    public class MarshalByRefObject : System.MarshalByRefObject, IExtMarshalByRefObject
    {

    }
}