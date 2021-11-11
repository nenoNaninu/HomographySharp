using System;
using System.Collections.Generic;
using HomographySharp;
using NUnit.Framework;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        FindHomographyTestForSetUp();
    }


    [Test]
    public void FindHomographyTest00()
    {
        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));

        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();

        var homo = Homography.Find(srcList, dstList);

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


        var mathNetMat = homo.ToMathNetMatrix();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Assert.IsTrue(Math.Abs(mathNetMat[i, j] - homo[i, j]) < 0.001);
            }
        }
    }

    [Test]
    public void FindHomographyTest3()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();

        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(10, 10));
        srcList.Add(new Point2<double>(100, 10));
        srcList.Add(new Point2<double>(100, 150));
        srcList.Add(new Point2<double>(10, 150));
        dstList.Add(new Point2<double>(11, 11));
        dstList.Add(new Point2<double>(500, 11));
        dstList.Add(new Point2<double>(500, 200));
        dstList.Add(new Point2<double>(11, 200));

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
            var result = homo.Translate((100 + 10) / 2.0, (150 + 10) / 2.0);
            double dstx = (500.0 + 11) / 2.0;
            double dsty = (200 + 11) / 2.0;
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

        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));
        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

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

    public static void FindHomographyTestForSetUp()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();

        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));
        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

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

        //Console.WriteLine(homo);
    }

    [Test]
    public void Copy()
    {
        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));
        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

        var homo = Homography.Find(srcList, dstList);

        var homo2 = Homography.Create(homo.ElementsAsSpan().ToArray());

        for (int i = 0; i < homo.Elements.Count; i++)
        {
            Assert.IsTrue(Math.Abs(homo.Elements[i] - homo2.Elements[i]) < 0.001);
        }
    }

    [Test]
    public void Copy2()
    {
        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));
        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

        var homo = Homography.Find(srcList, dstList);

        var homo2 = Homography.Create(homo.ElementsAsSpan());

        for (int i = 0; i < homo.Elements.Count; i++)
        {
            Assert.IsTrue(Math.Abs(homo.Elements[i] - homo2.Elements[i]) < 0.001);
        }
    }

    [Test]
    public void Copy3()
    {
        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));
        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

        var homo = Homography.Find(srcList, dstList);

        var homo2 = Homography.Create(homo.Elements);

        for (int i = 0; i < homo.Elements.Count; i++)
        {
            Assert.IsTrue(Math.Abs(homo.Elements[i] - homo2.Elements[i]) < 0.001);
        }
    }

    //[Test]
    //public void ToStringTest()
    //{
    //    var srcList = new List<Point2<double>>(4);
    //    var dstList = new List<Point2<double>>(4);

    //    srcList.Add(new Point2<double>(-152, 394));
    //    srcList.Add(new Point2<double>(218, 521));
    //    srcList.Add(new Point2<double>(223, -331));
    //    srcList.Add(new Point2<double>(-163, -219));
    //    dstList.Add(new Point2<double>(-666, 431));
    //    dstList.Add(new Point2<double>(500, 300));
    //    dstList.Add(new Point2<double>(480, -308));
    //    dstList.Add(new Point2<double>(-580, -280));

    //    var homo = Homography.Find(srcList, dstList);

    //    var str = homo.ToString();
    //    Console.WriteLine(str);

    //    var str2 = homo.ToMathNetMatrix().ToString();
    //    Console.WriteLine(str2);
    //}
}
