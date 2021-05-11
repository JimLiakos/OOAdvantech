using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using ConnectableControls.PropertyEditors;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
using DragDropActionManager = OOAdvantech.UserInterface.Runtime.DragDropActionManager;
using DragDropTransactionOptions = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions;
using System.Xml.Linq;


namespace ConnectableControls
{
    /// <MetaDataID>{BFC02484-1490-4BA9-B938-C9038B65385D}</MetaDataID>
    public class ComboBox : System.Windows.Forms.ComboBox, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl, OOAdvantech.UserInterface.Runtime.IPathDataDisplayer, OOAdvantech.UserInterface.Runtime.ICutCopyPasteMoveDragDrop
    {
        /// <MetaDataID>{f502bcff-62e3-4a11-8ba2-b80646f16f4e}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(OOAdvantech.UserInterface.Runtime.ViewControlObjectState oldState, OOAdvantech.UserInterface.Runtime.ViewControlObjectState newState)
        {


        }
        /// <MetaDataID>{3c8734ed-1fa0-4538-b927-312eb88beb82}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{a96a76bf-c54d-4325-b02d-bf55978b167c}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }
        /// <MetaDataID>{522ccec8-ebff-40bb-bcbe-1e5f3f936a17}</MetaDataID>
        public virtual void InitializeControl()
        {

        }
        /// <MetaDataID>{a69206fd-6bde-4553-8034-26b3116a45ff}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return false;
        }
        /// <MetaDataID>{6b21b994-dab7-42a4-afcf-01433f2b1015}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {

                return false;
            }
        }

        /// <exclude>Excluded</exclude>
        DependencyProperty _EnableProperty;
        /// <MetaDataID>{b146693d-52f1-44eb-a058-f312ffe51a3c}</MetaDataID>
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




        /// <MetaDataID>{31b3eb0f-888d-4ed8-a4cd-e6d3602a102e}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _AllPaths;

        /// <MetaDataID>{fadb91e4-0430-4c26-9cde-b0962f5ad78b}</MetaDataID>
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


        /// <MetaDataID>{3bc3dc8e-728b-4089-82de-27821285e855}</MetaDataID>
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
        /// <MetaDataID>{b76877b4-557b-46a2-a381-9a2fc52e5323}</MetaDataID>
        internal Dictionary<string, object> ExtraProperties = new Dictionary<string, object>();

        /// <MetaDataID>{24e81357-9d41-4131-9b6f-6081e41e1fc9}</MetaDataID>
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

            if (propertyName == "DragDropObject")
                return true;


            //if (propertyName == "this.Text")
            //    return true;

            if (propertyName == "Text")
                return true;

            return ExtraProperties.ContainsKey(propertyName);


        }
        /// <MetaDataID>{0cdcd981-533d-45fd-8aca-2a08e2248963}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            if (propertyName == "PresentationObject")
                return PresentationObject;

            if (propertyName == "DragDropObject")
                return DragDropObject;


            if (propertyName == "Text")
            {
                if (Text == this.NullValueName)
                    return "";
                return Text;
            }

            if (ExtraProperties.ContainsKey(propertyName))
                return ExtraProperties[propertyName];

           
            //if (propertyName == "this.Text")
            //{
            //    if (Text == this.NullValueName)
            //        return "";
            //    return Text;
            //}

            if (propertyName == "this")
                return this;

            //if (propertyName == "this.Value")
            //    return Value;

            if (propertyName == "Value")
                return Value;

            throw new Exception("There isn't property with name " + propertyName + ".");

        }

        /// <MetaDataID>{2b3f2b5a-9655-4ebd-8406-ce5a792cc9d9}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {
            if (propertyName == "Value")
                Value = value;
            else
                throw new Exception("The method or operation is not implemented.");

        }

        /// <MetaDataID>{0b17dd37-52d6-45f9-9999-1fff09217c51}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            if (propertyName == "PresentationObject")
                return PresentationObjectType;

            if (propertyName == "DragDropObject")
                return OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(typeof(System.Object));


            if (propertyName == "Text")
                return OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(typeof(System.String));

            if (propertyName == "Value")
                return ValueType;
            if (ExtraProperties.ContainsKey(propertyName))
            {
                object propertyValue = ExtraProperties[propertyName];
                if (propertyValue != null)
                    return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(propertyValue.GetType());

            }
            return null;
        }
        /// <MetaDataID>{853693ed-3e86-4aff-98ff-df213893c3d1}</MetaDataID>
        public virtual bool CanDisplayDataOfType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return true;
        }





        /// <MetaDataID>{c0996061-6771-4a7f-900e-2075333c686d}</MetaDataID>
        public virtual bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {

            bool hasErrors = false;
            if (!string.IsNullOrEmpty(_Path) && ValueType == null)
            {
                if (FindForm() != null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' has invalid connection path.", FindForm().GetType().FullName));
                else
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' has invalid connection path.", (UserInterfaceObjectConnection.ContainerControl as Control).Name));

                hasErrors = true;
            }


            if (!string.IsNullOrEmpty(_DisplayMember))
            {
                Type type = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetType(PresentationObjectType.GetExtensionMetaObject(typeof(System.Type)) as System.Type, _DisplayMember);
                if (type == null)
                {
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' has lost the DisplayMember property connection", FindForm().GetType().FullName));
                    hasErrors = true;
                }
            }
            if (CollectionView.SearchOperationCaller != null)
            {

                foreach (string error in CollectionView.SearchOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + ".OperationCall' " + error, FindForm().GetType().FullName));
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
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + ".InsertOperationCall' " + error, FindForm().GetType().FullName));
                }
            }
            if (CollectionView.RemoveOperationCaller != null)
            {
                foreach (string error in CollectionView.RemoveOperationCaller.CheckConnectionMetaData())
                {
                    hasErrors = true;
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + ".RemoveOperationCall' " + error, FindForm().GetType().FullName));
                }
            }

            if (!string.IsNullOrEmpty(PresentationObjectTypeFullName))
            {
                _PresentationObjectType = ViewControlObject.UserInterfaceObjectConnection.GetClassifier(PresentationObjectTypeFullName, true) as OOAdvantech.MetaDataRepository.Class;
                if (_PresentationObjectType == null)
                    errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' has invalid PresentationObject.", FindForm().GetType().FullName));
                else
                {
                    if (!OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Class>.CanPresent(_PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType))
                        errors.Add(new OOAdvantech.MetaDataRepository.MetaObject.MetaDataError("UI Error: ComboBox '" + Name + "' You can't assign the '" + _PresentationObjectType.FullName + "' to the PresentationObjectType.\n Check the property rules.", FindForm().GetType().FullName));
                }
            }

            return false;
        }




        /// <MetaDataID>{064F51A0-F3A0-4062-B159-A5F938AE3701}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.Name == "Enumeration" && UserInterfaceObjectConnection != null)
                return AssemblyManager.GetActiveWindowProject();

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
        /// <MetaDataID>{67E9A18F-F096-4A07-9B88-CA3C58C05249}</MetaDataID>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {

            if (!DroppedDown && Focused && !LoadingControlData)
            {

                if (SelectedIndex >= 0)
                {
                    Text = Items[SelectedIndex].ToString();
                    if (_Value != (Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject)
                    {
                        _Value = (Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject;

                        if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(Path as string))
                        {
                            if (ValueType == _PresentationObjectType && _PresentationObjectType != null)
                            {
                                this.UserInterfaceObjectConnection.SetValue(((Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject as OOAdvantech.UserInterface.Runtime.IPresentationObject).GetRealObject(), Path.ToString());
                            }
                            else
                                this.UserInterfaceObjectConnection.SetValue((Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject, Path.ToString());


                            LoadControlValues();
                        }
                        else
                        {
                            int ttr = 0;
                        }
                    }

                }

            }
            else if (_Value == null && string.IsNullOrEmpty(_Path))
            {
                _Value = (Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject;
            }

            base.OnSelectedIndexChanged(e);

        }
        /// <MetaDataID>{d2a9889e-4074-4955-8569-650f18c416a4}</MetaDataID>
        object _PresentationObject;
        /// <MetaDataID>{5fd632f6-3cdf-4b05-8edb-9fa01825593c}</MetaDataID>
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

        /// <MetaDataID>{2d409a46-8ec4-434e-be55-eb7096a02891}</MetaDataID>
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

        /// <MetaDataID>{88d3c434-8ee5-45cb-8c60-cbdd16756942}</MetaDataID>
        string PresentationObjectTypeFullName;
        /// <MetaDataID>{af6ee4ab-4007-421c-a989-5176aa77eafc}</MetaDataID>
        OOAdvantech.MetaDataRepository.Class _PresentationObjectType;
        /// <MetaDataID>{89d977ca-2900-4dc5-8f09-a14097261f60}</MetaDataID>
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



        /// <MetaDataID>{c805b472-10c1-41d9-91ee-7ac0105dc3ae}</MetaDataID>
        bool PreLoadedExecuted = false;
        /// <MetaDataID>{b42e748b-a93e-45b4-85d0-928115991551}</MetaDataID>
        bool _PreLoaded = true;
        /// <MetaDataID>{e9fb2481-a3ec-4c55-a426-843e9e82a6ff}</MetaDataID>
        [Category("Object Model Connection")]
        public bool PreLoaded
        {
            get
            {
                return _PreLoaded;
            }
            set
            {
                _PreLoaded = value;
            }
        }

        /// <MetaDataID>{9ed13fd3-610a-4095-a133-7b041c991306}</MetaDataID>
        string EnumFullName;
        /// <MetaDataID>{2120598c-eb46-40ad-8d8c-6f55e71aed2b}</MetaDataID>
        OOAdvantech.MetaDataRepository.Enumeration _Enumeration;
        /// <MetaDataID>{7b17f8bd-b24e-41c0-98d6-d56a90d892db}</MetaDataID>
        [Category("Object Model Connection")]
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object Enumeration
        {
            get
            {
                if (_Enumeration == null && !string.IsNullOrEmpty(EnumFullName) && this.UserInterfaceObjectConnection != null)
                    _Enumeration = this.UserInterfaceObjectConnection.GetClassifier(EnumFullName, true) as OOAdvantech.MetaDataRepository.Enumeration;

                if (_Enumeration == null)
                    return "";
                else
                    return _Enumeration.FullName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value as string))
                    EnumFullName = value as string;
                else if (value is OOAdvantech.MetaDataRepository.Enumeration)
                    _Enumeration = value as OOAdvantech.MetaDataRepository.Enumeration;
                else if (value is MetaData)
                    _Enumeration = (value as MetaData).MetaObject as OOAdvantech.MetaDataRepository.Enumeration;


            }
        }


        /// <MetaDataID>{8436644e-0ed4-42dc-904a-4192586378e9}</MetaDataID>
        bool _ChooseFromEnum;
        /// <MetaDataID>{1f58a390-8ec0-460e-b2ad-b3245f5d10e9}</MetaDataID>
        [Category("Object Model Connection")]
        public bool ChooseFromEnum
        {
            get
            {
                try
                {
                    if (ValueType != null && !(ValueType is OOAdvantech.MetaDataRepository.Enumeration))
                        return _ChooseFromEnum;
                    else
                    {
                        if ((ValueType is OOAdvantech.MetaDataRepository.Enumeration) && _AutoInsert)
                        {
                            TypeDescriptor.GetProperties(this).Find("AutoInsert", false).SetValue(this, false);
                            if (DesignMode)
                                System.Windows.Forms.MessageBox.Show("The property AutoInsert can't be active when the type of path member is enum.\nThe system will clear the AutoInsert property", "ComboBox " + Name);

                        }
                        return _ChooseFromEnum;
                    }
                }
                catch (System.Exception error)
                {
                    System.Windows.Forms.MessageBox.Show(error.Message + "\n" + error.StackTrace);
                    throw;
                }

            }
            set
            {
                try
                {
                    if ((ValueType == null || !(ValueType is OOAdvantech.MetaDataRepository.Enumeration)) && value && DesignMode)
                    {
                        if (Site != null && (Site.Container as System.ComponentModel.Design.IDesignerHost) != null && (Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                            _ChooseFromEnum = value;
                        else
                            System.Windows.Forms.MessageBox.Show("The type of path member isn't enum .", "ComboBox " + Name);
                    }
                    else
                        _ChooseFromEnum = value;
                }
                catch (System.Exception error)
                {
                    System.Windows.Forms.MessageBox.Show(error.Message + "\n" + error.StackTrace);
                    throw;
                }
            }

        }
        /// <MetaDataID>{28EE44C6-03CE-41CC-A61C-1C758E076FFF}</MetaDataID>
        protected override void OnSelectedItemChanged(EventArgs e)
        {
            base.OnSelectedItemChanged(e);
        }
        /// <MetaDataID>{b06d19cb-ee66-4757-b09f-7a65f1ab366e}</MetaDataID>
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

        /// <MetaDataID>{f1c7f06d-8a9e-4aaf-aac5-420d1a40d3c6}</MetaDataID>
        UpdateStyle _UpdateStyle;
        /// <MetaDataID>{9e52fa1b-a4af-4d4d-af75-f519046acd25}</MetaDataID>
        public UpdateStyle UpdateStyle
        {
            get
            {
                return _UpdateStyle;
            }
            set
            {

                _UpdateStyle = value;
            }
        }



        /// <MetaDataID>{ee7b6ed7-ecb0-41ac-83f4-dab87e044679}</MetaDataID>
        DropDownContainer.ListBoxNoneItem NullListBoxItem = new DropDownContainer.ListBoxNoneItem("");
        /// <MetaDataID>{cb85c068-eaeb-44f8-9d79-edca497400ae}</MetaDataID>
        System.Collections.ArrayList LoadedItems = new System.Collections.ArrayList();
        /// <MetaDataID>{db8c22c6-b1b7-40da-968e-c8a718da6bea}</MetaDataID>
        void FilterItems()
        {
            FilterItems(Text);
        }

        /// <MetaDataID>{aa35f9ea-c694-47f1-9b20-5da734d3cc05}</MetaDataID>
        void FilterItems(string text)
        {
            if (AutoSuggest)
            {
                #region Filter choices
                bool addNullItem = false;

                //addNullItem = true;

                foreach (DropDownContainer.ListBoxItem listBoxItem in LoadedItems)
                {
                    if (string.IsNullOrEmpty(text) || listBoxItem.ToString().ToLower().StartsWith(text.ToLower()))
                    {
                        if (!Items.Contains(listBoxItem))
                            Items.Add(listBoxItem);
                    }
                    else
                    {
                        if (Items.Contains(listBoxItem))
                            Items.Remove(listBoxItem);
                    }
                }

                System.Collections.ArrayList cloneItems = new System.Collections.ArrayList(Items);
                foreach (DropDownContainer.ListBoxItem listBoxItem in cloneItems)
                {
                    if (!string.IsNullOrEmpty(text) && !listBoxItem.ToString().ToLower().StartsWith(text.ToLower()))
                    {
                        if (Items.Contains(listBoxItem))
                            Items.Remove(listBoxItem);
                    }



                }
                if (ValueType != null && ValueType is OOAdvantech.MetaDataRepository.Enumeration)
                    addNullItem = false;

                if (string.IsNullOrEmpty(text) && !Items.Contains(NullListBoxItem))
                    Items.Add(NullListBoxItem);
                #endregion
            }
            else
            {
                #region Add all
                bool addNullItem = false;
                if (Items.Count == 0)
                    addNullItem = true;

                foreach (DropDownContainer.ListBoxItem listBoxItem in LoadedItems)
                {
                    if (!Items.Contains(listBoxItem))
                        Items.Add(listBoxItem);
                }
                if (ValueType != null && ValueType is OOAdvantech.MetaDataRepository.Enumeration)
                    addNullItem = false;

                if (addNullItem)
                    Items.Add(NullListBoxItem);

                #endregion
            }
            if (Items.Count == 0)
                SelectedIndex = -1;
            //else
            //    SelectedIndex = 0;

            DropDownHeight = DropDownHeight;

        }

        /// <MetaDataID>{7041FDF8-9963-44C5-956B-DC1BE52A63C5}</MetaDataID>
        void LoadItems()
        {
            if (DesignMode)
                return;

            if (ChooseFromEnum)
            {
                Type type = ValueType.GetExtensionMetaObject(typeof(Type)) as System.Type;
                LoadedItems.Clear();
                Items.Clear();
                foreach (System.Reflection.FieldInfo field in type.GetFields())
                {
                    if (field.IsLiteral)
                    {
                        string valueText = field.Name;
                        if (ConnectableControls.ViewControlObject.Translator != null)
                            valueText = ConnectableControls.ViewControlObject.Translator.Translate(field.DeclaringType.FullName + "." + field.Name);
                        if (valueText == null)
                            valueText = field.Name;

                        LoadedItems.Add(new DropDownContainer.ListBoxItem(field.GetValue(null), null, valueText));
                    }
                }

            }
            else
            {
                try
                {
                    //System.Reflection.MethodInfo methodInfo = CollectionView.SearchMethod;
                    if (UserInterfaceObjectConnection == null || CollectionView.SearchMethod == null)
                        return;

                    OOAdvantech.UserInterface.Runtime.IOperationCallerSource control = CollectionView.SearchOperationCaller.ReturnDestinationControl;


                    if (control == this || string.IsNullOrEmpty(CollectionView.SearchOperationCaller.OperationCall.ReturnValueDestination))
                    {
                        if (CollectionView.SearchOperationCaller.CheckConnectionMetaData().Count > 0 || !CanItAccept(CollectionView.SearchOperationCall, "OperationCall"))
                        {
                            System.Diagnostics.Debug.WriteLine("ComboBox [" + Name + "] OperationCall signature error");
                            return;
                        }

                        object returnValue = CollectionView.InvokeSearchOperation();

                        LoadedItems.Clear();
                        Items.Clear();
                        if (returnValue == null)
                            return;
                        bool tmp = false;
                        if (tmp)
                            CollectionView.SearchOperationCaller.CheckConnectionMetaData();


                        System.Collections.IEnumerator enumerator = returnValue.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(returnValue, new object[0]) as System.Collections.IEnumerator;
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

                        //foreach (string path in UserInterfaceObjectConnection.RootObjectNode.GetSubPaths(_Path))
                        //    paths.Add("Root." + path);
                        foreach (string path in CollectionView.SearchOperationCaller.ExtraPaths())
                            paths.Add("Root." + path);
                        // {ProjectManagement.InventoryAccounting.Account}	OOAdvantech.MetaDataRepository.Classifier {OOAdvantech.DotNetMetaDataRepository.Class}

                        UserInterfaceObjectConnection.BatchLoadPathsValues(enumerator, CollectionObjectType.GetExtensionMetaObject(typeof(Type)) as Type, paths);

                        enumerator.Reset();

                        while (enumerator.MoveNext())
                        {
                            object obj = enumerator.Current;
                            OOAdvantech.MetaDataRepository.Classifier type = null;
                            if (!string.IsNullOrEmpty(_DisplayMember))
                                type = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(PresentationObjectType, DisplayMember.ToString());
                            bool returnValueAsCollection = false;
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

                            if (type != null)
                            {
                                object displayedMemberValue = null;

                                if (presentationObject != null)
                                    displayedMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(presentationObject, PresentationObjectType, _DisplayMember, null, out returnValueAsCollection);
                                else
                                    displayedMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(obj, ValueType, _DisplayMember, null, out returnValueAsCollection);

                                if (displayedMemberValue != null)
                                    LoadedItems.Add(new DropDownContainer.ListBoxItem(obj, presentationObject, displayedMemberValue.ToString()));
                            }
                            else
                                LoadedItems.Add(new DropDownContainer.ListBoxItem(obj, presentationObject, text));


                        }
                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            if (Items.Count > 0)
                SelectedIndex = 0;

        }

        #region DragDrop Behavior

        /// <MetaDataID>{091bb927-69b0-48f0-abd7-11842a11c62c}</MetaDataID>
        public void CutObject(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <MetaDataID>{924b763f-060d-4386-8b9d-9eae55a835ae}</MetaDataID>
        public void PasteObject(object dropObject)
        {
            if (UserInterfaceObjectConnection != null && Path != null && ValueType != null && (ValueType.GetExtensionMetaObject(typeof(System.Type)) as System.Type).IsInstanceOfType(dropObject))
            {
                this.UserInterfaceObjectConnection.SetValue(dropObject, Path.ToString());
                LoadValue();
            }
        }
        /// <MetaDataID>{c344dbb3-c9dc-43e3-b8ab-292a89930bd2}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _AllowDropOperationCaller;
        /// <MetaDataID>{bf872aa0-a87e-466f-bb69-c3e5b670021e}</MetaDataID>
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
        /// <MetaDataID>{ab064aa0-9743-4ac1-9883-6e8b5c29bf47}</MetaDataID>
        bool _AllowDrag = false;
        /// <MetaDataID>{8f0181c1-9129-4e6a-9beb-2b6b544c2d54}</MetaDataID>
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

        /// <MetaDataID>{882749ac-9ead-4d3d-bfcc-deea3b08d7c4}</MetaDataID>
        XDocument AllowDropOperationCallMetaData;
        /// <MetaDataID>{7828884d-7b30-4179-a927-497ed75a02f7}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _AllowDropOperationCall;
        /// <MetaDataID>{0ff477b3-32ca-4fe7-9e20-d25f0db0f600}</MetaDataID>
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

        /// <MetaDataID>{bb5e75ea-f8d6-4494-ad2d-51d5e8767dde}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _DragDropOperationCaller;
        /// <MetaDataID>{b9432e61-d128-4ed1-9550-b766922d9316}</MetaDataID>
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

        /// <MetaDataID>{3a82048b-e92a-4114-911d-c1d2d3526467}</MetaDataID>
        XDocument DragDropOperationCallMetaData;
        /// <MetaDataID>{497f909c-7389-4e4e-bdbe-b022b81aaaca}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _DragDropOperationCall;
        /// <MetaDataID>{3abb9480-de1d-4cfd-a3a3-f267aff5d2bd}</MetaDataID>
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

        /// <MetaDataID>{3f06e9e5-b672-47f4-913f-e3941d33b5ca}</MetaDataID>
        DragDropTransactionOptions _DragDropTransactionOption;
        /// <MetaDataID>{19572d2a-0049-43f4-ae9d-956830479ad6}</MetaDataID>
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


        /// <MetaDataID>{d16429dd-5551-4210-921c-9f3660f8739b}</MetaDataID>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (AllowDrag && System.Windows.Forms.Control.MouseButtons == MouseButtons.Left)
            {
                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, Value, DragDropTransactionOption);
                DoDragDrop(dragDropActionManager, DragDropEffects.Copy | DragDropEffects.Move);
            }

        }
        /// <MetaDataID>{ae6b601b-45bb-46b3-bab4-0159b6df2e28}</MetaDataID>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            System.Drawing.Point mousePos = System.Windows.Forms.Control.MousePosition;
            if (Parent != null && !Parent.RectangleToScreen(new System.Drawing.Rectangle(Location, Size)).Contains(System.Windows.Forms.Control.MousePosition)
                && e.Button == MouseButtons.Left && AllowDrag)
            {
                DragDropActionManager dragDropActionManager = new DragDropActionManager(this, Value, DragDropTransactionOption);
                DoDragDrop(dragDropActionManager, DragDropEffects.Copy | DragDropEffects.Move);

            }

        }

        /// <MetaDataID>{b8bb24ee-af78-4ab9-a0f1-7bfcffc51d4a}</MetaDataID>
        bool _dragMode = false;

        /// <MetaDataID>{8e84f3ae-3bb3-4c3e-b2e3-2a2811340c19}</MetaDataID>
        object DragDropObject;
        /// <MetaDataID>{3990c153-5327-4f9a-94b3-848cb979270e}</MetaDataID>
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

        /// <MetaDataID>{ad77a730-777f-49a6-9bc6-ee7edc21b028}</MetaDataID>
        protected override void OnDragLeave(EventArgs e)
        {
            _dragMode = false;
            DragDropObject = null;
            base.OnDragLeave(e);
        }

        /// <MetaDataID>{fde6fd86-1b40-494b-81b5-96c1fae026d0}</MetaDataID>
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

        /// <MetaDataID>{CDC65010-8480-4A2C-8442-33223082AFE1}</MetaDataID>
        protected override void OnDropDown(EventArgs e)
        {
            if (!PreLoaded)
            {
                LoadItems();
                FilterItems();
            }
            if (PreLoaded && !PreLoadedExecuted)
            {
                LoadItems();
                PreLoadedExecuted = true;
                FilterItems();
            }


        }


        /// <MetaDataID>{e0074684-c03b-47d2-a837-442e5b7f2762}</MetaDataID>
        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                base.SelectedIndex = value;
            }
        }
        /// <MetaDataID>{68facd43-ed32-4e73-b915-e73eb15bf7d8}</MetaDataID>
        protected override void OnLostFocus(EventArgs e)
        {

            try
            {
                if (DesignMode)
                    return;
                if (UserInterfaceObjectConnection == null)
                    return;
                if (UserInterfaceObjectConnection.State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.Passive)
                    return;
                #region Calls the insert operation to add a new object in choices collection
                if (Items.Count == 0)
                    SelectedIndex = -1;
                //string text="";
                //try
                //{
                //    text = Text;
                //}
                //catch (System.Exception error)
                //{
                //    SelectedIndex = -1;

                //}

                if (AutoInsert && !string.IsNullOrEmpty(Text) && SelectedIndex == -1)
                    InsertNewItem();
                LoadValue();
                #endregion
            }
            finally
            {
                base.OnLostFocus(e);
            }


        }
        /// <MetaDataID>{6cbb0531-e18d-46b9-a463-23c97c6ee275}</MetaDataID>
        bool SuspendUpdate = false;
        /// <MetaDataID>{B936CF27-44C9-43A7-B4C1-B2E232BE65E3}</MetaDataID>
        protected override void OnDropDownClosed(EventArgs e)
        {
            try
            {
                SuspendUpdate = true;
                if (Items.Count == 0)
                    SelectedIndex = -1;



                if (SelectedIndex >= 0 && !IgnoreDropDown && Items.Count > 0)
                {
                    RunFilterFlag = false;
                    if ((Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject != _Value)
                    {
                        _Value = (Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject;
                        if (UserInterfaceObjectConnection != null && Path != null)
                        {
                            this.UserInterfaceObjectConnection.SetValue((Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject, Path.ToString());
                        }
                        //  this.ViewControlObject.SetValue((Items[SelectedIndex] as DropDownContainer.ListBoxItem).DisplayiedObject, Path.ToString());
                    }
                    if (SelectedIndex != -1)
                        Text = Items[SelectedIndex].ToString();
                    else
                        Text = "";

                }

                #region Calls the insert operation to add a new object in choices collection
                try
                {
                    //if (AutoInsert && !string.IsNullOrEmpty(Text) && SelectedIndex == -1)
                    //    InsertNewItem();
                }
                catch (Exception error)
                {
                    Text = "";
                    throw;
                }
                #endregion

            }
            catch (Exception error)
            {

                throw;
            }
            finally
            {
                SuspendUpdate = false;
            }


        }

        /// <MetaDataID>{4710f944-4c4e-4dba-aa16-bdfbd2cad847}</MetaDataID>
        private void InsertNewItem()
        {

            //if (Text == this.NullValueName)
            //    return;
            if (CollectionView.InsertOperationCaller != null && CollectionView.InsertOperationCaller.Operation != null)
            {
                object newObject = CollectionView.InsertOperationCaller.Invoke();


                if (newObject != null)
                {
                    //presentation


                    OOAdvantech.MetaDataRepository.Classifier type = null;
                    if (!string.IsNullOrEmpty(_DisplayMember))
                        type = OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection.GetClassifier(PresentationObjectType, DisplayMember.ToString());

                    this.UserInterfaceObjectConnection.Control(newObject);
                    object presentationObject = null;
                    string text = null;
                    if (newObject != null)
                        text = newObject.ToString();

                    if (ValueType != PresentationObjectType && PresentationObjectType != null)
                    {
                        presentationObject = this.UserInterfaceObjectConnection.GetPresentationObject(newObject, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                        if (presentationObject != null)
                            text = presentationObject.ToString();
                    }

                    if (type != null)
                    {
                        object displayedMemberValue = null;
                        bool returnValueAsCollection = false;
                        if (presentationObject != null)
                            displayedMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(presentationObject, PresentationObjectType, _DisplayMember, null, out returnValueAsCollection);
                        else
                            displayedMemberValue = UserInterfaceObjectConnection.GetDisplayedValue(newObject, ValueType, _DisplayMember, null, out returnValueAsCollection);

                        if (displayedMemberValue != null)
                        {
                            if (Items.Count > 0)
                            {
                                Items.Insert(Items.Count - 1, new DropDownContainer.ListBoxItem(newObject, presentationObject, displayedMemberValue.ToString()));
                                SelectedIndex = Items.Count - 2;
                            }
                            else
                                SelectedIndex = Items.Add(new DropDownContainer.ListBoxItem(newObject, presentationObject, displayedMemberValue.ToString()));
                        }
                    }
                    else
                    {
                        if (Items.Count > 0)
                        {
                            Items.Insert(Items.Count - 1, new DropDownContainer.ListBoxItem(newObject, presentationObject, text));
                            SelectedIndex = Items.Count - 2;
                        }
                        else
                            SelectedIndex = Items.Add(new DropDownContainer.ListBoxItem(newObject, presentationObject, text));
                    }
                    _Value = newObject;
                    _PresentationObject = presentationObject;
                    if (UserInterfaceObjectConnection != null && Path != null)
                        this.UserInterfaceObjectConnection.SetValue(newObject, _Path);
                }
            }
        }

        /// <MetaDataID>{fb800268-48ec-4167-b43f-cea428952112}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection = null;
        /// <MetaDataID>{45e7d80a-0a7a-4dcb-bfa8-bd2c83a0d854}</MetaDataID>
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
                _UserInterfaceObjectConnection = value;
                if (_UserInterfaceObjectConnection != null)
                    _UserInterfaceObjectConnection.AddControlledComponent(this);

            }
        }
        /// <MetaDataID>{abdfe140-1d66-48de-a214-418fb4124a88}</MetaDataID>
        [Category("Object Model Connection")]
        public ViewControlObject ViewControlObject
        {
            get
            {
                try
                {
                    if (UserInterfaceObjectConnection == null)
                        return null;
                    return UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject;
                }
                catch (System.Exception error)
                {
                    throw;

                }

            }
            set
            {
                if (value != null)
                    UserInterfaceObjectConnection = value.UserInterfaceObjectConnection;
                else
                    UserInterfaceObjectConnection = null;
            }
        }

        /// <MetaDataID>{a8a5dbe4-8817-454c-b11f-3441cfc4c2c6}</MetaDataID>
        object _Value;
        /// <MetaDataID>{b17ca9d1-fcff-411c-be04-bd2719ca61f6}</MetaDataID>
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

        /// <MetaDataID>{316c9f88-2605-459d-b615-3182191cc9c6}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OOAdvantech.MetaDataRepository.Classifier ValueType
        {
            get
            {


                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(_Path))
                    return this.UserInterfaceObjectConnection.GetClassifier(_Path as string);

                return CollectionObjectType;


            }
        }
        /// <MetaDataID>{c7a8f79d-b29a-4f37-a0fa-7ae753a32977}</MetaDataID>
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

        //public OOAdvantech.MetaDataRepository.Classifier ValueType
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_Path) && _Enumeration != null)
        //            return _Enumeration;

        //        if (ViewControlObject != null && !string.IsNullOrEmpty(_Path as string))
        //            return this.ViewControlObject.GetClassifier(_Path as string);

        //        OOAdvantech.MetaDataRepository.Operation searchOperation = CollectionView.SearchOperation;
        //        if (searchOperation != null)
        //        {
        //            foreach (OOAdvantech.MetaDataRepository.Operation operation in searchOperation.ReturnType.GetOperations("GetEnumerator"))
        //            {
        //                OOAdvantech.MetaDataRepository.Classifier enumerator = operation.ReturnType;
        //                if (enumerator.Name.IndexOf("IEnumerator`1") == 0 && enumerator.TemplateBinding != null)
        //                    return enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
        //            }
        //        }
        //        if (_Enumeration != null)
        //            return _Enumeration;
        //        return null;
        //    }
        //    set
        //    {

        //    }
        //}

        /// <MetaDataID>{314a2fce-6e52-4965-842f-f17003f47e43}</MetaDataID>
        string _EnableCondition;
        /// <MetaDataID>{cbae1046-8b08-4b02-bbaa-8a4f2cdfad8d}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Object Model Connection")]
        public object EnableCondition
        {
            get
            {
                return _EnableCondition;
            }
            set
            {
                if (value is string)
                    _EnableCondition = value as string;
            }
        }

        /// <MetaDataID>{c862fdd0-7489-40f1-a055-2834ef2da035}</MetaDataID>
        string _Path;
        /// <MetaDataID>{306c3d4a-5dc9-49d5-a9f9-052f3b8ab868}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("System uses the path to reach and get the value of the control."),
        Category("Object Model Connection")]
        public object Path
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
                if (Site != null &&
                    Site.Container is System.ComponentModel.Design.IDesignerHost &&
                    (Site.Container as System.ComponentModel.Design.IDesignerHost).Loading)
                    return;
                if ((ValueType is OOAdvantech.MetaDataRepository.Enumeration) && _AutoInsert)
                {
                    TypeDescriptor.GetProperties(this).Find("AutoInsert", false).SetValue(this, false);
                    if (DesignMode)
                        System.Windows.Forms.MessageBox.Show("The property AutoInsert can't be active when the type of path member is enum.\nThe system will clear the AutoInsert property", "ComboBox " + Name);
                }


            }
        }
        /// <MetaDataID>{ab7a2794-f03d-4098-b244-8b0a24cb2a1d}</MetaDataID>
        private string _WarnigMessageOnRemove;
        /// <MetaDataID>{c217aa65-a5c9-46e0-9cd9-29a3bf8a7ef1}</MetaDataID>
        public string WarnigMessageOnRemove
        {
            get { return _WarnigMessageOnRemove; }
            set { _WarnigMessageOnRemove = value; }
        }
        /// <MetaDataID>{120789ee-8bb1-49a3-9247-9e37b3a0df50}</MetaDataID>
        private string _NullValueName;

        /// <MetaDataID>{a80ffe01-d5ad-49c0-a7c3-c4622bee3845}</MetaDataID>
        public string NullValueName
        {
            get { return _NullValueName; }
            set
            {
                _NullValueName = value;
                if (_NullValueName == null)
                    _NullValueName = "";
                NullListBoxItem = new DropDownContainer.ListBoxNoneItem(_NullValueName);
            }
        }


        #region ComboBox operations
        /// <MetaDataID>{8494a858-8aab-4832-911f-186e8210815e}</MetaDataID>
        CollectionView CollectionView;
        /// <MetaDataID>{f3d339af-9c18-4887-9f64-2b245d815b71}</MetaDataID>
        XDocument RemoveOperationCallMetaData;
        /// <MetaDataID>{e11004be-c18f-46de-87df-7dd1dc2c448e}</MetaDataID>
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
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(default(string), "none");
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
                    TypeDescriptor.GetProperties(this).Find("RemoveOperationCall", false).SetValue(this, InsertOperationCall);
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
                    if (DesignMode || !string.IsNullOrEmpty(metaData))
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
                        CollectionView.RemoveOperationCall = new OOAdvantech.UserInterface.OperationCall();

                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        CollectionView.RemoveOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;
                }
                return;
            }
        }
        /// <MetaDataID>{791deb5b-a27b-4791-8da0-f39015aaf6d2}</MetaDataID>
        public ComboBox()
        {
            CollectionView = new CollectionView(this);

            _EnableProperty = new DependencyProperty(this, "Enabled");
            _DependencyProperties.Add(_EnableProperty);
        }
        /// <MetaDataID>{904f2b98-fa2d-498e-a2ae-ae9ecd315abb}</MetaDataID>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            DropDownHeight = DropDownHeight;
        }









        /// <MetaDataID>{8762081e-c6e4-4f3b-a425-cd543402c377}</MetaDataID>
        XDocument InsertOperationCallMetaData;
        /// <MetaDataID>{46588dfe-2504-4c78-a175-a677881ece01}</MetaDataID>
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
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(InsertOperationCallMetaData.ToString() as string, "none");
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

        /// <MetaDataID>{18b5a541-1621-4a5c-967c-7d07185bd921}</MetaDataID>
        XDocument MetaDataAsXmlDocument;
        /// <MetaDataID>{ddc539fd-8753-4cc8-a4c2-d63f5eae2137}</MetaDataID>
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

                    UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                    if (CollectionView.SearchOperation == null)
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(default(string), "none");
                    else
                        metaDataVaue = new UserInterfaceMetaData.MetaDataValue(MetaDataAsXmlDocument.ToString(), CollectionView.SearchOperation.Name);

                    //MetaDataValue metaDataVaue = new MetaDataValue(MetaDataAsXmlDocument.OuterXml as string);
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
                    //if (metaData == null && DesignMode)
                    //    TypeDescriptor.GetProperties(this).Find("OperationCall", false).SetValue(this, value);

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
                        //TODO προσορινό πρέπει να διαγραφεί
                        CollectionView.SearchOperationCall.Name = "OperationCall";
                        break;
                    }
                    if (CollectionView.SearchOperationCall == null)
                        CollectionView.SearchOperationCall = listViewStorage.NewObject(typeof(OOAdvantech.UserInterface.OperationCall)) as OOAdvantech.UserInterface.OperationCall;

                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.OperationCall)
                        CollectionView.SearchOperationCall = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.OperationCall;

                }
                return;


                //if (value is MetaDataValue)
                //    _OperationCall = (value as MetaDataValue).MetaDataAsXML;
                //else
                //    _OperationCall = value;

                //if (_OperationCall as string != null && Parent != null)
                //{
                //    //foreach (Control control in ControlsAsSource)
                //    //{
                //    //    control.Invalidated -= new InvalidateEventHandler(SourceControlInvalidated);
                //    //}
                //    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                //    document.LoadXml(value as string);
                //    ControlsAsSource.Clear();
                //    string controlName = null;
                //    foreach (System.Xml.XmlElement parameterElement in document.DocumentElement.ChildNodes)
                //    {

                //        controlName = parameterElement.GetAttribute("Source");
                //        System.Windows.Forms.Control control = GetControlWithName(FindForm().Controls, controlName);
                //        if (control != null)
                //            ControlsAsSource[controlName] = control;

                //    }
                //    controlName = document.DocumentElement.GetAttribute("ReturnDestination");
                //    if (!string.IsNullOrEmpty(controlName))
                //    {
                //        System.Windows.Forms.Control control = GetControlWithName(FindForm().Controls, controlName);
                //        if (control != null)
                //            ControlsAsSource[controlName] = control;
                //    }
                //}
                //if (DesignMode)
                //    Invalidate();


            }
        }

        #endregion






        /// <MetaDataID>{c2ab5b3d-15fb-489c-bc24-c923d7f7dc3a}</MetaDataID>
        string _DisplayMember;
        /// <summary>
        /// In case where the search box is control for member which is object 
        /// and the object is not value type, the search display the return value of ToString() function call.
        /// If you want to display a member of object use this property to choose the member.
        ///  </summary>
        /// <MetaDataID>{e7979964-f740-4326-920f-e74c9d8aceb9}</MetaDataID>
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
        //DisplayedValue DisplayedValue;
        //DisplayedValue DisplayedObjectMember;



        /// <summary>
        /// If this flag is true then, when DropDown window closed the combobox ignore the new item selection 
        /// </summary>
        /// <MetaDataID>{0574dbc7-5c13-4fe5-adc6-d45c068397ff}</MetaDataID>
        bool IgnoreDropDown = false;
        /// <MetaDataID>{a38d2c22-9690-41d5-a1b8-0c93ea019645}</MetaDataID>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Down && !DroppedDown) || (e.KeyData == Keys.Up && !DroppedDown))
            {

                //           RunFilterFlag=true;
                //AutoSuggestion();
                FilterItems();
                DroppedDown = true;
                // e.Handled = true;
                return;
            }
            base.OnKeyDown(e);
            if (e.KeyData == Keys.Delete)
            {
                if (SelectedIndex != -1)
                    e.Handled = true;

                RemoveSelectedItem();
                FilterItems();
            }

        }

        /// <MetaDataID>{b77b5017-f7cb-44d0-9317-bc1c737b0e16}</MetaDataID>
        private void RemoveSelectedItem()
        {
            if (CollectionView.RemoveOperation == null || SelectedIndex == -1 || Items[SelectedIndex] is DropDownContainer.ListBoxNoneItem)
                return;
            if (!string.IsNullOrEmpty(WarnigMessageOnRemove))
            {
                if (System.Windows.Forms.MessageBox.Show(WarnigMessageOnRemove, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            object reVal = CollectionView.RemoveOperationCaller.Invoke();
            if (reVal == null || !(reVal is bool))
            {
                if (LoadedItems.Contains(Items[SelectedIndex]))
                    LoadedItems.Remove(Items[SelectedIndex]);
                Items.RemoveAt(SelectedIndex);
                _Value = null;
                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(Path as string))
                    UserInterfaceObjectConnection.SetValue(null, Path.ToString());
            }
            else if ((bool)reVal)
            {
                if (LoadedItems.Contains(Items[SelectedIndex]))
                    LoadedItems.Remove(Items[SelectedIndex]);
                Items.RemoveAt(SelectedIndex);
                _Value = null;
                if (UserInterfaceObjectConnection != null && !string.IsNullOrEmpty(Path as string))
                    UserInterfaceObjectConnection.SetValue(null, Path.ToString());
            }
            else
            {
                //int oldSelection = SelectedIndex;

                //SelectedIndex = -1;
                //SelectedIndex = oldSelection;

            }
        }
        /// <MetaDataID>{56FD9ADC-3CC2-49AC-A0F8-366597697AE6}</MetaDataID>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
            {
                e.Handled = true;
                return;
            }


            base.OnKeyUp(e);
            System.Diagnostics.Debug.WriteLine(e.KeyValue.ToString());
            if (e.KeyData == Keys.Insert && DropDownStyle != ComboBoxStyle.DropDownList && SelectedIndex == -1)
                InsertNewItem();
            //if (AutoSuggest)
            //    RefreshSuggestions(e.KeyData);





        }
        /// <MetaDataID>{5ff15a96-0311-4c88-ac37-10649840affd}</MetaDataID>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            RunFilterFlag = true;
            if (e.KeyChar == '\b' && Text.Length == 0)
                AutoSuggestion();


            base.OnKeyPress(e);
        }
        /// <MetaDataID>{85c081f0-d722-48ec-a57e-3bfe7d233ae9}</MetaDataID>
        protected override void OnTextChanged(EventArgs e)
        {
            try
            {
                base.OnTextChanged(e);
                SuspendUpdate = true;




                if (RunFilterFlag && AutoSuggest)
                    AutoSuggestion();
                else
                {
                    if (!string.IsNullOrEmpty(Path as string) && this.UserInterfaceObjectConnection.GetClassifier(Path.ToString()).FullName == typeof(string).FullName)
                        if (Value != null && Value.ToString() != Text)
                        {
                            if (Path != null && UpdateStyle == ConnectableControls.UpdateStyle.Immediately)
                            {
                                var classifier = this.UserInterfaceObjectConnection.GetClassifier(Path.ToString());
                                if (classifier != null && classifier.FullName == typeof(string).FullName)
                                    this.UserInterfaceObjectConnection.SetValue(Text, Path.ToString());
                            }
                        }
                }
            }
            catch (System.Exception error)
            {

            }
            SuspendUpdate = false;
        }
        /// <MetaDataID>{ec5f3326-7d70-49f2-b57a-978646a764b1}</MetaDataID>
        bool RunFilterFlag = false;
        /// <MetaDataID>{0d08c3a5-16eb-4773-9817-2916cd475294}</MetaDataID>
        void AutoSuggestion()
        {
            if (AutoSuggest)
            {
                RunFilterFlag = false;
                if (!string.IsNullOrEmpty(Text))
                {

                    #region Refresh suggestions
                    int selectionStart = SelectionStart;
                    string tempText = Text;
                    if (!PreLoaded)
                        LoadItems();
                    if (PreLoaded && !PreLoadedExecuted)
                    {
                        LoadItems();
                        PreLoadedExecuted = true;
                    }

                    FilterItems();
                    int itemIndex = FindString(Text);
                    if (itemIndex != -1)
                    {
                        DroppedDown = true;
                        //if (SelectedIndex != itemIndex)
                        //    SelectedIndex = itemIndex;
                    }

                    Text = tempText;
                    SelectionStart = selectionStart;
                    if (Items.Count == 1 && Items[0] is ConnectableControls.DropDownContainer.ListBoxItem && Items[0].ToString() == Text)
                    {
                        DroppedDown = false;
                        SelectionLength = 0;
                        SelectionStart = selectionStart;

                    }

                    #endregion
                }
                else
                {
                    {
                        #region  Colse the DropDown window
                        //Force combobox to ignore the new item selection when the DropDown window closes
                        IgnoreDropDown = true;
                        try
                        {
                            SelectedIndex = -1;
                            DroppedDown = false;
                            if (!PreLoaded)
                                LoadItems();
                            if (PreLoaded && !PreLoadedExecuted)
                            {
                                LoadItems();
                                PreLoadedExecuted = true;
                            }


                            FilterItems();
                        }
                        finally
                        {
                            IgnoreDropDown = false;
                        }
                        #endregion
                    }

                }
            }


        }
        #region Auto disable code
        /// <MetaDataID>{0ab3374a-8879-4440-9675-ee3b5e4d61e8}</MetaDataID>
        bool _UserDefinedEnableValue = true;
        /// <MetaDataID>{cabe7b9f-fcdc-42fe-b97b-4e5a0fb87840}</MetaDataID>
        bool SuspendDisplayedValueChangedHandler = false;
        /// <MetaDataID>{e64efec3-f1da-45ee-9409-65a28b5cbfee}</MetaDataID>
        bool _AutoProducedEnableValue = true;
        /// <MetaDataID>{8baaafcd-815a-4e72-9326-02350bbb78b8}</MetaDataID>
        bool EnableAutoProduced = false;
        /// <MetaDataID>{91d56857-c5dc-4cc2-98d6-81ce0218c18c}</MetaDataID>
        bool AutoProducedEnableValue
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
        /// <MetaDataID>{a2f8f993-b78d-4eb8-a891-5715906a5b99}</MetaDataID>
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



        /// <MetaDataID>{95758b47-90a7-4f00-8047-0a0a9427af60}</MetaDataID>
        bool LoadingControlData = false;
        /// <MetaDataID>{BC0B8D71-0CD1-487F-9587-6F1E9CF11998}</MetaDataID>
        public void LoadControlValues()
        {

            if (LoadingControlData)
                return;
            try
            {
                LoadingControlData = true;
                UserInterfaceObjectConnection.ReleaseDataPathNodes(this);

                //#region Auto disable functionality
                AutoProducedEnableValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
                if (_AutoDisable && !ChooseFromEnum)
                {
                    AutoProducedEnableValue &= CollectionView.SearchOperationCaller.CanCall;
                    if (!Enabled)
                    {
                        Items.Clear();
                        LoadedItems.Clear();
                        if (PreLoaded)
                            PreLoadedExecuted = false;
                    }
                }
                //#endregion

                Text = "";
                #region Load the drop down list

                //if (CollectionView.SearchOperationCaller != null &&
                //    CollectionView.SearchOperationCaller.OperationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)
                //{
                //    PreLoaded 
                //}

                if (PreLoaded && (Items.Count == 0 || (CollectionView.SearchOperationCaller != null && CollectionView.SearchOperationCaller.OperationCall.CallType == OOAdvantech.UserInterface.CallType.ViewControlObjectOperationCall)))
                {
                    //TODO όταν κάνεις set value και το operation call είναι σε view view control object τότε ξανακαλείται η loaditems 
                    if ((!ChooseFromEnum && CollectionView.SearchOperationCaller.CanCall) || ChooseFromEnum)
                    {
                        LoadItems();
                        PreLoadedExecuted = true;
                        FilterItems();
                        if (Items.Count > 0 && string.IsNullOrEmpty(_Path))
                            SelectedIndex = 0;
                        SelectedIndex = SelectedIndex;
                    }
                }
                #endregion

                LoadValue();
                //if (BeforeLockEnableState.UnInitialized)
                //    BeforeLockEnableState.Value = Enabled;






            }
            catch (System.Exception error)
            {
                throw;
            }
            finally
            {
                LoadingControlData = false;

            }
            //bool Ena=En



        }

        //OOAdvantech.Member<bool> BeforeLockEnableState = new OOAdvantech.Member<bool>();
        /// <MetaDataID>{2e715c39-f5e3-403a-a6fa-db4bfabfeb80}</MetaDataID>
        public void LockStateChange(object sender)
        {
            AutoProducedEnableValue = UserInterfaceObjectConnection.CanEditValue(_Path, this);
            //if(BeforeLockEnableState.UnInitialized)
            //    BeforeLockEnableState.Value=Enabled;
            //bool isLocked=!UserInterfaceObjectConnection.CanEditValue(_Path, this);
            //if (!isLocked)
            //    Enabled = BeforeLockEnableState.Value;
            //else
            //    Enabled = false;

        }
        /// <MetaDataID>{0fda28bc-b8a0-4d09-9817-b77264ee0df8}</MetaDataID>
        bool IsConnectionDataCorrect
        {
            get
            {
                if (UserInterfaceObjectConnection == null)
                    return false;
                OOAdvantech.MetaDataRepository.Classifier _ValueType = UserInterfaceObjectConnection.GetClassifier(Path as string);
                if (_ValueType == null)
                    return false;
                return true;
            }
        }
        /// <MetaDataID>{02c0691c-f089-42ce-8fd2-bb20839ea92b}</MetaDataID>
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


        /// <MetaDataID>{e2c30f95-2572-4d43-a45f-9135d9cbb9a9}</MetaDataID>
        private void LoadValue()
        {
            if (!ExistConnectionData)
                return;
            if (IsConnectionDataCorrect)
            {


                #region Gets the selected value
                object displayedValue = null;

                bool returnValueAsCollection = false;
                displayedValue = this.UserInterfaceObjectConnection.GetDisplayedValue(_Path, this, out returnValueAsCollection);
                _Value = displayedValue;

                if (ValueType != PresentationObjectType && PresentationObjectType != null)
                {
                    _PresentationObject = this.UserInterfaceObjectConnection.GetPresentationObject(_Value, PresentationObjectType as OOAdvantech.MetaDataRepository.Class, ValueType.GetExtensionMetaObject(typeof(Type)) as Type);

                }

                if (!string.IsNullOrEmpty(_DisplayMember) /*&& UserInterfaceObjectConnection.CanAccessValue(PresentationObject, PresentationObjectType.GetExtensionMetaObject(typeof(Type)) as Type, _DisplayMember, this)*/)
                    displayedValue = this.UserInterfaceObjectConnection.GetDisplayedValue(PresentationObject, PresentationObjectType, _DisplayMember, this, out returnValueAsCollection);
                #endregion

                if (_Value != null)
                {
                    #region Set the value as selected item in dropdown list
                    string valueText = "";
                    if (displayedValue != null)
                        valueText = displayedValue.ToString();
                    if (ValueType != null && ValueType is OOAdvantech.MetaDataRepository.Enumeration)
                    {
                        if (ConnectableControls.ViewControlObject.Translator != null)
                            valueText = ConnectableControls.ViewControlObject.Translator.Translate(_Value.GetType().FullName + "." + _Value.ToString());
                    }
                    int i = 0;
                    bool itemFinded = false;
                    foreach (object item in Items)
                    {
                        DropDownContainer.ListBoxItem listBoxItem = item as DropDownContainer.ListBoxItem;
                        if (listBoxItem == null)
                            continue;
                        if (_Value.Equals(listBoxItem.DisplayiedObject))
                        {
                            SelectedIndex = i;
                            listBoxItem.Name = valueText;
                            this.RefreshItem(i);
                            itemFinded = true;
                            break;
                        }
                        i++;
                    }
                    if (!itemFinded && !(ValueType is OOAdvantech.MetaDataRepository.Enumeration))
                        Items.Add(new DropDownContainer.ListBoxItem(_Value, displayedValue, valueText));
                    Text = valueText;
                    #endregion
                }
                else
                {
                    #region If path is "(ViewControlObject)" then set path value the first drop down list item else set text with NullValueName
                    if (_Path == "(ViewControlObject)")  //if path is the ViewControlObject and value is null 
                    {                                   //then set the ViewControlObject with the first it of item collection
                        if (!(SelectedItem is DropDownContainer.ListBoxNoneItem) && SelectedItem != null)
                        {
                            if (ValueType != _PresentationObjectType && _PresentationObjectType != null)
                            {
                                this.UserInterfaceObjectConnection.SetValue(((SelectedItem as DropDownContainer.ListBoxItem).DisplayiedObject as OOAdvantech.UserInterface.Runtime.IPresentationObject).GetRealObject(), Path.ToString());
                            }
                            else
                                this.UserInterfaceObjectConnection.SetValue((SelectedItem as DropDownContainer.ListBoxItem).DisplayiedObject, Path.ToString());
                        }
                    }
                    else
                    {

                        Text = NullValueName;
                        try
                        {
                            SelectedIndex = Items.IndexOf(NullValueName);
                        }
                        catch (System.Exception)
                        {
                        }

                    }
                    #endregion
                }
            }
        }
        /// <MetaDataID>{41C7DE1F-687B-4D1B-BCC9-2957708FF563}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            if (!SuspendUpdate)
                LoadControlValues();
        }




        /// <MetaDataID>{DEF3675C-CA5A-47A9-B839-6108A6EDE49B}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {

            if ((metaObject is OOAdvantech.UserInterface.OperationCall) &&
                (propertyDescriptor == "AllowDropOperationCall" || propertyDescriptor == "DragDropOperationCaller" ||
                propertyDescriptor == "RemoveOperationCall" || propertyDescriptor == "InsertOperationCall"))
            {
                if (metaObject is OOAdvantech.UserInterface.OperationCall && new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation != null)
                    return true;
                else
                    return false;

                //if (propertyDescriptor == "AllowDropOperationCall")
                //{
                //    if (operation.ReturnType == null || operation.ReturnType.FullName !=typeof(OOAdvantech.DragDropMethod).FullName)
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
                //return true;
            }
            if ((metaObject is OOAdvantech.UserInterface.OperationCall) && propertyDescriptor == "OperationCall")
            {
                OOAdvantech.MetaDataRepository.Operation operation = new OperationCaller(metaObject as OOAdvantech.UserInterface.OperationCall, this).Operation;

                if (operation != null)
                {
                    if (!string.IsNullOrEmpty(_Path) && ValueType != null)
                    {
                        OOAdvantech.MetaDataRepository.Classifier collectionType = OOAdvantech.UserInterface.OperationCall.GetElementType(operation.ReturnType);
                        if (!string.IsNullOrEmpty(_Path) && ValueType != null)
                            return ((ValueType == collectionType) ||
                                (ValueType != null && collectionType != null && collectionType.IsA(ValueType)));
                    }
                    return true;

                }
                else
                    return false;

                //if (propertyDescriptor == "AllowDropOperationCall")
                //{
                //    if (operation.ReturnType == null || operation.ReturnType.FullName !=typeof(OOAdvantech.DragDropMethod).FullName)
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
                //return true;
            }



            if (metaObject is OOAdvantech.MetaDataRepository.AssociationEnd && propertyDescriptor == "Path")
                return true;
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute && propertyDescriptor == "Path")
                return true;
            if (metaObject is OOAdvantech.MetaDataRepository.Attribute &&
                propertyDescriptor == "Enabled" &&
                (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) == typeof(bool))
                return true;

            else if (metaObject is OOAdvantech.MetaDataRepository.BehavioralFeature && propertyDescriptor == "Operation")
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.Attribute && propertyDescriptor == "DisplayMember")
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.Enumeration && propertyDescriptor == "Enumeration")
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
                        {
                            _valueType = enumerator.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                            break;
                        }
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
        }

        /// <MetaDataID>{5FABFD9C-799F-42AA-8136-9517C2ECE909}</MetaDataID>
        public void SaveControlValues()
        {

        }


        /// <MetaDataID>{8dcb1381-b448-4dda-8808-bcf76dbaa897}</MetaDataID>
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

        /// <MetaDataID>{cb91def9-f490-4f4f-9e30-302eba3b6a44}</MetaDataID>
        bool _AutoSuggest = false;
        /// <MetaDataID>{a9c3524a-6b1b-4c48-96f3-f016bc639d01}</MetaDataID>
        [Category("Object Model Connection")]
        public bool AutoSuggest
        {
            get
            {
                return _AutoSuggest;
            }
            set
            {
                _AutoSuggest = value;
            }
        }

        /// <MetaDataID>{c644a75f-5080-49f5-b479-e94f616603f4}</MetaDataID>
        private bool _AutoDisable = true;
        /// <MetaDataID>{06d096dc-80e7-47c2-a5bf-cf9a4b59b61a}</MetaDataID>
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

        /// <MetaDataID>{012af533-5cf9-4ebf-865b-d5f925b97bd7}</MetaDataID>
        private bool _AutoInsert;
        /// <MetaDataID>{f4c5e3ea-0b10-4d81-a0b0-d2517839c665}</MetaDataID>
        [Category("Object Model Connection")]
        public bool AutoInsert
        {
            get
            {
                return _AutoInsert;
            }
            set
            {
                _AutoInsert = value;
            }
        }

        #region Data path nodes are all nodes of data tree where depends the value of control




        #endregion



    }
}
