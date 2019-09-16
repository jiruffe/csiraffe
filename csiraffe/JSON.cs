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

namespace Jiruffe.CSiraffe {

    /// <summary>
    /// CSiraffe
    /// <para>
    /// A .NET Core library for JSON conversion.
    /// </para>
    /// </summary>
    public static class JSON {

        /// <summary>
        /// Serializes <see cref="object"/> to <see cref="JSONElement"/>.
        /// </summary>
        /// <param name="o">The <see cref="object"/> to be serialized.</param>
        /// <returns>The <see cref="JSONElement"/> serialized.</returns>
        public static JSONElement Serialize(object o) {
            return default;
        }

        /// <summary>
        /// Deserializes JSON <see cref="string"/> to <see cref="JSONElement"/>.
        /// </summary>
        /// <param name="json">The JSON <see cref="string"/> to be deserialized.</param>
        /// <returns>The <see cref="JSONElement"/> deserialized.</returns>
        public static JSONElement Deserialize(string json) {
            return default;
        }

        /// <summary>
        /// Directly serializes <see cref="object"/> to JSON <see cref="string"/>.
        /// </summary>
        /// <param name="o">The <see cref="object"/> to be serialized.</param>
        /// <returns>The JSON <see cref="string"/> serialized.</returns>
        public static string Stringify(object o) {
            return default;
        }

        /// <summary>
        /// Directly deserializes JSON <see cref="string"/> to <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="System.Type"/>.</typeparam>
        /// <param name="json">The JSON <see cref="string"/> to be deserialized.</param>
        /// <returns>The <see cref="object"/> deserialized.</returns>
        public static T Parse<T>(string json) {
            return default;
        }

    }

}
