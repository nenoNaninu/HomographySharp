using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace HomographySharp.Double
{
    internal class DoubleHomographyMatrix : HomographyMatrix<double>
    {
        /// <summary>
        /// Row-major order
        /// </summary>
        private readonly double[] _elements;

        internal DoubleHomographyMatrix(double[] elements)
        {
            _elements = elements;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<double> Elements => _elements;

        /// <inheritdoc/>
        public override ReadOnlySpan<double> ElementsAsSpan() => _elements;

        public override int RowCount => 3;
        
        public override int ColumnCount => 3;

        public override double this[int row, int column]
        {
            get
            {
                if (0 <= row && row < 3 && 0 <= column && column < 3)
                {
                    return _elements[3 * row + column];
                }

                throw new ArgumentOutOfRangeException($"{nameof(row)} and {nameof(column)} must be greater than or equal to 0 and less than 3. The current arguments are {nameof(row)} = {row} and {nameof(column)} = {column}.");
            }
        }


#if NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public override Point2<double> Translate(double srcX, double srcY)
        {
            var dst1 = _elements[0] * srcX + _elements[1] * srcY + _elements[2];
            var dst2 = _elements[3] * srcX + _elements[4] * srcY + _elements[5];
            var dst3 = _elements[6] * srcX + _elements[7] * srcY + _elements[8];

            return new Point2<double>(dst1 / dst3, dst2 / dst3);
        }

        public override Matrix<double> ToMathNetMatrix()
        {
            var mat = new DenseMatrix(3, 3);
            var values = mat.Values;

            values[0] = _elements[0];
            values[1] = _elements[3];
            values[2] = _elements[6];

            values[3] = _elements[1];
            values[4] = _elements[4];
            values[5] = _elements[7];

            values[6] = _elements[2];
            values[7] = _elements[5];
            values[8] = _elements[8];
            return mat;
        }

        public override string ToString()
        {
            Span<int> paddingBuffer = stackalloc int[3];
            string[] stringBuffer = ArrayPool<string>.Shared.Rent(9);

            try
            {
                for (int i = 0; i < 9; i++)
                {
                    stringBuffer[i] = _elements[i].ToString("G6");
                }

                for (int i = 0; i < 3; i++)
                {
                    var length1 = stringBuffer[i].Length;
                    var length2 = stringBuffer[i + 3].Length;
                    var length3 = stringBuffer[i + 6].Length;
                    paddingBuffer[i] = Math.Max(Math.Max(length1, length2), length3);
                }

                var stringBuilder = new StringBuilder();
                for (int i = 0; i <= 6; i += 3)
                {
                    stringBuilder.Append(stringBuffer[i].PadLeft(paddingBuffer[0]));
                    stringBuilder.Append(", ");
                    stringBuilder.Append(stringBuffer[i + 1].PadLeft(paddingBuffer[1]));
                    stringBuilder.Append(", ");
                    stringBuilder.Append(stringBuffer[i + 2].PadLeft(paddingBuffer[2]));
                    stringBuilder.Append(Environment.NewLine);
                }

                return stringBuilder.ToString();
            }
            finally
            {
                ArrayPool<string>.Shared.Return(stringBuffer);
            }
        }
    }
}