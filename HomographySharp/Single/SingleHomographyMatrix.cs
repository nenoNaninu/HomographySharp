using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace HomographySharp.Single
{
    internal class SingleHomographyMatrix : HomographyMatrix<float>
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

        public override Matrix<float> ToMathNetMatrix()
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
                    var length2 = stringBuffer[3 + i].Length;
                    var length3 = stringBuffer[6 + i].Length;
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

#if NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public override Point2<float> Translate(float srcX, float srcY)
        {
            var dst1 = _elements[0] * srcX + _elements[1] * srcY + _elements[2];
            var dst2 = _elements[3] * srcX + _elements[4] * srcY + _elements[5];
            var dst3 = _elements[6] * srcX + _elements[7] * srcY + _elements[8];

            return new Point2<float>(dst1 / dst3, dst2 / dst3);
        }
    }
}