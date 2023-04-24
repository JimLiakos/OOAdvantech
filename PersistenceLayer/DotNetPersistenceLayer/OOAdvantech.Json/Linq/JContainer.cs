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
using System.Collections.Specialized;
#endif
using System.Threading;
using OOAdvantech.Json.Utilities;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
#if !HAVE_LINQ
using OOAdvantech.Json.Utilities.LinqBridge;
#else
using System.Linq;

#endif

namespace OOAdvantech.Json.Linq
{
    /// <summary>
    /// Represents a token that can contain other tokens.
    /// </summary>
    /// <MetaDataID>{f30d3dbb-4e35-41bf-b90c-da7bd9a45d79}</MetaDataID>
    public abstract partial class JContainer : JToken, IList<JToken>
#if HAVE_COMPONENT_MODEL
        , ITypedList, IBindingList
#endif
        , IList
#if HAVE_INOTIFY_COLLECTION_CHANGED
        , INotifyCollectionChanged
#endif
    {
#if HAVE_COMPONENT_MODEL
        /// <MetaDataID>{7685bbd0-7fc5-41e6-9240-a1cd113fa21e}</MetaDataID>
        internal ListChangedEventHandler _listChanged;
        /// <MetaDataID>{73c7c748-13f0-4363-bcc9-03c1bf7c5012}</MetaDataID>
        internal AddingNewEventHandler _addingNew;

        /// <summary>
        /// Occurs when the list changes or an item in the list changes.
        /// </summary>
        public event ListChangedEventHandler ListChanged
        {
            add => _listChanged += value;
            remove => _listChanged -= value;
        }

        /// <summary>
        /// Occurs before an item is added to the collection.
        /// </summary>
        public event AddingNewEventHandler AddingNew
        {
            add => _addingNew += value;
            remove => _addingNew -= value;
        }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
        /// <MetaDataID>{0b826c36-9281-457d-b4ff-c2fd2d6d8af5}</MetaDataID>
        internal NotifyCollectionChangedEventHandler _collectionChanged;

        /// <summary>
        /// Occurs when the items list of the collection has changed, or the collection is reset.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collectionChanged += value; }
            remove { _collectionChanged -= value; }
        }
#endif

        /// <summary>
        /// Gets the container's children tokens.
        /// </summary>
        /// <value>The container's children tokens.</value>
        /// <MetaDataID>{3bdee8b3-9369-4eea-a818-fde295e009d6}</MetaDataID>
        protected abstract IList<JToken> ChildrenTokens { get; }

        /// <MetaDataID>{9d9a89e4-1977-43b8-869b-c0d355ce1df6}</MetaDataID>
        private object _syncRoot;
#if (HAVE_COMPONENT_MODEL || HAVE_INOTIFY_COLLECTION_CHANGED)
        /// <MetaDataID>{a0717b8c-70a0-4a0d-88f9-32338c3264ea}</MetaDataID>
        private bool _busy;
#endif

        /// <MetaDataID>{9d238deb-2a77-4ec7-91f4-efbfb5a49aad}</MetaDataID>
        internal JContainer()
        {
        }

        /// <MetaDataID>{d35be380-c78e-42ee-94a5-0f356dd8fca5}</MetaDataID>
        internal JContainer(JContainer other)
                    : this()
        {
            ValidationUtils.ArgumentNotNull(other, nameof(other));

            int i = 0;
            foreach (JToken child in other)
            {
                AddInternal(i, child, false);
                i++;
            }
        }

        /// <MetaDataID>{d23b1cdf-678c-432c-9988-2f5d5e5ebf97}</MetaDataID>
        internal void CheckReentrancy()
        {
#if (HAVE_COMPONENT_MODEL || HAVE_INOTIFY_COLLECTION_CHANGED)
            if (_busy)
            {
                throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, GetType()));
            }
#endif
        }

        /// <MetaDataID>{458d0508-69b0-4f82-a0b8-cda867156797}</MetaDataID>
        internal virtual IList<JToken> CreateChildrenCollection()
        {
            return new List<JToken>();
        }

#if HAVE_COMPONENT_MODEL
        /// <summary>
        /// Raises the <see cref="AddingNew" /> event.
        /// </summary>
        /// <param name="e">The <see cref="AddingNewEventArgs" /> instance containing the event data.</param>
        /// <MetaDataID>{b0f38abf-6be7-48e8-a1e1-7a995e31fbfd}</MetaDataID>
        protected virtual void OnAddingNew(AddingNewEventArgs e)
        {
            _addingNew?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ListChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ListChangedEventArgs" /> instance containing the event data.</param>
        /// <MetaDataID>{ad3ec7a3-30e3-4184-ae85-e633100c4a89}</MetaDataID>
        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            ListChangedEventHandler handler = _listChanged;

            if (handler != null)
            {
                _busy = true;
                try
                {
                    handler(this, e);
                }
                finally
                {
                    _busy = false;
                }
            }
        }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
        /// <summary>
        /// Raises the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        /// <MetaDataID>{7f42f5c9-aa15-4753-a95b-be9762316f8b}</MetaDataID>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = _collectionChanged;

            if (handler != null)
            {
                _busy = true;
                try
                {
                    handler(this, e);
                }
                finally
                {
                    _busy = false;
                }
            }
        }
#endif

        /// <summary>
        /// Gets a value indicating whether this token has child tokens.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token has child values; otherwise, <c>false</c>.
        /// </value>
        /// <MetaDataID>{ad447a29-c2db-4715-b3c1-8938aaad0124}</MetaDataID>
        public override bool HasValues => ChildrenTokens.Count > 0;

        /// <MetaDataID>{81afea54-8ec8-4722-bb2f-8bf48d7512c6}</MetaDataID>
        internal bool ContentsEqual(JContainer container)
        {
            if (container == this)
            {
                return true;
            }

            IList<JToken> t1 = ChildrenTokens;
            IList<JToken> t2 = container.ChildrenTokens;

            if (t1.Count != t2.Count)
            {
                return false;
            }

            for (int i = 0; i < t1.Count; i++)
            {
                if (!t1[i].DeepEquals(t2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the first child token of this token.
        /// </summary>
        /// <value>
        /// A <see cref="JToken" /> containing the first child token of the <see cref="JToken" />.
        /// </value>
        /// <MetaDataID>{0107a036-af7e-45b5-b3c0-2b768a062750}</MetaDataID>
        public override JToken First
        {
            get
            {
                IList<JToken> children = ChildrenTokens;
                return (children.Count > 0) ? children[0] : null;
            }
        }

        /// <summary>
        /// Get the last child token of this token.
        /// </summary>
        /// <value>
        /// A <see cref="JToken" /> containing the last child token of the <see cref="JToken" />.
        /// </value>
        /// <MetaDataID>{3b0e1a64-92a0-4c4f-94ed-ccec3452b6bd}</MetaDataID>
        public override JToken Last
        {
            get
            {
                IList<JToken> children = ChildrenTokens;
                int count = children.Count;
                return (count > 0) ? children[count - 1] : null;
            }
        }

        /// <summary>
        /// Returns a collection of the child tokens of this token, in document order.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}" /> of <see cref="JToken" /> containing the child tokens of this <see cref="JToken" />, in document order.
        /// </returns>
        /// <MetaDataID>{1744843a-8f53-4b31-9e42-d232ba9da62a}</MetaDataID>
        public override JEnumerable<JToken> Children()
        {
            return new JEnumerable<JToken>(ChildrenTokens);
        }

        /// <summary>
        /// Returns a collection of the child values of this token, in document order.
        /// </summary>
        /// <typeparam name="T">The type to convert the values to.</typeparam>
        /// <returns>
        /// A <see cref="IEnumerable{T}" /> containing the child values of this <see cref="JToken" />, in document order.
        /// </returns>
        /// <MetaDataID>{84f9ae56-4cf6-4dac-bb23-bf9baddb2b24}</MetaDataID>
        public override IEnumerable<T> Values<T>()
        {
            return ChildrenTokens.Convert<JToken, T>();
        }

        /// <summary>
        /// Returns a collection of the descendant tokens for this token in document order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="JToken" /> containing the descendant tokens of the <see cref="JToken" />.</returns>
        /// <MetaDataID>{b47cec38-4abe-4d01-97ff-c986de1ae76d}</MetaDataID>
        public IEnumerable<JToken> Descendants()
        {
            return GetDescendants(false);
        }

        /// <summary>
        /// Returns a collection of the tokens that contain this token, and all descendant tokens of this token, in document order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="JToken" /> containing this token, and all the descendant tokens of the <see cref="JToken" />.</returns>
        /// <MetaDataID>{5ad9ea7f-d47c-473b-b2e1-4e185e298ed3}</MetaDataID>
        public IEnumerable<JToken> DescendantsAndSelf()
        {
            return GetDescendants(true);
        }

        /// <MetaDataID>{ff203a53-dddd-4ad7-b97b-e6cdf01da9a5}</MetaDataID>
        internal IEnumerable<JToken> GetDescendants(bool self)
        {
            if (self)
            {
                yield return this;
            }

            foreach (JToken o in ChildrenTokens)
            {
                yield return o;
                if (o is JContainer c)
                {
                    foreach (JToken d in c.Descendants())
                    {
                        yield return d;
                    }
                }
            }
        }

        /// <MetaDataID>{2336f53a-1009-4ae8-9b45-8820584a5640}</MetaDataID>
        internal bool IsMultiContent(object content)
        {
            return (content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]));
        }

        /// <MetaDataID>{bd7b27f4-bc82-454d-a3f8-427944e0e83b}</MetaDataID>
        internal JToken EnsureParentToken(JToken item, bool skipParentCheck)
        {
            if (item == null)
            {
                return JValue.CreateNull();
            }

            if (skipParentCheck)
            {
                return item;
            }

            // to avoid a token having multiple parents or creating a recursive loop, create a copy if...
            // the item already has a parent
            // the item is being added to itself
            // the item is being added to the root parent of itself
            if (item.Parent != null || item == this || (item.HasValues && Root == item))
            {
                item = item.CloneToken();
            }

            return item;
        }

        /// <MetaDataID>{23c6fe41-9a9c-41a5-8d1b-a5ad8338fa8f}</MetaDataID>
        internal abstract int IndexOfItem(JToken item);

        /// <MetaDataID>{cafb7db7-0b30-4a80-b479-49fe187e1707}</MetaDataID>
        internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
        {
            IList<JToken> children = ChildrenTokens;

            if (index > children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the List.");
            }

            CheckReentrancy();

            item = EnsureParentToken(item, skipParentCheck);

            JToken previous = (index == 0) ? null : children[index - 1];
            // haven't inserted new token yet so next token is still at the inserting index
            JToken next = (index == children.Count) ? null : children[index];

            ValidateToken(item, null);

            item.Parent = this;

            item.Previous = previous;
            if (previous != null)
            {
                previous.Next = item;
            }

            item.Next = next;
            if (next != null)
            {
                next.Previous = item;
            }

            children.Insert(index, item);

#if HAVE_COMPONENT_MODEL
            if (_listChanged != null)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
            if (_collectionChanged != null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
#endif
        }

        /// <MetaDataID>{393dee86-6c83-4ed2-8d77-df912a2fee01}</MetaDataID>
        internal virtual void RemoveItemAt(int index)
        {
            IList<JToken> children = ChildrenTokens;

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is less than 0.");
            }
            if (index >= children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is equal to or greater than Count.");
            }

            CheckReentrancy();

            JToken item = children[index];
            JToken previous = (index == 0) ? null : children[index - 1];
            JToken next = (index == children.Count - 1) ? null : children[index + 1];

            if (previous != null)
            {
                previous.Next = next;
            }
            if (next != null)
            {
                next.Previous = previous;
            }

            item.Parent = null;
            item.Previous = null;
            item.Next = null;

            children.RemoveAt(index);

#if HAVE_COMPONENT_MODEL
            if (_listChanged != null)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
            }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
            if (_collectionChanged != null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }
#endif
        }

        /// <MetaDataID>{12fd9be8-e610-4983-a268-1a048cb2a0b2}</MetaDataID>
        internal virtual bool RemoveItem(JToken item)
        {
            int index = IndexOfItem(item);
            if (index >= 0)
            {
                RemoveItemAt(index);
                return true;
            }

            return false;
        }

        /// <MetaDataID>{d13fe73c-b610-4c02-946f-ed3f7d55e5fd}</MetaDataID>
        internal virtual JToken GetItem(int index)
        {
            return ChildrenTokens[index];
        }

        /// <MetaDataID>{44c5e873-94e2-4b06-83ca-cb25ece72e6d}</MetaDataID>
        internal virtual void SetItem(int index, JToken item)
        {
            IList<JToken> children = ChildrenTokens;

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is less than 0.");
            }
            if (index >= children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is equal to or greater than Count.");
            }

            JToken existing = children[index];

            if (IsTokenUnchanged(existing, item))
            {
                return;
            }

            CheckReentrancy();

            item = EnsureParentToken(item, false);

            ValidateToken(item, existing);

            JToken previous = (index == 0) ? null : children[index - 1];
            JToken next = (index == children.Count - 1) ? null : children[index + 1];

            item.Parent = this;

            item.Previous = previous;
            if (previous != null)
            {
                previous.Next = item;
            }

            item.Next = next;
            if (next != null)
            {
                next.Previous = item;
            }

            children[index] = item;

            existing.Parent = null;
            existing.Previous = null;
            existing.Next = null;

#if HAVE_COMPONENT_MODEL
            if (_listChanged != null)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
            }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
            if (_collectionChanged != null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, existing, index));
            }
#endif
        }

        /// <MetaDataID>{6090bd78-49e4-436f-b1ce-0a5671fbf0aa}</MetaDataID>
        internal virtual void ClearItems()
        {
            CheckReentrancy();

            IList<JToken> children = ChildrenTokens;

            foreach (JToken item in children)
            {
                item.Parent = null;
                item.Previous = null;
                item.Next = null;
            }

            children.Clear();

#if HAVE_COMPONENT_MODEL
            if (_listChanged != null)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
#endif
#if HAVE_INOTIFY_COLLECTION_CHANGED
            if (_collectionChanged != null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
#endif
        }

        /// <MetaDataID>{a9e0ac67-3cc8-40fe-91a5-72b4f728ef63}</MetaDataID>
        internal virtual void ReplaceItem(JToken existing, JToken replacement)
        {
            if (existing == null || existing.Parent != this)
            {
                return;
            }

            int index = IndexOfItem(existing);
            SetItem(index, replacement);
        }

        /// <MetaDataID>{3b3cd08b-76d5-4981-bf58-ceca8048344f}</MetaDataID>
        internal virtual bool ContainsItem(JToken item)
        {
            return (IndexOfItem(item) != -1);
        }

        /// <MetaDataID>{2a665c0a-4ed9-451b-95c1-b5cdc3fc19c4}</MetaDataID>
        internal virtual void CopyItemsTo(Array array, int arrayIndex)
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
            foreach (JToken token in ChildrenTokens)
            {
                array.SetValue(token, arrayIndex + index);
                index++;
            }
        }

        /// <MetaDataID>{723538a1-ca38-4b26-8fa4-c28a54d8c2da}</MetaDataID>
        internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
        {
            if (currentValue is JValue v1)
            {
                // null will get turned into a JValue of type null
                if (v1.Type == JTokenType.Null && newValue == null)
                {
                    return true;
                }

                return v1.Equals(newValue);
            }

            return false;
        }

        /// <MetaDataID>{3095ed56-08d2-4fe3-abea-93a90ca91596}</MetaDataID>
        internal virtual void ValidateToken(JToken o, JToken existing)
        {
            ValidationUtils.ArgumentNotNull(o, nameof(o));

            if (o.Type == JTokenType.Property)
            {
                throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), GetType()));
            }
        }

        /// <summary>
        /// Adds the specified content as children of this <see cref="JToken" />.
        /// </summary>
        /// <param name="content">The content to be added.</param>
        /// <MetaDataID>{1e775cf6-8973-4936-8654-780b6d7f34e8}</MetaDataID>
        public virtual void Add(object content)
        {
            AddInternal(ChildrenTokens.Count, content, false);
        }

        /// <MetaDataID>{96c3480e-87bc-4809-8123-bdba46e93002}</MetaDataID>
        internal void AddAndSkipParentCheck(JToken token)
        {
            AddInternal(ChildrenTokens.Count, token, true);
        }

        /// <summary>
        /// Adds the specified content as the first children of this <see cref="JToken" />.
        /// </summary>
        /// <param name="content">The content to be added.</param>
        /// <MetaDataID>{7016bfbe-0cbf-47ff-8e6d-d74fab1fccf2}</MetaDataID>
        public void AddFirst(object content)
        {
            AddInternal(0, content, false);
        }

        /// <MetaDataID>{3406d30d-b21a-4a42-898e-54335a48894a}</MetaDataID>
        internal void AddInternal(int index, object content, bool skipParentCheck)
        {
            if (IsMultiContent(content))
            {
                IEnumerable enumerable = (IEnumerable)content;

                int multiIndex = index;
                foreach (object c in enumerable)
                {
                    AddInternal(multiIndex, c, skipParentCheck);
                    multiIndex++;
                }
            }
            else
            {
                JToken item = CreateFromContent(content);

                InsertItem(index, item, skipParentCheck);
            }
        }

        /// <MetaDataID>{c208f708-4cf9-4e14-bec4-dc5a2d927938}</MetaDataID>
        internal static JToken CreateFromContent(object content)
        {
            if (content is JToken token)
            {
                return token;
            }

            return new JValue(content);
        }

        /// <summary>
        /// Creates a <see cref="JsonWriter" /> that can be used to add tokens to the <see cref="JToken" />.
        /// </summary>
        /// <returns>A <see cref="JsonWriter" /> that is ready to have content written to it.</returns>
        /// <MetaDataID>{2a38a12b-58ff-49de-8921-fd700df35f9f}</MetaDataID>
        public JsonWriter CreateWriter()
        {
            return new JTokenWriter(this);
        }

        /// <summary>
        /// Replaces the child nodes of this token with the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <MetaDataID>{67d955e8-6930-4c78-a0c2-2899e147739d}</MetaDataID>
        public void ReplaceAll(object content)
        {
            ClearItems();
            Add(content);
        }

        /// <summary>
        /// Removes the child nodes from this token.
        /// </summary>
        /// <MetaDataID>{c376e6af-5c74-4db9-9161-e13195df1ea9}</MetaDataID>
        public void RemoveAll()
        {
            ClearItems();
        }

        /// <MetaDataID>{db688bf1-8f2a-4b5c-888c-c15424aa7bed}</MetaDataID>
        internal abstract void MergeItem(object content, JsonMergeSettings settings);

        /// <summary>
        /// Merge the specified content into this <see cref="JToken" />.
        /// </summary>
        /// <param name="content">The content to be merged.</param>
        /// <MetaDataID>{3bc5f1da-aae1-42b8-a5a9-1800b71f0dac}</MetaDataID>
        public void Merge(object content)
        {
            MergeItem(content, new JsonMergeSettings());
        }

        /// <summary>
        /// Merge the specified content into this <see cref="JToken" /> using <see cref="JsonMergeSettings" />.
        /// </summary>
        /// <param name="content">The content to be merged.</param>
        /// <param name="settings">The <see cref="JsonMergeSettings" /> used to merge the content.</param>
        /// <MetaDataID>{0b2d297c-9539-45d6-9e84-76ccdc409be0}</MetaDataID>
        public void Merge(object content, JsonMergeSettings settings)
        {
            MergeItem(content, settings);
        }

        /// <MetaDataID>{73d703c6-55f5-47d3-8db9-f910344c2efb}</MetaDataID>
        internal void ReadTokenFrom(JsonReader reader, JsonLoadSettings options)
        {
            int startDepth = reader.Depth;

            if (!reader.Read())
            {
                throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, GetType().Name));
            }

            ReadContentFrom(reader, options);

            int endDepth = reader.Depth;

            if (endDepth > startDepth)
            {
                throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, GetType().Name));
            }
        }

        /// <MetaDataID>{85e54570-0ffe-4466-8dc1-ff0c9c91c0f9}</MetaDataID>
        internal void ReadContentFrom(JsonReader r, JsonLoadSettings settings)
        {
            ValidationUtils.ArgumentNotNull(r, nameof(r));
            IJsonLineInfo lineInfo = r as IJsonLineInfo;

            JContainer parent = this;

            do
            {
                if (parent is JProperty p && p.Value != null)
                {
                    if (parent == this)
                    {
                        return;
                    }

                    parent = parent.Parent;
                }

                switch (r.TokenType)
                {
                    case JsonToken.None:
                        // new reader. move to actual content
                        break;
                    case JsonToken.StartArray:
                        JArray a = new JArray();
                        a.SetLineInfo(lineInfo, settings);
                        parent.Add(a);
                        parent = a;
                        break;

                    case JsonToken.EndArray:
                        if (parent == this)
                        {
                            return;
                        }

                        parent = parent.Parent;
                        break;
                    case JsonToken.StartObject:
                        JObject o = new JObject();
                        o.SetLineInfo(lineInfo, settings);
                        parent.Add(o);
                        parent = o;
                        break;
                    case JsonToken.EndObject:
                        if (parent == this)
                        {
                            return;
                        }

                        parent = parent.Parent;
                        break;
                    case JsonToken.StartConstructor:
                        JConstructor constructor = new JConstructor(r.Value.ToString());
                        constructor.SetLineInfo(lineInfo, settings);
                        parent.Add(constructor);
                        parent = constructor;
                        break;
                    case JsonToken.EndConstructor:
                        if (parent == this)
                        {
                            return;
                        }

                        parent = parent.Parent;
                        break;
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.Date:
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                        JValue v = new JValue(r.Value);
                        v.SetLineInfo(lineInfo, settings);
                        parent.Add(v);
                        break;
                    case JsonToken.Comment:
                        if (settings != null && settings.CommentHandling == CommentHandling.Load)
                        {
                            v = JValue.CreateComment(r.Value.ToString());
                            v.SetLineInfo(lineInfo, settings);
                            parent.Add(v);
                        }
                        break;
                    case JsonToken.Null:
                        v = JValue.CreateNull();
                        v.SetLineInfo(lineInfo, settings);
                        parent.Add(v);
                        break;
                    case JsonToken.Undefined:
                        v = JValue.CreateUndefined();
                        v.SetLineInfo(lineInfo, settings);
                        parent.Add(v);
                        break;
                    case JsonToken.PropertyName:
                        JProperty property = ReadProperty(r, settings, lineInfo, parent);
                        if (property != null)
                        {
                            parent = property;
                        }
                        else
                        {
                            r.Skip();
                        }
                        break;
                    default:
                        throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, r.TokenType));
                }
            } while (r.Read());
        }

        /// <MetaDataID>{7bd9d1ee-7844-4dd5-b64c-6be21f804063}</MetaDataID>
        private static JProperty ReadProperty(JsonReader r, JsonLoadSettings settings, IJsonLineInfo lineInfo, JContainer parent)
        {
            DuplicatePropertyNameHandling duplicatePropertyNameHandling = settings?.DuplicatePropertyNameHandling ?? DuplicatePropertyNameHandling.Replace;

            JObject parentObject = (JObject)parent;
            string propertyName = r.Value.ToString();
            JProperty existingPropertyWithName = parentObject.Property(propertyName, StringComparison.Ordinal);
            if (existingPropertyWithName != null)
            {
                if (duplicatePropertyNameHandling == DuplicatePropertyNameHandling.Ignore)
                {
                    return null;
                }
                else if (duplicatePropertyNameHandling == DuplicatePropertyNameHandling.Error)
                {
                    throw JsonReaderException.Create(r, "Property with the name '{0}' already exists in the current JSON object.".FormatWith(CultureInfo.InvariantCulture, propertyName));
                }
            }

            JProperty property = new JProperty(propertyName);
            property.SetLineInfo(lineInfo, settings);
            // handle multiple properties with the same name in JSON
            if (existingPropertyWithName == null)
            {
                parent.Add(property);
            }
            else
            {
                existingPropertyWithName.Replace(property);
            }

            return property;
        }

        /// <MetaDataID>{a0542f1c-8f0d-4d70-85d5-d8dc16022464}</MetaDataID>
        internal int ContentsHashCode()
        {
            int hashCode = 0;
            foreach (JToken item in ChildrenTokens)
            {
                hashCode ^= item.GetDeepHashCode();
            }
            return hashCode;
        }

#if HAVE_COMPONENT_MODEL
        /// <MetaDataID>{a907681f-bb13-4a72-b73a-73f5c3188293}</MetaDataID>
        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }

        /// <MetaDataID>{18980371-5e99-4132-8e58-afd83d2c5a58}</MetaDataID>
        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            ICustomTypeDescriptor d = First as ICustomTypeDescriptor;
            return d?.GetProperties();
        }
#endif

        #region IList<JToken> Members
        /// <MetaDataID>{e5452a7f-6e8a-4db8-b9f1-11bc7a409eba}</MetaDataID>
        int IList<JToken>.IndexOf(JToken item)
        {
            return IndexOfItem(item);
        }

        /// <MetaDataID>{3c1f0733-af2d-49b2-bb55-58e207d5c640}</MetaDataID>
        void IList<JToken>.Insert(int index, JToken item)
        {
            InsertItem(index, item, false);
        }

        /// <MetaDataID>{f8f6b424-d586-418f-a948-a3318b3fedab}</MetaDataID>
        void IList<JToken>.RemoveAt(int index)
        {
            RemoveItemAt(index);
        }

        /// <MetaDataID>{8b8a52da-cccc-4895-973c-be32a302f892}</MetaDataID>
        JToken IList<JToken>.this[int index]
        {
            get => GetItem(index);
            set => SetItem(index, value);
        }
        #endregion

        #region ICollection<JToken> Members
        /// <MetaDataID>{7240d7d1-945a-4fa3-b771-a39ac9b19256}</MetaDataID>
        void ICollection<JToken>.Add(JToken item)
        {
            Add(item);
        }

        /// <MetaDataID>{41616cd1-1548-467c-a39d-4f0c7b6e3573}</MetaDataID>
        void ICollection<JToken>.Clear()
        {
            ClearItems();
        }

        /// <MetaDataID>{ed7d4c8f-8262-4c8c-8395-8df8f9373451}</MetaDataID>
        bool ICollection<JToken>.Contains(JToken item)
        {
            return ContainsItem(item);
        }

        /// <MetaDataID>{5503e6a7-bb9c-42a9-94e0-fe0f260e663e}</MetaDataID>
        void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
        {
            CopyItemsTo(array, arrayIndex);
        }

        /// <MetaDataID>{c91d2abf-8eef-4586-bf05-6768cb1270f1}</MetaDataID>
        bool ICollection<JToken>.IsReadOnly => false;

        /// <MetaDataID>{850b4b6e-bf5c-423a-918c-a2a62107925f}</MetaDataID>
        bool ICollection<JToken>.Remove(JToken item)
        {
            return RemoveItem(item);
        }
        #endregion

        /// <MetaDataID>{06a37af4-1e19-4eff-b722-12ab28a28adf}</MetaDataID>
        private JToken EnsureValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is JToken token)
            {
                return token;
            }

            throw new ArgumentException("Argument is not a JToken.");
        }

        #region IList Members
        /// <MetaDataID>{94328299-7e5d-4edb-be58-1426aba8211e}</MetaDataID>
        int IList.Add(object value)
        {
            Add(EnsureValue(value));
            return Count - 1;
        }

        /// <MetaDataID>{1425a70f-3306-4988-882f-d023a2d96055}</MetaDataID>
        void IList.Clear()
        {
            ClearItems();
        }

        /// <MetaDataID>{f9829147-5829-4111-bbb4-ed943c21a28b}</MetaDataID>
        bool IList.Contains(object value)
        {
            return ContainsItem(EnsureValue(value));
        }

        /// <MetaDataID>{a827a4cc-52d8-499e-81ec-2b8ddf71d726}</MetaDataID>
        int IList.IndexOf(object value)
        {
            return IndexOfItem(EnsureValue(value));
        }

        /// <MetaDataID>{f35e8690-50d5-4125-9d4e-346876dc7ada}</MetaDataID>
        void IList.Insert(int index, object value)
        {
            InsertItem(index, EnsureValue(value), false);
        }

        /// <MetaDataID>{6429b540-e78c-4b05-bda6-c7b9083893b8}</MetaDataID>
        bool IList.IsFixedSize => false;

        /// <MetaDataID>{eb7f3786-a29c-47d0-9d04-f085752e414d}</MetaDataID>
        bool IList.IsReadOnly => false;

        /// <MetaDataID>{be275857-fe2f-409b-b247-9123b9f39d8c}</MetaDataID>
        void IList.Remove(object value)
        {
            RemoveItem(EnsureValue(value));
        }

        /// <MetaDataID>{3b6f8f4a-8bc7-4138-9961-129a5041e697}</MetaDataID>
        void IList.RemoveAt(int index)
        {
            RemoveItemAt(index);
        }

        /// <MetaDataID>{50702bc2-fec4-44b1-bf55-79a7ef028fac}</MetaDataID>
        object IList.this[int index]
        {
            get => GetItem(index);
            set => SetItem(index, EnsureValue(value));
        }
        #endregion

        #region ICollection Members
        /// <MetaDataID>{f0371684-6b3a-4053-a57a-030c83f76797}</MetaDataID>
        void ICollection.CopyTo(Array array, int index)
        {
            CopyItemsTo(array, index);
        }

        /// <summary>
        /// Gets the count of child JSON tokens.
        /// </summary>
        /// <value>The count of child JSON tokens.</value>
        /// <MetaDataID>{279a9b99-b350-4513-a2bc-504b18e61789}</MetaDataID>
        public int Count => ChildrenTokens.Count;

        /// <MetaDataID>{b9e465c0-c226-44a4-a6ac-a0f407fe821d}</MetaDataID>
        bool ICollection.IsSynchronized => false;

        /// <MetaDataID>{58bf24bb-9059-4fc3-a5d6-dd5a113dc4cb}</MetaDataID>
        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }

                return _syncRoot;
            }
        }
        #endregion

        #region IBindingList Members
#if HAVE_COMPONENT_MODEL
        /// <MetaDataID>{672d2ea5-5161-40ce-be18-a19b4602fc60}</MetaDataID>
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
        }

        /// <MetaDataID>{a4f6983f-21e8-4b96-8df8-61f2583ac97c}</MetaDataID>
        object IBindingList.AddNew()
        {
            AddingNewEventArgs args = new AddingNewEventArgs();
            OnAddingNew(args);

            if (args.NewObject == null)
            {
                throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, GetType()));
            }

            if (!(args.NewObject is JToken newItem))
            {
                throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JToken)));
            }

            Add(newItem);

            return newItem;
        }

        /// <MetaDataID>{2fde1e43-8f8b-4482-bb83-9ba94a9f65e3}</MetaDataID>
        bool IBindingList.AllowEdit => true;

        /// <MetaDataID>{f1a7875b-c0bb-4562-a62a-b0cd232ad7ee}</MetaDataID>
        bool IBindingList.AllowNew => true;

        /// <MetaDataID>{e4bdda23-d6f1-4d90-844c-0d600abe8b17}</MetaDataID>
        bool IBindingList.AllowRemove => true;

        /// <MetaDataID>{da740f6f-de9e-429d-b2fb-d7ba3afeb593}</MetaDataID>
        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        /// <MetaDataID>{01688589-aaa8-41c0-bb78-250b73923cd6}</MetaDataID>
        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        /// <MetaDataID>{909f5f30-6b2c-4409-8370-ab71b4d3c401}</MetaDataID>
        bool IBindingList.IsSorted => false;

        /// <MetaDataID>{c682d3a3-a780-426e-9ba3-701aa86d62f0}</MetaDataID>
        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
        }

        /// <MetaDataID>{5b97d1c0-97e6-4af5-a3e0-9ac6a85ab424}</MetaDataID>
        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

        /// <MetaDataID>{427d558f-695f-4987-8a67-289f78f443f4}</MetaDataID>
        ListSortDirection IBindingList.SortDirection => ListSortDirection.Ascending;

        /// <MetaDataID>{55f15ab0-946c-4edd-b4d9-3265b80ca6a0}</MetaDataID>
        PropertyDescriptor IBindingList.SortProperty => null;

        /// <MetaDataID>{883c667c-b0d2-43d4-ac22-2fd1e4f16fb1}</MetaDataID>
        bool IBindingList.SupportsChangeNotification => true;

        /// <MetaDataID>{24ffba9e-ed29-4a5c-a05d-dd0d60ac64fd}</MetaDataID>
        bool IBindingList.SupportsSearching => false;

        /// <MetaDataID>{6cbce4c4-dbc2-40da-abd5-d85853594c07}</MetaDataID>
        bool IBindingList.SupportsSorting => false;
#endif
        #endregion

        /// <MetaDataID>{236710c2-4ead-45c3-97eb-c05b57b6a458}</MetaDataID>
        internal static void MergeEnumerableContent(JContainer target, IEnumerable content, JsonMergeSettings settings)
        {
            switch (settings.MergeArrayHandling)
            {
                case MergeArrayHandling.Concat:
                    foreach (JToken item in content)
                    {
                        target.Add(item);
                    }
                    break;
                case MergeArrayHandling.Union:
#if HAVE_HASH_SET
                    HashSet<JToken> items = new HashSet<JToken>(target, EqualityComparer);

                    foreach (JToken item in content)
                    {
                        if (items.Add(item))
                        {
                            target.Add(item);
                        }
                    }
#else
                    Dictionary<JToken, bool> items = new Dictionary<JToken, bool>(EqualityComparer);
                    foreach (JToken t in target)
                    {
                        items[t] = true;
                    }

                    foreach (JToken item in content)
                    {
                        if (!items.ContainsKey(item))
                        {
                            items[item] = true;
                            target.Add(item);
                        }
                    }
#endif
                    break;
                case MergeArrayHandling.Replace:
                    target.ClearItems();
                    foreach (JToken item in content)
                    {
                        target.Add(item);
                    }
                    break;
                case MergeArrayHandling.Merge:
                    int i = 0;
                    foreach (object targetItem in content)
                    {
                        if (i < target.Count)
                        {
                            JToken sourceItem = target[i];

                            if (sourceItem is JContainer existingContainer)
                            {
                                existingContainer.Merge(targetItem, settings);
                            }
                            else
                            {
                                if (targetItem != null)
                                {
                                    JToken contentValue = CreateFromContent(targetItem);
                                    if (contentValue.Type != JTokenType.Null)
                                    {
                                        target[i] = contentValue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            target.Add(targetItem);
                        }

                        i++;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(settings), "Unexpected merge array handling when merging JSON.");
            }
        }
    }
}