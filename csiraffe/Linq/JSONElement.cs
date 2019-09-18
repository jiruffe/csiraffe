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
using Jiruffe.CSiraffe.Utility;

namespace Jiruffe.CSiraffe.Linq {

    /// <summary>
    /// Represents JSON element including JSON map {}, list [], or primitive value such as integer, string...
    /// </summary>
    public abstract class JSONElement : IDictionary<object, JSONElement> {

        #region Fields
        #endregion

        #region Indexers

        #region Implement IDictionary

        /// <summary>
        /// Get/Set sub-element with specified key.
        /// </summary>
        /// <param name="i">The specified key.</param>
        /// <returns>The sub-element.</returns>
        JSONElement IDictionary<object, JSONElement>.this[object i] {
            get {
                if (IsMap) {
                    return AsMap()[i];
                }
                if (IsList) {
                    if (i.GetType().IsValueType || i is ValueType) {
                        return AsList()[(int)i];
                    }
                    return default;
                }
                return this;
            }
            set {
                if (IsMap) {
                    AsMap()[i] = value;
                } else if (IsList) {
                    if (i is int) {
                        AsList()[(int)i] = value;
                    }
                }
            }
        }

        #endregion

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
                return Entries.Count <= 0;
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
        /// Get all the entries (key-value pair) of this element.
        /// </summary>
        public ICollection<KeyValuePair<object, JSONElement>> Entries {
            get {
                if (IsMap) {
                    return AsMap();
                }
                if (IsList) {
                    var lst = AsList();
                    var l_rst = Defaults<KeyValuePair<object, JSONElement>>.Collection;
                    for (var i = 0; i < lst.Count; i++) {
                        l_rst.Add(new KeyValuePair<object, JSONElement>(i, lst[i]));
                    }
                    return l_rst;
                }
                var rst = Defaults<KeyValuePair<object, JSONElement>>.Collection;
                rst.Add(new KeyValuePair<object, JSONElement>(default, this));
                return rst;
            }
        }

        #region Implement IDictionary

        /// <summary>
        /// Get all the keys of this element.
        /// </summary>
        ICollection<object> IDictionary<object, JSONElement>.Keys {
            get {
                if (IsMap) {
                    return AsMap().Keys;
                }
                if (IsList) {
                    var rst = Defaults<object>.Collection;
                    for (var i = 0; i < AsList().Count; i++) {
                        rst.Add(i);
                    }
                    return rst;
                }
                return Defaults<object>.Collection;
            }
        }

        /// <summary>
        /// Get all the sub-elements of this element.
        /// </summary>
        ICollection<JSONElement> IDictionary<object, JSONElement>.Values {
            get {
                if (IsMap) {
                    return AsMap().Values;
                }
                if (IsList) {
                    return AsList();
                }
                var rst = Defaults<JSONElement>.Collection;
                rst.Add(this);
                return rst;
            }
        }

        #endregion

        #region Implement ICollection

        /// <summary>
        /// Returns the number of sub-elements in this element.
        /// </summary>
        int ICollection<KeyValuePair<object, JSONElement>>.Count {
            get {
                return Entries.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="ICollection{T}"/> is read-only; otherwise, <c>false</c>.</returns>
        bool ICollection<KeyValuePair<object, JSONElement>>.IsReadOnly {
            get {
                return Entries.IsReadOnly;
            }
        }

        #endregion

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
        public JSONElement List(in IList<JSONElement> elements) {
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
        /// Get the <see cref="IList{T}"/> that this element represents.
        /// </summary>
        /// <returns>The <see cref="IList{T}"/> that this element represents.</returns>
        public IList<JSONElement> AsList() {
            throw new UnexpectedTypeException("Could not cast List from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="IDictionary{TKey, TValue}"/> that this element represents.
        /// </summary>
        /// <returns>The <see cref="IDictionary{TKey, TValue}"/> that this element represents.</returns>
        public IDictionary<object, JSONElement> AsMap() {
            return this;
        }

        /// <summary>
        /// Get the original value that this element represents.
        /// </summary>
        /// <returns>The original value that this element represents.</returns>
        public object AsPrimitive() {
            return this;
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
        /// Get the <see cref="bool"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="bool"/> value that this element represents.</returns>
        public bool AsBoolean() {
            throw new UnexpectedTypeException("Could not cast Boolean from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="byte"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="byte"/> value that this element represents.</returns>
        public byte AsByte() {
            throw new UnexpectedTypeException("Could not cast Byte from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="sbyte"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="sbyte"/> value that this element represents.</returns>
        public sbyte AsSByte() {
            throw new UnexpectedTypeException("Could not cast SByte from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="char"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="char"/> value that this element represents.</returns>
        public char AsChar() {
            throw new UnexpectedTypeException("Could not cast Char from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="decimal"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="decimal"/> value that this element represents.</returns>
        public decimal AsDecimal() {
            throw new UnexpectedTypeException("Could not cast Decimal from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="double"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="double"/> value that this element represents.</returns>
        public double AsDouble() {
            throw new UnexpectedTypeException("Could not cast Double from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="float"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="float"/> value that this element represents.</returns>
        public float AsFloat() {
            throw new UnexpectedTypeException("Could not cast Float from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="int"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="int"/> value that this element represents.</returns>
        public int AsInt32() {
            throw new UnexpectedTypeException("Could not cast Int32 from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="uint"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="uint"/> value that this element represents.</returns>
        public uint AsUInt32() {
            throw new UnexpectedTypeException("Could not cast UInt32 from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="long"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="long"/> value that this element represents.</returns>
        public long AsInt64() {
            throw new UnexpectedTypeException("Could not cast Int64 from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="ulong"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="ulong"/> value that this element represents.</returns>
        public ulong AsUInt64() {
            throw new UnexpectedTypeException("Could not cast UInt64 from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="short"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="short"/> value that this element represents.</returns>
        public short AsInt16() {
            throw new UnexpectedTypeException("Could not cast Int16 from " + GetType().Name);
        }

        /// <summary>
        /// Get the <see cref="ushort"/> value that this element represents.
        /// </summary>
        /// <returns>The <see cref="ushort"/> value that this element represents.</returns>
        public ushort AsUInt16() {
            throw new UnexpectedTypeException("Could not cast UInt16 from " + GetType().Name);
        }

        /// <summary>
        /// Converts this element to the target <see cref="System.Type"/> <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="System.Type"/>.</typeparam>
        /// <returns>The target <see cref="System.Type"/> <see cref="object"/>.</returns>
        public T As<T>() {
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
            if (IsList) {
                return AsList().GetHashCode();
            }
            if (IsMap) {
                return AsMap().GetHashCode();
            }
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
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
            if (Type != ((JSONElement)obj).Type) {
                return false;
            }
            if (IsVoid) {
                return true;
            }
            if (IsPrimitive) {
                return object.Equals(AsPrimitive(), ((JSONElement)obj).AsPrimitive());
            }
            if (IsList) {
                return object.Equals(AsList(), ((JSONElement)obj).AsList());
            }
            if (IsMap) {
                return object.Equals(AsMap(), ((JSONElement)obj).AsMap());
            }
            return object.Equals(this.ToString(), New(obj).ToString());
        }

        #endregion

        #region Implement IEnumerable

        IEnumerator<KeyValuePair<object, JSONElement>> IEnumerable<KeyValuePair<object, JSONElement>>.GetEnumerator() {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Entries.GetEnumerator();
        }

        #endregion

        #region Implement IDictionary

        void IDictionary<object, JSONElement>.Add(object key, JSONElement value) {
            throw new UnsupportedOperationException("Could not Add to " + GetType().Name);
        }

        bool IDictionary<object, JSONElement>.ContainsKey(object key) {
            return false;
        }

        bool IDictionary<object, JSONElement>.Remove(object key) {
            throw new UnsupportedOperationException("Could not Remove from " + GetType().Name);
        }

        bool IDictionary<object, JSONElement>.TryGetValue(object key, out JSONElement value) {
            value = default;
            return false;
        }

        #endregion

        #region Implement ICollection

        void ICollection<KeyValuePair<object, JSONElement>>.Add(KeyValuePair<object, JSONElement> item) {
            Entries.Add(item);
        }

        void ICollection<KeyValuePair<object, JSONElement>>.Clear() {
            Entries.Clear();
        }

        bool ICollection<KeyValuePair<object, JSONElement>>.Contains(KeyValuePair<object, JSONElement> item) {
            return Entries.Contains(item);
        }

        void ICollection<KeyValuePair<object, JSONElement>>.CopyTo(KeyValuePair<object, JSONElement>[] array, int arrayIndex) {
            Entries.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<object, JSONElement>>.Remove(KeyValuePair<object, JSONElement> item) {
            return Entries.Remove(item);
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

}
