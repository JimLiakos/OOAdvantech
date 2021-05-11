using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using ConnectableControls.PropertyEditors;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller ;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using System.Collections.Generic;

namespace ConnectableControls
{
    /// <MetaDataID>{63B554D0-27E8-4CF0-B0F5-B258C65E233B}</MetaDataID>
    public class PropertyGrid : System.Windows.Forms.PropertyGrid, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        public virtual void InitializeControl()
        {

        }
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }
        bool _AllowDrag = false;
        public bool AllowDrag
        {
            get
            {
                return _AllowDrag;
            }
            set
            {
                _AllowDrag = value; ;
            }
        }
        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }

 
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }


        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                if (_AllPaths == null)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    //_AllPaths.Add(_Path+".[Lock]");
                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    return _AllPaths;
                }
                else
                    return _AllPaths;
            }
        }



        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[1] { "Value" };
            }
        }
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;

            //if (propertyName == "this.Value")
            //    return Value;
            //if (propertyName == "this.Text")
            //    return Value;


            if (propertyName == "Value")
                return Value;
           

            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            if (propertyName == "this.Value")
                return true;

            if (propertyName == "Value")
                return true;
       
            return false;
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;

        }
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }

        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            return false;

        }

      
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                
                return SelectedObject;

            }
            set
            {
                SelectedObject = value;

            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                if (_ValueType != null)
                    return _ValueType;
                if(!string.IsNullOrEmpty(_Path))
                    _ValueType = UserInterfaceObjectConnection.GetClassifier(_Path);
                return _ValueType;
            }

        }
        public void SetValueType(OOAdvantech.MetaDataRepository.Classifier valueType)
        {
            _ValueType = valueType;

        }
  
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (UserInterfaceObjectConnection != null)
                return UserInterfaceObjectConnection.PresentationObjectType; 

            return null;
        }


        [ObsoleteAttribute("This property will be removed from future Versions.Use another property 'UpdateStyle'", false)]
        public bool ConnectedObjectAutoUpdate
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        UpdateStyle _UpdateStyle;
        public UpdateStyle UpdateStyle
        {
            get
            {
                return _UpdateStyle;
            }
            set
            {
                _UpdateStyle = value;

            }
        }

  
        OOAdvantech.MetaDataRepository.Classifier _ValueType;
        bool IsConnectionDataCorrect
        {
            get
            {
                if (UserInterfaceObjectConnection == null)
                {
                    if (ExistConnectionData)
                        AutoProducedEnabledValue = false;
                    return false;
                }
                _ValueType = UserInterfaceObjectConnection.GetClassifier(Path as string);
                if (_ValueType == null)
                {
                    if (ExistConnectionData)
                        AutoProducedEnabledValue = false;
                    return false;
                }

                return true;
            }
        }
        bool ExistConnectionData
        {
            get
            {
                if ((Path as string) == null || (Path as string).Length == 0)
                    return false;
                else
                    return true;

            }
        }
        
        bool InSelectionChange = false;
   
   

        private bool _AutoDisable = true;
        [Category("Object Model Connection")]
        public bool AutoDisable
        {
            get
            {
                return _AutoDisable;
            }
            set
            {
                _AutoDisable = value;
            }
        }


        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection  _ViewControlObject = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection  UserInterfaceObjectConnection
        {
            get
            {
                return _ViewControlObject;
                
            }
            set
            {
                if (_ViewControlObject != value)
                {
                    _ViewControlObject = value;
                    if (_ViewControlObject != null)
                        _ViewControlObject.AddControlledComponent(this);
                }

            }
        }
        [Category("Object Model Connection")]
        public ViewControlObject ViewControlObject
        {
            get
            {
                if (UserInterfaceObjectConnection == null)
                    return null;
                return UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject;
            }
            set
            {
                if (value != null)
                    UserInterfaceObjectConnection = value.UserInterfaceObjectConnection;
                else
                    UserInterfaceObjectConnection = null;

            }
        }
        public void SaveControlValues()
        {
            //if (!Enabled)
            //    return;
            //try
            //{
            //    SuspendDisplayedValueChangedHandler = true;
            //    if (IsConnectionDataCorrect && (_UpdateStyle == TextBoxUpdateStyle.OnSaveControlsValue || (Focused && _UpdateStyle == TextBoxUpdateStyle.OnLostFocus)) && UserInterfaceObjectConnection.CanEditValue(_Path, this))
            //    {
            //        if (string.IsNullOrEmpty(Text) && (_ValueType.GetExtensionMetaObject(typeof(Type)) as Type) != typeof(string))
            //            UserInterfaceObjectConnection.SetValue(OOAdvantech.AccessorBuilder.GetDefaultValue(_ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
            //        else
            //            UserInterfaceObjectConnection.SetValue(Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);

            //    }
            //}
            //finally
            //{
            //    SuspendDisplayedValueChangedHandler = false;
            //}
        }


        /// <MetaDataID>{011E9036-1C6E-4C86-8076-BF72A0256363}</MetaDataID>
        public void LoadControlValues()
        {
            if (!ExistConnectionData)
                return;
            if (IsConnectionDataCorrect)
            {
                UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;
                object displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                object value = displayedValue;
                SelectedObject = value;
                //if (value != null)
                //    Text = value.ToString();
                //else
                //    Text = "";
                //if (Enabled && _AutoDisable && !DesignMode)
                //    Enabled = UserInterfaceObjectConnection.CanAccessValue(_Path, this);
               // AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
            }


        }

        public void LockStateChange(object sender)
        {
            AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
        }

        #region Auto disable code
        bool _UserDefinedEnableValue=true;
        bool SuspendDisplayedValueChangedHandler = false;
        bool _AutoProducedEnableValue = true;
        bool EnableAutoProduced = false;
        bool AutoProducedEnabledValue
        {
            get
            {
                return _AutoProducedEnableValue;
            }
            set
            {
                EnableAutoProduced = true;
                try
                {
                    _AutoProducedEnableValue = value;
                    Enabled = _UserDefinedEnableValue & _AutoProducedEnableValue;
                }
                finally
                {
                    EnableAutoProduced = false;
                }
            }
        }
        protected override void OnEnabledChanged(EventArgs e)
        {

            base.OnEnabledChanged(e);
            if (!EnableAutoProduced)
            {
                _UserDefinedEnableValue = Enabled;
                try
                {
                    EnableAutoProduced = true;
                    Enabled = _AutoProducedEnableValue & _UserDefinedEnableValue;
                }
                finally
                {
                    EnableAutoProduced = false;
                }
            }
        }
        #endregion


        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            if (memberChangeEventArg.Type == OOAdvantech.UserInterface.Runtime.ChangeType.ValueChanged)
            {
                if (!SuspendDisplayedValueChangedHandler)
                    LoadControlValues();
            }
            else if (memberChangeEventArg.Type == OOAdvantech.UserInterface.Runtime.ChangeType.LockChanged)
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
        }

        /// <MetaDataID>{83C40642-200F-4371-9075-39C1CB7F5785}</MetaDataID>
        private string _Path;

        /// <MetaDataID>{5ED59BC4-4269-41B8-B1EE-C5262355FFED}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public Object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (value is string)
                    _Path = value as string;
                else if (value is MetaData)
                    _Path = (value as MetaData).Path;
            }
        }

        #region IObjectMemberViewControl Members


        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Path" && metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            else
                return false;
        }

        #endregion

        #region IObjectMemberViewControl Members

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object UIMetaDataObject
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        #endregion


        




    }
    ///// <MetaDataID>{6bec43f7-5435-402c-80ca-fa4450917c34}</MetaDataID>
    //public enum TextBoxUpdateStyle
    //{
    //    OnSaveControlsValue,
    //    Straightly,
    //    OnLostFocus
    //}
}



