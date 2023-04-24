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
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Json
{
    /// <MetaDataID>OOAdvantech.Json.JsonTextWriter</MetaDataID>
    public partial class JsonTextWriter
    {
        // It's not safe to perform the async methods here in a derived class as if the synchronous equivalent
        // has been overriden then the asychronous method will no longer be doing the same operation.
#if HAVE_ASYNC // Double-check this isn't included inappropriately.
        /// <MetaDataID>{84383969-a65a-4a87-92d3-ceecd26fcadc}</MetaDataID>
        private readonly bool _safeAsync;
#endif

        /// <summary>
        /// Asynchronously flushes whatever is in the buffer to the destination and also flushes the destination.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{77026ad9-76b4-4993-b6a2-a1b3251f454c}</MetaDataID>
        public override Task FlushAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoFlushAsync(cancellationToken) : base.FlushAsync(cancellationToken);
        }

        /// <MetaDataID>{4bc7fd66-44ff-469f-9e8d-c4b3ad3700a7}</MetaDataID>
        internal Task DoFlushAsync(CancellationToken cancellationToken)
        {
            return cancellationToken.CancelIfRequestedAsync() ?? _writer.FlushAsync();
        }

        /// <summary>
        /// Asynchronously writes the JSON value delimiter.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{f6799789-ec95-47bd-9ad1-dd34273b0314}</MetaDataID>
        protected override Task WriteValueDelimiterAsync(CancellationToken cancellationToken)
        {
            return _safeAsync ? DoWriteValueDelimiterAsync(cancellationToken) : base.WriteValueDelimiterAsync(cancellationToken);
        }

        /// <MetaDataID>{f53537cf-d3c3-47e0-980c-4db299548fbf}</MetaDataID>
        internal Task DoWriteValueDelimiterAsync(CancellationToken cancellationToken)
        {
            return _writer.WriteAsync(',', cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the specified end token.
        /// </summary>
        /// <param name="token">The end token to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{7a12b19f-50ab-4878-8169-c378948576b7}</MetaDataID>
        protected override Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken)
        {
            return _safeAsync ? DoWriteEndAsync(token, cancellationToken) : base.WriteEndAsync(token, cancellationToken);
        }

        /// <MetaDataID>{d1e4a2e8-1563-4fb3-b7ca-bc9a1b25acac}</MetaDataID>
        internal Task DoWriteEndAsync(JsonToken token, CancellationToken cancellationToken)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return _writer.WriteAsync('}', cancellationToken);
                case JsonToken.EndArray:
                    return _writer.WriteAsync(']', cancellationToken);
                case JsonToken.EndConstructor:
                    return _writer.WriteAsync(')', cancellationToken);
                default:
                    throw JsonWriterException.Create(this, "Invalid JsonToken: " + token, null);
            }
        }

        /// <summary>
        /// Asynchronously closes this writer.
        /// If <see cref="JsonWriter.CloseOutput" /> is set to <c>true</c>, the destination is also closed.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{38dcdc97-042d-43ed-b3de-f62e4c1fd9d4}</MetaDataID>
        public override Task CloseAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoCloseAsync(cancellationToken) : base.CloseAsync(cancellationToken);
        }

        /// <MetaDataID>{6e7025a2-23f6-4898-ab9c-659353940524}</MetaDataID>
        internal async Task DoCloseAsync(CancellationToken cancellationToken)
        {
            if (Top == 0) // otherwise will happen in calls to WriteEndAsync
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            while (Top > 0)
            {
                await WriteEndAsync(cancellationToken).ConfigureAwait(false);
            }

            CloseBufferAndWriter();
        }

        /// <summary>
        /// Asynchronously writes the end of the current JSON object or array.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{3151c5e8-95f1-4380-a01b-759997258e15}</MetaDataID>
        public override Task WriteEndAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteEndInternalAsync(cancellationToken) : base.WriteEndAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes indent characters.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{952c4dbc-997f-4ceb-92e0-6b74d27917ea}</MetaDataID>
        protected override Task WriteIndentAsync(CancellationToken cancellationToken)
        {
            return _safeAsync ? DoWriteIndentAsync(cancellationToken) : base.WriteIndentAsync(cancellationToken);
        }

        /// <MetaDataID>{aacd494c-3d7d-407c-99ea-4ca5f8a79389}</MetaDataID>
        internal Task DoWriteIndentAsync(CancellationToken cancellationToken)
        {
            // levels of indentation multiplied by the indent count
            int currentIndentCount = Top * _indentation;

            int newLineLen = SetIndentChars();

            if (currentIndentCount <= IndentCharBufferSize)
            {
                return _writer.WriteAsync(_indentChars, 0, newLineLen + currentIndentCount, cancellationToken);
            }

            return WriteIndentAsync(currentIndentCount, newLineLen, cancellationToken);
        }

        /// <MetaDataID>{f15c0e30-d3ee-4153-b2a4-0b5d28293295}</MetaDataID>
        private async Task WriteIndentAsync(int currentIndentCount, int newLineLen, CancellationToken cancellationToken)
        {
            await _writer.WriteAsync(_indentChars, 0, newLineLen + Math.Min(currentIndentCount, IndentCharBufferSize), cancellationToken).ConfigureAwait(false);

            while ((currentIndentCount -= IndentCharBufferSize) > 0)
            {
                await _writer.WriteAsync(_indentChars, newLineLen, Math.Min(currentIndentCount, IndentCharBufferSize), cancellationToken).ConfigureAwait(false);
            }
        }

        /// <MetaDataID>{150665b8-0bdb-4e8d-a3c6-c5d7fb5bee13}</MetaDataID>
        private Task WriteValueInternalAsync(JsonToken token, string value, CancellationToken cancellationToken)
        {
            Task task = InternalWriteValueAsync(token, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return _writer.WriteAsync(value, cancellationToken);
            }

            return WriteValueInternalAsync(task, value, cancellationToken);
        }

        /// <MetaDataID>{0909b7ac-3af3-4a92-bbb8-509e7f2d82b0}</MetaDataID>
        private async Task WriteValueInternalAsync(Task task, string value, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await _writer.WriteAsync(value, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes an indent space.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{4ec56269-7de1-497b-a857-1ad52126c0e2}</MetaDataID>
        protected override Task WriteIndentSpaceAsync(CancellationToken cancellationToken)
        {
            return _safeAsync ? DoWriteIndentSpaceAsync(cancellationToken) : base.WriteIndentSpaceAsync(cancellationToken);
        }

        /// <MetaDataID>{0bcbaae8-902f-4eec-ad75-1e420828b2a1}</MetaDataID>
        internal Task DoWriteIndentSpaceAsync(CancellationToken cancellationToken)
        {
            return _writer.WriteAsync(' ', cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes raw JSON without changing the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{5c2472a7-0d26-4ade-8eb6-8c8d1eea4a4f}</MetaDataID>
        public override Task WriteRawAsync(string json, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteRawAsync(json, cancellationToken) : base.WriteRawAsync(json, cancellationToken);
        }

        /// <MetaDataID>{be72d10a-211e-4b26-9f2d-208cf28ee0e3}</MetaDataID>
        internal Task DoWriteRawAsync(string json, CancellationToken cancellationToken)
        {
            return _writer.WriteAsync(json, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a null value.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{710f4a2e-de24-4243-862d-eb66df9fb57f}</MetaDataID>
        public override Task WriteNullAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteNullAsync(cancellationToken) : base.WriteNullAsync(cancellationToken);
        }

        /// <MetaDataID>{9e8d081b-5a26-4a78-858c-7d69479d3d7f}</MetaDataID>
        internal Task DoWriteNullAsync(CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.Null, JsonConvert.Null, cancellationToken);
        }

        /// <MetaDataID>{e4e40fc8-396d-487a-9dac-ad2ce2749c5a}</MetaDataID>
        private Task WriteDigitsAsync(ulong uvalue, bool negative, CancellationToken cancellationToken)
        {
            if (uvalue <= 9 & !negative)
            {
                return _writer.WriteAsync((char)('0' + uvalue), cancellationToken);
            }

            int length = WriteNumberToBuffer(uvalue, negative);
            return _writer.WriteAsync(_writeBuffer, 0, length, cancellationToken);
        }

        /// <MetaDataID>{fd2086de-3d3b-4e43-96a9-2341f2148e21}</MetaDataID>
        private Task WriteIntegerValueAsync(ulong uvalue, bool negative, CancellationToken cancellationToken)
        {
            Task task = InternalWriteValueAsync(JsonToken.Integer, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return WriteDigitsAsync(uvalue, negative, cancellationToken);
            }

            return WriteIntegerValueAsync(task, uvalue, negative, cancellationToken);
        }

        /// <MetaDataID>{648d13b9-57dd-4c0b-a51a-dbf70c7c18b2}</MetaDataID>
        private async Task WriteIntegerValueAsync(Task task, ulong uvalue, bool negative, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await WriteDigitsAsync(uvalue, negative, cancellationToken).ConfigureAwait(false);
        }

        /// <MetaDataID>{3c196d9c-b2c1-4bb4-a9fb-4fb8b39300dc}</MetaDataID>
        internal Task WriteIntegerValueAsync(long value, CancellationToken cancellationToken)
        {
            bool negative = value < 0;
            if (negative)
            {
                value = -value;
            }

            return WriteIntegerValueAsync((ulong)value, negative, cancellationToken);
        }

        /// <MetaDataID>{08a4c8ac-41c0-47ad-a0f7-b9c36f62d6be}</MetaDataID>
        internal Task WriteIntegerValueAsync(ulong uvalue, CancellationToken cancellationToken)
        {
            return WriteIntegerValueAsync(uvalue, false, cancellationToken);
        }

        /// <MetaDataID>{3ff3b8fb-03d9-410b-9425-8def15a32930}</MetaDataID>
        private Task WriteEscapedStringAsync(string value, bool quote, CancellationToken cancellationToken)
        {
            return JavaScriptUtils.WriteEscapedJavaScriptStringAsync(_writer, value, _quoteChar, quote, _charEscapeFlags, StringEscapeHandling, this, _writeBuffer, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{f370091a-8020-4ef6-81e5-e0e6413a9aff}</MetaDataID>
        public override Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWritePropertyNameAsync(name, cancellationToken) : base.WritePropertyNameAsync(name, cancellationToken);
        }

        /// <MetaDataID>{b0a54de6-c19c-4279-8b90-0c0fa0792722}</MetaDataID>
        internal Task DoWritePropertyNameAsync(string name, CancellationToken cancellationToken)
        {
            Task task = InternalWritePropertyNameAsync(name, cancellationToken);
            if (!task.IsCompletedSucessfully())
            {
                return DoWritePropertyNameAsync(task, name, cancellationToken);
            }

            task = WriteEscapedStringAsync(name, _quoteName, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return _writer.WriteAsync(':', cancellationToken);
            }

            return JavaScriptUtils.WriteCharAsync(task, _writer, ':', cancellationToken);
        }

        /// <MetaDataID>{7cbb34e3-e9c4-40fe-a70c-fc10b37613e0}</MetaDataID>
        private async Task DoWritePropertyNameAsync(Task task, string name, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);

            await WriteEscapedStringAsync(name, _quoteName, cancellationToken).ConfigureAwait(false);

            await _writer.WriteAsync(':').ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="escape">A flag to indicate whether the text should be escaped when it is written as a JSON property name.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{c1da347b-e410-42af-a9b3-13c3e5cd50d8}</MetaDataID>
        public override Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWritePropertyNameAsync(name, escape, cancellationToken) : base.WritePropertyNameAsync(name, escape, cancellationToken);
        }

        /// <MetaDataID>{02e52641-edb8-4acd-9f25-550f8272c3b7}</MetaDataID>
        internal async Task DoWritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken)
        {
            await InternalWritePropertyNameAsync(name, cancellationToken).ConfigureAwait(false);

            if (escape)
            {
                await WriteEscapedStringAsync(name, _quoteName, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                if (_quoteName)
                {
                    await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
                }

                await _writer.WriteAsync(name, cancellationToken).ConfigureAwait(false);

                if (_quoteName)
                {
                    await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
                }
            }

            await _writer.WriteAsync(':').ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes the beginning of a JSON array.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{34ad8fe2-2c21-4090-abe9-3d5699b32e03}</MetaDataID>
        public override Task WriteStartArrayAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteStartArrayAsync(cancellationToken) : base.WriteStartArrayAsync(cancellationToken);
        }

        /// <MetaDataID>{d319091b-9b6b-46c7-96d9-20475b370bac}</MetaDataID>
        internal Task DoWriteStartArrayAsync(CancellationToken cancellationToken)
        {
            Task task = InternalWriteStartAsync(JsonToken.StartArray, JsonContainerType.Array, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return _writer.WriteAsync('[', cancellationToken);
            }

            return DoWriteStartArrayAsync(task, cancellationToken);
        }

        /// <MetaDataID>{3018fe07-4d7e-4bd2-835a-9290f8b26bfb}</MetaDataID>
        internal async Task DoWriteStartArrayAsync(Task task, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);

            await _writer.WriteAsync('[', cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes the beginning of a JSON object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{a60a5008-94a7-4903-8a64-1da615787590}</MetaDataID>
        public override Task WriteStartObjectAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteStartObjectAsync(cancellationToken) : base.WriteStartObjectAsync(cancellationToken);
        }

        /// <MetaDataID>{3859dc26-262f-4adb-a72b-8620611475ae}</MetaDataID>
        internal Task DoWriteStartObjectAsync(CancellationToken cancellationToken)
        {
            Task task = InternalWriteStartAsync(JsonToken.StartObject, JsonContainerType.Object, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return _writer.WriteAsync('{', cancellationToken);
            }

            return DoWriteStartObjectAsync(task, cancellationToken);
        }

        /// <MetaDataID>{691e54d8-9142-4e4d-be4a-7bc6d6da2808}</MetaDataID>
        internal async Task DoWriteStartObjectAsync(Task task, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);

            await _writer.WriteAsync('{', cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes the start of a constructor with the given name.
        /// </summary>
        /// <param name="name">The name of the constructor.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{e3ae1368-c6eb-496c-a571-211fd274c46e}</MetaDataID>
        public override Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteStartConstructorAsync(name, cancellationToken) : base.WriteStartConstructorAsync(name, cancellationToken);
        }

        /// <MetaDataID>{335f0a6b-7d44-478a-9d5c-06be3d50792c}</MetaDataID>
        internal async Task DoWriteStartConstructorAsync(string name, CancellationToken cancellationToken)
        {
            await InternalWriteStartAsync(JsonToken.StartConstructor, JsonContainerType.Constructor, cancellationToken).ConfigureAwait(false);

            await _writer.WriteAsync("new ", cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(name, cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync('(').ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes an undefined value.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{6a6fa6e2-6f37-49bf-8bcb-f1471377b252}</MetaDataID>
        public override Task WriteUndefinedAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteUndefinedAsync(cancellationToken) : base.WriteUndefinedAsync(cancellationToken);
        }

        /// <MetaDataID>{3803af85-58bb-4029-a82f-7cd1429500ba}</MetaDataID>
        internal Task DoWriteUndefinedAsync(CancellationToken cancellationToken)
        {
            Task task = InternalWriteValueAsync(JsonToken.Undefined, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return _writer.WriteAsync(JsonConvert.Undefined, cancellationToken);
            }

            return DoWriteUndefinedAsync(task, cancellationToken);
        }

        /// <MetaDataID>{48563511-0e4e-4072-9375-acdb91ad6268}</MetaDataID>
        private async Task DoWriteUndefinedAsync(Task task, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await _writer.WriteAsync(JsonConvert.Undefined, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes the given white space.
        /// </summary>
        /// <param name="ws">The string of white space characters.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{8d8ba97c-5f2e-4902-bc92-02fdf5763c0d}</MetaDataID>
        public override Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteWhitespaceAsync(ws, cancellationToken) : base.WriteWhitespaceAsync(ws, cancellationToken);
        }

        /// <MetaDataID>{f9af8af5-3e0a-4c6f-895b-569ebc1404e5}</MetaDataID>
        internal Task DoWriteWhitespaceAsync(string ws, CancellationToken cancellationToken)
        {
            InternalWriteWhitespace(ws);
            return _writer.WriteAsync(ws, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="bool" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="bool" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{43188971-4698-46ae-8a2a-41eb02f0e5fe}</MetaDataID>
        public override Task WriteValueAsync(bool value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{d21c59b8-b526-47f0-87e0-351076ede9eb}</MetaDataID>
        internal Task DoWriteValueAsync(bool value, CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.Boolean, JsonConvert.ToString(value), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="bool" /> value.
        /// </summary>
        /// <param name="value">The <see cref="bool" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{14bea9db-ee5c-405f-b688-c609dd6e1ce1}</MetaDataID>
        public override Task WriteValueAsync(bool? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{e93b7393-9823-49f5-87ce-1a79a9199a6c}</MetaDataID>
        internal Task DoWriteValueAsync(bool? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="byte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="byte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{268bba48-7746-4e27-ab33-bb6f7d66532c}</MetaDataID>
        public override Task WriteValueAsync(byte value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="byte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="byte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{73a2edf0-784f-4030-a295-86bde40458b7}</MetaDataID>
        public override Task WriteValueAsync(byte? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{0d21ce4b-dbc0-4629-ae45-6d243c40a12a}</MetaDataID>
        internal Task DoWriteValueAsync(byte? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="byte" />[] value.
        /// </summary>
        /// <param name="value">The <see cref="byte" />[] value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{15808b37-fd27-467b-9954-f57450b49779}</MetaDataID>
        public override Task WriteValueAsync(byte[] value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? (value == null ? WriteNullAsync(cancellationToken) : WriteValueNonNullAsync(value, cancellationToken)) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{af80b8ef-e4a1-45d4-84da-98226a9c7660}</MetaDataID>
        internal async Task WriteValueNonNullAsync(byte[] value, CancellationToken cancellationToken)
        {
            await InternalWriteValueAsync(JsonToken.Bytes, cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
            await Base64Encoder.EncodeAsync(value, 0, value.Length, cancellationToken).ConfigureAwait(false);
            await Base64Encoder.FlushAsync(cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="char" /> value.
        /// </summary>
        /// <param name="value">The <see cref="char" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{26898ad5-127d-4a1c-abe5-b7fed932e337}</MetaDataID>
        public override Task WriteValueAsync(char value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{14e23420-6389-4715-8d43-81f688a99c63}</MetaDataID>
        internal Task DoWriteValueAsync(char value, CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.String, JsonConvert.ToString(value), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="char" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="char" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{16011db4-da1c-4328-a396-ec33d99efaa2}</MetaDataID>
        public override Task WriteValueAsync(char? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{91f65e32-d201-42e6-b90b-c35ee9457b1d}</MetaDataID>
        internal Task DoWriteValueAsync(char? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{056cbddb-2160-4fdc-8b6f-cb506f0ed2d8}</MetaDataID>
        public override Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{dd0fe9d6-47b0-457a-a99e-e2c6fbf96a21}</MetaDataID>
        internal async Task DoWriteValueAsync(DateTime value, CancellationToken cancellationToken)
        {
            await InternalWriteValueAsync(JsonToken.Date, cancellationToken).ConfigureAwait(false);
            value = DateTimeUtils.EnsureDateTime(value, DateTimeZoneHandling);

            if (string.IsNullOrEmpty(DateFormatString))
            {
                int length = WriteValueToBuffer(value);

                await _writer.WriteAsync(_writeBuffer, 0, length, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
                await _writer.WriteAsync(value.ToString(DateFormatString, Culture), cancellationToken).ConfigureAwait(false);
                await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="DateTime" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{051b9fab-4cbf-4080-9330-f684dfec6cb5}</MetaDataID>
        public override Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{e10632f0-934c-4af2-ba2e-b14ffd171f80}</MetaDataID>
        internal Task DoWriteValueAsync(DateTime? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="DateTimeOffset" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{ffe924de-abf9-4a26-9ce2-d7a8e9cd2bf0}</MetaDataID>
        public override Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{ef63151f-86f6-47e2-a3e6-6ba53e9624a0}</MetaDataID>
        internal async Task DoWriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken)
        {
            await InternalWriteValueAsync(JsonToken.Date, cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(DateFormatString))
            {
                int length = WriteValueToBuffer(value);

                await _writer.WriteAsync(_writeBuffer, 0, length, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
                await _writer.WriteAsync(value.ToString(DateFormatString, Culture), cancellationToken).ConfigureAwait(false);
                await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{6caac194-1ced-4c0c-ae7e-64846b227785}</MetaDataID>
        public override Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{ba4e8bb6-6cf9-4c1e-b63d-aac97e904e45}</MetaDataID>
        internal Task DoWriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="decimal" /> value.
        /// </summary>
        /// <param name="value">The <see cref="decimal" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{4e2476a6-a5bf-494b-b82a-65706ca4b90c}</MetaDataID>
        public override Task WriteValueAsync(decimal value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{07dcae40-2556-4b0c-ae49-7501be68b3fc}</MetaDataID>
        internal Task DoWriteValueAsync(decimal value, CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="decimal" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="decimal" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{1664b588-c4a8-477f-87e9-951b2fb1f091}</MetaDataID>
        public override Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{b10542ae-6844-4bbe-a823-89f7755df58f}</MetaDataID>
        internal Task DoWriteValueAsync(decimal? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="double" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{d3e3c51d-bfbc-4c54-b8e4-86d6beb3a3fe}</MetaDataID>
        public override Task WriteValueAsync(double value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteValueAsync(value, false, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{99b2ca2b-10ad-4263-80ab-757d37e69dc9}</MetaDataID>
        internal Task WriteValueAsync(double value, bool nullable, CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value, FloatFormatHandling, QuoteChar, nullable), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="double" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{856df464-e324-4978-96c6-7056251f60d1}</MetaDataID>
        public override Task WriteValueAsync(double? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? (value.HasValue ? WriteValueAsync(value.GetValueOrDefault(), true, cancellationToken) : WriteNullAsync(cancellationToken)) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="float" /> value.
        /// </summary>
        /// <param name="value">The <see cref="float" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{f672aad9-96b7-4c0c-9060-738d61472f5f}</MetaDataID>
        public override Task WriteValueAsync(float value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteValueAsync(value, false, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{77dede55-9fb1-4b2c-b4f2-0c9533506efb}</MetaDataID>
        internal Task WriteValueAsync(float value, bool nullable, CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value, FloatFormatHandling, QuoteChar, nullable), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="float" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="float" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{86224501-d0b1-4603-8fd0-67a7f141baed}</MetaDataID>
        public override Task WriteValueAsync(float? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? (value.HasValue ? WriteValueAsync(value.GetValueOrDefault(), true, cancellationToken) : WriteNullAsync(cancellationToken)) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Guid" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Guid" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{e802bf0e-8f02-4ae1-86a2-191b4a6952ac}</MetaDataID>
        public override Task WriteValueAsync(Guid value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{d0dee754-df7e-4795-a574-9bd54598b16e}</MetaDataID>
        internal async Task DoWriteValueAsync(Guid value, CancellationToken cancellationToken)
        {
            await InternalWriteValueAsync(JsonToken.String, cancellationToken).ConfigureAwait(false);

            await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
#if HAVE_CHAR_TO_STRING_WITH_CULTURE
            await _writer.WriteAsync(value.ToString("D", CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
#else
            await _writer.WriteAsync(value.ToString("D"), cancellationToken).ConfigureAwait(false);
#endif
            await _writer.WriteAsync(_quoteChar).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="Guid" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="Guid" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{ccac383c-333d-4fe7-adef-429014d4e8dd}</MetaDataID>
        public override Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{70f0099a-697b-4682-a116-5a019e357726}</MetaDataID>
        internal Task DoWriteValueAsync(Guid? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="int" /> value.
        /// </summary>
        /// <param name="value">The <see cref="int" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{84f3369b-fd42-4418-ac66-f0f2b3d5b667}</MetaDataID>
        public override Task WriteValueAsync(int value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="int" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="int" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{1dcbb7c9-15c7-492c-9b26-d59057e6a555}</MetaDataID>
        public override Task WriteValueAsync(int? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{4791d9b0-09ce-4849-b755-1eca5cbc6ec5}</MetaDataID>
        internal Task DoWriteValueAsync(int? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The <see cref="long" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{eb480f11-0a5c-4ef9-b8fd-1043eb96c79e}</MetaDataID>
        public override Task WriteValueAsync(long value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="long" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{0e5a52d7-3f78-4a17-bc79-c21e001dbc4d}</MetaDataID>
        public override Task WriteValueAsync(long? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{91b96ae2-7680-4c25-a2d0-7180bc7efeb9}</MetaDataID>
        internal Task DoWriteValueAsync(long? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

#if HAVE_BIG_INTEGER
        /// <MetaDataID>{50668c1e-465f-4d0a-80c0-2f74866ff360}</MetaDataID>
        internal Task WriteValueAsync(BigInteger value, CancellationToken cancellationToken)
        {
            return WriteValueInternalAsync(JsonToken.Integer, value.ToString(CultureInfo.InvariantCulture), cancellationToken);
        }
#endif

        /// <summary>
        /// Asynchronously writes a <see cref="object" /> value.
        /// </summary>
        /// <param name="value">The <see cref="object" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{4d1f6cb8-a761-4d21-bd85-2f41027f6567}</MetaDataID>
        public override Task WriteValueAsync(object value, CancellationToken cancellationToken = default)
        {
            if (_safeAsync)
            {
                if (value == null)
                {
                    return WriteNullAsync(cancellationToken);
                }
#if HAVE_BIG_INTEGER
                if (value is BigInteger i)
                {
                    return WriteValueAsync(i, cancellationToken);
                }
#endif

                return WriteValueAsync(this, ConvertUtils.GetTypeCode(value.GetType()), value, cancellationToken);
            }

            return base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="sbyte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="sbyte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{28b60663-9f83-4ff4-91c9-eeb198ad8e8c}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="sbyte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="sbyte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{50d6d961-3312-4f64-ab92-4edf922b1191}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{290ffbdd-e30b-454e-9138-005ac6a98131}</MetaDataID>
        internal Task DoWriteValueAsync(sbyte? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="short" /> value.
        /// </summary>
        /// <param name="value">The <see cref="short" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{a25e002d-82f5-4730-a45f-9e8c15458c56}</MetaDataID>
        public override Task WriteValueAsync(short value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="short" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="short" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{e41386c1-016b-447d-943c-5c688663221d}</MetaDataID>
        public override Task WriteValueAsync(short? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{aa6e6d13-5531-4537-8bc5-e939fc61a7a3}</MetaDataID>
        internal Task DoWriteValueAsync(short? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="string" /> value.
        /// </summary>
        /// <param name="value">The <see cref="string" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{812910c9-7b33-467b-9720-7195f88db259}</MetaDataID>
        public override Task WriteValueAsync(string value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{c3a2a704-5976-4488-aa44-8195d452c6c9}</MetaDataID>
        internal Task DoWriteValueAsync(string value, CancellationToken cancellationToken)
        {
            Task task = InternalWriteValueAsync(JsonToken.String, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return value == null ? _writer.WriteAsync(JsonConvert.Null, cancellationToken) : WriteEscapedStringAsync(value, true, cancellationToken);
            }

            return DoWriteValueAsync(task, value, cancellationToken);
        }

        /// <MetaDataID>{6405336f-1625-4a95-bcfc-e436ac424a14}</MetaDataID>
        private async Task DoWriteValueAsync(Task task, string value, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await (value == null ? _writer.WriteAsync(JsonConvert.Null, cancellationToken) : WriteEscapedStringAsync(value, true, cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{f4942117-671c-4c0a-8e10-86b7d651cd8a}</MetaDataID>
        public override Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{7cc8ccb7-f1a8-4c2a-a07b-9a9aa5d76914}</MetaDataID>
        internal async Task DoWriteValueAsync(TimeSpan value, CancellationToken cancellationToken)
        {
            await InternalWriteValueAsync(JsonToken.String, cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(_quoteChar, cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(value.ToString(null, CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(_quoteChar, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="TimeSpan" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{a45fb0a5-e45e-4db9-b866-72a75d46372f}</MetaDataID>
        public override Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{6848094f-ba36-4856-9a95-e8186519007d}</MetaDataID>
        internal Task DoWriteValueAsync(TimeSpan? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="uint" /> value.
        /// </summary>
        /// <param name="value">The <see cref="uint" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{de3725ea-18b0-4f85-a720-ad8e6b87e479}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(uint value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="uint" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="uint" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{990d31de-a833-4d1a-86b9-a6a3a328f887}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(uint? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{54a50116-a025-4ea4-8617-a5e6a8974b56}</MetaDataID>
        internal Task DoWriteValueAsync(uint? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="ulong" /> value.
        /// </summary>
        /// <param name="value">The <see cref="ulong" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{db3dd73a-24bf-47b7-9a5a-02ca24f33c72}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(ulong value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="ulong" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="ulong" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{54cc431e-6f75-42f0-ad66-e17e29a558f9}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{48a60984-5342-42fb-a6de-8065fffea2c8}</MetaDataID>
        internal Task DoWriteValueAsync(ulong? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Uri" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Uri" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{7610e8f9-548f-49b8-8b7c-609bdc15761b}</MetaDataID>
        public override Task WriteValueAsync(Uri value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? (value == null ? WriteNullAsync(cancellationToken) : WriteValueNotNullAsync(value, cancellationToken)) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{8f0c70c9-040a-4bb4-8af6-ef55cc96737a}</MetaDataID>
        internal Task WriteValueNotNullAsync(Uri value, CancellationToken cancellationToken)
        {
            Task task = InternalWriteValueAsync(JsonToken.String, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return WriteEscapedStringAsync(value.OriginalString, true, cancellationToken);
            }

            return WriteValueNotNullAsync(task, value, cancellationToken);
        }

        /// <MetaDataID>{70661573-af56-47c4-80a8-b4dfa4a9ca2d}</MetaDataID>
        internal async Task WriteValueNotNullAsync(Task task, Uri value, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await WriteEscapedStringAsync(value.OriginalString, true, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="ushort" /> value.
        /// </summary>
        /// <param name="value">The <see cref="ushort" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{b0b9cf7b-a8ae-4b31-8074-d026388b0a94}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(ushort value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? WriteIntegerValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="ushort" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="ushort" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{a9e8045d-79eb-4fbe-bd2a-52313d469d4e}</MetaDataID>
        [CLSCompliant(false)]
        public override Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteValueAsync(value, cancellationToken) : base.WriteValueAsync(value, cancellationToken);
        }

        /// <MetaDataID>{104414b0-32ca-4bd5-9f74-e3f9662ab6bf}</MetaDataID>
        internal Task DoWriteValueAsync(ushort? value, CancellationToken cancellationToken)
        {
            return value == null ? DoWriteNullAsync(cancellationToken) : WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a comment <c>/*...*/</c> containing the specified text.
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{5a7f1862-4a0f-429e-8f9d-ef1f75e9ddc8}</MetaDataID>
        public override Task WriteCommentAsync(string text, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteCommentAsync(text, cancellationToken) : base.WriteCommentAsync(text, cancellationToken);
        }

        /// <MetaDataID>{c5a893f7-dfcc-4f46-8e34-855bd15ef710}</MetaDataID>
        internal async Task DoWriteCommentAsync(string text, CancellationToken cancellationToken)
        {
            await InternalWriteCommentAsync(cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync("/*", cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(text, cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync("*/", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes the end of an array.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{9fb402c6-1301-49f5-8ba9-f8e9f6450f8f}</MetaDataID>
        public override Task WriteEndArrayAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? InternalWriteEndAsync(JsonContainerType.Array, cancellationToken) : base.WriteEndArrayAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the end of a constructor.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{8cf2b02f-e231-450e-a049-d62f5a1cf284}</MetaDataID>
        public override Task WriteEndConstructorAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken) : base.WriteEndConstructorAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the end of a JSON object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{86dddef2-5542-4f59-bbfd-d8910fc861b1}</MetaDataID>
        public override Task WriteEndObjectAsync(CancellationToken cancellationToken = default)
        {
            return _safeAsync ? InternalWriteEndAsync(JsonContainerType.Object, cancellationToken) : base.WriteEndObjectAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes raw JSON where a value is expected and updates the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>Derived classes must override this method to get asynchronous behaviour. Otherwise it will
        /// execute synchronously, returning an already-completed task.</remarks>
        /// <MetaDataID>{a5cd97d6-9697-43d7-84dd-d38db2a6ce3e}</MetaDataID>
        public override Task WriteRawValueAsync(string json, CancellationToken cancellationToken = default)
        {
            return _safeAsync ? DoWriteRawValueAsync(json, cancellationToken) : base.WriteRawValueAsync(json, cancellationToken);
        }

        /// <MetaDataID>{503ecfcc-8240-4139-a069-8cad88dc4286}</MetaDataID>
        internal Task DoWriteRawValueAsync(string json, CancellationToken cancellationToken)
        {
            UpdateScopeWithFinishedValue();
            Task task = AutoCompleteAsync(JsonToken.Undefined, cancellationToken);
            if (task.IsCompletedSucessfully())
            {
                return WriteRawAsync(json, cancellationToken);
            }

            return DoWriteRawValueAsync(task, json, cancellationToken);
        }

        /// <MetaDataID>{a1834e1a-420a-4ffa-abc6-3f2559dd9c4e}</MetaDataID>
        private async Task DoWriteRawValueAsync(Task task, string json, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await WriteRawAsync(json, cancellationToken).ConfigureAwait(false);
        }

        /// <MetaDataID>{00fe95a5-62c4-4424-b18b-8a4302f9a995}</MetaDataID>
        internal char[] EnsureWriteBuffer(int length, int copyTo)
        {
            if (length < 35)
            {
                length = 35;
            }

            char[] buffer = _writeBuffer;
            if (buffer == null)
            {
                return _writeBuffer = BufferUtils.RentBuffer(_arrayPool, length);
            }

            if (buffer.Length >= length)
            {
                return buffer;
            }

            char[] newBuffer = BufferUtils.RentBuffer(_arrayPool, length);
            if (copyTo != 0)
            {
                Array.Copy(buffer, newBuffer, copyTo);
            }

            BufferUtils.ReturnBuffer(_arrayPool, buffer);
            _writeBuffer = newBuffer;
            return newBuffer;
        }
    }
}
#endif