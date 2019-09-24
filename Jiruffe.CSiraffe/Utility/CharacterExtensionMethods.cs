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

    internal static class CharacterExtensionMethods {

        internal static bool IsVisible(this char c) {
            return !c.IsInvisible();
        }

        internal static bool IsVisibleAndNotSpace(this char c) {
            return !c.IsInvisibleOrSpace();
        }

        internal static bool IsInvisible(this char c) {
            return c < 32 || c == 127;
        }

        internal static bool IsInvisibleOrSpace(this char c) {
            return c <= 32 || c == 127;
        }

    }

}
