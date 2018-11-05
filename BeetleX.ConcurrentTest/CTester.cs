using System;
using System.Collections.Generic;
using System.Reflection;
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
            AddRegion(1000, 5000);
            AddRegion(5000, 10000);
            statisticTags.Add(new StatisticTag("Success"));
            statisticTags.Add(new StatisticTag("Error"));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("***********************************************************************");
            sb.AppendLine($"* https://github.com/IKende/ConcurrentTest.git");
            sb.AppendLine($"* Copyright © ikende.com 2018 email:henryfan@msn.com");
            sb.AppendLine($"* ServerGC:{System.Runtime.GCSettings.IsServerGC}");
            Console.Write(sb.ToString());
        }

        private System.Diagnostics.Stopwatch mStopwatch;

        private List<StatisticTag> statisticTags = new List<StatisticTag>();

        private List<TimeRegion> mRegions = new List<TimeRegion>();

        public IList<TimeRegion> TimeRegions => mRegions;

        public IList<StatisticTag> Tags => statisticTags;

        private int mOffsetTop = -1;

        private int mLastTop;

        private string mTestName;

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
            if (mCount > 0)
            {
                string total = $"[{mRuns}/{mCount}]";
                sb.AppendLine($"*{total.PadLeft(36)}|threads:[{mThreads}]");
            }
            for (int i = 0; i < statisticTags.Count; i++)
            {
                sb.AppendLine($"* {statisticTags[i].Report(last)}");
            }
            sb.AppendLine("-----------------------------------------------------------------------");
            for (int i = 0; i < mRegions.Count; i = i + 2)
            {
                sb.Append($"* {mRegions[i]}");
                if (i + 1 < mRegions.Count)
                    sb.Append($"{mRegions[i + 1]}");
                sb.Append("\r\n");
            }
            sb.AppendLine("***********************************************************************");
            sb.AppendLine("");
            return sb.ToString();
        }

        public string Report()
        {
            if ((mCount == -1 || mRuns < mCount))
                return OnReport();
            return OnReport(true);
        }

        public CTester ReportToConsole()
        {
            mLastTop = Console.CursorTop;
            mOffsetTop = 1;
            while ((mCount == -1 || mRuns < mCount))
            {
                if (mOffsetTop < 1)
                {
                    Console.CursorTop += mOffsetTop;
                    Console.CursorLeft = 0;
                }
                System.Threading.Thread.Sleep(1000);
                Console.Write(Report());
                if (mOffsetTop == 1)
                {
                    mOffsetTop = mLastTop - Console.CursorTop;
                }
            }
            System.Threading.Thread.Sleep(1000);
            if (mOffsetTop < 1)
            {
                Console.CursorTop += mOffsetTop;
                Console.CursorLeft = 0;
            }
            Console.Write(Report());
            return this;
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


        private int mPreppingThreads;

        private void OnPrepare(Action action)
        {
            Console.WriteLine("***********************************************************************");
            mOffsetTop = 1;
            mLastTop = Console.CursorTop;
            for (int i = 0; i < 10; i++)
                action();
            mPreppingThreads = mThreads;
            for (int i = 0; i < mThreads; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(o =>
                {
                    double time = mStopwatch.Elapsed.TotalSeconds;
                    try
                    {
                        while (true)
                        {
                            action();
                            if (mStopwatch.Elapsed.TotalSeconds - time > 5)
                                break;
                        }
                    }
                    catch (Exception e_)
                    {
                        Console.WriteLine(e_.Message);
                    }
                    finally
                    {
                        System.Threading.Interlocked.Decrement(ref mPreppingThreads);
                    }
                });
            }

            int count = 1;
            string wait = ".";
            while (true)
            {
                if (mOffsetTop < 1)
                {
                    Console.CursorTop += mOffsetTop;
                    Console.CursorLeft = 0;
                }
                Console.Write($"* {mTestName} test prepping {wait.PadRight(count % 10, '.')}");
                if (mOffsetTop == 1)
                {
                    mOffsetTop = mLastTop - Console.CursorTop;
                }
                System.Threading.Thread.Sleep(200);
                count++;
                if (mOffsetTop < 1)
                {
                    Console.CursorTop += mOffsetTop;
                    Console.CursorLeft = 0;
                }
                Console.Write($"* prepping {wait.PadRight(count % 10)}");
                if (mPreppingThreads < 1)
                    break;
            }
            if (mOffsetTop >= 0)
            {
                Console.CursorTop += mOffsetTop;
                Console.CursorLeft = 0;
            }
            Console.Write($"* {mTestName} test prepping completed");
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------------------");
            System.Threading.Thread.Sleep(3000);
        }

        public CTester Run(int thread, Action action, uint count, string testName)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            mCount = count;
            mThreads = thread;
            mRuns = 0;
            this.mTestName = testName;
            foreach (var t in Tags)
                t.Clear();
            foreach (var t in TimeRegions)
                t.Clear();
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

        public static void RunTest<T>(int threads, uint number) where T : new()
        {
            CTester cTester = new CTester();
            cTester.Run<T>(threads, number);
        }

        public void Run<T>(int threads, uint number) where T : new()
        {
            Type type = typeof(T);
            Object obj = new T();
            List<CTestCase> descriptions = new List<CTestCase>();
            foreach (MethodInfo m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                CTestCase item = m.GetCustomAttribute<CTestCase>();
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.Name))
                        item.Name = m.Name;
                    item.CaseObject = obj;
                    item.Action = (Action)Delegate.CreateDelegate(typeof(Action), obj, m, false);
                    descriptions.Add(item);
                }
            }
            if (descriptions.Count > 0)
            {
                foreach (var item in descriptions)
                {
                    Run(threads, item.Action, (uint)number, item.Name).ReportToConsole();
                }
            }
            else
            {
                Console.WriteLine($"* {type} No test cases exist!");
            }
        }

    }
}
