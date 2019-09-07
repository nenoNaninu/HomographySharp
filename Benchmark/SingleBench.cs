using System.Collections.Generic;
using System.Drawing;
using BenchmarkDotNet.Attributes;
using HomographySharp.Single;

namespace BenchmarkSpace
{
    [MemoryDiagnoser]
    public class SingleBench
    {
        [Benchmark]
        public void Bench()
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

            for (int i = 0; i < 100000; i++)
            {
                var homo = HomographyHelper.FindHomography(srcList, dstList);
                (double x, double y) = HomographyHelper.Translate(homo, -152f, 394f);
            }
        }
    }
}