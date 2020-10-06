using System.Collections.Generic;
using HomographySharp.Double;
using HomographySharp.Single;
#if NETSTANDARD2_1
using System.Numerics;
#endif

namespace HomographySharp
{
    public static class HomographyHelper
    {
        public static HomographyMatrix<float> FindHomography(IReadOnlyList<Vector2<float>> srcPoints, IReadOnlyList<Vector2<float>> dstPoints)
            => SingleHomographyHelper.FindHomography(srcPoints, dstPoints);

#if NETSTANDARD2_1
        public static HomographyMatrix<float> FindHomography(IReadOnlyList<Vector2> srcPoints, IReadOnlyList<Vector2> dstPoints)
            => SingleHomographyHelper.FindHomography(srcPoints, dstPoints);
#endif

        public static HomographyMatrix<double> FindHomography(IReadOnlyList<Vector2<double>> srcPoints, IReadOnlyList<Vector2<double>> dstPoints)
            => DoubleHomographyHelper.FindHomography(srcPoints, dstPoints);

        public static Vector2<float> Translate(HomographyMatrix<float> homographyMatrix, float srcX, float srcY)
            => homographyMatrix.Translate(srcX, srcY);

        public static Vector2<double> Translate(HomographyMatrix<double> homographyMatrix, double srcX, double srcY)
            => homographyMatrix.Translate(srcX, srcY);

#if NETSTANDARD2_1
        public static Vector2 AsVector2(this Vector2<float> source) => new Vector2(source.X, source.Y);
#endif
    }
}