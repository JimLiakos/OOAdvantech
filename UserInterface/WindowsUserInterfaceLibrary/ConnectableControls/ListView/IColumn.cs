using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ConnectableControls.ListView
{
    /// <MetaDataID>{c332e15b-fc42-431f-99a3-7cf032c713e2}</MetaDataID>
    [ToolboxItem(false)]
    public class ColumnConnection : System.ComponentModel.Component, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }

        public virtual void InitializeControl()
        {

        }
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            throw new NotImplementedException();
        }
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        #region IObjectMemberViewControl Members

        string OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.AllowDrag
        {
            get { throw new NotImplementedException(); }
        }

        bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.AllowDrop
        {
            get { throw new NotImplementedException(); }
        }

        bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            throw new NotImplementedException();
        }

        //OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.GetClassifierFor(ITypeDescriptorContext context)
        //{
        //    throw new NotImplementedException();
        //}

    

        //object OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.Value
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

        //OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.ValueType
        //{
        //    get { throw new NotImplementedException(); }
        //}

     

    

        #endregion

        #region IOperetionCallerSource Members

        string[] OOAdvantech.UserInterface.Runtime.IOperationCallerSource.PropertiesNames
        {
            get { throw new NotImplementedException(); }
        }

        object OOAdvantech.UserInterface.Runtime.IOperationCallerSource.GetPropertyValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        void OOAdvantech.UserInterface.Runtime.IOperationCallerSource.SetPropertyValue(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IOperationCallerSource.GetPropertyType(string propertyName)
        {
            throw new NotImplementedException();
        }

        bool OOAdvantech.UserInterface.Runtime.IOperationCallerSource.ContainsProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConnectableControl Members

        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IConnectableControl.UserInterfaceObjectConnection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

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

        bool OOAdvantech.UserInterface.Runtime.IConnectableControl.IsPropertyReadOnly(string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMetadataSelectionResolver Members

        bool OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver.CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPathDataDisplayer Members

        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.UserInterfaceObjectConnection
        {
            get { throw new NotImplementedException(); }
        }

        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get { throw new NotImplementedException(); }
        }

        bool OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.HasLockRequest
        {
            get { throw new NotImplementedException(); }
        }

        void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            throw new NotImplementedException();
        }

        void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.LockStateChange(object sender)
        {
            throw new NotImplementedException();
        }

        #endregion

        OOAdvantech.UserInterface.Column ColumnMetaData;
        public ColumnConnection(OOAdvantech.UserInterface.Column columnMetaData)
        {
            ColumnMetaData=columnMetaData;
        }

        #region IPathDataDisplayer Members

        public object Path
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void LoadControlValues()
        {
            throw new NotImplementedException();
        }

        public void SaveControlValues()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    /// <MetaDataID>{38a8ac49-b156-4847-9b45-7d2ba882b296}</MetaDataID>
    public interface IColumn //: OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        /// <MetaDataID>{4ba85b93-a728-4aac-99fa-b74772552835}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> Paths
        {
            get;
        }
        /// <MetaDataID>{cfaab468-a64e-408b-b14f-590a996a0111}</MetaDataID>
        object Path
        {
            get;
            set;
        }

        /// <MetaDataID>{c0535cde-c8d4-4d4c-bc81-0a68360c4757}</MetaDataID>
        int Order
        {
            get;
            set;
        }
        /// <MetaDataID>{73676db8-3d72-4e08-97fd-b73d6ca5ac39}</MetaDataID>
        String Name
        {
            get;
            set;
        }
        /// <MetaDataID>{05527f3c-3bb6-406e-a94c-ff61df2ab575}</MetaDataID>
        bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors);

        /// <MetaDataID>{d922aa32-3415-4efc-8647-a629d9d3f5b6}</MetaDataID>
        IListView Owner
        {
            get;
            set;
        }
        /// <MetaDataID>{3ed0ef0d-f4cb-4673-9f41-f73fff666818}</MetaDataID>
        OOAdvantech.UserInterface.Column ColumnMetaData
        {
            get;
            set;
        }
        /// <MetaDataID>{97d26bb6-701d-4b79-9bf9-48e1d7aed0c1}</MetaDataID>
        void SetValue(IRow row, object value);

        //ColumnConnection ColumnConnection
        //{
        //    get;
        //}

        //void DesigneRefresh();
        //object Path
        //{
        //    get;
        //    set;
        //}
        //String Name
        //{
        //    get;
        //    set;
        //}


    }

    /// <MetaDataID>{cd98b835-d829-4f18-85a0-a2a14f1a1e00}</MetaDataID>
    public enum ColumnType
    {
        TextColumn,
        ButtonColumn,
        CheckBoxColumn,
        ColorColumn,
        ComboBoxColumn,
        DateTimeColumn,
        ImageColumn,
        NumberColumn,
        ProgressBarColumn,
        SearchBoxColumn

    }
}
