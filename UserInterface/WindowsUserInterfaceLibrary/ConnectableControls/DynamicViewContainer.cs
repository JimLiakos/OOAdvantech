using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;
using System.Windows.Forms;

namespace ConnectableControls
{
    /// <MetaDataID>{c5245b6d-d2bc-4351-b6c1-b54b1994c1fa}</MetaDataID>
    public class DynamicViewContainer : System.Windows.Forms.UserControl, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        /// <MetaDataID>{1bdf36bf-182d-4dde-85e4-235cfab4eeb6}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        public struct UserViewControlIdentity
        {
            internal System.Type Type;
            internal UserViewControlIdentity(System.Type type)
            {
                Type = type;
            }
            static UserViewControlIdentity()
            {
                Empty = new UserViewControlIdentity(null); 
                
            }

            public readonly static UserViewControlIdentity Empty;
            public static bool operator ==(UserViewControlIdentity a, UserViewControlIdentity b)
            {
                return a.Type == b.Type;
            }
            public static bool operator !=(UserViewControlIdentity a, UserViewControlIdentity b)
            {
                return !(a == b);
            }

        



        }
        /// <MetaDataID>{b3b2b455-9e28-49d3-8883-a1cb4278c6fa}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{83b0a18b-5ede-4221-9545-62cb55313049}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{cf70c080-af0d-4b4d-a6ce-7f7d62ff7897}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }
        /// <MetaDataID>{9005a82c-1361-48d3-a9cd-eecf0b18c8d5}</MetaDataID>
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


        #region IPathDataDisplayer Members

        /// <MetaDataID>{a0a3eb34-e413-4701-8a80-1218f7455789}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        /// <MetaDataID>{670fa0c3-6115-4bf3-b9dd-f33f1569206f}</MetaDataID>
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


        /// <MetaDataID>{14c845d0-9fa1-4e5b-9b7e-f1aaeb271f57}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                return new OOAdvantech.Collections.Generic.List<string>();
            }
        }
        /// <MetaDataID>{49017a4d-893e-44a2-b9a3-b5231b2a861e}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{dbfba7f0-74f6-4839-adac-4d25d9afff3f}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            LoadControlValues();
        }

        /// <MetaDataID>{2aeb0d41-61bc-4386-8402-e99fd4556d64}</MetaDataID>
        public void LockStateChange(object sender)
        {


        }

        #endregion

        #region IObjectMemberViewControl Members

        /// <MetaDataID>{6a315cd6-e5a9-4c53-a157-4db579999ec4}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowDrag
        {
            get { return false; }
        }

        /// <MetaDataID>{d8760f41-381d-449f-b991-b7db1d4ff7db}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && ValueType == null)
            {
                if (FindForm() != null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: DynamicViewContainer '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                else
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: DynamicViewContainer '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                hasErrors = true;
            }
            if (!string.IsNullOrEmpty(_HostedViewIdentityPath))
            {
                 Type type = this.UserInterfaceObjectConnection.GetType(_HostedViewIdentityPath);
                 if (type != null)
                 {
                     if (type != typeof(UserViewControlIdentity))
                     {
                         if (FindForm() != null)
                             errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: DynamicViewContainer '" + Name + "' HostedViewIdentity must be type of UserViewControlIdentity.", FindForm().GetType().FullName));
                         else
                             errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: DynamicViewContainer '" + Name + "' HostedViewIdentity must be type of UserViewControlIdentity.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                         hasErrors = true;
                     }
                 }
                 else
                 {
                     if (FindForm() != null)
                         errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: DynamicViewContainer '" + Name + "' invalid HostedViewIdentityPath.", FindForm().GetType().FullName));
                     else
                         errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: DynamicViewContainer '" + Name + "' invalid HostedViewIdentityPath.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                 }

            }
            return hasErrors;
        }

        /// <MetaDataID>{47887a06-4e7a-4899-8324-a19c027a278f}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UserInterfaceObjectConnection.PresentationObjectType;
        }


        /// <MetaDataID>{9479c76c-4a72-4282-8cc0-1e05672280e1}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        /// <MetaDataID>{ec8d9a09-a724-4396-8741-aa23deb1d74e}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier _ValueType;

        /// <MetaDataID>{e2f0a88b-d608-4307-89d9-0e563a0c31e5}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                if (_ValueType != null)
                    return _ValueType;
                if (!string.IsNullOrEmpty(_Path))
                    _ValueType = UserInterfaceObjectConnection.GetClassifier(_Path);
                return _ValueType;
            }

        }

        /// <MetaDataID>{6f72711c-6f93-483b-bd9f-0ea5e4464415}</MetaDataID>
        string _Path;
        /// <MetaDataID>{9544c2d6-6e3e-4a34-87d7-fd642c5218ed}</MetaDataID>
        [Category("Object Model Connection")]
        [DisplayName("Displayed Object Path")]
        [DescriptionAttribute("Specifies the path of object, which displayed from hosted Object View Control.")]
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
                if (Controls.Count > 0)
                    (Controls[0] as UserControl).Path = _Path;

            }
        }



        /// <MetaDataID>{8bb791a6-1d60-4975-853e-1f57002f7924}</MetaDataID>
        string _HostedViewIdentityPath;
        /// <MetaDataID>{327931f7-19c0-42a3-9851-8f13c6fd2f72}</MetaDataID>
        [Category("Object Model Connection")]
        [DescriptionAttribute("Specifies the path of hosted Object View Control Identity. The connectable controls framework uses the identity to instantiate the proper Object View Control and presents the object.")]
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object HostedViewIdentityPath
        {
            get
            {
                return _HostedViewIdentityPath;
            }
            set
            {
                if (value is string)
                    _HostedViewIdentityPath = value as string;
                else if (value is MetaData)
                    _HostedViewIdentityPath = (value as MetaData).Path;
            }
        }


        /// <MetaDataID>{a7b58b64-19f0-40a0-be14-afa7305365b1}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier _DefaultHostedViewType;
        /// <MetaDataID>{3f40b6b9-1b3f-457e-8462-c3fdb5c88cae}</MetaDataID>
        string _DefaultHostedViewTypeFullName;
        /// <MetaDataID>{a73d24dc-4fcd-4e95-b4c2-724c43a7dd6a}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
         [Category("Object Model Connection")]
        [DescriptionAttribute("")]
        public object DefaultHostedViewType
        {
            get
            {
                if (_DefaultHostedViewType != null)
                    return _DefaultHostedViewType.FullName;
                else
                {
                    if (UserInterfaceObjectConnection != null)
                        _DefaultHostedViewType = UserInterfaceObjectConnection.GetClassifier(_DefaultHostedViewTypeFullName);
                }

                return _DefaultHostedViewTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Classifier)
                {
                    _DefaultHostedViewTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                    _DefaultHostedViewType = value as OOAdvantech.MetaDataRepository.Classifier;
                    if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.DesigneMode)
                        DisplayDynamicView();
                }
                if (value is string)
                {
                    _DefaultHostedViewTypeFullName = value as string;
                    if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.DesigneMode)
                        DisplayDynamicView();
                }
            }
        }

        /// <MetaDataID>{5e790368-2724-41d8-a079-43bd08956d56}</MetaDataID>
        private void DisplayDynamicView()
        {
            Controls.Clear();
            Type type = null;
            type = ModulePublisher.ClassRepository.GetType(_DefaultHostedViewTypeFullName, "");

            if (type != null && type.IsSubclassOf(typeof(UserControl)))
            {
                UserControl userContol = Activator.CreateInstance(type) as UserControl;
                userContol.ViewControlObject = ViewControlObject;
                Controls.Add(userContol);
                userContol.Path = _Path;
                userContol.Dock = (System.Windows.Forms.DockStyle)(int)DockHostedView;
            }
        }



        /// <MetaDataID>{ae63e564-af94-4ca4-a81c-be57d8f5a16f}</MetaDataID>
        System.Collections.Generic.Dictionary<System.Type, UserControl> HostedUserControls = new Dictionary<Type, UserControl>();
        /// <MetaDataID>{f9c5fb6b-413d-4470-9ada-9b44bc02d4dd}</MetaDataID>
        object ObjectDisplayedValue;

        /// <MetaDataID>{4fa7719d-ab46-41cc-bb2f-cc6e950a9cfb}</MetaDataID>
        private HostedViewDock _DockHostedView;
        /// <MetaDataID>{bbf6615b-75d7-4e76-85f9-46d5b8c71261}</MetaDataID>
        [Category("Layout")]
        [DefaultValue(HostedViewDock.None)]
        [ Browsable(true)]
        public HostedViewDock DockHostedView
        {
            get { return _DockHostedView; }
            set
            {
                _DockHostedView = value;

                //bool returnValueAsCollection = false;
                //Type _HostedViewType = UserInterfaceObjectConnection.GetDisplayedValue(_HostedViewIdentityPath, this, out returnValueAsCollection) as Type;

                //UserControl userContol = null;
                //if (_HostedViewType!=null&&HostedUserControls.TryGetValue(_HostedViewType, out userContol))
                //    userContol.Dock = (System.Windows.Forms.DockStyle)(int)_DockHostedView;
            }
        }

        /// <MetaDataID>{63dc425b-22ce-4c73-8c9d-6f85fae60711}</MetaDataID>
        public void LoadControlValues()
        {

            this.UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
            if (UserInterfaceObjectConnection != null)
            {
                Type type = this.UserInterfaceObjectConnection.GetType(_HostedViewIdentityPath);
                if (type != null)
                {


                    bool returnValueAsCollection = false;

                    UserViewControlIdentity userViewControlIdentity = new UserViewControlIdentity();
                    object viewControlIdentityObject = UserInterfaceObjectConnection.GetDisplayedValue(_HostedViewIdentityPath, this, out returnValueAsCollection);
                    if(viewControlIdentityObject is UserViewControlIdentity)
                        userViewControlIdentity =(UserViewControlIdentity)UserInterfaceObjectConnection.GetDisplayedValue(_HostedViewIdentityPath, this, out returnValueAsCollection);

                    Type _HostedViewType = null;
                    if (userViewControlIdentity.Type != null)
                        _HostedViewType = userViewControlIdentity.Type;

                    if (_HostedViewType != null && _HostedViewType.IsSubclassOf(typeof(UserControl)))
                    {
                        if (Controls.Count == 0 || Controls[0].GetType() != _HostedViewType)
                        {
                            if (Controls.Count > 0)
                                (Controls[0] as UserControl).ViewControlObject = null;
                            Controls.Clear();
                            UserControl userContol = null;
                            if (!HostedUserControls.TryGetValue(_HostedViewType, out userContol))
                            {
                                userContol = Activator.CreateInstance(_HostedViewType) as UserControl;
                                HostedUserControls[_HostedViewType] = userContol;
                            }
                            userContol.ViewControlObject = ViewControlObject;
                            Controls.Add(userContol);
                            userContol.Path = _Path;
                            userContol.Dock = (System.Windows.Forms.DockStyle)(int)DockHostedView;
                        }
                    }
                    else
                        Controls.Clear();

                }
            }


            if (UserInterfaceObjectConnection != null && Controls.Count > 0)
            {

                UserControl control = Controls[0] as UserControl;

                ObjectDisplayedValue = null;

                Type type = this.UserInterfaceObjectConnection.GetType(Path as string);
                if (type != null)
                {

                    //
                    bool returnValueAsCollection = false;
                    ObjectDisplayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                }
                //_Value = ObjectDisplayedValue;
                //.ContainerControl = this;
                control.Connection.Instance = ObjectDisplayedValue;
                //if (UserInterfaceObjectConnection.State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.Initialize)
                //    control.LoadControlData();
                //control.AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
            }

        }

        /// <MetaDataID>{63a6dbbf-e926-4d83-8ab0-7a186565701d}</MetaDataID>
        public void SaveControlValues()
        {

        }

        #endregion

        #region IOperetionCallerSource Members

        /// <MetaDataID>{52bdea50-7e27-4ecf-b9d2-3bf2a3ec653e}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get { return new string[0]; }
        }

        /// <MetaDataID>{14b40afb-635b-4f78-bd55-68beb7810967}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;

            if (propertyName == "this.Value")
                return Value;

            if (propertyName == "Value")
                return Value;
            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        /// <MetaDataID>{1ff06919-5386-4aa3-9217-7dd123e1ff0c}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        /// <MetaDataID>{b67fb42f-e5ea-416e-bc7d-ad92202a3d78}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;

        }

        /// <MetaDataID>{39eb219e-ad47-47f8-8948-21982a066394}</MetaDataID>
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

        #endregion

        #region IConnectableControl Members

        /// <MetaDataID>{df9d1109-f2c3-4387-b791-97971e2587bb}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IConnectableControl.UserInterfaceObjectConnection
        {
            get
            {
                return UserInterfaceObjectConnection;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <MetaDataID>{be764881-22fd-4001-8d10-f527393e15e3}</MetaDataID>
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

        /// <MetaDataID>{adab4a14-1427-4f99-9951-196d58ac5006}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion

        #region IMetadataSelectionResolver Members

        /// <MetaDataID>{ce4dc619-63ec-4dcf-acb1-3a2acf4494ed}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "HostedViewIdentityPath")
            {
                if (metaObject is OOAdvantech.MetaDataRepository.Attribute &&
                    (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.FullName == typeof(UserViewControlIdentity).FullName.Replace("+","."))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            return true;
        }

        #endregion
    }

    /// <MetaDataID>{131a5fad-f2f8-4f8d-a8c3-6a6135b61be9}</MetaDataID>
    public enum HostedViewDock
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
        Fill = 5
    }
}
