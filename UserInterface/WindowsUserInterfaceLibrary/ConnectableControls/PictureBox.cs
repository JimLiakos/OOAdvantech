using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;
using System.Drawing;
using OOAdvantech.UserInterface.Runtime;
using System.Windows.Forms;

namespace ConnectableControls
{
    /// <MetaDataID>{493b3596-b00f-4fd8-ba12-5d15cd76ce73}</MetaDataID>
    public class PictureBox : System.Windows.Forms.PictureBox, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {

        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        public virtual void InitializeControl()
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

        #region IObjectMemberViewControl Members

        #region public bool AllowDrag ,public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        public bool AllowDrag
        {
            get { return false; }
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
        #endregion

        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (UserInterfaceObjectConnection != null)
                return UserInterfaceObjectConnection.PresentationObjectType;

            return null;
        }

   

        #region public object Value
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
        #endregion

        #region public OOAdvantech.MetaDataRepository.Classifier ValueType
        OOAdvantech.MetaDataRepository.Classifier _ValueType;
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
        
        #endregion

        #region public Object Path
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
        #endregion

        public void SetValueType(OOAdvantech.MetaDataRepository.Classifier valueType)
        {
            _ValueType = valueType;

        }

        #region public void LoadControlData()
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
                    this.Image = value as Image;                
            }
        } 
        #endregion

        #region public void SaveControlData()
        public void SaveControlValues()
        {
            if (!Enabled)
                return;
            try
            {
                if (IsConnectionDataCorrect)
                {
                    if (Image==null && (_ValueType.GetExtensionMetaObject(typeof(Type)) as Type) != typeof(Image))
                        UserInterfaceObjectConnection.SetValue(OOAdvantech.AccessorBuilder.GetDefaultValue(_ValueType.GetExtensionMetaObject(typeof(Type)) as Type), Path as string);
                    else
                        UserInterfaceObjectConnection.SetValue(Image, Path as string);//Convert.ChangeType(Image, _ValueType.GetExtensionMetaObject(typeof(Type)) as Type), 
                }

            }
            finally
            {
            }
        } 
        #endregion

        #endregion

        #region IOperetionCallerSource Members

        #region public string[] PropertiesNames
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[1] { "Image" };
            }
        } 
        #endregion

        #region public object GetPropertyValue(string propertyName)
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;
            if (propertyName == "Value")
                return Value;
            if (propertyName == "Image")
                return Value;

            throw new Exception("There isn't property with name " + propertyName + ".");

        } 
        #endregion

        #region public void SetPropertyValue(string propertyName, object value)
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");
        } 
        #endregion

        #region public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;
        } 
        #endregion

        #region public bool ContainsProperty(string propertyName)
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
        
        #endregion

        #endregion

        #region IConnectableControl Members

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

        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion

        #region IMetadataSelectionResolver Members

        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Path" && metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            if (metaObject is OOAdvantech.UserInterface.OperationCall && new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                return true;
            else
                return false;
        }

        #endregion

        public PictureBox ()
	    {
            this.Cursor = System.Windows.Forms.Cursors.Hand;            
	    }

        private string _StartDirectory = "c:\\";

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = _StartDirectory;
            openFileDialog.Filter = "image files (*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png;)|*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png;";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (ExistConnectionData)
                {
                    _StartDirectory = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    Image img = Image.FromFile(openFileDialog.FileName);
                    this.Image = img.GetThumbnailImage(this.Width, this.Height, new Image.GetThumbnailImageAbort(ImageAbort), System.IntPtr.Zero);
                    this.UserInterfaceObjectConnection.SetValue(this.Image, this.Path as string);
                    //this.SizeMode= System.Windows.Forms.PictureBoxSizeMode.
                }                                    
            }

        }
               
        #region private bool ImageAbort()
        private bool ImageAbort()
        {
            return false;
        } 
        #endregion

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
                    //if (ExistConnectionData)
                    //    AutoProducedEnabledValue = false;
                    return false;
                }
                _ValueType = UserInterfaceObjectConnection.GetClassifier(Path as string);
                if (_ValueType == null)
                {
                    //if (ExistConnectionData)
                    //    AutoProducedEnabledValue = false;
                    return false;
                }

                return true;
            }
        } 
        #endregion

        #region public ViewControlObject ViewControlObject
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
        #endregion

        #region IPathDataDisplayer Members

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

        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            //throw new NotImplementedException();
        }

        public void LockStateChange(object sender)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
