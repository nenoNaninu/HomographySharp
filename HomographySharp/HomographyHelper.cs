using System.Numerics;
using System.Collections.Generic;
using HomographySharp.Double;
using HomographySharp.Single;

namespace HomographySharp
{
    public static class HomographyHelper
    {
        public static HomographyMatrix<float> FindHomography(IReadOnlyList<Point2<float>> srcPoints, IReadOnlyList<Point2<float>> dstPoints)
            => SingleHomographyHelper.FindHomography(srcPoints, dstPoints);

        public static HomographyMatrix<float> FindHomography(IReadOnlyList<Vector2> srcPoints, IReadOnlyList<Vector2> dstPoints)
            => SingleHomographyHelper.FindHomography(srcPoints, dstPoints);

        public static HomographyMatrix<double> FindHomography(IReadOnlyList<Point2<double>> srcPoints, IReadOnlyList<Point2<double>> dstPoints)
            => DoubleHomographyHelper.FindHomography(srcPoints, dstPoints);

        public static Point2<float> Translate(HomographyMatrix<float> homographyMatrix, float srcX, float srcY)
            => homographyMatrix.Translate(srcX, srcY);

        public static Point2<double> Translate(HomographyMatrix<double> homographyMatrix, double srcX, double srcY)
            => homographyMatrix.Translate(srcX, srcY);

        public static Vector2 AsVector2(this Point2<float> source) => new Vector2(source.X, source.Y);
    }
}