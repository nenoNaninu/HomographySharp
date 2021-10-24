using System;
using System.Collections.Generic;
using System.Text.Json;
using HomographySharp;
using NUnit.Framework;

namespace Tests
{
    public class UnitTest2
    {
        [SetUp]
        public void Setup()
        {
            FindHomographyTestForSetUp();
        }

        [Test]
        public void FindHomographyTest00()
        {
            var srcList = new List<Point2<float>>(4);
            var dstList = new List<Point2<float>>(4);

            srcList.Add(new Point2<float>(-152, 394));
            srcList.Add(new Point2<float>(218, 521));
            srcList.Add(new Point2<float>(223, -331));
            srcList.Add(new Point2<float>(-163, -219));

            dstList.Add(new Point2<float>(-666, 431));
            dstList.Add(new Point2<float>(500, 300));
            dstList.Add(new Point2<float>(480, -308));
            dstList.Add(new Point2<float>(-580, -280));

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var homo = Homography.Find(srcList, dstList);

            var json = JsonSerializer.Serialize(homo);
            try
            {
                var homo2 = JsonSerializer.Deserialize<HomographyMatrix<float>>(json);

                Assert.IsTrue(homo.Elements.Count == homo2?.Elements.Count);

                for (int i = 0; i < homo.Elements.Count; i++)
                {
                    Assert.IsTrue(Math.Abs(homo.Elements[i] - homo2.Elements[i]) < 0.001);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

            {
                var result = homo.Translate(-152, 394);
                Assert.IsTrue(Math.Abs(result.X - -666) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 431) < 0.001);
            }

            {
                var result = homo.Translate(218, 521);
                Assert.IsTrue(Math.Abs(result.X - 500) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 300) < 0.001);
            }

            {
                var result = homo.Translate(223, -331);
                Assert.IsTrue(Math.Abs(result.X - 480) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - -308) < 0.001);
            }

            Console.WriteLine(homo);
        }

        [Test]
        public void FindHomographyTest01()
        {
            var srcList = new List<Point2<float>>(4);
            var dstList = new List<Point2<float>>(4);

            srcList.Add(new Point2<float>(-152, 394));
            srcList.Add(new(218, 521));
            srcList.Add(new(223, -331));
            srcList.Add(new(-163, -219));

            dstList.Add(new Point2<float>(-666, 431));
            dstList.Add(new Point2<float>(500, 300));
            dstList.Add(new Point2<float>(480, -308));
            dstList.Add(new Point2<float>(-580, -280));

            var srcArray = srcList.ToArray();
            var dstArray = dstList.ToArray();

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var homo = Homography.Find(srcArray, dstArray);
            Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

            {
                var result = homo.Translate(-152, 394);
                Assert.IsTrue(Math.Abs(result.X - -666) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 431) < 0.001);
            }

            {
                var result = homo.Translate(218, 521);
                Assert.IsTrue(Math.Abs(result.X - 500) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 300) < 0.001);
            }

            {
                var result = homo.Translate(223, -331);
                Assert.IsTrue(Math.Abs(result.X - 480) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - -308) < 0.001);
            }

            Console.WriteLine(homo);
        }


        [Test]
        public void FindHomographyTest3()
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var srcList = new Point2<float>[4];
            var dstList = new Point2<float>[4];

            srcList[0] = new Point2<float>(10, 10);
            srcList[1] = new(100, 10);
            srcList[2] = new(100, 150);
            srcList[3] = new(10, 150);

            dstList[0] = new Point2<float>(11, 11);
            dstList[1] = new Point2<float>(500, 11);
            dstList[2] = new Point2<float>(500, 200);
            dstList[3] = new Point2<float>(11, 200);

            var homo = Homography.Find(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine($"=====test3 stop{stopWatch.ElapsedMilliseconds}=====");

            Console.WriteLine(homo);

            {
                var result = homo.Translate(100, 10);
                Assert.IsTrue(Math.Abs(result.X - 500) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 11) < 0.001);
            }

            {
                var result = homo.Translate(100, 150);
                Assert.IsTrue(Math.Abs(result.X - 500) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 200) < 0.001);
            }


            {
                var result = homo.Translate((100 + 10) / 2.0f, (150 + 10) / 2.0f);
                float dstx = (500 + 11) / 2.0f;
                float dsty = (200 + 11) / 2.0f;
                Console.WriteLine("result.X" + result.X);
                Console.WriteLine("result.Y" + result.Y);

                Console.WriteLine("dstx" + dstx);
                Console.WriteLine("dsty" + dsty);

                Assert.IsTrue(Math.Abs(result.X - dstx) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - dsty) < 0.001);
            }
        }

        [Test]
        public void FindHomographyTest4()
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var srcList = new List<Point2<float>>(4);
            var dstList = new List<Point2<float>>(4);

            srcList.Add(new Point2<float>(-152, 394));
            srcList.Add(new Point2<float>(218, 521));
            srcList.Add(new Point2<float>(223, -331));
            srcList.Add(new Point2<float>(-163, -219));

            dstList.Add(new Point2<float>(-666, 431));
            dstList.Add(new Point2<float>(500, 300));
            dstList.Add(new Point2<float>(480, -308));
            dstList.Add(new Point2<float>(-580, -280));

            var homo = Homography.Find(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

            {
                var result = homo.Translate(-152, 394);
                Assert.IsTrue(Math.Abs(result.X - -666) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 431) < 0.001);
            }

            {
                var result = homo.Translate(218, 521);
                Assert.IsTrue(Math.Abs(result.X - 500) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 300) < 0.001);
            }

            {
                var result = homo.Translate(223, -331);
                Assert.IsTrue(Math.Abs(result.X - 480) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - -308) < 0.001);
            }

            Console.WriteLine(homo);
        }

        public void FindHomographyTestForSetUp()
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var srcList = new List<Point2<float>>(4);
            var dstList = new List<Point2<float>>(4);

            srcList.Add(new Point2<float>(-152, 394));
            srcList.Add(new Point2<float>(218, 521));
            srcList.Add(new Point2<float>(223, -331));
            srcList.Add(new Point2<float>(-163, -219));

            dstList.Add(new Point2<float>(-666, 431));
            dstList.Add(new Point2<float>(500, 300));
            dstList.Add(new Point2<float>(480, -308));
            dstList.Add(new Point2<float>(-580, -280));

            var homo = Homography.Find(srcList, dstList);
            stopWatch.Stop();
            Console.WriteLine("setup!!!!!!!");
            //Console.WriteLine($"=====test4 stop{stopWatch.ElapsedMilliseconds}=====");

            {
                var result = homo.Translate(-152, 394);
                Assert.IsTrue(Math.Abs(result.X - -666) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 431) < 0.001);
            }

            {
                var result = homo.Translate(218, 521);
                Assert.IsTrue(Math.Abs(result.X - 500) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - 300) < 0.001);
            }

            {
                var result = homo.Translate(223, -331);
                Assert.IsTrue(Math.Abs(result.X - 480) < 0.001);
                Assert.IsTrue(Math.Abs(result.Y - -308) < 0.001);
            }
        }
    }
}