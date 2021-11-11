using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using HomographySharp;

namespace BenchmarkSpace
{
    [MemoryDiagnoser]
    public class SingleBench
    {
        [Benchmark]
        public static void Bench()
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

            for (int i = 0; i < 100000; i++)
            {
                var homo = Homography.Find(srcList, dstList);
                var _ = homo.Translate(-152f, 394f);
            }
        }

        [Benchmark]
        public static void Bench2()
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

            var homo = Homography.Find(srcList, dstList);

            for (int i = 0; i < 100000; i++)
            {
                var _ = homo.Translate(-152f, 394f);
            }
        }
    }
}
