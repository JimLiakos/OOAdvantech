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
using System.Runtime.CompilerServices;
using System.IO;
using System.Globalization;
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Json
{
    /// <MetaDataID>{0a597576-2365-49a2-b8af-b13f138fd7f4}</MetaDataID>
    internal enum ReadType
    {
        Read,
        ReadAsInt32,
        ReadAsInt64,
        ReadAsBytes,
        ReadAsString,
        ReadAsDecimal,
        ReadAsDateTime,
#if HAVE_DATE_TIME_OFFSET
        ReadAsDateTimeOffset,
#endif
        ReadAsDouble,
        ReadAsBoolean
    }

    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to JSON text data.
    /// </summary>
    /// <MetaDataID>OOAdvantech.Json.JsonTextReader</MetaDataID>
    public partial class JsonTextReader : JsonReader, IJsonLineInfo
    {
        /// <MetaDataID>{2c4f757f-f04e-4a59-b039-3bed2a55da9b}</MetaDataID>
        private const char UnicodeReplacementChar = '\uFFFD';
#if HAVE_BIG_INTEGER
        /// <MetaDataID>{150446da-5c87-4fd7-bfe9-fd2bccd3cdec}</MetaDataID>
        private const int MaximumJavascriptIntegerCharacterLength = 380;
#endif
#if DEBUG
        /// <MetaDataID>{6ff2197a-80fb-45a7-9e91-6fbf80fdef00}</MetaDataID>
        internal int LargeBufferLength { get; set; } = int.MaxValue / 2;
#else
        private const int LargeBufferLength = int.MaxValue / 2;
#endif

        /// <MetaDataID>{f1135d0b-8c9d-4f25-a311-5ac6834d4017}</MetaDataID>
        private readonly TextReader _reader;
        /// <MetaDataID>{c1703638-ce6d-49ba-97b0-6e5fd9357893}</MetaDataID>
        private char[] _chars;
        /// <MetaDataID>{2ecad13b-a6cc-4715-9742-52bb21bd3e2c}</MetaDataID>
        private int _charsUsed;
        /// <MetaDataID>{1f5c5189-c0cd-428a-8c20-4fc3ec0da36f}</MetaDataID>
        private int _charPos;
        /// <MetaDataID>{0b804d7c-fb67-45c9-99e9-a0111c3d995e}</MetaDataID>
        private int _lineStartPos;
        /// <MetaDataID>{a4e9c31b-2cb7-4cf1-937c-728e883d7966}</MetaDataID>
        private int _lineNumber;
        /// <MetaDataID>{4f4c6390-26e3-456f-8a2c-f4754938121e}</MetaDataID>
        private bool _isEndOfFile;
        /// <MetaDataID>{b48ce31c-39b8-4455-8362-6216b7d7eb4f}</MetaDataID>
        private StringBuffer _stringBuffer;
        /// <MetaDataID>{0c160323-abe6-4ffe-b8b4-16f0815d7707}</MetaDataID>
        private StringReference _stringReference;
        /// <MetaDataID>{10599be0-822c-4398-ba74-f3fadf00bd06}</MetaDataID>
        private IArrayPool<char> _arrayPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTextReader" /> class with the specified <see cref="TextReader" />.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader" /> containing the JSON data to read.</param>
        /// <MetaDataID>{012cde1a-2c82-4301-b5aa-959b50e53e9a}</MetaDataID>
        public JsonTextReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            _reader = reader;
            _lineNumber = 1;

#if HAVE_ASYNC
            _safeAsync = GetType() == typeof(JsonTextReader);
#endif
        }

#if DEBUG
        /// <MetaDataID>{0aeb7781-aedc-4b10-86bb-dd70a955278f}</MetaDataID>
        internal char[] CharBuffer
        {
            get => _chars;
            set => _chars = value;
        }

        /// <MetaDataID>{7dd02866-49e5-490b-b0a8-9cb246c9ab18}</MetaDataID>
        internal int CharPos => _charPos;
#endif

        /// <summary>
        /// Gets or sets the reader's property name table.
        /// </summary>
        /// <MetaDataID>{78f49bc8-6777-490b-904b-9beac684234c}</MetaDataID>
        public JsonNameTable PropertyNameTable { get; set; }

        /// <summary>
        /// Gets or sets the reader's character buffer pool.
        /// </summary>
        /// <MetaDataID>{bb1e155e-29fb-4ad1-8c0c-7ccf39b7b091}</MetaDataID>
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

        /// <MetaDataID>{c087885a-6cec-4acc-a707-f8296a085224}</MetaDataID>
        private void EnsureBufferNotEmpty()
        {
            if (_stringBuffer.IsEmpty)
            {
                _stringBuffer = new StringBuffer(_arrayPool, 1024);
            }
        }

        /// <MetaDataID>{522d8a98-00e9-403a-be5d-6365947e2cf1}</MetaDataID>
        private void SetNewLine(bool hasNextChar)
        {
            if (hasNextChar && _chars[_charPos] == StringUtils.LineFeed)
            {
                _charPos++;
            }

            OnNewLine(_charPos);
        }

        /// <MetaDataID>{d8b6c6aa-3f9a-4b7a-ace3-55c49c7a125c}</MetaDataID>
        private void OnNewLine(int pos)
        {
            _lineNumber++;
            _lineStartPos = pos;
        }

        /// <MetaDataID>{3d0c44c3-426e-4fd8-a6dc-abd92bcd3538}</MetaDataID>
        private void ParseString(char quote, ReadType readType)
        {
            _charPos++;

            ShiftBufferIfNeeded();
            ReadStringIntoBuffer(quote);
            ParseReadString(quote, readType);
        }

        /// <MetaDataID>{1f41e74f-0b1c-43fb-9477-1a29db481baf}</MetaDataID>
        private void ParseReadString(char quote, ReadType readType)
        {
            SetPostValueState(true);

            switch (readType)
            {
                case ReadType.ReadAsBytes:
                    Guid g;
                    byte[] data;
                    if (_stringReference.Length == 0)
                    {
                        data = CollectionUtils.ArrayEmpty<byte>();
                    }
                    else if (_stringReference.Length == 36 && ConvertUtils.TryConvertGuid(_stringReference.ToString(), out g))
                    {
                        data = g.ToByteArray();
                    }
                    else
                    {
                        data = Convert.FromBase64CharArray(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length);
                    }

                    SetToken(JsonToken.Bytes, data, false);
                    break;
                case ReadType.ReadAsString:
                    string text = _stringReference.ToString();

                    SetToken(JsonToken.String, text, false);
                    _quoteChar = quote;
                    break;
                case ReadType.ReadAsInt32:
                case ReadType.ReadAsDecimal:
                case ReadType.ReadAsBoolean:
                    // caller will convert result
                    break;
                default:
                    if (_dateParseHandling != DateParseHandling.None)
                    {
                        DateParseHandling dateParseHandling;
                        if (readType == ReadType.ReadAsDateTime)
                        {
                            dateParseHandling = DateParseHandling.DateTime;
                        }
#if HAVE_DATE_TIME_OFFSET
                        else if (readType == ReadType.ReadAsDateTimeOffset)
                        {
                            dateParseHandling = DateParseHandling.DateTimeOffset;
                        }
#endif
                        else
                        {
                            dateParseHandling = _dateParseHandling;
                        }

                        if (dateParseHandling == DateParseHandling.DateTime)
                        {
                            if (DateTimeUtils.TryParseDateTime(_stringReference, DateTimeZoneHandling, DateFormatString, Culture, out DateTime dt))
                            {
                                SetToken(JsonToken.Date, dt, false);
                                return;
                            }
                        }
#if HAVE_DATE_TIME_OFFSET
                        else
                        {
                            if (DateTimeUtils.TryParseDateTimeOffset(_stringReference, DateFormatString, Culture, out DateTimeOffset dt))
                            {
                                SetToken(JsonToken.Date, dt, false);
                                return;
                            }
                        }
#endif
                    }

                    SetToken(JsonToken.String, _stringReference.ToString(), false);
                    _quoteChar = quote;
                    break;
            }
        }

        /// <MetaDataID>{4162c835-689a-4c91-a87a-41118d9acd07}</MetaDataID>
        private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
        {
            const int charByteCount = 2;

            Buffer.BlockCopy(src, srcOffset * charByteCount, dst, dstOffset * charByteCount, count * charByteCount);
        }

        /// <MetaDataID>{73f978d6-1348-43ba-bd44-1f25778b6403}</MetaDataID>
        private void ShiftBufferIfNeeded()
        {
            // once in the last 10% of the buffer, or buffer is already very large then
            // shift the remaining content to the start to avoid unnecessarily increasing
            // the buffer size when reading numbers/strings
            int length = _chars.Length;
            if (length - _charPos <= length * 0.1 || length >= LargeBufferLength)
            {
                int count = _charsUsed - _charPos;
                if (count > 0)
                {
                    BlockCopyChars(_chars, _charPos, _chars, 0, count);
                }

                _lineStartPos -= _charPos;
                _charPos = 0;
                _charsUsed = count;
                _chars[_charsUsed] = '\0';
            }
        }

        /// <MetaDataID>{9fd2e5fa-41ae-4d1a-a8e6-6de3c5a75c16}</MetaDataID>
        private int ReadData(bool append)
        {
            return ReadData(append, 0);
        }

        /// <MetaDataID>{327c2007-821d-4a41-81a9-54b3169c251b}</MetaDataID>
        private void PrepareBufferForReadData(bool append, int charsRequired)
        {
            // char buffer is full
            if (_charsUsed + charsRequired >= _chars.Length - 1)
            {
                if (append)
                {
                    int doubledArrayLength = _chars.Length * 2;

                    // copy to new array either double the size of the current or big enough to fit required content
                    int newArrayLength = Math.Max(
                        doubledArrayLength < 0 ? int.MaxValue : doubledArrayLength, // handle overflow
                        _charsUsed + charsRequired + 1);

                    // increase the size of the buffer
                    char[] dst = BufferUtils.RentBuffer(_arrayPool, newArrayLength);

                    BlockCopyChars(_chars, 0, dst, 0, _chars.Length);

                    BufferUtils.ReturnBuffer(_arrayPool, _chars);

                    _chars = dst;
                }
                else
                {
                    int remainingCharCount = _charsUsed - _charPos;

                    if (remainingCharCount + charsRequired + 1 >= _chars.Length)
                    {
                        // the remaining count plus the required is bigger than the current buffer size
                        char[] dst = BufferUtils.RentBuffer(_arrayPool, remainingCharCount + charsRequired + 1);

                        if (remainingCharCount > 0)
                        {
                            BlockCopyChars(_chars, _charPos, dst, 0, remainingCharCount);
                        }

                        BufferUtils.ReturnBuffer(_arrayPool, _chars);

                        _chars = dst;
                    }
                    else
                    {
                        // copy any remaining data to the beginning of the buffer if needed and reset positions
                        if (remainingCharCount > 0)
                        {
                            BlockCopyChars(_chars, _charPos, _chars, 0, remainingCharCount);
                        }
                    }

                    _lineStartPos -= _charPos;
                    _charPos = 0;
                    _charsUsed = remainingCharCount;
                }
            }
        }

        /// <MetaDataID>{d559c43c-240f-415a-b19c-8311b57e4efa}</MetaDataID>
        private int ReadData(bool append, int charsRequired)
        {
            if (_isEndOfFile)
            {
                return 0;
            }

            PrepareBufferForReadData(append, charsRequired);

            int attemptCharReadCount = _chars.Length - _charsUsed - 1;

            int charsRead = _reader.Read(_chars, _charsUsed, attemptCharReadCount);

            _charsUsed += charsRead;

            if (charsRead == 0)
            {
                _isEndOfFile = true;
            }

            _chars[_charsUsed] = '\0';
            return charsRead;
        }

        /// <MetaDataID>{b2a8d9c5-cf16-4fb4-b44f-3b72fb5cd110}</MetaDataID>
        private bool EnsureChars(int relativePosition, bool append)
        {
            if (_charPos + relativePosition >= _charsUsed)
            {
                return ReadChars(relativePosition, append);
            }

            return true;
        }

        /// <MetaDataID>{79859489-5b23-4ce6-8da6-792c5e7bec0d}</MetaDataID>
        private bool ReadChars(int relativePosition, bool append)
        {
            if (_isEndOfFile)
            {
                return false;
            }

            int charsRequired = _charPos + relativePosition - _charsUsed + 1;

            int totalCharsRead = 0;

            // it is possible that the TextReader doesn't return all data at once
            // repeat read until the required text is returned or the reader is out of content
            do
            {
                int charsRead = ReadData(append, charsRequired - totalCharsRead);

                // no more content
                if (charsRead == 0)
                {
                    break;
                }

                totalCharsRead += charsRead;
            } while (totalCharsRead < charsRequired);

            if (totalCharsRead < charsRequired)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" />.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the next token was read successfully; <c>false</c> if there are no more tokens to read.
        /// </returns>
        /// <MetaDataID>{f6333019-6c1a-43e9-8b4a-5c0295a15c88}</MetaDataID>
        public override bool Read()
        {
            EnsureBuffer();

            while (true)
            {
                switch (_currentState)
                {
                    case State.Start:
                    case State.Property:
                    case State.Array:
                    case State.ArrayStart:
                    case State.Constructor:
                    case State.ConstructorStart:
                        return ParseValue();
                    case State.Object:
                    case State.ObjectStart:
                        return ParseObject();
                    case State.PostValue:
                        // returns true if it hits
                        // end of object or array
                        if (ParsePostValue(false))
                        {
                            return true;
                        }
                        break;
                    case State.Finished:
                        if (EnsureChars(0, false))
                        {
                            EatWhitespace();
                            if (_isEndOfFile)
                            {
                                SetToken(JsonToken.None);
                                return false;
                            }
                            if (_chars[_charPos] == '/')
                            {
                                ParseComment(true);
                                return true;
                            }

                            throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
                        }
                        SetToken(JsonToken.None);
                        return false;
                    default:
                        throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
                }
            }
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Nullable{T}" /> of <see cref="Int32" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Int32" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{50b086fc-5dea-4b32-b477-0d8893211923}</MetaDataID>
        public override int? ReadAsInt32()
        {
            return (int?)ReadNumberValue(ReadType.ReadAsInt32);
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Nullable{T}" /> of <see cref="DateTime" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="DateTime" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{ec3248bc-3127-4723-a72a-57f41b6136a0}</MetaDataID>
        public override DateTime? ReadAsDateTime()
        {
            return (DateTime?)ReadStringValue(ReadType.ReadAsDateTime);
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="String" />.
        /// </summary>
        /// <returns>A <see cref="String" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{c19a5c7d-c3b5-44ad-a784-2677d57336c4}</MetaDataID>
        public override string ReadAsString()
        {
            return (string)ReadStringValue(ReadType.ReadAsString);
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Byte" />[].
        /// </summary>
        /// <returns>A <see cref="Byte" />[] or <c>null</c> if the next JSON token is null. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{eb619ce9-6069-4e5c-a711-b00774d62b79}</MetaDataID>
        public override byte[] ReadAsBytes()
        {
            EnsureBuffer();
            bool isWrapped = false;

            switch (_currentState)
            {
                case State.PostValue:
                    if (ParsePostValue(true))
                    {
                        return null;
                    }
                    goto case State.Start;
                case State.Start:
                case State.Property:
                case State.Array:
                case State.ArrayStart:
                case State.Constructor:
                case State.ConstructorStart:
                    while (true)
                    {
                        char currentChar = _chars[_charPos];

                        switch (currentChar)
                        {
                            case '\0':
                                if (ReadNullChar())
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }
                                break;
                            case '"':
                            case '\'':
                                ParseString(currentChar, ReadType.ReadAsBytes);
                                byte[] data = (byte[])Value;
                                if (isWrapped)
                                {
                                    ReaderReadAndAssert();
                                    if (TokenType != JsonToken.EndObject)
                                    {
                                        throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
                                    }
                                    SetToken(JsonToken.Bytes, data, false);
                                }
                                return data;
                            case '{':
                                _charPos++;
                                SetToken(JsonToken.StartObject);
                                ReadIntoWrappedTypeObject();
                                isWrapped = true;
                                break;
                            case '[':
                                _charPos++;
                                SetToken(JsonToken.StartArray);
                                return ReadArrayIntoByteArray();
                            case 'n':
                                HandleNull();
                                return null;
                            case '/':
                                ParseComment(false);
                                break;
                            case ',':
                                ProcessValueComma();
                                break;
                            case ']':
                                _charPos++;
                                if (_currentState == State.Array || _currentState == State.ArrayStart || _currentState == State.PostValue)
                                {
                                    SetToken(JsonToken.EndArray);
                                    return null;
                                }
                                throw CreateUnexpectedCharacterException(currentChar);
                            case StringUtils.CarriageReturn:
                                ProcessCarriageReturn(false);
                                break;
                            case StringUtils.LineFeed:
                                ProcessLineFeed();
                                break;
                            case ' ':
                            case StringUtils.Tab:
                                // eat
                                _charPos++;
                                break;
                            default:
                                _charPos++;

                                if (!char.IsWhiteSpace(currentChar))
                                {
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }

                                // eat
                                break;
                        }
                    }
                case State.Finished:
                    ReadFinished();
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <MetaDataID>{3ea0c837-3c1b-4e37-b2ea-83e595d68c4b}</MetaDataID>
        private object ReadStringValue(ReadType readType)
        {
            EnsureBuffer();

            switch (_currentState)
            {
                case State.PostValue:
                    if (ParsePostValue(true))
                    {
                        return null;
                    }
                    goto case State.Start;
                case State.Start:
                case State.Property:
                case State.Array:
                case State.ArrayStart:
                case State.Constructor:
                case State.ConstructorStart:
                    while (true)
                    {
                        char currentChar = _chars[_charPos];

                        switch (currentChar)
                        {
                            case '\0':
                                if (ReadNullChar())
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }
                                break;
                            case '"':
                            case '\'':
                                ParseString(currentChar, readType);
                                return FinishReadQuotedStringValue(readType);
                            case '-':
                                if (EnsureChars(1, true) && _chars[_charPos + 1] == 'I')
                                {
                                    return ParseNumberNegativeInfinity(readType);
                                }
                                else
                                {
                                    ParseNumber(readType);
                                    return Value;
                                }
                            case '.':
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                if (readType != ReadType.ReadAsString)
                                {
                                    _charPos++;
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }
                                ParseNumber(ReadType.ReadAsString);
                                return Value;
                            case 't':
                            case 'f':
                                if (readType != ReadType.ReadAsString)
                                {
                                    _charPos++;
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }
                                string expected = currentChar == 't' ? JsonConvert.True : JsonConvert.False;
                                if (!MatchValueWithTrailingSeparator(expected))
                                {
                                    throw CreateUnexpectedCharacterException(_chars[_charPos]);
                                }
                                SetToken(JsonToken.String, expected);
                                return expected;
                            case 'I':
                                return ParseNumberPositiveInfinity(readType);
                            case 'N':
                                return ParseNumberNaN(readType);
                            case 'n':
                                HandleNull();
                                return null;
                            case '/':
                                ParseComment(false);
                                break;
                            case ',':
                                ProcessValueComma();
                                break;
                            case ']':
                                _charPos++;
                                if (_currentState == State.Array || _currentState == State.ArrayStart || _currentState == State.PostValue)
                                {
                                    SetToken(JsonToken.EndArray);
                                    return null;
                                }
                                throw CreateUnexpectedCharacterException(currentChar);
                            case StringUtils.CarriageReturn:
                                ProcessCarriageReturn(false);
                                break;
                            case StringUtils.LineFeed:
                                ProcessLineFeed();
                                break;
                            case ' ':
                            case StringUtils.Tab:
                                // eat
                                _charPos++;
                                break;
                            default:
                                _charPos++;

                                if (!char.IsWhiteSpace(currentChar))
                                {
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }

                                // eat
                                break;
                        }
                    }
                case State.Finished:
                    ReadFinished();
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <MetaDataID>{aee40593-bd99-4ee2-a8f1-8a7105142984}</MetaDataID>
        private object FinishReadQuotedStringValue(ReadType readType)
        {
            switch (readType)
            {
                case ReadType.ReadAsBytes:
                case ReadType.ReadAsString:
                    return Value;
                case ReadType.ReadAsDateTime:
                    if (Value is DateTime time)
                    {
                        return time;
                    }

                    return ReadDateTimeString((string)Value);
#if HAVE_DATE_TIME_OFFSET
                case ReadType.ReadAsDateTimeOffset:
                    if (Value is DateTimeOffset offset)
                    {
                        return offset;
                    }

                    return ReadDateTimeOffsetString((string)Value);
#endif
                default:
                    throw new ArgumentOutOfRangeException(nameof(readType));
            }
        }

        /// <MetaDataID>{f5b1426b-1cd6-4bf5-bc91-4339e8187eed}</MetaDataID>
        private JsonReaderException CreateUnexpectedCharacterException(char c)
        {
            return JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Nullable{T}" /> of <see cref="Boolean" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Boolean" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{72137413-845c-473e-b0fa-56481a3b17e7}</MetaDataID>
        public override bool? ReadAsBoolean()
        {
            EnsureBuffer();

            switch (_currentState)
            {
                case State.PostValue:
                    if (ParsePostValue(true))
                    {
                        return null;
                    }
                    goto case State.Start;
                case State.Start:
                case State.Property:
                case State.Array:
                case State.ArrayStart:
                case State.Constructor:
                case State.ConstructorStart:
                    while (true)
                    {
                        char currentChar = _chars[_charPos];

                        switch (currentChar)
                        {
                            case '\0':
                                if (ReadNullChar())
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }
                                break;
                            case '"':
                            case '\'':
                                ParseString(currentChar, ReadType.Read);
                                return ReadBooleanString(_stringReference.ToString());
                            case 'n':
                                HandleNull();
                                return null;
                            case '-':
                            case '.':
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                ParseNumber(ReadType.Read);
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
                            case 't':
                            case 'f':
                                bool isTrue = currentChar == 't';
                                string expected = isTrue ? JsonConvert.True : JsonConvert.False;

                                if (!MatchValueWithTrailingSeparator(expected))
                                {
                                    throw CreateUnexpectedCharacterException(_chars[_charPos]);
                                }
                                SetToken(JsonToken.Boolean, isTrue);
                                return isTrue;
                            case '/':
                                ParseComment(false);
                                break;
                            case ',':
                                ProcessValueComma();
                                break;
                            case ']':
                                _charPos++;
                                if (_currentState == State.Array || _currentState == State.ArrayStart || _currentState == State.PostValue)
                                {
                                    SetToken(JsonToken.EndArray);
                                    return null;
                                }
                                throw CreateUnexpectedCharacterException(currentChar);
                            case StringUtils.CarriageReturn:
                                ProcessCarriageReturn(false);
                                break;
                            case StringUtils.LineFeed:
                                ProcessLineFeed();
                                break;
                            case ' ':
                            case StringUtils.Tab:
                                // eat
                                _charPos++;
                                break;
                            default:
                                _charPos++;

                                if (!char.IsWhiteSpace(currentChar))
                                {
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }

                                // eat
                                break;
                        }
                    }
                case State.Finished:
                    ReadFinished();
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <MetaDataID>{abccd150-7740-4356-99aa-dd18adc3d2d4}</MetaDataID>
        private void ProcessValueComma()
        {
            _charPos++;

            if (_currentState != State.PostValue)
            {
                SetToken(JsonToken.Undefined);
                JsonReaderException ex = CreateUnexpectedCharacterException(',');
                // so the comma will be parsed again
                _charPos--;

                throw ex;
            }

            SetStateBasedOnCurrent();
        }

        /// <MetaDataID>{5d2194d2-c09f-4c01-83b8-5cc10a66b22d}</MetaDataID>
        private object ReadNumberValue(ReadType readType)
        {
            EnsureBuffer();

            switch (_currentState)
            {
                case State.PostValue:
                    if (ParsePostValue(true))
                    {
                        return null;
                    }
                    goto case State.Start;
                case State.Start:
                case State.Property:
                case State.Array:
                case State.ArrayStart:
                case State.Constructor:
                case State.ConstructorStart:
                    while (true)
                    {
                        char currentChar = _chars[_charPos];

                        switch (currentChar)
                        {
                            case '\0':
                                if (ReadNullChar())
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }
                                break;
                            case '"':
                            case '\'':
                                ParseString(currentChar, readType);
                                return FinishReadQuotedNumber(readType);
                            case 'n':
                                HandleNull();
                                return null;
                            case 'N':
                                return ParseNumberNaN(readType);
                            case 'I':
                                return ParseNumberPositiveInfinity(readType);
                            case '-':
                                if (EnsureChars(1, true) && _chars[_charPos + 1] == 'I')
                                {
                                    return ParseNumberNegativeInfinity(readType);
                                }
                                else
                                {
                                    ParseNumber(readType);
                                    return Value;
                                }
                            case '.':
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                ParseNumber(readType);
                                return Value;
                            case '/':
                                ParseComment(false);
                                break;
                            case ',':
                                ProcessValueComma();
                                break;
                            case ']':
                                _charPos++;
                                if (_currentState == State.Array || _currentState == State.ArrayStart || _currentState == State.PostValue)
                                {
                                    SetToken(JsonToken.EndArray);
                                    return null;
                                }
                                throw CreateUnexpectedCharacterException(currentChar);
                            case StringUtils.CarriageReturn:
                                ProcessCarriageReturn(false);
                                break;
                            case StringUtils.LineFeed:
                                ProcessLineFeed();
                                break;
                            case ' ':
                            case StringUtils.Tab:
                                // eat
                                _charPos++;
                                break;
                            default:
                                _charPos++;

                                if (!char.IsWhiteSpace(currentChar))
                                {
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }

                                // eat
                                break;
                        }
                    }
                case State.Finished:
                    ReadFinished();
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <MetaDataID>{acc5f8cb-cba2-406d-9daf-67fc53db72e7}</MetaDataID>
        private object FinishReadQuotedNumber(ReadType readType)
        {
            switch (readType)
            {
                case ReadType.ReadAsInt32:
                    return ReadInt32String(_stringReference.ToString());
                case ReadType.ReadAsDecimal:
                    return ReadDecimalString(_stringReference.ToString());
                case ReadType.ReadAsDouble:
                    return ReadDoubleString(_stringReference.ToString());
                default:
                    throw new ArgumentOutOfRangeException(nameof(readType));
            }
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{58c76141-0bd8-443a-8584-85548cbb6846}</MetaDataID>
        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            return (DateTimeOffset?)ReadStringValue(ReadType.ReadAsDateTimeOffset);
        }
#endif

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Nullable{T}" /> of <see cref="Decimal" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Decimal" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{7fe47ff6-3ff5-47fc-a463-1880b1940cee}</MetaDataID>
        public override decimal? ReadAsDecimal()
        {
            return (decimal?)ReadNumberValue(ReadType.ReadAsDecimal);
        }

        /// <summary>
        /// Reads the next JSON token from the underlying <see cref="TextReader" /> as a <see cref="Nullable{T}" /> of <see cref="Double" />.
        /// </summary>
        /// <returns>A <see cref="Nullable{T}" /> of <see cref="Double" />. This method will return <c>null</c> at the end of an array.</returns>
        /// <MetaDataID>{34c73acd-0d27-44b1-98cf-3c6e097f3f9d}</MetaDataID>
        public override double? ReadAsDouble()
        {
            return (double?)ReadNumberValue(ReadType.ReadAsDouble);
        }

        /// <MetaDataID>{429db97b-69e0-4922-a505-a1f9c46a80be}</MetaDataID>
        private void HandleNull()
        {
            if (EnsureChars(1, true))
            {
                char next = _chars[_charPos + 1];

                if (next == 'u')
                {
                    ParseNull();
                    return;
                }

                _charPos += 2;
                throw CreateUnexpectedCharacterException(_chars[_charPos - 1]);
            }

            _charPos = _charsUsed;
            throw CreateUnexpectedEndException();
        }

        /// <MetaDataID>{37f517f3-a722-44c5-93c2-91a41d80b628}</MetaDataID>
        private void ReadFinished()
        {
            if (EnsureChars(0, false))
            {
                EatWhitespace();
                if (_isEndOfFile)
                {
                    return;
                }
                if (_chars[_charPos] == '/')
                {
                    ParseComment(false);
                }
                else
                {
                    throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
                }
            }

            SetToken(JsonToken.None);
        }

        /// <MetaDataID>{f731fb24-dea2-4dc2-87cf-e552f58caa4e}</MetaDataID>
        private bool ReadNullChar()
        {
            if (_charsUsed == _charPos)
            {
                if (ReadData(false) == 0)
                {
                    _isEndOfFile = true;
                    return true;
                }
            }
            else
            {
                _charPos++;
            }

            return false;
        }

        /// <MetaDataID>{795f0b18-fac8-4455-b367-bf097d1db993}</MetaDataID>
        private void EnsureBuffer()
        {
            if (_chars == null)
            {
                _chars = BufferUtils.RentBuffer(_arrayPool, 1024);
                _chars[0] = '\0';
            }
        }

        /// <MetaDataID>{9d4366da-4254-4b6c-8174-5c881106bab1}</MetaDataID>
        private void ReadStringIntoBuffer(char quote)
        {
            int charPos = _charPos;
            int initialPosition = _charPos;
            int lastWritePosition = _charPos;
            _stringBuffer.Position = 0;

            while (true)
            {
                switch (_chars[charPos++])
                {
                    case '\0':
                        if (_charsUsed == charPos - 1)
                        {
                            charPos--;

                            if (ReadData(true) == 0)
                            {
                                _charPos = charPos;
                                throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
                            }
                        }
                        break;
                    case '\\':
                        _charPos = charPos;
                        if (!EnsureChars(0, true))
                        {
                            throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
                        }

                        // start of escape sequence
                        int escapeStartPos = charPos - 1;

                        char currentChar = _chars[charPos];
                        charPos++;

                        char writeChar;

                        switch (currentChar)
                        {
                            case 'b':
                                writeChar = '\b';
                                break;
                            case 't':
                                writeChar = '\t';
                                break;
                            case 'n':
                                writeChar = '\n';
                                break;
                            case 'f':
                                writeChar = '\f';
                                break;
                            case 'r':
                                writeChar = '\r';
                                break;
                            case '\\':
                                writeChar = '\\';
                                break;
                            case '"':
                            case '\'':
                            case '/':
                                writeChar = currentChar;
                                break;
                            case 'u':
                                _charPos = charPos;
                                writeChar = ParseUnicode();

                                if (StringUtils.IsLowSurrogate(writeChar))
                                {
                                    // low surrogate with no preceding high surrogate; this char is replaced
                                    writeChar = UnicodeReplacementChar;
                                }
                                else if (StringUtils.IsHighSurrogate(writeChar))
                                {
                                    bool anotherHighSurrogate;

                                    // loop for handling situations where there are multiple consecutive high surrogates
                                    do
                                    {
                                        anotherHighSurrogate = false;

                                        // potential start of a surrogate pair
                                        if (EnsureChars(2, true) && _chars[_charPos] == '\\' && _chars[_charPos + 1] == 'u')
                                        {
                                            char highSurrogate = writeChar;

                                            _charPos += 2;
                                            writeChar = ParseUnicode();

                                            if (StringUtils.IsLowSurrogate(writeChar))
                                            {
                                                // a valid surrogate pair!
                                            }
                                            else if (StringUtils.IsHighSurrogate(writeChar))
                                            {
                                                // another high surrogate; replace current and start check over
                                                highSurrogate = UnicodeReplacementChar;
                                                anotherHighSurrogate = true;
                                            }
                                            else
                                            {
                                                // high surrogate not followed by low surrogate; original char is replaced
                                                highSurrogate = UnicodeReplacementChar;
                                            }

                                            EnsureBufferNotEmpty();

                                            WriteCharToBuffer(highSurrogate, lastWritePosition, escapeStartPos);
                                            lastWritePosition = _charPos;
                                        }
                                        else
                                        {
                                            // there are not enough remaining chars for the low surrogate or is not follow by unicode sequence
                                            // replace high surrogate and continue on as usual
                                            writeChar = UnicodeReplacementChar;
                                        }
                                    } while (anotherHighSurrogate);
                                }

                                charPos = _charPos;
                                break;
                            default:
                                _charPos = charPos;
                                throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, @"\" + currentChar));
                        }

                        EnsureBufferNotEmpty();
                        WriteCharToBuffer(writeChar, lastWritePosition, escapeStartPos);

                        lastWritePosition = charPos;
                        break;
                    case StringUtils.CarriageReturn:
                        _charPos = charPos - 1;
                        ProcessCarriageReturn(true);
                        charPos = _charPos;
                        break;
                    case StringUtils.LineFeed:
                        _charPos = charPos - 1;
                        ProcessLineFeed();
                        charPos = _charPos;
                        break;
                    case '"':
                    case '\'':
                        if (_chars[charPos - 1] == quote)
                        {
                            FinishReadStringIntoBuffer(charPos - 1, initialPosition, lastWritePosition);
                            return;
                        }
                        break;
                }
            }
        }

        /// <MetaDataID>{123ea0c3-eab4-408c-9e98-927db056afde}</MetaDataID>
        private void FinishReadStringIntoBuffer(int charPos, int initialPosition, int lastWritePosition)
        {
            if (initialPosition == lastWritePosition)
            {
                _stringReference = new StringReference(_chars, initialPosition, charPos - initialPosition);
            }
            else
            {
                EnsureBufferNotEmpty();

                if (charPos > lastWritePosition)
                {
                    _stringBuffer.Append(_arrayPool, _chars, lastWritePosition, charPos - lastWritePosition);
                }

                _stringReference = new StringReference(_stringBuffer.InternalBuffer, 0, _stringBuffer.Position);
            }

            _charPos = charPos + 1;
        }

        /// <MetaDataID>{1addf50a-4a20-4cde-957c-484eed05bd54}</MetaDataID>
        private void WriteCharToBuffer(char writeChar, int lastWritePosition, int writeToPosition)
        {
            if (writeToPosition > lastWritePosition)
            {
                _stringBuffer.Append(_arrayPool, _chars, lastWritePosition, writeToPosition - lastWritePosition);
            }

            _stringBuffer.Append(_arrayPool, writeChar);
        }

        /// <MetaDataID>{874e4a67-056f-4566-a0d7-413dd8b1888b}</MetaDataID>
        private char ConvertUnicode(bool enoughChars)
        {
            if (enoughChars)
            {
                if (ConvertUtils.TryHexTextToInt(_chars, _charPos, _charPos + 4, out int value))
                {
                    char hexChar = Convert.ToChar(value);
                    _charPos += 4;
                    return hexChar;
                }
                else
                {
                    throw JsonReaderException.Create(this, @"Invalid Unicode escape sequence: \u{0}.".FormatWith(CultureInfo.InvariantCulture, new string(_chars, _charPos, 4)));
                }
            }
            else
            {
                throw JsonReaderException.Create(this, "Unexpected end while parsing Unicode escape sequence.");
            }
        }

        /// <MetaDataID>{4b5ebf72-f0e3-4ec8-aa6b-8a8d999077d0}</MetaDataID>
        private char ParseUnicode()
        {
            return ConvertUnicode(EnsureChars(4, true));
        }

        /// <MetaDataID>{974c2bdd-94b6-4b89-8ffc-926c9ac6f8b8}</MetaDataID>
        private void ReadNumberIntoBuffer()
        {
            int charPos = _charPos;

            while (true)
            {
                char currentChar = _chars[charPos];
                if (currentChar == '\0')
                {
                    _charPos = charPos;

                    if (_charsUsed == charPos)
                    {
                        if (ReadData(true) == 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else if (ReadNumberCharIntoBuffer(currentChar, charPos))
                {
                    return;
                }
                else
                {
                    charPos++;
                }
            }
        }

        /// <MetaDataID>{ad1856fd-4d6a-4ac5-b257-700ad81ecb00}</MetaDataID>
        private bool ReadNumberCharIntoBuffer(char currentChar, int charPos)
        {
            switch (currentChar)
            {
                case '-':
                case '+':
                case 'a':
                case 'A':
                case 'b':
                case 'B':
                case 'c':
                case 'C':
                case 'd':
                case 'D':
                case 'e':
                case 'E':
                case 'f':
                case 'F':
                case 'x':
                case 'X':
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return false;
                default:
                    _charPos = charPos;

                    if (char.IsWhiteSpace(currentChar) || currentChar == ',' || currentChar == '}' || currentChar == ']' || currentChar == ')' || currentChar == '/')
                    {
                        return true;
                    }

                    throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
            }
        }

        /// <MetaDataID>{d6399985-5df7-4a1f-a50e-777170e57121}</MetaDataID>
        private void ClearRecentString()
        {
            _stringBuffer.Position = 0;
            _stringReference = new StringReference();
        }

        /// <MetaDataID>{05eb9a2d-5130-4b0b-b1cb-6cd72481ce5d}</MetaDataID>
        private bool ParsePostValue(bool ignoreComments)
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (ReadData(false) == 0)
                            {
                                _currentState = State.Finished;
                                return false;
                            }
                        }
                        else
                        {
                            _charPos++;
                        }
                        break;
                    case '}':
                        _charPos++;
                        SetToken(JsonToken.EndObject);
                        return true;
                    case ']':
                        _charPos++;
                        SetToken(JsonToken.EndArray);
                        return true;
                    case ')':
                        _charPos++;
                        SetToken(JsonToken.EndConstructor);
                        return true;
                    case '/':
                        ParseComment(!ignoreComments);
                        if (!ignoreComments)
                        {
                            return true;
                        }
                        break;
                    case ',':
                        _charPos++;

                        // finished parsing
                        SetStateBasedOnCurrent();
                        return false;
                    case ' ':
                    case StringUtils.Tab:
                        // eat
                        _charPos++;
                        break;
                    case StringUtils.CarriageReturn:
                        ProcessCarriageReturn(false);
                        break;
                    case StringUtils.LineFeed:
                        ProcessLineFeed();
                        break;
                    default:
                        if (char.IsWhiteSpace(currentChar))
                        {
                            // eat
                            _charPos++;
                        }
                        else
                        {
                            // handle multiple content without comma delimiter
                            if (SupportMultipleContent && Depth == 0)
                            {
                                SetStateBasedOnCurrent();
                                return false;
                            }

                            throw JsonReaderException.Create(this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
                        }
                        break;
                }
            }
        }

        /// <MetaDataID>{425d9bcf-4e75-4654-b342-5d3b581d989f}</MetaDataID>
        private bool ParseObject()
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (ReadData(false) == 0)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            _charPos++;
                        }
                        break;
                    case '}':
                        SetToken(JsonToken.EndObject);
                        _charPos++;
                        return true;
                    case '/':
                        ParseComment(true);
                        return true;
                    case StringUtils.CarriageReturn:
                        ProcessCarriageReturn(false);
                        break;
                    case StringUtils.LineFeed:
                        ProcessLineFeed();
                        break;
                    case ' ':
                    case StringUtils.Tab:
                        // eat
                        _charPos++;
                        break;
                    default:
                        if (char.IsWhiteSpace(currentChar))
                        {
                            // eat
                            _charPos++;
                        }
                        else
                        {
                            return ParseProperty();
                        }
                        break;
                }
            }
        }

        /// <MetaDataID>{9408a4a8-d84e-4483-bcd5-795bde0ad147}</MetaDataID>
        private bool ParseProperty()
        {
            char firstChar = _chars[_charPos];
            char quoteChar;

            if (firstChar == '"' || firstChar == '\'')
            {
                _charPos++;
                quoteChar = firstChar;
                ShiftBufferIfNeeded();
                ReadStringIntoBuffer(quoteChar);
            }
            else if (ValidIdentifierChar(firstChar))
            {
                quoteChar = '\0';
                ShiftBufferIfNeeded();
                ParseUnquotedProperty();
            }
            else
            {
                throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
            }

            string propertyName;

            if (PropertyNameTable != null)
            {
                propertyName = PropertyNameTable.Get(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length);

                // no match in name table
                if (propertyName == null)
                {
                    propertyName = _stringReference.ToString();
                }
            }
            else
            {
                propertyName = _stringReference.ToString();
            }

            EatWhitespace();

            if (_chars[_charPos] != ':')
            {
                throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
            }

            _charPos++;

            SetToken(JsonToken.PropertyName, propertyName);
            _quoteChar = quoteChar;
            ClearRecentString();

            return true;
        }

        /// <MetaDataID>{9bc7c217-45fb-42dd-a927-fa9f351bae31}</MetaDataID>
        private bool ValidIdentifierChar(char value)
        {
            return (char.IsLetterOrDigit(value) || value == '_' || value == '$');
        }

        /// <MetaDataID>{b8a79d71-70bb-461e-9b8b-ec5ca7b9231a}</MetaDataID>
        private void ParseUnquotedProperty()
        {
            int initialPosition = _charPos;

            // parse unquoted property name until whitespace or colon
            while (true)
            {
                char currentChar = _chars[_charPos];
                if (currentChar == '\0')
                {
                    if (_charsUsed == _charPos)
                    {
                        if (ReadData(true) == 0)
                        {
                            throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
                        }

                        continue;
                    }

                    _stringReference = new StringReference(_chars, initialPosition, _charPos - initialPosition);
                    return;
                }

                if (ReadUnquotedPropertyReportIfDone(currentChar, initialPosition))
                {
                    return;
                }
            }
        }

        /// <MetaDataID>{4242856f-c9f8-4634-be2f-6bae6707a6e9}</MetaDataID>
        private bool ReadUnquotedPropertyReportIfDone(char currentChar, int initialPosition)
        {
            if (ValidIdentifierChar(currentChar))
            {
                _charPos++;
                return false;
            }

            if (char.IsWhiteSpace(currentChar) || currentChar == ':')
            {
                _stringReference = new StringReference(_chars, initialPosition, _charPos - initialPosition);
                return true;
            }

            throw JsonReaderException.Create(this, "Invalid JavaScript property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
        }

        /// <MetaDataID>{00054e94-b62b-4751-8123-ec635eeaa81c}</MetaDataID>
        private bool ParseValue()
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (ReadData(false) == 0)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            _charPos++;
                        }
                        break;
                    case '"':
                    case '\'':
                        ParseString(currentChar, ReadType.Read);
                        return true;
                    case 't':
                        ParseTrue();
                        return true;
                    case 'f':
                        ParseFalse();
                        return true;
                    case 'n':
                        if (EnsureChars(1, true))
                        {
                            char next = _chars[_charPos + 1];

                            if (next == 'u')
                            {
                                ParseNull();
                            }
                            else if (next == 'e')
                            {
                                ParseConstructor();
                            }
                            else
                            {
                                throw CreateUnexpectedCharacterException(_chars[_charPos]);
                            }
                        }
                        else
                        {
                            _charPos++;
                            throw CreateUnexpectedEndException();
                        }
                        return true;
                    case 'N':
                        ParseNumberNaN(ReadType.Read);
                        return true;
                    case 'I':
                        ParseNumberPositiveInfinity(ReadType.Read);
                        return true;
                    case '-':
                        if (EnsureChars(1, true) && _chars[_charPos + 1] == 'I')
                        {
                            ParseNumberNegativeInfinity(ReadType.Read);
                        }
                        else
                        {
                            ParseNumber(ReadType.Read);
                        }
                        return true;
                    case '/':
                        ParseComment(true);
                        return true;
                    case 'u':
                        ParseUndefined();
                        return true;
                    case '{':
                        _charPos++;
                        SetToken(JsonToken.StartObject);
                        return true;
                    case '[':
                        _charPos++;
                        SetToken(JsonToken.StartArray);
                        return true;
                    case ']':
                        _charPos++;
                        SetToken(JsonToken.EndArray);
                        return true;
                    case ',':
                        // don't increment position, the next call to read will handle comma
                        // this is done to handle multiple empty comma values
                        SetToken(JsonToken.Undefined);
                        return true;
                    case ')':
                        _charPos++;
                        SetToken(JsonToken.EndConstructor);
                        return true;
                    case StringUtils.CarriageReturn:
                        ProcessCarriageReturn(false);
                        break;
                    case StringUtils.LineFeed:
                        ProcessLineFeed();
                        break;
                    case ' ':
                    case StringUtils.Tab:
                        // eat
                        _charPos++;
                        break;
                    default:
                        if (char.IsWhiteSpace(currentChar))
                        {
                            // eat
                            _charPos++;
                            break;
                        }
                        if (char.IsNumber(currentChar) || currentChar == '-' || currentChar == '.')
                        {
                            ParseNumber(ReadType.Read);
                            return true;
                        }

                        throw CreateUnexpectedCharacterException(currentChar);
                }
            }
        }

        /// <MetaDataID>{e35669c6-f63f-4a8e-a935-dba359c3df73}</MetaDataID>
        private void ProcessLineFeed()
        {
            _charPos++;
            OnNewLine(_charPos);
        }

        /// <MetaDataID>{2e7b6040-5d78-4f14-a6a9-6e0ced26a8dc}</MetaDataID>
        private void ProcessCarriageReturn(bool append)
        {
            _charPos++;

            SetNewLine(EnsureChars(1, append));
        }

        /// <MetaDataID>{0c8d6883-ca93-462a-bf8d-9e3b397bae10}</MetaDataID>
        private void EatWhitespace()
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (ReadData(false) == 0)
                            {
                                return;
                            }
                        }
                        else
                        {
                            _charPos++;
                        }
                        break;
                    case StringUtils.CarriageReturn:
                        ProcessCarriageReturn(false);
                        break;
                    case StringUtils.LineFeed:
                        ProcessLineFeed();
                        break;
                    default:
                        if (currentChar == ' ' || char.IsWhiteSpace(currentChar))
                        {
                            _charPos++;
                        }
                        else
                        {
                            return;
                        }
                        break;
                }
            }
        }

        /// <MetaDataID>{6ed1695a-109c-4230-b3af-d22d3721580a}</MetaDataID>
        private void ParseConstructor()
        {
            if (MatchValueWithTrailingSeparator("new"))
            {
                EatWhitespace();

                int initialPosition = _charPos;
                int endPosition;

                while (true)
                {
                    char currentChar = _chars[_charPos];
                    if (currentChar == '\0')
                    {
                        if (_charsUsed == _charPos)
                        {
                            if (ReadData(true) == 0)
                            {
                                throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
                            }
                        }
                        else
                        {
                            endPosition = _charPos;
                            _charPos++;
                            break;
                        }
                    }
                    else if (char.IsLetterOrDigit(currentChar))
                    {
                        _charPos++;
                    }
                    else if (currentChar == StringUtils.CarriageReturn)
                    {
                        endPosition = _charPos;
                        ProcessCarriageReturn(true);
                        break;
                    }
                    else if (currentChar == StringUtils.LineFeed)
                    {
                        endPosition = _charPos;
                        ProcessLineFeed();
                        break;
                    }
                    else if (char.IsWhiteSpace(currentChar))
                    {
                        endPosition = _charPos;
                        _charPos++;
                        break;
                    }
                    else if (currentChar == '(')
                    {
                        endPosition = _charPos;
                        break;
                    }
                    else
                    {
                        throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
                    }
                }

                _stringReference = new StringReference(_chars, initialPosition, endPosition - initialPosition);
                string constructorName = _stringReference.ToString();

                EatWhitespace();

                if (_chars[_charPos] != '(')
                {
                    throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
                }

                _charPos++;

                ClearRecentString();

                SetToken(JsonToken.StartConstructor, constructorName);
            }
            else
            {
                throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
            }
        }

        /// <MetaDataID>{3d064c72-6efa-4d4e-bd38-b2dfa7ee3995}</MetaDataID>
        private void ParseNumber(ReadType readType)
        {
            ShiftBufferIfNeeded();

            char firstChar = _chars[_charPos];
            int initialPosition = _charPos;

            ReadNumberIntoBuffer();

            ParseReadNumber(readType, firstChar, initialPosition);
        }

        /// <MetaDataID>{66ef3a09-85c2-4cd1-8c5e-63e3172d1f92}</MetaDataID>
        private void ParseReadNumber(ReadType readType, char firstChar, int initialPosition)
        {
            // set state to PostValue now so that if there is an error parsing the number then the reader can continue
            SetPostValueState(true);

            _stringReference = new StringReference(_chars, initialPosition, _charPos - initialPosition);

            object numberValue;
            JsonToken numberType;

            bool singleDigit = (char.IsDigit(firstChar) && _stringReference.Length == 1);
            bool nonBase10 = (firstChar == '0' && _stringReference.Length > 1 && _stringReference.Chars[_stringReference.StartIndex + 1] != '.' && _stringReference.Chars[_stringReference.StartIndex + 1] != 'e' && _stringReference.Chars[_stringReference.StartIndex + 1] != 'E');

            switch (readType)
            {
                case ReadType.ReadAsString:
                    {
                        string number = _stringReference.ToString();

                        // validate that the string is a valid number
                        if (nonBase10)
                        {
                            try
                            {
                                if (number.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                                {
                                    Convert.ToInt64(number, 16);
                                }
                                else
                                {
                                    Convert.ToInt64(number, 8);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, number), ex);
                            }
                        }
                        else
                        {
                            if (!double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                            }
                        }

                        numberType = JsonToken.String;
                        numberValue = number;
                    }
                    break;
                case ReadType.ReadAsInt32:
                    {
                        if (singleDigit)
                        {
                            // digit char values start at 48
                            numberValue = firstChar - 48;
                        }
                        else if (nonBase10)
                        {
                            string number = _stringReference.ToString();

                            try
                            {
                                int integer = number.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(number, 16) : Convert.ToInt32(number, 8);

                                numberValue = integer;
                            }
                            catch (Exception ex)
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, number), ex);
                            }
                        }
                        else
                        {
                            ParseResult parseResult = ConvertUtils.Int32TryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out int value);
                            if (parseResult == ParseResult.Success)
                            {
                                numberValue = value;
                            }
                            else if (parseResult == ParseResult.Overflow)
                            {
                                throw ThrowReaderError("JSON integer {0} is too large or small for an Int32.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                            }
                            else
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                            }
                        }

                        numberType = JsonToken.Integer;
                    }
                    break;
                case ReadType.ReadAsDecimal:
                    {
                        if (singleDigit)
                        {
                            // digit char values start at 48
                            numberValue = (decimal)firstChar - 48;
                        }
                        else if (nonBase10)
                        {
                            string number = _stringReference.ToString();

                            try
                            {
                                // decimal.Parse doesn't support parsing hexadecimal values
                                long integer = number.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(number, 16) : Convert.ToInt64(number, 8);

                                numberValue = Convert.ToDecimal(integer);
                            }
                            catch (Exception ex)
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, number), ex);
                            }
                        }
                        else
                        {
                            ParseResult parseResult = ConvertUtils.DecimalTryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out decimal value);
                            if (parseResult == ParseResult.Success)
                            {
                                numberValue = value;
                            }
                            else
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                            }
                        }

                        numberType = JsonToken.Float;
                    }
                    break;
                case ReadType.ReadAsDouble:
                    {
                        if (singleDigit)
                        {
                            // digit char values start at 48
                            numberValue = (double)firstChar - 48;
                        }
                        else if (nonBase10)
                        {
                            string number = _stringReference.ToString();

                            try
                            {
                                // double.Parse doesn't support parsing hexadecimal values
                                long integer = number.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(number, 16) : Convert.ToInt64(number, 8);

                                numberValue = Convert.ToDouble(integer);
                            }
                            catch (Exception ex)
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith(CultureInfo.InvariantCulture, number), ex);
                            }
                        }
                        else
                        {
                            string number = _stringReference.ToString();

                            if (double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                            {
                                numberValue = value;
                            }
                            else
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                            }
                        }

                        numberType = JsonToken.Float;
                    }
                    break;
                case ReadType.Read:
                case ReadType.ReadAsInt64:
                    {
                        if (singleDigit)
                        {
                            // digit char values start at 48
                            numberValue = (long)firstChar - 48;
                            numberType = JsonToken.Integer;
                        }
                        else if (nonBase10)
                        {
                            string number = _stringReference.ToString();

                            try
                            {
                                numberValue = number.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(number, 16) : Convert.ToInt64(number, 8);
                            }
                            catch (Exception ex)
                            {
                                throw ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, number), ex);
                            }

                            numberType = JsonToken.Integer;
                        }
                        else
                        {
                            ParseResult parseResult = ConvertUtils.Int64TryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out long value);
                            if (parseResult == ParseResult.Success)
                            {
                                numberValue = value;
                                numberType = JsonToken.Integer;
                            }
                            else if (parseResult == ParseResult.Overflow)
                            {
#if HAVE_BIG_INTEGER
                                string number = _stringReference.ToString();

                                if (number.Length > MaximumJavascriptIntegerCharacterLength)
                                {
                                    throw ThrowReaderError("JSON integer {0} is too large to parse.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                                }

                                numberValue = BigIntegerParse(number, CultureInfo.InvariantCulture);
                                numberType = JsonToken.Integer;
#else
                                throw ThrowReaderError("JSON integer {0} is too large or small for an Int64.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
#endif
                            }
                            else
                            {
                                if (_floatParseHandling == FloatParseHandling.Decimal)
                                {
                                    parseResult = ConvertUtils.DecimalTryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out decimal d);
                                    if (parseResult == ParseResult.Success)
                                    {
                                        numberValue = d;
                                    }
                                    else
                                    {
                                        throw ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                                    }
                                }
                                else
                                {
                                    string number = _stringReference.ToString();

                                    if (double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                                    {
                                        numberValue = d;
                                    }
                                    else
                                    {
                                        throw ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
                                    }
                                }

                                numberType = JsonToken.Float;
                            }
                        }
                    }
                    break;
                default:
                    throw JsonReaderException.Create(this, "Cannot read number value as type.");
            }

            ClearRecentString();

            // index has already been updated
            SetToken(numberType, numberValue, false);
        }

        /// <MetaDataID>{a4a6a3c8-8f99-4284-811a-bea38fec6197}</MetaDataID>
        private JsonReaderException ThrowReaderError(string message, Exception ex = null)
        {
            SetToken(JsonToken.Undefined, null, false);
            return JsonReaderException.Create(this, message, ex);
        }

#if HAVE_BIG_INTEGER
        // By using the BigInteger type in a separate method,
        // the runtime can execute the ParseNumber even if 
        // the System.Numerics.BigInteger.Parse method is
        // missing, which happens in some versions of Mono
        /// <MetaDataID>{5a6c3149-1c31-41ca-b08c-33937eb3c1d2}</MetaDataID>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static object BigIntegerParse(string number, CultureInfo culture)
        {
            return System.Numerics.BigInteger.Parse(number, culture);
        }
#endif

        /// <MetaDataID>{2b6e11fd-1476-4f9a-8a61-9ae46fe6ec53}</MetaDataID>
        private void ParseComment(bool setToken)
        {
            // should have already parsed / character before reaching this method
            _charPos++;

            if (!EnsureChars(1, false))
            {
                throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
            }

            bool singlelineComment;

            if (_chars[_charPos] == '*')
            {
                singlelineComment = false;
            }
            else if (_chars[_charPos] == '/')
            {
                singlelineComment = true;
            }
            else
            {
                throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
            }

            _charPos++;

            int initialPosition = _charPos;

            while (true)
            {
                switch (_chars[_charPos])
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (ReadData(true) == 0)
                            {
                                if (!singlelineComment)
                                {
                                    throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
                                }

                                EndComment(setToken, initialPosition, _charPos);
                                return;
                            }
                        }
                        else
                        {
                            _charPos++;
                        }
                        break;
                    case '*':
                        _charPos++;

                        if (!singlelineComment)
                        {
                            if (EnsureChars(0, true))
                            {
                                if (_chars[_charPos] == '/')
                                {
                                    EndComment(setToken, initialPosition, _charPos - 1);

                                    _charPos++;
                                    return;
                                }
                            }
                        }
                        break;
                    case StringUtils.CarriageReturn:
                        if (singlelineComment)
                        {
                            EndComment(setToken, initialPosition, _charPos);
                            return;
                        }
                        ProcessCarriageReturn(true);
                        break;
                    case StringUtils.LineFeed:
                        if (singlelineComment)
                        {
                            EndComment(setToken, initialPosition, _charPos);
                            return;
                        }
                        ProcessLineFeed();
                        break;
                    default:
                        _charPos++;
                        break;
                }
            }
        }

        /// <MetaDataID>{c1d3b333-8b1e-4dc2-8e0f-be605e6f8aec}</MetaDataID>
        private void EndComment(bool setToken, int initialPosition, int endPosition)
        {
            if (setToken)
            {
                SetToken(JsonToken.Comment, new string(_chars, initialPosition, endPosition - initialPosition));
            }
        }

        /// <MetaDataID>{efae2145-6db0-4054-ae90-d1d3f078a64f}</MetaDataID>
        private bool MatchValue(string value)
        {
            return MatchValue(EnsureChars(value.Length - 1, true), value);
        }

        /// <MetaDataID>{fa004364-9803-4bca-89cf-460af20d75b3}</MetaDataID>
        private bool MatchValue(bool enoughChars, string value)
        {
            if (!enoughChars)
            {
                _charPos = _charsUsed;
                throw CreateUnexpectedEndException();
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (_chars[_charPos + i] != value[i])
                {
                    _charPos += i;
                    return false;
                }
            }

            _charPos += value.Length;

            return true;
        }

        /// <MetaDataID>{4779f06e-0969-4db3-bba2-80c662ea6fdd}</MetaDataID>
        private bool MatchValueWithTrailingSeparator(string value)
        {
            // will match value and then move to the next character, checking that it is a separator character
            bool match = MatchValue(value);

            if (!match)
            {
                return false;
            }

            if (!EnsureChars(0, false))
            {
                return true;
            }

            return IsSeparator(_chars[_charPos]) || _chars[_charPos] == '\0';
        }

        /// <MetaDataID>{7dcfaaec-0da3-4c16-8b84-a586ae49dec9}</MetaDataID>
        private bool IsSeparator(char c)
        {
            switch (c)
            {
                case '}':
                case ']':
                case ',':
                    return true;
                case '/':
                    // check next character to see if start of a comment
                    if (!EnsureChars(1, false))
                    {
                        return false;
                    }

                    char nextChart = _chars[_charPos + 1];

                    return (nextChart == '*' || nextChart == '/');
                case ')':
                    if (CurrentState == State.Constructor || CurrentState == State.ConstructorStart)
                    {
                        return true;
                    }
                    break;
                case ' ':
                case StringUtils.Tab:
                case StringUtils.LineFeed:
                case StringUtils.CarriageReturn:
                    return true;
                default:
                    if (char.IsWhiteSpace(c))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <MetaDataID>{e570a00c-0dfe-4099-b85c-53a8f3429098}</MetaDataID>
        private void ParseTrue()
        {
            // check characters equal 'true'
            // and that it is followed by either a separator character
            // or the text ends
            if (MatchValueWithTrailingSeparator(JsonConvert.True))
            {
                SetToken(JsonToken.Boolean, true);
            }
            else
            {
                throw JsonReaderException.Create(this, "Error parsing boolean value.");
            }
        }

        /// <MetaDataID>{3050b39b-12f2-4020-8f73-9501baa8889a}</MetaDataID>
        private void ParseNull()
        {
            if (MatchValueWithTrailingSeparator(JsonConvert.Null))
            {
                SetToken(JsonToken.Null);
            }
            else
            {
                throw JsonReaderException.Create(this, "Error parsing null value.");
            }
        }

        /// <MetaDataID>{141e4fca-6bb4-4cd2-816c-d9c928587bee}</MetaDataID>
        private void ParseUndefined()
        {
            if (MatchValueWithTrailingSeparator(JsonConvert.Undefined))
            {
                SetToken(JsonToken.Undefined);
            }
            else
            {
                throw JsonReaderException.Create(this, "Error parsing undefined value.");
            }
        }

        /// <MetaDataID>{32d9f8d8-62b0-40ed-b1a4-7a5107a2048a}</MetaDataID>
        private void ParseFalse()
        {
            if (MatchValueWithTrailingSeparator(JsonConvert.False))
            {
                SetToken(JsonToken.Boolean, false);
            }
            else
            {
                throw JsonReaderException.Create(this, "Error parsing boolean value.");
            }
        }

        /// <MetaDataID>{7c58b386-2e64-43c0-80fe-78e8eab4fa10}</MetaDataID>
        private object ParseNumberNegativeInfinity(ReadType readType)
        {
            return ParseNumberNegativeInfinity(readType, MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity));
        }

        /// <MetaDataID>{ca847fca-a80f-4b75-b49b-064c29ad3bda}</MetaDataID>
        private object ParseNumberNegativeInfinity(ReadType readType, bool matched)
        {
            if (matched)
            {
                switch (readType)
                {
                    case ReadType.Read:
                    case ReadType.ReadAsDouble:
                        if (_floatParseHandling == FloatParseHandling.Double)
                        {
                            SetToken(JsonToken.Float, double.NegativeInfinity);
                            return double.NegativeInfinity;
                        }
                        break;
                    case ReadType.ReadAsString:
                        SetToken(JsonToken.String, JsonConvert.NegativeInfinity);
                        return JsonConvert.NegativeInfinity;
                }

                throw JsonReaderException.Create(this, "Cannot read -Infinity value.");
            }

            throw JsonReaderException.Create(this, "Error parsing -Infinity value.");
        }

        /// <MetaDataID>{9b500ab7-21f8-4b89-a966-ae0bd990c800}</MetaDataID>
        private object ParseNumberPositiveInfinity(ReadType readType)
        {
            return ParseNumberPositiveInfinity(readType, MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity));
        }
        /// <MetaDataID>{9a5a2078-bbdc-4c15-bb29-bc169f061ae0}</MetaDataID>
        private object ParseNumberPositiveInfinity(ReadType readType, bool matched)
        {
            if (matched)
            {
                switch (readType)
                {
                    case ReadType.Read:
                    case ReadType.ReadAsDouble:
                        if (_floatParseHandling == FloatParseHandling.Double)
                        {
                            SetToken(JsonToken.Float, double.PositiveInfinity);
                            return double.PositiveInfinity;
                        }
                        break;
                    case ReadType.ReadAsString:
                        SetToken(JsonToken.String, JsonConvert.PositiveInfinity);
                        return JsonConvert.PositiveInfinity;
                }

                throw JsonReaderException.Create(this, "Cannot read Infinity value.");
            }

            throw JsonReaderException.Create(this, "Error parsing Infinity value.");
        }

        /// <MetaDataID>{3f1d81bf-b747-43b2-92bb-427a3ea5e341}</MetaDataID>
        private object ParseNumberNaN(ReadType readType)
        {
            return ParseNumberNaN(readType, MatchValueWithTrailingSeparator(JsonConvert.NaN));
        }

        /// <MetaDataID>{fe592326-d804-42e2-abea-157e36b8eca7}</MetaDataID>
        private object ParseNumberNaN(ReadType readType, bool matched)
        {
            if (matched)
            {
                switch (readType)
                {
                    case ReadType.Read:
                    case ReadType.ReadAsDouble:
                        if (_floatParseHandling == FloatParseHandling.Double)
                        {
                            SetToken(JsonToken.Float, double.NaN);
                            return double.NaN;
                        }
                        break;
                    case ReadType.ReadAsString:
                        SetToken(JsonToken.String, JsonConvert.NaN);
                        return JsonConvert.NaN;
                }

                throw JsonReaderException.Create(this, "Cannot read NaN value.");
            }

            throw JsonReaderException.Create(this, "Error parsing NaN value.");
        }

        /// <summary>
        /// Changes the reader's state to <see cref="JsonReader.State.Closed" />.
        /// If <see cref="JsonReader.CloseInput" /> is set to <c>true</c>, the underlying <see cref="TextReader" /> is also closed.
        /// </summary>
        /// <MetaDataID>{e1429616-2405-4219-8b5a-c9f797c146bb}</MetaDataID>
        public override void Close()
        {
            base.Close();

            if (_chars != null)
            {
                BufferUtils.ReturnBuffer(_arrayPool, _chars);
                _chars = null;
            }

            if (CloseInput)
            {
#if HAVE_STREAM_READER_WRITER_CLOSE
                _reader?.Close();
#else
                _reader?.Dispose();
#endif
            }

            _stringBuffer.Clear(_arrayPool);
        }

        /// <summary>
        /// Gets a value indicating whether the class can return line information.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if <see cref="JsonTextReader.LineNumber" /> and <see cref="JsonTextReader.LinePosition" /> can be provided; otherwise, <c>false</c>.
        /// </returns>
        /// <MetaDataID>{fe2e637b-c1fb-445b-b673-f13594b8ea61}</MetaDataID>
        public bool HasLineInfo()
        {
            return true;
        }

        /// <summary>
        /// Gets the current line number.
        /// </summary>
        /// <value>
        /// The current line number or 0 if no line information is available (for example, <see cref="JsonTextReader.HasLineInfo" /> returns <c>false</c>).
        /// </value>
        /// <MetaDataID>{387a154f-2bf1-4172-91b3-ac36018c77e6}</MetaDataID>
        public int LineNumber
        {
            get
            {
                if (CurrentState == State.Start && LinePosition == 0 && TokenType != JsonToken.Comment)
                {
                    return 0;
                }

                return _lineNumber;
            }
        }

        /// <summary>
        /// Gets the current line position.
        /// </summary>
        /// <value>
        /// The current line position or 0 if no line information is available (for example, <see cref="JsonTextReader.HasLineInfo" /> returns <c>false</c>).
        /// </value>
        /// <MetaDataID>{73004804-72d8-4f3e-9ca0-15c09be4fe90}</MetaDataID>
        public int LinePosition => _charPos - _lineStartPos;
    }
}
