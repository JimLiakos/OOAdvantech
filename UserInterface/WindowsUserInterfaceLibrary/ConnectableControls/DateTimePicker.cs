using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using ConnectableControls.PropertyEditors;

namespace ConnectableControls
{
    /// <MetaDataID>{318AB6C8-B106-4942-8122-EA4C5B29D9AF}</MetaDataID>
    public class DateTimePicker : System.Windows.Forms.DateTimePicker, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {
        }

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

        public DateTimePicker()
        {
            _EnableProperty = new DependencyProperty(this, "Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }

        /// <MetaDataID>{9ae3cca4-fc2b-4751-836c-c2c487da9f3a}</MetaDataID>
        public void EndOfInitialization()
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
        /// <MetaDataID>{ca0cb883-5647-457b-b62e-10989c2c2f43}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{7927518e-f421-4f07-8041-e882913d2150}</MetaDataID>
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


        /// <MetaDataID>{004873af-3045-47fd-baa0-4695be5193c4}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        /// <MetaDataID>{e1d1a12a-6a9d-42fc-957a-399ac0bea2e7}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{60cb2282-4197-4502-8cde-791fb77f7b18}</MetaDataID>
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



        /// <MetaDataID>{4cdabf6b-5282-41ef-9069-a534aed461d1}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[1] { "Value" };
            }
        }
        /// <MetaDataID>{72cdccb0-21a3-4b42-bdf8-f94c5f981a80}</MetaDataID>
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

        /// <MetaDataID>{dc19f406-fabb-4ed4-a330-fc70963cc62d}</MetaDataID>
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
            {
                if (string.IsNullOrEmpty(Path as string))
                    return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(System.DateTime));
                else
                    return ValueType;
            }

            return null;

        }
        /// <MetaDataID>{ba0c9c12-4be3-4bce-8d53-8abc892a4e85}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }
        /// <MetaDataID>{1b20e8a2-4f9a-4810-b239-29838d2fc067}</MetaDataID>
        private bool _AutoDisable = true;
        /// <MetaDataID>{43efa98e-4ba1-4865-8422-992b0254f22f}</MetaDataID>
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

        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            return false;

        }


        #region IObjectMemberViewControl Members


        /// <MetaDataID>{14cf1649-6d0c-4b5e-beca-2b86c02d31f0}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
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
        /// <MetaDataID>{8562dd22-0829-4381-91dd-5953d4c5fb6b}</MetaDataID>
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

        /// <MetaDataID>{321E39EC-0E0A-4890-87C7-B58269A4BB75}</MetaDataID>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);
            if (_UpdateStyle==UpdateStyle.Immediately && IsConnectionDataCorrect)
                UserInterfaceObjectConnection.SetValue(Value, Path as string);

        }

        /// <MetaDataID>{E2DD3104-1A31-4D35-94F0-6B21E12D318D}</MetaDataID>
        public void SaveControlValues()
        {
            if (IsConnectionDataCorrect && _UpdateStyle == UpdateStyle.OnSaveControlsValue)
                UserInterfaceObjectConnection.SetValue(Value, Path as string);


        }
        //OOAdvantech.UserInterface.Runtime.DisplayedValue DisplayedValue;


        /// <MetaDataID>{05608f32-6ea6-45c0-bcc0-367989fa0320}</MetaDataID>
        private string _Path;

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

        #region Auto disable code
        /// <MetaDataID>{b2c9ea89-0718-4fe0-89a0-9501700ecf8a}</MetaDataID>
        bool _UserDefinedEnableValue = true;
        /// <MetaDataID>{57e36993-8228-4824-8d64-1e1dd1a242e5}</MetaDataID>
        bool SuspendDisplayedValueChangedHandler = false;
        /// <MetaDataID>{21661c89-f2e7-4c5d-849a-df1c12d2f71c}</MetaDataID>
        bool _AutoProducedEnableValue = true;
        /// <MetaDataID>{3c10ac6f-f2d5-40f6-b145-9d43ac3e5baa}</MetaDataID>
        bool EnableAutoProduced = false;
        /// <MetaDataID>{3485020f-e96d-4513-86d5-61b8fdf8ffc6}</MetaDataID>
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

        /// <MetaDataID>{F74ED2E3-C504-4994-9AF0-B346AF1E9D6E}</MetaDataID>
        public void LoadControlValues()
        {
            if (!ExistConnectionData)
                return;
            if (IsConnectionDataCorrect)
            {
                UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;
                object displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path as string, this, out returnValueAsCollection);
                Value = displayedValue;
                //if (Enabled && _AutoDisable && !DesignMode)
                //    Enabled = UserInterfaceObjectConnection.CanAccessValue(_Path, this);
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
            }
        }
        public void LockStateChange(object sender)
        {
            AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
        }



        /// <MetaDataID>{0123D27D-DB29-462F-94E6-DE980A40CFD6}</MetaDataID>
        public void DisplayedValueChange(object sender, EventArgs e)
        {
            LoadControlValues();
        }


        /// <MetaDataID>{5303173C-97C5-4251-A9C1-C9F3C186FFE9}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
            {
                Type type = (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type;
                if (type == typeof(DateTime))
                    return true;
                //this is for the EnableProperty 
                //Comes in here from the DClick of the MetaDataNavigator line 1190
                if (type == typeof(bool))
                    return true;
                
                return false;
            }
            else
                return false;
        }

        /// <MetaDataID>{460b0ba1-5ffc-48f8-b354-b5d0cb4248bb}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                return base.Value;

            }
            set
            {
                if (value is DateTime)
                {
                    if (((DateTime)value) < MinDate)
                        base.Value = MinDate;
                    else if (((DateTime)value) > MaxDate)
                        base.Value = MaxDate;
                    else
                        base.Value = (DateTime)value;
                }
            }
        }
        /// <MetaDataID>{aa6e2524-61b2-4ab5-836c-be554dd8e8bf}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier _ValueType;
        /// <MetaDataID>{5b9a7f5a-8095-4273-a778-8db1efad949c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                return _ValueType;
            }

        }
        /// <MetaDataID>{d256620a-5aea-43fb-98a1-36634071265d}</MetaDataID>
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
        /// <MetaDataID>{5534e275-05b5-4a83-bea1-0e4bcd86aad0}</MetaDataID>
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

        /// <MetaDataID>{9CCB323D-7E27-4D0B-85D9-8EA8652768BB}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (UserInterfaceObjectConnection != null)
                return UserInterfaceObjectConnection.PresentationObjectType;

            return null;
        }


        #endregion

        #region IObjectMemberViewControl Members

        /// <MetaDataID>{2288cd33-6883-4136-9da8-26f6a2eb2080}</MetaDataID>
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

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            if (memberChangeEventArg.Type == OOAdvantech.UserInterface.Runtime.ChangeType.ValueChanged)
                LoadControlValues();
            else if (memberChangeEventArg.Type == OOAdvantech.UserInterface.Runtime.ChangeType.LockChanged)
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
        }
    }
}
