using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.ListView;
using OOAdvantech.MetaDataRepository;
using ConnectableControls.PropertyEditors;
using OOAdvantech.UserInterface.Runtime;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Drawing;
using System.Xml.Linq;
//using OOAdvantech.Transactions;

namespace ConnectableControls
{
    /// <MetaDataID>{c9224f2e-57aa-4f52-884c-14eddc376d92}</MetaDataID>
    [ToolboxBitmap(typeof(ViewControlObject), "Table.bmp")]
    public class ListConnection : System.ComponentModel.Component, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer, OOAdvantech.UserInterface.Runtime.ICollectionViewRunTime, OOAdvantech.UserInterface.Runtime.ICutCopyPasteMoveDragDrop//, IListView
    {
        /// <MetaDataID>{18ced657-4029-4ec0-95fc-d4f5631fc963}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {
            if (newState == ViewControlObjectState.UserInteraction)
                SelectionChanged();


        }
        /// <MetaDataID>{8f158d0f-1181-4da0-9935-9a756c2e3a36}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{95930b8c-a606-47b5-97f1-ee9331e541cc}</MetaDataID>
        public void AddDependencyProperty(OOAdvantech.UserInterface.Runtime.IDependencyProperty dependencyProperty)
        {
            _DependencyProperties.Add(dependencyProperty);
        }
        /// <MetaDataID>{a9ef3e7e-c400-40f7-91df-17843db547a4}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{e4e480c8-6fec-4e1a-8fe0-814050ddfba0}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{6a7e1dc3-ef4c-4466-802a-b1c8953802e0}</MetaDataID>
        public void RemoveDependencyProperty(OOAdvantech.UserInterface.Runtime.IDependencyProperty dependencyProperty)
        {
            if (_DependencyProperties.Contains(dependencyProperty))
                _DependencyProperties.Remove(dependencyProperty);
        }

        /// <MetaDataID>{43cfc3f1-0342-4c18-ab85-49ec5b9b513b}</MetaDataID>
        public ListConnection(IListView hostingListView)
        {
            HostingListView = hostingListView;
            //if (HostingListView is Control)
            //    (HostingListView as Control).Click += new EventHandler(OnClick);


            HostingListView.Click += new EventHandler(OnClick);

        }
        /// <MetaDataID>{3f5c541c-349c-4e24-b5eb-568b095cd9af}</MetaDataID>
        new bool DesignMode
        {
            get
            {
                
                if (UserInterfaceObjectConnection!=null&&UserInterfaceObjectConnection.ContainerControl is Control && (UserInterfaceObjectConnection.ContainerControl as Control).Site != null)
                    return (UserInterfaceObjectConnection.ContainerControl as Control).Site.DesignMode;
                return false;
            }
        }

        /// <MetaDataID>{bbdd3194-ebf8-405c-8c3e-efe21bc5ab63}</MetaDataID>
        void OnClick(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
            {

                //if (Menu.MenuCommands.Count > 0)
                {
                    ConnectableControls.Menus.PopupMenu popupMenu = new ConnectableControls.Menus.PopupMenu(false);

                    RowMenu = new OOAdvantech.UserInterface.MenuCommand(ListViewMetaData.Menu);

                    if (BeforeShowContextMenuOperationCaller != null && BeforeShowContextMenuOperationCaller.Operation != null)
                        BeforeShowContextMenuOperationCaller.Invoke();

                    if (RowMenu.SubMenuCommands.Count > 0)
                    {
                        ConnectableControls.Menus.MenuCommand menu = new ConnectableControls.Menus.MenuCommand(RowMenu, this);
                        if (Site == null || !Site.DesignMode)
                        {
                            foreach (ConnectableControls.Menus.MenuCommand menuCommand in menu.GetAllMenuCommands())
                                menuCommand.Click += new EventHandler(MenuCommandClicked);

                        }
                        int returnDir = 0;
                        //Menu.Command
                        popupMenu.TrackPopup(
                            Control.MousePosition,
                            Control.MousePosition,
                            ConnectableControls.Menus.Common.Direction.Horizontal,
                            menu,
                            0,
                            ConnectableControls.Menus.GapPosition.None, false, null, false, ref returnDir);

                        if (Site == null || !Site.DesignMode)
                        {
                            foreach (ConnectableControls.Menus.MenuCommand menuCommand in menu.GetAllMenuCommands())
                                menuCommand.Click -= new EventHandler(MenuCommandClicked);

                        }

                    }

                }

            }
        }



        /// <MetaDataID>{91cdaf44-176b-4099-bded-6e048ae25ee1}</MetaDataID>
        public IListView HostingListView;




        #region Conectable controls code


        /// <MetaDataID>{c25f6c6f-bbd3-4684-8aef-2ade68c85fd1}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.List<IColumn> Columns
        {
            get
            {
                return HostingListView.Columns;

            }
        }

        /// <MetaDataID>{48e3e2bb-5386-486d-89a7-66db04df5ec3}</MetaDataID>
        public IColumn ChangeColumnType(IColumn selectedColumn, string columnType)
        {
            return HostingListView.ChangeColumnType(selectedColumn, columnType);
        }

        /// <MetaDataID>{787ded75-87f4-40ea-a498-983842b81432}</MetaDataID>
        public string GetColumnTypeName(IColumn SelectedColumn)
        {
            return HostingListView.GetColumnTypeName(SelectedColumn);


        }

        /// <MetaDataID>{3b21b250-c235-49f9-9f9a-371604df04e2}</MetaDataID>
        public List<string> GetColumnTypesNames()
        {
            return HostingListView.GetColumnTypesNames();

        }

        /// <MetaDataID>{BC902574-509B-45E7-B0DB-0325846F9A4E}</MetaDataID>
        internal class ColumnSort : System.Collections.IComparer
        {
            /// <MetaDataID>{4CA8E657-9B3B-4096-AD30-9AB9C57FFD36}</MetaDataID>
            static internal ColumnSort Sort
            {
                get
                {
                    return new ColumnSort();
                }
            }



            #region IComparer Members

            /// <MetaDataID>{3118DA0F-AE46-4AE4-8CD1-0E047C3AFB5D}</MetaDataID>
            public int Compare(object x, object y)
            {
                IColumn xColumn = x as IColumn;
                IColumn yColumn = y as IColumn;
                if (xColumn == null && yColumn == null)
                    return 0;
                if (xColumn == null && yColumn != null)
                    return -1;
                if (xColumn != null && yColumn == null)
                    return 1;
                return xColumn.Order.CompareTo(yColumn.Order);

            }

            #endregion
        }


        //public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        //{
        //    return true;
        //}


        //private bool _AutoDisable = true;
        //[Category("Object Model Connection")]
        //public bool AutoDisable
        //{
        //    get
        //    {
        //        return _AutoDisable;
        //    }
        //    set
        //    {
        //        _AutoDisable = value;
        //    }
        //}


        #region IPathDataDisplayer

        /// <MetaDataID>{3b9ad6c9-43b0-4af5-98f4-4483e176a075}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            List<OOAdvantech.UserInterface.Runtime.MemberChange> memberChanges = UserInterfaceObjectConnection.GetChanges(_Path, memberChangeEventArg, this);
            if (memberChanges.Count == 0)
                return;
            if (memberChanges.Count == 1 && memberChanges[0].Type == OOAdvantech.UserInterface.Runtime.ChangeType.ValueChanged)
                LoadControlValues();
            if (memberChanges.Count == 1 && memberChanges[0].Type == OOAdvantech.UserInterface.Runtime.ChangeType.ItemsRemoved)
                HostingListView.RemoveRowAt(memberChanges[0].Index);
            if (memberChanges.Count == 1 && memberChanges[0].Type == OOAdvantech.UserInterface.Runtime.ChangeType.ItemsAdded)
            {

                object displayedObj = memberChanges[0].Value;
                IPresentationObject displayedPresentationObj = null;
                if (ValueType != PresentationObjectType && PresentationObjectType != null)
                    displayedPresentationObj = UserInterfaceObjectConnection.GetPresentationObject(displayedObj, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type);
                if (HostingListView.DataSourceSupported)
                {
                    if (HostingListView.DataSource == null)
                    {
                        List<object> dataSource = new List<object>();
                        dataSource.Add(new RecordProxy(this, CollectionObjectsProxyType, displayedObj, displayedPresentationObj).GetTransparentProxy());
                        HostingListView.DataSource = dataSource;
                    }
                    else
                    {
                        (HostingListView.DataSource as System.Collections.IList).Insert(memberChanges[0].Index, new RecordProxy(this, CollectionObjectsProxyType, displayedObj, displayedPresentationObj).GetTransparentProxy());
                        UserInterfaceObjectConnection.Control(displayedObj);
                        HostingListView.RefreshDataSource();
                    }

                }
                else
                {

                    IRow row = HostingListView.InsertRow(memberChanges[0].Index, displayedObj, displayedPresentationObj);
                    UserInterfaceObjectConnection.Control(row.PresentationObject);
                }

            }


        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{85aad4f4-0601-4641-9b2a-9dabebac47cc}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                if (_AllPaths == null)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    foreach (ConnectableControls.ListView.IColumn column in HostingListView.Columns)
                    {
                        foreach (string path in column.Paths)
                        {
                            if (ValueType != PresentationObjectType)
                            {
                                if (path.IndexOf("RealObject.") == 0)
                                    _AllPaths.Add(_Path + "." + path.Substring("RealObject.".Length));
                            }
                            else
                                _AllPaths.Add(_Path + "." + path);
                        }
                    }
                    if (_PresentationObjectType != null)
                    {
                        foreach (string path in OOAdvantech.UserInterface.Runtime.PresentationObjectPaths.GetExtraPathsFor(_PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type))
                        {
                            if (path.IndexOf("RealObject.") == 0)
                                _AllPaths.Add(_Path + "." + path.Substring("RealObject.".Length));
                        }
                    }
                    foreach (string path in UserInterfaceObjectConnection.GetExtraPathsFor(_Path))
                    {
                        if (path.IndexOf("RealObject.") == 0)
                            _AllPaths.Add(_Path + "." + path.Substring("RealObject.".Length));
                        else
                            _AllPaths.Add(_Path + "." + path);
                    }

                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    foreach (IColumn column in HostingListView.Columns)
                    {
                        foreach (string path in column.Paths)
                        {
                            if (ValueType == _PresentationObjectType && _PresentationObjectType != null)
                            {
                                if (path.IndexOf("RealObject.") == 0)
                                    _AllPaths.Add(_Path + "." + path.Substring("RealObject.".Length));
                            }
                            else
                                _AllPaths.Add(_Path + "." + path);
                        }
                    }

                    return _AllPaths;
                }
                else
                    return _AllPaths;
            }
        }


        /// <MetaDataID>{3d6168b0-1151-4adb-bb0d-d36c10807ea2}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }
        /// <MetaDataID>{d2d1135b-4f53-47a1-9ced-eaf07768813d}</MetaDataID>
        public void LockStateChange(object sender)
        {

        }


        #endregion


        #region IOperetionCallerSource
        /// <MetaDataID>{59448445-dab5-4e05-b509-6b7016a694da}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[6] { "SelectedRowValue", "SelectedRowsValues", "DragDropObject", "InsertRowIndex", "RowMenu", "CellMenu" };
            }
        }
        /// <MetaDataID>{c1d5de89-30dc-4972-8018-d88b2f4a763d}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "RowMenu")
                return RowMenu;
            if (propertyName == "this")
                return this;

            if (propertyName == "DragDropObject")
                return DragDropObject;


            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "Value")
                return Value;

            if (propertyName == "SelectedRowValue")
            {
                if (HostingListView.DataSourceSupported)
                {
                    if (HostingListView.SelectedRowsIndicies.Length == 0)
                        return null;
                    else
                    {
                        
                        object value= RecordProxy.GetObject((HostingListView.DataSource as System.Collections.IList)[HostingListView.SelectedRowsIndicies[0]]);
                        if (value != null && _PresentationObjectType != null && _PresentationObjectType.GetExtensionMetaObject<System.Type>() == value.GetType())
                            value = (value as IPresentationObject).GetRealObject();
                        return value;
                    }
                }
                else
                {
                    if (HostingListView.SelectedRows.Count == 0)
                        return null;
                    else
                        return HostingListView.SelectedRows[0].CollectionObject;

                }
            }
            if (propertyName == "SelectedRowsValues")
            {
                Type listType = typeof(System.Collections.Generic.List<>).MakeGenericType(new Type[1] { this.CollectionObjectType.GetExtensionMetaObject<Type>() });
                IList list = System.Activator.CreateInstance(listType) as IList;

                if (HostingListView.DataSourceSupported)
                {
                    foreach (int index in HostingListView.SelectedRowsIndicies)
                    {
                        object value = RecordProxy.GetObject((HostingListView.DataSource as System.Collections.IList)[HostingListView.SelectedRowsIndicies[index]]);
                        if (value != null && _PresentationObjectType != null && _PresentationObjectType.GetExtensionMetaObject<System.Type>() == value.GetType())
                            value = (value as IPresentationObject).GetRealObject();
                        list.Add(value);
                    }
                    return list;
                }
                else
                {
                    foreach (var row in HostingListView.SelectedRows)
                        list.Add(row.CollectionObject);
                    return list;
                }
            }
            

            if (propertyName == "SelectedCellValue")
                if (HostingListView.SelectedRows.Count == 0)
                    return null;
                else
                    return null;
            if (propertyName == "InsertRowIndex")
            {
                if (HostingListView.DataSourceSupported)
                {
                    if (HostingListView.SelectedRowsIndicies.Length == 0)
                        return -1;
                    else
                        return HostingListView.SelectedRowsIndicies[HostingListView.SelectedRowsIndicies.Length - 1];


                }
                else
                {

                    if (HostingListView.SelectedRows.Count == 0)
                        return -1;
                    else
                        return HostingListView.SelectedRows[HostingListView.SelectedRows.Count - 1].Index;
                }
            }

            throw new Exception("There isn't property with name " + propertyName + ".");

        }
        /// <MetaDataID>{6b476c7d-1e90-486d-8f28-2153fd87bbf9}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {

            if (propertyName == "DragDropObject")
                return true;


            if (propertyName == "this")
                return true;
            //if (propertyName == "this.Value")
            //    return true;

            if (propertyName == "Value")
                return true;

            if (propertyName == "SelectedRowValue")
                return true;

            if (propertyName == "SelectedRowsValues")
                return true;
            
            if (propertyName == "SelectedCellValue")
                return true;
            if (propertyName == "InsertRowIndex")
                return true;


            return false;
        }
        /// <MetaDataID>{79aebc21-1cc6-409c-9f87-9cb1674adb7b}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");
        }
        /// <MetaDataID>{7b0e9eac-436f-4e27-9551-2a78dcd33644}</MetaDataID>
        public Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "InsertRowIndex")
                return UserInterfaceObjectConnection.GetClassifier(typeof(int));

            if (propertyName == "Value")
                return ValueType;
            if (propertyName == "SelectedRowValue")
                return this.CollectionObjectType;

            if (propertyName == "DragDropObject")
                return UserInterfaceObjectConnection.GetClassifier(typeof(System.Object));

            if (propertyName == "RowMenu" || propertyName == "CellMenu")
                return UserInterfaceObjectConnection.GetClassifier(typeof(OOAdvantech.UserInterface.MenuCommand));

            if (propertyName == "SelectedRowsValues")
            {
                Type enumeratorType = typeof(System.Collections.Generic.List<>).MakeGenericType(new Type[1] { this.CollectionObjectType.GetExtensionMetaObject<Type>() });
                return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(enumeratorType);
            }

            return null;

        }

        #endregion

        #region IObjectMemberViewControl Members
        /// <MetaDataID>{ea097b29-41d6-4c42-a380-1cf67f1d3adf}</MetaDataID>
        public object DragDropObject;
        /// <exclude>Excluded</exclude>
        bool _AllowDrag = false;
        /// <MetaDataID>{570b3ac4-97ba-4267-bb13-9ea00b9afc29}</MetaDataID>
        [Category("DragDrop Behavior")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
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

        #region Menu code

        /// <exclude>Excluded</exclude>
        ConnectableControls.Menus.MenuCommand _Menu = null;

        /// <MetaDataID>{790f6114-7337-4438-b1a9-ca913e85f5d3}</MetaDataID>
        OOAdvantech.UserInterface.MenuCommand RowMenu;
        /// <MetaDataID>{0ee1bebe-a961-449b-8591-7c2bb5f19b3e}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectableControls.Menus.MenuCommand Menu
        {
            get
            {
                if (_Menu == null)
                {
                    object metadata = MetaData;//Load meta data creates list view
                    _Menu = new ConnectableControls.Menus.MenuCommand(ListViewMetaData.Menu, this);
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


        /// <exclude>Excluded</exclude>
        private ConnectableControls.Menus.MenuCommand _EditMenuCommand;
        /// <MetaDataID>{d8b1cce4-6160-4411-a535-15ac45ddc99f}</MetaDataID>
        [Category("List Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.SelectMenuCommand), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object EditMenuCommand
        {
            get
            {
                if (DesignMode)
                {
                    if (ListViewMetaData.EditMenuCommand != null)
                        return new ConnectableControls.PropertyEditors.MenuSelection(ListViewMetaData.EditMenuCommand.Name, Menu);
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
                    ListViewMetaData.EditMenuCommand = editMenuCommand;
                    if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                        !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    {
                        TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                    }

                }

            }
        }


        /// <exclude>Excluded</exclude>
        private ConnectableControls.Menus.MenuCommand _DeleteMenuCommand;
        /// <MetaDataID>{9e6d28d1-5df4-4f1a-b6c4-388388ba62bf}</MetaDataID>
        [Category("List Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.SelectMenuCommand), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object DeleteMenuCommand
        {
            get
            {
                if (DesignMode)
                {
                    if (ListViewMetaData.DeleteMenuCommand != null)
                        return new ConnectableControls.PropertyEditors.MenuSelection(ListViewMetaData.DeleteMenuCommand.Name, Menu);
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
                    ListViewMetaData.DeleteMenuCommand = deleteMenuCommand;
                    if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                        !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    {
                        TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                    }

                }

            }
        }


        /// <exclude>Excluded</exclude>
        private ConnectableControls.Menus.MenuCommand _InsertMenuCommand;
        /// <MetaDataID>{7809cbc8-5766-4669-887a-2dd5a79bc06f}</MetaDataID>
        [Category("List Menu")]
        [Editor(typeof(ConnectableControls.PropertyEditors.SelectMenuCommand), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object InsertMenuCommand
        {
            get
            {
                if (DesignMode)
                {
                    if (ListViewMetaData.InsertMenuCommand != null)
                        return new ConnectableControls.PropertyEditors.MenuSelection(ListViewMetaData.InsertMenuCommand.Name, Menu);
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
                    ListViewMetaData.InsertMenuCommand = insertMenuCommand;
                    if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                        !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    {
                        TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                    }

                }

            }
        }


        /// <MetaDataID>{b0d1043b-fe75-4459-9118-8f476bac7448}</MetaDataID>
        [Editor(typeof(ConnectableControls.PropertyEditors.EditMenuMetadata), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("List Menu")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
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



        /// <MetaDataID>{f2d7d1c0-6b68-48f7-b09b-78125dbb371b}</MetaDataID>
        void MenuCommandClicked(object sender, EventArgs e)
        {

            ConnectableControls.Menus.MenuCommand menucommand = sender as ConnectableControls.Menus.MenuCommand;
            if (ListViewMetaData.DeleteMenuCommand != null && menucommand.Command.CommandID == ListViewMetaData.DeleteMenuCommand.CommandID)
            {
                foreach (int index in HostingListView.SelectedRowsIndicies)
                    DeleteRow(index);
                return;
            }
            if (ListViewMetaData.InsertMenuCommand != null && menucommand.Command.CommandID == ListViewMetaData.InsertMenuCommand.CommandID)
            {
                HostingListView.InsertRow();
                return;
            }
            if (ListViewMetaData.EditMenuCommand != null && menucommand.Command.CommandID == ListViewMetaData.EditMenuCommand.CommandID)
            {
                if (HostingListView.DataSourceSupported)
                {
                    if (HostingListView.SelectedRowsIndicies.Length == 0)
                        return;
                    else
                    {
                        foreach (int index in HostingListView.SelectedRowsIndicies)
                        {
                            EditRow(index);
                        }
                    }
                }
                else
                {


                    foreach (IRow row in HostingListView.SelectedRows)
                        EditRow(row);
                }

           

                return;
            }




            if (menucommand.OnCommandOperationCaller != null)
            {
                if (UserInterfaceObjectConnection == null)
                    return;
                menucommand.OnCommandOperationCaller.ExecuteOperationCall();
            }
            else
                menucommand.Command.MenuClicked(GetPropertyValue("SelectedRowValue"));

            if (!string.IsNullOrEmpty(menucommand.ViewEditForm as string))
            {
                OOAdvantech.MetaDataRepository.Classifier formClassifier = this.UserInterfaceObjectConnection.GetClassifier(menucommand.ViewEditForm as string, true);
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


                            formConnectionControl.Instance = HostingListView.LastMouseOverRow.CollectionObject;
                            formConnectionControl.ContainerControl = form;
                            System.Reflection.MethodInfo showDialogMethod = formType.GetMethod("ShowDialog", new Type[0]);
                            try
                            {
                                UserInterfaceObjectConnection.Invoke(form, showDialogMethod, new object[0], menucommand.TransactionOption);
                            }
                            catch (System.Exception error)
                            {

                            }


                            break;


                        }
                    }
                }
            }


        }

        /// <MetaDataID>{75f0186f-006f-4c3e-9365-350fd030b5d1}</MetaDataID>
        public void EditRow(IRow row)
        {
            if (EditRowOperationCaller == null)
                return;

            if (EditRowOperationCaller.Operation != null)
            {
                object reVal = EditRowOperationCaller.Invoke();
                ViewControlObject.UserInterfaceObjectConnection.UpdateUserInterfaceFor(row.CollectionObject);
            }
        }
        /// <MetaDataID>{6f93d3a6-6db8-41ee-aba1-9bef69d337c2}</MetaDataID>
        public void EditRow(int rowIndex)
        {
            if (EditRowOperationCaller == null)
                return;

            if (EditRowOperationCaller.Operation != null)
            {
                object reVal = EditRowOperationCaller.Invoke();
                ViewControlObject.UserInterfaceObjectConnection.UpdateUserInterfaceFor(RecordProxy.GetObject((HostingListView.DataSource as System.Collections.IList)[rowIndex]));
            }
        }


        //[Category("List Menu")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[EditorAttribute(typeof(ConnectableControls.Menus.Collections.MenuCommandCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //public ConnectableControls.Menus.Collections.MenuCommandCollection MenuCommands
        //{
        //    get
        //    {
        //        return Menu.MenuCommands;
        //    }
        //}


        //MenuEdit _MenuData;
        //[Category("List Menu")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[EditorAttribute(typeof(ConnectableControls.Menus.Collections.MenuCommandCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //public MenuEdit MenuData
        //{
        //    get
        //    {
        //        if (_MenuData == null)
        //            _MenuData = new MenuEdit(Menu);
        //        return _MenuData;
        //    }
        //    set
        //    {
        //    }

        //}
        #endregion


        /// <MetaDataID>{4893aa04-987b-4ea1-ad3d-1547f33d06f0}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && ValueType == null)
            {
                errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ListView '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).FindForm().GetType().FullName));
                hasErrors = true;
            }
            if (ListViewMetaData == null || UserInterfaceObjectConnection == null)
                return hasErrors;

            if (ListViewMetaData.InsertOperation != null)
            {
                foreach (string error in UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.InsertOperation].CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ListView '" + Name + ".InsertRowOperationCall' " + error, (UserInterfaceObjectConnection.ContainerControl as Control).FindForm().GetType().FullName));
                }
            }
            if (ListViewMetaData.DeleteOperation != null)
            {
                foreach (string error in UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.DeleteOperation].CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ListView '" + Name + ".DeleteRowOperationCall' " + error, (UserInterfaceObjectConnection.ContainerControl as Control).FindForm().GetType().FullName));
                }
            }
            if (ListViewMetaData.EditOperation != null)
            {
                foreach (string error in UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.EditOperation].CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ListView '" + Name + ".EditRowOperationCall' " + error, (UserInterfaceObjectConnection.ContainerControl as Control).FindForm().GetType().FullName));
                }
            }
            foreach (IColumn column in HostingListView.Columns)
            {
                hasErrors |= (column).ErrorCheck(ref errors);
            }


            if (Menu != null)
            {
                hasErrors |= Menu.ErrorCheck(ref errors);
            }

            if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
            {
                _PresentationObjectType = ViewControlObject.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ListView '" + Name + "' has invalid PresentationObject.", (UserInterfaceObjectConnection.ContainerControl as Control).FindForm().GetType().FullName));
                else
                {
                    if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(_PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType))
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ListView '" + Name + "' You can't assign the '" + _PresentationObjectType.FullName + "' to the PresentationObjectType.\n Check the property rules.", (UserInterfaceObjectConnection.ContainerControl as Control).FindForm().GetType().FullName));
                }
            }

            return hasErrors;
        }
        /// <MetaDataID>{2D0AD6A1-F3AF-4CE9-94F3-DF88859D3E83}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if ((context.PropertyDescriptor.Name == "Path" || context.PropertyDescriptor.Name == "AssignPresentationObjectType") && UserInterfaceObjectConnection != null)
            {
                if (context.PropertyDescriptor.Name == "Path")
                    return UserInterfaceObjectConnection.PresentationObjectType;
                else
                    return AssemblyManager.GetActiveWindowProject();


            }
            if (context.PropertyDescriptor.Name == "SelectionMember")
                return UserInterfaceObjectConnection.PresentationObjectType;
            return null;
        }


        /// <exclude>Excluded</exclude>
        object _Value;

        /// <MetaDataID>{9738fe6f-0ef0-43d9-9a09-ef9fd24644aa}</MetaDataID>
        [Category("Object Model Connection")]
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
                if (value == null)
                    return;

                HostingListView.RemoveAllRows();

                object objectCollection = _Value;

                IEnumerator enumerator = objectCollection.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(objectCollection, new object[0]) as IEnumerator;

                OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
                paths.Add("Root");
                foreach (IColumn column in HostingListView.Columns)
                {
                    foreach (string path in column.Paths)
                    {
                        if (ValueType != PresentationObjectType)
                        {
                            if (path.IndexOf("RealObject.") == 0)
                                paths.Add("Root." + path.Substring("RealObject.".Length));
                        }
                        else
                            paths.Add("Root." + path);
                    }
                }
                UserInterfaceObjectConnection.BatchLoadPathsValues(enumerator, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type, paths);
                enumerator.Reset();
                // object obj = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;

                    bool returnValueAsCollection = false;
                    this.UserInterfaceObjectConnection.Control(obj);
                    IPresentationObject displayedPresentationObj = null;
                    if (ValueType != PresentationObjectType && PresentationObjectType != null)
                        displayedPresentationObj = UserInterfaceObjectConnection.GetPresentationObject(obj, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type);
                    IRow row = HostingListView.InsertRow(-1, obj, displayedPresentationObj);
                    UserInterfaceObjectConnection.Control(row.PresentationObject);
                }

            }
        }

        /// <MetaDataID>{f5603905-8672-4977-a490-727644a286dc}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                return CollectionObjectType;
            }

        }

        /// <exclude>Excluded</exclude>
        private string _Path;

        /// <MetaDataID>{5ED59BC4-4269-41B8-B1EE-C5262355FFED}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor)),
        Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public Object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _CollectionObjectType = null;

                string newPath = null;
                if (value is MetaData)
                    newPath = (value as MetaData).Path;
                if (value is string)
                    newPath = value as string;


                if ((_Path) != (newPath))
                {
                    if (_UserInterfaceObjectConnection != null)
                    {
                        _CollectionObjectType = null;
                        if (_Path != null)
                            _MetaData = null;
                    }

                    _Path = newPath;
                }
            }
        }
        /// <MetaDataID>{296F9210-F98C-40CD-917B-727D5F7427BA}</MetaDataID>
        public void SaveControlValues()
        {
        }
        /// <exclude>Excluded</exclude>
        Type _CollectionObjectsProxyType;
        /// <MetaDataID>{65d07d77-e9a4-45e5-83f3-1b60cbca7630}</MetaDataID>
        Type CollectionObjectsProxyType
        {
            get
            {
                if (_CollectionObjectsProxyType == null)
                {
                    TypeBuilder typeBuilder = RecordProxy.GetInterfaceTypeBuilder("IGridViewCollectionObject" + GetHashCode());
                    foreach (OOAdvantech.UserInterface.Column column in this.ListViewMetaData.Columns)
                    {
                        Type propertyType = null;

                        if (string.IsNullOrEmpty(column.DisplayMember))
                            propertyType = UserInterfaceObjectConnection.GetType(PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type, column.Path);
                        else
                            propertyType = UserInterfaceObjectConnection.GetType(PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type, column.Path + "." + column.DisplayMember);
                        RecordProxy.CreateProperty(typeBuilder, propertyType, column.Name);

                    }
                    _CollectionObjectsProxyType = typeBuilder.CreateType();
                }
                return _CollectionObjectsProxyType;
            }
        }
        //System.Collections.Generic.List<object> DisplayedData = new List<object>();
        /// <MetaDataID>{8E462EEE-2FAD-4F78-A4AF-B2053AA2BC5F}</MetaDataID>
        public void LoadControlValues()
        {
            //return;
            try
            {

                if (!HostingListView.DataSourceSupported)
                    HostingListView.RemoveAllRows();
                else
                    if (HostingListView.DataSource is List<object>)
                        (HostingListView.DataSource as List<object>).Clear();


                List<object> dataSource = null;
                if (HostingListView.DataSourceSupported)
                {
                    if (HostingListView.DataSource is List<object>)
                        dataSource = HostingListView.DataSource as List<object>;
                    else
                        dataSource = new List<object>();
                }





                UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                // object collectionDisplayedValue = null;
                object objectCollection = null;
                bool returnValueAsCollection = false;
                if (string.IsNullOrEmpty(_Path))
                {
                    if (LoadListOperationCaller==null|| LoadListOperationCaller.Operation == null)
                        return;
                    else
                    {
                        Value = LoadListOperationCaller.Invoke();
                        return;

                    }
                }
                else
                {
                    objectCollection = UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                }
                if (!returnValueAsCollection || objectCollection == null)
                    return;





                //object rr = CollectionObjectType;

                //_CollectionObjectType = ViewControlObject.GetClassifier(_Path as string).TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                IEnumerator enumerator = objectCollection.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(objectCollection, new object[0]) as IEnumerator;
                enumerator.Reset();
                // object obj = enumerator.Current;
                while (enumerator.MoveNext())
                {

                    object obj = enumerator.Current;
                    this.UserInterfaceObjectConnection.Control(obj);

                    IPresentationObject displayedPresentationObj = null;
                    if (ValueType != PresentationObjectType && PresentationObjectType != null)
                        displayedPresentationObj = UserInterfaceObjectConnection.GetPresentationObject(obj, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                    if (!HostingListView.DataSourceSupported)
                    {
                        IRow row = HostingListView.InsertRow(-1, obj, displayedPresentationObj);
                        UserInterfaceObjectConnection.Control(row.PresentationObject);
                    }
                    else
                    {
                        RecordProxy recordProxy = new RecordProxy(this, CollectionObjectsProxyType, obj, displayedPresentationObj);
                        dataSource.Add(recordProxy.GetTransparentProxy());
                    }
                }

                if (HostingListView.DataSourceSupported)
                {
                    if (HostingListView.DataSource == null && dataSource.Count > 0)
                        HostingListView.DataSource = dataSource;
                    else
                        HostingListView.RefreshDataSource();
                }
                
                if (!string.IsNullOrEmpty(_SelectionMember))
                {
                    object obj = UserInterfaceObjectConnection.GetDisplayedValue(_SelectionMember, this, out returnValueAsCollection);
                    if (HostingListView.DataSourceSupported)
                    {
                        HostingListView.SelectRow(-1);
                        if (!returnValueAsCollection)
                        {
                            if (obj is DisplayedValue)
                                obj = (obj as DisplayedValue).Value;
                            if (dataSource.Contains(obj))
                                HostingListView.SelectRow(dataSource.IndexOf(obj));
                        }
                    }
                }

            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{59f5456a-403d-48b3-99c2-c266204cce86}</MetaDataID>
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
        /// <exclude>Excluded</exclude>
        UpdateStyle _UpdateStyle;
        /// <MetaDataID>{d83d08bd-b16e-4ac1-949a-dadccc4c03a7}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateStyle UpdateStyle
        {
            get
            {
                return _UpdateStyle;
            }
            set
            {
                _UpdateStyle = value;

            }
        }


        #endregion

        #region IConnectableControl

        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection = null;
        /// <MetaDataID>{02f334f8-4e23-438f-a93f-9fd3f982b5d0}</MetaDataID>
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
                if (_UserInterfaceObjectConnection == null)
                    return;
                if (_UserInterfaceObjectConnection != null)
                    _UserInterfaceObjectConnection.AddControlledComponent(this);

            }
        }

        /// <MetaDataID>{504C92ED-18B0-49FC-904B-A2D3C1CDBA87}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if ((metaObject is OOAdvantech.UserInterface.OperationCall) &&
               propertyDescriptor == "BeforeShowContextMenuOperationCall")
            {

                (metaObject as OOAdvantech.UserInterface.OperationCall).TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            }


            if ((metaObject is OOAdvantech.UserInterface.OperationCall) &&
                (propertyDescriptor == "AllowDropOperationCall" ||
                propertyDescriptor == "DragDropOperationCall" ||
                propertyDescriptor == "InsertRowOperationCall" ||
                propertyDescriptor == "EditRowOperationCall" ||
                propertyDescriptor == "DeleteRowOperationCall" ||
                propertyDescriptor == "BeforeShowContextMenuOperationCall"))
            {

                OOAdvantech.MetaDataRepository.Operation operation = new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation;
                if (operation != null)
                    return true;
                else
                    return false;
            }

            if (propertyDescriptor == "ViewEditForm"
                && metaObject is OOAdvantech.MetaDataRepository.Classifier)
            {
                OOAdvantech.MetaDataRepository.Operation operation = (metaObject as OOAdvantech.MetaDataRepository.Classifier).GetOperation("ShowDialog", new string[0], true);
                if (operation != null && operation.ReturnType != null && operation.ReturnType.FullName == typeof(System.Windows.Forms.DialogResult).FullName)
                    return true;
                return false;

            }
            if (propertyDescriptor == "Path")
            {
                if (metaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                {
                    OOAdvantech.MetaDataRepository.Classifier collectionClassifier = (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).CollectionClassifier;
                    if (collectionClassifier != null && collectionClassifier.TemplateBinding != null)
                    {

                        foreach (OOAdvantech.MetaDataRepository.Operation operation in collectionClassifier.GetOperations("GetEnumerator"))
                        {
                            OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                            if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                return true;
                        }
                    }
                }
                if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
                {
                    OOAdvantech.MetaDataRepository.Classifier collectionClassifier = (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type;
                    if (collectionClassifier != null && collectionClassifier.TemplateBinding != null)
                    {

                        foreach (OOAdvantech.MetaDataRepository.Operation operation in collectionClassifier.GetOperations("GetEnumerator"))
                        {
                            OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                            if (enumerator.Name.IndexOf("IEnumerator`1") == 0)
                                return true;
                        }
                    }
                }

                System.Reflection.PropertyInfo propertyInfo = metaObject.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as PropertyInfo;
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.IsGenericType)
                        if (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(OOAdvantech.Collections.Generic.Set<>))
                            return true;
                }
            }
            else if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.Operation)
            {
                return true;
            }
            else if (metaObject is OOAdvantech.MetaDataRepository.Class && propertyDescriptor == "AssignPresentationObjectType")
            {
                if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(metaObject as OOAdvantech.MetaDataRepository.Class, CollectionObjectType))
                {
                    System.Windows.Forms.MessageBox.Show("You can't assign the '" + metaObject.FullName + "' to the AssignPresentationObjectType property.\n Check the property rules.");
                    return false;
                }
                else
                    return true;
            }
            else if (metaObject is OOAdvantech.MetaDataRepository.Feature || propertyDescriptor == "SelectionMember")
                return true;



            return false;



        }


        /// <MetaDataID>{d94959f0-d167-4f92-b42f-d545d96f21e6}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }


        /// <MetaDataID>{2c2b3724-69b1-4a3a-a51a-14affed22576}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object UIMetaDataObject
        {
            get
            {
                return ListViewMetaData;

            }
            set
            {

            }
        }

        #endregion



        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Classifier _CollectionObjectType;
        /// <MetaDataID>{b062ef9a-e8fc-4ad0-82be-cc5064e66e69}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier CollectionObjectType
        {
            get
            {
                if (_CollectionObjectType == null)
                {
                    if (_CollectionObjectType == null && !string.IsNullOrEmpty(_Path) && _Path.IndexOf("Control: ") == 0)
                    {

                        IObjectMemberViewControl control = UserInterfaceObjectConnection.GetControlWithName(_Path.Replace("Control: ", "")) as IObjectMemberViewControl;
                        if (control != null)
                        {
                            _CollectionObjectType = control.GetPropertyType("Value");
                            if (_CollectionObjectType == null)
                                return null;



                            foreach (OOAdvantech.MetaDataRepository.Operation operation in control.GetPropertyType("Value").GetOperations("GetEnumerator"))
                            {
                                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                                if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
                                {
                                    _CollectionObjectType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                                    break;
                                }
                            }


                        }
                    }
                    else if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path))
                    {
                        OOAdvantech.MetaDataRepository.Classifier type = _UserInterfaceObjectConnection.GetClassifier(_Path as string);
                        if(ViewControlObject!=null&& ViewControlObject.ViewControlObjectType==null)
                        {
                            string message = string.Format("The property ViewControlObjectType of view control object ({0}) has no value.\nPlease declare view control object type.", ViewControlObject.Name);
                            System.Windows.Forms.MessageBox.Show(message,"Connectable Controls",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);

                        }

                        if (type != null && type.IsBindedClassifier)
                            _CollectionObjectType = type.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                    }
                    else if (LoadListOperationCaller != null && LoadListOperationCaller.Operation != null && LoadListOperationCaller.Operation.ReturnType != null)
                    {
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in LoadListOperationCaller.Operation.ReturnType.GetOperations("GetEnumerator"))
                        {
                            OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                            if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
                            {
                                _CollectionObjectType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                                break;
                            }
                        }
                    }
                }
                return _CollectionObjectType;

            }
        }



        /// <MetaDataID>{94858EA4-8523-4EFB-98C7-19D373AFC220}</MetaDataID>
        public void RemoveColumn(IColumn column)
        {
            HostingListView.RemoveColumn(column as IColumn);

            if (ListViewMetaData != null && column.ColumnMetaData != null)
                ListViewMetaData.RemoveColumn(column.ColumnMetaData);
        }

        /// <MetaDataID>{F52B1337-C1B0-46D6-BD24-CB5906C01DF0}</MetaDataID>
        public void AddColumn(IColumn columnView)
        {
            if (columnView.Owner == null)
            {
                HostingListView.AddColumn(columnView);
                ListViewMetaData.AddColumn(columnView.ColumnMetaData);
            }
        }
        /// <MetaDataID>{7FC75432-CB04-4D35-BBD5-0C1B705D678A}</MetaDataID>
        public IColumn AddColumn(string path)
        {
            OOAdvantech.UserInterface.TextColumn textColumn = new OOAdvantech.UserInterface.TextColumn();
            ListViewMetaData.AddColumn(textColumn);
            return HostingListView.AddColumn(textColumn);
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Class _PresentationObjectType;
        /// <MetaDataID>{e33aeb5e-e8e8-49fc-a84c-f0563a3bb612}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                        _PresentationObjectType = this.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                        return _PresentationObjectType;
                    }
                    return ValueType;
                }

            }
        }

        /// <MetaDataID>{1c98894f-f113-4a6f-a011-e971f3fb5f82}</MetaDataID>
        string PresentationObjectTypeFullName;
        /// <MetaDataID>{141aaac9-3f52-45aa-981f-bf8550875edf}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
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

        /// <MetaDataID>{17bc0d5a-b509-4908-a741-192f42daf8ed}</MetaDataID>
        XDocument MetaDataAsXmlDocument;
        /// <MetaDataID>{4e53d320-f833-401d-b09c-aaaf8097b67d}</MetaDataID>
        public OOAdvantech.UserInterface.ListView ListViewMetaData;

        /// <exclude>Excluded</exclude>
        private object _MetaData;

        /// <MetaDataID>{5ED59BC4-4269-41B8-B1EE-C5262355FFED}</MetaDataID>
        [Editor(typeof(EditListMetaData), typeof(System.Drawing.Design.UITypeEditor)),
        Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public Object MetaData
        {
            get
            {
                if (MetaDataAsXmlDocument == null)
                {
                    MetaDataAsXmlDocument = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage listViewStorage = null;
                    listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;

                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = new UserInterfaceMetaData.MetaDataValue(MetaDataAsXmlDocument.ToString() as string, "(List MetaData)");
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
                OOAdvantech.PersistenceLayer.ObjectStorage listViewStorage = null;



                if (MetaDataAsXmlDocument == null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    MetaDataAsXmlDocument = new XDocument();
                    try
                    {

                        if (!string.IsNullOrEmpty(metaData))
                            MetaDataAsXmlDocument = XDocument.Parse(metaData);

                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            MetaDataAsXmlDocument = new XDocument();
                            listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                            ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;
                        }


                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        MetaDataAsXmlDocument = new XDocument();
                        listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;
                    }
                    try
                    {

                        OOAdvantech.Collections.StructureSet set = listViewStorage.Execute("SELECT listView FROM OOAdvantech.UserInterface.ListView listView ");
                        foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                        {
                            ListViewMetaData = setInstance["listView"] as OOAdvantech.UserInterface.ListView;
                            break;
                        }
                        if (ListViewMetaData == null)
                            ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }


                    if (HostingListView is ConnectableControls.List.ListView)
                        AssignHostListViewColumns();




                }
                return;

            }
        }
        /// <MetaDataID>{8e87599f-5c84-4ae9-a71b-4dda7fd70d0b}</MetaDataID>
        public void AssignHostListViewColumns()
        {
            System.Collections.Generic.Dictionary<string, ConnectableControls.ListView.IColumn> columns = new Dictionary<string, IColumn>();

            foreach (ConnectableControls.ListView.IColumn column in HostingListView.Columns)
                columns.Add(column.Name, column);

            {
                foreach (OOAdvantech.UserInterface.Column column in ListViewMetaData.Columns)
                {
                    ConnectableControls.ListView.IColumn listViewColumn = null;
                    if (columns.TryGetValue(column.Name, out listViewColumn))
                        listViewColumn.ColumnMetaData = column;
                    else
                        HostingListView.AddColumn(column);
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _SelectionMember;
        /// <MetaDataID>{5641dbaf-9894-4423-98e7-5981099a034e}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
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


        /// <MetaDataID>{412b8fc2-e406-4553-a0b2-62c88e61c7d0}</MetaDataID>
        [Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
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

        /// <MetaDataID>{753b7a7f-0e39-423b-a06b-3801f9c0fec4}</MetaDataID>
        bool ExistConnectionData
        {
            get
            {
                if ((Path as string) == null || (Path as string).Length == 0)
                    return false;
                else
                    return true;

            }
        }

        /// <MetaDataID>{c40b5491-7908-40f4-b23b-4c94a35ddcb9}</MetaDataID>
        [Category("Object Model Connection")]
        [Browsable(false)]
        public bool IsConnectionDataCorrect
        {
            get
            {
                return true;
                if (UserInterfaceObjectConnection == null)
                {
                    if (ExistConnectionData)
                        (HostingListView as Control).Enabled = false;
                    return false;
                }
                // _CollectionObjectType = ViewControlObject.GetClassifier(Path as string);



                if (CollectionObjectType == null)
                {

                    if (ExistConnectionData)
                        (HostingListView as Control).Enabled = false;
                    return false;
                }
                else
                {

                }
                if (UserInterfaceObjectConnection.Instance == null)
                {
                    if (ExistConnectionData)
                        (HostingListView as Control).Enabled = false;

                    return false;
                }
                return true;
            }
        }





        //OOAdvantech.UserInterface.Runtime.OperationCaller _InsertRowOperationCaller;
        //OOAdvantech.UserInterface.Runtime.OperationCaller InsertRowOperationCaller
        //{
        //    get
        //    {
        //        if (ListViewMetaData == null || ListViewMetaData.InsertOperation == null || UserInterfaceObjectConnection == null)
        //            return null;
        //        if (_InsertRowOperationCaller != null)
        //            return _InsertRowOperationCaller;
        //        _InsertRowOperationCaller = new OperationCaller(ListViewMetaData.InsertOperation, this);
        //        return _InsertRowOperationCaller;
        //    }
        //}

        /// <MetaDataID>{7a5c4b60-9ee0-4dcf-bed0-6e2a1d801eeb}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public UserInterfaceMetaData.MetaDataValue InsertRowOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.InsertOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.InsertOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (UserInterfaceObjectConnection==null||(!UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.InsertOperation].CanCall))
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Insert row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.InsertOperation].Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.InsertOperation;
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
                    UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.InsertOperation].Rebuild();

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _LoadListOperationCaller;
        /// <MetaDataID>{3d60231e-e13a-4c01-bd6a-205310dfb3eb}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller LoadListOperationCaller
        {
            get
            {
                if (ListViewMetaData == null || ListViewMetaData.LoadListOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_LoadListOperationCaller != null)
                    return _LoadListOperationCaller;
                _LoadListOperationCaller = new OperationCaller(ListViewMetaData.LoadListOperation, this);
                return _LoadListOperationCaller;
            }
        }


        /// <MetaDataID>{db6e426f-ae65-42e3-8d3d-d485ffaec4f5}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object LoadListOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.LoadListOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.LoadListOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (LoadListOperationCaller == null || LoadListOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(LoadList row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, LoadListOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.LoadListOperation;
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
                    _LoadListOperationCaller = null;
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _BeforeShowContextMenuOperationCaller;
        /// <MetaDataID>{ed21191d-1a44-4376-bc9f-4cc7d23b4de1}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller BeforeShowContextMenuOperationCaller
        {
            get
            {
                if (ListViewMetaData == null || ListViewMetaData.BeforeShowContextMenuOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_BeforeShowContextMenuOperationCaller != null)
                    return _BeforeShowContextMenuOperationCaller;
                _BeforeShowContextMenuOperationCaller = new OperationCaller(ListViewMetaData.BeforeShowContextMenuOperation, this);
                return _BeforeShowContextMenuOperationCaller;
            }
        }



        /// <MetaDataID>{fca0c324-bb17-4624-8699-4541c560c150}</MetaDataID>
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
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.BeforeShowContextMenuOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.BeforeShowContextMenuOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (BeforeShowContextMenuOperationCaller == null || BeforeShowContextMenuOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(LoadList row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, BeforeShowContextMenuOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.BeforeShowContextMenuOperation;
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
                    BeforeShowContextMenuOperationCaller.OperationCall.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
                return;
            }
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _EditRowOperationCaller;
        /// <MetaDataID>{3413bc56-5d84-49f0-84f1-52572671f492}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller EditRowOperationCaller
        {
            get
            {
                if (ListViewMetaData == null || ListViewMetaData.EditOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_EditRowOperationCaller != null)
                    return _EditRowOperationCaller;
                _EditRowOperationCaller = new OperationCaller(ListViewMetaData.EditOperation, this);
                return _EditRowOperationCaller;
            }
        }
        /// <MetaDataID>{ab757ba4-04f7-40a5-8b04-faa93f587fab}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object EditRowOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.EditOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.EditOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (EditRowOperationCaller == null || EditRowOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Edit row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, EditRowOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.EditOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return new UserInterfaceMetaData.MetaDataValue(error);
                }
            }
            set
            {
                _EditRowOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DeleteRowOperationCaller;
        /// <MetaDataID>{92f082b2-a8e9-46cf-bdcf-77f9fb65c1f5}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller DeleteRowOperationCaller
        {
            get
            {
                if (ListViewMetaData == null || ListViewMetaData.DeleteOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_DeleteRowOperationCaller != null)
                    return _DeleteRowOperationCaller;
                _DeleteRowOperationCaller = new OperationCaller(ListViewMetaData.DeleteOperation, this);
                return _DeleteRowOperationCaller;
            }
        }

        //object _DeleteRowOperationCall;
        /// <MetaDataID>{ca3253a6-3f86-46c0-b6be-be7f5bad81a4}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public UserInterfaceMetaData.MetaDataValue DeleteRowOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.DeleteOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.DeleteOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (DeleteRowOperationCaller == null || DeleteRowOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Delete row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, DeleteRowOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.DeleteOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return new UserInterfaceMetaData.MetaDataValue(error);
                }
            }
            set
            {
                _DeleteRowOperationCaller = null;
                if (Site != null && Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {

                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                return;
            }
        }



        /// <MetaDataID>{45B9DAEF-3059-44FE-8CA8-2EE2FF5007FA}</MetaDataID>
        /// <returns>
        /// The new row. If the ListConnection object can't add new row returns null.  
        /// </returns>
        public IRow InsertRow(int index)
        {

            if (ListViewMetaData == null || !UserInterfaceObjectConnection.CanCall(this, ListViewMetaData.InsertOperation))
                return null;
            object newobject = UserInterfaceObjectConnection.OperationCallers[this][ListViewMetaData.InsertOperation].Invoke();

            if (newobject == null)
                return null;

            if (newobject is MarshalByRefObject)
            {
                OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
                paths.Add("Root");
                foreach (IColumn column in HostingListView.Columns)
                {
                    foreach (string path in column.Paths)
                    {
                        if (ValueType != PresentationObjectType)
                        {
                            if (path.IndexOf("RealObject.") == 0)
                                paths.Add("Root." + path.Substring("RealObject.".Length));
                        }
                        else
                            paths.Add("Root." + path);
                    }
                }
                UserInterfaceObjectConnection.BatchLoadPathsValues(newobject, ValueType.GetExtensionMetaObject(typeof(Type)) as Type, paths);
            }

            UserInterfaceObjectConnection.AddCollectionObject(newobject, _Path, this, index);
            if (!HostingListView.DataSourceSupported)
            {
                foreach (IRow row in HostingListView.Rows)
                {
                    if (row.CollectionObject == newobject)
                        return row;
                }
            }
            return null;

        }

        /// <MetaDataID>{017a6ac5-acda-4c74-bdb8-f16bc3d53d67}</MetaDataID>
        public void DeleteRow(int rowIndex)
        {

            if (DeleteRowOperationCaller == null)
                return;
            if (DeleteRowOperationCaller.Operation != null)
            {
                object _object = null;
                if (HostingListView.DataSourceSupported)
                    _object = RecordProxy.GetObject((HostingListView.DataSource as System.Collections.IList)[rowIndex]);
                else
                    _object = RecordProxy.GetObject(HostingListView.Rows[rowIndex].CollectionObject);


                object reVal = DeleteRowOperationCaller.Invoke();
                if (reVal == null || !(reVal is bool))
                {
                    //tableModel.Rows.Remove(row);
                    UserInterfaceObjectConnection.RemoveCollectionObject(_object, _Path, this);
                }
                else if ((bool)reVal)
                {
                    // tableModel.Rows.Remove(row);
                    UserInterfaceObjectConnection.RemoveCollectionObject(_object, _Path, this);
                }

            }
        }
        /// <MetaDataID>{5EEAEF80-63B0-41DD-85DC-16EA0A89F7CD}</MetaDataID>
        public void DeleteRow(IRow row)
        {
            if (DeleteRowOperationCaller == null)
                return;
            if (DeleteRowOperationCaller.Operation != null)
            {
                object reVal = DeleteRowOperationCaller.Invoke();
                if (reVal == null || !(reVal is bool))
                {
                    //tableModel.Rows.Remove(row);
                    UserInterfaceObjectConnection.RemoveCollectionObject(row.CollectionObject, _Path, this);
                }
                else if ((bool)reVal)
                {
                    // tableModel.Rows.Remove(row);
                    UserInterfaceObjectConnection.RemoveCollectionObject(row.CollectionObject, _Path, this);
                }
            }
        }




        #region DragDrop Behavior

        /// <MetaDataID>{90289ffa-6733-489d-bf61-aeb1097f2bc5}</MetaDataID>
        public void CutObject(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{396e1be6-31d0-4fed-abb8-9e51a0eee1cf}</MetaDataID>
        public void PasteObject(object dropObject)
        {
            DragDropObject = dropObject;
            object objectCollection = DragDropOperationCaller.Invoke();
            IEnumerator enumerator = objectCollection.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(objectCollection, new object[0]) as IEnumerator;
            while (enumerator.MoveNext())
            {
                object displayedObj = null;
                object obj = enumerator.Current;
                this.UserInterfaceObjectConnection.Control(obj);
                object displayedPresentationObj = null;
                if (ValueType != PresentationObjectType && PresentationObjectType != null)
                    displayedPresentationObj = UserInterfaceObjectConnection.GetPresentationObject(displayedObj, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type);

                UserInterfaceObjectConnection.AddCollectionObject(obj, _Path, this, DragDropMarkPos);
            }

        }

        /// <MetaDataID>{ec40264a-7b5f-4223-bc1b-ea6dfad30cbf}</MetaDataID>
        public void DragRows(List<IRow> selectedRows)
        {
            if (selectedRows.Count > 1)
            {
                object[] selectedObjects = new object[HostingListView.SelectedRows.Count];
                int i = 0;
                foreach (IRow row in HostingListView.SelectedRows)
                    selectedObjects[i++] = row.CollectionObject;
                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, selectedObjects, DragDropTransactionOption);
                (HostingListView as Control).DoDragDrop(dragDropActionManager, DragDropEffects.All);
            }
            else
            {

                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, selectedRows[0].CollectionObject, DragDropTransactionOption);
                (HostingListView as Control).DoDragDrop(dragDropActionManager, DragDropEffects.All);
            }

        }


        /// <exclude>Excluded</exclude>
        DragDropTransactionOptions _DragDropTransactionOption;
        /// <MetaDataID>{93aa6c96-24ba-4416-9a7a-94bf8cb3ee8c}</MetaDataID>
        [Category("DragDrop Behavior")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
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







        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _AllowDropOperationCaller;
        /// <MetaDataID>{71ba91fd-7eeb-4eff-a5d5-e34ba5e30504}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller AllowDropOperationCaller
        {
            get
            {
                if (ListViewMetaData == null || ListViewMetaData.AllowDropOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_AllowDropOperationCaller != null)
                    return _AllowDropOperationCaller;
                _AllowDropOperationCaller = new OperationCaller(ListViewMetaData.AllowDropOperation, this);
                return _AllowDropOperationCaller;
            }
        }

        /// <MetaDataID>{7b1005d6-c6f9-4edd-9b77-387c5a7370bd}</MetaDataID>
        [Category("DragDrop Behavior")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object AllowDropOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.AllowDropOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.AllowDropOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (AllowDropOperationCaller == null || AllowDropOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(AllowDrop row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, AllowDropOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.AllowDropOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return new UserInterfaceMetaData.MetaDataValue(error);
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

        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DragDropOperationCaller;
        /// <MetaDataID>{bd01ac38-d502-475e-89e9-68d25cf3b937}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller DragDropOperationCaller
        {
            get
            {
                if (ListViewMetaData == null || ListViewMetaData.DragDropOperation == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_DragDropOperationCaller != null)
                    return _DragDropOperationCaller;
                _DragDropOperationCaller = new OperationCaller(ListViewMetaData.DragDropOperation, this);
                return _DragDropOperationCaller;
            }
        }

        /// <MetaDataID>{36451b06-d121-4034-8d4f-774d0b23dfa4}</MetaDataID>
        [Category("DragDrop Behavior")]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object DragDropOperationCall
        {
            get
            {
                try
                {
                    string xml = null;
                    if (ListViewMetaData == null)
                        return new UserInterfaceMetaData.MetaDataValue(xml, ""); ;

                    if (ListViewMetaData.DragDropOperation == null)
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ListViewMetaData);
                        ListViewMetaData.DragDropOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }


                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;

                    if (DragDropOperationCaller == null || DragDropOperationCaller.Operation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, "(Edit row operation)");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, DragDropOperationCaller.Operation.Name);
                    metaDataVaue.MetaDataAsObject = ListViewMetaData.DragDropOperation;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    return new UserInterfaceMetaData.MetaDataValue(error);
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

        /// <MetaDataID>{174e8e2a-563b-4972-9ea5-dcd1c3db43c4}</MetaDataID>
        DragDropEffects DragDropEffect = DragDropEffects.None;

        /// <MetaDataID>{ad16fa4e-bc83-45eb-be8e-40ad98fa93e4}</MetaDataID>
        public void DragEnter(DragEventArgs drgevent)
        {

            if (AllowDropOperationCaller != null && AllowDropOperationCaller.Operation != null)
            {

                DragDropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
                if (DragDropObject is DragDropActionManager)
                    DragDropObject = (DragDropObject as DragDropActionManager).DragedObject;

                object ret;
                try
                {
                    ret = AllowDropOperationCaller.Invoke();
                    if (ret is OOAdvantech.DragDropMethod)
                    {
                        drgevent.Effect = (DragDropEffects)ret;
                        DragDropEffect = (DragDropEffects)ret;
                    }
                    else
                        DragDropEffect = DragDropEffects.None;
                }
                catch
                {
                    DragDropEffect = DragDropEffects.None;
                }
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
                DragDropEffect = DragDropEffects.None;
            }


        }

        /// <MetaDataID>{eba84430-6e69-4061-9641-77fc9c145d65}</MetaDataID>
        public void OnDragDrop(DragEventArgs drgevent)
        {
            if (DragDropEffect != DragDropEffects.None)
            {
                if (DragDropOperationCaller != null && DragDropOperationCaller.Operation != null)
                {

                    object dropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
                    if (dropObject is DragDropActionManager)
                        (dropObject as DragDropActionManager).DropObject((OOAdvantech.DragDropMethod)drgevent.Effect, this);
                    else
                        PasteObject(dropObject);

                }
            }
            DragDropEffect = DragDropEffects.None;
            DragDropObject = null;

        }

        /// <MetaDataID>{c8d199db-7c8a-4dfb-9d40-88366981e002}</MetaDataID>
        int DragDropMarkPos = -1;

        #endregion

        #region ICollectionViewRunTime Members

        /// <MetaDataID>{28c58a07-9628-42dc-8cd8-c99c3e6e7aa9}</MetaDataID>
        public void SetSelected(object[] items)
        {

        }

        /// <MetaDataID>{13956677-f1b3-45b8-ae2f-537de24bbf68}</MetaDataID>
        public void AddItem(object item)
        {

            if (item is MarshalByRefObject)
            {
                OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
                paths.Add("Root");
                foreach (IColumn column in HostingListView.Columns)
                {
                    foreach (string path in column.Paths)
                    {
                        if (ValueType != PresentationObjectType)
                        {
                            if (path.IndexOf("RealObject.") == 0)
                                paths.Add("Root." + path.Substring("RealObject.".Length));
                        }
                        else
                            paths.Add("Root." + path);
                    }
                }
                UserInterfaceObjectConnection.BatchLoadPathsValues(item, ValueType.GetExtensionMetaObject(typeof(Type)) as Type, paths);
            }
            int index = -1;
            UserInterfaceObjectConnection.AddCollectionObject(item, _Path, this, index);

        }

        /// <MetaDataID>{64f45025-a0c1-4b55-b0d2-51a30b248385}</MetaDataID>
        public void RemoveItem(object item)
        {
            try
            {

                //object objectCollection = UserInterfaceObjectConnection.GetDisplayedValue(_Path as string, this).Value;

                //if (objectCollection != null)
                //{

                //    System.Reflection.MethodInfo RemoveMethod = objectCollection.GetType().GetMethod("Remove");

                //    RemoveMethod.Invoke(objectCollection, new object[1] { item });

                //}

                UserInterfaceObjectConnection.RemoveCollectionObject(item, _Path, this);

                //foreach (Row row in this.TableModel.Rows)
                //{

                //    if (row.CollectionObject.Equals(item))
                //    {

                //        TableModel.Rows.Remove(row);

                //        break;

                //    }

                //}

            }

            catch (NullReferenceException ex)
            {

                throw new NullReferenceException("item can not be removed", ex);

            }

        }

        /// <MetaDataID>{07233d26-0bc8-4d63-aae4-04299f12950d}</MetaDataID>
        public object this[int index]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }



        #endregion

        #region ICollection Members

        /// <MetaDataID>{81bd943a-5304-4d96-b340-9402173f55fe}</MetaDataID>
        public void CopyTo(Array array, int index)
        {

        }

        /// <MetaDataID>{9ac8b30e-e2e4-4faf-baac-3834e8c7e7f7}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get { return 0; }
        }

        /// <MetaDataID>{014d00b2-3c7b-4725-a7ca-0d6152bb72ed}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <MetaDataID>{85325051-8efb-4642-aa23-af36f6af8146}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SyncRoot
        {
            get { return null; }
        }

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{5e055aa6-9e90-44d0-836d-4ba697cc9800}</MetaDataID>
        public IEnumerator GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
        #endregion



        /// <MetaDataID>{43c23652-4a45-4d19-a627-6f0fd8e50951}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name
        {
            get
            {
                return HostingListView.Name;
            }
            set
            {
            }
        }

        #region ICollectionViewRunTime Members


        /// <MetaDataID>{aeb89de6-2ab1-40bd-923c-7e9d81afa321}</MetaDataID>
        public void SetRowColor(int index, System.Drawing.Color color)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IObjectMemberViewControl Members
        /// <MetaDataID>{f233219a-6bdf-46d1-ac88-5450b4787774}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowDrop
        {
            get
            {
                if (HostingListView is Control)
                    return (HostingListView as Control).AllowDrop;
                else
                    return false;
            }
        }

        #endregion



        /// <MetaDataID>{946143c8-98b7-4a93-808c-46f76e9da198}</MetaDataID>
        public void SelectionChanged()
        {
            if (!string.IsNullOrEmpty(_SelectionMember))
            {
                try
                {
                    object value = GetPropertyValue("SelectedRowValue");
                    if (_PresentationObjectType != null && value is IPresentationObject)
                        value = (value as IPresentationObject).GetRealObject();
                    UserInterfaceObjectConnection.SetValue(value, _SelectionMember);
                }
                catch (System.Exception error)
                {
                }
            }


        }
    }

}
