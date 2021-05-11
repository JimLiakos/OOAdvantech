using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using OOAdvantech.UserInterface.Runtime;
using ConnectableControls;
using OOAdvantech.Transactions;
using ConnectableControls.PropertyEditors;

namespace DXConnectableControls.XtraEditors
{
    /// <MetaDataID>{776f3fc6-c3dd-4a2d-8bba-bf1db93a4d9f}</MetaDataID>
    public class Button : DevExpress.XtraEditors.SimpleButton, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl
    {
        /// <MetaDataID>{225d7c1c-7fc5-4cb7-8bbd-a3ab98f96d3c}</MetaDataID>
        public Button():base()
        {
            _EnableProperty = new DependencyProperty(this, "Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }

        /// <MetaDataID>{774d258b-064d-4e1c-b3c3-6aa3a77698bc}</MetaDataID>
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

        /// <MetaDataID>{be5e6113-190e-4d29-9b12-e4bae28bac6a}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        #region public OOAdvantech.UserInterface.Runtime.OperationCaller OnClickOperationCaller
        /// <MetaDataID>{98a4c08a-df40-4b78-a4fb-5017a9f1d0df}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _OnClickOperationCaller;
        /// <MetaDataID>{0d42de65-bc7d-4a16-966c-c1c23a7b27ab}</MetaDataID>
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
        #endregion

        #region protected override void OnClick(EventArgs e)
        /// <MetaDataID>{eb4dcb70-f51e-4b71-9c25-3ce17b755074}</MetaDataID>
        protected override void OnClick(EventArgs e)
        {            
            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.Transaction != null)
            {
                if (UserInterfaceObjectConnection.Transaction.Status != OOAdvantech.Transactions.TransactionStatus.Continue)
                {
                    base.OnClick(e);
                    if (!FindForm().Modal && (DialogResult == System.Windows.Forms.DialogResult.Cancel || DialogResult == System.Windows.Forms.DialogResult.OK))
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

            object retvalue = CallOnClickOperation();
            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.State != OOAdvantech.UserInterface.Runtime.ViewControlObjectState.Passive)
            {
                if (retvalue is System.Windows.Forms.DialogResult)
                    this.DialogResult = (System.Windows.Forms.DialogResult)retvalue;
                if (retvalue is OOAdvantech.UserInterface.Runtime.DialogResult)
                    this.DialogResult = (System.Windows.Forms.DialogResult)(int)retvalue;

                if (!FindForm().Modal && (DialogResult == System.Windows.Forms.DialogResult.Cancel || DialogResult == System.Windows.Forms.DialogResult.OK))
                {
                    FindForm().DialogResult = DialogResult;
                    FindForm().Close();

                }                
            }
            if (SaveButton)
            {
                if (UserInterfaceObjectConnection.MasterViewControlObject != null)
                    (UserInterfaceObjectConnection.MasterViewControlObject as OOAdvantech.UserInterface.Runtime.FormObjectConnection).Save();
                else
                    (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).Save();

            }

            //System.Windows.Forms.Control control = Parent;
            //while (control != null && !(control is System.Windows.Forms.Form))
            //    control = control.Parent;
            //(control as System.Windows.Forms.Form).DialogResult = System.Windows.Forms.DialogResult.OK;
            //(control as System.Windows.Forms.Form).Close();

        } 
        #endregion

        #region private void CallOnClickOperation()
        //CallOnClickOperation now returns object in my button
        /// <MetaDataID>{b7adcd34-5e10-4847-8311-2e53f59aaac0}</MetaDataID>
        private object CallOnClickOperation()
        {
            if (UserInterfaceObjectConnection == null)
                return null;
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
                            return null;
                        }
                    }
                }
                return obj;
            }
            return null;
        } 
        #endregion

        /// <MetaDataID>{38e801dc-a2c0-474c-996b-341ec2516be8}</MetaDataID>
        public void InitializeControl()
        {

        }
        /// <MetaDataID>{3dee892f-74ea-4d56-85ab-086fcb5fcd7a}</MetaDataID>
        System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{2aa54361-a074-4970-bccf-c0f550d5307f}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }


        //public System.Windows.Forms.DialogResult DialogResult
        //{
        //    get
        //    {
        //        return base.DialogResult;
        //    }
        //    set
        //    {
        //        if (!SaveButton)
        //            base.DialogResult = value;
        //        else
        //            base.DialogResult = System.Windows.Forms.DialogResult.None;

        //    }
        //}

        #region public object OnClickOperationCall
        /// <MetaDataID>{e3ac6472-f455-49b1-91d3-4013ca570cfb}</MetaDataID>
        System.Xml.XmlDocument OnClickOperationCallMetaData;
        /// <MetaDataID>{edaad24f-7290-4961-ab6e-fa4a26a2a623}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _OnClickOperationCall;
        /// <MetaDataID>{a5d57946-286d-4fe7-ac39-9312fb18653f}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object OnClickOperationCall
        {
            get
            {
                if (OnClickOperationCallMetaData == null)
                {
                    OnClickOperationCallMetaData = new System.Xml.XmlDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _OnClickOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (OnClickOperationCaller == null || OnClickOperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(default(string), "none");
                else
                {
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(OnClickOperationCallMetaData.OuterXml as string, OnClickOperationCaller.Operation.Name);

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
                OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService.GetType();
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
                    OnClickOperationCallMetaData = new System.Xml.XmlDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            OnClickOperationCallMetaData.LoadXml(metaData);
                    }
                    catch (System.Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            OnClickOperationCallMetaData = new System.Xml.XmlDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", OnClickOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }

                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        OnClickOperationCallMetaData = new System.Xml.XmlDocument();
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
        #endregion

        #region public OOAdvantech.Collections.Generic.List<string> Paths
        /// <MetaDataID>{e3188b61-04e3-49e2-af6a-91e4fa80f1f1}</MetaDataID>
        static OOAdvantech.Collections.Generic.List<string> _AllPaths = new OOAdvantech.Collections.Generic.List<string>();

        /// <MetaDataID>{04c3af5e-ad84-4142-bdf6-9d00cb8eaace}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                return _AllPaths;
            }
        } 
        #endregion

        #region public bool SaveButton
        /// <MetaDataID>{4d41a993-0938-4107-bce4-0351499ee3cc}</MetaDataID>
        bool _SaveButton;
        /// <MetaDataID>{42752ef6-29b7-4da9-a686-18871545ae6e}</MetaDataID>
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
                    DialogResult = System.Windows.Forms.DialogResult.None;
            }
        } 
        #endregion

        #region public ViewControlObject ViewControlObject
        /// <MetaDataID>{1d664b40-6f59-4d36-bf7a-7e8a6b8e5931}</MetaDataID>
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
        #endregion

        #region public OOAdvantech.Transactions.TransactionOption TransactionOption
        /// <MetaDataID>{fe7ee34b-2586-49e1-a3c6-532fc3c3ceaa}</MetaDataID>
        OOAdvantech.Transactions.TransactionOption _TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
        /// <MetaDataID>{c3d367ad-2263-4428-81b4-c43a8de2808a}</MetaDataID>
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
        #endregion

        #region IObjectMemberViewControl Members

        #region public bool AllowDrag
        /// <MetaDataID>{9c0adde0-163c-4e0e-b8de-61002756ac07}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{e6659913-8c4f-4ad1-8b22-399d645231d8}</MetaDataID>
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
        #endregion

        #region public bool ConnectedObjectAutoUpdate
        /// <MetaDataID>{8adcddca-660f-4a0f-96f0-52ebf1ed301c}</MetaDataID>
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
        #endregion

        #region public bool ErrorCheck(ref System.Collections.ArrayList errors)
        /// <MetaDataID>{1b97a254-76f7-4ee8-b11a-d995d2381231}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
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
        #endregion

        #region public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        /// <MetaDataID>{34bfdd1c-e23c-4fdd-86bc-58520a1a92fa}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            return ValueType;
        } 
        #endregion

        #region LoadControlData,SaveControlData,Path,Value
        /// <MetaDataID>{c411f126-7258-45b7-9a1e-c70f1f1c17c1}</MetaDataID>
        public void LoadControlData()
        {

        }

        /// <MetaDataID>{eefa965b-40f7-4595-8e8d-f06c597667e4}</MetaDataID>
        public void SaveControlData()
        {

        }



        /// <MetaDataID>{8a576397-84c5-4214-9219-6b7616307f98}</MetaDataID>
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

        /// <MetaDataID>{58dfefe1-fcb8-4dde-8eac-1eb1e505f872}</MetaDataID>
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
        #endregion

        #region public OOAdvantech.MetaDataRepository.Classifier ValueType
        /// <MetaDataID>{362cf39b-44fe-47e5-b370-c634c4014264}</MetaDataID>
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
        #endregion

        #endregion

        #region IOperetionCallerSource Members

        #region public bool ContainsProperty(string propertyName)
        /// <MetaDataID>{049515ce-31e5-41f8-bff9-11e7b1bd5b8b}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "this")
                return true;

            if (propertyName == "Value")
                return true;
            return false;
        } 
        #endregion

        #region public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        /// <MetaDataID>{18775a05-d493-4bf2-8d86-86b59e413843}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return ValueType;
            return null;
        } 
        #endregion

        #region public object GetPropertyValue(string propertyName)
        /// <MetaDataID>{03f38a46-ccb1-478a-9019-8db76249bb36}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "this")
                return this;

            if (propertyName == "Value")
                return Value;


            throw new System.Exception("There isn't property with name " + propertyName + ".");
        } 
        #endregion

        #region public string[] PropertiesNames
        /// <MetaDataID>{92d10f80-4681-4746-ba27-9947360fa035}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[0];
            }
        } 
        #endregion

        /// <MetaDataID>{1ed8ecfc-7965-4625-8a4f-463ccd40ae37}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConnectableControl Members

        #region IsPropertyReadOnly,UIMetaDataObject
        /// <MetaDataID>{3888fe9d-a6de-40b7-b98b-184b8b07b554}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        /// <MetaDataID>{cf78e4ff-fffa-4e06-abd6-c925f911a5bf}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object UIMetaDataObject
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        } 
        #endregion

        /// <MetaDataID>{c1e07faf-02c3-45e3-b198-e25c01391cd2}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        /// <MetaDataID>{95e52dfe-1ac8-4284-ae45-1e5654027ba9}</MetaDataID>
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

        #endregion

        #region IMetadataSelectionResolver Members

        /// <MetaDataID>{e965dfef-c9c2-4d37-aa8e-55d05f042e86}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (metaObject is OOAdvantech.UserInterface.OperationCall && new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                return true;
            else
            {
                if (metaObject is OOAdvantech.MetaDataRepository.Attribute)
                {
                    Type type = (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type;
                    
                    //this is for the EnableProperty 
                    //Comes in here from the DClick of the MetaDataNavigator line 1190
                    if (type == typeof(bool))
                        return true;

                    return false;
                }
                else
                    return false;
            }
        }

        #endregion        
    }
}
