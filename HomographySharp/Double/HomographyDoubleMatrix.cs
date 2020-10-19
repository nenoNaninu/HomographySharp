using System;
using System.Collections.Generic;

namespace HomographySharp.Double
{
    internal class HomographyDoubleMatrix : HomographyMatrix<double>
    {
        private readonly double[] _elements;

        internal HomographyDoubleMatrix(double[] elements)
        {
            _elements = elements;
        }

        public override IReadOnlyList<double> Elements => _elements;

        public override ReadOnlySpan<double> ElementsAsSpan() => _elements;

        public override Point2<double> Translate(double srcX, double srcY)
        {
            var dst1 = _elements[0] * srcX + _elements[1] * srcY + _elements[2];
            var dst2 = _elements[3] * srcX + _elements[4] * srcY + _elements[5];
            var dst3 = _elements[6] * srcX + _elements[7] * srcY + _elements[8];

            return new Point2<double>(dst1 / dst3, dst2 / dst3);
        }
    }
}