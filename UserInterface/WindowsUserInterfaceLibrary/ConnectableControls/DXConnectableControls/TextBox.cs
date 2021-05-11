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
using ConnectableControls;

namespace DXConnectableControls.XtraEditors
{
    /// <MetaDataID>{63B554D0-27E8-4CF0-B0F5-B258C65E233B}</MetaDataID>
    public class TextBox :DevExpress.XtraEditors.TextEdit, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{7c7d15d7-5157-48d4-a951-da0ed6a9448a}</MetaDataID>
        public void InitializeControl()
        {
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


        /// <MetaDataID>{bddaff32-de15-4ff1-b351-fe74f50b6fa5}</MetaDataID>
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
                return new string[1] { "Text" };
            }
        }
        /// <MetaDataID>{b8c8819c-a133-406f-a742-8b71b7153d6e}</MetaDataID>
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
            if (propertyName == "Text")
                return Value;

            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        /// <MetaDataID>{b1de8d1d-f396-4a00-8078-456849ecdcd3}</MetaDataID>
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


        object _Value;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                
                return _Value;

            }
            set
            {
                _Value = value;

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

        string LastText;
        int LastSelectionStart=0;

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
   
        protected override void OnTextChanged(System.EventArgs e)
        {
            
            if (InSelectionChange)
                return;
            try
            {
                InSelectionChange = true;
                //if (!ExistConnectionData)
                //    return;
                if (ValueType!=null)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(Text.Trim()) && ValueType is OOAdvantech.MetaDataRepository.Primitive)
                        {
                            Text = "0";
                            SelectionStart = 0;
                            SelectionLength = 1;
                            LastSelectionStart = 1;
                            LastText = Text;

                        }
                        else
                        {
                            object Value = System.Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                            LastText = Text;
                            LastSelectionStart = SelectionStart;
                        }
                    }
                    catch (System.Exception error)
                    {

                        if (!string.IsNullOrEmpty(Text))
                        {
                            int _SelectionStart = SelectionStart - 1;
                            if (_SelectionStart < 0)
                                _SelectionStart = 0;
                            Text = LastText;
                            SelectionStart = LastSelectionStart;
                        }
                        else
                            LastText = Text;
                    }

                }
                base.OnTextChanged(e);
            }
            finally
            {
                InSelectionChange = false;

            }

        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            LastSelectionStart = SelectionStart;
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (DesignMode)
                return;
            try
            {
                if (!ExistConnectionData)
                    return;
                if (e.KeyData == Keys.Left || 
                    e.KeyData == Keys.Right||
                    e.KeyData == Keys.Home||
                    e.KeyData == Keys.End)
                    LastSelectionStart = SelectionStart;

                SuspendDisplayedValueChangedHandler = true;
                if (IsConnectionDataCorrect && _UpdateStyle == UpdateStyle.Immediately && UserInterfaceObjectConnection.CanEditValue(_Path, this))
                {
                    if (string.IsNullOrEmpty(Text) && (_ValueType.GetExtensionMetaObject(typeof(Type)) as Type)!=typeof(string))
                        UserInterfaceObjectConnection.SetValue(OOAdvantech.AccessorBuilder.GetDefaultValue(_ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
                    else
                        UserInterfaceObjectConnection.SetValue(Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);

                }
                
            }
            catch (System.Exception error)
            {
                throw;
            }
            finally
            {
                SuspendDisplayedValueChangedHandler = false;
            }
        }
        protected override void OnLostFocus(EventArgs e)
        {
            
            try
            {
                if (DesignMode)
                    return;
                try
                {

                    SuspendDisplayedValueChangedHandler = true;

                    if (!ExistConnectionData)
                        return;

                    if (IsConnectionDataCorrect && _UpdateStyle == UpdateStyle.OnLostFocus && UserInterfaceObjectConnection.CanEditValue(_Path, this))
                    {
                        if (string.IsNullOrEmpty(Text) && (_ValueType.GetExtensionMetaObject(typeof(Type)) as Type) != typeof(string))
                            UserInterfaceObjectConnection.SetValue(OOAdvantech.AccessorBuilder.GetDefaultValue(_ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
                        else
                            UserInterfaceObjectConnection.SetValue(Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);

                    }

                }
                catch (System.Exception error)
                {
                    throw;
                }
                finally
                {
                    SuspendDisplayedValueChangedHandler = false;
                }
            }
            finally
            {
                base.OnLostFocus(e);
            }



        }


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
            if (!Enabled)
                return;
            try
            {
                SuspendDisplayedValueChangedHandler = true;
                if (IsConnectionDataCorrect && (_UpdateStyle == UpdateStyle.OnSaveControlsValue || (Focused && _UpdateStyle == UpdateStyle.OnLostFocus)) && UserInterfaceObjectConnection.CanEditValue(_Path, this))
                {
                    if (string.IsNullOrEmpty(Text) && (_ValueType.GetExtensionMetaObject(typeof(Type)) as Type) != typeof(string))
                        UserInterfaceObjectConnection.SetValue(OOAdvantech.AccessorBuilder.GetDefaultValue(_ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
                    else
                        UserInterfaceObjectConnection.SetValue(Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);

                }
            }
            finally
            {
                SuspendDisplayedValueChangedHandler = false;
            }
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
                if (value != null)
                    Text = value.ToString();
                else
                    Text = "";
                //if (Enabled && _AutoDisable && !DesignMode)
                //    Enabled = UserInterfaceObjectConnection.CanAccessValue(_Path, this);
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
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

        System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get 
            {
                return _DependencyProperties;
            }
        }

    }
    /// <MetaDataID>{e4c2af3d-c88f-454a-b32a-8f6fac8c4f9e}</MetaDataID>
    public enum TextBoxUpdateStyle
    {
        OnSaveControlsValue,
        Straightly,
        OnLostFocus
    }
}



