using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DXConnectableControls.XtraGrid
{
    /// <MetaDataID>{93f282b1-592f-4d34-8158-91b9422329ff}</MetaDataID>
    public partial class GridControl : DevExpress.XtraGrid.GridControl, DevExpress.Data.IBoundControl, DevExpress.LookAndFeel.ISupportLookAndFeel, DevExpress.Utils.IToolTipControlClient
    {

        
        
        public GridControl()
        {
            InitializeComponent();

        }
        public override DevExpress.XtraEditors.Repository.RepositoryItemCollection RepositoryItems
        {
            get
            {
                return base.RepositoryItems;
            }
        }
        public override DevExpress.XtraGrid.Views.Base.BaseView CreateView(string name)
        {
            
            return base.CreateView(name);
        }



        System.ComponentModel.Design.IDesignerHost _DesignerHost;
        public override System.ComponentModel.ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;



                if (value != null && value.Container is System.ComponentModel.Design.IDesignerHost)
                {
                    _DesignerHost = value.Container as System.ComponentModel.Design.IDesignerHost;
                    _DesignerHost.TransactionClosed += new System.ComponentModel.Design.DesignerTransactionCloseEventHandler(OnDesignerTransactionClosed);
                    _DesignerHost.TransactionOpened += new EventHandler(OnDesignerTransactionOpened);

                    (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded += new System.ComponentModel.Design.ComponentEventHandler(OnComponentAdded);
                    (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentRemoved += new System.ComponentModel.Design.ComponentEventHandler(OnComponentRemoved);
                    //(_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged += new System.ComponentModel.Design.ComponentChangedEventHandler(ComponentChanged);




                }
                else
                {
                    if (_DesignerHost != null)
                    {
                        _DesignerHost.TransactionClosed -= new System.ComponentModel.Design.DesignerTransactionCloseEventHandler(OnDesignerTransactionClosed);
                        _DesignerHost.TransactionOpened -= new EventHandler(OnDesignerTransactionOpened);

                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded -= new System.ComponentModel.Design.ComponentEventHandler(OnComponentAdded);
                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentRemoved -= new System.ComponentModel.Design.ComponentEventHandler(OnComponentRemoved);

                        //(_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged -= new System.ComponentModel.Design.ComponentChangedEventHandler(ComponentChanged);
                        _DesignerHost = null;
                    }

                }
            }
        }

        void OnComponentRemoved(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {

            if (!_DesignerHost.Loading && 
                e.Component is DevExpress.XtraGrid.Views.Base.BaseView
                && NewView != null &&
                (e.Component as DevExpress.XtraGrid.Views.Base.BaseView).GridControl == this)
            {
                if (NewView is ConnectableControls.ListView.IListView &&
                    e.Component is ConnectableControls.ListView.IListView)
                {
                    
                    TypeDescriptor.GetProperties(NewView).Find("ListConnection", false).SetValue(NewView, (e.Component as ConnectableControls.ListView.IListView).ListConnection);
                    (e.Component as ConnectableControls.ListView.IListView).ListConnection.HostingListView = NewView as ConnectableControls.ListView.IListView;
                    (e.Component as ConnectableControls.ListView.IListView).ListConnection.Name = NewView.Name;

                    OldView = e.Component as DevExpress.XtraGrid.Views.Base.BaseView;
                }


            }
                



        }
        DevExpress.XtraGrid.Views.Base.BaseView NewView;
        DevExpress.XtraGrid.Views.Base.BaseView OldView;
        void OnComponentAdded(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {
            if (!_DesignerHost.Loading && 
                e.Component is DevExpress.XtraGrid.Views.Base.BaseView&&
                (e.Component as DevExpress.XtraGrid.Views.Base.BaseView).GridControl==this)
                NewView = e.Component as DevExpress.XtraGrid.Views.Base.BaseView;
            
            
        }

        void OnDesignerTransactionOpened(object sender, EventArgs e)
        {
            
        }

        void OnDesignerTransactionClosed(object sender, System.ComponentModel.Design.DesignerTransactionCloseEventArgs e)
        {

            if (OldView != null && NewView != null)
            {
                IConvertibleView convertibleView = NewView as IConvertibleView;
                NewView = null;
                OldView = null;
                convertibleView.ConvertionCompleted();
            }
            
        }




        protected override void FireChanged(object component)
        {
            if (component is DXConnectableControls.XtraGrid.Columns.GridColumn)
                return;
            base.FireChanged(component);
        }


        protected override void OnLoaded()
        {
            foreach (DevExpress.XtraGrid.Views.Base.BaseView view in ViewCollection)
            {
                if (view is DXConnectableControls.XtraGrid.Views.Grid.GridView || view is DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridView)
                {
                    foreach (DevExpress.XtraGrid.Columns.GridColumn column in new System.Collections.ArrayList((view as DevExpress.XtraGrid.Views.Grid.GridView).Columns))
                    {
                        if (column is ConnectableControls.ListView.IColumn)
                        {
                            if ((column as ConnectableControls.ListView.IColumn).ColumnMetaData!=null&&(column as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor != null &&
                                (column as DevExpress.XtraGrid.Columns.GridColumn).ColumnEdit == null)
                            {
                                foreach (DevExpress.XtraEditors.Repository.RepositoryItem repositoryItem in RepositoryItems)
                                {
                                    if (repositoryItem.Name == (column as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor.Name)
                                    {
                                        (column as DevExpress.XtraGrid.Columns.GridColumn).ColumnEdit = repositoryItem;
                                        if (repositoryItem is DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit)
                                            (repositoryItem as DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit).GridColumn = column; 
                                        break;

                                    }
                                }
                            }
                        }
                      
                   }
                }
            }
            base.OnLoaded();
        }
        protected override void RegisterAvailableViewsCore(DevExpress.XtraGrid.Registrator.InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);

            foreach (DevExpress.XtraGrid.Registrator.BaseInfoRegistrator infoRegister in new System.Collections.ArrayList(_AvailableViews))
            {
                if ((infoRegister.GetType() == typeof(DevExpress.XtraGrid.Registrator.GridInfoRegistrator)))
                {

                    _AvailableViews.Remove(infoRegister);
                    _AvailableViews.Add(new DevExpress.XtraGrid.Registrator.ConnCtrlGridInfoRegistrator());
                }
                if ((infoRegister.GetType() == typeof(DevExpress.XtraGrid.Registrator.BandedGridInfoRegistrator)))
                {

                    _AvailableViews.Remove(infoRegister);
                    _AvailableViews.Add(new DevExpress.XtraGrid.Registrator.ConnCtrlBandedGridInfoRegistrator());

                    break;
                }

            }

        }

        DevExpress.XtraGrid.Registrator.InfoCollection _AvailableViews;
        public override DevExpress.XtraGrid.Registrator.InfoCollection AvailableViews
        {
            get
            {
                if (_AvailableViews == null)
                    _AvailableViews = new DevExpress.XtraGrid.Registrator.InfoCollection();

                return _AvailableViews;
            }
        }


   
    }



    /// <MetaDataID>{2b356661-0bd5-4ad5-8bbb-d84f6d9ba5d9}</MetaDataID>
    internal interface IConvertibleView
    {
        void ConvertionCompleted();
    }

}

namespace DevExpress.XtraGrid.Registrator
{
    /// <MetaDataID>{1336f101-d4b6-4239-85fd-b282028f1998}</MetaDataID>
    public class ConnCtrlGridInfoRegistrator : GridInfoRegistrator
    {
        public override DevExpress.XtraGrid.Views.Base.BaseView CreateView(GridControl grid)
        {
           DXConnectableControls.XtraGrid.Views.Grid.GridView view = new DXConnectableControls.XtraGrid.Views.Grid.GridView();
            view.SetGridControl(grid);
            return view;
        }
        public override string ViewName
        {
            get
            {
                return base.ViewName;
            }
        }
        public override string StyleOwnerName
        {
            get
            {
                return base.StyleOwnerName;
            }
        }
        
    }



    /// <MetaDataID>{b1dbee6b-e0b2-4a91-8a44-148c36dd3ed3}</MetaDataID>
    public class ConnCtrlBandedGridInfoRegistrator : BandedGridInfoRegistrator
    {
        public override DevExpress.XtraGrid.Views.Base.BaseView CreateView(GridControl grid)
        {

            DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridView();

            bandedGridView.SetGridControl(grid);
            return bandedGridView;
        }
        public override string ViewName
        {
            get
            {
                return base.ViewName;
            }
        }
        public override string StyleOwnerName
        {
            get
            {
                return base.StyleOwnerName;
            }
        }

    }

   



}

