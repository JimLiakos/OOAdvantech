#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
#if HAVE_INOTIFY_COLLECTION_CHANGED
using System.Collections.ObjectModel;
using System.Collections.Specialized;
#endif
using System.ComponentModel;
#if HAVE_DYNAMIC
using System.Dynamic;
using System.Linq.Expressions;
#endif
using System.IO;
using OOAdvantech.Json.Utilities;
using System.Globalization;
#if !HAVE_LINQ
using OOAdvantech.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif

namespace OOAdvantech.Json.Linq
{
    /// <summary>
    /// Represents a JSON object.
    /// </summary>
    /// <example>
    ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParse" title="Parsing a JSON Object from Text" />
    /// </example>
    /// <MetaDataID>OOAdvantech.Json.Linq.JObject</MetaDataID>
    public partial class JObject : JContainer, IDictionary<string, JToken>, INotifyPropertyChanged
#if HAVE_COMPONENT_MODEL
        , ICustomTypeDescriptor
#endif
#if HAVE_INOTIFY_PROPERTY_CHANGING
        , INotifyPropertyChanging
#endif
    {
        /// <MetaDataID>{e9587f01-24bd-4ffc-9316-ae0a07a7ef0b}</MetaDataID>
        private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

        /// <summary>
        /// Gets the container's children tokens.
        /// </summary>
        /// <value>The container's children tokens.</value>
        /// <MetaDataID>{79916c20-c3f0-4864-8891-c248d7fdeaa3}</MetaDataID>
        protected override IList<JToken> ChildrenTokens => _properties;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

#if HAVE_INOTIFY_PROPERTY_CHANGING
        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="JObject" /> class.
        /// </summary>
        /// <MetaDataID>{a399a9bd-44bc-4035-95f6-9ada015f8c87}</MetaDataID>
        public JObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JObject" /> class from another <see cref="JObject" /> object.
        /// </summary>
        /// <param name="other">A <see cref="JObject" /> object to copy from.</param>
        /// <MetaDataID>{cb54a87e-da3b-45ac-937e-17ffa08fe083}</MetaDataID>
        public JObject(JObject other)
            : base(other)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JObject" /> class with the specified content.
        /// </summary>
        /// <param name="content">The contents of the object.</param>
        /// <MetaDataID>{e9a31de9-11d2-47a2-9155-239e36f347fa}</MetaDataID>
        public JObject(params object[] content)
            : this((object)content)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JObject" /> class with the specified content.
        /// </summary>
        /// <param name="content">The contents of the object.</param>
        /// <MetaDataID>{3b43341c-2758-47b3-86c8-8330f7524d35}</MetaDataID>
        public JObject(object content)
        {
            Add(content);
        }

        /// <MetaDataID>{f4f82c44-d8a5-4357-a429-64db325a57ab}</MetaDataID>
        internal override bool DeepEquals(JToken node)
        {
            if (!(node is JObject t))
            {
                return false;
            }

            return _properties.Compare(t._properties);
        }

        /// <MetaDataID>{92f6b595-193a-40e6-a7ac-12259a7de0f7}</MetaDataID>
        internal override int IndexOfItem(JToken item)
        {
            return _properties.IndexOfReference(item);
        }

        /// <MetaDataID>{2379f0df-5a29-4a31-ae62-58084aebcd8c}</MetaDataID>
        internal override void InsertItem(int index, JToken item, bool skipParentCheck)
        {
            // don't add comments to JObject, no name to reference comment by
            if (item != null && item.Type == JTokenType.Comment)
            {
                return;
            }

            base.InsertItem(index, item, skipParentCheck);
        }

        /// <MetaDataID>{aa277fba-d936-4238-8999-579cf1bff118}</MetaDataID>
        internal override void ValidateToken(JToken o, JToken existing)
        {
            ValidationUtils.ArgumentNotNull(o, nameof(o));

            if (o.Type != JTokenType.Property)
            {
                throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), GetType()));
            }

            JProperty newProperty = (JProperty)o;

            if (existing != null)
            {
                JProperty existingProperty = (JProperty)existing;

                if (newProperty.Name == existingProperty.Name)
                {
                    return;
                }
            }

            if (_properties.TryGetValue(newProperty.Name, out existing))
            {
                throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, newProperty.Name, GetType()));
            }
        }

        /// <MetaDataID>{0e3bb0b6-5eda-4a26-a0d3-ded199cb22c7}</MetaDataID>
        internal override void MergeItem(object content, JsonMergeSettings settings)
        {
            if (!(content is JObject o))
            {
                return;
            }

            foreach (KeyValuePair<string, JToken> contentItem in o)
            {
                JProperty existingProperty = Property(contentItem.Key, settings?.PropertyNameComparison ?? StringComparison.Ordinal);

                if (existingProperty == null)
                {
                    Add(contentItem.Key, contentItem.Value);
                }
                else if (contentItem.Value != null)
                {
                    if (!(existingProperty.Value is JContainer existingContainer) || existingContainer.Type != contentItem.Value.Type)
                    {
                        if (!IsNull(contentItem.Value) || settings?.MergeNullValueHandling == MergeNullValueHandling.Merge)
                        {
                            existingProperty.Value = contentItem.Value;
                        }
                    }
                    else
                    {
                        existingContainer.Merge(contentItem.Value, settings);
                    }
                }
            }
        }

        /// <MetaDataID>{2af67ebc-b720-4a44-8bef-802825a932ec}</MetaDataID>
        private static bool IsNull(JToken token)
        {
            if (token.Type == JTokenType.Null)
            {
                return true;
            }

            if (token is JValue v && v.Value == null)
            {
                return true;
            }

            return false;
        }

        /// <MetaDataID>{a461848a-1310-4dba-b277-f1172ab40c3f}</MetaDataID>
        internal void InternalPropertyChanged(JProperty childProperty)
        {
            OnPropertyChanged(childProperty.Name);
#if HAVE_COMPONENT_MODEL
            if (_listChanged != null)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, IndexOfItem(childProperty)));
            }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
            if (_collectionChanged != null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, childProperty, childProperty, IndexOfItem(childProperty)));
            }
#endif
        }

        /// <MetaDataID>{51b7df65-b766-4bf1-9564-8e94e536da1c}</MetaDataID>
        internal void InternalPropertyChanging(JProperty childProperty)
        {
#if HAVE_INOTIFY_PROPERTY_CHANGING
            OnPropertyChanging(childProperty.Name);
#endif
        }

        /// <MetaDataID>{15056543-d541-4038-9308-42cdfe2a116e}</MetaDataID>
        internal override JToken CloneToken()
        {
            return new JObject(this);
        }

        /// <summary>
        /// Gets the node type for this <see cref="JToken" />.
        /// </summary>
        /// <value>The type.</value>
        /// <MetaDataID>{717a1a34-f26c-4dc2-b580-eea1fb7d97bd}</MetaDataID>
        public override JTokenType Type => JTokenType.Object;

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}" /> of <see cref="JProperty" /> of this object's properties.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="JProperty" /> of this object's properties.</returns>
        /// <MetaDataID>{b1935064-bb5b-4ab0-a84f-fb81e3606e58}</MetaDataID>
        public IEnumerable<JProperty> Properties()
        {
            return _properties.Cast<JProperty>();
        }

        /// <summary>
        /// Gets a <see cref="JProperty" /> with the specified name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>A <see cref="JProperty" /> with the specified name or <c>null</c>.</returns>
        /// <MetaDataID>{12bab540-796f-4c8d-b503-55493371d7e3}</MetaDataID>
        public JProperty Property(string name)
        {
            return Property(name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the <see cref="JProperty" /> with the specified name.
        /// The exact name will be searched for first and if no matching property is found then
        /// the <see cref="StringComparison" /> will be used to match a property.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
        /// <returns>A <see cref="JProperty" /> matched with the specified name or <c>null</c>.</returns>
        /// <MetaDataID>{c49afd61-dc71-4e53-8244-69d447359910}</MetaDataID>
        public JProperty Property(string name, StringComparison comparison)
        {
            if (name == null)
            {
                return null;
            }

            if (_properties.TryGetValue(name, out JToken property))
            {
                return (JProperty)property;
            }

            // test above already uses this comparison so no need to repeat
            if (comparison != StringComparison.Ordinal)
            {
                for (int i = 0; i < _properties.Count; i++)
                {
                    JProperty p = (JProperty)_properties[i];
                    if (string.Equals(p.Name, name, comparison))
                    {
                        return p;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="JEnumerable{T}" /> of <see cref="JToken" /> of this object's property values.
        /// </summary>
        /// <returns>A <see cref="JEnumerable{T}" /> of <see cref="JToken" /> of this object's property values.</returns>
        /// <MetaDataID>{385ac598-0cda-4caf-95dd-dc7ad6f6dc8a}</MetaDataID>
        public JEnumerable<JToken> PropertyValues()
        {
            return new JEnumerable<JToken>(Properties().Select(p => p.Value));
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> with the specified key.
        /// </summary>
        /// <value>The <see cref="JToken" /> with the specified key.</value>
        /// <MetaDataID>{df93e247-d932-405c-a067-74503cb53ffc}</MetaDataID>
        public override JToken this[object key]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(key, nameof(key));

                if (!(key is string propertyName))
                {
                    throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
                }

                return this[propertyName];
            }
            set
            {
                ValidationUtils.ArgumentNotNull(key, nameof(key));

                if (!(key is string propertyName))
                {
                    throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
                }

                this[propertyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="JToken" /> with the specified property name.
        /// </summary>
        /// <value></value>
        /// <MetaDataID>{0be70b24-8701-483e-9b44-393a06c8e823}</MetaDataID>
        public JToken this[string propertyName]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));

                JProperty property = Property(propertyName, StringComparison.Ordinal);

                return property?.Value;
            }
            set
            {
                JProperty property = Property(propertyName, StringComparison.Ordinal);
                if (property != null)
                {
                    property.Value = value;
                }
                else
                {
#if HAVE_INOTIFY_PROPERTY_CHANGING
                    OnPropertyChanging(propertyName);
#endif
                    Add(new JProperty(propertyName, value));
                    OnPropertyChanged(propertyName);
                }
            }
        }

        /// <summary>
        /// Loads a <see cref="JObject" /> from a <see cref="JsonReader" />.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader" /> that will be read for the content of the <see cref="JObject" />.</param>
        /// <returns>A <see cref="JObject" /> that contains the JSON that was read from the specified <see cref="JsonReader" />.</returns>
        /// <exception cref="JsonReaderException">
        ///   <paramref name="reader" /> is not valid JSON.
        /// </exception>
        /// <MetaDataID>{190c5741-3089-43ed-84b3-57a898206e88}</MetaDataID>
        public new static JObject Load(JsonReader reader)
        {
            return Load(reader, null);
        }

        /// <summary>
        /// Loads a <see cref="JObject" /> from a <see cref="JsonReader" />.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader" /> that will be read for the content of the <see cref="JObject" />.</param>
        /// <param name="settings">The <see cref="JsonLoadSettings" /> used to load the JSON.
        /// If this is <c>null</c>, default load settings will be used.</param>
        /// <returns>A <see cref="JObject" /> that contains the JSON that was read from the specified <see cref="JsonReader" />.</returns>
        /// <exception cref="JsonReaderException">
        ///   <paramref name="reader" /> is not valid JSON.
        /// </exception>
        /// <MetaDataID>{495e750c-d600-440a-82b9-6c97b298f55f}</MetaDataID>
        public new static JObject Load(JsonReader reader, JsonLoadSettings settings)
        {
            ValidationUtils.ArgumentNotNull(reader, nameof(reader));

            if (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                {
                    throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
                }
            }

            reader.MoveToContent();

            if (reader.TokenType != JsonToken.StartObject)
            {
                throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
            }

            JObject o = new JObject();
            o.SetLineInfo(reader as IJsonLineInfo, settings);

            o.ReadTokenFrom(reader, settings);

            return o;
        }

        /// <summary>
        /// Load a <see cref="JObject" /> from a string that contains JSON.
        /// </summary>
        /// <param name="json">A <see cref="String" /> that contains JSON.</param>
        /// <returns>A <see cref="JObject" /> populated from the string that contains JSON.</returns>
        /// <exception cref="JsonReaderException">
        ///   <paramref name="json" /> is not valid JSON.
        /// </exception>
        /// <example>
        ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParse" title="Parsing a JSON Object from Text" />
        /// </example>
        /// <MetaDataID>{3d9c7401-7d09-4009-8304-cb59b371cbb9}</MetaDataID>
        public new static JObject Parse(string json)
        {
            return Parse(json, null);
        }

        /// <summary>
        /// Load a <see cref="JObject" /> from a string that contains JSON.
        /// </summary>
        /// <param name="json">A <see cref="String" /> that contains JSON.</param>
        /// <param name="settings">The <see cref="JsonLoadSettings" /> used to load the JSON.
        /// If this is <c>null</c>, default load settings will be used.</param>
        /// <returns>A <see cref="JObject" /> populated from the string that contains JSON.</returns>
        /// <exception cref="JsonReaderException">
        ///   <paramref name="json" /> is not valid JSON.
        /// </exception>
        /// <example>
        ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\LinqToJsonTests.cs" region="LinqToJsonCreateParse" title="Parsing a JSON Object from Text" />
        /// </example>
        /// <MetaDataID>{35fd0c84-e62e-486b-b874-fd4f57a921ba}</MetaDataID>
        public new static JObject Parse(string json, JsonLoadSettings settings)
        {
            using (JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                JObject o = Load(reader, settings);

                while (reader.Read())
                {
                    // Any content encountered here other than a comment will throw in the reader.
                }

                return o;
            }
        }

        /// <summary>
        /// Creates a <see cref="JObject" /> from an object.
        /// </summary>
        /// <param name="o">The object that will be used to create <see cref="JObject" />.</param>
        /// <returns>A <see cref="JObject" /> with the values of the specified object.</returns>
        /// <MetaDataID>{0f058cb1-2063-4e1d-9e00-7c4fc79baa04}</MetaDataID>
        public new static JObject FromObject(object o)
        {
            return FromObject(o, JsonSerializer.CreateDefault());
        }

        /// <summary>
        /// Creates a <see cref="JObject" /> from an object.
        /// </summary>
        /// <param name="o">The object that will be used to create <see cref="JObject" />.</param>
        /// <param name="jsonSerializer">The <see cref="JsonSerializer" /> that will be used to read the object.</param>
        /// <returns>A <see cref="JObject" /> with the values of the specified object.</returns>
        /// <MetaDataID>{0c9d96ec-1df5-41f8-b62c-c7c118d6da21}</MetaDataID>
        public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
        {
            JToken token = FromObjectInternal(o, jsonSerializer);

            if (token != null && token.Type != JTokenType.Object)
            {
                throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, token.Type));
            }

            return (JObject)token;
        }

        /// <summary>
        /// Writes this token to a <see cref="JsonWriter" />.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter" /> into which this method will write.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter" /> which will be used when writing the token.</param>
        /// <MetaDataID>{38606b38-f8dd-4935-a3f7-392d76ab80ea}</MetaDataID>
        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WriteStartObject();

            for (int i = 0; i < _properties.Count; i++)
            {
                _properties[i].WriteTo(writer, converters);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Gets the <see cref="Newtonsoft.Json.Linq.JToken" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="Newtonsoft.Json.Linq.JToken" /> with the specified property name.</returns>
        /// <MetaDataID>{11425a08-dbab-450c-a20b-71e581927ebe}</MetaDataID>
        public JToken GetValue(string propertyName)
        {
            return GetValue(propertyName, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the <see cref="Newtonsoft.Json.Linq.JToken" /> with the specified property name.
        /// The exact property name will be searched for first and if no matching property is found then
        /// the <see cref="StringComparison" /> will be used to match a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
        /// <returns>The <see cref="Newtonsoft.Json.Linq.JToken" /> with the specified property name.</returns>
        /// <MetaDataID>{0f750ac6-a6e5-484f-bd37-24bc1ea6d423}</MetaDataID>
        public JToken GetValue(string propertyName, StringComparison comparison)
        {
            if (propertyName == null)
            {
                return null;
            }

            // attempt to get value via dictionary first for performance
            var property = Property(propertyName, comparison);

            return property?.Value;
        }

        /// <summary>
        /// Tries to get the <see cref="Newtonsoft.Json.Linq.JToken" /> with the specified property name.
        /// The exact property name will be searched for first and if no matching property is found then
        /// the <see cref="StringComparison" /> will be used to match a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
        /// <returns>
        ///   <c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
        /// <MetaDataID>{d412494f-900f-4483-98a1-217f575fa134}</MetaDataID>
        public bool TryGetValue(string propertyName, StringComparison comparison, out JToken value)
        {
            value = GetValue(propertyName, comparison);
            return (value != null);
        }

        #region IDictionary<string,JToken> Members
        /// <summary>
        /// Adds the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{e9b251f2-6a5e-4689-9a43-d02015df75fe}</MetaDataID>
        public void Add(string propertyName, JToken value)
        {
            Add(new JProperty(propertyName, value));
        }

        /// <summary>
        /// Determines whether the JSON object has the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the JSON object has the specified property name; otherwise, <c>false</c>.</returns>
        /// <MetaDataID>{d870da46-c2f0-424d-bf79-50232cf55a28}</MetaDataID>
        public bool ContainsKey(string propertyName)
        {
            ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));

            return _properties.Contains(propertyName);
        }

        /// <MetaDataID>{58d1fc1e-7fa9-48de-96d3-bca076bab42d}</MetaDataID>
        ICollection<string> IDictionary<string, JToken>.Keys => _properties.Keys;

        /// <summary>
        /// Removes the property with the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if item was successfully removed; otherwise, <c>false</c>.</returns>
        /// <MetaDataID>{106eac62-b41b-4cc5-bb49-d9a0d60c6f1c}</MetaDataID>
        public bool Remove(string propertyName)
        {
            JProperty property = Property(propertyName, StringComparison.Ordinal);
            if (property == null)
            {
                return false;
            }

            property.Remove();
            return true;
        }

        /// <summary>
        /// Tries to get the <see cref="Newtonsoft.Json.Linq.JToken" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
        /// <MetaDataID>{b695d614-7e24-4377-b43f-0469152dfc0d}</MetaDataID>
        public bool TryGetValue(string propertyName, out JToken value)
        {
            JProperty property = Property(propertyName, StringComparison.Ordinal);
            if (property == null)
            {
                value = null;
                return false;
            }

            value = property.Value;
            return true;
        }

        /// <MetaDataID>{3a023510-7e6f-43d7-b585-c3d1a21a206d}</MetaDataID>
        ICollection<JToken> IDictionary<string, JToken>.Values => throw new NotImplementedException();

        #endregion

        #region ICollection<KeyValuePair<string,JToken>> Members
        /// <MetaDataID>{19d0b8fb-378b-4db2-b4c0-585c5e877030}</MetaDataID>
        void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
        {
            Add(new JProperty(item.Key, item.Value));
        }

        /// <MetaDataID>{36aae2a3-7d2f-4ba4-9ec0-8126c262920c}</MetaDataID>
        void ICollection<KeyValuePair<string, JToken>>.Clear()
        {
            RemoveAll();
        }

        /// <MetaDataID>{8bbc76c2-b64b-4a50-b700-b44a9ce75fab}</MetaDataID>
        bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
        {
            JProperty property = Property(item.Key, StringComparison.Ordinal);
            if (property == null)
            {
                return false;
            }

            return (property.Value == item.Value);
        }

        /// <MetaDataID>{a838f9eb-7401-47f1-ab1c-c3e433c4b56e}</MetaDataID>
        void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than 0.");
            }
            if (arrayIndex >= array.Length && arrayIndex != 0)
            {
                throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
            }
            if (Count > array.Length - arrayIndex)
            {
                throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
            }

            int index = 0;
            foreach (JProperty property in _properties)
            {
                array[arrayIndex + index] = new KeyValuePair<string, JToken>(property.Name, property.Value);
                index++;
            }
        }

        /// <MetaDataID>{3e872855-a0b8-4152-9638-e2ba8fa6ca7e}</MetaDataID>
        bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly => false;

        /// <MetaDataID>{a6cb426f-ff3e-4eff-b457-10728857f7c7}</MetaDataID>
        bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
        {
            if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item))
            {
                return false;
            }

                    ((IDictionary<string, JToken>)this).Remove(item.Key);
            return true;
        }
        #endregion

        /// <MetaDataID>{31598745-51e8-4f34-bd86-cbc9f56a4557}</MetaDataID>
        internal override int GetDeepHashCode()
        {
            return ContentsHashCode();
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        /// <MetaDataID>{967543e4-f48e-46a4-9696-5ffc0fa3a1c2}</MetaDataID>
        public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
        {
            foreach (JProperty property in _properties)
            {
                yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event with the provided arguments.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <MetaDataID>{c4f782a0-1297-43f7-be05-c6f7e2e1200f}</MetaDataID>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#if HAVE_INOTIFY_PROPERTY_CHANGING
        /// <summary>
        /// Raises the <see cref="PropertyChanging" /> event with the provided arguments.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <MetaDataID>{54afe06b-5851-421b-99f0-70a7a1a1624a}</MetaDataID>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
#endif

#if HAVE_COMPONENT_MODEL
        // include custom type descriptor on JObject rather than use a provider because the properties are specific to a type

        #region ICustomTypeDescriptor
        /// <MetaDataID>{793b46c8-2a19-45e0-95b1-7ff944517a95}</MetaDataID>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        /// <MetaDataID>{0262b25f-390f-43d1-ab52-b4e3321e56fc}</MetaDataID>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection descriptors = new PropertyDescriptorCollection(null);

            foreach (KeyValuePair<string, JToken> propertyValue in this)
            {
                descriptors.Add(new JPropertyDescriptor(propertyValue.Key));
            }

            return descriptors;
        }

        /// <MetaDataID>{9341e02f-a42e-4b86-8a90-ce59bdfcd4b0}</MetaDataID>
        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        /// <MetaDataID>{2388dbb6-2857-4882-9f80-ebae954d404b}</MetaDataID>
        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        /// <MetaDataID>{6f1fc23b-57f6-41f8-b86c-4b1d56955371}</MetaDataID>
        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        /// <MetaDataID>{073741fe-72ce-4549-b85e-23383f4687b6}</MetaDataID>
        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return new TypeConverter();
        }

        /// <MetaDataID>{14da1a5e-80a0-41b2-abaa-24bb37a6a415}</MetaDataID>
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        /// <MetaDataID>{d403189d-2b8f-4805-9fc4-bfcb2c99d8e6}</MetaDataID>
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        /// <MetaDataID>{475f8fcd-c004-420f-92d3-91f5a4313400}</MetaDataID>
        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        /// <MetaDataID>{dd4abe6c-6d06-4c73-9b68-b52c541db7af}</MetaDataID>
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        /// <MetaDataID>{558c1ff0-0438-43d2-8c7d-42d235dd5394}</MetaDataID>
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        /// <MetaDataID>{b99faa70-b111-49c9-ba99-0097648abf02}</MetaDataID>
        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            if (pd is JPropertyDescriptor)
            {
                return this;
            }

            return null;
        }
        #endregion

#endif

#if HAVE_DYNAMIC
        /// <summary>
        /// Returns the <see cref="DynamicMetaObject" /> responsible for binding operations performed on this object.
        /// </summary>
        /// <param name="parameter">The expression tree representation of the runtime value.</param>
        /// <returns>
        /// The <see cref="DynamicMetaObject" /> to bind this object.
        /// </returns>
        /// <MetaDataID>{2f2ce992-632d-4933-9360-4f7e436d965e}</MetaDataID>
        protected override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicProxyMetaObject<JObject>(parameter, this, new JObjectDynamicProxy());
        }

        private class JObjectDynamicProxy : DynamicProxy<JObject>
        {
            public override bool TryGetMember(JObject instance, GetMemberBinder binder, out object result)
            {
                // result can be null
                result = instance[binder.Name];
                return true;
            }

            public override bool TrySetMember(JObject instance, SetMemberBinder binder, object value)
            {
                // this can throw an error if value isn't a valid for a JValue
                if (!(value is JToken v))
                {
                    v = new JValue(value);
                }

                instance[binder.Name] = v;
                return true;
            }

            public override IEnumerable<string> GetDynamicMemberNames(JObject instance)
            {
                return instance.Properties().Select(p => p.Name);
            }
        }
#endif
    }
}
