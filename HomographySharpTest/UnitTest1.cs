using System;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using HomographySharp.Double;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            FindHomographyTestForSetUp();
        }

        //00と01は実行速度確認のために雑に置いている。
        //遅いのはJITだから初回コンパイルで実行時間かかるからかなー。

        [Test]
        public void FindHomographyTest00()
        {

            var srcList = new List<PointF>(4);
            var dstList = new List<PointF>(4);

            srcList.Add(new PointF { X = -152, Y = 394 });
            srcList.Add(new PointF { X = 218, Y = 521 });
            srcList.Add(new PointF { X = 223, Y = -331 });
            srcList.Add(new PointF { X = -163, Y = -219 });

            dstList.Add(new PointF { X = -666, Y = 431 });
            dstList.Add(new PointF { X = 500, Y = 300 });
            dstList.Add(new PointF { X = 480, Y = -308 });
            dstList.Add(new PointF { X = -580, Y = -280 });

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var homo = HomographyHelper.FindHomography(srcList, dstList);
            Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

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

            Console.WriteLine(homo);
        }

        [Test]
        public void FindHomographyTest01()
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

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            var homo = HomographyHelper.FindHomography(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine($"=====test1 stop{stopWatch.ElapsedMilliseconds}=====");

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
                (double x, double y) = HomographyHelper.Translate(homo, (100 + 10) / 2.0, (150 + 10) / 2.0);
                double dstx = (500.0 + 11) / 2.0;
                double dsty = (200 + 11) / 2.0;
                Console.WriteLine("x" + x);
                Console.WriteLine("y" + y);

                Console.WriteLine("dstx" + dstx);
                Console.WriteLine("dsty" + dsty);

                Assert.IsTrue(Math.Abs(x - dstx) < 0.001);
                Assert.IsTrue(Math.Abs(y - dsty) < 0.001);
            }
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

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            var homo = HomographyHelper.FindHomography(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine($"=====test1 stop{stopWatch.ElapsedMilliseconds}=====");

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
                (double x, double y) = HomographyHelper.Translate(homo, (100 + 10) / 2.0, (150 + 10) / 2.0);
                double dstx = (500.0 + 11) / 2.0;
                double dsty = (200 + 11) / 2.0;
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
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

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
                var v0 = HomographyHelper.CreateVector2(-666, 431);
                var v1 = HomographyHelper.CreateVector2(500, 300);
                var v2 = HomographyHelper.CreateVector2(480, -308);
                var v3 = HomographyHelper.CreateVector2(-580, -280);
                //var v0 = DenseVector.OfArray(new double[] {-666, 431});
                //var v1 = DenseVector.OfArray(new double[] { 500, 300 });
                //var v2 = DenseVector.OfArray(new double[] { 480, -308 });
                //var v3 = DenseVector.OfArray(new double[] { -580, -280 });
                dstList.Add(v0);
                dstList.Add(v1);
                dstList.Add(v2);
                dstList.Add(v3);
            }

            var homo = HomographyHelper.FindHomography(srcList, dstList);

            stopWatch.Stop();
            Console.WriteLine($"=====test2 stop{stopWatch.ElapsedMilliseconds}=====");

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

            Console.WriteLine(homo);
        }

        [Test]
        public void FindHomographyTest3()
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var srcList = new List<PointF>(4);
            var dstList = new List<PointF>(4);

            srcList.Add(new PointF { X = 10, Y = 10 });
            srcList.Add(new PointF { X = 100, Y = 10 });
            srcList.Add(new PointF { X = 100, Y = 150 });
            srcList.Add(new PointF { X = 10, Y = 150 });

            dstList.Add(new PointF { X = 11, Y = 11 });
            dstList.Add(new PointF { X = 500, Y = 11 });
            dstList.Add(new PointF { X = 500, Y = 200 });
            dstList.Add(new PointF { X = 11, Y = 200 });

            var homo = HomographyHelper.FindHomography(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine($"=====test3 stop{stopWatch.ElapsedMilliseconds}=====");

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
                (double x, double y) = HomographyHelper.Translate(homo, (100 + 10) / 2.0, (150 + 10) / 2.0);
                double dstx = (500.0 + 11) / 2.0;
                double dsty = (200 + 11) / 2.0;
                Console.WriteLine("x" + x);
                Console.WriteLine("y" + y);

                Console.WriteLine("dstx" + dstx);
                Console.WriteLine("dsty" + dsty);

                Assert.IsTrue(Math.Abs(x - dstx) < 0.001);
                Assert.IsTrue(Math.Abs(y - dsty) < 0.001);
            }
        }

        [Test]
        public void FindHomographyTest4()
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var srcList = new List<PointF>(4);
            var dstList = new List<PointF>(4);

            srcList.Add(new PointF { X = -152, Y = 394 });
            srcList.Add(new PointF { X = 218, Y = 521 });
            srcList.Add(new PointF { X = 223, Y = -331 });
            srcList.Add(new PointF { X = -163, Y = -219 });

            dstList.Add(new PointF { X = -666, Y = 431 });
            dstList.Add(new PointF { X = 500, Y = 300 });
            dstList.Add(new PointF { X = 480, Y = -308 });
            dstList.Add(new PointF { X = -580, Y = -280 });

            var homo = HomographyHelper.FindHomography(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

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

            Console.WriteLine(homo);
        }

        [Test]
        public void FindHomographySpeed()
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

            var stopWatch = new System.Diagnostics.Stopwatch();
            DenseMatrix homo = null;
            stopWatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                homo = HomographyHelper.FindHomography(srcList, dstList);
            }
            stopWatch.Stop();
            Console.WriteLine($"=====test1 stop{stopWatch.ElapsedMilliseconds}=====");

            Console.WriteLine(homo);
            stopWatch.Restart();

            for (int i = 0; i < 100000; i++)
            {
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
                    (double x, double y) = HomographyHelper.Translate(homo, (100 + 10) / 2.0, (150 + 10) / 2.0);
                    double dstx = (500.0 + 11) / 2.0;
                    double dsty = (200 + 11) / 2.0;
                    //Console.WriteLine("x" + x);
                    //Console.WriteLine("y" + y);

                    //Console.WriteLine("dstx" + dstx);
                    //Console.WriteLine("dsty" + dsty);

                    Assert.IsTrue(Math.Abs(x - dstx) < 0.001);
                    Assert.IsTrue(Math.Abs(y - dsty) < 0.001);
                }
            }

            stopWatch.Stop();
            Console.WriteLine($"=====translate test{stopWatch.ElapsedMilliseconds}=====");
        }

        public void FindHomographyTestForSetUp()
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var srcList = new List<PointF>(4);
            var dstList = new List<PointF>(4);

            srcList.Add(new PointF { X = -152, Y = 394 });
            srcList.Add(new PointF { X = 218, Y = 521 });
            srcList.Add(new PointF { X = 223, Y = -331 });
            srcList.Add(new PointF { X = -163, Y = -219 });

            dstList.Add(new PointF { X = -666, Y = 431 });
            dstList.Add(new PointF { X = 500, Y = 300 });
            dstList.Add(new PointF { X = 480, Y = -308 });
            dstList.Add(new PointF { X = -580, Y = -280 });

            var homo = HomographyHelper.FindHomography(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine("setup!!!!!!!");
            //Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

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

            //Console.WriteLine(homo);
        }
    }
}