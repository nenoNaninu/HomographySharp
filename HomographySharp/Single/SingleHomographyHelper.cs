using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra.Single;
#if NETSTANDARD2_1
using System.Numerics;
#endif

namespace HomographySharp.Single
{
    internal static class SingleHomographyHelper
    {
        /// <summary>
        /// dstXの方の係数行列の設定。
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="dstX"></param>
        /// <param name="rowIndex"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetCoefficientMatrixParametersForDstX(DenseMatrix matrix, float srcX, float srcY, float dstX, int rowIndex)
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
        private static void SetCoefficientMatrixParametersForDstY(DenseMatrix matrix, float srcX, float srcY, float dstY, int rowIndex)
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
        /// </summary>
        /// <param name="srcPoints">need 4 or more points before translate </param>
        /// <param name="dstPoints">need 4 or more points after translate</param>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
        /// <returns>Homography Matrix</returns>
        public static HomographySingleMatrix FindHomography(IReadOnlyList<Vector2<float>> srcPoints, IReadOnlyList<Vector2<float>> dstPoints)
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

            var elements = new float[9];

            elements[0] = parameterVec[0];
            elements[1] = parameterVec[1];
            elements[2] = parameterVec[2];

            elements[3] = parameterVec[3];
            elements[4] = parameterVec[4];
            elements[5] = parameterVec[5];

            elements[6] = parameterVec[6];
            elements[7] = parameterVec[7];
            elements[8] = 1;

            return new HomographySingleMatrix(elements);
        }

#if NETSTANDARD2_1
        /// <summary>
        /// </summary>
        /// <param name="srcPoints">need 4 or more points before translate </param>
        /// <param name="dstPoints">need 4 or more points after translate</param>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
        /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
        /// <returns>Homography Matrix</returns>
        public static HomographySingleMatrix FindHomography(IReadOnlyList<Vector2> srcPoints, IReadOnlyList<Vector2> dstPoints)
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

            var elements = new float[9];

            elements[0] = parameterVec[0];
            elements[1] = parameterVec[1];
            elements[2] = parameterVec[2];

            elements[3] = parameterVec[3];
            elements[4] = parameterVec[4];
            elements[5] = parameterVec[5];

            elements[6] = parameterVec[6];
            elements[7] = parameterVec[7];
            elements[8] = 1;

            return new HomographySingleMatrix(elements);
        }
#endif
    }
}
