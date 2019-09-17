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

namespace Jiruffe.CSiraffe.Linq {

    /// <summary>
    /// JSON <c>null</c>, <c>undefined</c> or <c>NaN</c>.
    /// </summary>
    internal sealed class JSONVoid : JSONElement {

        /// <summary>
        /// Lazy-Load singleton instance of <see cref="JSONVoid"/>.
        /// </summary>
        private static readonly Lazy<JSONVoid> _Lazy_Instance;

        /// <summary>
        /// Static constructor. Initialize the Lazy-Load singleton instance.
        /// </summary>
        static JSONVoid() {
            _Lazy_Instance = new Lazy<JSONVoid>(() => new JSONVoid());
        }

        /// <summary>
        /// Lazy-Load singleton instance of <see cref="JSONVoid"/>.
        /// </summary>
        internal JSONVoid Instance {
            get {
                return _Lazy_Instance.Value;
            }
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private JSONVoid() {
        }

    }

}
