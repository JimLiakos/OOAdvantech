// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetConnectableControls.com
// *****************************************************************************

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.Menus.Collections;
using OOAdvantech.Transactions;
using ConnectableControls.PropertyEditors;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using System.Collections.Generic;


namespace ConnectableControls.Menus
{
    // Declare event signature
    public delegate void CommandHandler(MenuCommand item);

    // Should animation be shown?
    /// <MetaDataID>{d540b724-1b8e-417f-8cf0-b7e5b52b964a}</MetaDataID>
    public enum Animate
    {
        No,
        Yes,
        System
    }

    // How should animation be displayed?
    /// <MetaDataID>{97b38fc5-e3b4-4609-aecb-232e815d6e00}</MetaDataID>
    public enum Animation
    {
        System = 0x00100000,
        Blend = 0x00080000,
        SlideCenter = 0x00040010,
        SlideHorVerPositive = 0x00040005,
        SlideHorVerNegative = 0x0004000A,
        SlideHorPosVerNegative = 0x00040009,
        SlideHorNegVerPositive = 0x00040006
    }

    /// <MetaDataID>{B5E7163F-5295-48E9-B5C4-912C9FC64CAF}</MetaDataID>
    [ToolboxItem(false)]
    [DefaultProperty("Text")]
    [DefaultEvent("Click")]
    public class MenuCommand : Component, IConnectableControl
    {
        /// <MetaDataID>{ea83a088-b8e3-40a0-a431-9c345bf42c42}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{3d713e59-e87c-4331-a177-e27a1e9e290c}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{88f83e6d-b117-4f01-9339-f42d6db606bc}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{581f36ec-07e2-4f54-af68-de1fb04aa851}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{9305b79b-9070-4dbe-846b-41bdef502b1a}</MetaDataID>
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{4cb6779f-d960-44e7-8114-25dfe5bd32c6}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            if (propertyName == "ViewEditForm" && OnCommandOperationCaller.Operation != null)
                return true;
            if (propertyName == "ViewEditObjectSource" && OnCommandOperationCaller.Operation != null)
                return true;
            if (propertyName == "ViewEditForm" && string.IsNullOrEmpty(ViewEditObjectSource as string))
                return true;
            if (propertyName == "OnCommandOperationCall" &&
                (!string.IsNullOrEmpty(ViewEditObjectSource as string) || !string.IsNullOrEmpty(ViewEditForm as string)))
                return true;



            return false;
        }

        // Enumeration of property change events
        public enum Property
        {
            Text,
            Enabled,
            ImageIndex,
            ImageList,
            Image,
            Shortcut,
            Checked,
            RadioCheck,
            Break,
            Infrequent,
            Visible,
            Description
        }
        /// <MetaDataID>{53c83a98-8cd7-4492-850a-0aa2def72b72}</MetaDataID>
        public override string ToString()
        {
            if (Command != null)
                return "(Menu MetaData)";
            return base.ToString();
        }

        // Declare the property change event signature
        public delegate void PropChangeHandler(MenuCommand item, Property prop);

        // Instance fields
        /// <MetaDataID>{ab222a99-fd27-46a5-bc32-0116f4a89d3c}</MetaDataID>
        protected bool _IsDraged = false;
        /// <MetaDataID>{fb428ded-6f63-40b3-877f-60caf4b474b2}</MetaDataID>
        protected bool _visible;
        /// <MetaDataID>{a77b5868-56f3-4b99-8aca-a8f6d00c4c20}</MetaDataID>
        protected bool _break;
        /// <MetaDataID>{fe63b470-2dfa-4b08-a334-52c29db8624d}</MetaDataID>
        protected string _text;
        /// <MetaDataID>{e9aeb34d-0c42-43da-9baf-7d444650944e}</MetaDataID>
        protected string _toolTipText;
        //protected string _description;
        /// <MetaDataID>{2c9baac2-2aa3-4426-96b5-e8ad678ee15e}</MetaDataID>
        protected bool _enabled;
        /// <MetaDataID>{27ba7be7-e44d-4760-89bf-8921fb829485}</MetaDataID>
        protected bool _checked;
        /// <MetaDataID>{47c240cd-c32d-4f61-9d54-ff974d6129e1}</MetaDataID>
        protected int _imageIndex;
        /// <MetaDataID>{738da692-1824-40de-a242-81a94b779cc8}</MetaDataID>
        protected bool _infrequent;
        /// <MetaDataID>{e16e0024-1103-4268-95eb-0b4f220a0ac8}</MetaDataID>
        protected object _tag;
        /// <MetaDataID>{df756f29-b6a2-4eaa-868a-21816befdd5f}</MetaDataID>
        protected bool _radioCheck;
        /// <MetaDataID>{cb285fac-1fce-4ee8-bd5f-231584433072}</MetaDataID>
        protected Shortcut _shortcut;
        /// <MetaDataID>{b5ac67dd-9205-49f1-8345-dc5e3e068a69}</MetaDataID>
        protected ImageList _imageList;
        /// <MetaDataID>{e2786712-d4e5-482e-84ad-96e9f148adc3}</MetaDataID>
        protected Image _image;
        /// <MetaDataID>{bc03274b-986c-4626-a22b-711110d7ad93}</MetaDataID>
        protected MenuCommandCollection _menuItems;
        /// <MetaDataID>{4e8808f2-6810-472c-b3ef-e53fe06fc4c0}</MetaDataID>
        protected bool _separator;

        // Exposed events
        public event EventHandler Click;
        public event EventHandler Update;
        public event CommandHandler PopupStart;
        public event CommandHandler PopupEnd;
        public event PropChangeHandler PropertyChanged;

        /// <MetaDataID>{C79A8FCC-1E21-4395-8841-60AED96DCBEC}</MetaDataID>
        public MenuCommand()
        {
            InternalConstruct("MenuItem", null, -1, Shortcut.None, null);
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _OnCommandOperationCaller;
        /// <MetaDataID>{eef3d391-5747-412f-9ee9-56b376f4c9f7}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.OperationCaller OnCommandOperationCaller
        {
            get
            {
                if (OwnerControl == null)
                    return null;
                if (_Command == null || _Command.OnCommandOperationCall == null)
                    return null;
                if (_OnCommandOperationCaller != null)
                    return _OnCommandOperationCaller;
                _OnCommandOperationCaller = new OperationCaller(_Command.OnCommandOperationCall, OwnerControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource,this.Command);
                return _OnCommandOperationCaller;
            }
        }

        /// <MetaDataID>{9d3f20f3-1f35-459b-9426-23171982ac5e}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _OnCommandOperationCall;
        /// <MetaDataID>{178b70d2-d809-4581-8856-78294b428835}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object OnCommandOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (Command == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (Command.OnCommandOperationCall == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Command);
                        Command.OnCommandOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                    if (OnCommandOperationCaller == null || OnCommandOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(On command operation)");
                    else
                    {
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, OnCommandOperationCaller.Operation.Name);
                        _TransactionOption = TransactionOption.Suppress;
                        if (Command != null)
                            Command.TransactionOption = _TransactionOption;

                        _ViewEditObjectSource = "";
                        _ViewEditForm = "";

                    }
                    metaDataVaue.MetaDataAsObject = Command.OnCommandOperationCall;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    throw;
                }

            }
            set
            {
                _OnCommandOperationCaller = null;
            }
        }


        /// <MetaDataID>{eb98e3a4-a0e9-44e1-aded-b6829f266d76}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _OnIsEnableOperationCall;
        /// <MetaDataID>{78ec3c88-f3e7-4cac-a2bd-f04b9f87e6c7}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object OnIsEnabledOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (Command == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;
                    if (Command.IsEnableOperationCall == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Command);
                        Command.IsEnableOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                    if (IsEnableOperationCaller == null || IsEnableOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(On command operation)");
                    else
                    {
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, IsEnableOperationCaller.Operation.Name);
                        _TransactionOption = TransactionOption.Suppress;
                        if (Command != null)
                            Command.TransactionOption = _TransactionOption;
                        _ViewEditObjectSource = "";
                        _ViewEditForm = "";
                    }
                    metaDataVaue.MetaDataAsObject = Command.IsEnableOperationCall;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            set
            {
                _IsEnableOperationCaller = null;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _IsEnableOperationCaller;
        /// <MetaDataID>{eef3d391-5747-412f-9ee9-56b376f4c9f7}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.OperationCaller IsEnableOperationCaller
        {
            get
            {
                if (OwnerControl == null)
                    return null;
                if (_Command == null || _Command.IsEnableOperationCall == null)
                    return null;
                if (_IsEnableOperationCaller != null)
                    return _IsEnableOperationCaller;
                _IsEnableOperationCaller = new OperationCaller(_Command.IsEnableOperationCall, OwnerControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource, this.Command);
                return _IsEnableOperationCaller;
            }
        }

        /// <MetaDataID>{da99331d-7c86-4e01-b9a2-f138a18dad90}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _OnIsVisibleOperationCall;
        /// <MetaDataID>{84208c2f-0447-42d6-9c78-11b5daaab0e8}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object OnIsVisibledOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (Command == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;
                    if (Command.IsVisibleOperationCall == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Command);
                        Command.IsVisibleOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                    if (IsVisibleOperationCaller == null || IsVisibleOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(On command operation)");
                    else
                    {
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, IsVisibleOperationCaller.Operation.Name);
                        _TransactionOption = TransactionOption.Suppress;
                        if (Command != null)
                            Command.TransactionOption = _TransactionOption;
                        _ViewEditObjectSource = "";
                        _ViewEditForm = "";
                    }
                    metaDataVaue.MetaDataAsObject = Command.IsVisibleOperationCall;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            set
            {
                _IsVisibleOperationCaller = null;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _IsVisibleOperationCaller;
        /// <MetaDataID>{eef3d391-5747-412f-9ee9-56b376f4c9f7}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.OperationCaller IsVisibleOperationCaller
        {
            get
            {
                if (OwnerControl == null)
                    return null;
                if (_Command == null || _Command.IsVisibleOperationCall == null)
                    return null;
                if (_IsVisibleOperationCaller != null)
                    return _IsVisibleOperationCaller;
                _IsVisibleOperationCaller = new OperationCaller(_Command.IsVisibleOperationCall, OwnerControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource,this.Command);
                return _IsVisibleOperationCaller;
            }
        }




        /// <MetaDataID>{73537A18-DFE9-4F8C-A987-A0967AC2C4E7}</MetaDataID>
        public MenuCommand(string text)
        {
            InternalConstruct(text, null, -1, Shortcut.None, null);
        }
        /// <MetaDataID>{ee49e94b-163c-4202-943a-227cde9434a3}</MetaDataID>
        public MenuCommand(System.Xml.XmlElement menuElement)
        {

            InternalConstruct(menuElement.GetAttribute("Name"), null, -1, Shortcut.None, null);
            foreach (System.Xml.XmlElement subMenuElement in menuElement.ChildNodes)
            {
                MenuCommands.Add(new MenuCommand(subMenuElement));
            }
        }

        /// <MetaDataID>{c7ce5a71-8758-4c61-9c00-4682459056fc}</MetaDataID>
        OOAdvantech.UserInterface.MenuCommand _Command;
        /// <MetaDataID>{685a1885-a395-4ed4-a05b-10c9f380a021}</MetaDataID>
        public OOAdvantech.UserInterface.MenuCommand Command
        {
            get
            {
                return _Command;
            }
        }
        /// <MetaDataID>{c8aa3cfe-0862-48c5-b257-b3da1c1847f0}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl _OwnerControl;
        /// <MetaDataID>{c047634a-c848-42e5-8cbe-8ec8e9c62509}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl OwnerControl
        {
            get
            {
                return _OwnerControl;
            }
            set
            {
                _OwnerControl = value;
                if (MenuCommands != null)
                {
                    foreach (MenuCommand menuCommand in MenuCommands)
                        menuCommand.OwnerControl = value;
                }
            }

        }
        /// <MetaDataID>{4e473df1-aa09-4a68-9ab9-3f59d4334e3b}</MetaDataID>
        public MenuCommand(OOAdvantech.UserInterface.MenuCommand menuCommand, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl ownerControl)
        {
            OwnerControl = ownerControl;
            _Command = menuCommand;

            InternalConstruct(menuCommand.Name, null, -1, Shortcut.None, null);
            //_ShowFormAssembly = menuCommand.ShowFormAssembly;
            _ViewEditForm = menuCommand.ViewEditForm;
            _ViewEditObjectSource = menuCommand.ViewEditObjectSource;
            _RefreshOwnerControl = menuCommand.RefreshOwnerControl;
            _TransactionOption = menuCommand.TransactionOption;
            foreach (OOAdvantech.UserInterface.MenuCommand subMenuCommand in menuCommand.SubMenuCommands)
            {
                MenuCommands.Add(new MenuCommand(subMenuCommand, ownerControl));
            }
        }


        /// <MetaDataID>{c91c51f0-7c19-4fd4-a0f9-7d8037872dee}</MetaDataID>
        public System.Xml.XmlDocument GetAsXml()
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml("<Menu Name=\"Main\" />");
            document.DocumentElement.SetAttribute("Name", Text);
            foreach (MenuCommand menuCommand in MenuCommands)
            {
                if (menuCommand is CreateMenuCommand)
                    continue;
                menuCommand.GetXml(document.DocumentElement);
            }
            return document;


        }
        /// <MetaDataID>{058fd683-b155-47a2-8bde-aaa0cf3b727f}</MetaDataID>
        void GetXml(System.Xml.XmlElement parentElement)
        {
            System.Xml.XmlElement menuElement = parentElement.AppendChild(parentElement.OwnerDocument.CreateElement("Menu")) as System.Xml.XmlElement;
            menuElement.SetAttribute("Name", Text);

            foreach (MenuCommand menuCommand in MenuCommands)
            {
                if (menuCommand is CreateMenuCommand)
                    continue;
                menuCommand.GetXml(menuElement);
            }



        }




        /// <MetaDataID>{47910BE5-96DF-46FD-A7CD-565A2C594522}</MetaDataID>
        public MenuCommand(string text, EventHandler clickHandler)
        {
            InternalConstruct(text, null, -1, Shortcut.None, clickHandler);
        }

        /// <MetaDataID>{E4DCA83F-1D01-423C-AC1A-A97A21282F71}</MetaDataID>
        public MenuCommand(string text, Shortcut shortcut)
        {
            InternalConstruct(text, null, -1, shortcut, null);
        }

        /// <MetaDataID>{D303E910-3215-49A0-B809-3C0B14C53E01}</MetaDataID>
        public MenuCommand(string text, Shortcut shortcut, EventHandler clickHandler)
        {
            InternalConstruct(text, null, -1, shortcut, clickHandler);
        }

        /// <MetaDataID>{DE5FA49D-8A23-4AAD-AEA2-EA89DD60D79D}</MetaDataID>
        public MenuCommand(string text, ImageList imageList, int imageIndex)
        {
            InternalConstruct(text, imageList, imageIndex, Shortcut.None, null);
        }

        /// <MetaDataID>{F9A836A3-6DA0-4C2D-AFE1-5DEF13F30BA5}</MetaDataID>
        public MenuCommand(string text, ImageList imageList, int imageIndex, Shortcut shortcut)
        {
            InternalConstruct(text, imageList, imageIndex, shortcut, null);
        }

        /// <MetaDataID>{7C5FE4CE-4AE1-4CE1-AF06-01FA37C319B1}</MetaDataID>
        public MenuCommand(string text, ImageList imageList, int imageIndex, EventHandler clickHandler)
        {
            InternalConstruct(text, imageList, imageIndex, Shortcut.None, clickHandler);
        }

        /// <MetaDataID>{1FD3FD07-8C38-4F08-A721-FD9256297758}</MetaDataID>
        public MenuCommand(string text,
                           ImageList imageList,
                           int imageIndex,
                           Shortcut shortcut,
                           EventHandler clickHandler)
        {
            InternalConstruct(text, imageList, imageIndex, shortcut, clickHandler);
        }

        /// <MetaDataID>{7E11A7ED-4951-4FA6-9576-57901E26C7B0}</MetaDataID>
        protected void InternalConstruct(string text,
                                         ImageList imageList,
                                         int imageIndex,
                                         Shortcut shortcut,
                                         EventHandler clickHandler)
        {
            // Save parameters
            _text = text;
            if (_text == "-")
                _separator = true;
            else
                _separator = false;
            _imageList = imageList;
            _imageIndex = imageIndex;
            _shortcut = shortcut;
            //_description = text;

            if (clickHandler != null)
                Click += clickHandler;

            // Define defaults for others
            _enabled = true;
            _checked = false;
            _radioCheck = false;
            _break = false;
            _tag = null;
            _visible = true;
            _infrequent = false;
            _image = null;

            // Create the collection of embedded menu commands
            _menuItems = new MenuCommandCollection();
            _menuItems.Inserted += new CollectionChange(SubMenuInserted);
            _menuItems.Removed += new CollectionChange(SubMenuRemoved);
        }

        /// <MetaDataID>{eb0773bd-6c65-4866-867f-3e5e4eb5fe50}</MetaDataID>
        void SubMenuRemoved(int index, object value)
        {
            if ((value is MenuCommand) && Command != null && (value as MenuCommand).Command != null)
                Command.RemoveSubCommand((value as MenuCommand).Command);

        }

        /// <MetaDataID>{3966e401-d16d-427a-8bb7-18b66607baab}</MetaDataID>
        void SubMenuInserted(int index, object value)
        {
            if (value is CreateMenuCommand)
                return;
            if (value is MenuCommand)
            {
                if (Command != null && (value as MenuCommand).Command != null)
                    Command.AddSubCommand((short)index, (value as MenuCommand).Command);
                else if (Command != null && (value as MenuCommand).Command == null)
                {
                    (value as MenuCommand)._Command = Command.NewSubCommand((short)index, (value as MenuCommand).Text);
                }
            }

        }
        /*MenuCommand _ParentMenu;
        public MenuCommand ParentMenu
        {
            get{return _ParentMenu;}
            set{_ParentMenu=value;}
        }*/



        //public MenuCommandCollection EditMenuCommands
        //{
        //    get { return _menuItems; }
        //    set
        //    {

        //    }
        //}
        /// <MetaDataID>{2badbd42-cdb8-48ee-adf1-8e5c05f48488}</MetaDataID>
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        // [EditorAttribute(typeof(MenuCommandCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public MenuCommandCollection MenuCommands
        {
            get { return _menuItems; }

        }
        /// <MetaDataID>{91bfcbbd-c539-473e-89ad-d94f8c72da7e}</MetaDataID>
        [Category("Layout")]
        public bool Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
                if (_separator)
                    Text = "-";
                else
                    if (Text == "-")
                        Text = "";
            }
        }
        /// <MetaDataID>{b02f5036-9d78-47d4-adba-77b6a9ba21af}</MetaDataID>
        [Category("Appearance")]
        public string ToolTipText
        {
            get { return _toolTipText; }

            set
            {
                if (_toolTipText != value)
                {
                    _toolTipText = value;
                    //OnPropertyChanged(Property.Text);
                }
            }
        }
        ///// <MetaDataID>{B4CB12B8-54BB-488C-A2E0-B9AF48C52D21}</MetaDataID>
        //private string _ShowFormAssembly;
        ///// <MetaDataID>{C0CF28AD-5F8C-48C6-9F73-FE7C03DA1469}</MetaDataID>
        //[Editor(typeof(ConnectableControls.EditAssemply), typeof(System.Drawing.Design.UITypeEditor))]
        //public string ShowFormAssembly
        //{
        //    get
        //    {
        //        return _ShowFormAssembly;
        //    }
        //    set
        //    {
        //        if (_ShowFormAssembly != value)
        //        {
        //            _ShowFormAssembly = value;
        //            if (_Command != null)
        //                _Command.ShowFormAssembly = _ShowFormAssembly;
        //        }
        //    }
        //}

        /// <MetaDataID>{ec604182-6f1c-419d-9e2d-d4f12eb424c3}</MetaDataID>
        String _ViewEditObjectSource;
        /// <MetaDataID>{2ffffefd-2473-46b4-8485-d3a96b105cd0}</MetaDataID>
        [Category("View Edit object")]
        [Editor(typeof(ConnectableControls.PropertyEditors.FormControlsSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public object ViewEditObjectSource
        {
            get
            {
                if (_ViewEditObjectSource != null)
                    return _ViewEditObjectSource;
                else
                    return "";
            }
            set
            {
                if (OnCommandOperationCaller != null && OnCommandOperationCaller.Operation != null)
                    return;
                if ((value as string) == "none")
                {
                    value = null;
                    ViewEditForm = null;
                }
                if (value is string)
                    _ViewEditObjectSource = value as string;
                if (value == null)
                    _ViewEditObjectSource = null;
                if (Command != null)
                    Command.ViewEditObjectSource = _ViewEditObjectSource;

            }
        }
        /// <MetaDataID>{590be0c6-37eb-45a1-9b60-35b5dcb33284}</MetaDataID>
        String _ViewEditForm;
        /// <MetaDataID>{3e2cd961-f153-4123-8605-a1f0d043a28a}</MetaDataID>
        [Category("View Edit object")]
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object ViewEditForm
        {
            get
            {
                if (_ViewEditForm != null)
                    return _ViewEditForm;
                else
                    return "";
            }
            set
            {
                if (value is string)
                    _ViewEditForm = value as string;
                if (value == null)
                {
                    _ViewEditForm = null;
                    if (Command != null)
                        Command.ViewEditForm = _ViewEditForm;
                }


                if (value is OOAdvantech.MetaDataRepository.Class)
                {
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (value as OOAdvantech.MetaDataRepository.Class).GetAttributes(true))
                    {
                        if (attribute.Type.FullName == typeof(ConnectableControls.FormConnectionControl).FullName)
                        {
                            //Type formType=(value as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(Type)) as System.Type;
                            //System.Reflection.MethodInfo ShowDialogMethod = _ShowForm.GetMethod("ShowDialog", new Type[0]);
                            //  System.Reflection.PropertyInfo FormConnectionControlProeprty = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo;
                            // System.Reflection.FieldInfo FormConnectionControlFieldInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                            // if (FormConnectionControlProeprty != null || FormConnectionControlFieldInfo != null)
                            //{
                            if (_ViewEditForm != (value as OOAdvantech.MetaDataRepository.Class).FullName)
                            {
                                _ViewEditForm = (value as OOAdvantech.MetaDataRepository.Class).FullName;
                                _OnCommandOperationCaller = null;
                                if (Command != null)
                                {
                                    Command.OnCommandOperationCall = null;
                                    Command.ViewEditForm = _ViewEditForm;
                                }
                            }


                            //}

                            break;
                        }

                    }


                    //if (_ShowForm != null)
                    //{
                    //    if(_ShowForm.getm 
                    //}
                }



            }
        }

        /// <MetaDataID>{052a815b-ef6a-46c6-8510-7b9fb02d976c}</MetaDataID>
        [Category("Appearance")]
        [DefaultValue("MenuItem")]
        [Localizable(true)]
        public string Text
        {
            get { return _text; }

            set
            {
                if (_text != value)
                {

                    _text = value;
                    if (_text == "-")
                        _separator = true;
                    else
                        _separator = false;
                    if (Command != null)
                        Command.Name = value;
                    OnPropertyChanged(Property.Text);
                }
            }
        }

        /// <MetaDataID>{03e65b2e-e4a5-4194-95c5-847e9f09a2b0}</MetaDataID>
        OOAdvantech.Transactions.TransactionOption _TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
        /// <MetaDataID>{370bba75-3826-4387-8386-92375f2fd70f}</MetaDataID>
        [Category("View Edit object")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return _TransactionOption;
            }
            set
            {
                if (OnCommandOperationCaller != null && OnCommandOperationCaller.Operation != null)
                    return;
                _TransactionOption = value;
                if (Command != null)
                    Command.TransactionOption = _TransactionOption;

            }
        }

        /// <MetaDataID>{1979e6e8-1156-4b54-b0ef-490b84102fe3}</MetaDataID>
        bool _RefreshOwnerControl = false;
        /// <MetaDataID>{4dffc2be-2e33-452c-9f22-3d47ca3fb099}</MetaDataID>
        [Category("View Edit object")]
        public bool RefreshOwnerControl
        {
            get
            {
                return _RefreshOwnerControl;
            }
            set
            {
                _RefreshOwnerControl = value;
                if (Command != null)
                    Command.RefreshOwnerControl = _RefreshOwnerControl;
            }
        }



        /// <MetaDataID>{97aa995a-0a01-43ae-85f7-1abaa59da204}</MetaDataID>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return _enabled; }

            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnPropertyChanged(Property.Enabled);
                }
            }
        }
        /// <MetaDataID>{08ac0a45-a03b-46ed-be39-84bbf6be80c9}</MetaDataID>
        [Category("Appearance")]
        [Browsable(false)]
        [DefaultValue(-1)]
        public int ImageIndex
        {
            get { return _imageIndex; }

            set
            {
                if (_imageIndex != value)
                {
                    _imageIndex = value;
                    OnPropertyChanged(Property.ImageIndex);
                }
            }
        }

        /// <MetaDataID>{7b8ef6e7-2784-41c4-8d81-d4708d02134b}</MetaDataID>
        [Category("Appearance")]
        public string CommandID
        {
            get 
            { 
                return Command.CommandID; 
            }
        }


        /// <MetaDataID>{8aebd21b-91bd-4a1b-ab5e-dfb86d965b36}</MetaDataID>
        [Category("Appearance")]
        [Browsable(false)]
        [DefaultValue(null)]
        public ImageList ImageList
        {
            get { return _imageList; }

            set
            {
                if (_imageList != value)
                {
                    _imageList = value;
                    OnPropertyChanged(Property.ImageList);
                }
            }
        }

        /// <MetaDataID>{aeb27fc1-f1d4-4ed1-b369-4fe38f3b224d}</MetaDataID>
        [Category("Appearance")]
        [DefaultValue(null)]
        [EditorAttribute(typeof(System.Drawing.Design.ImageEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Image Image
        {
            get
            {
                if (_image != null)
                    return _image;
                if (_imageIndex != -1)
                    return _imageList.Images[_imageIndex];
                return null;
            }

            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(Property.Image);
                }
            }
        }
        /// <MetaDataID>{b4e5cc04-9369-4954-90a8-6daa4b246322}</MetaDataID>
        [Category("Behavior")]
        [DefaultValue(typeof(Shortcut), "None")]
        public Shortcut Shortcut
        {
            get { return _shortcut; }

            set
            {
                if (_shortcut != value)
                {
                    _shortcut = value;
                    OnPropertyChanged(Property.Shortcut);
                }
            }
        }
        /// <MetaDataID>{0570a5fe-bcdf-43eb-b3cc-f05a006279c9}</MetaDataID>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool Checked
        {
            get { return _checked; }

            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    OnPropertyChanged(Property.Checked);
                }
            }
        }
        /// <MetaDataID>{9fc30e55-0ad6-40bd-8239-ef53b1e6daa2}</MetaDataID>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool RadioCheck
        {
            get { return _radioCheck; }

            set
            {
                if (_radioCheck != value)
                {
                    _radioCheck = value;
                    OnPropertyChanged(Property.RadioCheck);
                }
            }
        }

        /// <MetaDataID>{60f7436c-15b8-42c7-9d22-a693910e58c4}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDraged
        {
            get { return _IsDraged; }
            set
            {
                _IsDraged = value;
            }


        }

        /// <MetaDataID>{80f65ea5-2e99-450f-91b7-d19512f63a5b}</MetaDataID>
        [Category("Layout")]
        [DefaultValue(false)]
        public bool Break
        {
            get { return _break; }

            set
            {
                if (_break != value)
                {
                    _break = value;
                    OnPropertyChanged(Property.Break);
                }
            }
        }
        /// <MetaDataID>{b5909c8f-979a-4a85-bce5-3157e02e85c3}</MetaDataID>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Infrequent
        {
            get { return _infrequent; }

            set
            {
                if (_infrequent != value)
                {
                    _infrequent = value;
                    OnPropertyChanged(Property.Infrequent);
                }
            }
        }

        /// <MetaDataID>{d1ed676a-c429-418d-a899-e748bcdc02cf}</MetaDataID>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Visible
        {
            get { return _visible; }

            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OnPropertyChanged(Property.Visible);
                }
            }
        }
        /// <MetaDataID>{81b38381-c231-4ca5-afb6-dc6152533371}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool IsParent
        {
            get { return (_menuItems.Count > 0); }
        }
        //[Category("Appearance")]
        //[DefaultValue("")]
        //[Localizable(true)]
        //public string Description
        //{
        //    get { return _description; }
        //    set { _description = value; }
        //}
        /// <MetaDataID>{8985778c-eba0-4a91-8b6e-58f97f1bc0f9}</MetaDataID>
        [Category("Behavior")]
        [DefaultValue(null)]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <MetaDataID>{8895441D-E9B9-4822-BAE2-63E126CC0CA7}</MetaDataID>
        public virtual void OnPropertyChanged(Property prop)
        {
            // Any attached event handlers?
            if (PropertyChanged != null)
                PropertyChanged(this, prop);
        }

        /// <MetaDataID>{B479CB0A-56F9-4BB0-932C-D9CA7E34E1AB}</MetaDataID>
        public void PerformClick()
        {
            // Update command with correct state
            OnUpdate(EventArgs.Empty);

            // Notify event handlers of click event
            OnClick(EventArgs.Empty);
        }

        /// <MetaDataID>{793a9e79-d31d-48b4-99db-00c88cb60589}</MetaDataID>
        object GetSourceValue(string source)
        {
            try
            {


                if (source == "$ViewControlObject$")
                    return UserInterfaceObjectConnection.Instance;

                if (string.IsNullOrEmpty(source))
                    return null;

                int npos = source.LastIndexOf(".");
                if (npos == -1)
                {
                    if (source.Trim() == "this")
                        return OwnerControl;
                    return UserInterfaceObjectConnection.GetControlWithName(source);
                }
                else
                {
                    if (source.Substring(0, npos) == "this")
                        return OwnerControl.GetPropertyValue(source.Substring(npos + 1));
                    else
                    {
                        IObjectMemberViewControl objectMemberViewControl = UserInterfaceObjectConnection.GetControlWithName(source.Substring(0, npos)) as IObjectMemberViewControl;
                        return objectMemberViewControl.GetPropertyValue(source.Substring(npos + 1));
                    }
                }
            }
            catch (System.Exception error)
            {

            }
            return null;


        }
        /// <MetaDataID>{FC87E685-FE0B-4320-B5C9-4D1690625F0C}</MetaDataID>
        public virtual void OnClick(EventArgs e)
        {
            try
            {
                if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.DesigneMode)
                    return;

                if (Command != null)
                    Command.UserInterfaceObjectConnection = UserInterfaceObjectConnection;

                if (!string.IsNullOrEmpty(ViewEditForm as string))
                {
                    object viewEditObject = GetSourceValue(ViewEditObjectSource as string);

                    OOAdvantech.MetaDataRepository.Classifier formClassifier =UserInterfaceObjectConnection.GetClassifier(ViewEditForm as string, true);
                    if (formClassifier != null)
                    {
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in formClassifier.GetAttributes(true))
                        {
                            if (attribute.Type.FullName == typeof(ConnectableControls.FormConnectionControl).FullName)
                            {

                                System.Reflection.PropertyInfo formConnectionControlProperty = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo;
                                System.Reflection.FieldInfo formConnectionControlField = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                if (formConnectionControlProperty != null || formConnectionControlField != null)
                                {
                                    Type formType = formClassifier.GetExtensionMetaObject(typeof(Type)) as System.Type;
                                    Form form = formType.Assembly.CreateInstance(formType.FullName) as Form;
                                    ConnectableControls.FormConnectionControl formConnectionControl = null;

                                    if (formConnectionControlProperty != null)
                                        formConnectionControl = formConnectionControlProperty.GetValue(form, null) as ConnectableControls.FormConnectionControl;

                                    if (formConnectionControlField != null)
                                        formConnectionControl = formConnectionControlField.GetValue(form) as ConnectableControls.FormConnectionControl;


                                    formConnectionControl.Instance = viewEditObject;
                                    System.Reflection.MethodInfo showDialogMethod = formType.GetMethod("ShowDialog", new Type[0]);
                                    UserInterfaceObjectConnection.Invoke(form, showDialogMethod, new object[0], TransactionOption);
                                    if (RefreshOwnerControl)
                                        UserInterfaceObjectConnection.UpdateUserInterfaceFor(viewEditObject);


                                    break;
                                }
                            }
                        }
                    }


                }
                else if (OnCommandOperationCaller != null && OnCommandOperationCaller.Operation != null)
                {
                    if (UserInterfaceObjectConnection == null)
                        return;
                    OnCommandOperationCaller.ExecuteOperationCall();
                }
                else
                {

                    // Any attached event handlers?
                    if (Click != null)
                    {

                        if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.Transaction != null)
                        {
                            if (UserInterfaceObjectConnection.Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                                Click(this, e);
                            
                            else
                            {
                                using (OOAdvantech.Transactions.SystemStateTransition suppressstateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                                {
                                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(UserInterfaceObjectConnection.Transaction))
                                    {
                                        using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition(_TransactionOption))
                                        {
                                            Click(this, e);

                                            if (OOAdvantech.Transactions.Transaction.Current != null && OOAdvantech.Transactions.Transaction.Current.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                                                innerStateTransition.Consistent = true;
                                        }

                                        stateTransition.Consistent = true;
                                    }
                                }
                            }


                        }
                        else
                        {
                            if (_TransactionOption == OOAdvantech.Transactions.TransactionOption.Suppress)
                            {
                                Click(this, e);
                            }
                            else
                            {
                                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(_TransactionOption))
                                {
                                    Click(this, e);
                                    stateTransition.Consistent = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                System.Windows.Forms.MessageBox.Show(error.Message);
            }
        }

        /// <MetaDataID>{FD26D862-5584-4F62-84F2-DD8DDBB28A15}</MetaDataID>
        public virtual void OnUpdate(EventArgs e)
        {
            // Any attached event handlers?
            if (Update != null)
                Update(this, e);
        }

        /// <MetaDataID>{8E59CA1C-A965-42D4-8830-785731A382D0}</MetaDataID>
        public virtual void OnPopupStart()
        {
            // Any attached event handlers?
            if (PopupStart != null)
                PopupStart(this);

        }

        /// <MetaDataID>{9DFE8B94-A3A9-4872-B74A-44DB167E71F2}</MetaDataID>
        public virtual void OnPopupEnd()
        {
            // Any attached event handlers?
            if (PopupEnd != null)
                PopupEnd(this);
        }

        /// <MetaDataID>{c4286ec9-7972-437b-9c3a-a77c2ff78e83}</MetaDataID>
        internal void AddSubCommand(int i, MenuCommand menuCommand)
        {
            MenuCommands.Insert(i, menuCommand);
            //if (Command != null && menuCommand.Command != null)
            //    Command.AddSubCommand((short)i,menuCommand.Command);
            //else if (Command != null && menuCommand.Command == null)
            //{
            //    menuCommand.Command=Command.NewSubCommand(menuCommand.Text);
            //}



        }

        /// <MetaDataID>{6528e756-7b4c-44f8-b2d4-11d1170e0d23}</MetaDataID>
        internal void RemoveSubCommand(MenuCommand menuCommand)
        {
            MenuCommands.Remove(menuCommand);
            //if (Command != null && menuCommand.Command != null)
            //    Command.RemoveSubCommand(menuCommand.Command);

        }

        /// <MetaDataID>{d4474214-dfa7-4c2a-acb6-091b35b2ce54}</MetaDataID>
        internal void Delete()
        {
            if (Command != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Command) != null)
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(Command);

        }

        /// <MetaDataID>{b8542ce1-a3d9-4eac-8c9b-04945548c6eb}</MetaDataID>
        internal void RemoveCreateMenuCommands()
        {
            MenuCommandCollection menuCommands = new MenuCommandCollection(MenuCommands);
            foreach (MenuCommand menuCommand in menuCommands)
            {
                if (menuCommand is CreateMenuCommand)
                    MenuCommands.Remove(menuCommand);
                else
                    menuCommand.RemoveCreateMenuCommands();
            }
        }

        /// <MetaDataID>{28b69af5-1dbd-429b-b5d4-bfe2a22af63d}</MetaDataID>
        public MenuCommandCollection GetAllMenuCommands()
        {
            MenuCommandCollection allMenuCommands = new MenuCommandCollection();
            GetAllMenuCommands(allMenuCommands);
            return allMenuCommands;


        }

        /// <MetaDataID>{8fe87d4f-7fcf-486c-838f-bea7f2484325}</MetaDataID>
        private void GetAllMenuCommands(MenuCommandCollection allMenuCommands)
        {
            foreach (MenuCommand menuCommand in MenuCommands)
            {
                if (menuCommand.MenuCommands.Count > 0)
                    menuCommand.GetAllMenuCommands(allMenuCommands);
                else
                    allMenuCommands.Add(menuCommand);
            }

        }

        /// <MetaDataID>{b28a0ef4-2023-49ae-9b07-ca34bd90d530}</MetaDataID>
        internal bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (OnCommandOperationCaller != null)
            {
                if (OnCommandOperationCaller != null)
                {
                    foreach (string error in OnCommandOperationCaller.CheckConnectionMetaData())
                    {
                        hasErrors = true;
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: " + OwnerControl.ToString() + " Menu  '" + Text + "' OnCommandOperationCall " + error, OwnerControl.UserInterfaceObjectConnection.ContainerControl.GetType().FullName));
                    }
                }
            }

            if (!string.IsNullOrEmpty(ViewEditObjectSource as string))
            {
                OOAdvantech.MetaDataRepository.Classifier classifier = GetSourceClassifier(ViewEditObjectSource as string);
                if (!string.IsNullOrEmpty(ViewEditForm as string) && classifier != null)
                {
                    OOAdvantech.MetaDataRepository.Class formClass = UserInterfaceObjectConnection.GetClassifier(ViewEditForm as string, true) as OOAdvantech.MetaDataRepository.Class;
                    if (formClass != null)
                    {
                        bool attributeFounded = false;
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in formClass.GetAttributes(true))
                        {
                            if (attribute.Type != null && attribute.Type.FullName == typeof(FormConnectionControl).FullName)
                            {
                                attributeFounded = true;
                                System.Type type = formClass.GetExtensionMetaObject(typeof(Type)) as Type;
                                System.Reflection.ConstructorInfo ctor = type.GetConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly, null, new Type[0], null);
                                if (ctor == null)
                                {
                                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: An Error check for class '" + formClass.FullName + "' can not be made because a default constructor with InitializeComponent() is missing.", formClass.FullName));
                                    continue;
                                }

                                Form formObject = ctor.Invoke(new object[0]) as Form;
                                System.Reflection.FieldInfo fieldInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
                                System.Reflection.PropertyInfo propertyInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo;

                                ConnectableControls.ViewControlObject viewControlObject = null;
                                if (fieldInfo != null)
                                    viewControlObject = fieldInfo.GetValue(formObject) as ConnectableControls.ViewControlObject;
                                if (propertyInfo != null)
                                    viewControlObject = propertyInfo.GetValue(formObject, null) as ConnectableControls.ViewControlObject;
                                if (viewControlObject != null)
                                {
                                    if (!classifier.IsA(viewControlObject.UserInterfaceObjectConnection.ObjectType) && viewControlObject.UserInterfaceObjectConnection.ObjectType != classifier)
                                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: " + OwnerControl.Name + " Menu  '" + Text + "' edit object " + classifier.FullName + " and  Form '" + formObject.Text + "' object " + viewControlObject.UserInterfaceObjectConnection.ObjectType.FullName + " type mismatch", OwnerControl.UserInterfaceObjectConnection.ContainerControl.GetType().FullName));
                                }
                                else
                                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: " + OwnerControl.Name + " Menu  '" + Text + "' error on ViewEditForm, system can't find form connection control in form :" + formClass.FullName, OwnerControl.UserInterfaceObjectConnection.ContainerControl.GetType().FullName));
                                break;
                            }


                        }
                        if (!attributeFounded)
                            errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: " + OwnerControl.Name + " Menu  '" + Text + "' error on property ViewEditForm, system can't find Form Connection Control in Form :" + formClass.FullName, OwnerControl.UserInterfaceObjectConnection.ContainerControl.GetType().FullName));
                    }
                    else
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: " + OwnerControl.Name + " Menu  '" + Text + "' error on property ViewEditForm, system can't find Form '" + (ViewEditForm as string) + "'.", OwnerControl.UserInterfaceObjectConnection.ContainerControl.GetType().FullName));


                }
                if (classifier == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: " + OwnerControl.Name + " Menu  '" + Text + "' error on property ViewEditObjectSource system can't find source", OwnerControl.UserInterfaceObjectConnection.ContainerControl.GetType().FullName));

            }



            foreach (MenuCommand menu in MenuCommands)
                hasErrors |= menu.ErrorCheck(ref errors);
            return hasErrors;
        }

        /// <MetaDataID>{0463a17a-0603-46db-9d26-3e2683f04f9e}</MetaDataID>
        internal bool IsOnEditMode = false;

        #region IConnectableControl Members

        /// <MetaDataID>{a8ea6b28-a6c2-4cda-bf7b-37a1f85d7e7c}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                if (OwnerControl == null)
                    return null;
                return OwnerControl.UserInterfaceObjectConnection;
            }
            set
            {

            }
        }

        /// <MetaDataID>{746a72f9-df72-45bd-82ec-991cd6c01286}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object UIMetaDataObject
        {
            get
            {
                return Command;
            }
            set
            {

            }
        }

        /// <MetaDataID>{9f3cf46f-1fc3-4cef-ab44-a90adffc49d7}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Operation")
            {
                if (metaObject is OOAdvantech.UserInterface.OperationCall && new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, OwnerControl).Operation != null)
                    return true;
                else
                    return false;
            }
            if (propertyDescriptor == "ViewEditForm")
            {
                OOAdvantech.MetaDataRepository.Classifier classifier = GetSourceClassifier(ViewEditObjectSource as string);
                if (metaObject is OOAdvantech.MetaDataRepository.Classifier)
                {
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (metaObject as OOAdvantech.MetaDataRepository.Classifier).GetAttributes(true))
                    {
                        if (attribute.Type != null && attribute.Type.FullName == typeof(FormConnectionControl).FullName)
                            return true;
                    }

                }

                return false;

            }
            return true;

        }
        /// <MetaDataID>{12d5ce1c-bbd2-4bf7-b7ec-7a73bd871c13}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier GetSourceClassifier(string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;
            if (source == "$ViewControlObject$")
                return UserInterfaceObjectConnection.ObjectType;


            int npos = source.LastIndexOf(".");
            if (npos == -1)
            {
                if (source.Trim() == "this")
                    return UserInterfaceObjectConnection.GetClassifier(OwnerControl.GetType().FullName, true);
                IObjectMemberViewControl control = UserInterfaceObjectConnection.GetControlWithName(source);
                if (control != null)
                    return UserInterfaceObjectConnection.GetClassifier(control.GetType().FullName, true);

            }
            else
            {
                if (source.Substring(0, npos) == "this")
                    return OwnerControl.GetPropertyType(source.Substring(npos + 1));
                else
                {
                    IObjectMemberViewControl objectMemberViewControl = UserInterfaceObjectConnection.GetControlWithName(source.Substring(0, npos)) as IObjectMemberViewControl;
                    if (objectMemberViewControl != null)
                        return OwnerControl.GetPropertyType(source.Substring(npos + 1));
                    return null;
                }
            }
            return null;
        }


        /// <MetaDataID>{8e5b26e0-688e-46aa-ae1b-335618720cd6}</MetaDataID>
        public bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return false;
        }

        #endregion
    }
}