using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace ConnectableControls.SchedulerControl
{
    // <summary>
    /// A custom TypeConverter used to help convert CellPadding objects from 
    /// one Type to another
    /// </summary>
    internal class CellPaddingConverter : TypeConverter
    {
        #region public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        /// <summary>
        /// Returns whether this converter can convert an object of the 
        /// given type to the type of this converter, using the specified context
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides 
        /// a format context</param>
        /// <param name="sourceType">A Type that represents the type you 
        /// want to convert from</param>
        /// <returns>true if this converter can perform the conversion; 
        /// otherwise, false</returns>
        /// <MetaDataID>{9AC85376-4B4B-41D9-9CAB-53CC77B821EC}</MetaDataID>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        } 
        #endregion

        #region public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        /// <summary>
        /// Returns whether this converter can convert the object to the 
        /// specified type, using the specified context
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a 
        /// format context</param>
        /// <param name="destinationType">A Type that represents the type you 
        /// want to convert to</param>
        /// <returns>true if this converter can perform the conversion; 
        /// otherwise, false</returns>
        /// <MetaDataID>{2D16EF73-9074-4EB0-883C-7D5199EB6292}</MetaDataID>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        } 
        #endregion

        #region public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        /// <summary>
        /// Converts the given object to the type of this converter, using 
        /// the specified context and culture information
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a 
        /// format context</param>
        /// <param name="culture">The CultureInfo to use as the current culture</param>
        /// <param name="value">The Object to convert</param>
        /// <returns>An Object that represents the converted value</returns>
        /// <MetaDataID>{D47B95ED-6925-430A-8CE7-3D1F1CF6D7D5}</MetaDataID>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string text = ((string)value).Trim();

                if (text.Length == 0)
                {
                    return null;
                }

                if (culture == null)
                {
                    culture = CultureInfo.CurrentCulture;
                }

                char[] listSeparators = culture.TextInfo.ListSeparator.ToCharArray();

                string[] s = text.Split(listSeparators);

                if (s.Length < 4)
                {
                    return null;
                }

                return new CellPadding(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
            }

            return base.ConvertFrom(context, culture, value);
        }
        
        #endregion

        #region public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        /// <summary>
        /// Converts the given value object to the specified type, using 
        /// the specified context and culture information
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides 
        /// a format context</param>
        /// <param name="culture">A CultureInfo object. If a null reference 
        /// is passed, the current culture is assumed</param>
        /// <param name="value">The Object to convert</param>
        /// <param name="destinationType">The Type to convert the value 
        /// parameter to</param>
        /// <returns>An Object that represents the converted value</returns>
        /// <MetaDataID>{31819974-C367-448D-B5A7-B775A6D601A3}</MetaDataID>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if ((destinationType == typeof(string)) && (value is CellPadding))
            {
                CellPadding p = (CellPadding)value;

                if (culture == null)
                {
                    culture = CultureInfo.CurrentCulture;
                }

                string separator = culture.TextInfo.ListSeparator + " ";

                TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));

                string[] s = new string[4];

                s[0] = converter.ConvertToString(context, culture, p.Left);
                s[1] = converter.ConvertToString(context, culture, p.Top);
                s[2] = converter.ConvertToString(context, culture, p.Right);
                s[3] = converter.ConvertToString(context, culture, p.Bottom);

                return string.Join(separator, s);
            }

            if ((destinationType == typeof(InstanceDescriptor)) && (value is CellPadding))
            {
                CellPadding p = (CellPadding)value;

                Type[] t = new Type[4];
                t[0] = t[1] = t[2] = t[3] = typeof(int);

                ConstructorInfo info = typeof(CellPadding).GetConstructor(t);

                if (info != null)
                {
                    object[] o = new object[4];

                    o[0] = p.Left;
                    o[1] = p.Top;
                    o[2] = p.Right;
                    o[3] = p.Bottom;

                    return new InstanceDescriptor(info, o);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        } 
        #endregion

        #region public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        /// <summary>
        /// Creates an instance of the Type that this TypeConverter is associated 
        /// with, using the specified context, given a set of property values for 
        /// the object
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format 
        /// context</param>
        /// <param name="propertyValues">An IDictionary of new property values</param>
        /// <returns>An Object representing the given IDictionary, or a null 
        /// reference if the object cannot be created</returns>
        /// <MetaDataID>{9E984D9E-6C59-4759-A46C-F0D1119BCC80}</MetaDataID>
        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return new CellPadding((int)propertyValues["Left"],
                (int)propertyValues["Top"],
                (int)propertyValues["Right"],
                (int)propertyValues["Bottom"]);
        }
        
        #endregion

        #region public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        /// <summary>
        /// Returns whether changing a value on this object requires a call to 
        /// CreateInstance to create a new value, using the specified context
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a 
        /// format context</param>
        /// <returns>true if changing a property on this object requires a call 
        /// to CreateInstance to create a new value; otherwise, false</returns>
        /// <MetaDataID>{46097C96-F0E8-4E32-BCE1-FD2BD984A779}</MetaDataID>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        } 
        #endregion

        /// <summary>
        /// Returns a collection of properties for the type of array specified 
        /// by the value parameter, using the specified context and attributes
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format 
        /// context</param>
        /// <param name="value">An Object that specifies the type of array for 
        /// which to get properties</param>
        /// <param name="attributes">An array of type Attribute that is used as 
        /// a filter</param>
        /// <returns>A PropertyDescriptorCollection with the properties that are 
        /// exposed for this data type, or a null reference if there are no 
        /// properties</returns>
        /// <MetaDataID>{215611E2-2408-45E3-9BD4-127A041EC0F5}</MetaDataID>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(CellPadding), attributes);

            string[] s = new string[4];
            s[0] = "Left";
            s[1] = "Top";
            s[2] = "Right";
            s[3] = "Bottom";

            return collection.Sort(s);
        }


        /// <summary>
        /// Returns whether this object supports properties, using the specified context
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context</param>
        /// <returns>true if GetProperties should be called to find the properties of this 
        /// object; otherwise, false</returns>
        /// <MetaDataID>{6A573E9D-E3BF-4036-A2B1-75D992ECE200}</MetaDataID>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

    }
}
