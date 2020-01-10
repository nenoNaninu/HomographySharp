using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra.Double;

namespace HomographySharp.Double
{
    
    /// <summary>
    /// 
    /// </summary>
    public static class HomographyHelper
    {
        /// <summary>
        /// return DenseVector.OfArray(new double[] { x, y })
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>DenseVector.OfArray(new double[] { x, y })</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public static HomographyMatrix FindHomography(IReadOnlyList<DenseVector> srcPoints, IReadOnlyList<DenseVector> dstPoints)
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

            var answerMatrix = new DenseMatrix(3, 3);
            var rawAnswerArray = answerMatrix.Values;
            
            rawAnswerArray[0] = parameterVec[0];
            rawAnswerArray[3] = parameterVec[1];
            rawAnswerArray[6] = parameterVec[2];
            
            rawAnswerArray[1] = parameterVec[3];
            rawAnswerArray[4] = parameterVec[4];
            rawAnswerArray[7] = parameterVec[5];
            
            rawAnswerArray[2] = parameterVec[6];
            rawAnswerArray[5] = parameterVec[7];
            rawAnswerArray[8] = 1;

            return new HomographyMatrix(answerMatrix);
        }

        /// <summary>
        /// </summary>
        /// <param name="srcPoints">need 4 or more points before translate </param>
        /// <param name="dstPoints">need 4 or more points after translate</param>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
        /// <returns>Homography Matrix</returns>
        public static HomographyMatrix FindHomography(IReadOnlyList<PointF> srcPoints, IReadOnlyList<PointF> dstPoints)
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

            var answerMatrix = new DenseMatrix(3, 3);
            var rawAnswerArray = answerMatrix.Values;
            
            rawAnswerArray[0] = parameterVec[0];
            rawAnswerArray[3] = parameterVec[1];
            rawAnswerArray[6] = parameterVec[2];
            
            rawAnswerArray[1] = parameterVec[3];
            rawAnswerArray[4] = parameterVec[4];
            rawAnswerArray[7] = parameterVec[5];
            
            rawAnswerArray[2] = parameterVec[6];
            rawAnswerArray[5] = parameterVec[7];
            rawAnswerArray[8] = 1;

            return new HomographyMatrix(answerMatrix);
                
//            ↓余計なAllocが起きるので↑ベタが書きに。
//            return DenseMatrix.OfArray(new double[,]
//            {
//                {parameterVec[0], parameterVec[1], parameterVec[2]},
//                {parameterVec[3], parameterVec[4], parameterVec[5]},
//                {parameterVec[6], parameterVec[7], 1}
//            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="homography"></param>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static (double dstX, double dstY) Translate(DenseMatrix homography, double srcX, double srcY)
        {
            // ↓ in this case, allocation occurs
            //var vec = DenseVector.OfArray(new double[] { srcX, srcY, 1 });
            //var dst = homography * vec;
            //return (dst[0] / dst[2], dst[1] / dst[2]);

            if (homography.RowCount != 3 || homography.ColumnCount != 3)
            {
                throw new ArgumentException("The shape of homography matrix must be 3x3");
            }

            var rawArray = homography.Values;

            var dst1 = rawArray[0] * srcX + rawArray[3] * srcY + rawArray[6];
            var dst2 = rawArray[1] * srcX + rawArray[4] * srcY + rawArray[7];
            var dst3 = rawArray[2] * srcX + rawArray[5] * srcY + rawArray[8];

            return (dst1 / dst3, dst2 / dst3);
        }
    }
}
