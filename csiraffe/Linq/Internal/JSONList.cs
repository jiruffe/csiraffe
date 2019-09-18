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

using Jiruffe.CSiraffe.Utility;

namespace Jiruffe.CSiraffe.Linq.Internal {

    /// <summary>
    /// JSON list [].
    /// </summary>
    internal sealed class JSONList : JSONElement {

        #region Fields

        private readonly IList<JSONElement> _Sub_Elements;

        #endregion

        #region Indexers
        #endregion

        #region Accessors
        #endregion

        #region Constructors

        internal JSONList() : this(Defaults<JSONElement>.List) {
        }

        internal JSONList(in IList<JSONElement> elements) {
            _Sub_Elements = elements;
        }

        #endregion

        #region Methods

        public override IList<JSONElement> AsList() {
            return _Sub_Elements;
        }

        #endregion

    }

}
