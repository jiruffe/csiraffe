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

namespace Jiruffe.CSiraffe.Analyzer {

    /// <summary>
    /// JSON <see cref="string"/> &lt;=&gt; <see cref="object"/> conversion.
    /// </summary>
    internal static class DirectAnalyzer {

        /// <summary>
        /// <see cref="object"/> =&gt; JSON <see cref="string"/> conversion.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be converted.</param>
        /// <returns>The JSON <see cref="string"/> converted.</returns>
        internal static string Analyze(object obj) {
            return default;
        }

        /// <summary>
        /// JSON <see cref="string"/> =&gt; <see cref="object"/> conversion.
        /// </summary>
        /// <typeparam name="T">The target <see cref="Type"/>.</typeparam>
        /// <param name="str">The JSON <see cref="string"/> to be converted.</param>
        /// <returns>The target <see cref="Type"/> <see cref="object"/> converted.</returns>
        internal static T Analyze<T>(string str) {
            return default;
        }

    }

}
