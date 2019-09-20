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

        #region Escape Unescape

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

        #endregion

        #region SurroundedWith

        internal static bool SurroundedWith(this string str, string value) {
            return str.StartsWith(value) && str.EndsWith(value);
        }

        internal static bool SurroundedWith(this string str, char value) {
            return str.StartsWith(value) && str.EndsWith(value);
        }

        #endregion

        #region Numberic

        internal static bool IsNumberic(this string str) {

            foreach (var c in str) {
                if ('0' > c || '9' < c) {
                    return false;
                }
            }

            return true;

        }

        internal static bool IsBCPLStyleNumeric(this string str) {

            if (str.StartsWith('-')) {
                str = str.Substring(1);
            }

            int len = str.Length;

            if (len < 2) {
                return false;
            }

            char c0 = str[0];
            char c1 = str[1];

            if (c0 == '0') {
                if (c1 == 'x' && len >= 3) {
                    for (int i = 2; i < len; i++) {
                        char c = str[i];
                        if (!('0' <= c && '9' >= c) && !('a' <= c && 'f' >= c) && !('A' <= c && 'F' >= c)) {
                            return false;
                        }
                    }
                    return true;
                } else if (c1 == 'b' && len >= 3) {
                    for (int i = 2; i < len; i++) {
                        char c = str[i];
                        if ('0' != c && '1' != c) {
                            return false;
                        }
                    }
                    return true;
                } else {
                    for (int i = 1; i < len; i++) {
                        char c = str[i];
                        if ('0' > c || '7' < c)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            return false;

        }

        internal static bool IsRealNumber(this string str) {

            if (str.StartsWith('-')) {
                str = str.Substring(1);
            }

            if (0 >= str.Length) {
                return false;
            }

            int index = str.IndexOf('.');

            if (index < 0) {

                return str.IsNumberic();

            } else {

                return str.Substring(0, index).IsNumberic() && str.Substring(index + 1).IsNumberic();

            }

        }

        internal static bool IsScientificNotationNumber(this string str) {

            int index = str.IndexOf('E');

            return str.Substring(0, index).IsRealNumber() && str.Substring(index + 1).IsRealNumber();

        }

        internal static bool CouldCastToNumber(this string str) {

            if (str is null) {
                return false;
            }

            return str.IsRealNumber() || str.IsScientificNotationNumber() || str.IsBCPLStyleNumeric();

        }

        internal static object ToNumber(this string str) {

            if (!str.CouldCastToNumber()) {
                return default(int);
            }

            if (str.IsRealNumber()) {
                if (str.Contains('.')) {
                    return double.Parse(str);
                }
                return long.Parse(str);
            }
            if (str.IsScientificNotationNumber()) {
                int index = str.IndexOf('E');
                return double.Parse(str.Substring(0, index)) * Math.Pow(10, double.Parse(str.Substring(index + 1)));
            }
            if (str.IsBCPLStyleNumeric()) {
                long positive_rst;
                bool negative = false;
                if (str.StartsWith('-')) {
                    negative = true;
                    str = str.Substring(1);
                }
                if (str.StartsWith("0x")) {
                    positive_rst = Convert.ToInt64(str.Substring(2), 16);
                } else if (str.StartsWith("0b")) {
                    positive_rst = Convert.ToInt64(str.Substring(2), 2);
                } else {
                    positive_rst = Convert.ToInt64(str.Substring(1), 8);
                }
                return negative ? -positive_rst : positive_rst;
            }

            return default(int);

        }

        #endregion

    }

}
