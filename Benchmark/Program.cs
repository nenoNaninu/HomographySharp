using BenchmarkDotNet.Running;

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
