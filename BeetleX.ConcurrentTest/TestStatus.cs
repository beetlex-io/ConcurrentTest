using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.ConcurrentTest
{
    public struct TestStatus
    {
        public double StareTime;

        public CTester Test;

        public void Success()
        {
            Test.Statistic(this, CTester.SUCCESS_TAG, 1);
        }

        public void Statistic(string label, long value)
        {
            Test.Statistic(this, label, value);
        }

        public void Error()
        {
            Test.Statistic(this, CTester.ERROR_TAG, 1);
        }
    }
}
