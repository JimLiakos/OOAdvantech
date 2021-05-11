using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WinUserControl = System.Windows.Forms.UserControl;
using ConnectableControls.PropertyEditors;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;





namespace ConnectableControls
{
    /// <MetaDataID>{6d89e34e-bc7f-45cc-8ddb-56bbc2db3168}</MetaDataID>
    [ToolboxItem(false)]
    public partial class UserControl : WinUserControl, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
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
        /// <MetaDataID>{d3f1cb7d-762f-4aaf-a10d-6ef7f545a2ba}</MetaDataID>
        UpdateStyle _UpdateStyle;
        /// <MetaDataID>{b7d51448-8300-4831-b077-db146ded7504}</MetaDataID>
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

        public void LockStateChange(object sender)
        {

        }
        /// <MetaDataID>{8c75d6aa-deca-419e-aac3-7d80dda27a08}</MetaDataID>
        bool _AllowDrag = false;
        public bool AllowDrag
        {
            get
            {
                return _AllowDrag;
            }
            set
            {
                Connection.AllowDrag = value;
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


        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        /// <MetaDataID>{d339189d-98c8-49df-b909-ac0a91b78262}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{b2a88ef9-3b9f-41f3-bac9-3944d6e95b8f}</MetaDataID>
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
                    foreach (string path in Connection.UserInterfaceObjectConnection.RootObjectNode.Paths)
                        _AllPaths.Add(_Path + path.Substring("Root".Length));

                }

                return _AllPaths;
            }
        }
        /// <MetaDataID>{433d84b4-a9e8-4465-857c-aea636b6bf88}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string[] PropertiesNames
        {
            get
            {
                return new string[0];
            }
        }
        public virtual object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;

            if (propertyName == "this.Value")
                return Value;

            if (propertyName == "Value")
                return Value;
            throw new Exception("There isn't property with name " + propertyName + ".");

        }
        public virtual bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            //if (propertyName == "this.Value")
            //    return true;

            if (propertyName == "Value")
                return true;

            return false;
        }

        public virtual void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        public virtual OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;

        }

        /// <MetaDataID>{ffc90105-e03a-4df5-b7e7-2c527a9e1c75}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }

        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            return false;

        }


        /// <MetaDataID>{74836448-fc13-45fe-b027-e335eaab8f70}</MetaDataID>
        object ObjectDisplayedValue;

        /// <MetaDataID>{043587cb-47e4-4443-a238-6f46d2e6e3a9}</MetaDataID>
        public UserControl()
        {
            InitializeComponent();
            Connection.ContainerControl = this;
        }
        /// <MetaDataID>{14abbac4-8782-4082-9908-b45719ded17c}</MetaDataID>
        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }


        public virtual bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            return true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (UpdateStyle == UpdateStyle.OnLostFocus)
                SaveControlValues();

        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
        public override bool Focused
        {
            get
            {
                List<Control> childControls = new List<Control>();
                GetAllChildControls(this, childControls);
                foreach (Control control in childControls)
                {
                    if (control.Focused)
                        return true;
                }
                return false;
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {

            base.OnHandleCreated(e);
            List<Control> childControls = new List<Control>();
            GetAllChildControls(this, childControls);
            foreach (Control control in childControls)
            {
                control.LostFocus += new EventHandler(OnControlLostFocus);
                control.GotFocus += new EventHandler(OnControlGotFocus);
            }

        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            List<Control> childControls = new List<Control>();
            GetAllChildControls(this, childControls);
            foreach (Control control in childControls)
            {
                control.LostFocus -= new EventHandler(OnControlLostFocus);
                control.GotFocus -= new EventHandler(OnControlGotFocus);
            }

        }
        /// <MetaDataID>{c6832b23-e758-42e9-81e6-8d00ae278485}</MetaDataID>
        void GetAllChildControls(Control control, List<Control> childControls)
        {

            foreach (Control childControl in control.Controls)
            {
                childControls.Add(childControl);
                GetAllChildControls(childControl, childControls);
            }
        }

        /// <MetaDataID>{891b00df-fab3-46fa-9597-37e51b8024c8}</MetaDataID>
        void OnControlGotFocus(object sender, EventArgs e)
        {
            OnGotFocus(e);
        }

        /// <MetaDataID>{9364d7c2-932f-40a5-bb63-9091e200feca}</MetaDataID>
        void OnControlLostFocus(object sender, EventArgs e)
        {
            if (!Focused)
                OnLostFocus(e);
        }




        /// <MetaDataID>{ada92d06-69a7-4b9e-903e-5dfbec459899}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifier(ITypeDescriptorContext context)
        {
            return UserInterfaceObjectConnection.PresentationObjectType;
        }

        #region Auto disable code
        /// <MetaDataID>{f76ecff0-aae5-463e-a8e0-ad443d7ace60}</MetaDataID>
        bool _UserDefinedEnableValue = true;
        /// <MetaDataID>{4ed79ccc-868d-47c3-ba65-9979aaab4443}</MetaDataID>
        bool SuspendDisplayedValueChangedHandler = false;
        /// <MetaDataID>{eeb216ec-8073-4186-8c92-8b814d9e58b4}</MetaDataID>
        bool _AutoProducedEnableValue = true;
        /// <MetaDataID>{b16ea1af-ac51-4a79-aa2b-887b51d1c129}</MetaDataID>
        bool EnableAutoProduced = false;
        /// <MetaDataID>{0b698169-e50c-4ceb-90dd-971b142ee645}</MetaDataID>
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


        public virtual void InitializeControl()
        {

        }
        public virtual void LoadControlValues()
        {
            if (UserInterfaceObjectConnection != null)
            {
                ObjectDisplayedValue = null;

                Type type = this.UserInterfaceObjectConnection.GetType(Path as string);
                if (type == null)
                    return;
                this.UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;
                ObjectDisplayedValue = this.UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                _Value = ObjectDisplayedValue;
                Connection.ContainerControl = this;
                Connection.Instance = _Value;
                //AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
            }


        }
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {
           

        }

        /// <MetaDataID>{9de55efe-2ea4-44af-9fbc-109b3550703a}</MetaDataID>
        string _Path;
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object Path
        {
            get
            {
                return _Path;
            }
            set
            {
                string newPath = null;
                if (value is string)
                    newPath = value as string;
                else if (value is MetaData)
                    newPath = (value as MetaData).Path;

                if (_Path != newPath)
                    _ValueType = null;
                _Path = newPath;
            }
        }
        //public bool IsReadOnly
        //{
        //    get
        //    {
        //        return UserInterfaceObjectConnection.IsReadOnly(_Path);
        //    }
        //}
        public void SaveControlValues()
        {

            UserInterfaceObjectConnection.SetValue(Connection.Instance, _Path);

        }

        /// <MetaDataID>{76492184-3e2c-42b3-9268-fcf64a4c238d}</MetaDataID>
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

        /// <MetaDataID>{2502f9a6-c327-4419-847e-8fb942b47244}</MetaDataID>
        object _Value;
        /// <MetaDataID>{3f942663-26b1-4153-a3ec-a1ec1b78ad69}</MetaDataID>
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

        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.Name == "Path")
                return UserInterfaceObjectConnection.PresentationObjectType;

            return UserInterfaceObjectConnection.PresentationObjectType;
        }


        /// <MetaDataID>{fc5a2fef-3871-4770-bbe8-701439ff0a07}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier _ValueType;
        /// <MetaDataID>{142b1564-41cc-4217-9b6f-7871a09d35fb}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                if (_ValueType != null)
                    return _ValueType;
                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path as string))
                    _ValueType = this.UserInterfaceObjectConnection.GetClassifier(_Path as string);

                return _ValueType;

            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return Connection.UserInterfaceObjectConnection.MasterViewControlObject;
            }
            set
            {
                if (value != null)
                {
                    value.AddControlledComponent(this);
                    Connection.UserInterfaceObjectConnection.InstanceChanged += new EventHandler(ConnectionInstanceChanged);
                    Connection.UserInterfaceObjectConnection.MasterViewControlObject = value;
                }
                else
                {
                    //TODO To MasterViewControlObject μπορεί να μήν είναι το ιδιο με το value που προσθεθηκε
                    if (Connection.UserInterfaceObjectConnection.MasterViewControlObject != null)
                    {
                        Connection.UserInterfaceObjectConnection.MasterViewControlObject.RemoveControlledComponent(this);

                    }
                    Connection.UserInterfaceObjectConnection.MasterViewControlObject = value;

                    Connection.UserInterfaceObjectConnection.InstanceChanged -= new EventHandler(ConnectionInstanceChanged);
                }


            }
        }
        /// <MetaDataID>{557e9df6-dcd1-452f-86f9-317ba3186fe2}</MetaDataID>
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

        /// <MetaDataID>{b0919f6c-148f-4939-aa35-df3bcb8082fb}</MetaDataID>
        void ConnectionInstanceChanged(object sender, EventArgs e)
        {
            SaveControlValues();
        }

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            if (UserInterfaceObjectConnection != null)
            {
                ObjectDisplayedValue = null;

                Type type = this.UserInterfaceObjectConnection.GetType(Path as string);
                if (type == null)
                    return;
                this.UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;
                ObjectDisplayedValue = this.UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                _Value = ObjectDisplayedValue;
                Connection.ContainerControl = this;
                Connection.Instance = _Value;
            }
            

        }
        public static DynamicViewContainer.UserViewControlIdentity GetUserControlIdentity<T>() where T : UserControl 
        {
            return new DynamicViewContainer.UserViewControlIdentity( typeof(T));
        }

    }



}
