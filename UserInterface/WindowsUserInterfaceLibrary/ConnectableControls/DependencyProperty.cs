using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using ConnectableControls.PropertyEditors;
using OOAdvantech.UserInterface.Runtime;

namespace ConnectableControls
{
    /// <MetaDataID>{0ec01c3b-1817-4880-8980-c7d033506cea}</MetaDataID>
    public partial class DependencyProperty : Component, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer, OOAdvantech.UserInterface.Runtime.IDependencyProperty
    {
        /// <MetaDataID>{12eed666-7080-4a79-aa26-2ec4a34553f8}</MetaDataID>
        public object PropertyOwner; 
        /// <MetaDataID>{fe91878a-e7fb-4cff-bd9e-da03869b6384}</MetaDataID>
        public IConnectableControl ConnectableControl;
        /// <MetaDataID>{27d6f868-67a6-47fd-8fc3-83cdb47f05e3}</MetaDataID>
        public DependencyProperty(IConnectableControl connectableControl, string propertyName)
        {
            PropertyOwner = connectableControl;
            ConnectableControl = connectableControl;
            PropertyName = propertyName;
            InitializeComponent();
        }
        /// <MetaDataID>{7c22d680-aaea-4998-9e16-5dcb5dba7388}</MetaDataID>
        public DependencyProperty(IConnectableControl connectableControl, object propertyOwner, string propertyName)
        {
            PropertyOwner = propertyOwner;
            ConnectableControl = connectableControl;
            PropertyName = propertyName;
            InitializeComponent();


        }

        /// <MetaDataID>{ec71712a-6dd0-475d-9f6a-dea13e2d0288}</MetaDataID>
        public readonly string PropertyName;

        #region IPathDataDisplayer Members

        /// <MetaDataID>{4b0cda8f-75cf-4dea-b2c3-4b7b250c98bc}</MetaDataID>
        string _Path;
        /// <MetaDataID>{99790a22-591a-43dd-a57f-4ff5e4ff5002}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor)),
        Category("Object Model Connection")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                string newPath = null;
                if (value is MetaData)
                    newPath = (value as MetaData).Path;
                if (value is string)
                    newPath = value as string;
                if ((_Path) != (newPath))
                    _Path = newPath;
            }
        }

        /// <MetaDataID>{5a47b0da-4be5-4d9a-abc7-da82f0c62b6f}</MetaDataID>
        void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.LoadControlValues()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{5c52003c-596f-4c3a-b67f-e95fc4299a65}</MetaDataID>
        void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.SaveControlValues()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{9a6b9d8c-01bd-4f7a-a9d7-3b6b61088213}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.UserInterfaceObjectConnection
        {
            get { throw new NotImplementedException(); }
        }

        /// <MetaDataID>{7624aad1-6ea0-424d-8f8b-0e1f876a4532}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.Paths
        {
            get { throw new NotImplementedException(); }
        }

        /// <MetaDataID>{b42f9e08-4db0-43ef-a270-8dc4392d7f3a}</MetaDataID>
        bool OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.HasLockRequest
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{a7f14cb4-b506-4a32-a8dc-7061abde09dd}</MetaDataID>
        void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            bool returnValueAsCollection;
            SetAssignedProperyValue(ConnectableControl.UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection));
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{07d00b89-3178-40aa-bb64-778452135a0d}</MetaDataID>
        void OOAdvantech.UserInterface.Runtime.IPathDataDisplayer.LockStateChange(object sender)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDependencyProperty Members

        /// <MetaDataID>{08a47cef-24dc-4ddc-8f69-2e6fb3febc04}</MetaDataID>
        string IDependencyProperty.Path
        {
            get { return _Path; }
        }
        /// <MetaDataID>{e135b053-2e46-4174-90c1-9208879d8c20}</MetaDataID>
        void IDependencyProperty.LoadPropertyValue()
        {
            bool returnValueAsCollection;
            SetAssignedProperyValue( ConnectableControl.UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection));
        }

        /// <MetaDataID>{8f04b2b6-b59e-4982-a310-e00843d6e3fa}</MetaDataID>
        public void SetAssignedProperyValue(object value)
        {
            if (PropertyOwner != null)
            {
                System.Type type = PropertyOwner.GetType();
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(PropertyName);
                if (value == null)
                    return;
                if (value.GetType() != propertyInfo.PropertyType && !value.GetType().IsSubclassOf(propertyInfo.PropertyType))
                    return;

                //while (propertyInfo == null && type != typeof(object))
                //{
                //    type = type.BaseType;
                //    propertyInfo = type.GetProperty(PropertyName);
                //}
                if (propertyInfo != null)
                    propertyInfo.SetValue(PropertyOwner, value, null);
                return;
            }
        }

        /// <MetaDataID>{9302687f-0307-4a35-9ddc-2fc1bd5e79f0}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                
                if (PropertyOwner != null)
                {
                    System.Type type = PropertyOwner.GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(PropertyName);
                    //while (propertyInfo == null && type != typeof(object))
                    //{
                    //    type = type.BaseType;
                    //    propertyInfo = type.GetProperty(PropertyName);
                    //}
                    if (propertyInfo != null)
                        return propertyInfo.GetValue(PropertyOwner, null);
                }

                throw new NotImplementedException();
            }
            set
            {
                if (!string.IsNullOrEmpty(_Path) && ConnectableControl != null)
                    ConnectableControl.UserInterfaceObjectConnection.SetValue(value, _Path);
                SetAssignedProperyValue(value);
            
            }
        }

        #endregion

        #region IDependencyProperty Members


        //object IDependencyProperty.Value
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

        #endregion
    }
}
