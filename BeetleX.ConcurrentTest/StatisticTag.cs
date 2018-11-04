using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.ConcurrentTest
{
    public class StatisticTag
    {

        public StatisticTag(string label)
        {
            ID = label.GetHashCode();
            Label = label;

        }

        private long mLastValue;

        private long mValue;

        public int ID { get; set; }

        public string Label { get; set; }

        public long Value => mValue;

        public void Add(long value)
        {
            System.Threading.Interlocked.Add(ref mValue, value);
        }

        public override string ToString()
        {
            string value = $"{Label.PadLeft(15)}:{(mValue - mLastValue).ToString("0,000,000")}/s total:{mValue.ToString("000,000,000,000")}";
            mLastValue = mValue;
            return value;
        }
    }
}
