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

using Jiruffe.CSiraffe.Utility;

namespace Jiruffe.CSiraffe.Linq.Internal {

    /// <summary>
    /// JSON primitive value such as integer, string...
    /// </summary>
    internal sealed class JSONPrimitive : JSONElement {

        #region Fields

        private readonly object _Value;

        #endregion

        #region Indexers
        #endregion

        #region Accessors
        #endregion

        #region Constructors

        internal JSONPrimitive() : this(Defaults.Primitive) {
        }

        internal JSONPrimitive(in object obj) {
            _Value = obj;
        }

        #endregion

        #region Methods

        public override object AsPrimitive() {
            return _Value;
        }

        #endregion

    }

}
