using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using System.Xml.Linq;

namespace ConnectableControls
{
    /// <MetaDataID>{EAC4CEC5-52A2-4063-BC47-34D0881B229F}</MetaDataID>
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.Button), "Button.bmp")]
    public class Button : System.Windows.Forms.Button, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl
    {
        /// <MetaDataID>{77cf9e9e-5434-4428-8ca7-c6a4bcf01f1b}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{715aceb7-3fe4-4a95-99d3-24e4daaeb073}</MetaDataID>
        public virtual void InitializeControl()
        {

        }


        /// <exclude>Excluded</exclude>
        DependencyProperty _TextProperty;

        /// <MetaDataID>{9e1260cf-f30d-4780-a657-48bdbaab5b5d}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public DependencyProperty TextProperty
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

        /// <MetaDataID>{4046aa94-b49c-4fd9-968b-7cef4a0c0a6a}</MetaDataID>
        [ObsoleteAttribute("This property will be removed from future Versions.", false)]
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

        /// <MetaDataID>{2a9f7715-f1d8-4acf-92ba-f7d35521329e}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{c3ea2eac-ebcb-4838-9948-2c4b09ce3b82}</MetaDataID>
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

        /// <MetaDataID>{9dbb800b-8729-4380-90ab-5164a91fd052}</MetaDataID>
        bool _SaveButton;
        /// <MetaDataID>{c41acb2c-35ca-4623-9b4b-a1cf46f6a43a}</MetaDataID>
        [Category("Object Model Connection")]
        public bool SaveButton
        {
            get
            {
                return _SaveButton;

            }
            set
            {
                _SaveButton = value;
                if (_SaveButton)
                    DialogResult = DialogResult.None;
            }
        }
        /// <MetaDataID>{1bc7e17b-89f5-487b-a740-28fe678f95a6}</MetaDataID>
        public override DialogResult DialogResult
        {
            get
            {
                return base.DialogResult;
            }
            set
            {
                if (!SaveButton)
                    base.DialogResult = value;
                else
                    base.DialogResult = DialogResult.None;

            }
        }

        /// <MetaDataID>{39aca259-7fc7-4b8b-8048-4f58f6fe7ba1}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        /// <MetaDataID>{51c422d1-8ae0-416a-b528-957d7d8aab62}</MetaDataID>
        public Button()
        {
            System.Drawing.Image image = System.Drawing.ToolboxBitmapAttribute.GetImageFromResource(typeof(ViewControlObject), "Table.bmp", false);

            _TextProperty = new DependencyProperty(this, "Text");
            _DependencyProperties.Add(_TextProperty);
        }

        /// <MetaDataID>{8e2dc9d4-7a3f-4e02-b259-3b30128e1802}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[0];
            }
        }
        /// <MetaDataID>{37ee53c9-543a-48fb-9ac8-980b5429d5df}</MetaDataID>
        static OOAdvantech.Collections.Generic.List<string> _AllPaths = new OOAdvantech.Collections.Generic.List<string>();

        /// <MetaDataID>{29a8c94f-e4bd-4a7b-ad6d-2670a85cc634}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                return _AllPaths;
            }
        }
        /// <MetaDataID>{e7c4d35f-8d3c-48e4-8604-c5be340cfe98}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;
            // if (propertyName == "this.Value")
            //  return true;

            if (propertyName == "Value")
                return true;
            return false;
        }
        /// <MetaDataID>{d72da4db-980a-47d9-b286-e96f37d21c97}</MetaDataID>
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

        /// <MetaDataID>{407f2ea3-7621-4ad8-971c-59b523917855}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{5d649cff-7e96-42ef-a4c5-7586e75edc58}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;

        }


        /// <MetaDataID>{6a8f6457-28a3-4f3c-8005-cb88542d2fcd}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }

        /// <MetaDataID>{b1cd67e2-ff0e-4157-aa5c-4b1fcfd396e6}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (OnClickOperationCaller != null)
            {
                foreach (string error in OnClickOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: Button '" + Name + ".OnClickOperation' " + error, FindForm().GetType().FullName));
                }
            }
            return hasErrors;
        }

        /// <MetaDataID>{3301a6e3-c7c6-4b82-8209-749389f6cc22}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _OnClickOperationCaller;
        /// <MetaDataID>{df432354-f49d-495a-a57d-6ba445db418d}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller OnClickOperationCaller
        {
            get
            {
                if (UserInterfaceObjectConnection == null)
                    return null;
                if (_OnClickOperationCall == null)
                    return null;
                if (_OnClickOperationCaller != null)
                    return _OnClickOperationCaller;
                _OnClickOperationCaller = new OperationCaller(_OnClickOperationCall, this);
                return _OnClickOperationCaller;

            }
        }

        /// <MetaDataID>{943b4538-7755-4108-88d8-cbfe8187b217}</MetaDataID>
        XDocument OnClickOperationCallMetaData;
        /// <MetaDataID>{beec5db9-c746-4804-a062-f98067d2c682}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _OnClickOperationCall;
        /// <MetaDataID>{81a484de-9701-46f2-85b0-94d1dc58c4c3}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object OnClickOperationCall
        {
            get
            {
                if (OnClickOperationCallMetaData == null)
                {
                    OnClickOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _OnClickOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (OnClickOperationCaller == null || OnClickOperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(default(string), "none");
                else
                {
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(OnClickOperationCallMetaData.ToString() as string, OnClickOperationCaller.Operation.Name);

                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
                    PropertyDescriptor property = properties.Find("TransactionOption", false);
                    if (TransactionOption != OOAdvantech.Transactions.TransactionOption.Suppress)
                        property.SetValue(this, OOAdvantech.Transactions.TransactionOption.Suppress);



                }

                //MetaDataValue metaDataVaue = new MetaDataValue(OnClickOperationCallMetaData.OuterXml as string);
                metaDataVaue.MetaDataAsObject = _OnClickOperationCall;
                return metaDataVaue;



            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("OnClickOperationCall", false).SetValue(this, OnClickOperationCall);
                    return;
                }
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    if (metaData == null && DesignMode)
                        TypeDescriptor.GetProperties(this).Find("OnClickOperationCall", false).SetValue(this, null);

                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                if (metaDataVaue == null)
                    return;


                if (OnClickOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    _OnClickOperationCall = null;
                    OnClickOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            OnClickOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            OnClickOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }

                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        OnClickOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        _OnClickOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (_OnClickOperationCall == null)
                        _OnClickOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        _OnClickOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                if (_OnClickOperationCaller != null)
                    _OnClickOperationCaller.ReleaseEventHandlers();

                _OnClickOperationCaller = null;
                return;
            }
        }


        /// <MetaDataID>{83020f96-ce5c-49b3-9d09-e374d1a8bd27}</MetaDataID>
        OOAdvantech.Transactions.TransactionOption _TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
        /// <MetaDataID>{fa8736ec-1a00-4066-895c-bb0d205bcd30}</MetaDataID>
        [Category("Behavior")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return _TransactionOption;
            }
            set
            {
                if (OnClickOperationCaller != null && OnClickOperationCaller.Operation != null && value != OOAdvantech.Transactions.TransactionOption.Suppress)
                {
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
                    PropertyDescriptor property = properties.Find("TransactionOption", false);
                    property.SetValue(this, OOAdvantech.Transactions.TransactionOption.Suppress);
                }
                _TransactionOption = value;
            }
        }
        /// <MetaDataID>{2F2308CB-7091-41EB-98C8-0533EA26690F}</MetaDataID>
        protected override void OnClick(EventArgs e)
        {
            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.Transaction != null)
            {
                if (UserInterfaceObjectConnection.Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                {
                    base.OnClick(e);
                    if (!FindForm().Modal && (DialogResult == DialogResult.Cancel || DialogResult == DialogResult.OK))
                    {
                        FindForm().DialogResult = DialogResult;
                        FindForm().Close();

                    }
                    
                }
                else
                {
                    using (OOAdvantech.Transactions.SystemStateTransition suppressstateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(UserInterfaceObjectConnection.Transaction))
                        {
                            using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition(_TransactionOption))
                            {
                                base.OnClick(e);

                                if (OOAdvantech.Transactions.Transaction.Current != null && OOAdvantech.Transactions.Transaction.Current.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                                    innerStateTransition.Consistent = true;
                            }

                            stateTransition.Consistent = true;
                        }
                    }
                }


            }
            else
            {
                if (_TransactionOption == OOAdvantech.Transactions.TransactionOption.Suppress)
                {
                    base.OnClick(e);
                }
                else
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(_TransactionOption))
                    {
                        base.OnClick(e);
                        stateTransition.Consistent = true;
                    }
                }
            }

            CallOnClickOperation();
            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.State != OOAdvantech.UserInterface.Runtime.ViewControlObjectState.Passive)
            {
                if (!FindForm().Modal && (DialogResult == DialogResult.Cancel || DialogResult == DialogResult.OK))
                {
                    FindForm().DialogResult = DialogResult;
                    FindForm().Close();

                }
            }
            if (SaveButton)
            {
                UserInterfaceObjectConnection.Save();

            }

            //System.Windows.Forms.Control control = Parent;
            //while (control != null && !(control is System.Windows.Forms.Form))
            //    control = control.Parent;
            //(control as System.Windows.Forms.Form).DialogResult = System.Windows.Forms.DialogResult.OK;
            //(control as System.Windows.Forms.Form).Close();

        }

        /// <MetaDataID>{cd11bf93-6fe6-4f53-8f2a-104d9f2cdeec}</MetaDataID>
        private void CallOnClickOperation()
        {
            if (UserInterfaceObjectConnection == null)
                return;
            if (OnClickOperationCaller != null)
            {
                object obj = OnClickOperationCaller.Invoke();// OnClickOperationCaller.ExecuteOperationCall();
                if (obj is OOAdvantech.UserInterface.Runtime.DialogResult)// && FindForm().Modal)
                {
                    OOAdvantech.UserInterface.Runtime.DialogResult dialogResult = (OOAdvantech.UserInterface.Runtime.DialogResult)obj;
                    //Set the DialogResult for non modal forms.See line 368
                    this.DialogResult = (System.Windows.Forms.DialogResult)(int)dialogResult;
                    if (FindForm().Modal)
                    {
                        if (dialogResult != OOAdvantech.UserInterface.Runtime.DialogResult.None)
                        {
                            FindForm().DialogResult = this.DialogResult;
                            FindForm().Close();
                            return;
                        }
                    }
                }
            }




            //if (OnClickOperation != null && _OnClickOperationCall != null)
            //{
            //    if (ViewControlObject == null)
            //        return;
            //    ViewControlObject viewControlObject = ViewControlObject;

            //    int i = 0;
            //    System.Reflection.MethodInfo methodInfo = OnClickOperation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
            //    object[] parameterValues = new object[methodInfo.GetParameters().Length];

            //    foreach (System.Reflection.ParameterInfo parameterInfo in methodInfo.GetParameters())
            //    {
            //        string source = null;
            //        foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in _OnClickOperationCall.ParameterLoaders)
            //        {
            //            if (parameterLoader.Name == parameterInfo.Name)
            //            {
            //                source = parameterLoader.Source;
            //                break;
            //            }
            //        }
            //        if (source == "$ViewControlObject$")
            //        {
            //            parameterValues[i] = viewControlObject.Instance;
            //        }
            //        else if (source == "this")
            //        {
            //            parameterValues[i] = this;
            //        }
            //        else if (source == "this.Value")
            //        {
            //            parameterValues[i] = null;
            //        }
            //        else
            //        {
            //            IObjectMemberViewControl objectMemberViewControl = ViewControlObject.GetControlWithName(source) as IObjectMemberViewControl;
            //            parameterValues[i] = objectMemberViewControl.Value;
            //        }
            //        i++;
            //    }
            //    object returnValue=null;
            //    if (_OnClickOperationCall.CallType == OOAdvantech.UserInterface.CallType.StaticOperationCall)
            //        returnValue=methodInfo.Invoke(null, parameterValues);
            //    if (_OnClickOperationCall.CallType == OOAdvantech.UserInterface.CallType.HostingFormOperationCall)
            //        returnValue=methodInfo.Invoke(viewControlObject.ContainerControl, parameterValues);
            //    if (_OnClickOperationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)
            //        returnValue=methodInfo.Invoke(viewControlObject.Instance, parameterValues);

            //    if (returnValue != null)
            //    {
            //        IObjectMemberViewControl control = viewControlObject.GetControlWithName(_OnClickOperationCall.ReturnValueDestination) as IObjectMemberViewControl;
            //        if(control!=null)
            //            control.Value = returnValue;
            //    }

            //}
        }
        /// <MetaDataID>{0b926851-b081-453c-9acf-85449e801d88}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        /// <MetaDataID>{22abc12f-c7b0-4b3a-9af2-87c1ab08db28}</MetaDataID>
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
        /// <MetaDataID>{1f020668-2092-427c-8056-212215a7fbaf}</MetaDataID>
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
        ///// <MetaDataID>{ef333a5e-5dd6-4a24-89c8-ed50306fd049}</MetaDataID>
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public object UIMetaDataObject
        //{
        //    get
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }
        //    set
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }
        //}

        /// <MetaDataID>{f8ec0b8a-2526-49df-92e1-25a5939516fb}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {

            if (metaObject is OOAdvantech.UserInterface.OperationCall && new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.Attribute &&
                propertyDescriptor == "Text" &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) == typeof(string))
                return true;
            else

                return false;
        }


        #region IObjectMemberViewControl Members

        /// <MetaDataID>{384737df-b636-4255-951e-6a15c54aae1b}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            return ValueType;
        }


        /// <MetaDataID>{75d7c06b-70ba-4ce7-afbd-5b78c043ff18}</MetaDataID>
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

        /// <MetaDataID>{22aa3ac8-ca54-4627-9b93-63d8f4c38c17}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {
                if (OnClickOperationCaller != null && OnClickOperationCaller.Operation != null)
                    return OnClickOperationCaller.Operation.ReturnType;
                else
                    return null;
            }
        }

        /// <MetaDataID>{8f4e0a4b-d0c4-4568-aacf-51fda9d4efd3}</MetaDataID>
        public object Path
        {
            get
            {
                return "";
            }
            set
            {

            }
        }

        /// <MetaDataID>{719ed80b-9085-4157-946d-5348bbfec629}</MetaDataID>
        public void LoadControlValues()
        {
            

        }

        /// <MetaDataID>{6274b8e8-8e9e-4506-8882-06dffa88728c}</MetaDataID>
        public void SaveControlValues()
        {

        }

        #endregion




        /// <MetaDataID>{12b48ac4-44fe-4130-9512-15172e60bfe6}</MetaDataID>
        List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{15b22284-631c-4651-83a2-f61757e3a9f4}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get 
            {
                return _DependencyProperties;
            }
        }

       
    }
}
