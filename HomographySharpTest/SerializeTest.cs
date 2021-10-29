using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using HomographySharp;
using NUnit.Framework;

namespace Tests
{
    public class SerializeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Double()
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

            var json = JsonSerializer.Serialize(homo);
            try
            {
                var homo2 = JsonSerializer.Deserialize<HomographyMatrix<double>>(json);

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
        }

        [Test]
        public void Single()
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
                Assert.IsTrue(false);
                throw;
            }
        }

        [Test]
        public void ChunkTest()
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

            var chunk = new Chunk() { Id = Guid.NewGuid(), Homography = homo };

            string json = JsonSerializer.Serialize(chunk);

            Debug.WriteLine(json);

            var restore = JsonSerializer.Deserialize<Chunk>(json);

            var homo2 = restore?.Homography;

            if (homo2 is null)
            {
                Assert.IsNotNull(homo2);
            }

            Assert.IsTrue(homo.Elements.Count == homo2?.Elements.Count);

            for (int i = 0; i < homo.Elements.Count; i++)
            {
                Assert.IsTrue(Math.Abs(homo.Elements[i] - homo2.Elements[i]) < 0.001);
            }
        }


        [Test]
        public void ChunkTestCamelCase()
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

            var chunk = new Chunk() { Id = Guid.NewGuid(), Homography = homo };

            string json = JsonSerializer.Serialize(chunk, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Debug.WriteLine(json);

            var restore = JsonSerializer.Deserialize<Chunk>(json, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var homo2 = restore?.Homography;

            if (homo2 is null)
            {
                Assert.IsNotNull(homo2);
            }

            Assert.IsTrue(homo.Elements.Count == homo2?.Elements.Count);

            for (int i = 0; i < homo.Elements.Count; i++)
            {
                Assert.IsTrue(Math.Abs(homo.Elements[i] - homo2.Elements[i]) < 0.001);
            }
        }
    }

    public class Chunk
    {
        public HomographyMatrix<float> Homography { get; set; }

        public Guid Id { get; set; }
    }
}
