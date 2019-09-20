﻿#region License
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

using Jiruffe.CSiraffe.Linq;
using Jiruffe.CSiraffe.Utility;

namespace Jiruffe.CSiraffe.Analyzer {

    /// <summary>
    /// JSON <see cref="string"/> &lt;=&gt; <see cref="JSONEntity"/> conversion.
    /// </summary>
    internal static class StringAnalyzer {

        /// <summary>
        /// JSON <see cref="string"/> =&gt; <see cref="JSONEntity"/> conversion.
        /// </summary>
        /// <param name="str">The JSON <see cref="string"/> to be converted.</param>
        /// <returns>The <see cref="JSONEntity"/> converted.</returns>
        internal static JSONEntity Analyze(string str) {

            if (str is null) {
                goto exit_with_void;
            }

            str = str.Trim();

            if (!str.StartsWith(Constants.Tokens.JSONDictionaryStart) && !str.StartsWith(Constants.Tokens.JSONListStart)) {
                return Convert.FromString(str);
            }

            exit_with_void:
            return JSONEntity.Void;

        }

        /// <summary>
        /// <see cref="JSONEntity"/> =&gt; JSON <see cref="string"/> conversion.
        /// </summary>
        /// <param name="entity">The <see cref="JSONEntity"/> to be converted.</param>
        /// <returns>The JSON <see cref="string"/> converted.</returns>
        internal static string Analyze(JSONEntity entity) {
            return default;
        }

        private static class Convert {

            internal static JSONEntity FromString(string str) {

                if (str is null) {
                    return JSONEntity.Void;
                }
                if (str.SurroundedWith(Constants.Characters.APOSTROPHE) || str.SurroundedWith(Constants.Characters.QUOTE)) {
                    return JSONEntity.Primitive(str.Substring(1, str.Length - 1).Unescape());
                }
                if (string.Equals(str, Constants.Tokens.JSONPrimitiveTrue, StringComparison.OrdinalIgnoreCase)) {
                    return JSONEntity.Primitive(true);
                }
                if (string.Equals(str, Constants.Tokens.JSONPrimitiveFalse, StringComparison.OrdinalIgnoreCase)) {
                    return JSONEntity.Primitive(false);
                }
                if (string.Equals(str, Constants.Tokens.JSONVoidNaN)
                    || string.Equals(str, Constants.Tokens.JSONVoidNull, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(str, Constants.Tokens.JSONVoidUndefined, StringComparison.OrdinalIgnoreCase)) {
                    return JSONEntity.Void;
                }
                if (str.CouldCastToNumber()) {
                    return JSONEntity.Primitive(str.ToNumber());
                }

                return JSONEntity.Primitive(str.Unescape());

            }

        }

    }

}
