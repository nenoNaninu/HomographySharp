using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace HomographySharp.Single
{
    internal sealed class SingleHomographyMatrix : HomographyMatrix<float>
    {
        /// <summary>
        /// Row-major order
        /// </summary>
        private readonly float[] _elements;

        internal SingleHomographyMatrix(float[] elements)
        {
            _elements = elements;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<float> Elements => _elements;

        /// <inheritdoc/>
        public override ReadOnlySpan<float> ElementsAsSpan() => _elements;

        public override int RowCount => 3;

        public override int ColumnCount => 3;

        public override float this[int row, int column]
        {
            get
            {
                if (0 <= row && row < 3 && 0 <= column && column < 3)
                {
                    return _elements[row * 3 + column];
                }

                throw new ArgumentOutOfRangeException($"{nameof(row)} and {nameof(column)} must be greater than or equal to 0 and less than 3. The current arguments are {nameof(row)} = {row} and {nameof(column)} = {column}.");
            }
        }

#if NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public override Point2<float> Translate(float srcX, float srcY)
        {
            var elements = _elements;

            var dst1 = elements[0] * srcX + elements[1] * srcY + elements[2];
            var dst2 = elements[3] * srcX + elements[4] * srcY + elements[5];
            var dst3 = elements[6] * srcX + elements[7] * srcY + elements[8];

            return new Point2<float>(dst1 / dst3, dst2 / dst3);
        }

        public override Matrix<float> ToMathNetMatrix()
        {
            var mat = new DenseMatrix(3, 3);

            var values = mat.Values;
            var elements = _elements;

            values[0] = elements[0];
            values[1] = elements[3];
            values[2] = elements[6];

            values[3] = elements[1];
            values[4] = elements[4];
            values[5] = elements[7];

            values[6] = elements[2];
            values[7] = elements[5];
            values[8] = elements[8];

            return mat;
        }

        public override string ToString()
        {
            Span<int> paddingBuffer = stackalloc int[3];
            string[] stringBuffer = ArrayPool<string>.Shared.Rent(9);

            try
            {
                for (int i = 0; i < _elements.Length; i++)
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
                    stringBuilder.Append(' ');
                    stringBuilder.Append(stringBuffer[i + 1].PadLeft(paddingBuffer[1]));
                    stringBuilder.Append(' ');
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
