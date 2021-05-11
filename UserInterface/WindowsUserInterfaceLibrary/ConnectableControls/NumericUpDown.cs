using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;
using System.Windows.Forms;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;

namespace ConnectableControls
{
    /// <MetaDataID>{6a40f08e-3410-4e1f-84c7-1772fc52cf80}</MetaDataID>
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.NumericUpDown), "NumericUpDown.bmp")]
    public class NumericUpDown : System.Windows.Forms.NumericUpDown, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        public virtual void InitializeControl()
        {

        }
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }
        public void LockStateChange(object sender)
        {

        }
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        #region bool ExistConnectionData
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
        #endregion

        #region bool IsConnectionDataCorrect
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
                if (UserInterfaceObjectConnection.PresentationObject == null)
                {
                    if (ExistConnectionData)
                        AutoProducedEnabledValue = false;

                    return false;
                }
                return true;
            }
        } 
        #endregion

        #region protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        string LastText;
        //int LastSelectionStart = 0;
        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                //if (e.KeyData == Keys.Left || e.KeyData == Keys.Right)
                //    LastSelectionStart = this.sel;
                if (!ExistConnectionData)
                    return;

                if (IsConnectionDataCorrect && UpdateStyle==UpdateStyle.Immediately)
                {

                    UserInterfaceObjectConnection.SetValue(Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
                }


            }
            catch (System.Exception error)
            {
                throw;
            }
            finally
            {
            }
        } 
        #endregion

        #region protected override void OnTextChanged(EventArgs e)
        bool InSelectionChange = false;
        protected override void OnTextChanged(EventArgs e)
        {
            if (InSelectionChange)
                return;
            try
            {
                InSelectionChange = true;
                if (!ExistConnectionData)
                    return;
                if (IsConnectionDataCorrect)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(Text.Trim()) && ValueType is OOAdvantech.MetaDataRepository.Primitive)
                        {
                            Text = this.Minimum.ToString();
                            //SelectionStart = 0;
                            //SelectionLength = 1;
                            //LastSelectionStart = 1;
                            LastText = Text;
                        }
                        else
                        {
                            object Value = System.Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                            LastText = Text;
                            //LastSelectionStart = SelectionStart;
                        }
                    }
                    catch (System.Exception error)
                    {
                        //int _SelectionStart = SelectionStart - 1;
                        //if (_SelectionStart < 0)
                        //    _SelectionStart = 0;
                        Text = LastText;
                        //SelectionStart = LastSelectionStart;
                    }

                }
            }
            finally
            {
                InSelectionChange = false;

            }

            base.OnTextChanged(e);
        } 
        #endregion

        #region IObjectMemberViewControl Members
        public bool AllowDrag
        {
            get
            {
                return false;
            }
        }

        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && (UserInterfaceObjectConnection == null || UserInterfaceObjectConnection.GetClassifier(_Path as string) == null))
            {
                if (FindForm() != null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                else
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                return true;
            }
            return false;
        }

        public string[] PropertiesNames
        {
            get
            {
                return new string[0];
            }
        }

        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;

            if (propertyName == "Value")
                return Value;

            throw new Exception("There isn't property with name " + propertyName + ".");
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;
        }

        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;

            if (propertyName == "Value")
                return true;
            return false;
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
        /// <exclude>Excluded</exclude>
        UpdateStyle _UpdateStyle;
        [Category("Object Model Connection")]
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

        object _Value;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object Value
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

        OOAdvantech.MetaDataRepository.Classifier _ValueType;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                return _ValueType;
            }
        }

        private string _Path;
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
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


        #region Auto disable code
        bool _UserDefinedEnableValue = true;
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



        public void LoadControlValues()
        {
            if (!ExistConnectionData)
                return;
            if (IsConnectionDataCorrect)
            {
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
                UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;
                object displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path, this,out returnValueAsCollection);
                    DisplayedValue = displayedValue;

                object value = displayedValue;
                if (value != null)
                    Text = value.ToString();
                else
                    Text = "";
                //if (Enabled && _AutoDisable && !DesignMode)
                //    Enabled = UserInterfaceObjectConnection.CanAccessValue(_Path,this);
            }
        }

        public void SaveControlValues()
        {
            if (!Enabled)
                return;
            try
            {
                SuspendDisplayedValueChangedHandler = true;
                if (IsConnectionDataCorrect && 
                    UpdateStyle==UpdateStyle.OnSaveControlsValue&& 
                    UserInterfaceObjectConnection.CanEditValue(_Path,this) )
                    //&& UserInterfaceObjectConnection.CanAccessValue(_Path,this))
                    UserInterfaceObjectConnection.SetValue(Convert.ChangeType(Text, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
            }
            finally
            {
                SuspendDisplayedValueChangedHandler = false;
            }
        }

        #endregion

        #region IConnectableControl Members

        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        object DisplayedValue;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _ViewControlObject;
            }
            set
            {
                _ViewControlObject = value;
                if (_ViewControlObject != null)
                    _ViewControlObject.AddControlledComponent(this);

            }
        }

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

        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
            {
                Type type = (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type;
                if (type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(float) || type == typeof(decimal) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

    

        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion

        #region IPathDataDisplayer Members
        


        #endregion

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
            }
        }

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            if (!SuspendDisplayedValueChangedHandler)
                LoadControlValues();
        }
    }
}
