namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;
    using System;
    using System.Xml.Linq;
    using System.Linq;

    /// <MetaDataID>{364CDD63-4029-4BBD-8B07-82CED3A3C157}</MetaDataID>
    [BackwardCompatibilityID("{364CDD63-4029-4BBD-8B07-82CED3A3C157}")]
    [Persistent()]
    public class Component
    { 
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{779D7723-FB6D-49B7-94F2-93C08E83D9E1}</MetaDataID>
        private string _Identity;
        /// <MetaDataID>{25796BC6-A264-4933-A6EE-69AE5C900B5E}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember("_Identity")]
        public string Identity
        {
            get
            {
                return _Identity;
            }
            set
            {
                if (_Identity != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Identity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{A359AED1-B371-471C-BCE6-5EEC1E9CA99A}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember()]
        private XDocument  XMLDynamicProperties;
        /// <summary>This function retrieves the current value of a property of an metaobject, given a property and group name. </summary>
        /// <param name="PropertyNamespace">Name of the Namespace for which a property value is being retrieved </param>
        /// <param name="PropertyName">Name of the property whose value is being retrieved </param>
        /// <MetaDataID>{2D477A6A-C4CC-43CA-93EB-ECBEACFDE4D0}</MetaDataID>
        public object GetPropertyValue(Type PropertyType, string PropertyNamespace, string PropertyName)
        {
            if (XMLDynamicProperties == null)
                return null;
            if (XMLDynamicProperties.Root.Elements().Count() == 0)
                return null;
            XElement XMLTextElement = (XElement)XMLDynamicProperties.Root.Elements().ToArray()[0];
            XElement NamespaceElement = null;
            foreach (XElement CurrNode in XMLTextElement.Elements())
            {
                if (CurrNode.Name == PropertyNamespace)
                {
                    NamespaceElement = CurrNode;
                    break;
                }
            }
            if (NamespaceElement == null)
                return null;
            string PropertyValue = "";
            if (NamespaceElement.Attribute(PropertyName)!=null)
                PropertyValue= NamespaceElement.Attribute(PropertyName).Value;

            if (PropertyValue.Length == 0)
                return null;

            if (PropertyType == PropertyValue.GetType())
                return PropertyValue;
            if (PropertyType.BaseType == typeof(System.Enum))
            {
                if (PropertyValue != null)
                    if (PropertyValue.Length > 0)
                        return System.Enum.Parse(PropertyType, PropertyValue, false);
                return null;
            }
            else
            {
                if (PropertyType == typeof(System.DateTime))
                    return System.Convert.ChangeType(PropertyValue, PropertyType, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                else
                    return System.Convert.ChangeType(PropertyValue, PropertyType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
            }

        }
        /// <summary>This method set the value of a metaobject property. </summary>
        /// <param name="PropertyName">Name of the property whose value is being retrieved </param>
        /// <param name="PropertyValue">Value being set </param>
        /// <MetaDataID>{D9E74F5F-4D24-41A4-828C-370194BDC950}</MetaDataID>
        public void PutPropertyValue(string PropertyNamespace, string PropertyName, object PropertyValue)
        {
            if (PropertyValue == null)
                return;
            if (XMLDynamicProperties == null)
                XMLDynamicProperties = new XDocument();
            XElement XMLTextElement = null;
            XElement NamespaceElement = null;
            if (XMLDynamicProperties.Root.Elements().Count() == 0)
            {
                XMLTextElement = new XElement("XMLText");
                XMLDynamicProperties.Root.Add(XMLTextElement);
            }
            else
                XMLTextElement = (XElement)XMLDynamicProperties.Root.Elements().ToArray()[0];
            foreach (XElement CurrNode in XMLTextElement.Elements())
            {
                if (CurrNode.Name == PropertyNamespace)
                {
                    NamespaceElement = CurrNode;
                    break;
                }
            }
            if (NamespaceElement == null)
            {
                NamespaceElement =new XElement(PropertyNamespace);
                XMLTextElement.Add(NamespaceElement);
            }
            if (PropertyValue != null)
            {
                string StringValue;
                if (PropertyValue.GetType().BaseType == typeof(System.Enum))
                {
                    StringValue = PropertyValue.ToString();
                    NamespaceElement.SetAttributeValue(PropertyName, StringValue);
                }
                else
                {
                    StringValue = PropertyValue.ToString();

                    NamespaceElement.SetAttributeValue(PropertyName, StringValue);

                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{76847E6B-3875-45E9-8625-68B7FE258E68}</MetaDataID>
        private string _Type;
        /// <MetaDataID>{6C48C1FE-8495-4CBA-919F-FEA628058C7F}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_Type")]
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (_Type != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Type = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3A16EFE5-3029-43EE-B74B-BA66AEEE3562}</MetaDataID>
        protected string _Name;
        /// <MetaDataID>{2AA22091-1746-4622-9F83-B8741DFD3BAB}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_Name")]
        [System.ComponentModel.Browsable(true)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{16CD494C-D6C1-4E11-8637-DCE381CC97CA}</MetaDataID>
        protected ObjectStateManagerLink Properties;

    }
}
