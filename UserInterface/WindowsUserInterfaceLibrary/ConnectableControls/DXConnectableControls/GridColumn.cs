using System;
using System.Collections.Generic;
using System.Text;
using ConnectableControls.PropertyEditors;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using DevExpress.XtraEditors.Repository;
using System.Drawing.Design;

namespace DXConnectableControls.XtraGrid.Columns
{
    //[Browsable(false)]
    //[DesignerSerializer(typeof(MySerializer), typeof(CodeDomSerializer))]
    /// <MetaDataID>{fe78161d-525c-41fe-9509-d25acd176392}</MetaDataID>
    public class GridColumn : DevExpress.XtraGrid.Columns.GridColumn, ConnectableControls.ListView.IColumn, OOAdvantech.UserInterface.Runtime.IConnectableControl//, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl
    {
        /// <MetaDataID>{d5802964-0036-4129-9a71-715fed356051}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{c3fe980b-e7f6-48ee-8a84-e4dc8f49241a}</MetaDataID>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                if (_ColumnMetaData!=null&& _ColumnMetaData.Name != Name)
                {
                    _ColumnMetaData.Name = Name;
                    FieldName = _ColumnMetaData.Name;
                }

            }
        }


        /// <MetaDataID>{01615b71-aeef-4bdd-b0e5-c3169ed4259b}</MetaDataID>
        public GridColumn()
        {
 
        }

        /// <MetaDataID>{e6d9e272-19b6-485c-85c5-d4a0efebab30}</MetaDataID>
        public GridColumn(ConnectableControls.ListView.IListView owner, OOAdvantech.UserInterface.Column columnMetaData)
        {
            _Owner = owner;
            _ColumnMetaData = columnMetaData;
            base.Caption = _ColumnMetaData.Text;
            base.Visible = true;
            base.VisibleIndex = _ColumnMetaData.Position;
            base.Width = _ColumnMetaData.Width;
            TypeDescriptor.GetProperties(this).Find("Name", false).SetValue(this, owner.Name + "_" + columnMetaData.Name);
            base.Name =owner.Name+"_"+columnMetaData.Name;
            FieldName = columnMetaData.Name;
            if(_ColumnMetaData.Editor!=null)
                ColumnEditName = _ColumnMetaData.Editor.Name;

            
            


        }

        /// <MetaDataID>{1a4acd6a-e716-4d5c-85a5-02525d3941bf}</MetaDataID>
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                if (base.Site != null && base.Site.Container is System.ComponentModel.Design.IDesignerHost)
                {
                    System.ComponentModel.Design.IDesignerHost designerHost = base.Site.Container as System.ComponentModel.Design.IDesignerHost;
                    (designerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged -= new System.ComponentModel.Design.ComponentChangedEventHandler(OnComponentChanged);
                    designerHost.LoadComplete -= new EventHandler(OnDesignerHostLoadComplete);
                }

                base.Site = value;
                if (base.Site != null)
                {
                    if (_ColumnMetaData != null)
                    {
                        if (string.IsNullOrEmpty(_ColumnMetaData.Name))
                            _ColumnMetaData.Name = base.Site.Name;
                        else
                            base.Site.Name = _ColumnMetaData.Name;
                    }
                }

                if (value != null && value.Container is System.ComponentModel.Design.IDesignerHost)
                {
                    System.ComponentModel.Design.IDesignerHost designerHost = value.Container as System.ComponentModel.Design.IDesignerHost;
                    (designerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged += new System.ComponentModel.Design.ComponentChangedEventHandler(OnComponentChanged);
                    designerHost.LoadComplete += new EventHandler(OnDesignerHostLoadComplete);
                }


            }
        }
        /// <MetaDataID>{707ea9dd-b8a6-4aa5-a2f9-1680ae2cacad}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string FieldName
        {
            get
            {
                return base.FieldName;
            }
            set
            {
                base.FieldName = value;
            }
        }
        /// <MetaDataID>{7a49f4c5-d917-46b0-a0aa-e55c4a7e320d}</MetaDataID>
        void OnDesignerHostLoadComplete(object sender, EventArgs e)
        {
            
            if (_ColumnMetaData!=null&& _ColumnMetaData.Editor!=null)
            {
                foreach (IComponent component in Site.Container.Components)
                {
                    if (component.Site.Name == _ColumnMetaData.Editor.Name)
                    {
                        ColumnEdit = component as DevExpress.XtraEditors.Repository.RepositoryItem;
                        if (ColumnEdit is DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit)
                            (ColumnEdit as DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit).GridColumn = this;

                        break;
                    }
                }
            }
        }

        /// <MetaDataID>{29985c53-d20c-4050-a0d5-45582432f2dc}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Object DisplayMember
        {
            get
            {
                return _ColumnMetaData.DisplayMember;
            }
            set
            {

                if (_ColumnMetaData != null)
                {
                    if (value is ConnectableControls.PropertyEditors.MetaData)
                        _ColumnMetaData.DisplayMember = (value as ConnectableControls.PropertyEditors.MetaData).Path;
                    else
                        _ColumnMetaData.DisplayMember = value as string;


                }

            }
        }

        /// <MetaDataID>{daeba774-a42f-4f0c-9f84-a140bdcbcc02}</MetaDataID>
        public override string ColumnEditName
        {
            get
            {
                return base.ColumnEditName;
            }
            set
            {
                base.ColumnEditName = value;
            }
        }

        /// <MetaDataID>{5e4516b3-740d-4de8-b095-14485b89fd6d}</MetaDataID>
        [TypeConverter("DevExpress.XtraGrid.TypeConverters.ColumnEditConverter, DevExpress.XtraGrid.v8.3.Design")]
        [DefaultValue("")]
        [Editor("DevExpress.XtraGrid.Design.ColumnEditEditor, DevExpress.XtraGrid.v8.3.Design", typeof(UITypeEditor))]
        [Category("Data")]
        [Description("Gets or sets the repository item specifying the editor used to edit a column's cell values.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new RepositoryItem ColumnEdit 
        {
            get
            {
                return base.ColumnEdit;
            }
            set
            {
                if (value is DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit)
                    (value as DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit).GridColumn = this;
                base.ColumnEdit = value;

                if (base.ColumnEdit != null && _ColumnMetaData.Editor != null)
                {
                    if (base.ColumnEdit != null)
                        _ColumnMetaData.Editor.Name = base.ColumnEdit.Name;
                }

            }
        }

        /// <MetaDataID>{39c931b7-b218-4a05-aae8-aad8122983f3}</MetaDataID>
        void OnComponentChanged(object sender, System.ComponentModel.Design.ComponentChangedEventArgs e)
        {

            if (e.Component == this)
            {
                if (_ColumnMetaData.Name != Name)
                    _ColumnMetaData.Name = Name;
                if (_ColumnMetaData.Editor == null || _ColumnMetaData.Editor.Name != ColumnEditName)
                {
                    if (_ColumnMetaData.Editor == null)
                    {
                        if (ColumnEdit is DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit)
                        {
                            OOAdvantech.UserInterface.ColumnEditor columnEditor = new OOAdvantech.UserInterface.LookUpEditor();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_ColumnMetaData).CommitTransientObjectState(columnEditor);
                            _ColumnMetaData.Editor = columnEditor;

                        }
                        else
                        {
                            OOAdvantech.UserInterface.ColumnEditor columnEditor = new OOAdvantech.UserInterface.ColumnEditor();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_ColumnMetaData).CommitTransientObjectState(columnEditor);
                            _ColumnMetaData.Editor = columnEditor;
                        }
                    }
                    else
                    {
                        if (ColumnEdit is DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit &&
                            !(_ColumnMetaData.Editor is OOAdvantech.UserInterface.LookUpEditor))
                        {
                            OOAdvantech.UserInterface.ColumnEditor columnEditor = _ColumnMetaData.Editor;
                            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(columnEditor);
                            columnEditor = new OOAdvantech.UserInterface.LookUpEditor();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_ColumnMetaData).CommitTransientObjectState(columnEditor);
                            _ColumnMetaData.Editor = columnEditor;
                        }
                    }
                    _ColumnMetaData.Editor.Name = ColumnEditName;
                }
            }
        }


        /// <MetaDataID>{099edef2-018e-4bb4-8685-c907eb838011}</MetaDataID>
        public override string Caption
        {
            get
            {
                return base.Caption;
            }
            set
            {
                if (_ColumnMetaData != null)
                    _ColumnMetaData.Text = value;

                base.Caption = value;
            }
        }


        /// <MetaDataID>{0c8ac1f4-4381-4e5e-b7c3-18dc795e9156}</MetaDataID>
        public override int VisibleIndex
        {
            get
            {
                return base.VisibleIndex;
            }
            set
            {
                if (_ColumnMetaData != null)
                    _ColumnMetaData.Position = (short)value;

                base.VisibleIndex = value;
            }
        }


        /// <MetaDataID>{f0f9ccfb-d677-4093-9ff0-812c282c8182}</MetaDataID>
        public override int Width
        {
            get
            {
                if (DesignMode && _ColumnMetaData != null && _ColumnMetaData.Width != base.Width)
                {
                    _ColumnMetaData.Width = base.Width;
                }

                return base.Width;
            }
            set
            {
                if (_ColumnMetaData != null)
                    _ColumnMetaData.Width = value;

                base.Width = value;
            }
        }

        #region IColumn Members



        /// <MetaDataID>{75055022-b015-49ac-b154-312c34d20d49}</MetaDataID>
        int _Order = 0;
        /// <MetaDataID>{79d11e15-d1a4-4930-87d3-ee37be70807e}</MetaDataID>
        int ConnectableControls.ListView.IColumn.Order
        {
            get
            {
                if (_ColumnMetaData != null)
                    return _ColumnMetaData.Position;
                return _Order;

            }
            set
            {
                if (_ColumnMetaData != null)
                    _ColumnMetaData.Position = (short)value;
                _Order = value;
                base.VisibleIndex = value;

            }
        }

        /// <MetaDataID>{9589a118-b0cd-40ad-82d0-2f6dc6801279}</MetaDataID>
        ConnectableControls.ListView.IListView _Owner;
        /// <MetaDataID>{9c3b6737-5280-4545-9bc1-8987a591913c}</MetaDataID>
        ConnectableControls.ListView.IListView ConnectableControls.ListView.IColumn.Owner
        {
            get
            {
                return _Owner;

            }
            set
            {
                _Owner = value;

            }
        }


        /// <MetaDataID>{d29cde20-2ae5-4448-885b-48e0438c121f}</MetaDataID>
        OOAdvantech.UserInterface.Column _ColumnMetaData;

        /// <MetaDataID>{6cb2e4dc-b559-4838-9a4d-8f432d7dcb5b}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Column ColumnMetaData
        {
            get
            {

                return _ColumnMetaData;
            }
            set
            {
                _ColumnMetaData = value;
                if (_ColumnMetaData != null)
                {
                    FieldName = _ColumnMetaData.Name;

                    if (_ColumnMetaData.Editor != null)
                    {
                        if (ColumnEdit != null)
                            _ColumnMetaData.Editor.Name = ColumnEdit.Name;
                    }
                }
            }
        }

        /// <MetaDataID>{caba8322-eaab-4be0-ac74-5ba6cc401f5d}</MetaDataID>
        void ConnectableControls.ListView.IColumn.SetValue(ConnectableControls.ListView.IRow row, object value)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{83a14fed-507d-40b6-8921-7c93d1eb73b1}</MetaDataID>
        bool ConnectableControls.ListView.IColumn.ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            return false;
            //throw new NotImplementedException();
        }


        /// <MetaDataID>{aa34a60e-dff5-4a86-a0f5-5c873672f839}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;
        /// <MetaDataID>{9c074acb-ab11-4231-8718-2ddb4d764a68}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> ConnectableControls.ListView.IColumn.Paths
        {
            get
            {
                if (_AllPaths == null)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    if (ColumnMetaData != null)
                    {
                        _AllPaths.Add(ColumnMetaData.Path);
                        if (!string.IsNullOrEmpty(_ColumnMetaData.DisplayMember))
                            _AllPaths.Add(ColumnMetaData.Path + '.' + _ColumnMetaData.DisplayMember);
                    }
                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    if (ColumnMetaData != null)
                    {
                        _AllPaths.Add(ColumnMetaData.Path);
                        if (!string.IsNullOrEmpty(_ColumnMetaData.DisplayMember))
                            _AllPaths.Add(ColumnMetaData.Path + '.' + _ColumnMetaData.DisplayMember);
                    }
                    return _AllPaths;
                }
                else
                    return _AllPaths;

            }
        }
        /// <MetaDataID>{242c3216-fc5e-4229-8392-92631c5c0c24}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Path
        {
            get
            {
                
                if (_ColumnMetaData != null)
                    return _ColumnMetaData.Path;
                return "";
            }
            set
            {
                string path = null;
                if (value is string)
                    path = value as string;
                else if (value is MetaData)
                    path = (value as MetaData).Path;


                if (_ColumnMetaData != null)
                    _ColumnMetaData.Path = path;

            }
        }
        #endregion

        //#region IObjectMemberViewControl Members



        ////bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.AllowDrag
        ////{
        ////    get { throw new NotImplementedException(); }
        ////}

        ////bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.AllowDrop
        ////{
        ////    get { throw new NotImplementedException(); }
        ////}

        ////bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.ErrorCheck(ref System.Collections.ArrayList errors)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        ////{
        ////    return (this as OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl).ValueType;
        ////}

        ////bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.ConnectedObjectAutoUpdate
        ////{
        ////    get
        ////    {
        ////        throw new NotImplementedException();
        ////    }
        ////    set
        ////    {
        ////        throw new NotImplementedException();
        ////    }
        ////}

        ////object OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.Value
        ////{
        ////    get
        ////    {
        ////        throw new NotImplementedException();
        ////    }
        ////    set
        ////    {
        ////        throw new NotImplementedException();
        ////    }
        ////}

        ////internal OOAdvantech.MetaDataRepository.Classifier _ValueType;

        ////OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.ValueType
        ////{
        ////    get
        ////    {
        ////        if (_ValueType == null)
        ////        {
        ////            OOAdvantech.MetaDataRepository.Classifier collectionObjectType = _Owner.ListConnection.PresentationObjectType;
        ////            if (collectionObjectType != null)
        ////                _ValueType = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(_Owner.ListConnection.PresentationObjectType, _ColumnMetaData.Path);
        ////        }
        ////        return _ValueType;
        ////    }
        ////}

        ////protected string _Path;

        ////object OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.Path
        ////{
        ////    get
        ////    {
        ////        if (_ColumnMetaData != null)
        ////            return _ColumnMetaData.Path;
        ////        return _Path;
        ////    }
        ////    set
        ////    {
        ////        if (value is string)
        ////            _Path = value as string;
        ////        else if (value is MetaData)
        ////            _Path = (value as MetaData).Path;


        ////        if (_ColumnMetaData != null)
        ////            _ColumnMetaData.Path = _Path;

        ////    }
        ////}

        ////void OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.LoadControlData()
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////void OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.SaveControlData()
        ////{
        ////    throw new NotImplementedException();
        ////}

        //#endregion

        //#region IOperetionCallerSource Members

        ////string[] OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.PropertiesNames
        ////{
        ////    get { throw new NotImplementedException(); }
        ////}

        ////object OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.GetPropertyValue(string propertyName)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////void OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.SetPropertyValue(string propertyName, object value)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.GetPropertyType(string propertyName)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////bool OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.ContainsProperty(string propertyName)
        ////{
        ////    throw new NotImplementedException();
        ////}

        //#endregion

        //#region IConnectableControl Members

        ////OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IConnectableControl.UserInterfaceObjectConnection
        ////{
        ////    get
        ////    {
        ////        return _Owner.ListConnection.UserInterfaceObjectConnection;
        ////    }
        ////    set
        ////    {

        ////    }
        ////}

        ////object OOAdvantech.UserInterface.Runtime.IConnectableControl.UIMetaDataObject
        ////{
        ////    get
        ////    {
        ////        throw new NotImplementedException();
        ////    }
        ////    set
        ////    {
        ////        throw new NotImplementedException();
        ////    }
        ////}

        ////bool OOAdvantech.UserInterface.Runtime.IConnectableControl.IsPropertyReadOnly(string propertyName)
        ////{
        ////    return false;
        ////}

        //#endregion

        //#region IMetadataSelectionResolver Members

        ////bool OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver.CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        ////{
        ////    if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
        ////        return true;
        ////    else if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.Operation)
        ////        return true;
        ////    else
        ////        return false;

        ////}

        //#endregion

        //#region IPathDataDisplayer Members

        ////OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.UserInterfaceObjectConnection
        ////{
        ////    get
        ////    {
        ////        return _Owner.ListConnection.UserInterfaceObjectConnection;
        ////    }
        ////}

        ////OOAdvantech.Collections.Generic.List<string> _AllPaths;
        ////OOAdvantech.Collections.Generic.List<string> OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.Paths
        ////{
        ////    get
        ////    {
        ////        if (_AllPaths == null)
        ////        {
        ////            _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
        ////            _AllPaths.Add(ColumnMetaData.Path);
        ////            if (!string.IsNullOrEmpty(_ColumnMetaData.DisplayMember))
        ////                _AllPaths.Add(_Path + '.' + _ColumnMetaData.DisplayMember);
        ////            return _AllPaths;
        ////        }
        ////        else if (DesignMode)
        ////        {
        ////            _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
        ////            _AllPaths.Add(_Path);
        ////            if (!string.IsNullOrEmpty(_ColumnMetaData.DisplayMember))
        ////                _AllPaths.Add(_Path + '.' + _ColumnMetaData.DisplayMember);
        ////            return _AllPaths;
        ////        }
        ////        else
        ////            return _AllPaths;

        ////    }
        ////}

        ////bool OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.HasLockRequest
        ////{
        ////    get { return false; }
        ////}

        ////void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.LockStateChange(object sender)
        ////{
        ////    throw new NotImplementedException();
        ////}

        //#endregion

        #region IMetadataSelectionResolver Members

        /// <MetaDataID>{dfcd084e-f721-4c5c-ae21-d4708b43f98b}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            //else if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.Operation)
            //    return true;
            //else if (propertyDescriptor == "OperationCall" && metaObject is OOAdvantech.UserInterface.OperationCall)
            //{
            //    if (new OOAdvantech.UserInterface.Runtime.OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, o).Operation != null)
            //        return true;
            //    else
            //        return false;
            //}
            //else
                return false;
        }

        /// <MetaDataID>{97b945ed-b050-45a0-b425-89059ac0c313}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            return  ValueType;
            
        }

        #endregion
        /// <MetaDataID>{fc792f1d-0bbd-4698-8c68-462ad6371e15}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.Classifier _ValueType;
        /// <MetaDataID>{a0555fd3-5276-4d65-aa54-591bb19bd78d}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {

                if (_ValueType == null)
                {
                    OOAdvantech.MetaDataRepository.Classifier collectionObjectType = _Owner.ListConnection.PresentationObjectType;
                    if (collectionObjectType != null)
                        _ValueType = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(_Owner.ListConnection.PresentationObjectType, Path as string);
                }
                return _ValueType;
            }

        }

        #region IConnectableControl Members

        /// <MetaDataID>{537b0864-3de8-40e8-95be-3a930712566c}</MetaDataID>
        public void InitializeControl()
        {
            
        }

        /// <MetaDataID>{adb60bcd-3dff-4bc3-9786-7987ef1b9040}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                if(_Owner!=null)
                    return _Owner.ListConnection.UserInterfaceObjectConnection;
                return null;
            }
            set
            {
                
            }
        }

        /// <MetaDataID>{946fcaa5-1455-4774-9ac1-8d9192ac8cd9}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{3f4042eb-9683-49df-97a1-1396f261a349}</MetaDataID>
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{5fd4eb4b-ce4d-42f2-b8a6-2fdaa670dfc6}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion
    }


    //public class MySerializer : CodeDomSerializer
    //{
    //    public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
    //    {
    //        return base.Deserialize(manager, codeObject);
    //    }
    //    protected override object DeserializeInstance(IDesignerSerializationManager manager, Type type, object[] parameters, string name, bool addToContainer)
    //    {
    //        return base.DeserializeInstance(manager, type, parameters, name, addToContainer);
    //    }
    //    public override string GetTargetComponentName(System.CodeDom.CodeStatement statement, System.CodeDom.CodeExpression expression, Type targetType)
    //    {
    //        return base.GetTargetComponentName(statement, expression, targetType);
    //    }
    //    public override object Serialize(IDesignerSerializationManager manager, object value)
    //    {
    //        object tt = base.Serialize(manager, value);
    //        return tt;
    //    }
    //    public override object SerializeAbsolute(IDesignerSerializationManager manager, object value)
    //    {
    //        return base.SerializeAbsolute(manager, value);
    //    }
    //    public override System.CodeDom.CodeStatementCollection SerializeMember(IDesignerSerializationManager manager, object owningObject, MemberDescriptor member)
    //    {
    //        return base.SerializeMember(manager, owningObject, member);
    //    }
    //    public override System.CodeDom.CodeStatementCollection SerializeMemberAbsolute(IDesignerSerializationManager manager, object owningObject, MemberDescriptor member)
    //    {
    //        return base.SerializeMemberAbsolute(manager, owningObject, member);
    //    }



    //}



}
