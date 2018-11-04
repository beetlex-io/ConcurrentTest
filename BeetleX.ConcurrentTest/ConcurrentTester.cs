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
            AddRegion(1000, 10000);
            statisticTags.Add(new StatisticTag("Success"));
            statisticTags.Add(new StatisticTag("Error"));
        }

        private System.Diagnostics.Stopwatch mStopwatch;

        private List<StatisticTag> statisticTags = new List<StatisticTag>();

        private List<TimeRegion> mRegions = new List<TimeRegion>();

        public IList<TimeRegion> TimeRegions => mRegions;

        public IList<StatisticTag> Tags => statisticTags;

        private int mThreads;

        private long mCount;

        private long mRuns;

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

        public string OnReport(bool last = false)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"****************ConcurrentTest[{DateTime.Now}]*********************");
            if (mCount > 0)
            {
                sb.AppendLine($"*{mRuns.ToString().PadLeft(28)}/{mCount}|threads:{mThreads}");
            }
            for (int i = 0; i < statisticTags.Count; i++)
            {
                sb.AppendLine($"* {statisticTags[i].Report(last)}");
            }
            sb.AppendLine("-----------------------------------------------------------------------");
            for (int i = 0; i < mRegions.Count; i++)
            {
                sb.AppendLine($"* {mRegions[i]}");
            }
            sb.AppendLine("***********************************************************************");
            return sb.ToString();
        }

        public string Report()
        {
            if ((mCount == -1 || mRuns < mCount))
                return OnReport();
            return OnReport(true);
        }

        public void ReportToConsole()
        {
            while ((mCount == -1 || mRuns < mCount))
            {
                Console.CursorTop = 5;
                Console.CursorLeft = 0;
                System.Threading.Thread.Sleep(1000);
                Console.Write(Report());
            }
            System.Threading.Thread.Sleep(1000);
            Console.CursorTop = 5;
            Console.CursorLeft = 0;
            Console.Write(Report());
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

        private void OnPrepare(Action action)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("***********************************************************************");
            sb.AppendLine($"* https://github.com/IKende/ConcurrentTest.git");
            sb.AppendLine($"* Copyright © ikende.com 2018 email:henryfan@msn.com");
            sb.AppendLine($"* ServerGC:{System.Runtime.GCSettings.IsServerGC}");
            Console.Write(sb.ToString());
            string wait = ".";
            for (int k = 0; k < 100; k++)
            {
                Console.CursorTop = 4;
                Console.CursorLeft = 0;
                Console.Write($"* prepping {wait.PadRight(k % 50, '.')}");
                for (int i = 0; i < 10; i++)
                {
                    action();
                }
                System.Threading.Thread.Sleep(200);
                Console.CursorTop = 4;
                Console.CursorLeft = 0;
                Console.Write($"* prepping {wait.PadRight(k % 50)}");
            }
            Console.CursorTop = 4;
            Console.CursorLeft = 0;
            Console.Write($"* prepping completed");
            Console.WriteLine("");
        }

        public CTester Run(int thread, Action action, long count = -1)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            mCount = count;
            mThreads = thread;
            mRuns = 0;
            Console.Clear();
            OnPrepare(action);
            for (int i = 0; i < thread; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        long runs = System.Threading.Interlocked.Increment(ref mRuns);
                        if (runs > mCount)
                            System.Threading.Interlocked.Decrement(ref mRuns);
                        if (mCount == -1 || runs <= mCount)
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
                        else
                            break;
                    }
                });
            }
            return this;
        }

    }
}
