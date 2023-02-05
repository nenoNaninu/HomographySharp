using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MathNet.Numerics.LinearAlgebra;

namespace HomographySharp;

[JsonConverter(typeof(HomographyMatrixJsonConverter))]
public abstract class HomographyMatrix<T> where T : unmanaged, IEquatable<T>, IFormattable
{
    internal HomographyMatrix()
    {
    }

    /// <summary>
    /// Row-major order
    /// </summary>
    public abstract IReadOnlyList<T> Elements { get; }

    public abstract T this[int row, int column] { get; }

    public abstract int RowCount { get; }

    public abstract int ColumnCount { get; }

    public abstract Point2<T> Translate(T srcX, T srcY);

    public abstract Matrix<T> ToMathNetMatrix();

    /// <summary>
    /// Row-major order
    /// </summary>
    public abstract ReadOnlySpan<T> ElementsAsSpan();

    /// <summary>
    /// Row-major order
    /// </summary>
    public abstract ReadOnlyMemory<T> ElementsAsMemory();

    public abstract string ToString(IFormatProvider? provider);

    public abstract string ToString(string? format);

    public abstract string ToString(string? format, IFormatProvider? provider);
}
