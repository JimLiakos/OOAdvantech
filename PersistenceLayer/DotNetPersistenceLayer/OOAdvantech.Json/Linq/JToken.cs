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
using OOAdvantech.Json.Linq.JsonPath;
#if HAVE_DYNAMIC
using System.Dynamic;
using System.Linq.Expressions;
#endif
using System.IO;
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
using OOAdvantech.Json.Utilities;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
#if !HAVE_LINQ
using OOAdvantech.Json.Utilities.LinqBridge;
#else
using System.Linq;

#endif

namespace OOAdvantech.Json.Linq
{
    /// <summary>
    /// Represents an abstract JSON token.
    /// </summary>
    /// <MetaDataID>OOAdvantech.Json.Linq.JToken</MetaDataID>
    public abstract partial class JToken : IJEnumerable<JToken>, IJsonLineInfo
#if HAVE_ICLONEABLE
        , ICloneable
#endif
#if HAVE_DYNAMIC
        , IDynamicMetaObjectProvider
#endif
    {
        /// <MetaDataID>{c7645753-7dc3-4d46-a00e-32aca53083ad}</MetaDataID>
        private static JTokenEqualityComparer _equalityComparer;

        /// <MetaDataID>{6ecdd1b6-6048-41bb-b928-015e9d3e1ee1}</MetaDataID>
        private JContainer _parent;
        /// <MetaDataID>{1ecbbe85-fdd3-4cde-930e-fe070fa4da86}</MetaDataID>
        private JToken _previous;
        /// <MetaDataID>{d0cfefc1-7d5e-4c5f-921e-323f13098407}</MetaDataID>
        private JToken _next;
        /// <MetaDataID>{200637d3-d382-47d3-92a2-0a440a8e65a4}</MetaDataID>
        private object _annotations;

        /// <MetaDataID>{ffa1f05c-985b-43bc-ad28-a4def3b026ad}</MetaDataID>
        private static readonly JTokenType[] BooleanTypes = new[] { JTokenType.Integer, JTokenType.Float, JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Boolean };
        /// <MetaDataID>{b3349e68-4aba-4c19-b78a-b8ad08a19c46}</MetaDataID>
        private static readonly JTokenType[] NumberTypes = new[] { JTokenType.Integer, JTokenType.Float, JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Boolean };
#if HAVE_BIG_INTEGER
        /// <MetaDataID>{6a58c64a-6428-4b14-b086-c12a7cfb0074}</MetaDataID>
        private static readonly JTokenType[] BigIntegerTypes = new[] { JTokenType.Integer, JTokenType.Float, JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Boolean, JTokenType.Bytes };
#endif
        /// <MetaDataID>{1468263a-3a16-436c-af0f-951d0e3cc2df}</MetaDataID>
        private static readonly JTokenType[] StringTypes = new[] { JTokenType.Date, JTokenType.Integer, JTokenType.Float, JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Boolean, JTokenType.Bytes, JTokenType.Guid, JTokenType.TimeSpan, JTokenType.Uri };
        /// <MetaDataID>{2f2d976d-289f-41a7-bf0a-78c876bdca74}</MetaDataID>
        private static readonly JTokenType[] GuidTypes = new[] { JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Guid, JTokenType.Bytes };
        /// <MetaDataID>{b8d7049b-5f65-4f65-921e-053855476672}</MetaDataID>
        private static readonly JTokenType[] TimeSpanTypes = new[] { JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.TimeSpan };
        /// <MetaDataID>{cce13baf-90ed-4250-9dd1-9d1a5311e097}</MetaDataID>
        private static readonly JTokenType[] UriTypes = new[] { JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Uri };
        /// <MetaDataID>{7150b6cf-8325-4687-92fd-234e4e93bb87}</MetaDataID>
        private static readonly JTokenType[] CharTypes = new[] { JTokenType.Integer, JTokenType.Float, JTokenType.String, JTokenType.Comment, JTokenType.Raw };
        /// <MetaDataID>{3d36e331-c3ff-4c41-a5ac-d837a419a46d}</MetaDataID>
        private static readonly JTokenType[] DateTimeTypes = new[] { JTokenType.Date, JTokenType.String, JTokenType.Comment, JTokenType.Raw };
        /// <MetaDataID>{2c8dbc19-e82a-40b8-b69d-68f954e6cad9}</MetaDataID>
        private static readonly JTokenType[] BytesTypes = new[] { JTokenType.Bytes, JTokenType.String, JTokenType.Comment, JTokenType.Raw, JTokenType.Integer };

        /// <summary>
        /// Gets a comparer that can compare two tokens for value equality.
        /// </summary>
        /// <value>A <see cref="JTokenEqualityComparer" /> that can compare two nodes for value equality.</value>
        /// <MetaDataID>{5845342f-b1d0-4cf6-9902-114ec838ef04}</MetaDataID>
        public static JTokenEqualityComparer EqualityComparer
        {
            get
            {
                if (_equalityComparer == null)
                {
                    _equalityComparer = new JTokenEqualityComparer();
                }

                return _equalityComparer;
            }
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        /// <MetaDataID>{c9cb9cef-577b-4894-8129-246de68f4d20}</MetaDataID>
        public JContainer Parent
        {
            [DebuggerStepThrough]
            get { return _parent; }
            internal set { _parent = value; }
        }

        /// <summary>
        /// Gets the root <see cref="JToken" /> of this <see cref="JToken" />.
        /// </summary>
        /// <value>The root <see cref="JToken" /> of this <see cref="JToken" />.</value>
        /// <MetaDataID>{478c709b-1445-426a-9359-256f0da913bb}</MetaDataID>
        public JToken Root
        {
            get
            {
                JContainer parent = Parent;
                if (parent == null)
                {
                    return this;
                }

                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                }

                return parent;
            }
        }

        /// <MetaDataID>{052cc184-83c4-4b27-9f0b-deb8481b0934}</MetaDataID>
        internal abstract JToken CloneToken();
        /// <MetaDataID>{8c5733dd-e843-43e7-934f-eea51cfb2659}</MetaDataID>
        internal abstract bool DeepEquals(JToken node);

        /// <summary>
        /// Gets the node type for this <see cref="JToken" />.
        /// </summary>
        /// <value>The type.</value>
        /// <MetaDataID>{22bae447-2bd4-409e-8253-da18f660fabc}</MetaDataID>
        public abstract JTokenType Type { get; }

        /// <summary>
        /// Gets a value indicating whether this token has child tokens.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token has child values; otherwise, <c>false</c>.
        /// </value>
        /// <MetaDataID>{d8a75a35-b24a-4a32-bf0c-a87786b40866}</MetaDataID>
        public abstract bool HasValues { get; }

        /// <summary>
        /// Compares the values of two tokens, including the values of all descendant tokens.
        /// </summary>
        /// <param name="t1">The first <see cref="JToken" /> to compare.</param>
        /// <param name="t2">The second <see cref="JToken" /> to compare.</param>
        /// <returns>
        ///   <c>true</c> if the tokens are equal; otherwise <c>false</c>.</returns>
        /// <MetaDataID>{997da0d1-edd0-4426-937f-2cd8cb589176}</MetaDataID>
        public static bool DeepEquals(JToken t1, JToken t2)
        {
            return (t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2)));
        }

        /// <summary>
        /// Gets the next sibling token of this node.
        /// </summary>
        /// <value>The <see cref="JToken" /> that contains the next sibling token.</value>
        /// <MetaDataID>{2c95ac52-9a13-47b2-94ff-4e87f33831b5}</MetaDataID>
        public JToken Next
        {
            get => _next;
            internal set => _next = value;
        }

        /// <summary>
        /// Gets the previous sibling token of this node.
        /// </summary>
        /// <value>The <see cref="JToken" /> that contains the previous sibling token.</value>
        /// <MetaDataID>{3f6bd28e-76c2-4d29-bf9c-63aa77dc70df}</MetaDataID>
        public JToken Previous
        {
            get => _previous;
            internal set => _previous = value;
        }

        /// <summary>
        /// Gets the path of the JSON token. 
        /// </summary>
        /// <MetaDataID>{fdb5b0a1-b58d-4637-8ab2-20c34c6bd4e2}</MetaDataID>
        public string Path
        {
            get
            {
                if (Parent == null)
                {
                    return string.Empty;
                }

                List<JsonPosition> positions = new List<JsonPosition>();
                JToken previous = null;
                for (JToken current = this; current != null; current = current.Parent)
                {
                    switch (current.Type)
                    {
                        case JTokenType.Property:
                            JProperty property = (JProperty)current;
                            positions.Add(new JsonPosition(JsonContainerType.Object) { PropertyName = property.Name });
                            break;
                        case JTokenType.Array:
                        case JTokenType.Constructor:
                            if (previous != null)
                            {
                                int index = ((IList<JToken>)current).IndexOf(previous);

                                positions.Add(new JsonPosition(JsonContainerType.Array) { Position = index });
                            }
                            break;
                    }

                    previous = current;
                }

#if HAVE_FAST_REVERSE
                positions.FastReverse();
#else
                positions.Reverse();
#endif

                return JsonPosition.BuildPath(positions, null);
            }
        }

        /// <MetaDataID>{2110fec3-051e-45f8-9dd3-e64c0bff0de3}</MetaDataID>
        internal JToken()
        {
        }

        /// <summary>
        /// Adds the specified content immediately after this token.
        /// </summary>
        /// <param name="content">A content object that contains simple content or a collection of content objects to be added after this token.</param>
        /// <MetaDataID>{4aae5d8f-4e49-4d54-bf8d-f0022b1fdf5f}</MetaDataID>
        public void AddAfterSelf(object content)
        {
            if (_parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }

            int index = _parent.IndexOfItem(this);
            _parent.AddInternal(index + 1, content, false);
        }

        /// <summary>
        /// Adds the specified content immediately before this token.
        /// </summary>
        /// <param name="content">A content object that contains simple content or a collection of content objects to be added before this token.</param>
        /// <MetaDataID>{612ae113-f4d7-4224-be29-0d640a5659e6}</MetaDataID>
        public void AddBeforeSelf(object content)
        {
            if (_parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }

            int index = _parent.IndexOfItem(this);
            _parent.AddInternal(index, content, false);
        }

        /// <summary>
        /// Returns a collection of the ancestor tokens of this token.
        /// </summary>
        /// <returns>A collection of the ancestor tokens of this token.</returns>
        /// <MetaDataID>{3ddf6a6b-eef2-4573-8e7b-baf0e3d532b1}</MetaDataID>
        public IEnumerable<JToken> Ancestors()
        {
            return GetAncestors(false);
        }

        /// <summary>
        /// Returns a collection of tokens that contain this token, and the ancestors of this token.
        /// </summary>
        /// <returns>A collection of tokens that contain this token, and the ancestors of this token.</returns>
        /// <MetaDataID>{1fc862d2-cec7-4288-b487-bdf00f2dd557}</MetaDataID>
        public IEnumerable<JToken> AncestorsAndSelf()
        {
            return GetAncestors(true);
        }

        /// <MetaDataID>{eaa4fe1d-5fc3-4806-a6f2-aa1532c6441e}</MetaDataID>
        internal IEnumerable<JToken> GetAncestors(bool self)
        {
            for (JToken current = self ? this : Parent; current != null; current = current.Parent)
            {
                yield return current;
            }
        }

        /// <summary>
        /// Returns a collection of the sibling tokens after this token, in document order.
        /// </summary>
        /// <returns>A collection of the sibling tokens after this tokens, in document order.</returns>
        /// <MetaDataID>{b9793b61-9af1-421e-b3eb-a7bda2bfd736}</MetaDataID>
        public IEnumerable<JToken> AfterSelf()
        {
            if (Parent == null)
            {
                yield break;
            }

            for (JToken o = Next; o != null; o = o.Next)
            {
                yield return o;
            }
        }

        /// <summary>
        /// Returns a collection of the sibling tokens before this token, in document order.
        /// </summary>
        /// <returns>A collection of the sibling tokens before this token, in document order.</returns>
        /// <MetaDataID>{29144765-5623-4de9-b183-a1e3369394da}</MetaDataID>
        public IEnumerable<JToken> BeforeSelf()
        {
            for (JToken o = Parent.First; o != this; o = o.Next)
            {
                yield return o;
            }
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> with the specified key.
        /// </summary>
        /// <value>The <see cref="JToken" /> with the specified key.</value>
        /// <MetaDataID>{b275900a-9415-4d4a-8831-f6390f2331c8}</MetaDataID>
        public virtual JToken this[object key]
        {
            get => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
            set => throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
        }

        /// <summary>
        /// Gets the <see cref="JToken" /> with the specified key converted to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the token to.</typeparam>
        /// <param name="key">The token key.</param>
        /// <returns>The converted token value.</returns>
        /// <MetaDataID>{058492de-7c54-488e-a4d2-63fcbefb9dfb}</MetaDataID>
        public virtual T Value<T>(object key)
        {
            JToken token = this[key];

            // null check to fix MonoTouch issue - https://github.com/dolbz/Newtonsoft.Json/commit/a24e3062846b30ee505f3271ac08862bb471b822
            return token == null ? default : Extensions.Convert<JToken, T>(token);
        }

        /// <summary>
        /// Get the first child token of this token.
        /// </summary>
        /// <value>A <see cref="JToken" /> containing the first child token of the <see cref="JToken" />.</value>
        /// <MetaDataID>{332d19dc-9ca3-49f1-a81d-16369d801804}</MetaDataID>
        public virtual JToken First => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));

        /// <summary>
        /// Get the last child token of this token.
        /// </summary>
        /// <value>A <see cref="JToken" /> containing the last child token of the <see cref="JToken" />.</value>
        /// <MetaDataID>{53a6c51c-cb63-4913-9bd0-f84e52f84746}</MetaDataID>
        public virtual JToken Last => throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));

        /// <summary>
        /// Returns a collection of the child tokens of this token, in document order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="JToken" /> containing the child tokens of this <see cref="JToken" />, in document order.</returns>
        /// <MetaDataID>{de7a84c1-7053-4903-9bf9-aaf71e53fd5e}</MetaDataID>
        public virtual JEnumerable<JToken> Children()
        {
            return JEnumerable<JToken>.Empty;
        }

        /// <summary>
        /// Returns a collection of the child tokens of this token, in document order, filtered by the specified type.
        /// </summary>
        /// <typeparam name="T">The type to filter the child tokens on.</typeparam>
        /// <returns>A <see cref="JEnumerable{T}" /> containing the child tokens of this <see cref="JToken" />, in document order.</returns>
        /// <MetaDataID>{5cd98e64-6ea6-4270-a4f4-83442a27ddb4}</MetaDataID>
        public JEnumerable<T> Children<T>() where T : JToken
        {
            return new JEnumerable<T>(Children().OfType<T>());
        }

        /// <summary>
        /// Returns a collection of the child values of this token, in document order.
        /// </summary>
        /// <typeparam name="T">The type to convert the values to.</typeparam>
        /// <returns>A <see cref="IEnumerable{T}" /> containing the child values of this <see cref="JToken" />, in document order.</returns>
        /// <MetaDataID>{b2920ea7-88ec-4619-9410-3e650d4b0e4e}</MetaDataID>
        public virtual IEnumerable<T> Values<T>()
        {
            throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
        }

        /// <summary>
        /// Removes this token from its parent.
        /// </summary>
        /// <MetaDataID>{23044f20-ba89-43b5-871d-f3c0b6907c51}</MetaDataID>
        public void Remove()
        {
            if (_parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }

            _parent.RemoveItem(this);
        }

        /// <summary>
        /// Replaces this token with the specified token.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{3fdd220e-1f1c-4f8d-94e7-483e64f0e7b1}</MetaDataID>
        public void Replace(JToken value)
        {
            if (_parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }

            _parent.ReplaceItem(this, value);
        }

        /// <summary>
        /// Writes this token to a <see cref="JsonWriter" />.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter" /> into which this method will write.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter" /> which will be used when writing the token.</param>
        /// <MetaDataID>{38318563-4fee-4f12-ab47-067e230af487}</MetaDataID>
        public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

        /// <summary>
        /// Returns the indented JSON for this token.
        /// </summary>
        /// <returns>
        /// The indented JSON for this token.
        /// </returns>
        /// <MetaDataID>{237e8d2f-e564-44a6-bac5-1d245e830328}</MetaDataID>
        public override string ToString()
        {
            return ToString(Formatting.Indented);
        }

        /// <summary>
        /// Returns the JSON for this token using the given formatting and converters.
        /// </summary>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter" />s which will be used when writing the token.</param>
        /// <returns>The JSON for this token using the given formatting and converters.</returns>
        /// <MetaDataID>{61daf744-4cf5-4501-be7a-ff52946b4fe8}</MetaDataID>
        public string ToString(Formatting formatting, params JsonConverter[] converters)
        {
            using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                JsonTextWriter jw = new JsonTextWriter(sw);
                jw.Formatting = formatting;

                WriteTo(jw, converters);

                return sw.ToString();
            }
        }

        /// <MetaDataID>{0cbc62c6-149a-4f5a-946c-193104eaa494}</MetaDataID>
        private static JValue EnsureValue(JToken value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value is JProperty property)
            {
                value = property.Value;
            }

            JValue v = value as JValue;

            return v;
        }

        /// <MetaDataID>{29e51cdc-b950-400a-8928-e392198b861a}</MetaDataID>
        private static string GetType(JToken token)
        {
            ValidationUtils.ArgumentNotNull(token, nameof(token));

            if (token is JProperty p)
            {
                token = p.Value;
            }

            return token.Type.ToString();
        }

        /// <MetaDataID>{b4c03c24-71c5-4bc4-8881-237ed5259cd5}</MetaDataID>
        private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
        {
            return (Array.IndexOf(validTypes, o.Type) != -1) || (nullable && (o.Type == JTokenType.Null || o.Type == JTokenType.Undefined));
        }

        #region Cast from operators
        /// <summary>
        /// Performs an explicit conversion from <see cref="Newtonsoft.Json.Linq.JToken" /> to <see cref="System.Boolean" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{3099b74c-8719-40c9-b4ed-09c0be62d151}</MetaDataID>
        public static explicit operator bool(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, BooleanTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return Convert.ToBoolean((int)integer);
            }
#endif

            return Convert.ToBoolean(v.Value, CultureInfo.InvariantCulture);
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Performs an explicit conversion from <see cref="Newtonsoft.Json.Linq.JToken" /> to <see cref="System.DateTimeOffset" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{87a33ff8-a21d-43ff-af3c-b15296ef7b4d}</MetaDataID>
        public static explicit operator DateTimeOffset(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, DateTimeTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value is DateTimeOffset offset)
            {
                return offset;
            }

            if (v.Value is string s)
            {
                return DateTimeOffset.Parse(s, CultureInfo.InvariantCulture);
            }

            return new DateTimeOffset(Convert.ToDateTime(v.Value, CultureInfo.InvariantCulture));
        }
#endif

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Boolean" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{28e93895-e703-44fc-a77e-39e190f90e2f}</MetaDataID>
        public static explicit operator bool?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, BooleanTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return Convert.ToBoolean((int)integer);
            }
#endif

            return (v.Value != null) ? (bool?)Convert.ToBoolean(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Int64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{11b0a635-3a7e-456b-81b4-307cbce9a955}</MetaDataID>
        public static explicit operator long(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (long)integer;
            }
#endif

            return Convert.ToInt64(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="DateTime" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{a2e26d34-353d-46d2-b1b0-6a1ad39a5281}</MetaDataID>
        public static explicit operator DateTime?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, DateTimeTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_DATE_TIME_OFFSET
            if (v.Value is DateTimeOffset offset)
            {
                return offset.DateTime;
            }
#endif

            return (v.Value != null) ? (DateTime?)Convert.ToDateTime(v.Value, CultureInfo.InvariantCulture) : null;
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{c8a733d6-8a25-4e33-93e9-7b369ea35e08}</MetaDataID>
        public static explicit operator DateTimeOffset?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, DateTimeTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value == null)
            {
                return null;
            }
            if (v.Value is DateTimeOffset offset)
            {
                return offset;
            }

            if (v.Value is string s)
            {
                return DateTimeOffset.Parse(s, CultureInfo.InvariantCulture);
            }

            return new DateTimeOffset(Convert.ToDateTime(v.Value, CultureInfo.InvariantCulture));
        }
#endif

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Decimal" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{d720aa0d-751b-4107-bdcb-248791a8ae38}</MetaDataID>
        public static explicit operator decimal?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (decimal?)integer;
            }
#endif

            return (v.Value != null) ? (decimal?)Convert.ToDecimal(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Double" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{5948af27-ecdc-4aaa-a934-5b9cc744d89f}</MetaDataID>
        public static explicit operator double?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (double?)integer;
            }
#endif

            return (v.Value != null) ? (double?)Convert.ToDouble(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Char" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{c65e21e0-d288-4efa-810a-3aa3c181a100}</MetaDataID>
        public static explicit operator char?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, CharTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (char?)integer;
            }
#endif

            return (v.Value != null) ? (char?)Convert.ToChar(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Int32" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{cbc8eaf9-e0ba-46cd-ad06-6efe1f9839a5}</MetaDataID>
        public static explicit operator int(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (int)integer;
            }
#endif

            return Convert.ToInt32(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Int16" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{edb38170-1348-48e0-8226-d0ac19f7c1af}</MetaDataID>
        public static explicit operator short(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (short)integer;
            }
#endif

            return Convert.ToInt16(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="UInt16" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{e2ec969e-79fe-4f6b-984c-cbd78f0183a1}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator ushort(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (ushort)integer;
            }
#endif

            return Convert.ToUInt16(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Char" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{a17508dd-a8e9-451e-8d27-414e454fb103}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator char(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, CharTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (char)integer;
            }
#endif

            return Convert.ToChar(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Byte" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{7b83a74d-3d4d-4228-ad71-7a3c45e84401}</MetaDataID>
        public static explicit operator byte(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (byte)integer;
            }
#endif

            return Convert.ToByte(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Newtonsoft.Json.Linq.JToken" /> to <see cref="System.SByte" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{a8dcb60a-ef31-4f7b-bbcc-b55aa3600e8e}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator sbyte(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (sbyte)integer;
            }
#endif

            return Convert.ToSByte(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Int32" /> .
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{84fdbfb2-291c-4306-af6a-4dd4a58bbff4}</MetaDataID>
        public static explicit operator int?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (int?)integer;
            }
#endif

            return (v.Value != null) ? (int?)Convert.ToInt32(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Int16" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{ff4b8870-d471-43d3-a30f-992c8584c073}</MetaDataID>
        public static explicit operator short?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (short?)integer;
            }
#endif

            return (v.Value != null) ? (short?)Convert.ToInt16(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="UInt16" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{0657261c-e16e-4795-8340-5a2f066c1c6f}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator ushort?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (ushort?)integer;
            }
#endif

            return (v.Value != null) ? (ushort?)Convert.ToUInt16(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Byte" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{f7d40223-580c-4507-91e6-f6db2a6fe635}</MetaDataID>
        public static explicit operator byte?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (byte?)integer;
            }
#endif

            return (v.Value != null) ? (byte?)Convert.ToByte(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="SByte" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{b445ef92-50cd-46a0-b83e-cb8740f64c80}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator sbyte?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (sbyte?)integer;
            }
#endif

            return (v.Value != null) ? (sbyte?)Convert.ToSByte(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="DateTime" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{706881f4-514c-49db-a850-fdb3e18035ed}</MetaDataID>
        public static explicit operator DateTime(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, DateTimeTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_DATE_TIME_OFFSET
            if (v.Value is DateTimeOffset offset)
            {
                return offset.DateTime;
            }
#endif

            return Convert.ToDateTime(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Int64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{520cf25b-45fa-4b3b-ade7-805f6d234139}</MetaDataID>
        public static explicit operator long?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (long?)integer;
            }
#endif

            return (v.Value != null) ? (long?)Convert.ToInt64(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Single" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{5dffbbf8-69f0-4939-8424-9b72859fa9ec}</MetaDataID>
        public static explicit operator float?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (float?)integer;
            }
#endif

            return (v.Value != null) ? (float?)Convert.ToSingle(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Decimal" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{949803a2-db25-477f-a3d0-bc3eda871bba}</MetaDataID>
        public static explicit operator decimal(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (decimal)integer;
            }
#endif

            return Convert.ToDecimal(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="UInt32" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{f3633b07-b8cf-49d0-ab07-f1791917ed81}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator uint?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (uint?)integer;
            }
#endif

            return (v.Value != null) ? (uint?)Convert.ToUInt32(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="UInt64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{26bf6ed3-43c3-4df3-90ef-837804e2f2d5}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator ulong?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (ulong?)integer;
            }
#endif

            return (v.Value != null) ? (ulong?)Convert.ToUInt64(v.Value, CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Double" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{c1e1a3d4-544b-407b-963f-db54ae81adf8}</MetaDataID>
        public static explicit operator double(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (double)integer;
            }
#endif

            return Convert.ToDouble(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Single" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{ec19f2b3-eeb3-42b4-aa65-3172a72af594}</MetaDataID>
        public static explicit operator float(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (float)integer;
            }
#endif

            return Convert.ToSingle(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="String" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{6b62fed1-4920-4a14-8305-25e987b4ec2f}</MetaDataID>
        public static explicit operator string(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, StringTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value == null)
            {
                return null;
            }

            if (v.Value is byte[] bytes)
            {
                return Convert.ToBase64String(bytes);
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return integer.ToString(CultureInfo.InvariantCulture);
            }
#endif

            return Convert.ToString(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="UInt32" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{d9e1c059-8212-42a8-ad34-dde60894a41e}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator uint(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (uint)integer;
            }
#endif

            return Convert.ToUInt32(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Newtonsoft.Json.Linq.JToken" /> to <see cref="System.UInt64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{2063b4f2-dfe5-4728-a36c-1a0986c4b77b}</MetaDataID>
        [CLSCompliant(false)]
        public static explicit operator ulong(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, NumberTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return (ulong)integer;
            }
#endif

            return Convert.ToUInt64(v.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Byte" />[].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{32f0684d-6b2c-4826-be6a-2f0c5761e481}</MetaDataID>
        public static explicit operator byte[](JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, BytesTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value is string)
            {
                return Convert.FromBase64String(Convert.ToString(v.Value, CultureInfo.InvariantCulture));
            }
#if HAVE_BIG_INTEGER
            if (v.Value is BigInteger integer)
            {
                return integer.ToByteArray();
            }
#endif

            if (v.Value is byte[] bytes)
            {
                return bytes;
            }

            throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Guid" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{a3f58509-d539-48e2-a08e-bb4ef20ddb41}</MetaDataID>
        public static explicit operator Guid(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, GuidTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value is byte[] bytes)
            {
                return new Guid(bytes);
            }

            return (v.Value is Guid guid) ? guid : new Guid(Convert.ToString(v.Value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="Guid" /> .
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{2d0babb2-42cb-400b-b468-921f43b12d50}</MetaDataID>
        public static explicit operator Guid?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, GuidTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value == null)
            {
                return null;
            }

            if (v.Value is byte[] bytes)
            {
                return new Guid(bytes);
            }

            return (v.Value is Guid guid) ? guid : new Guid(Convert.ToString(v.Value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{3799ec7d-8b06-48d4-8f01-75f3c7d08066}</MetaDataID>
        public static explicit operator TimeSpan(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, TimeSpanTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            return (v.Value is TimeSpan span) ? span : ConvertUtils.ParseTimeSpan(Convert.ToString(v.Value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Nullable{T}" /> of <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{71d84836-d023-4d8b-b330-e170e2f4ec6a}</MetaDataID>
        public static explicit operator TimeSpan?(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, TimeSpanTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value == null)
            {
                return null;
            }

            return (v.Value is TimeSpan span) ? span : ConvertUtils.ParseTimeSpan(Convert.ToString(v.Value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="JToken" /> to <see cref="Uri" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <MetaDataID>{1a72c133-6c32-4cc0-89bb-402d596a7ded}</MetaDataID>
        public static explicit operator Uri(JToken value)
        {
            if (value == null)
            {
                return null;
            }

            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, UriTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to Uri.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value == null)
            {
                return null;
            }

            return (v.Value is Uri uri) ? uri : new Uri(Convert.ToString(v.Value, CultureInfo.InvariantCulture));
        }

#if HAVE_BIG_INTEGER
        /// <MetaDataID>{ae3529ac-6f5b-43cb-a730-f26fe48e977e}</MetaDataID>
        private static BigInteger ToBigInteger(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, BigIntegerTypes, false))
            {
                throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            return ConvertUtils.ToBigInteger(v.Value);
        }

        /// <MetaDataID>{dfe10338-fbb2-4a0f-b42c-50e9e8e18759}</MetaDataID>
        private static BigInteger? ToBigIntegerNullable(JToken value)
        {
            JValue v = EnsureValue(value);
            if (v == null || !ValidateToken(v, BigIntegerTypes, true))
            {
                throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
            }

            if (v.Value == null)
            {
                return null;
            }

            return ConvertUtils.ToBigInteger(v.Value);
        }
#endif
        #endregion

        #region Cast to operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="Boolean" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{b81321e8-0c49-4ed9-9608-c10de2b36499}</MetaDataID>
        public static implicit operator JToken(bool value)
        {
            return new JValue(value);
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Performs an implicit conversion from <see cref="DateTimeOffset" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{5f9a2e4b-63e2-4580-a245-2a1fcc7c9059}</MetaDataID>
        public static implicit operator JToken(DateTimeOffset value)
        {
            return new JValue(value);
        }
#endif

        /// <summary>
        /// Performs an implicit conversion from <see cref="Byte" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{83d05274-031c-4806-a9d2-46fb9876f485}</MetaDataID>
        public static implicit operator JToken(byte value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Byte" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{f066de69-1f20-4e13-974b-23016f51285e}</MetaDataID>
        public static implicit operator JToken(byte? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SByte" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{f3af8345-8b91-4938-b239-6da8a6961ef7}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(sbyte value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="SByte" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{860513ca-f8e7-45b3-bfc8-6f5ae9b062e9}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(sbyte? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Boolean" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{dc22bc55-1986-471f-8d82-4467d7f4661f}</MetaDataID>
        public static implicit operator JToken(bool? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Int64" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{36e573b1-fdbe-42b6-b16a-dfab3fa43cfe}</MetaDataID>
        public static implicit operator JToken(long value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="DateTime" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{8c4bdf93-3716-4f10-8b91-271e4a3917e6}</MetaDataID>
        public static implicit operator JToken(DateTime? value)
        {
            return new JValue(value);
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{e1b77108-a63b-4183-b9dd-bebaa021438d}</MetaDataID>
        public static implicit operator JToken(DateTimeOffset? value)
        {
            return new JValue(value);
        }
#endif

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Decimal" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{d33b43a1-183b-412c-ab86-5114af3bcbc6}</MetaDataID>
        public static implicit operator JToken(decimal? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Double" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{ad8f13b4-6bed-46de-9db4-bf3a647dce77}</MetaDataID>
        public static implicit operator JToken(double? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Int16" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{9899321c-5266-4fab-86b6-53464cefcf2e}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(short value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="UInt16" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{1d50ef27-40c8-41ba-a2d3-fdc49df5bf93}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(ushort value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Int32" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{fc8f50a8-b2f0-4de6-9daa-c8f32382078e}</MetaDataID>
        public static implicit operator JToken(int value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Int32" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{b1d839a5-f0db-4290-b017-de13beca99d7}</MetaDataID>
        public static implicit operator JToken(int? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DateTime" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{da7221e0-bccc-4db7-830a-1c3f3e92c23d}</MetaDataID>
        public static implicit operator JToken(DateTime value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Int64" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{13bd75ca-bee6-4282-856f-f03fb52a482b}</MetaDataID>
        public static implicit operator JToken(long? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Single" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{446a3be8-b525-4cd6-a1f8-ea1d3c757ec9}</MetaDataID>
        public static implicit operator JToken(float? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Decimal" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{c5556956-7547-41b8-afbd-e45eddbf3841}</MetaDataID>
        public static implicit operator JToken(decimal value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Int16" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{3a576e4b-8ab1-4a33-9d45-4f4a69c7a267}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(short? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="UInt16" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{5898d249-6b11-4701-95be-1a759dc47f59}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(ushort? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="UInt32" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{aaaf7f4e-0988-45ff-b9e9-2615de14ec7e}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(uint? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="UInt64" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{f2e4dc36-ce54-4c76-b9bf-a803fcc53a34}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(ulong? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Double" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{fba49953-853e-4d7b-876b-726bbc9a7f91}</MetaDataID>
        public static implicit operator JToken(double value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Single" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{5c673ca5-0e57-40a0-a14f-c44a23cac31e}</MetaDataID>
        public static implicit operator JToken(float value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="String" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{007234da-32b8-42ba-8642-20abe3f4e06e}</MetaDataID>
        public static implicit operator JToken(string value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="UInt32" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{1353b4c0-b0a0-4f2d-bb0b-f6da870602d1}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(uint value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="UInt64" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{780bc5c0-6cda-471e-afc9-a595665df27f}</MetaDataID>
        [CLSCompliant(false)]
        public static implicit operator JToken(ulong value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Byte" />[] to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{2786f878-0a0d-43be-bfb5-9730d3642cf5}</MetaDataID>
        public static implicit operator JToken(byte[] value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Uri" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{7e863408-6f77-4979-89b9-b614ffc51a7c}</MetaDataID>
        public static implicit operator JToken(Uri value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="TimeSpan" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{0a47cad7-c27b-4802-bf7c-bb2f8cf458b7}</MetaDataID>
        public static implicit operator JToken(TimeSpan value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="TimeSpan" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{b50a268a-0f00-4644-8ad6-912fe0e1b4eb}</MetaDataID>
        public static implicit operator JToken(TimeSpan? value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Guid" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{0ab03682-d18f-44a3-890d-cf397fdca791}</MetaDataID>
        public static implicit operator JToken(Guid value)
        {
            return new JValue(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{T}" /> of <see cref="Guid" /> to <see cref="JToken" />.
        /// </summary>
        /// <param name="value">The value to create a <see cref="JValue" /> from.</param>
        /// <returns>The <see cref="JValue" /> initialized with the specified value.</returns>
        /// <MetaDataID>{79c8cbb6-4ea7-4882-8aa5-377103a35cbd}</MetaDataID>
        public static implicit operator JToken(Guid? value)
        {
            return new JValue(value);
        }
        #endregion

        /// <MetaDataID>{cca1ed6e-34a1-4bc4-9d3f-7daa3cfc2cd1}</MetaDataID>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<JToken>)this).GetEnumerator();
        }

        /// <MetaDataID>{896a18f4-93a8-4bbd-b80a-c42d0bf051c2}</MetaDataID>
        IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
        {
            return Children().GetEnumerator();
        }

        /// <MetaDataID>{f2f63ca6-0697-456a-86b8-6ce72fe8fea7}</MetaDataID>
        internal abstract int GetDeepHashCode();

        /// <MetaDataID>{41e79aa3-f252-4cef-82b8-35b9986a564c}</MetaDataID>
        IJEnumerable<JToken> IJEnumerable<JToken>.this[object key] => this[key];

        /// <summary>
        /// Creates a <see cref="JsonReader" /> for this token.
        /// </summary>
        /// <returns>A <see cref="JsonReader" /> that can be used to read this token and its descendants.</returns>
        /// <MetaDataID>{44253ebf-af56-4bae-a42a-ba1d5f03101b}</MetaDataID>
        public JsonReader CreateReader()
        {
            return new JTokenReader(this);
        }

        /// <MetaDataID>{be4ec611-40f3-4027-8c95-6308235da930}</MetaDataID>
        internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
        {
            ValidationUtils.ArgumentNotNull(o, nameof(o));
            ValidationUtils.ArgumentNotNull(jsonSerializer, nameof(jsonSerializer));

            JToken token;
            using (JTokenWriter jsonWriter = new JTokenWriter())
            {
                jsonSerializer.Serialize(jsonWriter, o);
                token = jsonWriter.Token;
            }

            return token;
        }

        /// <summary>
        /// Creates a <see cref="JToken" /> from an object.
        /// </summary>
        /// <param name="o">The object that will be used to create <see cref="JToken" />.</param>
        /// <returns>A <see cref="JToken" /> with the value of the specified object.</returns>
        /// <MetaDataID>{75c61a43-1017-4f3b-913c-eae264bec826}</MetaDataID>
        public static JToken FromObject(object o)
        {
            return FromObjectInternal(o, JsonSerializer.CreateDefault());
        }

        /// <summary>
        /// Creates a <see cref="JToken" /> from an object using the specified <see cref="JsonSerializer" />.
        /// </summary>
        /// <param name="o">The object that will be used to create <see cref="JToken" />.</param>
        /// <param name="jsonSerializer">The <see cref="JsonSerializer" /> that will be used when reading the object.</param>
        /// <returns>A <see cref="JToken" /> with the value of the specified object.</returns>
        /// <MetaDataID>{1e8a5d16-91f1-4535-9692-dc0d2927bc4e}</MetaDataID>
        public static JToken FromObject(object o, JsonSerializer jsonSerializer)
        {
            return FromObjectInternal(o, jsonSerializer);
        }

        /// <summary>
        /// Creates an instance of the specified .NET type from the <see cref="JToken" />.
        /// </summary>
        /// <typeparam name="T">The object type that the token will be deserialized to.</typeparam>
        /// <returns>The new object created from the JSON value.</returns>
        /// <MetaDataID>{dd10f24d-b4ed-49f9-91b0-e231ff78b0e0}</MetaDataID>
        public T ToObject<T>()
        {
            return (T)ToObject(typeof(T));
        }

        /// <summary>
        /// Creates an instance of the specified .NET type from the <see cref="JToken" />.
        /// </summary>
        /// <param name="objectType">The object type that the token will be deserialized to.</param>
        /// <returns>The new object created from the JSON value.</returns>
        /// <MetaDataID>{02dac643-02b6-418c-8972-51a0eff7bf8e}</MetaDataID>
        public object ToObject(Type objectType)
        {
            if (JsonConvert.DefaultSettings == null)
            {
                PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out bool isEnum);

                if (isEnum)
                {
                    if (Type == JTokenType.String)
                    {
                        try
                        {
                            // use serializer so JsonConverter(typeof(StringEnumConverter)) + EnumMemberAttributes are respected
                            return ToObject(objectType, JsonSerializer.CreateDefault());
                        }
                        catch (Exception ex)
                        {
                            Type enumType = objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType);
                            throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, (string)this, enumType.Name), ex);
                        }
                    }

                    if (Type == JTokenType.Integer)
                    {
                        Type enumType = objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType);
                        return Enum.ToObject(enumType, ((JValue)this).Value);
                    }
                }

                switch (typeCode)
                {
                    case PrimitiveTypeCode.BooleanNullable:
                        return (bool?)this;
                    case PrimitiveTypeCode.Boolean:
                        return (bool)this;
                    case PrimitiveTypeCode.CharNullable:
                        return (char?)this;
                    case PrimitiveTypeCode.Char:
                        return (char)this;
                    case PrimitiveTypeCode.SByte:
                        return (sbyte)this;
                    case PrimitiveTypeCode.SByteNullable:
                        return (sbyte?)this;
                    case PrimitiveTypeCode.ByteNullable:
                        return (byte?)this;
                    case PrimitiveTypeCode.Byte:
                        return (byte)this;
                    case PrimitiveTypeCode.Int16Nullable:
                        return (short?)this;
                    case PrimitiveTypeCode.Int16:
                        return (short)this;
                    case PrimitiveTypeCode.UInt16Nullable:
                        return (ushort?)this;
                    case PrimitiveTypeCode.UInt16:
                        return (ushort)this;
                    case PrimitiveTypeCode.Int32Nullable:
                        return (int?)this;
                    case PrimitiveTypeCode.Int32:
                        return (int)this;
                    case PrimitiveTypeCode.UInt32Nullable:
                        return (uint?)this;
                    case PrimitiveTypeCode.UInt32:
                        return (uint)this;
                    case PrimitiveTypeCode.Int64Nullable:
                        return (long?)this;
                    case PrimitiveTypeCode.Int64:
                        return (long)this;
                    case PrimitiveTypeCode.UInt64Nullable:
                        return (ulong?)this;
                    case PrimitiveTypeCode.UInt64:
                        return (ulong)this;
                    case PrimitiveTypeCode.SingleNullable:
                        return (float?)this;
                    case PrimitiveTypeCode.Single:
                        return (float)this;
                    case PrimitiveTypeCode.DoubleNullable:
                        return (double?)this;
                    case PrimitiveTypeCode.Double:
                        return (double)this;
                    case PrimitiveTypeCode.DecimalNullable:
                        return (decimal?)this;
                    case PrimitiveTypeCode.Decimal:
                        return (decimal)this;
                    case PrimitiveTypeCode.DateTimeNullable:
                        return (DateTime?)this;
                    case PrimitiveTypeCode.DateTime:
                        return (DateTime)this;
#if HAVE_DATE_TIME_OFFSET
                    case PrimitiveTypeCode.DateTimeOffsetNullable:
                        return (DateTimeOffset?)this;
                    case PrimitiveTypeCode.DateTimeOffset:
                        return (DateTimeOffset)this;
#endif
                    case PrimitiveTypeCode.String:
                        return (string)this;
                    case PrimitiveTypeCode.GuidNullable:
                        return (Guid?)this;
                    case PrimitiveTypeCode.Guid:
                        return (Guid)this;
                    case PrimitiveTypeCode.Uri:
                        return (Uri)this;
                    case PrimitiveTypeCode.TimeSpanNullable:
                        return (TimeSpan?)this;
                    case PrimitiveTypeCode.TimeSpan:
                        return (TimeSpan)this;
#if HAVE_BIG_INTEGER
                    case PrimitiveTypeCode.BigIntegerNullable:
                        return ToBigIntegerNullable(this);
                    case PrimitiveTypeCode.BigInteger:
                        return ToBigInteger(this);
#endif
                }
            }

            return ToObject(objectType, JsonSerializer.CreateDefault());
        }

        /// <summary>
        /// Creates an instance of the specified .NET type from the <see cref="JToken" /> using the specified <see cref="JsonSerializer" />.
        /// </summary>
        /// <typeparam name="T">The object type that the token will be deserialized to.</typeparam>
        /// <param name="jsonSerializer">The <see cref="JsonSerializer" /> that will be used when creating the object.</param>
        /// <returns>The new object created from the JSON value.</returns>
        /// <MetaDataID>{05da48a0-3293-4d65-90c1-052fadec3fbe}</MetaDataID>
        public T ToObject<T>(JsonSerializer jsonSerializer)
        {
            return (T)ToObject(typeof(T), jsonSerializer);
        }

        /// <summary>
        /// Creates an instance of the specified .NET type from the <see cref="JToken" /> using the specified <see cref="JsonSerializer" />.
        /// </summary>
        /// <param name="objectType">The object type that the token will be deserialized to.</param>
        /// <param name="jsonSerializer">The <see cref="JsonSerializer" /> that will be used when creating the object.</param>
        /// <returns>The new object created from the JSON value.</returns>
        /// <MetaDataID>{7aaa8b6c-2d47-4da9-828f-b68436cb9683}</MetaDataID>
        public object ToObject(Type objectType, JsonSerializer jsonSerializer)
        {
            ValidationUtils.ArgumentNotNull(jsonSerializer, nameof(jsonSerializer));

            using (JTokenReader jsonReader = new JTokenReader(this))
            {
                return jsonSerializer.Deserialize(jsonReader, objectType);
            }
        }

        /// <summary>
        /// Creates a <see cref="JToken" /> from a <see cref="JsonReader" />.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader" /> positioned at the token to read into this <see cref="JToken" />.</param>
        /// <returns>
        /// A <see cref="JToken" /> that contains the token and its descendant tokens
        /// that were read from the reader. The runtime type of the token is determined
        /// by the token type of the first token encountered in the reader.
        /// </returns>
        /// <MetaDataID>{4bc98ad4-3fa3-4f0b-ab4b-92a05ba77111}</MetaDataID>
        public static JToken ReadFrom(JsonReader reader)
        {
            return ReadFrom(reader, null);
        }

        /// <summary>
        /// Creates a <see cref="JToken" /> from a <see cref="JsonReader" />.
        /// </summary>
        /// <param name="reader">An <see cref="JsonReader" /> positioned at the token to read into this <see cref="JToken" />.</param>
        /// <param name="settings">The <see cref="JsonLoadSettings" /> used to load the JSON.
        /// If this is <c>null</c>, default load settings will be used.</param>
        /// <returns>
        /// A <see cref="JToken" /> that contains the token and its descendant tokens
        /// that were read from the reader. The runtime type of the token is determined
        /// by the token type of the first token encountered in the reader.
        /// </returns>
        /// <MetaDataID>{a986ee1b-1952-453f-bad7-27f7d2bcb2a8}</MetaDataID>
        public static JToken ReadFrom(JsonReader reader, JsonLoadSettings settings)
        {
            ValidationUtils.ArgumentNotNull(reader, nameof(reader));

            bool hasContent;
            if (reader.TokenType == JsonToken.None)
            {
                hasContent = (settings != null && settings.CommentHandling == CommentHandling.Ignore)
                    ? reader.ReadAndMoveToContent()
                    : reader.Read();
            }
            else if (reader.TokenType == JsonToken.Comment && settings?.CommentHandling == CommentHandling.Ignore)
            {
                hasContent = reader.ReadAndMoveToContent();
            }
            else
            {
                hasContent = true;
            }

            if (!hasContent)
            {
                throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
            }

            IJsonLineInfo lineInfo = reader as IJsonLineInfo;

            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return JObject.Load(reader, settings);
                case JsonToken.StartArray:
                    return JArray.Load(reader, settings);
                case JsonToken.StartConstructor:
                    return JConstructor.Load(reader, settings);
                case JsonToken.PropertyName:
                    return JProperty.Load(reader, settings);
                case JsonToken.String:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Date:
                case JsonToken.Boolean:
                case JsonToken.Bytes:
                    JValue v = new JValue(reader.Value);
                    v.SetLineInfo(lineInfo, settings);
                    return v;
                case JsonToken.Comment:
                    v = JValue.CreateComment(reader.Value.ToString());
                    v.SetLineInfo(lineInfo, settings);
                    return v;
                case JsonToken.Null:
                    v = JValue.CreateNull();
                    v.SetLineInfo(lineInfo, settings);
                    return v;
                case JsonToken.Undefined:
                    v = JValue.CreateUndefined();
                    v.SetLineInfo(lineInfo, settings);
                    return v;
                default:
                    throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
            }
        }

        /// <summary>
        /// Load a <see cref="JToken" /> from a string that contains JSON.
        /// </summary>
        /// <param name="json">A <see cref="String" /> that contains JSON.</param>
        /// <returns>A <see cref="JToken" /> populated from the string that contains JSON.</returns>
        /// <MetaDataID>{a58eb0fb-e55d-4664-8171-e8d209fc4473}</MetaDataID>
        public static JToken Parse(string json)
        {
            return Parse(json, null);
        }

        /// <summary>
        /// Load a <see cref="JToken" /> from a string that contains JSON.
        /// </summary>
        /// <param name="json">A <see cref="String" /> that contains JSON.</param>
        /// <param name="settings">The <see cref="JsonLoadSettings" /> used to load the JSON.
        /// If this is <c>null</c>, default load settings will be used.</param>
        /// <returns>A <see cref="JToken" /> populated from the string that contains JSON.</returns>
        /// <MetaDataID>{3425d85c-4201-4428-adba-1add7f6f5ec1}</MetaDataID>
        public static JToken Parse(string json, JsonLoadSettings settings)
        {
            using (JsonReader reader = new JsonTextReader(new StringReader(json)))
            {
                JToken t = Load(reader, settings);

                while (reader.Read())
                {
                    // Any content encountered here other than a comment will throw in the reader.
                }


                return t;
            }
        }

        /// <summary>
        /// Creates a <see cref="JToken" /> from a <see cref="JsonReader" />.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader" /> positioned at the token to read into this <see cref="JToken" />.</param>
        /// <param name="settings">The <see cref="JsonLoadSettings" /> used to load the JSON.
        /// If this is <c>null</c>, default load settings will be used.</param>
        /// <returns>
        /// A <see cref="JToken" /> that contains the token and its descendant tokens
        /// that were read from the reader. The runtime type of the token is determined
        /// by the token type of the first token encountered in the reader.
        /// </returns>
        /// <MetaDataID>{742c02ad-230a-47ed-acd8-733d9898092f}</MetaDataID>
        public static JToken Load(JsonReader reader, JsonLoadSettings settings)
        {
            return ReadFrom(reader, settings);
        }

        /// <summary>
        /// Creates a <see cref="JToken" /> from a <see cref="JsonReader" />.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader" /> positioned at the token to read into this <see cref="JToken" />.</param>
        /// <returns>
        /// A <see cref="JToken" /> that contains the token and its descendant tokens
        /// that were read from the reader. The runtime type of the token is determined
        /// by the token type of the first token encountered in the reader.
        /// </returns>
        /// <MetaDataID>{a12021b9-4c41-4dfa-9f50-2054f81d5209}</MetaDataID>
        public static JToken Load(JsonReader reader)
        {
            return Load(reader, null);
        }

        /// <MetaDataID>{5dedcce6-12e6-4f49-9bee-667a34a98525}</MetaDataID>
        internal void SetLineInfo(IJsonLineInfo lineInfo, JsonLoadSettings settings)
        {
            if (settings != null && settings.LineInfoHandling != LineInfoHandling.Load)
            {
                return;
            }

            if (lineInfo == null || !lineInfo.HasLineInfo())
            {
                return;
            }

            SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
        }

        private class LineInfoAnnotation
        {
            internal readonly int LineNumber;
            internal readonly int LinePosition;

            public LineInfoAnnotation(int lineNumber, int linePosition)
            {
                LineNumber = lineNumber;
                LinePosition = linePosition;
            }
        }

        /// <MetaDataID>{951bb31f-d8e8-4ab8-adb2-10a3153d3d95}</MetaDataID>
        internal void SetLineInfo(int lineNumber, int linePosition)
        {
            AddAnnotation(new LineInfoAnnotation(lineNumber, linePosition));
        }

        /// <MetaDataID>{6164b450-0921-49b6-aad9-9df60a766faf}</MetaDataID>
        bool IJsonLineInfo.HasLineInfo()
        {
            return (Annotation<LineInfoAnnotation>() != null);
        }

        /// <MetaDataID>{a45d42aa-3706-4e83-a8fc-29d50b9fdb42}</MetaDataID>
        int IJsonLineInfo.LineNumber
        {
            get
            {
                LineInfoAnnotation annotation = Annotation<LineInfoAnnotation>();
                if (annotation != null)
                {
                    return annotation.LineNumber;
                }

                return 0;
            }
        }

        /// <MetaDataID>{a8719cba-9fe6-41e5-93e1-4a3782dc5a66}</MetaDataID>
        int IJsonLineInfo.LinePosition
        {
            get
            {
                LineInfoAnnotation annotation = Annotation<LineInfoAnnotation>();
                if (annotation != null)
                {
                    return annotation.LinePosition;
                }

                return 0;
            }
        }

        /// <summary>
        /// Selects a <see cref="JToken" /> using a JPath expression. Selects the token that matches the object path.
        /// </summary>
        /// <param name="path">
        /// A <see cref="String" /> that contains a JPath expression.
        /// </param>
        /// <returns>A <see cref="JToken" />, or <c>null</c>.</returns>
        /// <MetaDataID>{f92817fd-c031-4b9d-8832-9c7d1fc7ce80}</MetaDataID>
        public JToken SelectToken(string path)
        {
            return SelectToken(path, false);
        }

        /// <summary>
        /// Selects a <see cref="JToken" /> using a JPath expression. Selects the token that matches the object path.
        /// </summary>
        /// <param name="path">
        /// A <see cref="String" /> that contains a JPath expression.
        /// </param>
        /// <param name="errorWhenNoMatch">A flag to indicate whether an error should be thrown if no tokens are found when evaluating part of the expression.</param>
        /// <returns>A <see cref="JToken" />.</returns>
        /// <MetaDataID>{33f0ef2c-e735-4c12-b6e7-0610b37166cd}</MetaDataID>
        public JToken SelectToken(string path, bool errorWhenNoMatch)
        {
            JPath p = new JPath(path);

            JToken token = null;
            foreach (JToken t in p.Evaluate(this, this, errorWhenNoMatch))
            {
                if (token != null)
                {
                    throw new JsonException("Path returned multiple tokens.");
                }

                token = t;
            }

            return token;
        }

        /// <summary>
        /// Selects a collection of elements using a JPath expression.
        /// </summary>
        /// <param name="path">
        /// A <see cref="String" /> that contains a JPath expression.
        /// </param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="JToken" /> that contains the selected elements.</returns>
        /// <MetaDataID>{b411ec40-b5a4-441a-813d-59afc83a1438}</MetaDataID>
        public IEnumerable<JToken> SelectTokens(string path)
        {
            return SelectTokens(path, false);
        }

        /// <summary>
        /// Selects a collection of elements using a JPath expression.
        /// </summary>
        /// <param name="path">
        /// A <see cref="String" /> that contains a JPath expression.
        /// </param>
        /// <param name="errorWhenNoMatch">A flag to indicate whether an error should be thrown if no tokens are found when evaluating part of the expression.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="JToken" /> that contains the selected elements.</returns>
        /// <MetaDataID>{19ba69e3-1cb8-46b3-a48e-8d6e0de0a5c2}</MetaDataID>
        public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
        {
            JPath p = new JPath(path);
            return p.Evaluate(this, this, errorWhenNoMatch);
        }

#if HAVE_DYNAMIC
        /// <summary>
        /// Returns the <see cref="DynamicMetaObject" /> responsible for binding operations performed on this object.
        /// </summary>
        /// <param name="parameter">The expression tree representation of the runtime value.</param>
        /// <returns>
        /// The <see cref="DynamicMetaObject" /> to bind this object.
        /// </returns>
        /// <MetaDataID>{ab88d8a6-1321-49ce-ae6f-6366c40b37d8}</MetaDataID>
        protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicProxyMetaObject<JToken>(parameter, this, new DynamicProxy<JToken>());
        }

        /// <summary>
        /// Returns the <see cref="DynamicMetaObject" /> responsible for binding operations performed on this object.
        /// </summary>
        /// <param name="parameter">The expression tree representation of the runtime value.</param>
        /// <returns>
        /// The <see cref="DynamicMetaObject" /> to bind this object.
        /// </returns>
        /// <MetaDataID>{9dec525c-75e2-4a24-a439-da58e911a1d0}</MetaDataID>
        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return GetMetaObject(parameter);
        }
#endif

#if HAVE_ICLONEABLE
        /// <MetaDataID>{c1fe7f46-0625-48eb-912e-7b5689c8f42f}</MetaDataID>
        object ICloneable.Clone()
        {
            return DeepClone();
        }
#endif

        /// <summary>
        /// Creates a new instance of the <see cref="JToken" />. All child tokens are recursively cloned.
        /// </summary>
        /// <returns>A new instance of the <see cref="JToken" />.</returns>
        /// <MetaDataID>{051595b9-5f2a-4b15-9dfd-8af79db58c72}</MetaDataID>
        public JToken DeepClone()
        {
            return CloneToken();
        }

        /// <summary>
        /// Adds an object to the annotation list of this <see cref="JToken" />.
        /// </summary>
        /// <param name="annotation">The annotation to add.</param>
        /// <MetaDataID>{ee0e2dde-96d6-4d52-91ea-4d642b722fef}</MetaDataID>
        public void AddAnnotation(object annotation)
        {
            if (annotation == null)
            {
                throw new ArgumentNullException(nameof(annotation));
            }

            if (_annotations == null)
            {
                _annotations = (annotation is object[]) ? new[] { annotation } : annotation;
            }
            else
            {
                if (!(_annotations is object[] annotations))
                {
                    _annotations = new[] { _annotations, annotation };
                }
                else
                {
                    int index = 0;
                    while (index < annotations.Length && annotations[index] != null)
                    {
                        index++;
                    }
                    if (index == annotations.Length)
                    {
                        Array.Resize(ref annotations, index * 2);
                        _annotations = annotations;
                    }
                    annotations[index] = annotation;
                }
            }
        }

        /// <summary>
        /// Get the first annotation object of the specified type from this <see cref="JToken" />.
        /// </summary>
        /// <typeparam name="T">The type of the annotation to retrieve.</typeparam>
        /// <returns>The first annotation object that matches the specified type, or <c>null</c> if no annotation is of the specified type.</returns>
        /// <MetaDataID>{512769b1-8184-4041-b3b7-705e97dc58ba}</MetaDataID>
        public T Annotation<T>() where T : class
        {
            if (_annotations != null)
            {
                if (!(_annotations is object[] annotations))
                {
                    return (_annotations as T);
                }
                for (int i = 0; i < annotations.Length; i++)
                {
                    object annotation = annotations[i];
                    if (annotation == null)
                    {
                        break;
                    }

                    if (annotation is T local)
                    {
                        return local;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the first annotation object of the specified type from this <see cref="JToken" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> of the annotation to retrieve.</param>
        /// <returns>The first annotation object that matches the specified type, or <c>null</c> if no annotation is of the specified type.</returns>
        /// <MetaDataID>{8c46e64e-80bb-4221-bb5b-1f093efd0544}</MetaDataID>
        public object Annotation(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_annotations != null)
            {
                if (!(_annotations is object[] annotations))
                {
                    if (type.IsInstanceOfType(_annotations))
                    {
                        return _annotations;
                    }
                }
                else
                {
                    for (int i = 0; i < annotations.Length; i++)
                    {
                        object o = annotations[i];
                        if (o == null)
                        {
                            break;
                        }

                        if (type.IsInstanceOfType(o))
                        {
                            return o;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a collection of annotations of the specified type for this <see cref="JToken" />.
        /// </summary>
        /// <typeparam name="T">The type of the annotations to retrieve.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains the annotations for this <see cref="JToken" />.</returns>
        /// <MetaDataID>{4cbbea40-f4f7-481e-b191-9beddd544e22}</MetaDataID>
        public IEnumerable<T> Annotations<T>() where T : class
        {
            if (_annotations == null)
            {
                yield break;
            }

            if (_annotations is object[] annotations)
            {
                for (int i = 0; i < annotations.Length; i++)
                {
                    object o = annotations[i];
                    if (o == null)
                    {
                        break;
                    }

                    if (o is T casted)
                    {
                        yield return casted;
                    }
                }
                yield break;
            }

            if (!(_annotations is T annotation))
            {
                yield break;
            }

            yield return annotation;
        }

        /// <summary>
        /// Gets a collection of annotations of the specified type for this <see cref="JToken" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> of the annotations to retrieve.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="Object" /> that contains the annotations that match the specified type for this <see cref="JToken" />.</returns>
        /// <MetaDataID>{0d84500b-727d-4707-8d8a-a1765d1eff51}</MetaDataID>
        public IEnumerable<object> Annotations(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_annotations == null)
            {
                yield break;
            }

            if (_annotations is object[] annotations)
            {
                for (int i = 0; i < annotations.Length; i++)
                {
                    object o = annotations[i];
                    if (o == null)
                    {
                        break;
                    }

                    if (type.IsInstanceOfType(o))
                    {
                        yield return o;
                    }
                }
                yield break;
            }

            if (!type.IsInstanceOfType(_annotations))
            {
                yield break;
            }

            yield return _annotations;
        }

        /// <summary>
        /// Removes the annotations of the specified type from this <see cref="JToken" />.
        /// </summary>
        /// <typeparam name="T">The type of annotations to remove.</typeparam>
        /// <MetaDataID>{0144fde8-0989-4261-ba4e-6643b13b51f1}</MetaDataID>
        public void RemoveAnnotations<T>() where T : class
        {
            if (_annotations != null)
            {
                if (!(_annotations is object[] annotations))
                {
                    if (_annotations is T)
                    {
                        _annotations = null;
                    }
                }
                else
                {
                    int index = 0;
                    int keepCount = 0;
                    while (index < annotations.Length)
                    {
                        object obj2 = annotations[index];
                        if (obj2 == null)
                        {
                            break;
                        }

                        if (!(obj2 is T))
                        {
                            annotations[keepCount++] = obj2;
                        }

                        index++;
                    }

                    if (keepCount != 0)
                    {
                        while (keepCount < index)
                        {
                            annotations[keepCount++] = null;
                        }
                    }
                    else
                    {
                        _annotations = null;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the annotations of the specified type from this <see cref="JToken" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> of annotations to remove.</param>
        /// <MetaDataID>{b2b0e3b5-0037-4d6b-b345-5aabb9291753}</MetaDataID>
        public void RemoveAnnotations(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_annotations != null)
            {
                if (!(_annotations is object[] annotations))
                {
                    if (type.IsInstanceOfType(_annotations))
                    {
                        _annotations = null;
                    }
                }
                else
                {
                    int index = 0;
                    int keepCount = 0;
                    while (index < annotations.Length)
                    {
                        object o = annotations[index];
                        if (o == null)
                        {
                            break;
                        }

                        if (!type.IsInstanceOfType(o))
                        {
                            annotations[keepCount++] = o;
                        }

                        index++;
                    }

                    if (keepCount != 0)
                    {
                        while (keepCount < index)
                        {
                            annotations[keepCount++] = null;
                        }
                    }
                    else
                    {
                        _annotations = null;
                    }
                }
            }
        }
    }
}