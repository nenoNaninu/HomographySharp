using System;
using System.Collections.Generic;
using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HomographySharp.Double;

namespace BenchmarkSpace
{

    class Program
    {
        static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[]  
            {  
                typeof(DoubleBench),
                typeof(SingleBench)
            });

            switcher.Run(args);
        }
    }
}