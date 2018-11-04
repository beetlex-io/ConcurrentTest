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
            mDetail = new long[1024 * 1024];
        }

        private long[] mDetail;

        private long mCount;

        private long mLastValue;

        private long mValue;

        public int ID { get; set; }

        public string Label { get; set; }

        public long Value => mValue;

        public void Add(long value)
        {
            System.Threading.Interlocked.Add(ref mValue, value);
        }

        public string Report(bool last = false)
        {
            long concurrent = mValue - mLastValue;
            mLastValue = mValue;
            string value = $"{Label.PadLeft(13)}:{(concurrent).ToString().PadLeft(6)}/s total:{mValue.ToString().PadLeft(12)}|";
            if (last)
            {
                if (mCount > 5)
                {
                    Array.Sort(mDetail, 0, (int)mCount);
                    long min;
                    if (Label == CTester.ERROR_TAG)
                        min = mDetail[0];
                    else
                        min = mDetail[1];
                    long max = mDetail[mCount - 1];
                    value += $"[min:{min}/s  max:{max}/s]";
                }
            }
            else
            {
                mDetail[mCount] = concurrent;
                mCount++;
            }
            return value;
        }

    }
}
