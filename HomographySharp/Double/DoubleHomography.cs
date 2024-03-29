using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra.Double;

namespace HomographySharp.Double;

internal static class DoubleHomography
{
    /// <summary>
    /// Set the coefficient matrix for dstX.
    /// </summary>
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
    /// Set the coefficient matrix for dstY.
    /// </summary>
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

    /// <param name="srcPoints">need 4 or more points before translate </param>
    /// <param name="dstPoints">need 4 or more points after translate</param>
    /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
    /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
    public static DoubleHomographyMatrix Find(IReadOnlyList<Point2<double>> srcPoints, IReadOnlyList<Point2<double>> dstPoints)
    {
        if (srcPoints.Count < 4 || dstPoints.Count < 4)
        {
            throw new ArgumentException("srcPoints and dstPoints must require 4 or more points");
        }
        if (srcPoints.Count != dstPoints.Count)
        {
            throw new ArgumentException("srcPoints and dstPoints must same num");
        }

        int pointNum = srcPoints.Count;

        // q(dst vector) = A(nx8 coefficient matrix) * p(homography matrix parameter)
        // p = A^-1 * q
        var coefficientMatrix = DenseMatrix.Create(pointNum * 2, 8, 0);

        for (int i = 0; i < pointNum; i++)
        {
            var src = srcPoints[i];
            var dst = dstPoints[i];

            SetCoefficientMatrixParametersForDstX(coefficientMatrix, src.X, src.Y, dst.X, 2 * i);
            SetCoefficientMatrixParametersForDstY(coefficientMatrix, src.X, src.Y, dst.Y, 2 * i + 1);
        }

        var dstVec = DenseVector.Create(pointNum * 2, 0);

        for (int i = 0; i < pointNum; i++)
        {
            dstVec[i * 2] = dstPoints[i].X;
            dstVec[i * 2 + 1] = dstPoints[i].Y;
        }

        var inverseCoefficientMatrix = pointNum == 4 ? coefficientMatrix.Inverse() : coefficientMatrix.PseudoInverse();

        var parameterVec = inverseCoefficientMatrix * dstVec;

        var elements = new double[9];

        elements[0] = parameterVec[0];
        elements[1] = parameterVec[1];
        elements[2] = parameterVec[2];

        elements[3] = parameterVec[3];
        elements[4] = parameterVec[4];
        elements[5] = parameterVec[5];

        elements[6] = parameterVec[6];
        elements[7] = parameterVec[7];
        elements[8] = 1;

        return new DoubleHomographyMatrix(elements);
    }

    /// <param name="srcPoints">need 4 or more points before translate </param>
    /// <param name="dstPoints">need 4 or more points after translate</param>
    /// <exception cref="ArgumentException">srcPoints and dstPoints must require 4 or more points</exception>
    /// <exception cref="ArgumentException">srcPoints and dstPoints must same num</exception>
    public static DoubleHomographyMatrix Find(ReadOnlySpan<Point2<double>> srcPoints, ReadOnlySpan<Point2<double>> dstPoints)
    {
        if (srcPoints.Length < 4 || dstPoints.Length < 4)
        {
            throw new ArgumentException("srcPoints and dstPoints must require 4 or more points");
        }
        if (srcPoints.Length != dstPoints.Length)
        {
            throw new ArgumentException("srcPoints and dstPoints must same num");
        }

        int pointNum = srcPoints.Length;

        // q(dst vector) = A(nx8 coefficient matrix) * p(homography matrix parameter)
        // p = A^-1 * q
        var coefficientMatrix = DenseMatrix.Create(pointNum * 2, 8, 0);

        for (int i = 0; i < pointNum; i++)
        {
            var src = srcPoints[i];
            var dst = dstPoints[i];

            SetCoefficientMatrixParametersForDstX(coefficientMatrix, src.X, src.Y, dst.X, 2 * i);
            SetCoefficientMatrixParametersForDstY(coefficientMatrix, src.X, src.Y, dst.Y, 2 * i + 1);
        }

        var dstVec = DenseVector.Create(pointNum * 2, 0);

        for (int i = 0; i < pointNum; i++)
        {
            dstVec[i * 2] = dstPoints[i].X;
            dstVec[i * 2 + 1] = dstPoints[i].Y;
        }

        var inverseCoefficientMatrix = pointNum == 4 ? coefficientMatrix.Inverse() : coefficientMatrix.PseudoInverse();

        var parameterVec = inverseCoefficientMatrix * dstVec;

        var elements = new double[9];

        elements[0] = parameterVec[0];
        elements[1] = parameterVec[1];
        elements[2] = parameterVec[2];

        elements[3] = parameterVec[3];
        elements[4] = parameterVec[4];
        elements[5] = parameterVec[5];

        elements[6] = parameterVec[6];
        elements[7] = parameterVec[7];
        elements[8] = 1;

        return new DoubleHomographyMatrix(elements);
    }
}
