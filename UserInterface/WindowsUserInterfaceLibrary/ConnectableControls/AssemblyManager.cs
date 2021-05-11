using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.MetaDataRepository;

namespace ConnectableControls
{
    /// <MetaDataID>{7C756131-AFC7-416C-AAC7-9FBD9778A369}</MetaDataID>
    class AssemblyManager
    {
        static object VSStudio;
        static System.Type IDEManagerType;
        static System.Type MetaDataRepositoryAssemblyType;
        static AssemblyManager()
        {
            //ModulePublisher.ClassRepository.GetType(
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("BuildControllAddin, Version=1.0.2.0, Culture=neutral, PublicKeyToken=b768d6b172456496");
                VSStudio = assembly.GetType("BuildControllAddin.Connect").GetField("DTEObject").GetValue(null)  ;

            }
            catch(System.Exception error)
            {
            }
#if Net4

            //System.Reflection.Assembly codeMetaDataRepositoryAssembly = System.Reflection.Assembly.Load("CodeMetaDataRepository, PublicKeyToken=9ce9f0a461f2c1a5");
            System.Reflection.Assembly codeMetaDataRepositoryAssembly = ModulePublisher.ClassRepository.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager", "CodeMetaDataRepository, PublicKeyToken=9ce9f0a461f2c1a5").Assembly;
#else
            System.Reflection.Assembly codeMetaDataRepositoryAssembly= System.Reflection.Assembly.Load("CodeMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=a849addb868b59ea");
#endif

            IDEManagerType =codeMetaDataRepositoryAssembly.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager");
            IDEManagerType.GetMethod("Initialize").Invoke(null, new object[0]);
            MetaDataRepositoryAssemblyType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "");
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
      
            
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           
        }
        public static OOAdvantech.MetaDataRepository.Component GetActiveWindowProject()
        {
            try
            {
                return IDEManagerType.GetMethod("GetActiveWindowProject").Invoke(null, new object[0]) as OOAdvantech.MetaDataRepository.Component;

                //return OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(VSStudio.ActiveDocument.ProjectItem.ContainingProject) as OOAdvantech.CodeMetaDataRepository.Project;
            }
            catch(System.Exception error)
            {
            }
            return null;
        }

      


    
       // static System.Collections.Generic.Dictionary<string, OOAdvantech.CodeMetaDataRepository.Project> SolutionProjects = new Dictionary<string, OOAdvantech.CodeMetaDataRepository.Project>();
        static void SolutionOpened()
        {
            //try
            //{
            //    SolutionProjects.Clear();
            //    foreach (EnvDTE.Project vsProject in VSStudio.Solution.Projects)
            //    {
            //        if (vsProject.Object == null)
            //            continue;
            //        OOAdvantech.CodeMetaDataRepository.Project project = OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(vsProject) as OOAdvantech.CodeMetaDataRepository.Project;
            //        if (project == null)
            //            project = new OOAdvantech.CodeMetaDataRepository.Project(vsProject);

                    
            //        foreach (OOAdvantech.MetaDataRepository.MetaObject metObejct in project.Residents)
            //        {
            //            long count = 0;
            //            if (metObejct is Class)
            //                count = (metObejct as Class).Features.Count;

            //        }
            //        long counta = project.ClientDependencies.Count;


            //    }
            //}
            //catch (Exception error)
            //{

                
            //}
    
        }
        static bool _InVisualStudio = false;
        static internal bool InVisualStudio
        {
            get
            {
                if (VSStudio != null)
                    return true;
                return _InVisualStudio;

            }
            set
            {
                //_InVisualStudio = value;
            }
        }
     

        static OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName,bool caseSensitive)
        {
            if(InVisualStudio)
                return IDEManagerType.GetMethod("GetClassifier").Invoke(null, new object[2] { fullName, caseSensitive }) as OOAdvantech.MetaDataRepository.Classifier;
            else
                return MetaDataRepositoryAssemblyType.GetMethod("FindClassifier").Invoke(null, new object[2] { fullName, caseSensitive, }) as OOAdvantech.MetaDataRepository.Classifier;

        }

        public static bool ErrorCheck(OOAdvantech.MetaDataRepository.Component Component, ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
#if DEBUG
         //   System.Windows.Forms.MessageBox.Show("Erta "+Component.Identity.ToString());
#endif
            foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in Component.Residents)
            {
                if (metaObject is OOAdvantech.MetaDataRepository.Class)
                {
                    try
                    {
                        if ((metaObject as OOAdvantech.MetaDataRepository.Class).OwnedTemplateSignature != null)
                            continue;
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (metaObject as OOAdvantech.MetaDataRepository.Class).GetAttributes(true))
                        {
                            if (attribute.Type.FullName == typeof(ConnectableControls.ViewControlObject).FullName||
                                attribute.Type.FullName == typeof(ConnectableControls.FormConnectionControl).FullName)
                            {
                                System.Reflection.FieldInfo fieldInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                if (fieldInfo != null)
                                {
                                    Type type = metaObject.GetExtensionMetaObject(typeof(Type)) as Type;
                                    if (type.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.IgnoreErrorCheckAttribute), true).Length > 0)
                                        continue;

                                    System.Reflection.ConstructorInfo ctor = type.GetConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly,null,  new Type[0],null);
                                    if (ctor == null)
                                    {
                                        //"An Error check for class can not be made because a default constructor with InitializeComponent() is missing 
                                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: An Error check for class '" + metaObject.FullName + "' can not be made because a default constructor with InitializeComponent() is missing.", metaObject.FullName));
                                        continue;
                                    }
                                    

                                    object formObject = ctor.Invoke(new object[0]);
                                    ConnectableControls.ViewControlObject viewControlObject = fieldInfo.GetValue(formObject) as ConnectableControls.ViewControlObject;
                                    if (viewControlObject != null)
                                    {
                                        viewControlObject.UserInterfaceObjectConnection.ErrorCheck(ref errors);
                                    }
                                    else
                                    {
                                        OOAdvantech.MetaDataRepository.Operation operation= (metaObject as OOAdvantech.MetaDataRepository.Class).GetOperation("InitializeComponent",new string[0],false);
                                        System.Reflection.MethodInfo method = operation.GetExtensionMetaObject<System.Reflection.MethodInfo>();
                                        method.Invoke(formObject, new object[0]);
                                        viewControlObject = fieldInfo.GetValue(formObject) as ConnectableControls.ViewControlObject;
                                        if (viewControlObject != null)
                                            viewControlObject.UserInterfaceObjectConnection.ErrorCheck(ref errors);
                                    }

                                }

                            }

                        }
                    }
                    catch (System.Exception error)
                    {
                        if (error is System.Reflection.TargetInvocationException && error.InnerException!=null)
                            error = error.InnerException; 

                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError(error.Message+"  "+error.StackTrace,metaObject.FullName));
                    }

                }
                
            }
           // System.Windows.Forms.MessageBox.Show("Erta  End");
            //errors = new System.Collections.ArrayList();
            return errors.Count>0;
        }



        internal static void BrowseCode(OOAdvantech.MetaDataRepository.MetaObject codeElement)
        {
            if (codeElement is OOAdvantech.MetaDataRepository.Feature)
            {
                //OOAdvantech.MetaDataRepository.Feature feature = codeElement as OOAdvantech.MetaDataRepository.Feature;
                //if (feature.Owner != null && feature.Owner.ImplementationUnit != null)
                IDEManagerType.GetMethod("BrowseOnMetaObjectCode").Invoke(null, new object[1] {codeElement});
            }
            
        }
    }
}
