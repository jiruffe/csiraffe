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
using System.Linq;
using System.Text;

namespace Jiruffe.CSiraffe.Utility {

    internal static class StringExtensionMethods{

        internal static string Escape(this string str) {

            StringBuilder sb = new StringBuilder();

            foreach (var c in str) {

                if (Constants.Characters.ESCAPABLE_CHARACTER.Contains(c)) {
                    sb.Append(Constants.Characters.ESCAPE_SYMBOL);
                    sb.Append(Constants.Characters.ESCAPE_CHARACTER[c]);
                } else if (c < Constants.Characters.VISIBLE_ASCII_CHARACTER_WITH_MIN_CODE || c > Constants.Characters.VISIBLE_ASCII_CHARACTER_WITH_MAX_CODE) {
                    sb.Append(Constants.Characters.ESCAPE_SYMBOL);
                    sb.Append(Constants.Characters.UNICODE_SYMBOL);
                    sb.Append(Constants.Characters.DIGIT_TO_HEX_CHARACTER[(((uint)c) >> 12) & 0xf]);
                    sb.Append(Constants.Characters.DIGIT_TO_HEX_CHARACTER[(((uint)c) >> 8) & 0xf]);
                    sb.Append(Constants.Characters.DIGIT_TO_HEX_CHARACTER[(((uint)c) >> 4) & 0xf]);
                    sb.Append(Constants.Characters.DIGIT_TO_HEX_CHARACTER[c & 0xf]);
                } else {
                    sb.Append(c);
                }

            }

            return sb.ToString();

        }

        internal static string Unescape(this string str) {

            StringBuilder sb = new StringBuilder();

            for (var i = 0; i < str.Length; i++) {

                var c = str[i];

                if (Constants.Characters.ESCAPE_SYMBOL == c && i < str.Length - 1) {

                    c = str[++i];

                    if (Constants.Characters.UNESCAPABLE_CHARACTER.Contains(c)) {
                        sb.Append(Constants.Characters.UNESCAPE_CHARACTER[c]);
                    } else if (Constants.Characters.UNICODE_SYMBOL == c && i < str.Length - 4) {
                        int d = 0;
                        for (int j = 0; j < 4; j++) {
                            d = (d << 4) + Constants.Characters.HEX_CHARACTER_TO_DIGIT[str[++i]];
                        }
                        sb.Append((char)d);
                    } else {
                        sb.Append(Constants.Characters.ESCAPE_SYMBOL);
                        sb.Append(c);
                    }

                } else {
                    sb.Append(c);
                }

            }

            return sb.ToString();

        }

        internal static bool SurroundedWith(this string str, string value) {
            return str.StartsWith(value) && str.EndsWith(value);
        }

        internal static bool SurroundedWith(this string str, char value) {
            return str.StartsWith(value) && str.EndsWith(value);
        }

    }

}
