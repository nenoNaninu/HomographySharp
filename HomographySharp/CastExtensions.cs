using System.Drawing;
using System.Numerics;

namespace HomographySharp
{
    public static class CastExtensions
    {
        public static Vector2 AsVector2(this Point2<float> source) => new Vector2(source.X, source.Y);
        public static PointF AsPointF(this Point2<float> source) => new PointF(source.X, source.Y);
        public static Point2<float> AsPoint2(this Vector2 source) => new Point2<float>(source.X, source.Y);
        public static Point2<float> AsPoint2(this PointF source) => new Point2<float>(source.X, source.Y);
    }
}