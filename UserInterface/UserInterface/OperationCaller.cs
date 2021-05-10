using System;
using System.Collections.Generic;
using System.Text;


namespace OOAdvantech.UserInterface.Runtime
{

    /// <MetaDataID>{0623ab33-5187-4fc0-88cf-4644d6b6119b}</MetaDataID>
    public class OperationCaller
    {

        /// <MetaDataID>{d638c7cc-8a3e-4b99-adbc-3f3ba1efedb2}</MetaDataID>
        public readonly OperationCall OperationCall;
        /// <MetaDataID>{e0b5d86a-027d-4d7f-90a5-1b8f43363f09}</MetaDataID>
        public readonly UserInterfaceObjectConnection UIObjectConnection;
        /// <MetaDataID>{642d66f9-2c2f-4bae-9433-05c9b93db138}</MetaDataID>
        public readonly IOperationCallerSource OwnerMemberViewControl;
        /// <exclude>Excluded</exclude>
        OOAdvantech.MetaDataRepository.Operation _Operation;
        /// <MetaDataID>{e946330c-737f-43f2-9e34-53c9bba734bc}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Operation Operation
        {
            get
            {

                if (OperationCall == null)
                    return null;

                //TODO αν αλλάξει το όνομα τις operation απο code metadata repository 
                //δεν αλάζει το operation path στο operation call object


                if (_Operation != null)
                    return _Operation;

                _Operation = GetOperation();
                return _Operation;
            }
        }

        /// <MetaDataID>{fda17bb1-daa8-41ae-bdaf-e196eafff6f1}</MetaDataID>
        public System.Collections.Generic.List<string> CheckConnectionMetaData()
        {
            System.Collections.Generic.List<string> errorsList = new List<string>();

            if (!string.IsNullOrEmpty(OperationCall.OperationPath) && Operation == null)
            {
                errorsList.Add("Operation caller can't find operation '" + OperationCall.OperationPath + "'");


                if (OperationCall == null)
                    return errorsList;

                string operationName = OperationCall.OperationPath;
                string[] parametersTypes = new string[OperationCall.ParameterLoaders.Count];
                int i = 0;
                foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in OperationCall.ParameterLoaders)
                    parametersTypes[i++] = parameterLoader.ParameterType;

                List<string> parameterErrors;
                GetOperation(out parameterErrors);

                if (parameterErrors != null && parameterErrors.Count > 0)
                {
                    errorsList[0] = errorsList[0] + " (";
                    foreach (string error in parameterErrors)
                        errorsList[0] = errorsList[0] + "  " + error;
                    errorsList[0] = errorsList[0] + ") ";
                }
            }
            else
            {
                if (Operation != null)
                {

                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in Operation.Parameters)
                    {

                        Type type = parameter.Type.GetExtensionMetaObject(typeof(Type)) as Type;
                        Type sourceType = GetParameterSourceType(parameter.Name);
                        if (sourceType == null)
                        {
                            if (errorsList.Count == 0)
                                errorsList.Add("(source/return destination control) type mismatch on prameters (");
                            errorsList[0] += parameter.Name;
                        }
                        else if (!MetaDataRepository.Classifier.GetClassifier(sourceType).IsA(MetaDataRepository.Classifier.GetClassifier(type)) && type != sourceType)
                        {
                            if (errorsList.Count == 0)
                                errorsList.Add("(source/return destination control) type mismatch on prameters (");
                            errorsList[0] += parameter.Name;
                        }
                    }
                    if (errorsList.Count > 0)
                        errorsList[0] += ")";



                    if (DestinationType != null && DestinationType != Operation.ReturnType.GetExtensionMetaObject(typeof(Type)))
                    {
                        //if(ReturnDestinationControl!=null)
                        //    ReturnDestinationControl.CanItAccept(

                        //  OwnerMemberViewControl.CanDisplayDataOfType(DestinationType);
                    }
                }
            }


            return errorsList;

        }
        /// <MetaDataID>{989a0ff3-9aef-4edb-908b-aa7b15f93044}</MetaDataID>
        public void ReleaseEventHandlers()
        {
            if (UIObjectConnection.MasterViewControlObject == null)
                UIObjectConnection.ComponentNameChanged -= new ComponentNameChangedEventHandler(ComponentChangedName);
            else
                UIObjectConnection.MasterViewControlObject.ComponentNameChanged -= new ComponentNameChangedEventHandler(ComponentChangedName);



        }
        /// <MetaDataID>{25fabdd7-8897-4ee5-9e01-2e3b33539360}</MetaDataID>
        UserInterface.MenuCommand MenuCommand;
        /// <MetaDataID>{6f1ee861-ce30-4f84-82e4-227fed989f40}</MetaDataID>
        public OperationCaller(OperationCall operationCall, IOperationCallerSource ownerMemberViewControl, UserInterface.MenuCommand menuCommand)
            : this(operationCall, ownerMemberViewControl)
        {
            MenuCommand = menuCommand;
        }
        /// <MetaDataID>{16ece605-d497-4fbe-8a91-505cc1ab23d5}</MetaDataID>
        public OperationCaller(OperationCall operationCall, IOperationCallerSource ownerMemberViewControl)
        {
            try
            {
                OperationCall = operationCall;
                if (OperationCall == null)
                    throw new System.ArgumentNullException("operationCall");

                OwnerMemberViewControl = ownerMemberViewControl;
                if (OwnerMemberViewControl == null)
                    throw new System.ArgumentNullException("ownerMemberViewControl");

                UIObjectConnection = OwnerMemberViewControl.UserInterfaceObjectConnection;
                if (UIObjectConnection == null)
                    throw new System.ArgumentNullException("ownerMemberViewControl.ViewControlObject");
                if (operationCall.CalledObjectPath != null && UIObjectConnection.Name != operationCall.CalledObjectPath)
                {
                    foreach (UserInterfaceObjectConnection item in UIObjectConnection.GetHostContolViewControlObjects())
                    {
                        if (item.Name == operationCall.CalledObjectPath && operationCall.CalledObjectPath != null)
                        {
                            UIObjectConnection = item;
                            break;
                        }
                    }
                }

                if (UIObjectConnection.MasterViewControlObject == null)
                    UIObjectConnection.ComponentNameChanged += new ComponentNameChangedEventHandler(ComponentChangedName);
                else
                    UIObjectConnection.MasterViewControlObject.ComponentNameChanged += new ComponentNameChangedEventHandler(ComponentChangedName);
            }
            catch (System.Exception error)
            {
                throw;
            }


        }

        /// <MetaDataID>{e02f17ae-85ca-40ad-b654-e55aec41dfb7}</MetaDataID>
        void ComponentChangedName(object sender, ComponentNameChangedEventArgs e)
        {
            try
            {
                if (OperationCall.ReturnValueDestination != null && (OperationCall.ReturnValueDestination == e.OldName || OperationCall.ReturnValueDestination.IndexOf(e.OldName + ".") == 0))
                    OperationCall.ReturnValueDestination = (e.NewName as string) + OperationCall.ReturnValueDestination.Substring((e.OldName as string).Length);
                foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in OperationCall.ParameterLoaders)
                {
                    if (parameterLoader.Source != null && (parameterLoader.Source == e.OldName || parameterLoader.Source.IndexOf(e.OldName + ".") == 0))
                        parameterLoader.Source = (e.NewName as string) + parameterLoader.Source.Substring((e.OldName as string).Length);

                }
            }
            catch (System.Exception error)
            {
                throw;
            }


        }



        /// <MetaDataID>{3123a2fc-3976-4753-afe4-550da71e4a81}</MetaDataID>
        public void ExecuteOperationCall()
        {
            SetReturnValue(Invoke());
        }
        //public object Invoke(object[] parameterValues)
        //{
        //    OOAdvantech.Transactions.TransactionOption transactionOption = OperationCall.TransactionOption;

        //    OOAdvantech.MetaDataRepository.Operation operation = Operation;
        //    if (operation == null)
        //        return null;
        //    System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;


        //    object returnValue = null;
        //    if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.StaticOperationCall)
        //        returnValue = ViewControlObject.Invoke(null, methodInfo, parameterValues, transactionOption);
        //    else
        //    {

        //        object instance = null;
        //        instance = GetInvokeMethodObject(methodInfo);
        //        if (instance != null)
        //            returnValue = ViewControlObject.Invoke(instance, methodInfo, parameterValues, transactionOption);
        //    }
        //    return returnValue;
        //}

        /// <summary>
        /// Gets all values from the source controls and call the operation which is defined
        /// </summary>
        /// <param name="inViewControlTransaction">
        /// If this parameter is true then the operation caller use a transaction scoop with the trnsactio of view control object.
        /// </param>
        /// <param name="transactionOption"></param>
        /// The operation caller begins a transaction scoop accorting to transactionOption parameter
        /// <returns>
        /// return the object which returned from operation call.
        /// </returns>
        /// <MetaDataID>{b9233bdf-eb67-4f9d-8184-88159d47c3d5}</MetaDataID>
        public object Invoke()
        {
            OOAdvantech.Transactions.TransactionOption transactionOption = OperationCall.TransactionOption;

            OOAdvantech.MetaDataRepository.Operation operation = Operation;
            if (operation == null)
                return null;
            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
            object[] parameterValues = new object[methodInfo.GetParameters().Length];

            int i = 0;
            foreach (System.Reflection.ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                parameterValues[i] = GetParameterValue(parameterInfo.Name);
                if (parameterValues[i] != null
                    && !(parameterInfo.ParameterType.IsInstanceOfType(parameterValues[i]))
                    && parameterValues[i] is IPresentationObject)
                {
                    object realObj = (parameterValues[i] as IPresentationObject).GetRealObject();
                    if (parameterInfo.ParameterType.IsInstanceOfType(realObj))
                        parameterValues[i] = realObj;
                }

                i++;
            }
            object returnValue = null;
            if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.StaticOperationCall)
                returnValue = UIObjectConnection.Invoke(null, methodInfo, parameterValues, transactionOption);
            else
            {

                object instance = null;
                instance = GetInvokeMethodObject(methodInfo);
                if (instance != null)
                    returnValue = UIObjectConnection.Invoke(instance, methodInfo, parameterValues, transactionOption);
            }
            i = 0;
            foreach (System.Reflection.ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                if (IsParameterForForRefresh(parameterInfo.Name))
                {
                    DisplayedValue displayedValue = UIObjectConnection.GetDisplayedValue(parameterValues[i]);
                    if (displayedValue != null && displayedValue.Value != null)
                        displayedValue.UpdateUserInterface();
                }
                i++;
            }
            if (OperationCall.RefreshFromForReturnObject && returnValue != null)
            {
                DisplayedValue displayedValue = UIObjectConnection.GetDisplayedValue(returnValue);
                if (displayedValue != null && displayedValue.Value != null)
                    displayedValue.UpdateUserInterface();
            }





            return returnValue;


        }

        /// <MetaDataID>{9505bce6-63df-47f1-b0d6-4bec1546805d}</MetaDataID>
        private object GetInvokeMethodObject(System.Reflection.MethodInfo methodInfo)
        {
            object instance = null;

            if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.HostingFormOperationCall)
            {
                if (string.IsNullOrEmpty(OperationOwnerPath))
                    instance = UIObjectConnection.ContainerControl;
                else
                    instance = UISession.GetValue(UIObjectConnection.ContainerControl, UIObjectConnection.ContainerControlType.GetExtensionMetaObject(typeof(Type)) as Type, OperationOwnerPath);
            }
            if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)
            {

                if (string.IsNullOrEmpty(OperationOwnerPath))
                    instance = UIObjectConnection.PresentationObject;
                else
                    instance = UIObjectConnection.GetDisplayedValue(OperationOwnerPath, UIObjectConnection.GetObjectChangeStateManager(OwnerMemberViewControl as IPathDataDisplayer));
                if (instance is DisplayedValue)
                    instance = (instance as DisplayedValue).Value;
                if (instance != null && (OwnerMemberViewControl is IPathDataDisplayer))
                {
                    DisplayedValue displayedValue = null;
                    if (!UIObjectConnection.UserInterfaceSession.TryGetDisplayedValue(instance, out displayedValue)) //If there isn't display value for object create one
                    {

                        displayedValue = new DisplayedValue(instance, UIObjectConnection.UserInterfaceSession);
                        //UISession.GetCurrentUserInterfaceSession.Add(instance, displayedValue);
                        Member member = new Member(methodInfo.Name, displayedValue, methodInfo.DeclaringType, methodInfo.ReturnType);
                        displayedValue.Members.Add(methodInfo.Name, member);
                        UIObjectConnection.GetObjectChangeStateManager((OwnerMemberViewControl as IPathDataDisplayer)).AddDataPathNode(member);
                    }
                    else
                    {
                        displayedValue = UIObjectConnection.UserInterfaceSession[instance];
                        Member member = null;
                        if (!displayedValue.Members.ContainsKey(methodInfo.Name))
                        {
                            member = new Member(methodInfo.Name, displayedValue, methodInfo.DeclaringType, methodInfo.ReturnType, true);
                            displayedValue.Members.Add(methodInfo.Name, member);
                        }
                        else
                            member = displayedValue.Members[methodInfo.Name];
                        UIObjectConnection.GetObjectChangeStateManager((OwnerMemberViewControl as IPathDataDisplayer)).AddDataPathNode(member);
                    }
                }
            }

            if (OperationCall.CallType == OOAdvantech.UserInterface.CallType.ControlDisplayObjectCall)
            {
                string controlName = OperationCall.CalledObjectPath.Substring(0, OperationCall.CalledObjectPath.IndexOf('.'));
                string propertyName = OperationCall.CalledObjectPath.Substring(OperationCall.CalledObjectPath.IndexOf('.') + 1);
                IOperationCallerSource control = UIObjectConnection.GetControlWithName(controlName) as IOperationCallerSource;
                if (controlName == "this")
                    control = OwnerMemberViewControl;


                bool returnValueAsCollection = false;
                if (string.IsNullOrEmpty(OperationOwnerPath))
                    instance = control.GetPropertyValue(propertyName);
                else
                    instance = UIObjectConnection.GetDisplayedValue(control.GetPropertyValue(propertyName), control.GetPropertyType(propertyName), OperationOwnerPath, OwnerMemberViewControl as IPathDataDisplayer, out returnValueAsCollection);

                if (instance != null && (OwnerMemberViewControl is IPathDataDisplayer))
                {
                    DisplayedValue displayedValue = null;
                    if (!UIObjectConnection.UserInterfaceSession.TryGetDisplayedValue(instance, out displayedValue)) //If there isn't display value for object create one
                    {
                        displayedValue = new DisplayedValue(instance, UIObjectConnection.UserInterfaceSession);
                        //UISession.GetCurrentUserInterfaceSession.Add(instance, displayedValue);
                        Member member = new Member(methodInfo.Name, displayedValue, methodInfo.DeclaringType, methodInfo.ReturnType, true);
                        displayedValue.Members.Add(methodInfo.Name, member);
                        UIObjectConnection.GetObjectChangeStateManager((OwnerMemberViewControl as IPathDataDisplayer)).AddDataPathNode(member);
                    }
                    else
                    {
                        displayedValue = UIObjectConnection.UserInterfaceSession[instance];
                        Member member = null;
                        if (!displayedValue.Members.ContainsKey(methodInfo.Name))
                        {
                            member = new Member(methodInfo.Name, displayedValue, methodInfo.DeclaringType, methodInfo.ReturnType);
                            displayedValue.Members.Add(methodInfo.Name, member);
                        }
                        else
                            member = displayedValue.Members[methodInfo.Name];
                        UIObjectConnection.GetObjectChangeStateManager((OwnerMemberViewControl as IPathDataDisplayer)).AddDataPathNode(member);
                    }
                }
            }
            return instance;
        }
        /// <MetaDataID>{60e8e92c-de4e-44c5-899e-6eba2a407b45}</MetaDataID>
        void SetReturnValue(object value)
        {
            string destination = OperationCall.ReturnValueDestination;

            if (string.IsNullOrEmpty(destination))
                return;
            if (destination == "this")
                throw new System.Exception("The destination must be (Control Name).Value");
            if (destination == "this.Value")
                OwnerMemberViewControl.SetPropertyValue("Value", value);
            if (destination.LastIndexOf(".Value") != -1 && destination.LastIndexOf(".Value") == destination.Length - ".Value".Length)
            {
                destination = destination.Substring(0, destination.LastIndexOf(".Value"));
                IObjectMemberViewControl memberViewControl = UIObjectConnection.GetControlWithName(destination) as IObjectMemberViewControl;
                if (memberViewControl == null)
                    throw new System.Exception("System  can't find destination control .");
                memberViewControl.SetPropertyValue("Value", value);
            }
            else
                throw new System.Exception("The destination must be (Control Name).Value");
        }
        /// <MetaDataID>{b4d6d52b-1597-4bf2-9c57-9dd81d2eb872}</MetaDataID>
        OOAdvantech.MetaDataRepository.Operation GetOperation()
        {
            List<string> errors;
            try
            {
                return GetOperation(out  errors);
            }
            catch (System.Exception error)
            {
                return null;
            }
        }
        /// <summary>
        /// Defines the path which use the ViewControlObject to reach to object of operation.
        /// It is useful when CallType is (ViewControlObjectOperationCall).
        /// </summary>
        /// <MetaDataID>{908eb36f-1a81-4306-8602-eeeceff92f41}</MetaDataID>
        string OperationOwnerPath;

        /// <MetaDataID>{cd395a4c-16c7-4663-a251-d8cf946369cc}</MetaDataID>
        OOAdvantech.MetaDataRepository.Operation GetOperation(out System.Collections.Generic.List<string> errors)
        {
            OperationOwnerPath = null;
            string operationPath = OperationCall.OperationPath;

            string[] parametersTypes = new string[OperationCall.ParameterLoaders.Count];
            int i = 0;
            foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in OperationCall.ParameterLoaders)
                parametersTypes[i++] = parameterLoader.ParameterType;


            errors = null;
            if (string.IsNullOrEmpty(operationPath))
                return null;

            OOAdvantech.MetaDataRepository.Classifier classifier = null;
            string operationName = null;

            #region Gets operation classifier, operation name and operation owner path
            if (OperationCall.CallType == CallType.StaticOperationCall)
            {
                string typeFullName = operationPath.Substring(0, operationPath.LastIndexOf("."));
                operationName = operationPath.Substring(operationPath.LastIndexOf(".") + 1);
                classifier = UISession.GetClassifier(typeFullName, "", true, OwnerMemberViewControl.UserInterfaceObjectConnection);
                OperationOwnerPath = null;
                if (operationPath.LastIndexOf(".") == -1)
                    operationName = operationPath;
                else
                {
                    OperationOwnerPath = operationPath.Substring(0, operationPath.LastIndexOf("."));
                    operationName = operationPath.Substring(operationPath.LastIndexOf(".") + 1);
                }

            }
            else if (OperationCall.CallType == CallType.ViewControlObjectOperationCall)
            {
                if (UIObjectConnection.PresentationObjectType == null)
                    return null;
                OperationOwnerPath = null;
                if (operationPath.LastIndexOf(".") == -1)
                    operationName = operationPath;
                else
                {
                    OperationOwnerPath = operationPath.Substring(0, operationPath.LastIndexOf("."));
                    operationName = operationPath.Substring(operationPath.LastIndexOf(".") + 1);
                }
                if (OperationOwnerPath == null)
                    classifier = UIObjectConnection.PresentationObjectType;
                else
                    classifier = UserInterfaceObjectConnection.GetClassifier(UIObjectConnection.PresentationObjectType, OperationOwnerPath);
            }
            if (OperationCall.CallType == CallType.HostingFormOperationCall)
            {

                OperationOwnerPath = null;
                if (operationPath.LastIndexOf(".") == -1)
                    operationName = operationPath;
                else
                {
                    OperationOwnerPath = operationPath.Substring(0, operationPath.LastIndexOf("."));
                    operationName = operationPath.Substring(operationPath.LastIndexOf(".") + 1);
                }
                if (OperationOwnerPath == null)
                    classifier = UIObjectConnection.ContainerControlType;
                else
                    classifier = UserInterfaceObjectConnection.GetClassifier(UIObjectConnection.ContainerControlType, OperationOwnerPath);
            }
            else if (OperationCall.CallType == CallType.ControlDisplayObjectCall)
            {
                if (!string.IsNullOrEmpty(OperationCall.CalledObjectPath))
                {
                    OperationOwnerPath = null;
                    if (operationPath.LastIndexOf(".") == -1)
                        operationName = operationPath;
                    else
                    {
                        OperationOwnerPath = operationPath.Substring(0, operationPath.LastIndexOf("."));
                        operationName = operationPath.Substring(operationPath.LastIndexOf(".") + 1);
                    }

                    string controlName = OperationCall.CalledObjectPath.Substring(0, OperationCall.CalledObjectPath.IndexOf('.'));
                    string propertyName = OperationCall.CalledObjectPath.Substring(OperationCall.CalledObjectPath.IndexOf('.') + 1);
                    IOperationCallerSource control = UIObjectConnection.GetControlWithName(controlName) as IOperationCallerSource;
                    if (controlName == "this")
                        control = OwnerMemberViewControl;

                    if (string.IsNullOrEmpty(OperationOwnerPath))
                        classifier = control.GetPropertyType(propertyName);
                    else
                        classifier = UserInterfaceObjectConnection.GetClassifier(control.GetPropertyType(propertyName), OperationOwnerPath);
                }


            }
            #endregion

            if (classifier == null)
                return null;

            #region Checks if operation owner is property. If it is, return property access operation.
            OOAdvantech.MetaDataRepository.MetaObject metaObject = null;
            if (OperationCall.CallType == CallType.ControlDisplayObjectCall)
            {
                string controlName = OperationCall.CalledObjectPath.Substring(0, OperationCall.CalledObjectPath.IndexOf('.'));
                string propertyName = OperationCall.CalledObjectPath.Substring(OperationCall.CalledObjectPath.IndexOf('.') + 1);
                IOperationCallerSource control = UIObjectConnection.GetControlWithName(controlName) as IOperationCallerSource;
                if (controlName == "this")
                    control = OwnerMemberViewControl;

                metaObject = UserInterfaceObjectConnection.GetMetaObject(control.GetPropertyType(propertyName), OperationOwnerPath);
            }
            else if (OperationCall.CallType == CallType.HostingFormOperationCall)
            {
                metaObject = UserInterfaceObjectConnection.GetMetaObject(UIObjectConnection.ContainerControlType, OperationOwnerPath);
                //string controlName = OperationCall.CalledObjectPath.Substring(0, OperationCall.CalledObjectPath.IndexOf('.'));
                //string propertyName = OperationCall.CalledObjectPath.Substring(OperationCall.CalledObjectPath.IndexOf('.') + 1);
                //IObjectMemberViewControl control = ViewControlObject.GetControlWithName(controlName) as IObjectMemberViewControl;
                // metaObject = classifier;
            }
            else if (OperationCall.CallType == CallType.StaticOperationCall)
            {
                string typeFullName = operationPath.Substring(0, operationPath.LastIndexOf("."));
                operationName = operationPath.Substring(operationPath.LastIndexOf(".") + 1);
                metaObject = UserInterfaceObjectConnection.GetMetaObject(classifier, OperationOwnerPath);
            }
            else
                metaObject = UserInterfaceObjectConnection.GetMetaObject(UIObjectConnection.PresentationObjectType, OperationOwnerPath);

            if (metaObject is OOAdvantech.MetaDataRepository.Attribute &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Getter != null &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Getter.Name == operationName)
            {
                OperationOwnerPath = GetPropertyOwnerPath(metaObject, OperationOwnerPath);
                return (metaObject as OOAdvantech.MetaDataRepository.Attribute).Getter;
            }
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Setter != null &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Setter.Name == operationName)
            {
                OperationOwnerPath = GetPropertyOwnerPath(metaObject, OperationOwnerPath);
                if (parametersTypes[0].Trim() != (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.FullName.Trim())
                {
                    errors.Add("property type mismatch");
                    return null;
                }
                else
                    return (metaObject as OOAdvantech.MetaDataRepository.Attribute).Setter;
            }
            if (metaObject is OOAdvantech.MetaDataRepository.AssociationEnd &&
                (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Getter != null &&
                (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Getter.Name == operationName)
            {
                OperationOwnerPath = GetPropertyOwnerPath(metaObject, OperationOwnerPath);
                return (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Getter;
            }

            if (metaObject is OOAdvantech.MetaDataRepository.AssociationEnd &&
                (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Setter != null &&
                (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Setter.Name == operationName)
            {
                OperationOwnerPath = GetPropertyOwnerPath(metaObject, OperationOwnerPath);
                if (parametersTypes[0].Trim() != (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Specification.FullName.Trim())
                {
                    errors.Add("property type mismatch");
                    return null;
                }
                else
                    return (metaObject as OOAdvantech.MetaDataRepository.AssociationEnd).Setter;
            }

            #endregion


            #region Search for operation with the signature, which OperationCall defines.
            foreach (OOAdvantech.MetaDataRepository.Operation operation in classifier.GetOperations(true))
            {
                if (operation.Name == operationName)
                {
                    i = 0;
                    bool equal = true;
                    if (operation.Parameters.Count == parametersTypes.Length)
                    {
                        if (errors == null)
                            errors = new List<string>();
                        else
                            errors.Clear();
                        foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                        {
                            if (parameter.Type.FullName != parametersTypes[i])
                            {
                                MetaDataRepository.Classifier parameterSourceType = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(GetSourceType(OperationCall.ParameterLoaders[i].Source));
                                if (!parameterSourceType.IsA(parameter.Type))
                                {
                                    errors.Add("type mismatch on parameter '" + parameter.Name + "'");
                                    equal = false;
                                }
                            }
                            i++;
                        }
                    }
                    else
                        equal = false;
                    if (equal)
                        return operation;
                }

            }
            #endregion

            return null;
        }
        /// <summary>
        /// Returns property owner path from property path. If property belongs to the ViewControlObject.ObjectType returns null. 
        /// </summary>
        /// <param name="metaObject">Attribute or AssociationEnd</param>
        /// <param name="operationOwnerPath">the property path</param>
        /// <returns>
        /// Path of property owner object
        /// </returns>
        /// <MetaDataID>{e882ef72-3d82-4592-8153-c61f8f3b698d}</MetaDataID>
        private string GetPropertyOwnerPath(OOAdvantech.MetaDataRepository.MetaObject metaObject, string operationOwnerPath)
        {

            int nPos = operationOwnerPath.LastIndexOf(metaObject.Name);
            if (nPos == 0)
            {
                operationOwnerPath = null;
            }
            else
            {
                operationOwnerPath = operationOwnerPath.Substring(0, nPos);
                if (operationOwnerPath[nPos - 1] == '.')
                    operationOwnerPath = operationOwnerPath.Substring(0, nPos - 1);
            }
            return operationOwnerPath;
        }

        /// <summary>
        /// Defines the type which can accept the destination control
        /// </summary>
        /// <MetaDataID>{43984414-2f76-4c41-bf52-0fa95a14ed78}</MetaDataID>
        Type DestinationType
        {
            get
            {
                if (OperationCall == null)
                    return null;
                return GetSourceType(OperationCall.ReturnValueDestination);
            }
        }
        /// <summary>
        /// Returns the type of value of source control which assigned to parameter 
        /// </summary>
        /// <param name="parameterName">
        /// Defines the name of parameter which the operation want find the assigned control as source  
        /// </param>
        /// <returns>
        /// the value type of source control 
        /// </returns>
        /// <MetaDataID>{4c3cb5a7-b132-47e4-aeac-77151ee3b055}</MetaDataID>
        Type GetParameterSourceType(string parameterName)
        {
            if (OperationCall == null)
                return null;
            foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in OperationCall.ParameterLoaders)
            {
                if (parameterLoader.Name == parameterName)
                {
                    string source = parameterLoader.Source;
                    if (!string.IsNullOrEmpty(source))
                    {
                        string lowerCaseSource = source.ToLower();
                        if (lowerCaseSource.IndexOf("value(") == 0 && lowerCaseSource[lowerCaseSource.Length - 1] == ')')
                        {
                            string valueAsString = source.Substring(6, source.Length - 7);
                            valueAsString = valueAsString.Trim();
                            if (valueAsString != "null")
                            {
                                System.Type parameterType = GetParameterType(parameterName);
                                if (valueAsString.Length >= 2)
                                {
                                    if (valueAsString[0] == '"' && valueAsString[valueAsString.Length - 1] == '"')
                                        valueAsString = valueAsString.Substring(1, valueAsString.Length - 2);
                                }

                                object value = System.Convert.ChangeType(valueAsString, parameterType, System.Globalization.CultureInfo.GetCultureInfo(1033));
                                if (value == null && (parameterType.IsClass || parameterType.IsInterface))
                                    return parameterType;
                                if (value != null)
                                    return value.GetType();

                            }
                            return null;
                        }
                    }

                    return GetSourceType(parameterLoader.Source);
                }
            }
            throw new System.Exception("There isn't parameter with name " + parameterName + "."); ;
        }
        /// <MetaDataID>{7bc9b38f-1079-4be4-8e48-8780b9b1ebf1}</MetaDataID>
        Type GetParameterType(string parameterName)
        {
            if (Operation == null)
                return null;
            foreach (MetaDataRepository.Parameter parameter in Operation.Parameters)
            {
                if (parameter.Name == parameterName)
                    return parameter.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            }
            throw new System.Exception("There isn't parameter with name " + parameterName + "."); ;
        }

        /// <MetaDataID>{345780ba-b738-4b10-a2b6-4994a3e1b145}</MetaDataID>
        System.Type GetSourceType(string source)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                    return null;
                if (source == "$ViewControlObject$")
                    return UIObjectConnection.ObjectType.GetExtensionMetaObject(typeof(Type)) as Type;



                int npos = source.LastIndexOf(".");
                if (npos == -1)
                {
                    if (source.Trim() == "this")
                        return OwnerMemberViewControl.GetType();
                    IObjectMemberViewControl control = UIObjectConnection.GetControlWithName(source);
                    if (control != null)
                        control.GetType();



                }
                else
                {
                    if (source.Substring(0, npos) == "this")
                        return OwnerMemberViewControl.GetPropertyType(source.Substring(npos + 1)).GetExtensionMetaObject(typeof(Type)) as Type;
                    else
                    {
                        IOperationCallerSource objectMemberViewControl = UIObjectConnection.GetControlWithName(source.Substring(0, npos)) as IOperationCallerSource;
                        if (objectMemberViewControl != null)
                            return objectMemberViewControl.GetPropertyType(source.Substring(npos + 1)).GetExtensionMetaObject(typeof(Type)) as Type;
                        return null;
                    }
                }
                return null;
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{ed651033-091a-42b6-8d93-7172de93edf5}</MetaDataID>
        bool IsParameterForForRefresh(string parameterName)
        {
            foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in OperationCall.ParameterLoaders)
            {
                if (parameterLoader.Name == parameterName)
                    return parameterLoader.RefreshFrom;
            }
            return false;
        }


        /// <summary>
        /// Returns the of value of source control which assigned to parameter 
        /// </summary>
        /// <param name="parameterName">
        /// Defines the name of parameter which the operation want find the assigned control as source  
        /// </param>
        /// <returns>
        /// the value of source control 
        /// </returns>
        /// <MetaDataID>{c5875525-5d4e-4d72-a11c-6ad8d00caff1}</MetaDataID>
        object GetParameterValue(string parameterName)
        {
            foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in OperationCall.ParameterLoaders)
            {
                if (parameterLoader.Name == parameterName)
                {
                    try
                    {
                        string source = parameterLoader.Source;
                        if (string.IsNullOrEmpty(source))
                            return null;

                        source = source.Trim();
                        string lowerCaseSource = source.ToLower();
                        if (lowerCaseSource.IndexOf("value(") == 0 && lowerCaseSource[lowerCaseSource.Length - 1] == ')')
                        {
                            string valueAsString = source.Substring(6, source.Length - 7);
                            valueAsString = valueAsString.Trim();
                            if (valueAsString != "null")
                            {
                                System.Type parameterType = GetParameterType(parameterName);
                                if (valueAsString.Length >= 2)
                                {
                                    if (valueAsString[0] == '"' && valueAsString[valueAsString.Length - 1] == '"')
                                        valueAsString = valueAsString.Substring(1, valueAsString.Length - 2);
                                }

                                return System.Convert.ChangeType(valueAsString, parameterType, System.Globalization.CultureInfo.GetCultureInfo(1033));
                            }
                            return null;
                        }

                        if (source == "$ViewControlObject$")
                            return UIObjectConnection.Instance;
                        if (source == "$MenuCommand$")
                            return MenuCommand;


                        int npos = source.LastIndexOf(".");
                        if (npos == -1)
                        {
                            if (source.Trim() == "this")
                                return OwnerMemberViewControl;


                            return UIObjectConnection.GetControlWithName(source);
                        }
                        else
                        {
                            if (source.Substring(0, npos) == "this")
                                return OwnerMemberViewControl.GetPropertyValue(source.Substring(npos + 1));
                            else
                            {
                                IOperationCallerSource objectMemberViewControl = UIObjectConnection.GetControlWithName(source.Substring(0, npos)) as IOperationCallerSource;
                                return objectMemberViewControl.GetPropertyValue(source.Substring(npos + 1));
                            }
                        }
                    }
                    catch (System.Exception error)
                    {

                    }

                }
            }
            throw new System.Exception("There isn't parameter with name " + parameterName + ".");
        }


        /// <summary>
        /// Defines the cotrol where the operetion call wil assign the return value
        /// </summary>
        /// <MetaDataID>{66b3f001-eb22-4d53-837b-eab81fb670d7}</MetaDataID>
        public IOperationCallerSource ReturnDestinationControl
        {
            get
            {
                string destination = OperationCall.ReturnValueDestination;

                if (string.IsNullOrEmpty(destination))
                    return null;
                if (destination == "this")
                    return this.OwnerMemberViewControl;

                if (destination == "this.Value")
                    return OwnerMemberViewControl;
                if (destination.LastIndexOf(".Value") != -1 && destination.LastIndexOf(".Value") == destination.Length - ".Value".Length)
                {
                    destination = destination.Substring(0, destination.LastIndexOf(".Value"));
                    IOperationCallerSource memberViewControl = UIObjectConnection.GetControlWithName(destination) as IOperationCallerSource;
                    if (memberViewControl == null)
                        throw new System.Exception("System  can't find destination control .");
                    return memberViewControl;
                }
                else
                {
                    IOperationCallerSource memberViewControl = UIObjectConnection.GetControlWithName(destination) as IOperationCallerSource;
                    if (memberViewControl == null)
                        throw new System.Exception("System  can't find destination control .");
                    return memberViewControl;
                }
            }

        }
        /// <summary>
        /// Defines a flag which indicate if all is all right to call the operation
        /// Some problems are. There isn't object to call,the metadata are wrong and system can't find the operation to call etc.
        /// </summary>
        /// <MetaDataID>{5d21020d-c4b9-408c-9c29-f84d28e4b518}</MetaDataID>
        public bool CanCall
        {
            get
            {
                if (Operation != null && OperationCall.CallType == CallType.StaticOperationCall)
                    return true;
                if (Operation != null && OperationCall.CallType == CallType.HostingFormOperationCall)
                    return true;
                if (Operation != null && OperationCall.CallType == CallType.ViewControlObjectOperationCall)
                {
                    if (OperationOwnerPath == null && UIObjectConnection.Instance != null)
                        return true;
                    else if (OperationOwnerPath == null && UIObjectConnection.Instance == null)
                        return false;
                    else
                    {
                        DisplayedValue displayedValue = UIObjectConnection.GetDisplayedValue(OperationOwnerPath, UIObjectConnection.GetObjectChangeStateManager(OwnerMemberViewControl as IPathDataDisplayer));
                        if (displayedValue != null && displayedValue.Value != null)
                            return true;
                        else
                            return false;
                    }
                }
                return false;
            }

        }

        /// <MetaDataID>{e655d724-f769-407b-9963-b81fe7ededa9}</MetaDataID>
        public string[] ExtraPaths()
        {
            OOAdvantech.MetaDataRepository.Operation operation = Operation;
            if (operation == null)
                return new string[0];
            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
            return CollectionObjectPaths.GetPathsFor(methodInfo);

        }

        /// <MetaDataID>{09bcf40c-cb85-4e3f-b351-6199a992a163}</MetaDataID>
        public void Rebuild()
        {
            _Operation = null;
        }
    }


}
