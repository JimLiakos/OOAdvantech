using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using ConnectableControls;
using DevExpress.XtraGrid.Registrator;

namespace DXConnectableControls.XtraGrid.Views.BandedGrid
{


    /// <MetaDataID>{9daa83d1-78b0-494a-89ab-f0d1f6083369}</MetaDataID>
    public partial class BandedGridView : DevExpress.XtraGrid.Views.BandedGrid.BandedGridView, ConnectableControls.ListView.IListView, IConvertibleView
    {
        
        public BandedGridView()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(OnSelectionChanged);

        }

      
        protected override void RestoreLayoutCore(DevExpress.Utils.Serializing.XtraSerializer serializer, object path, DevExpress.Utils.OptionsLayoutBase options)
        {
            base.RestoreLayoutCore(serializer, path, options);
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
                    (designerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded -= new System.ComponentModel.Design.ComponentEventHandler(OnComponentAdded);
                    designerHost.LoadComplete -= new EventHandler(OnDesignerHostLoadComplete);
                }

                base.Site = value;

                if (value != null && value.Container is System.ComponentModel.Design.IDesignerHost)
                {
                    System.ComponentModel.Design.IDesignerHost designerHost = value.Container as System.ComponentModel.Design.IDesignerHost;
                    (designerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded += new System.ComponentModel.Design.ComponentEventHandler(OnComponentAdded);
                    designerHost.LoadComplete += new EventHandler(OnDesignerHostLoadComplete);
                }



            }
        }

        void OnComponentAdded(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {
            if (!(e.Component is DXConnectableControls.XtraGrid.Columns.GridColumn) && (e.Component is DevExpress.XtraGrid.Columns.GridColumn) && (e.Component as DevExpress.XtraGrid.Columns.GridColumn).View == this)
            {
                //DXConnectableControls.XtraGrid.Columns.GridColumn gridColumn = new DXConnectableControls.XtraGrid.Columns.GridColumn();
                //gridColumn.Name = (e.Component as DevExpress.XtraGrid.Columns.GridColumn).Name;
                //gridColumn.Caption = (e.Component as DevExpress.XtraGrid.Columns.GridColumn).Caption;
                //Columns.Remove((e.Component as DevExpress.XtraGrid.Columns.GridColumn));
                //Columns.Add(gridColumn);
            }

        }

        void OnDesignerHostLoadComplete(object sender, EventArgs e)
        {
            //  ListConnection.AssignHostListViewColumns();
        }
        protected override void OnLoaded()
        {

            base.OnLoaded();
            this.GridControl.DragEnter += new DragEventHandler(OnDragEnter);
            this.GridControl.DragDrop += new DragEventHandler(OnDragDrop);

        }
        protected override void OnEndInit()
        {

            ListConnection.AssignHostListViewColumns();
            base.OnEndInit();
        }


        void OnDragDrop(object sender, DragEventArgs e)
        {
            ListConnection.OnDragDrop(e);
        }

        void OnDragEnter(object sender, DragEventArgs e)
        {
            ListConnection.DragEnter(e);
        }


        void OnSelectionChanged(object sender, FocusedRowChangedEventArgs e)
        {
            ListConnection.SelectionChanged();

        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Insert)
                InsertRow();
            if (e.KeyData == Keys.Delete)
            {
                foreach (int index in GetSelectedRows())
                    ListConnection.DeleteRow(index);
            }

        }

        void InsertRow()
        {
            int index = -1;
            int[] selectedIndicies = GetSelectedRows();
            if (selectedIndicies.Length != 0)
                index = selectedIndicies[selectedIndicies.Length - 1];
            ListConnection.InsertRow(index);
            SelectCell(index, Columns[0]);
        }


        internal new void SetGridControl(DevExpress.XtraGrid.GridControl newControl)
        {
            base.SetGridControl(newControl);
        }

        protected override void FireChangedColumns()
        {
            //base.FireChangedColumns();
        }



        ListConnection _ListConnection = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public ListConnection ListConnection
        {
            get
            {
                try
                {
                    if (_ListConnection == null)
                        _ListConnection = new ListConnection(this);
                    return _ListConnection;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            set
            {
                if (value != null)
                    _ListConnection = value;
            }
        }


        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }




        #region IListView Members

        List<ConnectableControls.ListView.IColumn> ConnectableControls.ListView.IListView.Columns
        {
            get
            {
                List<ConnectableControls.ListView.IColumn> columns = new List<ConnectableControls.ListView.IColumn>();
                foreach (DevExpress.XtraGrid.Columns.GridColumn column in Columns)
                {
                    if (column is ConnectableControls.ListView.IColumn)
                        columns.Add(column as ConnectableControls.ListView.IColumn);

                }
                return columns;
            }
        }

        string ConnectableControls.ListView.IListView.Name
        {
            get
            {
                return Name;
            }
        }

        void ConnectableControls.ListView.IListView.RemoveColumn(ConnectableControls.ListView.IColumn column)
        {
            if (column is DevExpress.XtraGrid.Columns.GridColumn)
                Columns.Remove(column as DevExpress.XtraGrid.Columns.GridColumn);

        }

        void ConnectableControls.ListView.IListView.AddColumn(ConnectableControls.ListView.IColumn column)
        {

            if (column is DevExpress.XtraGrid.Columns.GridColumn)
                Columns.Add(column as DevExpress.XtraGrid.Columns.GridColumn);
            else if (column == null)
                throw new System.ArgumentException("Parameter is null", "column");
            else
                throw new System.ArgumentException("Invalid Column '" + column.GetType().FullName + "'", "column");

        }

        ConnectableControls.ListView.IColumn ConnectableControls.ListView.IListView.ChangeColumnType(ConnectableControls.ListView.IColumn selectedColumn, string columnType)
        {
            throw new NotImplementedException();
        }

        List<ConnectableControls.ListView.IRow> ConnectableControls.ListView.IListView.SelectedRows
        {
            get { throw new NotImplementedException(); }
        }

        int[] ConnectableControls.ListView.IListView.SelectedRowsIndicies
        {
            get
            {

                return GetSelectedRows();
            }
        }




        List<ConnectableControls.ListView.IRow> ConnectableControls.ListView.IListView.Rows
        {
            get { throw new NotImplementedException(); }
        }

        ConnectableControls.ListView.IRow ConnectableControls.ListView.IListView.InsertRow()
        {
            InsertRow();
            return null;
        }

        ConnectableControls.ListView.IRow ConnectableControls.ListView.IListView.LastMouseOverRow
        {
            get { throw new NotImplementedException(); }
        }

        void ConnectableControls.ListView.IListView.RemoveAllRows()
        {

            throw new NotImplementedException();
        }


        void ConnectableControls.ListView.IListView.RemoveRowAt(int index)
        {

            DeleteRow(index);
        }


        ConnectableControls.ListView.IRow ConnectableControls.ListView.IListView.InsertRow(int index, object displayedObj, OOAdvantech.UserInterface.Runtime.IPresentationObject displayedPresentationObj)
        {
            throw new NotImplementedException();
        }
        protected override void OnColumnAdded(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            base.OnColumnAdded(column);
            if (column is ConnectableControls.ListView.IColumn)
                (column as ConnectableControls.ListView.IColumn).Owner = this;
        }
        ConnectableControls.ListView.IColumn ConnectableControls.ListView.IListView.AddColumn(OOAdvantech.UserInterface.Column column)
        {
            DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn = new BandedGridColumn(this, column);
            Columns.Add(gridColumn);


            FireChangedColumns();
            return gridColumn;



        }
        //public override DevExpress.XtraGrid.Columns.GridColumnCollection Columns
        //{
        //    get
        //    {
        //        return base.Columns;
        //    }
        //}

        List<string> ConnectableControls.ListView.IListView.GetColumnTypesNames()
        {
            return new List<string>();
        }

        string ConnectableControls.ListView.IListView.GetColumnTypeName(ConnectableControls.ListView.IColumn SelectedColumn)
        {
            return "";
        }

        ListConnection ConnectableControls.ListView.IListView.ListConnection
        {
            get
            {
                return ListConnection;
            }
        }


        bool ConnectableControls.ListView.IListView.DataSourceSupported
        {
            get
            {
                return true;
            }
        }

        object ConnectableControls.ListView.IListView.DataSource
        {
            get
            {
                return DataSource;
            }
            set
            {
                GridControl.DataSource = value;
            }
        }

     

        protected override void SetState(int value)
        {
            base.SetState(value);
        }

        void ConnectableControls.ListView.IListView.RefreshDataSource()
        {
            GridControl.RefreshDataSource();

        }

        void ConnectableControls.ListView.IListView.RefreshRow(int rowHandle)
        {
            RefreshRow(rowHandle);
        }
        void ConnectableControls.ListView.IListView.RefreshRowCell(int rowHandle, ConnectableControls.ListView.IColumn column)
        {
            RefreshRowCell(rowHandle, column as DevExpress.XtraGrid.Columns.GridColumn);
        }
        void ConnectableControls.ListView.IListView.SelectRows(List<ConnectableControls.ListView.IRow> rows)
        {
        }
        void ConnectableControls.ListView.IListView.SelectRows(List<int> rowsIndicies)
        {
        }

        void ConnectableControls.ListView.IListView.SelectRow(ConnectableControls.ListView.IRow row)
        {
        }
        void ConnectableControls.ListView.IListView.SelectRow(int rowsIndex)
        {
            if (!OptionsSelection.MultiSelect)
            {
                //OptionsSelection.MultiSelect = true;
                //ClearSelection();
                //SelectRow(rowsIndex);
                //OptionsSelection.MultiSelect = false;

                FocusedRowHandle = rowsIndex;
            }
            else
                SelectRow(rowsIndex);
        }

        #endregion

        #region IConvertableView Members

        void IConvertibleView.ConvertionCompleted()
        {
            foreach (OOAdvantech.UserInterface.Column column in ListConnection.ListViewMetaData.Columns)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn orgBandedGridColumn = Columns.ColumnByName(column.Name);
                BandedGridColumn newBandedGridColumn = new BandedGridColumn();
                newBandedGridColumn.Name = orgBandedGridColumn.Name;
                newBandedGridColumn.Caption = orgBandedGridColumn.Caption;
                newBandedGridColumn.FieldName = orgBandedGridColumn.FieldName;
                newBandedGridColumn.VisibleIndex = orgBandedGridColumn.VisibleIndex;
                Columns.Remove(orgBandedGridColumn);
                System.ComponentModel.Design.IDesignerHost designerHost = base.Site.Container as System.ComponentModel.Design.IDesignerHost;
                designerHost.DestroyComponent(orgBandedGridColumn);
                Columns.Add(newBandedGridColumn);
                newBandedGridColumn.Name = orgBandedGridColumn.Name;
                newBandedGridColumn.Caption = orgBandedGridColumn.Caption;
                newBandedGridColumn.FieldName = orgBandedGridColumn.FieldName;
                newBandedGridColumn.VisibleIndex = orgBandedGridColumn.VisibleIndex;
                newBandedGridColumn.ColumnMetaData = column;

            }
            
        }

        #endregion
    }


    //public partial class BandedGridView : UserControl
    //{
    //    public BandedGridView()
    //    {
    //        InitializeComponent();
    //    }
    //}
}
