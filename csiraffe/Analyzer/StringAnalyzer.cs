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
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            Stack<JSONEntity> bases = new Stack<JSONEntity>();
            Stack<string> keys = new Stack<string>();
            StringBuilder sb = new StringBuilder();
            char last_token = Constants.Characters.NULL;

            foreach (var c in str) {

                if (Constants.Characters.QUOTE == last_token || Constants.Characters.APOSTROPHE == last_token) {
                    if (last_token == c) {
                        last_token = Constants.Characters.NULL;
                    }
                    sb.Append(c);
                    continue;
                }

                switch (c) {

                    case Constants.Characters.QUOTE:
                    case Constants.Characters.APOSTROPHE:
                        sb.Append(c);
                        last_token = c;
                        break;

                    case Constants.Tokens.JSONDictionaryStart:
                        bases.Push(JSONEntity.Dictionary());
                        last_token = c;
                        break;

                    case Constants.Tokens.JSONListStart:
                        bases.Push(JSONEntity.List());
                        last_token = c;
                        break;

                    case Constants.Tokens.JSONDictionaryKey:
                        var tkey = sb.ToString();
                        keys.Push((tkey.SurroundedWith(Constants.Characters.APOSTROPHE) || tkey.SurroundedWith(Constants.Characters.QUOTE) ? tkey.Substring(1, tkey.Length - 1) : tkey).Unescape()); ;
                        sb.Clear();
                        last_token = c;
                        break;

                    case Constants.Tokens.JSONSeparator:
                        JSONEntity current_entity = bases.Peek();
                        if (current_entity.IsDictionary) {
                            current_entity.AsDictionary().Add(keys.Pop(), Convert.FromString(sb.ToString()));
                        } else if (current_entity.IsList) {
                            current_entity.AsList().Add(Convert.FromString(sb.ToString()));
                        }
                        sb.Clear();
                        last_token = c;
                        break;

                    case Constants.Tokens.JSONDictionaryEnd:
                        JSONEntity current_dictionary = bases.Pop();
                        current_dictionary.AsDictionary().Add(keys.Pop(), Convert.FromString(sb.ToString()));
                        sb.Clear();
                        last_token = c;
                        if (0 >= bases.Count) {
                            return current_dictionary;
                        } else {
                            JSONEntity base_entity = bases.Peek();
                            if (base_entity.IsDictionary) {
                                base_entity.AsDictionary().Add(keys.Pop(), current_dictionary);
                            } else if (base_entity.IsList) {
                                base_entity.AsList().Add(current_dictionary);
                            }
                        }
                        break;

                    case Constants.Tokens.JSONListEnd:
                        JSONEntity current_list = bases.Pop();
                        current_list.AsList().Add(Convert.FromString(sb.ToString()));
                        sb.Clear();
                        last_token = c;
                        if (0 >= bases.Count) {
                            return current_list;
                        } else {
                            JSONEntity base_entity = bases.Peek();
                            if (base_entity.IsDictionary) {
                                base_entity.AsDictionary().Add(keys.Pop(), current_list);
                            } else if (base_entity.IsList) {
                                base_entity.AsList().Add(current_list);
                            }
                        }
                        break;

                    default:
                        if (0 < sb.Length || c.IsVisibleAndNotSpace()) {
                            last_token = Constants.Characters.NULL;
                            sb.Append(c);
                        }
                        break;

                }

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

            StringBuilder sb = new StringBuilder();

            switch (entity.EntityType) {

                case JSONEntityType.Dictionary:
                    sb.Append(Constants.Tokens.JSONDictionaryStart);
                    sb.Append(string.Join(Constants.Tokens.JSONSeparator, from e in entity.AsDictionary() select $"{Constants.Characters.QUOTE}{e.Key}{Constants.Characters.QUOTE}{Constants.Tokens.JSONDictionaryKey}{Analyze(e.Value)}"));
                    sb.Append(Constants.Tokens.JSONDictionaryEnd);
                    break;

                case JSONEntityType.List:
                    sb.Append(Constants.Tokens.JSONDictionaryStart);
                    sb.Append(string.Join(Constants.Tokens.JSONSeparator, from e in entity.AsList() select Analyze(e)));
                    sb.Append(Constants.Tokens.JSONDictionaryEnd);
                    break;

                case JSONEntityType.Primitive:
                    object v = entity.AsPrimitive();
                    if (v is string) {
                        sb.Append(Constants.Characters.QUOTE);
                        sb.Append(v as string);
                        sb.Append(Constants.Characters.QUOTE);
                    } else {
                        sb.Append(System.Convert.ToString(v));
                    }
                    break;

                case JSONEntityType.Void:
                    sb.Append(Constants.Tokens.JSONVoidNull);
                    break;

                default:
                    break;

            }

            return sb.ToString();

        }

        private static class Convert {

            internal static JSONEntity FromString(string str) {

                if (str is null || 0 >= str.Length) {
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
