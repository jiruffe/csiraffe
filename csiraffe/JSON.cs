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

using Jiruffe.CSiraffe.Analyzer;
using Jiruffe.CSiraffe.Exception;
using Jiruffe.CSiraffe.Linq;

namespace Jiruffe.CSiraffe {

    /// <summary>
    /// CSiraffe
    /// <para>
    /// A .NET Core library for JSON conversion.
    /// </para>
    /// </summary>
    public static class JSON {

        /// <summary>
        /// Serializes <see cref="object"/> to <see cref="JSONEntity"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be serialized.</param>
        /// <returns>The <see cref="JSONEntity"/> serialized.</returns>
        public static JSONEntity Serialize(object obj) {
            if (obj is JSONEntity) {
                return obj as JSONEntity;
            }
            return ObjectAnalyzer.Analyze(obj);
        }

        /// <summary>
        /// Deserializes JSON <see cref="string"/> to <see cref="JSONEntity"/>.
        /// </summary>
        /// <param name="str">The JSON <see cref="string"/> to be deserialized.</param>
        /// <returns>The <see cref="JSONEntity"/> deserialized.</returns>
        public static JSONEntity Deserialize(string str) {
            return StringAnalyzer.Analyze(str);
        }

        /// <summary>
        /// Directly serializes <see cref="object"/> to JSON <see cref="string"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be serialized.</param>
        /// <returns>The JSON <see cref="string"/> serialized.</returns>
        public static string Stringify(object obj) {
            if (obj is string) {
                return obj as string;
            }
            if (obj is JSONEntity) {
                return StringAnalyzer.Analyze(obj as JSONEntity);
            }
            return DirectAnalyzer.Analyze(obj);
        }

        /// <summary>
        /// Directly deserializes JSON <see cref="string"/> to <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <param name="str">The JSON <see cref="string"/> to be deserialized.</param>
        /// <returns>The <see cref="object"/> deserialized.</returns>
        public static T Parse<T>(string str) {
            if (typeof(T).IsAssignableFrom(typeof(string))) {
                return (T)(object)str;
            }
            if (typeof(T) == typeof(JSONEntity)) {
                return (T)(object)StringAnalyzer.Analyze(str);
            }
            if (typeof(T).IsSubclassOf(typeof(JSONEntity))) {
                throw new UnexpectedTypeException("Use JSON.Deserialize(string) instead.");
            }
            return DirectAnalyzer.Analyze<T>(str);
        }

    }

    /// <summary>
    /// Extension Methods for JSON conversion to <see cref="object"/> and <see cref="string"/>.
    /// </summary>
    public static class JSONExtensionMethods {

        /// <summary>
        /// Serializes <see cref="object"/> to <see cref="JSONEntity"/>.
        /// </summary>
        /// <seealso cref="JSON.Serialize(object)"/>
        /// <param name="obj">The <see cref="object"/> to be serialized.</param>
        /// <returns>The <see cref="JSONEntity"/> serialized.</returns>
        public static JSONEntity Serialize(this object obj) {
            return JSON.Serialize(obj);
        }

        /// <summary>
        /// Deserializes JSON <see cref="string"/> to <see cref="JSONEntity"/>.
        /// </summary>
        /// <seealso cref="JSON.Deserialize(string)"/>
        /// <param name="str">The JSON <see cref="string"/> to be deserialized.</param>
        /// <returns>The <see cref="JSONEntity"/> deserialized.</returns>
        public static JSONEntity Deserialize(this string str) {
            return JSON.Deserialize(str);
        }

        /// <summary>
        /// Directly serializes <see cref="object"/> to JSON <see cref="string"/>.
        /// </summary>
        /// <seealso cref="JSON.Stringify(object)"/>
        /// <param name="obj">The <see cref="object"/> to be serialized.</param>
        /// <returns>The JSON <see cref="string"/> serialized.</returns>
        public static string Stringify(this object obj) {
            return JSON.Stringify(obj);
        }

        /// <summary>
        /// Directly deserializes JSON <see cref="string"/> to <see cref="object"/>.
        /// </summary>
        /// <seealso cref="JSON.Parse{T}(string)"/>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <param name="str">The JSON <see cref="string"/> to be deserialized.</param>
        /// <returns>The <see cref="object"/> deserialized.</returns>
        public static T Parse<T>(this string str) {
            return JSON.Parse<T>(str);
        }

    }

}
