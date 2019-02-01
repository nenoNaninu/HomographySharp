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
        /// <param name="srcPoints">変換前の最低4点(need least 4points)</param>
        /// <param name="dstPoints">変換後の最低4点(need least 4points)</param>
        /// <returns>Homography Matrix</returns>
        public static DenseMatrix FindHomography(List<DenseVector> srcPoints, List<DenseVector> dstPoints)
        {
            if (srcPoints.Count != dstPoints.Count)
            {
                throw new ArgumentException("srcPoints and dstPoints must same num");
            }

            if (srcPoints.Any(x => x.Count != 2) || dstPoints.Any(x => x.Count != 2))
            {
                throw new ArgumentException("All vectors contained in srcPoints and dstPoints must be two dimensional(x and y).");
            }

            //q(dstのベクトル) = A(作成するべきnx8行列) * P(射影変換のパラメータ)
            //P = A^-1 * q
            //でパラメータが求まる。
            int pointNum = srcPoints.Count;
            DenseMatrix a = DenseMatrix.Create(pointNum * 2, 8, 0);

            for (int i = 0; i < pointNum; i++)
            {
                var src = srcPoints[i];
                var dst = dstPoints[i];

                var srcX = src[0];
                var dstX = dst[0];

                var srcY = src[1];
                var dstY = dst[1];

                var row1 = DenseVector.OfArray(new double[] { srcX, srcY, 1, 0, 0, 0, -dstX * srcX, -dstX * srcY });
                var row2 = DenseVector.OfArray(new double[] { 0, 0, 0, srcX, srcY, 1, -dstY * srcX, -dstY * srcY });

                a.SetRow(2 * i, row1);
                a.SetRow(2 * i + 1, row2);
            }

            var inverseA = a.PseudoInverse();

            var dstVec = DenseVector.Create(pointNum * 2, 0);

            for (int i = 0; i < pointNum; i++)
            {
                dstVec[i * 2] = dstPoints[i][0];
                dstVec[i * 2 + 1] = dstPoints[i][1];
            }

            var parameterVec = inverseA * dstVec;

            return DenseMatrix.OfArray(new double[,]
            {
                {parameterVec[0], parameterVec[1], parameterVec[2]},
                {parameterVec[3], parameterVec[4], parameterVec[5]},
                {parameterVec[6], parameterVec[7], 1}
            });
        }


        /// <summary>
        /// </summary>
        /// <param name="srcPoints">need least 4 points before translate </param>
        /// <param name="dstPoints">need least 4 points after translate</param>
        /// <returns>Homography Matrix</returns>
        public static DenseMatrix FindHomography(List<PointF> srcPoints, List<PointF> dstPoints)
        {
            if (srcPoints.Count != 4)
            {
                throw new ArgumentException("srcPoints and dstPoints must same num");
            }

            //q(dstのベクトル) = A(作成するべきnx8行列) * P(射影変換のパラメータ)
            //P = A^-1 * q
            //でパラメータが求まる。
            int pointNum = srcPoints.Count;
            DenseMatrix a = DenseMatrix.Create(pointNum * 2, 8, 0);

            for (int i = 0; i < pointNum; i++)
            {
                var src = srcPoints[i];
                var dst = dstPoints[i];

                var row1 = DenseVector.OfArray(new double[] { src.X, src.Y, 1, 0, 0, 0, -dst.X * src.X, -dst.X * src.Y });
                var row2 = DenseVector.OfArray(new double[] { 0, 0, 0, src.X, src.Y, 1, -dst.Y * src.X, -dst.Y * src.Y });

                a.SetRow(i * 2, row1);
                a.SetRow(i * 2 + 1, row2);
            }

            var inverseA = a.PseudoInverse();

            var dstVec = DenseVector.Create(pointNum * 2, 0);

            for (int i = 0; i < pointNum; i++)
            {
                dstVec[i * 2] = dstPoints[i].X;
                dstVec[i * 2 + 1] = dstPoints[i].Y;
            }

            var parameterVec = inverseA * dstVec;

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
