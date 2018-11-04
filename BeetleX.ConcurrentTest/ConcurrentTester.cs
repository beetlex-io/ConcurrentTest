using System;
using System.Collections.Generic;
namespace BeetleX.ConcurrentTest
{
    public class CTester
    {
        public const string SUCCESS_TAG = "Success";

        public const string ERROR_TAG = "Error";

        public CTester()
        {
            mStopwatch = new System.Diagnostics.Stopwatch();
            mStopwatch.Restart();
            AddRegion(0, 0.1);
            AddRegion(0.1, 0.5);
            AddRegion(0.5, 1);
            AddRegion(1, 5);
            AddRegion(5, 10);
            AddRegion(10, 50);
            AddRegion(50, 100);
            AddRegion(100, 1000);
            AddRegion(1000, 100000);
            statisticTags.Add(new StatisticTag("Success"));
            statisticTags.Add(new StatisticTag("Error"));
        }

        private System.Diagnostics.Stopwatch mStopwatch;

        private List<StatisticTag> statisticTags = new List<StatisticTag>();

        private List<TimeRegion> mRegions = new List<TimeRegion>();

        public IList<TimeRegion> TimeRegions => mRegions;

        public IList<StatisticTag> Tags => statisticTags;

        public void AddRegion(double start, double end)
        {
            mRegions.Add(new TimeRegion { Start = start, End = end });
        }

        public TestStatus CreateStatus()
        {
            TestStatus testStatus = new TestStatus();
            testStatus.Test = this;
            testStatus.StareTime = mStopwatch.Elapsed.TotalMilliseconds;
            return testStatus;
        }

        public void Report()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o =>
            {
                Console.Clear();
                Console.SetOut(new System.IO.StreamWriter(@"c:\test.txt"));
                while (true)
                {

                    Console.CursorTop = 0;
                    Console.CursorLeft = 0;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("*******************************************************************");
                    sb.AppendLine($"* https://github.com/IKende/ConcurrentTest.git");
                    sb.AppendLine($"* Copyright © ikende.com 2018 email:henryfan@msn.com");
                    sb.AppendLine($"****************ConcurrentTest[{DateTime.Now}]*****************");
                    for (int i = 0; i < statisticTags.Count; i++)
                    {
                        sb.AppendLine($"* {statisticTags[i]}");
                    }
                    sb.AppendLine("-------------------------------------------------------------------");
                    for (int i = 0; i < mRegions.Count; i++)
                    {
                        sb.AppendLine($"* {mRegions[i]}");
                    }
                    sb.AppendLine("*******************************************************************");
                    Console.Out.Flush();
                    Console.WriteLine(sb.ToString());
                    System.Threading.Thread.Sleep(1000);
                }
            });
        }


        public void Statistic(TestStatus testStatus, string tag, long value)
        {
            if (tag == SUCCESS_TAG)
            {
                double time = mStopwatch.Elapsed.TotalMilliseconds - testStatus.StareTime;
                for (int i = 0; i < mRegions.Count; i++)
                {
                    if (mRegions[i].Add(time))
                        break;
                }
            }
            int id = tag.GetHashCode();
            for (int i = 0; i < statisticTags.Count; i++)
            {
                if (statisticTags[i].ID == id)
                {
                    statisticTags[i].Add(value);
                    break;
                }
            }
        }

        public void Run(int thread, Action action)
        {
            for (int i = 0; i < thread; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        TestStatus testStatus = CreateStatus();
                        try
                        {
                            action();
                            testStatus.Success();
                        }
                        catch (Exception e_)
                        {
                            testStatus.Error();
                            Console.WriteLine($"{e_.Message} {e_.StackTrace}");
                        }
                    }
                });
            }
        }

    }
}
