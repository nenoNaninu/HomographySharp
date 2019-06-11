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
        /// return DenseVector.OfArray(new double[] { x, y })
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>DenseVector.OfArray(new double[] { x, y })</returns>
        public static DenseVector CreateVector2(double x, double y)
        {
            return DenseVector.OfArray(new double[] { x, y });
        }

        /// <summary>
        /// dstXの方の係数行列の設定。
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="dstX"></param>
        /// <param name="rowIndex"></param>
        private static void SetCoefficientMatrixParametersForDstX(DenseMatrix matrix, double srcX, double srcY, double dstX, int rowIndex)
        {
            matrix[rowIndex, 0] = srcX;
            matrix[rowIndex, 1] = srcY;
            matrix[rowIndex, 2] = 1;

            matrix[rowIndex, 3] = 0;
            matrix[rowIndex, 4] = 0;
            matrix[rowIndex, 5] = 0;

            matrix[rowIndex, 6] = -dstX * srcX;
            matrix[rowIndex, 7] = -dstX * srcY;
        }

        /// <summary>
        /// dstYの方の係数行列の設定。
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="dstY"></param>
        /// <param name="rowIndex"></param>
        private static void SetCoefficientMatrixParametersForDstY(DenseMatrix matrix, double srcX, double srcY, double dstY, int rowIndex)
        {
            matrix[rowIndex, 0] = 0;
            matrix[rowIndex, 1] = 0;
            matrix[rowIndex, 2] = 0;

            matrix[rowIndex, 3] = srcX;
            matrix[rowIndex, 4] = srcY;
            matrix[rowIndex, 5] = 1;

            matrix[rowIndex, 6] = -dstY * srcX;
            matrix[rowIndex, 7] = -dstY * srcY;
        }

        /// <summary>
        /// All vectors contained in srcPoints and dstPoints must be two dimensional(x and y).
        /// </summary>
        /// <param name="srcPoints">need 4 or more points before translate</param>
        /// <param name="dstPoints">need 4 or more points after translate</param>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
        /// <exception cref="ArgumentException">All vectors contained in srcPoints and dstPoints must be two dimensional(x and y).</exception>
        /// <returns>Homography Matrix</returns>
        public static DenseMatrix FindHomography(List<DenseVector> srcPoints, List<DenseVector> dstPoints)
        {
            if (srcPoints.Count < 4 || dstPoints.Count < 4)
            {
                throw new ArgumentException("srcPoints and dstPoints must require 4 or more points");
            }

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

                SetCoefficientMatrixParametersForDstX(a, srcX, srcY, dstX, 2 * i);
                SetCoefficientMatrixParametersForDstY(a, srcX, srcY, dstY, 2 * i + 1);
            }

            var dstVec = DenseVector.Create(pointNum * 2, 0);

            for (int i = 0; i < pointNum; i++)
            {
                dstVec[i * 2] = dstPoints[i][0];
                dstVec[i * 2 + 1] = dstPoints[i][1];
            }

            var inverseA = pointNum == 4 ? a.Inverse() : a.PseudoInverse();

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
        /// <param name="srcPoints">need 4 or more points before translate </param>
        /// <param name="dstPoints">need 4 or more points after translate</param>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
        /// <returns>Homography Matrix</returns>
        public static DenseMatrix FindHomography(List<PointF> srcPoints, List<PointF> dstPoints)
        {
            if (srcPoints.Count < 4 || dstPoints.Count < 4)
            {
                throw new ArgumentException("srcPoints and dstPoints must require 4 or more points");
            }
            if (srcPoints.Count != dstPoints.Count)
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

                SetCoefficientMatrixParametersForDstX(a, src.X, src.Y, dst.X, 2 * i);
                SetCoefficientMatrixParametersForDstY(a, src.X, src.Y, dst.Y, 2 * i + 1);
            }

            var dstVec = DenseVector.Create(pointNum * 2, 0);

            for (int i = 0; i < pointNum; i++)
            {
                dstVec[i * 2] = dstPoints[i].X;
                dstVec[i * 2 + 1] = dstPoints[i].Y;
            }

            var inverseA = pointNum == 4 ? a.Inverse() : a.PseudoInverse();

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
            // ↓ in this case, allocation occurs
            //var vec = DenseVector.OfArray(new double[] { srcX, srcY, 1 });
            //var dst = homography * vec;
            //return (dst[0] / dst[2], dst[1] / dst[2]);

            var dst1 = homography[0, 0] * srcX + homography[0, 1] * srcY + homography[0, 2];
            var dst2 = homography[1, 0] * srcX + homography[1, 1] * srcY + homography[1, 2];
            var dst3 = homography[2, 0] * srcX + homography[2, 1] * srcY + homography[2, 2];
            return (dst1 / dst3, dst2 / dst3);
        }
    }
}
