using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using HomographySharp;

namespace BenchmarkSpace
{
    [MemoryDiagnoser]
    public class DoubleBench
    {
        [Benchmark]
        public void Bench()
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

            for (int i = 0; i < 100000; i++)
            {
                var homo = Homography.Find(srcList, dstList);
                var result = homo.Translate(-152, 394);
            }
        }

        [Benchmark]
        public void Bench2()
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

            for (int i = 0; i < 100000; i++)
            {
                var result = homo.Translate(-152, 394);
            }
        }
    }
}