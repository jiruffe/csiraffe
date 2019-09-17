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

namespace Jiruffe.CSiraffe.Linq {

    /// <summary>
    /// Represents JSON element including JSON map {}, list [], or primitive value such as integer, string...
    /// </summary>
    public abstract class JSONElement : IEnumerable<JSONEntry> {

        /// <summary>
        /// Converts this element to JSON <see cref="string"/>.
        /// </summary>
        /// <returns>JSON <see cref="string"/>.</returns>
        public override string ToString() {
            return base.ToString();
        }

        /// <summary>
        /// Converts this element to <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <returns>The target <see cref="object"/>.</returns>
        public T ToObject<T>() {
            return default;
        }

        public abstract IEnumerator<JSONEntry> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

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

    public class JSONEntry {
    }

}
