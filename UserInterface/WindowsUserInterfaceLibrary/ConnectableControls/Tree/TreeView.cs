using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using System.Drawing.Design;
using ConnectableControls.Tree.NodeControls;
using ConnectableControls.PropertyEditors;
using OOAdvantech.UserInterface.Runtime;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{52e1d04d-18fa-4cdd-b0d6-e4ed5a766bc1}</MetaDataID>
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ViewControlObject), "TreeView.bmp")]
    public partial class TreeView : Control, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer, OOAdvantech.UserInterface.Runtime.ICutCopyPasteMoveDragDrop
    {
        /// <MetaDataID>{a790bfd3-3e4b-45b7-b29c-50cb148a8573}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }

        /// <MetaDataID>{c3a231da-bc13-4061-92da-b847647f91f4}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{4e7ec6cc-64d6-4102-a8ba-6091432b5fd4}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{3bc793c8-974e-4928-a8f2-38436860e2ae}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{db071225-8515-474d-b9b8-ef43fb3c132a}</MetaDataID>
        DependencyProperty _EnabledProperty;
        /// <MetaDataID>{34416fbb-479a-465f-8450-461080ba1a3b}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public DependencyProperty EnabledProperty
        {
            get
            {

                return _EnabledProperty;
            }
            set
            {
                if (value != null)
                {
                    _EnabledProperty = value;
                    _EnabledProperty.ConnectableControl = this;
                }
            }
        }

        /// <MetaDataID>{ab0f99b9-c2b5-4ec0-9ff1-4f563db9f906}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }
        /// <MetaDataID>{b05ec6b5-c292-4f3b-8dc5-b30305623dfe}</MetaDataID>
        public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        {
            LoadControlValues();

        }
        /// <MetaDataID>{9292f233-81ab-4b56-b731-0c2cf3e655ad}</MetaDataID>
        public void LockStateChange(object sender)
        {
         
        }



        /// <MetaDataID>{ad879563-b130-4100-ae3a-936023798ea4}</MetaDataID>
        XDocument MetaDataAsXmlDocument;

        /// <MetaDataID>{e39c6fee-7a36-4ad2-a99c-2b70934da0b7}</MetaDataID>
        OOAdvantech.UserInterface.TreeView _TreeViewMetaData;

        /// <MetaDataID>{2ef0168e-6e35-468c-be80-0d1b9d177509}</MetaDataID>
        OOAdvantech.UserInterface.TreeView TreeViewMetaData
        {
            get
            {
                if (_TreeViewMetaData == null)
                {
                    var metadata = MetaData;
                }
                return _TreeViewMetaData;
            }
        }

        /// <MetaDataID>{83C40642-200F-4371-9075-39C1CB7F5785}</MetaDataID>
        private object _MetaData;

        /// <MetaDataID>{5ED59BC4-4269-41B8-B1EE-C5262355FFED}</MetaDataID>
        [Editor(typeof(EditListMetaData), typeof(System.Drawing.Design.UITypeEditor)),
        Category("Object Model Connection")]
        public Object MetaData
        {
            get
            {
                if (MetaDataAsXmlDocument == null)
                {
                    MetaDataAsXmlDocument = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _TreeViewMetaData = storage.NewObject(typeof(OOAdvantech.UserInterface.TreeView)) as OOAdvantech.UserInterface.TreeView;

                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = new UserInterfaceMetaData.MetaDataValue(MetaDataAsXmlDocument.ToString(), "(Tree MetaData)");
                metaDataVaue.MetaDataAsObject = this;



                return metaDataVaue;
            }
            set
            {

                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;



                if (MetaDataAsXmlDocument == null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    MetaDataAsXmlDocument = new XDocument();
                    try
                    {

                        if (!string.IsNullOrEmpty(metaData))
                            MetaDataAsXmlDocument=XDocument.Parse(metaData);

                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            MetaDataAsXmlDocument = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                            _TreeViewMetaData = storage.NewObject(typeof(OOAdvantech.UserInterface.TreeView)) as OOAdvantech.UserInterface.TreeView;
                        }

                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        MetaDataAsXmlDocument = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        _TreeViewMetaData = storage.NewObject(typeof(OOAdvantech.UserInterface.TreeView)) as OOAdvantech.UserInterface.TreeView;
                    }
                    try
                    {

                        OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT treeView FROM OOAdvantech.UserInterface.TreeView treeView ");
                        foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                        {
                            _TreeViewMetaData = setInstance["treeView"] as OOAdvantech.UserInterface.TreeView;
                            break;
                        }
                        if (_TreeViewMetaData == null)
                            _TreeViewMetaData = storage.NewObject(typeof(OOAdvantech.UserInterface.TreeView)) as OOAdvantech.UserInterface.TreeView;
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }

                    foreach (OOAdvantech.UserInterface.Column column in TreeViewMetaData.Columns)
                    {

                    }
                    return;


                }
            }
        }


        #region Menu code

        /// <MetaDataID>{E950000D-2BB8-4217-BC9E-242747F0DE1B}</MetaDataID>
        ConnectableControls.Menus.MenuCommand _Menu = null;

        /// <MetaDataID>{68f3cd76-52d8-443c-82a2-ae0d56fc48e0}</MetaDataID>
        OOAdvantech.UserInterface.MenuCommand NodeMenu;
        /// <MetaDataID>{dc56c938-a977-47b0-b3c1-d6af386651ab}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectableControls.Menus.MenuCommand Menu
        {
            get
            {
                if (_Menu == null)
                {
                    object metadata = MetaData;//Load meta data creates list view
                    _Menu = new ConnectableControls.Menus.MenuCommand(TreeViewMetaData.Menu, this);
                    if (Site == null || !Site.DesignMode)
                    {
                        //foreach (ConnectableControls.Menus.MenuCommand menuCommand in _Menu.GetAllMenuCommands())
                        //    menuCommand.Click += new EventHandler(MenuCommandClicked);


                    }
                }
                return _Menu;


            }
            set
            {
            }
        }

        /// <MetaDataID>{605e400b-a1e4-48b8-8c14-1ddeab33eca3}</MetaDataID>
        void MenuCommandClicked(object sender, EventArgs e)
        {
            ConnectableControls.Menus.MenuCommand menucommand = sender as ConnectableControls.Menus.MenuCommand;
            if (TreeViewMetaData.InsertMenuCommand!=null && menucommand.Command.CommandID == TreeViewMetaData.InsertMenuCommand.CommandID)
            {
                InserNode();
                return;
            }
            if (TreeViewMetaData.InsertMenuCommand!=null&& menucommand.Command.CommandID == TreeViewMetaData.DeleteMenuCommand.CommandID)
            {

                foreach (TreeNode node in new ArrayList(SelectedNodes))
                    DeleteNode(node);
                UpdateSelection();
                SmartFullUpdate();
                return;
            }
            if (menucommand.OnCommandOperationCaller == null)
                menucommand.Command.MenuClicked(GetPropertyValue("SelectedNodeValue"));


            //throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{f5f9c63e-359d-4f53-82a7-e0d113dbdd2f}</MetaDataID>
        private void DeleteNode(TreeNode node)
        {
            if (DeleteNodeOperationCaller == null)
                return;
            if (DeleteNodeOperationCaller.Operation != null)
            {
                object reVal = DeleteNodeOperationCaller.Invoke();
                if (reVal == null || !(reVal is bool))
                {
                    TreeNode parent = node.Parent;
                    //parent.Nodes.Remove(node);
                    //node.Parent = null;

                    UserInterfaceObjectConnection.RemoveCollectionObject((parent.Tag as NodeDisplayedObject).Value, ValueType, SubNodesProperty as string, (node.Tag as NodeDisplayedObject).Value);
                }
                else if ((bool)reVal)
                {
                    TreeNode parent = node.Parent;
                    //parent.Nodes.Remove(node);
                    //node.Parent = null;

                    UserInterfaceObjectConnection.RemoveCollectionObject((parent.Tag as NodeDisplayedObject).Value, ValueType, SubNodesProperty as string, (node.Tag as NodeDisplayedObject).Value);
                }

            }

        }

        /// <MetaDataID>{3720199f-b32c-4331-a843-255382f18417}</MetaDataID>
        protected override void OnClick(EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
            {

                if (TreeViewMetaData!=null)
                {
                    ConnectableControls.Menus.PopupMenu popupMenu = new ConnectableControls.Menus.PopupMenu(false);

                    NodeMenu = new OOAdvantech.UserInterface.MenuCommand(TreeViewMetaData.Menu);

                    if (BeforeShowContextMenuOperationCaller!=null&&BeforeShowContextMenuOperationCaller.Operation != null&&BeforeShowContextMenuOperationCaller.Operation != null)
                        BeforeShowContextMenuOperationCaller.Invoke();

                    if (NodeMenu.SubMenuCommands.Count > 0)
                    {
                        ConnectableControls.Menus.MenuCommand menu = new ConnectableControls.Menus.MenuCommand(NodeMenu, this);
                        if (Site == null || !Site.DesignMode)
                        {
                            foreach (ConnectableControls.Menus.MenuCommand menuCommand in menu.GetAllMenuCommands())
                                menuCommand.Click += new EventHandler(MenuCommandClicked);

                        }
                        int returnDir = 0;
                        TreeNode selectedNode = SelectedNode;
                        //Menu.Command
                        popupMenu.TrackPopup(
                            Control.MousePosition,
                            Control.MousePosition,
                            ConnectableControls.Menus.Common.Direction.Horizontal,
                            menu,
                            0,
                            ConnectableControls.Menus.GapPosition.None, false, null, false, ref returnDir);

                        if (selectedNode != SelectedNode)
                            OnSelectionChanged();
                        if (Site == null || !Site.DesignMode)
                        {
                            foreach (ConnectableControls.Menus.MenuCommand menuCommand in menu.GetAllMenuCommands())
                                menuCommand.Click -= new EventHandler(MenuCommandClicked);

                        }

                    }




                }

            }
            base.OnClick(e);
        }

        /// <MetaDataID>{0933b1ba-0869-4cbc-b347-403802792d49}</MetaDataID>
        private ConnectableControls.Menus.MenuCommand _EditMenuCommand;
        /// <MetaDataID>{16094537-be05-484f-b39d-2b801d32881e}</MetaDataID>
        [Category("Tree Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.SelectMenuCommand), typeof(System.Drawing.Design.UITypeEditor))]
        public object EditMenuCommand
        {
            get
            {
                if (DesignMode)
                {
                    if (TreeViewMetaData.EditMenuCommand != null)
                        return new ConnectableControls.PropertyEditors.MenuSelection(TreeViewMetaData.EditMenuCommand.Name, Menu);
                    else
                        return new ConnectableControls.PropertyEditors.MenuSelection("None", Menu);
                }


                return _EditMenuCommand;
            }
            set
            {
                if (value is ConnectableControls.Menus.MenuCommand)
                {

                    OOAdvantech.UserInterface.MenuCommand editMenuCommand = (value as ConnectableControls.Menus.MenuCommand).Command;
                    TreeViewMetaData.EditMenuCommand = editMenuCommand;
                    if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                        !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    {
                        TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                    }

                }

            }
        }


        /// <MetaDataID>{6077f21c-61f4-4e36-ab6a-4ba330ce7bfe}</MetaDataID>
        private ConnectableControls.Menus.MenuCommand _DeleteMenuCommand;
        /// <MetaDataID>{bd46992d-8c6d-4b78-8afc-92b18fd974ae}</MetaDataID>
        [Category("Tree Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.SelectMenuCommand), typeof(System.Drawing.Design.UITypeEditor))]
        public object DeleteMenuCommand
        {
            get
            {
                if (DesignMode)
                {

                    if (TreeViewMetaData.DeleteMenuCommand != null)
                        return new ConnectableControls.PropertyEditors.MenuSelection(TreeViewMetaData.DeleteMenuCommand.Name, Menu);
                    else
                        return new ConnectableControls.PropertyEditors.MenuSelection("None", Menu);
                }


                return _DeleteMenuCommand;
            }
            set
            {
                if (value is ConnectableControls.Menus.MenuCommand)
                {

                    OOAdvantech.UserInterface.MenuCommand deleteMenuCommand = (value as ConnectableControls.Menus.MenuCommand).Command;
                    TreeViewMetaData.DeleteMenuCommand = deleteMenuCommand;
                    if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                        !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    {
                        TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                    }

                }

            }
        }


        /// <MetaDataID>{54bb15ed-b6f7-4c95-9851-4968d92c21d2}</MetaDataID>
        private ConnectableControls.Menus.MenuCommand _InsertMenuCommand;
        /// <MetaDataID>{b573113d-4d26-407d-a348-ec26ba716308}</MetaDataID>
        [Category("Tree Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.SelectMenuCommand), typeof(System.Drawing.Design.UITypeEditor))]
        public object InsertMenuCommand
        {
            get
            {
                if (DesignMode)
                {
                    if (TreeViewMetaData.InsertMenuCommand != null)
                        return new ConnectableControls.PropertyEditors.MenuSelection(TreeViewMetaData.InsertMenuCommand.Name, Menu);
                    else
                        return new ConnectableControls.PropertyEditors.MenuSelection("None", Menu);
                }


                return _InsertMenuCommand;
            }
            set
            {
                if (value is ConnectableControls.Menus.MenuCommand)
                {

                    OOAdvantech.UserInterface.MenuCommand insertMenuCommand = (value as ConnectableControls.Menus.MenuCommand).Command;
                    TreeViewMetaData.InsertMenuCommand = insertMenuCommand;
                    if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                        !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    {
                        TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                    }

                }

            }
        }


        /// <MetaDataID>{afed057b-906e-46f3-a965-767dc9b70a7e}</MetaDataID>
        [Editor(typeof(ConnectableControls.PropertyEditors.EditMenuMetadata), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Tree Menu")]
        public object DesignMenu
        {
            get
            {
                return Menu;
            }
            set
            {
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                if (Menu != null)
                    Menu.OwnerControl = this;

            }
        }




        /// <MetaDataID>{7373deb5-af62-40de-b474-ca85d290ba36}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _InsertNodeOperationCaller;
        /// <MetaDataID>{d96a1c96-ee0b-4151-83de-d8fc26207a9c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller InsertNodeOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.InsertOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_InsertNodeOperationCaller != null)
                    return _InsertNodeOperationCaller;
                _InsertNodeOperationCaller = new OperationCaller(TreeViewMetaData.InsertOperation, this);
                return _InsertNodeOperationCaller;
            }
        }

        /// <MetaDataID>{e8bc76e9-e166-449a-bac5-7bb607662bf6}</MetaDataID>
        object _InsertNodeOperationCall;
        /// <MetaDataID>{d4c0d003-f2f2-416b-929b-94258489dcdb}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object InsertNodeOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.InsertOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.InsertOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (InsertNodeOperationCaller == null || InsertNodeOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Insert Node operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, InsertNodeOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.InsertOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return error;
                }

            }
            set
            {
                //_InsertNodeOperation = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {
                    _InsertNodeOperationCaller = null;
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }



        /// <MetaDataID>{9c74f24b-d53f-48a6-8552-007598109106}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _EditNodeOperationCaller;
        /// <MetaDataID>{80b93884-6db6-4d66-ae5c-21505c5b0dac}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller EditNodeOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.EditOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_EditNodeOperationCaller != null)
                    return _EditNodeOperationCaller;
                _EditNodeOperationCaller = new OperationCaller(TreeViewMetaData.EditOperation, this);
                return _EditNodeOperationCaller;
            }
        }
        /// <MetaDataID>{76f42583-801c-4aff-baa2-607ac15268cc}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object EditNodeOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.EditOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.EditOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (EditNodeOperationCaller == null || EditNodeOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Edit row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, EditNodeOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.EditOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return error;
                }
            }
            set
            {
                _EditNodeOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }



        /// <MetaDataID>{4539d78f-7281-42fd-920a-45cf21f7cf4d}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DeleteNodeOperationCaller;
        /// <MetaDataID>{73161785-12b7-49d4-ad06-2d2b43ef2576}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller DeleteNodeOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.DeleteOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_DeleteNodeOperationCaller != null)
                    return _DeleteNodeOperationCaller;
                _DeleteNodeOperationCaller = new OperationCaller(TreeViewMetaData.DeleteOperation, this);
                return _DeleteNodeOperationCaller;
            }
        }

        //object _DeleteNodeOperationCall;
        /// <MetaDataID>{a122793e-a43b-4bdf-a6a0-c0a033c2de79}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object DeleteNodeOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.DeleteOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.DeleteOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (DeleteNodeOperationCaller == null || DeleteNodeOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Delete row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, DeleteNodeOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.DeleteOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return error;
                }
            }
            set
            {
                _DeleteNodeOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }







        #endregion


        /// <MetaDataID>{bfdb4b65-3bf2-4f42-85e9-dde608fa723c}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _BeforeShowContextMenuOperationCaller;
        /// <MetaDataID>{1bc03ef3-0829-4c7c-b500-c059194fefbf}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller BeforeShowContextMenuOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.BeforeShowContextMenuOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_BeforeShowContextMenuOperationCaller != null)
                    return _BeforeShowContextMenuOperationCaller;
                _BeforeShowContextMenuOperationCaller = new OperationCaller(TreeViewMetaData.BeforeShowContextMenuOperation, this);
                return _BeforeShowContextMenuOperationCaller;
            }
        }

        /// <MetaDataID>{977c6d88-3654-4c92-90c3-cd4f82e53505}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        [OOAdvantech.UserInterface.OperationCallTransactionOption(new OOAdvantech.Transactions.TransactionOption[] { OOAdvantech.Transactions.TransactionOption.Supported, OOAdvantech.Transactions.TransactionOption.Suppress })]
        public object BeforeShowContextMenuOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.BeforeShowContextMenuOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.BeforeShowContextMenuOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (BeforeShowContextMenuOperationCaller == null || BeforeShowContextMenuOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(LoadList row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, BeforeShowContextMenuOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.BeforeShowContextMenuOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return new UserInterfaceMetaData.MetaDataValue(error);
                }

            }
            set
            {
                //_InsertRowOperation = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {
                    _BeforeShowContextMenuOperationCaller = null;
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);

                }
                if (BeforeShowContextMenuOperationCaller != null && BeforeShowContextMenuOperationCaller.OperationCall != null)
                    BeforeShowContextMenuOperationCaller.OperationCall.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
                return;
            }
        }


        /// <MetaDataID>{c4221e32-e2c9-46db-af00-75ba42d38584}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        /// <exclude>Excluded</exclude>
        public int _RecursiveLoadSteps;
        /// <MetaDataID>{dc8db642-3e3c-4cfa-b97d-ebcec1425103}</MetaDataID>
        public int RecursiveLoadSteps
        {
            get
            {
                return _RecursiveLoadSteps;

            }
            set
            {
                _RecursiveLoadSteps = value;

            }
        }



        /// <MetaDataID>{9bd45a28-a11c-4489-b19d-ddffb944545c}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;
        /// <MetaDataID>{7e2720ab-8c4f-439d-aba6-7920dcbc85f2}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                if (_AllPaths == null)
                {

                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    if (RecursiveLoadSteps == 0 || string.IsNullOrEmpty(SubNodesProperty as string))
                    {
                        _AllPaths.Add(_Path);
                        if (!string.IsNullOrEmpty(_DisplayMember) && PresentationObjectType == ValueType)
                            _AllPaths.Add(_Path + "." + _DisplayMember);

                    }
                    else
                    {
                        _AllPaths.Add(_Path + ".Recursive(" + SubNodesProperty.ToString() + "," + (RecursiveLoadSteps).ToString() + ")");

                        if (!string.IsNullOrEmpty(_DisplayMember) && PresentationObjectType == ValueType)
                        {
                            _AllPaths.Add(_Path + ".Recursive(" + SubNodesProperty.ToString() + "," + (RecursiveLoadSteps).ToString() + ")" + "." + _DisplayMember);
                            _AllPaths.Add(_Path + "." + _DisplayMember);
                        }

                    }

                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);

                    return _AllPaths;
                }
                else
                    return _AllPaths;
            }
        }
        /// <MetaDataID>{3fdb9b55-0dbc-406e-a2a0-d32a60c4e17f}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {

                return new string[6] { "SelectedNodeValue", "DragDropObject", "DropMarkIndex", "CopyInsideObject", "DragObjectParent","NodeMenu" };

            }
        }
        /// <MetaDataID>{3e9fbd01-0fb1-423f-a14a-3a560ae1ee3a}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;


            if (propertyName == "NodeMenu")
                return NodeMenu;

            if (propertyName == "DragDropObject")
                return DragDropObject;
            if (propertyName == "CopyInsideObject")
            {
                if (!_dragMode)
                    return GetPropertyValue("SelectedNodeValue");

                if (DropPosition.Position == NodePosition.Inside)
                {
                    if (!(DragOverNode.Tag is NodeDisplayedObject))
                        return null;
                    return (DragOverNode.Tag as NodeDisplayedObject).Value;
                }

                if (DragOverNode.Parent == null)
                {
                    if (!(DragOverNode.Tag is NodeDisplayedObject))
                        return null;
                    return (DragOverNode.Tag as NodeDisplayedObject).Value;
                }

                if (DropPosition.Position == NodePosition.After || DropPosition.Position == NodePosition.Before)
                {
                    if (!(DragOverNode.Parent.Tag is NodeDisplayedObject))
                        return null;
                    return (DragOverNode.Parent.Tag as NodeDisplayedObject).Value;
                }
            }
            if (propertyName == "DragObjectParent")
            {
                if (!_dragSource)
                    return null;
                return DragObjectParent;
            }






            if (propertyName == "DropMarkIndex")
            {
                if (!_dragMode)
                    return -1;
                if (DropPosition.Position == NodePosition.Inside)
                    return 0;

                if (DragOverNode.Parent == null)
                    return 0;
                else
                {
                    int nodeIndex = DragOverNode.Parent.Nodes.IndexOf(DragOverNode);
                    if (DropPosition.Position == NodePosition.Before)
                        return nodeIndex;
                    else
                        return nodeIndex + 1;

                }
            }


            if (propertyName == "SelectedNodeValue")
            {
                if (SelectedNode != null && SelectedNode.Tag is NodeDisplayedObject)
                    return (SelectedNode.Tag as NodeDisplayedObject).Value;
                else
                    return null;
            }


            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "Value")
                return Value;
            throw new Exception("There isn't property with name " + propertyName + ".");

        }
        /// <MetaDataID>{b3855020-d929-43ef-ad4b-f9e0e69614f0}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "DragObjectParent")
                return true;
            if (propertyName == "CopyInsideObject")
                return true;
            if (propertyName == "DropMarkIndex")
                return true;

            if (propertyName == "this")
                return true;
            if (propertyName == "DragDropObject")
                return true;

            if (propertyName == "Value")
                return true;

            return false;
        }
        /// <MetaDataID>{b8995ff4-ba5f-48e8-9da4-e460b72e39a5}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }
        /// <MetaDataID>{00d724ca-f4ef-475f-ad6b-c13542b74ad4}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {

            if (propertyName == "CopyInsideObject")
                return ValueType;
            if (propertyName == "DragObjectParent")
                return ValueType;


            if (propertyName == "NodeMenu" )
                return UserInterfaceObjectConnection.GetClassifier(typeof(OOAdvantech.UserInterface.MenuCommand));



            if (propertyName == "SelectedNodeValue")
                return ValueType;
            if (propertyName == "DragDropObject")
                return UserInterfaceObjectConnection.GetClassifier(typeof(System.Object));

            if (propertyName == "DropMarkIndex")
                return UserInterfaceObjectConnection.GetClassifier(typeof(int));

            return null;

        }


        /// <MetaDataID>{28e0e658-3b36-4a26-85d5-a19ba446ad86}</MetaDataID>
        string _SubNodesProperty;
        /// <MetaDataID>{3d34e044-06d5-4182-a5cb-89de4f6b4a3b}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
        public object SubNodesProperty
        {
            get
            {
                return _SubNodesProperty;
            }
            set
            {
                if (value is string)
                    _SubNodesProperty = value as string;
                else if (value is MetaData)
                    _SubNodesProperty = (value as MetaData).Path;

            }
        }

        /// <MetaDataID>{3caf32e4-5c8c-438a-98f1-df0d881be958}</MetaDataID>
        string NodeObjectTypeFullName;
        /// <MetaDataID>{edfa2140-c227-4568-bd18-6e051a5ca9dc}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object NodeObjectType
        {
            get
            {
                try
                {
                    if (ValueType != null)
                        return ValueType.FullName;
                    else
                        return NodeObjectTypeFullName;
                }
                catch (System.Exception error)
                {
                    return "";

                }
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Classifier)
                    NodeObjectTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                if (value is ConnectableControls.PropertyEditors.MetaData
                    && (value as ConnectableControls.PropertyEditors.MetaData).MetaObject is OOAdvantech.MetaDataRepository.Classifier)
                {

                    NodeObjectTypeFullName = (value as ConnectableControls.PropertyEditors.MetaData).MetaObject.FullName;
                    if (string.IsNullOrEmpty(_SubNodesProperty))
                        _SubNodesProperty = GetFirstCandidateSubNodesProperty((value as ConnectableControls.PropertyEditors.MetaData).MetaObject as OOAdvantech.MetaDataRepository.Classifier);

                }
                if (value is string)
                    NodeObjectTypeFullName = (value as string);
            }
        }


        #region IObjectMemberViewControl Members


        /// <MetaDataID>{f00f77d2-1996-4616-a341-7b03dceb95fe}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {

            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && ValueType == null)
            {
                errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TreeView '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                hasErrors = true;
            }
            if (InsertNodeOperationCaller != null)
            {
                foreach (string error in InsertNodeOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TreeView '" + Name + ".InsertNodeOperationCall' " + error, FindForm().GetType().FullName));
                }
            }
            if (DeleteNodeOperationCaller != null)
            {
                foreach (string error in DeleteNodeOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TreeView '" + Name + ".DeleteNodeOperationCall' " + error, FindForm().GetType().FullName));
                }
            }
            if (EditNodeOperationCaller != null)
            {
                foreach (string error in EditNodeOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TreeView '" + Name + ".EditNodeOperationCall' " + error, FindForm().GetType().FullName));
                }
            }



            if (Menu != null)
            {
                hasErrors |= Menu.ErrorCheck(ref errors);
            }

            if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
            {
                _PresentationObjectType = ViewControlObject.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TreeView '" + Name + "' has invalid PresentationObject.", FindForm().GetType().FullName));
                else
                {
                    if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(_PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType))
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TreeView '" + Name + "' You can't assign the '" + _PresentationObjectType.FullName + "' to the PresentationObjectType.\n Check the property rules.", FindForm().GetType().FullName));
                }
            }

            return hasErrors;
        }

        /// <MetaDataID>{96bc1171-3fa0-413b-9017-979965d124d9}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {

            if (context.PropertyDescriptor.Name == "SubNodesProperty")
                return ValueType;
            if ((context.PropertyDescriptor.Name == "Path" || context.PropertyDescriptor.Name == "AssignPresentationObjectType") && UserInterfaceObjectConnection != null)
            {
                if (context.PropertyDescriptor.Name == "Path")
                    return UserInterfaceObjectConnection.PresentationObjectType;
                else
                    return AssemblyManager.GetActiveWindowProject();


            }
            if (context.PropertyDescriptor.Name == "NodeObjectType")
                return AssemblyManager.GetActiveWindowProject();

            if (context.PropertyDescriptor.Name == "DisplayMember")
                return PresentationObjectType;

            if (context.PropertyDescriptor.Name == "ImagePath")
                return PresentationObjectType;
            if (context.PropertyDescriptor.Name == "CheckUncheckPath")
                return PresentationObjectType;

            if (context.PropertyDescriptor.Name == "SelectionMember")
                return UserInterfaceObjectConnection.PresentationObjectType;
            return null;

        }
        /// <MetaDataID>{1a9a941f-47a8-4eca-9fc8-dd21e857a6a1}</MetaDataID>
        string _SelectionMember;
        /// <MetaDataID>{e43c6c8e-f1f3-4a22-a0dd-1c334992fe03}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object SelectionMember
        {
            get
            {
                return _SelectionMember;
            }
            set
            {
                if (value is string)
                    _SelectionMember = value as string;
                else if (value is MetaData)
                    _SelectionMember = (value as MetaData).Path;
            }
        }
        /// <MetaDataID>{3204217a-5463-4f85-b9ca-1a204d1241e9}</MetaDataID>
        [ObsoleteAttribute("This property will be removed from future Versions.Use another property 'UpdateStyle'", false)]
        public bool ConnectedObjectAutoUpdate
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        /// <MetaDataID>{665b4299-3079-49b7-b5a0-57c4b578beac}</MetaDataID>
        public UpdateStyle UpdateStyle
        {
            get
            {
                return UpdateStyle.Immediately;
            }
            set
            {
                

            }
        }

        /// <MetaDataID>{8a70d1b7-dfd7-41d6-909c-e1d504e83915}</MetaDataID>
        object _Value;
        /// <MetaDataID>{e39884ac-ea5c-40d8-8d7f-a0e7886b79df}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }




        /// <MetaDataID>{2749a604-5589-409e-9c0e-43ce0ef76886}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier PresentationObjectType
        {
            get
            {
                if (ValueType == null)
                    return null;
                if (_PresentationObjectType != null)
                    return _PresentationObjectType;
                else
                {
                    if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
                    {
                        _PresentationObjectType = this.ViewControlObject.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                        return _PresentationObjectType;
                    }
                    return ValueType;
                }

            }
        }
        /// <MetaDataID>{f9184428-ec52-410d-b7ff-eb2c664a1fe2}</MetaDataID>
        string PresentationObjectTypeFullName;
        /// <MetaDataID>{541362bc-5f9e-4f03-b679-802844d9b3f2}</MetaDataID>
        OOAdvantech.MetaDataRepository.Class _PresentationObjectType;
        /// <MetaDataID>{259181ab-4398-499b-9c1f-76d93dc2ee2a}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object AssignPresentationObjectType
        {
            get
            {
                if (_PresentationObjectType != null)
                    return _PresentationObjectType.FullName;

                return PresentationObjectTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Class)
                    _PresentationObjectType = value as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType != null)
                    PresentationObjectTypeFullName = _PresentationObjectType.FullName;
                if (value is string)
                    PresentationObjectTypeFullName = value as string;
                if (value is MetaData)
                {
                    PresentationObjectTypeFullName = (value as MetaData).MetaObject.FullName;
                    _PresentationObjectType = (value as MetaData).MetaObject as OOAdvantech.MetaDataRepository.Class;


                }
                if (value == null || (value is string && string.IsNullOrEmpty(value as string)))
                {
                    PresentationObjectTypeFullName = "";
                    _PresentationObjectType = null;
                }

            }
        }

        /// <MetaDataID>{935f99f7-08bd-4170-a151-856b23a228c2}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                if (String.IsNullOrEmpty(NodeObjectTypeFullName))
                    return null;
                else
                    return ViewControlObject.UserInterfaceObjectConnection.GetClassifier(NodeObjectTypeFullName, true);

            }
            set
            {

            }
        }


        /// <MetaDataID>{db4cab1d-529f-46e7-8b89-a2fbd5caae71}</MetaDataID>
        string _Path;
        /// <MetaDataID>{e2a5efb4-e5e9-48b3-a5e4-19068fec1f67}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
        public object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (value is string)
                    _Path = value as string;
                else if (value is MetaData)
                    _Path = (value as MetaData).Path;

            }
        }

        /// <MetaDataID>{130c2ea4-f788-4e60-be57-df7cdcc2cee9}</MetaDataID>
        string _DisplayMember;
        /// <summary>
        /// In case where the search box is control for member which is object 
        /// and the object is not value type, the search display the return value of ToString() function call.
        /// If you want to display a member of object use this property to choose the member.
        ///  </summary>
        /// <MetaDataID>{65015d2e-0bd8-4a72-bdac-c5e6b8ff5df4}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("When there is display member, the control presents the member of value object, otherwise presents the value object with the assistance of ToString method."),
        Category("Object Model Connection")]
        public object DisplayMember
        {
            get
            {
                return _DisplayMember;
            }
            set
            {
                if (value is string)
                    _DisplayMember = value as string;
                else if (value is MetaData)
                    _DisplayMember = (value as MetaData).Path;
            }
        }
        /// <MetaDataID>{c6276523-a312-4cbb-89bf-f0a1fcb7d639}</MetaDataID>
        string _CheckUncheckPath;
        /// <MetaDataID>{ccc8af97-8111-4539-b4d3-03fc1fb10a1d}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
        public object CheckUncheckPath
        {
            get
            {
                return _CheckUncheckPath;
            }
            set
            {
                if (value is string)
                    _CheckUncheckPath = value as string;
                else if (value is MetaData)
                    _CheckUncheckPath = (value as MetaData).Path;
            }
        }


        /// <MetaDataID>{95d2988a-739e-44a3-a477-e5fd726e6481}</MetaDataID>
        string _ImagePath;
        /// <MetaDataID>{8a572882-5e30-4445-b9d3-b052bf8cb928}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
        public object ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                if (value is string)
                    _ImagePath = value as string;
                else if (value is MetaData)
                    _ImagePath = (value as MetaData).Path;
            }
        }



        /// <MetaDataID>{770c4acf-3401-4fd8-a74c-225fcde9dcab}</MetaDataID>
        public void LoadControlValues()
        {

            if (NodeControls.Count == 0)
            {
                ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox = new ConnectableControls.Tree.NodeControls.NodeTextBox();
                NodeControls.Add(nodeTextBox);
            }
            

            TreeModel treeModel = new TreeModel(this);
            Model = treeModel;
            //  OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path, null);
            // treeModel.Nodes.Add(new Node(displayedValue,this));

        }
        /// <MetaDataID>{f90517b6-3f87-4188-97cf-fcde94c966bb}</MetaDataID>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }

        /// <MetaDataID>{db6a2224-b068-4cfd-a282-11732fb7046f}</MetaDataID>
        public void SaveControlValues()
        {
            
        }

        #endregion

        #region IConnectableControl Members

        /// <MetaDataID>{ac939fe7-f343-4632-b4f9-b8aa748c6056}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection = null;
        /// <MetaDataID>{d90f61a9-a94c-4bc1-a657-1223face417c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
            set
            {
                _UserInterfaceObjectConnection = value;
                if (_UserInterfaceObjectConnection != null)
                    _UserInterfaceObjectConnection.AddControlledComponent(this);
            }
        }
        /// <MetaDataID>{41745915-a6f6-4d69-93bc-8e9c9f337ece}</MetaDataID>
        [Category("Object Model Connection")]
        public ViewControlObject ViewControlObject
        {
            get
            {
                if (UserInterfaceObjectConnection == null)
                    return null;
                return UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject;
            }
            set
            {
                if (value != null)
                    UserInterfaceObjectConnection = value.UserInterfaceObjectConnection;
                else
                    UserInterfaceObjectConnection = null;
            }
        }

        /// <MetaDataID>{57cd20ba-5845-4098-81f8-bf2da15bd1b2}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object UIMetaDataObject
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        /// <MetaDataID>{3383dc00-e033-48a9-8959-cd77589ad74a}</MetaDataID>
        bool IsCandidateSubNodesProperty(OOAdvantech.MetaDataRepository.MetaObject member, OOAdvantech.MetaDataRepository.Classifier nodeObjectType)
        {
            OOAdvantech.MetaDataRepository.Classifier collectionClassifier = null;
            if (member is OOAdvantech.MetaDataRepository.Attribute)
                collectionClassifier = (member as OOAdvantech.MetaDataRepository.Attribute).Type;

            if (member is OOAdvantech.MetaDataRepository.AttributeRealization)
                collectionClassifier = (member as OOAdvantech.MetaDataRepository.AttributeRealization).Specification.Type;
            if (member is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                collectionClassifier = (member as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.CollectionClassifier;
            if (member is OOAdvantech.MetaDataRepository.AssociationEnd)
                collectionClassifier = (member as OOAdvantech.MetaDataRepository.AssociationEnd).CollectionClassifier;


            if (collectionClassifier != null)
            {
                if (collectionClassifier != null && collectionClassifier.TemplateBinding != null)
                {

                    foreach (OOAdvantech.MetaDataRepository.Operation operation in collectionClassifier.GetOperations("GetEnumerator"))
                    {
                        OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                        if (enumerator.Name.IndexOf("IEnumerator`1") == 0
                            && (enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier) == nodeObjectType)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;


        }
        /// <MetaDataID>{35b3d6a7-64ad-48d3-952e-6d99a82bdaaa}</MetaDataID>
        string GetFirstCandidateSubNodesProperty(OOAdvantech.MetaDataRepository.Classifier type)
        {


            foreach (OOAdvantech.MetaDataRepository.Feature feature in type.Features)
            {
                if (IsCandidateSubNodesProperty(feature, type))
                    return feature.Name;
            }
            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in type.GetAssociateRoles(false))
            {
                if (IsCandidateSubNodesProperty(associationEnd, type))
                    return associationEnd.Name;
            }
            return null;

        }
        /// <MetaDataID>{77bc82d3-e64b-4a5f-add1-14354f7f0e84}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {


            if ((metaObject is OOAdvantech.UserInterface.OperationCall ) &&
            (propertyDescriptor == "AllowDropOperationCall" || 
            propertyDescriptor == "DragDropOperationCall" ||
            propertyDescriptor == "InsertNodeOperationCall" || 
            propertyDescriptor == "EditNodeOperationCall" || 
            propertyDescriptor == "DeleteNodeOperationCall" || 
            propertyDescriptor == "CutOperationCall"||
            propertyDescriptor == "BeforeShowContextMenuOperationCall"))
            {
                if (new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                    return true;
                else
                    return false;


                //if (propertyDescriptor == "AllowDropOperationCall")
                //{
                //    if (operation.ReturnType == null || operation.ReturnType.FullName != typeof(OOAdvantech.DragDropMethod).FullName)
                //        return false;
                //    if (operation.Parameters.Count != 1)
                //        return false;
                //    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                //    {
                //        if (parameter.Type == null || parameter.Type.FullName != typeof(object).FullName)
                //            return false;
                //        else
                //            return true;
                //    }
                //    return false;
                //}
                //return true;
            }

            if (propertyDescriptor == "SubNodesProperty")
                return IsCandidateSubNodesProperty(metaObject, ValueType);

            if (propertyDescriptor == "NodeObjectType")
            {
                if (metaObject is OOAdvantech.MetaDataRepository.Classifier)
                {
                    if (!string.IsNullOrEmpty(GetFirstCandidateSubNodesProperty(metaObject as OOAdvantech.MetaDataRepository.Classifier)))
                        return true;
                }
            }
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute && propertyDescriptor == "DisplayMember")
                return true;
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute && propertyDescriptor == "ImagePath")
                return true;
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute && propertyDescriptor == "CheckUncheckPath")
                return true;


            
            //TODO θα πρέπει να ενημερώνεται ο χρήστης γιατί δέν μπορεί να πάρει την τιμή
            if (propertyDescriptor == "Path")
            {
                if (ValueType == null)
                {
                    System.Windows.Forms.MessageBox.Show("Set NodeObjectType property first","TreeView");
                    return false;
                }
                if (metaObject is OOAdvantech.MetaDataRepository.Attribute
                    && (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type != null
                    && (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.IsA(ValueType))
                {
                    return true;
                }
                if (IsCandidateSubNodesProperty(metaObject, ValueType))
                    return true;

            }
            if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.Operation)
            {
                return true;
            }
            if (metaObject is OOAdvantech.MetaDataRepository.Feature || propertyDescriptor == "SelectionMember")
                return true;
                
            
            if (metaObject is OOAdvantech.MetaDataRepository.Class && propertyDescriptor == "AssignPresentationObjectType")
            {
                if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(metaObject as OOAdvantech.MetaDataRepository.Class, ValueType))
                {
                    System.Windows.Forms.MessageBox.Show("You can't assign the '" + metaObject.FullName + "' to the PresentationObjectType.\n Check the property rules.");
                    return false;
                }
                else
                    return true;
            }
            return false;
        }

        /// <MetaDataID>{513e1d37-9ea0-45cb-87d5-bd8f3ea382ba}</MetaDataID>
        public bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return false;
        }

        #endregion


        #region Inner Classes

        private struct NodeControlInfo
        {
            public static readonly NodeControlInfo Empty = new NodeControlInfo();

            private NodeControl _control;
            public NodeControl Control
            {
                get { return _control; }
            }

            private Rectangle _bounds;
            public Rectangle Bounds
            {
                get { return _bounds; }
            }

            public NodeControlInfo(NodeControl control, Rectangle bounds)
            {
                _control = control;
                _bounds = bounds;
            }
        }

        private class ExpandedNode
        {
            private object _tag;
            public object Tag
            {
                get { return _tag; }
                set { _tag = value; }
            }

            private Collection<ExpandedNode> _children = new Collection<ExpandedNode>();
            public Collection<ExpandedNode> Children
            {
                get { return _children; }
                set { _children = value; }
            }
        }

        #endregion

        /// <MetaDataID>{1421fb33-9ea9-404d-a387-9d342c17b265}</MetaDataID>
        private const int TopMargin = 0;
        /// <MetaDataID>{aa7188e1-09d7-48df-9fe2-d5ee2f8d3af0}</MetaDataID>
        private const int LeftMargin = 7;
        /// <MetaDataID>{9f23b0ce-56a8-4916-9b67-1c4f495efb7b}</MetaDataID>
        private const int ItemDragSensivity = 4;
        /// <MetaDataID>{6d86dbd3-bf34-4f64-991d-ac0244f79704}</MetaDataID>
        private readonly int _columnHeaderHeight;
        /// <MetaDataID>{7e465596-a2a2-4d3b-89b5-6ce9306c7144}</MetaDataID>
        private const int DividerWidth = 9;

        /// <MetaDataID>{045e4250-344d-46c9-9139-20527c22f81d}</MetaDataID>
        private int _offsetX;
        /// <MetaDataID>{2562324e-ca7b-4aac-928b-37f806f13b0f}</MetaDataID>
        private int _firstVisibleRow;
        /// <MetaDataID>{69244776-0194-481d-ad8d-d53d14ee6d3f}</MetaDataID>
        private ReadOnlyCollection<TreeNode> _readonlySelection;
        /// <MetaDataID>{a65e1852-68ad-4335-8f86-e41f662e4ed5}</MetaDataID>
        private Pen _linePen;
        /// <MetaDataID>{d60dfa3a-2e33-4e86-ba58-782807702001}</MetaDataID>
        private Pen _markPen;
        /// <MetaDataID>{641b4832-6240-457f-a6e8-9eee95fad837}</MetaDataID>
        private bool _dragMode;
        /// <MetaDataID>{d888ca14-dc55-48a9-ad96-34d25e73584b}</MetaDataID>
        private bool _dragSource;
        /// <MetaDataID>{529d1232-9d6d-4146-b207-fbc006743cd8}</MetaDataID>
        private bool _suspendUpdate;
        /// <MetaDataID>{754745ad-1161-41e5-a2e4-c99abab1729e}</MetaDataID>
        private bool _structureUpdating;
        /// <MetaDataID>{6eb35320-ee7a-43c5-8e49-d5ff5f381e9a}</MetaDataID>
        private bool _needFullUpdate;
        /// <MetaDataID>{25acd010-c4e9-41ce-a3df-5a68abeb7699}</MetaDataID>
        private bool _fireSelectionEvent;
        /// <MetaDataID>{32c70cf0-0bb3-43f0-b11a-ed70ff482ffc}</MetaDataID>
        private NodePlusMinus _plusMinus;
        /// <MetaDataID>{604f7470-afbd-417a-9574-51ab6ec1b86a}</MetaDataID>
        private Control _currentEditor;
        /// <MetaDataID>{b89aea4b-708d-4030-86d2-d76d51fe24c2}</MetaDataID>
        private EditableControl _currentEditorOwner;
        /// <MetaDataID>{d5c503c3-ac32-46fa-be3e-571dd7f3d070}</MetaDataID>
        private ToolTip _toolTip;

        #region Internal Properties

        /// <MetaDataID>{50229dda-4a52-4479-aec9-d82617d4e884}</MetaDataID>
        private int ColumnHeaderHeight
        {
            get
            {
                if (UseColumns)
                    return _columnHeaderHeight;
                else
                    return 0;
            }
        }

        /// <summary>
        /// returns all nodes, which parent is expanded
        /// </summary>
        /// <MetaDataID>{017c913a-3706-4959-8b9b-f9b614d1220a}</MetaDataID>
        private IEnumerable<TreeNode> ExpandedNodes
        {
            get
            {
                if (_root.Nodes.Count > 0)
                {
                    TreeNode node = _root.Nodes[0];
                    while (node != null)
                    {
                        yield return node;
                        if (node.IsExpanded && node.Nodes.Count > 0)
                            node = node.Nodes[0];
                        else if (node.NextNode != null)
                            node = node.NextNode;
                        else
                            node = node.BottomNode;
                    }
                }
            }
        }

        /// <MetaDataID>{b1c20d0f-d4d1-49c2-b49c-0406f4fcaefa}</MetaDataID>
        private bool _suspendSelectionEvent;
        /// <MetaDataID>{e670bcaf-196c-498b-b53a-9c706d494473}</MetaDataID>
        internal bool SuspendSelectionEvent
        {
            get { return _suspendSelectionEvent; }
            set
            {
                _suspendSelectionEvent = value;
                if (!_suspendSelectionEvent && _fireSelectionEvent)
                    OnSelectionChanged();
            }
        }

        /// <MetaDataID>{0a43f7c1-7c4a-48e9-91b6-e4404155e035}</MetaDataID>
        private List<TreeNode> _rowMap;
        /// <MetaDataID>{730e5291-83b2-440c-9f8d-e916d9b5b30e}</MetaDataID>
        internal List<TreeNode> RowMap
        {
            get { return _rowMap; }
        }

        /// <MetaDataID>{7d282002-659e-4af6-8a88-b1e7f7015bb4}</MetaDataID>
        private TreeNode _selectionStart;
        /// <MetaDataID>{ee433094-e2fb-46e4-a0f0-24da8432ee73}</MetaDataID>
        internal TreeNode SelectionStart
        {
            get { return _selectionStart; }
            set { _selectionStart = value; }
        }

        /// <MetaDataID>{c14d9c5f-f623-464a-9cbe-b88a2f892d60}</MetaDataID>
        private InputState _input;
        /// <MetaDataID>{6fda8c8f-e888-4d49-a9fc-640b2f7fae9c}</MetaDataID>
        internal InputState Input
        {
            get { return _input; }
            set
            {
                _input = value;
            }
        }

        /// <MetaDataID>{ff57e771-5ee5-4e2d-a00c-1ecbed0c13dd}</MetaDataID>
        private bool _itemDragMode;
        /// <MetaDataID>{1f5ea678-1704-47d8-a5b6-7705228ec5d1}</MetaDataID>
        internal bool ItemDragMode
        {
            get { return _itemDragMode; }
            set { _itemDragMode = value; }
        }

        /// <MetaDataID>{828a0c66-7508-42e4-98dd-5be6ba02544f}</MetaDataID>
        private Point _itemDragStart;
        /// <MetaDataID>{c1fa3487-e216-4f6c-9b19-19aafc57202c}</MetaDataID>
        internal Point ItemDragStart
        {
            get { return _itemDragStart; }
            set { _itemDragStart = value; }
        }


        /// <summary>
        /// Number of rows fits to the screen
        /// </summary>
        /// <MetaDataID>{df3a6f6e-0503-447c-8955-939a070e35aa}</MetaDataID>
        internal int PageRowCount
        {
            get
            {
                return Math.Max((DisplayRectangle.Height - ColumnHeaderHeight) / (int)RowHeight, 0);
            }
        }

        /// <summary>
        /// Number of all visible nodes (which parent is expanded)
        /// </summary>
        /// <MetaDataID>{dc2b50ae-0589-47b0-acbf-a4460319305c}</MetaDataID>
        internal int RowCount
        {
            get
            {
                return _rowMap.Count;
            }
        }

        /// <MetaDataID>{de7f8948-d4f2-43e3-9738-90ad97cd4a5d}</MetaDataID>
        private int _contentWidth = 0;
        /// <MetaDataID>{a31da454-f31e-47c6-9f3e-20cf07779d03}</MetaDataID>
        private int ContentWidth
        {
            get
            {
                return _contentWidth;
            }
        }

        /// <MetaDataID>{3f3889b9-bc4d-4db0-8900-e91a12362175}</MetaDataID>
        internal int FirstVisibleRow
        {
            get { return _firstVisibleRow; }
            set
            {
                HideEditor();
                _firstVisibleRow = value;
                UpdateView();
            }
        }

        /// <MetaDataID>{45177f1c-6573-42da-8768-5e3dbbff6a13}</MetaDataID>
        private int OffsetX
        {
            get { return _offsetX; }
            set
            {
                HideEditor();
                _offsetX = value;
                UpdateView();
            }
        }

        /// <MetaDataID>{0a0cb979-ce9c-434c-ab0e-6c3eff5cfea7}</MetaDataID>
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle r = ClientRectangle;
                //r.Y += ColumnHeaderHeight;
                //r.Height -= ColumnHeaderHeight;
                int w = _vScrollBar.Visible ? _vScrollBar.Width : 0;
                int h = _hScrollBar.Visible ? _hScrollBar.Height : 0;
                return new Rectangle(r.X, r.Y, r.Width - w, r.Height - h);
            }
        }

        /// <MetaDataID>{d982e439-0158-48eb-8660-3645e6bb92c2}</MetaDataID>
        private List<TreeNode> _selection;
        /// <MetaDataID>{c9cb1711-9cac-4877-91c1-345b1cf4120f}</MetaDataID>
        internal List<TreeNode> Selection
        {
            get { return _selection; }
        }

        #endregion

        #region Public Properties

        #region DesignTime

        /// <MetaDataID>{efda3e7b-dee1-427c-a718-074da8e19111}</MetaDataID>
        private bool _fullRowSelect;
        /// <MetaDataID>{e0a42143-46ce-41ec-bab2-d930caf507b0}</MetaDataID>
        [DefaultValue(false), Category("Behavior")]
        public bool FullRowSelect
        {
            get { return _fullRowSelect; }
            set
            {
                _fullRowSelect = value;
                UpdateView();
            }
        }

        /// <MetaDataID>{f578838f-8095-4f35-bb22-72e30cb6449a}</MetaDataID>
        private bool _useColumns;
        /// <MetaDataID>{5b647f12-c843-4574-bee4-5317d48d82c0}</MetaDataID>
        [DefaultValue(false), Category("Behavior")]
        public bool UseColumns
        {
            get { return _useColumns; }
            set
            {
                _useColumns = value;
                FullUpdate();
            }
        }

        /// <MetaDataID>{4c167189-9fef-4a82-9f71-8563a08d9145}</MetaDataID>
        private bool _showLines = true;
        /// <MetaDataID>{2522f56c-d775-4fc6-ac27-0f65c992649a}</MetaDataID>
        [DefaultValue(true), Category("Behavior")]
        public bool ShowLines
        {
            get { return _showLines; }
            set
            {
                _showLines = value;
                UpdateView();
            }
        }

        /// <MetaDataID>{f0e8b002-ff59-4b47-9cfa-2dd71d7f408b}</MetaDataID>
        private bool _showNodeToolTips = false;
        /// <MetaDataID>{c0d21032-89d0-47e7-8d3a-27c7c41b3c31}</MetaDataID>
        [DefaultValue(false), Category("Behavior")]
        public bool ShowNodeToolTips
        {
            get { return _showNodeToolTips; }
            set { _showNodeToolTips = value; }
        }

        /// <MetaDataID>{78f7e3ba-7bf1-4700-b2ab-550fadaa115f}</MetaDataID>
        private bool _keepNodesExpanded;
        /// <MetaDataID>{ebc25547-4e33-4adc-acd6-804a6474cc9e}</MetaDataID>
        [DefaultValue(false), Category("Behavior")]
        public bool KeepNodesExpanded
        {
            get { return _keepNodesExpanded; }
            set { _keepNodesExpanded = value; }
        }

        /// <MetaDataID>{1be11339-d32b-438a-ba7f-75c039682bc4}</MetaDataID>
        private ITreeModel _model;
        /// <MetaDataID>{51852706-90b1-41a9-9c3c-7516b6a09d19}</MetaDataID>
        [Category("Data")]
        public ITreeModel Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    if (_model != null)
                        UnbindModelEvents();
                    _model = value;
                    CreateNodes();
                    FullUpdate();
                    if (_model != null)
                        BindModelEvents();
                }
            }
        }

        /// <MetaDataID>{cd4fc506-e5ec-4bc8-bd92-951c5827d21d}</MetaDataID>
        private BorderStyle _borderStyle;
        /// <MetaDataID>{9a6c3f8e-45dc-4d27-9322-ae3b40432fc0}</MetaDataID>
        [DefaultValue(BorderStyle.Fixed3D), Category("Appearance")]
        public BorderStyle BorderStyle
        {
            get
            {
                return this._borderStyle;
            }
            set
            {
                if (_borderStyle != value)
                {
                    _borderStyle = value;
                    base.UpdateStyles();
                }
            }
        }

        /// <MetaDataID>{bc44346d-89a6-4d6f-9a21-ed14aa7cd3eb}</MetaDataID>
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }


        /// <MetaDataID>{3f85029c-2dd9-4dfb-aa57-3bc27ffb0b1b}</MetaDataID>
        private static float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
        /// <MetaDataID>{7965ec9e-f5b7-4ee9-bcf5-f54623959222}</MetaDataID>
        static float? _TextScaleFactor;
        /// <MetaDataID>{1798259e-0aec-421b-b28e-27d0cd548fc4}</MetaDataID>
        public float TextScaleFactor
        {
            get
            {
                if (!_TextScaleFactor.HasValue)
                    _TextScaleFactor = this.CreateGraphics().DpiX / 96;
                return _TextScaleFactor.Value;
            }
        }

    /// <MetaDataID>{0b79b2f4-0510-4b72-87f3-3b39a90cb782}</MetaDataID>
    private double _rowHeight = 16;
        /// <MetaDataID>{c3f1c417-3997-4228-b266-7beede58d52e}</MetaDataID>
        [DefaultValue(16), Category("Appearance")]
        public double RowHeight
        {
            get
            {
                return _rowHeight* TextScaleFactor;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                _rowHeight = value/ TextScaleFactor;
                FullUpdate();
            }
        }

        /// <MetaDataID>{5fc8d747-fb08-49ce-baf4-5f5fc3adf69d}</MetaDataID>
        private TreeSelectionMode _selectionMode = TreeSelectionMode.Single;
        /// <MetaDataID>{b6bc09cc-14c7-4fc0-b3c1-eb2138948eff}</MetaDataID>
        [DefaultValue(TreeSelectionMode.Single), Category("Behavior")]
        public TreeSelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set { _selectionMode = value; }
        }

        /// <MetaDataID>{52e46344-a8a1-43c8-afbc-62719ba63ba0}</MetaDataID>
        private bool _hideSelection;
        /// <MetaDataID>{89fdeaca-20a6-4898-96c9-c1e7c1a169f1}</MetaDataID>
        [DefaultValue(false), Category("Behavior")]
        public bool HideSelection
        {
            get { return _hideSelection; }
            set
            {
                _hideSelection = value;
                UpdateView();
            }
        }

        /// <MetaDataID>{c120ca3b-fb02-440e-a157-4a9077c7c8d9}</MetaDataID>
        private float _topEdgeSensivity = 0.3f;
        /// <MetaDataID>{c1d547b5-0372-4d60-bc22-d41e177db99d}</MetaDataID>
        [DefaultValue(0.3f), Category("Behavior")]
        public float TopEdgeSensivity
        {
            get { return _topEdgeSensivity; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException();
                _topEdgeSensivity = value;
            }
        }

        /// <MetaDataID>{9f3ea828-d764-49cf-8dda-2d3b624524c7}</MetaDataID>
        private float _bottomEdgeSensivity = 0.3f;
        /// <MetaDataID>{8434e33c-3ac3-4ffe-b8ee-7810da916f0c}</MetaDataID>
        [DefaultValue(0.3f), Category("Behavior")]
        public float BottomEdgeSensivity
        {
            get { return _bottomEdgeSensivity; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("value should be from 0 to 1");
                _bottomEdgeSensivity = value;
            }
        }

        /// <MetaDataID>{8007b907-060f-438f-82c7-63c4d99aba3f}</MetaDataID>
        private bool _loadOnDemand=true;
        /// <MetaDataID>{83a25abb-20a1-4048-8e5c-eb6e5da15b93}</MetaDataID>
        [DefaultValue(false), Category("Behavior")]
        public bool LoadOnDemand
        {
            get { return _loadOnDemand; }
            set { _loadOnDemand = value; }
        }

        /// <MetaDataID>{616960e1-8169-41e2-a109-18dcad9e3ebb}</MetaDataID>
        private int _indent = 19;
        /// <MetaDataID>{f9f3620d-e64f-4e04-bc86-34705cc56960}</MetaDataID>
        [DefaultValue(19), Category("Behavior")]
        public int Indent
        {
            get { return _indent; }
            set
            {
                _indent = value;
                UpdateView();
            }
        }

        /// <MetaDataID>{c48264ab-3dc8-41ae-89ac-a5481e7f64dd}</MetaDataID>
        private Color _lineColor = SystemColors.ControlDark;
        /// <MetaDataID>{807c6f71-34e9-493c-ae4a-18aa55dbc741}</MetaDataID>
        [Category("Behavior")]
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                CreateLinePen();
                UpdateView();
            }
        }



        /// <MetaDataID>{e85b29e4-49bf-46a6-82b1-bc797ce619d8}</MetaDataID>
        private TreeColumnCollection _columns;
        /// <MetaDataID>{d1ced9c1-420d-4e5d-8193-3ab51681b21e}</MetaDataID>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<TreeColumn> Columns
        {
            get { return _columns; }
        }

        /// <MetaDataID>{6e5c29fa-ea14-4ef0-b366-0f0febfdb35e}</MetaDataID>
        private NodeControlsCollection _controls;
        /// <MetaDataID>{764c16ad-1e04-4d18-a789-0be100e63e67}</MetaDataID>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(NodeControlCollectionEditor), typeof(UITypeEditor))]
        public Collection<NodeControl> NodeControls
        {
            get
            {
                return _controls;
            }
        }

        #endregion

        #region RunTime

        /// <MetaDataID>{fafa7408-b971-4d66-a59f-2e8959aada89}</MetaDataID>
        [Browsable(false)]
        public IEnumerable<TreeNode> AllNodes
        {
            get
            {
                if (_root.Nodes.Count > 0)
                {
                    TreeNode node = _root.Nodes[0];
                    while (node != null)
                    {
                        yield return node;
                        if (node.Nodes.Count > 0)
                            node = node.Nodes[0];
                        else if (node.NextNode != null)
                            node = node.NextNode;
                        else
                            node = node.BottomNode;
                    }
                }
            }
        }

        /// <MetaDataID>{06f684d2-f13f-4c05-9010-cfb4860658c7}</MetaDataID>
        private DropPosition _dropPosition;
        /// <MetaDataID>{1c0dad37-14e8-4907-8abf-57c2c7d59bee}</MetaDataID>
        [Browsable(false)]
        public DropPosition DropPosition
        {
            get { return _dropPosition; }
            set { _dropPosition = value; }
        }

        /// <MetaDataID>{d99a44e9-d825-4a1c-9a8e-1f636fcee727}</MetaDataID>
        private TreeNode _root;
        /// <MetaDataID>{875b6724-cae4-4997-9ca7-67c48001b754}</MetaDataID>
        [Browsable(false)]
        public TreeNode Root
        {
            get { return _root; }
        }

        /// <MetaDataID>{8757ab06-bc69-4a30-b7a3-e0fb653131aa}</MetaDataID>
        [Browsable(false)]
        public ReadOnlyCollection<TreeNode> SelectedNodes
        {
            get
            {
                return _readonlySelection;
            }
        }
        /// <MetaDataID>{2958bf74-2099-4c3c-92dc-f3203d1a2a3d}</MetaDataID>
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {

                base.Site = value;
                if (DesignMode)
                {
                    (value.Container as System.ComponentModel.Design.IDesignerHost).LoadComplete += new EventHandler(DesignerHostLoadComplete);
                    if (NodeControls.Count == 0)
                    {

                    }
                }
            }
        }

        /// <MetaDataID>{94f57e62-d6bd-49ea-9ba3-56fa2218a9d7}</MetaDataID>
        void DesignerHostLoadComplete(object sender, EventArgs e)
        {

        }
        /// <MetaDataID>{09133de8-fc65-44e4-9cc2-e6e3054e6897}</MetaDataID>
        [Browsable(false)]
        public TreeNode SelectedNode
        {
            get
            {
                if (Selection.Count > 0)
                {
                    if (CurrentNode != null && CurrentNode.IsSelected)
                        return CurrentNode;
                    else
                        return Selection[0];
                }
                else
                    return null;
            }
            set
            {
                if (SelectedNode == value)
                    return;

                BeginUpdate();
                try
                {
                    if (value == null)
                    {
                        ClearSelection();
                    }
                    else
                    {
                        if (!IsMyNode(value))
                            throw new ArgumentException();

                        ClearSelection();
                        value.IsSelected = true;
                        CurrentNode = value;
                        EnsureVisible(value);
                    }
                }
                finally
                {
                    EndUpdate();
                }
            }
        }

        /// <MetaDataID>{ae6363d2-2895-4453-9950-bbac0d2bf58b}</MetaDataID>
        private TreeNode _currentNode;
        /// <MetaDataID>{feb09283-2d5e-4145-9f90-f87320ea1543}</MetaDataID>
        [Browsable(false)]
        public TreeNode CurrentNode
        {
            get { return _currentNode; }
            internal set { _currentNode = value; }
        }


        #endregion

        #endregion

        #region Public Events

        [Category("Action")]
        public event ItemDragEventHandler ItemDrag;
        /// <MetaDataID>{c59cefb6-38ca-4cb3-8feb-3aadd5519676}</MetaDataID>
        private void OnItemDrag(MouseButtons buttons, object item)
        {
            if (ItemDrag != null)
                ItemDrag(this, new ItemDragEventArgs(buttons, item));
        }

        [Category("Behavior")]
        public event EventHandler<TreeNodeAdvMouseEventArgs> NodeMouseDoubleClick;
        /// <MetaDataID>{373165c0-0c25-436a-9c3a-97448de552e1}</MetaDataID>
        private void OnNodeMouseDoubleClick(TreeNodeAdvMouseEventArgs args)
        {
            if (NodeMouseDoubleClick != null)
                NodeMouseDoubleClick(this, args);
        }

        [Category("Behavior")]
        public event EventHandler<TreeColumnEventArgs> ColumnWidthChanged;
        /// <MetaDataID>{4e28963f-9718-4e71-80f3-09bab01af808}</MetaDataID>
        internal void OnColumnWidthChanged(TreeColumn column)
        {
            if (ColumnWidthChanged != null)
                ColumnWidthChanged(this, new TreeColumnEventArgs(column));
        }

        [Category("Behavior")]
        public event EventHandler SelectionChanged;
        /// <MetaDataID>{9106a9fd-48a2-41eb-8df1-0e0a3e5ffd4c}</MetaDataID>
        internal void OnSelectionChanged()
        {
            if (SuspendSelectionEvent)
                _fireSelectionEvent = true;
            else
            {
                _fireSelectionEvent = false;
                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);

                if (!string.IsNullOrEmpty(_SelectionMember))
                {
                    try
                    {
                        object value = GetPropertyValue("SelectedNodeValue");
                        UserInterfaceObjectConnection.SetValue(value, _SelectionMember);
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
        }

        [Category("Behavior")]
        public event EventHandler<TreeViewAdvEventArgs> Collapsing;
        /// <MetaDataID>{aebc5fd0-3d8e-46e3-8092-5832d4379464}</MetaDataID>
        internal void OnCollapsing(TreeNode node)
        {
            if (Collapsing != null)
                Collapsing(this, new TreeViewAdvEventArgs(node));
        }

        [Category("Behavior")]
        public event EventHandler<TreeViewAdvEventArgs> Collapsed;
        /// <MetaDataID>{cd146688-70d0-49db-bff2-cf4ce7c78c1f}</MetaDataID>
        internal void OnCollapsed(TreeNode node)
        {
            if (Collapsed != null)
                Collapsed(this, new TreeViewAdvEventArgs(node));
        }

        [Category("Behavior")]
        public event EventHandler<TreeViewAdvEventArgs> Expanding;
        /// <MetaDataID>{31d4b4a4-a4ec-4b90-9a44-e720ddb6fabd}</MetaDataID>
        internal void OnExpanding(TreeNode node)
        {
            if (Expanding != null)
                Expanding(this, new TreeViewAdvEventArgs(node));
        }

        [Category("Behavior")]
        public event EventHandler<TreeViewAdvEventArgs> Expanded;
        /// <MetaDataID>{128a1788-15f3-4ec0-b279-3e81af1a7586}</MetaDataID>
        internal void OnExpanded(TreeNode node)
        {
            if (Expanded != null)
                Expanded(this, new TreeViewAdvEventArgs(node));
        }

        #endregion


        /// <MetaDataID>{a5521128-ff0b-4827-aabc-0f852b301e57}</MetaDataID>
        public static Image GetImageFromResourcea(Type t, string imageName, bool large)
        {
            Image image = null;
            try
            {  
                string fullName = imageName;
                string bitmapname = null;
                string str3 = null;
                string str4 = null;
                if (fullName == null)
                {
                    fullName = t.FullName;
                    int num = fullName.LastIndexOf('.');
                    if (num != -1)
                    {
                        fullName = fullName.Substring(num + 1);
                    }
                    bitmapname = fullName + ".ico";
                    str3 = fullName + ".bmp";
                }
                else if (string.Compare(System.IO.Path.GetExtension(imageName), ".ico", true, System.Globalization.CultureInfo.CurrentCulture) == 0)
                {
                    bitmapname = fullName;
                }
                else if (string.Compare(System.IO.Path.GetExtension(imageName), ".bmp", true, System.Globalization.CultureInfo.CurrentCulture) == 0)
                {
                    str3 = fullName;
                }
                else
                {
                    str4 = fullName;
                    str3 = fullName + ".bmp";
                    bitmapname = fullName + ".ico";
                }
                //image = GetBitmapFromResource(t, str4, large);
                System.IO.Stream manifestResourceStreamsd = t.Module.Assembly.GetManifestResourceStream(t, str4);
                if (image == null)
                {
                    System.IO.Stream manifestResourceStream = t.Module.Assembly.GetManifestResourceStream(t, str3);

                    //image = GetBitmapFromResource(t, str3, large);
                }
                if (image == null)
                {
                    System.IO.Stream manifestResourceStream = t.Module.Assembly.GetManifestResourceStream(t, bitmapname);
                    //image = GetIconFromResource(t, bitmapname, large);
                }
            }
            catch (Exception)
            {
            }
            return image;
        }





        /// <MetaDataID>{3ef16d6d-2ee1-4f3f-bcd1-f2c89cc71631}</MetaDataID>
        public TreeView()
        {

            this.SizeChanged += new EventHandler(TreeView_SizeChanged);
            Image imag = GetImageFromResourcea(typeof(ViewControlObject), "TreeView.bmp", true);
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable
                , true);


            if (Application.RenderWithVisualStyles)
                _columnHeaderHeight = 20;
            else
                _columnHeaderHeight = 17;

            BorderStyle = BorderStyle.Fixed3D;
            _hScrollBar.Height = SystemInformation.HorizontalScrollBarHeight;
            _vScrollBar.Width = SystemInformation.VerticalScrollBarWidth;
            _rowMap = new List<TreeNode>();
            _selection = new List<TreeNode>();
            _readonlySelection = new ReadOnlyCollection<TreeNode>(_selection);
            _columns = new TreeColumnCollection(this);
            _toolTip = new ToolTip();

            Input = new NormalInputState(this);
            CreateNodes();
            CreatePens();

            ArrangeControls();

            _plusMinus = new NodePlusMinus();
            _controls = new NodeControlsCollection(this);
            _EnabledProperty = new DependencyProperty( this, "Enabled");
            _DependencyProperties.Add(_EnabledProperty);

        }

        /// <MetaDataID>{6cbede91-1540-4467-9c12-2cc9382adfe1}</MetaDataID>
        void TreeView_SizeChanged(object sender, EventArgs e)
        {
            
        }


        #region Public Methods

        /// <MetaDataID>{d6bdecc6-adb2-4505-b0bf-fc0deb92aef0}</MetaDataID>
        public TreePath GetPath(TreeNode node)
        {
            try
            {
                if (node == _root)
                    return TreePath.Empty;
                else
                {
                    Stack<object> stack = new Stack<object>();
                    while (node != _root && node != null)
                    {
                        stack.Push(node.Tag);
                        node = node.Parent;
                    }
                    if (node == null)
                        return TreePath.Empty;

                    return new TreePath(stack.ToArray());
                }
            }
            catch (System.Exception error)
            {
                throw;

            }
        }

        /// <MetaDataID>{d5b43e09-6259-4f19-8c38-d841191cbab2}</MetaDataID>
        public TreeNode GetNodeAt(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                return null;

            point = ToAbsoluteLocation(point);
            int row = point.Y /(int) RowHeight;
            if (row < RowCount && row >= 0)
            {
                NodeControlInfo info = GetNodeControlInfoAt(_rowMap[row], point);
                if (info.Control != null)
                    return _rowMap[row];
            }
            return null;
        }
        /// <MetaDataID>{343bd8eb-77ad-46eb-9fc8-5a7d2e4939bf}</MetaDataID>
        public TreeNode GetNodeAtRow(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                return null;

            point = ToAbsoluteLocation(point);
            int row = point.Y / (int)RowHeight;
            if (row < RowCount && row >= 0)
            {
                //NodeControlInfo info = GetNodeControlInfoAt(_rowMap[row], point);
                //if (info.Control != null)
                return _rowMap[row];
            }
            return null;
        }

        /// <MetaDataID>{44636cef-ab0f-4eab-b2bf-ee670e530d5f}</MetaDataID>
        public void BeginUpdate()
        {
            _suspendUpdate = true;
        }

        /// <MetaDataID>{d3235165-376e-46c6-94b4-9cffcb0cfa2c}</MetaDataID>
        public void EndUpdate()
        {
            _suspendUpdate = false;
            if (_needFullUpdate)
                FullUpdate();
            else
                UpdateView();
        }

        /// <MetaDataID>{f9bdb425-28a7-4a73-9a88-d08b7bb076fc}</MetaDataID>
        public void ExpandAll()
        {
            BeginUpdate();
            SetIsExpanded(_root, true);
            EndUpdate();
        }

        /// <MetaDataID>{1e70befe-a5a2-4b9f-acfb-f2fb2807b3c0}</MetaDataID>
        public void CollapseAll()
        {
            BeginUpdate();
            SetIsExpanded(_root, false);
            EndUpdate();
        }


        /// <summary>
        /// Expand all parent nodes, andd scroll to the specified node
        /// </summary>
        /// <MetaDataID>{595c810c-6e2d-407f-9af0-5b77cdc09502}</MetaDataID>
        public void EnsureVisible(TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (!IsMyNode(node))
                throw new ArgumentException();

            TreeNode parent = node.Parent;
            while (parent != _root)
            {
                parent.IsExpanded = true;
                parent = parent.Parent;
            }
            ScrollTo(node);
        }

        /// <summary>
        /// Make node visible, scroll if needed. All parent nodes of the specified node must be expanded
        /// </summary>
        /// <param name="node"></param>
        /// <MetaDataID>{3a06bb2b-67dd-4c27-817b-a58ae068472c}</MetaDataID>
        public void ScrollTo(TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (!IsMyNode(node))
                throw new ArgumentException();

            if (node.Row < 0)
                CreateRowMap();

            int row = 0;
            if (node.Row < FirstVisibleRow)
                row = node.Row;
            else if (node.Row >= FirstVisibleRow + (PageRowCount - 1))
                row = node.Row - (PageRowCount - 1);

            if (row >= _vScrollBar.Minimum && row <= _vScrollBar.Maximum)
                _vScrollBar.Value = row;
        }

        #endregion

        /// <MetaDataID>{7e86784a-ea73-4148-bb5f-81f6da661695}</MetaDataID>
        private Point ToAbsoluteLocation(Point point)
        {
            return new Point(point.X + _offsetX, point.Y + (FirstVisibleRow * (int)RowHeight) - ColumnHeaderHeight);
        }

        /// <MetaDataID>{4e36c0d9-4dac-41f7-adee-c574e2d3a710}</MetaDataID>
        private Point ToViewLocation(Point point)
        {
            return new Point(point.X - _offsetX, point.Y - (FirstVisibleRow * (int)RowHeight) + ColumnHeaderHeight);
        }

        /// <MetaDataID>{bac8ae64-0910-4ac5-9931-d4178f1575aa}</MetaDataID>
        protected override void OnSizeChanged(EventArgs e)
        {
            ArrangeControls();
            SafeUpdateScrollBars();
            base.OnSizeChanged(e);
        }

        /// <MetaDataID>{b3df441e-4616-40ce-b63a-47ef8c144aed}</MetaDataID>
        private void ArrangeControls()
        {
            int hBarSize = _hScrollBar.Height;
            int vBarSize = _vScrollBar.Width;
            Rectangle clientRect = ClientRectangle;

            _hScrollBar.SetBounds(clientRect.X, clientRect.Bottom - hBarSize,
                clientRect.Width - vBarSize, hBarSize);

            _vScrollBar.SetBounds(clientRect.Right - vBarSize, clientRect.Y,
                vBarSize, clientRect.Height - hBarSize);
        }

        /// <MetaDataID>{0277ff1d-7ca9-4b9f-a1b0-bcd96d962806}</MetaDataID>
        private void SafeUpdateScrollBars()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(UpdateScrollBars));
            else
                UpdateScrollBars();
        }

        /// <MetaDataID>{72c1ab4b-635d-4da3-a72f-0f1e6d4e2fd3}</MetaDataID>
        private void UpdateScrollBars()
        {
            UpdateVScrollBar();
            UpdateHScrollBar();
            UpdateVScrollBar();
            UpdateHScrollBar();
            _hScrollBar.Width = DisplayRectangle.Width;
            _vScrollBar.Height = DisplayRectangle.Height;
        }

        /// <MetaDataID>{f3464a75-ca32-4ff0-abcf-364d2d4d4308}</MetaDataID>
        private void UpdateHScrollBar()
        {
            _hScrollBar.Maximum = ContentWidth;
            _hScrollBar.LargeChange = Math.Max(DisplayRectangle.Width, 0);
            _hScrollBar.SmallChange = 5;
            _hScrollBar.Visible = _hScrollBar.LargeChange < _hScrollBar.Maximum;
            _hScrollBar.Value = Math.Min(_hScrollBar.Value, _hScrollBar.Maximum - _hScrollBar.LargeChange + 1);
        }

        /// <MetaDataID>{d12c98ab-09d4-445c-995c-32c8c99b8f31}</MetaDataID>
        private void UpdateVScrollBar()
        {
            _vScrollBar.Maximum = Math.Max(RowCount - 1, 0);
            _vScrollBar.LargeChange = PageRowCount;
            _vScrollBar.Visible = _vScrollBar.LargeChange <= _vScrollBar.Maximum;
            _vScrollBar.Value = Math.Min(_vScrollBar.Value, _vScrollBar.Maximum - _vScrollBar.LargeChange + 1);
        }

        /// <MetaDataID>{7d9f6824-d5d8-40a2-bfa2-0e98ca3fe7a7}</MetaDataID>
        private void CreatePens()
        {
            CreateLinePen();
            CreateMarkPen();
        }

        /// <MetaDataID>{492ccc59-4373-44cd-a412-e9eff94fe800}</MetaDataID>
        private void CreateMarkPen()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLines(new Point[] { new Point(0, 0), new Point(1, 1), new Point(-1, 1), new Point(0, 0) });
            CustomLineCap cap = new CustomLineCap(null, path);
            cap.WidthScale = 1.0f;

            _markPen = new Pen(_dragDropMarkColor, _dragDropMarkWidth);
            _markPen.CustomStartCap = cap;
            _markPen.CustomEndCap = cap;
        }

        /// <MetaDataID>{c2a7d47b-442a-4ad3-87cb-35207f17d523}</MetaDataID>
        private void CreateLinePen()
        {
            _linePen = new Pen(_lineColor);
            _linePen.DashStyle = DashStyle.Dot;
        }

        /// <MetaDataID>{c2cbeace-2482-45ff-9406-3cc422b74004}</MetaDataID>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams res = base.CreateParams;
                switch (BorderStyle)
                {
                    case BorderStyle.FixedSingle:
                        res.Style |= 0x800000;
                        break;
                    case BorderStyle.Fixed3D:
                        res.ExStyle |= 0x200;
                        break;
                }
                return res;
            }
        }

        /// <MetaDataID>{ec99f710-7d9d-4993-902e-ecc4e2185b51}</MetaDataID>
        protected override void OnGotFocus(EventArgs e)
        {
            DisposeEditor();
            UpdateView();
            ChangeInput();
            base.OnGotFocus(e);
        }

        /// <MetaDataID>{ea58a4a5-c761-4ce3-ae6e-9a6dfbb3d203}</MetaDataID>
        protected override void OnLeave(EventArgs e)
        {
            DisposeEditor();
            UpdateView();
            base.OnLeave(e);
        }

        #region Keys

        /// <MetaDataID>{ce81cd74-1ab9-4815-a00e-ed4766d16f58}</MetaDataID>
        protected override bool IsInputKey(Keys keyData)
        {
            if (((keyData & Keys.Up) == Keys.Up)
                || ((keyData & Keys.Down) == Keys.Down)
                || ((keyData & Keys.Left) == Keys.Left)
                || ((keyData & Keys.Right) == Keys.Right))
                return true;
            else
                return base.IsInputKey(keyData);
        }

        /// <MetaDataID>{61c454d3-6e76-4759-a451-ce439b66ab52}</MetaDataID>
        internal void ChangeInput()
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if (!(Input is InputWithShift))
                    Input = new InputWithShift(this);
            }
            else if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (!(Input is InputWithControl))
                    Input = new InputWithControl(this);
            }
            else
            {
                if (!(Input.GetType() == typeof(NormalInputState)))
                    Input = new NormalInputState(this);
            }
        }

        /// <MetaDataID>{24517544-dcff-48e1-8039-d8b46d43238b}</MetaDataID>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey)
                    ChangeInput();
                Input.KeyDown(e);
                if (!e.Handled)
                {
                    foreach (NodeControlInfo item in GetNodeControls(CurrentNode))
                    {
                        item.Control.KeyDown(e);
                        if (e.Handled)
                            return;
                    }
                }
            }
        }

        /// <MetaDataID>{f3af5b21-d5f5-458d-b9e9-44bf909985d5}</MetaDataID>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey)
                    ChangeInput();
                if (!e.Handled)
                {
                    foreach (NodeControlInfo item in GetNodeControls(CurrentNode))
                    {
                        item.Control.KeyUp(e);
                        if (e.Handled)
                            return;
                    }
                }
            }
        }

        #endregion

        #region Mouse

        /// <MetaDataID>{67613dc6-26d0-41bd-ac75-2fe9449e5801}</MetaDataID>
        private TreeNodeAdvMouseEventArgs CreateMouseArgs(MouseEventArgs e)
        {
            TreeNodeAdvMouseEventArgs args = new TreeNodeAdvMouseEventArgs(e);
            args.ViewLocation = e.Location;
            args.AbsoluteLocation = ToAbsoluteLocation(e.Location);
            args.ModifierKeys = ModifierKeys;
            args.Node = GetNodeAt(e.Location);
            NodeControlInfo info = GetNodeControlInfoAt(args.Node, args.AbsoluteLocation);
            args.ControlBounds = info.Bounds;
            args.Control = info.Control;
            return args;
        }


        /// <MetaDataID>{45cbc3c7-b889-4bfe-843d-96a477dc7257}</MetaDataID>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (SystemInformation.MouseWheelScrollLines > 0)
            {
                int lines = e.Delta / 120 * SystemInformation.MouseWheelScrollLines;
                int newValue = _vScrollBar.Value - lines;
                _vScrollBar.Value = Math.Max(_vScrollBar.Minimum,
                    Math.Min(_vScrollBar.Maximum - _vScrollBar.LargeChange + 1, newValue));
            }
        }

        /// <MetaDataID>{1f7e1d0f-3de7-4043-931f-b006fd3a2348}</MetaDataID>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!Focused)
                Focus();

            if (e.Button == MouseButtons.Left)
            {
                TreeColumn c = GetColumnDividerAt(e.Location);
                if (c != null)
                {
                    Input = new ResizeColumnState(this, c, e.Location);
                    return;
                }
            }

            ChangeInput();
            TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);

            if (args.Node != null && args.Control != null)
                args.Control.MouseDown(args);

            if (!args.Handled)
                Input.MouseDown(args);
        }

        /// <MetaDataID>{5bb7a821-4ff3-4ae7-a46a-fd4047736b70}</MetaDataID>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);
            if (Input is ResizeColumnState)
                Input.MouseUp(args);
            else
            {
                base.OnMouseUp(e);
                if (args.Node != null && args.Control != null)
                    args.Control.MouseUp(args);
                if (!args.Handled)
                    Input.MouseUp(args);
            }
        }

        /// <MetaDataID>{53066c35-211b-4148-b58e-87512d97c71d}</MetaDataID>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);
            if (args.Node != null)
            {
                OnNodeMouseDoubleClick(args);
                if (args.Handled)
                    return;
            }

            if (args.Node != null && args.Control != null)
                args.Control.MouseDoubleClick(args);
            if (!args.Handled)
            {
                if (args.Node != null && args.Button == MouseButtons.Left)
                    args.Node.IsExpanded = !args.Node.IsExpanded;
            }
        }
        /// <MetaDataID>{8e307a37-63e0-48c9-a3d6-caaaa6f4fa8e}</MetaDataID>
        DragDropTransactionOptions _DragDropTransactionOption;
        /// <MetaDataID>{20f06682-506c-474e-beed-01d22267a994}</MetaDataID>
        [Category("DragDrop Behavior")]
        public DragDropTransactionOptions DragDropTransactionOption
        {
            get
            {
                return _DragDropTransactionOption;
            }
            set
            {
                _DragDropTransactionOption = value;
            }
        }

        /// <MetaDataID>{9afccba9-a201-40e7-af1b-e6cedcf7c3bf}</MetaDataID>
        System.Drawing.Point StartDragPoint = new System.Drawing.Point(-1, -1);
        /// <MetaDataID>{ae9ea9ff-1fde-482d-9e66-d5df42770165}</MetaDataID>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_dragMode)
                Invalidate();
            else
            {
                if (AllowDrag)
                {

                    if (e.Button == MouseButtons.Left && AllowDrag)
                    {

                        if (StartDragPoint.X == -1 && StartDragPoint.Y == -1)
                        {
                            StartDragPoint = e.Location;

                        }
                        else if (SelectedNodes.Count == 1 && SelectedNodes[0].Tag is NodeDisplayedObject)
                        {


                            int xDestance = StartDragPoint.X - e.X;
                            if (xDestance < 0)
                                xDestance = -xDestance;
                            int yDestance = StartDragPoint.Y - e.Y;
                            if (yDestance < 0)
                                yDestance = -yDestance;

                            if ((yDestance > 10 || xDestance > 10) && Cursor.Current != Cursors.IBeam && (SelectedNodes[0].Parent.Tag is NodeDisplayedObject) && (SelectedNodes[0].Parent.Tag as NodeDisplayedObject).Value != null)
                            {
                                if (SelectedNodes[0].Parent != null && SelectedNodes[0].Parent.Tag is NodeDisplayedObject)
                                    DragObjectParent = (SelectedNodes[0].Parent.Tag as NodeDisplayedObject).Value;
                                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, (SelectedNodes[0].Tag as NodeDisplayedObject).Value, DragDropTransactionOption);
                                _dragMode = true;
                                _dragSource = true;
                                DragDropEffects dragDropEffect = DoDragDrop(dragDropActionManager, DragDropEffects.All);
                                _dragSource = false;
                                _dragMode = false;
                                //if (dragDropEffect == DragDropEffects.Move && CutOperationCaller != null && CutOperationCaller.Operation != null)
                                //{
                                //    object ret = CutOperationCaller.Invoke();
                                //}




                            }
                        }
                    }
                    else
                    {
                        StartDragPoint.X = -1;
                        StartDragPoint.Y = -1;
                    }
                }
            }
            if (Input.MouseMove(e))
                return;

            base.OnMouseMove(e);
            SetCursor(e);
            if (e.Location.Y <= ColumnHeaderHeight)
            {
                _toolTip.Active = false;
            }
            else
            {
                UpdateToolTip(e);
                if (ItemDragMode && Dist(e.Location, ItemDragStart) > ItemDragSensivity
                    && CurrentNode != null && CurrentNode.IsSelected)
                {
                    ItemDragMode = false;
                    _toolTip.Active = false;
                    OnItemDrag(e.Button, Selection.ToArray());
                }
            }
        }

        /// <MetaDataID>{1d05f74e-65d0-4429-a178-8522a5b5186e}</MetaDataID>
        private void SetCursor(MouseEventArgs e)
        {
            if (GetColumnDividerAt(e.Location) == null)
                this.Cursor = Cursors.Default;
            else
                this.Cursor = Cursors.VSplit;
        }

        /// <MetaDataID>{53b3bf8f-5b3d-47bc-b143-ca2315985153}</MetaDataID>
        private TreeColumn GetColumnDividerAt(Point p)
        {
            if (p.Y > ColumnHeaderHeight)
                return null;

            int x = -OffsetX;
            foreach (TreeColumn c in Columns)
            {
                if (c.IsVisible)
                {
                    x += c.Width;
                    Rectangle rect = new Rectangle(x - DividerWidth / 2, 0, DividerWidth, ColumnHeaderHeight);
                    if (rect.Contains(p))
                        return c;
                }
            }
            return null;
        }

        /// <MetaDataID>{c4b32fa3-0d4c-49d8-ba1d-a0f79ede50d6}</MetaDataID>
        TreeNode _hoverNode;
        /// <MetaDataID>{c7cff2eb-3905-4cc9-b20e-874f97c3f3cb}</MetaDataID>
        NodeControl _hoverControl;
        /// <MetaDataID>{f5a5957d-ccae-47d6-987d-ad75af1811cf}</MetaDataID>
        private void UpdateToolTip(MouseEventArgs e)
        {
            if (_showNodeToolTips)
            {
                TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);
                if (args.Node != null)
                {
                    if (args.Node != _hoverNode || args.Control != _hoverControl)
                    {
                        string msg = args.Control.GetToolTip(args.Node);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            _toolTip.SetToolTip(this, msg);
                            _toolTip.Active = true;
                        }
                        else
                            _toolTip.SetToolTip(this, null);
                    }
                }
                else
                    _toolTip.SetToolTip(this, null);

                _hoverControl = args.Control;
                _hoverNode = args.Node;

            }
            else
                _toolTip.SetToolTip(this, null);
        }
        #endregion

        #region DragDrop Behavior
        /// <MetaDataID>{371aca98-bdc7-4a9e-b611-9d58bafc357d}</MetaDataID>
        TreeNode DragOverNode;
        /// <MetaDataID>{ea4123bb-1f91-458c-aea2-44296d731aba}</MetaDataID>
        System.DateTime DragOverNodeTime;


        /// <MetaDataID>{5b92e6c1-fe4f-456c-8db3-387e73297312}</MetaDataID>
        public void CutObject(object dropObject)
        {

            DragDropObject = dropObject;
            if (CutOperationCaller != null && CutOperationCaller.Operation != null)
            {
                object ret = CutOperationCaller.Invoke();
            }
            this.UserInterfaceObjectConnection.RemoveCollectionObject(GetPropertyValue("DragObjectParent"), ValueType, SubNodesProperty as string, dropObject);


        }

        /// <MetaDataID>{7e0d5ce7-afa5-4bcd-9147-6d8c02f794c6}</MetaDataID>
        public void PasteObject(object dropObject)
        {
            DragDropObject = dropObject;
            object ret = DragDropOperationCaller.Invoke();
            UserInterfaceObjectConnection.AddCollectionObject(GetPropertyValue("CopyInsideObject"), ValueType, SubNodesProperty as string, dropObject, null, (int)GetPropertyValue("DropMarkIndex"));

        }


        /// <MetaDataID>{4331f838-3c6f-47c5-a603-15f759d20ed9}</MetaDataID>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (_dragMode)
            {
                if (DropPosition.Node != DragOverNode)
                {
                    DragOverNodeTime = System.DateTime.Now;
                    DragOverNode = DropPosition.Node;
                }
                else if (DragOverNode != null && ((TimeSpan)(System.DateTime.Now - DragOverNodeTime)).TotalSeconds > 2)
                {
                    DragOverNode.IsExpanded = true;

                }


                ItemDragMode = false;
                Point pt = PointToClient(new Point(drgevent.X, drgevent.Y));

                if (this._vScrollBar.Visible && PointToClient(new Point(drgevent.X, drgevent.Y)).Y < (RowHeight))
                {
                    if (this._vScrollBar.Value > 0)
                        this._vScrollBar.Value--;

                }

                if (this._vScrollBar.Visible && PointToClient(new Point(drgevent.X, drgevent.Y)).Y > (Height - RowHeight))
                {
                    if (this._vScrollBar.Value <= this._vScrollBar.Maximum - this._vScrollBar.LargeChange)
                        this._vScrollBar.Value++;
                }


                if (this._hScrollBar.Visible && PointToClient(new Point(drgevent.X, drgevent.Y)).X < (RowHeight))
                {
                    if (this._hScrollBar.Value > 0)
                        this._hScrollBar.Value--;

                }

                if (this._hScrollBar.Visible && PointToClient(new Point(drgevent.X, drgevent.Y)).X > (Width - RowHeight))
                {
                    if (this._hScrollBar.Value <= this._hScrollBar.Maximum - this._hScrollBar.LargeChange)
                        this._hScrollBar.Value++;
                }






                SetDropPosition(pt);
                UpdateView();
            }

            base.OnDragOver(drgevent);
        }

        /// <MetaDataID>{364e177b-1942-4a8a-a5ff-951fd1b156e0}</MetaDataID>
        protected override void OnDragLeave(EventArgs e)
        {
            _dragMode = false;
            DragOverNode = null;
            DragDropObject = null;
            UpdateView();
            base.OnDragLeave(e);
        }

        /// <MetaDataID>{5f2295a6-cbd4-4042-bb6a-c00c67a90f61}</MetaDataID>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (_dragMode && DragDropOperationCaller != null && DragDropOperationCaller.Operation != null)
            {
                try
                {
                    object dropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);

                    if (dropObject is DragDropActionManager)
                        (dropObject as DragDropActionManager).DropObject((OOAdvantech.DragDropMethod)drgevent.Effect, this);
                    else
                        PasteObject(dropObject);

                }
                catch (System.Exception error)
                {
                    drgevent.Effect = DragDropEffects.None;
                    throw;
                }
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
            }


            _dragMode = false;
            DragDropObject = null;
            DragOverNode = null;
            UpdateView();
            base.OnDragDrop(drgevent);
        }
        /// <MetaDataID>{9db8320e-da42-492e-a1fd-b928bb6e91b5}</MetaDataID>
        object DragDropObject;
        /// <MetaDataID>{2454e109-0040-4c84-99b3-5a6eb58afcef}</MetaDataID>
        object DragObjectParent;
        /// <MetaDataID>{9b437bfc-71db-4983-9432-b7f555df5cdb}</MetaDataID>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            _dragMode = false;
            try
            {

                if (AllowDropOperationCaller != null && AllowDropOperationCaller.Operation != null)
                {
                    DragDropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
                    if (DragDropObject is DragDropActionManager)
                        DragDropObject = (DragDropObject as DragDropActionManager).DragedObject;


                    object ret = AllowDropOperationCaller.Invoke();
                    if (ret is OOAdvantech.DragDropMethod)
                    {
                        drgevent.Effect = (DragDropEffects)ret;
                        if ((DragDropEffects)ret != DragDropEffects.None)
                            _dragMode = true;
                    }
                }
                else
                    drgevent.Effect = DragDropEffects.None;
            }
            catch (System.Exception error)
            {
                throw;
            }
            base.OnDragEnter(drgevent);

        }





        /// <MetaDataID>{1485f8cd-10b5-4422-a915-00dad5cbb40d}</MetaDataID>
        private Color _dragDropMarkColor = Color.Blue;
        /// <MetaDataID>{ffe2bfde-67ea-4cbd-9d30-3ee89ffddee0}</MetaDataID>
        [Category("DragDrop Behavior")]
        public Color DragDropMarkColor
        {
            get { return _dragDropMarkColor; }
            set
            {
                _dragDropMarkColor = value;
                CreateMarkPen();
            }
        }

        /// <MetaDataID>{54d4f22f-b762-4646-a525-39fe0f7e7e5c}</MetaDataID>
        private float _dragDropMarkWidth = 1.0f;
        /// <MetaDataID>{b9c6b417-0fb4-4d4c-a2f4-4f437c719c2d}</MetaDataID>
        [DefaultValue(3.0f), Category("DragDrop Behavior")]
        public float DragDropMarkWidth
        {
            get { return _dragDropMarkWidth; }
            set
            {
                _dragDropMarkWidth = value;
                CreateMarkPen();
            }
        }
        /// <MetaDataID>{93a1f8af-b416-40d2-81b8-797e97befd18}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{7c23461a-8860-46a7-b6b0-ebb570025cf7}</MetaDataID>
        [Category("DragDrop Behavior")]
        public bool AllowDrag
        {
            get
            {
                return _AllowDrag;
            }
            set
            {
                _AllowDrag = value; ;
            }
        }

        /// <MetaDataID>{95952e07-5493-4175-b319-7ede1a405071}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _AllowDropOperationCaller;
        /// <MetaDataID>{bd63203c-02cc-4d53-8ce7-9bde90183f2f}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller AllowDropOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.AllowDropOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_AllowDropOperationCaller != null)
                    return _AllowDropOperationCaller;
                _AllowDropOperationCaller = new OperationCaller(TreeViewMetaData.AllowDropOperation, this);
                return _AllowDropOperationCaller;
            }
        }
        /// <MetaDataID>{39da25e7-9a8b-4755-8b98-0eee98fdb65c}</MetaDataID>
        [Category("DragDrop Behavior")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object AllowDropOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.AllowDropOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.AllowDropOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (AllowDropOperationCaller == null || AllowDropOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(AllowDrop row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, AllowDropOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.AllowDropOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return error;
                }
            }
            set
            {
                _AllowDropOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }



        /// <MetaDataID>{bcadef50-e443-4012-8f32-95bfaecb2bae}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DragDropOperationCaller;
        /// <MetaDataID>{27499a7b-389b-4b68-9622-60085425bb53}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller DragDropOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.DragDropOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_DragDropOperationCaller != null)
                    return _DragDropOperationCaller;
                _DragDropOperationCaller = new OperationCaller(TreeViewMetaData.DragDropOperation, this);
                return _DragDropOperationCaller;
            }
        }


        /// <MetaDataID>{510d14db-a91e-4fff-bff6-2324dba8c1ca}</MetaDataID>
        [Category("DragDrop Behavior")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object DragDropOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.DragDropOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.DragDropOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (DragDropOperationCaller == null || DragDropOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Edit row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, DragDropOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.DragDropOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return error;
                }
            }
            set
            {
                _DragDropOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }


        /// <MetaDataID>{0c6fb3c5-beee-4768-830a-4f44d55c00cc}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _CutOperationCaller;
        /// <MetaDataID>{6f53476f-2045-41c8-a98b-fb2ab7503cec}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller CutOperationCaller
        {
            get
            {
                if (TreeViewMetaData == null || TreeViewMetaData.CutOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_CutOperationCaller != null)
                    return _CutOperationCaller;
                _CutOperationCaller = new OperationCaller(TreeViewMetaData.CutOperation, this);
                return _CutOperationCaller;
            }
        }


        /// <MetaDataID>{5989ff0e-6caa-43e1-baf1-606c99c0c62a}</MetaDataID>
        [Category("DragDrop Behavior")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object CutOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (TreeViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (TreeViewMetaData.CutOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(TreeViewMetaData);
                        TreeViewMetaData.CutOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (CutOperationCaller == null || CutOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Edit row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, CutOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = TreeViewMetaData.CutOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return error;
                }
            }
            set
            {
                _CutOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }



        #endregion

        /// <MetaDataID>{ced97e9f-2a06-4354-87cd-844fdf5cd444}</MetaDataID>
        private IEnumerable<NodeControlInfo> GetNodeControls(TreeNode node)
        {
            if (node == null)
                yield break;

            int y = node.Row * (int)RowHeight + TopMargin;
            int x = (node.Level - 1) * _indent + LeftMargin;
            int width = _plusMinus.MeasureSize(node).Width;

            Rectangle rect = new Rectangle(x, y, width, (int)RowHeight);
            if (UseColumns && Columns.Count > 0 && Columns[0].Width < rect.Right)
                rect.Width = Columns[0].Width - x;
            yield return new NodeControlInfo(_plusMinus, rect);

            x += (width + 1);
            if (!UseColumns)
            {
                foreach (NodeControl c in NodeControls)
                {
                    width = c.MeasureSize(node).Width;
                    rect = new Rectangle(x, y, width, (int)RowHeight);
                    x += (width + 1);
                    yield return new NodeControlInfo(c, rect);
                }
            }
            else
            {
                int right = 0;
                foreach (TreeColumn col in Columns)
                {
                    if (col.IsVisible)
                    {
                        right += col.Width;
                        for (int i = 0; i < NodeControls.Count; i++)
                        {
                            NodeControl nc = NodeControls[i];
                            if (nc.Column == col.Index)
                            {
                                bool isLastControl = true;
                                for (int k = i + 1; k < NodeControls.Count; k++)
                                    if (NodeControls[k].Column == col.Index)
                                    {
                                        isLastControl = false;
                                        break;
                                    }

                                width = right - x;
                                if (!isLastControl)
                                    width = nc.MeasureSize(node).Width;
                                int maxWidth = Math.Max(0, right - x);
                                rect = new Rectangle(x, y, Math.Min(maxWidth, width), (int)RowHeight);
                                x += (width + 1);
                                yield return new NodeControlInfo(nc, rect);
                            }
                        }
                        x = right;
                    }
                }
            }
        }

        /// <MetaDataID>{a2e137a8-ffa3-4e23-87d9-6f0f28a04acc}</MetaDataID>
        private static double Dist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <MetaDataID>{84602ef8-1ee3-4c25-b93a-3ec2996d5060}</MetaDataID>
        private void SetDropPosition(Point pt)
        {
            TreeNode node = GetNodeAt(pt);
            _dropPosition.Node = node;
            if (node != null)
            {
                float pos = (pt.Y - ColumnHeaderHeight - ((node.Row - FirstVisibleRow) * (int)RowHeight)) / (float)RowHeight;
                if (pos < TopEdgeSensivity)
                    _dropPosition.Position = NodePosition.Before;
                else if (pos > (1 - BottomEdgeSensivity))
                    _dropPosition.Position = NodePosition.After;
                else
                    _dropPosition.Position = NodePosition.Inside;
            }
        }

        /// <MetaDataID>{4a2739c4-be31-4132-8ac0-33b63cb09a55}</MetaDataID>
        internal void FullUpdate()
        {
            CreateRowMap();
            SafeUpdateScrollBars();
            UpdateView();
            _needFullUpdate = false;
        }

        /// <MetaDataID>{6a7df317-32bb-46a9-b271-89d704808b71}</MetaDataID>
        internal void UpdateView()
        {
            if (!_suspendUpdate)
                Invalidate(false);
        }

        /// <MetaDataID>{af7eb1c7-c1cb-41cf-8043-0b83a6c10e34}</MetaDataID>
        private void CreateNodes()
        {
            Selection.Clear();
            SelectionStart = null;
            _root = new TreeNode(this, null);
            _root.IsExpanded = true;
            if (_root.Nodes.Count > 0)
                CurrentNode = _root.Nodes[0];
            else
                CurrentNode = null;
        }

        /// <MetaDataID>{5a2cfe80-22d1-4c57-ba15-fdfaddf79244}</MetaDataID>
        internal void ReadChilds(TreeNode parentNode)
        {
            ReadChilds(parentNode, null, RecursiveLoadSteps);
        }

        /// <MetaDataID>{0cf38bdf-686c-45ce-8882-a8a4bfee572d}</MetaDataID>
        void SetExpansionSign(TreeNode parentNode)
        {
            if (!parentNode.IsLeaf)
            {
                parentNode.IsExpandedOnce = true;
                if (Model != null)
                {
                    IEnumerable items = Model.GetChildren(GetPath(parentNode));
                    if (items != null)
                    {
                        parentNode.IsLeaf = true;
                        foreach (object obj in items)
                        {
                            parentNode.IsLeaf = false;
                            parentNode.IsExpandedOnce = false;
                            return;
                        }
                        foreach (TreeNode n in parentNode.Nodes)
                        {
                            n.Parent = null;
                        }
                        parentNode.Nodes.Clear();
                    }
                }
            }

        }

        /// <MetaDataID>{10c238d0-85f8-4a9f-8d59-907fcb5422a7}</MetaDataID>
        private void ReadChilds(TreeNode parentNode, Collection<ExpandedNode> expandedNodes, int recursiveStep)
        {
            if (!parentNode.IsLeaf)
            {
                parentNode.IsExpandedOnce = true;
                foreach (TreeNode n in parentNode.Nodes)
                {
                    n.Parent = null;
                }
                parentNode.Nodes.Clear();
                if (Model != null)
                {
                    IEnumerable items = Model.GetChildren(GetPath(parentNode));
                    if (items != null)
                    {

                        foreach (object obj in items)
                        {
                            Collection<ExpandedNode> expandedChildren = null;
                            if (expandedNodes != null)
                                foreach (ExpandedNode str in expandedNodes)
                                {
                                    if (str.Tag == obj)
                                    {


                                        expandedChildren = str.Children;
                                        break;
                                    }
                                }
                            AddNode(parentNode, obj, -1, expandedChildren, recursiveStep);
                        }
                    }
                }
                FullUpdate();
            }
        }

        /// <MetaDataID>{ac7d68c0-f816-415c-8e58-0ad373aadb5e}</MetaDataID>
        private void AddNode(TreeNode parent, object tag, int index, Collection<ExpandedNode> expandedChildren, int recursiveStep)
        {
            TreeNode node = new TreeNode(this, tag);
            node.Parent = parent;

            if (index >= 0 && index < parent.Nodes.Count)
                parent.Nodes.Insert(index, node);
            else
                parent.Nodes.Add(node);

            node.IsLeaf = Model.IsLeaf(GetPath(node));
            if (!LoadOnDemand)
                ReadChilds(node);
            else if (expandedChildren != null)
            {
                ReadChilds(node, expandedChildren, recursiveStep);
                node.IsExpanded = true;
            }
            if (recursiveStep > 0)
            {
                recursiveStep--;

                ReadChilds(node, null, recursiveStep);

            }
            else
                SetExpansionSign(node);

        }

        /// <MetaDataID>{60126c0f-0d64-45ec-af4e-9a6330fe1acb}</MetaDataID>
        private void AddNode(TreeNode parent, object tag, int index)
        {
            AddNode(parent, tag, index, null, -1);
        }
        //OOAdvantech.UserInterface.Runtime.OperationCaller _InsertRowOperationCaller;
        //public OOAdvantech.UserInterface.Runtime.OperationCaller InsertRowOperationCaller
        //{
        //    get
        //    {
        //        if (TreeViewMetaData == null || TreeViewMetaData.InsertOperation == null || UserInterfaceObjectConnection == null)
        //            return null;
        //        if (_InsertRowOperationCaller != null)
        //            return _InsertRowOperationCaller;
        //        _InsertRowOperationCaller = new OperationCaller(TreeViewMetaData.InsertOperation, this);
        //        return _InsertRowOperationCaller;
        //    }
        //}

        /// <MetaDataID>{a15c1c22-8f61-4e79-a584-6a31d8c67398}</MetaDataID>
        void InserNode()
        {
            if (SelectedNode == null || !(SelectedNode.Tag is NodeDisplayedObject))
                return;
            if (InsertNodeOperationCaller == null)
                return;
            SelectedNode.IsExpanded = true;
            object newobject = InsertNodeOperationCaller.Invoke();
            if (newobject == null)
                return;
            //(SelectedNode.Tag as NodeDisplayedObject)



            NodeDisplayedObject parent = SelectedNode.Tag as NodeDisplayedObject;

            this.UserInterfaceObjectConnection.Control(newobject);
            NodeDisplayedObject nodeDisplayedObject = new NodeDisplayedObject(this, newobject, null);
            //AddNode(SelectedNode, nodeDisplayedObject, 0);
            this.UserInterfaceObjectConnection.AddCollectionObject(parent.Value, ValueType, SubNodesProperty as string, newobject, parent, -1);


            SmartFullUpdate();




            //TOSHIBA SATELLITE P200-18Z
            //DisplayedValue displayedPresentationObj = null;
            //if (ValueType != PresentationObjectType && PresentationObjectType != null)
            //    displayedPresentationObj = UserInterfaceObjectConnection.GetPresentationObject(displayedObj.Value, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

        }

        /// <MetaDataID>{5ee2c899-2cc7-449a-8f22-5b64559cd8be}</MetaDataID>
        private void CreateRowMap()
        {
            _rowMap.Clear();
            int row = 0;
            _contentWidth = 0;
            foreach (TreeNode node in ExpandedNodes)
            {
                node.Row = row;
                _rowMap.Add(node);
                if (!UseColumns)
                {
                    Rectangle rect = GetNodeBounds(node);
                    _contentWidth = Math.Max(_contentWidth, rect.Right);
                }
                row++;
            }
            if (UseColumns)
            {
                _contentWidth = 0;
                foreach (TreeColumn col in _columns)
                    if (col.IsVisible)
                        _contentWidth += col.Width;
            }
        }

        /// <MetaDataID>{dbcb6060-727f-4749-a9d6-5eedd65503ed}</MetaDataID>
        private NodeControlInfo GetNodeControlInfoAt(TreeNode node, Point point)
        {
            foreach (NodeControlInfo info in GetNodeControls(node))
                if (info.Bounds.Contains(point))
                    return info;

            return NodeControlInfo.Empty;
        }

        /// <MetaDataID>{8ea0fba7-0400-4e1d-b1b6-99d079e64a10}</MetaDataID>
        private Rectangle GetNodeBounds(TreeNode node)
        {
            Rectangle res = Rectangle.Empty;
            foreach (NodeControlInfo info in GetNodeControls(node))
            {
                if (res == Rectangle.Empty)
                    res = info.Bounds;
                else
                    res = Rectangle.Union(res, info.Bounds);
            }
            return res;
        }

        /// <MetaDataID>{bc0ed386-d3f3-411d-824a-288933cd4b3a}</MetaDataID>
        private void _vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            FirstVisibleRow = _vScrollBar.Value;
        }

        /// <MetaDataID>{f236c341-0cc3-4044-a27c-d678602f8f03}</MetaDataID>
        private void _hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            OffsetX = _hScrollBar.Value;
        }

        /// <MetaDataID>{283a4296-72f1-4911-b3fa-3d7739176d6f}</MetaDataID>
        private void SetIsExpanded(TreeNode root, bool value)
        {
            foreach (TreeNode node in root.Nodes)
            {
                node.IsExpanded = value;
                SetIsExpanded(node, value);
            }
        }

        /// <MetaDataID>{5ab4fd92-e7a8-457b-8a49-921e791e150c}</MetaDataID>
        public void ClearSelection()
        {
            while (Selection.Count > 0)
                Selection[0].IsSelected = false;
        }

        /// <MetaDataID>{2080f1d5-52ef-484b-99ef-99ece1267444}</MetaDataID>
        internal void SmartFullUpdate()
        {
            if (_suspendUpdate || _structureUpdating)
                _needFullUpdate = true;
            else
                FullUpdate();
        }

        /// <MetaDataID>{83c82ba2-834a-4183-bae5-3de35040c31f}</MetaDataID>
        internal bool IsMyNode(TreeNode node)
        {
            if (node == null)
                return false;

            if (node.Tree != this)
                return false;

            while (node.Parent != null)
                node = node.Parent;

            return node == _root;
        }

        /// <MetaDataID>{ea56b626-36b8-4bac-b22a-be40949f1356}</MetaDataID>
        private void UpdateSelection()
        {
            bool flag = false;

            if (!IsMyNode(CurrentNode))
                CurrentNode = null;
            if (!IsMyNode(_selectionStart))
                _selectionStart = null;

            for (int i = Selection.Count - 1; i >= 0; i--)
                if (!IsMyNode(Selection[i]))
                {
                    flag = true;
                    Selection.RemoveAt(i);
                }

            if (flag)
                OnSelectionChanged();
        }

        /// <MetaDataID>{f7500913-7491-4ab8-971e-cc9ec2192f8d}</MetaDataID>
        internal void UpdateHeaders()
        {
            UpdateView();
        }

        /// <MetaDataID>{2033dade-b56a-408e-993d-de6996df6a5e}</MetaDataID>
        internal void UpdateColumns()
        {
            FullUpdate();
        }

        /// <MetaDataID>{435c6249-6cb2-431c-9a2b-5281696b7b1b}</MetaDataID>
        internal void ChangeColumnWidth(TreeColumn column)
        {
            if (!(_input is ResizeColumnState))
            {
                FullUpdate();
                OnColumnWidthChanged(column);
            }
        }

        #region Draw

        /// <MetaDataID>{319f2f74-16f4-4ecc-9186-2b227eae2e94}</MetaDataID>
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawContext context = new DrawContext();
            context.Graphics = e.Graphics;
            context.Font = this.Font;
            context.Enabled = Enabled;

            int y = 0;
            if (UseColumns)
            {
                DrawColumnHeaders(e.Graphics);
                y = ColumnHeaderHeight;
                if (Columns.Count == 0)
                    return;
            }

            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(-OffsetX, y - (FirstVisibleRow * (int)RowHeight));
            int row = FirstVisibleRow;
            while (row < RowCount && row - FirstVisibleRow <= PageRowCount)
            {
                TreeNode node = _rowMap[row];
                context.DrawSelection = DrawSelectionMode.None;
                context.CurrentEditorOwner = _currentEditorOwner;
                if (_dragMode)
                {
                    if ((_dropPosition.Node == node) && _dropPosition.Position == NodePosition.Inside)
                        context.DrawSelection = DrawSelectionMode.Active;
                }
                else
                {
                    if (node.IsSelected && Focused)
                        context.DrawSelection = DrawSelectionMode.Active;
                    else if (node.IsSelected && !Focused && !HideSelection)
                        context.DrawSelection = DrawSelectionMode.Inactive;
                }
                context.DrawFocus = Focused && CurrentNode == node;

                if (FullRowSelect)
                {
                    context.DrawFocus = false;
                    if (context.DrawSelection == DrawSelectionMode.Active || context.DrawSelection == DrawSelectionMode.Inactive)
                    {
                        Rectangle focusRect = new Rectangle(OffsetX, row * (int)RowHeight, ClientRectangle.Width, (int)RowHeight);
                        if (context.DrawSelection == DrawSelectionMode.Active)
                        {
                            e.Graphics.FillRectangle(SystemBrushes.Highlight, focusRect);
                            context.DrawSelection = DrawSelectionMode.FullRowSelect;
                        }
                        else
                        {
                            e.Graphics.FillRectangle(SystemBrushes.InactiveBorder, focusRect);
                            context.DrawSelection = DrawSelectionMode.None;
                        }
                    }
                }

                if (ShowLines)
                    DrawLines(e.Graphics, node);


                DrawNode(node, context);
                row++;
            }

            if (_dropPosition.Node != null && _dragMode)
                DrawDropMark(e.Graphics);
            // DrawCursorLine(e);
            e.Graphics.ResetTransform();
            DrawScrollBarsBox(e.Graphics);
        }

        /// <MetaDataID>{1cd084ab-ff3e-4f9e-aca6-dff634b4a0fe}</MetaDataID>
        private void DrawColumnHeaders(Graphics gr)
        {
            int x = 0;
            VisualStyleRenderer renderer = null;
            if (Application.RenderWithVisualStyles)
                renderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);

            DrawHeaderBackground(gr, renderer, new Rectangle(0, 0, ClientRectangle.Width + 10, ColumnHeaderHeight));
            gr.TranslateTransform(-OffsetX, 0);
            foreach (TreeColumn c in Columns)
            {
                if (c.IsVisible)
                {
                    Rectangle rect = new Rectangle(x, 0, c.Width, ColumnHeaderHeight);
                    x += c.Width;
                    DrawHeaderBackground(gr, renderer, rect);
                    c.Draw(gr, rect, Font);
                }
            }
        }

        /// <MetaDataID>{72d51f95-cbd6-4400-a8d1-5b87500a0736}</MetaDataID>
        private static void DrawHeaderBackground(Graphics gr, VisualStyleRenderer renderer, Rectangle rect)
        {
            if (renderer != null)
                renderer.DrawBackground(gr, rect);
            else
            {
                gr.FillRectangle(SystemBrushes.Control, rect);

                gr.DrawLine(SystemPens.ControlDark, rect.X, rect.Bottom - 2, rect.Right, rect.Bottom - 2);
                gr.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);

                gr.DrawLine(SystemPens.ControlDark, rect.Right - 2, rect.Y, rect.Right - 2, rect.Bottom - 2);
                gr.DrawLine(SystemPens.ControlLightLight, rect.Right - 1, rect.Y, rect.Right - 1, rect.Bottom - 1);
            }
        }

        /// <MetaDataID>{18a891db-d39c-4b6d-b1f4-caa4f8add7f0}</MetaDataID>
        public void DrawNode(TreeNode node, DrawContext context)
        {
            foreach (NodeControlInfo item in GetNodeControls(node))
            {
                context.Bounds = item.Bounds;
                context.Graphics.SetClip(item.Bounds);
                item.Control.Draw(node, context);
                context.Graphics.ResetClip();
            }
        }

        /// <MetaDataID>{14948f3e-9f48-47a6-ac2f-b630a92462ca}</MetaDataID>
        private void DrawScrollBarsBox(Graphics gr)
        {
            Rectangle r1 = DisplayRectangle;
            Rectangle r2 = ClientRectangle;
            gr.FillRectangle(SystemBrushes.Control,
                new Rectangle(r1.Right, r1.Bottom, r2.Width - r1.Width, r2.Height - r1.Height));
        }

        /// <MetaDataID>{1fb80a13-da86-406f-b5a6-6e818fc52539}</MetaDataID>
        private void DrawDropMark(Graphics gr)
        {
            if (_dropPosition.Position == NodePosition.Inside)
                return;

            Rectangle rect = GetNodeBounds(_dropPosition.Node);
            int right = DisplayRectangle.Right - LeftMargin + OffsetX;
            int y = rect.Y;
            if (_dropPosition.Position == NodePosition.After)
                y = rect.Bottom;
            gr.DrawLine(_markPen, rect.X, y, right, y);
        }



        /// <MetaDataID>{378e3897-e9a0-4672-ade3-cc80a667afe6}</MetaDataID>
        private void DrawLines(Graphics gr, TreeNode node)
        {
            if (UseColumns && Columns.Count > 0)
                gr.SetClip(new Rectangle(0, 0, Columns[0].Width, RowCount * (int)RowHeight + ColumnHeaderHeight));
            double textScale = gr.DpiX / 96;

            int row = node.Row;
            TreeNode curNode = node;
            while (curNode !=null&& curNode != _root)
            {
                int level = curNode.Level;
                int x = (level - 1) * _indent + (int)(NodePlusMinus.ImageSize * textScale) / 2 + LeftMargin;
                int width = NodePlusMinus.Width - (int)(NodePlusMinus.ImageSize * textScale) / 2;
                int y = row * (int)RowHeight + TopMargin;
                int y2 = y + (int)RowHeight;

                if (curNode == node)
                {
                    int midy = y + (int)RowHeight / 2;
                    gr.DrawLine(_linePen, x, midy, x + width, midy);
                    if (curNode.NextNode == null)
                        y2 = y + (int)RowHeight / 2;
                }

                if (node.Row == 0)
                    y = (int)RowHeight / 2;
                if (curNode.NextNode != null || curNode == node)
                    gr.DrawLine(_linePen, x, y, x, y2);

                curNode = curNode.Parent;
            }

            gr.ResetClip();
        }

        #endregion

        #region Editor
        /// <MetaDataID>{2a4c1d58-ffa4-43d4-ba63-aa948210dfc2}</MetaDataID>
        public void DisplayEditor(Control control, EditableControl owner)
        {
            if (control == null || owner == null)
                throw new ArgumentNullException();

            if (CurrentNode != null)
            {
                DisposeEditor();
                EditorContext context = new EditorContext();
                context.Owner = owner;
                context.CurrentNode = CurrentNode;
                context.Editor = control;

                SetEditorBounds(context);

                _currentEditor = control;
                _currentEditorOwner = owner;
                UpdateView();
                control.Parent = this;
                control.Focus();
                owner.UpdateEditor(control);
            }
        }

        /// <MetaDataID>{d0740bde-7fa0-4313-9b0c-2a6d63dc5a96}</MetaDataID>
        private void SetEditorBounds(EditorContext context)
        {
            foreach (NodeControlInfo info in GetNodeControls(context.CurrentNode))
            {
                if (context.Owner == info.Control && info.Control is EditableControl)
                {
                    Point p = ToViewLocation(info.Bounds.Location);
                    int width = DisplayRectangle.Width - p.X;
                    if (UseColumns && info.Control.Column < Columns.Count)
                    {
                        Rectangle rect = GetColumnBounds(info.Control.Column);
                        width = rect.Right - OffsetX - p.X;
                    }
                    context.Bounds = new Rectangle(p.X, p.Y, width, info.Bounds.Height);
                    ((EditableControl)info.Control).SetEditorBounds(context);
                    return;
                }
            }
        }

        /// <MetaDataID>{ccebc38e-9f99-4978-a7ba-436097139249}</MetaDataID>
        private Rectangle GetColumnBounds(int column)
        {
            int x = 0;
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].IsVisible)
                {
                    if (i < column)
                        x += Columns[i].Width;
                    else
                        return new Rectangle(x, 0, Columns[i].Width, 0);
                }
            }
            return Rectangle.Empty;
        }

        /// <MetaDataID>{af169eb1-1162-4f11-a2eb-46016f889752}</MetaDataID>
        public void HideEditor()
        {
            this.Focus();
            DisposeEditor();
        }

        /// <MetaDataID>{4fb83172-c4ba-4b66-bf0e-498adf30052d}</MetaDataID>
        public void UpdateEditorBounds()
        {
            if (_currentEditor != null)
            {
                EditorContext context = new EditorContext();
                context.Owner = _currentEditorOwner;
                context.CurrentNode = CurrentNode;
                context.Editor = _currentEditor;
                SetEditorBounds(context);
            }
        }

        /// <MetaDataID>{86b42015-35f3-4e2f-898d-8536858c04dc}</MetaDataID>
        private void DisposeEditor()
        {
            if (_currentEditor != null)
                _currentEditor.Parent = null;
            if (_currentEditor != null)
                _currentEditor.Dispose();
            _currentEditor = null;
            _currentEditorOwner = null;
        }
        #endregion

        #region ModelEvents
        /// <MetaDataID>{9356812e-2b4e-47bc-bc9b-6653d86071db}</MetaDataID>
        private void BindModelEvents()
        {
            _model.NodesChanged += new EventHandler<TreeModelEventArgs>(_model_NodesChanged);
            _model.NodesInserted += new EventHandler<TreeModelEventArgs>(_model_NodesInserted);
            _model.NodesRemoved += new EventHandler<TreeModelEventArgs>(_model_NodesRemoved);
            _model.StructureChanged += new EventHandler<TreePathEventArgs>(_model_StructureChanged);
        }

        /// <MetaDataID>{429b44c2-c25e-4f08-b124-1095b08328f8}</MetaDataID>
        private void UnbindModelEvents()
        {
            _model.NodesChanged -= new EventHandler<TreeModelEventArgs>(_model_NodesChanged);
            _model.NodesInserted -= new EventHandler<TreeModelEventArgs>(_model_NodesInserted);
            _model.NodesRemoved -= new EventHandler<TreeModelEventArgs>(_model_NodesRemoved);
            _model.StructureChanged -= new EventHandler<TreePathEventArgs>(_model_StructureChanged);
        }

        /// <MetaDataID>{48c3eb92-6d6b-488b-a0d5-c005e7893a4d}</MetaDataID>
        private void _model_StructureChanged(object sender, TreePathEventArgs e)
        {
            if (e.Path == null)
                throw new ArgumentNullException();

            TreeNode node = FindNode(e.Path);
            if (node != null)
            {
                Collection<ExpandedNode> expandedNodes = null;
                if (KeepNodesExpanded && node.IsExpanded)
                {
                    expandedNodes = FindExpandedNodes(node);
                }
                _structureUpdating = true;
                try
                {
                    ReadChilds(node, expandedNodes, RecursiveLoadSteps);
                    UpdateSelection();
                }
                finally
                {
                    _structureUpdating = false;
                }
                SmartFullUpdate();
            }
        }

        /// <MetaDataID>{c5e64fb7-0f23-4793-af3a-4ff68fc19d12}</MetaDataID>
        private Collection<ExpandedNode> FindExpandedNodes(TreeNode parent)
        {
            Collection<ExpandedNode> expandedNodes = null;
            expandedNodes = new Collection<ExpandedNode>();
            foreach (TreeNode child in parent.Nodes)
            {
                if (child.IsExpanded)
                {
                    ExpandedNode str = new ExpandedNode();
                    str.Tag = child.Tag;
                    str.Children = FindExpandedNodes(child);
                    expandedNodes.Add(str);
                }
            }
            return expandedNodes;
        }

        /// <MetaDataID>{136860c4-196f-4c56-9928-aab51ae08d64}</MetaDataID>
        private void _model_NodesRemoved(object sender, TreeModelEventArgs e)
        {
            TreeNode parent = FindNode(e.Path);
            if (parent != null)
            {
                if (e.Indices != null)
                {
                    List<int> list = new List<int>(e.Indices);
                    list.Sort();
                    for (int n = list.Count - 1; n >= 0; n--)
                    {
                        int index = list[n];
                        if (index >= 0 && index <= parent.Nodes.Count)
                        {
                            parent.Nodes[index].Parent = null;
                            parent.Nodes.RemoveAt(index);
                        }
                        else
                            throw new ArgumentOutOfRangeException("Index out of range");
                    }
                }
                else
                {
                    for (int i = parent.Nodes.Count - 1; i >= 0; i--)
                    {
                        for (int n = 0; n < e.Children.Length; n++)
                            if (parent.Nodes[i].Tag == e.Children[n])
                            {
                                parent.Nodes[i].Parent = null;
                                parent.Nodes.RemoveAt(i);
                                break;
                            }
                    }
                }
            }
            UpdateSelection();
            SmartFullUpdate();
        }

        /// <MetaDataID>{58068ef6-c0e9-4cfe-abb1-917b77ecdc3c}</MetaDataID>
        private void _model_NodesInserted(object sender, TreeModelEventArgs e)
        {
            if (e.Indices == null)
                throw new ArgumentNullException("Indices");

            TreeNode parent = FindNode(e.Path);
            if (parent != null)
            {
                for (int i = 0; i < e.Children.Length; i++)
                    AddNode(parent, e.Children[i], e.Indices[i]);
            }
            SmartFullUpdate();
        }

        /// <MetaDataID>{22639c37-d669-4a85-9d46-20abaa826273}</MetaDataID>
        private void _model_NodesChanged(object sender, TreeModelEventArgs e)
        {
            TreeNode parent = FindNode(e.Path);
            if (parent != null)
            {
                if (e.Indices != null)
                {
                    foreach (int index in e.Indices)
                    {
                        if (index >= 0 && index < parent.Nodes.Count)
                        {
                            TreeNode node = parent.Nodes[index];
                            Rectangle rect = GetNodeBounds(node);
                            _contentWidth = Math.Max(_contentWidth, rect.Right);
                        }
                        else
                            throw new ArgumentOutOfRangeException("Index out of range");
                    }
                }
                else
                {
                    foreach (TreeNode node in parent.Nodes)
                    {
                        foreach (object obj in e.Children)
                            if (node.Tag == obj)
                            {
                                Rectangle rect = GetNodeBounds(node);
                                _contentWidth = Math.Max(_contentWidth, rect.Right);
                            }
                    }
                }
            }
            SafeUpdateScrollBars();
            UpdateView();
        }

        /// <MetaDataID>{c009aad2-f8d0-4783-9705-410d68b5d34a}</MetaDataID>
        public TreeNode FindNode(TreePath path)
        {
            if (path.IsEmpty())
                return _root;
            else
                return FindNode(_root, path, 0);
        }

        /// <MetaDataID>{9a1c94b3-f1e1-44a1-9390-07725ccb3fd2}</MetaDataID>
        private TreeNode FindNode(TreeNode root, TreePath path, int level)
        {
            foreach (TreeNode node in root.Nodes)
                if (node.Tag == path.FullPath[level])
                {
                    if (level == path.FullPath.Length - 1)
                        return node;
                    else
                        return FindNode(node, path, level + 1);
                }
            return null;
        }
        #endregion







    }
}
