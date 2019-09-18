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
using System.Collections;
using System.Collections.Generic;

namespace Jiruffe.CSiraffe.Utility {

    internal static class Defaults {

        internal static object Primitive {
            get {
                return 0;
            }
        }

    }

    internal static class Defaults<T> {

        internal static ICollection<T> Collection {
            get {
                return List;
            }
        }

        internal static IList<T> List {
            get {
                return new List<T>();
            }
        }

    }

    internal static class Defaults<K, V> {

        internal static IDictionary<K, V> Dictionary {
            get {
                return new Dictionary<K, V>();
            }
        }

    }

}