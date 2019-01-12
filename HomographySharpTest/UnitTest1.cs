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
        public void FindHomographyTest1()
        {
            var srcList = new List<DenseVector>(4);
            var dstList = new List<DenseVector>(4);

            srcList.Add(DenseVector.OfArray(new double[] { 10, 10 }));
            srcList.Add(DenseVector.OfArray(new double[] { 100, 10 }));
            srcList.Add(DenseVector.OfArray(new double[] { 100, 150 }));
            srcList.Add(DenseVector.OfArray(new double[] { 10, 150 }));

            dstList.Add(DenseVector.OfArray(new double[] { 11, 11 }));
            dstList.Add(DenseVector.OfArray(new double[] { 500, 11 }));
            dstList.Add(DenseVector.OfArray(new double[] { 500, 200 }));
            dstList.Add(DenseVector.OfArray(new double[] { 11, 200 }));

            var homo = HomographyHelper.FindHomography(srcList, dstList);
            Console.WriteLine("=====test1=====");

            Console.WriteLine(homo);

            {
                (double x, double y) = HomographyHelper.Translate(homo, 100, 10);
                Assert.IsTrue(Math.Abs(x - 500) < 0.001);
                Assert.IsTrue(Math.Abs(y - 11) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 100, 150);
                Assert.IsTrue(Math.Abs(x - 500) < 0.001);
                Assert.IsTrue(Math.Abs(y - 200) < 0.001);
            }


            {
                (double x, double y) = HomographyHelper.Translate(homo, (100+10) / 2.0, (150+10) / 2.0);
                double dstx = (500.0+11) / 2.0;
                double dsty = (200+11) / 2.0;
                Console.WriteLine("x" + x);
                Console.WriteLine("y" + y);

                Console.WriteLine("dstx" + dstx);
                Console.WriteLine("dsty" + dsty);

                Assert.IsTrue(Math.Abs(x - dstx) < 0.001);
                Assert.IsTrue(Math.Abs(y - dsty) < 0.001);
            }
        }

        [Test]
        public void FindHomographyTest2()
        {
            var srcList = new List<DenseVector>(4);
            var dstList = new List<DenseVector>(4);

            {
                var v0 = DenseVector.OfArray(new double[] { -152, 394 });
                var v1 = DenseVector.OfArray(new double[] { 218, 521 });
                var v2 = DenseVector.OfArray(new double[] { 223, -331 });
                var v3 = DenseVector.OfArray(new double[] { -163, -219 });
                srcList.Add(v0);
                srcList.Add(v1);
                srcList.Add(v2);
                srcList.Add(v3);
            }

            {
                var v0 = DenseVector.OfArray(new double[] { -666, 431 });
                var v1 = DenseVector.OfArray(new double[] { 500, 300 });
                var v2 = DenseVector.OfArray(new double[] { 480, -308 });
                var v3 = DenseVector.OfArray(new double[] { -580, -280 });
                dstList.Add(v0);
                dstList.Add(v1);
                dstList.Add(v2);
                dstList.Add(v3);
            }

            var homo = HomographyHelper.FindHomography(srcList, dstList);

            {
                (double x, double y) = HomographyHelper.Translate(homo, -152, 394);
                Assert.IsTrue(Math.Abs(x - -666) < 0.001);
                Assert.IsTrue(Math.Abs(y - 431) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 218, 521);
                Assert.IsTrue(Math.Abs(x - 500) < 0.001);
                Assert.IsTrue(Math.Abs(y - 300) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 223, -331);
                Assert.IsTrue(Math.Abs(x - 480) < 0.001);
                Assert.IsTrue(Math.Abs(y - -308) < 0.001);
            }

            Console.WriteLine("=====test2=====");
            Console.WriteLine(homo);
        }

    }
}