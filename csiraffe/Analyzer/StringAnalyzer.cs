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

using Jiruffe.CSiraffe.Linq;

namespace Jiruffe.CSiraffe.Analyzer {

    /// <summary>
    /// JSON <see cref="string"/> &lt;=&gt; <see cref="JSONElement"/> conversion.
    /// </summary>
    internal static class StringAnalyzer {

        /// <summary>
        /// JSON <see cref="string"/> =&gt; <see cref="JSONElement"/> conversion.
        /// </summary>
        /// <param name="str">The JSON <see cref="string"/> to be converted.</param>
        /// <returns>The <see cref="JSONElement"/> converted.</returns>
        internal static JSONElement Analyze(string str) {
            return default;
        }

        /// <summary>
        /// <see cref="JSONElement"/> =&gt; JSON <see cref="string"/> conversion.
        /// </summary>
        /// <param name="element">The <see cref="JSONElement"/> to be converted.</param>
        /// <returns>The JSON <see cref="string"/> converted.</returns>
        internal static string Analyze(JSONElement element) {
            return default;
        }

    }

}
