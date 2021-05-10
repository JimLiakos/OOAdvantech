namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;
    using Transactions;
    using System;
    /// <MetaDataID>{857D76D6-3520-4689-9DEB-D8BC090FF776}</MetaDataID>
    [BackwardCompatibilityID("{857D76D6-3520-4689-9DEB-D8BC090FF776}")]
    [Persistent()]
    public class PresentationInfo
    {
        /// <MetaDataID>{575E3CF7-D4CD-44B5-A8EA-9A828F030688}</MetaDataID>
        protected ObjectStateManagerLink Properties;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{64ADFF23-7C62-4196-B4D5-53C6F3849B6D}</MetaDataID>
        private string _Caption;
        /// <MetaDataID>{8FF352F4-37D4-4FCB-8970-33B2C4D99E9E}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_Caption")]
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                if (_Caption != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _Caption = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{C49D51F3-FA6C-44D6-9D5A-1E89A3D62A23}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember()]
        private int Height;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{AAE0D270-399C-47E1-917B-B31FB2A645BF}</MetaDataID>
        private string _Identity;
        /// <MetaDataID>{29DE6EEA-4DFD-48ED-A069-F509BC8FCBB7}</MetaDataID>
        [BackwardCompatibilityID("+1")]
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
        /// <MetaDataID>{44192FE3-A5B5-4EA4-96AF-B7D4B2700B79}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        [PersistentMember()]
        private int Xpos;
        /// <MetaDataID>{1294BEDB-FDA9-480D-9B1B-E25518D958A4}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [PersistentMember()]
        private int Ypos;
        /// <MetaDataID>{0982998E-3267-4F09-8895-860A7B2722C0}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember()]
        private System.Xml.XmlDocument XMLDynamicProperties;
        /// <MetaDataID>{7E1AF47A-5B5F-408E-8B9B-9E6D62B1AB25}</MetaDataID>
        public System.Drawing.Size Size
        {
            set
            {
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    Height = value.Height;
                    Width = value.Width;
                    stateTransition.Consistent = true;
                }
            }
            get
            {
                return new System.Drawing.Size(Width, Height);

            }
        }
        /// <MetaDataID>{C86E9970-6E1A-49A5-BF74-335B346CFCF6}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember()]
        private int Width;
        /// <MetaDataID>{820EB253-6C70-4E78-89F4-C3533621A461}</MetaDataID>
        public System.Drawing.Point Location
        {
            get
            {

                return new System.Drawing.Point(Xpos, Ypos);
            }
            set
            {
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                {
                    Xpos = value.X;
                    Ypos = value.Y;
                    stateTransition.Consistent = true;
                }


            }
        }
        /// <MetaDataID>{2DFC9225-CEA9-4A99-81D7-D5DED19C7C75}</MetaDataID>
        /// <summary>This function retrieves the current value of a property of an metaobject, given a property and group name. </summary>
        /// <param name="PropertyNamespace">Name of the Namespace for which a property value is being retrieved </param>
        /// <param name="PropertyName">Name of the property whose value is being retrieved </param>
        public object GetPropertyValue(Type PropertyType, string PropertyNamespace, string PropertyName)
        {
            if (XMLDynamicProperties == null)
                return null;
            if (XMLDynamicProperties.ChildNodes.Count == 0)
                return null;
            System.Xml.XmlElement XMLTextElement = (System.Xml.XmlElement)XMLDynamicProperties.ChildNodes[0];
            System.Xml.XmlElement NamespaceElement = null;
            foreach (System.Xml.XmlNode CurrNode in XMLTextElement.ChildNodes)
            {
                if (CurrNode.Name == PropertyNamespace)
                {
                    NamespaceElement = (System.Xml.XmlElement)CurrNode;
                    break;
                }
            }
            if (NamespaceElement == null)
                return null;
            string PropertyValue = NamespaceElement.GetAttribute(PropertyName);

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
        /// <MetaDataID>{68AFC308-9381-4EDA-8223-789BD07191C1}</MetaDataID>
        /// <summary>This method set the value of a metaobject property. </summary>
        /// <param name="PropertyName">Name of the property whose value is being retrieved </param>
        /// <param name="PropertyValue">Value being set </param>
        public void PutPropertyValue(string PropertyNamespace, string PropertyName, object PropertyValue)
        {
            if (PropertyValue == null)
                return;
            if (XMLDynamicProperties == null)
                XMLDynamicProperties = new System.Xml.XmlDocument();
            System.Xml.XmlElement XMLTextElement = null;
            System.Xml.XmlElement NamespaceElement = null;
            if (XMLDynamicProperties.ChildNodes.Count == 0)
            {
                XMLTextElement = XMLDynamicProperties.CreateElement("XMLText");
                XMLDynamicProperties.AppendChild(XMLTextElement);
            }
            else
                XMLTextElement = (System.Xml.XmlElement)XMLDynamicProperties.ChildNodes[0];
            foreach (System.Xml.XmlNode CurrNode in XMLTextElement.ChildNodes)
            {
                if (CurrNode.Name == PropertyNamespace)
                {
                    NamespaceElement = (System.Xml.XmlElement)CurrNode;
                    break;
                }
            }
            if (NamespaceElement == null)
            {
                NamespaceElement = XMLDynamicProperties.CreateElement(PropertyNamespace);
                XMLTextElement.AppendChild(NamespaceElement);
            }
            if (PropertyValue != null)
            {
                string StringValue;
                if (PropertyValue.GetType().BaseType == typeof(System.Enum))
                {
                    StringValue = PropertyValue.ToString();
                    NamespaceElement.SetAttribute(PropertyName, StringValue);
                }
                else
                {
                    StringValue = PropertyValue.ToString();

                    NamespaceElement.SetAttribute(PropertyName, StringValue);

                }
            }
        }
    }
}
