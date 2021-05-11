using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.PropertyEditors;
using System.Runtime.InteropServices;

//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;
//using Microsoft.VisualStudio.Modeling.Shell;
using System.Xml;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.ComponentModel.Design.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Globalization;
using System.Xml.Linq;


namespace ConnectableControls
{
    /// <MetaDataID>{343d3919-1d17-4476-8957-85e96b6bb828}</MetaDataID>
    [Serializable]
    public class TimeSpanProperty : Component
    {
        internal TimeSpan TimeSpan
        {
            get
            {
                return new TimeSpan(_Days, _Hours, _Minutes, _Seconds, _Milliseconds);

            }

        }

        public TimeSpanProperty(TimeSpan timeSpan)
        {
            _Days = timeSpan.Days;
            _Hours = timeSpan.Hours;
            _Minutes = timeSpan.Minutes;
            _Seconds = timeSpan.Seconds;
            _Milliseconds = timeSpan.Milliseconds;

            //TimeSpan = timeSpan;
        }
        int _Days;
        public int Days
        {
            get
            {
                return _Days;
            }
            set
            {
                _Days = value;
                _Days = TimeSpan.Days;
                _Hours = TimeSpan.Hours;
                _Minutes = TimeSpan.Minutes;
                _Seconds = TimeSpan.Seconds;
                _Milliseconds = TimeSpan.Milliseconds;

            }
        }
        int _Hours;
        public int Hours
        {
            get
            {
                return _Hours;
            }
            set
            {
                _Hours = value;
                _Days = TimeSpan.Days;
                _Hours = TimeSpan.Hours;
                _Minutes = TimeSpan.Minutes;
                _Seconds = TimeSpan.Seconds;
                _Milliseconds = TimeSpan.Milliseconds;

            }
        }


        int _Minutes;
        public int Minutes
        {
            get
            {
                return _Minutes;
            }
            set
            {
                _Minutes = value;
                _Days = TimeSpan.Days;
                _Hours = TimeSpan.Hours;
                _Minutes = TimeSpan.Minutes;
                _Seconds = TimeSpan.Seconds;
                _Milliseconds = TimeSpan.Milliseconds;

            }
        }
        int _Seconds;
        public int Seconds
        {
            get
            {
                return _Seconds;
            }
            set
            {
                _Seconds = value;
                _Days = TimeSpan.Days;
                _Hours = TimeSpan.Hours;
                _Minutes = TimeSpan.Minutes;
                _Seconds = TimeSpan.Seconds;
                _Milliseconds = TimeSpan.Milliseconds;

            }
        }
        int _Milliseconds;
        public int Milliseconds
        {
            get
            {
                return _Milliseconds;
            }
            set
            {
                _Milliseconds = value;
                _Days = TimeSpan.Days;
                _Hours = TimeSpan.Hours;
                _Minutes = TimeSpan.Minutes;
                _Seconds = TimeSpan.Seconds;
                _Milliseconds = TimeSpan.Milliseconds;

            }
        }



    }

    /// <MetaDataID>{C8DCA3B9-65B0-4066-AA2A-3864D3353420}</MetaDataID>
    public class FormConnectionControl : ViewControlObject
    {

        bool _CreatePresentationObjectAnyway = false;
        [Category("Object Model Connection")]
        [Description("Create presentation object in case where the instance is null.")]
        public bool CreatePresentationObjectAnyway
        {
            get
            {
                return (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).CreatePresentationObjectAnyway;
            }
            set
            {
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).CreatePresentationObjectAnyway = value;
            }
        }

        public event EventHandler BeforeViewControlObjectInitialization;
        public event EventHandler AfterViewControlObjectInitialization;
        public override void OnBeforeViewControlObjectInitialization()
        {
            if (BeforeViewControlObjectInitialization != null)
                BeforeViewControlObjectInitialization(this, EventArgs.Empty);

        }
        public override void OnAfterViewControlObjectInitialization()
        {
            if (AfterViewControlObjectInitialization != null)
                AfterViewControlObjectInitialization(this, EventArgs.Empty);
        }



        [Category("Transaction Settings")]
        public bool RollbackOnNegativeAnswer
        {
            get
            {
                return (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnNegativeAnswer;
            }
            set
            {
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnNegativeAnswer = value;
            }
        }
        [Category("Transaction Settings")]
        public bool RollbackOnExitWithoutAnswer
        {
            get
            {
                return (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnExitWithoutAnswer;
            }
            set
            {
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).RollbackOnExitWithoutAnswer = value;
            }
        }


        /// <MetaDataID>{7608FEC5-8C57-4248-ADD1-E27012BF8862}</MetaDataID>
        public FormConnectionControl()
        {
            UserInterfaceObjectConnection = new OOAdvantech.UserInterface.Runtime.FormObjectConnection(this);

        }


        #region Design mode members
        System.ComponentModel.Design.IDesignerHost _DesignerHost;
        public override System.ComponentModel.ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;



                if (value != null && value.Container is System.ComponentModel.Design.IDesignerHost)
                {
                    _DesignerHost = value.Container as System.ComponentModel.Design.IDesignerHost;
                    foreach (Component component in _DesignerHost.Container.Components)
                    {
                        if (component is IObjectMemberViewControl)
                        {
                            if ((component as IObjectMemberViewControl).UserInterfaceObjectConnection != null && ((component as IObjectMemberViewControl).UserInterfaceObjectConnection.PresentationContextViewControl as ViewControlObject).Site != null)
                                continue;

                            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
                            PropertyDescriptor property = properties.Find("ViewControlObject", false);
                            if (property != null)
                                property.SetValue(component, this);
                        }
                    }

                    _DesignerHost.LoadComplete += new EventHandler(DesignerHostLoadComplete);
                    _DesignerHost.TransactionClosed += new System.ComponentModel.Design.DesignerTransactionCloseEventHandler(TransactionClosed);
                    (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded += new System.ComponentModel.Design.ComponentEventHandler(ComponentAdded);
                    (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentRemoved += new System.ComponentModel.Design.ComponentEventHandler(ComponentRemoved);
                    (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged += new System.ComponentModel.Design.ComponentChangedEventHandler(ComponentChanged);
                    AssemblyManager.InVisualStudio = true;
                }
                else
                {
                    if (_DesignerHost != null)
                    {
                        _DesignerHost.LoadComplete -= new EventHandler(DesignerHostLoadComplete);
                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded -= new System.ComponentModel.Design.ComponentEventHandler(ComponentAdded);
                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentRemoved -= new System.ComponentModel.Design.ComponentEventHandler(ComponentRemoved);
                        (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged -= new System.ComponentModel.Design.ComponentChangedEventHandler(ComponentChanged);
                        _DesignerHost = null;
                    }

                }
            }
        }

        void ContainerControl_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //System.Windows.Forms.Control control= ContainerControl.GetChildAtPoint(ContainerControl.PointToClient(System.Windows.Forms.Form.MousePosition));
            //ClassViewNavigationInfo.GetData(e.Data); 
        }

        /// <MetaDataID>{037F9BFE-05C7-4FB6-9913-2D2167BDAA8A}</MetaDataID>
        void TransactionClosed(object sender, System.ComponentModel.Design.DesignerTransactionCloseEventArgs e)
        {
            if (DesignMode)
            {
                if (!_DesignerHost.Loading)
                {
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);

                    try
                    {
                        Hashtable nametable = new Hashtable(_DesignerHost.Container.Components.Count);
                        XmlDocument document = new XmlDocument();


                        document.LoadXml("<Main/>");
                        document.DocumentElement.SetAttribute("RootComponentClassName", _DesignerHost.RootComponentClassName);
                        document.DocumentElement.AppendChild(WriteObject(document, nametable, _DesignerHost.RootComponent));

                        foreach (IComponent comp in _DesignerHost.Container.Components)
                        {
                            if (comp != _DesignerHost.RootComponent && !nametable.ContainsKey(comp))
                            {
                                document.DocumentElement.AppendChild(WriteObject(document, nametable, comp));
                            }
                        }

                        foreach (OOAdvantech.MetaDataRepository.Dependency depependency in ContainerControlType.ImplementationUnit.ClientDependencies)
                        {

                            string assemblyRef = depependency.Supplier.GetPropertyValue(typeof(string), ".Net", "AssemblyFullName") as string;
                            if (string.IsNullOrEmpty(assemblyRef))
                                assemblyRef = depependency.Supplier.Identity.ToString();

                            document.DocumentElement.AppendChild(document.CreateElement("AssemblyReference")).InnerText = assemblyRef;



                        }

                        document.Save(@"c:\Form.xml");
                    }
                    catch (Exception error)
                    {


                    }

                }

            }
        }
        #region Serialization
        private static readonly Attribute[] propertyAttributes = new Attribute[] {
			DesignOnlyAttribute.No
		};
        private XmlNode WriteObject(XmlDocument document, IDictionary nametable, object value)
        {
            IDesignerHost idh = (IDesignerHost)this._DesignerHost.GetService(typeof(IDesignerHost));
            Debug.Assert(value != null, "Should not invoke WriteObject with a null value");

            XmlNode node = document.CreateElement("Object");
            XmlAttribute typeAttr = document.CreateAttribute("type");

            typeAttr.Value = value.GetType().AssemblyQualifiedName;
            node.Attributes.Append(typeAttr);

            IComponent component = value as IComponent;

            if (component != null && component.Site != null && component.Site.Name != null)
            {
                XmlAttribute nameAttr = document.CreateAttribute("name");

                nameAttr.Value = component.Site.Name;
                node.Attributes.Append(nameAttr);
                //Debug.Assert(nametable[component] == null, "WriteObject should not be called more than once for the same object.  Use WriteReference instead");
                nametable[value] = component.Site.Name;
            }

            bool isControl = (value is System.Windows.Forms.Control);

            if (isControl)
            {
                XmlAttribute childAttr = document.CreateAttribute("children");

                childAttr.Value = "Controls";
                node.Attributes.Append(childAttr);
            }

            if (component != null)
            {
                if (isControl)
                {
                    foreach (System.Windows.Forms.Control child in ((System.Windows.Forms.Control)value).Controls)
                    {
                        if (child.Site != null && child.Site.Container == idh.Container)
                        {
                            node.AppendChild(WriteObject(document, nametable, child));
                        }
                    }
                }// if isControl

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, propertyAttributes);

                if (isControl)
                {
                    PropertyDescriptor controlProp = properties["Controls"];

                    if (controlProp != null)
                    {
                        PropertyDescriptor[] propArray = new PropertyDescriptor[properties.Count - 1];
                        int idx = 0;

                        foreach (PropertyDescriptor p in properties)
                        {
                            if (p != controlProp)
                            {
                                propArray[idx++] = p;
                            }
                        }

                        properties = new PropertyDescriptorCollection(propArray);
                    }
                }

                WriteProperties(document, properties, value, node, "Property");

                EventDescriptorCollection events = TypeDescriptor.GetEvents(value, propertyAttributes);
                IEventBindingService bindings = _DesignerHost.GetService(typeof(IEventBindingService)) as IEventBindingService;

                if (bindings != null)
                {
                    properties = bindings.GetEventProperties(events);
                    WriteProperties(document, properties, value, node, "Event");
                }
            }
            else
            {
                WriteValue(document, value, node);
            }

            return node;
        }
        private void WriteProperties(XmlDocument document, PropertyDescriptorCollection properties, object value, XmlNode parent, string elementName)
        {
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name == "AutoScaleBaseSize")
                {
                    string _DEBUG_ = prop.Name;
                }

                if (prop.ShouldSerializeValue(value))
                {
                    string compName = parent.Name;
                    XmlNode node = document.CreateElement(elementName);
                    XmlAttribute attr = document.CreateAttribute("name");

                    attr.Value = prop.Name;
                    node.Attributes.Append(attr);

                    DesignerSerializationVisibilityAttribute visibility = (DesignerSerializationVisibilityAttribute)prop.Attributes[typeof(DesignerSerializationVisibilityAttribute)];

                    switch (visibility.Visibility)
                    {
                        case DesignerSerializationVisibility.Visible:
                            if (!prop.IsReadOnly && WriteValue(document, prop.GetValue(value), node))
                            {
                                parent.AppendChild(node);
                            }

                            break;

                        case DesignerSerializationVisibility.Content:
                            object propValue = prop.GetValue(value);

                            if (typeof(IList).IsAssignableFrom(prop.PropertyType))
                            {
                                WriteCollection(document, (IList)propValue, node);
                            }
                            else
                            {
                                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(propValue, propertyAttributes);

                                WriteProperties(document, props, propValue, node, elementName);
                            }

                            if (node.ChildNodes.Count > 0)
                            {
                                parent.AppendChild(node);
                            }

                            break;

                        default:
                            break;
                    }
                }
            }
        }
        private XmlNode WriteReference(XmlDocument document, IComponent value)
        {
            IDesignerHost idh = (IDesignerHost)_DesignerHost.GetService(typeof(IDesignerHost));

            Debug.Assert(value != null && value.Site != null && value.Site.Container == idh.Container, "Invalid component passed to WriteReference");

            XmlNode node = document.CreateElement("Reference");
            XmlAttribute attr = document.CreateAttribute("name");

            attr.Value = value.Site.Name;
            node.Attributes.Append(attr);
            return node;
        }
        private bool WriteValue(XmlDocument document, object value, XmlNode parent)
        {
            IDesignerHost idh = (IDesignerHost)_DesignerHost.GetService(typeof(IDesignerHost));

            // For empty values, we just return.  This creates an empty node.
            if (value == null)
            {
                return true;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(value);
            (parent as XmlElement).SetAttribute("TypeConverter", converter.GetType().FullName);
            (parent as XmlElement).SetAttribute("TypeConverterAssembly", converter.GetType().Assembly.FullName);


            if (GetConversionSupported(converter, typeof(string)))
            {
                parent.InnerText = (string)converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(string));
            }
            else if (GetConversionSupported(converter, typeof(byte[])))
            {
                byte[] data = (byte[])converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(byte[]));

                parent.AppendChild(WriteBinary(document, data));
            }
            else if (GetConversionSupported(converter, typeof(InstanceDescriptor)))
            {
                InstanceDescriptor id = (InstanceDescriptor)converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(InstanceDescriptor));

                parent.AppendChild(WriteInstanceDescriptor(document, id, value));
            }
            else if (value is IComponent && ((IComponent)value).Site != null && ((IComponent)value).Site.Container == idh.Container)
            {
                parent.AppendChild(WriteReference(document, (IComponent)value));
            }
            else if (value.GetType().IsSerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();

                formatter.Serialize(stream, value);

                XmlNode binaryNode = WriteBinary(document, stream.ToArray());

                parent.AppendChild(binaryNode);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Simple helper method that returns true if the given type converter supports
        /// two-way conversion of the given type.
        /// </summary>
        private bool GetConversionSupported(TypeConverter converter, Type conversionType)
        {
            return (converter.CanConvertFrom(conversionType) && converter.CanConvertTo(conversionType));
        }


        private void WriteCollection(XmlDocument document, IList list, XmlNode parent)
        {
            foreach (object obj in list)
            {
                XmlNode node = document.CreateElement("Item");
                XmlAttribute typeAttr = document.CreateAttribute("type");

                typeAttr.Value = obj.GetType().AssemblyQualifiedName;
                node.Attributes.Append(typeAttr);
                WriteValue(document, obj, node);
                parent.AppendChild(node);
            }
        }
        private XmlNode WriteBinary(XmlDocument document, byte[] value)
        {
            XmlNode node = document.CreateElement("Binary");

            node.InnerText = Convert.ToBase64String(value);
            return node;
        }
        private XmlNode WriteInstanceDescriptor(XmlDocument document, InstanceDescriptor desc, object value)
        {
            XmlNode node = document.CreateElement("InstanceDescriptor");
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, desc.MemberInfo);

            XmlAttribute memberAttr = document.CreateAttribute("member");

            memberAttr.Value = Convert.ToBase64String(stream.ToArray());
            node.Attributes.Append(memberAttr);
            foreach (object arg in desc.Arguments)
            {
                XmlNode argNode = document.CreateElement("Argument");

                if (WriteValue(document, arg, argNode))
                {
                    node.AppendChild(argNode);
                }
            }

            if (!desc.IsComplete)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(value, propertyAttributes);

                WriteProperties(document, props, value, node, "Property");
            }

            return node;
        }

        #endregion
        bool DesignerHostLoadCompleted = false;
        System.Collections.ArrayList DesignerHostOnLoadEvents = new System.Collections.ArrayList();

        /// <MetaDataID>{10318B75-647A-4A4D-BAF7-2007DD456BB8}</MetaDataID>
        void DesignerHostLoadComplete(object sender, EventArgs e)
        {
            DesignerHostLoadCompleted = true;
            if (DesignMode)
            {

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
                PropertyDescriptor property = properties.Find("MetaData", false);
                if (FormMetaDataAsXmlDocument == null)
                {
                    FormMetaDataAsXmlDocument = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage objectViewStorage = null;
                    objectViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryObjectViewStorage", FormMetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    ObjectView = objectViewStorage.NewObject(typeof(OOAdvantech.UserInterface.ObjectView)) as OOAdvantech.UserInterface.ObjectView;
                    ObjectView.Identity = System.Guid.NewGuid().ToString();
                    property.SetValue(this, MetaData);
                }
                if (_ContainerControl == null)
                {
                    property = properties.Find("ContainerControl", false);
                    property.SetValue(this, _DesignerHost.RootComponent as System.Windows.Forms.Control);
                }

            }

        }
        #endregion


        #region ComponentEvents
        /// <MetaDataID>{9387DF96-2D7B-4374-AE43-3737AEE42608}</MetaDataID>
        void ComponentChanged(object sender, System.ComponentModel.Design.ComponentChangedEventArgs e)
        {
            try
            {
                if (_DesignerHost.Loading)
                    DesignerHostOnLoadEvents.Add(e);
                else
                {
                    return;
                    if (e.Component is System.Windows.Forms.Control && (e.Component as System.Windows.Forms.Control).Parent != null)
                    {
                        OOAdvantech.UserInterface.Control control = GetUserInterfaceMetaObject(GetControlIdentintity(e.Component as System.Windows.Forms.Control)) as OOAdvantech.UserInterface.Control;
                        if (control == null)
                        {
                            string identity = GetControlIdentintity(e.Component as System.Windows.Forms.Control);
                            OOAdvantech.UserInterface.Component userInterfaceMetaObject = GetUserInterfaceMetaObject(identity);
                            if (userInterfaceMetaObject == null)
                            {
                                userInterfaceMetaObject = CreateUserInterfaceMetaObject(e.Component as IComponent, identity);
                                ObjectView.AddComponent(userInterfaceMetaObject);
                                if (GetUserInterfaceMetaObject(GetControlIdentintity((e.Component as System.Windows.Forms.Control).Parent)) != null)
                                    (GetUserInterfaceMetaObject(GetControlIdentintity((e.Component as System.Windows.Forms.Control).Parent)) as OOAdvantech.UserInterface.ContainerControl).AddControl(userInterfaceMetaObject as OOAdvantech.UserInterface.Control);
                            }
                            control = userInterfaceMetaObject as OOAdvantech.UserInterface.Control;
                        }
                        if (e.Member != null)
                        {
                            if (e.Member.Name == "Size" && e.NewValue != null)
                            {
                                control.Size = (System.Drawing.Size)e.NewValue;
                            }
                            if (e.Member.Name == "Location" && e.NewValue != null)
                            {
                                control.Location = (System.Drawing.Point)e.NewValue;
                            }
                            if (e.Member.Name == "Name")
                                UserInterfaceObjectConnection.HostFormComponentNameChanged(sender, new OOAdvantech.UserInterface.Runtime.ComponentNameChangedEventArgs(e.Component, e.OldValue as string, e.NewValue as string));
                        }
                    }


                }


            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{AD236C84-08B7-46BB-B1C2-D11580DCD6E4}</MetaDataID>
        void ComponentRemoved(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {
            try
            {


                if (_DesignerHost.Loading)
                    DesignerHostOnLoadEvents.Add(e);
                else if (e.Component is System.Windows.Forms.Control)
                {

                    string identity = GetControlIdentintity(e.Component as System.Windows.Forms.Control);
                    OOAdvantech.UserInterface.Control userInterfaceMetaObject = GetUserInterfaceMetaObject(identity) as OOAdvantech.UserInterface.Control;
                    if (userInterfaceMetaObject != null)
                    {
                        if ((e.Component as System.Windows.Forms.Control).Parent != null)
                        {
                            OOAdvantech.UserInterface.ContainerControl control = GetUserInterfaceMetaObject(GetControlIdentintity((e.Component as System.Windows.Forms.Control).Parent)) as OOAdvantech.UserInterface.ContainerControl;
                            control.RemoveControl(userInterfaceMetaObject);
                        }
                        ObjectView.RemoveComponent(userInterfaceMetaObject);
                    }

                }
            }
            catch (Exception error)
            {

            }
        }

        /// <MetaDataID>{897E6A11-BA26-4F36-9660-AE37C6D4C157}</MetaDataID>
        void ComponentAdded(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        {
            try
            {
                if (!_DesignerHost.Loading && !DesignerHostLoadCompleted)
                {
                    if (!(_DesignerHost.RootComponent is System.Windows.Forms.Form))
                    {
                        (_DesignerHost as IContainer).Remove(this);
                        System.Windows.Forms.MessageBox.Show("This component can be used only in System.Windows.Forms.Form", "Connectable Controls Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return;


                    }
                    DesignerHostLoadComplete(null, null);
                    SychronizeMetaData(_DesignerHost.RootComponent as System.Windows.Forms.Control);
                    foreach (System.ComponentModel.Component component in _DesignerHost.Container.Components)
                    {
                        if (component is System.Windows.Forms.Control)
                            ObjectView.AddComponent(GetUserInterfaceMetaObject(GetControlIdentintity(component as System.Windows.Forms.Control)));
                    }
                    TypeDescriptor.GetProperties(this).Find("MetaData", false).SetValue(this, MetaData);
                }
                else if (!_DesignerHost.Loading && e.Component is System.Windows.Forms.Control)
                {

                }

                //if (e.Component is Menus.MenuCommand)
                //    _DesignerHost.Container.Remove(e.Component);


                if (_DesignerHost.Loading)
                    DesignerHostOnLoadEvents.Add(e);
            }
            catch (System.Exception error)
            {
            }
            try
            {
                PropertyDescriptor property = TypeDescriptor.GetProperties(e.Component).Find("ViewControlObject", false);
                if (property != null)
                    property.SetValue(e.Component, this);

                property = TypeDescriptor.GetProperties(e.Component).Find("ListConnection", false);
                if (property != null)
                {
                    object listConnection = property.GetValue(e.Component);
                    if (listConnection != null)
                    {
                        property = property = TypeDescriptor.GetProperties(listConnection).Find("ViewControlObject", false);
                        property.SetValue(listConnection, this);
                    }
                }

                property = TypeDescriptor.GetProperties(e.Component).Find("MasterViewControlObject", false);
                if (property != null)
                    property.SetValue(e.Component, this);

            }
            catch (Exception error)
            {


            }


        }
        #endregion

        #region Meta data
        /// <MetaDataID>{824B106E-497E-4FA3-8DD7-E4BE3CBA8A23}</MetaDataID>
        private void SychronizeMetaData(System.Windows.Forms.Control control)
        {

            OOAdvantech.UserInterface.ContainerControl containerControl = GetUserInterfaceMetaObject(GetControlIdentintity(control)) as OOAdvantech.UserInterface.ContainerControl;
            if (containerControl == null && control.Controls.Count == 0)
                return;


            foreach (System.ComponentModel.Component component in _DesignerHost.Container.Components)
            {
                if (component is System.Windows.Forms.Control && control.Controls.Contains(component as System.Windows.Forms.Control))
                {
                    string identity = GetControlIdentintity(component as System.Windows.Forms.Control);
                    OOAdvantech.UserInterface.Component userInterfaceMetaObject = GetUserInterfaceMetaObject(identity);
                    if (userInterfaceMetaObject == null)
                        userInterfaceMetaObject = CreateUserInterfaceMetaObject(component, identity);
                    if (userInterfaceMetaObject is OOAdvantech.UserInterface.Control)
                        containerControl.AddControl(userInterfaceMetaObject as OOAdvantech.UserInterface.Control);
                }
            }
            foreach (System.Windows.Forms.Control containedControl in control.Controls)
                SychronizeMetaData(containedControl);

        }

        /// <MetaDataID>{7ABA07D4-9C97-4A47-9376-3BCF3AE3BBC8}</MetaDataID>
        private OOAdvantech.UserInterface.Component CreateUserInterfaceMetaObject(IComponent component, string identity)
        {
            if (component is System.Windows.Forms.Control)
            {
                OOAdvantech.UserInterface.MemberAccessControl memberAccessControl = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ObjectView).NewObject(typeof(OOAdvantech.UserInterface.MemberAccessControl)) as OOAdvantech.UserInterface.MemberAccessControl;
                memberAccessControl.Identity = identity;
                memberAccessControl.Name = (component as System.Windows.Forms.Control).Name;
                memberAccessControl.Location = (component as System.Windows.Forms.Control).Location;
                memberAccessControl.Size = (component as System.Windows.Forms.Control).Size;
                return memberAccessControl;
            }
            return null;

        }

        /// <MetaDataID>{2021BC81-0843-49F2-BB1D-C9042360B21C}</MetaDataID>
        private OOAdvantech.UserInterface.Component GetUserInterfaceMetaObject(string identity)
        {
            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ObjectView);


            OOAdvantech.Collections.StructureSet set = objectStorage.Execute("SELECT component FROM OOAdvantech.UserInterface.Component component WHERE component.Identity = '" + identity + "'");
            foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                return setInstance["component"] as OOAdvantech.UserInterface.Component;
            return null;
        }

        /// <MetaDataID>{D3E41EBA-3650-47CE-B7EF-1B037FF8FA67}</MetaDataID>
        private string GetControlIdentintity(System.Windows.Forms.Control control)
        {
            if (control.FindForm() == control)
                return ObjectView.Identity;
            if (ContainerControl == null)
                throw new System.Exception("Initialization error");
            if (control == null)
                return "";
            return ObjectView.Identity + "." + control.Name;

            //if(control == DesignerHost
        }


        OOAdvantech.UserInterface.ObjectView ObjectView;
        XDocument FormMetaDataAsXmlDocument;

        [Editor(typeof(EditListMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object MetaData
        {
            get
            {
                if (FormMetaDataAsXmlDocument == null)
                    return new UserInterfaceMetaData.MetaDataValue("", null);
                UserInterfaceMetaData.MetaDataValue metaDataVaue = new UserInterfaceMetaData.MetaDataValue(FormMetaDataAsXmlDocument.ToString() as string, null);
                metaDataVaue.MetaDataAsObject = this;
                return metaDataVaue;
            }
            set
            {
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage objectViewStorage = null;



                if (FormMetaDataAsXmlDocument == null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    FormMetaDataAsXmlDocument = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                        {
                            FormMetaDataAsXmlDocument=XDocument.Parse(metaData);
                            objectViewStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporaryObjectViewStorage", FormMetaDataAsXmlDocument, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }
                        else
                        {
                            FormMetaDataAsXmlDocument = null;
                            return;

                        }


                    }
                    catch (System.Exception error)
                    {
                        FormMetaDataAsXmlDocument = null;
                        return;
                    }
                    //try
                    //{
                    OOAdvantech.Collections.StructureSet set = objectViewStorage.Execute("SELECT objectView FROM OOAdvantech.UserInterface.ObjectView  objectView");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        ObjectView = setInstance["objectView"] as OOAdvantech.UserInterface.ObjectView;
                        break;
                    }
                    //}
                    //catch (System.Exception error)
                    //{
                    //}
                    if (ObjectView == null)
                    {
                        FormMetaDataAsXmlDocument = null;
                        return;
                    }

                }
                return;
            }
        }
        #endregion



        public override System.Windows.Forms.Control ContainerControl
        {
            set
            {
                if (DesignMode && this._DesignerHost != null && _DesignerHost.RootComponent != value)
                    return;


                if (value.IsHandleCreated && !DesignMode)
                {
                    System.Diagnostics.Debug.WriteLine("FromConnectionControl must be initialiazed before system creates window for the Container control");
                    throw new System.Exception("FromConnectionControl must be initialiazed before system creates window for the Container control");
                }

                if (value != null && _ContainerControl != value)
                {
                    _ContainerControl = value;
                    if (!DesignMode)
                    {
                        ContainerControl.FindForm().Load += new EventHandler(OnHostFormLoad);
                        ContainerControl.FindForm().Closed += new EventHandler(OnHostFormClosed);
                    }
                }
                _ContainerControl = value;

            }
            get
            {
                return _ContainerControl;

            }
        }

        #region Transactions




        void OnHostFormClosed(object sender, EventArgs e)
        {

            try
            {
                ContainerControl.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                UnHookEventForDragAndDrop(_ContainerControl);
                RefreshControlsTimer.Tick -= new EventHandler(OnRefreshControls);
                RefreshControlsTimer.Interval = 1000;
                RefreshControlsTimer.Enabled = false;

                foreach (System.Windows.Forms.Control control in new List<System.Windows.Forms.Control>(HookedForDragAndDropControls))
                {
                    UnHookEventForDragAndDrop(control);
                }
                HookedForDragAndDropControls.Clear();

                System.Windows.Forms.Form hostform = _ContainerControl.FindForm();
                OOAdvantech.UserInterface.Runtime.DialogResult dialogResult = (OOAdvantech.UserInterface.Runtime.DialogResult)(int)(hostform.DialogResult);
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormClosed(dialogResult);

            }
            finally
            {
                ContainerControl.Cursor = System.Windows.Forms.Cursors.Default;

            }

        }
        bool _SkipErrorCheck = false;
        [Category("Object Model Connection")]
        public bool SkipErrorCheck
        {
            get
            {
                return _SkipErrorCheck;
            }
            set
            {
                _SkipErrorCheck = value;
            }
        }

        System.Windows.Forms.Timer RefreshControlsTimer = new System.Windows.Forms.Timer();
        void OnHostFormLoad(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                (UserInterfaceObjectConnection as OOAdvantech.UserInterface.Runtime.FormObjectConnection).HostFormLoad();
                HookEventForDragAndDrop(_ContainerControl);
                RefreshControlsTimer.Tick += new EventHandler(OnRefreshControls);
                RefreshControlsTimer.Interval = 500;
                RefreshControlsTimer.Enabled = true;

                if (!SkipErrorCheck)
                {
                    try
                    {
                        System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError> errors = new List<OOAdvantech.MetaDataRepository.MetaObject.MetaDataError>();
                        UserInterfaceObjectConnection.ErrorCheck(ref errors);
                        if (errors.Count > 0)
                        {
                            string errorsString = null;
                            foreach (OOAdvantech.MetaDataRepository.MetaObject.MetaDataError error in errors)
                            {
                                if (errorsString != null)
                                    errorsString += "\r\n";
                                errorsString += error.ErrorMessage;
                                errorsString += "\r\n";
                                errorsString += "in: " + error.ErrorPath;
                            }
                            System.Windows.Forms.MessageBox.Show(errorsString);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }



            }



        }

        void OnRefreshControls(object sender, EventArgs e)
        {
            UserInterfaceObjectConnection.RefreshUserInterface();

        }


        #endregion





    }
}
