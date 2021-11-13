using System.Drawing;
using System.Numerics;

namespace HomographySharp;

public static class TypeConvertExtensions
{
    public static Vector2 ToVector2(this Point2<float> source) => new Vector2(source.X, source.Y);
    public static PointF ToPointF(this Point2<float> source) => new PointF(source.X, source.Y);

    public static Point2<float> ToPoint2(this Vector2 source) => new Point2<float>(source.X, source.Y);
    public static Point2<float> ToPoint2(this PointF source) => new Point2<float>(source.X, source.Y);
}
