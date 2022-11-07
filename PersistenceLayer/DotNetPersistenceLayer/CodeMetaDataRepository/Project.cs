//using OOAdvantech.MetaDataRepository;
using System.Reflection;
using System.Linq;
using OOAdvantech.Transactions;
namespace OOAdvantech.CodeMetaDataRepository
{
    delegate void CodeElementChangedHandler(EnvDTE.CodeElement element, EnvDTE80.vsCMChangeKind change);
    /// <MetaDataID>{DA5401D5-0D54-47A0-9396-16E8244D4A14}</MetaDataID>
    public class Project : OOAdvantech.MetaDataRepository.Component
    {
        public override string AssemblyString
        {
            get
            {
                //foreach (EnvDTE.Property obj in VSProject.Properties)
                //{
                //    try
                //    {
                //        System.Diagnostics.Debug.WriteLine(obj.Name+":  "+obj.Value.ToString());
                //    }
                //    catch
                //    {
                //        //6972779043
                //        //2109245953
                //    }
                //}
                return base.AssemblyString;
            }
            set
            {
                base.AssemblyString = value;
            }
        }
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<RDBMSMappingContext> _RDBMSMappingContexts;
        public System.Collections.Generic.List<RDBMSMappingContext> RDBMSMappingContexts
        {
            get
            {
                if (_RDBMSMappingContexts == null)
                {


                    EnvDTE.ProjectItem folderProjectItem = null;
                    foreach (EnvDTE.ProjectItem existingProjectItem in VSProject.ProjectItems)
                    {
                        if (existingProjectItem.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}" && existingProjectItem.Name == "Mapping Contexts")
                        {
                            folderProjectItem = existingProjectItem;
                            break;
                        }
                    }
                    if (folderProjectItem != null)
                    {
                        _RDBMSMappingContexts = new System.Collections.Generic.List<RDBMSMappingContext>();
                        foreach (EnvDTE.ProjectItem projectItem in folderProjectItem.ProjectItems)
                        {
                            if (projectItem.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
                                _RDBMSMappingContexts.Add(new RDBMSMappingContext(projectItem));
                        }
                    }
                    else
                        return new System.Collections.Generic.List<RDBMSMappingContext>();

                }

                return _RDBMSMappingContexts;
            }
        }
        public event ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{2e3227aa-bd4e-4762-b030-569eebdfa965}</MetaDataID>
        public RDBMSMappingContext NewRDBMSMappingContext()
        {

            try
            {

                EnvDTE.ProjectItem folderProjectItem = null;
                foreach (EnvDTE.ProjectItem existingProjectItem in VSProject.ProjectItems)
                {
                    if (existingProjectItem.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}" && existingProjectItem.Name == "Mapping Contexts")
                    {
                        folderProjectItem = existingProjectItem;
                        break;
                    }
                }
                if (folderProjectItem == null)
                {
                    if (System.IO.Directory.Exists(new System.IO.FileInfo(VSProject.FileName).DirectoryName + @"\Mapping Contexts"))
                        folderProjectItem = VSProject.ProjectItems.AddFromDirectory(new System.IO.FileInfo(VSProject.FileName).DirectoryName + @"\Mapping Contexts");
                    else
                        folderProjectItem = VSProject.ProjectItems.AddFolder("Mapping Contexts", "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}");
                }
                VSProject.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Activate();
                (VSProject.DTE.ActiveWindow.Object as EnvDTE.UIHierarchy).GetItem(new System.IO.FileInfo(VSProject.DTE.Solution.FileName).Name.Substring(0, new System.IO.FileInfo(VSProject.DTE.Solution.FileName).Name.Length - new System.IO.FileInfo(VSProject.DTE.Solution.FileName).Extension.Length) + @"\" + VSProject.Name + @"\" + folderProjectItem.Name).Select(EnvDTE.vsUISelectionType.vsUISelectionTypeSelect);


                EnvDTE.ProjectItem projectItem = VSProject.DTE.ItemOperations.AddNewItem(@"Visual C# Items\Data\XML File", "Test.xml");
                projectItem.Document.ActiveWindow.Close(EnvDTE.vsSaveChanges.vsSaveChangesNo);
                projectItem.Properties.Item("BuildAction").Value = (int)VSLangProj.prjBuildAction.prjBuildActionEmbeddedResource;
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("RDBMSMapingStorage", document, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                document.Save(projectItem.get_FileNames(1));

                RDBMSMappingContext rdbmsMappingContext = new RDBMSMappingContext(projectItem);
                if (_RDBMSMappingContexts == null)
                    _RDBMSMappingContexts = new System.Collections.Generic.List<RDBMSMappingContext>();
                _RDBMSMappingContexts.Add(rdbmsMappingContext);

                OOAdvantech.EventUnderProtection.Invoke<ObjectChangeStateHandle>(ref ObjectChangeState, EventUnderProtection.ExceptionHandling.RemoveDisconnectedEventHandlers | EventUnderProtection.ExceptionHandling.IgnoreExceptions, this, "RDBMSMappingContexts");

            }
            catch (System.Exception error)
            {


            }
            return null;
        }

        /// <MetaDataID>{955a24f3-84ab-4831-8233-e66a0056e09a}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Class CreateClass(string className, string namespaceName)
        {


            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("CreateClass", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[2] { typeof(string), typeof(string) }, null);
                return IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[2] { className, namespaceName }) as MetaDataRepository.Class;
            }

            Class _class = null;
            if (!string.IsNullOrEmpty(namespaceName))
            {
                EnvDTE.CodeNamespace _namespace = VSProject.CodeModel.AddNamespace(namespaceName, className + ".cs", 0);
                System.Threading.Thread.Sleep(200);
                EnvDTE.CodeClass codeClass = _namespace.AddClass(className, 0, null, null, EnvDTE.vsCMAccess.vsCMAccessPublic);
                System.Threading.Thread.Sleep(200);
                _class = new Class(codeClass);
            }
            else
                _class = new Class(VSProject.CodeModel.AddClass(className, className + ".cs", 0, null, null, EnvDTE.vsCMAccess.vsCMAccessPublic));
            _Residents.Add(_class);
            return _class;


        }
        /// <MetaDataID>{e89c0074-c2fc-4509-8154-3b11c37202e7}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Interface CreateInterface(string interfaceName, string namespaceName)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("CreateInterface", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[2] { typeof(string), typeof(string) }, null);
                return IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[2] { interfaceName, namespaceName }) as MetaDataRepository.Interface;
            }
            Interface _interface = null;
            if (!string.IsNullOrEmpty(namespaceName))
            {
                EnvDTE.CodeNamespace _namespace = VSProject.CodeModel.AddNamespace(namespaceName, interfaceName + ".cs", 0);
                System.Threading.Thread.Sleep(200);
                EnvDTE.CodeInterface codeInterface = _namespace.AddInterface(interfaceName, 0, null, EnvDTE.vsCMAccess.vsCMAccessPublic);
                System.Threading.Thread.Sleep(200);
                _interface = new Interface(codeInterface);
            }
            else
                _interface = new Interface(VSProject.CodeModel.AddInterface(interfaceName, interfaceName + ".cs", 0, null, EnvDTE.vsCMAccess.vsCMAccessPublic));
            _Residents.Add(_interface);
            return _interface;

        }
        /// <MetaDataID>{eb6f0ade-6f39-49ff-a74d-443420ed8445}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Structure CreateStructure(string structureName, string namespaceName)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("CreateStructure", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[2] { typeof(string), typeof(string) }, null);
                return IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[2] { structureName, namespaceName }) as MetaDataRepository.Structure;
            }

            Structure _struct = null;
            if (!string.IsNullOrEmpty(namespaceName))
            {
                EnvDTE.CodeNamespace _namespace = VSProject.CodeModel.AddNamespace(namespaceName, structureName + ".cs", 0);
                System.Threading.Thread.Sleep(200);
                EnvDTE.CodeStruct codeStruct = _namespace.AddStruct(structureName, 0, null, null, EnvDTE.vsCMAccess.vsCMAccessPublic);
                System.Threading.Thread.Sleep(200);
                _struct = new Structure(codeStruct);
            }
            else
                _struct = new Structure(VSProject.CodeModel.AddStruct(structureName, structureName + ".cs", 0, null, null, EnvDTE.vsCMAccess.vsCMAccessPublic));
            _Residents.Add(_struct);
            return _struct;

        }
        /// <MetaDataID>{834831ea-27f4-44fb-be7a-e2c840ce3387}</MetaDataID>
        public override void AddResident(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {

            if (metaObject == null)
                return;
            if (!_Residents.Contains(metaObject))
                _Residents.Add(metaObject);

        }

        /// <MetaDataID>{22c36bff-4aeb-422c-a642-0676f4c5ca76}</MetaDataID>
        public string FileName
        {
            get
            {
                if (VSProject == null)
                    return "";
                else
                    return VSProject.FileName;
            }
        }
        /// <MetaDataID>{6CF437CE-43C5-4C53-AD7D-A2161662C9EA}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<ProjectItem> ProjectItems = new OOAdvantech.Collections.Generic.List<ProjectItem>();

        /// <MetaDataID>{6d92b522-f595-409d-9ebe-77636e70e6d1}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }
            long count = Residents.Count;
            base.Synchronize(OriginMetaObject);
        }

        /// <MetaDataID>{735ca8a9-4aa1-46fd-8d91-df411ccdf8ef}</MetaDataID>
        void EnableDelayLoadModelTimer()
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("EnableDelayLoadModelTimer", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[0] { }, null);
                IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[0] { });
                return;
            }



            IDEManager.SynchroForm.Timer.Interval = 300;
            IDEManager.SynchroForm.Timer.Tick -= new System.EventHandler(OnTimer);
            IDEManager.SynchroForm.Timer.Tick += new System.EventHandler(OnTimer);
            IDEManager.SynchroForm.Timer.Enabled = true;

        }
        /// <MetaDataID>{5EF9E0B0-5B7D-4EB5-AD52-953A943CDBA5}</MetaDataID>
        /// <exclude>Excluded</exclude>
        public object ObjectSearch;
        /// <MetaDataID>{85C07FFB-0870-4BF7-9218-F1FF5F140653}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.MetaObject> Residents
        {

            get
            {
                if (!IsResidentsLoaded)
                {



                    //using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,TransactionOption.Suppress))
                    //{

                    object obj = MetaObjectMapper.FindMetaObjectFor(VSProject) as Project;
                    _Residents.Clear();
                    foreach (MetaDataRepository.Classifier classifier in GetClassifiers(VSProject.ProjectItems))
                        _Residents.Add(classifier);

                    IsResidentsLoaded = true;

                    MergePartialClasses();


                    //    stateTransition.Consistent = true;
                    //}
                    //foreach (MetaDataRepository.Classifier classifier in GetClassifiers(VSProject.ProjectItems))
                    //    (classifier as CodeElementContainer).RefreshCodeElement((classifier as CodeElementContainer).CodeElement);


                }
                else
                {
                    if (_Residents == null)
                        return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>();
                }
                return base.Residents;
            }
        }

        /// <MetaDataID>{c4768caf-0baa-4b12-af68-50ffedc349f3}</MetaDataID>
        private void MergePartialClasses()
        {
            foreach (MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<MetaDataRepository.MetaObject>(_Residents))
            {
                if (metaObject is Class)
                {
                    Class _class = metaObject as Class;
                    if (_class.VSClass == null)
                        continue;
                    try
                    {
                        foreach (MetaDataRepository.MetaObject aMetaObject in _Residents)
                        {
                            if (aMetaObject.FullName == _class.FullName && aMetaObject != metaObject)
                            {
                                Class _partialClass = aMetaObject as Class;
                                if ((_class.VSClass as EnvDTE80.CodeClass2).PartialClasses.Count == 0 ||
                                    (_partialClass.VSClass as EnvDTE80.CodeClass2).PartialClasses.Count == 0)
                                    EnableDelayLoadModelTimer();

                                try
                                {
                                    if (((_class.VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 1 && (_class.VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1) == (_partialClass.VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1)) ||
                                        ((_partialClass.VSClass as EnvDTE80.CodeClass2).PartialClasses.Count > 1 && (_partialClass.VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1) == (_class.VSClass as EnvDTE80.CodeClass2).PartialClasses.Item(1)))
                                    {
                                        _partialClass.CodeElementRemoved();
                                        _class.RefreshCodeElement(_class.VSClass as EnvDTE.CodeElement);
                                        _class.MergePartialClasses();

                                    }
                                }
                                catch (System.Exception error)
                                {
                                    EnableDelayLoadModelTimer();
                                }
                            }
                        }
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
        }
        /// <MetaDataID>{d11931f9-5b14-4177-8951-3ac2c8d0898f}</MetaDataID>
        bool InClientDependenciesUpdate = false;
        /// <MetaDataID>{546abfe5-2da9-4bf6-af41-20e2027f2b91}</MetaDataID>
        bool LoadClientDependencies = true;
        /// <MetaDataID>{74f202e1-2e4f-4b3a-8278-cc814626e33f}</MetaDataID>
        System.Collections.Generic.List<VSLangProj.Reference> ProjectReferences = new System.Collections.Generic.List<VSLangProj.Reference>();
        /// <MetaDataID>{E4E0A3F8-9927-4D50-8E72-EC4606740137}</MetaDataID>
        internal EnvDTE.Project VSProject;

        public ProjectLanguage Laguage
        {
            get
            {
                try
                {
                    if (VSProject.CodeModel != null)
                    {
                        if (VSProject.CodeModel.Language == EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp)
                            return ProjectLanguage.CSharp;
                        if (VSProject.CodeModel.Language == EnvDTE.CodeModelLanguageConstants.vsCMLanguageVB)
                            return ProjectLanguage.VBNet;
                        if (VSProject.CodeModel.Language == EnvDTE.CodeModelLanguageConstants.vsCMLanguageVC)
                            return ProjectLanguage.Cpp;
                        if (VSProject.CodeModel.Language == EnvDTE80.CodeModelLanguageConstants2.vsCMLanguageJSharp)
                            return ProjectLanguage.JSharp;
                    }
                    if (VSProject.Kind.ToLower() == "{F088123C-0E9E-452A-89E6-6BA2F21D5CAC}".ToLower())
                        return ProjectLanguage.UMLModel;
                }
                catch (System.Exception error)
                {
                }
                return ProjectLanguage.Unknown;
            }
        }
        /// <MetaDataID>{1BDE30E7-9BE3-4FF2-8A8D-38CB762246C2}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Dependency> ClientDependencies
        {

            get
            {
                
                if (InClientDependenciesUpdate)
                    return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>(_ClientDependencies, OOAdvantech.Collections.CollectionAccessType.ReadOnly);

                bool clientDependenciesChanged = false;
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    InClientDependenciesUpdate = true;
                    VSLangProj.VSProject vsProject = VSProject.Object as VSLangProj.VSProject;

                    System.Collections.Generic.Dictionary<string, MetaDataRepository.Dependency> removedDependency = new System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.Dependency>();
                    foreach (MetaDataRepository.Dependency dependency in _ClientDependencies)
                        removedDependency[dependency.Supplier.Identity.ToString()] = dependency;

                    foreach (VSLangProj.Reference reference in vsProject.References)
                    {
                        try
                        {
                            if (reference.SourceProject != null)
                            {
                                if (!HasClientDependency(GetProjectIdentity(reference.SourceProject)))
                                {
                                    Project project = MetaObjectMapper.FindMetaObjectFor(reference.SourceProject) as Project;

                                    if (project == null)
                                        project = new Project(reference.SourceProject);
                                    clientDependenciesChanged = true;
                                    AddDependency("", project);
                                }
                                else
                                    removedDependency.Remove(GetProjectIdentity(reference.SourceProject));
                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(reference.Path))
                                { 
                                    System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.LoadFile(reference.Path);
                                    
                                    if (!HasClientDependency(DotNetMetaDataRepository.Assembly.GetIdentity(dotNetAssembly)))
                                    {

                                        System.Type metaObjectMapperType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper", "");
                                        OOAdvantech.MetaDataRepository.Component assembly = metaObjectMapperType.GetMethod("FindMetaObjectFor").Invoke(null, new object[1] { dotNetAssembly }) as OOAdvantech.MetaDataRepository.Component;
                                        if (assembly == null)
                                        {
//#if Net4
//                                            assembly = ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c", dotNetAssembly, new System.Type[1] { typeof(System.Reflection.Assembly) }) as OOAdvantech.MetaDataRepository.Component;
//#else
                                            assembly = ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7", dotNetAssembly, new System.Type[1] { typeof(System.Reflection.Assembly) }) as OOAdvantech.MetaDataRepository.Component;
//#endif
                                        }
                                        AddDependency("", assembly);
                                        clientDependenciesChanged = true;
                                    }
                                    else
                                        removedDependency.Remove(DotNetMetaDataRepository.Assembly.GetIdentity(dotNetAssembly));
                                }
                            }
                        }
                        catch (System.Exception error)
                        {
                        }
                    }
                    foreach (MetaDataRepository.Dependency dependency in removedDependency.Values)
                    {
                        clientDependenciesChanged = true;
                        _ClientDependencies.Remove(dependency);
                    }
                    if (clientDependenciesChanged && !LoadClientDependencies)
                        MetaObjectChangeState();

                    return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>(_ClientDependencies, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                catch (System.Exception error)
                {
                    throw;

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                    InClientDependenciesUpdate = false;
                    LoadClientDependencies = false;
                }


            }
        }

        /// <MetaDataID>{d1262a00-1c55-444c-a174-e46a4690059a}</MetaDataID>
        private bool HasClientDependency(string identity)
        {
            foreach (MetaDataRepository.Dependency dependency in _ClientDependencies)
            {
                if (dependency.Supplier.Identity.ToString() == identity)
                    return true;
            }
            return false;

        }




        /// <MetaDataID>{75685eb5-9811-437b-9102-ee6c40e59d2b}</MetaDataID>
        string myId = System.Guid.NewGuid().ToString();

        /// <MetaDataID>{921b7e6e-b157-4cf3-afb6-7950ebf5f8c5}</MetaDataID>
        public static string GetProjectIdentity(EnvDTE.Project project)
        {
            string identity = null;
            System.Xml.XmlDocument projectDocument = new System.Xml.XmlDocument();
            projectDocument.Load(project.FullName);
            foreach (System.Xml.XmlNode projectNode in projectDocument.DocumentElement.ChildNodes)
            {
                if (projectNode.Name == "PropertyGroup")
                {
                    System.Xml.XmlElement projectElement = projectNode as System.Xml.XmlElement;
                    foreach (System.Xml.XmlElement propertyElement in projectElement.ChildNodes)
                    {
                        if (propertyElement.Name == "ProjectGuid")
                        {
                            identity = propertyElement.InnerText;
                            break;
                        }
                    }
                }
            }
            if (identity == null && projectDocument.DocumentElement.HasAttribute("ProjectGUID"))
                identity = projectDocument.DocumentElement.GetAttribute("ProjectGUID");

            if (identity == null)
            {
                projectDocument.DocumentElement.SetAttribute("ProjectGUID",System.Guid.NewGuid().ToString());
                projectDocument.Save(project.FullName);
                identity = projectDocument.DocumentElement.GetAttribute("ProjectGUID");

                return identity;
            }
            return identity;



        }
        /// <MetaDataID>{ed64be10-ad2e-419b-a08a-639a6873a574}</MetaDataID>
        static string GetProgectIdentity(EnvDTE.Project project)
        {
            string identity = null;
            System.Xml.XmlDocument projectDocument = new System.Xml.XmlDocument();
            string assemblyName = null;
            System.Collections.Generic.List<string> outputPaths = new System.Collections.Generic.List<string>();

            foreach (System.Xml.XmlNode projectNode in projectDocument.DocumentElement.ChildNodes)
            {
                if (projectNode.Name == "PropertyGroup")
                {
                    System.Xml.XmlElement projectElement = projectNode as System.Xml.XmlElement;
                    foreach (System.Xml.XmlElement propertyElement in projectElement.ChildNodes)
                    {
                        if (propertyElement.Name == "ProjectGuid")
                        {
                            identity = propertyElement.InnerText;
                            //break;
                        }
                    }
                }
            }
            if (identity == null && projectDocument.DocumentElement.HasAttribute("ProjectGUID"))
                identity = projectDocument.DocumentElement.GetAttribute("ProjectGUID");
            if (identity == null)
                identity = project.Name;
            return identity;
        }
        /// <MetaDataID>{F8F886E3-271C-4ABB-9EDB-8CBA24D453C4}</MetaDataID>
        public Project(EnvDTE.Project project)
        {

            VSProject = project;
            _Name = VSProject.Name;
            try
            {
                _Name = VSProject.Properties.Item("AssemblyName").Value.ToString();
            }
            catch (System.Exception error)
            {
            }
            MetaObjectMapper.AddTypeMap(VSProject, this);
            string identity = null;
            System.Xml.XmlDocument projectDocument = new System.Xml.XmlDocument();
            projectDocument.Load(VSProject.FullName);
            string assemblyName = null;
            System.Collections.Generic.List<string> outputPaths = new System.Collections.Generic.List<string>();

            foreach (System.Xml.XmlNode projectNode in projectDocument.DocumentElement.ChildNodes)
            {
                if (projectNode.Name == "PropertyGroup")
                {
                    System.Xml.XmlElement projectElement = projectNode as System.Xml.XmlElement;
                    foreach (System.Xml.XmlElement propertyElement in projectElement.ChildNodes)
                    {
                        if (propertyElement.Name == "ProjectGuid")
                        {
                            identity = propertyElement.InnerText;
                            //break;
                        }
                        if (propertyElement.Name == "AssemblyName")
                            assemblyName = propertyElement.InnerText;

                        if (propertyElement.Name == "OutputPath")
                            outputPaths.Add(propertyElement.InnerText);
                    }
                }
            }
            if (identity == null && projectDocument.DocumentElement.HasAttribute("ProjectGUID"))
                identity = projectDocument.DocumentElement.GetAttribute("ProjectGUID");


            if (identity == null)
                identity = project.Name;

            string assemblyFullName = null;
            System.DateTime lastWriteTime = System.DateTime.MinValue;
            System.IO.Directory.SetCurrentDirectory(new System.IO.FileInfo(VSProject.FullName).Directory.FullName);
            foreach (string outputPath in outputPaths)
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(outputPath);
                if (directoryInfo.Exists &&
                    System.IO.File.Exists(directoryInfo.FullName + @"\" + assemblyName + ".dll"))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(directoryInfo.FullName + @"\" + assemblyName + ".dll");
                    if (fileInfo.LastWriteTime > lastWriteTime)
                    {
                        try
                        {
                            assemblyFullName = System.Reflection.AssemblyName.GetAssemblyName(fileInfo.FullName).FullName;
                            lastWriteTime = fileInfo.LastWriteTime;
                        }
                        catch (System.Exception error)
                        {
                        }
                    }
                }
                if (directoryInfo.Exists &&
                   System.IO.File.Exists(directoryInfo.FullName + @"\" + assemblyName + ".exe"))
                {

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(directoryInfo.FullName + @"\" + assemblyName + ".exe");
                    if (fileInfo.LastWriteTime > lastWriteTime)
                    {
                        try
                        {
                            assemblyFullName = System.Reflection.AssemblyName.GetAssemblyName(fileInfo.FullName).FullName;
                            lastWriteTime = fileInfo.LastWriteTime;
                        }
                        catch (System.Exception error)
                        {
                        }
                    }



                }
            }
            //if (!string.IsNullOrEmpty(assemblyFullName))
            //    base.PutPropertyValue(".Net", "AssemblyFullName", assemblyFullName);


            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identity);
            MetaDataRepository.MetaObject metaObject = MetaObjectMapper.FindMetaObject(_Identity);
            if (metaObject != null&&metaObject!=this)
                MetaObjectMapper.RemoveMetaObject(metaObject);



            MetaObjectMapper.AddMetaObject(this, project.Name);




            VisualStudioEventBridge.VisualStudioEvents.CodeElementAdded += new VisualStudioEventBridge.CodeElementAddedEventHandler(CodeElementAdded);
            VisualStudioEventBridge.VisualStudioEvents.CodeElementChanged += new VisualStudioEventBridge.CodeElementChangedEventHandler(CodeElementChanged);
            VisualStudioEventBridge.VisualStudioEvents.CodeElementDeleted += new VisualStudioEventBridge.CodeElementDeletedEventHandler(CodeElementDeleted);

            VisualStudioEventBridge.VisualStudioEvents.LineChanged += new VisualStudioEventBridge.LineChangedEventHandler(LineChanged);
            VisualStudioEventBridge.VisualStudioEvents.ProjectRemoved += new VisualStudioEventBridge.ProjectRemovedEventHandler(ProjectRemoved);
            VisualStudioEventBridge.VisualStudioEvents.SolutionClosed += new VisualStudioEventBridge.SolutionClosedEventHandler(SolutionClosed);
            VisualStudioEventBridge.VisualStudioEvents.ProjectItemAdded += new VisualStudioEventBridge.ProjectItemAddedEventHandler(ProjectItemAdded);
            VisualStudioEventBridge.VisualStudioEvents.ProjectItemRemoved += new VisualStudioEventBridge.ProjectItemRemovedEventHandler(ProjectItemRemoved);
            VSProject.DTE.Events.SolutionEvents.BeforeClosing += new EnvDTE._dispSolutionEvents_BeforeClosingEventHandler(OnBeforeSolutionClosing);

            (VSProject.DTE.Events as EnvDTE80.Events2).get_WindowVisibilityEvents(null).WindowShowing += new EnvDTE80._dispWindowVisibilityEvents_WindowShowingEventHandler(Project_WindowShowing);


            //VSProject.DTE.Events.SolutionEvents.


            //VisualStudioEventBridge.VisualStudioEvents.BeforeKeyPress += new VisualStudioEventBridge.BeforeKeyPressEventHandler(BeforeKeyPress);



        }

        /// <MetaDataID>{ad981023-750a-4f95-b94f-6d6c97c49f3c}</MetaDataID>
        void Project_WindowShowing(EnvDTE.Window Window)
        {

        }






        //void AfterKeyPress(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion)
        //{
        //    try
        //    {
        //        EnvDTE.CodeElement codeElement = Selection.TopPoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementClass);
        //        if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementClass && codeClasses.ContainsKey(codeElement))
        //        {
        //            codeClasses[codeElement] = System.DateTime.Now;
        //        }
        //    }
        //    catch (System.Exception err)
        //    {


        //    }
        //}
        // System.DateTime DelayLoadModelStartTime;

        /// <MetaDataID>{b6544dcb-0ae8-4bbc-856e-d24c0bcdd0f8}</MetaDataID>
        void OnTimer(object sender, System.EventArgs e)
        {
            IDEManager.SynchroForm.Timer.Enabled = false;
            IDEManager.SynchroForm.Timer.Tick -= new System.EventHandler(OnTimer);
            try
            {
                foreach (EnvDTE.ProjectItem vsProjectItem in new System.Collections.Generic.List<EnvDTE.ProjectItem>(pendingAddedProjectItems))
                {

                    try
                    {
                        if (vsProjectItem.FileCodeModel != null)
                        {
                            foreach (MetaDataRepository.Classifier classifier in GetClassifiers(vsProjectItem))
                                _Residents.Add(classifier);
                            pendingAddedProjectItems.Remove(vsProjectItem);
                        }
                        else
                            EnableDelayLoadModelTimer();
                    }
                    catch (System.Exception error)
                    {
                    }
                }

                MergePartialClasses();

                foreach (EnvDTE.CodeElement element in new System.Collections.Generic.List<EnvDTE.CodeElement>  (codeClasses.Keys))
                {
                    if ((System.DateTime.Now - codeClasses[element].LastCahengeDateTime).TotalMilliseconds > codeClasses[element].TimeoutInMiliseconds)
                    {


                        CodeElementChanged(element, EnvDTE80.vsCMChangeKind.vsCMChangeKindUnknown);
                        if (codeClasses.Count > 0)
                            IDEManager.SynchroForm.Timer.Tick += new System.EventHandler(OnTimer);
                        break;

                    }
                    else
                    {
                        try
                        {
                            CodeClassCursorPos codeClassCursorPos = codeClasses[element];
                            if (codeClassCursorPos.LineOffsetChar != element.StartPoint.Parent.Selection.TopPoint.LineCharOffset ||
                                codeClassCursorPos.Line != element.StartPoint.Parent.Selection.TopPoint.Line)
                            {
                                codeClassCursorPos.LineOffsetChar = element.StartPoint.Parent.Selection.TopPoint.LineCharOffset;
                                codeClassCursorPos.Line = element.StartPoint.Parent.Selection.TopPoint.Line;
                                codeClassCursorPos.LastCahengeDateTime = System.DateTime.Now;
                                
                                codeClassCursorPos.TimeoutInMiliseconds = 2500;
                                try
                                {
                                    codeClassCursorPos.TimeoutInMiliseconds = (element as EnvDTE.CodeClass).Members.Count * 25;
                                }
                                catch (System.Exception error)
                                {
                                }
                                codeClasses[element] = codeClassCursorPos;
                            }
                        }
                        catch (System.Exception error)
                        {


                        }



                        IDEManager.SynchroForm.Timer.Tick += new System.EventHandler(OnTimer);
                    }

                }

            }
            finally
            {
                IDEManager.SynchroForm.Timer.Enabled = true;
            }


        }


        /// <MetaDataID>{fe60d9c1-f0ed-4eab-89b2-21fa65c4c9d4}</MetaDataID>
        void OnBeforeSolutionClosing()
        {
            return;
            try
            {
                System.DateTime start = System.DateTime.Now;
                MetaObjectMapper.Clear();
                VisualStudioEventBridge.VisualStudioEvents.CodeElementAdded -= new VisualStudioEventBridge.CodeElementAddedEventHandler(CodeElementAdded);
                VisualStudioEventBridge.VisualStudioEvents.CodeElementChanged -= new VisualStudioEventBridge.CodeElementChangedEventHandler(CodeElementChanged);
                VisualStudioEventBridge.VisualStudioEvents.CodeElementDeleted -= new VisualStudioEventBridge.CodeElementDeletedEventHandler(CodeElementDeleted);
                VisualStudioEventBridge.VisualStudioEvents.LineChanged -= new VisualStudioEventBridge.LineChangedEventHandler(LineChanged);
                VisualStudioEventBridge.VisualStudioEvents.ProjectRemoved -= new VisualStudioEventBridge.ProjectRemovedEventHandler(ProjectRemoved);
                VisualStudioEventBridge.VisualStudioEvents.SolutionClosed -= new VisualStudioEventBridge.SolutionClosedEventHandler(SolutionClosed);
                VisualStudioEventBridge.VisualStudioEvents.ProjectItemAdded -= new VisualStudioEventBridge.ProjectItemAddedEventHandler(ProjectItemAdded);
                VisualStudioEventBridge.VisualStudioEvents.ProjectItemRemoved -= new VisualStudioEventBridge.ProjectItemRemovedEventHandler(ProjectItemRemoved);
                VSProject.DTE.Events.SolutionEvents.BeforeClosing -= new EnvDTE._dispSolutionEvents_BeforeClosingEventHandler(OnBeforeSolutionClosing);
                //VisualStudioEventBridge.VisualStudioEvents.AfterKeyPress -= new VisualStudioEventBridge.AfterKeyPressEventHandler(AfterKeyPress);
                //VisualStudioEventBridge.VisualStudioEvents.BeforeKeyPress -= new VisualStudioEventBridge.BeforeKeyPressEventHandler(BeforeKeyPress);

                

                foreach (ProjectItem projectItem in ProjectItems)
                    projectItem.ProjectItemRemoved();
                ProjectItems.Clear();
                System.TimeSpan timeSpan = System.DateTime.Now - start;
                System.Diagnostics.Debug.WriteLine(Name + "  " + timeSpan.ToString());

            }
            catch (System.Exception error)
            {

            }

        }



        ////void BeforeKeyPress(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion, ref bool CancelKeypress)
        //{
        //    if (Selection.Parent.Parent.ProjectItem.ContainingProject == VSProject)
        //    {



        //    }

        //}

        /// <MetaDataID>{492af6d5-7dc9-4fc4-ac35-a2eed4ef0feb}</MetaDataID>
        void ProjectItemRemoved(EnvDTE.ProjectItem vsProjectItem)
        {
            if (pendingAddedProjectItems.Contains(vsProjectItem))
                pendingAddedProjectItems.Remove(vsProjectItem);

            if (vsProjectItem.ContainingProject == VSProject && _Residents != null)
            {
                int count = this.Residents.Count;
                ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(vsProjectItem) as ProjectItem;
                if (projectItem != null)
                {
                    foreach (MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
                    {
                        if (metaObject is CodeElementContainer)
                            (metaObject as CodeElementContainer).CodeElementRemoved(vsProjectItem);

                        //if (metaObject.Namespace != null)
                        //    metaObject.Namespace.RemoveOwnedElement(metaObject);

                        //if (metaObject is MetaDataRepository.Classifier && _Residents.Contains(metaObject))
                        //    _Residents.Remove(metaObject);
                    }
                    projectItem.ProjectItemRemoved();
                    ProjectItems.Remove(projectItem);
                }
                //MetaObjectChangeState();
            }

        }

        /// <MetaDataID>{23adb8b9-64c4-4fe7-9e0d-bdd84bff98c8}</MetaDataID>
        System.Collections.Generic.List<EnvDTE.ProjectItem> pendingAddedProjectItems = new System.Collections.Generic.List<EnvDTE.ProjectItem>();

        /// <MetaDataID>{e1b5513d-6c39-4a72-9c8f-58111b7cbbff}</MetaDataID>
        void ProjectItemAdded(EnvDTE.ProjectItem vsProjectItem)
        {
            if (vsProjectItem.FileCodeModel == null)
                return;
            if (vsProjectItem.ContainingProject == VSProject)
            {
                int count = this.Residents.Count;
                ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(vsProjectItem) as ProjectItem;
                if (projectItem == null)
                {
                    if (vsProjectItem.FileCodeModel == null)
                    {
                        pendingAddedProjectItems.Add(vsProjectItem);
                        EnableDelayLoadModelTimer();
                    }
                    else
                    {


                        foreach (MetaDataRepository.Classifier classifier in GetClassifiers(vsProjectItem))
                            _Residents.Add(classifier);
                    }

                }
                // MetaObjectChangeState();
            }
        }

        /// <MetaDataID>{93dec7e4-7e85-4bc4-a223-2f7d767dc862}</MetaDataID>
        void SolutionClosed()
        {
            try
            {
                IDEManager.SynchroForm.Timer.Tick -= new System.EventHandler(OnTimer);
                System.DateTime start = System.DateTime.Now;
                MetaObjectMapper.Clear();
                VisualStudioEventBridge.VisualStudioEvents.CodeElementAdded -= new VisualStudioEventBridge.CodeElementAddedEventHandler(CodeElementAdded);
                VisualStudioEventBridge.VisualStudioEvents.CodeElementChanged -= new VisualStudioEventBridge.CodeElementChangedEventHandler(CodeElementChanged);
                VisualStudioEventBridge.VisualStudioEvents.CodeElementDeleted -= new VisualStudioEventBridge.CodeElementDeletedEventHandler(CodeElementDeleted);
                VisualStudioEventBridge.VisualStudioEvents.LineChanged -= new VisualStudioEventBridge.LineChangedEventHandler(LineChanged);
                VisualStudioEventBridge.VisualStudioEvents.ProjectRemoved -= new VisualStudioEventBridge.ProjectRemovedEventHandler(ProjectRemoved);
                VisualStudioEventBridge.VisualStudioEvents.SolutionClosed -= new VisualStudioEventBridge.SolutionClosedEventHandler(SolutionClosed);
                VisualStudioEventBridge.VisualStudioEvents.ProjectItemAdded -= new VisualStudioEventBridge.ProjectItemAddedEventHandler(ProjectItemAdded);
                VisualStudioEventBridge.VisualStudioEvents.ProjectItemRemoved -= new VisualStudioEventBridge.ProjectItemRemovedEventHandler(ProjectItemRemoved);

                //VisualStudioEventBridge.VisualStudioEvents.AfterKeyPress -= new VisualStudioEventBridge.AfterKeyPressEventHandler(AfterKeyPress);
                //VisualStudioEventBridge.VisualStudioEvents.BeforeKeyPress -= new VisualStudioEventBridge.BeforeKeyPressEventHandler(BeforeKeyPress);



                foreach (ProjectItem projectItem in ProjectItems)
                    projectItem.ProjectItemRemoved();
                ProjectItems.Clear();
                System.TimeSpan timeSpan = System.DateTime.Now - start;
                System.Diagnostics.Debug.WriteLine(Name + "  " + timeSpan.ToString());

            }
            catch (System.Exception error)
            {

            }

        }

        /// <MetaDataID>{537CE781-838C-44B8-BE19-AAD7D28789F5}</MetaDataID>
        void ProjectRemoved(EnvDTE.Project Project)
        {
            if (Project != VSProject)
                return;
            try
            {
                IDEManager.SynchroForm.Timer.Tick -= new System.EventHandler(OnTimer);
                int count = this.Residents.Count;
                foreach (ProjectItem projectItem in ProjectItems)
                    projectItem.ProjectItemRemoved();
                ProjectItems.Clear();

            }
            catch (System.Exception error)
            {

            }


        }

        /// <MetaDataID>{16fa1286-9973-461d-a290-3c62d8ff00b5}</MetaDataID>
        static Collections.Generic.Set<OOAdvantech.CodeMetaDataRepository.CodeElementContainer> RefreshStartPointCodeElements = new OOAdvantech.Collections.Generic.Set<CodeElementContainer>();
        /// <MetaDataID>{569A9ECB-5D58-42B0-A18C-1718D0974649}</MetaDataID>
        void LineChanged(EnvDTE.TextPoint StartPoint, EnvDTE.TextPoint EndPoint, int Hint)
        {
            try
            {
                if (StartPoint.Parent.Language=="CSharp" && StartPoint.Parent.Parent.ProjectItem.ContainingProject == VSProject)
                {
                    int linesDown = EndPoint.Line - StartPoint.Line;
                    if (linesDown == 0 || StartPoint.Parent.Parent.Name.IndexOf(".Designer.cs") != -1)
                        return;

                    ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(StartPoint.Parent.Parent.ProjectItem) as ProjectItem;
                    if (projectItem != null)
                    {
                        foreach (CodeElementContainer metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
                        {
                            try
                            {
                                if (metaObject.GetLine(projectItem) >= StartPoint.Line)
                                    metaObject.LineChanged(projectItem, linesDown);
                                metaObject.RefreshStartPoint();
                            }
                            catch (System.Exception Error)
                            {
                            }
                        }
                    }
                }
            }
            catch (System.Exception error)
            {

                
            }
            //try
            //{
            //    //Refresh();
            //    if (StartPoint.Parent.Parent.ProjectItem.ContainingProject == VSProject)
            //    {
            //        ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(StartPoint.Parent.Parent.ProjectItem) as ProjectItem;
            //        if (projectItem != null)
            //        {
            //            foreach (CodeElementContainer metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
            //            {

            //                if (StartPoint.Line <= metaObject.Line)
            //                    RefreshStartPointCodeElements.Add(metaObject);
            //            }
            //        }

            //    }
            //}
            //catch (System.Exception error)
            //{

            //}


        }
        struct CodeClassCursorPos
        {
            public int Line;
            public int LineOffsetChar;
            public System.DateTime LastCahengeDateTime;
            public int TimeoutInMiliseconds;

        }

        /// <MetaDataID>{CE65CBF9-D0F3-49C3-BA17-5470EFBC9D90}</MetaDataID>
        void CodeElementDeleted(object parent, EnvDTE.CodeElement element)
        {

            if (element.ProjectItem.ContainingProject == VSProject)
            {
                int count = this.Residents.Count;
                //Refresh();
                ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(element.ProjectItem) as ProjectItem;
                if (projectItem != null)
                {
                    bool removed = false;
                    bool update = false;
                    foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
                    {
                        try
                        {

                            CodeElementContainer codeElementContainer = metaObject as CodeElementContainer;
                            if (codeElementContainer != null && codeElementContainer.ContainCodeElement(element, parent))
                            {
                                if (RefreshStartPointCodeElements.Contains(codeElementContainer))
                                    RefreshStartPointCodeElements.Remove(codeElementContainer);
                                update = (metaObject is MetaDataRepository.Classifier) & metaObject.Namespace == null;
                                codeElementContainer.CodeElementRemoved(element.ProjectItem);

                                removed = true;

                                break;
                            }
                            if (element.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
                            {
                                if (metaObject.Namespace != null && metaObject.Namespace.FullName == element.Name)
                                {
                                    if (element.ProjectItem.FileCodeModel.CodeElements.Count > 0)
                                    {
                                        EnvDTE.EditPoint editPoint = element.ProjectItem.FileCodeModel.CodeElements.Item(1).StartPoint.CreateEditPoint();

                                        editPoint.MoveToLineAndOffset(codeElementContainer.GetLine(projectItem), codeElementContainer.GetLineCharOffset(projectItem));
                                        EnvDTE.CodeElement classCodeElement = editPoint.get_CodeElement(codeElementContainer.Kind);
                                        codeElementContainer.RefreshCodeElement(classCodeElement);
                                        update = true;

                                    }


                                }

                            }

                        }
                        catch (System.Exception error)
                        {

                        }
                    }

                    try
                    {
                        if (!removed && parent is EnvDTE.CodeElement)
                            CodeElementChanged(parent as EnvDTE.CodeElement, EnvDTE80.vsCMChangeKind.vsCMChangeKindUnknown);
                    }
                    catch (System.Exception error)
                    {
                    }
                    if (update)
                        MetaObjectChangeState();

                }
                // MetaObjectChangeState();
            }
        }
        System.Collections.Generic.Dictionary<EnvDTE.CodeElement, CodeClassCursorPos> codeClasses = new System.Collections.Generic.Dictionary<EnvDTE.CodeElement, CodeClassCursorPos>();
        /// <MetaDataID>{9aedae31-e4ff-434c-b630-b9e529fef2b5}</MetaDataID>
        bool InCodeElementChanged = false;

        /// <MetaDataID>{2b1c7124-6ebb-4cca-b92c-e815f5f38d32}</MetaDataID>
        void CodeElementChangedAss(EnvDTE.CodeElement element, EnvDTE80.vsCMChangeKind change)
        {
            EnvDTE.vsCMElement kind = element.Kind;
        }
        /// <MetaDataID>{106AB9A4-E18B-4438-AACF-410425E7EA81}</MetaDataID>
        void CodeElementChanged(EnvDTE.CodeElement element, EnvDTE80.vsCMChangeKind change)
        {


            //if (!IDEManager.SynchroForm.InvokeRequired)
            //{
            //    CodeElementChangedHandler codeElementAdded = new CodeElementChangedHandler(CodeElementChangedAss);
            //    codeElementAdded.BeginInvoke(EnvDTEObjectProxy.ControlRoseObject(element) as EnvDTE.CodeElement, change, null, null);

            //}

            if (element.ProjectItem == null)
                return;

            try
            {
                if (Laguage != ProjectLanguage.CSharp)
                    return; 

                if (element.ProjectItem.ContainingProject == VSProject)
                {
                    //    if (!IDEManager.SynchroForm.InvokeRequired)
                    //    {


                    //        VisualStudioEventBridge.CodeElementChangedEventHandler codeChangeHander = new VisualStudioEventBridge.CodeElementChangedEventHandler((this.CodeElementChanged));

                    //        codeChangeHander.BeginInvoke(System.Runtime.InteropServices.i element, EnvDTE80.vsCMChangeKind.vsCMChangeKindRename, null, null);
                    //        return;
                    //    }

                    try
                    {
                        EnvDTE.CodeFunction operationMember = (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementFunction) as EnvDTE.CodeFunction;

                        if (operationMember != null)
                            element = operationMember as EnvDTE.CodeElement;
                        else
                        {
                            EnvDTE.CodeProperty propertyMember = (element.ProjectItem.Document.Selection as EnvDTE.TextSelection).ActivePoint.get_CodeElement(EnvDTE.vsCMElement.vsCMElementProperty) as EnvDTE.CodeProperty;
                            if (propertyMember != null)
                                element = propertyMember as EnvDTE.CodeElement;

                        }

                        if (element.Kind == EnvDTE.vsCMElement.vsCMElementClass && codeClasses.ContainsKey(element) && (System.DateTime.Now - codeClasses[element].LastCahengeDateTime).TotalMilliseconds > codeClasses[element].TimeoutInMiliseconds)
                        {
                            codeClasses.Remove(element);
                        }
                        else if (element.Kind == EnvDTE.vsCMElement.vsCMElementClass)
                        {
                            CodeClassCursorPos codeClassCursorPos = new CodeClassCursorPos();
                            if (codeClasses.ContainsKey(element))
                                codeClassCursorPos = codeClasses[element];
                            codeClassCursorPos.LineOffsetChar = element.StartPoint.Parent.Selection.TopPoint.LineCharOffset;
                            codeClassCursorPos.Line = element.StartPoint.Parent.Selection.TopPoint.Line;
                            codeClassCursorPos.LastCahengeDateTime = System.DateTime.Now;
                            codeClassCursorPos.TimeoutInMiliseconds = 2500;
                            try
                            {
                                codeClassCursorPos.TimeoutInMiliseconds = (element as EnvDTE.CodeClass).Members.Count * 25;
                            }
                            catch (System.Exception error)
                            {
                            }
                            codeClasses[element] = codeClassCursorPos;
                            EnableDelayLoadModelTimer();
                            return;
                        }

                        if (InCodeElementChanged)
                            return;
                        InCodeElementChanged = true;
                        long residentsCount = Residents.Count;
                        bool namespaceChanged = false;
                        if (element.Kind == EnvDTE.vsCMElement.vsCMElementParameter)
                            element = (element as EnvDTE.CodeParameter).Parent;
                        ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(element.ProjectItem) as ProjectItem;
                        if (projectItem != null)
                        {
                            foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
                            {
                                CodeElementContainer codeElementContainer = metaObject as CodeElementContainer;
                                if (codeElementContainer != null
                                    && codeElementContainer.ContainCodeElement(element, null))
                                {
                                    string oldSignature = null;
                                    string newSignature = null;
                                    if (codeElementContainer is OOAdvantech.MetaDataRepository.Feature)
                                        oldSignature = CodeClassifier.GetSignature((codeElementContainer as OOAdvantech.MetaDataRepository.Feature), this);
                                    if (!CodeClassifier.HasDeclarationChange(codeElementContainer, element))
                                        return;
                                    codeElementContainer.RefreshCodeElement(element);
                                    if (codeElementContainer is OOAdvantech.MetaDataRepository.Feature)
                                        newSignature = CodeClassifier.GetSignature((codeElementContainer as OOAdvantech.MetaDataRepository.Feature), this);
                                    if (newSignature != oldSignature)
                                    {

                                        foreach (MetaDataRepository.Classifier clasifier in (codeElementContainer as OOAdvantech.MetaDataRepository.Feature).Owner.GetAllSpecializeClasifiers())
                                        {
                                            try
                                            {
                                                if (clasifier is Class)
                                                    (clasifier as Class).UpdateRealizations(oldSignature, newSignature);
                                                if (clasifier is Interface)
                                                    (clasifier as Interface).UpdateRealizations(oldSignature, newSignature);
                                                if (clasifier is Structure)
                                                    (clasifier as Structure).UpdateRealizations(oldSignature, newSignature);


                                                //(clasifier as CodeElementContainer).RefreshCodeElement((clasifier as CodeElementContainer).CodeElement);

                                            }
                                            catch (System.Exception error)
                                            {
                                            }
                                        }
                                    }
                                    return;
                                }
                            }

                            if ((element is EnvDTE.CodeVariable ||
                                element is EnvDTE.CodeProperty ||
                                element is EnvDTE.CodeFunction ||
                                element is EnvDTE.CodeAttribute))
                            {

                                EnvDTE.CodeElement parentCodeElement = GetCodeElementParent(element);
                                if ((parentCodeElement is EnvDTE.CodeVariable ||
                                 parentCodeElement is EnvDTE.CodeProperty ||
                                 parentCodeElement is EnvDTE.CodeFunction ||
                                 parentCodeElement is EnvDTE.CodeAttribute))
                                {
                                    parentCodeElement = GetCodeElementParent(parentCodeElement);
                                }


                                CodeElementContainer codeElementContainer = GetMemberOwner(element) as CodeElementContainer;
                                if (!(element is EnvDTE.CodeAttribute) && !(codeElementContainer is MetaDataRepository.Classifier))
                                {
                                    if (codeElementContainer != null && parentCodeElement != null && codeElementContainer.ContainCodeElement(parentCodeElement, null))
                                    {
                                        codeElementContainer.RefreshCodeElement(parentCodeElement);
                                        return;
                                    }
                                }
                                else if (projectItem.FindMetaObjectFor(element) == null)
                                {
                                    CodeElementAdded(element);
                                }

                            }

                            if (element.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
                            {


                                foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
                                {
                                    if (metaObject is OOAdvantech.MetaDataRepository.Classifier)
                                    {

                                        CodeElementContainer codeElementContainer = metaObject as CodeElementContainer;
                                        try
                                        {
                                            EnvDTE.EditPoint editPoint = element.StartPoint.CreateEditPoint();

                                            editPoint.MoveToLineAndOffset(codeElementContainer.GetLine(projectItem), codeElementContainer.GetLineCharOffset(projectItem));
                                            EnvDTE.CodeElement classCodeElement = editPoint.get_CodeElement(codeElementContainer.Kind);
                                            if (classCodeElement.Name == metaObject.Name)
                                            {
                                                EnableDelayLoadModelTimer();
                                                string oldNamespace = null;
                                                if (metaObject.Namespace != null)
                                                    oldNamespace = metaObject.Namespace.FullName;
                                                codeElementContainer.RefreshCodeElement(classCodeElement);
                                                string newNamespace = null;
                                                if (metaObject.Namespace != null)
                                                    newNamespace = metaObject.Namespace.FullName;
                                                if (oldNamespace != newNamespace)
                                                    namespaceChanged = true;
                                            }
                                        }
                                        catch (System.Exception error)
                                        {
                                        }
                                    }
                                }
                            }

                            if (element.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
                            {

                                Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(element.FullName) as Namespace;
                                if (_namespace != null)
                                {
                                    Collections.Generic.Set<MetaDataRepository.Classifier> classifiers = GetClassifiers(element as EnvDTE.CodeNamespace);
                                    foreach (MetaDataRepository.MetaObject metaObject in new Collections.Generic.Set<MetaDataRepository.MetaObject>(_namespace.OwnedElements))
                                    {
                                        if (metaObject is MetaDataRepository.Classifier &&
                                            metaObject is CodeElementContainer &&
                                            (metaObject as CodeElementContainer).ProjectItem == projectItem &&
                                            !classifiers.Contains(metaObject as MetaDataRepository.Classifier))
                                        {
                                            (metaObject as CodeElementContainer).CodeElementRemoved(element.ProjectItem);
                                        }
                                    }
                                }
                            }


                            if (namespaceChanged)//|| residentsCount != Residents.Count)
                                MetaObjectChangeState();
                        }
                    }
                    finally
                    {
                        InCodeElementChanged = false;

                    }
                }

            }
            catch (System.Exception error)
            {

            }


        }
        /// <MetaDataID>{D2089ED5-4AC7-4C95-A681-2E66B6FBAA23}</MetaDataID>
        void CodeElementAdded(EnvDTE.CodeElement element)
        {



            if (element.ProjectItem == null)
                return;
            if (element.Language != EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp)
                return;

            string vasdf = VSProject.UniqueName;
            if (element.ProjectItem.ContainingProject.UniqueName != VSProject.UniqueName)
                return;

            int count = this.Residents.Count;
            if (element.Kind == EnvDTE.vsCMElement.vsCMElementNamespace)
            {
                GetClassifiers(element as EnvDTE.CodeNamespace);
                CodeElementChanged(element, EnvDTE80.vsCMChangeKind.vsCMChangeKindUnknown);
            }
            else if (element.ProjectItem.ContainingProject == VSProject)
            {

                Collections.Generic.Set<MetaDataRepository.Classifier> projectItemsClassifiers = GetClassifiers(element.ProjectItem);
                if (element.Kind == EnvDTE.vsCMElement.vsCMElementClass ||
                    element.Kind == EnvDTE.vsCMElement.vsCMElementInterface ||
                    element.Kind == EnvDTE.vsCMElement.vsCMElementStruct)
                {

                    foreach (MetaDataRepository.Classifier classifier in GetClassifiers(element.ProjectItem))
                    {
                        if (_Residents != null && !_Residents.Contains(classifier))
                            _Residents.Add(classifier);
                        else if (classifier is CodeElementContainer)
                            (classifier as CodeElementContainer).RefreshCodeElement(element);
                    }
                }

                OOAdvantech.MetaDataRepository.Classifier memberOwner = GetMemberOwner(element);
                if (element.Kind == EnvDTE.vsCMElement.vsCMElementProperty ||
                    element.Kind == EnvDTE.vsCMElement.vsCMElementVariable ||
                    element.Kind == EnvDTE.vsCMElement.vsCMElementFunction)
                {
                    if (memberOwner is Class)
                        (memberOwner as Class).AddMember(element);
                    if (memberOwner is Interface)
                        (memberOwner as Interface).AddMember(element);
                    if (memberOwner is Structure)
                        (memberOwner as Structure).AddMember(element);


                }

            }
        }

        /// <MetaDataID>{0610d443-815f-4e94-abcd-ad24f6afbc13}</MetaDataID>
        private static OOAdvantech.MetaDataRepository.Classifier GetMemberOwner(EnvDTE.CodeElement member)
        {
            ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(member.ProjectItem) as ProjectItem;

            EnvDTE.CodeElement parentCodeElement = GetCodeElementParent(member);

            if ((parentCodeElement is EnvDTE.CodeVariable ||
               parentCodeElement is EnvDTE.CodeProperty ||
               parentCodeElement is EnvDTE.CodeFunction ||
               parentCodeElement is EnvDTE.CodeAttribute))
            {
                parentCodeElement = GetCodeElementParent(parentCodeElement);
            }

            if (parentCodeElement == null)
                return null;

            MetaDataRepository.MetaObject metaObject = projectItem.FindMetaObjectFor(parentCodeElement);
            if (metaObject == null && parentCodeElement.Kind == EnvDTE.vsCMElement.vsCMElementClass)
            {
                try
                {
                    metaObject = (MetaObjectMapper.FindMetaObjectFor((parentCodeElement as EnvDTE80.CodeClass2).PartialClasses.Item(1).ProjectItem) as ProjectItem).FindMetaObjectFor((parentCodeElement as EnvDTE80.CodeClass2).PartialClasses.Item(1));
                }
                catch (System.Exception error)
                {
                }
            }
            return metaObject as OOAdvantech.MetaDataRepository.Classifier;
        }

        /// <MetaDataID>{ac625813-d299-499b-a390-f772da1f6c02}</MetaDataID>
        private static EnvDTE.CodeElement GetCodeElementParent(EnvDTE.CodeElement member)
        {
            EnvDTE.CodeElement parentCodeElement = null;
            try
            {
                if (member.Kind == EnvDTE.vsCMElement.vsCMElementProperty)
                    parentCodeElement = (member as EnvDTE.CodeProperty).Parent as EnvDTE.CodeElement;
                else if (member.Kind == EnvDTE.vsCMElement.vsCMElementVariable)
                    parentCodeElement = (member as EnvDTE.CodeVariable).Parent as EnvDTE.CodeElement;
                else if (member.Kind == EnvDTE.vsCMElement.vsCMElementFunction)
                    parentCodeElement = (member as EnvDTE.CodeFunction).Parent as EnvDTE.CodeElement;
                else if (member.Kind == EnvDTE.vsCMElement.vsCMElementAttribute)
                    parentCodeElement = (member as EnvDTE.CodeAttribute).Parent as EnvDTE.CodeElement;
                else
                    return null;
                //if (member.Kind == EnvDTE.vsCMElement.vsCMElementEvent)
                //    parentCodeElement = (member as EnvDTE.CodeEv).Parent as EnvDTE.CodeElement;


            }
            catch { }
            try
            {
                if (parentCodeElement == null)
                    parentCodeElement = (member.StartPoint as EnvDTE.TextPoint).get_CodeElement(EnvDTE.vsCMElement.vsCMElementClass);
            }
            catch { }
            try
            {
                if (parentCodeElement == null)
                    parentCodeElement = (member.StartPoint as EnvDTE.TextPoint).get_CodeElement(EnvDTE.vsCMElement.vsCMElementInterface);
            }
            catch { }

            try
            {
                if (parentCodeElement == null)
                    parentCodeElement = (member.StartPoint as EnvDTE.TextPoint).get_CodeElement(EnvDTE.vsCMElement.vsCMElementStruct);
            }
            catch { }
            return parentCodeElement;
        }

        /// <MetaDataID>{bf772aa6-0d0e-41e6-b1c3-3238181dfeb6}</MetaDataID>
        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                SetIdentity(new OOAdvantech.MetaDataRepository.MetaObjectID(PropertyValue as string));
            }
            else
            {
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
        }

        /// <MetaDataID>{a541ea45-5127-429a-bc79-01b61187ada7}</MetaDataID>
        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                return Identity.ToString();
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }


        /// <MetaDataID>{27C732E1-4F93-434E-8ECB-6CE08B80878C}</MetaDataID>
        private bool IsResidentsLoaded = false;

        /// <MetaDataID>{78f7cbae-2991-4976-9888-8527dcf02861}</MetaDataID>
        System.Collections.Generic.List<EnvDTE.ProjectItem> GetCodeProjectItems(EnvDTE.ProjectItems projectItems)
        {
            System.Collections.Generic.List<EnvDTE.ProjectItem> codeProjectItems = new System.Collections.Generic.List<EnvDTE.ProjectItem>();

            foreach (EnvDTE.ProjectItem projectItem in projectItems)
            {
                if (projectItem.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}")
                    codeProjectItems.Add(projectItem);
                if (projectItem.ProjectItems.Count > 0)
                    codeProjectItems.AddRange(GetCodeProjectItems(projectItem.ProjectItems));
            }
            return codeProjectItems;

        }





        /// <MetaDataID>{06677e97-c51a-4493-abf6-e39c78ac19cf}</MetaDataID>
        static OOAdvantech.MetaDataRepository.Classifier voidClassifier;
        /// <MetaDataID>{458902f9-04b5-4811-9d66-a7dfc0d70764}</MetaDataID>
        static Project()
        {
            voidClassifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(void));
        }




        /// <MetaDataID>{6ae76196-7ea7-4ae5-a570-4866ba8e4725}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Classifier GetClassifier(MetaDataRepository.Classifier usedFromClassifier, EnvDTE.CodeElement codeElement)
        {
            if (LanguageParser.IsGeneric(codeElement) && usedFromClassifier.IsTemplate)
            {
                string typeFullName = null;
                System.Collections.Generic.List<string> parameters = null;
                LanguageParser.GetGenericMetaData(codeElement.FullName, VSProject.CodeModel.Language, ref typeFullName, ref parameters);
                if (!string.IsNullOrEmpty(typeFullName) && parameters.Count > 0)
                    return GetTemplateInstantiation(typeFullName, VSProject.CodeModel.Language, parameters, usedFromClassifier.OwnedTemplateSignature.OwnedParameters);
                else
                    return UnknownClassifier.GetClassifier(codeElement.FullName, this);
            }
            else
            {
                return GetClassifier(codeElement);
            }

        }


        /// <MetaDataID>{404ef1e4-16e0-4853-8a07-cc48cbe9fbef}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Classifier GetClassifier(EnvDTE.CodeElement codeElement)
        {
            // ProjectItem
            MetaDataRepository.Classifier classifier = null;
            if (codeElement.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationExternal)
            {

                if (LanguageParser.IsGeneric(codeElement))//((codeElement is EnvDTE80.CodeClass2) && (codeElement as EnvDTE80.CodeClass2).IsGeneric) || ((codeElement is EnvDTE80.CodeInterface2) && (codeElement as EnvDTE80.CodeInterface2).IsGeneric))
                {

                    string typeFullName = null;
                    System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();
                    LanguageParser.GetGenericMetaDataFromCSharpType(codeElement.FullName, ref typeFullName, ref parameters);
                    //if (ownerClassifier.OwnedTemplateSignature != null)
                    //    classifier = project.GetExternalClassifier(typeFullName + "`" + parameters.Count.ToString());
                    //else
                    classifier = GetTemplateInstantiation(typeFullName, codeElement.Language, parameters);

                    //(ImplementationUnit as Project).GetTemplateInstantiation(
                    //string typeFullName = null;
                    //System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();

                    //LanguageParser.GetGenericMetaDataFromCSharpType(codeElement.FullName, ref typeFullName, ref parameters);
                    //if (OwnedTemplateSignature != null)
                    //{
                    //    classifier = project.GetExternalClassifier(typeFullName + "`" + parameters.Count.ToString());
                    //}
                    //else
                    //{
                    //    Collections.Generic.List<MetaDataRepository.IParameterableElement> parametersClasifiers = new Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>();
                    //    bool genericCodeType = false;
                    //    string genericTypeFullName = typeFullName + "`" + parameters.Count.ToString();
                    //    typeFullName += "`" + parameters.Count.ToString() + "[";

                    //    foreach (string parameter in parameters)
                    //    {
                    //        if (!LanguageParser.IsGeneric(parameter, codeElement.Language))
                    //        {
                    //            MetaDataRepository.Classifier parameterClassifier = project.GetExternalClassifier(parameter);
                    //            parametersClasifiers.Add(parameterClassifier);
                    //            if (parameterClassifier.ImplementationUnit is Project)
                    //                genericCodeType = true;
                    //            typeFullName += "[" + parameterClassifier.FullName + "," + parameterClassifier.ImplementationUnit.Identity + "]";
                    //        }
                    //    }
                    //    typeFullName += "]";
                    //    classifier = project.GetExternalClassifier(typeFullName, genericCodeType);
                    //    if (classifier == null)
                    //    {
                    //        MetaDataRepository.Classifier genericClassifier = project.GetExternalClassifier(genericTypeFullName);
                    //        MetaDataRepository.TemplateBinding templateBinding = new OOAdvantech.MetaDataRepository.TemplateBinding(genericClassifier, parametersClasifiers);
                    //        if (genericClassifier is MetaDataRepository.Class)
                    //            classifier = new Class(templateBinding, ImplementationUnit);

                    //        if (genericClassifier is MetaDataRepository.Interface)
                    //        {
                    //            // if (LanguageParser.IsGeneric(codeTypeRef))
                    //            //{
                    //            //    string typeFullName = null;
                    //            //    System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();

                    //            //    LanguageParser.GetGenericMetaDataFromCSharpType(codeTypeRef.AsString, ref typeFullName, ref parameters);
                    //            //}

                    //            classifier = new Interface(templateBinding, this.ImplementationUnit);

                    //        }
                    //        if (genericClassifier is MetaDataRepository.Structure)
                    //            classifier = new Structure(codeElement as EnvDTE.CodeStruct);

                    //    }

                    //UserInterfaceTest.Mycollection`2


                    //}
                }
                else
                {

                    string fullName = null;
                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
                        fullName = codeElement.Name;
                    else
                        fullName = codeElement.FullName;

                    foreach (MetaDataRepository.Dependency dependency in ClientDependencies)
                    {
                        classifier = (dependency.Supplier as MetaDataRepository.Component).GetClassifier(fullName, true);
                        if (classifier != null)
                            return classifier;
                    }

                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
                        classifier = GetExternalClassifier(codeElement.Name);
                    else
                        classifier = GetExternalClassifier(codeElement.FullName);

                }


                // object dd = codeTypeRef.CodeType.ProjectItem;

            }
            else if (codeElement.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationProject)
            {

                classifier = MetaObjectMapper.FindMetaObjectFor(codeElement) as MetaDataRepository.Classifier;
                if (classifier == null)
                {
                    if (codeElement.Kind == EnvDTE.vsCMElement.vsCMElementOther)
                        classifier = GetExternalClassifier(codeElement.Name);
                    else
                        classifier = GetExternalClassifier(codeElement.FullName);
                }


            }
            return classifier;

        }

        /// <MetaDataID>{f6c595d4-4542-4c6e-b34a-c200c1ab736e}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Classifier GetClassifier(EnvDTE.CodeTypeRef codeTypeRef)
        {
            try
            {
                MetaDataRepository.Classifier classifier = null;
                if (codeTypeRef.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefVoid)
                    return voidClassifier;
                if (codeTypeRef.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefArray)
                {
                    //TODO  πως δουλεύει σε περίπτωση template array
                    //και αυτός ο κώδικας δεν επιστρέφη το σώστο 
                    string fullName = codeTypeRef.AsString.Substring(0, codeTypeRef.AsString.Length - 2);
                    classifier = GetExternalClassifier(fullName, true);
                    return classifier;

                }
                EnvDTE.CodeType codeType = null;
                try
                {
                    if (codeTypeRef.TypeKind != EnvDTE.vsCMTypeRef.vsCMTypeRefOther)
                        codeType = codeTypeRef.CodeType;
                }
                catch (System.Exception error)
                {
                }
                if (codeType == null || codeType.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationExternal)
                {
                    if (LanguageParser.IsGeneric(codeTypeRef,VSProject.CodeModel.Language ))
                    {
                        string typeFullName = null;
                        System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();
                        LanguageParser.GetGenericMetaDataFromCSharpType(codeTypeRef.AsString, ref typeFullName, ref parameters);
                        if(typeFullName==null)
                            LanguageParser.GetGenericMetaDataFromCSharpType(codeTypeRef.AsFullName, ref typeFullName, ref parameters); //for System.Nullable<>
                        //if (ownerClassifier.OwnedTemplateSignature != null)
                        //    classifier = project.GetExternalClassifier(typeFullName + "`" + parameters.Count.ToString());
                        //else
                        classifier = GetTemplateInstantiation(typeFullName,VSProject.CodeModel.Language, parameters);
                    }
                    else
                        classifier = GetExternalClassifier(codeTypeRef.AsFullName);

                }
                else if (codeType.InfoLocation == EnvDTE.vsCMInfoLocation.vsCMInfoLocationProject)
                {
                    classifier = MetaObjectMapper.FindMetaObjectFor(codeTypeRef.CodeType) as MetaDataRepository.Classifier;
                    if (classifier == null)
                        classifier = GetExternalClassifier(codeTypeRef.AsFullName);
                }
                if (classifier == null)
                    return UnknownClassifier.GetClassifier(codeTypeRef.AsString, this);
                else
                    return classifier;
            }
            catch (System.Exception error)
            {
                return UnknownClassifier.GetClassifier(codeTypeRef.AsString, this);
            }

        }



        /// <MetaDataID>{ccac6378-68c1-46df-852f-593ed087f211}</MetaDataID>
        Collections.Generic.Set<MetaDataRepository.Classifier> GetClassifiers(EnvDTE.CodeElement codeElement)
        {

            ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(codeElement.ProjectItem) as ProjectItem;
            if (projectItem != null)
            {
                foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>(projectItem.MetaObjectImplementations))
                {
                    CodeElementContainer codeElementContainer = metaObject as CodeElementContainer;
                    //CodeElementContainer metaObject
                    if (codeElementContainer != null
                        && (codeElementContainer.ContainCodeElement(codeElement, null)))
                    {
                        if (codeElementContainer.Kind == codeElement.Kind && IsResidentsLoaded)
                            codeElementContainer.RefreshCodeElement(codeElement);

                    }
                }
            }
            Collections.Generic.Set<MetaDataRepository.Classifier> classifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
            switch (codeElement.Kind)
            {
                case EnvDTE.vsCMElement.vsCMElementClass:
                    {
                        OOAdvantech.CodeMetaDataRepository.Class _class = null;
                        int partialClassesCount = 0;
                        try
                        {
                            partialClassesCount = (codeElement as EnvDTE80.CodeClass2).PartialClasses.Count;
                        }
                        catch (System.Exception error)
                        {
                        }
                        if (partialClassesCount > 1)
                        {
                            foreach (EnvDTE.CodeElement partialClass in (codeElement as EnvDTE80.CodeClass2).PartialClasses)
                            {
                                _class = MetaObjectMapper.FindMetaObjectFor(partialClass as EnvDTE.CodeClass) as Class;
                                if (_class != null && IsResidentsLoaded)
                                {
                                    //_class.RefreshCodeElement(codeElement);
                                    MetaObjectMapper.AddTypeMap(codeElement, _class);
                                    break;
                                }
                            }
                        }
                        else
                            _class = MetaObjectMapper.FindMetaObjectFor(codeElement as EnvDTE.CodeClass) as Class;

                        if (_class == null)
                        {
                            object objectm = MetaObjectMapper.FindMetaObjectFor(VSProject);
                            _class = new Class(codeElement as EnvDTE.CodeClass);
                            MetaObjectMapper.AddTypeMap(codeElement, _class);
                        }

                        try
                        {
                            if (codeElement.ProjectItem.ContainingProject == VSProject && !_Residents.Contains(_class))
                                _Residents.Add(_class);

                        }
                        catch (System.Exception error)
                        {
                        }
                        classifiers.Add(_class);
                        break;
                    }
                case EnvDTE.vsCMElement.vsCMElementDelegate:

                    OOAdvantech.CodeMetaDataRepository.Class _delegateClass = MetaObjectMapper.FindMetaObjectFor(codeElement as EnvDTE.CodeDelegate) as Class;
                    if (_delegateClass == null)
                    {
                        _delegateClass = new Class(codeElement as EnvDTE.CodeDelegate);
                        MetaObjectMapper.AddTypeMap(codeElement, _delegateClass);
                    }
                    classifiers.Add(_delegateClass);
                    break;

                case EnvDTE.vsCMElement.vsCMElementEnum:
                    {
                        OOAdvantech.CodeMetaDataRepository.Enumeration enumeration = MetaObjectMapper.FindMetaObjectFor(codeElement as EnvDTE.CodeEnum) as Enumeration;
                        if (enumeration == null)
                        {
                            enumeration = new Enumeration(codeElement as EnvDTE.CodeEnum);
                            MetaObjectMapper.AddTypeMap(codeElement, enumeration);
                        }
                        try
                        {
                            if (codeElement.ProjectItem.ContainingProject == VSProject && !_Residents.Contains(enumeration))
                                _Residents.Add(enumeration);

                        }
                        catch (System.Exception error)
                        {
                        }
                        classifiers.Add(enumeration);
                        break;
                    }
                case EnvDTE.vsCMElement.vsCMElementInterface:
                    {
                        Interface _interface = MetaObjectMapper.FindMetaObjectFor(codeElement as EnvDTE.CodeInterface) as Interface;
                        if (_interface == null)
                        {
                            _interface = new Interface(codeElement as EnvDTE.CodeInterface);
                            MetaObjectMapper.AddTypeMap(codeElement, _interface);
                            try
                            {
                                if (codeElement.ProjectItem.ContainingProject == VSProject && !_Residents.Contains(_interface))
                                    _Residents.Add(_interface);

                            }
                            catch (System.Exception error)
                            {
                            }
                        }
                        classifiers.Add(_interface);
                        break;
                    }
                case EnvDTE.vsCMElement.vsCMElementStruct:
                    {
                        Structure _structure = MetaObjectMapper.FindMetaObjectFor(codeElement as EnvDTE.CodeStruct) as Structure;
                        if (_structure == null)
                        {
                            _structure = new Structure(codeElement as EnvDTE.CodeStruct);
                            MetaObjectMapper.AddTypeMap(codeElement, _structure);
                        }
                        try
                        {
                            if (codeElement.ProjectItem.ContainingProject == VSProject && !_Residents.Contains(_structure))
                                _Residents.Add(_structure);

                        }
                        catch (System.Exception error)
                        {
                        }
                        classifiers.Add(_structure);
                        break;
                    }
                case EnvDTE.vsCMElement.vsCMElementNamespace:
                    {
                        Namespace _namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(codeElement.FullName);
                        if (_namespace == null)
                            _namespace = new Namespace(codeElement as EnvDTE.CodeNamespace);

                        foreach (EnvDTE.CodeElement namespaceCodeElement in (codeElement as EnvDTE.CodeNamespace).Members)
                        {
                            classifiers.AddRange(GetClassifiers(namespaceCodeElement));
                        }

                        break;
                    }

            }
            return classifiers;

        }


        public void UpdateObjectStateCode()
        {
            foreach (Class _class in Residents.OfType<Class>().ToList())
                _class.UpdateObjectStateCode();
        }


        /// <MetaDataID>{49980269-a2f4-40eb-8178-d8427f6cfa39}</MetaDataID>
        internal Collections.Generic.Set<MetaDataRepository.Classifier> GetClassifiers(EnvDTE.ProjectItem vsProjectItem)
        {
            Collections.Generic.Set<MetaDataRepository.Classifier> classifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();

            //Project Item Type   GUID  
            //Physical File       {6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}
            //Physical Folder     {6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}
            //Virtual Folder      {6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}
            //Subproject          {EA6618E8-6E24-4528-94BE-6889FE16485C}


            if (vsProjectItem.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}" ||( vsProjectItem.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}" &&vsProjectItem.FileCodeModel==null))
            {
                classifiers.AddRange(GetClassifiers(vsProjectItem.ProjectItems));
                return classifiers;
            }
            ProjectItem projectItem = MetaObjectMapper.FindMetaObjectFor(vsProjectItem) as ProjectItem;
            if (projectItem == null)
            {
                projectItem = new ProjectItem(vsProjectItem, MetaObjectMapper.FindMetaObjectFor(vsProjectItem.ContainingProject) as Project);
                ProjectItems.Add(projectItem);
            }
            if (vsProjectItem.FileCodeModel == null)
                return classifiers;



            // MetaObjectMapper.AddTypeMap(VSProject, this);
            EnvDTE.CodeElements ccodeElements = vsProjectItem.FileCodeModel.CodeElements;
            foreach (EnvDTE.CodeElement codeElement in vsProjectItem.FileCodeModel.CodeElements)
            {
                classifiers.AddRange(GetClassifiers(codeElement));
            }
            return classifiers;


        }

        /// <MetaDataID>{E9112D38-839E-4A7A-BF85-F279671F3514}</MetaDataID>
        private Collections.Generic.Set<MetaDataRepository.Classifier> GetClassifiers(EnvDTE.ProjectItems projectItems)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                System.Reflection.MethodInfo methodInfo = GetType().GetMethod("GetClassifiers", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[1] { typeof(EnvDTE.ProjectItems) }, null);
                return IDEManager.SynchroForm.SynchroInvoke(methodInfo, this, new object[1] { projectItems }) as Collections.Generic.Set<MetaDataRepository.Classifier>;
            }

            Collections.Generic.Set<MetaDataRepository.Classifier> classifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
            try
            {

                foreach (EnvDTE.ProjectItem vsProjectItem in projectItems)
                {
                    try
                    {

                        classifiers.AddRange(GetClassifiers(vsProjectItem));

                    }
                    catch (System.Exception error)
                    {
                    }
                }

            }
            catch (System.Exception error)
            {

            }
            return classifiers;
        }
        /// <MetaDataID>{9d0ec13a-20fe-403f-bb93-76878de49a12}</MetaDataID>
        public MetaDataRepository.Classifier GetClassifierFromProjectAndRefs(string fullName, string assemblyName, bool caseSensitive)
        {
            return GetExternalClassifier(fullName, assemblyName, caseSensitive);

        }


        /// <MetaDataID>{FD6DB491-3CC6-4B40-8D67-A513BB24A6E7}</MetaDataID>
        private Collections.Generic.Set<MetaDataRepository.Classifier> GetClassifiers(EnvDTE.CodeNamespace codeNamespace)
        {
            bool classifierAdded = false;
            Collections.Generic.Set<MetaDataRepository.Classifier> classifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
            foreach (EnvDTE.CodeElement codeElement in codeNamespace.Members)
            {
                OOAdvantech.MetaDataRepository.Classifier classifier = MetaObjectMapper.FindMetaObjectFor(codeElement) as OOAdvantech.MetaDataRepository.Classifier;
                if (classifier == null)
                {
                    ProjectItem projectItem = ProjectItem.GetProjectItem(codeElement.ProjectItem);
                    foreach (MetaDataRepository.MetaObject metaObject in projectItem.MetaObjectImplementations)
                    {
                        if (metaObject is CodeElementContainer && (metaObject as CodeElementContainer).ContainCodeElement(codeElement, null))
                        {
                            classifier = metaObject as MetaDataRepository.Classifier;
                            break;
                        }
                    }
                }

                if (classifier != null)
                {
                    classifiers.Add(classifier);

                }
                else
                {

                    switch (codeElement.Kind)
                    {
                        case EnvDTE.vsCMElement.vsCMElementClass:

                            OOAdvantech.CodeMetaDataRepository.Class _class = GetClassifier(codeElement.FullName, true) as Class;
                            if (_class == null)
                            {
                                _class = new Class(codeElement as EnvDTE.CodeClass);
                                if (_class.ImplementationUnit == this)
                                    _Residents.Add(_class);
                                classifierAdded = true;

                                MetaObjectMapper.AddTypeMap(codeElement, _class);
                            }
                            classifiers.Add(_class);
                            break;
                        case EnvDTE.vsCMElement.vsCMElementDelegate:
                            OOAdvantech.CodeMetaDataRepository.Class _delegateClass = GetClassifier(codeElement.FullName, true) as Class;
                            if (_delegateClass == null)
                            {
                                _delegateClass = new Class(codeElement as EnvDTE.CodeDelegate);
                                if (_delegateClass.ImplementationUnit == this)
                                    _Residents.Add(_delegateClass);
                                classifierAdded = true;

                            }
                            classifiers.Add(_delegateClass);
                            break;


                        case EnvDTE.vsCMElement.vsCMElementEnum:

                            OOAdvantech.CodeMetaDataRepository.Enumeration enumeration = GetClassifier(codeElement.FullName, true) as Enumeration;
                            if (enumeration == null)
                            {
                                enumeration = new Enumeration(codeElement as EnvDTE.CodeEnum);
                                if (enumeration.ImplementationUnit == this)
                                    _Residents.Add(enumeration);
                                classifierAdded = true;

                            }
                            classifiers.Add(enumeration);
                            break;

                        case EnvDTE.vsCMElement.vsCMElementInterface:

                            Interface _interface = GetClassifier(codeElement.FullName, true) as Interface;
                            if (_interface == null)
                            {
                                _interface = new Interface(codeElement as EnvDTE.CodeInterface);
                                if (_interface.ImplementationUnit == this)
                                    _Residents.Add(_interface);
                                classifierAdded = true;

                            }
                            classifiers.Add(_interface);
                            MetaObjectMapper.AddTypeMap(codeElement, _interface);
                            break;

                        case EnvDTE.vsCMElement.vsCMElementStruct:
                            Structure _structure = GetClassifier(codeElement.FullName, true) as Structure;
                            if (_structure == null)
                            {
                                _structure = new Structure(codeElement as EnvDTE.CodeStruct);
                                if (_structure.ImplementationUnit == this)
                                    _Residents.Add(_structure);
                                classifierAdded = true;

                            }
                            classifiers.Add(_structure);
                            break;

                        case EnvDTE.vsCMElement.vsCMElementNamespace:

                            Namespace _namespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(codeElement.FullName);
                            if (_namespace == null)
                                _namespace = new Namespace(codeElement as EnvDTE.CodeNamespace);
                            Namespace parentcodeNamespace = (Namespace)MetaObjectMapper.FindMetaObjectFor(codeNamespace.FullName);
                            _namespace.SetNamespace(parentcodeNamespace);
                            classifiers.AddRange(GetClassifiers(codeElement as EnvDTE.CodeNamespace));
                            break;

                        default:
                            break;
                    }
                }

            }
            if (classifierAdded)
                MetaObjectChangeState();
            return classifiers;
        }

        public static IDEManager GetIDEManager(string IDEProcessID)
        {

            IDEManager ideManager = null;
            if (IDEManagers.ContainsKey(IDEProcessID))
                return IDEManagers[IDEProcessID];


            string channelUri = "ipc://PID" + IDEProcessID;
            if (ideManager == null)
            {
                ideManager = OOAdvantech.Remoting.RemotingServices.CreateRemoteInstance(channelUri, typeof(IDEManager).FullName, typeof(IDEManager).Assembly.FullName) as IDEManager;
                IDEManagers[IDEProcessID] = ideManager;
            }
            return IDEManagers[IDEProcessID];
        }
        /// <MetaDataID>{456d00ca-63fa-4c25-bc1c-574942815fee}</MetaDataID>
        public IDEManager IDEManager
        {
            get
            {
                string IDEProcessID = MsdevManager.Msdev.GetIDEProcessID(VSProject.DTE);

                IDEManager ideManager = null;
                if (IDEManagers.ContainsKey(IDEProcessID))
                    return IDEManagers[IDEProcessID];


                string channelUri = "ipc://PID" + MsdevManager.Msdev.GetIDEProcessID(VSProject.DTE).ToString();
                if (ideManager == null)
                {
                    ideManager = OOAdvantech.Remoting.RemotingServices.CreateRemoteInstance(channelUri, typeof(IDEManager).FullName, typeof(IDEManager).Assembly.FullName )as IDEManager;
                    IDEManagers[IDEProcessID] = ideManager;
                }
                return IDEManagers[IDEProcessID];

            }
        }

        /// <MetaDataID>{3b6fbb28-3bd3-4ba4-b088-49858bd25d9c}</MetaDataID>
        public void ReconnectIDEManager()
        {
            string IDEProcessID = MsdevManager.Msdev.GetIDEProcessID(VSProject.DTE);
            string channelUri = "ipc://PID" + MsdevManager.Msdev.GetIDEProcessID(VSProject.DTE).ToString();
            IDEManagers[IDEProcessID] = OOAdvantech.Remoting.RemotingServices.CreateRemoteInstance(channelUri, typeof(IDEManager).FullName, typeof(IDEManager).Assembly.FullName) as IDEManager;
        }
        /// <MetaDataID>{5da9fcc0-0e8d-4836-9031-22677b518c8a}</MetaDataID>
        static System.Collections.Generic.Dictionary<string, IDEManager> IDEManagers = new System.Collections.Generic.Dictionary<string, IDEManager>();




        /// <MetaDataID>{0f9120b2-8a01-4423-bab0-384c93bdb4c2}</MetaDataID>
        public MetaDataRepository.Classifier GetClassifier(string classifierIdentity)
        {
            try
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() != "devenv")
                {
                    string componentIdentity = null;
                    string projectItemName = null;
                    try
                    {
                        IDEManager.GetProjectResident(classifierIdentity, Identity.ToString(), out componentIdentity, out projectItemName);
                    }
                    catch (System.Exception error)
                    {
                        ReconnectIDEManager();
                        IDEManager.GetProjectResident(classifierIdentity, Identity.ToString(), out componentIdentity, out projectItemName);
                    }

                    if (!string.IsNullOrEmpty(componentIdentity) && !string.IsNullOrEmpty(projectItemName))
                    {
                        if (componentIdentity == Identity.ToString())
                        {
                            EnvDTE.ProjectItem projectItem = GetProjectItemFromPath(projectItemName, VSProject.ProjectItems);
                            foreach (OOAdvantech.MetaDataRepository.Classifier projectItemClassifier in GetClassifiers(projectItem))
                            {

                                if ((projectItemClassifier as CodeElementContainer).Identity.ToString() == classifierIdentity)
                                    return projectItemClassifier;
                            }

                        }
                    }
                    return null;


                }
                else
                {
                    foreach (MetaDataRepository.MetaObject metaObject in Residents)
                    {
                        if ((metaObject as CodeElementContainer).Identity.ToString() == classifierIdentity)
                            if (metaObject is MetaDataRepository.Classifier)
                                return metaObject as MetaDataRepository.Classifier;
                            else
                                return null;
                    }
                }
                return null;
            }
            catch (System.Exception error)
            {
                throw;
            }
        }



        /// <MetaDataID>{988a409c-36ac-4c2a-b5b0-9e418dacb8c5}</MetaDataID>
        System.Collections.Generic.Dictionary<string, MetaDataRepository.Classifier> NamedClassifiers = new System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.Classifier>();



        /// <MetaDataID>{5b87016a-18f7-47e8-9074-64e9ffd32995}</MetaDataID>
        internal MetaDataRepository.Classifier GetTemplateInstantiation(string templateFullName, string language, System.Collections.Generic.List<string> parameters)
        {
            return GetTemplateInstantiation(templateFullName, language, parameters, new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.TemplateParameter>());
        }

        /// <MetaDataID>{96cee652-0713-4109-8ce9-e734c079dd1e}</MetaDataID>
        internal MetaDataRepository.Classifier GetTemplateInstantiation(string templateFullName, string language, System.Collections.Generic.List<string> parameters, Collections.Generic.Set<MetaDataRepository.TemplateParameter> userTemplateParameters)
        {
            //string typeFullName = null;
            MetaDataRepository.Classifier classifier = null;
            Collections.Generic.List<MetaDataRepository.IParameterableElement> parametersClasifiers = new Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>();
            string genericTypeFullName = templateFullName + "`" + parameters.Count.ToString();
            // typeFullName =templateFullName+ "`" + parameters.Count.ToString() + "[";

            //foreach (string parameter in parameters)
            //{
            //    bool resolved = false;
            //    foreach (MetaDataRepository.TemplateParameter templateParameter in userTemplateParameters)
            //    {
            //        if (templateParameter.Name == parameter)
            //        {
            //            parametersClasifiers.Add(templateParameter);
            //            resolved = true;
            //            break;
            //        }
            //    }
            //    if (resolved)
            //        continue;
            //    if (!LanguageParser.IsGeneric(parameter, language))
            //    {
            //        MetaDataRepository.Classifier parameterClassifier = GetExternalClassifier(parameter);
            //        if (parameterClassifier == null)
            //        {
            //            parameterClassifier = UnknownClassifier.GetClassifier(parameter, this);


            //        }
            //            //return null; //TODO η παράμετρος  temlateFullName δεν είναι TemplateInstantiation είναι μια class  "IList<T>" που κλειρωνομείται απο μία MyClass<T>

            //        parametersClasifiers.Add(parameterClassifier);
            //        typeFullName += "[" + parameterClassifier.FullName + "," + parameterClassifier.ImplementationUnit.Identity + "]";

            //    }
            //}
            //typeFullName += "]";


            MetaDataRepository.Classifier genericClassifier = GetExternalClassifier(genericTypeFullName);
            foreach (OOAdvantech.MetaDataRepository.Classifier residentClassifier in _Residents)
            {

                if (residentClassifier.TemplateBinding != null && residentClassifier.TemplateBinding.Signature == genericClassifier.OwnedTemplateSignature)
                {
                    bool exist = true;
                    foreach (OOAdvantech.MetaDataRepository.TemplateParameterSubstitution parameterSubstitution in residentClassifier.TemplateBinding.ParameterSubstitutions)
                    {
                        if (!parametersClasifiers.Contains(parameterSubstitution.ActualParameters[0]))
                            exist = false;
                    }
                    if (exist)
                        return residentClassifier;
                }
            }

            Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement> parameterSubstitutions = new OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>();
            foreach (string parameterName in new System.Collections.Generic.List<string>(parameters))
            {
                MetaDataRepository.IParameterableElement parameter = null;
                foreach (MetaDataRepository.TemplateParameter userTemplateParameter in userTemplateParameters)
                {
                    if (userTemplateParameter.FullName == parameterName)
                    {
                        parameter = userTemplateParameter;
                        break;
                    }
                }
                if (parameter == null)
                    parameter = GetExternalClassifier(parameterName);
                if (parameter == null)
                    parameter = UnknownClassifier.GetClassifier(parameterName, this);
                parameterSubstitutions.Add(parameter);

            }

            MetaDataRepository.TemplateBinding templateBinding = new OOAdvantech.MetaDataRepository.TemplateBinding(genericClassifier, parameterSubstitutions);
            if (genericClassifier is MetaDataRepository.Class)
                classifier = new Class(templateBinding, this);

            if (genericClassifier is MetaDataRepository.Interface)
                classifier = new Interface(templateBinding, this);
            if (genericClassifier is MetaDataRepository.Structure)
                classifier = new Structure(templateBinding, this);

            return classifier;
        }


        /// <MetaDataID>{ef77f3f5-6c26-4068-be6e-1f8d45d069b7}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName, bool caseSensitive)
        {




            bool generic = fullName.IndexOf("`") != -1;

            foreach (MetaDataRepository.Classifier classifier in Residents)
            {
                if (generic && classifier.IsTemplate || classifier.IsBindedClassifier)
                {
                    //classifier.FullName
                    //CodeClassifier.GetLanguageIndipentedFullName
                }
                if (caseSensitive && classifier.FullName == fullName)
                    return classifier;

                if (!caseSensitive && classifier.FullName.ToLower() == fullName.ToLower())
                    return classifier;
            }
            return null;


        }
        internal MetaDataRepository.Classifier GetExternalClassifier(string fullName, bool codeType)
        {
            return GetExternalClassifier(fullName, "", codeType);
        }
        /// <MetaDataID>{96057B72-4554-4FEC-B743-A530AF5FCA0C}</MetaDataID>
        internal MetaDataRepository.Classifier GetExternalClassifier(string fullName, string assemblyName, bool codeType)
        {
            System.DateTime start = System.DateTime.Now;
            string originFullName = fullName;
            try
            {
                string codeTypeFullName = null;

                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() != "devenv")
                {
                    if (NamedClassifiers.ContainsKey(fullName))
                        return NamedClassifiers[fullName];
                    if (fullName.IndexOf("`") == -1)
                    {
                        try
                        {
                            codeTypeFullName = VSProject.CodeModel.CreateCodeTypeRef(fullName).AsFullName;
                        }
                        catch (System.Exception error)
                        {
                            codeTypeFullName = "";
                        }
                    }
                    else
                        codeTypeFullName = "";

                    if (!string.IsNullOrEmpty(codeTypeFullName))
                        fullName = codeTypeFullName;
                    if (NamedClassifiers.ContainsKey(fullName))
                    {
                        if (fullName != originFullName)
                            NamedClassifiers[originFullName] = NamedClassifiers[fullName];
                        return NamedClassifiers[fullName];
                    }



                    string IDEProcessID = MsdevManager.Msdev.GetIDEProcessID(VSProject.DTE);
                    string componentIdentity = null;
                    string projectItemName = null;
                    try
                    {
                        IDEManager.GetClassifierLocation(fullName, Identity.ToString(), out componentIdentity, out projectItemName);
                    }
                    catch (System.Exception error)
                    {
                        ReconnectIDEManager();

                        try
                        {
                            IDEManager.GetClassifierLocation(fullName, Identity.ToString(), out componentIdentity, out projectItemName);
                        }
                        catch (System.Exception inError)
                        {

                            return null;
                        }
                    }
                    if (!string.IsNullOrEmpty(componentIdentity) && string.IsNullOrEmpty(projectItemName))
                    {
                        foreach (MetaDataRepository.Dependency dependency in this.ClientDependencies)
                        {
                            if (dependency.Supplier.Identity.ToString() == componentIdentity)
                            {
                                NamedClassifiers[fullName] = (dependency.Supplier as MetaDataRepository.Component).GetClassifier(fullName, true);
                                if (fullName != originFullName)
                                    NamedClassifiers[originFullName] = NamedClassifiers[fullName];

                                return NamedClassifiers[fullName];
                            }
                        }

                    }
                    if (!string.IsNullOrEmpty(componentIdentity) && !string.IsNullOrEmpty(projectItemName))
                    {
                        if (componentIdentity == Identity.ToString())
                        {
                            EnvDTE.ProjectItem projectItem = GetProjectItemFromPath(projectItemName, VSProject.ProjectItems);
                            foreach (OOAdvantech.MetaDataRepository.Classifier projectItemClassifier in GetClassifiers(projectItem))
                            {
                                if (projectItemClassifier.FullName == fullName)
                                {
                                    NamedClassifiers[fullName] = projectItemClassifier;
                                    if (fullName != originFullName)
                                        NamedClassifiers[originFullName] = NamedClassifiers[fullName];
                                    return projectItemClassifier;
                                }
                            }
                        }
                        foreach (MetaDataRepository.Dependency dependency in this.ClientDependencies)
                        {
                            if (dependency.Supplier.Identity.ToString() == componentIdentity && dependency.Supplier is Project)
                            {
                                EnvDTE.ProjectItem projectItem = GetProjectItemFromPath(projectItemName, (dependency.Supplier as Project).VSProject.ProjectItems);
                                foreach (OOAdvantech.MetaDataRepository.Classifier projectItemClassifier in (dependency.Supplier as Project).GetClassifiers(projectItem))
                                {
                                    if (projectItemClassifier.FullName == fullName)
                                    {
                                        NamedClassifiers[fullName] = projectItemClassifier;
                                        if (fullName != originFullName)
                                            NamedClassifiers[originFullName] = NamedClassifiers[fullName];

                                        return projectItemClassifier;
                                    }
                                }
                                break;

                                //foreach(EnvDTE.ProjectItem projectItem in GetCodeProjectItems((dependency.Supplier as Project).VSProject.ProjectItems))
                                //{

                                //    if(projectItem.Name==projectItemName)
                                //    {


                                //        foreach (OOAdvantech.MetaDataRepository.Classifier projectItemClassifier in (dependency.Supplier as Project).GetClassifiers(projectItem))
                                //        {
                                //            if (projectItemClassifier.FullName == fullName)
                                //            {
                                //                NamedClassifiers[fullName] = projectItemClassifier;
                                //                return projectItemClassifier;
                                //            }
                                //        }
                                //    }

                                //}


                            }
                        }

                    }
                    NamedClassifiers[fullName] = null;
                    if (fullName != originFullName)
                        NamedClassifiers[originFullName] = NamedClassifiers[fullName];

                    return null;


                }


                if (fullName.IndexOf("`") == -1)
                {
                    try
                    {

                        codeTypeFullName = VSProject.CodeModel.CreateCodeTypeRef(fullName).AsFullName;
                    }
                    catch (System.Exception error)
                    {
                        codeTypeFullName = "";

                    }

                }
                else
                    codeTypeFullName = "";


                if (!string.IsNullOrEmpty(codeTypeFullName))
                    fullName = codeTypeFullName;

                MetaDataRepository.Classifier classifier = null;
                if (ExternalClasifiers.TryGetValue(fullName, out classifier))
                    return classifier;

                MetaDataRepository.Component component = null;
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    if (assemblyName == Name)
                    {
                        classifier = GetClassifier(fullName, true);
                        if (classifier != null)
                            return classifier;
                    }
                    else
                    {
                        foreach (MetaDataRepository.Dependency dependency in ClientDependencies)
                        {
                            if (assemblyName == (dependency.Supplier as MetaDataRepository.Component).Name)
                            {
                                classifier = (dependency.Supplier as MetaDataRepository.Component).GetClassifier(fullName, true);
                                if (classifier != null)
                                {
                                    ExternalClasifiers[fullName] = classifier;
                                    return classifier;
                                }
                                break;
                            }
                        }
                    }
                }


                classifier = GetClassifier(fullName, true);
                if (classifier != null)
                    return classifier;
                foreach (MetaDataRepository.Dependency dependency in ClientDependencies)
                {
                    //if (!(dependency.Supplier is Project) && codeType)
                    //    continue;

                    classifier = (dependency.Supplier as MetaDataRepository.Component).GetClassifier(fullName, true);
                    if (classifier != null)
                    {
                        ExternalClasifiers[fullName] = classifier;
                        return classifier;
                    }
                }

                return classifier;
            }
            catch (System.Exception error)
            {
                throw;
            }
            finally
            {
                //System.TimeSpan timeSpan = System.DateTime.Now - start;
                //System.Diagnostics.Debug.WriteLine("GetExternalClassifier " + originFullName + "  " + timeSpan.TotalMilliseconds.ToString());
            }
        }

        System.Collections.Generic.Dictionary<string, MetaDataRepository.Classifier> ExternalClasifiers = new System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.Classifier>();


        /// <MetaDataID>{064a408b-3e01-48bb-8821-7a751ae6f06f}</MetaDataID>
        private EnvDTE.ProjectItem GetProjectItemFromPath(string path, EnvDTE.ProjectItems projectItems)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            string projectItemName = path;
            int nPos = path.IndexOf('\\');
            if (nPos != -1)
            {
                projectItemName = path.Substring(0, nPos);
                if (nPos + 1 != path.Length)
                    path = path.Substring(nPos + 1);
                else
                    path = null;
            }
            else
                path = null;
            foreach (EnvDTE.ProjectItem projectItem in projectItems)
            {
                if (projectItem.Name == projectItemName && path == null)
                    return projectItem;
                if (projectItem.Name == projectItemName && path != null)
                    return GetProjectItemFromPath(path, projectItem.ProjectItems);
            }
            return null;

        }


        /// <MetaDataID>{0AA21302-2E06-45A4-B032-A52015BB45E0}</MetaDataID>
        public MetaDataRepository.Classifier GetExternalClassifier(string fullName, string assemblyName)
        {
            return GetExternalClassifier(fullName, false);
        }

        public MetaDataRepository.Classifier GetExternalClassifier(string fullName)
        {
            return GetExternalClassifier(fullName, false);
        }



        /// <MetaDataID>{65a8d7ff-6f33-46f7-b3a6-ac53afccd9f9}</MetaDataID>
        internal void RebuildMetadata()
        {
            _Residents.RemoveAll();
            foreach (ProjectItem projectItem in ProjectItems)
                projectItem.MetaObjectImplementations.Clear();

            foreach (MetaDataRepository.Classifier classifier in GetClassifiers(VSProject.ProjectItems))
                _Residents.Add(classifier);


        }
    }

    /// <MetaDataID>{6fd1d9ef-acfb-485a-a262-0555c6de7dc8}</MetaDataID>
    public enum ProjectLanguage
    {
        CSharp,
        VBNet,
        JSharp,
        UMLModel,
        Cpp,
        Unknown
    }
}


//Model Project {F088123C-0E9E-452A-89E6-6BA2F21D5CAC}
//Deployment Merge Module	{06A35CCD-C46D-44D5-987B-CF40FF872267}
//Workflow (C#)	{14822709-B5A1-4724-98CA-57A101D1B079}
//Legacy (2003) Smart Device (C#)	{20D4826A-C6FA-45DB-90F4-C717570B9F32}
//Solution Folder	{2150E333-8FDC-42A3-9474-1A3956D46DE8}
//XNA (XBox)	{2DF5C3F4-5A5F-47a9-8E94-23B4456F55E2}
//Workflow Foundation	{32F31D43-81CC-4C15-9DE6-3FC5453562B6}
//Web Application (incl. MVC 5)	{349C5851-65DF-11DA-9384-00065B846F21}
//Test	{3AC096D0-A1C2-E12C-1390-A8335801FDAB}
//Windows Communication Foundation (WCF)	{3D9AD99F-2412-4246-B90B-4EAA41C64699}
//Deployment Cab	{3EA9E505-35AC-4774-B492-AD1749C4943A}
//Smart Device (C#)	{4D628B5B-2FBC-4AA6-8C16-197242AEB884}
//Database (other project types)	{4F174C21-8C12-11D0-8340-0000F80270F8}
//SharePoint (C#)	{593B0543-81F6-4436-BA1E-4747859CAAE2}
//ASP.NET MVC 1.0	{603C0E0B-DB56-11DC-BE95-000D561079B0}
//Windows Presentation Foundation (WPF)	{60DC8134-EBA5-43B8-BCC9-BB4BC16C2548}
//Smart Device (VB.NET)	{68B1623D-7FB9-47D8-8664-7ECEA3297D4F}
//MonoTouch	{6BC8ED88-2882-458C-8E55-DFD12B67127B}
//XNA (Windows)	{6D335F3A-9D43-41b4-9D22-F6F17C4BE596}
//Windows Phone 8/8.1 Blank/Hub/Webview App	{76F1466A-8B6D-4E39-A767-685A06062A39}
//Portable Class Library	{786C830F-07A1-408B-BD7F-6EE04809D6DB}
//C++	{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}
//Deployment Setup	{978C614F-708E-4E1A-B201-565925725DBA}
//Silverlight	{A1591282-1198-4647-A2B1-27E5FF5F6F3B}
//Visual Studio Tools for Applications (VSTA)	{A860303F-1F3F-4691-B57E-529FC101A107}
//Database	{A9ACE9BB-CECE-4E62-9AA4-C7E7C5BD2124}
//Deployment Smart Device Cab	{AB322303-2255-48EF-A496-5904EB18DA55}
//Visual Studio Tools for Office (VSTO)	{BAA0C2D2-18E2-41B9-852F-F413020CAA33}
//Windows Store Apps (Metro Apps)	{BC8A1FFA-BEE3-4634-8014-F334798102B3}
//C# in Dynamics 2012 AX AOT	{BF6F8E12-879D-49E7-ADF0-5503146B24B8}
//Windows Phone 8/8.1 App (C#)	{C089C8C0-30E0-4E22-80C0-CE093F111A43}
//Visual Database Tools	{C252FEB5-A946-4202-B1D4-9916A0590387}
//Legacy (2003) Smart Device (VB.NET)	{CB4CE8C6-1BDB-4DC7-A4D3-65A1999772F8}
//XNA (Zune)	{D399B71A-8929-442a-A9AC-8BEC78BB2433}
//Workflow (VB.NET)	{D59BE175-2ED0-4C54-BE3D-CDAA9F3214C8}
//Windows Phone 8/8.1 App (VB.NET)	{DB03555F-0C8B-43BE-9FF9-57896B3C5E56}
//Web Site	{E24C65DC-7377-472B-9ABA-BC803B73C61A}
//ASP.NET MVC 4.0	{E3E379DF-F4C6-4180-9B81-6769533ABE47}
//ASP.NET MVC 3.0	{E53F8FEA-EAE0-44A6-8774-FFD645390401}
//J#	{E6FDF86B-F3D1-11D4-8576-0002A516ECE8}
//SharePoint (VB.NET)	{EC05E597-79D4-47f3-ADA0-324C4F7C7484}
//Xamarin.Android / Mono for Android	{EFBA0AD7-5A72-4C68-AF49-83D382785DCF}
//Distributed System	{F135691A-BF7E-435D-8960-F99683D2D49C}
//VB.NET	{F184B08F-C81C-45F6-A57F-5ABD9991F28F}
//F#	{F2A71F9B-5D33-465A-A702-920D77279786}
//MonoTouch Binding	{F5B4F3BC-B597-4E2B-B552-EF5D8A32436F}
//ASP.NET MVC 2.0	{F85E285D-A4E0-4152-9332-AB1D724D3325}
//SharePoint Workflow	{F8810EC1-6754-47FC-A15F-DFABD2E3FA90}
//C#	{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}