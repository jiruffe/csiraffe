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

namespace Jiruffe.CSiraffe.Linq {

    /// <summary>
    /// Represents JSON element including JSON map {}, list [], or primitive value such as integer, string...
    /// </summary>
    public abstract class JSONElement : IEnumerable<JSONEntry> {

        #region Fields
        #endregion

        #region Indexers

        /// <summary>
        /// Get/Set sub-element with specified key.
        /// </summary>
        /// <param name="i">The specified key.</param>
        /// <returns>The sub-element.</returns>
        public JSONElement this[object i] {
            get {
                throw new UnsupportedOperationException("Could not get from " + GetType().Name);
            }
            set {
                throw new UnsupportedOperationException("Could not set to " + GetType().Name);
            }
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Returns <see cref="JSONVoid.Instance"/> which represents a <c>void</c> element,
        /// also known as <c>null</c>, <c>undefined</c> or <c>NaN</c> in JSON.
        /// </summary>
        public static JSONElement Void {
            get {
                return JSONVoid.Instance;
            }
        }

        /// <summary>
        /// Indicate whether this element is empty.
        /// </summary>
        public bool IsEmpty {
            get {
                throw new UnsupportedOperationException("Could not indicate whether is empty or not from " + GetType().Name);
            }
        }

        /// <summary>
        /// Indicate whether this element is <c>void</c>.
        /// </summary>
        public bool IsVoid {
            get {
                return this is JSONVoid;
            }
        }

        /// <summary>
        /// Indicate whether this element is an instance of <see cref="JSONList"/>.
        /// </summary>
        public bool IsList {
            get {
                return this is JSONList;
            }
        }

        /// <summary>
        /// Indicate whether this element is an instance of <see cref="JSONMap"/>.
        /// </summary>
        public bool IsMap {
            get {
                return this is JSONMap;
            }
        }

        /// <summary>
        /// Indicate whether this element is an instance of <see cref="JSONPrimitive"/>.
        /// </summary>
        public bool IsPrimitive {
            get {
                return this is JSONPrimitive;
            }
        }

        /// <summary>
        /// Get the <see cref="JSONElementType"/> of this element.
        /// </summary>
        public JSONElementType Type {
            get {
                if (IsVoid) {
                    return JSONElementType.Void;
                }
                if (IsList) {
                    return JSONElementType.List;
                }
                if (IsMap) {
                    return JSONElementType.Map;
                }
                if (IsPrimitive) {
                    return JSONElementType.Primitive;
                }
                return JSONElementType.Unknown;
            }
        }

        /// <summary>
        /// Returns the number of sub-elements in this element.
        /// </summary>
        public int Size {
            get {
                throw new UnsupportedOperationException("Could not get size from " + GetType().Name);
            }
        }

        /// <summary>
        /// Get all the entries (key-value pair) of this element.
        /// <seealso cref="JSONEntry"/>
        /// </summary>
        public IEnumerable<JSONEntry> Entries {
            get {
                throw new UnsupportedOperationException("Could not get entries from " + GetType().Name);
            }
        }

        /// <summary>
        /// Get all the keys of this element.
        /// </summary>
        public IEnumerable<object> Keys {
            get {
                throw new UnsupportedOperationException("Could not get keys from " + GetType().Name);
            }
        }

        /// <summary>
        /// Get all the sub-elements of this element.
        /// </summary>
        public IEnumerable<object> Elements {
            get {
                throw new UnsupportedOperationException("Could not get elements from " + GetType().Name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Alias of <see cref="Void"/>.
        /// </summary>
        /// <returns>What <see cref="Void"/> returns.</returns>
        [Obsolete("Deprecated. Use JSONElement.New(object) Instead.")]
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
        public JSONElement List(in ICollection<JSONElement> elements) {
            if (elements is null) {
                return List();
            }
            return new JSONList(elements);
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONMap"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="JSONMap"/>.</returns>
        public JSONElement Map() {
            return new JSONMap();
        }

        /// <summary>
        /// Get a new instance of <see cref="JSONMap"/> with specified sub-elements.
        /// </summary>
        /// <param name="elements">The sub-elements.</param>
        /// <returns>A new instance of <see cref="JSONMap"/> with specified sub-elements.</returns>
        public JSONElement Map(in IDictionary<object, JSONElement> elements) {
            if (elements is null) {
                return Map();
            }
            return new JSONMap(elements);
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
                goto err_type;
            }
            if (obj.GetType().IsValueType || obj is ValueType || obj is string) {
                return new JSONPrimitive(obj);
            }
        err_type:
            throw new UnexpectedTypeException("Use JSONElement.New(object) instead.");
        }

        /// <summary>
        /// Merge another element to this element.
        /// </summary>
        /// <param name="element">The other element.</param>
        /// <returns>This element itself.</returns>
        public JSONElement Merge(JSONElement element) {
            throw new UnsupportedOperationException("Only merging JSONList into JSONList or merging JSONMap into JSONMap are supported");
        }

        /// <summary>
        /// Returns the <see cref="string"/> that this element represents if instance of <see cref="JSONPrimitive"/>,
        /// or the JSON expression of this element otherwise.
        /// </summary>
        /// <returns>The <see cref="string"/> that this element represents or the JSON expression of this element.</returns>
        public string AsString() {
            return StringAnalyzer.Analyze(this);
        }

        /// <summary>
        /// Alias of <see cref="ToObject{T}"/>
        /// </summary>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <returns>The target <see cref="object"/>.</returns>
        public T As<T>() {
            return ToObject<T>();
        }

        /// <summary>
        /// Converts this element to <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <returns>The target <see cref="object"/>.</returns>
        public T ToObject<T>() {
            return ObjectAnalyzer.Analyze<T>(this);
        }

        #region Override Object

        /// <summary>
        /// Converts this element to JSON <see cref="string"/>.
        /// </summary>
        /// <returns>JSON <see cref="string"/>.</returns>
        public override string ToString() {
            return AsString();
        }

        #endregion

        #region Implement IEnumerable

        public IEnumerator<JSONEntry> GetEnumerator() {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
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
        /// JSON list [].
        /// <seealso cref="JSONList"/>
        /// </summary>
        List,
        /// <summary>
        /// JSON map {}.
        /// <seealso cref="JSONMap"/>
        /// </summary>
        Map,
        /// <summary>
        /// JSON primitive value such as integer, string...
        /// <seealso cref="JSONPrimitive"/>
        /// </summary>
        Primitive,
        /// <summary>
        /// JSON <c>null</c>, <c>undefined</c> or <c>NaN</c>.
        /// <seealso cref="JSONVoid"/>
        /// </summary>
        Void
    }

    /// <summary>
    /// An element entry (key-value pair).
    /// <seealso cref="JSONElement.Entries"/>
    /// </summary>
    public sealed class JSONEntry {

        #region Fields

        private readonly object _Key;
        private readonly JSONElement _Element;

        #endregion

        #region Accessors

        public object Key {
            get {
                return _Key;
            }
        }

        public JSONElement Element {
            get {
                return _Element;
            }
        }

        #endregion

        #region Constructors

        internal JSONEntry(object key, JSONElement element) {
            _Key = key;
            _Element = element;
        }

        #endregion

    }

}
