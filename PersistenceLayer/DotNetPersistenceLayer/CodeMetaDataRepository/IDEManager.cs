using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;


namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{e2cb996e-a3b2-474a-8938-42e6a9cb8ba1}</MetaDataID>
    public class SynchroForm //: System.Windows.Forms.Form
    {
        delegate void RefreshProjectHandler(string projectID, string residentIdentity);
        /// <MetaDataID>{22a489f2-b41c-49ce-8307-8fb4fb3a6a96}</MetaDataID>
        RefreshProjectHandler RefreshProjectEvent;


        /// <MetaDataID>{d8094221-8359-4a65-b010-a13cfb398561}</MetaDataID>
        public void Synchronize(OOAdvantech.MetaDataRepository.MetaObject metaObject, OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            System.Reflection.MethodInfo methodInfo = metaObject.GetType().GetMethod("Synchronize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[1] { typeof(OOAdvantech.MetaDataRepository.MetaObject) }, null);
            SynchroInvoke(methodInfo, metaObject, new object[1] { OriginMetaObject });

        }


        delegate Project GetProjectHandler(string projectID);


        delegate object SynchroInvokeHandler(System.Reflection.MethodInfo methodInfo, object obj, params object[] args);





        delegate void GetProjectResidentHandler(string identity, string projectID, out string componentIdentity, out string projectItemName);
        /// <MetaDataID>{118c144b-73c6-4f45-b002-25eed2ebf569}</MetaDataID>
        GetProjectResidentHandler GetProjectResidentEvent;

        delegate void GetClassifierLocationHandler(string fullName, string activeProjectID, out string componentIdentity, out string projectItemName);
        /// <MetaDataID>{85db7a34-0384-47a3-a392-0450bde6ede8}</MetaDataID>
        GetClassifierLocationHandler GetClassifierLocationEvent;
        /// <MetaDataID>{f0e900c3-9b56-42fd-99b5-9691cfba2431}</MetaDataID>
        public System.Windows.Forms.Timer Timer;
        /// <MetaDataID>{27295405-2b0a-4b0e-918e-f07251b677b1}</MetaDataID>
        private System.ComponentModel.IContainer components;





        /// <MetaDataID>{5b209823-ae63-4852-a134-4d0450b99938}</MetaDataID>
        IDEManager IDEManager;

        public bool InvokeRequired
        {
            get
            {
                return !System.Windows.Application.Current.Dispatcher.CheckAccess();
            }
        }

        /// <MetaDataID>{3845af3e-bf22-49ae-afac-e79e5b86e42d}</MetaDataID>
        public SynchroForm(IDEManager ideManager)
        {
            IDEManager = ideManager;
            InitializeComponent();
            GetProjectResidentEvent = new GetProjectResidentHandler(GetProjectResident);
            GetClassifierLocationEvent = new GetClassifierLocationHandler(GetClassifierLocation);
            //ShowInTaskbar = false;

            //Opacity = 0;
            //FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //MinimizeBox = false;
            //MaximizeBox = false;
            //this.ControlBox = false;
            //MinimumSize = new System.Drawing.Size(1, 1);

            //ClientSize = new System.Drawing.Size(0, 0);
            //Size = new System.Drawing.Size(1, 1);
            //Timer = new System.Windows.Forms.Timer();
            //if (VisualStudioEventBridge.VisualStudioEvents.DTEObject != null)
            //{
            //    Visible = false;
            //    Show();
            //    Visible = false;
            //}
        }
        /// <MetaDataID>{f6e43a14-3b13-4b4d-adbe-060bc016aab8}</MetaDataID>


        /// <MetaDataID>{e50301ae-e660-4850-9b3c-8456ac0bb2e4}</MetaDataID>
        //public void RefreshProject(string projectID, string residentIdentity)
        //{
        //    if (InvokeRequired)
        //        Invoke(RefreshProjectEvent, new object[2] { projectID, residentIdentity });
        //    else
        //    {
        //        if (string.IsNullOrEmpty(residentIdentity))
        //            IDEManager.RefreshProject(projectID);
        //        else
        //            IDEManager.RefreshProject(projectID, residentIdentity);
        //    }
        //}

        /// <MetaDataID>{6c79ddc5-09fd-4796-8a52-c007fa0d63fd}</MetaDataID>
        public object SynchroInvoke(System.Reflection.MethodInfo methodInfo, object obj, params object[] args)
        {
            object value= System.Windows.Application.Current.Dispatcher.Invoke(new SynchroInvokeHandler(InternalSynchroInvoke), methodInfo, obj, args);
            if(value is System.Exception)
            {
                throw (value as System.Exception);
            }
            return value;
        }

        object InternalSynchroInvoke(System.Reflection.MethodInfo methodInfo, object obj, params object[] args)
        {

            try
            {
                return methodInfo.Invoke(obj, args);
            }
            catch (Exception error)
            {
                return error;
                
            }
        }


        /// <MetaDataID>{de11db4c-4829-4055-a53f-147d99bf3d30}</MetaDataID>
        public void GetProjectResident(string identity, string projectID, out string componentIdentity, out string projectItemName)
        {
            componentIdentity = null;

            if (InvokeRequired)
            {
                object[] args = new object[4] { identity, projectID, null, null };
                System.Windows.Application.Current.Dispatcher.Invoke(GetProjectResidentEvent, args);
                componentIdentity = args[2] as string;
                projectItemName = args[3] as string;
            }
            else
                IDEManager.GetProjectResident(identity, projectID, out componentIdentity, out projectItemName);

        }
        /// <MetaDataID>{47611743-80c2-4cbd-a4d3-de93e02cb511}</MetaDataID>
        public void GetClassifierLocation(string fullName, string activeProjectID, out string componentIdentity, out string projectItemName)
        {
            componentIdentity = null;

            if (InvokeRequired)
            {
                object[] args = new object[4] { fullName, activeProjectID, null, null };

                System.Windows.Application.Current.Dispatcher.Invoke(GetClassifierLocationEvent, args);
                componentIdentity = args[2] as string;
                projectItemName = args[3] as string;
            }
            else
                IDEManager.GetClassifierLocation(fullName, activeProjectID, out componentIdentity, out projectItemName);

        }



        /// <MetaDataID>{ae1154d8-05eb-4bb8-bff4-2364745e89df}</MetaDataID>
        public Project GetProject(string projectID)
        {
            if (InvokeRequired)
            {
                object[] args = new object[1] { projectID };
                return System.Windows.Application.Current.Dispatcher.Invoke(new GetProjectHandler(GetProject), args) as Project;
            }
            else
                return IDEManager.GetProject(projectID);
        }

        /// <MetaDataID>{b7f4928e-b5d7-4b0c-bd48-550a509711af}</MetaDataID>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            //this.SuspendLayout();
            //// 
            //// Timer
            //// 
            //this.Timer.Interval = 500;
            //// 
            //// SynchroForm
            //// 
            //this.ClientSize = new System.Drawing.Size(292, 266);
            //this.ControlBox = false;
            //this.MaximizeBox = false;
            //this.MinimizeBox = false;
            //this.Name = "SynchroForm";
            //this.ShowIcon = false;
            //this.ShowInTaskbar = false;
            //this.ResumeLayout(false);

        }
    }
    /// <MetaDataID>{eb8c4f34-1643-4144-bd45-adec8cb2def0}</MetaDataID>
    public class IDEManager : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public IDEManager()
        {

            if (MonoStateObject == null)
            {
                MonoStateObject = this;
                _SynchroForm = new SynchroForm(MonoStateObject);

                _Solution = new Solution(VSStudio.Solution);
            }



        }

        /// <MetaDataID>{ff233a7f-a896-4ce9-a117-3e1d9107a51b}</MetaDataID>
        static System.Collections.Generic.Dictionary<string, IDEManager> IDEManagers = new System.Collections.Generic.Dictionary<string, IDEManager>();

        /// <MetaDataID>{c4985ba7-fc2c-4b29-a6a7-6175ea05468b}</MetaDataID>
        static public IDEManager GetIDEManager(EnvDTE.DTE dte)
        {
            string IDEProcessID = MsdevManager.Msdev.GetIDEProcessID(dte);

            IDEManager ideManager = null;
            if (IDEManagers.ContainsKey(IDEProcessID))
                ideManager = IDEManagers[IDEProcessID];
            if (ideManager != null)
            {
                try
                {
                    ideManager.GetProject("");
                    return ideManager;
                }
                catch (Exception error)
                {
                    ideManager = null;
                }
            }


            string channelUri = "ipc://PID" + MsdevManager.Msdev.GetIDEProcessID(dte).ToString();
            if (ideManager == null)
            {
                ideManager = OOAdvantech.Remoting.RemotingServices.CreateRemoteInstance(channelUri, typeof(IDEManager).FullName, typeof(IDEManager).Assembly.FullName) as IDEManager;
                IDEManagers[IDEProcessID] = ideManager;
            }
            return IDEManagers[IDEProcessID];

        }
        /// <MetaDataID>{ea19a072-be35-4e94-84fa-cee62924323d}</MetaDataID>
        static Solution _Solution = null;
        /// <MetaDataID>{bf029a72-8c81-4bd2-a3ec-9c6cd6f84c15}</MetaDataID>
        public Solution Solution
        {
            get
            {
                if (MonoStateObject != this)
                    return MonoStateObject.Solution;
                if (VSStudio == null || VSStudio.Solution == null)
                    return null;
                if (_Solution != null && VSStudio != null && VSStudio.Solution != _Solution.VSSolution)
                    _Solution = null;

                if (_Solution != null)
                    return _Solution;
                //   _Solution = MetaObjectMapper.FindMetaObjectFor(VSStudio.Solution) as Solution;
                if (_Solution == null)
                    _Solution = new Solution(VSStudio.Solution);


                return _Solution;
            }
        }
        /// <MetaDataID>{e8ddf88b-9c25-4e84-88e8-787672019dc0}</MetaDataID>
        public void StartSynchronize(Project activeProject)
        {
            //OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.Clear();
            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
        }
        /// <MetaDataID>{070f8ad2-178e-45c0-95ca-460b75f259f8}</MetaDataID>
        public static void BrowseOnMetaObjectCode(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            if (metaObject is CodeElementContainer)
            {
                try
                {
                    EnvDTE.CodeElement codeElement = (metaObject as CodeElementContainer).CodeElement;

                    codeElement.DTE.ItemOperations.OpenFile(codeElement.StartPoint.Parent.Parent.FullName, "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}");
                    codeElement.StartPoint.Parent.Parent.Activate();

                    (codeElement.StartPoint.Parent as EnvDTE.TextDocument).Selection.EndOfDocument(false);
                    (codeElement.StartPoint.Parent as EnvDTE.TextDocument).Selection.MoveTo(codeElement.StartPoint.Line, codeElement.StartPoint.LineCharOffset, false);





                }
                catch (System.Exception error)
                {

                }

            }
        }

        /// <MetaDataID>{cdd43cfd-384d-4a10-be25-1b386d54644a}</MetaDataID>
        //public void Browse(string projectID, string residentIdentity, string memberIdentity)
        //{
        //    foreach (Project project in Solution.Projects)
        //    {
        //        if (project.Identity.ToString() == projectID)
        //        {
        //            foreach (MetaDataRepository.Classifier classifier in project.Residents)
        //            {
        //                if (classifier.Identity.ToString() == residentIdentity)
        //                {
        //                    foreach (MetaDataRepository.MetaObject metaObject in classifier.Features)
        //                    {
        //                        if (metaObject.Identity.ToString() == memberIdentity)
        //                        {

        //                            if (metaObject is CodeElementContainer)
        //                            {

        //                                //(metaObject as CodeElementContainer).CodeElement.ProjectItem.Document.s
        //                                //.DTE.Solution.Open((metaObject as CodeElementContainer).CodeElement.ProjectItem.

        //                            }
        //                            break;
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }

        //}

        ///// <MetaDataID>{ab97d8df-fd2c-434e-a04c-b7ce845abba6}</MetaDataID>
        //public void RefreshProject(string projectID, string residentIdentity)
        //{
        //    if (MonoStateObject != this)
        //    {
        //        SynchroForm.RefreshProject(projectID, residentIdentity);
        //        return;

        //    }
        //    foreach (Project project in Solution.Projects)
        //    {
        //        if (project.Identity.ToString() == projectID)
        //        {

        //            // project.RebuildMetadata();




        //            //project.Refresh();

        //            foreach (MetaDataRepository.Classifier classifier in project.Residents)
        //            {
        //                if (classifier.Identity.ToString() == residentIdentity)
        //                {
        //                    if (classifier is Interface)
        //                        (classifier as Interface).Refresh();
        //                    if (classifier is Class)
        //                        (classifier as Class).Refresh();
        //                }
        //            }
        //        }
        //    }

        //}
        ///// <MetaDataID>{d88898ec-6666-4243-a647-533817008ff2}</MetaDataID>
        //public void RefreshProject(string projectID)
        //{
        //    if (MonoStateObject != this)
        //    {
        //        SynchroForm.RefreshProject(projectID, null);
        //        return;

        //    }
        //    foreach (Project project in Solution.Projects)
        //    {
        //        if (project.Identity.ToString() == projectID)
        //        {
        //            project.RebuildMetadata();


        //            foreach (MetaDataRepository.Classifier classifier in project.Residents)
        //            {
        //                if (classifier is Interface)
        //                    (classifier as Interface).Refresh();
        //                if (classifier is Class)
        //                    (classifier as Class).Refresh();
        //            }
        //        }
        //    }
        //}
        /// <MetaDataID>{d3b42753-aee2-404c-8f03-9b751a715c5f}</MetaDataID>
        public Project GetProject(string projectID)
        {
            if (MonoStateObject != this)
                return SynchroForm.GetProject(projectID);


            foreach (Project project in Solution.Projects)
            {
                if (project.Identity.ToString() == projectID)
                    return project;
            }
            return null;
        }

        /// <MetaDataID>{3e445e6b-7710-4c33-a7b0-23fc860e4701}</MetaDataID>
        public void GetProjectResident(string identity, string projectID, out string componentIdentity, out string projectItemName)
        {
            if (MonoStateObject != this)
            {
                SynchroForm.GetProjectResident(identity, projectID, out componentIdentity, out projectItemName);
                return;

            }
            componentIdentity = null;
            projectItemName = null;

            foreach (Project project in Solution.Projects)
            {
                if (project.Identity.ToString() == projectID)
                {
                    foreach (MetaDataRepository.MetaObject metaObject in project.Residents)
                    {
                        if ((metaObject as CodeElementContainer).Identity.ToString() == identity)
                        {
                            if (metaObject is MetaDataRepository.Classifier)
                            {

                                if ((metaObject as MetaDataRepository.Classifier).ImplementationUnit is Project)
                                {
                                    projectItemName = GetProjectItemFullPath(((metaObject as MetaDataRepository.Classifier) as CodeElementContainer).CodeElement.ProjectItem.ContainingProject.ProjectItems, ((metaObject as MetaDataRepository.Classifier) as CodeElementContainer).CodeElement.ProjectItem);
                                    componentIdentity = (metaObject as MetaDataRepository.Classifier).ImplementationUnit.Identity.ToString();

                                }
                                else
                                {
                                    componentIdentity = (metaObject as MetaDataRepository.Classifier).ImplementationUnit.Identity.ToString();

                                }


                            }
                            return;
                        }
                    }
                }
            }



        }
        /// <MetaDataID>{2a99888b-dd67-4903-9723-6e3c926a3a1c}</MetaDataID>
        public void GetClassifierLocation(string fullName, string activeProjectID, out string componentIdentity, out string projectItemName)
        {
            if (MonoStateObject != this)
            {
                SynchroForm.GetClassifierLocation(fullName, activeProjectID, out componentIdentity, out projectItemName);
                return;

            }

            try
            {
                componentIdentity = null;
                projectItemName = null;

                foreach (Project project in Solution.Projects)
                {
                    if (project.Identity.ToString() == activeProjectID)
                    {
                        OOAdvantech.MetaDataRepository.Classifier classifier = project.GetClassifier(fullName, true);
                        if (classifier == null)
                            classifier = project.GetExternalClassifier(fullName);
                        if (classifier != null)
                        {
                            if (classifier.ImplementationUnit is Project)
                            {
                                if (classifier is Enumeration)
                                {
                                    return;
                                }
                                else
                                {
                                    if (classifier.OwnedTemplateSignature == null && classifier.TemplateBinding != null)
                                        return;
                                    projectItemName = GetProjectItemFullPath((classifier as CodeElementContainer).CodeElement.ProjectItem.ContainingProject.ProjectItems, (classifier as CodeElementContainer).CodeElement.ProjectItem);
                                }
                                componentIdentity = classifier.ImplementationUnit.Identity.ToString();
                            }
                            else
                            {

                                componentIdentity = classifier.ImplementationUnit.Identity.ToString();

                            }
                            return;
                        }

                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }




        /// <MetaDataID>{8ddb9c9e-a15e-4359-88f3-e27760d804f2}</MetaDataID>
        private string GetProjectItemFullPath(EnvDTE.ProjectItems projectItems, EnvDTE.ProjectItem projectItem)
        {


            if (projectItems.Count == 0)
                return null;
            foreach (EnvDTE.ProjectItem subProjectItem in projectItems)
            {
                if (subProjectItem == projectItem)
                    return projectItem.Name;
            }
            foreach (EnvDTE.ProjectItem subProjectItem in projectItems)
            {
                string path = GetProjectItemFullPath(subProjectItem.ProjectItems, projectItem);
                if (path != null)
                    return subProjectItem.Name + @"\" + path;
            }
            return null;

        }

        /// <MetaDataID>{ee28dab4-ea6f-4c27-be6d-bb5bc930de72}</MetaDataID>
        public void StopSynchronize()
        {

            OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = null;
            // OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.Clear();
        }

        /// <MetaDataID>{30ea82a5-73af-469b-9333-ae75984c2dbf}</MetaDataID>
        static IDEManager MonoStateObject ;
        /// <MetaDataID>{a5d4f1dc-8c9f-4e41-9666-562ab7d85c72}</MetaDataID>
        public static void Initialize(bool secure=false)
        {

            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);
            try
            {
                ModulePublisher.ClassRepository.Init();
                if (!OOAdvantech.Remoting.RemotingServices.IsIpcSeverChannelRegiter("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString()))
                {
                    if(secure)
                        OOAdvantech.Remoting.RemotingServices.RegisterSecureIpcChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), true);
                    else
                        OOAdvantech.Remoting.RemotingServices.RegisterAnonymousIpcChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), true);

                }
            }
            catch (System.Exception error)
            {
            }

            // System.Object @object= ModulePublisher.ClassRepository.CreateInstance("Parser.Parser", "");
            //_SynchroForm = new SynchroForm(MonoStateObject);
            //_SynchroForm.Show();
            //SolutionOpened();
            VisualStudioEventBridge.VisualStudioEvents.SolutionOpened += new VisualStudioEventBridge.SolutionOpenedEventHandler(SolutionOpened);
            VisualStudioEventBridge.VisualStudioEvents.SolutionClosed += new VisualStudioEventBridge.SolutionClosedEventHandler(SolutionClosed);
            string ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "DTERemoting.config";


        }

        /// <MetaDataID>{32d64e70-0237-45c3-8cc8-b8b35ce8a3e0}</MetaDataID>
        static EnvDTE._DTE VSStudio
        {
            get
            {

                //if (VisualStudioEventBridge.VisualStudioEvents.DTEObject == null)
                //    VisualStudioEventBridge.VisualStudioEvents.DTEObject = GetCurrentDTE();
                return VisualStudioEventBridge.VisualStudioEvents.DTEObject as EnvDTE._DTE;
            }
        }

        /// <MetaDataID>{44910011-7904-4f2f-8230-ccab42e0fe24}</MetaDataID>
        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);
        /// <MetaDataID>{aac1c476-6b66-4262-a797-97bcdee37992}</MetaDataID>
        [DllImport("ole32.dll")]
        private static extern void GetRunningObjectTable(int reserved,
                                                         out IRunningObjectTable prot);
        /// <MetaDataID>{95f8399f-7936-4765-8a4d-dc806f417623}</MetaDataID>
        public static EnvDTE.DTE GetCurrentDTE()
        {
            //rot entry for visual studio running under current process.
            string vs2005RotEntry = String.Format("!VisualStudio.DTE.8.0:{0}",
                                             Process.GetCurrentProcess().Id);
            string vs2008RotEntry = String.Format("!VisualStudio.DTE.9.0:{0}",
                                             Process.GetCurrentProcess().Id);
            string vs2010RotEntry = String.Format("!VisualStudio.DTE.10.0:{0}",
                                 Process.GetCurrentProcess().Id);
            string vs2015RotEntry = String.Format("!VisualStudio.DTE.14.0:{0}",
                                 Process.GetCurrentProcess().Id);


            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);
            IEnumMoniker enumMoniker;
            rot.EnumRunning(out enumMoniker);
            enumMoniker.Reset();
            IntPtr fetched = IntPtr.Zero;
            IMoniker[] moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                CreateBindCtx(0, out bindCtx);
                string displayName;
                moniker[0].GetDisplayName(bindCtx, null, out displayName);
                if (displayName == vs2005RotEntry || displayName == vs2008RotEntry || displayName == vs2010RotEntry || displayName == vs2015RotEntry)
                {
                    object comObject;
                    rot.GetObject(moniker[0], out comObject);
                    return (EnvDTE.DTE)comObject;
                }
            }
            return null;
        }
        static SynchroForm _SynchroForm;
        /// <MetaDataID>{55077f62-1e08-4e95-9397-81cca98bbc75}</MetaDataID>
        public static SynchroForm SynchroForm
        {
            get
            {
                return _SynchroForm;
            }
        }

        
        /// <MetaDataID>{0f10e0cb-4558-44c6-af05-b0cc93486fd9}</MetaDataID>
        static IDEManager()
        {

               //if (!System.IO.File.Exists(ConfigFileName))
            //{
            //    System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
            //    XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
            //    XmlDocument.Save(ConfigFileName);
            //}
            //System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName, false);


        }
        //static System.Collections.Generic.Dictionary<string, OOAdvantech.CodeMetaDataRepository.Project> SolutionProjects = new Dictionary<string, OOAdvantech.CodeMetaDataRepository.Project>();
        /// <MetaDataID>{ab477893-179b-492e-a77a-0ebce0df1959}</MetaDataID>
        static void SolutionOpened()
        {
            try
            {

                if (_Solution != null && VSStudio != null && VSStudio.Solution == _Solution.VSSolution)
                {
                    int projectCount = _Solution.Projects.Count;
                    return;
                }
                _Solution = null;
                MetaObjectMapper.Clear();



                IDEManager IDEManager = new IDEManager();
                if (IDEManager.Solution != null && IDEManager.Solution.Projects != null)
                {
                    int projectCount = IDEManager.Solution.Projects.Count;
                }

            }
            catch (Exception error)
            {


            }

        }
        /// <MetaDataID>{44f482bd-9185-4f43-8e06-514d376a7fad}</MetaDataID>
        static void SolutionClosed()
        {
            _Solution = null;

        }
        /// <MetaDataID>{82400285-587d-431c-93d0-902b2683c79e}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Component GetActiveWindowProject()
        {
            try
            {

                return OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(VSStudio.ActiveDocument.ProjectItem.ContainingProject) as OOAdvantech.CodeMetaDataRepository.Project;
            }
            catch (System.Exception error)
            {
            }
            return null;
        }

        /// <MetaDataID>{2cff4916-c97e-4b60-985f-30b58d2d4985}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Component GetSelectedItemsProject()
        {
            try
            {

                return OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(VSStudio.SelectedItems.Item(1).ProjectItem.ContainingProject) as OOAdvantech.CodeMetaDataRepository.Project;
            }
            catch (System.Exception error)
            {
            }
            return null;
        }

        /// <MetaDataID>{1a14d153-4081-42ca-bf32-43aeb11b1c54}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName, string assemblyName, bool caseSensitive, System.ComponentModel.Component component)
        {
            try
            {
                int projectCount = MonoStateObject.Solution.Projects.Count;
                EnvDTE.Project componentProject = null;
                if (component != null &&
                    component.Site != null &&
                    component.Site.GetService(typeof(System.CodeDom.CodeTypeDeclaration)) is System.CodeDom.CodeTypeDeclaration &&
                    (component.Site.GetService(typeof(System.CodeDom.CodeTypeDeclaration)) as System.CodeDom.CodeTypeDeclaration).Members.Count > 0)
                {

                    foreach (System.CodeDom.CodeTypeMember codeTypeMember in (component.Site.GetService(typeof(System.CodeDom.CodeTypeDeclaration)) as System.CodeDom.CodeTypeDeclaration).Members)
                    {
                        if (codeTypeMember.LinePragma != null && System.IO.File.Exists(codeTypeMember.LinePragma.FileName))
                        {
                            foreach (EnvDTE.Project project in VSStudio.Solution.Projects)
                            {
                                try
                                {
                                    if (project.ProjectItems == null)
                                        continue;
                                    foreach (EnvDTE.ProjectItem projectItem in project.ProjectItems)
                                    {
                                        try
                                        {
                                            for (short i = 0; i != projectItem.FileCount; i++)
                                            {
                                                if (projectItem.get_FileNames(i) == codeTypeMember.LinePragma.FileName)
                                                {
                                                    componentProject = project;
                                                    break;
                                                }
                                            }
                                            if (componentProject != null)
                                                break;
                                        }
                                        catch (System.Exception error)
                                        {
                                        }
                                    }


                                }
                                catch (Exception error)
                                {
                                }
                                if (componentProject != null)
                                    break;

                            }

                        }
                        if (componentProject != null)
                            break;

                    }
                }
                if (componentProject != null)
                {
                    OOAdvantech.CodeMetaDataRepository.Project project = OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(componentProject) as OOAdvantech.CodeMetaDataRepository.Project;
                    if (project != null)
                        return project.GetClassifierFromProjectAndRefs(fullName, assemblyName, caseSensitive);
                }
                return (GetActiveWindowProject() as OOAdvantech.CodeMetaDataRepository.Project).GetClassifierFromProjectAndRefs(fullName, assemblyName, caseSensitive);
            }
            catch (System.Exception error)
            {
                if (GetSelectedItemsProject() is OOAdvantech.CodeMetaDataRepository.Project)
                    return (GetSelectedItemsProject() as OOAdvantech.CodeMetaDataRepository.Project).GetClassifierFromProjectAndRefs(fullName, assemblyName, caseSensitive);

                if (VSStudio != null)
                {
                    IDEManager ideManager = GetIDEManager(VSStudio);

                    if (ideManager != null && ideManager.Solution != null)
                    {
                        foreach (var project in ideManager.Solution.Projects)
                        {
                            var classifier = project.GetClassifierFromProjectAndRefs(fullName, assemblyName, caseSensitive);
                            if (classifier != null)
                                return classifier;
                        }
                    }
                }
                return null;
            }
        }




        /// <MetaDataID>{cd28ea35-329b-48ae-be9a-71c13f02f8e8}</MetaDataID>
        public void StartCodeModelSynchronize()
        {
            //MetaObjectMapper.Clear();
            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.SuspendSychronizationDelete = true;
            //(OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as OOAdvantech.CodeMetaDataRepository.MetaObjectsStack).StartSynchronize();
            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

        }

        /// <MetaDataID>{3d2c0f79-2f03-4b79-b3f5-a19233ab0638}</MetaDataID>
        public void EndCodeModelSynchronize()
        {
            if (OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator != null)
            {
                OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.SuspendSychronizationDelete = false;
                OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = null;
                // (OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as OOAdvantech.CodeMetaDataRepository.MetaObjectsStack).StopSynchronize();
                //MetaObjectMapper.Clear();
                OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
            }
        }
    }
}
