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
using System.Globalization;
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
using System.Text;
using System.IO;
using System.Xml;
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Json
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way of generating JSON data.
    /// </summary>
    /// <MetaDataID>OOAdvantech.Json.JsonTextWriter</MetaDataID>
    public partial class JsonTextWriter : JsonWriter
    {
        /// <MetaDataID>{11fbef4b-61eb-43f5-ad2c-2a66899bf130}</MetaDataID>
        private const int IndentCharBufferSize = 12;
        /// <MetaDataID>{8198eb13-3a34-4a9b-b15f-31d457ff002d}</MetaDataID>
        private readonly TextWriter _writer;
        /// <MetaDataID>{6c4b0fa6-334e-40b8-b7c2-820f9fa6cb78}</MetaDataID>
        private Base64Encoder _base64Encoder;
        /// <MetaDataID>{0039808f-3aba-4e43-9d6e-f637b849ba81}</MetaDataID>
        private char _indentChar;
        /// <MetaDataID>{7293f098-0d39-4df6-a4f9-ed3f66817555}</MetaDataID>
        private int _indentation;
        /// <MetaDataID>{e15413d5-44b5-4e40-88b9-5a5c2c71b266}</MetaDataID>
        private char _quoteChar;
        /// <MetaDataID>{a803105c-1b81-4725-b577-5306cc2350f0}</MetaDataID>
        private bool _quoteName;
        /// <MetaDataID>{ff6280a6-13d4-463a-93b3-9d5f1a9837be}</MetaDataID>
        private bool[] _charEscapeFlags;
        /// <MetaDataID>{04133276-6af2-419b-b523-dd62797078cb}</MetaDataID>
        private char[] _writeBuffer;
        /// <MetaDataID>{5e91e324-bb24-4121-b9e1-475b5f8ac033}</MetaDataID>
        private IArrayPool<char> _arrayPool;
        /// <MetaDataID>{3790987c-c185-4b1b-9075-954d2d3a1dc6}</MetaDataID>
        private char[] _indentChars;

        /// <MetaDataID>{62f24b88-a7f9-4bc8-b9a6-1c30e425f042}</MetaDataID>
        private Base64Encoder Base64Encoder
        {
            get
            {
                if (_base64Encoder == null)
                {
                    _base64Encoder = new Base64Encoder(_writer);
                }

                return _base64Encoder;
            }
        }

        /// <summary>
        /// Gets or sets the writer's character array pool.
        /// </summary>
        /// <MetaDataID>{3a80ef82-2137-4957-8b3f-a1101d3ccf5c}</MetaDataID>
        public IArrayPool<char> ArrayPool
        {
            get => _arrayPool;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _arrayPool = value;
            }
        }

        /// <summary>
        /// Gets or sets how many <see cref="JsonTextWriter.IndentChar" />s to write for each level in the hierarchy when <see cref="JsonWriter.Formatting" /> is set to <see cref="Formatting.Indented" />.
        /// </summary>
        /// <MetaDataID>{a16b9070-3015-4da3-a2da-190a848211f2}</MetaDataID>
        public int Indentation
        {
            get => _indentation;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Indentation value must be greater than 0.");
                }

                _indentation = value;
            }
        }

        /// <summary>
        /// Gets or sets which character to use to quote attribute values.
        /// </summary>
        /// <MetaDataID>{c8149f84-e131-4906-acd9-a8208f23dc3c}</MetaDataID>
        public char QuoteChar
        {
            get => _quoteChar;
            set
            {
                if (value != '"' && value != '\'')
                {
                    throw new ArgumentException(@"Invalid JavaScript string quote character. Valid quote characters are ' and "".");
                }

                _quoteChar = value;
                UpdateCharEscapeFlags();
            }
        }

        /// <summary>
        /// Gets or sets which character to use for indenting when <see cref="JsonWriter.Formatting" /> is set to <see cref="Formatting.Indented" />.
        /// </summary>
        /// <MetaDataID>{ea2d2ca3-f204-492d-b640-3dffa28826c4}</MetaDataID>
        public char IndentChar
        {
            get => _indentChar;
            set
            {
                if (value != _indentChar)
                {
                    _indentChar = value;
                    _indentChars = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether object names will be surrounded with quotes.
        /// </summary>
        /// <MetaDataID>{95e18a7b-6e45-4c9f-808a-421694abef0a}</MetaDataID>
        public bool QuoteName
        {
            get => _quoteName;
            set => _quoteName = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTextWriter" /> class using the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter" /> to write to.</param>
        /// <MetaDataID>{3001d30b-21d1-4f7e-94ea-82a1b9a5fd94}</MetaDataID>
        public JsonTextWriter(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            _writer = textWriter;
            _quoteChar = '"';
            _quoteName = true;
            _indentChar = ' ';
            _indentation = 2;

            UpdateCharEscapeFlags();

#if HAVE_ASYNC
            _safeAsync = GetType() == typeof(JsonTextWriter);
#endif
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying <see cref="TextWriter" /> and also flushes the underlying <see cref="TextWriter" />.
        /// </summary>
        /// <MetaDataID>{b3d4be8e-d367-4e9f-942e-fe8ddbe8a13a}</MetaDataID>
        public override void Flush()
        {
            _writer.Flush();
        }

        /// <summary>
        /// Closes this writer.
        /// If <see cref="JsonWriter.CloseOutput" /> is set to <c>true</c>, the underlying <see cref="TextWriter" /> is also closed.
        /// If <see cref="JsonWriter.AutoCompleteOnClose" /> is set to <c>true</c>, the JSON is auto-completed.
        /// </summary>
        /// <MetaDataID>{b2c88529-a1b9-4539-bc06-9b3c8cdac6d5}</MetaDataID>
        public override void Close()
        {
            base.Close();

            CloseBufferAndWriter();
        }

        /// <MetaDataID>{af63b628-cf09-44c4-b987-f67efdb0f968}</MetaDataID>
        private void CloseBufferAndWriter()
        {
            if (_writeBuffer != null)
            {
                BufferUtils.ReturnBuffer(_arrayPool, _writeBuffer);
                _writeBuffer = null;
            }

            if (CloseOutput)
            {
#if HAVE_STREAM_READER_WRITER_CLOSE
                _writer?.Close();
#else
                _writer?.Dispose();
#endif
            }
        }

        /// <summary>
        /// Writes the beginning of a JSON object.
        /// </summary>
        /// <MetaDataID>{730f9645-7014-441d-a3ec-316e0683a203}</MetaDataID>
        public override void WriteStartObject()
        {
            InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);

            _writer.Write('{');
        }

        /// <summary>
        /// Writes the beginning of a JSON array.
        /// </summary>
        /// <MetaDataID>{500d247a-11c0-4115-8ab6-f18e8dce5be6}</MetaDataID>
        public override void WriteStartArray()
        {
            InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);

            _writer.Write('[');
        }

        /// <summary>
        /// Writes the start of a constructor with the given name.
        /// </summary>
        /// <param name="name">The name of the constructor.</param>
        /// <MetaDataID>{5be61c0f-c07f-41a3-89da-b84fdf1a3a59}</MetaDataID>
        public override void WriteStartConstructor(string name)
        {
            InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);

            _writer.Write("new ");
            _writer.Write(name);
            _writer.Write('(');
        }

        /// <summary>
        /// Writes the specified end token.
        /// </summary>
        /// <param name="token">The end token to write.</param>
        /// <MetaDataID>{553ecfbb-b215-4c7a-ab1d-15bb7fadcd1b}</MetaDataID>
        protected override void WriteEnd(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    _writer.Write('}');
                    break;
                case JsonToken.EndArray:
                    _writer.Write(']');
                    break;
                case JsonToken.EndConstructor:
                    _writer.Write(')');
                    break;
                default:
                    throw JsonWriterException.Create(this, "Invalid JsonToken: " + token, null);
            }
        }

        /// <summary>
        /// Writes the property name of a name/value pair on a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <MetaDataID>{e46c9ff4-5213-4d3e-ad5e-a39ce01b9110}</MetaDataID>
        public override void WritePropertyName(string name)
        {
            InternalWritePropertyName(name);

            WriteEscapedString(name, _quoteName);

            _writer.Write(':');
        }

        /// <summary>
        /// Writes the property name of a name/value pair on a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="escape">A flag to indicate whether the text should be escaped when it is written as a JSON property name.</param>
        /// <MetaDataID>{c566f89d-a683-4ab9-9d6c-b850c89b78de}</MetaDataID>
        public override void WritePropertyName(string name, bool escape)
        {
            InternalWritePropertyName(name);

            if (escape)
            {
                WriteEscapedString(name, _quoteName);
            }
            else
            {
                if (_quoteName)
                {
                    _writer.Write(_quoteChar);
                }

                _writer.Write(name);

                if (_quoteName)
                {
                    _writer.Write(_quoteChar);
                }
            }

            _writer.Write(':');
        }

        /// <MetaDataID>{9189a398-2bf1-47b0-a716-8f2d8e55d2a0}</MetaDataID>
        internal override void OnStringEscapeHandlingChanged()
        {
            UpdateCharEscapeFlags();
        }

        /// <MetaDataID>{667456ab-16b9-4dd3-8131-ba83c7474716}</MetaDataID>
        private void UpdateCharEscapeFlags()
        {
            _charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(StringEscapeHandling, _quoteChar);
        }

        /// <summary>
        /// Writes indent characters.
        /// </summary>
        /// <MetaDataID>{d093a969-41e2-4664-8412-e0b5f0fb8e26}</MetaDataID>
        protected override void WriteIndent()
        {
            // levels of indentation multiplied by the indent count
            int currentIndentCount = Top * _indentation;

            int newLineLen = SetIndentChars();

            _writer.Write(_indentChars, 0, newLineLen + Math.Min(currentIndentCount, IndentCharBufferSize));

            while ((currentIndentCount -= IndentCharBufferSize) > 0)
            {
                _writer.Write(_indentChars, newLineLen, Math.Min(currentIndentCount, IndentCharBufferSize));
            }
        }

        /// <MetaDataID>{9f974164-0bf2-4aad-ad1b-178fc4e2dab2}</MetaDataID>
        private int SetIndentChars()
        {
            // Set _indentChars to be a newline followed by IndentCharBufferSize indent characters.
            string writerNewLine = _writer.NewLine;
            int newLineLen = writerNewLine.Length;
            bool match = _indentChars != null && _indentChars.Length == IndentCharBufferSize + newLineLen;
            if (match)
            {
                for (int i = 0; i != newLineLen; ++i)
                {
                    if (writerNewLine[i] != _indentChars[i])
                    {
                        match = false;
                        break;
                    }
                }
            }

            if (!match)
            {
                // If we're here, either _indentChars hasn't been set yet, or _writer.NewLine
                // has been changed, or _indentChar has been changed.
                _indentChars = (writerNewLine + new string(_indentChar, IndentCharBufferSize)).ToCharArray();
            }

            return newLineLen;
        }

        /// <summary>
        /// Writes the JSON value delimiter.
        /// </summary>
        /// <MetaDataID>{1d0dada2-9fc1-4a3d-b14f-fd50f19f8892}</MetaDataID>
        protected override void WriteValueDelimiter()
        {
            _writer.Write(',');
        }

        /// <summary>
        /// Writes an indent space.
        /// </summary>
        /// <MetaDataID>{dcea3282-f102-41dc-9965-aee5f5ed66e8}</MetaDataID>
        protected override void WriteIndentSpace()
        {
            _writer.Write(' ');
        }

        /// <MetaDataID>{02a50548-bf8f-4711-900b-c2bbc7d98396}</MetaDataID>
        private void WriteValueInternal(string value, JsonToken token)
        {
            _writer.Write(value);
        }

        #region WriteValue methods
        /// <summary>
        /// Writes a <see cref="Object" /> value.
        /// An error will raised if the value cannot be written as a single JSON token.
        /// </summary>
        /// <param name="value">The <see cref="Object" /> value to write.</param>
        /// <MetaDataID>{c4018e98-92d0-4962-b802-718cfc6944c5}</MetaDataID>
        public override void WriteValue(object value)
        {
#if HAVE_BIG_INTEGER
            if (value is BigInteger i)
            {
                InternalWriteValue(JsonToken.Integer);
                WriteValueInternal(i.ToString(CultureInfo.InvariantCulture), JsonToken.String);
            }
            else
#endif
            {
                base.WriteValue(value);
            }
        }

        /// <summary>
        /// Writes a null value.
        /// </summary>
        /// <MetaDataID>{62659a91-6306-4163-93ab-8223319c4342}</MetaDataID>
        public override void WriteNull()
        {
            InternalWriteValue(JsonToken.Null);
            WriteValueInternal(JsonConvert.Null, JsonToken.Null);
        }

        /// <summary>
        /// Writes an undefined value.
        /// </summary>
        /// <MetaDataID>{f2472857-56b5-44d7-b252-8cec7689267e}</MetaDataID>
        public override void WriteUndefined()
        {
            InternalWriteValue(JsonToken.Undefined);
            WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
        }

        /// <summary>
        /// Writes raw JSON.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        /// <MetaDataID>{d6ff28a5-5556-434f-812f-281fba4fae22}</MetaDataID>
        public override void WriteRaw(string json)
        {
            InternalWriteRaw();

            _writer.Write(json);
        }

        /// <summary>
        /// Writes a <see cref="String" /> value.
        /// </summary>
        /// <param name="value">The <see cref="String" /> value to write.</param>
        /// <MetaDataID>{6d34370d-5968-42e1-ad2d-4eda0f1499db}</MetaDataID>
        public override void WriteValue(string value)
        {
            InternalWriteValue(JsonToken.String);

            if (value == null)
            {
                WriteValueInternal(JsonConvert.Null, JsonToken.Null);
            }
            else
            {
                WriteEscapedString(value, true);
            }
        }

        /// <MetaDataID>{24f470fc-99ed-4133-951c-6fe1746888d6}</MetaDataID>
        private void WriteEscapedString(string value, bool quote)
        {
            EnsureWriteBuffer();
            JavaScriptUtils.WriteEscapedJavaScriptString(_writer, value, _quoteChar, quote, _charEscapeFlags, StringEscapeHandling, _arrayPool, ref _writeBuffer);
        }

        /// <summary>
        /// Writes a <see cref="Int32" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Int32" /> value to write.</param>
        /// <MetaDataID>{ddbea3a5-302a-4dcb-92c7-c4509da08147}</MetaDataID>
        public override void WriteValue(int value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt32" /> value to write.</param>
        /// <MetaDataID>{58e0cddc-c4ab-4e78-9073-2abf9ced9f34}</MetaDataID>
        [CLSCompliant(false)]
        public override void WriteValue(uint value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="Int64" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Int64" /> value to write.</param>
        /// <MetaDataID>{96731f65-2cfe-4242-9fa1-eb99d57849ff}</MetaDataID>
        public override void WriteValue(long value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt64" /> value to write.</param>
        /// <MetaDataID>{fb6d9ed8-a828-4554-a864-8abf30ef7541}</MetaDataID>
        [CLSCompliant(false)]
        public override void WriteValue(ulong value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value, false);
        }

        /// <summary>
        /// Writes a <see cref="Single" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Single" /> value to write.</param>
        /// <MetaDataID>{1ad408a1-49e8-4856-9da9-7b052dd95193}</MetaDataID>
        public override void WriteValue(float value)
        {
            InternalWriteValue(JsonToken.Float);
            WriteValueInternal(JsonConvert.ToString(value, FloatFormatHandling, QuoteChar, false), JsonToken.Float);
        }

        /// <summary>
        /// Writes a <see cref="Nullable{T}" /> of <see cref="Single" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="Single" /> value to write.</param>
        /// <MetaDataID>{b5dffac5-d7f7-4c91-858b-3af3a72bb3b9}</MetaDataID>
        public override void WriteValue(float? value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else
            {
                InternalWriteValue(JsonToken.Float);
                WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), FloatFormatHandling, QuoteChar, true), JsonToken.Float);
            }
        }

        /// <summary>
        /// Writes a <see cref="Double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Double" /> value to write.</param>
        /// <MetaDataID>{5824e872-86e7-47af-a347-4bea0d5c3671}</MetaDataID>
        public override void WriteValue(double value)
        {
            InternalWriteValue(JsonToken.Float);
            WriteValueInternal(JsonConvert.ToString(value, FloatFormatHandling, QuoteChar, false), JsonToken.Float);
        }

        /// <summary>
        /// Writes a <see cref="Nullable{T}" /> of <see cref="Double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="Double" /> value to write.</param>
        /// <MetaDataID>{a5c95f4b-3fd5-4246-a0bb-5ea1a1371e56}</MetaDataID>
        public override void WriteValue(double? value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else
            {
                InternalWriteValue(JsonToken.Float);
                WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), FloatFormatHandling, QuoteChar, true), JsonToken.Float);
            }
        }

        /// <summary>
        /// Writes a <see cref="Boolean" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Boolean" /> value to write.</param>
        /// <MetaDataID>{fcacfe40-27af-49c4-baa6-9339cf94cb0b}</MetaDataID>
        public override void WriteValue(bool value)
        {
            InternalWriteValue(JsonToken.Boolean);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
        }

        /// <summary>
        /// Writes a <see cref="Int16" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Int16" /> value to write.</param>
        /// <MetaDataID>{9abc1d51-c2c5-4fe8-b64d-22bae4ecade6}</MetaDataID>
        public override void WriteValue(short value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt16" /> value to write.</param>
        /// <MetaDataID>{609e0738-af98-486e-9bf1-08b1605afef2}</MetaDataID>
        [CLSCompliant(false)]
        public override void WriteValue(ushort value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="Char" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Char" /> value to write.</param>
        /// <MetaDataID>{7dff7ed6-0de4-4296-af0f-5b86d17ac1e7}</MetaDataID>
        public override void WriteValue(char value)
        {
            InternalWriteValue(JsonToken.String);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
        }

        /// <summary>
        /// Writes a <see cref="Byte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Byte" /> value to write.</param>
        /// <MetaDataID>{23913c0d-daa5-4ed0-bd43-4c15635473f7}</MetaDataID>
        public override void WriteValue(byte value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="SByte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="SByte" /> value to write.</param>
        /// <MetaDataID>{5baf9470-ee86-48c8-aeed-52efc0b62b5c}</MetaDataID>
        [CLSCompliant(false)]
        public override void WriteValue(sbyte value)
        {
            InternalWriteValue(JsonToken.Integer);
            WriteIntegerValue(value);
        }

        /// <summary>
        /// Writes a <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Decimal" /> value to write.</param>
        /// <MetaDataID>{7fb4c198-c265-4a4a-bb07-81edc09afced}</MetaDataID>
        public override void WriteValue(decimal value)
        {
            InternalWriteValue(JsonToken.Float);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
        }

        /// <summary>
        /// Writes a <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime" /> value to write.</param>
        /// <MetaDataID>{ac91efa3-6b4a-4b20-8d18-b10757d614b2}</MetaDataID>
        public override void WriteValue(DateTime value)
        {
            InternalWriteValue(JsonToken.Date);
            value = DateTimeUtils.EnsureDateTime(value, DateTimeZoneHandling);

            if (string.IsNullOrEmpty(DateFormatString))
            {
                int length = WriteValueToBuffer(value);

                _writer.Write(_writeBuffer, 0, length);
            }
            else
            {
                _writer.Write(_quoteChar);
                _writer.Write(value.ToString(DateFormatString, Culture));
                _writer.Write(_quoteChar);
            }
        }

        /// <MetaDataID>{2d1250da-6283-4b75-87ef-8803bfcbf086}</MetaDataID>
        private int WriteValueToBuffer(DateTime value)
        {
            EnsureWriteBuffer();

            int pos = 0;
            _writeBuffer[pos++] = _quoteChar;
            pos = DateTimeUtils.WriteDateTimeString(_writeBuffer, pos, value, null, value.Kind, DateFormatHandling);
            _writeBuffer[pos++] = _quoteChar;
            return pos;
        }

        /// <summary>
        /// Writes a <see cref="Byte" />[] value.
        /// </summary>
        /// <param name="value">The <see cref="Byte" />[] value to write.</param>
        /// <MetaDataID>{0540b48c-60f3-4746-9206-c2aa0fa8451f}</MetaDataID>
        public override void WriteValue(byte[] value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else
            {
                InternalWriteValue(JsonToken.Bytes);
                _writer.Write(_quoteChar);
                Base64Encoder.Encode(value, 0, value.Length);
                Base64Encoder.Flush();
                _writer.Write(_quoteChar);
            }
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Writes a <see cref="DateTimeOffset" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset" /> value to write.</param>
        /// <MetaDataID>{146d7e0b-dea0-43a9-9b79-71a75b28e0bf}</MetaDataID>
        public override void WriteValue(DateTimeOffset value)
        {
            InternalWriteValue(JsonToken.Date);

            if (string.IsNullOrEmpty(DateFormatString))
            {
                int length = WriteValueToBuffer(value);

                _writer.Write(_writeBuffer, 0, length);
            }
            else
            {
                _writer.Write(_quoteChar);
                _writer.Write(value.ToString(DateFormatString, Culture));
                _writer.Write(_quoteChar);
            }
        }

        /// <MetaDataID>{a5fe60aa-21f7-46ef-b7e4-30e4559e6cee}</MetaDataID>
        private int WriteValueToBuffer(DateTimeOffset value)
        {
            EnsureWriteBuffer();

            int pos = 0;
            _writeBuffer[pos++] = _quoteChar;
            pos = DateTimeUtils.WriteDateTimeString(_writeBuffer, pos, (DateFormatHandling == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, value.Offset, DateTimeKind.Local, DateFormatHandling);
            _writeBuffer[pos++] = _quoteChar;
            return pos;
        }
#endif

        /// <summary>
        /// Writes a <see cref="Guid" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Guid" /> value to write.</param>
        /// <MetaDataID>{cf3c2281-2951-4a20-b36a-b846d979c8df}</MetaDataID>
        public override void WriteValue(Guid value)
        {
            InternalWriteValue(JsonToken.String);

            string text = null;

#if HAVE_CHAR_TO_STRING_WITH_CULTURE
            text = value.ToString("D", CultureInfo.InvariantCulture);
#else
            text = value.ToString("D");
#endif

            _writer.Write(_quoteChar);
            _writer.Write(text);
            _writer.Write(_quoteChar);
        }

        /// <summary>
        /// Writes a <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan" /> value to write.</param>
        /// <MetaDataID>{13c25bd0-dfad-4a94-8290-41d675cdda39}</MetaDataID>
        public override void WriteValue(TimeSpan value)
        {
            InternalWriteValue(JsonToken.String);

            string text;
#if !HAVE_TIME_SPAN_TO_STRING_WITH_CULTURE
            text = value.ToString();
#else
            text = value.ToString(null, CultureInfo.InvariantCulture);
#endif

            _writer.Write(_quoteChar);
            _writer.Write(text);
            _writer.Write(_quoteChar);
        }

        /// <summary>
        /// Writes a <see cref="Uri" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Uri" /> value to write.</param>
        /// <MetaDataID>{5ddf88e6-8db6-4df8-bee8-599e10eb81af}</MetaDataID>
        public override void WriteValue(Uri value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else
            {
                InternalWriteValue(JsonToken.String);
                WriteEscapedString(value.OriginalString, true);
            }
        }
        #endregion

        /// <summary>
        /// Writes a comment <c>/*...*/</c> containing the specified text. 
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        /// <MetaDataID>{62d68af3-c0c5-4b65-9fab-48e797ebedbe}</MetaDataID>
        public override void WriteComment(string text)
        {
            InternalWriteComment();

            _writer.Write("/*");
            _writer.Write(text);
            _writer.Write("*/");
        }

        /// <summary>
        /// Writes the given white space.
        /// </summary>
        /// <param name="ws">The string of white space characters.</param>
        /// <MetaDataID>{b66f097b-71ce-4680-8572-b2fc3b5cd85e}</MetaDataID>
        public override void WriteWhitespace(string ws)
        {
            InternalWriteWhitespace(ws);

            _writer.Write(ws);
        }

        /// <MetaDataID>{001d7d76-2d36-4c08-b11c-f27247d9b48f}</MetaDataID>
        private void EnsureWriteBuffer()
        {
            if (_writeBuffer == null)
            {
                // maximum buffer sized used when writing iso date
                _writeBuffer = BufferUtils.RentBuffer(_arrayPool, 35);
            }
        }

        /// <MetaDataID>{0f1749c7-86be-49f5-9650-1a1fda1cf2c9}</MetaDataID>
        private void WriteIntegerValue(long value)
        {
            if (value >= 0 && value <= 9)
            {
                _writer.Write((char)('0' + value));
            }
            else
            {
                bool negative = value < 0;
                WriteIntegerValue(negative ? (ulong)-value : (ulong)value, negative);
            }
        }

        /// <MetaDataID>{4a45605b-3f6d-4811-84a3-1fc6fa3b0bd8}</MetaDataID>
        private void WriteIntegerValue(ulong value, bool negative)
        {
            if (!negative & value <= 9)
            {
                _writer.Write((char)('0' + value));
            }
            else
            {
                int length = WriteNumberToBuffer(value, negative);
                _writer.Write(_writeBuffer, 0, length);
            }
        }

        /// <MetaDataID>{156a5164-ed1d-43c6-b044-037cf97401d9}</MetaDataID>
        private int WriteNumberToBuffer(ulong value, bool negative)
        {
            if (value <= uint.MaxValue)
            {
                // avoid the 64 bit division if possible
                return WriteNumberToBuffer((uint)value, negative);
            }

            EnsureWriteBuffer();

            int totalLength = MathUtils.IntLength(value);

            if (negative)
            {
                totalLength++;
                _writeBuffer[0] = '-';
            }

            int index = totalLength;

            do
            {
                ulong quotient = value / 10;
                ulong digit = value - (quotient * 10);
                _writeBuffer[--index] = (char)('0' + digit);
                value = quotient;
            } while (value != 0);

            return totalLength;
        }

        /// <MetaDataID>{c93986cf-9584-422f-b3e5-f33adfa567d5}</MetaDataID>
        private void WriteIntegerValue(int value)
        {
            if (value >= 0 && value <= 9)
            {
                _writer.Write((char)('0' + value));
            }
            else
            {
                bool negative = value < 0;
                WriteIntegerValue(negative ? (uint)-value : (uint)value, negative);
            }
        }

        /// <MetaDataID>{826cd7d2-2a3c-4d1b-98f7-0e4468fd4606}</MetaDataID>
        private void WriteIntegerValue(uint value, bool negative)
        {
            if (!negative & value <= 9)
            {
                _writer.Write((char)('0' + value));
            }
            else
            {
                int length = WriteNumberToBuffer(value, negative);
                _writer.Write(_writeBuffer, 0, length);
            }
        }

        /// <MetaDataID>{c7daeede-54b2-46d5-a118-ad5d1414480a}</MetaDataID>
        private int WriteNumberToBuffer(uint value, bool negative)
        {
            EnsureWriteBuffer();

            int totalLength = MathUtils.IntLength(value);

            if (negative)
            {
                totalLength++;
                _writeBuffer[0] = '-';
            }

            int index = totalLength;

            do
            {
                uint quotient = value / 10;
                uint digit = value - (quotient * 10);
                _writeBuffer[--index] = (char)('0' + digit);
                value = quotient;
            } while (value != 0);

            return totalLength;
        }
    }
}