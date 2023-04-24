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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Json
{
    /// <MetaDataID>OOAdvantech.Json.JsonReader</MetaDataID>
    public abstract partial class JsonReader
    {
        /// <summary>
        /// Asynchronously reads the next JSON token from the source.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns <c>true</c> if the next token was read successfully; <c>false</c> if there are no more tokens to read.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{bae614b7-87eb-4138-999b-0045a85b1a78}</MetaDataID>
        public virtual Task<bool> ReadAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<bool>() ?? Read().ToAsync();
        }

        /// <summary>
        /// Asynchronously skips the children of the current token.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{7e11ca49-d677-4d02-b966-06c8d125d496}</MetaDataID>
        public async Task SkipAsync(CancellationToken cancellationToken = default)
        {
            if (TokenType == JsonToken.PropertyName)
            {
                await ReadAsync(cancellationToken).ConfigureAwait(false);
            }

            if (JsonTokenUtils.IsStartToken(TokenType))
            {
                int depth = Depth;

                while (await ReadAsync(cancellationToken).ConfigureAwait(false) && depth < Depth)
                {
                }
            }
        }

        /// <MetaDataID>{80410f50-d648-4797-9d94-be285e6cee05}</MetaDataID>
        internal async Task ReaderReadAndAssertAsync(CancellationToken cancellationToken)
        {
            if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                throw CreateUnexpectedEndException();
            }
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="bool" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="bool" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{c73fb3c3-8e7c-4501-8bed-dbd95a53ed1b}</MetaDataID>
        public virtual Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<bool?>() ?? Task.FromResult(ReadAsBoolean());
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="byte" />[].
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="byte" />[]. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{1d2bb2ae-98c8-4f0f-909c-edbdac3abbbd}</MetaDataID>
        public virtual Task<byte[]> ReadAsBytesAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<byte[]>() ?? Task.FromResult(ReadAsBytes());
        }

        /// <MetaDataID>{8b2b9161-457f-4194-9bb7-2799f83c0ddc}</MetaDataID>
        internal async Task<byte[]> ReadArrayIntoByteArrayAsync(CancellationToken cancellationToken)
        {
            List<byte> buffer = new List<byte>();

            while (true)
            {
                if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
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

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="DateTime" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="DateTime" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{ecd73d1f-8894-4214-9650-c0a58fd8684b}</MetaDataID>
        public virtual Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<DateTime?>() ?? Task.FromResult(ReadAsDateTime());
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="DateTimeOffset" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{c8a1564f-d13c-44d8-a0e0-763ad68b0d79}</MetaDataID>
        public virtual Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<DateTimeOffset?>() ?? Task.FromResult(ReadAsDateTimeOffset());
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="decimal" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="decimal" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{d5fcfd65-52a3-47e0-806e-7917346b54b8}</MetaDataID>
        public virtual Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<decimal?>() ?? Task.FromResult(ReadAsDecimal());
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="double" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="double" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{356c3656-d03d-4931-9918-e982e7d50fc5}</MetaDataID>
        public virtual Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(ReadAsDouble());
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="Nullable{T}" /> of <see cref="int" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="Nullable{T}" /> of <see cref="int" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{f03091fd-2f59-44e3-821d-f2f06f27a79c}</MetaDataID>
        public virtual Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<int?>() ?? Task.FromResult(ReadAsInt32());
        }

        /// <summary>
        /// Asynchronously reads the next JSON token from the source as a <see cref="string" />.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous read. The <see cref="Task{TResult}.Result" />
        /// property returns the <see cref="string" />. This result will be <c>null</c> at the end of an array.</returns>
        /// <remarks>The default behaviour is to execute synchronously, returning an already-completed task. Derived
        /// classes can override this behaviour for true asynchronicity.</remarks>
        /// <MetaDataID>{77a14257-ee2a-4e30-8190-26e3a50f856e}</MetaDataID>
        public virtual Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.CancelIfRequestedAsync<string>() ?? Task.FromResult(ReadAsString());
        }

        /// <MetaDataID>{643e0c4d-7b63-40f7-8d1b-9509a7a3974c}</MetaDataID>
        internal async Task<bool> ReadAndMoveToContentAsync(CancellationToken cancellationToken)
        {
            return await ReadAsync(cancellationToken).ConfigureAwait(false) && await MoveToContentAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <MetaDataID>{2887d736-c32a-41e4-80c7-6d252245d377}</MetaDataID>
        internal Task<bool> MoveToContentAsync(CancellationToken cancellationToken)
        {
            switch (TokenType)
            {
                case JsonToken.None:
                case JsonToken.Comment:
                    return MoveToContentFromNonContentAsync(cancellationToken);
                default:
                    return AsyncUtils.True;
            }
        }

        /// <MetaDataID>{143d5b6b-57ea-4215-a297-8f88b1948b0c}</MetaDataID>
        private async Task<bool> MoveToContentFromNonContentAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    return false;
                }

                switch (TokenType)
                {
                    case JsonToken.None:
                    case JsonToken.Comment:
                        break;
                    default:
                        return true;
                }
            }
        }
    }
}

#endif
