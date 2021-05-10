using OOAdvantech.UserInterface.Runtime;
using System.Windows.Markup.Primitives;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
namespace WPFUIElementObjectBind
{

    /// <MetaDataID>{fcc04bef-16e8-44ed-94a2-fbc531e36120}</MetaDataID>
    public class UIElementManager : OOAdvantech.UserInterface.Runtime.IPathDataDisplayer, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl
    {
        /// <MetaDataID>{4582cfc1-c697-4e5c-8e3f-11b9e7eb150f}</MetaDataID>
        static List<DependencyProperty> UIElementPropertiesLocks = new List<DependencyProperty>();
        /// <MetaDataID>{c59466b0-5470-4290-bea7-a391f03f16f7}</MetaDataID>
        public static void RegisterUIElementLockProperty(DependencyProperty dependencyProperty)
        {
            if (!UIElementPropertiesLocks.Contains(dependencyProperty))
                UIElementPropertiesLocks.Add(dependencyProperty);


        }
        /// <MetaDataID>{384fee1e-093e-4e18-9e6e-561aeb879a1e}</MetaDataID>
        static UIElementManager()
        {
            //RegisterUIElementLockProperty(TextBox.TextProperty);
            //RegisterUIElementExtender(typeof(DataGrid), typeof(DataGridExtender));
        }


        /// <MetaDataID>{617e709c-21c7-43c5-b3c9-28eb947f1e30}</MetaDataID>
        static Dictionary<System.Type, System.Type> UIElementsExtenders = new Dictionary<System.Type, System.Type>();

        public static void RegisterUIElementExtender(System.Type orgUIElementType, System.Type UIElementExtenderType)
        {
            UIElementsExtenders[orgUIElementType] = UIElementExtenderType;
        }
        /// <MetaDataID>{2f95b9fa-c39a-4c67-b60d-9283e0f8595e}</MetaDataID>
        public void UserInterfaceObjectConnectionChangeState(ViewControlObjectState oldState, ViewControlObjectState newState)
        {
        }
        /// <exclude>Excluded</exclude>
        System.Windows.FrameworkElement _UIElement;

        /// <MetaDataID>{ce02d896-dd6f-4dd3-8a52-61a6c3377701}</MetaDataID>
        [OOAdvantech.MetaDataRepository.Association("ManagedUIElement", typeof(System.Windows.FrameworkElement), OOAdvantech.MetaDataRepository.Roles.RoleA, "4c0b84e1-8866-4c0e-b2cf-1a25f63f20ca"), OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1, 1), OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(0)]
        public System.Windows.FrameworkElement UIElement
        {
            get
            {
                return _UIElement;
            }
        }
        /// <MetaDataID>{f76516cc-aec2-43e6-abf3-b367e1eb7832}</MetaDataID>
        ViewControlObject ViewControlObject;
        /// <MetaDataID>{eede8067-4112-4a60-9c7b-3904c64821dc}</MetaDataID>
        public UIElementManager(System.Windows.FrameworkElement uiElement, ViewControlObject viewControlObject)
        {
            _UIElement = uiElement;
            ViewControlObject = viewControlObject;
            foreach (var uiElementType in UIElementsExtenders.Keys)
            {
                if (uiElement.GetType() == uiElementType || uiElement.GetType().IsSubclassOf(uiElementType))
                {
                    IUIElementExtender uiElementExtender = System.Activator.CreateInstance(UIElementsExtenders[uiElementType]) as IUIElementExtender;
                    uiElementExtender.Attach(uiElement,this);
                    break;
                }
            }
        
            _UserInterfaceObjectConnection = viewControlObject.UserInterfaceObjectConnection;

            if (uiElement.DataContext is OOAdvantech.UserInterface.DynamicUIProxy)
            {
                OOAdvantech.UserInterface.DynamicUIProxy dynamicProxy = uiElement.DataContext as OOAdvantech.UserInterface.DynamicUIProxy;
                MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(uiElement);
                bool canEdit = true;
                if (markupObject != null)
                {
                    //foreach (MarkupProperty mp in markupObject.Properties)
                    //{
                    //    if (mp.DependencyProperty != null && UIElementPropertiesLocks.Contains(mp.DependencyProperty))
                    //    {
                    //        Binding binding = BindingOperations.GetBinding(uiElement as DependencyObject, mp.DependencyProperty);
                    //        string path = binding.Path.Path;
                    //        if (!dynamicProxy.UserInterfaceObjectConnection.CanEditValue(dynamicProxy.Target, dynamicProxy.TargetType, path, this))
                    //        {
                    //            canEdit = false;
                    //            break;

                    //        }
                    //    }
                    //}
                    //if (!canEdit)
                    //{

                    //    foreach (PropertyDescriptor pd in System.ComponentModel.TypeDescriptor.GetProperties(uiElement,
                    //    new System.Attribute[] { new System.ComponentModel.PropertyFilterAttribute(System.ComponentModel.PropertyFilterOptions.All) }))
                    //    {
                    //        if (pd.Name == "IsEnabled")
                    //        {
                               
                    //            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);

                    //            if (dpd != null && dpd.DependencyProperty != null)
                    //                uiElement.SetValue(dpd.DependencyProperty, false);
                    //        }
                    //    }
                    //}
                }
            }
        }


        #region IPathDataDisplayer Members

        /// <MetaDataID>{22156ed4-e0bc-419b-8e11-943050f51ad8}</MetaDataID>
        public object Path
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        /// <MetaDataID>{e670fff4-8f53-4150-bc6a-c9f49d00d838}</MetaDataID>
        public void LoadControlValues()
        {
            //MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(_UIElement);
            //foreach (MarkupProperty mp in markupObject.Properties)
            //{
            //    if (mp.DependencyProperty != null)
            //    {
            //        Binding binding = BindingOperations.GetBinding(_UIElement, mp.DependencyProperty);
            //        if (binding != null)
            //        {
            //            if (binding.ElementName == ViewControlObject.Name)
            //            {
            //                bool returnValueAsCollection = false;
            //                _UIElement.SetValue(mp.DependencyProperty, UserInterfaceObjectConnection.GetDisplayedValue(binding.Path.Path.Substring(7), this, out returnValueAsCollection));
            //                _UIElement.SetBinding(mp.DependencyProperty, binding);
            //            }

            //        }
            //    }

            //}

        }

        /// <MetaDataID>{255d2d7f-0aad-4f79-8e8c-8f3c9e481c45}</MetaDataID>
        public void SaveControlValues()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection;

        /// <MetaDataID>{c3a0aa5e-36b2-4237-af86-88e0434fa847}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
        }
        /// <MetaDataID>{424bd49a-ca71-4138-baae-5f2867f3561d}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> _Paths;
        /// <MetaDataID>{895b8f35-fdfd-4064-b5e0-37782bd0505e}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                if (_Paths == null)
                {
                    _Paths = new OOAdvantech.Collections.Generic.List<string>();
                    MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(_UIElement);
                    foreach (MarkupProperty mp in markupObject.Properties)
                    {
                        if (mp.DependencyProperty != null)
                        {
                            Binding binding = BindingOperations.GetBinding(_UIElement, mp.DependencyProperty);
                            if (binding != null)
                                _Paths.Add(binding.Path.Path);
                        }

                    }
                }
                return new OOAdvantech.Collections.Generic.List<string>(_Paths);


            }
        }

        /// <MetaDataID>{9daaa902-dccb-480a-b626-f80b90b93067}</MetaDataID>
        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{fc7e7f4a-417d-4a07-bbdf-05560fb73c9c}</MetaDataID>
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {

            if (_UIElement.DataContext is OOAdvantech.UserInterface.DynamicUIProxy)
            {
                OOAdvantech.UserInterface.DynamicUIProxy dynamicProxy = _UIElement.DataContext as OOAdvantech.UserInterface.DynamicUIProxy;
                MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(_UIElement);
                bool canEdit = true;
                if (markupObject != null)
                {
                    foreach (MarkupProperty mp in markupObject.Properties)
                    {
                        if (mp.DependencyProperty != null && UIElementPropertiesLocks.Contains(mp.DependencyProperty))
                        {
                            Binding binding = BindingOperations.GetBinding(_UIElement as DependencyObject, mp.DependencyProperty);
                            string path = binding.Path.Path;
                            if (!dynamicProxy.UserInterfaceObjectConnection.CanEditValue(dynamicProxy.Target, dynamicProxy.TargetType, path, this))
                            {
                                canEdit = false;
                                break;

                            }
                        }
                    }
                    if (!canEdit)
                    {

                        foreach (PropertyDescriptor pd in System.ComponentModel.TypeDescriptor.GetProperties(_UIElement,
                        new System.Attribute[] { new System.ComponentModel.PropertyFilterAttribute(System.ComponentModel.PropertyFilterOptions.All) }))
                        {
                            if (pd.Name == "IsEnabled")
                            {
                                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                                if (dpd != null && dpd.DependencyProperty != null)
                                    _UIElement.SetValue(dpd.DependencyProperty, false);
                            }
                        }
                    }
                    else
                    {
                        foreach (PropertyDescriptor pd in System.ComponentModel.TypeDescriptor.GetProperties(_UIElement,
                                new System.Attribute[] { new System.ComponentModel.PropertyFilterAttribute(System.ComponentModel.PropertyFilterOptions.All) }))
                        {
                            if (pd.Name == "IsEnabled")
                            {
                                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                                if (dpd != null && dpd.DependencyProperty != null)
                                    _UIElement.SetValue(dpd.DependencyProperty, true);
                            }
                        }

                    }
                }
            }
        }

        /// <MetaDataID>{2976cd22-ec63-406d-86a2-d5ac6233894e}</MetaDataID>
        public void LockStateChange(object sender)
        {

        }

        #endregion

        #region IObjectMemberViewControl Members

        /// <MetaDataID>{e2c5c2fe-64b0-46e9-8703-ac112e56e0ea}</MetaDataID>
        public string Name
        {
            get
            {
                return UIElement.Name;
            }
            set
            {

            }
        }

        /// <MetaDataID>{24372f30-09e4-46ea-a793-75b7a6edaa18}</MetaDataID>
        public bool AllowDrag
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{4ab09676-085b-47ee-9672-a0aacd3ba50a}</MetaDataID>
        public bool AllowDrop
        {
            get
            {
                return _UIElement.AllowDrop;
            }
        }

        /// <MetaDataID>{84b387c6-b876-45f4-935b-ea82feba599d}</MetaDataID>
        public bool ErrorCheck(ref System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors)
        {
            return false;
        }

        #endregion

        #region IOperationCallerSource Members

        /// <MetaDataID>{7c754ccc-7b16-40d9-a17d-1aac78dd3da6}</MetaDataID>
        public string[] PropertiesNames
        {
            get
            {
                return new string[0];
            }
        }

        /// <MetaDataID>{0a4fc051-52c7-4c0e-9cdd-34f2a61358a2}</MetaDataID>
        public object GetPropertyValue(string propertyName)
        {
            return null;
        }

        /// <MetaDataID>{7bd0bfb7-00f7-4bf5-9757-fee666c62b69}</MetaDataID>
        public void SetPropertyValue(string propertyName, object value)
        {

        }

        /// <MetaDataID>{a257807d-7812-45b7-a748-22ec575e045d}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetPropertyType(string propertyName)
        {
            return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(object));
        }

        /// <MetaDataID>{b1276b0f-15f6-43e1-989b-83b7905befbc}</MetaDataID>
        public bool ContainsProperty(string propertyName)
        {
            return false;
        }

        #endregion

        #region IConnectableControl Members

        /// <MetaDataID>{6dee896b-471f-4f6e-a9a8-7504cc681425}</MetaDataID>
        public void InitializeControl()
        {

        }

        /// <MetaDataID>{3f22cce7-879c-447b-a1a4-ebbf8c8a93ea}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection OOAdvantech.UserInterface.Runtime.IConnectableControl.UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
            set
            {

            }
        }

        /// <MetaDataID>{49719aff-7f5b-4779-a525-dd661d1e2a2d}</MetaDataID>
        public System.Collections.Generic.List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return new System.Collections.Generic.List<IDependencyProperty>();
            }
        }

        /// <MetaDataID>{e805fb37-d49d-4357-bdcb-7416347b2c4e}</MetaDataID>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return true;
        }

        #endregion

        #region IMetadataSelectionResolver Members

        /// <MetaDataID>{5aef2ebe-a6fc-4ede-8685-d78a81c952ee}</MetaDataID>
        public bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            return false;
        }

        /// <MetaDataID>{cc8408d2-a61f-42ac-b48e-4366a4442596}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (_UserInterfaceObjectConnection != null)
                return _UserInterfaceObjectConnection.PresentationObjectType;
            else
                return null;

        }

        #endregion

        internal bool GetPropertyPath(FrameworkElement frameworkElement,out string path)
        {
            path=null;
            Binding binding = BindingOperations.GetBinding(frameworkElement, FrameworkElement.DataContextProperty);
            
            if (binding!=null&&!string.IsNullOrEmpty(binding.ElementName))
            {
                object element = frameworkElement.FindName(binding.ElementName);
                if (element == ViewControlObject)
                    path = "ViewControlObject";
            }
            else
            {
                if (frameworkElement.Parent is FrameworkElement)
                    GetPropertyPath(frameworkElement.Parent as FrameworkElement, out path);
            }
            

            if (binding != null)
            {
                if (string.IsNullOrEmpty(path))
                    path = binding.Path.Path;
                else
                    path += "." + binding.Path.Path;
            }
            if (path != null && path.IndexOf("ViewControlObject.Source.Value") == 0)
                return true;
            else
                return false;




        }
        internal bool GetPropertyPath(FrameworkElement frameworkElement, DependencyProperty dependencyProperty, out string path)
        {
            if (!GetPropertyPath(frameworkElement, out path))
                return false;


            Binding binding =BindingOperations.GetBinding(frameworkElement,FrameworkElement.DataContextProperty);
            if (binding != null)
                path = binding.Path.Path;

            binding = BindingOperations.GetBinding(frameworkElement, dependencyProperty);
            if (string.IsNullOrEmpty(path))
                path = binding.Path.Path;
            else
                path += "." + binding.Path.Path;

            if (path != null && path.IndexOf("ViewControlObject.Source.Value") == 0)
            {
                path = path.Substring("ViewControlObject.Source.Value".Length + 1);
                return true;
            }
            else
                return false;

            
        }
    }
}
