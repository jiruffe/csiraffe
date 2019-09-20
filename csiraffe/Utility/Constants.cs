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

namespace Jiruffe.CSiraffe.Utility {

    internal static class Constants {

        internal static class Tokens {

            internal const string JSONDictionaryStart = "{";

            internal const string JSONDictionaryEnd = "}";

            internal const string JSONListStart = "[";

            internal const string JSONListEnd = "]";

            internal const string JSONVoidNaN = "NaN";

            internal const string JSONVoidNull = "null";

            internal const string JSONVoidUndefined = "undefined";

            internal const string JSONPrimitiveTrue = "true";

            internal const string JSONPrimitiveFalse = "false";

        }

        internal static class Characters {

            internal static readonly char[] DIGIT_TO_HEX_CHARACTER = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            internal static readonly int[] HEX_CHARACTER_TO_DIGIT = new int['f' + 1];
            internal static readonly char[] ESCAPE_CHARACTER = new char[93];
            internal static readonly char[] UNESCAPE_CHARACTER = new char[120];
            internal static readonly char[] ESCAPABLE_CHARACTER = { '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\u0007', '\b', '\t', '\n', '\u000B', '\f', '\r', '\"', '\'', '/', '\\' };
            internal static readonly char[] UNESCAPABLE_CHARACTER = { '0', '1', '2', '3', '4', '5', '6', '7', 'b', 't', 'n', 'v', 'f', 'r', '"', '\'', '/', '\\' };
            internal const char VISIBLE_ASCII_CHARACTER_WITH_MIN_CODE = ' ';
            internal const char VISIBLE_ASCII_CHARACTER_WITH_MAX_CODE = '~';
            internal const char ESCAPE_SYMBOL = '\\';
            internal const char UNICODE_SYMBOL = 'u';
            internal const char APOSTROPHE = '\'';
            internal const char QUOTE = '"';

            static Characters() {

                for (var i = '0'; i <= '9'; i++) {
                    HEX_CHARACTER_TO_DIGIT[i] = i - '0';
                }
                for (var i = 'a'; i <= 'f'; i++) {
                    HEX_CHARACTER_TO_DIGIT[i] = i - 'a' + 10;
                }
                for (var i = 'A'; i <= 'F'; i++) {
                    HEX_CHARACTER_TO_DIGIT[i] = i - 'A' + 10;
                }

                ESCAPE_CHARACTER['\0'] = '0';
                ESCAPE_CHARACTER['\u0001'] = '1';
                ESCAPE_CHARACTER['\u0002'] = '2';
                ESCAPE_CHARACTER['\u0003'] = '3';
                ESCAPE_CHARACTER['\u0004'] = '4';
                ESCAPE_CHARACTER['\u0005'] = '5';
                ESCAPE_CHARACTER['\u0006'] = '6';
                ESCAPE_CHARACTER['\u0007'] = '7';
                ESCAPE_CHARACTER['\b'] = 'b';
                ESCAPE_CHARACTER['\t'] = 't';
                ESCAPE_CHARACTER['\n'] = 'n';
                ESCAPE_CHARACTER['\u000B'] = 'v';
                ESCAPE_CHARACTER['\f'] = 'f';
                ESCAPE_CHARACTER['\r'] = 'r';
                ESCAPE_CHARACTER['\"'] = '"';
                ESCAPE_CHARACTER['\''] = '\'';
                ESCAPE_CHARACTER['/'] = '/';
                ESCAPE_CHARACTER['\\'] = '\\';

                UNESCAPE_CHARACTER['0'] = '\0';
                UNESCAPE_CHARACTER['1'] = '\u0001';
                UNESCAPE_CHARACTER['2'] = '\u0002';
                UNESCAPE_CHARACTER['3'] = '\u0003';
                UNESCAPE_CHARACTER['4'] = '\u0004';
                UNESCAPE_CHARACTER['5'] = '\u0005';
                UNESCAPE_CHARACTER['6'] = '\u0006';
                UNESCAPE_CHARACTER['7'] = '\u0007';
                UNESCAPE_CHARACTER['b'] = '\b';
                UNESCAPE_CHARACTER['t'] = '\t';
                UNESCAPE_CHARACTER['n'] = '\n';
                UNESCAPE_CHARACTER['v'] = '\u000B';
                UNESCAPE_CHARACTER['f'] = '\f';
                UNESCAPE_CHARACTER['r'] = '\r';
                UNESCAPE_CHARACTER['"'] = '\"';
                UNESCAPE_CHARACTER['\''] = '\'';
                UNESCAPE_CHARACTER['/'] = '/';
                UNESCAPE_CHARACTER['\\'] = '\\';

            }

        }

    }

}
