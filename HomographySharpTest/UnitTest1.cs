using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using HomographySharp;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FindHomographyTest()
        {
            var srcList = new List<Vector>();
            var dstList = new List<Vector>();

            {
                var v0 = DenseVector.OfArray(new double[] {0, 0});
                var v1 = DenseVector.OfArray(new double[] {100, 0});
                var v2 = DenseVector.OfArray(new double[] { 100, 100 });
                var v3 = DenseVector.OfArray(new double[] { 0, 100 });
                srcList.Add(v0);
                srcList.Add(v1);
                srcList.Add(v2);
                srcList.Add(v3);
            }

            {
                var v0 = DenseVector.OfArray(new double[] { 0, 0 });
                var v1 = DenseVector.OfArray(new double[] { 500, 0 });
                var v2 = DenseVector.OfArray(new double[] { 500, 200 });
                var v3 = DenseVector.OfArray(new double[] { 0, 200 });
                dstList.Add(v0);
                dstList.Add(v1);
                dstList.Add(v2);
                dstList.Add(v3);
            }

            var homo = HomographyHelper.FindHomography(srcList, dstList);
            Console.WriteLine(homo);
            Assert.IsTrue(true);
        }
    }
}