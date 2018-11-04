using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.ConcurrentTest
{
    public class TimeRegion
    {
        private long mCount;

        public double Start { get; set; }

        public double End { get; set; }

        public long Count => mCount;

        public bool Add(double value)
        {
            if (value >= Start && value < End)
            {
                System.Threading.Interlocked.Increment(ref mCount);
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            string time = $"[{Start}ms-{End}ms]";

            return $"{time.PadLeft(20)}:{Count.ToString("###,###,###,###,###")}";
        }
    }
}
