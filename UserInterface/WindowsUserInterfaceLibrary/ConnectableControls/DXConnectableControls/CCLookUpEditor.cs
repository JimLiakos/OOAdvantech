using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DXConnectableControls.XtraEditors.Repository;
using ConnectableControls;
using ConnectableControls.PropertyEditors;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Controls;
using System.Reflection.Emit;
using ConnectableControls.ListView;

namespace DXConnectableControls.XtraEditors.Repository
{

    /// <MetaDataID>{c0c684fd-770f-4c4d-b96f-9c7df6702a02}</MetaDataID>
    internal interface IDataSourceLoader
    {
        void LoadDataSource();
    }
    /// <MetaDataID>{4f4614e7-f503-4a40-acfd-626df85eabfa}</MetaDataID>
    [UserRepositoryItem("Register")]
    public class CCRepositoryItemLookUpEdit : RepositoryItemLookUpEdit, IDataSourceLoader//OOAdvantech.UserInterface.Runtime.IOperetionCallerSource,
    {
        public override void PopulateColumns()
        {
            base.PopulateColumns();
        }

        static CCRepositoryItemLookUpEdit()
        {
            Register();
        }
     
       
        public CCRepositoryItemLookUpEdit() 
        {
        }

        internal const string EditorName = "CCLookUpEdit";

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(CCLookUpEdit),
                typeof(CCRepositoryItemLookUpEdit), typeof(DevExpress.XtraEditors.ViewInfo.LookUpEditViewInfo),
                new DevExpress.XtraEditors.Drawing.ButtonEditPainter(), true, null));

        }
        public override string EditorTypeName
        {
            get { return EditorName; }
        }
   
        public override string GetDisplayText(DevExpress.Utils.FormatInfo format, object editValue)
        {
            if (!DesignMode && DataSource==null)
            {
                if (OperationCaller != null && OperationCaller.Operation != null && OperationCaller.Operation.ReturnType!=null)
                {
                    Type returnType = OperationCaller.Operation.ReturnType.GetExtensionMetaObject(typeof(Type)) as Type;
                    returnType = returnType.GetGenericArguments()[0];
                    if(returnType!=null)
                        DataSource = Activator.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(returnType));
                }
            }

            return base.GetDisplayText(format, editValue);

        }
      
        public override object GetDisplayValueByKeyValue(object keyValue)
        {
            if (DesignMode || GridColumn == null)
                return base.GetDisplayValueByKeyValue(keyValue);
            else
            {
                OOAdvantech.MetaDataRepository.Classifier classifier = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(GridColumn.ColumnType);
                bool returnValuesAsCollection = false;
                object value = _UserInterfaceObjectConnection.GetDisplayedValue(keyValue, classifier, DisplayMember, null, out returnValuesAsCollection);
                return value;
            }
        }
      
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection;

        [Category("Object Model Connection")]
        public ViewControlObject ViewControlObject
        {
            get
            {
                if (_UserInterfaceObjectConnection == null)
                    return null;
                return _UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject;
            }
            set
            {
                if (value != null)
                    _UserInterfaceObjectConnection = value.UserInterfaceObjectConnection;
                else
                    _UserInterfaceObjectConnection = null;
            }
        }

        OOAdvantech.UserInterface.Runtime.OperationCaller _OperationCaller;
        OOAdvantech.UserInterface.Runtime.OperationCaller OperationCaller
        {
            get
            {

                if ((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData == null || (((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor is OOAdvantech.UserInterface.LookUpEditor) && ((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor as OOAdvantech.UserInterface.LookUpEditor).SearchOperation == null || _UserInterfaceObjectConnection == null))
                    return null;
                if (_OperationCaller != null)
                    return _OperationCaller;
                _OperationCaller = new OOAdvantech.UserInterface.Runtime.OperationCaller(((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor as OOAdvantech.UserInterface.LookUpEditor).SearchOperation, (GridColumn as ConnectableControls.ListView.IColumn).Owner.ListConnection);
                return _OperationCaller;
            }

        }

        public override BaseEdit CreateEditor()
        {
            BaseEdit baseEdit=  base.CreateEditor();

            (baseEdit.Properties as DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit).GridColumn = GridColumn;
            (baseEdit.Properties as DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit).ViewControlObject = ViewControlObject;
            (baseEdit as CCLookUpEdit).DataSourceLoader = this;
            
            (DataSource as System.Collections.IList).Add(null);
            return baseEdit;
        }

        /// <exclude>Excluded</exclude>
        Type _CollectionObjectsProxyType;
        Type CollectionObjectsProxyType
        {
            get
            {
                if (_CollectionObjectsProxyType == null)
                {
                    TypeBuilder typeBuilder = RecordProxy.GetInterfaceTypeBuilder("IGridViewCollectionObject" + GetHashCode());
                    foreach (LookUpColumnInfo column in this.Columns)
                    {
                        Type propertyType = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetType( OperationCaller.Operation.ReturnType.GetExtensionMetaObject(typeof(Type)) as Type,column.FieldName);
                        RecordProxy.CreateProperty(typeBuilder, propertyType, column.FieldName);
                    }
                    _CollectionObjectsProxyType = typeBuilder.CreateType();
                }
                return _CollectionObjectsProxyType;
            }
        }


        void IDataSourceLoader.LoadDataSource()
        {
            object mas = OperationCaller.Invoke();
            (DataSource as System.Collections.IList).Clear();
            (DataSource as System.Collections.IList).Add(null);
            foreach (object obj in (mas as System.Collections.IEnumerable))
                (DataSource as System.Collections.IList).Add(obj);
        }



        [Browsable(true)]
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object LoadSelections
        {
            get
            {
                if (GridColumn != null && ((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor as OOAdvantech.UserInterface.LookUpEditor).SearchOperation == null)
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData);
                    ((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor as OOAdvantech.UserInterface.LookUpEditor).SearchOperation = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                string xml = null;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (OperationCaller == null || _OperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, null);
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(xml, _OperationCaller.Operation.Name);

                metaDataVaue.MetaDataAsObject = ((GridColumn as ConnectableControls.ListView.IColumn).ColumnMetaData.Editor as OOAdvantech.UserInterface.LookUpEditor).SearchOperation;
                return metaDataVaue;
            }
            set
            {
                _OperationCaller = null;
                return;
            }
        }

        internal DevExpress.XtraGrid.Columns.GridColumn GridColumn;

        //#region IOperetionCallerSource Members

        //string[] OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.PropertiesNames
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //object OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.GetPropertyValue(string propertyName)
        //{
        //    throw new NotImplementedException();
        //}

        //void OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.SetPropertyValue(string propertyName, object value)
        //{
        //    throw new NotImplementedException();
        //}

        //OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.GetPropertyType(string propertyName)
        //{
        //    throw new NotImplementedException();
        //}

        //bool OOAdvantech.UserInterface.Runtime.IOperetionCallerSource.ContainsProperty(string propertyName)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        //#region IConnectableControl Members

        //OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IConnectableControl.UserInterfaceObjectConnection
        //{
        //    get
        //    {
        //        return _UserInterfaceObjectConnection;
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //object OOAdvantech.UserInterface.Runtime.IConnectableControl.UIMetaDataObject
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //bool OOAdvantech.UserInterface.Runtime.IConnectableControl.IsPropertyReadOnly(string propertyName)
        //{
        //    if (GridColumn == null || GridColumn.ColumnEdit != this)
        //    {
        //        GridColumn = null;
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        //#endregion

        //#region IMetadataSelectionResolver Members

        //bool OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver.CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion


    }
}
namespace DXConnectableControls.XtraEditors
{
    /// <summary>
    /// MyButtonEdit is a descendant from ButtonEdit.
    /// It displays a dialog form below the text box when the edit button is clicked.
    /// </summary>
    /// <MetaDataID>{8b9c72d7-653c-48bb-a9f6-19bbb617bde1}</MetaDataID>
    public class CCLookUpEdit : LookUpEdit
    {
 
        static CCLookUpEdit()
        {
            CCRepositoryItemLookUpEdit.Register();
        }
        public CCLookUpEdit() { }

        internal IDataSourceLoader DataSourceLoader;
        public override string EditorTypeName
        {
            get { return CCRepositoryItemLookUpEdit.EditorName; }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new CCRepositoryItemLookUpEdit Properties
        {
            get { return base.Properties as CCRepositoryItemLookUpEdit; }
        }
        protected override void CreateRepositoryItem()
        {
            base.CreateRepositoryItem();
        }

        protected override DevExpress.XtraEditors.Popup.PopupBaseForm CreatePopupForm()
        {
            DevExpress.XtraEditors.Popup.PopupBaseForm mer= base.CreatePopupForm();
            return mer;
        }
        protected override void DoShowPopup()
        {
            if (DataSourceLoader != null)
                DataSourceLoader.LoadDataSource();

            //(Properties.DataSource as System.Collections.Generic.List<int>).Add((Properties.DataSource as System.Collections.Generic.List<int>).Count + 1);
            base.DoShowPopup();
        }
        
       
        protected virtual void ShowPopupForm()
        {
            using (Form form = new Form())
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = this.PointToScreen(new Point(0, Height));
                form.ShowDialog();
            }
        }
    }
}

