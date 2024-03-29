using System;

using EnvDTE;


using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Linq;

using OOAdvantech.Transactions;
using System.Windows;

namespace Microneme.OOAppDevToolBox
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class="IDTExtensibility2"/>
    /// <MetaDataID>{d0cee301-8e40-4b38-bd17-9a818f4454ea}</MetaDataID>
    public class DTEConnection
    { 

        //ModulePublisherService.IModulePublisher _ModulePublisherService;
        //public ModulePublisherService.IModulePublisher ModulePublisherService
        //{ 
        //    get 
        //    {
        //        if (_ModulePublisherService == null)
        //        {
        //            EndpointAddress address = new EndpointAddress("http://localhost:4321/ModulePublisherService");
        //            WSHttpBinding binding = new WSHttpBinding();
        //            _ModulePublisherService = new ModulePublisherService.ModulePublisherClient(binding, address);
        //        }
        //        return _ModulePublisherService;
        //    }
        //}
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public DTEConnection()
        {
        }
        DTE DTEObject;
        EnvDTE.AddIn addInInstance;
        EnvDTE.DTE applicationObject;
        Package VSPackage;

        EnvDTE.BuildEvents BuildEvents;
        EnvDTE.SolutionEvents SolutionEvents;
        EnvDTE80.TextDocumentKeyPressEvents TextDocumentKeyPressEvents;
        EnvDTE80.CodeModelEvents CodeModelEvents;
        TaskListEvents TaskListEvents;
        CommandEvents CommandEvents;
        CommandBarEvents oSubMenuItemHandler;
        CommandBarEvents oUMLMenuItemHandler;
        TextEditorEvents TextEditorEvents;
        EnvDTE.ProjectItemsEvents ProjectItemsEvents;
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, Package package)
        {
            _applicationObject = (EnvDTE80.DTE2)application;
            VSPackage = package;


            DTEObject = application as DTE;

            try
            {
                var solution = DTEObject.Solution;

            }
            catch (Exception)
            {


            }
            VisualStudioEventBridge.VisualStudioEvents.DTEObject = DTEObject;
            m_objDTE = DTEObject;

            ////bool rer = m_objDTE!=null;

            ////if (rer)
            ////    return;
            ///


            applicationObject = application as EnvDTE.DTE;
            BuildEvents = applicationObject.Events.BuildEvents;
            BuildEvents.OnBuildBegin += new _dispBuildEvents_OnBuildBeginEventHandler(BuildEvents_OnBuildBegin);
            BuildEvents.OnBuildDone += new _dispBuildEvents_OnBuildDoneEventHandler(BuildEvents_OnBuildDone);
            BuildEvents.OnBuildProjConfigDone += new _dispBuildEvents_OnBuildProjConfigDoneEventHandler(BuildEvents_OnBuildProjConfigDone);
            SolutionEvents = applicationObject.Events.SolutionEvents;
            SolutionEvents.BeforeClosing += new _dispSolutionEvents_BeforeClosingEventHandler(SolutionEvents_BeforeClosing);
            SolutionEvents.ProjectRemoved += new _dispSolutionEvents_ProjectRemovedEventHandler(SolutionEvents_ProjectRemoved);
            SolutionEvents.Opened += new _dispSolutionEvents_OpenedEventHandler(SolutionEvents_Opened);
            CodeModelEvents = (applicationObject.Events as EnvDTE80.Events2).CodeModelEvents;
            CodeModelEvents.ElementAdded += new EnvDTE80._dispCodeModelEvents_ElementAddedEventHandler(CodeModelEvents_ElementAdded);
            CodeModelEvents.ElementChanged += new EnvDTE80._dispCodeModelEvents_ElementChangedEventHandler(CodeModelEvents_ElementChanged);
            CodeModelEvents.ElementDeleted += new EnvDTE80._dispCodeModelEvents_ElementDeletedEventHandler(CodeModelEvents_ElementDeleted);
            try
            {
                TaskListEvents = applicationObject.Events.TaskListEvents;
                TaskListEvents.TaskNavigated += new _dispTaskListEvents_TaskNavigatedEventHandler(TaskListEvents_TaskNavigated);

            }
            catch (Exception error)
            {

            }
            CommandEvents = applicationObject.Events.CommandEvents;
            CommandEvents.AfterExecute += new _dispCommandEvents_AfterExecuteEventHandler(CommandEvents_AfterExecute);

            TextEditorEvents = applicationObject.Events.TextEditorEvents;
            TextEditorEvents.LineChanged += new _dispTextEditorEvents_LineChangedEventHandler(TextEditorEvents_LineChanged);
            ProjectItemsEvents = (applicationObject.Events as EnvDTE80.Events2).ProjectItemsEvents;
            ProjectItemsEvents.ItemAdded += new _dispProjectItemsEvents_ItemAddedEventHandler(ProjectItemsEvents_ItemAdded);
            ProjectItemsEvents.ItemRemoved += new _dispProjectItemsEvents_ItemRemovedEventHandler(ProjectItemsEvents_ItemRemoved);
            ProjectItemsEvents.ItemRenamed += new _dispProjectItemsEvents_ItemRenamedEventHandler(ProjectItemsEvents_ItemRenamed);
            TextDocumentKeyPressEvents = (applicationObject.Events as EnvDTE80.Events2).TextDocumentKeyPressEvents;
            TextDocumentKeyPressEvents.AfterKeyPress += new EnvDTE80._dispTextDocumentKeyPressEvents_AfterKeyPressEventHandler(TextDocumentKeyPressEvents_AfterKeyPress);
            TextDocumentKeyPressEvents.BeforeKeyPress += new EnvDTE80._dispTextDocumentKeyPressEvents_BeforeKeyPressEventHandler(TextDocumentKeyPressEvents_BeforeKeyPress);

            try
            {
                ModulePublisher.ClassRepository.Init();
                System.Reflection.Assembly codeMetaDataRepositoryAssembly;
                //codeMetaDataRepositoryAssembly = System.Reflection.Assembly.Load("CodeMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=9ce9f0a461f2c1a5");
                //System.Type IDEManagerType;
                //IDEManagerType = codeMetaDataRepositoryAssembly.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager");
                //IDEManagerType.GetMethod("Initialize").Invoke(null, null);
                OOAdvantech.CodeMetaDataRepository.IDEManager.Initialize();
            }
            catch (Exception ex)
            {
            }
            InitializeErrorListProvider();
            try
            {
                Microsoft.VisualStudio.CommandBars.CommandBarControl pCommandBarControl = ((application as DTE).CommandBars as Microsoft.VisualStudio.CommandBars.CommandBars)["Code Window"].Controls.Add(Microsoft.VisualStudio.CommandBars.MsoControlType.msoControlButton, 1, null, 30, true);
                pCommandBarControl.Visible = true;
                pCommandBarControl.Caption = "Browse Model";
                oSubMenuItemHandler = (_applicationObject as _DTE).Events.get_CommandBarEvents(pCommandBarControl) as CommandBarEvents;
                oSubMenuItemHandler.Click += new _dispCommandBarControlEvents_ClickEventHandler(m_pCommandBarEvents_Click);
            }
            catch (Exception error)
            {
            }
        }


        void m_pCommandBarEvents_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
        {

            try
            {
                TextSelection textSelection = _applicationObject.ActiveDocument.Selection as TextSelection;
                if (textSelection != null)
                {
                    CodeElement codeClassifier = textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementClass);
                    if (codeClassifier == null)
                        codeClassifier = textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementInterface);
                    if (codeClassifier == null)
                        return;
                    OOAdvantech.CodeMetaDataRepository.Project project = OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(codeClassifier.ProjectItem.ContainingProject) as OOAdvantech.CodeMetaDataRepository.Project;

                    if (project == null)
                        return;
                    string classifierIdentity = null;
                    string memberIdentity = null;
                    string projectFileName = codeClassifier.ProjectItem.ContainingProject.FullName;

                    foreach (OOAdvantech.MetaDataRepository.MetaObject resident in project.Residents)
                    {
                        if (resident is OOAdvantech.CodeMetaDataRepository.CodeElementContainer && (resident as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement == codeClassifier)
                        {
                            OOAdvantech.MetaDataRepository.Classifier classifier = resident as OOAdvantech.MetaDataRepository.Classifier;
                            classifierIdentity = classifier.Identity.ToString();
                            if (classifier is OOAdvantech.CodeMetaDataRepository.CodeElementContainer)
                                classifierIdentity = (classifier as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).Identity.ToString();

                            if (textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementProperty) != null)
                            {
                                CodeElement selectedCodeElement = textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementProperty);
                                foreach (OOAdvantech.CodeMetaDataRepository.CodeElementContainer codeElementContainer in classifier.GetAssociateRoles(false))
                                {
                                    if (codeElementContainer.CodeElement == selectedCodeElement)
                                    {

                                        memberIdentity = codeElementContainer.Identity.ToString();// (codeElementContainer as OOAdvantech.MetaDataRepository.AssociationEnd).Association.Identity.ToString();
                                        if (codeElementContainer is OOAdvantech.MetaDataRepository.AssociationEnd && (codeElementContainer as OOAdvantech.MetaDataRepository.AssociationEnd).Association != null)
                                            memberIdentity = (codeElementContainer as OOAdvantech.MetaDataRepository.AssociationEnd).Association.Identity.ToString();
                                        break;
                                    }
                                }
                                if (memberIdentity == null)
                                {
                                    foreach (OOAdvantech.CodeMetaDataRepository.CodeElementContainer codeElementContainer in classifier.GetAttributes(false))
                                    {
                                        if (codeElementContainer.CodeElement == selectedCodeElement)
                                        {
                                            memberIdentity = codeElementContainer.Identity.ToString();// (codeElementContainer as OOAdvantech.MetaDataRepository.MetaObject).Identity.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                            if (memberIdentity == null && textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementVariable) != null)
                            {
                                CodeElement selectedCodeElement = textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementVariable);
                                foreach (OOAdvantech.CodeMetaDataRepository.CodeElementContainer codeElementContainer in classifier.GetAssociateRoles(false))
                                {
                                    if (codeElementContainer.CodeElement == selectedCodeElement)
                                    {
                                        memberIdentity = codeElementContainer.Identity.ToString();// (codeElementContainer as OOAdvantech.MetaDataRepository.AssociationEnd).Association.Identity.ToString();
                                        break;
                                    }
                                }
                                if (memberIdentity == null)
                                {
                                    foreach (OOAdvantech.CodeMetaDataRepository.CodeElementContainer codeElementContainer in classifier.GetAttributes(false))
                                    {
                                        if (codeElementContainer.CodeElement == selectedCodeElement)
                                        {
                                            memberIdentity = codeElementContainer.Identity.ToString();// (codeElementContainer as OOAdvantech.MetaDataRepository.MetaObject).Identity.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                            if (memberIdentity == null && textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementFunction) != null)
                            {
                                CodeElement selectedCodeElement = textSelection.TopPoint.get_CodeElement(vsCMElement.vsCMElementFunction);
                                foreach (OOAdvantech.CodeMetaDataRepository.CodeElementContainer codeElementContainer in classifier.GetOperations(false))
                                {
                                    if (codeElementContainer.CodeElement == selectedCodeElement)
                                    {
                                        memberIdentity = codeElementContainer.Identity.ToString();// (codeElementContainer as OOAdvantech.MetaDataRepository.MetaObject).Identity.ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (classifierIdentity == null || projectFileName == null)
                        return;
                    IWshRuntimeLibrary.WshShell winShell = new IWshRuntimeLibrary.WshShell();
                    var roseProcess = System.Diagnostics.Process.GetProcesses().Where(p => p.ProcessName.ToLower() == "rose").FirstOrDefault();
                    if (roseProcess != null)
                    {
                        RoseAddinFacade.IRoseAddin addin = null;
                        string proccesID = roseProcess.Id.ToString();
                        string channelUri = "ipc://PID" + proccesID;
                        addin = OOAdvantech.Remoting.RemotingServices.CreateRemoteInstance(channelUri, "RoseMetaDataRepository.VSCodeElementsSynchronizer", "") as RoseAddinFacade.IRoseAddin;
                        if (addin != null)
                        {
                            addin.BrowseRoseItem(projectFileName, classifierIdentity, memberIdentity);
                            object wait = null;
                            object MainWindowTitle = addin.MainWindowTitle;
                            winShell.AppActivate(ref MainWindowTitle, ref wait);
                        }
                    }

                    //int numFetched;
                    //UCOMIRunningObjectTable runningObjectTable;
                    //UCOMIEnumMoniker monikerEnumerator;
                    //UCOMIMoniker[] monikers = new UCOMIMoniker[1];


                    //MsdevManager.Msdev.GetRunningObjectTable(0, out runningObjectTable);



                    //runningObjectTable.EnumRunning(out monikerEnumerator);
                    //monikerEnumerator.Reset();


                    //while (monikerEnumerator.Next(1, monikers, out numFetched) == 0)
                    //{
                    //    try
                    //    {

                    //        UCOMIBindCtx ctx;
                    //        MsdevManager.Msdev.CreateBindCtx(0, out ctx);

                    //        string runningObjectName;
                    //        monikers[0].GetDisplayName(ctx, null, out runningObjectName);
                    //        Guid clsid;
                    //        //if(runningObjectName!=null)
                    //        //    System.Windows.Forms.MessageBox.Show(runningObjectName);


                    //        object runningObjectVal = null;
                    //        runningObjectTable.GetObject(monikers[0], out runningObjectVal);


                    //        if (!(runningObjectVal is EnvDTE.DTE))
                    //        {

                    //            RoseAddinFacade.IRoseAddin addin = null;
                    //            //\runningObjectVal as RoseMetaDataRepository.IVSCodeElementsSynchronizer;
                    //            if (runningObjectName.IndexOf("RoseVisualStudioBridge:") == 0)
                    //            {

                    //                try
                    //                {
                    //                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    //                    {
                    //                        int tr = 0;
                    //                        Transaction.Current.Marshal();
                    //                        stateTransition.Consistent = true;
                    //                    }
                    //                    OOAdvantech.Transactions.TransactionRunTime.UnMarshal(Guid.NewGuid().ToString());
                    //                }
                    //                catch (Exception error)
                    //                {
                    //                }

                    //                //object _obj = OOAdvantech.Remoting.RemotingServices.CreateInstance("tcp://localhost:9051", "OOAdvantech.Transactions.TransactionCoordinator", "");

                    //                string proccesID = runningObjectName.Replace("RoseVisualStudioBridge:", "");
                    //                string channelUri = "ipc://PID" + proccesID;
                    //                addin = OOAdvantech.Remoting.RemotingServices.CreateInstance(channelUri, "RoseMetaDataRepository.VSCodeElementsSynchronizer", "") as RoseAddinFacade.IRoseAddin;
                    //            }

                    //            if (addin != null)
                    //            {
                    //                addin.BrowseRoseItem(projectFileName, classifierIdentity, memberIdentity);

                    //                object wait = null;
                    //                object MainWindowTitle = addin.MainWindowTitle;
                    //                winShell.AppActivate(ref MainWindowTitle, ref wait);


                    //                break;

                    //            }

                    //            Marshal.ReleaseComObject(runningObjectVal);
                    //        }


                    //    }
                    //    catch (System.Exception error)
                    //    {
                    //        //System.Windows.Forms.MessageBox.Show(error.Message);
                    //    }
                    //}
                }
            }
            catch (Exception error)
            {


            }



        }


        void SolutionEvents_Opened()
        {
            VisualStudioEventBridge.VisualStudioEvents.OnSolutionOpened();
        }

        void TextDocumentKeyPressEvents_BeforeKeyPress(string Keypress, TextSelection Selection, bool InStatementCompletion, ref bool CancelKeypress)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnBeforeKeyPress(Keypress, Selection, InStatementCompletion, ref CancelKeypress);
        }

        void TextDocumentKeyPressEvents_AfterKeyPress(string Keypress, TextSelection Selection, bool InStatementCompletion)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnAfterKeyPress(Keypress, Selection, InStatementCompletion);
        }

        void ProjectItemsEvents_ItemRenamed(ProjectItem ProjectItem, string OldName)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnProjectItemRenamed(ProjectItem, OldName);
        }

        void ProjectItemsEvents_ItemRemoved(ProjectItem ProjectItem)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnProjectItemRemoved(ProjectItem);
        }

        void ProjectItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnProjectItemAdded(ProjectItem);
        }

        void TextEditorEvents_LineChanged(TextPoint StartPoint, TextPoint EndPoint, int Hint)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnLineChanged(StartPoint, EndPoint, Hint);
        }

        void CommandEvents_AfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnAfterCommandExecute(Guid, ID, CustomIn, CustomOut);
        }

        void TaskListEvents_TaskNavigated(TaskItem TaskItem, ref bool NavigateHandled)
        {
            try
            {
                if (System.IO.File.Exists(TaskItem.FileName))
                {
                    applicationObject.ItemOperations.OpenFile(TaskItem.FileName);
                    EnvDTE.TextSelection selection;
                    selection = applicationObject.ActiveDocument.Selection as EnvDTE.TextSelection;
                    selection.GotoLine(TaskItem.Line);
                }
                else if (ErrorCodeElements.Contains(TaskItem.FileName))
                {
                    EnvDTE.CodeElement codeElement;
                    codeElement = ErrorCodeElements[TaskItem.FileName] as EnvDTE.CodeElement;
                    applicationObject.ItemOperations.OpenFile(codeElement.ProjectItem.get_FileNames(1) as string);
                    EnvDTE.TextSelection selection;
                    selection = applicationObject.ActiveDocument.Selection as EnvDTE.TextSelection;
                    selection.GotoLine(codeElement.StartPoint.Line);
                }

            }
            catch (Exception ex)
            {
            }

        }

        void CodeModelEvents_ElementDeleted(object Parent, CodeElement Element)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnCodeElementDeleted(Parent, Element);
        }

        void CodeModelEvents_ElementChanged(CodeElement Element, EnvDTE80.vsCMChangeKind Change)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnCodeElementChanged(Element, Change);
        }

        void CodeModelEvents_ElementAdded(CodeElement Element)
        {
            VisualStudioEventBridge.VisualStudioEvents.OnCodeElementAdded(Element);
        }

        void SolutionEvents_ProjectRemoved(EnvDTE.Project objProject)
        {

            EnvDTE.Project objErrorTaskProject = null;
            object objObject = null;
            try
            {
                if (m_colErrorTasks != null)
                {
                    foreach (ErrorTask objErrorTask in m_colErrorTasks)
                    {
                        objErrorTask.HierarchyItem.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out objObject);
                        if ((objObject.GetType() == typeof(EnvDTE.Project)))
                        {

                            if ((objErrorTaskProject.UniqueName == objProject.UniqueName))
                            {
                                this.RemoveTask(objErrorTask);
                            }

                        }

                    }

                }

            }
            catch (Exception objException)
            {
             System.Windows.Forms. MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        void SolutionEvents_BeforeClosing()
        {
            RemoveTasks();
        }

        private EnvDTE.CodeElement GetCodeElement(EnvDTE.CodeElement codeElement, string subElement)
        {
            EnvDTE.CodeNamespace codeNameSpace = null;
            EnvDTE.CodeClass codeClass = null;
            if ((codeElement.Kind == vsCMElement.vsCMElementNamespace))
            {
                codeNameSpace = codeElement as EnvDTE.CodeNamespace;
            }

            if (((codeElement.Kind == vsCMElement.vsCMElementClass)
                        || (codeElement.Kind == vsCMElement.vsCMElementInterface)))
            {
                codeClass = codeElement as EnvDTE.CodeClass;
            }

            if (!(codeNameSpace == null))
            {

                foreach (CodeElement member in codeNameSpace.Members)
                {
                    string memberName;
                    if ((member.Name == subElement))
                    {
                        return member;
                    }

                }

            }

            if (!(codeClass == null))
            {

                foreach (CodeElement member in codeClass.Members)
                {
                    string memberName;
                    if ((member.Name == subElement))
                    {
                        return member;
                    }

                }

            }

            return null;
        }

        private EnvDTE.CodeElement GetCodeElement(EnvDTE.Project project, string subElement)
        {

            foreach (EnvDTE.CodeElement subCodeElement in project.CodeModel.CodeElements)
            {
                if ((subCodeElement.Name == subElement))
                {
                    return subCodeElement;
                }

            }

            return null;
        }


        private EnvDTE.CodeElement GetCodeElementFromPath(string path, EnvDTE.Project project)
        {
            if (!System.IO.File.Exists(path))
            {
                string FullName;
                EnvDTE.CodeElement codeElement = null;
                FullName = path;
                while (!(FullName.IndexOf(".") == -1))
                {
                    int pos;
                    pos = FullName.IndexOf(".");
                    string Name;
                    Name = FullName.Substring(0, pos);
                    FullName = FullName.Substring((pos + 1), (FullName.Length
                                    - (pos + 1)));
                    if ((codeElement == null))
                    {
                        codeElement = this.GetCodeElement(project, Name);
                    }
                    else
                    {
                        codeElement = this.GetCodeElement(codeElement, Name);
                    }

                }

                if ((codeElement == null))
                {
                    codeElement = this.GetCodeElement(project, FullName);
                }
                else
                {
                    codeElement = this.GetCodeElement(codeElement, FullName);
                }

                return codeElement;
            }

            return null;
        }

        object OnBuildTaskLock = new object();
        void BuildEvents_OnBuildProjConfigDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            if ((Success == false))
            {
                return;
            }

            var task = System.Threading.Tasks.Task.Run(() =>
             {
                 lock (OnBuildTaskLock)
                 {
                     OnBuildDone(Project);
                 }
             });


        }


        private void OnBuildDone(string Project)
        {
            if (BuildAction == vsBuildAction.vsBuildActionBuild || BuildAction == vsBuildAction.vsBuildActionRebuildAll)
            {



                System.IO.DirectoryInfo mDirectory = new System.IO.DirectoryInfo("c:\\\\");
                //EnvDTE.Project ProjectObj;
                string ProjectName;
                ProjectName = Project;
                int Index;
                Index = ProjectName.LastIndexOf(".");
                ProjectName = ProjectName.Substring(0, Index);
                Index = ProjectName.LastIndexOf("\\");
                if (Index != -1)
                {
                    ProjectName = ProjectName.Substring(Index + 1);
                }

                EnvDTE.Window mWindow = applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
                //  Create handles to the Output window and its panes.
                EnvDTE.OutputWindow mOutputWindow = mWindow.Object as EnvDTE.OutputWindow;
                EnvDTE.OutputWindowPane mOutputWindowPane = mOutputWindow.OutputWindowPanes.Item("Build");

                foreach (EnvDTE.Project ProjectObj in applicationObject.Solution.Projects)
                {
                    try
                    {
                        if ((ProjectObj.Name == ProjectName))
                        {


                            string projectFullPath = ProjectObj.Properties.Item("FullPath").Value as string;
                            string outputDLLName = projectFullPath + ProjectObj.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value as string;
                            outputDLLName = outputDLLName + ProjectObj.Properties.Item("OutputFileName").Value as string;
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(outputDLLName);
                            DateTime LastWriteTime = fileInfo.LastWriteTime;
                            string assemplyName = fileInfo.Name.Replace(fileInfo.Extension, "");

                            CreateNetCoreProxies(projectFullPath, outputDLLName);


                            TaskErrorCategory = TaskErrorCategory.Message;



                            Microsoft.Win32.RegistryKey registryKey = null;

                            try
                            {
                                registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(("Assembly\\\\"
                                                + (assemplyName + ("\\\\"
                                                + (System.Diagnostics.FileVersionInfo.GetVersionInfo(outputDLLName).FileVersion + "\\\\")))));
                                if (!(registryKey == null))
                                {
                                    DateTime fileDateTime = System.DateTime.Parse(registryKey.GetValue("Date") as string);
                                    registryKey.Close();
                                    if ((fileDateTime.ToString() == LastWriteTime.ToString()))
                                    {
                                        return;
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                            }

                            string Command;

                            Command = (Path + "ModulePublisherHostProcess.exe");
                            string assemblyFileName = outputDLLName;
                            outputDLLName = ("\""
                                        + (outputDLLName + "\""));
                            System.Diagnostics.Process process;
                            
                            
                            if(!System.IO.Directory.Exists( System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\OOAppDevToolBox"))
                                System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\OOAppDevToolBox");
                            string errorFile = (AppDomain.CurrentDomain.BaseDirectory + "error.txt");
                            errorFile =System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\OOAppDevToolBox\error.txt";
                            if (System.IO.File.Exists(errorFile))
                            {
                                System.IO.File.Delete(errorFile);
                            }
                            string referencePath = string.Format(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + @"ObjectOrientedAppDevToolBox\{0}.reference.txt", ProjectName);
                            try
                            {
                                if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + @"ObjectOrientedAppDevToolBox"))
                                    System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + @"ObjectOrientedAppDevToolBox");


                                var vsproject = ProjectObj.Object as VSLangProj.VSProject;
                                System.IO.File.WriteAllLines(referencePath, vsproject.References.OfType<VSLangProj.Reference>().Select(x => x.Path).ToArray());
                            }
                            catch (Exception error)
                            {


                            }

                            //EnvDTE.ProjectItem projectItem;
                            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                            System.Xml.XmlElement xmlElement;
                            xmlElement = xmlDocument.CreateElement("Project");
                            xmlDocument.AppendChild(xmlElement);
                            xmlElement.SetAttribute("AssemblyFile", outputDLLName);
                            foreach (EnvDTE.ProjectItem projectItem in ProjectObj.ProjectItems)
                            {
                                string LocalPath = "";
                                string SubType = "";
                                int buildAction = 0;
                                try
                                {

                                    Dictionary<string, Property> projectItemProperties = projectItem.Properties.OfType<Property>().ToDictionary(x => x.Name);
                                    if (projectItemProperties.ContainsKey("LocalPath"))
                                        LocalPath = projectItemProperties["LocalPath"].Value as string;
                                    if (projectItemProperties.ContainsKey("SubType"))
                                        SubType = projectItemProperties["SubType"].Value as string;
                                    if (projectItemProperties.ContainsKey("BuildAction"))
                                        buildAction = (int)projectItemProperties["BuildAction"].Value;
                                    if (((SubType == "Code")
                                                && (buildAction == 1)))
                                    {
                                        System.Xml.XmlElement xmlFileElement;
                                        xmlFileElement = xmlDocument.CreateElement("File");
                                        xmlElement.AppendChild(xmlFileElement);
                                        xmlFileElement.SetAttribute("LocalPath", LocalPath);
                                    }

                                }
                                catch (Exception ex)
                                {
                                }

                            }

                            xmlDocument.Save(errorFile);
                            outputDLLName += " /errorfile:";
                            outputDLLName = outputDLLName + "\"" + errorFile + "\"";

                            projectFullPath = projectFullPath.Trim();
                            if (projectFullPath[projectFullPath.Length - 1] == '\\')
                                projectFullPath = projectFullPath.Substring(0, projectFullPath.Length - 1);

                            outputDLLName += " /projectpath:\"" + projectFullPath + "\"";
                            mOutputWindowPane.OutputString(String.Format("\nPerforming registration: {0}", ProjectName));
                            process = System.Diagnostics.Process.Start(Command, outputDLLName);
                            //this.ModulePublisherService.Publish(assemblyFileName, false, errorFile);
                            //this.ModulePublisherService.ExecudeModulePublishCommand(Command, OutputDLLName);
                            long Count;
                            Count = 0;
                            while (!process.HasExited)
                            {
                                Count = (Count + 1);
                                System.Threading.Thread.Sleep(200);
                                if ((Count > 400))
                                {
                                    break;
                                }

                            }



                            if (System.IO.File.Exists(errorFile))
                            {

                            }

                            if (System.IO.File.Exists(errorFile))
                            {
                                System.Xml.XmlDocument ErrorDocument = new System.Xml.XmlDocument();
                                ErrorDocument.Load(errorFile);
                                if ((ErrorDocument.DocumentElement.Name == "Errors"))
                                {
                                    //System.Xml.XmlElement errorElement;
                                    int errorCount = 0;
                                    foreach (System.Xml.XmlElement errorElement in ErrorDocument.DocumentElement.ChildNodes)
                                    {
                                        const string vsWindowKindErrorList = "{D78612C7-9962-4B83-95D9-268046DAD23A}";

                                        EnvDTE.Window errorListwin = applicationObject.Windows.Item(vsWindowKindErrorList);
                                        EnvDTE.Window win = applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);
                                        TaskList TL = win.Object as TaskList;
                                        TaskItem TLItem;
                                        //  Add a couple of tasks to the Task List.
                                        string errorMessage = "";
                                        string fileName = "";
                                        int line = 0;
                                        errorMessage = errorElement.InnerText;
                                        EnvDTE.CodeElement mCodeElement = null; ;
                                        if (errorElement.HasAttribute("FileName"))
                                        {
                                            fileName = errorElement.GetAttribute("FileName");
                                            mCodeElement = this.GetCodeElementFromPath(fileName, ProjectObj);
                                            if (!(mCodeElement == null))
                                            {
                                                if (!ErrorCodeElements.Contains(fileName))
                                                    ErrorCodeElements.Add(fileName, mCodeElement);
                                            }
                                        }

                                        if (errorElement.HasAttribute("LineNumber"))
                                            line = int.Parse(errorElement.GetAttribute("LineNumber"));


                                        if (!(mCodeElement == null))
                                        {
                                            this.AddErrorToErrorList(ProjectObj, mCodeElement.ProjectItem, errorMessage, TaskErrorCategory, mCodeElement.StartPoint.Line, mCodeElement.StartPoint.LineCharOffset);
                                            errorCount = (errorCount + 1);
                                            if ((!(MaxErrorPerProject == 0)
                                                        && (MaxErrorPerProject <= errorCount)))
                                            {
                                                break;
                                            }

                                        }
                                        else
                                        {
                                            this.AddErrorToErrorList(ProjectObj, null, errorMessage, TaskErrorCategory, 0, 0);
                                            errorCount = (errorCount + 1);
                                            if ((!(MaxErrorPerProject == 0)
                                                        && (MaxErrorPerProject <= errorCount)))
                                            {
                                                break;
                                            }



                                        }

                                    }

                                }

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }
            }

        }

        private void CreateNetCoreProxies(string projectFullPath, string outputDLLName)
        {
            //try
            //{
            //    var assembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(outputDLLName);
            //    FacadeProxiesGenerator.ProxiesGenerator.GenerateProxies(assembly, FacadeProxiesGenerator.ProxiesOutput.CSharp, projectFullPath);
            //}
            //catch (Exception error)
            //{


            //}
        }

        private object GetService(System.Type serviceType)
        {
            object objService = null;
            try
            {
                objService = Microsoft.VisualStudio.Shell.Package.GetGlobalService(serviceType);
            }
            catch (Exception objException)
            {
                System.Windows.Forms.MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return objService;
        }

        private void AddErrorToErrorList(EnvDTE.Project objProject, ProjectItem objProjectItem, string sErrorText, TaskErrorCategory eTaskErrorCategory, int iLine, int iColumn)
        {



            lock (this)
            {
                try
                {
                    IVsHierarchy objVsHierarchy = null;
                    ErrorTask objErrorTask = new Microsoft.VisualStudio.Shell.ErrorTask();
                    objErrorTask.ErrorCategory = eTaskErrorCategory;
                    objErrorTask.HierarchyItem = objVsHierarchy;
                    if (objProjectItem != null)
                        objErrorTask.Document = objProjectItem.FileNames[0] as string;
                    //  VS uses indexes starting at 0 while the automation model uses indexes starting at 1
                    objErrorTask.Line = (iLine - 1);
                    objErrorTask.Column = iColumn;
                    objErrorTask.Text = sErrorText;
                    objErrorTask.Navigate += new EventHandler(objErrorTask_Navigate);
                    m_colErrorTasks.Add(objErrorTask);
                    m_objErrorListProvider.Tasks.Add(objErrorTask);
                    int ere = 933;
                }
                catch (Exception objException)
                {
                    //MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }


        void BuildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                lock (OnBuildTaskLock)
                {
                    try
                    {
                        EnvDTE.Window mWindow = applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
                        //  Create handles to the Output window and its panes.
                        EnvDTE.OutputWindow mOutputWindow = mWindow.Object as EnvDTE.OutputWindow;
                        EnvDTE.OutputWindowPane mOutputWindowPane = mOutputWindow.OutputWindowPanes.Item("Build");
                        mOutputWindowPane.OutputString("\n==========  Registration  Finished  ==========");
                    }
                    catch (Exception error)
                    {
                    }
                }
            });
            if (!(m_objErrorListProvider == null))
            {
                if ((SwitchToErrorList && (m_colErrorTasks.Count > 0)))
                {
                    m_objErrorListProvider.Show();
                }
            }

        }

        System.Collections.Hashtable ErrorCodeElements = new System.Collections.Hashtable();

        vsBuildAction BuildAction;
        int DisplayErrorAsVSError = 0;
        int DisplayErrorAsVSWarning = 0;
        int DisplayErrorAsVSMessage = 0;
        string Path = null;
        void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            BuildAction = Action;

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\OOAdvantech\\Object Oriented RAD Post Build");
            if (registryKey.GetValue("DisplayErrorAsVSError") is int)
                DisplayErrorAsVSError = (int)registryKey.GetValue("DisplayErrorAsVSError");
            if (registryKey.GetValue("DisplayErrorAsVSWarning") is int)
                DisplayErrorAsVSWarning = (int)registryKey.GetValue("DisplayErrorAsVSWarning");
            if (registryKey.GetValue("DisplayErrorAsVSMessage") is int)
                DisplayErrorAsVSMessage = (int)registryKey.GetValue("DisplayErrorAsVSMessage");
            if (DisplayErrorAsVSError == 1)
                TaskErrorCategory = TaskErrorCategory.Error;
            if (DisplayErrorAsVSWarning == 1)
                TaskErrorCategory = TaskErrorCategory.Warning;
            if (DisplayErrorAsVSMessage == 1)
                TaskErrorCategory = TaskErrorCategory.Message;
            if (registryKey.GetValue("MaxErrorPerProject") != null)
                MaxErrorPerProject = (int)registryKey.GetValue("MaxErrorPerProject");
            if (registryKey.GetValue("SwitchToErrorList") is int)
            {
                if (((int)(registryKey.GetValue("SwitchToErrorList")) == 1))
                    SwitchToErrorList = true;
                else
                    SwitchToErrorList = false;
            }
            registryKey.Close();

            registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\\\OOAdvantech\\\\Object Oriented RAD Post Build", false);
            if (!(registryKey == null))
            {
                Path = registryKey.GetValue("Net4Path") as string;
                registryKey.Close();
            }
            if (!System.IO.Directory.Exists(Path))
            {
                registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("CLSID\\\\{1A7637F7-A08B-4F0F-AE03-55F2D8E0D545}\\\\InprocServer32");
                if (!(registryKey == null))
                {
                    Path = registryKey.GetValue("CodeBase") as string;
                    registryKey.Close();
                    if (System.IO.File.Exists(Path))
                    {
                        System.IO.FileInfo fInfo = new System.IO.FileInfo(Path);
                        Path = (fInfo.Directory.FullName + "\\\\");
                    }
                }
            }
            System.Threading.Tasks.Task.Run(() =>
            {
                lock (OnBuildTaskLock)
                {
                    ClearErrorList();
                }
            });
        }

        private void ClearErrorList()
        {
            var win = applicationObject.Windows.Item("{4A9B7E51-AA16-11D0-A8C5-00A0C921A4D2}");// EnvDTE.Constants.vsWindowKindTaskList);
            TaskList TL = win.Object as TaskList;
            foreach (TaskItem taskItem in TL.TaskItems)
            {
                try
                {
                    taskItem.Delete();
                }
                catch (Exception ex)
                {
                }
            }
            ErrorCodeElements.Clear();
            RemoveTasks();
        }

        private void RemoveTasks()
        {

            try
            {
                if (!(m_colErrorTasks == null))
                {
                    foreach (ErrorTask objErrorTask in m_colErrorTasks.ToArray())
                    {

                        RemoveTask(objErrorTask);
                    }

                }

            }
            catch (Exception objException)
            {
                System.Windows.Forms.MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void RemoveTask(ErrorTask objErrorTask)
        {
            try
            {
                m_objErrorListProvider.SuspendRefresh();
                objErrorTask.Navigate -= new EventHandler(objErrorTask_Navigate);
                m_colErrorTasks.Remove(objErrorTask);
                m_objErrorListProvider.Tasks.Remove(objErrorTask);
            }
            catch (Exception objException)
            {
               System.Windows.Forms. MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_objErrorListProvider.ResumeRefresh();
            }

        }

        void objErrorTask_Navigate(object sender, EventArgs e)
        {
            ErrorTask objErrorTask;
            bool bResult;
            try
            {
                objErrorTask = ((ErrorTask)(sender));
                objErrorTask.Line++;
                //  Fix the index start
                bResult = m_objErrorListProvider.Navigate(objErrorTask, new Guid("{7651A701-06E5-11D1-8EBD-00A0C90F26EA}"));// EnvDTE.Constants.vsViewKindCode));
                objErrorTask.Line--;
                //  Restore the index start
                if (!bResult)
                {
                    System.Windows.Forms.MessageBox.Show(("Error navigating to " + objErrorTask.Text), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception objException)
            {
                System.Windows.Forms.MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void InitializeErrorListProvider()
        {
            //var obj = VSPackage.GetService(typeof(SVsSolution));
            m_colErrorTasks = new List<ErrorTask>();
            m_objErrorListProvider = new Microsoft.VisualStudio.Shell.ErrorListProvider(VSPackage as IServiceProvider);
            m_objErrorListProvider.ProviderName = "ObjectOrientedAppDevToolBox Error Provider";
            //  TODO: (IMPORTANT) Use your OWN Guid here: 

            m_objErrorListProvider.ProviderGuid = new Guid("5BA8BB0D-D517-45ae-966C-864C536454F1");
            //m_objErrorListProvider.Show();
        }
        DTE m_objDTE;
        ErrorListProvider m_objErrorListProvider;
        List<ErrorTask> m_colErrorTasks;

        TaskErrorCategory TaskErrorCategory;
        int MaxErrorPerProject;
        bool SwitchToErrorList;

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection()
        {
            applicationObject.Events.BuildEvents.OnBuildBegin -= new _dispBuildEvents_OnBuildBeginEventHandler(BuildEvents_OnBuildBegin);
            applicationObject.Events.BuildEvents.OnBuildDone -= new _dispBuildEvents_OnBuildDoneEventHandler(BuildEvents_OnBuildDone);
            applicationObject.Events.BuildEvents.OnBuildProjConfigDone -= new _dispBuildEvents_OnBuildProjConfigDoneEventHandler(BuildEvents_OnBuildProjConfigDone);
            applicationObject.Events.SolutionEvents.BeforeClosing -= new _dispSolutionEvents_BeforeClosingEventHandler(SolutionEvents_BeforeClosing);
            applicationObject.Events.SolutionEvents.ProjectRemoved -= new _dispSolutionEvents_ProjectRemovedEventHandler(SolutionEvents_ProjectRemoved);
            (applicationObject.Events as EnvDTE80.Events2).CodeModelEvents.ElementAdded -= new EnvDTE80._dispCodeModelEvents_ElementAddedEventHandler(CodeModelEvents_ElementAdded);
            (applicationObject.Events as EnvDTE80.Events2).CodeModelEvents.ElementChanged -= new EnvDTE80._dispCodeModelEvents_ElementChangedEventHandler(CodeModelEvents_ElementChanged);
            (applicationObject.Events as EnvDTE80.Events2).CodeModelEvents.ElementDeleted -= new EnvDTE80._dispCodeModelEvents_ElementDeletedEventHandler(CodeModelEvents_ElementDeleted);
            applicationObject.Events.TaskListEvents.TaskNavigated -= new _dispTaskListEvents_TaskNavigatedEventHandler(TaskListEvents_TaskNavigated);
            applicationObject.Events.CommandEvents.AfterExecute -= new _dispCommandEvents_AfterExecuteEventHandler(CommandEvents_AfterExecute);
            applicationObject.Events.TextEditorEvents.LineChanged -= new _dispTextEditorEvents_LineChangedEventHandler(TextEditorEvents_LineChanged);
            (applicationObject.Events as EnvDTE80.Events2).ProjectItemsEvents.ItemAdded -= new _dispProjectItemsEvents_ItemAddedEventHandler(ProjectItemsEvents_ItemAdded);
            (applicationObject.Events as EnvDTE80.Events2).ProjectItemsEvents.ItemRemoved -= new _dispProjectItemsEvents_ItemRemovedEventHandler(ProjectItemsEvents_ItemRemoved);
            (applicationObject.Events as EnvDTE80.Events2).ProjectItemsEvents.ItemRenamed -= new _dispProjectItemsEvents_ItemRenamedEventHandler(ProjectItemsEvents_ItemRenamed);
            (applicationObject.Events as EnvDTE80.Events2).TextDocumentKeyPressEvents.AfterKeyPress -= new EnvDTE80._dispTextDocumentKeyPressEvents_AfterKeyPressEventHandler(TextDocumentKeyPressEvents_AfterKeyPress);
            (applicationObject.Events as EnvDTE80.Events2).TextDocumentKeyPressEvents.BeforeKeyPress -= new EnvDTE80._dispTextDocumentKeyPressEvents_BeforeKeyPressEventHandler(TextDocumentKeyPressEvents_BeforeKeyPress);

        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>


        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        private EnvDTE80.DTE2 _applicationObject;


        public EnvDTE.SolutionEvents m_objSolutionEvents { get; set; }
    }
}