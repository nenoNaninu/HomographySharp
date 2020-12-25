using System;
using System.Collections.Generic;

namespace HomographySharp.Single
{
    internal class SingleHomographyMatrix : HomographyMatrix<float>
    {
        private readonly float[] _elements;

        internal SingleHomographyMatrix(float[] elements)
        {
            _elements = elements;
        }

        public override IReadOnlyList<float> Elements => _elements;

        public override ReadOnlySpan<float> ElementsAsSpan() => _elements;

        public override Point2<float> Translate(float srcX, float srcY)
        {
            var dst1 = _elements[0] * srcX + _elements[1] * srcY + _elements[2];
            var dst2 = _elements[3] * srcX + _elements[4] * srcY + _elements[5];
            var dst3 = _elements[6] * srcX + _elements[7] * srcY + _elements[8];

            return new Point2<float>(dst1 / dst3, dst2 / dst3);
        }
    }
}