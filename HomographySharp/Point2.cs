using System;

namespace HomographySharp;

public readonly record struct Point2<T> : IEquatable<Point2<T>>
    where T : unmanaged, IEquatable<T>
{
    public readonly T X;
    public readonly T Y;

    public Point2(T x, T y)
    {
        X = x;
        Y = y;
    }
}
