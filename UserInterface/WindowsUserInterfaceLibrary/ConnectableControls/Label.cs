using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;

namespace ConnectableControls
{
    /// <MetaDataID>{0ada63fb-a73e-4c1d-b889-7470d7c127e1}</MetaDataID>
    public class Label : System.Windows.Forms.Label, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        public Label()
        {
            _TextProperty = new DependencyProperty(this, "Text");
            _DependencyProperties.Add(_TextProperty);
        }
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
            return false;
        }

        #endregion

        #region IOperationCallerSource Members

        public string[] PropertiesNames
        {
            get 
            { 
                return new string[0];
            }
        }

        public object GetPropertyValue(string propertyName)
        {
            throw new Exception("There isn't property with name " + propertyName + ".");
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            return null;
        }

        public bool ContainsProperty(string propertyName)
        {
            return false;
        }

        #endregion

        #region IConnectableControl Members

        public void InitializeControl()
        {
            
        }

        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;

            }
            set
            {
                if (_UserInterfaceObjectConnection != value)
                {
                    _UserInterfaceObjectConnection = value;
                    if (_UserInterfaceObjectConnection != null)
                        _UserInterfaceObjectConnection.AddControlledComponent(this);
                }

            }
        }

        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get { return _DependencyProperties; }
        }

        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion

        #region IMetadataSelectionResolver Members

        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor=="Text"&&
                metaObject is OOAdvantech.MetaDataRepository.Attribute && 
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.FullName == typeof(string).FullName)
                return true;
                

            return false;
        }

        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPathDataDisplayer Members
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

        DependencyProperty _TextProperty;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public DependencyProperty TextEnableProperty
        {
            get
            {

                return _TextProperty;
            }
            set
            {
                if (value != null)
                {
                    _TextProperty = value;
                    _TextProperty.ConnectableControl = this;
                }
            }
        }


        public void LoadControlValues()
        {
        }

        public void SaveControlValues()
        {
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
        public bool HasLockRequest
        {
            get { return false; }
        }

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            
        }

        public void LockStateChange(object sender)
        {
            
        }

        #endregion
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
    }
}
