#region License
/*
 *    Copyright 2019 Jiruffe
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

using Jiruffe.CSiraffe.Analyzer;
using Jiruffe.CSiraffe.Exception;
using Jiruffe.CSiraffe.Linq.Internal;
using Jiruffe.CSiraffe.Utility;

namespace Jiruffe.CSiraffe.Linq {

    /// <summary>
    /// Represents JSON element including JSON dictionary {}, list [], or primitive value such as integer, string...
    /// </summary>
    public abstract class JSONElement : IConvertible, IDictionary<string, JSONElement>, IList<JSONElement> {

        #region Fields
        #endregion

        #region Indexers

        #region Implement IDictionary

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="key">key</paramref> is not found.</exception>
        /// <exception cref="NotSupportedException">The property is set and the <see cref="IDictionary{TKey, TValue}"/> is read-only.</exception>
        JSONElement IDictionary<string, JSONElement>.this[string key] {
            get {
                if (IsDictionary) {
                    return AsDictionary()[key];
                }
                return default;
            }
            set {
                if (IsDictionary) {
                    AsDictionary()[key] = value;
                }
            }
        }

        #endregion

        #region Implement IList

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index">index</paramref> is not a valid index in the <see cref="IList{T}"/>.</exception>
        /// <exception cref="NotSupportedException">The property is set and the <see cref="IList{T}"/> is read-only.</exception>
        JSONElement IList<JSONElement>.this[int index] {
            get {
                if (IsList) {
                    return AsList()[index];
                }
                return default;
            }
            set {
                if (IsList) {
                    AsList()[index] = value;
                }
            }
        }

        #endregion

        #endregion

        #region Accessors

        /// <summary>
        /// Returns <see cref="JSONVoid.Instance"/> which represents a void element,
        /// also known as null, undefined or NaN in JSON.
        /// </summary>
        /// <returns><see cref="JSONVoid.Instance"/></returns>
        public static JSONElement Void {
            get {
                return JSONVoid.Instance;
            }
        }

        /// <summary>
        /// Indicate whether this element is empty.
        /// </summary>
        /// <returns>true if this element is empty; otherwise, false.</returns>
        public bool IsEmpty {
            get {
                if (IsDictionary) {
                    return AsDictionary().Count <= 0;
                }
                if (IsList) {
                    return AsList().Count <= 0;
                }
                if (IsPrimitive) {
                    return false;
                }
                if (IsVoid) {
                    return true;
                }
                return default;
            }
        }

        /// <summary>
        /// Indicate whether this element is void.
        /// </summary>
        /// <returns>true if this element is void; otherwise, false.</returns>
        public bool IsVoid {
            get {
                return this is JSONVoid;
            }
        }

        /// <summary>
        /// Indicate whether this element is an instance of <see cref="JSONDictionary"/>.
        /// </summary>
        /// <returns>true if this element is an instance of <see cref="JSONDictionary"/>; otherwise, false.</returns>
        public bool IsDictionary {
            get {
                return this is JSONDictionary;
            }
        }

        /// <summary>
        /// Indicate whether this element is an instance of <see cref="JSONList"/>.
        /// </summary>
        /// <returns>true if this element is an instance of <see cref="JSONList"/>; otherwise, false.</returns>
        public bool IsList {
            get {
                return this is JSONList;
            }
        }

        /// <summary>
        /// Indicate whether this element is an instance of <see cref="JSONPrimitive"/>.
        /// </summary>
        /// <returns>true if this element is an instance of <see cref="JSONPrimitive"/>; otherwise, false.</returns>
        public bool IsPrimitive {
            get {
                return this is JSONPrimitive;
            }
        }

        /// <summary>
        /// Get the <see cref="JSONElementType"/> of this element.
        /// </summary>
        /// <returns>The <see cref="JSONElementType"/> of this element</returns>
        public JSONElementType ElementType {
            get {
                if (IsVoid) {
                    return JSONElementType.Void;
                }
                if (IsDictionary) {
                    return JSONElementType.Dictionary;
                }
                if (IsList) {
                    return JSONElementType.List;
                }
                if (IsPrimitive) {
                    return JSONElementType.Primitive;
                }
                return JSONElementType.Unknown;
            }
        }

        #region Implement IDictionary

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the keys of the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <returns>An <see cref="ICollection{T}"/> containing the keys of the object that implements <see cref="IDictionary{TKey, TValue}"/>.</returns>
        ICollection<string> IDictionary<string, JSONElement>.Keys {
            get {
                if (IsDictionary) {
                    return AsDictionary().Keys;
                }

                return default;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the values in the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <returns>An <see cref="ICollection{T}"/> containing the values in the object that implements <see cref="IDictionary{TKey, TValue}"/>.</returns>
        ICollection<JSONElement> IDictionary<string, JSONElement>.Values {
            get {
                if (IsDictionary) {
                    return AsDictionary().Values;
                }
                return default;
            }
        }

        #region Implement IDictionary : ICollection

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="ICollection{T}"/>.</returns>
        int ICollection<KeyValuePair<string, JSONElement>>.Count {
            get {
                if (IsDictionary) {
                    return AsDictionary().Count;
                }
                return default;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="ICollection{T}"/> is read-only; otherwise, false.</returns>
        bool ICollection<KeyValuePair<string, JSONElement>>.IsReadOnly {
            get {
                if (IsDictionary) {
                    return AsDictionary().IsReadOnly;
                }
                return default;
            }
        }

        #endregion

        #endregion

        #region Implement IList

        #region Implement IList : ICollection

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="ICollection{T}"/>.</returns>
        int ICollection<JSONElement>.Count {
            get {
                if (IsList) {
                    return AsList().Count;
                }
                return default;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="ICollection{T}"/> is read-only; otherwise, false.</returns>
        bool ICollection<JSONElement>.IsReadOnly {
            get {
                if (IsList) {
                    return AsList().IsReadOnly;
                }
                return default;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Alias of <see cref="Void"/>.
        /// </summary>
        /// <returns>What <see cref="Void"/> returns.</returns>
        [Obsolete("Deprecated. Use JSONElement.New(object) instead.")]
        public JSONElement New() {
            return Void;
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONElement"/> with specified original <see cref="object"/>.
        /// </summary>
        /// <param name="obj">The original <see cref="object"/>.</param>
        /// <returns>A new instance of <see cref="JSONElement"/> with specified original <see cref="object"/>.</returns>
        public JSONElement New(in object obj) {
            if (obj is null) {
                return New();
            }
            if (obj is JSONElement) {
                return (JSONElement)obj;
            }
            return ObjectAnalyzer.Analyze(obj);
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONList"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="JSONList"/>.</returns>
        public JSONElement List() {
            return new JSONList();
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONList"/> with specified sub-elements.
        /// </summary>
        /// <param name="elements">The sub-elements.</param>
        /// <returns>A new instance of <see cref="JSONList"/> with specified sub-elements.</returns>
        public JSONElement List(in IList<JSONElement> elements) {
            if (elements is null) {
                return List();
            }
            return new JSONList(elements);
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONDictionary"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="JSONDictionary"/>.</returns>
        public JSONElement Dictionary() {
            return new JSONDictionary();
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONDictionary"/> with specified sub-elements.
        /// </summary>
        /// <param name="elements">The sub-elements.</param>
        /// <returns>A new instance of <see cref="JSONDictionary"/> with specified sub-elements.</returns>
        public JSONElement Dictionary(in IDictionary<string, JSONElement> elements) {
            if (elements is null) {
                return Dictionary();
            }
            return new JSONDictionary(elements);
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONPrimitive"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="JSONPrimitive"/>.</returns>
        public JSONElement Primitive() {
            return new JSONPrimitive();
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONPrimitive"/> with specified original value.
        /// </summary>
        /// <param name="obj">The original value.</param>
        /// <returns>A new instance of <see cref="JSONPrimitive"/> with specified original value.</returns>
        public JSONElement Primitive(in object obj) {
            if (obj is null) {
                return Primitive();
            }
            if (obj is JSONPrimitive) {
                return (JSONElement)obj;
            }
            if (obj is JSONElement) {
                return Primitive();
            }
            if (obj.GetType().IsValueType || obj is ValueType || obj is string) {
                return new JSONPrimitive(obj);
            }
            return Primitive();
        }

        /// <summary>
        /// Merge another element to this element.
        /// </summary>
        /// <param name="element">The other element.</param>
        /// <returns>This element itself.</returns>
        public JSONElement Merge(JSONElement element) {
            if (element is null) {
                return this;
            }
            if (IsDictionary && element.IsDictionary) {
                var thisDictionary = AsDictionary();
                foreach (var e in element.AsDictionary()) {
                    thisDictionary.Add(e);
                }
            }
            if (IsList && element.IsList) {
                var thisList = AsList();
                foreach (var e in element.AsList()) {
                    thisList.Add(e);
                }
            }
            throw new UnsupportedOperationException("Only merging JSONList into JSONList or JSONDictionary into JSONDictionary are supported.");
        }

        /// <summary>
        /// Get the <see cref="IList{T}"/> that this element represents.
        /// </summary>
        /// <returns>The <see cref="IList{T}"/> that this element represents.</returns>
        public virtual IList<JSONElement> AsList() {
            return Defaults<JSONElement>.List;
        }

        /// <summary>
        /// Get the <see cref="IDictionary{TKey, TValue}"/> that this element represents.
        /// </summary>
        /// <returns>The <see cref="IDictionary{TKey, TValue}"/> that this element represents.</returns>
        public virtual IDictionary<string, JSONElement> AsDictionary() {
            return Defaults<string, JSONElement>.Dictionary;
        }

        /// <summary>
        /// Get the original value that this element represents.
        /// </summary>
        /// <returns>The original value that this element represents.</returns>
        public virtual object AsPrimitive() {
            return Defaults.Primitive;
        }

        /// <summary>
        /// Converts this element to the target <see cref="Type"/> <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <returns>The target <see cref="Type"/> <see cref="object"/>.</returns>
        public T As<T>() {
            return ObjectAnalyzer.Analyze<T>(this);
        }

        #region Override Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return As<string>();
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            if (IsVoid || IsEmpty) {
                return 0;
            }
            if (IsPrimitive) {
                return AsPrimitive().GetHashCode();
            }
            if (IsDictionary) {
                return AsDictionary().GetHashCode();
            }
            if (IsList) {
                return AsList().GetHashCode();
            }
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) {
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj is null) {
                return false;
            }
            if (!(obj is JSONElement)) {
                return false;
            }
            if (ElementType != ((JSONElement)obj).ElementType) {
                return false;
            }
            if (IsVoid) {
                return true;
            }
            if (IsPrimitive) {
                return object.Equals(AsPrimitive(), ((JSONElement)obj).AsPrimitive());
            }
            if (IsDictionary) {
                return object.Equals(AsDictionary(), ((JSONElement)obj).AsDictionary());
            }
            if (IsList) {
                return object.Equals(AsList(), ((JSONElement)obj).AsList());
            }
            return object.Equals(this.ToString(), New(obj).ToString());
        }
        #endregion

        #region Implement IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            if (IsDictionary) {
                return AsDictionary().GetEnumerator();
            }
            if (IsList) {
                return AsList().GetEnumerator();
            }
            return default;
        }

        #endregion

        #region Implement IDictionary

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="ArgumentException">An element with the same key already exists in the <see cref="IDictionary{TKey, TValue}"/>.</exception>
        /// <exception cref="NotSupportedException">The <see cref="IDictionary{TKey, TValue}"/> is read-only.</exception>
        void IDictionary<string, JSONElement>.Add(string key, JSONElement value) {
            if (IsDictionary) {
                AsDictionary().Add(key, value);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="IDictionary{TKey, TValue}"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <returns>true if the <see cref="IDictionary{TKey, TValue}"/> contains an element with the key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        bool IDictionary<string, JSONElement>.ContainsKey(string key) {
            if (IsDictionary) {
                return AsDictionary().ContainsKey(key);
            }
            return default;
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key">key</paramref> was not found in the original <see cref="IDictionary{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="IDictionary{TKey, TValue}"/> is read-only.</exception>
        bool IDictionary<string, JSONElement>.Remove(string key) {
            if (IsDictionary) {
                return AsDictionary().Remove(key);
            }
            return default;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements <see cref="IDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        bool IDictionary<string, JSONElement>.TryGetValue(string key, out JSONElement value) {
            if (IsDictionary) {
                return AsDictionary().TryGetValue(key, out value);
            }
            value = default;
            return default;
        }

        #region Implement IDictionary : ICollection

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        void ICollection<KeyValuePair<string, JSONElement>>.Add(KeyValuePair<string, JSONElement> item) {
            if (IsDictionary) {
                AsDictionary().Add(item);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        void ICollection<KeyValuePair<string, JSONElement>>.Clear() {
            if (IsDictionary) {
                AsDictionary().Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}"/>.</param>
        /// <returns>true if <paramref name="item">item</paramref> is found in the <see cref="ICollection{T}"/>; otherwise, false.</returns>
        bool ICollection<KeyValuePair<string, JSONElement>>.Contains(KeyValuePair<string, JSONElement> item) {
            if (IsDictionary) {
                return AsDictionary().Contains(item);
            }
            return default;
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ICollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array">array</paramref> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex">arrayIndex</paramref> is less than 0.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="ICollection{T}"/> is greater than the available space from <paramref name="arrayIndex">arrayIndex</paramref> to the end of the destination <paramref name="array">array</paramref>.</exception>
        void ICollection<KeyValuePair<string, JSONElement>>.CopyTo(KeyValuePair<string, JSONElement>[] array, int arrayIndex) {
            if (IsDictionary) {
                AsDictionary().CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <returns>true if <paramref name="item">item</paramref> was successfully removed from the <see cref="ICollection{T}"/>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="ICollection{T}"/>.</returns>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        bool ICollection<KeyValuePair<string, JSONElement>>.Remove(KeyValuePair<string, JSONElement> item) {
            if (IsDictionary) {
                return AsDictionary().Remove(item);
            }
            return default;
        }
        
        #region Implement IDictionary : ICollection : IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<KeyValuePair<string, JSONElement>> IEnumerable<KeyValuePair<string, JSONElement>>.GetEnumerator() {
            if (IsDictionary) {
                return AsDictionary().GetEnumerator();
            }
            return default;
        }

        #endregion

        #endregion

        #endregion

        #region Implement IList

        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="IList{T}"/>.</param>
        /// <returns>The index of <paramref name="item">item</paramref> if found in the list; otherwise, -1.</returns>
        int IList<JSONElement>.IndexOf(JSONElement item) {
            if (IsList) {
                return AsList().IndexOf(item);
            }
            return default;
        }

        /// <summary>
        /// Inserts an item to the <see cref="IList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="IList{T}"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index">index</paramref> is not a valid index in the <see cref="IList{T}"/>.</exception>
        /// <exception cref="NotSupportedException">The <see cref="IList{T}"/> is read-only.</exception>
        void IList<JSONElement>.Insert(int index, JSONElement item) {
            if (IsList) {
                AsList().Insert(index, item);
            }
        }

        /// <summary>
        /// Removes the <see cref="IList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index">index</paramref> is not a valid index in the <see cref="IList{T}"/>.</exception>
        /// <exception cref="NotSupportedException">The <see cref="IList{T}"/> is read-only.</exception>
        void IList<JSONElement>.RemoveAt(int index) {
            if (IsList) {
                AsList().RemoveAt(index);
            }
        }

        #region Implement IList : ICollection

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        void ICollection<JSONElement>.Add(JSONElement item) {
            if (IsList) {
                AsList().Add(item);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        void ICollection<JSONElement>.Clear() {
            if (IsList) {
                AsList().Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}"/>.</param>
        /// <returns>true if <paramref name="item">item</paramref> is found in the <see cref="ICollection{T}"/>; otherwise, false.</returns>
        bool ICollection<JSONElement>.Contains(JSONElement item) {
            if (IsList) {
                return AsList().Contains(item);
            }
            return default;
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ICollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array">array</paramref> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex">arrayIndex</paramref> is less than 0.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="ICollection{T}"/> is greater than the available space from <paramref name="arrayIndex">arrayIndex</paramref> to the end of the destination <paramref name="array">array</paramref>.</exception>
        void ICollection<JSONElement>.CopyTo(JSONElement[] array, int arrayIndex) {
            if (IsList) {
                AsList().CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <returns>true if <paramref name="item">item</paramref> was successfully removed from the <see cref="ICollection{T}"/>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="ICollection{T}"/>.</returns>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        bool ICollection<JSONElement>.Remove(JSONElement item) {
            if (IsList) {
                return AsList().Remove(item);
            }
            return default;
        }

        #region Implement IList : ICollection : IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<JSONElement> IEnumerable<JSONElement>.GetEnumerator() {
            if (IsList) {
                return AsList().GetEnumerator();
            }
            return default;
        }

        #endregion

        #endregion

        #endregion

        #region Implement IConvertible

        /// <summary>
        /// Returns the <see cref="TypeCode"/> for this instance.
        /// </summary>
        /// <returns>The enumerated constant that is the <see cref="TypeCode"/> of the class or value type that implements this interface.</returns>
        TypeCode IConvertible.GetTypeCode() {
            if (IsPrimitive) {
                return Type.GetTypeCode(AsPrimitive().GetType());
            }
            return TypeCode.Object;
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="object"/> of the specified <see cref="Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An <see cref="object"/> instance of type <paramref name="conversionType">conversionType</paramref> whose value is equivalent to the value of this instance.</returns>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
            return Convert.ChangeType(GetType().GetMethod("As").MakeGenericMethod(new Type[] { conversionType }).Invoke(this, null), conversionType, provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="string"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="string"/> instance equivalent to the value of this instance.</returns>
        string IConvertible.ToString(IFormatProvider provider) {
            return Convert.ToString(As<string>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A Boolean value equivalent to the value of this instance.</returns>
        bool IConvertible.ToBoolean(IFormatProvider provider) {
            return Convert.ToBoolean(As<bool>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
        byte IConvertible.ToByte(IFormatProvider provider) {
            return Convert.ToByte(As<byte>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A Unicode character equivalent to the value of this instance.</returns>
        char IConvertible.ToChar(IFormatProvider provider) {
            return Convert.ToChar(As<char>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="decimal"/> number equivalent to the value of this instance.</returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider) {
            return Convert.ToDecimal(As<decimal>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A double-precision floating-point number equivalent to the value of this instance.</returns>
        double IConvertible.ToDouble(IFormatProvider provider) {
            return Convert.ToDouble(As<double>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
        short IConvertible.ToInt16(IFormatProvider provider) {
            return Convert.ToInt16(As<short>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
        int IConvertible.ToInt32(IFormatProvider provider) {
            return Convert.ToInt32(As<int>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
        long IConvertible.ToInt64(IFormatProvider provider) {
            return Convert.ToInt64(As<long>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider) {
            return Convert.ToSByte(As<sbyte>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A single-precision floating-point number equivalent to the value of this instance.</returns>
        float IConvertible.ToSingle(IFormatProvider provider) {
            return Convert.ToSingle(As<float>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider) {
            return Convert.ToUInt16(As<ushort>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
        uint IConvertible.ToUInt32(IFormatProvider provider) {
            return Convert.ToUInt32(As<uint>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider) {
            return Convert.ToUInt64(As<ulong>(), provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="DateTime"/> instance equivalent to the value of this instance.</returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider) {
            return Convert.ToDateTime(As<DateTime>(), provider);
        }

        #endregion

        #endregion

    }

    /// <summary>
    /// Represents JSON elements types.
    /// </summary>
    public enum JSONElementType {

        /// <summary>
        /// Unknown element type.
        /// </summary>
        Unknown,

        /// <summary>
        /// JSON dictionary {}.
        /// <seealso cref="JSONDictionary"/>
        /// </summary>
        Dictionary,

        /// <summary>
        /// JSON list [].
        /// <seealso cref="JSONList"/>
        /// </summary>
        List,

        /// <summary>
        /// JSON primitive value such as integer, string...
        /// <seealso cref="JSONPrimitive"/>
        /// </summary>
        Primitive,

        /// <summary>
        /// JSON null, undefined or NaN.
        /// <seealso cref="JSONVoid"/>
        /// </summary>
        Void

    }

}
