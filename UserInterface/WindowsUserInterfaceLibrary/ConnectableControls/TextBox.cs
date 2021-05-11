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
    public class TextBox : System.Windows.Forms.TextBox, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {

        /// <MetaDataID>{8fa51e96-5dea-4836-8f3f-d60d678873cf}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{72478df2-d102-4db0-88e9-1c68a89bccef}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{e930793d-1e0a-4426-bcb9-b9dd8bda1651}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{68a302a2-71ea-458f-91a6-12976d0d4268}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{a8adc6e3-b402-492a-9fef-e8c4d989077e}</MetaDataID>
        public TextBox()
        {
            _EnableProperty = new DependencyProperty(this,"Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }
         
        /// <exclude>Excluded</exclude>
        DependencyProperty _EnableProperty;

        /// <MetaDataID>{b1718e15-93bc-4e75-b47e-4dc4983239b0}</MetaDataID>
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




        /// <MetaDataID>{b46d724d-62a6-41fe-882c-fc9a5adc652e}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{40129d1a-2996-4092-b52c-b25857491700}</MetaDataID>
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
        /// <MetaDataID>{a4840f01-49cd-4c56-badc-7dcd27a1cf96}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }


        /// <MetaDataID>{f46bbd0b-01ac-4e36-8e0a-a04f796df9f1}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }


        /// <MetaDataID>{b2520473-aac8-4389-b89a-edbe759506f4}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{b3526aae-17e2-43dc-a6de-4158d23955d5}</MetaDataID>
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



        /// <MetaDataID>{a08b0e4d-f504-4c0a-bb29-738f9904c5f8}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[1] { "Text" };
            }
        }
        /// <MetaDataID>{302f00e5-1a37-4158-84d2-da886ddfb5e3}</MetaDataID>
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
                return Text;

            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        /// <MetaDataID>{9dd842ed-67c9-41b9-a09d-255f324496e2}</MetaDataID>
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

        /// <MetaDataID>{1845e267-194b-45ae-9d9f-0bec15c05d1f}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else if (propertyName == "Text")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        /// <MetaDataID>{c4bd9669-a015-4310-ae16-38d4f7fbd772}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
            {
                if (string.IsNullOrEmpty(Path as string))
                    return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(string));
                else
                    return ValueType;
            }
            if (propertyName == "Text")
                return OOAdvantech.MetaDataRepository.Classifier.GetClassifier( typeof(string));
            return null;

        }
        /// <MetaDataID>{f7d09672-0bf4-4a57-98a1-48f0a6e6acdc}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }

        /// <MetaDataID>{d7cff684-b628-4574-ab06-47941472bd15}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && (UserInterfaceObjectConnection == null || UserInterfaceObjectConnection.GetClassifier(_Path as string) == null))
            {
                if (FindForm() != null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TextBox '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                else
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: TextBox '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                return true;
            }
            return false;

        }


        /// <MetaDataID>{9fbea2d2-9599-4590-8bb7-7203a292e8a4}</MetaDataID>
        object _Value;
        /// <MetaDataID>{7fb2048c-2048-4915-8cfc-6ada2cab43f6}</MetaDataID>
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
        /// <MetaDataID>{d5decb63-ebf3-4dc1-8042-ca7065090bca}</MetaDataID>
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
        /// <MetaDataID>{8dcd6a20-bd74-49ed-94c5-2b0a57904e9c}</MetaDataID>
        public void SetValueType(OOAdvantech.MetaDataRepository.Classifier valueType)
        {
            _ValueType = valueType;

        }

        /// <MetaDataID>{9a55f1e9-cd96-4e12-a3a6-e57713b3cfc9}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (UserInterfaceObjectConnection != null)
                return UserInterfaceObjectConnection.PresentationObjectType; 

            return null;
        }

        /// <MetaDataID>{55e5fba0-3c0c-4e28-a157-a98c76028a92}</MetaDataID>
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
        /// <MetaDataID>{4f2e1ab0-1f25-46b2-a9a1-342f5df1bb33}</MetaDataID>
        UpdateStyle _UpdateStyle;
        /// <MetaDataID>{581257a7-9319-4e7d-b8bd-e75dea9f0b59}</MetaDataID>
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

        /// <MetaDataID>{672851e7-3461-4893-bf5b-74fd7be5848e}</MetaDataID>
        string LastText;
        /// <MetaDataID>{a0194e48-ae76-4547-a744-70ef175ff49f}</MetaDataID>
        int LastSelectionStart=0;

        /// <MetaDataID>{c8fd79d1-09b9-47d9-8941-3879566e6632}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier _ValueType;
        /// <MetaDataID>{a369ede4-a838-4e63-821e-d9d2d1a907c9}</MetaDataID>
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
        /// <MetaDataID>{d9866e81-f6f9-4135-86ff-f198818b2db5}</MetaDataID>
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

        /// <MetaDataID>{c182c72a-aa59-4d31-bbea-68f3c8c93498}</MetaDataID>
        bool InSelectionChange = false;

        /// <MetaDataID>{76eb6f3f-bf68-4afd-95be-1be5b9401886}</MetaDataID>
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
        /// <MetaDataID>{89467cd8-a0ea-4028-82bf-e257a9bcfcdd}</MetaDataID>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            LastSelectionStart = SelectionStart;
        }

        /// <MetaDataID>{b578895e-9a73-450d-bfe1-0d42e8cc2988}</MetaDataID>
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
        /// <MetaDataID>{2255750c-ee39-48d8-bdab-aba92f1389cf}</MetaDataID>
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


        /// <MetaDataID>{9135e90c-e27b-4c6f-917f-8805f7eba811}</MetaDataID>
        private bool _AutoDisable = true;
        /// <MetaDataID>{1a53fa94-92f3-495a-96d0-b87ac2e539eb}</MetaDataID>
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


        /// <MetaDataID>{5503d064-3718-49fa-a9a8-258602fdfb21}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection  _ViewControlObject = null;
        /// <MetaDataID>{c5f21d79-55ce-4799-8c8b-9d5f8b802651}</MetaDataID>
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
        /// <MetaDataID>{1a9471c5-afa4-41d1-8238-9578d629c4af}</MetaDataID>
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
        /// <MetaDataID>{5d95c6bd-dfbe-40b9-8227-c3d349ea0c43}</MetaDataID>
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

        /// <MetaDataID>{ed5a32d1-7a13-438a-8540-e5824ffde48f}</MetaDataID>
        public void LockStateChange(object sender)
        {
            AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
        }

        #region Auto disable code
        /// <MetaDataID>{10cc56bc-331e-416c-b8e9-b3c96862981e}</MetaDataID>
        bool _UserDefinedEnableValue=true;
        /// <MetaDataID>{1ec0b695-8c8b-43ef-bf04-bf52f9219db6}</MetaDataID>
        bool SuspendDisplayedValueChangedHandler = false;
        /// <MetaDataID>{e36ad260-73fa-4b9f-b444-85636f87df95}</MetaDataID>
        bool _AutoProducedEnableValue = true;
        /// <MetaDataID>{ce3521f3-c060-4722-b246-379ee2ada189}</MetaDataID>
        bool EnableAutoProduced = false;
        /// <MetaDataID>{1763358a-eb53-4dae-853e-d8ecc29f589b}</MetaDataID>
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
        /// <MetaDataID>{e0d704b5-bb8b-497e-b8a1-5ae53a789b4d}</MetaDataID>
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


        /// <MetaDataID>{cab9cf81-719a-4ec3-b7fb-7ac35d35f23b}</MetaDataID>
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


        /// <MetaDataID>{0d67ca8b-8000-4d01-a50b-b9961ef67b1a}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Path" && metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.Attribute &&
                propertyDescriptor == "Enabled" &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) == typeof(bool))
                return true;
            else

                return false;
        }

        #endregion

        #region IObjectMemberViewControl Members

        /// <MetaDataID>{21f24d17-444d-4ff5-ae9c-b07882d26f0b}</MetaDataID>
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



