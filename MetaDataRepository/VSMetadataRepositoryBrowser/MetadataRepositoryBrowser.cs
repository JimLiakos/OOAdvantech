using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace VSMetadataRepositoryBrowser
{
    /// <MetaDataID>{5692012b-4a45-487e-8372-df36d369ece2}</MetaDataID>
    public partial class MetadataRepositoryBrowser : Form
    {
        /// <MetaDataID>{7ae7383b-a91b-4446-80f1-5da08a64636b}</MetaDataID>
        IVSPackage VSPackage;
        /// <MetaDataID>{c3ee9d73-56ac-46a6-b677-38fd1c457fad}</MetaDataID>
        OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = new OOAdvantech.CodeMetaDataRepository.IDEManager();
        /// <MetaDataID>{efe81d76-5580-441a-9e83-558d55ac8e2e}</MetaDataID>
        public MetadataRepositoryBrowser(IVSPackage vsPackage)
        {
            InitializeComponent();
            TopLevel = false;
            Visible = true;
           
            VSPackage = vsPackage;

        }
        /// <MetaDataID>{a8274795-f94d-4e04-bec0-b9e355e77a35}</MetaDataID>
        MetadataRepositoryBrowser()
        {
        }
        /// <MetaDataID>{068d44f1-3ec4-4e8b-bb1a-e9cc4484721b}</MetaDataID>
        public void OnBeforeShowContextMenu(OOAdvantech.UserInterface.MenuCommand menu,object selectedItem)
        {

            if (selectedItem is MetaObjectTreeNode && (selectedItem as MetaObjectTreeNode).MetaObject is OOAdvantech.MetaDataRepository.Class)
            {
                OOAdvantech.UserInterface.MenuCommand command = new OOAdvantech.UserInterface.MenuCommand();
                command.Name = "Class ORM";
                command.Click += new OOAdvantech.UserInterface.MenuCommandClickedHandler(OnClassORM); 
                menu.AddSubCommand(command);

                command = new OOAdvantech.UserInterface.MenuCommand();
                command.Name = "Add/Update Object state code";
                command.Click += new OOAdvantech.UserInterface.MenuCommandClickedHandler(OnObjectStateCodeCodeUpdate);
                menu.AddSubCommand(command);
            }

            if (selectedItem is MetaObjectTreeNode && (selectedItem as MetaObjectTreeNode).MetaObject is  OOAdvantech.CodeMetaDataRepository.Project)
            {
                OOAdvantech.UserInterface.MenuCommand command = new OOAdvantech.UserInterface.MenuCommand();
                command.Name = "Project ORM";
                command.Click += new OOAdvantech.UserInterface.MenuCommandClickedHandler(OnClassORM); 
                menu.AddSubCommand(command);

                command = new OOAdvantech.UserInterface.MenuCommand();
                command.Name = "Add/Update Object state code";
                command.Click += new OOAdvantech.UserInterface.MenuCommandClickedHandler(OnObjectStateCodeCodeUpdate);
                menu.AddSubCommand(command);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            
            MetaObjectsTree.RowHeight= Graphics.FromHwnd(MetaObjectsTree.Handle).MeasureString("RowHeight", MetaObjectsTree.Font).Height + 4;
            base.OnLoad(e);
        }

        /// <MetaDataID>{24474ee8-9145-4c05-aac9-64adf1470c5a}</MetaDataID>
        void OnClassORM(OOAdvantech.UserInterface.MenuCommandClickedEventArg menuCommandEventArgs)
        {
            VSPackage.ShowORMToolWindow((menuCommandEventArgs.Sender as MetaObjectTreeNode).MetaObject);
            
        }

        void OnObjectStateCodeCodeUpdate(OOAdvantech.UserInterface.MenuCommandClickedEventArg menuCommandEventArgs)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if ((menuCommandEventArgs.Sender as MetaObjectTreeNode).MetaObject is OOAdvantech.CodeMetaDataRepository.Class)
                    ((menuCommandEventArgs.Sender as MetaObjectTreeNode).MetaObject as OOAdvantech.CodeMetaDataRepository.Class).UpdateObjectStateCode();
                if ((menuCommandEventArgs.Sender as MetaObjectTreeNode).MetaObject is OOAdvantech.CodeMetaDataRepository.Project)
                    ((menuCommandEventArgs.Sender as MetaObjectTreeNode).MetaObject as OOAdvantech.CodeMetaDataRepository.Project).UpdateObjectStateCode();

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        EnvDTE.SolutionEvents SolutionEvents;
        EnvDTE.DTEEvents DTEEvents;
        /// <MetaDataID>{17b575aa-2b3a-4720-9b20-c3b870a650e2}</MetaDataID>
        EnvDTE.DTE _DTE;
        /// <MetaDataID>{ee7133d9-c1e1-4e55-9b97-f032f9b2c85d}</MetaDataID>
        public EnvDTE.DTE DTE
        {
            get
            { 
                return _DTE;
                 
            }
            set
            {
                if (_DTE != value)
                {
                    _DTE = value;
                    DTEEvents = _DTE.Events.DTEEvents;
                    DTEEvents.OnBeginShutdown += new EnvDTE._dispDTEEvents_OnBeginShutdownEventHandler(OnBeginShutdown);

                    SolutionEvents = _DTE.Events.SolutionEvents;
                    SolutionEvents.Opened += new EnvDTE._dispSolutionEvents_OpenedEventHandler(SolutionOpened);
                    SolutionEvents.BeforeClosing += new EnvDTE._dispSolutionEvents_BeforeClosingEventHandler(OnBeforeClosing);  
                    SolutionEvents.AfterClosing += new EnvDTE._dispSolutionEvents_AfterClosingEventHandler(SolutionAfterClosing);
                    try
                    {

                        OOAdvantech.CodeMetaDataRepository.Solution solution = IDEManager.Solution;
                        if (solution != null)
                            Connection.Instance = new SolutionTreeNode(solution,null);

                    }
                    catch (System.Exception error)
                    {

                    }

                }



            }
        }

        /// <MetaDataID>{e0d32b82-ccfe-46e4-9f1e-f6f8ef16bd94}</MetaDataID>
        void OnBeginShutdown()
        {
            
        }

        /// <MetaDataID>{5d7a4177-c3fd-478a-bd58-8faf06a54406}</MetaDataID>
        void OnBeforeClosing()
        {
            Connection.Instance = null;
        }



        /// <MetaDataID>{a0ca191f-6fd4-4232-9c31-1a7192859780}</MetaDataID>
        void SolutionAfterClosing()
        {
            
        }

        /// <MetaDataID>{fbddf721-5e48-45e3-ab66-9dca5896f0c7}</MetaDataID>
        void SolutionOpened()
        {
            try
            {
                
                OOAdvantech.CodeMetaDataRepository.Solution solution = IDEManager.Solution;
                if (solution != null)
                    Connection.Instance = new SolutionTreeNode(solution,null);


            }
            catch (System.Exception error)
            {

            }

            
        }

    }
    /// <MetaDataID>{3319daca-28a4-4ba6-9fad-513c245f9c17}</MetaDataID>
    public interface IVSPackage
    {
        /// <MetaDataID>{2bc92c40-b872-4922-bf19-53fe4a513200}</MetaDataID>
        void ShowORMToolWindow(OOAdvantech.MetaDataRepository.MetaObject metaObject);
    }

}
