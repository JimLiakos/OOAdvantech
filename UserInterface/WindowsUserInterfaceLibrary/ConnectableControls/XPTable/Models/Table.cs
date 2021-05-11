/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using ConnectableControls;
using ConnectableControls.PropertyEditors;
using System.Linq;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;








using ConnectableControls.List.Editors;
using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Renderers;
using ConnectableControls.List.Sorting;
using ConnectableControls.List.Themes;
using ConnectableControls.List.Win32;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.UserInterface.Runtime;
using ConnectableControls.ListView;

namespace ConnectableControls.List.Models
{
    //public enum ColumnType
    //{
    //    TextColumn,
    //    ButtonColumn,
    //    CheckBoxColumn,
    //    ColorColumn,
    //    ComboBoxColumn,
    //    DateTimeColumn,
    //    ImageColumn,
    //    NumberColumn,
    //    ProgressBarColumn,
    //    SearchBoxColumn

    //}
}
namespace ConnectableControls.List
{

  


    /// <summary>
    /// Summary description for Table.
    /// </summary>
    /// <MetaDataID>{F211C39A-E6D5-4EA0-AEA7-CD8452C63496}</MetaDataID>
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ViewControlObject), "Table.bmp")]
    public class ListView : Control, ISupportInitialize, IListView
    {
        /// <MetaDataID>{f169c738-9ff8-4d63-8737-042db680054d}</MetaDataID>
        DependencyProperty _EnabledProperty;

        /// <MetaDataID>{37b923c4-3278-4c68-86b1-c3b96b267580}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        [Category("Object Model Connection")]
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
                    _EnabledProperty.ConnectableControl = ListConnection;
                }
            }
        }

        /// <MetaDataID>{96112955-188c-48b0-af6f-db4754efaf24}</MetaDataID>
        public void SelectRows(List<IRow> rows)
        {
        }
        /// <MetaDataID>{ef554834-c003-4a6b-bd21-9c639e5d4059}</MetaDataID>
        public void SelectRows(List<int> rowsIndicies)
        {
        }

        /// <MetaDataID>{ef6ddc14-908c-4410-9c11-5b2c39b87edf}</MetaDataID>
        public void SelectRow(IRow row)
        {
        }
        /// <MetaDataID>{e16e577f-6be1-426f-b6bf-30c165275367}</MetaDataID>
        public void SelectRow(int rowsIndex)
        {
        }

        #region Conectable controls code
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

        //internal OOAdvantech.MetaDataRepository.Classifier CollectionObjectType
        //{
        //    get
        //    {
        //        return ListConnection.CollectionObjectType;
          
        //    }
        //}



        #region backword cobatability

        //public OOAdvantech.MetaDataRepository.Classifier PresentationObjectType
        //{
        //    get
        //    {
        //        return ListConnection.PresentationObjectType;
        //    }
        //}
        //[Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        //public object AssignPresentationObjectType
        //{
        //    get
        //    {
        //        return  ListConnection.AssignPresentationObjectType;
        //    }
        //    set
        //    {
        //        ListConnection.AssignPresentationObjectType = value;
        //    }
        //}



        //[Category("DragDrop Behavior")]
        //[NotifyParentProperty(true)]
        //[Browsable(true)]
        //public bool AllowDrag
        //{
        //    get
        //    {
        //        return ListConnection.AllowDrag;
        //    }
        //    set
        //    {
        //        ListConnection.AllowDrag = value; ;
        //    }
        //}

        //System.Xml.XmlDocument MetaDataAsXmlDocument;
        //OOAdvantech.UserInterface.ListView ListViewMetaData;

        ///// <MetaDataID>{83C40642-200F-4371-9075-39C1CB7F5785}</MetaDataID>
        //private object _MetaData;
        ///// <MetaDataID>{5ED59BC4-4269-41B8-B1EE-C5262355FFED}</MetaDataID>
        //[Editor(typeof(EditListMetaData), typeof(System.Drawing.Design.UITypeEditor)),
        //Category("Object Model Connection")]
        //public Object MetaData
        //{
        //    get
        //    {
        //        return ListConnection.MetaData;
        //        //if (MetaDataAsXmlDocument == null)
        //        //{
        //        //    MetaDataAsXmlDocument = new System.Xml.XmlDocument();
        //        //    OOAdvantech.PersistenceLayer.ObjectStorage listViewStorage = null;
        //        //    listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
        //        //    ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;

        //        //}

        //        //MetaDataValue metaDataVaue = new MetaDataValue(MetaDataAsXmlDocument.OuterXml as string, "(List MetaData)");
        //        //metaDataVaue.MetaDataAsObject = this;



        //        //return metaDataVaue;
        //    }
        //    set
        //    {
        //        if(columnModel!=null)
        //            columnModel.Columns.Clear();
        //        ListConnection.MetaData = value;
        //        //string metaData = value as string;
        //        //MetaDataValue metaDataVaue = null;
        //        //if (value is MetaDataValue)
        //        //{
        //        //    metaData = (value as MetaDataValue).MetaDataAsXML;
        //        //    metaDataVaue = value as MetaDataValue;
        //        //}
        //        //OOAdvantech.PersistenceLayer.ObjectStorage listViewStorage = null;



        //        //if (MetaDataAsXmlDocument == null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
        //        //{
        //        //    MetaDataAsXmlDocument = new System.Xml.XmlDocument();
        //        //    try
        //        //    {

        //        //        if (!string.IsNullOrEmpty(metaData))
        //        //            MetaDataAsXmlDocument.LoadXml(metaData);

        //        //    }
        //        //    catch (Exception error)
        //        //    {
        //        //    }
        //        //    try
        //        //    {
        //        //        if (!string.IsNullOrEmpty(metaData))
        //        //            listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
        //        //        else
        //        //        {
        //        //            MetaDataAsXmlDocument = new System.Xml.XmlDocument();
        //        //            listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
        //        //            ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;
        //        //        }


        //        //    }
        //        //    catch (OOAdvantech.PersistenceLayer.StorageException error)
        //        //    {
        //        //        MetaDataAsXmlDocument = new System.Xml.XmlDocument();
        //        //        listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
        //        //        ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;
        //        //    }
        //        //    try
        //        //    {

        //        //        OOAdvantech.Collections.StructureSet set = listViewStorage.Execute("SELECT listView FROM OOAdvantech.UserInterface.ListView listView ");
        //        //        foreach (OOAdvantech.Collections.StructureSet setInstance in set)
        //        //        {
        //        //            ListViewMetaData = setInstance["listView"] as OOAdvantech.UserInterface.ListView;
        //        //            break;
        //        //        }
        //        //        if (ListViewMetaData == null)
        //        //            ListViewMetaData = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ListView)) as OOAdvantech.UserInterface.ListView;
        //        //    }
        //        //    catch (System.Exception error)
        //        //    {
        //        //        throw;
        //        //    }

        //        //    foreach (OOAdvantech.UserInterface.Column column in ListViewMetaData.Columns)
        //        //    {
        //        //        if (column is OOAdvantech.UserInterface.TextColumn)
        //        //        {
        //        //            Column columnView = new TextColumn(column);
        //        //            ColumnModel.Columns.Add(columnView);
        //        //        }
        //        //        if (column is OOAdvantech.UserInterface.ImageColumn)
        //        //        {
        //        //            Column columnView = new ImageColumn(column);
        //        //            ColumnModel.Columns.Add(columnView);
        //        //        }

        //        //        if (column is OOAdvantech.UserInterface.SearchBoxColumn)
        //        //        {
        //        //            Column columnView = new SearchBoxColumn(column as OOAdvantech.UserInterface.SearchBoxColumn);
        //        //            ColumnModel.Columns.Add(columnView);
        //        //        }
        //        //        if (column is OOAdvantech.UserInterface.ComboBoxColumn)
        //        //        {
        //        //            Column columnView = new ComboBoxColumn(column as OOAdvantech.UserInterface.ComboBoxColumn);
        //        //            ColumnModel.Columns.Add(columnView);
        //        //        }
        //        //        if (column is OOAdvantech.UserInterface.CheckBoxColumn)
        //        //        {
        //        //            Column columnView = new CheckBoxColumn(column as OOAdvantech.UserInterface.CheckBoxColumn);
        //        //            ColumnModel.Columns.Add(columnView);
        //        //        }
        //        //        if (column is OOAdvantech.UserInterface.DateTimeColumn)
        //        //        {
        //        //            Column columnView = new DateTimeColumn(column as OOAdvantech.UserInterface.DateTimeColumn);
        //        //            ColumnModel.Columns.Add(columnView);
        //        //        }



        //        //    }

        //        //}
        //        //return;

     
        //    }
        //}

        //string _SelectionMember;
        //[Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        //[Category("Object Model Connection")]
        //public object SelectionMember
        //{
        //    get
        //    {
        //        return ListConnection.SelectionMember;
        //        //return _SelectionMember;
        //    }
        //    set
        //    {
        //        ListConnection.SelectionMember = value;
        //        //if (value is string)
        //        //    _SelectionMember = value as string;
        //        //else if (value is MetaData)
        //        //    _SelectionMember = (value as MetaData).Path;
        //    }
        //}


        //[Category("Object Model Connection")]
        //public ViewControlObject ViewControlObject
        //{
        //    get
        //    {
        //        return ListConnection.ViewControlObject;
        //    }
        //    set
        //    {
        //        ListConnection.ViewControlObject = value;
        //    }
        //}

        //public object Path
        //{
        //    get
        //    {
        //        return ListConnection.Path;
        //    }
        //    set
        //    {
        //        ListConnection.Path = value;
        //    }


        //}

        //DragDropTransactionOptions _DragDropTransactionOption;
        //[Category("DragDrop Behavior")]
        //public DragDropTransactionOptions DragDropTransactionOption
        //{
        //    get
        //    {
        //        return ListConnection.DragDropTransactionOption;
        //    }
        //    set
        //    {
        //        ListConnection.DragDropTransactionOption = value;
        //    }
        //}

        #endregion

        /// <MetaDataID>{9961b682-8aeb-443d-81ac-b441785d488c}</MetaDataID>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (this.lastEditingCell != CellPos.Empty)
                {
                    int nextColumn = this.lastEditingCell.Column + 1;
                    while (columnModel.Columns.Count > nextColumn && !columnModel.Columns[nextColumn].Editable)
                        nextColumn++;

                    if (columnModel.Columns.Count > nextColumn)
                    {
                        EditCell(new CellPos(this.lastEditingCell.Row, nextColumn));
                        return true;
                    }

                }


            }

            return base.ProcessDialogKey(keyData);
        }


        /// <MetaDataID>{159da646-3b21-455e-84ba-3710cd5d46f0}</MetaDataID>
        ListConnection _ListConnection = null;
        /// <MetaDataID>{45ac61fa-a954-43bd-bd7c-dca9776c6237}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        [Category("Object Model Connection")]
        public ListConnection ListConnection
        {
            get
            {
                return _ListConnection;
            }
            set
            {
                if (value != null)
                    _ListConnection = value;
            }
        }

        /// <MetaDataID>{c530a85f-d4de-4dd9-a447-1f50ad05c715}</MetaDataID>
        void IListView.RefreshRow(int rowHandle)
        {
            throw new NotSupportedException();
        }

        /// <MetaDataID>{eb5a03a5-6b99-4bcf-a391-d5c5ebf6cb5e}</MetaDataID>
        int[] IListView.SelectedRowsIndicies
        {
            get
            {
                int[] selectedRowsIndicies = new int[SelectedItems.Length];
                int i = 0;
                foreach (IRow row in SelectedItems)
                {
                    selectedRowsIndicies[i]=row.Index;
                    i++;
                }
                return selectedRowsIndicies;
            }
        }

        /// <MetaDataID>{1d276027-b971-47df-bf52-6a991ac16913}</MetaDataID>
        void IListView.RefreshRowCell(int rowHandle, IColumn column)
        {
            throw new NotSupportedException();
        }

        /// <MetaDataID>{a4909231-c88b-457b-9410-4fea58d21cb4}</MetaDataID>
        bool ConnectableControls.ListView.IListView.DataSourceSupported
        {
            get
            {
                return false;
            }
            
        }

        /// <MetaDataID>{f251d3f6-1b5d-41cb-a03e-d12f5b395cc2}</MetaDataID>
        object ConnectableControls.ListView.IListView.DataSource
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <MetaDataID>{3ab71d2d-3fda-4619-b23b-b20cb4a7ef28}</MetaDataID>
        void ConnectableControls.ListView.IListView.RefreshDataSource()
        {
            throw new NotSupportedException();
        }


        /// <MetaDataID>{acdabc08-28bb-4b93-b484-e24b296b43d8}</MetaDataID>
        IRow ConnectableControls.ListView.IListView.InsertRow(int index, object displayedObj, IPresentationObject displayedPresentationObj)
        {
            Row row = new Row();
            if (displayedPresentationObj != null)
                row._PresentationObject = displayedPresentationObj;
            row.CollectionObject = displayedObj;

            foreach (Column column in ColumnModel.Columns)
                column.LoadCell(row);
            if (index == -1)
                TableModel.Rows.Add(row);
            else
                TableModel.Rows.Insert(index, row);

            return row;

        }
        /// <MetaDataID>{2c3b8dfa-0cab-4ea9-8091-1f0be42c883b}</MetaDataID>
        void ConnectableControls.ListView.IListView.RemoveAllRows()
        {
            TableModel m_tableModel = new TableModel();
            m_tableModel.RowHeight = RowHeight;
            TableModel = m_tableModel;
            Invalidate();
            

        }
        /// <MetaDataID>{b730ee02-6331-4f66-bdfc-fef8a3c44a53}</MetaDataID>
        IRow ConnectableControls.ListView.IListView.LastMouseOverRow
        {
            get
            {
                return tableModel.Rows[columnModel.Table.LastMouseCell.Row];
            }
        }
        /// <MetaDataID>{a99c9bb1-be5a-4cd4-baab-f18db1fa65db}</MetaDataID>
        void ConnectableControls.ListView.IListView.RemoveRowAt(int index)
        {
            TableModel.Rows.RemoveAt(index);

        }
        /// <MetaDataID>{7fe79b28-3f4f-4851-9e77-120b6c597644}</MetaDataID>
        List<IRow> ConnectableControls.ListView.IListView.SelectedRows
        {
            get 
            {
                List<IRow> selectedRows = new List<IRow>();
                foreach (IRow row in SelectedItems)
                    selectedRows.Add(row);
                return selectedRows;
            }
        }
        /// <MetaDataID>{ac1b3c69-ce28-4eb1-bdac-a06d6f97011b}</MetaDataID>
        List<IRow> ConnectableControls.ListView.IListView.Rows
        {
            get
            {
                List<IRow> rows = new List<IRow>();
                foreach (IRow row in tableModel.Rows)
                    rows.Add(row);
                return rows;
            }
        }
        /// <MetaDataID>{35939528-a0d8-43f7-92df-d23c7ecde137}</MetaDataID>
        System.Collections.Generic.List<IColumn> ConnectableControls.ListView.IListView.Columns
        {
            get
            {
                System.Collections.Generic.List<IColumn> columns = new List<IColumn>();
                if (columnModel == null)
                    return columns;
                foreach (IColumn column in columnModel.Columns)
                    columns.Add(column);
                return columns;

            }
        }
        /// <MetaDataID>{d59deb14-d81f-4718-b97d-f6b1cdd4eca1}</MetaDataID>
        IColumn ConnectableControls.ListView.IListView.AddColumn(OOAdvantech.UserInterface.Column column)
        {
            if (column is OOAdvantech.UserInterface.TextColumn)
            {
                Column columnView = new TextColumn(column);
                ColumnModel.Columns.Add(columnView);
                return columnView;
            }
            if (column is OOAdvantech.UserInterface.ImageColumn)
            {
                Column columnView = new ImageColumn(column);
                ColumnModel.Columns.Add(columnView);
                return columnView;
            }

            if (column is OOAdvantech.UserInterface.SearchBoxColumn)
            {
                Column columnView = new SearchBoxColumn(column as OOAdvantech.UserInterface.SearchBoxColumn);
                ColumnModel.Columns.Add(columnView);
                return columnView;
            }
            if (column is OOAdvantech.UserInterface.ComboBoxColumn)
            {
                Column columnView = new ComboBoxColumn(column as OOAdvantech.UserInterface.ComboBoxColumn);
                ColumnModel.Columns.Add(columnView);
                return columnView;
            }
            if (column is OOAdvantech.UserInterface.CheckBoxColumn)
            {
                Column columnView = new CheckBoxColumn(column as OOAdvantech.UserInterface.CheckBoxColumn);
                ColumnModel.Columns.Add(columnView);
                return columnView;
            }
            if (column is OOAdvantech.UserInterface.DateTimeColumn)
            {
                Column columnView = new DateTimeColumn(column as OOAdvantech.UserInterface.DateTimeColumn);
                ColumnModel.Columns.Add(columnView);
                return columnView;
            }
            return null;
        }
        /// <MetaDataID>{d8551328-4f22-42b9-92dc-e3982fa0827d}</MetaDataID>
        IColumn ConnectableControls.ListView.IListView.ChangeColumnType(IColumn selectedColumn, string columnType)
        {
            if (columnType == "ComboBoxColumn" &&
             selectedColumn != null &&
             !(selectedColumn is ComboBoxColumn))
            {
                OOAdvantech.UserInterface.ComboBoxColumn comboBoxColumn = new OOAdvantech.UserInterface.ComboBoxColumn(selectedColumn.ColumnMetaData);

                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(selectedColumn.ColumnMetaData).CommitTransientObjectState(comboBoxColumn);

                ComboBoxColumn comboBoxColumnView = new ComboBoxColumn(comboBoxColumn);
                comboBoxColumnView.ColumnMetaData = comboBoxColumn;
                ListConnection.AddColumn(comboBoxColumnView);
                IColumn oldColumn = selectedColumn;
                selectedColumn = comboBoxColumnView;
                ListConnection.RemoveColumn(oldColumn);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(oldColumn.ColumnMetaData);
            }
            else if (columnType == "CheckBoxColumn" &&
             selectedColumn != null &&
             !(selectedColumn is CheckBoxColumn))
            {
                OOAdvantech.UserInterface.CheckBoxColumn checkBoxColumn = new OOAdvantech.UserInterface.CheckBoxColumn(selectedColumn.ColumnMetaData);

                CheckBoxColumn checkBoxColumnView = new CheckBoxColumn(checkBoxColumn);
                checkBoxColumnView.ColumnMetaData = checkBoxColumn;
                IColumn oldColumn = selectedColumn;
                ListConnection.AddColumn(checkBoxColumnView);

                

                selectedColumn = checkBoxColumnView;
                ListConnection.RemoveColumn(oldColumn);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(oldColumn.ColumnMetaData);
            }
            else if (columnType == "SearchBoxColumn" &&
                selectedColumn != null &&
                !(selectedColumn is SearchBoxColumn))
            {
                OOAdvantech.UserInterface.SearchBoxColumn searchBoxColumn = new OOAdvantech.UserInterface.SearchBoxColumn(selectedColumn.ColumnMetaData);

                SearchBoxColumn searchBoxColumnView = new SearchBoxColumn(searchBoxColumn);
                searchBoxColumnView.ColumnMetaData = searchBoxColumn;
                ListConnection.AddColumn(searchBoxColumnView);
                IColumn oldColumn = selectedColumn;
                selectedColumn = searchBoxColumnView;
                ListConnection.RemoveColumn(oldColumn);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(oldColumn.ColumnMetaData);
            }
            else if (columnType == "TextColumn" &&
                 selectedColumn != null &&
                 !(selectedColumn is TextColumn))
            {
                OOAdvantech.UserInterface.TextColumn textColumn = new OOAdvantech.UserInterface.TextColumn(selectedColumn.ColumnMetaData);
                TextColumn textColumnView = new TextColumn(textColumn);
                textColumnView.ColumnMetaData = textColumn;
                ListConnection.AddColumn(textColumnView);
                IColumn oldColumn = selectedColumn;
                selectedColumn = textColumnView;
                ListConnection.RemoveColumn(oldColumn);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(oldColumn.ColumnMetaData);
            }
            else if (columnType == "ImageColumn" &&
             selectedColumn != null &&
             !(selectedColumn is ImageColumn))
            {
                OOAdvantech.UserInterface.ImageColumn imageColumn = new OOAdvantech.UserInterface.ImageColumn(selectedColumn.ColumnMetaData);
                ImageColumn imageColumnView = new ImageColumn(imageColumn);
                imageColumnView.ColumnMetaData = imageColumn;
                ListConnection.AddColumn(imageColumnView);
                IColumn oldColumn = selectedColumn;
                selectedColumn = imageColumnView;
                ListConnection.RemoveColumn(oldColumn);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(oldColumn.ColumnMetaData);
            }
            else if (columnType == "DateTimeColumn" &&
            selectedColumn != null &&
            !(selectedColumn is DateTimeColumn))
            {
                OOAdvantech.UserInterface.DateTimeColumn dateTimeColumn = new OOAdvantech.UserInterface.DateTimeColumn(selectedColumn.ColumnMetaData);
                DateTimeColumn dateTimeColumnView = new DateTimeColumn(dateTimeColumn);
                dateTimeColumnView.ColumnMetaData = dateTimeColumn;
                ListConnection.AddColumn(dateTimeColumnView);
                IColumn oldColumn = selectedColumn;
                selectedColumn = dateTimeColumnView;
                ListConnection.RemoveColumn(oldColumn);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(oldColumn.ColumnMetaData);
            }
            return selectedColumn;

            //int i = 0;
            //Columns.Clear();
            //foreach (ColumnItemView currentColumnItemView in ColumnsList.Items)
            //{
            //    currentColumnItemView.MetaObjectItemView.Column.Order = i++;
            //    ListView.AddColumn(currentColumnItemView.MetaObjectItemView.Column);

            //}
        }
        /// <MetaDataID>{4f377af4-f4b6-4da9-aaf5-c44ffd949fc3}</MetaDataID>
        string ConnectableControls.ListView.IListView.GetColumnTypeName(IColumn SelectedColumn)
        {

            if (SelectedColumn is TextColumn)
                return "TextColumn";

            //    ColumnsListMenu.MenuCommands[0].Checked = true;

            if (SelectedColumn is SearchBoxColumn)
                return "SearchBoxColumn";
            //    ColumnsListMenu.MenuCommands[9].Checked = true;

            if (SelectedColumn is CheckBoxColumn)
                return "CheckBoxColumn";
            //    ColumnsListMenu.MenuCommands[2].Checked = true;

            if (SelectedColumn is ComboBoxColumn)
                return "ComboBoxColumn";
            //    ColumnsListMenu.MenuCommands[4].Checked = true;

            if (SelectedColumn is DateTimeColumn)
                return "DateTimeColumn";
            //    ColumnsListMenu.MenuCommands[5].Checked = true;
            if (SelectedColumn is ImageColumn)
                return "ImageColumn";
            //    ColumnsListMenu.MenuCommands[6].Checked = true;

            if (SelectedColumn is ProgressBarColumn)
                return "ProgressBarColumn";
            if (SelectedColumn is ButtonColumn)
                return "ButtonColumn";

            if (SelectedColumn is ColorColumn)
                return "ColorColumn";
            throw new System.Exception("Unknown ColumnType");
        }
        /// <MetaDataID>{670a5fff-83b2-46f6-8d7b-5d7cad963066}</MetaDataID>
        List<string> ConnectableControls.ListView.IListView.GetColumnTypesNames()
        {

            List<string> columnTypesNames = new List<string>();
            columnTypesNames.Add("ButtonColumn");
            columnTypesNames.Add("CheckBoxColumn");
            columnTypesNames.Add("ColorColumn");
            columnTypesNames.Add("ComboBoxColumn");
            columnTypesNames.Add("DateTimeColumn");
            columnTypesNames.Add("ImageColumn");
            columnTypesNames.Add("ProgressBarColumn");
            columnTypesNames.Add("SearchBoxColumn");
            columnTypesNames.Add("TextColumn");
            return columnTypesNames;

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
                Column xColumn = x as Column;
                Column yColumn = y as Column;
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
        ///// <MetaDataID>{DDA72F37-34D9-468E-BE6A-BC6FAC2BBE06}</MetaDataID>
        //internal static Column CreateColumn(ColumnType columnType)
        //{

        //    switch (columnType)
        //    {
        //        case ColumnType.ButtonColumn:
        //            {
        //                return new List.Models.ButtonColumn();
        //                break;
        //            }
        //        case ColumnType.CheckBoxColumn:
        //            {
        //                return new List.Models.CheckBoxColumn();
        //                break;
        //            }
        //        case ColumnType.ColorColumn:
        //            {
        //                return new List.Models.ColorColumn();
        //                break;
        //            }
        //        case ColumnType.ComboBoxColumn:
        //            {
        //                return new List.Models.ComboBoxColumn();
        //                break;
        //            }
        //        case ColumnType.DateTimeColumn:
        //            {
        //                return new List.Models.DateTimeColumn();
        //                break;
        //            }
        //        case ColumnType.ImageColumn:
        //            {
        //                return new List.Models.ImageColumn();
        //                break;
        //            }
        //        case ColumnType.NumberColumn:
        //            {
        //                return new List.Models.NumberColumn();
        //                break;
        //            }
        //        case ColumnType.ProgressBarColumn:
        //            {
        //                return new List.Models.ProgressBarColumn();
        //                break;
        //            }
        //        case ColumnType.TextColumn:
        //            {
        //                return new List.Models.TextColumn();
        //                break;
        //            }
        //        case ColumnType.SearchBoxColumn:
        //            {
        //                return new List.Models.SearchBoxColumn();
        //                break;
        //            }
        //    }
        //    return new List.Models.TextColumn();

        //}


        /// <MetaDataID>{936d6407-59a0-48b5-b1ac-295ba5ae30f1}</MetaDataID>
        Timer MouseDownTimer = new Timer();
        /// <MetaDataID>{873778e6-ebfc-4c58-bbfe-d09755fa2be3}</MetaDataID>
        MouseEventArgs LazyMouseEvent;
        /// <MetaDataID>{ef20da2f-ca4d-41d1-899b-f6e7a2bc3d1e}</MetaDataID>
        void MouseDownTimerTick(object sender, EventArgs e)
        {
            OnMouseDown(LazyMouseEvent);
            LazyMouseEvent = null;
            MouseDownTimer.Enabled = false;
        }


        /// <MetaDataID>{45B9DAEF-3059-44FE-8CA8-2EE2FF5007FA}</MetaDataID>
        public IRow InsertRow()
        {

            int index = -1;

            if (SelectedIndicies.Length != 0)
                index = SelectedIndicies[SelectedIndicies.Length - 1];

            IRow row = ListConnection.InsertRow(index);
            if (row != null)
            {

                int nextColumn = 0;
                while (columnModel.Columns.Count > nextColumn && !columnModel.Columns[nextColumn].Editable)
                    nextColumn++;
                if (columnModel.Columns.Count > nextColumn)
                {
                    EditCell(new CellPos(row.Index, nextColumn));
                }
            }
            return row;

        }



        /// <MetaDataID>{2fc974d7-fae6-4d70-9a52-69574603e6b3}</MetaDataID>
        private CellPos lastEditingCell;

        /// <MetaDataID>{94858EA4-8523-4EFB-98C7-19D373AFC220}</MetaDataID>
        public void RemoveColumn(IColumn column)
        {
            columnModel.Columns.Remove(column as Column);
        }

        /// <MetaDataID>{F52B1337-C1B0-46D6-BD24-CB5906C01DF0}</MetaDataID>
        public void AddColumn(IColumn columnView)
        {
            if (columnView.Owner == null)
                columnView.Order = ColumnModel.Columns.Add(columnView as Column);
        }
        ///// <MetaDataID>{7FC75432-CB04-4D35-BBD5-0C1B705D678A}</MetaDataID>
        //public IColumn AddColumn(string path)
        //{
        //    OOAdvantech.UserInterface.TextColumn textColumn = new OOAdvantech.UserInterface.TextColumn();
        //    ListViewMetaData.AddColumn(textColumn);
        //    Column columnView = new TextColumn(textColumn);
        //    columnView.Order = columnModel.Columns.Add(columnView);
        //    return columnView;
        //}

        #region DragDrop Behavior

        //public void CutObject(object obj)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}



        /// <MetaDataID>{94ce879c-24b8-460e-90c9-cdc5ca0bb562}</MetaDataID>
        private void DragRow(MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left && this.TableState != TableState.ColumnResizing)
            {
                
                LazyMouseEvent = null;
                MouseDownTimer.Enabled = false;
                List<IRow> rows = new List<IRow>();
                if (SelectedItems.Length > 1)
                {
                    
                    foreach(IRow row in SelectedItems)
                        rows.Add(row);

                    ListConnection.DragRows(rows);
                }
                else
                {
                    int rowIndex = RowIndexAt(new Point(e.X, e.Y));
                    if (rowIndex != -1)
                    {
                        rows.Add(TableModel.Rows[rowIndex]);
                        ListConnection.DragRows(rows);
                    }
                }
            }
        }





        /// <MetaDataID>{8dc91392-6153-4204-81fe-ba8d07ad7203}</MetaDataID>
        private Color _dragDropMarkColor = Color.Blue;
        /// <MetaDataID>{a3d39502-c6cf-43d6-aaa0-667107de6c92}</MetaDataID>
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

        /// <MetaDataID>{77a12bf7-86e6-466e-8629-207aa561bff0}</MetaDataID>
        private float _dragDropMarkWidth = 1.0f;
        /// <MetaDataID>{00a2d5c9-7bfa-476c-a8f9-785c43d15084}</MetaDataID>
        [DefaultValue(1.0f), Category("DragDrop Behavior")]
        public float DragDropMarkWidth
        {
            get { return _dragDropMarkWidth; }
            set
            {
                _dragDropMarkWidth = value;
                CreateMarkPen();
            }
        }
        /// <MetaDataID>{012ce3fb-3211-4edf-9179-d62a79d9ae74}</MetaDataID>
        private Pen _markPen;
        /// <MetaDataID>{b153b843-7291-4fc1-8309-c71a9019b3a6}</MetaDataID>
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


        /// <MetaDataID>{b4ee4142-1b8a-4243-b337-ff5d0cf6c9e4}</MetaDataID>
        DragDropEffects DragDropEffect = DragDropEffects.None;
        /// <MetaDataID>{33ff2427-434b-45d0-9c6f-69cc86c77195}</MetaDataID>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            //drgevent.Effect=
            if (DragDropEffect != DragDropEffects.None)
            {

                Invalidate();

                Point point = PointToClient(new Point(drgevent.X, drgevent.Y));

                if (this.vScrollBar.Visible && PointToClient(new Point(drgevent.X, drgevent.Y)).Y < (RowHeight))
                {
                    if (this.vScrollBar.Value > 0)
                        this.vScrollBar.Value--;

                }

                if (this.vScrollBar.Visible && PointToClient(new Point(drgevent.X, drgevent.Y)).Y > (Height - RowHeight))
                {
                    if (this.vScrollBar.Value <= this.vScrollBar.Maximum - this.vScrollBar.LargeChange)
                        this.vScrollBar.Value++;

                }
            }
            base.OnDragOver(drgevent);
        }
        /// <MetaDataID>{bd66938c-00a3-441f-a100-7c1ccb0f63c1}</MetaDataID>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            ListConnection.DragEnter(drgevent);

            base.OnDragEnter(drgevent);

        }
        /// <MetaDataID>{9acf9f5f-65a5-4159-a3f9-980274900de0}</MetaDataID>
        protected override void OnDragLeave(EventArgs e)
        {
            
            DragDropEffect = DragDropEffects.None;
            ListConnection.DragDropObject = null;
            Invalidate();

            base.OnDragLeave(e);
        }
        /// <MetaDataID>{11e6fbcb-d572-4dd6-bf68-509a88157ae5}</MetaDataID>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            ListConnection.OnDragDrop(drgevent);
          
            Invalidate();

            base.OnDragDrop(drgevent);
        }

        /// <MetaDataID>{a0918da0-61d0-45d5-b2fe-43e17ef7140f}</MetaDataID>
        int DragDropMarkPos = -1;

        #endregion






        #region ICollectionViewRunTime Members

        /// <MetaDataID>{d11a74ff-811b-4682-a9e4-58a48e007de3}</MetaDataID>
        public void SetSelected(object[] items)
        {

        }

        /// <MetaDataID>{3a6ef198-114d-40d2-956a-901a304ed713}</MetaDataID>
        public void AddItem(object item)
        {
            ListConnection.AddItem(item);

        }

        /// <MetaDataID>{7223ffad-3511-40bd-8f90-d6296dcfd19d}</MetaDataID>
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

                ListConnection.RemoveItem(item);

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

        /// <MetaDataID>{4bf7163e-4dc1-4bb1-8a0f-db3ffb672c06}</MetaDataID>
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

        /// <MetaDataID>{89db95cb-1439-4b06-b2e5-ef3fbaf4ade0}</MetaDataID>
        public void SetRowColor(int index, Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ICollection Members

        /// <MetaDataID>{4d6ae46b-ed8e-4bce-afd4-82f09a68d6e7}</MetaDataID>
        public void CopyTo(Array array, int index)
        {

        }

        //public int Count
        //{
        //    get { return 0; }
        //}

        //public bool IsSynchronized
        //{
        //    get { return true; }
        //}

        //public object SyncRoot
        //{
        //    get { return null; }
        //}

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{2edfdda6-fa3d-429d-9306-d903f28acf8f}</MetaDataID>
        public IEnumerator GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
        #endregion




        /// $$$$$$$$$$$$$$$///
        /// <MetaDataID>{503f4cf7-d780-4146-9f7e-9a9703d6530d}</MetaDataID>

        #region Menu code





        private void EditRow(Row row)
        {
            ListConnection.EditRow(row);
            //if (EditRowOperationCaller == null)
            //    return;

            //if (EditRowOperationCaller.Operation != null)
            //{
            //    object reVal = EditRowOperationCaller.Invoke();
            //    ViewControlObject.UserInterfaceObjectConnection.UpdateUserInterfaceFor(row.CollectionObject);
            //    //DisplayedValue displayedValue = ViewControlObject.UserInterfaceObjectConnection.GetDisplayedValue(row.CollectionObject, null);
            //    //if (displayedValue != null && displayedValue.Value != null)
            //    //    displayedValue.UpdateUserInterface();
            //}

        }


        #endregion

        #region Event Handlers

        #region Cells

        /// <summary>
        /// Occurs when the value of a Cells property changes
        /// </summary>
        public event CellEventHandler CellPropertyChanged;

        #region Focus

        /// <summary>
        /// Occurs when a Cell gains focus
        /// </summary>
        public event CellFocusEventHandler CellGotFocus;

        /// <summary>
        /// Occurs when a Cell loses focus
        /// </summary>
        public event CellFocusEventHandler CellLostFocus;

        #endregion

        #region Keys

        /// <summary>
        /// Occurs when a key is pressed when a Cell has focus
        /// </summary>
        public event CellKeyEventHandler CellKeyDown;

        /// <summary>
        /// Occurs when a key is released when a Cell has focus
        /// </summary>
        public event CellKeyEventHandler CellKeyUp;

        #endregion

        #region Mouse

        /// <summary>
        /// Occurs when the mouse pointer enters a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseLeave;

        /// <summary>
        /// Occurs when a mouse pointer is over a Cell and a mouse button is pressed
        /// </summary>
        public event CellMouseEventHandler CellMouseDown;

        /// <summary>
        /// Occurs when a mouse pointer is over a Cell and a mouse button is released
        /// </summary>
        public event CellMouseEventHandler CellMouseUp;

        /// <summary>
        /// Occurs when a mouse pointer is moved over a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer hovers over a Cell
        /// </summary>
        public event CellMouseEventHandler CellMouseHover;

        /// <summary>
        /// Occurs when a Cell is clicked
        /// </summary>
        public event CellMouseEventHandler CellClick;

        /// <summary>
        /// Occurs when a Cell is double-clicked
        /// </summary>
        public event CellMouseEventHandler CellDoubleClick;

        #endregion

        #region Buttons

        /// <summary>
        /// Occurs when a Cell's button is clicked
        /// </summary>
        public event CellButtonEventHandler CellButtonClicked;

        #endregion

        #region CheckBox

        /// <summary>
        /// Occurs when a Cell's Checked value changes
        /// </summary>
        public event CellCheckBoxEventHandler CellCheckChanged;

        #endregion

        #endregion

        #region Column

        /// <summary>
        /// Occurs when a Column's property changes
        /// </summary>
        public event ColumnEventHandler ColumnPropertyChanged;

        #endregion

        #region Column Headers

        /// <summary>
        /// Occurs when the mouse pointer enters a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseLeave;

        /// <summary>
        /// Occurs when a mouse pointer is over a Column Header and a mouse button is pressed
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseDown;

        /// <summary>
        /// Occurs when a mouse pointer is over a Column Header and a mouse button is released
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseUp;

        /// <summary>
        /// Occurs when a mouse pointer is moved over a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer hovers over a Column Header
        /// </summary>
        public event HeaderMouseEventHandler HeaderMouseHover;

        /// <summary>
        /// Occurs when a Column Header is clicked
        /// </summary>
        public event HeaderMouseEventHandler HeaderClick;

        /// <summary>
        /// Occurs when a Column Header is double-clicked
        /// </summary>
        public event HeaderMouseEventHandler HeaderDoubleClick;

        /// <summary>
        /// Occurs when the height of the Column Headers changes
        /// </summary>
        public event EventHandler HeaderHeightChanged;

        #endregion

        #region ColumnModel

        /// <summary>
        /// Occurs when the value of the Table's ColumnModel property changes 
        /// </summary>
        public event EventHandler ColumnModelChanged;

        /// <summary>
        /// Occurs when a Column is added to the ColumnModel
        /// </summary>
        public event ColumnModelEventHandler ColumnAdded;

        /// <summary>
        /// Occurs when a Column is removed from the ColumnModel
        /// </summary>
        public event ColumnModelEventHandler ColumnRemoved;

        #endregion

        #region Editing

        /// <summary>
        /// Occurs when the Table begins editing a Cell
        /// </summary>
        public event CellEditEventHandler BeginEditing;

        /// <summary>
        /// Occurs when the Table stops editing a Cell
        /// </summary>
        public event CellEditEventHandler EditingStopped;

        /// <summary>
        /// Occurs when the editing of a Cell is cancelled
        /// </summary>
        public event CellEditEventHandler EditingCancelled;

        #endregion

        #region Rows

        /// <summary>
        /// Occurs when a Cell is added to a Row
        /// </summary>
        public event RowEventHandler CellAdded;

        /// <summary>
        /// Occurs when a Cell is removed from a Row
        /// </summary>
        public event RowEventHandler CellRemoved;

        /// <summary>
        /// Occurs when the value of a Rows property changes
        /// </summary>
        public event RowEventHandler RowPropertyChanged;

        #endregion

        #region Sorting

        /// <summary>
        /// Occurs when a Column is about to be sorted
        /// </summary>
        public event ColumnEventHandler BeginSort;

        /// <summary>
        /// Occurs after a Column has finished sorting
        /// </summary>
        public event ColumnEventHandler EndSort;

        #endregion

        #region Painting

        /// <summary>
        /// Occurs before a Cell is painted
        /// </summary>
        public event PaintCellEventHandler BeforePaintCell;

        /// <summary>
        /// Occurs after a Cell is painted
        /// </summary>
        public event PaintCellEventHandler AfterPaintCell;

        /// <summary>
        /// Occurs before a Column header is painted
        /// </summary>
        public event PaintHeaderEventHandler BeforePaintHeader;

        /// <summary>
        /// Occurs after a Column header is painted
        /// </summary>
        public event PaintHeaderEventHandler AfterPaintHeader;

        #endregion

        #region TableModel

        /// <summary>
        /// Occurs when the value of the Table's TableModel property changes 
        /// </summary>
        public event EventHandler TableModelChanged;

        /// <summary>
        /// Occurs when a Row is added into the TableModel
        /// </summary>
        public event TableModelEventHandler RowAdded;

        /// <summary>
        /// Occurs when a Row is removed from the TableModel
        /// </summary>
        public event TableModelEventHandler RowRemoved;

        /// <summary>
        /// Occurs when the value of the TableModel Selection property changes
        /// </summary>
        public event SelectionEventHandler SelectionChanged;

        /// <summary>
        /// Occurs when the value of the RowHeight property changes
        /// </summary>
        public event EventHandler RowHeightChanged;

        #endregion

        #endregion

        #region Class Data

        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{d451070a-1491-47ca-bc78-e17569efcb32}</MetaDataID>
        private System.ComponentModel.Container components = null;

        #region Border

        /// <summary>
        /// The style of the Table's border
        /// </summary>
        /// <MetaDataID>{741e9ab6-feee-4941-8b78-8db058f6269d}</MetaDataID>
        private BorderStyle borderStyle;

        #endregion

        #region Cells

        /// <summary>
        /// The last known cell position that the mouse was over
        /// </summary>
        /// <MetaDataID>{51dd0765-20b2-4bea-91c4-3846d0ffb01a}</MetaDataID>
        private CellPos lastMouseCell;

        /// <summary>
        /// The last known cell position that the mouse's left 
        /// button was pressed in
        /// </summary>
        /// <MetaDataID>{a1f90058-7f8a-472d-96be-7d2385bd88f5}</MetaDataID>
        private CellPos lastMouseDownCell;

        /// <MetaDataID>{ab053484-0885-4618-93d9-95dfc1981e05}</MetaDataID>
        private CellPos lastMouseClickCell;

        /// <summary>
        /// The position of the Cell that currently has focus
        /// </summary>
        /// <MetaDataID>{aafef682-36f4-4b19-acb4-28e40ffef717}</MetaDataID>
        private CellPos focusedCell;

        /// <summary>
        /// The Cell that is currently being edited
        /// </summary>
        /// <MetaDataID>{ea418b04-2f65-4e67-89d1-31e3a78f652a}</MetaDataID>
        private CellPos editingCell;

        /// <summary>
        /// The ICellEditor that is currently being used to edit a Cell
        /// </summary>
        /// <MetaDataID>{3ffbb278-6d1c-4969-8655-ee83912ccebf}</MetaDataID>
        private ICellEditor curentCellEditor;

        /// <summary>
        /// The action that must be performed on a Cell to start editing
        /// </summary>
        /// <MetaDataID>{beea08c0-93c8-44ab-aef9-46e519b97490}</MetaDataID>
        private EditStartAction editStartAction;

        /// <summary>
        /// The key that must be pressed for editing to start when 
        /// editStartAction is set to EditStartAction.CustomKey
        /// </summary>
        /// <MetaDataID>{6a8f70f0-d21a-4e8c-88a8-bc6ce66b01d0}</MetaDataID>
        private Keys customEditKey;

        /// <summary>
        /// The amount of time (in milliseconds) that that the 
        /// mouse pointer must hover over a Cell or Column Header before 
        /// a MouseHover event is raised
        /// </summary>
        /// <MetaDataID>{5e46e442-7b59-43ca-9d14-e7ed65a7dbc2}</MetaDataID>
        private int hoverTime;

        /// <summary>
        /// A TRACKMOUSEEVENT used to set the hoverTime
        /// </summary>
        /// <MetaDataID>{47400b02-a6cb-449c-9d81-a9a31f2be901}</MetaDataID>
        private TRACKMOUSEEVENT trackMouseEvent;

        #endregion

        #region Columns

        /// <summary>
        /// The ColumnModel of the Table
        /// </summary>
        /// <MetaDataID>{52cd97ed-9d8b-4bab-8b64-670fe0c3d8a1}</MetaDataID>
        private ColumnModel columnModel;

        /// <summary>
        /// Whether the Table supports column resizing
        /// </summary>
        /// <MetaDataID>{79d6e4b9-f848-4ba1-9c3d-1064a53051ab}</MetaDataID>
        private bool columnResizing;

        /// <summary>
        /// The index of the column currently being resized
        /// </summary>
        /// <MetaDataID>{c64d5d1b-0d84-43f0-a241-9b7693b31404}</MetaDataID>
        private int resizingColumnIndex;

        /// <summary>
        /// The x coordinate of the currently resizing column
        /// </summary>
        /// <MetaDataID>{06e1d67d-ceff-4cb3-b961-65362ce14992}</MetaDataID>
        private int resizingColumnAnchor;

        /// <summary>
        /// The horizontal distance between the resize starting
        /// point and the right edge of the resizing column
        /// </summary>
        /// <MetaDataID>{23b72f9c-cc85-42e5-9ba4-b37bb757db0c}</MetaDataID>
        private int resizingColumnOffset;

        /// <summary>
        /// The width that the resizing column will be set to 
        /// once column resizing is finished
        /// </summary>
        /// <MetaDataID>{74808473-7ba0-405a-b0e6-0e7f23e080ac}</MetaDataID>
        private int resizingColumnWidth;

        /// <summary>
        /// The index of the current pressed column
        /// </summary>
        /// <MetaDataID>{6a19a146-808f-4d23-a5e8-4c3609fcef5b}</MetaDataID>
        private int pressedColumn;

        /// <summary>
        /// The index of the current "hot" column
        /// </summary>
        /// <MetaDataID>{c3ea74de-6b46-4a64-b6f9-fcadb275d033}</MetaDataID>
        private int hotColumn;

        /// <summary>
        /// The index of the last sorted column
        /// </summary>
        /// <MetaDataID>{6b4f6828-b739-49fc-9826-e04db2ebff2e}</MetaDataID>
        private int lastSortedColumn;

        /// <summary>
        /// The Color of a sorted Column's background
        /// </summary>
        /// <MetaDataID>{a63cc6d0-8a57-48d9-ad50-cd8311df47bf}</MetaDataID>
        private Color sortedColumnBackColor;

        #endregion

        #region Grid

        /// <summary>
        /// Indicates whether grid lines appear between the rows and columns 
        /// containing the rows and cells in the Table
        /// </summary>
        /// <MetaDataID>{58fe8e98-dae5-4126-ae96-db99d0140c7b}</MetaDataID>
        private GridLines gridLines;

        /// <summary>
        /// The color of the grid lines
        /// </summary>
        /// <MetaDataID>{4e62dabf-b0b7-4a72-bc02-59e5d10c2d6d}</MetaDataID>
        private Color gridColor;

        /// <summary>
        /// The line style of the grid lines
        /// </summary>
        /// <MetaDataID>{b6dcd819-29eb-4aae-9336-21cac1696514}</MetaDataID>
        private GridLineStyle gridLineStyle;

        #endregion

        #region Header

        /// <summary>
        /// The styles of the column headers 
        /// </summary>
        /// <MetaDataID>{367aea0e-48e3-4733-aa94-19417fad3882}</MetaDataID>
        private ColumnHeaderStyle headerStyle;

        /// <summary>
        /// The Renderer used to paint the column headers
        /// </summary>
        /// <MetaDataID>{5a91520c-dbad-416e-9695-6d745111c031}</MetaDataID>
        private HeaderRenderer headerRenderer;

        /// <summary>
        /// The font used to draw the text in the column header
        /// </summary>
        /// <MetaDataID>{e08c1397-0737-438e-8b13-8366ad0211df}</MetaDataID>
        private Font headerFont;

        /// <summary>
        /// The ContextMenu for the column headers
        /// </summary>
        /// <MetaDataID>{99bf38a6-a7d4-4ee9-8063-d1694500fe31}</MetaDataID>
        private HeaderContextMenu headerContextMenu;

        #endregion

        #region Items

        /// <summary>
        /// The TableModel of the Table
        /// </summary>
        /// <MetaDataID>{ddba9fbd-4b54-4be9-950e-fc501eda09a5}</MetaDataID>
        private TableModel tableModel;

        #endregion

        #region Scrollbars

        /// <summary>
        /// Indicates whether the Table will allow the user to scroll to any 
        /// columns or rows placed outside of its visible boundaries
        /// </summary>
        /// <MetaDataID>{79d7383f-94d4-4afa-886a-eb10638ebfab}</MetaDataID>
        private bool scrollable;

        /// <summary>
        /// The Table's horizontal ScrollBar
        /// </summary>
        /// <MetaDataID>{766fb9a0-d4b3-4f28-8da1-78dd9f58ee92}</MetaDataID>
        private HScrollBar hScrollBar;

        /// <summary>
        /// The Table's vertical ScrollBar
        /// </summary>
        /// <MetaDataID>{5fc11ac2-6f0e-48bd-a271-27e64417a7a6}</MetaDataID>
        private VScrollBar vScrollBar;

        #endregion

        #region Selection

        /// <summary>
        /// Specifies whether rows and cells can be selected
        /// </summary>
        /// <MetaDataID>{6190e1c0-6a4e-4995-a5a6-7ff036494907}</MetaDataID>
        private bool allowSelection;

        /// <summary>
        /// Specifies whether multiple rows and cells can be selected
        /// </summary>
        /// <MetaDataID>{109f645b-d1e5-455b-931b-fbea0cbb5781}</MetaDataID>
        private bool multiSelect;

        /// <summary>
        /// Specifies whether clicking a row selects all its cells
        /// </summary>
        /// <MetaDataID>{8f3965ba-24ae-464a-9c73-2b22725c2b24}</MetaDataID>
        private bool fullRowSelect;

        /// <summary>
        /// Specifies whether the selected rows and cells in the Table remain 
        /// highlighted when the Table loses focus
        /// </summary>
        /// <MetaDataID>{f02964b9-57eb-4ebe-80aa-73d384b85fac}</MetaDataID>
        private bool hideSelection;

        /// <summary>
        /// The background color of selected rows and cells
        /// </summary>
        /// <MetaDataID>{f1892b9b-c526-4314-a56a-24c449f62eba}</MetaDataID>
        private Color selectionBackColor;

        /// <summary>
        /// The foreground color of selected rows and cells
        /// </summary>
        /// <MetaDataID>{88fa7d5c-4912-47d0-a105-b8280b59af11}</MetaDataID>
        private Color selectionForeColor;

        /// <summary>
        /// The background color of selected rows and cells when the Table 
        /// doesn't have focus
        /// </summary>
        /// <MetaDataID>{a3f1b94a-27eb-4d4b-a8af-5216846b5501}</MetaDataID>
        private Color unfocusedSelectionBackColor;

        /// <summary>
        /// The foreground color of selected rows and cells when the Table 
        /// doesn't have focus
        /// </summary>
        /// <MetaDataID>{bcd716b0-3507-42aa-aea1-d19f050fb07b}</MetaDataID>
        private Color unfocusedSelectionForeColor;

        /// <summary>
        /// Determines how selected Cells are hilighted
        /// </summary>
        /// <MetaDataID>{4503286e-5bc5-4a93-8ede-2f77dc09adf3}</MetaDataID>
        private SelectionStyle selectionStyle;

        #endregion

        #region Table

        /// <summary>
        /// The state of the table
        /// </summary>
        /// <MetaDataID>{0b4bd27f-b0d5-480b-8692-e21b2d15a4da}</MetaDataID>
        private TableState tableState;

        /// <summary>
        /// Is the Table currently initialising
        /// </summary>
        /// <MetaDataID>{8a03b9a4-1a6c-4b9a-8b11-c45b39c1925f}</MetaDataID>
        private bool init;

        /// <summary>
        /// The number of times BeginUpdate has been called
        /// </summary>
        /// <MetaDataID>{d91aef68-cc84-4444-89e9-d62dc0a0fed4}</MetaDataID>
        private int beginUpdateCount;

        /// <summary>
        /// The ToolTip used by the Table to display cell and column tooltips
        /// </summary>
        /// <MetaDataID>{35c2feff-4916-4a39-a6b3-11b790be28fd}</MetaDataID>
        private ToolTip toolTip;

        /// <summary>
        /// The alternating row background color
        /// </summary>
        /// <MetaDataID>{1997a398-33a9-409d-a366-2befb083cbb1}</MetaDataID>
        private Color alternatingRowColor;

        /// <summary>
        /// The text displayed in the Table when it has no data to display
        /// </summary>
        /// <MetaDataID>{aac78351-4ebc-4840-a2dd-2b195031e872}</MetaDataID>
        private string noItemsText;

        /// <summary>
        /// Specifies whether the Table is being used as a preview Table 
        /// in a ColumnColection editor
        /// </summary>
        /// <MetaDataID>{ced13d2b-cb72-4f56-b917-2a7706760891}</MetaDataID>
        private bool preview;

        /*/// <summary>
        /// Specifies whether pressing the Tab key while editing moves the 
        /// editor to the next available cell
        /// </summary>
        private bool tabMovesEditor;*/

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Table class with default settings
        /// </summary>
        /// <MetaDataID>{13F8BA10-9A09-4A50-B37C-65A9E13F6291}</MetaDataID>
        public ListView()
        {
            // starting setup
            this.init = true;
            ListConnection = new ListConnection(this);

            // This call is required by the Windows.Forms Form Designer.
            components = new System.ComponentModel.Container();

            //
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;

            this.Size = new Size(150, 150);

            this.BackColor = Color.White;

            //
            this.columnModel = null;
            this.tableModel = null;

            // header
            this.headerStyle = ColumnHeaderStyle.Clickable;
            this.headerFont = this.Font;
            this.headerRenderer = new XPHeaderRenderer();
            //this.headerRenderer = new GradientHeaderRenderer();
            //this.headerRenderer = new FlatHeaderRenderer();
            this.headerRenderer.Font = this.headerFont;
            this.headerContextMenu = new HeaderContextMenu();

            this.columnResizing = true;
            this.resizingColumnIndex = -1;
            this.resizingColumnWidth = -1;
            this.hotColumn = -1;
            this.pressedColumn = -1;
            this.lastSortedColumn = -1;
            this.sortedColumnBackColor = Color.WhiteSmoke;

            // borders
            this.borderStyle = BorderStyle.Fixed3D;

            // scrolling
            this.scrollable = true;

            this.hScrollBar = new HScrollBar();
            this.hScrollBar.Visible = false;
            this.hScrollBar.Location = new Point(this.BorderWidth, this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight);
            this.hScrollBar.Width = this.Width - (this.BorderWidth * 2) - SystemInformation.VerticalScrollBarWidth;
            this.hScrollBar.Scroll += new ScrollEventHandler(this.OnHorizontalScroll);
            this.Controls.Add(this.hScrollBar);

            this.vScrollBar = new VScrollBar();
            this.vScrollBar.Visible = false;
            this.vScrollBar.Location = new Point(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth, this.BorderWidth);
            this.vScrollBar.Height = this.Height - (this.BorderWidth * 2) - SystemInformation.HorizontalScrollBarHeight;
            this.vScrollBar.Scroll += new ScrollEventHandler(this.OnVerticalScroll);
            this.Controls.Add(this.vScrollBar);

            //
            this.gridLines = GridLines.None; ;
            this.gridColor = SystemColors.Control;
            this.gridLineStyle = GridLineStyle.Solid;

            this.allowSelection = true;
            this.multiSelect = false;
            this.fullRowSelect = true;
            this.hideSelection = false;
            this.selectionBackColor = SystemColors.Highlight;
            this.selectionForeColor = SystemColors.HighlightText;
            this.unfocusedSelectionBackColor = SystemColors.Control;
            this.unfocusedSelectionForeColor = SystemColors.ControlText;
            this.selectionStyle = SelectionStyle.ListView;
            this.alternatingRowColor = Color.Transparent;

            // current table state
            this.tableState = TableState.Normal;

            this.lastMouseCell = new CellPos(-1, -1);
            this.lastMouseDownCell = new CellPos(-1, -1);
            this.focusedCell = new CellPos(-1, -1);
            this.hoverTime = 1000;
            this.trackMouseEvent = null;
            this.ResetMouseEventArgs();

            this.toolTip = new ToolTip(this.components);
            this.toolTip.Active = false;
            this.toolTip.InitialDelay = 1000;

            this.noItemsText = "There are no items in this view";

            this.editingCell = new CellPos(-1, -1);
            this.lastEditingCell = this.editingCell;
            this.curentCellEditor = null;
            this.editStartAction = EditStartAction.DoubleClick;
            this.customEditKey = Keys.F5;
            //this.tabMovesEditor = true;

            // finished setting up
            this.beginUpdateCount = 0;
            this.init = false;
            this.preview = false;
            _FocusedBackColor = BackColor;
            MouseDownTimer.Tick += new EventHandler(MouseDownTimerTick);
            base.BackColor = Color.White;
            _EnabledProperty = new DependencyProperty(ListConnection,this, "Enabled");
            ListConnection.AddDependencyProperty(_EnabledProperty);
            
        }


        #endregion

        #region Methods

        #region Coordinate Translation

        #region ClientToDisplayRect

        /// <summary>
        /// Computes the location of the specified client point into coordinates 
        /// relative to the display rectangle
        /// </summary>
        /// <param name="x">The client x coordinate to convert</param>
        /// <param name="y">The client y coordinate to convert</param>
        /// <returns>A Point that represents the converted coordinates (x, y), 
        /// relative to the display rectangle</returns>
        /// <MetaDataID>{F4680C1A-9202-4813-B0A1-C22ABE3CAD79}</MetaDataID>
        public Point ClientToDisplayRect(int x, int y)
        {
            int xPos = x - this.BorderWidth;

            if (this.HScroll)
            {
                xPos += this.hScrollBar.Value;
            }

            int yPos = y - this.BorderWidth;

            if (this.VScroll)
            {
                yPos += this.TopIndex * this.RowHeight;
            }

            return new Point(xPos, yPos);
        }


        /// <summary>
        /// Computes the location of the specified client point into coordinates 
        /// relative to the display rectangle
        /// </summary>
        /// <param name="p">The client coordinate Point to convert</param>
        /// <returns>A Point that represents the converted Point, p, 
        /// relative to the display rectangle</returns>
        /// <MetaDataID>{9091FE51-0A20-442C-B402-6245BCC55E60}</MetaDataID>
        public Point ClientToDisplayRect(Point p)
        {
            return this.ClientToDisplayRect(p.X, p.Y);
        }


        /// <summary>
        /// Converts the location of the specified Rectangle into coordinates 
        /// relative to the display rectangle
        /// </summary>
        /// <param name="rect">The Rectangle to convert whose location is in 
        /// client coordinates</param>
        /// <returns>A Rectangle that represents the converted Rectangle, rect, 
        /// relative to the display rectangle</returns>
        /// <MetaDataID>{20EE23A6-9CAD-4D01-A7F0-2C8EB649AC3E}</MetaDataID>
        public Rectangle ClientToDisplayRect(Rectangle rect)
        {
            return new Rectangle(this.ClientToDisplayRect(rect.Location), rect.Size);
        }

        #endregion

        #region DisplayRectToClient

        /// <summary>
        /// Computes the location of the specified point relative to the display 
        /// rectangle point into client coordinates 
        /// </summary>
        /// <param name="x">The x coordinate to convert relative to the display rectangle</param>
        /// <param name="y">The y coordinate to convert relative to the display rectangle</param>
        /// <returns>A Point that represents the converted coordinates (x, y) relative to 
        /// the display rectangle in client coordinates</returns>
        /// <MetaDataID>{FE28B61F-6BDD-49A9-B75C-89093EBC7F8D}</MetaDataID>
        public Point DisplayRectToClient(int x, int y)
        {
            int xPos = x + this.BorderWidth;

            if (this.HScroll)
            {
                xPos -= this.hScrollBar.Value;
            }

            int yPos = y + this.BorderWidth;

            if (this.VScroll)
            {
                yPos -= this.TopIndex * this.RowHeight;
            }

            return new Point(xPos, yPos);
        }


        /// <summary>
        /// Computes the location of the specified point relative to the display 
        /// rectangle into client coordinates 
        /// </summary>
        /// <param name="p">The point relative to the display rectangle to convert</param>
        /// <returns>A Point that represents the converted Point relative to 
        /// the display rectangle, p, in client coordinates</returns>
        /// <MetaDataID>{EDD4784A-A2EF-4BB7-B4D6-51F64CA76CD3}</MetaDataID>
        public Point DisplayRectToClient(Point p)
        {
            return this.DisplayRectToClient(p.X, p.Y);
        }


        /// <summary>
        /// Converts the location of the specified Rectangle relative to the display 
        /// rectangle into client coordinates 
        /// </summary>
        /// <param name="rect">The Rectangle to convert whose location is relative to 
        /// the display rectangle</param>
        /// <returns>A Rectangle that represents the converted Rectangle relative to 
        /// the display rectangle, rect, in client coordinates</returns>
        /// <MetaDataID>{A59B149F-11EC-4B3A-84D8-9795089AF7BD}</MetaDataID>
        public Rectangle DisplayRectToClient(Rectangle rect)
        {
            return new Rectangle(this.DisplayRectToClient(rect.Location), rect.Size);
        }

        #endregion

        #region Cells

        /// <summary>
        /// Returns the Cell at the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate of the Cell</param>
        /// <param name="y">The client y coordinate of the Cell</param>
        /// <returns>The Cell at the specified client coordinates, or
        /// null if it does not exist</returns>
        /// <MetaDataID>{622458A8-7192-4A6E-ADEC-2FF46A3F9EA3}</MetaDataID>
        public Cell CellAt(int x, int y)
        {
            int row = this.RowIndexAt(x, y);
            int column = this.ColumnIndexAt(x, y);

            // return null if the row or column don't exist
            if (row == -1 || row >= this.TableModel.Rows.Count || column == -1 || column >= this.TableModel.Rows[row].Cells.Count)
            {
                return null;
            }

            return this.TableModel[row, column];
        }


        /// <summary>
        /// Returns the Cell at the specified client Point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>The Cell at the specified client Point, 
        /// or null if not found</returns>
        /// <MetaDataID>{3828568A-272E-48C0-986D-48CD432D1331}</MetaDataID>
        public Cell CellAt(Point p)
        {
            return this.CellAt(p.X, p.Y);
        }


        /// <summary>
        /// Returns a Rectangle that specifies the size and location the cell at 
        /// the specified row and column indexes in client coordinates
        /// </summary>
        /// <param name="row">The index of the row that contains the cell</param>
        /// <param name="column">The index of the column that contains the cell</param>
        /// <returns>A Rectangle that specifies the size and location the cell at 
        /// the specified row and column indexes in client coordinates</returns>
        /// <MetaDataID>{09A4AD98-3EA7-44A2-BBCA-7C680FAFCD0D}</MetaDataID>
        public Rectangle CellRect(int row, int column)
        {
            // return null if the row or column don't exist
            if (row == -1 || row >= this.TableModel.Rows.Count || column == -1 || column >= this.TableModel.Rows[row].Cells.Count)
            {
                return Rectangle.Empty;
            }

            Rectangle columnRect = this.ColumnRect(column);

            if (columnRect == Rectangle.Empty)
            {
                return columnRect;
            }

            Rectangle rowRect = this.RowRect(row);

            if (rowRect == Rectangle.Empty)
            {
                return rowRect;
            }

            return new Rectangle(columnRect.X, rowRect.Y, columnRect.Width, rowRect.Height);
        }


        /// <summary>
        /// Returns a Rectangle that specifies the size and location the cell at 
        /// the specified cell position in client coordinates
        /// </summary>
        /// <param name="cellPos">The position of the cell</param>
        /// <returns>A Rectangle that specifies the size and location the cell at 
        /// the specified cell position in client coordinates</returns>
        /// <MetaDataID>{31BB48C4-E186-4D4F-8C52-B5E04FA14AA8}</MetaDataID>
        public Rectangle CellRect(CellPos cellPos)
        {
            return this.CellRect(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        ///  Returns a Rectangle that specifies the size and location of the 
        ///  specified cell in client coordinates
        /// </summary>
        /// <param name="cell">The cell whose bounding rectangle is to be retrieved</param>
        /// <returns>A Rectangle that specifies the size and location the specified 
        /// cell in client coordinates</returns>
        /// <MetaDataID>{4C5E8CBB-F434-4183-8386-5179DA2B3D2A}</MetaDataID>
        public Rectangle CellRect(Cell cell)
        {
            if (cell == null || cell.Row == null || cell.InternalIndex == -1)
            {
                return Rectangle.Empty;
            }

            if (this.TableModel == null || this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            int row = this.TableModel.Rows.IndexOf(cell.Row);
            int col = cell.InternalIndex;

            return this.CellRect(row, col);
        }


        /// <summary>
        /// Returns whether Cell at the specified row and column indexes 
        /// is not null
        /// </summary>
        /// <param name="row">The row index of the cell</param>
        /// <param name="column">The column index of the cell</param>
        /// <returns>True if the cell at the specified row and column indexes 
        /// is not null, otherwise false</returns>
        /// <MetaDataID>{8EE2846C-7215-45E7-A753-B9D9C90C1BF7}</MetaDataID>
        protected internal bool IsValidCell(int row, int column)
        {
            if (this.TableModel != null && this.ColumnModel != null)
            {
                if (row >= 0 && row < this.TableModel.Rows.Count)
                {
                    if (column >= 0 && column < this.ColumnModel.Columns.Count)
                    {
                        return (this.TableModel.Rows[row].Cells[column] != null);
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Returns whether Cell at the specified cell position is not null
        /// </summary>
        /// <param name="cellPos">The position of the cell</param>
        /// <returns>True if the cell at the specified cell position is not 
        /// null, otherwise false</returns>
        /// <MetaDataID>{2A2B4F6B-2E7D-449E-91E2-C93866C084FD}</MetaDataID>
        protected internal bool IsValidCell(CellPos cellPos)
        {
            return this.IsValidCell(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        /// Returns a CellPos that specifies the next Cell that is visible 
        /// and enabled from the specified Cell
        /// </summary>
        /// <param name="start">A CellPos that specifies the Cell to start 
        /// searching from</param>
        /// <param name="wrap">Specifies whether to move to the start of the 
        /// next Row when the end of the current Row is reached</param>
        /// <param name="forward">Specifies whether the search should travel 
        /// in a forward direction (top to bottom, left to right) through the Cells</param>
        /// <param name="includeStart">Indicates whether the specified starting 
        /// Cell is included in the search</param>
        /// <param name="checkOtherCellsInRow">Specifies whether all Cells in 
        /// the Row should be included in the search</param>
        /// <returns>A CellPos that specifies the next Cell that is visible 
        /// and enabled, or CellPos.Empty if there are no Cells that are visible 
        /// and enabled</returns>
        /// <MetaDataID>{E086300F-29F8-4DB8-883A-15911CD8D786}</MetaDataID>
        protected CellPos FindNextVisibleEnabledCell(CellPos start, bool wrap, bool forward, bool includeStart, bool checkOtherCellsInRow)
        {
            if (this.ColumnCount == 0 || this.RowCount == 0)
            {
                return CellPos.Empty;
            }

            int startRow = start.Row != -1 ? start.Row : 0;
            int startCol = start.Column != -1 ? start.Column : 0;

            bool first = true;

            if (forward)
            {
                for (int i = startRow; i < this.RowCount; i++)
                {
                    int j = (first || !checkOtherCellsInRow ? startCol : 0);

                    for (; j < this.TableModel.Rows[i].Cells.Count; j++)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                if (!checkOtherCellsInRow)
                                {
                                    break;
                                }

                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Enabled && this.ColumnModel.Columns[j].Enabled && this.ColumnModel.Columns[j].Visible)
                        {
                            return new CellPos(i, j);
                        }

                        if (!checkOtherCellsInRow)
                        {
                            continue;
                        }
                    }

                    if (wrap)
                    {
                        if (i + 1 == this.TableModel.Rows.Count)
                        {
                            i = -1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = startRow; i >= 0; i--)
                {
                    int j = (first || !checkOtherCellsInRow ? startCol : this.TableModel.Rows[i].Cells.Count);

                    for (; j >= 0; j--)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                if (!checkOtherCellsInRow)
                                {
                                    break;
                                }

                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Enabled && this.ColumnModel.Columns[j].Enabled && this.ColumnModel.Columns[j].Visible)
                        {
                            return new CellPos(i, j);
                        }

                        if (!checkOtherCellsInRow)
                        {
                            continue;
                        }
                    }

                    if (wrap)
                    {
                        if (i - 1 == -1)
                        {
                            i = this.TableModel.Rows.Count;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return CellPos.Empty;
        }

        /// <summary>
        /// Returns a CellPos that specifies the next Cell that able to be 
        /// edited from the specified Cell
        /// </summary>
        /// <param name="start">A CellPos that specifies the Cell to start 
        /// searching from</param>
        /// <param name="wrap">Specifies whether to move to the start of the 
        /// next Row when the end of the current Row is reached</param>
        /// <param name="forward">Specifies whether the search should travel 
        /// in a forward direction (top to bottom, left to right) through the Cells</param>
        /// <param name="includeStart">Indicates whether the specified starting 
        /// Cell is included in the search</param>
        /// <returns>A CellPos that specifies the next Cell that is able to
        /// be edited, or CellPos.Empty if there are no Cells that editable</returns>
        /// <MetaDataID>{8566C171-3A55-4D82-A450-774B7FCDEB3D}</MetaDataID>
        protected CellPos FindNextEditableCell(CellPos start, bool wrap, bool forward, bool includeStart)
        {
            if (this.ColumnCount == 0 || this.RowCount == 0)
            {
                return CellPos.Empty;
            }

            int startRow = start.Row != -1 ? start.Row : 0;
            int startCol = start.Column != -1 ? start.Column : 0;

            bool first = true;

            if (forward)
            {
                for (int i = startRow; i < this.RowCount; i++)
                {
                    int j = (first ? startCol : 0);

                    for (; j < this.TableModel.Rows[i].Cells.Count; j++)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Editable && this.ColumnModel.Columns[j].Editable)
                        {
                            return new CellPos(i, j);
                        }
                    }

                    if (wrap)
                    {
                        if (i + 1 == this.TableModel.Rows.Count)
                        {
                            i = -1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = startRow; i >= 0; i--)
                {
                    int j = (first ? startCol : this.TableModel.Rows[i].Cells.Count);

                    for (; j >= 0; j--)
                    {
                        if (i == startRow && j == startCol)
                        {
                            if (!first)
                            {
                                return CellPos.Empty;
                            }

                            first = false;

                            if (!includeStart)
                            {
                                continue;
                            }
                        }

                        if (this.IsValidCell(i, j) && this.IsValidColumn(j) && this.TableModel[i, j].Editable && this.ColumnModel.Columns[j].Editable)
                        {
                            return new CellPos(i, j);
                        }
                    }

                    if (wrap)
                    {
                        if (i - 1 == -1)
                        {
                            i = this.TableModel.Rows.Count;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return CellPos.Empty;
        }

        #endregion

        #region Columns

        /// <summary>
        /// Returns the index of the Column at the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate of the Column</param>
        /// <param name="y">The client y coordinate of the Column</param>
        /// <returns>The index of the Column at the specified client coordinates, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{3E026154-BA60-4FE2-AC82-EE377712D28D}</MetaDataID>
        public int ColumnIndexAt(int x, int y)
        {
            if (this.ColumnModel == null)
            {
                return -1;
            }

            // convert to DisplayRect coordinates before 
            // sending to the ColumnModel
            return this.ColumnModel.ColumnIndexAtX(this.hScrollBar.Value + x - this.BorderWidth);
        }


        /// <summary>
        /// Returns the index of the Column at the specified client point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>The index of the Column at the specified client point, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{921E6214-2C2A-4681-8575-21D185ACAFB5}</MetaDataID>
        public int ColumnIndexAt(Point p)
        {
            return this.ColumnIndexAt(p.X, p.Y);
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified 
        /// column's header in client coordinates
        /// </summary>
        /// <param name="column">The index of the column</param>
        /// <returns>The bounding rectangle of the specified 
        /// column's header</returns>
        /// <MetaDataID>{DBCA2C31-A848-4AC9-8596-A6A0859EFFB7}</MetaDataID>
        public Rectangle ColumnHeaderRect(int column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = this.ColumnModel.ColumnHeaderRect(column);

            if (rect == Rectangle.Empty)
            {
                return rect;
            }

            rect.X -= this.hScrollBar.Value - this.BorderWidth;
            rect.Y = this.BorderWidth;

            return rect;
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified 
        /// column's header in client coordinates
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The bounding rectangle of the specified 
        /// column's header</returns>
        /// <MetaDataID>{28BF0218-06C1-4C32-A1BA-1702F4854542}</MetaDataID>
        public Rectangle ColumnHeaderRect(Column column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            return this.ColumnHeaderRect(this.ColumnModel.Columns.IndexOf(column));
        }


        /// <summary>
        /// Returns the bounding rectangle of the column at the 
        /// specified index in client coordinates
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The bounding rectangle of the column at the 
        /// specified index</returns>
        /// <MetaDataID>{6A0B3E89-A89B-4214-A240-05D0B15E0012}</MetaDataID>
        public Rectangle ColumnRect(int column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = this.ColumnHeaderRect(column);

            if (rect == Rectangle.Empty)
            {
                return rect;
            }

            rect.Y += this.HeaderHeight;
            rect.Height = this.TotalRowHeight;

            return rect;
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified column 
        /// in client coordinates
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The bounding rectangle of the specified 
        /// column</returns>
        /// <MetaDataID>{68A72ECC-D157-4CB3-961E-77B4666880B3}</MetaDataID>
        public Rectangle ColumnRect(Column column)
        {
            if (this.ColumnModel == null)
            {
                return Rectangle.Empty;
            }

            return this.ColumnRect(this.ColumnModel.Columns.IndexOf(column));
        }

        #endregion

        #region Rows

        /// <summary>
        /// Returns the index of the Row at the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate of the Row</param>
        /// <param name="y">The client y coordinate of the Row</param>
        /// <returns>The index of the Row at the specified client coordinates, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{2A210CE8-7A9D-4176-9091-52AC02FAC2D7}</MetaDataID>
        public int RowIndexAt(int x, int y)
        {
            if (this.TableModel == null)
            {
                return -1;
            }

            if (this.HeaderStyle != ColumnHeaderStyle.None)
            {
                y -= this.HeaderHeight;
            }

            y -= this.BorderWidth;

            if (y < 0)
            {
                return -1;
            }

            if (this.VScroll)
            {
                y += this.TopIndex * this.RowHeight;
            }

            return this.TableModel.RowIndexAt(y);
        }


        /// <summary>
        /// Returns the index of the Row at the specified client point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>The index of the Row at the specified client point, or
        /// -1 if it does not exist</returns>
        /// <MetaDataID>{64581C9A-CDBB-4015-AB47-BF3EF3F481CC}</MetaDataID>
        public int RowIndexAt(Point p)
        {
            return this.RowIndexAt(p.X, p.Y);
        }


        /// <summary>
        /// Returns the bounding rectangle of the row at the 
        /// specified index in client coordinates
        /// </summary>
        /// <param name="row">The index of the row</param>
        /// <returns>The bounding rectangle of the row at the 
        /// specified index</returns>
        /// <MetaDataID>{1D7D17B7-6AE3-4727-98A4-4F057213340B}</MetaDataID>
        public Rectangle RowRect(int row)
        {
            if (this.TableModel == null || this.ColumnModel == null || row == -1 || row > this.TableModel.Rows.Count)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = new Rectangle();

            rect.X = this.DisplayRectangle.X;
            rect.Y = this.BorderWidth + ((row - this.TopIndex) * this.RowHeight);

            rect.Width = this.ColumnModel.VisibleColumnsWidth;
            rect.Height = this.RowHeight;

            if (this.HeaderStyle != ColumnHeaderStyle.None)
            {
                rect.Y += this.HeaderHeight;
            }

            return rect;
        }


        /// <summary>
        /// Returns the bounding rectangle of the specified row 
        /// in client coordinates
        /// </summary>
        /// <param name="row">The row</param>
        /// <returns>The bounding rectangle of the specified 
        /// row</returns>
        /// <MetaDataID>{6600F62C-8819-4554-96FB-8524C2537504}</MetaDataID>
        public Rectangle RowRect(Row row)
        {
            if (this.TableModel == null)
            {
                return Rectangle.Empty;
            }

            return this.RowRect(this.TableModel.Rows.IndexOf(row));
        }

        #endregion

        #region Hit Tests

        /// <summary>
        /// Returns a TableRegions value that represents the table region at 
        /// the specified client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate</param>
        /// <param name="y">The client y coordinate</param>
        /// <returns>A TableRegions value that represents the table region at 
        /// the specified client coordinates</returns>
        /// <MetaDataID>{6391A3B9-34B5-4FDE-B206-20667CF201B5}</MetaDataID>
        public TableRegion HitTest(int x, int y)
        {
            if (this.HeaderStyle != ColumnHeaderStyle.None && this.HeaderRectangle.Contains(x, y))
            {
                return TableRegion.ColumnHeader;
            }
            else if (this.CellDataRect.Contains(x, y))
            {
                return TableRegion.Cells;
            }
            else if (!this.Bounds.Contains(x, y))
            {
                return TableRegion.NoWhere;
            }

            return TableRegion.NonClientArea;
        }


        /// <summary>
        /// Returns a TableRegions value that represents the table region at 
        /// the specified client point
        /// </summary>
        /// <param name="p">The point of interest</param>
        /// <returns>A TableRegions value that represents the table region at 
        /// the specified client point</returns>
        /// <MetaDataID>{096F6E8D-67A3-4380-9AB8-A9E08B47AA08}</MetaDataID>
        public TableRegion HitTest(Point p)
        {
            return this.HitTest(p.X, p.Y);
        }

        #endregion

        #endregion

        #region Dispose

        /// <summary>
        /// Releases the unmanaged resources used by the Control and optionally 
        /// releases the managed resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged 
        /// resources; false to release only unmanaged resources</param>
        /// <MetaDataID>{76286D37-0EE2-4A87-93DD-9267B1DD5A9E}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Removes the ColumnModel and TableModel from the Table
        /// </summary>
        /// <MetaDataID>{480ED4F9-3163-4BEA-9496-650FD9450E77}</MetaDataID>
        public void Clear()
        {
            if (this.ColumnModel != null)
            {
                this.ColumnModel = null;
            }

            if (this.TableModel != null)
            {
                this.TableModel = null;
            }
        }

        #endregion

        #region Editing

        /// <summary>
        /// Records the Cell that is currently being edited and the 
        /// ICellEditor used to edit the Cell
        /// </summary>
        /// <param name="cell">The Cell that is currently being edited</param>
        /// <param name="editor">The ICellEditor used to edit the Cell</param>
        /// <MetaDataID>{7B86B36D-20D9-47AD-8528-B2BC9EF31175}</MetaDataID>
        private void SetEditingCell(Cell cell, ICellEditor editor)
        {
            this.SetEditingCell(new CellPos(cell.Row.InternalIndex, cell.InternalIndex), editor);
        }


        /// <summary>
        /// Records the Cell that is currently being edited and the 
        /// ICellEditor used to edit the Cell
        /// </summary>
        /// <param name="cellPos">The Cell that is currently being edited</param>
        /// <param name="editor">The ICellEditor used to edit the Cell</param>
        /// <MetaDataID>{CB3C35F5-BCDA-4A4F-917D-2BC1F3818480}</MetaDataID>
        private void SetEditingCell(CellPos cellPos, ICellEditor editor)
        {
            this.editingCell = cellPos;
            this.curentCellEditor = editor;

        }


        /// <summary>
        /// Starts editing the Cell at the specified row and column indexes
        /// </summary>
        /// <param name="row">The row index of the Cell to be edited</param>
        /// <param name="column">The column index of the Cell to be edited</param>
        /// <MetaDataID>{5605B0E2-A52D-46C0-A340-070CF4552A50}</MetaDataID>
        public void EditCell(int row, int column)
        {
            this.EditCell(new CellPos(row, column));
        }


        /// <summary>
        /// Starts editing the Cell at the specified CellPos
        /// </summary>
        /// <param name="cellPos">A CellPos that specifies the Cell to be edited</param>
        /// <MetaDataID>{A1DCE0F2-8A52-4403-8162-3D5D10EA0B53}</MetaDataID>
        public void EditCell(CellPos cellPos)
        {
            // don't bother if the cell doesn't exists or the cell's
            // column is not visible or the cell is not editable
            if (!this.IsValidCell(cellPos) || !this.ColumnModel.Columns[cellPos.Column].Visible || !this.IsCellEditable(cellPos))
            {
                return;
            }

            // check if we're currently editing a cell
            if (this.EditingCell != CellPos.Empty)
            {
                // don't bother if we're already editing the cell.  
                // if we're editing a different cell stop editing
                if (this.EditingCell == cellPos)
                {
                    return;
                }
                else
                {
                    this.EditingCellEditor.StopEditing();
                }
            }
            if (this.EnsureVisible(cellPos))
            {
                this.Refresh();
            }

            Cell cell = this.TableModel[cellPos];
            ICellEditor editor = this.ColumnModel.GetCellEditor(cellPos.Column);

            // make sure we have an editor and that the cell 
            // and the cell's column are editable
            if (editor == null || !cell.Editable || !this.ColumnModel.Columns[cellPos.Column].Editable)
            {
                return;
            }




            Rectangle cellRect = this.CellRect(cellPos);

            // give anyone subscribed to the table's BeginEditing
            // event the first chance to cancel editing
            CellEditEventArgs e = new CellEditEventArgs(cell, editor, this, cellPos.Row, cellPos.Column, cellRect);

            this.OnBeginEditing(e);


            //
            if (!e.Cancel)
            {
                // get the editor ready for editing.  if PrepareForEditing
                // returns false, someone who subscribed to the editors 
                // BeginEdit event has cancelled editing
                if (!editor.PrepareForEditing(cell, this, cellPos, cellRect, e.Handled))
                {
                    return;
                }

                // keep track of the editing cell and editor 
                // and start editing
                this.editingCell = cellPos;
                this.curentCellEditor = editor;
                int vScrollBarValue = this.vScrollBar.Value;
                editor.StartEditing();
                this.vScrollBar.Value = vScrollBarValue;
                lastEditingCell = cellPos;

            }
        }


        /*/// <summary>
        /// Stops editing the current Cell and starts editing the next editable Cell
        /// </summary>
        /// <param name="forwards">Specifies whether the editor should traverse 
        /// forward when looking for the next editable Cell</param>
        protected internal void EditNextCell(bool forwards)
        {
            if (this.EditingCell == CellPos.Empty)
            {
                return;
            }
				
            CellPos nextCell = this.FindNextEditableCell(this.FocusedCell, true, forwards, false);

            if (nextCell != CellPos.Empty && nextCell != this.EditingCell)
            {
                this.StopEditing();

                this.EditCell(nextCell);
            }
        }*/



        /// <summary>
        /// Stops editing the current Cell and commits any changes
        /// </summary>
        /// <MetaDataID>{AB130550-677D-4139-BEE7-607D71F62096}</MetaDataID>
        public void StopEditing()
        {
            // don't bother if we're not editing
            if (this.EditingCell == CellPos.Empty)
            {
                return;
            }

            this.EditingCellEditor.StopEditing();

            this.Invalidate(this.RowRect(this.editingCell.Row));
            this.lastEditingCell = this.editingCell;
            this.editingCell = CellPos.Empty;
            this.curentCellEditor = null;

        }


        /// <summary>
        /// Cancels editing the current Cell and ignores any changes
        /// </summary>
        /// <MetaDataID>{8DA0CB01-4759-4F04-9C1C-E81EE218B7F5}</MetaDataID>
        public void CancelEditing()
        {
            // don't bother if we're not editing
            if (this.EditingCell == CellPos.Empty)
            {
                return;
            }

            this.EditingCellEditor.CancelEditing();

            this.editingCell = CellPos.Empty;
            this.curentCellEditor = null;
            this.lastEditingCell = CellPos.Empty;
        }


        /// <summary>
        /// Returns whether the Cell at the specified row and column is able 
        /// to be edited by the user
        /// </summary>
        /// <param name="row">The row index of the Cell to check</param>
        /// <param name="column">The column index of the Cell to check</param>
        /// <returns>True if the Cell at the specified row and column is able 
        /// to be edited by the user, false otherwise</returns>
        /// <MetaDataID>{3C37B325-92DA-4BBC-828C-F92884D6D89F}</MetaDataID>
        public bool IsCellEditable(int row, int column)
        {
            return this.IsCellEditable(new CellPos(row, column));
        }


        /// <summary>
        /// Returns whether the Cell at the specified CellPos is able 
        /// to be edited by the user
        /// </summary>
        /// <param name="cellpos">A CellPos that specifies the Cell to check</param>
        /// <returns>True if the Cell at the specified CellPos is able 
        /// to be edited by the user, false otherwise</returns>
        /// <MetaDataID>{5425C2ED-F9C8-4127-8349-BE8C71B9810A}</MetaDataID>
        public bool IsCellEditable(CellPos cellpos)
        {
            // don't bother if the cell doesn't exists or the cell's
            // column is not visible
            if (!this.IsValidCell(cellpos) || !this.ColumnModel.Columns[cellpos.Column].Visible)
            {
                return false;
            }

            return (this.TableModel[cellpos].Editable &&
                this.ColumnModel.Columns[cellpos.Column].Editable);
        }


        /// <summary>
        /// Returns whether the Cell at the specified row and column is able 
        /// to respond to user interaction
        /// </summary>
        /// <param name="row">The row index of the Cell to check</param>
        /// <param name="column">The column index of the Cell to check</param>
        /// <returns>True if the Cell at the specified row and column is able 
        /// to respond to user interaction, false otherwise</returns>
        /// <MetaDataID>{5A9E0D6E-5EF3-4F93-88B2-51524E358DD8}</MetaDataID>
        public bool IsCellEnabled(int row, int column)
        {
            return this.IsCellEnabled(new CellPos(row, column));
        }


        /// <summary>
        /// Returns whether the Cell at the specified CellPos is able 
        /// to respond to user interaction
        /// </summary>
        /// <param name="cellpos">A CellPos that specifies the Cell to check</param>
        /// <returns>True if the Cell at the specified CellPos is able 
        /// to respond to user interaction, false otherwise</returns>
        /// <MetaDataID>{867F0780-65EA-4466-87AA-263030C06905}</MetaDataID>
        public bool IsCellEnabled(CellPos cellpos)
        {
            // don't bother if the cell doesn't exists or the cell's
            // column is not visible
            if (!this.IsValidCell(cellpos) || !this.ColumnModel.Columns[cellpos.Column].Visible)
            {
                return false;
            }

            return (this.TableModel[cellpos].Enabled &&
                this.ColumnModel.Columns[cellpos.Column].Enabled);
        }

        #endregion

        #region Invalidate

        /// <summary>
        /// Invalidates the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to be invalidated</param>
        /// <MetaDataID>{B07D2099-430C-46B9-AFD5-C2A895C7B152}</MetaDataID>
        public void InvalidateCell(Cell cell)
        {
            this.InvalidateCell(cell.Row.Index, cell.Index);
        }


        /// <summary>
        /// Invalidates the Cell located at the specified row and column indicies
        /// </summary>
        /// <param name="row">The row index of the Cell to be invalidated</param>
        /// <param name="column">The column index of the Cell to be invalidated</param>
        /// <MetaDataID>{5453565C-B895-4A29-8ACA-4C0295AD6A43}</MetaDataID>
        public void InvalidateCell(int row, int column)
        {
            Rectangle cellRect = this.CellRect(row, column);

            if (cellRect == Rectangle.Empty)
            {
                return;
            }

            if (cellRect.IntersectsWith(this.CellDataRect))
            {
                this.Invalidate(Rectangle.Intersect(this.CellDataRect, cellRect), false);
            }
        }


        /// <summary>
        /// Invalidates the Cell located at the specified CellPos
        /// </summary>
        /// <param name="cellPos">A CellPos that specifies the Cell to be invalidated</param>
        /// <MetaDataID>{ADEBBD00-5315-4E77-803D-B2AD258E80D1}</MetaDataID>
        public void InvalidateCell(CellPos cellPos)
        {
            this.InvalidateCell(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        /// Invalidates the specified Row
        /// </summary>
        /// <param name="row">The Row to be invalidated</param>
        /// <MetaDataID>{4C538CD3-5D5D-4153-AC5A-37074F88FF34}</MetaDataID>
        public void InvalidateRow(Row row)
        {
            this.InvalidateRow(row.Index);
        }


        /// <summary>
        /// Invalidates the Row located at the specified row index
        /// </summary>
        /// <param name="row">The row index of the Row to be invalidated</param>
        /// <MetaDataID>{681EBBED-AEFA-488D-AF78-A15AC119DA38}</MetaDataID>
        public void InvalidateRow(int row)
        {
            Rectangle rowRect = this.RowRect(row);

            if (rowRect == Rectangle.Empty)
            {
                return;
            }

            if (rowRect.IntersectsWith(this.CellDataRect))
            {
                this.Invalidate(Rectangle.Intersect(this.CellDataRect, rowRect), false);
            }
        }


        /// <summary>
        /// Invalidates the Row located at the specified CellPos
        /// </summary>
        /// <param name="cellPos">A CellPos that specifies the Row to be invalidated</param>
        /// <MetaDataID>{32F7FAD9-4FA6-4278-94C1-975C8C1BE949}</MetaDataID>
        public void InvalidateRow(CellPos cellPos)
        {
            this.InvalidateRow(cellPos.Row);
        }

        #endregion

        #region Keys

        /// <summary>
        /// Determines whether the specified key is reserved for use by the Table
        /// </summary>
        /// <param name="key">One of the Keys values</param>
        /// <returns>true if the specified key is reserved for use by the Table; 
        /// otherwise, false</returns>
        /// <MetaDataID>{0A7EE5D6-DA0E-4D0D-AD2D-1E439CD04B25}</MetaDataID>
        protected internal bool IsReservedKey(Keys key)
        {
            if ((key & Keys.Alt) != Keys.Alt)
            {
                Keys k = key & Keys.KeyCode;

                return (k == Keys.Up ||
                    k == Keys.Down ||
                    k == Keys.Left ||
                    k == Keys.Right ||
                    k == Keys.PageUp ||
                    k == Keys.PageDown ||
                    k == Keys.Home ||
                    k == Keys.End ||
                    k == Keys.Tab ||
                    k == Keys.Insert ||
                    k == Keys.Delete);
            }

            return false;
        }


        /// <summary>
        /// Determines whether the specified key is a regular input key or a special 
        /// key that requires preprocessing
        /// </summary>
        /// <param name="keyData">One of the Keys values</param>
        /// <returns>true if the specified key is a regular input key; otherwise, false</returns>
        /// <MetaDataID>{4B1F0419-FE9A-401A-B6E6-C5470C945993}</MetaDataID>
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) != Keys.Alt)
            {
                Keys key = keyData & Keys.KeyCode;

                switch (key)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Prior:
                    case Keys.Next:
                    case Keys.End:
                    case Keys.Home:
                        {
                            return true;
                        }
                }

                if (base.IsInputKey(keyData))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Layout

        /// <summary>
        /// Prevents the Table from drawing until the EndUpdate method is called
        /// </summary>
        /// <MetaDataID>{9E9FE3D4-BB02-4693-8A57-5AB380E81E61}</MetaDataID>
        public void BeginUpdate()
        {
            if (this.IsHandleCreated)
            {
                if (this.beginUpdateCount == 0)
                {
                    NativeMethods.SendMessage(this.Handle, 11, 0, 0);
                }

                this.beginUpdateCount++;
            }
        }


        /// <summary>
        /// Resumes drawing of the Table after drawing is suspended by the 
        /// BeginUpdate method
        /// </summary>
        /// <MetaDataID>{D09161ED-D0DA-47C5-AEE5-83669E8258D0}</MetaDataID>
        public void EndUpdate()
        {
            if (this.beginUpdateCount <= 0)
            {
                return;
            }

            this.beginUpdateCount--;

            if (this.beginUpdateCount == 0)
            {
                NativeMethods.SendMessage(this.Handle, 11, -1, 0);

                this.PerformLayout();
                this.Invalidate(true);
            }
        }


        /// <summary>
        /// Signals the object that initialization is starting
        /// </summary>
        /// <MetaDataID>{D5198D05-4E3D-4A36-A64D-12F0B4EE0618}</MetaDataID>
        public void BeginInit()
        {
            this.init = true;
        }


        /// <summary>
        /// Signals the object that initialization is complete
        /// </summary>
        /// <MetaDataID>{0111BEFC-2D09-48BE-90EA-40A5F85831F7}</MetaDataID>
        public void EndInit()
        {
            this.init = false;

            this.PerformLayout();
        }


        /// <summary>
        /// Gets whether the Table is currently initializing
        /// </summary>
        /// <MetaDataID>{36b630ee-ddc0-43dd-ba1a-637f7511293a}</MetaDataID>
        [Browsable(false)]
        public bool Initializing
        {
            get
            {
                return this.init;
            }
        }

        #endregion

        #region Mouse

        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code
        /// </summary>
        /// <MetaDataID>{BA0EA68A-FCDD-4523-B35B-ED04B7D2A1D3}</MetaDataID>
        public new void ResetMouseEventArgs()
        {
            if (this.trackMouseEvent == null)
            {
                this.trackMouseEvent = new TRACKMOUSEEVENT();
                this.trackMouseEvent.dwFlags = 3;
                this.trackMouseEvent.hwndTrack = base.Handle;
            }

            this.trackMouseEvent.dwHoverTime = this.HoverTime;

            NativeMethods.TrackMouseEvent(this.trackMouseEvent);
        }

        #endregion

        #region Scrolling

        /// <summary>
        /// Updates the scrollbars to reflect any changes made to the Table
        /// </summary>
        /// <MetaDataID>{0ED6C54C-8E3A-4F6E-B16D-D12DF8B989D3}</MetaDataID>
        public void UpdateScrollBars()
        {
            if (!this.Scrollable || this.ColumnModel == null)
            {
                return;
            }

            // fix: Add width/height check as otherwise minimize 
            //      causes a crash
            //      Portia4ever (kangxj@126.com)
            //      13/09/2005
            //      v1.0.1
            if (this.Width == 0 || this.Height == 0)
            {
                return;
            }

            bool hscroll = (this.ColumnModel.VisibleColumnsWidth > this.Width - (this.BorderWidth * 2));
            bool vscroll = this.TotalRowAndHeaderHeight > (this.Height - (this.BorderWidth * 2) - (hscroll ? SystemInformation.HorizontalScrollBarHeight : 0));

            if (vscroll)
            {
                hscroll = (this.ColumnModel.VisibleColumnsWidth > this.Width - (this.BorderWidth * 2) - SystemInformation.VerticalScrollBarWidth);
            }

            if (hscroll)
            {
                Rectangle hscrollBounds = new Rectangle(this.BorderWidth,
                    this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight,
                    this.Width - (this.BorderWidth * 2),
                    SystemInformation.HorizontalScrollBarHeight);

                if (vscroll)
                {
                    hscrollBounds.Width -= SystemInformation.VerticalScrollBarWidth;
                }

                this.hScrollBar.Visible = true;
                this.hScrollBar.Bounds = hscrollBounds;
                this.hScrollBar.Minimum = 0;
                this.hScrollBar.Maximum = this.ColumnModel.VisibleColumnsWidth;
                this.hScrollBar.SmallChange = Column.MinimumWidth;
                if (hscrollBounds.Width - 1 < 0)
                    this.hScrollBar.LargeChange = 0;
                else
                    this.hScrollBar.LargeChange = hscrollBounds.Width - 1;

                if (this.hScrollBar.Value > this.hScrollBar.Maximum - this.hScrollBar.LargeChange)
                {
                    this.hScrollBar.Value = this.hScrollBar.Maximum - this.hScrollBar.LargeChange;
                }
            }
            else
            {
                this.hScrollBar.Visible = false;
                this.hScrollBar.Value = 0;
            }

            if (vscroll)
            {
                Rectangle vscrollBounds = new Rectangle(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth,
                    this.BorderWidth,
                    SystemInformation.VerticalScrollBarWidth,
                    this.Height - (this.BorderWidth * 2));

                if (hscroll)
                {
                    vscrollBounds.Height -= SystemInformation.HorizontalScrollBarHeight;
                }

                this.vScrollBar.Visible = true;
                this.vScrollBar.Bounds = vscrollBounds;
                this.vScrollBar.Minimum = 0;
                if (RowCount > 0 && VisibleRowCount > 0)
                    this.vScrollBar.Maximum = (this.RowCount > this.VisibleRowCount ? this.RowCount - 1 : this.VisibleRowCount);
                this.vScrollBar.SmallChange = 1;

                if (VisibleRowCount > 0)
                    this.vScrollBar.LargeChange = this.VisibleRowCount - 1;

                if (this.vScrollBar.Value > this.vScrollBar.Maximum - this.vScrollBar.LargeChange)
                {
                    this.vScrollBar.Value = this.vScrollBar.Maximum - this.vScrollBar.LargeChange;
                }
            }
            else
            {
                this.vScrollBar.Visible = false;
                this.vScrollBar.Value = 0;
            }
        }


        /// <summary>
        /// Scrolls the contents of the Table horizontally to the specified value
        /// </summary>
        /// <param name="value">The value to scroll to</param>
        /// <MetaDataID>{0DE30BDF-CC7C-4DAE-A653-0A0A70E9BB33}</MetaDataID>
        protected void HorizontalScroll(int value)
        {
            int scrollVal = this.hScrollBar.Value - value;

            if (scrollVal != 0)
            {
                RECT scrollRect = RECT.FromRectangle(this.PseudoClientRect);
                Rectangle invalidateRect = scrollRect.ToRectangle();

                NativeMethods.ScrollWindow(this.Handle, scrollVal, 0, ref scrollRect, ref scrollRect);

                if (scrollVal < 0)
                {
                    invalidateRect.X = invalidateRect.Right + scrollVal;
                }

                invalidateRect.Width = Math.Abs(scrollVal);

                this.Invalidate(invalidateRect, false);

                if (this.VScroll)
                {
                    this.Invalidate(new Rectangle(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth,
                        this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight,
                        SystemInformation.VerticalScrollBarWidth,
                        SystemInformation.HorizontalScrollBarHeight),
                        false);
                }
            }
        }


        /// <summary>
        /// Scrolls the contents of the Table vertically to the specified value
        /// </summary>
        /// <param name="value">The value to scroll to</param>
        /// <MetaDataID>{51FA14EB-BC2D-4182-850F-59E618200CA5}</MetaDataID>
        protected void VerticalScroll(int value)
        {
            int scrollVal = this.vScrollBar.Value - value;

            if (scrollVal != 0)
            {
                if (this.IsEditing)
                {
                    this.StopEditing();
                }

                RECT scrollRect = RECT.FromRectangle(this.CellDataRect);

                Rectangle invalidateRect = scrollRect.ToRectangle();

                scrollVal *= this.RowHeight;

                NativeMethods.ScrollWindow(this.Handle, 0, scrollVal, ref scrollRect, ref scrollRect);

                if (scrollVal < 0)
                {
                    invalidateRect.Y = invalidateRect.Bottom + scrollVal;
                }

                invalidateRect.Height = Math.Abs(scrollVal);

                this.Invalidate(invalidateRect, false);

                if (this.HScroll)
                {
                    this.Invalidate(new Rectangle(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth,
                        this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight,
                        SystemInformation.VerticalScrollBarWidth,
                        SystemInformation.HorizontalScrollBarHeight),
                        false);
                }
            }
        }


        /// <summary>
        /// Ensures that the Cell at the specified row and column is visible 
        /// within the Table, scrolling the contents of the Table if necessary
        /// </summary>
        /// <param name="row">The zero-based index of the row to scroll into view</param>
        /// <param name="column">The zero-based index of the column to scroll into view</param>
        /// <returns>true if the Table scrolled to the Cell at the specified row 
        /// and column, false otherwise</returns>
        /// <MetaDataID>{91BB0FFE-3B54-4D25-875C-6F0890F05691}</MetaDataID>
        public bool EnsureVisible(int row, int column)
        {
            if (!this.Scrollable || (!this.HScroll && !this.VScroll) || row == -1)
            {
                return false;
            }
            //   return false;

            if (column == -1)
            {
                if (this.FocusedCell.Column != -1)
                {
                    column = this.FocusedCell.Column;
                }
                else
                {
                    column = 0;
                }
            }

            int hscrollVal = this.hScrollBar.Value;
            int vscrollVal = this.vScrollBar.Value;
            bool moved = false;

            if (this.HScroll)
            {
                if (column < 0)
                {
                    column = 0;
                }
                else if (column >= this.ColumnCount)
                {
                    column = this.ColumnCount - 1;
                }

                if (this.ColumnModel.Columns[column].Visible)
                {
                    if (this.ColumnModel.Columns[column].Left < this.hScrollBar.Value)
                    {
                        hscrollVal = this.ColumnModel.Columns[column].Left;
                    }
                    else if (this.ColumnModel.Columns[column].Right > this.hScrollBar.Value + this.CellDataRect.Width)
                    {
                        hscrollVal = this.ColumnModel.Columns[column].Right - this.CellDataRect.Width;
                    }

                    if (hscrollVal > this.hScrollBar.Maximum - this.hScrollBar.LargeChange)
                    {
                        hscrollVal = this.hScrollBar.Maximum - this.hScrollBar.LargeChange;
                    }
                }
            }

            if (this.VScroll)
            {
                if (row < 0)
                {
                    vscrollVal = 0;
                }
                else if (row >= this.RowCount)
                {
                    vscrollVal = this.RowCount - 1;
                }
                else
                {
                    if (row < vscrollVal)
                    {
                        vscrollVal = row;
                    }
                    else if (row > vscrollVal + this.vScrollBar.LargeChange)
                    {
                        vscrollVal += row - (vscrollVal + this.vScrollBar.LargeChange);
                    }
                }

                if (vscrollVal > this.vScrollBar.Maximum - this.vScrollBar.LargeChange)
                {
                    vscrollVal = (this.vScrollBar.Maximum - this.vScrollBar.LargeChange) + 1;
                }
            }

            if (this.RowRect(row).Bottom > this.CellDataRect.Bottom)
            {
                vscrollVal++;
            }

            moved = (this.hScrollBar.Value != hscrollVal || this.vScrollBar.Value != vscrollVal);

            if (moved)
            {
                this.hScrollBar.Value = hscrollVal;
                this.vScrollBar.Value = vscrollVal;


                this.Invalidate(this.PseudoClientRect);
            }

            return moved;
        }


        /// <summary>
        /// Ensures that the Cell at the specified CellPos is visible within 
        /// the Table, scrolling the contents of the Table if necessary
        /// </summary>
        /// <param name="cellPos">A CellPos that contains the zero-based index 
        /// of the row and column to scroll into view</param>
        /// <returns></returns>
        /// <MetaDataID>{140C416E-7E16-4A32-B63D-3C8798E284C7}</MetaDataID>
        public bool EnsureVisible(CellPos cellPos)
        {
            //return true;
            return this.EnsureVisible(cellPos.Row, cellPos.Column);
        }


        /// <summary>
        /// Gets the index of the first visible Column currently displayed in the Table
        /// </summary>
        /// <MetaDataID>{5bf1fb21-da3a-488f-84cb-0e5612d7b773}</MetaDataID>
        [Browsable(false)]
        public int FirstVisibleColumn
        {
            get
            {
                if (this.ColumnModel == null || this.ColumnModel.VisibleColumnCount == 0)
                {
                    return -1;
                }

                return this.ColumnModel.ColumnIndexAtX(this.hScrollBar.Value);
            }
        }


        /// <summary>
        /// Gets the index of the last visible Column currently displayed in the Table
        /// </summary>
        /// <MetaDataID>{281d174c-e07b-4d53-9c9f-88f6f9163e26}</MetaDataID>
        [Browsable(false)]
        public int LastVisibleColumn
        {
            get
            {
                if (this.ColumnModel == null || this.ColumnModel.VisibleColumnCount == 0)
                {
                    return -1;
                }

                int rightEdge = this.hScrollBar.Value + this.PseudoClientRect.Right;

                if (this.VScroll)
                {
                    rightEdge -= this.vScrollBar.Width;
                }

                int col = this.ColumnModel.ColumnIndexAtX(rightEdge);

                if (col == -1)
                {
                    return this.ColumnModel.PreviousVisibleColumn(this.ColumnModel.Columns.Count);
                }
                else if (!this.ColumnModel.Columns[col].Visible)
                {
                    return this.ColumnModel.PreviousVisibleColumn(col);
                }

                return col;
            }
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Sorts the last sorted column opposite to its current sort order, 
        /// or sorts the currently focused column in ascending order if no 
        /// columns have been sorted
        /// </summary>
        /// <MetaDataID>{4F9E0A9D-61BE-44F9-9895-09DA291A9370}</MetaDataID>
        public void Sort()
        {
            this.Sort(true);
        }


        /// <summary>
        /// Sorts the last sorted column opposite to its current sort order, 
        /// or sorts the currently focused column in ascending order if no 
        /// columns have been sorted
        /// </summary>
        /// <param name="stable">Specifies whether a stable sorting method 
        /// should be used to sort the column</param>
        /// <MetaDataID>{61C741F1-D4AB-4C44-A0E5-97F8A7D5E6C2}</MetaDataID>
        public void Sort(bool stable)
        {
            // don't allow sorting if we're being used as a 
            // preview table in a ColumnModel editor
            if (this.Preview)
            {
                return;
            }

            // if we don't have a sorted column already, check if 
            // we can use the column of the cell that has focus
            if (!this.IsValidColumn(this.lastSortedColumn))
            {
                if (this.IsValidColumn(this.focusedCell.Column))
                {
                    this.lastSortedColumn = this.focusedCell.Column;
                }
            }

            // make sure the last sorted column exists
            if (this.IsValidColumn(this.lastSortedColumn))
            {
                // don't bother if the column won't let us sort
                if (!this.ColumnModel.Columns[this.lastSortedColumn].Sortable)
                {
                    return;
                }

                // work out which direction we should sort
                SortOrder newOrder = SortOrder.Ascending;

                Column column = this.ColumnModel.Columns[this.lastSortedColumn];

                if (column.SortOrder == SortOrder.Ascending)
                {
                    newOrder = SortOrder.Descending;
                }

                this.Sort(this.lastSortedColumn, column, newOrder, stable);
            }
        }


        /// <summary>
        /// Sorts the specified column opposite to its current sort order, 
        /// or in ascending order if the column is not sorted
        /// </summary>
        /// <param name="column">The index of the column to sort</param>
        /// <MetaDataID>{184CCD6C-D2BE-4681-820A-086DDDEE1DAA}</MetaDataID>
        public void Sort(int column)
        {
            this.Sort(column, true);
        }


        /// <summary>
        /// Sorts the specified column opposite to its current sort order, 
        /// or in ascending order if the column is not sorted
        /// </summary>
        /// <param name="column">The index of the column to sort</param>
        /// <param name="stable">Specifies whether a stable sorting method 
        /// should be used to sort the column</param>
        /// <MetaDataID>{864B9F65-1F8A-4C06-A877-BFF3C43415E3}</MetaDataID>
        public void Sort(int column, bool stable)
        {
            // don't allow sorting if we're being used as a 
            // preview table in a ColumnModel editor
            if (this.Preview)
            {
                return;
            }

            // make sure the column exists
            if (this.IsValidColumn(column))
            {
                // don't bother if the column won't let us sort
                if (!this.ColumnModel.Columns[column].Sortable)
                {
                    return;
                }

                // if we already have a different sorted column, set 
                // its sort order to none
                if (column != this.lastSortedColumn)
                {
                    if (this.IsValidColumn(this.lastSortedColumn))
                    {
                        this.ColumnModel.Columns[this.lastSortedColumn].InternalSortOrder = SortOrder.None;
                    }
                }

                this.lastSortedColumn = column;

                // work out which direction we should sort
                SortOrder newOrder = SortOrder.Ascending;

                Column col = this.ColumnModel.Columns[column];

                if (col.SortOrder == SortOrder.Ascending)
                {
                    newOrder = SortOrder.Descending;
                }

                this.Sort(column, col, newOrder, stable);
            }
        }


        /// <summary>
        /// Sorts the specified column in the specified sort direction
        /// </summary>
        /// <param name="column">The index of the column to sort</param>
        /// <param name="sortOrder">The direction the column is to be sorted</param>
        /// <MetaDataID>{6DACE91E-6676-4336-844E-8CB81183A204}</MetaDataID>
        public void Sort(int column, SortOrder sortOrder)
        {
            this.Sort(column, sortOrder, true);
        }


        /// <summary>
        /// Sorts the specified column in the specified sort direction
        /// </summary>
        /// <param name="column">The index of the column to sort</param>
        /// <param name="sortOrder">The direction the column is to be sorted</param>
        /// <param name="stable">Specifies whether a stable sorting method 
        /// should be used to sort the column</param>
        /// <MetaDataID>{729B2136-3D5F-4356-9352-47A3CD7277F1}</MetaDataID>
        public void Sort(int column, SortOrder sortOrder, bool stable)
        {
            // don't allow sorting if we're being used as a 
            // preview table in a ColumnModel editor
            if (this.Preview)
            {
                return;
            }

            // make sure the column exists
            if (this.IsValidColumn(column))
            {
                // don't bother if the column won't let us sort
                if (!this.ColumnModel.Columns[column].Sortable)
                {
                    return;
                }

                // if we already have a different sorted column, set 
                // its sort order to none
                if (column != this.lastSortedColumn)
                {
                    if (this.IsValidColumn(this.lastSortedColumn))
                    {
                        this.ColumnModel.Columns[this.lastSortedColumn].InternalSortOrder = SortOrder.None;
                    }
                }

                this.lastSortedColumn = column;

                this.Sort(column, this.ColumnModel.Columns[column], sortOrder, stable);
            }
        }


        /// <summary>
        /// Sorts the specified column in the specified sort direction
        /// </summary>
        /// <param name="index">The index of the column to sort</param>
        /// <param name="column">The column to sort</param>
        /// <param name="sortOrder">The direction the column is to be sorted</param>
        /// <param name="stable">Specifies whether a stable sorting method 
        /// should be used to sort the column</param>
        /// <MetaDataID>{3EBEDD04-7DCC-40C3-941B-F4AEDCB93894}</MetaDataID>
        private void Sort(int index, Column column, SortOrder sortOrder, bool stable)
        {
            // make sure a null comparer type doesn't sneak past

            ComparerBase comparer = null;

            if (column.Comparer != null)
            {
                comparer = (ComparerBase)Activator.CreateInstance(column.Comparer, new object[] { this.TableModel, index, sortOrder });
            }
            else if (column.DefaultComparerType != null)
            {
                comparer = (ComparerBase)Activator.CreateInstance(column.DefaultComparerType, new object[] { this.TableModel, index, sortOrder });
            }
            else
            {
                return;
            }

            column.InternalSortOrder = sortOrder;

            // create the comparer
            SorterBase sorter = null;

            // work out which sort method to use.
            // - InsertionSort/MergeSort are stable sorts, 
            //   whereas ShellSort/HeapSort are unstable
            // - InsertionSort/ShellSort are faster than 
            //   MergeSort/HeapSort on small lists and slower 
            //   on large lists
            // so we choose based on the size of the list and
            // whether the user wants a stable sort
            if (this.TableModel.Rows.Count < 1000)
            {
                if (stable)
                {
                    sorter = new InsertionSorter(this.TableModel, index, comparer, sortOrder);
                }
                else
                {
                    sorter = new ShellSorter(this.TableModel, index, comparer, sortOrder);
                }
            }
            else
            {
                if (stable)
                {
                    sorter = new MergeSorter(this.TableModel, index, comparer, sortOrder);
                }
                else
                {
                    sorter = new HeapSorter(this.TableModel, index, comparer, sortOrder);
                }
            }

            // don't let the table redraw
            this.BeginUpdate();

            this.OnBeginSort(new ColumnEventArgs(column, index, ColumnEventType.Sorting, null));

            sorter.Sort();

            this.OnEndSort(new ColumnEventArgs(column, index, ColumnEventType.Sorting, null));

            // redraw any changes
            this.EndUpdate();
        }


        /// <summary>
        /// Returns whether a Column exists at the specified index in the 
        /// Table's ColumnModel
        /// </summary>
        /// <param name="column">The index of the column to check</param>
        /// <returns>True if a Column exists at the specified index in the 
        /// Table's ColumnModel, false otherwise</returns>
        /// <MetaDataID>{E7CB4C06-D82C-47C0-8257-6C7FB9B06DBF}</MetaDataID>
        public bool IsValidColumn(int column)
        {
            if (this.ColumnModel == null)
            {
                return false;
            }

            return (column >= 0 && column < this.ColumnModel.Columns.Count);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// The height of each Row in the TableModel
        /// </summary>
        /// <MetaDataID>{5bb5ac21-2134-4095-a8ea-2e9b4342009d}</MetaDataID>
        private int rowHeight = TableModel.DefaultRowHeight;


        /// <summary>
        /// Gets or sets the height of each Row in the TableModel
        /// </summary>
        /// <MetaDataID>{bdcdaad2-fd5d-4872-bc50-7ba6887be984}</MetaDataID>
        [Category("Appearance"),
        Description("The height of each row")]
        public int RowHeight
        {
            get
            {
                return this.rowHeight;
            }

            set
            {
                if (value < TableModel.MinimumRowHeight)
                {
                    value = TableModel.MinimumRowHeight;
                }
                else if (value > TableModel.MaximumRowHeight)
                {
                    value = TableModel.MaximumRowHeight;
                }

                if (this.rowHeight != value)
                {
                    this.rowHeight = value;

                    this.OnRowHeightChanged(EventArgs.Empty);
                }
            }
        }






        #region Borders

        /// <summary>
        /// Gets or sets the border style for the Table
        /// </summary>
        /// <MetaDataID>{ccacd7cd-33a9-4d17-8500-957356d6eb32}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(BorderStyle.Fixed3D),
        Description("Indicates the border style for the Table")]
        public BorderStyle BorderStyle
        {
            get
            {
                return this.borderStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(BorderStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
                }

                if (borderStyle != value)
                {
                    this.borderStyle = value;

                    this.Invalidate(true);
                }
            }
        }


        /// <summary>
        /// Gets the width of the Tables border
        /// </summary>
        /// <MetaDataID>{4ba3ec14-6356-4b62-b96e-a17fb7631a48}</MetaDataID>
        protected int BorderWidth
        {
            get
            {
                if (this.BorderStyle == BorderStyle.Fixed3D)
                {
                    return SystemInformation.Border3DSize.Width;
                }
                else if (this.BorderStyle == BorderStyle.FixedSingle)
                {
                    return 1;
                }

                return 0;
            }
        }

        #endregion

        #region Cells

        /// <summary>
        /// Gets the last known cell position that the mouse was over
        /// </summary>
        /// <MetaDataID>{44282788-bef3-496f-b404-49329a1bd9fe}</MetaDataID>
        [Browsable(false)]
        public CellPos LastMouseCell
        {
            get
            {
                return this.lastMouseCell;
            }
        }


        /// <summary>
        /// Gets the last known cell position that the mouse's left 
        /// button was pressed in
        /// </summary>
        /// <MetaDataID>{c5f881e8-8b47-45f7-95d4-a606ba030129}</MetaDataID>
        [Browsable(false)]
        public CellPos LastMouseDownCell
        {
            get
            {
                return this.lastMouseDownCell;
            }
        }

        /// <MetaDataID>{21c4a0f8-801e-4689-8580-cf2b3368c576}</MetaDataID>
        [Browsable(false)]
        public CellPos LastMouseClickCell
        {
            get
            {
                return this.lastMouseClickCell;
            }
        }


        /// <summary>
        /// Gets or sets the position of the Cell that currently has focus
        /// </summary>
        /// <MetaDataID>{63fb4ef9-620d-443b-9fb7-6c2827098ffe}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CellPos FocusedCell
        {
            get
            {
                return this.focusedCell;
            }

            set
            {
                if (!this.IsValidCell(value))
                {
                    return;
                }

                if (!this.TableModel[value].Enabled)
                {
                    return;
                }

                if (this.focusedCell != value)
                {
                    if (!this.focusedCell.IsEmpty)
                    {
                        this.RaiseCellLostFocus(this.focusedCell);
                    }

                    this.focusedCell = value;

                    if (!value.IsEmpty)
                    {
                        this.EnsureVisible(value);

                        this.RaiseCellGotFocus(value);
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the amount of time (in milliseconds) that that the 
        /// mouse pointer must hover over a Cell or Column Header before 
        /// a MouseHover event is raised
        /// </summary>
        /// <MetaDataID>{9f96b088-4554-48d2-b6e4-b741df2f7589}</MetaDataID>
        [Category("Behavior"),
        DefaultValue(1000),
        Description("The amount of time (in milliseconds) that that the mouse pointer must hover over a Cell or Column Header before a MouseHover event is raised")]
        public int HoverTime
        {
            get
            {
                return this.hoverTime;
            }

            set
            {
                if (value < 100)
                {
                    throw new ArgumentException("HoverTime cannot be less than 100", "value");
                }

                if (this.hoverTime != value)
                {
                    this.hoverTime = value;

                    this.ResetMouseEventArgs();
                }
            }
        }

        #endregion

        #region ClientRectangle

        /// <summary>
        /// Gets the rectangle that represents the "client area" of the control.
        /// (The rectangle excludes the borders and scrollbars)
        /// </summary>
        /// <MetaDataID>{af7054b6-f964-4c09-a85b-2aebd08a305b}</MetaDataID>
        [Browsable(false)]
        public Rectangle PseudoClientRect
        {
            get
            {
                Rectangle clientRect = this.InternalBorderRect;

                if (this.HScroll)
                {
                    clientRect.Height -= SystemInformation.HorizontalScrollBarHeight;
                }

                if (this.VScroll)
                {
                    clientRect.Width -= SystemInformation.VerticalScrollBarWidth;
                }

                return clientRect;
            }
        }


        /// <summary>
        /// Gets the rectangle that represents the "cell data area" of the control.
        /// (The rectangle excludes the borders, column headers and scrollbars)
        /// </summary>
        /// <MetaDataID>{4a27e119-9f3e-4cbc-88a0-7a80d686d078}</MetaDataID>
        [Browsable(false)]
        public Rectangle CellDataRect
        {
            get
            {
                Rectangle clientRect = this.PseudoClientRect;

                if (this.HeaderStyle != ColumnHeaderStyle.None && this.ColumnCount > 0)
                {
                    clientRect.Y += this.HeaderHeight;
                    clientRect.Height -= this.HeaderHeight;
                }

                return clientRect;
            }
        }


        /// <summary></summary>
        /// <MetaDataID>{d622ccf0-6010-4b60-80df-1dc776b46507}</MetaDataID>
        private Rectangle InternalBorderRect
        {
            get
            {
                return new Rectangle(this.BorderWidth,
                    this.BorderWidth,
                    this.Width - (this.BorderWidth * 2),
                    this.Height - (this.BorderWidth * 2));
            }
        }

        #endregion

        #region ColumnModel

        /// <summary>
        /// Gets or sets the ColumnModel that contains all the Columns
        /// displayed in the Table
        /// </summary>
        /// <MetaDataID>{de0d1f84-f494-416d-9057-1a2cff9e5ae0}</MetaDataID>
        [Category("Columns"),
        DefaultValue(null),
        Description("Specifies the ColumnModel that contains all the Columns displayed in the Table")]
        public ColumnModel ColumnModel
        {
            get
            {
                if (this.columnModel == null)
                {
                    ColumnModel = new ColumnModel();
                }
                return this.columnModel;
            }

            set
            {
                if (this.columnModel != value)
                {
                    if (this.columnModel != null && this.columnModel.Table == this)
                    {
                        this.columnModel.InternalTable = null;
                    }

                    this.columnModel = value;

                    if (value != null)
                    {
                        value.InternalTable = this;
                    }

                    this.OnColumnModelChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Table allows users to resize Column widths
        /// </summary>
        /// <MetaDataID>{63292062-233c-42e0-addf-b5381b55416e}</MetaDataID>
        [Category("Columns"),
        DefaultValue(true),
        Description("Specifies whether the Table allows users to resize Column widths")]
        public bool ColumnResizing
        {
            get
            {
                return this.columnResizing;
            }

            set
            {
                if (this.columnResizing != value)
                {
                    this.columnResizing = value;
                }
            }
        }


        /// <summary>
        /// Returns the number of Columns in the Table
        /// </summary>
        /// <MetaDataID>{9eb08531-ac07-4613-bfa1-70e0ab9aadbf}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ColumnCount
        {
            get
            {
                if (this.ColumnModel == null)
                {
                    return -1;
                }

                return this.ColumnModel.Columns.Count;
            }
        }


        /// <summary>
        /// Returns the index of the currently sorted Column
        /// </summary>
        /// <MetaDataID>{aabf14ba-8deb-4875-9784-39e2019a4336}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SortingColumn
        {
            get
            {
                return this.lastSortedColumn;
            }
        }


        /// <summary>
        /// Gets or sets the background Color for the currently sorted column
        /// </summary>
        /// <MetaDataID>{261fe449-9617-43b2-b123-ff9bb2be2b68}</MetaDataID>
        [Category("Columns"),
        Description("The background Color for a sorted Column")]
        public Color SortedColumnBackColor
        {
            get
            {
                return this.sortedColumnBackColor;
            }

            set
            {
                if (this.sortedColumnBackColor != value)
                {
                    this.sortedColumnBackColor = value;

                    if (this.IsValidColumn(this.lastSortedColumn))
                    {
                        Rectangle columnRect = this.ColumnRect(this.lastSortedColumn);

                        if (this.PseudoClientRect.IntersectsWith(columnRect))
                        {
                            this.Invalidate(Rectangle.Intersect(this.PseudoClientRect, columnRect));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's SortedColumnBackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the SortedColumnBackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{3F60CFB4-778F-499A-BC33-7A09C217620B}</MetaDataID>
        private bool ShouldSerializeSortedColumnBackColor()
        {
            return this.sortedColumnBackColor != Color.WhiteSmoke;
        }

        #endregion

        #region DisplayRectangle

        /// <summary>
        /// Gets the rectangle that represents the display area of the Table
        /// </summary>
        /// <MetaDataID>{cad8e87f-c68c-4419-bc7f-f39558c9c498}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle displayRect = this.CellDataRect;

                if (!this.init)
                {
                    displayRect.X -= this.hScrollBar.Value;
                    displayRect.Y -= this.vScrollBar.Value;
                }

                if (this.ColumnModel == null)
                {
                    return displayRect;
                }

                if (this.ColumnModel.TotalColumnWidth <= this.CellDataRect.Width)
                {
                    displayRect.Width = this.CellDataRect.Width;
                }
                else
                {
                    displayRect.Width = this.ColumnModel.VisibleColumnsWidth;
                }

                if (this.TotalRowHeight <= this.CellDataRect.Height)
                {
                    displayRect.Height = this.CellDataRect.Height;
                }
                else
                {
                    displayRect.Height = this.TotalRowHeight;
                }

                return displayRect;
            }
        }

        #endregion

        #region Editing

        /// <summary>
        /// Gets whether the Table is currently editing a Cell
        /// </summary>
        /// <MetaDataID>{48a5ff7b-869d-4c86-ae80-895334b530df}</MetaDataID>
        [Browsable(false)]
        public bool IsEditing
        {
            get
            {
                return !this.EditingCell.IsEmpty;
            }
        }


        /// <summary>
        /// Gets a CellPos that specifies the position of the Cell that 
        /// is currently being edited
        /// </summary>
        /// <MetaDataID>{6a485509-8cc3-4775-842b-e42cb9571731}</MetaDataID>
        [Browsable(false)]
        public CellPos EditingCell
        {
            get
            {
                return this.editingCell;
            }
        }


        /// <summary>
        /// Gets the ICellEditor that is currently being used to edit a Cell
        /// </summary>
        /// <MetaDataID>{b5c21901-efa6-44b6-9adf-34d844fb5049}</MetaDataID>
        [Browsable(false)]
        public ICellEditor EditingCellEditor
        {
            get
            {
                return this.curentCellEditor;
            }
        }


        /// <summary>
        /// Gets or sets the action that causes editing to be initiated
        /// </summary>
        /// <MetaDataID>{c8c90387-951f-40b1-a9df-560e5ae13e3e}</MetaDataID>
        [Category("Editing"),
        DefaultValue(EditStartAction.DoubleClick),
        Description("The action that causes editing to be initiated")]
        public EditStartAction EditStartAction
        {
            get
            {
                return this.editStartAction;
            }

            set
            {
                if (!Enum.IsDefined(typeof(EditStartAction), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(EditStartAction));
                }

                if (this.editStartAction != value)
                {
                    this.editStartAction = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the custom key used to initiate Cell editing
        /// </summary>
        /// <MetaDataID>{0c2e3be3-2634-4ca1-8d82-c68946a6b021}</MetaDataID>
        [Category("Editing"),
        DefaultValue(Keys.F5),
        Description("The custom key used to initiate Cell editing")]
        public Keys CustomEditKey
        {
            get
            {
                return this.customEditKey;
            }

            set
            {
                if (this.IsReservedKey(value))
                {
                    throw new ArgumentException("CustomEditKey cannot be one of the Table's reserved keys " +
                        "(Up arrow, Down arrow, Left arrow, Right arrow, PageUp, " +
                        "PageDown, Home, End, Tab)", "value");
                }

                if (this.customEditKey != value)
                {
                    this.customEditKey = value;
                }
            }
        }


        /*/// <summary>
        /// Gets or sets whether pressing the Tab key during editing moves
        /// the editor to the next editable Cell
        /// </summary>
        [Category("Editing"),
        DefaultValue(true),
        Description("")]
        public bool TabMovesEditor
        {
            get
            {	
                return this.tabMovesEditor;
            }

            set
            {
                this.tabMovesEditor = value;
            }
        }*/

        #endregion

        #region Grid

        /// <summary>
        /// Gets or sets how grid lines are displayed around rows and columns
        /// </summary>
        /// <MetaDataID>{018a4cab-b7e7-4c0a-b956-ccd573f69c25}</MetaDataID>
        [Category("Grid"),
        DefaultValue(GridLines.None),
        Description("Determines how grid lines are displayed around rows and columns")]
        public GridLines GridLines
        {
            get
            {
                return this.gridLines;
            }

            set
            {
                if (!Enum.IsDefined(typeof(GridLines), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(GridLines));
                }

                if (this.gridLines != value)
                {
                    this.gridLines = value;

                    this.Invalidate(this.PseudoClientRect, false);
                }
            }
        }


        /// <summary>
        /// Gets or sets the style of the lines used to draw the grid
        /// </summary>
        /// <MetaDataID>{3e5d7166-b7fd-41fc-bca6-8fa9ea206dea}</MetaDataID>
        [Category("Grid"),
        DefaultValue(GridLineStyle.Solid),
        Description("The style of the lines used to draw the grid")]
        public GridLineStyle GridLineStyle
        {
            get
            {
                return this.gridLineStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(GridLineStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(GridLineStyle));
                }

                if (this.gridLineStyle != value)
                {
                    this.gridLineStyle = value;

                    if (this.GridLines != GridLines.None)
                    {
                        this.Invalidate(this.PseudoClientRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the Color of the grid lines
        /// </summary>
        /// <MetaDataID>{be8349a7-6db7-4523-9b07-effccd9030c5}</MetaDataID>
        [Category("Grid"),
        Description("The color of the grid lines")]
        public Color GridColor
        {
            get
            {
                return this.gridColor;
            }

            set
            {
                if (this.gridColor != value)
                {
                    this.gridColor = value;

                    if (this.GridLines != GridLines.None)
                    {
                        this.Invalidate(this.PseudoClientRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's GridColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the GridColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{CB3C1069-916D-485C-AC1D-D5A0D507E0FB}</MetaDataID>
        private bool ShouldSerializeGridColor()
        {
            return (this.GridColor != SystemColors.Control);
        }


        /// <summary></summary>
        /// <MetaDataID>{dea565b0-26e0-445f-9f97-89ca3e043700}</MetaDataID>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                base.BackColor = value;
            }
        }


        /// <MetaDataID>{5325973b-2075-47c1-8b7d-d6c229f0ef93}</MetaDataID>
        Color _BackColor;
        /// <MetaDataID>{fd7b3148-7112-4f8f-9b8b-482d2b801aaa}</MetaDataID>
        Color _FocusedBackColor;

        /// <MetaDataID>{470a9c87-c730-49d1-99da-b20f18a2332b}</MetaDataID>
        public Color FocusedBackColor
        {
            get
            {

                return _FocusedBackColor;
            }

            set
            {
                _FocusedBackColor = value;
            }
        }


        /// <summary>
        /// Specifies whether the Table's BackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the BackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{7F292E73-8607-4733-BF2C-88FC3FD4CF6D}</MetaDataID>
        private bool ShouldSerializeBackColor()
        {
            return (this.BackColor != Color.White);
        }

        #endregion

        #region Header

        /// <summary>
        /// Gets or sets the column header style
        /// </summary>
        /// <MetaDataID>{70d12fbb-be09-43b5-afd1-be1f97432be2}</MetaDataID>
        [Category("Columns"),
        DefaultValue(ColumnHeaderStyle.Clickable),
        Description("The style of the column headers")]
        public ColumnHeaderStyle HeaderStyle
        {
            get
            {
                return this.headerStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(ColumnHeaderStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnHeaderStyle));
                }

                if (this.headerStyle != value)
                {
                    this.headerStyle = value;

                    this.pressedColumn = -1;
                    this.hotColumn = -1;

                    this.Invalidate();
                }
            }
        }


        /// <summary>
        /// Gets the height of the column headers
        /// </summary>
        /// <MetaDataID>{3c9ec718-723f-45ee-a4a3-f0f28c7d4b0f}</MetaDataID>
        [Browsable(false)]
        public int HeaderHeight
        {
            get
            {
                if (this.ColumnModel == null || this.HeaderStyle == ColumnHeaderStyle.None)
                {
                    return 0;
                }

                return this.ColumnModel.HeaderHeight;
            }
        }


        /// <summary>
        /// Gets a Rectangle that specifies the size and location of 
        /// the Table's column header area
        /// </summary>
        /// <MetaDataID>{5e930581-3f61-4d11-b9c1-73d27746e8fa}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle HeaderRectangle
        {
            get
            {
                return new Rectangle(this.BorderWidth, this.BorderWidth, this.PseudoClientRect.Width, this.HeaderHeight);
            }
        }


        /// <summary>
        /// Gets or sets the font used to draw the text in the column headers
        /// </summary>
        /// <MetaDataID>{73a58f5f-273d-4d15-8d02-8c5993525568}</MetaDataID>
        [Category("Columns"),
        Description("The font used to draw the text in the column headers")]
        public Font HeaderFont
        {
            get
            {
                return this.headerFont;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("HeaderFont cannot be null");
                }

                if (this.headerFont != value)
                {
                    this.headerFont = value;

                    this.HeaderRenderer.Font = value;

                    this.Invalidate(this.HeaderRectangle, false);
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's HeaderFont property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the HeaderFont property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{D8EAD87B-8B92-4FDE-A67A-EC1E2357DDB8}</MetaDataID>
        private bool ShouldSerializeHeaderFont()
        {
            return this.HeaderFont != this.Font;
        }
        /// <MetaDataID>{5A987133-4D57-4C40-BF2E-ABF7E6BBEBD8}</MetaDataID>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
        }



        /// <summary>
        /// Gets or sets the HeaderRenderer used to draw the Column headers
        /// </summary>
        /// <MetaDataID>{2c169c81-5f42-4b3c-adee-95c620694c7b}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeaderRenderer HeaderRenderer
        {
            get
            {
                if (this.headerRenderer == null)
                {
                    this.headerRenderer = new XPHeaderRenderer();
                }

                return this.headerRenderer;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("HeaderRenderer cannot be null");
                }

                if (this.headerRenderer != value)
                {
                    this.headerRenderer = value;
                    this.headerRenderer.Font = this.HeaderFont;

                    this.Invalidate(this.HeaderRectangle, false);
                }
            }
        }


        /// <summary>
        /// Gets the ContextMenu used for Column Headers
        /// </summary>
        /// <MetaDataID>{08ddc53e-0281-4e2c-8c6d-86b25ed7c049}</MetaDataID>
        [Browsable(false)]
        public HeaderContextMenu HeaderContextMenu
        {
            get
            {
                return this.headerContextMenu;
            }
        }


        /// <summary>
        /// Gets or sets whether the HeaderContextMenu is able to be 
        /// displayed when the user right clicks on a Column Header
        /// </summary>
        /// <MetaDataID>{934581c3-6a10-4351-9001-93aad635b95f}</MetaDataID>
        [Category("Columns"),
        DefaultValue(true),
        Description("Indicates whether the HeaderContextMenu is able to be displayed when the user right clicks on a Column Header")]
        public bool EnableHeaderContextMenu
        {
            get
            {
                return this.HeaderContextMenu.Enabled;
            }

            set
            {
                this.HeaderContextMenu.Enabled = value;
            }
        }

        #endregion

        #region Rows



        /// <summary>
        /// Gets the combined height of all the rows in the Table
        /// </summary>
        /// <MetaDataID>{624ba4c6-d60e-4abb-a948-0822230be48a}</MetaDataID>
        [Browsable(false)]
        protected int TotalRowHeight
        {
            get
            {
                if (this.TableModel == null)
                {
                    return 0;
                }

                return this.TableModel.TotalRowHeight;
            }
        }


        /// <summary>
        /// Gets the combined height of all the rows in the Table 
        /// plus the height of the column headers
        /// </summary>
        /// <MetaDataID>{f17b66f8-9d2b-402a-8130-e2a70af0daea}</MetaDataID>
        [Browsable(false)]
        protected int TotalRowAndHeaderHeight
        {
            get
            {
                return this.TotalRowHeight + this.HeaderHeight;
            }
        }


        /// <summary>
        /// Returns the number of Rows in the Table
        /// </summary>
        /// <MetaDataID>{fec52626-573d-401a-a316-05401a58f1a5}</MetaDataID>
        [Browsable(false)]
        public int RowCount
        {
            get
            {
                if (this.TableModel == null)
                {
                    return 0;
                }

                return this.TableModel.Rows.Count;
            }
        }


        /// <summary>
        /// Gets the number of rows that are visible in the Table
        /// </summary>
        /// <MetaDataID>{f5c578b2-628f-4c58-87f3-5c1afb0337ff}</MetaDataID>
        [Browsable(false)]
        public int VisibleRowCount
        {
            get
            {
                int count = this.CellDataRect.Height / this.RowHeight;

                if ((this.CellDataRect.Height % this.RowHeight) > 0)
                {
                    count++;
                }

                return count;
            }
        }


        /// <summary>
        /// Gets the index of the first visible row in the Table
        /// </summary>
        /// <MetaDataID>{3bf1445e-b27a-45a2-94cf-52ace3b68035}</MetaDataID>
        [Browsable(false)]
        public int TopIndex
        {
            get
            {
                if (this.TableModel == null || this.TableModel.Rows.Count == 0)
                {
                    return -1;
                }

                if (this.VScroll)
                {
                    return this.vScrollBar.Value;
                }

                return 0;
            }
        }


        /// <summary>
        /// Gets the first visible row in the Table
        /// </summary>
        /// <MetaDataID>{a28b3eaa-d255-48e0-8532-89f5aaaa4741}</MetaDataID>
        [Browsable(false)]
        public Row TopItem
        {
            get
            {
                if (this.TableModel == null || this.TableModel.Rows.Count == 0)
                {
                    return null;
                }

                return this.TableModel.Rows[this.TopIndex];
            }
        }


        /// <summary>
        /// Gets or sets the background color of odd-numbered rows in the Table
        /// </summary>
        /// <MetaDataID>{518e4ef9-5ebc-405a-9033-20659ccf5a3d}</MetaDataID>
        [Category("Appearance"),
        DefaultValue(typeof(Color), "Transparent"),
        Description("The background color of odd-numbered rows in the Table")]
        public Color AlternatingRowColor
        {
            get
            {
                return this.alternatingRowColor;
            }

            set
            {
                if (this.alternatingRowColor != value)
                {
                    this.alternatingRowColor = value;

                    this.Invalidate(this.CellDataRect, false);
                }
            }
        }

        #endregion

        #region Scrolling

        /// <summary>
        /// Gets or sets a value indicating whether the Table will 
        /// allow the user to scroll to any columns or rows placed 
        /// outside of its visible boundaries
        /// </summary>
        /// <MetaDataID>{f264be6c-3146-4e45-b3c8-c51c4594e42e}</MetaDataID>
        [Category("Behavior"),
        DefaultValue(true),
        Description("Indicates whether the Table will display scroll bars if it contains more items than can fit in the client area")]
        public bool Scrollable
        {
            get
            {
                return this.scrollable;
            }

            set
            {
                if (this.scrollable != value)
                {
                    this.scrollable = value;

                    this.PerformLayout();
                }
            }
        }


        /// <summary>
        /// Gets a value indicating whether the horizontal 
        /// scroll bar is visible
        /// </summary>
        /// <MetaDataID>{2eebc9e1-cc17-4772-ac04-a73b5ffc92ea}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HScroll
        {
            get
            {
                if (this.hScrollBar == null)
                {
                    return false;
                }

                return this.hScrollBar.Visible;
            }
        }


        /// <summary>
        /// Gets a value indicating whether the vertical 
        /// scroll bar is visible
        /// </summary>
        /// <MetaDataID>{38b5b17d-1325-47a9-9973-735d73b57311}</MetaDataID>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool VScroll
        {
            get
            {
                if (this.vScrollBar == null)
                {
                    return false;
                }

                return this.vScrollBar.Visible;
            }
        }

        #endregion

        #region Selection

        /// <summary>
        /// Gets or sets whether cells are allowed to be selected
        /// </summary>
        /// <MetaDataID>{2e91c095-2168-4aa1-aebd-f794003303ff}</MetaDataID>
        [Category("Selection"),
        DefaultValue(true),
        Description("Specifies whether cells are allowed to be selected")]
        public bool AllowSelection
        {
            get
            {
                return this.allowSelection;
            }

            set
            {
                if (this.allowSelection != value)
                {
                    this.allowSelection = value;

                    if (!value && this.TableModel != null)
                    {
                        this.TableModel.Selections.Clear();
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets how selected Cells are drawn by a Table
        /// </summary>
        /// <MetaDataID>{914603f6-c0bc-4967-9e23-37bd3842b28b}</MetaDataID>
        [Category("Selection"),
        DefaultValue(SelectionStyle.ListView),
        Description("Determines how selected Cells are drawn by a Table")]
        public SelectionStyle SelectionStyle
        {
            get
            {
                return this.selectionStyle;
            }

            set
            {
                if (!Enum.IsDefined(typeof(SelectionStyle), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectionStyle));
                }

                if (this.selectionStyle != value)
                {
                    this.selectionStyle = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets whether multiple cells are allowed to be selected
        /// </summary>
        /// <MetaDataID>{3e40a230-5e46-4a9d-a57e-36534c52ee62}</MetaDataID>
        [Category("Selection"),
        DefaultValue(false),
        Description("Specifies whether multiple cells are allowed to be selected")]
        public bool MultiSelect
        {
            get
            {

                return this.multiSelect;
            }

            set
            {
                if (this.multiSelect != value)
                {
                    this.multiSelect = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets whether all other cells in the row are highlighted 
        /// when a cell is selected
        /// </summary>
        /// <MetaDataID>{491c3673-499b-4d1e-a258-8005c84ecb33}</MetaDataID>
        [Category("Selection"),
        DefaultValue(true),
        Description("Specifies whether all other cells in the row are highlighted when a cell is selected")]
        public bool FullRowSelect
        {
            get
            {
                return this.fullRowSelect;
            }

            set
            {
                if (this.fullRowSelect != value)
                {
                    this.fullRowSelect = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets whether highlighting is removed from the selected 
        /// cells when the Table loses focus
        /// </summary>
        /// <MetaDataID>{f897af28-a11a-4354-967e-856b011a080e}</MetaDataID>
        [Category("Selection"),
        DefaultValue(false),
        Description("Specifies whether highlighting is removed from the selected cells when the Table loses focus")]
        public bool HideSelection
        {
            get
            {
                return this.hideSelection;
            }

            set
            {
                if (this.hideSelection != value)
                {
                    this.hideSelection = value;

                    if (!this.Focused && this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the background color of a selected cell
        /// </summary>
        /// <MetaDataID>{d37a7ec0-4751-48f4-a942-053f9e82e35b}</MetaDataID>
        [Category("Selection"),
        Description("The background color of a selected cell")]
        public Color SelectionBackColor
        {
            get
            {
                return this.selectionBackColor;
            }

            set
            {
                if (this.selectionBackColor != value)
                {
                    this.selectionBackColor = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's SelectionBackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the SelectionBackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{00C01D96-E386-4F03-B7FA-5B8930F5B54B}</MetaDataID>
        private bool ShouldSerializeSelectionBackColor()
        {
            return (this.selectionBackColor != SystemColors.Highlight);
        }


        /// <summary>
        /// Gets or sets the foreground color of a selected cell
        /// </summary>
        /// <MetaDataID>{57fa502f-2d7e-46ce-867f-75db69b12d38}</MetaDataID>
        [Category("Selection"),
        Description("The foreground color of a selected cell")]
        public Color SelectionForeColor
        {
            get
            {
                return this.selectionForeColor;
            }

            set
            {
                if (this.selectionForeColor != value)
                {
                    this.selectionForeColor = value;

                    if (this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's SelectionForeColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the SelectionForeColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{95FAD47E-F361-478D-8307-1CD281C2A8A4}</MetaDataID>
        private bool ShouldSerializeSelectionForeColor()
        {
            return (this.selectionForeColor != SystemColors.HighlightText);
        }


        /// <summary>
        /// Gets or sets the background color of a selected cell when the 
        /// Table doesn't have the focus
        /// </summary>
        /// <MetaDataID>{2693f77e-95a7-4425-bafe-098ce2b1468c}</MetaDataID>
        [Category("Selection"),
        Description("The background color of a selected cell when the Table doesn't have the focus")]
        public Color UnfocusedSelectionBackColor
        {
            get
            {
                return this.unfocusedSelectionBackColor;
            }

            set
            {
                if (this.unfocusedSelectionBackColor != value)
                {
                    this.unfocusedSelectionBackColor = value;

                    if (!this.Focused && !this.HideSelection && this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's UnfocusedSelectionBackColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the UnfocusedSelectionBackColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{59D51259-7940-45C5-9DC1-547547DBE771}</MetaDataID>
        private bool ShouldSerializeUnfocusedSelectionBackColor()
        {
            return (this.unfocusedSelectionBackColor != SystemColors.Control);
        }


        /// <summary>
        /// Gets or sets the foreground color of a selected cell when the 
        /// Table doesn't have the focus
        /// </summary>
        /// <MetaDataID>{5f1015c5-e6bd-4a57-9ce6-c8b97611f79e}</MetaDataID>
        [Category("Selection"),
        Description("The foreground color of a selected cell when the Table doesn't have the focus")]
        public Color UnfocusedSelectionForeColor
        {
            get
            {
                return this.unfocusedSelectionForeColor;
            }

            set
            {
                if (this.unfocusedSelectionForeColor != value)
                {
                    this.unfocusedSelectionForeColor = value;

                    if (!this.Focused && !this.HideSelection && this.TableModel != null)
                    {
                        //this.Invalidate(Rectangle.Intersect(this.CellDataRect, this.TableModel.Selections.SelectionBounds), false);
                        this.Invalidate(this.CellDataRect, false);
                    }
                }
            }
        }


        /// <summary>
        /// Specifies whether the Table's UnfocusedSelectionForeColor property 
        /// should be serialized at design time
        /// </summary>
        /// <returns>True if the UnfocusedSelectionForeColor property should be 
        /// serialized, False otherwise</returns>
        /// <MetaDataID>{BAB4041D-A0EC-474C-9007-83B731D165C7}</MetaDataID>
        private bool ShouldSerializeUnfocusedSelectionForeColor()
        {
            return (this.unfocusedSelectionForeColor != SystemColors.ControlText);
        }


        /// <summary>
        /// Gets an array that contains the currently selected Rows
        /// </summary>
        /// <MetaDataID>{6ce56f85-7521-4410-9914-7323b5e63c23}</MetaDataID>
        [Browsable(false)]
        public Row[] SelectedItems
        {
            get
            {
                if (this.TableModel == null)
                {
                    return new Row[0];
                }

                return this.TableModel.Selections.SelectedItems;
            }
        }


        /// <summary>
        /// Gets an array that contains the indexes of the currently selected Rows
        /// </summary>
        /// <MetaDataID>{26e71ce3-bb64-449e-a072-b304200bf68b}</MetaDataID>
        [Browsable(false)]
        public int[] SelectedIndicies
        {
            get
            {
                if (this.TableModel == null)
                {
                    return new int[0];
                }

                return this.TableModel.Selections.SelectedIndicies;
            }
        }

        #endregion

        #region TableModel

        /// <summary>
        /// Gets or sets the TableModel that contains all the Rows
        /// and Cells displayed in the Table
        /// </summary>
        /// <MetaDataID>{74c2edeb-bfa1-4274-bc5d-47bff23a893e}</MetaDataID>
        [Category("Items"),
        DefaultValue(null),
        Description("Specifies the TableModel that contains all the Rows and Cells displayed in the Table")]
        public TableModel TableModel
        {
            get
            {
                if (this.tableModel == null)
                    this.tableModel = new TableModel();
                return this.tableModel;
            }

            set
            {
                if (this.tableModel != value)
                {
                    if (this.tableModel != null && this.tableModel.Table == this)
                    {
                        this.tableModel.InternalTable = null;
                    }

                    this.tableModel = value;

                    if (value != null)
                    {
                        value.RowHeight = rowHeight;
                        value.InternalTable = this;
                    }

                    this.OnTableModelChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// Gets or sets the text displayed by the Table when it doesn't 
        /// contain any items
        /// </summary>
        /// <MetaDataID>{eba16cf1-f490-4521-8826-316e3bddc846}</MetaDataID>
        [Category("Appearance"),
        DefaultValue("There are no items in this view"),
        Description("Specifies the text displayed by the Table when it doesn't contain any items")]
        public string NoItemsText
        {
            get
            {
                return this.noItemsText;
            }

            set
            {
                if (!this.noItemsText.Equals(value))
                {
                    this.noItemsText = value;

                    if (this.ColumnModel == null || this.TableModel == null || this.TableModel.Rows.Count == 0)
                    {
                        this.Invalidate(this.PseudoClientRect);
                    }
                }
            }
        }

        #endregion

        #region TableState

        /// <summary>
        /// Gets or sets the current state of the Table
        /// </summary>
        /// <MetaDataID>{e077d0dc-7898-48a1-83a7-a7da7ca3c2c9}</MetaDataID>
        protected TableState TableState
        {
            get
            {
                return this.tableState;
            }

            set
            {
                this.tableState = value;
            }
        }


        /// <summary>
        /// Calculates the state of the Table at the specified 
        /// client coordinates
        /// </summary>
        /// <param name="x">The client x coordinate</param>
        /// <param name="y">The client y coordinate</param>
        /// <MetaDataID>{8D4E0335-63BF-4A4F-84FA-0395A4A93A51}</MetaDataID>
        protected void CalcTableState(int x, int y)
        {
            TableRegion region = this.HitTest(x, y);

            // are we in the header
            if (region == TableRegion.ColumnHeader)
            {
                int column = this.ColumnIndexAt(x, y);

                // get out of here if we aren't in a column
                if (column == -1)
                {
                    this.TableState = TableState.Normal;

                    return;
                }

                // get the bounding rectangle for the column's header
                Rectangle columnRect = this.ColumnModel.ColumnHeaderRect(column);
                x = this.ClientToDisplayRect(x, y).X;

                // are we in a resizing section on the left
                if (x < columnRect.Left + Column.ResizePadding)
                {
                    this.TableState = TableState.ColumnResizing;

                    while (column != 0)
                    {
                        if (this.ColumnModel.Columns[column - 1].Visible)
                        {
                            break;
                        }

                        column--;
                    }

                    // if we are in the first visible column or the next column 
                    // to the left is disabled, then we should be potentialy 
                    // selecting instead of resizing
                    if (column == 0 || !this.ColumnModel.Columns[column - 1].Enabled)
                    {
                        this.TableState = TableState.ColumnSelecting;
                    }
                }
                // or a resizing section on the right
                else if (x > columnRect.Right - Column.ResizePadding)
                {
                    this.TableState = TableState.ColumnResizing;
                }
                // looks like we're somewhere in the middle of 
                // the column header
                else
                {
                    this.TableState = TableState.ColumnSelecting;
                }
            }
            else if (region == TableRegion.Cells)
            {
                this.TableState = TableState.Selecting;
            }
            else
            {
                this.TableState = TableState.Normal;
            }

            if (this.TableState == TableState.ColumnResizing && !this.ColumnResizing)
            {
                this.TableState = TableState.ColumnSelecting;
            }
        }


        /// <summary>
        /// Gets whether the Table is able to raise events
        /// </summary>
        /// <MetaDataID>{ad2a58a1-f2d9-44f0-9fe6-9bea1a40029c}</MetaDataID>
        protected internal bool CanRaiseEvents
        {
            get
            {
                return (this.IsHandleCreated && this.beginUpdateCount == 0);
            }
        }
        /// <MetaDataID>{ca8ec96f-bd6f-47a7-be27-c35b8a4db3c9}</MetaDataID>
        public override Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
            }
        }


        /// <summary>
        /// Gets or sets whether the Table is being used as a preview Table in 
        /// a ColumnCollectionEditor
        /// </summary>
        /// <MetaDataID>{35014d69-0189-4dd7-9405-81596df2ca71}</MetaDataID>
        internal bool Preview
        {
            get
            {
                return this.preview;
            }

            set
            {
                this.preview = value;
            }
        }

        #endregion

        #region ToolTips

        /// <summary>
        /// Gets the internal tooltip component
        /// </summary>
        /// <MetaDataID>{41085f10-d163-4b34-b7c7-124eea3e52f6}</MetaDataID>
        internal ToolTip ToolTip
        {
            get
            {
                return this.toolTip;
            }
        }


        /// <summary>
        /// Gets or sets whether ToolTips are currently enabled for the Table
        /// </summary>
        /// <MetaDataID>{8f17bcd0-a7bd-4ba5-9942-b2223e94dbb8}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(false),
        Description("Specifies whether ToolTips are enabled for the Table.")]
        public bool EnableToolTips
        {
            get
            {
                return this.toolTip.Active;
            }

            set
            {
                this.toolTip.Active = value;
            }
        }


        /// <summary>
        /// Gets or sets the automatic delay for the Table's ToolTip
        /// </summary>
        /// <MetaDataID>{1c0bcbd0-72cc-4b15-809f-e4e9cf1210a4}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(500),
        Description("Specifies the automatic delay for the Table's ToolTip.")]
        public int ToolTipAutomaticDelay
        {
            get
            {
                return this.toolTip.AutomaticDelay;
            }

            set
            {
                if (value > 0 && this.toolTip.AutomaticDelay != value)
                {
                    this.toolTip.AutomaticDelay = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the period of time the Table's ToolTip remains visible if 
        /// the mouse pointer is stationary within a Cell with a valid ToolTip text
        /// </summary>
        /// <MetaDataID>{f12d51c7-630e-4614-bdb9-8b0909578a89}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(5000),
        Description("Specifies the period of time the Table's ToolTip remains visible if the mouse pointer is stationary within a cell with specified ToolTip text.")]
        public int ToolTipAutoPopDelay
        {
            get
            {
                return this.toolTip.AutoPopDelay;
            }

            set
            {
                if (value > 0 && this.toolTip.AutoPopDelay != value)
                {
                    this.toolTip.AutoPopDelay = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the time that passes before the Table's ToolTip appears
        /// </summary>
        /// <MetaDataID>{4e4e1c69-da0c-45c3-ac55-959954b053e9}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(1000),
        Description("Specifies the time that passes before the Table's ToolTip appears.")]
        public int ToolTipInitialDelay
        {
            get
            {
                return this.toolTip.InitialDelay;
            }

            set
            {
                if (value > 0 && this.toolTip.InitialDelay != value)
                {
                    this.toolTip.InitialDelay = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets whether the Table's ToolTip window is 
        /// displayed even when its parent control is not active
        /// </summary>
        /// <MetaDataID>{10471cfe-ec51-4cef-980a-938b4c336876}</MetaDataID>
        [Category("ToolTips"),
        DefaultValue(false),
        Description("Specifies whether the Table's ToolTip window is displayed even when its parent control is not active.")]
        public bool ToolTipShowAlways
        {
            get
            {
                return this.toolTip.ShowAlways;
            }

            set
            {
                if (this.toolTip.ShowAlways != value)
                {
                    this.toolTip.ShowAlways = value;
                }
            }
        }


        /// <summary>
        /// </summary>
        /// <MetaDataID>{46CE6F61-DB82-43C3-BA86-6AF3AD307F18}</MetaDataID>
        private void ResetToolTip()
        {
            bool tooltipActive = this.ToolTip.Active;

            if (tooltipActive)
            {
                this.ToolTip.Active = false;
            }

            this.ResetMouseEventArgs();

            this.ToolTip.SetToolTip(this, null);

            if (tooltipActive)
            {
                this.ToolTip.Active = true;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Cells

        /// <summary>
        /// Raises the CellPropertyChanged event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{0238BBA3-C3A9-42C8-B3F1-5ADA9AF34261}</MetaDataID>
        protected internal virtual void OnCellPropertyChanged(CellEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateCell(e.Row, e.Column);

                if (CellPropertyChanged != null)
                {
                    CellPropertyChanged(this, e);
                }

                if (e.EventType == CellEventType.CheckStateChanged)
                {
                    this.OnCellCheckChanged(new CellCheckBoxEventArgs(e.Cell, e.Column, e.Row));
                }
            }
        }


        /// <summary>
        /// Handler for a Cells PropertyChanged event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{70F051BE-FE93-4BFE-A6D5-59ED5B8A4064}</MetaDataID>
        private void cell_PropertyChanged(object sender, CellEventArgs e)
        {
            this.OnCellPropertyChanged(e);
        }


        #region Buttons

        /// <summary>
        /// Raises the CellButtonClicked event
        /// </summary>
        /// <param name="e">A CellButtonEventArgs that contains the event data</param>
        /// <MetaDataID>{7266324A-3B35-4FD4-821B-181E8B4EB0BD}</MetaDataID>
        protected internal virtual void OnCellButtonClicked(CellButtonEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (CellButtonClicked != null)
                {
                    CellButtonClicked(this, e);
                }
            }
        }

        #endregion

        #region CheckBox

        /// <summary>
        /// Raises the CellCheckChanged event
        /// </summary>
        /// <param name="e">A CellCheckChanged that contains the event data</param>
        /// <MetaDataID>{1A565C8B-71EE-4C59-9432-9BD14B5869DA}</MetaDataID>
        protected internal virtual void OnCellCheckChanged(CellCheckBoxEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (CellCheckChanged != null)
                {
                    CellCheckChanged(this, e);
                }
            }
        }

        #endregion

        #region Focus

        /// <summary>
        /// Raises the CellGotFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{02771F10-C2DF-4FE4-8F27-21AEEBC829E0}</MetaDataID>
        protected virtual void OnCellGotFocus(CellFocusEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnGotFocus(e);
                }

                if (CellGotFocus != null)
                {
                    CellGotFocus(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the GotFocus event for the Cell at the specified position
        /// </summary>
        /// <param name="cellPos">The position of the Cell that gained focus</param>
        /// <MetaDataID>{26B56C10-ADF2-4045-9F52-293A21E76D0A}</MetaDataID>
        protected void RaiseCellGotFocus(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            ICellRenderer renderer = this.ColumnModel.GetCellRenderer(cellPos.Column);

            if (renderer != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellFocusEventArgs cfea = new CellFocusEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellGotFocus(cfea);
            }
        }


        /// <summary>
        /// Raises the CellLostFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{292FB897-A941-4FAF-A929-DDF1C2106A06}</MetaDataID>
        protected virtual void OnCellLostFocus(CellFocusEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnLostFocus(e);
                }

                if (CellLostFocus != null)
                {
                    CellLostFocus(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the LostFocus event for the Cell at the specified position
        /// </summary>
        /// <param name="cellPos">The position of the Cell that lost focus</param>
        /// <MetaDataID>{4D2527BD-DD77-4D9B-9603-97A416A9FC57}</MetaDataID>
        protected void RaiseCellLostFocus(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            ICellRenderer renderer = this.ColumnModel.GetCellRenderer(cellPos.Column);

            if (renderer != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel[cellPos.Row, cellPos.Column];
                }

                CellFocusEventArgs cfea = new CellFocusEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellLostFocus(cfea);
            }
        }

        #endregion

        #region Keys

        /// <summary>
        /// Raises the CellKeyDown event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{97326148-0840-41C7-B3DF-62230A48ADCC}</MetaDataID>
        protected virtual void OnCellKeyDown(CellKeyEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnKeyDown(e);
                }

                if (CellKeyDown != null)
                {
                    CellKeyDown(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a KeyDown event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{5AA751D2-12F7-476E-867C-FF6A6A263EA2}</MetaDataID>
        protected void RaiseCellKeyDown(CellPos cellPos, KeyEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellKeyEventArgs ckea = new CellKeyEventArgs(cell, this, cellPos, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellKeyDown(ckea);
            }
        }


        /// <summary>
        /// Raises the CellKeyUp event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{8ABAF03F-1030-4C85-BD5C-AC4410968671}</MetaDataID>
        protected virtual void OnCellKeyUp(CellKeyEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnKeyUp(e);
                }

                if (CellKeyUp != null)
                {
                    CellKeyUp(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a KeyUp event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{ED36EDB0-F411-40C0-BD83-898B3B11585B}</MetaDataID>
        protected void RaiseCellKeyUp(CellPos cellPos, KeyEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellKeyEventArgs ckea = new CellKeyEventArgs(cell, this, cellPos, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellKeyUp(ckea);
            }
        }

        #endregion

        #region Mouse

        #region MouseEnter

        /// <summary>
        /// Raises the CellMouseEnter event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{901CE110-E96A-4D2D-9DCE-F95513E6312D}</MetaDataID>
        protected virtual void OnCellMouseEnter(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseEnter(e);
                }

                if (CellMouseEnter != null)
                {
                    CellMouseEnter(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseEnter event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <MetaDataID>{E56309FF-6F2B-491B-9AAA-82D08451DD7C}</MetaDataID>
        protected void RaiseCellMouseEnter(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellMouseEnter(mcea);
            }
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the CellMouseLeave event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{0594AA39-C707-43B3-B61D-5DC1BE2DF8D2}</MetaDataID>
        protected virtual void OnCellMouseLeave(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseLeave(e);
                }

                if (CellMouseLeave != null)
                {
                    CellMouseLeave(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseLeave event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <MetaDataID>{4D964B3E-17E4-46FF-BEE1-7FB11EFAEE61}</MetaDataID>
        protected internal void RaiseCellMouseLeave(CellPos cellPos)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column));

                this.OnCellMouseLeave(mcea);
            }
        }

        #endregion

        #region MouseUp

        /// <summary>
        /// Raises the CellMouseUp event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{D40F58CB-DF18-4F5D-B23B-A21CCDD9FFF1}</MetaDataID>
        protected virtual void OnCellMouseUp(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseUp(e);
                }

                if (CellMouseUp != null)
                {
                    CellMouseUp(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseUp event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{75CD7E58-E5CE-4754-9DD3-0D03897FF9AC}</MetaDataID>
        protected void RaiseCellMouseUp(CellPos cellPos, MouseEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellMouseUp(mcea);
            }
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Raises the CellMouseDown event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{92C87C93-1B55-40BD-A3F5-493F668B157A}</MetaDataID>
        protected virtual void OnCellMouseDown(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseDown(e);
                }

                if (CellMouseDown != null)
                {
                    CellMouseDown(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseDown event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{9CE6208F-ADAC-4E6D-A38B-D8131833495A}</MetaDataID>
        protected void RaiseCellMouseDown(CellPos cellPos, MouseEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (!this.TableModel[cellPos].Enabled)
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellMouseDown(mcea);
            }
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Raises the CellMouseMove event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{96076AEB-9E2E-40B6-92B3-037375AC44EE}</MetaDataID>
        protected virtual void OnCellMouseMove(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(e.Column);

                if (renderer != null)
                {
                    renderer.OnMouseMove(e);
                }

                if (CellMouseMove != null)
                {
                    CellMouseMove(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseMove event for the Cell at the specified cell position
        /// </summary>
        /// <param name="cellPos">The position of the Cell</param>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{91B6B4BF-2C1F-44B0-9F7E-DABD2E1EE79F}</MetaDataID>
        protected void RaiseCellMouseMove(CellPos cellPos, MouseEventArgs e)
        {
            if (!this.IsValidCell(cellPos))
            {
                return;
            }

            if (this.ColumnModel.GetCellRenderer(cellPos.Column) != null)
            {
                Cell cell = null;

                if (cellPos.Column < this.TableModel.Rows[cellPos.Row].Cells.Count)
                {
                    cell = this.TableModel.Rows[cellPos.Row].Cells[cellPos.Column];
                }

                CellMouseEventArgs mcea = new CellMouseEventArgs(cell, this, cellPos.Row, cellPos.Column, this.CellRect(cellPos.Row, cellPos.Column), e);

                this.OnCellMouseMove(mcea);
            }
        }


        /// <summary>
        /// Resets the last known cell position that the mouse was over to empty
        /// </summary>
        /// <MetaDataID>{8400B4A6-BC7C-4CDC-9342-F71AB8B1B42E}</MetaDataID>
        internal void ResetLastMouseCell()
        {
            if (!this.lastMouseCell.IsEmpty)
            {
                this.ResetMouseEventArgs();

                CellPos oldLastMouseCell = this.lastMouseCell;
                this.lastMouseCell = CellPos.Empty;

                this.RaiseCellMouseLeave(oldLastMouseCell);
            }
        }

        #endregion

        #region MouseHover

        /// <summary>
        /// Raises the CellHover event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{0FAEE45C-36F8-40B9-9516-887AB6A5EF10}</MetaDataID>
        protected virtual void OnCellMouseHover(CellMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (CellMouseHover != null)
                {
                    CellMouseHover(e.Cell, e);
                }
            }
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the CellClick event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{170795AC-DD2C-41DC-BE30-8B28ACBEF573}</MetaDataID>
        protected virtual void OnCellClick(CellMouseEventArgs e)
        {
            lastMouseClickCell = e.CellPos;

            if (this.EditingCell != CellPos.Empty)
            {
                // don't bother if we're already editing the cell.  
                // if we're editing a different cell stop editing
                if (this.EditingCell == e.CellPos)
                {
                    return;
                }
                else
                {
                    this.EditingCellEditor.StopEditing();
                    this.editingCell = CellPos.Empty;
                }

            }

            if (!this.IsCellEnabled(e.CellPos))
            {
                return;
            }

            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(this.LastMouseCell.Column);

                if (renderer != null)
                {
                    renderer.OnClick(e);
                }

                if (CellClick != null)
                {
                    CellClick(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellDoubleClick event
        /// </summary>
        /// <param name="e">A CellEventArgs that contains the event data</param>
        /// <MetaDataID>{2A15B027-96D5-4283-942A-64933DAC5C7A}</MetaDataID>
        protected virtual void OnCellDoubleClick(CellMouseEventArgs e)
        {
            if (!this.IsCellEnabled(e.CellPos))
            {
                return;
            }

            if (this.CanRaiseEvents)
            {
                ICellRenderer renderer = this.ColumnModel.GetCellRenderer(this.LastMouseCell.Column);

                if (renderer != null)
                {
                    renderer.OnDoubleClick(e);
                }

                if (CellDoubleClick != null)
                {
                    CellDoubleClick(e.Cell, e);
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Columns

        /// <summary>
        /// Raises the ColumnPropertyChanged event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{11BD74E6-BF96-4522-8DC7-89181EBD3A5A}</MetaDataID>
        protected internal virtual void OnColumnPropertyChanged(ColumnEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                Rectangle columnHeaderRect;

                if (e.Index != -1)
                {
                    columnHeaderRect = this.ColumnHeaderRect(e.Index);
                }
                else
                {
                    columnHeaderRect = this.ColumnHeaderRect(e.Column);
                }

                switch (e.EventType)
                {
                    case ColumnEventType.VisibleChanged:
                    case ColumnEventType.WidthChanged:
                        {
                            if (e.EventType == ColumnEventType.VisibleChanged)
                            {
                                if (e.Column.Visible && e.Index != this.lastSortedColumn)
                                {
                                    e.Column.InternalSortOrder = SortOrder.None;
                                }

                                if (e.Index == this.FocusedCell.Column && !e.Column.Visible)
                                {
                                    int index = this.ColumnModel.NextVisibleColumn(e.Index);

                                    if (index == -1)
                                    {
                                        index = this.ColumnModel.PreviousVisibleColumn(e.Index);
                                    }

                                    if (index != -1)
                                    {
                                        this.FocusedCell = new CellPos(this.FocusedCell.Row, index);
                                    }
                                    else
                                    {
                                        this.FocusedCell = CellPos.Empty;
                                    }
                                }
                            }

                            if (columnHeaderRect.X <= 0)
                            {
                                this.Invalidate(this.PseudoClientRect);
                            }
                            else if (columnHeaderRect.Left <= this.PseudoClientRect.Right)
                            {
                                this.Invalidate(new Rectangle(columnHeaderRect.X,
                                    this.PseudoClientRect.Top,
                                    this.PseudoClientRect.Right - columnHeaderRect.X,
                                    this.PseudoClientRect.Height));
                            }

                            this.UpdateScrollBars();

                            break;
                        }

                    case ColumnEventType.TextChanged:
                    case ColumnEventType.StateChanged:
                    case ColumnEventType.ImageChanged:
                    case ColumnEventType.HeaderAlignmentChanged:
                        {
                            if (columnHeaderRect.IntersectsWith(this.HeaderRectangle))
                            {
                                this.Invalidate(columnHeaderRect);
                            }

                            break;
                        }

                    case ColumnEventType.AlignmentChanged:
                    case ColumnEventType.RendererChanged:
                    case ColumnEventType.EnabledChanged:
                        {
                            if (e.EventType == ColumnEventType.EnabledChanged)
                            {
                                if (e.Index == this.FocusedCell.Column)
                                {
                                    this.FocusedCell = CellPos.Empty;
                                }
                            }

                            if (columnHeaderRect.IntersectsWith(this.HeaderRectangle))
                            {
                                this.Invalidate(new Rectangle(columnHeaderRect.X,
                                    this.PseudoClientRect.Top,
                                    columnHeaderRect.Width,
                                    this.PseudoClientRect.Height));
                            }

                            break;
                        }
                }

                if (ColumnPropertyChanged != null)
                {
                    ColumnPropertyChanged(e.Column, e);
                }
            }
        }

        #endregion

        #region Column Headers

        #region MouseEnter

        /// <summary>
        /// Raises the HeaderMouseEnter event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{6E42CA31-12A2-44C5-ACE4-61D8086DF5BC}</MetaDataID>
        protected virtual void OnHeaderMouseEnter(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseEnter(e);
                }

                if (HeaderMouseEnter != null)
                {
                    HeaderMouseEnter(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseEnter event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <MetaDataID>{4D8C9FFB-13AA-4F56-AA5E-D53ADD1F2E4E}</MetaDataID>
        protected void RaiseHeaderMouseEnter(int index)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)));

                this.OnHeaderMouseEnter(mhea);
            }
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the HeaderMouseLeave event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{AFDFEA5F-5AE5-461A-9862-943D167F349D}</MetaDataID>
        protected virtual void OnHeaderMouseLeave(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseLeave(e);
                }

                if (HeaderMouseLeave != null)
                {
                    HeaderMouseLeave(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseLeave event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <MetaDataID>{E5CC2B42-EF2E-4870-8722-05A11C01A0AB}</MetaDataID>
        protected void RaiseHeaderMouseLeave(int index)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)));

                this.OnHeaderMouseLeave(mhea);
            }
        }

        #endregion

        #region MouseUp

        /// <summary>
        /// Raises the HeaderMouseUp event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{9A9C0364-689A-40EB-AE40-C904938A2961}</MetaDataID>
        protected virtual void OnHeaderMouseUp(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseUp(e);
                }

                if (HeaderMouseUp != null)
                {
                    HeaderMouseUp(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseUp event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{72A44C4E-1813-4BA4-86EC-51C2E0BDA9F1}</MetaDataID>
        protected void RaiseHeaderMouseUp(int index, MouseEventArgs e)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)), e);

                this.OnHeaderMouseUp(mhea);
            }
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Raises the HeaderMouseDown event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{99808724-FEF1-4D6F-B70F-68FC4F7C69D7}</MetaDataID>
        protected virtual void OnHeaderMouseDown(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseDown(e);
                }

                if (HeaderMouseDown != null)
                {
                    HeaderMouseDown(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseDown event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{E90C4BD2-258E-4068-BBD1-76F8DF973E5F}</MetaDataID>
        protected void RaiseHeaderMouseDown(int index, MouseEventArgs e)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)), e);

                this.OnHeaderMouseDown(mhea);
            }
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Raises the HeaderMouseMove event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{C6C14CF2-8B71-45B7-892B-9573F6E6F7C3}</MetaDataID>
        protected virtual void OnHeaderMouseMove(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnMouseMove(e);
                }

                if (HeaderMouseMove != null)
                {
                    HeaderMouseMove(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises a MouseMove event for the Column header at the specified colunm 
        /// index position
        /// </summary>
        /// <param name="index">The index of the column to recieve the event</param>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{8FB71649-691F-4ED9-8262-9AD7045660EA}</MetaDataID>
        protected void RaiseHeaderMouseMove(int index, MouseEventArgs e)
        {
            if (index < 0 || this.ColumnModel == null || index >= this.ColumnModel.Columns.Count)
            {
                return;
            }

            if (this.HeaderRenderer != null)
            {
                Column column = this.ColumnModel.Columns[index];

                HeaderMouseEventArgs mhea = new HeaderMouseEventArgs(column, this, index, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(index)), e);

                this.OnHeaderMouseMove(mhea);
            }
        }


        /// <summary>
        /// Resets the current "hot" column
        /// </summary>
        /// <MetaDataID>{BC66791E-617A-4A7F-9C4D-04ED4209338C}</MetaDataID>
        internal void ResetHotColumn()
        {
            if (this.hotColumn != -1)
            {
                this.ResetMouseEventArgs();

                int oldHotColumn = this.hotColumn;
                this.hotColumn = -1;

                this.RaiseHeaderMouseLeave(oldHotColumn);
            }
        }

        #endregion

        #region MouseHover

        /// <summary>
        /// Raises the HeaderMouseHover event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{7C2E2EA7-AAFF-467E-88DB-5C28D2D48DFA}</MetaDataID>
        protected virtual void OnHeaderMouseHover(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (HeaderMouseHover != null)
                {
                    HeaderMouseHover(e.Column, e);
                }
            }
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the HeaderClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2C34EF30-97CE-498B-BCBA-7E2963F21DEA}</MetaDataID>
        protected virtual void OnHeaderClick(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnClick(e);
                }

                if (HeaderClick != null)
                {
                    HeaderClick(e.Column, e);
                }
            }
        }


        /// <summary>
        /// Raises the HeaderDoubleClick event
        /// </summary>
        /// <param name="e">A HeaderMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{7412E055-C775-4066-8C5B-AD5A21A4ED6F}</MetaDataID>
        protected virtual void OnHeaderDoubleClick(HeaderMouseEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (this.HeaderRenderer != null)
                {
                    this.HeaderRenderer.OnDoubleClick(e);
                }

                if (HeaderDoubleClick != null)
                {
                    HeaderDoubleClick(e.Column, e);
                }
            }
        }

        #endregion

        #endregion

        #region ColumnModel

        /// <summary>
        /// Raises the ColumnModelChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{8DFE4D27-FD13-4AB8-A137-983A518DC816}</MetaDataID>
        protected virtual void OnColumnModelChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (ColumnModelChanged != null)
                {
                    ColumnModelChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the ColumnAdded event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{5CADEE99-565F-461E-AB51-10FC3AEDB4C7}</MetaDataID>
        protected internal virtual void OnColumnAdded(ColumnModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (ColumnAdded != null)
                {
                    ColumnAdded(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the ColumnRemoved event
        /// </summary>
        /// <param name="e">A ColumnModelEventArgs that contains the event data</param>
        /// <MetaDataID>{E7B4C008-E361-43FF-9436-E352A4D53EB1}</MetaDataID>
        protected internal virtual void OnColumnRemoved(ColumnModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (ColumnRemoved != null)
                {
                    ColumnRemoved(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the HeaderHeightChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{DB8B1904-833E-4F9C-A252-6086CA3FF131}</MetaDataID>
        protected internal virtual void OnHeaderHeightChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (HeaderHeightChanged != null)
                {
                    HeaderHeightChanged(this, e);
                }
            }
        }

        #endregion

        #region Editing

        /// <summary>
        /// Raises the BeginEditing event
        /// </summary>
        /// <param name="e">A CellEditEventArgs that contains the event data</param>
        /// <MetaDataID>{82A15115-CFEA-4D98-9C3E-54BB9F3FB262}</MetaDataID>
        protected internal virtual void OnBeginEditing(CellEditEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (BeginEditing != null)
                {
                    BeginEditing(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises the EditingStopped event
        /// </summary>
        /// <param name="e">A CellEditEventArgs that contains the event data</param>
        /// <MetaDataID>{FB375E0A-B43C-4D55-9F6B-DE44653EA41B}</MetaDataID>
        protected internal virtual void OnEditingStopped(CellEditEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (EditingStopped != null)
                {
                    EditingStopped(e.Cell, e);
                }
            }
        }


        /// <summary>
        /// Raises the EditingCancelled event
        /// </summary>
        /// <param name="e">A CellEditEventArgs that contains the event data</param>
        /// <MetaDataID>{0DB1EFEF-402A-40B6-8B51-5D28D8BDF6FE}</MetaDataID>
        protected internal virtual void OnEditingCancelled(CellEditEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                if (EditingCancelled != null)
                {
                    EditingCancelled(e.Cell, e);
                }
            }
        }

        #endregion

        #region Focus

        /// <summary>
        /// Raises the GotFocus event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{AAED0A08-E2B6-4118-B1C5-13CBC6FFFE2E}</MetaDataID>
        protected override void OnGotFocus(EventArgs e)
        {
            //if (SelectedIndicies.Length == 0 && tableModel != null && tableModel.Rows.Count > 0 && columnModel.Columns.Count > 0)
            //    this.TableModel.Selections.SelectCells(0, 0, 0, columnModel.Columns.Count - 1);

            _BackColor = BackColor;
            BackColor = _FocusedBackColor;
            if (this.FocusedCell.IsEmpty)
            {
                CellPos p = this.FindNextVisibleEnabledCell(this.FocusedCell, true, true, true, true);

                if (this.IsValidCell(p))
                {
                    this.FocusedCell = p;
                }
            }
            else
            {
                this.RaiseCellGotFocus(this.FocusedCell);
            }

            if (this.SelectedIndicies.Length > 0)
            {
                this.Invalidate(this.CellDataRect);
            }

            base.OnGotFocus(e);
        }


        /// <summary>
        /// Raises the LostFocus event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{245AA511-2C19-4BE7-8EBF-EDDE6E2D9FEB}</MetaDataID>
        protected override void OnLostFocus(EventArgs e)
        {
            base.BackColor = _BackColor;
            if (!this.FocusedCell.IsEmpty)
            {
                this.RaiseCellLostFocus(this.FocusedCell);
            }

            if (this.SelectedIndicies.Length > 0)
            {
                this.Invalidate(this.CellDataRect);
            }

            base.OnLostFocus(e);
        }

        #endregion

        #region Keys

        #region KeyDown

        /// <summary>
        /// Raises the KeyDown event
        /// </summary>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{15009A9C-38A9-4D1F-89AC-A831DAD67B4B}</MetaDataID>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (this.IsValidCell(this.FocusedCell))
            {
                if (this.IsReservedKey(e.KeyData))
                {
                    Keys key = e.KeyData & Keys.KeyCode;

                    if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right)
                    {
                        CellPos nextCell;

                        if (key == Keys.Up)
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, this.FocusedCell.Row > 0, false, false, false);
                        }
                        else if (key == Keys.Down)
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, this.FocusedCell.Row < this.RowCount - 1, true, false, false);
                        }
                        else if (key == Keys.Left)
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, false, false, false, true);
                        }
                        else
                        {
                            nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, false, true, false, true);
                        }

                        if (nextCell != CellPos.Empty)
                        {
                            this.FocusedCell = nextCell;

                            if ((e.KeyData & Keys.Modifiers) == Keys.Shift && this.MultiSelect)
                            {
                                this.TableModel.Selections.AddShiftSelectedCell(this.FocusedCell);
                            }
                            else
                            {
                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                    else if (e.KeyData == Keys.PageUp)
                    {
                        if (this.RowCount > 0)
                        {
                            CellPos nextCell;

                            if (!this.VScroll)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(new CellPos(0, this.FocusedCell.Column), true, true, true, false);
                            }
                            else
                            {
                                if (this.FocusedCell.Row > this.vScrollBar.Value && this.TableModel[this.vScrollBar.Value, this.FocusedCell.Column].Enabled)
                                {
                                    nextCell = this.FindNextVisibleEnabledCell(new CellPos(this.vScrollBar.Value, this.FocusedCell.Column), true, true, true, false);
                                }
                                else
                                {
                                    nextCell = this.FindNextVisibleEnabledCell(new CellPos(Math.Max(-1, this.vScrollBar.Value - (this.vScrollBar.LargeChange - 1)), this.FocusedCell.Column), true, true, true, false);
                                }
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                    else if (e.KeyData == Keys.PageDown)
                    {
                        if (this.RowCount > 0)
                        {
                            CellPos nextCell;

                            if (!this.VScroll)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(new CellPos(this.RowCount - 1, this.FocusedCell.Column), true, false, true, false);
                            }
                            else
                            {
                                if (this.FocusedCell.Row < this.vScrollBar.Value + this.vScrollBar.LargeChange)
                                {
                                    if (this.FocusedCell.Row == (this.vScrollBar.Value + this.vScrollBar.LargeChange) - 1 &&
                                        this.RowRect(this.vScrollBar.Value + this.vScrollBar.LargeChange).Bottom > this.CellDataRect.Bottom)
                                    {
                                        nextCell = this.FindNextVisibleEnabledCell(new CellPos(Math.Min(this.RowCount - 1, this.FocusedCell.Row - 1 + this.vScrollBar.LargeChange), this.FocusedCell.Column), true, false, true, false);
                                    }
                                    else
                                    {
                                        nextCell = this.FindNextVisibleEnabledCell(new CellPos(this.vScrollBar.Value + this.vScrollBar.LargeChange - 1, this.FocusedCell.Column), true, false, true, false);
                                    }
                                }
                                else
                                {
                                    nextCell = this.FindNextVisibleEnabledCell(new CellPos(Math.Min(this.RowCount - 1, this.FocusedCell.Row + this.vScrollBar.LargeChange), this.FocusedCell.Column), true, false, true, false);
                                }
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                    else if (e.KeyData == Keys.Home || e.KeyData == Keys.End)
                    {
                        if (this.RowCount > 0)
                        {
                            CellPos nextCell;

                            if (e.KeyData == Keys.Home)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(CellPos.Empty, true, true, true, true);
                            }
                            else
                            {
                                nextCell = this.FindNextVisibleEnabledCell(new CellPos(this.RowCount - 1, this.TableModel.Rows[this.RowCount - 1].Cells.Count), true, false, true, true);
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                this.TableModel.Selections.SelectCell(this.FocusedCell);
                            }
                        }
                    }
                }
                else
                {
                    // check if we can start editing with the custom edit key
                    if (e.KeyData == this.CustomEditKey)//&& this.EditStartAction == EditStartAction.CustomKey)
                    {
                        this.EditCell(this.FocusedCell);

                        return;
                    }

                    // send all other key events to the cell's renderer
                    // for further processing
                    this.RaiseCellKeyDown(this.FocusedCell, e);
                }
            }
            else
            {
                if (this.FocusedCell == CellPos.Empty)
                {
                    Keys key = e.KeyData & Keys.KeyCode;

                    if (this.IsReservedKey(e.KeyData))
                    {
                        if (key == Keys.Down || key == Keys.Right)
                        {
                            CellPos nextCell;

                            if (key == Keys.Down)
                            {
                                nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, true, true, true, false);
                            }
                            else
                            {
                                nextCell = this.FindNextVisibleEnabledCell(this.FocusedCell, false, true, true, true);
                            }

                            if (nextCell != CellPos.Empty)
                            {
                                this.FocusedCell = nextCell;

                                if ((e.KeyData & Keys.Modifiers) == Keys.Shift && this.MultiSelect)
                                {
                                    this.TableModel.Selections.AddShiftSelectedCell(this.FocusedCell);
                                }
                                else
                                {
                                    this.TableModel.Selections.SelectCell(this.FocusedCell);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region KeyUp

        /// <summary>
        /// Raises the KeyUp event
        /// </summary>
        /// <param name="e">A KeyEventArgs that contains the event data</param>
        /// <MetaDataID>{9190D24F-1418-4BCA-8C54-1F869F8DC8A3}</MetaDataID>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (!this.IsReservedKey(e.KeyData))
            {
                // 
                if (e.KeyData == this.CustomEditKey && this.EditStartAction == EditStartAction.CustomKey)
                {
                    return;
                }

                // send all other key events to the cell's renderer
                // for further processing
                this.RaiseCellKeyUp(this.FocusedCell, e);
            }
            else
            {

                if (e.KeyData == Keys.Insert)
                {
                    InsertRow();
                }
                if (e.KeyData == Keys.Delete)
                {
                    foreach (Row row in SelectedItems)
                        ListConnection.DeleteRow(row);
                }

                if (e.KeyData == Keys.Tab)
                {
                    if (this.lastEditingCell != CellPos.Empty)
                    {
                        int nextColumn = this.lastEditingCell.Column + 1;
                        while (columnModel.Columns.Count > nextColumn && !columnModel.Columns[nextColumn].Editable)
                            nextColumn++;

                        if (columnModel.Columns.Count > nextColumn)
                        {
                            EditCell(new CellPos(this.lastEditingCell.Row, nextColumn));
                        }

                    }


                }


            }
        }

        #endregion

        #endregion

        #region Layout

        /// <summary>
        /// Raises the Layout event
        /// </summary>
        /// <param name="levent">A LayoutEventArgs that contains the event data</param>
        /// <MetaDataID>{F69D37E0-CB3B-47E8-AC65-88840BE9C555}</MetaDataID>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (!this.IsHandleCreated || this.init)
            {
                return;
            }

            base.OnLayout(levent);

            this.UpdateScrollBars();
        }

        #endregion

        #region Mouse

        #region MouseUp

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{F229E761-171A-44A3-AE8D-D497EC04B4F2}</MetaDataID>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (!this.CanRaiseEvents)
            {
                return;
            }

            // work out the current state of  play
            this.CalcTableState(e.X, e.Y);

            TableRegion region = this.HitTest(e.X, e.Y);

            if (e.Button == MouseButtons.Left)
            {
                // if the left mouse button was down for a cell, 
                // Raise a mouse up for that cell
                if (!this.LastMouseDownCell.IsEmpty)
                {
                    if (this.IsValidCell(this.LastMouseDownCell))
                    {
                        this.RaiseCellMouseUp(this.LastMouseDownCell, e);
                    }

                    // reset the lastMouseDownCell
                    this.lastMouseDownCell = CellPos.Empty;
                }

                // if we have just finished resizing, it might
                // be a good idea to relayout the table
                if (this.resizingColumnIndex != -1)
                {
                    if (this.resizingColumnWidth != -1)
                    {
                        this.DrawReversibleLine(this.ColumnRect(this.resizingColumnIndex).Left + this.resizingColumnWidth);
                    }

                    this.ColumnModel.Columns[this.resizingColumnIndex].Width = this.resizingColumnWidth;

                    this.resizingColumnIndex = -1;
                    this.resizingColumnWidth = -1;

                    this.UpdateScrollBars();
                    this.Invalidate(this.PseudoClientRect, true);
                }

                // check if the mouse was released in a column header
                if (region == TableRegion.ColumnHeader)
                {
                    int column = this.ColumnIndexAt(e.X, e.Y);

                    // if we are in the header, check if we are in the pressed column
                    if (this.pressedColumn != -1)
                    {
                        if (this.pressedColumn == column)
                        {
                            if (this.hotColumn != -1 && this.hotColumn != column)
                            {
                                this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;
                            }

                            this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Hot;

                            this.RaiseHeaderMouseUp(column, e);
                        }

                        this.pressedColumn = -1;

                        // only sort the column if we have rows to sort
                        if (this.ColumnModel.Columns[column].Sortable)
                        {
                            if (this.TableModel != null && this.TableModel.Rows.Count > 0)
                            {
                                this.Sort(column);
                            }
                        }

                        this.Invalidate(this.HeaderRectangle, false);
                    }

                    return;
                }

                // the mouse wasn't released in a column header, so if we 
                // have a pressed column then we need to make it unpressed
                if (this.pressedColumn != -1)
                {
                    this.pressedColumn = -1;

                    this.Invalidate(this.HeaderRectangle, false);
                }
            }
            //if (TableModel != null && lastMouseDownCell.Row != null && lastMouseDownCell.Column != null)
            //    this.TableModel.Selections.SelectCell(lastMouseDownCell.Row, lastMouseDownCell.Column);
        }



        #endregion

        #region MouseDown



        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{90842172-6028-4A7B-A6B8-34D4B2C8B540}</MetaDataID>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!MouseDownTimer.Enabled && SelectedItems.Length > 1)
            {
                LazyMouseEvent = e;
                MouseDownTimer.Interval = 150;
                MouseDownTimer.Enabled = true;
                return;
            }


            base.OnMouseDown(e);





            if (!this.CanRaiseEvents)
            {
                return;
            }

            this.CalcTableState(e.X, e.Y);
            TableRegion region = this.HitTest(e.X, e.Y);

            int row = this.RowIndexAt(e.X, e.Y);
            int column = this.ColumnIndexAt(e.X, e.Y);

            if (this.IsEditing)
            {
                if (this.EditingCell.Row != row || this.EditingCell.Column != column)
                {
                    this.Focus();

                    if (region == TableRegion.ColumnHeader && e.Button != MouseButtons.Right)
                    {
                        return;
                    }
                }
            }

            #region ColumnHeader

            if (region == TableRegion.ColumnHeader)
            {
                if (e.Button == MouseButtons.Right && this.HeaderContextMenu.Enabled)
                {
                    this.HeaderContextMenu.Show(this, new Point(e.X, e.Y));

                    return;
                }

                if (column == -1 || !this.ColumnModel.Columns[column].Enabled)
                {
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    this.FocusedCell = new CellPos(-1, -1);

                    // don't bother going any further if the user 
                    // double clicked
                    if (e.Clicks > 1)
                    {
                        return;
                    }

                    this.RaiseHeaderMouseDown(column, e);

                    if (this.TableState == TableState.ColumnResizing)
                    {
                        Rectangle columnRect = this.ColumnModel.ColumnHeaderRect(column);
                        int x = this.ClientToDisplayRect(e.X, e.Y).X;

                        if (x <= columnRect.Left + Column.ResizePadding)
                        {
                            //column--;
                            column = this.ColumnModel.PreviousVisibleColumn(column);
                        }

                        this.resizingColumnIndex = column;

                        if (this.resizingColumnIndex != -1)
                        {
                            this.resizingColumnAnchor = this.ColumnModel.ColumnHeaderRect(column).Left;
                            this.resizingColumnOffset = x - (this.resizingColumnAnchor + this.ColumnModel.Columns[column].Width);
                        }
                    }
                    else
                    {
                        if (this.HeaderStyle != ColumnHeaderStyle.Clickable || !this.ColumnModel.Columns[column].Sortable)
                        {
                            return;
                        }

                        if (column == -1)
                        {
                            return;
                        }

                        if (this.pressedColumn != -1)
                        {
                            this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Normal;
                        }

                        this.pressedColumn = column;
                        this.ColumnModel.Columns[column].InternalColumnState = ColumnState.Pressed;
                    }

                    return;
                }
            }

            #endregion

            #region Cells

            if (region == TableRegion.Cells)
            {
                if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (!this.IsValidCell(row, column) || !this.IsCellEnabled(row, column))
                {
                    // clear selections
                    if (TableModel != null)
                        this.TableModel.Selections.Clear();

                    return;
                }

                this.FocusedCell = new CellPos(row, column);

                // don't bother going any further if the user 
                // double clicked or we're not allowed to select
                if (e.Clicks > 1 || !this.AllowSelection)
                {
                    return;
                }

                this.lastMouseDownCell.Row = row;
                this.lastMouseDownCell.Column = column;

                //
                this.RaiseCellMouseDown(new CellPos(row, column), e);

                if (!this.ColumnModel.Columns[column].Selectable)
                {
                    return;
                }

                //

                if ((ModifierKeys & Keys.Shift) == Keys.Shift && this.MultiSelect)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        return;
                    }
                    if (TableModel != null)
                        this.TableModel.Selections.AddShiftSelectedCell(row, column);

                    return;
                }

                if ((ModifierKeys & Keys.Control) == Keys.Control && this.MultiSelect)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        return;
                    }
                    if (TableModel != null)
                    {

                        if (this.TableModel.Selections.IsCellSelected(row, column))
                        {
                            this.TableModel.Selections.RemoveCell(row, column);
                        }
                        else
                        {
                            this.TableModel.Selections.AddCell(row, column);
                        }
                    }

                    return;
                }
                if (TableModel != null)
                {
                    if(!this.SelectedIndicies.ToList().Contains(row))
                        this.TableModel.Selections.SelectCell(row, column);
                }
            }

            #endregion
        }

        #endregion



        #region MouseMove

        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{DF055BA9-6EBE-44D1-BB11-2AF046C99FD7}</MetaDataID>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // don't go any further if the table is editing
            if (this.TableState == TableState.Editing)
            {
                return;
            }
            if (ListConnection.AllowDrag)
                DragRow(e);

            // if the left mouse button is down, check if the LastMouseDownCell 
            // references a valid cell.  if it does, send the mouse move message 
            // to the cell and then exit (this will stop other cells/headers 
            // from getting the mouse move message even if the mouse is over 
            // them - this seems consistent with the way windows does it for 
            // other controls)
            if (e.Button == MouseButtons.Left)
            {
                if (!this.LastMouseDownCell.IsEmpty)
                {
                    if (this.IsValidCell(this.LastMouseDownCell))
                    {
                        this.RaiseCellMouseMove(this.LastMouseDownCell, e);
                        return;
                    }
                }
            }

            // are we resizing a column?
            if (this.resizingColumnIndex != -1)
            {
                if (this.resizingColumnWidth != -1)
                {
                    this.DrawReversibleLine(this.ColumnRect(this.resizingColumnIndex).Left + this.resizingColumnWidth);
                }

                // calculate the new width for the column
                int width = this.ClientToDisplayRect(e.X, e.Y).X - this.resizingColumnAnchor - this.resizingColumnOffset;

                // make sure the new width isn't smaller than the minimum allowed
                // column width, or larger than the maximum allowed column width
                if (width < Column.MinimumWidth)
                {
                    width = Column.MinimumWidth;
                }
                else if (width > Column.MaximumWidth)
                {
                    width = Column.MaximumWidth;
                }

                this.resizingColumnWidth = width;

                //this.ColumnModel.Columns[this.resizingColumnIndex].Width = width;
                this.DrawReversibleLine(this.ColumnRect(this.resizingColumnIndex).Left + this.resizingColumnWidth);


                return;
            }
            if (e.Button == MouseButtons.None)
                Cursor = Cursors.Default;
            // work out the potential state of play
            this.CalcTableState(e.X, e.Y);

            TableRegion hitTest = this.HitTest(e.X, e.Y);

            #region ColumnHeader

            if (hitTest == TableRegion.ColumnHeader)
            {
                // this next bit is pretty complicated. need to work 
                // out which column is displayed as pressed or hot 
                // (so we have the same behaviour as a themed ListView
                // in Windows XP)

                int column = this.ColumnIndexAt(e.X, e.Y);

                // if this isn't the current hot column, reset the
                // hot columns state to normal and set this column
                // to be the hot column
                if (this.hotColumn != column)
                {
                    if (this.hotColumn != -1)
                    {
                        this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;

                        this.RaiseHeaderMouseLeave(this.hotColumn);
                    }

                    if (this.TableState != TableState.ColumnResizing)
                    {
                        this.hotColumn = column;

                        if (this.hotColumn != -1 && this.ColumnModel.Columns[column].Enabled)
                        {
                            this.ColumnModel.Columns[column].InternalColumnState = ColumnState.Hot;

                            this.RaiseHeaderMouseEnter(column);
                        }
                    }
                }
                else
                {
                    if (column != -1 && this.ColumnModel.Columns[column].Enabled)
                    {
                        this.RaiseHeaderMouseMove(column, e);
                    }
                }

                // if this isn't the pressed column, then the pressed columns
                // state should be set back to normal
                if (this.pressedColumn != -1 && this.pressedColumn != column)
                {
                    this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Normal;
                }
                // else if this is the pressed column and its state is not
                // pressed, then we had better set it
                else if (column != -1 && this.pressedColumn == column && this.ColumnModel.Columns[this.pressedColumn].ColumnState != ColumnState.Pressed)
                {
                    this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Pressed;
                }

                // set the cursor to a resizing cursor if necesary
                if (this.TableState == TableState.ColumnResizing)
                {
                    Rectangle columnRect = this.ColumnModel.ColumnHeaderRect(column);
                    int x = this.ClientToDisplayRect(e.X, e.Y).X;

                    this.Cursor = Cursors.VSplit;

                    // if the left mouse button is down, we don't want
                    // the resizing cursor so set it back to the default
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Cursor = Cursors.Default;
                    }

                    // if the mouse is in the left side of the column, 
                    // the first non-hidden column to the left needs to
                    // become the hot column (so the user knows which
                    // column would be resized if a resize action were
                    // to take place
                    if (x < columnRect.Left + Column.ResizePadding)
                    {
                        int col = column;

                        while (col != 0)
                        {
                            col--;

                            if (this.ColumnModel.Columns[col].Visible)
                            {
                                break;
                            }
                        }

                        if (col != -1)
                        {
                            if (this.ColumnModel.Columns[col].Enabled)
                            {
                                if (this.hotColumn != -1)
                                {
                                    this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;
                                }

                                this.hotColumn = col;
                                this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Hot;

                                this.RaiseHeaderMouseEnter(col);
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                            }
                        }
                    }
                    else
                    {
                        if (this.ColumnModel.Columns[column].Enabled)
                        {
                            // this mouse is in the right side of the column, 
                            // so this column needs to be dsiplayed hot
                            this.hotColumn = column;
                            this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Hot;
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
                else
                {
                    // we're not in a resizing area, so make sure the cursor
                    // is the default cursor (we may have just come from a
                    // resizing area)
                    this.Cursor = Cursors.Default;
                }

                // reset the last cell the mouse was over
                this.ResetLastMouseCell();

                return;
            }

            #endregion

            // we're outside of the header, so if there is a hot column,
            // it need to be reset
            if (this.hotColumn != -1)
            {
                this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;

                this.ResetHotColumn();
            }

            // if there is a pressed column, its state need to beset to normal
            if (this.pressedColumn != -1)
            {
                this.ColumnModel.Columns[this.pressedColumn].InternalColumnState = ColumnState.Normal;
            }

            #region Cells

            if (hitTest == TableRegion.Cells)
            {
                // find the cell the mouse is over
                CellPos cellPos = new CellPos(this.RowIndexAt(e.X, e.Y), this.ColumnIndexAt(e.X, e.Y));

                if (!cellPos.IsEmpty)
                {
                    if (cellPos != this.lastMouseCell)
                    {
                        // check if the cell exists (ie is not null)
                        if (this.IsValidCell(cellPos))
                        {
                            CellPos oldLastMouseCell = this.lastMouseCell;

                            if (!oldLastMouseCell.IsEmpty)
                            {
                                this.ResetLastMouseCell();
                            }

                            this.lastMouseCell = cellPos;

                            this.RaiseCellMouseEnter(cellPos);
                        }
                        else
                        {
                            this.ResetLastMouseCell();

                            // make sure the cursor is the default cursor 
                            // (we may have just come from a resizing area in the header)
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        this.RaiseCellMouseMove(cellPos, e);
                    }
                }
                else
                {
                    this.ResetLastMouseCell();

                    if (this.TableModel == null)
                    {
                        this.ResetToolTip();
                    }
                }

                return;
            }
            else
            {
                this.ResetLastMouseCell();

                if (!this.lastMouseDownCell.IsEmpty)
                {
                    this.RaiseCellMouseLeave(this.lastMouseDownCell);
                }

                if (this.TableModel == null)
                {
                    this.ResetToolTip();
                }

                // make sure the cursor is the default cursor 
                // (we may have just come from a resizing area in the header)
                this.Cursor = Cursors.Default;
            }

            #endregion


        }


        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{514BB8E0-7A4C-4820-BC16-CFA6E4B617CD}</MetaDataID>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            // we're outside of the header, so if there is a hot column,
            // it needs to be reset (this shouldn't happen, but better 
            // safe than sorry ;)
            if (this.hotColumn != -1)
            {
                this.ColumnModel.Columns[this.hotColumn].InternalColumnState = ColumnState.Normal;

                this.ResetHotColumn();
            }
        }

        #endregion

        #region MouseWheel

        /// <summary>
        /// Raises the MouseWheel event
        /// </summary>
        /// <param name="e">A MouseEventArgs that contains the event data</param>
        /// <MetaDataID>{4F0B99C2-C2F1-49C3-B678-86B57A295C22}</MetaDataID>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!this.Scrollable || (!this.HScroll && !this.VScroll))
            {
                return;
            }

            if (this.VScroll)
            {
                int newVal = this.vScrollBar.Value - ((e.Delta / 120) * SystemInformation.MouseWheelScrollLines);

                if (newVal < 0)
                {
                    newVal = 0;
                }
                else if (newVal > this.vScrollBar.Maximum - this.vScrollBar.LargeChange + 1)
                {
                    newVal = this.vScrollBar.Maximum - this.vScrollBar.LargeChange + 1;
                }

                this.VerticalScroll(newVal);
                this.vScrollBar.Value = newVal;
            }
            else if (this.HScroll)
            {
                int newVal = this.hScrollBar.Value - ((e.Delta / 120) * Column.MinimumWidth);

                if (newVal < 0)
                {
                    newVal = 0;
                }
                else if (newVal > this.hScrollBar.Maximum - this.hScrollBar.LargeChange)
                {
                    newVal = this.hScrollBar.Maximum - this.hScrollBar.LargeChange;
                }

                this.HorizontalScroll(newVal);
                this.hScrollBar.Value = newVal;
            }
        }

        #endregion

        #region MouseHover

        /// <summary>
        /// Raises the MouseHover event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{2DD1AA40-E424-4F64-901B-1FEBEBC9CF24}</MetaDataID>
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            if (this.IsValidCell(this.LastMouseCell))
            {
                this.OnCellMouseHover(new CellMouseEventArgs(this.TableModel[this.LastMouseCell], this, this.LastMouseCell, this.CellRect(this.LastMouseCell)));
            }
            else if (this.hotColumn != -1)
            {
                this.OnHeaderMouseHover(new HeaderMouseEventArgs(this.ColumnModel.Columns[this.hotColumn], this, this.hotColumn, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(this.hotColumn))));
            }
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{D2334325-29D0-41DD-BA2B-45722B2CC326}</MetaDataID>
        protected override void OnClick(EventArgs e)
        {
            Focus();
            base.OnClick(e);

            
            if (this.IsValidCell(this.LastMouseCell))
            {
                this.OnCellClick(new CellMouseEventArgs(this.TableModel[this.LastMouseCell], this, this.LastMouseCell, this.CellRect(this.LastMouseCell), e as MouseEventArgs));

            }
            else if (this.hotColumn != -1)
            {
                this.OnHeaderClick(new HeaderMouseEventArgs(this.ColumnModel.Columns[this.hotColumn], this, this.hotColumn, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(this.hotColumn))));
            }
    

        }

        /// <MetaDataID>{0ab14cfb-5a20-43a9-8270-62c2fd96a34a}</MetaDataID>
        protected override void OnCreateControl()
        {
            //_BackColor = BackColor;
            //BackColor = _FocusedBackColor;

            base.OnCreateControl();
        }
        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{61F81C9D-A9BA-4226-800B-CB6C7CE5ECE6}</MetaDataID>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (this.IsValidCell(this.LastMouseCell))
            {
                Rectangle cellRect = this.CellRect(this.LastMouseCell);

                this.OnCellDoubleClick(new CellMouseEventArgs(this.TableModel[this.LastMouseCell], this, this.LastMouseCell, this.CellRect(this.LastMouseCell)));
            }
            else if (this.hotColumn != -1)
            {
                this.OnHeaderDoubleClick(new HeaderMouseEventArgs(this.ColumnModel.Columns[this.hotColumn], this, this.hotColumn, this.DisplayRectToClient(this.ColumnModel.ColumnHeaderRect(this.hotColumn))));
            }
        }

        #endregion

        #endregion

        #region Paint

        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{859D63A6-E7CE-42FD-BDA1-C0CCA0B6CB77}</MetaDataID>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }



        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{41F740CD-1C68-4B41-AB78-354E8064B3BD}</MetaDataID>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
               
                // we'll do our own painting thanks
                //base.OnPaint(e);

                // check if we actually need to paint
                if (this.Width == 0 || this.Height == 0)
                {
                    return;
                }

                if (this.ColumnModel != null)
                {
                    // keep a record of the current clip region
                    Region clip = e.Graphics.Clip;

                    if (this.TableModel != null && this.TableModel.Rows.Count > 0)
                    {
                        this.OnPaintRows(e);

                        // reset the clipping region
                        e.Graphics.Clip = clip;
                    }

                    if (this.GridLines != GridLines.None)
                    {
                        this.OnPaintGrid(e);
                    }

                    if (this.HeaderStyle != ColumnHeaderStyle.None && this.ColumnModel.Columns.Count > 0)
                    {
                        if (this.HeaderRectangle.IntersectsWith(e.ClipRectangle))
                        {
                            this.OnPaintHeader(e);
                        }
                    }

                    // reset the clipping region
                    e.Graphics.Clip = clip;
                }

                this.OnPaintEmptyTableText(e);

                this.OnPaintBorder(e);
            }
            catch (Exception error)
            {


            }

        }


        /// <summary>
        /// Draws a reversible line at the specified screen x-coordinate 
        /// that is the height of the PseudoClientRect
        /// </summary>
        /// <param name="x">The screen x-coordinate of the reversible line 
        /// to be drawn</param>
        /// <MetaDataID>{ED92D71D-D264-4C27-AA8A-764719D2970A}</MetaDataID>
        private void DrawReversibleLine(int x)
        {
            Point start = this.PointToScreen(new Point(x, this.PseudoClientRect.Top));

            ControlPaint.DrawReversibleLine(start, new Point(start.X, start.Y + this.PseudoClientRect.Height), this.BackColor);
        }

        #region Border

        /// <summary>
        /// Paints the Table's border
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{C53F8515-2ADE-4D84-8978-32CEEDD6E0DF}</MetaDataID>
        protected void OnPaintBorder(PaintEventArgs e)
        {
            //e.Graphics.SetClip(e.ClipRectangle);

            if (this.BorderStyle == BorderStyle.Fixed3D)
            {
                if (ThemeManager.VisualStylesEnabled)
                {
                    TextBoxStates state = TextBoxStates.Normal;
                    if (!this.Enabled)
                    {
                        state = TextBoxStates.Disabled;
                    }

                    // draw the left border
                    Rectangle clipRect = new Rectangle(0, 0, SystemInformation.Border3DSize.Width, this.Height);
                    if (clipRect.IntersectsWith(e.ClipRectangle))
                    {
                        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                    }

                    // draw the top border
                    clipRect = new Rectangle(0, 0, this.Width, SystemInformation.Border3DSize.Height);
                    if (clipRect.IntersectsWith(e.ClipRectangle))
                    {
                        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                    }

                    // draw the right border
                    clipRect = new Rectangle(this.Width - SystemInformation.Border3DSize.Width, 0, this.Width, this.Height);
                    if (clipRect.IntersectsWith(e.ClipRectangle))
                    {
                        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                    }

                    // draw the bottom border
                    clipRect = new Rectangle(0, this.Height - SystemInformation.Border3DSize.Height, this.Width, SystemInformation.Border3DSize.Height);
                    if (clipRect.IntersectsWith(e.ClipRectangle))
                    {
                        ThemeManager.DrawTextBox(e.Graphics, this.ClientRectangle, clipRect, state);
                    }
                }
                else
                {
                    ControlPaint.DrawBorder3D(e.Graphics, 0, 0, this.Width, this.Height, Border3DStyle.Sunken);
                }
            }
            else if (this.BorderStyle == BorderStyle.FixedSingle)
            {
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            }

            if (this.HScroll && this.VScroll)
            {
                Rectangle rect = new Rectangle(this.Width - this.BorderWidth - SystemInformation.VerticalScrollBarWidth,
                    this.Height - this.BorderWidth - SystemInformation.HorizontalScrollBarHeight,
                    SystemInformation.VerticalScrollBarWidth,
                    SystemInformation.HorizontalScrollBarHeight);

                if (rect.IntersectsWith(e.ClipRectangle))
                {
                    e.Graphics.FillRectangle(SystemBrushes.Control, rect);
                }
            }
        }

        #endregion

        #region Cells

        /// <summary>
        /// Paints the Cell at the specified row and column indexes
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <param name="row">The index of the row that contains the cell to be painted</param>
        /// <param name="column">The index of the column that contains the cell to be painted</param>
        /// <param name="cellRect">The bounding Rectangle of the Cell</param>
        /// <MetaDataID>{A2E59225-1819-4326-A3B6-7DC2A27486F1}</MetaDataID>
        protected void OnPaintCell(PaintEventArgs e, int row, int column, Rectangle cellRect)
        {
            if (row == 0 && column == 1)
            {
                column = 1;
            }

            // get the renderer for the cells column
            ICellRenderer renderer = this.ColumnModel.Columns[column].Renderer;
            if (renderer == null)
            {
                // get the default renderer for the column
                renderer = this.ColumnModel.GetCellRenderer(this.ColumnModel.Columns[column].GetDefaultRendererName());
            }

            // if the renderer is still null (which it shouldn't)
            // the get out of here
            if (renderer == null)
            {
                return;
            }

            PaintCellEventArgs pcea = new PaintCellEventArgs(e.Graphics, cellRect);
            pcea.Graphics.SetClip(Rectangle.Intersect(e.ClipRectangle, cellRect));

            if (column < this.TableModel.Rows[row].Cells.Count)
            {
                // is the cell selected
                bool selected = false;

                if (this.FullRowSelect)
                {
                    selected = this.TableModel.Selections.IsRowSelected(row);
                }
                else
                {
                    if (this.SelectionStyle == SelectionStyle.ListView)
                    {
                        if (this.TableModel.Selections.IsRowSelected(row) && this.ColumnModel.PreviousVisibleColumn(column) == -1)
                        {
                            selected = true;
                        }
                    }
                    else if (this.SelectionStyle == SelectionStyle.Grid)
                    {
                        if (this.TableModel.Selections.IsCellSelected(row, column))
                        {
                            selected = true;
                        }
                    }
                }

                //
                bool editable = this.TableModel[row, column].Editable && this.TableModel.Rows[row].Editable && this.ColumnModel.Columns[column].Editable;
                bool enabled = this.TableModel[row, column].Enabled && this.TableModel.Rows[row].Enabled && this.ColumnModel.Columns[column].Enabled;

                // draw the cell
                pcea.SetCell(this.TableModel[row, column]);
                pcea.SetRow(row);
                pcea.SetColumn(column);
                pcea.SetTable(this);
                pcea.SetSelected(selected);
                pcea.SetFocused(this.Focused && this.FocusedCell.Row == row && this.FocusedCell.Column == column);
                pcea.SetSorted(column == this.lastSortedColumn);
                pcea.SetEditable(editable);
                pcea.SetEnabled(enabled);
                pcea.SetCellRect(cellRect);
            }
            else
            {
                // there isn't a cell for this column, so send a 
                // null value for the cell and the renderer will 
                // take care of the rest (it should draw an empty cell)

                pcea.SetCell(null);
                pcea.SetRow(row);
                pcea.SetColumn(column);
                pcea.SetTable(this);
                pcea.SetSelected(false);
                pcea.SetFocused(false);
                pcea.SetSorted(false);
                pcea.SetEditable(false);
                pcea.SetEnabled(false);
                pcea.SetCellRect(cellRect);
            }

            // let the user get the first crack at painting the cell
            this.OnBeforePaintCell(pcea);

            // only send to the renderer if the user hasn't 
            // set the handled property
            if (!pcea.Handled)
            {
                renderer.OnPaintCell(pcea);
            }

            // let the user have another go
            this.OnAfterPaintCell(pcea);
        }


        /// <summary>
        /// Raises the BeforePaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{A1E368D6-2752-40EE-8C1F-ECEE9712A442}</MetaDataID>
        protected virtual void OnBeforePaintCell(PaintCellEventArgs e)
        {
            if (BeforePaintCell != null)
            {
                BeforePaintCell(this, e);
            }
        }


        /// <summary>
        /// Raises the AfterPaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{57277FD3-65D2-4BE1-86A7-CDE27E64C880}</MetaDataID>
        protected virtual void OnAfterPaintCell(PaintCellEventArgs e)
        {
            if (AfterPaintCell != null)
            {
                AfterPaintCell(this, e);
            }
        }

        #endregion

        #region Grid

        /// <summary>
        /// Paints the Table's grid
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{E5BA7573-B108-4990-A16C-245BF8C40AD2}</MetaDataID>
        protected void OnPaintGrid(PaintEventArgs e)
        {
            DragDropMarkPos = -1;
            if (this.GridLines == GridLines.None)
            {
                return;
            }

            //
            //e.Graphics.SetClip(e.ClipRectangle);

            if (this.ColumnModel == null || this.ColumnModel.Columns.Count == 0)
            {
                return;
            }

            //e.Graphics.SetClip(e.ClipRectangle);

            if (this.ColumnModel != null)
            {
                using (Pen gridPen = new Pen(this.GridColor))
                {
                    //
                    gridPen.DashStyle = (DashStyle)this.GridLineStyle;

                    // check if we can draw column lines
                    if ((this.GridLines & GridLines.Columns) == GridLines.Columns)
                    {
                        int right = this.DisplayRectangle.X;

                        for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
                        {
                            if (this.ColumnModel.Columns[i].Visible)
                            {
                                right += this.ColumnModel.Columns[i].Width;

                                if (right >= e.ClipRectangle.Left && right <= e.ClipRectangle.Right)
                                {
                                    e.Graphics.DrawLine(gridPen, right - 1, e.ClipRectangle.Top, right - 1, e.ClipRectangle.Bottom);
                                }
                            }
                        }
                    }

                    if (this.TableModel != null)
                    {
                        // check if we can draw row lines
                        if ((this.GridLines & GridLines.Rows) == GridLines.Rows)
                        {
                            int y = this.CellDataRect.Y + this.RowHeight - 1;

                            for (int i = y; i <= e.ClipRectangle.Bottom; i += this.RowHeight)
                            {
                                if (i >= this.CellDataRect.Top)
                                {
                                    e.Graphics.DrawLine(gridPen, e.ClipRectangle.Left, i, e.ClipRectangle.Right, i);
                                }
                            }

                        }
                        if (DragDropEffect != DragDropEffects.None && RectangleToScreen(this.CellDataRect).Contains(System.Windows.Forms.Control.MousePosition))
                        {
                            //using (Pen cursorPen = new Pen(_CursorColor,_CursorWidth))
                            //{
                            int y = this.CellDataRect.Y + this.RowHeight - 1;

                            for (int i = y; i <= e.ClipRectangle.Bottom; i += this.RowHeight)
                            {
                                if (i >= this.CellDataRect.Top && i >= PointToClient(System.Windows.Forms.Control.MousePosition).Y)
                                {
                                    int scrollBarWidth = 0;
                                    if (this.vScrollBar.Visible)
                                        scrollBarWidth = this.vScrollBar.Width;
                                    //int leftCursorMatgin = (int)(_CursorWidth / 2)+2;
                                    //int rightCursorMatgin = (int)(_CursorWidth / 2) + 2 + scrollBarWidth;
                                    //if (_CursorWidth % 2 > 0)
                                    //    rightCursorMatgin++;

                                    e.Graphics.DrawLine(_markPen, e.ClipRectangle.Left + DragDropMarkWidth + 3, i, e.ClipRectangle.Right - scrollBarWidth - DragDropMarkWidth - 3, i);
                                    DragDropMarkPos = i / this.RowHeight;

                                    //   e.Graphics.DrawLine(cursorPen, e.ClipRectangle.Left + leftCursorMatgin, i - (_CursorWidth + 3), e.ClipRectangle.Left + leftCursorMatgin , i + (_CursorWidth + 3));
                                    ////   rightCursorMatgin = 27;
                                    //   e.Graphics.DrawLine(cursorPen, e.ClipRectangle.Right - rightCursorMatgin,i- (_CursorWidth + 3), e.ClipRectangle.Right - rightCursorMatgin,i+ (_CursorWidth + 3));
                                    break;
                                }
                            }
                            //}
                        }


                    }
                }
            }
        }

        #endregion

        #region Header

        /// <summary>
        /// Paints the Table's Column headers
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{360E4B41-60BA-4855-BC64-595B2B6F3C1B}</MetaDataID>
        protected void OnPaintHeader(PaintEventArgs e)
        {
            // only bother if we actually get to paint something
            if (!this.HeaderRectangle.IntersectsWith(e.ClipRectangle))
            {
                return;
            }

            int xPos = this.DisplayRectangle.Left;
            bool needDummyHeader = true;

            //
            PaintHeaderEventArgs phea = new PaintHeaderEventArgs(e.Graphics, e.ClipRectangle);

            for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
            {
                // check that the column isn't hidden
                if (this.ColumnModel.Columns[i].Visible)
                {
                    Rectangle colHeaderRect = new Rectangle(xPos, this.BorderWidth, this.ColumnModel.Columns[i].Width, this.HeaderHeight);

                    // check that the column intersects with the clipping rect
                    if (e.ClipRectangle.IntersectsWith(colHeaderRect))
                    {
                        // move and resize the headerRenderer
                        this.headerRenderer.Bounds = new Rectangle(xPos, this.BorderWidth, this.ColumnModel.Columns[i].Width, this.HeaderHeight);

                        // set the clipping area to the header renderers bounds
                        phea.Graphics.SetClip(Rectangle.Intersect(e.ClipRectangle, this.headerRenderer.Bounds));

                        // draw the column header
                        phea.SetColumn(this.ColumnModel.Columns[i]);
                        phea.SetColumnIndex(i);
                        phea.SetTable(this);
                        phea.SetHeaderStyle(this.HeaderStyle);
                        phea.SetHeaderRect(this.headerRenderer.Bounds);

                        // let the user get the first crack at painting the header
                        this.OnBeforePaintHeader(phea);

                        // only send to the renderer if the user hasn't 
                        // set the handled property
                        if (!phea.Handled)
                        {
                            this.headerRenderer.OnPaintHeader(phea);
                        }

                        // let the user have another go
                        this.OnAfterPaintHeader(phea);
                    }

                    // set the next column start position
                    xPos += this.ColumnModel.Columns[i].Width;

                    // if the next start poition is past the right edge
                    // of the clipping rectangle then we don't need to
                    // draw anymore
                    if (xPos >= e.ClipRectangle.Right)
                    {
                        return;
                    }

                    // check is the next column position is past the
                    // right edge of the table.  if it is, get out of
                    // here as we don't need to draw anymore columns
                    if (xPos >= this.ClientRectangle.Width)
                    {
                        needDummyHeader = false;

                        break;
                    }
                }
            }

            if (needDummyHeader)
            {
                // move and resize the headerRenderer
                this.headerRenderer.Bounds = new Rectangle(xPos, this.BorderWidth, this.ClientRectangle.Width - xPos + 2, this.HeaderHeight);

                phea.Graphics.SetClip(Rectangle.Intersect(e.ClipRectangle, this.headerRenderer.Bounds));

                phea.SetColumn(null);
                phea.SetColumnIndex(-1);
                phea.SetTable(this);
                phea.SetHeaderStyle(this.HeaderStyle);
                phea.SetHeaderRect(this.headerRenderer.Bounds);

                // let the user get the first crack at painting the header
                this.OnBeforePaintHeader(phea);

                // only send to the renderer if the user hasn't 
                // set the handled property
                if (!phea.Handled)
                {
                    this.headerRenderer.OnPaintHeader(phea);
                }

                // let the user have another go
                this.OnAfterPaintHeader(phea);
            }
        }


        /// <summary>
        /// Raises the BeforePaintHeader event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{60CF8972-3F5F-4ABE-8F0E-0C703879B26C}</MetaDataID>
        protected virtual void OnBeforePaintHeader(PaintHeaderEventArgs e)
        {
            if (BeforePaintHeader != null)
            {
                BeforePaintHeader(this, e);
            }
        }


        /// <summary>
        /// Raises the AfterPaintHeader event
        /// </summary>
        /// <param name="e">A PaintHeaderEventArgs that contains the event data</param>
        /// <MetaDataID>{3F393A1F-A501-4EBF-ADDB-FA0EA40D5D29}</MetaDataID>
        protected virtual void OnAfterPaintHeader(PaintHeaderEventArgs e)
        {
            if (AfterPaintHeader != null)
            {
                AfterPaintHeader(this, e);
            }
        }

        #endregion

        #region Rows

        /// <summary>
        /// Paints the Table's Rows
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{E950F81F-6CE1-49CC-93A2-B249AB21684B}</MetaDataID>
        protected void OnPaintRows(PaintEventArgs e)
        {
            int xPos = this.DisplayRectangle.Left;
            int yPos = this.PseudoClientRect.Top;

            if (this.HeaderStyle != ColumnHeaderStyle.None)
            {
                yPos += this.HeaderHeight;
            }

            Rectangle rowRect = new Rectangle(xPos, yPos, this.ColumnModel.TotalColumnWidth, this.RowHeight);

            for (int i = this.TopIndex; i < Math.Min(this.TableModel.Rows.Count, this.TopIndex + this.VisibleRowCount + 1); i++)
            {
                if (rowRect.IntersectsWith(e.ClipRectangle))
                {
                    this.OnPaintRow(e, i, rowRect);
                }
                else if (rowRect.Top > e.ClipRectangle.Bottom)
                {
                    break;
                }

                // move to the next row
                rowRect.Y += this.RowHeight;
            }

            //
            if (this.IsValidColumn(this.lastSortedColumn))
            {
                if (rowRect.Y < this.PseudoClientRect.Bottom)
                {
                    Rectangle columnRect = this.ColumnRect(this.lastSortedColumn);
                    columnRect.Y = rowRect.Y;
                    columnRect.Height = this.PseudoClientRect.Bottom - rowRect.Y;

                    if (columnRect.IntersectsWith(e.ClipRectangle))
                    {
                        columnRect.Intersect(e.ClipRectangle);

                        e.Graphics.SetClip(columnRect);

                        using (SolidBrush brush = new SolidBrush(this.SortedColumnBackColor))
                        {
                            e.Graphics.FillRectangle(brush, columnRect);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Paints the Row at the specified index
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <param name="row">The index of the Row to be painted</param>
        /// <param name="rowRect">The bounding Rectangle of the Row to be painted</param>
        /// <MetaDataID>{6E4EB320-E388-4775-913B-CCDEF34B2CF0}</MetaDataID>
        protected void OnPaintRow(PaintEventArgs e, int row, Rectangle rowRect)
        {
            Rectangle cellRect = new Rectangle(rowRect.X, rowRect.Y, 0, rowRect.Height);

            //e.Graphics.SetClip(rowRect);

            for (int i = 0; i < this.ColumnModel.Columns.Count; i++)
            {
                if (this.ColumnModel.Columns[i].Visible)
                {
                    cellRect.Width = this.ColumnModel.Columns[i].Width;

                    if (cellRect.IntersectsWith(e.ClipRectangle))
                    {
                        this.OnPaintCell(e, row, i, cellRect);
                    }
                    else if (cellRect.Left > e.ClipRectangle.Right)
                    {
                        break;
                    }

                    cellRect.X += this.ColumnModel.Columns[i].Width;
                }
            }
        }

        #endregion

        #region Empty Table Text

        /// <summary>
        /// Paints the message that is displayed when the Table doen't 
        /// contain any items
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data</param>
        /// <MetaDataID>{0101C2AE-181C-47DA-84B0-1C9CD8691B5E}</MetaDataID>
        protected void OnPaintEmptyTableText(PaintEventArgs e)
        {
            if (this.ColumnModel == null || this.RowCount == 0)
            {
                Rectangle client = this.CellDataRect;

                client.Y += 10;
                client.Height -= 10;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;

                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    if (this.DesignMode)
                    {
                        if (this.ColumnModel == null || this.TableModel == null)
                        {
                            string text = null;

                            if (this.ColumnModel == null)
                            {
                                if (this.TableModel == null)
                                {
                                    text = "Table does not have a ColumnModel or TableModel";
                                }
                                else
                                {
                                    text = "Table does not have a ColumnModel";
                                }
                            }
                            else if (this.TableModel == null)
                            {
                                //text = "Table does not have a TableModel";
                                if (ListConnection.CollectionObjectType == null)
                                    text = "There isn't collection type\n";
                                foreach (Column column in ColumnModel.Columns)
                                {
                                    if (column.ValueType == null)
                                        text += "Column '" + column.Text + "' has lose the connection.\n";

                                }


                                //System.Type tt = columnModel.Columns[1].ValuesType;

                            }

                            e.Graphics.DrawString(text, this.Font, brush, client, format);
                        }
                        else if (this.TableModel != null && this.TableModel.Rows.Count == 0)
                        {
                            if (this.NoItemsText != null && this.NoItemsText.Length > 0)
                            {
                                e.Graphics.DrawString(this.NoItemsText, this.Font, brush, client, format);
                            }
                        }
                    }
                    else
                    {
                        if (this.NoItemsText != null && this.NoItemsText.Length > 0)
                        {
                            if (DesignMode)
                                e.Graphics.DrawString(this.NoItemsText, this.Font, brush, client, format);
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Rows

        /// <summary>
        /// Raises the RowPropertyChanged event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{E34BE75B-274D-4F08-8659-D76A44BEBDA9}</MetaDataID>
        protected internal virtual void OnRowPropertyChanged(RowEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateRow(e.Index);

                if (RowPropertyChanged != null)
                {
                    RowPropertyChanged(e.Row, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellAdded event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{9619380E-B325-488D-BB29-05AC080D8D09}</MetaDataID>
        protected internal virtual void OnCellAdded(RowEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateRow(e.Index);

                if (CellAdded != null)
                {
                    CellAdded(e.Row, e);
                }
            }
        }


        /// <summary>
        /// Raises the CellRemoved event
        /// </summary>
        /// <param name="e">A RowEventArgs that contains the event data</param>
        /// <MetaDataID>{D42F284C-339C-4C91-846A-D0BC0E532449}</MetaDataID>
        protected internal virtual void OnCellRemoved(RowEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.InvalidateRow(e.Index);

                if (CellRemoved != null)
                {
                    CellRemoved(this, e);
                }

                if (e.CellFromIndex == -1 && e.CellToIndex == -1)
                {
                    if (this.FocusedCell.Row == e.Index)
                    {
                        this.focusedCell = CellPos.Empty;
                    }
                }
                else
                {
                    for (int i = e.CellFromIndex; i <= e.CellToIndex; i++)
                    {
                        if (this.FocusedCell.Row == e.Index && this.FocusedCell.Column == i)
                        {
                            this.focusedCell = CellPos.Empty;

                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Scrollbars

        /// <summary>
        /// Occurs when the Table's horizontal scrollbar is scrolled
        /// </summary>
        /// <param name="sender">The object that Raised the event</param>
        /// <param name="e">A ScrollEventArgs that contains the event data</param>
        /// <MetaDataID>{D84FF003-2E80-4E27-B2AA-BF030461EBEF}</MetaDataID>
        protected void OnHorizontalScroll(object sender, ScrollEventArgs e)
        {
            // stop editing as the editor doesn't move while 
            // the table scrolls
            if (this.IsEditing)
            {
                this.StopEditing();
            }

            if (this.CanRaiseEvents)
            {
                // non-solid row lines develop artifacts while scrolling 
                // with the thumb so we invalidate the table once thumb 
                // scrolling has finished to make them look nice again
                if (e.Type == ScrollEventType.ThumbPosition)
                {
                    if (this.GridLineStyle != GridLineStyle.Solid)
                    {
                        if (this.GridLines == GridLines.Rows || this.GridLines == GridLines.Both)
                        {
                            this.Invalidate(this.CellDataRect, false);
                        }
                    }

                    // same with the focus rect
                    if (this.FocusedCell != CellPos.Empty)
                    {
                        this.Invalidate(this.CellRect(this.FocusedCell), false);
                    }
                }
                else
                {
                    this.HorizontalScroll(e.NewValue);
                }
            }
        }


        /// <summary>
        /// Occurs when the Table's vertical scrollbar is scrolled
        /// </summary>
        /// <param name="sender">The object that Raised the event</param>
        /// <param name="e">A ScrollEventArgs that contains the event data</param>
        /// <MetaDataID>{05003D41-C34F-401D-8A95-32CB30B03B2F}</MetaDataID>
        protected void OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            // stop editing as the editor doesn't move while 
            // the table scrolls
            if (this.IsEditing)
            {
                this.StopEditing();
            }

            if (this.CanRaiseEvents)
            {
                // non-solid column lines develop artifacts while scrolling 
                // with the thumb so we invalidate the table once thumb 
                // scrolling has finished to make them look nice again
                if (e.Type == ScrollEventType.ThumbPosition)
                {
                    if (this.GridLineStyle != GridLineStyle.Solid)
                    {
                        if (this.GridLines == GridLines.Columns || this.GridLines == GridLines.Both)
                        {
                            this.Invalidate(this.CellDataRect, false);
                        }
                    }
                }
                else
                {
                    this.VerticalScroll(e.NewValue);
                }
            }
        }


        /// <summary>
        /// Handler for a ScrollBars GotFocus event
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{9AAC450E-298F-4DFC-8D7A-E1056F430493}</MetaDataID>
        private void scrollBar_GotFocus(object sender, EventArgs e)
        {
            // don't let the scrollbars have focus 
            // (appears to slow scroll speed otherwise)
            this.Focus();
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Raises the BeginSort event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{8CE53320-3E35-4DD7-9C0B-19A207F9CFFF}</MetaDataID>
        protected virtual void OnBeginSort(ColumnEventArgs e)
        {
            if (BeginSort != null)
            {
                BeginSort(this, e);
            }
        }


        /// <summary>
        /// Raises the EndSort event
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains the event data</param>
        /// <MetaDataID>{537CCBFC-707A-47B7-AAC6-3DA06A3AED75}</MetaDataID>
        protected virtual void OnEndSort(ColumnEventArgs e)
        {
            if (EndSort != null)
            {
                EndSort(this, e);
            }
        }

        #endregion

        #region TableModel

        /// <summary>
        /// Raises the TableModelChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{56C7CC39-A7CB-4670-9FE3-0175D2BCB5B2}</MetaDataID>
        protected internal virtual void OnTableModelChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (TableModelChanged != null)
                {
                    TableModelChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the SelectionChanged event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{F08F87D3-CD70-4CC3-A454-01B7845865D6}</MetaDataID>
        protected internal virtual void OnSelectionChanged(SelectionEventArgs e)
        {
            if (this.EditingCell != CellPos.Empty)
            {
                // don't bother if we're already editing the cell.  
                // if we're editing a different cell stop editing

                this.EditingCellEditor.StopEditing();
                this.editingCell = CellPos.Empty;
            }




            if (this.CanRaiseEvents)
            {
                if (e.OldSelectionBounds != Rectangle.Empty)
                {
                    Rectangle invalidateRect = new Rectangle(this.DisplayRectToClient(e.OldSelectionBounds.Location), e.OldSelectionBounds.Size);

                    if (this.HeaderStyle != ColumnHeaderStyle.None)
                    {
                        invalidateRect.Y += this.HeaderHeight;
                    }

                    this.Invalidate(invalidateRect);
                }

                if (e.NewSelectionBounds != Rectangle.Empty)
                {
                    Rectangle invalidateRect = new Rectangle(this.DisplayRectToClient(e.NewSelectionBounds.Location), e.NewSelectionBounds.Size);

                    if (this.HeaderStyle != ColumnHeaderStyle.None)
                    {
                        invalidateRect.Y += this.HeaderHeight;
                    }

                    this.Invalidate(invalidateRect);
                }
                ListConnection.SelectionChanged();
           
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowHeightChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <MetaDataID>{121E4D8C-D2B9-48E0-A010-FA3D1E74FFD2}</MetaDataID>
        protected internal virtual void OnRowHeightChanged(EventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (RowHeightChanged != null)
                {
                    RowHeightChanged(this, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowAdded event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{66608B80-A9DB-458C-8664-51451BFE680E}</MetaDataID>
        protected internal virtual void OnRowAdded(TableModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (RowAdded != null)
                {
                    RowAdded(e.TableModel, e);
                }
            }
        }


        /// <summary>
        /// Raises the RowRemoved event
        /// </summary>
        /// <param name="e">A TableModelEventArgs that contains the event data</param>
        /// <MetaDataID>{05278530-B6E0-48DD-AC26-21DED5580967}</MetaDataID>
        protected internal virtual void OnRowRemoved(TableModelEventArgs e)
        {
            if (this.CanRaiseEvents)
            {
                this.PerformLayout();
                this.Invalidate();

                if (RowRemoved != null)
                {
                    RowRemoved(e.TableModel, e);
                }
            }
        }

        #endregion

        #endregion




    
    }


    
   
}
