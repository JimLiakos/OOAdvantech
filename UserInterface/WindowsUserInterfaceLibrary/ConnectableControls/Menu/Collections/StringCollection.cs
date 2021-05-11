// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetConnectableControls.com
// *****************************************************************************

using System;
using System.Xml;
using ConnectableControls.Menus.Collections;

namespace ConnectableControls.Menus.Collections
{
	/// <MetaDataID>{F329A67D-FA07-48C9-9A09-AA3853FD8269}</MetaDataID>
    public class StringCollection : CollectionWithEvents
    {
		/// <MetaDataID>{BEDFFEB5-3D01-47F9-856C-AE3E7AEED05B}</MetaDataID>
        public String Add(String value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <MetaDataID>{A4E08728-5F61-40DC-9190-7ED4C12AA8C2}</MetaDataID>
        public void AddRange(String[] values)
        {
            // Use existing method to add each array entry
            foreach(String item in values)
                Add(item);
        }

		/// <MetaDataID>{0E0889C2-5F49-4679-B964-C0B5611EF108}</MetaDataID>
        public void Remove(String value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <MetaDataID>{0F4D9216-6ED3-4710-8E15-1F5CBBE17A7A}</MetaDataID>
        public void Insert(int index, String value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <MetaDataID>{4E7E4C64-941E-45F3-AEEE-90B541699D68}</MetaDataID>
        public bool Contains(String value)
        {
			// Value comparison
			foreach(String s in base.List)
				if (value.Equals(s))
					return true;

			return false;
        }

		/// <MetaDataID>{4482965B-6DF1-4D27-9255-507FA084CE63}</MetaDataID>
        public bool Contains(StringCollection values)
        {
			foreach(String c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

        public String this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as String); }
        }

		/// <MetaDataID>{A9E146DC-5413-4FEB-9102-151AEC4489A8}</MetaDataID>
        public int IndexOf(String value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		/// <MetaDataID>{F999ECEE-7725-4C1B-9889-2D8304D82A91}</MetaDataID>
		public void SaveToXml(string name, XmlTextWriter xmlOut)
		{
			xmlOut.WriteStartElement(name);
			xmlOut.WriteAttributeString("Count", this.Count.ToString());

			foreach(String s in base.List)
			{
				xmlOut.WriteStartElement("Item");
				xmlOut.WriteAttributeString("Name", s);
				xmlOut.WriteEndElement();
			}

			xmlOut.WriteEndElement();
		}

		/// <MetaDataID>{A388414F-9B04-4A34-A027-B1C1456B6667}</MetaDataID>
		public void LoadFromXml(string name, XmlTextReader xmlIn)
		{
			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != name)
				throw new ArgumentException("Incorrect node name found");

			this.Clear();

			// Grab raw position information
			string attrCount = xmlIn.GetAttribute(0);

			// Convert from string to proper types
			int count = int.Parse(attrCount);

			for(int index=0; index<count; index++)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != "Item")
					throw new ArgumentException("Incorrect node name found");

				this.Add(xmlIn.GetAttribute(0));
			}

			if (count > 0)
			{
				// Move over the end element of the collection
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != name)
					throw new ArgumentException("Incorrect node name found");
			}
		}
    }
}
