using System;
using System.Diagnostics;

namespace DependencyScanner.Core.Tools
{
    public static class WatchExtensions
    {
        public static TimeSpan Measure(Stopwatch watch, Action actionToMeasure)
        {
            watch.Reset();
            watch.Start();

            actionToMeasure();

            watch.Stop();
            return watch.Elapsed;
        }

        public static TimeSpan Measure(Action actionToMeasure)
        {
            return Measure(new Stopwatch(), actionToMeasure);
        }
    }
}
