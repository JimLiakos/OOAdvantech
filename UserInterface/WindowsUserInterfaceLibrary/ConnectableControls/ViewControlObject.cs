using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using ConnectableControls.PropertyEditors;
using OOAdvantech.UserInterface.Runtime;
using System.Xml.Linq;

namespace ConnectableControls
{


    /// <MetaDataID>{D5B608AE-44DF-4893-B11F-A11B54102825}</MetaDataID>
    public class ViewControlObject : System.ComponentModel.Component, OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl, OOAdvantech.UserInterface.Runtime.IOperationCallerSource, OOAdvantech.UserInterface.Runtime.ICutCopyPasteMoveDragDrop
    {
        /// <MetaDataID>{9473e11b-3b3a-4d81-a3e6-7765319f8aa6}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{39c31f26-900c-407e-b28d-641f2f3a1f71}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{22dae133-8880-4586-ad7d-adcf4090ebad}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{fbbb26a5-2191-46db-963d-1329376f3c16}</MetaDataID>
        public void InitializeControl()
        {

        }


        /// <MetaDataID>{ed2a5bf8-75f5-4e5d-b8a9-00e55a47d753}</MetaDataID>
        public virtual void OnBeforeViewControlObjectInitialization()
        {

        }
        /// <MetaDataID>{6b15d933-b27a-4dc8-9012-e6b23c1bcab6}</MetaDataID>
        public virtual void OnAfterViewControlObjectInitialization()
        {
        }
        /// <MetaDataID>{118ddb28-363c-43bc-a6fa-94c36307674b}</MetaDataID>
        public void DisableAllControls()
        {
            try
            {

                DisableAllControls(ContainerControl.FindForm());

            }
            catch (Exception error)
            {
            }
        }
        /// <MetaDataID>{c563fd18-2e4b-4dfa-b2cd-6e166eda9d80}</MetaDataID>
        public void DisableAllControls(Control containerControl)
        {
            try
            {
                foreach (Control control in containerControl.Controls)
                {
                    if (control is Button && (
                        (control as Button).DialogResult == System.Windows.Forms.DialogResult.Cancel ||
                        (control as Button).DialogResult == System.Windows.Forms.DialogResult.Abort ||
                        (control as Button).DialogResult == System.Windows.Forms.DialogResult.No))
                    {
                        continue;
                    }
                    try
                    {
                        if (!(control is Panel) || control.Enabled)
                            control.Enabled = false;
                        DisableAllControls(control);
                    }
                    catch (Exception error)
                    {
                    }
                }

            }
            catch (Exception error)
            {
            }
        }



        /// <MetaDataID>{0348bd34-5c42-4418-98b5-339fcbc6fc5e}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection;
        /// <exclude>Excluded</exclude>
        /// <summary>
        /// If the Removed property is true then the component has removed from the Form (Designe Area)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Removed
        {
            get
            {
                return OnRemove;
            }
        }

        /// <MetaDataID>{e909c3fe-9598-471d-9c39-be7be2514ff8}</MetaDataID>
        public object ControlObject(object _object)
        {
            return UserInterfaceObjectConnection.ControlObject(_object);
        }

        /// <MetaDataID>{E5BB2506-F9EB-4726-ABFC-0DD3DA12966B}</MetaDataID>
        public override string ToString()
        {
            return Name;
        }
        /// <exclude>Excluded</exclude>
        private string _Name;

        /// <MetaDataID>{3D72589A-9E4A-4EEC-BC0B-8C9174321AD3}</MetaDataID>
        //[Browsable(false)]
        [Category("Object Model Connection")]
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
                {
                    _Name = value;
                    // theViewControlObject.Name = _Name;
                }
            }
        }


        /// <MetaDataID>{85164593-9C73-4DBB-8066-3ACBFAE972D3}</MetaDataID>
        public static ITraslator Translator;

        /// <MetaDataID>{7b17a515-59e2-40a5-bf64-487cfa538c82}</MetaDataID>
        string _ViewControlObjectAssembly;
        /// <MetaDataID>{a694e1c1-0926-4d60-9283-3f1a2be6747d}</MetaDataID>
        [Browsable(false)]
        public string ViewControlObjectAssembly
        {
            get
            {
                return _ViewControlObjectAssembly;

            }
            set
            {
                _ViewControlObjectAssembly = value;
                UserInterfaceObjectConnection.AssemblyMetadata = value;
            }
        }

        /// <MetaDataID>{8c81ca9a-b23b-482a-b742-0437540ed601}</MetaDataID>
        ConnectedTypeMetaData _ConnectedObjectTypeMetaData;
        /// <MetaDataID>{37c51c0b-ed6c-4251-a6ed-3adfb0dc0565}</MetaDataID>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ConnectedTypeMetaData ConnectedObjectTypeMetaData
        {
            get
            {

                return _ConnectedObjectTypeMetaData;
            }
            set
            {
                if (value != null)
                {
                    _ConnectedObjectTypeMetaData = value;
                    _ConnectedObjectTypeMetaData.ConnectableControl = this;
                }
            }
        }

        /// <MetaDataID>{5810f8f0-af60-4c05-94e0-0859157a1dea}</MetaDataID>
        [Category("Transaction Settings")]
        public OOAdvantech.Transactions.TransactionOption TransactionOption
        {
            get
            {
                return UserInterfaceObjectConnection.TransactionOption;
            }
            set
            {
                UserInterfaceObjectConnection .TransactionOption = value;
            }
        }
        /// <MetaDataID>{aa77656a-6b10-490f-ac34-c9ea96e1ca90}</MetaDataID>
        [Category("Transaction Settings")]
        public bool IniateTransactionOnInstanceSet
        {
            get
            {
                return UserInterfaceObjectConnection.IniateTransactionOnInstanceSet;
            }
            set
            {
                UserInterfaceObjectConnection.IniateTransactionOnInstanceSet = value;
            }
        }
        /// <MetaDataID>{95297e6a-6114-4721-8fb0-ced3fdaa54a7}</MetaDataID>
        [Description("The lock wait time in Milliseconds")]
        [Category("Transaction Settings")]
        public int TransactionObjectLockTimeOut
        {
            get
            {
                return (int)UserInterfaceObjectConnection .TransactionObjectLockTimeOut.TotalMilliseconds;
            }
            set
            {
                UserInterfaceObjectConnection .TransactionObjectLockTimeOut = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <MetaDataID>{1BFCA073-9EA1-4DF7-A4CE-177CB4487050}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object ViewControlObjectType
        {
            get
            {
                if (UserInterfaceObjectConnection.ObjectType != null)
                    return UserInterfaceObjectConnection.ObjectType.FullName;

                return UserInterfaceObjectConnection.ViewObjectTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Classifier)
                {

                    UserInterfaceObjectConnection.ViewObjectTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                    _ViewControlObjectAssembly = (value as OOAdvantech.MetaDataRepository.Classifier).ImplementationUnit.Name;
                    UserInterfaceObjectConnection.AssemblyMetadata = _ViewControlObjectAssembly;
                    TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                }

                if (UserInterfaceObjectConnection.ObjectType != null)
                {
                    UserInterfaceObjectConnection.ViewObjectTypeFullName = UserInterfaceObjectConnection.ObjectType.FullName;
                    _ViewControlObjectAssembly = UserInterfaceObjectConnection.ObjectType.ImplementationUnit.Name;
                    TypeDescriptor.GetProperties(this).Find("ViewControlObjectAssembly", false).SetValue(this, _ViewControlObjectAssembly);
                    try
                    {
                        var assmb = UserInterfaceObjectConnection.ObjectType.GetExtensionMetaObject<System.Type>().Assembly;
                    }
                    catch (Exception error)
                    {

                        
                    }
                }
                if (value is string)
                    UserInterfaceObjectConnection.ViewObjectTypeFullName = value as string;
            }
        }





        /// <MetaDataID>{91d3e541-dec3-44e1-8335-9bdcd2afbdad}</MetaDataID>
        [Category("Object Model Connection")]
        virtual public ViewControlObject MasterViewControlObject
        {
            set
            {
                if (value != null)
                    UserInterfaceObjectConnection.MasterViewControlObject = value.UserInterfaceObjectConnection;
                else
                    UserInterfaceObjectConnection.MasterViewControlObject = null;

            }
            get
            {
                if (UserInterfaceObjectConnection.MasterViewControlObject == null)
                    return null;
                return UserInterfaceObjectConnection.MasterViewControlObject.PresentationContextViewControl as ViewControlObject;
            }
        }



        /// <MetaDataID>{0CA482CD-1ADD-4336-B46B-0963FB299D5B}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object AssignPresentationObjectType
        {
            get
            {

                return UserInterfaceObjectConnection.PresentationObjectTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Class)
                    UserInterfaceObjectConnection.PresentationObjectType = value as OOAdvantech.MetaDataRepository.Class;
                if (value is string)
                    UserInterfaceObjectConnection.PresentationObjectTypeFullName = value as string;
                if (value == null)
                {
                    UserInterfaceObjectConnection.PresentationObjectTypeFullName = "";
                    UserInterfaceObjectConnection.PresentationObjectType = null;
                }

            }
        }



        /// <summary>Required designer variable.</summary>
        /// <MetaDataID>{400B1B99-A5A4-412A-AB31-91E3348AD8D4}</MetaDataID>
        private System.ComponentModel.Container components = null;

        /// <MetaDataID>{de32d383-4dbd-4574-83ff-8254ccb54fe4}</MetaDataID>
        System.Windows.Forms.Form HostForm;
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
                if (DesignMode)
                {
                    _ContainerControl = value;
                    return;
                }
                else
                {


                    if (_ContainerControl != value && _ContainerControl != null)
                    {
                        UnHookEventForDragAndDrop(_ContainerControl);
                        if (_ContainerControl.FindForm() != null && HostForm != null)
                        {
                            HostForm.Closed -= new EventHandler(ContainerControlClosed);
                            HostForm = null;
                        }
                    }
                    if (_ContainerControl != value)
                    {
                        _ContainerControl = value;

                        if (_ContainerControl != null)
                        {

                            HookEventForDragAndDrop(_ContainerControl);
                            _ContainerControl.VisibleChanged += new EventHandler(ContainerControlVisibleChanged);
                        }
                    }
                }
            }
        }



        #region DragDrop Behavior
        /// <MetaDataID>{d3b1e8b7-9434-4e3f-944c-c5b10094ba6c}</MetaDataID>
        protected List<Control> HookedForDragAndDropControls = new List<Control>();
        /// <MetaDataID>{ae6e0429-9f72-4607-b74d-929f648bcded}</MetaDataID>
        protected void HookEventForDragAndDrop(Control control)
        {
            if (DesignMode || !(AllowDrag || AllowDrop))
                return;
            if (!HookedForDragAndDropControls.Contains(control))
            {
                if (control is OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl &&
                    (control as OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl).AllowDrag)
                    return;

                if (control is ConnectableControls.ListView.IListView &&
                    (control as ConnectableControls.ListView.IListView).ListConnection != null &&
                    (control as ConnectableControls.ListView.IListView).ListConnection.AllowDrag)
                    return;


                if (control == ContainerControl)
                {
                }
                HookedForDragAndDropControls.Add(control);
                if (AllowDrag)
                    control.MouseMove += new MouseEventHandler(ContainerControlMouseMove);
                mouseMoveCount++;
                control.ControlAdded += new ControlEventHandler(OnControlAdded);
                control.ControlRemoved += new ControlEventHandler(OnControlRemoved);
                control.DragEnter += new DragEventHandler(OnDragEnter);
                control.DragOver += new DragEventHandler(OnDragOver);
                control.DragDrop += new DragEventHandler(OnDragDrop);
                control.DragLeave += new EventHandler(OnDragLeave);

                foreach (Control innerControl in control.Controls)
                    HookEventForDragAndDrop(innerControl);

            }

        }
        /// <MetaDataID>{71da2ea7-6569-4bf3-9b7b-d344688c3a5c}</MetaDataID>
        protected void UnHookEventForDragAndDrop(Control control)
        {
            if (DesignMode)
                return;
            if (HookedForDragAndDropControls.Contains(control))
            {
                if (AllowDrag)
                    control.MouseMove -= new MouseEventHandler(ContainerControlMouseMove);
                mouseMoveCount--;
                control.ControlAdded -= new ControlEventHandler(OnControlAdded);
                control.ControlRemoved -= new ControlEventHandler(OnControlRemoved);
                control.DragEnter -= new DragEventHandler(OnDragEnter);
                control.DragOver -= new DragEventHandler(OnDragOver);
                control.DragDrop -= new DragEventHandler(OnDragDrop);
                control.DragLeave -= new EventHandler(OnDragLeave);
                HookedForDragAndDropControls.Remove(control);
                foreach (Control innerControl in control.Controls)
                {
                    UnHookEventForDragAndDrop(innerControl);
                }

                if (control == ContainerControl)
                {
                }

            }

        }
        /// <MetaDataID>{50467e2a-7780-4570-8d68-216823dae0a5}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{7ab13b1a-3157-42b8-8f94-f004f9b2e068}</MetaDataID>
        [Category("DragDrop Behavior")]
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

        /// <MetaDataID>{93611921-f897-43ce-9852-bc8bdb65db84}</MetaDataID>
        [Category("DragDrop Behavior")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowDrop
        {
            get
            {
                if (ContainerControl != null)
                    return ContainerControl.AllowDrop;
                else
                    return false;
            }
        }

        /// <MetaDataID>{40b95fc3-2894-4e4c-9576-e6543d744e84}</MetaDataID>
        DragDropTransactionOptions _DragDropTransactionOption;
        /// <MetaDataID>{2039c0a7-3a0d-4c42-a9af-c1eeb0c77cb3}</MetaDataID>
        [Category("DragDrop Behavior")]
        public DragDropTransactionOptions DragDropTransactionOption
        {
            get
            {
                return _DragDropTransactionOption;
            }
            set
            {
                _DragDropTransactionOption = value;
            }
        }

        /// <MetaDataID>{f5811504-ed3e-43c9-9c1b-f985bfef38f7}</MetaDataID>
        void OnDragLeave(object sender, EventArgs e)
        {
            DragDropObject = null;
        }

        /// <MetaDataID>{ca2f5fa5-03d5-47b3-8d33-de3454508a00}</MetaDataID>
        public void CutObject(object dropObject)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{cca9af23-29c8-4933-8757-34800dea384f}</MetaDataID>
        public void PasteObject(object dropObject)
        {
            if (DragDropOperationCaller != null && DragDropOperationCaller.Operation != null)
            {
                object @object = DragDropOperationCaller.Invoke();
                if ((UserInterfaceObjectConnection.ObjectType.GetExtensionMetaObject(typeof(Type)) as Type).IsInstanceOfType(@object))
                    Instance = @object;
            }
            else if ((UserInterfaceObjectConnection.ObjectType.GetExtensionMetaObject(typeof(Type)) as Type).IsInstanceOfType(dropObject))
                Instance = dropObject;
        }

        /// <MetaDataID>{7b422231-84a0-4157-9b51-88d9a26a3bba}</MetaDataID>
        void OnDragDrop(object sender, DragEventArgs e)
        {
            
                DragDropObject = e.Data.GetData(e.Data.GetFormats()[0]);

                if (DragDropObject is DragDropActionManager)
                    (DragDropObject as DragDropActionManager).DropObject((OOAdvantech.DragDropMethod)e.Effect, this);
                else
                    PasteObject(DragDropObject);
            
            DragDropObject = null;
        }

        /// <MetaDataID>{443b5ed0-63e7-497c-b7ed-1e851972a6a5}</MetaDataID>
        void OnDragOver(object sender, DragEventArgs e)
        {
            if (sender is System.Windows.Forms.Control)
                System.Diagnostics.Debug.WriteLine((sender as System.Windows.Forms.Control).Name);

        }

        /// <MetaDataID>{9ad1f4f6-4258-4796-a108-71a7b1ba9b22}</MetaDataID>
        void OnDragEnter(object sender, DragEventArgs drgevent)
        {
            if (drgevent.Effect == DragDropEffects.None)
            {
                DragDropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
                if (DragDropObject is DragDropActionManager)
                    DragDropObject = (DragDropObject as DragDropActionManager).DragedObject;

                if (AllowDropOperationCaller != null && AllowDropOperationCaller.Operation != null)
                {
                    object ret = AllowDropOperationCaller.Invoke();
                    if (ret is OOAdvantech.DragDropMethod)
                        drgevent.Effect = (DragDropEffects)ret;
                }
                else
                {
                    if ((UserInterfaceObjectConnection.ObjectType.GetExtensionMetaObject(typeof(Type)) as Type).IsInstanceOfType(DragDropObject))
                        drgevent.Effect = DragDropEffects.Copy;
                    else
                        drgevent.Effect = DragDropEffects.None;
                }
            }

        }

        /// <MetaDataID>{832e2fd3-49a0-41dd-b817-fa151f92cd88}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _AllowDropOperationCaller;
        /// <MetaDataID>{b191fab6-dc93-4470-ba2e-e811964545ea}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller AllowDropOperationCaller
        {
            get
            {
                if (AllowDropOperationCallMetaData == null || _AllowDropOperationCall == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_AllowDropOperationCaller != null)
                    return _AllowDropOperationCaller;
                _AllowDropOperationCaller = new OOAdvantech.UserInterface.Runtime.OperationCaller(_AllowDropOperationCall, this);
                return _AllowDropOperationCaller;
            }
        }

        /// <MetaDataID>{527c9772-a03d-4486-9561-a520a97aa086}</MetaDataID>
        XDocument AllowDropOperationCallMetaData;
        /// <MetaDataID>{c20a767f-8c75-4f43-8559-7c6882585bd6}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _AllowDropOperationCall;
        /// <MetaDataID>{4d2ff824-0de7-42a0-968c-561ad9e5d0db}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("DragDrop Behavior")]
        public object AllowDropOperationCall
        {
            get
            {
                if (AllowDropOperationCallMetaData == null)
                {
                    AllowDropOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", AllowDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _AllowDropOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }


                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (AllowDropOperationCaller == null || AllowDropOperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(AllowDropOperationCallMetaData.ToString() as string, "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(AllowDropOperationCallMetaData.ToString() as string, AllowDropOperationCaller.Operation.Name);

                metaDataVaue.MetaDataAsObject = _AllowDropOperationCall;
                return metaDataVaue;



            }
            set
            {
                _AllowDropOperationCaller = null;
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("AllowDropOperationCall", false).SetValue(this, AllowDropOperationCall);
                    return;
                }
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                if (metaDataVaue == null)
                    return;


                if (AllowDropOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    _AllowDropOperationCall = null;
                    AllowDropOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            AllowDropOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", AllowDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            AllowDropOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", AllowDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }


                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        AllowDropOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", AllowDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    try
                    {

                        OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                        foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                        {
                            _AllowDropOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                            break;
                        }
                        if (_AllowDropOperationCall == null)
                            _AllowDropOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                    }
                    catch (System.Exception error)
                    {
                        throw; 

                    }
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        _AllowDropOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }

        /// <MetaDataID>{388337d9-c55c-4c5e-ba0d-5bedd137a638}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DragDropOperationCaller;
        /// <MetaDataID>{70d479e7-f934-4c04-a02e-69b20f42e67c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.UserInterface.Runtime.OperationCaller DragDropOperationCaller
        {
            get
            {
                if (DragDropOperationCallMetaData == null || _DragDropOperationCall == null || UserInterfaceObjectConnection == null)
                    return null;
                if (_DragDropOperationCaller != null)
                    return _DragDropOperationCaller;
                _DragDropOperationCaller = new OOAdvantech.UserInterface.Runtime.OperationCaller(_DragDropOperationCall, this);
                return _DragDropOperationCaller;
            }
        }

        /// <MetaDataID>{49cf127b-abc7-49d2-ab03-ee66b2141683}</MetaDataID>
        XDocument DragDropOperationCallMetaData;
        /// <MetaDataID>{16d23c1e-9342-4dfe-8edd-983b88c05e80}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _DragDropOperationCall;
        /// <MetaDataID>{20024bc4-3f13-41f3-859f-f0054b9196b5}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("DragDrop Behavior")]
        public object DragDropOperationCall
        {
            get
            {
                if (DragDropOperationCallMetaData == null)
                {
                    DragDropOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", DragDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _DragDropOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }


                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (DragDropOperationCaller == null || DragDropOperationCaller.Operation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(DragDropOperationCallMetaData.ToString() as string, "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(DragDropOperationCallMetaData.ToString() as string, DragDropOperationCaller.Operation.Name);

                metaDataVaue.MetaDataAsObject = _DragDropOperationCall;
                return metaDataVaue;



            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("DragDropOperationCall", false).SetValue(this, DragDropOperationCall);
                    return;
                }
                _DragDropOperationCaller = null;
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                if (metaDataVaue == null)
                    return;


                if (DragDropOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    _DragDropOperationCall = null;
                    DragDropOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            DragDropOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", DragDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            DragDropOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", DragDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }



                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        DragDropOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", DragDropOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        _DragDropOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (_DragDropOperationCall == null)
                        _DragDropOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        _DragDropOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }

        /// <MetaDataID>{83f381b9-ed28-44e1-af2f-3510ea5c3735}</MetaDataID>
        void OnControlRemoved(object sender, ControlEventArgs e)
        {
            UnHookEventForDragAndDrop(e.Control);

        }
        /// <MetaDataID>{a2f74da2-38cd-47ee-b929-74eef207df44}</MetaDataID>
        static int mouseMoveCount = 0;

        /// <MetaDataID>{dead2169-7563-4af4-8cbb-0eca343ecc8e}</MetaDataID>
        void OnControlAdded(object sender, ControlEventArgs e)
        {
            HookEventForDragAndDrop(e.Control);

        }


        /// <MetaDataID>{53c5d79a-1a95-49b7-9017-d9423e618a4f}</MetaDataID>
        void ContainerControlVisibleChanged(object sender, EventArgs e)
        {
            if (_ContainerControl.Visible && HostForm == null)
            {
                HostForm = _ContainerControl.FindForm();
                if (HostForm != null)
                {
                    _ContainerControl.VisibleChanged -= new EventHandler(ContainerControlVisibleChanged);
                    HostForm.Closed += new EventHandler(ContainerControlClosed);
                    _ContainerControl.ParentChanged += new EventHandler(OnParentChanged);

                }

            }
            else
            {
                if (HostForm != null)
                {
                    HostForm.Closed -= new EventHandler(ContainerControlClosed);
                    HostForm = null;
                    _ContainerControl.VisibleChanged += new EventHandler(ContainerControlVisibleChanged);
                }


            }
        }

        /// <MetaDataID>{0bbbcf89-3a1f-4e7c-8308-03ed1473b26a}</MetaDataID>
        void OnParentChanged(object sender, EventArgs e)
        {
            if (_ContainerControl.Parent == null && HostForm != null)
            {
                HostForm.Closed -= new EventHandler(ContainerControlClosed);
                HostForm = null;
                _ContainerControl.VisibleChanged += new EventHandler(ContainerControlVisibleChanged);
            }

        }

        /// <MetaDataID>{5b6cfa63-b2cf-4658-81ad-a4953b66b304}</MetaDataID>
        void ContainerControlClosed(object sender, EventArgs e)
        {
            UnHookEventForDragAndDrop(_ContainerControl);
            HostForm.Closed -= new EventHandler(ContainerControlClosed);
            HostForm = null;

            foreach (Control control in new List<Control>(HookedForDragAndDropControls))
            {
                UnHookEventForDragAndDrop(control);
            }
            HookedForDragAndDropControls.Clear();

        }
        /// <MetaDataID>{ce5a5299-ab8e-4a07-9abd-6be97a070c98}</MetaDataID>
        System.Drawing.Point StartDragPoint = new System.Drawing.Point(-1, -1);
        /// <MetaDataID>{c9d8135c-d0be-4404-ac24-bca8bc366275}</MetaDataID>
        protected void ContainerControlMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && AllowDrag)
            {

                if (MasterViewControlObject != null && MasterViewControlObject.ContainerControl == ContainerControl)
                    return;
                if (StartDragPoint.X == -1 && StartDragPoint.Y == -1)
                {
                    StartDragPoint = e.Location;

                }
                else if (Instance != null)
                {
                    int xDestance = StartDragPoint.X - e.X;
                    if (xDestance < 0)
                        xDestance = -xDestance;
                    int yDestance = StartDragPoint.Y - e.Y;
                    if (yDestance < 0)
                        yDestance = -yDestance;

                    if ((yDestance > 10 || xDestance > 10) && Cursor.Current != Cursors.IBeam)
                    {
                        DragDropActionManager dragDropActionManager = new DragDropActionManager(this, Instance, DragDropTransactionOption);
                        ContainerControl.DoDragDrop(Instance, DragDropEffects.Copy);
                    }
                }
            }
            else
            {
                StartDragPoint.X = -1;
                StartDragPoint.Y = -1;
            }
        }
        #endregion




        /// <MetaDataID>{B54FA8CB-7A44-483B-B07D-FCC188AA1FF7}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Instance
        {
            get
            {
                return UserInterfaceObjectConnection.Instance;

            }
            set
            {
                UserInterfaceObjectConnection.Instance = value;
            }
        }


        /// <MetaDataID>{B28F69C0-01D2-4518-AE99-6593A5941775}</MetaDataID>
        public ViewControlObject(IContainer container)
        {
            UserInterfaceObjectConnection = new OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection(this);


            container.Add(this);
            InitializeComponent();
            _ConnectedObjectTypeMetaData = new ConnectedTypeMetaData(this);

        }
        /// <MetaDataID>{fe44ae02-062b-475a-9107-718e81ab0245}</MetaDataID>
        System.ComponentModel.Design.IDesignerHost _DesignerHost = null;
        /// <MetaDataID>{75826f66-26ff-4c5b-8565-fb8bb9f8a7e7}</MetaDataID>
        bool OnRemove = false;
        /// <MetaDataID>{1D6CA470-D142-442D-90E5-857E78500A75}</MetaDataID>
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                if (DesignMode &&
                    value == null &&
                    base.Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    !(base.Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                {
                    try
                    {
                        OnRemove = true;
                        UserInterfaceObjectConnection.PresentationContextViewControlChange();
                    }
                    finally
                    {
                        OnRemove = false;
                    }
                }

                base.Site = value;

                if (DesignMode && base.Site != null)
                {
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
                    PropertyDescriptor property = properties.Find("ContainerControl", false);
                    property.SetValue(this, (base.Site.Container as System.ComponentModel.Design.IDesignerHost).RootComponent as System.Windows.Forms.Control);

                }
                if (DesignMode && UserInterfaceObjectConnection != null)
                    UserInterfaceObjectConnection.State = ViewControlObjectState.DesigneMode;
                if (DesignMode)
                {
                    if (value != null && value.Container is System.ComponentModel.Design.IDesignerHost)
                    {
                        
                        _DesignerHost = value.Container as System.ComponentModel.Design.IDesignerHost;
                        _DesignerHost.LoadComplete += new EventHandler(DesignerHostLoadComplete);
                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded += new System.ComponentModel.Design.ComponentEventHandler(ComponentAdded);

                    }
                    else
                    {
                        _DesignerHost.LoadComplete -= new EventHandler(DesignerHostLoadComplete);
                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded -= new System.ComponentModel.Design.ComponentEventHandler(ComponentAdded);
                        _DesignerHost = null;

                    }
                }
            }
        }

        /// <MetaDataID>{0ae7e79b-11ce-45d9-beca-2e9d3895848a}</MetaDataID>
        void ComponentAdded(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {
            if (DesignerHostLoadCompleted&&MasterViewControlObject==null)
            {
                PropertyDescriptor property = TypeDescriptor.GetProperties(e.Component).Find("ViewControlObject", false);
                if (property != null)
                    property.SetValue(e.Component, this);

            }
            
        }
        /// <MetaDataID>{5e63d5e6-e8b0-49c3-a7ce-47bc2305e6cc}</MetaDataID>
        bool DesignerHostLoadCompleted = false;
        /// <MetaDataID>{5b1a4192-34b9-4f4a-b94a-39626b94b980}</MetaDataID>
        void DesignerHostLoadComplete(object sender, EventArgs e)
        {
            DesignerHostLoadCompleted = true;
        }



        /// <MetaDataID>{6AF54161-D8C0-4616-91ED-0C0AE7CC89BD}</MetaDataID>
        public ViewControlObject()
        {
            UserInterfaceObjectConnection = new OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection(this);
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            InitializeComponent();
            _ConnectedObjectTypeMetaData = new ConnectedTypeMetaData(this);

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

        #region Component Designer generated code
        /// <summary>Required method for Designer support - do not modify
        /// the contents of this method with the code editor.</summary>
        /// <MetaDataID>{347B0166-03CA-47D4-AED1-ACBE5E04B5B1}</MetaDataID>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region IPresentationContextViewControl Members

        /// <MetaDataID>{2b2c9b9f-7458-4b54-aea6-a91ea8c6650b}</MetaDataID>
        bool OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl.InvokeRequired
        {
            get
            {
                return ContainerControl.FindForm().InvokeRequired;
            }
        }

        /// <MetaDataID>{4c88bd88-480b-4be7-a14f-21cb0ddedb2a}</MetaDataID>
        object OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl.SynchroInvoke(Delegate method, params object[] args)
        {
            if (!ContainerControl.FindForm().IsDisposed && ContainerControl.FindForm().IsHandleCreated)
            {
                try
                {
                    return ContainerControl.FindForm().Invoke(method, args);
                }
                catch (Exception error)
                {
                }
            }
            return null;
        }

        /// <MetaDataID>{5ef72cd0-ae1e-4f62-8ebb-c5bc43292f63}</MetaDataID>
        string OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl.Name
        {
            get
            {
                return Name;
            }
        }

        /// <MetaDataID>{75a2c8f7-b6da-4dc7-a714-94c5be6daacd}</MetaDataID>
        string OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl.HostControlName
        {
            get
            {
                return ContainerControl.Name;
            }
        }

        /// <MetaDataID>{3c1b1928-60c5-44bf-8b9e-9ff283453f79}</MetaDataID>
        object OOAdvantech.UserInterface.Runtime.IPresentationContextViewControl.ContainerControl
        {
            get
            {
                return this.ContainerControl;
            }
        }

        #endregion

        #region IPresentationContextViewControl Members


        /// <MetaDataID>{3aeb90d4-824b-47d0-b122-efbfa3614fce}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ContainerControlType
        {
            get
            {
                if (DesignMode)
                    return UserInterfaceObjectConnection.GetClassifier((Site.Container as System.ComponentModel.Design.IDesignerHost).RootComponentClassName, true);
                else
                    return UserInterfaceObjectConnection.GetClassifier(ContainerControl.GetType());

            }
        }

        #endregion

        #region IOperetionCallerSource Members

        /// <MetaDataID>{91a930c3-577a-4281-b133-baa1fbf98da7}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                return new string[2] { "Value", "DragDropObject" };
            }
        }
        /// <MetaDataID>{abf1eee2-1214-4ac2-a9c0-169f54f36aa0}</MetaDataID>
        object DragDropObject;

        /// <MetaDataID>{158c8512-0cac-4f33-bdbe-32f6f02f2e0b}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "Value")
                return Instance;
            if (propertyName == "DragDropObject")
                return DragDropObject;


            return null;

        }

        /// <MetaDataID>{4f860f0e-ab9a-4064-ae75-2165367034a9}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Instance = value;

        }

        /// <MetaDataID>{e68ba6c2-5c4e-4e5c-9d9f-efacbd47ed46}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "Value")
                return UserInterfaceObjectConnection.ObjectType;
            else
                return null;


        }

        /// <MetaDataID>{43e428cf-5b79-43b8-83b8-8a8c7c85f11f}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "Value")
                return true;
            if (propertyName == "DragDropObject")
                return true;
            if (propertyName == "DragDropObject")
                return true;


            return false;
        }


        /// <MetaDataID>{5a812c02-aa57-4121-828b-54a346e425f4}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IConnectableControl.UserInterfaceObjectConnection
        {
            get
            {
                return UserInterfaceObjectConnection;
            }
            set
            {
            }
        }

        /// <MetaDataID>{0c9437ea-8160-4e0b-8383-973b4dee7921}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.ICutCopyPasteMoveDragDrop.UserInterfaceObjectConnection
        {
            get
            {
                return UserInterfaceObjectConnection;
            }

        }



        #endregion

        #region IConnectableControl Members

        /// <MetaDataID>{49a13af7-6a09-4abe-9e13-d7a572319ace}</MetaDataID>
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

        /// <MetaDataID>{78676bd3-d0d0-4c11-8ec2-b0b5da4784e1}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if ((metaObject is OOAdvantech.UserInterface.OperationCall) &&
                (propertyDescriptor == "AllowDropOperationCall" || propertyDescriptor == "DragDropOperationCall"))
            {

                if (new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                    return true;
                else
                    return false;


                //if (propertyDescriptor == "AllowDropOperationCall")
                //{
                //    if (operation.ReturnType == null || operation.ReturnType.FullName != typeof(OOAdvantech.DragDropMethod).FullName)
                //        return false;
                //    if (operation.Parameters.Count != 1)
                //        return false;
                //    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                //    {
                //        if (parameter.Type == null || parameter.Type.FullName != typeof(object).FullName)
                //            return false;
                //        else
                //            return true;
                //    }
                //    return false;
                //}
                //return false;
            }
            return false;
        }

        /// <MetaDataID>{4730a63b-cae2-495c-be03-10921727dd93}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }

        #endregion

        #region IMetadataSelectionResolver Members


        /// <MetaDataID>{1e2c8c20-c4ec-4448-bf35-cbae9b056564}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    /// <MetaDataID>{ab615123-c0d2-4e31-af74-e6338a3cf305}</MetaDataID>
    public class ConnectedTypeMetaData : System.ComponentModel.Component
    {
        internal IConnectableControl ConnectableControl;
        public ConnectedTypeMetaData(IConnectableControl connectableControl)
        {
            ConnectableControl = connectableControl;
        }


        string _TypeAssembly;
        [Browsable(false)]
        public string TypeAssembly
        {
            get
            {
                return _TypeAssembly;
            }
            set
            {
                _TypeAssembly = value;
                ConnectableControl.UserInterfaceObjectConnection.AssemblyMetadata = value;
            }
        }




        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object TypeMetaData
        {
            get
            {
                if (ConnectableControl.UserInterfaceObjectConnection.ObjectType != null)
                    return ConnectableControl.UserInterfaceObjectConnection.ObjectType.FullName;
                return ConnectableControl.UserInterfaceObjectConnection.ViewObjectTypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Classifier)
                {
                    ConnectableControl.UserInterfaceObjectConnection.ViewObjectTypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;
                    _TypeAssembly = (value as OOAdvantech.MetaDataRepository.Classifier).ImplementationUnit.Name;
                    ConnectableControl.UserInterfaceObjectConnection.AssemblyMetadata = _TypeAssembly;
                    TypeDescriptor.GetProperties(this).Find("TypeAssembly", false).SetValue(this, _TypeAssembly);
                }
                if (ConnectableControl.UserInterfaceObjectConnection.ObjectType != null)
                {
                    ConnectableControl.UserInterfaceObjectConnection.ViewObjectTypeFullName = ConnectableControl.UserInterfaceObjectConnection.ObjectType.FullName;
                    _TypeAssembly = ConnectableControl.UserInterfaceObjectConnection.ObjectType.ImplementationUnit.Name;
                    TypeDescriptor.GetProperties(this).Find("TypeAssembly", false).SetValue(this, _TypeAssembly);
                }
                if (value is string)
                    ConnectableControl.UserInterfaceObjectConnection.ViewObjectTypeFullName = value as string;
            }
        }
    }


    /// <MetaDataID>{d515af14-4061-435c-af18-467ea87d6505}</MetaDataID>
    public enum UpdateStyle
    {
        OnSaveControlsValue,
        Immediately,
        OnLostFocus
    }
}
