using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using ConnectableControls.PropertyEditors;

namespace ConnectableControls
{

    public enum ViewControlObjectState
    {
        DesigneMode,
        Initialize,
        UserInteraction,
        UpdateControlValues,
        SaveControlValues,
        Passive
    }
    /// <MetaDataID>{D5B608AE-44DF-4893-B11F-A11B54102825}</MetaDataID>
    public class ViewControlObject : System.ComponentModel.Component
    {
        /// <exclude>Excluded</exclude>
        public ViewControlObjectState _State = ViewControlObjectState.Initialize;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ViewControlObjectState State
        {
            get
            {
                if (DesignMode)
                    State = ViewControlObjectState.DesigneMode;

                if (_MasterViewControlObject != null)
                    return _MasterViewControlObject.State;
                else
                    return _State;
            }
            protected set
            {


                if (_MasterViewControlObject != null)
                    _MasterViewControlObject.State = value;
                else
                    _State = value;


            }
        }
        /// <MetaDataID>{407E09F7-FBFF-4BD1-9BF0-09D0F58863CF}</MetaDataID>
        static ViewControlObject()
        {
           // OOAdvantech.UserInterface.PresentationObjectsController.ObjectUIWrapper = new OOAdvantech.UserInterface.ObjectUIWrapper(GenerateProxy);
        }
        ///// <MetaDataID>{8EE1AC49-4DF7-4D2F-BBD0-3AD645812CE0}</MetaDataID>
        //internal static object GenerateProxy(object _object)
        //{
        //    if (_object is MarshalByRefObject)
        //        if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_object as MarshalByRefObject))
        //            if (!(System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) is UIProxy))
        //                return new UIProxy(_object as MarshalByRefObject, _object.GetType()).GetTransparentProxy();

        //    if (_object is MarshalByRefObject && !(System.Runtime.Remoting.RemotingServices.GetRealProxy(_object) is UIProxy))
        //        return new UIProxy(_object as MarshalByRefObject, _object.GetType()).GetTransparentProxy();

        //    return _object;

        //}
        /// <MetaDataID>{E5BB2506-F9EB-4726-ABFC-0DD3DA12966B}</MetaDataID>
        public override string ToString()
        {
            return Name;
        }
        /// <exclude>Excluded</exclude>
        private string _Name;

        /// <MetaDataID>{3D72589A-9E4A-4EEC-BC0B-8C9174321AD3}</MetaDataID>
        public string Name
        {
            get
            {
                if (DesignMode && Site != null)
                    return Site.Name;
                return _Name;
            }
            set
            {
                if (!DesignMode)
                    _Name = value;
            }
        }




        /// <MetaDataID>{0F2A226C-D7AD-4A24-8410-E6F4D28E41F3}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            foreach (System.ComponentModel.IComponent component in _ControlledComponents)
            {
                IObjectMemberViewControl memberViewControl = component as IObjectMemberViewControl;
                if (memberViewControl != null)
                    hasErrors |= memberViewControl.ErrorCheck(ref errors);
            }

            if (_ObjectType == null)
                if (!string.IsNullOrEmpty(_ViewObjectTypeFullName))
                    _ObjectType = AssemblyManager.GetClassifier(_ViewObjectTypeFullName, true);
            if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
            {

                _PresentationObjectType = AssemblyManager.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ViewControlObject '" + Name + "' has invalid PresentationObject.", ContainerControl.GetType().FullName));
                else
                {
                    foreach (OOAdvantech.MetaDataRepository.Classifier classifier in (_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).GetAllGeneralClasifiers())
                    {
                        if (classifier is OOAdvantech.MetaDataRepository.Interface)
                            continue;
                        OOAdvantech.MetaDataRepository.Class _class = classifier as OOAdvantech.MetaDataRepository.Class;

                        if (_class.TemplateBinding != null && (_class.TemplateBinding.Signature.Template as OOAdvantech.MetaDataRepository.Classifier).FullName == typeof(OOAdvantech.UserInterface.PresentationObject<>).FullName)
                        {
                            OOAdvantech.MetaDataRepository.Classifier realObjectClass = (_class.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier);
                            foreach (OOAdvantech.MetaDataRepository.Operation operation in (_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).GetOperations((_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).Name, true))
                            {
                                if (operation.Parameters.Count == 1)
                                {
                                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                                    {
                                        if (parameter.Type == realObjectClass && parameter.Type == _ObjectType)
                                            return hasErrors;
                                    }
                                }
                            }
                        } 
                    }
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: You can't assign the '" + _PresentationObjectType.FullName + "' to the PresentationObjectType.\n Check the property rules.", ContainerControl.GetType().FullName));
                }
                 
            }
            return hasErrors;
        }
        /// <exclude>Excluded</exclude>
        protected ViewControlObject _MasterViewControlObject;
        /// <MetaDataID>{5ACBB159-F345-44C6-AD47-4399231C3504}</MetaDataID>
        virtual public ViewControlObject MasterViewControlObject
        {
            set
            {
                if (_MasterViewControlObject != null)
                    _MasterViewControlObject.RemoveControlledComponent(this);

                if (value != null && value.MasterViewControlObject == null && !(value is FormConnectionControl))
                    throw new System.Exception("System can't find the FormConnectionControl");
                if (value != null && value.MasterViewControlObject == null && (value is FormConnectionControl))
                    _MasterViewControlObject = value;
                if (value != null && value.MasterViewControlObject != null)
                    _MasterViewControlObject = value.MasterViewControlObject;
                if (_MasterViewControlObject != null)
                    _MasterViewControlObject.AddControlledComponent(this);
            }
            get
            {
                return _MasterViewControlObject;
            }
        }
        /// <MetaDataID>{85164593-9C73-4DBB-8066-3ACBFAE972D3}</MetaDataID>
        public static ITraslator Translator;



        /// <exclude>Excluded</exclude>
        string _ViewObjectTypeFullName;
        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Classifier _ObjectType;
        /// <MetaDataID>{578053BD-0C9C-4232-8306-D925F9F86CFD}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ObjectType
        {
            get
            {
                if (_ObjectType != null)
                    return _ObjectType;
                if (!string.IsNullOrEmpty(_ViewObjectTypeFullName))
                {
                    _ObjectType = AssemblyManager.GetClassifier(_ViewObjectTypeFullName, true);
                }
                return _ObjectType;
            }
        }

        /// <MetaDataID>{1BFCA073-9EA1-4DF7-A4CE-177CB4487050}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object ViewControlObjectType
        {
            get
            {
                if (_ObjectType != null)
                    return _ObjectType.FullName;

                return _ViewObjectTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Classifier)
                    _ObjectType = value as OOAdvantech.MetaDataRepository.Classifier;
                if (_ObjectType != null)
                    _ViewObjectTypeFullName = _ObjectType.FullName;
                if (value is string)
                    _ViewObjectTypeFullName = value as string;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier PresentationObjectType
        {
            get
            {
                if (ObjectType == null)
                    return null;
                if (_PresentationObjectType != null)
                    return _PresentationObjectType;
                else
                {
                    if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
                    {
                        _PresentationObjectType = AssemblyManager.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                        return _PresentationObjectType;
                    }
                    return ObjectType;
                }

            }
        }
        /// <MetaDataID>{21200365-9306-450B-85C9-2FF0257EF22E}</MetaDataID>
        string PresentationObjectTypeFullName;
        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Class _PresentationObjectType;
        /// <MetaDataID>{0CA482CD-1ADD-4336-B46B-0963FB299D5B}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object AssignPresentationObjectType
        {
            get
            {
                if (_PresentationObjectType != null)
                    return _PresentationObjectType.FullName;

                return PresentationObjectTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Class)
                    _PresentationObjectType = value as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType != null)
                    PresentationObjectTypeFullName = _PresentationObjectType.FullName;
                if (value is string)
                    PresentationObjectTypeFullName = value as string;
                if (value == null)
                {
                    PresentationObjectTypeFullName = "";
                    _PresentationObjectType = null;
                }

            }
        }

        /// <summary>Required designer variable.</summary>
        /// <MetaDataID>{400B1B99-A5A4-412A-AB31-91E3348AD8D4}</MetaDataID>
        private System.ComponentModel.Container components = null;

        /// <exclude>Excluded</exclude>
        protected System.Windows.Forms.Control _ContainerControl;
        /// <MetaDataID>{C31F6FC3-2D91-444A-AE35-5AEF2716AE38}</MetaDataID>
        [Browsable(false)]
        public virtual System.Windows.Forms.Control ContainerControl
        {
            get
            {
                return _ContainerControl;
            }
            set
            {
                _ContainerControl = value;
            }

        }
        /// <exclude>Excluded</exclude>
        object _PresentationObject;
        public object PresentationObject
        {
            get
            {
                if (_PresentationObject == null)
                    return _Instance;
                else
                    return _PresentationObject;
            }
        }

        /// <MetaDataID>{DBE34209-760F-4839-AA92-D9EA6B0E98F9}</MetaDataID>
        internal void SetInsance(object value)
        {

            object oldInstanceValue = _Instance;

            #region Create presantation object if declared
            if (PresentationObjectType != null && value != null)
            {
                if (_ObjectType == null)
                    if (!string.IsNullOrEmpty(_ViewObjectTypeFullName))
                        _ObjectType = AssemblyManager.GetClassifier(_ViewObjectTypeFullName, true);

                if (_PresentationObjectType == null && !string.IsNullOrEmpty(PresentationObjectTypeFullName))
                {
                    _PresentationObjectType = AssemblyManager.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;

                    System.Reflection.ConstructorInfo contructor = ((_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(System.Type)) as System.Type).GetConstructor(new System.Type[] { _ObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type });
                    _PresentationObject = contructor.Invoke(new object[1] { value });

                }
                else if (_PresentationObjectType != null)
                {
                    System.Reflection.ConstructorInfo contructor = ((_PresentationObjectType as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(System.Type)) as System.Type).GetConstructor(new System.Type[] { _ObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type });
                    _PresentationObject = contructor.Invoke(new object[1] { value });


                }


            }
            else
                _PresentationObject = null;
            #endregion
            _Instance = value;

            if (State == ViewControlObjectState.Initialize || State == ViewControlObjectState.DesigneMode)
                return;


            #region Precondition check
            if (ContainerControl == null)
                throw new System.Exception(string.Format("There isn't ContainerControl ({0})", Name));

            if (this.GetType() != typeof(FormConnectionControl) && MasterViewControlObject == null)
                throw new System.Exception(string.Format("There isn't 'FormConnectionControl' as root of ViewControlObject tree"));
            #endregion


            if (_Instance != oldInstanceValue)
            {
                if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null)
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                    {
                        try
                        {

                            LoadControlsData();
                            if (InstanceChanged != null)
                                InstanceChanged(this, EventArgs.Empty);

                        }
                        finally
                        {
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                {
                    LoadControlsData();
                    if (InstanceChanged != null)
                        InstanceChanged(this, EventArgs.Empty);

                }
            }
        }


        /// <exclude>Excluded</exclude>
        protected object _Instance;
        /// <MetaDataID>{B54FA8CB-7A44-483B-B07D-FCC188AA1FF7}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Instance
        {
            get
            {
                return _Instance;

            }
            set
            {
                SetInsance(value);
            }
        }


        public event EventHandler BeforeLoadControlsData;
        /// <MetaDataID>{350BE67B-485E-49BA-BFFC-830891F12131}</MetaDataID>
        internal protected void LoadControlsData()
        {
            if (this.ObjectType == null)
                return;
            try
            {
                StartControlValuesUpdate();



                if (BeforeLoadControlsData != null)
                    BeforeLoadControlsData(this, EventArgs.Empty);
                if (_Instance != null)
                {
                    if (_Instance is MarshalByRefObject)//&& OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_Instance))
                    {
                        OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(_Instance as MarshalByRefObject, RootObjectNode.Paths);



                    }
                    if (!DisplayedValue.DisplayedValues.ContainsKey(PresentationObject))
                        DisplayedValue.DisplayedValues[PresentationObject] = new DisplayedValue(PresentationObject, this);

                    foreach (System.ComponentModel.IComponent component in _ControlledComponents)
                    {
                        if (component is IObjectMemberViewControl)
                            (component as IObjectMemberViewControl).LoadControlData();
                        if (component is ViewControlObject)
                            (component as ViewControlObject).LoadControlsData();
                    }
                }
                else
                {
                    ReleaseDataPathNodes();
                    //DisplayedValue.DisplayedValue.DisplayedValues.Clear();
                    foreach (System.ComponentModel.IComponent component in _ControlledComponents)
                    {
                        if (component is IObjectMemberViewControl)
                            (component as IObjectMemberViewControl).LoadControlData();
                        if (component is ViewControlObject)
                            (component as ViewControlObject).LoadControlsData();
                    }
                }
            }
            finally
            {
                EndControlValuesUpdate();
            }
        }
        /// <MetaDataID>{4E14E63C-0467-49B1-9DB5-37FE070FF805}</MetaDataID>
        internal protected void SaveControlData()
        {
            foreach (System.ComponentModel.IComponent component in _ControlledComponents)
            {
                IObjectMemberViewControl ObjectMemberViewControl = component as IObjectMemberViewControl;
                if (ObjectMemberViewControl != null && !ObjectMemberViewControl.ConnectedObjectAutoUpdate)
                    ObjectMemberViewControl.SaveControlData();
                if (component is ViewControlObject)
                    (component as ViewControlObject).SaveControlData();

            }
        }



        /// <MetaDataID>{B28F69C0-01D2-4518-AE99-6593A5941775}</MetaDataID>
        public ViewControlObject(IContainer container)
        {

            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        /// <MetaDataID>{1D6CA470-D142-442D-90E5-857E78500A75}</MetaDataID>
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                if (value == null && DesignMode && base.Site != null)
                    (base.Site.Container as System.ComponentModel.Design.IComponentChangeService).ComponentRemoving -= new System.ComponentModel.Design.ComponentEventHandler(ComponentRemoving);
                base.Site = value;
                if (DesignMode && base.Site != null)
                {
                    (base.Site.Container as System.ComponentModel.Design.IComponentChangeService).ComponentRemoving += new System.ComponentModel.Design.ComponentEventHandler(ComponentRemoving);
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
                    PropertyDescriptor property = properties.Find("ContainerControl", false);
                    property.SetValue(this, (base.Site.Container as System.ComponentModel.Design.IDesignerHost).RootComponent as System.Windows.Forms.Control);

                }

            }
        }

        /// <MetaDataID>{9D772F1A-CC08-4C0B-954E-D610A2C567F2}</MetaDataID>
        void ComponentRemoving(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {
            if (_ControlledComponents.Contains(e.Component))
                _ControlledComponents.Remove(e.Component);

        }


        /// <MetaDataID>{6AF54161-D8C0-4616-91ED-0C0AE7CC89BD}</MetaDataID>
        public ViewControlObject()
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>Clean up any resources being used.</summary>
        /// <MetaDataID>{EA1FE528-F214-47AF-AD9F-1501B5048806}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.List<System.ComponentModel.IComponent> _ControlledComponents = new OOAdvantech.Collections.Generic.List<IComponent>();

        /// <MetaDataID>{0DC7CEDE-426A-4366-B2D7-6199A8CE7538}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.Collections.Generic.List<System.ComponentModel.IComponent> ControlledComponents
        {
            get
            {
                return new OOAdvantech.Collections.Generic.List<IComponent>(_ControlledComponents);
            }

        }

        /// <MetaDataID>{B18064C9-7E8C-452C-8F15-0EFD0FA3A570}</MetaDataID>
        public void AddControlledComponent(System.ComponentModel.IComponent component)
        {
            if (!_ControlledComponents.Contains(component))
                _ControlledComponents.Add(component);
        }
        /// <MetaDataID>{BF994231-86D9-4768-A18E-8FA031B45056}</MetaDataID>
        public void RemoveControlledComponent(System.ComponentModel.IComponent component)
        {
            if (_ControlledComponents.Contains(component))
                _ControlledComponents.Remove(component);
        }
        /// <summary>
        /// This operation collect the view control object of form or control 
        /// </summary>
        /// <returns>
        /// A collection with view control objects
        /// </returns>
        /// <MetaDataID>{C0A0A1A6-48F9-44D7-9A69-CFA3FC4F5223}</MetaDataID>
        public System.Collections.Generic.List<ViewControlObject> GetHostContolViewControlObjects()
        {

            if (MasterViewControlObject != null)
                return MasterViewControlObject.GetHostContolViewControlObjects();
            else
            {
                System.Collections.Generic.List<ViewControlObject> viewControlObjects = new List<ViewControlObject>();
                viewControlObjects.Add(this);

                foreach (System.ComponentModel.IComponent component in _ControlledComponents)
                {
                    if (component is ViewControlObject && (component as ViewControlObject).ContainerControl == ContainerControl)
                        viewControlObjects.Add(component as ViewControlObject);
                }
                return viewControlObjects;
            }

        }

        /// <summary>
        /// This operation search for the control with name the controlName value.
        /// If there isn't control return null.
        /// </summary>
        /// <param name="controlName">
        /// Define the name of control which system want
        /// </param>
        /// <returns>
        /// The control which has the same name with the parameter
        /// </returns>
        /// <MetaDataID>{80BDCF6A-87BC-4490-A80B-A02BCD60E66D}</MetaDataID>
        public System.Windows.Forms.Control GetControlWithName(string controlName)
        {
            if (MasterViewControlObject != null)
                return MasterViewControlObject.InternalGetControlWithName(controlName);
            else
                return InternalGetControlWithName(controlName);

        }
        /// <MetaDataID>{3472646E-D5B6-4490-B958-50230389393E}</MetaDataID>
        System.Windows.Forms.Control InternalGetControlWithName(string controlName)
        {
            foreach (System.ComponentModel.IComponent component in _ControlledComponents)
            {
                if (component is Control && (component as Control).Name == controlName)
                    return component as Control;
                if (component is ViewControlObject && (component as ViewControlObject).ContainerControl == ContainerControl)
                {
                    Control control = (component as ViewControlObject).InternalGetControlWithName(controlName);
                    if (control != null)
                        return control;
                }
            }
            return null;
        }


        #region Component Designer generated code
        /// <summary>Required method for Designer support - do not modify
        /// the contents of this method with the code editor.</summary>
        /// <MetaDataID>{347B0166-03CA-47D4-AED1-ACBE5E04B5B1}</MetaDataID>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        ///// <summary>
        ///// This method returns the displayed value, 
        ///// for the value of path. The root of path is the instance of view control object.
        ///// The value of displayed value object is a presentation object which 
        ///// wrap the value of the original displayd value object. 
        ///// The presentation object is useful when you want to 
        ///// produce more information from those where the original object gives (calculated field etc).      
        ///// </summary>
        ///// <param name="path">The path which follow the system to get the value.</param>
        ///// <param name="pathDataDisplayer">The object which display the value. </param>
        ///// <param name="presentationObjectType">Defines a wraper class for the value of displayed value.</param>
        ///// <returns>Displayd value</returns>
        ///// <MetaDataID>{B7D9CD6A-C3E7-47F6-9AC2-93B12426CD80}</MetaDataID>
        //public DisplayedValue GetDisplayedValue(string path, IPathDataDisplayer pathDataDisplayer)
        //{
        //    return GetDisplayedValue(Instance, ObjectType, path, pathDataDisplayer);
        //}
        ///// <summary>
        ///// This method has the same functionality with original 
        ///// but with the addition of parameter presentationObjectType 
        ///// wrap the value of the original display value with a presentation object. 
        ///// The presentation object is useful when you want to produce more information from those 
        ///// where the original object gives (calculated field etc).      
        ///// </summary>
        ///// <param name="obj">object as root of path</param>
        ///// <param name="classifier">Type where system search for member. Member is the first part of path</param>
        ///// <param name="path">The path which follow the system to get the value.</param>
        ///// <param name="pathDataDisplayer">The object which display the value. </param>
        ///// <param name="presentationObjectType">Defines a wraper class for the value of displayed value.</param>
        ///// <returns>Displayd value</returns>
        ///// <MetaDataID>{BA4F448E-7D0B-48AC-A247-2391B6A3BE8F}</MetaDataID>
        //public DisplayedValue GetDisplayedValue(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, IPathDataDisplayer pathDataDisplayer )
        //{
        //    DisplayedValue displayedValue = GetDisplayedValue(obj, classifier, path, pathDataDisplayer);
        //}
        public DisplayedValue GetPresentationObject(object obj, OOAdvantech.MetaDataRepository.Class presentationObjectType,Type type)
        {
            if (presentationObjectType == null || obj == null)
                return new DisplayedValue(null, this);

            System.Reflection.ConstructorInfo contructor = null;
            contructor = ((presentationObjectType as OOAdvantech.MetaDataRepository.Class).GetExtensionMetaObject(typeof(System.Type)) as System.Type).GetConstructor(new System.Type[] { type });
            object value = contructor.Invoke(new object[1] { obj });
            DisplayedValue displayedValue = new DisplayedValue(value, this);
            return displayedValue;
        }

        /// <summary>
        /// This method returns the displayed value, 
        /// for the value of path. The root of path is the instance of view control object.
        /// </summary>
        /// <param name="path">The path which follow the system to get the value.</param>
        /// <param name="pathDataDisplayer">The object which display the value. </param>
        /// <param name="presentationObjectType">Defines a wraper class for the value of displayed value.</param>
        /// <returns>Displayd value</returns>
        /// <MetaDataID>{0AAE9B7C-8211-41CB-98BA-F7D7AB80C291}</MetaDataID>
        public DisplayedValue GetDisplayedValue(string path, IPathDataDisplayer pathDataDisplayer)
        {

            if (ObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control object");
                return null;
            }

            System.Type type = PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type;
            if (path == "(ViewControlObject)")
            {
                if (PresentationObject == null)
                    return new DisplayedValue(PresentationObject, this);
                if (!DisplayedValue.DisplayedValues.ContainsKey(PresentationObject))
                    DisplayedValue.DisplayedValues[PresentationObject] = new DisplayedValue(PresentationObject, this);
                return DisplayedValue.DisplayedValues[PresentationObject];
            }
            if (path.IndexOf("(ViewControlObject)") == 0)
                path = path.Substring("(ViewControlObject).".Length);


            return GetDisplayedValue(PresentationObject, type, path, pathDataDisplayer);
        }

        /// <summary>
        /// Retrieve the displayed value of object
        /// </summary>
        /// <param name="obj">the object of displayed value </param>
        /// <param name="pathDataDisplayer">The object which display the value.</param>
        /// <returns>The displayd value of object</returns>
        /// <MetaDataID>{430FBD1D-1185-4076-84D6-4A0FC5880BEE}</MetaDataID>
        public DisplayedValue GetDisplayedValue(object obj, IPathDataDisplayer pathDataDisplayer)
        {
            Type type = null;
            string path = null;
            return GetDisplayedValue(obj, type, path, pathDataDisplayer);
        }
        /// <summary>
        /// The main work of this function is to load the transaction of user interface on stack and forward the call
        /// </summary>
        /// <param name="obj">object as root of path</param>
        /// <param name="type">Type where system search for member. Member is the first part of path</param>
        /// <param name="path">The path where system follow to get the value.</param>
        /// <param name="pathDataDisplayer">The object which display the value. </param>
        /// <returns>The displayd value of path</returns>
        /// <MetaDataID>{11E13CE1-B4DA-4990-931F-489B4CA58448}</MetaDataID>
        DisplayedValue GetDisplayedValue(object obj, Type type, string path, IPathDataDisplayer pathDataDisplayer)
        {
            if (!string.IsNullOrEmpty(path) && path.IndexOf("Control: ") == 0)
                return new DisplayedValue(null, this);
            if (State == ViewControlObjectState.Passive)
                return new DisplayedValue(null, this);

            if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null)
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                {
                    try
                    {
                        return InternalGetDisplayedValue(obj, type, path, pathDataDisplayer);
                    }
                    finally
                    {
                        stateTransition.Consistent = true;
                    }
                }
            }
            else
                return InternalGetDisplayedValue(obj, type, path, pathDataDisplayer);


        }
        /// <summary>
        /// Retrieves the displayd value from path with start object the obj parameter
        /// </summary>
        /// <param name="obj">object as root of path</param>
        /// <param name="classifier">Type where system search for member. Member is the first part of path</param>
        /// <param name="path">The path which follow the system to get the value.</param>
        /// <param name="pathDataDisplayer">The object which display the value. </param>
        /// <returns>Displayd value</returns>
        /// <MetaDataID>{2133057C-B8E5-4F88-A5B9-5328F27E1462}</MetaDataID>
        public DisplayedValue GetDisplayedValue(object obj, OOAdvantech.MetaDataRepository.Classifier classifier, string path, IPathDataDisplayer pathDataDisplayer)
        {
            Type type = classifier.GetExtensionMetaObject(typeof(Type)) as Type;
            return GetDisplayedValue(obj, type, path, pathDataDisplayer);
        }

        /// <MetaDataID>{DA88F963-4F70-4A8D-9F30-66265FCABA66}</MetaDataID>
        DisplayedValue InternalGetDisplayedValue(object obj, Type type, string path, IPathDataDisplayer pathDataDisplayer)
        {

            try
            {
                CurrentViewControlObject = this;

                if (type == null && path != null && string.IsNullOrEmpty(path))
                    throw new System.Exception("You can't retrieve the displayed value of path because the type parameter is null.");
                if (obj == null)
                    return new DisplayedValue(null, this); // return null display value


                obj = GetRealObject(obj);


                if (!DisplayedValue.DisplayedValues.ContainsKey(obj)) //If there isn't display value for object create one
                    DisplayedValue.DisplayedValues.Add(obj, new DisplayedValue(obj, this));

                if (string.IsNullOrEmpty(path)) // If there isn't path the return the display value for object
                    return DisplayedValue.DisplayedValues[obj];
                else
                {
                    DisplayedValue parentDisplayedValue = DisplayedValue.DisplayedValues[obj];

                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {

                        #region If exist in displayd values tree then returns the display value which already exist
                        if (parentDisplayedValue.MemberValues.ContainsKey(path))
                        {
                            if (pathDataDisplayer != null)
                                pathDataDisplayer.AddDataPathNode(parentDisplayedValue.MemberValues[path]);
                            return parentDisplayedValue.MemberValues[path][0];
                        }
                        #endregion

                        #region Else get the value of object member and create a new display value.

                        DisplayedValue memberDisplayedValue = null;
                        System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                        object memberValue = null;
                        if (memberInfo is System.Reflection.PropertyInfo)
                            memberValue = (memberInfo as System.Reflection.PropertyInfo).GetValue(obj, null);
                        if (memberInfo is System.Reflection.FieldInfo)
                            memberValue = (memberInfo as System.Reflection.FieldInfo).GetValue(obj);

                        if (memberValue != null && DisplayedValue.DisplayedValues.ContainsKey(memberValue))
                            memberDisplayedValue = DisplayedValue.DisplayedValues[memberValue];
                        else
                        {
                            memberDisplayedValue = new DisplayedValue(memberValue, this);

                            if (memberValue != null && memberDisplayedValue.ChangeStateExist)
                                DisplayedValue.DisplayedValues[memberValue] = memberDisplayedValue;
                        }

                        if (memberDisplayedValue != null)
                        {

                            if (!parentDisplayedValue.MemberValues.ContainsKey(path))
                                parentDisplayedValue.MemberValues.Add(path, new Member(path, parentDisplayedValue));
                            parentDisplayedValue.MemberValues[path][0] = memberDisplayedValue;
                            if (pathDataDisplayer != null)
                                pathDataDisplayer.AddDataPathNode(parentDisplayedValue.MemberValues[path]);
                            return memberDisplayedValue;
                        }
                        #endregion
                    }
                    else
                    {
                        string memberName = path.Substring(0, nPos);//Retrieve member from path
                        path = path.Substring(nPos + 1);//Retrieve the remainning path
                        DisplayedValue displayedValue = null;

                        if (DisplayedValue.DisplayedValues.ContainsKey(obj) && DisplayedValue.DisplayedValues[obj].MemberValues.ContainsKey(memberName))
                        {
                            Type memberType = null;
                            #region Retrieve displayed value and type of member the displayed value for member already exist
                            displayedValue = DisplayedValue.DisplayedValues[obj].MemberValues[memberName][0];
                            System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);
                            if (memberInfo is System.Reflection.PropertyInfo)
                                memberType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;
                            if (memberInfo is System.Reflection.FieldInfo)
                                memberType = (memberInfo as System.Reflection.FieldInfo).FieldType;
                            #endregion

                            parentDisplayedValue.MemberValues[memberName][0] = displayedValue;
                            if (pathDataDisplayer != null)
                                pathDataDisplayer.AddDataPathNode(parentDisplayedValue.MemberValues[memberName]);

                            //if the member value isn't null continue with remain path
                            if (displayedValue != null && displayedValue.Value != null)
                                return GetDisplayedValue(displayedValue.Value, memberType, path, pathDataDisplayer);
                            else
                                return displayedValue;

                        }
                        else
                        {
                            Type memberType = null;
                            #region Retrieve value and type of member
                            System.Reflection.MemberInfo memberInfo = GetMember(type, memberName);
                            object subNodeobj = null;
                            if (memberInfo is System.Reflection.PropertyInfo)
                            {
                                subNodeobj = (memberInfo as System.Reflection.PropertyInfo).GetValue(obj, null);
                                memberType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;
                            }
                            if (memberInfo is System.Reflection.FieldInfo)
                            {
                                subNodeobj = (memberInfo as System.Reflection.FieldInfo).GetValue(obj);
                                memberType = (memberInfo as System.Reflection.FieldInfo).FieldType;
                            }
                            subNodeobj = GetRealObject(subNodeobj);
                            #endregion

                            #region Retrieve displayed value for member value if it isn't exist create one
                            if (subNodeobj == null || !DisplayedValue.DisplayedValues.ContainsKey(subNodeobj))
                                displayedValue = new DisplayedValue(subNodeobj, this);
                            else
                                displayedValue = DisplayedValue.DisplayedValues[subNodeobj];
                            #endregion

                            #region Update displayed values tree and attach the control to the displayed value auto update system

                            if (!parentDisplayedValue.MemberValues.ContainsKey(memberName))
                                parentDisplayedValue.MemberValues.Add(memberName, new Member(memberName, parentDisplayedValue));
                            parentDisplayedValue.MemberValues[memberName][0] = displayedValue;
                            if (pathDataDisplayer != null)
                                pathDataDisplayer.AddDataPathNode(parentDisplayedValue.MemberValues[memberName]);
                            #endregion

                            //if the member value isn't null continue with remain path
                            if (displayedValue != null && displayedValue.Value != null)
                                return GetDisplayedValue(displayedValue.Value, memberType, path, pathDataDisplayer);
                            else
                                return displayedValue;
                        }

                    }

                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            finally
            {
                CurrentViewControlObject = null;
            }
            return null;
        }
        /// <summary>
        /// This method returns the original object if the obj is wrapped from UIProxy object else return the obj
        /// </summary>
        /// <param name="obj">Defines the object where maybe is a UIProxy that wrap the original object.</param>
        /// <returns>The original object if the obj is UIProxy wrap</returns>
        /// <MetaDataID>{9919A658-05DD-4EC1-8A4B-7F1DCBF870A7}</MetaDataID>
        public static object GetRealObject(object obj)
        {
            //if (obj is OOAdvantech.UserInterface.IPresentationObject)
            //    obj = (obj as OOAdvantech.UserInterface.IPresentationObject).GetRealObject();

            if (obj is MarshalByRefObject && System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) is UIProxy)
                obj = (System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) as UIProxy)._RealTransparentProxy;
            return obj;
        }
        /// <summary>
        /// Return the member metadata form parameter type with name the parameter memberName.
        /// If the there isn't then return null. If there are more than one in hierarchy then return the member which
        /// is declared in type of parameter type.
        /// Method is useful for member which is proprty or field.
        /// </summary>
        /// <param name="type">Defines the type where method look for member</param>
        /// <param name="memberName">Defines the member name</param>
        /// <returns>Member metadata object</returns>
        /// <MetaDataID>{F750076C-4FE1-4182-9530-695DE419A293}</MetaDataID>
        static private System.Reflection.MemberInfo GetMember(Type type, string memberName)
        {
            System.Reflection.MemberInfo[] members = type.GetMember(memberName);
            if (members.Length > 0)
                return members[0];
            else
            {
                OOAdvantech.MetaDataRepository.Classifier clasifier = AssemblyManager.GetClassifier(type.FullName, true);
                if (clasifier != null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in clasifier.GetAttributes(true))
                    {
                        if (attribute.Name == memberName)
                            return attribute.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in clasifier.GetRoles(true))
                    {
                        if (associationEnd.Name == memberName)
                            return associationEnd.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
                    }

                }


                return null;
            }
        }

        /// <summary>
        /// Retrieves the value  from path with start object the obj parameter
        /// System retrieve value from business object not from caching data in user interfce.
        /// This method participates in user interface update scenario. 
        /// This scenario triggered from object change state event.
        /// </summary>
        /// <param name="obj">object as root of path</param>
        /// <param name="type">Type where system search for member. 
        /// Member is the first part of path</param>
        /// <param name="path">The path where system follow to get the value.</param>
        /// <returns>The value of path</returns>
        /// <MetaDataID>{B56FE880-1C7C-490C-A94D-9966B7AF5904}</MetaDataID>
        internal  object InternalGetValue(object obj, Type type, string path)
        {
            if (obj == null)
                return null;

            //  if (!IsActive)
            //  throw new System.Exception("You can't use GetValue method. Check the IsActive preperty before call this method");


            if (path != null && path.Length > 0)
            {
                int nPos = path.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        return propertyInfo.GetValue(obj, null);
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        return fieldInfo.GetValue(obj);
                    }
                }
                else
                {
                    string member = path.Substring(0, nPos);
                    path = path.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        object subNodeobj = propertyInfo.GetValue(obj, null);
                        return InternalGetValue(subNodeobj, propertyInfo.PropertyType, path);
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                        object subNodeobj = fieldInfo.GetValue(obj);
                        return InternalGetValue(subNodeobj, fieldInfo.FieldType, path);
                    }

                }

            }
            return null;

        }
        /// <MetaDataID>{2847C0A9-878E-44EB-AD58-06FC1065C12D}</MetaDataID>
        [ThreadStatic]
        internal static ViewControlObject CurrentViewControlObject = null;

        public event EventHandler InstanceChanged;


        public bool CanAccessValue(object obj, Type type, string path)
        {
            if (Transaction != null && OOAdvantech.Transactions.Transaction.Current == null)
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                {
                    bool retValue = InternalCanAccessValue(obj, type, path);
                    stateTransition.Consistent = true;
                    return retValue;
                }
            }
            else
                return InternalCanAccessValue(obj, type, path);

        }
        /// <MetaDataID>{88FCF7ED-280B-4A3A-9A28-4FC9CA3EC1DC}</MetaDataID>
        bool InternalCanAccessValue(object obj, Type type, string path)
        {
            try
            {
                if (Transaction != null && Transaction.Status == OOAdvantech.Transactions.TransactionStatus.Aborted)
                    return false;

                if (type == null && path == null)
                    return false;

                if (!string.IsNullOrEmpty(path))
                {
                    if (path == "(ViewControlObject)")
                        return true;
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {
                        if (obj != null)
                            return true;
                        else
                            return false;

                    }
                    else
                    {
                        //if (obj != null)
                        //    return true;
                        //else
                        //    return false;

                        string member = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                        if (memberInfo is System.Reflection.PropertyInfo)
                        {
                            System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;

                            DisplayedValue displayedValue = null;
                            //TODO DisplayedValue.DisplayedValues.ContainsKey(obj) στο else τα γράφω κανονικά
                            if (DisplayedValue.DisplayedValues.ContainsKey(obj) && DisplayedValue.DisplayedValues[obj].MemberValues.ContainsKey(member))
                                displayedValue = DisplayedValue.DisplayedValues[obj].MemberValues[member][0];
                            else
                            {
                                DisplayedValue parentDisplayedValue = DisplayedValue.DisplayedValues[obj];

                                object subNodeobj = propertyInfo.GetValue(obj, null);
                                if (subNodeobj != null)
                                {
                                    if (!DisplayedValue.DisplayedValues.ContainsKey(subNodeobj))
                                        displayedValue = new DisplayedValue(subNodeobj, this);
                                    else
                                        displayedValue = DisplayedValue.DisplayedValues[subNodeobj];

                                    if (!parentDisplayedValue.MemberValues.ContainsKey(member))
                                        parentDisplayedValue.MemberValues.Add(member, new Member(member, parentDisplayedValue));

                                    parentDisplayedValue.MemberValues[member][0] = displayedValue;
                                }

                            }
                            if (displayedValue != null && displayedValue.Value != null)
                            {

                                DisplayedValue.DisplayedValues[displayedValue.Value] = displayedValue;
                                return CanAccessValue(displayedValue.Value, propertyInfo.PropertyType, path);
                            }
                            else
                                return false;
                        }

                        if (memberInfo is System.Reflection.FieldInfo)
                        {
                            System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                            DisplayedValue displayedValue = null;
                            if (DisplayedValue.DisplayedValues.ContainsKey(obj) && DisplayedValue.DisplayedValues[obj].MemberValues.ContainsKey(path))
                                displayedValue = DisplayedValue.DisplayedValues[obj].MemberValues[path][0];
                            else
                            {
                                DisplayedValue parentDisplayedValue = DisplayedValue.DisplayedValues[obj];

                                object subNodeobj = fieldInfo.GetValue(obj);
                                if (subNodeobj != null)
                                {
                                    if (!DisplayedValue.DisplayedValues.ContainsKey(subNodeobj))
                                        displayedValue = new DisplayedValue(subNodeobj, this);
                                    else
                                        displayedValue = DisplayedValue.DisplayedValues[subNodeobj];


                                    if (!parentDisplayedValue.MemberValues.ContainsKey(member))
                                        parentDisplayedValue.MemberValues.Add(member, new Member(member, parentDisplayedValue));
                                    parentDisplayedValue.MemberValues[member][0] = displayedValue;
                                }

                            }
                            if (displayedValue != null && displayedValue.Value != null)
                            {
                                DisplayedValue.DisplayedValues[displayedValue.Value] = displayedValue;
                                return CanAccessValue(displayedValue.Value, fieldInfo.FieldType, path);
                            }
                            else
                                return false;
                        }

                    }

                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            return false;

        }
        /// <MetaDataID>{FCCC3233-3229-4F33-93B3-C7423F2D3E7C}</MetaDataID>
        public bool CanAccessValue(string path)
        {
            if (PresentationObjectType == null)
                return false;
            return CanAccessValue(PresentationObject, PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as System.Type, path);
        }
        /// <MetaDataID>{6E7D04D6-F909-4769-97EE-DDC0A644D7C7}</MetaDataID>
        public static Type GetType(Type type, string source)
        {
            if (type == null)
                return null;


            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, source);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        return propertyInfo.PropertyType;
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        return fieldInfo.FieldType;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        return GetType(propertyInfo.PropertyType, source);
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        return GetType(fieldInfo.FieldType, source);
                    }
                }
            }
            else
                return type;
            return null;


        }

        /// <MetaDataID>{E8DB6C23-0A1A-4E95-9A5A-E338CC0788B9}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.MetaObject GetMetaObject(OOAdvantech.MetaDataRepository.Classifier classifier, string source)
        {
            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == source)
                            return attribute;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == source)
                            return associationEnd;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == member)
                            return GetMetaObject(attribute.Type, source); ;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == member)
                            if (associationEnd.CollectionClassifier != null)
                                return GetMetaObject(associationEnd.CollectionClassifier, source);

                            else
                                return GetMetaObject(associationEnd.Specification, source);
                    }
                }
            }
            return null;
        }

        /// <MetaDataID>{DF91D997-DCC6-41DF-97F7-B95C8F67CFF8}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier GetClassifier(OOAdvantech.MetaDataRepository.Classifier classifier, string source)
        {
            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == source)
                            return attribute.Type;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == source)
                            if (associationEnd.CollectionClassifier != null)
                                return associationEnd.CollectionClassifier;
                            else
                                return associationEnd.Specification;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Name == member)
                            return GetClassifier(attribute.Type, source); ;
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Name == member)
                            if (associationEnd.CollectionClassifier != null)
                                return GetClassifier(associationEnd.CollectionClassifier, source);

                            else
                                return GetClassifier(associationEnd.Specification, source);
                    }
                }
            }
            return null;
        }
        /// <MetaDataID>{93454EB4-31D3-48EA-B4B1-7E13DA40237B}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifier(string path)
        {
            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control component.");
                return null;
            }
            if (string.IsNullOrEmpty(path))
                return null;
            if (path == "(ViewControlObject)")
                return PresentationObjectType;
            return GetClassifier(PresentationObjectType, path);


        }

        /// <MetaDataID>{4C932B4B-C099-411E-8919-D990F58B3A25}</MetaDataID>
        public Type GetType(string path)
        {
            if (AssemblyManager.InVisualStudio)
                throw new System.Exception("Operation GetType doesn't implement for the designe mode");

            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control component.");
                return null;
            }
            if (string.IsNullOrEmpty(path))
                return null;

            //if (Instance== null)
            //{
            //    System.Diagnostics.Debug.WriteLine("There isn't instance on view control component.");
            //    return null;
            //}


            System.Type type = PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type;
            if (path == "(ViewControlObject)")
                return type;
            if (path.IndexOf("(ViewControlObject)") == 0)
                path = path.Substring("(ViewControlObject).".Length);


            return GetType(type, path);



        }


        /// <MetaDataID>{DA0A1FF5-0545-4F6F-B601-4E9498B11766}</MetaDataID>
        internal bool IsReadOnly(Type type, string source)
        {
            if (source != null && source.Length > 0)
            {
                int nPos = source.IndexOf(".");
                if (nPos == -1)
                {
                    System.Reflection.MemberInfo memberInfo = GetMember(type, source);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        return !propertyInfo.CanWrite;
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        return fieldInfo.IsInitOnly;
                    }
                }
                else
                {
                    string member = source.Substring(0, nPos);
                    source = source.Substring(nPos + 1);
                    System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                    if (memberInfo is System.Reflection.PropertyInfo)
                    {
                        System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                        //if (!propertyInfo.CanWrite)
                        //    return true;

                        return IsReadOnly(propertyInfo.PropertyType, source);
                    }

                    if (memberInfo is System.Reflection.FieldInfo)
                    {
                        System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                        //if (fieldInfo.IsInitOnly)
                        //    return true;
                        return IsReadOnly(fieldInfo.FieldType, source);
                    }
                }
            }
            if (type != null)
                System.Diagnostics.Debug.WriteLine("System can't find member '" + source + "' on type " + type.FullName + ".");
            else
                System.Diagnostics.Debug.WriteLine("System can't find member '" + source + "'.");

            return true;


        }
        /// <MetaDataID>{5B2B85D2-2CA0-4755-A18A-779CF30C1BF3}</MetaDataID>
        internal bool IsReadOnly(string path)
        {
            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control object");
                return true;
            }
            System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
            return IsReadOnly(type, path);
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.ObjectNode _RootObjectNode;
        [Editor(typeof(EditCachingDataMetaTree), typeof(System.Drawing.Design.UITypeEditor))]
        public OOAdvantech.UserInterface.ObjectNode RootObjectNode
        {
            get
            {
                try
                {
                    if (_RootObjectNode == null)
                    {
                        _RootObjectNode = new OOAdvantech.UserInterface.ObjectNode("Root", null);
                        foreach (System.ComponentModel.IComponent component in _ControlledComponents)
                        {
                            if (component is IObjectMemberViewControl)
                            {
                                foreach (string path in (component as IObjectMemberViewControl).AllPaths)
                                    _RootObjectNode.AddPath(path);
                            }
                        }
                    }
                    else if (DesignMode)
                    {
                        foreach (System.ComponentModel.IComponent component in _ControlledComponents)
                        {
                            if (component is IObjectMemberViewControl)
                            {
                                foreach (string path in (component as IObjectMemberViewControl).AllPaths)
                                    _RootObjectNode.AddPath(path);
                            }
                        }
                    }

                }
                catch (System.Exception error)
                {

                }
                return _RootObjectNode;

            }
            set
            {
            }

        }




        /// <MetaDataID>{F0AC2FC1-D437-4EB1-87FA-47090FE78425}</MetaDataID>
        public void SetValue(object obj, object value, OOAdvantech.MetaDataRepository.Classifier classifier, string path)
        {
            if (State == ViewControlObjectState.UserInteraction || State == ViewControlObjectState.SaveControlValues)
            {

                Type type = classifier.GetExtensionMetaObject(typeof(Type)) as Type;
                if (Transaction != null)
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                    {
                        InternalSetValue(obj, value, type, path);
                        stateTransition.Consistent = true;
                    }
                }
                else
                    InternalSetValue(obj, value, type, path);
            }
        }
        /// <MetaDataID>{7E598AD1-54D6-4AE4-B768-CB1861C5599E}</MetaDataID>
        internal void SetValue(object value, string path)
        {

            if (PresentationObjectType == null)
            {
                System.Diagnostics.Debug.WriteLine("There isn't type for view control object");
                return;
            }
            if (PresentationObject == null && path != "(ViewControlObject)")
                return;

            if (Transaction != null)
            {
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                {
                    System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
                    if (path == "(ViewControlObject)")
                        Instance = value;
                    else
                        InternalSetValue(PresentationObject, value, type, path);
                    stateTransition.Consistent = true;
                }
            }
            else
            {
                System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
                if (path == "(ViewControlObject)")
                    Instance = value;
                else
                    InternalSetValue(PresentationObject, value, type, path);
            }




        }
        /// <MetaDataID>{86EC8471-32FD-469F-887F-A90979AB33A4}</MetaDataID>
        internal void UpdateValue(object obj, object value, Type type, string path, string fullPath)
        {
            try
            {
                if (!DisplayedValue.DisplayedValues.ContainsKey(obj))
                    return;

                DisplayedValue parentDisplayedValue = DisplayedValue.DisplayedValues[obj];

                if (path != null && path.Length > 0)
                {
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {
                        if (parentDisplayedValue.MemberValues.ContainsKey(path) &&
                            parentDisplayedValue.MemberValues[path][0].Value == value)
                            return;
                        if (!parentDisplayedValue.MemberValues.ContainsKey(path))
                            parentDisplayedValue.MemberValues.Add(path, new Member(path, parentDisplayedValue));
                        if (value != null && DisplayedValue.DisplayedValues.ContainsKey(value))
                            parentDisplayedValue.MemberValues[path][0] = DisplayedValue.DisplayedValues[value];
                        else
                            parentDisplayedValue.MemberValues[path][0] = new DisplayedValue(value, this);
                        return;


                    }
                    else
                    {
                        string member = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                        if (memberInfo is System.Reflection.PropertyInfo)
                        {
                            System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                            UpdateValue(propertyInfo.GetValue(obj, null), value, propertyInfo.PropertyType, path, fullPath);
                            return;
                        }

                        if (memberInfo is System.Reflection.FieldInfo)
                        {
                            System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                            UpdateValue(fieldInfo.GetValue(obj), value, fieldInfo.FieldType, path, fullPath);
                            return;
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }

        }

        /// <MetaDataID>{0229E382-9188-4FFC-81DD-6B5BFA49A549}</MetaDataID>
        internal void InternalSetValue(object obj, object value, Type type, string path)
        {
            try
            {
                CurrentViewControlObject = this;
                obj = GetRealObject(obj);

                if (!DisplayedValue.DisplayedValues.ContainsKey(obj))
                    return;



                DisplayedValue parentDisplayedValue = DisplayedValue.DisplayedValues[obj];

                if (path != null && path.Length > 0)
                {
                    int nPos = path.IndexOf(".");
                    if (nPos == -1)
                    {
                        System.Reflection.MemberInfo memberInfo = GetMember(type, path);
                        if (memberInfo is System.Reflection.PropertyInfo)
                        {
                            System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                            if (propertyInfo.CanWrite)
                                propertyInfo.SetValue(obj, value, null);

                        }

                        if (memberInfo is System.Reflection.FieldInfo)
                        {

                            System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                            if (!fieldInfo.IsInitOnly)
                                fieldInfo.SetValue(obj, value);

                        }

                        if (parentDisplayedValue.MemberValues.ContainsKey(path) &&
                            parentDisplayedValue.MemberValues[path][0].Value == value)
                            return;
                        if (!parentDisplayedValue.MemberValues.ContainsKey(path))
                            parentDisplayedValue.MemberValues.Add(path, new Member(path, parentDisplayedValue));
                        if (value == null)
                            parentDisplayedValue.MemberValues[path][0] = new DisplayedValue(value, this);
                        else
                        {
                            if (DisplayedValue.DisplayedValues.ContainsKey(value))
                                parentDisplayedValue.MemberValues[path][0] = DisplayedValue.DisplayedValues[value];
                            else
                                parentDisplayedValue.MemberValues[path][0] = new DisplayedValue(value, this);
                        }
                        return;


                    }
                    else
                    {
                        string member = path.Substring(0, nPos);
                        path = path.Substring(nPos + 1);
                        System.Reflection.MemberInfo memberInfo = GetMember(type, member);
                        if (memberInfo is System.Reflection.PropertyInfo)
                        {
                            System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                            InternalSetValue(propertyInfo.GetValue(obj, null), value, propertyInfo.PropertyType, path);
                            return;
                        }

                        if (memberInfo is System.Reflection.FieldInfo)
                        {
                            System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;

                            InternalSetValue(fieldInfo.GetValue(obj), value, fieldInfo.FieldType, path);
                            return;
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
            finally
            {
                CurrentViewControlObject = null;
            }



        }

        /// <MetaDataID>{0B95659E-4488-45A2-8D8A-DB2FAAC686DD}</MetaDataID>
        [Browsable(false)]
        public virtual OOAdvantech.Transactions.Transaction Transaction
        {
            get
            {
                if (_MasterViewControlObject != null)
                    return _MasterViewControlObject.Transaction;
                return null;
            }
            set
            {

            }
        }

        /// <MetaDataID>{E622F956-4440-4DC4-8B94-7A161861ED3A}</MetaDataID>
        internal object Invoke(object instance, System.Reflection.MethodInfo methodInfo, object[] parameters)
        {
            return Invoke(instance, methodInfo, parameters, OOAdvantech.Transactions.TransactionOption.Supported);
        }
        /// <MetaDataID>{AB8F0175-18F7-44F0-9282-EA7C7C65E32A}</MetaDataID>
        internal object Invoke(object instance, System.Reflection.MethodInfo methodInfo, object[] parameters, OOAdvantech.Transactions.TransactionOption transactionOption)
        {
            #region Gets real objects if there are UIProxy objects
            instance = GetRealObject(instance);
            int i = 0;
            foreach (object parameterObject in parameters)
                parameters[i++] = GetRealObject(parameterObject);
            #endregion

            if (Transaction != null && transactionOption != OOAdvantech.Transactions.TransactionOption.Suppress)
            {
                object retValue = null;
                using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(Transaction))
                {
                    using (OOAdvantech.Transactions.SystemStateTransition innerStateTransition = new OOAdvantech.Transactions.SystemStateTransition(transactionOption))
                    {
                        //System.Type type = ModulePublisher.ClassRepository.GetType(PresentationObjectType.FullName, "");
                        try
                        {
                            retValue = methodInfo.Invoke(instance, parameters);
                        }
                        catch (System.Exception error)
                        {
                            if (OOAdvantech.Transactions.Transaction.Current.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                            {
                                if (error is System.Reflection.TargetInvocationException)
                                    innerStateTransition.StateTransitionTransaction.Abort(error.InnerException);
                                else
                                    innerStateTransition.StateTransitionTransaction.Abort(error);
                            }

                            throw;
                        }
                        if (OOAdvantech.Transactions.Transaction.Current.Status == OOAdvantech.Transactions.TransactionStatus.Continue)
                            innerStateTransition.Consistent = true;
                    }
                    stateTransition.Consistent = true;
                    return retValue;
                }
            }
            else
            {
                if (transactionOption == OOAdvantech.Transactions.TransactionOption.Suppress)
                    return methodInfo.Invoke(instance, parameters);
                else
                {
                    using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(transactionOption))
                    {
                        object retValue = methodInfo.Invoke(instance, parameters);
                        stateTransition.Consistent = true;
                        return retValue;

                    }

                }
            }

        }
        /// <MetaDataID>{712BA740-61A2-4604-80CE-18611930490B}</MetaDataID>
        internal object Invoke(System.Reflection.MethodInfo methodInfo, object[] parameters, OOAdvantech.Transactions.TransactionOption transactionOption)
        {
            return Invoke(Instance, methodInfo, parameters, transactionOption);
        }
        /// <MetaDataID>{39E01A31-4496-4360-9519-FCC891399B4C}</MetaDataID>
        internal object Invoke(System.Reflection.MethodInfo methodInfo, object[] parameters)
        {
            return Invoke(Instance, methodInfo, parameters);
        }

        /// <MetaDataID>{C254F7CF-B2F8-4586-BCDB-FC9A56C65BA6}</MetaDataID>
        ///<summary>
        /// Disconnect controls from displayed values tree.
        /// Removes the event handlers where consume the object change state events
        ///</summary>
        public void ReleaseDataPathNodes()
        {
            foreach (System.ComponentModel.IComponent component in _ControlledComponents)
            {
                if (component is IPathDataDisplayer)
                    (component as IPathDataDisplayer).ReleaseDataPathoNodes();
                if (component is ViewControlObject)
                    (component as ViewControlObject).ReleaseDataPathNodes();

            }

        }

        /// <MetaDataID>{C2D803A3-5C21-4249-B5D9-832868D8D364}</MetaDataID>
        internal bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string PropertyDescriptor)
        {
            try
            {

                if (metaObject is OOAdvantech.MetaDataRepository.Class && PropertyDescriptor == "AssignPresentationObjectType")
                {
                    if (_ObjectType == null)
                        if (!string.IsNullOrEmpty(_ViewObjectTypeFullName))
                            _ObjectType = AssemblyManager.GetClassifier(_ViewObjectTypeFullName, true);


                    foreach (OOAdvantech.MetaDataRepository.Classifier _interface in (metaObject as OOAdvantech.MetaDataRepository.Class).GetAllGeneralClasifiers())
                    {
                        if (!(_interface is OOAdvantech.MetaDataRepository.Class))
                            continue;
                        if (_interface.TemplateBinding != null && (_interface.TemplateBinding.Signature.Template as OOAdvantech.MetaDataRepository.Classifier).FullName == typeof(OOAdvantech.UserInterface.PresentationObject<>).FullName)
                        {
                            OOAdvantech.MetaDataRepository.Classifier realObjectClass = (_interface.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier);
                            foreach (OOAdvantech.MetaDataRepository.Operation operation in (metaObject as OOAdvantech.MetaDataRepository.Class).GetOperations((metaObject as OOAdvantech.MetaDataRepository.Class).Name, true))
                            {
                                if (operation.Parameters.Count == 1)
                                {
                                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                                    {
                                        if (parameter.Type.IsA(realObjectClass) && parameter.Type == _ObjectType)
                                            return true;
                                    }
                                }
                            }
                        }
                    }
                    System.Windows.Forms.MessageBox.Show("You can't assign the '" + metaObject.FullName + "' to the PresentationObjectType.\n Check the property rules.");
                    return false;
                }
                if (metaObject is OOAdvantech.MetaDataRepository.Classifier)
                    return true;
                return false;
            }
            catch (System.Exception error)
            {
                throw;
            }


        }


        int ControlValuesUpdateRefCount = 0;
        internal void EndControlValuesUpdate()
        {
            ControlValuesUpdateRefCount--;
            if (ControlValuesUpdateRefCount == 0)
                State = ViewControlObjectState.UserInteraction;
        }
        internal void StartControlValuesUpdate()
        {
            State = ViewControlObjectState.UpdateControlValues;
            ControlValuesUpdateRefCount++;
        }
    }
}
