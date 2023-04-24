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

#if HAVE_ASYNC

using System;
using System.Globalization;
using System.Threading;
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
using System.Threading.Tasks;
using OOAdvantech.Json.Serialization;
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Json
{
    /// <MetaDataID>OOAdvantech.Json.JsonTextReader</MetaDataID>
    public partial class JsonTextReader
    {
        // It's not safe to perform the async methods here in a derived class as if the synchronous equivalent
        // has been overriden then the asychronous method will no longer be doing the same operation
#if HAVE_ASYNC // Double-check this isn't included inappropriately.
        /// <MetaDataID>{19b49937-b6fd-4078-a1b5-f90fbdec74d9}</MetaDataID>
        private readonly bool _safeAsync;
#endif

        /// <summary>
        /// Asynchronously reads the next JSON token from the source.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns <c>true</c> if the next token was read successfully; <c>false</c> if there are no more tokens to read.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{a3343f1a-8b29-4dca-902f-1e5a54142724}</MetaDataID>
        public override Task<bool> ReadAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsync(cancellationToken) : base.ReadAsync(cancellationToken);
        }

        /// <MetaDataID>{173c99ca-a1b6-4616-b13d-561e8a172081}</MetaDataID>
        internal Task<bool> DoReadAsync(CancellationToken cancellationToken)
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
                        return ParseValueAsync(cancellationToken);
                    case State.Object:
                    case State.ObjectStart:
                        return ParseObjectAsync(cancellationToken);
                    case State.PostValue:
                        Task<bool> task = ParsePostValueAsync(false, cancellationToken);
                        if (task.IsCompletedSucessfully())
                        {
                            if (task.Result)
                            {
                                return AsyncUtils.True;
                            }
                        }
                        else
                        {
                            return DoReadAsync(task, cancellationToken);
                        }
                        break;
                    case State.Finished:
                        return ReadFromFinishedAsync(cancellationToken);
                    default:
                        throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
                }
            }
        }

        /// <MetaDataID>{abd070e7-11dc-47a3-86e5-2ccc23f2e12b}</MetaDataID>
        private async Task<bool> DoReadAsync(Task<bool> task, CancellationToken cancellationToken)
        {
            bool result = await task.ConfigureAwait(false);
            if (result)
            {
                return true;
            }
            return await DoReadAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <MetaDataID>{cd42ca5f-7d20-4e60-8064-0255d8c04af5}</MetaDataID>
        private async Task<bool> ParsePostValueAsync(bool ignoreComments, CancellationToken cancellationToken)
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (await ReadDataAsync(false, cancellationToken).ConfigureAwait(false) == 0)
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
                        await ParseCommentAsync(!ignoreComments, cancellationToken).ConfigureAwait(false);
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
                        await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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

        /// <MetaDataID>{951ecf68-5e90-4275-ab39-119e290d4a8f}</MetaDataID>
        private async Task<bool> ReadFromFinishedAsync(CancellationToken cancellationToken)
        {
            if (await EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false))
            {
                await EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
                if (_isEndOfFile)
                {
                    SetToken(JsonToken.None);
                    return false;
                }

                if (_chars[_charPos] == '/')
                {
                    await ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
                    return true;
                }

                throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
            }

            SetToken(JsonToken.None);
            return false;
        }

        /// <MetaDataID>{9b618cd8-5aad-484e-9ce6-6be7afa927a4}</MetaDataID>
        private Task<int> ReadDataAsync(bool append, CancellationToken cancellationToken)
        {
            return ReadDataAsync(append, 0, cancellationToken);
        }

        /// <MetaDataID>{295b8ef4-6c11-4b62-ac64-0134f94c778e}</MetaDataID>
        private async Task<int> ReadDataAsync(bool append, int charsRequired, CancellationToken cancellationToken)
        {
            if (_isEndOfFile)
            {
                return 0;
            }

            PrepareBufferForReadData(append, charsRequired);

            int charsRead = await _reader.ReadAsync(_chars, _charsUsed, _chars.Length - _charsUsed - 1, cancellationToken).ConfigureAwait(false);

            _charsUsed += charsRead;

            if (charsRead == 0)
            {
                _isEndOfFile = true;
            }

            _chars[_charsUsed] = '\0';
            return charsRead;
        }

        /// <MetaDataID>{99cfab81-1066-4405-b75e-c320847480e4}</MetaDataID>
        private async Task<bool> ParseValueAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (await ReadDataAsync(false, cancellationToken).ConfigureAwait(false) == 0)
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
                        await ParseStringAsync(currentChar, ReadType.Read, cancellationToken).ConfigureAwait(false);
                        return true;
                    case 't':
                        await ParseTrueAsync(cancellationToken).ConfigureAwait(false);
                        return true;
                    case 'f':
                        await ParseFalseAsync(cancellationToken).ConfigureAwait(false);
                        return true;
                    case 'n':
                        if (await EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false))
                        {
                            switch (_chars[_charPos + 1])
                            {
                                case 'u':
                                    await ParseNullAsync(cancellationToken).ConfigureAwait(false);
                                    break;
                                case 'e':
                                    await ParseConstructorAsync(cancellationToken).ConfigureAwait(false);
                                    break;
                                default:
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
                        await ParseNumberNaNAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
                        return true;
                    case 'I':
                        await ParseNumberPositiveInfinityAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
                        return true;
                    case '-':
                        if (await EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false) && _chars[_charPos + 1] == 'I')
                        {
                            await ParseNumberNegativeInfinityAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
                        }
                        else
                        {
                            await ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
                        }
                        return true;
                    case '/':
                        await ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
                        return true;
                    case 'u':
                        await ParseUndefinedAsync(cancellationToken).ConfigureAwait(false);
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
                        await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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
                            await ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
                            return true;
                        }

                        throw CreateUnexpectedCharacterException(currentChar);
                }
            }
        }

        /// <MetaDataID>{faf0f9c8-5ff9-4e53-8784-bb653eab6ed1}</MetaDataID>
        private async Task ReadStringIntoBufferAsync(char quote, CancellationToken cancellationToken)
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

                            if (await ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
                            {
                                _charPos = charPos;
                                throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
                            }
                        }

                        break;
                    case '\\':
                        _charPos = charPos;
                        if (!await EnsureCharsAsync(0, true, cancellationToken).ConfigureAwait(false))
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
                                writeChar = await ParseUnicodeAsync(cancellationToken).ConfigureAwait(false);

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
                                        if (await EnsureCharsAsync(2, true, cancellationToken).ConfigureAwait(false) && _chars[_charPos] == '\\' && _chars[_charPos + 1] == 'u')
                                        {
                                            char highSurrogate = writeChar;

                                            _charPos += 2;
                                            writeChar = await ParseUnicodeAsync(cancellationToken).ConfigureAwait(false);

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
                        await ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
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

        /// <MetaDataID>{b55caf62-dd22-4e61-b6fd-7e88993eab1a}</MetaDataID>
        private Task ProcessCarriageReturnAsync(bool append, CancellationToken cancellationToken)
        {
            _charPos++;

            Task<bool> task = EnsureCharsAsync(1, append, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                SetNewLine(task.Result);
                return AsyncUtils.CompletedTask;
            }

            return ProcessCarriageReturnAsync(task);
        }

        /// <MetaDataID>{3b05d497-829f-4e9f-9732-746c9279d4de}</MetaDataID>
        private async Task ProcessCarriageReturnAsync(Task<bool> task)
        {
            SetNewLine(await task.ConfigureAwait(false));
        }

        /// <MetaDataID>{c83636d8-adc6-4ea8-841f-02cd59f94498}</MetaDataID>
        private async Task<char> ParseUnicodeAsync(CancellationToken cancellationToken)
        {
            return ConvertUnicode(await EnsureCharsAsync(4, true, cancellationToken).ConfigureAwait(false));
        }

        /// <MetaDataID>{cb9cc671-1208-4899-9f90-a5ad2522f30f}</MetaDataID>
        private Task<bool> EnsureCharsAsync(int relativePosition, bool append, CancellationToken cancellationToken)
        {
            if (_charPos + relativePosition < _charsUsed)
            {
                return AsyncUtils.True;
            }

            if (_isEndOfFile)
            {
                return AsyncUtils.False;
            }

            return ReadCharsAsync(relativePosition, append, cancellationToken);
        }

        /// <MetaDataID>{61c34e2a-398d-48ec-82e8-5ec585e2d6b6}</MetaDataID>
        private async Task<bool> ReadCharsAsync(int relativePosition, bool append, CancellationToken cancellationToken)
        {
            int charsRequired = _charPos + relativePosition - _charsUsed + 1;

            // it is possible that the TextReader doesn't return all data at once
            // repeat read until the required text is returned or the reader is out of content
            do
            {
                int charsRead = await ReadDataAsync(append, charsRequired, cancellationToken).ConfigureAwait(false);

                // no more content
                if (charsRead == 0)
                {
                    return false;
                }

                charsRequired -= charsRead;
            } while (charsRequired > 0);

            return true;
        }

        /// <MetaDataID>{413d7969-899f-4916-81fa-04fef3f46ef8}</MetaDataID>
        private async Task<bool> ParseObjectAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (await ReadDataAsync(false, cancellationToken).ConfigureAwait(false) == 0)
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
                        await ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
                        return true;
                    case StringUtils.CarriageReturn:
                        await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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
                            return await ParsePropertyAsync(cancellationToken).ConfigureAwait(false);
                        }

                        break;
                }
            }
        }

        /// <MetaDataID>{0bfd9524-a3b0-4a26-bf68-67ff18681125}</MetaDataID>
        private async Task ParseCommentAsync(bool setToken, CancellationToken cancellationToken)
        {
            // should have already parsed / character before reaching this method
            _charPos++;

            if (!await EnsureCharsAsync(1, false, cancellationToken).ConfigureAwait(false))
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
                            if (await ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
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
                            if (await EnsureCharsAsync(0, true, cancellationToken).ConfigureAwait(false))
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

                        await ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
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

        /// <MetaDataID>{e2630ad8-d739-41e2-8e1f-dc18086c0d6a}</MetaDataID>
        private async Task EatWhitespaceAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                char currentChar = _chars[_charPos];

                switch (currentChar)
                {
                    case '\0':
                        if (_charsUsed == _charPos)
                        {
                            if (await ReadDataAsync(false, cancellationToken).ConfigureAwait(false) == 0)
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
                        await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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

        /// <MetaDataID>{7b6f6106-d746-4881-9e38-624ecf4ed384}</MetaDataID>
        private async Task ParseStringAsync(char quote, ReadType readType, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _charPos++;

            ShiftBufferIfNeeded();
            await ReadStringIntoBufferAsync(quote, cancellationToken).ConfigureAwait(false);
            ParseReadString(quote, readType);
        }

        /// <MetaDataID>{3139ecc9-86b7-4ec9-bbb8-a72fe4f00935}</MetaDataID>
        private async Task<bool> MatchValueAsync(string value, CancellationToken cancellationToken)
        {
            return MatchValue(await EnsureCharsAsync(value.Length - 1, true, cancellationToken).ConfigureAwait(false), value);
        }

        /// <MetaDataID>{68451c92-3bb6-42a9-b027-b2c9f107992a}</MetaDataID>
        private async Task<bool> MatchValueWithTrailingSeparatorAsync(string value, CancellationToken cancellationToken)
        {
            // will match value and then move to the next character, checking that it is a separator character
            if (!await MatchValueAsync(value, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }

            if (!await EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }

            return IsSeparator(_chars[_charPos]) || _chars[_charPos] == '\0';
        }

        /// <MetaDataID>{c7f46336-72f2-45d5-a166-8d2f358e5765}</MetaDataID>
        private async Task MatchAndSetAsync(string value, JsonToken newToken, object tokenValue, CancellationToken cancellationToken)
        {
            if (await MatchValueWithTrailingSeparatorAsync(value, cancellationToken).ConfigureAwait(false))
            {
                SetToken(newToken, tokenValue);
            }
            else
            {
                throw JsonReaderException.Create(this, "Error parsing " + newToken.ToString().ToLowerInvariant() + " value.");
            }
        }

        /// <MetaDataID>{63725795-220a-4a07-902b-c7e4a801b5a5}</MetaDataID>
        private Task ParseTrueAsync(CancellationToken cancellationToken)
        {
            return MatchAndSetAsync(JsonConvert.True, JsonToken.Boolean, true, cancellationToken);
        }

        /// <MetaDataID>{2404714c-66cf-487f-adbd-90ad5b608382}</MetaDataID>
        private Task ParseFalseAsync(CancellationToken cancellationToken)
        {
            return MatchAndSetAsync(JsonConvert.False, JsonToken.Boolean, false, cancellationToken);
        }

        /// <MetaDataID>{d613af62-ca9a-467a-8a6c-c72e6cd2fb40}</MetaDataID>
        private Task ParseNullAsync(CancellationToken cancellationToken)
        {
            return MatchAndSetAsync(JsonConvert.Null, JsonToken.Null, null, cancellationToken);
        }

        /// <MetaDataID>{0528f2c1-8ac0-4c52-a60f-adfa8fcd33e2}</MetaDataID>
        private async Task ParseConstructorAsync(CancellationToken cancellationToken)
        {
            if (await MatchValueWithTrailingSeparatorAsync("new", cancellationToken).ConfigureAwait(false))
            {
                await EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);

                int initialPosition = _charPos;
                int endPosition;

                while (true)
                {
                    char currentChar = _chars[_charPos];
                    if (currentChar == '\0')
                    {
                        if (_charsUsed == _charPos)
                        {
                            if (await ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
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
                        await ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
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

                await EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);

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

        /// <MetaDataID>{323aefc5-d945-4df7-abe2-76668ed4c870}</MetaDataID>
        private async Task<object> ParseNumberNaNAsync(ReadType readType, CancellationToken cancellationToken)
        {
            return ParseNumberNaN(readType, await MatchValueWithTrailingSeparatorAsync(JsonConvert.NaN, cancellationToken).ConfigureAwait(false));
        }

        /// <MetaDataID>{fc3c1473-1fae-417a-969b-c0431de7ed73}</MetaDataID>
        private async Task<object> ParseNumberPositiveInfinityAsync(ReadType readType, CancellationToken cancellationToken)
        {
            return ParseNumberPositiveInfinity(readType, await MatchValueWithTrailingSeparatorAsync(JsonConvert.PositiveInfinity, cancellationToken).ConfigureAwait(false));
        }

        /// <MetaDataID>{54bed2a0-3fa3-4e14-9e1c-0939daaff883}</MetaDataID>
        private async Task<object> ParseNumberNegativeInfinityAsync(ReadType readType, CancellationToken cancellationToken)
        {
            return ParseNumberNegativeInfinity(readType, await MatchValueWithTrailingSeparatorAsync(JsonConvert.NegativeInfinity, cancellationToken).ConfigureAwait(false));
        }

        /// <MetaDataID>{7e94842a-817b-44aa-8682-ba155b248b28}</MetaDataID>
        private async Task ParseNumberAsync(ReadType readType, CancellationToken cancellationToken)
        {
            ShiftBufferIfNeeded();

            char firstChar = _chars[_charPos];
            int initialPosition = _charPos;

            await ReadNumberIntoBufferAsync(cancellationToken).ConfigureAwait(false);

            ParseReadNumber(readType, firstChar, initialPosition);
        }

        /// <MetaDataID>{40c9f201-af45-4d7a-8126-4e4e0d359825}</MetaDataID>
        private Task ParseUndefinedAsync(CancellationToken cancellationToken)
        {
            return MatchAndSetAsync(JsonConvert.Undefined, JsonToken.Undefined, null, cancellationToken);
        }

        /// <MetaDataID>{f85cf88e-af41-4c27-8a27-3922e832a378}</MetaDataID>
        private async Task<bool> ParsePropertyAsync(CancellationToken cancellationToken)
        {
            char firstChar = _chars[_charPos];
            char quoteChar;

            if (firstChar == '"' || firstChar == '\'')
            {
                _charPos++;
                quoteChar = firstChar;
                ShiftBufferIfNeeded();
                await ReadStringIntoBufferAsync(quoteChar, cancellationToken).ConfigureAwait(false);
            }
            else if (ValidIdentifierChar(firstChar))
            {
                quoteChar = '\0';
                ShiftBufferIfNeeded();
                await ParseUnquotedPropertyAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
            }

            string propertyName;

            if (PropertyNameTable != null)
            {
                propertyName = PropertyNameTable.Get(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length)
                    // no match in name table
                    ?? _stringReference.ToString();
            }
            else
            {
                propertyName = _stringReference.ToString();
            }

            await EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);

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

        /// <MetaDataID>{7d3c4afe-c864-4ec2-bac2-ece709968e49}</MetaDataID>
        private async Task ReadNumberIntoBufferAsync(CancellationToken cancellationToken)
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
                        if (await ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
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

        /// <MetaDataID>{5badf2f5-591c-41dc-aa89-6cd376d3fa97}</MetaDataID>
        private async Task ParseUnquotedPropertyAsync(CancellationToken cancellationToken)
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
                        if (await ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
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

        /// <MetaDataID>{d0e83169-2eb7-4f50-89ca-f1f255da3589}</MetaDataID>
        private async Task<bool> ReadNullCharAsync(CancellationToken cancellationToken)
        {
            if (_charsUsed == _charPos)
            {
                if (await ReadDataAsync(false, cancellationToken).ConfigureAwait(false) == 0)
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

        /// <MetaDataID>{6fc3e16b-51d1-4aff-bae1-41cd898cd20f}</MetaDataID>
        private async Task HandleNullAsync(CancellationToken cancellationToken)
        {
            if (await EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false))
            {
                if (_chars[_charPos + 1] == 'u')
                {
                    await ParseNullAsync(cancellationToken).ConfigureAwait(false);
                    return;
                }

                _charPos += 2;
                throw CreateUnexpectedCharacterException(_chars[_charPos - 1]);
            }

            _charPos = _charsUsed;
            throw CreateUnexpectedEndException();
        }

        /// <MetaDataID>{0ee5877a-3e4c-4359-97d4-5fca87623231}</MetaDataID>
        private async Task ReadFinishedAsync(CancellationToken cancellationToken)
        {
            if (await EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false))
            {
                await EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
                if (_isEndOfFile)
                {
                    SetToken(JsonToken.None);
                    return;
                }

                if (_chars[_charPos] == '/')
                {
                    await ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
                }
            }

            SetToken(JsonToken.None);
        }

        /// <MetaDataID>{7b8e21c5-5860-4c29-a1d0-69b6e2852a46}</MetaDataID>
        private async Task<object> ReadStringValueAsync(ReadType readType, CancellationToken cancellationToken)
        {
            EnsureBuffer();

            switch (_currentState)
            {
                case State.PostValue:
                    if (await ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
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
                                if (await ReadNullCharAsync(cancellationToken).ConfigureAwait(false))
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }

                                break;
                            case '"':
                            case '\'':
                                await ParseStringAsync(currentChar, readType, cancellationToken).ConfigureAwait(false);
                                return FinishReadQuotedStringValue(readType);
                            case '-':
                                if (await EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false) && _chars[_charPos + 1] == 'I')
                                {
                                    return ParseNumberNegativeInfinity(readType);
                                }
                                else
                                {
                                    await ParseNumberAsync(readType, cancellationToken).ConfigureAwait(false);
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

                                await ParseNumberAsync(ReadType.ReadAsString, cancellationToken).ConfigureAwait(false);
                                return Value;
                            case 't':
                            case 'f':
                                if (readType != ReadType.ReadAsString)
                                {
                                    _charPos++;
                                    throw CreateUnexpectedCharacterException(currentChar);
                                }

                                string expected = currentChar == 't' ? JsonConvert.True : JsonConvert.False;
                                if (!await MatchValueWithTrailingSeparatorAsync(expected, cancellationToken).ConfigureAwait(false))
                                {
                                    throw CreateUnexpectedCharacterException(_chars[_charPos]);
                                }

                                SetToken(JsonToken.String, expected);
                                return expected;
                            case 'I':
                                return await ParseNumberPositiveInfinityAsync(readType, cancellationToken).ConfigureAwait(false);
                            case 'N':
                                return await ParseNumberNaNAsync(readType, cancellationToken).ConfigureAwait(false);
                            case 'n':
                                await HandleNullAsync(cancellationToken).ConfigureAwait(false);
                                return null;
                            case '/':
                                await ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
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
                                await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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
                    await ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <MetaDataID>{281190fa-9ac9-4db5-953d-e9e3ef7e4e28}</MetaDataID>
        private async Task<object> ReadNumberValueAsync(ReadType readType, CancellationToken cancellationToken)
        {
            EnsureBuffer();

            switch (_currentState)
            {
                case State.PostValue:
                    if (await ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
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
                                if (await ReadNullCharAsync(cancellationToken).ConfigureAwait(false))
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }

                                break;
                            case '"':
                            case '\'':
                                await ParseStringAsync(currentChar, readType, cancellationToken).ConfigureAwait(false);
                                return FinishReadQuotedNumber(readType);
                            case 'n':
                                await HandleNullAsync(cancellationToken).ConfigureAwait(false);
                                return null;
                            case 'N':
                                return await ParseNumberNaNAsync(readType, cancellationToken).ConfigureAwait(false);
                            case 'I':
                                return await ParseNumberPositiveInfinityAsync(readType, cancellationToken).ConfigureAwait(false);
                            case '-':
                                if (await EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false) && _chars[_charPos + 1] == 'I')
                                {
                                    return await ParseNumberNegativeInfinityAsync(readType, cancellationToken).ConfigureAwait(false);
                                }
                                else
                                {
                                    await ParseNumberAsync(readType, cancellationToken).ConfigureAwait(false);
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
                                await ParseNumberAsync(readType, cancellationToken).ConfigureAwait(false);
                                return Value;
                            case '/':
                                await ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
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
                                await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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
                    await ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="bool" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="bool" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{c95d5067-a408-48c3-aa0a-64607961c157}</MetaDataID>
        public override Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsBooleanAsync(cancellationToken) : base.ReadAsBooleanAsync(cancellationToken);
        }

        /// <MetaDataID>{3ca22f3d-bab3-496f-ba4e-843364c59718}</MetaDataID>
        internal async Task<bool?> DoReadAsBooleanAsync(CancellationToken cancellationToken)
        {
            EnsureBuffer();

            switch (_currentState)
            {
                case State.PostValue:
                    if (await ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
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
                                if (await ReadNullCharAsync(cancellationToken).ConfigureAwait(false))
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }

                                break;
                            case '"':
                            case '\'':
                                await ParseStringAsync(currentChar, ReadType.Read, cancellationToken).ConfigureAwait(false);
                                return ReadBooleanString(_stringReference.ToString());
                            case 'n':
                                await HandleNullAsync(cancellationToken).ConfigureAwait(false);
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
                                await ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
                                bool b;
#if HAVE_BIG_INTEGER
                                if (Value is BigInteger i)
                                {
                                    b = i != 0;
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
                                if (!await MatchValueWithTrailingSeparatorAsync(isTrue ? JsonConvert.True : JsonConvert.False, cancellationToken).ConfigureAwait(false))
                                {
                                    throw CreateUnexpectedCharacterException(_chars[_charPos]);
                                }

                                SetToken(JsonToken.Boolean, isTrue);
                                return isTrue;
                            case '/':
                                await ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
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
                                await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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
                    await ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="byte" />[].
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="byte" />[]. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{aaea15b9-151e-48a7-97ca-3ca7a2a1cd38}</MetaDataID>
        public override Task<byte[]> ReadAsBytesAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsBytesAsync(cancellationToken) : base.ReadAsBytesAsync(cancellationToken);
        }

        /// <MetaDataID>{75371594-090d-4124-86a5-8b6dd705d8f1}</MetaDataID>
        internal async Task<byte[]> DoReadAsBytesAsync(CancellationToken cancellationToken)
        {
            EnsureBuffer();
            bool isWrapped = false;

            switch (_currentState)
            {
                case State.PostValue:
                    if (await ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
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
                                if (await ReadNullCharAsync(cancellationToken).ConfigureAwait(false))
                                {
                                    SetToken(JsonToken.None, null, false);
                                    return null;
                                }

                                break;
                            case '"':
                            case '\'':
                                await ParseStringAsync(currentChar, ReadType.ReadAsBytes, cancellationToken).ConfigureAwait(false);
                                byte[] data = (byte[])Value;
                                if (isWrapped)
                                {
                                    await ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
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
                                await ReadIntoWrappedTypeObjectAsync(cancellationToken).ConfigureAwait(false);
                                isWrapped = true;
                                break;
                            case '[':
                                _charPos++;
                                SetToken(JsonToken.StartArray);
                                return await ReadArrayIntoByteArrayAsync(cancellationToken).ConfigureAwait(false);
                            case 'n':
                                await HandleNullAsync(cancellationToken).ConfigureAwait(false);
                                return null;
                            case '/':
                                await ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
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
                                await ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
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
                    await ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
                    return null;
                default:
                    throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, CurrentState));
            }
        }

        /// <MetaDataID>{b6371d48-b585-4445-8fcc-190680221461}</MetaDataID>
        private async Task ReadIntoWrappedTypeObjectAsync(CancellationToken cancellationToken)
        {
            await ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
            if (Value != null && Value.ToString() == JsonTypeReflector.TypePropertyName)
            {
                await ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
                if (Value != null && Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
                {
                    await ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
                    if (Value.ToString() == JsonTypeReflector.ValuePropertyName)
                    {
                        return;
                    }
                }
            }

            throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="DateTime" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="DateTime" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{6e4f8307-150e-4884-b709-4e189e8ceed6}</MetaDataID>
        public override Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsDateTimeAsync(cancellationToken) : base.ReadAsDateTimeAsync(cancellationToken);
        }

        /// <MetaDataID>{742a506d-33c0-47ff-b30a-b8dec4cb1e02}</MetaDataID>
        internal async Task<DateTime?> DoReadAsDateTimeAsync(CancellationToken cancellationToken)
        {
            return (DateTime?)await ReadStringValueAsync(ReadType.ReadAsDateTime, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{6c941fa9-0f2d-423f-8072-387179d046ba}</MetaDataID>
        public override Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsDateTimeOffsetAsync(cancellationToken) : base.ReadAsDateTimeOffsetAsync(cancellationToken);
        }

        /// <MetaDataID>{705622cf-ba3d-4705-bf88-b6fd4f0e6942}</MetaDataID>
        internal async Task<DateTimeOffset?> DoReadAsDateTimeOffsetAsync(CancellationToken cancellationToken)
        {
            return (DateTimeOffset?)await ReadStringValueAsync(ReadType.ReadAsDateTimeOffset, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="decimal" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="decimal" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{20e3ee3f-0fe3-4592-9657-9495c83c2cd6}</MetaDataID>
        public override Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsDecimalAsync(cancellationToken) : base.ReadAsDecimalAsync(cancellationToken);
        }

        /// <MetaDataID>{9d9a82ed-03ce-4b68-9baf-087ba548cce5}</MetaDataID>
        internal async Task<decimal?> DoReadAsDecimalAsync(CancellationToken cancellationToken)
        {
            return (decimal?)await ReadNumberValueAsync(ReadType.ReadAsDecimal, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="double" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="double" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{d06a586c-a4e8-46cf-ba73-9a404964984c}</MetaDataID>
        public override Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsDoubleAsync(cancellationToken) : base.ReadAsDoubleAsync(cancellationToken);
        }

        /// <MetaDataID>{0b880b8b-42d8-42a4-abcd-9f621c6bf72d}</MetaDataID>
        internal async Task<double?> DoReadAsDoubleAsync(CancellationToken cancellationToken)
        {
            return (double?)await ReadNumberValueAsync(ReadType.ReadAsDouble, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="int" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="int" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{2c4b9b15-e94b-4355-a480-4e85d36929f5}</MetaDataID>
        public override Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsInt32Async(cancellationToken) : base.ReadAsInt32Async(cancellationToken);
        }

        /// <MetaDataID>{8f028d59-2a4a-4b22-8d8a-4a0ea1286185}</MetaDataID>
        internal async Task<int?> DoReadAsInt32Async(CancellationToken cancellationToken)
        {
            return (int?)await ReadNumberValueAsync(ReadType.ReadAsInt32, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="string" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="string" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{3b157eec-abb9-46ac-ab39-eecf60906b6c}</MetaDataID>
        public override Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoReadAsStringAsync(cancellationToken) : base.ReadAsStringAsync(cancellationToken);
        }

        /// <MetaDataID>{d2bbf425-eb24-41db-98d8-f055e1af2e5e}</MetaDataID>
        internal async Task<string> DoReadAsStringAsync(CancellationToken cancellationToken)
        {
            return (string)await ReadStringValueAsync(ReadType.ReadAsString, cancellationToken).ConfigureAwait(false);
        }
    }
}

#endif
