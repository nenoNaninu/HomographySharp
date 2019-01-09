using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace HomographySharp
{
    public static class HomographyHelper
    {
        /// <summary>
        /// 4点以上入れても最初の4点しか考慮しない
        /// 引数のVectorは2次元(x,y)であること
        /// </summary>
        /// <param name="srcPoints">変換前の4点</param>
        /// <param name="dstPoints">変換後の4点</param>
        /// <returns>3x3の射影変換行列</returns>
        public static DenseMatrix FindHomography(List<DenseVector> srcPoints, List<DenseVector> dstPoints)
        {
            if (srcPoints.Count != 4 || dstPoints.Count != 4)
            {
                throw new ArgumentException("srcPointsもしくはdstPointsはそれぞれ4点必要です。");
            }

            //q(dstのベクトル) = A(作成するべき8x8行列) * P(射影変換のパラメータ)
            //P = A^-1 * q
            //でパラメータが求まる。
            DenseMatrix a = DenseMatrix.Create(8, 8, 0);

            for (int i = 0; i < 4; i++)
            {
                var src = srcPoints[i];
                var dst = dstPoints[i];

                var srcX = src[0];
                var dstX = dst[0];

                var srcY = src[1];
                var dstY = dst[1];

                var row1 = DenseVector.OfArray(new double[] { srcX, srcY, 1, 0, 0, 0, -dstX * srcX, -dstX * srcY });
                var row2 = DenseVector.OfArray(new double[] { 0, 0, 0, srcX, srcY, 1, -dstY * srcX, -dstY * srcY });

                a.SetRow(i, row1);
                a.SetRow(i + 4, row2);
            }

            var inversed = a.Inverse();

            var dstVec = DenseVector.OfArray(new double[]
            {
                dstPoints[0][0],dstPoints[1][0],dstPoints[2][0],dstPoints[3][0],
                dstPoints[0][1],dstPoints[1][1],dstPoints[2][1],dstPoints[3][1]
            });

            var parameterVec = inversed * dstVec;

            return DenseMatrix.OfArray(new double[,]
            {
                {parameterVec[0], parameterVec[1], parameterVec[2]},
                {parameterVec[3], parameterVec[4], parameterVec[5]},
                {parameterVec[6], parameterVec[7], 1}
            });
        }

        public static (double x, double y) Translate(DenseMatrix homography, double x, double y)
        {
            var vec = DenseVector.OfArray(new double[] {x, y, 1});
            var dst = homography * vec;
            return (dst[0] / dst[2], dst[1] / dst[2]);
        }
    }
}
