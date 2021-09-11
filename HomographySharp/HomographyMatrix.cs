using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using System.Text.Json.Serialization;

namespace HomographySharp
{
    [JsonConverter(typeof(HomographyJsonConverter))]
    public abstract class HomographyMatrix<T> where T : unmanaged, IEquatable<T>, IFormattable
    {
        public abstract Point2<T> Translate(T srcX, T srcY);

        /// <summary>
        /// Row-major order
        /// </summary>
        public abstract IReadOnlyList<T> Elements { get; }

        /// <summary>
        /// Row-major order
        /// </summary>
        public abstract ReadOnlySpan<T> ElementsAsSpan();

        public abstract T this[int row, int column] { get; }

        public abstract int RowCount { get; }

        public abstract int ColumnCount { get; }

        public abstract Matrix<T> ToMathNetMatrix();

        internal HomographyMatrix()
        {
        }
    }
}