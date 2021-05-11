using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using ConnectableControls.PropertyEditors;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller ;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;


namespace ConnectableControls
{
    /// <MetaDataID>{AEAD21E7-A478-45AF-BD4E-4EE5ED32F337}</MetaDataID>
    public class CheckBox : System.Windows.Forms.CheckBox, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        /// <MetaDataID>{0e49de0b-8072-4df6-858d-c55176dd7ffe}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{6af8afec-8407-4c79-baef-ac7497ba89c7}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{03fc8b0c-21b1-444b-ad36-8e3457a7bf92}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }
        /// <MetaDataID>{bcc4f014-f881-44ef-9182-29c16037d6dc}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{37af6b9f-b4fe-4227-b653-4151158dc1eb}</MetaDataID>
        public CheckBox()
        {
            _EnableProperty = new DependencyProperty(this, "Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }


        /// <MetaDataID>{4e58a350-efe0-4802-a133-a53490a3b11d}</MetaDataID>
        DependencyProperty _EnableProperty;
        /// <MetaDataID>{67f31ddd-5db8-4730-9d56-185076105f99}</MetaDataID>
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
        /// <MetaDataID>{7953b4b6-541c-4ac7-82ee-7fdfc26efc83}</MetaDataID>
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
        /// <MetaDataID>{fdce35f4-6337-422b-8bd1-adbf59b7baa2}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{36f75e57-f20a-4a5a-94f7-3842592d5720}</MetaDataID>
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
        /// <MetaDataID>{7a2f3713-1edd-4c14-a7f2-30c02c29448d}</MetaDataID>
        public void LockStateChange(object sender)
        {

        }
        /// <MetaDataID>{4a2e6110-e49b-4961-839b-4268c6e65098}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }
        /// <MetaDataID>{aa1063dc-86a4-4678-824a-cc00b7fe08fb}</MetaDataID>
        public void TransactionLocked(bool locked)
        {
        }


        /// <MetaDataID>{6a15c537-40cb-4962-819b-30afca3d8fa8}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }
        /// <MetaDataID>{d29ff751-f068-4d6a-88e3-970b9073118c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[0];
            }
        }
        /// <MetaDataID>{1ba8027a-23a0-4bb8-a20e-08af63ad45ff}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            //if (propertyName == "this.Value")
            //    return true;

            if (propertyName == "Value")
                return true;
            return false;
        }
        /// <MetaDataID>{aba03d3b-f5cb-4461-b454-d3e706860088}</MetaDataID>
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

        /// <MetaDataID>{654f1705-171e-45db-81c9-06e7a416a3b4}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {


            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{2e74f791-5be0-4a76-a1e8-551dc9d04e0d}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            if (propertyName == "Text")
                return OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(typeof(System.String));

            return null;

        }
        /// <MetaDataID>{8ac65f9e-69c0-4a52-9e1a-d4698331ff77}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }

        /// <MetaDataID>{403be43f-ced3-442f-a1e1-30d14abf871d}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) &&(UserInterfaceObjectConnection==null ||UserInterfaceObjectConnection.GetClassifier(_Path as string) == null))
            {
                if (FindForm() != null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: CheckBox '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                else
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: CheckBox '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                return true;
            }
            return false;
        }

        /// <MetaDataID>{669d8917-e343-430f-b72a-59eb2a927109}</MetaDataID>
        private bool _AutoDisable = true;
        /// <MetaDataID>{f6a11be9-568f-4d4d-abc0-84343b8018a2}</MetaDataID>
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


        #region IObjectMemberViewControl Members





        /// <MetaDataID>{6def8788-06ee-4f47-8e64-e544e19e0023}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection  _ViewControlObject = null;
        /// <MetaDataID>{c2625882-0f85-42e5-9c1f-49150d7fdf0a}</MetaDataID>
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




        /// <MetaDataID>{bc1d5abb-1a70-472f-bf87-4006ad7749da}</MetaDataID>
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
        /// <MetaDataID>{875BDC22-C4F4-4B3C-A063-CDD5CE212573}</MetaDataID>
        protected override void OnCheckStateChanged(EventArgs e)
        {
            base.OnCheckStateChanged(e);
            if (UpdateStyle == UpdateStyle.Immediately && IsConnectionDataCorrect)
                UserInterfaceObjectConnection.SetValue(Value, Path as string);

        }

        /// <MetaDataID>{215aff4b-4bd4-418e-80e1-28f8c589808d}</MetaDataID>
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
                        UserInterfaceObjectConnection.SetValue(Value, Path as string);
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

        /// <MetaDataID>{B4C413B7-C240-4B5E-A188-0EFD3700F7FD}</MetaDataID>
        public void SaveControlValues()
        {
            if (IsConnectionDataCorrect && UpdateStyle==UpdateStyle.OnSaveControlsValue)
                UserInterfaceObjectConnection.SetValue(Value, Path as string);


        }
        /// <MetaDataID>{50808889-dda9-4a59-8bff-9c69c7cdc1f6}</MetaDataID>
        object DisplayedValue;


        /// <MetaDataID>{d277330f-1c25-4389-8b9a-b755936f2bf1}</MetaDataID>
        private string _Path;

        /// <MetaDataID>{d9f1cb73-bbc7-41af-b53e-d631ed39cb6e}</MetaDataID>
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

        /// <MetaDataID>{06a8e449-edbc-4254-aee8-6109a0c6d0c5}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{db00deb9-0086-47b0-a9a6-996d74290487}</MetaDataID>
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

        #region Auto disable code
        /// <MetaDataID>{f986cdb2-e152-4279-ae62-290459b61ea7}</MetaDataID>
        bool _UserDefinedEnableValue = true;
        /// <MetaDataID>{f10a6816-d95e-4deb-aa46-51f13ae3a109}</MetaDataID>
        bool SuspendDisplayedValueChangedHandler = false;
        /// <MetaDataID>{162e8a58-4598-41f7-b7f9-e0f763cd2a25}</MetaDataID>
        bool _AutoProducedEnableValue = true;
        /// <MetaDataID>{6c4b9727-7717-4b56-af79-32d25e4bb689}</MetaDataID>
        bool EnableAutoProduced = false;
        /// <MetaDataID>{62547b6c-64c4-40db-a1b0-1300f953ea4e}</MetaDataID>
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
        /// <MetaDataID>{0829a601-fcd8-4af7-93a8-0bff5cbd9163}</MetaDataID>
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

        /// <MetaDataID>{14C4D50C-B236-4AF4-96F9-6C6AA72C5292}</MetaDataID>
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

                bool returnValueAsCollection = false;

                if (!ExistConnectionData)
                    return;
                if (IsConnectionDataCorrect)
                {
                    UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                    AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
                    object displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path as string, this, out returnValueAsCollection);
                    if (DisplayedValue != displayedValue)
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

        /// <MetaDataID>{FE28024F-4E17-44C5-9851-847A83FAA04D}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            LoadControlValues();
        }


        /// <MetaDataID>{8D450411-2F6A-412D-BE0A-1B98273E81E7}</MetaDataID>
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

        /// <MetaDataID>{6f17e9a1-d5b2-4b27-ae15-c93b069cc582}</MetaDataID>
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
        /// <MetaDataID>{0036b0f2-75e9-46af-bf9d-e905ebabf07b}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier _ValueType;
        /// <MetaDataID>{a8a26881-6de2-4954-a7d3-8ca040900fcf}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                return _ValueType;
            }

        }
        /// <MetaDataID>{8c84072c-cf82-4374-a089-bf9010577956}</MetaDataID>
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
                if (UserInterfaceObjectConnection.PresentationObject== null)
                {
                    if (ExistConnectionData)
                        AutoProducedEnabledValue = false;

                    return false;
                }
                return true;
            }
        }
        /// <MetaDataID>{1ec2e801-5184-49d2-a962-2b22da719aa5}</MetaDataID>
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

        /// <MetaDataID>{57139216-2622-4D66-B727-D6865C18DCBA}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (UserInterfaceObjectConnection != null)
                return UserInterfaceObjectConnection.PresentationObjectType;

            return null;
        }



        /// <exclude>Excluded</exclude>
        UpdateStyle _UpdateStyle;
        /// <MetaDataID>{85f1346a-7c01-4358-ab85-6a1dde00f25a}</MetaDataID>
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

        /// <MetaDataID>{df57961b-a57c-4a87-8f17-695c72f1d67d}</MetaDataID>
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
}
