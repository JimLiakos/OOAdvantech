using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ConnectableControls.PropertyEditors;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using OOAdvantech.UserInterface.Runtime;
using System.Xml.Linq;
namespace ConnectableControls
{

    /// <MetaDataID>{92365164-7C07-417C-B52C-C90FA0D7E665}</MetaDataID>
    public partial class SearchTextBox : Control, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer, OOAdvantech.UserInterface.Runtime.ICutCopyPasteMoveDragDrop
    {
        /// <MetaDataID>{d4d9609b-7a34-465c-b740-2f39b424a5d6}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{31338c34-37b2-48d1-8626-8064ca4f956c}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }
        /// <MetaDataID>{711ca43e-000d-4dac-a142-88d45ff2b8f2}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }



        /// <MetaDataID>{8f0f1ae0-ca02-471c-8073-aca3e41c34b9}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{8659f2f8-e4e0-4793-9757-3d306859e50c}</MetaDataID>
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

                    if (!string.IsNullOrEmpty(_DisplayMember))
                        _AllPaths.Add(_Path + '.' + _DisplayMember);

                    if (_PresentationObjectType != null)
                    {
                        foreach (string path in OOAdvantech.UserInterface.Runtime.PresentationObjectPaths.GetExtraPathsFor(_PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type))
                        {
                            if (path.IndexOf("RealObject.") == 0)
                                _AllPaths.Add(_Path + "." + path.Substring("RealObject.".Length));
                        }
                    }
                    if (UserInterfaceObjectConnection != null)
                    {
                        foreach (string path in UserInterfaceObjectConnection.GetExtraPathsFor(_Path))
                        {
                            if (path.IndexOf("RealObject.") == 0)
                                _AllPaths.Add(_Path + "." + path.Substring("RealObject.".Length));
                            else
                                _AllPaths.Add(_Path + "." + path);
                        }
                    }

                    return _AllPaths;
                }
                else if (DesignMode)
                {
                    _AllPaths = new OOAdvantech.Collections.Generic.List<string>();
                    _AllPaths.Add(_Path);
                    if (!string.IsNullOrEmpty(_DisplayMember))
                        _AllPaths.Add(_Path + '.' + _DisplayMember);
                    return _AllPaths;
                }
                else
                    return _AllPaths;
            }
        }

        /// <MetaDataID>{82e0fd76-0db4-4256-8f4f-faf854230c7f}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] PropertiesNames
        {
            get
            {
                if (ValueType != null && PresentationObjectType != ValueType)
                    return new string[4] { "Value", "Text", "DragDropObject", "PresentationObject" };
                else
                    return new string[3] { "Value", "Text", "DragDropObject" };
            }
        }
        /// <MetaDataID>{ec7d927f-1a72-42dd-a69e-c9f616c0962c}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {

            if (propertyName == "Text")
                return Text;
            if (propertyName == "PresentationObject")
                return PresentationObject;

            if (propertyName == "DragDropObject")
                return DragDropObject;
            //if (propertyName == "this.Text")
            //    return Text;

            if (propertyName == "this")
                return this;

            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "Value")
                return Value;
            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        /// <MetaDataID>{cb63b131-434e-4f08-8348-8789d18000f8}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            if (propertyName == "PresentationObject" && ValueType != null && PresentationObjectType != ValueType)
                return true;

            if (propertyName == "this")
                return true;
            //if (propertyName == "this.Value")
            //    return true;

            if (propertyName == "Value")
                return true;

            //if (propertyName == "this.Text")
            //    return true;

            if (propertyName == "Text")
                return true;

            if (propertyName == "DragDropObject")
                return true;



            return false;
        }

        /// <MetaDataID>{8005171c-394c-4e94-9126-a835c15718f3}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        /// <MetaDataID>{18ded2ce-5232-4131-bf14-d57c5a3eb4c9}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "PresentationObject")
                return PresentationObjectType;

            if (propertyName == "DragDropObject")
                return UserInterfaceObjectConnection.GetClassifier(typeof(System.Object));

            if (propertyName == "Value")
                return ValueType;
            if (propertyName == "Text")
                return UserInterfaceObjectConnection.GetClassifier(typeof(System.String));
            return null;

        }
        /// <MetaDataID>{7ad89b32-0579-41ae-a0cd-8ba80cf339bb}</MetaDataID>
        private bool _AutoDisable = true;
        /// <MetaDataID>{18530abb-92b4-440f-9426-2aabc46dcfff}</MetaDataID>
        [Category("Object Model Connection")]
        public bool AutoDisable
        {
            get
            {
                return _AutoDisable;
            }
            set
            {
                _AutoDisable = value;
            }
        }


        /// <MetaDataID>{534cadac-d4d7-46b0-9ca6-cb8f5a8a3a66}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }


        /// <MetaDataID>{f50351c8-f9f8-4494-9b63-668af0f75c9a}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && ValueType == null)
            {
                errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                hasErrors = true;
            }
            if (!string.IsNullOrEmpty(_DisplayMember))
            {
                Type type = UserInterfaceObjectConnection.GetType(PresentationObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type, _DisplayMember);
                if (type == null)
                {
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + "' has lost the DisplayMember property connection", FindForm().GetType().FullName));
                    hasErrors = true;
                }
            }
            if (CollectionView.SearchOperationCaller != null)
            {

                foreach (string error in CollectionView.SearchOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + ".OperationCall' " + error, FindForm().GetType().FullName));
                }

                if (CollectionView.SearchOperationCaller.Operation != null && CollectionView.SearchOperationCaller.Operation.ReturnType != null)
                {
                    OOAdvantech.MetaDataRepository.Classifier itemType = OOAdvantech.UserInterface.OperationCall.GetElementType(CollectionView.SearchOperationCaller.Operation.ReturnType);
                    if (ValueType == null || itemType == null || !itemType.IsA(ValueType))
                    {
                        hasErrors = true;
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + " Type Mismatch between OperationCall return type and Path type", FindForm().GetType().FullName));
                    }
                }
            }
            if (CollectionView.InsertOperationCaller != null)
            {
                foreach (string error in CollectionView.InsertOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + ".InsertOperationCall' " + error, FindForm().GetType().FullName));
                }
            }
            if (CollectionView.RemoveOperationCaller != null)
            {
                foreach (string error in CollectionView.RemoveOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + ".RemoveOperationCall' " + error, FindForm().GetType().FullName));
                }
            }
            if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
            {
                _PresentationObjectType = ViewControlObject.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + "' has invalid PresentationObject.", FindForm().GetType().FullName));
                else
                {
                    if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(_PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType))
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: SearchingBox '" + Name + "' You can't assign the '" + _PresentationObjectType.FullName + "' to the PresentationObjectType.\n Check the property rules.", FindForm().GetType().FullName));
                }
            }
            return hasErrors;
        }



        #region Displayed object member of object and choice collections

        /// <MetaDataID>{8c166c00-28fb-4eb1-b0bb-cbe989e59d21}</MetaDataID>
        string _DisplayMember;
        /// <summary>
        /// In case where the search box is control for member which is object 
        /// and the object is not value type, the search display the return value of ToString() function call.
        /// If you want to display a member of object use this property to choose the member.
        ///  </summary>
        /// <MetaDataID>{b3c95066-5c7e-411d-919f-9aebe8f1125e}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("When there is display member, the control presents the member of value object, otherwise presents the value object with the assistance of ToString method."),
        Category("Object Model Connection")]
        public object DisplayMember
        {
            get
            {
                return _DisplayMember;
            }
            set
            {
                if (value is string)
                    _DisplayMember = value as string;
                else if (value is MetaData)
                    _DisplayMember = (value as MetaData).Path;
            }
        }


        /// <MetaDataID>{FBE951ED-AEBF-482E-87B6-D1BAA3D83A47}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if ((context.PropertyDescriptor.Name == "Path" || context.PropertyDescriptor.Name == "AssignPresentationObjectType") && UserInterfaceObjectConnection != null)
            {
                if (context.PropertyDescriptor.Name == "Path")
                    return UserInterfaceObjectConnection.PresentationObjectType;
                else
                    return AssemblyManager.GetActiveWindowProject();


            }

            if (context.PropertyDescriptor.Name == "DisplayMember")
                return PresentationObjectType;

            return null;
        }
        /// <MetaDataID>{72978523-e0a6-49d2-9d2b-1f38873fcf45}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier PresentationObjectType
        {
            get
            {
                if (_PresentationObjectType != null)
                    return _PresentationObjectType;
                if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
                    _PresentationObjectType = this.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;

                if (_PresentationObjectType != null)
                    return _PresentationObjectType;
                return ValueType;
            }
        }

        /// <MetaDataID>{d7b88a56-599e-414c-ae9f-9a57c4eedc36}</MetaDataID>
        string PresentationObjectTypeFullName;
        /// <MetaDataID>{1c1e53f0-0a03-49ca-934f-2757e30efbb9}</MetaDataID>
        OOAdvantech.MetaDataRepository.Class _PresentationObjectType;
        /// <MetaDataID>{ab7c28e8-2c16-4270-8f8e-4a4a12150d93}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        new public object AssignPresentationObjectType
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
                if (value is MetaData)
                {
                    PresentationObjectTypeFullName = (value as MetaData).MetaObject.FullName;
                    _PresentationObjectType = (value as MetaData).MetaObject as OOAdvantech.MetaDataRepository.Class;


                }
                if (value == null || (value is string && string.IsNullOrEmpty(value as string)))
                {
                    PresentationObjectTypeFullName = "";
                    _PresentationObjectType = null;
                }

            }
        }


        /// <MetaDataID>{fffafc8b-78cd-4315-a91b-0c03e3019b6c}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier CollectionObjectType
        {
            get
            {
                OOAdvantech.MetaDataRepository.Operation searchOperation = CollectionView.SearchOperation;
                if (searchOperation != null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Operation operation in searchOperation.ReturnType.GetOperations("GetEnumerator"))
                    {
                        OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                        if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
                            return enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                    }
                }
                return null;
            }
        }

        /// <MetaDataID>{c718856f-562c-42d3-bf28-03f9822f71c2}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {


                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path))
                    return this.UserInterfaceObjectConnection.GetClassifier(_Path);

                return CollectionObjectType;


            }
        }

        /// <MetaDataID>{fd77d1c5-3837-4738-80c2-f211753e11f8}</MetaDataID>
        object _Value;
        /// <MetaDataID>{38ecca2c-1c3a-4c18-b399-0cd9c0d75ecc}</MetaDataID>
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


        /// <MetaDataID>{cd4058c8-54e7-4c04-9525-bfcb202a3ad0}</MetaDataID>
        CollectionView CollectionView;

        #endregion


        /// <MetaDataID>{ce141312-b4eb-4427-a705-9837a2441ea1}</MetaDataID>
        [Category("Object Model Connection")]
        [Browsable(false)]
        public bool IsConnectionDataCorrect
        {
            get
            {
                try
                {
                    return true;

                    if (UserInterfaceObjectConnection == null && !string.IsNullOrEmpty(Path as string))
                        return false;
                    if (CollectionView.SearchOperation == null && !string.IsNullOrEmpty(OperationCall as string))
                    {
                        if (DesignMode)
                            TextBox.Text = "Error on OperationCall Property";

                        return false;
                    }
                    if (CollectionView.SearchOperation != null)
                    {
                        if (CollectionView.SearchOperation.ReturnType.GetOperations("GetEnumerator").Count == 0)
                        {
                            if (DesignMode)
                                TextBox.Text = "Error on OperationCall return type";
                            return false;
                        }
                        else
                        {
                            OOAdvantech.MetaDataRepository.Classifier operationCallRetrunType = CollectionView.SearchOperation.ReturnType.GetOperations("GetEnumerator")[0].ReturnType;
                            if (operationCallRetrunType.Name.IndexOf("IEnumerator`1") != 0)
                            {
                                if (DesignMode)
                                    TextBox.Text = "Error on OperationCall return type";
                                return false;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_DisplayMember) && OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(operationCallRetrunType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier, _DisplayMember) == null)
                                {
                                    if (DesignMode)
                                        TextBox.Text = "Display member is'nt member of " + (operationCallRetrunType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier).Name;
                                    return false;
                                }
                            }

                            OOAdvantech.MetaDataRepository.Operation operation = CollectionView.SearchOperation;
                            if (operation != null)
                            {
                                operationCallRetrunType = operation.ReturnType.GetOperations("GetEnumerator")[0].ReturnType;
                                if (operationCallRetrunType.Name.IndexOf("IEnumerator`1") == 0)
                                {
                                    if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path as string))
                                    {
                                        if (this.UserInterfaceObjectConnection.GetClassifier(_Path as string) != operationCallRetrunType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier)
                                        {
                                            if (DesignMode)
                                                TextBox.Text = "Type mismatch check operation call return type" + (operationCallRetrunType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier).Name;
                                            return false;
                                        }
                                    }
                                }
                            }

                        }

                    }

                    if (UserInterfaceObjectConnection == null && CollectionView.SearchMethod == null && !string.IsNullOrEmpty(_DisplayMember))
                    {
                        if (DesignMode)
                            TextBox.Text = "Error on DisplayMember Property";


                        return false;
                    }
                    if (UserInterfaceObjectConnection == null && CollectionView.SearchMethod != null)
                    {
                        if (DesignMode)
                            TextBox.Text = "ViewControlObject Property is empty";

                        return false;


                    }
                    OOAdvantech.MetaDataRepository.Classifier classifier = null;
                    if (UserInterfaceObjectConnection != null)
                    {
                        if (DesignMode)
                        {
                            //TODO add functionality

                            classifier = UserInterfaceObjectConnection.GetClassifier(Path as string);
                            if (classifier == null && !string.IsNullOrEmpty(Path as string))
                            {
                                TextBox.Text = "Error on Path Property";
                                return false;
                            }
                            if (CollectionView.SearchOperation != null)
                            {
                                OOAdvantech.MetaDataRepository.Operation searchOperation = CollectionView.SearchOperation;
                                if (!searchOperation.ReturnType.IsBindedClassifier)
                                    return false;
                                if (searchOperation.ReturnType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] != classifier && !string.IsNullOrEmpty(Path as string))
                                {
                                    TextBox.Text = "Error on Path Property";
                                    return false;
                                }
                            }

                            return true;

                        }
                        else
                        {
                            classifier = UserInterfaceObjectConnection.GetClassifier(Path as string);
                            if (classifier == null && !string.IsNullOrEmpty(Path as string))
                            {
                                if (DesignMode)
                                    TextBox.Text = "Error on Path Property";

                                return false;
                            }
                            if (CollectionView.SearchOperation != null)
                            {
                                OOAdvantech.MetaDataRepository.Operation operation = CollectionView.SearchOperation;
                                if (!operation.ReturnType.IsBindedClassifier)
                                    return false;

                                if ((operation.ReturnType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier) != classifier && !string.IsNullOrEmpty(Path as string))
                                {
                                    if (DesignMode)
                                        TextBox.Text = "Error on Path Property";

                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CollectionView.SearchOperation != null)
                        {
                            OOAdvantech.MetaDataRepository.Operation operation = CollectionView.SearchOperation;
                            if (!operation.ReturnType.IsBindedClassifier)
                                return false;
                            classifier = operation.ReturnType.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                        }

                    }
                    if (!string.IsNullOrEmpty(_DisplayMember) && !string.IsNullOrEmpty(Path as string) && classifier == null)
                        return false;

                    if (!string.IsNullOrEmpty(_DisplayMember) && classifier != null)
                    {
                        if (OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(classifier, _DisplayMember) == null)
                            return false;
                    }
                    if (DesignMode)
                        TextBox.Text = Text;
                    return true;
                }
                catch (Exception error)
                {
                    return false;
                }
            }

        }











        #region Collection view operations

        /// <MetaDataID>{671b86df-11e4-42fe-8642-90b218912fed}</MetaDataID>
        XDocument InsertOperationCallMetaData;
        /// <MetaDataID>{d5791a26-2982-4b63-9735-f07580dabe97}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object InsertOperationCall
        {
            get
            {
                if (InsertOperationCallMetaData == null)
                {
                    InsertOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", InsertOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    CollectionView.InsertOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (CollectionView.InsertOperation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(default(string), "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(InsertOperationCallMetaData.ToString() as string, CollectionView.InsertOperation.Name);

                metaDataVaue.MetaDataAsObject = CollectionView.InsertOperationCall;
                return metaDataVaue;
            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("InsertOperationCall", false).SetValue(this, InsertOperationCall);
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


                if (InsertOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    CollectionView.InsertOperationCall = null;
                    InsertOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            InsertOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", InsertOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            InsertOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", InsertOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }


                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        InsertOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", InsertOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        CollectionView.InsertOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (CollectionView.InsertOperationCall == null)
                        CollectionView.InsertOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        CollectionView.InsertOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }
        /// <MetaDataID>{33054f8e-b398-4e59-90b4-b1df04fc5471}</MetaDataID>
        XDocument RemoveOperationCallMetaData;
        /// <MetaDataID>{cfb45c33-10db-41d2-af93-319768c91a0a}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object RemoveOperationCall
        {
            get
            {
                if (RemoveOperationCallMetaData == null)
                {
                    RemoveOperationCallMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", RemoveOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    CollectionView.RemoveOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }

                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (CollectionView.RemoveOperation == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(RemoveOperationCallMetaData.ToString() as string, "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(RemoveOperationCallMetaData.ToString() as string, CollectionView.RemoveOperation.Name);

                //MetaDataValue metaDataVaue = new MetaDataValue(RemoveOperationCallMetaData.OuterXml as string);
                metaDataVaue.MetaDataAsObject = CollectionView.RemoveOperationCall;
                return metaDataVaue;



            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("RemoveOperationCall", false).SetValue(this, RemoveOperationCall);
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


                if (RemoveOperationCallMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    CollectionView.RemoveOperationCall = null;
                    RemoveOperationCallMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            RemoveOperationCallMetaData = XDocument.Parse(metaData);
                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", RemoveOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            RemoveOperationCallMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", RemoveOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }



                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        RemoveOperationCallMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", RemoveOperationCallMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        CollectionView.RemoveOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (CollectionView.RemoveOperationCall == null)
                        CollectionView.RemoveOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        CollectionView.RemoveOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }

        /// <MetaDataID>{e0a49934-5f56-4f77-bda9-f015f81cf58b}</MetaDataID>
        XDocument MetaDataAsXmlDocument;
        /// <MetaDataID>{b0ae10e2-baed-4fc1-a4b8-5c0658aeef67}</MetaDataID>
        [Editor(typeof(EditOperationCallMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object OperationCall
        {
            get
            {
                try
                {
                    if (MetaDataAsXmlDocument == null)
                    {
                        MetaDataAsXmlDocument = new XDocument();
                        OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        CollectionView.SearchOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                    }


                    //MetaDataValue metaDataVaue = new MetaDataValue(MetaDataAsXmlDocument.OuterXml as string);
                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                    if (CollectionView.SearchOperation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(default(string), "none");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(MetaDataAsXmlDocument.ToString(), CollectionView.SearchOperation.Name);

                    metaDataVaue.MetaDataAsObject = CollectionView.SearchOperationCall;
                    return metaDataVaue;
                }
                catch (System.Exception error)
                {
                    throw;
                }

            }
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("OperationCall", false).SetValue(this, OperationCall);
                    return;
                }
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage listViewStorage = null;
                if (metaDataVaue == null)
                    return;



                if (MetaDataAsXmlDocument == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    CollectionView.SearchOperationCall = null;
                    MetaDataAsXmlDocument = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            MetaDataAsXmlDocument = XDocument.Parse(metaData);

                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            MetaDataAsXmlDocument = new XDocument();
                            listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }


                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        MetaDataAsXmlDocument = new XDocument();
                        listViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", MetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = listViewStorage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        CollectionView.SearchOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (CollectionView.SearchOperationCall == null)
                    {
                        CollectionView.SearchOperationCall = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                    }

                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                    {
                        CollectionView.SearchOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                    }

                }
            }
        }
        #endregion



        ///// <MetaDataID>{41001096-0EF0-4E04-A596-7E7062A98B5B}</MetaDataID>
        //internal static System.Windows.Forms.Control GetControlWithName(System.Windows.Forms.Control.ControlCollection controls, string controlName)
        //{

        //    //TODO θα υπάρχει πρόβλημα όταν ένα control που είναι στη φορμα θα περιέχει control με το ιδιο όνομα
        //    if (controls.ContainsKey(controlName))
        //        return controls[controlName];

        //    foreach (System.Windows.Forms.Control containedControl in controls)
        //    {
        //        System.Windows.Forms.Control control = GetControlWithName(containedControl.Controls, controlName);
        //        if (control != null)
        //            return control;
        //    }
        //    return null;
        //}




        /// <MetaDataID>{82391f2e-6c7b-45d9-b6a3-c95dc455c8ce}</MetaDataID>
        DropDownContainer DropDownContainer;
        /// <MetaDataID>{C5EF04C2-434C-4159-93DC-0D0D4226E7BF}</MetaDataID>
        public SearchTextBox()
        {

            InitializeComponent();
            CollectionView = new CollectionView(this);

            //   BackColor = TextBox.BackColor;
            TextBox.AutoSize = false;
            SetStyle(ControlStyles.FixedHeight, true);
            TextBox.BorderStyle = BorderStyle.None;
            this.DropDownContainer = new DropDownContainer();

            this.DropDownContainer.MdiParent = this.FindForm();

            this.DropDownContainer.ItemSelected += new EventHandler(DropDownContainer_ItemSelected);
            this.DropDownContainer.VisibleChanged += new EventHandler(DropDownContainerVisibleChanged);

            TextBox.LostFocus += new EventHandler(TextBoxLostFocus);
            SearchBtn.LostFocus += new EventHandler(TextBoxLostFocus);
            TextBox.KeyDown += new KeyEventHandler(OnKeyDown);

            _EnableProperty = new DependencyProperty(this, "Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }

        /// <MetaDataID>{1c3c22ed-38b4-4dcb-86e8-f6e803878d65}</MetaDataID>
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




        /// <MetaDataID>{7ce00b55-6987-456d-9014-0a1bb3977311}</MetaDataID>
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
                OnSearchButtonClick(this, EventArgs.Empty);

        }





        /// <MetaDataID>{0FE6C5DD-F77C-4A1C-9578-D6464E4C81B8}</MetaDataID>
        void DropDownContainerVisibleChanged(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }



        /// <MetaDataID>{B2693CE3-CA49-43A4-BF41-813125066A48}</MetaDataID>
        void TextBoxLostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }
        /// <MetaDataID>{8a51d977-8d4b-4eaf-bcc8-d48b3436946d}</MetaDataID>
        int _DropDownHeight = 106;
        /// <MetaDataID>{f39ab33a-a141-41a6-a619-8a6bcea26389}</MetaDataID>
        [Description("The height, in pixel, of the drop-down box in a search box"),
        Category("Behavior")]
        public int DropDownHeight
        {
            get
            {
                return _DropDownHeight;
            }
            set
            {
                _DropDownHeight = value;
            }
        }

        /// <MetaDataID>{f61d7870-6d9b-4996-bbb9-0a6f223ede26}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _ViewControlObject = null;
        /// <MetaDataID>{356a7cec-bf2e-4126-9dff-3d8ec7329514}</MetaDataID>
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
                if (DesignMode)
                    Invalidate();
            }
        }
        /// <MetaDataID>{d3912bfa-2310-4ae6-afb6-f7850ef5aea5}</MetaDataID>
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


        /// <MetaDataID>{61C7B403-E519-4206-A618-7F003D9B4BD2}</MetaDataID>
        void DropDownContainer_ItemSelected(object sender, EventArgs e)
        {
            TextBox.Text = (sender as DropDownContainer).SelectedObject.ToString();
            _Value = ((sender as DropDownContainer).SelectedObject as DropDownContainer.ListBoxItem).DisplayiedObject;

            if (UserInterfaceObjectConnection != null && Path != null)
            {
                if (ValueType != _PresentationObjectType && _PresentationObjectType != null)
                {
                    this.UserInterfaceObjectConnection.SetValue((((sender as DropDownContainer).SelectedObject as DropDownContainer.ListBoxItem).DisplayiedObject as OOAdvantech.UserInterface.Runtime.IPresentationObject).GetRealObject(), Path.ToString());
                }
                else
                    this.UserInterfaceObjectConnection.SetValue(((sender as DropDownContainer).SelectedObject as DropDownContainer.ListBoxItem).DisplayiedObject, Path.ToString());

            }
            OOAdvantech.UserInterface.Runtime.PresentationObject<object> rt;


            LoadControlValues();
        }



        /// <MetaDataID>{f8039c99-de55-4ecf-b13a-c16e42eeb842}</MetaDataID>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                TextBox.Font = Font;
                this.DropDownContainer.Font = Font;

                OnSizeChanged(null);
            }
        }


        /// <MetaDataID>{cc9cf706-c81b-4219-b878-7ccc92cb316e}</MetaDataID>
        Color _OriginBackColor = Color.Black;

        /// <MetaDataID>{0685470E-4780-4E14-B879-D886071DB01E}</MetaDataID>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            if (!IsConnectionDataCorrect)
            {
                if (TextBox.BackColor != Color.FromArgb(255, 153, 153))
                {
                    _OriginBackColor = TextBox.BackColor;
                    TextBox.BackColor = Color.FromArgb(255, 153, 153);
                }
            }
            else
            {
                if (_OriginBackColor != Color.Black)
                    TextBox.BackColor = _OriginBackColor;

            }
            System.Reflection.PropertyInfo property = typeof(Application).GetProperty("UseVisualStyles", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            bool useVisualStyles = (bool)property.GetValue(null, null);
            if (useVisualStyles || DesignMode)
                TextBoxRenderer.DrawTextBox(pevent.Graphics, ClientRectangle, System.Windows.Forms.VisualStyles.TextBoxState.Normal);
            else
                ControlPaint.DrawBorder3D(pevent.Graphics, ClientRectangle, Border3DStyle.Sunken);
        }
        /// <MetaDataID>{32F7C163-29DC-4175-8B8E-F9B247D87499}</MetaDataID>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            OnSizeChanged(null);


        }
        /// <MetaDataID>{1ECE5ED7-2D7E-409F-BB39-4E4911AE36BC}</MetaDataID>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (IsHandleCreated)
            {
                SuspendLayout();

                TextBox.Location = new Point(2, 2);
                int fontHeight = (int)Font.GetHeight(System.Drawing.Graphics.FromHwnd(Handle));
                TextBox.Height = fontHeight + 3;
                this.SearchBtn.Height = TextBox.Height + 2;
                int width = this.SearchBtn.Width - this.SearchBtn.Height;// the search button is square
                this.SearchBtn.Width = this.SearchBtn.Height;
                this.SearchBtn.Location = new Point(Size.Width - SearchBtn.Width - 1, 1);
                TextBox.Width = Width - this.SearchBtn.Height - 4;

                if (Height != TextBox.Height + 4)
                    Height = TextBox.Height + 4;

                Invalidate();

                ResumeLayout();
                if (e != null)
                    base.OnSizeChanged(e);
            }

        }
        /// <MetaDataID>{9a3e14ab-a90e-4340-906b-41e205c9edd5}</MetaDataID>
        [Browsable(false)]
        public bool HasFocus
        {
            get
            {
                return SearchBtn.Focused | TextBox.Focused | DropDownContainer.Focused;
            }
        }
        /// <MetaDataID>{ff776ce6-c470-42c0-93f1-6b99ed13a19a}</MetaDataID>
        public new Rectangle Bounds
        {
            get
            {

                if (!DropDownContainer.Visible)
                    return base.Bounds;
                else
                {
                    if (DropDownContainer.Bounds.Y > base.Bounds.Y)
                        return new Rectangle(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, base.Bounds.Height + DropDownContainer.Bounds.Height);
                    else
                        return new Rectangle(base.Bounds.X, DropDownContainer.Bounds.Y, base.Bounds.Width, base.Bounds.Height + DropDownContainer.Bounds.Height);

                }
            }
        }


        #region DragDrop Behavior

        /// <MetaDataID>{e1c8ec3c-8bca-44e5-88fa-cc5348d517bb}</MetaDataID>
        public void CutObject(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{9374c104-706c-48f1-a593-978a237d439f}</MetaDataID>
        public void PasteObject(object dropObject)
        {

            DragDropObject = dropObject;
            if (_dragMode && UserInterfaceObjectConnection != null && Path != null && ValueType != null && (ValueType.GetExtensionMetaObject(typeof(System.Type)) as System.Type).IsInstanceOfType(DragDropObject))
            {
                this.UserInterfaceObjectConnection.SetValue(DragDropObject, Path.ToString());
                LoadControlValues();
            }
        }
        /// <MetaDataID>{eab74b08-7e7b-488f-acc5-9de274edf3ef}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _AllowDropOperationCaller;
        /// <MetaDataID>{e97e5c5f-906f-456f-8135-c331a762e5c6}</MetaDataID>
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
                _AllowDropOperationCaller = new OperationCaller(_AllowDropOperationCall, this);
                return _AllowDropOperationCaller;
            }
        }
        /// <MetaDataID>{4f1c1135-005e-4719-9078-74e9f80bcb91}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{ceee2dee-8b8b-4a9b-82c3-9080330077ac}</MetaDataID>
        [Category("DragDrop Behavior")]
        public bool AllowDrag
        {
            get
            {
                return _AllowDrag;
            }
            set
            {
                _AllowDrag = value;
            }
        }

        /// <MetaDataID>{b72885ad-d7a6-4ed0-b9a1-c0bc590eaa5e}</MetaDataID>
        XDocument AllowDropOperationCallMetaData;
        /// <MetaDataID>{569a5d24-536b-44e5-9f98-00dd53034980}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _AllowDropOperationCall;
        /// <MetaDataID>{b8b8b564-0d31-4091-b26b-073bc39bb45b}</MetaDataID>
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

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT operation FROM OOAdvantech.UserInterface.OperationCall operation ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        _AllowDropOperationCall = setInstance["operation"] as OOAdvantech.UserInterface.OperationCall;
                        break;
                    }
                    if (_AllowDropOperationCall == null)
                        _AllowDropOperationCall = storage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        _AllowDropOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }

        /// <MetaDataID>{7752676a-d001-443e-842b-52dad97b00ab}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DragDropOperationCaller;
        /// <MetaDataID>{320e3334-f0cf-4f79-a5d3-07af30716334}</MetaDataID>
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
                _DragDropOperationCaller = new OperationCaller(_DragDropOperationCall, this);
                return _DragDropOperationCaller;
            }
        }

        /// <MetaDataID>{331b54b6-cb2f-4d84-843b-4bc64994d742}</MetaDataID>
        XDocument DragDropOperationCallMetaData;
        /// <MetaDataID>{472c6c58-927e-4157-b71c-988484fad001}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _DragDropOperationCall;
        /// <MetaDataID>{214afdcc-d7ca-484e-a37e-aa7503e7b8a9}</MetaDataID>
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

        /// <MetaDataID>{d48184e1-041f-4c1b-8241-3b8fdff9be2f}</MetaDataID>
        DragDropTransactionOptions _DragDropTransactionOption;
        /// <MetaDataID>{252bc3d7-5429-4789-8861-d36705310f06}</MetaDataID>
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


        /// <MetaDataID>{1f12611b-1844-4aaa-abf4-25870593ea65}</MetaDataID>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (AllowDrag && System.Windows.Forms.Control.MouseButtons == MouseButtons.Left)
            {
                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, Value, DragDropTransactionOption);
                DoDragDrop(dragDropActionManager, DragDropEffects.Copy | DragDropEffects.Move);
            }

        }
        /// <MetaDataID>{7ef653db-6ddd-4fbf-a88b-d31fcd7e75dc}</MetaDataID>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            System.Drawing.Point mousePos = System.Windows.Forms.Control.MousePosition;
            if (!Parent.RectangleToScreen(new System.Drawing.Rectangle(Location, Size)).Contains(System.Windows.Forms.Control.MousePosition)
                && e.Button == MouseButtons.Left && AllowDrag)
            {
                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, Value, DragDropTransactionOption);
                DoDragDrop(dragDropActionManager, DragDropEffects.Copy | DragDropEffects.Move);

            }

        }

        /// <MetaDataID>{83db0b3d-66f1-448b-90f4-d4479dee019c}</MetaDataID>
        bool _dragMode = false;

        /// <MetaDataID>{8dbfbf2f-efc6-4dcd-b77f-d31b3a02b46b}</MetaDataID>
        object DragDropObject;
        /// <MetaDataID>{29c7d7e8-a557-4c60-8e46-db4a8b7b6d8f}</MetaDataID>
        protected override void OnDragLeave(EventArgs e)
        {
            _dragMode = false;
            DragDropObject = null;
            base.OnDragLeave(e);
        }

        /// <MetaDataID>{d2146416-85a7-49f9-89ca-ffe9611e79c9}</MetaDataID>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            _dragMode = false;
            try
            {
                if (AllowDropOperationCaller != null && AllowDropOperationCaller.Operation != null)
                {
                    DragDropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
                    if (DragDropObject is DragDropActionManager)
                        DragDropObject = (DragDropObject as DragDropActionManager).DragedObject;

                    object ret = AllowDropOperationCaller.Invoke();
                    if (ret is OOAdvantech.DragDropMethod)
                    {
                        drgevent.Effect = (DragDropEffects)ret;
                        if ((DragDropEffects)ret != DragDropEffects.None)
                            _dragMode = true;
                    }
                }
                else
                    drgevent.Effect = DragDropEffects.None;
            }
            catch (System.Exception error)
            {
                throw;
            }
            base.OnDragEnter(drgevent);

        }

        /// <MetaDataID>{e2d1565a-8a37-435c-b189-45a4556e7fe6}</MetaDataID>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {


            if (_dragMode)
            {
                object dropObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
                if (dropObject is DragDropActionManager)
                    (dropObject as DragDropActionManager).DropObject((OOAdvantech.DragDropMethod)drgevent.Effect, this);
                else
                    PasteObject(dropObject);
            }
            _dragMode = false;
            DragDropObject = null;
            base.OnDragDrop(drgevent);
        }

        #endregion

        /// <MetaDataID>{B5FA563E-8FBC-47AC-A4AD-5655310F55FF}</MetaDataID>
        private void OnSearchButtonClick(object sender, EventArgs e)
        {

            if (UserInterfaceObjectConnection == null)
                return;
            object returnValue = CollectionView.InvokeSearchOperation();
            if (returnValue == null || returnValue.GetType().GetMethod("GetEnumerator", new System.Type[0]) == null)
                return;
            OOAdvantech.UserInterface.Runtime.IOperationCallerSource control = CollectionView.SearchOperationCaller.ReturnDestinationControl;
            if (control == this || string.IsNullOrEmpty(CollectionView.SearchOperationCaller.OperationCall.ReturnValueDestination))
            {
                System.Collections.IEnumerator enumerator = returnValue.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(returnValue, new object[0]) as System.Collections.IEnumerator;
                LoadDropDownContainer(enumerator);
                ShowDropDown();
            }
            else if (control is IObjectMemberViewControl)
                control.SetPropertyValue("Value", returnValue);
        }


        /// <MetaDataID>{5c7591d4-0be2-4f78-88b9-13e4aa0d507d}</MetaDataID>
        private void ShowDropDown()
        {
            this.DropDownContainer.Font = Font;

            if (DropDownContainer.Height > DropDownHeight)
                DropDownContainer.Height = DropDownHeight;

            Point p = this.Parent.PointToScreen(Location);


            Rectangle screenBounds = Screen.GetBounds(p);
            int height = this.DropDownContainer.Height;
            if (p.Y + this.Height + this.DropDownContainer.Height > screenBounds.Bottom)
            {
                p.Y -= this.DropDownContainer.Size.Height;
                this.DropDownContainer.Location = p;

                this.FindForm().AddOwnedForm(this.DropDownContainer);
                //this.activationListener.AssignHandle(this.parentForm.Handle);

                this.DropDownContainer.Width = Width;
                this.DropDownContainer.SelectedObject = TextBox.Text;
                this.DropDownContainer.ShowDropDown();

            }
            else
            {
                p.Y += this.Height;
                this.DropDownContainer.Location = p;

                this.FindForm().AddOwnedForm(this.DropDownContainer);
                //this.activationListener.AssignHandle(this.parentForm.Handle);

                this.DropDownContainer.Width = Width;
                this.DropDownContainer.SelectedObject = TextBox.Text;
                this.DropDownContainer.ShowDropDown();
                this.DropDownContainer.Activate();


            }

            // A little bit of fun.  We've shown the popup,
            // but because we've kept the main window's
            // title bar in focus the tab sequence isn't quite
            // right.  This can be fixed by sending a tab,
            // but that on its own would shift focus to the
            // second control in the form.  So send a tab,
            // followed by a reverse-tab.

            // Send a Tab command:

            List.Win32.NativeMethods.keybd_event((byte)Keys.Tab, 0, 0, 0);
            List.Win32.NativeMethods.keybd_event((byte)Keys.Tab, 0, List.Win32.KeyEventFFlags.KEYEVENTF_KEYUP, 0);

            // Send a reverse Tab command:
            List.Win32.NativeMethods.keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
            List.Win32.NativeMethods.keybd_event((byte)Keys.Tab, 0, 0, 0);
            List.Win32.NativeMethods.keybd_event((byte)Keys.Tab, 0, List.Win32.KeyEventFFlags.KEYEVENTF_KEYUP, 0);
            List.Win32.NativeMethods.keybd_event((byte)Keys.ShiftKey, 0, List.Win32.KeyEventFFlags.KEYEVENTF_KEYUP, 0);
        }

        /// <MetaDataID>{6cc3f487-9204-4719-b9bb-317e1d8fa164}</MetaDataID>
        private void LoadDropDownContainer(System.Collections.IEnumerator enumerator)
        {

            OOAdvantech.Collections.Generic.List<string> paths = new OOAdvantech.Collections.Generic.List<string>();
            paths.Add("Root");
            if (!string.IsNullOrEmpty(_DisplayMember) && ValueType == PresentationObjectType)
                paths.Add("Root." + _DisplayMember);
            if (_PresentationObjectType != null)
            {
                foreach (string path in OOAdvantech.UserInterface.Runtime.PresentationObjectPaths.GetExtraPathsFor(_PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type))
                {
                    if (path.IndexOf("RealObject.") == 0)
                        paths.Add("Root." + path.Substring("RealObject.".Length));
                }
            }

            foreach (string path in CollectionView.SearchOperationCaller.ExtraPaths())
                paths.Add("Root." + path);


            UserInterfaceObjectConnection.BatchLoadPathsValues(enumerator, PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type, paths);

            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;


                this.UserInterfaceObjectConnection.Control(obj);
                object presentationObject = null;
                string text = null;
                if (obj != null)
                    text = obj.ToString();

                if (ValueType != PresentationObjectType && PresentationObjectType != null)
                {
                    presentationObject = this.UserInterfaceObjectConnection.GetPresentationObject(obj, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                    if (presentationObject != null)
                        text = presentationObject.ToString();
                }


                System.Type type = null;
                if (!string.IsNullOrEmpty(_DisplayMember))
                    type = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(ValueType, DisplayMember.ToString()).GetExtensionMetaObject(typeof(Type)) as Type;
                if (type != null)
                {
                    object displayedMemberValue = null;
                    bool returnValueAsCollection = false;
                    if (presentationObject != null)
                        displayedMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(presentationObject, PresentationObjectType, _DisplayMember, null, out returnValueAsCollection);
                    else
                        displayedMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(obj, ValueType, _DisplayMember, null, out returnValueAsCollection);


                    object memberValue = null;
                    memberValue = displayedMemberValue;

                    if (memberValue != null)
                        this.DropDownContainer.Items.Add(new DropDownContainer.ListBoxItem(obj, presentationObject, memberValue.ToString()));
                }
                else
                    this.DropDownContainer.Items.Add(new DropDownContainer.ListBoxItem(obj, presentationObject, text));
            }
        }
        /// <MetaDataID>{56d1e8af-82fd-465f-9fdd-53ba12606038}</MetaDataID>
        string _Text="";
        /// <MetaDataID>{6e10e448-f870-49b1-b4b6-89c4eec68726}</MetaDataID>
        public override string Text
        {
            get
            {
                if (DesignMode)
                    return _Text;
                else
                    return TextBox.Text;
            }
            set
            {
                _Text = value;
                if (_Text == null)
                    _Text = "";

                TextBox.Text = value;
            }
        }
        /// <MetaDataID>{421f74f1-b92a-46c4-9173-ba54735d3eec}</MetaDataID>
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
        /// <MetaDataID>{8ecd788b-5fa8-4808-8016-455f51779161}</MetaDataID>
        [Description("The connected object updated automatically when the value of control changed, otherwise the object updated when the form closing.")
        , Category("Object Model Connection")]
        public UpdateStyle UpdateStyle
        {
            get
            {
                return UpdateStyle.OnSaveControlsValue; ;
            }
            set
            {
             

            }
        }
        #region IObjectMemberViewControl Members
        /// <MetaDataID>{76808074-209C-4AD4-8887-7CDC5D252602}</MetaDataID>
        protected override void OnLostFocus(EventArgs e)
        {
            if (!HasFocus)
            {
                if (UserInterfaceObjectConnection.State == ViewControlObjectState.Passive)
                    return;
                if (string.IsNullOrEmpty(_Path))
                    return;
                Text = "";
                if (_Value != null)
                {
                    OOAdvantech.MetaDataRepository.Classifier type = null;
                    if (ValueType == null)
                        return;
                    if (!string.IsNullOrEmpty(_DisplayMember))
                        type = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(ValueType, DisplayMember.ToString());
                    if (type != null)
                    {

                        bool returnValueAsCollection = false;
                        
                        object displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(_Value, ValueType, _DisplayMember, null, out returnValueAsCollection);

                        object memberValue = null;
                        memberValue = displayedValue;
                        if (memberValue != null)
                            Text = memberValue.ToString();
                    }
                    else
                        Text = _Value.ToString();

                }
            }
            base.OnLostFocus(e);
        }

        /// <MetaDataID>{23ef332e-165d-4bab-9605-94d469f92f6c}</MetaDataID>
        object _PresentationObject;
        /// <MetaDataID>{3e283528-aa20-4e3a-8e71-8d72e66d8316}</MetaDataID>
        public object PresentationObject
        {
            get
            {
                if (_PresentationObject != null)
                    return _PresentationObject;
                else
                    return _Value;
            }
        }


        #region Auto disable code
        /// <MetaDataID>{36df892f-5f8d-4102-b8d1-00bd902e1026}</MetaDataID>
        bool _UserDefinedEnableValue = true;
        /// <MetaDataID>{c7039e09-a103-46a0-b279-252a6f02fa3e}</MetaDataID>
        bool SuspendDisplayedValueChangedHandler = false;
        /// <MetaDataID>{b9071fd6-99d0-47c3-9c86-00960b8a3edf}</MetaDataID>
        bool _AutoProducedEnableValue = true;
        /// <MetaDataID>{e1d647ff-0783-49ee-b85c-280584172899}</MetaDataID>
        bool EnableAutoProduced = false;
        /// <MetaDataID>{37dd2c1c-063f-4a45-8be0-a9f2b84e8f4c}</MetaDataID>
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
        /// <MetaDataID>{b907a5ce-8dbf-40a4-b866-67408151700a}</MetaDataID>
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


        /// <MetaDataID>{d9fe6891-262b-4fa5-a477-9472c0690b8e}</MetaDataID>
        public void LockStateChange(object sender)
        {
            AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
        }


        /// <MetaDataID>{257C57BC-2015-4602-8ED8-E0A949AE1128}</MetaDataID>
        public void LoadControlValues()
        {

            if (UserInterfaceObjectConnection != null)
            {
                object ObjectDisplayedValue = null;
                object ObjectMemberDisplayedValue = null;

                Type type = this.UserInterfaceObjectConnection.GetType(Path as string);
                if (type == null)
                    return;
                //if (_AutoDisable && !DesignMode)
                //    Enabled = UserInterfaceObjectConnection.CanAccessValue(_Path, this);
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);


                UserInterfaceObjectConnection.ReleaseDataPathNodes(this);
                bool returnValueAsCollection = false;
                ObjectDisplayedValue = this.UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                _Value = ObjectDisplayedValue;

                if (ValueType != PresentationObjectType && PresentationObjectType != null)
                    _PresentationObject = this.UserInterfaceObjectConnection.GetPresentationObject(ObjectDisplayedValue, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType.GetExtensionMetaObject(typeof(Type)) as Type);

                type = null;
                Text = "";
                if (!string.IsNullOrEmpty(_DisplayMember))
                {
                    type = UserInterfaceObjectConnection.GetType(PresentationObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type, _DisplayMember);

                    if (type != null)
                    {

                        ObjectMemberDisplayedValue = this.UserInterfaceObjectConnection.GetDisplayedValue(PresentationObject, PresentationObjectType, _DisplayMember, this, out returnValueAsCollection);
                        if (ObjectMemberDisplayedValue != null)
                            Text = ObjectMemberDisplayedValue.ToString();
                    }
                }
                else
                {
                    if (PresentationObject != null)
                        Text = PresentationObject.ToString();
                }
            }

        }

        /// <MetaDataID>{9D6D57E0-6484-4A8D-8F13-A84FF5921EAF}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            if (memberChangeEventArg.Type == OOAdvantech.UserInterface.Runtime.ChangeType.ValueChanged)
                LoadControlValues();
            else if (memberChangeEventArg.Type == OOAdvantech.UserInterface.Runtime.ChangeType.LockChanged)
                AutoProducedEnabledValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);


        }

        /// <MetaDataID>{AB7CD802-4427-402B-8F21-EB43B7858759}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {

            if ((metaObject is OOAdvantech.UserInterface.OperationCall) &&
            (propertyDescriptor == "AllowDropOperationCall" || propertyDescriptor == "OperationCall" || propertyDescriptor == "DragDropOperationCaller" ||
            propertyDescriptor == "RemoveOperationCall" || propertyDescriptor == "InsertOperationCall"))
            {

                OOAdvantech.MetaDataRepository.Operation operation = new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation ;

                if (propertyDescriptor == "AllowDropOperationCall")
                {
                    if (operation.ReturnType == null || operation.ReturnType.FullName != typeof(OOAdvantech.DragDropMethod).FullName)
                        return false;
                    if (operation.Parameters.Count != 1)
                        return false;
                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                    {
                        if (parameter.Type == null || parameter.Type.FullName != typeof(object).FullName)
                            return false;
                        else
                            return true;
                    }
                    return false;
                }
                return true;
            }
            if (metaObject is OOAdvantech.MetaDataRepository.AssociationEnd || propertyDescriptor == "Path")
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.BehavioralFeature || propertyDescriptor == "Operation")
                return true;

            else if (metaObject is OOAdvantech.MetaDataRepository.Feature || propertyDescriptor == "DisplayMember")
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.Class && propertyDescriptor == "AssignPresentationObjectType")
            {

                OOAdvantech.MetaDataRepository.Classifier _valueType = null;
                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path))
                    _valueType = this.UserInterfaceObjectConnection.GetClassifier(_Path as string);

                OOAdvantech.MetaDataRepository.Operation searchOperation = CollectionView.SearchOperation;
                if (searchOperation != null)
                {
                    foreach (OOAdvantech.MetaDataRepository.Operation operation in searchOperation.ReturnType.GetOperations("GetEnumerator"))
                    {
                        OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
                        if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
                            _valueType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                    }
                }
                if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(metaObject as OOAdvantech.MetaDataRepository.Class, _valueType))
                {
                    System.Windows.Forms.MessageBox.Show("You can't assign the '" + metaObject.FullName + "' to the PresentationObjectType.\n Check the property rules.");
                    return false;
                }
                else
                    return true;

            }
            else
                return false;
            //throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{694FB8ED-5C9A-4FFC-A9FB-3751CC835E54}</MetaDataID>
        public void SaveControlValues()
        {

        }

        /// <MetaDataID>{86d2b185-04c2-4f77-9ebf-a304500ebc47}</MetaDataID>
        private string _Path;
        /// <MetaDataID>{8c0d3ea6-8873-4b01-ac21-d22e55787060}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
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
                if (DesignMode)
                    Invalidate();

            }
        }


        /// <MetaDataID>{61369556-90c8-452c-9020-c794a7cd13f2}</MetaDataID>
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

        #endregion
    }
    /// <MetaDataID>{6552F838-3F85-4BE3-8BCA-619C1C0957A1}</MetaDataID>
    [DesignTimeVisible(false),
    ToolboxItem(false)]
    public class SearchButton : Button
    {

        /// <MetaDataID>{3AE86441-67FA-409E-9F81-D3E9DC4CE29B}</MetaDataID>
        static Bitmap SearchButtonImage;
        /// <MetaDataID>{C44B9F71-6E38-450D-AB29-1B6FAFE904CD}</MetaDataID>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (SearchButtonImage == null)
                SearchButtonImage = new Bitmap(typeof(SearchButton).Assembly.GetManifestResourceStream("ConnectableControls.Search.bmp"));

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorKey(SearchButtonImage.GetPixel(0, 0), SearchButtonImage.GetPixel(0, 0));
            pevent.Graphics.DrawImage(SearchButtonImage, ClientRectangle, 0, 0, SearchButtonImage.Width, SearchButtonImage.Height,
                                            GraphicsUnit.Pixel, imageAttributes);
        }
    }
}
