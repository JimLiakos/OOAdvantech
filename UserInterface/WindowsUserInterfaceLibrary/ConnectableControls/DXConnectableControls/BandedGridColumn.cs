using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevExpress.XtraEditors.Repository;
using ConnectableControls.PropertyEditors;
using System.Drawing.Design;

namespace DXConnectableControls.XtraGrid.Views.BandedGrid
{
    /// <MetaDataID>{f64e188e-6a8c-4018-9feb-942a15eeca3e}</MetaDataID>
    public class BandedGridColumn: DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn, ConnectableControls.ListView.IColumn//, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl
    {
        public BandedGridColumn()
        {
        }
        public BandedGridColumn(ConnectableControls.ListView.IListView owner, OOAdvantech.UserInterface.Column columnMetaData)
        {
            _Owner = owner;
            _ColumnMetaData = columnMetaData;
            base.Caption = _ColumnMetaData.Text;
            base.Visible = true;
            base.VisibleIndex = _ColumnMetaData.Position;
            base.Width = _ColumnMetaData.Width;
            base.Name = columnMetaData.Name;
            FieldName = base.Name;
            if(_ColumnMetaData.Editor!=null)
                ColumnEditName = _ColumnMetaData.Editor.Name;
        }
       
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
        void OnDesignerHostLoadComplete(object sender, EventArgs e)
        {
            
            if (_ColumnMetaData!=null&&_ColumnMetaData.Editor!=null)
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
            }
        }

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



        int _Order = 0;
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

        ConnectableControls.ListView.IListView _Owner;
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


        OOAdvantech.UserInterface.Column _ColumnMetaData;

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
                if(_ColumnMetaData!=null)
                    FieldName = _ColumnMetaData.Name;
            }
        }

        void ConnectableControls.ListView.IColumn.SetValue(ConnectableControls.ListView.IRow row, object value)
        {
            throw new NotImplementedException();
        }
        bool ConnectableControls.ListView.IColumn.ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            return false;
            
        }


        OOAdvantech.Collections.Generic.List<string> _AllPaths;
        OOAdvantech.Collections.Generic.List<string> ConnectableControls.ListView.IColumn.Paths
        {
            get
            {
                if (_AllPaths == null)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(ColumnMetaData.Path);
                    if (!string.IsNullOrEmpty(_ColumnMetaData.DisplayMember))
                        _AllPaths.Add(ColumnMetaData.Path + '.' + _ColumnMetaData.DisplayMember);
                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(ColumnMetaData.Path);
                    if (!string.IsNullOrEmpty(_ColumnMetaData.DisplayMember))
                        _AllPaths.Add(ColumnMetaData.Path + '.' + _ColumnMetaData.DisplayMember);
                    return _AllPaths;
                }
                else
                    return _AllPaths;

            }
        }
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

   
    }
}
