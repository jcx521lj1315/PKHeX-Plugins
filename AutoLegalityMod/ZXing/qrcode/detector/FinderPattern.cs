/*
* Copyright 2007 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using System;

namespace com.google.zxing.qrcode.detector
{
    /// <summary> <p>Encapsulates a finder pattern, which are the three square patterns found in
    /// the corners of QR Codes. It also encapsulates a count of similar finder patterns,
    /// as a convenience to the finder's bookkeeping.</p>
    ///
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>
    public sealed class FinderPattern : ResultPoint
    {
        public float EstimatedModuleSize { get; }

        internal int Count { get; private set; }

        //UPGRADE_NOTE: Final was removed from the declaration of 'estimatedModuleSize '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        internal FinderPattern(float posX, float posY, float estimatedModuleSize) : base(posX, posY)
        {
            EstimatedModuleSize = estimatedModuleSize;
            Count = 1;
        }

        internal void IncrementCount()
        {
            Count++;
        }

        /// <summary> <p>Determines if this finder pattern "about equals" a finder pattern at the stated
        /// position and size -- meaning, it is at nearly the same center with nearly the same size.</p>
        /// </summary>
        internal bool AboutEquals(float moduleSize, float i, float j)
        {
            if (Math.Abs(i - Y) <= moduleSize && Math.Abs(j - X) <= moduleSize)
            {
                float moduleSizeDiff = Math.Abs(moduleSize - EstimatedModuleSize);
                return moduleSizeDiff <= 1.0f || moduleSizeDiff / EstimatedModuleSize <= 1.0f;
            }
            return false;
        }
    }
}