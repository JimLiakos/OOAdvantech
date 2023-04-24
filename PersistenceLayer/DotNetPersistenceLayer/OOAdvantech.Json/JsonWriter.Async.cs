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
    public abstract partial class JsonWriter
    {
        /// <MetaDataID>{4ab36c2f-8d09-47eb-afcf-2975caa1ab95}</MetaDataID>
        internal Task AutoCompleteAsync(JsonToken tokenBeingWritten, CancellationToken cancellationToken)
        {
            State oldState = _currentState;

            // gets new state based on the current state and what is being written
            State newState = StateArray[(int)tokenBeingWritten][(int)oldState];

            if (newState == State.Error)
            {
                throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), oldState.ToString()), null);
            }

            _currentState = newState;

            if (_formatting == Formatting.Indented)
            {
                switch (oldState)
                {
                    case State.Start:
                        break;
                    case State.Property:
                        return WriteIndentSpaceAsync(cancellationToken);
                    case State.ArrayStart:
                    case State.ConstructorStart:
                        return WriteIndentAsync(cancellationToken);
                    case State.Array:
                    case State.Constructor:
                        return tokenBeingWritten == JsonToken.Comment ? WriteIndentAsync(cancellationToken) : AutoCompleteAsync(cancellationToken);
                    case State.Object:
                        switch (tokenBeingWritten)
                        {
                            case JsonToken.Comment:
                                break;
                            case JsonToken.PropertyName:
                                return AutoCompleteAsync(cancellationToken);
                            default:
                                return WriteValueDelimiterAsync(cancellationToken);
                        }

                        break;
                    default:
                        if (tokenBeingWritten == JsonToken.PropertyName)
                        {
                            return WriteIndentAsync(cancellationToken);
                        }

                        break;
                }
            }
            else if (tokenBeingWritten != JsonToken.Comment)
            {
                switch (oldState)
                {
                    case State.Object:
                    case State.Array:
                    case State.Constructor:
                        return WriteValueDelimiterAsync(cancellationToken);
                }
            }

            return AsyncUtils.CompletedTask;
        }

        /// <MetaDataID>{40808148-2038-462e-a178-4654c294c237}</MetaDataID>
        private async Task AutoCompleteAsync(CancellationToken cancellationToken)
        {
            await WriteValueDelimiterAsync(cancellationToken).ConfigureAwait(false);
            await WriteIndentAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously closes this writer.
        /// If <see cref="JsonWriter.CloseOutput" /> is set to <c>true</c>, the destination is also closed.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{6275814e-c5e2-4d68-b29a-7d14fe83352d}</MetaDataID>
        public virtual Task CloseAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            Close();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously flushes whatever is in the buffer to the destination and also flushes the destination.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{4eef7d5e-7cb4-4b1b-848c-88e0a8b59cdc}</MetaDataID>
        public virtual Task FlushAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            Flush();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the specified end token.
        /// </summary>
        /// <param name="token">The end token to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d3f6e38b-b3cc-4f0a-b1b2-306afee63642}</MetaDataID>
        protected virtual Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteEnd(token);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes indent characters.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{8fa51e76-d2bb-489c-89f8-a585979bcb08}</MetaDataID>
        protected virtual Task WriteIndentAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteIndent();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the JSON value delimiter.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{17f981ee-cf0e-4432-b925-fed4b17960a6}</MetaDataID>
        protected virtual Task WriteValueDelimiterAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValueDelimiter();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes an indent space.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{10751547-cb80-4976-8085-ceb5eee06c86}</MetaDataID>
        protected virtual Task WriteIndentSpaceAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteIndentSpace();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes raw JSON without changing the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{2be1c7ba-cbcc-4c65-b193-222d905c3063}</MetaDataID>
        public virtual Task WriteRawAsync(string json, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteRaw(json);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the end of the current JSON object or array.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d2b8f841-1f6e-451a-8cb3-979b3975bbfb}</MetaDataID>
        public virtual Task WriteEndAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteEnd();
            return AsyncUtils.CompletedTask;
        }

        /// <MetaDataID>{5440f533-3314-408d-ba0b-f8a05ef2701d}</MetaDataID>
        internal Task WriteEndInternalAsync(CancellationToken cancellationToken)
        {
            JsonContainerType type = Peek();
            switch (type)
            {
                case JsonContainerType.Object:
                    return WriteEndObjectAsync(cancellationToken);
                case JsonContainerType.Array:
                    return WriteEndArrayAsync(cancellationToken);
                case JsonContainerType.Constructor:
                    return WriteEndConstructorAsync(cancellationToken);
                default:
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return cancellationToken.FromCanceled();
                    }

                    throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type, null);
            }
        }

        /// <MetaDataID>{2c9ddb87-b079-46be-889b-edd1e07808aa}</MetaDataID>
        internal Task InternalWriteEndAsync(JsonContainerType type, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            int levelsToComplete = CalculateLevelsToComplete(type);
            while (levelsToComplete-- > 0)
            {
                JsonToken token = GetCloseTokenForType(Pop());

                Task t;
                if (_currentState == State.Property)
                {
                    t = WriteNullAsync(cancellationToken);
                    if (!t.IsCompletedSucessfully())
                    {
                        return AwaitProperty(t, levelsToComplete, token, cancellationToken);
                    }
                }

                if (_formatting == Formatting.Indented)
                {
                    if (_currentState != State.ObjectStart && _currentState != State.ArrayStart)
                    {
                        t = WriteIndentAsync(cancellationToken);
                        if (!t.IsCompletedSucessfully())
                        {
                            return AwaitIndent(t, levelsToComplete, token, cancellationToken);
                        }
                    }
                }

                t = WriteEndAsync(token, cancellationToken);
                if (!t.IsCompletedSucessfully())
                {
                    return AwaitEnd(t, levelsToComplete, cancellationToken);
                }

                UpdateCurrentState();
            }

            return AsyncUtils.CompletedTask;

            // Local functions, params renamed (capitalized) so as not to capture and allocate when calling async
            async Task AwaitProperty(Task task, int LevelsToComplete, JsonToken token, CancellationToken CancellationToken)
            {
                await task.ConfigureAwait(false);

                //  Finish current loop
                if (_formatting == Formatting.Indented)
                {
                    if (_currentState != State.ObjectStart && _currentState != State.ArrayStart)
                    {
                        await WriteIndentAsync(CancellationToken).ConfigureAwait(false);
                    }
                }

                await WriteEndAsync(token, CancellationToken).ConfigureAwait(false);

                UpdateCurrentState();

                await AwaitRemaining(LevelsToComplete, CancellationToken).ConfigureAwait(false);
            }

            async Task AwaitIndent(Task task, int LevelsToComplete, JsonToken token, CancellationToken CancellationToken)
            {
                await task.ConfigureAwait(false);

                //  Finish current loop

                await WriteEndAsync(token, CancellationToken).ConfigureAwait(false);

                UpdateCurrentState();

                await AwaitRemaining(LevelsToComplete, CancellationToken).ConfigureAwait(false);
            }

            async Task AwaitEnd(Task task, int LevelsToComplete, CancellationToken CancellationToken)
            {
                await task.ConfigureAwait(false);

                //  Finish current loop

                UpdateCurrentState();

                await AwaitRemaining(LevelsToComplete, CancellationToken).ConfigureAwait(false);
            }

            async Task AwaitRemaining(int LevelsToComplete, CancellationToken CancellationToken)
            {
                while (LevelsToComplete-- > 0)
                {
                    JsonToken token = GetCloseTokenForType(Pop());

                    if (_currentState == State.Property)
                    {
                        await WriteNullAsync(CancellationToken).ConfigureAwait(false);
                    }

                    if (_formatting == Formatting.Indented)
                    {
                        if (_currentState != State.ObjectStart && _currentState != State.ArrayStart)
                        {
                            await WriteIndentAsync(CancellationToken).ConfigureAwait(false);
                        }
                    }

                    await WriteEndAsync(token, CancellationToken).ConfigureAwait(false);

                    UpdateCurrentState();
                }
            }
        }

        /// <summary>
        /// Asynchronously writes the end of an array.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{df7d24d0-cf1d-4d9b-8e95-e0d99371fec1}</MetaDataID>
        public virtual Task WriteEndArrayAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteEndArray();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the end of a constructor.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{c72684fd-b699-440e-8bd8-c4ff5d07b89b}</MetaDataID>
        public virtual Task WriteEndConstructorAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteEndConstructor();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the end of a JSON object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d16724c1-fde0-4766-b314-b57d93194d51}</MetaDataID>
        public virtual Task WriteEndObjectAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteEndObject();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a null value.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{38ce291d-5530-4ec1-9984-b66d49540727}</MetaDataID>
        public virtual Task WriteNullAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteNull();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{77805a9d-a335-4efd-b4ee-a4e80a209c6f}</MetaDataID>
        public virtual Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WritePropertyName(name);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="escape">A flag to indicate whether the text should be escaped when it is written as a JSON property name.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{2e6db7af-3d2d-4893-8e80-64b81ae3be50}</MetaDataID>
        public virtual Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WritePropertyName(name, escape);
            return AsyncUtils.CompletedTask;
        }

        /// <MetaDataID>{ab6bf09f-b0f9-48b7-949c-7a6e141e3224}</MetaDataID>
        internal Task InternalWritePropertyNameAsync(string name, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            _currentPosition.PropertyName = name;
            return AutoCompleteAsync(JsonToken.PropertyName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the beginning of a JSON array.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{0e50c039-4851-4bbd-ad70-c3efe0914b0c}</MetaDataID>
        public virtual Task WriteStartArrayAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteStartArray();
            return AsyncUtils.CompletedTask;
        }

        /// <MetaDataID>{79a18176-1eef-4d22-a90a-01abe11c4e67}</MetaDataID>
        internal async Task InternalWriteStartAsync(JsonToken token, JsonContainerType container, CancellationToken cancellationToken)
        {
            UpdateScopeWithFinishedValue();
            await AutoCompleteAsync(token, cancellationToken).ConfigureAwait(false);
            Push(container);
        }

        /// <summary>
        /// Asynchronously writes a comment <c>/*...*/</c> containing the specified text.
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d6dd43b6-56bd-44e3-8f85-d25551d42cde}</MetaDataID>
        public virtual Task WriteCommentAsync(string text, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteComment(text);
            return AsyncUtils.CompletedTask;
        }

        /// <MetaDataID>{447de89e-9b09-4eba-87e7-8f4b0b71b6ef}</MetaDataID>
        internal Task InternalWriteCommentAsync(CancellationToken cancellationToken)
        {
            return AutoCompleteAsync(JsonToken.Comment, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes raw JSON where a value is expected and updates the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d45ee19f-de2b-4cd7-b5d7-1181aac429c6}</MetaDataID>
        public virtual Task WriteRawValueAsync(string json, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteRawValue(json);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the start of a constructor with the given name.
        /// </summary>
        /// <param name="name">The name of the constructor.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{771f9135-5d80-4985-9f4d-2a0deb99bdea}</MetaDataID>
        public virtual Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteStartConstructor(name);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the beginning of a JSON object.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{9ba4584b-8e84-4484-adb6-5a7803c7df05}</MetaDataID>
        public virtual Task WriteStartObjectAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteStartObject();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the current <see cref="JsonReader" /> token.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read the token from.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d1137b42-0ea5-4b8c-bc45-21fb387a7a90}</MetaDataID>
        public Task WriteTokenAsync(JsonReader reader, CancellationToken cancellationToken = default)
        {
            return WriteTokenAsync(reader, true, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the current <see cref="JsonReader" /> token.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read the token from.</param>
        /// <param name="writeChildren">A flag indicating whether the current token's children should be written.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{2af90177-f56f-41ae-b557-d497d04b048e}</MetaDataID>
        public Task WriteTokenAsync(JsonReader reader, bool writeChildren, CancellationToken cancellationToken = default)
        {
            ValidationUtils.ArgumentNotNull(reader, nameof(reader));

            return WriteTokenAsync(reader, writeChildren, true, true, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the <see cref="JsonToken" /> token and its value.
        /// </summary>
        /// <param name="token">The <see cref="JsonToken" /> to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{8a84c53b-df4b-4a7c-b783-edb1e5a6cde3}</MetaDataID>
        public Task WriteTokenAsync(JsonToken token, CancellationToken cancellationToken = default)
        {
            return WriteTokenAsync(token, null, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes the <see cref="JsonToken" /> token and its value.
        /// </summary>
        /// <param name="token">The <see cref="JsonToken" /> to write.</param>
        /// <param name="value">
        /// The value to write.
        /// A value is only required for tokens that have an associated value, e.g. the <see cref="String" /> property name for <see cref="JsonToken.PropertyName" />.
        /// <c>null</c> can be passed to the method for tokens that don't have a value, e.g. <see cref="JsonToken.StartObject" />.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{2253d48a-5191-4a0e-bcb0-d3d887512ff1}</MetaDataID>
        public Task WriteTokenAsync(JsonToken token, object value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            switch (token)
            {
                case JsonToken.None:
                    // read to next
                    return AsyncUtils.CompletedTask;
                case JsonToken.StartObject:
                    return WriteStartObjectAsync(cancellationToken);
                case JsonToken.StartArray:
                    return WriteStartArrayAsync(cancellationToken);
                case JsonToken.StartConstructor:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    return WriteStartConstructorAsync(value.ToString(), cancellationToken);
                case JsonToken.PropertyName:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    return WritePropertyNameAsync(value.ToString(), cancellationToken);
                case JsonToken.Comment:
                    return WriteCommentAsync(value?.ToString(), cancellationToken);
                case JsonToken.Integer:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    return
#if HAVE_BIG_INTEGER
                        value is BigInteger integer ? WriteValueAsync(integer, cancellationToken) :
#endif
                        WriteValueAsync(Convert.ToInt64(value, CultureInfo.InvariantCulture), cancellationToken);
                case JsonToken.Float:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    if (value is decimal dec)
                    {
                        return WriteValueAsync(dec, cancellationToken);
                    }

                    if (value is double doub)
                    {
                        return WriteValueAsync(doub, cancellationToken);
                    }

                    if (value is float f)
                    {
                        return WriteValueAsync(f, cancellationToken);
                    }

                    return WriteValueAsync(Convert.ToDouble(value, CultureInfo.InvariantCulture), cancellationToken);
                case JsonToken.String:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    return WriteValueAsync(value.ToString(), cancellationToken);
                case JsonToken.Boolean:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    return WriteValueAsync(Convert.ToBoolean(value, CultureInfo.InvariantCulture), cancellationToken);
                case JsonToken.Null:
                    return WriteNullAsync(cancellationToken);
                case JsonToken.Undefined:
                    return WriteUndefinedAsync(cancellationToken);
                case JsonToken.EndObject:
                    return WriteEndObjectAsync(cancellationToken);
                case JsonToken.EndArray:
                    return WriteEndArrayAsync(cancellationToken);
                case JsonToken.EndConstructor:
                    return WriteEndConstructorAsync(cancellationToken);
                case JsonToken.Date:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    if (value is DateTimeOffset offset)
                    {
                        return WriteValueAsync(offset, cancellationToken);
                    }

                    return WriteValueAsync(Convert.ToDateTime(value, CultureInfo.InvariantCulture), cancellationToken);
                case JsonToken.Raw:
                    return WriteRawValueAsync(value?.ToString(), cancellationToken);
                case JsonToken.Bytes:
                    ValidationUtils.ArgumentNotNull(value, nameof(value));
                    if (value is Guid guid)
                    {
                        return WriteValueAsync(guid, cancellationToken);
                    }

                    return WriteValueAsync((byte[])value, cancellationToken);
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof(token), token, "Unexpected token type.");
            }
        }

        /// <MetaDataID>{9b6043eb-c971-41a5-b0d9-3ae56cc8520a}</MetaDataID>
        internal virtual async Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
        {
            int initialDepth = CalculateWriteTokenInitialDepth(reader);

            do
            {
                // write a JValue date when the constructor is for a date
                if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
                {
                    await WriteConstructorDateAsync(reader, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    if (writeComments || reader.TokenType != JsonToken.Comment)
                    {
                        await WriteTokenAsync(reader.TokenType, reader.Value, cancellationToken).ConfigureAwait(false);
                    }
                }
            } while (
                // stop if we have reached the end of the token being read
                initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0)
                && writeChildren
                && await reader.ReadAsync(cancellationToken).ConfigureAwait(false));

            if (initialDepth < CalculateWriteTokenFinalDepth(reader))
            {
                throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
            }
        }

        // For internal use, when we know the writer does not offer true async support (e.g. when backed
        // by a StringWriter) and therefore async write methods are always in practice just a less efficient
        // path through the sync version.
        /// <MetaDataID>{cee41cc5-10dd-4b37-ad5b-ea53fdc861c0}</MetaDataID>
        internal async Task WriteTokenSyncReadingAsync(JsonReader reader, CancellationToken cancellationToken)
        {
            int initialDepth = CalculateWriteTokenInitialDepth(reader);

            do
            {
                // write a JValue date when the constructor is for a date
                if (reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
                {
                    WriteConstructorDate(reader);
                }
                else
                {
                    WriteToken(reader.TokenType, reader.Value);
                }
            } while (
                // stop if we have reached the end of the token being read
                initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0)
                && await reader.ReadAsync(cancellationToken).ConfigureAwait(false));

            if (initialDepth < CalculateWriteTokenFinalDepth(reader))
            {
                throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
            }
        }

        /// <MetaDataID>{58185589-87e5-4ca4-9bf1-5127249d8711}</MetaDataID>
        private async Task WriteConstructorDateAsync(JsonReader reader, CancellationToken cancellationToken)
        {
            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
            }
            if (reader.TokenType != JsonToken.Integer)
            {
                throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType, null);
            }

            DateTime date = DateTimeUtils.ConvertJavaScriptTicksToDateTime((long)reader.Value);

            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
            }
            if (reader.TokenType != JsonToken.EndConstructor)
            {
                throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + reader.TokenType, null);
            }

            await WriteValueAsync(date, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="bool" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="bool" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{af7a88e2-1ac7-4b9e-8511-51334df3cb4a}</MetaDataID>
        public virtual Task WriteValueAsync(bool value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="bool" /> value.
        /// </summary>
        /// <param name="value">The <see cref="bool" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{be1821da-9396-4484-9d15-20d19a011d7d}</MetaDataID>
        public virtual Task WriteValueAsync(bool? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="byte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="byte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{458e75bc-d98d-4d0f-a944-6ade08ec8eee}</MetaDataID>
        public virtual Task WriteValueAsync(byte value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="byte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="byte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{58d9e86d-138d-47f1-bf0c-9d5f7848d21c}</MetaDataID>
        public virtual Task WriteValueAsync(byte? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="byte" />[] value.
        /// </summary>
        /// <param name="value">The <see cref="byte" />[] value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{aabd2dc5-5bbd-41f9-b71a-eb85f73f3f67}</MetaDataID>
        public virtual Task WriteValueAsync(byte[] value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="char" /> value.
        /// </summary>
        /// <param name="value">The <see cref="char" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{9eb731d7-423b-4a50-a459-bfdc66074f51}</MetaDataID>
        public virtual Task WriteValueAsync(char value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="char" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="char" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{ef1c2137-4a42-41b8-a4d6-347a29c1c8a3}</MetaDataID>
        public virtual Task WriteValueAsync(char? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{8d5e302c-53e8-4853-8ccd-928957543726}</MetaDataID>
        public virtual Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="DateTime" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{906a72c2-c808-4f11-90cd-e8300fd1d50b}</MetaDataID>
        public virtual Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="DateTimeOffset" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{89ac9f39-de75-4a86-accb-6f30eefc3473}</MetaDataID>
        public virtual Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{e3eba37b-6d3b-4f97-8cfd-626913302b4a}</MetaDataID>
        public virtual Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="decimal" /> value.
        /// </summary>
        /// <param name="value">The <see cref="decimal" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{e79bd3ab-739f-45b8-965f-dd440cd0ff17}</MetaDataID>
        public virtual Task WriteValueAsync(decimal value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="decimal" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="decimal" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{9fc5bf96-498b-445e-9f1d-d6e49aa17059}</MetaDataID>
        public virtual Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="double" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{b1fac426-5c86-4250-ad35-6774b83372a5}</MetaDataID>
        public virtual Task WriteValueAsync(double value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="double" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{552a7cfb-6fc0-4e75-8543-6a97445a36a4}</MetaDataID>
        public virtual Task WriteValueAsync(double? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="float" /> value.
        /// </summary>
        /// <param name="value">The <see cref="float" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{791229be-fb6c-40ae-aa53-ed21c03e3b9f}</MetaDataID>
        public virtual Task WriteValueAsync(float value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="float" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="float" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{2090aef0-94ec-4402-88d7-29f6fba45a62}</MetaDataID>
        public virtual Task WriteValueAsync(float? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Guid" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Guid" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{3f0a2962-a65c-4808-9383-6a7b23b57c31}</MetaDataID>
        public virtual Task WriteValueAsync(Guid value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="Guid" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="Guid" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{3eb14d7e-8d5f-4a21-ae45-b92f4ef50a8e}</MetaDataID>
        public virtual Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="int" /> value.
        /// </summary>
        /// <param name="value">The <see cref="int" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{5f35ccb2-f72a-439a-9b62-548a3c8d9554}</MetaDataID>
        public virtual Task WriteValueAsync(int value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="int" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="int" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{ab22f3c5-7a85-43a4-a50b-a5cffb33e6ea}</MetaDataID>
        public virtual Task WriteValueAsync(int? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The <see cref="long" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{8453d501-4ce5-4a0f-92e9-b59188e2020a}</MetaDataID>
        public virtual Task WriteValueAsync(long value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="long" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{c733d833-ba01-419b-b9db-162aaadd949b}</MetaDataID>
        public virtual Task WriteValueAsync(long? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="object" /> value.
        /// </summary>
        /// <param name="value">The <see cref="object" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{4bba0302-d8b4-4172-bc28-879010cbe2ec}</MetaDataID>
        public virtual Task WriteValueAsync(object value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="sbyte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="sbyte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{e0daf2c8-267a-43b1-9a99-dcc809aa95ab}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="sbyte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="sbyte" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{4873c101-2323-4a50-bf1d-ddacd446e04c}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="short" /> value.
        /// </summary>
        /// <param name="value">The <see cref="short" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{77937493-77b3-49a7-80e7-3b288386b6f6}</MetaDataID>
        public virtual Task WriteValueAsync(short value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="short" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="short" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{749cdb3e-6dbc-4fae-8f1c-1800286a2c0b}</MetaDataID>
        public virtual Task WriteValueAsync(short? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="string" /> value.
        /// </summary>
        /// <param name="value">The <see cref="string" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{83903864-d3f3-46a1-8529-f1bcded4b368}</MetaDataID>
        public virtual Task WriteValueAsync(string value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{2e9a210c-1609-4ed1-a0e6-3656683c738a}</MetaDataID>
        public virtual Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="TimeSpan" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{91d6f572-f172-4d1c-af21-dbb663510bc4}</MetaDataID>
        public virtual Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="uint" /> value.
        /// </summary>
        /// <param name="value">The <see cref="uint" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{023363b5-9ddf-4f19-a4fa-bbd5670aa643}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(uint value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="uint" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="uint" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{97ae7fbc-ff33-4ed4-8db3-24909156b3a7}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(uint? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="ulong" /> value.
        /// </summary>
        /// <param name="value">The <see cref="ulong" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{25747136-ce78-4832-b48a-5a9060a55061}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(ulong value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="ulong" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="ulong" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{0e3789c0-a8d7-4a90-864f-172508185b5d}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Uri" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Uri" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{232ae1fb-e632-4a75-a74e-3441d0428329}</MetaDataID>
        public virtual Task WriteValueAsync(Uri value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="ushort" /> value.
        /// </summary>
        /// <param name="value">The <see cref="ushort" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{c0843f0e-f2a0-4f59-a8fb-c90626c976d8}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(ushort value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes a <see cref="Nullable{T}" /> of <see cref="ushort" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{T}" /> of <see cref="ushort" /> value to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{b25722e3-5d98-4b3c-bce1-1bbc464f44c2}</MetaDataID>
        [CLSCompliant(false)]
        public virtual Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteValue(value);
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes an undefined value.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{65e92087-f5f7-4b2e-95be-e7278dffea95}</MetaDataID>
        public virtual Task WriteUndefinedAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteUndefined();
            return AsyncUtils.CompletedTask;
        }

        /// <summary>
        /// Asynchronously writes the given white space.
        /// </summary>
        /// <param name="ws">The string of white space characters.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{4d9ec0af-df3a-4b64-a16c-0409e770353c}</MetaDataID>
        public virtual Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            WriteWhitespace(ws);
            return AsyncUtils.CompletedTask;
        }

        /// <MetaDataID>{7fffdda5-f76f-4dc5-8575-166ef14057a8}</MetaDataID>
        internal Task InternalWriteValueAsync(JsonToken token, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            UpdateScopeWithFinishedValue();
            return AutoCompleteAsync(token, cancellationToken);
        }

        /// <summary>
        /// Asynchronously ets the state of the <see cref="JsonWriter" />.
        /// </summary>
        /// <param name="token">The <see cref="JsonToken" /> being written.</param>
        /// <param name="value">The value being written.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{7805ce53-ad3d-4fd6-ab92-338ef75850d9}</MetaDataID>
        protected Task SetWriteStateAsync(JsonToken token, object value, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            switch (token)
            {
                case JsonToken.StartObject:
                    return InternalWriteStartAsync(token, JsonContainerType.Object, cancellationToken);
                case JsonToken.StartArray:
                    return InternalWriteStartAsync(token, JsonContainerType.Array, cancellationToken);
                case JsonToken.StartConstructor:
                    return InternalWriteStartAsync(token, JsonContainerType.Constructor, cancellationToken);
                case JsonToken.PropertyName:
                    if (!(value is string s))
                    {
                        throw new ArgumentException("A name is required when setting property name state.", nameof(value));
                    }

                    return InternalWritePropertyNameAsync(s, cancellationToken);
                case JsonToken.Comment:
                    return InternalWriteCommentAsync(cancellationToken);
                case JsonToken.Raw:
                    return AsyncUtils.CompletedTask;
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Date:
                case JsonToken.Bytes:
                case JsonToken.Null:
                case JsonToken.Undefined:
                    return InternalWriteValueAsync(token, cancellationToken);
                case JsonToken.EndObject:
                    return InternalWriteEndAsync(JsonContainerType.Object, cancellationToken);
                case JsonToken.EndArray:
                    return InternalWriteEndAsync(JsonContainerType.Array, cancellationToken);
                case JsonToken.EndConstructor:
                    return InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken);
                default:
                    throw new ArgumentOutOfRangeException(nameof(token));
            }
        }

        /// <MetaDataID>{22de347e-2e92-4703-a971-390f264232b3}</MetaDataID>
        internal static Task WriteValueAsync(JsonWriter writer, PrimitiveTypeCode typeCode, object value, CancellationToken cancellationToken)
        {
            while (true)
            {
                switch (typeCode)
                {
                    case PrimitiveTypeCode.Char:
                        return writer.WriteValueAsync((char)value, cancellationToken);
                    case PrimitiveTypeCode.CharNullable:
                        return writer.WriteValueAsync(value == null ? (char?)null : (char)value, cancellationToken);
                    case PrimitiveTypeCode.Boolean:
                        return writer.WriteValueAsync((bool)value, cancellationToken);
                    case PrimitiveTypeCode.BooleanNullable:
                        return writer.WriteValueAsync(value == null ? (bool?)null : (bool)value, cancellationToken);
                    case PrimitiveTypeCode.SByte:
                        return writer.WriteValueAsync((sbyte)value, cancellationToken);
                    case PrimitiveTypeCode.SByteNullable:
                        return writer.WriteValueAsync(value == null ? (sbyte?)null : (sbyte)value, cancellationToken);
                    case PrimitiveTypeCode.Int16:
                        return writer.WriteValueAsync((short)value, cancellationToken);
                    case PrimitiveTypeCode.Int16Nullable:
                        return writer.WriteValueAsync(value == null ? (short?)null : (short)value, cancellationToken);
                    case PrimitiveTypeCode.UInt16:
                        return writer.WriteValueAsync((ushort)value, cancellationToken);
                    case PrimitiveTypeCode.UInt16Nullable:
                        return writer.WriteValueAsync(value == null ? (ushort?)null : (ushort)value, cancellationToken);
                    case PrimitiveTypeCode.Int32:
                        return writer.WriteValueAsync((int)value, cancellationToken);
                    case PrimitiveTypeCode.Int32Nullable:
                        return writer.WriteValueAsync(value == null ? (int?)null : (int)value, cancellationToken);
                    case PrimitiveTypeCode.Byte:
                        return writer.WriteValueAsync((byte)value, cancellationToken);
                    case PrimitiveTypeCode.ByteNullable:
                        return writer.WriteValueAsync(value == null ? (byte?)null : (byte)value, cancellationToken);
                    case PrimitiveTypeCode.UInt32:
                        return writer.WriteValueAsync((uint)value, cancellationToken);
                    case PrimitiveTypeCode.UInt32Nullable:
                        return writer.WriteValueAsync(value == null ? (uint?)null : (uint)value, cancellationToken);
                    case PrimitiveTypeCode.Int64:
                        return writer.WriteValueAsync((long)value, cancellationToken);
                    case PrimitiveTypeCode.Int64Nullable:
                        return writer.WriteValueAsync(value == null ? (long?)null : (long)value, cancellationToken);
                    case PrimitiveTypeCode.UInt64:
                        return writer.WriteValueAsync((ulong)value, cancellationToken);
                    case PrimitiveTypeCode.UInt64Nullable:
                        return writer.WriteValueAsync(value == null ? (ulong?)null : (ulong)value, cancellationToken);
                    case PrimitiveTypeCode.Single:
                        return writer.WriteValueAsync((float)value, cancellationToken);
                    case PrimitiveTypeCode.SingleNullable:
                        return writer.WriteValueAsync(value == null ? (float?)null : (float)value, cancellationToken);
                    case PrimitiveTypeCode.Double:
                        return writer.WriteValueAsync((double)value, cancellationToken);
                    case PrimitiveTypeCode.DoubleNullable:
                        return writer.WriteValueAsync(value == null ? (double?)null : (double)value, cancellationToken);
                    case PrimitiveTypeCode.DateTime:
                        return writer.WriteValueAsync((DateTime)value, cancellationToken);
                    case PrimitiveTypeCode.DateTimeNullable:
                        return writer.WriteValueAsync(value == null ? (DateTime?)null : (DateTime)value, cancellationToken);
                    case PrimitiveTypeCode.DateTimeOffset:
                        return writer.WriteValueAsync((DateTimeOffset)value, cancellationToken);
                    case PrimitiveTypeCode.DateTimeOffsetNullable:
                        return writer.WriteValueAsync(value == null ? (DateTimeOffset?)null : (DateTimeOffset)value, cancellationToken);
                    case PrimitiveTypeCode.Decimal:
                        return writer.WriteValueAsync((decimal)value, cancellationToken);
                    case PrimitiveTypeCode.DecimalNullable:
                        return writer.WriteValueAsync(value == null ? (decimal?)null : (decimal)value, cancellationToken);
                    case PrimitiveTypeCode.Guid:
                        return writer.WriteValueAsync((Guid)value, cancellationToken);
                    case PrimitiveTypeCode.GuidNullable:
                        return writer.WriteValueAsync(value == null ? (Guid?)null : (Guid)value, cancellationToken);
                    case PrimitiveTypeCode.TimeSpan:
                        return writer.WriteValueAsync((TimeSpan)value, cancellationToken);
                    case PrimitiveTypeCode.TimeSpanNullable:
                        return writer.WriteValueAsync(value == null ? (TimeSpan?)null : (TimeSpan)value, cancellationToken);
#if HAVE_BIG_INTEGER
                    case PrimitiveTypeCode.BigInteger:

                        // this will call to WriteValueAsync(object)
                        return writer.WriteValueAsync((BigInteger)value, cancellationToken);
                    case PrimitiveTypeCode.BigIntegerNullable:

                        // this will call to WriteValueAsync(object)
                        return writer.WriteValueAsync(value == null ? (BigInteger?)null : (BigInteger)value, cancellationToken);
#endif
                    case PrimitiveTypeCode.Uri:
                        return writer.WriteValueAsync((Uri)value, cancellationToken);
                    case PrimitiveTypeCode.String:
                        return writer.WriteValueAsync((string)value, cancellationToken);
                    case PrimitiveTypeCode.Bytes:
                        return writer.WriteValueAsync((byte[])value, cancellationToken);
#if HAVE_DB_NULL_TYPE_CODE
                    case PrimitiveTypeCode.DBNull:
                        return writer.WriteNullAsync(cancellationToken);
#endif
                    default:
#if HAVE_ICONVERTIBLE
                        if (value is IConvertible convertible)
                        {
                            ResolveConvertibleValue(convertible, out typeCode, out value);
                            continue;
                        }
#endif

                        // write an unknown null value, fix https://github.com/JamesNK/Newtonsoft.Json/issues/1460
                        if (value == null)
                        {
                            return writer.WriteNullAsync(cancellationToken);
                        }

                        throw CreateUnsupportedTypeException(writer, value);
                }
            }
        }
    }
}

#endif
