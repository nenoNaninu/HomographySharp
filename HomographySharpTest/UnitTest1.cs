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
            Console.WriteLine("=====test1=====");

            Console.WriteLine(homo);

            {
                (double x, double y) = HomographyHelper.Translate(homo, 100, 0);
                Assert.IsTrue(Math.Abs(x - 500) < 0.001);
                Assert.IsTrue(Math.Abs(y - 0) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 100, 100);
                Assert.IsTrue(Math.Abs(x - 500) < 0.001);
                Assert.IsTrue(Math.Abs(y - 200) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 50, 50);
                Assert.IsTrue(Math.Abs(x - 250) < 0.001);
                Assert.IsTrue(Math.Abs(y - 100) < 0.001);
            }

            //パラメータのテスト
            //正解はOpenCV(C++)で出した射影変換行列を用いている。

            Assert.IsTrue(Math.Abs(homo.Values[0] - 5) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[1] - 0) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[2] - 0) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[3] - 0) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[4] - 2) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[5] - 0) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[6] - 0) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[7] - 0) < 0.001);
            Assert.IsTrue(Math.Abs(homo.Values[8] - 1) < 0.001);
        }

        [Test]
        public void FindHomographyTest2()
        {
            var srcList = new List<DenseVector>(4);
            var dstList = new List<DenseVector>(4);

            {
                var v0 = DenseVector.OfArray(new double[] { 123, 541 });
                var v1 = DenseVector.OfArray(new double[] { 362, 794 });
                var v2 = DenseVector.OfArray(new double[] { 362, -300 });
                var v3 = DenseVector.OfArray(new double[] { 123, -203 });
                srcList.Add(v0);
                srcList.Add(v1);
                srcList.Add(v2);
                srcList.Add(v3);
            }

            {
                var v0 = DenseVector.OfArray(new double[] { 60, 777 });
                var v1 = DenseVector.OfArray(new double[] { 1320, 444 });
                var v2 = DenseVector.OfArray(new double[] { 1423, -5041 });
                var v3 = DenseVector.OfArray(new double[] { -200, 609 });
                dstList.Add(v0);
                dstList.Add(v1);
                dstList.Add(v2);
                dstList.Add(v3);
            }

            var homo = HomographyHelper.FindHomography(srcList, dstList);


            {
                (double x, double y) = HomographyHelper.Translate(homo, 123, 541);
                Assert.IsTrue(Math.Abs(x - 60) < 0.001);
                Assert.IsTrue(Math.Abs(y - 777) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 362, 794);
                Assert.IsTrue(Math.Abs(x - 1320) < 0.001);
                Assert.IsTrue(Math.Abs(y - 444) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 362, -300);
                Assert.IsTrue(Math.Abs(x - 1423) < 0.001);
                Assert.IsTrue(Math.Abs(y - -5041) < 0.001);
            }

            {
                (double x, double y) = HomographyHelper.Translate(homo, 50, 50);
                Assert.IsTrue(Math.Abs(x - -3.16)  < 0.001);
                Assert.IsTrue(Math.Abs(y - 188.878) < 0.001);
            }


            Console.WriteLine("=====test2=====");
            Console.WriteLine(homo);
            //パラメータのテスト
            //正解はOpenCV(C++)で出した射影変換行列を用いている。

            //Assert.IsTrue(Math.Abs(homo.Values[0] - 1.1794) < 0.001);
            //Assert.IsTrue(Math.Abs(homo.Values[1] - -0.12962637) < 0.001);
            //Assert.IsTrue(Math.Abs(homo.Values[2] - 62.86479036) < 0.001);

            //Assert.IsTrue(Math.Abs(homo.Values[3] - 3.0328645) < 0.001);
            //Assert.IsTrue(Math.Abs(homo.Values[4] - -0.059784) < 0.001);
            //Assert.IsTrue(Math.Abs(homo.Values[5] - 5.932097) < 0.001);

            //Assert.IsTrue(Math.Abs(homo.Values[6] - -0.0034928) < 0.001);
            //Assert.IsTrue(Math.Abs(homo.Values[7] - -0.000138222) < 0.001);
            //Assert.IsTrue(Math.Abs(homo.Values[8] - 1) < 0.001);
        }

    }
}