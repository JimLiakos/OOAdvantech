using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.IO;
using System.ComponentModel.Design.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Windows.Forms;

namespace DXConnectableControls
{
    public class ComponentLoader
    { 
        /// Reads the "Object" tags. This returns an instance of the
        /// newly created object. Returns null if there was an error.
        private object ReadObject(XmlNode node, ArrayList errors)
        {
            XmlAttribute typeAttr = node.Attributes["type"];

            if (typeAttr == null)
            {
                errors.Add("<Object> tag is missing required type attribute");
                return null;
            }

            Type type = Type.GetType(typeAttr.Value);

            if (type == null)
            {
                errors.Add(string.Format("Type {0} could not be loaded.", typeAttr.Value));
                return null;
            }

            // This can be null if there is no name for the object.
            //
            XmlAttribute nameAttr = node.Attributes["name"];
            object instance;

            if (typeof(IComponent).IsAssignableFrom(type))
            {
                if (nameAttr == null)
                {
                    instance = host.CreateComponent(type);
                }
                else
                {
                    instance = host.CreateComponent(type, nameAttr.Value);
                }
            }
            else
            {
                instance = Activator.CreateInstance(type);
            }

            // Got an object, now we must process it.  Check to see if this tag
            // offers a child collection for us to add children to.
            //
            XmlAttribute childAttr = node.Attributes["children"];
            IList childList = null;

            if (childAttr != null)
            {
                PropertyDescriptor childProp = TypeDescriptor.GetProperties(instance)[childAttr.Value];

                if (childProp == null)
                {
                    errors.Add(string.Format("The children attribute lists {0} as the child collection but this is not a property on {1}", childAttr.Value, instance.GetType().FullName));
                }
                else
                {
                    childList = childProp.GetValue(instance) as IList;
                    if (childList == null)
                    {
                        errors.Add(string.Format("The property {0} was found but did not return a valid IList", childProp.Name));
                    }
                }
            }

            // Now, walk the rest of the tags under this element.
            //
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name.Equals("Object"))
                {
                    // Another object.  In this case, create the object, and
                    // parent it to ours using the children property.  If there
                    // is no children property, bail out now.
                    if (childAttr == null)
                    {
                        errors.Add("Child object found but there is no children attribute");
                        continue;
                    }

                    // no sense doing this if there was an error getting the property.  We've already reported the
                    // error above.
                    if (childList != null)
                    {
                        object childInstance = ReadObject(childNode, errors);

                        childList.Add(childInstance);
                    }
                }
                else if (childNode.Name.Equals("Property"))
                {
                    // A property.  Ask the property to parse itself.
                    //
                    ReadProperty(childNode, instance, errors);
                }
                else if (childNode.Name.Equals("Event"))
                {
                    // An event.  Ask the event to parse itself.
                    //
                    ReadEvent(childNode, instance, errors);
                }
            }

            return instance;
        }


        private XmlNode WriteObject(XmlDocument document, IDictionary nametable, object value)
        {
            IDesignerHost idh = (IDesignerHost)this.host.GetService(typeof(IDesignerHost));
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
                Debug.Assert(nametable[component] == null, "WriteObject should not be called more than once for the same object.  Use WriteReference instead");
                nametable[value] = component.Site.Name;
            }

            bool isControl = (value is Control);

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
                    foreach (Control child in ((Control)value).Controls)
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
                IEventBindingService bindings = host.GetService(typeof(IEventBindingService)) as IEventBindingService;

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
        /// <summary>
        /// Simple helper method that returns true if the given type converter supports
        /// two-way conversion of the given type.
        /// </summary>
        private bool GetConversionSupported(TypeConverter converter, Type conversionType)
        {
            return (converter.CanConvertFrom(conversionType) && converter.CanConvertTo(conversionType));
        }
        private bool WriteValue(XmlDocument document, object value, XmlNode parent)
        {
            IDesignerHost idh = (IDesignerHost)this.host.GetService(typeof(IDesignerHost));

            // For empty values, we just return.  This creates an empty node.
            if (value == null)
            {
                return true;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(value);

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

        private XmlNode WriteBinary(XmlDocument document, byte[] value)
        {
            XmlNode node = document.CreateElement("Binary");

            node.InnerText = Convert.ToBase64String(value);
            return node;
        }

        private XmlNode WriteReference(XmlDocument document, IComponent value)
        {
            IDesignerHost idh = (IDesignerHost)this.host.GetService(typeof(IDesignerHost));

            Debug.Assert(value != null && value.Site != null && value.Site.Container == idh.Container, "Invalid component passed to WriteReference");

            XmlNode node = document.CreateElement("Reference");
            XmlAttribute attr = document.CreateAttribute("name");

            attr.Value = value.Site.Name;
            node.Attributes.Append(attr);
            return node;
        }

    }
}
