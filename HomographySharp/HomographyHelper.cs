using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace HomographySharp
{
    public static class HomographyHelper
    {
        /// <summary>
        /// All vectors contained in srcPoints and dstPoints must be two dimensional(x and y).
        /// </summary>
        /// <param name="srcPoints">変換前の4点(need 4points)</param>
        /// <param name="dstPoints">変換後の4点(need 4points)</param>
        /// <returns>Homography Matrix</returns>
        public static DenseMatrix FindHomography(List<DenseVector> srcPoints, List<DenseVector> dstPoints)
        {
            if (srcPoints.Count != 4 || dstPoints.Count != 4)
            {
                throw new ArgumentException("srcPoints and dstPoints need just 4 points");
            }

            if (srcPoints.Any(x => x.Count != 2) || dstPoints.Any(x => x.Count != 2))
            {
                throw new ArgumentException("All vectors contained in srcPoints and dstPoints must be two dimensional(x and y).");
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


        /// <summary>
        /// </summary>
        /// <param name="srcPoints">need 4 points in the area before translate </param>
        /// <param name="dstPoints">need 4 points after translate</param>
        /// <returns>Homography Matrix</returns>
        public static DenseMatrix FindHomography(List<PointF> srcPoints, List<PointF> dstPoints)
        {
            if (srcPoints.Count != 4 || dstPoints.Count != 4)
            {
                throw new ArgumentException("srcPoints and dstPoints need just 4 points");
            }

            //q(dstのベクトル) = A(作成するべき8x8行列) * P(射影変換のパラメータ)
            //P = A^-1 * q
            //でパラメータが求まる。
            DenseMatrix a = DenseMatrix.Create(8, 8, 0);

            for (int i = 0; i < 4; i++)
            {
                var src = srcPoints[i];
                var dst = dstPoints[i];

                var row1 = DenseVector.OfArray(new double[] { src.X, src.Y, 1, 0, 0, 0, -dst.X * src.X, -dst.X * src.Y });
                var row2 = DenseVector.OfArray(new double[] { 0, 0, 0, src.X, src.Y, 1, -dst.Y * src.X, -dst.Y * src.Y });

                a.SetRow(i, row1);
                a.SetRow(i + 4, row2);
            }

            var inversed = a.Inverse();

            var dstVec = DenseVector.OfArray(new double[]
            {
                dstPoints[0].X,dstPoints[1].X,dstPoints[2].X,dstPoints[3].X,
                dstPoints[0].Y,dstPoints[1].Y,dstPoints[2].Y,dstPoints[3].Y
            });

            var parameterVec = inversed * dstVec;

            return DenseMatrix.OfArray(new double[,]
            {
                {parameterVec[0], parameterVec[1], parameterVec[2]},
                {parameterVec[3], parameterVec[4], parameterVec[5]},
                {parameterVec[6], parameterVec[7], 1}
            });
        }

        public static (double dstX, double dstY) Translate(DenseMatrix homography, double srcX, double srcY)
        {
            var vec = DenseVector.OfArray(new double[] { srcX, srcY, 1 });
            var dst = homography * vec;
            return (dst[0] / dst[2], dst[1] / dst[2]);
        }
    }
}
