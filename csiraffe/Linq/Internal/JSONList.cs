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
    internal sealed class JSONList : JSONEntity {

        #region Fields

        private readonly IList<JSONEntity> _Sub_Entities;

        #endregion

        #region Indexers
        #endregion

        #region Accessors
        #endregion

        #region Constructors

        internal JSONList() : this(Defaults<JSONEntity>.List) {
        }

        internal JSONList(in IList<JSONEntity> entities) {
            _Sub_Entities = entities;
        }

        #endregion

        #region Methods

        public override IList<JSONEntity> AsList() {
            return _Sub_Entities;
        }

        #endregion

    }

}
