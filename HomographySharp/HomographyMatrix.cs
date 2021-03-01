using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace HomographySharp
{
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

        public abstract Matrix<T> ToMathNetMatrix();

        internal HomographyMatrix()
        {
        }
    }
}