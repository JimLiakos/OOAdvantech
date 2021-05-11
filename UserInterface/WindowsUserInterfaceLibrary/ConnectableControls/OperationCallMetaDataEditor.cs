using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ConnectableControls.PropertyEditors;

using OOAdvantech.MetaDataRepository;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
//using OOAdvantech.Transactions;


namespace ConnectableControls.PropertyEditors 
{
    /// <MetaDataID>{B957397A-8878-4F78-A69E-ABA5CD30D214}</MetaDataID>
    public partial class OperationCallMetaDataEditor : System.Windows.Forms.Form, OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver
    {
        OOAdvantech.Transactions.TransactionOption[] TransactionOptions;
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            throw new NotImplementedException();
        }

        #region IMetadataSelectionResolver Members

        public bool CanItAccept(MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Operation" && metaObject is OOAdvantech.MetaDataRepository.BehavioralFeature)
                return true;
            else
                return false;

            
        }

        #endregion
 
        string OperationParameterLabel = "Operation Parameters";
        /// <MetaDataID>{9A570B4E-F18C-4D68-A35F-B70C99A8F7F9}</MetaDataID>
        Dictionary<Parameter, CustomProperty> ParameterProperties = new Dictionary<Parameter, CustomProperty>();
        /// <MetaDataID>{05EAE018-94FA-4D0B-AEDD-F85F2C65AA3F}</MetaDataID>
        System.ComponentModel.ITypeDescriptorContext Context;
        /// <MetaDataID>{B7C887EC-6B57-4CDB-845A-804109E302E0}</MetaDataID>
        CustomProperty SearchOperationTypeProperty;
        /// <MetaDataID>{85392692-6F55-46CB-B79F-5D10733BD55D}</MetaDataID>
        CustomProperty SearchOperationProperty;
        /// <MetaDataID>{4BB2C2C1-77F3-4650-BAD1-482DD08835DE}</MetaDataID>
        CustomProperty SearchOperationReturnProperty;

        CustomProperty OperationTransactionOptionProperty;

        CustomProperty ViewControlProperty;
        CustomProperty ObjectMemberControlProperty;
        System.Collections.Generic.List<string> RefreshedParameters = new List<string>();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {

            OperationCall.CallType = (OOAdvantech.UserInterface.CallType)SearchOperationTypeProperty.Value;
            OperationCall.TransactionOption = (OOAdvantech.Transactions.TransactionOption)OperationTransactionOptionProperty.Value;
            OperationCall.OperationPath = SearchOperationProperty.Value.ToString();

            if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall &&
                ViewControlProperty != null &&
                ViewControlProperty.Value is OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection)
                OperationCall.CalledObjectPath = (ViewControlProperty.Value as OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection).Name;
            else if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.ControlDisplayObjectCall &&
                ObjectMemberControlProperty != null &&
                ObjectMemberControlProperty.Value is string)
                OperationCall.CalledObjectPath = ObjectMemberControlProperty.Value as string;
            else
                OperationCall.CalledObjectPath = "";



            if (Operation != null)
            {
                int i = 0;
                foreach (OOAdvantech.MetaDataRepository.Parameter parameter in Operation.Parameters)
                {
                    OOAdvantech.UserInterface.ParameterLoader parameterLoader = OperationCall.GetParameterLoaderAt(i);
                    if (parameterLoader == null)
                        parameterLoader = OperationCall.NewParameterLoader(parameter.Name);
                    else
                        parameterLoader.Name = parameter.Name;

                    parameterLoader.ParameterType = parameter.Type.FullName;
                    if (ParameterProperties[parameter].Value != null)
                        parameterLoader.Source = ParameterProperties[parameter].Value.ToString();
                    if (RefreshedParameters.Contains(parameter.Name))
                        parameterLoader.RefreshFrom = true;
                    else
                        parameterLoader.RefreshFrom = false;


                    i++;
                }
                while (i < OperationCall.ParameterLoaders.Count)
                    OperationCall.RemoveParameterLoader(OperationCall.GetParameterLoaderAt(i));
                if (SearchOperationReturnProperty == null)
                    OperationCall.ReturnValueDestination = null;
                else
                {
                    OperationCall.ReturnValueDestination = SearchOperationReturnProperty.Value as string;
                    if (RefreshedParameters.Contains(SearchOperationReturnProperty.Name))
                        OperationCall.RefreshFromForReturnObject = true;
                    else
                        OperationCall.RefreshFromForReturnObject = false;




                    if (OperationCaller.UIObjectConnection.GetControlWithName(OperationCall.ReturnValueDestination) is IObjectMemberViewControl &&
                       OperationCaller.UIObjectConnection.GetControlWithName(OperationCall.ReturnValueDestination) != (Context.Instance as Control))
                    {
                        if ((Context.Instance as IObjectMemberViewControl) != (OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[OperationCall.ReturnValueDestination] as Control)
                        {
                            if (Context.Instance is Control)
                                (OperationCaller.UIObjectConnection.GetControlWithName(OperationCall.ReturnValueDestination) as IPathDataDisplayer).Path = "Control: " + (Context.Instance as Control).Name;
                            if (Context.Instance is ConnectableControls.Menus.MenuCommand)
                                (OperationCaller.UIObjectConnection.GetControlWithName(OperationCall.ReturnValueDestination) as IPathDataDisplayer).Path = "Control: " + (Context.Instance as ConnectableControls.Menus.MenuCommand).OwnerControl.Name;

                        }
                    }
                }
            }
            if (OperationCaller != null)
            {
                OperationCaller.ReleaseEventHandlers();

            }
            if (OperationCall != null)
                OperationCall.Name = Context.PropertyDescriptor.Name;

            if (Operation==null||(Context.Instance as OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver).CanItAccept(OperationCall, Context.PropertyDescriptor.Name))
                e.Cancel = false;
            else
                e.Cancel = true;
            base.OnClosing(e);
        }
        /// <MetaDataID>{985D1670-43B0-4936-ADEE-B51EB8453680}</MetaDataID>
        protected override void OnClosed(EventArgs e)
        {
            //OperationCall.AssemblyFullName = SearchOperationAssemblyProperty.Value as string;
            try
            {
                OperationCall.CallType = (OOAdvantech.UserInterface.CallType)SearchOperationTypeProperty.Value;
                OperationCall.TransactionOption = (OOAdvantech.Transactions.TransactionOption)OperationTransactionOptionProperty.Value;
                OperationCall.OperationPath = SearchOperationProperty.Value.ToString();

                if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall &&
                    ViewControlProperty != null &&
                    ViewControlProperty.Value is OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection)
                    OperationCall.CalledObjectPath = (ViewControlProperty.Value as OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection).Name;
                else if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.ControlDisplayObjectCall &&
                    ObjectMemberControlProperty != null &&
                    ObjectMemberControlProperty.Value is string)
                    OperationCall.CalledObjectPath = ObjectMemberControlProperty.Value as string;
                else
                    OperationCall.CalledObjectPath = "";



                if (Operation != null)
                {
                    int i = 0;
                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in Operation.Parameters)
                    {
                        OOAdvantech.UserInterface.ParameterLoader parameterLoader = OperationCall.GetParameterLoaderAt(i);
                        if (parameterLoader == null)
                            parameterLoader = OperationCall.NewParameterLoader(parameter.Name);
                        else
                            parameterLoader.Name = parameter.Name;

                        parameterLoader.ParameterType = parameter.Type.FullName;
                        if (ParameterProperties[parameter].Value != null)
                            parameterLoader.Source = ParameterProperties[parameter].Value.ToString();
                        if (RefreshedParameters.Contains(parameter.Name))
                            parameterLoader.RefreshFrom = true;
                        else
                            parameterLoader.RefreshFrom = false;


                        i++;
                    }
                    while (i < OperationCall.ParameterLoaders.Count)
                        OperationCall.RemoveParameterLoader(OperationCall.GetParameterLoaderAt(i));
                    if (SearchOperationReturnProperty == null)
                        OperationCall.ReturnValueDestination = null;
                    else
                    {
                        OperationCall.ReturnValueDestination = SearchOperationReturnProperty.Value as string;
                        if (RefreshedParameters.Contains(SearchOperationReturnProperty.Name))
                            OperationCall.RefreshFromForReturnObject = true;
                        else
                            OperationCall.RefreshFromForReturnObject = false;




                        if ((OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[OperationCall.ReturnValueDestination] is IObjectMemberViewControl &&
                           (OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[OperationCall.ReturnValueDestination] != (Context.Instance as Control))
                        {
                            if ((Context.Instance as IObjectMemberViewControl) != (OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[OperationCall.ReturnValueDestination] as Control)
                            {
                                if (Context.Instance is Control)
                                    ((OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[OperationCall.ReturnValueDestination] as IPathDataDisplayer).Path = "Control: " + (Context.Instance as Control).Name;
                                if (Context.Instance is ConnectableControls.Menus.MenuCommand)
                                    ((OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[OperationCall.ReturnValueDestination] as IPathDataDisplayer).Path = "Control: " + (Context.Instance as ConnectableControls.Menus.MenuCommand).OwnerControl.Name;

                            }
                        }
                    }
                }
                if (OperationCaller != null)
                {
                    OperationCaller.ReleaseEventHandlers();

                }
                if (OperationCall != null)
                    OperationCall.Name = Context.PropertyDescriptor.Name;



                base.OnClosed(e);
            }
            catch (System.Exception error)
            {
                throw;
            }

        }
        public bool BrowseCode = false;
        /// <MetaDataID>{015BDCE8-D8B5-46F8-8188-D2B02023F4B0}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller OperationCaller;

        /// <MetaDataID>{27FB30FF-CDCA-4088-B30E-06E540F1D0EF}</MetaDataID>
        public OperationCallMetaDataEditor(System.ComponentModel.ITypeDescriptorContext context, OOAdvantech.UserInterface.OperationCall operationCall)
        {

            foreach (System.Attribute attribute in context.PropertyDescriptor.Attributes)
            {
                if (attribute is OOAdvantech.UserInterface.OperationCallTransactionOption)
                {
                    TransactionOptions = (attribute as OOAdvantech.UserInterface.OperationCallTransactionOption).TransactionOptions;
                    break;
                }
            }
            InitializeComponent();
            Context = context;
            OperationCall = operationCall;

            PropertyGridEx grid = PropertyGrid;
            PropertyGrid.Item.Instance = context.Instance;
            PropertyGrid.Item.PropertiesOwner =this ;
            PropertyGrid.Item.PropertyName = context.PropertyDescriptor.Name;
            PropertyGrid.Item.Add("Operation", "", false, "Operation", "The component accept custom UITypeEditor.", true);
            PropertyGrid.Item[PropertyGrid.Item.Count - 1].CustomEditor = new EditMetaData();
            SearchOperationProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
            PropertyGrid.Item.Add("Transaction Option", OOAdvantech.Transactions.TransactionOption.Supported, false, "Operation", "The component accept custom UITypeEditor.", true);

            OperationTransactionOptionProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1]; ;
    


            PropertyGrid.Item.Add("Operation Type", OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall, false, "Operation", "The component accept custom UITypeEditor.", true);

            SearchOperationTypeProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
            PropertyGrid.ShowCustomProperties = true;
            PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);


            try
            {

                SearchOperationTypeProperty.Value = operationCall.CallType;
                SearchOperationProperty.Value = operationCall.OperationPath;
                OperationTransactionOptionProperty.Value = operationCall.TransactionOption;
                if (operationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)
                {

                    PropertyGrid.Item.Add("ViewControl components", "", false, "Operation", "The component accept custom UITypeEditor.", true);
                    ViewControlProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
                    PropertyGrid.ShowCustomProperties = true;
                    ViewControlProperty.CustomEditor = new ViewControlObjectSelector();
                    if (!string.IsNullOrEmpty(operationCall.CalledObjectPath))
                    {
                        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection viewControlObject = null;
                        if (Context.Instance is OOAdvantech.UserInterface.Runtime.IOperationCallerSource)
                            viewControlObject = (Context.Instance as OOAdvantech.UserInterface.Runtime.IOperationCallerSource).UserInterfaceObjectConnection;
                        if (Context.Instance is Menus.MenuCommand && (Context.Instance as Menus.MenuCommand).OwnerControl != null)
                            viewControlObject = (Context.Instance as Menus.MenuCommand).OwnerControl.UserInterfaceObjectConnection;
                        if (viewControlObject != null)
                        {
                            foreach (OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection item in viewControlObject.GetHostContolViewControlObjects())
                            {
                                if (item.Name == operationCall.CalledObjectPath)
                                {
                                    ViewControlProperty.Value = item;
                                    break;
                                }
                            }

                        }

                    }

                }
                if (operationCall.CallType == OOAdvantech.UserInterface.CallType.ControlDisplayObjectCall)
                {
                    PropertyGrid.Item.Add("Controls", "", false, "Operation", "The component accept custom UITypeEditor.", true);
                    ObjectMemberControlProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
                    PropertyGrid.ShowCustomProperties = true;
                    ObjectMemberControlProperty.CustomEditor = new FormControlsSelector();
                    ObjectMemberControlProperty.Value = operationCall.CalledObjectPath;


                }


                string[] parametersTypes = new string[operationCall.ParameterLoaders.Count];
                int i = 0;
                foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in operationCall.ParameterLoaders)
                    parametersTypes[i++] = parameterLoader.ParameterType;


                //Operation =OperationCaller.GetOperation(SearchOperationProperty.Value as string, parametersTypes);
                if (Context.Instance is OOAdvantech.UserInterface.Runtime.IOperationCallerSource)
                    OperationCaller = new OperationCaller(operationCall, (Context.Instance as OOAdvantech.UserInterface.Runtime.IOperationCallerSource));
                if (Context.Instance is Menus.MenuCommand)
                    OperationCaller = new OperationCaller(operationCall, (Context.Instance as Menus.MenuCommand).OwnerControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource);
                if (OperationCaller != null)
                    Operation = OperationCaller.Operation;

                if (Operation != null)
                {
                    i = 0;
                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in Operation.Parameters)
                    {
                        PropertyGrid.Item.Add(parameter.Name, "", false, OperationParameterLabel, "The component accept custom UITypeEditor.", true);
                        PropertyGrid.Item[PropertyGrid.Item.Count - 1].CustomEditor = new FormControlsSelector();
                        ParameterProperties[parameter] = PropertyGrid.Item[PropertyGrid.Item.Count - 1];

                        OOAdvantech.UserInterface.ParameterLoader parameterLoader = operationCall.GetParameterLoaderAt(i++);
                        parameterLoader.Name = parameter.Name;
                        parameterLoader.ParameterType = parameter.Type.FullName;
                        PropertyGrid.Item[PropertyGrid.Item.Count - 1].Value = parameterLoader.Source;
                        if (parameterLoader.RefreshFrom)
                            if (!RefreshedParameters.Contains(parameter.Name))
                                RefreshedParameters.Add(parameter.Name);


                    }
                }
                if (Operation != null && Operation.ReturnType != null && Operation.ReturnType.Name.ToLower() != "void")
                {
                    PropertyGrid.Item.Add("Return Value", "", false, OperationParameterLabel, "The component accept custom UITypeEditor.", true);
                    PropertyGrid.Item[PropertyGrid.Item.Count - 1].CustomEditor = new FormControlsSelector();
                    SearchOperationReturnProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
                    SearchOperationReturnProperty.Value = operationCall.ReturnValueDestination;
                    if (operationCall.RefreshFromForReturnObject)
                        if (!RefreshedParameters.Contains(SearchOperationReturnProperty.Name))
                            RefreshedParameters.Add(SearchOperationReturnProperty.Name);



                }

                if (TransactionOptions != null)
                {
                    bool validValue = false;
                    foreach (OOAdvantech.Transactions.TransactionOption transactionOption in TransactionOptions)
                    {
                        if (transactionOption == (OOAdvantech.Transactions.TransactionOption)OperationTransactionOptionProperty.Value)
                        {
                            validValue = true;
                            break;
                        }
                    }
                    if (!validValue)
                        OperationTransactionOptionProperty.Value = TransactionOptions[0];
                }

            }
            catch (System.Exception error)
            {
            }





        }

        /// <MetaDataID>{07F93D70-81F7-4795-A3F9-8EDB89A50FFB}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Operation Operation;
        OOAdvantech.UserInterface.OperationCall OperationCall;

        /// <MetaDataID>{EAE55716-27E6-46C5-A209-F59ECEFF38F2}</MetaDataID>
        void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                if (SearchOperationProperty != null && e.ChangedItem.Label == SearchOperationProperty.Name)
                {

                    RefreshedParameters.Clear();
                    Operation = (SearchOperationProperty.Value as MetaData).MetaObject as OOAdvantech.MetaDataRepository.Operation;

                    for (int i = 0; i < PropertyGrid.Item.Count; i++)
                    {
                        if (PropertyGrid.Item[i].Category == OperationParameterLabel)
                        {
                            PropertyGrid.Item.RemoveAt(i);
                            i--;
                        }
                    }
                    if (Operation == null)
                    {
                        SearchOperationProperty.Value = "";
                        PropertyGrid.Refresh();
                        return;
                    }
                    if (ViewControlProperty != null)
                        SearchOperationProperty.Value = (SearchOperationProperty.Value as MetaData).Path;
                    else if (ObjectMemberControlProperty != null)
                        SearchOperationProperty.Value = (SearchOperationProperty.Value as MetaData).Path;
                    else if ((OOAdvantech.UserInterface.CallType)SearchOperationTypeProperty.Value == OOAdvantech.UserInterface.CallType.HostingFormOperationCall)
                        SearchOperationProperty.Value = (SearchOperationProperty.Value as MetaData).Path;
                    else
                        SearchOperationProperty.Value = Operation.FullName;

                    ParameterProperties.Clear();
                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in Operation.Parameters)
                    {
                        PropertyGrid.Item.Add(parameter.Name, "", false, OperationParameterLabel, "The component accept custom UITypeEditor.", true);
                        //PropertyGrid.Item[PropertyGrid.Item.Count - 1].CustomEditor =new UIListboxEditor();// new FormControlsSelector(Context.Container.Components); 

                        PropertyGrid.Item[PropertyGrid.Item.Count - 1].CustomEditor = new FormControlsSelector();
                        ParameterProperties[parameter] = PropertyGrid.Item[PropertyGrid.Item.Count - 1];



                    }
                    if (Operation != null && Operation.ReturnType != null && Operation.ReturnType.Name.ToLower() != "void")
                    {
                        PropertyGrid.Item.Add("Return Value", "", false, OperationParameterLabel, "The component accept custom UITypeEditor.", true);
                        PropertyGrid.Item[PropertyGrid.Item.Count - 1].CustomEditor = new FormControlsSelector();
                        SearchOperationReturnProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];

                    }
                    PropertyGrid.Refresh();
                }
                if (e.ChangedItem.PropertyDescriptor.Category == OperationParameterLabel && e.ChangedItem.Value is Control)
                {
                    ((e.ChangedItem.PropertyDescriptor as CustomProperty.CustomPropertyDescriptor).CustomProperty as CustomProperty).Value = (e.ChangedItem.Value as Control).Name;

                }
                if (SearchOperationReturnProperty != null && e.ChangedItem.Label == SearchOperationReturnProperty.Name)
                {


                    if ((OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[e.ChangedItem.Value as string] is IObjectMemberViewControl &&
                        (OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[e.ChangedItem.Value as string] != (Context.Instance as Control))
                    {
                        if ((Context.Instance as IObjectMemberViewControl) != (OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[e.ChangedItem.Value as string] as Control)
                        {
                            if (Context.Instance is Control)
                                ((OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[e.ChangedItem.Value as string] as IPathDataDisplayer).Path = "Control: " + (Context.Instance as Control).Name;
                            if (Context.Instance is ConnectableControls.Menus.MenuCommand)
                                ((OperationCaller.UIObjectConnection.PresentationContextViewControl as ConnectableControls.ViewControlObject).ContainerControl.FindForm().Container.Components[e.ChangedItem.Value as string] as IPathDataDisplayer).Path = "Control: " + (Context.Instance as ConnectableControls.Menus.MenuCommand).OwnerControl.Name;

                        }
                    }

                }

                if (SearchOperationTypeProperty != null && e.ChangedItem.Label == SearchOperationTypeProperty.Name)
                {
                    if ((OOAdvantech.UserInterface.CallType)SearchOperationTypeProperty.Value == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)
                    {
                        if (ViewControlProperty == null)
                        {
                            if (ObjectMemberControlProperty != null)
                            {
                                PropertyGrid.Item.Remove(ObjectMemberControlProperty.Name);
                                ViewControlProperty = null;
                            }
                            SearchOperationProperty.Value = "";
                            PropertyGrid.Refresh();
                            RefreshedParameters.Clear();

                            PropertyGrid.Item.Add("ViewControl components", "", false, "Operation", "The component accept custom UITypeEditor.", true);
                            ViewControlProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
                            PropertyGrid.ShowCustomProperties = true;
                            ViewControlProperty.CustomEditor = new ViewControlObjectSelector();
                            SearchOperationProperty.Value = "";
                            PropertyGrid.Refresh();

                        }
                    }
                    else if ((OOAdvantech.UserInterface.CallType)SearchOperationTypeProperty.Value == OOAdvantech.UserInterface.CallType.ControlDisplayObjectCall)
                    {
                        if (ViewControlProperty != null)
                        {
                            PropertyGrid.Item.Remove(ViewControlProperty.Name);
                            ViewControlProperty = null;
                        }
                        SearchOperationProperty.Value = "";
                        PropertyGrid.Refresh();

                        PropertyGrid.Item.Add("Controls", "", false, "Operation", "The component accept custom UITypeEditor.", true);
                        ObjectMemberControlProperty = PropertyGrid.Item[PropertyGrid.Item.Count - 1];
                        PropertyGrid.ShowCustomProperties = true;
                        ObjectMemberControlProperty.CustomEditor = new FormControlsSelector();
                        SearchOperationProperty.Value = "";
                        PropertyGrid.Refresh();


                    }
                    else if (ViewControlProperty != null)
                    {
                        PropertyGrid.Item.Remove(ViewControlProperty.Name);
                        ViewControlProperty = null;
                        ObjectMemberControlProperty = null;

                        SearchOperationProperty.Value = "";
                        PropertyGrid.Refresh();
                    }
                }

                if (OperationTransactionOptionProperty != null && e.ChangedItem.Label == OperationTransactionOptionProperty.Name)
                {
                    if (TransactionOptions != null)
                    {
                        bool validValue=false;
                        foreach (OOAdvantech.Transactions.TransactionOption transactionOption in TransactionOptions)
                        {
                            if (transactionOption == (OOAdvantech.Transactions.TransactionOption)OperationTransactionOptionProperty.Value)
                            {
                                validValue = true;
                                break;
                            }
                        }
                        if (!validValue)
                            OperationTransactionOptionProperty.Value = e.OldValue; 

                    }

                }

            }
            catch (Exception error)
            {
                throw;
            }


        }

        private void PropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (e.NewSelection.Parent.Label == OperationParameterLabel)
            {
                RefreshPropertyValue.Visible = true;


                if (RefreshedParameters.Contains(e.NewSelection.PropertyDescriptor.Name))
                    RefreshPropertyValue.Checked = true;
                else
                    RefreshPropertyValue.Checked = false;


            }
            else
                RefreshPropertyValue.Visible = false;


        }

        private void RefreshPropertyValue_CheckedChanged(object sender, EventArgs e)
        {
            if (RefreshPropertyValue.Checked && !RefreshedParameters.Contains(PropertyGrid.SelectedGridItem.PropertyDescriptor.Name))
                RefreshedParameters.Add(PropertyGrid.SelectedGridItem.PropertyDescriptor.Name);
            else if (!RefreshPropertyValue.Checked && RefreshedParameters.Contains(PropertyGrid.SelectedGridItem.PropertyDescriptor.Name))
                RefreshedParameters.Remove(PropertyGrid.SelectedGridItem.PropertyDescriptor.Name);

        }

        private void BrowseCode_Click(object sender, EventArgs e)
        {
            BrowseCode = true;
            Close();

        }



    }
}