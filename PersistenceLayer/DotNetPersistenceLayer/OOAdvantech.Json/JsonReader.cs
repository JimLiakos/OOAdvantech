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
using System.IO;
using System.Globalization;
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
using OOAdvantech.Json.Serialization;
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Json
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to serialized JSON data.
    /// </summary>
    /// <MetaDataID>OOAdvantech.Json.JsonReader</MetaDataID>
    public abstract partial class JsonReader : IDisposable
    {
        /// <summary>
        /// Specifies the state of the reader.
        /// </summary>
        protected internal enum State
        {
            /// <summary>
            /// A <see cref="JsonReader"/> read method has not been called.
            /// </summary>
            Start,

            /// <summary>
            /// The end of the file has been reached successfully.
            /// </summary>
            Complete,

            /// <summary>
            /// Reader is at a property.
            /// </summary>
            Property,

            /// <summary>
            /// Reader is at the start of an object.
            /// </summary>
            ObjectStart,

            /// <summary>
            /// Reader is in an object.
            /// </summary>
            Object,

            /// <summary>
            /// Reader is at the start of an array.
            /// </summary>
            ArrayStart,

            /// <summary>
            /// Reader is in an array.
            /// </summary>
            Array,

            /// <summary>
            /// The <see cref="JsonReader.Close()"/> method has been called.
            /// </summary>
            Closed,

            /// <summary>
            /// Reader has just read a value.
            /// </summary>
            PostValue,

            /// <summary>
            /// Reader is at the start of a constructor.
            /// </summary>
            ConstructorStart,

            /// <summary>
            /// Reader is in a constructor.
            /// </summary>
            Constructor,

            /// <summary>
            /// An error occurred that prevents the read operation from continuing.
            /// </summary>
            Error,

            /// <summary>
            /// The end of the file has been reached successfully.
            /// </summary>
            Finished
        }

        // current Token data
        /// <MetaDataID>{e99b559c-fa83-4c45-bc48-9d4dfdcf08e4}</MetaDataID>
        private JsonToken _tokenType;
        /// <MetaDataID>{818ef8af-9541-4414-8163-bae99fed93b2}</MetaDataID>
        private object _value;
        /// <MetaDataID>{bd97f6d5-68c3-4477-9bf1-8d6f9b151942}</MetaDataID>
        internal char _quoteChar;
        /// <MetaDataID>{8902ae81-5fd6-4b6e-9ec6-ab02f5e8023d}</MetaDataID>
        internal State _currentState;
        /// <MetaDataID>{92078b5f-9267-46d0-aaef-e523061445cf}</MetaDataID>
        private JsonPosition _currentPosition;
        /// <MetaDataID>{3fe6d43e-5254-4e67-9924-0cf094ecf23c}</MetaDataID>
        private CultureInfo _culture;
        /// <MetaDataID>{a77b53f6-bc61-41c0-bad8-abbef5a6b7ef}</MetaDataID>
        private DateTimeZoneHandling _dateTimeZoneHandling;
        /// <MetaDataID>{7372af45-a1b8-40d1-b19d-8b082ce36bd8}</MetaDataID>
        private int? _maxDepth;
        /// <MetaDataID>{a0717c07-20bf-4086-9a4a-a0e550fb61d1}</MetaDataID>
        private bool _hasExceededMaxDepth;
        /// <MetaDataID>{802d9d8c-3c48-4a03-9400-61d7e710dc60}</MetaDataID>
        internal DateParseHandling _dateParseHandling;
        /// <MetaDataID>{1591a5e5-6387-40c3-ba37-bf5ee5c2e9ab}</MetaDataID>
        internal FloatParseHandling _floatParseHandling;
        /// <MetaDataID>{07d78457-143d-43dc-813a-24e02b7ad4a1}</MetaDataID>
        private string _dateFormatString;
        /// <MetaDataID>{75d5a404-0295-4d33-9f97-8974a28d1877}</MetaDataID>
        private List<JsonPosition> _stack;

        /// <summary>
        /// Gets the current reader state.
        /// </summary>
        /// <value>The current reader state.</value>
        /// <MetaDataID>{f72c4461-0cae-44c9-a89a-3be758bd1331}</MetaDataID>
        protected State CurrentState => _currentState;

        /// <summary>
        /// Gets or sets a value indicating whether the source should be closed when this reader is closed.
        /// </summary>
        /// <value>
        ///   <c>true</c> to close the source when this reader is closed; otherwise <c>false</c>. The default is <c>true</c>.
        /// </value>
        /// <MetaDataID>{a1643815-bcb0-40bf-9ed1-6b2efaca0ebf}</MetaDataID>
        public bool CloseInput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple pieces of JSON content can
        /// be read from a continuous stream without erroring.
        /// </summary>
        /// <value>
        ///   <c>true</c> to support reading multiple pieces of JSON content; otherwise <c>false</c>.
        /// The default is <c>false</c>.
        /// </value>
        /// <MetaDataID>{3dd5571f-cdf6-453b-bf01-0a2acaf573dc}</MetaDataID>
        public bool SupportMultipleContent { get; set; }

        /// <summary>
        /// Gets the quotation mark character used to enclose the value of a string.
        /// </summary>
        /// <MetaDataID>{3bcf8518-fc3e-4f66-956b-e970d7b3474d}</MetaDataID>
        public virtual char QuoteChar
        {
            get => _quoteChar;
            protected internal set => _quoteChar = value;
        }

        /// <summary>
        /// Gets or sets how <see cref="DateTime" /> time zones are handled when reading JSON.
        /// </summary>
        /// <MetaDataID>{068f5502-9f5a-44dc-9a7e-f3c41a9cec49}</MetaDataID>
        public DateTimeZoneHandling DateTimeZoneHandling
        {
            get => _dateTimeZoneHandling;
            set
            {
                if (value < DateTimeZoneHandling.Local || value > DateTimeZoneHandling.RoundtripKind)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _dateTimeZoneHandling = value;
            }
        }

        /// <summary>
        /// Gets or sets how date formatted strings, e.g. "\/Date(1198908717056)\/" and "2012-03-21T05:40Z", are parsed when reading JSON.
        /// </summary>
        /// <MetaDataID>{035ab6cb-8dfb-45c0-84c3-fb229f0dc573}</MetaDataID>
        public DateParseHandling DateParseHandling
        {
            get => _dateParseHandling;
            set
            {
                if (value < DateParseHandling.None ||
#if HAVE_DATE_TIME_OFFSET
                    value > DateParseHandling.DateTimeOffset
#else
                    value > DateParseHandling.DateTime
#endif
                    )
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _dateParseHandling = value;
            }
        }

        /// <summary>
        /// Gets or sets how floating point numbers, e.g. 1.0 and 9.9, are parsed when reading JSON text.
        /// </summary>
        /// <MetaDataID>{a8ad65f4-94a8-4884-a336-58f90dc1ccf1}</MetaDataID>
        public FloatParseHandling FloatParseHandling
        {
            get => _floatParseHandling;
            set
            {
                if (value < FloatParseHandling.Double || value > FloatParseHandling.Decimal)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _floatParseHandling = value;
            }
        }

        /// <summary>
        /// Gets or sets how custom date formatted strings are parsed when reading JSON.
        /// </summary>
        /// <MetaDataID>{e0a8c9c1-cb0f-4c57-8658-41765f34efa7}</MetaDataID>
        public string DateFormatString
        {
            get => _dateFormatString;
            set => _dateFormatString = value;
        }

        /// <summary>
        /// Gets or sets the maximum depth allowed when reading JSON. Reading past this depth will throw a <see cref="JsonReaderException" />.
        /// </summary>
        /// <MetaDataID>{b4404884-6e56-4e19-bad1-113c6e8ecf79}</MetaDataID>
        public int? MaxDepth
        {
            get => _maxDepth;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Value must be positive.", nameof(value));
                }

                _maxDepth = value;
            }
        }

        /// <summary>
        /// Gets the type of the current JSON token. 
        /// </summary>
        /// <MetaDataID>{fb32ada8-8610-4588-9f72-4eecb39a2fbb}</MetaDataID>
        public virtual JsonToken TokenType => _tokenType;

        /// <summary>
        /// Gets the text value of the current JSON token.
        /// </summary>
        /// <MetaDataID>{ead23864-7afa-4ea6-bc36-2329e688bdc5}</MetaDataID>
        public virtual object Value => _value;

        /// <summary>
        /// Gets the .NET type for the current JSON token.
        /// </summary>
        /// <MetaDataID>{2ef78cda-d2d8-4ab9-b473-b3131dd0049e}</MetaDataID>
        public virtual Type ValueType => _value?.GetType();

        /// <summary>
        /// Gets the depth of the current token in the JSON document.
        /// </summary>
        /// <value>The depth of the current token in the JSON document.</value>
        /// <MetaDataID>{262e6f29-f63e-4f12-9f7e-2d2e05c69de1}</MetaDataID>
        public virtual int Depth
        {
            get
            {
                int depth = _stack?.Count ?? 0;
                if (JsonTokenUtils.IsStartToken(TokenType) || _currentPosition.Type == JsonContainerType.None)
                {
                    return depth;
                }
                else
                {
                    return depth + 1;
                }
            }
        }

        /// <summary>
        /// Gets the path of the current JSON token. 
        /// </summary>
        /// <MetaDataID>{5903fae6-dde5-406c-9e9e-912c15b19120}</MetaDataID>
        public virtual string Path
        {
            get
            {
                if (_currentPosition.Type == JsonContainerType.None)
                {
                    return string.Empty;
                }

                bool insideContainer = (_currentState != State.ArrayStart
                                        && _currentState != State.ConstructorStart
                                        && _currentState != State.ObjectStart);

                JsonPosition? current = insideContainer ? (JsonPosition?)_currentPosition : null;

                return JsonPosition.BuildPath(_stack, current);
            }
        }

        /// <summary>
        /// Gets or sets the culture used when reading JSON. Defaults to <see cref="CultureInfo.InvariantCulture" />.
        /// </summary>
        /// <MetaDataID>{f5d85b5f-4785-4855-9e23-56f1d18fa8e8}</MetaDataID>
        public CultureInfo Culture
        {
            get => _culture ?? CultureInfo.InvariantCulture;
            set => _culture = value;
        }

        /// <MetaDataID>{ad05441d-c0ae-400a-b633-0685ca899189}</MetaDataID>
        internal JsonPosition GetPosition(int depth)
        {
            if (_stack != null && depth < _stack.Count)
            {
                return _stack[depth];
            }

            return _currentPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonReader" /> class.
        /// </summary>
        /// <MetaDataID>{d845d2b8-b34f-4bcb-b7a2-1a1393d31b7e}</MetaDataID>
        protected JsonReader()
        {
            _currentState = State.Start;
            _dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            _dateParseHandling = DateParseHandling.DateTime;
            _floatParseHandling = FloatParseHandling.Double;

            CloseInput = true;
        }

        /// <MetaDataID>{75c06cbe-1332-41c8-a341-1bb32226d3fb}</MetaDataID>
        private void Push(JsonContainerType value)
        {
            UpdateScopeWithFinishedValue();

            if (_currentPosition.Type == JsonContainerType.None)
            {
                _currentPosition = new JsonPosition(value);
            }
            else
            {
                if (_stack == null)
                {
                    _stack = new List<JsonPosition>();
                }

                _stack.Add(_currentPosition);
                _currentPosition = new JsonPosition(value);

                // this is a little hacky because Depth increases when first property/value is written but only testing here is faster/simpler
                if (_maxDepth != null && Depth + 1 > _maxDepth && !_hasExceededMaxDepth)
                {
                    _hasExceededMaxDepth = true;
                    throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, _maxDepth));
                }
            }
        }

        /// <MetaDataID>{3a7eb55f-79b9-455b-8872-ba916c39f97c}</MetaDataID>
        private JsonContainerType Pop()
        {
            JsonPosition oldPosition;
            if (_stack != null && _stack.Count > 0)
            {
                oldPosition = _currentPosition;
                _currentPosition = _stack[_stack.Count - 1];
                _stack.RemoveAt(_stack.Count - 1);
            }
            else
            {
                oldPosition = _currentPosition;
                _currentPosition = new JsonPosition();
            }

            if (_maxDepth != null && Depth <= _maxDepth)
            {
                _hasExceededMaxDepth = false;
            }

            return oldPosition.Type;
        }

        /// <MetaDataID>{8b5362e6-f49b-4f52-9e84-f7aa257b0b90}</MetaDataID>
        private JsonContainerType Peek()
        {
            return _currentPosition.Type;
        }

        /// <summary>
        /// Reads the next JSON token from the source.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the next token was read successfully; <c>false</c> if there are no more tokens to read.</returns>
        /// <MetaDataID>{d51e89c9-969a-4061-b288-8fd2219a5394}</MetaDataID>
        public abstract bool Read();

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="Int32" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Int32" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{fcd71f69-5a5b-4fa0-9178-204a834da997}</MetaDataID>
        public virtual int? ReadAsInt32()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Integer:
                case JsonToken.Float:
                    object v = Value;
                    if (v is int i)
                    {
                        return i;
                    }

#if HAVE_BIG_INTEGER
                    if (v is BigInteger value)
                    {
                        i = (int)value;
                    }
                    else
#endif
                    {
                        try
                        {
                            i = Convert.ToInt32(v, CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            // handle error for large integer overflow exceptions
                            throw JsonReaderException.Create(this, "Could not convert to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, v), ex);
                        }
                    }

                    SetToken(JsonToken.Integer, i, false);
                    return i;
                case JsonToken.String:
                    string s = (string)Value;
                    return ReadInt32String(s);
            }

            throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
        }

        /// <MetaDataID>{cbd6dd4a-8936-4c27-98a4-5208f4c2e63b}</MetaDataID>
        internal int? ReadInt32String(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SetToken(JsonToken.Null, null, false);
                return null;
            }

            if (int.TryParse(s, NumberStyles.Integer, Culture, out int i))
            {
                SetToken(JsonToken.Integer, i, false);
                return i;
            }
            else
            {
                SetToken(JsonToken.String, s, false);
                throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
            }
        }

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="String" />.
        /// </summary>
        /// <returns>A <see cref="String" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{f4f6a482-7c6b-4b54-bc8f-5b8b9234e76d}</MetaDataID>
        public virtual string ReadAsString()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.String:
                    return (string)Value;
            }

            if (JsonTokenUtils.IsPrimitiveToken(t))
            {
                object v = Value;
                if (v != null)
                {
                    string s;
                    if (v is IFormattable formattable)
                    {
                        s = formattable.ToString(null, Culture);
                    }
                    else
                    {
                        s = v is Uri uri ? uri.OriginalString : v.ToString();
                    }

                    SetToken(JsonToken.String, s, false);
                    return s;
                }
            }

            throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
        }

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Byte" />[].
        /// </summary>
        /// <returns>A <see cref="Byte" />[] or <c>null</c> if the next JSON token is null. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{e4f50dee-a783-4582-8891-9daf0be4f505}</MetaDataID>
        public virtual byte[] ReadAsBytes()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.StartObject:
                    {
                        ReadIntoWrappedTypeObject();

                        byte[] data = ReadAsBytes();
                        ReaderReadAndAssert();

                        if (TokenType != JsonToken.EndObject)
                        {
                            throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
                        }

                        SetToken(JsonToken.Bytes, data, false);
                        return data;
                    }
                case JsonToken.String:
                    {
                        // attempt to convert possible base 64 or GUID string to bytes
                        // GUID has to have format 00000000-0000-0000-0000-000000000000
                        string s = (string)Value;

                        byte[] data;

                        if (s.Length == 0)
                        {
                            data = CollectionUtils.ArrayEmpty<byte>();
                        }
                        else if (ConvertUtils.TryConvertGuid(s, out Guid g1))
                        {
                            data = g1.ToByteArray();
                        }
                        else
                        {
                            data = Convert.FromBase64String(s);
                        }

                        SetToken(JsonToken.Bytes, data, false);
                        return data;
                    }
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Bytes:
                    if (Value is Guid g2)
                    {
                        byte[] data = g2.ToByteArray();
                        SetToken(JsonToken.Bytes, data, false);
                        return data;
                    }

                    return (byte[])Value;
                case JsonToken.StartArray:
                    return ReadArrayIntoByteArray();
            }

            throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
        }

        /// <MetaDataID>{bc06e124-2be9-41a3-8ac1-7caaa5ad3a16}</MetaDataID>
        internal byte[] ReadArrayIntoByteArray()
        {
            List<byte> buffer = new List<byte>();

            while (true)
            {
                if (!Read())
                {
                    SetToken(JsonToken.None);
                }

                if (ReadArrayElementIntoByteArrayReportDone(buffer))
                {
                    byte[] d = buffer.ToArray();
                    SetToken(JsonToken.Bytes, d, false);
                    return d;
                }
            }
        }

        /// <MetaDataID>{4f1d69c7-3ba8-46aa-b443-71f7751f1569}</MetaDataID>
        private bool ReadArrayElementIntoByteArrayReportDone(List<byte> buffer)
        {
            switch (TokenType)
            {
                case JsonToken.None:
                    throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
                case JsonToken.Integer:
                    buffer.Add(Convert.ToByte(Value, CultureInfo.InvariantCulture));
                    return false;
                case JsonToken.EndArray:
                    return true;
                case JsonToken.Comment:
                    return false;
                default:
                    throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
            }
        }

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="Double" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Double" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{ec162ba3-8cc9-4720-b0b6-9d41a0f031cd}</MetaDataID>
        public virtual double? ReadAsDouble()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Integer:
                case JsonToken.Float:
                    object v = Value;
                    if (v is double d)
                    {
                        return d;
                    }

#if HAVE_BIG_INTEGER
                    if (v is BigInteger value)
                    {
                        d = (double)value;
                    }
                    else
#endif
                    {
                        d = Convert.ToDouble(v, CultureInfo.InvariantCulture);
                    }

                    SetToken(JsonToken.Float, d, false);

                    return (double)d;
                case JsonToken.String:
                    return ReadDoubleString((string)Value);
            }

            throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
        }

        /// <MetaDataID>{cf0afcea-34c5-4714-bf8a-b1c3f39997d1}</MetaDataID>
        internal double? ReadDoubleString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SetToken(JsonToken.Null, null, false);
                return null;
            }

            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, Culture, out double d))
            {
                SetToken(JsonToken.Float, d, false);
                return d;
            }
            else
            {
                SetToken(JsonToken.String, s, false);
                throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
            }
        }

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="Boolean" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Boolean" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{ea67d28d-3c0e-4a53-b985-7517bb562bd4}</MetaDataID>
        public virtual bool? ReadAsBoolean()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Integer:
                case JsonToken.Float:
                    bool b;
#if HAVE_BIG_INTEGER
                    if (Value is BigInteger integer)
                    {
                        b = integer != 0;
                    }
                    else
#endif
                    {
                        b = Convert.ToBoolean(Value, CultureInfo.InvariantCulture);
                    }

                    SetToken(JsonToken.Boolean, b, false);
                    return b;
                case JsonToken.String:
                    return ReadBooleanString((string)Value);
                case JsonToken.Boolean:
                    return (bool)Value;
            }

            throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
        }

        /// <MetaDataID>{e2945524-e097-4fe5-ab98-2626151d24bb}</MetaDataID>
        internal bool? ReadBooleanString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SetToken(JsonToken.Null, null, false);
                return null;
            }

            if (bool.TryParse(s, out bool b))
            {
                SetToken(JsonToken.Boolean, b, false);
                return b;
            }
            else
            {
                SetToken(JsonToken.String, s, false);
                throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
            }
        }

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="Decimal" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Decimal" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{730cf788-d482-4784-bfa0-3e903b686f56}</MetaDataID>
        public virtual decimal? ReadAsDecimal()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Integer:
                case JsonToken.Float:
                    object v = Value;

                    if (v is decimal d)
                    {
                        return d;
                    }

#if HAVE_BIG_INTEGER
                    if (v is BigInteger value)
                    {
                        d = (decimal)value;
                    }
                    else
#endif
                    {
                        try
                        {
                            d = Convert.ToDecimal(v, CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            // handle error for large integer overflow exceptions
                            throw JsonReaderException.Create(this, "Could not convert to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, v), ex);
                        }
                    }

                    SetToken(JsonToken.Float, d, false);
                    return d;
                case JsonToken.String:
                    return ReadDecimalString((string)Value);
            }

            throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
        }

        /// <MetaDataID>{dfb6fd1a-5322-4cef-ba03-567a857164c9}</MetaDataID>
        internal decimal? ReadDecimalString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SetToken(JsonToken.Null, null, false);
                return null;
            }

            if (decimal.TryParse(s, NumberStyles.Number, Culture, out decimal d))
            {
                SetToken(JsonToken.Float, d, false);
                return d;
            }
            else if (ConvertUtils.DecimalTryParse(s.ToCharArray(), 0, s.Length, out d) == ParseResult.Success)
            {
                // This is to handle strings like "96.014e-05" that are not supported by traditional decimal.TryParse
                SetToken(JsonToken.Float, d, false);
                return d;
            }
            else
            {
                SetToken(JsonToken.String, s, false);
                throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
            }
        }

        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="DateTime" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="DateTime" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{4504ac83-f3df-462a-9874-f99702a3e28e}</MetaDataID>
        public virtual DateTime? ReadAsDateTime()
        {
            switch (GetContentToken())
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Date:
#if HAVE_DATE_TIME_OFFSET
                    if (Value is DateTimeOffset offset)
                    {
                        SetToken(JsonToken.Date, offset.DateTime, false);
                    }
#endif

                    return (DateTime)Value;
                case JsonToken.String:
                    string s = (string)Value;
                    return ReadDateTimeString(s);
            }

            throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
        }

        /// <MetaDataID>{9a325023-1ebf-4a35-a589-1220b8bd66ff}</MetaDataID>
        internal DateTime? ReadDateTimeString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SetToken(JsonToken.Null, null, false);
                return null;
            }

            if (DateTimeUtils.TryParseDateTime(s, DateTimeZoneHandling, _dateFormatString, Culture, out DateTime dt))
            {
                dt = DateTimeUtils.EnsureDateTime(dt, DateTimeZoneHandling);
                SetToken(JsonToken.Date, dt, false);
                return dt;
            }

            if (DateTime.TryParse(s, Culture, DateTimeStyles.RoundtripKind, out dt))
            {
                dt = DateTimeUtils.EnsureDateTime(dt, DateTimeZoneHandling);
                SetToken(JsonToken.Date, dt, false);
                return dt;
            }

            throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{e46caf8b-ee82-4b45-9d1f-6129e3082c4f}</MetaDataID>
        public virtual DateTimeOffset? ReadAsDateTimeOffset()
        {
            JsonToken t = GetContentToken();

            switch (t)
            {
                case JsonToken.None:
                case JsonToken.Null:
                case JsonToken.EndArray:
                    return null;
                case JsonToken.Date:
                    if (Value is DateTime time)
                    {
                        SetToken(JsonToken.Date, new DateTimeOffset(time), false);
                    }

                    return (DateTimeOffset)Value;
                case JsonToken.String:
                    string s = (string)Value;
                    return ReadDateTimeOffsetString(s);
                default:
                    throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, t));
            }
        }

        /// <MetaDataID>{f7ac77c5-8a95-4429-8930-79bfeb8b700e}</MetaDataID>
        internal DateTimeOffset? ReadDateTimeOffsetString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                SetToken(JsonToken.Null, null, false);
                return null;
            }

            if (DateTimeUtils.TryParseDateTimeOffset(s, _dateFormatString, Culture, out DateTimeOffset dt))
            {
                SetToken(JsonToken.Date, dt, false);
                return dt;
            }

            if (DateTimeOffset.TryParse(s, Culture, DateTimeStyles.RoundtripKind, out dt))
            {
                SetToken(JsonToken.Date, dt, false);
                return dt;
            }

            SetToken(JsonToken.String, s, false);
            throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
        }
#endif

        /// <MetaDataID>{c458c551-ec5e-4d0c-9685-8fa7c53dec69}</MetaDataID>
        internal void ReaderReadAndAssert()
        {
            if (!Read())
            {
                throw CreateUnexpectedEndException();
            }
        }

        /// <MetaDataID>{38c3b731-5109-4046-bda4-4d9e06bf10ac}</MetaDataID>
        internal JsonReaderException CreateUnexpectedEndException()
        {
            return JsonReaderException.Create(this, "Unexpected end when reading JSON.");
        }

        /// <MetaDataID>{c1568517-92cc-41a7-984b-34f360145aa9}</MetaDataID>
        internal void ReadIntoWrappedTypeObject()
        {
            ReaderReadAndAssert();
            if (Value != null && Value.ToString() == JsonTypeReflector.TypePropertyName)
            {
                ReaderReadAndAssert();
                if (Value != null && Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
                {
                    ReaderReadAndAssert();
                    if (Value.ToString() == JsonTypeReflector.ValuePropertyName)
                    {
                        return;
                    }
                }
            }

            throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
        }

        /// <summary>
        /// Skips the children of the current token.
        /// </summary>
        /// <MetaDataID>{bb7165f5-b3d0-4eb4-a4f1-0d147d3d8162}</MetaDataID>
        public void Skip()
        {
            if (TokenType == JsonToken.PropertyName)
            {
                Read();
            }

            if (JsonTokenUtils.IsStartToken(TokenType))
            {
                int depth = Depth;

                while (Read() && (depth < Depth))
                {
                }
            }
        }

        /// <summary>
        /// Sets the current token.
        /// </summary>
        /// <param name="newToken">The new token.</param>
        /// <MetaDataID>{1715011f-79d7-4c4e-b1ed-4fc9c2c90b92}</MetaDataID>
        protected void SetToken(JsonToken newToken)
        {
            SetToken(newToken, null, true);
        }

        /// <summary>
        /// Sets the current token and value.
        /// </summary>
        /// <param name="newToken">The new token.</param>
        /// <param name="value">The value.</param>
        /// <MetaDataID>{06765af8-806c-4869-8d4b-a74f457cc612}</MetaDataID>
        protected void SetToken(JsonToken newToken, object value)
        {
            SetToken(newToken, value, true);
        }

        /// <summary>
        /// Sets the current token and value.
        /// </summary>
        /// <param name="newToken">The new token.</param>
        /// <param name="value">The value.</param>
        /// <param name="updateIndex">A flag indicating whether the position index inside an array should be updated.</param>
        /// <MetaDataID>{3cbc5b10-0526-4527-9d93-78812f07525a}</MetaDataID>
        protected void SetToken(JsonToken newToken, object value, bool updateIndex)
        {
            _tokenType = newToken;
            _value = value;

            switch (newToken)
            {
                case JsonToken.StartObject:
                    _currentState = State.ObjectStart;
                    Push(JsonContainerType.Object);
                    break;
                case JsonToken.StartArray:
                    _currentState = State.ArrayStart;
                    Push(JsonContainerType.Array);
                    break;
                case JsonToken.StartConstructor:
                    _currentState = State.ConstructorStart;
                    Push(JsonContainerType.Constructor);
                    break;
                case JsonToken.EndObject:
                    ValidateEnd(JsonToken.EndObject);
                    break;
                case JsonToken.EndArray:
                    ValidateEnd(JsonToken.EndArray);
                    break;
                case JsonToken.EndConstructor:
                    ValidateEnd(JsonToken.EndConstructor);
                    break;
                case JsonToken.PropertyName:
                    _currentState = State.Property;

                    _currentPosition.PropertyName = (string)value;
                    break;
                case JsonToken.Undefined:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.String:
                case JsonToken.Raw:
                case JsonToken.Bytes:
                    SetPostValueState(updateIndex);
                    break;
            }
        }

        /// <MetaDataID>{2075472a-2e5a-43f6-9b35-8dc0a3b4776e}</MetaDataID>
        internal void SetPostValueState(bool updateIndex)
        {
            if (Peek() != JsonContainerType.None || SupportMultipleContent)
            {
                _currentState = State.PostValue;
            }
            else
            {
                SetFinished();
            }

            if (updateIndex)
            {
                UpdateScopeWithFinishedValue();
            }
        }

        /// <MetaDataID>{700b1c34-abbe-4a8b-bb96-0934d60fe94a}</MetaDataID>
        private void UpdateScopeWithFinishedValue()
        {
            if (_currentPosition.HasIndex)
            {
                _currentPosition.Position++;
            }
        }

        /// <MetaDataID>{50f3c580-86b9-48d1-8dc8-11367b1e4303}</MetaDataID>
        private void ValidateEnd(JsonToken endToken)
        {
            JsonContainerType currentObject = Pop();

            if (GetTypeForCloseToken(endToken) != currentObject)
            {
                throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, currentObject));
            }

            if (Peek() != JsonContainerType.None || SupportMultipleContent)
            {
                _currentState = State.PostValue;
            }
            else
            {
                SetFinished();
            }
        }

        /// <summary>
        /// Sets the state based on current token type.
        /// </summary>
        /// <MetaDataID>{cb9a59a9-54c7-4d05-9459-21e5a1381b2d}</MetaDataID>
        protected void SetStateBasedOnCurrent()
        {
            JsonContainerType currentObject = Peek();

            switch (currentObject)
            {
                case JsonContainerType.Object:
                    _currentState = State.Object;
                    break;
                case JsonContainerType.Array:
                    _currentState = State.Array;
                    break;
                case JsonContainerType.Constructor:
                    _currentState = State.Constructor;
                    break;
                case JsonContainerType.None:
                    SetFinished();
                    break;
                default:
                    throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, currentObject));
            }
        }

        /// <MetaDataID>{204fcb24-8e9c-40ec-a162-0bde80c304ff}</MetaDataID>
        private void SetFinished()
        {
            _currentState = SupportMultipleContent ? State.Start : State.Finished;
        }

        /// <MetaDataID>{920dd001-25ec-4d19-8e02-59065957d7bd}</MetaDataID>
        private JsonContainerType GetTypeForCloseToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return JsonContainerType.Object;
                case JsonToken.EndArray:
                    return JsonContainerType.Array;
                case JsonToken.EndConstructor:
                    return JsonContainerType.Constructor;
                default:
                    throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token));
            }
        }

        /// <MetaDataID>{9c098670-ae1d-418d-88b4-1219a9db5a45}</MetaDataID>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <MetaDataID>{f591aaba-fb75-42a8-9898-433f9d42d803}</MetaDataID>
        protected virtual void Dispose(bool disposing)
        {
            if (_currentState != State.Closed && disposing)
            {
                Close();
            }
        }

        /// <summary>
        /// Changes the reader's state to <see cref="JsonReader.State.Closed" />.
        /// If <see cref="JsonReader.CloseInput" /> is set to <c>true</c>, the source is also closed.
        /// </summary>
        /// <MetaDataID>{ce1c135f-27df-4b01-a26f-48d3f98a2ad3}</MetaDataID>
        public virtual void Close()
        {
            _currentState = State.Closed;
            _tokenType = JsonToken.None;
            _value = null;
        }

        /// <MetaDataID>{c77585ad-8196-4642-8fd9-b81f97406743}</MetaDataID>
        internal void ReadAndAssert()
        {
            if (!Read())
            {
                throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
            }
        }

        /// <MetaDataID>{8cc324c9-d3aa-45db-832b-178702bcf2e5}</MetaDataID>
        internal void ReadForTypeAndAssert(JsonContract contract, bool hasConverter)
        {
            if (!ReadForType(contract, hasConverter))
            {
                throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
            }
        }

        /// <MetaDataID>{bae499d6-8924-40e8-b623-15e876e5372f}</MetaDataID>
        internal bool ReadForType(JsonContract contract, bool hasConverter)
        {
            // don't read properties with converters as a specific value
            // the value might be a string which will then get converted which will error if read as date for example
            if (hasConverter)
            {
                return Read();
            }

            ReadType t = contract?.InternalReadType ?? ReadType.Read;

            switch (t)
            {
                case ReadType.Read:
                    return ReadAndMoveToContent();
                case ReadType.ReadAsInt32:
                    ReadAsInt32();
                    break;
                case ReadType.ReadAsInt64:
                    bool result = ReadAndMoveToContent();
                    if (TokenType == JsonToken.Undefined)
                    {
                        throw JsonReaderException.Create(this, "An undefined token is not a valid {0}.".FormatWith(CultureInfo.InvariantCulture, contract?.UnderlyingType ?? typeof(long)));
                    }
                    return result;
                case ReadType.ReadAsDecimal:
                    ReadAsDecimal();
                    break;
                case ReadType.ReadAsDouble:
                    ReadAsDouble();
                    break;
                case ReadType.ReadAsBytes:
                    ReadAsBytes();
                    break;
                case ReadType.ReadAsBoolean:
                    ReadAsBoolean();
                    break;
                case ReadType.ReadAsString:
                    ReadAsString();
                    break;
                case ReadType.ReadAsDateTime:
                    ReadAsDateTime();
                    break;
#if HAVE_DATE_TIME_OFFSET
                case ReadType.ReadAsDateTimeOffset:
                    ReadAsDateTimeOffset();
                    break;
#endif
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (TokenType != JsonToken.None);
        }

        /// <MetaDataID>{2f2d912d-1232-4d61-997e-83265e3948fd}</MetaDataID>
        internal bool ReadAndMoveToContent()
        {
            return Read() && MoveToContent();
        }

        /// <MetaDataID>{adf2e8ed-269e-40ed-9fca-56381783029f}</MetaDataID>
        internal bool MoveToContent()
        {
            JsonToken t = TokenType;
            while (t == JsonToken.None || t == JsonToken.Comment)
            {
                if (!Read())
                {
                    return false;
                }

                t = TokenType;
            }

            return true;
        }

        /// <MetaDataID>{40a77728-cb0a-4dd0-969a-933f2c620bba}</MetaDataID>
        private JsonToken GetContentToken()
        {
            JsonToken t;
            do
            {
                if (!Read())
                {
                    SetToken(JsonToken.None);
                    return JsonToken.None;
                }
                else
                {
                    t = TokenType;
                }
            } while (t == JsonToken.Comment);

            return t;
        }
    }
}
