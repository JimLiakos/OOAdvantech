using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ConnectableControls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Registrator;
using ConnectableControls.ListView;
using OOAdvantech.UserInterface;


namespace DXConnectableControls.XtraGrid.Views.Grid
{
    /// <MetaDataID>{d156438f-8fc2-46a7-a6e3-11e7616e5abe}</MetaDataID>
    public partial class GridView : DevExpress.XtraGrid.Views.Grid.GridView, ConnectableControls.ListView.IListView, IConvertibleView
    {
        public GridView()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.FocusedRowChanged+=new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(OnSelectionChanged);
           // 

            
        }
        protected override void RestoreLayoutCore(DevExpress.Utils.Serializing.XtraSerializer serializer, object path, DevExpress.Utils.OptionsLayoutBase options)
        {
            base.RestoreLayoutCore(serializer, path, options);
        }
        protected override void OnColumnAdded(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            base.OnColumnAdded(column);
            if (column is IColumn)
                (column as IColumn).Owner = this;
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
            if (!(e.Component is DXConnectableControls.XtraGrid.Columns.GridColumn)&& (e.Component is DevExpress.XtraGrid.Columns.GridColumn) && (e.Component as DevExpress.XtraGrid.Columns.GridColumn).View == this)
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
            else if(column==null )
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
            get {throw new NotImplementedException(); }
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

        ConnectableControls.ListView.IColumn ConnectableControls.ListView.IListView.AddColumn(OOAdvantech.UserInterface.Column column)
        {
            DXConnectableControls.XtraGrid.Columns.GridColumn gridColumn = new DXConnectableControls.XtraGrid.Columns.GridColumn(this, column);
            Columns.Add(gridColumn);
            gridColumn.Name = gridColumn.Name;

            
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
            List<string> columnTypesNames = new List<string>();
            //columnTypesNames.Add("ButtonColumn");
            columnTypesNames.Add("CheckBoxColumn");
            //columnTypesNames.Add("ColorColumn");
            columnTypesNames.Add("ComboBoxColumn");
            columnTypesNames.Add("DateTimeColumn");
            columnTypesNames.Add("ImageColumn");
            //columnTypesNames.Add("ProgressBarColumn");
            columnTypesNames.Add("SearchBoxColumn");
            columnTypesNames.Add("TextColumn");
            return columnTypesNames;
        }

        string ConnectableControls.ListView.IListView.GetColumnTypeName(ConnectableControls.ListView.IColumn SelectedColumn)
        {
            if (SelectedColumn.ColumnMetaData is TextColumn)
                return "TextColumn";
            if (SelectedColumn.ColumnMetaData is SearchBoxColumn)
                return "SearchBoxColumn";
            if (SelectedColumn.ColumnMetaData is CheckBoxColumn)
                return "CheckBoxColumn";
            if (SelectedColumn.ColumnMetaData is ComboBoxColumn)
                return "ComboBoxColumn";
            if (SelectedColumn is DateTimeColumn)
                return "DateTimeColumn";
            if (SelectedColumn.ColumnMetaData is ImageColumn)
                return "ImageColumn";
            //if (SelectedColumn is ProgressBarColumn)
            //    return "ProgressBarColumn";
            //if (SelectedColumn is ButtonColumn)
            //    return "ButtonColumn";

            //if (SelectedColumn is ColorColumn)
            //    return "ColorColumn";
            throw new System.Exception("Unknown ColumnType");
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
        void ConnectableControls.ListView.IListView.RefreshRowCell(int rowHandle, ConnectableControls.ListView.IColumn column)
        {
            RefreshRowCell(rowHandle, column as DevExpress.XtraGrid.Columns.GridColumn);
        }
        void ConnectableControls.ListView.IListView.SelectRows(List<IRow> rows)
        {
        }
        void ConnectableControls.ListView.IListView.SelectRows(List<int> rowsIndicies)
        {
        }

        void ConnectableControls.ListView.IListView.SelectRow(IRow row)
        {
        }
        void ConnectableControls.ListView.IListView.SelectRow(int rowsIndex)
        {
            if (!OptionsSelection.MultiSelect)
            {
                OptionsSelection.MultiSelect = true;
                ClearSelection();
                SelectRow(rowsIndex);
                OptionsSelection.MultiSelect = false;
                //FocusedRowHandle = rowsIndex;
            }
            else
                SelectRow(rowsIndex);
        }

        protected override bool PostEditor(bool causeValidation)
        {

            //try
            //{
                return base.PostEditor(causeValidation);
            //}
            //catch (Exception error)
            //{

            //    return false;
            //}
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
       

        #endregion

        #region IConvertableView Members

        void IConvertibleView.ConvertionCompleted()
        {
            foreach (OOAdvantech.UserInterface.Column column in ListConnection.ListViewMetaData.Columns)
            {
                DevExpress.XtraGrid.Columns.GridColumn orgGridColumn = Columns.ColumnByName(column.Name);
                DXConnectableControls.XtraGrid.Columns.GridColumn newGridColumn = new DXConnectableControls.XtraGrid.Columns.GridColumn();
                newGridColumn.Name = orgGridColumn.Name;
                newGridColumn.Caption = orgGridColumn.Caption;
                newGridColumn.FieldName = orgGridColumn.FieldName;
                newGridColumn.VisibleIndex = orgGridColumn.VisibleIndex;
                Columns.Remove(orgGridColumn);
                System.ComponentModel.Design.IDesignerHost designerHost = base.Site.Container as System.ComponentModel.Design.IDesignerHost;
                designerHost.DestroyComponent(orgGridColumn);
                Columns.Add(newGridColumn);
                newGridColumn.Name = orgGridColumn.Name;
                newGridColumn.Caption = orgGridColumn.Caption;
                newGridColumn.FieldName = orgGridColumn.FieldName;
                newGridColumn.VisibleIndex = orgGridColumn.VisibleIndex;
                newGridColumn.ColumnMetaData = column;

            }

            
        }

        #endregion
    }
}
