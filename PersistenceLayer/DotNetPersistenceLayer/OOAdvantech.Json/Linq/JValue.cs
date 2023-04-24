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
using System.Diagnostics;
using OOAdvantech.Json.Utilities;
using System.Globalization;
#if HAVE_DYNAMIC
using System.Dynamic;
using System.Linq.Expressions;
#endif
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif

namespace OOAdvantech.Json.Linq
{
    /// <summary>
    /// Represents a value in JSON (string, integer, date, etc).
    /// </summary>
    /// <MetaDataID>OOAdvantech.Json.Linq.JValue</MetaDataID>
    public partial class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>
#if HAVE_ICONVERTIBLE
        , IConvertible
#endif
    {
        /// <MetaDataID>{31c414ae-f97b-4a53-a811-5ee3b921d135}</MetaDataID>
        private JTokenType _valueType;
        /// <MetaDataID>{d171f81e-ac71-464e-8b5e-29d2eb68c2dc}</MetaDataID>
        private object _value;

        /// <MetaDataID>{1bf0d41c-0520-46b5-8f0f-bc9a3083ca43}</MetaDataID>
        internal JValue(object value, JTokenType type)
        {
            _value = value;
            _valueType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class from another <see cref="JValue" /> object.
        /// </summary>
        /// <param name="other">A <see cref="JValue" /> object to copy from.</param>
        /// <MetaDataID>{45b82e81-a751-4700-bb18-9433dd6de105}</MetaDataID>
        public JValue(JValue other)
            : this(other.Value, other.Type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{505ded3b-a74b-4534-8997-6015c5551e5b}</MetaDataID>
        public JValue(long value)
            : this(value, JTokenType.Integer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{c48b4e59-a685-4ae5-8feb-675fd7fa660d}</MetaDataID>
        public JValue(decimal value)
            : this(value, JTokenType.Float)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{f9f08368-dd75-4e52-8472-cdd870e46a99}</MetaDataID>
        public JValue(char value)
            : this(value, JTokenType.String)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{4fe8f907-4aba-49ac-8e1a-0825435f7753}</MetaDataID>
        [CLSCompliant(false)]
        public JValue(ulong value)
            : this(value, JTokenType.Integer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{31853a4d-453e-44c2-a719-45435919aeca}</MetaDataID>
        public JValue(double value)
            : this(value, JTokenType.Float)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{f1045e8b-1656-4b0f-8ce2-afcffe4fad92}</MetaDataID>
        public JValue(float value)
            : this(value, JTokenType.Float)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{ee2494bd-37f5-4f85-b02a-a1ab4ef5aebe}</MetaDataID>
        public JValue(DateTime value)
            : this(value, JTokenType.Date)
        {
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{30a25cff-0b94-4cbe-a694-98dc0be0d804}</MetaDataID>
        public JValue(DateTimeOffset value)
            : this(value, JTokenType.Date)
        {
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{d7c35c7b-9386-42c7-a25b-e487dcf63ca6}</MetaDataID>
        public JValue(bool value)
            : this(value, JTokenType.Boolean)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{ad34c232-5cc5-4aea-9413-a62a80ad64cd}</MetaDataID>
        public JValue(string value)
            : this(value, JTokenType.String)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{ced4014f-02aa-4521-9644-32fe0be1e279}</MetaDataID>
        public JValue(Guid value)
            : this(value, JTokenType.Guid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{b5213f2e-d8f4-4dab-8b32-9943fa753e2c}</MetaDataID>
        public JValue(Uri value)
            : this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{5343c3d7-5f7c-421a-b955-47d1a39a6749}</MetaDataID>
        public JValue(TimeSpan value)
            : this(value, JTokenType.TimeSpan)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue" /> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{033d18aa-113b-4fb7-b2b0-65ab2d289c22}</MetaDataID>
        public JValue(object value)
            : this(value, GetValueType(null, value))
        {
        }

        /// <MetaDataID>{e6b99fb2-5dbc-4237-9064-b0951eb76ce2}</MetaDataID>
        internal override bool DeepEquals(JToken node)
        {
            if (!(node is JValue other))
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }

            return ValuesEquals(this, other);
        }

        /// <summary>
        /// Gets a value indicating whether this token has child tokens.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token has child values; otherwise, <c>false</c>.
        /// </value>
        /// <MetaDataID>{55b80673-5dbc-4abc-a33e-7e73160986d6}</MetaDataID>
        public override bool HasValues => false;

#if HAVE_BIG_INTEGER
        /// <MetaDataID>{64d66473-70d6-41fc-ad36-1241123a9e1f}</MetaDataID>
        private static int CompareBigInteger(BigInteger i1, object i2)
        {
            int result = i1.CompareTo(ConvertUtils.ToBigInteger(i2));

            if (result != 0)
            {
                return result;
            }

            // converting a fractional number to a BigInteger will lose the fraction
            // check for fraction if result is two numbers are equal
            if (i2 is decimal d1)
            {
                return (0m).CompareTo(Math.Abs(d1 - Math.Truncate(d1)));
            }
            else if (i2 is double || i2 is float)
            {
                double d = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
                return (0d).CompareTo(Math.Abs(d - Math.Truncate(d)));
            }

            return result;
        }
#endif

        /// <MetaDataID>{6ca23df4-0e56-43f4-a87b-0716b8ee1943}</MetaDataID>
        internal static int Compare(JTokenType valueType, object objA, object objB)
        {
            if (objA == objB)
            {
                return 0;
            }
            if (objB == null)
            {
                return 1;
            }
            if (objA == null)
            {
                return -1;
            }

            switch (valueType)
            {
                case JTokenType.Integer:
                    {
#if HAVE_BIG_INTEGER
                        if (objA is BigInteger integerA)
                        {
                            return CompareBigInteger(integerA, objB);
                        }
                        if (objB is BigInteger integerB)
                        {
                            return -CompareBigInteger(integerB, objA);
                        }
#endif
                        if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
                        {
                            return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
                        }
                        else if (objA is float || objB is float || objA is double || objB is double)
                        {
                            return CompareFloat(objA, objB);
                        }
                        else
                        {
                            return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
                        }
                    }
                case JTokenType.Float:
                    {
#if HAVE_BIG_INTEGER
                        if (objA is BigInteger integerA)
                        {
                            return CompareBigInteger(integerA, objB);
                        }
                        if (objB is BigInteger integerB)
                        {
                            return -CompareBigInteger(integerB, objA);
                        }
#endif
                        if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
                        {
                            return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
                        }
                        return CompareFloat(objA, objB);
                    }
                case JTokenType.Comment:
                case JTokenType.String:
                case JTokenType.Raw:
                    string s1 = Convert.ToString(objA, CultureInfo.InvariantCulture);
                    string s2 = Convert.ToString(objB, CultureInfo.InvariantCulture);

                    return string.CompareOrdinal(s1, s2);
                case JTokenType.Boolean:
                    bool b1 = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
                    bool b2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);

                    return b1.CompareTo(b2);
                case JTokenType.Date:
#if HAVE_DATE_TIME_OFFSET
                    if (objA is DateTime dateA)
                    {
#else
                        DateTime dateA = (DateTime)objA;
#endif
                        DateTime dateB;

#if HAVE_DATE_TIME_OFFSET
                        if (objB is DateTimeOffset offsetB)
                        {
                            dateB = offsetB.DateTime;
                        }
                        else
#endif
                        {
                            dateB = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
                        }

                        return dateA.CompareTo(dateB);
#if HAVE_DATE_TIME_OFFSET
                    }
                    else
                    {
                        DateTimeOffset offsetA = (DateTimeOffset)objA;
                        if (!(objB is DateTimeOffset offsetB))
                        {
                            offsetB = new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture));
                        }

                        return offsetA.CompareTo(offsetB);
                    }
#endif
                case JTokenType.Bytes:
                    if (!(objB is byte[] bytesB))
                    {
                        throw new ArgumentException("Object must be of type byte[].");
                    }

                    byte[] bytesA = objA as byte[];
                    Debug.Assert(bytesA != null);

                    return MiscellaneousUtils.ByteArrayCompare(bytesA, bytesB);
                case JTokenType.Guid:
                    if (!(objB is Guid))
                    {
                        throw new ArgumentException("Object must be of type Guid.");
                    }

                    Guid guid1 = (Guid)objA;
                    Guid guid2 = (Guid)objB;

                    return guid1.CompareTo(guid2);
                case JTokenType.Uri:
                    Uri uri2 = objB as Uri;
                    if (uri2 == null)
                    {
                        throw new ArgumentException("Object must be of type Uri.");
                    }

                    Uri uri1 = (Uri)objA;

                    return Comparer<string>.Default.Compare(uri1.ToString(), uri2.ToString());
                case JTokenType.TimeSpan:
                    if (!(objB is TimeSpan))
                    {
                        throw new ArgumentException("Object must be of type TimeSpan.");
                    }

                    TimeSpan ts1 = (TimeSpan)objA;
                    TimeSpan ts2 = (TimeSpan)objB;

                    return ts1.CompareTo(ts2);
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof(valueType), valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
            }
        }

        /// <MetaDataID>{da0f2946-c41e-4643-bdc1-6da1ec321469}</MetaDataID>
        private static int CompareFloat(object objA, object objB)
        {
            double d1 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
            double d2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);

            // take into account possible floating point errors
            if (MathUtils.ApproxEquals(d1, d2))
            {
                return 0;
            }

            return d1.CompareTo(d2);
        }

#if HAVE_EXPRESSIONS
        /// <MetaDataID>{0bde3cc6-75ba-431f-a620-4e2a801e48d0}</MetaDataID>
        private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
        {
            if (objA is string || objB is string)
            {
                if (operation == ExpressionType.Add || operation == ExpressionType.AddAssign)
                {
                    result = objA?.ToString() + objB?.ToString();
                    return true;
                }
            }

#if HAVE_BIG_INTEGER
            if (objA is BigInteger || objB is BigInteger)
            {
                if (objA == null || objB == null)
                {
                    result = null;
                    return true;
                }

                // not that this will lose the fraction
                // BigInteger doesn't have operators with non-integer types
                BigInteger i1 = ConvertUtils.ToBigInteger(objA);
                BigInteger i2 = ConvertUtils.ToBigInteger(objB);

                switch (operation)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddAssign:
                        result = i1 + i2;
                        return true;
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractAssign:
                        result = i1 - i2;
                        return true;
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyAssign:
                        result = i1 * i2;
                        return true;
                    case ExpressionType.Divide:
                    case ExpressionType.DivideAssign:
                        result = i1 / i2;
                        return true;
                }
            }
            else
#endif
                if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
            {
                if (objA == null || objB == null)
                {
                    result = null;
                    return true;
                }

                decimal d1 = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
                decimal d2 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);

                switch (operation)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddAssign:
                        result = d1 + d2;
                        return true;
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractAssign:
                        result = d1 - d2;
                        return true;
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyAssign:
                        result = d1 * d2;
                        return true;
                    case ExpressionType.Divide:
                    case ExpressionType.DivideAssign:
                        result = d1 / d2;
                        return true;
                }
            }
            else if (objA is float || objB is float || objA is double || objB is double)
            {
                if (objA == null || objB == null)
                {
                    result = null;
                    return true;
                }

                double d1 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
                double d2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);

                switch (operation)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddAssign:
                        result = d1 + d2;
                        return true;
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractAssign:
                        result = d1 - d2;
                        return true;
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyAssign:
                        result = d1 * d2;
                        return true;
                    case ExpressionType.Divide:
                    case ExpressionType.DivideAssign:
                        result = d1 / d2;
                        return true;
                }
            }
            else if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte ||
                     objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
            {
                if (objA == null || objB == null)
                {
                    result = null;
                    return true;
                }

                long l1 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
                long l2 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);

                switch (operation)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddAssign:
                        result = l1 + l2;
                        return true;
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractAssign:
                        result = l1 - l2;
                        return true;
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyAssign:
                        result = l1 * l2;
                        return true;
                    case ExpressionType.Divide:
                    case ExpressionType.DivideAssign:
                        result = l1 / l2;
                        return true;
                }
            }

            result = null;
            return false;
        }
#endif

        /// <MetaDataID>{e2c96601-2c6e-4cba-97b6-0459da32ad44}</MetaDataID>
        internal override JToken CloneToken()
        {
            return new JValue(this);
        }

        /// <summary>
        /// Creates a <see cref="JValue" /> comment with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="JValue" /> comment with the given value.</returns>
        /// <MetaDataID>{43807ff2-4596-4cb5-8094-846a2d497898}</MetaDataID>
        public static JValue CreateComment(string value)
        {
            return new JValue(value, JTokenType.Comment);
        }

        /// <summary>
        /// Creates a <see cref="JValue" /> string with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="JValue" /> string with the given value.</returns>
        /// <MetaDataID>{7dcc9680-3b76-494e-ae26-294608dbc10e}</MetaDataID>
        public static JValue CreateString(string value)
        {
            return new JValue(value, JTokenType.String);
        }

        /// <summary>
        /// Creates a <see cref="JValue" /> null value.
        /// </summary>
        /// <returns>A <see cref="JValue" /> null value.</returns>
        /// <MetaDataID>{eeccb11c-b32b-486c-baff-7cb9772a5adb}</MetaDataID>
        public static JValue CreateNull()
        {
            return new JValue(null, JTokenType.Null);
        }

        /// <summary>
        /// Creates a <see cref="JValue" /> undefined value.
        /// </summary>
        /// <returns>A <see cref="JValue" /> undefined value.</returns>
        /// <MetaDataID>{997fa359-ecbe-4ea2-b88c-b030f1c1718d}</MetaDataID>
        public static JValue CreateUndefined()
        {
            return new JValue(null, JTokenType.Undefined);
        }

        /// <MetaDataID>{227ce6d9-d7f1-40c9-9994-e64a5db529c8}</MetaDataID>
        private static JTokenType GetValueType(JTokenType? current, object value)
        {
            if (value == null)
            {
                return JTokenType.Null;
            }
#if HAVE_ADO_NET
            else if (value == DBNull.Value)
            {
                return JTokenType.Null;
            }
#endif
            else if (value is string)
            {
                return GetStringValueType(current);
            }
            else if (value is long || value is int || value is short || value is sbyte
                     || value is ulong || value is uint || value is ushort || value is byte)
            {
                return JTokenType.Integer;
            }
            else if (value is Enum)
            {
                return JTokenType.Integer;
            }
#if HAVE_BIG_INTEGER
            else if (value is BigInteger)
            {
                return JTokenType.Integer;
            }
#endif
            else if (value is double || value is float || value is decimal)
            {
                return JTokenType.Float;
            }
            else if (value is DateTime)
            {
                return JTokenType.Date;
            }
#if HAVE_DATE_TIME_OFFSET
            else if (value is DateTimeOffset)
            {
                return JTokenType.Date;
            }
#endif
            else if (value is byte[])
            {
                return JTokenType.Bytes;
            }
            else if (value is bool)
            {
                return JTokenType.Boolean;
            }
            else if (value is Guid)
            {
                return JTokenType.Guid;
            }
            else if (value is Uri)
            {
                return JTokenType.Uri;
            }
            else if (value is TimeSpan)
            {
                return JTokenType.TimeSpan;
            }

            throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
        }

        /// <MetaDataID>{94c2a2c9-cf3b-49dc-aa47-8c3475338cf2}</MetaDataID>
        private static JTokenType GetStringValueType(JTokenType? current)
        {
            if (current == null)
            {
                return JTokenType.String;
            }

            switch (current.GetValueOrDefault())
            {
                case JTokenType.Comment:
                case JTokenType.String:
                case JTokenType.Raw:
                    return current.GetValueOrDefault();
                default:
                    return JTokenType.String;
            }
        }

        /// <summary>
        /// Gets the node type for this <see cref="JToken" />.
        /// </summary>
        /// <value>The type.</value>
        /// <MetaDataID>{a1944b4a-9808-4582-a4b2-701aff7d8196}</MetaDataID>
        public override JTokenType Type => _valueType;

        /// <summary>
        /// Gets or sets the underlying token value.
        /// </summary>
        /// <value>The underlying token value.</value>
        /// <MetaDataID>{5b578497-422c-420b-b9f8-894e5fb6bc5a}</MetaDataID>
        public object Value
        {
            get => _value;
            set
            {
                Type currentType = _value?.GetType();
                Type newType = value?.GetType();

                if (currentType != newType)
                {
                    _valueType = GetValueType(_valueType, value);
                }

                _value = value;
            }
        }

        /// <summary>
        /// Writes this token to a <see cref="JsonWriter" />.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter" /> into which this method will write.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter" />s which will be used when writing the token.</param>
        /// <MetaDataID>{322a5e63-b1de-4de5-a87f-a77fa30de027}</MetaDataID>
        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            if (converters != null && converters.Length > 0 && _value != null)
            {
                JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, _value.GetType());
                if (matchingConverter != null && matchingConverter.CanWrite)
                {
                    matchingConverter.WriteJson(writer, _value, JsonSerializer.CreateDefault());
                    return;
                }
            }

            switch (_valueType)
            {
                case JTokenType.Comment:
                    writer.WriteComment(_value?.ToString());
                    return;
                case JTokenType.Raw:
                    writer.WriteRawValue(_value?.ToString());
                    return;
                case JTokenType.Null:
                    writer.WriteNull();
                    return;
                case JTokenType.Undefined:
                    writer.WriteUndefined();
                    return;
                case JTokenType.Integer:
                    if (_value is int i)
                    {
                        writer.WriteValue(i);
                    }
                    else if (_value is long l)
                    {
                        writer.WriteValue(l);
                    }
                    else if (_value is ulong ul)
                    {
                        writer.WriteValue(ul);
                    }
#if HAVE_BIG_INTEGER
                    else if (_value is BigInteger integer)
                    {
                        writer.WriteValue(integer);
                    }
#endif
                    else
                    {
                        writer.WriteValue(Convert.ToInt64(_value, CultureInfo.InvariantCulture));
                    }
                    return;
                case JTokenType.Float:
                    if (_value is decimal dec)
                    {
                        writer.WriteValue(dec);
                    }
                    else if (_value is double d)
                    {
                        writer.WriteValue(d);
                    }
                    else if (_value is float f)
                    {
                        writer.WriteValue(f);
                    }
                    else
                    {
                        writer.WriteValue(Convert.ToDouble(_value, CultureInfo.InvariantCulture));
                    }
                    return;
                case JTokenType.String:
                    writer.WriteValue(_value?.ToString());
                    return;
                case JTokenType.Boolean:
                    writer.WriteValue(Convert.ToBoolean(_value, CultureInfo.InvariantCulture));
                    return;
                case JTokenType.Date:
#if HAVE_DATE_TIME_OFFSET
                    if (_value is DateTimeOffset offset)
                    {
                        writer.WriteValue(offset);
                    }
                    else
#endif
                    {
                        writer.WriteValue(Convert.ToDateTime(_value, CultureInfo.InvariantCulture));
                    }
                    return;
                case JTokenType.Bytes:
                    writer.WriteValue((byte[])_value);
                    return;
                case JTokenType.Guid:
                    writer.WriteValue((_value != null) ? (Guid?)_value : null);
                    return;
                case JTokenType.TimeSpan:
                    writer.WriteValue((_value != null) ? (TimeSpan?)_value : null);
                    return;
                case JTokenType.Uri:
                    writer.WriteValue((Uri)_value);
                    return;
            }

            throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof(Type), _valueType, "Unexpected token type.");
        }

        /// <MetaDataID>{42c25634-4235-4210-bab7-3e796f81a6c2}</MetaDataID>
        internal override int GetDeepHashCode()
        {
            int valueHashCode = (_value != null) ? _value.GetHashCode() : 0;

            // GetHashCode on an enum boxes so cast to int
            return ((int)_valueType).GetHashCode() ^ valueHashCode;
        }

        /// <MetaDataID>{51a39523-ab7c-4757-b71b-31c03211a3a5}</MetaDataID>
        private static bool ValuesEquals(JValue v1, JValue v2)
        {
            return (v1 == v2 || (v1._valueType == v2._valueType && Compare(v1._valueType, v1._value, v2._value) == 0));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        /// <MetaDataID>{2212b988-c1f0-4a81-abed-8899fc99e5f4}</MetaDataID>
        public bool Equals(JValue other)
        {
            if (other == null)
            {
                return false;
            }

            return ValuesEquals(this, other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object" /> is equal to the current <see cref="Object" />.
        /// </summary>
        /// <param name="obj">The <see cref="Object" /> to compare with the current <see cref="Object" />.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Object" /> is equal to the current <see cref="Object" />; otherwise, <c>false</c>.
        /// </returns>
        /// <MetaDataID>{b763335d-22b2-4d4d-8fc3-5994bf0d361e}</MetaDataID>
        public override bool Equals(object obj)
        {
            return Equals(obj as JValue);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="Object" />.
        /// </returns>
        /// <MetaDataID>{5edb2b1d-ed0a-4289-9815-a68fad2ee90d}</MetaDataID>
        public override int GetHashCode()
        {
            if (_value == null)
            {
                return 0;
            }

            return _value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String" /> that represents this instance.
        /// </returns>
        /// <MetaDataID>{1a17289c-6da7-499d-8a9f-3dc8fcf70b32}</MetaDataID>
        public override string ToString()
        {
            if (_value == null)
            {
                return string.Empty;
            }

            return _value.ToString();
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="String" /> that represents this instance.
        /// </returns>
        /// <MetaDataID>{cc295d3a-f43d-49ac-ac00-4b90ca12b560}</MetaDataID>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="String" /> that represents this instance.
        /// </returns>
        /// <MetaDataID>{2bd6c941-5207-4c62-a89e-d6b97ebc4838}</MetaDataID>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="String" /> that represents this instance.
        /// </returns>
        /// <MetaDataID>{d78d7c38-2f9c-4a59-9023-c2ace8d94e6f}</MetaDataID>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (_value == null)
            {
                return string.Empty;
            }

            if (_value is IFormattable formattable)
            {
                return formattable.ToString(format, formatProvider);
            }
            else
            {
                return _value.ToString();
            }
        }

#if HAVE_DYNAMIC
        /// <summary>
        /// Returns the <see cref="DynamicMetaObject" /> responsible for binding operations performed on this object.
        /// </summary>
        /// <param name="parameter">The expression tree representation of the runtime value.</param>
        /// <returns>
        /// The <see cref="DynamicMetaObject" /> to bind this object.
        /// </returns>
        /// <MetaDataID>{e1ec4247-5bd1-483d-8190-1892ee590b60}</MetaDataID>
        protected override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicProxyMetaObject<JValue>(parameter, this, new JValueDynamicProxy());
        }

        private class JValueDynamicProxy : DynamicProxy<JValue>
        {
            public override bool TryConvert(JValue instance, ConvertBinder binder, out object result)
            {
                if (binder.Type == typeof(JValue) || binder.Type == typeof(JToken))
                {
                    result = instance;
                    return true;
                }

                object value = instance.Value;

                if (value == null)
                {
                    result = null;
                    return ReflectionUtils.IsNullable(binder.Type);
                }

                result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
                return true;
            }

            public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, out object result)
            {
                object compareValue = arg is JValue value ? value.Value : arg;

                switch (binder.Operation)
                {
                    case ExpressionType.Equal:
                        result = (Compare(instance.Type, instance.Value, compareValue) == 0);
                        return true;
                    case ExpressionType.NotEqual:
                        result = (Compare(instance.Type, instance.Value, compareValue) != 0);
                        return true;
                    case ExpressionType.GreaterThan:
                        result = (Compare(instance.Type, instance.Value, compareValue) > 0);
                        return true;
                    case ExpressionType.GreaterThanOrEqual:
                        result = (Compare(instance.Type, instance.Value, compareValue) >= 0);
                        return true;
                    case ExpressionType.LessThan:
                        result = (Compare(instance.Type, instance.Value, compareValue) < 0);
                        return true;
                    case ExpressionType.LessThanOrEqual:
                        result = (Compare(instance.Type, instance.Value, compareValue) <= 0);
                        return true;
                    case ExpressionType.Add:
                    case ExpressionType.AddAssign:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractAssign:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyAssign:
                    case ExpressionType.Divide:
                    case ExpressionType.DivideAssign:
                        if (Operation(binder.Operation, instance.Value, compareValue, out result))
                        {
                            result = new JValue(result);
                            return true;
                        }
                        break;
                }

                result = null;
                return false;
            }
        }
#endif

        /// <MetaDataID>{ce6739f9-6707-4d69-87cb-c69c767e9771}</MetaDataID>
        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            JTokenType comparisonType;
            object otherValue;
            if (obj is JValue value)
            {
                otherValue = value.Value;
                comparisonType = (_valueType == JTokenType.String && _valueType != value._valueType)
                    ? value._valueType
                    : _valueType;
            }
            else
            {
                otherValue = obj;
                comparisonType = _valueType;
            }

            return Compare(comparisonType, _value, otherValue);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance is less than <paramref name="obj" />.
        /// Zero
        /// This instance is equal to <paramref name="obj" />.
        /// Greater than zero
        /// This instance is greater than <paramref name="obj" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   <paramref name="obj" /> is not of the same type as this instance.
        /// </exception>
        /// <MetaDataID>{2b979f9c-e6f7-4106-98ec-ed2f9face1d8}</MetaDataID>
        public int CompareTo(JValue obj)
        {
            if (obj == null)
            {
                return 1;
            }

            JTokenType comparisonType = (_valueType == JTokenType.String && _valueType != obj._valueType)
                ? obj._valueType
                : _valueType;

            return Compare(comparisonType, _value, obj._value);
        }

#if HAVE_ICONVERTIBLE
        /// <MetaDataID>{9381ba18-01bd-4864-b159-fcf2759891bf}</MetaDataID>
        TypeCode IConvertible.GetTypeCode()
        {
            if (_value == null)
            {
                return TypeCode.Empty;
            }

            if (_value is IConvertible convertable)
            {
                return convertable.GetTypeCode();
            }

            return TypeCode.Object;
        }

        /// <MetaDataID>{ed6f246f-4dfe-48ed-a3e3-46062b5a5caa}</MetaDataID>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool)this;
        }

        /// <MetaDataID>{30d5848c-d6f6-4b5d-aa43-1a3bc9361820}</MetaDataID>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)this;
        }

        /// <MetaDataID>{33328a2a-ec7b-4493-9694-121494a38d9a}</MetaDataID>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)this;
        }

        /// <MetaDataID>{ae0eb78e-f1bf-419c-8367-26424fdc60b8}</MetaDataID>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)this;
        }

        /// <MetaDataID>{6aaaa782-dcdd-42ec-b463-ad6c513b3ea6}</MetaDataID>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)this;
        }

        /// <MetaDataID>{08069e42-bbb9-4ff1-9ab4-5007945b7bce}</MetaDataID>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)this;
        }

        /// <MetaDataID>{5cc013af-a2d5-440c-bcbd-dbe526adeb17}</MetaDataID>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int)this;
        }

        /// <MetaDataID>{a142885d-bfb5-4b38-9f33-999cdd98901c}</MetaDataID>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)this;
        }

        /// <MetaDataID>{6f684772-d730-403a-9fc1-ab1c4128f6bf}</MetaDataID>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (long)this;
        }

        /// <MetaDataID>{21e075e5-7462-49fb-b609-e5192af9788b}</MetaDataID>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)this;
        }

        /// <MetaDataID>{8b468114-8c7d-4add-ada5-ecaec69aa3ca}</MetaDataID>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)this;
        }

        /// <MetaDataID>{d88c642e-3480-453d-9098-1e0f69b3cec4}</MetaDataID>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)this;
        }

        /// <MetaDataID>{ae1a396d-7460-40ee-8246-4a477c8d23d0}</MetaDataID>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)this;
        }

        /// <MetaDataID>{a9ec52df-6631-4f5b-a5de-a1335ed03c7a}</MetaDataID>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return (DateTime)this;
        }

        /// <MetaDataID>{b21e7f4b-f1fc-49bc-8625-a699c3896b9f}</MetaDataID>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ToObject(conversionType);
        }
#endif
    }
}
