using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using OOAdvantech.Transactions;
using OOAdvantech.Remoting;

namespace RoseMetaDataRepository
{




    /// <MetaDataID>{416c2c20-b5a5-4a9d-8c70-554c6f89894f}</MetaDataID>
    [Guid("5E6D9E39-596A-4736-8463-E1629551841B")]
    [ComVisible(true)]
    public interface IVSCodeElementsSynchronizer
    {
        /// <MetaDataID>{62c9a800-fac5-4e97-8cc7-714c435b9732}</MetaDataID>
        void BrowseRoseItem(string componentIdentity, string classifierIdentity, string memberIdentity);

        /// <MetaDataID>{91c47a59-b4b1-47ac-a609-1b05b981aeed}</MetaDataID>
        string MainWindowTitle
        {
            get;
        }
    }



#if Net4


    /// <MetaDataID>{A574A68E-EB3B-4EC7-A787-68DBC874937E}</MetaDataID>
    [Guid("749BAE0B-4A62-4816-9B73-FCB07026CD60")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class VSCodeElementsSynchronizer : MarshalByRefObject, IVSCodeElementsSynchronizer, IExtMarshalByRefObject, RoseAddinFacade.IRoseAddin
    {
        /// <MetaDataID>{1b344523-6acc-4ae3-bbcd-f84aa26a980e}</MetaDataID>
        static string Net2ChannelUri;
        /// <MetaDataID>{a21dd27d-8d47-43f6-b06c-323e46a7e6a3}</MetaDataID>
        static VSCodeElementsSynchronizer MonoStateObject;
        /// <MetaDataID>{614dc240-0384-4e6e-afb3-959de613a73a}</MetaDataID>
        static VSCodeElementsSynchronizer()
        {
            string ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "RoseRemoting.config";
            if (!System.IO.File.Exists(ConfigFileName))
            {
                System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
                XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
                XmlDocument.Save(ConfigFileName);
            }
            System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName, false);

            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);
            try
            {
                //OOAdvantech.Remoting.RemotingServices.ProcessUserAnonymous = true;

                if (!OOAdvantech.Remoting.RemotingServices.IsIpcSeverChannelRegiter("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString()))
                    OOAdvantech.Remoting.RemotingServices.RegisterSecureIpcChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), true);

                //OOAdvantech.Remoting.RemotingServices.RegisterFreeIpcChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
                //OOAdvantech.Remoting.RemotingServices.RegisterIpcChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(),false);
                Net2ChannelUri = "PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString() + "Net2";
                ModulePublisher.ClassRepository.Init();
            }
            catch (System.Exception error)
            {
                //TODO Κάπως να ειδοποιήσω
                //if(
                //throw;
            }
        }

        /// <MetaDataID>{e468b89e-310b-4dbc-b05b-b01dbc6f0ca8}</MetaDataID>
        public VSCodeElementsSynchronizer()
        {
            if (MonoStateObject == null)
            {
                ModulePublisher.ClassRepository.LoadAssembly("CodeMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=9ce9f0a461f2c1a5");
                MonoStateObject = this;

            }

        }
        /// <MetaDataID>{1cbffcf2-4a25-4c70-8f9c-78bd73905fee}</MetaDataID>
        public string MainWindowTitle
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainWindowTitle;
            }
        }
        delegate void BrowseRoseItemHandler(string projectFileName, string classifierIdentity, string memberIdentity);
        /// <MetaDataID>{ae9db26e-511a-4f69-b666-b20e56610da9}</MetaDataID>
        public void BrowseRoseItem(string projectFileName, string classifierIdentity, string memberIdentity)
        {
            new BrowseRoseItemHandler(MonoStateObject.InnerBrowseRoseItem).BeginInvoke(projectFileName, classifierIdentity, memberIdentity, null, this);
        }
        /// <MetaDataID>{91011692-f716-4849-af3f-45a91e0f70f3}</MetaDataID>
        public void InnerBrowseRoseItem(string projectFileName, string classifierIdentity, string memberIdentity)
        {
            if (SynchroForm.InvokeRequired)
            {
                SynchroForm.Invoke(new BrowseRoseItemHandler(InnerBrowseRoseItem), new object[3] { projectFileName, classifierIdentity, memberIdentity });
                return;

            }

            MetaObjectMapper.Clear();
            try
            {
                if (RoseApplication == null || string.IsNullOrEmpty(classifierIdentity))
                    return;
                RationalRose.RoseClass roseClass = null;
                try
                {
                    string tt = RoseApplication.ApplicationPath;
                    RationalRose.RoseModel model = RoseApplication.CurrentModel;
                }
                catch (System.Exception error)
                {
                }
                RationalRose.RoseModuleCollection modules = RoseApplication.CurrentModel.GetAllModules();
                int count = modules.Count;
                for (int i = 0; i < count; i++)
                {
                    RationalRose.RoseModule module = modules.GetAt((short)(i + 1));
                    Component component = new Component(module);
                    if (component.Project == projectFileName)
                    {

                        foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in component.Residents)
                        {
                            if (metaObject.Identity.ToString() == classifierIdentity)
                            {
                                if (metaObject is Class)
                                    roseClass = (metaObject as Class).RoseClass;
                                if (metaObject is Interface)
                                    roseClass = (metaObject as Interface).RoseClass;
                                break;
                            }

                        }
                        if (roseClass != null)
                            break;
                    }

                }
                object MainWindowTitle = System.Diagnostics.Process.GetCurrentProcess().MainWindowTitle;
                if (roseClass == null)
                    return;
                if (roseClass.GetPropertyValue("MetaData", "MetaObjectID") == classifierIdentity)
                {
                    if (string.IsNullOrEmpty(memberIdentity))
                    {
                        RoseApplication.SelectObjectInBrowser(roseClass as RationalRose.RoseObject);
                        object wait = null;
                        // winShell.AppActivate(ref MainWindowTitle,ref wait);
                        //SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                        return;
                    }

                    for (int i = 0; i < roseClass.Attributes.Count; i++)
                    {
                        RationalRose.RoseAttribute roseAttribute = roseClass.Attributes.GetAt((short)(i + 1));

                        if (roseAttribute.GetPropertyValue("MetaData", "MetaObjectID") == memberIdentity)
                        {
                            RoseApplication.SelectObjectInBrowser(roseAttribute as RationalRose.RoseObject);
                            object wait = null;
                            //  winShell.AppActivate(ref MainWindowTitle, ref wait);

                            //SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                            return;
                        }
                    }

                    for (int i = 0; i < roseClass.Operations.Count; i++)
                    {
                        RationalRose.RoseOperation roseOperation = roseClass.Operations.GetAt((short)(i + 1));

                        if (roseOperation.GetPropertyValue("MetaData", "MetaObjectID") == memberIdentity)
                        {
                            RoseApplication.SelectObjectInBrowser(roseOperation as RationalRose.RoseObject);
                            object wait = null;
                            // winShell.AppActivate(ref MainWindowTitle, ref wait);

                            //SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                            //SetForegroundWindow 
                            return;
                        }
                    }
                    for (int i = 0; i < roseClass.GetAssociateRoles().Count; i++)
                    {
                        RationalRose.RoseRole roseRole = roseClass.GetAssociateRoles().GetAt((short)(i + 1));
                        if (roseRole.Association.GetPropertyValue("C#", "Identity") == memberIdentity)
                        {
                            RoseApplication.SelectObjectInBrowser(roseRole as RationalRose.RoseObject);
                            object wait = null;
                            //  winShell.AppActivate(ref MainWindowTitle, ref wait);

                            // SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                            return;
                        }
                    }



                }
            }
            finally
            {
                MetaObjectMapper.Clear();
            }

        }
        /// <MetaDataID>{d0892510-7e09-4071-994a-2b01f388e200}</MetaDataID>
        public void OnModifiedModelElementEx(RationalRose.RoseApplication roseApplication, RationalRose.RoseItem roseItem, int Reason, object Info)
        {
            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            roseItem = RoseObjectProxy.ControlRoseObject(roseItem) as RationalRose.RoseItem;
            roseApplication = null;
            roseApplication = null;
            GC.Collect();


        }
        /// <MetaDataID>{ddde4c43-df71-4817-be26-c4b1ad67cc3b}</MetaDataID>
        SynchroForm SynchroForm;
        /// <MetaDataID>{47139966-ddd8-4e9e-9819-239d9a867c04}</MetaDataID>
        int ROTCookie;
        /// <MetaDataID>{0ae35966-2fad-437e-b9ed-3f193cfe5da6}</MetaDataID>
        RationalRose.RoseApplication RoseApplication;
        /// <MetaDataID>{80ccff76-0155-445b-9276-b5136ef9e302}</MetaDataID>
        RationalRose.RoseContextMenuItem BrowseObjMessageMenu;
        /// <MetaDataID>{61FD810C-6DF9-46D9-904A-DC44CCF1019C}</MetaDataID>
        public void OnActivate(RationalRose.RoseApplication roseApplication)
        {
            //System.Windows.Forms.MessageBox.Show(this.GetType().Assembly.CodeBase);
            //System.Windows.Forms.MessageBox.Show(this.GetType().Assembly.FullName);
            //System.Windows.Forms.MessageBox.Show(typeof(RationalRose.IRoseAction).Assembly.CodeBase);
            //System.Windows.Forms.MessageBox.Show(typeof(RationalRose.IRoseAction).Assembly.FullName);
            RoseApplication = roseApplication;

            RoseAddinFacade.RoseAddin.Net4Addin = this;
            //  RoseApplication =RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;;
            SynchroForm = new SynchroForm();

            ModulePublisher.ClassRepository.Init();

            RoseVisualStudioBridge = new RoseVisualStudioBridge();

            UCOMIRunningObjectTable runningObjectTable = null;
            UCOMIMoniker moniker = null;
            MsdevManager.Msdev.GetRunningObjectTable(0, out runningObjectTable);
            MsdevManager.Msdev.CreateFileMoniker("RoseVisualStudioBridge:" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), out moniker);
            runningObjectTable.Register((int)MsdevManager.ROTFlags.REGISTRATIONKEEPSALIVE, this, moniker, out ROTCookie);


            if (!roseApplication.PathMap.HasEntry("dot Net Projects Path"))
                roseApplication.PathMap.AddEntry("dot Net Projects Path", "", "");

            if (!roseApplication.PathMap.HasEntry("Root"))
                roseApplication.PathMap.AddEntry("Root", "", "");

            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog = new ErrorLog(roseApplication);

            RationalRose.RoseAddIn CSharpAddIn = null;
            for (short i = 0; i != roseApplication.AddInManager.AddIns.Count; i++)
            {
                RationalRose.RoseAddIn roseAddIn = roseApplication.AddInManager.AddIns.GetAt((short)(i + 1));
                if (roseAddIn.Name == "C#")
                {
                    CSharpAddIn = roseAddIn;
                    break;
                }

            }



            if (CSharpAddIn != null)
            {
          


                #region Create class menu

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Submenu C#", "C#");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Browse Source", "C#Browse Source");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Browse Member Source", "C#Go to Operation  Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Generate Code...", "C#Update Code...");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Update Model...", "C#Update Model...");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Generate Member Code...", "C#Update Member Code...");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Update Member Model...", "C#Update Member Model...");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Remove unassigned item", "C#RemoveUnassignedItem");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "Edit Item Code ID", "C#EditClassID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsClass, "EndSubmenu", "C#EndSubmenu");

                #endregion


                #region Create component menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Submenu C#", "C#CompCOM C++");

                //CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "UpdateCode", "C#UpdateModuleCode");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Update Model", "C#UpdateModuleModel");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Browse Source", "C#CompBrowse Project");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Separator", "C#CompSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Assignment To Project..", "C#CompAssignment To Project..");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "EndSubmenu", "C#CompEndSubmenu");
                
                #endregion 

                #region Create operation menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Separator", "C#pSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Submenu C#", "C#Operation");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Browse Source", "C#Go to Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Generate Code", "C#Update operation Code");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Update Model", "C#Update operation Model");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Edit Item Code ID", "C#EditOperationID");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "EndSubmenu", "C#EndSubmenu");
                #endregion

                #region Create attribute menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Separator", "C#pSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Submenu C#", "C#Attribute");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Browse Source", "C#Go to Attribute Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Generate Code", "C#Update Attribute Code");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Update Model", "C#Update Attribute Model");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Edit Item Code ID", "C#EditAttributeID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "EndSubmenu", "C#EndSubmenu");
                #endregion

                #region Create Role menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Separator", "C#pSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Submenu C#", "C#Role");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Browse Source", "C#Go to Role Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Generate Code", "C#Update Role Code");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Update Model", "C#Update Role Model");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Edit Item Code ID", "C#EditRoleID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "EndSubmenu", "C#EndSubmenu");
                #endregion       
         
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsPackage, "Edit Item Code ID", "C#EditCategoryID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Submenu C#", "C#");
                BrowseObjMessageMenu = CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Browse Source", "CMCBrowse SourceCode");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "EndSubmenu", "C#EndSubmenu");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Separator", "C#Separator");




            }


            try
            {

                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            }
            catch (System.Exception error)
            {


            }
            roseApplication = null;

        }
        /// <MetaDataID>{0D33D0DB-3506-49E0-B441-6D1CE6BC2EF6}</MetaDataID>
        public void OnDeactivate(RationalRose.RoseApplication roseApplication)
        {
            BrowseObjMessageMenu = null;
            SynchroForm.Close();
            SynchroForm = null;

            //RoseApplication = null;
            UCOMIRunningObjectTable runningObjectTable = null;
            MsdevManager.Msdev.GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.Revoke(ROTCookie);

            OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog = null;
            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            roseApplication = null;

            GC.Collect();
            RoseObjectProxy.ReeasAll();
            GC.Collect();


            // Marshal.ReleaseComObject(roseApplication);
            roseApplication = null;
            //        RoseObjects.ReleaseRoseObjects();


        }




        /// <MetaDataID>{52521afa-c547-4835-9c9a-520a00829f35}</MetaDataID>
        [DllImport("ole32.dll")]
        static extern int GetRunningObjectTable(uint reserved,
           out System.Runtime.InteropServices.ComTypes.IRunningObjectTable pprot);

        /// <MetaDataID>{81bd3a1b-634d-4e95-aee6-b136f47ea8ed}</MetaDataID>
        System.Drawing.Point MouseLocation;
        /// <MetaDataID>{26a8a82d-914d-4794-9d8e-7b2c2709e9ab}</MetaDataID>
        public bool OnEnableContextMenuItems(RationalRose.RoseApplication roseApplication, int intItemType)
        {
            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;

            MouseLocation = System.Windows.Forms.Form.MousePosition;

            if (((int)RationalRose.RoseContextMenuItemType.rsDefault) == intItemType && BrowseObjMessageMenu != null)
            {
                bool tmp = roseApplication.CurrentModel.GetActiveDiagram() is RationalRose.RoseScenarioDiagram;
                int tmpp = (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().Count;
                if (tmpp > 0)
                {

                    //string rtrt = item.Name;
                    //if(item is RationalRose.RoseMessage);



                }
                bool tmpa = roseApplication.CurrentModel.GetActiveDiagram() is RationalRose.RoseScenarioDiagram;
                if (tmpa)
                {
                    int tmpb = (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().Count;
                }

                if (roseApplication.CurrentModel.GetActiveDiagram() is RationalRose.RoseScenarioDiagram
                    && (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().Count > 0)
                {

                    RationalRose.RoseItem roseItem = (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().GetAt(1);
                    if (roseItem is RationalRose.RoseMessage && (roseItem as RationalRose.RoseMessage).GetOperation() != null)
                    {
                        RationalRose.RoseOperation op = (roseItem as RationalRose.RoseMessage).GetOperation();
                        BrowseObjMessageMenu.MenuState = (short)RationalRose.RoseMenuState.rsEnabled;
                    }
                    else
                        BrowseObjMessageMenu.MenuState = (short)RationalRose.RoseMenuState.rsDisabled;

                }
                else
                    BrowseObjMessageMenu.MenuState = (short)RationalRose.RoseMenuState.rsDisabled;


            }
            roseApplication = null;
            return true;
        }
        /// <MetaDataID>{c5c67b73-32e7-49a6-ba2f-1741ee91ba53}</MetaDataID>
        public void OnOpenModel(RationalRose.RoseApplication roseApplication)
        {
            RoseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            roseApplication = null;

        }

        /// <MetaDataID>{aa9690dd-9c2e-4e74-9b2a-84167e72d196}</MetaDataID>
        public bool InnerSelectedContextMenuItem(object roseApplication, string internalName)
        {
            return OnSelectedContextMenuItem(roseApplication as RationalRose.RoseApplication, internalName);
        }
        /// <MetaDataID>{eb252aec-2838-417b-917b-c2997a277eb8}</MetaDataID>
        public bool SelectedContextMenuItem(object roseApplication, string internalName, int mouseLocationX, int mouseLocationY)
        {
            MouseLocation = new System.Drawing.Point(mouseLocationX, mouseLocationY);
            new SelectedContextMenuItemHandler(MonoStateObject.InnerSelectedContextMenuItem).BeginInvoke(roseApplication, internalName, null, this);
            return true;
        }

        delegate bool SelectedContextMenuItemHandler(object roseApplication, string internalName);

        /// <MetaDataID>{a6f6e53d-c930-4db8-a7d6-3a1398920823}</MetaDataID>
        internal static RoseVisualStudioBridge RoseVisualStudioBridge;
        /// <MetaDataID>{5CC00EB6-98C6-49D7-A8F9-C77872A12C35}</MetaDataID>
        public bool OnSelectedContextMenuItem(RationalRose.RoseApplication roseApplication, string internalName)
        {
            try
            {
                roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
                bool hasNet2ModelItems = false;
                bool hasNet4ModelItems = false;
                HasNet2ModelItems(roseApplication, internalName, out hasNet2ModelItems, out hasNet4ModelItems);
                if (hasNet2ModelItems)
                {

                    for (int i = 0; i < roseApplication.AddInManager.AddIns.Count; i++)
                    {
                        RationalRose.IRoseAddIn addin = roseApplication.AddInManager.AddIns.GetAt((short)(i + 1));
                        if (addin.Name == "Net2")
                        {
                            string chanelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(addin.EventHandler as MarshalByRefObject);
                            RoseAddinFacade.IRoseAddin RoseAddin = addin.EventHandler as RoseAddinFacade.IRoseAddin;
                            RoseAddin.SelectedContextMenuItem(null, internalName, MouseLocation.X, MouseLocation.Y);
                            if (!hasNet4ModelItems)
                                return true;
                        }
                    }
                }


                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;


                    //roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
                    RoseApplication = roseApplication;
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog = new ErrorLog(roseApplication);
                    // roseApplication = null;
                    GC.Collect();
                    if ("CMCBrowse SourceCode" == internalName)
                    {
                        if (roseApplication.CurrentModel.GetSelectedItems().Count > 0
                            && roseApplication.CurrentModel.GetSelectedItems().GetAt(1) is RationalRose.RoseMessage)
                        {
                            RoseVisualStudioBridge.Browse(roseApplication);

                        }
                    }
                    if ("C#CompAssignment To Project.." == internalName)
                    {
                        try
                        {
                            MetaObjectMapper.Clear();
                            if (roseApplication.CurrentModel.GetSelectedModules().Count > 0)
                            {
                                RationalRose.RoseModule roseComponent = roseApplication.CurrentModel.GetSelectedModules().GetAt(1);
                                ProjectSelectionForm projectSelectionForm = new ProjectSelectionForm();

                                Component component = new Component(roseComponent);
                                projectSelectionForm.Connection.Instance = component;
                                projectSelectionForm.ShowDialog();
                                component.RoseComponent = null;


                            }

                        }
                        catch (System.Exception error)
                        {


                        }
                    }
                    if ("C#EditClassID" == internalName
                        || "C#EditAttributeID" == internalName
                        || "C#EditOperationID" == internalName
                        || "C#EditCategoryID" == internalName)
                    {

                        if (roseApplication.CurrentModel.GetSelectedItems().Count > 0)
                        {
                            try
                            {
                                ModelItemIdentityView modelItemIdentity = new ModelItemIdentityView();
                                modelItemIdentity.ConnectionControl.Instance = new RoseModelItem(roseApplication.CurrentModel.GetSelectedItems().GetAt(1));
                                modelItemIdentity.ShowDialog();
                                (modelItemIdentity.ConnectionControl.Instance as RoseModelItem).RoseItem = null;

                            }
                            catch (System.Exception error)
                            {

                            }
                        }

                    }

                    if ("C#RemoveUnassignedItem" == internalName)
                    {
                        RoseVisualStudioBridge.RemoveUnassignedItems(roseApplication);
                    }

                    if ("C#CompBrowse Project" == internalName)
                    {
                        try
                        {

                            MetaObjectMapper.Clear();
                            if (roseApplication.CurrentModel.GetSelectedModules().Count > 0)
                            {
                                RationalRose.RoseModule roseComponent = roseApplication.CurrentModel.GetSelectedModules().GetAt(1);

                                Component component = new Component(roseComponent);

                                EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                                //if (dte != null)
                                //    MsdevManager.Msdev.ShowIDE(dte);
                                foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                                {
                                    try
                                    {
                                        if (vsProject.FileName == component.Project)
                                        {
                                            OOAdvantech.MetaDataRepository.Component project = RoseVisualStudioBridge.GetComponentFor(vsProject);


                                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = RoseVisualStudioBridge.InitMetaObjectsStack();

                                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(roseApplication);
                                            //OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(vsProject.DTE);
                                            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                                            component.SetIdentity(project.Identity.ToString());
                                            MetaObjectMapper.AddTypeMap(component.RoseComponent.GetUniqueID(), component);



                                            //project.Synchronize(component);
                                            component.Synchronize(project);

                                            //foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in project.Residents)
                                            //{
                                            //    string identity= metaObject.Identity.ToString();
                                            //}

                                        }

                                    }
                                    catch (System.Exception error)
                                    {

                                    }
                                    finally
                                    {
                                        RoseVisualStudioBridge.ClearMetaObjectsStack();
                                        MetaObjectMapper.Clear();
                                    }
                                }

                                component.RoseComponent = null;
                            }



                        }
                        catch (System.Exception error)
                        {

                        }
                    }

                    if ("C#UpdateModuleCode" == internalName)
                    {
                        RoseVisualStudioBridge.UpdateModuleCode(roseApplication);
                    }
                    if ("C#Update Code..." == internalName)
                    {
                        RoseVisualStudioBridge.UpdateCode(roseApplication);
                    }
                    if ("C#Update Model..." == internalName)
                        RoseVisualStudioBridge.UpdateRoseItem(roseApplication);
                    if ("C#UpdateModuleModel" == internalName)
                        RoseVisualStudioBridge.UpdateRoseModule(roseApplication);

                    if (("C#Go to Operation  Definition" == internalName
                        || "C#Update Member Code..." == internalName
                        || "C#Update Member Model..." == internalName
                        && roseApplication.CurrentModel.GetSelectedClasses().Count > 0))
                    {
                        try
                        {
                            ActionType ActionType = default(ActionType);
                            if ("C#Update Member Code..." == internalName)
                                ActionType = ActionType.UpdateCode;
                            if ("C#Go to Operation  Definition" == internalName)
                                ActionType = ActionType.Browse;

                            if ("C#Update Member Model..." == internalName)
                                ActionType = ActionType.UpdateModel;



                            MetaObjectMapper.Clear();
                            //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                            {
                                RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt(1);

                                ClassifierMembersView classifierMembersView = new ClassifierMembersView();


                                classifierMembersView.Location = MouseLocation;
                                Component implementationUnit = null;

                                if (roseClass.GetAssignedModules().Count > 0)
                                {
                                    RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);
                                    implementationUnit = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                                    if (implementationUnit == null)
                                        implementationUnit = new Component(roseComponent);
                                }
                                if (roseApplication.CurrentModel.GetSelectedClasses().GetAt(1).Stereotype == "Interface")
                                    classifierMembersView.ConnectionControl.Instance = new ClassifierPresentation(new Interface(roseApplication.CurrentModel.GetSelectedClasses().GetAt(1), implementationUnit), ActionType);
                                else
                                    classifierMembersView.ConnectionControl.Instance = new ClassifierPresentation(new Class(roseApplication.CurrentModel.GetSelectedClasses().GetAt(1), implementationUnit), ActionType);
                                classifierMembersView.Show();
                                // stateTransition.Consistent = true;
                            }


                        }
                        catch (System.Exception error)
                        {

                        }

                        MetaObjectMapper.Clear();



                    }

                    if ("C#Browse Source" == internalName
                        || "C#Go to Definition" == internalName
                        || "C#Go to Attribute Definition" == internalName
                        || "C#Go to Role Definition" == internalName)
                    {
                        RoseVisualStudioBridge.Browse(roseApplication);
                    }

                    if ("C#Update operation Code" == internalName
                        || "C#Update Attribute Code" == internalName
                         || "C#Update Role Code" == internalName)
                    {
                        RoseVisualStudioBridge.UpdateMemberCode(roseApplication);
                    }
                    if ("C#Update operation Model" == internalName
                    || "C#Update Attribute Model" == internalName
                     || "C#Update Role Model" == internalName)
                    {
                        RoseVisualStudioBridge.UpdateMemberModel(roseApplication);
                    }



                    //for (int i = 0; i != roseApplication.CurrentModel.GetSelectedClasses().Count; i++)
                    //{
                    //    RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt((short)(i + 1));

                    //    try
                    //    {

                    //        string name = roseClass.Name;
                    //    }
                    //    catch (Exception error)
                    //    {


                    //    }
                    //}

                    roseApplication = null;
                    GC.Collect();
                    return true;
                    stateTransition.Consistent = true;
                }

            }
            finally
            {

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            }

        }

        /// <MetaDataID>{1e293431-acab-45ba-89b9-ffa82bb38e2e}</MetaDataID>
        private void HasNet2ModelItems(RationalRose.RoseApplication roseApplication, string internalName, out bool hasNet2ModelItems, out bool hasNet4ModelItems)
        {
            hasNet2ModelItems = false;
            hasNet4ModelItems = false;
            for (int i = 0; i < roseApplication.CurrentModel.GetSelectedClasses().Count; i++)
            {
                RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt((short)(i + 1));
                if (roseClass.GetAssignedModules().Count == 0)
                    continue;
                if (roseClass.GetAssignedModules().Count == 0)
                    continue;
                RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);

                Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                if (component == null)
                    component = new Component(roseComponent);
                EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                if (dte.Version == "9.0")
                    hasNet2ModelItems = true;
                else
                    hasNet4ModelItems = true;
            }

        }
    }
#else

    /// <MetaDataID>{A574A68E-EB3B-4EC7-A787-68DBC874937E}</MetaDataID>
    [Guid("B754EB9C-8D51-46df-8757-28D2C377B926")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class VSNet2CodeElementsSynchronizer :MarshalByRefObject,  IVSCodeElementsSynchronizer,IExtMarshalByRefObject,RoseAddinFacade.IRoseAddin
    {
        /// <MetaDataID>{a21dd27d-8d47-43f6-b06c-323e46a7e6a3}</MetaDataID>
      public  static VSNet2CodeElementsSynchronizer MonoStateObject;
        /// <MetaDataID>{614dc240-0384-4e6e-afb3-959de613a73a}</MetaDataID>
        static VSNet2CodeElementsSynchronizer()
        {
            string ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "RoseRemoting.config";
            if (!System.IO.File.Exists(ConfigFileName))
            {
                System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
                XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
                XmlDocument.Save(ConfigFileName);
            }
            System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName, false);

            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);
            try
            {

                ModulePublisher.ClassRepository.Init();
                OOAdvantech.Remoting.RemotingServices.RegisterChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString()+"Net2",false); 
            }
            catch (System.Exception error)
            {
                //TODO Κάπως να ειδοποιήσω
                //if(
                //throw;
            }
        }
        /// <MetaDataID>{e468b89e-310b-4dbc-b05b-b01dbc6f0ca8}</MetaDataID>
        public VSNet2CodeElementsSynchronizer()
        {
            if (MonoStateObject == null)
            {
                ModulePublisher.ClassRepository.LoadAssembly("CodeMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=a849addb868b59ea");
                MonoStateObject = this;

            }
   
        }
        /// <MetaDataID>{1cbffcf2-4a25-4c70-8f9c-78bd73905fee}</MetaDataID>
        public string MainWindowTitle
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainWindowTitle;
            }
        }
        delegate void BrowseRoseItemHandler(string projectFileName, string classifierIdentity, string memberIdentity);
        /// <MetaDataID>{ae9db26e-511a-4f69-b666-b20e56610da9}</MetaDataID>
        public void BrowseRoseItem(string projectFileName, string classifierIdentity, string memberIdentity)
        {
            new BrowseRoseItemHandler(MonoStateObject. InnerBrowseRoseItem).BeginInvoke(projectFileName, classifierIdentity, memberIdentity, null, this);
        }
        /// <MetaDataID>{91011692-f716-4849-af3f-45a91e0f70f3}</MetaDataID>
        public void InnerBrowseRoseItem(string projectFileName, string classifierIdentity, string memberIdentity)
        {
            if (SynchroForm.InvokeRequired)
            {
                SynchroForm.Invoke(new BrowseRoseItemHandler(InnerBrowseRoseItem), new object[3] { projectFileName, classifierIdentity, memberIdentity });
                return;

            }

            MetaObjectMapper.Clear();
            try
            {
                if (RoseApplication == null || string.IsNullOrEmpty(classifierIdentity))
                    return;
                RationalRose.RoseClass roseClass = null;
                try
                {
                    string tt = RoseApplication.ApplicationPath;
                    RationalRose.RoseModel model = RoseApplication.CurrentModel;
                }
                catch (System.Exception error)
                {
                }
                RationalRose.RoseModuleCollection modules = RoseApplication.CurrentModel.GetAllModules();
                int count = modules.Count;
                for (int i = 0; i < count; i++)
                {
                    RationalRose.RoseModule module = modules.GetAt((short)(i + 1));
                    Component component = new Component(module);
                    if (component.Project == projectFileName)
                    {

                        foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in component.Residents)
                        {
                            if (metaObject.Identity.ToString() == classifierIdentity)
                            {
                                if (metaObject is Class)
                                    roseClass = (metaObject as Class).RoseClass;
                                if (metaObject is Interface)
                                    roseClass = (metaObject as Interface).RoseClass;
                                break;
                            }

                        }
                        if (roseClass != null)
                            break;
                    }

                }
                object MainWindowTitle = System.Diagnostics.Process.GetCurrentProcess().MainWindowTitle;
                if (roseClass == null)
                    return;
                if (roseClass.GetPropertyValue("MetaData", "MetaObjectID") == classifierIdentity)
                {
                    if (string.IsNullOrEmpty(memberIdentity))
                    {
                        RoseApplication.SelectObjectInBrowser(roseClass as RationalRose.RoseObject);
                        object wait = null;
                        // winShell.AppActivate(ref MainWindowTitle,ref wait);
                        //SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                        return;
                    }

                    for (int i = 0; i < roseClass.Attributes.Count; i++)
                    {
                        RationalRose.RoseAttribute roseAttribute = roseClass.Attributes.GetAt((short)(i + 1));

                        if (roseAttribute.GetPropertyValue("MetaData", "MetaObjectID") == memberIdentity)
                        {
                            RoseApplication.SelectObjectInBrowser(roseAttribute as RationalRose.RoseObject);
                            object wait = null;
                            //  winShell.AppActivate(ref MainWindowTitle, ref wait);

                            //SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                            return;
                        }
                    }

                    for (int i = 0; i < roseClass.Operations.Count; i++)
                    {
                        RationalRose.RoseOperation roseOperation = roseClass.Operations.GetAt((short)(i + 1));

                        if (roseOperation.GetPropertyValue("MetaData", "MetaObjectID") == memberIdentity)
                        {
                            RoseApplication.SelectObjectInBrowser(roseOperation as RationalRose.RoseObject);
                            object wait = null;
                            // winShell.AppActivate(ref MainWindowTitle, ref wait);

                            //SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                            //SetForegroundWindow 
                            return;
                        }
                    }
                    for (int i = 0; i < roseClass.GetAssociateRoles().Count; i++)
                    {
                        RationalRose.RoseRole roseRole = roseClass.GetAssociateRoles().GetAt((short)(i + 1));
                        if (roseRole.Association.GetPropertyValue("C#", "Identity") == memberIdentity)
                        {
                            RoseApplication.SelectObjectInBrowser(roseRole as RationalRose.RoseObject);
                            object wait = null;
                            //  winShell.AppActivate(ref MainWindowTitle, ref wait);

                            // SetForegroundWindow(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
                            return;
                        }
                    }



                }
            }
            finally
            {
                MetaObjectMapper.Clear();
            }

        }
        /// <MetaDataID>{d0892510-7e09-4071-994a-2b01f388e200}</MetaDataID>
        public void OnModifiedModelElementEx(RationalRose.RoseApplication roseApplication, RationalRose.RoseItem roseItem, int Reason, object Info)
        {
            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            roseItem = RoseObjectProxy.ControlRoseObject(roseItem) as RationalRose.RoseItem;
            roseApplication = null;
            roseApplication = null;
            GC.Collect();


        }
        /// <MetaDataID>{ddde4c43-df71-4817-be26-c4b1ad67cc3b}</MetaDataID>
        SynchroForm SynchroForm;
        /// <MetaDataID>{47139966-ddd8-4e9e-9819-239d9a867c04}</MetaDataID>
        int ROTCookie;
        /// <MetaDataID>{0ae35966-2fad-437e-b9ed-3f193cfe5da6}</MetaDataID>
        RationalRose.RoseApplication RoseApplication;
        /// <MetaDataID>{80ccff76-0155-445b-9276-b5136ef9e302}</MetaDataID>
        RationalRose.RoseContextMenuItem BrowseObjMessageMenu;
        /// <MetaDataID>{61FD810C-6DF9-46D9-904A-DC44CCF1019C}</MetaDataID>
        public void OnActivate(RationalRose.RoseApplication roseApplication)
        {
            RoseAddinFacade.RoseAddin.Net2Addin = this;
            //System.Windows.Forms.MessageBox.Show(this.GetType().Assembly.CodeBase);
            //System.Windows.Forms.MessageBox.Show(this.GetType().Assembly.FullName);
            //System.Windows.Forms.MessageBox.Show(typeof(RationalRose.IRoseAction).Assembly.CodeBase);
            //System.Windows.Forms.MessageBox.Show(typeof(RationalRose.IRoseAction).Assembly.FullName);


            //  RoseApplication =RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;;
            SynchroForm = new SynchroForm();

            ModulePublisher.ClassRepository.Init();

            RoseVisualStudioBridge = new RoseVisualStudioBridge();

            UCOMIRunningObjectTable runningObjectTable = null;
            UCOMIMoniker moniker = null;
            MsdevManager.Msdev.GetRunningObjectTable(0, out runningObjectTable);
            MsdevManager.Msdev.CreateFileMoniker("RoseVisualStudioBridge:" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), out moniker);
            runningObjectTable.Register((int)MsdevManager.ROTFlags.REGISTRATIONKEEPSALIVE, this, moniker, out ROTCookie);


            if (!roseApplication.PathMap.HasEntry("dot Net Projects Path"))
                roseApplication.PathMap.AddEntry("dot Net Projects Path","","");

            if (!roseApplication.PathMap.HasEntry("Root"))
                roseApplication.PathMap.AddEntry("Root", "","");

            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog = new ErrorLog(roseApplication);
            RoseApplication = roseApplication;
            RationalRose.RoseAddIn CSharpAddIn = null;
            for (short i = 0; i != roseApplication.AddInManager.AddIns.Count; i++)
            {
                RationalRose.RoseAddIn roseAddIn = roseApplication.AddInManager.AddIns.GetAt((short)(i + 1));
                if (roseAddIn.Name == "C#")
                {
                    CSharpAddIn = roseAddIn;
                    break;
                }

            }


#if Net4
            if (CSharpAddIn != null)
            {
          
                #region Create component menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Submenu C#", "C#CompCOM C++");

                //CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "UpdateCode", "C#UpdateModuleCode");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Update Model", "C#UpdateModuleModel");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Browse Source", "C#CompBrowse Project");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Separator", "C#CompSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "Assignment To Project..", "C#CompAssignment To Project..");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsComponent, "EndSubmenu", "C#CompEndSubmenu");
                
                #endregion 

                #region Create operation menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Separator", "C#pSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Submenu C#", "C#Operation");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Browse Source", "C#Go to Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Generate Code", "C#Update operation Code");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Update Model", "C#Update operation Model");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "Edit Item Code ID", "C#EditOperationID");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsOperation, "EndSubmenu", "C#EndSubmenu");
                #endregion

                #region Create attribute menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Separator", "C#pSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Submenu C#", "C#Attribute");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Browse Source", "C#Go to Attribute Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Generate Code", "C#Update Attribute Code");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Update Model", "C#Update Attribute Model");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "Edit Item Code ID", "C#EditAttributeID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsAttribute, "EndSubmenu", "C#EndSubmenu");
                #endregion

                #region Create Role menu
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Separator", "C#pSeparator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Submenu C#", "C#Role");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Browse Source", "C#Go to Role Definition");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Generate Code", "C#Update Role Code");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Update Model", "C#Update Role Model");

                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "Edit Item Code ID", "C#EditRoleID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsRole, "EndSubmenu", "C#EndSubmenu");
                #endregion       
         
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsPackage, "Edit Item Code ID", "C#EditCategoryID");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Separator", "C#Separator");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Submenu C#", "C#");
                BrowseObjMessageMenu = CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Browse Source", "CMCBrowse SourceCode");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "EndSubmenu", "C#EndSubmenu");
                CSharpAddIn.AddContextMenuItem((short)RationalRose.RoseContextMenuItemType.rsDefault, "Separator", "C#Separator");




            }
#endif

            try
            {

                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            }
            catch (System.Exception error)
            {


            }
            roseApplication = null;

        }
        /// <MetaDataID>{0D33D0DB-3506-49E0-B441-6D1CE6BC2EF6}</MetaDataID>
        public void OnDeactivate(RationalRose.RoseApplication roseApplication)
        {
            BrowseObjMessageMenu = null;
            SynchroForm.Close();
            SynchroForm = null;

            //RoseApplication = null;
            UCOMIRunningObjectTable runningObjectTable = null;
            MsdevManager.Msdev.GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.Revoke(ROTCookie);

            OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog = null;
            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            roseApplication = null;

            GC.Collect();
            RoseObjectProxy.ReeasAll();
            GC.Collect();


            // Marshal.ReleaseComObject(roseApplication);
            roseApplication = null;
            //        RoseObjects.ReleaseRoseObjects();


        }




        /// <MetaDataID>{52521afa-c547-4835-9c9a-520a00829f35}</MetaDataID>
        [DllImport("ole32.dll")]
        static extern int GetRunningObjectTable(uint reserved,
           out System.Runtime.InteropServices.ComTypes.IRunningObjectTable pprot);

        /// <MetaDataID>{81bd3a1b-634d-4e95-aee6-b136f47ea8ed}</MetaDataID>
        System.Drawing.Point MouseLocation;
        /// <MetaDataID>{26a8a82d-914d-4794-9d8e-7b2c2709e9ab}</MetaDataID>
        public bool OnEnableContextMenuItems(RationalRose.RoseApplication roseApplication, int intItemType)
        {
            roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;

            MouseLocation = System.Windows.Forms.Form.MousePosition;

            if (((int)RationalRose.RoseContextMenuItemType.rsDefault) == intItemType && BrowseObjMessageMenu != null)
            {
                bool tmp = roseApplication.CurrentModel.GetActiveDiagram() is RationalRose.RoseScenarioDiagram;
                int tmpp = (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().Count;
                if (tmpp > 0)
                {

                    //string rtrt = item.Name;
                    //if(item is RationalRose.RoseMessage);



                }
                bool tmpa = roseApplication.CurrentModel.GetActiveDiagram() is RationalRose.RoseScenarioDiagram;
                if (tmpa)
                {
                    int tmpb = (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().Count;
                }

                if (roseApplication.CurrentModel.GetActiveDiagram() is RationalRose.RoseScenarioDiagram
                    && (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().Count > 0)
                {

                    RationalRose.RoseItem roseItem = (roseApplication.CurrentModel.GetActiveDiagram() as RationalRose.RoseScenarioDiagram).GetSelectedItems().GetAt(1);
                    if (roseItem is RationalRose.RoseMessage && (roseItem as RationalRose.RoseMessage).GetOperation() != null)
                    {
                        RationalRose.RoseOperation op = (roseItem as RationalRose.RoseMessage).GetOperation();
                        BrowseObjMessageMenu.MenuState = (short)RationalRose.RoseMenuState.rsEnabled;
                    }
                    else
                        BrowseObjMessageMenu.MenuState = (short)RationalRose.RoseMenuState.rsDisabled;

                }
                else
                    BrowseObjMessageMenu.MenuState = (short)RationalRose.RoseMenuState.rsDisabled;


            }
            roseApplication = null;
            return true;
        }
        /// <MetaDataID>{c5c67b73-32e7-49a6-ba2f-1741ee91ba53}</MetaDataID>
        public void OnOpenModel(RationalRose.RoseApplication roseApplication)
        {
            RoseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
            roseApplication = null;

        }

        /// <MetaDataID>{a6f6e53d-c930-4db8-a7d6-3a1398920823}</MetaDataID>
        internal static RoseVisualStudioBridge RoseVisualStudioBridge;





        /// <MetaDataID>{efe54d8a-a21f-46aa-b0d1-af032ec5371a}</MetaDataID>
        public bool InnerSelectedContextMenuItem(object roseApplication, string internalName)
        {
            if (SynchroForm.InvokeRequired)
            {
                object ret = SynchroForm.Invoke(new SelectedContextMenuItemHandler(MonoStateObject.InnerSelectedContextMenuItem), new object[2] { roseApplication, internalName });
                if (ret is bool)
                    return (bool)ret;
                return false;

            }
            return OnSelectedContextMenuItem(roseApplication as RationalRose.RoseApplication, internalName);
        }

        public bool SelectedContextMenuItem(object roseApplication, string internalName, int mouseLocationX, int mouseLocationY)
        {
            MouseLocation = new System.Drawing.Point(mouseLocationX, mouseLocationY);
            IAsyncResult result = new SelectedContextMenuItemHandler(MonoStateObject.InnerSelectedContextMenuItem).BeginInvoke(roseApplication, internalName, null, this);
            //result.AsyncWaitHandle.WaitOne();
            return true;
        }

        delegate bool SelectedContextMenuItemHandler(object roseApplication, string internalName);
        /// <MetaDataID>{5CC00EB6-98C6-49D7-A8F9-C77872A12C35}</MetaDataID>
        public bool OnSelectedContextMenuItem(RationalRose.RoseApplication roseApplication, string internalName)
        {
            try
            {
               

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                    if (roseApplication != null)
                    {
                        roseApplication = RoseObjectProxy.ControlRoseObject(roseApplication) as RationalRose.RoseApplication;
                        RoseApplication = roseApplication;
                    }
                    else
                        roseApplication = RoseApplication;
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog = new ErrorLog(roseApplication);
                    // roseApplication = null;
                    GC.Collect();
                    if ("CMCBrowse SourceCode" == internalName)
                    {
                        if (roseApplication.CurrentModel.GetSelectedItems().Count > 0
                            && roseApplication.CurrentModel.GetSelectedItems().GetAt(1) is RationalRose.RoseMessage)
                        {
                            RoseVisualStudioBridge.Browse(roseApplication);

                        }
                    }
                    if ("C#CompAssignment To Project.." == internalName)
                    {
                        try
                        {
                            MetaObjectMapper.Clear();
                            if (roseApplication.CurrentModel.GetSelectedModules().Count > 0)
                            {
                                RationalRose.RoseModule roseComponent = roseApplication.CurrentModel.GetSelectedModules().GetAt(1);
                                ProjectSelectionForm projectSelectionForm = new ProjectSelectionForm();

                                Component component = new Component(roseComponent);
                                projectSelectionForm.Connection.Instance = component;
                                projectSelectionForm.ShowDialog();
                                component.RoseComponent = null;


                            }

                        }
                        catch (System.Exception error)
                        {


                        }
                    }
                    if ("C#EditClassID" == internalName
                        || "C#EditAttributeID" == internalName
                        || "C#EditOperationID" == internalName
                        || "C#EditCategoryID" == internalName)
                    {

                        if (roseApplication.CurrentModel.GetSelectedItems().Count > 0)
                        {
                            try
                            {
                                ModelItemIdentityView modelItemIdentity = new ModelItemIdentityView();
                                modelItemIdentity.ConnectionControl.Instance = new RoseModelItem(roseApplication.CurrentModel.GetSelectedItems().GetAt(1));
                                modelItemIdentity.ShowDialog();
                                (modelItemIdentity.ConnectionControl.Instance as RoseModelItem).RoseItem = null;

                            }
                            catch (System.Exception error)
                            {

                            }
                        }

                    }

                    if ("C#RemoveUnassignedItem" == internalName)
                    {
                        RoseVisualStudioBridge.RemoveUnassignedItems(roseApplication);
                    }

                    if ("C#CompBrowse Project" == internalName)
                    {
                        try
                        {

                            MetaObjectMapper.Clear();
                            if (roseApplication.CurrentModel.GetSelectedModules().Count > 0)
                            {
                                RationalRose.RoseModule roseComponent = roseApplication.CurrentModel.GetSelectedModules().GetAt(1);

                                Component component = new Component(roseComponent);

                                EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                                //if (dte != null)
                                //    MsdevManager.Msdev.ShowIDE(dte);
                                foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                                {
                                    try
                                    {
                                        if (vsProject.FileName == component.Project)
                                        {
                                            OOAdvantech.MetaDataRepository.Component project =RoseVisualStudioBridge.GetComponentFor(vsProject);


                                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator=RoseVisualStudioBridge.InitMetaObjectsStack();
                                           
                                            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(roseApplication);
                                            //OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(vsProject.DTE);
                                            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                                            component.SetIdentity(project.Identity.ToString());
                                            MetaObjectMapper.AddTypeMap(component.RoseComponent.GetUniqueID(), component);



                                            //project.Synchronize(component);
                                            component.Synchronize(project);

                                            //foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in project.Residents)
                                            //{
                                            //    string identity= metaObject.Identity.ToString();
                                            //}

                                        }

                                    }
                                    catch (System.Exception error)
                                    {

                                    }
                                    finally
                                    {
                                        RoseVisualStudioBridge.ClearMetaObjectsStack();
                                        MetaObjectMapper.Clear();
                                    }
                                }

                                component.RoseComponent = null;
                            }



                        }
                        catch (System.Exception error)
                        {

                        }
                    }

                    if ("C#UpdateModuleCode" == internalName)
                    {
                        RoseVisualStudioBridge.UpdateModuleCode(roseApplication);
                    }
                    if ("C#Update Code..." == internalName)
                    {
                        RoseVisualStudioBridge.UpdateCode(roseApplication);
                    }
                    if ("C#Update Model..." == internalName)
                        RoseVisualStudioBridge.UpdateRoseItem(roseApplication);
                    if ("C#UpdateModuleModel" == internalName)
                        RoseVisualStudioBridge.UpdateRoseModule(roseApplication);
                    
                    if (("C#Go to Operation  Definition" == internalName
                        || "C#Update Member Code..." == internalName
                        || "C#Update Member Model..." == internalName
                        && roseApplication.CurrentModel.GetSelectedClasses().Count > 0))
                    {
                        try
                        {
                            ActionType ActionType = default(ActionType);
                            if ("C#Update Member Code..." == internalName)
                                ActionType = ActionType.UpdateCode;
                            if ("C#Go to Operation  Definition" == internalName)
                                ActionType = ActionType.Browse;

                            if ("C#Update Member Model..." == internalName)
                                ActionType = ActionType.UpdateModel;



                            MetaObjectMapper.Clear();
                            //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                            {
                                RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt(1);

                                ClassifierMembersView classifierMembersView = new ClassifierMembersView();


                                classifierMembersView.Location = MouseLocation;
                                Component implementationUnit = null;

                                if (roseClass.GetAssignedModules().Count > 0)
                                {
                                    RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);
                                    implementationUnit = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                                    if (implementationUnit == null)
                                        implementationUnit = new Component(roseComponent);
                                }
                                if (roseApplication.CurrentModel.GetSelectedClasses().GetAt(1).Stereotype == "Interface")
                                    classifierMembersView.ConnectionControl.Instance = new ClassifierPresentation(new Interface(roseApplication.CurrentModel.GetSelectedClasses().GetAt(1), implementationUnit), ActionType);
                                else
                                    classifierMembersView.ConnectionControl.Instance = new ClassifierPresentation(new Class(roseApplication.CurrentModel.GetSelectedClasses().GetAt(1), implementationUnit), ActionType);
                                classifierMembersView.Show();
                                // stateTransition.Consistent = true;
                            }


                        }
                        catch (System.Exception error)
                        {

                        }

                        MetaObjectMapper.Clear();



                    }

                    if ("C#Browse Source" == internalName
                        || "C#Go to Definition" == internalName
                        || "C#Go to Attribute Definition" == internalName
                        || "C#Go to Role Definition" == internalName)
                    {
                        RoseVisualStudioBridge.Browse(roseApplication);
                    }

                    if ("C#Update operation Code" == internalName
                        || "C#Update Attribute Code" == internalName
                         || "C#Update Role Code" == internalName)
                    {
                        RoseVisualStudioBridge.UpdateMemberCode(roseApplication);
                    }
                    if ("C#Update operation Model" == internalName
                    || "C#Update Attribute Model" == internalName
                     || "C#Update Role Model" == internalName)
                    {
                        RoseVisualStudioBridge.UpdateMemberModel(roseApplication);
                    }














                    //for (int i = 0; i != roseApplication.CurrentModel.GetSelectedClasses().Count; i++)
                    //{
                    //    RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt((short)(i + 1));

                    //    try
                    //    {

                    //        string name = roseClass.Name;
                    //    }
                    //    catch (Exception error)
                    //    {


                    //    }
                    //}

                    roseApplication = null;
                    GC.Collect();
                    return true;
                    stateTransition.Consistent = true;
                }

            }
            finally
            {

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            }

        }




     

        
    }

#endif


    /// <MetaDataID>{ff04cb08-e4fe-4f94-80ea-f755d4427cdd}</MetaDataID>
    internal class SynchroForm : System.Windows.Forms.Form
    {


        /// <MetaDataID>{a7913d23-87d4-4e9f-a5ee-8f287dd53a9d}</MetaDataID>
        public SynchroForm()
        {
            ShowInTaskbar = false;
            Text = "";
            MinimizeBox = false;
            MaximizeBox = false;
            this.ControlBox = false;
            MinimumSize = new System.Drawing.Size(1, 1);
            ClientSize = new System.Drawing.Size(0, 0);
            Size = new System.Drawing.Size(1, 1);
            Visible = false;
            Show();
            Visible = false;

        }
    }




}
