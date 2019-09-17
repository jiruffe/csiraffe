﻿#region License
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

namespace Jiruffe.CSiraffe.Exception {

    /// <summary>
    /// Represents <see cref="JSONException"/> occurred if operation is not acceptable.
    /// </summary>
    public class UnsupportedOperationException : JSONException {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOperationException"/> class.
        /// </summary>
        public UnsupportedOperationException() : base() {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOperationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public UnsupportedOperationException(string message) : base(message) {

        }

        #endregion

    }

}
