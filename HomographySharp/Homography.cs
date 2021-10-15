using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using HomographySharp.Double;
using HomographySharp.Single;

namespace HomographySharp
{
    public static class Homography
    {
        public static HomographyMatrix<float> Find(IReadOnlyList<Point2<float>> srcPoints, IReadOnlyList<Point2<float>> dstPoints)
            => SingleHomography.Find(srcPoints, dstPoints);

        public static HomographyMatrix<float> Find(ReadOnlySpan<Point2<float>> srcPoints, ReadOnlySpan<Point2<float>> dstPoints)
            => SingleHomography.Find(srcPoints, dstPoints);

        public static HomographyMatrix<float> Find(Point2<float>[] srcPoints, Point2<float>[] dstPoints)
            => SingleHomography.Find(new ReadOnlySpan<Point2<float>>(srcPoints), new ReadOnlySpan<Point2<float>>(dstPoints));

        public static HomographyMatrix<float> Find(IReadOnlyList<Vector2> srcPoints, IReadOnlyList<Vector2> dstPoints)
            => SingleHomography.Find(srcPoints, dstPoints);

        public static HomographyMatrix<float> Find(ReadOnlySpan<Vector2> srcPoints, ReadOnlySpan<Vector2> dstPoints)
            => SingleHomography.Find(srcPoints, dstPoints);

        public static HomographyMatrix<float> Find(Vector2[] srcPoints, Vector2[] dstPoints)
            => SingleHomography.Find(new ReadOnlySpan<Vector2>(srcPoints), new ReadOnlySpan<Vector2>(dstPoints));

        public static HomographyMatrix<float> Create(ReadOnlySpan<float> elements)
        {
            if (elements.Length != 9)
            {
                throw new ArgumentException("elements.Length must be 9.");
            }

            return new SingleHomographyMatrix(elements.ToArray());
        }

        public static HomographyMatrix<float> Create(IReadOnlyList<float> elements)
        {
            if (elements.Count != 9)
            {
                throw new ArgumentException("elements.Count must be 9.");
            }

            return new SingleHomographyMatrix(elements.ToArray());
        }

        public static HomographyMatrix<float> Create(float[] elements)
        {
            if (elements.Length != 9)
            {
                throw new ArgumentException("elements.Length must be 9.");

            }

            var array = new float[9];
            Array.Copy(elements, array, 9);

            return new SingleHomographyMatrix(array);
        }

        public static HomographyMatrix<double> Find(IReadOnlyList<Point2<double>> srcPoints, IReadOnlyList<Point2<double>> dstPoints)
            => DoubleHomography.Find(srcPoints, dstPoints);

        public static HomographyMatrix<double> Find(ReadOnlySpan<Point2<double>> srcPoints, ReadOnlySpan<Point2<double>> dstPoints)
            => DoubleHomography.Find(srcPoints, dstPoints);

        public static HomographyMatrix<double> Find(Point2<double>[] srcPoints, Point2<double>[] dstPoints)
            => DoubleHomography.Find(new ReadOnlySpan<Point2<double>>(srcPoints), new ReadOnlySpan<Point2<double>>(dstPoints));

        public static HomographyMatrix<double> Create(ReadOnlySpan<double> elements)
        {
            if (elements.Length != 9)
            {
                throw new ArgumentException("elements.Length must be 9.");
            }

            return new DoubleHomographyMatrix(elements.ToArray());
        }

        public static HomographyMatrix<double> Create(IReadOnlyList<double> elements)
        {
            if (elements.Count != 9)
            {
                throw new ArgumentException("elements.Count must be 9.");
            }

            return new DoubleHomographyMatrix(elements.ToArray());
        }

        public static HomographyMatrix<double> Create(double[] elements)
        {
            if (elements.Length != 9)
            {
                throw new ArgumentException("elements.Length must be 9.");
            }

            var array = new double[9];
            Array.Copy(elements, array, 9);

            return new DoubleHomographyMatrix(array);
        }
    }
}