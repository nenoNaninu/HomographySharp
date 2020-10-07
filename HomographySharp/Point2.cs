using System;
using System.Collections.Generic;

namespace HomographySharp
{
    public readonly struct Point2<T> : IEquatable<Point2<T>>
        where T : struct
    {
        public readonly T X;
        public readonly T Y;

        public Point2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point2<T> other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Point2<T> other && Equals(other);
        }

        public static bool operator ==(Point2<T> a, Point2<T> b) => a.Equals(b);
        public static bool operator !=(Point2<T> a, Point2<T> b) => !a.Equals(b);

        public override int GetHashCode()
        {
#if NETSTANDARD2_1
            return HashCode.Combine(X, Y);
#else
            return EqualityComparer<T>.Default.GetHashCode(X) ^ EqualityComparer<T>.Default.GetHashCode(Y);
#endif
        }
    }
}