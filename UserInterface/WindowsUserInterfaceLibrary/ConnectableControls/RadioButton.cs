using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;
using OOAdvantech.Collections;

namespace ConnectableControls
{
    /// <MetaDataID>{7eabb046-143f-4287-98a1-06b428553e01}</MetaDataID>
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.RadioButton), "RadioButton.bmp")]
    public class RadioButton : System.Windows.Forms.RadioButton, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        /// <MetaDataID>{827cc240-9043-42db-b6ee-de05230eead8}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{f0dd2f96-3ac5-4203-a832-bebf2f94c286}</MetaDataID>
        public virtual void InitializeControl()
        {

        }

        /// <MetaDataID>{92ec8714-149c-4c46-ace9-823bf52a47ec}</MetaDataID>
        public RadioButton()
        {
            _EnableProperty = new DependencyProperty(this, "Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }

        /// <exclude>Excluded</exclude>
        DependencyProperty _EnableProperty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public DependencyProperty EnableProperty
        {
            get
            {

                return _EnableProperty;
            }
            set
            {
                if (value != null)
                {
                    _EnableProperty = value;
                    _EnableProperty.ConnectableControl = this;
                }
            }
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

        /// <MetaDataID>{a1fa3bd8-8750-458c-a32b-ba558ef5ed02}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            if (UserInterfaceObjectConnection != null)
                return UserInterfaceObjectConnection.PresentationObjectType;

            return null;
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
                return false;
            }
        }

        /// <MetaDataID>{07c9c5c7-f6d5-4cce-be25-86dadb0416ed}</MetaDataID>
        public void LockStateChange(object sender)
        {

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
            }
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
      
        #region protected override void OnCheckedChanged(EventArgs e)
        /// <MetaDataID>{75f396a2-b819-4e69-b320-6fe0bceb0101}</MetaDataID>
        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            if (UpdateStyle==UpdateStyle.Immediately && IsConnectionDataCorrect)
                UserInterfaceObjectConnection.SetValue(Value, Path as string);

        }
        #endregion       

        #region IObjectMemberViewControl Members

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
        //DisplayedValue DisplayedValue;

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
        /// <MetaDataID>{90d4ce02-f65d-40c4-a740-e44005f97c17}</MetaDataID>
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


        /// <MetaDataID>{01e3d787-e3b7-490c-ab71-67533ec378ae}</MetaDataID>
        public void LoadControlValues()
        {
            try
            {

                String valueText = Text;
                if (ConnectableControls.ViewControlObject.Translator != null)
                {
                    valueText = ConnectableControls.ViewControlObject.Translator.Translate(FindForm().GetType().FullName + "." + Name);
                    if (valueText != null)
                        Text = valueText;
                }


                if (!ExistConnectionData)
                    return;
                if (IsConnectionDataCorrect)
                {
                    UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                    AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(Path as string, this);
                    bool returnValueAsCollection = false;
                    object displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path as string, this,out returnValueAsCollection );
                        DisplayedValue = displayedValue;

                    Value = displayedValue;
                    //if (Enabled && _AutoDisable && !DesignMode)
                    //    Enabled = UserInterfaceObjectConnection.CanAccessValue(_Path, this);

                }
            }
            catch (Exception error)
            {
            }
        }

               


        #region public void SaveControlData()
        /// <MetaDataID>{a77e274c-5ea7-41b7-bea0-4d434626c21f}</MetaDataID>
        public void SaveControlValues()
        {
            if (IsConnectionDataCorrect && UpdateStyle ==UpdateStyle.OnSaveControlsValue)
                UserInterfaceObjectConnection.SetValue(Value, Path as string);
        }

        #endregion



        /// <MetaDataID>{277444e2-2afa-44c5-aa2a-cc199d11cbe2}</MetaDataID>
        bool OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && (UserInterfaceObjectConnection == null || UserInterfaceObjectConnection.GetClassifier(_Path as string) == null))
            {
                if (FindForm() != null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: RadioButton '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                else
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: RadioButton '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

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

            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "Value")
                return Value;


            throw new Exception("There isn't property with name " + propertyName + ".");
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

        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            
            if (propertyName == "Value")
                return true;
            return false;
        }

        //OOAdvantech.MetaDataRepository.Classifier OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl.GetClassifierFor(ITypeDescriptorContext context)
        //{
        //    if (UserInterfaceObjectConnection != null)
        //        return UserInterfaceObjectConnection.PresentationObjectType;

        //    return null;
        //}



        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                return Checked;

            }
            set
            {
                if (value is bool)
                {
                    Checked = (bool)value;
                }

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
        #endregion

        #region IConnectableControl Members

        public bool IsPropertyReadOnly(string propertyName)
        {            
            return false;         
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
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        object DisplayedValue;
            
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
                if (type == typeof(bool))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        //bool OOAdvantech.UserInterface.Runtime.IConnectableControl.CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        //{
        //    return true;
        //}

        #endregion

        #region IPathDataDisplayer Members

        /// <MetaDataID>{FE28024F-4E17-44C5-9851-847A83FAA04D}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            LoadControlValues();
        }

        

        

        #endregion
    }
}
