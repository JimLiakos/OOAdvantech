using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.UserInterface.Runtime;

namespace ConnectableControls.ListView
{
    /// <MetaDataID>{ffbbb787-be7e-4677-bed6-0a4f821b1735}</MetaDataID>
    public interface IListView
    {
        /// <MetaDataID>{e4d328d0-a454-46ae-9162-b4ae0deb64c1}</MetaDataID>
        System.Collections.Generic.List<IColumn> Columns
        {
            get;
        }
        /// <MetaDataID>{d704321c-8280-4e91-be1f-38b7506c8768}</MetaDataID>
        string Name
        {
            get;
        }
        //
        // Summary:
        //     Occurs when a View is clicked.
        event EventHandler Click;

        /// <MetaDataID>{5445c67f-f8f1-47c5-87e6-f14765b99d13}</MetaDataID>
        void RemoveColumn(IColumn column);
        /// <MetaDataID>{79a8ed6c-c623-446f-8bc1-14f49d89de85}</MetaDataID>
        void AddColumn(IColumn column);
       // IColumn AddColumn(string path);
        //Object MetaData
        //{
        //    get;
        //    set;
        //}
        /// <MetaDataID>{e4cb9035-e094-4468-a3d6-b4c7f86e0774}</MetaDataID>
        IColumn ChangeColumnType(IColumn selectedColumn, string columnType);

        /// <MetaDataID>{1bff34b2-d5e6-442f-ab47-7a235c197ab7}</MetaDataID>
        int[] SelectedRowsIndicies
        {
            get;
        }
        /// <MetaDataID>{ea9ebcf5-f756-4d6d-bd08-5351254d2a9c}</MetaDataID>
        void SelectRows(List< IRow> rows);
        /// <MetaDataID>{73d4aff7-676e-49dd-9f4f-c5fc0c248dd2}</MetaDataID>
        void SelectRows(List<int> rowsIndicies);

        /// <MetaDataID>{a9bdc5da-0cfc-4bf5-81db-52e3d93fa96c}</MetaDataID>
        void SelectRow(IRow row);
        /// <MetaDataID>{4fd51822-4e70-48c3-bfc5-9585dfe7fc4e}</MetaDataID>
        void SelectRow(int rowsIndex);


        /// <MetaDataID>{bb59b5fd-9cdd-4616-acc6-4303390c67cc}</MetaDataID>
        List<IRow> SelectedRows
        {
            get;
        }
        /// <MetaDataID>{9e9ad6ce-91ae-4a7c-a9e3-124fbc16b038}</MetaDataID>
        bool DataSourceSupported
        {
            get;
        }
        /// <MetaDataID>{999769ed-6f7f-4f75-9520-0359aec19112}</MetaDataID>
        object DataSource
        {
            get;
            set;
        }
        /// <MetaDataID>{fd5c4041-34b4-4263-bd80-73d1c5d43ec3}</MetaDataID>
        void RefreshDataSource();

        /// <MetaDataID>{c007041b-4ad4-4bd2-98e2-bd02b2e81a38}</MetaDataID>
        List<IRow> Rows
        {
            get;
        }

        /// <MetaDataID>{ef2e8cf7-bb24-4fa8-9b27-c72e7b39b924}</MetaDataID>
        IRow InsertRow();

        /// <MetaDataID>{18de1788-3805-4c6c-a37f-b69d6ed2152b}</MetaDataID>
        IRow LastMouseOverRow
        {
            get;
        }
        /// <MetaDataID>{2c2de1ed-b596-46cf-902f-19433c00e1fa}</MetaDataID>
        void RemoveAllRows();
        /// <MetaDataID>{57508c3f-f471-4d57-9981-87139275c960}</MetaDataID>
        void RefreshRow(int rowHandle);
        /// <MetaDataID>{314f1bd8-cd6c-42bd-ab6e-032842683705}</MetaDataID>
        void RefreshRowCell(int rowHandle, IColumn column);

        /// <MetaDataID>{a51a787b-5728-4ce5-a72b-f21a9ac76ae0}</MetaDataID>
        void RemoveRowAt(int index);
        /// <MetaDataID>{2461c466-ff48-4109-9ac1-64d66e056427}</MetaDataID>
        IRow InsertRow(int index, object displayedObj, IPresentationObject displayedPresentationObj);
        /// <MetaDataID>{0b7ec5c5-eb02-47d9-8052-d31fa39c2b56}</MetaDataID>
        IColumn AddColumn(OOAdvantech.UserInterface.Column column);
        /// <MetaDataID>{5ceeea9c-f7c2-4755-a10b-1f4b520143ab}</MetaDataID>
        List<string> GetColumnTypesNames();
        /// <MetaDataID>{308b5367-8ac1-4a17-a97d-a48136bd079e}</MetaDataID>
        string GetColumnTypeName(IColumn SelectedColumn);
        /// <MetaDataID>{5ac96e10-e958-4548-905e-054042496d25}</MetaDataID>
        ListConnection ListConnection
        {
            get;
        }
        //MetaDataValue LoadListOperationCall
        //{
        //    get;
        //    set;
        //}
        //MetaDataValue EditRowOperationCall
        //{
        //    get;
        //    set;
        //}
        //MetaDataValue DeleteRowOperationCall
        //{
        //    get;
        //    set;
        //}
        //MetaDataValue AllowDropOperationCall
        //{
        //    get;
        //    set;
        //}
        //MetaDataValue DragDropOperationCall
        //{
        //    get;
        //    set;
        //}
        //MetaDataValue InsertRowOperationCall
        //{
        //    get;
        //    set;
        //}


    }
}
